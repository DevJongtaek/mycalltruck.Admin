using mycalltruck.Admin.Class;
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
    public partial class Dialog_CustomerImage : Form
    {
        public Dialog_CustomerImage()
        {
            InitializeComponent();
        }

        public void LoadCutomer(int CustomerId)
        {
            Data.Connection((_Connection) =>
            {
                int ImageId1 = 0;
                int ImageId2 = 0;
                int ImageId3 = 0;
                int ImageId4 = 0;
                int ImageId5 = 0;
                using (SqlCommand _Command = _Connection.CreateCommand())
                {
                    _Command.CommandText = $"SELECT SangHo, Image1, Image2, Image3, Image4, Image5 FROM Customers WHERE CustomerId = {CustomerId}";
                    using (SqlDataReader _Reader = _Command.ExecuteReader(CommandBehavior.SingleRow))
                    {
                        if (_Reader.Read())
                        {
                            CustomerName.Text = _Reader.GetString(0);
                            if (!_Reader.IsDBNull(1))
                            {
                                ImageId1 = _Reader.GetInt32(1);
                            }
                            if (!_Reader.IsDBNull(2))
                            {
                                ImageId2 = _Reader.GetInt32(2);
                            }
                            if (!_Reader.IsDBNull(3))
                            {
                                ImageId3 = _Reader.GetInt32(3);
                            }
                            if (!_Reader.IsDBNull(4))
                            {
                                ImageId4 = _Reader.GetInt32(4);
                            }
                            if (!_Reader.IsDBNull(5))
                            {
                                ImageId5 = _Reader.GetInt32(5);
                            }
                        }
                    }
                }
                if(ImageId1 > 0)
                {
                    using (SqlCommand _Command = _Connection.CreateCommand())
                    {
                        _Command.CommandText = $"SELECT ISNULL(Name, '') FROM ImageReferences WHERE ImageReferenceId = {ImageId1}";
                        using (SqlDataReader _Reader = _Command.ExecuteReader(CommandBehavior.SingleRow))
                        {
                            if (_Reader.Read())
                            {
                                ImageName1.Text = _Reader.GetString(0);
                                Image1.LoadAsync($"http://m.cardpay.kr/ImageFromAdmin/GetImage?ImageReferenceId={ImageId1}");
                            }
                        }
                    }
                }
                if (ImageId2 > 0)
                {
                    using (SqlCommand _Command = _Connection.CreateCommand())
                    {
                        _Command.CommandText = $"SELECT ISNULL(Name, '') FROM ImageReferences WHERE ImageReferenceId = {ImageId2}";
                        using (SqlDataReader _Reader = _Command.ExecuteReader(CommandBehavior.SingleRow))
                        {
                            if (_Reader.Read())
                            {
                                ImageName2.Text = _Reader.GetString(0);
                                Image2.LoadAsync($"http://m.cardpay.kr/ImageFromAdmin/GetImage?ImageReferenceId={ImageId2}");
                            }
                        }
                    }
                }
                if (ImageId3 > 0)
                {
                    using (SqlCommand _Command = _Connection.CreateCommand())
                    {
                        _Command.CommandText = $"SELECT ISNULL(Name, '') FROM ImageReferences WHERE ImageReferenceId = {ImageId3}";
                        using (SqlDataReader _Reader = _Command.ExecuteReader(CommandBehavior.SingleRow))
                        {
                            if (_Reader.Read())
                            {
                                ImageName3.Text = _Reader.GetString(0);
                                Image3.LoadAsync($"http://m.cardpay.kr/ImageFromAdmin/GetImage?ImageReferenceId={ImageId3}");
                            }
                        }
                    }
                }
                if (ImageId4 > 0)
                {
                    using (SqlCommand _Command = _Connection.CreateCommand())
                    {
                        _Command.CommandText = $"SELECT ISNULL(Name, '') FROM ImageReferences WHERE ImageReferenceId = {ImageId4}";
                        using (SqlDataReader _Reader = _Command.ExecuteReader(CommandBehavior.SingleRow))
                        {
                            if (_Reader.Read())
                            {
                                ImageName4.Text = _Reader.GetString(0);
                                Image4.LoadAsync($"http://m.cardpay.kr/ImageFromAdmin/GetImage?ImageReferenceId={ImageId4}");
                            }
                        }
                    }
                }
                if (ImageId5 > 0)
                {
                    using (SqlCommand _Command = _Connection.CreateCommand())
                    {
                        _Command.CommandText = $"SELECT ISNULL(Name, '') FROM ImageReferences WHERE ImageReferenceId = {ImageId5}";
                        using (SqlDataReader _Reader = _Command.ExecuteReader(CommandBehavior.SingleRow))
                        {
                            if (_Reader.Read())
                            {
                                ImageName5.Text = _Reader.GetString(0);
                                Image5.LoadAsync($"http://m.cardpay.kr/ImageFromAdmin/GetImage?ImageReferenceId={ImageId5}");
                            }
                        }
                    }
                }
            });

            if (!this.Visible)
                this.Show();
            this.Activate();

            ImageContainerScrollbar.Value = 0;
        }

        private void ImageContainerScrollbar_Scroll(object sender, ScrollEventArgs e)
        {
            ImageContainer.Top = e.NewValue * -1;
        }

        private void Dialog_CustomerImage_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }
    }
}
