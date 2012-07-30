using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.Security.Cryptography;
using System.Collections;
public partial class Home_Room_TencentReceive : SitePageBase
{
    SystemOptions so = new SystemOptions();
    public string Script = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        AjaxPro.Utility.RegisterTypeForAjax(typeof(Home_Room_TencentReceive), this.Page);


        if (this.Request.Form.AllKeys.Length < 1)
        {
            Shove._Web.JavaScript.Alert(this.Page, "接口调用失败，请重新登录。", "../../UserLogin.aspx");

            return;
        }
        StringBuilder sb = new StringBuilder();

        string input_charset = Request.Form["charset"] == null ? "" : Request.Form["charset"].ToString().Trim();
        string tmstamp = Request.Form["tmstamp"] == null ? "" : Request.Form["tmstamp"].ToString().Trim();
        string sign = Request.Form["sign"] == null ? "" : Request.Form["sign"].ToString().Trim();
        string key =so["MemberSharing_Tencent_MD5"].ToString("").Trim();

        string id = Request.Form["id"] == null ? "" : Request.Form["id"].ToString().Trim();
        string email = "" ;
        if (id.IndexOf("@") > 0 && id.IndexOf(".") > 0)
        {
            email = id;
        }
        else
        {
            email = id + "@qq.com";
        }

        if (!IsPostBack)
        {
            if (Shove._Convert.StrToLong(GetTmstamp(), 0) - Shove._Convert.StrToLong(tmstamp, 0) > 60 * 2)
            {
                Shove._Web.JavaScript.Alert(this.Page, "登陆超时，请重新登录。", "../../UserLogin.aspx");

                return;
            }
            string[] allKeys = Request.Form.AllKeys;
            string TencentSign = GetSign(key, input_charset, allKeys);
            if (TencentSign != sign)
            {
                Shove._Web.JavaScript.Alert(this.Page, "您不是有效的腾讯用户不能登录本站，请您注册成为本站会员，再登录，谢谢！(-1001)。", "../../UserLogin.aspx");

                return;
            }
            if (String.IsNullOrEmpty(id))
            {
                Shove._Web.JavaScript.Alert(this.Page, "您不是有效的腾讯用户不能登录本站，请您注册成为本站会员，再登录，谢谢！(-1002)。", "../../UserLogin.aspx");

                return;
            }

        }

        bool isUseCheckCode = _Site.SiteOptions["Opt_isUseCheckCode"].ToBoolean(true);
        CheckCode.Visible = isUseCheckCode;

        new Login().SetCheckCode(_Site, ShoveCheckCode1);

        Sites site = new Sites()[Shove._Web.Utility.GetUrlWithoutHttp()];

        if (site == null)
        {
            Shove._Web.JavaScript.Alert(this.Page, "站点信息不存在。", "../../UserLogin.aspx");

            return;
        }

        if (_User != null && Shove._Web.Cache.GetCache("UserQQBind_" + _User.ID.ToString()) != null)
        {
            
            if (Shove._Convert.StrToLong(id, 0) < 1)
            {
                Shove._Web.JavaScript.Alert(this.Page, "您输入的 QQ 号码不合法！", "TencentLogin.aspx");

                return;
            }
            Shove._Web.Cache.ClearCache("UserQQBind_" + _User.ID.ToString());
            System.Threading.Thread.Sleep(500);

            int ReturnValue = -1;
            string ReturnDescription = "";

            _User.QQ = id;
            _User.isQQValided = true;
            ReturnValue = _User.EditByID(ref ReturnDescription);

            if (ReturnValue < 0)
            {
                Shove._Web.JavaScript.Alert(this.Page, ReturnDescription);

                return;
            }

            Shove._Web.JavaScript.Alert(this.Page, "QQ号码绑定成功！","UserQQBind.aspx");
        }
      
        DAL.Tables.T_Users t_users = new DAL.Tables.T_Users();
        DataTable dt = t_users.Open("[ID], [Name]", "SiteID = " + site.ID.ToString() + " and QQ = '" + Shove._Web.Utility.FilteSqlInfusion(id) + "' and IsQQValided = 1", "[ID]");

        if (dt == null)
        {
            Shove._Web.JavaScript.Alert(this.Page, "数据库繁忙，请重试。", "../../UserLogin.aspx");

            return;
        }

