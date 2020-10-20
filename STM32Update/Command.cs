using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STM32Update
{
    public class Command
    {
        public const byte CTRLCODE_READ_REGISTER = 0x00;
        public const byte CTRLCODE_WRITE_REGISTER = 0x01;
        public const byte CTRLCODE_SET_REGISTER = 0x02;
        public const byte CTRLCODE_RST_REGISTER = 0x03;
        public const byte CTRLCODE_BYTE_STREAM = 0x10;
        public const byte CTRLCODE_IAP_DATA = 0x11;

        public const byte IAP_CTRLCODE_RESEND = 0x00;
        public const byte IAP_CTRLCODE_CONFIRM = 0xff;
        public const byte IAP_CTRLCODE_TOTALFRAMENUM = 0x01;
    }
}
