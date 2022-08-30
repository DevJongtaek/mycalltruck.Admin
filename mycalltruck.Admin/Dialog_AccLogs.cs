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
    public partial class Dialog_AccLogs : Form
    {
        public Dialog_AccLogs()
        {
            InitializeComponent();

        }

        public void SetListDataSource(object iDataSource)
        {
            newDGV1.AutoGenerateColumns = false;
            newDGV1.DataSource = iDataSource;
        }

        private void newDGV1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            if (e.ColumnIndex == 0)
            {
                e.Value = (newDGV1.RowCount - e.RowIndex).ToString("N0");
            }
        }
    }
}
