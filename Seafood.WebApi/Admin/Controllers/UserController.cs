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
            List<User> res = new List<User>();
            var user = unitOfWork.UserRepository.Find(x => !x.IsDeleted).ToList();
            for(int i = 1; i<10; i++)
            {
                res.AddRange(user);
            }    
            return Json(new
            {
                queryModel.draw,
                recordsTotal = 215,
                recordsFiltered = 215,
                data = res
            });
        }
    }
}