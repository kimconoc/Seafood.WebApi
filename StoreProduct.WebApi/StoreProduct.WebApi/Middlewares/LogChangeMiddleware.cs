using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace StoreProduct.WebApi.Middlewares
{
    public class LogChangeMiddleware : OwinMiddleware
    {
        public LogChangeMiddleware(OwinMiddleware next)
            : base(next)
        {
        }

        public async override Task Invoke(IOwinContext context)
        {
            var cr_at = DateTime.Now;
            var request_path = context.Request.Path.Value;
            var request_method = context.Request.Method;
            if (!request_path.Contains("api"))
            {
                await Next.Invoke(context);
                return;
            }

            var log_created_by = context.Request.User.Identity.Name;
            var log_ip = context.Request.RemoteIpAddress;
            var log_uri = context.Request.Uri.ToString();
            var log_action = request_method;
            var log_request = string.Empty;
            if (context.Request.Body.CanRead)
            {
                using (StreamReader reader = new StreamReader(context.Request.Body))
                {
                    log_request = reader.ReadToEndAsync().Result;
                }
            }
            await Next.Invoke(context);
        }
    }
}