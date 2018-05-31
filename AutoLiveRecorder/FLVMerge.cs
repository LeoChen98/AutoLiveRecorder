using System;
using System.Collections.Generic;
using System.IO;

namespace AutoLiveRecorder
{
    /// <summary>
    /// FLV合并静态类
    /// </summary>
    internal class FLVMerge
    {
        #region Private 字段

        private const int FLV_HEADER_SIZE = 9;

        private const int FLV_TAG_HEADER_SIZE = 11;

        private const int MAX_DATA_SIZE = 16777220;

        #endregion Private 字段

        #region Public 方法

        /// <summary>
        /// 获取flv文件列表
        /// </summary>
        /// <param name="savepath">文件本名</param>
        /// <returns>文件列表</returns>
        public static List<string> GetPathList(string savepath)
        {
            List<string> results = new List<string>();
            int ii = 1;
            string savepathx = savepath;
            string savepathn = savepath.Substring(0, savepath.LastIndexOf("."));
            savepathx = savepathn + "-1.flv";
            while (File.Exists(savepathx))
            {
                results.Add(savepathx);
                ii++;
                savepathx = savepathn + "-" + ii + ".flv";
            }
            return results;
        }

        /// <summary>
        /// 开始合并
        /// </summary>
        /// <param name="paths">文件列表</param>
        /// <param name="savepath">输出文件</param>
        public static void StartMarge(List<string> paths, string savepath)
        {
            FileStream fsMerge = new FileStream(savepath, FileMode.Create);
            FileStream inputfs;
            int time = 0;
            foreach (string i in paths)
            {
                inputfs = new FileStream(i, FileMode.Open);
                if (time == 0)
                {
                    time = Merge(inputfs, fsMerge, true, 0);
                }
                else
                {
                    time = Merge(inputfs, fsMerge, false, time);
                }
                inputfs.Close();
                File.Delete(i);
            }
            fsMerge.Close();
        }

        #endregion Public 方法

        #region Private 方法

        private static int FromInt24StringBe(byte b0, byte b1, byte b2)
        {
            return (int)((b0 << 16) | (b1 << 8) | (b2));
        }

        private static FLVContext GetFLVFileInfo(FileStream fs)
        {
            bool hasAudioParams, hasVideoParams;
            int skipSize, readLen;
            int dataSize;
            byte tagType;
            byte[] tmp = new byte[FLV_TAG_HEADER_SIZE + 1];
            if (fs == null) return null;

            FLVContext flvCtx = new FLVContext();
            fs.Position = 0;
            skipSize = 9;
            fs.Position += skipSize;
            hasVideoParams = hasAudioParams = false;
            skipSize = 4;
            while (!hasVideoParams || !hasAudioParams)
            {
                fs.Position += skipSize;

                if (FLV_TAG_HEADER_SIZE + 1 != fs.Read(tmp, 0, tmp.Length))
                    return null;

                tagType = (byte)(tmp[0] & 0x1f);
                switch (tagType)
                {
                    case 8:
                        flvCtx.soundFormat = (byte)((tmp[FLV_TAG_HEADER_SIZE] & 0xf0) >> 4);
                        flvCtx.soundRate = (byte)((tmp[FLV_TAG_HEADER_SIZE] & 0x0c) >> 2);
                        flvCtx.soundSize = (byte)((tmp[FLV_TAG_HEADER_SIZE] & 0x02) >> 1);
                        flvCtx.soundType = (byte)((tmp[FLV_TAG_HEADER_SIZE] & 0x01) >> 0);
                        hasAudioParams = true;
                        break;

                    case 9:
                        flvCtx.videoCodecID = (byte)((tmp[FLV_TAG_HEADER_SIZE] & 0x0f));
                        hasVideoParams = true;
                        break;

                    default:
                        break;
                }

                dataSize = FromInt24StringBe(tmp[1], tmp[2], tmp[3]);
                skipSize = dataSize - 1 + 4;
            }

            return flvCtx;
        }

        private static int GetTimestamp(byte b0, byte b1, byte b2, byte b3)
        {
            return ((b3 << 24) | (b0 << 16) | (b1 << 8) | (b2));
        }

        private static bool IsFLVFile(FileStream fs)
        {
            int len;
            byte[] buf = new byte[FLV_HEADER_SIZE];
            fs.Position = 0;
            if (FLV_HEADER_SIZE != fs.Read(buf, 0, buf.Length))
                return false;

            if (buf[0] != 'F' || buf[1] != 'L' || buf[2] != 'V' || buf[3] != 0x01)
                return false;
            else
                return true;
        }

        private static bool IsSuitableToMerge(FLVContext flvCtx1, FLVContext flvCtx2)
        {
            return (flvCtx1.soundFormat == flvCtx2.soundFormat) &&
              (flvCtx1.soundRate == flvCtx2.soundRate) &&
              (flvCtx1.soundSize == flvCtx2.soundSize) &&
              (flvCtx1.soundType == flvCtx2.soundType) &&
              (flvCtx1.videoCodecID == flvCtx2.videoCodecID);
        }

        private static int Merge(FileStream fsInput, FileStream fsMerge, bool isFirstFile, int lastTimestamp = 0)
        {
            int readLen;
            int curTimestamp = 0;
            int newTimestamp = 0;
            int dataSize;
            byte[] tmp = new byte[20];
            byte[] buf = new byte[MAX_DATA_SIZE];

            fsInput.Position = 0;
            if (isFirstFile)
            {
                if (FLV_HEADER_SIZE + 4 == (fsInput.Read(tmp, 0, FLV_HEADER_SIZE + 4)))
                {
                    fsMerge.Position = 0;
                    fsMerge.Write(tmp, 0, FLV_HEADER_SIZE + 4);
                }
            }
            else
            {
                fsInput.Position = FLV_HEADER_SIZE + 4;
            }

            while (fsInput.Read(tmp, 0, FLV_TAG_HEADER_SIZE) > 0)
            {
                dataSize = FromInt24StringBe(tmp[1], tmp[2], tmp[3]);
                curTimestamp = GetTimestamp(tmp[4], tmp[5], tmp[6], tmp[7]);
                newTimestamp = curTimestamp + lastTimestamp;
                SetTimestamp(tmp, 4, newTimestamp);
                fsMerge.Write(tmp, 0, FLV_TAG_HEADER_SIZE);

                readLen = dataSize + 4;
                if (fsInput.Read(buf, 0, readLen) > 0)
                {
                    fsMerge.Write(buf, 0, readLen);
                }
                else
                {
                    goto failed;
                }
            }

            return newTimestamp;

            failed:
            throw new Exception("Merge Failed");
        }

        private static void SetTimestamp(byte[] data, int idx, int newTimestamp)
        {
            data[idx + 3] = (byte)(newTimestamp >> 24);
            data[idx + 0] = (byte)(newTimestamp >> 16);
            data[idx + 1] = (byte)(newTimestamp >> 8);
            data[idx + 2] = (byte)(newTimestamp);
        }

        #endregion Private 方法

        #region Private 类

        private class FLVContext
        {
            #region Public 字段

            public byte soundFormat;
            public byte soundRate;
            public byte soundSize;
            public byte soundType;
            public byte videoCodecID;

            #endregion Public 字段
        }

        #endregion Private 类
    }
}