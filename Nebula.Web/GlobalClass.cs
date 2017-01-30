using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Web;

namespace Nebula.Web
{
    public static class GlobalClass
    {


        //get folders
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

        public static string ExplantationVideoPath
        {
            get
            {
                return System.Web.HttpContext.Current.Server.MapPath("~/Content/explentation-videos");
            }

        }
       
        public static string ImgPath
        {
            get
            {
                return System.Web.HttpContext.Current.Server.MapPath("~/Content/Images");
            }
        }

        public static string UserFilesPath
        {
            get
            {
                return System.Web.HttpContext.Current.Server.MapPath("~/Content/user-files");
            }

        }

        public static bool isAuth
        {
            get
            {
                return HttpContext.Current.Session["nebula"] == null ? false : true;
            }

        }

        public static bool IsAuthTeacher
        {
            get
            {
                return HttpContext.Current.Session["Teacher"] == null ? false : true;
            }
        }


        public static int GetExamsCount
        {
            get
            {
                return Int16.Parse(System.Web.Configuration.WebConfigurationManager.AppSettings["ExamsCount"]);
            }
        }

        public static int GetGeoExamDuration
        {
            get
            {
                return int.Parse(System.Web.Configuration.WebConfigurationManager.AppSettings["GeoExamDuration"]);
            }
        }

        public static double GeTExamTime
        {
            get
            {
                return Double.Parse(System.Web.Configuration.WebConfigurationManager.AppSettings["ExamTime"]);
            }
        }



        public static int GetExamMinimalTime
        {
            get
            {
                return int.Parse(System.Web.Configuration.WebConfigurationManager.AppSettings["MinimalExamTime"]);
            }
        }

        public static double [] ExamCoefficients
        {
            get
            {  
                return  System.Web.Configuration.WebConfigurationManager.AppSettings["ExamCoefficients"]
                    .Split(';').Select(double.Parse).ToArray();
            }
        }

        public static double ShewoniliMinRQ
        {
            get
            {
                return double.Parse(System.Web.Configuration.WebConfigurationManager.AppSettings["ShewoniliMinRQ"]);
            }
        }

        public static int JamuriMinRQ
        {
            get
            {
                return int.Parse(System.Web.Configuration.WebConfigurationManager.AppSettings["JamuriMinRQ"]);
            }
        }

  

        public static byte AnalogeId
        {
            get
            {
                return byte.Parse(System.Web.Configuration.WebConfigurationManager.AppSettings["AnalogeId"]);
            }
        }
        public static DateTime ScheduleEndDate
        {
            get
            {
              
                int[] scheduleDateArr = System.Web.Configuration.WebConfigurationManager.AppSettings["ShceduleEndDate"]
                    .Split(';').Select(m => int.Parse(m)).ToArray();             
                return new DateTime(scheduleDateArr[0], scheduleDateArr[1], scheduleDateArr[2]);
            }
        }

        public static DateTime LicenseEndDate
        {
            get
            {
              
                int[] licenseEndArr = System.Web.Configuration.WebConfigurationManager.AppSettings["LicenseEndDate"]
                 .Split(';').Select(m => int.Parse(m)).ToArray();
                return new DateTime(licenseEndArr[0], licenseEndArr[1], licenseEndArr[2]);
            }
        }
 

        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }





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



        public static bool IsValidVideoFIle(HttpPostedFileBase video)
        {
            return false;
        }

        public static bool IsValidImgFile(HttpPostedFileBase image)
        {
            return true;
        }


        public static string JsonSerializer<T>(T t)
        {
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
            MemoryStream ms = new MemoryStream();
            ser.WriteObject(ms, t);
            string jsonString = System.Text.Encoding.UTF8.GetString(ms.ToArray());
            ms.Close();
            return jsonString;
        }
        /// <summary>
        /// JSON Deserialization
        /// </summary>
        public static T JsonDeserialize<T>(string jsonString)
        {
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
            MemoryStream ms = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(jsonString));
            T obj = (T)ser.ReadObject(ms);
            return obj;
        }
        public static string LicenseRoleWord
        {
            get
            {
                return "License";
            }
        }

        public static string TransactionError
        {
            get
            {
                return System.Web.Configuration.WebConfigurationManager.AppSettings["TransactionError"];
            }
        }
        public static int Price
        {
            get
            {
                return int.Parse(System.Web.Configuration.WebConfigurationManager.AppSettings["Price"]);
            }
        }

        public static int CSPrice
        {
            get
            {
                return int.Parse(System.Web.Configuration.WebConfigurationManager.AppSettings["CSPrice"]);
            }
        }

        public static int GeoPrice
        {
            get
            {
                return int.Parse(System.Web.Configuration.WebConfigurationManager.AppSettings["GeoPrice"]);
            }
        }

        public static int EngPrice
        {
            get
            {
                return int.Parse(System.Web.Configuration.WebConfigurationManager.AppSettings["EngPrice"]);
            }
        }

        public static int NovaPrice
        {
            get
            {
                return int.Parse(System.Web.Configuration.WebConfigurationManager.AppSettings["NovaPrice"]);
            }
        }

        public static string GeoPay
        {
            get
            {
                return "GEOPAY";
            }
        }
        public static string NovaPay
        {
            get
            {
                return "NOVAPAY";
            }
        }

        public static string SampleUserId
        {
            get
            {
                return "695bca23-75a0-4bb5-b5df-6dbce162893c";
            }
        }

        public static string DestionationEmail
        {
            get
            {
                return System.Web.Configuration.WebConfigurationManager.AppSettings["DestinationEmail"];
            }
        }

        public static string sep(string s)
        {
            int l = s.IndexOf(";");
            if (l > 0)
            {
                return s.Substring(0, l);
            }
            return "";
        }

        public static bool IsValidFile(this HttpPostedFileBase postedFile)
        {
            //-------------------------------------------
            //  Check the image mime types
            //-------------------------------------------
            if (postedFile.ContentType.ToLower() != "image/jpg" &&
                        postedFile.ContentType.ToLower() != "image/jpeg" &&
                        postedFile.ContentType.ToLower() != "image/pjpeg" &&
                        postedFile.ContentType.ToLower() != "image/gif" &&
                        postedFile.ContentType.ToLower() != "image/x-png" &&
                        postedFile.ContentType.ToLower() != "image/png" &&
                        postedFile.ContentType.ToLower() != "application/pdf" &&
                        postedFile.ContentType.ToLower() != "application/msword" &&
                        postedFile.ContentType.ToLower() != "text/plain"  &&
                        postedFile.ContentType.ToLower() != "application/vnd.openxmlformats-officedocument.wordprocessingml.document")
            {
                return false;
            }

        
            return true;

        }

        

    }

 

  
    }
