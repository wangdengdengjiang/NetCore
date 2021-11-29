using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagement.Modles
{
    /// <summary>
    /// 学生模型
    /// 类文件，存储数据
    /// </summary>
    public class Student
    {
        public string Id { set; get; }
        [Display(Name = "姓名")]
        [Required(ErrorMessage = "请输入名字")]
        public string Name { set; get; }
        public ClassNameEnum  ClassName { set; get; }
        public string Email { set; get; }
    }
}
