using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Utilities
{
    public class FileIO
    {
        public static void fun()        {

            ////获取模块的完整路径。
            //string path1 = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
            ////获取和设置当前目录(该进程从中启动的目录)的完全限定目录
            //string path2 = System.Environment.CurrentDirectory;
            ////获取应用程序的当前工作目录
            //string path3 = System.IO.Directory.GetCurrentDirectory();
            ////获取程序的基目录
            //string path4 = System.AppDomain.CurrentDomain.BaseDirectory;
            ////获取和设置包括该应用程序的目录的名称
            //string path5 = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase;

        }
        public static string ReadTextFromFile(string path)
        {
            FileStream fs = new FileStream(path, FileMode.Open);
            byte[] data = new byte[fs.Length];
            fs.Read(data, 0, data.Length);
            fs.Close();

            Decoder decoder = Encoding.UTF8.GetDecoder();
            int count = decoder.GetCharCount(data, 0, data.Length);
            char[] charData = new char[count];
            decoder.GetChars(data, 0, data.Length, charData, 0);

            string txt = charData.ToString();
            return txt;
        }

        public static object ReadFileToMemory(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return null;
            }

            string str = File.ReadAllText(filePath).Trim();
            IFormatter formatter = new BinaryFormatter();
            byte[] byt = Convert.FromBase64String(str);

            if (byt.Length == 0)
                return null;

            object obj = null;
            using (Stream stream = new MemoryStream(byt, 0, byt.Length))
            {
                obj = formatter.Deserialize(stream);
            }

            return obj;

        }


        /// <summary>
        /// 把字典写入记事本文件中
        /// </summary>
        /// <param name="filePath">记事本文件的全路径</param>
        /// <param name="contents">要写入的内容</param>
        public static void SaveMemoryToFile(string filePath, object contents)
        {
            if (contents == null)
            {
                return;
            }
            if(string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentNullException("保存学习历史的文件路径为空！");
            }

            if (!File.Exists(filePath))
            {
                FileStream fs = File.Create(filePath);
                fs.Close();
            }

            IFormatter formatter = new BinaryFormatter();
            string result = string.Empty;
            using (MemoryStream stream = new MemoryStream())
            {
                formatter.Serialize(stream, contents);
                byte[] byt = new byte[stream.Length];
                byt = stream.ToArray();
                result = Convert.ToBase64String(byt);
                stream.Flush();
            }
            if (result != null && result.Length > 0)
            {
                File.WriteAllText(filePath, result);
            }

        }


        ///<summary>
                ///将字符串生转化为固定长度左对齐，右补空格
                ///</summary>
                ///<paramname="strTemp"></param>需要补齐的字符串
                ///<paramname="length"></param>补齐后的长度
                ///<returns></returns>
        public static string AlignmentStrFunc(string strTemp, int length)
        {
            byte[] byteStr = System.Text.Encoding.Default.GetBytes(strTemp.Trim());

            int iLength = byteStr.Length;
            int iNeed = length - iLength;

            byte[] spaceLen = Encoding.Default.GetBytes(" "); //一个空格的长度
            iNeed = iNeed / spaceLen.Length;

            string spaceString = SpaceStrFunc(iNeed);
            return strTemp + spaceString;
        }
        public static string SpaceStrFunc(int length)
        {
            string strReturn = string.Empty;

            if (length > 0)
            {

                for (int i = 0; i < length; i++)
                {
                    strReturn += " ";
                }
            }
            return strReturn;
        }

    }
}
