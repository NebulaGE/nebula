using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nebula.Domain;
using Nebula.Services.Dto;

namespace Nebula.Services.Services.Web.Common
{
    public class ExercisesService
    {
        private readonly GeneralRepository _generalRepository;

        public ExercisesService(GeneralRepository generalRepository)
        {
            _generalRepository = generalRepository;
        }

        public void SetAnswer(SetAnswerDto model)
        {
            
        }
    }
}
