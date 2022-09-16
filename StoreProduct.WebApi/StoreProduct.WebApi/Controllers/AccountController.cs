using Scrypt;
using StoreProduct.Domain.Common.Constant;
using StoreProduct.Domain.Models.ParameterModel;
using StoreProduct.Domain.Models.DataAccessModel;
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
using System.Reflection;
using StoreProduct.Domain.Common.FileLog;

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
                return Ok(BadRequest());

            var user = ValidateUser(request.Username, request.Password);
            if (user == null)
            {
                var result = BadRequest();
                result.Message = Message.LOGIN_ERROR;
                return Ok(result);
            }
            if (user.IsLocked)
            {
                var result = BadRequest();
                result.Message = Message.Account_LOCKED;
                return Ok(result);
            }

            // Đăng ký cookieValue nhảy vào starup
            ClaimsIdentity identity = CreateIdentity(user);
            Request.GetOwinContext().Authentication.SignIn(identity);
            //
            dynamic data = new
            {
                Id = user.Id,
                Username = user.Username,
                Roles = user.Roles,
                Fullname = user.Fullname,
                FirstName = user.FirstName,
                LastName = user.LastName,
                MiddleName = user.MiddleName,
                DisplayName = user.DisplayName,
                Mobile = user.Mobile,
                EmailAddress = user.EmailAddress,
                IsAdminUser = user.IsAdminUser,
                IsLocked = user.IsLocked
            };
            return Ok(RequestOK<dynamic>(data));
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
            string roles = string.IsNullOrEmpty(user.Roles) ? "" : user.Roles;
            string isAdminUser = user.IsAdminUser? "true" : "false";
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, username.Trim()),
                new Claim("Roles", roles.Trim()),
                new Claim("IsAdminUser", isAdminUser),
            };
            var identity = new ClaimsIdentity(claims, Constant.AuthenticationType);
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
                var keyCookieName = System.Configuration.ConfigurationManager.AppSettings["KeyCookieName"];
                CookieHeaderValue cookie = Request.Headers.GetCookies(keyCookieName).FirstOrDefault();
                if (cookie != null)
                {
                    RemoveSession(cookie[keyCookieName].Value);
                    var new_cookie = new CookieHeaderValue(keyCookieName, "")
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
        #region CreateAccount
        [HttpPost]
        [Route("api/Account/Create")]
        public IHttpActionResult CreateAccount([FromBody] CreateAccountParameter request)
        {
            if (request == null)
                return Ok(BadRequest());
            if (!Helper.ValidPhoneNumer(request.NumberPhone) || !Helper.IsValidEmail(request.Email)
                || string.IsNullOrEmpty(request.FirstName) || string.IsNullOrEmpty(request.LastName) || string.IsNullOrEmpty(request.Password))
                return Ok(BadRequest());

            try
            {
                var user = unitOfWork.UserRepository.FirstOrDefault(s => s.Username.Trim().ToLower().Equals(request.NumberPhone.Trim().ToLower()));
                if (user != null)
                {
                    var result = BadRequest();
                    result.Message = new
                    {
                        ViMessage = "Số điện thoại đã được đăng ký",
                    };
                    return Ok(result);
                }
                ScryptEncoder encoder = new ScryptEncoder();
                var passwordHash = encoder.Encode(request.Password);
                User userCreate = new User()
                {
                    Username = request.NumberPhone,
                    PasswordHash = passwordHash,
                    Fullname = request.FirstName + request.LastName,
                    DisplayName = request.FirstName + request.LastName,
                    Mobile = request.NumberPhone,
                    EmailAddress = request.Email,
                };
                unitOfWork.UserRepository.Add(userCreate);
                unitOfWork.Commit();
                //
                bool data = true;
                return Ok(RequestOK<bool>(data));
            }
            catch(Exception ex)
            {
                FileHelper.GeneratorFileByDay(ex.ToString(), MethodBase.GetCurrentMethod().Name);
                return Ok(ServerError());
            }
        }
        #endregion CreateAccount
    }
}