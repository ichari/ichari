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

public partial class PL5_PL5_KJFB : TrendChartPageBase
{
    int[] num = new int[50];
    int[] sum = new int[50];
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
        dt = BLL.PL5_HMFB_SeleteByNum(30);
        GridView1.DataSource = dt;
        GridView1.DataBind();
    }

    protected void ColorBind()
    {
            for (int i = GridView1.Rows.Count; i > 0; i--)
            {
                int j = GridView1.Rows.Count - i;
                GridView1.Rows[j].Cells[0].Text = i.ToString();
            }

        for (int j = 0; j < 3; j++)
        {
              for (int i = 0; i < GridView1.Rows.Count; i++)
            
                if (i % 2 == 1)
                {
                    GridView1.Rows[i].Cells[j].BackColor = System.Drawing.Color.FromArgb(233,248,207);
                }
        }

        for (int j = 3; j < 13; j++)
        {
            int k = 0;
            for (int i = 0; i < GridView1.Rows.Count; i++)
            {
                if (i % 2 == 1)
                {
                    GridView1.Rows[i].Cells[j].BackColor = System.Drawing.Color.FromArgb(233,248,207);
                }

                if (GridView1.Rows[i].Cells[j].Text == "0")
                {
                    GridView1.Rows[i].Cells[j].BackColor = System.Drawing.Color.FromArgb(225,000,000);
                    GridView1.Rows[i].Cells[j].ForeColor = System.Drawing.Color.FromArgb(255,255,255);
                    k = k + 1;
                    int t = j - 3;
                    GridView1.Rows[i].Cells[j].Text = t.ToString();
                }
            }
            num[j - 3] = k;
            int a = RadioSelete();
            sum[j-3] = 50 * k / a;
        }

        for (int j = 13; j < 23; j++)
        {
            int k = 0;
            for (int i = 0; i < GridView1.Rows.Count; i++)
            {
                if (GridView1.Rows[i].Cells[j].Text == "0")
                {
                    GridView1.Rows[i].Cells[j].BackColor = System.Drawing.Color.FromArgb(000, 000,255);
                    GridView1.Rows[i].Cells[j].ForeColor = System.Drawing.Color.FromArgb(255, 255, 255);
                    k = k + 1;
                    int t = j - 13;
                    GridView1.Rows[i].Cells[j].Text = t.ToString();
                }
            }
            num[j - 3] = k;
            int a = RadioSelete();
            sum[j - 3] = 50 * k / a;
        }

        for (int j = 23; j < 33; j++)
        {
            int k = 0;
            for (int i = 0; i < GridView1.Rows.Count; i++)
            {
                if (i % 2 == 1)
                {
                    GridView1.Rows[i].Cells[j].BackColor = System.Drawing.Color.FromArgb(233, 248, 207);
                }
                if (GridView1.Rows[i].Cells[j].Text == "0")
                {
                    GridView1.Rows[i].Cells[j].BackColor = System.Drawing.Color.FromArgb(225, 000, 000);
                    GridView1.Rows[i].Cells[j].ForeColor = System.Drawing.Color.FromArgb(255, 255, 255);
                    k = k + 1;
                    int t = j - 23;
                    GridView1.Rows[i].Cells[j].Text = t.ToString();
                }
            }
            num[j - 3] = k;
            int a = RadioSelete();
            sum[j - 3] = 50 * k / a;
        }

        for (int j = 33; j < 43; j++)
        {
            int k = 0;
            for (int i = 0; i < GridView1.Rows.Count; i++)
            {
                if (GridView1.Rows[i].Cells[j].Text == "0")
                {
                    GridView1.Rows[i].Cells[j].BackColor = System.Drawing.Color.FromArgb(000, 000, 255);
                    GridView1.Rows[i].Cells[j].ForeColor = System.Drawing.Color.FromArgb(255, 255, 255);
                    k = k + 1;
                    int t = j - 33;
                    GridView1.Rows[i].Cells[j].Text = t.ToString();
                }
            }
            num[j - 3] = k;
            int a = RadioSelete();
            sum[j - 3] = 50 * k / a;
        }

        for (int j = 43; j < 53; j++)
        {
            int k = 0;
            for (int i = 0; i < GridView1.Rows.Count; i++)
            {
                if (i % 2 == 1)
                {
                    GridView1.Rows[i].Cells[j].BackColor = System.Drawing.Color.FromArgb(233, 248, 207);
                }
                if (GridView1.Rows[i].Cells[j].Text == "0")
                {
                    GridView1.Rows[i].Cells[j].BackColor = System.Drawing.Color.FromArgb(225, 000, 000);
                    GridView1.Rows[i].Cells[j].ForeColor = System.Drawing.Color.FromArgb(255, 255, 255);
                    k = k + 1;
                    int t = j - 43;
                    GridView1.Rows[i].Cells[j].Text = t.ToString();
                }
            }
            num[j - 3] = k;
            int a = RadioSelete();
            sum[j - 3] = 50 * k / a;
        }
    }

    protected void RadioButton1_CheckedChanged1(object sender, EventArgs e)
    {
        DataTable dt = BLL.PL5_HMFB_SeleteByNum(100);
        GridView1.DataSource = dt;
        GridView1.DataBind();
        ColorBind();
    }
    protected void RadioButton2_CheckedChanged(object sender, EventArgs e)
    {
        DataTable dt = BLL.PL5_HMFB_SeleteByNum(50);
        GridView1.DataSource = dt;
        GridView1.DataBind();
        ColorBind();
   
    }
    protected void RadioButton3_CheckedChanged(object sender, EventArgs e)
    {
        DataTable dt = BLL.PL5_HMFB_SeleteByNum(30);
        GridView1.DataSource = dt; 
        GridView1.DataBind();
        ColorBind();
    }
    protected void RadioButton4_CheckedChanged1(object sender, EventArgs e)
    {
        DataTable dt = BLL.PL5_HMFB_SeleteByNum(20);
        GridView1.DataSource = dt;
        GridView1.DataBind();
        ColorBind();
    }
    protected void RadioButton5_CheckedChanged1(object sender, EventArgs e)
    {
        DataTable dt = BLL.PL5_HMFB_SeleteByNum(10);
        GridView1.DataSource = dt;
        GridView1.DataBind();
        ColorBind();
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
        output.Write("<td rowspan='3' align='center' valign='middle'>序号</td>");
        output.Write("<td rowspan='3' align='center' valign='middle'>期数</td>");
        output.Write("<td rowspan='3' align='center' valign='middle'>开奖号</td>");
        output.Write("<td rowspan='1' colspan ='50' align='center' valign='middle'><strong><font color ='#FF0000'>排列五开奖号码</font></strong></td>");
        
        output.Write("<tr align='center' valign='middle'>");
        output.Write("<td colspan='10'>第一位</td>");
        output.Write("<td colspan='10'>第二位</td>");
        output.Write("<td colspan='10'>第三位</td>");
        output.Write("<td colspan='10'>第四位</td>");
        output.Write("<td colspan='10'>第五位</td>");

        output.Write("<tr align='center' valign='middle'>");
        for (int i = 0; i < 10; i++)
        {
            output.Write("<td bgcolor ='#F8EFEF'><font color ='#FF0000'>");
            output.Write(i.ToString() + "</font></td>");
        }
        for (int i = 0; i < 10; i++)
        {
            output.Write("<td bgcolor ='#CAD9E8'><font color ='#OO33CC'>");
            output.Write(i.ToString() + "</font></td>");
        }
        for (int i = 0; i < 10; i++)
        {
            output.Write("<td bgcolor ='#F8EFEF'><font color ='#FF0000'>");
            output.Write(i.ToString() + "</font></td>");
        }
        for (int i = 0; i < 10; i++)
        {
            output.Write("<td bgcolor ='#CAD9E8'><font color ='#OO33CC'>");
            output.Write(i.ToString() + "</font></td>");
        }
        for (int i = 0; i < 10; i++)
        {
            output.Write("<td bgcolor ='#F8EFEF'><font color ='#FF0000'>");
            output.Write(i.ToString() + "</font></td>");
        }

    }

    protected void DrawGridFooter(HtmlTextWriter output, Control ctl)
    {
        output.Write("<td rowspan = '5' colspan='2'>预测方法：<br>用鼠标点击<br>方格即可显示<br>为开奖号色</td>");
        output.Write("<td bgcolor ='#FFFFFF'>&nbsp</td>");
        for (int i = 0; i < 10; i++)
        {
            output.Write("<td bgcolor='#FFFFFF' onClick=Style(this,'#DD0000','#FFFFFF') style='color:#FFFFFF;'>");
            output.Write(i.ToString() + "</td>");
        }
        for (int i = 0; i < 10; i++)
        {
            output.Write("<td bgcolor='#D0E6F7' onClick=Style(this,'#0000FF','#D0E6F7') style='color:#D0E6F7;'>");
            output.Write(i.ToString() + "</td>");
        }
        for (int i = 0; i < 10; i++)
        {
            output.Write("<td bgcolor='#FFFFFF' onClick=Style(this,'#DD0000','#FFFFFF') style='color:#FFFFFF;'>");
            output.Write(i.ToString() + "</td>");
        }

        for (int i = 0; i < 10; i++)
        {
            output.Write("<td bgcolor='#D0E6F7' onClick=Style(this,'#0000FF','#D0E6F7') style='color:#D0E6F7;'>");
            output.Write(i.ToString() + "</td>");
        }
        for (int i = 0; i < 10; i++)
        {
            output.Write("<td bgcolor='#FFFFFF' onClick=Style(this,'#DD0000','#FFFFFF') style='color:#FFFFFF;'>");
            output.Write(i.ToString() + "</td>");
        }

        for (int j = 0; j < 4; j++)
        {
            output.Write("<tr align ='center' valign='middle'>");
            output.Write("<td bgcolor ='#FFFFFF'>&nbsp</td>");
            for (int i = 0; i< 10; i++)
            {
                output.Write("<td bgcolor='#FFFFFF' onClick=Style(this,'#DD0000','#FFFFFF') style='color:#FFFFFF;'>");
                output.Write(i.ToString() + "</td>");
            }
            for (int i = 0; i < 10; i++)
            {
                output.Write("<td bgcolor='#D0E6F7' style='color:#D0E6F7;'>");
                output.Write(i.ToString() + "</td>");
            }
            for (int i = 0; i < 10; i++)
            {
                output.Write("<td bgcolor='#FFFFFF' onClick=Style(this,'#DD0000','#FFFFFF') style='color:#FFFFFF;'>");
                output.Write(i.ToString() + "</td>");
            }

            for (int i = 0; i < 10; i++)
            {
                output.Write("<td bgcolor='#D0E6F7' onClick=Style(this,'#0000FF','#D0E6F7') style='color:#D0E6F7;'>");
                output.Write(i.ToString() + "</td>");
            }
            for (int i = 0; i < 10; i++)
            {
                output.Write("<td bgcolor='#FFFFFF' onClick=Style(this,'#DD0000','#FFFFFF') style='color:#FFFFFF;'>");
                output.Write(i.ToString() + "</td>");
            }

            for (int i = 0; i < 4; i++)
            {
                //output.Write("<td align ='center' valign='middle'>");
            }
        }

        output.Write("<tr align = 'center' valign='middle'>");
        output.Write("<td rowspan ='2'>序号</td>");
        output.Write("<td colspan ='1' height='50'>出现次数</td>");
        output.Write("<td colspan ='1'>&nbsp</td>");
        for (int i = 0; i < 50; i++)
        {
            output.Write("<td bgcolor='#FFFFFF' align='center' valign='bottom'>");
            output.Write(num[i].ToString() + "<br><img src='../image/01[1].gif' width='8' height='" + sum[i].ToString() + "' title='" + num[i].ToString() + "'></td>");
        }
        output.Write("<tr align = 'center' valign='middle'>");
        output.Write("<td align = 'center' valign='middle'>数字序号</td>");
        output.Write("<td align = 'center'>开奖号</td>");
        for (int i = 0; i < 10; i++)
        {
            output.Write("<td bgcolor ='#F8EFEF'><font color ='#FF0000'>");
            output.Write(i.ToString() + "</font></td>");
        }
        for (int i = 0; i < 10; i++)
        {
            output.Write("<td bgcolor ='#CAD9E8'><font color ='#OO33CC'>");
            output.Write(i.ToString() + "</font></td>");
        }
        for (int i = 0; i < 10; i++)
        {
            output.Write("<td bgcolor ='#F8EFEF'><font color ='#FF0000'>");
            output.Write(i.ToString() + "</font></td>");
        }
        for (int i = 0; i < 10; i++)
        {
            output.Write("<td bgcolor ='#CAD9E8'><font color ='#OO33CC'>");
            output.Write(i.ToString() + "</font></td>");
        }
        for (int i = 0; i < 10; i++)
        {
            output.Write("<td bgcolor ='#F8EFEF'><font color ='#FF0000'>");
            output.Write(i.ToString() + "</font></td>");
        }


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
