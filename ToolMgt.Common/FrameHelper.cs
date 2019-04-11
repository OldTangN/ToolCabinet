using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolMgt.Common
{
    /*
     缓冲区工具类
     */
    public class FrameHelper
    {
      
        /// <summary>
        /// byte[]转成string字符串，空格分隔
        /// </summary>
        /// <param name="inbyte"></param>
        /// <returns></returns>
        public static string bytetostr(byte[] inbyte)  //byte转为string
        {
            string temp = "";

            if (inbyte != null)
            {
                foreach (byte b in inbyte)
                    temp += b.ToString("X2") + " ";
            }

            return temp;
        }

        /// <summary>
        /// byte[]转成string字符串，空格分隔
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string ToHexString(byte[] bytes) // 整型16进制转换成 16进制的string
        {
            string hexString = string.Empty;
            if (bytes != null)
            {
                StringBuilder strB = new StringBuilder();

                for (int i = 0; i < bytes.Length; i++)
                {
                    strB.Append(bytes[i].ToString("X2") + " ");
                }
                hexString = strB.ToString();
            }
            return hexString.Trim();
        }

        /// <summary>
        /// byte[]转成string字符串，无分隔
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string ToHexString_1(byte[] bytes) // 整型16进制转换成 16进制的string
        {
            string hexString = string.Empty;
            if (bytes != null)
            {
                StringBuilder strB = new StringBuilder();

                for (int i = 0; i < bytes.Length; i++)
                {
                    strB.Append(bytes[i].ToString("X2")); //different
                }
                hexString = strB.ToString();
            }
            return hexString;
        }


        public static byte[] StrToHexByte(string hexString)    //16进制字符串转byte
        {
            if (hexString == null)
                return null;
            hexString = hexString.Replace(" ", "");   //去掉空格


            double dlen = Math.Ceiling((double)hexString.Length / 2);          //防止奇数位不能整除
            int len = (int)dlen;
            byte[] returnBytes = new byte[len];
            try
            {
                for (int i = 0; i < returnBytes.Length; i++)
                    returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            }
            catch
            {

            }
            return returnBytes;
        }

        public static int HexStringToint(string HexString) // 16进制字符串转为int数值
        {
            int result = 0;

            string[] temp = HexString.Trim().Split(' ');

            result = Int32.Parse(temp[0], System.Globalization.NumberStyles.HexNumber) * 256 + Int32.Parse(temp[1], System.Globalization.NumberStyles.HexNumber);
            //result = Convert.ToInt16(temp[0]) *256+ Convert.ToInt16(temp[1]);
            return result;
        }
        public static int HexString2Int(string HexString)  //16进制字符串转int："8E2"-->2274
        {
            if (string.IsNullOrEmpty(HexString))
                return -1;
            int num = Int32.Parse(HexString, System.Globalization.NumberStyles.HexNumber);
            return num;
        }
        public static string DecToHex(string x)  //十进制字符转换为16进制字符
        {
            string Result = "";
            if (x == "")
            {
                return Result;
            }
            int DataValue = int.Parse(x);
            Result = DataValue.ToString("X8");
            return Result;
        }

        /// <summary>
        /// 获取2进制
        /// </summary>
        /// <param name="x">字符串</param>
        /// <param name="len">字节长度</param>
        /// <returns></returns>
        public static string HexToBin(string x, int len)
        {
            string Result = "";
            x = x.Replace(" ", "");
            if (x == "")
            {
                return Result;
            }
            int Datain = Convert.ToInt32(x, 16);
            Result = System.Convert.ToString(Datain, 2);
            while (Result.Length < len * 8)
            {
                Result = "0" + Result;
            }

            return Result;
        }
        /// <summary>
        /// 二进制字符串转int
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int BinToInt(string str)
        {

            int strToint = Convert.ToInt32(str, 2);
            return strToint;

        }
    }
}
