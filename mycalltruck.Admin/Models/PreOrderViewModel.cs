using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mycalltruck.Admin.Models
{
    public class PreOrderViewModel
    {
        public int PreOrderId { get; set; }
        public int Seq { get; set; }
        public double Distance { get; set; }
        public String CarType { get; set; }
        public String CarSize { get; set; }
        public string CarYear { get; set; }
        public string IsGroup { get; set; }
        public string CarNo { get; set; }
        public bool IsPreview { get; set; }
        public string DriverName { get; set; }
        public string PhoneNo { get; set; }
        public bool IsCustomSelected { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public int DriverId { get; set; }
        public string Park { get; set; }
        public String RouteType { get; set; }
        public DateTime StopTime { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string DriverCode { get; set; }
        public string LastNoticeTime { get; set; }
        public string LastNoticeFlag { get; set; }
        public string LastNoticeGPS { get; set; }
    }
}
