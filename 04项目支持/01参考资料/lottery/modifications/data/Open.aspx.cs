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
using System.Data.SqlClient;

using Shove.Database;
using System.Text;

public partial class Admin_Open : AdminPageBase
{
    private bool Step1IsOpen;
    private bool Step2IsOpen;

    protected void Page_Load(object sender, EventArgs e)
    {
        this.Server.ScriptTimeout = 60 * 10;

        if (!this.IsPostBack)
        {
            BindDataForLottery();

            BindDataForIsuse();

            h_SchemeID.Value = "0";
        }

        btnGO_Step1.AlertText = "确信输入无误，并立即执行开奖第一步骤吗？";
    }

    #region Web 窗体设计器生成的代码

    override protected void OnInit(EventArgs e)
    {
        RequestLoginPage = this.Request.Url.AbsoluteUri;

        RequestCompetences = Competences.BuildCompetencesList(Competences.LotteryWin);

        base.OnInit(e);
    }

    #endregion

    private void BindDataForLottery()
    {
        DataTable dt = new DAL.Tables.T_Lotteries().Open("[ID], [Name]", "[ID] in (" + (_Site.UseLotteryListRestrictions == "" ? "-1" : _Site.UseLotteryListRestrictions) + ")", "[Order]");

        if (dt == null)
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", this.GetType().BaseType.FullName);

            return;
        }

        Shove.ControlExt.FillDropDownList(ddlLottery, dt, "Name", "ID");

