using Amin.Model;
using Amin.MemCached;
using Newtonsoft.Json;
using System;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Configuration;
using System.Net.Http;
using System.Net;
using Seafood.Repository.EntityFamework;
using Amin.Models;

namespace Amin
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_AuthenticateRequest(Object sender, EventArgs e)
        {
            if (HttpContext.Current.User != null)
            {
                if (HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    if (HttpContext.Current.User.Identity is FormsIdentity)
                    {
                        string session = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName].Value;
                        if (!IsTokenValid(session))
                        {
                            HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, "");
                            cookie.Expires = DateTime.Now.AddYears(-1);
                            Response.Cookies.Add(cookie);
                            Response.Redirect("/Account/Login");
                        }
                        else
                        {
                            FormsIdentity id = (FormsIdentity)HttpContext.Current.User.Identity;
                            FormsAuthenticationTicket ticket = id.Ticket;
                            var userData = JsonConvert.DeserializeObject<UserData>(ticket.UserData);
                            string[] roles = userData.Roles.ToArray();
                            HttpContext.Current.User = new GenericPrincipal(id, roles);
                        }
                    }
                }
            }
        }

        protected void Application_EndRequest()
        {
            var context = new HttpContextWrapper(Context);
            //Do a direct 401 unautorized
            if (Context.Response.StatusCode == 302 && context.Request.IsAjaxRequest())
            {
                Context.Response.Clear();
                Context.Response.StatusCode = 401;
            }
        }

        private bool IsTokenValid(string session)
        {
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
    }
}
