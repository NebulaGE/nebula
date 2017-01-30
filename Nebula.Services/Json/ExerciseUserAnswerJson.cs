using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nebula.Services.Json
{
    public class ExerciseUserAnswerJson
    {
        public int answerId { get; set; }
        public int questionId { get; set; }
        public int? correctAnswerId { get; set; }
    }
}
