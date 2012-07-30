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

public partial class Home_TrendCharts_JXSSC_SSC_5X_PJZZST : TrendChartPageBase
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
        dt = new TrendChart().SSC_5X_PJZZST_Select(30, "", "", ref result); //，默认显示30期

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
        dt = new TrendChart().SSC_5X_PJZZST_Select(30, "", "", ref result); //，默认显示30期

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
        dt = new TrendChart().SSC_5X_PJZZST_Select(50, "", "", ref result); //，默认显示50期

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
        dt = new TrendChart().SSC_5X_PJZZST_Select(100, "", "", ref result); //，默认显示100期

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

        dt = new TrendChart().SSC_5X_PJZZST_Select(0, tb1.Text.ToString(), tb2.Text.ToString(), ref result);

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
        string ImgID = "";
        string ImgID1 = "";

        lbline.Text = "<script type=\"text/javascript\">function DrawLines(){";

        for (int i = 0; i < GridView1.Rows.Count; i++)
        {
            for (int j = 2; j < 12; j++)
            {
                if (Shove._Convert.StrToInt(GridView1.Rows[i].Cells[j].Text, -1) == 0)
                {
                    int k = j - 2;

                    Label lb = new Label();
                    lb.Text = "<img src='../Images/blue_" + k.ToString() + ".gif' id='" + "GridView1_ctl" + (i + 2).ToString().PadLeft(2, '0') + "_" + i.ToString() + "_" + j.ToString() + "' />";

                    if (ImgID != "")
                    {
                        lbline.Text += "DrawLine('" + ImgID + "','" + "GridView1_ctl" + (i + 2).ToString().PadLeft(2, '0') + "_" + i.ToString() + "_" + j.ToString() + "', 'blue');";
                    }

                    GridView1.Rows[i].Cells[j].Controls.Add(lb);

                    ImgID = "GridView1_ctl" + (i + 2).ToString().PadLeft(2, '0') + "_" + i.ToString() + "_" + j.ToString();
                }
            }

            for (int j = 25; j < 35; j++)
            {
                if (Shove._Convert.StrToInt(GridView1.Rows[i].Cells[j].Text, -1) == 0)
                {
                    int k = j - 25;

                    Label lb = new Label();
                    lb.Text = "<img src='../Images/orange_" + k.ToString() + ".gif' id='" + "GridView1_ctl" + (i + 2).ToString().PadLeft(2, '0') + "_" + i.ToString() + "_" + j.ToString() + "' />";

                    if (ImgID1 != "")
                    {
                        lbline.Text += "DrawLine('" + ImgID1 + "','" + "GridView1_ctl" + (i + 2).ToString().PadLeft(2, '0') + "_" + i.ToString() + "_" + j.ToString() + "', 'blue');";
                    }

                    GridView1.Rows[i].Cells[j].Controls.Add(lb);

                    ImgID1 = "GridView1_ctl" + (i + 2).ToString().PadLeft(2, '0') + "_" + i.ToString() + "_" + j.ToString();

                }
            }
        }

        lbline.Text += "}</script>";
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
        output.Write("<td rowspan='2'　 bgcolor='#E8F1F8'>期号</td>");
        output.Write("<td rowspan='2'　 bgcolor='#E8F1F8'>奖号</td>");
        output.Write("<td colspan='10'  bgcolor='#E8F1F8' height='18px'>平均值号码走势图</td>");
        output.Write("<td rowspan='2'　 bgcolor='#E8F1F8'>值</td>");
        output.Write("<td colspan='12'  bgcolor='#E8F1F8' height='18px'>平均值号码属性</td>");
        output.Write("<td colspan='10'  bgcolor='#E8F1F8' height='18px'>平均值振幅</td>");

        output.Write("<tr  >");

        for (int i = 0; i < 10; i++)
        {
            output.Write("<td bgcolor='#f7f7f7' height='18px' ForeColor='#ff0000'>");
            output.Write(i.ToString() + "</td>");
        }

        output.Write("<td 　 bgcolor='#E8F1F8'>奇</td>");
        output.Write("<td 　 bgcolor='#E8F1F8'>偶</td>");
        output.Write("<td 　 bgcolor='#E8F1F8'>大</td>");
        output.Write("<td 　 bgcolor='#E8F1F8'>小</td>");
        output.Write("<td 　 bgcolor='#E8F1F8'>质</td>");
        output.Write("<td 　 bgcolor='#E8F1F8'>合</td>");
        output.Write("<td 　 bgcolor='#E8F1F8'>0路</td>");
        output.Write("<td 　 bgcolor='#E8F1F8'>1路</td>");
        output.Write("<td 　 bgcolor='#E8F1F8'>2路</td>");
        output.Write("<td 　 bgcolor='#E8F1F8'>大</td>");
        output.Write("<td 　 bgcolor='#E8F1F8'>中</td>");
        output.Write("<td 　 bgcolor='#E8F1F8'>小</td>");

        for (int i = 0; i < 10; i++)
        {
            output.Write("<td bgcolor='#f7f7f7' height='18px' ForeColor='#ff0000'>");
            output.Write(i.ToString() + "</td>");
        }
    }

    protected void DrawGridFooter(HtmlTextWriter output, Control ctl)
    {
        output.Write("<td rowspan='2'　 bgcolor='#E8F1F8'>期号</td>");
        output.Write("<td rowspan='2'　 bgcolor='#E8F1F8'>奖号</td>");

        for (int i = 0; i < 10; i++)
        {
            output.Write("<td bgcolor='#f7f7f7' height='18px' ForeColor='#ff0000'>");
            output.Write(i.ToString() + "</td>");
        }

        output.Write("<td rowspan='2'　 bgcolor='#E8F1F8'>值</td>");
        output.Write("<td 　 bgcolor='#E8F1F8'>奇</td>");
        output.Write("<td 　 bgcolor='#E8F1F8'>偶</td>");
        output.Write("<td 　 bgcolor='#E8F1F8'>大</td>");
        output.Write("<td 　 bgcolor='#E8F1F8'>小</td>");
        output.Write("<td 　 bgcolor='#E8F1F8'>质</td>");
        output.Write("<td 　 bgcolor='#E8F1F8'>合</td>");
        output.Write("<td 　 bgcolor='#E8F1F8'>0路</td>");
        output.Write("<td 　 bgcolor='#E8F1F8'>1路</td>");
        output.Write("<td 　 bgcolor='#E8F1F8'>2路</td>");
        output.Write("<td 　 bgcolor='#E8F1F8'>大</td>");
        output.Write("<td 　 bgcolor='#E8F1F8'>中</td>");
        output.Write("<td 　 bgcolor='#E8F1F8'>小</td>");

        for (int i = 0; i < 10; i++)
        {
            output.Write("<td bgcolor='#f7f7f7' height='18px' ForeColor='#ff0000'>");
            output.Write(i.ToString() + "</td>");
        }

        output.Write("<tr  bgcolor='#E8F1F8'>");
        output.Write("<td colspan='10'  >平均值号码走势图</td>");
        output.Write("<td colspan='12'  >平均值号码属性</td>");
        output.Write("<td colspan='10' height='18px'>平均值振幅</td>");
    }
}
