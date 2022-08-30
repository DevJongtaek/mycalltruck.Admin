using mycalltruck.Admin.Class.Common;
using mycalltruck.Admin.Class.DataSet;
using mycalltruck.Admin.UI;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace mycalltruck.Admin
{
    public partial class FrmMN0701_DRIVERPAY : Form
    {
        string Up_Status = string.Empty;
        public FrmMN0701_DRIVERPAY()
        {
            InitializeComponent();
            YearAdd();
            cmbSearch.SelectedIndex = 0;
            cmb_Card.SelectedIndex = 0;
        }

        private void FrmMN0701_DRIVERPAY_Load(object sender, EventArgs e)
        {
            // TODO: 이 코드는 데이터를 'cMDataSet.DRIVERPAY' 테이블에 로드합니다. 필요한 경우 이 코드를 이동하거나 제거할 수 있습니다.
            this.dRIVERPAYTableAdapter.Fill(this.cMDataSet.DRIVERPAY);
            this.clientsTableAdapter.Fill(this.cMDataSet.Clients);
            DriverRepository mDriverRepository = new DriverRepository();
            mDriverRepository.Select(baseDataSet.Drivers);
            btn_Search_Click(null, null);
        }

        private void YearAdd()
        {
            cmbYear.Items.Clear();
            int iThatYear = int.Parse(DateTime.Now.Year.ToString());
            int iThatMonth = int.Parse(DateTime.Now.Month.ToString());
            for (int i = iThatYear-3; i <= iThatYear + 1; i++)
            {
                cmbYear.Items.Add(i);
            }
            cmbYear.SelectedIndex = cmbYear.FindString(iThatYear.ToString());
            cmbMonth.SelectedIndex = cmbMonth.FindString(iThatMonth.ToString().PadLeft(2, '0'));
        }

        private void _Search()
        {
            this.dRIVERPAYTableAdapter.Fill(this.cMDataSet.DRIVERPAY);
            string _FilterString = string.Empty;
            
            string _cmbSearchString = string.Empty;
            string _DateSearchString = string.Empty;
            string _CardSearchString = string.Empty;

            DateTime S_Date = DateTime.Parse(cmbYear.Text + "-" + cmbMonth.Text + "-" + 01).AddMonths(-2);
            DateTime e_Date = DateTime.Parse(cmbYear.Text + "-" + cmbMonth.Text + "-" + 01);

            //_DateSearchString = "RequestDate >='" + S_Date.ToString().Replace("-", "").Substring(0, 6) + "' AND RequestDate <='" + e_Date.ToString().Replace("-", "").Substring(0, 6) + "' ";

            _DateSearchString = "RequestDate ='" + e_Date.ToString().Replace("-", "").Substring(0, 6) + "'  ";

            _FilterString += _DateSearchString;

            if (cmbSearch.Text == "운송사코드")
            {
                var codes = cMDataSet.Clients.Where(c => c.Code.Contains(txtSearch.Text)).Select(c => c.ClientId).ToArray();
                if (codes.Count() > 0)
                {
                    string filter = String.Format("Clientid IN ('{0}'", codes[0]);
                    for (int i = 1; i < codes.Count(); i++)
                    {
                        filter += string.Format(", '{0}'", codes[i]);
                    }
                    filter += ")";
                    _cmbSearchString = filter;
                }
                else
                    _cmbSearchString = "";


            }
            else if (cmbSearch.Text == "운송사명")
            {
                var codes = cMDataSet.Clients.Where(c => c.Name.Contains(txtSearch.Text)).Select(c => c.ClientId).ToArray();
                if (codes.Count() > 0)
                {
                    string filter = String.Format("Clientid IN ('{0}'", codes[0]);
                    for (int i = 1; i < codes.Count(); i++)
                    {
                        filter += string.Format(", '{0}'", codes[i]);
                    }
                    filter += ")";
                    _cmbSearchString = filter;
                }
                else
                    _cmbSearchString = "";
            }
            else if (cmbSearch.Text == "차주코드")
            {
                var codes = baseDataSet.Drivers.Where(c => c.Code.Contains(txtSearch.Text)).Select(c => c.DriverId).ToArray();
                if (codes.Count() > 0)
                {
                    string filter = String.Format("DriverId IN ('{0}'", codes[0]);
                    for (int i = 1; i < codes.Count(); i++)
                    {
                        filter += string.Format(", '{0}'", codes[i]);
                    }
                    filter += ")";
                    _cmbSearchString = filter;
                }
                else
                    _cmbSearchString = "";
            }

            else if (cmbSearch.Text == "기사명")
            {
                var codes = baseDataSet.Drivers.Where(c => c.CarYear.Contains(txtSearch.Text)).Select(c => c.DriverId).ToArray();
                if (codes.Count() > 0)
                {
                    string filter = String.Format("DriverId IN ('{0}'", codes[0]);
                    for (int i = 1; i < codes.Count(); i++)
                    {
                        filter += string.Format(", '{0}'", codes[i]);
                    }
                    filter += ")";
                    _cmbSearchString = filter;
                }
                else
                    _cmbSearchString = "";
            }
            else if (cmbSearch.Text == "차량번호")
            {
                var codes = baseDataSet.Drivers.Where(c => c.CarNo.Contains(txtSearch.Text)).Select(c => c.DriverId).ToArray();
                if (codes.Count() > 0)
                {
                    string filter = String.Format("DriverId IN ('{0}'", codes[0]);
                    for (int i = 1; i < codes.Count(); i++)
                    {
                        filter += string.Format(", '{0}'", codes[i]);
                    }
                    filter += ")";
                    _cmbSearchString = filter;
                }
                else
                    _cmbSearchString = "";
            }
            else if (cmbSearch.Text == "아이디")
            {
                var codes = baseDataSet.Drivers.Where(c => c.LoginId.Contains(txtSearch.Text)).Select(c => c.DriverId).ToArray();
                if (codes.Count() > 0)
                {
                    string filter = String.Format("DriverId IN ('{0}'", codes[0]);
                    for (int i = 1; i < codes.Count(); i++)
                    {
                        filter += string.Format(", '{0}'", codes[i]);
                    }
                    filter += ")";
                    _cmbSearchString = filter;
                }
                else
                    _cmbSearchString = "";
            }

            else if (cmbSearch.Text == "핸드폰번호")
            {
                var codes = baseDataSet.Drivers.Where(c => c.MobileNo.Contains(txtSearch.Text) || c.MobileNo.Replace("-", "").Contains(txtSearch.Text)).Select(c => c.DriverId).ToArray();
                if (codes.Count() > 0)
                {
                    string filter = String.Format("DriverId IN ('{0}'", codes[0]);
                    for (int i = 1; i < codes.Count(); i++)
                    {
                        filter += string.Format(", '{0}'", codes[i]);
                    }
                    filter += ")";
                    _cmbSearchString = filter;
                }
                else
                    _cmbSearchString = "";
            }
            if (_cmbSearchString != string.Empty)
            {
                _FilterString += " AND " + _cmbSearchString;
            }
            else
            {
                _FilterString +=  _cmbSearchString;
            }

            if (cmb_Card.SelectedIndex == 1)
            {
                var codes = baseDataSet.Drivers.Where(c => c.InsuranceShcard != "" || c.InsuranceKbCard != "" || c.InsuranceWrCard != "").Select(c => c.DriverId).ToArray();
                if (codes.Count() > 0)
                {
                    string filter = String.Format("DriverId IN ('{0}'", codes[0]);
                    for (int i = 1; i < codes.Count(); i++)
                    {
                        filter += string.Format(", '{0}'", codes[i]);
                    }
                    filter += ")";
                    _CardSearchString = filter;
                }
                else
                    _CardSearchString = "";
            }
            else if (cmb_Card.SelectedIndex == 2)
            {
                var codes = baseDataSet.Drivers.Where(c => c.InsuranceShcard == "" && c.InsuranceKbCard == "" && c.InsuranceWrCard == "").Select(c => c.DriverId).ToArray();
                if (codes.Count() > 0)
                {
                    string filter = String.Format("DriverId IN ('{0}'", codes[0]);
                    for (int i = 1; i < codes.Count(); i++)
                    {
                        filter += string.Format(", '{0}'", codes[i]);
                    }
                    filter += ")";
                    _CardSearchString = filter;
                }
                else
                    _CardSearchString = "";
            }
            if (_CardSearchString != string.Empty && _FilterString != string.Empty)
            {
                _FilterString += " AND " + _CardSearchString;
            }
            else
            {
                _FilterString += _CardSearchString;
            }
            try
            {

                dRIVERPAYBindingSource.Filter = _FilterString;

            }
            catch
            {
                btn_Search_Click(null, null);
            }

        }
         private Dictionary<string, string> DicSearch = new Dictionary<string, string>();
       
        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == 1 && e.RowIndex >= 0)
            {
                dataGridView1[e.ColumnIndex, e.RowIndex].Value = (dataGridView1.Rows.Count - e.RowIndex).ToString("N0");
            }
            if (e.ColumnIndex == 2 && e.RowIndex >= 0)
            {
                e.Value = e.Value.ToString().Substring(0, 4) + "/" + e.Value.ToString().Substring(4, 2);
            }
            else if (e.ColumnIndex == 3 && e.RowIndex >= 0)
            {
                var Selected = ((DataRowView)dataGridView1.Rows[e.RowIndex].DataBoundItem).Row as CMDataSet.DRIVERPAYRow;

                var Query = cMDataSet.Drivers.Where(c => c.DriverId == Selected.DriverId).ToArray();

                if (Query.Any())
                {
                    var ClientsQuery = cMDataSet.Clients.Where(c => c.ClientId == Query.First().CandidateId).ToArray();
                    if (ClientsQuery.Any())
                    {
                        dataGridView1[e.ColumnIndex, e.RowIndex].Value = ClientsQuery.First().Code;
                    }
                }

            }
            else if (e.ColumnIndex == 4 && e.RowIndex >= 0)
            {
                var Selected = ((DataRowView)dataGridView1.Rows[e.RowIndex].DataBoundItem).Row as CMDataSet.DRIVERPAYRow;

                var Query = cMDataSet.Drivers.Where(c => c.DriverId == Selected.DriverId).ToArray();

                if (Query.Any())
                {
                    var ClientsQuery = cMDataSet.Clients.Where(c => c.ClientId == Query.First().CandidateId).ToArray();

                    if (ClientsQuery.Any())
                    {
                        dataGridView1[e.ColumnIndex, e.RowIndex].Value = ClientsQuery.First().Name;
                    }
                }

            }

            else if (e.ColumnIndex == 5 && e.RowIndex >= 0)
            {
                var Selected = ((DataRowView)dataGridView1.Rows[e.RowIndex].DataBoundItem).Row as CMDataSet.DRIVERPAYRow;

                var Query = cMDataSet.Drivers.Where(c => c.DriverId == Selected.DriverId).ToArray();

                if (Query.Any())
                {
                    e.Value = Query.First().Code;
                }

            }


            else if (e.ColumnIndex == 6 && e.RowIndex >= 0)
            {
                var Selected = ((DataRowView)dataGridView1.Rows[e.RowIndex].DataBoundItem).Row as CMDataSet.DRIVERPAYRow;

                var Query = cMDataSet.Drivers.Where(c => c.DriverId == Selected.DriverId).ToArray();

                if (Query.Any())
                {

                    dataGridView1[e.ColumnIndex, e.RowIndex].Value = Query.First().CarYear;
                }
                

            }
            else if (e.ColumnIndex == 7 && e.RowIndex >= 0)
            {
                var Selected = ((DataRowView)dataGridView1.Rows[e.RowIndex].DataBoundItem).Row as CMDataSet.DRIVERPAYRow;

                var Query = cMDataSet.Drivers.Where(c => c.DriverId == Selected.DriverId).ToArray();

                if (Query.Any())
                {

                    dataGridView1[e.ColumnIndex, e.RowIndex].Value = Query.First().CarNo;
                }


            }

            else if (e.ColumnIndex == 8 && e.RowIndex >= 0)
            {
                var Selected = ((DataRowView)dataGridView1.Rows[e.RowIndex].DataBoundItem).Row as CMDataSet.DRIVERPAYRow;

                var Query = cMDataSet.Drivers.Where(c => c.DriverId == Selected.DriverId).ToArray();

                if (Query.Any())
                {

                    dataGridView1[e.ColumnIndex, e.RowIndex].Value = Query.First().LoginId;
                }


            }

            else if (e.ColumnIndex == 9 && e.RowIndex >= 0)
            {
                var Selected = ((DataRowView)dataGridView1.Rows[e.RowIndex].DataBoundItem).Row as CMDataSet.DRIVERPAYRow;

                var Query = cMDataSet.Drivers.Where(c => c.DriverId == Selected.DriverId).ToArray();

                if (Query.Any())
                {

                    dataGridView1[e.ColumnIndex, e.RowIndex].Value = Query.First().MobileNo;
                }


            }


            else if (e.ColumnIndex == 10 && e.RowIndex >= 0)
            {
                var Selected = ((DataRowView)dataGridView1.Rows[e.RowIndex].DataBoundItem).Row as CMDataSet.DRIVERPAYRow;

                var Query = cMDataSet.Drivers.Where(c => c.DriverId == Selected.DriverId).ToArray();

                if (Query.Any())
                {
                    if (!String.IsNullOrEmpty(Query.First().InsuranceShcard.ToString()))
                    {

                        if (Query.First().InsuranceShcard.ToString().Length == 16)
                        {
                            dataGridView1[e.ColumnIndex, e.RowIndex].Value = Query.First().InsuranceShcard.ToString().Substring(0, 4) + "-" + Query.First().InsuranceShcard.ToString().Substring(4, 4) + "-" + Query.First().InsuranceShcard.ToString().Substring(8, 4) + "-"+Query.First().InsuranceShcard.ToString().Substring(12, 4);
                        }
                    }
                }


            }

            else if (e.ColumnIndex == 11 && e.RowIndex >= 0)
            {
                var Selected = ((DataRowView)dataGridView1.Rows[e.RowIndex].DataBoundItem).Row as CMDataSet.DRIVERPAYRow;

                var Query = cMDataSet.Drivers.Where(c => c.DriverId == Selected.DriverId).ToArray();

                if (Query.Any())
                {

                  


                    if (!String.IsNullOrEmpty(Query.First().InsuranceKbCard))
                    {

                        if (Query.First().InsuranceKbCard.ToString().Length == 16)
                        {
                            dataGridView1[e.ColumnIndex, e.RowIndex].Value = Query.First().InsuranceKbCard.ToString().Substring(0, 4) + "-" + Query.First().InsuranceKbCard.ToString().Substring(4, 4) + "-" + Query.First().InsuranceKbCard.ToString().Substring(8, 4) +"-"+ Query.First().InsuranceKbCard.ToString().Substring(12, 4);
                        }


                       // dataGridView1[e.ColumnIndex, e.RowIndex].Value = Query.First().InsuranceKbCard;
                    }
                }


            }
            else if (e.ColumnIndex == 12 && e.RowIndex >= 0)
            {
                var Selected = ((DataRowView)dataGridView1.Rows[e.RowIndex].DataBoundItem).Row as CMDataSet.DRIVERPAYRow;

                var Query = cMDataSet.Drivers.Where(c => c.DriverId == Selected.DriverId).ToArray();

                if (Query.Any())
                {
                    if (!String.IsNullOrEmpty(Query.First().InsuranceWrCard))
                    {
                       // dataGridView1[e.ColumnIndex, e.RowIndex].Value = Query.First().InsuranceWrCard;

                        if (Query.First().InsuranceKbCard.ToString().Length == 16)
                        {
                            dataGridView1[e.ColumnIndex, e.RowIndex].Value = Query.First().InsuranceWrCard.ToString().Substring(0, 4) + "-" + Query.First().InsuranceWrCard.ToString().Substring(4, 4) + "-" + Query.First().InsuranceWrCard.ToString().Substring(8, 4) +"-"+ Query.First().InsuranceWrCard.ToString().Substring(12, 4);
                        }

                    }
                }


            }
            else if (e.ColumnIndex == 13 && e.RowIndex >= 0)
            {
                if (!String.IsNullOrEmpty(e.Value.ToString()))
                {
                    e.Value = String.Format("{0:#,##0}", Convert.ToInt64(e.Value));
                }


            }
            else if (e.ColumnIndex == 14 && e.RowIndex >= 0)
            {
                if (!String.IsNullOrEmpty(e.Value.ToString()))
                {
                    e.Value = String.Format("{0:#,##0}", Convert.ToInt64(e.Value));
                }


            }
            else if (e.ColumnIndex == 15 && e.RowIndex >= 0)
            {
                if (!String.IsNullOrEmpty(e.Value.ToString()))
                {
                    e.Value = String.Format("{0:#,##0}", Convert.ToInt64(e.Value));
                }


            }
            else if (e.ColumnIndex == 17 && e.RowIndex >= 0)
            {
               // var Selected = ((DataRowView)dataGridView1.Rows[e.RowIndex].DataBoundItem).Row as CMDataSet.DRIVERPAYRow;
                if (!String.IsNullOrEmpty(e.Value.ToString()))
                {
                    e.Value = e.Value.ToString().Substring(0, 4);
                }
            }
        }

        private void btn_Request_Click(object sender, EventArgs e)
        {int icount = 0;
            if (MessageBox.Show("정말 청구 하시겠습니까?", "기사청구", MessageBoxButtons.YesNo) != DialogResult.Yes) return;


            try
            {
                var Table = new CMDataSet.DRIVERPAYDataTable();
                foreach (var item in cMDataSet.Drivers.Where(c => Int64.Parse(c.ServicePrice) > 0).ToArray())
                {
                    //dRIVERPAYTableAdapter.Insert(cmbYear.SelectedText + cmbMonth.SelectedText, item.DriverId, Decimal.Parse(item.ServicePrice), DBNull.Value);

                    var Query = cMDataSet.DRIVERPAY.Where(c => c.DriverId == item.DriverId && c.RequestDate == cmbYear.Text + cmbMonth.Text).ToArray();

                    if (Query.Count() == 0)
                    {

                        var Row = Table.NewDRIVERPAYRow();

                        Row.RequestDate = cmbYear.Text + cmbMonth.Text;
                        Row.DriverId = item.DriverId;
                        Row.UseTax = item.useTax;
                        if (item.useTax == true)
                        {
                           Row.UintPrice =  decimal.Parse((Int64.Parse(item.ServicePrice) / 1.1).ToString());
                           // Row.Tax =
                           Row.Tax = decimal.Parse(item.ServicePrice) - decimal.Parse((Int64.Parse(item.ServicePrice) / 1.1).ToString());
                        }
                        else
                        {
                            Row.UintPrice = decimal.Parse(item.ServicePrice);
                            Row.Tax = 0;
                        }
                        Row.Price = decimal.Parse(item.ServicePrice);
                        Row.ClientId = item.CandidateId;
                        Row.R_Price = 0;
                        //Row.FilterType = iNotificationFilterType;
                        //Row.State = iStartState;
                        //Row.City = iStartCity;
                        //Row.Street = iStartStreet;
                        //Row.Radius = iNotificationRadius;
                        //Row.GroupName = iNotificationGroupName;
                        //Row.IsChecked = false;
                        //Row.CreateTime = DateTime.Now;
                        //Row.DriverId = driverId;
                        //Row.ClientId = ClientId.Value;
                        //Row.OrderId = OrderId;
                        Table.AddDRIVERPAYRow(Row);
                        icount++;

                    }

                }
                //bar.Value = 0;
                //bar.Visible = true;
                //pnProgress.Visible = true;


                Thread t = new Thread(new ThreadStart(() =>
                {

                    //try
                    //{
                    //    bar.Invoke(new Action(() => bar.Value++));
                    //}
                    //  catch { }
                    var Adapter = new CMDataSetTableAdapters.DRIVERPAYTableAdapter();
                    Adapter.Update(Table);

                    //     pnProgress.Invoke(new Action(() => pnProgress.Visible = false));



                }));
                t.SetApartmentState(ApartmentState.STA);
                t.Start();

                if (icount > 0)
                {

                    MessageBox.Show(ButtonMessage.GetMessage(MessageType.청구성공, "기사 청구", icount), "기사 청구 성공");

                    btn_Search_Click(null, null);

                }
                else
                {
                    MessageBox.Show("청구할 건이 없습니다.", "청구", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
            }
            catch { }
        }
        private void barStep()
        {
            try
            {
                bar.Invoke(new Action(() => bar.Value++));
            }
            catch { }
        }
        private void btn_Search_Click(object sender, EventArgs e)
        {
            _Search();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btn_Delete_Click(object sender, EventArgs e)
        {
            List<DataGridViewRow> deleteRows = new List<DataGridViewRow>();

            if (dataGridView1.Rows.Count > 0)
            {
                dataGridView1.SelectAll();

                foreach (DataGridViewCell item in dataGridView1.SelectedCells)
                {
                    if (item.RowIndex != -1 && deleteRows.Contains(item.OwningRow) == false) deleteRows.Add(item.OwningRow);


                }
                if (MessageBox.Show(ButtonMessage.GetMessage(MessageType.삭제질문, "기사수금", deleteRows.Count), "선택항목 삭제 여부 확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

                foreach (DataGridViewRow row in deleteRows)
                {
                    dataGridView1.Rows.Remove(row);
                }
               

            }

            int _rows = UpdateDB();
            MessageBox.Show(ButtonMessage.GetMessage(MessageType.삭제성공, "기사수금", deleteRows.Count), Text, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }

        private int UpdateDB()
        {
            int _rows = 0;
            try
            {

                dRIVERPAYBindingSource.EndEdit();
                _rows = cMDataSet.DRIVERPAY.Where(c => c.RowState != DataRowState.Unchanged).Count();


               // var Row = ((DataRowView)dRIVERPAYBindingSource.Current).Row as CMDataSet.DRIVERPAYRow;




                //  clientsTableAdapter.Update(SingleDataSet.Instance.Clients);


                dRIVERPAYTableAdapter.Update(cMDataSet.DRIVERPAY);
                // MessageBox.Show(ButtonMessage.GetMessage(MessageType.삭제성공, "기사수금결제", 1), "기사수금결제 삭제 성공");
                btn_Search_Click(null, null);



            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "기사수금결제 변경 실패", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                _rows = -1;
            }
            return _rows;
        }

        private void btn_New_Click(object sender, EventArgs e)
        {
            YearAdd();
            cmbSearch.SelectedIndex = 0;
            txtSearch.Clear();
            cmb_Card.SelectedIndex = 0;
            btn_Search_Click(null, null);
        }

        private void cmbSearch_SelectedIndexChanged(object sender, EventArgs e)
        {

            txtSearch.Clear();
            if (cmbSearch.SelectedIndex == 0)
            {
                txtSearch.ReadOnly = true;
            }
            else
            {
                txtSearch.ReadOnly = false;
            }
        }

        private void txtSearch_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Return) return;
            _Search();
        }

        private void btn_File_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            getFilterString();

            if (MessageBox.Show(" 신용카드  결제를 하시겠습니까? \n\n 의뢰건수 :  "+checkedCodes.Count()+" 건" , "기사수금결제", MessageBoxButtons.YesNo) != DialogResult.Yes) return;

            var Url = "http://m.mycalltruck.co.kr/Admin/CardAuth";
            var models = dataGridView1.Rows.Cast<DataGridViewRow>().Where(c => (((DataRowView)c.DataBoundItem).Row as CMDataSet.DRIVERPAYRow).CheckBox).Select(c => ((DataRowView)c.DataBoundItem).Row as CMDataSet.DRIVERPAYRow);
            var Ids = models.Select(c => c.DriverPayId).ToArray();
            var IsTest = true;

            //Step 1. Login
            var SessinId = Guid.NewGuid().ToString();
            using (SqlConnection Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                Connection.Open();
                SqlCommand Command = Connection.CreateCommand();
                Command.CommandText =
                    @"INSERT INTO UserAccountInfoes (SessinId, UserId)  VALUES (@iSessinId, @iUserId)";
                Command.Parameters.AddWithValue("@iSessinId", SessinId);
                Command.Parameters.AddWithValue("@iUserId", LocalUser.Instance.LogInInformation.UserId);
                Command.ExecuteNonQuery();
            }
            //Step 2. Action
            WebClient mWebClient = new WebClient();
            mWebClient.Headers.Add(HttpRequestHeader.Cookie, String.Format("SessinId={0}", SessinId));
            mWebClient.Headers.Add(HttpRequestHeader.ContentType, "application/json");
            dynamic Data = new ExpandoObject();
            Data.IsTest = IsTest;
            Data.DriverPayIds = Ids;
            var sData = JsonConvert.SerializeObject(Data);
            try
            {
                var r = Encoding.UTF8.GetString(mWebClient.UploadData(Url, Encoding.UTF8.GetBytes(sData)));
                MessageBox.Show("결제가 완료 되었습니다.");
                btn_Search_Click(null, null);
            }
            catch (Exception)
            {
                MessageBox.Show("결제가 취소 되었습니다.");
            }
            //Step 3. Logout
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                var Selected = ((DataRowView)dataGridView1.SelectedRows[0].DataBoundItem).Row as CMDataSet.DRIVERPAYRow;
               

                if (Selected != null)
                {

                    //string FileInfo1 = Selected.Error;

                    //FileInfo fileinfo = null;




                    //if (File.Exists(FileInfo1) == true)
                    //{
                    //    System.Diagnostics.Process.Start(FileInfo1);
                    //}
                    //else
                    //{
                    if (!String.IsNullOrEmpty(Selected.Error))
                    {
                        MessageBox.Show("" + Selected.Error + "", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }

                    
                    

                }
            }
            catch { }
        }
        bool overhead = false;
        List<string> checkedCodes = new List<string>();

        private void chkAllSelect_CheckedChanged(object sender, EventArgs e)
        {
            overhead = true;
            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                dataGridView1[CheckBox.Index, i].Value = chkAllSelect.Checked;
            }
            overhead = false;
            dataGridView1_CellValueChanged(null, null);
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            
            try
            {

                if (e.ColumnIndex == 0)
                {
                    object o = dataGridView1[e.ColumnIndex, e.RowIndex].Value;
                    string code = ((DataRowView)dRIVERPAYBindingSource[e.RowIndex])["DriverPayId"].ToString();
                    if (Convert.ToBoolean(o))
                    {
                        if (!checkedCodes.Contains(code))
                            checkedCodes.Add(code);
                    }
                    else
                    {
                        if (checkedCodes.Contains(code))
                            checkedCodes.Remove(code);
                    }
                }

            }
            catch { }
        }
        private string getFilterString()
        {
            string r = "'0'";
            if (checkedCodes.Count > 0)
            {
                r = String.Join(",", checkedCodes.Select(c => "'" + c + "'").ToArray());
            }
            return r;
        }

        private void dataGridView1_Paint(object sender, PaintEventArgs e)
        {
            Rectangle rect1 = dataGridView1.GetColumnDisplayRectangle(CheckBox.Index, true);
            chkAllSelect.Location = new Point(rect1.Location.X + 2, rect1.Location.Y + 4);
            if (rect1.Width == 0) chkAllSelect.Visible = false;
            else chkAllSelect.Visible = true;
        }
    }
}
