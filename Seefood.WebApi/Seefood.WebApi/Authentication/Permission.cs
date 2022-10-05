using Microsoft.Owin;
using Seefood.Domain.Common.Constant;
using Seefood.Domain.Models.BaseModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Security.Claims;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace Seefood.WebApi.Authentication
{
    public class Permission : AuthorizeAttribute
    {
        public string Code { get; set; }
        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            var ip = GetClientIpAddress(actionContext.Request);
            try
            {
                if (ConfigurationManager.AppSettings["HiddenError"].Equals("false") && ConfigurationManager.AppSettings["DevWriteLists"].Contains(ip))
                    return true;
            }
            catch (Exception) { }
            try
            {
                var principal = actionContext.RequestContext.Principal as ClaimsPrincipal;
                bool isAdminUser = principal.Claims.FirstOrDefault(c => c.Type == "IsAdminUser").Value.Equals("true");
                if (!isAdminUser)
                    return false;

                var roles = principal.Claims.FirstOrDefault(c => c.Type == "Roles").Value;
                if (string.IsNullOrEmpty(roles))
                    return false;

                string[] array_roles = roles.Split(',');
                return array_roles.Contains(Code);
            }
            catch (Exception)
            {
                return false;
            }
        }
        protected override void HandleUnauthorizedRequest(HttpActionContext actionContext)
        {
            base.HandleUnauthorizedRequest(actionContext);
            var objForbidden = new RequestBaseNoData()
            {
                Success = false,
                StatusCode = (int)HttpStatusCode.Forbidden,
                Message = Domain.Common.Constant.Message.Un_Au_Thorized
            };
            actionContext.Response = new HttpResponseMessage
            {
                Content = new ObjectContent<RequestBaseNoData>(objForbidden, new JsonMediaTypeFormatter())
            };
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