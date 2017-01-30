 
using System.ComponentModel.DataAnnotations.Schema; 

namespace Nebula.Domain.Entities
{
    public class QuestionTag
    {
        public int Id { get; set; }

        [ForeignKey("CSTag")]
        public int TagId { get; set; }
        public CSTag CSTag { get; set; }

        [ForeignKey("Question")]
        public int QuestionId { get; set; }
        public CSQuestion Question { get; set; }

        public int? VoteCount { get; set; }
        public bool IsDeleted { get; set; }

    }
}