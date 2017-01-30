using Nebula.Domain.Entities; 
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration; 

namespace Nebula.Domain.EntityConfigurations
{
    public class UserLicenseConfig :  EntityTypeConfiguration<UserLicense>
    {
        public UserLicenseConfig()
        { 
            HasRequired(m => m.User)
                .WithMany(m => m.Licenses)
                .HasForeignKey(m => m.UserId)
                .WillCascadeOnDelete(false); 

            Property(m => m.TransactionId)
                 .HasMaxLength(150)
                 .HasColumnAnnotation("Index",
                 new IndexAnnotation(new IndexAttribute("IX_TransactionId") { IsUnique = true }));
        }
    }
}