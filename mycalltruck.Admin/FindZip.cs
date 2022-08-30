using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace mycalltruck.Admin
{
    public partial class FindZip : Form
    {
        public FindZip()
        {
            InitializeComponent();
            InitializeStorage();
        }

        public String Zip { get; set; }
        public String Address { get; set; }

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
            try
            {
                var Url = $"http://www.juso.go.kr/addrlink/addrLinkApi.do?resultType=json&countPerPage=100&keyword={txtQuery.Text}&confmKey=U01TX0FVVEgyMDE2MTIyMjE2MTY0NjE3NTky";
                WebClient _WebClient = new WebClient();
                _WebClient.Encoding = Encoding.UTF8;
                var r = _WebClient.DownloadString(Url);
                dynamic _Array = JsonConvert.DeserializeObject(r);
                if(_Array.results != null)
                {
                    dynamic jusoArray = _Array.results.juso;
                    foreach (var juso in jusoArray)
                    {
                        var roadAddr = juso.roadAddr.ToString();
                        var zipNo = juso.zipNo.ToString();
                        _CoreList.Add(new Model {
                            Zip = zipNo,
                            Address = roadAddr,
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
            var _Model = _CoreList[RowIndex];
            Zip = _Model.Zip;
            Address = _Model.Address;
            Close();
        }
        #endregion

        #region STORAGE
        class Model
        {
            public String Zip { get; set; }
            public String Address { get; set; }
        }
        BindingList<Model> _CoreList = new BindingList<Model>();
        private void InitializeStorage()
        {
            newDGV1.AutoGenerateColumns = false;
            newDGV1.DataSource = _CoreList;
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

    }
}
