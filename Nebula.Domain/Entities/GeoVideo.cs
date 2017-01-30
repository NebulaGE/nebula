using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nebula.Domain.Entities
{
    public class GeoVideo
    {  
        public int Id { get; set; }

        public string Name { get; set; }
        public string FileLink { get; set; } 

        public int? GeoAuthorWorkPartId { get; set; }

        public int? GeoGrammarSubTagId { get; set; }

        public GeoGrammarSubTag GeoGrammarSubTag { get; set; }

        public GeoAuthorWorkPart GeoAuthorWorkPart { get; set; }  
    }
}
