 using System.Collections.Generic; 

namespace Nebula.Domain.Entities
{
    public class CSTag
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public byte QuestionTypeId { get; set; }
        public bool IsPayed { get; set; }
        public CSQuestionCategory QuestionType { get; set; }
   //     public ICollection<ModulTags> ModulTags { get; set; }
        public ICollection<QuestionTag> QuestionTags { get; private set; }
        public ICollection<Video> Videos { get; private set; }
        public CSTag()
        {
            QuestionTags = new HashSet<QuestionTag>();
            Videos = new HashSet<Video>();
        }
      
    }
}