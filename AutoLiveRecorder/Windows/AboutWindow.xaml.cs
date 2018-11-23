using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace AutoLiveRecorder
{
    /// <summary>
    /// AboutWindow.xaml 的交互逻辑
    /// </summary>
    public partial class AboutWindow : Window
    {
        #region Public 构造函数

        public AboutWindow()
        {
            InitializeComponent();
        }

        #endregion Public 构造函数

        #region Private 方法

        /// <summary>
        /// 检查更新按钮鼠标弹起
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_CheckUpdate_MouseUp(object sender, MouseButtonEventArgs e)
        {
        }

        /// <summary>
        /// 关闭按钮鼠标进入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_Close_MouseEnter(object sender, MouseEventArgs e)
        {
            Btn_Close.Background = new SolidColorBrush(Color.FromArgb(20, 255, 255, 255)); ;
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
            Close();
        }

        /// <summary>
        /// 官网URL鼠标弹起
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_CopyRightURLOpen_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Process.Start("https://www.zhangbudademao.com");
        }

        /// <summary>
        /// 开源地址URL鼠标弹起
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lbl_OpenSourceURLValue_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Process.Start("https://github.com/LeoChen98/AutoLiveRecorder");
        }

        /// <summary>
        /// 标题框鼠标移动
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

        #endregion Private 方法
    }
}