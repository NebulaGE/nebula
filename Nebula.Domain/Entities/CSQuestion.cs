using System;
using System.Collections.Generic; 

namespace Nebula.Domain.Entities
{
    public class CSQuestion
    {
        public int Id { get; set; }
        public string QuestionText { get; set; }
        public string AnswersCaption { get; set; }
        public int CorrectAnswerId { get; set; }
        public int? Num { get; set; }
        public int? CSQuestionInfoId { get; set; }
        public CSQuestionInfo CSQuestionInfo { get; set; }
        public int? ModuleId { get; set; }
        public Module Module { get; set; }
        public ICollection<QuestionTag> QuestionTags { get; private set; }
        public int? ConditionId { get; set; }
        public Condition Condition { get; set; }
        public ICollection<Answer> Answers { get; private set; }         
        public int? VideoId { get; set; }
        public Video Video { get; set; }
        public int? VideoPartId { get; set; }
        public VideoPart VideoPart { get; set; }
        public CSQuestionType? QuestionType { get; set; }
        public bool IsWordAnalog { get; set; }
        public DateTime? CreateTime { get; private set; }
        public bool IsBookQuestion { get; set; }
        public bool IsFreeQuestion { get; set; }
        public bool IsNaecQuestion { get; set; }
        public string ExplanationVideoLink { get; set; }

        public CSQuestion()
        {
            QuestionTags = new HashSet<QuestionTag>();
            Answers = new HashSet<Answer>();
            CreateTime = DateTime.Now;
        } 
    }

    public enum CSQuestionType
    {
        ExamQuestion = 1,
        ExerciseQuestion = 2,
        VideQuestion = 3
    }
}