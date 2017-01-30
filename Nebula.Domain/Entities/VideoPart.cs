 
using System.Collections.Generic; 

namespace Nebula.Domain.Entities
{
    public class VideoPart
    {
        public int Id { get; set; }        
        public int Second { get; set; }
        public int VideoId { get; set; }
        public Video Video { get; set; }      
        public ICollection<CSQuestion> Questions { get; private set; }
        public VideoPart()
        {
            Questions = new HashSet<CSQuestion>();
        }
    }
}
