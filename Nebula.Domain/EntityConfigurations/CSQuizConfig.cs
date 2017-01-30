using Nebula.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;
using System.Data.Entity.Infrastructure.Annotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nebula.Domain.EntityConfigurations
{
    public class CSQuizConfig : EntityTypeConfiguration<CSQuiz>
    {
        public CSQuizConfig()
        {
         

        }
    }
}