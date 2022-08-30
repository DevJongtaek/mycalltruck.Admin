using mycalltruck.Admin.Class.Common;
using mycalltruck.Admin.Class.Extensions;
using mycalltruck.Admin.UI;
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

namespace mycalltruck.Admin
{
    public partial class FrmMN0303_CARGOFPIS : Form
    {
        string UP_Status = string.Empty;
        string G_Gubun = string.Empty;
        string SIdx = string.Empty;
        bool fPISTRUBindingSource_CurrentChanging = false;

        bool fPISINFO1BindingSource_CurrentChanging = false;
        int GridIndex = 0;
        bool AllowFPIS_In = false;
        int iContType = 0;
        MenuAuth auth = MenuAuth.None;
        private void ApplyAuth()
        {
            auth = this.GetAuth();
            switch (auth)
            {
                case MenuAuth.None:
                    MessageBox.Show("잘못된 접근입니다. 권한이 없는 메뉴로 들어왔습니다.\n해당 프로그램을 종료합니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    Close();
                    return;
                case MenuAuth.Read:

                    btn_New.Enabled = false;
                    btnUpdate.Enabled = false;
                    btnCurrentDelete.Enabled = false;




                    dataGridView1.ReadOnly = true;
                    //grid2.ReadOnly = true;
                    break;
            }


        }

        public FrmMN0303_CARGOFPIS()
        {
            InitializeComponent();

            if (!LocalUser.Instance.LogInInformation.IsAdmin)
            {
                ApplyAuth();
            }



            dtpStart.Value = DateTime.Parse(DateTime.Now.ToString("yyyy") + "-01-01");
            dtpEnd.Value = DateTime.Now;


          
            dtp_CONT_FROM.Value = DateTime.Now;
            cmb_Search.SelectedIndex = 1;
            cmb_DateSearch.SelectedIndex = 1;
            cmb_Cont.SelectedIndex = 0;
       

            dtpSdateS.Value = DateTime.Parse(DateTime.Now.ToString("yyyy") + "-01-01");
            dtpEdateS.Value = DateTime.Now;

            cmb_TRU_Search.SelectedIndex = 0;




            cmb_FPISSearch.SelectedIndex = 0;

            cmb_Fpis.SelectedIndex = 0;
           
        //    fPISTRUBindingSource_CurrentChanged(null, null);
       

            if (!LocalUser.Instance.LogInInformation.IsAdmin)
            {
                dataGridView1.Columns[2].Visible = false;
                dataGridView1.Columns[3].Visible = false;
                cmb_FPISSearch.Visible = false;
                txt_FPISSearch.Visible = false;
                //     tableLayoutPanel10.Visible = true;

                
                    InitClientTable();
                



            }
            else
            {
                btn_New.Enabled = false;
            }
        }
        class ClientViewModel
        {
            public int ClientId { get; set; }
            public string Code { get; set; }
            public string Name { get; set; }
            public string LoginId { get; set; }
        }
        private List<ClientViewModel> _ClientTable = new List<ClientViewModel>();

        private void InitClientTable()
        {
            using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                connection.Open();
                using (SqlCommand commnad = connection.CreateCommand())
                {
                    commnad.CommandText = "SELECT ClientId, Code, Name, LoginId FROM Clients WHERE ClientId  = @ClientId";
                    commnad.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                    using (SqlDataReader dataReader = commnad.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            _ClientTable.Add(
                              new ClientViewModel
                              {
                                  ClientId = dataReader.GetInt32(0),
                                  Code = dataReader.GetString(1),
                                  Name = dataReader.GetString(2),
                                  LoginId = dataReader.GetString(3),
                              });
                        }
                    }
                }
                connection.Close();
            }
        }
        private void _InitCmb_Search()
        {
            var GridCargoItemDataSource = cMDataSet.FPISOptions.Where(c => c.Div == "CargoItem").Select(c => new { c.FpisOptionId, c.Name, c.Seq, c.Value }).OrderBy(c => c.Seq).ToArray();

            cONTITEMDataGridViewTextBoxColumn.DataSource = GridCargoItemDataSource;
            cONTITEMDataGridViewTextBoxColumn.DisplayMember = "Name";
            cONTITEMDataGridViewTextBoxColumn.ValueMember = "value";


          
            var GridCargoFormDataSource = cMDataSet.FPISOptions.Where(c => c.Div == "CargoForm").Select(c => new { c.FpisOptionId, c.Name, c.Seq, c.Value }).OrderBy(c => c.Seq).ToArray();
            cONTGOODSFORMDataGridViewTextBoxColumn.DataSource = GridCargoFormDataSource;
            cONTGOODSFORMDataGridViewTextBoxColumn.DisplayMember = "Name";
            cONTGOODSFORMDataGridViewTextBoxColumn.ValueMember = "value";



            var GridCargoSizeDataSource = cMDataSet.FPISOptions.Where(c => c.Div == "CargoSize").Select(c => new { c.FpisOptionId, c.Name, c.Seq, c.Value }).OrderBy(c => c.Seq).ToArray();
            cONTGOODSUNITDataGridViewTextBoxColumn.DataSource = GridCargoSizeDataSource;
            cONTGOODSUNITDataGridViewTextBoxColumn.DisplayMember = "Name";
            cONTGOODSUNITDataGridViewTextBoxColumn.ValueMember = "value";


            var CargoUseDataSource = cMDataSet.FPISOptions.Where(c => c.Div == "CargoUse").Select(c => new { c.FpisOptionId, c.Name, c.Seq, c.Value }).OrderBy(c => c.Seq).ToArray();
            CONT_MANG_TYPE.DataSource = CargoUseDataSource;
            CONT_MANG_TYPE.DisplayMember = "Name";
            CONT_MANG_TYPE.ValueMember = "value";


             var CargoUseDataSource2 = cMDataSet.FPISOptions.Where(c => c.Div == "CargoUse").Select(c => new { c.FpisOptionId, c.Name, c.Seq, c.Value }).OrderBy(c => c.Seq).ToArray();
             tRUMANGTYPEDataGridViewTextBoxColumn.DataSource = CargoUseDataSource2;
             tRUMANGTYPEDataGridViewTextBoxColumn.DisplayMember = "Name";
             tRUMANGTYPEDataGridViewTextBoxColumn.ValueMember = "value";

            


        }
        private void _InitCmb()
        {
            
            var CargoItemDataSource = cMDataSet.FPISOptions.Where(c => c.Div == "CargoItem").Select(c => new { c.FpisOptionId, c.Name, c.Seq, c.Value }).OrderBy(c => c.Seq).ToArray();
            cmb_CONT_ITEM.DataSource = CargoItemDataSource;
            cmb_CONT_ITEM.DisplayMember = "Name";
            cmb_CONT_ITEM.ValueMember = "value";

            var CargoItemDataSource2 = cMDataSet.FPISOptions.Where(c => c.Div == "CargoItem").Select(c => new { c.FpisOptionId, c.Name, c.Seq, c.Value }).OrderBy(c => c.Seq).ToArray();
            cmb_TRU_CONT_ITEM.DataSource = CargoItemDataSource2;
            cmb_TRU_CONT_ITEM.DisplayMember = "Name";
            cmb_TRU_CONT_ITEM.ValueMember = "value";



            var CargoFormDataSource = cMDataSet.FPISOptions.Where(c => c.Div == "CargoForm").Select(c => new { c.FpisOptionId, c.Name, c.Seq, c.Value }).OrderBy(c => c.Seq).ToArray();
            cmb_CONT_GOODS_FORM.DataSource = CargoFormDataSource;
            cmb_CONT_GOODS_FORM.DisplayMember = "Name";
            cmb_CONT_GOODS_FORM.ValueMember = "value";

            var CargoFormDataSource2 = cMDataSet.FPISOptions.Where(c => c.Div == "CargoForm").Select(c => new { c.FpisOptionId, c.Name, c.Seq, c.Value }).OrderBy(c => c.Seq).ToArray();
            cmb_TRU_GOODS_FORM.DataSource = CargoFormDataSource2;
            cmb_TRU_GOODS_FORM.DisplayMember = "Name";
            cmb_TRU_GOODS_FORM.ValueMember = "value";


            var CargoSizeDataSource = cMDataSet.FPISOptions.Where(c => c.Div == "CargoSize").Select(c => new { c.FpisOptionId, c.Name, c.Seq, c.Value }).OrderBy(c => c.Seq).ToArray();
            cmb_CONT_GOODS_UNIT.DataSource = CargoSizeDataSource;
            cmb_CONT_GOODS_UNIT.DisplayMember = "Name";
            cmb_CONT_GOODS_UNIT.ValueMember = "value";


            var CargoUseDataSource = cMDataSet.FPISOptions.Where(c => c.Div == "CargoUse").Select(c => new { c.FpisOptionId, c.Name, c.Seq, c.Value }).OrderBy(c => c.Seq).ToArray();
            cmb_CONT_MANG_TYPE.DataSource = CargoUseDataSource;
            cmb_CONT_MANG_TYPE.DisplayMember = "Name";
            cmb_CONT_MANG_TYPE.ValueMember = "value";

            var CargoUseDataSource2 = cMDataSet.FPISOptions.Where(c => c.Div == "CargoUse").Select(c => new { c.FpisOptionId, c.Name, c.Seq, c.Value }).OrderBy(c => c.Seq).ToArray();
            cmb_TRU_MANG_TYPE.DataSource = CargoUseDataSource2;
            cmb_TRU_MANG_TYPE.DisplayMember = "Name";
            cmb_TRU_MANG_TYPE.ValueMember = "value";

            var CargoCarryDataSource = cMDataSet.FPISOptions.Where(c => c.Div == "CargoCarry").Select(c => new { c.FpisOptionId, c.Name, c.Seq, c.Value }).OrderBy(c => c.Seq).ToArray();
            cmb_TRU_TRANS_TYPE.DataSource = CargoCarryDataSource;
            cmb_TRU_TRANS_TYPE.DisplayMember = "Name";
            cmb_TRU_TRANS_TYPE.ValueMember = "value";

            var CargoGubunDataSource = cMDataSet.FPISOptions.Where(c => c.Div == "CargoGubun").Select(c => new { c.FpisOptionId, c.Name, c.Seq, c.Value }).OrderBy(c => c.Seq).ToArray();
            cmb_CargoGubun.DataSource = CargoGubunDataSource;
            cmb_CargoGubun.DisplayMember = "Name";
            cmb_CargoGubun.ValueMember = "value";

        }



