namespace mycalltruck.Admin.DataSets
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CallSms")]
    public partial class CallSms
    {
        [Key]
        public int CallSmsId { get; set; }

        public DateTime CTime { get; set; }


        [Required]
        [StringLength(16)]
        public string OriginalPhoneNo { get; set; }

        [Required]
        [StringLength(24)]
        public string SmsResult { get; set; }

      
        
        public string ResultMessage { get; set; }

        public int ClientId { get; set; }

        [StringLength(20)]
        public string LoginId { get; set; }

        public int CustomerId { get; set; }


        public int DriverId { get; set; }

        public string CarNo { get; set; }

        public string SangHo { get; set; }

        public string Msg { get; set; }
    }
}
