using Nebula.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace Nebula.Domain.EntityConfigurations
{
    public class FinishedTaskConfig : EntityTypeConfiguration<FinishedTask>
    {
        public FinishedTaskConfig()
        {
            HasRequired(m => m.Task)
                .WithMany()
                .HasForeignKey(m => m.TaskId)
                .WillCascadeOnDelete(true);

            HasRequired(m => m.User)
                .WithMany(m => m.FinishedTasks)
                .HasForeignKey(m => m.UserId)
                .WillCascadeOnDelete(true);

            HasRequired(m => m.Module)
                .WithMany()
                .HasForeignKey(m => m.ModuleId)
                .WillCascadeOnDelete(true);

            //HasOptional(m => m.Question)
            //    .WithMany()
            //    .HasForeignKey(m => m.QuestionId)
            //    .WillCascadeOnDelete(true);
        }
    }
}