using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Text;
using System.Text.RegularExpressions;

public partial class Home_Room_PromoteCpsReg : SitePageBase
{
    private  string KeyPromotionUserID = "SLS.TWZT.PromotionUserID";
    protected void Page_Load(object sender, EventArgs e)
    {
        AjaxPro.Utility.RegisterTypeForAjax(typeof(Home_Room_PromoteCpsReg), this.Page);

        if (!IsPostBack)
        {
            bool isUseCheckCode = _Site.SiteOptions["Opt_isUseCheckCode"].ToBoolean(true);
            CheckCode1.Visible = isUseCheckCode;
            CheckCode2.Visible = isUseCheckCode;

            new Login().SetCheckCode(_Site, ShoveCheckCode1);
            new Login().SetCheckCode(_Site, ShoveCheckCode2);

            if (_User != null)
            {
                Response.Redirect("../../Default.aspx");
            }

            //绑定[中奖公告]数据
            BindAffiches();
             
            //处理推荐
            SetCommenderName("");
            pnlShowPromotionInfo.Visible = false;
            pnlShowErrorInfo.Visible = false;

            lblInputError.Visible = false;
            lblInputError.Text = "";
            long commenderID = -1;
            string paramID = Shove._Web.Utility.GetRequest("id");
            if(string.IsNullOrEmpty(paramID)||paramID.Length != 11)
            {
                pnlShowErrorInfo.Visible = true;
                lblShowErrorInfo.Text="无效的推荐人ID.用户可以正常注册站长商家!";
            }
            else //"推荐站长"
            {
                commenderID = Shove._Convert.StrToLong(paramID.Substring(0, 10), -1);
                object tempConnenderName = Shove.Database.MSSQL.ExecuteScalar("select name from T_Users where ID=" + commenderID, new Shove.Database.MSSQL.Parameter[0]);
                if (tempConnenderName == null || tempConnenderName.ToString() == "")
                {
                    pnlShowErrorInfo.Visible = true;
                    lblShowErrorInfo.Text = "不存在推荐人ID.用户可以正常注册站长商家!";
                }
                else
                {
                    pnlShowPromotionInfo.Visible = true;
                    SetCommenderName(tempConnenderName.ToString());
                }
            }
            if (commenderID > 0)
            {
                // 把推荐会员ID写入Session
                Session[KeyPromotionUserID] = commenderID.ToString();
            }
        }

    }

    //绑定[中奖公告]数据
    private void BindAffiches()
    {
        //时时彩中奖
        BindWinUsers();

        //中奖公告
        string CacheKey = "Home_Room_PromoteUserReg_BindAffiches";

        DataTable dt = Shove._Web.Cache.GetCacheAsDataTable(CacheKey);

        if (dt == null)
        {
            string strCmd = "select top 7 * from V_News where isShow=1 and SiteID=1 and [TypeName]='中奖公告'  order by datetime desc";

            dt = Shove.Database.MSSQL.Select(strCmd);

            if (dt == null)
            {
                PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", this.GetType().BaseType.FullName);

                return;
            }

            Shove._Web.Cache.SetCache(CacheKey, dt);
        }

        rptWinAffiches.DataSource = dt;
        rptWinAffiches.DataBind();
    }

