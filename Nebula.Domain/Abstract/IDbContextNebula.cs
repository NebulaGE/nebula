using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation; 
using System.Threading.Tasks;
using Nebula.Domain.Entities;

namespace Nebula.Domain.Abstract
{
    public interface IDbContextNebula : IDisposable
    {  
        IDbSet<Answer> Answers { get; set; }
        IDbSet<UserAnswer> UserAnswers { get; set; }
        IDbSet<CSQuestion> CSQuestions { get; set; }
        IDbSet<CSQuiz> Quizes { get; set; }
        IDbSet<CSUserQuizzes> UserQuizes { get; set; }
        IDbSet<CSQuestionCategory> CSQuestionCategories { get; set; }
        IDbSet<Module> Modules { get; set; }
        IDbSet<News> News { get; set; } 
        IDbSet<CSTag> Tags { get; set; }
        IDbSet<ExamType> ExamTypes { get; set; }
        IDbSet<MathQuiz> MathQuizes { get; set; }
        IDbSet<VerbalQuiz> VerbalQuizes { get; set; }
        IDbSet<Video> Videos { get; set; }
        IDbSet<VideoPart> VideoParts { get; set; }
        IDbSet<Entities.Condition> Conditions { get; set; }
        IDbSet<QuizSkeleton> QuizSkeletions { get; set; }
        IDbSet<QuestionTag> QuestionTags { get; set; }
        IDbSet<Image> Images { get; set; }
        IDbSet<TaggingHierarchy> TaggingHierarchies { get; set; }
        IDbSet<CSQuestionInfo> CSQuestionInfos { get; set; }
        IDbSet<FinishedTask> FinishedTasks { get; set; }
        IDbSet<Notification> Notifications { get; set; }

        IDbSet<UserLicense> UserLicensesHistory { get; set; }
        IDbSet<FailTransaction> FailTransactions { get; set; }
        IDbSet<NovaPaymentHistory> NovaPaymentHistories { get; set; }
        IDbSet<UserQuizSchedule> UserQuizShcedules { get; set; }
        IDbSet<EndOfTheDay> EndOfTheDays { get; set; }
        IDbSet<AboutNebula> AboutNebulas { get; set; }
        IDbSet<Teacher> Teachers { get; set; }
        IDbSet<TeacherStudent> TeacherStudents { get; set; }
        IDbSet<Pocket> Prices { get; set; }
        IDbSet<Cupon> Cupons { get; set; }
        IDbSet<UserCupon> UserCupons { get; set; }
        IDbSet<UserCuponTry> UserCuponTries { get; set; }
        IDbSet<ExplantationVideo> ExplantationVideos { get; set; }
        IDbSet<Competition> Competitions { get; set; }
        IDbSet<GeoAnswer> GeoAnswers { get; set; }
        IDbSet<GeoAuthor> GeoAuthors { get; set; }
        IDbSet<GeoAuthorWork> GeoAuthorWorks { get; set; }
        IDbSet<GeoQuestion> GeoQuestions { get; set; }
        IDbSet<GeoUserAnswer> GeoUserAnswers { get; set; }
        IDbSet<GeoVideo> GeoVideos { get; set; } 
        IDbSet<GeoGrammarTag> GeoGrammarTags { get; set; }
        IDbSet<GeoGrammarSubTag> GeoGrammarSubTags { get; set; }
        IDbSet<GeoAuthorWorkPart> GeoAuthorWorkParts { get; set; } 
        IDbSet<GeoFictionText> GeoFictionTexts { get; set; }  
        IDbSet<GeoUserExam> GeoUserExams { get; set; }
        IDbSet<QuestionsForBook> QuestionsForBooks { get; set; }
        IDbSet<BookCode> BookCodes { get; set; }    


        DbContextConfiguration Configuration { get; }

        Task<int> SaveChangesAsync();

        //DbSet Set(Type entityType);

        IDbSet<TEntity> Set<TEntity>() where TEntity : class;

        int SaveChanges();

        IEnumerable<DbEntityValidationResult> GetValidationErrors();

        DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;

        DbEntityEntry Entry(object entity); 
    }
}
