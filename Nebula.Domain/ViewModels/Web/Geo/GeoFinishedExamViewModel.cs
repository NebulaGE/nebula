using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nebula.Domain.ViewModels.Web.Geo
{
    public class GeoFinishedExamViewModel
    {
        public int ExamId { get; set; }
        public string TextEditing { get; set; }
        public string Essay { get; set; }
        public string ProseTheme { get; set; }
        public string PoetryTheme { get; set; }
        public string UserTheme { get; set; }
    }
}
