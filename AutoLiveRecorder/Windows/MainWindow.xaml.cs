using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace AutoLiveRecorder
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Public Constructors

        /// <summary>
        /// 构造函数
        /// </summary>
        public MainWindow()
        {
            if (File.Exists("update.tmp"))
            {
                Update.DoUpdate();
            }

            InitializeComponent();

            //初始化托盘图标
            NotifyIcon.NotifyIcon_Init();
        }

        #endregion Public Constructors

        #region Private Methods

        /// <summary>
        /// 添加任务按钮鼠标进入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_AddTask_MouseEnter(object sender, MouseEventArgs e)
        {
            Btn_AddTask.Background = new SolidColorBrush(Color.FromArgb(60, 255, 255, 255));
        }

        /// <summary>
        /// 添加任务按钮鼠标离开
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_AddTask_MouseLeave(object sender, MouseEventArgs e)
        {
            Btn_AddTask.Background = null;
        }

        /// <summary>
        /// 添加任务按钮鼠标弹起
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_AddTask_MouseUp(object sender, MouseButtonEventArgs e)
        {
            TaskSettingWindow t = new TaskSettingWindow(WorkList);
            t.Show();
        }

        /// <summary>
        /// 全部开始按钮鼠标进入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_AllTaskStart_MouseEnter(object sender, MouseEventArgs e)
        {
            Btn_AllTaskStart.Background = new SolidColorBrush(Color.FromArgb(60, 255, 255, 255));
            lbl_Notice.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// 全部开始鼠标离开
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_AllTaskStart_MouseLeave(object sender, MouseEventArgs e)
        {
            Btn_AllTaskStart.Background = null;
            lbl_Notice.Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// 全部开始按钮弹起
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_AllTaskStart_MouseUp(object sender, MouseButtonEventArgs e)
        {
            WorkList.AllStart();
        }

        /// <summary>
        /// 全部停止鼠标进入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_AllTaskStop_MouseEnter(object sender, MouseEventArgs e)
        {
            Btn_AllTaskStop.Background = new SolidColorBrush(Color.FromArgb(60, 255, 255, 255));
        }

        /// <summary>
        /// 全部停止鼠标离开
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_AllTaskStop_MouseLeave(object sender, MouseEventArgs e)
        {
            Btn_AllTaskStop.Background = null;
        }

        /// <summary>
        /// 全部停止按钮鼠标弹起
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_AllTaskStop_MouseUp(object sender, MouseButtonEventArgs e)
        {
            WorkList.AllStop();
        }

        /// <summary>
        /// 检查更新按钮点击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_CheckUpdate_Click(object sender, RoutedEventArgs e)
        {
            Update.CheckUpdate();
        }

        /// <summary>
        /// 关闭按钮鼠标进入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_Close_MouseEnter(object sender, MouseEventArgs e)
        {
            Btn_Close.Background = new SolidColorBrush(Color.FromArgb(20, 255, 255, 255));
        }

        /// <summary>
        /// 关闭按钮鼠标离开
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_Close_MouseLeave(object sender, MouseEventArgs e)
        {
            Btn_Close.Background = null;
        }

        /// <summary>
        /// 关闭按钮鼠标弹起
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_Close_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (Properties.Settings.Default.IsBtnCloseNotNoticeAgain)
            {
                if (Properties.Settings.Default.IsBtnCloseToExit)
                {
                    Bas.Exit();
                }
                else
                {
                    Hide();
                }
            }
            else
            {
                CloseBtnModeChoserWindow c = new CloseBtnModeChoserWindow();
                c.ShowDialog();
            }
        }

        /// <summary>
        /// 退出按钮点击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_Exit_Click(object sender, RoutedEventArgs e)
        {
            Bas.Exit();
        }

        /// <summary>
        /// 菜单按钮鼠标进入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_Menu_MouseEnter(object sender, MouseEventArgs e)
        {
            Btn_Menu.Background = new SolidColorBrush(Color.FromArgb(20, 255, 255, 255));
        }

        /// <summary>
        /// 菜单按钮鼠标离开
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_Menu_MouseLeave(object sender, MouseEventArgs e)
        {
            Btn_Menu.Background = null;
        }

        /// <summary>
        /// 菜单按钮弹起
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_Menu_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Btn_Menu.ContextMenu.IsOpen = true;
        }

        /// <summary>
        /// 最小化按钮鼠标进入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_Minisize_MouseEnter(object sender, MouseEventArgs e)
        {
            Btn_Minisize.Background = new SolidColorBrush(Color.FromArgb(20, 255, 255, 255));
        }

        /// <summary>
        /// 最小化按钮鼠标离开
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_Minisize_MouseLeave(object sender, MouseEventArgs e)
        {
            Btn_Minisize.Background = null;
        }

        /// <summary>
        /// 最小化按钮鼠标弹起
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_Minisize_MouseUp(object sender, MouseButtonEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        /// <summary>
        /// 删除任务按钮鼠标进入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_RemoveTask_MouseEnter(object sender, MouseEventArgs e)
        {
            Btn_RemoveTask.Background = new SolidColorBrush(Color.FromArgb(60, 255, 255, 255));
        }

        /// <summary>
        /// 删除任务按钮鼠标离开
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_RemoveTask_MouseLeave(object sender, MouseEventArgs e)
        {
            Btn_RemoveTask.Background = null;
        }

        /// <summary>
        /// 删除任务按钮鼠标弹起
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_RemoveTask_MouseUp(object sender, MouseButtonEventArgs e)
        {
            WorkList.RemoveTask();
        }

        /// <summary>
        /// 关于按钮点击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_ShowAboutWindow_Click(object sender, RoutedEventArgs e)
        {
            AboutWindow a = new AboutWindow();
            a.Show();
        }

        /// <summary>
        /// 设置按钮点击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_ShowSettingWindow_Click(object sender, RoutedEventArgs e)
        {
            SettingWindow s = new SettingWindow();
            s.Show();
        }

        /// <summary>
        /// 标题标签鼠标移动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TitleGrid_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        #endregion Private Methods
    }
}