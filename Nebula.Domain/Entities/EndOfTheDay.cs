using System; 

namespace Nebula.Domain.Entities
{
    public class EndOfTheDay
    {
        public int Id { get; set; }
        public string Status { get; set; }
        public DateTime TriggerTime { get; set; }
    }
}