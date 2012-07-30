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

public partial class Home_Room_ChaseDetail : RoomPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        int ChaseID = Shove._Convert.StrToInt(Shove._Web.Utility.GetRequest("id"), -1);

        if (ChaseID < 0)
        {
            this.Response.Redirect("Chase.aspx");

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
        DataTable dt = new DAL.Views.V_ChaseTasksTotal().Open("", "ID = " + ChaseID.ToString() + " and UserID=" + _User.ID.ToString(), "");

        if ((dt == null) || (dt.Rows.Count < 1))
        {
            PF.GoError(ErrorNumber.DataReadWrite, "您还没有追号！", this.GetType().FullName);
            return;
        }

        DataRow dr = dt.Rows[0];

        labChase_id.Text = ChaseID.ToString();

        Label1.Text = dr["LotteryName"].ToString();
        Label3.Text = dr["Description"].ToString();

        double IsuseMoney, SumMoney, BuyedMoney, QuashedMoney;;
        int SumIsuseNum, BuyedIsuseNum, QuashedIsuseNum;
        SumMoney = Shove._Convert.StrToDouble(dr["SumMoney"].ToString(), 0);
        SumIsuseNum = Shove._Convert.StrToInt(dr["SumIsuseNum"].ToString(), 0);
        BuyedIsuseNum = Shove._Convert.StrToInt(dr["BuyedIsuseNum"].ToString(), 0);
        QuashedIsuseNum = Shove._Convert.StrToInt(dr["QuashedIsuseNum"].ToString(), 0);

        BuyedMoney = Shove._Convert.StrToDouble(dr["BuyedMoney"].ToString(), 0);
        QuashedMoney = Shove._Convert.StrToDouble(dr["QuashedMoney"].ToString(), 0);

        try
        {
            IsuseMoney = SumMoney / SumIsuseNum;
        }
        catch
        {
            PF.GoError(ErrorNumber.DataReadWrite, "投注记录有错误", this.GetType().FullName);
            return;
        }

        Label4.Text ="</font>共<font color=\'red\'>" + SumIsuseNum.ToString() + "</font>期<font color=\'red\'>" +
            SumMoney.ToString("N") + "</font>元; 已完成<font color=\'red\'>" + BuyedIsuseNum.ToString() + "</font>期<font color=\'red\'>" + (BuyedMoney).ToString("N") + "</font>元; 已取消<font color=\'red\'>" +
            QuashedIsuseNum.ToString() + "</font>期<font color=\'red\'>" + (QuashedMoney).ToString("N") + "</font>元。";

        btnQuash.Enabled = (SumIsuseNum > (BuyedIsuseNum + QuashedIsuseNum));


        //填充每期列表
        dt = new DAL.Views.V_ChaseTaskDetails().Open("", "ChaseTaskID = " + ChaseID.ToString(), "[DateTime]");

        if ((dt == null) || dt.Rows.Count < 1)
        {
            PF.GoError(ErrorNumber.DataReadWrite, "没有追号记录", this.GetType().FullName);
            return;
        }
        
        LbPlayTypeName.Text = dt.Rows[0]["PlayTypeName"].ToString();        //买法类型

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

            int QuashStatus = Shove._Convert.StrToInt(e.Item.Cells[7].Text, 0);
            long SchemeID = Shove._Convert.StrToLong(e.Item.Cells[9].Text, -1);
            if (QuashStatus != 0)
            {
                if (QuashStatus == 2)
                {
                    e.Item.Cells[4].Text = "系统撤单";
                }
                else
                {
                    e.Item.Cells[4].Text = "用户撤消";
                }
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
                    ((Button)e.Item.Cells[6].FindControl("btnQuashIsuse")).Enabled = true;
                }
            }
        }
    }

    protected void g_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
    {
        if (e.CommandName == "QuashIsuse")
        {
            int ChaseIsuse_id = Shove._Convert.StrToInt(e.Item.Cells[10].Text, 0);
            if (ChaseIsuse_id < 1)
            {
                PF.GoError(ErrorNumber.NoData, "找不到追号记录", this.GetType().FullName);
                return;
            }

            string Return = "";
            int Result = _User.QuashChaseTaskDetail(ChaseIsuse_id, false, ref Return);
            if ((Return != "") || (Result != 0))
            {
                PF.GoError(ErrorNumber.DataReadWrite, Return, this.GetType().FullName);

                return;
            }

            int Chase_id = Shove._Convert.StrToInt(labChase_id.Text, 0);
            if (Chase_id < 1)
            {
                PF.GoError(ErrorNumber.NoData, "没有记录!", this.GetType().FullName);

                return;
            }

            BindData(Chase_id);

            return;
        }
    }

    protected void btnQuash_Click(object sender, System.EventArgs e)
    {
        int Chase_id = Shove._Convert.StrToInt(labChase_id.Text, 0);
        if (Chase_id < 1)
        {
            PF.GoError(ErrorNumber.NoData, "没有记录!", this.GetType().FullName);

            return;
        }

        string Return = "";
        int Result = _User.QuashChaseTask(Chase_id, false,ref Return);
        if ((Return != "") || (Result != 0))
        {
            PF.GoError(ErrorNumber.DataReadWrite, Return, this.GetType().FullName);

            return;
        }

        BindData(Chase_id);
    }
}
