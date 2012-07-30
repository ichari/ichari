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

public partial class View_PL3_HZ : TrendChartPageBase
{
    int[] num = new int[42];
    int[] sum = new int[42];

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
        dt = BLL.PL3_HZ_SeleteByNum(30);
        GridView1.DataSource = dt;
        GridView1.DataBind();
    }

    protected void ColorBind()
    {
        for (int j = 4; j < 11; j++)
        {
             int tem = 0;
            for (int i = 0; i < GridView1.Rows.Count; i++)
            {
                if (GridView1.Rows[i].Cells[j].Text == "0")
                {
                    GridView1.Rows[i].Cells[j].BackColor = System.Drawing.Color.FromArgb(255, 000, 000);
                    GridView1.Rows[i].Cells[j].ForeColor = System.Drawing.Color.FromArgb(255, 255, 255);
                    int k = j - 4;
                    GridView1.Rows[i].Cells[j].Text = k.ToString();
                    tem = tem + 1;
                }
            }
            num[j - 4] = tem;
            int a = RadioSelete();
            sum[j - 4] = 50 * tem / a;
        }
        for (int j = 12; j < 19; j++)
        {
            int tem = 0;
            for (int i = 0; i < GridView1.Rows.Count; i++)
            {
                if (GridView1.Rows[i].Cells[j].Text == "0")
                {
                    GridView1.Rows[i].Cells[j].BackColor = System.Drawing.Color.FromArgb(255, 000, 000);
                    GridView1.Rows[i].Cells[j].ForeColor = System.Drawing.Color.FromArgb(255, 255, 255);
                    int k = j - 5;
                    GridView1.Rows[i].Cells[j].Text = k.ToString();
                    tem = tem + 1;
                }
            }
            num[j - 5] = tem;
            int a = RadioSelete();
            sum[j - 5] = 50 * tem / a;
        }

        for (int j = 20; j < 27; j++)
        {
            int tem = 0;
            for (int i = 0; i < GridView1.Rows.Count; i++)
            {
                if (GridView1.Rows[i].Cells[j].Text == "0")
                {
                    GridView1.Rows[i].Cells[j].BackColor = System.Drawing.Color.FromArgb(000, 000,255);
                    GridView1.Rows[i].Cells[j].ForeColor = System.Drawing.Color.FromArgb(255, 255, 255);
                    int k = j - 6;
                    GridView1.Rows[i].Cells[j].Text = k.ToString();
                    tem = tem + 1;
                }
            }
            num[j - 6] = tem;
            int a = RadioSelete();
            sum[j - 6] = 50 * tem / a;
        }

        for (int j = 28; j < 35; j++)
        {
            int tem = 0;
            for (int i = 0; i < GridView1.Rows.Count; i++)
            {
                if (GridView1.Rows[i].Cells[j].Text == "0")
                {
                    GridView1.Rows[i].Cells[j].BackColor = System.Drawing.Color.FromArgb(255, 000, 000);
                    GridView1.Rows[i].Cells[j].ForeColor = System.Drawing.Color.FromArgb(255, 255, 255);
                    int k = j - 7;
                    GridView1.Rows[i].Cells[j].Text = k.ToString();
                    tem = tem + 1;
                }
            }
            num[j - 7] = tem;
            int a = RadioSelete();
            sum[j - 7] = 50 * tem / a;
        }

        for (int j = 36; j < 46; j++)
        {
            int tem = 0;
            for (int i = 0; i < GridView1.Rows.Count; i++)
            {
                if (GridView1.Rows[i].Cells[j].Text == "0")
                {
                    GridView1.Rows[i].Cells[j].BackColor = System.Drawing.Color.FromArgb(000, 000,255);
                    GridView1.Rows[i].Cells[j].ForeColor = System.Drawing.Color.FromArgb(255, 255, 255);
                    int k = j - 36;
                    GridView1.Rows[i].Cells[j].Text = k.ToString();
                    tem = tem + 1;
                }
            }
            num[j - 8] = tem;
            int a = RadioSelete();
            sum[j - 8] = 50 * tem / a;
        }
        for (int j = 47; j < 49; j++)
        {
            int tem = 0;
            for (int i = 0; i < GridView1.Rows.Count; i++)
            {
                if (GridView1.Rows[i].Cells[j].Text == "奇" || GridView1.Rows[i].Cells[j].Text == "偶")
                {
                    GridView1.Rows[i].Cells[j].BackColor = System.Drawing.Color.FromArgb(221, 000, 000);
                    GridView1.Rows[i].Cells[j].ForeColor = System.Drawing.Color.FromArgb(255, 255, 255);

                    tem = tem + 1;
                }
            }
            num[j - 9] = tem;
            int a = RadioSelete();
            sum[j - 9] = 50 * tem / a;
        }

        for (int j = 50; j < 52; j++)
        {
            int tem = 0;
            for (int i = 0; i < GridView1.Rows.Count; i++)
            {
                if (GridView1.Rows[i].Cells[j].Text == "大" || GridView1.Rows[i].Cells[j].Text == "小")
                {
                    GridView1.Rows[i].Cells[j].BackColor = System.Drawing.Color.FromArgb(221, 000, 000);
                    GridView1.Rows[i].Cells[j].ForeColor = System.Drawing.Color.FromArgb(255, 255, 255);

                    tem = tem + 1;
                }
            }
            num[j - 10] = tem;
            int a = RadioSelete();
            sum[j - 10] = 50 * tem / a;
        }

        for( int i = 0 ;i< GridView1.Rows.Count; i++)
            for (int j = 0; j < GridView1.Columns.Count; j++)
            {
                if (GridView1.Rows[i].Cells[j].Text == "-1")
                {
                    GridView1.Rows[i].Cells[j].Text = "&nbsp;";
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

    protected void DrawGridFooter(HtmlTextWriter output, Control ctl)
    {
        output.Write("<td rowspan = '5' colspan='2'>预测方法：<br>用鼠标点击<br>方格即可显示<br>为开奖号色</td>");
        output.Write("<td>&nbsp</td>");
        output.Write("<td bgcolor='#CCCCCC' width='2px'>&nbsp</td>");

        for (int i = 0; i < 7; i++)
        {
            output.Write("<td bgcolor='#FFFFFF' onClick=Style(this,'#FF0000','#FFFFFF') style='color:#FFFFFF;'>");
            output.Write(i.ToString() + "</td>");
        }
        output.Write("<td bgcolor='#CCCCCC' width='2px'>&nbsp</td>");

        for (int i = 7; i < 14; i++)
        {
            output.Write("<td bgcolor='#FFFFFF' onClick=Style(this,'#FF0000','#FFFFFF') style='color:#FFFFFF;'>");
            output.Write(i.ToString() + "</td>");
        }
        output.Write("<td bgcolor='#CCCCCC' width='2px'>&nbsp</td>");

        for (int i = 14; i < 21; i++)
        {
            output.Write("<td bgcolor='#FFFFFF' onClick=Style(this,'#FF0000','#FFFFFF') style='color:#FFFFFF;'>");
            output.Write(i.ToString() + "</td>");
        }
        output.Write("<td bgcolor='#CCCCCC' width='2px'>&nbsp</td>");
        for (int i = 21; i < 28; i++)
        {
            output.Write("<td bgcolor='#FFFFFF' onClick=Style(this,'#FF0000','#FFFFFF') style='color:#FFFFFF;'>");
            output.Write(i.ToString() + "</td>");
        }
        output.Write("<td bgcolor='#CCCCCC' width='2px'>&nbsp</td>");
        for (int i = 0; i < 10; i++)
        {
            output.Write("<td bgcolor='#E3F4E3' onClick=Style(this,'#0000FF','#E3F4E3') style='color:#E3F4E3;'>");
            output.Write(i.ToString() + "</td>");
        }
        output.Write("<td bgcolor='#CCCCCC' width='2px'>&nbsp</td>");
        output.Write("<td bgcolor='#FFFFFF' onClick=Style(this,'#DD0000','#FFFFFF') style='color:#FFFFFF;'>奇</td>");
        output.Write("<td bgcolor='#FFFFFF' onClick=Style(this,'#DD0000','#FFFFFF') style='color:#FFFFFF;'>偶</td>");
        output.Write("<td bgcolor='#CCCCCC' width='2px'>&nbsp</td>");
        output.Write("<td bgcolor='#FFFFFF' onClick=Style(this,'#0000FF','#FFFFFF') style='color:#FFFFFF;'>大</td>");
        output.Write("<td bgcolor='#FFFFFF' onClick=Style(this,'#0000FF','#FFFFFF') style='color:#FFFFFF;'>小</td>");


        for (int J = 0; J < 4; J++)
        {
            output.Write("<tr align ='center' valign='middle'>");
            output.Write("<td>&nbsp</td>");
            output.Write("<td bgcolor='#CCCCCC' width='2px'>&nbsp</td>");

            for (int i = 0; i < 7; i++)
            {
                output.Write("<td bgcolor='#FFFFFF' onClick=Style(this,'#FF0000','#FFFFFF') style='color:#FFFFFF;'>");
                output.Write(i.ToString() + "</td>");
            }
            output.Write("<td bgcolor='#CCCCCC' width='2px'>&nbsp</td>");

            for (int i = 7; i < 14; i++)
            {
                output.Write("<td bgcolor='#FFFFFF' onClick=Style(this,'#FF0000','#FFFFFF') style='color:#FFFFFF;'>");
                output.Write(i.ToString() + "</td>");
            }
            output.Write("<td bgcolor='#CCCCCC' width='2px'>&nbsp</td>");

            for (int i = 14; i < 21; i++)
            {
                output.Write("<td bgcolor='#FFFFFF' onClick=Style(this,'#FF0000','#FFFFFF') style='color:#FFFFFF;'>");
                output.Write(i.ToString() + "</td>");
            }
            output.Write("<td bgcolor='#CCCCCC' width='2px'>&nbsp</td>");
            for (int i = 21; i < 28; i++)
            {
                output.Write("<td bgcolor='#FFFFFF' onClick=Style(this,'#FF0000','#FFFFFF') style='color:#FFFFFF;'>");
                output.Write(i.ToString() + "</td>");
            }
            output.Write("<td bgcolor='#CCCCCC' width='2px'>&nbsp</td>");
            for (int i = 0; i < 10; i++)
            {
                output.Write("<td bgcolor='#E3F4E3' onClick=Style(this,'#0000FF','#E3F4E3') style='color:#E3F4E3;'>");
                output.Write(i.ToString() + "</td>");
            }
            output.Write("<td bgcolor='#CCCCCC' width='2px'>&nbsp</td>");
            output.Write("<td bgcolor='#FFFFFF' onClick=Style(this,'#DD0000','#FFFFFF') style='color:#FFFFFF;'>奇</td>");
            output.Write("<td bgcolor='#FFFFFF' onClick=Style(this,'#DD0000','#FFFFFF') style='color:#FFFFFF;'>偶</td>");
            output.Write("<td bgcolor='#CCCCCC' width='2px'>&nbsp</td>");
            output.Write("<td bgcolor='#FFFFFF' onClick=Style(this,'#0000FF','#FFFFFF') style='color:#FFFFFF;'>大</td>");
            output.Write("<td bgcolor='#FFFFFF' onClick=Style(this,'#0000FF','#FFFFFF') style='color:#FFFFFF;'>小</td>");

        }

        output.Write("<tr align = 'center' valign='middle'>");
        output.Write("<td height='50'>出现次数</td>");
        output.Write("<td>&nbsp</td>");
        output.Write("<td>&nbsp</td>");

        output.Write("<td bgcolor='#CCCCCC' width='2px'>&nbsp</td>");
        for (int j = 0; j < 7; j++)
        {
            output.Write("<td bgcolor='#FFFFFF' align='center' valign='bottom'>");
            output.Write(num[j].ToString() + "<br><img src='../image/01[1].gif' width='8' height='" + sum[j].ToString() + "' title='" + num[j].ToString() + "'></td>");
        }

        output.Write("<td bgcolor='#CCCCCC' width='2px'>&nbsp</td>");
        for (int j = 7; j < 14; j++)
        {
            output.Write("<td bgcolor='#FFFFFF' align='center' valign='bottom'>");
            output.Write(num[j].ToString() + "<br><img src='../image/01[1].gif' width='8' height='" + sum[j].ToString() + "' title='" + num[j].ToString() + "'></td>");
        }
        output.Write("<td bgcolor='#CCCCCC' width='2px'>&nbsp</td>");
        for (int j = 14; j < 21; j++)
        {
            output.Write("<td bgcolor='#FFFFFF' align='center' valign='bottom'>");
            output.Write(num[j].ToString() + "<br><img src='../image/01[1].gif' width='8' height='" + sum[j].ToString() + "' title='" + num[j].ToString() + "'></td>");
        }
        output.Write("<td bgcolor='#CCCCCC' width='2px'>&nbsp</td>");
        for (int j = 21; j < 28; j++)
        {
            output.Write("<td bgcolor='#FFFFFF' align='center' valign='bottom'>");
            output.Write(num[j].ToString() + "<br><img src='../image/01[1].gif' width='8' height='" + sum[j].ToString() + "' title='" + num[j].ToString() + "'></td>");
        }
        output.Write("<td bgcolor='#CCCCCC' width='2px'>&nbsp</td>");
        for (int j = 28; j < 38; j++)
        {
            output.Write("<td bgcolor='#FFFFFF' align='center' valign='bottom'>");
            output.Write(num[j].ToString() + "<br><img src='../image/01[1].gif' width='8' height='" + sum[j].ToString() + "' title='" + num[j].ToString() + "'></td>");
        }
        output.Write("<td bgcolor='#CCCCCC' width='2px'>&nbsp</td>");
        for (int j = 38; j < 40; j++)
        {
            output.Write("<td bgcolor='#FFFFFF' align='center' valign='bottom'>");
            output.Write(num[j].ToString() + "<br><img src='../image/01[1].gif' width='8' height='" + sum[j].ToString() + "' title='" + num[j].ToString() + "'></td>");
        }
        output.Write("<td bgcolor='#CCCCCC' width='2px'>&nbsp</td>");
        for (int j = 40; j < 42; j++)
        {
            output.Write("<td bgcolor='#FFFFFF' align='center' valign='bottom'>");
            output.Write(num[j].ToString() + "<br><img src='../image/01[1].gif' width='8' height='" + sum[j].ToString() + "' title='" + num[j].ToString() + "'></td>");
        }
        output.Write("<tr align = 'center' valign='middle'>");
        output.Write("<td >期  数</td>");
        output.Write("<td >开奖号</td>");
        output.Write("<td >和值</td>");

        for (int j = 0; j < 4; j++)
        {
            int k = j * 7;
            output.Write("<td bgcolor='#CCCCCC' width='2px'>&nbsp</td>");
            for (int i = 0; i < 7; i++)
            {
                output.Write("<td bgcolor='#E1EFFF'><strong><font color='#FF0000'>");
               
                output.Write(k.ToString() + "</td>");
                k = k + 1;
            }
        }

        output.Write("<td bgcolor='#CCCCCC' width='2px'>&nbsp</td>");
        for (int i = 0; i < 10; i++)
        {
            output.Write("<td bgcolor='#C9E9CA'><strong><font color='#FF0000'>");

            output.Write(i.ToString() + "</font></strong></td>");
        }
        output.Write("<td bgcolor='#CCCCCC' width='2px'>&nbsp</td>");
        output.Write("<td bgcolor='#CACAFF'><strong><font color='#DD0000'>奇</font></strong></td>");
        output.Write("<td bgcolor='#CACAFF'><strong><font color='#DD0000'>偶</font></strong></td>");
        output.Write("<td bgcolor='#CCCCCC' width='2px'>&nbsp</td>");
        output.Write("<td bgcolor='#C9E9CA'><strong><font color='#DD0000'>大</font></strong></td>");
        output.Write("<td bgcolor='#C9E9CA'><strong><font color='#DD0000'>小</font></strong></td>");

    }

    protected void DrawGridHeader(HtmlTextWriter output, Control ctl)
    {
        output.Write("<td rowspan='2'>期  数</td>");
        output.Write("<td rowspan='2'>开奖号</td>");
        output.Write("<td rowspan='2'>和<br>值</td>");
        output.Write("<td colspan ='33' align='center' valign='middle'>排列3和值区</td>");
        output.Write("<td colspan ='11' align='center' valign='middle'>合值区</td>");
        output.Write("<td colspan ='3' align='center' valign='middle'>奇偶</td>");
        output.Write("<td colspan ='2' align='center' valign='middle'>大小</td>");

        output.Write("<tr align = 'center' valign='middle'>");
        output.Write("<td bgcolor='#CCCCCC' width='2px'>&nbsp</td>");
        for (int i = 0; i < 7; i++)
        {
            output.Write("<td align = 'center' valign='middle' bgcolor='#E1EFFF'><strong><font color='#FF0000'>");
            output.Write(i.ToString() + "</td>");
        }
        output.Write("<td bgcolor='#CCCCCC' width='2px'>&nbsp</td>");
        for (int i = 7; i < 14; i++)
        {
            output.Write("<td align = 'center' valign='middle' bgcolor='#E1EFFF'><strong><font color='#FF0000'>");
            output.Write(i.ToString() + "</td>");
        }
        output.Write("<td bgcolor='#CCCCCC' width='2px'>&nbsp</td>");
        for (int i = 14; i < 21; i++)
        {
            output.Write("<td align = 'center' valign='middle' bgcolor='#E1EFFF'><strong><font color='#FF0000'>");
            output.Write(i.ToString() + "</td>");
        }
        output.Write("<td bgcolor='#CCCCCC' width='2px'>&nbsp</td>");
        for (int i = 21; i < 28; i++)
        {
            output.Write("<td align = 'center' valign='middle' bgcolor='#E1EFFF'><strong><font color='#FF0000'>");
            output.Write(i.ToString() + "</td>");
        }
        output.Write("<td bgcolor='#CCCCCC' width='2px'>&nbsp</td>");

        for (int i = 0; i < 10; i++)
        {
            output.Write("<td align = 'center' valign='middle' bgcolor='#C9E9CA'><strong><font color='#FF0000'>");
            output.Write(i.ToString() + "</td>");
        }
        output.Write("<td bgcolor='#CCCCCC' width='2px'>&nbsp</td>");

        output.Write("<td bgcolor='#CACAFF'><strong><font color='#DD0000'>奇</font></strong></td>");
        output.Write("<td bgcolor='#CACAFF'><strong><font color='#DD0000'>偶</font></strong></td>");
        output.Write("<td bgcolor='#CCCCCC' width='2px'>&nbsp</td>");
        output.Write("<td bgcolor='#C9E9CA'><strong><font color='#DD0000'>大</font></strong></td>");
        output.Write("<td bgcolor='#C9E9CA'><strong><font color='#DD0000'>小</font></strong></td>");

    }

    protected void RadioButton1_CheckedChanged1(object sender, EventArgs e)
    {
        DataTable dt = BLL.PL3_HZ_SeleteByNum(100);
        GridView1.DataSource = dt;
        GridView1.DataBind();
        ColorBind();
    }
    protected void RadioButton2_CheckedChanged(object sender, EventArgs e)
    {
        DataTable dt = BLL.PL3_HZ_SeleteByNum(50);
        GridView1.DataSource = dt;
        GridView1.DataBind();
        ColorBind();

    }
    protected void RadioButton3_CheckedChanged(object sender, EventArgs e)
    {
        DataTable dt = BLL.PL3_HZ_SeleteByNum(30);
        GridView1.DataSource = dt;
        GridView1.DataBind();
        ColorBind();
    }
    protected void RadioButton4_CheckedChanged1(object sender, EventArgs e)
    {
        DataTable dt = BLL.PL3_HZ_SeleteByNum(20);
        GridView1.DataSource = dt;
        GridView1.DataBind();
        ColorBind();
    }
    protected void RadioButton5_CheckedChanged1(object sender, EventArgs e)
    {
        DataTable dt = BLL.PL3_WH_SeleteByNum(10);
        GridView1.DataSource = BLL.PL3_HZ_SeleteByNum(10);
        GridView1.DataBind();
        ColorBind();
    }

    private int RadioSelete()
    {
        int k = 1;

        if (RadioButton1.Checked == true)
        {

            k = 80;
        }

        if (RadioButton2.Checked == true)
        {
            k = 40;
        }

        if (RadioButton3.Checked == true)
        {
            k = 25;
        }

        if (RadioButton4.Checked == true)
        {
            k = 15;
        }

        if (RadioButton5.Checked == true)
        {
            k = 8;
        }

        return k;

    }
}
