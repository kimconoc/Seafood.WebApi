using Amin.MemCached;
using Amin.Models;
using Newtonsoft.Json;
using Seafood.Domain.Common.Constant;
using Seafood.Domain.Common.Enum;
using Seafood.Domain.Common.Extentions;
using Seafood.Domain.Common.FileLog;
using Seafood.Repository.EntityFamework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Amin.Controllers
{
    public class BaseController : Controller
    {
        protected IUnitOfWork unitOfWork = new EfUnitOfWork();
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                unitOfWork.Dispose();
            }
            base.Dispose(disposing);
        }

        public UserData GetCurrentUser()
        {
            UserData userData = null;
            var signinTokenCookie = Request.Cookies[GetSigninToken()];
            if (signinTokenCookie != null && !string.IsNullOrEmpty(signinTokenCookie.Value))
            {
                try
                {
                    var token = FormsAuthentication.Decrypt(signinTokenCookie.Value);
                    userData = JsonConvert.DeserializeObject<UserData>(token.UserData);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return userData;
        }
        private static string GetSigninToken()
        {
            return FormsAuthentication.FormsCookieName;
        }
        protected string GetStatusNameOrder(int typeStatusOrder)
        {
            string statusName = string.Empty;
            switch (typeStatusOrder)
            {
                case (int)StatusOrderEnum.DangXuLy:
                    statusName = StatusOrderEnum.DangXuLy.GetDescription();
                    break;
                case (int)StatusOrderEnum.DangVanChuyen:
                    statusName = StatusOrderEnum.DangVanChuyen.GetDescription();
                    break;
                case (int)StatusOrderEnum.DonDaGiao:
                    statusName = StatusOrderEnum.DonDaGiao.GetDescription();
                    break;
                case (int)StatusOrderEnum.DonDaHuy:
                    statusName = StatusOrderEnum.DonDaHuy.GetDescription();
                    break;
            }
            return statusName;
        }
        protected override void OnException(ExceptionContext filterContext)
        {
            FileHelper.GeneratorFileByDay(filterContext.Exception.Message, MethodBase.GetCurrentMethod().Name);
            filterContext.ExceptionHandled = true;
            filterContext.Result = RedirectToAction("Login", "Account");
        }

        protected DataResponse<TRequest> Success_Request<TRequest>(TRequest data)
        {
            return new DataResponse<TRequest>()
            {
                Data = data,
                Success = true,
                StatusCode = (int)HttpStatusCode.OK,
                Message = "Thành công"
            };
        }

        protected StatusResponse Success_Request()
        {
            return new StatusResponse()
            {
                Success = true,
                StatusCode = (int)HttpStatusCode.OK,
                Message = "Thành công"
            };
        }

        protected StatusResponse Not_Found(string message = "")
        {
            return new StatusResponse()
            {
                Success = true,
                StatusCode = (int)HttpStatusCode.NotFound,
                Message = string.IsNullOrEmpty(message) ? "Không tìm thấy dữ liệu" : message
            };
        }

        protected StatusResponse Bad_Request(string message = "")
        {
            return new StatusResponse()
            {
                Success = false,
                StatusCode = (int)HttpStatusCode.BadRequest,
                Message = string.IsNullOrEmpty(message)? "Dữ liệu định dạng sai": message
            };
        }

        protected StatusResponse Server_Error(string message = "")
        {
            return new StatusResponse()
            {
                Success = false,
                StatusCode = (int)HttpStatusCode.InternalServerError,
                Message = string.IsNullOrEmpty(message) ? "Có lỗi trong quá trình xử lý" : message
            };
        }
    }
}