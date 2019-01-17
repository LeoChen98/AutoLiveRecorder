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
        #region Private Fields

        /// <summary>
        /// 是否是新建
        /// </summary>
        private bool IsNew = false;

        /// <summary>
        /// 工作项实例
        /// </summary>
        private Cls_WorkListItem Item;

        /// <summary>
        /// 任务列表控件
        /// </summary>
        private WorkList TaskList;

        #endregion Private Fields

        #region Public Constructors

        /// <summary>
        /// 新建任务构造函数
        /// </summary>
        public TaskSettingWindow(WorkList TaskList)
        {
            InitializeComponent();

            this.TaskList = TaskList;

            Item = new Cls_WorkListItem();
            IsNew = true;

            SetBinding();
        }

        /// <summary>
        /// 修改任务构造函数
        /// </summary>
        /// <param name="Item"></param>
        public TaskSettingWindow(Cls_WorkListItem Item, WorkList TaskList)
        {
            InitializeComponent();

            this.TaskList = TaskList;

            this.Item = Item;

            SetBinding();
        }

        #endregion Public Constructors

        #region Private Methods

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
            if (TB_RoomInfo.Text != "" && TB_RoomInfo.Text != "URL格式错误或平台未支持。")
            {
                if (!(bool)RB_StartWhenTime.IsChecked || lbl_TimeInfo.Content.ToString() != "")
                {
                    if ((bool)RB_StartNow.IsChecked)
                        Item.StartMode = Cls_WorkListItem.StartModeType.Now;
                    else if ((bool)RB_StartWhenLiveStart.IsChecked)
                        Item.StartMode = Cls_WorkListItem.StartModeType.WhenStart;
                    else if ((bool)RB_Manuel.IsChecked)
                        Item.StartMode = Cls_WorkListItem.StartModeType.Manuel;
                    else if ((bool)RB_StartWhenTime.IsChecked)
                        Item.StartMode = Cls_WorkListItem.StartModeType.WhenTime;

                    Item.IsRepeat = (bool)CB_IsRepeat.IsChecked;
                    Item.IsTranslateAfterCompleted = (bool)CB_IsTranslateAfterCompleted.IsChecked;

                    if (IsNew)
                    {
                        Bas.TaskList.Add(Item);
                        TaskList.AddTask(Item);
                    }

                    Item.SettingFinished();
                    Close();
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("请选择正确的启动条件或启动时间！");
                }
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

        private void RB_StartWhenLiveStart_Click(object sender, RoutedEventArgs e)
        {
            if ((bool)RB_StartWhenLiveStart.IsChecked)
            {
                CB_IsRepeat.Visibility = Visibility.Visible;
            }
            else
            {
                CB_IsRepeat.Visibility = Visibility.Hidden;
            }
        }

        /// <summary>
        /// 设置数据绑定
        /// </summary>
        private void SetBinding()
        {
            Binding bindFNT = new Binding
            {
                Source = Item,
                Mode = BindingMode.OneWay,
                Path = new PropertyPath("FNT")
            };
            BindingOperations.SetBinding(lbl_TimeInfo, ContentProperty, bindFNT);

            Binding bindDanmu = new Binding
            {
                Source = Item,
                Mode = BindingMode.TwoWay,
                Path = new PropertyPath("IsRecordDanmu")
            };
            BindingOperations.SetBinding(CB_IsRecordDanmu, DependencyProperties.IsChecked, bindDanmu);

            Binding bindTrans = new Binding
            {
                Source = Item,
                Mode = BindingMode.TwoWay,
                Path = new PropertyPath("IsTranslateAfterCompleted")
            };
            BindingOperations.SetBinding(CB_IsTranslateAfterCompleted, DependencyProperties.IsChecked, bindTrans);

            Binding bindInfo = new Binding
            {
                Source = Item,
                Mode = BindingMode.OneWay,
                Path = new PropertyPath("RoomInfoLong")
            };
            BindingOperations.SetBinding(TB_RoomInfo, System.Windows.Controls.TextBox.TextProperty, bindInfo);
            Binding bindURL = new Binding
            {
                Source = Item,
                Mode = BindingMode.OneWay,
                Path = new PropertyPath("URL")
            };
            BindingOperations.SetBinding(TB_URL, System.Windows.Controls.TextBox.TextProperty, bindURL);

            Binding bindIsRepeat = new Binding
            {
                Source = Item,
                Mode = BindingMode.OneWay,
                Path = new PropertyPath("IsRepeat")
            };
            BindingOperations.SetBinding(CB_IsRepeat, DependencyProperties.IsChecked, bindIsRepeat);
        }

        private void TB_URL_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            Bas.AnalysisURL(TB_URL.Text, ref Item);
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

                case Cls_WorkListItem.StartModeType.Manuel:
                    RB_Manuel.IsChecked = true;
                    break;

                default:
                    break;
            }
            CB_IsTranslateAfterCompleted.IsChecked = Item.IsTranslateAfterCompleted;
            CB_IsRepeat.IsChecked = Item.IsRepeat;
        }

        #endregion Private Methods
    }
}