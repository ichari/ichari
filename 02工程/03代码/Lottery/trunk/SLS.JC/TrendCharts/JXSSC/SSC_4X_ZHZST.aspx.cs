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

public partial class Home_TrendCharts_JXSSC_SSC_4X_ZHZST : TrendChartPageBase
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
        dt = new TrendChart().SSC_4X_ZHZST_Select(30, "", "", ref result); //，默认显示30期

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
        dt = new TrendChart().SSC_4X_ZHZST_Select(30, "", "", ref result); //，默认显示30期

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
        dt = new TrendChart().SSC_4X_ZHZST_Select(50, "", "", ref result); //，默认显示50期

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
        dt = new TrendChart().SSC_4X_ZHZST_Select(100, "", "", ref result); //，默认显示100期

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

        dt = new TrendChart().SSC_4X_ZHZST_Select(0, tb1.Text.ToString(), tb2.Text.ToString(), ref result);

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
                lb.Text = GridView1.Rows[i].Cells[1].Text.Substring(0, 1) + "<font color='red'>" + GridView1.Rows[i].Cells[1].Text.Substring(1, 4) + "</font>";

                GridView1.Rows[i].Cells[1].Controls.Add(lb);
            }

            if (GridView1.Rows[i].Cells[8].Text == "0")
            {
                HtmlImage img = new HtmlImage();
                img.Src = "../Images/blue_kong.gif";
                GridView1.Rows[i].Cells[8].Controls.Add(img);
            }

            if (GridView1.Rows[i].Cells[20].Text == "0")
            {
                HtmlImage img = new HtmlImage();
                img.Src = "../Images/red_kong.gif";
                GridView1.Rows[i].Cells[20].Controls.Add(img);
            }

            if (GridView1.Rows[i].Cells[14].Text == "0")
            {
                HtmlImage img = new HtmlImage();
                img.Src = "../Images/orange_kong.gif";
                GridView1.Rows[i].Cells[14].Controls.Add(img);
            }

            if (GridView1.Rows[i].Cells[26].Text == "0")
            {
                HtmlImage img = new HtmlImage();
                img.Src = "../Images/blue_kong.gif";
                GridView1.Rows[i].Cells[26].Controls.Add(img);
            }

            for (int j = 9; j < 12; j++)
            {
                if (Shove._Convert.StrToInt(GridView1.Rows[i].Cells[j].Text, -1) == 0)
                {
                    int k = j - 8;
                    HtmlImage img = new HtmlImage();

                    img.Src = "../Images/blue_" + k.ToString() + ".gif";

                    GridView1.Rows[i].Cells[j].Controls.Add(img);
                }
            }

            for (int j = 12, k = 5; j < 14; j++, k += 2)
            {
                if (Shove._Convert.StrToInt(GridView1.Rows[i].Cells[j].Text, -1) == 0)
                {
                    HtmlImage img = new HtmlImage();

                    img.Src = "../Images/blue_" + k.ToString() + ".gif";

                    GridView1.Rows[i].Cells[j].Controls.Add(img);
                }
            }

            if (Shove._Convert.StrToInt(GridView1.Rows[i].Cells[15].Text, -1) == 0)
            {
                HtmlImage img = new HtmlImage();

                img.Src = "../Images/orange_0.gif";

                GridView1.Rows[i].Cells[15].Controls.Add(img);
            }

            for (int j = 16, k = 4; j < 19; j++, k += 2)
            {
                if (Shove._Convert.StrToInt(GridView1.Rows[i].Cells[j].Text, 0) == 0)
                {
                    HtmlImage img = new HtmlImage();

                    img.Src = "../Images/orange_" + k.ToString() + ".gif";

                    GridView1.Rows[i].Cells[j].Controls.Add(img);
                }
            }

            if (Shove._Convert.StrToInt(GridView1.Rows[i].Cells[19].Text, 0) == 0)
            {
                HtmlImage img = new HtmlImage();

                img.Src = "../Images/orange_9.gif";

                GridView1.Rows[i].Cells[19].Controls.Add(img);
            }

            for (int j = 21, k = 1; j < 24; j++, k++)
            {
                if (Shove._Convert.StrToInt(GridView1.Rows[i].Cells[j].Text, 0) == 0)
                {
                    HtmlImage img = new HtmlImage();

                    img.Src = "../Images/red_" + k.ToString() + ".gif";

                    GridView1.Rows[i].Cells[j].Controls.Add(img);
                }
            }

            for (int j = 24, k = 5; j < 26; j++, k += 2)
            {
                if (Shove._Convert.StrToInt(GridView1.Rows[i].Cells[j].Text, 0) == 0)
                {
                    HtmlImage img = new HtmlImage();

                    img.Src = "../Images/red_" + k.ToString() + ".gif";

                    GridView1.Rows[i].Cells[j].Controls.Add(img);
                }
            }

            if (Shove._Convert.StrToInt(GridView1.Rows[i].Cells[27].Text, 0) == 0)
            {
                HtmlImage img = new HtmlImage();

                img.Src = "../Images/blue_0.gif";

                GridView1.Rows[i].Cells[27].Controls.Add(img);
            }

            if (Shove._Convert.StrToInt(GridView1.Rows[i].Cells[31].Text, 0) == 0)
            {
                HtmlImage img = new HtmlImage();

                img.Src = "../Images/blue_9.gif";

                GridView1.Rows[i].Cells[31].Controls.Add(img);
            }

            for (int j = 28, k = 4; j < 31; j++, k += 2)
            {
                if (Shove._Convert.StrToInt(GridView1.Rows[i].Cells[j].Text, 0) == 0)
                {
                    HtmlImage img = new HtmlImage();

                    img.Src = "../Images/blue_" + k.ToString() + ".gif";

                    GridView1.Rows[i].Cells[j].Controls.Add(img);
                }
            }

            if (GridView1.Rows[i].Cells[3].Text.Length > 1)
            {
                string Celltext3 = "";

                ArrayList al = new ArrayList();

                for (int a = 0; a < GridView1.Rows[i].Cells[3].Text.Length; a++)
                {
                    al.Add(GridView1.Rows[i].Cells[3].Text.Substring(a, 1));
                }

                al.Sort();

                for (int a = 0; a < al.Count; a++)
                {
                    Celltext3 += al[a] + ",";
                }

                GridView1.Rows[i].Cells[3].Text = Celltext3.Substring(0, Celltext3.Length - 1);
            }

            if (GridView1.Rows[i].Cells[4].Text.Length > 1)
            {
                string Celltext4 = "";

                ArrayList al2 = new ArrayList();

                for (int a = 0; a < GridView1.Rows[i].Cells[4].Text.Length; a++)
                {
                    al2.Add(GridView1.Rows[i].Cells[4].Text.Substring(a, 1));
                }

                al2.Sort();

                for (int a = 0; a < al2.Count; a++)
                {
                    Celltext4 += al2[a] + ",";
                }

                GridView1.Rows[i].Cells[4].Text = Celltext4.Substring(0, Celltext4.Length - 1);
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


        output.Write("<td rowspan='2'　 bgcolor='#E8F1F8'>期号</td>");
        output.Write("<td rowspan='2'　 bgcolor='#E8F1F8'>奖号</td>");
        output.Write("<td rowspan='2'　 bgcolor='#E8F1F8'>质合类型</td>");
        output.Write("<td rowspan='2'　 bgcolor='#E8F1F8'>质数</td>");
        output.Write("<td rowspan='2'　 bgcolor='#E8F1F8'>合数</td>");
        output.Write("<td rowspan='2'　 bgcolor='#E8F1F8'>质合比</td>");
        output.Write("<td rowspan='2'　 bgcolor='#E8F1F8'>质数和</td>");
        output.Write("<td rowspan='2'　 bgcolor='#E8F1F8'>合数和</td>");
        output.Write("<td colspan='6'　 bgcolor='#E8F1F8'>质数最大数</td>");
        output.Write("<td colspan='6'　 bgcolor='#E8F1F8'>合数最大数</td>");
        output.Write("<td colspan='6'　 bgcolor='#E8F1F8'>质数最小数</td>");
        output.Write("<td colspan='6'　 bgcolor='#E8F1F8' height='20'>合数最小数</td>");

        output.Write("<tr  bgcolor='#f7f7f7'>");

        for (int k = 0; k < 2; k++)
        {
            output.Write("<td 　 height='20px'>空</td>");
            output.Write("<td 　>1</td>");
            output.Write("<td 　>2</td>");
            output.Write("<td 　>3</td>");
            output.Write("<td 　>5</td>");
            output.Write("<td 　>7</td>");
            output.Write("<td 　>空</td>");
            output.Write("<td 　>0</td>");
            output.Write("<td 　>4</td>");
            output.Write("<td 　>6</td>");
            output.Write("<td 　>8</td>");
            output.Write("<td 　 >9</td>");
        }
    }

    protected void DrawGridFooter(HtmlTextWriter output, Control ctl)
    {
        output.Write("<td rowspan='2'　 bgcolor='#E8F1F8'>期号</td>");
        output.Write("<td rowspan='2'　 bgcolor='#E8F1F8'>奖号</td>");
        output.Write("<td rowspan='2'　 bgcolor='#E8F1F8'>质合类型</td>");
        output.Write("<td rowspan='2'　 bgcolor='#E8F1F8'>质数</td>");
        output.Write("<td rowspan='2'　 bgcolor='#E8F1F8'>合数</td>");
        output.Write("<td rowspan='2'　 bgcolor='#E8F1F8'>质合比</td>");
        output.Write("<td rowspan='2'　 bgcolor='#E8F1F8'>质数和</td>");
        output.Write("<td rowspan='2'　 bgcolor='#E8F1F8'>合数和</td>");

        for (int k = 0; k < 2; k++)
        {
            output.Write("<td 　 bgcolor='#F7F7F7' height='20px'>空</td>");
            output.Write("<td 　 bgcolor='#F7F7F7'>1</td>");
            output.Write("<td 　 bgcolor='#F7F7F7'>2</td>");
            output.Write("<td 　 bgcolor='#F7F7F7'>3</td>");
            output.Write("<td 　 bgcolor='#F7F7F7'>5</td>");
            output.Write("<td 　 bgcolor='#F7F7F7'>7</td>");
            output.Write("<td 　 bgcolor='#F7F7F7'>空</td>");
            output.Write("<td 　 bgcolor='#F7F7F7'>0</td>");
            output.Write("<td 　 bgcolor='#F7F7F7'>4</td>");
            output.Write("<td 　 bgcolor='#F7F7F7'>6</td>");
            output.Write("<td 　 bgcolor='#F7F7F7'>8</td>");
            output.Write("<td 　 bgcolor='#F7F7F7' >9</td>");
        }

        output.Write("<tr  bgcolor='#E8F1F8'>");
        output.Write("<td colspan='6'　>质数最大数</td>");
        output.Write("<td colspan='6'　>合数最大数</td>");
        output.Write("<td colspan='6'>质数最小数</td>");
        output.Write("<td colspan='6'>合数最小数</td>");
    }
}
