using Seafood.Domain.Models.DataAccessModel;
using Seafood.WebApi.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Seafood.WebApi.Controllers
{
    [SessionAuthorizeApi]
    public class AddressController : BaseApiController
    {
        [HttpGet]
        [Route("api/Address/GetListAddressByUserId")]
        public IHttpActionResult GetListAddressByUserId(Guid userId)
        {
            var litResult = new List<Addresse>();
            litResult = unitOfWork.AddresseRepository.Find(x => !x.IsDeleted && x.UserId == userId).ToList();
            return Ok(Request_OK<dynamic>(litResult));
        }
    }
}