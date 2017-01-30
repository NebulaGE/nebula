using Nebula.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace Nebula.Domain.EntityConfigurations
{
    public class NotificationConfig : EntityTypeConfiguration<Notification>
    {
        public NotificationConfig()
        {
            HasRequired(m => m.User)
                .WithMany(m => m.Notifications)
                .HasForeignKey(m => m.UserId)
                .WillCascadeOnDelete(true);
        }
    }
}