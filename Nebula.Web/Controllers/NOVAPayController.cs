using Nebula.Web.Models.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity; 
using Microsoft.AspNet.Identity.Owin; 
using System.Security.Cryptography;
using System.Text;
using System.Data.Entity;
using Nebula.Domain.Abstract;
using Nebula.Domain.Entities;

namespace Nebula.Web.Controllers
{
    public class NOVAPayController : Controller
    {
        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        private readonly BaseRepository<User> _userRepo;
        private readonly BaseRepository<Pocket> _pocketRepo;
        private readonly BaseRepository<UserCupon> _userCuponRepo;
        private readonly BaseRepository<Cupon> _cuponRepo; 
        private readonly BaseRepository<NovaPaymentHistory> _novaPaymentHistoryRepo;
        private readonly BaseRepository<FailTransaction> _failTransactionRepo;
         
        private readonly string XML_HEADER = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";
        private readonly int TIME = (int)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;
     //   private readonly int PRICE = GlobalClass.NovaPrice;
 
        private readonly string SECRET_KEY = "";
        private readonly string USERNAME = "";
        private readonly string PASSWORD = "";

        public NOVAPayController(
            BaseRepository<User> userRpository, 
            BaseRepository<Pocket> pocketRpository, 
            BaseRepository<UserCupon> userCuponRpository, 
            BaseRepository<NovaPaymentHistory> novaPaymentHistoryRpository, 
            BaseRepository<FailTransaction> failTransactionRpository,
            BaseRepository<Cupon> cuponRpository)
        {
            _userRepo = userRpository;
            _pocketRepo = pocketRpository;
            _userCuponRepo = userCuponRpository;
            _novaPaymentHistoryRepo = novaPaymentHistoryRpository;
            _failTransactionRepo = failTransactionRpository;
            _cuponRepo = cuponRpository;
        }

        // GET: NOVAPay
        public string Index(NovaPayViewModel model)
        {          
            var user = new User();
     
            try
            {                       
              //  int price =  _pocketRepo.Set().First(m => m.Id == 1).Price;
                
                string response = string.Empty;

                if (!CheckRequiredParameters(new List<string> { model.USERNAME, model.PASSWORD, model.HASH_CODE }))
                {
                    response = Error(4, "No required parameter");
                   return response;
                }

                string hashGenerator = Request.QueryString.AllKeys.Where(key => key != "HASH_CODE").
                    Aggregate("", (current, key) => current + Request.QueryString[key]);

                hashGenerator += SECRET_KEY;
                hashGenerator = CreateMD5Hash(hashGenerator);

                if (hashGenerator != model.HASH_CODE)
                {
                    response = Error(3, "Hash code is invalid");
                    return response;
                }

                if (USERNAME != model.USERNAME || PASSWORD != model.PASSWORD)
                {
                    response = Error(2, "Password or username is incorrect");
                    return response;
                }
                 

                switch (model.OP.ToLower())

                {
                    case "debt":

                        if (!CheckRequiredParameters(new List<string> { model.CUSTOMER_ID }))
                        {
                            response = Error(4, "No required parameter");
                            return response;
                        }

                        user = _userRepo.Set().SingleOrDefault(m => m.IdentificationNumber == model.CUSTOMER_ID);

                        if (user == null)
                        {
                            response = Error(6, "Customer does not exist");
                            return response;
                        }

                         //user.VerifyNovaBalance = int.Parse(model.PAY_AMOUNT);  
                         response = DebtXML(user, 0);
                

                        break;
                    case "verify":
                        if (!CheckRequiredParameters(new List<string> { model.CUSTOMER_ID }))
                        {
                            response = Error(4, "No required parameter");
                            return response;
                        }
                    
                            user = _userRepo.Set().SingleOrDefault(m => m.IdentificationNumber == model.CUSTOMER_ID);
                            if(user == null) {
                                response = Error(6, "Customer does not exist");
                                return response;
                            }


                       //if(user.VerifyNovaBalance != int.Parse(model.PAY_AMOUNT))
                       //     return Error(7, "amount do not match");
                          
                        
                        response = VerifyXML(); 
                        break;
                    case "pay":
                
                        if (!CheckRequiredParameters(new List<string> { model.PAY_AMOUNT,  model.CUSTOMER_ID, model.PAY_SRC, model.PAYMENT_ID }))
                        {
                            response = Error(4, "No required parameter");
                            return response;
                        }
                         
                        user = _userRepo.Set().SingleOrDefault(m => m.IdentificationNumber == model.CUSTOMER_ID);
          
                      
                        if (_novaPaymentHistoryRepo.Set().Any(m => m.TransId == model.PAYMENT_ID))
                        {
                            return Error(8, "PAYMENT_ID is not unique");
                        } 

                        if (user == null)
                        {
                            return Error(6, "Customer does not exist");
                        }
                
                  
                        user.Balance = int.Parse(model.PAY_AMOUNT);
                      
                        var novaHistory = new NovaPaymentHistory
                        {
                            BuyDate = DateTime.Now,
                            LincenseAndDate = GlobalClass.LicenseEndDate,
                            UserId = user.Id,
                            Payed = true,
                            Price = int.Parse(model.PAY_AMOUNT),
                            TransId = model.PAYMENT_ID     
                        };

                        _novaPaymentHistoryRepo._context.NovaPaymentHistories.Add(novaHistory);
    
                        _cuponRepo._context.SaveChanges();
                      
                        response = PayXML(novaHistory.Id);
                        break;
                    case "ping":
                        response = PingXML();
                        break;

                }

                return response;
            }
            catch (Exception e)
            {

                _failTransactionRepo.Save(new FailTransaction
                {

                    TransactionDate = DateTime.Now,
                    UserId = user == null ? "user is null" : user.Id,
                    Error = e.Message,
                    Type = GlobalClass.NovaPay
                }); 
                return Error(9, "Payment is unavailable");
            }

        }

