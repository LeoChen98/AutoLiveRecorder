using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;

namespace AutoLiveRecorder
{
    /// <summary>
    /// B站直播录制类
    /// </summary>
    internal class B_Live
    {
        #region Private 字段

        private bool _IsRecording = false;

        /// <summary>
        /// 录制文件列表
        /// </summary>
        private List<string> filelist;

        /// <summary>
        /// 房间信息实例
        /// </summary>
        private RoomInfo Info;

        /// <summary>
        /// 监视进程实例
        /// </summary>
        private System.Threading.Thread Monitor;

        /// <summary>
        /// 录制进程实例
        /// </summary>
        private System.Threading.Thread Recorder;

        /// <summary>
        /// 视频流实例
        /// </summary>
        private Stream stream;

        /// <summary>
        /// 写文件实例
        /// </summary>
        private FileStream writer;

        #endregion Private 字段

        #region Public 构造函数

        /// <summary>
        /// 构造函数，新建一个B站录制实例
        /// </summary>
        /// <param name="id">房间id（长短皆可）</param>
        public B_Live(int id)
        {
            Info = new RoomInfo();
            HttpWebRequest Req = (HttpWebRequest)WebRequest.Create("https://api.live.bilibili.com/room/v1/Room/room_init?id=" + id);
            HttpWebResponse Rep = (HttpWebResponse)Req.GetResponse();
            StreamReader Reader = new StreamReader(Rep.GetResponseStream());
            string tmp = Reader.ReadToEnd();
            if (Regex.Split(Regex.Split(tmp, "\"live_status\":")[1], ",")[0] != "1")
            {
                Info.IsLive = false;
            }
            else
            {
                Info.IsLive = true;
            }
            Info.RoomId = int.Parse(Regex.Split(Regex.Split(tmp, "\"room_id\":")[1], ",")[0]);
            filelist = new List<string>();
        }

        #endregion Public 构造函数

        #region Public 委托

        /// <summary>
        /// 录制状态改变事件委托
        /// </summary>
        /// <param name="RecordStatus">录制状态</param>
        public delegate void RecordStatusChangedEvent(bool RecordStatus);

        #endregion Public 委托

        #region Public 事件

        /// <summary>
        /// 录制状态改变事件
        /// </summary>
        public event RecordStatusChangedEvent RecordStatusChanged;

        #endregion Public 事件

        #region Public 属性

        /// <summary>
        /// 指示是否正在录制
        /// </summary>
        public bool IsRecording
        {
            get
            {
                return _IsRecording;
            }
            set
            {
                _IsRecording = value;
                RecordStatusChanged(value);
            }
        }

        #endregion Public 属性

        #region Public 方法

        /// <summary>
        /// 取消录制预位
        /// </summary>
        public void CancelRecordReady()
        {
            Monitor.Abort();
        }

        /// <summary>
        /// 录制预位
        /// </summary>
        public void ReadyToRecord(string savepath)
        {
            Monitor = new System.Threading.Thread(() =>
            {
                while (true)
                {
                    if (IsLive()&& !IsRecording) StartRecord(savepath);
                    System.Threading.Thread.Sleep(1000);
                }
            }
            );
            Monitor.Start();
        }

