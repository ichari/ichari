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
using System.Drawing;

using Shove.Database;

public partial class Admin_LotteryTimeSet : AdminPageBase
{
    public int PreLotteryID = -1;
    public Color PreLotteryColor = Color.Linen;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            BindData();

            g.Columns[4].Visible = _User.Competences.IsOwnedCompetences(Competences.BuildCompetencesList(Competences.LotteryIsuseScheme));
        }
    }

    #region Web 窗体设计器生成的代码

    override protected void OnInit(EventArgs e)
    {
        RequestLoginPage = this.Request.Url.AbsoluteUri;

        RequestCompetences = Competences.BuildCompetencesList(Competences.LotteryIsuseScheme,Competences.QueryData);

        base.OnInit(e);
    }

    #endregion

    private void BindData()
    {
        DataTable dt = new DAL.Views.V_PlayTypes().Open("", "LotteryID in (" + (_Site.UseLotteryList == "" ? "-1" : _Site.UseLotteryList) + ")", "[Order], [ID]");

        if (dt == null)
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", "Admin_LotteryTimeSet");

            return;
        }

        PF.DataGridBindData(g, dt);
    }

    protected void g_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.EditItem)
        {
            TextBox tb1 = (TextBox)e.Item.Cells[2].FindControl("tbSystemEndAheadMinute");
            tb1.Text = e.Item.Cells[8].Text;

            TextBox tb2 = (TextBox)e.Item.Cells[3].FindControl("tbChaseExecuteDeferMinute");
            tb2.Text = e.Item.Cells[9].Text;

            string MaxChaseIsuse = e.Item.Cells[7].Text.Replace("&nbsp;", "").Trim();

            if (MaxChaseIsuse == "")
            {
                tb2.Visible = false;
            }

            int RowLotteryID = Shove._Convert.StrToInt(e.Item.Cells[6].Text, -1);

            if (RowLotteryID != PreLotteryID)
            {
                PreLotteryID = RowLotteryID;

                if (PreLotteryColor.Name == Color.Linen.Name)
                {
                    PreLotteryColor = Color.White;
                }
                else
                {
                    PreLotteryColor = Color.Linen;
                }
            }

            e.Item.BackColor = PreLotteryColor;
            tb1.BackColor = PreLotteryColor;
            tb2.BackColor = PreLotteryColor;
        }
    }

    protected void g_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
    {
        if (e.CommandName == "btnOK")
        {
            int ID = Shove._Convert.StrToInt(e.Item.Cells[5].Text, -1);
            int LotteryID = Shove._Convert.StrToInt(e.Item.Cells[6].Text, -1);

            int SystemEndAheadMinute = Shove._Convert.StrToInt(((TextBox)e.Item.Cells[2].FindControl("tbSystemEndAheadMinute")).Text, -1);
            int ChaseExecuteDeferMinute = Shove._Convert.StrToInt(((TextBox)e.Item.Cells[3].FindControl("tbChaseExecuteDeferMinute")).Text, -1);

            if (SystemEndAheadMinute < 2)
            {
                Shove._Web.JavaScript.Alert(this.Page, "提前截止分钟数最少必须 2 分钟，否则系统执行可能会因时间过短而不能及时处理，导致数据错误！");

                return;
            }

            string MaxChaseIsuse = e.Item.Cells[7].Text.Replace("&nbsp;", "").Trim();

            if (MaxChaseIsuse != "")
            {
                if (ChaseExecuteDeferMinute < 1)
                {
                    Shove._Web.JavaScript.Alert(this.Page, "追号任务自动执行必须在开始时间后最少 1 分钟！");

                    return;
                }
            }

            DAL.Tables.T_PlayTypes T_PlayTypes = new DAL.Tables.T_PlayTypes();

            T_PlayTypes.SystemEndAheadMinute.Value = SystemEndAheadMinute;

            if (T_PlayTypes.Update("[ID] = " + ID.ToString()) < 0)
            {
                PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", "Admin_LotteryTimeSet");

                return;
            }

            if (MaxChaseIsuse != "")
            {
                if (MSSQL.ExecuteNonQuery("update T_Lotteries set ChaseExecuteDeferMinute = " + ChaseExecuteDeferMinute.ToString() + " where [ID] = " + LotteryID.ToString()) < 0)
                {
                    PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", "Admin_LotteryTimeSet");

                    return;
                }
            }

            BindData();

            Shove._Web.JavaScript.Alert(this.Page, "保存成功。");

            return;
        }
    }
}
