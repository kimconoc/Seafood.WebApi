using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using Seefood.Domain.Common.Constant;
using Seefood.Domain.Models.DataAccessModel;
using Seefood.Repository.EntityFamework;
using Seefood.WebApi.App_Start;
using Seefood.WebApi.Middlewares;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Http;

[assembly: OwinStartup(typeof(Seefood.WebApi.Startup))]

namespace Seefood.WebApi
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // Enable CORS (cross origin resource sharing) for making request using browser from different domains
            if (ConfigurationManager.AppSettings["HiddenError"].Equals("false"))
                app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);

            //Cookie authenicate
            var session_timeout = Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["SessionTimeout"]);
            var keyCookieName = System.Configuration.ConfigurationManager.AppSettings["KeyCookieName"];
            app.UseCookieAuthentication(new CookieAuthenticationOptions()
            {
                AuthenticationType = Constant.AuthenticationType,
                CookieName = keyCookieName,
                SlidingExpiration = true,
                ExpireTimeSpan = TimeSpan.FromMinutes(session_timeout),
                Provider = new CookieAuthenticationProvider
                {
                    OnResponseSignedIn = context =>
                    {
                        var cookies = context.Response.Headers.GetCommaSeparatedValues("Set-Cookie");
                        var cookieValue = GetAuthenCookie(cookies);

                        if (!string.IsNullOrEmpty(cookieValue))
                            UpdateSession(context.Identity.Name, cookieValue);
                    }
                }
            });
            //Log middleware, vị trí này được chạy đầu tiên
            app.Use(typeof(LogChangeMiddleware));

            HttpConfiguration config = new HttpConfiguration();
            WebApiConfig.Register(config);
        }

        private string GetAuthenCookie(IList<string> cookies)
        {
            var cookieValue = "";

            foreach (var cookie in cookies)
            {
                var cookieKeyIndex = cookie.IndexOf(ConfigurationManager.AppSettings["KeyCookieName"]);
                if (cookieKeyIndex != -1)
                {
                    // Add extra character for '='
                    cookieValue = cookie.Substring(ConfigurationManager.AppSettings["KeyCookieName"].Length + 1);
                    break;
                }
            }
            return cookieValue;
        }
        private void UpdateSession(string username, string cookieValue)
        {
            try
            {
                using (var unitOfWork = new EfUnitOfWork())
                {
                    SessionAuthorize sessionAuthorize = new SessionAuthorize()
                    {
                        Username = username,
                        SessionId = cookieValue.Substring(0, 20),
                        Session = cookieValue
                    };
                    unitOfWork.SessionAuthorizeRepository.Add(sessionAuthorize);
                    unitOfWork.Commit();
                }
            }
            catch (Exception) { }
        }
    }
}