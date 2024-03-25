using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StackOverFLow.ServiceLayers;
using StackOverFlow.ViewModels;
using StackOverFlow.CustomFilter;


namespace StackOverFlow.Controllers
{
    public class QuestionsController : Controller
    {
        IQuestionService qs;
        IAnswersService asr;
        ICategoryService cs;

        public QuestionsController(IQuestionService qs, IAnswersService asr, ICategoryService cs)
        {
            this.qs = qs;
            this.asr = asr;
            this.cs = cs;
        }

        public ActionResult View(int id)
        {
            this.qs.UpdateQuestionViewsCount(id, 1);
            int uid = Convert.ToInt32(Session["CurrentUserID"]);
            QuestionViewModel qvm = this.qs.GetQuestionByQuestionID(id, uid);
            return View(qvm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [UserAuthorizationFilterAttribute]
        public ActionResult AddAnswer(NewAnswerViewModel navm)
        {
            navm.UserID = Convert.ToInt32(Session["CurrentUserID"]);
            navm.AnswerDateAndTime = DateTime.Now;
            navm.VotesCount = 0;
            if(ModelState.IsValid)
            {
                this.asr.InsertAnswer(navm);
                return RedirectToAction("View","Questions", new {id = navm.QuestionID});
            }
            else
            {
                QuestionViewModel qvm = this.qs.GetQuestionByQuestionID(navm.QuestionID, navm.UserID);
                return View("View", qvm);
            }
        }

        [HttpPost]
        public ActionResult EditAnswer(EditAnswerViewModel eavm)
        {
            if (ModelState.IsValid)
            {
                eavm.UserID = Convert.ToInt32(Session["CurrentUserID"]);
                this.asr.UpdateAnswer(eavm);
                return RedirectToAction("View", new { id = eavm.QuestionID });
            }
            else
            {
                return RedirectToAction("View", new { id = eavm.QuestionID });
            }
            
        }

        public ActionResult Create()
        {
            List<CategoryViewModel> categories = this.cs.GetCategories();
            ViewBag.Categories = categories;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [UserAuthorizationFilter]
        public ActionResult Create(NewQuestionViewModel qvm)
        {
            if (ModelState.IsValid)
            {
                qvm.AnswersCount = 0;
                qvm.ViewsCount = 0;
                qvm.VotesCount = 0;
                qvm.QuestionDateAndTime = DateTime.Now;
                qvm.UserID = Convert.ToInt32(Session["CurrentUserID"]);
                this.qs.Insertquestion(qvm);
                return RedirectToAction("Questions", "Home");
            }
            else
            {
                return View();
            }
        }
    }
}