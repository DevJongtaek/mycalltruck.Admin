using mycalltruck.Admin.Class.Common;
using mycalltruck.Admin.Class.DataSet;
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
    public partial class Frm0104_DTGTOTAL : Form
    {
        public Frm0104_DTGTOTAL()
        {
            InitializeComponent();
        }
        string FullDays = string.Empty;
        string FirstDays = string.Empty;
        string SecondDays = string.Empty;
        string ThirdDays = string.Empty;
        string LastDays = string.Empty;
        string FullMonth = string.Empty;
        private void Frm0104_DTGTOTAL_Load(object sender, EventArgs e)
        {
            this.dtgLogTableAdapter.Fill(this.cMDataSet.DTGLog, LocalUser.Instance.LogInInformation.ClientId);
            dtpStart.Value = DateTime.Now.AddMonths(-3).AddDays(1);
            dtpEnd.Value = DateTime.Now;
            DriverRepository mDriverRepository = new DriverRepository();
            mDriverRepository.Select(baseDataSet.Drivers);
        }
        string LastDay = string.Empty;
        private void driversBindingSource_CurrentChanged(object sender, EventArgs e)
        {
            if (driversBindingSource.Current == null)
                return;
            var Selected = ((DataRowView)driversBindingSource.Current).Row as BaseDataSet.DriversRow;
            if (Selected != null)
            {
                var Query = cMDataSet.DTGLog.Where(c => c.DriverId == Selected.DriverId).ToArray();

                DateTime dtend = new DateTime(Math.Abs(dtpEnd.Value.Date.Subtract(dtpStart.Value.Date).Ticks));
                FullMonth = (((dtend.Year - 1) * 12) + (dtend.Month)).ToString();
                FullDays = (dtpEnd.Value - dtpStart.Value).Days.ToString();
                FirstDays = (System.DateTime.DaysInMonth(dtpStart.Value.Year, dtpStart.Value.Month) - dtpStart.Value.Day).ToString();



                #region 첫달
                Button[] DText = new Button[int.Parse(FirstDays) + 1];

                pn_1_month.Controls.Clear();
                pn_2_month.Controls.Clear();
                pn_3_month.Controls.Clear();
                pn_4_month.Controls.Clear();
                int j = 0;
                int k = 0;
                lbl_Month1.Text = "[ " + dtpStart.Value.Year.ToString() + "年 " + dtpStart.Value.Month.ToString() + "月 ]";
                for (int i = 0; i < DText.Length; i++)
                {


                    DText[i] = new Button();




                    if (i > 19)
                    {

                        DText[i].Location = new Point(k * 32, 64);
                        k++;
                    }
                    else if (i > 9)
                    {

                        DText[i].Location = new Point(j * 32, 32);
                        j++;
                    }
                    else
                    {
                        DText[i].Location = new Point(i * 32, 0);
                    }
                    DText[i].Size = new Size(30, 30);

                    DText[i].Name = "DText" + i.ToString();


                    DText[i].Text = (int.Parse(dtpStart.Value.Day.ToString()) + i).ToString();

                    string iYear = dtpStart.Value.Year.ToString();
                    string iMonth = dtpStart.Value.Month.ToString();
                    string iDay = ((int.Parse(dtpStart.Value.Day.ToString())) + i).ToString();
                    string iDate = DateTime.Parse(iYear + "-" + iMonth + "-" + iDay).ToString("yyyy-MM-dd");


                    if (Query.Any())
                    {
                        var DtgLogYn = cMDataSet.DTGLog.Where(c => c.DriverId == Selected.DriverId && c.Date.Date == DateTime.Parse(iDate)).ToArray();
                        if (DtgLogYn.Any())
                        {
                            DText[i].BackColor = Color.Blue;
                        }
                        else
                        {
                            DText[i].BackColor = Color.Red;
                        }
                    }
                    else
                    {
                        DText[i].BackColor = Color.Red;

                    }

                    DText[i].TabIndex = i;
                    DText[i].ForeColor = System.Drawing.Color.Snow;
                    // DText[i].Enabled = false;
                    pn_1_month.Controls.Add(DText[i]);

                }


                //var Query1 = cMDataSet.DTGLog.Where( c=> c.DriverId == 
                #endregion


                #region 두번째달
                int SecondCnt = int.Parse(FullDays) - int.Parse(FirstDays);


                int SecontLastDat = (System.DateTime.DaysInMonth(int.Parse(dtpStart.Value.AddMonths(1).Year.ToString()), int.Parse(dtpStart.Value.AddMonths(1).Month.ToString())));
                //
                if (SecondCnt >= 1)
                {
                    lbl_Month2.Visible = true;

                    Button[] DText2 = new Button[1];


                    //남은일수가 마지막날짜보다 클때
                    if (SecondCnt > SecontLastDat)
                    {
                        DText2 = new Button[SecontLastDat];
                    }
                    else
                    {
                        DText2 = new Button[SecondCnt];
                    }
                    pn_2_month.Controls.Clear();
                    pn_3_month.Controls.Clear();
                    pn_4_month.Controls.Clear();
                    j = 0;
                    k = 0;
                    for (int i = 0; i < DText2.Length; i++)
                    {

                        DText2[i] = new Button();
                        //     DText2[i].Enabled = false;
                        DText2[i].ForeColor = System.Drawing.Color.Snow;
                        if (i > 19)
                        {

                            DText2[i].Location = new Point(k * 32, 64);
                            k++;
                        }
                        else if (i > 9)
                        {

                            DText2[i].Location = new Point(j * 32, 32);
                            j++;
                        }
                        else
                        {
                            DText2[i].Location = new Point(i * 32, 0);
                        }
                        DText2[i].Size = new Size(30, 30);

                        DText2[i].Name = "DText2" + i.ToString();

                        DText2[i].Text = (1 + i).ToString();



                        string iYear = dtpStart.Value.AddMonths(1).Year.ToString();
                        string iMonth = dtpStart.Value.AddMonths(1).Month.ToString();
                        string iDay = (1 + i).ToString();
                        string iDate = DateTime.Parse(iYear + "-" + iMonth + "-" + iDay).ToString("yyyy-MM-dd");


                        if (Query.Any())
                        {
                            var DtgLogYn = cMDataSet.DTGLog.Where(c => c.DriverId == Selected.DriverId && c.Date.Date == DateTime.Parse(iDate)).ToArray();
                            if (DtgLogYn.Any())
                            {
                                DText2[i].BackColor = Color.Blue;
                            }
                            else
                            {
                                DText2[i].BackColor = Color.Red;
                            }
                        }
                        else
                        {
                            DText2[i].BackColor = Color.Red;
                        }

                        DText2[i].TabIndex = i;

                        pn_2_month.Controls.Add(DText2[i]);

                    }
                }
                else
                {
                    lbl_Month2.Visible = false;
                }
                lbl_Month2.Text = "[ " + dtpStart.Value.AddMonths(1).Year.ToString() + "年 " + dtpStart.Value.AddMonths(1).Month.ToString() + "月 ]";
                #endregion

                #region 세번째달
                //남은일수
                int Thirdcnt = int.Parse(FullDays) - int.Parse(FirstDays) - SecontLastDat;

                //마지막날짜
                int ThirdLastDat = (System.DateTime.DaysInMonth(int.Parse(dtpStart.Value.AddMonths(2).Year.ToString()), int.Parse(dtpStart.Value.AddMonths(2).Month.ToString())));
                //
                if (Thirdcnt >= 1)
                {
                    lbl_Month3.Visible = true;
                    Button[] DText3 = new Button[1];
                    //남은일수가 마지막날짜보다 클때
                    if (Thirdcnt > ThirdLastDat)
                    {
                        DText3 = new Button[ThirdLastDat];
                    }
                    else
                    {
                        DText3 = new Button[Thirdcnt];
                    }

                    pn_3_month.Controls.Clear();
                    pn_4_month.Controls.Clear();
                    j = 0;
                    k = 0;
                    for (int i = 0; i < DText3.Length; i++)
                    {

                        DText3[i] = new Button();
                        //   DText3[i].Enabled = false;
                        DText3[i].ForeColor = System.Drawing.Color.Snow;

                        if (i > 19)
                        {

                            DText3[i].Location = new Point(k * 32, 64);
                            k++;
                        }
                        else if (i > 9)
                        {

                            DText3[i].Location = new Point(j * 32, 32);
                            j++;
                        }
                        else
                        {
                            DText3[i].Location = new Point(i * 32, 0);
                        }
                        DText3[i].Size = new Size(30, 30);

                        DText3[i].Name = "DText3" + i.ToString();
                        DText3[i].Text = (1 + i).ToString();



                        string iYear = dtpStart.Value.AddMonths(2).Year.ToString();
                        string iMonth = dtpStart.Value.AddMonths(2).Month.ToString();
                        string iDay = (1 + i).ToString();
                        string iDate = DateTime.Parse(iYear + "-" + iMonth + "-" + iDay).ToString("yyyy-MM-dd");


                        if (Query.Any())
                        {
                            var DtgLogYn = cMDataSet.DTGLog.Where(c => c.DriverId == Selected.DriverId && c.Date.Date == DateTime.Parse(iDate)).ToArray();
                            if (DtgLogYn.Any())
                            {
                                DText3[i].BackColor = Color.Blue;

                            }
                            else
                            {
                                DText3[i].BackColor = Color.Red;

                            }
                        }
                        else
                        {
                            DText3[i].BackColor = Color.Red;

                        }


                        DText3[i].TabIndex = i;

                        pn_3_month.Controls.Add(DText3[i]);

                    }
                }
                else
                {
                    lbl_Month3.Visible = false;
                }
                lbl_Month3.Text = "[ " + dtpStart.Value.AddMonths(2).Year.ToString() + "年 " + dtpStart.Value.AddMonths(2).Month.ToString() + "月 ]";
                #endregion

                #region 네번째달
                int Lastcnt = int.Parse(FullDays) - int.Parse(FirstDays) - SecontLastDat - ThirdLastDat;


                int LastDat = (System.DateTime.DaysInMonth(int.Parse(dtpStart.Value.AddMonths(3).Year.ToString()), int.Parse(dtpStart.Value.AddMonths(3).Month.ToString())));
                //
                if (Lastcnt >= 1)
                {
                    lbl_Month4.Visible = true;
                    Button[] DText4 = new Button[1];
                    //남은일수가 마지막날짜보다 클때
                    if (Lastcnt > LastDat)
                    {
                        DText4 = new Button[LastDat];
                    }
                    else
                    {
                        DText4 = new Button[Lastcnt];
                    }

                    pn_4_month.Controls.Clear();
                    j = 0;
                    k = 0;
                    for (int i = 0; i < DText4.Length; i++)
                    {

                        DText4[i] = new Button();
                        //  DText4[i].Enabled = false;
                        DText4[i].ForeColor = System.Drawing.Color.Snow;
                        if (i > 19)
                        {

                            DText4[i].Location = new Point(k * 32, 64);
                            k++;
                        }
                        else if (i > 9)
                        {

                            DText4[i].Location = new Point(j * 32, 32);
                            j++;
                        }
                        else
                        {
                            DText4[i].Location = new Point(i * 32, 0);
                        }
                        DText4[i].Size = new Size(30, 30);

                        DText4[i].Name = "DText4" + i.ToString();
                        DText4[i].Text = (1 + i).ToString();


                        string iYear = dtpStart.Value.AddMonths(3).Year.ToString();
                        string iMonth = dtpStart.Value.AddMonths(3).Month.ToString();
                        string iDay = (1 + i).ToString();
                        string iDate = DateTime.Parse(iYear + "-" + iMonth + "-" + iDay).ToString("yyyy-MM-dd");


                        if (Query.Any())
                        {
                            var DtgLogYn = cMDataSet.DTGLog.Where(c => c.DriverId == Selected.DriverId && c.Date.Date == DateTime.Parse(iDate)).ToArray();
                            if (DtgLogYn.Any())
                            {
                                DText4[i].BackColor = Color.Blue;
                            }
                            else
                            {
                                DText4[i].BackColor = Color.Red;
                            }
                        }
                        else
                        {
                            DText4[i].BackColor = Color.Red;
                        }

                        DText4[i].TabIndex = i;

                        pn_4_month.Controls.Add(DText4[i]);

                    }
                }
                else
                {
                    lbl_Month4.Visible = false;
                }
                lbl_Month4.Text = "[ " + dtpStart.Value.AddMonths(3).Year.ToString() + "年 " + dtpStart.Value.AddMonths(3).Month.ToString() + "月 ]";
                #endregion

                //  }

                // label3.Text = String.Join(",", FirstMonth.Select(c => "" + c + "").ToArray());
            }
        }

        private void dtpEnd_ValueChanged(object sender, EventArgs e)
        {
            // LastDay = System.DateTime.DaysInMonth(dtpStart.Value.Year, dtpStart.Value.Month).ToString();
            F_Error("E");

            driversBindingSource_CurrentChanged(null, null);


        }

        private void dtpStart_ValueChanged(object sender, EventArgs e)
        {
            F_Error("S");

            // FullDays = (dtpEnd.Value - dtpStart.Value).Days.ToString();


            driversBindingSource_CurrentChanged(null, null);
            // MessageBox.Show(Days);
        }
        //   List<string> FirstMonth = new List<string>();
        private void Fcalendar()
        {
            //FirstMonth.Clear();

            //for (int i = dtpStart.Value.Day; i <= int.Parse(LastDay); i++)
            //{
            //    FirstMonth.Add(i.ToString());
            //}

        }
        private void F_Error(string SE)
        {
            string a = dtpEnd.Value.AddMonths(-3).Date.ToString();
            string b = dtpStart.Value.AddMonths(3).AddDays(1).Date.ToString();
            switch (SE)
            {
                case "S":

                    if (dtpEnd.Value.Date >= dtpStart.Value.Date.AddMonths(3).Date)
                    //if ((dtpEnd.Value - dtpStart.Value).Days > 93)
                    {
                        MessageBox.Show("세달 이상의 자료를 검색할 수 없습니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                        //dtpStart.Value = DateTime.Now.AddMonths(-3).AddDays(1);
                        //dtpEnd.Value = DateTime.Now;

                    }
                    break;




                case "E":

                    if (dtpEnd.Value.AddMonths(-3).Date >= dtpStart.Value.Date)
                    //if ((dtpEnd.Value - dtpStart.Value).Days > 93)
                    {
                        MessageBox.Show("세달 이상의 자료를 검색할 수 없습니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        //dtpStart.Value = DateTime.Now.AddMonths(-3).AddDays(1);
                        //dtpEnd.Value = DateTime.Now;
                        return;
                    }
                    break;
            }
        }

        private void newDGV1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == 0 && e.RowIndex >= 0)
            {
                newDGV1[e.ColumnIndex, e.RowIndex].Value = (newDGV1.Rows.Count - e.RowIndex).ToString("N0").Replace(",", "");
            }
        }
    }
}
