using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mycalltruck.Admin.DataSets
{
    using mycalltruck.Admin.Class;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.SqlClient;

    [Table("Drivers")]
    public partial class Driver
    {
        public int DriverId { get; set; }
        public string Name { get; set; }
        public string CarYear { get; set; }
        public string MobileNo { get; set; }
        public string CarNo { get; set; }
        public int CarType { get; set; }
        public int CarSize { get; set; }
        public string BizNo { get; set; }
        public int? ServiceState { get; set; }
        public int? BrandId { get; set; }
        public string CEO { get; set; }
        public string Upjong { get; set; }
        public string Uptae { get; set; }
        public string AddressState { get; set; }
        public string AddressCity { get; set; }
        public string AddressDetail { get; set; }
        public string PayBankName { get; set; }
        public string PayAccountNo { get; set; }
        public string PayInputName { get; set; }
     

        //private decimal? _DriverPoint = 0;

        //public virtual decimal DriverPoint
        //{
        //    get
        //    {
        //        if (_DriverPoint == null)
        //        {
        //            Data.Connection(_Connection =>
        //            {
        //                using (SqlCommand _Command = _Connection.CreateCommand())
        //                {
        //                    _Command.CommandText = "SELECT dbo._GetDriverPoint(@DriverId)";
        //                    _Command.Parameters.AddWithValue("@DriverId", DriverId);
        //                    _DriverPoint = (decimal)_Command.ExecuteScalar();
        //                }
        //            });
        //        }

        //        return _DriverPoint ?? 0;
        //    }
        //    set { _DriverPoint = value; }
        //}
    }
}
