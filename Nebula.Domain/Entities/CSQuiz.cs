 
using System.Collections.Generic; 

namespace Nebula.Domain.Entities
{
    public class CSQuiz
    {     
        public int Id { get; set; }      

        public ICollection<MathQuiz> MathQuizzes { get; set; }
        public ICollection<VerbalQuiz> VerbalQuizzes { get; set; }

        public CSQuiz()
        {
            MathQuizzes = new HashSet<MathQuiz>();
            VerbalQuizzes = new HashSet<VerbalQuiz>();
        }

        public void Eaaa(string aaaa)
        {
            
        }
    } 
}