using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mycalltruck.Admin.Models
{
    public class CustomSelectViewModel
    {
        public int CustomSelectId { get; set; }
        public DateTime CreateTime { get; set; }
        public String CarType { get; set; }
        public String CarSize { get; set; }
        public string CarYear { get; set; }
        public String GroupName { get; set; }
        public string CarNo { get; set; }
        public string DriverName { get; set; }
        public string DriverMobileNo { get; set; }
        public string Remark { get; set; }
        public string Park { get; set; }
        public String RouteType { get; set; }
    }
}
