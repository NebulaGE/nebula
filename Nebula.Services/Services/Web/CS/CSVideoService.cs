using Nebula.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nebula.Services.Services.Web.CS
{
    public class CSVideoService
    {
        private readonly GeneralRepository _generalRepository;

        public CSVideoService(GeneralRepository generalRepository)
        {
            _generalRepository = generalRepository;
        }
    }
}
