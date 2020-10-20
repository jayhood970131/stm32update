using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.IO;
namespace STM32Update
{
    public class FrameIAP
    {
        
        public byte[] IAP_frame = null;  //一帧的数据

        private byte[] currentCRC = null; //当前帧的CRC校验值，下标0号为16位CRC校验码低8位，1号为高8位，默认值为0

        public const byte DATA_FRAME = 0x00;
        public const byte COMMAND_FRAME = 0x01;

        
        /*
         */
        public FrameIAP()
        {
          this.currentCRC = new byte[2];
          this.currentCRC[0] = 0x00;
          this.currentCRC[1] = 0x00;
        }

        /*
         * rawData 数据
         * 输入数据并装成一帧
         */
        public bool enterData(BCPHeader h,byte[] rawData)
        {
        
            BCP_FrameHandler bcp_frame = new BCP_FrameHandler(h,rawData);
            bcp_frame.getBCP_Frame(ref this.IAP_frame);  //将结果复制出来
            this.currentCRC[0] = this.IAP_frame[this.IAP_frame.Length - 2];
            this.currentCRC[1] = this.IAP_frame[this.IAP_frame.Length - 1];
            return true;
        }

        public bool enterData(BCPHeader h,byte length_H,byte length_L)
        {
            BCP_FrameHandler bcp_frame = new BCP_FrameHandler(h, length_H,length_L);
            bcp_frame.getBCP_Frame(ref this.IAP_frame);  //将结果复制出来
            this.currentCRC[0] = this.IAP_frame[this.IAP_frame.Length - 2];
            this.currentCRC[1] = this.IAP_frame[this.IAP_frame.Length - 1];
            return true;
        }
   
        public bool sp_FrameSend(SerialPort sp)
        {
            if (sp == null)
                return false;
            if (this.IAP_frame == null)
                return false;

            sp.Write(this.IAP_frame, 0,this.IAP_frame.Length); //数据写入缓冲
            return true;
        }

        /*获取int的最低一个字节*/
        private byte getIntLowByte(int targetInt)
        {

            return (byte)targetInt;
        }

        public byte[] getCurrentCRC()
        {
            return this.currentCRC;
        }
    }
}
