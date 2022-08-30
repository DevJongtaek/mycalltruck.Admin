using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using mycalltruck.Admin.Class.Common;
using mycalltruck.Admin.UI;

namespace mycalltruck.Admin
{
    public partial class FrmMNFPISADD : Form
    {

        string Gubun = string.Empty;
        string sDate = string.Empty;
        string eDate = string.Empty;
        string Quater = string.Empty;
        string MakeFpis = string.Empty;
        string NEWFPISCount = "0";
        string FPISCount = "0";
        public FrmMNFPISADD()
        {
            InitializeComponent();
        }

        private void FrmMNFPISADD_Load(object sender, EventArgs e)
        {
            YearAdd();

            Dictionary<string, string> MonthList = new Dictionary<string, string>();
            MonthList.Add("1", "1분기(1~3월)");
            MonthList.Add("2", "2분기(4~6월)");
            MonthList.Add("3", "3분기(7~9월)");
            MonthList.Add("4", "4분기(10~12월)");

            cmb_Month.DataSource = new BindingSource(MonthList, null);
            cmb_Month.DisplayMember = "Value";
            cmb_Month.ValueMember = "Key";


            cmb_Month.SelectedIndex = 0;
        }
        private void YearAdd()
        {
            cmbYear.Items.Clear();
            int iThatYear = int.Parse(DateTime.Now.Year.ToString());

            for (int i = iThatYear - 3; i <= iThatYear + 1; i++)
            {
                cmbYear.Items.Add(i);
            }
            cmbYear.SelectedIndex = cmbYear.FindString(iThatYear.ToString());

        }
        private void FPISCreate()
        {


            Quater = cmb_Month.SelectedValue.ToString();
            MakeFpis = cmbYear.Text;
            if (cmb_Month.SelectedIndex == 0)
            {
                sDate = cmbYear.Text + "/" + "01/01";
                eDate = cmbYear.Text + "/" + "03/31";

            }
            else if (cmb_Month.SelectedIndex == 1)
            {
                sDate = cmbYear.Text + "/" + "04/01";
                eDate = cmbYear.Text + "/" + "06/30";
                // Quater = "2";
            }
            else if (cmb_Month.SelectedIndex == 2)
            {

                sDate = cmbYear.Text + "/" + "07/01";
                eDate = cmbYear.Text + "/" + "09/30";
                //Quater = "3";
            }
            else if (cmb_Month.SelectedIndex == 3)
            {
                sDate = cmbYear.Text + "/" + "10/01";
                eDate = cmbYear.Text + "/" + "12/31";
                //Quater = "4";

            }
            using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                {
                    cn.Open();

                    var Command1 = cn.CreateCommand();
                    Command1.CommandText = "SELECT count(*) FROM NEWFPIS where ClientId = @ClientId AND Quater = @Quater  AND MakeFpis = @MakeFpis ";

                    Command1.Parameters.AddWithValue("@Quater", Quater);
                    Command1.Parameters.AddWithValue("@MakeFpis", MakeFpis);
                    Command1.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                    var o1 = Command1.ExecuteScalar();
                    if (o1 != null)
                    {
                        NEWFPISCount = o1.ToString();
                    }


                    cn.Close();

                }
            }


