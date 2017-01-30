using Nebula.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nebula.Domain.EntityConfigurations
{
    public class GeoUserExamConfig : EntityTypeConfiguration<GeoUserExam>
    {
        public GeoUserExamConfig()
        {
            HasRequired(m => m.User)
               .WithMany(m => m.GeoExams)
               .HasForeignKey(m => m.UserId)
               .WillCascadeOnDelete(false);

            HasRequired(m => m.GeoEssay)
               .WithMany()
               .HasForeignKey(m => m.GeoEssayId)
               .WillCascadeOnDelete(false);

            HasRequired(m => m.GeoTextEditing)
                  .WithMany()
                  .HasForeignKey(m => m.GeoTextEditingId)
                  .WillCascadeOnDelete(false);

            HasRequired(m => m.Poetry)
                 .WithMany()
                 .HasForeignKey(m => m.PoetryId)
                 .WillCascadeOnDelete(false);

            HasRequired(m => m.Prose)
                .WithMany()
                .HasForeignKey(m => m.ProseId)
                .WillCascadeOnDelete(false);
        }
    }
}
