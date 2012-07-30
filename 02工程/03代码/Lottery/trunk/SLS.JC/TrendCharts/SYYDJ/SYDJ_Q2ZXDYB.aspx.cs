using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class DefaultQ2Z : TrendChartPageBase
{
    private int dtcount = 0;
    private int[] allCount = new int[19];
    private int[] avgCount = new int[19];
    private int[] maxCount = new int[19];
    private int[] lianCount = new int[19];

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
        DataTable dt = tc.SYDJ_Q2ZXDYB(DaySpan, Type, ref returnValue, ref returnDescription);
        dtcount = dt.Rows.Count + 13;

        gvtable.DataSource = dt;
        gvtable.DataBind();
    }


    private void ColorBind()
    {


        for (int j = 0; j < gvtable.Rows.Count; j++)
        {
            for (int i = 4; i <= 22; i++)
            {
                if (Convert.ToInt32(gvtable.Rows[j].Cells[i].Text) <= 0)
                {
        
                    allCount[i - 4] = allCount[i - 4] + 1;

                    if (Convert.ToInt32(gvtable.Rows[j].Cells[i].Text) * -1 > lianCount[i - 4])
                    {
                        lianCount[i - 4] = Convert.ToInt32(gvtable.Rows[j].Cells[i].Text) * -1;
                    }

                    if (Convert.ToInt32(gvtable.Rows[j].Cells[1].Text) > Convert.ToInt32(gvtable.Rows[j].Cells[2].Text))
                    {
                        gvtable.Rows[j].Cells[i].CssClass = "cbg4";
                    }
                    else
                    {
                        gvtable.Rows[j].Cells[i].CssClass = "cbg5"; 
                    }
                    gvtable.Rows[j].Cells[i].Text = gvtable.Rows[j].Cells[1].Text + "&nbsp;" + gvtable.Rows[j].Cells[2].Text;
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

            
        }


        for (int i = 0; i < 19; i++)
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

        output.Write("<td width='80' rowspan='2' align='center' ><strong>期号</strong></td>");
        output.Write("<td rowspan='2' class='cfont1'  colSpan='2'  >开奖号码</td>");
        output.Write("<TD style='BACKGROUND: #ccc' rowspan = " + dtcount.ToString() + " width=2>&nbsp;</TD>");

        output.Write("<td width='35' rowspan='2'  class='cbg1 '><strong>和值</strong></td>");
   
        output.Write("<td colspan='19'><strong>前二组选和值</strong></td>");
        output.Write("<TD style='BACKGROUND: #ccc' rowspan = " + dtcount.ToString() + " width=2>&nbsp;</TD>");

       
        output.Write("<tr class= 'thbg01' >");

        for (int i = 3; i <= 21; i++)
        {
            output.Write("<TD ><STRONG>" + i.ToString() + "</STRONG></TD>");
        }
 
        output.Write("</tr  >");
 
    }

    protected void DrawGridFooter(HtmlTextWriter output, Control ctl)
    {
        

        //===============================总共出现几次 平均遗漏值 最大遗漏值 最大连出值

    
        output.Write("<td  height='36px'>出现总次数  </td>");
        output.Write("<td >&nbsp;</td>");
        output.Write("<td >&nbsp;</td>");
        output.Write("<td >&nbsp;</td>");

        for (int i = 0; i < 19; i++)
        {
            if (allCount[i].ToString().Length >= 3)
            {
                output.Write("<td class='cfont_small'>" + allCount[i].ToString() + "</td>");
            }
            else
            {
                output.Write("<td >" + allCount[i].ToString() + "</td>");
            } 
            
        }
 

        output.Write("<tr  >");
        output.Write("<td  height='36px'>平均遗漏值</td>");
        output.Write("<td >&nbsp;</td>");
        output.Write("<td >&nbsp;</td>");
        output.Write("<td >&nbsp;</td>");

        for (int i = 0; i < 19; i++)
        {
            if (avgCount[i].ToString().Length >= 3)
            {
                output.Write("<td class='cfont_small'>" + avgCount[i].ToString() + "</td>");
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

        for (int i = 0; i < 19; i++)
        {
            if (maxCount[i].ToString().Length >= 3)
            {
                output.Write("<td class='cfont_small'>" + maxCount[i].ToString() + "</td>");
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

        for (int i = 0; i < 19; i++)
        {
            if (lianCount[i].ToString().Length >= 3)
            {
                output.Write("<td class='cfont_small'>" + lianCount[i].ToString() + "</td>");
            }
            else
            {
                output.Write("<td >" + lianCount[i].ToString() + "</td>");
            } 
        }

        output.Write("</tr  >");

        //=======================================================
        output.Write("<tr class= 'thbg01' >");
        output.Write("<td rowspan='6' class='td_bg'   width='80px' align='middle'  > &nbsp;</td>");
        output.Write("<td rowspan='6' class='cfont1 td_bg' colSpan='2'  >开奖号码</td>");
        output.Write("<td width='35' rowspan='6' class='cbg1' style='background:#EFF7FE' ><strong>和值</strong></td>");

        for (int i = 3; i <= 21; i++)
        {
            output.Write("<TD width=44><STRONG>" + i.ToString() + "</STRONG></TD>");
        }

        output.Write("</tr  >");

        output.Write("<tr  >");

        output.Write("<td height='19'>01  02</td>");
        output.Write("<td>01 03</td>");
        output.Write("<td>01 04</td>");
        output.Write("<td>01 05</td>");
        output.Write("<td>01 06</td>");

        output.Write("<td>01 07</td>");
        output.Write("<td>01 08</td>");
        output.Write("<td>01 09</td>");
        output.Write("<td>01 10</td>");
        output.Write("<td>01 11</td>");

        output.Write("<td>02 11</td>");
        output.Write("<td>03 11</td>");
        output.Write("<td>04 11</td>");
        output.Write("<td>05 11</td>");
        output.Write("<td>07 10</td>");

        output.Write("<td>07 11</td>");
        output.Write("<td>08 11</td>");
        output.Write("<td>09 11</td>");
        output.Write("<td>10 11</td>");
 
        output.Write("</tr  >");

        output.Write("<tr  >");

        output.Write("<td height='19'>&nbsp;</td>");
        output.Write("<td>&nbsp;</td>");
        output.Write("<td height='15'>02 03</td>");
        output.Write("<td>02 04</td>");
        output.Write("<td>02 05</td>");

        output.Write("<td>02 06</td>");
        output.Write("<td>02 07</td>");
        output.Write("<td>02 08</td>");
        output.Write("<td>02 09</td>");
        output.Write("<td>02 10</td>");
        

        output.Write("<td>03 10</td>");
        output.Write("<td>04 10</td>");
        output.Write("<td>05 10</td>");
        output.Write("<td>06 10</td>");
        output.Write("<td>08 09</td>");

        output.Write("<td>08 10</td>");
        output.Write("<td>09 10</td>");
        output.Write("<td>&nbsp;</td>");
        output.Write("<td>&nbsp;</td>");

        output.Write("</tr  >");

        output.Write("<tr  >");

        output.Write("<td height='19'>&nbsp;</td>");
        output.Write("<td>&nbsp;</td>");
        output.Write("<td height='15'>&nbsp;</td>");
        output.Write("<td>&nbsp;</td>");
        output.Write("<td>03 04</td>");

        output.Write("<td>03 05</td>");
        output.Write("<td>03 06</td>");
        output.Write("<td>03 07</td>");
        output.Write("<td>03 08</td>");
        output.Write("<td>03 09</td>");


        output.Write("<td>04 09</td>");
        output.Write("<td>05 09</td>");
        output.Write("<td>06 09</td>");
        output.Write("<td>07 09</td>");
        output.Write("<td>06 11</td>");

        output.Write("<td>&nbsp;</td>");
        output.Write("<td>&nbsp;</td>");
        output.Write("<td>&nbsp;</td>");
        output.Write("<td>&nbsp;</td>");

        output.Write("</tr  >");

        output.Write("<tr  >");

        output.Write("<td height='19'>&nbsp;</td>");
        output.Write("<td>&nbsp;</td>");
        output.Write("<td height='15'>&nbsp;</td>");
        output.Write("<td>&nbsp;</td>");
        output.Write("<td>&nbsp;</td>");

        output.Write("<td>&nbsp;</td>");
        output.Write("<td height='15'>04 05</td>");
        output.Write("<td>04 06</td>");
        output.Write("<td>04 07</td>");
        output.Write("<td>04 08</td>");


        output.Write("<td>05 08</td>");
        output.Write("<td>06 08</td>");
        output.Write("<td>07 08</td>");
        output.Write("<td>&nbsp;</td>");
        output.Write("<td>&nbsp;</td>");

        output.Write("<td>&nbsp;</td>");
        output.Write("<td>&nbsp;</td>");
        output.Write("<td>&nbsp;</td>");
        output.Write("<td>&nbsp;</td>");

        output.Write("</tr  >");

        output.Write("<tr  >");

        output.Write("<td height='19'>&nbsp;</td>");
        output.Write("<td>&nbsp;</td>");
        output.Write("<td height='15'>&nbsp;</td>");
        output.Write("<td>&nbsp;</td>");
        output.Write("<td>&nbsp;</td>");

        output.Write("<td>&nbsp;</td>");
        output.Write("<td height='15'>&nbsp;</td>");
        output.Write("<td>&nbsp;</td>");
        output.Write("<td>05 06</td>");
        output.Write("<td>05 07</td>");


        output.Write("<td>06 07</td>");
        output.Write("<td>&nbsp;</td>");
        output.Write("<td>&nbsp;</td>");
        output.Write("<td>&nbsp;</td>");
        output.Write("<td>&nbsp;</td>");

        output.Write("<td>&nbsp;</td>");
        output.Write("<td>&nbsp;</td>");
        output.Write("<td>&nbsp;</td>");
        output.Write("<td>&nbsp;</td>");

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
            e.Row.Cells[3].CssClass = "cbg1";
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
