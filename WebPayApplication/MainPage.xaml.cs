using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Shapes;
using System.Json;
using System.IO;
using System.Windows.Browser;

namespace WebPayApplication
{
    public partial class MainPage : UserControl
    {
        public MainPage(String iTransactionId)
        {
            InitializeComponent();
            PasswordButtonArray.Add(Password0);
            PasswordButtonArray.Add(Password1);
            PasswordButtonArray.Add(Password2);
            PasswordButtonArray.Add(Password3);
            PasswordButtonArray.Add(Password4);
            PasswordButtonArray.Add(Password5);
            PasswordButtonArray.Add(Password6);
            PasswordButtonArray.Add(Password7);
            PasswordButtonArray.Add(Password8);
            PasswordButtonArray.Add(Password9);
            for (int i = 1; i < 13; i++)
            {
                CardMonthInput.Items.Add(i.ToString());
            }
            CardMonthInput.SelectedIndex = 0;
            for (int i = 0; i < 7; i++)
            {
                CardYearInput.Items.Add(DateTime.Now.AddYears(i).Year.ToString());
            }
            CardYearInput.SelectedIndex = 0;
            WebClient mWebClient = new WebClient();
            var Url = HtmlPage.Document.DocumentUri.AbsoluteUri.Replace(HtmlPage.Document.DocumentUri.LocalPath, "");
            Url += String.Format("/api/Pay?TransactionId={0}", iTransactionId);
            mWebClient.UploadStringCompleted += (sender, e) =>
            {
                if (e.Cancelled || e.Error != null)
                {
                    MessageBox.Show("잘못된 접근 입니다.", "카드페이", MessageBoxButton.OK);
                    CloseApplication();
                    return;
                }
                List<Item> ItemsSource = new List<Item>();
                using (StringReader _Reader = new StringReader(e.Result))
                {
                    var TradeArray = JsonArray.Load(_Reader);
                    foreach (JsonObject Trade in TradeArray)
                    {
                        ItemsSource.Add(
                            new Item
                            {
                                TradeId = Trade["TradeId"],
                                Price = Trade["Price"],
                                Fee = Trade["Fee"],
                                Sum = Trade["Sum"],
                                Remark = Trade["Remark"],
                            });
                    }
                }
                Dispatcher.BeginInvoke(() =>
                {
                    TradeDataList.ItemsSource = ItemsSource;
                    SumLabel.Text = ItemsSource.Sum(c => c.Sum).ToString("N0");
                });
            };
            mWebClient.UploadStringAsync(new Uri(Url), "");
        }

        private void BtnAccept_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnPassword_Click(object sender, RoutedEventArgs e)
        {
            PasswordInput.Password = "";
            PasswordOverlay.Visibility = System.Windows.Visibility.Visible;
            PasswordBox.Visibility = System.Windows.Visibility.Visible;
        }
        List<Grid> PasswordButtonArray = new List<Grid>();
        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (PasswordInput.Password.Length > 1)
                return;
            foreach (Grid grid in PasswordButtonArray)
            {
                grid.Effect = new BlurEffect { Radius = 2 };
            }
        }

        private void Grid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (PasswordInput.Password.Length > 1)
                return;
            PasswordInput.Password += (sender as Grid).Tag.ToString();
            foreach (Grid grid in PasswordButtonArray)
            {
                grid.Effect = null;
            }
            if (PasswordInput.Password.Length > 2)
                PasswordInput.Password = PasswordInput.Password.Substring(0, 2);
            if (PasswordInput.Password.Length == 2)
            {
                PasswordOverlay.Visibility = System.Windows.Visibility.Collapsed;
                PasswordBox.Visibility = System.Windows.Visibility.Collapsed;
            }
        }

        private void CloseApplication()
        {
            HtmlPage.Document.GetElementById("form1").RemoveChild(HtmlPage.Document.GetElementById("silverlightControlHost"));
        }

        public class Item 
        {
            public int TradeId { get; set; }
            public decimal Price { get; set; }
            public decimal Fee { get; set; }
            public decimal Sum { get; set; }
            public decimal Remark { get; set; }
        }
    }
}
