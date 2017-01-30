using System.Data.Entity.ModelConfiguration; 
using Nebula.Domain.Entities;

namespace Nebula.Domain.EntityConfigurations
{
    public class GeoAuthorWorkPartConfig : EntityTypeConfiguration<GeoAuthorWorkPart>
    {
        public GeoAuthorWorkPartConfig() 
        {
            HasRequired(a => a.AuthorWork)
                .WithMany(a => a.GeoAuthorWorkParts)
                .HasForeignKey(a => a.AuthorWorkId);
        }
    }
}
