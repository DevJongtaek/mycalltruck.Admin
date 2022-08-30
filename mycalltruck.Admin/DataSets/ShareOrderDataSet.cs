namespace mycalltruck.Admin.DataSets
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class ShareOrderDataSet : DbContext
    {
        public ShareOrderDataSet()
            : base(Properties.Settings.Default.TruckConnectionString)
        {
        }

        private static ShareOrderDataSet _Instance = null;
        public static ShareOrderDataSet Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new ShareOrderDataSet();
                    _Instance.Orders.Any();
                    _Instance.OrderUpdateLogs.Any();
                    _Instance.ClientPoints.Any();
                }
                return _Instance;
            }
        }

        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderUpdateLog> OrderUpdateLogs { get; set; }

        public virtual DbSet<ClientPoint> ClientPoints { get; set; }
        public virtual DbSet<DriverPoint> DriverPoints { get; set; }
        public virtual DbSet<Trade> Trades { get; set; }
        public virtual DbSet<SalesManage> SalesManage { get; set; }
        public virtual DbSet<Driver> Drivers { get; set; }
        public virtual DbSet<DriverInstance> DriverInstances { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        

    }
}
