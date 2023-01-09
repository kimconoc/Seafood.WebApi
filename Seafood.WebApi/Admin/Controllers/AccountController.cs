using Admin.Models;
using System.Web.Mvc;


namespace Amin.Controllers
{
    public class AccountController : BaseController
    {
        #region Login
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model, string ReturnUrl = "")
        {
            //Authenticator.SetAuth(userBase.Result.Data, HttpContext);
            return RedirectToAction("Index", "Admin");
        }
        #endregion Login 
    }
}