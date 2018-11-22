using System;
using System.Windows;

namespace AutoLiveRecorder
{
    internal class NotifyIcon
    {
        #region Private Fields

        /// <summary>
        /// 系统托盘图标实例
        /// </summary>
        private static System.Windows.Forms.NotifyIcon NI;

        #endregion Private Fields

        #region Public Methods

        /// <summary>
        /// 系统托盘图标初始化程序
        /// </summary>
        public static void NotifyIcon_Init()
        {
            NI = new System.Windows.Forms.NotifyIcon();

            NI.Icon = Properties.Resources.favicon;
            NI.Text = "录播姬";

            //打开菜单项
            System.Windows.Forms.MenuItem open = new System.Windows.Forms.MenuItem("显示主窗口");
            open.Click += new EventHandler((s, e) => { Application.Current.MainWindow.Show(); Application.Current.MainWindow.Activate(); });

            //设置菜单项
            System.Windows.Forms.MenuItem setting = new System.Windows.Forms.MenuItem("设置");
            setting.Click += new EventHandler((s, e) => { SettingWindow ss = new SettingWindow(); ss.Show(); });

            //检查更新菜单项
            System.Windows.Forms.MenuItem checkupdate = new System.Windows.Forms.MenuItem("检查更新");
            checkupdate.Click += new EventHandler((s, e) => { });

            //关于菜单项
            System.Windows.Forms.MenuItem about = new System.Windows.Forms.MenuItem("关于");
            about.Click += new EventHandler((s, e) => { AboutWindow a = new AboutWindow(); a.ShowDialog(); });

            //退出菜单项
            System.Windows.Forms.MenuItem exit = new System.Windows.Forms.MenuItem("退出");
            exit.Click += new EventHandler((s, e) => { Bas.Exit(); });

            //关联托盘控件
            System.Windows.Forms.MenuItem[] childen = new System.Windows.Forms.MenuItem[] { open, setting, checkupdate, about, exit };
            NI.ContextMenu = new System.Windows.Forms.ContextMenu(childen);

            NI.Click += new EventHandler((s, e) => { Application.Current.MainWindow.Show(); Application.Current.MainWindow.Activate(); });
            NI.BalloonTipClicked += new EventHandler((s, e) => { Application.Current.MainWindow.Show(); Application.Current.MainWindow.Activate(); });

            NI.Visible = true;
        }

        /// <summary>
        /// 显示通知
        /// </summary>
        /// <param name="text">通知正文</param>
        /// <param name="title">通知标题</param>
        /// <param name="Icon">通知图标</param>
        /// <param name="time">显示时间</param>
        public static void ShowTip(string text, string title = "录播姬", System.Windows.Forms.ToolTipIcon Icon = System.Windows.Forms.ToolTipIcon.Info, int time = 5000)
        {
            if (Properties.Settings.Default.IsNoticeShow)
            {
                NI.BalloonTipText = text;
                NI.BalloonTipTitle = title;
                NI.BalloonTipIcon = Icon;
                NI.ShowBalloonTip(time);
            }
        }

        /// <summary>
        /// 显示通知
        /// </summary>
        /// <param name="text">通知正文</param>
        /// <param name="type">通知类型 0=一般通知 1=状态通知 -1=错误通知</param>
        public static void ShowTip(string text, int type = 0)
        {
            if (Properties.Settings.Default.IsNoticeShow)
            {
                switch (type)
                {
                    case 0:
                        NI.BalloonTipText = text;
                        NI.BalloonTipTitle = "录播姬";
                        NI.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
                        NI.ShowBalloonTip(5000);
                        break;

                    case 1:
                        if (Properties.Settings.Default.IsStatusNoticeShow)
                        {
                            NI.BalloonTipText = text;
                            NI.BalloonTipTitle = "录播姬";
                            NI.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
                            NI.ShowBalloonTip(5000);
                        }
                        break;

                    case -1:
                        if (Properties.Settings.Default.IsErrNoticeShow)
                        {
                            NI.BalloonTipText = text;
                            NI.BalloonTipTitle = "录播姬";
                            NI.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Error;
                            NI.ShowBalloonTip(5000);
                        }
                        break;
                }
            }
        }

        #endregion Public Methods
    }
}