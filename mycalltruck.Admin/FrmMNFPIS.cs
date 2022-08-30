
using mycalltruck.Admin.Class.Extensions;
using mycalltruck.Admin.Class.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using mycalltruck.Admin.DataSets;
using System.Runtime.CompilerServices;

namespace mycalltruck.Admin
{
    public partial class FrmMNFPIS : Form
    {

        //private DateTime now = DateTime.Now;
        //private DataGridViewCellStyle headerStyle = new DataGridViewCellStyle();
        //private DataGridViewCellStyle leftheaderStyle = new DataGridViewCellStyle();
        //private DataGridViewCellStyle numberStyle = new DataGridViewCellStyle();
        //private DataGridViewCellStyle moneyStyle = new DataGridViewCellStyle();
        //private DataGridViewCellStyle normalStyle = new DataGridViewCellStyle();
        //private DataGridViewCellStyle centerStyle = new DataGridViewCellStyle();
        //private DataGridViewCellStyle rightStyle = new DataGridViewCellStyle();
        //private DataGridViewCellStyle sumStyle = new DataGridViewCellStyle();
        //private DataGridViewCellStyle sumNumStyle = new DataGridViewCellStyle();
        //private DataGridViewCellStyle sumMoneyStyle = new DataGridViewCellStyle();
        //private DataGridViewCellStyle minusmoneyStyle = new DataGridViewCellStyle();
        //private DataGridViewCellStyle ordernumStyle = new DataGridViewCellStyle();
        //private DataGridViewCellStyle bluefontnumber;
        //private DataGridViewCellStyle bluefontordernumStyle = new DataGridViewCellStyle();



        string Gubun = string.Empty;
        string sDate = string.Empty;
        string eDate = string.Empty;
        string Quater = string.Empty;
        string MakeFpis = string.Empty;
        string NEWFPISCount = "0";


        string sDate1 = string.Empty;
        string eDate1 = string.Empty;
        string FPIS_FILE_INFO = string.Empty;
        string FPIS_ID_IN = string.Empty;
        string ClientName = string.Empty;
        int R_index = 0;
        bool AllowFPIS_In = false;
        private Thread t = null;


        public FrmMNFPIS()
        {
            InitializeComponent();
        }

