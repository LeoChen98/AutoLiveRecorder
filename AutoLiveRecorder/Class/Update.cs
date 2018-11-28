using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace AutoLiveRecorder
{
    internal class Update
    {
        #region Public Methods

        /// <summary>
        /// 检查更新
        /// </summary>
        /// <param name="MustGUI">是否必须有界面提示</param>
        public static void CheckUpdate(bool MustGUI = false)
        {
            try
            {
                ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
                ServicePointManager.DefaultConnectionLimit = 512;

                HttpWebRequest req = (HttpWebRequest)WebRequest.Create("https://github.com/LeoChen98/AutoLiveRecorder/releases/latest");

                req.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8";
                req.KeepAlive = true;
                req.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/70.0.3538.110 Safari/537.36";
                HttpWebResponse rep = (HttpWebResponse)req.GetResponse();
                StreamReader reader = new StreamReader(rep.GetResponseStream());
                string str = reader.ReadToEnd();
                reader.Close();
                rep.Close();
                if (req != null) req.Abort();

                string newver = Regex.Split(Regex.Split(str, "<span class=\"css-truncate-target\" style=\"max-width: 125px\">")[1], "</span>")[0];
                string changes = GetChanges(str);
                string updatetime = Regex.Replace(Regex.Split(Regex.Split(str, "<relative-time datetime=\"")[1], "\"")[0], "(?<date>.*?)T(?<time>.*?)Z", "$1 $2");

                if (newver != Application.ProductVersion && newver != Properties.Settings.Default.IgnoreVersion)//与服务器版本不同且未被忽略
                {
                    if (MessageBox.Show("有新版本！（" + Application.ProductVersion + "→" + newver + "）\r\n\r\n更新内容：\r\n" + changes + "\r\n更新时间：" + updatetime + "\r\n是否开始更新？（更新会调用命令控制行程序并要求管理员权限。）", "有新版本！", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        WebClient client = new WebClient();
                        client.DownloadFileAsync(new Uri("https://github.com/LeoChen98/AutoLiveRecorder/releases/download/" + newver + "/AutoLiveRecorder.exe"), Application.StartupPath + "\\update.tmp");
                        client.DownloadFileCompleted += new System.ComponentModel.AsyncCompletedEventHandler((o, e) =>
                        {
                            DoUpdate();
                        }
                        );
                    }
                    else
                    {
                        if (MessageBox.Show("是否不再提示本次更新？", "检查更新 - 录播姬", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            Properties.Settings.Default.IgnoreVersion = newver;
                            Properties.Settings.Default.Save();
                        }
                    }
                }
                else if (MustGUI) MessageBox.Show("版本已是最新", "录播姬");
            }
            catch (WebException wex)
            {
                if (wex.Status != WebExceptionStatus.ConnectionClosed && wex.Status != WebExceptionStatus.KeepAliveFailure && wex.Status != WebExceptionStatus.NameResolutionFailure && wex.Status != WebExceptionStatus.ProtocolError)
                {
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// 执行更新
        /// </summary>
        public static void DoUpdate()
        {
            foreach (Cls_WorkListItem i in Bas.TaskList)
            {
                if (i.Status == Cls_WorkListItem.StatusCode.Arranging || i.Status == Cls_WorkListItem.StatusCode.Recording || i.Status == Cls_WorkListItem.StatusCode.Translating)
                {
                    MessageBox.Show("你有任务在进行，更新程序将在主程序下次启动时应用更新。");
                    return;
                }
            }
            File.WriteAllText(Application.StartupPath + "\\update.bat", "@echo off\r\n" +
                                    "choice /t 5 /d y /n >nul\r\n" +                                                                                   //等待5s开始
                                    "copy /y /b \"" + Application.StartupPath + "\\update.tmp" + "\" \"" + Application.ExecutablePath + "\"\r\n" +     //覆盖程序
                                    "del /q \"" + Application.StartupPath + "\\update.tmp" + "\"\r\n" +                                                //删除更新缓存
                                    "start \"\" \"" + Application.ExecutablePath + "\"\r\n" +                                                                     //启动程序
                                    "del %0", Encoding.Default);                                                                                                         //删除更新脚本自身
            Process p = new Process();
            p.StartInfo.FileName = Application.StartupPath + "\\update.bat";
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            p.StartInfo.Verb = "runas";//管理员启动
            p.Start();
            Environment.Exit(2);
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// 获得更新内容
        /// </summary>
        /// <param name="str">更新页文本</param>
        /// <returns>更新内容文本</returns>
        private static string GetChanges(string str)
        {
            str = Regex.Split(str, "<div class=\"markdown-body\">")[1];
            str = Regex.Split(str, "</div>")[0];
            str = Regex.Replace(str, "^\\n", "");
            str = Regex.Replace(str, "(?<before>\\s*<h2>)(?<content>.*?)(?<after></h2>)", "·$2：");

            foreach (Match i in Regex.Matches(str, "(?<before><ol>)(?<content>[\\s\\S]*?)(?<after></ol>)"))
            {
                string liststr = "";
                MatchCollection mc = Regex.Matches(i.Groups[2].Value, "(?<before><li>)(?<content>[\\s\\S]*?)(?<after></li>)");
                for (int j = 0; j < mc.Count; j++)
                {
                    liststr += (j + 1).ToString() + "." + mc[j].Groups[2].Value + "\r\n";
                }
                liststr = liststr.Substring(0, liststr.Length - 2);
                str = str.Replace(i.Value, liststr);
            }

            return str;
        }

        #endregion Private Methods
    }
}