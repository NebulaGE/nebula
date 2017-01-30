using System.Collections.Generic; 
using Nebula.Domain.Entities;

namespace Nebula.Domain.ViewModels.Web
{
    public class NewsViewModel
    {
        public News CurrentNews { get; set; }

        public List<NewsNavigationViewModel> NavigationNews { get; set; } 
    }
}
