using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nebula.Domain.ViewModels.Web.Geo
{
    public class GeoMainPageViewModel
    {
        public int VideoTagId { get; set; }
        public int VideoAuthorWorkId{ get; set; }

        public int ExerciseSubTagId { get; set; }
        public int ExerciseAuthorWorkId { get; set; }
        public ExamStatus ExamStatus { get; set; }
        public int? PrevExam { get; set; }

        public bool HasGeoLicense { get; set; }
    }

    public enum ExamStatus
    {
        Start = 1,
        Continue = 2,
        Disable = 3 
    }
}
