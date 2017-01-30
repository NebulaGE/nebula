using System.Data.Entity.ModelConfiguration;
using Nebula.Domain.Entities;

namespace Nebula.Domain.EntityConfigurations
{
    public class GeoQuestionConfig : EntityTypeConfiguration<GeoQuestion>
    {
        public GeoQuestionConfig()
        {
            Property(a => a.Text) 
                .IsRequired();  

            HasOptional(a => a.GeoAuthorWorkPart)
                .WithMany(a => a.Questions)
                .HasForeignKey(a => a.GeoAuthorWorkPartId)
                .WillCascadeOnDelete(true);

            HasOptional(a => a.GeoGrammarSubTag)
                .WithMany(a => a.Questions)
                .HasForeignKey(a => a.GeoGrammarSubTagId)
                .WillCascadeOnDelete(true);

            HasOptional(a => a.GeoFictionText)
               .WithMany(a => a.Questions)
               .HasForeignKey(a => a.GeoFictionTextId)
               .WillCascadeOnDelete(true);
        }
    }
}
