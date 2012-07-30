using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using System.Text;

public partial class Default3FB : TrendChartPageBase
{
    private int dtcount = 0;
    private int[] allCount = new int[27];
    private int[] avgCount = new int[27];
    private int[] maxCount = new int[27];
    private int[] lianCount = new int[27];
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            MyDataBind(1,1);
            ColorBind();
        }
    }
 
    private void MyDataBind(int DaySpan, int Type)
    {
        int returnValue = 0;
        string returnDescription = "";
        TrendChart tc = new TrendChart();
        DataTable dt = tc.SYDJ_Q3FBT(DaySpan, Type, ref returnValue, ref returnDescription);
        dtcount = dt.Rows.Count + 13;

        gvtable.DataSource = dt;
        gvtable.DataBind();
    }


    private void ColorBind()
    {


        for (int j = 0; j < gvtable.Rows.Count; j++)
        {
            for (int i = 4; i <= 30; i++)
            {
                try
                {
                    if (Convert.ToInt32(gvtable.Rows[j].Cells[i].Text) <= 0)
                    {

                        allCount[i - 4] = allCount[i - 4] + 1;

                        if (Convert.ToInt32(gvtable.Rows[j].Cells[i].Text) * -1 > lianCount[i - 4])
                        {
                            lianCount[i - 4] = Convert.ToInt32(gvtable.Rows[j].Cells[i].Text) * -1;
                        }
                        if (i <= 14)
                        {
                            gvtable.Rows[j].Cells[i].CssClass = "chartBall04";
                            gvtable.Rows[j].Cells[i].Text = Convert.ToString(i - 3);
                        }
                        if (i > 14 && i <= 22)
                        {
                            gvtable.Rows[j].Cells[i].CssClass = "cbg4";
                            switch(i)
                            {
                                case 15:
                                    gvtable.Rows[j].Cells[i].Text = "奇奇奇";
                                    break;
                                case 16:
                                    gvtable.Rows[j].Cells[i].Text = "奇奇偶";
                                    break;
                                case 17:
                                    gvtable.Rows[j].Cells[i].Text = "奇偶奇";
                                    break;
                                case 18:
                                    gvtable.Rows[j].Cells[i].Text = "偶奇奇";
                                    break;
                                case 19:
                                    gvtable.Rows[j].Cells[i].Text = "奇偶偶";
                                    break;
                                case 20:
                                    gvtable.Rows[j].Cells[i].Text = "偶奇偶";
                                    break;
                                case 21:
                                    gvtable.Rows[j].Cells[i].Text = "偶偶奇";
                                    break;
                                case 22:
                                    gvtable.Rows[j].Cells[i].Text = "偶偶偶";
                                    break;
                            }
                            
                        
                        }

                        if (i >= 23 && i <= 30)
                        {
                            gvtable.Rows[j].Cells[i].CssClass = "cbg8";
                            switch (i)
                            {
                                case 23:
                                    gvtable.Rows[j].Cells[i].Text = "小小小";
                                    break;
                                case 24:
                                    gvtable.Rows[j].Cells[i].Text = "小小大";
                                    break;
                                case 25:
                                    gvtable.Rows[j].Cells[i].Text = "小大小";
                                    break;
                                case 26:
                                    gvtable.Rows[j].Cells[i].Text = "大小小";
                                    break;
                                case 27:
                                    gvtable.Rows[j].Cells[i].Text = "小大大";
                                    break;
                                case 28:
                                    gvtable.Rows[j].Cells[i].Text = "大小大";
                                    break;
                                case 29:
                                    gvtable.Rows[j].Cells[i].Text = "大大小";
                                    break;
                                case 30:
                                    gvtable.Rows[j].Cells[i].Text = "大大大";
                                    break;
                            }
                        }

                       
                    }
                    else
                    {
                        if (gvtable.Rows[j].Cells[i].Text.Length >= 3)
                        {
                            gvtable.Rows[j].Cells[i].CssClass = "yl01 cfont_small";
                        }
                        else
                        {
                            gvtable.Rows[j].Cells[i].CssClass = "yl01";
                        }

                        if (Convert.ToInt32(gvtable.Rows[j].Cells[i].Text) > maxCount[i - 4])
                        {
                            maxCount[i - 4] = Convert.ToInt32(gvtable.Rows[j].Cells[i].Text);
                        }
                    }
                }
                catch (Exception ex)
                {
                    new Log("System").Write("SYDJ_Q3FBT.aspx" + ex.Message + " 页面异常");
                }
            }

        }
        for (int i = 0; i < 27; i++)
        {
            if (allCount[i] != 0)
            {
                avgCount[i] = (dtcount - 13) / allCount[i];
            }
            else
            {
                avgCount[i] = maxCount[i];
            }
        }

    }

    protected void DrawGridHeader(HtmlTextWriter output, Control ctl)
    {

        output.Write("<td  rowspan='2' align='center' ><strong>期号</strong></td>");
        output.Write("<td rowspan='2' class='cfont1'  colSpan='3'  >开奖号码</td>");
        output.Write("<TD style='BACKGROUND: #ccc' rowspan = " + dtcount.ToString() + " width=2>&nbsp;</TD>");

        output.Write("<td colspan='11'><strong>开奖号码分布图</strong></td>");
        output.Write("<TD style='BACKGROUND: #ccc' rowspan = " + dtcount.ToString() + " width=2>&nbsp;</TD>");

        output.Write("<td colspan='8'><strong>奇偶组合</strong></td>");
        output.Write("<TD style='BACKGROUND: #ccc' rowspan = " + dtcount.ToString() + " width=2>&nbsp;</TD>");

        output.Write("<td colspan='8'><strong>大小组合</strong></td>");
        output.Write("<TD style='BACKGROUND: #ccc' rowspan = " + dtcount.ToString() + " width=2>&nbsp;</TD>");
 
        output.Write("<tr class= 'thbg01' >");

        for (int i = 1; i <= 11; i++)
        {
            output.Write("<TD width=20> " + i.ToString() + "</TD>");
        }

        output.Write("<td  >奇奇奇</td>");
        output.Write("<td  >奇奇偶</td>");
        output.Write("<td  >奇偶奇</td>");
        output.Write("<td  >偶奇奇</td>");
        output.Write("<td  >奇偶偶</td>");
        output.Write("<td  >偶奇偶</td>");
        output.Write("<td  >偶偶奇</td>");
        output.Write("<td  >偶偶偶</td>");

        output.Write("<td height='15' >小小小</td>");
        output.Write("<td  >小小大</td>");
        output.Write("<td  >小大小</td>");
        output.Write("<td  >大小小</td>");
        output.Write("<td  >小大大</td>");
        output.Write("<td  >大小大</td>");
        output.Write("<td  >大大小</td>");
        output.Write("<td  >大大大</td>");

        output.Write("</tr  >");


    }

    protected void DrawGridFooter(HtmlTextWriter output, Control ctl)
    {
        output.Write("<td  height='36px'>预选1</td>");
        output.Write("<td >&nbsp;</td>");
        output.Write("<td >&nbsp;</td>");
        output.Write("<td >&nbsp;</td>");

        for (int i = 1; i <= 11; i++)
        {
            output.Write("<TD onClick=ShowImg(this," + Convert.ToString(i) + ",4)  style='cursor:pointer;'>&nbsp;</TD>");
        }


        output.Write("<TD onClick=ShowImg(this,'" + "奇奇奇" + "',5) style='cursor:pointer;'>&nbsp;</TD>");
        output.Write("<TD onClick=ShowImg(this,'" + "奇奇偶" + "',5) style='cursor:pointer;'>&nbsp;</TD>");
        output.Write("<TD onClick=ShowImg(this,'" + "奇偶奇" + "',5) style='cursor:pointer;'>&nbsp;</TD>");
        output.Write("<TD onClick=ShowImg(this,'" + "偶奇奇" + "',5) style='cursor:pointer;'>&nbsp;</TD>");
        output.Write("<TD onClick=ShowImg(this,'" + "奇偶偶" + "',5) style='cursor:pointer;'>&nbsp;</TD>");
        output.Write("<TD onClick=ShowImg(this,'" + "偶奇偶" + "',5) style='cursor:pointer;'>&nbsp;</TD>");
        output.Write("<TD onClick=ShowImg(this,'" + "偶偶奇" + "',5) style='cursor:pointer;'>&nbsp;</TD>");
        output.Write("<TD onClick=ShowImg(this,'" + "偶偶偶" + "',5) style='cursor:pointer;'>&nbsp;</TD>");

        output.Write("<TD onClick=ShowImg(this,'" + "小小小" + "',6) style='cursor:pointer;'>&nbsp;</TD>");
        output.Write("<TD onClick=ShowImg(this,'" + "小小大" + "',6) style='cursor:pointer;'>&nbsp;</TD>");
        output.Write("<TD onClick=ShowImg(this,'" + "小大小" + "',6) style='cursor:pointer;'>&nbsp;</TD>");
        output.Write("<TD onClick=ShowImg(this,'" + "大小小" + "',6) style='cursor:pointer;'>&nbsp;</TD>");
        output.Write("<TD onClick=ShowImg(this,'" + "小大大" + "',6) style='cursor:pointer;'>&nbsp;</TD>");
        output.Write("<TD onClick=ShowImg(this,'" + "大小大" + "',6) style='cursor:pointer;'>&nbsp;</TD>");
        output.Write("<TD onClick=ShowImg(this,'" + "大大小" + "',6) style='cursor:pointer;'>&nbsp;</TD>");
        output.Write("<TD onClick=ShowImg(this,'" + "大大大" + "',6) style='cursor:pointer;'>&nbsp;</TD>");

        output.Write("<tr  >");
        output.Write("<td  height='36px'>预选2</td>");
        output.Write("<td >&nbsp;</td>");
        output.Write("<td >&nbsp;</td>");
        output.Write("<td >&nbsp;</td>");

        for (int i = 1; i <= 11; i++)
        {
            output.Write("<TD onClick=ShowImg(this," + Convert.ToString(i) + ",4)  style='cursor:pointer;'>&nbsp;</TD>");
        }


        output.Write("<TD onClick=ShowImg(this,'" + "奇奇奇" + "',5) style='cursor:pointer;'>&nbsp;</TD>");
        output.Write("<TD onClick=ShowImg(this,'" + "奇奇偶" + "',5) style='cursor:pointer;'>&nbsp;</TD>");
        output.Write("<TD onClick=ShowImg(this,'" + "奇偶奇" + "',5) style='cursor:pointer;'>&nbsp;</TD>");
        output.Write("<TD onClick=ShowImg(this,'" + "偶奇奇" + "',5) style='cursor:pointer;'>&nbsp;</TD>");
        output.Write("<TD onClick=ShowImg(this,'" + "奇偶偶" + "',5) style='cursor:pointer;'>&nbsp;</TD>");
        output.Write("<TD onClick=ShowImg(this,'" + "偶奇偶" + "',5) style='cursor:pointer;'>&nbsp;</TD>");
        output.Write("<TD onClick=ShowImg(this,'" + "偶偶奇" + "',5) style='cursor:pointer;'>&nbsp;</TD>");
        output.Write("<TD onClick=ShowImg(this,'" + "偶偶偶" + "',5) style='cursor:pointer;'>&nbsp;</TD>");

        output.Write("<TD onClick=ShowImg(this,'" + "小小小" + "',6) style='cursor:pointer;'>&nbsp;</TD>");
        output.Write("<TD onClick=ShowImg(this,'" + "小小大" + "',6) style='cursor:pointer;'>&nbsp;</TD>");
        output.Write("<TD onClick=ShowImg(this,'" + "小大小" + "',6) style='cursor:pointer;'>&nbsp;</TD>");
        output.Write("<TD onClick=ShowImg(this,'" + "大小小" + "',6) style='cursor:pointer;'>&nbsp;</TD>");
        output.Write("<TD onClick=ShowImg(this,'" + "小大大" + "',6) style='cursor:pointer;'>&nbsp;</TD>");
        output.Write("<TD onClick=ShowImg(this,'" + "大小大" + "',6) style='cursor:pointer;'>&nbsp;</TD>");
        output.Write("<TD onClick=ShowImg(this,'" + "大大小" + "',6) style='cursor:pointer;'>&nbsp;</TD>");
        output.Write("<TD onClick=ShowImg(this,'" + "大大大" + "',6) style='cursor:pointer;'>&nbsp;</TD>");

        output.Write("</tr  >");

        output.Write("<tr  >");
        output.Write("<td  height='36px'>预选3</td>");
        output.Write("<td >&nbsp;</td>");
        output.Write("<td >&nbsp;</td>");
        output.Write("<td >&nbsp;</td>");

        for (int i = 1; i <= 11; i++)
        {
            output.Write("<TD onClick=ShowImg(this," + Convert.ToString(i) + ",4)  style='cursor:pointer;'>&nbsp;</TD>");
        }
        
        output.Write("<TD onClick=ShowImg(this,'" + "奇奇奇" + "',5) style='cursor:pointer;'>&nbsp;</TD>");
        output.Write("<TD onClick=ShowImg(this,'" + "奇奇偶" + "',5) style='cursor:pointer;'>&nbsp;</TD>");
        output.Write("<TD onClick=ShowImg(this,'" + "奇偶奇" + "',5) style='cursor:pointer;'>&nbsp;</TD>");
        output.Write("<TD onClick=ShowImg(this,'" + "偶奇奇" + "',5) style='cursor:pointer;'>&nbsp;</TD>");
        output.Write("<TD onClick=ShowImg(this,'" + "奇偶偶" + "',5) style='cursor:pointer;'>&nbsp;</TD>");
        output.Write("<TD onClick=ShowImg(this,'" + "偶奇偶" + "',5) style='cursor:pointer;'>&nbsp;</TD>");
        output.Write("<TD onClick=ShowImg(this,'" + "偶偶奇" + "',5) style='cursor:pointer;'>&nbsp;</TD>");
        output.Write("<TD onClick=ShowImg(this,'" + "偶偶偶" + "',5) style='cursor:pointer;'>&nbsp;</TD>");

        output.Write("<TD onClick=ShowImg(this,'" + "小小小" + "',6) style='cursor:pointer;'>&nbsp;</TD>");
        output.Write("<TD onClick=ShowImg(this,'" + "小小大" + "',6) style='cursor:pointer;'>&nbsp;</TD>");
        output.Write("<TD onClick=ShowImg(this,'" + "小大小" + "',6) style='cursor:pointer;'>&nbsp;</TD>");
        output.Write("<TD onClick=ShowImg(this,'" + "大小小" + "',6) style='cursor:pointer;'>&nbsp;</TD>");
        output.Write("<TD onClick=ShowImg(this,'" + "小大大" + "',6) style='cursor:pointer;'>&nbsp;</TD>");
        output.Write("<TD onClick=ShowImg(this,'" + "大小大" + "',6) style='cursor:pointer;'>&nbsp;</TD>");
        output.Write("<TD onClick=ShowImg(this,'" + "大大小" + "',6) style='cursor:pointer;'>&nbsp;</TD>");
        output.Write("<TD onClick=ShowImg(this,'" + "大大大" + "',6) style='cursor:pointer;'>&nbsp;</TD>");

        output.Write("</tr  >");

        output.Write("<tr  >");
        output.Write("<td  height='36px'>预选4</td>");
        output.Write("<td >&nbsp;</td>");
        output.Write("<td >&nbsp;</td>");
        output.Write("<td >&nbsp;</td>");

        for (int i = 1; i <= 11; i++)
        {
            output.Write("<TD onClick=ShowImg(this," + Convert.ToString(i) + ",4)  style='cursor:pointer;'>&nbsp;</TD>");
        }


        output.Write("<TD onClick=ShowImg(this,'" + "奇奇奇" + "',5) style='cursor:pointer;'>&nbsp;</TD>");
        output.Write("<TD onClick=ShowImg(this,'" + "奇奇偶" + "',5) style='cursor:pointer;'>&nbsp;</TD>");
        output.Write("<TD onClick=ShowImg(this,'" + "奇偶奇" + "',5) style='cursor:pointer;'>&nbsp;</TD>");
        output.Write("<TD onClick=ShowImg(this,'" + "偶奇奇" + "',5) style='cursor:pointer;'>&nbsp;</TD>");
        output.Write("<TD onClick=ShowImg(this,'" + "奇偶偶" + "',5) style='cursor:pointer;'>&nbsp;</TD>");
        output.Write("<TD onClick=ShowImg(this,'" + "偶奇偶" + "',5) style='cursor:pointer;'>&nbsp;</TD>");
        output.Write("<TD onClick=ShowImg(this,'" + "偶偶奇" + "',5) style='cursor:pointer;'>&nbsp;</TD>");
        output.Write("<TD onClick=ShowImg(this,'" + "偶偶偶" + "',5) style='cursor:pointer;'>&nbsp;</TD>");

        output.Write("<TD onClick=ShowImg(this,'" + "小小小" + "',6) style='cursor:pointer;'>&nbsp;</TD>");
        output.Write("<TD onClick=ShowImg(this,'" + "小小大" + "',6) style='cursor:pointer;'>&nbsp;</TD>");
        output.Write("<TD onClick=ShowImg(this,'" + "小大小" + "',6) style='cursor:pointer;'>&nbsp;</TD>");
        output.Write("<TD onClick=ShowImg(this,'" + "大小小" + "',6) style='cursor:pointer;'>&nbsp;</TD>");
        output.Write("<TD onClick=ShowImg(this,'" + "小大大" + "',6) style='cursor:pointer;'>&nbsp;</TD>");
        output.Write("<TD onClick=ShowImg(this,'" + "大小大" + "',6) style='cursor:pointer;'>&nbsp;</TD>");
        output.Write("<TD onClick=ShowImg(this,'" + "大大小" + "',6) style='cursor:pointer;'>&nbsp;</TD>");
        output.Write("<TD onClick=ShowImg(this,'" + "大大大" + "',6) style='cursor:pointer;'>&nbsp;</TD>");

        output.Write("</tr  >");

        output.Write("<tr  >");
        output.Write("<td  height='36px'>预选5</td>");
        output.Write("<td >&nbsp;</td>");
        output.Write("<td >&nbsp;</td>");
        output.Write("<td >&nbsp;</td>");

        for (int i = 1; i <= 11; i++)
        {
            output.Write("<TD onClick=ShowImg(this," + Convert.ToString(i) + ",4)  style='cursor:pointer;'>&nbsp;</TD>");
        }


        output.Write("<TD onClick=ShowImg(this,'" + "奇奇奇" + "',5) style='cursor:pointer;'>&nbsp;</TD>");
        output.Write("<TD onClick=ShowImg(this,'" + "奇奇偶" + "',5) style='cursor:pointer;'>&nbsp;</TD>");
        output.Write("<TD onClick=ShowImg(this,'" + "奇偶奇" + "',5) style='cursor:pointer;'>&nbsp;</TD>");
        output.Write("<TD onClick=ShowImg(this,'" + "偶奇奇" + "',5) style='cursor:pointer;'>&nbsp;</TD>");
        output.Write("<TD onClick=ShowImg(this,'" + "奇偶偶" + "',5) style='cursor:pointer;'>&nbsp;</TD>");
        output.Write("<TD onClick=ShowImg(this,'" + "偶奇偶" + "',5) style='cursor:pointer;'>&nbsp;</TD>");
        output.Write("<TD onClick=ShowImg(this,'" + "偶偶奇" + "',5) style='cursor:pointer;'>&nbsp;</TD>");
        output.Write("<TD onClick=ShowImg(this,'" + "偶偶偶" + "',5) style='cursor:pointer;'>&nbsp;</TD>");

        output.Write("<TD onClick=ShowImg(this,'" + "小小小" + "',6) style='cursor:pointer;'>&nbsp;</TD>");
        output.Write("<TD onClick=ShowImg(this,'" + "小小大" + "',6) style='cursor:pointer;'>&nbsp;</TD>");
        output.Write("<TD onClick=ShowImg(this,'" + "小大小" + "',6) style='cursor:pointer;'>&nbsp;</TD>");
        output.Write("<TD onClick=ShowImg(this,'" + "大小小" + "',6) style='cursor:pointer;'>&nbsp;</TD>");
        output.Write("<TD onClick=ShowImg(this,'" + "小大大" + "',6) style='cursor:pointer;'>&nbsp;</TD>");
        output.Write("<TD onClick=ShowImg(this,'" + "大小大" + "',6) style='cursor:pointer;'>&nbsp;</TD>");
        output.Write("<TD onClick=ShowImg(this,'" + "大大小" + "',6) style='cursor:pointer;'>&nbsp;</TD>");
        output.Write("<TD onClick=ShowImg(this,'" + "大大大" + "',6) style='cursor:pointer;'>&nbsp;</TD>");

        output.Write("</tr  >");

        //===============================总共出现几次 平均遗漏值 最大遗漏值 最大连出值 cfont_small

        output.Write("<tr  >");
        output.Write("<td  height='36px'>出现总次数</td>");
        output.Write("<td >&nbsp;</td>");
        output.Write("<td >&nbsp;</td>");
        output.Write("<td >&nbsp;</td>");

        for (int i = 0; i < 27; i++)
        {
            if (allCount[i].ToString().Length >= 3)
            {
                output.Write("<td class ='cfont_small'>" + allCount[i].ToString() + "</td>");
            }
            else
            {
                output.Write("<td >" + allCount[i].ToString() + "</td>");
            }
        }

        output.Write("</tr  >");

        output.Write("<tr  >");
        output.Write("<td  height='36px'>平均遗漏值</td>");
        output.Write("<td >&nbsp;</td>");
        output.Write("<td >&nbsp;</td>");
        output.Write("<td >&nbsp;</td>");

        for (int i = 0; i < 27; i++)
        {
            if (avgCount[i].ToString().Length >= 3)
            {
                output.Write("<td class ='cfont_small'>" + avgCount[i].ToString() + "</td>");
            }
            else
            {
                output.Write("<td >" + avgCount[i].ToString() + "</td>");
            }
        }

        output.Write("</tr  >");

        output.Write("<tr  >");
        output.Write("<td  height='36px'>最大遗漏值</td>");
        output.Write("<td >&nbsp;</td>");
        output.Write("<td >&nbsp;</td>");
        output.Write("<td >&nbsp;</td>");

        for (int i = 0; i < 27; i++)
        {
            if (maxCount[i].ToString().Length >= 3)
            {
                output.Write("<td class ='cfont_small'>" + maxCount[i].ToString() + "</td>");
            }
            else
            {
                output.Write("<td >" + maxCount[i].ToString() + "</td>");
            }
        }

        output.Write("</tr  >");

        output.Write("<tr  >");
        output.Write("<td  height='36px'>最大连出值</td>");
        output.Write("<td >&nbsp;</td>");
        output.Write("<td >&nbsp;</td>");
        output.Write("<td >&nbsp;</td>");

        for (int i = 0; i < 27; i++)
        {
            if (lianCount[i].ToString().Length >= 3)
            {
                output.Write("<td class ='cfont_small'>" + lianCount[i].ToString() + "</td>");
            }
            else
            {
                output.Write("<td >" + lianCount[i].ToString() + "</td>");
            }
        }

        output.Write("</tr  >");

        //=======================================================
        output.Write("<tr class= 'thbg01' >");
        output.Write("<td rowspan='2' class='td_bg' align='middle'  > &nbsp;</td>");
        output.Write("<td rowspan='2' class='cfont1 td_bg' colSpan='3'  >开奖号码</td>");

        for (int i = 1; i <= 11; i++)
        {
            output.Write("<TD width=20>" + i.ToString() + "</TD>");
        }

        output.Write("<td height='15' >奇奇奇</td>");
        output.Write("<td  >奇奇偶</td>");
        output.Write("<td  >奇偶奇</td>");
        output.Write("<td  >偶奇奇</td>");
        output.Write("<td  >奇偶偶</td>");
        output.Write("<td  >偶奇偶</td>");
        output.Write("<td  >偶偶奇</td>");
        output.Write("<td  >偶偶偶</td>");

        output.Write("<td  height='15' >小小小</td>");
        output.Write("<td  >小小大</td>");
        output.Write("<td  >小大小</td>");
        output.Write("<td  >大小小</td>");
        output.Write("<td  >小大大</td>");
        output.Write("<td  >大小大</td>");
        output.Write("<td  >大大小</td>");
        output.Write("<td  >大大大</td>");

        output.Write("</tr  >");

        output.Write("<tr  >");

        output.Write("<td colspan='11'><strong>开奖号码分布图</strong></td>");
        output.Write("<td colspan='8'><strong>奇偶组合</strong></td>");
        output.Write("<td colspan='8'><strong>大小组合</strong></td>");
 
        output.Write("</tr  >");
    }

    protected void gvtable_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.SetRenderMethodDelegate(new RenderMethod(DrawGridFooter));

        }

        if (e.Row.RowType == DataControlRowType.Header)
        {
            e.Row.SetRenderMethodDelegate(new RenderMethod(DrawGridHeader));
        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[1].CssClass = "chartBall01";
            e.Row.Cells[2].CssClass = "chartBall01";
            e.Row.Cells[3].CssClass = "chartBall01";
        }
    }
    protected void lbtnToday_Click(object sender, EventArgs e)
    {
        MyDataBind(0, 0);
        ColorBind();
    }
    protected void lbtnLast_Click(object sender, EventArgs e)
    {
        MyDataBind(1, 0);
        ColorBind();
    }
    protected void ddlSelect_SelectedIndexChanged(object sender, EventArgs e)
    {
        int k = Convert.ToInt32(ddlSelect.Items[ddlSelect.SelectedIndex].Value);

        MyDataBind(k - 1, 1);
        ColorBind();
    }
}
