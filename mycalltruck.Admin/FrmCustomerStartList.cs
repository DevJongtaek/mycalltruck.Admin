using mycalltruck.Admin.DataSets;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace mycalltruck.Admin
{
    public partial class FrmCustomerStartList : Form
    {
        int _CustomerId = 0;
        public FrmCustomerStartList()
        {
            InitializeComponent();
        }
        public String StartState { get; set; }
        public String StartCity { get; set; }
        public String StartStreet { get; set; }
        public String StartDetail { get; set; }
        public String StartPhoneNo { get; set; }
        public String StartGubun { get; set; }
        public String CustomerPhoneNo { get; set; }
        public String StartName { get; set; }
        private void FrmCustomerStartList_Load(object sender, EventArgs e)
        {
           
            CustomerStartData();

            // TODO: 이 코드는 데이터를 'customerStartManageDataSet.CustomerStartManage' 테이블에 로드합니다. 필요 시 이 코드를 이동하거나 제거할 수 있습니다.
            // this.customerStartManageTableAdapter.Fill(this.customerStartManageDataSet.CustomerStartManage);



        }
        public void Search(int CustomerId = 0)
        {
            _CustomerId = CustomerId;

            CustomerStartData();

        }
        private void CustomerStartData()
        {

            customerStartManageDataSet.CustomerStartManage.Clear();

            using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                _Connection.Open();
                SqlCommand _Command = _Connection.CreateCommand();
                _Command.CommandText =

                      @"SELECT  idx, StartState, StartCity, StartStreet, StartDetail, StartName, StartPhoneNo, ClientId, CustomerId, CreateTime,Gubun FROM(
SELECT 0 as idx,AddressState as StartState, AddressCity as StartCity,'' as StartStreet, AddressDetail as StartDetail,''as StartName,PhoneNo as StartPhoneNo,ClientId,CustomerId,CreateTime,'본사' as Gubun FROM Customers    
UNION  
SELECT  idx, StartState, StartCity, StartStreet, StartDetail, StartName, StartPhoneNo, ClientId, CustomerId, CreateTime,'추가' as Gubun FROM CustomerStartManage WHERE SGubun = 'S')A
                        where CustomerId = @CustomerId
                       
                        order by CreateTime DESC";


                _Command.Parameters.AddWithValue("@CustomerId", _CustomerId);
                using (SqlDataReader _Reader = _Command.ExecuteReader())
                {
                    customerStartManageDataSet.CustomerStartManage.Load(_Reader);
                }
            }
        }

        private void newDGV1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            //  var Selected = ((DataRowView)customerStartManageBindingSource.Current).Row as CustomerStartManageDataSet.CustomerStartManageRow;

            if (e.ColumnIndex == ColumnNumber.Index)
            {
                e.Value = (newDGV1.Rows.Count - e.RowIndex).ToString();
            }
            if (e.ColumnIndex == ColumnStart.Index)
            {
                var Selected = ((DataRowView)newDGV1.Rows[e.RowIndex].DataBoundItem).Row as CustomerStartManageDataSet.CustomerStartManageRow;
                e.Value = Selected.StartState + " " + Selected.StartCity + " " + Selected.StartStreet + " " + Selected.StartDetail;
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

        private void _Select(int RowIndex)
        {
         
            var _Model = customerStartManageDataSet.CustomerStartManage[RowIndex];

         
            StartState = _Model.StartState;
            StartCity = _Model.StartCity;
            StartStreet = _Model.StartStreet;
            StartDetail = _Model.StartDetail;
            StartPhoneNo = _Model.StartPhoneNo;
            StartGubun = _Model.Gubun;
            StartName = _Model.StartName;
            Close();
        }
        class Model
        {
            public String StartState { get; set; }
            public String StartCity { get; set; }
            public String StartStreet { get; set; }
            public String StartDetail { get; set; }
            public String StartPhoneNo { get; set; }
            public String SrartGubun { get; set; }
            public String StartName { get; set; }

        }

        private void newDGV1_KeyPress(object sender, KeyPressEventArgs e)
        {

            if (e.KeyChar == Convert.ToChar(Keys.Enter))
            {

                int i = newDGV1.CurrentRow.Index;


               // MessageBox.Show(i.ToString());

                if (i < 0)
                {
                    return;
                }
                _Select(i);
            }


            //if (e.RowIndex < 0)
            //    return;

            //_Select(e.RowIndex);
        }

        private void newDGV1_KeyUp(object sender, KeyEventArgs e)
        {
            //if (e.KeyCode.Equals(Keys.Enter))
            //{
            //    var _Model = customerStartManageDataSet.CustomerStartManage[RowIndex];
            //    StartState = _Model.StartState;
            //    StartCity = _Model.StartCity;
            //    StartStreet = _Model.StartStreet;
            //    StartDetail = _Model.StartDetail;
            //    StartPhoneNo = _Model.StartPhoneNo;
            //    StartGubun = _Model.Gubun;
            //    Close();
            //}

                
        }

        private void newDGV1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                int column = newDGV1.CurrentCell.ColumnIndex;
                int row = newDGV1.CurrentCell.RowIndex;
                newDGV1.CurrentCell = newDGV1[column, row];
                e.Handled = true;
            }

            
        }
    }
}
