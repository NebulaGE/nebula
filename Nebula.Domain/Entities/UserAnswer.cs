using System; 
using System.ComponentModel.DataAnnotations.Schema; 

namespace Nebula.Domain.Entities
{
    public class UserAnswer
    {
        public int Id { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }
        public User User { get; set; }

        [ForeignKey("Answer")]
        public int AnswerId { get; set; }
        public Answer Answer { get; set; }

        //[ForeignKey("Question")]
        //public int? QuestionId { get; set; }

        //public CSQuestion Question { get; set; }

        public DateTime AnswerDate { get; set; } 
    }
}