using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;

public partial class Home_Room_ChaseExecutedSchemes : RoomPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        int ChaseID = Shove._Convert.StrToInt(Shove._Web.Utility.GetRequest("id"), -1);

        if (ChaseID < 0)
        {
            this.Response.Redirect("ViewChase.aspx");

            return;
        }

        if (!this.IsPostBack)
        {
            BindData(ChaseID);
        }
    }

    #region Web 窗体设计器生成的代码

    override protected void OnInit(EventArgs e)
    {
        RequestLoginPage = this.Request.Url.AbsoluteUri;

        base.OnInit(e);
    }

    #endregion

    private void BindData(int ChaseID)
    {
        string Key = "Home_Room_ChaseExecutedSchemes_BindData_" + ChaseID.ToString();
        DataTable dtChase = Shove._Web.Cache.GetCacheAsDataTable(Key);

        StringBuilder sb = new StringBuilder();

        if (dtChase == null || dtChase.Rows.Count == 0)
        {
            sb.Append("select Title,Name,IsuseCount,IsuseCount*Multiple*Nums*Price as SumMoney,Money,QuashStatus,ExecutedCount,ExecutedCount*Multiple*Nums*Price as ExcutedMoney,")
                .Append("IsuseCount-ExecutedCount as NoExecutedCount,Title,StopTypeWhenWin,StopTypeWhenWinMoney from T_Chases a inner join T_Lotteries b ")
                .Append("on a.LotteryID = b.ID and a.ID=" + ChaseID.ToString() + " ")
                .Append("left join (select ChaseID,count(SchemeID) as ExecutedCount from  T_ExecutedChases group by ChaseID)c on a.ID = c.ChaseID");

            dtChase = Shove.Database.MSSQL.Select(sb.ToString());

            if (dtChase == null || dtChase.Rows.Count == 0)
            {
                PF.GoError(ErrorNumber.DataReadWrite, "此记录不存在或已被删除！", this.GetType().FullName);

                return;
            }

            Shove._Web.Cache.SetCache(Key, dtChase);
        }

        DataRow dr = dtChase.Rows[0];

        lbLotteryName.Text = dr["Name"].ToString();
        lbTitle.Text = dr["Title"].ToString();

        if (dr["StopTypeWhenWin"].ToString() == "1")
        {
            lbStopCondition.Text = "完成方案";
        }
        else
        {
            lbStopCondition.Text = "单期中奖金额达到" + dr["StopTypeWhenWinMoney"].ToString();
        }

        double SumMoney, BuyedMoney, QuashedMoney; ;
        int SumIsuseNum, BuyedIsuseNum, QuashedIsuseNum;
        SumMoney = Shove._Convert.StrToDouble(dr["SumMoney"].ToString(), 0);
        SumIsuseNum = Shove._Convert.StrToInt(dr["IsuseCount"].ToString(), 0);
        BuyedIsuseNum = Shove._Convert.StrToInt(dr["ExecutedCount"].ToString(), 0);
        QuashedIsuseNum = Shove._Convert.StrToInt(dr["NoExecutedCount"].ToString(), 0);

        BuyedMoney = Shove._Convert.StrToDouble(dr["ExcutedMoney"].ToString(), 0);
        QuashedMoney = Shove._Convert.StrToDouble(dr["Money"].ToString(), 0);

        lbDescription.Text = "</font>共<font color=\'red\'>" + SumIsuseNum.ToString() + "</font>期<font color=\'red\'>" +
            SumMoney.ToString("N") + "</font>元; 已完成<font color=\'red\'>" + BuyedIsuseNum.ToString() + "</font>期<font color=\'red\'>" + (BuyedMoney).ToString("N") + "</font>元; 未执行<font color=\'red\'>" +
            QuashedIsuseNum.ToString() + "</font>期<font color=\'red\'>" + (QuashedMoney).ToString("N") + "</font>元。";

        //填充每期列表
        Key = "Home_Room_ChaseExecutedSchemes_BindDataDetails_" + ChaseID.ToString();
        DataTable dt = Shove._Web.Cache.GetCacheAsDataTable(Key);

        sb = new StringBuilder();

        if (dt == null)
        {
            sb.Append("select PlayTypeName,ID,IsuseName,LotteryNumber,Multiple,Money,QuashStatus,Buyed from T_ExecutedChases a inner join V_Schemes b ")
                .Append("on a.SchemeID = b.ID and a.ChaseID =" + ChaseID.ToString() + " order by ID");

            dt = Shove.Database.MSSQL.Select(sb.ToString());

            if (dt == null)
            {
                PF.GoError(ErrorNumber.DataReadWrite, "您还没有追号！", this.GetType().FullName);

                return;
            }

            Shove._Web.Cache.SetCache(Key, dt);
        }

        if (dt.Rows.Count > 0)
        {
            lbPlayTypeName.Text = dt.Rows[0]["PlayTypeName"].ToString();        //买法类型
        }

        PF.DataGridBindData(g, dt);
    }

    protected void g_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.EditItem)
        {
            e.Item.Cells[0].Text += "期";
            e.Item.Cells[1].Text = Shove._Convert.ToHtmlCode(e.Item.Cells[1].Text);
            e.Item.Cells[2].Text = "倍数:" + e.Item.Cells[2].Text;

            double money;
            money = Shove._Convert.StrToDouble(e.Item.Cells[3].Text, 0);
            e.Item.Cells[3].Text = (money == 0) ? "" : money.ToString("N");

            int QuashStatus = Shove._Convert.StrToInt(e.Item.Cells[6].Text, 0);
            long SchemeID = Shove._Convert.StrToLong(e.Item.Cells[8].Text, -1);
            if (QuashStatus != 0)
            {
                e.Item.Cells[4].Text = "系统撤单";
            }
            else
            {
                if (SchemeID > 0)
                {
                    e.Item.Cells[4].Text = "<font color=\'#330099\'>已成功</font>";
                    e.Item.Cells[5].Text = "<a href='Scheme.aspx?id=" + SchemeID.ToString() + "' target='_blank'><font color=\"#330099\">查看方案</Font></a>";
                }
                else
                {
                    e.Item.Cells[4].Text = "<font color=\'Red\'>进行中</font>";
                }
            }
        }
    }
}
