using System.Collections.Generic;
using System.ComponentModel.DataAnnotations; 
using Nebula.Domain.Entities;

namespace Nebula.Domain.ViewModels.Admin.Geo
{
    public class GeoQuestionViewModel
    {
        public GeoQuestionViewModel()
        {
            Authors = new List<GeoAuthor>();
            Tags = new List<GeoGrammarTag>();
        }

        public int Id  { get; set; }

        [Required]
        public string Text { get; set; }

        public string VideoExplanationLink { get; set; }

        public bool IsFreeQuestion { get; set; }    

        public string Controller { get; set; }
        public string Action { get; set; }

        public byte CategoryId { get; set; }

        [Required]
        public int CorrectAnswerId { get; set; }
          
        public List<AnswerViewModel> Answers { get; set; }

        [Required] 
        public GeoQuestionType QuestionType { get; set; }

        public GeoQuestionCategory Category { get; set; }
           
        public int? AuthorWorkPartId { get; set; } 
 
        public List<GeoAuthor> Authors { get; set; }

        public int? SubTagId { get; set; }

        public List<GeoGrammarTag> Tags { get; set; }

        public int? GeoFictionTextId { get; set; }
        public GeoFictionText GeoFictionText { get; set; }
    }
}
