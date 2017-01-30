using Nebula.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nebula.Domain.ViewModels.Web.Geo
{
    public class GeoExamViewModel
    {
        public int ExamId { get; set; }
        public GeoTextEditing TextEditing { get; set; }
        public GeoEssay Essay { get; set; }
        public GeoFictionText Poetry { get; set; }
        public GeoFictionText Prose { get; set; }
     
    }
}
