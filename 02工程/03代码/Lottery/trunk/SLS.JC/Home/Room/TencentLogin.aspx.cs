using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Security.Cryptography;
using System.Collections;
public partial class Home_Room_TencentLogin : System.Web.UI.Page
{
    #region 网关参数
    public string requestUrl = "";
    public string sign_type = "";
    private string key = "";
    public string sign_encrypt_keyid = "";
    public string input_charset = "";
    public string service = "";
    public string chnid = "";
    public string chtype = "";
    public string redirect_url = "";
    public string attach = "";
    public UInt32 tmstamp ;
    public string sign = "";

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        SettingParams();
    }
    //生成请求
    public void SettingParams()
    {
        SystemOptions so = new SystemOptions();

        //请求网关
        requestUrl = so["MemberSharing_Tencent_Gateway"].ToString("").Trim();

        //签名类型
        sign_type = "md5";

        //签名
        key = so["MemberSharing_Tencent_MD5"].ToString("").Trim();

        //对于商户：使用和支付时一样的key，可以登录财付通商户系统修改。
        sign_encrypt_keyid = "0";

        //字符编码格式
        input_charset = "GBK";

        //服务名称
        service = "login";

        //商户编号
        chnid = so["MemberSharing_Tencent_UserNumber"].ToString("").Trim();

        //chnid类型
        chtype = "0";

        //服务器通知返回接口
        redirect_url = Shove._Web.Utility.GetUrl() + "/Home/Room/TencentReceive.aspx";

        //自定义参数
        attach =redirect_url;

        //时间戳
        tmstamp = GetTmstamp();
        Dictionary<string, string> dict = new Dictionary<string, string>();
        dict.Add("sign_type", sign_type);
        dict.Add("sign_encrypt_keyid", sign_encrypt_keyid);
        dict.Add("input_charset", input_charset);
        dict.Add("service", service);
        dict.Add("chnid", chnid);
        dict.Add("chtype", chtype);
        dict.Add("redirect_url", redirect_url);
        dict.Add("attach", attach);
        dict.Add("tmstamp", tmstamp.ToString());
        sign=GetSign(key, input_charset,dict);
    }

    //生成签名 
    public string GetSign(string key, string input_charset, Dictionary<string, string> dict)
    {
        StringBuilder sb = new StringBuilder();

        ArrayList akeys = new ArrayList(dict.Keys);
        akeys.Sort();

        foreach (string k in akeys)
        {
            string v = (string)dict[k];
            if (null != v && "".CompareTo(v) != 0
                && "sign".CompareTo(k) != 0 && "key".CompareTo(k) != 0)
            {
                sb.Append(k + "=" + v + "&");
            }
        }
        sb.Append("key=" + key);
        return GetMD5(sb.ToString(), input_charset).ToLower();
    }

    //生成时间戳
    public UInt32 GetTmstamp()
    {
        TimeSpan ts = DateTime.Now - TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
        return Convert.ToUInt32(ts.TotalSeconds);
    }
    /// <summary>
    /// 获取找小写的MD5签名结果
    /// </summary>
    /// <param name="encypStr"></param>
    /// <returns></returns>
    private string GetMD5(string encypStr,string charset)
    {
        string retStr;
        MD5CryptoServiceProvider m5 = new MD5CryptoServiceProvider();

        //创建md5对象
        byte[] inputBye;
        byte[] outputBye;

        //使用utf-8编码方式把字符串转化为字节数组．
        inputBye = Encoding.GetEncoding(charset).GetBytes(encypStr);

        outputBye = m5.ComputeHash(inputBye);

        retStr = System.BitConverter.ToString(outputBye);
        retStr = retStr.Replace("-", "").ToLower();
        return retStr;
    }
}
