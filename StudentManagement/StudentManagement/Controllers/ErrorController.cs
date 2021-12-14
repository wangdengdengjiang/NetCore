using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagement.Controllers
{
    public class ErrorController : Controller
    {
        [Route("Error/{StatusCode}")]
        public IActionResult HttpStatusCodeHandler (int StatusCode)
        {
            var statusCodeResult = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();
            switch (StatusCode) {
                case 404:
                    ViewBag.ErrorMessage = "抱歉，该页面不存在！";
                    ViewBag.Path = statusCodeResult.OriginalPath;
                    ViewBag.QueryStr = statusCodeResult.OriginalQueryString;
                    ViewBag.BasePath = statusCodeResult.OriginalPathBase;
                    break;
            }
            return View("NotFound");
        }

        [AllowAnonymous]
        [Route("Error")]
        public IActionResult Error() 
        {
            var exceptionHandlerPAthFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            ViewBag.ExceptioPath = exceptionHandlerPAthFeature.Path;
            ViewBag.ExceptinMessage = exceptionHandlerPAthFeature.Error.Message;
            ViewBag.ExceptioStackTrace = exceptionHandlerPAthFeature.Error.StackTrace;
            
            return View("Error");
        }


    }
}
