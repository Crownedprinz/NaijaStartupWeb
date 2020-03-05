using System;
using System.Linq;
using NaijaStartupWeb.Models;
using NaijaStartupWeb.Services;
using Microsoft.AspNetCore.Http;
using static NaijaStartupWeb.Models.NsuVariables;
using System.Web.Mvc;

namespace NaijaStartupWeb.Helpers
{
        internal class UnauthorizedCustomFilterAttribute : Attribute, IActionFilter
    {
        private bool skipLogging = false;
        public void OnActionExecuting(ActionExecutingContext context)
        {

            if (context.HttpContext.User == null)
            {
                context.Result = new RedirectResult("~/Index.html");
                return;
            }
            if (context.HttpContext.Session == null)
            {
                context.Result = new RedirectResult("~/Index.html");
                return;
            }
            var gV = context.HttpContext.Session["GlobalVariables"];
            if (gV == null)
            {
                context.Result = new RedirectResult("~/Index.html");
                return;
            }
        }

            public void OnActionExecuted(ActionExecutedContext context)
            {
            if (skipLogging)
            {
                return;
            }
            // do something after the action executes
        }
        }

}
