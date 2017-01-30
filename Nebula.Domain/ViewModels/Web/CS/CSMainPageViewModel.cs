using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nebula.Domain.ViewModels.Web.CS
{
    public class CSMainPageViewModel
    {
        public int VerbalVideosTagId { get; set; }
        public int MathVideosTagId { get; set; }
        public int VerbalExercisesModuleId { get; set; }
        public int MathExercisesModuleId{ get; set; }
        public int LeftDays { get; set; }
        public int? PrevQuizId { get; set; }
        public int MathType { get; set; }
        public int VerbalType { get; set; }
        public CSQuizStatus QuizStatus { get; set; }
    }

    public enum CSQuizStatus
    {
        Start = 1,
        Continue = 2,
        Locked = 3,
        NeedLicense = 4
    }
}
