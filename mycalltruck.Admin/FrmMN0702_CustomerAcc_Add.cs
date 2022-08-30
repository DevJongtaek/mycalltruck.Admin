using mycalltruck.Admin.CMDataSetTableAdapters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace mycalltruck.Admin
{
    public partial class FrmMN0702_CustomerAcc_Add : Form
    {
        //Added
        int ClientId = 0;
        int CustomerId = 0;
        string CustomerCode = "";
        string ClientBizNo = "";
        string CustomerBizNo = "";

        public FrmMN0702_CustomerAcc_Add()
        {
            InitializeComponent();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (Add())
            {
                ClearAdded();
            }
        }

        private void btnAddClose_Click(object sender, EventArgs e)
        {
            if (Add())
            {
                Close();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void FrmMN0702_CustomerAcc_Add_Load(object sender, EventArgs e)
        {
            ClearAdded();
        }

        private void aSearchClient_Click(object sender, EventArgs e)
        {
            using (SqlConnection Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                SqlCommand Command = Connection.CreateCommand();
                Command.CommandText = "SELECT ClientId, Code, Name, BizNo from Clients Where BizNo = @BizNo";
                Command.Parameters.AddWithValue("@BizNo", iClientBizNo.Text);
                Connection.Open();
                IDataReader Reader = Command.ExecuteReader();
                if (Reader.Read())
                {
                    ClientId = Reader.GetInt32(0);
                    iClientCode.Text = Reader.GetString(1);
                    iClientName.Text = Reader.GetString(2);
                    ClientBizNo = Reader.GetString(3);
                }
                else
                {
                    ClientId = 0;
                    ClientBizNo = "";
                    iClientBizNo.Clear();
                    iClientCode.Clear();
                    iClientName.Clear();
                    MessageBox.Show("알 수 없는 사업자번호입니다.");
                }
                Connection.Close();
            }
        }

        private void aSearchCustomer_Click(object sender, EventArgs e)
        {
            using (SqlConnection Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                SqlCommand Command = Connection.CreateCommand();
                Command.CommandText = "SELECT CustomerId, Code, Sangho, CEO, BizNo from Customers Where BizNo = @BizNo And SalesGubun <> 2";
                Command.Parameters.AddWithValue("@BizNo", iCustomerBizNo.Text);
                Connection.Open();
                IDataReader Reader = Command.ExecuteReader();
                if (Reader.Read())
                {
                    CustomerId = Reader.GetInt32(0);
                    CustomerCode = Reader.GetString(1);
                    iCustomerSangHo.Text = Reader.GetString(2);
                    iCustomerCEO.Text = Reader.GetString(3);
                    CustomerBizNo = Reader.GetString(4);
                }
                else
                {
                    CustomerId = 0;
                    CustomerCode = "";
                    CustomerBizNo = "";
                    iCustomerSangHo.Clear();
                    iCustomerCEO.Clear();
                    iCustomerBizNo.Clear();
                    MessageBox.Show("알 수 없는 사업자번호입니다.");
                }
                Connection.Close();
            }
        }

        private void ClearAdded()
        {
            ClientId = 0;
            ClientBizNo = "";
            iClientBizNo.Clear();
            iClientCode.Clear();
            iClientName.Clear();

            CustomerId = 0;
            CustomerCode = "";
            CustomerBizNo = "";
            iCustomerSangHo.Clear();
            iCustomerCEO.Clear();
            iCustomerBizNo.Clear();

            iAccName.SelectedIndex = 0;
            iAccState.SelectedIndex = 0;
            iAccDate.Value = DateTime.Now.Date;
            iAccStart.Value = DateTime.Now.Date;
            iAccStop.Value = DateTime.Now.Date;
            iRegDate.Text = DateTime.Now.Date.ToString("yyyy-MM-dd");

            iAccNo.Clear();
            iAccAmount.Value = 0;
            iAccPercent.Value = 0;
            iAccPrice.Value = 0;
            iAccLimite.Value = 0;
            iAccCurrent.Value = 0;
        }

        private void iAccAmount_ValueChanged(object sender, EventArgs e)
        {
            if (iAccAmount.Value > 99999999999)
                iAccAmount.Value = 99999999999;
         //   iAccPrice.Value = iAccAmount.Value * iAccPercent.Value;
            iAccPrice.Value = decimal.Parse((double.Parse(iAccAmount.Value.ToString()) * (double.Parse(iAccPercent.Value.ToString()) * 0.01)).ToString());
        }

        private void iAccPercent_ValueChanged(object sender, EventArgs e)
        {
            if (iAccPercent.Value > 999.999m)
                iAccPercent.Value = 999.999m;
            iAccPrice.Value = decimal.Parse((double.Parse(iAccAmount.Value.ToString()) * (double.Parse(iAccPercent.Value.ToString()) * 0.01)).ToString());
        }

        private void iAccPrice_ValueChanged(object sender, EventArgs e)
        {
            if (iAccPrice.Value > 99999999999)
                iAccPrice.Value = 99999999999;
        }

        private void iAccLimite_ValueChanged(object sender, EventArgs e)
        {
            if (iAccLimite.Value > 99999999999)
                iAccLimite.Value = 99999999999;

        }

        private void iAccCurrent_ValueChanged(object sender, EventArgs e)
        {
            if (iAccCurrent.Value > 99999999999)
                iAccCurrent.Value = 99999999999;

        }

        private bool Add()
        {
            bool HasError = false;
            string ErrorMessage = "";

            if (ClientId == 0)
            {
                HasError = true;
                ErrorMessage += "계약자사업번호를 검색하여 주십시오." + Environment.NewLine;
            }
            if (CustomerId == 0)
            {
                HasError = true;
                ErrorMessage += "화주사업번호를 검색하여 주십시오." + Environment.NewLine;
            }
            if (String.IsNullOrEmpty(iAccName.Text))
            {
                HasError = true;
                ErrorMessage += "보험증권번호를 입력하여 주십시오." + Environment.NewLine;
            }

            if (HasError)
            {
                MessageBox.Show(ErrorMessage.Trim());
            }
            else
            {
                try
                {
                    using (SqlConnection Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                    {
                        SqlCommand Command = Connection.CreateCommand();
                        Command.CommandText =
                            "Insert Into CustomerAccs (CustomerBizNo, CustomerSangHo, CustomerCeo, CustomerId, ClientId, ClientCode, ClientBizNo, ClientName, AccNo, AccName, AccAmount, AccPercent, AccPrice, AccDate, AccLimite, AccCurrent, AccState, AccStart, AccStop, RegDate)"
                            + "Values (@CustomerBizNo, @CustomerSangHo, @CustomerCeo, @CustomerId, @ClientId, @ClientCode, @ClientBizNo, @ClientName, @AccNo, @AccName, @AccAmount, @AccPercent, @AccPrice, @AccDate, @AccLimite, @AccCurrent, @AccState, @AccStart, @AccStop, @RegDate)";
                        Command.Parameters.AddWithValue("@CustomerBizNo", CustomerBizNo);
                        Command.Parameters.AddWithValue("@CustomerSangHo", iCustomerSangHo.Text);
                        Command.Parameters.AddWithValue("@CustomerCeo", iCustomerCEO.Text);
                        Command.Parameters.AddWithValue("@CustomerId", CustomerId);
                        Command.Parameters.AddWithValue("@ClientId", ClientId);
                        Command.Parameters.AddWithValue("@ClientCode", iClientCode.Text);
                        Command.Parameters.AddWithValue("@ClientBizNo", ClientBizNo);
                        Command.Parameters.AddWithValue("@ClientName", iClientName.Text);
                        Command.Parameters.AddWithValue("@AccNo", iAccNo.Text);
                        Command.Parameters.AddWithValue("@AccName", iAccName.Text);
                        Command.Parameters.AddWithValue("@AccAmount", iAccAmount.Value);
                        Command.Parameters.AddWithValue("@AccPercent", (float)iAccPercent.Value);
                        Command.Parameters.AddWithValue("@AccPrice", iAccPrice.Value);
                        Command.Parameters.AddWithValue("@AccDate", iAccDate.Value);
                        Command.Parameters.AddWithValue("@AccLimite", iAccLimite.Value);
                        Command.Parameters.AddWithValue("@AccCurrent", iAccCurrent.Value);
                        Command.Parameters.AddWithValue("@AccState", iAccState.Text);
                        Command.Parameters.AddWithValue("@AccStart", iAccStart.Value);
                        Command.Parameters.AddWithValue("@AccStop", iAccStop.Value);
                        Command.Parameters.AddWithValue("@RegDate", DateTime.Parse(iRegDate.Text));
                        Connection.Open();
                        Command.ExecuteNonQuery();
                        Connection.Close();
                    }
                    MessageBox.Show("화주보험정보가 추가되었습니다.", "화주보험관리 추가 성공");
                }
                catch(Exception E)
                {
                    MessageBox.Show(E.Message);
                    HasError = true;
                }

            }
            return !HasError;
        }

        private void iClientBizNo_TextChanged(object sender, EventArgs e)
        {
            ClientId = 0;
            ClientBizNo = "";
            iClientCode.Clear();
            iClientName.Clear();
        }

        private void iCustomerBizNo_TextChanged(object sender, EventArgs e)
        {
            if (iAccName.SelectedIndex != 1)
            {
                CustomerId = 0;
                CustomerCode = "";
                CustomerBizNo = "";


                iCustomerSangHo.Clear();
                iCustomerCEO.Clear();
            }
        }

        private void iCustomer_Copy()
        {

            using (SqlConnection Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                SqlCommand Command = Connection.CreateCommand();
                Command.CommandText = "SELECT ClientId, Code, Name, BizNo,CEO from Clients Where BizNo = @BizNo";
                Command.Parameters.AddWithValue("@BizNo", iClientBizNo.Text);
                Connection.Open();
                IDataReader Reader = Command.ExecuteReader();
                if (Reader.Read())
                {
                    CustomerId = Reader.GetInt32(0);
                    CustomerCode = Reader.GetString(1);
                    iCustomerSangHo.Text = Reader.GetString(2);
                    iCustomerCEO.Text = Reader.GetString(4);
                    iCustomerBizNo.Text = Reader.GetString(3);
                    CustomerBizNo = Reader.GetString(3);
                }
                else
                {
                    CustomerId = 0;
                    CustomerCode = "";
                    CustomerBizNo = "";
                    iCustomerSangHo.Clear();
                    iCustomerCEO.Clear();
                    iCustomerBizNo.Clear();
                    MessageBox.Show("알 수 없는 사업자번호입니다.");
                }
                Connection.Close();
            }
        }
        private void iAccName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (iAccName.SelectedIndex == 1)
            {
                lbl_CustomerBizNo.Text = "운송사 사업자번호 :";
                lbl_CustomerSangHo.Text = "운송사 상호 :";

                //iCustomerBizNo.Text = iClientBizNo.Text;
                //iCustomerSangHo.Text = iClientName.Text;
                //iCustomerCEO.Text = 
                iCustomer_Copy();
            }
            else
            {
                lbl_CustomerBizNo.Text = "화주 사업자번호 :";
                lbl_CustomerSangHo.Text = "화주 상호 :";
            }
        }
    }
}
