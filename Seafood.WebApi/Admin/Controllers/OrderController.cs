using Amin.Controllers;
using Amin.CustomAuthen;
using Amin.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Dynamic;
using static Admin.Common.Constant;
using Seafood.Domain.Common.Extentions;
using Seafood.Domain.Common.Constant;
using Seafood.Domain.Common.Enum;

namespace Admin.Controllers
{
    [SessionAuthen(Role = AdminRoles.SuperAdmin)]
    public class OrderController : BaseController
    {
        // GET: Order
        public ActionResult ListOrder()
        {
            return View();
        }
        public ActionResult GetListOrder(DataTablesQueryModel queryModel)
        {
            int totalResultsCount;
            var take = queryModel.length;
            var skip = queryModel.start;
            string sortBy = "";
            string sortDir = "";
            if (queryModel.order != null)
            {
                sortBy = queryModel.columns[queryModel.order[0].column].data;
                sortDir = queryModel.order[0].dir.ToLower();
            }
            var list = unitOfWork.OrderRepository.AsQueryable().Where(x => !x.IsDeleted);
            var filterCode = queryModel.columns.First(s => s.name == "Code").search.value;
            var filterMobile = queryModel.columns.First(s => s.name == "Mobile").search.value;
            var filterStatus = queryModel.columns.First(s => s.name == "Status").search.value;
            if (!string.IsNullOrEmpty(filterCode))
            {
                list = list.Where(s => s.Code.Trim().ToLower().Contains(filterCode.ToLower()));
            }
            if (!string.IsNullOrEmpty(filterStatus) && int.Parse(filterStatus) >= 0)
            {
                int status = int.Parse(filterStatus);
                list = list.Where(s => s.Status.Equals(status));
            }
            
            var litResult = from resOrder in list
                            join user in unitOfWork.UserRepository.AsQueryable().Where(e => !e.IsDeleted)
                            on resOrder.UserId equals user.Id into res1
                            from resUser in res1.DefaultIfEmpty()
                            join prod in unitOfWork.ProductRepository.AsQueryable().Where(e => !e.IsDeleted)
                            on resOrder.ProductId equals prod.Id into res2
                            from resProd in res2.DefaultIfEmpty()
                            join proces in unitOfWork.ProdProcessingRepository.AsQueryable().Where(e => !e.IsDeleted)
                            on resOrder.ProdProcessingId equals proces.Id into res3
                            from resProces in res3.DefaultIfEmpty()
                            where string.IsNullOrEmpty(filterMobile) || resUser.Mobile.Contains(filterMobile)
                            orderby resOrder.CreatedAt
                            select new
                            {
                                Id = resOrder.Id,
                                DisplayName = resUser.DisplayName,
                                Code = resOrder.Code,
                                Mobile = resUser.Mobile,
                                Status = resOrder.Status,
                                Product = resProd.Name,
                                Quantity = resOrder.Quantity,
                                ProdProcessingsName = resProces.Name,
                                ProdProcessingsPrice = resProces.Price,
                                CodeVoucher = resOrder.CodeVoucher,
                                TypeVoucher = resOrder.TypeVoucher,
                                TotalPrice = resOrder.TotalPrice,
                                TimeOrder = resOrder.TimeOrder,
                                TimeDeliveryStart = resOrder.TimeDeliveryStart,
                                EstimateDeliveryTime = resOrder.EstimateDeliveryTime,
                                SuccessfulDeliveryTime = resOrder.SuccessfulDeliveryTime,
                                CancellationTime = resOrder.CancellationTime,
                                UpdatedAt = resOrder.UpdatedAt,
                            };

            totalResultsCount = litResult.Count();
            var result = litResult.OrderBy(sortBy + (sortDir == "desc" ? " descending" : "")).Skip(skip).Take(take).ToList()
              .Select(x => new
               {
                  Id = x.Id,
                  DisplayName = x.DisplayName,
                  Code = x.Code,
                  Mobile = x.Mobile,
                  Status = GetStatusNameOrder(x.Status),
                  Product = x.Product,
                  Quantity = x.Quantity,
                  ProdProcessingsName = x.ProdProcessingsName,
                  ProdProcessingsPrice = Helper.FomatToTypeMoney(x.ProdProcessingsPrice),
                  Voucher = x.CodeVoucher == null && x.TypeVoucher == null ? "" : GetInfoVoucher(x.CodeVoucher, x.TypeVoucher),
                  TotalPrice = Helper.FomatToTypeMoney(x.TotalPrice),
                  TimeOrder = x.TimeOrder.FormatDatetimeToHourMinDayMonthYear(),
                  TimeDeliveryStart = x.TimeDeliveryStart != null ? StringExtension.FormatDatetimeToHourMinDayMonthYear(x.TimeDeliveryStart.Value) : null,
                  EstimateDeliveryTime = x.EstimateDeliveryTime != null ? StringExtension.FormatDatetimeToHourMinDayMonthYear(x.EstimateDeliveryTime.Value) : null,
                  SuccessfulDeliveryTime = x.SuccessfulDeliveryTime != null ? StringExtension.FormatDatetimeToHourMinDayMonthYear(x.SuccessfulDeliveryTime.Value) : null,
                  CancellationTime = x.CancellationTime != null ? StringExtension.FormatDatetimeToHourMinDayMonthYear(x.CancellationTime.Value) : null,
                  UpdatedAt = x.UpdatedAt != null ? StringExtension.FormatDatetimeToHourMinDayMonthYear(x.UpdatedAt.Value) : null,
              });
            return Json(new
            {
                queryModel.draw,
                recordsTotal = totalResultsCount,
                recordsFiltered = totalResultsCount,
                data = result
            });
        }
        private string GetInfoVoucher(string codeVoucher, int? typeVoucher)
        {
            string info = string.Empty;
            switch (typeVoucher)
            {
                case (int)TypeVoucherEnum.Seafood:
                    var res1 = unitOfWork.VoucherSeafoodRepository.FirstOrDefault(x => !x.IsDeleted & x.Code == codeVoucher & x.TypeVoucher == typeVoucher);
                    info = string.Format(res1?.Code + "<br />" + res1.Title + "<br />" + Helper.FomatToTypeMoney(res1.ReductionAmount));
                    break;
                case (int)TypeVoucherEnum.User:
                    var res2 = unitOfWork.VoucherRepository.FirstOrDefault(x => !x.IsDeleted & x.Code == codeVoucher & x.TypeVoucher == typeVoucher);
                    info = string.Format(res2?.Code + "<br />" + res2.Title + "<br />" + Helper.FomatToTypeMoney(res2.ReductionAmount));
                    break;
            }

            return info;


        }
    }
}