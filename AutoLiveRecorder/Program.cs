using System;
using System.Windows.Forms;

namespace AutoLiveRecorder
{
    internal static class Program
    {
        #region Private 方法

        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }

        #endregion Private 方法
    }
}