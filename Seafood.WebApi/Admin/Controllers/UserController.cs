using Amin.Controllers;
using Amin.CustomAuthen;
using Amin.Model;
using Seafood.Domain.Models.DataAccessModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static Admin.Common.Constant;
using System.Linq.Dynamic;
using Seafood.Domain.Common.Extentions;

namespace Admin.Controllers
{
    [SessionAuthen(Role = AdminRoles.SuperAdmin)]
    public class UserController : BaseController
    {
        public ActionResult ListUser()
        {
            return View();
        }
        public ActionResult GetlistUser(Search search)
        {
            return PartialView("_PartialViewListUser", new List<User>());
        }
        public ActionResult GetListUsers(DataTablesQueryModel queryModel)
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
            var list = unitOfWork.UserRepository.AsQueryable();
            var filterName = queryModel.search.value;
            if (!string.IsNullOrEmpty(filterName))
            {
                list = list.Where(s => s.DisplayName.Trim().ToLower().Contains(filterName.ToLower()));
            }
            totalResultsCount = list.Count();
            var result = list.OrderByDescending(t => t.CreatedAt).OrderBy(sortBy + (sortDir == "desc" ? " descending" : "")).Skip(skip).Take(take).ToList()
                 .Select(x => new
                 {
                     Id = x.Id,
                     DisplayName = x.DisplayName,
                     Mobile = x.Mobile.Trim(),
                     Email = x.Email,
                     Sex = x.Sex != 0 && x.Sex != 1 ? "" : x.Sex == 0? "Nữ" : "Nam",
                     CreatedAt = x.CreatedAt.FormatDatetimeToHourMinDayMonthYear(),
                     UpdatedAt = x.UpdatedAt != null ? StringExtension.FormatDatetimeToHourMinDayMonthYear(x.UpdatedAt.Value) : null,
                     Birthday = x.Birthday != null ? StringExtension.FormatDatetimeToDayMonthYear(x.Birthday.Value) : null,
                     IsLocked = x.IsLocked,
                     StrIsLocked = x.IsLocked? "Đã khóa":"Hoạt động",
                     IsDeleted = x.IsDeleted? "Đã xóa" : "Hoạt động",
                 });  
            return Json(new
            {
                queryModel.draw,
                recordsTotal = totalResultsCount,
                recordsFiltered = totalResultsCount,
                data = result
            });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeactivateUser(Guid id)
        {
            var user = unitOfWork.UserRepository.GetById(id);
            if (user == null)
                return Json(Bad_Request());
            else
            {
                user.IsLocked = true;
                unitOfWork.UserRepository.Update(user);
                unitOfWork.Commit();
            }
            return Json(Success_Request());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ActivateUser(Guid id)
        {
            var user = unitOfWork.UserRepository.GetById(id);
            if (user == null)
                return Json(Bad_Request());
            else
            {
                user.IsLocked = false;
                unitOfWork.UserRepository.Update(user);
                unitOfWork.Commit();
            }
            return Json(Success_Request());
        }
    }
}