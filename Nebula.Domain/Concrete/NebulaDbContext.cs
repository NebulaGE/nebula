using System.Data.Entity;
using Nebula.Domain.Abstract;
using Nebula.Domain.Entities;
using Microsoft.AspNet.Identity.EntityFramework;
using Nebula.Domain.EntityConfigurations; 

namespace Nebula.Domain.Concrete
{  
    public class NebulaDbContext : IdentityDbContext<User>, IDbContextNebula
    {  
        public static NebulaDbContext Create()
        { 
            return new NebulaDbContext();
        }
        
        public NebulaDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
            //Database.SetInitializer<NebulaDbContext>(new CreateDatabaseIfNotExists<NebulaDbContext>());
        } 

        public IDbSet<Answer> Answers { get; set; }
        public IDbSet<UserAnswer> UserAnswers { get; set; }
        public IDbSet<CSQuestion> CSQuestions { get; set; }
        public IDbSet<CSQuiz> Quizes { get; set; }
        public IDbSet<CSUserQuizzes> UserQuizes { get; set; }
        public IDbSet<CSQuestionCategory> CSQuestionCategories { get; set; }
        public IDbSet<Module> Modules { get; set; }
        public IDbSet<CSTag> Tags { get; set; }
        public IDbSet<ExamType> ExamTypes { get; set; }
        public IDbSet<MathQuiz> MathQuizes { get; set; }
        public IDbSet<VerbalQuiz> VerbalQuizes { get; set; }
        public IDbSet<Video> Videos { get; set; }
        public IDbSet<VideoPart> VideoParts { get; set; }
        public IDbSet<Entities.Condition> Conditions { get; set; }
        public IDbSet<QuizSkeleton> QuizSkeletions { get; set; }
        public IDbSet<QuestionTag> QuestionTags { get; set; }
        public IDbSet<Image> Images { get; set; }
        public IDbSet<TaggingHierarchy> TaggingHierarchies { get; set; } 
        public IDbSet<CSQuestionInfo> CSQuestionInfos { get; set; } 
        public IDbSet<FinishedTask> FinishedTasks { get; set; } 
        public IDbSet<Notification> Notifications { get; set; } 
        public IDbSet<UserLicense> UserLicensesHistory { get; set; }
        public IDbSet<FailTransaction> FailTransactions { get; set; }
        public IDbSet<NovaPaymentHistory> NovaPaymentHistories { get; set; }
        public IDbSet<UserQuizSchedule> UserQuizShcedules { get; set; }
        public IDbSet<EndOfTheDay> EndOfTheDays { get; set; }
        public IDbSet<AboutNebula> AboutNebulas { get; set; }
        public IDbSet<Teacher> Teachers { get; set; }
        public IDbSet<TeacherStudent> TeacherStudents { get; set; }
        public IDbSet<Pocket> Prices { get; set; }
        public IDbSet<Cupon> Cupons { get; set; }
        public IDbSet<UserCupon> UserCupons { get; set; }
        public IDbSet<UserCuponTry> UserCuponTries { get; set; }
        public IDbSet<ExplantationVideo> ExplantationVideos { get; set; }
        public IDbSet<Competition> Competitions { get; set; }
        public IDbSet<GeoAnswer> GeoAnswers { get; set; }
        public IDbSet<GeoAuthor> GeoAuthors { get; set; }
        public IDbSet<GeoAuthorWork> GeoAuthorWorks { get; set; }
        public IDbSet<GeoQuestion> GeoQuestions { get; set; }
        public IDbSet<GeoUserAnswer> GeoUserAnswers { get; set; }
        public IDbSet<GeoVideo> GeoVideos { get; set; }  
        public IDbSet<GeoGrammarTag> GeoGrammarTags { get; set; } 
        public IDbSet<GeoGrammarSubTag> GeoGrammarSubTags { get; set; }
        public IDbSet<GeoAuthorWorkPart> GeoAuthorWorkParts { get; set; }
        public IDbSet<News> News{ get; set; }
        public IDbSet<GeoFictionText> GeoFictionTexts { get; set; } 
        public IDbSet<GeoUserExam> GeoUserExams { get; set; }
        public IDbSet<QuestionsForBook> QuestionsForBooks { get; set; }
        public IDbSet<BookCode> BookCodes { get; set; }

        public new IDbSet<TEntity> Set<TEntity>() where TEntity : class
        {
            return base.Set<TEntity>();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        { 
            modelBuilder.Configurations.Add(new AnswerConfig());
            modelBuilder.Configurations.Add(new CSQuestionConfig());
            modelBuilder.Configurations.Add(new CSQuizConfig());
            modelBuilder.Configurations.Add(new CSQuestionCategoryConfig());
            modelBuilder.Configurations.Add(new ModuleConfig());
            modelBuilder.Configurations.Add(new ExamTypeConfig());
            modelBuilder.Configurations.Add(new VideoPartConfig());
            modelBuilder.Configurations.Add(new UserQuizesConfig());
            modelBuilder.Configurations.Add(new VideoConfig());
            modelBuilder.Configurations.Add(new TagConfig());
            modelBuilder.Configurations.Add(new FinishedTaskConfig());
            modelBuilder.Configurations.Add(new NotificationConfig()); 
            modelBuilder.Configurations.Add(new UserLicenseConfig());
            modelBuilder.Configurations.Add(new NovaPaymentHistoryConfig());
            modelBuilder.Configurations.Add(new UserQuizScheduleConfig());
            modelBuilder.Configurations.Add(new CuponConfig());
            modelBuilder.Configurations.Add(new GeoAuthorWorkConfig());
            modelBuilder.Configurations.Add(new GeoAuthorWorkPartConfig());
            modelBuilder.Configurations.Add(new GeoGrammarSubTagConfig());            
            modelBuilder.Configurations.Add(new GeoQuestionConfig());
            modelBuilder.Configurations.Add(new GeoVideoConfig());
            modelBuilder.Configurations.Add(new GeoAnswerConfig());
            modelBuilder.Configurations.Add(new UserGeoAnswerConfig()); 
            modelBuilder.Configurations.Add(new GeoUserExamConfig());

            base.OnModelCreating(modelBuilder);
        }

        public System.Data.Entity.DbSet<Nebula.Domain.Entities.GeoEssay> GeoEssays { get; set; }

        public System.Data.Entity.DbSet<Nebula.Domain.Entities.GeoTextEditing> GeoTextEditings { get; set; } 
    }
}
