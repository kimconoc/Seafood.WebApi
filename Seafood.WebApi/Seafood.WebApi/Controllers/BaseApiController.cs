using Seafood.Domain.Models.BaseModel;
using Seafood.Repository.EntityFamework;
using System.Net;
using System.ServiceModel.Channels;
using System.Web;
using System.Web.Http;

namespace Seafood.WebApi.Controllers
{
    public class BaseApiController : ApiController
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
        protected string GetIp()
        {
            if (Request.Properties.ContainsKey("MS_HttpContext"))
            {
                return ((HttpContextWrapper)Request.Properties["MS_HttpContext"]).Request.UserHostAddress;
            }
            else if (Request.Properties.ContainsKey(RemoteEndpointMessageProperty.Name))
            {
                RemoteEndpointMessageProperty prop = (RemoteEndpointMessageProperty)Request.Properties[RemoteEndpointMessageProperty.Name];
                return prop.Address;
            }
            else if (HttpContext.Current != null)
            {
                return HttpContext.Current.Request.UserHostAddress;
            }
            else
            {
                return null;
            }
        }
        protected RequestBase<TRequest> RequestOK<TRequest>(TRequest data)
        {
            return new RequestBase<TRequest>()
            {
                Data = data,
                Success = true,
                StatusCode = (int)HttpStatusCode.OK,
                Message = Domain.Common.Constant.Message.Successful
            };
        }
        protected RequestBaseNoData BadRequest()
        {
            return new RequestBaseNoData()
            {
                Success = false,
                StatusCode = (int)HttpStatusCode.BadRequest,
                Message = Domain.Common.Constant.Message.Bad_Request
            };
        }
        protected RequestBaseNoData NotFound()
        {
            return new RequestBaseNoData()
            {
                Success = true,
                StatusCode = (int)HttpStatusCode.NotFound,
                Message = Domain.Common.Constant.Message.DATA_NOT_FOUND
            };
        }
        protected RequestBaseNoData ServerError()
        {
            return new RequestBaseNoData()
            {
                Success = false,
                StatusCode = (int)HttpStatusCode.InternalServerError,
                Message = Domain.Common.Constant.Message.INTERAL_SERVER_ERROR
            };
        }
    }
}