    private void BindWinUsers()
    {
        string Key = "Home_Room_PromoteUserReg_BindWinUsers";

        DataTable dt = Shove._Web.Cache.GetCacheAsDataTable(Key);

        StringBuilder sb = new StringBuilder();

        if (dt == null)
        {
            sb.AppendLine("select b.Name as UserName,c.Name as PlayName,WinMoneyNoWithTax from")
                .AppendLine("(select top 27 InitiateUserID,PlayTypeID,WinMoneyNoWithTax from T_Schemes a inner join")
                .AppendLine("T_Isuses b on a.IsuseID = b.ID  and  WinMoneyNoWithTax > 0 and  LotteryID = 62  order by DateTime desc)a")
                .AppendLine("inner join T_Users b on a.InitiateUserID = b.ID inner join T_PlayTypes c on a.PlayTypeID = c.ID");

            dt = Shove.Database.MSSQL.Select(sb.ToString());

            if (dt == null || dt.Rows.Count == 0)
            {
                return;
            }

            Shove._Web.Cache.SetCache(Key, dt, 300);
        }

        sb = new StringBuilder();

        sb.AppendLine("<div id='scrollWinUsers' style='overflow: hidden; height:100px';>")
            .AppendLine("<table width='100%' border='0' cellspacing='0' cellpadding='0'>");

        foreach (DataRow dr in dt.Rows)
        {
            sb.AppendLine("<tr><td width='33%'class='black12'align='center' style='padding-bottom:5px'>")
                .AppendLine(Shove._String.Cut(dr["UserName"].ToString(), 4))
                .AppendLine("</td>")
                .AppendLine("<td width='33%' class='black12' align='center'>")
                .AppendLine(Shove._String.Cut(dr["PlayName"].ToString(), 4))
                .AppendLine("</td>")
                .AppendLine("<td width='33%' class='red12' align='center'>")
                .AppendLine(Shove._Convert.StrToDouble(dr["WinMoneyNoWithTax"].ToString(), 0).ToString("N")).Append("元").Append("</td></tr>");
        }

        sb.AppendLine("</table>")
            .AppendLine("</div>");

        divWinUsers.InnerHtml = sb.ToString();
    }

