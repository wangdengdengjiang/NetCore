using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagement.Modles
{
    /// <summary>
    /// 班级的枚举信息
    /// </summary>
    public enum ClassNameEnum
    {
        [Display(Name = "未分配")]
        None,
        [Display(Name = "一年级")]
        FirstClass,
        [Display(Name = "二年级")]
        SecondClass,
        [Display(Name = "三年级")]
        ThirdClass

    }
}
