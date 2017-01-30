using System;
using Nebula.Domain.Abstract;
using Nebula.Domain.Entities;

namespace Nebula.Services.Utils
{
    public static class UserLicenseCheckerHelper
    {
        public static bool CheckGeoLicense(BaseRepository<User> userRepo,string userId)
        {
            var user = userRepo.Fetch(userId);

            return user == null || !user.HasGeoLicense || DateTime.Now >= user.LicenseEndDate;
        }

        public static bool CheckEngLicense(BaseRepository<User> userRepo, string userId)
        {
            var user = userRepo.Fetch(userId);

            return user == null || !user.HasEngLicense || DateTime.Now >= user.EngLicenseEndDate;
        }
    }
}
