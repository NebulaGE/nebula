using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Nebula.Domain.Entities;

namespace Nebula.Domain.ViewModels.Admin.CS
{
    public class CSQuestionViewModel
    {
        public CSQuestionViewModel()
        {
            Answers = new List<AnswerViewModel>();
            Modules = new List<ModuleViewModel>();
            Tags = new List<CSTagViewModel>();
            Infos = new List<CSQuestionInfoViewModel>();
            TagIds = new List<int>();
        }

        public int Id { get; set; }

        [Required]
        public string Text { get; set; }

        public string VideoExplanationLink { get; set; }

        public bool IsFreeQuestion { get; set; }

        public string Controller { get; set; }
        public string Action { get; set; }

        public CSQuestionType Type { get; set; }

        public byte CategoryId { get; set; }

        public int? ModuleId { get; set; }

        public int? QueueNumber { get; set; }
        public List<int> TagIds { get; set; }
        public int? InfoId { get; set; }
        public int? ConditionId { get; set; }
        public int CorrectAnswerId { get; set; }
         
        public int? VideoPartId { get; set; }
        public int? VideoId { get; set; }
        public List<AnswerViewModel> Answers { get; set; }
        public List<ModuleViewModel> Modules { get; set; }
        public List<CSTagViewModel> Tags { get; set; }
        public List<CSQuestionInfoViewModel> Infos { get; set; }

        public string NumHelper(int num)
        {
            switch (num)
            {
                case 0:
                    return "ა)";
                case 1:
                    return "ბ)";
                case 2:
                    return "გ)";
                case 3:
                    return "დ)";
                case 4:
                    return "ე)";
                default:
                    return string.Empty;
            }
        } 
    }
}
