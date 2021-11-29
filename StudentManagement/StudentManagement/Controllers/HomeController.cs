using Microsoft.AspNetCore.Mvc;
using StudentManagement.Modles;
using StudentManagement.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagement.Controllers
{
    [Route("[controller]/[action]")]
    public class HomeController : Controller
    {
        private readonly IStudentInterface _studentInterface;
        /// <summary>
        /// 构造函数注入的形式注入IStudentInterface
        /// 但是，如果IStudentInterface接口有多个实现类
        /// 计算机并不知道在构造函数中的接口用参数到底是用哪一个类去实现的，因为程序里面根本没有去用到Mock仓储，
        /// 所以要使用依赖注入的方式把接口和实现给绑定起来，计算机才知道使用的是Mock里面的方法
        /// </summary>
        /// <param name="studentInterface"></param>
        public HomeController(IStudentInterface studentInterface)
        {
            _studentInterface = studentInterface;
        }


        //[Route("Home")]
        //[Route("Home/Index")]
        //[Route("Index")]
        [Route("")]
        //[Route("[action]")]
        [Route("~/")]  //找绝对路径
        [Route("~/Home")]
        public IActionResult Index()
        {
             IEnumerable<Student> students= _studentInterface.getAllStudent();
            //控制器名称和视图
            //return View("~/Views/Home/Index.cshtml", students);
            return View(students);
        }

        
        /// <summary>
        /// 将数据从控制器传递到视图 -- 弱类型视图
        /// </summary>
        /// <returns></returns>
        //[Route("Weak")]
        //[Route("[action]")]
        public ActionResult Weak() {
            Student student = _studentInterface.GetStudent("1");
            //将数据从控制器传递到视图
            //1、ViewData
            //ViewData["PageTitle"] = "Student Details";
            //ViewData["Student"] = student;

            //2、ViewBag
            ViewBag.PageTitle = "Student Details";
            ViewBag.Student = student;

            return View("Details"); //View是由 Controller 所提供的视图文件
        }

        /// <summary>
        /// 将数据从控制器传递到视图 -- 强类型视图
        /// </summary>
        /// <returns></returns>

        //[Route("Home/Details/{id?}")]
        //[Route("Details/{id?}")]
        //[Route("[action]/{id?}")]
        [Route("{id?}")]
        public IActionResult Details(string id)   //会去找同名的视图
        {
            //id = (id.Equals(0)) ? 1 : id;
            //Student model = _studentInterface.GetStudent(1);
            HomeDetailsViewModel homeDetailsViewModel = new HomeDetailsViewModel()
            {
                //最初id 为int 类型，但是无法实现id ?? 1 ，因为int 不能被赋值为null
                Student = _studentInterface.GetStudent(id ?? "1"),
                PageTitle = "学生详情页面"
            };
            return View(homeDetailsViewModel); //View是由 Controller 所提供的视图文件
        }

        [HttpGet]  //区分两个Create，一个get请求，一个post请求
        public IActionResult Create()
        {

            return View();
        }

        [HttpPost]
        //public RedirectToActionResult Create(Student student)
        public IActionResult Create(Student student)
        {
            if (ModelState.IsValid)
            { 
                 Student newStudent = _studentInterface.Add(student);
                return View("Details", new { id = newStudent.Id });
            }
            return View();
           
        }

    }
}
