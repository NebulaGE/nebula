using System.Data.Entity.ModelConfiguration;
using Nebula.Domain.Entities;

namespace Nebula.Domain.EntityConfigurations
{
    public class GeoAnswerConfig : EntityTypeConfiguration<GeoAnswer>
    {
        public GeoAnswerConfig()
        {
           

            HasRequired(a => a.Question)
                .WithMany(a => a.Answers)
                .HasForeignKey(a => a.QuestionId)
                .WillCascadeOnDelete(true);
        }
    }
}
