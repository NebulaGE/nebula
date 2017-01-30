using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Web.Mvc;
using Nebula.Domain.Abstract;
using Nebula.Domain.Entities;
using System.Linq;
using Nebula.Services.Json;
using Nebula.Services.Dto;
using Nebula.Services.Utils;

namespace Nebula.Web.Controllers.Geo
{
    public class GeoVideoController : BaseController
    {
        private readonly BaseRepository<User> _userRepo;
        private readonly BaseRepository<GeoGrammarTag> _tagRepo;
        private readonly BaseRepository<GeoGrammarSubTag> _subTagRepo;
        private readonly BaseRepository<GeoVideo> _videoRepo;
        private readonly BaseRepository<GeoUserAnswer> _userAnswerRepo;
        private readonly BaseRepository<GeoQuestion> _questionRepo;
        private readonly BaseRepository<GeoAnswer> _answerRepo;
        private readonly BaseRepository<GeoAuthor> _authorRepo;
        private readonly BaseRepository<GeoAuthorWork> _workRepo;
        private readonly BaseRepository<GeoAuthorWorkPart> _workPartRepo;
        public GeoVideoController(
            BaseRepository<User> userRepo,
            BaseRepository<GeoGrammarTag> tagRepo,
            BaseRepository<GeoVideo> videoRepo,
            BaseRepository<GeoGrammarSubTag> subTagRepo,
            BaseRepository<GeoUserAnswer> geoUserAnswerRepo,
            BaseRepository<GeoQuestion> geoQuestionRepo,
            BaseRepository<GeoAnswer> answerRepo,
            BaseRepository<GeoAuthor> authorRepo,
            BaseRepository<GeoAuthorWork> workRepo,
            BaseRepository<GeoAuthorWorkPart> workPartRepo)
        {
            _userRepo = userRepo;
            _tagRepo = tagRepo;
            _videoRepo = videoRepo;
            _subTagRepo = subTagRepo;
            _userAnswerRepo = geoUserAnswerRepo;
            _questionRepo = geoQuestionRepo;
            _answerRepo = answerRepo;
            _authorRepo = authorRepo;
            _workRepo = workRepo;
            _workPartRepo = workPartRepo;
        }

        public ActionResult Index(byte category, int tagId)
        {
            var user = _userRepo.Fetch(_currentUserId);

            ViewBag.CatId = category;

            if (category == 1)
            {
                var tag = _tagRepo.Fetch(tagId);

                if (!tag.IsPayed)
                    return View();
            }
            else
            {
                var author = _authorRepo.Fetch(tagId);

                if (!author.IsPayed)
                    return View();
            }

            if (!user.HasGeoLicense)
                return RedirectToAction("Index", "Payment");

            return View();
        }

        public ActionResult Literature(int authorWorkId)
        {
            var user = _userRepo.Fetch(_currentUserId);

            ViewBag.NeedsLicense = UserLicenseCheckerHelper.CheckGeoLicense(_userRepo, _currentUserId);

            var author = _authorRepo.Fetch(authorWorkId);

            if (!author.IsPayed)
                return View();

            if (!user.HasGeoLicense)
                return RedirectToAction("Index", "Payment");

            return View();
        }

        public ActionResult Grammar(int tagId)
        {
            var user = _userRepo.Fetch(_currentUserId);

            ViewBag.NeedsLicense = UserLicenseCheckerHelper.CheckGeoLicense(_userRepo, _currentUserId);

            var tag = _tagRepo.Fetch(tagId);

            if (!tag.IsPayed)
                return View();

            if (!user.HasGeoLicense)
                return RedirectToAction("Index", "Payment");

            return View();
        }

