using Nebula.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using System.Data.Entity;
using Nebula.Domain.ViewModels.Web.Geo;
using Nebula.Domain.Entities;
using Nebula.Services.Utils;

namespace Nebula.Services.Services.Web.Geo
{
    public class GeoExamService
    {
        private readonly GeneralRepository _generalRepository;

        public GeoExamService(GeneralRepository generalRepository)
        {
            _generalRepository = generalRepository;
        }


        public GeoUserExam GetLastExam(string userId)
        {


            var lastExam = _generalRepository
                .GeoUserExam.Set()
                .Where(m => m.UserId == userId && !m.IsFinished)
                .Include(m => m.Poetry.Questions.Select(a => a.Answers))
                .Include(m => m.Prose.Questions.Select(a => a.Answers))
                .Include(m => m.GeoTextEditing)
                .Include(m => m.GeoEssay)
                .Include(m => m.User.GeoAnswers.Select(a => a.Answer))
                .FirstOrDefault();

            if (lastExam != null)
            {
                //timeout
                if (lastExam.Timeout())
                {
                    lastExam.IsFinished = true;
                    _generalRepository.GeoUserExam.Save(lastExam);
                    return null;
                }

            }
            return lastExam;
        }

        public GeoUserExam Init(string userId, out bool continueExam)
        {
            var lastExam = GetLastExam(userId);
            if (lastExam == null)
            {
                continueExam = false;
                return InitExam(userId);
            }

            continueExam = true;
            return lastExam;
        }

        public GeoUserExam InitExam(string userId)
        {


            //for exclude 
            var finishedExams = _generalRepository
                .GeoUserExam.Set().Where(m => m.UserId == userId && m.IsFinished);
                 
            var textEditing = _generalRepository.GeoTextEditing.Set()
          
                .Where(m => !finishedExams.Any(f => f.GeoTextEditingId == m.Id))
                .OrderBy(m => Guid.NewGuid())
                .FirstOrDefault();

            var essey = _generalRepository.GeoEssay.Set() 
                .Where(m => !finishedExams.Any(f => f.GeoEssayId == m.Id))
                .OrderBy(m => Guid.NewGuid())
                .FirstOrDefault();

            var poetry = _generalRepository.GeoFictionText.Set()
                 .Include(m => m.Questions.Select(a => a.Answers)) 
                .Where(m => !finishedExams.Any(f => f.PoetryId == m.Id) && m.ThemeType == GeoThemeType.Poetry)
                .OrderBy(m => Guid.NewGuid())
                .FirstOrDefault();

            var prose = _generalRepository.GeoFictionText.Set()
                  .Include(m => m.Questions.Select(a => a.Answers))
                .Where(m => !finishedExams.Any(f => f.ProseId == m.Id) && m.ThemeType == GeoThemeType.Prose)
                 .OrderBy(m => Guid.NewGuid())
                 .FirstOrDefault();

            if (textEditing == null || essey == null || poetry == null || prose == null)
                throw new Exception("Resourses is limited");

            var geoExam = new GeoUserExam
            {
                UserId = userId,
                GeoTextEditingId = textEditing.Id,
                ChoosenTheme = GeoUserChoosenTheme.None,
                GeoEssayId = essey.Id,
                StartTime = DateTime.Now,
                PoetryId = poetry.Id,
                ProseId = prose.Id
            };

            _generalRepository.GeoUserExam.Save(geoExam);

            return _generalRepository
                      .GeoUserExam.Set()
                      .Where(m => m.Id == geoExam.Id)
                      .Include(m => m.Poetry.Questions.Select(a => a.Answers))
                      .Include(m => m.Prose.Questions.Select(a => a.Answers))
                      .Include(m => m.GeoTextEditing)
                      .Include(m => m.GeoEssay)
                      .Include(m => m.User.GeoAnswers.Select(a => a.Answer))
                      .FirstOrDefault();
        }



        public void SaveEssey(int examId, string text)
        {
            var userId = HttpContext.Current.User.Identity.GetUserId();
            var exam = _generalRepository.GeoUserExam.Fetch(examId);
            if (exam.UserId != userId)
                throw new Exception("User session end");

            if (exam.Timeout())
            {
                exam.IsFinished = true;
                _generalRepository.GeoUserExam.Save(exam);
                throw new Exception("Exam session end");
            }
            exam.Essay = text;
            _generalRepository.GeoUserExam.Save(exam);
        }


