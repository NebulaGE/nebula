using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Nebula.Domain.Abstract;
using Nebula.Domain.Concrete;
using Nebula.Domain.olddb;
using Nebula.Domain.Entities;

namespace Nebula.Services.Services
{
    public class RestoreDatabase
    {
        public void FillCouponTypes()
        {
            var _db = new NebulaDbContext();

            var coupons = _db.Cupons.ToList();

            foreach (var item in coupons)
            {
                item.Type = CouponType.Cs;
            }

            _db.SaveChanges();
        }

        public void FillBookCodes()
        {
            var _db = new NebulaDbContext();

            for (int i = 0; i < 3200; i++)
            {
                _db.BookCodes.Add(new BookCode()
                {
                    Code = Guid.NewGuid().ToString()

                });
            }
        }

        public void ReturnMathBookQuestions()
        {
            var questioInfoIdToXclude = new List<int> { 47, 48, 49 };

            var _db = new NebulaDbContext();

            var questions = _db.CSQuestions.Include(x => x.Module)
                .Where(x => x.ModuleId != null
                && x.Module.CSQuestionCategoryId == 2
                && x.ConditionId == null
                && x.IsNaecQuestion == false).ToList();

            foreach (var q in questions)
            {
                q.IsBookQuestion = !questioInfoIdToXclude.Any(x => x == q.CSQuestionInfoId);
            }

            _db.SaveChanges();
        }

        public void MoveBookQuestions()
        {
            var questioInfoIdToXclude = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20 };
            //var questioInfoIdToXcludeFromExercises = new List<int> { 1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,33,34,35,36,37,38,39,40,41,42,43,44 };

            var _db = new NebulaDbContext();

            var questions = _db.CSQuestions.ToList();

            foreach (var q in questions)
            {
                if (questioInfoIdToXclude.Any(x => x == q.CSQuestionInfoId))
                {
                    q.IsBookQuestion = true;
                }
                //else if (questioInfoIdToXcludeFromExercises.Any(x=>x==q.CSQuestionInfoId))
                //{
                //    q.IsNaecQuestion = true;
                //}
            }

            _db.SaveChanges();
        }

        public void RemoveDuplicateUserAnswers()
        {
            NebulaDbContext _db = new NebulaDbContext();
            var answers = _db.GeoUserAnswers
                .Include(m => m.Answer)
                .GroupBy(m => new
                    {
                        m.UserId,
                        m.Answer.QuestionId    
                    })
                .Select(m => new
                {
                    count = m.Count(),
                    questionId = m.Key.QuestionId,
                    userId =  m.Key.UserId,
                    answers = m.Select(x => new
                         {
                           answerId = x.AnswerId,
                           date = x.AnswerDate
                        }).OrderByDescending(x => x.date).ToList()
                })
                .ToList();

            var duplicateAnswers = answers.Where(m => m.count > 1).ToList();

            foreach (var item in duplicateAnswers)
            {
                foreach (var answerId in item.answers.Skip(1).ToList())
                {
                    var userAnswer =
                        _db.GeoUserAnswers.Where(m => m.UserId == item.userId && m.AnswerId == answerId.answerId)
                            .First();
                    _db.GeoUserAnswers.Remove(userAnswer);
                }
                

            }
            _db.SaveChanges();

        }


        
        public void CorrectLicenses()
        {
            NebulaDbContext _db = new NebulaDbContext();

            var users = _db.Users.ToList();

            foreach (var user in users)
            {
                user.HasGeoLicense = false;
                user.HasAllLicense = false;
                user.HasEngLicense = false;

                if (user.Email == "adminnebula@gmail.com")
                {
                    user.HasAllLicense = true;
                }
            }

            _db.SaveChanges();
        }

        public void CorrectVideoEmbed()
        {

            IDbContextNebula _db = new NebulaDbContext();

            var videos = _db.GeoVideos.ToList();

            foreach (var video in videos)
            {
                video.FileLink = video.FileLink.Replace(@"width=""640""", @"width=""810""").Replace(@"height=""480""", @"height=""410""").Replace(@"height=""360""", @"height=""410""");
            }

            _db.SaveChanges();
        }

        public void FillLostData()
        {
            IDbContextNebula _db = new NebulaDbContext();
            var questions = _db.CSQuestions.ToList();
            var tasks = _db.Conditions.ToList();

            using (var oldEnt = new nebulaEntities())
            {
                foreach (var oldQuestion in oldEnt.CSQuestionsOld)
                {
                    var currentQuestion = questions.SingleOrDefault(m => m.Id == oldQuestion.Id);
                    if (currentQuestion != null)
                    {
                        if (oldQuestion.IsExamQuestion)
                        {
                            currentQuestion.QuestionType = CSQuestionType.ExamQuestion;
                        }
                        if (oldQuestion.IsExerciseQuestion)
                        {
                            currentQuestion.QuestionType = CSQuestionType.ExerciseQuestion;
                        }
                        if (oldQuestion.IsVideoQuestion)
                        {
                            currentQuestion.QuestionType = CSQuestionType.VideQuestion;
                        }
                        _db.SaveChanges();
                        // _generalRepository.CsQuestion.Save(currentQuestion);
                    }

                }

                foreach (var oldTask in oldEnt.TaskWithMultipleQuestionsOld)
                {
                    var currentTask = tasks.SingleOrDefault(m => m.Id == oldTask.Id);
                    if (currentTask != null)
                    {
                        if (oldTask.IsExamPiroba)
                        {
                            currentTask.Type = TaskType.IsExamTask;
                        }
                        if (oldTask.IsExercisePiroba)
                        {
                            currentTask.Type = TaskType.IsExerciseTask;
                        }
                        if (oldTask.IsVideoPiroba)
                        {
                            currentTask.Type = TaskType.IsVideoTask;
                        }
                        _db.SaveChanges();
                    }
                }
            }
        }

        public void MakeCorrectionInFinishedTasks()
        {
            NebulaDbContext _db = new NebulaDbContext();

            var finishedTasks = _db.FinishedTasks
                .Include(m => m.Task)
                .Where(m => m.Task.Type != TaskType.IsExerciseTask).ToList();

            foreach (var f in finishedTasks)
            {
                _db.FinishedTasks.Remove(f);
            }


            _db.SaveChanges();

            var grouppedTasks = _db.FinishedTasks
                .Where(m => m.UserId != null)
                .GroupBy(m => new
                {
                    m.UserId,
                    m.TaskId
                }).ToList();

            foreach (var grouppedTask in grouppedTasks)
            {
                var delete = grouppedTask.Any(m => m.IsFinished);
                if (delete)
                {
                    foreach (var notFinished in grouppedTask.Where(x => !x.IsFinished))
                    {
                        _db.FinishedTasks.Remove(notFinished);
                    }
                }
            }

            _db.SaveChanges();
        }
    }
}
