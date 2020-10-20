using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
namespace STM32Update
{

    public class FileHandler
    {
        protected string cfg_filepath = "";  //文件路径
        protected string current_line;  //当前行
        protected OpenFileDialog opfile;
        protected bool file_selected = false;  //是否已选择文件

        private bool file_existed = false;
        //private byte[] config_data = null;    
        private int fileByteCounts = 0;
        private int fileType;
        public IAPFileData fileData = null;
        public const byte FILE_BIN = 0x01;      //文件类型
        public const byte FILE_HEX = 0x02;

        public const byte FILE_NOT_SELECTED = 0;
        public const byte FILE_SELECTED = 1;
        public const byte FILE_FORMAT_ILLEGAL = 2;
        private List<string> hexString;         //存放读取到的配置文件的每一个字符串 1.地址 2.寄存器名称 3.寄存器值 4.寄存器注释
         /*通过对话框选择配置文件*/
        public FileHandler(byte type)
        {
            this.fileType = type;
            opfile = new OpenFileDialog();
            
            switch(type)
            {
                case FileHandler.FILE_BIN:
                    opfile.Filter = "二进制文件(*.bin)|*.bin";
                    break;
                case FileHandler.FILE_HEX:
                    {
                        opfile.Filter = "文本文件(*.txt)|*.txt";
                        this.hexString = new List<string>();
                    }
                    break;
                default:
                    opfile.Filter = "二进制文件(*.bin)|*.bin";
                    break;

            }

            //opfile.Filter = "二进制文件(*.bin)|*.bin";
            if (opfile.ShowDialog() == DialogResult.OK)
            {
                string str = System.IO.Path.GetFullPath(opfile.FileName);    //读当前文件绝对路径
                this.file_selected = true;
                this.cfg_filepath = str;
            }
            else
            {
                this.file_selected = false;
                this.file_existed = false;
            }
            //if (this.file_selected == true)
            //{
            //    MessageBox.Show("已选择" + this.get_cfg_filename(), "选择完毕");

            //}

        }
        /*读取配置文件
         * frameSize指定一帧数据的大小
         */
        public byte read_configFile()
        {
            switch(this.fileType)
            {
                case FileHandler.FILE_BIN:
                    {
                        try
                        {
                            if (this.file_selected == true)
                            {
                                /*计算二进制文件有多少个Bytes*/
                                BinaryReader br = new BinaryReader(new FileStream(this.cfg_filepath, FileMode.Open));
                                fileByteCounts = 0;  //计数文件中有多少个byte

                                if (!(br.BaseStream.Position < br.BaseStream.Length))
                                {
                                    fileByteCounts = 0;
                                    return FILE_NOT_SELECTED;
                                }

                                while (br.BaseStream.Position < br.BaseStream.Length)  //未读完数据
                                {
                                    br.ReadByte();
                                    fileByteCounts++;
                                }
                                br.Close();

                                /*使用FileData类，将文件读取，以分帧发送的形式处理数据*/
                                this.fileData = new IAPFileData(fileByteCounts);
                                br = new BinaryReader(new FileStream(this.cfg_filepath, FileMode.Open));
                                for (int i = 0; i < fileByteCounts; i++)      //一次性读入所有数据
                                {
                                    this.fileData.setElementByIndex(i, br.ReadByte());
                                }
                                br.Close();
                                return FILE_SELECTED;
                            }
                            else
                                return FILE_NOT_SELECTED;   //没有选择文件
                        }
                        catch (Exception e)
                        {
                            MessageBox.Show("配置文件格式错误！", "ERROR");
                        }
                    }
                    break;
                case FileHandler.FILE_HEX:
                    {
                        try
                        {
                            if (this.file_selected == true)
                            {
                                //StreamReader sr = File.OpenText(this.cfg_filepath);
                                StreamReader sr = new StreamReader(this.cfg_filepath, System.Text.Encoding.GetEncoding("utf-8"));
                                String line;

                                while ((line = sr.ReadLine()) != null)  //未读完数据
                                {
                                    string str =new Regex("[\\s]+").Replace(line, " "); //正则表达式
                                    string[] s = str.Split();
                                    if (line.Equals("")) //数据不为空行
                                    {
                                        continue;
                                    }

                                    if (s[0].Length % 2 != 0)       //若十六进制数值没有写0，补上一个0
                                        s[0] = "0x0" + s[0].Substring(2, 1);

                                    if (s[2].Length % 2 != 0)        //若十六进制数值没有写0，补上一个0 寄存器值最多32位
                                        s[2] = "0x0" + s[2].Substring(2, s[2].Length-2);

                                    this.hexString.Add(s[0]);
                                    this.hexString.Add(s[1]);
                                    this.hexString.Add(s[2]);

                                    if (s.Length == 4)      //检查是否有注释
                                        this.hexString.Add(s[3]);  
                                    else
                                        this.hexString.Add("");
                                }

                                sr.Close();
                                return FILE_SELECTED;
                            }
                            else
                            {
                                return FILE_NOT_SELECTED;
                            }
                        }catch(Exception e)
                        {
                            MessageBox.Show("读取配置文件时出错，请检查配置文件格式。", "ERROR");
                        }
                        

                    }
                    break;
                default:
                    return FILE_NOT_SELECTED;
            }
            
            return FILE_SELECTED;
        }

        /*返回配置文件地址*/
        public string get_cfg_filename()
        {
            return this.cfg_filepath;
        }

        /*返回数据数组*/
        public void get_configData(byte[] configData)
        {
           for(int i=0;i<this.fileData.getDataLength();i++)
           {
               configData[i] = this.fileData.getElementByIndex(i);
           }
        }

        /*返回文件字节个数*/
        public int get_fileByteCounts()
        {
            return this.fileByteCounts;
        }


        public bool isFileSelected()
        {
            if (this.file_selected == true)
                return true;
            else
                return false;
        }
        
        public List<string> getHexString()
        {
            return this.hexString;
        }
    }
}
