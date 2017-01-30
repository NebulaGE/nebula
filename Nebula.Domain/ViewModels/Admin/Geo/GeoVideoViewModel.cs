using Nebula.Domain.Entities; 
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Nebula.Domain.ViewModels.Admin.Geo
{
    public class GeoVideoViewModel
    { 
        public int Id { get; set; }

        public string Title { get; set; }

        [Required]
        public string FileLink { get; set; }

        public byte Category { get; set; }

        public int? TagId { get; set; }

        public int? AuthorWorkPartId { get; set; } 

        public string TagName { get; set; }

        public string WorkTitle { get; set; } 

        public string PartName { get; set; }

        public string SubTagName { get; set; }
    }
}
