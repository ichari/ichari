using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Admin_AllBuyFocus : AdminPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindDataForLottery();
            ddlLottery_SelectedIndexChanged(ddlLottery, new EventArgs());
        }

    }

    #region Web 窗体设计器生成的代码

    override protected void OnInit(EventArgs e)
    {
        RequestLoginPage = this.Request.Url.AbsoluteUri;

        RequestCompetences = Competences.BuildCompetencesList(Competences.EditNews, Competences.QueryData);

        base.OnInit(e);
    }

    #endregion

    private void BindDataForLottery()
    {
        DataTable dt = new DAL.Tables.T_Lotteries().Open("[ID], [Name]", "[ID] in (" + (_Site.UseLotteryListRestrictions == "" ? "-1" : _Site.UseLotteryListRestrictions) + ")", "[Order]");

        if (dt == null)
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", this.GetType().BaseType.FullName);

            return;
        }

        Shove.ControlExt.FillDropDownList(ddlLottery, dt, "Name", "ID");
    }

    protected void btnSubMit_Click(object sender, EventArgs e)
    {
        string name = tbUserName.Text.Trim();

        if (name == "")
        {
            Shove._Web.JavaScript.Alert(this.Page, "请输入合买活跃明星！");

            return;
        }

        name = name.Replace("，", ",");

        string[] n = name.Split(',');

        name = "";
        foreach (string s in n)
        {
            Users u = new Users(_Site.ID)[_Site.ID, s.Trim()];

            if (u == null)
            {
                Shove._Web.JavaScript.Alert(this.Page, "对不起，您输入的"+s+"不存在！");

                return;
            }

            name += u.Name + "|" + u.ID.ToString() + ",";
        }

        if (name.EndsWith(","))
        {
            name = name.Substring(0, name.Length - 1);
        }

        DAL.Tables.T_ActiveAllBuyStar t = new DAL.Tables.T_ActiveAllBuyStar();

        t.LotterieID.Value = ddlLottery.SelectedValue;
        t.UserList.Value = name;
        t.Order.Value = Shove._Convert.StrToInt(tbOrder.Text, 1);

        if (t.GetCount("LotterieID=" + ddlLottery.SelectedValue + "") > 0)
        {
            t.Update("LotterieID=" + ddlLottery.SelectedValue + "");
        }
        else
        {
            t.Insert();
        }

        Shove._Web.Cache.ClearCache("Home_Room_JoinAllBuy_BindActiveMembers");
        Shove._Web.JavaScript.Alert(this.Page, "操作成功！");
    }

    protected void ddlLottery_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataTable dt = new DAL.Tables.T_ActiveAllBuyStar().Open("LotterieID,UserList,[Order]", "LotterieID = "+ddlLottery.SelectedValue+"","");

        if (dt == null || dt.Rows.Count < 1)
        {
            tbUserName.Text = "";
        }
        else
        {
            tbUserName.Text = dt.Rows[0]["UserList"].ToString();

            string[] userlist = tbUserName.Text.Trim().Split(',');
            tbUserName.Text = "";
            foreach (string s in userlist)
            {
                tbUserName.Text += s.Split('|')[0] + ",";
            }

            if (tbUserName.Text.EndsWith(","))
            {
                tbUserName.Text = tbUserName.Text.Substring(0, tbUserName.Text.Length-1);
            }

            tbOrder.Text = dt.Rows[0]["Order"].ToString();
        }
    }
}
