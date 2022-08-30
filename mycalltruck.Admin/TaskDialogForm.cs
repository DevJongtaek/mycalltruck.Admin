using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace mycalltruck.Admin
{
    public partial class TaskDialogForm : Form
    {
        public TaskDialogForm()
        {
            InitializeComponent();
        }
        public DialogResult Show(string head, string fileName)
        {
            Text = head;
            lblContent1.Text = String.Format("이름 : {0}", fileName);
            this.ShowDialog();
            return result;
        }
        private DialogResult result=DialogResult.Cancel;
        private void btnOpen_Click(object sender, EventArgs e)
        {
            result = DialogResult.Yes;
            Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            result = DialogResult.No;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            result = DialogResult.Cancel;
            Close();
        }
    }
}
