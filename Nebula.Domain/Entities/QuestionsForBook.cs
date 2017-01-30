using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nebula.Domain.Entities
{
    public class QuestionsForBook
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
        public ICollection<QuestionTag> QuestionTags { get;  set; }
        public int? ConditionId { get; set; }
        public Condition Condition { get; set; }
        public ICollection<Answer> Answers { get;  set; }
        public int? VideoId { get; set; }
        public Video Video { get; set; }
        public int? VideoPartId { get; set; }
        public VideoPart VideoPart { get; set; }
        public CSQuestionType? QuestionType { get; set; }
        public bool IsWordAnalog { get; set; }
        public DateTime? CreateTime { get;  set; }

        public QuestionsForBook()
        {
            QuestionTags = new HashSet<QuestionTag>();
            Answers = new HashSet<Answer>();
            CreateTime = DateTime.Now;
        } 
    }
}
