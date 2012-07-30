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

public partial class PL5_PL5_HZ : TrendChartPageBase
{
    int[] num = new int[38];
    int[] sum = new int[38];

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
        dt = BLL.PL5_HZ_SeleteByNum(30);
        GridView1.DataSource = dt;
        GridView1.DataBind();
    }

    protected void RadioButton1_CheckedChanged1(object sender, EventArgs e)
    {
        DataTable dt = BLL.PL5_HZ_SeleteByNum(100);
        GridView1.DataSource = dt;
        GridView1.DataBind();
        ColorBind();
    }
    protected void RadioButton2_CheckedChanged(object sender, EventArgs e)
    {
        DataTable dt = BLL.PL5_HZ_SeleteByNum(50);
        GridView1.DataSource = dt;
        GridView1.DataBind();
        ColorBind();

    }
    protected void RadioButton3_CheckedChanged(object sender, EventArgs e)
    {
        DataTable dt = BLL.PL5_HZ_SeleteByNum(30);
        GridView1.DataSource = dt;
        GridView1.DataBind();
        ColorBind();
    }
    protected void RadioButton4_CheckedChanged1(object sender, EventArgs e)
    {
        DataTable dt = BLL.PL5_HZ_SeleteByNum(20);
        GridView1.DataSource = dt;
        GridView1.DataBind();
        ColorBind();
    }
    protected void RadioButton5_CheckedChanged1(object sender, EventArgs e)
    {
        DataTable dt = BLL.PL5_HZ_SeleteByNum(10);
        GridView1.DataSource = dt;
        GridView1.DataBind();
        ColorBind();
    }

    protected void ColorBind()
    {
        for (int j = 3; j < 31; j++)
        {
            int tem = 0;
            for (int i = 0; i < GridView1.Rows.Count; i++)
            {
                if (GridView1.Rows[i].Cells[j].Text == "0")
                {
                    GridView1.Rows[i].Cells[j].BackColor = System.Drawing.Color.FromArgb(221,000,000);
                    GridView1.Rows[i].Cells[j].ForeColor = System.Drawing.Color.FromArgb(255, 255, 255);
                    int k =j + 7;
                    GridView1.Rows[i].Cells[j].Text = k.ToString();
                    tem = tem + 1;
                }
            }

            num[j - 3] = tem;
            int a = RadioSelete();
            sum[j - 3] = (50 * tem) / a;
        }

        for (int j = 31; j < 41; j++)
        {
            int tem = 0;
            for (int i = 0; i < GridView1.Rows.Count; i++)
            {
                if (GridView1.Rows[i].Cells[j].Text == "0")
                {
                    GridView1.Rows[i].Cells[j].BackColor = System.Drawing.Color.FromArgb(000, 000,255);
                    GridView1.Rows[i].Cells[j].ForeColor = System.Drawing.Color.FromArgb(255, 255, 255);
                    int k = j - 31;
                    GridView1.Rows[i].Cells[j].Text = k.ToString();
                    tem = tem + 1;
                }
            }
            num[j - 3] = tem;
            int a = RadioSelete();
            sum[j - 3] = (50 * tem) / a;
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

    protected void DrawGridHeader(HtmlTextWriter output, Control ctl)
    {
        output.Write("<td rowspan ='2' align='center' valign='middle'>期 数</td>");
        output.Write("<td rowspan ='2' align='center' valign='middle'>开奖号码</td>");
        output.Write("<td rowspan ='2' align='center' valign='middle'>和值</td>");
        output.Write("<td colspan ='28' align='center' valign='middle'>排列5和值</td>");
        output.Write("<td colspan ='10' align='center' valign='middle'>合值区</td>");

        output.Write("<tr align='center' valign='middle'>");

        for (int i = 10; i < 38; i++)
        {
            output.Write("<td bgcolor='#E1EFFF'><font color='#DD0000'>");
            output.Write(i.ToString() + "</font></td>");
        }

        for (int i = 0; i < 10; i++)
        {
            output.Write("<td bgcolor='#C9E9CA'><font color='#DD0000'>");
            output.Write(i.ToString() + "</font></td>");
        }

    }

    protected void DrawGridFooter(HtmlTextWriter output, Control ctl)
    {
        output.Write("<td colspan='2' rowspan='5'>预测方法：<br>用鼠标点击<br>方格即可显示<br>为开奖号色</td>");
        output.Write("<td>&nbsp</td>");
        for (int i = 10; i < 38; i++)
        {
            output.Write("<td bgcolor='#FFFFFF' onClick=Style(this,'#DD0000','#FFFFFF') style='color:#FFFFFF;'>");
            output.Write(i.ToString() + "</td>");

        }

        for (int i = 0; i < 10; i++)
        {
            output.Write("<td bgcolor='#E3F4E3' onClick=Style(this,'#0000FF','#E3F4E3') style='color:#E3F4E3;'>");
            output.Write(i.ToString() + "</td>");
        }

        for (int j = 0; j < 4; j++)
        {
            output.Write("<tr align='center' valign='middle'>");
            output.Write("<td>&nbsp</td>");
            for (int i = 10; i < 38; i++)
            {
                output.Write("<td bgcolor='#FFFFFF' onClick=Style(this,'#DD0000','#FFFFFF') style='color:#FFFFFF;'>");
                output.Write(i.ToString() + "</td>");

            }

            for (int i = 0; i < 10; i++)
            {
                output.Write("<td bgcolor='#E3F4E3' onClick=Style(this,'#0000FF','#E3F4E3') style='color:#E3F4E3;'>");
                output.Write(i.ToString() + "</td>");
            }
        }
       
        output.Write("<tr align='center' valign='middle'>");
        output.Write("<td height='50px' align='center' valign='middle'>出现次数</td>");
        output.Write("<td>&nbsp</td>");
        output.Write("<td>&nbsp</td>");
        for (int i = 0; i < 38; i++)
        {
            output.Write("<td align='center' valign='bottom'>");
            output.Write(num[i].ToString() + "<br><img src='../Image/01[1].gif' height='" + sum[i].ToString() + "'' title='" + num[i].ToString() + "' width='8px'></td>");
            
        }
        output.Write("<tr align='center' valign='middle'>");
        output.Write("<td>期 数</td>");
        output.Write("<td>开奖号码</td>");
        output.Write("<td>和值</td>");
        for (int i = 10; i < 38; i++)
        {
            output.Write("<td bgcolor='#E1EFFF'><font color='#DD0000'>");
            output.Write(i.ToString() + "</font></td>");
        }
        for (int i = 0; i < 10; i++)
        {
            output.Write("<td bgcolor='#C9E9CA'><font color='#DD0000'>");
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