        private void FrmMN0301_CARGOACCEPT_Load(object sender, EventArgs e)
        {
            // TODO: 이 코드는 데이터를 'cMDataSet.FPIS_TRU_EXCEL' 테이블에 로드합니다. 필요한 경우 이 코드를 이동하거나 제거할 수 있습니다.
            this.fPIS_TRU_EXCELTableAdapter.Fill(this.cMDataSet.FPIS_TRU_EXCEL);
            // TODO: 이 코드는 데이터를 'cMDataSet.FPIS_TRU' 테이블에 로드합니다. 필요한 경우 이 코드를 이동하거나 제거할 수 있습니다.
            this.fPIS_TRUTableAdapter.Fill(this.cMDataSet.FPIS_TRU);
            // TODO: 이 코드는 데이터를 'cMDataSet.FPIS_CONT' 테이블에 로드합니다. 필요한 경우 이 코드를 이동하거나 제거할 수 있습니다.
            this.fPIS_CONTTableAdapter.Fill(this.cMDataSet.FPIS_CONT);

            // TODO: 이 코드는 데이터를 'cMDataSet.FPISINFO' 테이블에 로드합니다. 필요한 경우 이 코드를 이동하거나 제거할 수 있습니다.
            this.fPISINFO1TableAdapter.Fill(this.cMDataSet.FPISINFO1, LocalUser.Instance.LogInInformation.ClientId);

            this.clientUsersTableAdapter.FillForAdmin(this.cMDataSet.ClientUsers);

            this.customersTableAdapter.Fill(this.cMDataSet.Customers);


            this.fpisOptionsTableAdapter.Fill(this.cMDataSet.FPISOptions);
            dtp_I_Start.Value = DateTime.Parse(DateTime.Now.ToString("yyyy") + "-01-01");
            dtp_I_End.Value = DateTime.Now;


            _InitCmb();
            _InitCmb_Search();



            btn_Search_Click(null, null);
            //   btn_CL_Search_Click(null, null);
            clientsTableAdapter.Fill(this.cMDataSet.Clients);
            if (!LocalUser.Instance.LogInInformation.IsAdmin)
            {
                var Query = cMDataSet.Clients.Where(c => c.ClientId == LocalUser.Instance.LogInInformation.ClientId).ToArray();

                AllowFPIS_In = Query.First().AllowFPIS_In;
                iContType = Query.First().ContType;
            }
            tabControl1.SelectTab(0);

            //  dataGridView2_CellClick(null, null);
            if (!AllowFPIS_In)
            {
                tabPage2.Enabled = false;
                
            }
            else if (LocalUser.Instance.LogInInformation.IsAdmin)
            {
                tabPage2.Enabled = false;
            }
            else
            {
                tabPage2.Enabled = true;
            }


           // var CleintQuery = cMDataSet.Clients.Where(c => c.ClientId == LocalUser.Instance.LogInInfomation.ClientId).ToArray();

            if (iContType == 2)
            {
                panel_Standard.Visible = true;
                Panel_Standard2.Visible = true;
            }
            else
            {
                panel_Standard.Visible = false;
                Panel_Standard2.Visible = false;
            }
            _SetFocus(this);
        }

        private void _SetFocus(Control _Parent)
        {
            foreach (Control _Control in _Parent.Controls)
            {
                _Control.GotFocus += (sender, e) => {
                    Console.WriteLine(((Control)sender).Name);
                };
                _SetFocus(_Control);
            }
        }

        private void btn_New_Click(object sender, EventArgs e)
        {
            //의뢰자/계약정보
            if (tabControl1.SelectedIndex == 0)
            {
               // clientsTableAdapter.Fill(this.cMDataSet.Clients);
               // var Query = cMDataSet.Clients.Where(c => c.ClientId == LocalUser.Instance.LogInInfomation.ClientId).ToArray();
                if (iContType == 2)
                {
                    FrmMN0303_CARGOFPIS_Add _Form = new FrmMN0303_CARGOFPIS_Add();
                    _Form.Owner = this;
                    _Form.StartPosition = FormStartPosition.CenterParent;
                    _Form.ShowDialog();
                    btn_Search_Click(null, null);
                }
                else
                {
                    FrmMN0303_CARGOFPIS_Add_Default _Form = new FrmMN0303_CARGOFPIS_Add_Default();
                    _Form.Owner = this;
                    _Form.StartPosition = FormStartPosition.CenterParent;
                    _Form.ShowDialog();
                    btn_Search_Click(null, null);
                }

            }
            else
            {
                string FpisId = txt_FpisId.Text;
                string GoodsUnit = txt_GoodsUnit.Text;

                if (!String.IsNullOrEmpty(FpisId) && !String.IsNullOrEmpty(GoodsUnit))
                {
                    if (iContType == 2)
                    {
                        FrmMN0303_CARGOFPIS_Add2 _Form = new FrmMN0303_CARGOFPIS_Add2(FpisId, GoodsUnit);
                        _Form.Owner = this;
                        _Form.StartPosition = FormStartPosition.CenterParent;
                        _Form.ShowDialog();

                        btn_TRU_Search_Click(null, null);

                    }
                    else
                    {
                        FrmMN0303_CARGOFPIS_Add2_Default _Form = new FrmMN0303_CARGOFPIS_Add2_Default(FpisId, GoodsUnit);
                        _Form.Owner = this;
                        _Form.StartPosition = FormStartPosition.CenterParent;
                        _Form.ShowDialog();

                        btn_TRU_Search_Click(null, null);



                    }


                }
                else
                {
                    MessageBox.Show("의뢰자/계약정보가 없습니다.");
                }



            }
           // fPISINFOBindingSource_CurrentChanged(null, null);
            
           
         
        }

        private void btnAddr1_Click(object sender, EventArgs e)
        {


            FrmCargoZip _frmCargoZip = new FrmCargoZip();
            _frmCargoZip.grid1.KeyDown += new KeyEventHandler((object isender, KeyEventArgs ie) =>
            {
                if (ie.KeyCode != Keys.Return) return;
                if (_frmCargoZip.grid1.SelectedCells.Count == 0) return;
                if (_frmCargoZip.grid1.SelectedCells[0].RowIndex < 0) return;


                txtStartAddr.Text = _frmCargoZip.grid1[1, _frmCargoZip.grid1.SelectedCells[0].RowIndex].Value.ToString();
                txtStartZip.Text = _frmCargoZip.grid1[0, _frmCargoZip.grid1.SelectedCells[0].RowIndex].Value.ToString();

              

                _frmCargoZip.Close();
            });
            _frmCargoZip.grid1.MouseDoubleClick += new MouseEventHandler((object isender, MouseEventArgs ie) =>
            {
                if (_frmCargoZip.grid1.SelectedCells.Count == 0) return;
                if (_frmCargoZip.grid1.SelectedCells[0].RowIndex < 0) return;
                txtStartAddr.Text = _frmCargoZip.grid1[1, _frmCargoZip.grid1.SelectedCells[0].RowIndex].Value.ToString();
                txtStartZip.Text = _frmCargoZip.grid1[0, _frmCargoZip.grid1.SelectedCells[0].RowIndex].Value.ToString();

                _frmCargoZip.Close();
            });






            _frmCargoZip.Owner = this;
            _frmCargoZip.StartPosition = FormStartPosition.CenterParent;
            _frmCargoZip.ShowDialog();
           
        }

        private void btnAddr2_Click(object sender, EventArgs e)
        {
            FrmCargoZip _frmCargoZip = new FrmCargoZip();
            _frmCargoZip.grid1.KeyDown += new KeyEventHandler((object isender, KeyEventArgs ie) =>
            {
                if (ie.KeyCode != Keys.Return) return;
                if (_frmCargoZip.grid1.SelectedCells.Count == 0) return;
                if (_frmCargoZip.grid1.SelectedCells[0].RowIndex < 0) return;


                txtEndAddr.Text = _frmCargoZip.grid1[1, _frmCargoZip.grid1.SelectedCells[0].RowIndex].Value.ToString();
                txtEndZip.Text = _frmCargoZip.grid1[0, _frmCargoZip.grid1.SelectedCells[0].RowIndex].Value.ToString();


                _frmCargoZip.Close();
            });
            _frmCargoZip.grid1.MouseDoubleClick += new MouseEventHandler((object isender, MouseEventArgs ie) =>
            {
                if (_frmCargoZip.grid1.SelectedCells.Count == 0) return;
                if (_frmCargoZip.grid1.SelectedCells[0].RowIndex < 0) return;
                txtEndAddr.Text = _frmCargoZip.grid1[1, _frmCargoZip.grid1.SelectedCells[0].RowIndex].Value.ToString();
                txtEndZip.Text = _frmCargoZip.grid1[0, _frmCargoZip.grid1.SelectedCells[0].RowIndex].Value.ToString();

                _frmCargoZip.Close();
            });






            _frmCargoZip.Owner = this;
            _frmCargoZip.StartPosition = FormStartPosition.CenterParent;
            _frmCargoZip.ShowDialog();
            // txt_AddrDetail.Focus();
        }

        private void btnAddr3_Click(object sender, EventArgs e)
        {
            FrmCargoZip _frmCargoZip = new FrmCargoZip();
            _frmCargoZip.grid1.KeyDown += new KeyEventHandler((object isender, KeyEventArgs ie) =>
            {
                if (ie.KeyCode != Keys.Return) return;
                if (_frmCargoZip.grid1.SelectedCells.Count == 0) return;
                if (_frmCargoZip.grid1.SelectedCells[0].RowIndex < 0) return;


                txtStartAddr1.Text = _frmCargoZip.grid1[1, _frmCargoZip.grid1.SelectedCells[0].RowIndex].Value.ToString();
                txtStartZip1.Text = _frmCargoZip.grid1[0, _frmCargoZip.grid1.SelectedCells[0].RowIndex].Value.ToString();

            

                _frmCargoZip.Close();
            });
            _frmCargoZip.grid1.MouseDoubleClick += new MouseEventHandler((object isender, MouseEventArgs ie) =>
            {
                if (_frmCargoZip.grid1.SelectedCells.Count == 0) return;
                if (_frmCargoZip.grid1.SelectedCells[0].RowIndex < 0) return;
                txtStartAddr1.Text = _frmCargoZip.grid1[1, _frmCargoZip.grid1.SelectedCells[0].RowIndex].Value.ToString();
                txtStartZip1.Text = _frmCargoZip.grid1[0, _frmCargoZip.grid1.SelectedCells[0].RowIndex].Value.ToString();

                _frmCargoZip.Close();
            });






            _frmCargoZip.Owner = this;
            _frmCargoZip.StartPosition = FormStartPosition.CenterParent;
            _frmCargoZip.ShowDialog();
           
        }

