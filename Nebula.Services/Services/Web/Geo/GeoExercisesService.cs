using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNet.Identity;
using Nebula.Domain;
using Nebula.Services.Json;
using System.Data.Entity;
using Nebula.Domain.Entities;
using Nebula.Services.Dto;
using System.Web;

namespace Nebula.Services.Services.Web.Geo
{
    public class GeoExercisesService
    {
        private readonly GeneralRepository _generalRepository;

        public GeoExercisesService(GeneralRepository generalRepository)
        {
            _generalRepository = generalRepository;
        }

        public ExerciseQuestionAndUserAnswerJson GetGrammarQuestions(int subTagId, string userId)
        {
            var model = new ExerciseQuestionAndUserAnswerJson();

            var userAnswers = _generalRepository.GeoUserAnswer.Set()
                .Include(m => m.Answer.Question.Answers)
                .Where(m => m.Answer.Question.GeoGrammarSubTagId == subTagId && m.UserId == userId && m.Answer.Question.QuestionType == GeoQuestionType.ExerciseQuestion)
                .OrderByDescending(m => m.AnswerDate).ToList();

            int takeLatestAnswersCount = userAnswers.Count() % 10;
            int takeRemainQuestions = 10 - takeLatestAnswersCount;

            // latest answered
            var passedAnswers = userAnswers.Take(takeLatestAnswersCount)
                 .OrderBy(m => m.AnswerDate)
                 .ToList();

            model.userAnswers = passedAnswers.Select(m => new ExerciseUserAnswerJson
            {
                answerId = m.AnswerId,
                correctAnswerId = m.Answer.IsCorrect ? m.AnswerId : m.Answer.Question.Answers.FirstOrDefault(a => a.IsCorrect).Id,
                questionId = m.Answer.QuestionId
            }).ToList();

            model.questions.AddRange(passedAnswers.Select(m => new ExerciseQuestionJson
            {
                id = m.Answer.QuestionId,
                text = m.Answer.Question.Text,
                explanationVideo = m.Answer.Question.ExplanationVideoLink,
                answers = m.Answer.Question.Answers
                 .OrderBy(a => a.Id)
                 .Select(a => new ExerciseAnswerJson
                 {
                     id = a.Id,
                     text = a.Text
                 }).ToList()
            }));

            // new questions and exclude old ones
            var newQuestions = _generalRepository.GeoQuestion.Set()
                     .Include(m => m.Answers)
                     .Where(m => m.GeoGrammarSubTagId == subTagId && m.QuestionType == GeoQuestionType.ExerciseQuestion).ToList()
                     .Where(m => !userAnswers.Any(x => x.Answer.QuestionId == m.Id)).OrderBy(m => Guid.NewGuid()).Take(takeRemainQuestions).ToList()
                     .Select(m => new ExerciseQuestionJson
                     {
                         id = m.Id,
                         text = m.Text,
                         explanationVideo = m.ExplanationVideoLink,
                         answers = m.Answers
                         .OrderBy(a => a.Id)
                         .Select(a => new ExerciseAnswerJson
                         {
                             id = a.Id,
                             text = a.Text
                         }).ToList()
                     }).ToList();

            model.questions.AddRange(newQuestions);

            return model;
        }

        public ExerciseQuestionAndUserAnswerJson GetGrammarQuestionsForGuest()
        {
            var model = new ExerciseQuestionAndUserAnswerJson
            {
                questions =
                    _generalRepository.GeoQuestion.Set()
                        .Include(m => m.Answers)
                        .Where(a => a.IsFreeQuestion && a.QuestionType == GeoQuestionType.ExerciseQuestion && a.GeoAuthorWorkPartId == null )
                        .OrderBy(m => m.Id).Take(24).ToList()
                        .Select(m => new ExerciseQuestionJson
                        {
                            id = m.Id,
                            text = m.Text,
                            answers = m.Answers
                                .OrderBy(a => a.Id)
                                .Select(a => new ExerciseAnswerJson
                                {
                                    id = a.Id,
                                    text = a.Text
                                }).ToList()
                        }).ToList()
            };

            return model;
        }

