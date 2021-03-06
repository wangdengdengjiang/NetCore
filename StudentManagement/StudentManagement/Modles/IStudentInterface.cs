using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagement.Modles
{
    /// <summary>
    /// 仓储接口，调用实现方法
    /// </summary>
    public interface IStudentInterface
    {
        Student GetStudent(int Id);

        IEnumerable<Student> getAllStudent();

        Student Add(Student student);

        Student Update(Student student);

        Student Delete(int id); 

    }

}
