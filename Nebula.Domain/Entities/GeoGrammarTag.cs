using System.Collections.Generic; 

namespace Nebula.Domain.Entities
{
    public class GeoGrammarTag
    {
        public GeoGrammarTag()
        {
            GeoGrammarSubTags = new HashSet<GeoGrammarSubTag>();
        }

        public int Id { get; set; }

        public string TagName { get; set; }

        public bool IsPayed { get; set; }

        public ICollection<GeoGrammarSubTag> GeoGrammarSubTags { get; private set; } 
    }
}
