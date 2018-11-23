using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace AutoLiveRecorder
{
    /// <summary>
    /// CloseBtnModeChoserWindow.xaml 的交互逻辑
    /// </summary>
    public partial class CloseBtnModeChoserWindow : Window
    {
        #region Public 构造函数

        public CloseBtnModeChoserWindow()
        {
            InitializeComponent();
        }

        #endregion Public 构造函数

        #region Private 方法

        private void Btn_Cancel_MouseEnter(object sender, MouseEventArgs e)
        {
            Btn_Cancel.Background = new SolidColorBrush(Color.FromArgb(153, 255, 255, 255));
        }

        private void Btn_Cancel_MouseLeave(object sender, MouseEventArgs e)
        {
            Btn_Cancel.Background = new SolidColorBrush(Color.FromArgb(102, 255, 255, 255));
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
            Btn_Comfirm.Background = new SolidColorBrush(Color.FromArgb(153, 255, 255, 255));
        }

        private void Btn_Comfirm_MouseLeave(object sender, MouseEventArgs e)
        {
            Btn_Comfirm.Background = new SolidColorBrush(Color.FromArgb(102, 255, 255, 255));
        }

        private void Btn_Comfirm_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Properties.Settings.Default.IsBtnCloseToExit = (bool)RB_Exit.IsChecked;
            Properties.Settings.Default.IsBtnCloseNotNoticeAgain = (bool)CB_NotAskAgain.IsChecked;

            if ((bool)RB_Exit.IsChecked)
            {
                Bas.Exit();
            }
            else
            {
                Application.Current.MainWindow.Hide();
            }

            Close();
        }

        private void TitleGrid_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (Properties.Settings.Default.IsBtnCloseNotNoticeAgain) CB_NotAskAgain.IsChecked = true; else CB_NotAskAgain.IsChecked = false;
            if (Properties.Settings.Default.IsBtnCloseToExit) RB_Exit.IsChecked = true; else RB_Minisize.IsChecked = true;
        }

        #endregion Private 方法
    }
}