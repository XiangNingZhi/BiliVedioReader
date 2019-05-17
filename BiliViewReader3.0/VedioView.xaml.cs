using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Win32;

namespace BiliViewReader3
{
    /// <summary>
    /// VedioView.xaml 的交互逻辑
    /// </summary>
    public partial class VedioView : Window
    {
        public VedioView()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 从B站获取到的封面图片路径
        /// </summary>
        public static string Pic_Name { get; set; }

        /// <summary>
        /// 保存封面方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "Image Files (*.bmp, *.png, *.jpg)|*.bmp;*.png;*.jpg | All Files | *.*";
                sfd.FileName = Pic_Name.Substring(Pic_Name.LastIndexOf('/') + 1);//取最后一个斜杠后面所有的文本，即可取到图片名（作为保存的默认图片名）
                if (sfd.ShowDialog() == true)
                {
                    var encoder = new JpegBitmapEncoder();
                    //取出Image控件里的文件
                    encoder.Frames.Add(BitmapFrame.Create((BitmapSource)this.titlePic.Source));
                    //创建文件流
                    using (FileStream fs = new FileStream(sfd.FileName, FileMode.Create, FileAccess.ReadWrite))
                    {
                        encoder.Save(fs);
                    }
                }
                MessageBox.Show("保存成功");
            }
            catch (Exception)
            {
                MessageBox.Show("发生异常");
            }
        }
    }
}
