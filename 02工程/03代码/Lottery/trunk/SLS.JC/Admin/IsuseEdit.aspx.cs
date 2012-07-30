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

public partial class Admin_IsuseEdit : AdminPageBase
{
    private DataTable dt_FootballLeagueTypes;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            long IsuseID = Shove._Convert.StrToLong(Shove._Web.Utility.GetRequest("IsuseID"), -1);

            if (IsuseID < 0)
            {
                PF.GoError(ErrorNumber.Unknow, "参数错误", "Admin_IsuseEdit");

                return;
            }

            int LotteryID = Shove._Convert.StrToInt(Shove._Web.Utility.GetRequest("LotteryID"), -1);

            if (!PF.ValidLotteryID(_Site, LotteryID))
            {
                PF.GoError(ErrorNumber.Unknow, "参数错误", "Admin_IsuseEdit");

                return;
            }

            tbIsuseID.Text = IsuseID.ToString();
            tbLotteryID.Text = LotteryID.ToString();

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
            }

            BindData();

            btnEdit.Visible = _User.Competences.IsOwnedCompetences(Competences.BuildCompetencesList(Competences.LotteryIsuseScheme));
        }
    }

    #region Web 窗体设计器生成的代码

    override protected void OnInit(EventArgs e)
    {
        RequestLoginPage = this.Request.Url.AbsoluteUri;

        RequestCompetences = Competences.BuildCompetencesList(Competences.LotteryIsuseScheme,Competences.QueryData);

        base.OnInit(e);
    }

    #endregion

    private void BindData()
    {
        DataTable dt = new DAL.Tables.T_Isuses().Open("", "[ID]=" + Shove._Web.Utility.FilteSqlInfusion(tbIsuseID.Text), "");

        if ((dt == null) || (dt.Rows.Count < 1))
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", this.Page.GetType().BaseType.FullName);

            return;
        }

        tbIsuse.Text = dt.Rows[0]["Name"].ToString();
        tbStartTime.Text = Shove._Convert.StrToDateTime(dt.Rows[0]["StartTime"].ToString(), "0000-00-00 00:00:00").ToString("yyyy-MM-dd HH:mm:ss");
        tbEndTime.Text = Shove._Convert.StrToDateTime(dt.Rows[0]["EndTime"].ToString(), "0000-00-00 00:00:00").ToString("yyyy-MM-dd HH:mm:ss");

        if (tbLotteryID.Text == SLS.Lottery.SFC.sID || tbLotteryID.Text == SLS.Lottery.ZCSFC.sID || tbLotteryID.Text == SLS.Lottery.ZCRJC.sID)
        {
            dt = new DAL.Tables.T_IsuseForSFC().Open("", "IsuseID = " + Shove._Web.Utility.FilteSqlInfusion(tbIsuseID.Text), "[No]");

            if (dt == null)
            {
                PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", this.Page.GetType().BaseType.FullName);

                return;
            }

            if (dt.Rows.Count != 14)
            {
                return;
            }

            for (int i = 0; i < 14; i++)
            {
                TextBox tb = (TextBox)this.FindControl("tbSFC" + (i + 1).ToString());
                TextBox tb_1 = (TextBox)this.FindControl("tbSFC" + (i + 1).ToString() + "_1");
                TextBox tb_2 = (TextBox)this.FindControl("tbSFC" + (i + 1).ToString() + "_2");

                tb.Text = dt.Rows[i]["HostTeam"].ToString();
                tb_1.Text = dt.Rows[i]["QuestTeam"].ToString();
                tb_2.Text = dt.Rows[i]["DateTime"].ToString();
            }
        }

        if (tbLotteryID.Text == SLS.Lottery.JQC.sID)
        {
            dt = new DAL.Tables.T_IsuseForJQC().Open("", "IsuseID = " + Shove._Web.Utility.FilteSqlInfusion(tbIsuseID.Text), "[No]");

            if ((dt == null) || (dt.Rows.Count < 8))
            {
                PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", this.Page.GetType().BaseType.FullName);

                return;
            }

            for (int i = 0; i < 8; i++)
            {
                TextBox tb = (TextBox)this.FindControl("tbJQC" + (i + 1).ToString());
                TextBox tb_2 = (TextBox)this.FindControl("tbJQC" + (i + 1).ToString() + "_2");

                tb.Text = dt.Rows[i]["Team"].ToString();
                tb_2.Text = dt.Rows[i]["DateTime"].ToString();
            }
        }

        if (tbLotteryID.Text == SLS.Lottery.LCBQC.sID)
        {
            dt = new DAL.Tables.T_IsuseForLCBQC().Open("", "IsuseID = " + Shove._Web.Utility.FilteSqlInfusion(tbIsuseID.Text), "[No]");

            if ((dt == null) || (dt.Rows.Count < 6))
            {
                PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", this.Page.GetType().BaseType.FullName);

                return;
            }

            for (int i = 0; i < 6; i++)
            {
                TextBox tb = (TextBox)this.FindControl("tbLCBQC" + (i + 1).ToString());
                TextBox tb_1 = (TextBox)this.FindControl("tbLCBQC" + (i + 1).ToString() + "_1");
                TextBox tb_2 = (TextBox)this.FindControl("tbLCBQC" + (i + 1).ToString() + "_2");

                tb.Text = dt.Rows[i]["HostTeam"].ToString();
                tb_1.Text = dt.Rows[i]["QuestTeam"].ToString();
                tb_2.Text = dt.Rows[i]["DateTime"].ToString();
            }
        }

        if (tbLotteryID.Text == SLS.Lottery.LCDC.sID)
        {
            dt = new DAL.Tables.T_IsuseForLCDC().Open("", "IsuseID = " + Shove._Web.Utility.FilteSqlInfusion(tbIsuseID.Text), "[No]");

            if ((dt == null) || (dt.Rows.Count < 1))
            {
                PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", this.Page.GetType().BaseType.FullName);

                return;
            }

            for (int i = 0; i < 1; i++)
            {
                TextBox tb = (TextBox)this.FindControl("tbLCDC" + (i + 1).ToString());
                TextBox tb_1 = (TextBox)this.FindControl("tbLCDC" + (i + 1).ToString() + "_1");
                TextBox tb_2 = (TextBox)this.FindControl("tbLCDC" + (i + 1).ToString() + "_2");

                tb.Text = dt.Rows[i]["HostTeam"].ToString();
                tb_1.Text = dt.Rows[i]["QuestTeam"].ToString();
                tb_2.Text = dt.Rows[i]["DateTime"].ToString();
            }
        }     

        DataTable dtTestNumber = new DAL.Tables.T_TestNumber().Open("", "IsuseID = " + Shove._Web.Utility.FilteSqlInfusion(tbIsuseID.Text), "");

        DataTable dtTotalMoney = new DAL.Tables.T_TotalMoney().Open("", "IsuseID= " + Shove._Web.Utility.FilteSqlInfusion(tbIsuseID.Text) , "");


        if (dtTestNumber == null)
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", this.Page.GetType().BaseType.FullName);

            return;
        }
        if (dtTotalMoney == null)
        {
            PF.GoError(ErrorNumber.DataReadWrite,"数据库繁忙，请重试",this.Page.GetType().BaseType.FullName);
            return;
        }
        if (dtTestNumber.Rows.Count > 0)
        {
            tbTestNumber.Text = dtTestNumber.Rows[0]["TestNumber"].ToString();
            hidID.Value = dtTestNumber.Rows[0]["ID"].ToString();
        }
        if (dtTotalMoney.Rows.Count > 0)
        {
            tbMoney.Text = dtTotalMoney.Rows[0]["TotalMoney"].ToString();
            moneyID.Value = dtTotalMoney.Rows[0]["ID"].ToString();
        }
    }

    protected void btnEdit_Click(object sender, System.EventArgs e)
    {
        string Isuse = "";

        try
        {
            Isuse = Shove._Web.Utility.FilteSqlInfusion(tbIsuse.Text.Trim());
        }
        catch
        { }

        if (Isuse == "")
        {
            Shove._Web.JavaScript.Alert(this.Page, "期号不能为空！");

            return;
        }

        DataTable dt = new DAL.Tables.T_Isuses().Open("[ID]", "[Name]='" + Isuse + "' and LotteryID=" + Shove._Web.Utility.FilteSqlInfusion(tbLotteryID.Text) + " and [ID] <> " + Shove._Web.Utility.FilteSqlInfusion(tbIsuseID.Text), "");

        if (dt == null)
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", this.Page.GetType().BaseType.FullName);

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

        long IsuseID = long.Parse(tbIsuseID.Text);

        int ReturnValue = -1;
        string ReturnDescription = "";

        int Result = DAL.Procedures.P_IsuseEdit(IsuseID, Isuse, StartTime, EndTime, AdditionasXml, ref ReturnValue, ref ReturnDescription);

        if (Result < 0)
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", this.Page.GetType().BaseType.FullName);

            return;
        }

        if (ReturnValue < 0)
        {
            PF.GoError(ErrorNumber.Unknow, ReturnDescription, this.Page.GetType().BaseType.FullName);

            return;
        }

        if (tbTestNumber.Text.Trim() != "")
        {
            DAL.Tables.T_TestNumber t_TestNumber = new DAL.Tables.T_TestNumber();

            t_TestNumber.TestNumber.Value = tbTestNumber.Text.Trim();
            t_TestNumber.IsuseID.Value = IsuseID.ToString();

            if (Shove._Convert.StrToLong(hidID.Value, 0) > 0)
            {
                t_TestNumber.Update("ID=" + hidID.Value);
            }
            else
            {
                t_TestNumber.Insert();
            }
        }

        if (tbMoney.Text.Trim() != "")
        {
            DAL.Tables.T_TotalMoney t_TotalMoney = new DAL.Tables.T_TotalMoney();
            t_TotalMoney.TotalMoney.Value = tbMoney.Text.Trim();
            t_TotalMoney.IsuseID.Value = tbIsuseID.Text;

            if (Shove._Convert.StrToLong(moneyID.Value, 0) > 0)
            {
                t_TotalMoney.Update("ID=" + moneyID.Value);
            }
            else
            {
                t_TotalMoney.Insert();
            }
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
        int DataCount = DataListZCDC.Items.Count;
        if (DataCount < 10)
        {
            Shove._Web.JavaScript.Alert(this.Page, "本期比赛的总场数输入有误！");
            return -1;
        }

        TextBox[] tb1 = new TextBox[DataCount];
        TextBox[] tb2 = new TextBox[DataCount];
        TextBox[] tb3 = new TextBox[DataCount];
        DropDownList[] ddlLetBall = new DropDownList[DataCount];
        DropDownList[] ddlLeagueType = new DropDownList[DataCount];

        string[] Xmlparams = new string[DataCount * 5];

        //构建格式：类别,主场,客场,让球数,比赛时间|类别,主场,客场,让球数,比赛时间
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

    protected void DataListZCDC_ItemCommand(object source, DataListCommandEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.EditItem)
        {
            if (e.CommandName == "btEdit")
            {
                this.DataListZCDC.EditItemIndex = e.Item.ItemIndex;
            }
            if (e.CommandName == "btUpdate")
            {
                try
                {
                    TextBox tb1 = (TextBox)(DataListZCDC.Items[e.Item.ItemIndex].FindControl("tb1ZCDC"));
                    TextBox tb2 = (TextBox)(DataListZCDC.Items[e.Item.ItemIndex].FindControl("tb2ZCDC"));
                    TextBox tb3 = (TextBox)(DataListZCDC.Items[e.Item.ItemIndex].FindControl("tb3ZCDC"));
                    DropDownList ddlLetBall = (DropDownList)(DataListZCDC.Items[e.Item.ItemIndex].FindControl("ddlLetBall"));
                    DropDownList ddlLeagueType = (DropDownList)(DataListZCDC.Items[e.Item.ItemIndex].FindControl("ddlLeagueType"));
                    HiddenField hf1 = (HiddenField)(DataListZCDC.Items[e.Item.ItemIndex].FindControl("hfId"));

                    if ((tb1.Text.Trim() == "") || (tb2.Text.Trim() == "") || (tb3.Text.Trim() == ""))
                    {
                        Shove._Web.JavaScript.Alert(this.Page, "本场比赛球队名称输入不完整！");

                        return;
                    }

                    object dt = PF.ValidLotteryTime(tb3.Text.Trim());

                    if (dt == null)
                    {
                        Shove._Web.JavaScript.Alert(this.Page, "本场比赛球队时间输入不正确！(格式：0000-00-00 00:00:00)");

                        return;
                    }

                    int id = int.Parse(hf1.Value);
                    long IsuseID = int.Parse(tbIsuseID.Text);
                    //int return_int = 0;
                    //string return_str = "";
                    //DAL.Procedures.P_IsuseEditOneForZCDC(id, int.Parse(ddlLeagueType.SelectedValue), tb1.Text.Trim(), tb2.Text.Trim(), tb3.Text.Trim(), IsuseID, ddlLeagueType.SelectedValue, ref return_int, ref return_str);
                    //if (return_int < 0)
                    //{
                    //    Shove._Web.JavaScript.Alert(this.Page, return_str);
                    //    return;
                    //}

                    //Shove._Web.JavaScript.Alert(this.Page, "更新成功!");
                }
                catch
                {
                    PF.GoError(ErrorNumber.Unknow, "单场信息更新错误。", this.Page.GetType().BaseType.FullName);

                    return;
                }

                this.DataListZCDC.EditItemIndex = -1;
            }

            BindData();
        }
    }

    protected void DataListZCDC_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.EditItem)
        {
            HiddenField hfTypeid = (HiddenField)(e.Item.FindControl("hfTypeid"));
            DropDownList ddlLeagueType = (DropDownList)(e.Item.FindControl("ddlLeagueType"));

            ddlLeagueTypeDataBind(ddlLeagueType);
            DropDownListDefault(ddlLeagueType, hfTypeid.Value);

            HiddenField hfLetBall = (HiddenField)(e.Item.FindControl("hfLetBall"));
            DropDownList ddlLetBall = (DropDownList)(e.Item.FindControl("ddlLetBall"));
            DropDownListDefault(ddlLetBall, hfLetBall.Value);
        }
    }

    private void ddlLeagueTypeDataBind(DropDownList ddl)
    {
        if (dt_FootballLeagueTypes == null)
        {
            dt_FootballLeagueTypes = MSSQL.Select("select ID, Name from T_FootballLeagueTypes where isUse = 1 order by [Order]");
        }

        if (dt_FootballLeagueTypes == null || dt_FootballLeagueTypes.Rows.Count == 0)
        {
            Shove._Web.JavaScript.Alert(this.Page, "请先添加联赛类别！");

            return;
        }

        ddl.DataSource = dt_FootballLeagueTypes.DefaultView;
        ddl.DataTextField = "Name";
        ddl.DataValueField = "ID";
        ddl.DataBind();
    }

    private void DropDownListDefault(DropDownList ddl, string defaultValue)
    {
        foreach (ListItem item in ddl.Items)
        {
            if (item.Value == defaultValue)
            {
                item.Selected = true;
            }
        }
    }

    protected void btnBack_Click(object sender, System.EventArgs e)
    {
        this.Response.Redirect("Isuse.aspx?LotteryID=" + tbLotteryID.Text, true);
    }
}