        public ExerciseQuestionAndUserAnswerJson GetLiteratureQuestions(int authorId, string userId)
        {
            var model = new ExerciseQuestionAndUserAnswerJson();

            var userAnswers = _generalRepository
                .GeoUserAnswer.Set()
                .Include(m => m.Answer.Question.Answers)
                .Include(m => m.Answer.Question.GeoAuthorWorkPart.AuthorWork)
                .Where(m => m.Answer.Question.GeoAuthorWorkPart.AuthorWork.AuthorId == authorId
                && m.UserId == userId
                && m.Answer.Question.QuestionType == GeoQuestionType.ExerciseQuestion)
                .OrderByDescending(m => m.AnswerDate)
                .ToList();

            int takeLatestAnswersCount = userAnswers.Count() % 10;
            int takeRemainQuestions = 10 - takeLatestAnswersCount;

            // latest answered
            var passedAnswers = userAnswers.Take(takeLatestAnswersCount)
                 .OrderBy(m => m.AnswerDate)
                 .ToList();

            model.userAnswers = passedAnswers
                .Select(m => new ExerciseUserAnswerJson
                {
                    answerId = m.AnswerId,
                    correctAnswerId = m.Answer.IsCorrect ? m.AnswerId : m.Answer.Question.Answers.FirstOrDefault(a => a.IsCorrect)?.Id,
                    questionId = m.Answer.QuestionId
                }).ToList();

            model.questions.AddRange(passedAnswers.Select(m => new ExerciseQuestionJson
            {
                id = m.Answer.QuestionId,
                text = m.Answer.Question.Text,
                explanationVideo = m.Answer.Question.ExplanationVideoLink,
                answers = m.Answer.Question.Answers
                 .OrderBy(a => a.Id)
                 .Select(a => new ExerciseAnswerJson
                 {
                     id = a.Id,
                     text = a.Text
                 }).ToList()
            }));

            // new questions and exclude old ones
            var newQuestions = _generalRepository.GeoQuestion.Set()
                     .Include(m => m.Answers)
                     .Include(m => m.GeoAuthorWorkPart.AuthorWork)
                     .Where(m => m.GeoAuthorWorkPart.AuthorWork.AuthorId == authorId && m.QuestionType == GeoQuestionType.ExerciseQuestion)
                     .ToList()
                     .Where(m => !userAnswers.Any(x => x.Answer.QuestionId == m.Id))
                     .OrderBy(m => Guid.NewGuid())
                     .Take(takeRemainQuestions).ToList()
                     .Select(m => new ExerciseQuestionJson
                     {
                         id = m.Id,
                         text = m.Text,
                         explanationVideo=m.ExplanationVideoLink,
                         answers = m.Answers
                         .OrderBy(a => a.Id)
                         .Select(a => new ExerciseAnswerJson
                         {
                             id = a.Id,
                             text = a.Text
                         }).ToList()
                     }).ToList();

            model.questions.AddRange(newQuestions);

            return model;
        }

        public ExerciseQuestionAndUserAnswerJson GetLiteratureQuestionsForGuest()
        {
            var model = new ExerciseQuestionAndUserAnswerJson
            {
                questions =
                _generalRepository.GeoQuestion.Set()
                    .Include(m => m.Answers)
                    .Where(a => a.IsFreeQuestion && a.QuestionType == GeoQuestionType.ExerciseQuestion && a.GeoGrammarSubTagId == null )
                    .OrderBy(m => m.Id).Take(24).ToList()
                    .Select(m => new ExerciseQuestionJson
                    {
                        id = m.Id,
                        text = m.Text,
                        answers = m.Answers
                            .OrderBy(a => a.Id)
                            .Select(a => new ExerciseAnswerJson
                            {
                                id = a.Id,
                                text = a.Text
                            }).ToList()
                    }).ToList()
            };

            return model;
        }

