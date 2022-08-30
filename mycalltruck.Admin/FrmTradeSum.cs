using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace mycalltruck.Admin
{
    public partial class FrmTradeSum : Form
    {
        string DateS = string.Empty;
        int iYear = DateTime.Now.Year;
        public FrmTradeSum(String Sdate, String Edate)
        {
            InitializeComponent();


           
            cmb_SumGubun.SelectedIndex = 0;

            cmb_TaxGubun.SelectedIndex = 0;
        }
        private void FrmTradeSum_Load(object sender, EventArgs e)
        {
            _Year();

            _Month();


            lbl_Date.Text = cmb_Year.SelectedValue + "년" + cmb_Month.SelectedValue+"월";
            
         
        }

        private void _Month()
        {
            Dictionary<string, string> Month = new Dictionary<string, string>();


            Month.Add("01", "01");
            Month.Add("02", "02");
            Month.Add("03", "03");
            Month.Add("04", "04");
            Month.Add("05", "05");
            Month.Add("06", "06");
            Month.Add("07", "07");
            Month.Add("08", "08");
            Month.Add("09", "09");
            Month.Add("10", "10");
            Month.Add("11", "11");
            Month.Add("12", "12");

            //for (int i = 1; i <= 12; i++)
            //{

            //    Month.Add(i.ToString(),i.ToString());

            //}


            cmb_Month.DataSource = new BindingSource(Month, null);
            cmb_Month.DisplayMember = "Value";
            cmb_Month.ValueMember = "Key";

           // cmb_Month.SelectedIndex = cmb_Month.FindString(DateTime.Now.Month.ToString("MM"));
            string iMonth = DateTime.Now.ToString("yyyyMMdd");
            cmb_Month.SelectedValue = iMonth.Substring(4,2);
        }
        private void _Year()
        {
            cmb_Year.DataSource = null;


            Dictionary<string, string> Year = new Dictionary<string, string>();
            

            for (int i = iYear - 10; i < iYear + 11; i++)
            {
                Year.Add(i.ToString(), i.ToString());
            }


            cmb_Year.DataSource = new BindingSource(Year, null);
            cmb_Year.DisplayMember = "Value";
            cmb_Year.ValueMember = "Key";

            cmb_Year.SelectedIndex = cmb_Year.FindString(iYear.ToString());
            //cmb_Year.SelectedText = iYear.ToString();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void cmb_Year_SelectionChangeCommitted(object sender, EventArgs e)
        {
            iYear =Convert.ToInt32(cmb_Year.SelectedValue.ToString());
            _Year();
        }

        private void cmb_Year_KeyPress(object sender, KeyPressEventArgs e)
        {
            iYear = Convert.ToInt32(cmb_Year.SelectedValue.ToString());
            _Year();
        }

        private void cmb_Year_Leave(object sender, EventArgs e)
        {

        }

        private void cmb_Year_SelectedIndexChanged(object sender, EventArgs e)
        {
            lbl_Date.Text = cmb_Year.SelectedValue + "년" + cmb_Month.SelectedValue + "월";
        }

        private void cmb_Month_SelectedIndexChanged(object sender, EventArgs e)
        {
            lbl_Date.Text = cmb_Year.SelectedValue + "년" + cmb_Month.SelectedValue + "월";
        }

        private void cmb_TaxGubun_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(cmb_TaxGubun.SelectedIndex == 0)
            {
                cmb_SumGubun.Enabled = true;

                label5.Text = lbl_Date.Text + " 전표를 합치겠습니까?";
                // 이 기간의 전표를 합치겠습니까?
            }
            else
            {
                cmb_SumGubun.Enabled = false;

                label5.Text = lbl_Date.Text + " 전표를 분리하겠습니까?";
            }
        }

      
    }
}
