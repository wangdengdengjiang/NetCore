using Microsoft.AspNetCore.Mvc;
using StudentManagement.Modles;
using StudentManagement.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

namespace StudentManagement.Controllers
{
    [Route("[controller]/[action]")]
    public class HomeController : Controller
    {
        private readonly IStudentInterface _studentInterface;
        private readonly IWebHostEnvironment _hostingEnvironment;

        /// <summary>
        /// 构造函数注入的形式注入IStudentInterface
        /// 但是，如果IStudentInterface接口有多个实现类
        /// 计算机并不知道在构造函数中的接口用参数到底是用哪一个类去实现的，因为程序里面根本没有去用到Mock仓储，
        /// 所以要使用依赖注入的方式把接口和实现给绑定起来，计算机才知道使用的是Mock里面的方法
        /// </summary>
        /// <param name="studentInterface"></param>
        public HomeController(IStudentInterface studentInterface, IWebHostEnvironment hostingEnvironment)
        {
            _studentInterface = studentInterface;
            _hostingEnvironment = hostingEnvironment;
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
            Student student = _studentInterface.GetStudent(1);
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
        public IActionResult Details(int id)   //会去找同名的视图
        {
            //throw new Exception("此异常页面出现在Details中");

            id = (id.Equals(0)) ? 1 : id;
            Student student = _studentInterface.GetStudent(id);
            if (student != null)
            {
                //Student model = _studentInterface.GetStudent(1);
                HomeDetailsViewModel homeDetailsViewModel = new HomeDetailsViewModel()
                {
                    //最初id 为int 类型，但是无法实现id ?? 1 ，因为int 不能被赋值为null
                    Student = _studentInterface.GetStudent(id),
                    PageTitle = "学生详情页面"
                };
                return View(homeDetailsViewModel); //View是由 Controller 所提供的视图文件
            }
            else 
            {
                Response.StatusCode = 404;
                return View("StudentNotFound", id);
            }
            
        }

        [HttpGet]  //区分两个Create，一个get请求，一个post请求
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        //public RedirectToActionResult Create(Student student)
        public IActionResult Create(StudentCreateViewModel model)
        {
            //属性验证
            if (ModelState.IsValid)
            {
                string uniqueFileName = UploadPhtotFile(model);
                #region 单文件上传
                //if (model.Photo != null)
                //{
                //  将图片上传到wwwroot目录下的image文件夹中，获取文件夹路径，需要注入ASP.NET Core提供的IWebHostEnvironment服务
                //  通过IWebHostEnvironment 服务获取wwwroot文件夹的路径
                //    string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "images");
                //  确保文件芦名氏唯一的，附加一GUID
                //    uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Photo.FileName;
                //    string filePath = Path.Combine(uploadsFolder,uniqueFileName);
                //    model.Photo.CopyTo(new FileStream(filePath, FileMode.Create));
                //}
                #endregion
                #region 多文件上传
                //if (model.Photos != null && model.Photos.Count > 0) 
                //{
                //    foreach (var Photo in model.Photos)
                //    {
                //        //将图片上传到wwwroot目录下的image文件夹中，获取文件夹路径，需要注入ASP.NET Core提供的IWebHostEnvironment服务
                //        //通过IWebHostEnvironment 服务获取wwwroot文件夹的路径
                //        string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "images");
                //        //确保文件芦名氏唯一的，附加一GUID
                //        uniqueFileName = Guid.NewGuid().ToString() + "_" + Photo.FileName;
                //        string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                //        Photo.CopyTo(new FileStream(filePath, FileMode.Create));
                //    }
                //}
                #endregion
                Student newStudent = new Student
                {
                    Name = model.Name,
                    Email = model.Email,
                    ClassName = model.ClassName,
                    PhotoPath = uniqueFileName
                };
                _studentInterface.Add(newStudent);

                //Student newStudent = _studentInterface.Add(student);
                return RedirectToAction("Details", new { id = newStudent.Id });
            }
            return View();
        }

        [HttpGet]
        public ViewResult Edit(int id)
        {
            Student student = _studentInterface.GetStudent(id);
            if (student != null)
            {
                StudentEditViewModel studentEditView = new StudentEditViewModel
                {
                    Id = student.Id,
                    Name = student.Name,
                    Email = student.Email,
                    ClassName = student.ClassName,
                    ExistingPhotoPath = student.PhotoPath
                };
                return View(studentEditView);
            }
            throw new Exception("查询不到学生信息！");
        }

        [HttpPost]
        public IActionResult Edit(StudentEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                Student student = _studentInterface.GetStudent(model.Id);
                student.Email = model.Email;
                student.Name = model.Name;
                student.ClassName = model.ClassName;
                if (model.Photos.Count > 0)
                {
                    if (model.ExistingPhotoPath != null)
                    {
                        string filePath = Path.Combine(_hostingEnvironment.WebRootPath, "images", model.ExistingPhotoPath);
                        System.IO.File.Delete(filePath);
                    }
                    student.PhotoPath = UploadPhtotFile(model);
                }
                Student newStudent = _studentInterface.Update(student);
                return RedirectToAction("Details", new { id = model.Id});
            }
            else
            { 
                return View(model.Id);
            }
            
        }
        
        #region 多文件上传
        private string UploadPhtotFile(StudentCreateViewModel model) 
        {
            string uniqueFileName = "";
            
            if (model.Photos != null && model.Photos.Count > 0)
            {
                foreach (var photo in model.Photos)
                {
                    //将图片上传到wwwroot目录下的image文件夹中，获取文件夹路径，需要注入ASP.NET Core提供的IWebHostEnvironment服务
                    //通过IWebHostEnvironment 服务获取wwwroot文件夹的路径
                    string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "images");
                    //确保文件名是唯一的，附加一个新的GUID
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + photo.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    //因为使用了非托管资源、所以需要手动进行释放
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        photo.CopyTo(stream);
                    }
                }
            }
            return uniqueFileName;
        }
        #endregion

    }
}
