using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Default3HZ : TrendChartPageBase
{
    private int dtcount = 0;
    private int[] allCount = new int[38];
    private int[] avgCount = new int[38];
    private int[] maxCount = new int[38];
    private int[] lianCount = new int[38];
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
        DataTable dt = tc.SYDJ_Q3HZT(DaySpan, Type, ref returnValue, ref returnDescription);
        dtcount = dt.Rows.Count + 13;


        gvtable.DataSource = dt;
        gvtable.DataBind();
    }


    private void ColorBind()
    {
        for (int j = 0; j < gvtable.Rows.Count; j++)
        {
            for (int i = 4; i <= 41; i++)
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
                        if (i <= 28)
                        {
                            gvtable.Rows[j].Cells[i].CssClass = "chartBall04";
                            gvtable.Rows[j].Cells[i].Text = Convert.ToString(i+2);
                        }
                        if (i > 28 && i <= 38)
                        {
                            gvtable.Rows[j].Cells[i].CssClass = "cbg4";
                            gvtable.Rows[j].Cells[i].Text = Convert.ToString(i - 29);
                        }

                        if (i >= 39 && i <= 41)
                        {
                            gvtable.Rows[j].Cells[i].CssClass = "cbg8";
                            gvtable.Rows[j].Cells[i].Text = Convert.ToString(i - 39);
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
                    new Log("System").Write("SYDJ_Q3HZT.aspx" + ex.Message + " 页面异常");
                }
            }

        }
        for (int i = 0; i < 33; i++)
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
        output.Write("<td rowspan='2' class='cfont1'  colSpan='3'  >开奖号码</td>");
        output.Write("<TD style='BACKGROUND: #ccc' rowspan = " + dtcount.ToString() + " width=2>&nbsp;</TD>");

        output.Write("<td colspan='25'><strong>和值</strong></td>");
        output.Write("<TD style='BACKGROUND: #ccc' rowspan = " + dtcount.ToString() + " width=2>&nbsp;</TD>");

        output.Write("<td colspan='10'><strong>和尾</strong></td>");
        output.Write("<TD style='BACKGROUND: #ccc' rowspan = " + dtcount.ToString() + " width=2>&nbsp;</TD>");

        output.Write("<td colspan='3'><strong>除3余数</strong></td>");
        output.Write("<TD style='BACKGROUND: #ccc' rowspan = " + dtcount.ToString() + " width=2>&nbsp;</TD>");
 
        output.Write("<tr class= 'thbg01' >");

        for (int i = 6; i <= 30; i++)
        {
            output.Write("<TD width=21><STRONG>" + i.ToString() + "</STRONG></TD>");
        }

        for (int i = 0; i <= 9; i++)
        {
            output.Write("<TD width=21><STRONG>" + i.ToString() + "</STRONG></TD>");
        }

        for (int i = 0; i < 3; i++)
        {
            output.Write("<TD width=21><STRONG>" + i.ToString() + "</STRONG></TD>");
        }
 
        output.Write("</tr  >");


    }

    protected void DrawGridFooter(HtmlTextWriter output, Control ctl)
    {
        output.Write("<td  height='36px'>预选1</td>");
        output.Write("<td >&nbsp;</td>");
        output.Write("<td >&nbsp;</td>");
        output.Write("<td >&nbsp;</td>");

        for (int i = 6; i <= 30; i++)
        {
            output.Write("<TD onClick=ShowImg(this," + Convert.ToString(i) + ",4)  style='cursor:pointer;'>&nbsp;</TD>");
        }

        for (int i = 0; i <= 9; i++)
        {
            output.Write("<TD onClick=ShowImg(this," + Convert.ToString(i) + ",2)  style='cursor:pointer;'>&nbsp;</TD>");
        }

        for (int i = 0; i <= 2; i++)
        {
            output.Write("<TD onClick=ShowImg(this," + Convert.ToString(i) + ",3)  style='cursor:pointer;'>&nbsp;</TD>");
        }
  
        output.Write("<tr  >");
        output.Write("<td  height='36px'>预选2</td>");
        output.Write("<td >&nbsp;</td>");
        output.Write("<td >&nbsp;</td>");
        output.Write("<td >&nbsp;</td>");

        for (int i = 6; i <= 30; i++)
        {
            output.Write("<TD onClick=ShowImg(this," + Convert.ToString(i) + ",4)  style='cursor:pointer;'>&nbsp;</TD>");
        }

        for (int i = 0; i <= 9; i++)
        {
            output.Write("<TD onClick=ShowImg(this," + Convert.ToString(i) + ",2)  style='cursor:pointer;'>&nbsp;</TD>");
        }

        for (int i = 0; i <= 2; i++)
        {
            output.Write("<TD onClick=ShowImg(this," + Convert.ToString(i) + ",3)  style='cursor:pointer;'>&nbsp;</TD>");
        }
        output.Write("</tr  >");

        output.Write("<tr  >");
        output.Write("<td  height='36px'>预选3</td>");
        output.Write("<td >&nbsp;</td>");
        output.Write("<td >&nbsp;</td>");
        output.Write("<td >&nbsp;</td>");

        for (int i = 6; i <= 30; i++)
        {
            output.Write("<TD onClick=ShowImg(this," + Convert.ToString(i) + ",4)  style='cursor:pointer;'>&nbsp;</TD>");
        }

        for (int i = 0; i <= 9; i++)
        {
            output.Write("<TD onClick=ShowImg(this," + Convert.ToString(i) + ",2)  style='cursor:pointer;'>&nbsp;</TD>");
        }

        for (int i = 0; i <= 2; i++)
        {
            output.Write("<TD onClick=ShowImg(this," + Convert.ToString(i) + ",3)  style='cursor:pointer;'>&nbsp;</TD>");
        }

        output.Write("</tr  >");

        output.Write("<tr  >");
        output.Write("<td  height='36px'>预选4</td>");
        output.Write("<td >&nbsp;</td>");
        output.Write("<td >&nbsp;</td>");
        output.Write("<td >&nbsp;</td>");

        for (int i = 6; i <= 30; i++)
        {
            output.Write("<TD onClick=ShowImg(this," + Convert.ToString(i) + ",4)  style='cursor:pointer;'>&nbsp;</TD>");
        }

        for (int i = 0; i <= 9; i++)
        {
            output.Write("<TD onClick=ShowImg(this," + Convert.ToString(i) + ",2)  style='cursor:pointer;'>&nbsp;</TD>");
        }

        for (int i = 0; i <= 2; i++)
        {
            output.Write("<TD onClick=ShowImg(this," + Convert.ToString(i) + ",3)  style='cursor:pointer;'>&nbsp;</TD>");
        }

        output.Write("</tr  >");

        output.Write("<tr  >");
        output.Write("<td  height='36px'>预选5</td>");
        output.Write("<td >&nbsp;</td>");
        output.Write("<td >&nbsp;</td>");
        output.Write("<td >&nbsp;</td>");

        for (int i = 6; i <= 30; i++)
        {
            output.Write("<TD onClick=ShowImg(this," + Convert.ToString(i) + ",4)  style='cursor:pointer;'>&nbsp;</TD>");
        }

        for (int i = 0; i <= 9; i++)
        {
            output.Write("<TD onClick=ShowImg(this," + Convert.ToString(i) + ",2)  style='cursor:pointer;'>&nbsp;</TD>");
        }

        for (int i = 0; i <= 2; i++)
        {
            output.Write("<TD onClick=ShowImg(this," + Convert.ToString(i) + ",3)  style='cursor:pointer;'>&nbsp;</TD>");
        }

        output.Write("</tr  >");

        //===============================总共出现几次 平均遗漏值 最大遗漏值 最大连出值 cfont_small

        output.Write("<tr  >");
        output.Write("<td  height='36px'>出现总次数</td>");
        output.Write("<td >&nbsp;</td>");
        output.Write("<td >&nbsp;</td>");
        output.Write("<td >&nbsp;</td>");

        for (int i = 0; i < 38; i++)
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

        output.Write("</tr  >");

        output.Write("<tr  >");
        output.Write("<td  height='36px'>平均遗漏值</td>");
        output.Write("<td >&nbsp;</td>");
        output.Write("<td >&nbsp;</td>");
        output.Write("<td >&nbsp;</td>");

        for (int i = 0; i < 38; i++)
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

        for (int i = 0; i < 38; i++)
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

        for (int i = 0; i < 38; i++)
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
        output.Write("<td rowspan='2' class='td_bg'   width='80px' align='middle'  > &nbsp;</td>");
        output.Write("<td rowspan='2' class='cfont1 td_bg' colSpan='3'  >开奖号码</td>");

        for (int i = 6; i <= 30; i++)
        {
            output.Write("<TD width=21><STRONG>" + i.ToString() + "</STRONG></TD>");
        }

        for (int i = 0; i <= 9; i++)
        {
            output.Write("<TD width=21><STRONG>" + i.ToString() + "</STRONG></TD>");
        }

        for (int i = 0; i < 3; i++)
        {
            output.Write("<TD width=21><STRONG>" + i.ToString() + "</STRONG></TD>");
        }

        output.Write("</tr  >");

        output.Write("<tr  >");


        output.Write("<td colspan='25'><strong>和值</strong></td>");
 
        output.Write("<td colspan='10'><strong>和尾</strong></td>");
 
        output.Write("<td colspan='3'><strong>除3余数</strong></td>");
 
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
