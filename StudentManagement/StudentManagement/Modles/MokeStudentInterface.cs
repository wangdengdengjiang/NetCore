using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagement.Modles
{
    public class MokeStudentInterface : IStudentInterface
    {
        private List<Student> _students;
        public MokeStudentInterface()
        {
            _students = new List<Student>() {
                new Student(){Id = "1", Name = "李一一",ClassName = ClassNameEnum.FirstClass, Email="liyiyi@53.com" },
                new Student(){Id = "2", Name = "王小二",ClassName =ClassNameEnum.SecondClass, Email="wangxiaoer@53.com" },
                new Student(){Id = "3", Name = "张三生",ClassName = ClassNameEnum.ThirdClass, Email="zhangsanshewng@53.com" }
            };
        }

        public IEnumerable<Student> getAllStudent()
        {
            return _students;
        }

        public Student GetStudent(string Id)
        {
            return _students.FirstOrDefault(a => a.Id == Id);
        }

        public Student Add(Student student)
        {
            student.Id = _students.Max(s => s.Id) + 1;
            _students.Add(student);
            return student;
        }
    }
}
