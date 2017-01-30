using System.Data.Entity.ModelConfiguration;
using Nebula.Domain.Entities;

namespace Nebula.Domain.EntityConfigurations
{
    public class GeoAuthorWorkConfig : EntityTypeConfiguration<GeoAuthorWork>
    {
        public GeoAuthorWorkConfig()
        {
            Property(a => a.Title)
                .HasMaxLength(100)
                .IsRequired();

            HasRequired(a => a.Author)
                .WithMany(a => a.GeoAuthorWorks)
                .HasForeignKey(a => a.AuthorId);
        }
    }
}
