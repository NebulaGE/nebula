 

namespace Nebula.Domain.Entities
{
    public class TeacherStudent
    {
        public int Id { get; set; }
      
        public int TeacherId { get; set; }
        public Teacher Teacher { get; set; }

        public string StudentId { get; set; }
        public User Student { get; set; }

        public bool IsConfirmed { get; set; }
    }
}