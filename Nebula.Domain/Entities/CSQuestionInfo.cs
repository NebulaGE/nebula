 
using System.Collections.Generic; 

namespace Nebula.Domain.Entities
{
    public class CSQuestionInfo
    {
        public int Id { get; set; }
        public int Year { get; set; }
        public byte Version { get; set; }
        public ICollection<CSQuestion> Questions { get; private set; }

        public CSQuestionInfo()
        {
            Questions = new HashSet<CSQuestion>();
        }

        public string GetName()
        {
            return  Year+ " - " + Version;
        }
    }
}