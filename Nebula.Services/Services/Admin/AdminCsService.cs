using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Nebula.Domain;
using Nebula.Domain.Entities;
using Nebula.Domain.ViewModels.Admin;
using Nebula.Domain.ViewModels.Admin.CS;
using Nebula.Services.Utils;

namespace Nebula.Services.Services.Admin
{

    public class AdminCsService
    {
        private readonly GeneralRepository _generalRepository;

        public AdminCsService(GeneralRepository generalRepository)
        {
            _generalRepository = generalRepository;
        }

        #region  CsQuestion 
        public CSQuestionViewModel GetCsQuestionViewModel(byte questionCategoryId, int id = 0)
        {
            var model = new CSQuestionViewModel
            {
                CategoryId = questionCategoryId,
                Infos = _generalRepository.CSQuestionInfo.Set().AsEnumerable().Select(m => new CSQuestionInfoViewModel
                {
                    Id = m.Id,
                    Name = m.GetName()
                }).ToList(),
                Tags = _generalRepository.Tag.Set().Where(m => m.QuestionTypeId == questionCategoryId).Select(m => new CSTagViewModel
                {
                    Id = m.Id,
                    Name = m.Name
                }).ToList(),
                Modules = _generalRepository.Module.Set().Where(m => m.CSQuestionCategoryId == questionCategoryId).Select(m => new ModuleViewModel
                {
                    Id = m.Id,
                    Name = m.Name
                }).ToList()
            };
            model.Infos.Insert(0, new CSQuestionInfoViewModel { Id = null, Name = "წელი-ვარიანტი" });

            if (id == 0)
                return model;

            var dbQuestion = _generalRepository.CsQuestion.Set()
                .Where(m => m.Id == id)
                .Include(m => m.Answers)
                .Include(m => m.QuestionTags).Single();

            model.Id = id;

            model.Answers = dbQuestion.Answers.Select(m => new AnswerViewModel
            {
                Id = m.Id,
                IsCorrect = m.IsCorrect,
                Text = m.AnswerString
            }).ToList();

            model.Text = dbQuestion.QuestionText;
            model.IsFreeQuestion = dbQuestion.IsFreeQuestion;
            model.VideoExplanationLink = dbQuestion.ExplanationVideoLink;
            model.ConditionId = dbQuestion.ConditionId;
            model.Type = (CSQuestionType)dbQuestion.QuestionType;

            model.ModuleId = dbQuestion.ModuleId;
            model.QueueNumber = dbQuestion.Num;
            model.TagIds = dbQuestion.QuestionTags.Select(m => m.TagId).ToList();
            model.InfoId = dbQuestion.CSQuestionInfoId;

            return model;
        }

        public void SaveCsQuestion(CSQuestionViewModel model)
        {
            if (model.Id == 0)
                CreateCsQuestion(model);
            else
                UpdateCsQuestion(model);
        }

        private void CreateCsQuestion(CSQuestionViewModel model)
        {
            if (!model.Answers.Any(m => m.IsCorrect))
                throw new System.Exception("Please choose the correct answer");

            var question = new CSQuestion
            {
                QuestionText = model.Text,
                QuestionType = model.Type,
                IsFreeQuestion = model.IsFreeQuestion,
                ExplanationVideoLink = model.VideoExplanationLink,
                CSQuestionInfoId = model.InfoId,
                ConditionId = model.ConditionId,
                Num = model.QueueNumber,
                VideoPartId = model.VideoPartId,
                VideoId = model.VideoId,
                ModuleId = model.ModuleId
            };

            foreach (var answer in model.Answers)
            {
                question.Answers.Add(new Answer
                {
                    AnswerString = answer.Text,
                    IsCorrect = answer.IsCorrect
                });
            }
            foreach (var tagId in model.TagIds.Where(m => m != -1))
            {
                question.QuestionTags.Add(new QuestionTag
                {
                    TagId = tagId
                });
            }

         

            _generalRepository.CsQuestion.Save(question);

            question.CorrectAnswerId = question.Answers.First(a => a.IsCorrect).Id;
            _generalRepository.CsQuestion.Save(question);
        }

