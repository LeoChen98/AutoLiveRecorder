using System.Collections.Generic;
using System.Windows.Controls;

namespace AutoLiveRecorder
{
    /// <summary>
    /// WorkList.xaml 的交互逻辑
    /// </summary>
    public partial class WorkList : UserControl
    {
        #region Public Fields

        /// <summary>
        /// 选中项列表
        /// </summary>
        public List<SelectedItem> SelectedList;

        #endregion Public Fields

        #region Public Constructors

        /// <summary>
        /// 构造函数
        /// </summary>
        public WorkList()
        {
            InitializeComponent();

            Bas.TaskList = new List<Cls_WorkListItem>();
            SelectedList = new List<SelectedItem>();
        }

        #endregion Public Constructors

        #region Public Methods

        /// <summary>
        /// 添加任务
        /// </summary>
        /// <param name="task"></param>
        public void AddTask(Cls_WorkListItem task)
        {
            Container.Children.Add(new WorkListItem(task, this));
            RefreshNoitce();
        }

        /// <summary>
        /// 全部开始
        /// </summary>
        public void AllStart()
        {
        }

        /// <summary>
        /// 全部停止
        /// </summary>
        public void AllStop()
        {
        }

        /// <summary>
        /// 子控件选中调用
        /// </summary>
        /// <param name="item">子控件实例</param>
        /// <param name="task">任务实例</param>
        /// <param name="IsSelected">指示是否选中</param>
        public void ItemSelected(WorkListItem item, Cls_WorkListItem task, bool IsSelected)
        {
            if (IsSelected)
            {
                SelectedList.Add(new SelectedItem() { Item = item, Task = task });
            }
            else
            {
                SelectedList.Remove(new SelectedItem() { Item = item, Task = task });
            }
        }

        /// <summary>
        /// 刷新空列表提示显示
        /// </summary>
        public void RefreshNoitce()
        {
            if (Container.Children.Count > 0)
            {
                EmptyNotice.Visibility = System.Windows.Visibility.Hidden;
            }
            else
            {
                EmptyNotice.Visibility = System.Windows.Visibility.Visible;
            }
        }

        /// <summary>
        /// 移除任务
        /// </summary>
        public void RemoveTask()
        {
            foreach (SelectedItem i in SelectedList)
            {
                if (i.Task.Status != Cls_WorkListItem.StatusCode.Recording)
                {
                    Bas.TaskList.Remove(i.Task);
                    Container.Children.Remove(i.Item);
                    i.Task = null;
                }
                else
                {
                    if (System.Windows.Forms.MessageBox.Show(i.Task.RoomTitle + "正在录制中，是否继续删除？", "录播姬", System.Windows.Forms.MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                    {
                        i.Task.StopRecord();
                        Bas.TaskList.Remove(i.Task);
                        Container.Children.Remove(i.Item);
                        i.Task = null;
                    }
                    else
                    {
                        i.Item.checkBox.IsChecked = false;
                    }
                }
            }
            SelectedList.Clear();
            RefreshNoitce();
        }

        #endregion Public Methods

        #region Public Classes

        public class SelectedItem
        {
            #region Public Fields

            public WorkListItem Item;
            public Cls_WorkListItem Task;

            #endregion Public Fields
        }

        #endregion Public Classes
    }
}