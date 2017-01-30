using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Nebula.Domain.Abstract;
using Nebula.Domain.Concrete;
using Nebula.Domain.Entities;

namespace Nebula.Test
{
    public class BaseTest
    {
        protected IDbContextNebula DbContextOfferPriceEvaluator;
        protected IDbSet<MathQuiz> _mathQuizes;
        protected IDbSet<Module> _modules;
        protected IDbSet<TaggingHierarchy> _taggingHierarchies;
        protected IDbSet<CSQuestion> _questions;
        protected IDbSet<User> _users;
        protected IDbSet<CSUserQuizzes> _userQuizes;
        protected IDbSet<VerbalQuiz> _verbalQuizes;

         
        [TestInitialize]
        public void Init()
        {
            _mathQuizes = new FakeDbSet<MathQuiz>();
            _modules = new FakeDbSet<Module>();
            _taggingHierarchies = new FakeDbSet<TaggingHierarchy>();
            _questions = new FakeDbSet<CSQuestion>();
            _users = new FakeDbSet<User>();
            _userQuizes = new FakeDbSet<CSUserQuizzes>();
            _verbalQuizes = new FakeDbSet<VerbalQuiz>(); 

            CultureInfo culture = new System.Globalization.CultureInfo("en-US")
            {
                DateTimeFormat = { ShortDatePattern = "dd/MM/yy" }
            };
            Thread.CurrentThread.CurrentCulture = culture;


            var offerPriceEvaluatorContext = new Mock<IDbContextNebula>();
            offerPriceEvaluatorContext.SetupGet(x => x.MathQuizes).Returns(_mathQuizes);
            offerPriceEvaluatorContext.SetupGet(x => x.Modules).Returns(_modules);
            offerPriceEvaluatorContext.SetupGet(x => x.TaggingHierarchies).Returns(_taggingHierarchies);
            offerPriceEvaluatorContext.SetupGet(x => x.CSQuestions).Returns(_questions);
            offerPriceEvaluatorContext.SetupGet(x => x.UserQuizes).Returns(_userQuizes);
            offerPriceEvaluatorContext.SetupGet(x => x.VerbalQuizes).Returns(_verbalQuizes); 

            offerPriceEvaluatorContext.Setup(x => x.Set<MathQuiz>()).Returns(_mathQuizes);
            offerPriceEvaluatorContext.Setup(x => x.Set<Module>()).Returns(_modules);
            offerPriceEvaluatorContext.Setup(x => x.Set<TaggingHierarchy>()).Returns(_taggingHierarchies);
            offerPriceEvaluatorContext.Setup(x => x.Set<CSQuestion>()).Returns(_questions);
            offerPriceEvaluatorContext.Setup(x => x.Set<CSUserQuizzes>()).Returns(_userQuizes);
            offerPriceEvaluatorContext.Setup(x => x.Set<VerbalQuiz>()).Returns(_verbalQuizes); 

            DbContextOfferPriceEvaluator = offerPriceEvaluatorContext.Object;
             
        }
    }
}
