using System; 
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema; 

namespace Nebula.Domain.Entities
{
    public class VerbalQuiz
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Question")]
        public int QuestionId { get; set; }
        public CSQuestion Question { get; set; }

        [ForeignKey("CSQuiz")]
        public int CSQuizId { get; set; }
        public CSQuiz CSQuiz { get; set; }

        [ForeignKey("Answer")]
        public int? AnswerId { get; set; }
        public Answer Answer { get; set; }

 
        public bool IsCorrect { get; set; }
  

        public DateTime? OpenDate { get; set; }
        public DateTime? CloseDate { get; set; }
    }
}