            using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                {
                    cn.Open();

                    var Command1 = cn.CreateCommand();
                    Command1.CommandText = @"SELECT Count(*) FROM(select Customers.BizNo,CASE Customers.BizGubun WHEN 1 THEN 2 ELSE 1 END AS BizGubun  ,substring(replace(FPIS_CONT.CONT_FROM,'/',''),1,6) as CONT_FROM ,sum(convert(bigint,FPIS_CONT.CONT_DEPOSIT)) as CONT_DEPOSIT
                                            ,'' as ETC1,'' as ETC2 ,Drivers.CarNo  ,substring(convert(varchar, Orders.CreateTime, 112), 1, 6) as StopTime
                                            , replace(convert(varchar,(sum(Orders.Price))),'.00','') as Price
                                            ,Sum(case Orders.CarCount when 0 then 1 else Orders.CarCount end) as CarCount ,'' as TRU_COMP_BSNS_NUM ,'' as TRU_CONT_FROM ,'' as TRU_DEPOSIT ,'' as TRU_MANG_TYPE
                                           from Customers JOIN FPIS_CONT ON Customers.CustomerId = FPIS_CONT.CustomerId
                                            JOIN Orders ON Customers.CustomerId = Orders.CustomerId
                                            Join Drivers ON Orders.DriverId = Drivers.DriverId
                                            Where Customers.ClientId = @ClientId AND Orders.OrderStatus = 3
                                          
                                            AND FPIS_CONT.CONT_FROM >= @Sdate AND FPIS_CONT.CONT_FROM <= @Edate
                                            
                                            AND substring(convert(varchar,Orders.CreateTime,111),1,7)  >= @Sdate
                                            AND substring(convert(varchar,Orders.CreateTime,111),1,7)  <= @Edate
                                            GROUP BY Customers.BizNo,BizGubun,substring(replace(FPIS_CONT.CONT_FROM, '/', ''), 1, 6),Drivers.CarNo ,substring(convert(varchar, Orders.CreateTime, 112), 1, 6)
                                            Union all
                                            Select Customers.BizNo ,CASE Customers.BizGubun WHEN 1 THEN 2 ELSE 1 END AS BizGubun ,substring(replace(FPIS_CONT.CONT_FROM, '/', ''), 1, 6) as CONT_FROM ,sum(convert(bigint, FPIS_CONT.CONT_DEPOSIT)) as CONT_DEPOSIT
                                            ,'' as ETC1,'' as ETC2 ,'' as CarNo  ,'' as StopTime ,NULL as Price ,NULL as CarCount ,FPIS_TRU.TRU_COMP_BSNS_NUM
                                            ,substring(replace(FPIS_CONT.CONT_FROM, '/', ''), 1, 6) as CONT_FROM ,sum(convert(bigint, FPIS_TRU.TRU_DEPOSIT)) ,max(FPIS_TRU.TRU_MANG_TYPE)
                                            from Customers JOIN FPIS_CONT ON Customers.CustomerId = FPIS_CONT.CustomerId
                                            JOIN Orders ON Customers.CustomerId = Orders.CustomerId
                                            Join FPIS_TRU ON FPIS_TRU.FPIS_ID = FPIS_CONT.FPIS_ID
                                            Where Customers.ClientId = @ClientId AND Orders.OrderStatus = 3
                                            AND FPIS_CONT.CONT_FROM >= @Sdate AND FPIS_CONT.CONT_FROM <= @Edate
                                            GROUP BY Customers.BizNo,BizGubun,substring(replace(FPIS_CONT.CONT_FROM, '/', ''), 1, 6),FPIS_TRU.TRU_COMP_BSNS_NUM,substring(replace(FPIS_CONT.CONT_FROM, '/', ''), 1, 6)) as a";

                    Command1.Parameters.AddWithValue("@Sdate", sDate);
                    Command1.Parameters.AddWithValue("@Edate", eDate);
                    Command1.Parameters.AddWithValue("@Quater", Quater);
                    Command1.Parameters.AddWithValue("@MakeFpis", MakeFpis);
                    Command1.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                    var o1 = Command1.ExecuteScalar();
                    if (o1 != null)
                    {
                        FPISCount = o1.ToString();
                    }


