using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STM32Update
{
    public class BCPHeader
    {
        public byte Frame_Head = 0;
        public byte Protocal_Num = 0;
        public byte Version_Num = 0;
        public byte Target_Addr = 0;
        public byte Source_Addr = 0;
        public byte Port_Num = 0;
        public byte Control_Code = 0;
        public byte StartReg_Addr_L = 0;
        public byte StartReg_Addr_H = 0;
        
        public void UseDefaultHeader()
        {
            this.Frame_Head = 0x7e;
            this.Protocal_Num = 0x00;
            this.Version_Num = 0x00;
        }
    }

 
}
