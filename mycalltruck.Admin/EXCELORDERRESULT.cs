using mycalltruck.Admin.Class.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using mycalltruck.Admin.DataSets;

using mycalltruck.Admin.Class;
using Newtonsoft.Json.Linq;
using System.Net;
using System.IO;
using System.Data.SqlClient;

namespace mycalltruck.Admin
{
    public partial class EXCELORDERRESULT : Form
    {
        CargoApiClass cargoApiClass = new CargoApiClass();


        public EXCELORDERRESULT(string RequestCnt,string Success_cnt,DataTable dataTable)
        {
            InitializeComponent();

            label4.Text = RequestCnt;
            label5.Text = Success_cnt;
        }
        string ApiUrl = "";
        string ApiGubun = "";
        string CargoId = "";
        private string _ORDERCODE;
        string _SHA256HASH = "";
        string Key = "783AE1841E52783A";
        string CargoStatus = "";

        string code = "";
        string msg = "";
        private void EXCELRESULT_Load(object sender, EventArgs e)
        {


            if (MessageBox.Show($"화물맨으로 연동하시겠습니까?", "화물맨 연동", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            {


            }
            else
            {
                using (ShareOrderDataSet ShareOrderDataSet = new ShareOrderDataSet())
                {

                }
                   // setCargoRegist("등록", Current.OrderId);

            }


        }
        private string _AddressStateParse(String _Value)
        {
            string R = "";
            switch (_Value)
            {
                case "강원도":
                    R = "강원";
                    break;
                case "경기도":
                    R = "경기";
                    break;
                case "경상남도":
                    R = "경남";
                    break;
                case "경상북도":
                    R = "경북";
                    break;
                case "광주광역시":
                    R = "광주";
                    break;
                case "대구광역시":
                    R = "대구";
                    break;
                case "대전광역시":
                    R = "대전";
                    break;
                case "부산광역시":
                    R = "부산";
                    break;
                case "서울특별시":
                    R = "서울";
                    break;
                case "세종특별자치시":
                    R = "세종";
                    break;
                case "울산광역시":
                    R = "울산";
                    break;
                case "인천광역시":
                    R = "인천";
                    break;
                case "전라남도":
                    R = "전남";
                    break;
                case "전라북도":
                    R = "전북";
                    break;
                case "제주특별자치도":
                    R = "제주";
                    break;
                case "충청남도":
                    R = "충남";
                    break;
                case "충청북도":
                    R = "충북";
                    break;
                default:
                    break;
            }
            return R;
        }

        private string _AddressCityParse(String _Value)
        {
            string R = _Value;
            if (_Value == "제주시")
            {
                R = "제주시";
            }
            else
            {
                if (_Value.Length > 2)
                    R = _Value.Substring(0, _Value.Length - 1);
                //var _Values = _Value.Split(' ');

                //if (_Values.Length > 1)
                //{
                //    if (_Values[0].Length > 2)
                //        R = _Values[0].Substring(0, _Values[0].Length - 1)+" "+ _Values[1]


                //}
                //else
                //{
                //    if (_Value.Length > 2)
                //        R = _Value.Substring(0, _Value.Length - 1);
                //}


            }
            return R;

            //string R = _Value;
            //if (_Value.Length > 2)
            //    R = _Value.Substring(0, _Value.Length - 1);
            //return R;
        }



        private void setCargoRegist(string CargoGubun, int _Orderid = 0, int _CarCountValue = 0, int _i = 0)
        {
            string result;
            var _CargoAddrSearch = cargoDataSet.CargoApiAdress.ToArray();
            LocalUser.Instance.LogInInformation.LoadClient();
            //CargoId = LocalUser.Instance.LogInInformation.Client.CargoLoginId;
            CargoId = LocalUser.Instance.LogInInformation.CargoLoginId;
            if (CargoId == "")
            {
                MessageBox.Show("내정보에 화물맨 아이디를 등록하세요.");

                FrmMDI.LoadForm("FrmMN0204_CARGOOWNERMANAGE", "");
                return;
            }

            if (_Orderid != 0)
            {
                _ORDERCODE = _Orderid.ToString();
            }
            var _Carsize = "";// CarSize.Text;
            string CarTypechange = "";
            string LOACITY = "";
            string LOACODE = "";
            string POIX = "";
            string POIY = "";

            string DOWCODE = "";
            string DOWCITY = "";
            string POIX_OUT = "";
            string POIY_OUT = "";
            using (ShareOrderDataSet ShareOrderDataSet = new ShareOrderDataSet())
            {
                Int32 IOrderCode = Convert.ToInt32(_ORDERCODE);
                var CargoOrder = ShareOrderDataSet.Orders.Where(c => c.OrderId == IOrderCode).FirstOrDefault();

                #region 상차지
                string startaddr1 = cargoApiClass.Get_addr_search1(_AddressStateParse(CargoOrder.StartState), ApiUrl);
                //     string startaddr2 = "";
                string startaddr3 = "";
                string _StartStreet = "";
                string _StartDetail = "";
                string _startcity = "";


               
                if (string.IsNullOrEmpty(_StartStreet))
                {
                    var EditStartStreet = CargoOrder.StartStreet;
                    if (!string.IsNullOrEmpty(EditStartStreet))
                    {
                        if (EditStartStreet.Length > 2)
                        {
                            if (EditStartStreet == "동남구" && EditStartStreet == "서북구")
                            {

                            }
                            else
                            {
                                if (CargoOrder.StartStreet.Substring(CargoOrder.StartStreet.Length - 1, 1) == "시" || CargoOrder.StartStreet.Substring(CargoOrder.StartStreet.Length - 1, 1) == "구" || CargoOrder.StartStreet.Substring(CargoOrder.StartStreet.Length - 1, 1) == "읍")
                                {
                                    EditStartStreet = _AddressCityParse(EditStartStreet);
                                }
                            }
                        }
                    }

                    //화물맨주소검색
                    var _Q = _CargoAddrSearch.Where(c => c.SIDO + "" + c.GU + "" + c.GUN + "" + c.DONG == _AddressStateParse(CargoOrder.StartState) + "" + _AddressCityParse(CargoOrder.StartCity) + "" + EditStartStreet);

                    if (_Q.Any())
                    {
                        LOACODE = _Q.First().AREACODE;
                        POIX = _Q.First().POIX;
                        POIY = _Q.First().POIY;
                        LOACITY = _Q.First().SIDO + " " + _Q.First().GU + " " + _Q.First().GUN + " " + _Q.First().DONG;
                        LOACITY = LOACITY.Replace("  ", " ");
                    }
                    else
                    {

                        if (!String.IsNullOrEmpty(CargoOrder.StartState) && !String.IsNullOrEmpty(CargoOrder.StartCity))
                        {
                            CargoAddress cargoAddress = new CargoAddress(CargoOrder.StartState, CargoOrder.StartCity, CargoOrder.StartStreet);
                            cargoAddress.StartPosition = FormStartPosition.CenterParent;

                            if (cargoAddress.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                            {

                                LOACODE = cargoAddress._AREACODE;
                                POIX = cargoAddress._POIX;
                                POIY = cargoAddress._POIY;


                                LOACITY = cargoAddress._SIDO + " " + cargoAddress._Gu_Gun + " " + cargoAddress._DONG;

                                LOACITY = LOACITY.Replace("  ", " ");
                            }
                            else
                            {
                                MessageBox.Show($"\"화물맨\" {CargoGubun}에 실패하였습니다\r\n존재하지않는 주소입니다.", "화물맨 연동", MessageBoxButtons.OK);
                                return;
                            }
                        }
                        else
                        {
                            MessageBox.Show($"\"화물맨\" {CargoGubun}에 실패하였습니다\r\n존재하지않는 주소입니다.", "화물맨 연동", MessageBoxButtons.OK);
                            return;
                        }
                    }

                }
                else
                {
                    if (!string.IsNullOrEmpty(_StartStreet))
                    {
                        var _StartCitys = _StartStreet.Split(' ');


                        //if (_StartCitys[0] == "제주시")
                        //{
                        //    _StartCitys[0] = "제주시시";
                        //}



                        if (_StartCitys.Length == 3)
                        {
                            //if (_StartCitys[0].Length > 2)
                            //{
                            //    _startcity = _StartCitys[0].Substring(0, _StartCitys[0].Trim().Length - 1) + _StartCitys[1];
                            //}
                            //else
                            //{
                            //    _startcity = _StartCitys[0].Trim() + _StartCitys[1];

                            //}
                            _startcity = _AddressCityParse(_StartCitys[0]) + _StartCitys[1];

                            _StartDetail = _StartCitys[2];
                        }
                        else
                        {
                            //if (_StartCitys[0].Length > 2)
                            //{
                            //    _startcity = _StartCitys[0].Substring(0, _StartCitys[0].Trim().Length - 1);
                            //}
                            //else
                            //{
                            //    _startcity = _StartCitys[0].Trim();

                            //}
                            _startcity = _AddressCityParse(_StartCitys[0]);
                            _StartDetail = _StartCitys[1];
                        }

                    }
                    else
                    {

                        MessageBox.Show($"\"화물맨\" {CargoGubun}에 실패하였습니다\r\n존재하지않는 주소입니다.", "화물맨 연동", MessageBoxButtons.OK);
                        return;


                    }


                    if (!String.IsNullOrEmpty(startaddr1))
                    {
                        if (_startcity.Contains("고양일산"))
                        {
                            _startcity = "고양일산";
                        }
                        startaddr3 = cargoApiClass.get_address_search(_AddressStateParse(CargoOrder.StartState) + "" + _startcity + "" + _StartDetail, _StartDetail, ApiUrl);

                    }
                    else
                    {
                        MessageBox.Show($"\"화물맨\" {CargoGubun}에 실패하였습니다\r\n화물맨 주소 매칭불가", "화물맨 연동", MessageBoxButtons.OK);

                        return;
                    }





                    if (String.IsNullOrEmpty(startaddr3))
                    {
                        MessageBox.Show($"\"화물맨\" {CargoGubun}에 실패하였습니다\r\n화물맨 주소 매칭불가", "화물맨 연동", MessageBoxButtons.OK);

                        return;
                    }
                    var ss = startaddr3.Split(',');

                    LOACODE = ss[0];
                    POIX = ss[1];
                    POIY = ss[2];


                    LOACITY = ss[3] + " " + ss[4] + " " + ss[5] + " " + ss[6];

                    LOACITY = LOACITY.Replace("  ", " ");
                }



                #endregion 출발지

                #region 하차지

                string stopaddr1 = cargoApiClass.Get_addr_search1(_AddressStateParse(CargoOrder.StopState), ApiUrl);
                //  string stopaddr2 = "";
                string stopaddr3 = "";
                string _Stopstreet = "";
                string _StopDetail = "";

                string _stopcity = "";

              
                if (string.IsNullOrEmpty(_Stopstreet))
                {
                    var EditStopStreet = CargoOrder.StopStreet;
                    if (!string.IsNullOrEmpty(EditStopStreet))
                    {
                        if (EditStopStreet.Length > 2)
                        {
                            if (EditStopStreet == "동남구" && EditStopStreet == "서북구")
                            {

                            }
                            else
                            {
                                if (CargoOrder.StopStreet.Substring(CargoOrder.StopStreet.Length - 1, 1) == "시" || CargoOrder.StopStreet.Substring(CargoOrder.StopStreet.Length - 1, 1) == "구" || CargoOrder.StopStreet.Substring(CargoOrder.StopStreet.Length - 1, 1) == "읍")
                                {
                                    EditStopStreet = _AddressCityParse(EditStopStreet);
                                }
                            }
                        }
                    }


                    //화물맨주소검색
                    var _Q = _CargoAddrSearch.Where(c => c.SIDO + "" + c.GU + "" + c.GUN + "" + c.DONG == _AddressStateParse(CargoOrder.StopState) + "" + _AddressCityParse(CargoOrder.StopCity) + "" + EditStopStreet);

                    if (_Q.Any())
                    {
                        DOWCODE = _Q.First().AREACODE;
                        POIX_OUT = _Q.First().POIX;
                        POIY_OUT = _Q.First().POIY;
                        DOWCITY = _Q.First().SIDO + " " + _Q.First().GU + " " + _Q.First().GUN + " " + _Q.First().DONG;
                        DOWCITY = DOWCITY.Replace("  ", " ");
                    }

                    else
                    {
                        if (!String.IsNullOrEmpty(CargoOrder.StopState) && !String.IsNullOrEmpty(CargoOrder.StopCity))
                        {

                            CargoAddress cargoAddress = new CargoAddress(CargoOrder.StopState, CargoOrder.StopCity, CargoOrder.StopStreet);
                            cargoAddress.StartPosition = FormStartPosition.CenterParent;
                            if (cargoAddress.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                            {

                                // _Stopstreet = cargoApiClass.roadaddr_trans(CargoOrder.StopState + " " + CargoOrder.StopCity + " " + cargoAddress._DONG);
                                DOWCODE = cargoAddress._AREACODE;
                                POIX_OUT = cargoAddress._POIX;
                                POIY_OUT = cargoAddress._POIY;


                                DOWCITY = cargoAddress._SIDO + " " + cargoAddress._Gu_Gun + " " + cargoAddress._DONG;

                                DOWCITY = DOWCITY.Replace("  ", " ");





                            }
                            else
                            {
                                MessageBox.Show($"\"화물맨\" {CargoGubun}에 실패하였습니다\r\n존재하지않는 주소입니다.", "화물맨 연동", MessageBoxButtons.OK);
                                return;
                            }
                        }
                        else
                        {
                            MessageBox.Show($"\"화물맨\" {CargoGubun}에 실패하였습니다\r\n존재하지않는 주소입니다.", "화물맨 연동", MessageBoxButtons.OK);
                            return;
                        }
                    }


                }
                else
                {
                    if (!string.IsNullOrEmpty(_Stopstreet))
                    {
                        var _StopCitys = _Stopstreet.Split(' ');


                        //if (_StopCitys[0] == "제주시")
                        //{
                        //    _StopCitys[0] = "제주시시";
                        //}


                        if (_StopCitys.Length == 3)
                        {
                            //if (_StopCitys[0].Length > 2)
                            //{
                            //    _stopcity = _StopCitys[0].Substring(0, _StopCitys[0].Trim().Length - 1) + _StopCitys[1];
                            //}
                            //else
                            //{
                            //    _stopcity = _StopCitys[0].Trim() + _StopCitys[1];

                            // }

                            _stopcity = _AddressCityParse(_StopCitys[0]) + _StopCitys[1];


                            _StopDetail = _StopCitys[2];

                        }
                        else
                        {
                            //if (_StopCitys[0].Length > 2)
                            //{
                            //    _stopcity = _StopCitys[0].Substring(0, _StopCitys[0].Trim().Length - 1);
                            //}
                            //else
                            //{
                            //    _stopcity = _StopCitys[0].Trim();

                            //}

                            _stopcity = _AddressCityParse(_StopCitys[0]);

                            _StopDetail = _StopCitys[1];
                        }

                    }
                    else
                    {
                        MessageBox.Show($"\"화물맨\" {CargoGubun}에 실패하였습니다\r\n존재하지않는 주소입니다.", "화물맨 연동", MessageBoxButtons.OK);

                        return;
                    }


                    if (!String.IsNullOrEmpty(stopaddr1))
                    {
                        if (_stopcity.Contains("일산"))
                        {
                            _stopcity = "일산";
                        }
                        // startaddr2 = cargoApiClass.get_addr_search2(startaddr1, _startcity);
                        stopaddr3 = cargoApiClass.get_address_search(_AddressStateParse(CargoOrder.StopState) + "" + _stopcity + "" + _StopDetail, _StopDetail, ApiUrl);
                        // startaddr =  cargoApiClass.get_addr_searchname(startaddr2, _startcity, _AddressStateParse(CargoOrder.StartState), _StartStreet);
                    }
                    else
                    {
                        MessageBox.Show($"\"화물맨\" {CargoGubun}에 실패하였습니다\r\n화물맨 주소 매칭불가", "화물맨 연동", MessageBoxButtons.OK);

                        return;
                    }





                    if (String.IsNullOrEmpty(stopaddr3))
                    {
                        MessageBox.Show($"\"화물맨\" {CargoGubun}에 실패하였습니다\r\n화물맨 주소 매칭불가", "화물맨 연동", MessageBoxButtons.OK);

                        return;
                    }
                    var st = stopaddr3.Split(',');



                    DOWCODE = st[0];

                    POIX_OUT = st[1];
                    POIY_OUT = st[2];

                    DOWCITY = st[3] + " " + st[4] + " " + st[5] + " " + st[6];
                    DOWCITY = DOWCITY.Replace("  ", " ");
                }





                #endregion 하차지

                #region 선/착불
                if (CargoOrder.PayLocation == 2)
                {
                    if (CargoOrder.StartPrice != 0 && CargoOrder.StopPrice != 0)
                    {
                        MessageBox.Show($"\"화물맨\" 연동시에는\r\n선불금액 또는 착불금액 한가지만 입력하셔야 합니다.\r\n수정후 화물맨에 등록하시기바랍니다.", "화물맨 연동", MessageBoxButtons.OK);

                        return;
                    }
                }
                #endregion

            }

            CarTypechange = "";//CarType.Text;
            pnProgress.Visible = true;
            Task.Factory.StartNew(() =>
            {
                bar.Invoke(new Action(() =>
                {
                    bar.PerformStep();
                }));
                #region
                var MOVETYPELIST = cargoApiClass.Get_code_config("MOVETYPE", ApiUrl);

                var TONLIST = cargoApiClass.Get_code_config("TON", ApiUrl);

                var CARTYPELIST = cargoApiClass.Get_code_config("CARTYPE", ApiUrl);

                var ALONELIST = cargoApiClass.Get_code_config("ALONE", ApiUrl);

                var STATELIST = cargoApiClass.Get_code_config("STATE", ApiUrl);

                var PAYMETHODLIST = cargoApiClass.Get_code_config("PAYMETHOD", ApiUrl);


                using (ShareOrderDataSet ShareOrderDataSet = new ShareOrderDataSet())
                {
                    Int32 IOrderCode = Convert.ToInt32(_ORDERCODE);
                    var CargoOrder = ShareOrderDataSet.Orders.Where(c => c.OrderId == IOrderCode).FirstOrDefault();


                    //주소찾기




                    _SHA256HASH = DateTime.Now.ToString("yyyyMMddHHmmss") + CargoApiClass.SHA256Hash(DateTime.Now.ToString("yyyyMMddHHmmss") + Key);







                   

                    
                    string CARTON = "";
                    //var _CARTON = TONLIST.Where(c => c.VALUE.Replace("톤", "") == CarSize.Text).ToArray();
                    decimal dCarsize = 0;
                    if (decimal.TryParse(_Carsize, out dCarsize))
                    {


                        if (dCarsize == 7.5m)
                        {
                            dCarsize = 8m;
                        }
                        else if (dCarsize == 15m)
                        {
                            dCarsize = 16m;
                        }
                        _Carsize = dCarsize.ToString() + "톤";

                        if (dCarsize < 1)
                        {
                            _Carsize = "1톤미만";
                        }
                    }


                    var _CARTON = TONLIST.Where(c => c.VALUE == _Carsize).ToArray();

                    if (_CARTON.Any())
                    {

                        CARTON = TONLIST.Where(c => c.VALUE == _Carsize).FirstOrDefault().KEY;

                    }
                    else
                    {

                        CARTON = TONLIST.Where(c => c.VALUE == "기타").FirstOrDefault().KEY;

                    }



                    if (CarTypechange == "카고+윙바디")
                    {
                        CarTypechange = "카고/윙바디";
                    }
                    else if (CarTypechange == "츄레라")
                    {
                        CarTypechange = "추레라";
                    }

                    var _CARTYPE = CARTYPELIST.Where(c => c.VALUE == CarTypechange).ToArray();
                    string CARTYPE = "";
                    if (_CARTYPE.Any())
                    {
                        CARTYPE = CARTYPELIST.Where(c => c.VALUE == CarTypechange).FirstOrDefault().KEY;



                    }
                    else
                    {
                        CARTYPE = CARTYPELIST.Where(c => c.VALUE == "카고").FirstOrDefault().KEY;
                    }

                    var LOADTYPE = "";
                    //var OWID = LocalUser.Instance.LogInInformation.Client.CargoLoginId;  //"화물맨";
                    var OWID = LocalUser.Instance.LogInInformation.CargoLoginId;  //"화물맨";
                    var PAYMENT = "";
                    string SATYPE = "";
                    string HATYPE = "";
                    string PAY = "";
                    string FEE = "";
                    if (CargoOrder.IsShared)
                    {
                        LOADTYPE = ALONELIST.Where(c => c.VALUE == "혼적").FirstOrDefault().KEY;
                    }
                    else
                    {
                        LOADTYPE = ALONELIST.Where(c => c.VALUE == "독차").FirstOrDefault().KEY;
                    }




                    switch (CargoOrder.PayLocation)
                    {
                        //인수증
                        case 1:
                            PAYMENT = PAYMETHODLIST.Where(c => c.VALUE == "인수증").FirstOrDefault().KEY;

                            break;
                        //선/착불
                        case 2:
                            PAYMENT = PAYMETHODLIST.Where(c => c.VALUE == "선불").FirstOrDefault().KEY;
                            break;
                        //인수증+선/착불
                        case 4:
                            PAYMENT = PAYMETHODLIST.Where(c => c.VALUE == "선불").FirstOrDefault().KEY;
                            break;
                        default:
                            PAYMENT = PAYMETHODLIST.Where(c => c.VALUE == "인수증").FirstOrDefault().KEY;
                            break;
                    }

                    if (String.IsNullOrEmpty(CargoOrder.StartInfo))
                    {
                        SATYPE = MOVETYPELIST.Where(c => c.VALUE == "일반").FirstOrDefault().KEY;
                    }
                    else
                    {
                        SATYPE = MOVETYPELIST.Where(c => c.VALUE == CargoOrder.StartInfo).FirstOrDefault().KEY;
                    }

                    if (String.IsNullOrEmpty(CargoOrder.StopInfo))
                    {
                        HATYPE = MOVETYPELIST.Where(c => c.VALUE == "일반").FirstOrDefault().KEY;
                    }
                    else
                    {
                        HATYPE = MOVETYPELIST.Where(c => c.VALUE == CargoOrder.StopInfo).FirstOrDefault().KEY;
                    }



                    string WEIGHT = "";
                    var CargoCarSize = SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "CarSize" && c.Value == CargoOrder.CarSize).ToArray();


                    if (!String.IsNullOrEmpty(CargoOrder.ItemSize) && CargoOrder.ItemSize != "0")
                    {
                        WEIGHT = CargoOrder.ItemSize;

                    }
                    else
                    {
                        WEIGHT = (CargoCarSize.FirstOrDefault().Number * 1.1d).ToString("N1");
                    }


                    string url = "";
                    if (CargoGubun == "수정")
                    {
                        url = $"{ApiUrl}/service2/csr/set_cargo_update";
                    }
                    else
                    {
                        url = $"{ApiUrl}/service2/csr/set_cargo_regist";
                    }
                    if (PAYMENT == "payMethod01")
                    {
                        //착불
                        if (CargoOrder.StartPrice == 0)
                        {
                            //착불
                            PAYMENT = "payMethod03";
                            PAY = Convert.ToInt64(CargoOrder.StopPrice - CargoOrder.DriverPrice).ToString().Replace(",", "");
                        }
                        //선불
                        else if (CargoOrder.StopPrice == 0)
                        {

                            PAY = Convert.ToInt64(CargoOrder.StartPrice - CargoOrder.DriverPrice).ToString().Replace(",", "");
                        }
                        //선착불
                        else if (CargoOrder.StartPrice != 0 && CargoOrder.StopPrice != 0)
                        {
                            PAY = Convert.ToInt64(CargoOrder.StartPrice + CargoOrder.StopPrice - CargoOrder.DriverPrice).ToString().Replace(",", "");
                        }

                    }
                    //선/착불이 아닌경우
                    else
                    {
                        //PAY = Convert.ToInt64(CargoOrder.ClientPrice).ToString().Replace(",", "");
                        //지불운임/기사운임
                        PAY = Convert.ToInt64(CargoOrder.TradePrice + CargoOrder.AlterPrice).ToString().Replace(",", "");
                    }

                    string json = "{" +
                                    "\"COMCODE\":\"207\"," +
                                    "\"ORDERCODE\":\"" + CargoOrder.OrderId + "\"" +
                                    ",\"HASH\":\"" + _SHA256HASH + "\"" +
                                    ",\"LOADAY\":\"" + CargoOrder.StartTime.ToString("yyyy-MM-dd HH:mm") + "\"" +
                                    ",\"LOACITY\":\"" + LOACITY + "\"" +//+ CargoOrder.StartState + " " + CargoOrder.StartCity + " " + CargoOrder.StartStreet + "\"" +
                                    ",\"LOACODE\":\"" + LOACODE + "\"" +
                                    ",\"POIX\":\"" + POIX + "\"" +
                                    ",\"POIY\":\"" + POIY + "\"" +
                                    ",\"DOWDAY\":\"" + CargoOrder.StopTime.ToString("yyyy-MM-dd HH:mm") + "\"" +
                                    ",\"DOWCITY\":\"" + DOWCITY + "\"" +//+ CargoOrder.StopState + " " + CargoOrder.StopCity + " " + CargoOrder.StopStreet + "\"" +
                                    ",\"DOWCODE\":\"" + DOWCODE + "\"" +
                                    ",\"POIX_OUT\":\"" + POIX_OUT + "\"" +
                                    ",\"POIY_OUT\":\"" + POIY_OUT + "\"" +
                                    ",\"CARTON\":\"" + CARTON + "\"" +
                                    ",\"CARTYPE\":\"" + CARTYPE + "\"" +
                                    ",\"LOADTYPE\":\"" + LOADTYPE + "\"" +
                                    ",\"OWID\":\"" + OWID + "\"" +
                                    ",\"OWNAME\":\"" + LocalUser.Instance.LogInInformation.Client.Name + "\"" +
                                    ",\"PAYMENT\":\"" + PAYMENT + "\"" +
                                    ",\"PAY\":\"" + PAY + "\"" +
                                    ",\"FEE\":\"" + Convert.ToInt64(CargoOrder.DriverPrice).ToString().Replace(",", "") + "\"" +
                                    ",\"INFO\":\"" + CargoOrder.Remark + "\"" +
                                    ",\"ETC\":\"" + CargoOrder.Etc + "\"" +
                                    ",\"PHONE\":\"" + LocalUser.Instance.LogInInformation.Client.PhoneNo + "\"" +
                                    ",\"SATYPE\":\"" + SATYPE + "\"" +
                                    ",\"HATYPE\":\"" + HATYPE + "\"" +
                                    ",\"WEIGHT\":\"" + WEIGHT + "\"" +

                                  "}";
                    JObject response = null;
                    try
                    {
                        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                        request.ContentType = "application/json";
                        request.Method = "POST";

                        using (StreamWriter stream = new StreamWriter(request.GetRequestStream()))
                        {
                            stream.Write(json);
                            stream.Flush();
                            stream.Close();
                        }

                        HttpWebResponse httpResponse = (HttpWebResponse)request.GetResponse();
                        using (StreamReader reader = new StreamReader(httpResponse.GetResponseStream()))
                        {

                            result = reader.ReadToEnd();


                            response = JObject.Parse(result);

                            msg = response["msg"].ToString();
                            code = response["code"].ToString();

                            if (code == "200")
                            {
                                Data.Connection(_Connection =>
                                {
                                    using (SqlCommand _Command = _Connection.CreateCommand())
                                    {
                                        _Command.CommandText = "Update Orders SET CargoApiYN = 'Y' , CargoApiStatus = @CargoGubun WHERE OrderId = @OrderId ";

                                        _Command.Parameters.AddWithValue("@OrderId", CargoOrder.OrderId);
                                        _Command.Parameters.AddWithValue("@CargoGubun", CargoGubun);

                                        _Command.ExecuteNonQuery();
                                    }
                                });

                                if (String.IsNullOrEmpty(CargoStatus))
                                {
                                    //if (_CarCountValue != 0 && _i != 0)
                                    //{
                                    //    if (_CarCountValue == _i + 1)
                                    //    {
                                    //        MessageBox.Show($"\"화물맨\"에 {CargoGubun}되었습니다", "화물맨 연동", MessageBoxButtons.OK);
                                    //    }
                                    //}
                                    //else
                                    //{
                                    MessageBox.Show($"\"화물맨\"에 {CargoGubun}되었습니다", "화물맨 연동", MessageBoxButtons.OK);

                                    //}
                                }



                            }
                            else
                            {
                                if (msg.Contains("ordercode"))
                                {
                                    MessageBox.Show("화물번호가 중복되었습니다");
                                }
                                else if (msg.Contains("loaday"))
                                {
                                    MessageBox.Show("상차시간이 올바르지 않습니다.");
                                }
                                else if (msg.Contains("loacity"))
                                {
                                    MessageBox.Show("상차지가 올바르지 않습니다.");
                                }
                                else if (msg.Contains("dowday"))
                                {
                                    MessageBox.Show("하차시간이 올바르지 않습니다.");
                                }
                                else if (msg.Contains("dowcity"))
                                {
                                    MessageBox.Show("하차지가 올바르지 않습니다.");
                                }
                                else if (msg.Contains("carton"))
                                {
                                    MessageBox.Show("차량톤수 올바르지 않습니다.");
                                }
                                else if (msg.Contains("cartype"))
                                {
                                    MessageBox.Show("차량타입 올바르지 않습니다.");
                                }
                                else if (msg.Contains("loadtype"))
                                {
                                    MessageBox.Show("혼적구분이 올바르지 않습니다.");
                                }
                                else if (msg.Contains("ow_name"))
                                {
                                    MessageBox.Show("화주이름이 올바르지 않습니다.");
                                }
                                else if (msg.Contains("payment"))
                                {
                                    MessageBox.Show("결제방식이 올바르지 않습니다.");
                                }
                                else if (msg.Contains("pay"))
                                {
                                    MessageBox.Show("운임료가 올바르지 않습니다.");
                                }
                                else if (msg.Contains("fee"))
                                {
                                    MessageBox.Show("수수료가 올바르지 않습니다.");
                                }
                                else if (msg.Contains("info"))
                                {
                                    MessageBox.Show("화물정보가 올바르지 않습니다.");
                                }
                                else if (msg.Contains("etc"))
                                {
                                    MessageBox.Show("비고가 올바르지 않습니다.");
                                }
                                else if (msg.Contains("real_phone"))
                                {
                                    MessageBox.Show("핸드폰번호가 올바르지 않습니다.");
                                }
                                else if (msg.Contains("SangType"))
                                {
                                    MessageBox.Show("상차방법이 올바르지 않습니다.");
                                }
                                else if (msg.Contains("HaType"))
                                {
                                    MessageBox.Show("하차방법이 올바르지 않습니다.");
                                }
                                else if (msg.Contains("LocationCode"))
                                {
                                    MessageBox.Show("지역코드가 올바르지 않습니다.");
                                }
                                else if (msg.Contains("weight"))
                                {
                                    MessageBox.Show("화물실중량 값이 올바르지 않습니다.");
                                }
                                else
                                {
                                    MessageBox.Show(msg);
                                }

                            }



                        }


                    }
                    catch
                    {


                    }

                }
                #endregion


                Invoke(new Action(() => pnProgress.Visible = false));




            });

        }
        private void btn_OK_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
