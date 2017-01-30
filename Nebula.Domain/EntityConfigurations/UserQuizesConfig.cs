using Nebula.Domain.Entities; 
using System.Data.Entity.ModelConfiguration; 

namespace Nebula.Domain.EntityConfigurations
{
    public class UserQuizesConfig : EntityTypeConfiguration<CSUserQuizzes>
    {
        public UserQuizesConfig()
        {

            HasRequired(u => u.User)
                .WithMany(u => u.UserQuizzes)
                .HasForeignKey(u => u.UserId)
                .WillCascadeOnDelete(false);

            HasRequired(u => u.Quiz)
                .WithMany()
                .HasForeignKey(u => u.QuizId)
                .WillCascadeOnDelete(false);
        }
    }
}