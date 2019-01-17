using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading;

namespace AutoLiveRecorder
{
    /// <summary>
    /// 工作项类
    /// </summary>
    public class Cls_WorkListItem : INotifyPropertyChanged
    {
        #region Private Fields

        private string _Frequency;
        private string _Host;
        private bool _IsRecordDanmu;
        private bool _IsRepeat;
        private bool _IsTranslateAfterCompleted;
        private PlatformType _Platform;
        private string _Roomid;
        private string _RoomTitle;
        private DateTime _StartTime;
        private StatusCode _Status;

        private string _URL;

        private WebClient client;

        /// <summary>
        /// 保存文件名
        /// </summary>
        private string FileName;

        /// <summary>
        /// 指示录制进程是否应该退出
        /// </summary>
        private bool IsRecorderAbortRequested = false;

        /// <summary>
        /// 计时器
        /// </summary>
        private Timer MyTimer;

        /// <summary>
        /// 录制进程实例
        /// </summary>
        private Thread Recorder;

        private List<string> tmpFileList = new List<string>();

        private FileStream writer;

        #endregion Private Fields

        #region Public Constructors

        /// <summary>
        /// 空白构造函数
        /// </summary>
        public Cls_WorkListItem()
        {
            MyTimer = new Timer(MyTimerCallBack);
        }

        #endregion Public Constructors

        #region Public Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion Public Events

        #region Public Enums

        /// <summary>
        /// 平台枚举
        /// </summary>
        public enum PlatformType
        {
            /// <summary>
            /// 默认
            /// </summary>
            None = 0,

            /// <summary>
            /// B站
            /// </summary>
            Bilibili = 1,

            /// <summary>
            /// 斗鱼
            /// </summary>
            Douyu = 2,

            /// <summary>
            /// 虎牙
            /// </summary>
            Huya = 3,

            /// <summary>
            /// 熊猫TV
            /// </summary>
            Panda = 4
        }

        /// <summary>
        /// 开始条件枚举
        /// </summary>
        public enum StartModeType
        {
            /// <summary>
            /// 立即开始
            /// </summary>
            Now = 0,

            /// <summary>
            /// 开播时开始
            /// </summary>
            WhenStart = 1,

            /// <summary>
            /// 定时开始
            /// </summary>
            WhenTime = 2,

            /// <summary>
            /// 手动开始
            /// </summary>
            Manuel = 3
        }

        /// <summary>
        /// 状态枚举
        /// </summary>
        public enum StatusCode
        {
            /// <summary>
            /// 待机中
            /// </summary>
            Waiting = 0,

            /// <summary>
            /// 正在录制
            /// </summary>
            Recording = 1,

            /// <summary>
            /// 整理中
            /// </summary>
            Arranging = 2,

            /// <summary>
            /// 转码中
            /// </summary>
            Translating = 3,

            /// <summary>
            /// 完成
            /// </summary>
            Finished = 4,

            /// <summary>
            /// 出错
            /// </summary>
            Error = -1
        }

        #endregion Public Enums

        #region Public Properties

        /// <summary>
        /// 任务频率和时间
        /// </summary>
        public string FNT
        {
            get
            {
                if (_Frequency != null && _StartTime != null)
                {
                    return _Frequency + "," + _StartTime.Hour + ":" + _StartTime.Minute + ":" + _StartTime.Second;
                }
                else
                {
                    return "";
                }
            }
        }

        /// <summary>
        /// 任务频率
        /// </summary>
        public string Frequency
        {
            get { return _Frequency; }
            set
            {
                _Frequency = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("FNT"));
            }
        }

