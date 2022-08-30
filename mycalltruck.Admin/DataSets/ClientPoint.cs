using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mycalltruck.Admin.DataSets
{
    public class ClientPoint
    {
        public int ClientPointId { get; set; }
        public DateTime CDate { get; set; }
        public decimal Amount { get; set; }
        public int ClientId { get; set; }
        public string MethodType { get; set; }
        public string Remark { get; set; }
        public int? ahst_id { get; set; }
    }
}
