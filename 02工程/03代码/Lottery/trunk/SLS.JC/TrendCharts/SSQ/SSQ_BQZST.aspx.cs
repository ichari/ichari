using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Text;

public partial class Home_TrendCharts_SSQ_SSQ_BQZST : TrendChartPageBase
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

        DataRow[] drAd = dt.Select("LotteryID=5 and [Name] = '广告一'", "[Order]");
        DataRow[] drAd1 = dt.Select("LotteryID=5 and [Name] = '广告二'", "[Order]");
        DataRow[] drAd2 = dt.Select("LotteryID=5 and [Name] = '广告三'", "[Order]");

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
            sb.Append("<tr><td class='blue'><a style='color:").Append(title.Length == 2 ? title[1] : "#000000").Append(";' href=\"").Append(dr["Url"].ToString()).Append("\" target='_blank'>").Append(title[0]).AppendLine("</a></td></tr>");
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
                sb.Append("<tr><td class='blue'><a style='color:").Append(title.Length == 2 ? title[1] : "#000000").Append(";' href=\"").Append(dr["Url"].ToString()).Append("\" target='_blank'>").Append(title[0]).AppendLine("</a></td></tr>");
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
                sb.Append("<tr><td class='blue'><a style='color:").Append(title.Length == 2 ? title[1] : "#000000").Append(";' href=\"").Append(dr["Url"].ToString()).Append("\" target='_blank'>").Append(title[0]).AppendLine("</a></td></tr>");
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
        dt = new TrendChart().SSQ_BQZH_Select(30, "", "", ref result); //，默认显示30期

        if (dt == null || dt.Rows.Count < 1)
        {
            //Shove._Web.JavaScript.Alert(this.Page, "数据库里没有此彩种开奖号码信息！");
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
        dt = new TrendChart().SSQ_BQZH_Select(30, "", "", ref result); //，默认显示30期

        if (dt == null || dt.Rows.Count < 1)
        {
            //Shove._Web.JavaScript.Alert(this.Page, "数据库里没有此彩种开奖号码信息！");
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
        dt = new TrendChart().SSQ_BQZH_Select(50, "", "", ref result); //，默认显示50期

        if (dt == null || dt.Rows.Count < 1)
        {
            //Shove._Web.JavaScript.Alert(this.Page, "数据库里没有此彩种开奖号码信息！");
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
        dt = new TrendChart().SSQ_BQZH_Select(100, "", "", ref result); //，默认显示100期

        if (dt == null || dt.Rows.Count < 1)
        {
           // Shove._Web.JavaScript.Alert(this.Page, "数据库里没有此彩种开奖号码信息！");
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

        dt = new TrendChart().SSQ_BQZH_Select(0, tb1.Text.ToString(), tb2.Text.ToString(), ref result);

        if (dt == null || dt.Rows.Count < 1)
        {
            //Shove._Web.JavaScript.Alert(this.Page, "数据库里没有此彩种开奖号码信息！");
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

        lbline.Text = "<script type=\"text/javascript\">function DrawLines(){";

        for (int i = 0; i < GridView1.Rows.Count; i++)
        {
            for (int j = 3; j < 19; j++)
            {
                if (Shove._Convert.StrToInt(GridView1.Rows[i].Cells[j].Text, 0) == 0)
                {
                    Label lb = new Label();
                    lb.Text = "<strong style='color:Red' id='" + "GridView1_ctl" + (i + 2).ToString().PadLeft(2, '0') + "_" + i.ToString() + "_" + j.ToString() + "' />"+(j-2).ToString();    
                   // lb.Text = "<lable width='20' style='font-weight:bold; color:Red;' id='" + "GridView1_ctl" + (i + 2).ToString().PadLeft(2, '0') + "_" + i.ToString() + "_" + j.ToString() + "' />"+(j-2).ToString();

                    if (ImgID != "")
                    {
                        lbline.Text += "DrawLine('" + ImgID + "','" + "GridView1_ctl" + (i + 2).ToString().PadLeft(2, '0') + "_" + i.ToString() + "_" + j.ToString() + "', 'blue');";
                    }

                    GridView1.Rows[i].Cells[j].Controls.Add(lb);

                    ImgID = "GridView1_ctl" + (i + 2).ToString().PadLeft(2, '0') + "_" + i.ToString() + "_" + j.ToString();
                }
            }

            for (int j = 19; j < 23; j++)
            {
                if (GridView1.Rows[i].Cells[j].Text == "0")
                {
                    GridView1.Rows[i].Cells[j].Text = "&nbsp;";
                }
            }

            for (int j = 23; j < 26; j++)
            {
                if (Shove._Convert.StrToInt(GridView1.Rows[i].Cells[j].Text, 0) == -1)
                {
                    GridView1.Rows[i].Cells[j].Text = "&nbsp;";
                }
            }
            if (Shove._Convert.StrToInt(GridView1.Rows[i].Cells[23].Text, -1) == 0)
                {
                    GridView1.Rows[i].Cells[23].Text = "◎";
                }
            if (Shove._Convert.StrToInt(GridView1.Rows[i].Cells[24].Text, 0) == 1)
            {
                GridView1.Rows[i].Cells[24].Text = "①";
            }
            if (Shove._Convert.StrToInt(GridView1.Rows[i].Cells[25].Text, 0) == 2)
            {
                GridView1.Rows[i].Cells[25].Text = "②";
            }

            for (int j = 26; j < 30; j++)
            {
                int ii = Shove._Convert.StrToInt(GridView1.Rows[i].Cells[2].Text, 0);

                if (Shove._Convert.StrToInt(GridView1.Rows[i].Cells[j].Text, 0) == 0)
                {
                    GridView1.Rows[i].Cells[j].Text = ii.ToString();
                    GridView1.Rows[i].Cells[j].ForeColor = System.Drawing.Color.FromArgb(000, 000,255);
                    GridView1.Rows[i].Cells[j].Font.Bold = true;
                }
            }

            if (Shove._Convert.StrToInt(GridView1.Rows[i].Cells[30].Text, 0) == 0)
            {
                GridView1.Rows[i].Cells[30].Text = "金";
                GridView1.Rows[i].Cells[30].ForeColor = System.Drawing.Color.FromArgb(255, 000, 000);
            }
            if (Shove._Convert.StrToInt(GridView1.Rows[i].Cells[31].Text, 0) == 0)
            {
                GridView1.Rows[i].Cells[31].Text = "木";
                GridView1.Rows[i].Cells[31].ForeColor = System.Drawing.Color.FromArgb(000, 000,255);
            }
             if (Shove._Convert.StrToInt(GridView1.Rows[i].Cells[32].Text, 0) == 0)
            {
                GridView1.Rows[i].Cells[32].Text = "水";
                GridView1.Rows[i].Cells[32].ForeColor = System.Drawing.Color.FromArgb(255, 000, 000);
            }
            if (Shove._Convert.StrToInt(GridView1.Rows[i].Cells[33].Text, 0) == 0)
            {
                GridView1.Rows[i].Cells[33].Text = "火";
                GridView1.Rows[i].Cells[33].ForeColor = System.Drawing.Color.FromArgb(000, 000,255);
            }
             if (Shove._Convert.StrToInt(GridView1.Rows[i].Cells[34].Text, 0) == 0)
            {
                GridView1.Rows[i].Cells[34].Text = "土";
                GridView1.Rows[i].Cells[34].ForeColor = System.Drawing.Color.FromArgb(255, 000, 000);
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
        output.Write("<td rowspan='2'　 bgcolor='#e8f1f8'>期号</td>");
        output.Write("<td colspan='2'　 bgcolor='#e8f1f8'>奖号</td>");

        output.Write("<td colspan='16'　 bgcolor='#e8f1f8' height='28px'>蓝球分布</td>");
        output.Write("<td colspan='2'　 bgcolor='#e8f1f8'>奇偶走势</td>");
        output.Write("<td colspan='2'　 bgcolor='#e8f1f8'>质合走势</td>");
        output.Write("<td colspan='3'　 bgcolor='#e8f1f8'>012路</td>");
        output.Write("<td colspan='4'　 bgcolor='#e8f1f8'>四区分布</td>");
        output.Write("<td colspan='5'　 bgcolor='#e8f1f8'>蓝球五行属性</td>");

        output.Write("<tr  bgcolor='#e8f1f8' >");
        output.Write("<td  height='42px'>红球</td>");
        output.Write("<td >蓝球</td>");  

        for (int i = 1; i < 10; i++)
        {
            output.Write("<td  >");
            output.Write(0 + i.ToString() + "</td>");
        }
        for (int i = 10; i < 17; i++)
        {
            output.Write("<td >");
            output.Write(i.ToString() + "</td>");
        }

        output.Write("<td >奇</td>");
        output.Write("<td >偶</td>");
        output.Write("<td >质</td>");
        output.Write("<td >合</td>");
        output.Write("<td >0</td>");
        output.Write("<td >1</td>");
        output.Write("<td >2</td>");
        output.Write("<td >1-4</td>");
        output.Write("<td >5-8</td>");
        output.Write("<td >9-12</td>");
        output.Write("<td >13-16</td>");
        output.Write("<td >金</td>");
        output.Write("<td >木</td>");
        output.Write("<td >水</td>");
        output.Write("<td >火</td>");
        output.Write("<td >土</td>");
    }

    protected void DrawGridFooter(HtmlTextWriter output, Control ctl)
    {
        output.Write("<td colspan='3' rowspan='2' height='20px'>预选行</td>");

        for (int i = 1; i < 10; i++)
        {
            output.Write("<td bgcolor='#FFFFFF' height='20' onClick=Style(this,'#ff0000','#FFFFFF') style='color:#FFFFFF;cursor:pointer;' ><strong>");
            output.Write(i.ToString() + "</strong></span></td>");
        }

        for (int i = 10; i < 17; i++)
        {
            output.Write("<td bgcolor='#FFFFFF' onClick=Style(this,'#ff0000','#FFFFFF') style='color:#FFFFFF;cursor:pointer;' ><strong>");
            output.Write(i.ToString() + "</strong></td>");
        }

        output.Write("<td bgcolor='#FFFFFF' onClick=Style(this,'#000000','#FFFFFF') style='color:#FFFFFF;cursor:pointer;' >奇</td>");
        output.Write("<td bgcolor='#FFFFFF' onClick=Style(this,'#ff0000','#FFFFFF') style='color:#FFFFFF;cursor:pointer;' >偶</td>");
        output.Write("<td bgcolor='#FFFFFF' onClick=Style(this,'#000000','#FFFFFF') style='color:#FFFFFF;cursor:pointer;' >质</td>");
        output.Write("<td bgcolor='#FFFFFF' onClick=Style(this,'#ff0000','#FFFFFF') style='color:#FFFFFF;cursor:pointer;' >合</td>");
        output.Write("<td bgcolor='#FFFFFF' onClick=Style(this,'#000000','#FFFFFF') style='color:#FFFFFF;cursor:pointer;' >0</td>");
        output.Write("<td bgcolor='#FFFFFF' onClick=Style(this,'#ff0000','#FFFFFF') style='color:#FFFFFF;cursor:pointer;' >1</td>");
        output.Write("<td bgcolor='#FFFFFF' onClick=Style(this,'#0000ff','#FFFFFF') style='color:#FFFFFF;cursor:pointer;' >2</td>");
        for (int i = 1; i < 5; i++)
        {
           output.Write("<td>&nbsp;</td>");
        }
        output.Write("<td bgcolor='#FFFFFF' onClick=Style(this,'#ff0000','#FFFFFF') style='color:#FFFFFF;cursor:pointer;' >金</td>");
        output.Write("<td bgcolor='#FFFFFF' onClick=Style(this,'#0000ff','#FFFFFF') style='color:#FFFFFF;cursor:pointer;' >木</td>");
        output.Write("<td bgcolor='#FFFFFF' onClick=Style(this,'#ff0000','#FFFFFF') style='color:#FFFFFF;cursor:pointer;' >水</td>");
        output.Write("<td bgcolor='#FFFFFF' onClick=Style(this,'#0000ff','#FFFFFF') style='color:#FFFFFF;cursor:pointer;' >火</td>");
        output.Write("<td bgcolor='#FFFFFF' onClick=Style(this,'#ff0000','#FFFFFF') style='color:#FFFFFF;cursor:pointer;' >土</td>");

        output.Write("<tr  bgcolor='#e8f1f8' style='cursor:pointer;' >");

        for (int i = 1; i < 10; i++)
        {
            output.Write("<td bgcolor='#FFFFFF' height='20' onClick=Style(this,'#ff0000','#FFFFFF') style='color:#FFFFFF;' ><strong>");
            output.Write(i.ToString() + "</strong></span></td>");
        }

        for (int i = 10; i < 17; i++)
        {
            output.Write("<td bgcolor='#FFFFFF' onClick=Style(this,'#ff0000','#FFFFFF') style='color:#FFFFFF;' ><strong>");
            output.Write(i.ToString() + "</strong></td>");
        }

        output.Write("<td bgcolor='#FFFFFF' onClick=Style(this,'#000000','#FFFFFF') style='color:#FFFFFF;' >奇</td>");
        output.Write("<td bgcolor='#FFFFFF' onClick=Style(this,'#ff0000','#FFFFFF') style='color:#FFFFFF;' >偶</td>");
        output.Write("<td bgcolor='#FFFFFF' onClick=Style(this,'#000000','#FFFFFF') style='color:#FFFFFF;' >质</td>");
        output.Write("<td bgcolor='#FFFFFF' onClick=Style(this,'#ff0000','#FFFFFF') style='color:#FFFFFF;' >合</td>");
        output.Write("<td bgcolor='#FFFFFF' onClick=Style(this,'#000000','#FFFFFF') style='color:#FFFFFF;' >0</td>");
        output.Write("<td bgcolor='#FFFFFF' onClick=Style(this,'#ff0000','#FFFFFF') style='color:#FFFFFF;' >1</td>");
        output.Write("<td bgcolor='#FFFFFF' onClick=Style(this,'#0000ff','#FFFFFF') style='color:#FFFFFF;' >2</td>");
        for (int i = 1; i < 5; i++)
        {
            output.Write("<td bgcolor='#FFFFFF'>&nbsp;</td>");
        }
        output.Write("<td bgcolor='#FFFFFF' onClick=Style(this,'#ff0000','#FFFFFF') style='color:#FFFFFF;' >金</td>");
        output.Write("<td bgcolor='#FFFFFF' onClick=Style(this,'#0000ff','#FFFFFF') style='color:#FFFFFF;' >木</td>");
        output.Write("<td bgcolor='#FFFFFF' onClick=Style(this,'#ff0000','#FFFFFF') style='color:#FFFFFF;' >水</td>");
        output.Write("<td bgcolor='#FFFFFF' onClick=Style(this,'#0000ff','#FFFFFF') style='color:#FFFFFF;' >火</td>");
        output.Write("<td bgcolor='#FFFFFF' onClick=Style(this,'#ff0000','#FFFFFF') style='color:#FFFFFF;' >土</td>");

        output.Write("<tr  bgcolor='#e8f1f8' >");
        output.Write("<td rowspan='2'　 >期号</td>");
        output.Write("<td  height='42px'>红球</td>");
        output.Write("<td >蓝球</td>");
        for (int i = 1; i < 10; i++)
        {
            output.Write("<td  >");
            output.Write(0+i.ToString() + "</td>");
        }
        for (int i = 10; i < 17; i++)
        {
            output.Write("<td >");
            output.Write(i.ToString() + "</td>");
        }
        output.Write("<td >奇</td>");
        output.Write("<td >偶</td>");
        output.Write("<td >质</td>");
        output.Write("<td >合</td>");
        output.Write("<td >0</td>");
        output.Write("<td >1</td>");
        output.Write("<td >2</td>");
        output.Write("<td >1-4</td>");
        output.Write("<td >5-8</td>");
        output.Write("<td >9-12</td>");
        output.Write("<td >13-16</td>");
        output.Write("<td >金</td>");
        output.Write("<td >木</td>");
        output.Write("<td >水</td>");
        output.Write("<td >火</td>");
        output.Write("<td >土</td>");

        output.Write("<tr  bgcolor='#e8f1f8' >");
        output.Write("<td colspan='2'　 bgcolor='#e8f1f8'>奖号</td>");
        output.Write("<td colspan='16'　  height='28px'>蓝球分布</td>");
        output.Write("<td colspan='2'　>奇偶走势</td>");
        output.Write("<td colspan='2'　>质合走势</td>");
        output.Write("<td colspan='3'　>012路</td>");
        output.Write("<td colspan='4'　>四区分布</td>");
        output.Write("<td colspan='5'　>蓝球五行属性</td>");
    }
}
