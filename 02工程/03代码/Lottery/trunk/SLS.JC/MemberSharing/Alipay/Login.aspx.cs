using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;
using System.Security.Cryptography;

public partial class MemberSharing_Alipay_Login : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SystemOptions so = new SystemOptions();

        string key = so["MemberSharing_Alipay_MD5"].ToString("");
        string return_url = Shove._Web.Utility.GetUrl() + "/MemberSharing/Alipay/Receive.aspx"; //服务器通知返回接口
        string gateway = "https://mapi.alipay.com/gateway.do";             //支付接口
        string _input_charset = "utf-8";
        string service = "alipay.auth.authorize";
        string sign_type = "MD5";
        string partner = so["MemberSharing_Alipay_UserNumber"].ToString("");          //卖家商户号
        string target_service = "user.auth.quick.login";

        Alipay.Gateway.Utility ap = new Alipay.Gateway.Utility();
        string aliay_url = ap.CreatUrl(
            gateway,
            service,
            partner,
            sign_type,
            key,
            return_url,
            _input_charset,
            target_service
        );

        Response.Redirect(aliay_url, true);
    }
}
