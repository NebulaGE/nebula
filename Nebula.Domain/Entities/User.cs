using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;
using System;

namespace Nebula.Domain.Entities
{

    public class User : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User> manager)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);

            return userIdentity;
        }

        public User()
        {
            FinishedTasks = new HashSet<FinishedTask>();
            Notifications = new HashSet<Notification>();
            Licenses = new HashSet<UserLicense>();
            NovaPaymentHistories = new HashSet<NovaPaymentHistory>();
            QuizSchedules = new HashSet<UserQuizSchedule>();
            Teachers = new HashSet<TeacherStudent>();
            Answers = new HashSet<UserAnswer>();
            UserQuizzes = new HashSet<CSUserQuizzes>();
            GeoAnswers = new HashSet<GeoUserAnswer>();
            GeoExams = new HashSet<GeoUserExam>();
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ExternalUserName { get; set; }
        public string FacebookId { get; set; }
        public string GoogleId { get; set; }

        public string SocialText { get; set; }
        public string FileName { get; set; }
        public bool IsSocailActivate { get; set; }

        //public bool IsSocialCSRequested { get; set; }
        //public  bool IsSocialEngRequested { get; set; }
        //public  bool IsSocialGeoRequested { get; set; } 

        [StringLength(11)]
        public string IdentificationNumber { get; set; }

        public System.DateTime RegisterDate { get; set; }
        public System.DateTime LastLogin { get; set; }
        public System.DateTime? SocialTextSendDate { get; set; }

        public bool HasLicense { get; set; }

        public bool HasGeoLicense { get; set; }

        public bool HasEngLicense { get; set; }

        public bool HasAllLicense { get; set; }

        public bool HasUploadPdf { get; set; }
        public bool IsSocialRefused { get; set; }

        public double Balance { get; set; }
        public int VerifyNovaBalance { get; set; }

        public System.DateTime? LicenseBuyDate { get; set; }
        public System.DateTime? LicenseEndDate { get; set; }

        public DateTime? GeoLicenseBuyDate { get; set; }
        public DateTime? GeoLicenseEndDate { get; set; }

        public DateTime? EngLicenseBuyDate { get; set; }
        public DateTime? EngLicenseEndDate { get; set; }

        public DateTime? AllLicenseBuyDate { get; set; }
        public DateTime? AllLicenseEndDate { get; set; }

        public ICollection<FinishedTask> FinishedTasks { get; set; }
        public ICollection<Notification> Notifications { get; set; }

        public ICollection<UserLicense> Licenses { get; set; }

        public ICollection<NovaPaymentHistory> NovaPaymentHistories { get; set; }
        public ICollection<UserQuizSchedule> QuizSchedules { get; set; }
        public ICollection<TeacherStudent> Teachers { get; set; }
        public ICollection<UserAnswer> Answers { get; set; }
        public ICollection<CSUserQuizzes> UserQuizzes { get; set; }
        public ICollection<GeoUserAnswer> GeoAnswers { get; set; }
        public ICollection<GeoUserExam> GeoExams { get; set; } 
    }
}

