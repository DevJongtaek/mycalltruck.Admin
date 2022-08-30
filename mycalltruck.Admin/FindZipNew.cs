using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;
using mycalltruck.Admin.Class.Common;
using Newtonsoft.Json;

namespace mycalltruck.Admin

{
    public partial class FindZipNew : Form
    {
        string _StartText = "";
        string _Gubun = "";
        public FindZipNew()
        {
            InitializeComponent();
            InitializeStorage();
            InitializeStorageSimple();
            rdoSimple.Enabled = false;
        }
        public FindZipNew(string StartText, string Gubun)
        {
            InitializeComponent();

            InitializeStorage();

            InitializeStorageSimple();

            _StartText = StartText;
            _Gubun = Gubun;
            txtQuery.Text = _StartText;

            if(_Gubun =="Order")
            {
                rdoSimple.Enabled = true;
            }
            else
            {
                rdoSimple.Enabled = false;

            }
            _Search();
        }
        public String Zip { get; set; }
        public String Address { get; set; }
        public String Jibun { get; set; }

        #region ACTION
        private void btnSearch_Click(object sender, EventArgs e)
        {
            _Search();
        }
        private void txtQuery_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                _Search();
            }
        }
        private void newDGV1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            if (e.ColumnIndex != ColumnAction.Index)
                return;
            _Select(e.RowIndex);
        }
        #endregion

        #region UPDATE
        private void _Search()
        {
            if (String.IsNullOrEmpty(txtQuery.Text))
                return;
            _CoreList.Clear();

            _SCoreList.Clear();


            try
            {
                var Query = SingleDataSet.Instance.AddressReferences.Where(c => c.State.Contains(txtQuery.Text) || c.City.Contains(txtQuery.Text) || c.Street.Contains(txtQuery.Text));

                if (Query.Any())
                {
                    foreach (var sjuso in Query.ToArray())
                    {
                        _SCoreList.Add(new SModel
                        {

                            Sido = sjuso.State,
                            SiGuNGu = sjuso.City,
                            Dong = sjuso.Street,
                        });

                    }
                }
            }
            catch
            {

            }

            try
            {
                var Url = $"http://www.juso.go.kr/addrlink/addrLinkApi.do?resultType=json&countPerPage=100&keyword={txtQuery.Text}&confmKey=U01TX0FVVEgyMDE2MTIyMjE2MTY0NjE3NTky";
                WebClient _WebClient = new WebClient();
                _WebClient.Encoding = Encoding.UTF8;
                var r = _WebClient.DownloadString(Url);
                dynamic _Array = JsonConvert.DeserializeObject(r);
                if (_Array.results != null)
                {
                    dynamic jusoArray = _Array.results.juso;
                    foreach (var juso in jusoArray)
                    {
                        var roadAddr = juso.roadAddr.ToString();
                        var zipNo = juso.zipNo.ToString();
                        var jibunAddr = juso.jibunAddr;
                        _CoreList.Add(new Model
                        {
                            Zip = zipNo,
                            Address = roadAddr,
                            Jibun = jibunAddr,
                        });
                    }
                }
            }
            catch (Exception)
            {

            }

        }
        private void _Select(int RowIndex)
        {
            if (rdoSimple.Checked)
            {
                var _SModel = _SCoreList[RowIndex];
                Zip = _SModel.Sido + " " + _SModel.SiGuNGu + " " + _SModel.Dong;
                Zip = Zip.Replace("  ", " ");
                Address = _SModel.Sido + " " + _SModel.SiGuNGu + " " + _SModel.Dong;
                Address = Address.Replace("  ", " ");
                Jibun = _SModel.Sido + " " + _SModel.SiGuNGu + " " + _SModel.Dong;
                Jibun = Jibun.Replace("  ", " ");
            }
            else
            {
                var _Model = _CoreList[RowIndex];
                Zip = _Model.Zip;
                Address = _Model.Address;
                Jibun = _Model.Jibun;
            }
            Close();
        }
        #endregion

        #region STORAGE
        class Model
        {
            public String Zip { get; set; }
            public String Address { get; set; }
            public String Jibun { get; set; }
        }
        BindingList<Model> _CoreList = new BindingList<Model>();
        private void InitializeStorage()
        {
            newDGV1.AutoGenerateColumns = false;
            newDGV1.DataSource = _CoreList;
        }


        class SModel
        {
            public String Sido { get; set; }
            public String SiGuNGu { get; set; }
            public String Dong { get; set; }
        }
        BindingList<SModel> _SCoreList = new BindingList<SModel>();
        private void InitializeStorageSimple()
        {
            newDGV2.AutoGenerateColumns = false;
            newDGV2.DataSource = _SCoreList;
        }
        #endregion

        #region VIEW
        private void newDGV1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            if (e.ColumnIndex == ColumnNumber.Index)
            {
                e.Value = (e.RowIndex + 1).ToString();
            }
        }
        #endregion

        private void newDGV1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            //if (e.RowIndex < 0)
            //    return;
            //if (e.ColumnIndex != ColumnAction.Index)
            //    return;
            //_Select(e.RowIndex);
        }

        private void newDGV1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.RowIndex < 0)
                return;

            _Select(e.RowIndex);
        }
        
        private void newDGV1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            //if (e.RowIndex < 0)
            //    return;

            //_Select(e.RowIndex);
        }

        private void rdoRoad_CheckedChanged(object sender, EventArgs e)
        {
           
        }

        private void rdoSimple_CheckedChanged(object sender, EventArgs e)
        {
            _Search();
            if (rdoSimple.Checked)
            {
                newDGV1.Visible = false;
                newDGV2.Visible = true;
            }
            else
            {
                newDGV1.Visible = true;
                newDGV2.Visible = false;
            }
        }

        private void newDGV2_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            if (e.ColumnIndex == dataGridViewTextBoxColumn1.Index)
            {
                e.Value = (e.RowIndex + 1).ToString();
            }
        }

        private void newDGV2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            if (e.ColumnIndex != dataGridViewButtonColumn1.Index)
                return;
            _Select(e.RowIndex);
        }

        private void newDGV2_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            _Select(e.RowIndex);
        }
    }
}
