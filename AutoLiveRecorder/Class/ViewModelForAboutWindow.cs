using System.ComponentModel;

namespace AutoLiveRecorder
{
    public class ViewModelForAboutWindow : INotifyPropertyChanged
    {
        #region Public Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion Public Events

        #region Public Properties

        /// <summary>
        /// 程序版本字符串
        /// </summary>
        public static string VerString
        {
            get
            {
                return "程序版本：" + System.Windows.Forms.Application.ProductVersion;
            }
        }

        #endregion Public Properties
    }
}