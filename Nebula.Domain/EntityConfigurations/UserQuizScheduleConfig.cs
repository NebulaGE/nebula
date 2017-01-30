using Nebula.Domain.Entities; 
using System.Data.Entity.ModelConfiguration; 

namespace Nebula.Domain.EntityConfigurations
{
    public class UserQuizScheduleConfig : EntityTypeConfiguration<UserQuizSchedule>
    {
        public UserQuizScheduleConfig()
        {
            HasRequired(m => m.User)
                .WithMany(m => m.QuizSchedules)
                .HasForeignKey(m => m.UserId)
                .WillCascadeOnDelete(true);
        }
    }
}