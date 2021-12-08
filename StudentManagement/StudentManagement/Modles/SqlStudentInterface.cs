using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagement.Modles
{
    public class SqlStudentInterface : IStudentInterface
    {
        private List<Student> _students;
        private readonly AppDbContext context;
        public SqlStudentInterface(AppDbContext context)
        {
            this.context = context;
        }

        public SqlStudentInterface()
        {
            _students = new List<Student>() {
                new Student(){Id = 1, Name = "李一一",ClassName = ClassNameEnum.FirstClass, Email="liyiyi@53.com" },
                new Student(){Id = 2, Name = "王小二",ClassName =ClassNameEnum.SecondClass, Email="wangxiaoer@53.com" },
                new Student(){Id = 3, Name = "张三生",ClassName = ClassNameEnum.ThirdClass, Email="zhangsanshewng@53.com" }
            };
        }

        public IEnumerable<Student> getAllStudent()
        {
            //return _students;
            return context.Students;
        }

        public Student GetStudent(int Id)
        {
            return context.Students.Find(Id);
        }

        public Student Add(Student student)
        {
            context.Students.Add(student);
            context.SaveChanges();
            return student;
        }

        public Student Update(Student student)
        { 
            var mystudent = context.Students.Attach(student);  //给要更改的student打标记，跟踪给定实体
            mystudent.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
            return student;
        }

        public Student Delete(int id)
        {
            Student student = context.Students.Find(id);
            if (student != null)
            {
                context.Students.Remove(student);
            }
            return student;
        }
    }
}
