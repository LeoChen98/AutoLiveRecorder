using System.Windows;

namespace AutoLiveRecorder
{
    internal class DependencyProperties
    {
        #region Public Fields

        /// <summary>
        /// CheckBox.IsCheck属性
        /// </summary>
        public static DependencyProperty IsChecked = DependencyProperty.Register("IsChecked", typeof(bool), typeof(System.Windows.Controls.CheckBox));

        #endregion Public Fields
    }
}