using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Configuration;
using System.Web.Security;
using System.Text;
using System.Security.Cryptography;
using System.Collections.Specialized;
using System.IO;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Net;
using System.Text.RegularExpressions;

public partial class Home_Room_Receive : SitePageBase
{
    SystemOptions so = new SystemOptions();
    public string Script = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        AjaxPro.Utility.RegisterTypeForAjax(typeof(Home_Room_Receive), this.Page);

        if (this.Request.QueryString.Count < 1)
        {
            Shove._Web.JavaScript.Alert(this.Page, "接口调用失败，请重新登录。", "../../Default.aspx");

            return;
        }

        string alipayNotifyURL = "http://notify.alipay.com/trade/notify_query.do?";
        string partner = so["MemberSharing_Alipay_UserNumber"].ToString("");  //卖家商户号

        alipayNotifyURL = alipayNotifyURL + "partner=" + partner + "&notify_id=" + Request.QueryString["notify_id"];

        //获取支付宝ATN返回结果，true是正确的订单信息，false 是无效的
        string responseTxt = Get_Http(alipayNotifyURL, 120000);

        if (responseTxt == "false")
        {
            Shove._Web.JavaScript.Alert(this.Page, "接口调用失败，请重新登录。", "../../Default.aspx");

            return;
        }

        bool isUseCheckCode = _Site.SiteOptions["Opt_isUseCheckCode"].ToBoolean(true);
        CheckCode.Visible = isUseCheckCode;

        new Login().SetCheckCode(_Site, ShoveCheckCode1);

        string key = so["MemberSharing_Alipay_MD5"].ToString(""); //partner 的对应交易安全校验码（必须填写）和alipay.cs文件中值是一样的
        string _input_charset = "utf-8";

        int i;
        String[] requestarr = Request.QueryString.AllKeys;

        //进行排序；
        string[] Sortedstr = Shove.Alipay.Alipay.BubbleSort(requestarr);

        //构造待md5摘要字符串 ；
        StringBuilder prestr = new StringBuilder();

        for (i = 0; i < Sortedstr.Length; i++)
        {
            if (String.IsNullOrEmpty(Sortedstr[i]))
            {
                continue;
            }

            if (Request.QueryString[Sortedstr[i]] != "" && Sortedstr[i] != "sign" && Sortedstr[i] != "sign_type" && Sortedstr[i].ToLower() != "pn")
            {
                if (i == Sortedstr.Length - 1)
                {
                    prestr.Append(Sortedstr[i] + "=" + Request.QueryString[Sortedstr[i]]);

                }
                else
                {
                    prestr.Append(Sortedstr[i] + "=" + Request.QueryString[Sortedstr[i]] + "&");
                }
            }
        }

        prestr.Append(key);

        //生成Md5摘要；
        string mysign = Shove.Alipay.Alipay.GetMD5(prestr.ToString(), _input_charset);

        string sign = (Request.QueryString["sign"] == null ? "" : Request.QueryString["sign"].ToString());
        string Success = (Request.QueryString["is_success"] == null ? "" : Request.QueryString["is_success"].ToString().ToUpper());
        string User_id = (Request.QueryString["user_id"] == null ? "" : Request.QueryString["user_id"].ToString());
        string RealName = (Request.QueryString["real_name"] == null ? "" : Request.QueryString["real_name"].ToString());
        string Email = (Request.QueryString["email"] == null ? "" : Request.QueryString["email"].ToString()); 

        //******************************************************************************
        if (mysign != sign)   //验证支付发过来的消息，签名是否正确（防止有伪造消息）
        {
            //WriteLog
            PF.GoError(ErrorNumber.Unknow, "您不是有效的支付宝会员不能登录本站，请您注册成为本站会员，再登录，谢谢！(-1001)", this.GetType().FullName);

            return;
        }

        if (Success != "T")
        {
            PF.GoError(ErrorNumber.Unknow, "您不是有效的支付宝会员不能登录本站，请您注册成为本站会员，再登录，谢谢！(-1002)", this.GetType().FullName);

            return;
        }

        labAccount.Text = Email;

        //处理数据

        if (String.IsNullOrEmpty(User_id))
        {
            PF.GoError(ErrorNumber.Unknow, "您不是有效的支付宝会员不能登录本站，请您注册成为本站会员，再登录，谢谢！(-1003)", this.GetType().FullName);

            return;
        }

