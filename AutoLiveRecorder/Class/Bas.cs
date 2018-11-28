using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;

namespace AutoLiveRecorder
{
    /// <summary>
    /// 全局模块
    /// </summary>
    internal class Bas
    {
        #region Public Fields

        /// <summary>
        /// 任务列表
        /// </summary>
        public static List<Cls_WorkListItem> TaskList;

        #endregion Public Fields

        #region Public Methods

        /// <summary>
        /// URL解析
        /// </summary>
        /// <param name="URL"></param>
        /// <returns></returns>
        public static void AnalysisURL(string URL, ref Cls_WorkListItem item)
        {
            if (URL.Length == 0) return;
            Regex reg = new Regex("[^\\s]/\\d+");
            if (URL.Contains("live.bilibili.com") && reg.Match(URL).Success)
            {
                item.URL = URL;
                B_Live.GetRoomInfo(ref item);
            }
            else
            {
                item.Platform = Cls_WorkListItem.PlatformType.None;
                item.CallPropertyChanged("RoomInfoLong");
            }
        }

        /// <summary>
        /// 退出程序
        /// </summary>
        public static void Exit()
        {
            Environment.Exit(0);
        }

        /// <summary>
        /// HTTP GET方法封装
        /// </summary>
        /// <param name="url">GET地址</param>
        /// <returns>接口内容</returns>
        public static string GetBody(string url, string roomid = "")
        {
            try
            {
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);

                req.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8";
                req.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/67.0.3396.99 Safari/537.36";

                HttpWebResponse rep = (HttpWebResponse)req.GetResponse();
                StreamReader reader = new StreamReader(rep.GetResponseStream());
                string r = reader.ReadToEnd();
                reader.Close();
                rep.Close();
                if (req != null) req.Abort();
                return r;
            }
            catch (WebException wex)
            {
                if (wex.Status != WebExceptionStatus.ConnectionClosed && wex.Status != WebExceptionStatus.KeepAliveFailure && wex.Status != WebExceptionStatus.NameResolutionFailure && wex.Status != WebExceptionStatus.ProtocolError)
                {
                }
                return "";
            }
            catch (Exception)
            {
                return "";
            }
        }

        /// <summary>
        /// 获取可用文件名
        /// </summary>
        /// <param name="FileName">不带后缀名的文件名</param>
        /// <param name="ExtName">后缀名</param>
        /// <param name="Path">目录路径</param>
        /// <returns>可用文件名（完整路径）</returns>
        public static string GetFreeFileName(string FileName, string ExtName, string Path)
        {
            if (!File.Exists(Path + "\\" + FileName + "." + ExtName)) return Path + "\\" + FileName + "." + ExtName;

            int i = 1;

            while (File.Exists(Path + "\\" + FileName + "-" + i + "." + ExtName))
            {
                i++;
            }
            return Path + "\\" + FileName + "-" + i + "." + ExtName;
        }

        /// <summary>
        /// 获取可用的缓存文件名
        /// </summary>
        /// <param name="FileName">最终文件的完整路径</param>
        /// <returns>可用的缓存文件路径</returns>
        public static string GetFreeTmpFileName(string FileName)
        {
            string[] a = FileName.Split('.');
            string r = "";
            for (int i = 0; i < a.Length; i++)
            {
                if (i == a.Length - 2)
                {
                    r += a[i] + "-{id}.";
                }
                else
                {
                    r += a[i] + ".";
                }
            }
            r += "tmp";

            int j = 1;
            string t = r.Replace("{id}", j.ToString());
            while (File.Exists(t))
            {
                j++;
                t = r.Replace("{id}", j.ToString());
            }
            return t;
        }

        /// <summary>
        /// 解析Json
        /// </summary>
        /// <param name="JsonStr">Json字符串</param>
        /// <param name="Keys">路径（反斜杠分割）</param>
        /// <param name="step">第几步</param>
        /// <returns>值</returns>
        public static object GetJsonValueByKey(string JsonStr, string Keys, int step = 0)
        {
            string Key = Keys.Split('/')[step];
            int TolStep = Keys.Split('/').Length;
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            Dictionary<string, object> p = serializer.Deserialize<Dictionary<string, object>>(JsonStr);
            if (p.ContainsKey(Key))
            {
                if (p[Key].GetType() == typeof(ArrayList) && step < TolStep - 1)
                {
                    ArrayList al = (ArrayList)p[Key];
                    List<object> results = new List<object>();
                    foreach (Dictionary<string, object> i in al)
                    {
                        results.Add(GetJsonValueByKey(i, Keys, step + 1));
                    }
                    return results;
                }
                else
                if (p[Key].GetType() == typeof(Dictionary<string, object>) && step < TolStep - 1)
                {
                    Dictionary<string, object> di = (Dictionary<string, object>)p[Key];
                    return GetJsonValueByKey(di, Keys, step + 1);
                }
                else
                {
                    return p[Key];
                }
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// 解析Json
        /// </summary>
        /// <param name="JsonObject">Json对象</param>
        /// <param name="Keys">路径（反斜杠分割）</param>
        /// <param name="step">第几步</param>
        /// <returns>值</returns>
        public static object GetJsonValueByKey(Dictionary<string, object> JsonObject, string Keys, int step = 0)
        {
            string Key = Keys.Split('/')[step];
            int TolStep = Keys.Split('/').Length;
            if (JsonObject.ContainsKey(Key))
            {
                if (JsonObject[Key].GetType() == typeof(ArrayList) && step < TolStep - 1)
                {
                    ArrayList al = (ArrayList)JsonObject[Key];
                    List<object> results = new List<object>();
                    foreach (Dictionary<string, object> i in al)
                    {
                        results.Add(GetJsonValueByKey(i, Keys, step + 1));
                    }
                    return results;
                }
                else
                if (JsonObject[Key].GetType() == typeof(Dictionary<string, object>) && step < TolStep - 1)
                {
                    Dictionary<string, object> di = (Dictionary<string, object>)JsonObject[Key];
                    return GetJsonValueByKey(di, Keys, step + 1);
                }
                else
                {
                    return JsonObject[Key];
                }
            }
            else
            {
                return "";
            }
        }

        #endregion Public Methods
    }
}