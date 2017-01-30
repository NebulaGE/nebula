using System;
using System.Collections.Generic; 

namespace Nebula.Domain.Entities
{
    public class GeoQuestion
    {
        public GeoQuestion()
        {
            Answers = new HashSet<GeoAnswer>();
            CreateTime = DateTime.Now;
        }

        public int Id { get; set; }

        public string Text { get; set; }
          
        public int CorrectAnswerId { get; set; }

        public ICollection<GeoAnswer> Answers { get; private set; }

        public int? GeoAuthorWorkPartId { get; set; } 
        public GeoAuthorWorkPart GeoAuthorWorkPart { get; set; }

        public int? GeoGrammarSubTagId { get; set; }
        public GeoGrammarSubTag GeoGrammarSubTag { get; set; }

        public int? GeoFictionTextId { get; set; }
        public GeoFictionText GeoFictionText { get; set; }

        public GeoQuestionType QuestionType { get; set; } 

        public GeoQuestionCategory Category { get; set; }

        public DateTime CreateTime { get; private set; }

        public bool IsFreeQuestion { get; set; }

        public string ExplanationVideoLink { get; set; }

        public string GetAuthorName()
        {
            return GeoAuthorWorkPart?.AuthorWork?.Author?.AuthorName;
        }
    }

    public enum GeoQuestionType
    {
        ExamQuestion = 1, 
        ExerciseQuestion = 2,
        PostQuestion = 3, 
        PreQuestion = 4,
        CatQuestion = 5
       
    }

    public enum GeoQuestionCategory
    {
        Literature = 1,
        Grammar = 2
    }
}
