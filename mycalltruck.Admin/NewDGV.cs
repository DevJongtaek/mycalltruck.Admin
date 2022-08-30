using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using mycalltruck.Admin.Class.Common;
using mycalltruck.Admin.Class;

namespace mycalltruck.Admin
{
    public class NewDGV : DataGridView
    {
        public NewDGV()
        {
            DoubleBuffered = true;
            InitializeComponent();
        }
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // NewDGV
            // 
            this.AllowUserToResizeRows = false;
            this.BackgroundColor = System.Drawing.Color.White;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleVertical;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            //dataGridViewCellStyle1.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle1.Font = new Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.GridColor = System.Drawing.Color.White;
            this.Margin = new System.Windows.Forms.Padding(0);
            this.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.RowTemplate.Height = 23;
            this.MultiSelect = true;
            this.SetStyle(ControlStyles.DoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);


            //this.SizeChanged += new System.EventHandler(this.NewDGV_SizeChanged);
            this.KeyUp += new KeyEventHandler(NewDGV_KeyUp);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
        }

        void NewDGV_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.C)
            {
                Clipboard.Clear();
                Clipboard.SetDataObject(this.GetClipboardContent().GetText(TextDataFormat.UnicodeText));
            }
        }
        private void NewDGV_SizeChanged(object sender, EventArgs e)
        {
            if (this.Width == 0 || this.Height == 00)
                this.BackgroundImage = null;
            else
            {

                this.BackgroundColor = ApplicationColor.Instance.GridBackColor;
                this.DefaultCellStyle.BackColor = ApplicationColor.Instance.GridDefaultCellBackColor;
                this.DefaultCellStyle.ForeColor = ApplicationColor.Instance.GridDefaultCellForeColor;
                this.DefaultCellStyle.SelectionBackColor = ApplicationColor.Instance.GridSelectedCellBackColor;
                this.DefaultCellStyle.SelectionForeColor = ApplicationColor.Instance.GridSelectedCellForeColor;
                this.AlternatingRowsDefaultCellStyle.BackColor = ApplicationColor.Instance.GridAlternatingCellBackColor;
                this.AlternatingRowsDefaultCellStyle.ForeColor = ApplicationColor.Instance.GridAlternatingCellForeColor;
                this.AlternatingRowsDefaultCellStyle.SelectionBackColor = ApplicationColor.Instance.GridSelectedCellBackColor;
                this.AlternatingRowsDefaultCellStyle.SelectionForeColor = ApplicationColor.Instance.GridSelectedCellForeColor;
            }

        }
    }
}
