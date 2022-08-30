using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace mycalltruck.Admin.DataSets
{
    [Table("DriverInstances")]
    public class DriverInstance
    {
        public int DriverInstanceId { get; set; }
        public int DriverId { get; set; }
        public int ClientId { get; set; }
        [ForeignKey("DriverId")]
        public virtual Driver DriverModel { get; set; }
    }
}
