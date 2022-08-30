namespace mycalltruck.Admin.DataSets
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class ShareOrderBDataSet : DbContext
    {
        public ShareOrderBDataSet()
            : base(Properties.Settings.Default.TruckConnectionString)
        {
        }

        private static ShareOrderBDataSet _Instance = null;
        public static ShareOrderBDataSet Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new ShareOrderBDataSet();
                    _Instance.OrderBs.Any();
                    
                }
                return _Instance;
            }
        }

        public virtual DbSet<OrderB> OrderBs { get; set; }
    
     
        public virtual DbSet<DriverInstance> DriverInstances { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }

    }
}
