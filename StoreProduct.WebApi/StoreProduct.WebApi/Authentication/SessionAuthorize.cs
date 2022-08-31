using Microsoft.Owin;
using StoreProduct.Domain.Common.Constant;
using StoreProduct.Repository.EntityFamework;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace StoreProduct.WebApi.Authentication
{
    public class SessionAuthorize : AuthorizeAttribute
    {
        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            var ip = GetClientIpAddress(actionContext.Request);
            try
            {
                if (ConfigurationManager.AppSettings["HiddenError"].Equals("false") && ConfigurationManager.AppSettings["DevWriteLists"].Contains(ip))
                    return true;
            }
            catch (Exception) { }

            if (actionContext.Request.RequestUri.AbsoluteUri.ToString().Contains("api/PublicApi"))
            {
                try
                {
                    var token = actionContext.Request.Headers.FirstOrDefault(h => h.Key.Equals("Authorization"));
                    string app_token = ConfigurationManager.AppSettings["PublicApiToken"];
                    string token_value = token.Value.FirstOrDefault();
                    bool is_valieate = token_value.ToString().Contains(ConfigurationManager.AppSettings["PublicApiToken"].ToString());
                    return is_valieate;
                }
                catch (Exception)
                {
                    return false;
                }
            }

            // kiểm tra token có tồn tại không, hoặc token hết hạn chưa ?
            if (!base.IsAuthorized(actionContext))
                return false;

            dynamic authen_cookie;
            try
            {
                var keyCookieName = System.Configuration.ConfigurationManager.AppSettings["KeyCookieName"];
                authen_cookie = actionContext.Request.Headers.GetCookies(keyCookieName).FirstOrDefault();
                if (authen_cookie != null)
                {
                    var session_id = authen_cookie[keyCookieName].Value.Substring(0, 20);
                    var test = IsTokenValid(session_id);
                    return test;
                }
            }
            catch (Exception) { }
            return false;
        }

        protected override void HandleUnauthorizedRequest(HttpActionContext actionContext)
        {
            base.HandleUnauthorizedRequest(actionContext);
            actionContext.Response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.Unauthorized,
                Content = new ObjectContent<dynamic>(Message.Un_Au_Thorized, new JsonMediaTypeFormatter())
            };
        }

        private bool IsTokenValid(string session_id)
        {
            using (IUnitOfWork unitOfWork = new EfUnitOfWork())
            {
                var user = unitOfWork.UserRepository.FirstOrDefault(
                    e => !e.IsDeleted &&
                    !string.IsNullOrEmpty(e.SessionId) &&
                    e.SessionId == session_id
                );
                if (user != null)
                    return user.Username.Trim() == HttpContext.Current.User.Identity.Name;

                return false;
            }
        }

        private string GetClientIpAddress(HttpRequestMessage request)
        {
            if (request.Properties.ContainsKey("MS_HttpContext"))
            {
                return IPAddress.Parse(((HttpContextBase)request.Properties["MS_HttpContext"]).Request.UserHostAddress).ToString();
            }
            if (request.Properties.ContainsKey("MS_OwinContext"))
            {
                return IPAddress.Parse(((OwinContext)request.Properties["MS_OwinContext"]).Request.RemoteIpAddress).ToString();
            }
            return String.Empty;
        }
    }
}