using Nebula.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace Nebula.Domain.EntityConfigurations
{
    public class CSQuestionCategoryConfig : EntityTypeConfiguration<CSQuestionCategory>
    {
        public CSQuestionCategoryConfig()
        {
            Property(ck => ck.Name)
                .HasMaxLength(50)
                .IsRequired(); 
        }
    }
}