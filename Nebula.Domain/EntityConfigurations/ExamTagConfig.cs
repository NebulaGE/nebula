using Nebula.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace Nebula.Domain.EntityConfigurations
{
    public class ModuleConfig : EntityTypeConfiguration<Module>
    {
        public ModuleConfig()
        {
            Property(qt => qt.Name)
                .HasMaxLength(80)
                .IsRequired();

            HasRequired(qt => qt.CSQuestionCategory)
                .WithMany(qt => qt.Modules)
                .HasForeignKey(qt => qt.CSQuestionCategoryId)
                .WillCascadeOnDelete(false);

                //HasMany(c => c.Questions)
                //    .WithMany(t => t.QuestionTags)
                //    .Map(m =>
                //    {
                //        m.ToTable("QuestionTags");
                //        m.MapLeftKey("TagId");
                //        m.MapRightKey("CSQuestionId");
                //    });
            }
        }
    }
