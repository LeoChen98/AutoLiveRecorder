using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace AutoLiveRecorder
{
    /// <summary>
    /// WorkListItem.xaml 的交互逻辑
    /// </summary>
    public partial class WorkListItem : UserControl
    {
        #region Public Constructors

        public WorkListItem()
        {
            InitializeComponent();
        }

        #endregion Public Constructors

        #region Private Methods

        private void Btn_Setting_MouseEnter(object sender, MouseEventArgs e)
        {
            Btn_Setting.Background = new SolidColorBrush(Color.FromArgb(80, 86, 105, 241));
        }

        private void Btn_Setting_MouseLeave(object sender, MouseEventArgs e)
        {
            Btn_Setting.Background = null;
        }

        private void Btn_Setting_MouseUp(object sender, MouseButtonEventArgs e)
        {
        }

        private void Btn_Start_MouseEnter(object sender, MouseEventArgs e)
        {
            Btn_Start.Background = new SolidColorBrush(Color.FromArgb(80, 86, 105, 241));
        }

        private void Btn_Start_MouseLeave(object sender, MouseEventArgs e)
        {
            Btn_Start.Background = null;
        }

        private void Btn_Start_MouseUp(object sender, MouseButtonEventArgs e)
        {
        }

        private void Btn_Stop_MouseEnter(object sender, MouseEventArgs e)
        {
            Btn_Stop.Background = new SolidColorBrush(Color.FromArgb(80, 86, 105, 241));
        }

        private void Btn_Stop_MouseLeave(object sender, MouseEventArgs e)
        {
            Btn_Stop.Background = null;
        }

        private void Btn_Stop_MouseUp(object sender, MouseButtonEventArgs e)
        {
        }

        #endregion Private Methods
    }
}