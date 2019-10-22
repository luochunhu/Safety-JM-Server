using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Management;

namespace Basic.Framework.Version
{
    /// <summary>
    ///     作者：
    /// 创建时间：2016-8-11
    /// 功能描述：授权管理帮助类
    /// </summary>
    public static class DESHelper
    {

        #region DES
        private static string sKey = "023CQLCH";
        public static string Encrypt(string pToEncrypt)
        {
            //访问数据加密标准(DES)算法的加密服务提供程序 (CSP) 版本的包装对象
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            des.Key = ASCIIEncoding.ASCII.GetBytes(sKey);　//建立加密对象的密钥和偏移量
            des.IV = ASCIIEncoding.ASCII.GetBytes(sKey);　 //原文使用ASCIIEncoding.ASCII方法的GetBytes方法

            byte[] inputByteArray = Encoding.Default.GetBytes(pToEncrypt);//把字符串放到byte数组中

            MemoryStream ms = new MemoryStream();//创建其支持存储区为内存的流　
            //定义将数据流链接到加密转换的流
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            //上面已经完成了把加密后的结果放到内存中去

            StringBuilder ret = new StringBuilder();
            foreach (byte b in ms.ToArray())
            {
                ret.AppendFormat("{0:X2}", b);
            }
            ret.ToString();
            return ret.ToString();
        }

        public static string Decrypt(string pToDecrypt)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();

            byte[] inputByteArray = new byte[pToDecrypt.Length / 2];
            for (int x = 0; x < pToDecrypt.Length / 2; x++)
            {
                int i = (Convert.ToInt32(pToDecrypt.Substring(x * 2, 2), 16));
                inputByteArray[x] = (byte)i;
            }

            des.Key = ASCIIEncoding.ASCII.GetBytes(sKey);　//建立加密对象的密钥和偏移量，此值重要，不能修改
            des.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);

            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();

            //建立StringBuild对象，createDecrypt使用的是流对象，必须把解密后的文本变成流对象
            StringBuilder ret = new StringBuilder();

            return System.Text.Encoding.Default.GetString(ms.ToArray());
        }

        #endregion DES

        #region 机器码相关
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string GetFormatMachineCode()
        {
            string code = GetMachineCode();
            //string formatCode = "";
            //if (code.Length < 35)
            //{
            //    return code;
            //}
            //for (int i = 0; i < code.Length; i++)
            //{
            //    if (i % 5 == 0)
            //    {
            //        formatCode += code[i] + "-";
            //    }
            //    else
            //    {
            //        formatCode += code[i];
            //    }

            //}

            //return formatCode.Substring(2, 35);

            return code;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string GetMachineCode()
        {
            string machineCode = "";
            List<string> macList = new List<string>();
            try
            {
                macList = GetMacAddress();
                machineCode = Newtonsoft.Json.JsonConvert.SerializeObject(macList);

                //machineCode = GetCpuID() + "-" + GetDiskID() + "-" + GetMacAddress();
                //File.WriteAllText("d:\\machine.txt", GetCpuID() + "-" + GetDiskID() + "-" + GetMacAddress());
            }
            catch
            {
                macList.Add(System.Environment.MachineName + "--flysky--flysky");
                machineCode = Newtonsoft.Json.JsonConvert.SerializeObject(macList);
            }
            finally
            {
                //bug : all machine code is machineName..... 
                //machineCode = System.Environment.MachineName + "-";// + System.Environment.UserName;
                //File.WriteAllText("d:\\machine.txt", machineCode);
            }
            return Encrypt(machineCode);
        }

       
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string GetCpuID()
        {
            try
            {
                //获取CPU序列号代码   
                string cpuInfo = "";//cpu序列号   
                ManagementClass mc = new ManagementClass("Win32_Processor");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    cpuInfo = mo.Properties["ProcessorId"].Value.ToString();
                }
                moc = null;
                mc = null;
                return cpuInfo;
            }
            catch
            {
                return "unknow";
            }
            finally
            {
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static List<string> GetMacAddress()
        {
            try
            {
                List<string> macList = new List<string>();

                //获取网卡硬件地址   
                string mac = "";
                ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    if ((bool)mo["IPEnabled"] == true)
                    {
                        // mac = mac + "||"+ mo["MacAddress"].ToString() ;
                        //// break;

                        mac =  mo["MacAddress"].ToString();
                        macList.Add(mac);
                        // break;
                    }
                }
                moc = null;
                mc = null;
                return macList;
            }
            catch
            {
                return new List<string>() { "unknow" };
            }
            finally
            {
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string GetDiskID()
        {
            try
            {
                //获取硬盘ID   
                String HDid = "";
                ManagementClass mc = new ManagementClass("Win32_DiskDrive");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    HDid = (string)mo.Properties["Model"].Value;
                }
                moc = null;
                mc = null;
                return HDid;
            }
            catch
            {
                return "unknow";
            }
            finally
            {
            }
        }
        #endregion 机器码相关
        
    }

    /// <summary>
    /// 授权信息类
    /// </summary>
    public class AuthorizationInfo
    {
        /// <summary>
        /// 加密公匙
        /// </summary>
        public string PublicKey { get; set; }
        /// <summary>
        /// 加密后的注册码
        /// </summary>
        public string RegisterCode { get; set; }
        /// <summary>
        /// 授权产品编码
        /// </summary>
        public string ProductCode { get; set; }
        /// <summary>
        /// 客户信息
        /// </summary>
        public string CustomerInfo { get; set; }
        /// <summary>
        /// 授权模式
        /// 0：试用模式；1：开发模式  2：商用模式；
        /// </summary>
        public int AuthorizeMode { get; set; }
        /// <summary>
        /// 授权有效期
        /// </summary>
        public DateTime EndTime { get; set; }
    }
}
