using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace mycalltruck.Admin.DataSets
{
    [Table("Customers")]
    public partial class Referral
    { 
        public int CustomerId { get; set; }
        public string SangHo { get; set; }
        public string PhoneNo { get; set; }
        public string AddressState { get; set; }
        public string AddressCity { get; set; }
        public int ClientId { get; set; }
        public int BizGubun { get; set; }
        public int SalesDay { get; set; }
    }
}
