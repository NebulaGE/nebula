 

namespace Nebula.Domain.Entities
{
    public class Answer
    {
        public int Id { get; set; }
        public string AnswerString { get; set; }
        public bool IsCorrect { get; set; }        
        public int QuestionId { get; set; }
        public CSQuestion Question { get; set; }
    ///    public ICollection<User> Users { get; set; }

    }
}