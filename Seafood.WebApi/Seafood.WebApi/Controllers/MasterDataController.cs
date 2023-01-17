using Seafood.Domain.Common.Extentions;
using Seafood.Domain.Common.FileLog;
using Seafood.Domain.Models.DataAccessModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Http;

namespace Seafood.WebApi.Controllers
{
    public class MasterDataController : BaseApiController
    {
        [HttpGet]
        [Route("api/MasterData/GetInfoShopSeeFood")]
        public IHttpActionResult GetInfoShopSeeFood(string region = "")
        {
            try
            {
                var listObj = from shop in unitOfWork.ShopSeafoodRepository.AsQueryable().Where(e => !e.IsDeleted)
                           join reg in unitOfWork.RegionRepository.AsQueryable().Where(e => !e.IsDeleted)
                           on new
                           {
                               key1 = shop.CodeRegion,
                               key2 = shop.CodeDistrict,
                               key3 = shop.CodeWard,
                           }
                           equals new
                           {
                               key1 = reg.CodeRegion,
                               key2 = reg.CodeDistrict,
                               key3 = reg.CodeWard,
                           } 
                           into result
                           from res in result.DefaultIfEmpty()
                           select new
                           {
                               Id = shop.Id,
                               Name = shop.Name,
                               Mobile = shop.Mobile,
                               TypeAddress = shop.TypeAddress,
                               Address = shop.Address,
                               NameWard = res.NameWard,
                               NameDistrict = res.NameDistrict,
                               NameRegion = res.NameRegion,
                           };

                return Ok(Request_OK<dynamic>(listObj));
            }
            catch (Exception ex)
            {
                FileHelper.GeneratorFileByDay(ex.ToString(), MethodBase.GetCurrentMethod().Name);
                return Ok(Server_Error());
            }
        }

        [HttpGet]
        [Route("api/MasterData/GetListSeafoodPromotion")]
        public IHttpActionResult GetListSeafoodPromotion(string shopCode = "")
        {
            try
            {

                var listObj = new List<SeafoodPromotion>();
                if (string.IsNullOrEmpty(shopCode))
                {
                    listObj = unitOfWork.SeafoodPromotionRepository.Find(x => !x.IsDeleted).ToList();
                }
                else
                {
                    listObj = unitOfWork.SeafoodPromotionRepository.Find(x => !x.IsDeleted && x.ShopCode == shopCode).ToList();
                }
                return Ok(Request_OK<dynamic>(listObj));
            }
            catch (Exception ex)
            {
                FileHelper.GeneratorFileByDay(ex.ToString(), MethodBase.GetCurrentMethod().Name);
                return Ok(Server_Error());
            }
        }
        [HttpGet]
        [Route("api/MasterData/GetListPromotionByProdId")]
        public IHttpActionResult GetListPromotionByProdId(Guid prodId)
        {
            try
            {

                var listObj = unitOfWork.ProdPromotionRepository.Find(e => !e.IsDeleted && e.ProductId == prodId).ToList();
                return Ok(Request_OK<dynamic>(listObj));
            }
            catch (Exception ex)
            {
                FileHelper.GeneratorFileByDay(ex.ToString(), MethodBase.GetCurrentMethod().Name);
                return Ok(Server_Error());
            }
        }

        [HttpGet]
        [Route("api/MasterData/GetListRegion")]
        public IHttpActionResult GetListRegion(string txtSearch = "",string codeRegion = "", string codeDistrict = "")
        {
            try
            {

                var listObj = new List<Region>();
                if (!string.IsNullOrEmpty(codeRegion) && !string.IsNullOrEmpty(codeDistrict))
                {
                    listObj = unitOfWork.RegionRepository.Find(x => !x.IsDeleted && x.CodeRegion == codeRegion && x.CodeDistrict == codeDistrict && 
                    (string.IsNullOrEmpty(txtSearch) || x.NameWard.ToLower().Contains(txtSearch.ToLower()))).ToList();
                }
                else if (!string.IsNullOrEmpty(codeRegion))
                {
                    listObj = unitOfWork.RegionRepository.Find(x => !x.IsDeleted && x.CodeRegion == codeRegion &&
                    (string.IsNullOrEmpty(txtSearch) || x.NameDistrict.ToLower().Contains(txtSearch.ToLower()))).ToList();
                }
                else
                {
                    listObj = unitOfWork.RegionRepository.Find(x => !x.IsDeleted &&
                    (string.IsNullOrEmpty(txtSearch) || x.NameRegion.ToLower().Contains(txtSearch.ToLower()))).ToList();
                }    
                return Ok(Request_OK<dynamic>(listObj));
            }
            catch (Exception ex)
            {
                FileHelper.GeneratorFileByDay(ex.ToString(), MethodBase.GetCurrentMethod().Name);
                return Ok(Server_Error());
            }
        }

        [HttpGet]
        [Route("api/MasterData/GetCountBasketByUserId")]
        public IHttpActionResult GetCountBasketByUserId(Guid userId)
        {
            int count = 0;
            if (userId == null)
                return Ok(Bad_Request());

            var prodBaskets = unitOfWork.BasketRepository.AsQueryable().Where(x => !x.IsDeleted && x.UserId == userId);
            count = prodBaskets.Count();
            return Ok(Request_OK(count));
        }
    }
}