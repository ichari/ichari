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

using System.Text;
using Shove.Database;

public partial class Home_Room_FollowScheme : RoomPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            tbLotteryID.Value = Shove._Web.Utility.GetRequest("LotteryID");
            tbNumber.Value = Shove._Web.Utility.GetRequest("Number");
            BindData();
        }
    }

    #region Web 窗体设计器生成的代码

    override protected void OnInit(EventArgs e)
    {
        RequestLoginPage = this.Request.Url.AbsoluteUri;

        isRequestLogin = false;
        isAtFramePageLogin = false;

        base.OnInit(e);
    }

    #endregion

    private void BindData()
    {
        long UserID = -1;

        if (_User != null)
        {
            UserID = _User.ID;
        }

        hlAdd.NavigateUrl = "FollowFriendScheme.aspx?LotteryID=" + tbLotteryID.Value+"&Number="+tbNumber.Value+"";

        string strCmd = "SELECT ID,[Name],[Level],MaxFollowNumber FROM dbo.T_Users ";

        if (UserID > 0)
        {
            strCmd += " WHERE ID in (select FollowUserID from T_CustomFriendFollowSchemes where UserID = " + UserID.ToString() + " and LotteryID in (-1," + tbLotteryID.Value + "))   and ID <> " + UserID.ToString();
        }
        else
        {
            strCmd += " WHERE 0=1";
        }

        string sName = Shove._Web.Utility.FilteSqlInfusion(Name.Text.Trim());
        if (sName != "" && sName != "输入用户名")
        {
            strCmd += " and [Name] LIKE '%" + sName + "%'";
        }

        DataTable dt = MSSQL.Select(strCmd);

        if (dt == null)
        {
            PF.GoError(ErrorNumber.Unknow, "数据库繁忙，请重试", this.GetType().BaseType.FullName);

            return;
        }

        PF.DataGridBindData(g, dt, gPager);

        gPager.Visible = true;

    }

    protected void g_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.EditItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            DataRow dr = drv.Row;

            if (dr == null)
            {
                return;
            }

            //战绩
            TableCell Level = e.Item.Cells[1];

            Level.CssClass = "blue12";

            if (dr["Level"].ToString() == "0")
            {
                Level.Text = "<a href='../Web/Score.aspx?id=" + dr["ID"].ToString() + "&LotteryID=" + tbLotteryID.Value + "' title='点击查看' target='_blank'>-</a>";
            }
            else
            {
                int level = Shove._Convert.StrToInt(dr["Level"].ToString(), 0);

                string img = "";

                for (int i = 1; i <= level; i++)
                {
                    img += "<img src='Images/gold.gif'/>";
                }
                Level.Text = "<a href='../Web/Score.aspx?id=" + dr["ID"].ToString() + "&LotteryID=" + tbLotteryID.Value + "' title='点击查看' target='_blank'>"+img+"</a>";
            }
            //END

            //已定制/未定制
            TableCell Customization = e.Item.Cells[2];

            //好友定制跟单表缓存，★注意有用户定制跟单时需刷新缓存
            string CacheKey = "T_CustomFriendFollowSchemes"+tbLotteryID.Value;
            DataTable dt = Shove._Web.Cache.GetCacheAsDataTable(CacheKey);

            if (dt == null)
            {
                dt = MSSQL.Select("select * from T_CustomFriendFollowSchemes where LotteryID in (-1," + tbLotteryID.Value + ")");

                if (dt != null && dt.Rows.Count > 0)
                {
                    Shove._Web.Cache.SetCache(CacheKey, dt, 6000);
                }
            }

            int FollowUserCount = 0;

            if (dt != null && dt.Rows.Count > 0)
            {
                FollowUserCount = dt.Select("FollowUserID = " + dr["ID"].ToString()).Length;
            }

            Customization.Text = FollowUserCount.ToString() + "/" + dr["MaxFollowNumber"].ToString();
            //END

            //所有跟单人
            TableCell ViewAllFollowUsers = e.Item.Cells[3];

            if (FollowUserCount > 0)
            {
                ViewAllFollowUsers.CssClass = "blue12";
                ViewAllFollowUsers.Text = "<a href='javascript:;' onclick=\"showDialog('FollowFriendView.aspx?ID=" + dr["ID"].ToString() + "&FollowUserName=" + System.Web.HttpUtility.UrlEncode(dr["Name"].ToString()) + "');\">查看</a>";
            }
            else
            {
                ViewAllFollowUsers.Text = "-";
            }
            //END

            //定制状态
            TableCell Status = e.Item.Cells[4];

            int FollowStatus = -1;
            if (_User == null)
            {
                Status.Text = "未知";
            }
            else
            {
                if (dt.Select("UserID = " + _User.ID.ToString() + " and FollowUserID = " + dr["ID"].ToString()).Length > 0)
                {
                    Status.Text = "已定制";
                    FollowStatus = 1;
                }
                else
                {
                    Status.Text = "未定制";
                    FollowStatus = 0;
                }
            }
            //END

            //定制自动跟单
            TableCell Opt = e.Item.Cells[5];

            if (FollowStatus == -1)
            {
                Opt.Text = "-";

                return;
            }

            Label lbEdit = (Label)Opt.FindControl("lbEdit");
            LinkButton Cancel = (LinkButton)Opt.FindControl("Cancel");
            Cancel.Visible = false;
            Opt.CssClass = "blue12";

            if (FollowStatus == 0)
            {
                if (FollowUserCount >= Shove._Convert.StrToInt(dr["MaxFollowNumber"].ToString(), 200))
                {
                    Opt.Text = "已满额";
                }
                else
                {
                    Opt.Text = "<script>var e_script_" + dr["ID"].ToString() + "=\"parent.iframeFollowScheme.showDialog('FollowFriendSchemeAdd.aspx?FollowUserID=" + dr["ID"].ToString() + "&FollowUserName=" + System.Web.HttpUtility.UrlEncode(dr["Name"].ToString()) + "&LotteryID=" + tbLotteryID.Value + "&Number="+tbNumber.Value+"')\";</script><a href='javascript:;' onclick=\"if(parent.CreateLogin(this)){showDialog('FollowFriendSchemeAdd.aspx?FollowUserID=" + dr["ID"].ToString() + "&FollowUserName=" + System.Web.HttpUtility.UrlEncode(dr["Name"].ToString()) + "&LotteryID=" + tbLotteryID.Value + "&Number="+tbNumber.Value+"');}\">定制</a>";
                }

                Cancel.Visible = false;

                return;
            }

            if (FollowStatus == 1)
            {
                lbEdit.Text = "<a href='javascript:;' onclick=\"showDialog('FollowFriendSchemeEdit.aspx?FollowUserID=" + dr["ID"].ToString() + "&FollowUserName=" + System.Web.HttpUtility.UrlEncode(dr["Name"].ToString()) + "&UserID=" + _User.ID.ToString() + "')\">修改</a>";
                //lbEdit.Text = "<script>var e_script_" + dr["ID"].ToString() + "=\"parent.iframeFollowScheme.showDialog('FollowFriendSchemeEdit.aspx?FollowUserID=" + dr["ID"].ToString() + "&FollowUserName=" + System.Web.HttpUtility.UrlEncode(dr["Name"].ToString()) + "&&UserID=" + _User.ID.ToString() + "&Number="+tbNumber.Value+"')\";</script><a href='javascript:;' onclick=\"if(parent.CreateLogin(e_script_" + dr["ID"].ToString() + ")){showDialog('FollowFriendSchemeEdit.aspx?FollowUserID=" + dr["ID"].ToString() + "&FollowUserName=" + System.Web.HttpUtility.UrlEncode(dr["Name"].ToString()) + "&UserID=" + _User.ID.ToString() + "&Number="+tbNumber.Value+"');}\">修改</a>";
                Cancel.Visible = true;
                Cancel.Attributes.Add("onclick", "return isCancelFollowScheme()");

                return;
            }
            //END
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

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        BindData();
    }

    protected void g_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        //取消定制
        if (e.CommandName == "Cancel")
        {
            if (_User == null)
            {
                Shove._Web.JavaScript.Alert(this, "已退出登陆，请重新登陆", this.Request.Url.AbsoluteUri);

                return;
            }

            int FollowUserID = Shove._Convert.StrToInt(e.Item.Cells[6].Text, -1);

            if (FollowUserID < 0)
            {
                Shove._Web.JavaScript.Alert(this, "取消定制失败！");
                return;
            }

            string sql = "delete from T_CustomFriendFollowSchemes where FollowUserID = " + FollowUserID.ToString() + " and UserID = " + _User.ID.ToString();

            int i = MSSQL.ExecuteNonQuery(sql, new MSSQL.Parameter[0]);

            if (i < 0)
            {
                Shove._Web.JavaScript.Alert(this, "取消定制失败！");
            }
            else
            {
                //刷新缓存
                string CacheKey = "T_CustomFriendFollowSchemes"+tbLotteryID.Value;

                Shove._Web.Cache.ClearCache(CacheKey);

                BindData();
                Shove._Web.JavaScript.Alert(this, "取消定制成功！");
            }
        }
    }

    protected void btnList_Click(object sender, EventArgs e)
    {
        Name.Text = "";

        BindData();
    }
}
