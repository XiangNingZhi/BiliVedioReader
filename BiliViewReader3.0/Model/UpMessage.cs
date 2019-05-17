using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiliViewReader3.Model
{
    public class UpMessage
    {
        /// <summary>
        /// AV号
        /// </summary>
        public string aid { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 视频简介
        /// </summary>
        public string description { get; set; }
        /// <summary>
        /// 投稿时间
        /// </summary>
        public DateTime created { get; set; }
        /// <summary>
        /// 视频长度
        /// </summary>
        public string length { get; set; }
        /// <summary>
        /// 题图路径
        /// </summary>
        public string pic { get; set; }
        /// <summary>
        /// 播放数
        /// </summary>
        public int play { get; set; }
    }
}
