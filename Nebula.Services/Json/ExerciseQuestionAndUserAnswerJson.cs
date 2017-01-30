using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nebula.Services.Json
{
    public class ExerciseQuestionAndUserAnswerJson
    {
        public ExerciseQuestionAndUserAnswerJson()
        {
            questions = new List<ExerciseQuestionJson>();
            userAnswers = new List<ExerciseUserAnswerJson>();
        }
        public List<ExerciseQuestionJson> questions { get; set; }
        public List<ExerciseUserAnswerJson> userAnswers { get; set; }
    }
}
