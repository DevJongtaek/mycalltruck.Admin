namespace mycalltruck.Admin.DataSets
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class CallDataSet : DbContext
    {
        public CallDataSet()
            : base("name=CallDataSet")
        {
        }

        public virtual DbSet<Call> Calls { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
