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

public partial class Home_TrendCharts_FC3D_FC3D_ZHXT : TrendChartPageBase
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

        DataRow[] drAd = dt.Select("LotteryID=6 and [Name] = '广告一'", "[Order]");
        DataRow[] drAd1 = dt.Select("LotteryID=6 and [Name] = '广告二'", "[Order]");
        DataRow[] drAd2 = dt.Select("LotteryID=6 and [Name] = '广告三'", "[Order]");

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
        dt = new TrendChart().FC3D_ZHFB_Select(30, "", "", ref result); //，默认显示30期

        if (dt == null || dt.Rows.Count < 1)
        {
            //Shove._Web.JavaScript.Alert(this.Page, "数据库里没有此彩种开奖号码信息！");
        }
        else
        {
            int i = dt.Rows.Count;
            tb1.Text = dt.Rows[0]["Isuse"].ToString();
            tb2.Text = dt.Rows[i - 1]["Isuse"].ToString();
        }

        GridView1.DataSource = dt;
        GridView1.DataBind();
    }

    protected void btnTop30_Click(object sender, EventArgs e)
    {
        string result = "";

        DataTable dt = new DataTable();
        dt = new TrendChart().FC3D_ZHFB_Select(30, "", "", ref result); //，默认显示30期

        if (dt == null || dt.Rows.Count < 1)
        {
            //Shove._Web.JavaScript.Alert(this.Page, "数据库里没有此彩种开奖号码信息！");
        }
        else
        {
            int i = dt.Rows.Count;
            tb1.Text = dt.Rows[0]["Isuse"].ToString();
            tb2.Text = dt.Rows[i - 1]["Isuse"].ToString();
        }

        GridView1.DataSource = dt;
        GridView1.DataBind();
        ColorBind();
    }

    protected void btnTop50_Click(object sender, EventArgs e)
    {
        string result = "";

        DataTable dt = new DataTable();
        dt = new TrendChart().FC3D_ZHFB_Select(50, "", "", ref result); //，默认显示50期

        if (dt == null || dt.Rows.Count < 1)
        {
            //Shove._Web.JavaScript.Alert(this.Page, "数据库里没有此彩种开奖号码信息！");
        }
        else
        {
            int i = dt.Rows.Count;
            tb1.Text = dt.Rows[0]["Isuse"].ToString();
            tb2.Text = dt.Rows[i - 1]["Isuse"].ToString();
        }

        GridView1.DataSource = dt;
        GridView1.DataBind();
        ColorBind();
    }

    protected void btnTop100_Click(object sender, EventArgs e)
    {
        string result = "";

        DataTable dt = new DataTable();
        dt = new TrendChart().FC3D_ZHFB_Select(100, "", "", ref result); //，默认显示100期

        if (dt == null || dt.Rows.Count < 1)
        {
            //Shove._Web.JavaScript.Alert(this.Page, "数据库里没有此彩种开奖号码信息！");
        }
        else
        {
            int i = dt.Rows.Count;
            tb1.Text = dt.Rows[0]["Isuse"].ToString();
            tb2.Text = dt.Rows[i - 1]["Isuse"].ToString();
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

        dt = new TrendChart().FC3D_ZHFB_Select(0, tb1.Text.ToString(), tb2.Text.ToString(), ref result);

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
        lbline.Text = "<script type=\"text/javascript\">function DrawLines(){";
       string ImgID = "";
       string  ImgID1 = "";
       string ImgID2 = "";
       string ImgID3 = "";

        for (int i = 0; i < GridView1.Rows.Count; i++)
        {
                if (Shove._Convert.StrToInt(GridView1.Rows[i].Cells[12].Text, 0) == 0)
                {
                    HtmlImage hi = new HtmlImage();
                    hi.Src ="../../../Images/4[1].jpg";
                    GridView1.Rows[i].Cells[12].Controls.Add(hi);                  
                }
            

            for (int j = 14; j < 24; j++)
            {
                if (Shove._Convert.StrToInt(GridView1.Rows[i].Cells[j].Text, 0) == 0)
                {
                    Label lb = new Label();
                    lb.Text = "<strong style='color:red;' id='" + "GridView1_ctl" + (i + 2).ToString().PadLeft(2, '0') + "_" + i.ToString() + "_" + j.ToString() + "'>" + (j - 14).ToString();

                    if (ImgID != null && ImgID != "")
                    {
                        lbline.Text +="DrawLine('" + ImgID + "','" + "GridView1_ctl" + (i + 2).ToString().PadLeft(2, '0') + "_" + i.ToString() + "_" + j.ToString() + "', '#A9A9A9');";
                    }

                    GridView1.Rows[i].Cells[j].Controls.Add(lb);

                    ImgID ="GridView1_ctl" + (i + 2).ToString().PadLeft(2, '0') + "_" + i.ToString() + "_" + j.ToString();
                }
            }

            for (int j = 25; j < 35; j++)
            {
                if (Shove._Convert.StrToInt(GridView1.Rows[i].Cells[j].Text, 0) == 0)
                {
                    Label lb = new Label();
                    lb.Text = "<strong style='color:blue;' id='" + "GridView1_ctl" + (i + 2).ToString().PadLeft(2, '0') + "_" + i.ToString() + "_" + j.ToString() + "'>" + (j - 25).ToString();

                    if (ImgID1 != null && ImgID1 != "")
                    {
                        lbline.Text += "DrawLine('" + ImgID1 + "','" + "GridView1_ctl" + (i + 2).ToString().PadLeft(2, '0') + "_" + i.ToString() + "_" + j.ToString() + "', '#A9A9A9');";
                    }

                    GridView1.Rows[i].Cells[j].Controls.Add(lb);

                    ImgID1 = "GridView1_ctl" + (i + 2).ToString().PadLeft(2, '0') + "_" + i.ToString() + "_" + j.ToString();
                }
            }

            for (int j = 35; j < 45; j++)
            {
                if (Shove._Convert.StrToInt(GridView1.Rows[i].Cells[j].Text, 0) == 0)
                {
                    Label lb = new Label();
                    lb.Text = "<strong style='color:red;' id='" + "GridView1_ctl" + (i + 2).ToString().PadLeft(2, '0') + "_" + i.ToString() + "_" + j.ToString() + "'>" + (j - 35).ToString();

                    if (ImgID2 != null && ImgID2 != "")
                    {
                        lbline.Text += "DrawLine('" + ImgID2 + "','" + "GridView1_ctl" + (i + 2).ToString().PadLeft(2, '0') + "_" + i.ToString() + "_" + j.ToString() + "', '#A9A9A9');";
                    }

                    GridView1.Rows[i].Cells[j].Controls.Add(lb);

                    ImgID2 = "GridView1_ctl" + (i + 2).ToString().PadLeft(2, '0') + "_" + i.ToString() + "_" + j.ToString();
                }
            }

            for (int j = 45; j < 55; j++)
            {
                if (Shove._Convert.StrToInt(GridView1.Rows[i].Cells[j].Text, 0) == 0)
                {
                    Label lb = new Label();
                    lb.Text = "<strong style='color:red;' id='" + "GridView1_ctl" + (i + 2).ToString().PadLeft(2, '0') + "_" + i.ToString() + "_" + j.ToString() + "'>" + (j - 45).ToString();

                    if (ImgID3 != null && ImgID3 != "")
                    {
                        lbline.Text += "DrawLine('" + ImgID3 + "','" + "GridView1_ctl" + (i + 2).ToString().PadLeft(2, '0') + "_" + i.ToString() + "_" + j.ToString() + "', '#A9A9A9');";
                    }

                    GridView1.Rows[i].Cells[j].Controls.Add(lb);

                    ImgID3 = "GridView1_ctl" + (i + 2).ToString().PadLeft(2, '0') + "_" + i.ToString() + "_" + j.ToString();
                }
            }

            for (int j = 2; j < 12; j++)
            {
                if (Shove._Convert.StrToInt(GridView1.Rows[i].Cells[j].Text, 0) == -1)
                {
                    GridView1.Rows[i].Cells[j].Text = "&nbsp;";
                }
            }

            string str = GridView1.Rows[i].Cells[1].Text;
            int ii = int.Parse(str.Substring(0, 1));
            int jj = int.Parse(str.Substring(1, 1));
            int kk = int.Parse(str.Substring(2, 1));


            if (ii == jj || ii == kk)
            {
                int m = ii;
                if (m == 0)
                {
                    GridView1.Rows[i].Cells[2].ForeColor = System.Drawing.Color.FromArgb(255, 000, 000);
                }
                else if (m == 1)
                {
                    GridView1.Rows[i].Cells[3].ForeColor = System.Drawing.Color.FromArgb(255, 000, 000);
                }
                else if (m == 2)
                {
                    GridView1.Rows[i].Cells[4].ForeColor = System.Drawing.Color.FromArgb(255, 000, 000);
                }
                else if (m == 3)
                {
                    GridView1.Rows[i].Cells[5].ForeColor = System.Drawing.Color.FromArgb(255, 000, 000);
                }
                else if (m == 4)
                {
                    GridView1.Rows[i].Cells[6].ForeColor = System.Drawing.Color.FromArgb(255, 000, 000);
                }
                else if (m == 5)
                {
                    GridView1.Rows[i].Cells[7].ForeColor = System.Drawing.Color.FromArgb(255, 000, 000);
                }
                else if (m == 6)
                {
                    GridView1.Rows[i].Cells[8].ForeColor = System.Drawing.Color.FromArgb(255, 000, 000);
                }
                else if (m == 7)
                {
                    GridView1.Rows[i].Cells[9].ForeColor = System.Drawing.Color.FromArgb(255, 000, 000);
                }
                else if (m == 8)
                {
                    GridView1.Rows[i].Cells[10].ForeColor = System.Drawing.Color.FromArgb(255, 000, 000);
                }
                else
                {
                    GridView1.Rows[i].Cells[11].ForeColor = System.Drawing.Color.FromArgb(255, 000, 000);
                }
            }
            if (jj == kk)
            {
                int m = jj;
                if (m == 0)
                {
                    GridView1.Rows[i].Cells[2].ForeColor = System.Drawing.Color.FromArgb(255, 000, 000);
                }
                else if (m == 1)
                {
                    GridView1.Rows[i].Cells[3].ForeColor = System.Drawing.Color.FromArgb(255, 000, 000);
                }
                else if (m == 2)
                {
                    GridView1.Rows[i].Cells[4].ForeColor = System.Drawing.Color.FromArgb(255, 000, 000);
                }
                else if (m == 3)
                {
                    GridView1.Rows[i].Cells[5].ForeColor = System.Drawing.Color.FromArgb(255, 000, 000);
                }
                else if (m == 4)
                {
                    GridView1.Rows[i].Cells[6].ForeColor = System.Drawing.Color.FromArgb(255, 000, 000);
                }
                else if (m == 5)
                {
                    GridView1.Rows[i].Cells[7].ForeColor = System.Drawing.Color.FromArgb(255, 000, 000);
                }
                else if (m == 6)
                {
                    GridView1.Rows[i].Cells[8].ForeColor = System.Drawing.Color.FromArgb(255, 000, 000);
                }
                else if (m == 7)
                {
                    GridView1.Rows[i].Cells[9].ForeColor = System.Drawing.Color.FromArgb(255, 000, 000);
                }
                else if (m == 8)
                {
                    GridView1.Rows[i].Cells[10].ForeColor = System.Drawing.Color.FromArgb(255, 000, 000);
                }
                else
                {
                    GridView1.Rows[i].Cells[11].ForeColor = System.Drawing.Color.FromArgb(255, 000, 000);
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
        output.Write("<td rowspan='2'　 bgcolor='#e8f1f8'>期号</td>");
        //output.Write("<td rowspan='2'　 bgcolor='#e8f1f8'>试机号码</td>");
        output.Write("<td rowspan='2'　 bgcolor='#e8f1f8'>开奖号码</td>");
        output.Write("<td colspan='10'　 bgcolor='#e8f1f8' height='26px'>号码分布图</td>");
        output.Write("<td rowspan='2'　 bgcolor='#e8f1f8'>组三间隔</td>");
        output.Write("<td rowspan='2'　 bgcolor='#e8f1f8'>和值</td>");
        output.Write("<td colspan='10'　 bgcolor='#e8f1f8'>和值尾数走势图</td>");
        output.Write("<td rowspan='2'　 bgcolor='#e8f1f8'>012路</td>");
        output.Write("<td colspan='10'　 bgcolor='#e8f1f8'>第一位</td>");
        output.Write("<td colspan='10'　 bgcolor='#e8f1f8'>第二位</td>");
        output.Write("<td colspan='10'　 bgcolor='#e8f1f8'>第三位</td>");

        output.Write("<tr  bgcolor='#e8f1f8' height='18px'>");

        for (int j = 0; j < 5; j++)
        {
            for (int i = 0; i < 10; i++)
            {
                output.Write("<td>");
                output.Write(i.ToString() + "</td>");
            }
        }
    }

    protected void DrawGridFooter(HtmlTextWriter output, Control ctl)
    {
        output.Write("<td  height='22px'>预选1</td>");
        output.Write("<td>&nbsp;</td>");

        for (int i = 0; i < 10; i++)
        {
            output.Write("<td bgcolor='#FFFFFF' onClick=Style(this,'#000000','#FFFFFF') style='color:#FFFFFF;cursor:pointer;'><strong>");
            output.Write(i.ToString() + "</strong></span></td>");
        }
        output.Write("<td>&nbsp;</td>");
        output.Write("<td>&nbsp;</td>");

        for (int i = 0; i < 10; i++)
        {
            output.Write("<td bgcolor='#FFFFFF' onClick=Style(this,'#0000ff','#FFFFFF') style='color:#FFFFFF;cursor:pointer;' ><strong>");
            output.Write(i.ToString() + "</strong></span></td>");
        }
        output.Write("<td>&nbsp;</td>");

        for (int i = 0; i < 10; i++)
        {
            output.Write("<td bgcolor='#FFFFFF' onClick=Style(this,'#0000ff','#FFFFFF') style='color:#FFFFFF;cursor:pointer;' ><strong>");
            output.Write(i.ToString() + "</strong></span></td>");
        }
        for (int i = 0; i < 10; i++)
        {
            output.Write("<td bgcolor='#FFFFFF' onClick=Style(this,'#ff0000','#FFFFFF') style='color:#FFFFFF;' ><strong>");
            output.Write(i.ToString() + "</strong></span></td>");
        }
        for (int i = 0; i < 10; i++)
        {
            output.Write("<td bgcolor='#FFFFFF' onClick=Style(this,'#0000ff','#FFFFFF') style='color:#FFFFFF;' ><strong>");
            output.Write(i.ToString() + "</strong></span></td>");
        }

        output.Write("<tr  style='cursor:pointer;'>");
        output.Write("<td  height='22px'>预选2</td>");

        output.Write("<td>&nbsp;</td>");

        for (int i = 0; i < 10; i++)
        {
            output.Write("<td bgcolor='#FFFFFF' onClick=Style(this,'#000000','#FFFFFF') style='color:#FFFFFF;' ><strong>");
            output.Write(i.ToString() + "</strong></span></td>");
        }
        output.Write("<td>&nbsp;</td>");
        output.Write("<td>&nbsp;</td>");

        for (int i = 0; i < 10; i++)
        {
            output.Write("<td bgcolor='#FFFFFF' onClick=Style(this,'#0000ff','#FFFFFF') style='color:#FFFFFF;' ><strong>");
            output.Write(i.ToString() + "</strong></span></td>");
        }
        output.Write("<td>&nbsp;</td>");

        for (int i = 0; i < 10; i++)
        {
            output.Write("<td bgcolor='#FFFFFF' onClick=Style(this,'#0000ff','#FFFFFF') style='color:#FFFFFF;' ><strong>");
            output.Write(i.ToString() + "</strong></span></td>");
        }
        for (int i = 0; i < 10; i++)
        {
            output.Write("<td bgcolor='#FFFFFF' onClick=Style(this,'#ff0000','#FFFFFF') style='color:#FFFFFF;' ><strong>");
            output.Write(i.ToString() + "</strong></span></td>");
        }
        for (int i = 0; i < 10; i++)
        {
            output.Write("<td bgcolor='#FFFFFF' onClick=Style(this,'#0000ff','#FFFFFF') style='color:#FFFFFF;' ><strong>");
            output.Write(i.ToString() + "</strong></span></td>");
        }

        output.Write("<tr style='cursor:pointer;' >");
        output.Write("<td  height='22px'>预选3</td>");
        output.Write("<td>&nbsp;</td>");

        for (int i = 0; i < 10; i++)
        {
            output.Write("<td bgcolor='#FFFFFF' onClick=Style(this,'#000000','#FFFFFF') style='color:#FFFFFF;' ><strong>");
            output.Write(i.ToString() + "</strong></span></td>");
        }
        output.Write("<td>&nbsp;</td>");
        output.Write("<td>&nbsp;</td>");

        for (int i = 0; i < 10; i++)
        {
            output.Write("<td bgcolor='#FFFFFF' onClick=Style(this,'#0000ff','#FFFFFF') style='color:#FFFFFF;' ><strong>");
            output.Write(i.ToString() + "</strong></span></td>");
        }
        output.Write("<td>&nbsp;</td>");

        for (int i = 0; i < 10; i++)
        {
            output.Write("<td bgcolor='#FFFFFF' onClick=Style(this,'#0000ff','#FFFFFF') style='color:#FFFFFF;' ><strong>");
            output.Write(i.ToString() + "</strong></span></td>");
        }
        for (int i = 0; i < 10; i++)
        {
            output.Write("<td bgcolor='#FFFFFF' onClick=Style(this,'#ff0000','#FFFFFF') style='color:#FFFFFF;' ><strong>");
            output.Write(i.ToString() + "</strong></span></td>");
        }
        for (int i = 0; i < 10; i++)
        {
            output.Write("<td bgcolor='#FFFFFF' onClick=Style(this,'#0000ff','#FFFFFF') style='color:#FFFFFF;' ><strong>");
            output.Write(i.ToString() + "</strong></span></td>");
        }

        output.Write("<tr bgcolor='#e8f1f8'>");
        output.Write("<td rowspan='2'　 >期  号</td>");
        //output.Write("<td rowspan='2'　 >试机号码</td>");
        output.Write("<td rowspan='2'　 >开奖号码</td>");

        for (int i = 0; i < 10; i++)
        {
            output.Write("<td  height='18px'>");
            output.Write(i.ToString() + "</td>");
        }
        output.Write("<td rowspan='2'>组三间隔</td>");
        output.Write("<td rowspan='2'>和值</td>");
        for (int i = 0; i < 10; i++)
        {
            output.Write("<td >");
            output.Write(i.ToString() + "</td>");
        }
      
        output.Write("<td rowspan='2'　 bgcolor='#e8f1f8'>012路</td>");

        for (int j = 0; j < 3; j++)
        {
            for (int i = 0; i < 10; i++)
            {
                output.Write("<td bgcolor='#e8f1f8'  rowspan='1'>");
                output.Write(i.ToString() + "</td>");
            }
        }

        output.Write("<tr  bgcolor='#e8f1f8' height='26px'>");

        output.Write("<td colspan='10'　>号码分布图</td>");
        output.Write("<td colspan='10'　>和值尾号走势图</td>");
        output.Write("<td colspan='10'　>第一位</td>");
        output.Write("<td colspan='10'　>第二位</td>");
        output.Write("<td colspan='10'　>第三位</td>");
    }
}
