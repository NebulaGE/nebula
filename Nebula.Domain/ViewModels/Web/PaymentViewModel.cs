using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nebula.Domain.ViewModels.Web
{
    public class PaymentViewModel
    {
        public int CsPrice { get; set; }

        public int GeoPrice { get; set; }
        
        public int EngPrice { get; set; }

        public PaymentType Type { get; set; }
    }

    public enum PaymentType
    {
        CS = 1,
        Geo = 2,
        Eng = 3,
        Balance
    }
}
