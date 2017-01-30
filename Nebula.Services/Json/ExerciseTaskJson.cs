using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nebula.Services.Json
{
    public class ExerciseTaskJson
    {
        public ExerciseTaskJson()
        {
            questions = new List<ExerciseQuestionJson>();
            userAnswers = new List<ExerciseUserAnswerJson>();
        }
        public int id { get; set; }
        public string text { get; set; }
        public List<ExerciseQuestionJson> questions { get; set; }
        public List<ExerciseUserAnswerJson> userAnswers { get; set; }
    }
}
