using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Text.RegularExpressions;

public partial class Home_Room_OnlinePay_CardPassword_Default : RoomPageBase
{
    public string Balance;
    public string UserName;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (_User != null)
        {
            Balance = _User.Balance.ToString();
            UserName = _User.Name.ToString();
        }
    }

    #region Web 窗体设计器生成的代码

    override protected void OnInit(EventArgs e)
    {
        RequestLoginPage = this.Request.Url.AbsoluteUri;
    
        base.OnInit(e);
    }

    #endregion

    protected void btnOK_Click(object sender, EventArgs e)
    {
        int Freeze = 0;
        int ReturnValue = 0;
        string ReturnDescription = "";

        int Result = DAL.Procedures.P_CardPasswordTryErrorFreeze(_Site.ID, _User.ID, ref Freeze, ref ReturnValue, ref ReturnDescription);

        if ((Result < 0) || (ReturnValue < 0))
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", "Room_ViewChase");

            return;
        }

        if (Freeze > 0)
        {
            Shove._Web.JavaScript.Alert(this.Page, "您输入错误的卡密号码次数过多，系统已经暂时锁定。"+Freeze.ToString()+"分钟后才可以使用卡密支付功能。");

            return;
        }

        string Number = Shove._Convert.ToDBC(tbCardPassword.Text.Trim());

        if (String.IsNullOrEmpty(Number))
        {
            Shove._Web.JavaScript.Alert(this.Page, "请输入充值卡密。");

            return;
        }

        System.Threading.Thread.Sleep(1000);

        if (!Regex.IsMatch(Number, @"^[\d]{20}$", RegexOptions.Compiled | RegexOptions.IgnoreCase))
        {
            ReturnValue = 0;
            ReturnDescription = "";

            DAL.Procedures.P_CardPasswordTryErrorAdd(_User.ID, Number, ref ReturnValue, ref ReturnDescription);

            Shove._Web.JavaScript.Alert(this.Page, "您输入的卡密号码错误!");

            return;
        }

        if (Number.Substring(0, 4) == "1008")
        {
            Response.Redirect("CardPasswordValid.aspx?Num="+Number);

            return;
        }

        ReturnDescription = "";

        if (new CardPassword().Use(Number, _Site.ID, _User.ID, ref ReturnDescription) < 0)
        {
            Shove._Web.JavaScript.Alert(this.Page, ReturnDescription);

            return;
        }

        Shove._Web.JavaScript.Alert(this.Page, "卡密充值成功, 请点击“查看我的账户”查看投注卡账户余额情况。");
    }
}