        public void SaveTheme(int examId, string text) 
        {
            var userId = HttpContext.Current.User.Identity.GetUserId();
            var exam = _generalRepository.GeoUserExam.Fetch(examId);
            if (exam.UserId != userId)
                throw new Exception("User session end");
            if (exam.Timeout()) {
                exam.IsFinished = true;
                _generalRepository.GeoUserExam.Save(exam);
                throw new Exception("Exam session end");
            }
            exam.UserTheme = text;
            _generalRepository.GeoUserExam.Save(exam);
        }


        public void SaveTextEditing(int examId, string text)
        {
            var userId = HttpContext.Current.User.Identity.GetUserId();
            var exam = _generalRepository.GeoUserExam.Fetch(examId);
            if (exam.UserId != userId)
                throw new Exception("User session end");

            if (exam.Timeout())
            {
                exam.IsFinished = true;
                _generalRepository.GeoUserExam.Save(exam);
                throw new Exception("Exam session end");
            }
           

            exam.TextEditing = text;
            _generalRepository.GeoUserExam.Save(exam);
        }

        public void SaveAnswer(int answerId, int questionId, int examId)
        {
            var userId = HttpContext.Current.User.Identity.GetUserId();
            var exam = _generalRepository.GeoUserExam.Fetch(examId);

            if (exam.UserId != userId)
                throw new Exception("User session exception");

            if (exam.Timeout() || exam.IsFinished)
                throw new Exception("Exam is timeout");

            var userAnswer = _generalRepository.GeoUserAnswer.Set()
                .SingleOrDefault(m => m.Answer.QuestionId == questionId && m.UserId == userId);

            if (userAnswer == null)
            {
                _generalRepository.GeoUserAnswer.Save(new GeoUserAnswer
                {
                    AnswerId = answerId,
                    UserId = userId,
                    AnswerDate = DateTime.Now
                });
            }
            else
            {
                userAnswer.AnswerId = answerId;
                _generalRepository.GeoUserAnswer._context.SaveChanges();
            }
        }


        public void GameOver(GeoFinishedExamViewModel model)
        {
            var userId = HttpContext.Current.User.Identity.GetUserId();
            var exam = _generalRepository.GeoUserExam.Fetch(model.ExamId);
            if(exam.UserId != userId)
                throw new Exception("User session end");
            if (exam.Timeout())
            {
                exam.IsFinished = true;
                _generalRepository.GeoUserExam.Save(exam);
                throw new Exception("Exam session end");
            }

            exam.TextEditing = model.TextEditing;
            exam.UserTheme = model.UserTheme;

            exam.Essay = model.Essay;
            exam.IsFinished = true;
            exam.EndTime = DateTime.Now;
            _generalRepository.GeoUserExam.Save(exam);
        }
         
        public int GetExamsCount(string userId) 
            {
            return _generalRepository.GeoUserExam.Set().Where(m => m.UserId == userId).Count();
        }

        public List<Tuple<int, int>> GetUserAnswerIds(int examId)
        {
            return _generalRepository.GeoUserAnswer.Set()
                .Include(m => m.User.GeoExams)
                .Where(m => m.User.GeoExams.Any(x => x.Id == examId))
                .Select(m => new Tuple<int, int>(m.AnswerId, m.Answer.QuestionId)).ToList();     
        }

        public void Report()
        {
            var user1Id = "21cc14ba-be6f-4d70-b057-41d69b4a1e46"; //გოგოლაძე
            var user2Id = "4429344d-9d4d-422e-ba16-d626b563a64d"; // Tika Dzlierishvili

            var exams = _generalRepository
                
                .GeoUserExam               
                .Set()
                .Include(m => m.Prose.Questions.Select(x => x.Answers))
                .Include(m => m.Poetry.Questions.Select(x => x.Answers))
                .Where(m => (m.UserId == user1Id || m.UserId == user2Id) && (!string.IsNullOrEmpty(m.TextEditing) || !string.IsNullOrEmpty(m.Essay) || 
                !string.IsNullOrEmpty(m.UserTheme)))
                .ToList();
            foreach (var exam in exams)
            {
              
                int proseCorrectAnswersCount = _generalRepository.GeoUserAnswer.Set()
                    .Where(m => m.Answer.Question.GeoFictionTextId == exam.ProseId && m.UserId == exam.UserId ).Count(m => m.Answer.IsCorrect);

                int poetryCorrectAnswersCount = _generalRepository.GeoUserAnswer.Set()
                    .Where(m => m.Answer.Question.GeoFictionTextId == exam.PoetryId && m.UserId == exam.UserId).Count(m => m.Answer.IsCorrect);
                
            }
        }
    }
}
