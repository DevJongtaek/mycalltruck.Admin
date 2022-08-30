using mycalltruck.Admin.Class.Common;
using mycalltruck.Admin.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Security;
using System.Security.Cryptography;
using System.Net;
using mycalltruck.Admin.DataSets;

namespace mycalltruck.Admin
{
    public partial class FrmMN0214_SUBCARGOOWNERMANAGE_Add : Form
    {
        private string Sgubun = "";
        public FrmMN0214_SUBCARGOOWNERMANAGE_Add()
        {
            InitializeComponent();
            txt_CreateDate.Text = DateTime.Now.ToString("yyyy-MM-dd").Replace("-", "/");
        }
        private void FrmMN0204_CARGOOWNERMANAGE_Add_Load(object sender, EventArgs e)
        {
            txt_CreateDate.Text = DateTime.Now.ToString("yyyy-MM-dd").Replace("-", "/");
            ClientsCode_Add();
            txt_Name.Focus();
        }

        private void ClientsCode_Add()
        {
            var _Code = 100001;
            using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                _Connection.Open();
                using (SqlCommand _Command = _Connection.CreateCommand())
                {
                    _Command.CommandText = "SELECT Code FROM SubClients WHERE ClientId = @ClientId ORDER BY Code DESC";
                    _Command.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                    var o = _Command.ExecuteScalar();
                    if (o != null)
                    {
                        _Code = int.Parse(o.ToString()) + 1;
                    }
                }
                _Connection.Close();
            }
            txt_Code.Text = _Code.ToString("000000");
        }

        private void SaveClear()
        {
            ClientsCode_Add();
            txt_Name.Text = "";
            txt_BizNo.Text = "";
            txt_CEO.Text = "";
            txt_Uptae.Text = "";
            txt_Upjong.Text = "";
            txt_Email.Text = "";
            txt_MobileNo.Text = "";
            txt_PhoneNo.Text = "";
            txt_Zip.Text = "";
            txt_State.Text = "";
            txt_City.Text = "";
            txt_Street.Text = "";
            txt_CreateDate.Text = DateTime.Now.ToString("yyyy-MM-dd").Replace("-", "/");
            txt_Code.Focus();
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            Sgubun = "Add";
            _UpdateDB();
        }