        private void FrmMNFPIS_Load(object sender, EventArgs e)
        {
            Dictionary<string, string> MonthList = new Dictionary<string, string>();
            MonthList.Add("1", "1분기(1~3월)");
            MonthList.Add("2", "2분기(4~6월)");
            MonthList.Add("3", "3분기(7~9월)");
            MonthList.Add("4", "4분기(10~12월)");

            cmb_Month.DataSource = new BindingSource(MonthList, null);
            cmb_Month.DisplayMember = "Value";
            cmb_Month.ValueMember = "Key";
            if (!LocalUser.Instance.LogInInformation.IsAdmin)
            {
                LocalUser.Instance.LogInInformation.LoadClient();
                lbl_ClientName.Text = LocalUser.Instance.LogInInformation.ClientName;
                ClientName = LocalUser.Instance.LogInInformation.ClientName;
                AllowFPIS_In = LocalUser.Instance.LogInInformation.Client.AllowFPIS_In;
                if (LocalUser.Instance.LogInInformation.ClientUserId > 0)
                {

                    lbl_ClientName.Text = LocalUser.Instance.LogInInformation.Client.Name + "(" + LocalUser.Instance.LogInInformation.ClientName + ")";
                    ClientName = LocalUser.Instance.LogInInformation.Client.Name;
                }
            }
            else
            {
                lbl_ClientName.Text = "관리자";
            }
            YearAdd();
            btn_Search_Click(null, null);
            lbl_ExcelPath.Text = "엑셀파일폴더 : " + LocalUser.Instance.PersonalOption.FPISCAR + "\\" + LocalUser.Instance.LogInInformation.ClientName + "\\운송";
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
        private void lbl_FPIS_Tru_Click(object sender, EventArgs e)
        {
            //if (AllowFPIS_In == false)
            //{
            //    MessageBox.Show("화물위탁(주는)정보를 사용하지 않고있습니다.\n 관리자에게 문의하세요", "경고", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //    return;
            //}

            btn_File.Visible = true;


            R_index = 0;



          

            FPIS_FILE_INFO = LocalUser.Instance.PersonalOption.FPISTRU + "\\" + ClientName + "\\위탁";
        }




        string fileString = string.Empty;
        string title = string.Empty;
        byte[] ieExcel;





        private void btn_File_Click(object sender, EventArgs e)
        {

            FrmMNFPISADD _Form = new FrmMNFPISADD();
            _Form.Owner = this;
            _Form.StartPosition = FormStartPosition.CenterParent;
            _Form.ShowDialog(

            );
            btn_Search_Click(null, null);

            // FPISCreate();


        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == 0 && e.RowIndex >= 0)
            {
                dataGridView1[e.ColumnIndex, e.RowIndex].Value = (dataGridView1.Rows.Count - e.RowIndex).ToString("N0");
            }
            
            else if (dataGridView1.Columns[e.ColumnIndex] == tRUDEPOSITDataGridViewTextBoxColumn1)
            {
                //var Selected = ((DataRowView)dataGridView1.Rows[e.RowIndex].DataBoundItem).Row as FPISDataSet.NewFPISRow;


                //if(Selected.TRU_DEPOSIT == "0")
                //{
                //    e.Value = "";
                //}
                
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //if (e.ColumnIndex != 4)
            //    return;

            //try
            //{
            //    var Selected = ((DataRowView)dataGridView1.SelectedRows[0].DataBoundItem).Row as CMDataSet.FPIS_FILERow;
            //    if (Selected != null)
            //    {

            //        string FileInfo1 = Selected.FOLDER + "\\" + Selected.FILE_NAME + ".xls";

            //        FileInfo fileinfo = null;




            //        if (File.Exists(FileInfo1) == true)
            //        {
            //            System.Diagnostics.Process.Start(FileInfo1);
            //        }
            //        else
            //        {


            //            MessageBox.Show("해당파일을 찾을수 없습니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //        }


            //    }
            //}
            //catch { }
        }

        private void label9_Click(object sender, EventArgs e)
        {
            //FrmMNCarFPIS _Form = new FrmMNCarFPIS();
            //_Form.Owner = this;
            //_Form.StartPosition = FormStartPosition.CenterParent;
            //_Form.ShowDialog();

            //panel15.Visible = false;


            // panel18.Visible = false;
            //dtp_Sdate.Visible = false;
            //dtp_Edate.Visible = false;
            btn_File.Visible = true;


            lbl_Title.Text = "차량별 직접운송 생성";
            btn_File.Text = "직접운송 화일생성";




            Gubun = "CarFPIS";
            R_index = 0;



        

            //  FPIS_FILE_INFO = LocalUser.Instance.LogInInfomation.FPISCAR + "\\" + item.Name + "\\운송";


        }

        Thread tAction;
      


        private void grid1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void grid1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            //if (e.ColumnIndex != 7)
            //    return;

            //try
            //{
            //    var Selected = ((DataRowView)grid1.SelectedRows[0].DataBoundItem).Row as CMDataSet.FPIS_FILERow;
            //    //var Selected = ((DataRowView)fPISFILEBindingSource.Current).Row as CMDataSet.FPIS_FILERow;


            //    if (Selected != null)
            //    {

            //        string FileInfo1 = Selected.FOLDER + "\\" + Selected.FILE_NAME + ".xls";

            //        FileInfo fileinfo = null;




            //        if (File.Exists(FileInfo1) == true)
            //        {
            //            System.Diagnostics.Process.Start(FileInfo1);
            //        }
            //        else
            //        {


            //            MessageBox.Show("해당파일을 찾을수 없습니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //        }


            //    }
            //}
            //catch { }
        }

