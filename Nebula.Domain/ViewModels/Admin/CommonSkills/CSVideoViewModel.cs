using Nebula.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Nebula.Domain.ViewModels.Admin.CommonSkills
{
    public class CSVideoViewModel
    {
        public CSVideoViewModel()
        {
            Tags = new List<CSTagViewModel>();
            Parts = new List<VideoPart>();
        }
        public int Id { get; set; }

        [Display(Name="სავარჯიშოა?")]
        public bool IsExercise { get; set; }
        public int TagId { get; set; }
       
        [Required]
        public string Name { get; set; }
        [Required]
        public int Order { get; set; }

        public HttpPostedFileBase VideoFile { get; set; }

        public string AlternateLink { get; set; }

        public List<CSTagViewModel> Tags { get; set; }

        public List<VideoPart> Parts { get; set; }

    }
}
