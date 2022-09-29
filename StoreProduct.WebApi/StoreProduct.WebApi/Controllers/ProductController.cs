using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace StoreProduct.WebApi.Controllers
{
    public class ProductController : BaseApiController
    {
        [HttpGet]
        [Route("api/Product/GetAllProd")]
        public IHttpActionResult GetAllProd()
        {
            var products = unitOfWork.ProductRepository.AsQueryable();

            if(products == null || products.Count() == 0)
            {
                return Ok(NotFound());
            }    

            dynamic data = products.ToList();
            return Ok(RequestOK<dynamic>(data));
        }

        [HttpGet]
        [Route("api/Product/GetProdByCode")]
        public IHttpActionResult GetProdByCode(string code)
        {
            var products = unitOfWork.ProductRepository.AsQueryable();

            if (products == null || products.Count() == 0)
            {
                return Ok(NotFound());
            }

            dynamic data = products.Where(x => x.CategoryCode == code).ToList();
            return Ok(RequestOK<dynamic>(data));
        }
    }
}