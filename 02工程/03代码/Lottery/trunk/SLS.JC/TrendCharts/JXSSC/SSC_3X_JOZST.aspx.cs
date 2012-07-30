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
using System.Text;

public partial class Home_TrendCharts_JXSSC_SSC_3X_JOZST : TrendChartPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            GridViewBind();
            ColorBind();
            GridView1.Style["border-collapse"] = "";
            BindDataForAD();
        }
    }

    private void BindDataForAD()
    {
        lbAd.Text = "&nbsp;";
        string CacheKey = "Advertisements";
        DataTable dt = Shove._Web.Cache.GetCacheAsDataTable(CacheKey);

        if (dt == null)
        {
            dt = new DAL.Tables.T_Advertisements().Open("", "isShow=1", "");

            if (dt == null || dt.Rows.Count < 1)
            {
                return;
            }

            Shove._Web.Cache.SetCache(CacheKey, dt, 600);
        }

        DataRow[] drAd = dt.Select("LotteryID=61 and [Name] = '广告一'", "[Order]");
        DataRow[] drAd1 = dt.Select("LotteryID=61 and [Name] = '广告二'", "[Order]");
        DataRow[] drAd2 = dt.Select("LotteryID=61 and [Name] = '广告三'", "[Order]");

        if (drAd.Length < 1)
        {
            return;
        }

        StringBuilder sb = new StringBuilder();

        sb.AppendLine("<div id='icefable1'>")
            .AppendLine("<table width='200' border='0' cellpadding='0' cellspacing='0'>")
            .AppendLine("<tbody style='height: 20px;'>");

        foreach (DataRow dr in drAd)
        {
            string[] title = dr["Title"].ToString().Split(new String[] { "Color" }, StringSplitOptions.None);
            sb.Append("<tr><td class='blue'><a style='color:").Append(title.Length == 2 ? title[1] : "#000000").Append(";' href=\"").Append(dr["Url"].ToString().ToLower()).Append("\" target='_blank'>").Append(title[0]).AppendLine("</a></td></tr>");
        }

        sb.AppendLine("</tbody>")
            .AppendLine("</table>")
            .AppendLine("</div>")
            .AppendLine("<script type='text/jscript' language='javascript'>")
            .AppendLine("marqueesHeight=20;")
            .AppendLine("stopscroll=false;")
            .AppendLine("with(icefable1){")
            .AppendLine("style.height=marqueesHeight;")
            .AppendLine("style.overflowX='visible';")
            .AppendLine("style.overflowY='hidden';")
            .AppendLine("noWrap=true;")
            .AppendLine("onmouseover=new Function('stopscroll=true');")
            .AppendLine("onmouseout=new Function('stopscroll=false');")
            .AppendLine("}")
            .AppendLine("preTop=0; currentTop=marqueesHeight; stoptime=0;")
            .AppendLine("icefable1.innerHTML+=icefable1.innerHTML;")
            .AppendLine("")
            .AppendLine("function init_srolltext(){")
            .AppendLine("icefable1.scrollTop=0;")
            .AppendLine("scrollUpInterval = setInterval('scrollUp()',1);")
            .AppendLine("}")
            .AppendLine("")
            .AppendLine("function scrollUp(){")
            .AppendLine("if(stopscroll==true) return;")
            .AppendLine("currentTop+=1;")
            .AppendLine("if(currentTop==marqueesHeight+1)")
            .AppendLine("{")
            .AppendLine("stoptime+=1;")
            .AppendLine("currentTop-=1;")
            .AppendLine("if(stoptime==300) ")
            .AppendLine("{")
            .AppendLine("currentTop=0;")
            .AppendLine("stoptime=0;  		")
            .AppendLine("}")
            .AppendLine("}")
            .AppendLine("else {  	")
            .AppendLine("preTop=icefable1.scrollTop;")
            .AppendLine("icefable1.scrollTop+=1;")
            .AppendLine("if(preTop==icefable1.scrollTop){")
            .AppendLine("icefable1.scrollTop=marqueesHeight;")
            .AppendLine("icefable1.scrollTop+=1;")
            .AppendLine("}")
            .AppendLine("}")
            .AppendLine("}")
            .AppendLine("init_srolltext();");

        if (drAd.Length == 1)
        {
            sb.AppendLine("clearInterval(scrollUpInterval);");
        }

        sb.AppendLine("</script>");

        lbAd.Text = sb.ToString();

        sb = new StringBuilder();

        if (drAd1.Length > 0)
        {
            sb.AppendLine("<div id='icefable2'>")
            .AppendLine("<table width='100%' border='0' cellpadding='0' cellspacing='0'>")
            .AppendLine("<tbody style='height: 20px;'>");

            foreach (DataRow dr in drAd1)
            {
                string[] title = dr["Title"].ToString().Split(new String[] { "Color" }, StringSplitOptions.None);
                sb.Append("<tr><td class='blue'><a style='color:").Append(title.Length == 2 ? title[1] : "#000000").Append(";' href=\"").Append(dr["Url"].ToString().ToLower()).Append("\" target='_blank'>").Append(title[0]).AppendLine("</a></td></tr>");
            }

            sb.AppendLine("</tbody>")
                .AppendLine("</table>")
                .AppendLine("</div>")
                .AppendLine("<script type='text/jscript' language='javascript'>")
                .AppendLine("marqueesHeight2=20;")
                .AppendLine("stopscroll2=false;")
                .AppendLine("with(icefable2){")
                .AppendLine("style.height=marqueesHeight2;")
                .AppendLine("style.overflowX='visible';")
                .AppendLine("style.overflowY='hidden';")
                .AppendLine("noWrap=true;")
                .AppendLine("onmouseover=new Function('stopscroll=true');")
                .AppendLine("onmouseout=new Function('stopscroll=false');")
                .AppendLine("}")
                .AppendLine("preTop2=0; currentTop2=marqueesHeight2; stoptime2=0;")
                .AppendLine("icefable2.innerHTML+=icefable2.innerHTML;")
                .AppendLine("")
                .AppendLine("function init_srolltext2(){")
                .AppendLine("icefable2.scrollTop=0;")
                .AppendLine("scrollUpInterval2 = setInterval('scrollUp1()',1);")
                .AppendLine("}")
                .AppendLine("")
                .AppendLine("function scrollUp1(){")
                .AppendLine("if(stopscroll2==true) return;")
                .AppendLine("currentTop2+=1;")
                .AppendLine("if(currentTop2==marqueesHeight2+1)")
                .AppendLine("{")
                .AppendLine("stoptime2+=1;")
                .AppendLine("currentTop2-=1;")
                .AppendLine("if(stoptime2==300) ")
                .AppendLine("{")
                .AppendLine("currentTop2=0;")
                .AppendLine("stoptime2=0;  		")
                .AppendLine("}")
                .AppendLine("}")
                .AppendLine("else {  	")
                .AppendLine("preTop2=icefable2.scrollTop;")
                .AppendLine("icefable2.scrollTop+=1;")
                .AppendLine("if(preTop==icefable2.scrollTop){")
                .AppendLine("icefable2.scrollTop=marqueesHeight2;")
                .AppendLine("icefable2.scrollTop+=1;")
                .AppendLine("}")
                .AppendLine("}")
                .AppendLine("}")
                .AppendLine("init_srolltext2();");

            if (drAd1.Length == 1)
            {
                sb.AppendLine("clearInterval(scrollUpInterval2);");
            }

            sb.AppendLine("</script>");
        }

        lbAd1.Text = sb.ToString();

        sb = new StringBuilder();

        if (drAd2.Length > 0)
        {
            sb.AppendLine("<div id='icefable3'>")
                .AppendLine("<table width='100%' border='0' cellpadding='0' cellspacing='0'>")
                .AppendLine("<tbody style='height: 20px;'>");

            foreach (DataRow dr in drAd2)
            {
                string[] title = dr["Title"].ToString().Split(new String[] { "Color" }, StringSplitOptions.None);
                sb.Append("<tr><td class='blue'><a style='color:").Append(title.Length == 2 ? title[1] : "#000000").Append(";' href=\"").Append(dr["Url"].ToString().ToLower()).Append("\" target='_blank'>").Append(title[0]).AppendLine("</a></td></tr>");
            }

            sb.AppendLine("</tbody>")
                .AppendLine("</table>")
                .AppendLine("</div>")
                .AppendLine("")
                .AppendLine("<script type='text/jscript' language='javascript'>")
                .AppendLine("marqueesHeight3=20;")
                .AppendLine("stopscroll3=false;")
                .AppendLine("with(icefable3){")
                .AppendLine("style.height=marqueesHeight3;")
                .AppendLine("style.overflowX='visible';")
                .AppendLine("style.overflowY='hidden';")
                .AppendLine("noWrap=true;")
                .AppendLine("onmouseover=new Function('stopscroll=true');")
                .AppendLine("onmouseout=new Function('stopscroll=false');")
                .AppendLine("}")
                .AppendLine("preTop3=0; currentTop3=marqueesHeight; stoptime3=0;")
                .AppendLine("icefable3.innerHTML+=icefable3.innerHTML;")
                .AppendLine("")
                .AppendLine("function init_srolltext3(){")
                .AppendLine("icefable3.scrollTop=0;")
                .AppendLine("scrollUpInterval3 = setInterval('scrollUp3()',1);")
                .AppendLine("}")
                .AppendLine("")
                .AppendLine("function scrollUp3(){")
                .AppendLine("if(stopscroll3==true) return;")
                .AppendLine("currentTop3+=1;")
                .AppendLine("if(currentTop3==marqueesHeight3+1)")
                .AppendLine("{")
                .AppendLine("stoptime3+=1;")
                .AppendLine("currentTop3-=1;")
                .AppendLine("if(stoptime3==300) ")
                .AppendLine("{")
                .AppendLine("currentTop3=0;")
                .AppendLine("stoptime3=0;  		")
                .AppendLine("}")
                .AppendLine("}")
                .AppendLine("else {  	")
                .AppendLine("preTop3=icefable3.scrollTop;")
                .AppendLine("icefable3.scrollTop+=1;")
                .AppendLine("if(preTop3==icefable3.scrollTop){")
                .AppendLine("icefable3.scrollTop=marqueesHeight;")
                .AppendLine("icefable3.scrollTop+=1;")
                .AppendLine("}")
                .AppendLine("}")
                .AppendLine("}")
                .AppendLine("init_srolltext3();");

            if (drAd2.Length == 1)
            {
                sb.AppendLine("clearInterval(scrollUpInterval3);");
            }

            sb.AppendLine("</script>");
        }

        lbAd2.Text = sb.ToString();
    }

    protected void GridViewBind()
    {
        string result = "";

        DataTable dt = new DataTable();
        dt = new TrendChart().SSC_3X_JOZST_Select(30, "", "", ref result); //，默认显示30期

        if (dt == null || dt.Rows.Count < 1)
        {

        }
        else
        {
            int k = dt.Rows.Count;
            tb1.Text = dt.Rows[0]["Isuse"].ToString();
            tb2.Text = dt.Rows[k - 1]["Isuse"].ToString();
        }

        GridView1.DataSource = dt;
        GridView1.DataBind();
    }

    protected void btnTop30_Click(object sender, EventArgs e)
    {
        string result = "";

        DataTable dt = new DataTable();
        dt = new TrendChart().SSC_3X_JOZST_Select(30, "", "", ref result); //，默认显示30期

        if (dt == null || dt.Rows.Count < 1)
        {

        }
        else
        {
            int k = dt.Rows.Count;
            tb1.Text = dt.Rows[0]["Isuse"].ToString();
            tb2.Text = dt.Rows[k - 1]["Isuse"].ToString();
        }

        GridView1.DataSource = dt;
        GridView1.DataBind();
        ColorBind();
    }

    protected void btnTop50_Click(object sender, EventArgs e)
    {
        string result = "";

        DataTable dt = new DataTable();
        dt = new TrendChart().SSC_3X_JOZST_Select(50, "", "", ref result); //，默认显示50期

        if (dt == null || dt.Rows.Count < 1)
        {

        }
        else
        {
            int k = dt.Rows.Count;
            tb1.Text = dt.Rows[0]["Isuse"].ToString();
            tb2.Text = dt.Rows[k - 1]["Isuse"].ToString();
        }

        GridView1.DataSource = dt;
        GridView1.DataBind();
        ColorBind();
    }

    protected void btnTop100_Click(object sender, EventArgs e)
    {
        string result = "";

        DataTable dt = new DataTable();
        dt = new TrendChart().SSC_3X_JOZST_Select(100, "", "", ref result); //，默认显示100期

        if (dt == null || dt.Rows.Count < 1)
        {

        }
        else
        {
            int k = dt.Rows.Count;
            tb1.Text = dt.Rows[0]["Isuse"].ToString();
            tb2.Text = dt.Rows[k - 1]["Isuse"].ToString();
        }

        GridView1.DataSource = dt;
        GridView1.DataBind();
        ColorBind();
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        if (tb1.Text == "" || tb1.Text == null || tb2.Text == "" || tb2.Text == null)
        {
            Shove._Web.JavaScript.Alert(this.Page, "请输入起止期数！");
        }

        int i = Shove._Convert.StrToInt(tb1.Text, 0);
        int j = Shove._Convert.StrToInt(tb2.Text, 0);
        if (i > j)
        {
            Shove._Web.JavaScript.Alert(this.Page, "起始期数输入有误，请查证在输入！");
            return;
        }

        string result = "";
        DataTable dt = null;

        dt = new TrendChart().SSC_3X_JOZST_Select(0, tb1.Text.ToString(), tb2.Text.ToString(), ref result);

        if (dt == null || dt.Rows.Count < 1)
        {

        }
        else
        {
            int k = dt.Rows.Count;
            tb1.Text = dt.Rows[0]["Isuse"].ToString();
            tb2.Text = dt.Rows[k - 1]["Isuse"].ToString();
        }

        GridView1.DataSource = dt;
        GridView1.DataBind();
        ColorBind();
    }

    protected void ColorBind()
    {
        for (int i = 0; i < GridView1.Rows.Count; i++)
        {
            if (GridView1.Rows[i].Cells[1].Text.Length == 5)
            {
                Label lb = new Label();
                lb.Text = GridView1.Rows[i].Cells[1].Text.Substring(0, 2) + "<font color='red'>" + GridView1.Rows[i].Cells[1].Text.Substring(2, 3) + "</font>";

                GridView1.Rows[i].Cells[1].Controls.Add(lb);
            }

            if (Shove._Convert.StrToInt(GridView1.Rows[i].Cells[3].Text, 0) == 0)
            {
                GridView1.Rows[i].Cells[3].Text = "奇奇奇";
                GridView1.Rows[i].Cells[3].ForeColor = System.Drawing.Color.FromArgb(255, 255, 255);
                GridView1.Rows[i].Cells[3].Font.Bold = true;
                GridView1.Rows[i].Cells[3].Style.Value = "background-color:Red";
            }

            if (Shove._Convert.StrToInt(GridView1.Rows[i].Cells[4].Text, 0) == 0)
            {
                GridView1.Rows[i].Cells[4].Text = "奇奇偶";
                GridView1.Rows[i].Cells[4].ForeColor = System.Drawing.Color.FromArgb(255, 255, 255);
                GridView1.Rows[i].Cells[4].Font.Bold = true;
                GridView1.Rows[i].Cells[4].Style.Value = "background-color:Red";
            }

            if (Shove._Convert.StrToInt(GridView1.Rows[i].Cells[5].Text, 0) == 0)
            {
                GridView1.Rows[i].Cells[5].Text = "奇偶奇";
                GridView1.Rows[i].Cells[5].ForeColor = System.Drawing.Color.FromArgb(255, 255, 255);
                GridView1.Rows[i].Cells[5].Font.Bold = true;
                GridView1.Rows[i].Cells[5].Style.Value = "background-color:Red";
            }

            if (Shove._Convert.StrToInt(GridView1.Rows[i].Cells[6].Text, 0) == 0)
            {
                GridView1.Rows[i].Cells[6].Text = "奇偶偶";
                GridView1.Rows[i].Cells[6].ForeColor = System.Drawing.Color.FromArgb(255, 255, 255);
                GridView1.Rows[i].Cells[6].Font.Bold = true;
                GridView1.Rows[i].Cells[6].Style.Value = "background-color:Red";
            }

            if (Shove._Convert.StrToInt(GridView1.Rows[i].Cells[7].Text, 0) == 0)
            {
                GridView1.Rows[i].Cells[7].Text = "偶奇奇";
                GridView1.Rows[i].Cells[7].ForeColor = System.Drawing.Color.FromArgb(255, 255, 255);
                GridView1.Rows[i].Cells[7].Font.Bold = true;
                GridView1.Rows[i].Cells[7].Style.Value = "background-color:Red";
            }

            if (Shove._Convert.StrToInt(GridView1.Rows[i].Cells[8].Text, 0) == 0)
            {
                GridView1.Rows[i].Cells[8].Text = "偶奇偶";
                GridView1.Rows[i].Cells[8].ForeColor = System.Drawing.Color.FromArgb(255, 255, 255);
                GridView1.Rows[i].Cells[8].Font.Bold = true;
                GridView1.Rows[i].Cells[8].Style.Value = "background-color:Red";
            }

            if (Shove._Convert.StrToInt(GridView1.Rows[i].Cells[9].Text, 0) == 0)
            {
                GridView1.Rows[i].Cells[9].Text = "偶偶奇";
                GridView1.Rows[i].Cells[9].ForeColor = System.Drawing.Color.FromArgb(255, 255, 255);
                GridView1.Rows[i].Cells[9].Font.Bold = true;
                GridView1.Rows[i].Cells[9].Style.Value = "background-color:Red";
            }

            if (Shove._Convert.StrToInt(GridView1.Rows[i].Cells[10].Text, 0) == 0)
            {
                GridView1.Rows[i].Cells[10].Text = "偶偶偶";
                GridView1.Rows[i].Cells[10].ForeColor = System.Drawing.Color.FromArgb(255, 255, 255);
                GridView1.Rows[i].Cells[10].Font.Bold = true;
                GridView1.Rows[i].Cells[10].Style.Value = "background-color:Red";
            }

            if (Shove._Convert.StrToInt(GridView1.Rows[i].Cells[12].Text, 0) == 0)
            {
                GridView1.Rows[i].Cells[12].Text = "0奇";
                GridView1.Rows[i].Cells[12].ForeColor = System.Drawing.Color.FromArgb(255, 255, 255);
                GridView1.Rows[i].Cells[12].Font.Bold = true;
                GridView1.Rows[i].Cells[12].Style.Value = "background-color:Red";
            }

            if (Shove._Convert.StrToInt(GridView1.Rows[i].Cells[13].Text, 0) == 0)
            {
                GridView1.Rows[i].Cells[13].Text = "1奇";
                GridView1.Rows[i].Cells[13].ForeColor = System.Drawing.Color.FromArgb(255, 255, 255);
                GridView1.Rows[i].Cells[13].Font.Bold = true;
                GridView1.Rows[i].Cells[13].Style.Value = "background-color:Red";
            }

            if (Shove._Convert.StrToInt(GridView1.Rows[i].Cells[14].Text, 0) == 0)
            {
                GridView1.Rows[i].Cells[14].Text = "2奇";
                GridView1.Rows[i].Cells[14].ForeColor = System.Drawing.Color.FromArgb(255, 255, 255);
                GridView1.Rows[i].Cells[14].Font.Bold = true;
                GridView1.Rows[i].Cells[14].Style.Value = "background-color:Red";
            }

            if (Shove._Convert.StrToInt(GridView1.Rows[i].Cells[15].Text, 0) == 0)
            {
                GridView1.Rows[i].Cells[15].Text = "3奇";
                GridView1.Rows[i].Cells[15].ForeColor = System.Drawing.Color.FromArgb(255, 255, 255);
                GridView1.Rows[i].Cells[15].Font.Bold = true;
                GridView1.Rows[i].Cells[15].Style.Value = "background-color:Red";
            }
        }
    }

    protected void GridView1_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.SetRenderMethodDelegate(new RenderMethod(DrawGridFooter));
        }

        if (e.Row.RowType == DataControlRowType.Header)
        {
            e.Row.SetRenderMethodDelegate(new RenderMethod(DrawGridHeader));
        }
    }

    protected void DrawGridHeader(HtmlTextWriter output, Control ctl)
    {
        output.Write("<td rowspan='2'　align='center' valign='middle' bgcolor='#e8f1f8'>期号</td>");
        output.Write("<td rowspan='2'　align='center' valign='middle' bgcolor='#e8f1f8'>奖号</td>");
        output.Write("<td rowspan='2'　align='center' valign='middle' bgcolor='#e8f1f8'>类型</td>");
        output.Write("<td colspan='8'　align='center' valign='middle' bgcolor='#e8f1f8' height='20px'>单选奇偶类型</td>");
        output.Write("<td colspan='5'　align='center' valign='middle' bgcolor='#e8f1f8'>组选奇偶类型</td>");

        output.Write("<tr align='center' valign='middle' bgcolor='#f7f7f7'>");
        output.Write("<td 　align='center' valign='middle' bgcolor='#f7f7f7' height='20px'>奇奇奇</td>");
        output.Write("<td 　align='center' valign='middle' bgcolor='#f7f7f7'>奇奇偶</td>");
        output.Write("<td 　align='center' valign='middle' bgcolor='#f7f7f7'>奇偶奇</td>");
        output.Write("<td 　align='center' valign='middle' bgcolor='#f7f7f7'>奇偶偶</td>");
        output.Write("<td 　align='center' valign='middle' bgcolor='#f7f7f7'>偶奇奇</td>");
        output.Write("<td 　align='center' valign='middle' bgcolor='#f7f7f7'>偶奇偶</td>");
        output.Write("<td 　align='center' valign='middle' bgcolor='#f7f7f7'>偶偶奇</td>");
        output.Write("<td 　align='center' valign='middle' bgcolor='#f7f7f7'>偶偶偶</td>");
        output.Write("<td 　align='center' valign='middle' bgcolor='#f7f7f7'>奇偶比</td>");
        output.Write("<td 　align='center' valign='middle' bgcolor='#f7f7f7'>0奇</td>");
        output.Write("<td 　align='center' valign='middle' bgcolor='#f7f7f7'>1奇</td>");
        output.Write("<td 　align='center' valign='middle' bgcolor='#f7f7f7'>2奇</td>");
        output.Write("<td 　align='center' valign='middle' bgcolor='#f7f7f7'>3奇</td>");
    }

    protected void DrawGridFooter(HtmlTextWriter output, Control ctl)
    {
        output.Write("<td rowspan='2' colspan='2'　align='center' valign='middle'  bgcolor='#FFFFFF'>预测行</td>");

        for (int k = 0; k < 2; k++)
        {
            if (k == 1)
            {
                output.Write("<tr align='center' valign='middle'>");
            }

            output.Write("<td 　align='center' valign='middle' bgcolor='#FFFFFF'>&nbsp;</td>");

            output.Write("<td  height='18'　align='center' valign='middle' bgcolor='#fdfcdf' onClick=Style1(this,'#ff0000','#fdfcdf') style='color:#fdfcdf; cursor:pointer;'>奇奇奇</td>");
            output.Write("<td 　align='center' valign='middle' bgcolor='#fdfcdf' onClick=Style1(this,'#0000ff','#fdfcdf') style='color:#fdfcdf; cursor:pointer;'>奇奇偶</td>");
            output.Write("<td 　align='center' valign='middle' bgcolor='#fdfcdf' onClick=Style1(this,'#ff0000','#fdfcdf') style='color:#fdfcdf; cursor:pointer;'>奇偶奇</td>");
            output.Write("<td 　align='center' valign='middle' bgcolor='#fdfcdf' onClick=Style1(this,'#0000ff','#fdfcdf') style='color:#fdfcdf; cursor:pointer;'>奇偶偶</td>");
            output.Write("<td 　align='center' valign='middle' bgcolor='#fdfcdf' onClick=Style1(this,'#ff0000','#fdfcdf') style='color:#fdfcdf; cursor:pointer;'>偶奇奇</td>");
            output.Write("<td 　align='center' valign='middle' bgcolor='#fdfcdf' onClick=Style1(this,'#0000ff','#fdfcdf') style='color:#fdfcdf; cursor:pointer;'>偶奇偶</td>");
            output.Write("<td 　align='center' valign='middle' bgcolor='#fdfcdf' onClick=Style1(this,'#ff0000','#fdfcdf') style='color:#fdfcdf; cursor:pointer;'>偶偶奇</td>");
            output.Write("<td 　align='center' valign='middle' bgcolor='#fdfcdf' onClick=Style1(this,'#0000ff','#fdfcdf') style='color:#fdfcdf; cursor:pointer;'>偶偶偶</td>");
            output.Write("<td 　align='center' valign='middle'  bgcolor='#FFFFFF'>&nbsp;</td>");
            output.Write("<td 　align='center' valign='middle' bgcolor='#fdfcdf' onClick=Style1(this,'#ff0000','#fdfcdf') style='color:#fdfcdf; cursor:pointer;'>0奇</td>");
            output.Write("<td 　align='center' valign='middle' bgcolor='#fdfcdf' onClick=Style1(this,'#0000ff','#fdfcdf') style='color:#fdfcdf; cursor:pointer;'>1奇</td>");
            output.Write("<td 　align='center' valign='middle' bgcolor='#fdfcdf' onClick=Style1(this,'#ff0000','#fdfcdf') style='color:#fdfcdf; cursor:pointer;'>2奇</td>");
            output.Write("<td 　align='center' valign='middle' bgcolor='#fdfcdf' onClick=Style1(this,'#0000ff','#fdfcdf') style='color:#fdfcdf; cursor:pointer;'>3奇</td>");
        }

        output.Write("<tr align='center' valign='middle' >");
        output.Write("<td rowspan='2'　 bgcolor='#e8f1f8'>期号</td>");
        output.Write("<td rowspan='2'　 bgcolor='#e8f1f8'>奖号</td>");
        output.Write("<td rowspan='2'　 bgcolor='#e8f1f8'>类型</td>");

        output.Write("<td 　 bgcolor='#f7f7f7' height='20px'>奇奇奇</td>");
        output.Write("<td 　 bgcolor='#f7f7f7'>奇奇偶</td>");
        output.Write("<td 　 bgcolor='#f7f7f7'>奇偶奇</td>");
        output.Write("<td 　 bgcolor='#f7f7f7'>奇偶偶</td>");
        output.Write("<td 　 bgcolor='#f7f7f7'>偶奇奇</td>");
        output.Write("<td 　 bgcolor='#f7f7f7'>偶奇偶</td>");
        output.Write("<td 　 bgcolor='#f7f7f7'>偶偶奇</td>");
        output.Write("<td 　 bgcolor='#f7f7f7'>偶偶偶</td>");
        output.Write("<td 　 bgcolor='#f7f7f7'>奇偶比</td>");
        output.Write("<td 　 bgcolor='#f7f7f7'>0奇</td>");
        output.Write("<td 　 bgcolor='#f7f7f7'>1奇</td>");
        output.Write("<td 　 bgcolor='#f7f7f7'>2奇</td>");
        output.Write("<td 　 bgcolor='#f7f7f7'>3奇</td>");

        output.Write("<tr align='center' valign='middle' bgcolor='#e8f1f8'>");
        output.Write("<td colspan='8'　 bgcolor='#e8f1f8' height='20px'>单选奇偶类型</td>");
        output.Write("<td colspan='5'　 bgcolor='#e8f1f8'>组选奇偶类型</td>");
    }
}