        private void UpdateCsQuestion(CSQuestionViewModel model)
        {
            if (!model.Answers.Any(m => m.IsCorrect))
                throw new System.Exception("Please choose the correct answer");

            var dbCsQuestion = _generalRepository.CsQuestion.Fetch(model.Id);

            dbCsQuestion.QuestionText = model.Text;
            dbCsQuestion.ExplanationVideoLink = model.VideoExplanationLink;
            dbCsQuestion.IsFreeQuestion = model.IsFreeQuestion;
            dbCsQuestion.ConditionId = model.ConditionId;
            dbCsQuestion.QuestionType = model.Type;
            dbCsQuestion.ModuleId = model.ModuleId;
            dbCsQuestion.Num = model.QueueNumber;
            dbCsQuestion.CSQuestionInfoId = model.InfoId;
            _generalRepository.CsQuestion.Save(dbCsQuestion);

          
            var dbAnswers = _generalRepository.Answer.Set()
                .Where(m => m.QuestionId == model.Id).ToList();

            //check answers deletion 
            foreach (var dbAnswer in dbAnswers)
            {
                if (model.Answers.All(m => m.Id != dbAnswer.Id))
                    _generalRepository.Answer.Delete(dbAnswer.Id);
            }

          

            //answers update
            foreach (var answer in model.Answers)
            {
                if (answer.Id == 0)
                {
                    _generalRepository.Answer.Save(new Answer
                    {
                        QuestionId = model.Id,
                        AnswerString = answer.Text,
                        IsCorrect = answer.IsCorrect
                    });
                }
                else
                {
                    var dbAnswer = _generalRepository.Answer.Fetch(answer.Id);
                    dbAnswer.AnswerString = answer.Text;
                    dbAnswer.IsCorrect = answer.IsCorrect;
                    _generalRepository.Answer.Save(dbAnswer);
                }
            }

            var dbTags = _generalRepository.QuestionTag.Set()
                .Where(m => m.QuestionId == model.Id).ToList();

            //delete tag if user uncheck 
            foreach (var dbTag in dbTags)
            {
                if (model.TagIds.All(m => m != dbTag.TagId))
                {
                    _generalRepository.QuestionTag.Delete(dbTag.Id);
                }
            }

            //add tag if not exist
            foreach (var tagId in model.TagIds.Where(m => m != -1))
            {
                if (dbTags.All(m => m.TagId != tagId))
                {
                    _generalRepository.QuestionTag.Save(new QuestionTag
                    {
                        TagId = tagId,
                        QuestionId = model.Id
                    });
                }
            }

            dbCsQuestion.CorrectAnswerId = dbCsQuestion.Answers.First(a => a.IsCorrect).Id;
            _generalRepository.CsQuestion.Save(dbCsQuestion);
        }

        public int? GetNextQuestionId(int? questionInfoId, int? questionQueueNumber)
        {
            questionQueueNumber += 1;
            var nextQuestion =
                _generalRepository.CsQuestion.Set().FirstOrDefault(m => m.CSQuestionInfoId == questionInfoId
                                                                        && m.Num == questionQueueNumber);

            if (nextQuestion == null)
                return null;
            {
                // if question has condition get next question from condition
                if (nextQuestion.ConditionId == null)
                    return nextQuestion.Id;

                var conditionQuestionsCount =
                    _generalRepository.CsQuestion.Set().Count(m => m.ConditionId == nextQuestion.ConditionId);

                questionQueueNumber += conditionQuestionsCount;

                var conditionQuestion =
                    _generalRepository.CsQuestion.Set().FirstOrDefault(m => m.CSQuestionInfoId == questionInfoId
                                                                            && m.Num == questionQueueNumber);

                return conditionQuestion?.Id ?? nextQuestion.Id;
            }
        }
        #endregion 

        #region Condition