    protected void rptWinAffiches_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.EditItem)
        {
            if (e.Item.DataItem == null)
            {
                Response.Write("e.Item.DataItem == null");
                return;
            }
            DataRowView dr = (DataRowView)e.Item.DataItem;

            HyperLink hl = e.Item.FindControl("hlWinAffichesTitle") as HyperLink;

            string title = dr["Title"].ToString();
            hl.Text = Shove._String.HtmlTextCut(title, 10);

            hl.NavigateUrl = dr["Content"].ToString();
        }
    }

    private void SetCommenderName(string name)
    {
        lblCommenderName1.Text = name;
        lblCommenderName2.Text = name;
    }

    protected void btnReg_Click(object sender, EventArgs e)
    {
        //检查输入
        string inputErrorInfo = "";
        if (!PF.CheckUserName(tbUserName.Text))
        {
            inputErrorInfo += "对不起用户名中含有禁止使用的字符.\r\n";
        }

        if (Shove._String.GetLength(tbUserName.Text) < 5 || Shove._String.GetLength(tbUserName.Text) > 16)
        {
            inputErrorInfo += "用户名长度在 5-16 个英文字符或数字、中文 3-8 之间.\r\n";
        }

        if (tbPassword.Text.Length < 6 || tbPassword.Text.Length > 16)
        {
            inputErrorInfo += "密码长度必须在 6-16 位之间.\r\n";
        }

        if (tbSiteName.Text.Trim().Length ==0 )
        {
            inputErrorInfo += "网站名称不能为空.\r\n";
        }
        if (tbSiteURL.Text.Trim().Length == 0)
        {
            inputErrorInfo += "网站地址不能为空.\r\n";
        }

        if (!Shove._String.Valid.isEmail(tbEmail.Text))
        {
            inputErrorInfo += "电子邮件地址格式不正确.\r\n";
        }
        if (!ckbAgree.Checked)
        {
            inputErrorInfo += "必须同意本站会员注册协议才能注册会员。\r\n";
        }

        if (CheckCode2.Visible)
        {
            if (tbCheckCode.Text.Trim() == "")
            {
                inputErrorInfo += "请输入验证码！\n";
            }
            else
            {
                if (!ShoveCheckCode1.Valid(tbCheckCode.Text.Trim()))
                {
                    inputErrorInfo += "验证码输入有误！\n";
                }
            }

        }

        if (inputErrorInfo != "")
        {
            lblInputError.Visible = true;
            lblInputError.Text = "输入资料错误:\r\n" + inputErrorInfo;
            return;
        }

        long CpsID = -1;
        long CommenderID = -1;

        if (Session[KeyPromotionUserID] != null)
        {
            CommenderID = Shove._Convert.StrToLong(Session[KeyPromotionUserID].ToString(), -1);
        }


        //检查推荐人是否为CPS商家,是就把此会员标记CSPID
        object tempOjb = Shove.Database.MSSQL.ExecuteScalar("select ID from T_Cps where OwnerUserID=" + CommenderID, new Shove.Database.MSSQL.Parameter[0]);
        if (tempOjb != null)
        {
            CpsID = Shove._Convert.StrToLong(tempOjb.ToString(), -1);
        }

        System.Threading.Thread.Sleep(500);

        string Name = tbUserName.Text.Trim();
        string Password = tbPassword.Text.Trim();
        string Email = tbEmail.Text.Trim();
        string Mobile = tbTel.Text.Trim();
        string QQ = tbQQ.Text.Trim();


        Users user = new Users(_Site.ID);
        user.Name = Name;
        user.Password = Password;
        user.Email = Email;
        user.Mobile = Mobile;
        user.QQ = QQ;
        user.UserType = 2;


        if (CpsID > 0)//推荐人为cps商家就填CpsID字段
        {
            user.CommenderID = -1;
            user.CpsID = CpsID;
        }
        else
        {
            user.CommenderID = CommenderID;
            user.CpsID = -1;
        }

        string ReturnDescription = "";
        int Result = user.Add(ref ReturnDescription);

        if (Result < 0)
        {
            Shove._Web.JavaScript.Alert(this, ReturnDescription);

            return;
        }
        else //注册成功,加入CPS站长商家
        {
            double cpsBonusScale = 0.00;
            DataTable dtTemp = new DAL.Tables.T_Sites().Open("Opt_CpsBonusScale", "", "");
            if (dtTemp != null && dtTemp.Rows.Count > 0)
                cpsBonusScale = double.Parse(dtTemp.Rows[0]["Opt_CpsBonusScale"].ToString());
            user.cps.SiteID = 1;
            user.cps.CommendID = CommenderID;

            user.cps.Name = tbSiteName.Text;
            user.cps.Url = tbSiteURL.Text;
            user.cps.BonusScale = cpsBonusScale;
            user.cps.ON = true;

            user.cps.Telephone = tbTel.Text.Trim();
            user.cps.Email = Email;
            user.cps.QQ = QQ;
            user.cps.Type = 2;
            user.cps.DomainName = user.GetPromotionURL(0);

            if (user.cps.Add(ref ReturnDescription) < 0)
            {
                Shove._Web.JavaScript.Alert(this, ReturnDescription);

                return;
            }
        }

        Result = user.Login(ref ReturnDescription);

        if (Result < 0)
        {
            Shove._Web.JavaScript.Alert(this, ReturnDescription);

            return;
        }

        Response.Redirect("../../Default.aspx");

        
    }

    /// <summary>
    /// 校验用户是否可用
    /// </summary>
    /// <returns></returns>
    [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.None)]
    public int CheckUserName(string name)
    {
        if (!PF.CheckUserName(name))
        {
            return -1;
        }

        DataTable dt = new DAL.Tables.T_Users().Open("ID", "Name = '" + Shove._Web.Utility.FilteSqlInfusion(name) + "'", "");

        if (dt != null && dt.Rows.Count > 0)
        {
            return -2;
        }

        if (Shove._String.GetLength(name) < 5 || Shove._String.GetLength(name) > 16)
        {
            return -3;
        }

        return 0;
    }

    /// <summary>
    /// 校验注册信息
    /// </summary>
    /// <returns></returns>
    [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.None)]
    public string CheckReg(string name, string password, string email)
    {
        name = name.Trim();
        password = password.Trim();
        email = email.Trim();
        if (!PF.CheckUserName(name))
        {
            return "对不起用户名中含有禁止使用的字符";
        }

        if (Shove._String.GetLength(name) < 5 || Shove._String.GetLength(name) > 16)
        {
            return "用户名长度在 5-16 个英文字符或数字、中文 3-8 之间。";
        }

        if (password.Length < 6 || password.Length > 16)
        {
            return "密码长度必须在 6-16 位之间。";
        }

        if (!Shove._String.Valid.isEmail(email))
        {
            return "电子邮件地址格式不正确。";
        }
        return "";
    }

    protected void btnLogin_Click(object sender, EventArgs e)
    {
        string ReturnDescription = "";

        int Result = new Login().LoginSubmit(this.Page, _Site, tbFormUserName.Text, tbFormUserPassword.Text, tbFormCheckCode.Text, ShoveCheckCode1, ref ReturnDescription);

        if (Result < 0)
        {
            Shove._Web.JavaScript.Alert(this.Page, ReturnDescription);

            return;
        }

        this.Response.Redirect("../../Default.aspx");
    }
}
