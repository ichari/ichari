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

public partial class Admin_IsuseAdd2 : AdminPageBase
{
    private DataTable dt_FootballLeagueTypes;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            int LotteryID = Shove._Convert.StrToInt(Shove._Web.Utility.GetRequest("LotteryID"), -1);

            if (LotteryID != 1 && LotteryID != 2 && LotteryID != 3 && LotteryID != 4 && LotteryID != 9 && LotteryID != 10 && LotteryID != 14 && LotteryID != 15 && LotteryID != 39)
            {
                PF.GoError(ErrorNumber.Unknow, "参数错误", this.GetType().FullName);

                return;
            }

            tbLotteryID.Text = LotteryID.ToString();

            string IntervalType = DAL.Functions.F_GetLotteryIntervalType(LotteryID);

            if (IntervalType.StartsWith("分"))
            {
                this.Response.Redirect("IsuseAddForKeno.aspx?LotteryID=" + LotteryID.ToString(), true);

                return;
            }

            if (LotteryID == SLS.Lottery.SFC.ID)
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
        this.Response.Redirect("Isuse2.aspx?LotteryID=" + tbLotteryID.Text, true);
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
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", this.GetType().FullName);

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

        if ((tbLotteryID.Text == SLS.Lottery.SFC.sID) && (BuildAdditionasXmlForSFC(ref AdditionasXml) < 0))
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
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", this.GetType().FullName);

            return;
        }

        if (NewIsuseID < 0)
        {
            PF.GoError(ErrorNumber.Unknow, ReturnDescription, this.GetType().FullName);

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
                if (Results < 0)
                {
                    break;
                }

                if (NewIsuseID < 0)
                {
                    continue;
                }
            }
        }

        this.Response.Redirect("Isuse2.aspx?LotteryID=" + tbLotteryID.Text, true);
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

            ddlLeagueTypeDataBind(ddlLeagueType);
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
}
