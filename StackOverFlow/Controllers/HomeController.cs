﻿using StackOverFlow.CustomFilter;
using StackOverFlow.ViewModels;
using StackOverFLow.ServiceLayers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Filters;
using System.Web.Mvc;

namespace StackOverFlow.Controllers
{
    public class HomeController : Controller
    {
        IQuestionService qs;
        ICategoryService cs;
        public HomeController(IQuestionService qs, ICategoryService cs)
        {
            this.qs = qs;
            this.cs = cs;
        }

        public ActionResult Index()
        {
            List < QuestionViewModel> questions = this.qs.GetQuestions().Take(10).ToList();
            return View(questions);
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }

        public ActionResult Categories()
        {
            List<CategoryViewModel> categories = this.cs.GetCategories();
            return View(categories);    
        }

        [Route("allquestions")]
        public ActionResult Questions()
        {
            List<QuestionViewModel> questions = this.qs.GetQuestions();
            return View(questions);
        }

        public ActionResult Search(string str)
        {
            List<QuestionViewModel> questions = this.qs.GetQuestions().Where(temp => temp.QuestionName.ToLower().Contains(str.ToLower()) || temp.Category.CategoryName.ToLower().Contains(str.ToLower())).ToList();
            ViewBag.str = str;
            return View(questions);
        }

        
        [UserAuthorizationFilter]
        public ActionResult Delete(int cid)
        {
         this.cs.DeleteCategory(cid);
            return RedirectToAction("Categories");
        }

        public ActionResult ViewQuestions(string cname)
        {
           cname = cname.Trim();
           List<QuestionViewModel> questions = this.qs.GetQuestions().Where(temp => temp.Category.CategoryName.Contains(cname)).ToList();
            return View(questions);
        }
    }
}