using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagement.Controllers
{
    public class ErrorController : Controller
    {
        private ILogger<ErrorController> logger;

        /// <summary>
        /// 通过ASP.Net Core 依赖注入服务ILogger 接口，指定类型的控制器作为泛型参数
        /// </summary>
        /// <param name="logger"></param>
        public ErrorController(ILogger<ErrorController> logger)
        {
            this.logger = logger;
        }

        [Route("Error/{StatusCode}")]
        public IActionResult HttpStatusCodeHandler (int StatusCode)
        {
            var statusCodeResult = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();
            switch (StatusCode) {
                case 404:
                    //将异常信息抛出在异常信息视图页面中（不安全） 
                    ViewBag.ErrorMessage = "抱歉，该页面不存在！";
                    logger.LogWarning($"发生了一个404错误，路径 = {statusCodeResult.OriginalPath}, 以及查询字符串 = {statusCodeResult.OriginalQueryString}");
                    //ViewBag.Path = statusCodeResult.OriginalPath;
                    //ViewBag.QueryStr = statusCodeResult.OriginalQueryString;
                    //ViewBag.BasePath = statusCodeResult.OriginalPathBase;
                    break;
            }
            return View("NotFound");
        }

        [AllowAnonymous]
        [Route("Error")]
        public IActionResult Error() 
        {
            var exceptionHandlerPAthFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

            //将异常信息抛出在异常信息视图页面中（不安全） 
            //ViewBag.ExceptioPath = exceptionHandlerPAthFeature.Path;
            //ViewBag.ExceptinMessage = exceptionHandlerPAthFeature.Error.Message;
            //ViewBag.ExceptioStackTrace = exceptionHandlerPAthFeature.Error.StackTrace;

            logger.LogError($"路径 = {exceptionHandlerPAthFeature.Path}， 产生了一个错误 = {exceptionHandlerPAthFeature.Error}");

            return View("Error");
        }


    }
}
