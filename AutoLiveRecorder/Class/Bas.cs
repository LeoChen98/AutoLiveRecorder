using System;
using System.Text.RegularExpressions;

namespace AutoLiveRecorder
{
    internal class Bas
    {
        #region Public 方法

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
                return "B站";
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

        #endregion Public 方法
    }
}