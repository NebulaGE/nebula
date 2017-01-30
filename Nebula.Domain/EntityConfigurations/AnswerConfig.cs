using Nebula.Domain.Entities; 
using System.Data.Entity.ModelConfiguration; 

namespace Nebula.Domain.EntityConfigurations
{
    public class AnswerConfig : EntityTypeConfiguration<Answer>
    {
        public AnswerConfig()
        {
            Property(a => a.AnswerString)
                .IsRequired();

            HasRequired(a => a.Question)
                .WithMany(q => q.Answers)
                .HasForeignKey(a => a.QuestionId)
                .WillCascadeOnDelete(true);  
        }
    }
}
 