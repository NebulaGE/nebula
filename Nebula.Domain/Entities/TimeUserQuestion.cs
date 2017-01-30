using System; 

namespace Nebula.Domain.Entities
{
    public class TimeUserQuestion
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }

        public int QuestionId { get; set; }
        public CSQuestion Question { get; set; }

        public int QuizId { get; set; }
        public CSQuiz Quiz { get; set; }

        public DateTime OpenDate { get; set; }
        public DateTime? CloseDate { get; set; } 
    }
}