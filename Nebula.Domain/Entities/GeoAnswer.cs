
using System.Collections.Generic;

namespace Nebula.Domain.Entities
{
    public class GeoAnswer
    {
        public GeoAnswer()
        {
            GeoUserAnswers = new HashSet<GeoUserAnswer>();
        }
        public int Id { get; set; }

        public string Text { get; set; }
        public bool IsCorrect { get; set; }

        public int QuestionId { get; set; }
        public GeoQuestion Question { get; set; }

        public ICollection<GeoUserAnswer> GeoUserAnswers { get; set; }
    }
}