        private void grid1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            //try
            //{
            //    var Selected = ((DataRowView)grid1.SelectedRows[0].DataBoundItem).Row as CMDataSet.FPIS_FILERow;
            //    //var Selected = ((DataRowView)fPISFILEBindingSource.Current).Row as CMDataSet.FPIS_FILERow;


            //    if (Selected != null)
            //    {

            //        string FileInfo1 = Selected.FOLDER + "\\" + Selected.FILE_NAME + ".xls";

            //        FileInfo fileinfo = null;




            //        if (File.Exists(FileInfo1) == true)
            //        {
            //            System.Diagnostics.Process.Start(FileInfo1);
            //        }
            //        else
            //        {


            //            MessageBox.Show("해당파일을 찾을수 없습니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //        }


            //    }
            //}
            //catch { }
        }

        private void newDGV1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            //if (e.ColumnIndex == 1 && e.RowIndex >= 0)
            //{
            //    var Selected = ((DataRowView)newDGV1.Rows[e.RowIndex].DataBoundItem).Row as CMDataSet.FPIS_TRU_EXCELRow;
            //    if (Selected.Gubun == "Order")
            //    {
            //        var Query = cMDataSet.Drivers.Where(c => c.DriverId == Selected.driverid).ToArray();

            //        if (Query.Any())
            //        {
            //            if (Query.First().BizNo.Length == 10)
            //            {
            //                e.Value = Query.First().BizNo.Substring(0, 3) + "-" + Query.First().BizNo.Substring(3, 2) + "-" + Query.First().BizNo.Substring(5, 5); ;

            //            }
            //            else
            //            {
            //                e.Value = Query.First().BizNo;
            //            }
            //        }

            //    }

            //}
            //else if (newDGV1.Columns[e.ColumnIndex] == cONTFROMDataGridViewTextBoxColumn)
            //{
            //    var Selected = ((DataRowView)newDGV1.Rows[e.RowIndex].DataBoundItem).Row as CMDataSet.FPIS_TRU_EXCELRow;
            //    e.Value = Selected.CONT_FROM.Substring(1, 6);

            //}

            //else if (newDGV1.Columns[e.ColumnIndex] == Column11)
            ////else if (e.ColumnIndex == 2 && e.RowIndex >= 0)
            //{
            //    var Selected = ((DataRowView)newDGV1.Rows[e.RowIndex].DataBoundItem).Row as CMDataSet.FPIS_TRU_EXCELRow;
            //    e.Value = Selected.CONT_FROM + "_R" + (newDGV1.Rows.Count - e.RowIndex).ToString("N0");

