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
using System.Data.OleDb;
using System.Net;
using System.IO;
using System.Xml;
using System.Text;

public partial class Admin_PrintOutput : AdminPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            BindDataForLottery();

            BindDataForIsuse();

            BindData();

            btnBuyAll.Visible = _User.Competences.IsOwnedCompetences(Competences.BuildCompetencesList(Competences.LotteryBuy));

            ddlType.Items.Add("金额");
            ddlType.Items.Add("时间");
            ddlType.Items.Add("过关方式");
            ddlType.Items.Add("全部");
        }
    }

    #region Web 窗体设计器生成的代码

    override protected void OnInit(EventArgs e)
    {
        RequestLoginPage = this.Request.Url.AbsoluteUri;

        RequestCompetences = Competences.BuildCompetencesList(Competences.LotteryBuy,Competences.QueryData);

        base.OnInit(e);
    }

    #endregion

    private void BindDataForLottery()
    {
        DataTable dt = new DAL.Tables.T_Lotteries().Open("[ID], [Name]", "[ID] in (" + (_Site.UseLotteryListRestrictions == "" ? "-1" : _Site.UseLotteryListRestrictions) + ")", "[Order]");

        if (dt == null)
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", "Admin_PrintOutput");

            return;
        }

        Shove.ControlExt.FillDropDownList(ddlLottery, dt, "Name", "ID");
    }

    private void BindDataForIsuse()
    {
        DataTable dt = new DAL.Tables.T_Isuses().Open("[ID], [Name]", "LotteryID = " + Shove._Web.Utility.FilteSqlInfusion(ddlLottery.SelectedValue) + " and StartTime < GetDate() and IsOpened = 0", "EndTime");

        if (dt == null)
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", "Admin_PrintOutput");

            return;
        }

        Shove.ControlExt.FillDropDownList(ddlIsuse, dt, "Name", "ID");

        g.Visible = (ddlIsuse.Items.Count > 0);
    }

    protected void ddlLottery_SelectedIndexChanged(object sender, System.EventArgs e)
    {
        ddlType.Visible = ddlLottery.SelectedValue == "72";

        BindDataForIsuse();

        BindData();
    }

    protected void ddlIsuse_SelectedIndexChanged(object sender, System.EventArgs e)
    {
        BindData();
    }

    protected void ddlType_SelectedIndexChanged(object sender, System.EventArgs e)
    {
        BindData();
    }

    private void BindData()
    {
        if (ddlIsuse.Items.Count < 1)
        {
            return;
        }

        string SQL = "";

        if (ddlType.Visible)
        {
            if (ddlType.SelectedValue == "金额")
            {
                SQL = "Money desc ";
            }
            else if (ddlType.SelectedValue == "过关方式")
            {
                SQL = "PlayTypeID desc ";
            }
            else if (ddlType.SelectedValue == "时间")
            {
                SQL = "SystemEndTime desc ";
            }
        }

        DataTable dt = new DAL.Views.V_SchemeSchedules().Open("", "IsuseID = " + Shove._Web.Utility.FilteSqlInfusion(ddlIsuse.SelectedValue) + " and Schedule >= 100 and QuashStatus = 0 and Buyed = 0 and isOpened = 0", "[Money] desc");

        if (dt == null)
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", "Admin_PrintOutput");

            return;
        }

        if (ddlLottery.SelectedValue == "72")
        {
            DataTable dtMatch = new DAL.Tables.T_PassRate().Open("", "", "");
            DataRow[] dr = null;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i]["Money"] = double.Parse(dt.Rows[i]["Money"].ToString()).ToString("N");

                if (dt.Rows[i]["LotteryNumber"].ToString().IndexOf("A0") >= 0)
                {
                    dt.Rows[i]["PlayTypeName"] = dt.Rows[i]["PlayTypeName"].ToString() + "**" + "单关";
                }
                else
                {
                    dt.Rows[i]["PlayTypeName"] = dt.Rows[i]["PlayTypeName"].ToString() + "**" + "过关";
                }

                string str = dt.Rows[i]["LotteryNumber"].ToString();

                if (string.IsNullOrEmpty(str))
                {
                    continue;
                }

                string Number = str.Split(';')[1].Replace("][", "|").Substring(1, str.Split(';')[1].Length - 2);
                string[] Numbers = Number.Split('|');
                string MatchID = "";

                for (int j = 0; j < Numbers.Length; j++)
                {
                    if (Numbers[j].IndexOf("(") < 0)
                    {
                        continue;
                    }

                    MatchID += Shove._Convert.StrToLong(Numbers[j].Substring(0, Numbers[j].IndexOf("(")), 1).ToString() + ",";
                }

                if (MatchID.EndsWith(","))
                {
                    MatchID = MatchID.Substring(0, MatchID.Length - 1);
                }

                dr = dtMatch.Select("MatchID in (" + MatchID + ")", "StopSellTime");

                if (dr.Length < 1)
                {
                    continue;
                }

                dt.Rows[i]["LotteryNumber"] = "查看详细信息";
                dt.Rows[i]["SystemEndTime"] = dr[0]["StopSellTime"].ToString();

                dt.AcceptChanges();
            }
        }

        DataTable dtNew = dt.Clone();

        DataRow[] drs = dt.Select("",SQL);
        foreach (DataRow dr in drs)
        {
            dtNew.ImportRow(dr);
        }

        g.DataSource = dtNew;
        g.DataBind();

        btnDownload.Enabled = (dt.Rows.Count > 0);
        btnDownload_txt.Enabled = (dt.Rows.Count > 0);

        btnBuyAll.Visible = (dt.Rows.Count > 0);

        fileUp.Disabled = (dt.Rows.Count < 1);
        btnUpload.Enabled = (dt.Rows.Count > 0);
    }

    protected void btnRefresh_Click(object sender, System.EventArgs e)
    {
        BindData();
    }

    protected void g_ItemCommand(object source, System.Web.UI.WebControls.DataListCommandEventArgs e)
    {
        if (e.CommandName == "btnBuy")
        {
            int ReturnValue = -1;
            string ReturnDescription = "";

            int Results = -1;
            Results = DAL.Procedures.P_SchemePrintOut(
                Shove._Convert.StrToLong(((HtmlInputHidden)e.Item.FindControl("tbSiteID")).Value, -1),
                Shove._Convert.StrToLong(((HtmlInputHidden)e.Item.FindControl("tbID")).Value, -1),
                _User.ID, PrintOutTypes.Local, "", true, ref ReturnValue, ref ReturnDescription);

            if (Results < 0)
            {
                PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", this.Page.GetType().BaseType.FullName);

                return;
            }

            if (ReturnValue < 0)
            {
                PF.GoError(ErrorNumber.Unknow, ReturnDescription, this.Page.GetType().BaseType.FullName);

                return;
            }

            BindData();

            return;
        }
    }

    protected void btnBuyAll_Click(object sender, EventArgs e)
    {
        if (g.Items.Count < 0)
        {
            Shove._Web.JavaScript.Alert(this.Page, "没有数据。");

            return;
        }

        int ReturnValue = -1;
        string ReturnDescription = "";

        int Fail = 0, OK = 0;

        foreach (DataListItem item in g.Items)
        {
            long SiteID = Shove._Convert.StrToLong(((HtmlInputHidden)item.FindControl("tbSiteID")).Value, -1);
            long _ID = Shove._Convert.StrToLong(((HtmlInputHidden)item.FindControl("tbID")).Value, -1);

            if ((SiteID < 1) || (_ID < 1))
            {
                Fail++;

                continue;
            }

            ReturnDescription = "";
            int Results = -1;
            Results = DAL.Procedures.P_SchemePrintOut(SiteID, _ID, _User.ID, PrintOutTypes.Local, "请电询", true, ref ReturnValue, ref ReturnDescription);

            if (Results < 0)
            {
                Fail++;

                continue;
            }


            if (ReturnValue < 0)
            {
                Fail++;

                continue;
            }

            OK++;
        }

        BindData();

        Shove._Web.JavaScript.Alert(this.Page, String.Format("操作完成，其中成功出票 {0} 个方案，失败 {1} 个方案。", OK, Fail));
    }

    protected void btnUpload_Click(object sender, System.EventArgs e)
    {
        string NewFileName = "";

        if (Shove._IO.File.UploadFile(this.Page, fileUp, "../Temp/", ref NewFileName, "execl") != 0)
        {
            Shove._Web.JavaScript.Alert(this.Page, "文件上传失败。");

            return;
        }

        string strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + this.Server.MapPath("../Temp/" + NewFileName) + ";Extended Properties=Excel 8.0;";
        OleDbConnection oleconn = new OleDbConnection(strConn);

        try
        {
            oleconn.Open();
        }
        catch
        {
            System.IO.File.Delete(this.Page.Server.MapPath("../Temp/" + NewFileName));

            Shove._Web.JavaScript.Alert(this.Page, "上传的文件读取错误。");

            return;
        }

        DataTable schemaTable = oleconn.GetOleDbSchemaTable(System.Data.OleDb.OleDbSchemaGuid.Tables, null);
        OleDbCommand Cmd = new OleDbCommand("select 站点编号 as SiteID, 内部编号 as [id], 彩票标识号 as LotteryCode from [" + schemaTable.Rows[0][2].ToString().Trim() + "]", oleconn);
        OleDbDataReader dr = null;

        try
        {
            dr = Cmd.ExecuteReader();
        }
        catch
        {
            oleconn.Close();
            System.IO.File.Delete(this.Page.Server.MapPath("../Temp/" + NewFileName));

            Shove._Web.JavaScript.Alert(this.Page, "上传的文件数据格式不正确。");

            return;
        }

        int OK = 0, Fail = 0;

        while (dr.Read())
        {
            long SiteID = Shove._Convert.StrToLong(dr["SiteID"].ToString(), -1);
            long _ID = Shove._Convert.StrToLong(dr["id"].ToString(), -1);
            string _LotteryCode = dr["LotteryCode"].ToString().Trim();

            if ((SiteID < 1) || (_ID < 1))
            {
                Fail++;

                continue;
            }

            if ((_LotteryCode == "") || (_LotteryCode == "<请输入标识号>"))
            {
                _LotteryCode = "请电询";
            }

            int ReturnValue = -1;
            string ReturnDescription = "";

            int Results = -1;
            Results = DAL.Procedures.P_SchemePrintOut(SiteID, _ID, _User.ID, PrintOutTypes.Local, _LotteryCode, true, ref ReturnValue, ref ReturnDescription);

            if (Results < 0)
            {
                Fail++;

                continue;
            }


            if (ReturnValue < 0)
            {
                Fail++;

                continue;
            }

            OK++;
        }

        dr.Close();
        oleconn.Close();

        BindData();

        System.IO.File.Delete(this.Page.Server.MapPath("../Temp/" + NewFileName));
        Shove._Web.JavaScript.Alert(this.Page, String.Format("操作完成，其中成功出票 {0} 个方案，失败 {1} 个方案。", OK, Fail));
    }

    protected void btnDownload_Click(object sender, EventArgs e)
    {
        DataTable dt = new DAL.Views.V_Isuses().Open("", "[id] = " + Shove._Web.Utility.FilteSqlInfusion(ddlIsuse.SelectedValue), "");

        if ((dt == null) || (dt.Rows.Count < 1))
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", this.Page.GetType().BaseType.FullName);

            return;
        }

        string FileName = dt.Rows[0]["Code"].ToString() + dt.Rows[0]["Name"].ToString() + ".xls";

        dt = new DAL.Views.V_SchemeSchedules().Open("", "IsuseID = " + Shove._Web.Utility.FilteSqlInfusion(ddlIsuse.SelectedValue) + " and Schedule >= 100 and QuashStatus = 0 and Buyed = 0", "[Money] desc");

        if (dt == null)
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", this.Page.GetType().BaseType.FullName);

            return;
        }

        if (dt.Rows.Count < 1)
        {
            Shove._Web.JavaScript.Alert(this.Page, "没有数据。");

            return;
        }

        HttpResponse response;

        response = Page.Response;
        response.AppendHeader("Content-Disposition", "attachment;filename=" + FileName);
        Response.ContentType = "application/ms-excel";
        response.ContentEncoding = System.Text.Encoding.GetEncoding("gb2312");
        response.Write("站点编号\t内部编号\t方案编号\t类别\t购买内容\t倍数\t金额\t彩票标识号\n");

        foreach (DataRow dr in dt.Rows)
        {
            string LotteryNumber = "";
            try
            {
                LotteryNumber = "| " + dr["LotteryNumber"].ToString().Replace("\r\n", " | ") + " |";
            }
            catch { }
            response.Write(dr["SiteID"].ToString() + "\t" + dr["ID"].ToString() + "\t" + dr["SchemeNumber"].ToString() + "\t" + dr["PlayTypeName"].ToString() + "\t" + LotteryNumber + "\t" + dr["Multiple"].ToString() + "\t" + dr["Money"].ToString() + "\t<请输入标识号>\n");
        }

        response.End();
    }

    protected void btnDownload_txt_Click(object sender, EventArgs e)
    {
        DataTable dt = new DAL.Views.V_Isuses().Open("", "[ID] = " + Shove._Web.Utility.FilteSqlInfusion(ddlIsuse.SelectedValue), "");

        if ((dt == null) || (dt.Rows.Count < 1))
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", this.Page.GetType().BaseType.FullName);

            return;
        }

        string FileName = dt.Rows[0]["Code"].ToString() + dt.Rows[0]["Name"].ToString() + ".txt";

        dt = new DAL.Views.V_SchemeSchedules().Open("", "IsuseID = " + Shove._Web.Utility.FilteSqlInfusion(ddlIsuse.SelectedValue) + " and Schedule >= 100 and QuashStatus = 0 and Buyed = 0", "[Money] desc");

        if (dt == null)
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", this.Page.GetType().BaseType.FullName);

            return;
        }

        if (dt.Rows.Count < 1)
        {
            Shove._Web.JavaScript.Alert(this.Page, "没有数据。");

            return;
        }

        HttpResponse response;

        response = Page.Response;
        response.AppendHeader("Content-Disposition", "attachment;filename=" + FileName);
        Response.ContentType = "application/ms-txt";
        response.ContentEncoding = System.Text.Encoding.GetEncoding("utf-8");

        foreach (DataRow dr in dt.Rows)
        {
            string LotteryNumber = dr["LotteryNumber"].ToString();

            response.Write("***********************************************\r\n");
            response.Write("站点编号: " + dr["SiteID"].ToString() + "\r\n");
            response.Write("方案编号: " + dr["SchemeNumber"].ToString() + "\t内部编号:" + dr["ID"].ToString() + "\r\n");
            response.Write("方案类别: " + dr["PlayTypeName"].ToString() + "\r\n");
            response.Write("方案倍数: " + dr["Multiple"].ToString() + "\t方案金额:" + double.Parse(dr["Money"].ToString()).ToString("N") + "\r\n");
            response.Write("***********************************************\r\n");
            response.Write(LotteryNumber);
            response.Write("\r\n\r\n\r\n");
        }

        response.End();
    }

    protected void btnOkoooDownload_txt_Click(object sender, EventArgs e)
    {
        DataTable dt = new DAL.Views.V_Isuses().Open("", "[ID] = " + Shove._Web.Utility.FilteSqlInfusion(ddlIsuse.SelectedValue), "");

        if ((dt == null) || (dt.Rows.Count < 1))
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", this.Page.GetType().BaseType.FullName);

            return;
        }

        string FileName = dt.Rows[0]["Code"].ToString() + dt.Rows[0]["Name"].ToString() + ".txt";

        dt = new DAL.Views.V_SchemeSchedules().Open("", "IsuseID = " + Shove._Web.Utility.FilteSqlInfusion(ddlIsuse.SelectedValue) + " and Schedule >= 100 and QuashStatus = 0 and Buyed = 0", "[Money] desc");

        if (dt == null)
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", this.Page.GetType().BaseType.FullName);

            return;
        }

        if (dt.Rows.Count < 1)
        {
            Shove._Web.JavaScript.Alert(this.Page, "没有数据。");

            return;
        }

        HttpResponse response;

        response = Page.Response;
        response.AppendHeader("Content-Disposition", "attachment;filename=" + FileName);
        Response.ContentType = "application/ms-txt";
        response.ContentEncoding = System.Text.Encoding.GetEncoding("utf-8");

        string CacheKey = "JCZC_Scheme_Bind";

        DataTable dtMatch = Shove._Web.Cache.GetCacheAsDataTable(CacheKey);

        if (dtMatch == null)
        {
            dtMatch = new DAL.Tables.T_Match().Open("ID, MatchNumber, StopSellingTime", "", "");

            if (dtMatch == null)
            {
                return;
            }

            if (dtMatch.Rows.Count < 1)
            {
                return;
            }

            Shove._Web.Cache.SetCache(CacheKey, dtMatch, 3600);
        }

        foreach (DataRow dr in dt.Rows)
        {
            string LotteryNumber = dr["LotteryNumber"].ToString();
            int PlayTypeID = Shove._Convert.StrToInt(dr["PlayTypeID"].ToString(), 0);

            ArrayList al = new ArrayList();

            string[] strs = LotteryNumber.Split('\n');
            string[] strNumbers = null;

            string CanonicalNumber = "";

            if (strs == null)
                return;
            if (strs.Length == 0)
                return;

            for (int i = 0; i < strs.Length; i++)
            {
                if (string.IsNullOrEmpty(strs[i]))
                {
                    continue;
                }

                strNumbers = new SLS.Lottery()[Shove._Convert.StrToInt(PlayTypeID.ToString().Substring(0, PlayTypeID.ToString().Length - 2), 72)].ToSingle(strs[i], ref CanonicalNumber, PlayTypeID);

                if (strNumbers == null)
                {
                    continue;
                }

                for (int j = 0; j < strNumbers.Length; j++)
                {
                    al.Add(strNumbers[j]);
                }
            }

            string[] LotteryNumbers = new string[al.Count];

            StringBuilder sbLotteryNumbers = new StringBuilder();

            for (int i = 0; i < al.Count; i++)
            {
                if (i == al.Count)
                {
                    sbLotteryNumbers.Append(al[i].ToString());
                }
                else
                {
                    sbLotteryNumbers.Append(al[i].ToString() + "\n");
                }

                LotteryNumbers[i] = al[i].ToString();
            }

            int Multiple = 0;
            string Number = "";
            string BuyWays = "";

            foreach (string str in LotteryNumbers)
            {
                if (string.IsNullOrEmpty(str))
                {
                    continue;
                }

                if (str.Split(';').Length < 3)
                {
                    continue;
                }

                try
                {
                    Multiple = Shove._Convert.StrToInt(str.Split(';')[2].Substring(1, str.Split(';')[2].Length - 2).Substring(2), 1);
                }
                catch
                { }

                Number = str.Split(';')[1].Substring(1, str.Split(';')[1].Length - 2);
                string[] Numbers = Number.Split('|');


                if (Numbers.Length == 1)
                {
                    BuyWays = "单关";
                }
                else
                {
                    BuyWays = Numbers.Length.ToString() + "串1";
                }

                long MatchID = 0;

                response.Write(dr["SchemeNumber"].ToString() + "," + PlayTypeName(PlayTypeID) + ",T,");

                for (int i = 0; i < Numbers.Length; i++)
                {
                    if (Numbers[i].IndexOf("(") < 0)
                    {
                        continue;
                    }

                    MatchID = Shove._Convert.StrToLong(Numbers[i].Substring(0, Numbers[i].IndexOf("(")), 1);

                    DataRow[] drMatch = dtMatch.Select("ID=" + MatchID.ToString());

                    if (drMatch.Length < 1)
                    {
                        continue;
                    }

                    response.Write("(" + GetMatchNumber(drMatch[0]["MatchNumber"].ToString()) + ">" + Getesult(PlayTypeID, Numbers[i].Substring(Numbers[i].IndexOf("(") + 1, Numbers[i].IndexOf(")") - Numbers[i].IndexOf("(") - 1)) + ")");

                }

                response.Write("," + BuyWays + "," + Multiple + "," + (2 * Multiple).ToString() + "\r\n");
            }
        }

        response.End();
    }

    public string Getesult(int PlayType, string num)
    {
        string res = string.Empty;

        switch (PlayType)
        {
            case 7201:
                res = Get7201(num);
                break;
            case 7202:
                res = Get7202(num);
                break;
            case 7203:
                res = Get7203(num);
                break;
            case 7204:
                res = Get7204(num);
                break;
            case 7301:
                res = Get7301(num);
                break;
            case 7302:
                res = Get7302(num);
                break;
            case 7303:
                res = Get7303(num);
                break;
            case 7304:
                res = Get7304(num);
                break;
        }

        return res;
    }

    #region 足彩

    /// <summary>
    /// 胜平负
    /// </summary>
    /// <param name="num"></param>
    /// <returns></returns>
    private string Get7201(string num)
    {
        string res = string.Empty;
        switch (num)
        {
            case "1": res = "3";
                break;
            case "2": res = "1";
                break;
            case "3": res = "0";
                break;
            default:
                res = "";
                break;
        }

        return res;
    }

    /// <summary>
    /// 比分
    /// </summary>
    /// <param name="num"></param>
    /// <returns></returns>
    private string Get7202(string num)
    {
        string res = string.Empty;
        switch (num)
        {
            case "1": res = "10";
                break;
            case "2": res = "20";
                break;
            case "3": res = "21";
                break;
            case "4": res = "30";
                break;
            case "5": res = "31";
                break;
            case "6": res = "32";
                break;
            case "7": res = "40";
                break;
            case "8": res = "41";
                break;
            case "9": res = "42";
                break;
            case "10": res = "50";
                break;
            case "11": res = "51";
                break;
            case "12": res = "52";
                break;
            case "13": res = "P0";
                break;
            case "14": res = "00";
                break;
            case "15": res = "11";
                break;
            case "16": res = "22";
                break;
            case "17": res = "33";
                break;
            case "18": res = "PP";
                break;
            case "19": res = "01";
                break;
            case "20": res = "02";
                break;
            case "21": res = "12";
                break;
            case "22": res = "03";
                break;
            case "23": res = "13";
                break;
            case "24": res = "23";
                break;
            case "25": res = "04";
                break;
            case "26": res = "14";
                break;
            case "27": res = "24";
                break;
            case "28": res = "05";
                break;
            case "29": res = "15";
                break;
            case "30": res = "25";
                break;
            case "31": res = "00";
                break;
            default:
                res = "";
                break;
        }

        return res;
    }

    /// <summary>
    /// 总进球
    /// </summary>
    /// <param name="num"></param>
    /// <returns></returns>
    private string Get7203(string num)
    {
        string res = string.Empty;
        switch (num)
        {
            case "1": res = "0";
                break;
            case "2": res = "1";
                break;
            case "3": res = "2";
                break;
            case "4": res = "3";
                break;
            case "5": res = "4";
                break;
            case "6": res = "5";
                break;
            case "7": res = "6";
                break;
            case "8": res = "7+";
                break;
            default:
                res = "";
                break;
        }

        return res;
    }

    /// <summary>
    /// 半全场胜平负
    /// </summary>
    /// <param name="num"></param>
    /// <returns></returns>
    private string Get7204(string num)
    {
        string res = string.Empty;
        switch (num)
        {
            case "1": res = "胜胜";
                break;
            case "2": res = "胜平";
                break;
            case "3": res = "胜负";
                break;
            case "4": res = "平胜";
                break;
            case "5": res = "平平";
                break;
            case "6": res = "平负";
                break;
            case "7": res = "负胜";
                break;
            case "8": res = "负平";
                break;
            case "9": res = "负负";
                break;
            default:
                res = "";
                break;
        }

        return res;
    }

    #endregion 足彩

    #region 篮彩

    /// <summary>
    /// 胜负
    /// </summary>
    /// <param name="num"></param>
    /// <returns></returns>
    private static string Get7301(string num)
    {
        string res = string.Empty;
        switch (num)
        {
            case "1": res = "主负";
                break;
            case "2": res = "主胜";
                break;

            default:
                res = "";
                break;
        }

        return res;
    }

    /// <summary>
    /// 让分胜负
    /// </summary>
    /// <param name="num"></param>
    /// <returns></returns>
    private static string Get7302(string num)
    {
        string res = string.Empty;
        switch (num)
        {
            case "1": res = "让分主负";
                break;
            case "2": res = "让分主胜";
                break;

            default:
                res = "";
                break;
        }

        return res;
    }

    /// <summary>
    /// 胜分差
    /// </summary>
    /// <param name="num"></param>
    /// <returns></returns>
    private static string Get7303(string num)
    {
        string res = string.Empty;
        switch (num)
        {
            case "1": res = "客胜 1-5";
                break;
            case "2": res = "主胜 1-5";
                break;
            case "3": res = "客胜 6-10";
                break;
            case "4": res = "主胜 6-10";
                break;
            case "5": res = "客胜 11-15";
                break;
            case "6": res = "主胜 11-15";
                break;
            case "7": res = "客胜 16-20";
                break;
            case "8": res = "主胜 16-20";
                break;
            case "9": res = "客胜 21-25";
                break;
            case "10": res = "主胜 21-25";
                break;
            case "11": res = "客胜 26+";
                break;
            case "12": res = "主胜 26+";
                break;
            default:
                res = "";
                break;
        }

        return res;
    }

    /// <summary>
    /// 大小分
    /// </summary>
    /// <param name="num"></param>
    /// <returns></returns>
    private static string Get7304(string num)
    {
        string res = string.Empty;
        switch (num)
        {
            case "1": res = "大";
                break;
            case "2": res = "小";
                break;

            default:
                res = "";
                break;
        }

        return res;
    }

    #endregion 篮彩

    private string PlayTypeName(int PlayTypeID)
    {
        switch (PlayTypeID)
        {
            case 7201:
                return "竞彩足球让分胜平负";
            case 7202:
                return "竞彩足球总进球";
            case 7203:
                return "竞彩足球比分";
            case 7204:
                return "竞彩足球半全场";
            case 7301:
                return "竞彩篮球胜负";
            case 7302:
                return "竞彩篮球让分胜负";
            case 7303:
                return "竞彩篮球胜分差";
            case 7304:
                return "竞彩篮球大小分";
            default :
                return "";
        }
    }

    private string GetMatchNumber(string MatchNumber)
    {
        string TempMatchNumber = MatchNumber.Substring(2);

        switch (MatchNumber.Substring(0, 2))
        {
            case "周一":
                return "1" + TempMatchNumber;
            case "周二":
                return "2" + TempMatchNumber;
            case "周三":
                return "3" + TempMatchNumber;
            case "周四":
                return "4" + TempMatchNumber;
            case "周五":
                return "5" + TempMatchNumber;
            case "周六":
                return "6" + TempMatchNumber;
            case "周日":
                return "7" + TempMatchNumber;
            default:
                return "";
        }
    }

    private static string Post(string Url, string RequestString, int TimeoutSeconds)
    {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);

        if (TimeoutSeconds > 0)
        {
            request.Timeout = 1000 * TimeoutSeconds;
        }
        request.Method = "POST";
        request.AllowAutoRedirect = true;

        byte[] data = System.Text.Encoding.GetEncoding("GBK").GetBytes(RequestString);

        request.ContentType = "application/x-www-form-urlencoded";
        request.Accept = "";
        Stream outstream = request.GetRequestStream();

        outstream.Write(data, 0, data.Length);
        outstream.Close();

        HttpWebResponse hwrp = (HttpWebResponse)request.GetResponse();
        string ContentType = hwrp.Headers.Get("Content-Type");

        StreamReader sr = null;

        sr = new StreamReader(hwrp.GetResponseStream(), System.Text.Encoding.GetEncoding("GBK"));

        return sr.ReadToEnd();
    }
}
