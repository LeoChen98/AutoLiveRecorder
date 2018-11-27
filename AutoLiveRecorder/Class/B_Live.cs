using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AutoLiveRecorder
{
    /// <summary>
    /// B站直播类
    /// </summary>
    internal class B_Live
    {
        #region Public Methods

        /// <summary>
        /// 获取房间信息
        /// </summary>
        /// <param name="item">任务实例</param>
        /// <returns></returns>
        public static Cls_WorkListItem GetRoomInfo(Cls_WorkListItem item)
        {
            string str = Bas.GetBody(item.URL);
            Regex reg = new Regex("<script>window.__NEPTUNE_IS_MY_WAIFU__={.*}</script>");
            string jsonstr = reg.Match(str).Value;
            jsonstr = Regex.Split(Regex.Split(jsonstr, "window.__NEPTUNE_IS_MY_WAIFU__=")[1], "</script>")[0];

            item.Roomid = Bas.GetJsonValueByKey(jsonstr, "roomInitRes/data/room_id").ToString();
            switch ((int)Bas.GetJsonValueByKey(jsonstr, "roomInitRes/data/live_status"))
            {
                case 1:
                    item.IsLiving = true;
                    break;

                default:
                    item.IsLiving = false;
                    break;
            }
            item.RoomTitle = Bas.GetJsonValueByKey(jsonstr, "baseInfoRes/data/title").ToString();

            List<string> Urls = new List<string>();
            foreach (Dictionary<string, object> i in (ArrayList)Bas.GetJsonValueByKey(jsonstr, "playUrlRes/data/durl"))
            {
                Urls.Add(Bas.GetJsonValueByKey(i, "url").ToString());
            }
            item.VideoUrl = Urls.ToArray();

            item.IsSupportDanmu = true;

            item.Host = Regex.Split(new Regex("\"uname\":.*,").Match(str).Value, "\"")[3];

            return item;
        }

        /// <summary>
        /// 获取房间信息
        /// </summary>
        /// <param name="URL">直播间地址</param>
        /// <returns></returns>
        public static string GetRoomInfo(string URL)
        {
            string str = Bas.GetBody(URL);
            if (!string.IsNullOrEmpty(str))
            {
                Regex reg = new Regex("<script>window.__NEPTUNE_IS_MY_WAIFU__={.*}</script>");
                string jsonstr = reg.Match(str).Value;
                jsonstr = Regex.Split(Regex.Split(jsonstr, "window.__NEPTUNE_IS_MY_WAIFU__=")[1], "</script>")[0];

                string livestatus;
                switch ((int)Bas.GetJsonValueByKey(jsonstr, "roomInitRes/data/live_status"))
                {
                    case 1:
                        livestatus = "在播";
                        break;

                    default:
                        livestatus = "离线";
                        break;
                }

                return
                    "房间号：" + Bas.GetJsonValueByKey(jsonstr, "roomInitRes/data/room_id").ToString() + "\r\n" +
                    "标题：" + Bas.GetJsonValueByKey(jsonstr, "baseInfoRes/data/title").ToString() + "\r\n" +
                    "主播：" + Regex.Split(new Regex("\"uname\":.*,").Match(str).Value, "\"")[3] + "\r\n" +
                    "直播状态：" + livestatus + "\r\n" +
                    "是否支持弹幕录制：支持";
            }
            else
            {
                return "";
            }
        }

        #endregion Public Methods
    }
}