 
using System.Collections.Generic; 

namespace Nebula.Domain.Entities
{
    public class Module
    {
        public Module()
        {
            Questions = new HashSet<CSQuestion>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public byte CSQuestionCategoryId { get; set; }
   //     public ICollection<ModulTags> ModulTags { get; set; }
        public CSQuestionCategory CSQuestionCategory { get; set; }
        public ICollection<CSQuestion> Questions { get; set; }
        public bool? HasMultipleQuestions { get; set; }
    }
}