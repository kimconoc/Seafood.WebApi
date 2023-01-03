using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Seafood.WebApi.Controllers
{
    public class VoucherController : BaseApiController
    {
        [HttpGet]
        [Route("api/Voucher/GetVoucherByUserId")]
        public IHttpActionResult GetVoucherByUserId(Guid userId)
        {
            List<dynamic> result = new List<dynamic>();
            var voucherSeafoods = unitOfWork.VoucherSeafoodRepository.Find(x => !x.IsDeleted).ToList();
            if(voucherSeafoods != null && voucherSeafoods.Any())
            {
                result.AddRange(voucherSeafoods);
            }
            var vouchers = unitOfWork.VoucherRepository.Find(x => !x.IsDeleted && x.UserId == userId).ToList();
            if (vouchers != null && vouchers.Any())
            {
                result.AddRange(vouchers);
            }
            return Ok(Request_OK<List<dynamic>>(result));
        }
    }
}