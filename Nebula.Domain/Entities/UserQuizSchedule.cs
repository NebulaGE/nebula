using System; 

namespace Nebula.Domain.Entities
{
    public class UserQuizSchedule
    {
        public int Id { get; set; }
        public int QuizNum { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public DateTime OpenDate { get; set; }
    }
}