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
        public int Id { set; get; }

        [Display(Name = "姓名")]  //显式属性，lable中英文变中文
        [Required(ErrorMessage = "请输入名字"),MaxLength(50,ErrorMessage ="名字长度不能超过五十个字符")]
        public string Name { set; get; }
        [Required]
        [Display(Name = "班级")]
        public ClassNameEnum  ClassName { set; get; }
        [Required]
        [Display(Name = "邮箱")]
        [RegularExpression(@"^[a-zA-Z0-9_-]+@[a-zA-Z0-9_-]+(\.[a-zA-Z0-9_-]+)+$", ErrorMessage ="请输入正确格式的邮箱")]
        public string Email { set; get; }
    }
}
