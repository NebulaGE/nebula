using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nebula.Domain.Abstract;
using Nebula.Domain.Concrete;
using Nebula.Domain.Entities;
using Nebula.Domain.Repositories;
using Nebula.Web.Controllers;

namespace Nebula.Test
{
    [TestClass]
    public class UnitTestExam : BaseTest
    {
        [TestMethod]
        public void TestGetExamResult()
        {
            var _db = new NebulaDbContext();
            BaseRepository<VerbalQuiz> vq = new VerbalQuizRepo(_db);
            BaseRepository<MathQuiz> mq = new MathQuizRepo(_db);
            BaseRepository<User> u = new UserRepo(_db);
            BaseRepository<TaggingHierarchy> th = new TaggingHierarchyRepo(_db);
            BaseRepository<CSUserQuizzes> uq = new CSUserQuizzesRepo(_db);
            BaseRepository<CSQuestion> q = new CSQuestionRepo(_db);
            BaseRepository<Module> m = new ModuleRepo(_db);

            var ec = new ExamController(m, th, q, u, uq, vq, mq)
            {
                _currentUserId = "4c642e1a-815d-4136-ae3f-5189bd7346a8"
            };
            ec.GetResult(5457);
        }
    }
}
