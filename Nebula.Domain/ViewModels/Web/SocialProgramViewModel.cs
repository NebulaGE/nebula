using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nebula.Domain.ViewModels.Web
{
    public class SocialProgramViewModel
    {
        [Required(ErrorMessage ="გთხოვთ შეავსოთ მეილის ველი")]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage ="პაროლის შეყვანა აუცილებელია")]
        [StringLength(100, ErrorMessage = "პაროლი უნდა შედგებოდეს მინიმუმ 6 სიმბოლოსგან", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "გაიმეროეთ პაროლის ველი არ ემთხვევა პაროლის ველს")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "სახელი უნდა შედგებოდეს მინიმუმ 2 სიმბოლოსგან"),MinLength(2, ErrorMessage = "სახელი უნდა შედგებოდეს მინიმუმ 2 სიმბოლოსგან")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "გვარი უნდა შედგებოდეს მინიმუმ 4 სიმბოლოსგან"), MinLength(2, ErrorMessage = "გვარი უნდა შედგებოდეს მინიმუმ 4 სიმბოლოსგან")]
        public string LastName { get; set; }

        [Required(ErrorMessage ="საფუძვლის ტექსტი არ შეიძლება იყოს ცარიელი")]
        public string SocialText { get; set; }

        [Required(ErrorMessage ="გთხვოვთ შეავსოთ პირადი ნომრის ველი"),MinLength(11,ErrorMessage = "გთხოვთ, სწორად შეიყვანოთ პირადი ნომერი"),MaxLength(11,ErrorMessage = "გთხოვთ, სწორად შეიყვანოთ პირადი ნომერი")]
        public string IdentificationNumber { get; set; }

        [Required(ErrorMessage = "გთხოვთ შეავსოთ მობილურის ველი")]
        public string PhoneNumber { get; set; }
    }
}
