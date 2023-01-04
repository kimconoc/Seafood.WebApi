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

            var basket = unitOfWork.BasketRepository.FirstOrDefault(x => x.UserId == basketModel.UserId && x.ProductId == basketModel.ProductId);
            if(basket != null)
            {
                basket.ProdProcessingId = basketModel.ProdProcessingId;
                basket.Note = basketModel.Note;
                basket.IsDeleted = false;
                unitOfWork.BasketRepository.Update(basket);

            }    
            else
            {
                unitOfWork.BasketRepository.Add(basketModel);
            }    
            unitOfWork.Commit();
            return Ok(Request_OK(true));
        }
        [HttpGet]
        [Route("api/Basket/GetBasketByUserId")]
        public IHttpActionResult GetBasketByUserId(Guid userId)
        {

            if (userId == null)
                return Ok(Bad_Request());

            var prodBaskets = (from bask in unitOfWork.BasketRepository.AsQueryable().Where(x => !x.IsDeleted && x.UserId == userId)
                                join prod in unitOfWork.ProductRepository.AsQueryable().Where(e => !e.IsDeleted)
                                on bask.ProductId equals prod.Id into res1
                                from resProd in res1.DefaultIfEmpty()
                                join imge in unitOfWork.ImageRepository.AsQueryable().Where(e => !e.IsDeleted && e.IsImageMain)
                                on bask.ProductId equals imge.ProductId into res2
                                from resImge in res2.DefaultIfEmpty()
                                join proces in unitOfWork.ProdProcessingRepository.AsQueryable().Where(e => !e.IsDeleted)
                                on bask.ProdProcessingId equals proces.Id into res3
                                from resProces in res3.DefaultIfEmpty()
                                select new
                                {
                                    Id = bask.Id,
                                    Imge = resImge.UrlPath,
                                    ProductId = resProd.Id,
                                    ProductName = resProd.Name,
                                    ProductDescription = resProd.Description,
                                    ProdProcessingId = resProces.Id,
                                    ProdProcessingName = resProces.Name,
                                    Price = resProd.Price,
                                    PriceSale = resProd.PriceSale,
                                });

            dynamic data = prodBaskets.ToList();
            return Ok(Request_OK<dynamic>(data));
        }
        [HttpPost]
        [Route("api/Basket/DeleteBasketById")]
        public IHttpActionResult DeleteBasketById([FromBody] List<Guid> lstBasket)
        {
            if (lstBasket == null || lstBasket.Count == 0)
                return Ok(Bad_Request());

            foreach(var item in lstBasket)
            {
                var basket = unitOfWork.BasketRepository.GetById(item);
                unitOfWork.BasketRepository.Delete(basket);
            }    
            unitOfWork.Commit();
            return Ok(Request_OK(true));
        }
    }
}