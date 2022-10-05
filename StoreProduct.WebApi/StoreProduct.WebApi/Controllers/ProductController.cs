﻿using StoreProduct.Domain.Common.FileLog;
using StoreProduct.Domain.Models.DataAccessModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
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
            try
            {
                var ipRequest = GetIp();
                var products = (from prod in unitOfWork.ProductRepository.AsQueryable().Where(e => !e.IsDeleted)
                                join favourite in unitOfWork.FavouriteProdRepository.AsQueryable().Where(e => !e.IsDeleted && e.IpRequest.Equals(ipRequest))
                                on prod.Id equals favourite.ProductId into res
                                from x in res.DefaultIfEmpty()
                                select new
                                {
                                    Id = prod.Id,
                                    CategoryCode = prod.CategoryCode,
                                    ImgeProdId = prod.ImgeProdId,
                                    Name = prod.Name,
                                    DescPromotion = prod.DescPromotion,
                                    Description = prod.Description,
                                    Outstanding = prod.Outstanding,
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
        public IHttpActionResult GetProdByCode(string code)
        {
            try
            {
                var products = unitOfWork.ProductRepository.AsQueryable();

                if (products == null || products.Count() == 0)
                {
                    return Ok(NotFound());
                }

                dynamic data = products.Where(x => x.CategoryCode == code).ToList();
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
                var obj = unitOfWork.FavouriteProdRepository.FirstOrDefault(x => x.Id.Equals(prodId));
                if(obj != null)
                {
                    obj.ClassName = className;
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