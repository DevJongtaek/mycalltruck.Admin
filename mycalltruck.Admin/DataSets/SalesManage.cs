using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace mycalltruck.Admin.DataSets
{
    [Table("SalesManage")]
    public partial class SalesManage
    {
        [Key]
        public int SalesId { get; set; }
        public DateTime RequestDate { get; set; }
        public decimal Amount { get; set; }
        public int PayState { get; set; }
        public DateTime? PayDate { get; set; }

        public int? ClientId { get; set; }
        public int? CustomerId { get; set; }

        [ForeignKey("CustomerId")]
        public virtual Customer CustomerModel { get; set; }

    }
}
