using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Nebula.Services.Utils
{
    public static class FilesHelper
    {
        public static void AddFile(HttpPostedFileBase file, string filename, string url)
        {
            string filePath = Path.Combine(url, filename);
            file.SaveAs(filePath);
        }

        public static void DeleteFile(string filename, string url)
        {
            var path = Path.Combine(url, filename);
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
    }
}
