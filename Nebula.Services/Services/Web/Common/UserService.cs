using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Nebula.Domain;
using Nebula.Domain.Abstract;
using Nebula.Domain.Entities;

namespace Nebula.Services.Services.Web.Common
{
    public class UserService
    {

        private readonly GeneralRepository _generalRepository;

        public UserService(GeneralRepository generalRepository)
        {
            _generalRepository = generalRepository;
        }




    }
}