        /// <summary>
        /// 开始录制
        /// </summary>
        /// <param name="savepath">录制文件路径</param>
        public void StartRecord(string savepath)
        {
            if (IsLive())
            {
                HttpWebRequest Req = (HttpWebRequest)WebRequest.Create("https://api.live.bilibili.com/room/v1/Room/playUrl?quality=0&platform=web&cid=" + Info.RoomId);
                HttpWebResponse Rep = (HttpWebResponse)Req.GetResponse();
                StreamReader Reader = new StreamReader(Rep.GetResponseStream());
                string tmp = Reader.ReadToEnd();
                Regex reg = new Regex("\"https://(.*?)\"");
                Regex reg2 = new Regex("wsTime=\\d*\"$");
                Info.urls = new List<string>();
                foreach (Match i in reg.Matches(tmp))
                {
                    string str = i.Value;
                    if (reg2.Match(str).Success)
                    {
                        Info.urls.Add(str.Replace("\"", ""));
                    }
                }
                Recorder = new System.Threading.Thread(() =>
                {
                    try
                    {
                        WebClient client = new WebClient();
                        stream = client.OpenRead(new Uri(Info.urls[0]));
                        //获取文件名
                        int ii = 1;
                        string savepathx = savepath;
                        string savepathn = savepath.Substring(0, savepath.LastIndexOf("."));
                        savepathx = savepathn + "-" + ii + ".flv";
                        while (File.Exists(savepathx))
                        {
                            ii++;
                            savepathx = savepathn + "-" + ii + ".flv";
                        }
                        filelist.Add(savepathx);
                        writer = new FileStream(savepathx, FileMode.Create);
                        byte[] mbyte = new byte[1024];
                        int readL = stream.Read(mbyte, 0, 1024);
                        while (readL != 0)
                        {
                            writer.Write(mbyte, 0, readL);//写文件
                            readL = stream.Read(mbyte, 0, 1024);//读流
                        }
                        if (IsLive()) StartRecord(savepath); else IsRecording = false;// ArrangeFile(savepath);
                    }
                    catch (Exception ex)
                    {
                        if (ex.InnerException.Message != "正在中止线程。")
                            if (IsLive()) StartRecord(savepath); else IsRecording = false;// ArrangeFile(savepath);
                    }
                    finally
                    {
                        //关闭流
                        stream.Close();
                        //关闭文件
                        writer.Close();
                        //整理文件
                        ArrangeFile(savepath);
                        //合并文件
                        //FLVMerge.StartMarge(FLVMerge.GetPathList(savepath), savepath);
                    }
                });
                Recorder.Start();
                IsRecording = true;
            }
            else
            {
                IsRecording = false;
                ArrangeFile(savepath);
            }
        }

        /// <summary>
        /// 停止录制
        /// </summary>
        public void StopRecord()
        {
            Recorder.Abort();
            if (Monitor != null && Monitor.IsAlive == true)
            {
                Monitor.Abort();
            }
            IsRecording = false;
        }

        #endregion Public 方法

        #region Private 方法

        /// <summary>
        /// 文件整理
        /// </summary>
        /// <param name="savepathn">文件名称（不含扩展名）</param>
        private void ArrangeFile(string savepath)
        {
            string savepathn = savepath.Substring(0, savepath.LastIndexOf("."));
            if (filelist.Count == 1)
            {
                File.Move(filelist[0], savepath);
                Process.Start("explorer.exe","/select," + savepath);
            }
            else
            {
                Directory.CreateDirectory(savepathn);
                foreach (string i in filelist)
                {
                    string[] tmp = Regex.Split(i, "\\");
                    string filename = tmp[tmp.Length - 1];
                    File.Move(i, savepath + "\\" + filename);
                }
                Process.Start("explorer.exe",savepathn);
            }
        }

        /// <summary>
        /// 判断是否正在直播
        /// </summary>
        /// <returns>是否在直播</returns>
        private bool IsLive()
        {
            HttpWebRequest Req = (HttpWebRequest)WebRequest.Create("https://api.live.bilibili.com/room/v1/Room/room_init?id=" + Info.RoomId);
            HttpWebResponse Rep = (HttpWebResponse)Req.GetResponse();
            StreamReader Reader = new StreamReader(Rep.GetResponseStream());
            string tmp = Reader.ReadToEnd();
            if (Regex.Split(Regex.Split(tmp, "\"live_status\":")[1], ",")[0] != "1")
            {
                Info.IsLive = false;
                return false;
            }
            else
            {
                Info.IsLive = true;
                return true;
            }
        }

        #endregion Private 方法

        #region Private 类

        /// <summary>
        /// 房间信息
        /// </summary>
        private class RoomInfo
        {
            #region Public 字段

            /// <summary>
            /// 是否在播
            /// </summary>
            public bool IsLive;

            /// <summary>
            /// 房间真实id
            /// </summary>
            public int RoomId;

            /// <summary>
            /// 流地址
            /// </summary>
            public List<string> urls;

            #endregion Public 字段
        }

        #endregion Private 类
    }
}

// https://api.live.bilibili.com/room/v1/Room/room_init?id={id}  ->{cid}
// https://api.live.bilibili.com/room/v1/Room/playUrl?cid={cid}&quality=0&platform=web ->durl x 4
// filedownload:durl