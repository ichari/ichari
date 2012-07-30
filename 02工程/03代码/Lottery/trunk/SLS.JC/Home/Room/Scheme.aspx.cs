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
using System.Text;
using System.Web.UI.MobileControls;
using System.Collections.Generic;

public partial class Home_Room_Scheme : RoomPageBase
{
    protected long SchemeID = -1;
    private bool Opt_FullSchemeCanQuash = false;

    public string LotteryID = "5";
    public string LotteryName;
    public int PlayTypeID;
    private string dingZhi = "";
    int All_QuashStatus = 0;

    protected void Page_Load(object sender, System.EventArgs e)
    {
        AjaxPro.Utility.RegisterTypeForAjax(typeof(Home_Room_Scheme), this.Page);

        SchemeID = Shove._Convert.StrToLong(Shove._Web.Utility.GetRequest("id"), -1);

        if (SchemeID < 1)
        {
            PF.GoError(ErrorNumber.Unknow, "参数错误(-26)", this.GetType().FullName);

            return;
        }

        Opt_FullSchemeCanQuash = _Site.SiteOptions["Opt_FullSchemeCanQuash"].ToBoolean(false);

        if (!IsPostBack)
        {
            tbSchemeID.Text = SchemeID.ToString();
            if (_User != null)
            {
                labBalance.Text = _User.Balance.ToString("N");
            }

            BindData();
        }
    }

    #region Web 窗体设计器生成的代码

    override protected void OnInit(EventArgs e)
    {
        RequestLoginPage = this.Request.Url.AbsoluteUri;
        isRequestLogin = false;
        base.OnInit(e);
    }

    #endregion

    private void BindData()
    {
        DataTable dt = new DAL.Views.V_SchemeSchedulesWithQuashed().Open("", "[id] = " + Shove._Web.Utility.FilteSqlInfusion(tbSchemeID.Text), "");

        if ((dt == null) || (dt.Rows.Count < 1))
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试(-141)", this.GetType().BaseType.FullName);

            return;
        }

        DataRow dr = dt.Rows[0];

        long InitiateUserID = Shove._Convert.StrToLong(dr["InitiateUserID"].ToString(), 0);

        // 过滤竞彩足、篮球
        int _LotteryID = Shove._Convert.StrToInt(dr["LotteryID"].ToString(), 0);

        if (_LotteryID == 72 || _LotteryID == 73 || (_User !=  null && InitiateUserID == _User.ID))
        {
            this.btn_Single.Visible = false;
            this.btn_All.Visible = false;
        }

        hfID.Value = InitiateUserID.ToString();
        LotteryName = dr["LotteryName"].ToString();

        Label3.Text = LotteryName + "<font class='red14'>" + dr["IsuseName"].ToString() + "</font>期" + dr["PlayTypeName"].ToString() + "认购方案";
        labTitle.Text = dr["IsuseName"].ToString();
        labStartTime.Text = dr["StartTime"].ToString();
        tbIsuseID.Text = dr["IsuseID"].ToString();
        tbLotteryID.Text = dr["LotteryID"].ToString();
        LotteryID = tbLotteryID.Text;
        PlayTypeID = Shove._Convert.StrToInt(dr["PlayTypeID"].ToString(), 0);

        labEndTime.Text = dr["SystemEndTime"].ToString();

        labInitiateUser.Text = dr["InitiateName"].ToString() + "&nbsp;&nbsp;【<A class=li3 href='../Web/Score.aspx?id=" + dr["InitiateUserID"].ToString() + "&LotteryID=" + tbLotteryID.Text + "' target='_blank'>发起人历史战绩</A>】";

        All_QuashStatus = Shove._Convert.StrToShort(dr["QuashStatus"].ToString(), 0);

        bool Buyed = Shove._Convert.StrToBool(dr["Buyed"].ToString(), false);
        int Share = Shove._Convert.StrToInt(dr["Share"].ToString(), 0);
        int BuyedShare = Shove._Convert.StrToInt(dr["BuyedShare"].ToString(), 0);
        double Money = Shove._Convert.StrToDouble(dr["Money"].ToString(), 0);
        double AssureMoney = Shove._Convert.StrToDouble(dr["AssureMoney"].ToString(), 0);
        double WinMoney = Shove._Convert.StrToDouble(dr["WinMoney"].ToString(), 0);
        short SecrecyLevel = Shove._Convert.StrToShort(dr["SecrecyLevel"].ToString(), 0);
        bool IsuseOpenedWined = false;

        if (Share > 1)
        {
            lbSchemeBonus.Text = (Shove._Convert.StrToDouble(dr["SchemeBonusScale"].ToString(), 0.04) * 100).ToString() + "%";
        }

        labSchedule.Text = dr["Schedule"].ToString();

        DataTable dtIsuse = dtIsuse = new DAL.Views.V_Isuses().Open("IsOpened, WinLotteryNumber,Code", "[id] = " + dr["IsuseID"].ToString(), "");

        if (dtIsuse == null)
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试(-213)", this.GetType().FullName);

