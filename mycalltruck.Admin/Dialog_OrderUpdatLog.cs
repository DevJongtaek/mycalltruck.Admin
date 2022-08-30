using mycalltruck.Admin.DataSets;
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
    public partial class Dialog_OrderUpdatLog : Form
    {
        int DataCount = 0;
        public Dialog_OrderUpdatLog(OrderUpdateLog[] DataSource)
        {
            InitializeComponent();
            DataCount = DataSource.Length;
            newDGV1.AutoGenerateColumns = false;
            newDGV1.DataSource = DataSource;
        }

        private void newDGV1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            if(e.ColumnIndex == ColumnNo.Index)
            {
                e.Value = DataCount - e.RowIndex;
            }
        }
    }
}
