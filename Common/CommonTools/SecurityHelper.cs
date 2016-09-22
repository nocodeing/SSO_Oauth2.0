using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace CommonTools
{
    /// <summary>
    /// 加密解密等安全帮助类
    /// </summary>
    public class SecurityHelper
    {

        private readonly SymmetricAlgorithm _mobjCryptoService;
        private readonly string _key;
        private const string DefaultKey = "Guz(%&hj7x89H$yuBI0456FtmaT5&fvHUFCy76*h%(HilJ$lhj!y6&(*jkP87jH7";

        /// <summary>    
        /// 对称加密类的构造函数    
        /// </summary>    
        public SecurityHelper()
            : this(DefaultKey)
        {
        }
        public SecurityHelper(string key)
        {
            _mobjCryptoService = new RijndaelManaged();
            _key = GetMD5_32(key, "GB2312");
        }

        /// <summary>    
        /// 获得密钥    
        /// </summary>    
        /// <returns>密钥</returns>    
        private byte[] GetLegalKey()
        {
            var sTemp = _key;
            _mobjCryptoService.GenerateKey();
            var bytTemp = _mobjCryptoService.Key;
            var keyLength = bytTemp.Length;
            if (sTemp.Length > keyLength)
                sTemp = sTemp.Substring(0, keyLength);
            else if (sTemp.Length < keyLength)
                sTemp = sTemp.PadRight(keyLength, ' ');
            return Encoding.ASCII.GetBytes(sTemp);
        }

        /// <summary>    
        /// 获得初始向量IV    
        /// </summary>    
        /// <returns>初试向量IV</returns>    
        private byte[] GetLegalIv()
        {
            string sTemp = "ADSFD$#&^*&(*%$@#!@%$*)(_)+HGCXDSFHGD:{PO>:FDDSDVCDSSDVC&";
            _mobjCryptoService.GenerateIV();
            var bytTemp = _mobjCryptoService.IV;
            var ivLength = bytTemp.Length;
            if (sTemp.Length > ivLength)
                sTemp = sTemp.Substring(0, ivLength);
            else if (sTemp.Length < ivLength)
                sTemp = sTemp.PadRight(ivLength, ' ');
            return Encoding.ASCII.GetBytes(sTemp);
        }
        /// <summary>    
        /// 加密方法    
        /// </summary>    
        /// <param name="source">待加密的串</param>    
        /// <returns>经过加密的串</returns>    
        public string Encrypto(string source)
        {
            var bytIn = Encoding.UTF8.GetBytes(source);
            var ms = new MemoryStream();
            _mobjCryptoService.Key = GetLegalKey();
            _mobjCryptoService.IV = GetLegalIv();
            var encrypto = _mobjCryptoService.CreateEncryptor();
            var cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Write);
            cs.Write(bytIn, 0, bytIn.Length);
            cs.FlushFinalBlock();
            ms.Close();
            var bytOut = ms.ToArray();
            return Convert.ToBase64String(bytOut);
        }

        /// <summary>    
        /// 解密方法    
        /// </summary>    
        /// <param name="source">待解密的串</param>    
        /// <returns>经过解密的串</returns>    
        public string Decrypto(string source)
        {
            var bytIn = Convert.FromBase64String(source);
            var ms = new MemoryStream(bytIn, 0, bytIn.Length);
            _mobjCryptoService.Key = GetLegalKey();
            _mobjCryptoService.IV = GetLegalIv();
            var encrypto = _mobjCryptoService.CreateDecryptor();
            var cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Read);
            var sr = new StreamReader(cs);
            var result = sr.ReadToEnd();
            ms.Close();
            return result;
        }


        public string GetMD5_32(string s, string inputCharset)
        {
            var md5 = new MD5CryptoServiceProvider();
            var t = md5.ComputeHash(Encoding.GetEncoding(inputCharset).GetBytes(s));
            var sb = new StringBuilder(32);
            foreach (var t1 in t)
            {
                sb.Append(t1.ToString("x").PadLeft(2, '0'));
            }
            return sb.ToString();
        }
    }
}
