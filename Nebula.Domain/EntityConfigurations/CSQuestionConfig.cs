using Nebula.Domain.Entities; 
using System.Data.Entity.ModelConfiguration; 

namespace Nebula.Domain.EntityConfigurations
{
    public class CSQuestionConfig : EntityTypeConfiguration<CSQuestion>
    {
        public CSQuestionConfig()
        {
            Property(q => q.QuestionText)
                .IsRequired(); 

            HasOptional(q => q.Condition)
                .WithMany(tm => tm.Questions)
                .HasForeignKey(q => q.ConditionId)
                .WillCascadeOnDelete(true);

            HasOptional(q => q.Module)
             .WithMany(tm => tm.Questions)
             .HasForeignKey(q => q.ModuleId)
             .WillCascadeOnDelete(false);

            HasOptional(q => q.Video)
             .WithMany(v => v.QuizQuestions)
             .HasForeignKey(q => q.VideoId)
             .WillCascadeOnDelete(true);

            HasOptional(q => q.VideoPart)
             .WithMany(v => v.Questions)
             .HasForeignKey(q => q.VideoPartId)
             .WillCascadeOnDelete(true);

            HasOptional(q => q.CSQuestionInfo)
            .WithMany(v => v.Questions)
            .HasForeignKey(q => q.CSQuestionInfoId)
            .WillCascadeOnDelete(false);  
        }
    }
}