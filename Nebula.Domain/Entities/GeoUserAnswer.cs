using System; 
using System.ComponentModel.DataAnnotations.Schema; 

namespace Nebula.Domain.Entities
{
    public class GeoUserAnswer
    {
        public int Id { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }
  
        public int AnswerId { get; set; }
        public GeoAnswer Answer { get; set; }

        public DateTime AnswerDate { get; set; }
    }
}
