using Seafood.Domain.Common.Constant;
using Seafood.Domain.Common.FileLog;
using Seafood.Domain.Models.DataAccessModel;
using Seafood.WebApi.Authentication;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Web;
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

        [HttpPost]
        [Route("api/User/UpdateAvarta")]
        public IHttpActionResult UpdateAvarta(Guid userId, HttpPostedFileBase imgUpload)
        {
            try
            {
                var user = unitOfWork.UserRepository.FirstOrDefault(x => !x.IsDeleted && !x.IsLocked && x.Id == userId);
                if (user == null)
                    return Ok(Not_Found());
                if (imgUpload != null)
                {
                    string filePath = string.Empty;
                    filePath = System.Web.Hosting.HostingEnvironment.MapPath("~/FileUpload/seafood/avarta-user/");
                    if (!Directory.Exists(filePath))
                    {
                        Directory.CreateDirectory(filePath);
                    }
                    filePath = filePath + Path.GetFileName(userId.ToString());
                    imgUpload.SaveAs(filePath);

                    user.Avarta = filePath;
                    unitOfWork.UserRepository.Update(user);
                    unitOfWork.Commit();

                }
                else
                {
                    return Ok(Bad_Request());
                }
                dynamic data = true;
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