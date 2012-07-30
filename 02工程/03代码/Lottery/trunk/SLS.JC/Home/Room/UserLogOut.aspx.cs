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

public partial class Home_Room_UserLogOut : RoomPageBase
{
    public string Balance;
    public string UserName;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (_User != null)
        {
            Balance = _User.Balance.ToString();
            UserName = _User.Name.ToString();

            if (_User.SecurityQuestion.StartsWith("自定义问题|"))
            {
                lbQuestion.Text = _User.SecurityQuestion.Remove(0, 6);
            }
            else
            {
                lbQuestion.Text = _User.SecurityQuestion;
            }

            if (lbQuestion.Text == "")
            {
                lbQuestionInfo.Text = "设置安全保护问题";
            }
            else
            {
                lbQuestionInfo.Text = "修改安全保护问题";
            }
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
        string Reason = tbReason.Text;

        int ReasonID = -1;
        string ReturnDescription = "";

        if (string.IsNullOrEmpty(Reason))
        {
            Shove._Web.JavaScript.Alert(this.Page, "请输入注销原因！");

            return;
        }

        string password = tbPassWord.Text.Trim();

        if (string.IsNullOrEmpty(password))
        {
            Shove._Web.JavaScript.Alert(this.Page, "请输入密码！");

            return;   
        }

        if (_User == null)
        {
            return;
        }

       
        if (this.lbQuestion.Text == "")
        {
            Response.Write("<script type='text/javascript'>alert('为了您的账户安全，请先设置安全保护问题，谢谢！');window.location='SafeSet.aspx?FromUrl=UserLogOut.aspx';</script>");

            return;
        }

        if (PF.EncryptPassword(password) != _User.Password)
        {
            Shove._Web.JavaScript.Alert(this.Page, "请核实您的密码，谢谢！");
            return;
        }

        if (tbMyA.Text.Trim() != _User.SecurityAnswer)
        {
            Shove._Web.JavaScript.Alert(this.Page, "安全保护问题回答错误。");

            return;
        }

        _User.Reason = Reason;
        _User.isCanLogin = false;

        ReasonID = _User.EditByID(ref ReturnDescription);

        if (ReasonID < 0)
        {
            Shove._Web.JavaScript.Alert(this.Page, ReturnDescription);

            return;
        }

        if (_User != null)
        {
            if (_User.Logout(ref ReturnDescription) < 0)
            {
                PF.GoError(ErrorNumber.Unknow, ReturnDescription, this.GetType().FullName);

                return;
            }
        }

        string DefaultURL = ResolveUrl("~/");
        Response.Write("<script language=\"javascript\">try{window.location.href = '" + DefaultURL + "';document.getElementById('HidUserID').value='-1';}catch(e){window.location.href = '" + DefaultURL + "';}</script>");
    }

    protected void btnDownLoad_Click(object sender, EventArgs e)
    {
        string CacheKey = "Home_Room_Invest_BindHistoryData" + _User.ID.ToString();
        DataTable dt = Shove._Web.Cache.GetCacheAsDataTable(CacheKey);

        if (dt == null)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("select * from (select LotteryID,LotteryName,PlayTypeID,InitiateName,PlayTypeName, ")
                .Append("SchemeShare,a.Money,b.Share,b.DetailMoney,SchemeWinMoney, b.WinMoneyNoWithTax,a.DateTime, ")
                .Append("b.SchemeID,QuashStatus,Buyed,AssureMoney,BuyedShare,IsOpened,c.Memo  from   ")
                .Append("(select UserID,SchemeID,SUM(Share) as Share,SUM(DetailMoney) as DetailMoney, ")
                .Append("sum(WinMoneyNoWithTax) as  WinMoneyNoWithTax  from V_BuyDetailsWithQuashedAll   ")
                .Append("where  UserID = " + _User.ID.ToString() + " and InitiateUserID = UserID group by SchemeID,UserID)b ")
                .Append("left join (select * from V_BuyDetailsWithQuashedAll where UserID = " + _User.ID.ToString() + " and   ")
                .Append("UserID = InitiateUserID and isWhenInitiate = 1)a ")
                .Append("on a.UserID = b.UserID and ")
                .Append("a.SchemeID = b.SchemeID  left join (select SchemeID,Memo from T_UserDetails where ")
                .Append("OperatorType = 9 and UserID = " + _User.ID.ToString() + ") c  ")
                .Append("on b.SchemeID = c.SchemeID union select  LotteryID,LotteryName,PlayTypeID,InitiateName, ")
                .Append("PlayTypeName,SchemeShare,a.Money,Share,DetailMoney,SchemeWinMoney, WinMoneyNoWithTax, ")
                .Append("a.DateTime,a.SchemeID,QuashStatus,Buyed,AssureMoney,BuyedShare,IsOpened,b.Memo from  ")
                .Append("(select * from V_BuyDetailsWithQuashedAll where UserID = " + _User.ID.ToString() + " and UserID<>InitiateUserID) a left join (select SchemeID,Memo from T_UserDetails where  ")
                .Append("OperatorType = 9 and UserID = " + _User.ID.ToString() + ")b on a.SchemeID = b.SchemeID)a order by DateTime desc");

            dt = Shove.Database.MSSQL.Select(sb.ToString());

            if (dt == null)
            {
                PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", this.GetType().BaseType.FullName);

                return;
            }

            Shove._Web.Cache.SetCache(CacheKey, dt, 60);
        }
       
        string FileName = "T_Invert.xls";

        HttpResponse response = Page.Response;

        response.AppendHeader("Content-Disposition", "attachment;filename=" + FileName);
        Response.ContentType = "application/ms-excel";
        response.ContentEncoding = System.Text.Encoding.GetEncoding("gb2312");

        foreach (DataColumn dc in dt.Columns)
        {
            response.Write(dc.ColumnName + "\t");
        }

        response.Write("\n");

        foreach (DataRow dr in dt.Rows)
        {
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                response.Write(dr[i].ToString() + "\t");
            }

            response.Write("\n");
        }

        response.End();
    }
}
