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

        private new WorkList Parent;

        #endregion Private Fields

        #region Public Constructors

        public WorkListItem(Cls_WorkListItem Item, WorkList Parent)
        {
            InitializeComponent();

            this.Item = Item;
            this.Parent = Parent;

            Binding bindTitle = new Binding
            {
                Source = Item,
                Mode = BindingMode.OneWay,
                Path = new System.Windows.PropertyPath("RoomTitle")
            };
            BindingOperations.SetBinding(lbl_RoomTitle, ContentProperty, bindTitle);

            Binding bindInfo = new Binding
            {
                Source = Item,
                Mode = BindingMode.OneWay,
                Path = new System.Windows.PropertyPath("RoomInfo")
            };
            BindingOperations.SetBinding(lbl_RoomInfo, ContentProperty, bindInfo);

            Binding bindStatus = new Binding
            {
                Source = Item,
                Mode = BindingMode.OneWay,
                Path = new System.Windows.PropertyPath("StatusString")
            };
            BindingOperations.SetBinding(lbl_TaskStatus, ContentProperty, bindStatus);

            //Binding bindStatuscolor = new Binding
            //{
            //    Source = Item,
            //    Mode = BindingMode.OneWay,
            //    Path = new System.Windows.PropertyPath("StatusColor")
            //};
            //BindingOperations.SetBinding(lbl_TaskStatus, ForegroundProperty, bindStatuscolor);
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
            if (Item.Status != Cls_WorkListItem.StatusCode.Recording)
            {
                TaskSettingWindow t = new TaskSettingWindow(Item, Parent);
                t.Show();
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("录制进行中，请先结束录制。");
            }
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
            if (Item.StartMode == Cls_WorkListItem.StartModeType.Manuel)
            {
                Item.StartRecord(true);
            }
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
            Item.StopRecord();
        }

        private void CheckBox_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            Parent.ItemSelected(this, Item, (bool)checkBox.IsChecked);
        }

        private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
        }

        #endregion Private Methods
    }
}