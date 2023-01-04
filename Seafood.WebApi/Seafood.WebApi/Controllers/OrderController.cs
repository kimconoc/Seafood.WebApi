using Seafood.Domain.Common.Constant;
using Seafood.Domain.Common.Enum;
using Seafood.Domain.Common.FileLog;
using Seafood.Domain.Models.DataAccessModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;

namespace Seafood.WebApi.Controllers
{
    public class OrderController : BaseApiController
    {
        [HttpPost]
        [Route("api/Order/CreateOrderUserId")]
        public IHttpActionResult CreateOrderUserId([FromBody] List<Order> orders)
        {
            try
            {
                if (orders == null || orders.Any())
                    return Ok(Bad_Request());
                DateTime start = DateTime.Now;
                bool isVoucher = false;
                string voucher = String.Empty;
                while (!isVoucher && DateTime.Now.Subtract(start).TotalSeconds < 10)
                {
                    voucher = Helper.CreateCodeVoucher();
                    var orderByVoucher = unitOfWork.OrderRepository.FirstOrDefault(x => !x.IsDeleted && x.Code == voucher);
                    if (orderByVoucher == null)
                    {
                        isVoucher = true;
                    }
                    else
                    {
                        voucher = String.Empty;
                    }    
                }
                if(string.IsNullOrEmpty(voucher) || !VerifileTotalPrice(orders))
                    return Ok(Request_OK(false));

                foreach (var order in orders)
                {
                    order.Code = voucher;
                    order.TimeOrder = start;
                    order.Status = (int)StatusOrderEnum.DangXuLy;
                    unitOfWork.OrderRepository.Add(order);
                }    
                unitOfWork.Commit();
                return Ok(Request_OK(true));
            }
            catch(Exception ex)
            {
                FileHelper.GeneratorFileByDay(ex.ToString(), MethodBase.GetCurrentMethod().Name);
                return Ok(Request_OK(false));
            }
           
        }

        private bool VerifileTotalPrice(List<Order> orders)
        {
            int total = 0;
            string codeVoucher = orders[0].CodeVoucher;
            int typeVoucher = orders[0].TypeVoucher;
            int totalPrice = orders[0].TotalPrice;
            foreach (var order in orders)
            {
                var prod = unitOfWork.ProductRepository.FirstOrDefault(x => !x.IsDeleted && x.Id == order.ProductId);
                if (prod == null)
                    return false;

                total = prod.PriceSale * order.Quantity;
            }
            if (!string.IsNullOrEmpty(codeVoucher))
            {
                if (typeVoucher == (int)TypeVoucherEnum.Seafood)
                {
                    var voucher = unitOfWork.VoucherSeafoodRepository.FirstOrDefault(x => !x.IsDeleted && x.TypeVoucher == typeVoucher && x.Code == codeVoucher);
                    if (voucher != null)
                    {
                        if (total > voucher.ReductionAmount)
                        {
                            total = totalPrice - voucher.ReductionAmount;
                        }
                        else
                        {
                            total = 0;
                        }
                    }
                }
            }
            else if (typeVoucher == (int)TypeVoucherEnum.User)
            {
                var voucher = unitOfWork.VoucherRepository.FirstOrDefault(x => !x.IsDeleted && x.TypeVoucher == typeVoucher && x.Code == codeVoucher);
                if (voucher != null)
                {
                    if (total > voucher.ReductionAmount)
                    {
                        total = totalPrice - voucher.ReductionAmount;
                    }
                    else
                    {
                        total = 0;
                    }
                }
            }
            return total == totalPrice;
        }
    }
}