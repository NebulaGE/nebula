using Nebula.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace Nebula.Domain.EntityConfigurations
{
    public class TeacherStudentConfig : EntityTypeConfiguration<TeacherStudent>
    {
        public TeacherStudentConfig()
        {
            HasRequired(m => m.Student)
                .WithMany(m => m.Teachers)
                .HasForeignKey(m => m.StudentId)
                .WillCascadeOnDelete(true);


            HasRequired(m => m.Teacher)
                .WithMany(m => m.Students)
                .HasForeignKey(m => m.TeacherId)
                .WillCascadeOnDelete(true);
                               
        }
    }
}