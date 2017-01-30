using System.Data.Entity;
using System.Linq; 
using System.Web.Mvc;
using Nebula.Domain;
using Nebula.Domain.Abstract;
using Nebula.Domain.Entities;
using Nebula.Domain.ViewModels.Admin;
using Nebula.Domain.ViewModels.Admin.CS;
using Nebula.Services.Services.Admin;
using PagedList; 

namespace Nebula.Web.Controllers.Admin
{
   [Authorize(Roles = "Admin")]

    public class AdminCSQuestionsController : Controller
    {
        private readonly BaseRepository<CSQuestionInfo> _cSQuestionInfoRepo;
        private readonly BaseRepository<Module> _moduleRepo;
        private readonly BaseRepository<CSQuestion> _cSQuestionRepo;
        private readonly AdminCsService _adminService; 
      
        public AdminCSQuestionsController(
            BaseRepository<CSQuestionInfo> cSQuestionInfoRepo,
            BaseRepository<CSTag> cSTagRepository, 
            BaseRepository<Module> moduleRepository,
            BaseRepository<CSQuestion> cSQuestionRepo,
            BaseRepository<Answer> answerRepo,
            BaseRepository<QuestionTag> questionTagRepo)
        {
            _cSQuestionInfoRepo = cSQuestionInfoRepo;
            _moduleRepo = moduleRepository;
            _cSQuestionRepo = cSQuestionRepo;

            _adminService = new AdminCsService(new GeneralRepository
            {
                CSQuestionInfo = _cSQuestionInfoRepo,
                Tag = cSTagRepository,
                Module = _moduleRepo,
                CsQuestion = _cSQuestionRepo,
                Answer = answerRepo,
                QuestionTag =  questionTagRepo
            });
        }
         
        // GET: AdminCSQuestions
       

        public ActionResult Manage(byte questionCategoryId, int id = 0)
        {  
            var model = _adminService
                .GetCsQuestionViewModel(questionCategoryId, id);
            model.Action = "Save";
            model.Controller = "AdminCSQuestions";

            return View(model); 
        } 
        
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Save(CSQuestionViewModel model)
        {
            //if (!ModelState.IsValid)
            //    return View();

            _adminService.SaveCsQuestion(model);

            int? nextQuestionId = null;
            if (model.Id != 0)
             nextQuestionId = _adminService.GetNextQuestionId(model.InfoId, model.QueueNumber);

            return nextQuestionId != null 
                ? RedirectToAction("Manage", new { questionCategoryId = model.CategoryId ,id = nextQuestionId})
                : RedirectToAction("Index" , new { id = model.CategoryId} );
        }


        



        // POST: AdminCSQuestions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id,  string returnUrl)
        { 
            _cSQuestionRepo.Delete(id);
            return Redirect(returnUrl);
        } 
    }
}
