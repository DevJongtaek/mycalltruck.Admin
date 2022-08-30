using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mycalltruck.Admin.Models
{
    public class GetPreOrderViewModel
    {
        /// <summary>
        /// 인근공차 = 1
        /// 행정구역 = 2
        /// 그룹공차 = 3
        /// 그룹+인근 = 4
        /// 개별차량 = 5
        /// </summary>
        public int FilterType { get; set; }
        /// <summary>
        /// 차량번호 = 1
        /// 기사명 = 2
        /// 핸드폰번호 = 3
        /// 아이디 = 4
        /// </summary>
        public int DriverFilterType { get; set; }
        public string DriverFilterValue { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public int Radius { get; set; }
        public int CarType { get; set; }
        public int CarSize { get; set; }
        public int PreviewType { get; set; }
        public string GroupName { get; set; }
        public string ClientState { get; set; }
        public string ClientCity { get; set; }
        public string ClientStreet { get; set; }
        public string ClientName { get; set; }


        public int RowCount { get; set; }
        public int ClientId { get; set; }

        public bool IsLogin { get; set; }

    }
}