        private void btnAddr4_Click(object sender, EventArgs e)
        {
            FrmCargoZip _frmCargoZip = new FrmCargoZip();
            _frmCargoZip.grid1.KeyDown += new KeyEventHandler((object isender, KeyEventArgs ie) =>
            {
                if (ie.KeyCode != Keys.Return) return;
                if (_frmCargoZip.grid1.SelectedCells.Count == 0) return;
                if (_frmCargoZip.grid1.SelectedCells[0].RowIndex < 0) return;


                txtEndAddr1.Text = _frmCargoZip.grid1[1, _frmCargoZip.grid1.SelectedCells[0].RowIndex].Value.ToString();
                txtEndZip1.Text = _frmCargoZip.grid1[0, _frmCargoZip.grid1.SelectedCells[0].RowIndex].Value.ToString();

              
                _frmCargoZip.Close();
            });
            _frmCargoZip.grid1.MouseDoubleClick += new MouseEventHandler((object isender, MouseEventArgs ie) =>
            {
                if (_frmCargoZip.grid1.SelectedCells.Count == 0) return;
                if (_frmCargoZip.grid1.SelectedCells[0].RowIndex < 0) return;
                txtEndAddr1.Text = _frmCargoZip.grid1[1, _frmCargoZip.grid1.SelectedCells[0].RowIndex].Value.ToString();
                txtEndZip1.Text = _frmCargoZip.grid1[0, _frmCargoZip.grid1.SelectedCells[0].RowIndex].Value.ToString();

                _frmCargoZip.Close();
            });






            _frmCargoZip.Owner = this;
            _frmCargoZip.StartPosition = FormStartPosition.CenterParent;
            _frmCargoZip.ShowDialog();
          
        }

     

        private void btn_Inew_Click(object sender, EventArgs e)
        {
            dtpStart.Value = DateTime.Parse(DateTime.Now.ToString("yyyy") + "-01-01");
            dtpEnd.Value = DateTime.Now;

            cmb_DateSearch.SelectedIndex = 1;
            cmb_Search.SelectedIndex = 1;
            txtSearch.Text = string.Empty;
            cmb_FPISSearch.SelectedIndex = 0;
            cmb_Cont.SelectedIndex = 0;
            txt_FPISSearch.Clear();

            btn_Search_Click(null, null);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            //cmb_SearchDate.SelectedIndex = 1;
            dtpSdateS.Value = DateTime.Parse(DateTime.Now.ToString("yyyy") + "-01-01");
            dtpEdateS.Value = DateTime.Now;


            //dtpSdateS.Value = DateTime.Now.AddMonths(-1);
            //dtpEdateS.Value = DateTime.Now;

            cmb_TRU_Search.SelectedIndex = 0;

            txtSearch2.Text = string.Empty;

            btn_TRU_Search_Click(null, null);
        }

       

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            err.Clear();

