using Seafood.Domain.Common.Constant;
using Seafood.Domain.Common.FileLog;
using Seafood.Domain.Models.DataAccessModel;
using Seafood.WebApi.Authentication;
using System;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Web.Http;

namespace Seafood.WebApi.Controllers
{
    [SessionAuthorizeApi]
    public class UserController : BaseApiController
    {
        [HttpGet]
        [Route("api/User/Info")]
        [Permission(Code = "XEM_USER")]
        public IHttpActionResult GetUser()
        {
            try
            {
                var user = unitOfWork.UserRepository.Find(x => !x.IsDeleted).Select(e => new User
                {
                    Id = e.Id,
                    Username = e.Username,
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    Fullname = e.Fullname,
                    IsAdminUser = e.IsAdminUser,
                    IsLocked = e.IsLocked,
                    EmailAddress = e.EmailAddress,
                    Mobile = e.Mobile
                }).ToList(); ;
                return Content(HttpStatusCode.OK, new
                {
                    User = user,
                });
            }
            catch (Exception ex)
            {
                FileHelper.GeneratorFileByDay(ex.ToString(), MethodBase.GetCurrentMethod().Name);
                return Content(HttpStatusCode.BadRequest, Message.Bad_Request);
            }
        }
    }
}