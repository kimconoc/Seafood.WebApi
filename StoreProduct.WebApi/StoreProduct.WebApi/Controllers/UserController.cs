using StoreProduct.WebApi.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StoreProduct.WebApi.Controllers
{
    [SessionAuthorize]
    public class UserController : BaseApiController
    {
        
    }
}