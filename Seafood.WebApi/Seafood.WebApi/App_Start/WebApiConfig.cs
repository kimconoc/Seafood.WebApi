﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Seafood.WebApi.App_Start
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            //if (ConfigurationManager.AppSettings["HiddenError"].Equals("true"))
            //{
            //    config.Filters.Add(new CustomExceptionFilter());
            //    config.MessageHandlers.Add(new CustomModifyingErrorMessageDelegatingHandler());
            //}

            // Configure JSON 
            //config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));
        }
    }
}