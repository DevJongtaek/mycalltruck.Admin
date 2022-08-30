using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mycalltruck.Admin.DataSets
{
    public class OrderUpdateLog
    {
        public int OrderUpdateLogId { get; set; }
        public DateTime UpdateTime { get; set; }
        public string LoginId { get; set; }
        public string CarSize { get; set; }
        public string CarType { get; set; }
        public string PayLocation { get; set; }
        public string Amount { get; set; }
        public string Remark { get; set; }
        public int ClientId { get; set; }
        public int OrderId { get; set; }
        public string Customer { get; set; }
    }
}
