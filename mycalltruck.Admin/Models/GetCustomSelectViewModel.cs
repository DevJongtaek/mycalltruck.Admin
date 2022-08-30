using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mycalltruck.Admin.Models
{
    public class GetCustomSelectViewModel
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int CarType { get; set; }
        public int CarSize { get; set; }
        public string GroupName { get; set; }
        public int DriverFilterType { get; set; }
        public string DriverFilterValue { get; set; }
    }
}
