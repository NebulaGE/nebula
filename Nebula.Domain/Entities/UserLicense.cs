using System;
using Nebula.Domain.ViewModels.Web;

namespace Nebula.Domain.Entities
{
    public class UserLicense
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public string TransactionId { get; set; }
        public DateTime BuyDate { get; set; }
        public DateTime LincenseAndDate { get; set; }
        public int Price { get; set; }
        public bool Payed { get; set; }

        public bool Reversed { get; set; }
        public string Result { get; set; } 
        public PaymentType?  PaymentType { get; set; }
    }
}