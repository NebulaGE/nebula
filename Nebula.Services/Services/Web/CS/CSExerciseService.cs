using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nebula.Domain;
using Nebula.Domain.Entities;
using Nebula.Services.Dto;
using Nebula.Services.Json;
using System.Web;
using Microsoft.AspNet.Identity;

namespace Nebula.Services.Services.Web.CS
{
    public class CSExerciseService
    {
        private readonly GeneralRepository _generalRepository;

        public CSExerciseService(GeneralRepository generalRepository)
        {
            _generalRepository = generalRepository;
        }

        public ExerciseQuestionAndUserAnswerJson GetQuestions(int moduleId, string userId)
        {
            var model = new ExerciseQuestionAndUserAnswerJson();

            var userAnswers = _generalRepository
                .UserAnswer.Set()
                .Include(m => m.Answer.Question.Answers)
                .Where(m => m.Answer.Question.ModuleId == moduleId &&
                 m.UserId == userId && m.Answer.Question.QuestionType == CSQuestionType.ExerciseQuestion 
                 && !m.Answer.Question.IsBookQuestion && !m.Answer.Question.IsNaecQuestion
                /*  && !string.IsNullOrEmpty(m.Answer.Question.ExplanationVideoLink)*/)
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
                    correctAnswerId = m.Answer.IsCorrect ?
                m.AnswerId : m.Answer.Question.Answers.FirstOrDefault(a => a.IsCorrect).Id,
                    questionId = m.Answer.QuestionId
                }).ToList();



            model.questions.AddRange(passedAnswers.Select(m => new ExerciseQuestionJson
            {
                id = m.Answer.QuestionId,
                text = m.Answer.Question.QuestionText,
                explanationVideo = m.Answer.Question.ExplanationVideoLink,
                answers = m.Answer.Question.Answers
                 .OrderBy(a => a.Id)
                 .Select(a => new ExerciseAnswerJson
                 {
                     id = a.Id,
                     text = a.AnswerString
                 }).ToList()
            }));

            // new questions and exclude old ones 
            var newQuestions = _generalRepository.CsQuestion.Set()
                     .Include(m => m.Answers)
                     .Where(m => m.ModuleId == moduleId && m.QuestionType == CSQuestionType.ExerciseQuestion && m.Answers.Any() 
                     && !m.IsBookQuestion && !m.IsNaecQuestion /*&& !string.IsNullOrEmpty(m.ExplanationVideoLink)*/).ToList()
                     .Where(m => !userAnswers.Any(x => x.Answer.QuestionId == m.Id))
                     .OrderBy(m => Guid.NewGuid()).Take(takeRemainQuestions).ToList()
                     .Select(m => new ExerciseQuestionJson
                     {
                         id = m.Id,
                         text = m.QuestionText,
                         explanationVideo = m.ExplanationVideoLink,
                         answers = m.Answers
                         .OrderBy(a => a.Id)
                         .Select(a => new ExerciseAnswerJson
                         {
                             id = a.Id,
                             text = a.AnswerString
                         }).ToList()
                     }).ToList();

            model.questions.AddRange(newQuestions);

