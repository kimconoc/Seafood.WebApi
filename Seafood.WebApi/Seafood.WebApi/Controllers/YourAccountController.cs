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
    public class YourAccountController : BaseApiController
    {
        [HttpGet]
        [Route("api/User/GetUserById")]
        public IHttpActionResult GetUserById(Guid userId)
        {
            try
            {
                var user = unitOfWork.UserRepository.FirstOrDefault(x => !x.IsDeleted && !x.IsLocked && x.Id == userId);
                if(user == null)
                    return Ok(Not_Found());
                dynamic data = user;
                return Ok(Request_OK<dynamic>(data));
            }
            catch (Exception ex)
            {
                FileHelper.GeneratorFileByDay(ex.ToString(), MethodBase.GetCurrentMethod().Name);
                return Ok(Server_Error());
            }
        }
    }
}