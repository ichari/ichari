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

public partial class Home_Room_SchemeAll : RoomPageBase
{

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            HidIsuseID.Value = Shove._Web.Utility.GetRequest("IsuseID");
            HidLotteryID.Value = Shove._Web.Utility.GetRequest("LotteryID");
            HidNumber.Value = Shove._Web.Utility.GetRequest("Number");

            BindData();
        }
    }

    #region Web 窗体设计器生成的代码

    override protected void OnInit(EventArgs e)
    {
        RequestLoginPage = this.Request.Url.AbsoluteUri;

        isRequestLogin = false;
        isAtFramePageLogin = false;

        isAllowPageCache = true;
        PageCacheSeconds = 60;

        base.OnInit(e);
    }

    #endregion

    private void BindData()
    {
        long LotteryID = Shove._Convert.StrToLong(HidLotteryID.Value, 0);
        long IsuseID = Shove._Convert.StrToLong(Shove._Web.Utility.FilteSqlInfusion(HidIsuseID.Value), 0);
        string Search = HidSearch.Value;
        string Sort = HidSort.Value;
        string Filter = HidFilter.Value;
        int pageNumber = Shove._Convert.StrToInt(HidPageNumber.Value, 1);

        HidSortID.Value = HidSortID.Value;

        //本期全部方案
        string CacheKey = "Home_Room_SchemeAll_BindData" + IsuseID.ToString();
        DataTable dt = Shove._Web.Cache.GetCacheAsDataTable(CacheKey);
        if (dt == null)
        {
            StringBuilder sql = new StringBuilder();

            sql.AppendLine("select a.ID,b.Name as InitiateName,AtTopStatus,b.Level,Money, c.Name as PlayTypeName, Share, BuyedShare, Schedule, AssureMoney, ")
                   .AppendLine("	InitiateUserID, QuashStatus, PlayTypeID, Buyed, SecrecyLevel, EndTime, d.IsOpened, LotteryNumber,case Schedule when 100 then 1 else 0 end as IsFull ")
                   .AppendLine("from")
                   .AppendLine("	(")
                   .AppendLine("		select ID, EndTime,IsOpened from T_Isuses where ID = @ID ")
                   .AppendLine("	) as d")
                   .AppendLine("inner join T_Schemes a on a.IsuseID = d.ID")
                   .AppendLine("inner join T_Users b on a.InitiateUserID = b.ID")
                   .AppendLine("inner join T_PlayTypes c on a.PlayTypeID = c.ID")
                   .AppendLine("order by a.QuashStatus asc,IsFull asc, a.AtTopStatus desc, a.[Money] desc");

            dt = MSSQL.Select(sql.ToString(), new MSSQL.Parameter("ID", SqlDbType.BigInt, 0, ParameterDirection.Input, IsuseID.ToString()));

            if (dt == null)
            {
                PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", this.GetType().FullName);
                return;
            }
            
            DataTable dtR = new DataTable();
            DataColumn[] dc = new DataColumn[dt.Columns.Count];
            for (int i = 0; i < dc.Length; i++) {
                dc[i] = new DataColumn(dt.Columns[i].ColumnName, typeof(System.String));
            }

            dtR.Columns.AddRange(dc);
            dtR.Columns["Money"].DataType = Type.GetType("System.Double");
            dtR.Columns["Share"].DataType = Type.GetType("System.Int32");
            dtR.Columns["Schedule"].DataType = Type.GetType("System.Double");

            foreach (DataRow dr in dt.Rows) {
                dtR.Rows.Add(dr.ItemArray);
            }

            dtR.Columns.Add("TmpID", Type.GetType("System.Int32"));
            dtR.Columns.Add("EachMoney", typeof(System.String));
            dtR.Columns.Add("Color", typeof(System.String));
            dtR.Columns.Add("State", typeof(System.String));
            dtR.Columns.Add("Join", typeof(System.String));
            dtR.Columns.Add("Assure", typeof(System.String));
            dtR.Columns.Add("Initiater", typeof(System.String));

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dtR.Rows[i]["EachMoney"] = (Shove._Convert.StrToDouble(dt.Rows[i]["Money"].ToString(), 0) / Shove._Convert.StrToDouble(dt.Rows[i]["Share"].ToString(), 0)).ToString("N");
                dtR.Rows[i]["Money"] = Shove._Convert.StrToDouble(dt.Rows[i]["Money"].ToString(), 0).ToString("N");
                dtR.Rows[i]["Color"] = i % 2 > 0 ? "#F7FEFA" : "White";
               
                string InitiateName = dt.Rows[i]["InitiateName"].ToString();
                if (InitiateName.Length > 10) {
                    InitiateName = InitiateName.Substring(0, 10);
                }

                double AssureMoney = Shove._Convert.StrToDouble(dt.Rows[i]["AssureMoney"].ToString(), 0);
                if (AssureMoney > 0) {
                    dtR.Rows[i]["Assure"] = "<Font color=\'red\'>" + dt.Rows[i]["Schedule"].ToString() + "%<br />+" + ((AssureMoney / Shove._Convert.StrToDouble(dt.Rows[i]["Money"].ToString(), 0)) * 100).ToString("N") + "%(保)</ Font>";
                }
                else {
                    dtR.Rows[i]["Assure"] = "<Font color=\'red\'>" + dt.Rows[i]["Schedule"].ToString() + "%</ Font><br />";
                }

                int Level = Shove._Convert.StrToInt(dt.Rows[i]["Level"].ToString(), 0);
                if (Level > 5) {
                    Level = 5;
                }
                dtR.Rows[i]["Initiater"] = dt.Rows[i]["InitiateName"].ToString();
                dtR.Rows[i]["InitiateName"] = "<a style='cursor:hand' href='../Web/Score.aspx?id=" + dt.Rows[i]["InitiateUserID"].ToString() + "&LotteryID=" + HidLotteryID.Value + "' title='点击查看历史战绩' target='_blank'>" + InitiateName + "</a>";
                if (Level > 0) {
                    dtR.Rows[i]["Level"] = "<a style='cursor:hand' href='../Web/Score.aspx?id=" + dt.Rows[i]["InitiateUserID"].ToString() + "&LotteryID=" + HidLotteryID.Value + "' title='点击查看历史战绩' target='_blank'> <div style='background-image:url(Images/gold.gif); width:" + (9 * Level).ToString() + "px;background-repeat:repeat-x;'>&nbsp;</div></a>";
                }
                else {
                    dtR.Rows[i]["Level"] = "&nbsp;";
                }

                string ID = dtR.Rows[i]["ID"].ToString();
                short QuashStatus = Shove._Convert.StrToShort(dtR.Rows[i]["QuashStatus"].ToString(), 0);
                bool Buyed = Shove._Convert.StrToBool(dtR.Rows[i]["Buyed"].ToString(), false);
                bool Stop = (Shove._Convert.StrToDateTime(dtR.Rows[i]["EndTime"].ToString(), System.DateTime.Now.ToString()) <= System.DateTime.Now);
                
                if (QuashStatus != 0) {
                    if (QuashStatus == 2) {
                        dtR.Rows[i]["State"] = "<font color='blue'>撤单</font>";
                    }
                    else {
                        dtR.Rows[i]["State"] = "撤单";
                    }
                    dtR.Rows[i]["Join"] = "<Font color=\'#000000\'>--</font>";
                }
                else {
                    if (Buyed) {
                        dtR.Rows[i]["State"] = "<Font color=\'#FF0065\'>已成功</font>";
                        dtR.Rows[i]["Join"] = "<Font color=\'#000000\'>--</font>";
                    }
                    else {
                        if (dtR.Rows[i]["Schedule"].ToString() == "100") {
                            dtR.Rows[i]["State"] = "<Font color=\'#FF0065\'>满员</font>";
                            dtR.Rows[i]["Join"] = "<Font color=\'#000000\'>--</font>";
                        }
                        else {
                            // 历史期
                            if (Stop) {
                                dtR.Rows[i]["State"] = "未成功";
                                dtR.Rows[i]["Join"] = "<Font color=\'#000000\'>--</font>";
                            }
                            else {
                                dtR.Rows[i]["State"] = "未满";
                                dtR.Rows[i]["Join"] = "<a href='Scheme.aspx?id=" + ID + "' target='_blank' title='点击查看方案详细信息'><font color=\"#FF0065\">入伙</font></a>";
                            }
                        }
                    }
                }
                string LotteryNumber = dt.Rows[i]["LotteryNumber"].ToString().Trim();
                //short SecrecyLevel = Shove._Convert.StrToShort(dt.Rows[i]["SecrecyLevel"].ToString(), 0);
                //long InitiateUserID = Shove._Convert.StrToLong(dt.Rows[i]["InitiateUserID"].ToString(), 0);
                //bool Stop1 = (Shove._Convert.StrToDateTime(dt.Rows[i]["EndTime"].ToString(), System.DateTime.Now.ToString()) <= System.DateTime.Now);
                //bool IsOpened = Shove._Convert.StrToBool(dt.Rows[i]["IsOpened"].ToString(), false);
                //  SecrecyLevel 0 不保密 1 到截止 2 到开奖 3 永远
                //if ((SecrecyLevel == SchemeSecrecyLevels.ToDeadline) && !Stop && ((_User == null) || ((_User != null) && (InitiateUserID != _User.ID) && (!_User.isOwnedViewSchemeCompetence()))))
                //{
                //    dt.Rows[i]["LotteryNumber"] = "已保密，截止后公开";
                //}
                //else if ((SecrecyLevel == SchemeSecrecyLevels.ToOpen) && !IsOpened && ((_User == null) || ((_User != null) && (InitiateUserID != _User.ID) && (!_User.isOwnedViewSchemeCompetence()))))
                //{
                //    dt.Rows[i]["LotteryNumber"] = "已保密，开奖后公开";
                //}
                //else if ((SecrecyLevel == SchemeSecrecyLevels.Always) && ((_User == null) || (!_User.isOwnedViewSchemeCompetence() && (InitiateUserID != _User.ID))))
                //{
                //    dt.Rows[i]["LotteryNumber"] = "已保密";
                //}
                //else
                //{
                    //if (LotteryNumber.Length > 28)
                    //{
                    //    dt.Rows[i]["LotteryNumber"] = "<a href='Scheme.aspx?id=" + dt.Rows[i]["ID"].ToString() + "' target='_blank' title='点击查看方案详细信息'><font color=\"#FF0065\">投注内容</font></a>";
                    //}
                    //else
                    //{
                    if ((LotteryID == 1 || LotteryID == 2 || LotteryID == 15) && string.IsNullOrEmpty(LotteryNumber))
                    {
                        dtR.Rows[i]["LotteryNumber"] = "<a href='Scheme.aspx?id=" + dt.Rows[i]["ID"].ToString() + "' target='_blank'><font color=\"#FF0065\">未上传方案</font></a>";
                    }
                    else
                    {
                        dtR.Rows[i]["LotteryNumber"] = "<a href='Scheme.aspx?id=" + dt.Rows[i]["ID"].ToString() + "' target='_blank' title='点击查看方案详细信息'><font color=\"#FF0065\">投注内容</font></a>";
                        //dt.Rows[i]["LotteryNumber"] = "<a href='Scheme.aspx?id=" + dt.Rows[i]["ID"].ToString() + "' target='_blank' title='点击查看方案详细信息'>" + LotteryNumber + "</a>";
                    }
                    //}
                //}
            }
            dt = dtR;
            Shove._Web.Cache.SetCache(CacheKey, dt, ((HidLotteryID.Value == "29" || HidLotteryID.Value == "62") ? 60 : 600));
        }
        
        DataRow[] drTemp = null;

        if (Search != "" && Search != "输入用户名")
        {
            drTemp = dt.Select("Initiater like '%" + Shove._Web.Utility.FilteSqlInfusion(Search) + "%'");
        }

        switch (Filter)
        {
            case "1":
                drTemp = dt.Select("Convert([Money],System.Double) >= 1000");
                break;
            case "2":
                drTemp = dt.Select("Convert([Money],System.Double) < 1000");
                break;
            case "3":
                drTemp = dt.Select("Convert(Share,System.Int32) = BuyedShare");
                break;
            case "4":
                drTemp = dt.Select("Convert(Share,System.Int32) <> BuyedShare");
                break;
            case "5":
                drTemp = dt.Select("Convert(QuashStatus,System.Int32) <> 0");
                break;
            case "6":
                drTemp = dt.Select("Convert(AssureMoney,System.Double) > 0");
                break;
            case "7":
                drTemp = dt.Select(" 1 = 1 "); 
                break;
        }

        if (Sort != "")
        {
            drTemp = dt.Select("", Sort + " " + HidOrder.Value);
        }

        if (drTemp != null)
        {
            dt = dt.Clone();

            foreach (DataRow dr in drTemp)
            {
                dt.Rows.Add(dr.ItemArray);
            }
        }

        int c = dt.Rows.Count;

        for (int r = 0; r < c; r++)
        {
            dt.Rows[r]["TmpID"] = r + 1;
        }

        int perPageRowCount = 20;
        int rowCount = dt.Rows.Count;
        int pageCount = rowCount % perPageRowCount == 0 ? rowCount / perPageRowCount : rowCount / perPageRowCount + 1;

        if (pageNumber > pageCount)
        {
            pageNumber = pageCount;
        }


        int showPageCount = 10;
        int pageFirstID = pageNumber % showPageCount == 0 ? showPageCount * (pageNumber / showPageCount - 1) + 1 : showPageCount * (pageNumber / showPageCount) + 1;
        int prePageID = (pageFirstID / showPageCount + 1) == 1 ? 1 : showPageCount * (pageFirstID / showPageCount - 1) + 1;
        int nextPageID = pageCount > showPageCount * (pageNumber / showPageCount + 2) ? showPageCount * (pageNumber / showPageCount + 1) + 1 : pageCount;

        if (pageFirstID + showPageCount > pageCount)
        {
            pageFirstID = pageCount - showPageCount + 1;
        }

        if (pageFirstID <= 0)
        {
            pageFirstID = 1;
        }

        DataRow[] drShow = dt.Select("TmpID > " + (perPageRowCount * (pageNumber - 1)).ToString() + " and TmpID < " + (perPageRowCount * pageNumber + 1).ToString());

        DataTable dtShow = dt.Clone();

        foreach (DataRow dr in drShow)
        {
            dtShow.Rows.Add(dr.ItemArray);
        }

        rptSchemes.DataSource = dtShow;
        rptSchemes.DataBind();

        StringBuilder sb = new StringBuilder();

        sb.Append("<tr>")
             .Append("<td width='31%' height='36' align='left' class='black12'>")
             .Append("第<span class='red'>").Append(pageNumber.ToString()).Append("/").Append(pageCount.ToString()).Append("</span>页 <span class='red'>").Append(perPageRowCount.ToString()).Append("</span>条/页 共<span class='red'>").Append(rowCount.ToString()).Append("</span>条")
             .Append("</td>")
             .Append("<td width='69%' align='right'>")
             .Append("<table border='0' cellspacing='4' cellpadding='0'>")
             .Append("<tbody style='text-align:center; width:20px;'>")
             .Append("<tr>")
             .Append("<td valign='middle' class='ball' onclick='showPage(1);'>")
             .Append("<img src='images/page_first.gif' width='9' height='8' />")
             .Append("</td>")
             .Append("<td class='ball' onclick='showPage(").Append(prePageID.ToString()).Append(");'>")
             .Append("<img src='images/page_previous.gif' width='9' height='8' />");


        for (int p = pageFirstID; p < pageFirstID + showPageCount && p <= pageCount; p++)
        {
            sb.Append("</td>")
                .Append("<td id='page_").Append(p.ToString()).Append("' class='ball").Append(pageNumber == p ? "_r" : p <= pageCount ? "" : "_c").Append("'");

            if (p <= pageCount)
            {
                sb.Append(" onclick='showPage(").Append(p.ToString()).Append(");'");
            }

            sb.Append(">")
                .Append(p.ToString())
                .Append("</td>");
        }

        sb.Append("<td class='ball' onclick='showPage(").Append(nextPageID.ToString()).Append(");'>")
        .Append("<img src='images/page_3.gif' width='9' height='8' />")
        .Append("</td>")
        .Append("<td class='ball' onclick='showPage(").Append(pageCount.ToString()).Append(");'>")
        .Append("<img src='images/page_4.gif' width='9' height='8' />")
        .Append("</td>")
        .Append("<td >")
        .Append("<input type='text' class='ball_50' id='txtgopage' maxlength='").Append(pageCount.ToString().Length.ToString()).Append("' />")
        .Append("</td>")
        .Append("<td style='width:25px; height=5; font-family:tahoma;font-weight:bold; color:#FFFFFF; cursor:hand; background:#6B96CB;font-size: 13px;' onclick=\"showPage(document.getElementById('txtgopage').value);\">")
        .Append("GO")
        .Append("</td>")
        .Append("</tr>")
        .Append("</tbody>")
        .Append("</table>")
        .Append("</td>")
        .Append("</tr>");

        tbPaging.InnerHtml = sb.ToString();
    }

    protected void btnType_1_Click(object sender, System.EventArgs e)
    {
        HidFilter.Value = ((LinkButton)sender).ID.Substring(8, 1);
        HidOrder.Value = "";
        HidSort.Value = "";
        HidPageNumber.Value = "1";

        BindData();
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        HidSearch.Value = Shove._Web.Utility.FilteSqlInfusion(TxtName.Text.Trim());
        HidSort.Value = "";
        HidPageNumber.Value = "1";
        HidFilter.Value = "";

        BindData();
    }

    protected void btnPaging_Click(object sender, EventArgs e)
    {
        BindData();
    }

    protected void btnSorting_Click(object sender, EventArgs e)
    {
        if (HidOrder.Value == "")
        {
            HidOrder.Value = "asc";
        }
        else
        {
            if (HidOrder.Value == "asc")
            {
                HidOrder.Value = "desc";
            }
            else
            {
                HidOrder.Value = "asc";
            }
        }

        BindData();
    }
}
