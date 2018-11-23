using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace AutoLiveRecorder
{
    /// <summary>
    /// WorkListItem.xaml 的交互逻辑
    /// </summary>
    public partial class WorkListItem : UserControl
    {
        #region Private Fields

        private Cls_WorkListItem Item;

        #endregion Private Fields

        #region Public Constructors

        public WorkListItem(Cls_WorkListItem Item)
        {
            InitializeComponent();

            this.Item = Item;

            Binding bindTitle = new Binding();
            bindTitle.Source = Item;
            bindTitle.Mode = BindingMode.OneWay;
            bindTitle.Path = new System.Windows.PropertyPath("RoomTitle");
            BindingOperations.SetBinding(lbl_RoomTitle, ContentProperty, bindTitle);

            Binding bindInfo = new Binding();
            bindInfo.Source = Item;
            bindInfo.Mode = BindingMode.OneWay;
            bindInfo.Path = new System.Windows.PropertyPath("RoomInfo");
            BindingOperations.SetBinding(lbl_RoomInfo, ContentProperty, bindInfo);

            Binding bindStatus = new Binding();
            bindStatus.Source = Item;
            bindStatus.Mode = BindingMode.OneWay;
            bindStatus.Path = new System.Windows.PropertyPath("Status");
            BindingOperations.SetBinding(lbl_TaskStatus, ContentProperty, bindStatus);
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

        private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
        }

        #endregion Private Methods
    }
}