using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Nebula.Services.ThirdPartyServices
{
    public class TbcThirdPartyService : ICertificatePolicy
    {
        private string lastError = string.Empty;
        private string postRequest = "command=v&ammount=100&currency=981&client_ip_addr=95.104.29.163&desc=order description&language=ka";
        private const string merchantHandlerHost = "https://91.235.243.70:18443/ecomm2/MerchantHandler";
        private const string clientHandlerHost = "https://securepay.ufc.ge/ecomm2/ClientHandler";

        //  private const string certificatePath = "C:\\inetpub\\vhosts\\leavingstone.net\\certificates\\nebula\\securepay.ufc.ge_5300657_merchant_wp.p12";

        private const string ECCOM_PARAM_COMMAND = "command";
        private const string ECCOM_PARAM_AMOUNT = "amount";
        private const string ECCOM_PARAM_CURRENCY = "currency";
        private const string ECCOM_PARAM_CLIENT_IP_ADDR = "client_ip_addr";
        private const string ECCOM_PARAM_DESCRIPTION = "description";
        private const string ECCOM_PARAM_LANGUAGE = "language";
        private const string ECCOM_PARAM_TRANSACTION_ID_POST = "trans_id";
        private const string ECCOM_PARAM_TRANSACTION_ID = "TRANSACTION_ID: ";
        public const string RESULT_OK = "OK";
        public const string RESULT_FAILED = "FAILED";
        public const string RESULT_CREATED = "CREATED";
        public const string RESULT_PENDING = "PENDING";
        public const string RESULT_DECLINED = "DECLINED";
        public const string RESULT_REVERSED = "REVERSED";
        public const string RESULT_AUTOREVERSED = "AUTOREVERSED";
        public const string RESULT_TIMEOUT = "TIMEOUT";

        public string LastResponse { get; private set; }

        public string LastError
        {
            get
            {
                return this.lastError;
            }
        }





        public bool CheckValidationResult(ServicePoint sp, X509Certificate certificate, WebRequest request, int error)
        {
            return true;
        }

        public string MakeMerchantRequest()
        {
            return this.MakeSSLRequest(this.postRequest);
        }

        private string MakeSSLRequest(string postDataPayload)
        {
            X509Certificate2 x509Certificate2 = new X509Certificate2();
            x509Certificate2.Import(certificatePath, certificatePassword, X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.Exportable | X509KeyStorageFlags.PersistKeySet);
            ServicePointManager.CertificatePolicy = (ICertificatePolicy)new TbcThirdPartyService();
            HttpWebRequest httpWebRequest = (HttpWebRequest)null;
            httpWebRequest = (HttpWebRequest)WebRequest.Create("https://securepay.ufc.ge:18443/ecomm2/MerchantHandler");
            byte[] bytes = Encoding.UTF8.GetBytes(postDataPayload);
            httpWebRequest.Method = "POST";
            httpWebRequest.ContentType = "application/x-www-form-urlencoded";
            httpWebRequest.ContentLength = (long)bytes.Length;
            httpWebRequest.UseDefaultCredentials = true;
            httpWebRequest.KeepAlive = false;
            if (x509Certificate2 != null)
            {
                httpWebRequest.ClientCertificates.Add((X509Certificate)x509Certificate2);
            }
            Stream requestStream = httpWebRequest.GetRequestStream();

            requestStream.Write(bytes, 0, bytes.Length);
            requestStream.Close();
            this.LastResponse = new StreamReader(httpWebRequest.GetResponse().GetResponseStream(), Encoding.UTF8).ReadToEnd();
            return this.LastResponse;
        }

        private string BuildPOSTPayload(Dictionary<string, string> keyValuePairs)
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (KeyValuePair<string, string> keyValuePair in keyValuePairs)
            {
                stringBuilder.Append(keyValuePair.Key + "=" + HttpUtility.UrlEncode(keyValuePair.Value) + "&");
            }


            string str = stringBuilder.ToString();
            str.Substring(0, str.Length - 1);
            return stringBuilder.ToString();
        }


        public string EndOfTheDay()
        {

            string result = this.MakeSSLRequest(this.BuildPOSTPayload(new Dictionary<string, string>()
        {

            {
              "command",
              "b"
            }
        }));

            return result;
        }

        public string RefundTansFull(string transId)
        {
            string result = this.MakeSSLRequest(this.BuildPOSTPayload(new Dictionary<string, string>()
                {

                {
                  "command",
                  "k"
                },
                {
                    "trans_id",
                    transId
                }
                }));

            return result;
        }

        public string ReverseTransaction(string transId, int amount)
        {
            string result = this.MakeSSLRequest(this.BuildPOSTPayload(new Dictionary<string, string>()
                {

                {
                  "command",
                  "r"
                },
                {
                    "trans_id",
                    transId
                },
                {
                 "amount",
                  amount.ToString()
                 }
                }));

            return result;
        }


        public string RegisterTransaction(int ammount, string description, string clientIpAdress, int currency = 981, string language = "ka")
        {
            string postDataPayload = this.BuildPOSTPayload(new Dictionary<string, string>()
                  {
                    {
                      "command",
                      "v"
                    },
                    {
                      "amount",
                      ammount.ToString()
                    },
                    {
                      "currency",
                      currency.ToString()
                    },
                    {
                      "client_ip_addr",
                      clientIpAdress
                    },
                    {
                      "description",
                      description
                    },
                    {
                      "language",
                      language
                    }
                  });

            string result = this.MakeSSLRequest(postDataPayload);
            this.LastResponse = result;
            string userFriendlyResult;
            if (result.Contains("error"))
            {
                this.lastError = result;
                userFriendlyResult = "error";
            }
            else
                userFriendlyResult = result.Substring("TRANSACTION_ID: ".Length, 28);

            return userFriendlyResult;
        }

        public string[] GetTransactionResult(string transactionId, string clientIp)
        {
            string postDataPayload = this.BuildPOSTPayload(new Dictionary<string, string>()
              {
                {
                  "command",
                  "c"
                },
                {
                  "trans_id",
                  transactionId
                },
                {
                  "client_ip_addr",
                  clientIp
                }
              });

            string result = this.MakeSSLRequest(postDataPayload);
            string userFriendlyResult = new StringReader(result).ReadLine();

            if (result.Contains("error"))
            {
                this.lastError = result;
                userFriendlyResult = "error";
            }
            else
                userFriendlyResult = userFriendlyResult.Replace("RESULT: ", "");

            return new string[] { userFriendlyResult, result };
        }
    }

    internal enum HandlerType
    {
        MERCHANT,
        CLIENT,
    }
}