            if (tabControl1.SelectedIndex == 0)
            {



                if (cmb_CargoGubun.SelectedIndex == 0)
                {
                    //if (txt_CL_COMP_NM.Text == "")
                    //{
                    //    MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                    //    err.SetError(txt_CL_COMP_NM, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                    //    return;

                    //}

                    if (txt_CL_COMP_NM.Text == "")
                    {
                        MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                        err.SetError(txt_CL_COMP_NM, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                        return;

                    }


                    if (!String.IsNullOrEmpty(txt_CL_COMP_BSNS_NUM.Text))
                    {

                        if (txt_CL_COMP_BSNS_NUM.Text.Length != 12 || txt_CL_COMP_BSNS_NUM.Text.Contains(" "))
                        {
                            MessageBox.Show("사업자 번호가 완전하지 않습니다.", "사업자 번호 불완전");
                            err.SetError(txt_CL_COMP_BSNS_NUM, "사업자 번호가 완전하지 않습니다.");

                            return;
                        }
                      
                        txt_CL_COMP_BSNS_NUM.Mask = "999-99-99999";

                    }
                }

                else
                {
                    if (txt_CL_COMP_NM.Text == "")
                    {
                        MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                        err.SetError(txt_CL_COMP_NM, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                        return;

                    }

                    if (!String.IsNullOrEmpty(txt_CL_COMP_BSNS_NUM.Text))
                    {

                        if (txt_CL_COMP_BSNS_NUM.Text.Length != 12 || txt_CL_COMP_BSNS_NUM.Text.Contains(" "))
                        {
                            MessageBox.Show("사업자 번호가 완전하지 않습니다.", "사업자 번호 불완전");
                            err.SetError(txt_CL_COMP_BSNS_NUM, "사업자 번호가 완전하지 않습니다.");

                            return;
                        }
                   
                        txt_CL_COMP_BSNS_NUM.Mask = "999-99-99999";

                    }
                }





                if (dtp_CONT_TO.Value.Date < dtp_CONT_FROM.Value.Date)
                {
                    MessageBox.Show("계약종료일은 계약시작일 이후로 설정하셔야 합니다.");
                    err.SetError(dtp_CONT_TO, "계약종료일은 계약시작일 이후로 설정하셔야 합니다.");
                    return;
                }

                
                if (txt_CONT_DEPOSIT.Text == "")
                {
                    MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                    err.SetError(txt_CONT_DEPOSIT, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                    return;

                }





                UP_Status = "Update";
                G_Gubun = "CONT";

            }
            else
            {


                if (txt_TRU_COMP_BSNS_NUM.Text == "")
                {
                    MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                    err.SetError(txt_TRU_COMP_BSNS_NUM, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                    return;
                }
                else if (txt_TRU_COMP_BSNS_NUM.Text.Replace(" ", "").Length != 12 || txt_TRU_COMP_BSNS_NUM.Text.Contains(" "))
                {
                    MessageBox.Show("사업자 번호가 완전하지 않습니다.", "사업자 번호 불완전");
                    err.SetError(txt_TRU_COMP_BSNS_NUM, "사업자 번호가 완전하지 않습니다.");
                    return;
                }
               



                var Query = cMDataSet.FPIS_CONT.Where(c => c.FPIS_ID == NFPISId.Value);
                DateTime dContFrom = DateTime.Parse(Query.First().CONT_FROM);

                if (dtp_TRU_CONT_FROM.Value.Date < dContFrom)
                {
                    MessageBox.Show("계약시작일은 위탁받은날 이후로 설정하셔야 합니다.");
                    err.SetError(dtp_TRU_CONT_TO, "계약시작일은 위탁받은날 이후로 설정하셔야 합니다.");
                    return ;
                }



                if (dtp_TRU_CONT_TO.Value.Date < dtp_TRU_CONT_FROM.Value.Date)
                {
                    MessageBox.Show("계약종료일은 계약시작일 이후로 설정하셔야 합니다.");
                    err.SetError(dtp_TRU_CONT_TO, "계약종료일은 계약시작일 이후로 설정하셔야 합니다.");
                    return;
                }


                if (txt_TRU_DEPOSIT.Text == "")
                {
                    MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                    err.SetError(txt_TRU_DEPOSIT, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                    return;
                }
              

                UP_Status = "Update";
                G_Gubun = "TRU";



            }

            int _rows = UpdateDB(UP_Status, G_Gubun);
            
        }

        private int UpdateDB(string Status,string Gubun)
        {
            int _rows = 0;
            switch (Gubun)
            {
                case "CONT":
                    try
                    {



                        fPISCONTBindingSource.EndEdit();
                        //_rows = SingleDataSet.Instance.TB_FPIS_MODULE.Where(c => c.RowState != DataRowState.Unchanged).Count();

                      

                         

                        fPIS_CONTTableAdapter.Update(cMDataSet);

                        if (Status == "Update")
                        {
                            MessageBox.Show(ButtonMessage.GetMessage(MessageType.수정성공, "의뢰자/계약", 1), "의뢰자/계약정보 수정 성공");
                            if (dataGridView1.RowCount > 1)
                            {
                                GridIndex = fPISCONTBindingSource.Position;
                                btn_Search_Click(null, null);
                                dataGridView1.CurrentCell = dataGridView1.Rows[GridIndex].Cells[0];

                            }
                            else
                            {
                                btn_Search_Click(null, null);
                            }


                            
                        }
                        else if (Status == "Delete")
                        {


                            MessageBox.Show(ButtonMessage.GetMessage(MessageType.삭제성공, "의뢰자/계약", 1), "의뢰자/계약정보 삭제 성공");
                            btn_Search_Click(null, null);
                        }

                        btn_CL_Search_Click(null, null);
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.Message, "의뢰자/계약정보 변경 실패", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        _rows = -1;
                    }

                    break;

                case "TRU":
                    try
                    {
                        fPISTRUBindingSource.EndEdit();

                        fPIS_TRUTableAdapter.Update(cMDataSet);

                        if (Status == "Update")
                        {
                            MessageBox.Show(ButtonMessage.GetMessage(MessageType.수정성공, "위탁", 1), "위탁정보 수정 성공");


                            if (dataGridView1.RowCount > 1)
                            {
                                GridIndex = fPISTRUBindingSource.Position;
                                btn_TRU_Search_Click(null, null);
                                grid3.CurrentCell = grid3.Rows[GridIndex].Cells[0];

                            }
                            else
                            {
                                btn_Search_Click(null, null);
                            }
                        }
                        else if (Status == "Delete")
                        {
                            MessageBox.Show(ButtonMessage.GetMessage(MessageType.삭제성공, "위탁", 1), "위탁정보 삭제 성공");
                            btn_TRU_Search_Click(null, null);
                        }
                       

                    }

                    catch (Exception e)
                    {
                        MessageBox.Show(e.Message, "위탁정보 변경 실패", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        _rows = -1;
                    }
                    break;

              
            }
         //   fPISINFOBindingSource_CurrentChanged(null, null);
            return _rows;
           
        }

        private void _Search(string Gubun)
        {
            string _FilterString = string.Empty;
            string _FilterString2 = string.Empty;
            string _cmbSearchString = string.Empty;

            string _DateSearchString = string.Empty;
            string _DateSearchString2 = string.Empty;

            string _FPISSearchString = string.Empty;
            string _ClientSearchString = string.Empty;
            string _ContSearchString = string.Empty;
            string _FpisSearch = string.Empty;

            string _AllSearchString = string.Empty;
            //  this.fPIS_CONTTableAdapter.Fill(this.cMDataSet.FPIS_CONT);

            this.clientUsersTableAdapter.FillForAdmin(this.cMDataSet.ClientUsers);

            #region cont
            if (Gubun == "CONT")
            {

                try
                {
                    //this.fPIS_CONTTableAdapter.Fill(this.cMDataSet.FPIS_CONT);
                    this.fPIS_CONTTableAdapter.Fill(this.cMDataSet.FPIS_CONT);

                   
                 
                    if (cmb_Search.Text == "상호/성명")
                    {
                        var codes = cMDataSet.FPIS_CONT.Where(c => c.CL_COMP_NM.Contains(txtSearch.Text));
                        if (codes.Count() > 0)
                        {
                            string filter = string.Format("CL_COMP_NM Like  '%{0}%'", txtSearch.Text);
                            _cmbSearchString = filter;

                        }
                        else
                            _cmbSearchString = "";
                    }

                    else if (cmb_Search.Text == "사업자등록번호")
                    {

                        string filter = string.Format("ISNULL(BizSearch,'') Like  '%{0}%'", txtSearch.Text);
                        _cmbSearchString = filter;


                    }
                    //else if (cmb_Search.Text == "운송정보(품목)")
                    //{
                    //    var codes = cMDataSet.FPISOptions.Where(c => c.Div == "CargoItem" && c.Name.Contains(txtSearch.Text)).Select(c => c.Value).ToArray();
                    //    if (codes.Count() > 0)
                    //    {
                    //        string filter = String.Format("CONT_ITEM IN ('{0}'", codes[0]);
                    //        for (int i = 1; i < codes.Count(); i++)
                    //        {
                    //            filter += string.Format(", '{0}'", codes[i]);
                    //        }
                    //        filter += ")";
                    //        _cmbSearchString = filter;
                    //    }
                    //    else
                    //        _cmbSearchString = "";
                    //}
                    //else if (cmb_Search.Text == "출발지(코드)")
                    //{

                    //    string filter = string.Format("CONT_START_ADDR Like  '%{0}%' or CONT_START_ADDR1 LIKE'%{0}%'", txtSearch.Text);
                    //    _cmbSearchString = filter;


                    //}
                    //else if (cmb_Search.Text == "도착지(코드)")
                    //{

                    //    string filter = string.Format("CONT_END_ADDR Like  '%{0}%' or CONT_END_ADDR1 LIKE'%{0}%'", txtSearch.Text);
                    //    _cmbSearchString = filter;

                    //}

                    _FilterString += _cmbSearchString;


                   if(cmb_DateSearch.SelectedIndex == 0)
                   {
                       _DateSearchString = "CREATE_DATE1 >= '" + dtpStart.Text + "'  AND CREATE_DATE1 <= '" + dtpEnd.Text + "'";


                   }
                   else
                   {
                       _DateSearchString = "CONT_FROM >= '" + dtpStart.Text + "'  AND CONT_FROM <= '" + dtpEnd.Text + "'";
                   }


                       

                   




                    if (_FilterString != string.Empty && _DateSearchString != string.Empty)
                    {
                        _FilterString += " AND " + _DateSearchString;
                    }
                    else
                    {
                        _FilterString += _DateSearchString;
                    }

                    //var Selected = ((DataRowView)dataGridView1.Rows[e.RowIndex].DataBoundItem).Row as CMDataSet.FPIS_CONTRow;

                    //if (Selected.ONE_GUBUN == "1")
                    //{
                    //    dataGridView1[e.ColumnIndex, e.RowIndex].Value = "단일";
                    //}
                    //else
                    //{
                    //    if (Selected.CONT_YN == false)
                    //    {
                    //        dataGridView1[e.ColumnIndex, e.RowIndex].Value = "1차";
                    //    }
                    //    else
                    //    {
                    //        dataGridView1[e.ColumnIndex, e.RowIndex].Value = "2차";
                    //    }
                    //}

                    if (cmb_Cont.Text == "단일")
                    {
                      
                            string filter = string.Format("ONE_GUBUN =  '{0}'", "1");
                            _ContSearchString = filter;

                        
                    }
                    else if (cmb_Cont.Text == "1차")
                    {

                        string filter = string.Format("CONT_YN =  '{0}' AND ONE_GUBUN = 0 ", false);
                        _ContSearchString = filter;


                    }
                    else if (cmb_Cont.Text == "2차")
                    {

                        string filter = string.Format("CONT_YN =  '{0}' AND ONE_GUBUN = 0", true);
                        _ContSearchString = filter;


                    }


                    if (_FilterString != string.Empty && _ContSearchString != string.Empty)
                    {
                        _FilterString += " AND " + _ContSearchString;
                    }
                    else
                    {
                        _FilterString += _ContSearchString;
                    }



                    if (cmb_FPISSearch.SelectedIndex != 0 && !String.IsNullOrEmpty(txt_FPISSearch.Text))
                    {
                        List<int> VisibleOrderIds = new List<int>();
                        foreach (var item in cMDataSet.FPIS_CONT)
                        {
                            if (cmb_FPISSearch.SelectedIndex == 1 && !String.IsNullOrEmpty(txt_FPISSearch.Text))
                            {
                              
                                var iName = "";

                                var Query = _ClientTable.Where(c => c.LoginId == item.CliendId);

                               
                                if (Query.Any())
                                {
                                    iName = Query.First().Code;
                                    if (iName.Contains(txt_FPISSearch.Text))
                                        VisibleOrderIds.Add(item.FPIS_ID);
                                }

                                var Query2 = cMDataSet.ClientUsers.Where(c => c.LoginId == item.CliendId);

                                if (Query2.Any())
                                {
                                    var Query3 = _ClientTable.Where(c => c.ClientId == Query2.First().ClientId);

                                    if (Query3.Any())
                                    {

                                        iName = Query3.First().Code;
                                        if (iName.Contains(txt_FPISSearch.Text))
                                            VisibleOrderIds.Add(item.FPIS_ID);
                                    }

                                }
                            

                            }
                            else if (cmb_FPISSearch.SelectedIndex == 2 && !String.IsNullOrEmpty(txt_FPISSearch.Text))
                            {
                                var iName = "";

                                var Query = _ClientTable.Where(c => c.LoginId == item.CliendId);
                                if (Query.Any())
                                {
                                    iName = Query.First().Name;
                                    if (iName.Contains(txt_FPISSearch.Text))
                                        VisibleOrderIds.Add(item.FPIS_ID);
                                }


                                var Query2 = cMDataSet.ClientUsers.Where(c => c.LoginId == item.CliendId);

                                if (Query2.Any())
                                {
                                    var Query3 = _ClientTable.Where(c => c.ClientId == Query2.First().ClientId);

                                    if (Query3.Any())
                                    {

                                        iName = Query3.First().Name;
                                        if (iName.Contains(txt_FPISSearch.Text))
                                            VisibleOrderIds.Add(item.FPIS_ID);
                                    }

                                }


                            }
                        }
                        if (VisibleOrderIds.Count == 0)
                        {
                            _FPISSearchString = "FPIS_ID = -1";
                        }
                        else
                        {
                            _FPISSearchString = "FPIS_ID IN (" + String.Join(",", VisibleOrderIds) + ")";
                        }
                    }
                    else
                    {
                        fPISCONTBindingSource.Filter = "";
                    }

                    if (_FilterString != string.Empty && _FPISSearchString != string.Empty)
                    {
                        _FilterString += " AND " + _FPISSearchString;
                    }
                    else
                    {
                        _FilterString += _FPISSearchString;
                    }
                   
                    if (!LocalUser.Instance.LogInInformation.IsAdmin)
                    {
                        List<string> VisibleOrderIds = new List<string>();

                        if (LocalUser.Instance.LogInInformation.ClientUserId > 0)
                        {
                          
                            var Clientid = _ClientTable.Where(c => c.ClientId == LocalUser.Instance.LogInInformation.ClientId);
                            
                                var Query = cMDataSet.ClientUsers.Where(c => c.ClientId == Clientid.First().ClientId);
                                if (Query.Any())
                                {

                                    foreach (var item in Query)
                                    {
                                        VisibleOrderIds.Add("'"+item.LoginId+"'");
                                    }


                                }


                            if (_FilterString != string.Empty)
                            {
                                if (VisibleOrderIds.Count == 0)
                                {
                                    _FilterString += " AND Cliendid = '" + Clientid.First().LoginId + "'";
                                }

                                else
                                {
                                    _FilterString += " AND (Cliendid = '" + Clientid.First().LoginId + "'" + " or Cliendid IN (" + String.Join(",", VisibleOrderIds) + "))";
                                }
                            }
                            else
                            {
                                if (VisibleOrderIds.Count == 0)
                                {
                                    _FilterString += " Cliendid = '" + Clientid.First().LoginId + "'";
                                }

                                else
                                {
                                    _FilterString += " (Cliendid = '" + Clientid.First().LoginId + "'" + " or Cliendid IN (" + String.Join(",", VisibleOrderIds) + "))"; 

                                }
                            }
                           
                        }
                        else
                        {
                            var Clientid = _ClientTable.Where(c => c.ClientId == LocalUser.Instance.LogInInformation.ClientId);

                            var Query = cMDataSet.ClientUsers.Where(c => c.ClientId == Clientid.First().ClientId);
                            if (Query.Any())
                            {

                                foreach (var item in Query)
                                {
                                    VisibleOrderIds.Add("'" + item.LoginId + "'");
                                }


                            }


                            if (_FilterString != string.Empty)
                            {
                                if (VisibleOrderIds.Count == 0)
                                {
                                    _FilterString += " AND Cliendid = '" + LocalUser.Instance.LogInInformation.LoginId + "'";
                                }
                                else
                                {
                                    _FilterString += " AND (Cliendid = '" + LocalUser.Instance.LogInInformation.LoginId + "'" + " or Cliendid IN (" + String.Join(",", VisibleOrderIds) + "))";
                                }
                            }
                            else
                            {

                                if (VisibleOrderIds.Count == 0)
                                {
                                    _FilterString += " Cliendid = '" + LocalUser.Instance.LogInInformation.LoginId + "'";
                                }

                                else
                                {
                                    _FilterString += " (Cliendid = '" + LocalUser.Instance.LogInInformation.LoginId + "'" + " or Cliendid IN (" + String.Join(",", VisibleOrderIds) + "))";
                                }
                            }

                        }
                    }
                    
                    try
                    {

                        fPISCONTBindingSource.Filter = _FilterString;

                    }
                    catch
                    {
                        btn_Search_Click(null, null);
                    }
                }
                catch(Exception e)
                {
                }

            }
            #endregion
            else if (Gubun == "TRU")
            {
                getFilterString();
                _FilterString = string.Empty;
                _FilterString2 = string.Empty;
                _cmbSearchString = string.Empty;

                _DateSearchString = string.Empty;
               

            //    this.fPIS_TRUTableAdapter.Fill(this.cMDataSet.FPIS_TRU);
             //   this.fPIS_TRU_EXCELTableAdapter.Fill(this.cMDataSet.FPIS_TRU_EXCEL);
                this.fPIS_TRUTableAdapter.Fill(this.cMDataSet.FPIS_TRU);
               

                try
                {

                    if (cmb_TRU_Search.Text == "상호")
                    {
                      
                            string filter = string.Format("TRU_COMP_NM Like  '%{0}%'", txtSearch2.Text);
                            _cmbSearchString = filter;

                      
                    }

                    else if (cmb_TRU_Search.Text == "사업자번호")
                    {
                       
                            string filter = string.Format("TRU_COMP_BSNS_NUM Like  '%{0}%'", txtSearch2.Text);
                            _cmbSearchString = filter;

                     



                    }
                 
                    else if (cmb_TRU_Search.Text == "출발지(코드)")
                    {

                        string filter = string.Format("TRU_START_ADDR Like  '%{0}%' or TRU_START_ADDR1 LIKE'%{0}%'", txtSearch2.Text);
                        _cmbSearchString = filter;


                    }
                    else if (cmb_TRU_Search.Text == "도착지(코드)")
                    {

                        string filter = string.Format("TRU_END_ADDR Like  '%{0}%' or TRU_END_ADDR1 LIKE'%{0}%'", txtSearch2.Text);
                        _cmbSearchString = filter;

                    }
                    else if (cmb_TRU_Search.Text == "위탁구분")
                    {
                        string filter = string.Format("CONT_GUBUN Like  '%{0}%' ", txtSearch2.Text);
                        _cmbSearchString = filter;
                    }

                    _FilterString += _cmbSearchString;


                 
                    try
                    {


                        _DateSearchString = "TRU_CONT_FROM >= '" + dtpSdateS.Text + "'  AND TRU_CONT_FROM <= '" + dtpEdateS.Text + "'";
                        _DateSearchString2 = "TRU_CONT_FROM1 >= '" + dtpSdateS.Text + "'  AND TRU_CONT_FROM1 <= '" + dtpEdateS.Text + "'";

                    }
                    catch
                    {
                        _DateSearchString = "";
                        _DateSearchString2 = "";
                    }


                    if (_FilterString != string.Empty && _DateSearchString != string.Empty)
                    {
                        _FilterString += " AND " + _DateSearchString;
                    }
                    else
                    {
                        _FilterString += _DateSearchString;
                    }


                    if (cmb_Fpis.SelectedIndex == 1)
                    {
                        _FpisSearch = String.Format(" ISNULL(FPIS_F_DATE,'9999-12-31 00:00:00.000') ='{0}' ", "9999-12-31 00:00:00.000");
                    }
                    else if (cmb_Fpis.SelectedIndex == 2)
                    {
                        _FpisSearch = String.Format(" ISNULL(FPIS_F_DATE,'9999-12-31 00:00:00.000') <> '{0}' ", "9999-12-31 00:00:00.000");
                    }
                    else if (cmb_Fpis.SelectedIndex == 3)
                    {
                        _FpisSearch = "";
                    }
                    else
                    {
                        _FpisSearch = "";
                    }

                    if (_FilterString != string.Empty && _FpisSearch != string.Empty)
                    {
                        _FilterString += " AND " + _FpisSearch;
                    }
                    else
                    {
                        _FilterString += _FpisSearch;
                    }

                        if (_FilterString != string.Empty)
                        {
                            //_FilterString += " AND " + "FPIS_ID =" + NFPISId.Value + "";

                            if (checkedCodes.Count > 0)
                            {
                                _FilterString += " AND " + "FPIS_ID in(" + String.Join(",", checkedCodes) + ") ";
                            }
                            else
                            {
                                _FilterString += " AND " + "FPIS_ID = -1 ";
                            }
                        }
                        else
                        {
                            _FilterString += _DateSearchString;
                        }

                        //#region 차량

                        //List<string> VisibleOrderIds = new List<string>();

                        //this.driverGroupsTableAdapter.Fill(this.cMDataSet.DriverGroups);
                        //this.driversTableAdapter.Fill(this.cMDataSet.Drivers);


                        //var DriverGroupsCnt = cMDataSet.DriverGroups.Where(c => c.ClientId == LocalUser.Instance.LogInInfomation.ClientId).ToArray();
                        //var DriverCnt = cMDataSet.Drivers.Where(c => c.CandidateId == LocalUser.Instance.LogInInfomation.ClientId).ToArray();

                        //if (DriverGroupsCnt.Any())
                        //{
                        //    var Query = cMDataSet.DriverGroups.Where(c => c.ClientId == LocalUser.Instance.LogInInfomation.ClientId && c.Cont == false).ToArray();

                        //    foreach (var item in Query)
                        //    {
                        //        VisibleOrderIds.Add("'" + item.DriverId + "'");
                        //    }




                        //}
                        //if (DriverCnt.Any())
                        //{
                        //    var Query = cMDataSet.Drivers.Where(c => c.CandidateId == LocalUser.Instance.LogInInfomation.ClientId && c.Car_ContRact == false).ToArray();

                        //    foreach (var item in Query)
                        //    {
                        //        VisibleOrderIds.Add("'" + item.DriverId + "'");
                        //        VisibleOrderIds.Add("0");
                        //    }

                        //}


                        //if (VisibleOrderIds.Any())
                        //{
                        //    string filter = "DriverId  in  (" + String.Join(",", VisibleOrderIds) + ")";


                        //    _AllSearchString = filter;
                        //}
                        //#endregion

                        //if (_FilterString != string.Empty && _AllSearchString != string.Empty)
                        //{
                        //    _FilterString += " AND " + _AllSearchString;
                        //}
                        //else
                        //{
                        //    _FilterString += _AllSearchString;
                        //}

                        try
                        {

                            fPISTRUBindingSource.Filter = _FilterString;


                            //if (LocalUser.Instance.LogInInfomation.ClientUserId > 0)
                            //{
                            //    var Clientid = SingleDataSet.Instance.Clients.Where(c => c.ClientId == LocalUser.Instance.LogInInfomation.ClientId);


                            //    fPISTRUEXCELBindingSource.Filter = _DateSearchString2 + "AND CliendId = '" + Clientid.First().LoginId + "'";



                            //}
                            //else
                            //{
                            //    fPISTRUEXCELBindingSource.Filter = _DateSearchString2 + "AND CliendId = '" + LocalUser.Instance.LogInInfomation.UserID + "'";

                            //}



                        }
                        catch
                        {
                            btn_TRU_Search_Click(null, null);
                        }
                }
                catch
                {
                }

            }
            



        }
      


        private void btn_Search_Click(object sender, EventArgs e)
        {
            G_Gubun = "CONT";
            _Search(G_Gubun);
        }

        private void btnCurrentDelete_Click(object sender, EventArgs e)
        {
            List<DataGridViewRow> deleteRows = new List<DataGridViewRow>();

            if (tabControl1.SelectedIndex == 0)
            {
                if (dataGridView1.SelectedCells.Count > 0)
                {
                    foreach (DataGridViewCell item in dataGridView1.SelectedCells)
                    {
                        if (item.RowIndex != -1 && deleteRows.Contains(item.OwningRow) == false) deleteRows.Add(item.OwningRow);
                    }

                    int FPISIDcount = 0;
                    foreach (DataGridViewRow dRow in deleteRows)
                    {

                        FPISIDcount += (int)fPIS_CONTTableAdapter.GetBYFPISID(int.Parse(dRow.Cells["FPIS_ID"].Value.ToString()));

                    }
                    if (FPISIDcount > 0)
                    {
                        MessageBox.Show(string.Format("사용중인 데이터 {0}건이  존재하므로\n이 의뢰자/계약정보는 삭제할 수 없습니다.",
                            FPISIDcount), "의뢰자/계약정보 삭제 실패");
                        return;
                    }



                    if (MessageBox.Show(ButtonMessage.GetMessage(MessageType.삭제질문, "의뢰자/계약", deleteRows.Count), "선택항목 삭제 여부 확인", MessageBoxButtons.YesNo) != DialogResult.Yes) return;
                    foreach (DataGridViewRow row in deleteRows)
                    {
                        dataGridView1.Rows.Remove(row);
                    }
                }

                G_Gubun = "CONT";

            }
            else
            {

                if (grid3.SelectedCells.Count > 0)
                {
                    foreach (DataGridViewCell item in grid3.SelectedCells)
                    {
                        if (item.RowIndex != -1 && deleteRows.Contains(item.OwningRow) == false) deleteRows.Add(item.OwningRow);
                    }


                    if (MessageBox.Show(ButtonMessage.GetMessage(MessageType.삭제질문, "위탁", deleteRows.Count), "선택항목 삭제 여부 확인", MessageBoxButtons.YesNo) != DialogResult.Yes) return;
                    foreach (DataGridViewRow row in deleteRows)
                    {
                        grid3.Rows.Remove(row);
                    }
                }

                G_Gubun = "TRU";


            }
            var UP_Status = "Delete";
            int _rows = UpdateDB(UP_Status,G_Gubun);
        }

        private void btn_CL_Search_Click(object sender, EventArgs e)
        {
           
            this.fPISINFO1TableAdapter.Fill(this.cMDataSet.FPISINFO1, LocalUser.Instance.LogInInformation.ClientId);

            DateTime Sdate = DateTime.Parse(DateTime.Now.ToString("yyyy") + "-01-01");
            DateTime Edate = DateTime.Now;

            try
            {
                if (String.IsNullOrEmpty(txt_Fsearch.Text))
                {

                    fPISINFO1BindingSource.Filter = " CONT_FROM >= '" + dtp_I_Start.Text + "'  AND CONT_FROM <= '" + dtp_I_End.Text + "' AND CONT_YN = false "; 
                }
                else
                {

                    fPISINFO1BindingSource.Filter = string.Format("CL_COMP_NM Like  '%{0}%' AND CONT_YN = {1} ", txt_Fsearch.Text, false) + " AND CONT_FROM >= '" + dtp_I_Start.Text + "'  AND CONT_FROM <= '" + dtp_I_End.Text+ "'";
                   
                }

            }
            catch
            {
            }
        }

        private void btn_TRU_Search_Click(object sender, EventArgs e)
        {
            G_Gubun = "TRU";
            _Search(G_Gubun);

            
        }

        private void fPISTRUBindingSource_CurrentChanged(object sender, EventArgs e)
        {
            err.Clear();

            try
            {
                if (tabControl1.SelectedIndex == 1)
                {
                    // if (fPISCONTBindingSource == null) return;
                    if (fPISTRUBindingSource.Current == null) return;

                    fPISTRUBindingSource_CurrentChanging = true;

                    var Selected = ((DataRowView)fPISTRUBindingSource.Current).Row as CMDataSet.FPIS_TRURow;


                    if (Selected != null)
                    {


                        //if (Selected.CONT_GUBUN == "차량위탁")
                        //{
                        //    btnUpdate.Enabled = false;
                        //    btnCurrentDelete.Enabled = false;
                        //    panel16.Enabled = false;

                        //}
                        //else
                        //{
                          
                        //    //if (Selected.FPIS_F_DATE1 == "")
                        //    //{
                        //    //    btnUpdate.Enabled = false;
                        //    //    btnCurrentDelete.Enabled = false;
                        //    //    panel16.Enabled = false;
                        //    //}
                        //    //else
                        //    //{
                        //    //    btnUpdate.Enabled = true;
                        //    //    btnCurrentDelete.Enabled = true;
                        //    //    panel16.Enabled = true;

                        //    //}

                        //    btnUpdate.Enabled = true;
                        //    btnCurrentDelete.Enabled = true;
                        //    panel16.Enabled = true;
                        //}

                    }                //DataRowView view = fPISTRUBindingSource.Current as DataRowView;

                    //if (view == null) return;
                    //CMDataSet.FPIS_TRURow row = view.Row as CMDataSet.FPIS_TRURow;
                    //if (row == null) return;

                    //var SizeName = cMDataSet.FPISOptions.Where(c => c.Value == row.TRU_GOODS_UNIT && c.Div == "CargoSize").Select(c => new { c.Name }).ToArray();

                    //label81.Text = SizeName.First().Name;
                }
                

            }



            catch { }

          //  FPIS_Graph();

        }

        //private void btn_CarSearch_Click(object sender, EventArgs e)
        //{
        //    G_Gubun = "CAR";
        //    _Search(G_Gubun);
        //}

      

        private void fPISCARBindingSource_CurrentChanged(object sender, EventArgs e)
        {
            err.Clear();

            //try
            //{
            //    if (fPISCARBindingSource == null) return;
            //    if (fPISCARBindingSource.Current == null) return;
            //    DataRowView view = fPISCARBindingSource.Current as DataRowView;

            //    if (view == null) return;
            //    CMDataSet.FPIS_CARRow row = view.Row as CMDataSet.FPIS_CARRow;
            //    if (row == null) return;

            //    var SizeName = cMDataSet.FPISOptions.Where(c => c.Value == row.CAR_GOODS_UNIT && c.Div == "CargoSize").Select(c => new { c.Name }).ToArray();

            //    label85.Text = SizeName.First().Name;
            //}
            //catch { }
        }

        private void tabControl1_TabIndexChanged(object sender, EventArgs e)
        {
          
            //    fPISTRUBindingSource_CurrentChanged(null, null);
            
            
        }

        private void tabControl2_SelectedIndexChanged(object sender, EventArgs e)
        {

          
           
        }
        string I_CONT_GOODS_CNT, I_CONT_DEPOSIT, I_TRU_GOODS_CNT, I_TRU_DEPOSIT, I_CAR_GOODS_CNT, I_CAR_CHARGE, I_ORDER_GOODS_CNT, I_ORDER_CHARGE;
        private void fPISINFOBindingSource_CurrentChanged(object sender, EventArgs e)
        {
            //err.Clear();


            if (fPISINFO1BindingSource.Current == null)
            {
                btn_TRU_Search_Click(null, null);
                return;
            }

            fPISINFO1BindingSource_CurrentChanging = true;

            var Selected = ((DataRowView)fPISINFO1BindingSource.Current).Row as CMDataSet.FPISINFO1Row;


            if (Selected != null)
            {
                int intchange;


                NFPISId.Value = Selected.FPIS_ID;


                for (int i = 0; i < dataGridView2.RowCount; i++)
                {

                    dataGridView2[0, i].Value = false; // 6은 CheckBox가 있는 열 번호
                    chkAllSelect.Checked = false;

                }

                // 현재 선택된 행의 CheckBox값을 True로 설정

                for (int i = 0; i < dataGridView2.SelectedRows.Count; i++)
                {

                    //Convert.ToInt32 => string을 int로 변환

                    intchange = Convert.ToInt32(dataGridView2.SelectedRows[i].Index);

                    dataGridView2[0, intchange].Value = true;
                    chkAllSelect.Checked = false;

                }


               
          
               btn_TRU_Search_Click(null, null);

              

                   
               


                var SizeName = cMDataSet.FPISOptions.Where(c => c.Value == Selected.CONT_GOODS_UNIT && c.Div == "CargoSize").Select(c => new { c.Name }).ToArray();
                label81.Text = SizeName.First().Name;
               // label85.Text = SizeName.First().Name;

             



            }



            fPISINFO1BindingSource_CurrentChanging = false;
        }
        string FPIS_ID_IN;
      
        

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 0)
            {
                
              
               
                    //btnUpdate.Enabled = true;
                    //btnCurrentDelete.Enabled = true;
                 
               


                btn_Search_Click(null, null);


            }
            else
            {
                //if (cbo_CONT_YN.Checked == false)
                //{
                //    btn_CL_Search_Click(null, null);
                //}

                btn_CL_Search_Click(null, null);

                dataGridView2_CellClick(null, null);
                btn_TRU_Search_Click(null, null);


               


            }
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {

            //var Selected = ((DataRowView)dataGridView1.Rows[e.RowIndex].DataBoundItem).Row as CMDataSet.OrdersRow;
            //dataGridView1[e.ColumnIndex, e.RowIndex].Value = Selected.StartTime.ToString("d").Replace("-", "/");
            //운송사코드
            if (e.ColumnIndex == 0 && e.RowIndex >= 0)
            {
                dataGridView1[e.ColumnIndex, e.RowIndex].Value = (dataGridView1.Rows.Count - e.RowIndex).ToString("N0");
            }

            else if(dataGridView1.Columns[e.ColumnIndex] == cONTFROMDataGridViewTextBoxColumn)
            {

                e.Value = e.Value.ToString().Replace("-", "/");
            }
            else if (dataGridView1.Columns[e.ColumnIndex] == Column2)
            {
                var Selected = ((DataRowView)dataGridView1.Rows[e.RowIndex].DataBoundItem).Row as CMDataSet.FPIS_CONTRow;

                var Query = _ClientTable.Where(c => c.LoginId == Selected.CliendId);



                if (Query.Any())
                {
                    dataGridView1[e.ColumnIndex, e.RowIndex].Value = Query.First().Code;
                }
                else
                {
                    var Query2 = cMDataSet.ClientUsers.Where(c => c.LoginId == Selected.CliendId);

                    if (Query2.Any())
                    {
                        var Query3 = _ClientTable.Where(c => c.ClientId == Query2.First().ClientId);

                        if (Query3.Any())
                        {
                            dataGridView1[e.ColumnIndex, e.RowIndex].Value = Query3.First().Code;

                        }
                    }
                }
            }
            //운송사명
            else if (dataGridView1.Columns[e.ColumnIndex] == Column3)
            {
                var Selected = ((DataRowView)dataGridView1.Rows[e.RowIndex].DataBoundItem).Row as CMDataSet.FPIS_CONTRow;
                var Query = _ClientTable.Where(c => c.LoginId == Selected.CliendId);



                if (Query.Any())
                {
                    dataGridView1[e.ColumnIndex, e.RowIndex].Value = Query.First().Name;
                }

                else
                {
                    var Query2 = cMDataSet.ClientUsers.Where(c => c.LoginId == Selected.CliendId);
                    if (Query2.Any())
                    {
                        var Query3 = _ClientTable.Where(c => c.ClientId == Query2.First().ClientId);
                        if (Query3.Any())
                        {
                            dataGridView1[e.ColumnIndex, e.RowIndex].Value = Query3.First().Name;
                        }
                    }
                }
            }

            else if (dataGridView1.Columns[e.ColumnIndex] == CREATE_DATEDataGridViewTextBoxColumn)

            {
                e.Value = ((DateTime)e.Value).ToString("d").Replace("-", "/");
            }

            else if (dataGridView1.Columns[e.ColumnIndex] == CONT_YN)
            {
                var Selected = ((DataRowView)dataGridView1.Rows[e.RowIndex].DataBoundItem).Row as CMDataSet.FPIS_CONTRow;

                if (Selected.ONE_GUBUN == "1")
                {
                    dataGridView1[e.ColumnIndex, e.RowIndex].Value = "단일";
                }
                else
                {
                    if (Selected.CONT_YN == false)
                    {
                        dataGridView1[e.ColumnIndex, e.RowIndex].Value = "1차";
                    }
                    else
                    {
                        dataGridView1[e.ColumnIndex, e.RowIndex].Value = "2차";
                    }
                }

            }
            else if (dataGridView1.Columns[e.ColumnIndex] == cONTGOODSCNTDataGridViewTextBoxColumn)
            {
                if (!String.IsNullOrEmpty(e.Value.ToString()))
                {
                    e.Value = String.Format("{0:#,##0}", Convert.ToInt64(e.Value));
                }

            }
            //CONT_DEPOSIT
            else if (dataGridView1.Columns[e.ColumnIndex] == CONT_DEPOSIT)
            {
                if (!String.IsNullOrEmpty(e.Value.ToString()))
                {
                    e.Value = String.Format("{0:#,##0}", Convert.ToInt64(e.Value));
                }

            }

            else if (dataGridView1.Columns[e.ColumnIndex] == cLCOMPBSNSNUMDataGridViewTextBoxColumn)
            {
                var Selected = ((DataRowView)dataGridView1.Rows[e.RowIndex].DataBoundItem).Row as CMDataSet.FPIS_CONTRow;
                if (Selected.CL_COMP_BSNS_NUM.Length == 10)

                {
                    e.Value = Selected.CL_COMP_BSNS_NUM.Substring(0, 3) + "-" + Selected.CL_COMP_BSNS_NUM.Substring(3, 2) + "-" + Selected.CL_COMP_BSNS_NUM.Substring(5, 5);
                }

            }

            else if (dataGridView1.Columns[e.ColumnIndex] == cLPTELDataGridViewTextBoxColumn)
            {
                //var Selected = ((DataRowView)dataGridView1.Rows[e.RowIndex].DataBoundItem).Row as CMDataSet.FPIS_CONTRow;
                //if (Selected.CL_COMP_BSNS_NUM.Length == 10)

                //{
                //    e.Value = Selected.CL_P_TEL.Substring(0, 3) + "-" + Selected.CL_COMP_BSNS_NUM.Substring(3, 2) + "-" + Selected.CL_COMP_BSNS_NUM.Substring(5, 5);
                //}

            }
            

            else if (dataGridView1.Columns[e.ColumnIndex] == cLCOMPCORPNUMDataGridViewTextBoxColumn)
            {
                var Selected = ((DataRowView)dataGridView1.Rows[e.RowIndex].DataBoundItem).Row as CMDataSet.FPIS_CONTRow;
                var _Customer = cMDataSet.Customers.FirstOrDefault(c => c.CustomerId == Selected.CustomerId && c.ClientId == LocalUser.Instance.LogInInformation.ClientId);


                if (_Customer.ResgisterNo.Length == 13)

                {
                    e.Value = _Customer.ResgisterNo.Substring(0, 6) + "-" + _Customer.ResgisterNo.Substring(7, 7);
                }
                else
                {

                    e.Value = _Customer.ResgisterNo;

                }

            }

            
        }

        private void fPISCONTBindingSource_CurrentChanged(object sender, EventArgs e)
        {
            err.Clear();

            //FPIS_Graph();
            
            if (fPISCONTBindingSource.Current == null)
                return;
          //  ordersBindingSource_CurrentChanging = true;
            var Selected = ((DataRowView)fPISCONTBindingSource.Current).Row as CMDataSet.FPIS_CONTRow;
            if (Selected != null)
            {
                if (Selected.ONE_GUBUN == "1")
                {
                    btnUpdate.Enabled = false;
                }
                else
                {
                    btnUpdate.Enabled = true;
                }

                var _Deposit = Selected.CONT_DEPOSIT;
                decimal _d = 0;
                if(decimal.TryParse(_Deposit, out _d))
                {
                    txt_CONT_DEPOSIT.Text = _d.ToString("N0");
                }
                else
                {
                    txt_CONT_DEPOSIT.Text = "0";
                }
                var _Customer = cMDataSet.Customers.FirstOrDefault(c => c.CustomerId == Selected.CustomerId && c.ClientId == LocalUser.Instance.LogInInformation.ClientId);
                if (_Customer != null)
                    txt_CL_COMP_CORP_NUM.Text = _Customer.ResgisterNo;
            }
        }

        private void txtSearch_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Return) return;
            btn_Search_Click(null, null);
        }

        private void txt_FPISSearch_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Return) return;
            btn_Search_Click(null, null);
        }

