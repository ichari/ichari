using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Admin_PersonagesEdit : AdminPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindLotteryType();
        }
    }

    private void BindLotteryType()
    {
        string CacheKey = "dtLotteriesUseLotteryList";
        DataTable dtLotteries = Shove._Web.Cache.GetCacheAsDataTable(CacheKey);

        if (dtLotteries == null)
        {
            dtLotteries = new DAL.Tables.T_Lotteries().Open("[ID], [Name], [Code]", "[ID] in(" + (_Site.UseLotteryList == "" ? "-1" : _Site.UseLotteryList) + ")", "[ID]");

            if (dtLotteries == null)
            {
                PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", this.GetType().BaseType.FullName + "(-46)");

                return;
            }

            Shove._Web.Cache.SetCache(CacheKey, dtLotteries, 6000);
        }

        ddlLotteries.DataSource = dtLotteries;
        ddlLotteries.DataTextField = "Name";
        ddlLotteries.DataValueField = "ID";
        ddlLotteries.DataBind();

        hidID.Value = Shove._Web.Utility.GetRequest("ID");
        DataTable dt = new DAL.Tables.T_Personages().Open("", "ID=" + Shove._Web.Utility.FilteSqlInfusion(hidID.Value) + "", "");

        if (dt == null || dt.Rows.Count == 0)
        {
            Shove._Web.JavaScript.Alert(this.Page, "记录不存在！");

            return;
        }

        if (ddlLotteries.Items.FindByValue(dt.Rows[0]["LotteryID"].ToString()) != null)
        {
            ddlLotteries.SelectedValue = dt.Rows[0]["LotteryID"].ToString();
        }

        tbOrder.Text = dt.Rows[0]["Order"].ToString();
        cbisShow.Checked = Shove._Convert.StrToBool(dt.Rows[0]["IsShow"].ToString(), true);
        tbName.Text = dt.Rows[0]["UserName"].ToString();
    }

    protected override void OnInit(EventArgs e)
    {
        RequestLoginPage = this.Request.Url.AbsoluteUri;

        RequestCompetences = Competences.BuildCompetencesList(Competences.FillContent);

        base.OnInit(e);
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        string UserName = Shove._Web.Utility.FilteSqlInfusion(tbName.Text.Trim());

        if (UserName == "")
        {
            Shove._Web.JavaScript.Alert(this.Page, "请输入名人用户名！");

            return;
        }

        int order = Shove._Convert.StrToInt(tbOrder.Text.Trim(), -1);

        if (order < 0)
        {
            Shove._Web.JavaScript.Alert(this.Page, "顺序输入非法！");

            return;
        }

        DataTable dt = new DAL.Tables.T_Users().Open("ID", "Name='" + UserName + "'", "");

        if (dt == null || dt.Rows.Count == 0)
        {
            Shove._Web.JavaScript.Alert(this.Page, "不存在" + UserName + "用户！");

            return;
        }

        dt = new DAL.Tables.T_Personages().Open("ID", "UserName='" + UserName + "' and LotteryID=" + Shove._Web.Utility.FilteSqlInfusion(ddlLotteries.SelectedValue) + " and ID<>"+Shove._Web.Utility.FilteSqlInfusion(hidID.Value)+"", "");

        if (dt != null && dt.Rows.Count > 0)
        {
            Shove._Web.JavaScript.Alert(this.Page, UserName + "已经是" + ddlLotteries.SelectedItem.Text + "的名人了！");

            return;
        }

        DAL.Tables.T_Personages p = new DAL.Tables.T_Personages();

        p.Order.Value = order;
        p.UserName.Value = UserName;
        p.LotteryID.Value = ddlLotteries.SelectedValue;
        p.IsShow.Value = cbisShow.Checked;

        long l = p.Update("ID ="+ Shove._Web.Utility.FilteSqlInfusion(hidID.Value));

        if (l > 0)
        {
            Shove._Web.Cache.ClearCache("Admin_Personages");

            Shove._Web.Cache.ClearCache("DataCache_CelebrityHall_Collects");
            Shove._Web.Cache.ClearCache("DataCache_CelebrityHall_Star");
            Shove._Web.Cache.ClearCache("DataCache_CelebrityHall_Recommends");

            Shove._Web.JavaScript.Alert(this, "修改成功", "Personages.aspx?LotteryID=" + ddlLotteries.SelectedValue);
        }
        else
        {
            Shove._Web.JavaScript.Alert(this, "修改失败");
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("Personages.aspx?LotteryID=" + ddlLotteries.SelectedValue, true);
    }
}
