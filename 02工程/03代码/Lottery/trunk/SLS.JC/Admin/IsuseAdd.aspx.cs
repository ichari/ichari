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
using System.Text;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;

using Shove.Database;

public partial class Admin_IsuseAdd : AdminPageBase
{
    private DataTable dt_FootballLeagueTypes;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            int LotteryID = Shove._Convert.StrToInt(Shove._Web.Utility.GetRequest("LotteryID"), -1);

            if (!PF.ValidLotteryID(_Site, LotteryID))
            {
                PF.GoError(ErrorNumber.Unknow, "参数错误", "Admin_IsuseAdd");

                return;
            }

            tbLotteryID.Text = LotteryID.ToString();

            string IntervalType = DAL.Functions.F_GetLotteryIntervalType(LotteryID);

            if (IntervalType.StartsWith("分"))
            {
                this.Response.Redirect("IsuseAddForKeno.aspx?LotteryID=" + LotteryID.ToString(), true);

                return;
            }

            if (LotteryID == SLS.Lottery.SFC.ID || LotteryID == SLS.Lottery.ZCSFC.ID || LotteryID == SLS.Lottery.ZCRJC.ID)
            {
                pSFC.Visible = true;
            }

            if (LotteryID == SLS.Lottery.JQC.ID)
            {
                pJQC.Visible = true;
            }

            if (LotteryID == SLS.Lottery.LCBQC.ID)
            {
                pLCBQC.Visible = true;
            }

            if (LotteryID == SLS.Lottery.LCDC.ID)
            {
                pLCDC.Visible = true;
            }

            if (LotteryID == SLS.Lottery.ZCDC.ID)
            {
                ZCDC.Visible = true;

                btnAdd.Enabled = false;
            }

