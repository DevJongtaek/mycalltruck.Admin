using mycalltruck.Admin.Class.Common;
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
    public partial class FrmGongJi : Form
    {
        public FrmGongJi()
        {
            InitializeComponent();
        }

        private void FrmGongJi_Load(object sender, EventArgs e)
        {
            DataLoad();
        }

        private void DataLoad()
        {
            string _Contents = "";
            using (SqlConnection Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                Connection.Open();
                SqlCommand Command = Connection.CreateCommand();
                Command.CommandText =
                    @"SELECT Contents FROM Gongji WHERE UseYn = 'Y'";

                var DataReader = Command.ExecuteReader();
                if (DataReader.Read())
                {
                    //IsLogin = true;
                    //bool IsAdmin = true;
                    //String LoginId = txtID.Text;
                    _Contents = DataReader.GetString(0);
                    // LocalUser.Instance.LogInInformation = new LogInInformation(IsLogin, IsAdmin, LoginId, UserId);
                }
                Connection.Close();
            }

            if (!string.IsNullOrEmpty(_Contents))
            {

                txtContents.Text = _Contents;
                txtContents.Select(0, 1);
            }

            if (LocalUser.Instance.PersonalOption.GonjJiNO)
            {

                checkBox1.Checked = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                LocalUser.Instance.PersonalOption.GonjJiNO = true;
                LocalUser.Instance.PersonalOption.GonjJiDate = DateTime.Now.Date;
            }
            else
            {
                LocalUser.Instance.PersonalOption.GonjJiNO = false;
                LocalUser.Instance.PersonalOption.GonjJiDate = DateTime.Now.Date;

            }
            LocalUser.Instance.Write();

        
        }
    }
}
