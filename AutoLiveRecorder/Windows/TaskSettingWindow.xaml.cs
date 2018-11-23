using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace AutoLiveRecorder
{
    /// <summary>
    /// TaskSettingWindow.xaml 的交互逻辑
    /// </summary>
    public partial class TaskSettingWindow : Window
    {
        #region Private 字段

        /// <summary>
        /// 是否是新建
        /// </summary>
        private bool IsNew = false;

        /// <summary>
        /// 工作项实例
        /// </summary>
        private Cls_WorkListItem Item;

        #endregion Private 字段

        #region Public 构造函数

        /// <summary>
        /// 新建任务构造函数
        /// </summary>
        public TaskSettingWindow()
        {
            InitializeComponent();

            Item = new Cls_WorkListItem();
            IsNew = true;

            SetBinding();
        }

        /// <summary>
        /// 修改任务构造函数
        /// </summary>
        /// <param name="Item"></param>
        public TaskSettingWindow(Cls_WorkListItem Item)
        {
            InitializeComponent();

            this.Item = Item;

            SetBinding();
        }

        #endregion Public 构造函数

        #region Private 方法

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
            if (TB_RoomInfo.Text != "" && TB_RoomInfo.Text != "URL格式错误或平台未支持。")
            {
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("请输入正确的URL！");
            }
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
            Btn_ShowTimeSetterWindow.Background = new SolidColorBrush(Color.FromArgb(204, 255, 255, 255));
        }

        private void Btn_ShowTimeSetterWindow_MouseLeave(object sender, MouseEventArgs e)
        {
            Btn_ShowTimeSetterWindow.Background = new SolidColorBrush(Color.FromArgb(153, 255, 255, 255));
        }

        private void Btn_ShowTimeSetterWindow_MouseUp(object sender, MouseButtonEventArgs e)
        {
            TimeSetterWindow t = new TimeSetterWindow(Item);
            t.ShowDialog();
        }

        /// <summary>
        /// 设置数据绑定
        /// </summary>
        private void SetBinding()
        {
            Binding bindFNT = new Binding();
            bindFNT.Source = Item;
            bindFNT.Mode = BindingMode.OneWay;
            bindFNT.Path = new PropertyPath("FNT");
            BindingOperations.SetBinding(lbl_TimeInfo, ContentProperty, bindFNT);

            DependencyProperty IsChecked = DependencyProperty.Register("IsChecked", typeof(bool), typeof(System.Windows.Controls.CheckBox));

            Binding bindDanmu = new Binding();
            bindDanmu.Source = Item;
            bindDanmu.Mode = BindingMode.TwoWay;
            bindDanmu.Path = new PropertyPath("IsRecordDanmu");
            BindingOperations.SetBinding(CB_IsRecordDanmu, IsChecked, bindDanmu);

            Binding bindTrans = new Binding();
            bindTrans.Source = Item;
            bindTrans.Mode = BindingMode.TwoWay;
            bindTrans.Path = new PropertyPath("IsTranslateAfterCompleted");
            BindingOperations.SetBinding(CB_IsTranslateAfterCompleted, IsChecked, bindTrans);
        }

        private void TB_URL_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            TB_RoomInfo.Text = Bas.AnalysisURL(TB_URL.Text);
        }

        private void TitleGrid_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void window_Loaded(object sender, RoutedEventArgs e)
        {
            switch (Item.StartMode)
            {
                case Cls_WorkListItem.StartModeType.Now:
                    RB_StartNow.IsChecked = true;
                    break;

                case Cls_WorkListItem.StartModeType.WhenStart:
                    RB_StartWhenLiveStart.IsChecked = true;
                    break;

                case Cls_WorkListItem.StartModeType.WhenTime:
                    RB_StartWhenTime.IsChecked = true;
                    break;

                default:
                    break;
            }
        }

        #endregion Private 方法
    }
}