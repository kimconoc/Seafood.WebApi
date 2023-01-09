using Admin.Models;
using Amin.CustomAuthen;
using Amin.Models;
using Newtonsoft.Json;
using Seafood.Domain.Models.DataAccessModel;
using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Amin.Controllers
{
    public class AccountController : BaseController
    {
        #region Login
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model, string ReturnUrl = "")
        {
            bool isValidADAccount = LoginAdAccount(model.UserName, model.Password);
            if (isValidADAccount)
            {
                try
                {
                    var userAdmin = unitOfWork.UserAdminRepository.FirstOrDefault(s => !s.IsDeleted && s.Username.Equals(model.UserName) && s.PasswordHash.Equals(model.Password) && s.IsAdminUser);
                    if (userAdmin == null)
                    {
                        ViewBag.Message = "User is not registered to application";
                    }
                    else
                    {
                        var userData = StoreUserData(userAdmin);
                        FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, model.UserName, DateTime.Now, DateTime.Now.AddMinutes(720), false, JsonConvert.SerializeObject(userData), FormsAuthentication.FormsCookiePath);
                        string hash = FormsAuthentication.Encrypt(ticket);
                        LogLogin(hash.Substring(0, 20), hash, userAdmin.Username);
                        HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, hash);
                        Response.Cookies.Add(cookie);
                        return RedirectToAction("Index", "Admin");
                    }
                }
                catch(Exception ex)
                {
                    ViewBag.Message = "Have error when login. Please check with our Administrator";
                }

            }
            else
            {
                ViewBag.Message = "Input informations is incorrect";
            }
            return View(model);
        }
        public ActionResult Logout()
        {
            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                try
                {
                    string session = Request.Cookies[FormsAuthentication.FormsCookieName].Value;
                    RemoveSession(session);
                }
                catch (Exception) { }
            }

            FormsAuthentication.SignOut();
            HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, "");
            cookie.Expires = DateTime.Now.AddYears(-1);
            Response.Cookies.Add(cookie);

            return RedirectToAction("Login", "Account");
        }
        private bool LoginAdAccount(string userName, string password)
        {
            return true;
            bool isValidAdAccount = false;
            using (PrincipalContext context = new PrincipalContext(ContextType.Domain))
            {
                isValidAdAccount = context.ValidateCredentials(userName, password);
            }
            return isValidAdAccount;
        }
        private UserData StoreUserData(UserAdmin userAdmin)
        {
            var userData = new UserData
            {
                UserId = userAdmin.Id,
                Username = userAdmin.Username,
                DisplayName = userAdmin.DisplayName,
                Roles = userAdmin.Roles != null? new List<string>(userAdmin.Roles.Split(',')) : new List<string>(),
            };
            return userData;
        }
        private void LogLogin(string SessionId, string Session, string username)
        {
            var sessionAuthorizeAdmin = new SessionAuthorizeAdmin
            {
                Username = username,
                IpRequest = Request.UserHostAddress,
                SessionId = SessionId,
                Session = Session,
                TimeLogin = DateTime.Now,
            };
            unitOfWork.SessionAuthorizeAdminRepository.Add(sessionAuthorizeAdmin);
            unitOfWork.Commit();
        }
        private void RemoveSession(string session)
        {
            var session_id = session.Substring(0, 20);
            var sessionAuthorizeAdmin = unitOfWork.SessionAuthorizeAdminRepository.FirstOrDefault(
                e => !e.IsDeleted &&
                !string.IsNullOrEmpty(e.SessionId) &&
                e.SessionId == session_id
            );
            if (sessionAuthorizeAdmin != null)
            {
                sessionAuthorizeAdmin.Session = null;
                sessionAuthorizeAdmin.SessionId = null;
                sessionAuthorizeAdmin.TimeLogout = DateTime.Now;
                unitOfWork.SessionAuthorizeAdminRepository.Update(sessionAuthorizeAdmin);
                unitOfWork.Commit();
            }
        }


        #endregion Login 
    }
}