using Nebula.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Text;

using Nebula.Web.Models.BusinessLogic;
using Microsoft.AspNet.Identity;
using Nebula.Domain.Abstract;
using Nebula.Domain.Entities;

namespace Nebula.Web.Controllers
{

    public class TeacherController : Controller
    {

        private readonly BaseRepository<Teacher> _teacherRepo;
        private readonly BaseRepository<TeacherStudent> _teacherStudentRepo;
        private readonly BaseRepository<CSUserQuizzes> _userQuizzRepo;
        private readonly BaseRepository<Module> _moduleRepo;
        private readonly BaseRepository<User> _userRepo;
        private readonly BaseRepository<Notification> _notificationRepo;

        public TeacherController(
            BaseRepository<Teacher> teacherRepo, 
            BaseRepository<TeacherStudent> teacherStudentRepo, 
            BaseRepository<CSUserQuizzes> userQuizzRepo, 
            BaseRepository<Module> moduleRepo, 
            BaseRepository<User> userRepo, 
            BaseRepository<Notification> notificationRepo)
        {
            _teacherRepo = teacherRepo;
            _teacherStudentRepo = teacherStudentRepo;
            _userQuizzRepo = userQuizzRepo;
            _moduleRepo = moduleRepo;
            _userRepo = userRepo;
            _notificationRepo = notificationRepo;
        }

        // GET: Teacher

        public ActionResult Login()
        {
            if (GlobalClass.IsAuthTeacher)
                return RedirectToAction("MyStudents");

            return View();
        }

        [HttpPost]
        public JsonResult Login(Teacher teacher)
        {
            try
            {
                var teach = _teacherRepo.Set().SingleOrDefault(m => 
                    m.FirstName.ToLower() == teacher.FirstName.ToLower() 
                    && m.LastName.ToLower() == teacher.LastName.ToLower() &&
                    m.Password == teacher.Password);

                if (teach == null)
                {
                    return Json(new
                    {
                        error = "სახელი,გვარი ან პაროლი არასწორია"
                    });

                }
                Session["Teacher"] = teach.Id;

                return Json(new
                {
                    success = true
                });
            }
            catch (Exception)
            {
                return Json(new
                {
                    error = "მოხდა სისტემური შეცდომა"
                });
            }
        }

        [HttpPost]
        public JsonResult Register(Teacher teacher)
        {
            try
            {
                if (!string.IsNullOrEmpty(teacher.Email))
                {
                    if (_teacherRepo.Set().Any(m => m.Email.ToLower() == teacher.Email.ToLower()))
                    {

                        return Json(new { error = "ეს ელ.ფოსტა უკვე გამოყენებულია" });
                    }

                }

                if (_teacherRepo.Set().Any(m => m.IdentificationNumber == teacher.IdentificationNumber))
                {
                    return Json(new { error = "ეს პირადი ნომერი უკვე გამოყენებულია" });
                }
                 
                var teach = new Teacher
                {
                    Email = teacher.Email,
                    FirstName = teacher.FirstName,
                    LastName = teacher.LastName,
                    Password = teacher.Password,
                    PhoneNumber = teacher.PhoneNumber,
                    IdentificationNumber = teacher.IdentificationNumber
                };
                _teacherRepo.Save(teach); 

                Session["Teacher"] = teach.Id;

                return Json(new { success = true });
            }
            catch (Exception)
            {
                return Json(new { error = "მოხდა სისტემური შეცდომა, გთხოვთ მოგვიანებით სცადოთ რეგისტრაცია" });
            }
         
        }



  

        [Nebula.Web.Filters.TeacherAuthorize]
        public ActionResult MyStudents()
        {
         
            int teacherId = int.Parse(Session["Teacher"].ToString());

            var model = _teacherStudentRepo.Set().Where(m => m.TeacherId == teacherId && m.IsConfirmed)
                .Include(m => m.Student);
           
            
         
            var userIds = new List<string>();
            userIds.AddRange(model.Select(m => m.StudentId));

            var finishedQuizzes = _userQuizzRepo.Set().Where(m => m.QuizConfirmed == true && userIds.Any(ui => ui == m.UserId))                              
                                  .Include(m => m.Quiz.VerbalQuizzes.Select(q => q.Question.Module))
                                  .Include(m => m.Quiz.MathQuizzes.Select(q => q.Question.Module)).ToList();

            var viewModel = new List<MyStudentsViewModel>();
            var quizHelper = QuizHelper.GetQuiz(_userQuizzRepo._context);
            int takeCount = GlobalClass.ExamCoefficients.Count();
            var module = _moduleRepo.Set().ToList();

            foreach (var user in model)
            {
                var quizzes = finishedQuizzes.Where(m => m.UserId == user.StudentId)
                     .OrderByDescending(m => m.VerbalQuizStartDate)
                    .Take(takeCount).ToList();

                var lastExam = quizzes.FirstOrDefault();

                viewModel.Add(new MyStudentsViewModel {
                    UserId =  user.StudentId,
                    FullName = user.Student.FirstName + " " + user.Student.LastName,
                    IdentificationNumber = user.Student.IdentificationNumber,
                    ExcpectedScore = !quizzes.Any() ? (int)Math.Round(quizHelper.CalculateModulesPercent(quizzes, module).excpectedScore, 0) : 0,
                    LastExamScore = lastExam?.Quiz.VerbalQuizzes.Sum(m => m.IsCorrect ? 1 : 0) + lastExam.Quiz.MathQuizzes.Sum(m => m.IsCorrect ? 1 : 0) ?? 0
                });
            }

            return View(viewModel);
        }


        [HttpPost]
        [Authorize]
        public JsonResult Confirm(int teacherId)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var teacherStudent = _teacherStudentRepo.Set().SingleOrDefault(m => m.StudentId == userId && m.TeacherId == teacherId);

                if (teacherStudent == null)
                    return Json(new
                    {
                        error = true
                    });
                teacherStudent.IsConfirmed = true;
                _teacherStudentRepo.Save(teacherStudent);
                return Json(new
                {
                    success = true
                });
            }
            catch (Exception)
            {
                return Json(new
                {
                    error = true
                });
            }
        
        }


        [HttpPost]
        [Authorize]
        public JsonResult Remove(int teacherId)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var teacherStudent = _teacherStudentRepo.Set()
                    .SingleOrDefault(m => m.StudentId == userId && m.TeacherId == teacherId);
                if (teacherStudent == null)
                    return Json(new
                    {
                        error = true
                    });
                _teacherStudentRepo.Delete(teacherStudent); 

                return Json(new
                {
                    success = true
                });
            }
            catch (Exception)
            {
                return Json(new
                {
                    error = true
                });
            }

        }

        [HttpPost]
        public ActionResult Logout()
        {
            Session["Teacher"] = null;
            return RedirectToAction("Login");
        } 
    }

    #region helper classes
    public class MyStudentsViewModel
    {
        public string UserId { get; set; }
        public string FullName { get; set; }
        public string IdentificationNumber { get; set; }
        public int ExcpectedScore { get; set; }
        public int LastExamScore { get; set; }
    }
    #endregion
}