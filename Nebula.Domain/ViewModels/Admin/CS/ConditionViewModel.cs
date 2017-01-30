using System.Collections.Generic;
using Nebula.Domain.Entities;

namespace Nebula.Domain.ViewModels.Admin.CS
{
    public class ConditionViewModel
    {
        public ConditionViewModel()
        {
            Modules = new List<ModuleViewModel>();
            Infos = new List<CSQuestionInfoViewModel>();
            Questions = new List<ConditionRelatedQuestionsViewModel>();
        }

        public int Id { get; set; }
        public byte CategoryId { get; set; }

        public string Text { get; set; }

        public bool GuestCondition { get; set; }

        public TaskType? Type { get; set; }
        public int? ModuleId { get; set; }

        public int? InfoId { get; set; }

        public int?  QueueNumber { get; set; }

        public List<CSQuestionInfoViewModel> Infos { get; set; }

        public List<ModuleViewModel> Modules { get; set; }

        public List<ConditionRelatedQuestionsViewModel> Questions { get; set; }

        public CSQuestionViewModel QuestionCreateViewModel { get; set; }

    }
}
