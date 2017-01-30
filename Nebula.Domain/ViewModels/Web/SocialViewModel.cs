using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nebula.Domain.Entities;

namespace Nebula.Domain.ViewModels.Web
{
    public class SocialViewModel
    {
        public SocialProgramViewModel Social { get; set; }

        public User User { get; set; }
    }
}