        public List<ExerciseNavigationJson> GetGrammarNavigation()
        {
            var userId = HttpContext.Current.User.Identity.GetUserId();

            var subTags = _generalRepository.GeoGrammarSubTag.Set()
                .Where(a => a.Questions.Any(b => b.QuestionType == GeoQuestionType.ExerciseQuestion)).Select(m => new ExerciseNavigationJson
                {
                    id = m.Id,
                    name = m.TagName
                }).ToList();

            foreach (var subTag in subTags)
            {
                subTag.questionsCount = _generalRepository.GeoQuestion.Set().Count(x => x.GeoGrammarSubTagId == subTag.id && x.QuestionType == GeoQuestionType.ExerciseQuestion);

                subTag.userAnswersCount = _generalRepository.GeoUserAnswer.Set()
                    .Include(x => x.Answer.Question).Count(x => x.Answer.Question.GeoGrammarSubTagId == subTag.id && x.UserId == userId
                        && x.Answer.Question.QuestionType == GeoQuestionType.ExerciseQuestion);
            }

            return subTags;
        }

        public List<ExerciseNavigationJson> GetNavigationForGuest()
        {
            var subTags = new List<ExerciseNavigationJson>
            {
                new ExerciseNavigationJson
                {
                    id = 1,
                    name = "უფასო სავარჯიშოები",
                    questionsCount = 24,
                    userAnswersCount = 0,
                    type = ExerciseType.Question
                }
            };

            return subTags;
        }

        public List<ExerciseNavigationJson> GetLiteratureNavigation()
        {
            var userId = HttpContext.Current.User.Identity.GetUserId();
            var authros = _generalRepository.GeoAuthor.Set()
                .Select(m => new ExerciseNavigationJson
                {
                    id = m.Id,
                    name = m.AuthorName
                }).ToList();


            foreach (var author in authros)
            {
                author.questionsCount = _generalRepository.GeoQuestion.Set()
                    .Include(m => m.GeoAuthorWorkPart.AuthorWork).Count(x => x.GeoAuthorWorkPart.AuthorWork.AuthorId == author.id && x.QuestionType == GeoQuestionType.ExerciseQuestion);

                author.userAnswersCount = _generalRepository.GeoUserAnswer.Set()
                .Include(x => x.Answer.Question.GeoAuthorWorkPart.AuthorWork).Count(x => x.Answer.Question.GeoAuthorWorkPart.AuthorWork.AuthorId == author.id && x.UserId == userId
                             && x.Answer.Question.QuestionType == GeoQuestionType.ExerciseQuestion);
            }

            authros = authros.Where(m => m.questionsCount > 0).ToList();

            return authros;
        }

        public ExerciseSetAnswerJson SetAnswer(SetAnswerDto model, string userId)
        {
            var userAnswer = _generalRepository.GeoUserAnswer.Set()
              .Include(m => m.Answer)
              .FirstOrDefault(m => m.Answer.QuestionId == model.QuestionId && m.UserId == userId);

            if (userAnswer == null)
            {
                _generalRepository.GeoUserAnswer.Save(new GeoUserAnswer
                {
                    AnswerDate = DateTime.Now,
                    AnswerId = model.AnswerId,
                    UserId = userId
                });
            }
            else
            {
                userAnswer.AnswerId = model.AnswerId;
                _generalRepository.GeoUserAnswer.Save(userAnswer);
            }

            var correctAnswerId = _generalRepository.GeoAnswer.Set().SingleOrDefault(m => m.QuestionId == model.QuestionId && m.IsCorrect).Id;

            return new ExerciseSetAnswerJson
            {
                questionId = model.QuestionId,
                correctAnswerId = correctAnswerId
            };
        }

        public ExerciseSetAnswerJson SetAnswerForGuest(SetAnswerDto model)
        {
            return new ExerciseSetAnswerJson
            {
                questionId = model.QuestionId,
                correctAnswerId = _generalRepository.GeoAnswer.Set().SingleOrDefault(m => m.QuestionId == model.QuestionId && m.IsCorrect).Id
            };
        }
    }
}
