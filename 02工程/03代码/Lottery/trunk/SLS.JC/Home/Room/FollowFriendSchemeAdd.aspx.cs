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
using Shove.Database;
using System.Text;
using System.Collections.Generic;

public partial class Home_Room_FollowFriendSchemeAdd : RoomPageBase
{
    public string Source = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            long FollowUserID = 0;
            Source = System.Web.HttpUtility.UrlDecode(Shove._Web.Utility.GetRequest("Source"));

            FollowUserID = Shove._Convert.StrToLong(Shove._Web.Utility.GetRequest("FollowUserID"), -1);
            if (FollowUserID.ToString() == "-1")
            {
                return;
            }

            lbUserName.Text = GetUserName(FollowUserID);
            int LotteryID = Shove._Convert.StrToInt(Shove._Web.Utility.GetRequest("LotteryID"), -1);
            HidNumber.Value = Shove._Web.Utility.GetRequest("Number");

            if (LotteryID == -1)
            {
                LotteryID = 1;
            }

            if (!DataCache.Lotteries.ContainsKey(LotteryID) || FollowUserID < 0 || lbUserName.Text == "")
            {
                PF.GoError(ErrorNumber.Unknow, "参数错误，系统异常。", this.GetType().BaseType.FullName);

                return;
            }

            HidFollowUserID.Value = FollowUserID.ToString();
            int FollowFriendID = Shove._Convert.StrToInt(Shove._Web.Utility.GetRequest("FollowFriendID"), 0);
            BindDataForLottery(LotteryID);
            int DzLotteryID = Shove._Convert.StrToInt(Shove._Web.Utility.GetRequest("DzLotteryID"), -1);
            ddlLotteries.SelectedValue = DzLotteryID.ToString();
            BindFollowList();
        }
    }

    private string GetUserName(long id)
    {
        string sql = "select name from T_Users where id = " + id;
        DataTable dt = Shove.Database.MSSQL.Select(sql);
        if (dt == null || dt.Rows.Count < 1)
        {
            return "";
        }
        else
        {
            return dt.Rows[0][0].ToString();
        }

    }

    #region Web 窗体设计器生成的代码

    override protected void OnInit(EventArgs e)
    {
        RequestLoginPage = this.Request.Url.AbsoluteUri;
        isRequestLogin = false;
        base.OnInit(e);
    }

    #endregion

    private void BindDataForLottery(int LotteryID)
    {
        string CacheKey = "dtLotteriesUseLotteryList";
        DataTable dtLotteries = Shove._Web.Cache.GetCacheAsDataTable(CacheKey);

        if (dtLotteries == null)
        {
            dtLotteries = new DAL.Tables.T_Lotteries().Open("[ID], [Name], [Code]", "[ID] in(" + (_Site.UseLotteryList == "" ? "-1" : _Site.UseLotteryList) + ")", "[ID]");

            if (dtLotteries == null)
            {
                PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", this.GetType().BaseType.FullName + "(-39)");

                return;
            }

            Shove._Web.Cache.SetCache(CacheKey, dtLotteries, 6000);
        }

        if (dtLotteries == null)
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", this.GetType().BaseType.FullName + "(-49)");

            return;
        }

        ddlLotteries.Items.Clear();
        ddlLotteries.Items.Add(new ListItem("全部彩种", "-1"));

        foreach (DataRow dr in dtLotteries.Rows)
        {
            string strLotteryName = dr["Name"].ToString();
            if (dr["ID"].ToString() == "61")
            {
                strLotteryName = strLotteryName.Replace("江西", "");
            }

            ddlLotteries.Items.Add(new ListItem(strLotteryName, dr["ID"].ToString()));
        }

        if (ddlLotteries.Items.FindByValue(LotteryID.ToString()) != null)
        {
            ddlLotteries.SelectedValue = LotteryID.ToString();
        }

        ddlLotteries_SelectedIndexChanged(ddlLotteries, new EventArgs());
    }

    protected void ddlLotteries_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlLotteries.SelectedValue == "-1")
        {
            ddlPlayTypes.Items.Clear();
            ddlPlayTypes.Items.Add(new ListItem("全部玩法", "-1"));

            return;
        }

        //玩法信息缓存 6000 秒
        string CacheKey = "dtPlayTypes_" + ddlLotteries.SelectedValue;
        DataTable dtPlayTypes = Shove._Web.Cache.GetCacheAsDataTable(CacheKey);

        if (dtPlayTypes == null)
        {
            dtPlayTypes = new DAL.Tables.T_PlayTypes().Open("", "LotteryID in (" + (_Site.UseLotteryList == "" ? "-1" : _Site.UseLotteryList) + ") and LotteryID = " + Shove._Convert.StrToInt(ddlLotteries.SelectedValue, -1).ToString(), "[ID]");

            if (dtPlayTypes == null || dtPlayTypes.Rows.Count < 1)
            {
                PF.GoError(ErrorNumber.NoData, "数据库繁忙，请重试", this.GetType().FullName + "(-85)");

                return;
            }

            Shove._Web.Cache.SetCache(CacheKey, dtPlayTypes, 6000);
        }

        if (dtPlayTypes == null)
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", this.GetType().BaseType.FullName + "(-95)");

            return;
        }

        ddlPlayTypes.Items.Clear();
        ddlPlayTypes.Items.Add(new ListItem("全部玩法", "-1"));

        foreach (DataRow dr in dtPlayTypes.Rows)
        {
            ddlPlayTypes.Items.Add(new ListItem(dr["Name"].ToString(), dr["ID"].ToString()));
        }

        if (ddlLotteries.SelectedValue == "1" && HidNumber.Value == "9")
        {
            ddlPlayTypes.SelectedValue = "103";
        }
    }

    protected void btn_OK_Click(object sender, EventArgs e)
    {
        if (_User == null)
        {
            return;
        }
        double MoneyStart = 0;
        double MoneyEnd = 0;
        int BuyShareStart = 1;
        int BuyShareEnd = 1;
        if (Source == "1")
        {
            if (lbUserName.Text == "")
            {
                Shove._Web.JavaScript.Alert(this.Page, "对不起，用户访问错误.");
                Response.Write("<scrpit type=\"text/javascript\" language=\"javascript\">top.location = \"UserEdit.aspx\" </script>");
                return;
            }
        }

        if (string.IsNullOrEmpty(_User.IDCardNumber) || string.IsNullOrEmpty(_User.RealityName))
        {
            Response.Write("<script>alert('对不起，您的身份证号码或者真实姓名没有填写，为了不影响您的领奖，请先完善这些资料。谢谢');parent.location.href ='UserEdit.aspx';</script>");
            return;
        }

        try
        {
            MoneyEnd = double.Parse(tbMaxMoney.Text);
            MoneyStart = double.Parse(tbMinMoney.Text);
        }
        catch
        {
            Shove._Web.JavaScript.Alert(this.Page, "输入有误");

            return;
        }

        if (MoneyEnd < MoneyStart)
        {
            Shove._Web.JavaScript.Alert(this.Page, "输入有误，最低金额不能大于最高金额");

            return;
        }

        BuyShareStart = Shove._Convert.StrToInt(tbBuyShareStart.Text, -1);
        BuyShareEnd = Shove._Convert.StrToInt(tbBuyShareEnd.Text, -1);

        if (BuyShareStart < 1 || BuyShareEnd < 1 || BuyShareStart > BuyShareEnd)
        {
            Shove._Web.JavaScript.Alert(this.Page, "份数输入有误");

            return;
        }

        long count = new DAL.Tables.T_CustomFriendFollowSchemes().GetCount("LotteryID in(" + ddlLotteries.SelectedValue + ",-1) and PlayTypeID in(" + ddlPlayTypes.SelectedValue + ",-1) and UserID=" + _User.ID.ToString() + " and FollowUserID=" + HidFollowUserID.Value + "");

        if (count > 0)
        {
            Shove._Web.JavaScript.Alert(this.Page, "您已经订制过此好友的跟单！");

            return;
        }

        if (_User.ID == Shove._Convert.StrToLong(HidFollowUserID.Value, 0))
        {
            Shove._Web.JavaScript.Alert(this.Page, "您不能定制自己的跟单!");

            return;
        }

        DAL.Tables.T_CustomFriendFollowSchemes tcffs = new DAL.Tables.T_CustomFriendFollowSchemes();

        tcffs.SiteID.Value = 1;
        tcffs.UserID.Value = _User.ID;
        tcffs.FollowUserID.Value = HidFollowUserID.Value;
        tcffs.LotteryID.Value = ddlLotteries.SelectedValue;
        tcffs.PlayTypeID.Value = ddlPlayTypes.SelectedValue;
        tcffs.MoneyStart.Value = MoneyStart;
        tcffs.MoneyEnd.Value = MoneyEnd;
        tcffs.BuyShareStart.Value = BuyShareStart;
        tcffs.BuyShareEnd.Value = BuyShareEnd;
        tcffs.Type.Value = 1;

        long l = tcffs.Insert();

        Shove._Web.JavaScript.Alert(this.Page, "定制跟单成功");
        if (l > 0)
        {
            //刷新缓存
            string CacheKey = "T_CustomFriendFollowSchemes" + ddlLotteries.SelectedValue;
            Shove._Web.Cache.ClearCache(CacheKey);

            if (ddlLotteries.SelectedValue == "-1")
            {
                foreach (KeyValuePair<int, string> kvp in DataCache.Lotteries)
                {
                    CacheKey = "T_CustomFriendFollowSchemes" + kvp.Key.ToString();

                    Shove._Web.Cache.ClearCache(CacheKey);
                }
            }

            Response.Redirect("FollowScheme.aspx?LotteryID=" + Shove._Convert.StrToInt(Shove._Web.Utility.GetRequest("LotteryID"), 5) + "&Go=-1", true);

            return;
        }
        else
        {
            PF.GoError(ErrorNumber.Unknow, "数据库繁忙，请重试", this.GetType().BaseType.FullName);

            return;
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("FollowFriendScheme.aspx?LotteryID=" + Shove._Convert.StrToInt(Shove._Web.Utility.GetRequest("LotteryID"), 5), true);
    }

    private void BindFollowList()
    {
        DataTable dt = Shove._Web.Cache.GetCacheAsDataTable("FollowList");

        if (dt == null)
        {
            string strCmd = "select top 10 t1.FollowUserID,t1.FollowCount,t2.Name,(select LotteryID from (select top 1 LotteryID,sum(DetailMoney) as DetailMoney from V_BuyDetails as s where s.UserID=t2.ID group by LotteryID order by DetailMoney desc) as W) as LotteryID from (select COUNT(FollowUserID) as FollowCount,FollowUserID from T_CustomFriendFollowSchemes group by FollowUserID) as t1 inner join T_Users as t2 on t1.FollowUserID=t2.ID order by  t1.FollowCount desc";
            dt = MSSQL.Select(strCmd);

            if (dt == null)
            {
                PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", this.GetType().BaseType.FullName);

                return;
            }

            if (dt.Rows.Count > 0)
            {
                Shove._Web.Cache.SetCache("FollowList", dt, 60);
            }
        }

        StringBuilder sb = new StringBuilder();

        if (dt.Rows.Count == 0)
        {
            sb.AppendLine("<tr>")
                .AppendLine("<td height=\"20\" colspan=\"3\" align=\"center\" class=\"blue12\">")
                .AppendLine("暂无数据")
                .AppendLine("</td>")
                .AppendLine("</tr>");
        }
        else
        {
            int i = 0;
            foreach (DataRow dr in dt.Rows)
            {
                int lotteryid = 0;
                if (Shove._Convert.StrToInt(dr["LotteryID"] == null ? "" : dr["LotteryID"].ToString(), 0) == 0)
                {
                    lotteryid = 5;
                }
                else
                {
                    lotteryid = Shove._Convert.StrToInt(Shove._Web.Utility.FilteSqlInfusion(dr["LotteryID"].ToString()), 0);
                }
                string UserName = dr["Name"].ToString();
                string url = "";
                string sql = "SELECT TOP 1 ID FROM dbo.V_Schemes with (nolock) WHERE SiteID =@SiteID AND InitiateUserID = @InitiateUserID AND QuashStatus = 0 AND isOpened = 0 ORDER BY Money desc,Schedule desc,  CONVERT(Datetime,[Datetime]) desc ";
                DataTable dtSchemes = MSSQL.Select(sql,
                    new MSSQL.Parameter("SiteID", SqlDbType.Int, 0, ParameterDirection.Input, _Site.ID),
                    new MSSQL.Parameter("InitiateUserID", SqlDbType.Int, 0, ParameterDirection.Input, Shove._Web.Utility.FilteSqlInfusion(dr["FollowUserID"].ToString())));
                if (dtSchemes != null && dtSchemes.Rows.Count > 0)
                {
                    if (dtSchemes.Rows.Count == 1)
                    {
                        url = "<a href='" + Shove._Web.Utility.GetUrl() + "/Home/Room/Scheme.aspx?id=" + dtSchemes.Rows[0]["ID"].ToString() + "' target='_blank'>查看</a>";
                    }
                }
                i++;
                sb.AppendLine("<tr>")
                    .Append("<td width=\"8%\" height=\"28\" align=\"center\" bgcolor=\"#FFFFFF\" class=\"blue12\">")
                    .Append("<img src=\"images/num_" + i.ToString() + ".gif\" width=\"13\" height=\"13\" />")
                    .AppendLine("</td>")
                    .Append("<td width=\"30%\" height=\"28\" align=\"center\" bgcolor=\"#FFFFFF\" class=\"blue12\" title='" + UserName + "'>")
                    .Append("<a href='..\\Web\\Score.aspx?id=")
                    .Append(dr["FollowUserID"].ToString())
                    .Append("&LotteryID=" + lotteryid.ToString() + "' target='_blank'>")
                    //.Append(Shove._String.Cut(UserName, 4))
                    .Append(UserName)
                    .Append("</a>")
                    .AppendLine("</td>")
                    .Append("<td width=\"11%\"  bgcolor=\"#FFFFFF\" align=\"center\" class=\"blue12\">");
                sb.Append(dr["FollowCount"].ToString())
                    .AppendLine("</td>")
                    .Append("<td width=\"22%\"  bgcolor=\"#FFFFFF\" class=\"red12_2\" align='center'>");

                sql = "Select SUM(DetailMoney) AS DetailMoney From T_BuyDetails b "+
                      "Left Join T_CustomFriendFollowSchemes c on b.UserID = c.UserID "+
                      "Left Join T_Schemes s on b.SchemeID = s.ID "+
                      "where s.InitiateUserID = @UserID and c.FollowUserID = @UserID";
                DataTable dtTotalFollowMoney = MSSQL.Select(sql,
                    new MSSQL.Parameter("UserID",SqlDbType.BigInt,0,ParameterDirection.Input,Shove._Web.Utility.FilteSqlInfusion(dr["FollowUserID"].ToString())));
                if (dtTotalFollowMoney != null && dtTotalFollowMoney.Rows.Count > 0)
                {
                    sb.Append(Shove._Convert.StrToDouble(dtTotalFollowMoney.Rows[0]["DetailMoney"].ToString(), 0).ToString("N0"));
                }
                sb.AppendLine("</td>")
                 .Append("<td width=\"20%\" height=\"28\" align=\"center\" bgcolor=\"#FFFFFF\" class=\"blue12\">");
                if (url != "")
                {
                    sb.Append(url);
                }
                else
                {
                    sb.Append("无");
                }
                sb.Append("</td>")
                    .Append("<td width=\"9%\"  bgcolor=\"#FFFFFF\" class=\"red12_2\" align='center'>");
                sb.Append("<a  href='FollowFriendSchemeAdd.aspx?Source=1&FollowUserID=" + dr["FollowUserID"].ToString() + "&FollowUserName=" + HttpUtility.UrlEncode(UserName) + "&LotteryID=-1'>定制</a>")
                    .AppendLine("</td>")
                    .AppendLine("</tr>");

            }
        }

        tbFollowList.InnerHtml += sb.ToString();
    }
    protected void btn_Single_Click(object sender, ImageClickEventArgs e)
    {
        Shove._Web.JavaScript.Alert(this.Page, "xxx");
    }
}