        Sites site = new Sites()[Shove._Web.Utility.GetUrlWithoutHttp()];

        if (site == null)
        {
            PF.GoError(ErrorNumber.Unknow, "会员数据校验错误。", this.GetType().FullName);

            return;
        }

        if (_User != null && Shove._Web.Cache.GetCache("BindAlipay_" + _User.ID.ToString()) != null)
        {
            Shove._Web.Cache.ClearCache("BindAlipay_" + _User.ID.ToString());

            System.Threading.Thread.Sleep(500);

            Users tu = new Users(_Site.ID);
            _User.Clone(tu);

            _User.AlipayID = User_id;
            _User.isAlipayNameValided = true;
            _User.AlipayName = Email;

            string ReturnDescription = "";

            if (_User.EditByID(ref ReturnDescription) < 0)
            {
                tu.Clone(_User);
                Shove._Web.JavaScript.Alert(this.Page, ReturnDescription);

                return;
            }

            Shove._Web.JavaScript.Alert(this.Page, "支付宝绑定成功！", "BindAlipay.aspx");
        }

        DAL.Tables.T_Users t_users = new DAL.Tables.T_Users();
        DataTable dt = t_users.Open("[ID], [Name]", "SiteID = " + site.ID.ToString() + " and AlipayID = '" + User_id + "' and isAlipayNameValided = 1", "[ID]");

        if (dt == null)
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", this.GetType().FullName);

