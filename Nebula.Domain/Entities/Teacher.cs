 
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations; 

namespace Nebula.Domain.Entities
{
    public class Teacher
    {
        public int Id { get; set; }
      

        [MaxLength(40)]

        public string PhoneNumber { get; set; }

        [MaxLength(40)]
        [Required(ErrorMessage = "სახელი სავალდებულოა")]
        public string FirstName { get; set; }

        [MaxLength(40)]
        [Required(ErrorMessage="გვარი სავალდებულოა")]
        public string LastName { get; set; }

     
     
        [MaxLength(100)]
        public string Email { get; set; }

     
        [Required(ErrorMessage = "პაროლი სავალდებულოა")]
        public string Password { get; set; }

        [MaxLength(11)]  
        public string IdentificationNumber { get; set; }

        public ICollection<TeacherStudent> Students { get; private set; }
        public Teacher()
        {
            Students = new HashSet<TeacherStudent>();
        }

    }
}