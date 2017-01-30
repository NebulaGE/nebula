using System.Collections.Generic;

namespace Nebula.Domain.Entities
{
    public class GeoAuthorWorkPart 
    {
        public GeoAuthorWorkPart()
        {
            GeoVideos = new HashSet<GeoVideo>();
            Questions = new HashSet<GeoQuestion>(); 
        }

        public int Id { get; set; }

        public string PartName { get; set; }    

        public int AuthorWorkId { get; set; }

        public GeoAuthorWork AuthorWork { get; set; }

        public ICollection<GeoVideo> GeoVideos { get; set; }

        public ICollection<GeoQuestion> Questions { get; private set; } 
    }
}