        public JsonResult GetLiteratureNavigation()
        {
            var user = _userRepo.Fetch(_currentUserId);

            var hasLicense = user != null && user.HasGeoLicense;

            var authorsWithWorks = _authorRepo.Set()
                .OrderByDescending(m => !m.IsPayed)
                .Include(a => a.GeoAuthorWorks.Select(y => y.GeoAuthorWorkParts))
                .Where(a => a.GeoAuthorWorks.Any(x => x.GeoAuthorWorkParts.Any(b => (b.Questions.Count(c => c.QuestionType == GeoQuestionType.PreQuestion) > 0
                 && b.Questions.Count(c => c.QuestionType == GeoQuestionType.PostQuestion) > 0 && b.GeoVideos.Any()) /*|| x.Title.StartsWith("1")*/))).Select(a => new
                 {
                     id = a.Id,
                     isPayed = a.IsPayed,
                     hasLicense = hasLicense,
                     needLicense = a.IsPayed && !hasLicense,
                     name = a.AuthorName,
                     works = a.GeoAuthorWorks.Where(x => x.GeoAuthorWorkParts.Any(b => (b.Questions.Count(c => c.QuestionType == GeoQuestionType.PreQuestion) > 0
                           && b.Questions.Count(c => c.QuestionType == GeoQuestionType.PostQuestion) > 0
                           && b.GeoVideos.Any()) /*|| x.Title.StartsWith("1")*/))
                     .Select(b => new
                     {
                         name = b.Title,
                         id = b.Id
                     })
                 });

            return Json(new { authorsWithWorks }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetLiteratureWorksWithParts(int workId)
        {
            var user = _userRepo.Fetch(_currentUserId);

            var work = _workRepo.Set()
                .Include(a => a.Author)
                .Include(a => a.GeoAuthorWorkParts.Select(b => b.Questions.Select(c => c.Answers)))
                .Include(a => a.GeoAuthorWorkParts.Select(b => b.GeoVideos))
                .FirstOrDefault(a => a.Id == workId);

            if (user == null && work.Author.IsPayed)
                return NeedLicenseJson();
            if (work.Author.IsPayed && !user.HasGeoLicense)
                return NeedLicenseJson();

            var userAnswers = _userAnswerRepo.Set()
                 .Include(a => a.Answer.Question.Answers)
                 .Include(a => a.Answer.Question.GeoAuthorWorkPart)
                .Where(m => m.UserId == _currentUserId &&
                m.Answer.Question.GeoAuthorWorkPart.AuthorWorkId == workId).ToList();

            return Json(new
            {
                workParts = work.GeoAuthorWorkParts
                .Where(m => m.GeoVideos.Any()
                 && m.Questions.Any(q => q.QuestionType == GeoQuestionType.PostQuestion)
                 && m.Questions.Any(q => q.QuestionType == GeoQuestionType.PreQuestion))
                .Select(a => new
                {
                    deleteId = a.Id,

                    preQuestions = a.Questions
                    .Where(b => b.QuestionType == GeoQuestionType.PreQuestion).OrderBy(m => m.Id)
                    .Select(b => new ExerciseQuestionJson
                    {
                        id = b.Id,
                        text = b.Text,
                        answers = b.Answers.Select(c => new ExerciseAnswerJson
                        {
                            id = c.Id,
                            text = c.Text
                        }).ToList(),
                    }),

                    postQuestions = a.Questions
                    .Where(b => b.QuestionType == GeoQuestionType.PostQuestion)
                    .OrderBy(m => m.Id)
                      .Select(b => new ExerciseQuestionJson
                      {
                          id = b.Id,
                          text = b.Text,
                          answers = b.Answers.Select(c => new ExerciseAnswerJson
                          {
                              id = c.Id,
                              text = c.Text
                          }).ToList(),
                      }),

                    video = a.GeoVideos.Where(b => b.GeoAuthorWorkPartId == a.Id).Select(b => new
                    {
                        name = b.Name,
                        id = b.Id,
                        link = b.FileLink,
                        workPartId = b.GeoAuthorWorkPartId
                    }).FirstOrDefault(),

                    postQuestionAnswers = userAnswers.Where(m =>
                        m.Answer.Question.QuestionType == GeoQuestionType.PostQuestion
                         && m.Answer.Question.GeoAuthorWorkPartId == a.Id)
                            .Select(x => new ExerciseUserAnswerJson
                            {
                                answerId = x.AnswerId,
                                questionId = x.Answer.QuestionId,
                                correctAnswerId = x.Answer.Question.Answers.FirstOrDefault(q => q.IsCorrect).Id
                            }).OrderBy(m => m.questionId).ToList(),

                    preQuestionAnswers = userAnswers.Where(m =>
                          m.Answer.Question.QuestionType == GeoQuestionType.PreQuestion &&
                          m.Answer.Question.GeoAuthorWorkPartId == a.Id)
                    .Select(x => new ExerciseUserAnswerJson
                    {
                        answerId = x.AnswerId,
                        questionId = x.Answer.QuestionId,
                        correctAnswerId = x.Answer.Question.Answers.FirstOrDefault(q => q.IsCorrect).Id
                    }).OrderBy(m => m.questionId).ToList()
                })
            });
        }

        public JsonResult GetGrammarNavigation()
        {
            var user = _userRepo.Fetch(_currentUserId);

            var hasLicense = user != null && user.HasGeoLicense;

            var tags = _tagRepo.Set()
                .OrderByDescending(m => !m.IsPayed)
                .Include(a => a.GeoGrammarSubTags)
                .Where(a => a.GeoGrammarSubTags.Count > 0 && a.GeoGrammarSubTags.Any(b => b.GeoVideos.Any() && b.Questions.Count(c => c.QuestionType == GeoQuestionType.PostQuestion) > 0))
                .Select(a => new
                {
                    id = a.Id,
                    isPayed = a.IsPayed,
                    name = a.TagName,
                    videoCount = a.GeoGrammarSubTags.Count,
                    needLicense = a.IsPayed && !hasLicense,
                    subTags = a.GeoGrammarSubTags.Where(x => (x.GeoVideos.Any() && x.Questions.Count(y => y.QuestionType == GeoQuestionType.PostQuestion) > 0)).Select(b => new
                    {
                        name = b.TagName,
                        id = b.Id
                    })
                }).ToList();

            return Json(new { tags }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetGrammarSubTagVideoAndPostQuestions(int subTagId)
        {
            var user = _userRepo.Fetch(_currentUserId);

            var subTag = _subTagRepo.Set()
                .Include(a => a.GeoGrammarTag)
                .Include(m => m.Questions.Select(a => a.Answers))
                .First(a => a.Id == subTagId);

            if (user == null && subTag.GeoGrammarTag.IsPayed) 
                return NeedLicenseJson(); 
            if(subTag.GeoGrammarTag.IsPayed && !user.HasGeoLicense)
                return NeedLicenseJson(); 

            var videos = _videoRepo.Set().Where(a => a.GeoGrammarSubTagId == subTag.Id);

            var userAnswers = _userAnswerRepo.Set()
                .Include(m => m.Answer.Question.Answers)
                .Where(m => m.UserId == _currentUserId && m.Answer.Question.GeoGrammarSubTagId == subTagId &&
                m.Answer.Question.QuestionType == GeoQuestionType.PostQuestion).Select(m => new ExerciseUserAnswerJson
                {
                    answerId = m.AnswerId,
                    questionId = m.Answer.QuestionId,
                    correctAnswerId = m.Answer.Question.Answers.FirstOrDefault(q => q.IsCorrect).Id
                })
                .OrderBy(m => m.questionId).ToList();

            return Json(new
            {
                video = videos.Select(a => new
                {
                    name = a.Name,
                    id = a.Id,
                    link = a.FileLink,
                    subTagId = a.GeoGrammarSubTagId
                }).First(),

                postQuestions = subTag.Questions.Where(a => a.QuestionType == GeoQuestionType.PostQuestion)
                .OrderBy(m => m.Id)
                .Select(a => new ExerciseQuestionJson
                {
                    id = a.Id,
                    text = a.Text,
                    hasAnswered = userAnswers.Any(m => m.questionId == a.Id),
                    answers = a.Answers.Select(b => new ExerciseAnswerJson
                    {
                        id = b.Id,
                        text = b.Text
                    }).ToList(),
                }),

                userAnswers = userAnswers
            });
        }

        [HttpPost]
        public JsonResult SetAnswer(SetAnswerDto model)
        {
            if (string.IsNullOrEmpty(_currentUserId)) {
                return Json(SetAnswerForGuest(model));
            }

            var userAnswer = _userAnswerRepo.Set()
                .Include(m => m.Answer)
                .FirstOrDefault(m => m.Answer.QuestionId == model.QuestionId && m.UserId == _currentUserId);

            if (userAnswer == null)
            {
                _userAnswerRepo.Save(new GeoUserAnswer
                {
                    AnswerDate = DateTime.Now,
                    AnswerId = model.AnswerId,
                    UserId = _currentUserId
                });
            }
            else
            {
                userAnswer.AnswerId = model.AnswerId;
                _userAnswerRepo.Save(userAnswer);
            }

            var correctAnswerId = _answerRepo.Set().SingleOrDefault(m => m.QuestionId == model.QuestionId && m.IsCorrect).Id;

            return Json(new ExerciseSetAnswerJson
            {
                questionId = model.QuestionId,
                correctAnswerId = correctAnswerId
            });
        } 

        public ExerciseSetAnswerJson SetAnswerForGuest(SetAnswerDto model)
        {
            return new ExerciseSetAnswerJson
            {
                questionId = model.QuestionId,
                correctAnswerId = _answerRepo.Set().SingleOrDefault(m => m.QuestionId == model.QuestionId && m.IsCorrect).Id
            };
        }

        [HttpPost]
        public JsonResult ResetLiteraturePreVideoQuestions(int id)
        {
            if (string.IsNullOrEmpty(_currentUserId))
                return null;

            var userAnswers =
                _userAnswerRepo.Set()
                    .Include(a => a.Answer.Question)
                    .Where(a => a.UserId == _currentUserId
                    && a.Answer.Question.GeoAuthorWorkPartId == id
                    && a.Answer.Question.QuestionType == GeoQuestionType.PreQuestion).ToList();

            foreach (var item in userAnswers)
                _userAnswerRepo.Delete(item);

            return Json(new { success = true });
        }

        [HttpPost]
        public JsonResult ResetLiteraturePostVideoQuestions(int id)
        {
            if (string.IsNullOrEmpty(_currentUserId))
                return null;

            var userAnswers =
                _userAnswerRepo.Set()
                    .Include(a => a.Answer.Question)
                    .Where(a => a.UserId == _currentUserId
                    && a.Answer.Question.GeoAuthorWorkPartId == id
                    && a.Answer.Question.QuestionType == GeoQuestionType.PostQuestion).ToList();

            foreach (var item in userAnswers)
                _userAnswerRepo.Delete(item);

            return Json(new { success = true });
        }

        [HttpPost]
        public JsonResult ResetGrammarVideoQuestions(int id)
        {
            if (string.IsNullOrEmpty(_currentUserId))
                return null;

            var userAnswers =
                _userAnswerRepo.Set()
                    .Include(a => a.Answer.Question)
                    .Where(a => a.UserId == _currentUserId
                    && a.Answer.Question.GeoGrammarSubTagId == id
                    && a.Answer.Question.QuestionType == GeoQuestionType.PostQuestion).ToList();

            foreach (var item in userAnswers)
                _userAnswerRepo.Delete(item);

            return Json(new { success = true });
        }
    }
}