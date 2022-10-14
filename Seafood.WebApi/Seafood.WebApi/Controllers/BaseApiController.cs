using Seafood.Domain.Common.Enum;
using Seafood.Domain.Models.BaseModel;
using Seafood.Domain.Models.DataAccessModel;
using Seafood.Repository.EntityFamework;
using System;
using System.Collections.Generic;
using System.Linq;
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
        protected List<SeafoodPromotion> GetListSeafoodPromotion()
        {
            return unitOfWork.SeafoodPromotionRepository.AsQueryable().ToList();
        }
        protected string GetIconByCategory(string categoryCode)
        {
            return unitOfWork.CategoryRepository.FirstOrDefault(e => !e.IsDeleted && e.Code == categoryCode).Icon;
        }
        protected List<Image> GetListImageById(Guid id , int typeEnum)
        {
            if ((int)ImageTypeEnum.Product == typeEnum)
            {
                return unitOfWork.ImageRepository.Find(e => !e.IsDeleted && e.ProductId == id).ToList();
            }  
            else if ((int)ImageTypeEnum.Shop == typeEnum)
            {
                return unitOfWork.ImageRepository.Find(e => !e.IsDeleted && e.ShopSeefoodId == id).ToList();
            }
            else if ((int)ImageTypeEnum.More == typeEnum)
            {
                return unitOfWork.ImageRepository.Find(e => !e.IsDeleted && e.MoreImgId == id).ToList();
            }

            return null;
        }
        protected RequestBase<TRequest> Request_OK<TRequest>(TRequest data)
        {
            return new RequestBase<TRequest>()
            {
                Data = data,
                Success = true,
                StatusCode = (int)HttpStatusCode.OK,
                Message = Domain.Common.Constant.Message.Successful
            };
        }
        protected RequestBaseNoData Bad_Request()
        {
            return new RequestBaseNoData()
            {
                Success = false,
                StatusCode = (int)HttpStatusCode.BadRequest,
                Message = Domain.Common.Constant.Message.Bad_Request
            };
        }
        protected RequestBaseNoData Not_Found()
        {
            return new RequestBaseNoData()
            {
                Success = true,
                StatusCode = (int)HttpStatusCode.NotFound,
                Message = Domain.Common.Constant.Message.DATA_NOT_FOUND
            };
        }
        protected RequestBaseNoData Server_Error()
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