            cbAutoNext10Isuse.Visible = (IntervalType == "天");
            if (LotteryID == SLS.Lottery.SSQ.ID /* || LotteryID == SLS.Lottery.TCCJDLT.ID */)
                pnMultiAdd.Visible = true;
            else
                pnMultiAdd.Visible = false;
        }
    }

    #region Web 窗体设计器生成的代码

    override protected void OnInit(EventArgs e)
    {
        RequestLoginPage = this.Request.Url.AbsoluteUri;

        RequestCompetences = Competences.BuildCompetencesList(Competences.LotteryIsuseScheme);

        base.OnInit(e);
    }

    #endregion

    protected void btnBack_Click(object sender, EventArgs e)
    {
        this.Response.Redirect("Isuse.aspx?LotteryID=" + tbLotteryID.Text, true);
    }

    protected void btnAdd_Click(object sender, System.EventArgs e)
    {
        string Isuse = "";

        try
        {
            Isuse = Shove._Web.Utility.FilteSqlInfusion(tbIsuse.Text.Trim());
        }
        catch { }

        if (Isuse == "")
        {
            Shove._Web.JavaScript.Alert(this.Page, "期号不能为空！");

            return;
        }

        DataTable dt = new DAL.Tables.T_Isuses().Open("[ID]", "[Name] = '" + Isuse + "' and LotteryID = " + Shove._Web.Utility.FilteSqlInfusion(tbLotteryID.Text), "");

        if (dt == null)
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", "Admin_IsuseAdd");

            return;
        }

        if (dt.Rows.Count > 0)
        {
            Shove._Web.JavaScript.Alert(this.Page, "期号已经存在，请不要输入重名期号！");

            return;
        }

        System.DateTime StartTime, EndTime;

        object time = PF.ValidLotteryTime(tbStartTime.Text);

        if (time == null)
        {
            Shove._Web.JavaScript.Alert(this.Page, "开始时间格式输入错误！");
            return;
        }

        StartTime = (DateTime)time;

        time = PF.ValidLotteryTime(tbEndTime.Text);

        if (time == null)
        {
            Shove._Web.JavaScript.Alert(this.Page, "截止时间格式输入错误！");
            return;
        }

        EndTime = (DateTime)time;

        if (EndTime <= StartTime)
        {
            Shove._Web.JavaScript.Alert(this.Page, "截止时间应该在开始时间之后！");

            return;
        }

        string AdditionasXml = "";

        if ((tbLotteryID.Text == SLS.Lottery.SFC.sID || tbLotteryID.Text == SLS.Lottery.ZCSFC.sID || tbLotteryID.Text == SLS.Lottery.ZCRJC.sID) && (BuildAdditionasXmlForSFC(ref AdditionasXml) < 0))
        {
            return;
        }

        if ((tbLotteryID.Text == SLS.Lottery.JQC.sID) && (BuildAdditionasXmlForJQC(ref AdditionasXml) < 0))
        {
            return;
        }

        if ((tbLotteryID.Text == SLS.Lottery.LCBQC.sID) && (BuildAdditionasXmlForLCBQC(ref AdditionasXml) < 0))
        {
            return;
        }

        if ((tbLotteryID.Text == SLS.Lottery.LCDC.sID) && (BuildAdditionasXmlForLCDC(ref AdditionasXml) < 0))
        {
            return;
        }

        if ((tbLotteryID.Text == SLS.Lottery.ZCDC.sID) && (BuildAdditionasXmlForZCDC(ref AdditionasXml) < 0))
        {
            return;
        }

        int LotteryID = int.Parse(tbLotteryID.Text);

        long NewIsuseID = -1;
        string ReturnDescription = "";

        int Result = DAL.Procedures.P_IsuseAdd(LotteryID, Isuse, StartTime, EndTime, AdditionasXml, ref NewIsuseID, ref ReturnDescription);

        if (Result < 0)
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", "Admin_IsuseAdd");

            return;
        }

        if (NewIsuseID < 0)
        {
            PF.GoError(ErrorNumber.Unknow, ReturnDescription, "Admin_IsuseAdd");

            return;
        }

        DAL.Tables.T_TotalMoney dtTotalMoney = new DAL.Tables.T_TotalMoney();

        dtTotalMoney.IsuseID.Value = NewIsuseID;
        dtTotalMoney.TotalMoney.Value = this.tbMoney.Text;

        if (dtTotalMoney.Insert() < 0)
        {
            Shove._Web.JavaScript.Alert(this.Page, "添加奖池奖金失败！");
            return;
        }

        if (cbAutoNext10Isuse.Visible && cbAutoNext10Isuse.Checked && (AdditionasXml == ""))
        {
            string IsuseYear = Isuse.Substring(0, Isuse.Length - 3);
            int IsuseNum = Shove._Convert.StrToInt(Isuse.Substring(Isuse.Length - 3, 3), 0);

            for (int i = 1; i <= 9; i++)
            {
                IsuseNum++;
                string NextIsuse = IsuseYear + IsuseNum.ToString().PadLeft(3, '0');
                StartTime = StartTime.AddDays(1);
                EndTime = EndTime.AddDays(1);
                int Results = -1;
                Results = DAL.Procedures.P_IsuseAdd(LotteryID, NextIsuse, StartTime, EndTime, "", ref NewIsuseID, ref ReturnDescription);
                if (Result < 0)
                {
                    break;
                }

                if (NewIsuseID < 0)
                {
                    continue;
                }
            }
        }
        Shove._Web.Cache.ClearCache(CacheKey.LotteryCalendar);
        Shove._Web.Cache.ClearCache(DataCache.IsusesInfo + tbLotteryID.Text.Trim());
        this.Response.Redirect("Isuse.aspx?LotteryID=" + tbLotteryID.Text, true);
    }

    protected void btnMultiAdd_Click(object sender, System.EventArgs e)
    {
        int LotteryID = int.Parse(tbLotteryID.Text);
        string Isuse = "";
        if (LotteryID != SLS.Lottery.SSQ.ID && LotteryID != SLS.Lottery.TCCJDLT.ID)
        {
            btnMultiAdd.Visible = false;
            btnMultiAdd.Enabled = false;
            return;
        }
        try
        {
            Isuse = Shove._Web.Utility.FilteSqlInfusion(tbIsuse.Text.Trim());
        }
        catch { }

        if (Isuse == "")
        {
            Shove._Web.JavaScript.Alert(this.Page, "期号不能为空！");

            return;
        }
        System.DateTime StartTime, EndTime;

        object time = PF.ValidLotteryTime(tbStartTime.Text);

        if (time == null)
        {
            Shove._Web.JavaScript.Alert(this.Page, "开始时间格式输入错误！");
            return;
        }
        DayOfWeek dow = ((DateTime)time).DayOfWeek;
        switch(LotteryID)
        {
            case SLS.Lottery.TCCJDLT.ID:
                if (dow != DayOfWeek.Monday && dow != DayOfWeek.Wednesday && dow != DayOfWeek.Saturday)
                {
                    Shove._Web.JavaScript.Alert(this.Page, "开始时间输入不正确(周一、三、六)！");
                    return;
                }
                break;
            case SLS.Lottery.SSQ.ID:
                if (dow != DayOfWeek.Tuesday && dow != DayOfWeek.Thursday && dow != DayOfWeek.Sunday)
                {
                    Shove._Web.JavaScript.Alert(this.Page, "开始时间输入不正确(周二、四、日)！");
                    return;
                }
                break;
            default:
                Shove._Web.JavaScript.Alert(this.Page, "开始时间输入不正确！");
                return;
        }
        
        StartTime = (DateTime)time;
        EndTime = StartTime;
        switch (LotteryID)
        {
            case SLS.Lottery.TCCJDLT.ID:
                if (EndTime.DayOfWeek == DayOfWeek.Wednesday)
                    EndTime = EndTime.AddDays(3);
                else
                    EndTime = EndTime.AddDays(2);
                break;
            case SLS.Lottery.SSQ.ID:
                if (EndTime.DayOfWeek == DayOfWeek.Thursday)
                    EndTime = EndTime.AddDays(3);
                else
                    EndTime = EndTime.AddDays(2);
                break;
        }
        int x = 0;
        if (!int.TryParse(tbMultiDays.Text, out x))
        {
            Shove._Web.JavaScript.Alert(this.Page, "增加天数格式输入错误！");
            return;
        }
        long NewIsuseID = -1;
        string ReturnDescription = "";

        DataTable dt;
        for (int i = 0; i < x; i++)
        {
            dt = new DAL.Tables.T_Isuses().Open("[ID]", "[Name] = '" + Isuse + "' and LotteryID = " + Shove._Web.Utility.FilteSqlInfusion(tbLotteryID.Text), "");
            if (dt == null)
            {
                PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", "Admin_IsuseAdd");
                i = i - 1;
            }
            else if (dt.Rows.Count == 0)
            {
                int Result = DAL.Procedures.P_IsuseAdd(LotteryID, Isuse, StartTime, EndTime, "", ref NewIsuseID, ref ReturnDescription);

                if (Result < 0)
                {
                    PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", "Admin_IsuseAdd");
                    return;
                }
                else if (NewIsuseID < 0)
                {
                    PF.GoError(ErrorNumber.Unknow, ReturnDescription, "Admin_IsuseAdd");
                    return;
                }
                else
                {
                    DAL.Tables.T_TotalMoney dtTotalMoney = new DAL.Tables.T_TotalMoney();
                    dtTotalMoney.IsuseID.Value = NewIsuseID;
                    dtTotalMoney.TotalMoney.Value = "";

                    if (dtTotalMoney.Insert() < 0)
                    {
                        Shove._Web.JavaScript.Alert(this.Page, "添加奖池奖金失败！");
                        return;
                    }
                }
            }
            Isuse = (int.Parse(Isuse) + 1).ToString();
            StartTime = EndTime;
            switch (LotteryID)
            {
                case SLS.Lottery.TCCJDLT.ID:
                    if (EndTime.DayOfWeek == DayOfWeek.Wednesday)
                        EndTime = EndTime.AddDays(1);
                    break;
                case SLS.Lottery.SSQ.ID:
                    if (EndTime.DayOfWeek == DayOfWeek.Thursday)
                        EndTime = EndTime.AddDays(1);
                    break;
            }
            EndTime = EndTime.AddDays(2);
            NewIsuseID = -1;
            ReturnDescription = "";
        }
        Shove._Web.Cache.ClearCache(CacheKey.LotteryCalendar);
        Shove._Web.Cache.ClearCache(DataCache.IsusesInfo + tbLotteryID.Text.Trim());
        this.Response.Redirect("Isuse.aspx?LotteryID=" + tbLotteryID.Text, true);
    }

    private int BuildAdditionasXmlForSFC(ref string AdditionasXml)
    {
        TextBox[] tb = new TextBox[14];
        TextBox[] tb_1 = new TextBox[14];
        TextBox[] tb_2 = new TextBox[14];

        for (int i = 0; i < 14; i++)
        {
            tb[i] = (TextBox)this.FindControl("tbSFC" + (i + 1).ToString());
            tb_1[i] = (TextBox)this.FindControl("tbSFC" + (i + 1).ToString() + "_1");
            tb_2[i] = (TextBox)this.FindControl("tbSFC" + (i + 1).ToString() + "_2");

            if ((tb[i].Text.Trim() == "") || (tb_1[i].Text.Trim() == ""))
            {
                Shove._Web.JavaScript.Alert(this.Page, "第 " + (i + 1).ToString() + " 场比赛球队名称输入不完整！");

                return -1;
            }
        }

        AdditionasXml = PF.BuildIsuseAdditionasXmlFor2Team(14,
            tb[0].Text.Trim(), tb_1[0].Text.Trim(), tb_2[0].Text.Trim(),
            tb[1].Text.Trim(), tb_1[1].Text.Trim(), tb_2[1].Text.Trim(),
            tb[2].Text.Trim(), tb_1[2].Text.Trim(), tb_2[2].Text.Trim(),
            tb[3].Text.Trim(), tb_1[3].Text.Trim(), tb_2[3].Text.Trim(),
            tb[4].Text.Trim(), tb_1[4].Text.Trim(), tb_2[4].Text.Trim(),
            tb[5].Text.Trim(), tb_1[5].Text.Trim(), tb_2[5].Text.Trim(),
            tb[6].Text.Trim(), tb_1[6].Text.Trim(), tb_2[6].Text.Trim(),
            tb[7].Text.Trim(), tb_1[7].Text.Trim(), tb_2[7].Text.Trim(),
            tb[8].Text.Trim(), tb_1[8].Text.Trim(), tb_2[8].Text.Trim(),
            tb[9].Text.Trim(), tb_1[9].Text.Trim(), tb_2[9].Text.Trim(),
            tb[10].Text.Trim(), tb_1[10].Text.Trim(), tb_2[10].Text.Trim(),
            tb[11].Text.Trim(), tb_1[11].Text.Trim(), tb_2[11].Text.Trim(),
            tb[12].Text.Trim(), tb_1[12].Text.Trim(), tb_2[12].Text.Trim(),
            tb[13].Text.Trim(), tb_1[13].Text.Trim(), tb_2[13].Text.Trim());

        return 0;
    }

    private int BuildAdditionasXmlForJQC(ref string AdditionasXml)
    {
        TextBox[] tb = new TextBox[8];
        TextBox[] tb_2 = new TextBox[8];

        for (int i = 0; i < 8; i++)
        {
            tb[i] = (TextBox)this.FindControl("tbJQC" + (i + 1).ToString());
            tb_2[i] = (TextBox)this.FindControl("tbJQC" + (i + 1).ToString() + "_2");

            if (tb[i].Text.Trim() == "")
            {
                Shove._Web.JavaScript.Alert(this.Page, "第 " + (i + 1).ToString() + " 个球队名称输入不完整！");

                return -1;
            }
        }

        AdditionasXml = PF.BuildIsuseAdditionasXmlFor1Team(8,
            tb[0].Text.Trim(), tb_2[0].Text.Trim(),
            tb[1].Text.Trim(), tb_2[0].Text.Trim(),
            tb[2].Text.Trim(), tb_2[2].Text.Trim(),
            tb[3].Text.Trim(), tb_2[2].Text.Trim(),
            tb[4].Text.Trim(), tb_2[4].Text.Trim(),
            tb[5].Text.Trim(), tb_2[4].Text.Trim(),
            tb[6].Text.Trim(), tb_2[6].Text.Trim(),
            tb[7].Text.Trim(), tb_2[6].Text.Trim());

        return 0;
    }

    private int BuildAdditionasXmlForLCBQC(ref string AdditionasXml)
    {
        TextBox[] tb = new TextBox[6];
        TextBox[] tb_1 = new TextBox[6];
        TextBox[] tb_2 = new TextBox[6];

        for (int i = 0; i < 6; i++)
        {
            tb[i] = (TextBox)this.FindControl("tbLCBQC" + (i + 1).ToString());
            tb_1[i] = (TextBox)this.FindControl("tbLCBQC" + (i + 1).ToString() + "_1");
            tb_2[i] = (TextBox)this.FindControl("tbLCBQC" + (i + 1).ToString() + "_2");

            if ((tb[i].Text.Trim() == "") || (tb_1[i].Text.Trim() == ""))
            {
                Shove._Web.JavaScript.Alert(this.Page, "第 " + (i + 1).ToString() + " 场比赛球队名称输入不完整！");

                return -1;
            }
        }

        AdditionasXml = PF.BuildIsuseAdditionasXmlFor2Team(6,
            tb[0].Text.Trim(), tb_1[0].Text.Trim(), tb_2[0].Text.Trim(),
            tb[1].Text.Trim(), tb_1[1].Text.Trim(), tb_2[1].Text.Trim(),
            tb[2].Text.Trim(), tb_1[2].Text.Trim(), tb_2[2].Text.Trim(),
            tb[3].Text.Trim(), tb_1[3].Text.Trim(), tb_2[3].Text.Trim(),
            tb[4].Text.Trim(), tb_1[4].Text.Trim(), tb_2[4].Text.Trim(),
            tb[5].Text.Trim(), tb_1[5].Text.Trim(), tb_2[5].Text.Trim());

        return 0;
    }

    private int BuildAdditionasXmlForLCDC(ref string AdditionasXml)
    {
        TextBox[] tb = new TextBox[1];
        TextBox[] tb_1 = new TextBox[1];
        TextBox[] tb_2 = new TextBox[1];

        for (int i = 0; i < 1; i++)
        {
            tb[i] = (TextBox)this.FindControl("tbLCDC" + (i + 1).ToString());
            tb_1[i] = (TextBox)this.FindControl("tbLCDC" + (i + 1).ToString() + "_1");
            tb_2[i] = (TextBox)this.FindControl("tbLCDC" + (i + 1).ToString() + "_2");

            if ((tb[i].Text.Trim() == "") || (tb_1[i].Text.Trim() == ""))
            {
                Shove._Web.JavaScript.Alert(this.Page, "第 " + (i + 1).ToString() + " 场比赛球队名称输入不完整！");

                return -1;
            }
        }

        AdditionasXml = PF.BuildIsuseAdditionasXmlFor2Team(1,
            tb[0].Text.Trim(), tb_1[0].Text.Trim(), tb_2[0].Text.Trim());

        return 0;
    }

    private int BuildAdditionasXmlForZCDC(ref string AdditionasXml)
    {
        int CompetitionCount = Shove._Convert.StrToInt(CompetitionNum.Text.Trim(), 0);

        if (CompetitionCount < 10)
        {
            Shove._Web.JavaScript.Alert(this.Page, "输入的本期比赛总场数输入有误！");

            return -1;
        }

        TextBox[] tb1 = new TextBox[CompetitionCount];
        TextBox[] tb2 = new TextBox[CompetitionCount];
        TextBox[] tb3 = new TextBox[CompetitionCount];
        DropDownList[] ddlLetBall = new DropDownList[CompetitionCount];
        DropDownList[] ddlLeagueType = new DropDownList[CompetitionCount];

        string[] Xmlparams = new string[CompetitionCount * 5];

        //构建格式：类别,主场,客场,让球数,比赛时间|类别,主场,客场,让球数,比赛时间
        int DataCount = DataListZCDC.Items.Count;
        for (int i = 0; i < DataCount; i++)
        {
            tb1[i] = (TextBox)(DataListZCDC.Items[i].FindControl("tb1ZCDC"));
            tb2[i] = (TextBox)(DataListZCDC.Items[i].FindControl("tb2ZCDC"));
            tb3[i] = (TextBox)(DataListZCDC.Items[i].FindControl("tb3ZCDC"));
            ddlLetBall[i] = (DropDownList)(DataListZCDC.Items[i].FindControl("ddlLetBall"));
            ddlLeagueType[i] = (DropDownList)(DataListZCDC.Items[i].FindControl("ddlLeagueType"));

            if ((tb1[i].Text.Trim() == "") || (tb2[i].Text.Trim() == "") || (tb3[i].Text.Trim() == ""))
            {
                Shove._Web.JavaScript.Alert(this.Page, "第 " + (i + 1).ToString() + " 场比赛球队名称输入不完整！");
                return -2;
            }

            object dt = PF.ValidLotteryTime(tb3[i].Text.Trim());
            if (dt == null)
            {
                Shove._Web.JavaScript.Alert(this.Page, "第 " + (i + 1).ToString() + " 场比赛球队时间输入不正确！(格式：0000-00-00 00:00:00)");
                return -3;
            }

            Xmlparams[i * 5] = ddlLeagueType[i].SelectedValue;
            Xmlparams[i * 5 + 1] = tb1[i].Text.Trim();
            Xmlparams[i * 5 + 2] = tb2[i].Text.Trim();
            Xmlparams[i * 5 + 3] = ddlLetBall[i].SelectedValue;
            Xmlparams[i * 5 + 4] = dt.ToString();
        }

        AdditionasXml = PF.BuildIsuseAdditionasXmlForZCDC(Xmlparams);

        return 0;
    }

    protected void btCompetitionNum_Click(object sender, EventArgs e)
    {
        if (CompetitionNum.Text.Trim() == "")
        {
            Shove._Web.JavaScript.Alert(this.Page, "请输入本期比赛的总场数！");
            return;
        }

        int AddCompetitionNum = Shove._Convert.StrToInt(CompetitionNum.Text.Trim(), 0);
        if (AddCompetitionNum < 16)
        {
            Shove._Web.JavaScript.Alert(this.Page, "输入的本期比赛总场数输入有误！");
            return;
        }

        DataListZCDC.Visible = true;
        tableCompetitionNum.Visible = false;

        DataTable dt = new DataTable();
        DataColumn newDC;

        newDC = new DataColumn("ID", System.Type.GetType("System.Int32"));
        dt.Columns.Add(newDC);

        newDC = new DataColumn("HostTeam", System.Type.GetType("System.String"));
        dt.Columns.Add(newDC);

        newDC = new DataColumn("QuestTeam", System.Type.GetType("System.String"));
        dt.Columns.Add(newDC);

        newDC = new DataColumn("Time", System.Type.GetType("System.String"));
        dt.Columns.Add(newDC);

        for (int i = 0; i < AddCompetitionNum; i++)
        {
            DataRow dr = dt.NewRow();
            dr[0] = i + 1;
            dt.Rows.Add(dr);
        }

        DataListZCDC.DataSource = dt.DefaultView;
        DataListZCDC.DataBind();

        btnAdd.Enabled = true;
    }

    protected void DataListZCDC_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.EditItem)
        {
            DropDownList ddlLeagueType = (DropDownList)e.Item.FindControl("ddlLeagueType");
            DropDownList ddlLetBall = (DropDownList)e.Item.FindControl("ddlLetBall");            
            HiddenField HidLeagueType = (HiddenField)e.Item.FindControl("HidLeagueType");
            HiddenField Hiddloseball = (HiddenField)e.Item.FindControl("Hiddloseball");
            HiddenField HidColor = (HiddenField)e.Item.FindControl("HidColor");

            ddlLeagueTypeDataBind(ddlLeagueType);
        //    DropDownListDefault(ddlLeagueType, HidLeagueType.Value.Trim());
            ddlLeagueType.ClearSelection();
            ddlLeagueType.Items.FindByText(HidLeagueType.Value.Trim()).Selected = true;
            ddlLetBall.Text = Hiddloseball.Value.Replace("+","").Trim();
        }
    }

    private void ddlLeagueTypeDataBind(DropDownList ddl)
    {
        if (dt_FootballLeagueTypes == null)
        {
            dt_FootballLeagueTypes = MSSQL.Select("select * from T_FootballLeagueTypes where isUse = 1 order by [Order]");
        }

        if (dt_FootballLeagueTypes == null || dt_FootballLeagueTypes.Rows.Count == 0)
        {
            Shove._Web.JavaScript.Alert(this.Page, "请先添加联赛类别！");

            return;
        }

        Shove.ControlExt.FillDropDownList(ddl, dt_FootballLeagueTypes, "Name", "ID");
    }
    protected void tb_auto_Click(object sender, EventArgs e)
    {
        if (tbIsuse.Text.Trim() == "")
        {
            Shove._Web.JavaScript.Alert(this, "期号不能为空。");
            return;
        }
        DataListZCDC.Visible = true;
        tableCompetitionNum.Visible = false;


        DataTable table = CatchIsues();
        if (table.Rows.Count == 0)
        {
            DataListZCDC.Visible = false;
            tableCompetitionNum.Visible = true;
            Shove._Web.JavaScript.Alert(this, "自动添加失败请手动添加。");
            return;
        }

        CompetitionNum.Text = table.Rows.Count.ToString();
        UPlLeague(table);

        DataListZCDC.DataSource = table;
        DataListZCDC.DataBind();

        btnAdd.Enabled = true;
    }

    protected DataTable CatchIsues()
    {
        int IStart = 0;
        int ILen = 0;
        string Isuse = tbIsuse.Text;
        string Html = string.Empty;
        string Error = string.Empty;
        string[] Trs = null;
        string[] Tds = null;
        string[] Days = null;
        string Date = string.Empty;

        string League = string.Empty;
        string MainTeam = string.Empty;
        string GuestTeam = string.Empty;
        string MatchDate = string.Empty;
        string LoseBall = string.Empty;
        string Color = string.Empty;
        int count = 1;

        DateTime dtTime = DateTime.Now;
        DateTime MatchDateTime = DateTime.Now;

        DataTable table = new DataTable();
        table.Columns.Add("ID",typeof(System.String));
        table.Columns.Add("League", typeof(System.String));
        table.Columns.Add("HostTeam", typeof(System.String));
        table.Columns.Add("QuestTeam", typeof(System.String));
        table.Columns.Add("loseBall", typeof(System.String));
        table.Columns.Add("Time", typeof(System.String));
        table.Columns.Add("color", typeof(System.String));

        Html = GetHtml("http://trade.500wan.com/pages/info/bjdc/zc.php?msgmode=2&expect=" + Isuse + "&jumphost=http://www.500wan.com", true);

        GetStrScope(Html, "<tbody id=\"mathList\">", "<input type=\"hidden\" id=\"yc_changci\" value=\"0\">", out IStart, out ILen);
        Html = Html.Substring(IStart, ILen);
        if (ILen == 0)
        {
            Error = "抓取500万单场期号截取字符串失败";
        }
        Days = Html.Split(new string[] { "<td colspan=\"21\">" }, StringSplitOptions.RemoveEmptyEntries);
        for (int i = 1; i < Days.Length; i++)
        {
            Date = Days[i].Split(new string[] { "</td>" }, StringSplitOptions.RemoveEmptyEntries)[0];
            Date = GetDate(Date);
            Trs = Days[i].Split(new string[] { "/tr>" }, StringSplitOptions.RemoveEmptyEntries);
            for (int j = 1; j < Trs.Length; j++)
            {
                Tds = Trs[j].Split(new string[] { "</td>" }, StringSplitOptions.RemoveEmptyEntries);
                if (Tds.Length == 19 || Tds.Length == 20)
                {
                    League = getHtmlText(Tds[1], 3);
                    GetStrScope(Tds[1], "bgcolor=", ">", out IStart, out ILen);
                    Color = Tds[1].Substring(IStart, ILen).Replace("bgcolor=", "").Replace("\"", "");
                    MatchDate = Date + " " + GetTime(Tds[2]);
                    MainTeam = getHtmlText(Tds[4], 2);
                    LoseBall = getHtmlText(Tds[5], 2);
                    LoseBall = LoseBall.Length == 0 ? getHtmlText(Tds[5], 4) : LoseBall;
                    GuestTeam = getHtmlText(Tds[6], 2);

                    MatchDateTime = Shove._Convert.StrToDateTime(MatchDate + ":00", DateTime.Now.ToString());

                    if (MatchDateTime.CompareTo(dtTime) < 0)
                    {
                        MatchDateTime = MatchDateTime.AddDays(1);
                    }

                    DataRow DRow = table.NewRow();
                    DRow["id"] = (count++).ToString();
                    DRow["League"] = League;
                    DRow["HostTeam"] = MainTeam;
                    DRow["QuestTeam"] = GuestTeam;
                    DRow["loseBall"] = LoseBall;
                    DRow["Time"] = MatchDateTime.ToString();
                    DRow["Color"] = Color;

                    dtTime = MatchDateTime;

                    table.Rows.Add(DRow);
                }
            }
        }

        return table;
    }

    protected void UPlLeague(DataTable table)
    {
        string League = string.Empty;

        DAL.Tables.T_FootballLeagueTypes T_FootballLeagueTypes = new DAL.Tables.T_FootballLeagueTypes();

        foreach (DataRow row in table.Rows)
        {
            if (League == row["League"].ToString())
                continue;
            League = row["League"].ToString();
            if (T_FootballLeagueTypes.GetCount("name='" + League + "'") == 0)
            {
                T_FootballLeagueTypes.Name.Value = League;
                T_FootballLeagueTypes.MarkersColor.Value = row["Color"].ToString();

                T_FootballLeagueTypes.Insert();
            }

        }
    }

   

    #region 抓取代码
    public string GetHtml(string Url, bool IsGB)
    {
        HttpWebResponse hwrs = null;
        StreamReader reader = null;
        HttpWebRequest hwr = null;

        try
        {
            hwr = (HttpWebRequest)HttpWebRequest.Create(Url);      //建立HttpWebRequest對象
            hwr.Timeout = 120000;                                                  //定義服務器超時時間
            // WebProxy proxy = new WebProxy(PorXY, Port);                                      //定義一個網關對象
            //proxy.Credentials = new NetworkCredential("f3210316", "6978233");      //用戶名,密碼
            hwr.UseDefaultCredentials = true;                                      //啟用網關認証
            //    hwr.Proxy = proxy;

            try
            {
                hwrs = (HttpWebResponse)hwr.GetResponse();              //取得回應
            }
            catch
            {
                // MessageBox.Show("无法连接代理！");
                return null;
            }

            //判断HTTP响应状态 
            if (hwrs.StatusCode != HttpStatusCode.OK)
            {
                return null;
            }
            else
            {
                if (IsGB)
                {
                    reader = new StreamReader(hwrs.GetResponseStream(), Encoding.GetEncoding("GB2312"));
                }
                else
                {
                    reader = new StreamReader(hwrs.GetResponseStream(), Encoding.Default);
                }
                string html = reader.ReadToEnd();
                return html;
            }
        }
        catch (SystemException)
        {
            //Log.HandleException("行：3004附近：", "GetHtml(string Url):抓取页面\"" + Url + "\"出错,错误：" + ex.Message);
            return null;
        }
        finally
        {
            if (hwrs != null)
            {
                hwrs.Close();
                hwrs = null;
            }
            if (reader != null)
            {
                reader.Close();
            }

            if (hwr != null)
            {
                hwr = null;
            }
        }
    }

    /// <summary>
    /// 得到子字符串的开始位置与长度不包括结束的字符串
    /// </summary>
    /// <param name="str"></param>
    /// <param name="strStart"></param>
    /// <param name="strEnd"></param>
    /// <param name="IStart"></param>
    /// <param name="ILen"></param>
    public void GetStrScope(string str, string strStart, string strEnd, out int IStart, out int ILen)
    {
        int IEnd = -1;
        IStart = str.IndexOf(strStart);
        if (IStart != -1)
        {
            IEnd = str.IndexOf(strEnd, IStart);
            if (IEnd == -1)
            {
                IStart = 0;
                ILen = 0;
            }
            else
            {
                ILen = IEnd - IStart;
            }
        }
        else
        {
            IStart = 0;
            ILen = 0;
        }
    }

    public bool IsDate(string date)
    {
        return Regex.Match(date, @"^\d{4}-\d{1,2}-\d{1,2}$").Success;
    }


    /// <summary>
    /// 取Html标签中元素
    /// </summary>
    /// <param name="str"></param>
    /// <param name="i"></param>
    /// <returns></returns>
    public string getHtmlText(string str, int i)
    {
        if (i == 1)
        {
            str = str.Split('>')[str.Split('>').Length - 1];
        }
        else
        {
            str = str.Split('>')[str.Split('>').Length - i].Split('<')[0];
        }

        return TrimSpaceletter(str);
    }

    /// <summary>
    /// 去除前后空格
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public string TrimSpaceletter(string str)
    {
        return System.Text.RegularExpressions.Regex.Replace(str, @"^\s*|\s*$", ""); ;
    }

    /// <summary>
    /// 过滤得到日期格式字符串
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public string GetDate(string str)
    {
        return System.Text.RegularExpressions.Regex.Match(str, @"\d{4}-\d{2}-\d{2}").Value;
    }

    /// <summary>
    /// 过滤得到时间格式字符串
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public string GetTime(string str)
    {
        return System.Text.RegularExpressions.Regex.Match(str, @"\d{2}:\d{2}").Value;
    }

    #endregion 抓取代码

}