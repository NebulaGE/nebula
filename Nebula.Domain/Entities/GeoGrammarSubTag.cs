
using System.Collections.Generic;

namespace Nebula.Domain.Entities
{
    public class GeoGrammarSubTag
    {
        public GeoGrammarSubTag()
        {
            Questions = new HashSet<GeoQuestion>();
            GeoVideos = new HashSet<GeoVideo>();
        }

        public int Id { get; set; }

        public string TagName { get; set; }

        public int GeoGrammarTagId { get; set; }

        public GeoGrammarTag GeoGrammarTag { get; set; }

        public ICollection<GeoVideo> GeoVideos { get; set; } 

        public ICollection<GeoQuestion> Questions { get; private set; } 
    }
}
