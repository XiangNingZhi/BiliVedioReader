using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using BiliViewReader3.Model;
using Microsoft.Win32;

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

        #region 查询按钮单击事件
        /// <summary>
        /// 根据文本框填入的ID（只能为数字）查询该用户的所有投稿信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ID_Select(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(uidBox.Text, out int num))
            {
                ReaderView.ItemsSource = null;
                int count = BiliBiliReader.GetVedioCount(num);
                CountLabel.Content = "视频数量：" + count;
                int PageNum = 1;
                List<UpMessage> list = new List<UpMessage>();
                if (count > 100)
                {
                    for (int i = 100; i < count; i += 100)
                    {
                        list.AddRange(BiliBiliReader.GetUP(num, PageNum));
                        PageNum++;
                    }
                    if ((count - (100 * (PageNum - 1))) > 0)
                    {
                        list.AddRange(BiliBiliReader.GetUP(num, PageNum));
                    }
                }
                else
                {
                    list = BiliBiliReader.GetUP(num, 1);
                }
                ReaderView.ItemsSource = list;
                
            }
            else
            {
                MessageBox.Show("输入ID必须为数字！");
            }
        }
        #endregion

        #region 背景图加载方法
        /// <summary>
        /// 窗体加载时加载背景图
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            string str = GetPic();
            if (str != null)
            {
                bgImg.ImageSource = new BitmapImage(new Uri(str, UriKind.Relative));
            }
        }

        /// <summary>
        /// 根据图片文件夹随机抽选一张图片作为背景图
        /// </summary>
        /// <returns></returns>
        private string GetPic()
        {
            if (Directory.Exists("pics"))
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
                    return pics[random.Next(0, pics.Count)];
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region 添加图片方法
        /// <summary>
        /// 添加图片按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bg_add(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.DefaultExt = "*.png|*.jpg";

            //获取文件路径
            bool? result = ofd.ShowDialog();
            //判断是否选择文件并判断文件是否为图片
            if (result == true && FileType.IsImage(ofd.FileName))
            {
                //调用保存文件方法，并将背景图加载进当前运行程序
                string file = ofd.FileName;
                var bg_path = img_Save(file);
                bgImg.ImageSource = new BitmapImage(new Uri(bg_path, UriKind.Relative));
            }
        }

        /// <summary>
        /// 文件保存方法，如果程序目录下没有图片文件夹则会新建
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private string img_Save(string fileName)
        {
            if (Directory.Exists("pics"))
            {
                string newFileName = $"{Directory.GetCurrentDirectory()}/pics/{Guid.NewGuid().ToString() + Path.GetExtension(fileName) }";
                File.Copy(fileName, newFileName, true);
                return newFileName;
            }
            else
            {
                Directory.CreateDirectory("pics");
                string newFileName = $"{Directory.GetCurrentDirectory()}/pics/{Guid.NewGuid().ToString() + Path.GetExtension(fileName) }";
                File.Copy(fileName, newFileName, true);
                return newFileName;
            }
        }
        #endregion

        #region 查看视频详细方法
        /// <summary>
        /// 双击列表会根据选中行显示该视频的详细信息，并且获取封面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReaderView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (ReaderView.SelectedValue != null)
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
                VedioView.Pic_Name = vedio.pic;
                vv.Show();
            }
        }
        #endregion

        #region 点击列表的列头进行排序
        private void View_Sort(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource is GridViewColumnHeader)
            {
                //获取选中列
                var clickedColumn = (e.OriginalSource as GridViewColumnHeader).Column;
                if (clickedColumn != null)
                {
                    //获取选中列的数据源名字
                    string bindingProperty = (clickedColumn.DisplayMemberBinding as Binding).Path.Path;
                    //获取列表排序信息
                    var desColle = this.ReaderView.Items.SortDescriptions;
                    //默认进行一次升序排序（因为B站的数据本身是按照时间降序的）
                    ListSortDirection sortDirection = ListSortDirection.Ascending;
                    //判断是否有排序规则
                    if (desColle.Count > 0)
                    {
                        SortDescription sd = desColle[0];
                        sortDirection = (ListSortDirection)((((int)sd.Direction) + 1) % 2);//如果是升序则反转为降序，如果是降序反之
                        desColle.Clear();
                    }
                    //加入排序规则
                    desColle.Add(new SortDescription(bindingProperty, sortDirection));
                }
            }
        }
        #endregion
    }
}
