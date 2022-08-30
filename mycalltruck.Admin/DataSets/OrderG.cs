
namespace mycalltruck.Admin.DataSets
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class OrderG
    {
        

        [Key]
        public int OrderGid { get; set; }

        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerRemark { get; set; }
        public int StartCId { get; set; }
        public string StartCname { get; set; }
        public string StartCAddress { get; set; }
        public DateTime StartInDate { get; set; }
        public string StartInTimeGubun { get; set; }
        public int StartInTimeHour { get; set; }
        public bool StartInTimeMinute { get; set; }
        public int StartInGubun { get; set; }


        public DateTime StartDate { get; set; }
        public string StartTimeGubun { get; set; }
        public int StartTimeHour { get; set; }
        public bool StartTimeMinute { get; set; }

        public int StopDayGubun { get; set; }
        public int StopGubun { get; set; }
        public string Remark { get; set; }

        public int DriverId { get; set; }
        public string DriverName { get; set; }
        public string DriverRemark { get; set; }


        public int StopCustomerId { get; set; }
        public string StopCustomerName { get; set; }
        public string StopAddress { get; set; }
        public decimal StopPlt { get; set; }
        public decimal StopBox { get; set; }
        public DateTime StopDateTime { get; set; }
        public bool StopDateYN { get; set; }

        public int Stop1stCustomerId { get; set; }
        public string Stop1stCustomerName { get; set; }
        public string Stop1stAddress { get; set; }
        public decimal Stop1stPlt { get; set; }
        public decimal Stop1stBox { get; set; }
        public DateTime Stop1stDatetime { get; set; }
        public bool Stop1stDateYN { get; set; }

        public int Stop2stCustomerId { get; set; }
        public string Stop2stCustomerName { get; set; }
        public string Stop2stAddress { get; set; }
        public decimal Stop2stPlt { get; set; }
        public decimal Stop2stBox { get; set; }
        public DateTime Stop2stDatetime { get; set; }
        public bool Stop2stDateYN { get; set; }

        public int Stop3stCustomerId { get; set; }
        public string Stop3stCustomerName { get; set; }
        public string Stop3stAddress { get; set; }
        public decimal Stop3stPlt { get; set; }
        public decimal Stop3stBox { get; set; }
        public DateTime Stop3stDatetime { get; set; }
        public bool Stop3stDateYN { get; set; }

        public int Stop4stCustomerId { get; set; }
        public string Stop4stCustomerName { get; set; }
        public string Stop4stAddress { get; set; }
        public decimal Stop4stPlt { get; set; }
        public decimal Stop4stBox { get; set; }
        public DateTime Stop4stDatetime { get; set; }
        public bool Stop4stDateYN { get; set; }

        public string StopRemark { get; set; }

        public DateTime CreateTime { get; set; }
        [ForeignKey("DriverId")]
        public virtual Driver DriverModel { get; set; }

        public int ClientId { get; set; }

        public DateTime StartDateTime { get; set; }
        public bool StartDateYN { get; set; }
        public DateTime Start1stDatetime { get; set; }
        public bool Start1stDateYN { get; set; }
        public DateTime Start2stDatetime { get; set; }
        public bool Start2stDateYN { get; set; }
        public DateTime Start3stDatetime { get; set; }
        public bool Start3stDateYN { get; set; }
        public DateTime Start4stDatetime { get; set; }
        public bool Start4stDateYN { get; set; }

    }
}
