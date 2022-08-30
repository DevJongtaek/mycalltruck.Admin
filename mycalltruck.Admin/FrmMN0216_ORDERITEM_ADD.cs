using mycalltruck.Admin.Class.Common;
using mycalltruck.Admin.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using mycalltruck.Admin.Class.Extensions;
using System.IO;
using mycalltruck.Admin.DataSets;

namespace mycalltruck.Admin
{
    public partial class FrmMN0216_ORDERITEM_ADD : Form
    {
        public FrmMN0216_ORDERITEM_ADD()
        {
            InitializeComponent();
        }

        private void FrmMN0216_ORDERITEM_ADD_Load(object sender, EventArgs e)
        {
            ItemCode_Add();
           
            CreateDate.Text = DateTime.Now.ToString("yyyy-MM-dd").Replace("-", "/");

            ItemName.Focus();
        }
        private void ItemCode_Add()
        {

            this.orderItemTableAdapter.Fill(orderItemDataSet.OrderItems);
            var ManagerCode = orderItemDataSet.OrderItems.Where(c => c.ClientId == LocalUser.Instance.LogInInformation.ClientId).Select(c => new { c.ItemCode }).OrderByDescending(c => c.ItemCode).ToArray();



            if (ManagerCode.Count() > 0)
            {
                var sManagerCode = 1001;
                var ManagerCodeCandidate = orderItemDataSet.OrderItems.Where(c => c.ClientId == LocalUser.Instance.LogInInformation.ClientId).OrderBy(c => c.ItemCode).Select(c => c.ItemCode).ToArray();
                while (true)
                {
                    if (!ManagerCodeCandidate.Any(c => c == sManagerCode.ToString()))
                    {
                        break;
                    }
                    sManagerCode++;
                }
                ItemCode.Text = sManagerCode.ToString();
            }
            else
            {

                ItemCode.Text = "1001";
            }



        }

        public bool IsSuccess = false;
        public OrderItemDataSet.OrderItemsRow CurrentCode = null;

        private int _UpdateDB()
        {
            err.Clear();

            if (ItemCode.Text == "")
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                err.SetError(ItemCode, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));

            }
            else if (orderItemDataSet.OrderItems.Where(c => c.ItemCode == ItemCode.Text && c.ClientId == LocalUser.Instance.LogInInformation.ClientId).Count() > 0)
            {

                MessageBox.Show("코드가 중복되었습니다.!!", "코드 입력 오류");
                err.SetError(ItemCode, "코드가 중복되었습니다.!!");
                return -1;

            }

            if (ItemName.Text == "")
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목누락");
                err.SetError(ItemName, ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1));
                return -1;

            }


        






            try
            {
                _AddClient();
                return 1;
            }
            catch (NoNullAllowedException ex)
            {
               

                MessageBox.Show(ButtonMessage.GetMessage(MessageType.필수항목누락, "", -1), "필수항목 누락");
                return 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "추가 작업 실패");
                return 0;
            }

        }
        private void _AddClient()
        {
            OrderItemDataSet.OrderItemsRow row = orderItemDataSet.OrderItems.NewOrderItemsRow();

            CurrentCode = row;

            row.ItemCode = ItemCode.Text;

            row.ItemName = ItemName.Text;

          



            row.CreateDate = DateTime.Parse(CreateDate.Text);
            row.Remark = Remark.Text;


            //  row.DriverId = int.Parse(txt_DriverId.Text);
            row.ClientId = LocalUser.Instance.LogInInformation.ClientId;


            orderItemDataSet.OrderItems.AddOrderItemsRow(row);
            

            try
            {

                orderItemTableAdapter.Update(row);
            }
            catch
            {
                MessageBox.Show("품목코드 추가 실패하였습니다.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (_UpdateDB() > 0)
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.추가성공, "품목코드", 1), "품목코드 추가 성공");


                ItemCode_Add();
                ItemName.Text = "";
                Remark.Text = "";
                CreateDate.Text = DateTime.Now.ToString("yyyy-MM-dd").Replace("-", "/");

              



            }

            ItemName.Focus();
        }

        private void btnAddClose_Click(object sender, EventArgs e)
        {
            if (_UpdateDB() > 0)
            {
                MessageBox.Show(ButtonMessage.GetMessage(MessageType.추가성공, "품목코드", 1), "품목코드 추가 성공");
                IsSuccess = true;
                Close();
            }
            else
            {

            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
