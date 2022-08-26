using Scrypt;
using StoreProduct.Domain.Common.Constant;
using StoreProduct.Domain.Models.Account;
using StoreProduct.Domain.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web;
using System.Web.Http;

namespace StoreProduct.WebApi.Controllers
{
    public class AccountController : BaseApiController
    {
        [HttpPost]
        //[CSRFCheck]
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

            ClaimsIdentity identity = CreateIdentity(user);
            Request.GetOwinContext().Authentication.SignIn(identity);
            return Content(HttpStatusCode.OK, new { Token = "a" });
        }

        private User ValidateUser(string username, string password)
        {
            //string devAccounts = ConfigurationManager.AppSettings.Get("DEV_ACCOUNT");
            //if (ConfigurationManager.AppSettings["HiddenError"].Equals("false") && devAccounts.Contains(username))
            //    return unitOfWork.UserRepository.FirstOrDefault(s => s.Username.Equals(username));

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
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, roles),
                new Claim("Roles", role),
            };

            var identity = new ClaimsIdentity(claims, "ApplicationCookie");
            return identity;
        }

    }
}