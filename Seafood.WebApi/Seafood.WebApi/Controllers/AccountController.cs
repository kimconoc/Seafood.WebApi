using Scrypt;
using Seafood.Domain.Common.Constant;
using Seafood.Domain.Models.ParameterModel;
using Seafood.Domain.Models.DataAccessModel;
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
using Seafood.Domain.Common.FileLog;
using Seafood.Domain.Models.BaseModel;
using Seafood.WebApi.Authentication;

namespace Seafood.WebApi.Controllers
{
    public class AccountController : BaseApiController
    {
        #region IsAuthori
        [HttpGet]
        [Route("api/Account/IsAuthori")]
        [SessionAuthorizeApi]
        public HttpResponseMessage IsAuthoriAPI()
        {
            var resp = new HttpResponseMessage();
            var username = GetUsername();
            var userData = unitOfWork.UserRepository.FirstOrDefault(s => s.Username.Trim().ToLower().Equals(username.Trim().ToLower()));
            dynamic data = new
            {
                Id = userData.Id,
                Username = userData.Username,
                Roles = userData.Roles,
                Fullname = userData.Fullname,
                FirstName = userData.FirstName,
                LastName = userData.LastName,
                MiddleName = userData.MiddleName,
                DisplayName = userData.DisplayName,
                Mobile = userData.Mobile,
                EmailAddress = userData.EmailAddress,
                IsAdminUser = userData.IsAdminUser,
                IsLocked = userData.IsLocked
            };
            var objIsAuthori = new RequestBase<dynamic>()
            {
                Data = data,
                Success = false,
                StatusCode = (int)HttpStatusCode.OK,
                Message = Domain.Common.Constant.Message.Successful
            };
            resp.Content = new ObjectContent<RequestBase<dynamic>>(objIsAuthori, new JsonMediaTypeFormatter());
            return resp;
        }
        #endregion IsAuthori
        #region Login
        [HttpPost]
        [Route("api/Account/Login")]
        public IHttpActionResult LoginAPI([FromBody] LoginParameterModel request)
        {
            if (!request.Validate())
                return Ok(Bad_Request());

            var userData = ValidateUser(request.Username, request.Password);
            if (userData == null)
            {
                var result = Bad_Request();
                result.Message = Message.LOGIN_ERROR;
                return Ok(result);
            }
            if (userData.IsLocked)
            {
                var result = Bad_Request();
                result.Message = Message.Account_LOCKED;
                return Ok(result);
            }

            // Đăng ký cookieValue nhảy vào starup
            ClaimsIdentity identity = CreateIdentity(userData);
            Request.GetOwinContext().Authentication.SignIn(identity);
            //
            dynamic data = new
            {
                Id = userData.Id,
                Username = userData.Username,
                Roles = userData.Roles,
                Fullname = userData.Fullname,
                FirstName = userData.FirstName,
                LastName = userData.LastName,
                MiddleName = userData.MiddleName,
                DisplayName = userData.DisplayName,
                Mobile = userData.Mobile,
                EmailAddress = userData.EmailAddress,
                IsAdminUser = userData.IsAdminUser,
                IsLocked = userData.IsLocked
            };
            return Ok(Request_OK<dynamic>(data));
        }

        private User ValidateUser(string username, string password)
        {
            string devAccounts = ConfigurationManager.AppSettings.Get("DevUsername");
            if (ConfigurationManager.AppSettings["HiddenError"].Equals("true") && devAccounts.Contains(username))
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

        private ClaimsIdentity CreateIdentity(User userData)
        {
            string username = string.IsNullOrEmpty(userData.Username) ? "" : userData.Username;
            string roles = string.IsNullOrEmpty(userData.Roles) ? "" : userData.Roles;
            string isAdminUser = userData.IsAdminUser? "true" : "false";
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
        public HttpResponseMessage LogoutAPI()
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

            var objLogout = new RequestBase<bool>()
            {
                Data = true,
                Success = false,
                StatusCode = (int)HttpStatusCode.OK,
                Message = Domain.Common.Constant.Message.Successful
            };
            resp.Content = new ObjectContent<RequestBase<bool>>(objLogout, new JsonMediaTypeFormatter());
            return resp;
        }

        private void RemoveSession(string session)
        {
            var session_id = session.Substring(0, 20);
            var sessionAuthorize = unitOfWork.SessionAuthorizeRepository.FirstOrDefault(
                e => !e.IsDeleted &&
                !string.IsNullOrEmpty(e.SessionId) &&
                e.SessionId == session_id
            );
            if (sessionAuthorize != null)
            {
                sessionAuthorize.Session = null;
                sessionAuthorize.SessionId = null;
                unitOfWork.SessionAuthorizeRepository.Update(sessionAuthorize);
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
                return Ok(Bad_Request());
            if (!Helper.ValidPhoneNumer(request.NumberPhone) || !Helper.IsValidEmail(request.Email)
                || string.IsNullOrEmpty(request.FirstName) || string.IsNullOrEmpty(request.LastName) || string.IsNullOrEmpty(request.Password))
                return Ok(Bad_Request());

            try
            {
                var user = unitOfWork.UserRepository.FirstOrDefault(s => s.Username.Trim().ToLower().Equals(request.NumberPhone.Trim().ToLower()));
                if (user != null)
                {
                    var result = Bad_Request();
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
                    Username = request.NumberPhone.Trim(),
                    PasswordHash = passwordHash.Trim(),
                    Fullname = request.FirstName.Trim() + " " + request.LastName.Trim(),
                    DisplayName = request.FirstName.Trim() + " " + request.LastName.Trim(),
                    Mobile = request.NumberPhone.Trim(),
                    EmailAddress = request.Email.Trim(),
                };
                unitOfWork.UserRepository.Add(userCreate);
                unitOfWork.Commit();
                //
                bool data = true;
                return Ok(Request_OK<bool>(data));
            }
            catch(Exception ex)
            {
                FileHelper.GeneratorFileByDay(ex.ToString(), MethodBase.GetCurrentMethod().Name);
                return Ok(Server_Error());
            }
        }
        #endregion CreateAccount
    }
}