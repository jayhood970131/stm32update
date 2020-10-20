using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text;
using System.Windows.Forms;
namespace STM32Update
{
    /*
     * 寄存器记录类
     * 功能：一个寄存名对应一个寄存器地址、一个寄存器值
     *      故一个寄存器名（string）对应一个寄存器地址（byte）和一个寄存器值文本框
     */
    public class RegRecord
    {
        public string regName;          //寄存器名
        public byte[] regValArr;        //寄存器值--按字节分成数据存储
        public uint regVal;             //寄存器值 -- 最多32位
        public byte regAddr;            //寄存器地址
        public string regComment;       //注释
        public TextBox tbxHexVal;       //对应寄存器值输入文本框(HEX)
        public TextBox tbxDecVal;       //对应寄存器值输入文本框(DEC)
    }
}
