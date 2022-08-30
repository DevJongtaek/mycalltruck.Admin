namespace mycalltruck.Admin.DataSets
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class Order
    {
        [Key]
        public int OrderId { get; set; }

        public DateTime StartTime { get; set; }

        //[Required]
        public string StartState { get; set; }

        public string StartCity { get; set; }

        public decimal Price { get; set; }

        public decimal? ClientPrice { get; set; } = 0;

        [Required]
        public string Item { get; set; }

        public DateTime CreateTime { get; set; }

        public int? ClientId { get; set; }

        public DateTime StartDate { get; set; }

        public bool IsShared { get; set; }

        public int CarType { get; set; }

        public int CarSize { get; set; }

        //[Required]
        public string StopState { get; set; }

        public string StopCity { get; set; }

        public int PayLocation { get; set; }

        public int CarCount { get; set; }

        public DateTime? AcceptTime { get; set; }

        public string Driver { get; set; }

        public string DriverPhoneNo { get; set; }

        public string DriverCarModel { get; set; }

        public string Remark { get; set; }

        public string StartStreet { get; set; } = "";

        public string StopStreet { get; set; } = "";

        public int NotificationFilterType { get; set; } = 0;

        public int NotificationRadius { get; set; } = 0;

        public string NotificationGroupName { get; set; } = "";

        public int OrderStatus { get; set; } = 1;

        public int? DriverId { get; set; }

        public double X { get; set; } = 0;

        public double Y { get; set; } = 0;

        public string OrderPhoneNo { get; set; }

        public string NotificationState { get; set; } = "";

        public string NotificationCity { get; set; } = "";

        public string NotificationStreet { get; set; } = "";

        public double? NotificationX { get; set; } = 0;

        public double? NotificationY { get; set; } = 0;

        public int? FPIS_ID { get; set; }

        public DateTime StopTime { get; set; }

        public string ItemSize { get; set; } = "0";
        //public double ItemSize { get; set; } = 0;

        public string DriverCarNo { get; set; }

        public DateTime? FPIS_F_DATE { get; set; }

        public DateTime? FPIS_C_DATE { get; set; }

        public int? TradeId { get; set; }

        public string Wgubun { get; set; } = "";

        public int? CustomerId { get; set; }

        public int? SalesManageId { get; set; }

        [Column(TypeName = "text")]
        public string Etc { get; set; }

        public DateTime? RequestDateSC { get; set; }

        public int SourceType { get; set; } = 1;

        public int Agubun { get; set; } = 2;

        public int? SubClientId { get; set; }

        public int? ClientUserId { get; set; }

        public decimal? commission { get; set; }

        public bool? HasPoint { get; set; }

        public int? PointMethod { get; set; }

        [StringLength(20)]
        public string UniqueKey { get; set; }

        public bool? ItemSizeInclude { get; set; }

        public int? SharedItemLength { get; set; }

        public int? SharedItemSize { get; set; }

        public bool? Emergency { get; set; }

        public bool? Round { get; set; }

        public bool? Reservation { get; set; }

        [StringLength(10)]
        public string StartInfo { get; set; }

        [StringLength(10)]
        public string StopInfo { get; set; }

        public int? StartTimeType { get; set; }

        public int? StopTimeType { get; set; }

        public bool? StartTimeHalf { get; set; }

        public bool? StartTimeETC { get; set; }

        public bool? StopTimeHalf { get; set; }

        [StringLength(20)]
        public string StopPhoneNo { get; set; }

        public int? StartTimeHour { get; set; }

        public int? StopTimeHour { get; set; }

        [StringLength(30)]
        public string StartDetail { get; set; }

        [StringLength(30)]
        public string StopDetail { get; set; }

        [StringLength(30)]
        public string Customer { get; set; }

        public int? StopDateHelper { get; set; }

        public bool? CustomerPay { get; set; }

        public decimal? DriverPoint { get; set; }
        public string DriverPayDate { get; set; }
        public decimal? DriverPayPrice { get; set; }
        public decimal? DriverPayVAT { get; set; }
        public bool? UseCardPay { get; set; }
        public string CustomerPayDate { get; set; }
        public decimal? CustomerPayPrice { get; set; }
        public decimal? CustomerPayVAT { get; set; }

        public int? ImageA { get; set; }
        public int? ImageB { get; set; }
        public int? ImageC { get; set; }

        public int? RegisterId { get; set; }
        public string RegisterName { get; set; }

        public string AccountMemo { get; set; }
        public decimal? TradePrice { get; set; }
        public decimal? SalesPrice { get; set; }
        public decimal? StartPrice { get; set; }
        public decimal? StopPrice { get; set; }
        public decimal? AlterPrice { get; set; }
        public decimal? DriverPrice { get; set; }
        public decimal? PartnerPrice { get; set; }

        public decimal? AlterTradePrice { get; set; }
        public decimal? NTradePrice { get; set; }
        public decimal? NAlterTradePrice { get; set; }
        public decimal? NPrice { get; set; }

        public int ReferralId { get; set; }
        [ForeignKey("DriverId")]
        public virtual Driver DriverModel { get; set; }
        [ForeignKey("CustomerId")]
        public virtual Customer CustomerModel { get; set; }
        [ForeignKey("TradeId")]
        public virtual Trade TradeModel { get; set; }
        [ForeignKey("SalesManageId")]
        public virtual SalesManage SalesManageModel { get; set; }

        [ForeignKey("ReferralId")]
        public virtual Customer ReferralModel { get; set; }

        public string RegisterMobileNo { get; set; }

        public int LMSCnt { get; set; }
        public int LMSCustomerCnt { get; set; }

        public string StartName { get; set; }
        public string StopName { get; set; }
        public string StartMemo { get; set; }
        public string StopMemo { get; set; }
        public string RequestMemo { get; set; }
     
        public string StartPhoneNo { get; set; }

        public bool? MyCarOrder { get; set; }
        public int OrderClientId { get; set; }
        public int FOrderId { get; set; }

        public string ReferralAccountYN { get; set; } = "N";

        public bool? StartMulti { get; set; }
        public string OrdersLoginId { get; set; }
        public string OrdersAcceptId { get; set; }

        public string CustomerManager { get; set; }
        public bool? StopMulti { get; set; }

        public string UnitItem { get; set; } = "";
        public int UnitType { get; set; }

        public bool? SugumCheck { get; set; }

        public int? TaxTradeId { get; set; }

        public int? DriverGrade { get; set; }

        public string CargoApiYN { get; set; }

        public string CargoApiStatus { get; set; }


        public string Call24ApiYN { get; set; }

        public string Call24ApiStatus { get; set; }
        public string Call24ordNo { get; set; }





        //R : 도로명 ,J : 지번, S : 간략주소
        public string AddressGubun { get; set; }

        //로그인구분 CL : 운송주선사 , CU : 화주
        public string LoginGubun { get; set; }

        public int? CustomerTeam { get; set; } = 0;

        public string OrderLine { get; set; }
        public string OrderProduct { get; set; }
        public string OrderTurn { get; set; }

        public int? OrderWidth { get; set; } = 0;
        public int? OrderLength { get; set; } = 0;
        public int? OrderHeight { get; set; } = 0;
        public bool IsShore { get; set; } = true;

        public string ReferralName { get; set; }
    }
}
