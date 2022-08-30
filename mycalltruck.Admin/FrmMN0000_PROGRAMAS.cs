using mycalltruck.Admin.Class.Common;
using mycalltruck.Admin.Class.Extensions;
using mycalltruck.Admin.UI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace mycalltruck.Admin
{
    public partial class FrmMN0000_PROGRAMAS : Form
    {
        int GridIndex = 0;

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


        public FrmMN0000_PROGRAMAS()
        {
            InitializeComponent();
            if (!LocalUser.Instance.LogInInformation.IsAdmin)
            {
                ApplyAuth();
            }
            _InitCmb();
            _InitCmbSearch();
        }

        private void FrmMN0000_PROGRAMAS_Load(object sender, EventArgs e)
        {
            // TODO: 이 코드는 데이터를 'cMDataSet.CardPayAS' 테이블에 로드합니다. 필요 시 이 코드를 이동하거나 제거할 수 있습니다.
            this.cardPayASTableAdapter.FillByAdmin(this.cMDataSet.CardPayAS);

            if (!LocalUser.Instance.LogInInformation.IsAdmin)
            {
                pn_Client.Enabled = true;
                pn_Admin.Enabled = false;
                btn_New.Enabled = true;



            }
            else
            {

                pn_Client.Enabled = true;
                pn_Admin.Enabled = true;
                btn_New.Enabled = false;
            }

            btn_Search_Click(null, null);

        }
        private void _InitCmb()
        {
            var ProcessStateDataSource = SingleDataSet.Instance.StaticOptions.Where(c=> c.Div == "ASState" && c.Value != 0).Select(c => new { c.Name }).ToArray();
            cmb_State.DataSource = ProcessStateDataSource;
            cmb_State.DisplayMember = "Name";
            cmb_State.ValueMember = "Name";

            var ProcessStateDataSource1 = SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "ASState").Select(c => new { c.Name }).ToArray();
            cmb_State_I.DataSource = ProcessStateDataSource1;
            cmb_State_I.DisplayMember = "Name";
            cmb_State_I.ValueMember = "Name";

        }
        private Dictionary<string, string> DicSearch = new Dictionary<string, string>();
        private void _InitCmbSearch()
        {
            DataGridViewColumn[] cols = new DataGridViewColumn[] {  clientNameDataGridViewTextBoxColumn, processNameDataGridViewTextBoxColumn };
            cmb_Search.Items.Clear();
            DicSearch.Clear();
            cmb_Search.Items.Add("전체");

            foreach (var item in cols)
            {

                cmb_Search.Items.Add(item.HeaderText);
                if (item.DataPropertyName == null || item.DataPropertyName == "")
                {
                    DicSearch.Add(item.HeaderText, "'" + item.Name);
                }
                else
                {
                    DicSearch.Add(item.HeaderText, item.DataPropertyName);
                }
            }
            if (cmb_Search.Items.Count > 0) cmb_Search.SelectedIndex = 0;








        }
        private void btn_New_Click(object sender, EventArgs e)
        {

            FrmMN0000_PROGRAMAS_ADD _Form = new FrmMN0000_PROGRAMAS_ADD();
            _Form.Owner = this;
            _Form.StartPosition = FormStartPosition.CenterParent;
            _Form.ShowDialog(

            );
            btn_Search_Click(null, null);
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            err.Clear();

            if (txt_PhoneNo.Text.Replace("-", "").Length < 9)
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                err.SetError(txt_PhoneNo, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                
            }




            if (txt_Contents.Text == "")
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                err.SetError(txt_Contents, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
               

            }
            UpdateDB();
        }
        private void UpdateDB()
        {
            try
            {
                cardPayASBindingSource.EndEdit();

                var Row = ((DataRowView)cardPayASBindingSource.Current).Row as CMDataSet.CardPayASRow;
                if (LocalUser.Instance.LogInInformation.IsAdmin)
                {
                    Row.ProcessDate = dtp_ProcessDate.Value;
                }
                cardPayASTableAdapter.Update(Row);
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.수정성공, "프로그램 A/S", 1), "프로그램 A/S 수정 성공");

                if (dataGridView1.RowCount > 1)
                {
                    GridIndex = cardPayASBindingSource.Position;
                    btn_Search_Click(null, null);
                    dataGridView1.CurrentCell = dataGridView1.Rows[GridIndex].Cells[0];
                }
                else
                {
                    btn_Search_Click(null, null);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "프로그램 A/S 변경 실패", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }
        private void btnCurrentDelete_Click(object sender, EventArgs e)
        {
            List<DataGridViewRow> deleteRows = new List<DataGridViewRow>();

            if (dataGridView1.SelectedCells.Count > 0)
            {
                foreach (DataGridViewCell item in dataGridView1.SelectedCells)
                {
                    if (item.RowIndex != -1 && deleteRows.Contains(item.OwningRow) == false) deleteRows.Add(item.OwningRow);
                }
               



                if (MessageBox.Show(ButtonMessage.GetMessage(MessageType.삭제질문, "프로그램 A/S", deleteRows.Count), "선택항목 삭제 여부 확인", MessageBoxButtons.YesNo) != DialogResult.Yes) return;
                foreach (DataGridViewRow row in deleteRows)
                {
                    dataGridView1.Rows.Remove(row);
                }
                cardPayASTableAdapter.Update(cMDataSet.CardPayAS);
                


            }
            //Up_Status = "Delete";
            //int _rows = UpdateDB(Up_Status);
            MessageBox.Show(ButtonMessage.GetMessage(MessageType.삭제성공, "프로그램 A/S", 1), "프로그램 A/S 삭제 성공");
            btn_Search_Click(null, null);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btn_Search_Click(object sender, EventArgs e)
        {
            _Search();
        }

        private void btn_Inew_Click(object sender, EventArgs e)
        {
            cmb_Search.SelectedIndex = 0;
            txt_Search.Text = string.Empty;
            cmb_State_I.SelectedIndex = 0;

            _Search();
        }


        private void _Search()
        {
            if(!LocalUser.Instance.LogInInformation.IsAdmin)

            {

                cardPayASTableAdapter.FillByClient(cMDataSet.CardPayAS, LocalUser.Instance.LogInInformation.ClientId);
            }
            else
            {
                cardPayASTableAdapter.FillByAdmin(cMDataSet.CardPayAS);
            }
         
            string _FilterString = string.Empty;
            string _cmbSearchString = string.Empty;

            string _cmbSearchStatus = string.Empty;
            try
            {
               if (cmb_Search.Text == "상호")
                {

                    string filter = string.Format("ClientName Like  '%{0}%'", txt_Search.Text);
                    _cmbSearchString = filter;


                }
                else if (cmb_Search.Text == "처리자")
                {
                    string filter = string.Format("ProcessName Like  '%{0}%'", txt_Search.Text);
                    _cmbSearchString = filter;
                }
             

                _FilterString += _cmbSearchString;

                
                if (cmb_State_I.SelectedIndex != 0)
                {

                   

                    string BizCode = cmb_State_I.SelectedValue.ToString();
                    _cmbSearchStatus = "ProcessState = '" + BizCode + "'";
                }
                if (_FilterString != string.Empty && _cmbSearchStatus != string.Empty)
                {
                    _FilterString += " AND " + _cmbSearchStatus;
                }
                else
                {
                    _FilterString += _cmbSearchStatus;
                }



                try
                {

                    cardPayASBindingSource.Filter = _FilterString;
                }
                catch
                {
                    btn_Search_Click(null, null);
                }
            }
            catch
            {
            }


        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            //순번
            if (e.ColumnIndex == 0 && e.RowIndex >= 0)
            {
                dataGridView1[e.ColumnIndex, e.RowIndex].Value = (dataGridView1.Rows.Count - e.RowIndex).ToString("N0").Replace(",", "");
            }

            else if (dataGridView1.Columns[e.ColumnIndex] == requestDateDataGridViewTextBoxColumn)
            {

                // var Selected = ((DataRowView)dataGridView1.Rows[e.RowIndex].DataBoundItem).Row as CMDataSet.CardPayASRow;


                e.Value = DateTime.Parse(e.Value.ToString()).ToString("d").Replace("-", "/");

            }

            else if (dataGridView1.Columns[e.ColumnIndex] == processDateDataGridViewTextBoxColumn)
            {

                var Selected = ((DataRowView)dataGridView1.Rows[e.RowIndex].DataBoundItem).Row as CMDataSet.CardPayASRow;

                if (Selected.ProcessState == "처리완료")
                {
                    e.Value = DateTime.Parse(Selected.ProcessDate.ToString()).ToString("d").Replace("-", "/");
                }
                else
                {
                    e.Value = "";

                }
            }

        }

        private void txt_Search_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Return) return;
            _Search();
        }

        private void btn_File1_Click(object sender, EventArgs e)
        {
            var Selected = ((DataRowView)cardPayASBindingSource.Current).Row as CMDataSet.CardPayASRow;
            if (Selected == null)
                return;
            dlgOpen.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            if (dlgOpen.ShowDialog() != DialogResult.OK) return;



            using (MemoryStream ms = new MemoryStream())
            using (MemoryStream mm = new MemoryStream())
            {
                Bitmap _b = null;
                try
                {
                    var b = File.ReadAllBytes(dlgOpen.FileName);
                    ms.Write(b, 0, b.Length);
                    ms.Position = 0;
                    _b = new Bitmap(ms);
                }
                catch (Exception)
                {
                    MessageBox.Show("읽을 수 없는 이미지 파일입니다. 다른 파일을 선택해주십시오.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }

                try
                {
                    _b.Save(mm, System.Drawing.Imaging.ImageFormat.Png);
                    byte[] bytes = mm.ToArray();

                    AndroidImageViewModel i = new Admin.AndroidImageViewModel();
                    i.DriverId = 0;
                    i.ImageData64String = System.Convert.ToBase64String(bytes);

                    var http = (HttpWebRequest)WebRequest.Create(new Uri("http://m.cardpay.kr/ImageFromApp/SetASPaper"));
                    http.Accept = "application/json";
                    http.ContentType = "application/json";
                    http.Method = "POST";

                    string parsedContent = JsonConvert.SerializeObject(i);
                    ASCIIEncoding encoding = new ASCIIEncoding();
                    Byte[] b = encoding.GetBytes(parsedContent);

                    Stream newStream = http.GetRequestStream();
                    newStream.Write(b, 0, b.Length);
                    newStream.Close();

                    var response = http.GetResponse();

                    var stream = response.GetResponseStream();
                    var sr = new StreamReader(stream);
                    var content = sr.ReadToEnd();
                    var o = (dynamic)JsonConvert.DeserializeObject(content);
                    if ((bool)o.Result)
                    {
                        txt_FileName1.Text = System.IO.Path.GetFileName(dlgOpen.FileName);
                        Selected.FileName1 = txt_FileName1.Text;
                        Selected.FilePath1 = (string)o.FilePath;
                        cardPayASTableAdapter.Update(Selected);
                        MessageBox.Show("이미지 전송이 완료 되었습니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("이미지를 전송 중 오류가 발생하였습니다. 잠시후 다시 시도해주시기 바랍니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("이미지를 전송 중 오류가 발생하였습니다. 잠시후 다시 시도해주시기 바랍니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
            }

        }

        private void btn_FileView1_Click(object sender, EventArgs e)
        {
            var Selected = ((DataRowView)cardPayASBindingSource.Current).Row as CMDataSet.CardPayASRow;
            if (Selected == null || String.IsNullOrEmpty(Selected.FilePath1))
                return;
            Bitmap _b = null;
            PrintDocument pDoc = new PrintDocument();
            PageSettings ps = new PageSettings();
            ps.Margins = new Margins(10, 10, 10, 10);
            pDoc.DefaultPageSettings = ps;
            PrintPreviewDialog ppDoc = new PrintPreviewDialog();
            ppDoc.ClientSize = new System.Drawing.Size(500, 500);

            //   ppDoc.ClientSize = new System.Drawing.Size(pic_BizPaper.Image.Width, pic_BizPaper.Image.Height);
            ppDoc.UseAntiAlias = true;
            pDoc.PrintPage += new PrintPageEventHandler((_sender, _e) =>
            {
                if (_b.Width > _b.Height)
                {
                    _b.RotateFlip(RotateFlipType.Rotate90FlipNone);
                }
                var width = _b.Width;
                var height = _b.Height;
                if (_b.Width > _e.MarginBounds.Width)
                {
                    width = _e.MarginBounds.Width;
                    height = height * _e.MarginBounds.Width / _b.Width;
                }
                else if (_b.Height > _e.MarginBounds.Height)
                {
                    height = _e.MarginBounds.Height;
                    width = width * _e.MarginBounds.Height / _b.Height;
                }
                _e.Graphics.DrawImage(_b, 0, 0, width, height);

            });
            ppDoc.Document = pDoc;
            ((Form)ppDoc).WindowState = FormWindowState.Maximized;
            Task.Factory.StartNew(() =>
            {
                WebClient mWebClient = new WebClient();
                try
                {
                    var b = mWebClient.DownloadData("http://m.cardpay.kr/ImageFromAdmin/GetASPaper1?AsId=" + Selected.AsId.ToString());
                    MemoryStream ms = new MemoryStream();
                    ms.Write(b, 0, b.Length);
                    ms.Position = 0;
                    _b = new Bitmap(ms);
                }
                catch (Exception)
                {
                    Invoke(new Action(() => MessageBox.Show("이미지를 가져오는 중 오류가 발생하였습니다. 잠시 후 다시 이용바랍니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop)));
                    return;
                }
                Invoke(new Action(() => ppDoc.Show()));
            });
        }

        private void cardPayASBindingSource_CurrentChanged(object sender, EventArgs e)
        {
            

            if (cardPayASBindingSource.Current == null)
                return;

            var Selected = ((DataRowView)cardPayASBindingSource.Current).Row as CMDataSet.CardPayASRow;
            if (Selected != null)
            {


                if (Selected.ProcessState == "처리완료")
                {

                    dtp_ProcessDate.Enabled = true;
                    dtp_ProcessDate.Value = DateTime.Now;
                }

                if (!LocalUser.Instance.LogInInformation.IsAdmin)
                {
                    if (Selected.ProcessState == "접수")
                    {
                        btnCurrentDelete.Enabled = true;
                        btnUpdate.Enabled = true;

                    }
                    else
                    {
                        btnCurrentDelete.Enabled = false;
                        btnUpdate.Enabled = false;

                    }
                }
            }
        }

        private void cmb_State_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmb_State.SelectedValue == null)
                return;

            if (cmb_State.SelectedValue.ToString() == "처리완료")
            {

                dtp_ProcessDate.Enabled = true;
                dtp_ProcessDate.Value = DateTime.Now;

            }
            else
            {

                dtp_ProcessDate.Enabled = false;
                dtp_ProcessDate.Value = DateTime.Now;
            }
        }

        private void txt_Contents_TextChanged(object sender, EventArgs e)
        {
            #region 텍스트 길이 체크
            try
            {
                char[] msg_chars = this.txt_Contents.Text.ToCharArray();
                int len = 0;
                foreach (char msg_char in msg_chars)
                {
                    if (char.IsDigit(msg_char) || char.IsWhiteSpace(msg_char) || char.IsUpper(msg_char) || char.IsLower(msg_char))
                    {
                        len++;
                    }
                    else
                    {
                        len += 2;
                    }
                }
                this.lbl_txtLengh.Text = " " + len.ToString() + " / 5000 byte";
                if (len > 5000)
                {
                    MessageBox.Show("5000 byte를 초과하여 입력할 수 없습니다.");
                    this.txt_Contents.Text = this.txt_Contents.Text.Remove(txt_Contents.Text.Length - 1);
                }
            }
            catch (Exception ex)
            {
                // MessageManager.ShowMessage(MessageType.Error, "오류가 발생했습니다.", ex, false);
            }
            #endregion
        }

        private void txt_ProcessContents_TextChanged(object sender, EventArgs e)
        {
            #region 텍스트 길이 체크
            try
            {
                char[] msg_chars = this.txt_ProcessContents.Text.ToCharArray();
                int len = 0;
                foreach (char msg_char in msg_chars)
                {
                    if (char.IsDigit(msg_char) || char.IsWhiteSpace(msg_char) || char.IsUpper(msg_char) || char.IsLower(msg_char))
                    {
                        len++;
                    }
                    else
                    {
                        len += 2;
                    }
                }
                this.lbl_txtLengh2.Text = " " + len.ToString() + " / 2000 byte";
                if (len > 2000)
                {
                    MessageBox.Show("2000 byte를 초과하여 입력할 수 없습니다.");
                    this.txt_ProcessContents.Text = this.txt_ProcessContents.Text.Remove(txt_ProcessContents.Text.Length - 1);
                }
            }
            catch (Exception ex)
            {
                // MessageManager.ShowMessage(MessageType.Error, "오류가 발생했습니다.", ex, false);
            }
            #endregion
        }

        private void btn_FileView2_Click(object sender, EventArgs e)
        {
            var Selected = ((DataRowView)cardPayASBindingSource.Current).Row as CMDataSet.CardPayASRow;
            if (Selected == null || String.IsNullOrEmpty(Selected.FilePath1))
                return;
            Bitmap _b = null;
            PrintDocument pDoc = new PrintDocument();
            PageSettings ps = new PageSettings();
            ps.Margins = new Margins(10, 10, 10, 10);
            pDoc.DefaultPageSettings = ps;
            PrintPreviewDialog ppDoc = new PrintPreviewDialog();
            ppDoc.ClientSize = new System.Drawing.Size(500, 500);

            //   ppDoc.ClientSize = new System.Drawing.Size(pic_BizPaper.Image.Width, pic_BizPaper.Image.Height);
            ppDoc.UseAntiAlias = true;
            pDoc.PrintPage += new PrintPageEventHandler((_sender, _e) =>
            {
                if (_b.Width > _b.Height)
                {
                    _b.RotateFlip(RotateFlipType.Rotate90FlipNone);
                }
                var width = _b.Width;
                var height = _b.Height;
                if (_b.Width > _e.MarginBounds.Width)
                {
                    width = _e.MarginBounds.Width;
                    height = height * _e.MarginBounds.Width / _b.Width;
                }
                else if (_b.Height > _e.MarginBounds.Height)
                {
                    height = _e.MarginBounds.Height;
                    width = width * _e.MarginBounds.Height / _b.Height;
                }
                _e.Graphics.DrawImage(_b, 0, 0, width, height);

            });
            ppDoc.Document = pDoc;
            ((Form)ppDoc).WindowState = FormWindowState.Maximized;
            Task.Factory.StartNew(() =>
            {
                WebClient mWebClient = new WebClient();
                try
                {
                    var b = mWebClient.DownloadData("http://m.cardpay.kr/ImageFromAdmin/GetASPaper2?AsId=" + Selected.AsId.ToString());
                    MemoryStream ms = new MemoryStream();
                    ms.Write(b, 0, b.Length);
                    ms.Position = 0;
                    _b = new Bitmap(ms);
                }
                catch (Exception)
                {
                    Invoke(new Action(() => MessageBox.Show("이미지를 가져오는 중 오류가 발생하였습니다. 잠시 후 다시 이용바랍니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop)));
                    return;
                }
                Invoke(new Action(() => ppDoc.Show()));
            });
        }

        private void btn_File2_Click(object sender, EventArgs e)
        {
            var Selected = ((DataRowView)cardPayASBindingSource.Current).Row as CMDataSet.CardPayASRow;
            if (Selected == null)
                return;
            dlgOpen.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            if (dlgOpen.ShowDialog() != DialogResult.OK) return;



            using (MemoryStream ms = new MemoryStream())
            using (MemoryStream mm = new MemoryStream())
            {
                Bitmap _b = null;
                try
                {
                    var b = File.ReadAllBytes(dlgOpen.FileName);
                    ms.Write(b, 0, b.Length);
                    ms.Position = 0;
                    _b = new Bitmap(ms);
                }
                catch (Exception)
                {
                    MessageBox.Show("읽을 수 없는 이미지 파일입니다. 다른 파일을 선택해주십시오.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }

                try
                {
                    _b.Save(mm, System.Drawing.Imaging.ImageFormat.Png);
                    byte[] bytes = mm.ToArray();

                    AndroidImageViewModel i = new Admin.AndroidImageViewModel();
                    i.DriverId = 0;
                    i.ImageData64String = System.Convert.ToBase64String(bytes);

                    var http = (HttpWebRequest)WebRequest.Create(new Uri("http://m.cardpay.kr/ImageFromApp/SetASPaper"));
                    http.Accept = "application/json";
                    http.ContentType = "application/json";
                    http.Method = "POST";

                    string parsedContent = JsonConvert.SerializeObject(i);
                    ASCIIEncoding encoding = new ASCIIEncoding();
                    Byte[] b = encoding.GetBytes(parsedContent);

                    Stream newStream = http.GetRequestStream();
                    newStream.Write(b, 0, b.Length);
                    newStream.Close();

                    var response = http.GetResponse();

                    var stream = response.GetResponseStream();
                    var sr = new StreamReader(stream);
                    var content = sr.ReadToEnd();
                    var o = (dynamic)JsonConvert.DeserializeObject(content);
                    if ((bool)o.Result)
                    {
                        txt_FileName2.Text = System.IO.Path.GetFileName(dlgOpen.FileName);
                        Selected.FileName2 = txt_FileName2.Text;
                        Selected.FilePath2 = (string)o.FilePath;
                        cardPayASTableAdapter.Update(Selected);
                        MessageBox.Show("이미지 전송이 완료 되었습니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("이미지를 전송 중 오류가 발생하였습니다. 잠시후 다시 시도해주시기 바랍니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("이미지를 전송 중 오류가 발생하였습니다. 잠시후 다시 시도해주시기 바랍니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
            }
        }
    }



}
