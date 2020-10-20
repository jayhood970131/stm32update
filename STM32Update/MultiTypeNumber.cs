using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STM32Update
{
    /*
     * MutiTypeNumber类通过输入任何一种类型的数据，转换成另外两种数据：
     * input para：byte类型数据 ex. 0x0e
     * Hex：十六进制形式 ex. "0x0e"
     * Bin：二进制形式 ex. "00010010"
     */
    public class MultiTypeNumber
    {

        public const byte HexToBi = 0x00;
        public const byte STR_BIN = 0x00;
        public const byte STR_HEX = 0x01;
        public const byte STR_HEX_HAS_HEAD = 0x02;  //byte转hex形式的string时是否加上0x
        public const byte STR_HEX_NO_HEAD = 0x03;

        private byte byteNumber = 0;
        private string byteNumber_Bin = "";
        private string byteNumber_Hex = "";

        public MultiTypeNumber()
        {

        }

        /*
         * 设置数据
         */
        public void setByteNumber(byte num,byte isHasHead)
        {
            this.byteNumber = num;

            //byte转成16进制显示
            string str_H = "";
            string str_L = "";

            int temp = 0;

            temp = num & 0xf0;  //高四位
            switch (temp)
            {
                case 0x00:
                    str_H = "0";
                    break;
                case 0x10:
                    str_H = "1";
                    break;
                case 0x20:
                    str_H = "2";
                    break;
                case 0x30:
                    str_H = "3";
                    break;
                case 0x40:
                    str_H = "4";
                    break;
                case 0x50:
                    str_H = "5";
                    break;
                case 0x60:
                    str_H = "6";
                    break;
                case 0x70:
                    str_H = "7";
                    break;
                case 0x80:
                    str_H = "8";
                    break;
                case 0x90:
                    str_H = "9";
                    break;
                case 0xa0:
                    str_H = "a";
                    break;
                case 0xb0:
                    str_H = "b";
                    break;
                case 0xc0:
                    str_H = "c";
                    break;
                case 0xd0:
                    str_H = "d";
                    break;
                case 0xe0:
                    str_H = "e";
                    break;
                case 0xf0:
                    str_H = "f";
                    break;
                default:
                    str_H = "err";
                    break;
            }

            temp = num & 0x0f;  //低四位
            switch (temp)
            {
                case 0x00:
                    str_L = "0";
                    break;
                case 0x01:
                    str_L = "1";
                    break;
                case 0x02:
                    str_L = "2";
                    break;
                case 0x03:
                    str_L = "3";
                    break;
                case 0x04:
                    str_L = "4";
                    break;
                case 0x05:
                    str_L = "5";
                    break;
                case 0x06:
                    str_L = "6";
                    break;
                case 0x07:
                    str_L = "7";
                    break;
                case 0x08:
                    str_L = "8";
                    break;
                case 0x09:
                    str_L = "9";
                    break;
                case 0x0a:
                    str_L = "a";
                    break;
                case 0x0b:
                    str_L = "b";
                    break;
                case 0x0c:
                    str_L = "c";
                    break;
                case 0x0d:
                    str_L = "d";
                    break;
                case 0x0e:
                    str_L = "e";
                    break;
                case 0x0f:
                    str_L = "f";
                    break;
                default:
                    str_L = "or";
                    break;
            }

            if (isHasHead == MultiTypeNumber.STR_HEX_HAS_HEAD)
                this.byteNumber_Hex = "0x" + str_H + str_L;
            else
                this.byteNumber_Hex = str_H + str_L;
            
        }
        /*
         * 输入一个string类型显示的十六进制数或二进制数，并转换为其他两种类型
         */
        public void setByteNumber(string num_str,byte str_Type)
        {
            byteNumber = 0x00;
            if (str_Type == MultiTypeNumber.STR_HEX)   //字符串代表的是十六进制数
            {
                byte[] temp = System.Text.Encoding.ASCII.GetBytes(num_str.Substring(0, 1));  //高四位
                switch (temp[0])
                {
                    case 48:
                        byteNumber |= 0x00; //0
                        break;
                    case 49:
                        byteNumber |= 0x10; //1
                        break;
                    case 50:
                        byteNumber |= 0x20;
                        break;
                    case 51:
                        byteNumber |= 0x30;
                        break;
                    case 52:
                        byteNumber |= 0x40;
                        break;
                    case 53:
                        byteNumber |= 0x50;
                        break;
                    case 54:
                        byteNumber |= 0x60;
                        break;
                    case 55:
                        byteNumber |= 0x70;
                        break;
                    case 56:
                        byteNumber |= 0x80;
                        break;
                    case 57:
                        byteNumber |= 0x90; //9
                        break;
                    case 65:                   //A
                    case 97:                   //a
                        byteNumber |= 0xa0;
                        break;
                    case 66:
                    case 98:
                        byteNumber |= 0xb0;
                        break;
                    case 67:
                    case 99:
                        byteNumber |= 0xc0;
                        break;
                    case 68:
                    case 100:
                        byteNumber |= 0xd0;
                        break;
                    case 69:
                    case 101:
                        byteNumber |= 0xe0;
                        break;
                    case 70:                  //F
                    case 102:                 //f
                        byteNumber |= 0xf0;
                        break;
                    default:
                        break;

                }

                temp = System.Text.Encoding.ASCII.GetBytes(num_str.Substring(1, 1));  //低四位
                switch (temp[0])
                {
                    case 48:
                        byteNumber |= 0x00; //0
                        break;
                    case 49:
                        byteNumber |= 0x01; //1
                        break;
                    case 50:
                        byteNumber |= 0x02;
                        break;
                    case 51:
                        byteNumber |= 0x03;
                        break;
                    case 52:
                        byteNumber |= 0x04;
                        break;
                    case 53:
                        byteNumber |= 0x05;
                        break;
                    case 54:
                        byteNumber |= 0x06;
                        break;
                    case 55:
                        byteNumber |= 0x07;
                        break;
                    case 56:
                        byteNumber |= 0x08;
                        break;
                    case 57:
                        byteNumber |= 0x09; //9
                        break;
                    case 65:                   //A
                    case 97:                   //a
                        byteNumber |= 0x0a;
                        break;
                    case 66:
                    case 98:
                        byteNumber |= 0x0b;
                        break;
                    case 67:
                    case 99:
                        byteNumber |= 0x0c;
                        break;
                    case 68:
                    case 100:
                        byteNumber |= 0x0d;
                        break;
                    case 69:
                    case 101:
                        byteNumber |= 0x0e;
                        break;
                    case 70:                  //F
                    case 102:                 //f
                        byteNumber |= 0x0f;
                        break;
                    default:
                        break;

                }
            }
            else
            {

            }
        }
        /*
         * 返回十六进制形式显示的数据
         */
        public string getByteNumber_Hex()   
        {
            return this.byteNumber_Hex;
        }
        /*
         * 返回byte形式存储的数据
         */
        public byte getByteNumber_Byte()
        {
            return this.byteNumber;
        }

    }

     
}
