using mycalltruck.Admin.Class.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mycalltruck.Admin.DataSets
{
    class Schalar
    {
        public ShareOrderDataSet ShareOrderDataSet { get; set; } = new ShareOrderDataSet();

        public decimal GetMisubyDriverId(int DriverId)
        {
            try
            {
                if (LocalUser.Instance.LogInInformation.Client.StaticFromOrder)
                {
                    return ShareOrderDataSet.Orders.Where(c => c.DriverId == DriverId && c.ClientId == LocalUser.Instance.LogInInformation.ClientId && c.OrderStatus == 3 && (c.TradeModel == null || c.TradeModel.PayState != 1)).Sum(c => (c.TradePrice ?? 0));
                }
                else
                {
                    return ShareOrderDataSet.Trades.Where(c => c.DriverId == DriverId && c.ClientId == LocalUser.Instance.LogInInformation.ClientId && c.PayState != 1).Sum(c => c.Amount);
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public decimal GetMisubyCustomerId(int CustomerId)
        {
            try
            {
                if (LocalUser.Instance.LogInInformation.Client.StaticFromOrder)
                {
                    return ShareOrderDataSet.Orders.Where(c => c.CustomerId == CustomerId && c.ClientId == LocalUser.Instance.LogInInformation.ClientId && c.OrderStatus == 3 && (c.SalesManageModel == null || c.SalesManageModel.PayState != 1)).Sum(c => (c.SalesPrice ?? 0));
                }
                else
                {
                    return ShareOrderDataSet.SalesManage.Where(c => c.CustomerId == CustomerId && c.ClientId == LocalUser.Instance.LogInInformation.ClientId && c.PayState != 1).Sum(c => c.Amount);
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }
    }
}
