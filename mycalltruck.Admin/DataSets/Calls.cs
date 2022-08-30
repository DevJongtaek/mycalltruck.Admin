namespace mycalltruck.Admin.DataSets
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Calls")]
    public partial class Call
    {
        [Key]
        public int CallId { get; set; }

        [Required]
        [StringLength(16)]
        public string OriginalPhoneNo { get; set; }

        [Required]
        [StringLength(24)]
        public string Target { get; set; }

        [Required]
        [StringLength(6)]
        public string Div { get; set; }

        public int CustomerId { get; set; }

        public int DriverId { get; set; }

        public DateTime CTime { get; set; }

        public int ClientId { get; set; }

        [Required]
        [StringLength(16)]
        public string ClientPhoneNo { get; set; }

        [StringLength(420)]
        public string Memo { get; set; }

        [StringLength(20)]
        public string LoginId { get; set; }

        [StringLength(20)]
        public string Gugun { get; set; }
        
    }
}
