using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nebula.Web.Models
{
    public class CustomException : Exception
    {
        public CustomException(string message)
            : base(message) { }
    }
}