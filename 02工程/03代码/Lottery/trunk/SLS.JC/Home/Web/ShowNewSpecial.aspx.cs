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
using System.Text.RegularExpressions;
using System.Text;

public partial class Home_Web_ShowNewSpecial : SitePageBase
{
    protected static string NewsHtml = "";
    public string WinUserList = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            hID.Value = Shove._Convert.StrToLong(Shove._Web.Utility.GetRequest("ID"), -1).ToString();
            BindData();
            // 绑定最新中奖
            BindWin();
            BindDataForAnalysisAndTerminologyAndSkills();
            ShowInfo.Visible = false;
            CommentContents.Visible = false;
            Comments.Visible = false;
        }
    }

    /// <summary>
    /// 绑定最新中奖
    /// </summary>
    private void BindWin()
    {
        DataTable dt = Shove._Web.Cache.GetCacheAsDataTable("DataCache_ZX");

        if (dt == null)
        {

            string sql = "select  top 10 u.ID as UserID,l.ID as LotteryID, u.Name as UserName,l.Name as Name,i.Name as IsuseName ,s.Multiple, b.WinMoneyNoWithTax as [Money]"
                + "from T_BuyDetails b "
                + "inner join T_Users u on b.UserID = u.ID "
                + "inner join T_Schemes s on b.SchemeID = s.ID "
                + "inner join T_Isuses i on s.IsuseID = i.ID "
                + "inner join T_Lotteries l on i.LotteryID = l.ID "
                + "where b.[WinMoneyNoWithTax] > 1 "
                + "order by  b.WinMoneyNoWithTax desc";

            dt = Shove.Database.MSSQL.Select(sql);

            if (dt == null)
            {
                PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试(130)", this.GetType().BaseType.FullName);

                return;
            }

            if (dt.Rows.Count > 0)
            {
                Shove._Web.Cache.SetCache("DataCache_ZX", dt, 1200);
            }
        }

        if (dt.Rows.Count < 1)
        {
            return;
        }

        StringBuilder sb = new StringBuilder();

        foreach (DataRow dr in dt.Rows)
        {
            sb.Append("<tr><td class=\"text_l\">" + dr["UserName"].ToString().Substring(0, 1) + "***</td>");
            sb.Append("<td class=\"text_c\">" + dr["Name"].ToString() + "</td>");
            sb.Append("<td class=\"text_c\">" + dr["Multiple"].ToString() + "</td>");
            sb.Append("<td class=\"red text_r\">" + Shove._Convert.StrToDouble(dr["Money"].ToString(), 0).ToString("N") + "<span class=\"gray\">元</span></td></tr>");
        }

        WinUserList = sb.ToString();
    }

    private void BindDataForAnalysisAndTerminologyAndSkills()
    {
        string key = "Home_Web_ShowAffiches_Default_GetNews";

        DataTable dt = Shove._Web.Cache.GetCacheAsDataTable(key);

        if (dt == null || dt.Rows.Count == 0)
        {
            string sql = @"select * from
                                (select top 6 ID,Title,TypeName,Content from V_News where isShow = 1  and [TypeName] = '推荐分析'
                                order by isCommend,ID desc)a
                            union
                            select * from
                                (select top 6 ID,Title,TypeName,Content from V_News where isShow = 1  and [TypeName] = '投注技巧'
                                order by isCommend,ID desc)b
                            union
                            select * from
                                (select top 6 ID,Title,TypeName,Content from V_News where isShow = 1  and [TypeName] = '中奖故事'
                                order by isCommend,ID desc)c
                             union
                            select * from
                                (select top 5 ID,Title,TypeName,Content from V_News where isShow = 1  and [TypeName] = '中奖公告'
                                order by isCommend,ID desc)d
                             union
                            select * from
                                (select top 7 ID,Title,TypeName,Content from V_News where isShow = 1  and [TypeName] = '精彩活动'
                                order by isCommend,ID desc)e";

            dt = Shove.Database.MSSQL.Select(sql);

            if (dt == null)
            {
                return;
            }

            Shove._Web.Cache.SetCache(key, dt, 1200);
        }

        DataTable dtAnalysis = dt.Clone();
        DataTable dtSkills = dt.Clone();

        if (dt.Rows.Count > 0)
        {
            foreach (DataRow dr in dt.Rows)
            {
                switch (dr["TypeName"].ToString())
                {
                    case "推荐分析":
                        {
                            dtAnalysis.Rows.Add(dr.ItemArray);
                        } break;
                    case "投注技巧":
                        {
                            dtSkills.Rows.Add(dr.ItemArray);
                        } break;
                }
            }
        }

        Regex regex = new Regex(@"^http://([\w-]+\.)+[\w-]+(/[\w-./?%&=]*)?$", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        System.Text.StringBuilder sb = new System.Text.StringBuilder();

        foreach (DataRow dr in dtAnalysis.Rows)
        {
            sb.AppendLine("<tr><td width=\"4%\" height=\"25\" class=\"red14\">·");
            Match m = regex.Match(dr["Content"].ToString());

            if (m.Success)
            {
                sb.Append("<a href=\"" + dr["Content"].ToString() + "\" target=\"_blank\">" + Shove._String.Cut(dr["Title"].ToString(), 16) + "</a>");
            }
            else
            {
                if (hID.Value.Equals(dr["ID"].ToString()))
                { // ID 相同
                    sb.Append("<span style=\"color:gray\">" + Shove._String.Cut(dr["Title"].ToString(), 16) + "</span>");
                }
                else
                {
                    sb.Append("<a href=\"/Home/Web/ShowNews.aspx?Id=" + dr["ID"].ToString() + "\">" + Shove._String.Cut(dr["Title"].ToString(), 16) + "</a>");
                }
            }
            sb.AppendLine("</td></tr>");
        }

        NewsHtml = sb.ToString();
    }

    protected void rptSkills_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.EditItem || e.Item.ItemType == ListItemType.AlternatingItem)
        {

            DataRow dr = ((DataRowView)e.Item.DataItem).Row;
            HyperLink hl = e.Item.FindControl("hlSkills") as HyperLink;
            string title = dr["Title"].ToString();
            hl.Text = Shove._String.HtmlTextCut(title, 10);

            hl.NavigateUrl = dr["Content"].ToString();
        }
    }

    protected void rptRecommendAnalysis_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.EditItem)
        {
            DataRow dr = ((DataRowView)e.Item.DataItem).Row;
            HyperLink hl = e.Item.FindControl("hlAnalysisTitle") as HyperLink;

            string title = dr["Title"].ToString();
            hl.Text = Shove._String.HtmlTextCut(title, 10);
            hl.NavigateUrl = dr["Content"].ToString();
        }
    }

    private void BindData()
    {
        long id = Shove._Convert.StrToLong(hID.Value, -1);

        if (id < 0)
        {
            PF.GoError(ErrorNumber.Unknow, "您访问的数据不存在，可能是参数错误或内容已经被删除！", this.GetType().BaseType.FullName);

            return;
        }

        DataTable dt = Shove.Database.MSSQL.Select("select * from T_NewSpecial where Id = " + id);

        if (dt == null)
        {
            return;
        }
        if (dt.Rows.Count < 1)
        {
            return;
        }

        this.lbTitle.Text = dt.Rows[0]["Title"].ToString();
        this.lbDateTime.Text = dt.Rows[0]["DateTime"].ToString();
        this.lbContent.Text = dt.Rows[0]["Content"].ToString();
        
    }
   
    private void BindNewsDetail(DataRow dr)
    {
        hlNewsType.Text = dr["TypeName"].ToString();
    }

    private void BindComments(DataTable dt)
    {
        if (dt == null || dt.Rows.Count == 0)
        {
            NoNewsComments.Visible = true;

            return;
        }

        labNewsComments.Text = dt.Rows.Count.ToString();

        rptNewsComments.DataSource = dt;
        rptNewsComments.DataBind();
    }

    protected void btnComments_Click(object sender, EventArgs e)
    {
        System.Threading.Thread.Sleep(500);

        if (tbCommentserName.Text.Trim() == "")
        {
            Shove._Web.JavaScript.Alert(this.Page, "请输入您的姓名！");
            return;
        }

        if (tbContent.Text.Trim() == "")
        {
            Shove._Web.JavaScript.Alert(this.Page, "您的评论内容！");
            return;
        }

        long CommentserID = -1;

        if (_User != null)
        {
            CommentserID = _User.ID;
        }

        long ReturnValue = -1;
        string ReturnDescription = "";

        int Result = DAL.Procedures.P_NewsAddComments(_Site.ID, Shove._Convert.StrToLong(hID.Value, 0), DateTime.Now, CommentserID, tbCommentserName.Text.Trim(), tbContent.Text.Trim(), true, ref ReturnValue, ref ReturnDescription);

        if (Result < 0)
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", this.GetType().BaseType.FullName);

            return;
        }

        if (ReturnValue < 0)
        {
            Shove._Web.JavaScript.Alert(this.Page, ReturnDescription);

            return;
        }

        Shove._Web.JavaScript.Alert(this.Page, "您的评论已提交成功！");

        Shove._Web.Cache.ClearCache("NewsDetail" + hID.Value);

        BindData();
    }
}
