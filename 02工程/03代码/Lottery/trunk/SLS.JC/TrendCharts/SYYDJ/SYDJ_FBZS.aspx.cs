using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Xml;

public partial class DefaultFB :TrendChartPageBase
{
    private int dtcount = 0;
    private int[] allCount = new int[29];
    private int[] avgCount = new int[29];
    private int[] maxCount = new int[29];
    private int[] lianCount = new int[29];

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DataBind(1,1);
            ColorBind();
        }
    }
    private void DataBind(int DaySpan,int Type)
    {
        int returnValue = 0;
        string returnDescription = "";
        TrendChart tc = new TrendChart();
        DataTable dt = tc.SYDJ_FBZS(DaySpan, Type, ref returnValue, ref returnDescription);
        dtcount = dt.Rows.Count + 13;

        gvtable.DataSource = dt;
        gvtable.DataBind();
       
    }

    private void ColorBind()
    {

        try
        {
            for (int j = 0; j < gvtable.Rows.Count; j++)
            {
                for (int i = 6; i <= 16; i++)
                {
                    if (Convert.ToInt32(gvtable.Rows[j].Cells[i].Text) <= 0)
                    {
                        gvtable.Rows[j].Cells[i].CssClass = "chartBall04";


                        allCount[i - 6] = allCount[i - 6] + 1;

                        if (Convert.ToInt32(gvtable.Rows[j].Cells[i].Text) * -1 > lianCount[i - 6])
                        {
                            lianCount[i - 6] = Convert.ToInt32(gvtable.Rows[j].Cells[i].Text) * -1;
                        }

                        gvtable.Rows[j].Cells[i].Text = Convert.ToString(i - 5);
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

                        if (Convert.ToInt32(gvtable.Rows[j].Cells[i].Text) > maxCount[i - 6])
                        {
                            maxCount[i - 6] = Convert.ToInt32(gvtable.Rows[j].Cells[i].Text);
                        }
                    }
                }

                for (int i = 17; i <= 22; i++)
                {
                    if (Convert.ToInt32(gvtable.Rows[j].Cells[i].Text) <= 0)
                    {
                        gvtable.Rows[j].Cells[i].CssClass = "cbg4";

                        allCount[i - 6] = allCount[i - 6] + 1;

                        if (Convert.ToInt32(gvtable.Rows[j].Cells[i].Text) * -1 > lianCount[i - 6])
                        {
                            lianCount[i - 6] = Convert.ToInt32(gvtable.Rows[j].Cells[i].Text) * -1;
                        }

                        gvtable.Rows[j].Cells[i].Text = Convert.ToString(i - 17);
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

                        if (Convert.ToInt32(gvtable.Rows[j].Cells[i].Text) > maxCount[i - 6])
                        {
                            maxCount[i - 6] = Convert.ToInt32(gvtable.Rows[j].Cells[i].Text);
                        }
                    }
                }
                for (int i = 23; i <= 28; i++)
                {
                    if (Convert.ToInt32(gvtable.Rows[j].Cells[i].Text) <= 0)
                    {
                        gvtable.Rows[j].Cells[i].CssClass = "cbg5";


                        allCount[i - 6] = allCount[i - 6] + 1;

                        if (Convert.ToInt32(gvtable.Rows[j].Cells[i].Text) * -1 > lianCount[i - 6])
                        {
                            lianCount[i - 6] = Convert.ToInt32(gvtable.Rows[j].Cells[i].Text) * -1;
                        }

                        gvtable.Rows[j].Cells[i].Text = Convert.ToString(i - 23);
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

                        if (Convert.ToInt32(gvtable.Rows[j].Cells[i].Text) > maxCount[i - 6])
                        {
                            maxCount[i - 6] = Convert.ToInt32(gvtable.Rows[j].Cells[i].Text);
                        }
                    }
                }
                for (int i = 29; i <= 34; i++)
                {
                    if (Convert.ToInt32(gvtable.Rows[j].Cells[i].Text) <= 0)
                    {
                        gvtable.Rows[j].Cells[i].CssClass = "cbg4";



                        allCount[i - 6] = allCount[i - 6] + 1;

                        if (Convert.ToInt32(gvtable.Rows[j].Cells[i].Text) * -1 > lianCount[i - 6])
                        {
                            lianCount[i - 6] = Convert.ToInt32(gvtable.Rows[j].Cells[i].Text) * -1;
                        }

                        gvtable.Rows[j].Cells[i].Text = Convert.ToString(i - 29);
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

                        if (Convert.ToInt32(gvtable.Rows[j].Cells[i].Text) > maxCount[i - 6])
                        {
                            maxCount[i - 6] = Convert.ToInt32(gvtable.Rows[j].Cells[i].Text);
                        }
                    }
                }

            }

            for (int i = 0; i < 29; i++)
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
        catch (Exception ex)
        {
            new Log("System").Write("SYDJ_FBZS.aspx" + ex.Message + " 页面异常");
        }

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
                e.Row.Cells[4].CssClass = "chartBall01";
                e.Row.Cells[5].CssClass = "chartBall01";
                 
            }
 
        }
        
        protected void DrawGridHeader(HtmlTextWriter output, Control ctl)
        {
            
            output.Write("<td rowspan='2'  width='80px' align='middle'  >期号</td>");
            output.Write("<td rowspan='2' class='cfont1' rowSpan='2' colSpan='5'  >开奖号码</td>");
            output.Write("<TD style='BACKGROUND: #ccc' rowspan = " + dtcount.ToString() + " width=2>&nbsp;</TD>");

            output.Write("<TD colSpan=11><STRONG>开奖号码分布图</STRONG></TD>");
            output.Write("<TD style='BACKGROUND: #ccc' rowspan = "+dtcount.ToString()+" width=2>&nbsp;</TD>");

            output.Write("<TD colSpan=6><STRONG>奇数个数遗漏 </STRONG></TD>");
            output.Write("<TD style='BACKGROUND: #ccc' rowspan = " + dtcount.ToString() + " width=2>&nbsp;</TD>"); 

            output.Write("<TD colSpan=6><STRONG>小数个数遗漏 </STRONG></TD>");
            output.Write("<TD style='BACKGROUND: #ccc' rowspan = " + dtcount.ToString() + " width=2>&nbsp;</TD>");

            output.Write("<TD colSpan=6><STRONG>质数个数遗漏 </STRONG></TD>");
            output.Write("<TD style='BACKGROUND: #ccc' rowspan = " + dtcount.ToString() + " width=2>&nbsp;</TD>");

            output.Write("<tr class= 'thbg01' >");

            output.Write("<TD width=26><STRONG>1</STRONG></TD>");
            output.Write("<TD width=26><STRONG>2</STRONG></TD>");
            output.Write("<TD width=26><STRONG>3</STRONG></TD>");
            output.Write("<TD width=26><STRONG>4</STRONG></TD>");
            output.Write("<TD width=26><STRONG>5</STRONG></TD>");
            output.Write("<TD width=26><STRONG>6</STRONG></TD>");
            output.Write("<TD width=26><STRONG>7</STRONG></TD>");
            output.Write("<TD width=26><STRONG>8</STRONG></TD>");
            output.Write("<TD width=26><STRONG>9</STRONG></TD>");
            output.Write("<TD width=26><STRONG>10</STRONG></TD>");
            output.Write("<TD width=26><STRONG>11</STRONG></TD>");
            output.Write("<TD width=26><STRONG>0</STRONG></TD>");
            output.Write("<TD width=26><STRONG>1</STRONG></TD>");
            output.Write("<TD width=26><STRONG>2</STRONG></TD>");
            output.Write("<TD width=26><STRONG>3</STRONG></TD>");
            output.Write("<TD width=26><STRONG>4</STRONG></TD>");
            output.Write("<TD width=26><STRONG>5</STRONG></TD>");
            output.Write("<TD width=26><STRONG>0</STRONG></TD>");
            output.Write("<TD width=26><STRONG>1</STRONG></TD>");
            output.Write("<TD width=26><STRONG>2</STRONG></TD>");
            output.Write("<TD width=26><STRONG>3</STRONG></TD>");
            output.Write("<TD width=26><STRONG>4</STRONG></TD>");
            output.Write("<TD width=26><STRONG>5</STRONG></TD>");
            output.Write("<TD width=26><STRONG>0</STRONG></TD>");
            output.Write("<TD width=26><STRONG>1</STRONG></TD>");
            output.Write("<TD width=26><STRONG>2</STRONG></TD>");
            output.Write("<TD width=26><STRONG>3</STRONG></TD>");
            output.Write("<TD width=26><STRONG>4</STRONG></TD>");
            output.Write("<TD width=26><STRONG>5</STRONG></TD>");


            output.Write("</tr  >");

             
        }

        protected void DrawGridFooter(HtmlTextWriter output, Control ctl)
        {
            output.Write("<td  height='36px'>预选1</td>");
            output.Write("<td >&nbsp;</td>");
            output.Write("<td >&nbsp;</td>");
            output.Write("<td >&nbsp;</td>");
            output.Write("<td >&nbsp;</td>");
            output.Write("<td >&nbsp;</td>");

            for (int i = 0; i < 11; i++)
            {
                output.Write("<td onClick=ShowImg(this," + Convert.ToString(i + 1) + ",1)  style='cursor:pointer;'>&nbsp;</td>");
            }
            for (int i = 0; i < 6; i++)
            {
                output.Write("<td onClick=ShowImg(this," + i.ToString() + ",2)  style='cursor:pointer;'>&nbsp;</td>");
            }
            for (int i = 0; i < 6; i++)
            {
                output.Write("<td onClick=ShowImg(this," + i.ToString() + ",3)  style='cursor:pointer;'>&nbsp;</td>");
            }
            for (int i = 0; i < 6; i++)
            {
                output.Write("<td onClick=ShowImg(this," + i.ToString() + ",2)  style='cursor:pointer;'>&nbsp;</td>");
            }

            output.Write("<tr  >");
            output.Write("<td  height='36px'>预选2</td>");
            output.Write("<td >&nbsp;</td>");
            output.Write("<td >&nbsp;</td>");
            output.Write("<td >&nbsp;</td>");
            output.Write("<td >&nbsp;</td>");
            output.Write("<td >&nbsp;</td>");

            for (int i = 0; i < 11; i++)
            {
                output.Write("<td onClick=ShowImg(this," + Convert.ToString(i + 1) + ",1)  style='cursor:pointer;'>&nbsp;</td>");
            }
            for (int i = 0; i < 6; i++)
            {
                output.Write("<td onClick=ShowImg(this," + i.ToString() + ",2)  style='cursor:pointer;'>&nbsp;</td>");
            }
            for (int i = 0; i < 6; i++)
            {
                output.Write("<td onClick=ShowImg(this," + i.ToString() + ",3)  style='cursor:pointer;'>&nbsp;</td>");
            }
            for (int i = 0; i < 6; i++)
            {
                output.Write("<td onClick=ShowImg(this," + i.ToString() + ",2)  style='cursor:pointer;'>&nbsp;</td>");
            }
            output.Write("</tr  >");

            output.Write("<tr  >");
            output.Write("<td  height='36px'>预选3</td>");
            output.Write("<td >&nbsp;</td>");
            output.Write("<td >&nbsp;</td>");
            output.Write("<td >&nbsp;</td>");
            output.Write("<td >&nbsp;</td>");
            output.Write("<td >&nbsp;</td>");

            for (int i = 0; i < 11; i++)
            {
                output.Write("<td onClick=ShowImg(this," + Convert.ToString(i + 1) + ",1)  style='cursor:pointer;'>&nbsp;</td>");
            }
            for (int i = 0; i < 6; i++)
            {
                output.Write("<td onClick=ShowImg(this," + i.ToString() + ",2)  style='cursor:pointer;'>&nbsp;</td>");
            }
            for (int i = 0; i < 6; i++)
            {
                output.Write("<td onClick=ShowImg(this," + i.ToString() + ",3)  style='cursor:pointer;'>&nbsp;</td>");
            }
            for (int i = 0; i < 6; i++)
            {
                output.Write("<td onClick=ShowImg(this," + i.ToString() + ",2)  style='cursor:pointer;'>&nbsp;</td>");
            }
            output.Write("</tr  >");

            output.Write("<tr  >");
            output.Write("<td  height='36px'>预选4</td>");
            output.Write("<td >&nbsp;</td>");
            output.Write("<td >&nbsp;</td>");
            output.Write("<td >&nbsp;</td>");
            output.Write("<td >&nbsp;</td>");
            output.Write("<td >&nbsp;</td>");

            for (int i = 0; i < 11; i++)
            {
                output.Write("<td onClick=ShowImg(this,"+Convert.ToString(i+1)+",1)  style='cursor:pointer;'>&nbsp;</td>");
            }
            for (int i = 0; i < 6; i++)
            {
                output.Write("<td onClick=ShowImg(this,"+i.ToString()+",2)  style='cursor:pointer;'>&nbsp;</td>");
            }
            for (int i = 0; i < 6; i++)
            {
                output.Write("<td onClick=ShowImg(this," + i.ToString() + ",3)  style='cursor:pointer;'>&nbsp;</td>");
            }
            for (int i = 0; i < 6; i++)
            {
                output.Write("<td onClick=ShowImg(this," + i.ToString() + ",2)  style='cursor:pointer;'>&nbsp;</td>");
            }
            output.Write("</tr  >");

            output.Write("<tr  >");
            output.Write("<td  height='36px'>预选5</td>");
            output.Write("<td >&nbsp;</td>");
            output.Write("<td >&nbsp;</td>");
            output.Write("<td >&nbsp;</td>");
            output.Write("<td >&nbsp;</td>");
            output.Write("<td >&nbsp;</td>");

            for (int i = 0; i < 11; i++)
            {
                output.Write("<td onClick=ShowImg(this," + Convert.ToString(i + 1) + ",1)  style='cursor:pointer;'>&nbsp;</td>");
            }
            for (int i = 0; i < 6; i++)
            {
                output.Write("<td onClick=ShowImg(this," + i.ToString() + ",2)  style='cursor:pointer;'>&nbsp;</td>");
            }
            for (int i = 0; i < 6; i++)
            {
                output.Write("<td onClick=ShowImg(this," + i.ToString() + ",3)  style='cursor:pointer;'>&nbsp;</td>");
            }
            for (int i = 0; i < 6; i++)
            {
                output.Write("<td onClick=ShowImg(this," + i.ToString() + ",2)  style='cursor:pointer;'>&nbsp;</td>");
            }
            output.Write("</tr  >");

            //===============================总共出现几次 平均遗漏值 最大遗漏值 最大连出值 cfont_small
            output.Write("<tr  >");
            output.Write("<td  height='36px'>出现总次数</td>");
            output.Write("<td >&nbsp;</td>");
            output.Write("<td >&nbsp;</td>");
            output.Write("<td >&nbsp;</td>");
            output.Write("<td >&nbsp;</td>");
            output.Write("<td >&nbsp;</td>");

            for (int i = 0; i < 29; i++)
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
            output.Write("<td >&nbsp;</td>");
            output.Write("<td >&nbsp;</td>");

            for (int i = 0; i < 29; i++)
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
            output.Write("<td >&nbsp;</td>");
            output.Write("<td >&nbsp;</td>");

            for (int i = 0; i < 29; i++)
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
            output.Write("<td >&nbsp;</td>");
            output.Write("<td >&nbsp;</td>");

            for (int i = 0; i < 29; i++)
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
            output.Write("<td rowspan='2' class='td_bg'   width='80px' align='middle'  > &nbsp;</td>");
            output.Write("<td rowspan='2' class='cfont1 td_bg'  rowSpan='2' colSpan='5'  >开奖号码</td>");
         
 
            output.Write("<TD width=26><STRONG>1</STRONG></TD>");
            output.Write("<TD width=26><STRONG>2</STRONG></TD>");
            output.Write("<TD width=26><STRONG>3</STRONG></TD>");
            output.Write("<TD width=26><STRONG>4</STRONG></TD>");
            output.Write("<TD width=26><STRONG>5</STRONG></TD>");
            output.Write("<TD width=26><STRONG>6</STRONG></TD>");
            output.Write("<TD width=26><STRONG>7</STRONG></TD>");
            output.Write("<TD width=26><STRONG>8</STRONG></TD>");
            output.Write("<TD width=26><STRONG>9</STRONG></TD>");
            output.Write("<TD width=26><STRONG>10</STRONG></TD>");
            output.Write("<TD width=26><STRONG>11</STRONG></TD>");
            output.Write("<TD width=26><STRONG>0</STRONG></TD>");
            output.Write("<TD width=26><STRONG>1</STRONG></TD>");
            output.Write("<TD width=26><STRONG>2</STRONG></TD>");
            output.Write("<TD width=26><STRONG>3</STRONG></TD>");
            output.Write("<TD width=26><STRONG>4</STRONG></TD>");
            output.Write("<TD width=26><STRONG>5</STRONG></TD>");
            output.Write("<TD width=26><STRONG>0</STRONG></TD>");
            output.Write("<TD width=26><STRONG>1</STRONG></TD>");
            output.Write("<TD width=26><STRONG>2</STRONG></TD>");
            output.Write("<TD width=26><STRONG>3</STRONG></TD>");
            output.Write("<TD width=26><STRONG>4</STRONG></TD>");
            output.Write("<TD width=26><STRONG>5</STRONG></TD>");
            output.Write("<TD width=26><STRONG>0</STRONG></TD>");
            output.Write("<TD width=26><STRONG>1</STRONG></TD>");
            output.Write("<TD width=26><STRONG>2</STRONG></TD>");
            output.Write("<TD width=26><STRONG>3</STRONG></TD>");
            output.Write("<TD width=26><STRONG>4</STRONG></TD>");
            output.Write("<TD width=26><STRONG>5</STRONG></TD>");

            output.Write("</tr  >");

            output.Write("<tr  >");
            output.Write("<TD colSpan=12><STRONG>开奖号码分布图</STRONG></TD>");
            output.Write("<TD colSpan=7><STRONG>奇数个数遗漏 </STRONG></TD>");
            output.Write("<TD colSpan=7><STRONG>小数个数遗漏 </STRONG></TD>");
            output.Write("<TD colSpan=7><STRONG>质数个数遗漏 </STRONG></TD>");
             
            output.Write("</tr  >");
        }

        protected void lbtnToday_Click(object sender, EventArgs e)
        {
            DataBind(0, 0);
            ColorBind();
        }
        protected void lbtnLast_Click(object sender, EventArgs e)
        {
            DataBind(1, 0);
            ColorBind();
        }
        protected void ddlSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            int k = Convert.ToInt32(ddlSelect.Items[ddlSelect.SelectedIndex].Value);

            DataBind(k - 1, 1);
            ColorBind();
        }
    
}