        if (ddlLottery.Items.Count < 1)
        {
            btnGO_Step1.Enabled = false;
            btnGO_Step2.Enabled = false;
            btnGO_Step3.Enabled = false;
            tbWinNumber.Enabled = false;
        }
        else
        {
            ddlLottery_SelectedIndexChanged(ddlLottery, new EventArgs());
        }
    }

    private void BindDataForIsuse()
    {
        if (ddlLottery.Items.Count < 1)
        {
            return;
        }

        string Code =  "and EndTime < GetDate()";

        DataTable dt = new DAL.Tables.T_Isuses().Open("[ID], [Name]", "LotteryID = " + Shove._Web.Utility.FilteSqlInfusion(ddlLottery.SelectedValue) + Code + " and isOpened = 0", "EndTime");

        if (dt == null)
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", this.GetType().BaseType.FullName);

            return;
        }

        Shove.ControlExt.FillDropDownList(ddlIsuse, dt, "Name", "ID");

        if (ddlIsuse.Items.Count > 0)
        {
            WinNumberOther.Visible = true;
            btnGO_Step1.Enabled = true;
            btnGO_Step2.Enabled = false;
            btnGO_Step3.Enabled = false;
            tbWinNumber.Enabled = true;
        }
        else
        {
            WinNumberOther.Visible = true;
            btnGO_Step1.Enabled = false;
            btnGO_Step2.Enabled = false;
            btnGO_Step3.Enabled = false;
            tbWinNumber.Enabled = false;
        }
    }

    private void BindDataForWinMoney()
    {
        if (ddlLottery.Items.Count < 1)
        {
            return;
        }

        DataTable dt = new DAL.Tables.T_WinTypes().Open("", "LotteryID = " + Shove._Web.Utility.FilteSqlInfusion(ddlLottery.SelectedValue), "[Order]");

        if (dt == null)
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", this.GetType().BaseType.FullName);

            return;
        }

        g.DataSource = dt.DefaultView;
        g.DataBind();
    }

    protected void ddlLottery_SelectedIndexChanged(object sender, System.EventArgs e)
    {
        BindDataForWinMoney();

        string WinLotteryExemple = "格式：" + DAL.Functions.F_GetLotteryWinNumberExemple(int.Parse(ddlLottery.SelectedValue));

        labTip.Text = WinLotteryExemple;
        tbWinNumber.MaxLength = WinLotteryExemple.Length - 3;

        tbOpenAffiche.Value = new OpenAfficheTemplates()[int.Parse(ddlLottery.SelectedValue)];

        BindDataForIsuse();
    }

    protected void ddlIsuse_SelectedIndexChanged(object sender, System.EventArgs e)
    {
        if (ddlLottery.Items.Count < 1)
        {
            return;
        }

        DataTable dt = new DAL.Tables.T_Isuses().Open("WinLotteryNumber, isOpened", "[ID] = " + Shove._Web.Utility.FilteSqlInfusion(ddlIsuse.SelectedValue), "");

        if ((dt == null) || (dt.Rows.Count < 1))
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", this.GetType().BaseType.FullName);

            return;
        }

        bool isOpened = Shove._Convert.StrToBool(dt.Rows[0]["isOpened"].ToString(), true);
        string WinLotteryNumber = dt.Rows[0]["WinLotteryNumber"].ToString();

        if (isOpened)
        {
            btnGO_Step1.Enabled = false;
            btnGO_Step2.Enabled = false;
            btnGO_Step3.Enabled = false;
            tbWinNumber.Enabled = false;

            PF.GoError(ErrorNumber.Unknow, "此期已经开奖了，不能重复开奖。", this.GetType().BaseType.FullName);

            return;
        }

        WinNumberOther.Visible = true;
        btnGO_Step1.Enabled = true;
        btnGO_Step2.Enabled = false;
        btnGO_Step3.Enabled = false;
        tbWinNumber.Enabled = true;

        if (WinLotteryNumber != "")
        {
            tbWinNumber.Text = WinLotteryNumber;
        }
    }

    protected void g_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DataKey key = g.DataKeys[g.DataKeys.Count - 1];

            double money = Shove._Convert.StrToDouble(key.Values[0].ToString(), 0);
            ((TextBox)e.Row.Cells[1].FindControl("tbMoney")).Text = (money == 0 ? "" : money.ToString());

            money = Shove._Convert.StrToDouble(key.Values[1].ToString(), 0);
            ((TextBox)e.Row.Cells[2].FindControl("tbMoneyNoWithTax")).Text = (money == 0 ? "" : money.ToString());
        }
    }

    protected void btnGO_Step1_Click(object sender, EventArgs e)
    {
        btnGO_Step1.AlertText = "";

        if (ddlLottery.SelectedValue == SLS.Lottery.ZCDC.sID)
        {
            Shove._Web.JavaScript.Alert(this.Page, "足彩单场不支持分步开奖。");

            return;
        }

        tbWinNumber.Text = Shove._Convert.ToDBC(tbWinNumber.Text.Trim().Replace("　", " ")).Trim();

        if (!new SLS.Lottery()[int.Parse(ddlLottery.SelectedValue)].AnalyseWinNumber(tbWinNumber.Text))
        {
            Shove._Web.JavaScript.Alert(this.Page, "开奖号码不正确！");

            return;
        }

        SystemOptions so = new SystemOptions();
        bool isCompareWinMoneyNoWithFax = so["isCompareWinMoneyNoWithFax"].ToBoolean(true);

        string WinListXML = "<WinLists>";

        double[] WinMoneyList = new double[g.Rows.Count * 2];

        for (int i = 0; i < g.Rows.Count; i++)
        {
            WinMoneyList[i * 2] = Shove._Convert.StrToDouble(((TextBox)g.Rows[i].Cells[1].FindControl("tbMoney")).Text, 0);
            WinMoneyList[i * 2 + 1] = Shove._Convert.StrToDouble(((TextBox)g.Rows[i].Cells[2].FindControl("tbMoneyNoWithTax")).Text, 0);

            if (WinMoneyList[i * 2] < 0)
            {
                Shove._Web.JavaScript.Alert(this.Page, "第 " + (i + 1).ToString() + " 项奖金输入错误！");

                return;
            }

            if (WinMoneyList[i * 2] < WinMoneyList[i * 2 + 1])
            {
                if (isCompareWinMoneyNoWithFax)
                {
                    Shove._Web.JavaScript.Alert(this.Page, "第 " + (i + 1).ToString() + " 项税后奖金输入错误(不能大于税前奖金)！");

                    return;
                }
            }

            WinListXML += "<WinList defaultMoney=\"" + WinMoneyList[i * 2].ToString() + "\" DefaultMoneyNoWithTax=\"" + WinMoneyList[i * 2 + 1].ToString() + "\"/>";
        }

        WinListXML += "</WinLists>";

        DataTable dtIsuseBonuses = new DAL.Tables.T_IsuseBonuses().Open("", "IsuseID = " + Shove._Web.Utility.FilteSqlInfusion(ddlIsuse.SelectedValue), "");

        if (dtIsuseBonuses == null)
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", this.GetType().BaseType.FullName);

            return;
        }

        if (dtIsuseBonuses.Rows.Count < 1)
        {
            int ReturnValue = -1;
            string ReturnDescription = "";


            int Result = DAL.Procedures.P_IsuseBonusesAdd(Shove._Convert.StrToLong(ddlIsuse.SelectedValue, 0), _User.ID, WinListXML, ref ReturnValue, ref ReturnDescription);

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

            Shove._Web.JavaScript.Alert(this.Page, "请下一位管理员继续开奖！");

            return;
        }

        if (dtIsuseBonuses.Rows[0]["UserID"].ToString() == _User.ID.ToString())
        {
            Shove._Web.JavaScript.Alert(this.Page, "请下一位管理员继续开奖！");

            return;
        }

        for (int i = 0; i < dtIsuseBonuses.Rows.Count; i++)
        {
            if ((WinMoneyList[i * 2] != Shove._Convert.StrToDouble(dtIsuseBonuses.Rows[i]["defaultMoney"].ToString(), 0)) || (WinMoneyList[i * 2 + 1] != Shove._Convert.StrToDouble(dtIsuseBonuses.Rows[i]["DefaultMoneyNoWithTax"].ToString(), 0)))
            {
                DAL.Tables.T_IsuseBonuses T_IsuseBonuses = new DAL.Tables.T_IsuseBonuses();
                T_IsuseBonuses.Delete("IsuseID = " + ddlIsuse.SelectedValue);

                Shove._Web.JavaScript.Alert(this.Page, "两次奖项输入不一致，请联系上一次开奖操作员！");

                return;
            }
        }

        DataTable dt = new DAL.Tables.T_Schemes().Open("* ", "IsuseID = " + Shove._Web.Utility.FilteSqlInfusion(ddlIsuse.SelectedValue) + " and isOpened = 0 and Buyed = 1", "[ID]");

        if (dt == null)
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", this.GetType().BaseType.FullName);

            return;
        }

        StringBuilder sb = new StringBuilder();

        string NoWinSchemeID = "";

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            string LotteryNumber = dt.Rows[i]["LotteryNumber"].ToString();
            string Description = "";
            double WinMoneyNoWithTax = 0;


            double WinMoney = 0;

            try
            {
                WinMoney = new SLS.Lottery()[int.Parse(ddlLottery.SelectedValue)].ComputeWin(LotteryNumber, tbWinNumber.Text.Trim(), ref Description, ref WinMoneyNoWithTax, int.Parse(dt.Rows[i]["PlayTypeID"].ToString()), WinMoneyList);
            }
            catch
            {
                WinMoney = 0;

                new Log("System").Write("方案 ID:" + dt.Rows[i]["ID"].ToString() + " 算奖出现错误!");
            }

            if (WinMoney == 0)
            {
                NoWinSchemeID += dt.Rows[i]["ID"].ToString()  + ",";

                continue;
            }

            sb.Append("update T_Schemes set EditWinMoney = ").Append(WinMoney * Shove._Convert.StrToInt(dt.Rows[i]["Multiple"].ToString(), 1))
                .Append(", EditWinMoneyNoWithTax = ").Append(WinMoneyNoWithTax * Shove._Convert.StrToInt(dt.Rows[i]["Multiple"].ToString(), 1))
                .Append(", WinDescription = '").Append(Description).Append("'")
                .Append(" where [ID] = ").AppendLine(dt.Rows[i]["ID"].ToString());
        }

        Shove.Database.MSSQL.ExecuteNonQuery(sb.ToString(), new Shove.Database.MSSQL.Parameter[0]);

        if (NoWinSchemeID.EndsWith(","))
        {
            NoWinSchemeID = NoWinSchemeID.Substring(0, NoWinSchemeID.Length - 1);
        }

        if (!string.IsNullOrEmpty(NoWinSchemeID))
        {
            StringBuilder sb1 = new StringBuilder();

            sb1.Append("update T_Schemes set EditWinMoney = 0")
                .Append(", EditWinMoneyNoWithTax = 0, isOpened = 1 , OpenOperatorID=" + _User.ID.ToString())
                .Append(", WinDescription = ''")
                .Append(" where [ID] in (" + NoWinSchemeID + ")");

            Shove.Database.MSSQL.ExecuteNonQuery(sb1.ToString(), new Shove.Database.MSSQL.Parameter[0]);
        }

        dt = new DAL.Tables.T_Schemes().Open("top 1 * ", "IsuseID = " + Shove._Web.Utility.FilteSqlInfusion(ddlIsuse.SelectedValue) + " and isOpened = 0 and Buyed = 1 and WinDescription is null", "[ID]");

        if (dt == null)
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", this.GetType().BaseType.FullName);

            return;
        }

        Step1IsOpen = (dt.Rows.Count > 0);

        btnGO_Step1.Enabled = Step1IsOpen;
        btnGO_Step2.Enabled = (!Step1IsOpen);
        btnGO_Step3.Enabled = ((!Step1IsOpen) && (!Step2IsOpen));

        string Message = "请再次执行第一步";

        if (!Step1IsOpen)
        {
            Message = "开奖步骤一已经完成，请执行第二步.";
        }

        Shove._Web.JavaScript.Alert(this.Page, Message);
    }

    protected void btnGO_Step2_Click(object sender, EventArgs e)
    {
        btnGO_Step1.AlertText = "";

        if (ddlLottery.SelectedValue == SLS.Lottery.ZCDC.sID)
        {
            Shove._Web.JavaScript.Alert(this.Page, "足彩单场不支持分步开奖。");

            return;
        }

        tbWinNumber.Text = Shove._Convert.ToDBC(tbWinNumber.Text.Trim().Replace("　", " ")).Trim();

        if (!new SLS.Lottery()[int.Parse(ddlLottery.SelectedValue)].AnalyseWinNumber(tbWinNumber.Text))
        {
            Shove._Web.JavaScript.Alert(this.Page, "开奖号码不正确！");

            return;
        }

        SystemOptions so = new SystemOptions();
        bool isCompareWinMoneyNoWithFax = so["isCompareWinMoneyNoWithFax"].ToBoolean(true);

        double[] WinMoneyList = new double[g.Rows.Count * 2];

        for (int i = 0; i < g.Rows.Count; i++)
        {
            WinMoneyList[i * 2] = Shove._Convert.StrToDouble(((TextBox)g.Rows[i].Cells[1].FindControl("tbMoney")).Text, 0);
            WinMoneyList[i * 2 + 1] = Shove._Convert.StrToDouble(((TextBox)g.Rows[i].Cells[2].FindControl("tbMoneyNoWithTax")).Text, 0);

            if (WinMoneyList[i * 2] < 0)
            {
                Shove._Web.JavaScript.Alert(this.Page, "第 " + (i + 1).ToString() + " 项奖金输入错误！");

                return;
            }

            if (WinMoneyList[i * 2] < WinMoneyList[i * 2 + 1])
            {
                if (isCompareWinMoneyNoWithFax)
                {
                    Shove._Web.JavaScript.Alert(this.Page, "第 " + (i + 1).ToString() + " 项税后奖金输入错误(不能大于税前奖金)！");

                    return;
                }
            }
        }

        if (Shove._Convert.ToTextCode(tbOpenAffiche.Value.Trim()) == "")
        {
            Shove._Web.JavaScript.Alert(this.Page, "请输入开奖公告！");

            return;
        }

        DataTable dt = new DAL.Tables.T_ElectronTicketAgentSchemes().Open("*", "IsuseID = " + Shove._Web.Utility.FilteSqlInfusion(ddlIsuse.SelectedValue) + " and WinMoney is null and state = 1", "[ID]");

        if (dt == null)
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", this.GetType().BaseType.FullName);

            return;
        }

        DAL.Tables.T_ElectronTicketAgentSchemes t_ElectronTicketAgentSchemes = new DAL.Tables.T_ElectronTicketAgentSchemes();

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            string LotteryNumber = dt.Rows[i]["LotteryNumber"].ToString();
            string Description = "";
            double WinMoneyNoWithTax = 0;

            double WinMoney = new SLS.Lottery()[int.Parse(ddlLottery.SelectedValue)].ComputeWin(LotteryNumber, tbWinNumber.Text.Trim(), ref Description, ref WinMoneyNoWithTax, int.Parse(dt.Rows[i]["PlayTypeID"].ToString()), WinMoneyList);

            t_ElectronTicketAgentSchemes.WinMoney.Value = WinMoney * Shove._Convert.StrToInt(dt.Rows[i]["Multiple"].ToString(), 1);
            t_ElectronTicketAgentSchemes.WinMoneyWithoutTax.Value = WinMoneyNoWithTax * Shove._Convert.StrToInt(dt.Rows[i]["Multiple"].ToString(), 1);
            t_ElectronTicketAgentSchemes.WinDescription.Value = Description;

            t_ElectronTicketAgentSchemes.Update("[ID] =" + dt.Rows[i]["ID"].ToString());
        }

        dt = new DAL.Tables.T_ElectronTicketAgentSchemes().Open("top 1 * ", "IsuseID = " + Shove._Web.Utility.FilteSqlInfusion(ddlIsuse.SelectedValue) + " and state = 1 and WinMoney is null", "[ID]");

        if (dt == null)
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", this.GetType().BaseType.FullName);

            return;
        }

        Step2IsOpen = (dt.Rows.Count > 0);

        btnGO_Step2.Enabled = Step2IsOpen;
        btnGO_Step3.Enabled = (!Step2IsOpen);

        string Message = "请再次执行第二步";

        if (!Step2IsOpen)
        {
            Message = "开奖步骤二已经完成，请执行第三步.";
        }

        Shove._Web.JavaScript.Alert(this.Page, Message);
    }

    protected void btnGO_Step3_Click(object sender, EventArgs e)
    {
        btnGO_Step1.AlertText = "";

        if (ddlLottery.SelectedValue == SLS.Lottery.ZCDC.sID)
        {
            Shove._Web.JavaScript.Alert(this.Page, "足彩单场不支持分步开奖。");

            return;
        }

        tbWinNumber.Text = Shove._Convert.ToDBC(tbWinNumber.Text.Trim().Replace("　", " ")).Trim();

        if (!new SLS.Lottery()[int.Parse(ddlLottery.SelectedValue)].AnalyseWinNumber(tbWinNumber.Text))
        {
            Shove._Web.JavaScript.Alert(this.Page, "开奖号码不正确！");

            return;
        }

        int SchemeCount = 0, QuashCount = 0, WinCount = 0, WinNoBuyCount = 0;
        //  总方案数，处理时撤单数，中奖数，中奖但未成功数

        int ReturnValue = -1;
        string ReturnDescription = "";
        DataSet ds = null;
        bool isEndOpen = false;

        string ConnectionString2 = Shove._Web.WebConfig.GetAppSettingsString("ConnectionString");
        SqlConnection conn1 = Shove.Database.MSSQL.CreateDataConnection<System.Data.SqlClient.SqlConnection>(ConnectionString2 + ";Connect Timeout=120;");

        int Result = P_Win(conn1, ref ds,
             long.Parse(ddlIsuse.SelectedValue),
             tbWinNumber.Text.Trim(),
             tbOpenAffiche.Value,
             _User.ID, 
             true,
             ref SchemeCount, ref QuashCount, ref WinCount, ref WinNoBuyCount,
             ref isEndOpen,
             ref ReturnValue, ref ReturnDescription);

        if ((ds == null) || (ReturnDescription != "") || (ReturnValue < 0) || (Result < 0))
        {
            Response.Write(ddlIsuse.SelectedValue + "<br>");
            Response.Write(tbWinNumber.Text.Trim() + "<br>");
            Response.Write(tbOpenAffiche.Value + "<br>");
            Response.Write(_User.ID.ToString() + "<br>");
            Response.Write(SchemeCount.ToString() + "<br>");
            Response.Write(QuashCount.ToString() + "<br>");
            Response.Write(WinCount.ToString() + "<br>");
            Response.Write(WinNoBuyCount.ToString() + "<br>");
            Response.Write(isEndOpen.ToString() + "<br>");
            Response.Write(ReturnValue.ToString() + "<br>");
            Response.Write(ReturnDescription.ToString() + "<br>");
            Response.Write(Result.ToString() + "<br>");

            //PF.GoError(ErrorNumber.DataReadWrite, "数据库读写错误。", this.GetType().BaseType.FullName + Result);

            return;
        }

        PF.SendWinNotification(ds);

        btnGO_Step1.Enabled = false;
        btnGO_Step2.Enabled = false;
        btnGO_Step3.Enabled = true;

        string Message = "开奖成功，总方案 {0} 个，撤单未满员或未出票方案 {1} 个，有效中奖方案 {2} 个。本期开奖还未全部完成, 请继续操作第三步。";

        if (isEndOpen)
        {
            BindDataForIsuse();
            btnGO_Step3.Enabled = false;
            Message = "开奖成功，总方案 {0} 个，撤单未满员或未出票方案 {1} 个，有效中奖方案 {2} 个。本期开奖已全部完成。";
        }

        DataTable dt = new DAL.Tables.T_Schemes().Open("", "IsuseID=" + Shove._Convert.StrToLong(ddlIsuse.SelectedValue, 0).ToString(), "");

        if (dt == null)
        {
            Shove._Web.JavaScript.Alert(this.Page, "开奖出现异常!请联系开发维护人员");
            return;
        }

        SchemeCount = dt.Rows.Count;

        DataRow[] dr = dt.Select("QuashStatus <> 0");

        QuashCount = dr.Length;

        dr = dt.Select("WinMoney > 0");

        WinCount = dr.Length;

        Shove._Web.Cache.ClearCache(DataCache.IsusesInfo + ddlLottery.SelectedValue);
        Shove._Web.JavaScript.Alert(this.Page, String.Format(Message, SchemeCount, QuashCount, WinCount));
    }

    private int P_Win(SqlConnection conn, ref DataSet ds, long IsuseID, string WinLotteryNumber, string OpenAffiche, long OpenOperatorID, bool isEndTheIsuse, ref int SchemeCount, ref int QuashCount, ref int WinCount, ref int WinNoBuyCount, ref bool isEndOpen, ref int ReturnValue, ref string ReturnDescription)
    {
        MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

        int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_Win", ref ds, ref Outputs,
            new MSSQL.Parameter("IsuseID", SqlDbType.BigInt, 0, ParameterDirection.Input, IsuseID),
            new MSSQL.Parameter("WinLotteryNumber", SqlDbType.VarChar, 0, ParameterDirection.Input, WinLotteryNumber),
            new MSSQL.Parameter("OpenAffiche", SqlDbType.VarChar, 0, ParameterDirection.Input, OpenAffiche),
            new MSSQL.Parameter("OpenOperatorID", SqlDbType.BigInt, 0, ParameterDirection.Input, OpenOperatorID),
            new MSSQL.Parameter("isEndTheIsuse", SqlDbType.Bit, 0, ParameterDirection.Input, isEndTheIsuse),
            new MSSQL.Parameter("SchemeCount", SqlDbType.Int, 4, ParameterDirection.Output, SchemeCount),
            new MSSQL.Parameter("QuashCount", SqlDbType.Int, 4, ParameterDirection.Output, QuashCount),
            new MSSQL.Parameter("WinCount", SqlDbType.Int, 4, ParameterDirection.Output, WinCount),
            new MSSQL.Parameter("WinNoBuyCount", SqlDbType.Int, 4, ParameterDirection.Output, WinNoBuyCount),
            new MSSQL.Parameter("isEndOpen", SqlDbType.Bit, 0, ParameterDirection.Output, isEndOpen),
            new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
            new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
            );

        try
        {
            SchemeCount = System.Convert.ToInt32(Outputs["SchemeCount"]);
        }
        catch { }

        try
        {
            QuashCount = System.Convert.ToInt32(Outputs["QuashCount"]);
        }
        catch { }

        try
        {
            WinCount = System.Convert.ToInt32(Outputs["WinCount"]);
        }
        catch { }

        try
        {
            WinNoBuyCount = System.Convert.ToInt32(Outputs["WinNoBuyCount"]);
        }
        catch { }

        try
        {
            isEndOpen = System.Convert.ToBoolean(Outputs["isEndOpen"]);
        }
        catch { }

        try
        {
            ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
        }
        catch { }

        try
        {
            ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
        }
        catch { }

        return CallResult;
    }
}
