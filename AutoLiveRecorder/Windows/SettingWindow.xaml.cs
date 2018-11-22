using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace AutoLiveRecorder
{
    /// <summary>
    /// SettingWindow.xaml 的交互逻辑
    /// </summary>
    public partial class SettingWindow : Window
    {
        #region Public Constructors

        public SettingWindow()
        {
            InitializeComponent();
        }

        #endregion Public Constructors

        #region Private Methods

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

        private void Btn_ShowSavePath_MouseUp(object sender, MouseButtonEventArgs e)
        {
            using (System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                dialog.ShowNewFolderButton = true;
                dialog.Description = "请选择一个目录作为录制文件的保存目录。";
                dialog.SelectedPath = Properties.Settings.Default.SavePath;
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    lbl_SavePathKey.Content = "保存路径：" + dialog.SelectedPath;
                    Properties.Settings.Default.SavePath = dialog.SelectedPath;
                    Properties.Settings.Default.Save();
                }
            }
            Activate();
        }

        private void CB_IsErrorNotice_Click(object sender, RoutedEventArgs e)
        {
            if ((bool)CB_IsErrorNotice.IsChecked)
            {
                Properties.Settings.Default.IsErrNoticeShow = true;
            }
            else
            {
                Properties.Settings.Default.IsErrNoticeShow = false;
            }
            Properties.Settings.Default.Save();
        }

        private void CB_IsNoticeShow_Click(object sender, RoutedEventArgs e)
        {
            if ((bool)CB_IsNoticeShow.IsChecked)
            {
                Properties.Settings.Default.IsNoticeShow = true;
                Properties.Settings.Default.Save();
                NoticeGrid.Visibility = Visibility.Visible;
            }
            else
            {
                Properties.Settings.Default.IsNoticeShow = false;
                Properties.Settings.Default.Save();
                NoticeGrid.Visibility = Visibility.Hidden;
            }
        }

        private void CB_IsShowCloseBtnMode_Click(object sender, RoutedEventArgs e)
        {
            if ((bool)CB_IsShowCloseBtnMode.IsChecked)
            {
                Properties.Settings.Default.IsBtnCloseNotNoticeAgain = true;
                CloseBtnMode.Visibility = Visibility.Visible;
            }
            else
            {
                Properties.Settings.Default.IsBtnCloseNotNoticeAgain = false;
                CloseBtnMode.Visibility = Visibility.Hidden;
            }
            Properties.Settings.Default.Save();
        }

        private void CB_IsStatusNotice_Click(object sender, RoutedEventArgs e)
        {
            if ((bool)CB_IsStatusNotice.IsChecked)
            {
                Properties.Settings.Default.IsStatusNoticeShow = true;
            }
            else
            {
                Properties.Settings.Default.IsStatusNoticeShow = false;
            }
            Properties.Settings.Default.Save();
        }

        private void RB_Exit_Click(object sender, RoutedEventArgs e)
        {
            RB_HideToTray.IsChecked = false;
            Properties.Settings.Default.IsBtnCloseToExit = true;
            Properties.Settings.Default.Save();
        }

        private void RB_HideToTray_Click(object sender, RoutedEventArgs e)
        {
            RB_Exit.IsChecked = false;
            Properties.Settings.Default.IsBtnCloseToExit = false;
            Properties.Settings.Default.Save();
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
            lbl_SavePathKey.Content = "保存路径：" + Properties.Settings.Default.SavePath;

            if (Properties.Settings.Default.IsNoticeShow)
            {
                CB_IsNoticeShow.IsChecked = true;
                NoticeGrid.Visibility = Visibility.Visible;
            }
            else
            {
                CB_IsNoticeShow.IsChecked = false;
                NoticeGrid.Visibility = Visibility.Hidden;
            }

            CB_IsStatusNotice.IsChecked = Properties.Settings.Default.IsStatusNoticeShow;
            CB_IsErrorNotice.IsChecked = Properties.Settings.Default.IsErrNoticeShow;

            if (Properties.Settings.Default.IsBtnCloseNotNoticeAgain)
            {
                CB_IsShowCloseBtnMode.IsChecked = true;
                CloseBtnMode.Visibility = Visibility.Visible;
            }
            else
            {
                CB_IsShowCloseBtnMode.IsChecked = false;
                CloseBtnMode.Visibility = Visibility.Hidden;
            }

            if (Properties.Settings.Default.IsBtnCloseToExit)
            {
                RB_Exit.IsChecked = true;
                RB_HideToTray.IsChecked = false;
            }
            else
            {
                RB_Exit.IsChecked = false;
                RB_HideToTray.IsChecked = true;
            }
        }

        #endregion Private Methods
    }
}