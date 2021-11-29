using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagement.Controllers
{
    public class DepartmentController : Controller
    {
        public string List() 
        {
            return "返回DepartmentController 控制器List 方法"; 
        }
        public string Details()
        {
            return "返回DepartmentController 控制器Details 方法";
        }

    }
}
