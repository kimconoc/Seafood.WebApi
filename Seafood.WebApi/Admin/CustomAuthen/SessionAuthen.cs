using Amin.Models;
using Newtonsoft.Json;
using Seafood.Repository.EntityFamework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Amin.CustomAuthen
{
    public class SessionAuthen: AuthorizeAttribute
    {
        public string Role { get; set; }
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            bool isAuthroized = base.AuthorizeCore(httpContext);
            if (!isAuthroized)
                return false;

            return IsTokenValid(httpContext) && IsRoleValid(httpContext);
        }

        private bool IsTokenValid(HttpContextBase httpContext)
        {
            try
            {
                string session = httpContext.Request.Cookies[FormsAuthentication.FormsCookieName].Value;
                var session_id = session.Substring(0, 20);
                using (IUnitOfWork unitOfWork = new EfUnitOfWork())
                {
                    var sessionAuthorizeAdmin = unitOfWork.SessionAuthorizeAdminRepository.FirstOrDefault(
                        e => !e.IsDeleted &&
                        !string.IsNullOrEmpty(e.SessionId) &&
                        e.SessionId == session_id
                    );
                    return sessionAuthorizeAdmin != null;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        private bool IsRoleValid(HttpContextBase httpContext)
        {
            try
            {
                FormsIdentity id = (FormsIdentity)HttpContext.Current.User.Identity;
                FormsAuthenticationTicket ticket = id.Ticket;
                var userData = JsonConvert.DeserializeObject<UserData>(ticket.UserData);

                if(userData != null && userData.Roles.Any())
                {
                    List<string> Roles = new List<string>(Role.Split(','));
                    foreach(string role in Roles)
                    {
                        if (userData.Roles.Any(x => x == role))
                        {
                            return true;
                        }    
                    }      
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}