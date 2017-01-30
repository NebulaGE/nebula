 
using System.Collections.Generic; 

namespace Nebula.Domain.Entities
{
    public class CSQuestionCategory
    {
        public byte Id { get; set; }
        public string Name { get; set; }
  
        public ICollection<Module> Modules { get; private set; }
        public ICollection<CSTag> Tags { get; private set; }
        public CSQuestionCategory()
        {
            Modules = new HashSet<Module>();
            Tags = new HashSet<CSTag>();
        }
    }
}