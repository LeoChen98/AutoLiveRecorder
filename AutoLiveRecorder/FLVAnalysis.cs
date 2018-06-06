using System;
using System.IO;

namespace AutoLiveRecorder
{
    /// <summary>
    /// FLV分析类
    /// </summary>
    internal class FLVAnalysis
    {
        #region Private 字段

        private const int FLV_HEADER_SIZE = 9;

        private const int FLV_TAG_HEADER_SIZE = 11;

        private const int FLV_TAG_SIZE_SIZE = 4;

        private int _i;

        private FileStream fs;

        #endregion Private 字段

        #region Public 构造函数

        /// <summary>
        /// 初始化实例
        /// </summary>
        /// <param name="filename">文件地址</param>
        public FLVAnalysis(string filename)
        {
            fs = new FileStream(filename, FileMode.Open);
        }

        #endregion Public 构造函数

        #region Public 方法

        /// <summary>
        /// 分析
        /// </summary>
        public void Analysis()
        {
            byte[] header = new byte[FLV_TAG_HEADER_SIZE];
            byte[] tmp;
            _i = FLV_HEADER_SIZE + FLV_TAG_SIZE_SIZE;
            header = FsRead(FLV_TAG_HEADER_SIZE);
            int datasize = GetDataSize(header);
            byte[] body = new byte[datasize];
            body = FsRead(datasize);
            switch (header[0])
            {
                case 0x08:

                    break;

                case 0x09:

                    break;

                case 0x12:

                    break;

                default:
                    break;
            }
        }

        #endregion Public 方法

        #region Private 方法

        private byte[] FsRead(int count)
        {
            byte[] tmp = new byte[count];
            if (fs.Length < _i + count)
            {
                fs.Read(tmp, _i, count);
                _i += count;
                return tmp;
            }
            else
            {
                fs.Read(tmp, _i, Convert.ToInt32(fs.Length) - _i);
                _i = Convert.ToInt32(fs.Length - 1);
                return tmp;
            }
        }

        private int GetDataSize(byte[] header)
        {
            byte[] tmp = new byte[3];
            Array.Copy(header, 1, tmp, 0, 3);
            return BitConverter.ToInt32(tmp, 0);
        }

        #endregion Private 方法

        #region Private 类

        private class FLVInfo
        {
        }

        #endregion Private 类
    }
}