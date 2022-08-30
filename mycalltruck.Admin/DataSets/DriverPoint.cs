using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mycalltruck.Admin.DataSets
{
    public class DriverPoint
    {
        public int DriverPointId { get; set; }
        public DateTime CDate { get; set; }
        public decimal Amount { get; set; }
        public int OrderId { get; set; }
        public int DriverId { get; set; }
        public int? ClientId { get; set; }
        public string Remark { get; set; }
        public string PointItem { get; set; }
        //public int UseCount { get; set; }
    }
}