        public ConditionViewModel GetConditionViewModel(byte categoryId, int id = 0)
        {
            var model = new ConditionViewModel
            {
                CategoryId = categoryId,
                Infos = _generalRepository.CSQuestionInfo.Set().AsEnumerable().Select(m => new CSQuestionInfoViewModel
                {
                    Id = m.Id,
                    Name = m.GetName()
                }).ToList(),

                Modules = _generalRepository.Module.Set().Where(m => m.CSQuestionCategoryId == categoryId).Select(m => new ModuleViewModel
                {
                    Id = m.Id,
                    Name = m.Name
                }).ToList()
            };

            model.Infos.Insert(0, new CSQuestionInfoViewModel { Id = null, Name = "წელი-ვარიანტი" });

            if (id == 0)
                return model;

            var dbCondition = _generalRepository.Condition.Set()
                .Include(t => t.Module)
                .Include(t => t.Questions.Select(m => m.Module))
                .First(t => t.Id == id);
            model.Id = dbCondition.Id;
            model.ModuleId = dbCondition.ModuleId;
            model.GuestCondition = dbCondition.IsGuestCondition;
            model.Type = dbCondition.Type;
            model.Text = dbCondition.TaskText; 
            model.QueueNumber = dbCondition.Num;

            model.Questions = dbCondition.Questions.Select(m => new ConditionRelatedQuestionsViewModel
            {
                Id = m.Id,
                Module = m.Module.Name,
                VideoExplanationLink = m.ExplanationVideoLink,
                Text = m.QuestionText,
                Type = m.QuestionType,
            }).ToList();

            model.QuestionCreateViewModel = GetCsQuestionViewModel(categoryId);
            model.QuestionCreateViewModel.ConditionId = model.Id;
            model.QuestionCreateViewModel.ModuleId = model.ModuleId;
            model.QuestionCreateViewModel.InfoId = model.InfoId;
            model.QuestionCreateViewModel.Type = model.Type == TaskType.IsExamTask ? CSQuestionType.ExamQuestion : CSQuestionType.ExerciseQuestion;
            model.QuestionCreateViewModel.Controller = "AdminPirobebi";
            model.QuestionCreateViewModel.Action = "SaveQuestion";

            return model;
        }

        public void SaveCondition(ConditionViewModel model)
        {
            if (model.Id == 0)
                CreateCondition(model);
            else
                UpdateCondition(model);
        }

        public void CreateCondition(ConditionViewModel model)
        {
            var condition = new Condition
            {
                CSQuestionInfoId = model.InfoId,
                IsGuestCondition = model.GuestCondition,
                ModuleId = model.ModuleId,
                Num = model.QueueNumber,
                TaskText = model.Text,
                Type = model.Type
            };

            _generalRepository.Condition.Save(condition);

            model.Id = condition.Id;
        }

        public void UpdateCondition(ConditionViewModel model)
        {
            var dbCondition = _generalRepository.Condition.Fetch(model.Id);
            dbCondition.IsGuestCondition = model.GuestCondition;
            dbCondition.TaskText = model.Text;
            dbCondition.ModuleId = model.ModuleId;
            dbCondition.Type = model.Type;
            dbCondition.CSQuestionInfoId = model.InfoId;
            dbCondition.Num = model.QueueNumber;
            _generalRepository.Condition.Save(dbCondition);

            UpdateConditionQuestions(dbCondition);

        }

        public void UpdateConditionQuestions(Condition condition)
        {
            var questions = _generalRepository.CsQuestion.Set()
                .Where(m => m.ConditionId == condition.Id).ToList();

            foreach(var question in questions)
            {
                question.CSQuestionInfoId = condition.CSQuestionInfoId; 
            }
            _generalRepository.CsQuestion._context.SaveChanges();
            
        }
        #endregion

        #region Video


        public void SaveVideo(Video video)
        {
            if (video.Id == 0)
                CreateVideo(video);
            else
                UpdateVideo(video);
        }

        public void CreateVideo(Video video)
        {
            if (!video.IsExerciseType)
            {
                string fileName = Guid.NewGuid() + video.VideoFile.FileName;
                var tag = _generalRepository.Tag.Fetch(video.TagId);
                string path = tag.IsPayed ? PathsHelper.PayedVideoPath : PathsHelper.VideoPath;


                FilesHelper.AddFile(video.VideoFile, fileName, path);
                video.FileName = fileName;

            }
            _generalRepository.Video.Save(video);
        }

        public void UpdateVideo(Video video)
        {
            if (video.VideoFile != null)
            {
                string fileName = Guid.NewGuid() + video.VideoFile.FileName;
                var tag = _generalRepository.Tag.Fetch(video.TagId);
                if (tag.IsPayed)
                {
                    FilesHelper.AddFile(video.VideoFile, fileName, PathsHelper.PayedVideoPath);
                    FilesHelper.DeleteFile(video.FileName, PathsHelper.PayedVideoPath);
                }
                else
                {
                    FilesHelper.AddFile(video.VideoFile, fileName, PathsHelper.VideoPath);
                    FilesHelper.DeleteFile(video.FileName, PathsHelper.VideoPath);
                }

                video.FileName = fileName;

            }
            _generalRepository.Video.Save(video);

        }
        #endregion
    }
}
