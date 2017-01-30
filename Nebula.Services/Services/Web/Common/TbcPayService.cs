using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Microsoft.AspNet.Identity;
using Nebula.Domain;
using Nebula.Domain.Entities;

namespace Nebula.Services.Services.Web.Common
{
    public class TbcPayService
    {
        private readonly GeneralRepository _generalRepository;

        public TbcPayService(GeneralRepository generalRepository)
        {
            _generalRepository = generalRepository;
        }

        public string GenerateTransactionId()
        {
            try
            {

                string currentUserId = HttpContext.Current.User.Identity.GetUserId();
                var currentUser = _generalRepository.User.Fetch(currentUserId);



              
                //    //check cupon
                //    var userCupon = _userCuponRepo.Set()
                //                    .Include(m => m.Cupon)
                //                    .OrderByDescending(m => m.Id)
                //                    .FirstOrDefault(m => m.UserId == currentUserId);

                //    int price = CuponManager.GetCuponManager(_userCuponRepo._context)
                //        .GeneratePriceByCuponCode(GlobalClass.Price, userCupon);

                //    //generate transaction id
                //    var tbcPay = new TBCPay();
                //    string transactionId = tbcPay.RegisterTransaction(price, "nebula license", Request.ServerVariables["REMOTE_ADDR"], 981, "ka");

                //    if (_userLicenseRepo.Set().Any(m => m.TransactionId == transactionId))
                //    {
                //        throw new Exception("მიმდინარე ტრანზაქცია უკვე დაფიქსირებულია, სცადეთ ხელახლა");
                //    }
                //    _userCuponRepo._context.UserLicensesHistory.Add(new UserLicense
                //    {
                //        BuyDate = DateTime.Now,
                //        LincenseAndDate = GlobalClass.LicenseEndDate,
                //        Payed = false,
                //        UserId = currentUserId,
                //        Result = "not payed yet",
                //        TransactionId = transactionId,
                //        Price = price
                //    });

                //    currentUser.PhoneNumber = model.PhoneNumber;
                //    currentUser.IdentificationNumber = model.IdentificationNumber;
                //    currentUser.Email = model.Email;
                //    currentUser.UserName = model.Email;
                //    currentUser.FirstName = model.FirstName;
                //    currentUser.LastName = model.LastName;
                //    _userCuponRepo._context.SaveChanges();

                //    return Json(new
                //    {
                //        transactionId = transactionId
                //    });
                //}
                //catch (Exception e)
                //{
                //    return Json(new
                //    {
                //        error = e.Message
                //    });
                //}

            }
            catch (Exception)
            {

            }
            return "";

        }
    }

  
}
