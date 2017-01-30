 
using System.Collections.Generic; 

namespace Nebula.Domain.Entities
{
    public class Condition
    {
        public int Id { get; set; }
        public string TaskText { get; set; }
        public ICollection<CSQuestion> Questions { get; set; }

        public int? ModuleId { get; set; }
        public Module Module { get; set; }

        public int? CSQuestionInfoId { get; set; }
        public CSQuestionInfo CSQuestionInfo { get; set; }
        public int? Num { get; set; }

        public bool IsGuestCondition { get; set; }

        public TaskType? Type { get; set; }

        public Condition()
        {
            Questions = new HashSet<CSQuestion>();
        }
    }

    public enum TaskType
    {
        IsVideoTask = 1,
        IsExerciseTask = 2,
        IsExamTask = 3
    }
}