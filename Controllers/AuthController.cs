using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using XYZShoppingMVC.Models;
using XYZShoppingMVC.Utils;

namespace XYZShoppingMVC.Controllers
{
    public class AuthController : Controller
    {
        // GET: Auth
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            LoginModel lm = new LoginModel();
            return View(lm);
        }

        [HttpPost]
        public ActionResult Login(LoginModel lm)
        {
            if(ModelState.IsValid)
            {
                bool res = BusinessLayer.BusinessAuth.VerifyLogin(lm.Username, lm.Password);
                if (res == true)
                {
                    string roles = BusinessLayer.BusinessAuth.GetRolesForUser(lm.Username);

                    FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, lm.Username, DateTime.Now, DateTime.Now.AddMinutes(15), false, roles);
                    string encryptedticket = FormsAuthentication.Encrypt(ticket);
                    HttpCookie ck = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedticket);
                    Response.Cookies.Add(ck);
                    SessionFacade.LOGGEDIN = lm.Username;
                    string redirectUrl = FormsAuthentication.GetRedirectUrl(lm.Username, false);
                    if (redirectUrl == "/default.aspx")
                        redirectUrl = "~/home/index";
                    return Redirect(redirectUrl);
                }
                else
                    ViewBag.msg = "Invalid login!!!!!";
            }
            return View(lm);
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
    }
}