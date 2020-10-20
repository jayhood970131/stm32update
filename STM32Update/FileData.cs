using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.IO;
namespace STM32Update
{
    public class FileData
    {
        protected byte[] config_data = null;  //存放数据,一次性从二进制文件中load入
        
        /* size 指定一帧大小
         * frameByteCount提供数据字节数*/
        

        public FileData()
        {

        }
        public int getDataLength()
        {
            if (this.config_data == null)
                return 0;
            return this.config_data.Length;
        }

        public byte getElementByIndex(int i)
        {
            if (this.config_data == null)
                return 0;
            if (i >= this.config_data.Length || i < 0)
                return 0;
            return this.config_data[i];
        }

        public bool setElementByIndex(int i,byte value)
        {
            if (this.config_data == null)
                return false;
            if(i >= this.config_data.Length || i < 0)
                return false;
            this.config_data[i] = value;
            return true;
        }
      
    }

    public class IAPFileData:FileData
    {
        private FrameIAP currentFrame = null;  //基于BCP协议的一帧数据 用于发送IAP数据
        private int currentFrameNum = 0;  //当前帧数
        private int totalFrameNum = 0;    //数据共几帧
        private int frameDataSize = 1024;  //默认一帧1024 Bytes

        private byte[] currentFrameCRC = null;  //当前帧的CRC校验值
        public IAPFileData(int frameByteCount)
        {
            this.currentFrame = new FrameIAP();
            this.config_data = new byte[frameByteCount];
            this.currentFrameCRC = new byte[2];
        }

        /*获取下一帧*/
        public bool loadNextFrame(byte targetAddr, byte srcAddr)
        {
            if (this.config_data == null)
                return false;
            if (this.currentFrameNum == totalFrameNum)
                return false;

            int leftBytesNum = this.config_data.Length - this.currentFrameNum * frameDataSize;  //剩余的字节数
            byte[] rawData = null;      //需要传送的裸数据
            if (leftBytesNum < frameDataSize)       //下一帧的大小不足文本框中设定的一帧的大小
            {
                rawData = new byte[leftBytesNum];
                for (int i = 0; i < leftBytesNum; i++)
                {
                    rawData[i] = this.config_data[currentFrameNum * frameDataSize + i];
                }

            }
            else
            {
                rawData = new byte[frameDataSize];
                this.currentFrame.IAP_frame = new byte[frameDataSize];          //下一帧的大小为文本框中设定的一帧的大小
                for (int i = 0; i < frameDataSize; i++)
                {
                    rawData[i] = this.config_data[currentFrameNum * frameDataSize + i];
                }
            }
            BCPHeader h = new BCPHeader();
            h.Frame_Head = 0x7e;
            h.Protocal_Num = 0x00;
            h.Version_Num = 0x00;
            h.Target_Addr = targetAddr;
            h.Source_Addr = srcAddr;
            h.Port_Num = 0x00;
            h.Control_Code = 0x11;

            h.StartReg_Addr_L = (byte)((this.currentFrameNum + 1) & 0x00ff);
            h.StartReg_Addr_H = (byte)(((this.currentFrameNum + 1) & 0xff00) >> 8);
            this.currentFrame.enterData(h, rawData);   //将数据按照BCP协议写入一帧
            currentFrameNum++; //当前帧数加1
            return true;
        }
        /*重新载入第一帧*/
        public bool reloadFirstFrame(byte targetAddr,byte srcAddr)
        {
            if (this.config_data == null)
                return false;
            setCurrentFrameNum(0);
            if (this.currentFrameNum == totalFrameNum)
                return false;

            int leftBytesNum = this.config_data.Length - this.currentFrameNum * frameDataSize;  //剩余的字节数
            byte[] rawData = null;
            if (leftBytesNum < frameDataSize)
            {
                rawData = new byte[leftBytesNum];
                for (int i = 0; i < leftBytesNum; i++)
                {
                    rawData[i] = this.config_data[currentFrameNum * frameDataSize + i];
                }

            }
            else
            {
                rawData = new byte[frameDataSize];
                this.currentFrame.IAP_frame = new byte[frameDataSize];          //下一帧满2k
                for (int i = 0; i < 1024; i++)
                {
                    rawData[i] = this.config_data[currentFrameNum * frameDataSize + i];
                }
            }

            BCPHeader h = new BCPHeader();
            h.Frame_Head = 0x7e;
            h.Protocal_Num = 0x00;
            h.Version_Num = 0x00;
            h.Target_Addr = targetAddr;
            h.Source_Addr = srcAddr;
            h.Port_Num = 0x00;
            h.Control_Code = 0x11;
            h.StartReg_Addr_L = (byte)((this.currentFrameNum + 1) & 0x0f);
            h.StartReg_Addr_H = (byte)((this.currentFrameNum + 1) & 0xf0);
            this.currentFrame.enterData(h, rawData);
            currentFrameNum++; //当前帧数加1
            return true;

        }

        /*重置
         *当前帧数为0
         */
        public bool frameRST()
        {
            if (this.config_data == null)
                return false;
            this.currentFrame.IAP_frame = null;
            this.currentFrameNum = 0;
            return true;
        }
        /*计算总共多少帧*/
        public bool calculateTotalFrame()
        {
            if (this.config_data == null)
                return false;
            double temp = 0;
            temp = Math.Ceiling((double)this.config_data.Length / frameDataSize);
            this.totalFrameNum = (int)Math.Ceiling(temp);
            return true;
        }
        /*设置当前帧号*/
        public bool setCurrentFrameNum(int frameNum)
        {
            if (frameNum > this.totalFrameNum || frameNum < 0)
                return false;
            this.currentFrameNum = frameNum;
            return true;

        }
        /*返回当前帧号*/
        public int getCurrentFrameNum()
        {
            if (this.config_data == null)
                return 0;
            return this.currentFrameNum;
        }

        public bool sendFrame(SerialPort sp)
        {
            if (this.currentFrame.IAP_frame == null)
                return false;
            this.currentFrame.sp_FrameSend(sp);
            return true;
        }

        /*返回文件一共占用几帧*/
        public int getTotalFrameNum()
        {
            return this.totalFrameNum;
        }

        /*返回是否为最后一帧*/
        public bool isLastFrame()
        {
            if (this.currentFrameNum == this.totalFrameNum)
                return true;
            else
                return false;
        }
        /*设置一帧里数据的大小，即数据包的大小*/
        public void setDataFrameSize(int size)
        {
            this.frameDataSize = size;
            //this.currentFrameNum = 0;  //重新开始从第一帧发送。
            calculateTotalFrame();
        }

        public byte[] getCurrentFrameCRC()
        {
            this.currentFrameCRC = this.currentFrame.getCurrentCRC();
            return this.currentFrameCRC;
        }
    }
}
