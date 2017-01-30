using Nebula.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace Nebula.Domain.EntityConfigurations
{
    public class TaskWithMultipleQuestionConfig : EntityTypeConfiguration<Condition>
    {
        public TaskWithMultipleQuestionConfig()
        {

            HasOptional(t => t.Module)
                .WithMany()
                .HasForeignKey(t => t.ModuleId)
                .WillCascadeOnDelete(false);

            HasOptional(t => t.CSQuestionInfo)
               .WithMany()
               .HasForeignKey(t => t.CSQuestionInfoId)
               .WillCascadeOnDelete(false);

       
        }
    }
}