        /// <summary>
        /// 主播
        /// </summary>
        public string Host
        {
            get
            {
                return _Host;
            }
            set
            {
                _Host = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("RoomInfo"));
            }
        }

        /// <summary>
        /// 是否录制弹幕
        /// </summary>
        public bool IsRecordDanmu
        {
            get { return _IsRecordDanmu; }
            set
            {
                _IsRecordDanmu = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsRecordDanmu"));
            }
        }

        public bool IsRepeat
        {
            get
            {
                return _IsRepeat;
            }
            set
            {
                _IsRepeat = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsRepeat"));
            }
        }

        /// <summary>
        /// 是否支持弹幕
        /// </summary>
        public bool IsSupportDanmu { get; set; }

        /// <summary>
        /// 是否录制完成后转码
        /// </summary>
        public bool IsTranslateAfterCompleted
        {
            get { return _IsTranslateAfterCompleted; }
            set
            {
                _IsTranslateAfterCompleted = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsTranslateAfterCompleted"));
            }
        }

        /// <summary>
        /// 平台
        /// </summary>
        public PlatformType Platform
        {
            get
            {
                return _Platform;
            }
            set
            {
                _Platform = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("RoomInfo"));
            }
        }

        /// <summary>
        /// 平台名字符串
        /// </summary>
        public string PlatformString
        {
            get
            {
                switch (Platform)
                {
                    case PlatformType.Bilibili:
                        return
                            "B站";

                    case PlatformType.Douyu:
                        return
                           "斗鱼";

                    case PlatformType.Huya:
                        return
                           "虎牙";

                    case PlatformType.Panda:
                        return
                            "熊猫TV";
                    default:
                        return "";
                }
            }
        }

        /// <summary>
        /// 房间号
        /// </summary>
        public string Roomid
        {
            get
            {
                return _Roomid;
            }
            set
            {
                _Roomid = value;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("RoomInfo"));
            }
        }

        /// <summary>
        /// 房间信息
        /// </summary>
        public string RoomInfo
        {
            get
            {
                switch (Platform)
                {
                    case PlatformType.Bilibili:
                        return Host + " · " + Roomid + " · B站";

                    case PlatformType.Douyu:
                        return Host + " · " + Roomid + " · 斗鱼";

                    case PlatformType.Huya:
                        return Host + " · " + Roomid + " · 虎牙";

                    case PlatformType.Panda:
                        return Host + " · " + Roomid + " · 熊猫TV";

                    default:
                        return Host + " · " + Roomid;
                }
            }
        }

        /// <summary>
        /// 房间信息（长）
        /// </summary>
        public string RoomInfoLong
        {
            get
            {
                if (!string.IsNullOrEmpty(URL))
                {
                    string livestatus;
                    switch (RoomStatus)
                    {
                        case 1:
                            livestatus = "在播";
                            break;

                        default:
                            livestatus = "离线";
                            break;
                    }

                    switch (Platform)
                    {
                        case PlatformType.Bilibili:
                            return
                                "平台：B站" + "\r\n" +
                                "房间号：" + Roomid + "\r\n" +
                                "标题：" + RoomTitle + "\r\n" +
                                "主播：" + Host + "\r\n" +
                                "直播状态：" + livestatus + "\r\n" +
                                "是否支持弹幕录制：" + IsSupportDanmu.ToString();

                        case PlatformType.Douyu:
                            return
                               "平台：斗鱼" + "\r\n" +
                               "房间号：" + Roomid + "\r\n" +
                               "标题：" + RoomTitle + "\r\n" +
                               "主播：" + Host + "\r\n" +
                               "直播状态：" + livestatus + "\r\n" +
                               "是否支持弹幕录制：" + IsSupportDanmu.ToString();

                        case PlatformType.Huya:
                            return
                               "平台：虎牙" + "\r\n" +
                               "房间号：" + Roomid + "\r\n" +
                               "标题：" + RoomTitle + "\r\n" +
                               "主播：" + Host + "\r\n" +
                               "直播状态：" + livestatus + "\r\n" +
                               "是否支持弹幕录制：" + IsSupportDanmu.ToString();

                        case PlatformType.Panda:
                            return
                                "平台：熊猫TV" + "\r\n" +
                                "房间号：" + Roomid + "\r\n" +
                                "标题：" + RoomTitle + "\r\n" +
                                "主播：" + Host + "\r\n" +
                                "直播状态：" + livestatus + "\r\n" +
                                "是否支持弹幕录制：" + IsSupportDanmu.ToString();

                        default:
                            return "直播间不存在或平台未支持。";
                    }
                }
                else
                {
                    return "";
                }
            }
        }

        /// <summary>
        /// 房间状态
        /// </summary>
        public int RoomStatus { get; set; }

        /// <summary>
        /// 房间标题
        /// </summary>
        public string RoomTitle
        {
            get { return _RoomTitle; }
            set
            {
                _RoomTitle = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("RoomTitle"));
            }
        }

        /// <summary>
        /// 开始条件
        /// </summary>
        public StartModeType StartMode { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartTime
        {
            get
            {
                if (_StartTime == new DateTime())
                {
                    return new DateTime(1970, 1, 1, 8, 0, 0);
                }
                else
                {
                    return _StartTime;
                }
            }
            set
            {
                _StartTime = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("FNT"));
            }
        }

        public StatusCode Status
        {
            get
            {
                return _Status;
            }
            set
            {
                _Status = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("StatusString"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("StatusColor"));
            }
        }

        /// <summary>
        /// 指示任务状态显示的颜色
        /// </summary>
        public System.Windows.Media.SolidColorBrush StatusColor
        {
            get
            {
                switch (Status)
                {
                    case StatusCode.Waiting:
                        return new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 0, 0, 0));

                    case StatusCode.Recording:
                    case StatusCode.Translating:
                    case StatusCode.Arranging:
                        return new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 255, 202, 0));

                    case StatusCode.Finished:
                        return new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 0, 255, 0));

                    case StatusCode.Error:
                        return new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 255, 0, 0));

                    default:
                        return new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 0, 0, 0));
                }
            }
        }

        /// <summary>
        /// 状态文本
        /// </summary>
        public string StatusString
        {
            get
            {
                switch (_Status)
                {
                    case StatusCode.Waiting:
                        return "待机中";

                    case StatusCode.Recording:
                        return "录制中";

                    case StatusCode.Finished:
                        return "已完成";

                    case StatusCode.Arranging:
                        return "整理中";

                    case StatusCode.Translating:
                        return "转码中";

                    case StatusCode.Error:
                        return "出错了";

                    default:
                        return "未知状态";
                }
            }
        }

        /// <summary>
        /// 直播地址
        /// </summary>
        public string URL
        {
            get { return _URL; }
            set
            {
                _URL = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("URL"));
            }
        }

        /// <summary>
        /// 视频地址
        /// </summary>
        public string[] VideoUrl { get; set; }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// 调起属性更改事件通知
        /// </summary>
        /// <param name="PropertyName">属性名</param>
        public void CallPropertyChanged(string PropertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }

        /// <summary>
        /// 设置完毕调用
        /// </summary>
        public void SettingFinished()
        {
            if (Status == StatusCode.Waiting)
            {
                switch (StartMode)
                {
                    case StartModeType.Now:
                        StartRecord();
                        break;

                    case StartModeType.WhenStart:
                    case StartModeType.WhenTime:
                        MyTimer.Change(0, 1000);
                        break;

                    case StartModeType.Manuel:
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// 开始录制
        /// </summary>
        public void StartRecord(bool IsManuelStart = false)
        {
            if (StartMode != StartModeType.WhenStart)
            {
                switch (Platform)
                {
                    case PlatformType.None:
                        break;

                    case PlatformType.Bilibili:
                        if (B_Live.IsLiving(URL))
                            VideoUrl = B_Live.GetVideoURL(URL);
                        else
                        {
                            StartMode = StartModeType.WhenStart;
                            MyTimer.Change(1000, 1000);
                            if (IsManuelStart) System.Windows.Forms.MessageBox.Show("直播未开始，任务已切换至“直播开始时开始录制”模式。");
                            return;
                        }
                        break;

                    case PlatformType.Douyu:
                        break;

                    case PlatformType.Huya:
                        break;

                    case PlatformType.Panda:
                        break;

                    default:
                        break;
                }
            }

            Status = StatusCode.Recording;

            Recorder = new Thread(DoRecord);
            IsRecorderAbortRequested = false;
            Recorder.Start();
        }

        /// <summary>
        /// 停止录制
        /// </summary>
        public void StopRecord()
        {
            if (Status == StatusCode.Recording)
            {
                IsRecorderAbortRequested = true;
            }
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// 文件整理
        /// </summary>
        /// <param name="FileName">文件路径</param>
        private void ArrangeFile()
        {
            FlvMerge.Merge(tmpFileList, FileName);
            foreach (string i in tmpFileList)
            {
                if (File.Exists(i))
                {
                    File.Delete(i);
                }
            }
            tmpFileList.Clear();
            if (IsTranslateAfterCompleted)
            {
                Status = StatusCode.Translating;
                Transcode();
            }
            if ((StartMode == StartModeType.WhenStart && IsRepeat) || (StartMode == StartModeType.WhenTime && !Frequency.Contains("仅一次")))
                Status = StatusCode.Waiting;
            else
                Status = StatusCode.Finished;
        }

        /// <summary>
        /// 执行录制
        /// </summary>
        private void DoRecord()
        {
            bool isFinished = false;
            try
            {
                client = new WebClient();

                Stream stream = client.OpenRead(new Uri(VideoUrl[0]));

                if (!Directory.Exists(Properties.Settings.Default.SavePath)) Directory.CreateDirectory(Properties.Settings.Default.SavePath);
                FileName = Bas.GetFreeFileName(PlatformString + "-" + Roomid + "-" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + " " + DateTime.Now.Hour + "-" + DateTime.Now.Minute + "-" + DateTime.Now.Second, "flv", Properties.Settings.Default.SavePath);
                string tmpFileName = Bas.GetFreeTmpFileName(FileName);
                tmpFileList.Add(tmpFileName);
                writer = new FileStream(tmpFileName, FileMode.Create);

                long tmpFileLength = 0;
                byte[] mbyte = new byte[1024];
                int readL = stream.Read(mbyte, 0, 1024);
                while (readL != 0 && !IsRecorderAbortRequested)
                {
                    writer.Write(mbyte, 0, readL);//写文件
                    tmpFileLength += readL;
                    //文件分片
                    if (tmpFileLength >= 1073741824)
                    {
                        //关闭流
                        stream = null;
                        //关闭连接
                        client.Dispose();
                        //关闭文件
                        writer.Close();
                        //新建分片
                        DoRecord(FileName);
                        isFinished = true;
                        return;
                    }
                    readL = stream.Read(mbyte, 0, 1024);//读流
                }
                if (IsLiving() && !IsRecorderAbortRequested) DoRecord(FileName);
                else
                {
                    Status = StatusCode.Arranging;
                }
            }
            catch
            {
                if (IsLiving() && !IsRecorderAbortRequested) DoRecord(FileName);
                else
                {
                    Status = StatusCode.Arranging;
                }
            }
            finally
            {
                if (!isFinished)
                {
                    //关闭连接
                    client.Dispose();
                    //关闭文件
                    writer.Close();
                    //整理文件
                    ArrangeFile();
                }
            }
        }

        /// <summary>
        /// 执行录制
        /// </summary>
        /// <param name="FileName">文件路径</param>
        private void DoRecord(string FileName)
        {
            bool isFinished = false;
            try
            {
                client = new WebClient();

                Stream stream = client.OpenRead(new Uri(VideoUrl[0]));

                string tmpFileName = Bas.GetFreeTmpFileName(FileName);
                tmpFileList.Add(tmpFileName);
                writer = new FileStream(tmpFileName, FileMode.Create);

                long tmpFileLength = 0;
                byte[] mbyte = new byte[1024];
                int readL = stream.Read(mbyte, 0, 1024);
                while (readL != 0 && !IsRecorderAbortRequested)
                {
                    writer.Write(mbyte, 0, readL);//写文件
                    tmpFileLength += readL;
                    //文件分片
                    if (tmpFileLength >= 1073741824)
                    {
                        //关闭连接
                        client.Dispose();
                        //关闭文件
                        writer.Close();
                        //新建分片
                        DoRecord(FileName);
                        isFinished = true;
                        return;
                    }
                    readL = stream.Read(mbyte, 0, 1024);//读流
                }
                if (IsLiving() && !IsRecorderAbortRequested) DoRecord(FileName);
                else
                {
                    Status = StatusCode.Arranging;
                }
            }
            catch
            {
                if (IsLiving() && !IsRecorderAbortRequested) DoRecord(FileName);
                else
                {
                    Status = StatusCode.Arranging;
                }
            }
            finally
            {
                if (!isFinished)
                {
                    //关闭连接
                    client.Dispose();
                    //关闭文件
                    writer.Close();
                    //整理文件
                    ArrangeFile();
                }
            }
        }

        /// <summary>
        /// 获取当前日期星期
        /// </summary>
        /// <returns></returns>
        private string GetTodayDay()
        {
            switch (DateTime.Now.DayOfWeek)
            {
                case DayOfWeek.Sunday:
                    return "日";

                case DayOfWeek.Monday:
                    return "一";

                case DayOfWeek.Tuesday:
                    return "二";

                case DayOfWeek.Wednesday:
                    return "三";

                case DayOfWeek.Thursday:
                    return "四";

                case DayOfWeek.Friday:
                    return "五";

                case DayOfWeek.Saturday:
                    return "六";

                default:
                    return "";
            }
        }

        /// <summary>
        /// 是否在播查询
        /// </summary>
        /// <returns>是否在播</returns>
        private bool IsLiving()
        {
            switch (Platform)
            {
                case PlatformType.None:
                    return false;

                case PlatformType.Bilibili:
                    return B_Live.IsLiving(URL);

                case PlatformType.Douyu:
                    return false;

                case PlatformType.Huya:
                    return false;

                case PlatformType.Panda:
                    return false;

                default:
                    return false;
            }
        }

        private void MyTimerCallBack(object state)
        {
            if (Status == StatusCode.Waiting)
            {
                switch (StartMode)
                {
                    case StartModeType.WhenStart:
                        switch (Platform)
                        {
                            case PlatformType.None:
                                break;

                            case PlatformType.Bilibili:
                                if (B_Live.IsLiving(URL))
                                {
                                    VideoUrl = B_Live.GetVideoURL(URL);
                                    StartRecord();
                                }
                                break;

                            case PlatformType.Douyu:
                                break;

                            case PlatformType.Huya:
                                break;

                            case PlatformType.Panda:
                                break;

                            default:
                                break;
                        }
                        break;

                    case StartModeType.WhenTime:
                        string DayOfWeekString = GetTodayDay();

                        if (Frequency.Contains("每天") || Frequency.Contains(DayOfWeekString) || (Frequency.Contains("仅一次") && Status == StatusCode.Waiting))
                        {
                            if (StartTime.Hour == DateTime.Now.Hour && StartTime.Minute == DateTime.Now.Minute && StartTime.Second == DateTime.Now.Second)
                            {
                                StartRecord();
                            }
                        }

                        break;

                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// 转码
        /// </summary>
        private void Transcode()
        {
        a: if (File.Exists(Properties.Settings.Default.SavePath + "\\ffmpeg.exe"))
            {
                Process p = new Process()
                {
                    StartInfo = new ProcessStartInfo()
                    {
                        FileName = Properties.Settings.Default.SavePath + "\\ffmpeg.exe",
                        CreateNoWindow = true,
                        WindowStyle = ProcessWindowStyle.Hidden,
                        Arguments = "-i \"" + FileName + "\" -vcodec copy -acodec copy \"" + Bas.GetMP4FileName(FileName) + "\""
                    }
                };
                p.Start();
            }
            else
            {
                WebClient c = new WebClient();
                c.DownloadFile("http://update.zhangbudademao.com/112/ffmpeg.exe", Properties.Settings.Default.SavePath + "\\ffmpeg.exe");
                goto a;
            }
        }

        #endregion Private Methods
    }
}