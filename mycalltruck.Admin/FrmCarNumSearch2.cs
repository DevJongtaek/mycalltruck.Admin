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
    public partial class FrmCarNumSearch2 : Form
    {
        public FrmCarNumSearch2()
        {
            InitializeComponent();

            cmb_Search.SelectedIndex = 0;
        }

        private void FrmCarNumSearch_Load(object sender, EventArgs e)
        {
            btn_Search_Click(null, null);
        }

        private void btn_Search_Click(object sender, EventArgs e)
        {
            _Search();
        }

        string DriverIdIN = string.Empty;
        private void _Search()
        {
            this.drivers_CarTableAdapter.Fill(this.cMDataSet.Drivers_Car);
            var driverGroupsTableAdapter = new CMDataSetTableAdapters.DriverGroupsTableAdapter();
            driverGroupsTableAdapter.Fill(this.cMDataSet.DriverGroups);

            try
            {
                DriverIdIN = string.Empty;

                int[] DriverID = cMDataSet.DriverGroups.Where(c => c.ClientId == LocalUser.Instance.LogInInformation.ClientId).Select(c => c.DriverId).ToArray();

                if (DriverID.Length > 0)
                {
                    foreach (var code in DriverID)
                    {
                        DriverIdIN += code + ",";
                    }
                    DriverIdIN = DriverIdIN.Substring(0, DriverIdIN.Length - 1);


                    //if (String.IsNullOrEmpty(txt_Search.Text))
                    //{

                    //    driversCarBindingSource.Filter = string.Format("DriverId  IN ({0}) or CandidateId  = {1}", DriverIdIN, LocalUser.Instance.LogInInfomation.ClientId);
                    //}
                    //else
                    //{

                    //    driversCarBindingSource.Filter = string.Format("CarNo Like '%{0}%'  and (DriverId  IN ({1}) or CandidateId = {2})", txt_Search.Text, DriverIdIN, LocalUser.Instance.LogInInfomation.ClientId);

                    //}

                     if (String.IsNullOrEmpty(txt_Search.Text))
                    {
                        driversCarBindingSource.Filter = string.Format("DriverId IN ({0}) or CandidateId  = {1}", DriverIdIN, LocalUser.Instance.LogInInformation.ClientId);
                    }
                    else
                    {
                        if (cmb_Search.Text == "차량번호")
                        {
                            driversCarBindingSource.Filter = string.Format("CarNo Like  '%{0}%' and (DriverId  IN ({1}) or CandidateId = {2})", txt_Search.Text, DriverIdIN, LocalUser.Instance.LogInInformation.ClientId);
                        }
                        //else if (cmb_Search.Text == "기사명")
                        //{
                        //    driversCarBindingSource.Filter = string.Format("CarYear Like  '%{0}%' and (DriverId  IN ({1}) or CandidateId = {2})", txt_Search.Text, DriverIdIN, LocalUser.Instance.LogInInfomation.ClientId);
                        //}
                        else if (cmb_Search.Text == "상호")
                        {
                            driversCarBindingSource.Filter = string.Format("Name Like  '%{0}%' and (DriverId  IN ({1}) or CandidateId = {2})", txt_Search.Text, DriverIdIN, LocalUser.Instance.LogInInformation.ClientId);
                        }
                    }

                }
                
                else
                {
                    //if (String.IsNullOrEmpty(txt_Search.Text))
                    //{

                    //    driversCarBindingSource.Filter = string.Format(" CandidateId  = {0}",  LocalUser.Instance.LogInInfomation.ClientId);
                    //}
                    //else
                    //{

                    //    driversCarBindingSource.Filter = string.Format("CarNo Like '%{0}%'  and (CandidateId = {1})", txt_Search.Text, LocalUser.Instance.LogInInfomation.ClientId);

                    //}

                    if (String.IsNullOrEmpty(txt_Search.Text))
                    {
                        driversCarBindingSource.Filter = string.Format(" CandidateId  = {0}", LocalUser.Instance.LogInInformation.ClientId);
                    }
                    else
                    {
                        if (cmb_Search.Text == "차량번호")
                        {
                            driversCarBindingSource.Filter = string.Format("CarNo Like '%{0}%'  and (CandidateId = {1})", txt_Search.Text, LocalUser.Instance.LogInInformation.ClientId);
                        }
                        else if (cmb_Search.Text == "기사명")
                        {
                            driversCarBindingSource.Filter = string.Format("CarYear Like  '%{0}%' and (CandidateId = {1})", txt_Search.Text, LocalUser.Instance.LogInInformation.ClientId);
                        }
                        else if (cmb_Search.Text == "상호")
                        {
                            driversCarBindingSource.Filter = string.Format("Name Like  '%{0}%' and  (CandidateId = {1})", txt_Search.Text, LocalUser.Instance.LogInInformation.ClientId);
                        }
                    }
                }
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
            cmb_Search.SelectedIndex = 0;
            txt_Search.Text = string.Empty;
            btn_Search_Click(null, null);
        }
    }
}
