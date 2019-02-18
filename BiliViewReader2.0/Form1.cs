using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.IO;
using System.IO.Compression;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BiliViewReader2._0
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private async void SelectButton_Click(object sender, EventArgs e)
        {
            ViewBox.Clear();
            if (string.IsNullOrEmpty(uidBox.Text))
            {
                MessageBox.Show("uid不能为空！");
            }
            else
            {
                List<Vedio> vedios = GetVedios(uidBox.Text);
                foreach (var vedio in vedios)
                {
                    await GetMessage(vedio);
                }
            }
        }

        /// <summary>
        /// 异步调用方法
        /// </summary>
        /// <param name="vedio">单个视频对象</param>
        /// <returns></returns>
        private async Task GetMessage(Vedio vedio)
        {
            string View = await Task.Run(() => GetView(vedio.aid));
            ViewBox.AppendText($"帐号 {vedio.author} 的视频 {vedio.title}（av{vedio.aid}） 的{View}" + System.Environment.NewLine);

        }


        /// <summary>
        /// 获取视频的总弹幕数和播放数
        /// </summary>
        /// <param name="av">视频av号</param>
        /// <returns></returns>
        private string GetView(string av)
        {
            string number = av.Replace("av", "");
            //获取B站网站视频的各项数据，这个链接可以取到弹幕和播放次数
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create("https://api.bilibili.com/x/web-interface/archive/stat?aid=" + number);
            //伪造浏览器（UserAgent是IE的）
            request.UserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 10.0; WOW64; Trident/7.0; .NET4.0C; .NET4.0E; .NET CLR 2.0.50727; .NET CLR 3.0.30729; .NET CLR 3.5.30729)";
            WebResponse response = request.GetResponse();//获取response
            StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);//获取输入流，实际为json
            string jsonStr = reader.ReadToEnd();
            reader.Close();//关闭流
            response.Close();//关闭响应

            JObject json = JObject.Parse(jsonStr);//转为json对象
            JToken data = json["data"];//取出键名为data的对象，里面包含各种数据
            int view = data["view"].Value<int>();//view为播放次数
            int danmaku = data["danmaku"].Value<int>();//danmaku为总弹幕数
            return "播放数为:" + view + "，总弹幕数为:" + danmaku + "。";
        }



        /// <summary>
        /// 根据uid返回该用户所有的视频数据
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        private List<Vedio> GetVedios(string uid)
        {
            int count = GetVedioCount(uid);
            //请求第二次，根据获取到的总数和uid一页显示所有的视频数据
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create($"https://space.bilibili.com/ajax/member/getSubmitVideos?mid={uid}&pagesize={count}&page=1");
            //伪造浏览器（UserAgent是IE的）
            request.UserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 10.0; WOW64; Trident/7.0; .NET4.0C; .NET4.0E; .NET CLR 2.0.50727; .NET CLR 3.0.30729; .NET CLR 3.5.30729)";
            WebResponse response = request.GetResponse();//获取response
            StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);//获取输入流，实际为json
            string JsonStr = reader.ReadToEnd();
            reader.Close();//关闭流
            response.Close();//关闭响应

            JObject json = JObject.Parse(JsonStr);
            //如果请求没问题，返回的json中状态这个键的值会为true
            if (json["status"].Value<bool>())
            {
                //所有的视频数据在data对象的vlist数组里
                var data = json["data"];
                var vlist = (JArray)data["vlist"];//vlist为对象数组
                List<Vedio> vedios = new List<Vedio>();
                foreach (var item in vlist)
                {
                    //单个视频赋值
                    Vedio v = new Vedio
                    {
                        aid = item["aid"].Value<string>(),
                        author = item["author"].Value<string>(),
                        title = item["title"].Value<string>(),
                        length = item["length"].Value<string>()
                    };
                    vedios.Add(v);
                }
                return vedios;
            }
            else
            {
                MessageBox.Show("查询内容有误");
            }
            return new List<Vedio>();
        }

        /// <summary>
        /// 根据uid获取该用户投稿的所有视频
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <returns></returns>
        private int GetVedioCount(string uid)
        {
            //请求地址参数：mid=用户id，pagesize等于分页大小，page=页数（为用户个人主页查看用的ajax）
            //先请求第一次，取count字段获得视频总数
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create($"https://space.bilibili.com/ajax/member/getSubmitVideos?mid={uid}&pagesize=1&page=1");
            //伪造浏览器（UserAgent是IE的）
            request.UserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 10.0; WOW64; Trident/7.0; .NET4.0C; .NET4.0E; .NET CLR 2.0.50727; .NET CLR 3.0.30729; .NET CLR 3.5.30729)";
            WebResponse response = request.GetResponse();//获取response
            StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);//获取输入流，实际为json
            string JsonStr = reader.ReadToEnd();
            reader.Close();//关闭流
            response.Close();//关闭响应

            JObject json = JObject.Parse(JsonStr);
            //如果请求没问题，返回的json中状态这个键的值会为true
            if (json["status"].Value<bool>())
            {
                var data = json["data"];//取出data段
                var count = data["count"].Value<int>();//取出count段
                return count;
            }
            else
            {
                MessageBox.Show("查询内容有误");
            }
            return 0;
        }
    }

    #region 实体对象
    public class Vedio
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// AV号
        /// </summary>
        public string aid { get; set; }
        /// <summary>
        /// 用户名称
        /// </summary>
        public string author { get; set; }
        /// <summary>
        /// 视频长度
        /// </summary>
        public string length { get; set; }
    }
    #endregion

}
