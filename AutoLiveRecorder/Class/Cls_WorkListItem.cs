using System;
using System.ComponentModel;

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
        private bool _IsTranslateAfterCompleted;
        private PlatformType _Platform;
        private string _Roomid;
        private string _RoomTitle;
        private DateTime _StartTime;
        private string _Status;

        #endregion Private Fields

        #region Public Constructors

        /// <summary>
        /// 空白构造函数
        /// </summary>
        public Cls_WorkListItem()
        {
        }

        /// <summary>
        /// 通过直播间地址构建
        /// </summary>
        /// <param name="URL">直播间地址</param>
        public Cls_WorkListItem(string URL)
        {
        }

        /// <summary>
        /// 通过房间号构建
        /// </summary>
        /// <param name="Roomid">房间号</param>
        /// <param name="platform">平台</param>
        public Cls_WorkListItem(string Roomid, PlatformType platform)
        {
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
            /// B站
            /// </summary>
            Bilibili = 0,

            /// <summary>
            /// 斗鱼
            /// </summary>
            Douyu = 1,

            /// <summary>
            /// 虎牙
            /// </summary>
            Huya = 2,

            /// <summary>
            /// 熊猫TV
            /// </summary>
            Panda = 3
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
            WhenTime = 2
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
        /// 是否在播
        /// </summary>
        public bool IsLiving { get; set; }

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
                        return Host + "·" + Roomid + "·B站";

                    case PlatformType.Douyu:
                        return Host + "·" + Roomid + "·斗鱼";

                    case PlatformType.Huya:
                        return Host + "·" + Roomid + "·虎牙";

                    case PlatformType.Panda:
                        return Host + "·" + Roomid + "·熊猫TV";

                    default:
                        return Host + "·" + Roomid;
                }
            }
        }

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
            get { return _StartTime; }
            set
            {
                _StartTime = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("FNT"));
            }
        }

        /// <summary>
        /// 状态
        /// </summary>
        public string Status
        {
            get
            {
                return _Status;
            }
            set
            {
                _Status = value;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Status"));
            }
        }

        /// <summary>
        /// 直播地址
        /// </summary>
        public string URL { get; set; }

        /// <summary>
        /// 视频地址
        /// </summary>
        public string[] VideoUrl { get; set; }

        #endregion Public Properties
    }
}