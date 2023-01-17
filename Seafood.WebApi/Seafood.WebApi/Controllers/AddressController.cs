using Seafood.Domain.Common.Enum;
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
                                 Id = address.Id,
                                 UserId = address.UserId,
                                 FullName = address.FullName,
                                 Mobile = address.Mobile,
                                 TypeAddress = address.TypeAddress,
                                 TypeAddressDetail = address.TypeAddressDetail,
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
            var address = unitOfWork.AddresseRepository.FirstOrDefault(x => !x.IsDeleted && x.Id == addressId);
            if(address == null)
            {
                return Ok(Bad_Request());
            }    
            var result = (from addre in unitOfWork.AddresseRepository.AsQueryable().Where(e => !e.IsDeleted && e.Id == addressId)
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
                             into resu
                             from res in resu.DefaultIfEmpty()
                             select new
                             {
                                 Id = address.Id,
                                 UserId = address.UserId,
                                 FullName = address.FullName,
                                 Mobile = address.Mobile,
                                 TypeAddress = address.TypeAddress,
                                 IsAddressMain = address.IsAddressMain,
                                 Address = address.Address,
                                 CodeWard = res.CodeWard,
                                 NameWard = res.NameWard,
                                 CodeDistrict = res.CodeDistrict,
                                 NameDistrict = res.NameDistrict,
                                 CodeRegion = res.CodeRegion,
                                 NameRegion = res.NameRegion,
                             }).FirstOrDefault();

            return Ok(Request_OK(result));
        }

        [HttpPost]
        [Route("api/Address/CreateAddressByUserId")]
        public IHttpActionResult CreateAddressByUserId([FromBody] AddressParameter addressModel)
        {
            if(addressModel == null)
                return Ok(Bad_Request());

            if (addressModel.IsAddressMain)
            {
                var litResult = unitOfWork.AddresseRepository.Find(x => !x.IsDeleted && x.UserId == addressModel.UserId).ToList();
                foreach (var item in litResult)
                {
                    item.IsAddressMain = false;
                    unitOfWork.AddresseRepository.Update(item);
                }
            }

            Addresse addresse = new Addresse();
            addresse.UserId = addressModel.UserId;
            addresse.FullName = addressModel.FullName;
            addresse.Mobile = addressModel.Mobile;
            addresse.CodeRegion = addressModel.CodeRegion;
            addresse.CodeDistrict = addressModel.CodeDistrict;
            addresse.CodeWard = addressModel.CodeWard;
            addresse.TypeAddressDetail = addressModel.typeAddressDetail;
            addresse.TypeAddress = (int)TypeAddressEnum.User;
            addresse.Address = addressModel.Address;
            addresse.IsAddressMain = addressModel.IsAddressMain;
            unitOfWork.AddresseRepository.Add(addresse);
            unitOfWork.Commit();
            return Ok(Request_OK(true));
        }

        [HttpPost]
        [Route("api/Address/UpdateAddressByUserId")]
        public IHttpActionResult UpdateAddressByUserId([FromBody] AddressParameter addressModel)
        {
            if (addressModel == null)
                return Ok(Bad_Request());

            var address = unitOfWork.AddresseRepository.FirstOrDefault(x => !x.IsDeleted && x.Id == addressModel.Id);
            if(!address.IsAddressMain)
            {
                if(addressModel.IsAddressMain)
                {
                    var litResult = unitOfWork.AddresseRepository.Find(x => !x.IsDeleted && x.UserId == addressModel.UserId).ToList();
                    foreach(var item in litResult)
                    {
                        item.IsAddressMain = false;
                        unitOfWork.AddresseRepository.Update(item);
                    }    
                }    
            }
            address.UserId = addressModel.UserId;
            address.FullName = addressModel.FullName;
            address.Mobile = addressModel.Mobile;
            address.CodeRegion = addressModel.CodeRegion;
            address.CodeDistrict = addressModel.CodeDistrict;
            address.CodeWard = addressModel.CodeWard;
            address.TypeAddressDetail = addressModel.typeAddressDetail;
            address.TypeAddress = (int)TypeAddressEnum.User;
            address.Address = addressModel.Address;
            address.IsAddressMain = addressModel.IsAddressMain;
            unitOfWork.AddresseRepository.Update(address);
            unitOfWork.Commit();
            return Ok(Request_OK(true));
        }

        [HttpDelete]
        [Route("api/Address/DeleteAddressById")]
        public IHttpActionResult DeleteAddressById(Guid addressId)
        {
            var address = unitOfWork.AddresseRepository.FirstOrDefault(x => !x.IsDeleted && x.Id == addressId);
            if (address == null || address.IsAddressMain)
                return Ok(Request_OK(false));
            unitOfWork.AddresseRepository.Delete(address);
            unitOfWork.Commit();
            return Ok(Request_OK(true));
        }
    }
}