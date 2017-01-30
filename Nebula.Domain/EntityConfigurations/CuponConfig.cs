using Nebula.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace Nebula.Domain.EntityConfigurations
{
    public class CuponConfig: EntityTypeConfiguration<Cupon>
    {
        public CuponConfig()
        {

            Property(m => m.CuponCode)
                .HasMaxLength(6)
                .IsRequired()
                .HasColumnAnnotation("Index",
                new IndexAnnotation(new IndexAttribute("IX_CuponCode") { IsUnique = true }));
          

        }
    }
}