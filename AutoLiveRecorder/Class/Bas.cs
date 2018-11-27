using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;

namespace AutoLiveRecorder
{
    internal class Bas
    {
        #region Public Methods

        /// <summary>
        /// URL解析
        /// </summary>
        /// <param name="URL"></param>
        /// <returns></returns>
        public static string AnalysisURL(string URL)
        {
            if (URL.Length == 0) return "";
            Regex reg = new Regex("[^\\s]/\\d+");
            if (URL.Contains("live.bilibili.com") && reg.Match(URL).Success)
            {
                return B_Live.GetRoomInfo(URL);
            }
            return "URL格式错误或平台未支持。";
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
                req.Headers.Add("Origin: https://live.bilibili.com");
                req.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8";
                req.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/67.0.3396.99 Safari/537.36";

                if (roomid != "") req.Referer = "https://live.bilibili.com/" + roomid;
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
            catch (Exception ex)
            {
                return "";
            }
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