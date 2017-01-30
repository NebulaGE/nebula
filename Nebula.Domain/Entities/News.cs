using System; 

namespace Nebula.Domain.Entities
{
    public class News
    {
        public int Id { get; set; }

        public string Author { get; set; }

        public string Title { get; set; }

        public string Text { get; set; }

        public bool ShowOnHomePage { get; set; }

        public bool IsBlog { get; set; }
         
        public DateTime Date { get; set; } 
    }
}
