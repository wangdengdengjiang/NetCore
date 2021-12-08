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

        // ����Ӧ�ó�����Ҫ�ķ��������
        // This method gets called by the runtime. Use this method to add services to the container.
        [Obsolete]
        public void ConfigureServices(IServiceCollection services)
        {
            //ʹ��DB���ӷ���ʹ��SqlServer��չ��������ȡSQL server�����ַ���
            //Dbcontext ��DbcontextPool���� DbContextPool ��һ�����ݿ����ӳأ������ǰ���ݿ����ӳ��е����ӿ��ã��Ϳ���ֱ�ӷ��ʳ����е�ʵ�����������´���һ��ʵ��
            services.AddDbContextPool<AppDbContext>(
               optionsAction:options=>options.UseSqlServer(Configuration.GetConnectionString("StudentDbconnecttion"))
                );

            services.AddRazorPages();
            //AddMvcCore ֻ�����˺��ĵ�mvc����
            //AddMvc ������������mvc Core�Լ���ص��������õķ���ͷ���
            services.AddMvc(options => options.EnableEndpointRouting = false); //��mvc������ӵ���Ŀ�е�����ע��������
            services.AddScoped<MessageService>();

            //����ע�룬���ӿں�ʵ�ֹ�����һ��
            services.AddScoped<IStudentInterface, SqlStudentInterface>();
            //services.AddSingleton<IStudentInterface, MokeStudentInterface>();
            //services.AddScoped<IStudentInterface, MokeStudentInterface>();
            //services.AddTransient<IStudentInterface, MokeStudentInterface>();
        }

        //�м��������ͽ���http��Ӧ�Ĺܵ�
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            # region �м����������Ա����
            if (env.IsDevelopment())
            {
                //�Զ��忪����Ա�쳣
                //DeveloperExceptionPageOptions developerExceptionPageOptions = new DeveloperExceptionPageOptions();
                //developerExceptionPageOptions.SourceCodeLineCount = 1;
                //app.UseDeveloperExceptionPage(developerExceptionPageOptions); //�м����������Ա����
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            #endregion

            #region Use�м�� & �ܵ�ִ������
            //app.Run�м�������أ��������κ����󣬶�ֻ��ִ����������
            //һ���ܵ��У�ֻ������һ���ն��м��Run���ն��м�����ùܵ���·���ó���ȥ�����������м����
            //��Ҫ���ö���м������next
            //app.Use(async (context,next) =>
            //{
            //    context.Response.ContentType = "text/plain;charset = utf-8"; //������ı�������
            //    logger.LogInformation("M1: ��������");
            //    //await context.Response.WriteAsync("This is my ��һ�� �м�� ");
            //    await next();
            //    logger.LogInformation("M1: ������Ӧ");
            //});
            #endregion

            #region Ĭ���ļ��м�� UseDefaultFiles
            //��Ĭ���ļ�������Ϊ myDefault.html
            //DefaultFilesOptions defaultFilesOptions = new DefaultFilesOptions();
            //defaultFilesOptions.DefaultFileNames.Clear();
            //defaultFilesOptions.DefaultFileNames.Add("myDefault.html");
            ////���Ĭ���ļ��м��(һ���ھ�̬�ļ��м��֮ǰ)������Ĭ��ҳ��Ϊ�������ṩ��վ����㣩
            ////Ĭ�ϻ�ȥ�� ��index.html , index.htm , default.html , default.htm
            //app.UseDefaultFiles(defaultFilesOptions);
            #endregion

            #region ��̬�ļ��м�� UseStaticFiles
            //��Ӿ�̬�ļ��м��
            app.UseStaticFiles();
            #endregion

            #region Ĭ���ĵ��� UseFileServer �м��
            //FileServerOptions fileServerOptions = new FileServerOptions();
            //fileServerOptions.DefaultFilesOptions.DefaultFileNames.Clear();
            //fileServerOptions.DefaultFilesOptions.DefaultFileNames.Add("myDefault.html");
            ////UseFileServer ����� UseStaticFiles��UseDefaultFiles �� UseDirectoryBrowser����ѡ���Ĺ��ܡ�
            //app.UseFileServer(fileServerOptions);
            #endregion

            #region һЩ�������м��
            //app.UseWelcomePage();  //��ӭ����
            //app.UseDirectoryBrowser(); //Ŀ¼���������ָ��Ŀ¼���г�Ŀ¼
            #endregion

            #region ·��
            ///��ͳ·��
            //���mvc�м����������ܵ��У��Զ����ú���Ĭ��·�ɹ���
            //app.UseMvcWithDefaultRoute();
            //�Զ���·��ģ�壨����
            //app.UseMvc(routes =>
            //{
            //    routes.MapRoute("default", "{controller=Home}/{action=Details}/{id?}");
            //});
            // ����·��
            //��ʹ������һ��mvc����·��������Controller��������
            app.UseMvc();
            #endregion

            #region �ն��м��
            app.Run(async (context) =>
            {
                context.Response.ContentType = "text/plain;charset=utf-8";
                //throw new Exception("���������ڹܵ��з�����һЩ�쳣�����飡");  //ץȡ�쳣
                await context.Response.WriteAsync(" Hosting Envirment��  " + env.EnvironmentName);
            });
            #endregion
        }
    }
}
