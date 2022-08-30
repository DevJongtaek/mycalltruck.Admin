
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
    public partial class FrmMNSTATSbak : Form
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


        public FrmMNSTATSbak()
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

            //cmb_Month.DataSource = new BindingSource(MonthList, null);
            //cmb_Month.DisplayMember = "Value";
            //cmb_Month.ValueMember = "Key";
            if (!LocalUser.Instance.LogInInformation.IsAdmin)
            {
                LocalUser.Instance.LogInInformation.LoadClient();
               // lbl_ClientName.Text = LocalUser.Instance.LogInInformation.ClientName;
                ClientName = LocalUser.Instance.LogInInformation.ClientName;
                AllowFPIS_In = LocalUser.Instance.LogInInformation.Client.AllowFPIS_In;
                if (LocalUser.Instance.LogInInformation.ClientUserId > 0)
                {

                    //lbl_ClientName.Text = LocalUser.Instance.LogInInformation.Client.Name + "(" + LocalUser.Instance.LogInInformation.ClientName + ")";
                    ClientName = LocalUser.Instance.LogInInformation.Client.Name;
                }
            }
            else
            {
                //lbl_ClientName.Text = "관리자";
            }
            YearAdd();
            btn_Search_Click(null, null);
           
        }

        private void YearAdd()
        {
            //cmbYear.Items.Clear();
            //int iThatYear = int.Parse(DateTime.Now.Year.ToString());

            //for (int i = iThatYear - 3; i <= iThatYear + 1; i++)
            //{
            //    cmbYear.Items.Add(i);
            //}
            //cmbYear.SelectedIndex = cmbYear.FindString(iThatYear.ToString());

        }
        private void lbl_FPIS_Tru_Click(object sender, EventArgs e)
        {
            //if (AllowFPIS_In == false)
            //{
            //    MessageBox.Show("화물위탁(주는)정보를 사용하지 않고있습니다.\n 관리자에게 문의하세요", "경고", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //    return;
            //}

           

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

       

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
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
           


            //lbl_Title.Text = "차량별 직접운송 생성";
            



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
       
        }

        private void btn_Excel_Click(object sender, EventArgs e)
        {
            ExcelExport();

        }

        private void ExcelExport()
        {
            //ieExcel = Properties.Resources.일괄입력사용자_신고양식_미대행_;
            //FPIS_FILE_INFO =  LocalUser.Instance.PersonalOption.FPISCAR + "\\" + LocalUser.Instance.LogInInformation.ClientName + "\\운송";
            //fileString = string.Format("{0}_{1}_{2}{3}", ClientName, cmbYear.Text, cmb_Month.SelectedValue.ToString() + " 분기 ", DateTime.Now.ToString("hh:mm:ss").Replace(":", ""));
            //pnProgress.Visible = true;
            //bar.Value = 0;
            //Thread t = new Thread(new ThreadStart(() =>
            //{
            //    dataGridView1.ExportExistExcel(title, fileString, bar, true, ieExcel, 3, FPIS_FILE_INFO);
            //    pnProgress.Invoke(new Action(() => pnProgress.Visible = false));
            //}));
            //t.SetApartmentState(ApartmentState.STA);
            //t.Start();

        }

      
        UCAccount01 ucAccount011 = new UCAccount01();
        UCAccount02 ucAccount021 = new UCAccount02();
        UCAccount03 ucAccount031 = new UCAccount03();
        UCAccount04 ucAccount041 = new UCAccount04();
        UCAccount05 ucAccount051 = new UCAccount05();
        UCAccount06 ucAccount061 = new UCAccount06();
        UCSTATS1 ucSTATS11 = new UCSTATS1();
        UCSTATS2 ucSTATS21 = new UCSTATS2();
        UCSTATS3 ucSTATS31 = new UCSTATS3();
        UCSTATS4 ucSTATS41 = new UCSTATS4();
        UCSTATS5 ucSTATS51 = new UCSTATS5();
        UCSTATS6 ucSTATS61 = new UCSTATS6();
        UCSTATS7 ucSTATS71 = new UCSTATS7();
        UCNSTATS1 ucNSTATS1 = new UCNSTATS1();



        private void Account01()
        {
            

            this.panel2.Controls.Clear();
            this.panel2.Controls.Add(ucAccount011);
            //ucAccount011.Input.Text = "";
            //ucAccount011.ParentDataGrid.DataSource = "";
            //ucAccount011.ModelDataGrid.DataSource = "";
            ucAccount011.Dock = System.Windows.Forms.DockStyle.Fill;
            ucAccount011.Location = new System.Drawing.Point(0, 0);
            ucAccount011.Name = "ucAcoount011";
            ucAccount011.Size = new System.Drawing.Size(804, 579);
            ucAccount011.TabIndex = 0;

           
        }
        private void Account02()
        {
           

            this.panel2.Controls.Clear();
            this.panel2.Controls.Add(ucAccount021);
            ucAccount021.Dock = System.Windows.Forms.DockStyle.Fill;
            ucAccount021.Location = new System.Drawing.Point(0, 0);
            ucAccount021.Name = "ucAccount021";
            ucAccount021.Size = new System.Drawing.Size(804, 579);
            ucAccount021.TabIndex = 0;

            ucAccount021.Refresh();

        }
        private void Account03()
        {
            this.panel2.Controls.Clear();
            this.panel2.Controls.Add(ucAccount031);
            ucAccount031.Dock = System.Windows.Forms.DockStyle.Fill;
            ucAccount031.Location = new System.Drawing.Point(0, 0);
            ucAccount031.Name = "ucAccount031";
            ucAccount031.Size = new System.Drawing.Size(804, 579);
            ucAccount031.TabIndex = 0;
            ucAccount031.Refresh();
        }
        private void Account04()
        {
            this.panel2.Controls.Clear();
            this.panel2.Controls.Add(ucAccount041);
            ucAccount041.Dock = System.Windows.Forms.DockStyle.Fill;
            ucAccount041.Location = new System.Drawing.Point(0, 0);
            ucAccount041.Name = "ucAccount041";
            ucAccount041.Size = new System.Drawing.Size(804, 579);
            ucAccount041.TabIndex = 0;
        }
        private void Account05()
        {
            this.panel2.Controls.Clear();
            this.panel2.Controls.Add(ucAccount051);
            ucAccount051.Dock = System.Windows.Forms.DockStyle.Fill;
            ucAccount051.Location = new System.Drawing.Point(0, 0);
            ucAccount051.Name = "ucAccount051";
            ucAccount051.Size = new System.Drawing.Size(804, 579);
            ucAccount051.TabIndex = 0;
        }
        private void Account06()
        {
            this.panel2.Controls.Clear();
            this.panel2.Controls.Add(ucAccount061);
            ucAccount061.Dock = System.Windows.Forms.DockStyle.Fill;
            ucAccount061.Location = new System.Drawing.Point(0, 0);
            ucAccount061.Name = "ucAccount061";
            ucAccount061.Size = new System.Drawing.Size(804, 579);
            ucAccount061.TabIndex = 0;
        }

        private void STATS1()
        {
            this.panel2.Controls.Clear();
            this.panel2.Controls.Add(ucSTATS11);
            ucSTATS11.Dock = System.Windows.Forms.DockStyle.Fill;
            ucSTATS11.Location = new System.Drawing.Point(0, 0);
            ucSTATS11.Name = "ucSTATS11";
            ucSTATS11.Size = new System.Drawing.Size(804, 579);
            ucSTATS11.TabIndex = 0;
        }

        private void STATS2()
        {
            this.panel2.Controls.Clear();
            this.panel2.Controls.Add(ucSTATS21);
            ucSTATS21.Dock = System.Windows.Forms.DockStyle.Fill;
            ucSTATS21.Location = new System.Drawing.Point(0, 0);
            ucSTATS21.Name = "ucSTATS21";
            ucSTATS21.Size = new System.Drawing.Size(804, 579);
            ucSTATS21.TabIndex = 0;
        }

        private void STATS3()
        {
            this.panel2.Controls.Clear();
            this.panel2.Controls.Add(ucSTATS31);
            ucSTATS31.Dock = System.Windows.Forms.DockStyle.Fill;
            ucSTATS31.Location = new System.Drawing.Point(0, 0);
            ucSTATS31.Name = "ucSTATS31";
            ucSTATS31.Size = new System.Drawing.Size(804, 579);
            ucSTATS31.TabIndex = 0;
        }

        private void STATS4()
        {
            this.panel2.Controls.Clear();
            this.panel2.Controls.Add(ucSTATS41);
            ucSTATS41.Dock = System.Windows.Forms.DockStyle.Fill;
            ucSTATS41.Location = new System.Drawing.Point(0, 0);
            ucSTATS41.Name = "ucSTATS41";
            ucSTATS41.Size = new System.Drawing.Size(804, 579);
            ucSTATS41.TabIndex = 0;
        }

        private void STATS5()
        {
            this.panel2.Controls.Clear();
            this.panel2.Controls.Add(ucSTATS51);
            ucSTATS51.Dock = System.Windows.Forms.DockStyle.Fill;
            ucSTATS51.Location = new System.Drawing.Point(0, 0);
            ucSTATS51.Name = "ucSTATS51";
            ucSTATS51.Size = new System.Drawing.Size(804, 579);
            ucSTATS51.TabIndex = 0;
        }

        private void STATS6()
        {
            this.panel2.Controls.Clear();
            this.panel2.Controls.Add(ucSTATS61);
            ucSTATS61.Dock = System.Windows.Forms.DockStyle.Fill;
            ucSTATS61.Location = new System.Drawing.Point(0, 0);
            ucSTATS61.Name = "ucSTATS61";
            ucSTATS61.Size = new System.Drawing.Size(804, 579);
            ucSTATS61.TabIndex = 0;
        }

        private void STATS7()
        {
            this.panel2.Controls.Clear();
            this.panel2.Controls.Add(ucSTATS71);
            ucSTATS71.Dock = System.Windows.Forms.DockStyle.Fill;
            ucSTATS71.Location = new System.Drawing.Point(0, 0);
            ucSTATS71.Name = "ucSTATS71";
            ucSTATS71.Size = new System.Drawing.Size(804, 579);
            ucSTATS71.TabIndex = 0;
        }

        private void NSTAT1()
        {
            this.panel2.Controls.Clear();
            this.panel2.Controls.Add(ucNSTATS1);
            ucNSTATS1.Dock = System.Windows.Forms.DockStyle.Fill;
            ucNSTATS1.Location = new System.Drawing.Point(0, 0);
            ucNSTATS1.Name = "ucNSTATS1";
            ucNSTATS1.Size = new System.Drawing.Size(804, 579);
            ucNSTATS1.TabIndex = 0;
        }
        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if(treeView1.SelectedNode.Name == "Account01")
            {
                Account01();
            }
            else if (treeView1.SelectedNode.Name == "Account02")
            {
                Account02();
            }
            else if (treeView1.SelectedNode.Name == "Account03")
            {
                Account03();
            }
            else if (treeView1.SelectedNode.Name == "Account04")
            {
                Account04();
            }
            else if (treeView1.SelectedNode.Name == "Account05")
            {
                Account05();
            }
            else if (treeView1.SelectedNode.Name == "Account06")
            {
                Account06();
            }
            else if (treeView1.SelectedNode.Name == "STATS1")
            {
                STATS1();
            }
            else if (treeView1.SelectedNode.Name == "STATS2")
            {
                STATS2();
            }
            else if (treeView1.SelectedNode.Name == "STATS3")
            {
                STATS3();
            }
            else if (treeView1.SelectedNode.Name == "STATS4")
            {
                STATS4();
            }
            else if (treeView1.SelectedNode.Name == "STATS5")
            {
                STATS5();
            }
            else if (treeView1.SelectedNode.Name == "STATS6")
            {
                STATS6();
            }
            else if (treeView1.SelectedNode.Name == "STATS7")
            {
                STATS7();
            }

            else if (treeView1.SelectedNode.Name == "NSTAT1")
            {
                NSTAT1();
            }

        }
    }
}
