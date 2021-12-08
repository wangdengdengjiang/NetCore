using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using StudentManagement.Modles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagement
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // 配置应用池所需要的服务和内容
        // This method gets called by the runtime. Use this method to add services to the container.
        [Obsolete]
        public void ConfigureServices(IServiceCollection services)
        {
            //使用DB连接服务，使用SqlServer拓展方法，获取SQL server连接字符串
            //Dbcontext 和DbcontextPool区别： DbContextPool 是一个数据库连接池，如果当前数据库连接池中的连接可用，就可以直接访问池子中的实例，而不会新创建一个实例
            services.AddDbContextPool<AppDbContext>(
               optionsAction:options=>options.UseSqlServer(Configuration.GetConnectionString("StudentDbconnecttion"))
                );

            services.AddRazorPages();
            //AddMvcCore 只包含了核心的mvc功能
            //AddMvc 包含了依赖于mvc Core以及相关第三方常用的服务和方法
            services.AddMvc(options => options.EnableEndpointRouting = false); //将mvc服务添加到项目中的依赖注入容器中
            services.AddScoped<MessageService>();

            //依赖注入，将接口和实现关联在一起
            services.AddScoped<IStudentInterface, SqlStudentInterface>();
            //services.AddSingleton<IStudentInterface, MokeStudentInterface>();
            //services.AddScoped<IStudentInterface, MokeStudentInterface>();
            //services.AddTransient<IStudentInterface, MokeStudentInterface>();
        }

        //中间件：处理和接收http响应的管道
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            # region 中间件：开发人员报错
            if (env.IsDevelopment())
            {
                //自定义开发人员异常
                //DeveloperExceptionPageOptions developerExceptionPageOptions = new DeveloperExceptionPageOptions();
                //developerExceptionPageOptions.SourceCodeLineCount = 1;
                //app.UseDeveloperExceptionPage(developerExceptionPageOptions); //中间件：开发人员报错
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            #endregion

            #region Use中间件 & 管道执行流程
            //app.Run中间件做拦截，无论是任何请求，都只会执行里面的语句
            //一个管道中，只允许有一个终端中间件Run（终端中间件会让管道短路，让程序不去调用其他的中间件）
            //若要调用多个中间件，用next
            //app.Use(async (context,next) =>
            //{
            //    context.Response.ContentType = "text/plain;charset = utf-8"; //解决中文编码问题
            //    logger.LogInformation("M1: 传入请求");
            //    //await context.Response.WriteAsync("This is my 第一个 中间件 ");
            //    await next();
            //    logger.LogInformation("M1: 传出响应");
            //});
            #endregion

            #region 默认文件中间件 UseDefaultFiles
            //将默认文件名更改为 myDefault.html
            //DefaultFilesOptions defaultFilesOptions = new DefaultFilesOptions();
            //defaultFilesOptions.DefaultFileNames.Clear();
            //defaultFilesOptions.DefaultFileNames.Add("myDefault.html");
            ////添加默认文件中间件(一定在静态文件中间件之前)（设置默认页面为访问者提供网站的起点）
            ////默认会去找 ：index.html , index.htm , default.html , default.htm
            //app.UseDefaultFiles(defaultFilesOptions);
            #endregion

            #region 静态文件中间件 UseStaticFiles
            //添加静态文件中间件
            app.UseStaticFiles();
            #endregion

            #region 默认文档的 UseFileServer 中间件
            //FileServerOptions fileServerOptions = new FileServerOptions();
            //fileServerOptions.DefaultFilesOptions.DefaultFileNames.Clear();
            //fileServerOptions.DefaultFilesOptions.DefaultFileNames.Add("myDefault.html");
            ////UseFileServer 结合了 UseStaticFiles、UseDefaultFiles 和 UseDirectoryBrowser（可选）的功能。
            //app.UseFileServer(fileServerOptions);
            #endregion

            #region 一些其他的中间件
            //app.UseWelcomePage();  //欢迎界面
            //app.UseDirectoryBrowser(); //目录浏览允许在指定目录中列出目录
            #endregion

            #region 路由
            ///传统路由
            //添加mvc中间件到请求处理管道中，自动配置好了默认路由规则
            //app.UseMvcWithDefaultRoute();
            //自定义路由模板（规则）
            //app.UseMvc(routes =>
            //{
            //    routes.MapRoute("default", "{controller=Home}/{action=Details}/{id?}");
            //});
            // 属性路由
            //且使用启用一个mvc服务，路由配置在Controller控制器中
            app.UseMvc();
            #endregion

            #region 终端中间件
            app.Run(async (context) =>
            {
                context.Response.ContentType = "text/plain;charset=utf-8";
                //throw new Exception("您的请求在管道中发生了一些异常，请检查！");  //抓取异常
                await context.Response.WriteAsync(" Hosting Envirment：  " + env.EnvironmentName);
            });
            #endregion
        }
    }
}
