 

namespace Nebula.Domain.Entities
{
    public class LastOpenExerciseQuestion
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }

        public int QuestionId { get; set; }
        public CSQuestion Question { get; set; }

        public int ModuleId { get; set; }
        public Module Module { get; set; }
       
       
    }
}