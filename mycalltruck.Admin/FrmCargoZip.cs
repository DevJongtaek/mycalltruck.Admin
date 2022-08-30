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
    public partial class FrmCargoZip : Form
    {
        public FrmCargoZip()
        {
            InitializeComponent();

            _InitCmb();

            btnSearch_Click(null, null);
        }

        private void _InitCmb()
        {
            Dictionary<string, string> IsSharedList = new Dictionary<string, string>();
            IsSharedList.Add("1", "시군구");
            IsSharedList.Add("2", "읍면동");
            IsSharedList.Add("3", "시도");
            IsSharedList.Add("4", "국명");


            cmb_Search.DataSource = new BindingSource(IsSharedList, null);
            cmb_Search.DisplayMember = "Value";
            cmb_Search.ValueMember = "Key";
        }

        private void FrmCargoZip_Load(object sender, EventArgs e)
        {
            // TODO: 이 코드는 데이터를 'cMDataSet.CargoZipcode' 테이블에 로드합니다. 필요한 경우 이 코드를 이동하거나 제거할 수 있습니다.
            this.cargoZipcodeTableAdapter.Fill(this.cMDataSet.CargoZipcode, "%", "Korea", "%", "%");

           

        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            _Search();
        }

        private void _Search()
        {

          //  cargoZipcodeTableAdapter.Fill(SingleDataSet.Instance.CargoZipcode);
           
            string _FilterString = string.Empty;
            string _cmbSearchString = string.Empty;
          

            try
            {
                if (cmb_Search.SelectedValue.ToString() == "1")
                {
                    this.cargoZipcodeTableAdapter.Fill(this.cMDataSet.CargoZipcode, "%", "Korea",txt_Search.Text, "%");
                }
                else if (cmb_Search.SelectedValue.ToString() == "2")
                {
                    this.cargoZipcodeTableAdapter.Fill(this.cMDataSet.CargoZipcode, "%", "Korea", "%", txt_Search.Text);
                }
                else if (cmb_Search.SelectedValue.ToString() == "3")
                {
                    this.cargoZipcodeTableAdapter.Fill(this.cMDataSet.CargoZipcode, txt_Search.Text, "Korea", "%", "%");
                }
                else if (cmb_Search.SelectedValue.ToString() == "4")
                {
                    this.cargoZipcodeTableAdapter.Fill(this.cMDataSet.CargoZipcode, txt_Search.Text, "State", "%", "%");
                }
              
              
            }
            catch
            {
            }


        }

        private void txt_Search_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Return) return;
            btnSearch_Click(null,null);
        }
    }
}
