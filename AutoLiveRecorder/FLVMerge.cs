using System;
using System.Collections.Generic;
using System.IO;

namespace AutoLiveRecorder
{
    public class FlvMerge
    {
        #region Public Methods

        /// <summary>
        /// 合并函数
        /// </summary>
        /// <param name="files">文件路径列表</param>
        /// <param name="savepath">合成后文件保存位置</param>
        public static void Merge(List<string> files, string savepath)
        {
            int t = 0;//总时间戳
            int i = 0;//指针
            int c = 0;//文件序号

            //打开输出流
            FileStream Fs = File.Open(savepath, FileMode.Create, FileAccess.ReadWrite);

            //打开输入流
            FileStream Fr = File.OpenRead(files[0]);

            //取第一个文件的头作为公共头

            byte[] bheaderLength = new byte[4];
            Fr.Position += 5;
            Fr.Read(bheaderLength, 0, 4);
            int headerLength = ToInt32HL(bheaderLength);            //取头部长度

            byte[] bheader = new byte[headerLength];
            Fr.Position = 0;
            Fr.Read(bheader, 0, headerLength);
            Fs.Write(bheader, 0, headerLength);
            i += headerLength;

            byte[] btype = new byte[1];
            Fr.Position += 4;
            Fr.Read(btype, 0, 1);
            //取第一个文件的MetaData作为公共MetaData（如果存在的话）
            if (btype[0] == 0x12)
            {
                byte[] bMetaDataSize = new byte[3];
                Fr.Read(bMetaDataSize, 0, 3);
                int MetaDataSize = ToInt24HL(bMetaDataSize);                //获取MetaData的大小
                byte[] bMetaTag = new byte[MetaDataSize + 15];
                Fr.Position = i;
                Fr.Read(bMetaTag, 0, MetaDataSize + 15);
                Fs.Write(bMetaTag, 0, MetaDataSize + 15);
                i += MetaDataSize + 15;
            }

            //合并
            foreach (string file in files)
            {
                int ct = 0;                //分时间戳

                if (c != 0)
                {
                    Fr = File.OpenRead(file);
                    byte[] b = new byte[4];
                    Fr.Position += 5;
                    Fr.Read(b, 0, 4);
                    i = ToInt32HL(b);
                }

                //遍历Tag
                while (i + 15 < Fr.Length)
                {
                    Fr.Position += 4;
                    btype = new byte[1];
                    Fr.Read(btype, 0, 1);
                    //获取Tag长度
                    byte[] btagsize = new byte[3];
                    Fr.Read(btagsize, 0, 3);
                    int tagSize = ToInt24HL(btagsize) + 15;

                    if (btype[0] != 0x12)                        //跳过MetaDataTag
                    {
                        byte[] bts = new byte[4];
                        Fr.Read(bts, 0, 4);
                        ct = (int)GetTimeStamp(bts);

                        byte[] tmp = new byte[tagSize];
                        Fr.Position = i;
                        Fr.Read(tmp, 0, tagSize);

                        if (c != 0)         //判断是否第一个文件
                        {
                            Array.Copy(GetTimeStampB(ct + t), 0, tmp, 8, 4);
                        }

                        if (Fr.Length >= i + tagSize)
                        {
                            Fs.Write(tmp, 0, tagSize);
                        }
                    }

                    i += tagSize;//移动指针
                }
                Fr.Close();//关闭文件

                i = 0; ;//初始化指针
                t += ct;//更新总时间戳
                c++;//更新文件序号
            }

            //关闭输出流
            Fs.Close();
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// 获取FLV Tag的时间戳
        /// </summary>
        /// <param name="data">字节数组</param>
        /// <param name="offset">偏移，默认为0</param>
        /// <returns>时间戳</returns>
        private static uint GetTimeStamp(byte[] data, int offset = 0)
        {
            return (uint)((data[offset + 2] & 0xFF) | ((data[offset + 1] & 0xFF) << 8) | ((data[offset] & 0xFF) << 16) | ((data[offset + 3] & 0xFF) << 24));
        }

        /// <summary>
        /// 获取FLV Tag时间戳的字节形式
        /// </summary>
        /// <param name="TimeStamp">时间戳</param>
        /// <returns>字节数组</returns>
        private static byte[] GetTimeStampB(int TimeStamp)
        {
            byte[] b = new byte[4];
            b = BitConverter.GetBytes(TimeStamp);
            if (BitConverter.IsLittleEndian)
            {
                return new byte[4] { b[2], b[1], b[0], b[3] };
            }
            else
            {
                return new byte[4] { b[1], b[2], b[3], b[0] };
            }
        }

        /// <summary>
        /// 取24位高位有符号整型
        /// </summary>
        /// <param name="data">字节数组</param>
        /// <param name="offset">偏移</param>
        /// <returns>整型</returns>
        private static int ToInt24HL(byte[] data, int offset = 0)
        {
            return (int)(data[offset + 2] & 0xFF) | ((data[offset + 1] & 0xFF) << 8) | ((data[offset] & 0xFF) << 16);
        }

        /// <summary>
        /// 取32位高位有符号整型
        /// </summary>
        /// <param name="data">字节数组</param>
        /// <param name="offset">偏移</param>
        /// <returns>整型</returns>
        private static int ToInt32HL(byte[] data, int offset = 0)
        {
            return (int)((data[offset + 3] & 0xFF) | ((data[offset + 2] & 0xFF) << 8) | ((data[offset + 1] & 0xFF) << 16) | ((data[offset] & 0xFF) << 24));
        }

        /// <summary>
        /// 取32位高位无符号整型
        /// </summary>
        /// <param name="data">字节数组</param>
        /// <param name="offset">偏移</param>
        /// <returns>整型</returns>
        private static uint ToUInt32HL(byte[] data, int offset = 0)
        {
            return (uint)((data[offset + 3] & 0xFF) | ((data[offset + 2] & 0xFF) << 8) | ((data[offset + 1] & 0xFF) << 16) | ((data[offset] & 0xFF) << 24));
        }

        #endregion Private Methods
    }
}