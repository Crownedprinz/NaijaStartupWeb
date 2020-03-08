using System;
using System.Linq;
using NaijaStartupWeb.Models;
using NaijaStartupWeb.Services;
using Microsoft.AspNetCore.Http;
using static NaijaStartupWeb.Models.NsuVariables;
using System.Web.Mvc;

namespace NaijaStartupWeb.Helpers
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class UnauthorizedCustomFilterAttribute : ActionFilterAttribute, IActionFilter
    {
        private bool skipLogging = false;
        void IActionFilter.OnActionExecuting(ActionExecutingContext context)
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


    }


}