            return model;
        }

        public ExerciseQuestionAndUserAnswerJson GetQuestionsForGuest(int moduleId)
        {
            int maxModule;
            int minModule;

            if (moduleId == 1)
            {
                minModule = 0;
                maxModule = 5;
            }
            else
            {
                minModule = 4;
                maxModule = 9;
            }

            var model = new ExerciseQuestionAndUserAnswerJson
            {
                questions = _generalRepository.CsQuestion.Set()
                    .Include(m => m.Answers)
                    .Where(a => a.IsFreeQuestion 
                           && a.QuestionType == CSQuestionType.ExerciseQuestion 
                           && a.ModuleId < maxModule && a.ModuleId > minModule 
                           && !string.IsNullOrEmpty(a.ExplanationVideoLink)) 
                    .OrderBy(m => m.Id).Take(24).ToList()
                    .Select(m => new ExerciseQuestionJson
                    {
                        id = m.Id,
                        text = m.QuestionText,
                        explanationVideo = m.ExplanationVideoLink, 
                        answers = m.Answers
                            .OrderBy(a => a.Id)
                            .Select(a => new ExerciseAnswerJson
                            {
                                id = a.Id,
                                text = a.AnswerString
                            }).ToList()
                    }).ToList()
            };

            return model;
        }

        public List<ExerciseNavigationJson> GetNavigation(int type)
        {
            var userId = HttpContext.Current.User.Identity.GetUserId();


            var modules = _generalRepository.Module.Set()
               .Where(m => m.CSQuestionCategoryId == type)
               .AsEnumerable()
                .Select(m => new ExerciseNavigationJson
                {
                    id = m.Id,
                    name = m.Name,
                    type = m.HasMultipleQuestions.GetValueOrDefault() ? ExerciseType.Task : ExerciseType.Question
                }).ToList();

            foreach (var module in modules)
            {
                module.questionsCount = _generalRepository.CsQuestion.Set()
                    .Count(x => x.ModuleId == module.id & x.QuestionType == CSQuestionType.ExerciseQuestion && !x.IsBookQuestion && !x.IsNaecQuestion 
                           /*&& !string.IsNullOrEmpty(x.ExplanationVideoLink)*/);

                module.userAnswersCount = _generalRepository.UserAnswer.Set()
                    .Include(x => x.Answer.Question)
                    .Count(x => x.Answer.Question.ModuleId == module.id && x.UserId == userId 
                    && !x.Answer.Question.IsBookQuestion && !x.Answer.Question.IsNaecQuestion & x.Answer.Question.QuestionType == CSQuestionType.ExerciseQuestion 
                    /*&& !string.IsNullOrEmpty(x.Answer.Question.ExplanationVideoLink)*/);
            }

            return modules;
        }

        public List<ExerciseNavigationJson> GetNavigationForGuest(int type)
        {
            var module = new List<ExerciseNavigationJson>
            {
                new ExerciseNavigationJson
                {
                    id = type,
                    name = "უფასო სავარჯიშოები",
                    questionsCount = 24,
                    userAnswersCount = 0,
                    type = ExerciseType.Question
                }
            };

            return module;
        }

        public ExerciseSetAnswerJson SetAnswer(SetAnswerDto model, string userId)
        {
            var userAnswer = _generalRepository.UserAnswer.Set()
              .Include(m => m.Answer)
              .FirstOrDefault(m => m.Answer.QuestionId == model.QuestionId && m.UserId == userId);

            if (userAnswer == null)
            {
                _generalRepository.UserAnswer.Save(new UserAnswer
                {
                    AnswerDate = DateTime.Now,
                    AnswerId = model.AnswerId,
                    UserId = userId
                });
            }
            else
            {
                userAnswer.AnswerId = model.AnswerId;
                _generalRepository.UserAnswer.Save(userAnswer);
            }

            var correctAnswerId = _generalRepository.Answer.Set()
                .SingleOrDefault(m => m.QuestionId == model.QuestionId && m.IsCorrect).Id;

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
                correctAnswerId = _generalRepository.Answer.Set().SingleOrDefault(m => m.QuestionId == model.QuestionId && m.IsCorrect).Id
            };
        }

        public ExerciseTaskJson GetTask(int moduleId)
        {
            var userId = HttpContext.Current.User.Identity.GetUserId();

            var model = new ExerciseTaskJson();

            var currentTask = _generalRepository.FinishedTask.Set()
                .Include(m => m.Task.Questions.Select(a => a.Answers))                 
                .FirstOrDefault(m => m.UserId == userId && !m.IsFinished && m.ModuleId == moduleId 
                && m.Task.Questions.Any(x=>!x.IsBookQuestion && !x.IsNaecQuestion /*&& !string.IsNullOrEmpty(x.ExplanationVideoLink)*/) && m.Task.Type == TaskType.IsExerciseTask)?.Task;

            //continue
            if (currentTask != null)
            {
                foreach (var question in currentTask.Questions)
                {
                    var tempAnswer = _generalRepository.UserAnswer
                        .Set()
                        .Include(m => m.Answer.Question)
                        .FirstOrDefault(m => m.Answer.QuestionId == question.Id && m.UserId == userId);

                    if (tempAnswer != null)
                    {
                        model.userAnswers.Add(new ExerciseUserAnswerJson
                        {
                            answerId = tempAnswer.AnswerId,
                            questionId = question.Id,
                            correctAnswerId = question.Answers.FirstOrDefault(m => m.IsCorrect).Id
                        });
                    }
                }
            }
            //get new
            else
            {
                var excludeTaskIds = _generalRepository.FinishedTask.Set()
                    .Where(m => m.UserId == userId && m.IsFinished).Select(m => m.TaskId).ToList();

                currentTask = _generalRepository.Condition
                    .Set()
                    .Include(m => m.Questions.Select(a => a.Answers))
                    .Where(m => m.Type == TaskType.IsExerciseTask  && m.ModuleId == moduleId
                     && m.Questions.Any(x => !x.IsNaecQuestion && !x.IsBookQuestion /*&& !string.IsNullOrEmpty(x.ExplanationVideoLink)*/))
                    .AsEnumerable()
                    .OrderBy(m => Guid.NewGuid())
                    .FirstOrDefault(m => !excludeTaskIds.Any(x => x == m.Id));

                if (currentTask != null)
                {
                    _generalRepository.FinishedTask.Save(new FinishedTask
                    {
                        IsFinished = currentTask.Questions.Count() > 0 ? false : true,
                        ModuleId = moduleId,
                        TaskId = currentTask.Id,
                        UserId = userId
                    });
                }

            }

            // if empty
            if (currentTask == null)
                throw new Exception("Task resources limited");

            model.id = currentTask.Id;
            model.text = currentTask.TaskText; 
            model.questions = currentTask.Questions.Select(m => new ExerciseQuestionJson
            {
                id = m.Id,
                text = m.QuestionText,
                explanationVideo = m.ExplanationVideoLink, 
                answers = m.Answers.Select(a => new ExerciseAnswerJson
                {
                    id = a.Id,
                    text = a.AnswerString
                }).ToList()
            }).ToList();

            return model;
        }


        public ExerciseSetAnswerJson SetTaskAnswer(SetTaskAnswerDto model)
        {
            var userId = HttpContext.Current.User.Identity.GetUserId();

            var userAnswer = _generalRepository.UserAnswer.Set()
                   .Include(m => m.Answer)
                   .FirstOrDefault(m => m.Answer.QuestionId == model.QuestionId && m.UserId == userId);

            if (userAnswer == null)
            {
                _generalRepository.UserAnswer.Save(new UserAnswer
                {
                    AnswerDate = DateTime.Now,
                    AnswerId = model.AnswerId,
                    UserId = userId
                });
            }

            else
            {
                userAnswer.AnswerId = model.AnswerId;
                _generalRepository.UserAnswer.Save(userAnswer);
            }

            int userAnswersCount = _generalRepository.UserAnswer
                   .Set()
                   .Include(m => m.Answer.Question).Count(m => m.Answer.Question.ConditionId == model.TaskId && m.UserId == userId);

            int taskQuestionsCount = _generalRepository.CsQuestion.Set().Count(m => m.ConditionId == model.TaskId);

            if (userAnswersCount >= taskQuestionsCount)
            {
                var userTask = _generalRepository.FinishedTask.Set()
                    .FirstOrDefault(m => m.TaskId == model.TaskId && m.UserId == userId);

                userTask.IsFinished = true;
                _generalRepository.FinishedTask.Save(userTask);
            }
            var correctAnswerId = _generalRepository.Answer.Set()
                    .SingleOrDefault(m => m.QuestionId == model.QuestionId && m.IsCorrect).Id;

            return new ExerciseSetAnswerJson
            {
                correctAnswerId = correctAnswerId,
                questionId = model.QuestionId
            };
        }
    }
}
