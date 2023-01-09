using Amin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Amin.CustomAuthen
{
    public class SessionAuthen: AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            bool isAuthroized = base.AuthorizeCore(httpContext);
            if (!isAuthroized)
                return false;

            return IsTokenValid(httpContext);
        }

        private bool IsTokenValid(HttpContextBase httpContext)
        {
            return false;
        }
    }
}