using System;
using System.Collections.Generic; 

namespace Nebula.Domain.Entities
{
    public class GeoAuthorWork
    {
        public GeoAuthorWork()
        {
            GeoAuthorWorkParts = new HashSet<GeoAuthorWorkPart>();
        }

        public int Id { get; set; }

        public string Title { get; set; } 

        public int AuthorId { get; set; }   
        
        public GeoAuthor Author { get; set; }

        public ICollection<GeoAuthorWorkPart> GeoAuthorWorkParts { get; private set; } 
    }
}
