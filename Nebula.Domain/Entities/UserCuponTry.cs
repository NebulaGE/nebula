using System; 
using System.ComponentModel.DataAnnotations.Schema; 

namespace Nebula.Domain.Entities
{
    public class UserCuponTry
    {
        public int Id { get; set; }
            
        public DateTime TryDate { get; set; }
        public DateTime LockDate { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }
        public User User { get; set; } 
    }
}