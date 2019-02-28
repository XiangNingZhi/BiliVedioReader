using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BiliViewReader3.Model;

namespace BiliViewReader3
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        //查询按钮单击时间
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(uidBox.Text, out int num))
            {
                ReaderView.ItemsSource = null;
                int count = BiliBiliReader.GetVedioCount(num);
                CountLabel.Content = "视频数量：" + count;
                List<UpMessage> list = BiliBiliReader.GetUP(num, count);
                ReaderView.ItemsSource = list;
            }
            else
            {
                MessageBox.Show("输入ID必须为数字！");
            }
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            string str = GetPic();
            if (str != null)
            {
                bgImg.ImageSource = new BitmapImage(new Uri(str, UriKind.Relative));
            }
        }


        private string GetPic()
        {
            List<string> list = new List<string> { "*.png", "*.jpg" };
            List<string> pics = new List<string>();
            foreach (var item in list)
            {
                pics.AddRange(Directory.GetFiles("pics", item).ToList());
            }
            Random random = new Random();
            if (pics.Count > 0)
            {
                return pics[random.Next(0, pics.Count - 1)];
            }
            else
            {
                return null;
            }
        }

        private void ReaderView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            UpMessage vedio = (UpMessage)ReaderView.SelectedValue;
            VedioView vv = new VedioView();
            vv.Title = $"{vedio.aid}详细信息";
            vv.TitleLabel.Content = vedio.title;
            vv.descriptionBox.Text = vedio.description;
            vv.aidLabel.Content = $"av号：{vedio.aid}";
            vv.playLabel.Content = $"播放数：{vedio.play}";
            vv.lengthLabel.Content = $"视频长度：{vedio.length}";
            vv.createdLabel.Content = $"审核通过时间：{vedio.created}";
            ImageSourceConverter imageSourceConverter = new ImageSourceConverter();
            vv.titlePic.Source = (ImageSource)imageSourceConverter.ConvertFrom(BiliBiliReader.getPic(vedio.pic));
            vv.Show();
        }
    }
}
