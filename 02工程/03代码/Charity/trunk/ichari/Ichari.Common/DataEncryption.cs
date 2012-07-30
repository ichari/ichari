using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Web;
using System.Configuration;

namespace Ichari.Common
{
    public static class DataEncryption
    {
        public static byte[] StringToBytes(string str_to_byte)
        {
            return Encoding.ASCII.GetBytes(str_to_byte);
        }
        public static string BytesToString(byte[] byte_to_str)
        {
            return Encoding.ASCII.GetString(byte_to_str);
        }

        #region encryptions
        public static AesCryptoServiceProvider GetAESProvider()
        {
            UTF8Encoding benc = new UTF8Encoding();
            FileStream fs;
            AesCryptoServiceProvider enc_aes = new AesCryptoServiceProvider();

            fs = new FileStream(HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["AESKeyFile"].ToString()), FileMode.Open, FileAccess.Read);
            byte[] fByte = new byte[fs.Length];
            fs.Read(fByte, 0, fByte.Length);
            fs.Close();

            enc_aes.Padding = PaddingMode.ISO10126;
            enc_aes.KeySize = 256;
            byte[] ebyte = new byte[32];
            byte[] eIV = new byte[16];

            Array.Copy(fByte, 0, ebyte, 0, 32);
            Array.Copy(fByte, 32, eIV, 0, 16);
            enc_aes.Key = ebyte;
            enc_aes.IV = eIV;

            return enc_aes;
        }
        public static ICryptoTransform GetDecryptor()
        {
            return GetAESProvider().CreateDecryptor();
        }
        public static ICryptoTransform GetEncryptor()
        {
            return GetAESProvider().CreateEncryptor();
        }
        public static byte[] HexStringToBytes(string hex_to_byte)
        {
            char[] sp = new char[1];
            sp[0] = '-';
            string[] hstr = hex_to_byte.Split(sp);
            byte[] hbyte = new byte[hstr.Length];
            for (int i = 0; i < hstr.Length; i++)
                hbyte[i] = Byte.Parse(hstr[i], System.Globalization.NumberStyles.HexNumber);

            return hbyte;
        }
        public static string BytesToHexString(byte[] byte_to_hex)
        {
            return BitConverter.ToString(byte_to_hex);
        }
        /**
         * str_in is in format of FF-FF-FF-FF, hex string with the dashes seperating each byte
         */
        public static string DecryptAES(string str_in)
        {
            MemoryStream msAES = new MemoryStream();
            CryptoStream csAES = new CryptoStream(msAES, GetDecryptor(), CryptoStreamMode.Write);
            byte[] bstr = HexStringToBytes(str_in);
            csAES.Write(bstr, 0, bstr.Length);
            csAES.FlushFinalBlock();
            byte[] decMsg = msAES.ToArray();

            msAES.Close();
            csAES.Close();

            return BytesToString(decMsg);
        }
        public static string EncryptAES(string str_in)
        {
            MemoryStream msAES = new MemoryStream();
            CryptoStream csAES = new CryptoStream(msAES, GetEncryptor(), CryptoStreamMode.Write);
            byte[] msg = StringToBytes(str_in);
            csAES.Write(msg, 0, msg.Length);
            csAES.FlushFinalBlock();
            byte[] encMsg = msAES.ToArray();

            msAES.Close();
            csAES.Close();

            return BytesToHexString(encMsg);
        }
        #endregion

        #region hash

        public static string HashString(string str_to_hash)
        {
            HMACSHA256 hs256 = new HMACSHA256(Encoding.ASCII.GetBytes(ConfigurationManager.AppSettings["UserSalt"]));
            return BytesToHexString(hs256.ComputeHash(StringToBytes(str_to_hash))).Replace("-", "");
        }

        public static byte[] HashStringToBytes(string str_to_hash)
        {
            HMACSHA256 hs256 = new HMACSHA256(Encoding.ASCII.GetBytes(ConfigurationManager.AppSettings["UserSalt"]));
            return hs256.ComputeHash(StringToBytes(str_to_hash));
        }

        #endregion

        /// <summary>
        /// save cookie
        /// </summary>
        /// <param name="signStr">sha6(userid+username+pwd+key)</param>
        /// <param name="userName"></param>
        /// <param name="lottery_uid"></param>
        public static void SaveToCookies(string signStr,string userName, long lottery_uid,string ckDomain,bool isrememberState)
        {
            #region Lottery
            string uid = lottery_uid.ToString();
            string enc_uid = EncryptAES(uid);
            HttpCookie siteCookie = new HttpCookie(ConfigurationManager.AppSettings["SiteCookieName"]);
            siteCookie.Values.Add(ConfigurationManager.AppSettings["LotteryCookieName"], enc_uid);
            #endregion

            siteCookie.Values.Add("userName", userName);
            siteCookie.Values.Add("sign",signStr);
            if (isrememberState)
            {
                siteCookie.Expires = DateTime.Now.AddMonths(1);
            }
            siteCookie.Path = "/";
            siteCookie.Domain = ckDomain;
            siteCookie.HttpOnly = true;

            HttpContext.Current.Response.Cookies.Add(siteCookie);
        }

        /**
         * Generates a number 0 ~ 1000 using crypto random number generator, with a random skipping of 5~9 rounds
         * Process: gets the first random number mod 5 plus 5 to get a number, n, between 5 to 9 to determine the number
         *      of rounds to skip.
         *          generate n numbers and discard them
         *          next number generated is converted into unsigned 16-bit int, then converted to 32-bit int,
         *      then mod 1000 to get the final number to return
         */
        public static int GenerateNum()
        {
            byte[] bInt = new byte[2];
            RNGCryptoServiceProvider cr = new RNGCryptoServiceProvider();

            cr.GetBytes(bInt);
            return Convert.ToInt32(BitConverter.ToUInt16(bInt, 0)) % 1000;
        }
        
        private static double WinningChance = double.Parse(ConfigurationManager.AppSettings["DrawingChance"]);
        
        public static bool GetDrawingResult()
        {
            if (GenerateNum() < Convert.ToInt32(WinningChance * 10))
                return true;
            return false;
        }

        /// <summary>
        /// Hashes the input string from UPOP without signMethod and signature variables
        /// </summary>
        /// <param name="str_in">String to hash</param>
        /// <returns>Returns the hashed string</returns>
        public static string HashUPOPChariOrder(string str_in)
        {
            MD5CryptoServiceProvider mhash = new MD5CryptoServiceProvider();
            string hstr = BytesToHexString(mhash.ComputeHash(StringToBytes(ConfigurationManager.AppSettings["UPOPChariKey"]))).Replace("-", "").ToLower();
            string hkey = str_in + "&" + hstr;
            return BytesToHexString(mhash.ComputeHash(StringToBytes(hkey)));
        }
    
    }
}
