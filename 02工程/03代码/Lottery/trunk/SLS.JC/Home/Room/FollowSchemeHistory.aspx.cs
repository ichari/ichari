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

public partial class Home_Room_FollowSchemeHistory : RoomPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            BindDataForLottery();
            hdCurDiv.Value = Shove._Web.Utility.GetRequest("Type");
            BindDataForPlayType(1);
            BindDataForPlayType(2);
            BindDataForPlayType(3);

            BindDataForFriendFollowScheme();
            BindWhoFollowScheme();
        }
    }

    #region Web 窗体设计器生成的代码

    override protected void OnInit(EventArgs e)
    {
        RequestLoginPage = this.Request.Url.AbsoluteUri;

        base.OnInit(e);
    }

    #endregion

    /// <summary>
    /// 绑定ddl中的彩种,玩法
    /// </summary>
    private void BindDataForLottery()
    {
        ddlLottery.Items.Clear();
        ddlLottery.Items.Add(new ListItem("全部彩种", "-1"));

        ddlLotterySet.Items.Clear();
        ddlLotterySet.Items.Add(new ListItem("全部彩种", "-1"));
        BindDataForPlayType(2);
        ddlWhoLottery.Items.Clear();

        ddlWhoLottery.Items.Add(new ListItem("全部彩种", "-1"));
        BindDataForPlayType(3);
        if (_Site.UseLotteryList == "")
        {
            PF.GoError(ErrorNumber.Unknow, "暂无玩法", "Room_InvestHistory");

            return;
        }

        DataTable dt = new DAL.Tables.T_Lotteries().Open("[ID], [Name]", "[ID] in (" + (_Site.UseLotteryList == "" ? "-1" : _Site.UseLotteryList) + ")", "[Order]");

        if (dt == null)
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", "Room_InvestHistory");

            return;
        }
        //绑定彩种
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            string strLotteryName = dt.Rows[i]["Name"].ToString();
            if (dt.Rows[i]["ID"].ToString() == "61")
            {
                strLotteryName = strLotteryName.Replace("江西","");
            }
            ddlLottery.Items.Add(new ListItem(strLotteryName, dt.Rows[i]["ID"].ToString()));
            ddlLotterySet.Items.Add(new ListItem(strLotteryName, dt.Rows[i]["ID"].ToString()));
            ddlWhoLottery.Items.Add(new ListItem(strLotteryName, dt.Rows[i]["ID"].ToString()));
        }
    }

    /// <summary>
    /// 绑定playtyoe的值
    /// </summary>
    /// <param name="type">判断绑定的是哪个ddl</param>
    private void BindDataForPlayType(int type)
    {

        DataTable dt = null;

        if (type == 1)
        {
            if (ddlLottery.Items.Count < 1)
            {
                return;
            }

            ddlPlayType.Items.Clear();
            ddlPlayType.Items.Add(new ListItem("全部玩法", "-1"));
            dt = new DAL.Views.V_PlayTypes().Open("ID,LotteryID,Name,LotteryName,BuyFileName", "LotteryID=" + ddlLottery.SelectedValue, "");
        }
        else if (type == 2)
        {
            if (ddlLotterySet.Items.Count < 1)
            {
                return;
            }

            ddlPlayTypeSet.Items.Clear();
            ddlPlayTypeSet.Items.Add(new ListItem("全部玩法", "-1"));
            dt = new DAL.Views.V_PlayTypes().Open("ID,LotteryID,Name,LotteryName,BuyFileName", "LotteryID=" + ddlLotterySet.SelectedValue, "");
        }
        else if (type == 3)
        {
            if (ddlWhoLottery.Items.Count < 1)
            {
                return;
            }
            ddlWhoPlaytype.Items.Clear();
            ddlWhoPlaytype.Items.Add(new ListItem("全部玩法", "-1"));
            dt = new DAL.Views.V_PlayTypes().Open("ID,LotteryID,Name,LotteryName,BuyFileName", "LotteryID=" + ddlWhoLottery.SelectedValue, "");
        }

        if (dt == null)
        {
            PF.GoError(ErrorNumber.Unknow, "暂无玩法", this.GetType().FullName);

            return;
        }

        if (type == 1)
        {
            for (int j = 0; j < dt.Rows.Count; j++)
            {
                string strLotteryName = dt.Rows[j]["Name"].ToString().ToString();
                if (dt.Rows[j]["Name"].ToString() == "61")
                {
                    strLotteryName = strLotteryName.Replace("江西", "");
                }

                if (dt.Rows[j]["Name"].ToString() != "混合投注")
                {
                    ddlPlayType.Items.Add(new ListItem(strLotteryName, dt.Rows[j]["ID"].ToString()));
                }
            }
        }
        else if (type == 2)
        {

            for (int j = 0; j < dt.Rows.Count; j++)
            {
                string strLotteryName = dt.Rows[j]["Name"].ToString().ToString();
                if (dt.Rows[j]["Name"].ToString() == "61")
                {
                    strLotteryName = strLotteryName.Replace("江西", "");
                }

                if (dt.Rows[j]["Name"].ToString() != "混合投注")
                {
                    ddlPlayTypeSet.Items.Add(new ListItem(strLotteryName, dt.Rows[j]["ID"].ToString()));
                }
            }
        }
        else if (type == 3)
        {
            for (int j = 0; j < dt.Rows.Count; j++)
            {
                string strLotteryName = dt.Rows[j]["Name"].ToString().ToString();
                if (dt.Rows[j]["Name"].ToString() == "61")
                {
                    strLotteryName = strLotteryName.Replace("江西", "");
                }

                if (dt.Rows[j]["Name"].ToString() != "混合投注")
                {
                    ddlWhoPlaytype.Items.Add(new ListItem(strLotteryName, dt.Rows[j]["ID"].ToString()));
                }
            }
        }
    }

    protected void ddlLottery_SelectedIndexChanged(object sender, System.EventArgs e)
    {
        DropDownList ddl = (DropDownList)sender;
        if (ddl.ID == "ddlLottery")
        {
            BindDataForPlayType(1);
        }
        else if (ddl.ID == "ddlWhoLottery")
        {
            BindDataForPlayType(3);
            BindWhoFollowScheme();
        }
        else
        {
            BindDataForPlayType(2);
            BindDataForFriendFollowScheme();
        }
    }

    protected void ddlPlayType_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindData();
    }

    /// <summary>
    /// 绑定谁要跟我单
    /// </summary>
    private void BindWhoFollowScheme()
    {
        DataTable dt = null;
        string strCmd = "select UserID , LotteryID , PlayTypeID,MoneyStart,MoneyEnd , [DateTime] from T_CustomFriendFollowSchemes where FollowUserID = " + _User.ID.ToString();

        if (ddlLotterySet.SelectedValue != "-1")
        {
            strCmd += " and LotteryID = " + ddlLotterySet.SelectedValue + "";
        }

        if (ddlPlayTypeSet.SelectedValue != "-1")
        {
            strCmd += " and PlayTypeID = " + ddlPlayTypeSet.SelectedValue + "";
        }

        dt = Shove.Database.MSSQL.Select(strCmd, new Shove.Database.MSSQL.Parameter[0]);

        if (dt == null)
        {
            PF.GoError(ErrorNumber.Unknow, "参数错误，系统异常。", this.GetType().FullName);
        }
        PF.DataGridBindData(gWhoFollowSchemes, dt, gPageWhoFollowSchemes);

    }
    
    private void BindDataForFriendFollowScheme()
    {
        long UserID = -1;

        if (_User != null)
        {
            UserID = _User.ID;
        }

        DataTable dt = null;
        string sName = Shove._Web.Utility.FilteSqlInfusion(TxtName.Text.Trim());
        if (sName != "")
        {
            string strCmd = "SELECT ID,[NAME],[Level],MaxFollowNumber FROM dbo.T_Users ";


            if (sName != "" && sName != "输入用户名")
            {
                strCmd += " WHERE [Name] LIKE '%" + sName + "%' and ID not in (select FollowUserID from T_CustomFriendFollowSchemes where UserID = " + UserID.ToString() + " and LotteryID in(-1," + ddlLotterySet.SelectedValue + "))";
            }
            else
            {
                strCmd += " WHERE 0=1";
            }

            if (UserID > 0)
            {
                strCmd += " and ID <> " + UserID.ToString();
            }

            dt = MSSQL.Select(strCmd);

            if (dt == null)
            {
                PF.GoError(ErrorNumber.Unknow, "数据库繁忙，请重试", this.GetType().BaseType.FullName);

                return;
            }
        }
        else
        {
            string strCmd = "select FollowUserID from T_CustomFriendFollowSchemes where UserID = " + _User.ID.ToString();

            if (ddlLotterySet.SelectedValue != "-1")
            {
                strCmd += " and LotteryID = " + ddlLotterySet.SelectedValue + "";
            }

            if (ddlPlayTypeSet.SelectedValue != "-1")
            {
                strCmd += " and PlayTypeID = " + ddlPlayTypeSet.SelectedValue + "";
            }

            dt = Shove.Database.MSSQL.Select("SELECT ID,[Name],[Level],MaxFollowNumber FROM dbo.T_Users WHERE ID in (" + strCmd + ")", new Shove.Database.MSSQL.Parameter[0]);

            if (dt == null)
            {
                PF.GoError(ErrorNumber.Unknow, "参数错误，系统异常。", this.GetType().FullName);
            }
        }
        gSetFollowScheme.DataSource = dt;
        gSetFollowScheme.DataBind();

        gPager.Visible = true;
    }

    /// <summary>
    /// 绑定定制的跟单记录
    /// </summary>
    private void BindData()
    {

        string Condition = " a.[UserID] = " + _User.ID.ToString() + "  and StartTime < GetDate() ";

        if (ddlLottery.SelectedValue != "-1")
        {
            Condition += " and LotteryID=" + Shove._Convert.StrToInt(ddlLottery.SelectedValue, -1).ToString() + "";
        }

        if (ddlPlayType.SelectedValue != "-1")
        {
            Condition += " and PlayTypeID = " + Shove._Convert.StrToInt(ddlPlayType.SelectedValue, -1).ToString();
        }

        if (Condition != "")
        {
            Condition += " and isAutoFollowScheme = 1";
        }
        else
        {
            Condition = "isAutoFollowScheme = 1";
        }


        DataTable dt = Shove.Database.MSSQL.Select("select a.*,b.Memo  from V_BuyDetailsWithQuashedAll a left join (select Memo,SchemeID from T_UserDetails where OperatorType = 9)b on a.SchemeID = b.SchemeID where  " + Condition + " order by a.ID desc");

        if (dt == null)
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", "Room_InvestHistory");

            return;
        }

        PF.DataGridBindData(g, dt, gPager);

        gPager.Visible = true;

        //本页记录
        this.lblPageBuyMoney.Text = PF.GetSumByColumn(dt, 10, true, gPager.PageSize, gPager.PageIndex).ToString("N");
        this.lblPageReward.Text = PF.GetSumByColumn(dt, 8, true, gPager.PageSize, gPager.PageIndex).ToString("N");

        //总记录
        this.lblTotalBuyMoney.Text = PF.GetSumByColumn(dt, 10, false, gPager.PageSize, gPager.PageIndex).ToString("N");
        this.lblTotalReward.Text = PF.GetSumByColumn(dt, 8, false, gPager.PageSize, gPager.PageIndex).ToString("N");
    }

    protected void btnSearch_Click(object sender, System.EventArgs e)
    {
        BindDataForFriendFollowScheme();
    }

    protected void btnFollowScheme_Click(object sender, EventArgs e)
    {
        BindWhoFollowScheme();
    }

    protected void gPager_PageWillChange(object Sender, Shove.Web.UI.PageChangeEventArgs e)
    {
        Shove.Web.UI.ShoveGridPager gPager = (Shove.Web.UI.ShoveGridPager)Sender;

        if (gPager.ID == "gPager")
        {
            hdCurDiv.Value = "divMyFollowSchemes";
            BindData();
        }
        else if (gPager.ID == "gPagerSetFollowScheme")
        {
            hdCurDiv.Value = "divSetMyFollowSchemes";
            BindDataForFriendFollowScheme();
        }
    }

    protected void g_SortCommand(object source, DataGridSortCommandEventArgs e)
    {
        DataGrid grid = (DataGrid)source;
        if (grid.ID == "g")
        {
            BindData();
        }
        else if (grid.ID == "gSetFollowScheme")
        {
            BindDataForFriendFollowScheme();
        }
        else if (grid.ID == "gWhoFollowSchemes")
        {

        }
    }

    protected void g_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.EditItem)
        {
            long SchemeID = Shove._Convert.StrToLong(e.Item.Cells[12].Text, 0);
            string str = e.Item.Cells[1].Text;

            if (str.Length > 6)
            {
                str = str.Substring(0, 5) + "..";
            }

            e.Item.Cells[1].Text = "<span class='red12_2'><a href='Scheme.aspx?id=" + SchemeID.ToString() + "' target='_blank'>" + str + "</a></span>";

            str = e.Item.Cells[2].Text.Trim();

            if (str.Length > 6)
            {
                str = str.Substring(0, 5) + "..";
            }

            e.Item.Cells[2].Text = str;

            double money;

            money = Shove._Convert.StrToDouble(e.Item.Cells[4].Text, 0);
            e.Item.Cells[4].Text = (money == 0) ? "" : money.ToString("N");

            money = Shove._Convert.StrToDouble(e.Item.Cells[6].Text, 0);
            e.Item.Cells[6].Text = (money == 0) ? "" : money.ToString("N");

            double i = Shove._Convert.StrToDouble(e.Item.Cells[3].Text, 0);
            double j = Shove._Convert.StrToDouble(e.Item.Cells[5].Text, 0);

            if (j >= i)
            {
                e.Item.Cells[7].Text = "100%";
            }
            else
            {
                if (i > 0)
                {
                    e.Item.Cells[7].Text = Math.Round(j / i * 100, 2).ToString() + "%";
                }
            }

            money = Shove._Convert.StrToDouble(e.Item.Cells[8].Text, 0);
            e.Item.Cells[8].Text = (money == 0) ? "" : money.ToString("N");

            money = Shove._Convert.StrToDouble(e.Item.Cells[9].Text, 0);
            e.Item.Cells[9].Text = (money == 0) ? "" : money.ToString("N");

            short QuashStatus = Shove._Convert.StrToShort(e.Item.Cells[13].Text, 0);

            if (QuashStatus != 0)
            {
                e.Item.Cells[11].Text = "已撤单";

                if (QuashStatus == 1)
                {
                    e.Item.Cells[11].Text = "用户撤单";
                }
                else
                {
                    if(!string.IsNullOrEmpty(e.Item.Cells[17].Text.Trim()))
                    {
                        e.Item.Cells[11].Text = e.Item.Cells[17].Text;
                    }
                }
            }
            else
            {
                bool Buyed = Shove._Convert.StrToBool(e.Item.Cells[14].Text, false);

                if (Buyed)
                {
                    e.Item.Cells[11].Text = "<Font color=\'Red\'>已成功</font>";
                }
                else
                {
                    int BuyedShare = Shove._Convert.StrToInt(e.Item.Cells[16].Text, 0);
                    int SchemeShare = Shove._Convert.StrToInt(e.Item.Cells[3].Text, 0);

                    if (BuyedShare >= SchemeShare)
                    {
                        e.Item.Cells[11].Text = "<Font color=\'Red\'>已满员</font>";
                    }
                    else
                    {
                        e.Item.Cells[11].Text = "未成功";
                    }
                }
            }
        }
    }

    protected void gSetFollowScheme_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
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
                Level.Text = "<a href='../Web/Score.aspx?id=" + dr["ID"].ToString() + "' title='点击查看' target='_blank'>-</a>";
            }
            else
            {
                Level.Text = "<a href='../Web/Score.aspx?id=" + dr["ID"].ToString() + "' title='点击查看' target='_blank'><span style='background-image:url(Images/gold.gif); width:" + dr["Level"].ToString() + "px;background-repeat:repeat-x;'></span></a>";
            }
            //END

            //已定制/未定制
            TableCell Customization = e.Item.Cells[2];

            //好友定制跟单表缓存，★注意有用户定制跟单时需刷新缓存
            string CacheKey = "T_CustomFriendFollowSchemes" + ddlLotterySet.SelectedValue;
            DataTable dt = Shove._Web.Cache.GetCacheAsDataTable(CacheKey);

            if (dt == null)
            {
                if (ddlLotterySet.SelectedValue == "-1")
                {
                    dt = MSSQL.Select("select * from T_CustomFriendFollowSchemes");
                }
                else
                {
                    dt = MSSQL.Select("select * from T_CustomFriendFollowSchemes where LotteryID in (-1," + ddlLotterySet.SelectedValue + ")");
                }

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
                    Opt.Text = "<a href='FollowFriendSchemeAdd_User.aspx?FollowUserID=" + dr["ID"].ToString() + "&FollowUserName=" + System.Web.HttpUtility.UrlEncode(dr["Name"].ToString()) + "&LotteryID=" + ddlLotterySet.SelectedValue + "'>定制</a>";
                    //Opt.Text = "<script>var e_script_" + dr["ID"].ToString() + "=\"showDialog('FollowFriendSchemeAdd.aspx?FollowUserID=" + dr["ID"].ToString() + "&FollowUserName=" + System.Web.HttpUtility.UrlEncode(dr["Name"].ToString()) + "')\";</script><a href='javascript:;' onclick=\"if(parent.document.iframeTop.CreateLogin(e_script_" + dr["ID"].ToString() + ")){showDialog('FollowFriendSchemeAdd.aspx?FollowUserID=" + dr["ID"].ToString() + "&FollowUserName=" + System.Web.HttpUtility.UrlEncode(dr["Name"].ToString()) + "');}\">定制</a>";
                }

                Cancel.Visible = false;

                return;
            }

            if (FollowStatus == 1)
            {
                lbEdit.Text = "<a href='javascript:;' onclick=\"showDialog('FollowFriendSchemeEdit.aspx?FollowUserID=" + dr["ID"].ToString() + "&FollowUserName=" + System.Web.HttpUtility.UrlEncode(dr["Name"].ToString()) + "&UserID=" + _User.ID.ToString() + "')\">修改</a>";
                //lbEdit.Text = "<script>var e_script_" + dr["ID"].ToString() + "=\"parent.iframeFollowScheme.showDialog('FollowFriendSchemeEdit.aspx?FollowUserID=" + dr["ID"].ToString() + "&FollowUserName=" + System.Web.HttpUtility.UrlEncode(dr["Name"].ToString()) + "&&UserID=" + _User.ID.ToString() + "')\";</script><a href='javascript:;' onclick=\"if(parent.document.iframeTop.CreateLogin(e_script_" + dr["ID"].ToString() + ")){showDialog('FollowFriendSchemeEdit.aspx?FollowUserID=" + dr["ID"].ToString() + "&FollowUserName=" + System.Web.HttpUtility.UrlEncode(dr["Name"].ToString()) + "&UserID=" + _User.ID.ToString() + "');}\">修改</a>";
                Cancel.Visible = true;
                Cancel.Attributes.Add("onclick", "return isCancelFollowScheme()");

                return;
            }
            //END
        }
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
                string CacheKey = "T_CustomFriendFollowSchemes" + ddlLotterySet.SelectedValue;

                Shove._Web.Cache.ClearCache(CacheKey);

                BindDataForFriendFollowScheme();
                Shove._Web.JavaScript.Alert(this, "取消定制成功！");
            }
        }
    }

    /// <summary>
    /// 获得用户名
    /// </summary>
    /// <param name="_userId">用户ID</param>
    /// <returns>用户名</returns>
    protected string GetUserName(object _userId)
    {
        string userId = _userId.ToString();
        DataTable dt = new DAL.Tables.T_Users().Open("Name", "id = " + userId, "");
        return dt.Rows[0][0].ToString();
    }

    /// <summary>
    /// 获得彩种
    /// </summary>
    /// <param name="_lotteryId">彩种ID</param>
    /// <returns>彩种Name</returns>
    protected string GetLottery(object _lotteryId)
    {
        int lotteryId = Shove._Convert.StrToInt(_lotteryId.ToString(), 0);
        if (lotteryId < 1)
        {
            return "全部彩种";
        }
        else
        {
            DataTable dt = new DAL.Tables.T_Lotteries().Open("Name", "id = " + lotteryId, "");
            return dt.Rows[0][0].ToString();
        }

    }

    /// <summary>
    /// 获得玩法
    /// </summary>
    /// <param name="_playTypeId">玩法ID</param>
    /// <returns>玩法Name</returns>
    protected string GetPlayType(object _playTypeId)
    {
        int playTypeId = Shove._Convert.StrToInt(_playTypeId.ToString(), 0);
        if (playTypeId < 1)
        {
            return "全部玩法";
        }
        else
        {
            DataTable dt = new DAL.Tables.T_PlayTypes().Open("Name", "id = " + playTypeId, "");
            return dt.Rows[0][0].ToString();
        }

    }
}