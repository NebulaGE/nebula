using System;
using System.Collections.Generic; 

namespace Nebula.Domain.Entities
{
    public class GeoAuthor
    {
        public GeoAuthor()
        {
            GeoAuthorWorks = new HashSet<GeoAuthorWork>();
        }

        public int Id { get; set; }

        public string AuthorName { get; set; } 

        public bool IsPayed { get; set; }

        public ICollection<GeoAuthorWork> GeoAuthorWorks { get; private set; }
    }
}
