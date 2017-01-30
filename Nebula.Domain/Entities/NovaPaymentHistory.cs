using System; 

namespace Nebula.Domain.Entities
{
    public class NovaPaymentHistory
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public DateTime BuyDate { get; set; }
        public DateTime LincenseAndDate { get; set; }
        public bool Payed { get; set; }
        public int Price { get; set; }
        public string TransId { get; set; } 
    }
}