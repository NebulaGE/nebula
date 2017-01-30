using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nebula.Services.Dto
{
    public class SetTaskAnswerDto
    {
        public int QuestionId { get; set; }
        public int AnswerId { get; set; }
        public int TaskId { get; set; }
    }
}
