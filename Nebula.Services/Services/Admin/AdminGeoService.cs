using System.Data.Entity;
using System.Linq;
using Nebula.Domain;
using Nebula.Domain.Entities;
using Nebula.Domain.ViewModels.Admin;
using Nebula.Domain.ViewModels.Admin.Geo;

namespace Nebula.Services.Services.Admin
{
    public class AdminGeoService
    {
        private readonly GeneralRepository _generalRepository;

        public AdminGeoService(GeneralRepository generalRepository)
        {
            _generalRepository = generalRepository;
        }

        #region FictionTextAnalysis 

        public GeoFictionTextViewModel GetGeoFictionTextViewModel(byte category, int id = 0)
        {
            var model = new GeoFictionTextViewModel
            {
                ThemeType = category
            };

            if (id == 0)
                return model;

            var dbFictionText = _generalRepository.GeoFictionText.Set()
                .Include(m => m.Questions) 
                .First(m => m.Id == id);

            model.Id = id;
            model.PointOne = dbFictionText.PointOne;
            model.PointTwo = dbFictionText.PointTwo;
            model.PointThree = dbFictionText.PointThree;
            model.Title = dbFictionText.Title;
            model.Text = dbFictionText.Text;

            model.Questions= dbFictionText.Questions.Select(m => new GeoQuestionViewModel
            {
                Id = m.Id, 
                QuestionType = m.QuestionType, 
                Text = m.Text ,
                GeoFictionTextId = m.GeoFictionTextId
            }).ToList();

            model.QuestionCreateViewModel = new GeoQuestionViewModel
            {
                QuestionType = GeoQuestionType.ExamQuestion,  
                GeoFictionTextId = model.Id,
                Action="SaveQuestion",
                Controller= "AdminGeoTextAnalisys",
                CategoryId=3
            };

            return model;
        }

        public void SaveGeoFictionText(GeoFictionTextViewModel model)
        {
            if (model.Id == 0)
                CreateGeoFictionText(model);
            else
                UpdateGeoFictionText(model);
        }

        public void CreateGeoFictionText(GeoFictionTextViewModel model)
        {
            var fictionText = new GeoFictionText
            {
                PointOne=model.PointOne,
                PointTwo=model.PointTwo,
                PointThree=model.PointThree,
                Text=model.Text,
                Title=model.Title,
                ThemeType=model.ThemeType==1?GeoThemeType.Poetry:GeoThemeType.Prose
             };

            _generalRepository.GeoFictionText.Save(fictionText);
            model.Id = fictionText.Id;
        }

        public void UpdateGeoFictionText(GeoFictionTextViewModel model)
        {
            var fictionText = _generalRepository.GeoFictionText.Fetch(model.Id);

            fictionText.PointOne = model.PointOne;
            fictionText.PointTwo = model.PointTwo;
            fictionText.PointThree = model.PointThree;
            fictionText.Text = model.Text;
            fictionText.Title = model.Title;
            fictionText.ThemeType = model.ThemeType == 1 ? GeoThemeType.Poetry : GeoThemeType.Prose;

            _generalRepository.GeoFictionText.Save(fictionText);
        }
        #endregion

        #region Question 
        public GeoQuestionViewModel GetGeoQuestionViewModel (byte questionCategory, int fictionId,  int id = 0)
        {
            var model = new GeoQuestionViewModel
            {
                CategoryId = questionCategory,
            };

            if (questionCategory == 1)
                model.Authors = _generalRepository.GeoAuthor
                    .Set()
                    .Include(a => a.GeoAuthorWorks.Select(x => x.GeoAuthorWorkParts)).ToList();

            else if (questionCategory == 2)
                model.Tags = _generalRepository.GeoGrammarTag.Set().Include(a => a.GeoGrammarSubTags).ToList();
            else
            {
                model.QuestionType = GeoQuestionType.ExamQuestion;
                model.GeoFictionTextId = fictionId;
            }

            if (id == 0)
                return model;

            var question = _generalRepository.GeoQuestion
                .Set().Where(a => a.Id == id)
                .Include(a => a.Answers).Single();

            model.Id = question.Id;
            model.Text = question.Text;
            model.VideoExplanationLink = question.ExplanationVideoLink;

            model.Answers = question.Answers.Select(a => new AnswerViewModel
            {
                Id = a.Id,
                IsCorrect = a.IsCorrect,
                Text = a.Text
            }).ToList();

            model.QuestionType = question.QuestionType;
            model.IsFreeQuestion = question.IsFreeQuestion;

            if (question.GeoAuthorWorkPartId != null)
                model.AuthorWorkPartId = (int)question.GeoAuthorWorkPartId;

            model.SubTagId = question.GeoGrammarSubTagId;

            return model;
        }