                    cn.Close();

                }
            }
            if(FPISCount == "0")
            {
                MessageBox.Show("생성할 자료가 없습니다.", "FPIS 파일");
                return;
            }

            if (NEWFPISCount == "0")
            {


                using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                {
                    _Connection.Open();
                    using (SqlCommand _Command = _Connection.CreateCommand())
                    {
                        _Command.CommandText = @"INSERT INTO NEWFPIS
                                            (BizNo, BizGubun,  Cont_From, Cont_Deposit, ETC1, ETC2, CarNo, StopTime, Price, CarCount, TRU_COMP_MSNS_NUM, TRU_CONT_FROM, TRU_DEPOSIT, TRU_MANG_TYPE,MakeFpis,Quater,ClientId)
                                            select Replace(Customers.BizNo,'-',''),CASE Customers.BizGubun WHEN 1 THEN 2 ELSE 1 END AS BizGubun ,substring(replace(FPIS_CONT.CONT_FROM,'/',''),1,6) as CONT_FROM ,sum(convert(bigint,FPIS_CONT.CONT_DEPOSIT)) as CONT_DEPOSIT
                                            ,'' as ETC1,'' as  ETC2 ,Drivers.CarNo  ,substring(convert(varchar,Orders.CreateTime,112),1,6) as StopTime  , replace(convert(varchar,(sum(Orders.Price))),'.00','') as Price
                                            ,Sum(case Orders.CarCount when 0 then 1 else Orders.CarCount end ) as CarCount ,'' as TRU_COMP_BSNS_NUM ,'' as TRU_CONT_FROM ,'' as TRU_DEPOSIT ,'' as TRU_MANG_TYPE, @MakeFpis,@Quater,@ClientId 
                                            from Customers  JOIN FPIS_CONT ON Customers.CustomerId = FPIS_CONT.CustomerId
                                            JOIN Orders On Customers.CustomerId = Orders.CustomerId
                                            Join Drivers ON Orders.DriverId = Drivers.DriverId
                                            Where  Customers.ClientId  = @ClientId AND Orders.OrderStatus = 3
                                         
                                            AND FPIS_CONT.CONT_FROM >= @Sdate AND FPIS_CONT.CONT_FROM <= @Edate

                                            AND substring(convert(varchar,Orders.CreateTime,111),1,7)  >= @Sdate
                                            AND substring(convert(varchar,Orders.CreateTime,111),1,7)  <= @Edate
                                            GROUP BY Customers.BizNo,BizGubun,substring(replace(FPIS_CONT.CONT_FROM,'/',''),1,6),Drivers.CarNo ,substring(convert(varchar,Orders.CreateTime,112),1,6)
                                            Union all
                                            Select Replace(Customers.BizNo,'-','') ,CASE Customers.BizGubun WHEN 1 THEN 2 ELSE 1 END AS BizGubun ,substring(replace(FPIS_CONT.CONT_FROM,'/',''),1,6) as CONT_FROM ,sum(convert(bigint,FPIS_CONT.CONT_DEPOSIT)) as CONT_DEPOSIT
                                            ,'' as ETC1,'' as  ETC2 ,'' as CarNo  ,'' as StopTime ,NULL as Price ,NULL as CarCount ,Replace(FPIS_TRU.TRU_COMP_BSNS_NUM,'-','')
                                            ,substring(replace(FPIS_CONT.CONT_FROM,'/',''),1,6) as CONT_FROM ,convert(varchar,sum(convert(bigint,FPIS_TRU.TRU_DEPOSIT))) ,max(FPIS_TRU.TRU_MANG_TYPE)
                                            , @MakeFpis,@Quater,@ClientId
                                            from Customers JOIN FPIS_CONT ON Customers.CustomerId = FPIS_CONT.CustomerId
                                            JOIN Orders ON Customers.CustomerId = Orders.CustomerId
                                            Join FPIS_TRU ON FPIS_TRU.FPIS_ID = FPIS_CONT.FPIS_ID 
                                            Where  Customers.ClientId  =@ClientId AND Orders.OrderStatus = 3
                                            AND FPIS_CONT.CONT_FROM >= @Sdate AND  FPIS_CONT.CONT_FROM <= @Edate
                                            GROUP BY Customers.BizNo,BizGubun,substring(replace(FPIS_CONT.CONT_FROM,'/',''),1,6),FPIS_TRU.TRU_COMP_BSNS_NUM,substring(replace(FPIS_CONT.CONT_FROM,'/',''),1,6)";

                        _Command.Parameters.AddWithValue("@Sdate", sDate);
                        _Command.Parameters.AddWithValue("@Edate", eDate);
                        _Command.Parameters.AddWithValue("@Quater", Quater);
                        _Command.Parameters.AddWithValue("@MakeFpis", MakeFpis);
                        _Command.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                        _Command.ExecuteNonQuery();
                    }
                    _Connection.Close();
                }

            }
            else
            {
                if (MessageBox.Show(" 이미 생성된 자료가 있습니다. 생성할경우 기존파일은 삭제됩니다. 진행하시겠습니까?", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                    return;

                using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                {
                    cn.Open();
                    SqlCommand cmd = cn.CreateCommand();
                    cmd.CommandText = "delete NEWFPIS where ClientId = @ClientId AND Quater = @Quater  AND MakeFpis = @MakeFpis ";

                    cmd.Parameters.AddWithValue("@Quater", Quater);
                    cmd.Parameters.AddWithValue("@MakeFpis", MakeFpis);
                    cmd.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                    cmd.ExecuteNonQuery();
                    cn.Close();
                }


                using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                {
                    _Connection.Open();
                    using (SqlCommand _Command = _Connection.CreateCommand())
                    {
                        _Command.CommandText = @"INSERT INTO NEWFPIS
                                            (BizNo, BizGubun,  Cont_From, Cont_Deposit, ETC1, ETC2, CarNo, StopTime, Price, CarCount, TRU_COMP_MSNS_NUM, TRU_CONT_FROM, TRU_DEPOSIT, TRU_MANG_TYPE,MakeFpis,Quater,ClientId)
                                            select Replace(Customers.BizNo,'-',''),CASE Customers.BizGubun WHEN 1 THEN 2 ELSE 1 END AS BizGubun ,substring(replace(FPIS_CONT.CONT_FROM,'/',''),1,6) as CONT_FROM ,sum(convert(bigint,FPIS_CONT.CONT_DEPOSIT)) as CONT_DEPOSIT
                                            ,'' as ETC1,'' as  ETC2 ,Drivers.CarNo  ,substring(convert(varchar,Orders.CreateTime,112),1,6) as StopTime , replace(convert(varchar,(sum(Orders.Price))),'.00','') as Price
                                            ,Sum(case Orders.CarCount when 0 then 1 else Orders.CarCount end ) as CarCount ,'' as TRU_COMP_BSNS_NUM ,'' as TRU_CONT_FROM ,'' as TRU_DEPOSIT ,'' as TRU_MANG_TYPE, @MakeFpis,@Quater,@ClientId 
                                            from Customers  JOIN FPIS_CONT ON Customers.CustomerId = FPIS_CONT.CustomerId
                                            JOIN Orders ON Customers.CustomerId = Orders.CustomerId
                                            Join Drivers ON Orders.DriverId = Drivers.DriverId
                                            Where  Customers.ClientId  = @ClientId AND Orders.OrderStatus = 3
                                              -- AND FPIS_CONT.CONT_FROM >= @Sdate AND FPIS_CONT.CONT_TO <= @Sdate
                                            AND FPIS_CONT.CONT_FROM >= @Sdate AND FPIS_CONT.CONT_FROM <= @Edate
                                            AND substring(convert(varchar,Orders.CreateTime,111),1,7)  >= @Sdate
                                            AND substring(convert(varchar,Orders.CreateTime,111),1,7)  <= @Edate
                                            GROUP BY Customers.BizNo,BizGubun,substring(replace(FPIS_CONT.CONT_FROM,'/',''),1,6),Drivers.CarNo ,substring(convert(varchar,Orders.CreateTime,112),1,6)
                                            Union all
                                            Select Replace(Customers.BizNo,'-','') ,CASE Customers.BizGubun WHEN 1 THEN 2 ELSE 1 END AS BizGubun ,substring(replace(FPIS_CONT.CONT_FROM,'/',''),1,6) as CONT_FROM ,sum(convert(bigint,FPIS_CONT.CONT_DEPOSIT)) as CONT_DEPOSIT
                                            ,'' as ETC1,'' as  ETC2 ,'' as CarNo  ,'' as StopTime ,NULL as Price ,NULL as CarCount ,Replace(FPIS_TRU.TRU_COMP_BSNS_NUM,'-','')
                                            ,substring(replace(FPIS_CONT.CONT_FROM,'/',''),1,6) as CONT_FROM ,convert(varchar,sum(convert(bigint,FPIS_TRU.TRU_DEPOSIT))) ,max(FPIS_TRU.TRU_MANG_TYPE), @MakeFpis,@Quater,@ClientId
                                            from Customers JOIN FPIS_CONT ON Customers.CustomerId = FPIS_CONT.CustomerId
                                            JOIN Orders ON Customers.CustomerId = Orders.CustomerId
                                            Join FPIS_TRU ON FPIS_TRU.FPIS_ID = FPIS_CONT.FPIS_ID 
                                            Where  Customers.ClientId  =@ClientId AND Orders.OrderStatus = 3
                                            AND FPIS_CONT.CONT_FROM >= @Sdate AND  FPIS_CONT.CONT_FROM <= @Edate
                                            GROUP BY Customers.BizNo,BizGubun,substring(replace(FPIS_CONT.CONT_FROM,'/',''),1,6),FPIS_TRU.TRU_COMP_BSNS_NUM,substring(replace(FPIS_CONT.CONT_FROM,'/',''),1,6)";

                        _Command.Parameters.AddWithValue("@Sdate", sDate);
                        _Command.Parameters.AddWithValue("@Edate", eDate);
                        _Command.Parameters.AddWithValue("@Quater", Quater);
                        _Command.Parameters.AddWithValue("@MakeFpis", MakeFpis);
                        _Command.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                        _Command.ExecuteNonQuery();
                    }
                    _Connection.Close();
                }


            }

            try
            {
              
                MessageBox.Show("파일생성이 완료되었습니다.", "FPIS 파일");
                    
            }

            catch
            {


            }
        }

        private void btn_File_Click(object sender, EventArgs e)
        {
            FPISCreate();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
