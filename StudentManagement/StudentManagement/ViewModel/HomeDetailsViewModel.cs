using StudentManagement.Modles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagement.ViewModel
{
    /// <summary>
    /// 服务于View 视图页面
    /// </summary>
    public class HomeDetailsViewModel
    {
        public Student Student { set; get; }

        public string PageTitle { set; get; }
    }
}
