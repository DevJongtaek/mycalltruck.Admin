using mycalltruck.Admin.Class.Common;
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
    public partial class Frm_MN0702_CustomerAcc : Form
    {
        public Frm_MN0702_CustomerAcc()
        {
            InitializeComponent();
        }

        private void Frm_MN0702_CustomerAcc_Load(object sender, EventArgs e)
        {
            if(!LocalUser.Instance.LogInInformation.IsAdmin)
            {
                btn_New.Visible = false;
                btnUpdate.Visible = false;
                btnCurrentDelete.Visible = false;
                iAccNo.ReadOnly = true;
                iAccName.Enabled = false;
                iAccAmount.Enabled = false;
                iAccPercent.Enabled = false;
                iAccPrice.Enabled = false;
                iAccDate.Enabled = false;
                iAccLimite.Enabled = false;
                iAccCurrent.Enabled = false;
                iAccState.Enabled = false;
                iAccStart.Enabled = false;
                iAccStop.Enabled = false;
                this.customerAccsTableAdapter.FillByClientId(this.cMDataSet.CustomerAccs, LocalUser.Instance.LogInInformation.ClientId);
                InitSearch();
                InitBinding();
            }
            else
            {
                this.customerAccsTableAdapter.Fill(this.cMDataSet.CustomerAccs);
                InitSearch();
                InitBinding();
            }

        }

        private void InitBinding()
        {
            this.iClientCode.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.customerAccsBindingSource, "ClientCode", true, DataSourceUpdateMode.Never));
            this.iClientBizNo.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.customerAccsBindingSource, "ClientBizNo", true, DataSourceUpdateMode.Never));
            this.iClientName.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.customerAccsBindingSource, "ClientName", true, DataSourceUpdateMode.Never));
            this.iAccName.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.customerAccsBindingSource, "AccName", true, DataSourceUpdateMode.Never));
            this.iCustomerSangHo.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.customerAccsBindingSource, "CustomerSangHo", true, DataSourceUpdateMode.Never));
            this.iAccNo.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.customerAccsBindingSource, "AccNo", true, DataSourceUpdateMode.Never));
            this.iCustomerBizNo.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.customerAccsBindingSource, "CustomerBizNo", true, DataSourceUpdateMode.Never));
            this.iCustomerCEO.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.customerAccsBindingSource, "CustomerCeo", true, DataSourceUpdateMode.Never));
            this.iRegDate.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.customerAccsBindingSource, "RegDate", true, DataSourceUpdateMode.Never));
            this.iAccStop.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.customerAccsBindingSource, "AccStop", true, DataSourceUpdateMode.Never));
            this.iAccStart.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.customerAccsBindingSource, "AccStart", true, DataSourceUpdateMode.Never));
            this.iAccState.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.customerAccsBindingSource, "AccState", true, DataSourceUpdateMode.Never));
            this.iAccDate.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.customerAccsBindingSource, "AccDate", true, DataSourceUpdateMode.Never));
            this.iAccAmount.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.customerAccsBindingSource, "AccAmount", true, DataSourceUpdateMode.Never));
            this.iAccPercent.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.customerAccsBindingSource, "AccPercent", true, DataSourceUpdateMode.Never));
            this.iAccPrice.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.customerAccsBindingSource, "AccPrice", true, DataSourceUpdateMode.Never));
            this.iAccLimite.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.customerAccsBindingSource, "AccLimite", true, DataSourceUpdateMode.Never));
            this.iAccCurrent.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.customerAccsBindingSource, "AccCurrent", true, DataSourceUpdateMode.Never));

        }

        private void InitSearch()
        {
            sAccState.SelectedIndex = 0;
            sCondition.SelectedIndex = 0;
            SAccName.SelectedIndex = 0;
            sText.Clear();
            sDate.Value = DateTime.Now.Date;
        }

        private void btn_New_Click(object sender, EventArgs e)
        {
            FrmMN0702_CustomerAcc_Add AddedForm = new FrmMN0702_CustomerAcc_Add();
            AddedForm.ShowDialog();
            customerAccsTableAdapter.ClearBeforeFill = true;
            this.customerAccsTableAdapter.Fill(this.cMDataSet.CustomerAccs);
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            var Data = ((DataRowView)customerAccsBindingSource.Current).Row as CMDataSet.CustomerAccsRow;
            if(Data != null)
            {
                int CustomerAccId = Data.CustomerAccId;
                if(Update(CustomerAccId))
                {
                    using (SqlConnection Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                    {
                        SqlCommand Command = Connection.CreateCommand();
                        Command.CommandText = "Select CustomerAccId, CustomerBizNo, CustomerSangHo, CustomerCeo, CustomerId, ClientId, ClientCode, ClientBizNo, ClientName, AccNo, AccName, AccAmount, AccPercent, AccPrice, AccDate, AccLimite, AccCurrent, AccState, AccStart, AccStop, RegDate From CustomerAccs Where CustomerAccId = @CustomerAccId";
                        Command.Parameters.AddWithValue("@CustomerAccId", CustomerAccId);
                        Connection.Open();
                        var DataReader = Command.ExecuteReader();
                        cMDataSet.CustomerAccs.Load(DataReader, LoadOption.OverwriteChanges);
                        Connection.Close();
                    }
                    MessageBox.Show("수정 되었습니다.");
                }
            }
        }

        private void btnCurrentDelete_Click(object sender, EventArgs e)
        {
            var Data = ((DataRowView)customerAccsBindingSource.Current).Row as CMDataSet.CustomerAccsRow;
            if (Data != null)
            {
                int CustomerAccId = Data.CustomerAccId;
                using (SqlConnection Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                {
                    SqlCommand Command = Connection.CreateCommand();
                    Command.CommandText = "Delete From CustomerAccs Where CustomerAccId = @CustomerAccId";
                    Command.Parameters.AddWithValue("@CustomerAccId", CustomerAccId);
                    Connection.Open();
                    Command.ExecuteNonQuery();
                    Connection.Close();
                }
                cMDataSet.CustomerAccs.RemoveCustomerAccsRow(Data);
                MessageBox.Show("삭제 되었습니다.");
            }
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void sCondition_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(sCondition.SelectedIndex == 7|| sCondition.SelectedIndex == 8)
            {
                sText.Visible = false;
                sDate.Visible = true;
            }
            else
            {
                sText.Visible = true;
                sDate.Visible = false;
            }
        }

        private void aSearch_Click(object sender, EventArgs e)
        {
            String[] Conditions = { "ClientCode",
                    "ClientBizNo",
                    "ClientName",
                    "CustomerBizNo",
                    "CustomerSangHo",
                    "CustomerCeo",
                    "AccNo",
                    "AccName",
                    "AccDate",
                    "RegDate",
                };

            List<String> Filters = new List<string>();
            if(sAccState.SelectedIndex > 0)
            {
                Filters.Add(String.Format("AccState = '{0}'", sAccState.Text));
            }
            if(sCondition.SelectedIndex == 8 || sCondition.SelectedIndex == 9)
            {
                Filters.Add(String.Format("{0} = '{1}'", Conditions[sCondition.SelectedIndex], sDate.Text));
            }
            else if (!String.IsNullOrEmpty(sText.Text))
            {
                Filters.Add(String.Format("{0} Like '%{1}%'", Conditions[sCondition.SelectedIndex], sText.Text));
            }
            if (SAccName.SelectedIndex > 0)
            {
                Filters.Add(String.Format("AccName = '{0}'", SAccName.Text));
            }

            if(Filters.Count > 0)
            {
                customerAccsBindingSource.Filter = String.Join(" AND ", Filters);
            }
            else
            {
                customerAccsBindingSource.Filter = "";
            }
        }

        private void aClear_Click(object sender, EventArgs e)
        {
            InitSearch();
            customerAccsBindingSource.Filter = "";
        }

        private void iAccAmount_ValueChanged(object sender, EventArgs e)
        {
            if (iAccAmount.Value > 99999999999)
                iAccAmount.Value = 99999999999;
            iAccPrice.Value = decimal.Parse(( double.Parse(iAccAmount.Value.ToString()) * (double.Parse(iAccPercent.Value.ToString()) * 0.01)).ToString());
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

        private bool Update(int CustomerAccId)
        {
            bool HasError = false;
            string ErrorMessage = "";

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
                            "Update CustomerAccs "
                            + "Set AccNo = @AccNo, AccName = @AccName, AccAmount = @AccAmount, AccPercent = @AccPercent, AccPrice = @AccPrice, AccDate = @AccDate, AccLimite = @AccLimite, AccCurrent = @AccCurrent, AccState = @AccState, AccStart = @AccStart, AccStop = @AccStop "
                            + "Where CustomerAccId = @CustomerAccId";

                        //Command.Parameters.AddWithValue("@CustomerBizNo", iCustomerBizNo.Text);
                        //Command.Parameters.AddWithValue("@CustomerSangHo", iCustomerSangHo.Text);
                        //Command.Parameters.AddWithValue("@CustomerCeo", iCustomerCEO.Text);
                        //Command.Parameters.AddWithValue("@CustomerId", CustomerId);
                        //Command.Parameters.AddWithValue("@ClientId", ClientId);
                        //Command.Parameters.AddWithValue("@ClientCode", iClientCode.Text);
                        //Command.Parameters.AddWithValue("@ClientBizNo", iClientBizNo.Text);
                        //Command.Parameters.AddWithValue("@ClientName", iClientName.Text);
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
                        Command.Parameters.AddWithValue("@CustomerAccId", CustomerAccId);
                        //Command.Parameters.AddWithValue("@RegDate", DateTime.Parse(iRegDate.Text));
                        Connection.Open();
                        Command.ExecuteNonQuery();
                        Connection.Close();
                    }
                }
                catch (Exception E)
                {
                    MessageBox.Show(E.Message);
                    HasError = true;
                }

            }
            return !HasError;
        }

        private void newDGV1_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0)
                return;
            if(e.ColumnIndex == 0)
            {
                newDGV1[e.ColumnIndex, e.RowIndex].Value = (cMDataSet.CustomerAccs.Rows.Count - e.RowIndex).ToString("N0");
            }
            if(e.ColumnIndex == 14)
            {
                var Data = ((DataRowView) newDGV1.Rows[e.RowIndex].DataBoundItem).Row as CMDataSet.CustomerAccsRow;
                if(Data != null)
                {
                    newDGV1[e.ColumnIndex, e.RowIndex].Value = String.Format("{0:yyyy-MM-dd} ~ {1:yyyy-MM-dd}", Data.AccStart, Data.AccStop);
                }
            }
        }

        private void iAccName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (iAccName.SelectedIndex == 1)
            {
                lbl_CustomerBizNo.Text = "운송사 사업자번호 :";
                lbl_CustomerSangHo.Text = "운송사 상호 :";
            }
            else
            {
                lbl_CustomerBizNo.Text = "화주 사업자번호 :";
                lbl_CustomerSangHo.Text = "화주 상호 :";
            }
        }
    }
}
