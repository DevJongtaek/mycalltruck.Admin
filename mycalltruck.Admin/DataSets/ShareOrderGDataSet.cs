namespace mycalltruck.Admin.DataSets
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class ShareOrderGDataSet : DbContext
    {
        public ShareOrderGDataSet()
            : base(Properties.Settings.Default.TruckConnectionString)
        {
        }

        private static ShareOrderGDataSet _Instance = null;
        public static ShareOrderGDataSet Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new ShareOrderGDataSet();
                    _Instance.OrderGs.Any();
                    //_Instance.OrderUpdateLogs.Any();
                    //_Instance.ClientPoints.Any();
                }
                return _Instance;
            }
        }

        public virtual DbSet<OrderG> OrderGs { get; set; }
        //public virtual DbSet<OrderUpdateLog> OrderUpdateLogs { get; set; }

        //public virtual DbSet<ClientPoint> ClientPoints { get; set; }
        //public virtual DbSet<Trade> Trades { get; set; }
        //public virtual DbSet<SalesManage> SalesManage { get; set; }
        public virtual DbSet<Driver> Drivers { get; set; }
        public virtual DbSet<DriverInstance> DriverInstances { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }

    }
}
