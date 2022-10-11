using Seefood.Domain.Common.Constant;
using Seefood.Domain.Common.FileLog;
using Seefood.Domain.Models.DataAccessModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Web;
using System.Web.Http;

namespace Seefood.WebApi.Controllers
{
    public class ProductController : BaseApiController
    {
        [HttpGet]
        [Route("api/Product/GetAllProd")]
        public IHttpActionResult GetAllProd(string regionCode = Constant.Region_HaNoi)
        {
            try
            {
                var ipRequest = GetIp();   
                var products = (from prod in unitOfWork.ProductRepository.AsQueryable().Where(e => !e.IsDeleted && regionCode.Equals(e.RegionCode))
                                join favourite in unitOfWork.FavouriteProdRepository.AsQueryable().Where(e => !e.IsDeleted && e.IpRequest.Equals(ipRequest))
                                on prod.Id equals favourite.ProductId into res1
                                from x in res1.DefaultIfEmpty()
                                join prodPromotion in unitOfWork.ProdPromotionRepository.AsQueryable().Where(e => !e.IsDeleted && e.PromotionMain)
                                on prod.Id equals prodPromotion.ProductId into res2
                                from y in res2.DefaultIfEmpty()
                                select new
                                {
                                    Id = prod.Id,
                                    CategoryCode = prod.CategoryCode,
                                    RegionDistrictCode = prod.RegionDistrictCode,
                                    RegionCode = prod.RegionCode,
                                    Name = prod.Name,
                                    Description = prod.Description,
                                    DescPromotion = y.Content,
                                    ReviewProd = prod.ReviewProd,
                                    Favourite = x.ClassName,
                                    Price = prod.Price,
                                    PriceSale = prod.PriceSale,
                                    Amount = prod.Amount
                                });

                if (products == null || products.Count() == 0)
                {
                    return Ok(NotFound());
                }

                dynamic data = products.ToList().OrderBy(fa => fa.Favourite).Reverse();
                return Ok(RequestOK<dynamic>(data));
            }
            catch(Exception ex)
            {
                FileHelper.GeneratorFileByDay(ex.ToString(), MethodBase.GetCurrentMethod().Name);
                return Ok(ServerError());
            }
            
        }

        [HttpGet]
        [Route("api/Product/GetProdByCode")]
        public IHttpActionResult GetProdByCode(string code, string regionCode = Constant.Region_HaNoi)
        {
            try
            {
                var ipRequest = GetIp();
                var products = (from prod in unitOfWork.ProductRepository.AsQueryable().Where(e => !e.IsDeleted && e.CategoryCode.Equals(code) && regionCode.Equals(e.RegionCode))
                                join favourite in unitOfWork.FavouriteProdRepository.AsQueryable().Where(e => !e.IsDeleted && e.IpRequest.Equals(ipRequest))
                                on prod.Id equals favourite.ProductId into res1
                                from x in res1.DefaultIfEmpty()
                                join prodPromotion in unitOfWork.ProdPromotionRepository.AsQueryable().Where(e => !e.IsDeleted && e.PromotionMain)
                                on prod.Id equals prodPromotion.ProductId into res2
                                from y in res2.DefaultIfEmpty()
                                select new
                                {
                                    Id = prod.Id,
                                    CategoryCode = prod.CategoryCode,
                                    RegionDistrictCode = prod.RegionDistrictCode,
                                    RegionCode = prod.RegionCode,
                                    Name = prod.Name,
                                    Description = prod.Description,
                                    DescPromotion = y.Content,
                                    ReviewProd = prod.ReviewProd,
                                    Favourite = x.ClassName,
                                    Price = prod.Price,
                                    PriceSale = prod.PriceSale,
                                    Amount = prod.Amount
                                });

                if (products == null || products.Count() == 0)
                {
                    return Ok(NotFound());
                }

                dynamic data = products.ToList().OrderBy(fa => fa.Favourite).Reverse();
                return Ok(RequestOK<dynamic>(data));
            }
            catch(Exception ex)
            {
                FileHelper.GeneratorFileByDay(ex.ToString(), MethodBase.GetCurrentMethod().Name);
                return Ok(ServerError());
            }
        }

        [HttpGet]
        [Route("api/Product/ExecuteToFavourite")]
        public IHttpActionResult ExecuteToFavourite(Guid prodId,string className)
        {
            try
            {
                var obj = unitOfWork.FavouriteProdRepository.FirstOrDefault(x => x.ProductId == prodId);
                if(obj != null)
                {
                    obj.ClassName = className;
                    unitOfWork.FavouriteProdRepository.Update(obj);
                    unitOfWork.Commit();
                }    
                else
                {
                    var objExecute = new FavouriteProd()
                    {
                        ProductId = prodId,
                        IpRequest = GetIp(),
                        ClassName = className,
                    };
                    unitOfWork.FavouriteProdRepository.Add(objExecute);
                    unitOfWork.Commit();
                };  
                dynamic data = true;
                return Ok(RequestOK<dynamic>(data));
            }
            catch (Exception ex)
            {
                FileHelper.GeneratorFileByDay(ex.ToString(), MethodBase.GetCurrentMethod().Name);
                return Ok(ServerError());
            }
        }
    }
}