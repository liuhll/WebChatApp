using System;
using System.Security.Cryptography;
using System.Text;

namespace Jeuci.WeChatApp.Common.Tools
{
    public static class EncryptionHelper
    {
        //public static string GetUserSignByHash256(string userName, string passwordStrSha256)
        //{
        //    var privateSha256 = EncryptSHA256(userName + passwordStrSha256);
        //    return privateSha256;
        //}

        #region  哈希  签名


        /// <summary>
        /// SHA256哈希
        /// </summary>
        /// <param name="instr">要加密的字符串</param>
        /// <returns>64位大写SHA256哈希值</returns>
        public static string EncryptSHA256(string instr)
        {
            return EncryptSHA256(instr, Encoding.UTF8);
        }

        /// <summary>
        /// SHA256哈希
        /// </summary>
        /// <param name="instr">要加密的字符串</param>
        /// <param name="bytesEncoding">编码</param>
        /// <returns>64位大写SHA256哈希值</returns>
        public static string EncryptSHA256(string instr, Encoding bytesEncoding)
        {
            byte[] toByte = EncryptSHA256ToBytes(instr, bytesEncoding);
            string result = BitConverter.ToString(toByte).ToUpper().Replace("-", "");
            return result;
        }

        /// <summary>
        /// SHA256哈希
        /// </summary>
        /// <param name="instr">要加密的字符串</param>
        /// <param name="bytesEncoding">编码</param>
        /// <returns>32长度的字节数组</returns>
        public static byte[] EncryptSHA256ToBytes(string instr, Encoding bytesEncoding)
        {
            byte[] toByte = bytesEncoding.GetBytes(instr);
            using (SHA256Managed sha256 = new SHA256Managed())
            {
                toByte = sha256.ComputeHash(toByte);
                return toByte;
            }
        }

        /// <summary>
        /// SHA哈希
        /// </summary>
        /// <param name="instr">要加密的字符串</param>
        /// <returns>40位大写SHA哈希值</returns>
        public static string EncryptSHA(string instr)
        {
            return EncryptSHA(instr, Encoding.UTF8);
        }

        /// <summary>
        /// SHA哈希
        /// </summary>
        /// <param name="instr">要加密的字符串</param>
        /// <param name="bytesEncoding">编码</param>
        /// <returns>40位大写SHA哈希值</returns>
        public static string EncryptSHA(string instr, Encoding bytesEncoding)
        {
            try
            {
                byte[] toByte = EncryptSHAToBytes(instr, bytesEncoding);
                string result = BitConverter.ToString(toByte).ToLower().Replace("-", "");
                return result;
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// SHA哈希
        /// </summary>
        /// <param name="instr">要加密的字符串</param>
        /// <param name="bytesEncoding">编码</param>
        /// <returns>20长度的字节数组</returns>
        public static byte[] EncryptSHAToBytes(string instr, Encoding bytesEncoding)
        {
            SHA1Managed sha = null;
            try
            {
                byte[] toByte = bytesEncoding.GetBytes(instr);
                sha = new SHA1Managed();
                toByte = sha.ComputeHash(toByte);
                return toByte;
            }
            catch
            {
                return null;
            }
            finally
            {
                if (sha != null)
                    sha.Clear();
            }
        }



        /// <summary>
        /// MD5哈希
        /// </summary>
        /// <param name="instr">要加密的字符串</param>
        /// <returns>32位大写MD5哈希值</returns>
        public static string EncryptMD5(string instr)
        {
            return EncryptMD5(instr, Encoding.UTF8);
        }

        /// <summary>
        /// MD5哈希
        /// </summary>
        /// <param name="instr">要加密的字符串</param>
        /// <param name="bytesEncoding">字节流编码</param>
        /// <returns>32位大写MD5哈希值</returns>
        public static string EncryptMD5(string instr, Encoding bytesEncoding)
        {
            try
            {
                byte[] toByte = EncryptMD5ToBytes(instr, bytesEncoding);
                string result = BitConverter.ToString(toByte).ToUpper().Replace("-", "");
                return result;
            }
            catch
            {
                return "";
            }
        }


        // MD5哈希
        /// </summary>
        /// <param name="instr">要加密的字符串</param>
        /// <param name="bytesEncoding">字节流编码</param>
        /// <returns>16长度的MD5哈希字节数组</returns>
        public static byte[] EncryptMD5ToBytes(string instr, Encoding bytesEncoding)
        {
            MD5CryptoServiceProvider md5 = null;
            try
            {
                byte[] toByte = bytesEncoding.GetBytes(instr);
                md5 = new MD5CryptoServiceProvider();
                toByte = md5.ComputeHash(toByte);
                return toByte;
            }
            catch
            {
                return null;
            }
            finally
            {
                if (md5 != null)
                    md5.Clear();
            }
        }

        #endregion

    }
}