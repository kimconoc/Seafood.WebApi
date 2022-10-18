﻿using Seafood.Domain.Common.FileLog;
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
        [Route("api/Product/GetInfoShopSeeFood")]
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
    }
}