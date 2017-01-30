using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nebula.Services.Json
{
    public class ExerciseQuestionJson
    {
        public ExerciseQuestionJson()
        {
            answers = new List<ExerciseAnswerJson>();
        }

        public int id { get; set; }
        public string text { get; set; }
        public string explanationVideo { get; set; }    
        public bool hasAnswered { get; set; }
        public List<ExerciseAnswerJson> answers { get; set; }
    }
}

