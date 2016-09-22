using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace CommonTools
{
    /// <summary>
    /// 加密 现用于cookie加解密
    /// </summary>
    public class EncryptHelper
    {
        /// <summary>
        /// 系统秘钥
        /// </summary>
        private const string MySecretKey = "@#$%^&)*_+-&#$!";

        #region 加密&解密

        /// <summary>
        /// DES对称加密方法
        /// </summary>
        /// <param name="toEncrypt">原始待加密数据</param>
        /// <param name="secretKey">加密密钥</param>
        public static string EncryptData(string toEncrypt, string secretKey="")
        {
            try
            {
                string newSecretKey = Md5Encrpt(secretKey + MySecretKey).Substring(16, 8);
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                //把字符串放到byte数组中    
                Byte[] inputByteArray = Encoding.Default.GetBytes(toEncrypt);
                //建立加密对象的密钥和偏移量
                des.Key = ASCIIEncoding.ASCII.GetBytes(newSecretKey);
                //原文使用ASCIIEncoding.ASCII方法的GetBytes方法 
                des.IV = ASCIIEncoding.ASCII.GetBytes(newSecretKey);
                //使得输入密码必须输入英文文本
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                StringBuilder ret = new StringBuilder();
                foreach (Byte b in ms.ToArray())
                {
                    ret.AppendFormat("{0:X2}", b);
                }
                return ret.ToString();
            }
            catch
            {
                return "";
            }
        }


        /// <summary>
        /// DES对称解密方法
        /// </summary>
        /// <param name="toDecrypt">待解密数据</param>
        /// <param name="secretKey">解密密钥</param>
        public static string DecryptData(string toDecrypt, string secretKey="")
        {
            try
            {
                string newSecretKey = Md5Encrpt(secretKey + MySecretKey).Substring(16, 8);
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                Byte[] inputByteArray = new byte[toDecrypt.Length/2];
                for (int x = 0; x < toDecrypt.Length/2; x++)
                {
                    int i = (Convert.ToInt32(toDecrypt.Substring(x*2, 2), 16));
                    inputByteArray[x] = (byte) i;
                }
                //建立加密对象的密钥和偏移量，此值重要，不能修改 
                des.Key = ASCIIEncoding.ASCII.GetBytes(newSecretKey);
                des.IV = ASCIIEncoding.ASCII.GetBytes(newSecretKey);
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                return System.Text.Encoding.Default.GetString(ms.ToArray());
            }
            catch
            {
                return "";
            }
        }


        private static string Md5Encrpt(string text)
        {
            StringBuilder sb = new StringBuilder(32);
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] t = md5.ComputeHash(Encoding.Unicode.GetBytes(text));
            for (int i = 0; i < t.Length; i++)
            {
                sb.Append(t[i].ToString("x").PadLeft(2, '0'));
            }

            return sb.ToString();
        }

        #endregion
    }
}