        public void SaveGeoQuestion(GeoQuestionViewModel model)
        {
            if (model.Id == 0)
                CreateGeoQuestion(model);
            else
                UpdateGeoQuestion(model);
        }

        public void CreateGeoQuestion(GeoQuestionViewModel model)
        {
            if (!model.Answers.Any(m => m.IsCorrect))
                throw new System.Exception("Please choose the correct answer");

            var question = new GeoQuestion
            {
                Text = model.Text,
                IsFreeQuestion = model.IsFreeQuestion,
                ExplanationVideoLink = model.VideoExplanationLink,
                GeoAuthorWorkPartId = model.AuthorWorkPartId,
                GeoGrammarSubTagId = model.SubTagId,
                GeoFictionTextId = model.GeoFictionTextId,
                Category = model.CategoryId == 2 ? GeoQuestionCategory.Grammar : GeoQuestionCategory.Literature,
                QuestionType = model.QuestionType
            };
       

            foreach (var item in model.Answers)
            {
                question.Answers.Add(new GeoAnswer()
                {
                    Text = item.Text,
                    IsCorrect = item.IsCorrect
                });
            }

           

            _generalRepository.GeoQuestion.Save(question);
            question.CorrectAnswerId = question.Answers.First(a => a.IsCorrect).Id;
            _generalRepository.GeoQuestion.Save(question);
        }

        public void UpdateGeoQuestion(GeoQuestionViewModel model)
        {
            if (!model.Answers.Any(m => m.IsCorrect))
                throw new System.Exception("Please choose the correct answer");

            var question = _generalRepository.GeoQuestion.Fetch(model.Id);

            question.Text = model.Text;
            question.ExplanationVideoLink = model.VideoExplanationLink;
            question.IsFreeQuestion = model.IsFreeQuestion;
            question.GeoAuthorWorkPartId = model.AuthorWorkPartId;
            question.GeoGrammarSubTagId = model.SubTagId;
            question.GeoFictionTextId = model.GeoFictionTextId;
            question.Category = model.CategoryId == 1 ? GeoQuestionCategory.Literature : GeoQuestionCategory.Grammar;
            question.QuestionType = model.QuestionType;

            _generalRepository.GeoQuestion.Save(question);

            var answers = _generalRepository.GeoAnswer.Set().Where(a => a.QuestionId == model.Id).ToList();



            foreach (var answer in answers)
            {
                if (model.Answers.All(a => a.Id != answer.Id))
                    _generalRepository.GeoAnswer.Delete(answer.Id);
            }

            foreach (var answer in model.Answers)
            {
                if (answer.Id == 0)
                {
                    _generalRepository.GeoAnswer.Save(new GeoAnswer
                    {
                        Text = answer.Text,
                        IsCorrect = answer.IsCorrect,
                        QuestionId = model.Id
                    });
                }
                else
                {
                    var geoAnswer = _generalRepository.GeoAnswer.Fetch(answer.Id);
                    geoAnswer.Text = answer.Text;
                    geoAnswer.IsCorrect = answer.IsCorrect;
                    _generalRepository.GeoAnswer.Save(geoAnswer);
                }
            }

            question.CorrectAnswerId = question.Answers.First(a => a.IsCorrect).Id;
            _generalRepository.GeoQuestion.Save(question);
        }
        #endregion 
    }
}
