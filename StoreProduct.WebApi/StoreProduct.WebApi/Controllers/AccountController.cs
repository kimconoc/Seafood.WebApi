using Scrypt;
using StoreProduct.Domain.Common.Constant;
using StoreProduct.Domain.Models.Account;
using StoreProduct.Domain.Models.User;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Web;
using System.Web.Http;

namespace StoreProduct.WebApi.Controllers
{
    public class AccountController : BaseApiController
    {
        #region Login
        [HttpPost]
        [Route("api/Account/Login")]
        public IHttpActionResult LoginAPI([FromBody] LoginParameterModel request)
        {
            if (!request.Validate())
                return Content(HttpStatusCode.BadRequest, Message.LOGIN_ERROR);

            var user = ValidateUser(request.Username, request.Password);
            if (user == null)
            {
                return Content(HttpStatusCode.BadRequest, Message.LOGIN_ERROR);
            }
            if (user.IsLocked)
            {
                return Content(HttpStatusCode.BadRequest, new { ViMessage = "Tài khoản đã bị khóa" });
            }

            // Đăng ký cookieValue nhảy vào starup
            ClaimsIdentity identity = CreateIdentity(user);
            Request.GetOwinContext().Authentication.SignIn(identity);
            return Content(HttpStatusCode.OK, new { Token = "a" });
        }

        private User ValidateUser(string username, string password)
        {
            string devAccounts = ConfigurationManager.AppSettings.Get("DevUsername");
            if (ConfigurationManager.AppSettings["HiddenError"].Equals("false") && devAccounts.Contains(username))
                return unitOfWork.UserRepository.FirstOrDefault(s => s.Username.Equals(username));

            var userData = unitOfWork.UserRepository.FirstOrDefault(s => s.Username.Trim().ToLower().Equals(username.Trim().ToLower()));
            if (userData != null)
            {
                ScryptEncoder encoder = new ScryptEncoder();
                bool isPasswor = encoder.Compare(password.Trim(), userData.PasswordHash.Trim());
                if (isPasswor)
                {
                    return userData;
                }
            }
            return null;
        }

        private ClaimsIdentity CreateIdentity(User user)
        {
            string username = string.IsNullOrEmpty(user.Username) ? "" : user.Username;
            string roles = "roles";
            string role = "role";
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, username.Trim()),
                new Claim(ClaimTypes.Role, roles),
                new Claim("Roles", role),
            };

            var identity = new ClaimsIdentity(claims, "ApplicationCookie");
            return identity;
        }
        #endregion Login
        #region Logout
        [HttpGet]
        [Route("api/Account/Logout")]
        public HttpResponseMessage GetLogout()
        {
            var resp = new HttpResponseMessage();
            try
            {
                CookieHeaderValue cookie = Request.Headers.GetCookies("StoreProduct").FirstOrDefault();
                if (cookie != null)
                {
                    RemoveSession(cookie["StoreProduct"].Value);
                    var new_cookie = new CookieHeaderValue("StoreProduct", "")
                    {
                        Expires = DateTime.Now.AddDays(-1),
                        Domain = cookie.Domain,
                        Path = cookie.Path
                    };
                    resp.Headers.AddCookies(new[] { new_cookie });
                }
            }
            catch (Exception) { }
            try
            {
                var form_token = Request.Headers.GetValues("RequestVerificationToken").First();
                var cookie_token = Request.Headers.GetCookies("__RequestVerificationToken").LastOrDefault();
                var csrf_token = string.Format("{0}{1}", form_token, cookie_token["__RequestVerificationToken"].Value);
                var new_cookie_token = new CookieHeaderValue("__RequestVerificationToken", "")
                {
                    Expires = DateTime.Now.AddDays(-1),
                    Domain = cookie_token.Domain,
                    Path = cookie_token.Path
                };
                resp.Headers.AddCookies(new[] { new_cookie_token });
            }
            catch (Exception) { }
            resp.StatusCode = HttpStatusCode.OK;
            resp.Content = new ObjectContent<dynamic>(Message.Successful, new JsonMediaTypeFormatter());
            return resp;
        }

        private void RemoveSession(string session)
        {
            var session_id = session.Substring(0, 20);
            var user = unitOfWork.UserRepository.FirstOrDefault(
                e => !e.IsDeleted &&
                !string.IsNullOrEmpty(e.SessionId) &&
                e.SessionId == session_id
            );
            if (user != null)
            {
                user.Session = null;
                user.SessionId = null;
                unitOfWork.UserRepository.Update(user);
                unitOfWork.Commit();
            }
        }
        #endregion Logout
    }
}