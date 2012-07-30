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

public partial class Admin_News : AdminPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            BindDataForType();
            BindData();

            btnAdd.Visible = _User.Competences.IsOwnedCompetences(Competences.BuildCompetencesList(Competences.EditNews));
            g.Columns[7].Visible = btnAdd.Visible;
        }
    }

    #region Web 窗体设计器生成的代码

    override protected void OnInit(EventArgs e)
    {
        RequestLoginPage = this.Request.Url.AbsoluteUri;

        RequestCompetences = Competences.BuildCompetencesList(Competences.EditNews,Competences.QueryData);

        base.OnInit(e);
    }

    #endregion

    /// <summary>
    /// 绑定新闻类型树
    /// </summary>
    private void BindDataForType()
    {
        DataTable dt = new DAL.Tables.T_NewsTypes().Open("", "SiteID = " + _Site.ID.ToString(), "[ID]");

        if (dt == null)
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", this.GetType().BaseType.FullName);

            return;
        }

        tv.DataTable = dt;
        tv.DataBind();

        foreach (TreeNode tn in tv.Nodes)
        {
            tn.NavigateUrl = "";

            foreach (TreeNode t in tn.ChildNodes)
            {
                t.NavigateUrl = "";
            }
        }

        string TypeID = Shove._Convert.StrToLong(Shove._Web.Utility.GetRequest("TypeID"), -1).ToString();

        if (TypeID != "-1")
        {
            Shove.ControlExt.SetTreeViewSelectedFromValue(tv, TypeID);
        }
        else if (tv.Nodes.Count > 0)
        {
            tv.Nodes[0].Select();
        }
    }

    private void BindData()
    {
        if (tv.Nodes.Count < 1)
        {
            return;
        }

        string TypeID = tv.SelectedValue;

        if (TypeID == "")
        {
            TypeID = tv.Nodes[0].Value;
        }

        DataTable dt = new DAL.Tables.T_News().Open("", "SiteID = " + _Site.ID.ToString() + " and TypeID = " + Shove._Web.Utility.FilteSqlInfusion(TypeID), "[DateTime] desc");

        if (dt == null)
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", this.GetType().BaseType.FullName);

            return;
        }



        PF.DataGridBindData(g, dt, gPager);
    }

    protected void g_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.EditItem)
        {
            CheckBox cb = (CheckBox)e.Item.Cells[2].FindControl("cbisShow");
            cb.Checked = Shove._Convert.StrToBool(e.Item.Cells[9].Text, true);

            cb = (CheckBox)e.Item.Cells[3].FindControl("cbisHasImage");
            cb.Checked = Shove._Convert.StrToBool(e.Item.Cells[10].Text, true);

            cb = (CheckBox)e.Item.Cells[4].FindControl("cbisCommend");
            cb.Checked = Shove._Convert.StrToBool(e.Item.Cells[11].Text, true);

            cb = (CheckBox)e.Item.Cells[5].FindControl("cbisHot");
            cb.Checked = Shove._Convert.StrToBool(e.Item.Cells[12].Text, true);
        }
    }

    protected void g_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
    {
        if (e.CommandName == "Edit")
        {
            this.Response.Redirect("NewsEdit.aspx?TypeID=" + tv.SelectedValue + "&ID=" + e.Item.Cells[8].Text, true);

            return;
        }

        if (e.CommandName == "Del")
        {
            int ReturnValue = -1;
            string ReturnDescription = "";

            int Results = -1;
            Results = DAL.Procedures.P_NewsDelete(_Site.ID, long.Parse(e.Item.Cells[8].Text), ref ReturnValue, ref ReturnDescription);

            if (Results < 0)
            {
                PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", this.GetType().BaseType.FullName);

                return;
            }

            if (ReturnValue < 0)
            {
                PF.GoError(ErrorNumber.Unknow, ReturnDescription, this.GetType().BaseType.FullName);

                return;
            }

            if (tv.SelectedNode.Text == "时时乐资讯")
            {
                Shove._Web.Cache.ClearCache(DataCache.LotteryNews+"29");
            }

            if (tv.SelectedNode.Text == "十一运夺金资讯")
            {
                Shove._Web.Cache.ClearCache(DataCache.LotteryNews+"62");
            }

            if (tv.SelectedNode.Text == "热门人物追踪")
            {
                Shove._Web.Cache.ClearCache("Home_Room_JoinAllBuy_BindNews");
            }

            string type = tv.SelectedNode.Text.Trim();

            if (type == "体彩资讯" || type == "足彩资讯" || type == "篮彩资讯")
            {
                Shove._Web.Cache.ClearCache("Default_GetNews");
            }

            if (type.Contains("3D"))
            {
                Shove._Web.Cache.ClearCache("Home_Room_Buy_BindNewsForLottery6");
            }

            if (type.Contains("双色球"))
            {
                Shove._Web.Cache.ClearCache("Home_Room_Buy_BindNewsForLottery5");
            }

            if (type.Contains("玩法攻略"))
            {
                Shove._Web.Cache.ClearCache("Default_BindWFGL");
            }
            
            // 附加清除
            if (type.Contains("擂台新闻"))
            {
                Shove._Web.Cache.ClearCache("DataCache_Challenge_72_News");
            }

            if (type.Contains("名人堂新闻"))
            {
                Shove._Web.Cache.ClearCache("DataCache_CelebrityHall_News");
            }

            if (type.Contains("竞技彩资讯") || type.Contains("数字彩资讯"))
            {
                Shove._Web.Cache.ClearCache("Default_BindSportsNews");
            }

            if (type.Contains("竞彩足球"))
            {
                Shove._Web.Cache.ClearCache("ForeCast_BindNewsForLottery竞彩足球");
            }
            if (type.Contains("竞彩篮球"))
            {
                Shove._Web.Cache.ClearCache("ForeCast_BindNewsForLottery竞彩篮球");
            }
            if (type.Contains("超级大乐透"))
            {
                Shove._Web.Cache.ClearCache("ForeCast_BindNewsForLottery超级大乐透");
            }
            if (type.Contains("排列3/5"))
            {
                Shove._Web.Cache.ClearCache("ForeCast_BindNewsForLottery排列3/5");
            }
            if (type.Contains("七星彩"))
            {
                Shove._Web.Cache.ClearCache("ForeCast_BindNewsForLottery七星彩");
            }
            if (type.Contains("22选5"))
            {
                Shove._Web.Cache.ClearCache("ForeCast_BindNewsForLottery22选5");
            }
            if (type.Contains("竞彩足球"))
            {
                Shove._Web.Cache.ClearCache("ForeCast_BindNewsForLottery竞彩足球");
            }

            if (type.Contains("足彩资讯"))
            {
                Shove._Web.Cache.ClearCache("ForeCast_BindNewsForLottery足彩资讯");
            }

            if (type.Contains("欧冠资讯"))
            {
                Shove._Web.Cache.ClearCache("ForeCast_BindNewsForLottery欧冠资讯");
            }
            if (type.Contains("英超资讯"))
            {
                Shove._Web.Cache.ClearCache("ForeCast_BindNewsForLottery英超资讯");
            }
            if (type.Contains("西甲资讯"))
            {
                Shove._Web.Cache.ClearCache("ForeCast_BindNewsForLottery西甲资讯");
            }
            if (type.Contains("意甲资讯"))
            {
                Shove._Web.Cache.ClearCache("ForeCast_BindNewsForLottery意甲资讯");
            }
            if (type.Contains("德甲资讯"))
            {
                Shove._Web.Cache.ClearCache("ForeCast_BindNewsForLottery德甲资讯");
            }
            BindData();

            return;
        }
    }

    protected void btnAdd_Click(object sender, System.EventArgs e)
    {
        this.Response.Redirect("NewsAdd.aspx?TypeID=" + tv.SelectedValue, true);
    }

    protected void gPager_PageWillChange(object Sender, Shove.Web.UI.PageChangeEventArgs e)
    {
        BindData();
    }

    protected void gPager_SortBefore(object source, System.Web.UI.WebControls.DataGridSortCommandEventArgs e)
    {
        BindData();
    }

    protected void tv_SelectedNodeChanged(object sender, EventArgs e)
    {
        BindData();
    }
}
