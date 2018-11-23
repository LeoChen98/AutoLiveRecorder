using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace AutoLiveRecorder
{
    /// <summary>
    /// TimeSetterWindow.xaml 的交互逻辑
    /// </summary>
    public partial class TimeSetterWindow : Window
    {
        #region Private 字段

        private Cls_WorkListItem Item;

        #endregion Private 字段

        #region Public 构造函数

        public TimeSetterWindow(Cls_WorkListItem Item)
        {
            InitializeComponent();

            this.Item = Item;
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
            DialogResult = false;
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
            if ((bool)RB_EveryDay.IsChecked) Item.Frequency = "每天";
            if ((bool)RB_Once.IsChecked) Item.Frequency = "仅一次";
            if ((bool)RB_EveryWeek.IsChecked)
            {
                Item.Frequency = "每周";
                if ((bool)CB_Monday.IsChecked)
                {
                    Item.Frequency += "一、";
                }
                if ((bool)CB_Tuesday.IsChecked)
                {
                    Item.Frequency += "二、";
                }
                if ((bool)CB_Wednesday.IsChecked)
                {
                    Item.Frequency += "三、";
                }
                if ((bool)CB_Thursday.IsChecked)
                {
                    Item.Frequency += "四、";
                }
                if ((bool)CB_Friday.IsChecked)
                {
                    Item.Frequency += "五、";
                }
                if ((bool)CB_Saturday.IsChecked)
                {
                    Item.Frequency += "六、";
                }
                if ((bool)CB_Sunday.IsChecked)
                {
                    Item.Frequency += "日、";
                }
                Item.Frequency = Item.Frequency.Substring(0, Item.Frequency.Length - 1);
            }

            Item.StartTime = new DateTime(int.Parse(TB_Year.Text), int.Parse(TB_Month.Text), int.Parse(TB_Date.Text), int.Parse(TB_Hour.Text), int.Parse(TB_Minute.Text), int.Parse(TB_Second.Text));
            Close();
        }

        private void TB_Date_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            int data = int.Parse(TB_Date.Text);
            if (e.Delta > 0) data++; else if (e.Delta < 0) data--;
            switch (int.Parse(TB_Month.Text))
            {
                case 1:
                case 3:
                case 5:
                case 7:
                case 8:
                case 10:
                case 12:
                    if (data > 31) data -= 31;
                    if (data < 1) data += 31;
                    break;

                case 4:
                case 6:
                case 9:
                case 11:
                    if (data > 30) data -= 30;
                    if (data < 1) data += 30;
                    break;

                case 2:
                    if ((int.Parse(TB_Year.Text) % 4) == 0)
                    {
                        if (data > 29) data -= 29;
                        if (data < 1) data += 29;
                        break;
                    }
                    else
                    {
                        if (data > 28) data -= 28;
                        if (data < 1) data += 28;
                        break;
                    }
                default:
                    break;
            }
            TB_Date.Text = data.ToString();
        }

        private void TB_Hour_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            int data = int.Parse(TB_Hour.Text);
            if (e.Delta > 0) data++; else if (e.Delta < 0) data--;
            if (data > 23) data -= 24;
            if (data < 0) data += 24;
            TB_Hour.Text = data.ToString();
        }

        private void TB_Minute_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            int data = int.Parse(TB_Minute.Text);
            if (e.Delta > 0) data++; else if (e.Delta < 0) data--;
            if (data > 59) data -= 60;
            if (data < 0) data += 60;
            TB_Minute.Text = data.ToString();
        }

        private void TB_Month_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            int data = int.Parse(TB_Month.Text);
            if (e.Delta > 0) data++; else if (e.Delta < 0) data--;
            if (data > 12) data -= 12;
            if (data < 1) data += 12;
            TB_Month.Text = data.ToString();
        }

        private void TB_Second_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            int data = int.Parse(TB_Second.Text);
            if (e.Delta > 0) data++; else if (e.Delta < 0) data--;
            if (data > 59) data -= 60;
            if (data < 0) data += 60;
            TB_Second.Text = data.ToString();
        }

        private void TB_Year_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            int data = int.Parse(TB_Year.Text);
            if (e.Delta > 0) data++; else if (e.Delta < 0) data--;
            if (data > 9999) data = 0;
            if (data < 0) data += 9999;
            TB_Year.Text = data.ToString();
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
            if (!string.IsNullOrEmpty(Item.Frequency))
            {
                switch (Item.Frequency)
                {
                    case "仅一次":
                        RB_Once.IsChecked = true;
                        break;

                    case "每天":
                        RB_EveryDay.IsChecked = true;
                        break;

                    default:
                        if (Item.Frequency.Contains("每周"))
                        {
                            RB_EveryWeek.IsChecked = true;
                            if (Item.Frequency.Contains("一")) CB_Monday.IsChecked = true;
                            if (Item.Frequency.Contains("二")) CB_Tuesday.IsChecked = true;
                            if (Item.Frequency.Contains("三")) CB_Wednesday.IsChecked = true;
                            if (Item.Frequency.Contains("四")) CB_Thursday.IsChecked = true;
                            if (Item.Frequency.Contains("五")) CB_Friday.IsChecked = true;
                            if (Item.Frequency.Contains("六")) CB_Saturday.IsChecked = true;
                            if (Item.Frequency.Contains("日")) CB_Sunday.IsChecked = true;
                        }
                        break;
                }
            }

            if (Item.StartTime != null && Item.StartTime != new DateTime())
            {
                TB_Year.Text = Item.StartTime.Year.ToString();
                TB_Month.Text = Item.StartTime.Month.ToString();
                TB_Date.Text = Item.StartTime.Day.ToString();
                TB_Hour.Text = Item.StartTime.Hour.ToString();
                TB_Minute.Text = Item.StartTime.Minute.ToString();
                TB_Second.Text = Item.StartTime.Second.ToString();
            }
            else
            {
                TB_Year.Text = DateTime.Now.Year.ToString();
                TB_Month.Text = DateTime.Now.Month.ToString();
                TB_Date.Text = DateTime.Now.Day.ToString();
                TB_Hour.Text = DateTime.Now.Hour.ToString();
                TB_Minute.Text = DateTime.Now.Minute.ToString();
                TB_Second.Text = DateTime.Now.Second.ToString();
            }
        }

        #endregion Private 方法
    }
}