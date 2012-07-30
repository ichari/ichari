using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class _Default : TrendChartPageBase 
{
    private int dtcount = 0;
    private int[] allCount = new int[24];
    private int[] avgCount = new int[24];
    private int[] maxCount = new int[24];
    private int[] lianCount = new int[24];
    private static int WeiSpan = 1;
    private static int WeiType = 1;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            MyDataBind(1, 1, 1);
            ColorBind();
        }
    }
    protected void lbToday_Click(object sender, EventArgs e)
    {

    }
    protected void lbLastDay_Click(object sender, EventArgs e)
    {

    }
    protected void ddlNearDay_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    private void MyDataBind(int dataSpan, int Type,int number)
    {
        int returnValue = 0;
        string returnDescription = "";

        TrendChart tc = new TrendChart();
        DataTable dt = tc.SYDJ_DWZS(dataSpan,Type,number,ref returnValue ,ref returnDescription);
        
        dtcount = dt.Rows.Count + 13;

        gvtable.DataSource = dt;
        gvtable.DataBind();
    }

    private void ColorBind()
    {

        for (int j = 0; j < gvtable.Rows.Count; j++)
        {
            for (int i = 1; i <= 11; i++)
            {
                if (Convert.ToInt32(gvtable.Rows[j].Cells[i].Text) <= 0)
                {
                    gvtable.Rows[j].Cells[i].CssClass = "chartBall01";


                    allCount[i - 1] = allCount[i - 1] + 1;

                    if (Convert.ToInt32(gvtable.Rows[j].Cells[i].Text) * -1 > lianCount[i - 1])
                    {
                        lianCount[i - 1] = Convert.ToInt32(gvtable.Rows[j].Cells[i].Text) * -1;
                    }

                    gvtable.Rows[j].Cells[i].Text = Convert.ToString(i);
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

                    if (Convert.ToInt32(gvtable.Rows[j].Cells[i].Text) > maxCount[i - 1])
                    {
                        maxCount[i - 1] = Convert.ToInt32(gvtable.Rows[j].Cells[i].Text);
                    }
                }
            }

            for (int i = 12; i <= 17; i++)
            {
                if (Convert.ToInt32(gvtable.Rows[j].Cells[i].Text) <= 0)
                {
                    gvtable.Rows[j].Cells[i].CssClass = "cbg4";

                    allCount[i - 1] = allCount[i - 1] + 1;

                    if (Convert.ToInt32(gvtable.Rows[j].Cells[i].Text) * -1 > lianCount[i - 1])
                    {
                        lianCount[i - 1] = Convert.ToInt32(gvtable.Rows[j].Cells[i].Text) * -1;
                    }

                    if (i == 12)
                    {
                        gvtable.Rows[j].Cells[i].Text = "小奇质";
                    }
                    if (i == 13)
                    {
                        gvtable.Rows[j].Cells[i].Text = "小偶质";
                    }
                    if (i == 14)
                    {
                        gvtable.Rows[j].Cells[i].Text = "小偶合";
                    }
                    if (i == 15)
                    {
                        gvtable.Rows[j].Cells[i].Text = "大奇质";
                    }
                    if (i == 16)
                    {
                        gvtable.Rows[j].Cells[i].Text = "大奇合";
                    }
                    if (i == 17)
                    {
                        gvtable.Rows[j].Cells[i].Text = "大偶合";
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

                    if (Convert.ToInt32(gvtable.Rows[j].Cells[i].Text) > maxCount[i - 1])
                    {
                        maxCount[i - 1] = Convert.ToInt32(gvtable.Rows[j].Cells[i].Text);
                    }
                }
            }
            for (int i = 18; i <= 20; i++)
            {
                if (Convert.ToInt32(gvtable.Rows[j].Cells[i].Text) <= 0)
                {
                    gvtable.Rows[j].Cells[i].CssClass = "cbg3";


                    allCount[i - 1] = allCount[i - 1] + 1;

                    if (Convert.ToInt32(gvtable.Rows[j].Cells[i].Text) * -1 > lianCount[i - 1])
                    {
                        lianCount[i - 1] = Convert.ToInt32(gvtable.Rows[j].Cells[i].Text) * -1;
                    }
                   
                    gvtable.Rows[j].Cells[i].Text = Convert.ToString(i - 18);
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

                    if (Convert.ToInt32(gvtable.Rows[j].Cells[i].Text) > maxCount[i - 1])
                    {
                        maxCount[i - 1] = Convert.ToInt32(gvtable.Rows[j].Cells[i].Text);
                    }
                }
            }

            for (int i = 21; i <= 24; i++)
            {
                try
                {
                    if (Convert.ToInt32(gvtable.Rows[j].Cells[i].Text) <= 0)
                    {
                        gvtable.Rows[j].Cells[i].CssClass = "cbg1";



                        allCount[i - 1] = allCount[i - 1] + 1;

                        if (Convert.ToInt32(gvtable.Rows[j].Cells[i].Text) * -1 > lianCount[i - 1])
                        {
                            lianCount[i - 1] = Convert.ToInt32(gvtable.Rows[j].Cells[i].Text) * -1;
                        }
                        switch (i)
                        {
                            case 21:
                                gvtable.Rows[j].Cells[i].Text = "重";
                                break;
                            case 22:
                                gvtable.Rows[j].Cells[i].Text = "邻";
                                break;
                            case 23:
                                gvtable.Rows[j].Cells[i].Text = "间";
                                break;
                            case 24:
                                gvtable.Rows[j].Cells[i].Text = "孤";
                                break;
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

                        if (Convert.ToInt32(gvtable.Rows[j].Cells[i].Text) > maxCount[i - 1])
                        {
                            maxCount[i - 1] = Convert.ToInt32(gvtable.Rows[j].Cells[i].Text);
                        }
                    }
                }
                catch (Exception ex)
                {
                    new Log("System").Write("SYDJ_DWZS.aspx" + ex.Message + " 页面异常");
                }
            }

        }

        for (int i = 0; i < 24; i++)
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
            
        }
    }

    protected void DrawGridHeader(HtmlTextWriter output, Control ctl)
    {

        output.Write("<td width='80' rowspan='2' align='center'  ><strong>期号</strong></td>");
        output.Write("<td colspan='11'><strong>开奖号码分布图</strong></td>");
        output.Write("<TD style='BACKGROUND: #ccc' rowspan = " + dtcount.ToString() + " width=2>&nbsp;</TD>");

        output.Write("<td colspan='6'><strong>数字特征</strong></td>");
        output.Write("<TD style='BACKGROUND: #ccc' rowspan = " + dtcount.ToString() + " width=2>&nbsp;</TD>");

        output.Write("<td colspan='3'><strong>除3余数</strong></td>");
        output.Write("<TD style='BACKGROUND: #ccc' rowspan = " + dtcount.ToString() + " width=2>&nbsp;</TD>");

        output.Write("<td colspan='5'><strong>重邻间孤</strong></td>");
        
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

        output.Write("<TD width=45><STRONG>小奇质</STRONG></TD>");
        output.Write("<TD width=45><STRONG>小偶质</STRONG></TD>");
        output.Write("<TD width=45><STRONG>小偶合</STRONG></TD>");
        output.Write("<TD width=45><STRONG>大奇质</STRONG></TD>");
        output.Write("<TD width=45><STRONG>大奇合</STRONG></TD>");
        output.Write("<TD width=45><STRONG>大偶合</STRONG></TD>");
        output.Write("<TD width=45><STRONG>余0</STRONG></TD>");
        output.Write("<TD width=45><STRONG>余1</STRONG></TD>");
        output.Write("<TD width=45><STRONG>余2</STRONG></TD>");
        output.Write("<TD width=45><STRONG>重</STRONG></TD>");
        output.Write("<TD width=45><STRONG>邻</STRONG></TD>");
        output.Write("<TD width=45><STRONG>间</STRONG></TD>");
        output.Write("<TD width=45><STRONG>孤</STRONG></TD>");
 
        output.Write("</tr  >");


    }

    protected void DrawGridFooter(HtmlTextWriter output, Control ctl)
    {
        output.Write("<td  height='36px'>预选1</td>");
 

        for (int i = 0; i < 11; i++)
        {
            output.Write("<td onClick=ShowImg(this," + Convert.ToString(i + 1) + ",1)  style='cursor:pointer;'>&nbsp;</td>");
        }
        output.Write("<td onClick=ShowImg(this,'" + "小奇质" + "',2)  style='cursor:pointer;'>&nbsp;</td>");
        output.Write("<td onClick=ShowImg(this,'" + "小偶质" + "',2)  style='cursor:pointer;'>&nbsp;</td>");
        output.Write("<td onClick=ShowImg(this,'" + "小偶合" + "',2)  style='cursor:pointer;'>&nbsp;</td>");
        output.Write("<td onClick=ShowImg(this,'" + "大奇质" + "',2)  style='cursor:pointer;'>&nbsp;</td>");
        output.Write("<td onClick=ShowImg(this,'" + "大奇合" + "',2)  style='cursor:pointer;'>&nbsp;</td>");
        output.Write("<td onClick=ShowImg(this,'" + "大偶合" + "',2)  style='cursor:pointer;'>&nbsp;</td>");
         
        for (int i = 0; i < 3; i++)
        {
            output.Write("<td onClick=ShowImg(this," + i.ToString() + ",3)  style='cursor:pointer;'>&nbsp;</td>");
        }

        output.Write("<td onClick=ShowImg(this,'" + "重" + "',2)  style='cursor:pointer;'>&nbsp;</td>");
        output.Write("<td onClick=ShowImg(this,'" + "邻" + "',2)  style='cursor:pointer;'>&nbsp;</td>");
        output.Write("<td onClick=ShowImg(this,'" + "间" + "',2)  style='cursor:pointer;'>&nbsp;</td>");
        output.Write("<td onClick=ShowImg(this,'" + "孤" + "',2)  style='cursor:pointer;'>&nbsp;</td>");

        output.Write("<tr  >");
        output.Write("<td  height='36px'>预选2</td>");
 

        for (int i = 0; i < 11; i++)
        {
            output.Write("<td onClick=ShowImg(this," + Convert.ToString(i + 1) + ",1)  style='cursor:pointer;'>&nbsp;</td>");
        }
        output.Write("<td onClick=ShowImg(this,'" + "小奇质" + "',2)  style='cursor:pointer;'>&nbsp;</td>");
        output.Write("<td onClick=ShowImg(this,'" + "小偶质" + "',2)  style='cursor:pointer;'>&nbsp;</td>");
        output.Write("<td onClick=ShowImg(this,'" + "小偶合" + "',2)  style='cursor:pointer;'>&nbsp;</td>");
        output.Write("<td onClick=ShowImg(this,'" + "大奇质" + "',2)  style='cursor:pointer;'>&nbsp;</td>");
        output.Write("<td onClick=ShowImg(this,'" + "大奇合" + "',2)  style='cursor:pointer;'>&nbsp;</td>");
        output.Write("<td onClick=ShowImg(this,'" + "大偶合" + "',2)  style='cursor:pointer;'>&nbsp;</td>");

        for (int i = 0; i < 3; i++)
        {
            output.Write("<td onClick=ShowImg(this," + i.ToString() + ",3)  style='cursor:pointer;'>&nbsp;</td>");
        }

        output.Write("<td onClick=ShowImg(this,'" + "重" + "',2)  style='cursor:pointer;'>&nbsp;</td>");
        output.Write("<td onClick=ShowImg(this,'" + "邻" + "',2)  style='cursor:pointer;'>&nbsp;</td>");
        output.Write("<td onClick=ShowImg(this,'" + "间" + "',2)  style='cursor:pointer;'>&nbsp;</td>");
        output.Write("<td onClick=ShowImg(this,'" + "孤" + "',2)  style='cursor:pointer;'>&nbsp;</td>");
        output.Write("</tr  >");

        output.Write("<tr  >");
        output.Write("<td  height='36px'>预选3</td>");
 

        for (int i = 0; i < 11; i++)
        {
            output.Write("<td onClick=ShowImg(this," + Convert.ToString(i + 1) + ",1)  style='cursor:pointer;'>&nbsp;</td>");
        }
        output.Write("<td onClick=ShowImg(this,'" + "小奇质" + "',2)  style='cursor:pointer;'>&nbsp;</td>");
        output.Write("<td onClick=ShowImg(this,'" + "小偶质" + "',2)  style='cursor:pointer;'>&nbsp;</td>");
        output.Write("<td onClick=ShowImg(this,'" + "小偶合" + "',2)  style='cursor:pointer;'>&nbsp;</td>");
        output.Write("<td onClick=ShowImg(this,'" + "大奇质" + "',2)  style='cursor:pointer;'>&nbsp;</td>");
        output.Write("<td onClick=ShowImg(this,'" + "大奇合" + "',2)  style='cursor:pointer;'>&nbsp;</td>");
        output.Write("<td onClick=ShowImg(this,'" + "大偶合" + "',2)  style='cursor:pointer;'>&nbsp;</td>");

        for (int i = 0; i < 3; i++)
        {
            output.Write("<td onClick=ShowImg(this," + i.ToString() + ",3)  style='cursor:pointer;'>&nbsp;</td>");
        }

        output.Write("<td onClick=ShowImg(this,'" + "重" + "',2)  style='cursor:pointer;'>&nbsp;</td>");
        output.Write("<td onClick=ShowImg(this,'" + "邻" + "',2)  style='cursor:pointer;'>&nbsp;</td>");
        output.Write("<td onClick=ShowImg(this,'" + "间" + "',2)  style='cursor:pointer;'>&nbsp;</td>");
        output.Write("<td onClick=ShowImg(this,'" + "孤" + "',2)  style='cursor:pointer;'>&nbsp;</td>");
        output.Write("</tr  >");

        output.Write("<tr  >");
        output.Write("<td  height='36px'>预选4</td>");
 

        for (int i = 0; i < 11; i++)
        {
            output.Write("<td onClick=ShowImg(this," + Convert.ToString(i + 1) + ",1)  style='cursor:pointer;'>&nbsp;</td>");
        }
        output.Write("<td onClick=ShowImg(this,'" + "小奇质" + "',2)  style='cursor:pointer;'>&nbsp;</td>");
        output.Write("<td onClick=ShowImg(this,'" + "小偶质" + "',2)  style='cursor:pointer;'>&nbsp;</td>");
        output.Write("<td onClick=ShowImg(this,'" + "小偶合" + "',2)  style='cursor:pointer;'>&nbsp;</td>");
        output.Write("<td onClick=ShowImg(this,'" + "大奇质" + "',2)  style='cursor:pointer;'>&nbsp;</td>");
        output.Write("<td onClick=ShowImg(this,'" + "大奇合" + "',2)  style='cursor:pointer;'>&nbsp;</td>");
        output.Write("<td onClick=ShowImg(this,'" + "大偶合" + "',2)  style='cursor:pointer;'>&nbsp;</td>");

        for (int i = 0; i < 3; i++)
        {
            output.Write("<td onClick=ShowImg(this," + i.ToString() + ",3)  style='cursor:pointer;'>&nbsp;</td>");
        }

        output.Write("<td onClick=ShowImg(this,'" + "重" + "',2)  style='cursor:pointer;'>&nbsp;</td>");
        output.Write("<td onClick=ShowImg(this,'" + "邻" + "',2)  style='cursor:pointer;'>&nbsp;</td>");
        output.Write("<td onClick=ShowImg(this,'" + "间" + "',2)  style='cursor:pointer;'>&nbsp;</td>");
        output.Write("<td onClick=ShowImg(this,'" + "孤" + "',2)  style='cursor:pointer;'>&nbsp;</td>");
        output.Write("</tr  >");

        output.Write("<tr  >");
        output.Write("<td  height='36px'>预选5</td>");
 

        for (int i = 0; i < 11; i++)
        {
            output.Write("<td onClick=ShowImg(this," + Convert.ToString(i + 1) + ",1)  style='cursor:pointer;'>&nbsp;</td>");
        }
        output.Write("<td onClick=ShowImg(this,'" + "小奇质" + "',2)  style='cursor:pointer;'>&nbsp;</td>");
        output.Write("<td onClick=ShowImg(this,'" + "小偶质" + "',2)  style='cursor:pointer;'>&nbsp;</td>");
        output.Write("<td onClick=ShowImg(this,'" + "小偶合" + "',2)  style='cursor:pointer;'>&nbsp;</td>");
        output.Write("<td onClick=ShowImg(this,'" + "大奇质" + "',2)  style='cursor:pointer;'>&nbsp;</td>");
        output.Write("<td onClick=ShowImg(this,'" + "大奇合" + "',2)  style='cursor:pointer;'>&nbsp;</td>");
        output.Write("<td onClick=ShowImg(this,'" + "大偶合" + "',2)  style='cursor:pointer;'>&nbsp;</td>");

        for (int i = 0; i < 3; i++)
        {
            output.Write("<td onClick=ShowImg(this," + i.ToString() + ",3)  style='cursor:pointer;'>&nbsp;</td>");
        }

        output.Write("<td onClick=ShowImg(this,'" + "重" + "',2)  style='cursor:pointer;'>&nbsp;</td>");
        output.Write("<td onClick=ShowImg(this,'" + "邻" + "',2)  style='cursor:pointer;'>&nbsp;</td>");
        output.Write("<td onClick=ShowImg(this,'" + "间" + "',2)  style='cursor:pointer;'>&nbsp;</td>");
        output.Write("<td onClick=ShowImg(this,'" + "孤" + "',2)  style='cursor:pointer;'>&nbsp;</td>");
        output.Write("</tr  >");

        //===============================总共出现几次 平均遗漏值 最大遗漏值 最大连出值 cfont_small
        output.Write("<tr  >");
        output.Write("<td  height='36px'>出现总次数</td>");
 

        for (int i = 0; i < 24; i++)
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
 

        for (int i = 0; i < 24; i++)
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
 

        for (int i = 0; i < 24; i++)
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
 

        for (int i = 0; i < 24; i++)
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
        
        output.Write("<td width='80' rowspan='2' align='center'  ><strong>&nbsp;</strong></td>");

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

        output.Write("<TD width=45><STRONG>小奇质</STRONG></TD>");
        output.Write("<TD width=45><STRONG>小偶质</STRONG></TD>");
        output.Write("<TD width=45><STRONG>小偶合</STRONG></TD>");
        output.Write("<TD width=45><STRONG>大奇质</STRONG></TD>");
        output.Write("<TD width=45><STRONG>大奇合</STRONG></TD>");
        output.Write("<TD width=45><STRONG>大偶合</STRONG></TD>");
        output.Write("<TD width=45><STRONG>余0</STRONG></TD>");
        output.Write("<TD width=45><STRONG>余1</STRONG></TD>");
        output.Write("<TD width=45><STRONG>余2</STRONG></TD>");
        output.Write("<TD width=45><STRONG>重</STRONG></TD>");
        output.Write("<TD width=45><STRONG>邻</STRONG></TD>");
        output.Write("<TD width=45><STRONG>间</STRONG></TD>");
        output.Write("<TD width=45><STRONG>孤</STRONG></TD>");
        output.Write("</tr  >");


        output.Write("<tr  >");
        output.Write("<td colspan='12'><strong>开奖号码分布图</strong></td>");
  

        output.Write("<td colspan='7'><strong>数字特征</strong></td>");
  

        output.Write("<td colspan='4'><strong>除3余数</strong></td>");
 

        output.Write("<td colspan='6'><strong>重邻间孤</strong></td>");

        output.Write("</tr  >");
    }

    
    //第一位
    protected void lbtn1_Click(object sender, EventArgs e)
    {
        MyDataBind(WeiSpan, WeiType, 1);
        ColorBind();
    }
    //第二位
    protected void lbtn2_Click(object sender, EventArgs e)
    {
        MyDataBind(WeiSpan, WeiType, 2);
        ColorBind();
    }
    //第三位
    protected void lbtn3_Click(object sender, EventArgs e)
    {
        MyDataBind(WeiSpan, WeiType, 3);
        ColorBind();
    }
    //第四位
    protected void lbtn4_Click(object sender, EventArgs e)
    {
        MyDataBind(WeiSpan, WeiType, 4);
        ColorBind();
    }
    //第五位
    protected void lbtn5_Click(object sender, EventArgs e)
    {
        MyDataBind(WeiSpan, WeiType, 5);
        ColorBind();
    }
    //今日数据
    protected void lbtnToday_Click(object sender, EventArgs e)
    {
        WeiSpan = 0;
        WeiType = 0;
        MyDataBind(0, 0, 1);
        ColorBind();
    }
    //昨日数据
    protected void lbtnLast_Click(object sender, EventArgs e)
    {
        WeiSpan = 1;
        WeiType = 0;
        MyDataBind(1, 0, 1);
        ColorBind();
    }
    //选择查寻天
    protected void ddlSelect_SelectedIndexChanged(object sender, EventArgs e)
    {
        int k = Convert.ToInt32(ddlSelect.Items[ddlSelect.SelectedIndex].Value);
        WeiSpan = k - 1;
        WeiType = 1;
        MyDataBind(k-1, 1, 1);
        ColorBind();
    }
}
