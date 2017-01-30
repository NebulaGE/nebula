using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration; 
using Nebula.Domain.Entities;

namespace Nebula.Domain.EntityConfigurations
{
    public class UserGeoAnswerConfig : EntityTypeConfiguration<GeoUserAnswer>
    {
        public UserGeoAnswerConfig()
        {
            HasRequired(m => m.User)
             .WithMany(m => m.GeoAnswers)
             .HasForeignKey(m => m.UserId)
             .WillCascadeOnDelete(true);

            HasRequired(m => m.Answer)
                .WithMany(m => m.GeoUserAnswers)
                .HasForeignKey(m => m.AnswerId)
                .WillCascadeOnDelete(true);

            Property(m => m.AnswerId)
                 .IsRequired()
                 .HasColumnAnnotation(
                  IndexAnnotation.AnnotationName,
                   new IndexAnnotation(new IndexAttribute("IX_User_Answer_Id", 1) { IsUnique = true })
                );

            Property(m => m.UserId)
                .IsRequired()
                .HasColumnAnnotation(
                 IndexAnnotation.AnnotationName,
                  new IndexAnnotation(new IndexAttribute("IX_User_Answer_Id", 2) { IsUnique = true })
              );
        }
    }
}
