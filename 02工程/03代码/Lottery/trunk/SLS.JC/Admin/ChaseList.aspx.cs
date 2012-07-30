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
using System.Text;

public partial class Admin_ChaseList : AdminPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindDataForLottery();
            ddlLottery_SelectedIndexChanged(ddlLottery, new EventArgs());
        }
    }

    protected void btnRead_Click(object sender, EventArgs e)
    {
        if (ddlType.SelectedValue == "1")
        {
            if (tbUserName.Text.Trim() == "")
            {
                Shove._Web.JavaScript.Alert(this.Page, "请输入用户名。");

                return;
            }

            Users tu = new Users(_Site.ID)[_Site.ID, tbUserName.Text.Trim()];

            if (tu == null)
            {
                Shove._Web.JavaScript.Alert(this.Page, "用户名不存在。");

                return;
            }

            tbID.Text = tu.ID.ToString();
        }
        else
        {
            if (ddlIsuses.Items.Count == 0)
            {
                Shove._Web.JavaScript.Alert(this.Page, "请选择期号。");

                return;
            }
        }

        BindData();
    }

    #region Web 窗体设计器生成的代码

    override protected void OnInit(EventArgs e)
    {
        RequestLoginPage = this.Request.Url.AbsoluteUri;

        RequestCompetences = Competences.BuildCompetencesList(Competences.LotteryIsuseScheme);

        base.OnInit(e);
    }

    #endregion

    private void BindDataForLottery()
    {
        DataTable dt = new DAL.Tables.T_Lotteries().Open("[ID], [Name]", "[ID] in (" + (_Site.UseLotteryListRestrictions == "" ? "-1" : _Site.UseLotteryListRestrictions) + ") and ID not in(1,2,15)", "[Order]");

        if (dt == null)
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", "Admin_ChaseList");

            return;
        }

        Shove.ControlExt.FillDropDownList(ddlLottery, dt, "Name", "ID");
    }

    private void BindData()
    {
        string Key = "Admin_ChaseList_BindData_" + ddlType.SelectedValue == "1" ? tbID.Text : ddlIsuses.SelectedValue;

        DataTable dt = Shove._Web.Cache.GetCacheAsDataTable(Key);

        if (dt == null)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("select ChaseTaskID, QuashStatus, Executed, SUM(Num) as Num, SUM(Money) as Money into #TempT_ChaseTaskDetails from (select 1 as Num, * from T_ChaseTaskDetails) as a group by ChaseTaskID, QuashStatus, Executed ");
            sb.Append(@"select '我的追号' as Type, tc.ID, tu.Name, tc.DateTime, tl.Name as LotteryName, tc.Title, a.SumMoney, a.SumIsuseNum, a.BuyedIsuseNum, a.QuashedIsuseNum, tc.QuashStatus,2 as StopType, tc.StopWhenWinMoney from T_ChaseTasks tc inner join (
                     select ID, 
                     isnull(sum(isnull(SumMoney, 0)), 0) as SumMoney, 
                     isnull(sum(isnull(SumIsuseNum, 0)), 0) as SumIsuseNum, 
                     isnull(sum(isnull(BuyedIsuseNum, 0)), 0) as BuyedIsuseNum, 
                     isnull(sum(isnull(QuashedIsuseNum, 0)), 0) as QuashedIsuseNum
                     from (
                     select ChaseTaskID as ID, isnull(sum(isnull([Money], 0)), 0) as SumMoney, sum(isnull(Num, 0)) as SumIsuseNum, 0 as BuyedIsuseNum, 0 as QuashedIsuseNum from #TempT_ChaseTaskDetails group by ChaseTaskID
                     union all
                     select ChaseTaskID as ID, 0 as SumMoney, 0 as SumIsuseNum, sum(isnull(Num, 0)) as BuyedIsuseNum, 0 as QuashedIsuseNum from #TempT_ChaseTaskDetails where Executed = 1 and QuashStatus = 0 group by ChaseTaskID
                     union all
                     select ChaseTaskID as ID, 0 as SumMoney, 0 as SumIsuseNum, 0 as BuyedIsuseNum, sum(isnull(Num, 0)) as QuashedIsuseNum from #TempT_ChaseTaskDetails where  QuashStatus <> 0 group by ChaseTaskID
                     ) a group by ID) a
                     on tc.ID = a.ID
                     left join T_Users tu on tc.UserID = tu.ID
                     left join T_Lotteries tl on tc.LotteryID = tl.ID ");

            if (ddlType.SelectedValue == "1")
            {
                sb.Append("where UserID=" + Shove._Web.Utility.FilteSqlInfusion(tbID.Text.Trim()) + " ");
            }
            else
            {
                sb.Append("where ID in(select ChaseTaskID from T_ChaseTaskDetails where IsuseID = " + ddlIsuses.SelectedValue + ") ");
            }

            sb.Append(" union  select '追号套餐' as Type,a.ID,d.Name,DateTime,b.Name,Title,IsuseCount*Multiple*Nums*Price as SumMoney,IsuseCount,ExecutedCount,")
             .Append(" IsuseCount-ExecutedCount as NoExecutedCount,QuashStatus,StopTypeWhenWin,StopTypeWhenWinMoney from T_Chases a inner join T_Lotteries b ")
             .Append(" on a.LotteryID = b.ID ");

            if (ddlType.SelectedValue == "1")
            {
                sb.Append("and UserID=" + Shove._Web.Utility.FilteSqlInfusion(tbID.Text.Trim()) + " ");
            }
            else
            {
                sb.Append("and (a.ID in (select ChaseID from T_ExecutedChases where SchemeID in(select ID from T_Schemes where IsuseID = " + ddlIsuses.SelectedValue + ")) ");

                if (new DAL.Tables.T_Isuses().GetCount("ID=" + ddlIsuses.SelectedValue + " and getdate() between StartTime and EndTime") > 0)
                {
                    sb.Append("or a.ID in (select ChaseID from T_ExecutedChases where QuashStatus = 0 and Money > 0 and LotteryID="+ddlLottery.SelectedValue+"))");
                }
                else
                {
                    sb.Append(")");
                }
            }

            sb.Append("left join T_Users d on a.UserID = d.ID  ");
            sb.Append(" left join (select ChaseID,count(SchemeID) as ExecutedCount from  T_ExecutedChases group by ChaseID)c on a.ID = c.ChaseID");
            sb.Append("  drop table #TempT_ChaseTaskDetails ");

            dt = Shove.Database.MSSQL.Select(sb.ToString());

            if (dt == null)
            {
                PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", "Admin_ChaseList");

                return;
            }

            Shove._Web.Cache.SetCache(Key, dt);
        }

        PF.DataGridBindData(g, dt, gPager);
    }

    protected void g_ItemDataBound(object sender, DataGridItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.EditItem)
        {
            string str = e.Item.Cells[4].Text.Trim();
            if (str.Length > 8)
            {
                str = str.Substring(0, 7) + "..";
            }

            string type = e.Item.Cells[1].Text;

            if (type == "我的追号")
            {
                e.Item.Cells[3].Text = "<a href='ChaseDetail.aspx?id=" + e.Item.Cells[10].Text + "'><font color=\"#330099\">" + e.Item.Cells[3].Text + "</Font></a>";
                e.Item.Cells[4].Text = "<a href='ChaseDetail.aspx?id=" + e.Item.Cells[10].Text + "'><font color=\"#330099\">" + str + "</Font></a>";
            }
            else
            {
                e.Item.Cells[3].Text = "<a href='ChaseDetails.aspx?id=" + e.Item.Cells[10].Text + "'><font color=\"#330099\">" + e.Item.Cells[3].Text + "</Font></a>";
                e.Item.Cells[4].Text = "<a href='ChaseDetails.aspx?id=" + e.Item.Cells[10].Text + "'><font color=\"#330099\">" + str + "</Font></a>";
            }

            double money = Shove._Convert.StrToDouble(e.Item.Cells[5].Text, 0);
            e.Item.Cells[5].Text = (money == 0) ? "" : money.ToString("N");

            int SumIsuseNum = Shove._Convert.StrToInt(e.Item.Cells[6].Text, 0);
            int BuyedIsuseNum = Shove._Convert.StrToInt(e.Item.Cells[7].Text, 0);
            int QuashedIsuseNum = Shove._Convert.StrToInt(e.Item.Cells[13].Text, 0);
            int QuashStatus = Shove._Convert.StrToInt(e.Item.Cells[11].Text, 0);

            if (type == "我的追号")
            {
                e.Item.Cells[9].Text = (SumIsuseNum > (BuyedIsuseNum + QuashedIsuseNum)) ? "<Font color=\'Red\'>进行中</font>" : "已终止";
            }
            else
            {
                e.Item.Cells[9].Text = QuashStatus == 1 ? "已终止" : (SumIsuseNum == BuyedIsuseNum ? "已完成" : "<Font color=\'Red\'>进行中</font>");
            }

            int Type = Shove._Convert.StrToInt(e.Item.Cells[12].Text, 1);

            double StopMoney = Shove._Convert.StrToDouble(e.Item.Cells[8].Text, 0);

            if (Type == 1 || StopMoney == 0)
            {
                e.Item.Cells[8].Text = "完成方案";
            }
            else
            {
                e.Item.Cells[8].Text = "单期中奖金额达到" + StopMoney.ToString("N") + "元";
            }

            if (tdIsuses.Visible && e.Item.Cells[9].Text != "已终止" && e.Item.Cells[9].Text != "已完成")
            {
                if (e.Item.Cells[1].Text == "我的追号")
                {
                    DataTable dt = new DAL.Tables.T_ChaseTaskDetails().Open("Executed", "ChaseTaskID=" + e.Item.Cells[10].Text + " and IsuseID=" + ddlIsuses.SelectedValue + "", "");

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        if (Shove._Convert.StrToBool(dt.Rows[0]["Executed"].ToString(), false))
                        {
                            e.Item.Cells[9].Text = "已执行";
                        }
                        else
                        {
                            e.Item.Cells[9].Text = "未执行";
                        }
                    }
                }
                else
                {
                    if (new DAL.Tables.T_ExecutedChases().GetCount("ChaseID=" + e.Item.Cells[10].Text + " and SchemeID in (select ID from T_Schemes where IsuseID=" + ddlIsuses.SelectedValue + ")") > 0)
                    {
                        e.Item.Cells[9].Text = "已执行";
                    }
                    else
                    {
                        e.Item.Cells[9].Text = "未执行";
                    }
                }
            }
        }
    }

    protected void gPager_PageWillChange(object Sender, Shove.Web.UI.PageChangeEventArgs e)
    {
        BindData();
    }

    protected void gPager_SortBefore(object source, System.Web.UI.WebControls.DataGridSortCommandEventArgs e)
    {
        BindData();
    }

    protected void ddlLottery_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataTable dt = new DAL.Tables.T_Isuses().Open("ID,Name", "LotteryID=" + ddlLottery.SelectedValue + " and  getdate()>StartTime", "EndTime desc");

        if (dt == null)
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", "Admin_ChaseList");

            return;
        }

        Shove.ControlExt.FillDropDownList(ddlIsuses, dt, "Name", "ID");
    }

    protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlType.SelectedValue == "1")
        {
            tdUserName.Visible = true;
            tdIsuses.Visible = false;
        }
        else
        {
            tdUserName.Visible = false;
            tdIsuses.Visible = true;
        }
    }
}
