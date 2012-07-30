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

public partial class Home_Room_EditFriendFollowScheme : RoomPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            BindData();
        }
    }

    #region Web 窗体设计器生成的代码

    override protected void OnInit(EventArgs e)
    {
        RequestLoginPage = this.Request.Url.AbsoluteUri;

        base.OnInit(e);
    }

    #endregion

    private void BindData()
    {
        DataTable dt = Shove.Database.MSSQL.Select("SELECT ID,[Name],[Level],MaxFollowNumber FROM dbo.T_Users WHERE ID in (select FollowUserID from T_CustomFriendFollowSchemes where UserID = " + _User.ID.ToString() + ") and ID <> " + _User.ID.ToString(), new Shove.Database.MSSQL.Parameter[0]);

        if (dt == null)
        {
            PF.GoError(ErrorNumber.Unknow, "参数错误，系统异常。", this.GetType().FullName);
        }

        g.DataSource = dt;
        g.DataBind();

        gPager.Visible = true;
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
                string CacheKey = "T_CustomFriendFollowSchemes"+HidLotteryID.Value;

                Shove._Web.Cache.ClearCache(CacheKey);

                BindData();
                Shove._Web.JavaScript.Alert(this, "取消定制成功！");
            }
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
                Level.Text = "<a href='../Web/Score.aspx?id=" + dr["ID"].ToString() + "&LotteryID=" + HidLotteryID.Value + "' title='点击查看' target='_blank'>-</a>";
            }
            else
            {
                Level.Text = "<a href='../Web/Score.aspx?id=" + dr["ID"].ToString() + "&LotteryID=" + HidLotteryID.Value + "' title='点击查看' target='_blank'><span style='background-image:url(Images/gold.gif); width:" + dr["Level"].ToString() + "px;background-repeat:repeat-x;'></span></a>";
            }
            //END

            //已定制/未定制
            TableCell Customization = e.Item.Cells[2];

            //好友定制跟单表缓存，★注意有用户定制跟单时需刷新缓存
            string CacheKey = "T_CustomFriendFollowSchemes"+HidLotteryID.Value;
            DataTable dt = Shove._Web.Cache.GetCacheAsDataTable(CacheKey);

            if (dt == null)
            {
                dt = MSSQL.Select("select * from T_CustomFriendFollowSchemes");

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
                    Opt.Text = "<script>var e_script_" + dr["ID"].ToString() + "=\"mainFrame.document.DZHYGD_Iframe.showDialog('FollowFriendSchemeAdd.aspx?FollowUserID=" + dr["ID"].ToString() + "&FollowUserName=" + System.Web.HttpUtility.UrlEncode(dr["Name"].ToString()) + "&LotteryID=" + HidLotteryID.Value + "')\";</script><a href='javascript:;' onclick=\"if(parent.parent.CreateLogin(this)){showDialog('FollowFriendSchemeAdd.aspx?FollowUserID=" + dr["ID"].ToString() + "&FollowUserName=" + System.Web.HttpUtility.UrlEncode(dr["Name"].ToString()) + "&LotteryID=" + HidLotteryID.Value + "');}\">定制</a>";
                }

                Cancel.Visible = false;

                return;
            }

            if (FollowStatus == 1)
            {
                //lbEdit.Text = "<a href='javascript:;' onclick=\"showDialog('FollowFriendSchemeEdit.aspx?FollowUserID=" + dr["ID"].ToString() + "&FollowUserName=" + System.Web.HttpUtility.UrlEncode(dr["Name"].ToString()) + "&UserID=" + _User.ID.ToString() + "')\">修改</a>";
                lbEdit.Text = "<script>var e_script_" + dr["ID"].ToString() + "=\"mainFrame.document.DZHYGD_Iframe.showDialog('FollowFriendSchemeEdit.aspx?FollowUserID=" + dr["ID"].ToString() + "&FollowUserName=" + System.Web.HttpUtility.UrlEncode(dr["Name"].ToString()) + "&&UserID=" + _User.ID.ToString() + "')\";</script><a href='javascript:;' onclick=\"if(parent.parent.CreateLogin(this)){showDialog('FollowFriendSchemeEdit.aspx?FollowUserID=" + dr["ID"].ToString() + "&FollowUserName=" + System.Web.HttpUtility.UrlEncode(dr["Name"].ToString()) + "&UserID=" + _User.ID.ToString() + "');}\">修改</a>";
                Cancel.Visible = true;

                return;
            }
            //END
        }
    }
}
