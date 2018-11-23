using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace AutoLiveRecorder
{
    /// <summary>
    /// TaskSettingWindow.xaml 的交互逻辑
    /// </summary>
    public partial class TaskSettingWindow : Window
    {
        #region Public Constructors

        public TaskSettingWindow(bool IsNew = false)
        {
            InitializeComponent();
        }

        #endregion Public Constructors

        #region Private Methods

        private void Btn_Cancel_MouseEnter(object sender, MouseEventArgs e)
        {
            Btn_Cancel.Background = new SolidColorBrush(Color.FromArgb(60, 255, 255, 255));
        }

        private void Btn_Cancel_MouseLeave(object sender, MouseEventArgs e)
        {
            Btn_Cancel.Background = new SolidColorBrush(Color.FromArgb(40, 255, 255, 255));
        }

        private void Btn_Cancel_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Close();
        }

        private void Btn_Close_MouseEnter(object sender, MouseEventArgs e)
        {
            Btn_Close.Background = new SolidColorBrush(Color.FromArgb(20, 255, 255, 255));
        }

        private void Btn_Close_MouseLeave(object sender, MouseEventArgs e)
        {
            Btn_Close.Background = null;
        }

        private void Btn_Close_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Close();
        }

        private void Btn_Comfirm_MouseEnter(object sender, MouseEventArgs e)
        {
            Btn_Comfirm.Background = new SolidColorBrush(Color.FromArgb(60, 255, 255, 255));
        }

        private void Btn_Comfirm_MouseLeave(object sender, MouseEventArgs e)
        {
            Btn_Comfirm.Background = new SolidColorBrush(Color.FromArgb(40, 255, 255, 255));
        }

        private void Btn_Comfirm_MouseUp(object sender, MouseButtonEventArgs e)
        {
        }

        private void Btn_Minisize_MouseEnter(object sender, MouseEventArgs e)
        {
            Btn_Minisize.Background = new SolidColorBrush(Color.FromArgb(20, 255, 255, 255));
        }

        private void Btn_Minisize_MouseLeave(object sender, MouseEventArgs e)
        {
            Btn_Minisize.Background = null;
        }

        private void Btn_Minisize_MouseUp(object sender, MouseButtonEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void Btn_ShowTimeSetterWindow_MouseEnter(object sender, MouseEventArgs e)
        {
            Btn_ShowTimeSetterWindow.Background = new SolidColorBrush(Color.FromArgb(80, 255, 255, 255));
        }

        private void Btn_ShowTimeSetterWindow_MouseLeave(object sender, MouseEventArgs e)
        {
            Btn_ShowTimeSetterWindow.Background = new SolidColorBrush(Color.FromArgb(60, 255, 255, 255));
        }

        private void Btn_ShowTimeSetterWindow_MouseUp(object sender, MouseButtonEventArgs e)
        {
            TimeSetterWindow t = new TimeSetterWindow();
            t.ShowDialog();
        }

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