            return;
        }

        if (!IsPostBack)
        {
            tbRealityName.Text = RealName;
            hidUserID.Value = User_id;

            if (dt.Rows.Count < 1)
            {
                // 不存在，注册为新会员
                tbAlipayID.Text = User_id;
                Script = "btn_CheckUserName('" + tbName.Text + "')";
                btnSelect.Enabled = false;
                tableSelect.Visible = false;

                return;
            }
            else if (dt.Rows.Count == 1)
            {
                long UserID = Shove._Convert.StrToLong(dt.Rows[0]["ID"].ToString(), -1);

                if (UserID < 0)
                {
                    PF.GoError(ErrorNumber.Unknow, "会员数据校验错误。", this.GetType().FullName);

                    return;
                }

                Users user = new Users(site.ID)[site.ID, UserID];

                if (user == null)
                {
                    PF.GoError(ErrorNumber.Unknow, "会员数据校验错误。", this.GetType().FullName);

                    return;
                }

                string ReturnDescription = "";

                if (user.LoginDirect(ref ReturnDescription) < 0)
                {
                    PF.GoError(ErrorNumber.Unknow, ReturnDescription, this.GetType().FullName);

                    return;
                }

                Response.Redirect("../../Default.aspx", true);

                return;
            }

            // 绑定了多个用户
            tableRegister.Visible = false;
            btnOK.Enabled = false;
            tableSelect.Visible = true;
            btnSelect.Enabled = true;

            Shove.ControlExt.FillDropDownList(ddlName, dt, "Name", "ID");
            ddlName.SelectedIndex = 0;
        }
    }

    //获取远程服务器ATN结果
    private String Get_Http(String a_strUrl, int timeout)
    {
        string strResult;
        try
        {
            HttpWebRequest myReq = (HttpWebRequest)HttpWebRequest.Create(a_strUrl);
            myReq.Timeout = timeout;
            HttpWebResponse HttpWResp = (HttpWebResponse)myReq.GetResponse();
            Stream myStream = HttpWResp.GetResponseStream();
            StreamReader sr = new StreamReader(myStream, Encoding.Default);
            StringBuilder strBuilder = new StringBuilder();
            while (-1 != sr.Peek())
            {
                strBuilder.Append(sr.ReadLine());
            }

            strResult = strBuilder.ToString();
        }
        catch (Exception exp)
        {

            strResult = "错误：" + exp.Message;
        }

        return strResult;
    }

    protected void btnOK_Click(object sender, EventArgs e)
    {
        if (tbName.Text.Trim() == "")
        {
            Shove._Web.JavaScript.Alert(this.Page, "请输入用户名。");

            return;
        }

        if (string.IsNullOrEmpty(tbRealityName.Text.Trim()))
        {
            Shove._Web.JavaScript.Alert(this.Page, "请输入真实姓名。");

            return;
        }

        if (CheckCode.Visible)
        {
            if (tbCheckCode.Text.Trim() == "")
            {
                Shove._Web.JavaScript.Alert(this.Page, "请输入验证码！");

                return;
            }
            else
            {
                if (!ShoveCheckCode1.Valid(tbCheckCode.Text.Trim()))
                {
                    Shove._Web.JavaScript.Alert(this.Page, "验证码输入有误！");

                    return;
                }
            }

        }

        System.Threading.Thread.Sleep(500);

        Sites site = new Sites()[Shove._Web.Utility.GetUrlWithoutHttp()];

        if (site == null)
        {
            return;
        }

        long CpsID = -1;
        string URL = new FirstUrl().Get();
        if (!URL.StartsWith("http://"))
        {
            URL = "http://" + URL;
            URL = URL.Split('?'.ToString().ToCharArray())[0];
        }
        DataTable dt = new DAL.Tables.T_Cps().Open("id, [ON], [Name]", "SiteID = " + _Site.ID.ToString() + " and DomainName = '" + URL + "' or DomainName='" + Shove._Web.Utility.GetUrl() + "'", "");

        if ((dt != null) && (dt.Rows.Count > 0))
        {
            if (Shove._Convert.StrToBool(dt.Rows[0]["ON"].ToString(), false))
            {
                CpsID = Shove._Convert.StrToLong(dt.Rows[0]["ID"].ToString(), -1);
            }
        }

        string Password = GetRandPassword();

        Users user = new Users(site.ID);

        user.Name = tbName.Text.Trim();
        user.RealityName = tbRealityName.Text.Trim();
        user.Password = Password;
        user.PasswordAdv = Password;
        user.CityID = 1;
        user.Email = labAccount.Text.Trim();
        user.ComeFrom = 4;
        user.UserType = 2;
        user.CpsID = CpsID;
        user.CommenderID = -1;

        string ReturnDescription = "";

        if (user.Add(ref ReturnDescription) < 0)
        {
            Shove._Web.JavaScript.Alert(this.Page, ReturnDescription + "用户添加失败");

            return;
        }

        DAL.Tables.T_Users T_Users = new DAL.Tables.T_Users();
        T_Users.AlipayName.Value = labAccount.Text;
        T_Users.AlipayID.Value = hidUserID.Value;
        T_Users.isAlipayNameValided.Value = true;
        T_Users.Update("[ID] = " + user.ID.ToString());

        user.LoginDirect(ref ReturnDescription);

        this.Response.Redirect("UserRegSuccess.aspx", true);
    }

    private string GetRandPassword()
    {
        string CharSet = "0123456789";
        string Password = "";
        Random rand = new Random(DateTime.Now.Millisecond);

        for (int i = 0; i < 6; i++)
        {
            Password += CharSet[rand.Next(0, 9)].ToString();
        }

        return Password;
    }

    protected void btnSelect_Click(object sender, EventArgs e)
    {
        if (ddlName.Items.Count < 1)
        {
            return;
        }

        long UserID = Shove._Convert.StrToLong(ddlName.SelectedValue, -1);

        if (UserID < 0)
        {
            this.Response.Write("接口调用失败，原因：系统错误(1005)。");
            this.Response.End();

            return;
        }

        Sites site = new Sites()[Shove._Web.Utility.GetUrlWithoutHttp()];

        if (site == null)
        {
            this.Response.Write("接口调用失败，原因：系统错误(1006)。");
            this.Response.End();

            return;
        }

        Users user = new Users(site.ID)[site.ID, UserID];

        if (user == null)
        {
            this.Response.Write("接口调用失败，原因：系统错误(1007)。");
            this.Response.End();

            return;
        }

        string ReturnDescription = "";
        if (user.LoginDirect(ref ReturnDescription) < 0)
        {
            this.Response.Write(ReturnDescription);

            return;
        }

        this.Response.Redirect("../../Default.aspx", true);
    }

    /// <summary>
    /// 检测用户名
    /// </summary>
    /// <returns></returns>
    [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.None)]
    public int CheckUserName(string userName)
    {
        if (!PF.CheckUserName(userName))
        {
            return -1;
        }

        DataTable dt = new DAL.Tables.T_Users().Open("ID", "Name = '" + Shove._Web.Utility.FilteSqlInfusion(userName) + "'", "");

        if (dt != null && dt.Rows.Count > 0)
        {
            return -2;
        }

        if (Shove._String.GetLength(userName) < 5 || Shove._String.GetLength(userName) > 16)
        {
            return -3;
        }

        return 0;
    }
}