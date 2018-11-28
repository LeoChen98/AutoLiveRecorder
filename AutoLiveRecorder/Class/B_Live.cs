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
        public static bool GetRoomInfo(ref Cls_WorkListItem item)
        {
            try
            {
                string str = Bas.GetBody(item.URL);
                if (!string.IsNullOrEmpty(str))
                {
                    Regex reg = new Regex("<script>window.__NEPTUNE_IS_MY_WAIFU__={.*}</script>");
                    string jsonstr = reg.Match(str).Value;
                    jsonstr = Regex.Split(Regex.Split(jsonstr, "window.__NEPTUNE_IS_MY_WAIFU__=")[1], "</script>")[0];

                    item.Platform = Cls_WorkListItem.PlatformType.Bilibili;

                    item.Roomid = Bas.GetJsonValueByKey(jsonstr, "roomInitRes/data/room_id").ToString();
                    item.RoomStatus = (int)Bas.GetJsonValueByKey(jsonstr, "roomInitRes/data/live_status");
                    item.RoomTitle = Bas.GetJsonValueByKey(jsonstr, "baseInfoRes/data/title").ToString();

                    if (item.RoomStatus == 1)
                    {
                        List<string> Urls = new List<string>();
                        foreach (Dictionary<string, object> i in (ArrayList)Bas.GetJsonValueByKey(jsonstr, "playUrlRes/data/durl"))
                        {
                            Urls.Add(Bas.GetJsonValueByKey(i, "url").ToString());
                        }
                        item.VideoUrl = Urls.ToArray();
                    }

                    item.IsSupportDanmu = true;

                    if (str.Contains("uname"))
                    {
                        item.Host = Regex.Split(new Regex("\"uname\":.*,").Match(str).Value, "\"")[3];
                    }
                    else
                    {
                        string strhost = Bas.GetBody("https://api.live.bilibili.com/live_user/v1/UserInfo/get_anchor_in_room?roomid=" + item.Roomid);
                        item.Host = Bas.GetJsonValueByKey(strhost, "data/info/uname").ToString();
                    }

                    item.CallPropertyChanged("RoomInfoLong");

                    return true;
                }
                else
                {
                    item.Platform = Cls_WorkListItem.PlatformType.None;

                    item.CallPropertyChanged("RoomInfoLong");

                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 获取视频地址
        /// </summary>
        /// <param name="URL">直播间地址</param>
        /// <returns>视频地址集合</returns>
        public static string[] GetVideoURL(string URL)
        {
            string str = Bas.GetBody(URL);
            Regex reg = new Regex("<script>window.__NEPTUNE_IS_MY_WAIFU__={.*}</script>");
            string jsonstr = reg.Match(str).Value;
            jsonstr = Regex.Split(Regex.Split(jsonstr, "window.__NEPTUNE_IS_MY_WAIFU__=")[1], "</script>")[0];

            List<string> Urls = new List<string>();
            foreach (Dictionary<string, object> i in (ArrayList)Bas.GetJsonValueByKey(jsonstr, "playUrlRes/data/durl"))
            {
                Urls.Add(Bas.GetJsonValueByKey(i, "url").ToString());
            }
            return Urls.ToArray();
        }

        /// <summary>
        /// 查询直播间是否在播
        /// </summary>
        /// <param name="URL">直播间地址</param>
        /// <returns>是否在播</returns>
        public static bool IsLiving(string URL)
        {
            string str = Bas.GetBody(URL);
            Regex reg = new Regex("<script>window.__NEPTUNE_IS_MY_WAIFU__={.*}</script>");
            string jsonstr = reg.Match(str).Value;
            jsonstr = Regex.Split(Regex.Split(jsonstr, "window.__NEPTUNE_IS_MY_WAIFU__=")[1], "</script>")[0];
            if ((int)Bas.GetJsonValueByKey(jsonstr, "roomInitRes/data/live_status") == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion Public Methods
    }
}