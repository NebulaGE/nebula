using Nebula.Domain.Entities; 
using System.Data.Entity.ModelConfiguration; 

namespace Nebula.Domain.EntityConfigurations
{
    public class VideoPartConfig : EntityTypeConfiguration<VideoPart>
    {
        public VideoPartConfig()
        { 
            HasRequired(vd => vd.Video)
                .WithMany(v => v.VideoParts)
                .HasForeignKey(vd => vd.VideoId)
                .WillCascadeOnDelete(false); 
        }
    }
} 