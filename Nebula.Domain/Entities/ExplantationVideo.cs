 
using System.ComponentModel.DataAnnotations.Schema; 
using System.Web;

namespace Nebula.Domain.Entities
{
    public class ExplantationVideo
    {
        public int Id { get; set; }
        public string Name { get; set; } 
        public string FileName { get; set; }
        [NotMapped]
        public HttpPostedFileBase Video { get; set; }
    }
}