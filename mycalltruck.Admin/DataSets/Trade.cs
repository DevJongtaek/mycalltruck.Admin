using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mycalltruck.Admin.DataSets
{

    [Table("Trades")]
    public partial class Trade
    {
        public int TradeId { get; set; }
        public DateTime RequestDate { get; set; }
        public int PayState { get; set; }
        public DateTime? PayDate { get; set; }
        public decimal Amount { get; set; }
        public int? ClientId { get; set; }
        public int DriverId { get; set; }

        [ForeignKey("DriverId")]
        public virtual Driver DriverModel { get; set; }
    }
}
