using System; 
using System.ComponentModel.DataAnnotations.Schema; 

namespace Nebula.Domain.Entities
{
    public class UserCupon
    {
        public int Id { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }
        public User User { get; set; }

        [ForeignKey("Cupon")]
        public int CuponId { get; set; }
        public Cupon Cupon { get; set; }
        public DateTime ActivationDate { get; set; }
        public DateTime? ExpireDate { get; set; }

    }
}