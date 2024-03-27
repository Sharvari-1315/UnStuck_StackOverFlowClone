using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StackOverFlow.CustomFilter;
using StackOverFlow.ViewModels;
using StackOverFLow.ServiceLayers;

namespace StackOverFlow.Controllers
{
    public class AccountController : Controller
    {
        IUsersService us;
        public AccountController(IUsersService us)
        {
            this.us = us;
        }
        
        public ActionResult Register()
        {
            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Register(RegisterViewModel rvm)
        {
            if (ModelState.IsValid)
            {
                int uid = this.us.InsertUser(rvm);
                Session["CurrentUserID"] = uid;
                Session["CurrentUserName"] = rvm.Name;
                Session["CurrentUserEmail"] = rvm.Email;
                Session["CurrentUserPassword"] = rvm.Password;
                Session["CurrentUserIsAdmin"] = false;
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View();
            }
            
        }

        public ActionResult Login()
        {
            LoginViewModel lvm = new LoginViewModel();
            return View(lvm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel lvm)
        {
            if (ModelState.IsValid)
            {
                UserViewModel uvm = this.us.GetUsersByEmailAndPassword(lvm.Email, lvm.Password);
                if(uvm != null)
                {
                    Session["CurrentUserID"] = uvm.UserID;
                    Session["CurrentUserName"] = uvm.Name;
                    Session["CurrentUserEmail"] = uvm.Email;
                    Session["CurrentUserPassword"] = uvm.Password;
                    Session["CurrentUserIsAdmin"] = uvm.IsAdmin;

                    if (uvm.IsAdmin)
                    {
                        return RedirectToRoute(new { area = "admin", controller = "AdminHome", action = "Index" });
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
            }
            else
            {
                return View(lvm);
            }
            return View(lvm);
        }


        public ActionResult LogOut()
        {
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }

       [UserAuthorizationFilterAttribute]
        public ActionResult ChangeProfile()
        {
            int uid = Convert.ToInt32(Session["CurrentUserID"]);
            UserViewModel uvm = this.us.GetUsersByUserID(uid);
            EditUserDetailsViewModel eudvm = new EditUserDetailsViewModel() { Name = uvm.Name, Email = uvm.Email, Mobile = uvm.Mobile,UserID= uvm.UserID};
            return View(eudvm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [UserAuthorizationFilterAttribute]
        public ActionResult ChangeProfile(EditUserDetailsViewModel eudvm)
        {
               if(ModelState.IsValid)
            {
                eudvm.UserID = Convert.ToInt32(Session["CurrentUserID"]);
                this.us.UpdateUserDetails(eudvm);
                Session["CurrentUserName"] = eudvm.Name;
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View(eudvm);
            }
                
        }

        [UserAuthorizationFilterAttribute]
        public ActionResult ChangePassword()
        {
            int uid = Convert.ToInt32(Session["CurrentUserID"]);
            UserViewModel uvm = this.us.GetUsersByUserID(uid);
            EditUserPasswordViewModel eupvm = new EditUserPasswordViewModel() { Email = uvm.Email,Password="",ConfirmPassword= "",UserID= uvm.UserID};
            return View(eupvm);
        }


        [UserAuthorizationFilterAttribute]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(EditUserPasswordViewModel eupvm)
        {
            if (ModelState.IsValid)
            {

                eupvm.UserID = Convert.ToInt32(Session["CurrentUserID"]);
                this.us.UpdateUserPassword(eupvm);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View(eupvm) ;
            }

        }
        }
    }