        private void dataGridView2_SelectionChanged(object sender, EventArgs e)
        {

          
        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int intchange;

            try
            {

                if (dataGridView2.CurrentCell != null)
                {

                    // 모든 행의 CheckBox값을 False로 설정

                    for (int i = 0; i < dataGridView2.RowCount; i++)
                    {

                        dataGridView2[0, i].Value = false; // 6은 CheckBox가 있는 열 번호
                        chkAllSelect.Checked = false;

                    }

                    // 현재 선택된 행의 CheckBox값을 True로 설정

                    for (int i = 0; i < dataGridView2.SelectedRows.Count; i++)
                    {

                        //Convert.ToInt32 => string을 int로 변환

                        intchange = Convert.ToInt32(dataGridView2.SelectedRows[i].Index);

                        dataGridView2[0, intchange].Value = true;
                        chkAllSelect.Checked = false;

                    }

                  //  btn_TRU_Search_Click(null, null);

                }

            }
            catch { }
        }

        private void grid3_SelectionChanged(object sender, EventArgs e)
        {
            //if (fPISINFOBindingSource_CurrentChanging)
            //    return;
            //if (fPISINFOBindingSource.Current == null)
            //    return;
            //var Selected = ((DataRowView)fPISINFOBindingSource.Current).Row as CMDataSet.FPISINFORow;
            //if (Selected == null)
            //    return;

           



        }

