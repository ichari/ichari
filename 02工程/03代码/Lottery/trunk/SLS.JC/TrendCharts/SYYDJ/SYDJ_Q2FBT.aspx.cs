using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class DefaultQ2 : TrendChartPageBase
{

    private int dtcount = 0;
    private int[] allCount = new int[33];
    private int[] avgCount = new int[33];
    private int[] maxCount = new int[33];
    private int[] lianCount = new int[33];

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            MyDataBind(1, 1);
            ColorBind();
        }
    }

    private void MyDataBind(int DaySpan, int Type)
    {
        int returnValue = 0;
        string returnDescription = "";
        TrendChart tc = new TrendChart();
        DataTable dt = tc.SYDJ_Q2FBT(DaySpan, Type, ref returnValue, ref returnDescription);
        dtcount = dt.Rows.Count + 13;

        gvtable.DataSource = dt;
        gvtable.DataBind();
    }


    private void ColorBind()
    {


        for (int j = 0; j < gvtable.Rows.Count; j++)
        {
            for (int i = 3; i <= 13; i++)
            {
                if (Convert.ToInt32(gvtable.Rows[j].Cells[i].Text) <= 0)
                {
                    gvtable.Rows[j].Cells[i].CssClass = "chartBall04";


                    allCount[i - 3] = allCount[i - 3] + 1;

                    if (Convert.ToInt32(gvtable.Rows[j].Cells[i].Text) * -1 > lianCount[i - 3])
                    {
                        lianCount[i - 3] = Convert.ToInt32(gvtable.Rows[j].Cells[i].Text) * -1;
                    }

                    gvtable.Rows[j].Cells[i].Text = Convert.ToString(i - 2);
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

                    if (Convert.ToInt32(gvtable.Rows[j].Cells[i].Text) > maxCount[i - 3])
                    {
                        maxCount[i - 3] = Convert.ToInt32(gvtable.Rows[j].Cells[i].Text);
                    }
                }
            }

            for (int i = 14; i <= 24; i++)
            {
                if (Convert.ToInt32(gvtable.Rows[j].Cells[i].Text) <= 0)
                {
                    gvtable.Rows[j].Cells[i].CssClass = "chartBall03";


                    allCount[i - 3] = allCount[i - 3] + 1;

                    if (Convert.ToInt32(gvtable.Rows[j].Cells[i].Text) * -1 > lianCount[i - 3])
                    {
                        lianCount[i - 3] = Convert.ToInt32(gvtable.Rows[j].Cells[i].Text) * -1;
                    }

                    gvtable.Rows[j].Cells[i].Text = Convert.ToString(i - 13);
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

                    if (Convert.ToInt32(gvtable.Rows[j].Cells[i].Text) > maxCount[i -3])
                    {
                        maxCount[i - 3] = Convert.ToInt32(gvtable.Rows[j].Cells[i].Text);
                    }
                }
            }
           
            for (int i = 25; i <= 35; i++)
            {
                if (Convert.ToInt32(gvtable.Rows[j].Cells[i].Text) <= 0)
                {
                    gvtable.Rows[j].Cells[i].CssClass = "chartBall02";


                    allCount[i - 3] = allCount[i - 3] + 1;

                    if (Convert.ToInt32(gvtable.Rows[j].Cells[i].Text) * -1 > lianCount[i - 3])
                    {
                        lianCount[i - 3] = Convert.ToInt32(gvtable.Rows[j].Cells[i].Text) * -1;
                    }

                    gvtable.Rows[j].Cells[i].Text = Convert.ToString(i - 24);
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

                    if (Convert.ToInt32(gvtable.Rows[j].Cells[i].Text) > maxCount[i - 3])
                    {
                        maxCount[i - 3] = Convert.ToInt32(gvtable.Rows[j].Cells[i].Text);
                    }
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
        output.Write("<td rowspan='2' class='cfont1'  colSpan='2'  >开奖号码</td>");
        output.Write("<TD style='BACKGROUND: #ccc' rowspan = " + dtcount.ToString() + " width=2>&nbsp;</TD>");

        output.Write(" <td colspan='11'><strong>两码混合分布图</strong></td>");
        output.Write("<TD style='BACKGROUND: #ccc' rowspan = " + dtcount.ToString() + " width=2>&nbsp;</TD>");

        output.Write("<td colspan='11'><strong>第一个号码</strong></td>");
        output.Write("<TD style='BACKGROUND: #ccc' rowspan = " + dtcount.ToString() + " width=2>&nbsp;</TD>");

        output.Write("<td colspan='11'><strong>第二个号码</strong></td>");
        output.Write("<TD style='BACKGROUND: #ccc' rowspan = " + dtcount.ToString() + " width=2>&nbsp;</TD>");
 
        output.Write("<tr class= 'thbg01' >");

        for (int i = 1; i <= 11; i++)
        {
            output.Write("<TD width=25><STRONG>" + i.ToString() + "</STRONG></TD>");
        }

        for (int i = 1; i <= 11; i++)
        {
            output.Write("<TD width=25><STRONG>" + i.ToString() + "</STRONG></TD>");
        }

        for (int i = 1; i <= 11; i++)
        {
            output.Write("<TD width=25><STRONG>" + i.ToString() + "</STRONG></TD>");
        }

        output.Write("</tr  >");


    }

    protected void DrawGridFooter(HtmlTextWriter output, Control ctl)
    {
        output.Write("<td  height='36px'>预选1</td>");
        output.Write("<td >&nbsp;</td>");
        output.Write("<td >&nbsp;</td>");
     

        for (int i = 0; i <= 10; i++)
        {
            output.Write("<TD onClick=ShowImg(this," + Convert.ToString(i + 1) + ",4)  style='cursor:pointer;'>&nbsp;</TD>");
        }

        for (int i = 0; i <= 10; i++)
        {
            output.Write("<TD onClick=ShowImg(this," + Convert.ToString(i + 1) + ",3)  style='cursor:pointer;'>&nbsp;</TD>");
        }

        for (int i = 0; i <= 10; i++)
        {
            output.Write("<TD onClick=ShowImg(this," + Convert.ToString(i + 1) + ",2)  style='cursor:pointer;'>&nbsp;</TD>");
        }
 
        output.Write("<tr  >");
        output.Write("<td  height='36px'>预选2</td>");
        output.Write("<td >&nbsp;</td>");
        output.Write("<td >&nbsp;</td>");


        for (int i = 0; i <= 10; i++)
        {
            output.Write("<TD onClick=ShowImg(this," + Convert.ToString(i + 1) + ",4)  style='cursor:pointer;'>&nbsp;</TD>");
        }

        for (int i = 0; i <= 10; i++)
        {
            output.Write("<TD onClick=ShowImg(this," + Convert.ToString(i + 1) + ",3)  style='cursor:pointer;'>&nbsp;</TD>");
        }

        for (int i = 0; i <= 10; i++)
        {
            output.Write("<TD onClick=ShowImg(this," + Convert.ToString(i + 1) + ",2)  style='cursor:pointer;'>&nbsp;</TD>");
        }

        output.Write("</tr  >");

        output.Write("<tr  >");
        output.Write("<td  height='36px'>预选3</td>");
        output.Write("<td >&nbsp;</td>");
        output.Write("<td >&nbsp;</td>");


        for (int i = 0; i <= 10; i++)
        {
            output.Write("<TD onClick=ShowImg(this," + Convert.ToString(i + 1) + ",4)  style='cursor:pointer;'>&nbsp;</TD>");
        }

        for (int i = 0; i <= 10; i++)
        {
            output.Write("<TD onClick=ShowImg(this," + Convert.ToString(i + 1) + ",3)  style='cursor:pointer;'>&nbsp;</TD>");
        }

        for (int i = 0; i <= 10; i++)
        {
            output.Write("<TD onClick=ShowImg(this," + Convert.ToString(i + 1) + ",2)  style='cursor:pointer;'>&nbsp;</TD>");
        }

        output.Write("</tr  >");

        output.Write("<tr  >");
        output.Write("<td  height='36px'>预选4</td>");
        output.Write("<td >&nbsp;</td>");
        output.Write("<td >&nbsp;</td>");


        for (int i = 0; i <= 10; i++)
        {
            output.Write("<TD onClick=ShowImg(this," + Convert.ToString(i + 1) + ",4)  style='cursor:pointer;'>&nbsp;</TD>");
        }

        for (int i = 0; i <= 10; i++)
        {
            output.Write("<TD onClick=ShowImg(this," + Convert.ToString(i + 1) + ",3)  style='cursor:pointer;'>&nbsp;</TD>");
        }

        for (int i = 0; i <= 10; i++)
        {
            output.Write("<TD onClick=ShowImg(this," + Convert.ToString(i + 1) + ",2)  style='cursor:pointer;'>&nbsp;</TD>");
        }
        output.Write("</tr  >");

        output.Write("<tr  >");
        output.Write("<td  height='36px'>预选5</td>");
        output.Write("<td >&nbsp;</td>");
        output.Write("<td >&nbsp;</td>");


        for (int i = 0; i <= 10; i++)
        {
            output.Write("<TD onClick=ShowImg(this," + Convert.ToString(i + 1) + ",4)  style='cursor:pointer;'>&nbsp;</TD>");
        }

        for (int i = 0; i <= 10; i++)
        {
            output.Write("<TD onClick=ShowImg(this," + Convert.ToString(i + 1) + ",3)  style='cursor:pointer;'>&nbsp;</TD>");
        }

        for (int i = 0; i <= 10; i++)
        {
            output.Write("<TD onClick=ShowImg(this," + Convert.ToString(i + 1) + ",2)  style='cursor:pointer;'>&nbsp;</TD>");
        }
        output.Write("</tr  >");

        //===============================总共出现几次 平均遗漏值 最大遗漏值 最大连出值 cfont_small

        output.Write("<tr  >");
        output.Write("<td  height='36px'>出现总次数</td>");
        output.Write("<td >&nbsp;</td>");
        output.Write("<td >&nbsp;</td>");
 

        for (int i = 0; i < 33; i++)
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

        for (int i = 0; i < 33; i++)
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

        for (int i = 0; i < 33; i++)
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

        for (int i = 0; i < 33; i++)
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
        output.Write("<td rowspan='2' class='cfont1 td_bg' colSpan='2'  >开奖号码</td>");

        for (int i = 1; i <= 11; i++)
        {
            output.Write("<TD width=25><STRONG>" + i.ToString() + "</STRONG></TD>");
        }

        for (int i = 1; i <= 11; i++)
        {
            output.Write("<TD width=25><STRONG>" + i.ToString() + "</STRONG></TD>");
        }

        for (int i = 1; i <= 11; i++)
        {
            output.Write("<TD width=25><STRONG>" + i.ToString() + "</STRONG></TD>");
        }

        output.Write("</tr  >");

        output.Write("<tr  >");

        output.Write(" <td colspan='12'><strong>两码混合分布图</strong></td>");
 
        output.Write("<td colspan='12'><strong>第一个号码</strong></td>");
  
        output.Write("<td colspan='13'><strong>第二个号码</strong></td>");
  
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
