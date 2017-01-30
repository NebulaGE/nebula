using System; 
using System.Web.Mvc;
using System.Web.Routing;
using Nebula.Domain.Abstract;
using Nebula.Domain.Concrete;
using Nebula.Domain.Entities;
using Nebula.Domain.Repositories;
using Ninject;
using Ninject.Web.Common;

namespace Nebula.Web.Infrastructure
{
    public class NinjectControllerFactory : DefaultControllerFactory
    {
        private readonly IKernel ninjectKernel;

        public NinjectControllerFactory()
        {
            ninjectKernel = new StandardKernel();
            AddBindings();
        }

        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            return controllerType == null ? null : (IController) ninjectKernel.Get(controllerType);
        }

        private void AddBindings()
        {
            ninjectKernel.Bind<IDbContextNebula>().To<NebulaDbContext>().InRequestScope();

            ninjectKernel.Bind<BaseRepository<Answer>>().To<AnswerRepo>();
            ninjectKernel.Bind<BaseRepository<AboutNebula>>().To<AboutNebulaRepo>();
            ninjectKernel.Bind<BaseRepository<Competition>>().To<CompetitionRepo>();
            ninjectKernel.Bind<BaseRepository<Condition>>().To<ConditionRepo>();
            ninjectKernel.Bind<BaseRepository<CSQuestion>>().To<CSQuestionRepo>();
            ninjectKernel.Bind<BaseRepository<CSQuestionCategory>>().To<CSQuestionCategoryRepo>();
            ninjectKernel.Bind<BaseRepository<CSQuestionInfo>>().To<CSQuestionInfoRepo>();
            ninjectKernel.Bind<BaseRepository<CSQuiz>>().To<CSQuizRepo>();
            ninjectKernel.Bind<BaseRepository<CSUserQuizzes>>().To<CSUserQuizzesRepo>();
            ninjectKernel.Bind<BaseRepository<Cupon>>().To<CuponRepo>();
            ninjectKernel.Bind<BaseRepository<EndOfTheDay>>().To<EndOfTheDayRepo>();
            ninjectKernel.Bind<BaseRepository<ExamType>>().To<ExamTypeRepo>();
            ninjectKernel.Bind<BaseRepository<ExplantationVideo>>().To<ExplantationVideoRepo>();
            ninjectKernel.Bind<BaseRepository<FailTransaction>>().To<FailTransactionRepo>();
            ninjectKernel.Bind<BaseRepository<FinishedTask>>().To<FinishedTaskRepo>();
            ninjectKernel.Bind<BaseRepository<Image>>().To<ImageRepo>();
            ninjectKernel.Bind<BaseRepository<LastOpenExerciseQuestion>>().To<LastOpenExerciseQuestionRepo>();
            ninjectKernel.Bind<BaseRepository<MathQuiz>>().To<MathQuizRepo>();
            ninjectKernel.Bind<BaseRepository<Module>>().To<ModuleRepo>(); 
            ninjectKernel.Bind<BaseRepository<Notification>>().To<NotificationRepo>();
            ninjectKernel.Bind<BaseRepository<NovaPaymentHistory>>().To<NovaPaymentHistoryRepo>();
            ninjectKernel.Bind<BaseRepository<Pocket>>().To<PocketRepo>();
            ninjectKernel.Bind<BaseRepository<QuestionTag>>().To<QuestionTagRepo>();
            ninjectKernel.Bind<BaseRepository<QuizSkeleton>>().To<QuizSkeletonRepo>();
            ninjectKernel.Bind<BaseRepository<CSTag>>().To<TagRepo>();
            ninjectKernel.Bind<BaseRepository<TaggingHierarchy>>().To<TaggingHierarchyRepo>();
            ninjectKernel.Bind<BaseRepository<Teacher>>().To<TeacherRepo>();
            ninjectKernel.Bind<BaseRepository<TeacherStudent>>().To<TeacherStudentRepo>();
            ninjectKernel.Bind<BaseRepository<TimeUserQuestion>>().To<TimeUserQuestionRepo>();
            ninjectKernel.Bind<BaseRepository<User>>().To<UserRepo>();
            ninjectKernel.Bind<BaseRepository<UserAnswer>>().To<UserAnswerRepo>();
            ninjectKernel.Bind<BaseRepository<UserCupon>>().To<UserCuponRepo>();
            ninjectKernel.Bind<BaseRepository<UserCuponTry>>().To<UserCuponTryRepo>();
            ninjectKernel.Bind<BaseRepository<UserLicense>>().To<UserLicenseRepo>();
            ninjectKernel.Bind<BaseRepository<UserQuizSchedule>>().To<UserQuizScheduleRepo>();
            ninjectKernel.Bind<BaseRepository<VerbalQuiz>>().To<VerbalQuizRepo>();
            ninjectKernel.Bind<BaseRepository<Video>>().To<VideoRepo>();
            ninjectKernel.Bind<BaseRepository<VideoExercise>>().To<VideoExerciseRepo>();
            ninjectKernel.Bind<BaseRepository<VideoPart>>().To<VideoPartRepo>();
            ninjectKernel.Bind<BaseRepository<GeoQuestion>>().To<GeoQuestionRepo>();
            ninjectKernel.Bind<BaseRepository<GeoAuthor>>().To<GeoAuthorRepo>();
            ninjectKernel.Bind<BaseRepository<GeoAuthorWork>>().To<GeoAuthorWorkRepo>();
            ninjectKernel.Bind<BaseRepository<GeoAuthorWorkPart>>().To<GeoAuthorWorkPartRepo>();
            ninjectKernel.Bind<BaseRepository<GeoVideo>>().To<GeoVideoRepo>();
            ninjectKernel.Bind<BaseRepository<GeoUserAnswer>>().To<UserGeoAnswerRepo>();
            ninjectKernel.Bind<BaseRepository<GeoGrammarTag>>().To<GeoGrammarTagRepo>();
            ninjectKernel.Bind<BaseRepository<GeoGrammarSubTag>>().To<GeoGrammarSubTagRepo>();
            ninjectKernel.Bind<BaseRepository<GeoEssay>>().To<GeoEssayRepo>();
            ninjectKernel.Bind<BaseRepository<GeoFictionText>>().To<GeoFictionTextRepo>();  
            ninjectKernel.Bind<BaseRepository<GeoUserExam>>().To<GeoUserExamRepo>();
            ninjectKernel.Bind<BaseRepository<GeoTextEditing>>().To<GeoTextEditingRepo>();
            ninjectKernel.Bind<BaseRepository<GeoAnswer>>().To<GeoAnswerRepo>();
            ninjectKernel.Bind<BaseRepository<News>>().To<NewsRepo>();
        }
    }
}