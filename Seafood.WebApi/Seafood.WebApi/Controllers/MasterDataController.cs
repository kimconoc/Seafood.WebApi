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

                var listObj = new List<ShopSeafood>();
                if (string.IsNullOrEmpty(region))
                {
                    listObj = unitOfWork.ShopSeafoodRepository.Find(x => !x.IsDeleted).ToList();
                }
                else
                {
                    listObj = unitOfWork.ShopSeafoodRepository.Find(x => !x.IsDeleted && x.RegionCode == region).ToList();
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
        [Route("api/MasterData/GetListRegion")]
        public IHttpActionResult GetListRegion(string txtSearch = "",string codeRegion = "", string codeDistrict = "")
        {
            try
            {

                var listObj = new List<Region>();
                if (!string.IsNullOrEmpty(codeRegion) && !string.IsNullOrEmpty(codeDistrict))
                {
                    listObj = unitOfWork.RegionRepository.Find(x => !x.IsDeleted && x.CodeRegion == codeRegion && x.CodeDistrict == codeDistrict && 
                    (string.IsNullOrEmpty(txtSearch) || x.NameWard.ToLower().GetVnStringOnlyCharactersAndNumbers().Contains(txtSearch.ToLower().GetVnStringOnlyCharactersAndNumbers()))).ToList();
                }
                else if (!string.IsNullOrEmpty(codeRegion))
                {
                    listObj = unitOfWork.RegionRepository.Find(x => !x.IsDeleted && x.CodeRegion == codeRegion &&
                    (string.IsNullOrEmpty(txtSearch) || x.NameDistrict.ToLower().GetVnStringOnlyCharactersAndNumbers().Contains(txtSearch.ToLower().GetVnStringOnlyCharactersAndNumbers()))).ToList();
                }
                else
                {
                    listObj = unitOfWork.RegionRepository.Find(x => !x.IsDeleted &&
                    (string.IsNullOrEmpty(txtSearch) || x.NameRegion.ToLower().GetVnStringOnlyCharactersAndNumbers().Contains(txtSearch.ToLower().GetVnStringOnlyCharactersAndNumbers()))).ToList();
                }    
                return Ok(Request_OK<dynamic>(listObj));
            }
            catch (Exception ex)
            {
                FileHelper.GeneratorFileByDay(ex.ToString(), MethodBase.GetCurrentMethod().Name);
                return Ok(Server_Error());
            }
        }
    }
}