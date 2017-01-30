using System.Collections.Generic;
using System.ComponentModel.DataAnnotations; 

namespace Nebula.Domain.ViewModels.Admin.Geo
{
    public class GeoFictionTextViewModel
    {
        public GeoFictionTextViewModel()
        {
            Questions = new List<GeoQuestionViewModel>(); 
        }

        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Text { get; set; }

        public int ThemeType { get; set; } 

        [Required]
        public string PointOne { get; set; }

        [Required]
        public string PointTwo { get; set; }

        [Required]
        public string PointThree { get; set; }

        public List<GeoQuestionViewModel> Questions { get; set; }

        public GeoQuestionViewModel QuestionCreateViewModel { get; set; }
    }
}
