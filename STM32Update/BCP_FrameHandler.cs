using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STM32Update
{
    public class BCP_FrameHandler
    {
        private byte[] bcp_ProcessedFrame = null;

        public BCP_FrameHandler(BCPHeader head,byte[] data)  //数据长度自动计算
        {
            CRC16 crc6_modbus = new CRC16();
            if (data == null)  //不携带数据
            {
                this.bcp_ProcessedFrame = new byte[14];
                this.bcp_ProcessedFrame[0] = head.Frame_Head;
                this.bcp_ProcessedFrame[1] = head.Protocal_Num;
                this.bcp_ProcessedFrame[2] = head.Version_Num;
                this.bcp_ProcessedFrame[3] = head.Target_Addr;
                this.bcp_ProcessedFrame[4] = head.Source_Addr;
                this.bcp_ProcessedFrame[5] = head.Port_Num;
                this.bcp_ProcessedFrame[6] = head.Control_Code;
                this.bcp_ProcessedFrame[7] = 0x00;           //保留
                this.bcp_ProcessedFrame[8] = head.StartReg_Addr_L;
                this.bcp_ProcessedFrame[9] = head.StartReg_Addr_H;

                this.bcp_ProcessedFrame[10] = 0x00;      //数据长度为0
                this.bcp_ProcessedFrame[11] = 0x00;

                byte[] arrayForCRC = new byte[this.bcp_ProcessedFrame.Length - 2]; //不包括 CRC_L，CRC_H
                for (int i = 0; i < arrayForCRC.Length; i++)
                {
                    arrayForCRC[i] = this.bcp_ProcessedFrame[i];
                }
                crc6_modbus.CRC16_modbus(arrayForCRC, arrayForCRC.Length);  //计算出CRC校验码

                this.bcp_ProcessedFrame[12] = crc6_modbus.getCRC16_L();
                this.bcp_ProcessedFrame[13] = crc6_modbus.getCRC16_H();

            }
            else
            {
                this.bcp_ProcessedFrame = new byte[14 + data.Length];
                this.bcp_ProcessedFrame[0] = head.Frame_Head;
                this.bcp_ProcessedFrame[1] = head.Protocal_Num;
                this.bcp_ProcessedFrame[2] = head.Version_Num;
                this.bcp_ProcessedFrame[3] = head.Target_Addr;
                this.bcp_ProcessedFrame[4] = head.Source_Addr;
                this.bcp_ProcessedFrame[5] = head.Port_Num;
                this.bcp_ProcessedFrame[6] = head.Control_Code;
                this.bcp_ProcessedFrame[7] = 0x00;           //保留
                this.bcp_ProcessedFrame[8] = head.StartReg_Addr_L;
                this.bcp_ProcessedFrame[9] = head.StartReg_Addr_H;

                int data_length = data.Length;
                this.bcp_ProcessedFrame[10] = (byte)(data_length);  //数据长度L
                this.bcp_ProcessedFrame[11] = (byte)(data_length >> 8); //数据长度H

                for (int i = 0; i < data.Length; i++)        //载入数据
                {
                    this.bcp_ProcessedFrame[12 + i] = data[i];
                }

                byte[] arrayForCRC = new byte[this.bcp_ProcessedFrame.Length - 2]; //不包括 CRC_L，CRC_H
                for (int i = 0; i < arrayForCRC.Length; i++)
                {
                    arrayForCRC[i] = this.bcp_ProcessedFrame[i];
                }
                crc6_modbus.CRC16_modbus(arrayForCRC, arrayForCRC.Length);  //计算出CRC校验码
                this.bcp_ProcessedFrame[this.bcp_ProcessedFrame.Length - 2] = crc6_modbus.getCRC16_L();
                this.bcp_ProcessedFrame[this.bcp_ProcessedFrame.Length - 1] = crc6_modbus.getCRC16_H();
            }

        }

        public BCP_FrameHandler(BCPHeader head,byte length_H,byte length_L)  //无数据，但数据长度由用户指定
        {
            CRC16 crc6_modbus = new CRC16();
            this.bcp_ProcessedFrame = new byte[14];
            this.bcp_ProcessedFrame[0] = head.Frame_Head;
            this.bcp_ProcessedFrame[1] = head.Protocal_Num;
            this.bcp_ProcessedFrame[2] = head.Version_Num;
            this.bcp_ProcessedFrame[3] = head.Target_Addr;
            this.bcp_ProcessedFrame[4] = head.Source_Addr;
            this.bcp_ProcessedFrame[5] = head.Port_Num;
            this.bcp_ProcessedFrame[6] = head.Control_Code;
            this.bcp_ProcessedFrame[7] = 0x00;           //保留
            this.bcp_ProcessedFrame[8] = head.StartReg_Addr_L;
            this.bcp_ProcessedFrame[9] = head.StartReg_Addr_H;

            this.bcp_ProcessedFrame[10] = length_L;      //数据长度由用户指定
            this.bcp_ProcessedFrame[11] = length_H;


            byte[] arrayForCRC = new byte[this.bcp_ProcessedFrame.Length - 2]; //不包括 CRC_L，CRC_H
            for (int i = 0; i < arrayForCRC.Length; i++)
            {
                arrayForCRC[i] = this.bcp_ProcessedFrame[i];
            }
            crc6_modbus.CRC16_modbus(arrayForCRC, arrayForCRC.Length);  //计算出CRC校验码
            this.bcp_ProcessedFrame[this.bcp_ProcessedFrame.Length - 2] = crc6_modbus.getCRC16_L();
            this.bcp_ProcessedFrame[this.bcp_ProcessedFrame.Length - 1] = crc6_modbus.getCRC16_H();
        }

        public void getBCP_Frame(ref byte[] rx_array)
        {
            if(this.bcp_ProcessedFrame==null)
            {
                rx_array = null;
                return;
            }
               
            rx_array = new byte[this.bcp_ProcessedFrame.Length];
            for(int i=0;i<rx_array.Length;i++)  //将结果复制出去
            {
                rx_array[i] = this.bcp_ProcessedFrame[i];
            }
        }

    }
    


    
}
