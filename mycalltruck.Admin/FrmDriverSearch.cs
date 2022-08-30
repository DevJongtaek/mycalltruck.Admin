using mycalltruck.Admin.Class.Common;
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
    public partial class FrmDriverSearch : Form
    {
        public FrmDriverSearch()
        {
            InitializeComponent();

           // cmb_Search.SelectedIndex = 0;
        }

        private void FrmCarNumSearch_Load(object sender, EventArgs e)
        {
            // TODO: 이 코드는 데이터를 'cMDataSet.DriverAdd' 테이블에 로드합니다. 필요한 경우 이 코드를 이동하거나 제거할 수 있습니다.
            this.driverAddTableAdapter.Fill(this.cMDataSet.DriverAdd);
           
      

            btn_Search_Click(null, null);
        }

        private void btn_Search_Click(object sender, EventArgs e)
        {
            _Search();
        }

        string DriverIdIN = string.Empty;
        private void _Search()
        {
            if (!LocalUser.Instance.LogInInformation.IsAdmin)
            {
                driverAddTableAdapter.FillByClient(cMDataSet.DriverAdd, LocalUser.Instance.LogInInformation.ClientId);
            }
            else
            {
                driverAddTableAdapter.Fill(cMDataSet.DriverAdd);
            }
            try
            {
                driverAddBindingSource.Filter = string.Format("PhoneNo Like '%{0}%'  or Name Like '%{0}%'  or MobileNo Like '%{0}%' ", txt_Search.Text);
            }
            catch
            {
            }
        }

        private void txt_Search_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Return) return;
            btn_Search_Click(null, null);
        }

        private void btn_new_Click(object sender, EventArgs e)
        {
          //  cmb_Search.SelectedIndex = 0;
            txt_Search.Text = string.Empty;
            btn_Search_Click(null, null);
        }
    }
}