        private void grid3_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == 0 && e.RowIndex >= 0)
            {
                grid3[e.ColumnIndex, e.RowIndex].Value = (grid3.Rows.Count - e.RowIndex).ToString("N0");
            }

            //if (e.ColumnIndex == 1 && e.RowIndex >= 0)
            //{
            //    e.Value = e.Value.ToString().Replace("위탁", "");
            //}

            else if (e.ColumnIndex == 5 && e.RowIndex >= 0)
            {
                var Selected = ((DataRowView)grid3.Rows[e.RowIndex].DataBoundItem).Row as CMDataSet.FPIS_TRURow;
                var Query = cMDataSet.FPIS_CONT.Where(c => c.FPIS_ID == Selected.FPIS_ID);



                if (Query.Any())
                {
                    e.Value = Query.First().CL_COMP_NM;
                }


            }
            //else if (e.ColumnIndex == 6 && e.RowIndex >= 0)
            //{
            //    var Selected = ((DataRowView)grid3.Rows[e.RowIndex].DataBoundItem).Row as CMDataSet.FPIS_TRURow;
            //    if (Selected.CONT_GUBUN == "차량위탁")
            //    {
            //        var Query = cMDataSet.Drivers.Where(c => c.DriverId == Selected.DriverId).ToArray();

            //        if (Query.Any())
            //        {
            //            e.Value = Query.First().Name;
            //        }

            //    }

            //}

            //else if (e.ColumnIndex == 7 && e.RowIndex >= 0)
            //{
            //    var Selected = ((DataRowView)grid3.Rows[e.RowIndex].DataBoundItem).Row as CMDataSet.FPIS_TRURow;
            //    if (Selected.CONT_GUBUN == "차량위탁")
            //    {
            //        var Query = cMDataSet.Drivers.Where(c => c.DriverId == Selected.DriverId).ToArray();

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
            //else if (e.ColumnIndex ==7 && e.RowIndex >= 0)
            //{
            //    var Selected = ((DataRowView)grid3.Rows[e.RowIndex].DataBoundItem).Row as CMDataSet.FPIS_TRURow;
            //    if (!String.IsNullOrEmpty(Selected.TRU_COMP_BSNS_NUM))
            //    {
            //        if (Selected.CONT_GUBUN == "차량위탁")
            //        {
            //            e.Value = e.Value.ToString().Substring(0, 3) + "-" + e.Value.ToString().Substring(3, 2) + "-" + e.Value.ToString().Substring(5, 5);

            //        }
            //    }

            //}
            //else if (e.ColumnIndex == 9 && e.RowIndex >= 0)
            //{
            //    //var Selected = ((DataRowView)grid3.Rows[e.RowIndex].DataBoundItem).Row as CMDataSet.FPIS_TRURow;
            //    //var Query = cMDataSet.Drivers.Where(c => c.DriverId == Selected.dr);



            //    //if (Query.Any())
            //    //{
            //    //    grid3[e.ColumnIndex, e.RowIndex].Value = (grid3.Rows.Count - e.RowIndex).ToString("N0");
            //    //}

            //}

            else if (e.ColumnIndex == 14 && e.RowIndex >= 0)
            {
            //    if (!String.IsNullOrEmpty(e.Value.ToString()))
            //    {
            //        e.Value = String.Format("{0:#,##0}", Convert.ToDecimal(e.Value));
            //    }

            }
            else if (e.ColumnIndex == 15 && e.RowIndex >= 0)
            {
                if (!String.IsNullOrEmpty(e.Value.ToString()))
                {
                    e.Value = String.Format("{0:#,##0}", Convert.ToDecimal(e.Value));
                }

            }

            else if (e.ColumnIndex == 27 && e.RowIndex >= 0)
            {
                if (!String.IsNullOrEmpty(e.Value.ToString()))
                {
                    e.Value = ((DateTime)e.Value).ToString("d").Replace("-", "/");
                }

            }
        }

       
        //private void grid4_SelectionChanged(object sender, EventArgs e)
        //{
        //    if (fPISINFOBindingSource_CurrentChanging)
        //        return;
        //    if (fPISINFOBindingSource.Current == null)
        //        return;
        //    var Selected = ((DataRowView)fPISINFOBindingSource.Current).Row as CMDataSet.FPISINFORow;
        //    if (Selected == null)
        //        return;

        //}

        //private void txtSearch3_KeyUp(object sender, KeyEventArgs e)
        //{
        //    if (e.KeyCode != Keys.Return) return;
        //    btn_CarSearch_Click(null, null);
        //}

        private void txtSearch2_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Return) return;
            btn_TRU_Search_Click(null, null);
        }

        private void txt_Fsearch_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Return) return;
            btn_CL_Search_Click(null, null);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btn_infoNew_Click(object sender, EventArgs e)
        {
            dtp_I_Start.Value = DateTime.Parse(DateTime.Now.ToString("yyyy") + "-01-01");
            dtp_I_End.Value = DateTime.Now;
            txt_Fsearch.Text = string.Empty;
            btn_CL_Search_Click(null, null);
        }

        private void btn_Customer_Click(object sender, EventArgs e)
        {
            if (fPISCONTBindingSource.Current == null)
                return;
            var Selected = ((DataRowView)fPISCONTBindingSource.Current).Row as CMDataSet.FPIS_CONTRow;
            if (Selected == null)
                return;
            FrmCustomerSearch2 _frmCustomerSearch = new FrmCustomerSearch2("1,3");
            _frmCustomerSearch.Owner = this;
            _frmCustomerSearch.StartPosition = FormStartPosition.CenterParent;
            if (_frmCustomerSearch.ShowDialog() == DialogResult.OK)
            {
                txt_CL_COMP_NM.Text = _frmCustomerSearch.CL_COMP_NM;
                txt_CL_COMP_BSNS_NUM.Text = _frmCustomerSearch.CL_COMP_BSNS;
                txt_CL_P_TEL.Text = _frmCustomerSearch.CL_P_TEL;
                txt_CL_COMP_CORP_NUM.Text = _frmCustomerSearch.CL_COMP_CORP_NUM;

                Selected.CustomerId = _frmCustomerSearch.CustomerId;
            }
        }

        private void btn_Customer2_Click(object sender, EventArgs e)
        {
            FrmCustomerSearch _frmCustomerSearch = new FrmCustomerSearch("2,3");
            _frmCustomerSearch.grid1.KeyDown += new KeyEventHandler((object isender, KeyEventArgs ie) =>
            {
                if (ie.KeyCode != Keys.Return) return;
                if (_frmCustomerSearch.grid1.SelectedCells.Count == 0) return;
                if (_frmCustomerSearch.grid1.SelectedCells[0].RowIndex < 0) return;


                txt_TRU_COMP_NM.Text = _frmCustomerSearch.grid1[0, _frmCustomerSearch.grid1.SelectedCells[0].RowIndex].Value.ToString();
                txt_TRU_COMP_BSNS_NUM.Text = _frmCustomerSearch.grid1[1, _frmCustomerSearch.grid1.SelectedCells[0].RowIndex].Value.ToString();


                txt_TRU_COMP_CORP_NUM.Text = _frmCustomerSearch.grid1[14, _frmCustomerSearch.grid1.SelectedCells[0].RowIndex].Value.ToString();



                _frmCustomerSearch.Close();
            });
            _frmCustomerSearch.grid1.MouseDoubleClick += new MouseEventHandler((object isender, MouseEventArgs ie) =>
            {
                if (_frmCustomerSearch.grid1.SelectedCells.Count == 0) return;
                if (_frmCustomerSearch.grid1.SelectedCells[0].RowIndex < 0) return;
                txt_TRU_COMP_NM.Text = _frmCustomerSearch.grid1[0, _frmCustomerSearch.grid1.SelectedCells[0].RowIndex].Value.ToString();
                txt_TRU_COMP_BSNS_NUM.Text = _frmCustomerSearch.grid1[1, _frmCustomerSearch.grid1.SelectedCells[0].RowIndex].Value.ToString();


                txt_TRU_COMP_CORP_NUM.Text = _frmCustomerSearch.grid1[14, _frmCustomerSearch.grid1.SelectedCells[0].RowIndex].Value.ToString();
                _frmCustomerSearch.Close();
            });






            _frmCustomerSearch.Owner = this;
            _frmCustomerSearch.StartPosition = FormStartPosition.CenterParent;
            _frmCustomerSearch.ShowDialog();
        }

        private void tabControl1_DrawItem(object sender, DrawItemEventArgs e)
        {
            Graphics gr = e.Graphics; // 뭔가를 그리고 싶다면 반드시 그래픽스 객체 얻기
            Font fnt1 = new Font(e.Font, FontStyle.Regular);  // 그리고 싶은 글꼴설정
            Font fnt2 = new Font(e.Font, FontStyle.Regular);  // 그리고 싶은 글꼴설정
            //  Font fnt3 = new Font("굴림", 10, FontStyle.Bold, GraphicsUnit.Point);
            StringFormat sf = new StringFormat(); // 문자열 형식지정하는 객체로 여기서는 정렬기준만 설정했습니다.
            sf.Alignment = StringAlignment.Center;
            sf.LineAlignment = StringAlignment.Center;
            Brush foreBruch;
            Brush foreBruch2;
            Brush backBruch;
            string tabName = this.tabControl1.TabPages[e.Index].Text;

            if (e.Index == this.tabControl1.SelectedIndex)
            {
               // e.Graphics.FillRectangle(Brushes.Yellow, e.Bounds);
                   backBruch = new SolidBrush(Color.Yellow);

                foreBruch = Brushes.Blue;
              
            }
            else
            {
                backBruch = new SolidBrush(Color.White);
                foreBruch = Brushes.Black;
               
            }
            e.Graphics.FillRectangle(backBruch, e.Bounds);
            Rectangle recTab = e.Bounds;

            if (true)
            {
                recTab = new Rectangle(recTab.X, recTab.Y + 4, recTab.Width, recTab.Height);
                gr.DrawString(tabName, fnt2, foreBruch, recTab, sf);  // 하나씩 탭에 그립니다.

            }
            else
            {
               // recTab = new Rectangle(recTab.X, recTab.Y + 4, recTab.Width, recTab.Height + 4);
                gr.DrawString(tabName, fnt2, foreBruch, recTab, sf);
            }

           

         


        }

        private void dataGridView2_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {

            //var GridCargoItemDataSource1 = cMDataSet.FPISOptions.Where(c => c.Div == "CargoItem").Select(c => new { c.FpisOptionId, c.Name, c.Seq, c.Value }).OrderBy(c => c.Seq).ToArray();

            //cONTITEMDataGridViewTextBoxColumn1.DataSource = GridCargoItemDataSource1;
            //cONTITEMDataGridViewTextBoxColumn1.DisplayMember = "Name";
            //cONTITEMDataGridViewTextBoxColumn1.ValueMember = "value";
            if (e.ColumnIndex == 1 && e.RowIndex >= 0)
            {
                var Selected = ((DataRowView)dataGridView2.Rows[e.RowIndex].DataBoundItem).Row as CMDataSet.FPISINFO1Row;

                if (Selected.ONE_GUBUN == "1")
                {
                    e.Value = "단일건";
                }
            }
            else if (e.ColumnIndex == 2 && e.RowIndex >= 0)
            {
                var Selected = ((DataRowView)dataGridView2.Rows[e.RowIndex].DataBoundItem).Row as CMDataSet.FPISINFO1Row;
                var Query = cMDataSet.FPISOptions.Where(c => c.Div == "CargoItem" && c.Value == Selected.CONT_ITEM);



                if (Query.Any())
                {
                    e.Value = Query.First().Name;
                }


            }
        }
        bool overhead = false;
        List<string> checkedCodes = new List<string>();

        private void tabControl1_Selecting(object sender, TabControlCancelEventArgs e)
        {
            e.Cancel = !e.TabPage.Enabled;
        }

        private void txt_CONT_DEPOSIT_Enter(object sender, EventArgs e)
        {
            txt_CONT_DEPOSIT.Text = txt_CONT_DEPOSIT.Text.Replace(",", "");
        }

        private void txt_CONT_DEPOSIT_Leave(object sender, EventArgs e)
        {
            decimal _d = 0;
            txt_CONT_DEPOSIT.Text = txt_CONT_DEPOSIT.Text.Replace(",", "");
            if (decimal.TryParse(txt_CONT_DEPOSIT.Text, out _d))
            {
                txt_CONT_DEPOSIT.Text = _d.ToString("N0");
                var Selected = ((DataRowView)fPISCONTBindingSource.Current).Row as CMDataSet.FPIS_CONTRow;
                if (Selected != null)
                {
                    Selected.CONT_DEPOSIT = _d.ToString();
                }

            }
        }

        private void txt_CONT_DEPOSIT_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back)))
            {
                e.Handled = true;
            }
        }

        private void dataGridView2_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {

                if (e.ColumnIndex == 0)
                {
                    object o = dataGridView2[e.ColumnIndex, e.RowIndex].Value;
                    string code = ((DataRowView)fPISINFO1BindingSource[e.RowIndex])["FPIS_Id"].ToString();
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
            if (overhead) return;
        }

        private void chkAllSelect_CheckedChanged(object sender, EventArgs e)
        {
            overhead = true;
            for (int i = 0; i < dataGridView2.RowCount; i++)
            {
                dataGridView2[CheckBox.Index, i].Value = chkAllSelect.Checked;
            }
            overhead = false;
            dataGridView2_CellValueChanged(null, null);
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

        private void cmb_CargoGubun_SelectedIndexChanged(object sender, EventArgs e)
        {
            err.Clear();
            if (cmb_CargoGubun.SelectedIndex == 0)
            {
                label4.ForeColor = Color.Blue;
                label6.ForeColor = Color.Blue;
                cbo_CONT_YN.Enabled = true;
            }
            else
            {
                label4.ForeColor = Color.Blue;
                label6.ForeColor = Color.Blue;
                cbo_CONT_YN.Checked = false;
                cbo_CONT_YN.Enabled = false;
            }
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
           



        }

        private void grid3_CellLeave(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void fPISINFO1BindingSource_CurrentChanged(object sender, EventArgs e)
        {
           
            //err.Clear();


            if (fPISINFO1BindingSource.Current == null)
            {
                btn_TRU_Search_Click(null, null);
                return;
            }

            fPISINFO1BindingSource_CurrentChanging = true;

            var Selected = ((DataRowView)fPISINFO1BindingSource.Current).Row as CMDataSet.FPISINFO1Row;


            if (Selected != null)
            {
                int intchange;


                NFPISId.Value = Selected.FPIS_ID;


                for (int i = 0; i < dataGridView2.RowCount; i++)
                {

                    dataGridView2[0, i].Value = false; // 6은 CheckBox가 있는 열 번호
                    chkAllSelect.Checked = false;

                }

                // 현재 선택된 행의 CheckBox값을 True로 설정

                for (int i = 0; i < dataGridView2.SelectedRows.Count; i++)
                {

                    //Convert.ToInt32 => string을 int로 변환

                    intchange = Convert.ToInt32(dataGridView2.SelectedRows[i].Index);

                    dataGridView2[0, intchange].Value = true;
                    chkAllSelect.Checked = false;

                }


               
          
               btn_TRU_Search_Click(null, null);







                var SizeName = cMDataSet.FPISOptions.FirstOrDefault(c => c.Value == Selected.CONT_GOODS_UNIT && c.Div == "CargoSize");
                if (SizeName != null)
                    label81.Text = SizeName.Name;
               // label85.Text = SizeName.First().Name;
              
             



            }



            fPISINFO1BindingSource_CurrentChanging = false;
        
        }
    }
}
