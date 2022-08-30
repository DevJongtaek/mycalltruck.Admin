using mycalltruck.Admin.Class.Common;
using mycalltruck.Admin.DataSets;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace mycalltruck.Admin
{
    public partial class FrmTradePaperAdd : Form
    {
        int _TradeId = 0;
        int _DriverId = 0;

        TradeDataSet TradeDataSet = new TradeDataSet();
        
        string _SImage1, _SImage2, _SImage3, _SImage4, _SImage5;

        public FrmTradePaperAdd(int DriverId,int TradeId)
        {
            _DriverId = DriverId;
            _TradeId = TradeId;
            InitializeComponent();
        }

        private void FrmTradePaperAdd_Load(object sender, EventArgs e)
        {
            tradesTableAdapter1.Fill(TradeDataSet.Trades);
        }
        private String GetSelectCommand()
        {
           
            return @"SELECT distinct Trades.TradeId, Trades.RequestDate, BeginDate, EndDate, 
                trades.Item, Trades.Price, VAT, Amount, PayState, PayDate, Trades.PayBankName, Trades.PayBankCode, Trades.PayAccountNo, Trades.PayInputName, 
                Trades.DriverId, Trades.ClientId, Trades.UseTax, LGD_OID, 
                LGD_Result, LGD_Accept_Date, LGD_Cancel_Date, LGD_Last_Function, LGD_Last_Date, AllowAcc, HasAcc, ClientAccId, 
                SUMYN, SumPrice, SumVAT, SumAmount, SUMFROMDate, SUMToDate, 
                Image1, Image2, Image3, Image4, Image5, Image6, Image7, Image8, Image9, Image10, 
                HasETax, Trades.SourceType,
                Drivers.LoginId AS DriverLoginId, Drivers.CarNo AS DriverCarNo, Drivers.BizNo AS DriverBizNo, Drivers.Name AS DriverName, 
                Drivers.ServiceState, Drivers.MID, 
                Clients.Code AS ClientCode, Clients.Name AS ClientName, Trades.AcceptCount, Trades.SubClientId, Trades.ClientUserId
                ,(SELECT ISNULL(GroupName,'미설정') FROM  DriverInstances WHERE DriverId = Trades.DriverId and ClientId = Trades.ClientId) as GroupName
                ,Trades.ExcelExportYN,ISNULL(Trades.EtaxCanCelYN,'N') AS EtaxCanCelYN,trusteeMgtKey,TransportDate,Trades.StartState,Trades.StopState,Trades.PdfFileName,Trades.AipId,Trades.DocId,Trades.DeleteYn,Trades.UpdateDateTime
				,Orders.TradeId AS OTradeId,Orders.ReferralId
                FROM     Trades
                JOIN Drivers ON Trades.DriverId = Drivers.DriverId
                JOIN Clients ON Trades.ClientId = Clients.ClientId 
			    LEFT JOIN Orders ON Trades.TradeId = Orders.TradeId ";


        }

        private void LoadTable()
        {
            TradeDataSet.Trades.Clear();
            using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                _Connection.Open();
                using (SqlCommand _Command = _Connection.CreateCommand())
                {
                    String SelectCommandText = GetSelectCommand();

                    List<String> WhereStringList = new List<string>();
                    WhereStringList.Add("Trades.ClientId = @ClientId");
                    _Command.Parameters.AddWithValue("@ClientId", LocalUser.Instance.LogInInformation.ClientId);
                    


                    if (WhereStringList.Count > 0)
                    {
                        var WhereString = $"WHERE {String.Join(" AND ", WhereStringList)}";
                        SelectCommandText += Environment.NewLine + WhereString;

                    }

                    SelectCommandText += " Order by Trades.RequestDate Desc ";


                    
                    _Command.CommandText = SelectCommandText;


                    using (SqlDataReader _Reader = _Command.ExecuteReader())
                    {
                        TradeDataSet.Trades.Load(_Reader);


                    }
                }
                _Connection.Close();
            }

            
        }
        private void btnOk_Click(object sender, EventArgs e)
        {
            LoadTable();
            if (String.IsNullOrEmpty(dlgOpen.FileName))
            {
                MessageBox.Show("파일을 선택하세요");
                return;
            }
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
                    i.DriverId = _DriverId;
                    i.ImageData64String = System.Convert.ToBase64String(bytes);

                    var http = (HttpWebRequest)WebRequest.Create(new Uri("http://m.cardpay.kr/ImageFromApp/SetTradeImageAdmin"));
                   // var http = (HttpWebRequest)WebRequest.Create(new Uri("http://localhost/ImageFromApp/SetTradeImageAdmin"));
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
                       txt_TradePaper.Text = System.IO.Path.GetFileName(dlgOpen.FileName);
                        var _Image1 = $"/ImageFromAdmin/GetImage?ImageReferenceId={o.FilePath}";
                        //Selected.FileName2 = txt_TradePaper.Text;
                        //Selected.FilePath2 = (string)o.FilePath;
                        //cardPayASTableAdapter.Update(Selected);
                        
                        var Query = TradeDataSet.Trades.Where(c => c.TradeId == _TradeId);
                       

                        if(Query.Any())
                        {
                            _SImage1 = Query.First().Image1;
                            _SImage2 = Query.First().Image2;
                            _SImage3 = Query.First().Image3;
                            _SImage4 = Query.First().Image4;
                            _SImage5 = Query.First().Image5;
                        }

                        using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
                        {
                            _Connection.Open();
                            if (String.IsNullOrEmpty(_SImage1) && String.IsNullOrEmpty(_SImage2) && String.IsNullOrEmpty(_SImage3) && String.IsNullOrEmpty(_SImage4) && String.IsNullOrEmpty(_SImage5))
                            {
                                using (SqlCommand _Command = _Connection.CreateCommand())
                                {
                                    _Command.CommandText = "Update Trades SET Image1 = @Image WHERE TradeId = @TradeId";
                                    _Command.Parameters.AddWithValue("@TradeId", _TradeId);
                                    _Command.Parameters.AddWithValue("@Image", _Image1);
                                    _Command.ExecuteNonQuery();
                                }

                            }
                           else if (!String.IsNullOrEmpty(_SImage1) && String.IsNullOrEmpty(_SImage2) && String.IsNullOrEmpty(_SImage3) && String.IsNullOrEmpty(_SImage4) && String.IsNullOrEmpty(_SImage5))
                            {
                                using (SqlCommand _Command = _Connection.CreateCommand())
                                {
                                    _Command.CommandText = "Update Trades SET Image2 = @Image WHERE TradeId = @TradeId";
                                    _Command.Parameters.AddWithValue("@TradeId", _TradeId);
                                    _Command.Parameters.AddWithValue("@Image", _Image1);
                                    _Command.ExecuteNonQuery();
                                }

                            }
                            else if (!String.IsNullOrEmpty(_SImage1) && !String.IsNullOrEmpty(_SImage2) && String.IsNullOrEmpty(_SImage3) && String.IsNullOrEmpty(_SImage4) && String.IsNullOrEmpty(_SImage5))
                            {
                                using (SqlCommand _Command = _Connection.CreateCommand())
                                {
                                    _Command.CommandText = "Update Trades SET Image3 = @Image WHERE TradeId = @TradeId";
                                    _Command.Parameters.AddWithValue("@TradeId", _TradeId);
                                    _Command.Parameters.AddWithValue("@Image", _Image1);
                                    _Command.ExecuteNonQuery();
                                }

                            }
                            else if (!String.IsNullOrEmpty(_SImage1) && !String.IsNullOrEmpty(_SImage2) && !String.IsNullOrEmpty(_SImage3) && String.IsNullOrEmpty(_SImage4) && String.IsNullOrEmpty(_SImage5))
                            {
                                using (SqlCommand _Command = _Connection.CreateCommand())
                                {
                                    _Command.CommandText = "Update Trades SET Image4 = @Image WHERE TradeId = @TradeId";
                                    _Command.Parameters.AddWithValue("@TradeId", _TradeId);
                                    _Command.Parameters.AddWithValue("@Image", _Image1);
                                    _Command.ExecuteNonQuery();
                                }

                            }
                            else if (!String.IsNullOrEmpty(_SImage1) && !String.IsNullOrEmpty(_SImage2) && !String.IsNullOrEmpty(_SImage3) && !String.IsNullOrEmpty(_SImage4) && String.IsNullOrEmpty(_SImage5))
                            {
                                using (SqlCommand _Command = _Connection.CreateCommand())
                                {
                                    _Command.CommandText = "Update Trades SET Image5 = @Image WHERE TradeId = @TradeId";
                                    _Command.Parameters.AddWithValue("@TradeId", _TradeId);
                                    _Command.Parameters.AddWithValue("@Image", _Image1);
                                    _Command.ExecuteNonQuery();
                                }

                            }
                            else if (!String.IsNullOrEmpty(_SImage1) && !String.IsNullOrEmpty(_SImage2) && !String.IsNullOrEmpty(_SImage3) && String.IsNullOrEmpty(_SImage4) && !String.IsNullOrEmpty(_SImage5))
                            {
                                MessageBox.Show("이미지가 모두 등록되어있습니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                _Connection.Close();
                                this.Close();
                                return;

                            }
                            //using (SqlCommand _Command = _Connection.CreateCommand())
                            //{
                            //    _Command.CommandText = "Update Trades SET Image1 = @Image1 WHERE TradeId = @TradeId";
                            //    _Command.Parameters.AddWithValue("@TradeId", _TradeId);
                            //    _Command.Parameters.AddWithValue("@Image1", _Image1);
                            //    _Command.ExecuteNonQuery();
                            //}


                            _Connection.Close();
                        }

                        MessageBox.Show("이미지 전송이 완료 되었습니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("이미지를 전송 중 오류가 발생하였습니다. 잠시후 다시 시도해주시기 바랍니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message +"\r\n이미지를 전송 중 오류가 발생하였습니다. 잠시후 다시 시도해주시기 바랍니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
            }
        }

        private void btnNo_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_TradePaperAdd_Click(object sender, EventArgs e)
        {


            dlgOpen.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            if (dlgOpen.ShowDialog() != DialogResult.OK) return;

            txt_TradePaper.Text = System.IO.Path.GetFileName(dlgOpen.FileName);


           
        }
        
    }
}
