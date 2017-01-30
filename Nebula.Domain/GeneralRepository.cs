using Nebula.Domain.Abstract;
using Nebula.Domain.Entities;
using Nebula.Domain.Repositories;

namespace Nebula.Domain
{
    public class GeneralRepository
    {    
        public BaseRepository<AboutNebula> AboutNebula { get; set; }
        public BaseRepository<Answer> Answer { get; set; }
        public BaseRepository<Competition> Competition { get; set; }
        public BaseRepository<Condition> Condition { get; set; }
        public BaseRepository<CSQuestionCategory> CsQuestionCategory { get; set; }
        public BaseRepository<CSQuestion> CsQuestion { get; set; }
        public BaseRepository<CSQuiz> CsQuiz { get; set; }
        public BaseRepository<CSUserQuizzes> CsUserQuizzes { get; set; }
        public BaseRepository<Cupon> Cupon { get; set; }
        public BaseRepository<EndOfTheDay> EndOfTheDay { get; set; }
        public BaseRepository<ExamType> ExamType { get; set; }
        public BaseRepository<ExplantationVideo> ExplantationVideo { get; set; }
        public BaseRepository<FinishedTask> FinishedTask { get; set; }
        public BaseRepository<Image> Image { get; set; }
        public BaseRepository<LastOpenExerciseQuestion> LastOpenExerciseQuestion { get; set; }
        public BaseRepository<MathQuiz> MathQuiz { get; set; }
        public BaseRepository<Module> Module { get; set; } 
        public BaseRepository<Notification> Notification { get; set; }
        public BaseRepository<NovaPaymentHistory> NovaPaymentHistory { get; set; }
        public BaseRepository<Pocket> Pocket { get; set; }
        public BaseRepository<QuestionTag> QuestionTag { get; set; }
        public BaseRepository<QuizSkeleton> QuizSkeleton { get; set; }
        public BaseRepository<TaggingHierarchy> TaggingHierarchy { get; set; }
        public BaseRepository<CSTag> Tag { get; set; }
        public BaseRepository<Teacher> Teacher { get; set; }
        public BaseRepository<TeacherStudent> TeacherStudent { get; set; }
        public BaseRepository<TimeUserQuestion> TimeUserQuestion { get; set; }
        public BaseRepository<UserAnswer> UserAnswer { get; set; }
        public BaseRepository<UserCupon> UserCupon { get; set; }
        public BaseRepository<UserCuponTry> UserCuponTry { get; set; }
        public BaseRepository<UserLicense> UserLicense { get; set; }
        public BaseRepository<UserQuizSchedule> UserQuizSchedule { get; set; }
        public BaseRepository<User> User { get; set; }
        public BaseRepository<VerbalQuiz> VerbalQuiz { get; set; }
        public BaseRepository<VideoExercise> VideoExercise { get; set; }
        public BaseRepository<VideoPart> VideoPart { get; set; }
        public BaseRepository<Video> Video { get; set; }
        public BaseRepository<CSQuestionInfo> CSQuestionInfo { get; set; }
        public BaseRepository<GeoQuestion> GeoQuestion { get; set; }
        public BaseRepository<GeoVideo> GeoVideo { get; set; }
        public BaseRepository<GeoAuthorWork> GeoAuthorWork { get; set; }
        public BaseRepository<GeoAuthorWorkPart> GeoAuthorWorkPart { get; set; }
        public BaseRepository<GeoAuthor> GeoAuthor { get; set; }
        public BaseRepository<GeoAnswer> GeoAnswer { get; set; }
        public BaseRepository<GeoUserAnswer> GeoUserAnswer { get; set; } 
        public BaseRepository<GeoGrammarTag> GeoGrammarTag { get; set; }
        public BaseRepository<GeoGrammarSubTag> GeoGrammarSubTag { get; set; }
        public BaseRepository<GeoFictionText> GeoFictionText { get; set; } 
        public BaseRepository<GeoUserExam> GeoUserExam { get; set; }
        public BaseRepository<GeoEssay> GeoEssay { get; set; }
        public BaseRepository<GeoTextEditing> GeoTextEditing { get; set; }
    }
}
