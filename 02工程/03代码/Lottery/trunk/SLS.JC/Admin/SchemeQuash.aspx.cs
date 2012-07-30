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

public partial class Admin_SchemeQuash : AdminPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            BindDataForLottery();

            BindData();
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

    private void BindDataForLottery()
    {
        DataTable dt = new DAL.Tables.T_Lotteries().Open("[ID], [Name]", "[ID] in (" + (_Site.UseLotteryListRestrictions == "" ? "-1" : _Site.UseLotteryListRestrictions) + ")", "[Order]");

        if (dt == null)
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", this.GetType().FullName);

            return;
        }

        Shove.ControlExt.FillDropDownList(ddlLottery, dt, "Name", "ID");
    }

    private void BindData()
    {
        if (ddlLottery.Items.Count < 1)
        {
            return;
        }

        string Condition = "LotteryID = " + Shove._Web.Utility.FilteSqlInfusion(ddlLottery.SelectedValue) + " and QuashStatus = 0 and Buyed = 0 and isOpened = 0";

        if (tbQuash.Text.Trim() != "")
        {
            if (rb1.Checked)
            {
                Condition += " and SchemeNumber = '" + Shove._Web.Utility.FilteSqlInfusion(tbQuash.Text.Trim()) + "'";
            }
            else
            {
                Condition += " and InitiateName = '" + Shove._Web.Utility.FilteSqlInfusion(tbQuash.Text.Trim()) + "'";
            }
        }

        DataTable dt = new DAL.Views.V_SchemeSchedules().Open("", Condition, "[Money] desc");

        if (dt == null)
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", "Admin_SchemeQuash");

            return;
        }
        
        dt.Columns.Add("LocateWaysAndMultiples", System.Type.GetType("System.String"));

        string BuyContent = "";
        string CnLocateWaysAndMultiples = "";

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            dt.Rows[i]["Money"] = double.Parse(dt.Rows[i]["Money"].ToString()).ToString("N");

            if (new SLS.Lottery()[SLS.Lottery.ZCDC.sID].CheckPlayType(int.Parse(dt.Rows[i]["PlayTypeID"].ToString())))
            {
                dt.Rows[i]["Multiple"] = 0;

                try
                {
                    BuyContent = "";
                    CnLocateWaysAndMultiples = "";

                    if (new SLS.Lottery()[SLS.Lottery.ZCDC.sID].GetSchemeSplit(dt.Rows[i]["LotteryNumber"].ToString(), ref BuyContent, ref CnLocateWaysAndMultiples))
                    {
                        dt.Rows[i]["LocateWaysAndMultiples"] = CnLocateWaysAndMultiples;
                    }
                    else
                    {
                        dt.Rows[i]["LocateWaysAndMultiples"] = "<font color='red'>读取错误！</font>";
                    }
                }
                catch
                {
                    dt.Rows[i]["LocateWaysAndMultiples"] = "<font color='red'>读取错误！</font>";
                }
            }
            else if (Shove._Web.Utility.FilteSqlInfusion(ddlLottery.SelectedValue) == "72" || Shove._Web.Utility.FilteSqlInfusion(ddlLottery.SelectedValue) == "73")
            {
                dt.Rows[i]["LotteryNumber"] = "查看详细信息";
            }

            dt.AcceptChanges();
        }

        g.DataSource = dt;
        g.DataBind();
    }

    protected void ddlLottery_SelectedIndexChanged(object sender, System.EventArgs e)
    {
        BindData();
    }

    protected void btnRefresh_Click(object sender, System.EventArgs e)
    {
        BindData();
    }

    protected void g_ItemCommand(object source, System.Web.UI.WebControls.DataListCommandEventArgs e)
    {
        if (e.CommandName == "btnQuash")
        {
            long SiteID = Shove._Convert.StrToLong(((HtmlInputHidden)e.Item.FindControl("tbSiteID")).Value, -1);
            long InitiateUserID = Shove._Convert.StrToLong(((HtmlInputHidden)e.Item.FindControl("tbInitiateUserID")).Value, -1);
            long SchemeID = Shove._Convert.StrToLong(((HtmlInputHidden)e.Item.FindControl("tbSchemeID")).Value, -1);

            if ((SiteID < 0) || (InitiateUserID < 0) || (SchemeID < 0))
            {
                PF.GoError(ErrorNumber.Unknow, "参数错误", "Admin_SchemeQuash");

                return;
            }

            Users tu = new Users(SiteID)[SiteID, InitiateUserID];

            if (tu == null)
            {
                PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", "Admin_SchemeQuash");

                return;
            }

            string ReturnDescription = "";
            if (tu.QuashScheme(SchemeID, true, ref ReturnDescription) < 0)
            {
                PF.GoError(ErrorNumber.Unknow, ReturnDescription, "Admin_SchemeQuash");

                return;
            }

            DAL.Tables.T_UserDetails t = new DAL.Tables.T_UserDetails();

            t.Memo.Value = "投注通讯故障撤单";
            t.Update("SchemeID = " + SchemeID.ToString() + " and OperatorType = 9");

            BindData();

            return;
        }
    }
    protected void g_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.EditItem)
        {
            Shove.Web.UI.ShoveConfirmButton sb = e.Item.FindControl("btnQuash") as Shove.Web.UI.ShoveConfirmButton;

            sb.Visible = _User.Competences.IsOwnedCompetences(Competences.BuildCompetencesList(Competences.LotteryIsuseScheme));
        }
    }

    public string GetScriptResTable(string t_str)
    {
        if (ddlLottery.SelectedValue == "72" || ddlLottery.SelectedValue == "73")
        {
            return PF.GetScriptResTable(t_str);
        }
        else
        {
            return t_str;
        }
    }
}
