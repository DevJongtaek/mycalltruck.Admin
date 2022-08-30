using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace mycalltruck.Admin
{
    public partial class CargoAddress : Form
    {
        public string _SIDO = "";
        public string _DONG = "";
        public string _POIX = "";
        public string _POIY = "";
        public string _AREACODE = "";
        public string _Gu_Gun = "";
        public CargoAddress(string Sido, string Gu, string Gun)
        {
            InitializeComponent();

            //cargoApiAdressTableAdapter.Fill(cargoDataSet.CargoApiAdress);
            cargoApiAdressNewTableAdapter.Fill(cargoDataSet.CargoApiAdressNew);


            //var _Q = cargoDataSet.CargoApiAdress.Where(c => c.SIDO + " " + c.GU == _AddressStateParse(Sido) + " " + _AddressCityParse(Gu) && c.DONG != "").ToList();
            var _Q = cargoDataSet.CargoApiAdressNew.Where(c => c.SIDO + " " + c.GU == _AddressStateParse(Sido) + " " + Gu && c.DONG != "").ToList();

            if (_Q.Any())
            {

                cmb_Dong.DisplayMember = "DONG";
                cmb_Dong.ValueMember = "AREACODE";

                //cmb_Dong.DataSource = cargoDataSet.CargoApiAdress.Where(c => c.SIDO + " " + c.GU == _AddressStateParse(Sido) + " " + _AddressCityParse(Gu) && c.DONG != "").ToList();
                cmb_Dong.DataSource = cargoDataSet.CargoApiAdressNew.Where(c => c.SIDO + " " + c.GU == _AddressStateParse(Sido) + " " + Gu && c.DONG != "").ToList();


                cmb_Dong.SelectedIndex = 0;



                lbl_Sido.Text = _AddressStateParse(Sido);
                //lbl_GuGun.Text = _AddressCityParse(Gu);
                lbl_GuGun.Text = Gu;
            }
            else
            {
                MessageBox.Show("해당주소가 없습니다.\r\n화물정보에서 올바른 주소 수정후 다시 연동해주세요.");
                btnOK.Enabled = false;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            //if (cmb_Dong.SelectedIndex == 0)
            //{

            //}
            //else
            //{
            var Query = cargoDataSet.CargoApiAdressNew.Where(c => c.AREACODE == cmb_Dong.SelectedValue.ToString()).ToArray();

            if (Query.Any())
            {
                _SIDO = Query.First().SIDO;
                _Gu_Gun = Query.First().GU + " " + Query.First().GUN;
                _DONG = Query.First().DONG;
                _POIX = Query.First().POIX;
                _POIY = Query.First().POIY;
                _AREACODE = Query.First().AREACODE;
            }
            //}
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
                //        R = _Values[0].Substring(0, _Values[0].Length - 1) + " " + _Values[1];
                //}
                //else
                //{
                //    if (_Value.Length > 2)
                //        R = _Value.Substring(0, _Value.Length - 1);
                //}


            }
            return R;
        }

    }
}
