using System; 

namespace Nebula.Domain.Entities
{
    public class FailTransaction
    {
        public int Id { get; set; }
        public string UserId { get; set; }      
        public string Result { get; set; }
        public string Error { get; set; }
        public string TransactionId { get; set; }
        public string Type { get; set; }
        public DateTime TransactionDate { get; set; } 
    }
}