using Seafood.Domain.Common.Constant;
using Seafood.Domain.Common.Enum;
using Seafood.Domain.Common.FileLog;
using Seafood.Domain.Models.DataAccessModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Web;
using System.Web.Http;

namespace Seafood.WebApi.Controllers
{
    public class ProductController : BaseApiController
    {
        [HttpGet]
        [Route("api/Product/GetAllProd")]
        public IHttpActionResult GetAllProd()
        {
            try
            {
                var ipRequest = GetIp();   
                var products = (from prod in unitOfWork.ProductRepository.AsQueryable().Where(e => !e.IsDeleted)
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
                    return Ok(Not_Found());
                }

                dynamic data = products.ToList().OrderBy(fa => fa.Favourite).Reverse();
                return Ok(Request_OK<dynamic>(data));
            }
            catch(Exception ex)
            {
                FileHelper.GeneratorFileByDay(ex.ToString(), MethodBase.GetCurrentMethod().Name);
                return Ok(Server_Error());
            }
            
        }

        [HttpGet]
        [Route("api/Product/GetProdByCode")]
        public IHttpActionResult GetProdByCode(string code)
        {
            try
            {
                var ipRequest = GetIp();
                var products = (from prod in unitOfWork.ProductRepository.AsQueryable().Where(e => !e.IsDeleted && e.CategoryCode.Equals(code))
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
                    return Ok(Not_Found());
                }

                dynamic data = products.ToList().OrderBy(fa => fa.Favourite).Reverse();
                return Ok(Request_OK<dynamic>(data));
            }
            catch(Exception ex)
            {
                FileHelper.GeneratorFileByDay(ex.ToString(), MethodBase.GetCurrentMethod().Name);
                return Ok(Server_Error());
            }
        }

        [HttpGet]
        [Route("api/Product/GetProdDetailtById")]
        public IHttpActionResult GetProdDetailtById(Guid prodId)
        {
            try
            {
                var product = unitOfWork.ProductRepository.FirstOrDefault(
                e => !e.IsDeleted &&
                e.Id == prodId);

                if(product == null)
                {
                    return Ok(Not_Found());
                }    

                dynamic data = new
                {
                    Id = product.Id,
                    CategoryCode = product.CategoryCode,
                    Name = product.Name,
                    Description = product.Description,
                    ReviewProd = product.ReviewProd,
                    Price = product.Price,
                    PriceSale = product.PriceSale,
                    Amount = product.Amount,
                    Icon = GetIconByCategory(product.CategoryCode),
                    Images = GetListImageById(prodId, (int)ImageTypeEnum.Product),
                    ListProcessing = GetListProcessingByIdProd(prodId),
                    ListPromotion = GetListPromotionByIdProd(prodId),
                    ListProdInfo = GetListProdInfoByIdProd(prodId),
                    ListSeafoodPromotion = GetListSeafoodPromotion(),
                };
                return Ok(Request_OK<dynamic>(data));
            }
            catch (Exception ex)
            {
                FileHelper.GeneratorFileByDay(ex.ToString(), MethodBase.GetCurrentMethod().Name);
                return Ok(Server_Error());
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
                return Ok(Request_OK<dynamic>(data));
            }
            catch (Exception ex)
            {
                FileHelper.GeneratorFileByDay(ex.ToString(), MethodBase.GetCurrentMethod().Name);
                return Ok(Server_Error());
            }
        }

        #region PrivateMenthod
        private List<ProdProcessing> GetListProcessingByIdProd(Guid prodId)
        {
            return unitOfWork.ProdProcessingRepository.Find(e => !e.IsDeleted && (e.ProductId == prodId || e.Note.Equals("DENGUYEN") || e.Note.Equals("THEOYEUCAU"))).ToList();
        }
        private List<ProdInfo> GetListProdInfoByIdProd(Guid prodId)
        {
            return unitOfWork.ProdInfoRepository.Find(e => !e.IsDeleted && e.ProductId == prodId).ToList();
        }
        #endregion PrivateMenthod
    }
}