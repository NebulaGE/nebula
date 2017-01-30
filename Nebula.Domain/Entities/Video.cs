 
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema; 
using System.Web;

namespace Nebula.Domain.Entities
{
    public class Video
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string FileName { get; set; }        
        public string AlterneteLink { get; set; }
        public int TagId { get; set; }
        public CSTag  CSTag { get; set; }
        public bool IsExerciseType { get; set; }

        public ICollection<VideoPart> VideoParts { get; set; } 
        public ICollection<CSQuestion> QuizQuestions { get; set; }       
         
        public byte Numeration { get; set; }

        [NotMapped]
        public HttpPostedFileBase VideoFile { get; set; }
    }
}