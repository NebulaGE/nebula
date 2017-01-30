using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nebula.Domain.Entities
{
    public class GeoTextEditing
    {
        public int Id { get; set; }
        public string IncorrectText { get; set; }
        public string CorrectText { get; set; }
    }
}
