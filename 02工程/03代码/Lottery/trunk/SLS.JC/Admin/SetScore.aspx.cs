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

public partial class Admin_SetScore : AdminPageBase
{
    DAL.Tables.T_WinScoreScale scoreScale = new DAL.Tables.T_WinScoreScale();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindDataForLottery();
            BindData();
        }
    }

    #region Web 窗体设计器生成的代码

    override protected void OnInit(EventArgs e)
    {
        RequestLoginPage = this.Request.Url.AbsoluteUri;

        RequestCompetences = Competences.BuildCompetencesList(Competences.Options, Competences.QueryData);

        base.OnInit(e);
    }

    #endregion


    //绑定彩种
    private void BindDataForLottery()
    {
        DataTable dt = new DAL.Tables.T_Lotteries().Open("[ID], [Name]", "[ID] in (" + (_Site.UseLotteryListRestrictions == "" ? "-1" : _Site.UseLotteryListRestrictions) + ")", "[Order]");

        if (dt == null)
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", "Admin_SetScore");

            return;
        }

        Shove.ControlExt.FillDropDownList(ddlLottery, dt, "Name", "ID");
    }

    //根据玩法显示相应的信息
    private void BindData()
    {
        if (ddlLottery.Items.Count < 1)
        {
            return;
        }

        string sql = @"select a.id, a.WinMoney,a.ScoreScale,a.WinTypeID, b.Name as WinTypesName from T_WinScoreScale a inner join T_WinTypes b on a.WinTypeID=b.ID and b.LotteryID= " + Shove._Web.Utility.FilteSqlInfusion(ddlLottery.SelectedValue.ToString()) + " order by WinMoney";
        DataTable dt = MSSQL.Select(sql);

        if ((dt == null) || (dt.Rows.Count < 1))
        {
            sql = @"select a.id, a.WinMoney,a.ScoreScale,a.PlayTypeID, b.Name as WinTypesName from T_PlayScoreScale a inner join T_PlayTypes b on a.PlayTypeID=b.ID and b.LotteryID= " + Shove._Web.Utility.FilteSqlInfusion(ddlLottery.SelectedValue.ToString()) + " order by WinMoney";
            dt = MSSQL.Select(sql);

            if (dt == null)
            {
                PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", "Admin_SetScore");

                return;
            }
        }

        g.DataSource = dt;
        g.DataBind();

        txtMoney.Text = "";
        txtScoreScale.Text = "";
    }

    protected void ddlLottery_SelectedIndexChanged(object sender, System.EventArgs e)
    {
        BindData();
    }

    protected void btnOK_Click(object sender, EventArgs e)
    {
        int id = Shove._Convert.StrToInt(hfID.Value, -1);
        string winMoney = txtMoney.Text.Trim();
        string scale = txtScoreScale.Text.Trim();

        if (winMoney == "")
        {
            Shove._Web.JavaScript.Alert(this.Page, "请输入中奖金额！");

            return;
        }

        if (scale == "")
        {
            Shove._Web.JavaScript.Alert(this.Page, "请输入积分比例！");

            return;
        }

        scoreScale.WinMoney.Value = Shove._Convert.StrToDouble(winMoney, 0);
        scoreScale.ScoreScale.Value = Shove._Convert.StrToDouble(scale, 0);

        if (id > 0)
        {
            if (scoreScale.Update("ID=" + id) < 0)
            {
                Shove._Web.JavaScript.Alert(this.Page, "修改失败！");
                return;
            }
            else
            {
                Shove._Web.JavaScript.Alert(this.Page, "修改成功！");
                BindData();
                btnCancel_Click(sender, e);
            }
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        this.txtMoney.Text = "";
        this.txtScoreScale.Text = "";
        hfID.Value = "-1";
    }

    protected void g_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
    {
        int id = Shove._Convert.StrToInt(e.Item.Cells[4].Text, -1);
        if (e.CommandName == "Edit")
        {
            DataTable dt = scoreScale.Open("", "ID=" + id, "");
            txtMoney.Text = dt.Rows[0]["WinMoney"].ToString();
            txtScoreScale.Text = dt.Rows[0]["ScoreScale"].ToString();
            hfID.Value = id.ToString();
        }
    }
}
