using System.Collections.Generic; 

namespace Nebula.Domain.Entities
{
    public class GeoFictionText
    {
        public GeoFictionText()
        {
            Questions = new HashSet<GeoQuestion>(); 
        }

        public int Id { get; set; }

        public string Title { get; set; }
        public string Text { get; set; }
        public GeoThemeType ThemeType { get; set; }

        public string PointOne { get; set; }
        public string PointTwo { get; set; }
        public string PointThree { get; set; }

        public ICollection<GeoQuestion> Questions { get; set; }
    } 

    public enum GeoThemeType
    {
        Poetry = 1,
        Prose = 2
    }
}
