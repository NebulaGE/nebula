using Nebula.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace Nebula.Domain.EntityConfigurations
{
    public class ExamTypeConfig : EntityTypeConfiguration<ExamType>
    {
        public ExamTypeConfig()
        {
            Property(e => e.Name)
                .HasMaxLength(50)
                .IsRequired();
        }
    }
}