            //}
        }

        private void lbl_FPIS_Car_Total_Click(object sender, EventArgs e)
        {

        }

        private void newDGV2_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            //if (newDGV2.Columns[e.ColumnIndex] == FpisId)
            //{
            //    e.Value = (newDGV2.Rows.Count - e.RowIndex).ToString("N0");
            //}
        }

        private void btn_info_Click(object sender, EventArgs e)
        {

        }

        private void btn_Search_Click(object sender, EventArgs e)
        {
            newFPISTableAdapter.Fill(fPISDataSet.NewFPIS, LocalUser.Instance.LogInInformation.ClientId);


            newFPISBindingSource.Filter = "Quater = '" + cmb_Month.SelectedValue.ToString() + "' AND MakeFpis = '" + cmbYear.Text + "'";
        }

        private void btn_Excel_Click(object sender, EventArgs e)
        {
            ExcelExport();

        }

        private void ExcelExport()
        {
            ieExcel = Properties.Resources.일괄입력사용자_신고양식_미대행_;
            FPIS_FILE_INFO =  LocalUser.Instance.PersonalOption.FPISCAR + "\\" + LocalUser.Instance.LogInInformation.ClientName + "\\운송";
            fileString = string.Format("{0}_{1}_{2}{3}", ClientName, cmbYear.Text, cmb_Month.SelectedValue.ToString() + " 분기 ", DateTime.Now.ToString("hh:mm:ss").Replace(":", ""));
            pnProgress.Visible = true;
            bar.Value = 0;
            Thread t = new Thread(new ThreadStart(() =>
            {
                dataGridView1.ExportExistExcel(title, fileString, bar, true, ieExcel, 3, FPIS_FILE_INFO);
                pnProgress.Invoke(new Action(() => pnProgress.Visible = false));
            }));
            t.SetApartmentState(ApartmentState.STA);
            t.Start();

        }

        class ExcelModel : INotifyPropertyChanged
        {
            private String _SBizNo = "";
            private String _SBizGubun = "";
            private String _SIdx = "";
            private String _SCont_From = "";
            private String _SCont_Deposit = "";
            private String _SETC1 = "";
            private String _SETC2 = "";
            private String _SCarNo = "";
            private String _SStopTime = "";
            private String _SPrice = "";
            private String _SCarCount = "";
            private String _STRU_COMP_MSNS_NUM = "";
            private String _STRU_CONT_FROM = "";
            private String _STRU_DEPOSIT = "";
            private String _STRU_MANG_TYPE = "";
          

            public string SBizNo
            {
                get
                {
                    return _SBizNo;
                }

                set
                {
                    SetField(ref _SBizNo, value);
                }
            }

            public string SBizGubun
            {
                get
                {
                    return _SBizGubun;
                }

                set
                {
                    SetField(ref _SBizGubun, value);
                }
            }

            public string SIdx
            {
                get
                {
                    return _SIdx;
                }

                set
                {
                    SetField(ref _SIdx, value);
                }
            }

            public string SCont_From
            {
                get
                {
                    return _SCont_From;
                }

                set
                {
                    SetField(ref _SCont_From, value);
                }
            }

            public string SCont_Deposit
            {
                get
                {
                    return _SCont_Deposit;
                }

                set
                {
                    SetField(ref _SCont_Deposit, value);
                }
            }

            public string SETC1
            {
                get
                {
                    return _SETC1;
                }

                set
                {
                    SetField(ref _SETC1, value);
                }
            }

            public string SCarNo
            {
                get
                {
                    return _SCarNo;
                }

                set
                {
                    SetField(ref _SCarNo, value);
                }
            }
            public string SStopTime
            {
                get
                {
                    return _SStopTime;
                }

                set
                {
                    SetField(ref _SStopTime, value);
                }
            }

            public string SPrice
            {
                get
                {
                    return _SPrice;
                }

                set
                {
                    SetField(ref _SPrice, value);
                }
            }


            public string SCarCount
            {
                get
                {
                    return _SCarCount;
                }

                set
                {
                    SetField(ref _SCarCount, value);
                }
            }
            public string STRU_COMP_MSNS_NUM
            {
                get
                {
                    return _STRU_COMP_MSNS_NUM;
                }

                set
                {
                    SetField(ref _STRU_COMP_MSNS_NUM, value);
                }
            }
            public string STRU_CONT_FROM
            {
                get
                {
                    return _STRU_CONT_FROM;
                }

                set
                {
                    SetField(ref _STRU_CONT_FROM, value);
                }
            }
            public string STRU_DEPOSIT
            {
                get
                {
                    return _STRU_DEPOSIT;
                }

                set
                {
                    SetField(ref _STRU_DEPOSIT, value);
                }
            }

            public string STRU_MANG_TYPE
            {
                get
                {
                    return _STRU_MANG_TYPE;
                }

                set
                {
                    SetField(ref _STRU_MANG_TYPE, value);
                }
            }







            public event PropertyChangedEventHandler PropertyChanged;
            protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
            {
                if (EqualityComparer<T>.Default.Equals(field, value)) return false;
                field = value;
                OnPropertyChanged(propertyName);
                return true;
            }
        }

        private void lbl_ExcelPath_Click(object sender, EventArgs e)
        {
            string FileInfo1 =  LocalUser.Instance.PersonalOption.FPISCAR + "\\" + LocalUser.Instance.LogInInformation.ClientName + "\\운송";


            System.Diagnostics.Process.Start(FileInfo1);


           
        }
    }
}
