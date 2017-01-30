using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nebula.Services.Json
{
    public class ExerciseNavigationJson
    {
        public int id { get; set; }
        public string name { get; set; }
        public int userAnswersCount { get; set; }
        public int questionsCount { get; set; }
        public ExerciseType type { get; set; } = ExerciseType.Question;
    }

    public enum ExerciseType
    {
        Question = 1,
        Task = 2
    }
}
