
namespace mycalltruck.Admin.DataSets
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class OrderB
    {
        

        [Key]
        public int OrderBid { get; set; }

        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerRemark { get; set; }
      
        public DateTime CreateDate { get; set; }
        public DateTime BanDate { get; set; }
      
        public int ItemId { get; set; }
        public string ItemName { get; set; }

        public decimal BanPLTNum { get; set; }
        public decimal BanBOXNum { get; set; }

        public string Remark { get; set; }



        public int ClientId { get; set; }

        public int DriverId { get; set; }

        public string DriverCarNo { get; set; }
        public string DriverName { get; set; }

    }
}
