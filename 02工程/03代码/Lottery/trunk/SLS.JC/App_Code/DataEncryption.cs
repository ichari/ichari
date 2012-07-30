using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.IO;
using System.Configuration;
using System.Text;

/// <summary>
/// Customized data encryption for standard use across the whole platform to unify the encryption used on cookies
/// </summary>
public class DataEncryption
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

    public static void SaveToCookies(string message, long lottery_uid)
    {
        #region Lottery
        string uid = lottery_uid.ToString();
        string enc_uid = EncryptAES(uid);
        HttpCookie siteCookie = new HttpCookie(ConfigurationManager.AppSettings["SiteCookieName"]);
        siteCookie.Domain = ConfigurationManager.AppSettings["SiteCookieName"];
        siteCookie.Name = ConfigurationManager.AppSettings["SiteCookieName"];
        if (string.IsNullOrEmpty(siteCookie.Name))
            siteCookie.Name = "ichari.com";
        siteCookie.Values.Add("t", DateTime.Now.Ticks.ToString());
        siteCookie.Values.Add(ConfigurationManager.AppSettings["LotteryCookieName"], enc_uid);
        siteCookie.Expires = DateTime.Now.AddDays(3);
        #endregion

        //siteCookie.Values.Add(ConfigurationManager.AppSettings["SiteCookieName"], message);
        HttpContext.Current.Response.Cookies.Add(siteCookie);
    }
    public static HttpCookie MakeCookie(string message, long lottery_uid)
    {
        #region Lottery
        string uid = lottery_uid.ToString();
        string enc_uid = EncryptAES(uid);
        HttpCookie siteCookie = new HttpCookie(ConfigurationManager.AppSettings["SiteCookieName"]);
        siteCookie.Domain = ConfigurationManager.AppSettings["SiteCookieName"];
        siteCookie.Name = ConfigurationManager.AppSettings["SiteCookieName"];
        if (string.IsNullOrEmpty(siteCookie.Name))
            siteCookie.Name = "ichari.com";
        siteCookie.Values.Add("t", DateTime.Now.Ticks.ToString());
        siteCookie.Values.Add(ConfigurationManager.AppSettings["LotteryCookieName"], enc_uid);
        #endregion
        siteCookie.Expires = DateTime.Now.AddDays(3);
        //siteCookie.Values.Add(ConfigurationManager.AppSettings["SiteCookieName"], message);
        return siteCookie;
    }
    public static HttpCookie GetCookie()
    {
        string cookieName = string.IsNullOrEmpty(ConfigurationManager.AppSettings["SiteCookieName"]) ? "ichari.com" : ConfigurationManager.AppSettings["SiteCookieName"];
        return HttpContext.Current.Request.Cookies[cookieName];
    }
}