 

namespace Nebula.Domain.Entities
{
    public class BookCode
    {
        public int Id { get; set; }

        public string Code { get; set; }  

        public bool IsUsed { get; set; } 
    }
}
