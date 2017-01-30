using System.Collections.Generic;
using Nebula.Domain.Entities;

namespace Nebula.Domain.ViewModels.Web
{
    public class HomePageNewsViewModel
    {
        public List<News> Tweets { get; set; }

        public List<News> Blogs { get; set; }
    }
}