        if (!IsPostBack)
        {
            labAccount.Text = id;
            labAccount2.Text = id;

            if (dt.Rows.Count < 1)
            {
                // 不存在，注册为新会员
                tbQQID.Text = id;
                tbName.Text = id;
                tbEmail.Text = email;

                btnSelect.Enabled = false;
                tableSelect.Visible = false;
                Script = "btn_CheckUserName('" + id + "')";
                return;
            }
            else if (dt.Rows.Count == 1)
            {
                long UserID = Shove._Convert.StrToLong(dt.Rows[0]["ID"].ToString(), -1);

                if (UserID < 0)
                {
                    Shove._Web.JavaScript.Alert(this.Page, "用户信息读取错误。", "../../UserLogin.aspx");

                    return;
                }

                Users user = new Users(site.ID)[site.ID, UserID];

                if (user == null)
                {
                    Shove._Web.JavaScript.Alert(this.Page, "用户信息不存在。", "../../UserLogin.aspx");

                    return;
                }

                string ReturnDescription = "";

                if (user.LoginDirect(ref ReturnDescription) < 0)
                {
                    PF.GoError(ErrorNumber.Unknow, ReturnDescription, this.GetType().FullName);

                    return;
                }

                ResponseToDistination(user, id);

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

    protected void btnOK_Click(object sender, EventArgs e)
    {

        #region 验证
        if (tbName.Text.Trim() == "")
        {
            Shove._Web.JavaScript.Alert(this.Page, "请输入用户名。");

            return;
        }

        if (tbTrueName.Text.Trim() == "")
        {
            Shove._Web.JavaScript.Alert(this.Page,"请输入真实姓名。");

            return;
        }

        if (tbPassword.Text == "")
        {
            Shove._Web.JavaScript.Alert(this.Page, "请输入用户密码。");

            return;
        }

        if (tbPassword.Text.Length < 6)
        {
            Shove._Web.JavaScript.Alert(this.Page, "用户密码长度不足 6 位。");

            return;
        }

        if (tbPassword.Text != tbPassword2.Text)
        {
            Shove._Web.JavaScript.Alert(this.Page, "输入的两次密码不一致。");

            return;
        }

        if (tbEmail.Text.Trim() == "")
        {
            Shove._Web.JavaScript.Alert(this.Page, "请输入您的邮箱。");

            return;
        }

        if (!Shove._String.Valid.isEmail(tbEmail.Text.Trim()))
        {
            Shove._Web.JavaScript.Alert(this.Page,"请正确输入您的邮箱。");

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
        #endregion

        System.Threading.Thread.Sleep(500);

        Sites site = new Sites()[Shove._Web.Utility.GetUrlWithoutHttp()];

        if (site == null)
        {
            return;
        }

        Users user = new Users(site.ID);

        user.Name =Shove._Web.Utility.FilteSqlInfusion(tbName.Text.Trim());
        user.RealityName = Shove._Web.Utility.FilteSqlInfusion(tbTrueName.Text.Trim());
        user.Password = Shove._Web.Utility.FilteSqlInfusion(tbPassword.Text.Trim());
        user.PasswordAdv = Shove._Web.Utility.FilteSqlInfusion(tbPassword.Text.Trim());
        user.CityID = 1;
        user.ComeFrom = 4;
        user.Email = Shove._Web.Utility.FilteSqlInfusion(tbEmail.Text.Trim());
        user.isEmailValided = true;
        user.QQ = tbQQID.Text.Trim();
        user.UserType = 2;
        user.CommenderID = -1;
        user.isQQValided = true;

        string ReturnDescription = "";

        int UserID = user.Add(ref ReturnDescription);

        if (UserID < 0)
        {
            Shove._Web.JavaScript.Alert(this.Page, ReturnDescription + "用户添加失败");

            return;
        }

        int Result = user.Login(ref ReturnDescription);

        if (Result < 0)
        {
            new Log("Users").Write("注册成功后登录失败：" + ReturnDescription);
            Shove._Web.JavaScript.Alert(this, ReturnDescription);

            return;
        }
        Response.Redirect("UserRegSuccess.aspx",true);
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

        ResponseToDistination(user,"245108764");
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
    
    //生成签名 
    public string GetSign(string key, string input_charset, params string[] requestarr)
    {
        ArrayList list = new ArrayList(requestarr);
        //构造待md5摘要字符串 ；
        list.Sort();
        StringBuilder prestr = new StringBuilder();

        for (int i = 0; i < list.Count; i++)
        {
            if (String.IsNullOrEmpty(list[i].ToString()))
            {
                continue;
            }

            if (Request.Form[list[i].ToString()] != "" && !list[i].ToString().Equals("sign"))
            {
                prestr.Append("" + list[i].ToString()+ "" + "=" + Request.Form[list[i].ToString()].ToString().Trim() + "&");
            }
        }
        
        prestr.Append("key=" + key);
        string mysign = GetMD5(prestr.ToString().Trim(),input_charset).ToLower();

        return mysign;
    }

    //生成时间戳
    public string GetTmstamp()
    {
        DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(197011));
        DateTime dtNow = DateTime.Parse(DateTime.Now.ToString());
        string tmstamp = (dtNow - dtStart).TotalSeconds.ToString();
        return tmstamp;
    }

    /// <summary>
    /// 获取大写的MD5签名结果
    /// </summary>
    /// <param name="encypStr"></param>
    /// <returns></returns>
    private string GetMD5(string encypStr, string charset)
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

    public void ResponseToDistination(Users user,string id)
    {
        if (Shove._Web.Cache.GetCache("UserQQBind_" + user.ID.ToString()) != null)
        {
            Shove._Web.Cache.ClearCache("UserQQBind_" + user.ID.ToString());
            if (Shove._Convert.StrToLong(id, 0) < 1)
            {
                Shove._Web.JavaScript.Alert(this.Page, "您输入的 QQ 号码不合法！");

                return;
            }

            System.Threading.Thread.Sleep(500);

            int ReturnValue = -1;
            string ReturnDescription = "";

            user.isQQValided = true;
            user.QQ = id;

            ReturnValue = user.EditByID(ref ReturnDescription);

            if (ReturnValue < 0)
            {
                Shove._Web.JavaScript.Alert(this.Page, ReturnDescription);

                return;
            }
            Shove._Web.JavaScript.Alert(this.Page, "QQ号码绑定成功。", "UserQQBind.aspx");
        }
        else
        {
            Response.Redirect("../../Default.aspx", true);
        }
    }
}