using Nebula.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace Nebula.Domain.EntityConfigurations
{
    public class TagConfig :  EntityTypeConfiguration<CSTag>
    {
        public TagConfig()
        {
            HasRequired(qt => qt.QuestionType)
                .WithMany(qt => qt.Tags)
                .HasForeignKey(qt => qt.QuestionTypeId)
                .WillCascadeOnDelete(false);
        }
    }
}