using Nebula.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace Nebula.Domain.EntityConfigurations
{
    public class VideoConfig : EntityTypeConfiguration<Video>
    {
        public VideoConfig()
        {
            HasRequired(v => v.CSTag)
                .WithMany(t => t.Videos)
                .HasForeignKey(v => v.TagId)
                .WillCascadeOnDelete(false); 
        }
    }
}