using Nebula.Domain.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nebula.Domain.Entities
{
    public class GeoUserExam
    {
        public int Id { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }

        public string TextEditing { get; set; }
        public int GeoTextEditingId { get; set; }
        public GeoTextEditing GeoTextEditing { get; set; }

        public string Essay { get; set; }
        public int GeoEssayId { get; set; }
        public GeoEssay GeoEssay { get; set; }
        public string UserTheme { get; set; }

        public int PoetryId { get; set; }
        public GeoFictionText Poetry { get; set; }

        public int ProseId { get; set; }
        public GeoFictionText Prose { get; set; }

        public GeoUserChoosenTheme ChoosenTheme { get; set; }

        public bool IsFinished { get; set; }

        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }


        public double GetRemainingSeconds(bool @continue)
        {
            return @continue ?  Config.GeoExamDuration -  DateTime.Now.Subtract(StartTime).TotalSeconds : Config.GeoExamDuration;
        }

        public bool Timeout()
        {
            if (DateTime.Now.Subtract(StartTime).TotalSeconds > Config.GeoExamDuration)
                return true;
                   
            return false;
        }

    }

    public enum GeoUserChoosenTheme
    {
        None = 0,
        Peotry = 1,
        Prose = 2     
    }

   
}
