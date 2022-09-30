using StoreProduct.Domain.Common.Constant;
using StoreProduct.Domain.Common.FileLog;
using StoreProduct.Domain.Models.DataAccessModel;
using StoreProduct.WebApi.Authentication;
using System;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Web.Http;

namespace StoreProduct.WebApi.Controllers
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