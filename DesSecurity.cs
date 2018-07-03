using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Zdd.Utility
{
    /// <summary>
    /// 字符加解密
    /// </summary>
    public class DesSecurity
    {
        static readonly string KEY_64 = "QC198188";
        static readonly string IV_64 = "APISVRCE"; //注意了，是8个字符，64位
        /// <summary>
        /// 加密字符串
        /// </summary>
        /// <param name="data"></param>
        /// <returns>返回加密后的base64字符串</returns>
        public static string Encode(string data)
        {
            MemoryStream ms = null;
            CryptoStream cst = null;
            StreamWriter sw = null;
            try
            {
                byte[] byKey = System.Text.ASCIIEncoding.ASCII.GetBytes(KEY_64);
                byte[] byIV = System.Text.ASCIIEncoding.ASCII.GetBytes(IV_64);
                DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
                int i = cryptoProvider.KeySize;
                ms = new MemoryStream();
                cst =
                    new CryptoStream(ms, cryptoProvider.CreateEncryptor(byKey, byIV), CryptoStreamMode.Write);
                sw = new StreamWriter(cst);
                sw.Write(data);
                sw.Flush();
                cst.FlushFinalBlock();
                sw.Flush();
                return Convert.ToBase64String(ms.GetBuffer(), 0, (int)ms.Length);
            }
            catch
            {
                return string.Empty;
            }
            finally
            {
                if (ms != null) ms.Dispose();
                if (cst != null) cst.Dispose();
                if (sw != null) sw.Dispose();
            }
        }

        /// <summary>
        /// 解密字符串
        /// </summary>
        /// <param name="data">加密的base64字符串</param>
        /// <returns></returns>
        public static string Decode(string data)
        {
            MemoryStream ms = null;
            CryptoStream cst = null;
            StreamReader sr = null;
            try
            {
                byte[] byKey = System.Text.ASCIIEncoding.ASCII.GetBytes(KEY_64);
                byte[] byIV = System.Text.ASCIIEncoding.ASCII.GetBytes(IV_64);
                byte[] byEnc = Convert.FromBase64String(data);
                DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
                ms = new MemoryStream(byEnc);
                cst = new CryptoStream(ms, cryptoProvider.CreateDecryptor(byKey, byIV), CryptoStreamMode.Read);
                sr = new StreamReader(cst);
                return sr.ReadToEnd();
            }
            catch
            {
                return string.Empty;
            }
            finally
            {
                if (ms != null) ms.Dispose();
                if (cst != null) cst.Dispose();
                if (sr != null) sr.Dispose();
            }
        }
        /// <summary>
        /// DES加密,使用默认编码Encoding.Default
        /// </summary>
        /// <param name="sourcestring">待加密的字符串</param>
        /// <param name="key">8字节密钥</param>
        /// <param name="iv">8字节向量,默认 00000000</param>
        /// <returns>返回 加密的base64格式字符串</returns>
        public static string DesEncrypt(string sourcestring, string key, string iv = "")
        {
            try
            {
                byte[] btKey = Encoding.Default.GetBytes(key);
                byte[] btIV = new byte[8] { 0, 0, 0, 0, 0, 0, 0, 0 };
                if (!string.IsNullOrEmpty(iv))
                {
                    btIV = Encoding.Default.GetBytes(iv);
                }

                using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        byte[] inData = Encoding.Default.GetBytes(sourcestring);
                        using (CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(btKey, btIV), CryptoStreamMode.Write))
                        {
                            cs.Write(inData, 0, inData.Length);
                            cs.FlushFinalBlock();
                        }
                        return Convert.ToBase64String(ms.ToArray());
                    }
                }
            }
            catch (Exception)
            {

            }
            return "DES加密出错";
        }

        /// <summary>
        /// .NET Des解密 使用默认编码Encoding.Default
        /// </summary>
        /// <param name="encryptedstring">加密过的Base64格式字符串</param>
        /// <param name="key">8字节密钥字符串</param>
        /// <param name="iv">8字节IV,默认 00000000</param>
        /// <returns>返回源字符串</returns>
        public static string DesDecrypt(string encryptedstring, string key, string iv = "")
        {
            try
            {
                byte[] btKey = Encoding.Default.GetBytes(key);
                byte[] btIV = new byte[8] { 0, 0, 0, 0, 0, 0, 0, 0 };
                if (!string.IsNullOrEmpty(iv))
                {
                    btIV = Encoding.Default.GetBytes(iv);
                }

                using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        byte[] inData = Convert.FromBase64String(encryptedstring);
                        using (CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(btKey, btIV), CryptoStreamMode.Write))
                        {
                            cs.Write(inData, 0, inData.Length);
                            cs.FlushFinalBlock();
                        }
                        return Encoding.Default.GetString(ms.ToArray());
                    }
                }
            }
            catch (Exception)
            {
                return encryptedstring;
            }
        }
        //////////////////////////////////////////////////////////////////////////
        #region CBC模式**
        /// <summary>  
        /// DES3 CBC模式加密  
        /// </summary>  
        /// <param name="key">密钥,24字节明文</param>  
        /// <param name="iv">IV</param>  
        /// <param name="data">明文的byte数组</param>  
        /// <returns>密文的byte数组</returns>  
        public static byte[] Des3EncodeCBC(byte[] key, byte[] iv, byte[] data)
        {
            //复制于MSDN  
            try
            {
                // Create a MemoryStream.  
                MemoryStream mStream = new MemoryStream();
                TripleDESCryptoServiceProvider tdsp = new TripleDESCryptoServiceProvider();
                tdsp.Mode = CipherMode.CBC;             //默认值  
                tdsp.Padding = PaddingMode.PKCS7;       //默认值  
                // Create a CryptoStream using the MemoryStream   
                // and the passed key and initialization vector (IV).  
                CryptoStream cStream = new CryptoStream(mStream,
                    tdsp.CreateEncryptor(key, iv),
                    CryptoStreamMode.Write);
                // Write the byte array to the crypto stream and flush it.  
                cStream.Write(data, 0, data.Length);
                cStream.FlushFinalBlock();
                // Get an array of bytes from the   
                // MemoryStream that holds the   
                // encrypted data.  
                byte[] ret = mStream.ToArray();
                // Close the streams.  
                cStream.Close();
                mStream.Close();
                // Return the encrypted buffer.  
                return ret;
            }
            catch (CryptographicException e)
            {
                Console.WriteLine("A Cryptographic error occurred: {0}", e.Message);
                return null;
            }
        }
        /// <summary>  
        /// DES3 CBC模式解密  
        /// </summary>  
        /// <param name="key">密钥,24字节明文</param>  
        /// <param name="iv">IV</param>  
        /// <param name="data">密文的byte数组</param>  
        /// <returns>明文的byte数组</returns>  
        public static byte[] Des3DecodeCBC(byte[] key, byte[] iv, byte[] data)
        {
            try
            {
                // Create a new MemoryStream using the passed   
                // array of encrypted data.  
                MemoryStream msDecrypt = new MemoryStream(data);
                TripleDESCryptoServiceProvider tdsp = new TripleDESCryptoServiceProvider();
                tdsp.Mode = CipherMode.CBC;
                tdsp.Padding = PaddingMode.PKCS7;
                // Create a CryptoStream using the MemoryStream   
                // and the passed key and initialization vector (IV).  
                CryptoStream csDecrypt = new CryptoStream(msDecrypt,
                    tdsp.CreateDecryptor(key, iv),
                    CryptoStreamMode.Read);
                // Create buffer to hold the decrypted data.  
                byte[] fromEncrypt = new byte[data.Length];
                // Read the decrypted data out of the crypto stream  
                // and place it into the temporary buffer.  
                csDecrypt.Read(fromEncrypt, 0, fromEncrypt.Length);
                //Convert the buffer into a string and return it.  
                return fromEncrypt;
            }
            catch (CryptographicException e)
            {
                Console.WriteLine("A Cryptographic error occurred: {0}", e.Message);
                return null;
            }
        }
        #endregion
        #region ECB模式
        /// <summary>  
        /// DES3 ECB模式加密  
        /// </summary>  
        /// <param name="key">密钥,24字节明文</param>  
        /// <param name="iv">IV(当模式为ECB时，IV无用)</param>  
        /// <param name="str">明文的byte数组</param>  
        /// <returns>密文的byte数组</returns>  
        public static byte[] Des3EncodeECB(byte[] key, byte[] iv, byte[] data)
        {
            try
            {
                // Create a MemoryStream.  
                MemoryStream mStream = new MemoryStream();
                TripleDESCryptoServiceProvider tdsp = new TripleDESCryptoServiceProvider();
                tdsp.Mode = CipherMode.ECB;
                tdsp.Padding = PaddingMode.PKCS7;
                // Create a CryptoStream using the MemoryStream   
                // and the passed key and initialization vector (IV).  
                CryptoStream cStream = new CryptoStream(mStream,
                    tdsp.CreateEncryptor(key, iv),
                    CryptoStreamMode.Write);
                // Write the byte array to the crypto stream and flush it.  
                cStream.Write(data, 0, data.Length);
                cStream.FlushFinalBlock();
                // Get an array of bytes from the   
                // MemoryStream that holds the   
                // encrypted data.  
                byte[] ret = mStream.ToArray();
                // Close the streams.  
                cStream.Close();
                mStream.Close();
                // Return the encrypted buffer.  
                return ret;
            }
            catch (CryptographicException e)
            {
                Console.WriteLine("A Cryptographic error occurred: {0}", e.Message);
                return null;
            }
        }
        /// <summary>  
        /// DES3 ECB模式解密  
        /// </summary>  
        /// <param name="key">密钥,24字节明文</param>  
        /// <param name="iv">IV(当模式为ECB时，IV无用)</param>  
        /// <param name="str">密文的byte数组</param>  
        /// <returns>明文的byte数组</returns>  
        public static byte[] Des3DecodeECB(byte[] key, byte[] iv, byte[] data)
        {
            try
            {
                // Create a new MemoryStream using the passed   
                // array of encrypted data.  
                MemoryStream msDecrypt = new MemoryStream(data);
                TripleDESCryptoServiceProvider tdsp = new TripleDESCryptoServiceProvider();
                tdsp.Mode = CipherMode.ECB;
                tdsp.Padding = PaddingMode.PKCS7;
                // Create a CryptoStream using the MemoryStream   
                // and the passed key and initialization vector (IV).  
                CryptoStream csDecrypt = new CryptoStream(msDecrypt,
                    tdsp.CreateDecryptor(key, iv),
                    CryptoStreamMode.Read);
                // Create buffer to hold the decrypted data.  
                byte[] fromEncrypt = new byte[data.Length];
                // Read the decrypted data out of the crypto stream  
                // and place it into the temporary buffer.  
                csDecrypt.Read(fromEncrypt, 0, fromEncrypt.Length);
                //Convert the buffer into a string and return it.  
                return fromEncrypt;
            }
            catch (CryptographicException e)
            {
                Console.WriteLine("A Cryptographic error occurred: {0}", e.Message);
                return null;
            }
        }
        #endregion
        //////////////////////////////////////////////////////////////////////////
    }
}
