using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nebula.Services.Utils
{
    public static class PathsHelper
    {
        public static string VideoPath
        {
            get
            {
                return System.Web.HttpContext.Current.Server.MapPath("~/Content/Videos");
            }

        }
        public static string PayedVideoPath
        {
            get
            {
                return System.Web.HttpContext.Current.Server.MapPath("~/Content/payed-videos");
            }

        }
    }
}