        private string DebtXML(User user, int price)
        {

            return XML_HEADER +
             "<pay-response>" +
                "<status code=\"0\">OK</status>" +
                "<timestamp>" + TIME + "</timestamp>" +
                "<debt>" + price + "</debt>" +
                "<additional-info>" +
                    "<parameter name=\"first-name\">" + user.FirstName + "</parameter>" +
                  "<parameter name=\"last-name\">" + user.LastName + "</parameter>" +
                "</additional-info>" +
             "</pay-response>";

        }

        private string VerifyXML()
        {

            return XML_HEADER +
                "<pay-response>" +
                   "<status code=\"0\">OK</status>" +
                   "<timestamp>" + TIME + "</timestamp>" +
                "</pay-response>";
        }
        private string PayXML(int receiptId)
        {
            return XML_HEADER +
                "<pay-response>" +
                   "<status code=\"0\">OK</status>" +
                   "<timestamp>" + TIME + "</timestamp>" +
                   "<receipt-id>"+ receiptId+ "</receipt-id>"+
                "</pay-response>";
        }

        private string PingXML()
        {
            return XML_HEADER +
              "<pay-response>" +
                 "<status code=\"0\">OK</status>" +
                 "<timestamp>" + TIME + "</timestamp>" +
              "</pay-response>";
        }



        private string Error(int errorCode, string error)
        {
            return XML_HEADER +
             "<pay-response>" +
                "<status code=\"" + errorCode + "\">" + error + "</status>" +
                "<timestamp>" + TIME + "</timestamp>" +
              "</pay-response>";
        }


        private string CreateMD5Hash(string input)
        {
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
        }
        private bool CheckRequiredParameters(List<string> parameters)
        {
            foreach (string parameter in parameters)
            {
                if (string.IsNullOrEmpty(parameter))
                    return false;
            }
            return true;
        } 
    }

    public class NovaPayViewModel
    {
        public string OP { get; set; }
        public string USERNAME { get; set; }
        public string PASSWORD { get; set; }
        public string CUSTOMER_ID { get; set; }
        public string SERVICE_ID { get; set; }
        public string HASH_CODE { get; set; }
        public string PAY_AMOUNT { get; set; }
        public string PAY_SRC { get; set; }
        public string PAYMENT_ID { get; set; }
        public string EXTRA_INFO { get; set; }
    }
}