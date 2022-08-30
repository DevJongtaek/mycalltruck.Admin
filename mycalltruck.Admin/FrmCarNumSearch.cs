using mycalltruck.Admin.Class.Common;
using mycalltruck.Admin.Class.DataSet;
using mycalltruck.Admin.DataSets;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace mycalltruck.Admin
{
    public partial class FrmCarNumSearch : Form
    {
        public FrmCarNumSearch()
        {
            InitializeComponent();
            cmb_Search.SelectedIndex = 0;
            DialogResult = DialogResult.Abort;
        }

        public int DriverId { get; set; }
        public String CEO { get; set; }
        public String Name { get; set; }
        public String CarNo { get; set; }
        public String MobileNo { get; set; }
        public int DriverCarType { get; set; }
        public int DriverCarSize { get; set; }
        public String DriverCarNo { get; set; }
        public String DriverName { get; set; }
        public String DriverMobileNo { get; set; }


        private void FrmCarNumSearch_Load(object sender, EventArgs e)
        {
            btn_Search_Click(null, null);
        }

        private void btn_Search_Click(object sender, EventArgs e)
        {
            _Search();
        }

        private void _Search()
        {
            try
            {
                List<String> WhereStringList = new List<string>();
                if (!String.IsNullOrEmpty(txt_Search.Text))
                {
                    switch (cmb_Search.Text)
                    {
                        case "상호":
                            WhereStringList.Add("Drivers.Name LIKE '%" + txt_Search.Text + "%'");
                            break;
                        case "차량번호":
                            WhereStringList.Add("Drivers.CarNo LIKE '%" + txt_Search.Text + "%'");
                            break;
                        case "기사명":
                            WhereStringList.Add("Drivers.CarYear LIKE '%" + txt_Search.Text + "%'");
                            break;
                        default:
                            break;
                    }
                }
                DriverRepository mDriverRepository = new DriverRepository();
                mDriverRepository.Select(baseDataSet.Drivers, WhereStringList);
            }
            catch
            {
                MessageBox.Show("조회가 실패하였습니다. 잠시 후에 다시 시도해주시기 바랍니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }


        private void txt_Search_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Return) return;
            btn_Search_Click(null, null);
        }

        private void btn_new_Click(object sender, EventArgs e)
        {
            cmb_Search.SelectedIndex = 0;
            txt_Search.Text = string.Empty;
            btn_Search_Click(null, null);
        }

        private void grid1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == Car_ContRact.Index && e.RowIndex >= 0)
            {
                var Selected = ((DataRowView)grid1.Rows[e.RowIndex].DataBoundItem).Row as BaseDataSet.DriversRow;
                var Query = SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "CarGubun" && c.Value == Selected.CarGubun);
                if (Query.Any())
                {
                    e.Value = Query.First().Name;
                }
            }
        }

        private void grid1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            _Select();
        }

        private void grid1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Return)
                return;
            if (grid1.CurrentCell == null)
                return;
            if (grid1.CurrentCell.RowIndex < 0)
                return;
            _Select();
        }

        private void _Select()
        {
            var Selected = ((DataRowView)driversBindingSource.Current).Row as BaseDataSet.DriversRow;
            DriverId = Selected.DriverId;
            DriverCarType = Selected.CarType;
            DriverCarSize = Selected.CarSize;
            DriverCarNo = Selected.CarNo;
            DriverName = Selected.CarYear;
            DriverMobileNo = Selected.MobileNo;

            CEO = Selected.CEO;
            Name = Selected.Name;
            CarNo = Selected.CarNo;
            MobileNo = Selected.MobileNo;

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
