 
namespace Nebula.Domain.Entities
{
    public class FinishedTask
    {
        public int Id { get; set; }

        public int TaskId { get; set; }
        public Condition Task { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }

        public int ModuleId { get; set; }
        public Module Module { get; set; }

        public bool IsFinished { get; set; }
    }
}