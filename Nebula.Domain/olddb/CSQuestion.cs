//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Nebula.Domain.olddb
{
    using System;
    using System.Collections.Generic;
    
    public partial class CSQuestion
    {
        public int Id { get; set; }
        public string QuestionString { get; set; }
        public int CorrectAnswerId { get; set; }
        public Nullable<int> ExamTagId { get; set; }
        public Nullable<int> TaskWithMultipleQuestionId { get; set; }
        public Nullable<int> VideoId { get; set; }
        public Nullable<int> VideoPartId { get; set; }
        public bool IsExamQuestion { get; set; }
        public bool IsExerciseQuestion { get; set; }
        public string AnswersCaption { get; set; }
        public bool IsWordAnalog { get; set; }
        public Nullable<int> Num { get; set; }
        public Nullable<int> QuestionInfoId { get; set; }
        public bool IsVideoQuestion { get; set; }
    
        public virtual TaskWithMultipleQuestion TaskWithMultipleQuestion { get; set; }
    }
}