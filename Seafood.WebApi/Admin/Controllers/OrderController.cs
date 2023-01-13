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
            var filterName = queryModel.search.value;
            if (!string.IsNullOrEmpty(filterName))
            {
                list = list.Where(s => s.Code.Trim().ToLower().Contains(filterName.ToLower()));
            }
            totalResultsCount = list.Count();
            var result = list.OrderByDescending(t => t.CreatedAt).OrderBy(sortBy + (sortDir == "desc" ? " descending" : "")).Skip(skip).Take(take).ToList()
                 .Select(x => new
                 {
                     Id = x.Id,
                     CreatedAt = x.CreatedAt.FormatDatetimeToHourMinDayMonthYear(),
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
    }
}