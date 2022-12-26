using Seafood.Domain.Models.DataAccessModel;
using Seafood.Domain.Models.ParameterModel;
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
            var litResult = (from address in unitOfWork.AddresseRepository.AsQueryable().Where(e => !e.IsDeleted && e.UserId == userId)
                            join region in unitOfWork.RegionRepository.AsQueryable().Where(e => !e.IsDeleted)
                            on new
                            {
                                key1 = address.CodeRegion,
                                key2 = address.CodeDistrict,
                                key3 = address.CodeWard,
                            }
                            equals new
                            {
                                 key1 = region.CodeRegion,
                                 key2 = region.CodeDistrict,
                                 key3 = region.CodeWard,
                             }
                             into result
                             from res in result.DefaultIfEmpty()
                             select new
                             {
                                 Id = res.Id,
                                 UserId = address.UserId,
                                 FullName = address.FullName,
                                 Mobile = address.Mobile,
                                 TypeAddress = address.TypeAddress,
                                 IsAddressMain = address.IsAddressMain,
                                 Address = address.Address,
                                 NameWard = res.NameWard,
                                 NameDistrict = res.NameDistrict,
                                 NameRegion = res.NameRegion,
                             });

            if (litResult == null || litResult.Count() == 0)
            {
                return Ok(Not_Found());
            }
            dynamic data = litResult.ToList();
            return Ok(Request_OK<dynamic>(litResult));
        }

        [HttpGet]
        [Route("api/Address/GetAddressByUserId")]
        public IHttpActionResult GetAddressByUserId(Guid addressId)
        {
            var result = unitOfWork.AddresseRepository.FirstOrDefault(x => !x.IsDeleted && x.Id == addressId);
            return Ok(Request_OK(result));
        }

        [HttpPost]
        [Route("api/Address/CreateAddressByUserId")]
        public IHttpActionResult CreateAddressByUserId([FromBody] AddressParameter address)
        {
            if(address == null)
                return Ok(Bad_Request());

            if (address.IsAddressMain)
            {
                var litResult = unitOfWork.AddresseRepository.Find(x => !x.IsDeleted && x.UserId == address.UserId).ToList();
                foreach (var item in litResult)
                {
                    item.IsAddressMain = false;
                    unitOfWork.AddresseRepository.Update(item);
                }
            }

            Addresse addresse = new Addresse();
            addresse.UserId = address.UserId;
            addresse.FullName = address.FullName;
            addresse.Mobile = address.Mobile;
            addresse.CodeRegion = address.CodeRegion;
            addresse.CodeDistrict = address.CodeDistrict;
            addresse.CodeWard = address.CodeWard;
            addresse.TypeAddress = address.TypeAddress;
            addresse.Address = address.Address;
            addresse.IsAddressMain = address.IsAddressMain;
            unitOfWork.AddresseRepository.Add(addresse);
            unitOfWork.Commit();
            return Ok(Request_OK(true));
        }

        [HttpPost]
        [Route("api/Address/UpdateAddressByUserId")]
        public IHttpActionResult UpdateAddressByUserId([FromBody] AddressParameter address)
        {
            if (address == null)
                return Ok(Bad_Request());

            var result = unitOfWork.AddresseRepository.FirstOrDefault(x => !x.IsDeleted && x.Id == address.Id);
            if(!result.IsAddressMain)
            {
                if(address.IsAddressMain)
                {
                    var litResult = unitOfWork.AddresseRepository.Find(x => !x.IsDeleted && x.UserId == address.UserId).ToList();
                    foreach(var item in litResult)
                    {
                        item.IsAddressMain = false;
                        unitOfWork.AddresseRepository.Update(item);
                    }    
                }    
            }
            result.UserId = address.UserId;
            result.FullName = address.FullName;
            result.Mobile = address.Mobile;
            result.CodeRegion = address.CodeRegion;
            result.CodeDistrict = address.CodeDistrict;
            result.CodeWard = address.CodeWard;
            result.TypeAddress = address.TypeAddress;
            result.Address = address.Address;
            result.IsAddressMain = address.IsAddressMain;
            unitOfWork.AddresseRepository.Update(result);
            unitOfWork.Commit();
            return Ok(Request_OK(true));
        }

        [HttpDelete]
        [Route("api/Address/DeleteAddressByUserId")]
        public IHttpActionResult DeleteAddressByUserId(Guid addressId)
        {
            var result = unitOfWork.AddresseRepository.FirstOrDefault(x => !x.IsDeleted && x.Id == addressId);
            if (result == null)
                return Ok(Request_OK(false));
            unitOfWork.AddresseRepository.Delete(result);
            unitOfWork.Commit();
            return Ok(Request_OK(true));
        }
    }
}