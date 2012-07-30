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

public partial class Home_Room_CoBuy : RoomPageBase
{
    protected StringBuilder script = new StringBuilder();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            HidIsuseID.Value = Shove._Web.Utility.GetRequest("IsuseID");
            HidLotteryID.Value = Shove._Web.Utility.GetRequest("LotteryID");
            HidNumber.Value = Shove._Web.Utility.GetRequest("Number");
            BindPersonages();

            script.AppendLine("__doPostBack('btnSearch', '');");
        }
        else
        {
            script.AppendLine("document.getElementById(\"g\").setAttribute(\"border\", \"1\");")
                .AppendLine("document.getElementById(\"g\").removeAttribute(\"style\");")
                .AppendLine("document.getElementById(\"g\").setAttribute(\"width\", \"100%\");");
        }
    }

    #region Web 窗体设计器生成的代码

    override protected void OnInit(EventArgs e)
    {
        RequestLoginPage = this.Request.Url.AbsoluteUri;

        isRequestLogin = false;
        isAtFramePageLogin = false;

        isAllowPageCache = true;
        PageCacheSeconds = 60;

        base.OnInit(e);
    }

    #endregion

    private void BindDataForType()
    {
        long IsuseID = Shove._Convert.StrToLong(Shove._Web.Utility.FilteSqlInfusion(HidIsuseID.Value), 0);

        //合买方案缓存 60 秒
        string CacheKey = "Home_Room_CoBuy_BindDataForType" + HidIsuseID.Value;
        DataTable dt = null;

        if (TxtName.Text.Trim() == "" || TxtName.Text.Trim() == "输入用户名")           //2010-7-9添加的判断
        {
            dt = Shove._Web.Cache.GetCacheAsDataTable(CacheKey);
        }

        if (dt == null)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("select SchemeNumber, InitiateName, Level, Money, d.Name as PlayTypeName, Share, Schedule, AssureMoney, a.ID, InitiateUserID, QuashStatus, PlayTypeID, Buyed, SecrecyLevel, EndTime, b.IsOpened, LotteryNumber,b.LotteryID ")
                .AppendLine("from ")
                .AppendLine("	(")
                .AppendLine("		select T_Schemes.ID,IsuseID,AtTopStatus,T_Users.Name as InitiateName, T_Users.Level, SchemeNumber,ReSchedule,Money,Share, Schedule, AssureMoney, InitiateUserID, QuashStatus, PlayTypeID, Buyed, SecrecyLevel, LotteryNumber ")
                .AppendLine("		from T_Schemes  left join T_Users  on T_Schemes.InitiateUserID = T_Users.id")
                .AppendLine("		where IsuseID = @IsuseID and QuashStatus = 0 and Share > BuyedShare ")
                .AppendLine("" + ((TxtName.Text.Trim() == "" || TxtName.Text.Trim() == "输入用户名") ? " and not exists (select 1 from T_SchemesTop where T_Schemes.ID = T_SchemesTop.SchemeID) " : (" and T_Users.Name like '%" + Shove._Web.Utility.FilteSqlInfusion(TxtName.Text.Trim()) + "%' ")))
                .AppendLine("			and T_Schemes.SiteID = @SiteID")
                .AppendLine("	)as a")
                .AppendLine("inner join T_Isuses b on a.IsuseID = b.ID")
                .AppendLine("inner join T_PlayTypes d on d.ID = a.PlayTypeID")
                .AppendLine("order by AtTopStatus desc, ReSchedule desc, [Money] desc");

            dt = MSSQL.Select(sb.ToString(),
                new MSSQL.Parameter("IsuseID", SqlDbType.BigInt, 0, ParameterDirection.Input, IsuseID),
                new MSSQL.Parameter("SiteID", SqlDbType.Int, 0, ParameterDirection.Input, _Site.ID));

            if (dt == null)
            {
                PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", this.GetType().FullName);

                return;
            }
        }

        DataTable dtSchemesFormulae = new DAL.Tables.T_SchemesFormulae().Open("", "LotteryID=" + HidLotteryID.Value, "");

        if (dtSchemesFormulae == null)
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", this.GetType().FullName);

            return;
        }

        dt.Columns.Add("IsTop", typeof(System.Int32));

        if ((TxtName.Text.Trim() == "" || TxtName.Text.Trim() == "输入用户名") && dtSchemesFormulae.Rows.Count > 0)
        {
            BindSchemesFormulae();

            DataTable dtSchemesTop = new DAL.Tables.T_SchemesTop().Open("", "IsuseID=" + IsuseID.ToString(), "Sort");

            if (dtSchemesTop == null)
            {
                PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", this.GetType().FullName);

                return;
            }

            DataTable dtSchemes = null;

            if (dtSchemesTop.Rows.Count > 0)
            {
                StringBuilder sb = new StringBuilder();

                sb.AppendLine("select SchemeNumber, InitiateName, Level, Money, d.Name as PlayTypeName, Share, Schedule, AssureMoney, a.ID, InitiateUserID, QuashStatus, PlayTypeID, Buyed, SecrecyLevel, EndTime, b.IsOpened, LotteryNumber,b.LotteryID, 1 as IsTop ")
                    .AppendLine("from ")
                    .AppendLine("	(")
                    .AppendLine("		select T_Schemes.ID,IsuseID,AtTopStatus,T_Users.Name as InitiateName, T_Users.Level, SchemeNumber,ReSchedule,Money,Share, Schedule, AssureMoney, InitiateUserID, QuashStatus, PlayTypeID, Buyed, SecrecyLevel, LotteryNumber ")
                    .AppendLine("		from T_Schemes  left join T_Users  on T_Schemes.InitiateUserID = T_Users.id")
                    .AppendLine("		where IsuseID = @IsuseID and QuashStatus = 0 and Share > BuyedShare ")
                    .AppendLine("           and exists (select 1 from T_SchemesTop where T_Schemes.ID = T_SchemesTop.SchemeID) ")
                    .AppendLine("			and T_Schemes.SiteID = @SiteID")
                    .AppendLine("	)as a")
                    .AppendLine("inner join T_Isuses b on a.IsuseID = b.ID")
                    .AppendLine("inner join T_PlayTypes d on d.ID = a.PlayTypeID")
                    .AppendLine("order by AtTopStatus desc, ReSchedule desc, [Money] desc");

                dtSchemes = MSSQL.Select(sb.ToString(),
                    new MSSQL.Parameter("IsuseID", SqlDbType.BigInt, 0, ParameterDirection.Input, IsuseID),
                    new MSSQL.Parameter("SiteID", SqlDbType.Int, 0, ParameterDirection.Input, _Site.ID));
            }

            int TopCount = 10 - dtSchemesTop.Rows.Count;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (Shove._Convert.StrToDouble(dt.Rows[i]["Money"].ToString(), 0) > Shove._Convert.StrToDouble(dtSchemesFormulae.Rows[0]["Money"].ToString(), 0)
                    && Shove._Convert.StrToFloat(dt.Rows[i]["Schedule"].ToString(), 0) > Shove._Convert.StrToFloat(dtSchemesFormulae.Rows[0]["MinMoney"].ToString(), 0)
                    && ((Shove._Convert.StrToDouble(dt.Rows[i]["AssureMoney"].ToString(), 0) / Shove._Convert.StrToDouble(dt.Rows[i]["Money"].ToString(), 0)) * 100 + Shove._Convert.StrToDouble(dt.Rows[i]["Schedule"].ToString(), 0) > Shove._Convert.StrToDouble(dtSchemesFormulae.Rows[0]["MinMoney"].ToString(), 0))
                    && TopCount < 10)
                {
                    dt.Rows[i]["IsTop"] = 1;
                }
                else
                {
                    dt.Rows[i]["IsTop"] = 0;
                }

                dt.AcceptChanges();
            }

            DataRow dr = null;
            DataRow drSchemes = null;

            if (dtSchemes != null && dtSchemes.Rows.Count > 0)
            {
                for (int i = 0; i < dtSchemesTop.Rows.Count; i++)
                {
                    dr = dt.NewRow();
                    try
                    {
                        drSchemes = dtSchemes.Select("ID=" + dtSchemesTop.Rows[i]["SchemeID"].ToString())[0];
                    }
                    catch
                    { }
                    dr["SchemeNumber"] = drSchemes["SchemeNumber"];
                    dr["InitiateName"] = drSchemes["InitiateName"];
                    dr["Level"] = drSchemes["Level"];
                    dr["Money"] = drSchemes["Money"];
                    dr["PlayTypeName"] = drSchemes["PlayTypeName"];
                    dr["Share"] = drSchemes["Share"];
                    dr["Schedule"] = drSchemes["Schedule"];
                    dr["AssureMoney"] = drSchemes["AssureMoney"];
                    dr["ID"] = drSchemes["ID"];
                    dr["InitiateUserID"] = drSchemes["InitiateUserID"];
                    dr["QuashStatus"] = drSchemes["QuashStatus"];
                    dr["PlayTypeID"] = drSchemes["PlayTypeID"];
                    dr["Buyed"] = drSchemes["Buyed"];
                    dr["SecrecyLevel"] = drSchemes["SecrecyLevel"];
                    dr["EndTime"] = drSchemes["EndTime"];
                    dr["IsOpened"] = drSchemes["IsOpened"];
                    dr["LotteryNumber"] = drSchemes["LotteryNumber"];
                    dr["LotteryID"] = drSchemes["LotteryID"];
                    dr["IsTop"] = drSchemes["IsTop"];

                    dt.Rows.InsertAt(dr, Shove._Convert.StrToInt(dtSchemesTop.Rows[i]["Sort"].ToString(), 0));
                    dt.AcceptChanges();
                }
            }
        }

        dt.Columns.Add("Assure", typeof(System.String));

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            double AssureMoney = Shove._Convert.StrToDouble(dt.Rows[i]["AssureMoney"].ToString(), 0);

            if (AssureMoney > 0)
            {
                dt.Rows[i]["Assure"] = "<Font color=\'red\'>" + dt.Rows[i]["Schedule"].ToString() + "%<br />+" + ((AssureMoney / Shove._Convert.StrToDouble(dt.Rows[i]["Money"].ToString(), 0)) * 100).ToString("N") + "%(保)</ Font>";
            }
            else
            {
                dt.Rows[i]["Assure"] = "<Font color=\'red\'>" + dt.Rows[i]["Schedule"].ToString() + "%</ Font><br />";
            }

            dt.AcceptChanges();
        }

        PF.DataGridBindData(g, dt, gPager);

        if (g.Items.Count == 0)
        {
            string userName = Shove._Web.Utility.FilteSqlInfusion(TxtName.Text.Trim());

            if (userName != "" && userName != "输入用户名" && Personages.InnerHtml.Contains(userName))
            {
                StringBuilder sb = new StringBuilder();

                sb.AppendLine("select SchemeNumber,InitiateName, Level, Money, d.Name as PlayTypeName, Share, Schedule, AssureMoney, a.ID, InitiateUserID, QuashStatus, PlayTypeID, Buyed, SecrecyLevel, EndTime, IsOpened, LotteryNumber,a.LotteryID")
                    .AppendLine("from ")
                    .AppendLine("(")
                    .AppendLine("	select top 5 SchemeNumber, Money, Share, Schedule, AssureMoney, s.ID,u.Name as InitiateName,Level, InitiateUserID, QuashStatus, PlayTypeID, Buyed, SecrecyLevel, EndTime, s.isOpened, LotteryNumber,LotteryID ")
                    .AppendLine("	from T_Schemes  s left join T_Isuses t on s.IsuseID = t.ID ")
                    .AppendLine("	left join T_Users u on s.InitiateUserID = u.ID")
                    .AppendLine("	where s.Share > 1 and t.LotteryID = @LotteryID ")
                    .AppendLine("	and u.Name like @InitiateName and s.SiteID = @SiteID")
                    .AppendLine("   order by QuashStatus asc,[Datetime] desc,[Money] desc")
                    .AppendLine(") as a")
                    .AppendLine("inner join T_PlayTypes d on d.ID = a.PlayTypeID");

                //合买方案缓存 60 秒
                CacheKey = userName + "CoBuySchemes_" + HidLotteryID.Value + "_Top5" + sb;
                dt = Shove._Web.Cache.GetCacheAsDataTable(CacheKey);

                if (dt == null)
                {
                    dt = MSSQL.Select(sb.ToString(),
                        new MSSQL.Parameter("LotteryID", SqlDbType.Int, 0, ParameterDirection.Input, Shove._Convert.StrToLong(Shove._Web.Utility.FilteSqlInfusion(HidLotteryID.Value), 0)),
                        new MSSQL.Parameter("InitiateName", SqlDbType.VarChar, 0, ParameterDirection.Input, "'%" + userName + "%'"),
                        new MSSQL.Parameter("SiteID", SqlDbType.Int, 0, ParameterDirection.Input, _Site.ID));

                    if (dt == null)
                    {
                        PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", this.GetType().FullName);

                        return;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        Shove._Web.Cache.SetCache(CacheKey, dt, 60);
                    }
                }

                dt.Columns.Add("Assure", typeof(System.String));

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    double AssureMoney = Shove._Convert.StrToDouble(dt.Rows[i]["AssureMoney"].ToString(), 0);

                    if (AssureMoney > 0)
                    {
                        dt.Rows[i]["Assure"] = "<Font color=\'red\'>" + dt.Rows[i]["Schedule"].ToString() + "%<br />+" + ((AssureMoney / Shove._Convert.StrToDouble(dt.Rows[i]["Money"].ToString(), 0)) * 100).ToString("N") + "%(保)</ Font>";
                    }
                    else
                    {
                        dt.Rows[i]["Assure"] = "<Font color=\'red\'>" + dt.Rows[i]["Schedule"].ToString() + "%</ Font><br />";
                    }

                    dt.AcceptChanges();
                }

                PF.DataGridBindData(g, dt, gPager);
            }
        }

        divData.Visible = true;
        divLoding.Visible = false;
    }

    protected void gPager_PageWillChange(object Sender, Shove.Web.UI.PageChangeEventArgs e)
    {
        BindDataForType();
    }

    protected void gPager_SortBefore(object source, System.Web.UI.WebControls.DataGridSortCommandEventArgs e)
    {
        BindDataForType();
    }

    protected void g_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.EditItem)
        {
            e.Item.Cells[1].Text = "<font color=\'#000000\'>" + e.Item.Cells[1].Text + "</font>";

            if (Shove._Convert.StrToDouble(e.Item.Cells[22].Text, 0) > 0)
            {
                e.Item.Cells[1].Text = "<font color=\'red\'>[顶]</font>" + e.Item.Cells[1].Text;
            }

            double Level = Shove._Convert.StrToDouble(e.Item.Cells[2].Text, 0);
            if (Level > 0)
            {
                e.Item.Cells[2].Text = "<a href='../Web/Score.aspx?id=" + e.Item.Cells[14].Text + "&LotteryID=" + e.Item.Cells[21].Text + "' target='_blank' title='点击查看历史战绩'>";

                for (int i = 0; i < Level; i++)
                {
                    e.Item.Cells[2].Text += "<img src='Images/gold.gif' border='0'>";
                }
                e.Item.Cells[2].Text += "</a>";
            }
            else
            {
                e.Item.Cells[2].Text = "&nbsp;";
            }

            double money;
            money = Shove._Convert.StrToDouble(e.Item.Cells[3].Text, 0);
            e.Item.Cells[3].Text = (money == 0) ? "&nbsp;" : money.ToString("N");

            short QuashStatus = Shove._Convert.StrToShort(e.Item.Cells[15].Text, 0);
            bool Buyed = Shove._Convert.StrToBool(e.Item.Cells[17].Text, false);
            short SecrecyLevel = Shove._Convert.StrToShort(e.Item.Cells[18].Text, 0);
            bool Stop = (Shove._Convert.StrToDateTime(e.Item.Cells[19].Text, System.DateTime.Now.ToString()) <= System.DateTime.Now);
            bool IsOpened = Shove._Convert.StrToBool(e.Item.Cells[20].Text, false);

            long InitiateUserID = Shove._Convert.StrToLong(e.Item.Cells[14].Text, 0);

            // 投住内容
            //  SecrecyLevel 0 不保密 1 到截止 2 到开奖 3 永远
            if ((SecrecyLevel == SchemeSecrecyLevels.ToDeadline) && !Stop && ((_User == null) || ((_User != null) && (InitiateUserID != _User.ID) && (!_User.isOwnedViewSchemeCompetence()))))
            {
                e.Item.Cells[5].Text = "已保密，截止后公开";
            }
            else if ((SecrecyLevel == SchemeSecrecyLevels.ToOpen) && !IsOpened && ((_User == null) || ((_User != null) && (InitiateUserID != _User.ID) && (!_User.isOwnedViewSchemeCompetence()))))
            {
                e.Item.Cells[5].Text = "已保密，开奖后公开";
            }
            else if ((SecrecyLevel == SchemeSecrecyLevels.Always) && ((_User == null) || ((_User != null) && (InitiateUserID != _User.ID) && (!_User.isOwnedViewSchemeCompetence()))))
            {
                e.Item.Cells[5].Text = "已保密";
            }
            else
            {
                if (e.Item.Cells[5].Text.Trim().Length > 28)
                {
                    e.Item.Cells[5].Text = "<a href='Scheme.aspx?id=" + e.Item.Cells[13].Text + "' target='_blank' title='点击查看方案详细信息'><font color=\"red\">投注内容</font></a>";
                }
                else
                {
                    if ((HidLotteryID.Value == "1" || HidLotteryID.Value == "2" || HidLotteryID.Value == "3") && (string.IsNullOrEmpty(e.Item.Cells[5].Text.Trim()) || e.Item.Cells[5].Text == "&nbsp;"))
                    {
                        e.Item.Cells[5].Text = "<a href='Scheme.aspx?id=" + e.Item.Cells[13].Text + "' target='_blank'><font color=\"red\">未上传方案</font></a>";
                    }
                    else
                    {
                        e.Item.Cells[5].Text = "<a href='Scheme.aspx?id=" + e.Item.Cells[13].Text + "' target='_blank' title='点击查看方案详细信息'>" + e.Item.Cells[5].Text.Trim() + "</a>";
                    }
                }
            }

            int Share = Shove._Convert.StrToInt(e.Item.Cells[6].Text, 1);

            e.Item.Cells[7].Text = Math.Round(money / Share, 2).ToString("N");

            if (QuashStatus != 0)
            {
                if (QuashStatus == 2)
                {
                    e.Item.Cells[9].Text = "<font color='blue'>撤单</font>";
                }
                else
                {
                    e.Item.Cells[9].Text = "撤单";
                }

                e.Item.Cells[10].Text = "--";
            }
            else
            {
                if (Buyed)
                {
                    e.Item.Cells[9].Text = "<Font color=\'Red\'>已成功</font>";
                    e.Item.Cells[10].Text = "--";
                }
                else
                {
                    if (e.Item.Cells[8].Text == "100%")
                    {
                        e.Item.Cells[9].Text = "<Font color=\'Red\'>满员</font>";
                        e.Item.Cells[10].Text = "--";
                    }
                    else
                    {
                        if (Stop) // 历史期
                        {
                            e.Item.Cells[9].Text = "未成功";
                            e.Item.Cells[9].Text = "--";
                        }
                        else
                        {
                            e.Item.Cells[9].Text = "未满";

                            e.Item.Cells[10].Text = "<a href='Scheme.aspx?id=" + e.Item.Cells[13].Text + "' target='_blank' title='点击查看方案详细信息'><font color=\"red\">入伙</font></a>";
                        }
                    }
                }
            }
        }
    }

    private void BindPersonages()
    {
        string Key = "Admin_Personages";

        DataTable dt = Shove._Web.Cache.GetCacheAsDataTable(Key);

        if (dt == null)
        {
            dt = new DAL.Tables.T_Personages().Open("", "", "[Order]");

            if (dt == null || dt.Rows.Count == 0)
            {
                return;
            }

            Shove._Web.Cache.SetCache(Key, dt, 6000);
        }

        DataRow[] drs = dt.Select("IsShow=1 and LotteryID=" + HidLotteryID.Value + "", "[Order]");

        int i = 0;

        System.Text.StringBuilder sb = new StringBuilder();
        //sb.AppendLine("合买：      ");
        sb.AppendLine("<table border=\"0\" height=\"28\">");
        foreach (DataRow dr in drs)
        {
            if (i == 0)
            {
                sb.AppendLine("<tr>");
                sb.AppendLine("<td rowspan=\"2\">合买名人：</td>");
            }
            sb.AppendLine("<td>");
            string html = "<a href='FollowFriendSchemeAdd.aspx?LotteryID=" + HidLotteryID.Value + "&FollowUserID=" + dr["UserID"].ToString() + "&FollowUserName=" + HttpUtility.UrlEncode(dr["UserName"].ToString()) + "' style=' text-decoration:none;padding-left:15px;color:#FF0065'>" + dr["UserName"].ToString() + "</a>";
            sb.AppendLine(html);
            sb.AppendLine("</td>");
            i++;
            if (i == 6)
            {
                break;
            }
            if (i == 3)
            {
                sb.AppendLine("</tr>");
                sb.AppendLine("<tr>");
            }
        }
        sb.AppendLine("</tr></table>");
        Personages.InnerHtml = sb.ToString();
    }

    private void BindSchemesFormulae()
    {
        DataTable dt = new DAL.Tables.T_SchemesFormulae().Open("", "LotteryID=" + HidLotteryID.Value, "");

        if (dt == null)
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", "Admin_SchemesFormulaeSet");

            return;
        }

        if (dt.Rows.Count < 1)
        {
            return;
        }

        lbContent.Text = " 1、 方案金额大于等于" + Shove._Convert.StrToDouble(dt.Rows[0]["Money"].ToString(), 0).ToString("N") + "元<br />2、 进度或者进度+保底大于等于" + dt.Rows[0]["Schedule"].ToString() + "%<br />3、 进度不少于方案总金额的" + dt.Rows[0]["MinMoney"].ToString() + "%";
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        BindDataForType();
    }
}
