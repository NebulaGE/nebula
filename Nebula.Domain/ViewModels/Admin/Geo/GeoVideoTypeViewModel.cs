using System.Collections.Generic; 
using Nebula.Domain.Entities;

namespace Nebula.Domain.ViewModels.Admin.Geo
{
    public class GeoVideoTypeViewModel
    {
        public List<GeoAuthorWork> Work { get; set; }

        public List<GeoGrammarTag> Tag { get; set; } 
    }
}
