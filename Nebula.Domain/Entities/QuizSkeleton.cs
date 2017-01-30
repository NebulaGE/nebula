 
namespace Nebula.Domain.Entities
{
    public class QuizSkeleton
    {
        public int Id { get; set; }
        public int ModuleId { get; set; }
        public Module Module { get; set; }
        public byte CSQuestionCategoryId { get; set; }
        public CSQuestionCategory CSQuestionCategory { get; set; }
        public byte QuestionCount { get; set; }
        public byte Ordering { get; set; }
        public bool IsTask { get; set; } 
    }
}