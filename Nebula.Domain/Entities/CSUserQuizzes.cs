using System;

namespace Nebula.Domain.Entities
{
    public class CSUserQuizzes
    {
        public int Id { get; set; }

 
        public string UserId { get; set; }
        public User User { get; set; }

   
        public int QuizId { get; set; }
        public CSQuiz Quiz { get; set; }

        public int? MathUserScore { get; set; }
        public int? VerbalUserScore { get; set; }

        public DateTime? UserLastActive { get; set; }
        public DateTime? VerbalQuizStartDate { get; set; }
        public DateTime? VerbalQuizEndDate { get; set; }
        public DateTime? MathQuizStartDate { get; set; }
        public DateTime? MathQuizEndDate { get; set; }
        public double UserInactiveSeconds { get; set; }
        public bool IsFinished { get; set; }
        public bool IsTimeOut { get; set; }
        public bool VerbalQuizTimeOut { get; set; }
        public bool MathQuizTimeOut { get; set; }

        public bool? QuizConfirmed { get; set; }
        public bool? MathQuizConfirmed { get; set; }
        public bool? VerbalQuizConfirmed { get; set; }
    }
}