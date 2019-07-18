using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;
using BiliViewReader3.Model;
using System.Net;
using System.IO;
using System.Windows;

namespace BiliViewReader3
{
    public class BiliBiliReader
    {
        /// <summary>
        /// 根据uid返回该用户所有的视频数据
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="count">视频总数</param>
        /// <returns></returns>
        public static List<UpMessage> GetUP(int uid, int PageNum)
        {
            //请求第二次，根据获取到的总数和uid一页显示所有的视频数据
            //注意：该API地址最多请求的每页条数是100，如果超过100则会返回参数错误，需分开加载
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create($"https://space.bilibili.com/ajax/member/getSubmitVideos?mid={uid}&pagesize=100&page={PageNum}");
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
                List<UpMessage> vedios = new List<UpMessage>();
                foreach (var item in vlist)
                {
                    //单个视频赋值
                    UpMessage v = new UpMessage
                    {
                        aid = "av" + item["aid"].Value<string>(),
                        title = item["title"].Value<string>(),
                        length = item["length"].Value<string>(),
                        created = timeToString(item["created"].Value<string>()),
                        description = item["description"].Value<string>(),
                        play = item["play"].Value<string>() == "--" ? -1 : item["play"].Value<int>(),//遇到了播放数有问题，B站上显示“--”的情况
                        pic = item["pic"].Value<string>()
                    };
                    vedios.Add(v);
                }
                return vedios;
            }
            else
            {
                return new List<UpMessage>();
            }
        }

        /// <summary>
        /// 根据uid获取该用户投稿的所有视频数量
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <returns></returns>
        public static int GetVedioCount(int uid)
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

        /// <summary>
        /// 获取B站封面图
        /// </summary>
        /// <param name="url">地址</param>
        /// <returns></returns>
        public static byte[] getPic(string url)
        {
            //请求图片网站
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http:" + url);
            //伪造浏览器（UserAgent是IE的）
            request.UserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 10.0; WOW64; Trident/7.0; .NET4.0C; .NET4.0E; .NET CLR 2.0.50727; .NET CLR 3.0.30729; .NET CLR 3.5.30729)";
            using (Stream stream = request.GetResponse().GetResponseStream())
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    byte[] buffer = new byte[1024];
                    int current = 0;
                    do
                    {
                        ms.Write(buffer, 0, current);
                    } while ((current = stream.Read(buffer, 0, buffer.Length)) != 0);
                    return ms.ToArray();
                }
            }
        }


        /// <summary>
        /// 时间戳转日期
        /// </summary>
        /// <returns></returns>
        public static DateTime timeToString(string timestamp)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = long.Parse(timestamp + "0000000");
            TimeSpan toNow = new TimeSpan(lTime);
            DateTime time = dtStart.Add(toNow);
            //return $"{time.Year}年{time.Month}月{time.Day}日 {time.TimeOfDay}";//废弃方案，以前直接转时间文本现在返回datetime方便ListView排序
            return time;
        }
    }
}
