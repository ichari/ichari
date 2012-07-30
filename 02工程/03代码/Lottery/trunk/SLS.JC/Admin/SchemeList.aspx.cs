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

using Shove.Database;

public partial class Admin_SchemeList : AdminPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindDataForUser();

            BindDataForLottery();

            BindDataForIsuse();
        }
    }

    #region Web 窗体设计器生成的代码

    override protected void OnInit(EventArgs e)
    {
        RequestLoginPage = this.Request.Url.AbsoluteUri;

        RequestCompetences = Competences.BuildCompetencesList(Competences.LotteryIsuseScheme);

        base.OnInit(e);
    }

    #endregion

    private void BindDataForUser()
    {
        DataTable dt = new DAL.Tables.T_Users().Open("[ID], [Name]", "SiteID = " + _Site.ID.ToString() + " and [ID] in (select distinct UserID from T_CompetencesOfUsers union all select distinct UserID from T_UserInGroups)", "[ID]");

        if (dt == null)
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", "Admin_SchemeList");

            return;
        }

        ddlUser.Items.Add(new ListItem("全部操作员", "-1"));

        foreach (DataRow dr in dt.Rows)
        {
            ddlUser.Items.Add(new ListItem(dr["Name"].ToString(), dr["ID"].ToString()));
        }

        ddlUser.SelectedIndex = 0;
    }

    private void BindDataForLottery()
    {
        DataTable dt = new DAL.Tables.T_Lotteries().Open("[ID], [Name]", "[ID] in (" + (_Site.UseLotteryListRestrictions == "" ? "-1" : _Site.UseLotteryListRestrictions) + ")", "[Order]");

        if (dt == null)
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", "Admin_SchemeList");

            return;
        }

        Shove.ControlExt.FillListBox(listLottery, dt, "Name", "ID");
    }

    private void BindDataForIsuse()
    {
        if (listLottery.Items.Count < 1)
        {
            return;
        }

        DataTable dt = new DAL.Tables.T_Isuses().Open("", "StartTime < GetDate() and LotteryID = " + Shove._Web.Utility.FilteSqlInfusion(listLottery.SelectedValue), "EndTime desc");

        if (dt == null)
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", "Admin_SchemeList");

            return;
        }

        listIsuse.Items.Clear();

        Shove.ControlExt.FillListBox(listIsuse, dt, "Name", "ID");
    }

    protected void ddlLottery_SelectedIndexChanged(object sender, System.EventArgs e)
    {
        BindDataForIsuse();

        BindData();
    }

    protected void listIsuse_SelectedIndexChanged(object sender, System.EventArgs e)
    {
        BindData();
    }

    protected void ddlUser_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindData();
    }

    private void BindData()
    {
        if (listIsuse.Items.Count < 1)
        {
            return;
        }

        int Type = 1;
        try
        {
            Type = int.Parse(this.ViewState["Admin_SchemeList_Type"].ToString());
        }
        catch
        {
            Type = 1;
        }

        if ((Type < 1) || (Type > 7))
        {
            PF.GoError(ErrorNumber.Unknow, "参数错误", "Admin_SchemeList");

            return;
        }

        DataTable dt = null;
        string Condition = " SiteID = " + _Site.ID.ToString() + " and IsuseID = " + listIsuse.SelectedValue;

        if (ddlUser.Enabled)
        {
            int Operator_id = int.Parse(ddlUser.SelectedValue);

            if (Operator_id >= 0)
            {
                Condition += " and BuyOperatorID = " + ddlUser.SelectedValue;
            }
        }

        switch (Type)
        {
            case 1:
                dt = MSSQL.Select("select * from V_SchemeSchedulesWithQuashed with (nolock) where " + Condition + " order by  [DateTime] desc");
                break;
            case 2:
                dt = MSSQL.Select("select * from V_SchemeSchedulesWithQuashed with (nolock) where " + Condition + " and Buyed=1 order by [Money] desc");
                break;
            case 3:
                dt = MSSQL.Select("select * from V_SchemeSchedulesWithQuashed with (nolock) where " + Condition + " and Buyed=0 order by [DateTime] desc");
                break;
            case 4:
                dt = MSSQL.Select("select * from V_SchemeSchedulesWithQuashed with (nolock) where " + Condition + " and QuashStatus<>0 order by [DateTime] desc");
                break;
            case 5:
                dt = MSSQL.Select("select * from V_SchemeSchedulesWithQuashed with (nolock) where " + Condition + " and QuashStatus = 2 order by [DateTime] desc");
                break;
            case 6:
                dt = MSSQL.Select("select * from V_SchemeSchedulesWithQuashed with (nolock) where " + Condition + " and WinMoney > 0 and Buyed=1 order by [DateTime] desc");
                break;
            case 7:
                dt = MSSQL.Select("select * from V_SchemeSchedulesWithQuashed with (nolock) where " + Condition + " and WinMoney > 0 and Buyed=0 order by [DateTime] desc");
                break;
        }

        if (dt == null)
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", "Admin_SchemeList");

            return;
        }

        PF.DataGridBindData(g, dt, gPager);
    }

    protected void RadTypeClick(object sender, System.EventArgs e)
    {
        int Type = Shove._Convert.StrToInt(((RadioButton)sender).ID.Substring(8, 1), 1);
        this.ViewState["Admin_SchemeList_Type"] = Type;

        ddlUser.Enabled = (Type == 2);

        BindData();
    }

    protected void g_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.EditItem)
        {
            e.Item.Attributes["onmouseover"] = "this.name=this.style.backgroundColor;this.style.backgroundColor='MistyRose'";
            e.Item.Attributes["onmouseout"] = "this.style.backgroundColor=this.name;";

            e.Item.Cells[1].Text = "<a href='../Home/Room/Scheme.aspx?id=" + e.Item.Cells[11].Text + "' target='_blank'>" + e.Item.Cells[1].Text + "</a>";

            e.Item.Cells[2].Text = "<a href='../Home/Web/Score.aspx?id=" + e.Item.Cells[12].Text + "&LotteryID=" + listLottery.SelectedValue + "' target='_blank'>" + e.Item.Cells[2].Text + "</a>";
            
            if (Shove._Convert.StrToDouble(e.Item.Cells[10].Text, 0) > 0)
            {
                e.Item.Cells[2].Text += "<font color=\'red\'>(保)</font>";
            }

            double money;

            money = Shove._Convert.StrToDouble(e.Item.Cells[4].Text, 0);
            e.Item.Cells[4].Text = (money == 0) ? "" : money.ToString("N");

            e.Item.ToolTip = e.Item.Cells[5].Text.Trim();
            
            string str = e.Item.Cells[5].Text;
            e.Item.Cells[5].Text = "";

            if (new SLS.Lottery.ZCDC().CheckPlayType(Shove._Convert.StrToInt(e.Item.Cells[14].Text, -1)))
            {
                string vote = "";
                DataTable dtnew = PF.GetZCDCBuyContent(str, Shove._Convert.StrToLong(e.Item.Cells[11].Text, -1), ref vote);

                if (dtnew == null)
                {
                    PF.GoError(ErrorNumber.DataReadWrite, "数据访问错误", this.GetType().BaseType.FullName);

                    return;
                }

                foreach (DataRow dr in dtnew.Rows)
                {
                    if (e.Item.Cells[5].Text == "")
                    {
                        e.Item.Cells[5].Text += dr["No"].ToString() + " " + dr["LeagueTypeName"].ToString() + " " + dr["HostTeam"].ToString() + " VS " + dr["QuestTeam"].ToString() + " " + dr["Content"].ToString() + " " + dr["LotteryResult"].ToString();
                    }
                    else
                    {
                        e.Item.Cells[5].Text += "<br />" + dr["No"].ToString() + " " + dr["LeagueTypeName"].ToString() + " " + dr["HostTeam"].ToString() + " VS " + dr["QuestTeam"].ToString() + " " + dr["Content"].ToString() + " " + dr["LotteryResult"].ToString();
                    }
                }

                e.Item.Cells[5].Text += "<br /><font color='red'>" + vote + "</font>";
            }
            else
            {
                e.Item.Cells[5].Text = str;

                if (Shove._String.StringAt(str, '\n') >= _Site.SiteOptions["Opt_MaxShowLotteryNumberRows"].ToShort(0) || (listLottery.SelectedValue == "72") || (listLottery.SelectedValue == "73"))
                {
                    e.Item.Cells[5].Text = "<a href='../Home/Web/DownLoadSchemeFile.aspx?id=" + e.Item.Cells[11].Text + "' target='_blank'>下载方案</a>";
                }
                else
                {
                    str = e.Item.Cells[5].Text.Replace("\r\n", ", ");

                    if (str.Length > 25)
                    {
                        str = str.Substring(0, 23) + "..";
                    }

                    e.Item.Cells[5].Text = str;
                }
            }

            int Share = Shove._Convert.StrToInt(e.Item.Cells[6].Text, 1);

            e.Item.Cells[7].Text = Math.Round(money / Share, 2).ToString("N");
            e.Item.Cells[8].Text += "%";

            short QuashStatus = Shove._Convert.StrToShort(e.Item.Cells[13].Text, 0);

            if (QuashStatus == 2)
            {
                e.Item.Cells[9].Text = "<font color='blue'>系统撤单</font>";
            }
            else if (QuashStatus == 1)
            {
                e.Item.Cells[9].Text = "已撤单";
            }
            else
            {
                bool Buyed = Shove._Convert.StrToBool(e.Item.Cells[16].Text, false);

                if (Buyed)
                {
                    e.Item.Cells[9].Text = "<Font color=\'Red\'>已成功</font>";
                }
                else
                {
                    if (e.Item.Cells[8].Text == "100%")
                    {
                        e.Item.Cells[9].Text = "<Font color=\'Red\'>满员</font>";
                    }
                    else
                    {
                        e.Item.Cells[9].Text = "未满";
                    }
                }
            }
        }
    }

    protected void gPager_PageWillChange(object Sender, Shove.Web.UI.PageChangeEventArgs e)
    {
        BindData();
    }

    protected void gPager_SortBefore(object source, DataGridSortCommandEventArgs e)
    {
        BindData();
    }
}
