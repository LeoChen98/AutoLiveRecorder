using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace AutoLiveRecorder
{
    class VideoProcess
    {
        static Process FFMpeg = new Process();
        public static void TranscodeToMp4(List<string> filelist,string savepath)
        {
            if (System.IO.File.Exists("ffmpeg.exe"))
            {
                FFMpeg.StartInfo.FileName = @"ffmpeg.exe";
                FFMpeg.StartInfo.Arguments = "";
                FFMpeg.StartInfo.UseShellExecute = false;
                FFMpeg.StartInfo.RedirectStandardError = true;
                FFMpeg.StartInfo.CreateNoWindow = false;
                FFMpeg.ErrorDataReceived += new DataReceivedEventHandler(Output);
                string savepathn = savepath.Substring(0, savepath.LastIndexOf("."));
                string concatstr = "";
                foreach (string i in filelist)
                {
                    FFMpeg.StartInfo.Arguments = "-i \"" + i + "\" -c copy -bsf:v h264_mp4toannexb -f mpegts \"" + i + ".ts\"";
                    FFMpeg.Start();
                    FFMpeg.BeginErrorReadLine();
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
                FFMpeg.StartInfo.Arguments = "-i \"" + concatstr + "\" -c copy -bsf:a aac_adtstoasc -movflags +faststart \"" + savepathn + ".mp4\"";
                FFMpeg.Start();
                FFMpeg.BeginErrorReadLine();
                FFMpeg.WaitForExit();
                FFMpeg.Close();
            }
            else
            {
                throw new Exception("ffmpeg.exe is not exist.");
            }
        }

        static void Output(object sendProcess, DataReceivedEventArgs output)
        {
            
        }
    }
}
