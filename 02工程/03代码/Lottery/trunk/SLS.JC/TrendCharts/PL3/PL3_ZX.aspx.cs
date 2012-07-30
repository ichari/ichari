using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class View_PL3_ZX : TrendChartPageBase
{
    public int[] aa = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
    public int[] bb = new int[20];
   
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            GridViewBind();
            ColorBind();
            GridView1.Style["border-collapse"] = "";
        }
    }

    protected void GridViewBind()
    {
        DataTable dt = new DataTable();
        dt = BLL.PL3_ZX_SeleteByNum(30);
        GridView1.DataSource = dt;
        GridView1.DataBind();
    }

    protected void ColorBind()
    {
        for (int j = 2; j <= 6; j++)
            for (int i = 0; i < GridView1.Rows.Count; i++)
            {
                GridView1.Rows[i].Cells[j].BackColor = System.Drawing.Color.FromArgb(194, 223, 193);

                GridView1.Rows[i].Cells[j].ForeColor = System.Drawing.Color.FromArgb(128, 128, 128);

                if (GridView1.Rows[i].Cells[j].Text == "0")
                {
                    GridView1.Rows[i].Cells[j].BackColor = System.Drawing.Color.FromArgb(221, 000, 000);
                    GridView1.Rows[i].Cells[j].ForeColor = System.Drawing.Color.FromArgb(255, 255, 255);
                    GridView1.Rows[i].Cells[j].Text = aa[j + 3].ToString();

                }
            }

        for (int j = 7; j <= 16; j++)
            for (int i = 0; i < GridView1.Rows.Count; i++)
            {
                GridView1.Rows[i].Cells[j].BackColor = System.Drawing.Color.FromArgb(225, 239, 255);

                GridView1.Rows[i].Cells[j].ForeColor = System.Drawing.Color.FromArgb(128, 128, 128);

                if (GridView1.Rows[i].Cells[j].Text == "0")
                {
                    GridView1.Rows[i].Cells[j].BackColor = System.Drawing.Color.FromArgb(221, 000, 000);
                    GridView1.Rows[i].Cells[j].ForeColor = System.Drawing.Color.FromArgb(255, 255, 255);
                    GridView1.Rows[i].Cells[j].Text = aa[j - 7].ToString();
                }
            }

        for (int j = 17; j <= 21; j++)
            for (int i = 0; i < GridView1.Rows.Count; i++)
            {
                GridView1.Rows[i].Cells[j].BackColor = System.Drawing.Color.FromArgb(194, 223, 193);

                GridView1.Rows[i].Cells[j].ForeColor = System.Drawing.Color.FromArgb(128, 128, 128);

                if (GridView1.Rows[i].Cells[j].Text == "0")
                {
                    GridView1.Rows[i].Cells[j].BackColor = System.Drawing.Color.FromArgb(221, 000, 000);
                    GridView1.Rows[i].Cells[j].ForeColor = System.Drawing.Color.FromArgb(255, 255, 255);
                    GridView1.Rows[i].Cells[j].Text = aa[j - 17].ToString();
                }
            }
    }

    #region 按纽选择事件

    protected void RadioButton1_CheckedChanged1(object sender, EventArgs e)
    {
        DataTable dt = BLL.PL3_ZX_SeleteByNum(100);
        GridView1.DataSource = dt;
        GridView1.DataBind();
        ColorBind();
    }
    protected void RadioButton2_CheckedChanged(object sender, EventArgs e)
    {
        DataTable dt = BLL.PL3_ZX_SeleteByNum(50);
        GridView1.DataSource = dt;
        GridView1.DataBind();
        ColorBind();

    }
    protected void RadioButton3_CheckedChanged(object sender, EventArgs e)
    {
        DataTable dt = BLL.PL3_ZX_SeleteByNum(30);
        GridView1.DataSource = dt;
        GridView1.DataBind();
        ColorBind();
    }
    protected void RadioButton4_CheckedChanged1(object sender, EventArgs e)
    {
        DataTable dt = BLL.PL3_ZX_SeleteByNum(20);
        GridView1.DataSource = dt;
        GridView1.DataBind();
        ColorBind();
    }
    protected void RadioButton5_CheckedChanged1(object sender, EventArgs e)
    {
        DataTable dt = BLL.PL3_ZX_SeleteByNum(10);
        GridView1.DataSource = BLL.PL3_ZX_SeleteByNum(10);
        GridView1.DataBind();
        ColorBind();
    }

    #endregion

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

    protected void DrawGridFooter(HtmlTextWriter output, Control ctl)
    {
        output.Write("<td colspan='2' rowspan='5'>预测方法：<br>用鼠标点击<br>方格即可显示<br>为开奖号色</td>");

        for (int i = 5; i < 10; i++)
        {
            output.Write("<td bgcolor='#C2DFC1'onClick=Style(this,'#DD0000','#C2DFC1') style='color:#C2DFC1'>");
            output.Write(i.ToString() + "</td>");
        }

        for (int j = 0; j < 10; j++)
        {
            //output.Write("<td bgcolor='#C2DFC1'onClick=Style(this,'#DD0000','#C2DFC1') style='color:#C2DFC1'>");
            output.Write("<td bgcolor='#E1EFFF' onClick=Style(this,'#DD0000','#E1EFFF') style='color:#E1EFFF'>");
            output.Write(j.ToString() + "</td>");
        }

        for (int k = 0; k < 5; k++)
        {
            output.Write("<td bgcolor='#C2DFC1' onClick=Style(this,'#DD0000','#C2DFC1') style='color:#C2DFC1'>");
           
            output.Write(k.ToString() + "</td>");
        }

        //output.Write("<td bgcolor='#E1EFFF'onClick=Style(this,'#AA1A00','#E1EFFF') style='color:#E1EFFF'>0</td>");
        //output.Write("<td bgcolor='#E1EFFF'onClick=Style(this,'#AA1A00','#E1EFFF') style='color:#E1EFFF'>1</td>");
        //output.Write("<td bgcolor='#E1EFFF'onClick=Style(this,'#AA1A00','#E1EFFF') style='color:#E1EFFF'>2</td>");
        //output.Write("<td bgcolor='#E1EFFF'onClick=Style(this,'#AA1A00','#E1EFFF') style='color:#E1EFFF;>3</td>");
        //output.Write("<td bgcolor='#E1EFFF'onClick=Style(this,'#AA1A00','#E1EFFF') style='color:#E1EFFF;>4</td>");
        //output.Write("<td bgcolor='#E1EFFF'onClick=Style(this,'#AA1A00','#E1EFFF') style='color:#E1EFFF;>5</td>");
        //output.Write("<td bgcolor='#E1EFFF'onClick=Style(this,'#AA1A00','#E1EFFF') style='color:#E1EFFF;>6</td>");
        //output.Write("<td bgcolor='#E1EFFF'onClick=Style(this,'#AA1A00','#E1EFFF') style='color:#E1EFFF;>7</td>");
        //output.Write("<td bgcolor='#E1EFFF'onClick=Style(this,'#AA1A00','#E1EFFF') style='color:#E1EFFF;>8</td>");
        //output.Write("<td bgcolor='#E1EFFF'onClick=Style(this,'#AA1A00','#E1EFFF') style='color:#E1EFFF;>9</td>");

        //output.Write("<td bgcolor='#C2DFC1'onClick=Style(this,'#AA1A00','#C2DFC1') style='color:#C2DFC1;>0</td>");
        //output.Write("<td bgcolor='#C2DFC1'onClick=Style(this,'#AA1A00','#C2DFC1') style='color:#C2DFC1;>1</td>");
        //output.Write("<td bgcolor='#C2DFC1'onClick=Style(this,'#AA1A00','#C2DFC1') style='color:#C2DFC1;>2</td>");
        //output.Write("<td bgcolor='#C2DFC1'onClick=Style(this,'#AA1A00','#C2DFC1') style='color:#C2DFC1;>3</td>");
        //output.Write("<td bgcolor='#C2DFC1'onClick=Style(this,'#AA1A00','#C2DFC1') style='color:#C2DFC1;>4</td>");

        for (int n = 0; n < 4; n++)
        {
            output.Write("<tr align='center' valign='middle'>");

            for (int i = 5; i < 10; i++)
            {
                output.Write("<td bgcolor='#C2DFC1' onClick=Style(this,'#DD0000','#C2DFC1') style='color:#C2DFC1'>");
                output.Write(i.ToString() + "</td>");
            }

            for (int j = 0; j < 10; j++)
            {
                output.Write("<td bgcolor='#E1EFFF' onClick=Style(this,'#DD0000','#E1EFFF') style='color:#E1EFFF'>");
                output.Write(j.ToString() + "</td>");
            }

            for (int k = 0; k < 5; k++)
            {
                output.Write("<td bgcolor='#C2DFC1' onClick=Style(this,'#DD0000','#C2DFC1') style='color:#C2DFC1'>");
                output.Write(k.ToString() + "</td>");
            }
        }

        // 最后到两行-- -

        output.Write("<tr align='center' valign='middle'>");

        output.Write("<td >出现次数</td>");
        output.Write("<td >&nbsp;</td>");

        for (int j = 2; j < GridView1.Columns.Count; j++)
        {
            int num = 0;
            for ( int i = 0; i < GridView1.Rows.Count; i++)
            {
                if (GridView1.Rows[i].Cells[j].BackColor == System.Drawing.Color.FromArgb(221,000,000))
                {
                    num = num + 1; 
                }
            }

            bb[j - 2] = num;

            output.Write("<td>");
            output.Write( bb[j-2].ToString() + "</td>");
        }
        

  //最后一行     
        output.Write("<tr align='center' valign='middle'>");

        output.Write("<td >期 数</td>");
        output.Write("<td >开奖号码</td>");
        for (int i = 5; i < 10; i++)
        {
            output.Write("<td> <strong><font color ='DD0000'>");
            output.Write(i.ToString() + "</font></strong></td>");
        }
        for (int i = 0; i < 10; i++)
        {
            output.Write("<td> <strong><font color ='DD0000'>");
            output.Write(i.ToString() + "</font></strong></td>");
        }
        for (int i = 0; i < 5; i++)
        {
            output.Write("<td><strong><font color ='DD0000'>");
            output.Write(i.ToString() + "</font></strong></td>");
        }


    }

    protected void DrawGridHeader(HtmlTextWriter output, Control ctl)
    {
        output.Write("<td rowspan='2'>期 数</td>");
        output.Write("<td rowspan='2'>开奖号码</td>");
        output.Write("<td colspan='20' align='center' valign='middle'>排列3组选分布</td>");

        output.Write("<tr align='center' valign='middle'>");
        for (int i = 5; i < 10; i++)
        {
            output.Write("<td bgcolor='#B0D6AF'><strong><font color='#DD0000'>");
            output.Write(i.ToString()+"</td>");
        }
        for (int i = 0; i < 10; i++)
        {
            output.Write("<td bgcolor='#B5D2FF'><strong><font color='#DD0000'>");
            output.Write(i.ToString() + "</td>");
        }

        for (int i = 0; i < 5; i++)
        {
            output.Write("<td bgcolor='#B0D6AF'><strong><font color='#DD0000'>");
            output.Write(i.ToString() + "</td>");
        }
    }
}









