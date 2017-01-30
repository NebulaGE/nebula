using System.Data.Entity.ModelConfiguration; 
using Nebula.Domain.Entities;

namespace Nebula.Domain.EntityConfigurations
{
    public class GeoVideoConfig :  EntityTypeConfiguration<GeoVideo>
    {
        public GeoVideoConfig()
        {
            Property(a => a.Name)
                .HasMaxLength(250)
                .IsRequired();

            Property(a => a.FileLink)
                .IsRequired(); 

            HasOptional(a => a.GeoAuthorWorkPart)
                .WithMany(a => a.GeoVideos)
                .HasForeignKey(a => a.GeoAuthorWorkPartId);

            HasOptional(a => a.GeoGrammarSubTag)
                .WithMany(a => a.GeoVideos)
                .HasForeignKey(a => a.GeoGrammarSubTagId);
        }
    }
}