        private int _UpdateDB()
        {
            err.Clear();
            if (txt_Code.Text == "")
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                err.SetError(txt_Code, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
            }
            else
            {
                using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                {
                    _Connection.Open();
                    using (SqlCommand _Command = _Connection.CreateCommand())
                    {
                        _Command.CommandText = "SELECT Code FROM SubClients WHERE ClientId = @ClientId AND Code = @Code";
                        _Command.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                        _Command.Parameters.AddWithValue("@Code", txt_Code.Text);
                        var o = _Command.ExecuteScalar();
                        if (o != null)
                        {
                            MessageBox.Show("코드가 중복되었습니다.!!", "코드 입력 오류");
                            err.SetError(txt_Code, "코드가 중복되었습니다.!!");
                            return -1;
                        }
                    }
                    _Connection.Close();
                }
            }
            if (txt_Name.Text == "")
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                err.SetError(txt_Name, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                return -1;
            }
            if (txt_BizNo.Text.Length != 12)
            {
                MessageBox.Show("사업자 번호가 완전하지 않습니다.", "사업자 번호 불완전");
                err.SetError(txt_BizNo, "사업자 번호가 완전하지 않습니다.");
                return -1;
            }
            else 
            {
                using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                {
                    _Connection.Open();
                    using (SqlCommand _Command = _Connection.CreateCommand())
                    {
                        _Command.CommandText = "SELECT BizNo FROM SubClients WHERE ClientId = @ClientId AND BizNo = @BizNo";
                        _Command.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                        _Command.Parameters.AddWithValue("@BizNo", txt_BizNo.Text);
                        var o = _Command.ExecuteScalar();
                        if (o != null)
                        {
                            MessageBox.Show("사업자 번호가 중복되었습니다.!!", "사업자 번호 입력 오류");
                            err.SetError(txt_BizNo, "사업자 번호가 중복되었습니다.!!");
                            return -1;
                        }
                    }
                    _Connection.Close();
                }
            }
            if (txt_CEO.Text == "")
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                err.SetError(txt_CEO, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                return -1;
            }
            if (txt_Uptae.Text == "")
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                err.SetError(txt_Uptae, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                return -1;
            }
            if (txt_Upjong.Text == "")
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                err.SetError(txt_Upjong, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                return -1;
            }
            if (string.IsNullOrWhiteSpace(txt_Zip.Text) || string.IsNullOrWhiteSpace(txt_Street.Text))
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                err.SetError(txt_Street, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                return -1;
            }
            if (txt_PhoneNo.Text.Replace(" ","").Replace("-","") == "")
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                err.SetError(txt_PhoneNo, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                return -1;
            }
            try
            {
                _AddClient();
                return 1;
            }
            catch (NoNullAllowedException ex)
            {
                string iName = string.Empty;
                string code = ex.Message.Split(' ')[0].Replace("'", "");
                if (code == "Code") iName = "코드";
                if (code == "Name") iName = "상호";
                if (code == "BizNo") iName = "사업자 번호";
                if (code == "CEO") iName = "대표자명";
                if (code == "Addr") iName = "주소";
                if (code == "Uptae") iName = "업태";
                if (code == "Upjong") iName = "업종";
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목 누락");
                return 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "추가 작업 실패");
                return 0;
            }
        }

        public ClientDataSet.SubClientsRow Added = null;
        private void _AddClient()
        {
            Added = clientDataSet.SubClients.NewSubClientsRow();

            Added.Code = txt_Code.Text;
            Added.Name = txt_Name.Text;
            Added.BizNo = txt_BizNo.Text;
            Added.CEO = txt_CEO.Text;
            Added.Uptae = txt_Uptae.Text;
            Added.Upjong = txt_Upjong.Text;

            Added.AddressState = txt_State.Text;
            Added.AddressCity = txt_City.Text;
            Added.AddressDetail = txt_Street.Text;
            Added.ZipCode = txt_Zip.Text;
            Added.MobileNo = txt_MobileNo.Text;
            Added.PhoneNo = txt_PhoneNo.Text;
            Added.Email = txt_Email.Text;

            Added.CreateDate = DateTime.Now;
            Added.ClientId = LocalUser.Instance.LogInInformation.ClientId;

            clientDataSet.SubClients.AddSubClientsRow(Added);
            try
            {
                subClientsTableAdapter.Update(Added);
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.추가성공, "지점", 1), "지점 정보 추가 성공");
                if (Sgubun == "Add")
                {
                    SaveClear();
                }
                else
                {
                    Close();
                }
            }
            catch
            {
                MessageBox.Show("지점 정보 추가 실패하였습니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

        }
      
        public bool IsSuccess = false;
        private void btnAddClose_Click(object sender, EventArgs e)
        {
            Sgubun = "Close";
            _UpdateDB();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void FrmMN0204_CARGOOWNERMANAGE_Add_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
                btnAdd_Click(null, null);
            else if (e.KeyCode == Keys.F6)
                btnAddClose_Click(null, null);
        }

        private void btnFindZip_Click(object sender, EventArgs e)
        {
            FindZip f = new Admin.FindZip();
            f.ShowDialog();
            if (!String.IsNullOrEmpty(f.Zip) && !String.IsNullOrEmpty(f.Address))
            {
                txt_Zip.Text = f.Zip;
                var ss = f.Address.Split(' ');
                txt_State.Text = ss[0];
                txt_City.Text = ss[1];
                txt_Street.Text = String.Join(" ", ss.Skip(2));
            }
        }

        private void txt_MobileNo_Enter(object sender, EventArgs e)
        {
            txt_MobileNo.Text = txt_MobileNo.Text.Replace(",", "");
        }

        private void txt_MobileNo_Leave(object sender, EventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(txt_MobileNo.Text))
            {
                var _S = txt_MobileNo.Text.Replace("-", "").Replace(" ", "");
                if (_S.Length > 3)
                {
                    _S = _S.Substring(0, 3) + "-" + _S.Substring(3);
                }
                if (_S.Length > 7)
                {
                    _S = _S.Substring(0, 7) + "-" + _S.Substring(7);
                }
                if (_S.Length > 12)
                {
                    _S = _S.Replace("-", "");
                    _S = _S.Substring(0, 3) + "-" + _S.Substring(3, 4) + "-" + _S.Substring(7, 4);
                }
                txt_MobileNo.Text = _S;
            }
        }

        private void txt_MobileNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back)))
            {
                e.Handled = true;
            }
        }

        private void txt_PhoneNo_Enter(object sender, EventArgs e)
        {
            txt_PhoneNo.Text = txt_PhoneNo.Text.Replace(",", "");
        }

        private void txt_PhoneNo_Leave(object sender, EventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(txt_PhoneNo.Text))
            {
                var _S = txt_PhoneNo.Text.Replace("-", "").Replace(" ", "");
                if (_S.StartsWith("02"))
                {
                    if (_S.Length > 2)
                    {
                        _S = _S.Substring(0, 2) + "-" + _S.Substring(2);
                    }
                    if (_S.Length > 6)
                    {
                        _S = _S.Substring(0, 6) + "-" + _S.Substring(6);
                    }
                    if (_S.Length > 11)
                    {
                        _S = _S.Replace("-", "");
                        _S = _S.Substring(0, 2) + "-" + _S.Substring(2, 4) + "-" + _S.Substring(6, 4);
                    }
                }
                else
                {
                    if (_S.Length > 3)
                    {
                        _S = _S.Substring(0, 3) + "-" + _S.Substring(3);
                    }
                    if (_S.Length > 7)
                    {
                        _S = _S.Substring(0, 7) + "-" + _S.Substring(7);
                    }
                    if (_S.Length > 12)
                    {
                        _S = _S.Replace("-", "");
                        _S = _S.Substring(0, 3) + "-" + _S.Substring(3, 4) + "-" + _S.Substring(7, 4);
                    }
                }
                txt_PhoneNo.Text = _S;
            }
        }

        private void txt_PhoneNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back)))
            {
                e.Handled = true;
            }
        }


    }
}
