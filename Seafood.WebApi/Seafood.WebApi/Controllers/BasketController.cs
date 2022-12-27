using Seafood.Domain.Models.DataAccessModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Seafood.WebApi.Controllers
{
    public class BasketController : BaseApiController
    {
        [HttpPost]
        [Route("api/Basket/AddProductToBasket")]
        public IHttpActionResult AddProductToBasket([FromBody] Basket basketModel)
        {
            if (basketModel == null)
                return Ok(Bad_Request());

            var basket = unitOfWork.BasketRepository.FirstOrDefault(x => !x.IsDeleted && x.UserId == basketModel.UserId && x.ProductId == basketModel.ProductId);
            if(basket != null)
            {
                return Ok(Request_OK(true));
            }    

            unitOfWork.BasketRepository.Add(basketModel);
            unitOfWork.Commit();
            return Ok(Request_OK(true));
        }
    }
}