            return;
        }

        if (dtIsuse.Rows.Count < 1)
        {
            PF.GoError(ErrorNumber.Unknow, "系统错误(-220)", this.GetType().FullName);

            return;
        }

        IsuseOpenedWined = Shove._Convert.StrToBool(dt.Rows[0]["SchemeIsOpened"].ToString(), true);

        lbWinNumber.Text = dtIsuse.Rows[0]["WinLotteryNumber"].ToString();
        ImageLogo.ImageUrl = "images/lottery/" + dtIsuse.Rows[0]["Code"].ToString().ToLower() + ".jpg";

        //能撤消整个方案
        //Opt_FullSchemeCanQuash 是否允许撤消满员方案
        bool isSchemeCanQuash = _Site.SiteOptions["Opt_FullSchemeCanQuash"].ToBoolean(false);

        if (!isSchemeCanQuash)
        {
            btnQuashScheme.Visible = ((All_QuashStatus == 0) && (!Buyed) && (Share > BuyedShare) && _User != null && (InitiateUserID == _User.ID));
        }
        else
        {
            btnQuashScheme.Visible = ((All_QuashStatus == 0) && (!Buyed) && _User != null && (InitiateUserID == _User.ID));
        }

        short AtTopStatus = Shove._Convert.StrToShort(dr["AtTopStatus"].ToString(), 0);
        bool AtTopApplication = (AtTopStatus != 0);

        if (AtTopStatus == 0)
        {
            cbAtTopApplication.Visible = ((All_QuashStatus == 0) && (!Buyed) && (Share > BuyedShare) && _User != null && (InitiateUserID == _User.ID));
            cbAtTopApplication.Checked = AtTopApplication;
        }
        else
        {
            labAtTop.Visible = true;
        }

        bool Stop = false;

        //  投住内容

        labMultiple.Text = dr["Multiple"].ToString();

        //  SecrecyLevel 0 不保密 1 到截止 2 到开奖 3 永远
        if ((SecrecyLevel == SchemeSecrecyLevels.ToDeadline) && !Stop && ((_User == null) || ((_User != null) && (InitiateUserID != _User.ID) && (!_User.isOwnedViewSchemeCompetence()))))
        {
            labLotteryNumber.Text = "投注内容已经被保密，将在本期投注截止后公开。";
        }
        else if ((SecrecyLevel == SchemeSecrecyLevels.ToOpen) && !IsuseOpenedWined && ((_User == null) || ((_User != null) && (InitiateUserID != _User.ID) && (!_User.isOwnedViewSchemeCompetence()))))
        {
            labLotteryNumber.Text = "投注内容已经被保密，将在本期开奖后公开。";
        }
        else if ((SecrecyLevel == SchemeSecrecyLevels.Always) && ((_User == null) || ((_User != null) && (InitiateUserID != _User.ID) && (!_User.isOwnedViewSchemeCompetence()))))
        {
            labLotteryNumber.Text = "投注内容已经被保密。";
        }
        else
        {
            int MaxShowLotteryNumberRows = _Site.SiteOptions["Opt_MaxShowLotteryNumberRows"].ToShort(0);

            string t_str = "";

            try
            {
                t_str = dr["LotteryNumber"].ToString();
            }
            catch { }

            if (Shove._String.StringAt(t_str, '\n') < 1 && !string.IsNullOrEmpty(t_str))
            {
                StringBuilder sbTeam = new StringBuilder();

                if ((new SLS.Lottery.JCLQ().CheckPlayType(Shove._Convert.StrToInt(dr["PlayTypeID"].ToString(), -1))) || (new SLS.Lottery.JCZQ().CheckPlayType(Shove._Convert.StrToInt(dr["PlayTypeID"].ToString(), -1))))
                {
                    labLotteryNumber.Text = PF.GetScriptResTable(t_str);
                }
                else if (dr["LotteryID"].ToString().Equals("75") || dr["LotteryID"].ToString().Equals("74"))
                {
                    DataTable dtIsusesTeat = new DAL.Tables.T_IsuseForSFC().Open("", "IsuseID=" + dr["IsuseID"].ToString(), "No");

                    sbTeam.Append("<div class=\"tdbback\"><table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" class=\"tablelay\">");
                    sbTeam.Append("<th>场次</th>");
                    sbTeam.Append("<th>1</th>");
                    sbTeam.Append("<th>2</th>");
                    sbTeam.Append("<th>3</th>");
                    sbTeam.Append("<th>4</th>");
                    sbTeam.Append("<th>5</th>");
                    sbTeam.Append("<th>6</th>");
                    sbTeam.Append("<th>7</th>");
                    sbTeam.Append("<th>8</th>");
                    sbTeam.Append("<th>9</th>");
                    sbTeam.Append("<th>10</th>");
                    sbTeam.Append("<th>11</th>");
                    sbTeam.Append("<th>12</th>");
                    sbTeam.Append("<th>13</th>");
                    sbTeam.Append("<th>14</th>");
                    sbTeam.Append("<th>倍数</th>");
                    sbTeam.Append("<th>金额</th></tr>");
                    sbTeam.Append("<tr class=\"tr1\"><td>对阵</td>");

                    for (int i = 0; i < dtIsusesTeat.Rows.Count; i++)
                    {
                        sbTeam.Append("<td><div class=\"texts\">" + dtIsusesTeat.Rows[i]["HostTeam"].ToString() + " <span> VS </span> " + dtIsusesTeat.Rows[i]["QuestTeam"].ToString() + " </div></td>");
                    }

                    sbTeam.Append("<td>&nbsp;</td><td class=\"gray trline trline4 trline5\">单位（元）</td></tr>");
                    sbTeam.Append("<tr class=\"tr2\"><td class=\"gray trline trline3\">选号</td>");

                    for (int i = 0; i < 14; i++)
                    {
                        if (t_str.Substring(0, 1).Equals("("))
                        {
                            sbTeam.Append("<td>" + t_str.Substring(1, t_str.IndexOf(")") - 1) + "</td>");

                            t_str = t_str.Substring(t_str.IndexOf(")") + 1);
                        }
                        else
                        {
                            sbTeam.Append("<td>" + t_str.Substring(0, 1) + "</td>");

                            t_str = t_str.Substring(1);
                        }
                    }

                    sbTeam.Append("<td>" + dr["Multiple"].ToString() + "</td>");
                    sbTeam.Append("<td class=\"red\">￥" + Money.ToString() + "</td></tr></table></div>");

                    labLotteryNumber.Text = sbTeam.ToString();
                }
                else if (dr["LotteryID"].ToString().Equals("2"))
                {
                    DataTable dtIsusesTeat = new DAL.Tables.T_IsuseForJQC().Open("", "IsuseID=" + dr["IsuseID"].ToString(), "");

                    sbTeam.Append("<div class=\"tdbback\"><table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" class=\"tablelay\">");
                    sbTeam.Append("<th>场次</th>");
                    sbTeam.Append("<th>1</th>");
                    sbTeam.Append("<th>2</th>");
                    sbTeam.Append("<th>3</th>");
                    sbTeam.Append("<th>4</th>");
                    sbTeam.Append("<th>5</th>");
                    sbTeam.Append("<th>6</th>");
                    sbTeam.Append("<th>7</th>");
                    sbTeam.Append("<th>8</th>");
                    sbTeam.Append("<th>倍数</th>");
                    sbTeam.Append("<th>金额</th></tr>");
                    sbTeam.Append("<tr class=\"tr1\"><td>对阵</td>");

                    for (int i = 0; i < dtIsusesTeat.Rows.Count; i++)
                    {
                        sbTeam.Append("<td><div class=\"texts\">" + dtIsusesTeat.Rows[i]["Team"].ToString() + " <div></td>");
                    }

                    sbTeam.Append("<td>&nbsp;</td><td class=\"gray trline trline4 trline5\">单位（元）</td></tr>");
                    sbTeam.Append("<tr class=\"tr2\"><td class=\"gray trline trline3\">选号</td>");

                    for (int i = 0; i < 8; i++)
                    {
                        if (t_str.Substring(0, 1).Equals("("))
                        {
                            sbTeam.Append("<td>" + t_str.Substring(1, t_str.IndexOf(")") - 1) + "</td>");

                            t_str = t_str.Substring(0, t_str.IndexOf(")") + 1);
                        }
                        else
                        {
                            sbTeam.Append("<td>" + t_str.Substring(0, 1) + "</td>");

                            t_str = t_str.Substring(1);
                        }
                    }

                    sbTeam.Append("<td>" + dr["Multiple"].ToString() + "</td>");
                    sbTeam.Append("<td class=\"red\">￥" + Money.ToString() + "</td></tr></table></div>");

                    labLotteryNumber.Text = sbTeam.ToString();
                }
                else if (dr["LotteryID"].ToString().Equals("15"))
                {
                    DataTable dtIsusesTeat = new DAL.Tables.T_IsuseForLCBQC().Open("", "IsuseID=" + dr["IsuseID"].ToString(), "");

                    sbTeam.Append("<div class=\"tdbback\"><table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" class=\"tablelay\">");
                    sbTeam.Append("<th>场次</th>");
                    sbTeam.Append("<th>1</th>");
                    sbTeam.Append("<th>2</th>");
                    sbTeam.Append("<th>3</th>");
                    sbTeam.Append("<th>4</th>");
                    sbTeam.Append("<th>5</th>");
                    sbTeam.Append("<th>6</th>");
                    sbTeam.Append("<th>7</th>");
                    sbTeam.Append("<th>8</th>");
                    sbTeam.Append("<th>9</th>");
                    sbTeam.Append("<th>10</th>");
                    sbTeam.Append("<th>11</th>");
                    sbTeam.Append("<th>12</th>");
                    sbTeam.Append("<th>倍数</th>");
                    sbTeam.Append("<th>金额</th></tr>");
                    sbTeam.Append("<tr class=\"tr1\"><td>对阵</td>");

                    for (int i = 0; i < dtIsusesTeat.Rows.Count; i++)
                    {
                        sbTeam.Append("<td><div class=\"texts\">" + dtIsusesTeat.Rows[i]["HostTeam"].ToString() + " <span> VS </span> " + dtIsusesTeat.Rows[i]["QuestTeam"].ToString() + " </div> <span class=\"red\">半</span></td>");
                        sbTeam.Append("<td><div class=\"texts\">" + dtIsusesTeat.Rows[i]["HostTeam"].ToString() + " <span> VS </span> " + dtIsusesTeat.Rows[i]["QuestTeam"].ToString() + " </div> <span class=\"red\">全</span> </td>");
                    }

                    sbTeam.Append("<td>&nbsp;</td><td class=\"gray trline trline4 trline5\">单位（元）</td></tr>");
                    sbTeam.Append("<tr class=\"tr2\"><td class=\"gray trline trline3\">选号</td>");

                    for (int i = 0; i < 12; i++)
                    {
                        if (t_str.Substring(0, 1).Equals("("))
                        {
                            sbTeam.Append("<td>" + t_str.Substring(1, t_str.IndexOf(")") - 1) + "</td>");

                            t_str = t_str.Substring(0, t_str.IndexOf(")") + 1);
                        }
                        else
                        {
                            sbTeam.Append("<td>" + t_str.Substring(0, 1) + "</td>");

                            t_str = t_str.Substring(1);
                        }
                    }

                    sbTeam.Append("<td>" + dr["Multiple"].ToString() + "</td>");
                    sbTeam.Append("<td class=\"red\">￥" + Money.ToString() + "</td></tr></table></div>");

                    labLotteryNumber.Text = sbTeam.ToString();
                }
                else
                {
                    labLotteryNumber.Text = Shove._Convert.ToHtmlCode(t_str) + "&nbsp;";

                    if (IsuseOpenedWined)
                    {
                        NumberDuiBi(labLotteryNumber.Text, lbWinNumber.Text, PlayTypeID);
                    }
                }
            }
            else if (Shove._String.StringAt(t_str, '\n') < MaxShowLotteryNumberRows && !dr["LotteryID"].ToString().Equals("74") && !dr["LotteryID"].ToString().Equals("75") && !dr["LotteryID"].ToString().Equals("2") && !dr["LotteryID"].ToString().Equals("15") && !dr["LotteryID"].ToString().Equals("72") && !dr["LotteryID"].ToString().Equals("73"))
            {
                labLotteryNumber.Text = Shove._Convert.ToHtmlCode(t_str) + "&nbsp;";

                if (IsuseOpenedWined)
                {
                    NumberDuiBi(labLotteryNumber.Text, lbWinNumber.Text, PlayTypeID);
                }
            }
            else if(!string.IsNullOrEmpty(t_str))
            {
                linkDownloadScheme.Visible = true;
                linkDownloadScheme.NavigateUrl = "../Web/DownloadSchemeFile.aspx?id=" + tbSchemeID.Text;
            }

            if (dr["LotteryID"].ToString().Equals("72") || dr["LotteryID"].ToString().Equals("73"))
            {
                labEndTime.Text = GetScriptResTable(t_str);
            }
        }

        System.DateTime EndTime = Shove._Convert.StrToDateTime(labEndTime.Text, DateTime.Now.ToString());

        if (DateTime.Now >= EndTime)
        {
            Stop = true;
            tbStop.Text = Stop.ToString();
        }

        if (All_QuashStatus > 0)
        {
            if (All_QuashStatus == 2)
            {
                labState.Text = "已撤单(系统撤单)";
            }
            else
            {
                labState.Text = "已撤单";
            }
        }
        else
        {
            if (Buyed)
            {
                labState.Text = "<FONT color='red'>已出票</font>";
            }
            else
            {
                if (Stop)
                {
                    labState.Text = "已截止";
                }
                else
                {
                    if (Share <= BuyedShare)
                    {
                        labState.Text = "<FONT color='red'>已满员</font>";
                    }
                    else
                    {
                        labState.Text = "<font color='red'>抢购中...</font>";
                    }
                }
            }
        }

        // 填充
        labSchemeNumber.Text = dr["SchemeNumber"].ToString();
        labSchemeMoney.Text = Shove._Convert.StrToDouble(dr["Money"].ToString(), 0).ToString("N");
        labSchemeTitle.Text = dr["Title"].ToString() + "&nbsp;";
        labSchemeDescription.Text = dr["Description"].ToString() + "&nbsp;";

        labSchemeADUrl.Text = Shove._Web.Utility.GetUrl() + "/Home/Room/Scheme.aspx?id=" + tbSchemeID.Text;
        
        if (string.IsNullOrEmpty(dr["LotteryNumber"].ToString()) && (LotteryID == "1" || LotteryID == "2" || LotteryID == "15" || LotteryID == "74" || LotteryID == "75") && All_QuashStatus == 0 && !Stop && !Buyed)
        {
            labLotteryNumber.Text = "未上传";

            if (_User != null && dr["InitiateUserID"].ToString() == _User.ID.ToString())
            {
                lbUploadScheme.Visible = true;

                DataTable dtPrepareBet = new DAL.Tables.T_PrepareBet().Open("", "SchemeID=" + SchemeID.ToString(), "");

                if (dtPrepareBet == null)
                {
                    PF.GoError(ErrorNumber.DataReadWrite, "数据访问错误(-364)", this.GetType().FullName);

                    return;
                }

                if (dtPrepareBet.Rows.Count < 1)
                {
                    hidMaxMoney.Value = Money.ToString();
                }
                else
                {
                    hidMaxMoney.Value = dtPrepareBet.Rows[0]["MaxMoney"].ToString();
                }
            }
        }

        labAssureMoney.Text = (AssureMoney > 0) ? string.Format("<FONT color='red'>{0}</font> 元", AssureMoney.ToString("N")) : "未保底";

        if (All_QuashStatus > 0)
        {
            if (All_QuashStatus == 2)
            {
                labWin.Text = "已撤单(系统撤单)";
            }
            else
            {
                labWin.Text = "已撤单";
            }
        }
        else
        {
            if (Stop)
            {
                labWin.Text = string.Format("<FONT color='red'>{0}</font> 元", WinMoney.ToString("N"));
                string WinDescription = dr["WinDescription"].ToString();

                if (WinDescription != "")
                {
                    labWin.Text += "<br />" + WinDescription;
                }
                else
                {
                    if (IsuseOpenedWined)
                    {
                        labWin.Text += "  未中奖";
                    }
                    else
                    {
                        labWin.Text += "  <font color='red'>【注】</font>中奖结果在开奖后需要一段时间才能显示。";
                    }
                }
            }
            else
            {
                labWin.Text = "尚未截止";
            }
        }

        if (IsuseOpenedWined)
        {
            if (LotteryID == "1" || LotteryID == "2" || LotteryID == "15")
            {
                labWin.Text += "(命中<font color='red'>" + CompareLotteryNumberToWinNumber(dr["LotteryNumber"].ToString(), dr["WinLotteryNumber"].ToString()).ToString() + "</font>场)";
            }
        }

        if (Stop)
        {
            labCannotBuyTip.Text = "方案已截止，不能认购";
            labCannotBuyTip.Visible = true;
            pBuy.Visible = false;
            btnOK.Enabled = false;
        }
        else
        {
            if (All_QuashStatus > 0)
            {
                labCannotBuyTip.Text = "方案已撤单，不能认购";
                labCannotBuyTip.Visible = true;
                pBuy.Visible = false;
                btnOK.Enabled = false;
            }
            else
            {
                if (BuyedShare >= Share)
                {
                    labCannotBuyTip.Text = "方案已满员，不能认购";
                    labCannotBuyTip.Visible = true;
                    pBuy.Visible = false;
                    btnOK.Enabled = false;
                }
                else
                {
                    labCannotBuyTip.Visible = false;
                    pBuy.Visible = true;
                    btnOK.Enabled = true;
                }
            }
        }

        labShare.Text = Share.ToString();
        labBuyedShare.Text = (Share - BuyedShare).ToString();
        labShareMoney.Text = (Money / Share).ToString("N");

        // 绑定参与用户列表
        BindDataForUserList();

        if (_User != null)
        {
            DataTable dtMyBuy = new DAL.Views.V_BuyDetailsWithQuashedAll().Open("[id],[DateTime],[Money],Share,SchemeShare,BuyedShare,QuashStatus,Buyed,IsuseID,Code,Schedule,DetailMoney,isWhenInitiate, WinMoneyNoWIthTax", "SiteID = " + _Site.ID.ToString() + " and SchemeID = " + Shove._Web.Utility.FilteSqlInfusion(tbSchemeID.Text) + " and [UserID] = " + _User.ID.ToString(), "[id]");

            if (dtMyBuy == null)
            {
                PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试(-518)", this.GetType().FullName);

                return;
            }

            if (dtMyBuy.Rows.Count == 0)
            {
                labMyBuy.Text = "此方案还没有我的认购记录。";
                labMyBuy.Visible = true;

                g.Visible = false;
            }
            else
            {
                labMyBuy.Visible = false;

                g.Visible = true;

                PF.DataGridBindData(g, dtMyBuy);

                if (IsuseOpenedWined)
                {
                    double DetailMoney = 0;

                    for (int i = 0; i < dtMyBuy.Rows.Count; i++)
                    {
                        DetailMoney += double.Parse(dtMyBuy.Rows[i]["WinMoneyNoWIthTax"].ToString());
                    }

                    lbReward.Text = DetailMoney.ToString("N");
                }

            }

            if (_User.UserType < 2)
            {
                btnOK.Enabled = false;
                btnQuashScheme.Enabled = false;
            }
        }

    }

    private void NumberDuiBi(string LotteryNumber, string WinLotteryNumber, int PlayTypeID)
    {
        if (LotteryID == "62")
        {
            string Number = LotteryNumber.Replace("&nbsp;", " ").Replace("<br/>", " ").Trim();
            string[] lotteryNumber = null;
            string[] winLotteryNumber = WinLotteryNumber.Split(' ');

            if (PlayTypeID == 6201 || PlayTypeID == 6209)
            {
                if (PlayTypeID == 6201)
                {
                    Number = Number.Replace(" ", "|");
                    lotteryNumber = Number.Split('|'); 

                    for (int i = 0; i < lotteryNumber.Length; i++)
                    {
                        if (lotteryNumber[i] == winLotteryNumber[0])
                        {
                            LotteryNumber = LotteryNumber.Replace(lotteryNumber[i], "<font color='Red'>" + lotteryNumber[i] + "</font>");
                        }
                    }
                }
                else if (PlayTypeID == 6209)
                {
                    Number = Number.Replace(" ", "|");
                    lotteryNumber = Number.Split('|'); 

                    LotteryNumber = "";
                    for (int i = 0; i < lotteryNumber.Length; i++)
                    {
                        if (i % 2 == 0 && lotteryNumber[i] == winLotteryNumber[0])
                        {
                            LotteryNumber += "<font color='Red'>" + lotteryNumber[i] + "</font>|";
                        }
                        else if (i % 2 == 1 && lotteryNumber[i] == winLotteryNumber[1])
                        {
                            LotteryNumber += "<font color='Red'>" + lotteryNumber[i] + "</font>|";
                        }
                        else
                        {
                            LotteryNumber += lotteryNumber[i] + "|";
                        }

                        if (i % 2 == 1)
                        {
                            LotteryNumber += "<br>";
                        }
                    }
                }

                labLotteryNumber.Text = LotteryNumber;
            }
            else
            {
                if (PlayTypeID == 6211 || PlayTypeID == 6212)
                {
                    string[] Numbers = LotteryNumber.Replace("<br/>", " ").Split(' ');

                    string[] EachNumber = null;

                    LotteryNumber = "";

                    if (PlayTypeID == 6212)
                    {
                        for (int i = 0; i < Numbers.Length; i++)
                        {
                            EachNumber = Numbers[i].Replace("&nbsp;", " ").Split(' ');
                            for (int j = 0; j < EachNumber.Length; j++)
                            {
                                if (EachNumber[j] == winLotteryNumber[0] || EachNumber[j] == winLotteryNumber[1] || EachNumber[j] == winLotteryNumber[2])
                                {
                                    LotteryNumber += "<font color='Red'>" + EachNumber[j] + "</font>&nbsp;";
                                }
                                else
                                {
                                    LotteryNumber += EachNumber[j] + "&nbsp;";
                                }

                            }
                            LotteryNumber += "<br>";
                        }
                    }
                    else
                    {
                        for (int i = 0; i < Numbers.Length; i++)
                        {
                            EachNumber = Numbers[i].Replace("&nbsp;", " ").Split(' ');
                            for (int j = 0; j < EachNumber.Length; j++)
                            {
                                if (EachNumber[j] == winLotteryNumber[0] || EachNumber[j] == winLotteryNumber[1])
                                {
                                    LotteryNumber += "<font color='Red'>" + EachNumber[j] + "</font>&nbsp;";
                                }
                                else
                                {
                                    LotteryNumber += EachNumber[j] + "&nbsp;";
                                }

                            }
                            LotteryNumber += "<br>";
                        }
                    }

                    labLotteryNumber.Text = LotteryNumber;
                }
                else
                {
                    lotteryNumber = Number.Replace("|", " ").Split(' ');

                    foreach (string s in lotteryNumber)
                    {
                        if (WinLotteryNumber.IndexOf(s) > -1)
                        {
                            LotteryNumber = LotteryNumber.Replace(s, "<font color='Red'>" + s + "</font>");
                        }
                    }

                    labLotteryNumber.Text = LotteryNumber;
                }
            }
        }
    }

    private void BindDataForUserList()
    {
        string sql = "select * from V_BuyDetailsWithQuashed with (nolock) where SiteID = @SiteID and SchemeID = @SchemeID and QuashStatus = 0 order by [id]";
        if (All_QuashStatus == 2)   //当为系统撤单时，查询合买方案的所有参与用户
        {
            sql = "SELECT dbo.T_BuyDetails.DateTime, dbo.T_BuyDetails.Share,dbo.T_BuyDetails.Share * (dbo.T_Schemes.Money / dbo.T_Schemes.Share) AS DetailMoney, dbo.T_Users.Name" +
                  " FROM dbo.T_BuyDetails INNER JOIN " +
                  "  dbo.T_Users ON dbo.T_BuyDetails.UserID = dbo.T_Users.ID INNER JOIN" +
                  " dbo.T_Schemes ON dbo.T_BuyDetails.SchemeID = dbo.T_Schemes.ID AND dbo.T_BuyDetails.QuashStatus <> 1" +
                  " where T_Schemes.SiteID = @SiteID and SchemeID = @SchemeID order by T_Schemes.[id]";
        }
        DataTable dtUserList = MSSQL.Select(sql,
                            new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, _Site.ID),
                            new MSSQL.Parameter("SchemeID", SqlDbType.VarChar, 0, ParameterDirection.Input, Shove._Web.Utility.FilteSqlInfusion(tbSchemeID.Text)));

        if (dtUserList == null)
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试(-503)", this.GetType().FullName);

            return;
        }

        labUserList.Text = String.Format("总共有 <font color='red'>{0}</font> 个用户参与。", dtUserList.Rows.Count) +
            ((dtUserList.Rows.Count > 0) ? "&nbsp;&nbsp;【<A class=li3 href='javascript:onUserListClick();'>打开/隐藏明细</A>】" : "");

        gUserList.DataSource = dtUserList.DefaultView;
        gUserList.DataBind();
    }

    protected void btnOK_Click(object sender, System.EventArgs e)
    {
        DateTime EndTime = DateTime.Parse(labEndTime.Text);

        if (DateTime.Now > EndTime)
        {
            Shove._Web.JavaScript.Alert(this.Page, "投注时间已经截止，不能认购。");

            return;
        }

        //既不是发起人，也不在招股对象之内
        if (!_User.isCanViewSchemeContent(SchemeID))
        {
            Shove._Web.JavaScript.Alert(this.Page, "对不起，您不在此方案的招股对象之内。");

            return;
        }

        double ShareMoney = 0;
        int Share = 0;

        try
        {
            ShareMoney = double.Parse(labShareMoney.Text);
            Share = int.Parse(tbShare.Text);
        }
        catch
        {
            Shove._Web.JavaScript.Alert(this.Page, "输入有错误，请仔细检查。");

            return;
        }

        if ((ShareMoney <= 0) || (Share < 1) || (Share > Shove._Convert.StrToInt(labShare.Text, 0)))
        {
            Shove._Web.JavaScript.Alert(this.Page, "输入有错误，请仔细检查。");

            return;
        }

        if ((ShareMoney * Share) > _User.Balance)
        {
            Shove._Web.JavaScript.Alert(this.Page, "您的账户余额不足，请先充值，谢谢。");

            return;
        }

        string ReturnDescription = "";

        if (_User.JoinScheme(long.Parse(tbSchemeID.Text), Share, ref ReturnDescription) < 0 || ReturnDescription != "")
        {
            if (ReturnDescription.IndexOf("方案剩余份数已不足") > -1)
            {
                try
                {
                    string strShare = ReturnDescription.Split(new string[] { ",剩余 " }, StringSplitOptions.None)[1].Split(' ')[0].ToString();

                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "", "alert('" + ReturnDescription + "');document.getElementById('tbShare').value='" + strShare + "';document.getElementById('labShare').innerText='" + strShare + "';", true);
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "", "alert('方案剩余份数已不足 " + Share.ToString() + " 份');document.getElementById('tbShare').value='" + (Share - 1).ToString() + "';", true);
                }
            }
            else
            {
                Shove._Web.JavaScript.Alert(this.Page, ReturnDescription);
            }

            return;
        }
        else
        {
            tbShare.Text = "";

            Shove._Web.Cache.ClearCache("Home_Room_CoBuy_BindDataForType" + tbIsuseID.Text);
            Shove._Web.Cache.ClearCache("Home_Room_SchemeAll_BindData" + tbIsuseID.Text);
            Shove._Web.Cache.ClearCache("Home_Room_JoinAllBuy_BindData");

            Response.Write("<script>try{window.opener.parent.ReloadSchedule();} catch(ex) {};window.location.href='UserBuySuccess.aspx?LotteryID=" + LotteryID.ToString() + "&Type=3&Money=" + (ShareMoney * Share).ToString() + "&SchemeID=" + tbSchemeID.Text + "'</script>");
        }
    }

    protected void btnQuashScheme_Click(object sender, System.EventArgs e)
    {
        if (_User.UserType == 1)
        {
            Shove._Web.JavaScript.Alert(this.Page, "对不起，您还不是高级会员，请先免费升级为高级会员。谢谢！");

            return;
        }

        DateTime EndTime = DateTime.Parse(labEndTime.Text);

        if (DateTime.Now > EndTime)
        {
            Shove._Web.JavaScript.Alert(this.Page, "投注时间已经截止，不能撤消方案。");

            return;
        }

        double Opt_Betting_ForbidenCancel_Percent = Shove._Convert.StrToDouble(new SystemOptions()["Betting_ForbidenCancel_Percent"].Value.ToString(), 0);

        if (Opt_Betting_ForbidenCancel_Percent > 0 && Shove._Convert.StrToDouble(labSchedule.Text, -1) >= Opt_Betting_ForbidenCancel_Percent)
        {
            Shove._Web.JavaScript.Alert(this.Page, "对不起，由于本方案进度已经达到 " + Opt_Betting_ForbidenCancel_Percent.ToString("N") + "%，即将满员，不允许撤单。");

            return;
        }

        string ReturnDescription = "";

        if (_User.QuashScheme(int.Parse(tbSchemeID.Text), false, ref ReturnDescription) < 0)
        {
            PF.GoError(ErrorNumber.Unknow, ReturnDescription, this.GetType().FullName);

            return;
        }
        Shove._Web.Cache.ClearCache("Home_Room_CoBuy_BindDataForType" + tbIsuseID.Text);
        Shove._Web.Cache.ClearCache("Home_Room_SchemeAll_BindData" + tbIsuseID.Text);

        BindData();

        return;
    }

    protected void cbAtTopApplication_CheckedChanged(object sender, System.EventArgs e)
    {
        int AtTopApplication = cbAtTopApplication.Checked ? 1 : 0;

        if (MSSQL.ExecuteNonQuery("update T_Schemes set AtTopStatus = " + AtTopApplication.ToString() + " where SiteID = " + _Site.ID.ToString() + " and [id] = " + Shove._Web.Utility.FilteSqlInfusion(tbSchemeID.Text)) < 0)
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试(-678)", this.GetType().FullName);

            return;
        }

        BindData();
    }

    protected void g_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
    {
        if (e.CommandName == "QuashBuy")
        {
            double Opt_Betting_ForbidenCancel_Percent = Shove._Convert.StrToDouble(new SystemOptions()["Betting_ForbidenCancel_Percent"].Value.ToString(), 0);

            if (Opt_Betting_ForbidenCancel_Percent > 0 && Shove._Convert.StrToDouble(labSchedule.Text, -1) >= Opt_Betting_ForbidenCancel_Percent)
            {
                Shove._Web.JavaScript.Alert(this.Page, "对不起，由于本方案进度已经达到 " + Opt_Betting_ForbidenCancel_Percent.ToString("N") + "%，即将满员，不允许撤单。");

                return;
            }

            long BuyDetailID = Shove._Convert.StrToLong(e.Item.Cells[12].Text, 0);

            if (BuyDetailID < 1)
            {
                PF.GoError(ErrorNumber.Unknow, "参数错误(-694)", this.GetType().FullName);

                return;
            }

            string ReturnDescription = "";

            if (_User.Quash(BuyDetailID, ref ReturnDescription) < 0)
            {
                PF.GoError(ErrorNumber.Unknow, ReturnDescription + "(-703)", this.GetType().FullName);

                return;
            }

            BindData();

            Shove._Web.Cache.ClearCache("Home_Room_CoBuy_BindDataForType" + tbIsuseID.Text);
            Shove._Web.Cache.ClearCache("Home_Room_SchemeAll_BindData" + tbIsuseID.Text);

            return;
        }
    }

    protected void g_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.EditItem)
        {
            e.Item.Cells[0].Text = "<font color='red'>" + e.Item.Cells[0].Text + "</font> 份";

            double money;
            money = Shove._Convert.StrToDouble(e.Item.Cells[1].Text, 0);
            e.Item.Cells[1].Text = "<font color='red'>" + ((money == 0) ? "0.00" : money.ToString("N")) + "</font> 元";

            e.Item.Cells[2].Text = e.Item.Cells[8].Text + e.Item.Cells[9].Text + e.Item.Cells[12].Text;

            short QuashStatus = Shove._Convert.StrToShort(e.Item.Cells[6].Text, 0);
            bool Buyed = Shove._Convert.StrToBool(e.Item.Cells[7].Text, false);
            bool Stop = Shove._Convert.StrToBool(tbStop.Text, false);
            double Schedule = Shove._Convert.StrToDouble(e.Item.Cells[11].Text, 0);
            int SchemeShare = Shove._Convert.StrToInt(e.Item.Cells[14].Text, 0);
            int BuyedShare = Shove._Convert.StrToInt(e.Item.Cells[10].Text, 0);

            Button btnQuashBuy = ((Button)e.Item.Cells[5].FindControl("btnQuashBuy"));

            if (QuashStatus > 0)
            {
                btnQuashBuy.Enabled = false;
                e.Item.Cells[4].Text = "已撤单";
            }
            else
            {
                if (Stop)
                {
                    if (Schedule >= 100)
                    {
                        e.Item.Cells[4].Text = "<Font color=\'Red\'>已成功</font>";
                    }
                    else
                    {
                        e.Item.Cells[4].Text = "未成功";
                    }

                    btnQuashBuy.Enabled = false;
                }
                else
                {
                    if (Buyed)
                    {
                        e.Item.Cells[4].Text = "<Font color=\'Red\'>已出票</font>";

                        btnQuashBuy.Enabled = false;
                    }
                    else
                    {
                        e.Item.Cells[4].Text = "<Font color=\'Red\'>进行中</font>";
                    }

                    bool isWhenInitiate = Shove._Convert.StrToBool(e.Item.Cells[13].Text, true);
                    bool isFull = (SchemeShare <= BuyedShare);

                    if (isWhenInitiate)
                    {
                        btnQuashBuy.Enabled = false;
                    }
                    else
                    {
                        if (isFull)
                        {
                            btnQuashBuy.Enabled = (_User != null && Opt_FullSchemeCanQuash && (_User.UserType > 1));
                        }
                        else
                        {
                            btnQuashBuy.Enabled = (_User != null && _User.UserType > 1);
                        }
                    }
                }
            }
        }
    }

    protected void gUserList_PageIndexChanged(object sender, EventArgs e)
    {

        BindDataForUserList();
    }

    [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
    public string GetUserBalance()
    {
        Users u = Users.GetCurrentUser(1);

        return u.Balance.ToString("N");
    }

    [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
    public string AnalyseScheme(string Content, string LotteryID, int PlayTypeID)
    {
        string Result = new SLS.Lottery()[Shove._Convert.StrToInt(LotteryID, 5)].AnalyseScheme(Content, PlayTypeID);

        return Result.Trim();
    }

    [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
    public string UpdateLotteryNumber(string id, string content, int Money)
    {
        DataTable dt = new DAL.Tables.T_Schemes().Open("","id=" + id,"");

        if (dt == null)
        {
            return "";
        }

        if (dt.Rows.Count != 1)
        {
            return "";
        }

        string Share = dt.Rows[0]["Share"].ToString();
        string Schedule = dt.Rows[0]["Schedule"].ToString();

        DAL.Tables.T_Schemes s = new DAL.Tables.T_Schemes();

        s.LotteryNumber.Value = content;
        s.Money.Value = Money;
        s.Share.Value = Money;

        if (s.Update("ID=" + id) < 0)
        {
            return "修改方案号码失败！";
        }

        return "上传成功！";
    }

    protected void btn_Single_Click(object sender, EventArgs e)
    {
        ResponseTailor(true);
    }

    private void ResponseTailor(bool b)
    {
        long userid = Shove._Convert.StrToLong(hfID.Value, -1);
        int lotteryid = Shove._Convert.StrToInt(tbLotteryID.Text, -1);
        int temp = -1;
        if (b)
        {
            temp = lotteryid;
        }
        string headMethod = "followScheme(" + temp + ");$Id(\"iframeFollowScheme\").src=\"../Home/Room/FollowFriendSchemeAdd.aspx?LotteryID=" + temp + "&DzLotteryID=" + temp;

        if (userid < 1)
        {
            PF.GoError(ErrorNumber.Unknow, "参数错误", this.GetType().FullName);

            return;
        }
        if (!new SLS.Lottery().ValidID(lotteryid))
        {
            PF.GoError(ErrorNumber.Unknow, "参数错误！(彩种)", this.GetType().FullName);

            return;
        }
        Users tu = new Users(_Site.ID);
        tu.ID = userid;
        dingZhi = "&FollowUserID=" + userid + "&FollowUserName=" + HttpUtility.UrlEncode(tu.Name) + "\"";
        string ReturnDescription = "";
        if (tu.GetUserInformationByID(ref ReturnDescription) != 0)
        {

            return;
        }
        dingZhi = Shove._Security.Encrypt.EncryptString(PF.GetCallCert(), headMethod + dingZhi);

        if ((lotteryid != 72) || (lotteryid != 73))
        {
            Response.Redirect("/Lottery/" + DataCache.LotterieBuyPage[lotteryid] + "?DZ=" + dingZhi + "");
        }
    }

    protected void btn_All_Click(object sender, ImageClickEventArgs e)
    {
        ResponseTailor(false);
    }

    private int CompareLotteryNumberToWinNumber(string LotteryNumber, string WinNumber)
    {
        string[] Number = LotteryNumber.Trim().Split('\n');

        string number;
        bool isEnd;
        int row;
        int Max = 0;
        int Num;
        foreach (string s in Number)
        {
            isEnd = false;
            row = -1;
            Num = 0;

            for (int i = 0; i < s.Trim().Length; i++)
            {
                if (!isEnd)
                {
                    row++;
                }

                number = s.Trim().Substring(i, 1);

                if (number == "(")
                {
                    isEnd = true;
                    continue;
                }
                else
                {
                    if (number == ")")
                    {
                        isEnd = false;
                        continue;
                    }
                    else
                    {
                        if (number == WinNumber.Substring(row, 1))
                        {
                            Num++;
                        }
                    }
                }
            }

            if (Num > Max)
            {
                Max = Num;
            }
        }

        return Max;
    }

    public string GetScriptResTable(string val)
    {
        try
        {
            val = val.Trim();

            int Istart, Ilen;

            GetStrScope(val, "[", "]", out Istart, out Ilen);

            string matchlist = val.Substring(Istart + 1, Ilen - 1);

            string type = val.Split(';')[0];

            if (type.Substring(0, 2) != "72" && type.Substring(0, 2) != "73")
            {
                return val;
            }

            string Matchids = "";
            string MatchListDan = "";

            if (val.Split(';').Length == 4)
            {
                MatchListDan = matchlist.Split(']')[0];

                foreach (string match in MatchListDan.Split('|'))
                {
                    Matchids += match.Split('(')[0] + ",";
                }
            }

            foreach (string match in matchlist.Split('|'))
            {
                Matchids += match.Split('(')[0] + ",";
            }

            if (Matchids.EndsWith(","))
            {
                Matchids = Matchids.Substring(0, Matchids.Length - 1);
            }

            DataTable table = null;

            if (type.Substring(0, 2) == "72")
            {
                table = new DAL.Tables.T_Match().Open("StopSellingTime", "id in (" + Matchids + ")", " StopSellingTime ");
            }
            else
            {
                table = new DAL.Tables.T_MatchBasket().Open("StopSellingTime", "id in (" + Matchids + ")", " StopSellingTime");
            }

            if (table.Rows.Count < 1)
            {
                return "";
            }

            DataTable dtPlayType = new DAL.Tables.T_PlayTypes().Open("SystemEndAheadMinute", "ID=" + type.Substring(0, 4), "");

            if (dtPlayType == null)
            {
                return "";
            }

            if (dtPlayType.Rows.Count < 1)
            {
                return "";
            }

            return Shove._Convert.StrToDateTime(table.Rows[0]["StopSellingTime"].ToString(), DateTime.Now.AddDays(-1).ToString()).AddMinutes(Shove._Convert.StrToInt(dtPlayType.Rows[0]["SystemEndAheadMinute"].ToString(), 0) * -1).ToString();
        }
        catch {
            return "";
        }
    }

    public void GetStrScope(string str, string strStart, string strEnd, out int IStart, out int ILen)
    {
        IStart = str.IndexOf(strStart);
        if (IStart != -1)
            ILen = str.LastIndexOf(strEnd) - IStart;
        else
        {
            IStart = 0;
            ILen = 0;
        }
    }
}
