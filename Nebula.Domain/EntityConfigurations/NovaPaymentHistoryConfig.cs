using Nebula.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace Nebula.Domain.EntityConfigurations
{
    public class NovaPaymentHistoryConfig :  EntityTypeConfiguration<NovaPaymentHistory>
    {
        public NovaPaymentHistoryConfig()
        {
         HasRequired(m => m.User)
          .WithMany(m => m.NovaPaymentHistories)
          .HasForeignKey(m => m.UserId)
          .WillCascadeOnDelete(false); 
        } 
    }
}