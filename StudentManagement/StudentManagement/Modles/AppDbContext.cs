using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagement.Modles
{
    public class AppDbContext : DbContext
    {
        //基类构造函数，创建数据上下文, 并额外添加一个给基类传递数据库连接参数的构造函数
        //默认映射规则：DBset属性名字对应数据库表名；实体类属性对应数据库表列; Id/类名+Id作为主键字段
        public AppDbContext(DbContextOptions<AppDbContext> options) :base(options)
        {
        }

        public DbSet<Student> Students { set; get; }


    }
}
