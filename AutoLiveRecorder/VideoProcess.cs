using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace AutoLiveRecorder
{
    internal class VideoProcess
    {
        #region Private 字段

        private static Process FFMpeg = new Process();

        #endregion Private 字段

        #region Public 委托

        /// <summary>
        /// 转码完成的事件处理程序
        /// </summary>
        /// <param name="filename">最终输出视频的路径</param>
        public delegate void TranscodeFinishedEventHandler(string filename);

        #endregion Public 委托

        #region Public 事件

        /// <summary>
        /// 转码完成的事件
        /// </summary>
        public static event TranscodeFinishedEventHandler TranscodeFinished;

        #endregion Public 事件

        #region Public 方法

        /// <summary>
        /// 合并flv并转码为mp4
        /// </summary>
        /// <param name="filelist">文件列表</param>
        /// <param name="savepath">保存位置</param>
        public static void TranscodeToMp4(List<string> filelist, string savepath)
        {
            if (System.IO.File.Exists("ffmpeg.exe"))
            {
                //初始化ffmpeg启动设置
                FFMpeg.StartInfo.FileName = @"ffmpeg.exe";
                FFMpeg.StartInfo.Arguments = "";
                FFMpeg.StartInfo.UseShellExecute = false;
                FFMpeg.StartInfo.RedirectStandardError = true;
                FFMpeg.StartInfo.CreateNoWindow = false;
                FFMpeg.ErrorDataReceived += new DataReceivedEventHandler(Output);
                string savepathn = savepath.Substring(0, savepath.LastIndexOf("."));
                string concatstr = "";
                //转换合并格式
                if (filelist.Count > 1)
                {
                    foreach (string i in filelist)
                    {
                        FFMpeg.StartInfo.Arguments = "-i \"" + i + "\" -c copy -bsf:v h264_mp4toannexb -f mpegts \"" + i + ".ts\"";
                        FFMpeg.Start();
                        FFMpeg.WaitForExit();
                        FFMpeg.Close();
                        if (concatstr == "")
                        {
                            concatstr = "concat:\"" + i + ".ts\"";
                        }
                        else
                        {
                            concatstr += "|\"" + i + ".ts\"";
                        }
                    }
                    //合并文件
                    FFMpeg.StartInfo.Arguments = "-i \"" + concatstr + "\" -c copy -bsf:a aac_adtstoasc -movflags +faststart \"" + savepathn + ".mp4\"";
                    FFMpeg.Start();
                    FFMpeg.WaitForExit();
                    FFMpeg.Close();
                    FFMpeg.Dispose();
                    TranscodeFinished(savepathn + ".mp4");
                }
                else if (filelist.Count == 1)
                {
                    //转码文件
                    FFMpeg.StartInfo.Arguments = "-i \"" + filelist[0] + "\" -c copy -bsf:a aac_adtstoasc -movflags +faststart \"" + savepathn + ".mp4\"";
                    FFMpeg.Start();
                    FFMpeg.WaitForExit();
                    FFMpeg.Close();
                    FFMpeg.Dispose();
                    TranscodeFinished(savepathn + ".mp4");
                }
                //删除临时文件
                foreach (string i in filelist)
                {
                    System.IO.File.Delete(i + ".ts");
                }
            }
            else
            {
                throw new Exception("ffmpeg.exe is not exist.");
            }
        }

        #endregion Public 方法

        #region Private 方法

        private static void Output(object sendProcess, DataReceivedEventArgs output)
        {
        }

        #endregion Private 方法
    }
}