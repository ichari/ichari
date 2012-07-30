using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

using System.Text;
using System.IO;
using System.Security.Cryptography;

public partial class Home_Room_MyIcaile : RoomPageBase
{
    int[] arrLotteries = new int[] { 5, 6, 29, 39 };

    public static int LotteryID = 0;

    public string isAdministrator = "false";

    public string Balance;
    public string UserName;

    private DataTable dta = null;
    #region Web 窗体设计器生成的代码

    override protected void OnInit(EventArgs e)
    {
        RequestLoginPage = "MyIcaile.aspx";

        isRequestLogin = true;
        isAtFramePageLogin = false;

        base.OnInit(e);
    }

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            BindDataGetWinNumber();
            //BindData();
            BindDataInvestHistory();
            BindDataReward();
            BindDataForLottery();

            Balance = "0";

            //divIsLogin.Visible = true;
            myIcaileTab.Style.Add("display", "none");

            if (_User != null)
            {

                //divIsLogin.Visible = false;
                myIcaileTab.Style.Remove("display");

                BindViewAccountData();
            }
            PopNews();
        }

        if (_User != null)
        {
            isAdministrator = DAL.Functions.F_GetIsAdministrator(_Site.ID, _User.ID).ToString().ToLower();
            Balance = _User.Balance.ToString();
            UserName = _User.Name.ToString();
            StringBuilder sb = new StringBuilder();
            sb.Append("<a href='OnlinePay/Default.aspx'>").Append("【我要充值】").Append("</a>");
            sp_isGoLogin.InnerHtml = sb.ToString();
        }
        else
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<a onclick='return CreateLogin(this)' style='cursor:hand;'>").Append("【我要充值】").Append("</a>");
            sp_isGoLogin.InnerHtml = sb.ToString();
        }
    }

    private void PopNews()
    {
        if (_User == null)
        {
            return;
        }

        string CacheKeyName = "Get_Win_Info_" + _User.ID.ToString();

        DataTable dt = Shove._Web.Cache.GetCacheAsDataTable("Get_Win_Info_" + _User.ID.ToString());
        if (dt == null)
        {
            dt = new DAL.Views.V_BuyDetails().Open("top 1 LotteryName,IsuseName,WinLotteryNumber,LotteryNumber,WinMoneyNoWithTax", "IsOpened = 1 and UserID = " + _User.ID, "ID DESC");

            if (dt == null)
            {
                PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试(537)", this.GetType().FullName);

                return;
            }
            Shove._Web.Cache.SetCache(CacheKeyName, dt, 300);
        }

        StringBuilder sb = new StringBuilder();
        sb.Append(_User.Name).Append(",");
        if (dt.Rows.Count != 0)
        {

            string SiteUrl = Shove._Web.Utility.GetUrl();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                sb.Append(dt.Rows[i]["LotteryName"].ToString()).Append("第")
                    .Append(dt.Rows[i]["IsuseName"].ToString()).Append("期中奖号码是 ")
                    .Append(dt.Rows[i]["WinLotteryNumber"].ToString())
                    .Append(" 您投注的号码是")
                    .Append(Shove._String.Cut(dt.Rows[i]["LotteryNumber"].ToString(), 15)).Append(" ，")
                    .Append(Shove._Convert.StrToDouble(dt.Rows[i]["WinMoneyNoWithTax"].ToString(), 0) > 0 ? "<font style='color:red;'>中奖了</font>，希望您再接再厉，夺得更多奖金。" : "没有中奖，希望您继续努力，祝您早日中大奖。");

            }

            string FloatNotify = HmtlManage.GetHtml(AppDomain.CurrentDomain.BaseDirectory + "Home/Room/Template/FloatNotify.html");
            label1.Text = FloatNotify.Replace("$FloatNotifyContent$", sb.ToString());

        }
    }

    /// <summary>
    /// 开奖公告
    /// </summary>
    private void BindDataGetWinNumber()
    {
        DataTable dt = Shove._Web.Cache.GetCacheAsDataTable("Room_Welcome_GetWinNumber");

        if (dt == null)
        {
            int ReturnValue = 0;
            string ReturnDescription = "";

            DataSet ds = null;

            DAL.Procedures.P_GetWinLotteryNumber(ref ds, _Site.ID, -1, 0, ref ReturnValue, ref ReturnDescription);

            if (ds == null)
            {
                PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试(128)", this.GetType().BaseType.FullName);

                return;
            }

            if (ReturnValue < 0)
            {
                Shove._Web.JavaScript.Alert(this.Page, ReturnDescription);

                return;
            }

            if (ds.Tables.Count < 1)
            {
                PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试(142)", this.GetType().BaseType.FullName);

                return;
            }

            dt = ds.Tables[0];
            Shove._Web.Cache.SetCache("Room_Welcome_GetWinNumber", dt, 600);
        }

        //if (dt != null && dt.Rows.Count > 0)
        //{
        //    lbNum1.Text = "";

        //    for (int i = 0; i < dt.Rows.Count; i++)
        //    {
        //        if (dt.Rows[i]["LotteryID"].ToString() == SLS.Lottery.SSQ.sID && lbNum1.Text == "")
        //        {
        //            lbNum1.Text = dt.Rows[i]["WinLotteryNumber"].ToString();
        //            lbqishu1.Text = dt.Rows[i]["IsuseName"].ToString() + "期";

        //            if (("247").IndexOf(GetDateofWeek(DateTime.Now.DayOfWeek.ToString())) > -1)
        //            {
        //                lbSsq.Text = "今天开奖";
        //            }
        //            else if (("136").IndexOf(GetDateofWeek(DateTime.Now.DayOfWeek.ToString())) > -1)
        //            {
        //                lbSsq.Text = "明天开奖";
        //            }
        //            else
        //            {
        //                lbSsq.Text = "后天开奖";
        //            }
        //        }
        //        else if (dt.Rows[i]["LotteryID"].ToString() == SLS.Lottery.FC3D.sID && lbNum2.Text == "")
        //        {
        //            lbNum2.Text = dt.Rows[i]["WinLotteryNumber"].ToString();
        //            lbqishu2.Text = dt.Rows[i]["IsuseName"].ToString() + "期";

        //            lbFc3d.Text = "今天开奖";
        //        }
        //        else if (dt.Rows[i]["LotteryID"].ToString() == SLS.Lottery.QLC.sID && lbNum3.Text == "")
        //        {
        //            lbNum3.Text = dt.Rows[i]["WinLotteryNumber"].ToString();
        //            lbqishu3.Text = dt.Rows[i]["IsuseName"].ToString() + "期";

        //            if (("135").IndexOf(GetDateofWeek(DateTime.Now.DayOfWeek.ToString())) > -1)
        //            {
        //                lbQlc.Text = "今天开奖";
        //            }
        //            else if (("247").IndexOf(GetDateofWeek(DateTime.Now.DayOfWeek.ToString())) > -1)
        //            {
        //                lbQlc.Text = "明天开奖";
        //            }
        //            else if (("6").IndexOf(GetDateofWeek(DateTime.Now.DayOfWeek.ToString())) > -1)
        //            {
        //                lbQlc.Text = "后天开奖";
        //            }
        //        }
        //        else if (dt.Rows[i]["LotteryID"].ToString() == SLS.Lottery.SHSSL.sID && lbNum4.Text == "")
        //        {
        //            lbNum4.Text = dt.Rows[i]["WinLotteryNumber"].ToString();
        //            lbqishu4.Text = dt.Rows[i]["IsuseName"].ToString() + "期";

        //            lbSsl.Text = "30分钟开奖";
        //        }
        //        else if (dt.Rows[i]["LotteryID"].ToString() == SLS.Lottery.HD15X5.sID && lbNum5.Text == "")
        //        {
        //            lbNum5.Text = dt.Rows[i]["WinLotteryNumber"].ToString();
        //            lbqishu5.Text = dt.Rows[i]["IsuseName"].ToString() + "期";

        //            lb15X5.Text = "今天开奖";
        //        }
        //        else if (dt.Rows[i]["LotteryID"].ToString() == SLS.Lottery.DF6J1.sID && lbNum6.Text == "")
        //        {
        //            lbNum6.Text = dt.Rows[i]["WinLotteryNumber"].ToString();
        //            lbqishu6.Text = dt.Rows[i]["IsuseName"].ToString() + "期";

        //            if (("136").IndexOf(GetDateofWeek(DateTime.Now.DayOfWeek.ToString())) > -1)
        //            {

        //                lbDF6J1.Text = "今天开奖";
        //            }
        //            else if (("257").IndexOf(GetDateofWeek(DateTime.Now.DayOfWeek.ToString())) > -1)
        //            {
        //                lbDF6J1.Text = "明天开奖";
        //            }
        //            else if (("4").IndexOf(GetDateofWeek(DateTime.Now.DayOfWeek.ToString())) > -1)
        //            {
        //                lbDF6J1.Text = "后天开奖";
        //            }
        //        }
        //        else if (dt.Rows[i]["LotteryID"].ToString() == SLS.Lottery.TTCX4.sID && lbNum7.Text == "")
        //        {
        //            lbNum7.Text = dt.Rows[i]["WinLotteryNumber"].ToString();
        //            lbqishu7.Text = dt.Rows[i]["IsuseName"].ToString() + "期";

        //            lbTtc.Text = "今天开奖";
        //        }
        //    }
        //}
    }

    protected void dlExpertsCommends_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.EditItem)
        {
            DataRow dr = dta.Rows[e.Item.ItemIndex];

            Label Title1 = (Label)e.Item.FindControl("Title1");

            Title1.Text = dr["Title1"].ToString();
            if (dr["Title1"].ToString().Length > 18)
            {
                Title1.Text = dr["Title1"].ToString().Substring(0, 18);
            }
        }
    }

    private void BindDataInvestHistory()
    {
        if (_User == null)
        {
            return;
        }

        string CacheKey = "Home_Room_Invest_BindHistoryData" + _User.ID.ToString();
        DataTable dt = Shove._Web.Cache.GetCacheAsDataTable(CacheKey);

        if (dt == null)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("select * from (select LotteryID,LotteryName,PlayTypeID,InitiateName,PlayTypeName, ")
                .Append("SchemeShare,a.Money,b.Share,b.DetailMoney,SchemeWinMoney, b.WinMoneyNoWithTax,a.DateTime, ")
                .Append("b.SchemeID,QuashStatus,Buyed,AssureMoney,BuyedShare,IsOpened,c.Memo  from   ")
                .Append("(select UserID,SchemeID,SUM(Share) as Share,SUM(DetailMoney) as DetailMoney, ")
                .Append("sum(WinMoneyNoWithTax) as  WinMoneyNoWithTax  from V_BuyDetailsWithQuashedAll   ")
                .Append("where  UserID = " + _User.ID.ToString() + " and InitiateUserID = UserID group by SchemeID,UserID)b ")
                .Append("left join (select * from V_BuyDetailsWithQuashedAll where UserID = " + _User.ID.ToString() + " and   ")
                .Append("UserID = InitiateUserID and isWhenInitiate = 1)a ")
                .Append("on a.UserID = b.UserID and ")
                .Append("a.SchemeID = b.SchemeID  left join (select SchemeID,Memo from T_UserDetails where ")
                .Append("OperatorType = 9 and UserID = " + _User.ID.ToString() + ") c  ")
                .Append("on b.SchemeID = c.SchemeID union select  LotteryID,LotteryName,PlayTypeID,InitiateName, ")
                .Append("PlayTypeName,SchemeShare,a.Money,Share,DetailMoney,SchemeWinMoney, WinMoneyNoWithTax, ")
                .Append("a.DateTime,a.SchemeID,QuashStatus,Buyed,AssureMoney,BuyedShare,IsOpened,b.Memo from  ")
                .Append("(select * from V_BuyDetailsWithQuashedAll where UserID = " + _User.ID.ToString() + " and UserID<>InitiateUserID) a left join (select SchemeID,Memo from T_UserDetails where  ")
                .Append("OperatorType = 9 and UserID = " + _User.ID.ToString() + ")b on a.SchemeID = b.SchemeID)a order by DateTime desc");

            dt = Shove.Database.MSSQL.Select(sb.ToString());

            if (dt == null)
            {
                PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", this.GetType().BaseType.FullName);

                return;
            }

            Shove._Web.Cache.SetCache(CacheKey, dt, 60);
        }

        gInvestHistory.DataSource = dt;
        gInvestHistory.DataBind();

        //总计
        this.lblBuySum.Text = PF.GetSumByColumn(dt, 8, false, gInvestHistory.PageSize, gPagerHistory.PageIndex).ToString("N");
        this.lblSumWinMoney.Text = PF.GetSumByColumn(dt, 9, false, gInvestHistory.PageSize, gPagerHistory.PageIndex).ToString("N");

        this.lblMySumWinMoney.Text = PF.GetSumByColumn(dt, 10, false, gInvestHistory.PageSize, gPagerHistory.PageIndex).ToString("N");

        double winSum = Shove._Convert.StrToDouble(this.lblSumWinMoney.Text, 0);    //中奖总额
        double buySum = Shove._Convert.StrToDouble(this.lblBuySum.Text, 0);    //购买总额
        double winMoney = winSum - buySum;
        if (winMoney <= 0)
        {
            this.lblSumWinProfitPoints.Text = "0.00";
        }
        else
        {
            double winRate = winMoney / buySum;
            if (winRate > 1)
            {
                this.lblSumWinProfitPoints.Text = Math.Round(winRate, 2).ToString("N") + "倍";
            }
            else
            {
                this.lblSumWinProfitPoints.Text = (Math.Round(winRate, 2) * 100).ToString("N") + "%";
            }
        }




        lbgInvestHistoryMessage.Text = "显示 10 条，共 " + dt.Rows.Count.ToString() + " 条记录 ，<span class=\"blue12_line\"><a  href='InvestHistory.aspx'>[查看全部记录]</a></span>";
    }

    protected void gInvestHistory_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.EditItem)
        {

            string str1 = e.Item.Cells[0].Text;
            if (str1.Length > 6)
            {
                e.Item.Cells[0].Text = str1.Substring(0, 6) + "…";
            }

            e.Item.Cells[0].Attributes.Add("title", str1);

            long SchemeID = Shove._Convert.StrToLong(e.Item.Cells[13].Text, 0);
            string str = e.Item.Cells[2].Text;

          


            if (str.Length > 6)
            {
                str = str.Substring(0, 5) + "..";
            }

            e.Item.Cells[2].Text = "<span class='red12_2'><a href='Scheme.aspx?id=" + SchemeID.ToString() + "' target='_blank'>" + str + "</a></span>";

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

            if (money == 0)
            {
                e.Item.Cells[10].Text = "<a href='Scheme.aspx?id=" + SchemeID.ToString() + "' target='_blank'>" + "未中奖" + "</a>";
                e.Item.Cells[10].Style.Add(HtmlTextWriterStyle.Color, "FFFFCC");
            }
            else
            {
                e.Item.Cells[10].Text = "<a href='Scheme.aspx?id=" + SchemeID.ToString() + "' target='_blank'>" + "中奖啦!" + "</a>";
            }

            bool IsOpened = false;

            try
            {
                DataRowView drv = (DataRowView)e.Item.DataItem;
                DataRow dr = drv.Row;

                IsOpened = Shove._Convert.StrToBool(dr["IsOpened"].ToString(), false);

                if (!IsOpened)
                {
                    e.Item.Cells[10].Text = "<a href='Scheme.aspx?id=" + SchemeID.ToString() + "' target='_blank'>" + "未开奖" + "</a>";
                    e.Item.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#FFFFCA");
                }
            }
            catch { }

            if (!IsOpened)
            {
                short QuashStatus = Shove._Convert.StrToShort(e.Item.Cells[14].Text, 0);

                if (QuashStatus != 0)
                {
                    string StatusDescription = "已撤单";

                    if (QuashStatus == 1)
                    {
                        StatusDescription = "用户撤单";
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(e.Item.Cells[18].Text.Trim()))
                        {
                            StatusDescription = e.Item.Cells[18].Text;
                        }
                    }

                    e.Item.Cells[12].Text = "<a href='Scheme.aspx?id=" + SchemeID.ToString() + "' target='_blank'>" + StatusDescription + "</a>";
                }
                else
                {
                    bool Buyed = Shove._Convert.StrToBool(e.Item.Cells[15].Text, false);

                    if (Buyed)
                    {
                        e.Item.Cells[12].Text = "<a href='Scheme.aspx?id=" + SchemeID.ToString() + "' target='_blank'>" + "<Font color=\'Red\'>已成功</font>" + "</a>";
                    }
                    else
                    {
                        int BuyedShare = Shove._Convert.StrToInt(e.Item.Cells[17].Text, 0);
                        int SchemeShare = Shove._Convert.StrToInt(e.Item.Cells[3].Text, 0);

                        if (BuyedShare >= SchemeShare)
                        {
                            e.Item.Cells[12].Text = "<a href='Scheme.aspx?id=" + SchemeID.ToString() + "' target='_blank'>" + "<Font color=\'Red\'>已满员</font>" + "</a>";
                        }
                        else
                        {
                            e.Item.Cells[12].Text = "<a href='Scheme.aspx?id=" + SchemeID.ToString() + "' target='_blank'>" + "未成功" + "</a>";
                        }
                    }
                }
            }
            else
            {
                if (money > 0)
                {
                    e.Item.Cells[12].Text = "<a href='Scheme.aspx?id=" + SchemeID.ToString() + "' target='_blank'>" + "<font color=\"red\">中奖啦!</font>" + "</a>";
                }
                else
                {
                    e.Item.Cells[12].Text = "<a href='Scheme.aspx?id=" + SchemeID.ToString() + "' target='_blank'>" + "未中奖" + "</a>";
                }
            }
        }
    }

    private void BindDataReward()
    {
        if (_User == null)
        {
            return;
        }

        string CacheKeyName = "Room_Welcome_Reward_" + _User.ID.ToString();

        DataTable dt = Shove._Web.Cache.GetCacheAsDataTable(CacheKeyName);

        if (dt == null)
        {
            dt = new DAL.Views.V_BuyDetailsWithQuashedAll().Open("UserID, SchemeID, LotteryNumber, IsuseName, SchemeWinMoney, LotteryName, WinMoneyNoWithTax,DetailMoney", "[UserID] = " + _User.ID.ToString() + " and EndTime < GetDate() and WinMoneyNoWithTax > 0", "[DateTime] desc, [ID]");

            if (dt == null)
            {
                PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试(732)", this.GetType().FullName);

                return;
            }

            Shove._Web.Cache.SetCache(CacheKeyName, dt, 120);
        }

        gReward.DataSource = dt;
        gReward.DataBind();

        this.lblRewardCount.Text = dt.Rows.Count.ToString();
        this.lblRewardMoney.Text = PF.GetSumByColumn(dt, 6, false, gReward.PageSize, 0).ToString("N");
    }

    protected void gReward_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.EditItem)
        {
            double money;
            double BuyMoney;
            BuyMoney = Shove._Convert.StrToDouble(e.Item.Cells[3].Text, 0);
            e.Item.Cells[3].Text = (BuyMoney == 0) ? "" : BuyMoney.ToString("N");

            money = Shove._Convert.StrToDouble(e.Item.Cells[4].Text, 0);
            e.Item.Cells[4].Text = (money == 0) ? "" : money.ToString("N");

            money = Shove._Convert.StrToDouble(e.Item.Cells[5].Text, 0);
            e.Item.Cells[5].Text = (money == 0) ? "" : money.ToString("N");


            if (money > 0)
            {
                e.Item.Cells[7].Text = "<font color=\"red\">中奖啦!</font>";
            }
            else
            {
                e.Item.Cells[7].Text = "未中奖";
            }

            e.Item.ToolTip = e.Item.Cells[2].Text;

            e.Item.Cells[2].Text = "<span class='red12_2'><a href='Scheme.aspx?id=" + e.Item.Cells[8].Text + "' target='_blank' class = 'red12_2'>投注内容</a></span>";

            double winMoneyNoWithTax = Shove._Convert.StrToDouble(e.Item.Cells[5].Text, 0);  //我的奖金
            double detailMoney = Shove._Convert.StrToDouble(e.Item.Cells[3].Text, 0);        //我投注的金额
            double winMoney = (winMoneyNoWithTax - detailMoney);

            double winRate = winMoney / detailMoney;

            if (winRate >= 1)
            {
                e.Item.Cells[6].Text = Math.Round(winRate, 2).ToString() + "倍";
            }
            else
            {
                e.Item.Cells[6].Text = (Math.Round(winRate, 2) * 100).ToString() + "%";
            }

            if (winMoney < 0)
            {
                e.Item.Cells[6].Text = "";
            }
        }
    }

    protected void gRewardPager_PageWillChange(object Sender, Shove.Web.UI.PageChangeEventArgs e)
    {
        BindDataInvestHistory();
        //lbMessage.Text = "<script>ExchangeDivMenu2('RoomWelcome',3,4);document.getElementById(\"RoomWelcome3\").className='RoomWelcome';</script>";
    }

    protected void gRewardPager_SortBefore(object source, System.Web.UI.WebControls.DataGridSortCommandEventArgs e)
    {
        BindDataInvestHistory();
        //lbMessage.Text = "<script>ExchangeDivMenu2('RoomWelcome',3,4);document.getElementById(\"RoomWelcome3\").className='RoomWelcome';</script>";
    }

    private void BindViewAccountData()
    {
        labUserType.Text = ((_User.UserType == 1) ? "普通用户" : ((_User.UserType == 3) ? "VIP" : "高级用户"));
        labBalance.Text = (_User.Balance + _User.Freeze).ToString("N") + "元";
        labFreeze.Text = _User.Freeze.ToString("N");
        labBalanceDo.Text = _User.Balance.ToString("N");
        labScoring.Text = _User.Scoring.ToString();
    }

    private string GetDateofWeek(string Date)
    {
        switch (Date)
        {
            case "Monday":
                return "1";
            case "Tuesday":
                return "2";
            case "Wednesday":
                return "3";
            case "Thursday":
                return "4";
            case "Friday":
                return "5";
            case "Saturday":
                return "6";
            case "Sunday":
                return "7";
            default:
                return "1";
        }
    }

    private void BindDataForLottery()
    {
        ddlLottery.Items.Clear();
        ddlLottery.Items.Add(new ListItem("全部彩种", "-1"));

        if (_Site.UseLotteryList == "")
        {
            PF.GoError(ErrorNumber.Unknow, "暂无玩法", "Room_InvestHistory");

            return;
        }

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

        for (int i = 0; i < dtLotteries.Rows.Count; i++)
        {
            //if (arrLotteries.Contains(Shove._Convert.StrToInt(dtLotteries.Rows[i]["ID"].ToString(), 0)))
            //{
            ddlLottery.Items.Add(new ListItem(dtLotteries.Rows[i]["Name"].ToString(), dtLotteries.Rows[i]["ID"].ToString()));
            //}
        }
    }

    protected void ddlLottery_SelectedIndexChanged(object sender, System.EventArgs e)
    {
        if (_User == null)
        {
            return;
        }

        string CacheKeyName = "Room_Welcome_InvestHistory_" + _User.ID.ToString();

        Shove._Web.Cache.ClearCache(CacheKeyName);

        BindDataInvestHistory();
    }
    protected void gInvestHistory_SortCommand(object source, DataGridSortCommandEventArgs e)
    {
        BindDataInvestHistory();
    }

    protected void gPager_PageWillChange(object Sender, Shove.Web.UI.PageChangeEventArgs e)
    {
        BindDataInvestHistory();
    }

    private void MenuByStatus()
    {
        bool isLogin = _User == null ? false : true;

        bool isAllowPromotion = false;
        DataTable dt = new DAL.Tables.T_Sites().Open("Opt_Promotion_Status_ON", "", "");
        if (dt != null && dt.Rows.Count > 0)
        {
            isAllowPromotion = bool.Parse(dt.Rows[0]["Opt_Promotion_Status_ON"].ToString());
        }
    }
    [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
    public string isLoginsas()
    {
        Users u = Users.GetCurrentUser(1);

        return u.ID.ToString();
    }
    [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
    public string GetBackUrl(string urls)
    {
        string url = "MyIcaile.aspx";
        url = Shove._Security.Encrypt.EncryptString(PF.GetCallCert(), url);
        return url;
    }
}
