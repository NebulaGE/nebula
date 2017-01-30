using Nebula.Domain.Entities;

namespace Nebula.Domain.ViewModels.Admin.CS
{
    public class ConditionRelatedQuestionsViewModel
    {
        public int Id { get; set; }
        public string Module { get; set; }
        public string Text { get; set; }
        public string VideoExplanationLink { get; set; }
        public CSQuestionType? Type { get; set; }
    }
}
