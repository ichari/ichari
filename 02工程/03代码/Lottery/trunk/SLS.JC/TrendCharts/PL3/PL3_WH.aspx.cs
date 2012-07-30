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

public partial class View_PL3_WH : TrendChartPageBase
{
    int[] num = new int[57];
    int[] sum = new int[57];

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
        dt = BLL.PL3_WH_SeleteByNum(30);
        GridView1.DataSource = dt;
        GridView1.DataBind();
    }

    protected void ColorBind()
    {
        for (int j = 3; j < 22; j++)
        {
            int tem = 0;
            for (int i = 0; i < GridView1.Rows.Count; i++)
            {
                if (GridView1.Rows[i].Cells[j].Text == "0")
                {
                    GridView1.Rows[i].Cells[j].BackColor = System.Drawing.Color.FromArgb(221,000,000);
                    int k = j - 3;
                    GridView1.Rows[i].Cells[j].Text = k.ToString();
                    GridView1.Rows[i].Cells[j].ForeColor = System.Drawing.Color.FromArgb(255, 255, 255);
                    tem = tem + 1;
                }
            }
            num[j - 3] = tem;
            int a = RadioSelete();
            sum[j - 3] = 50 * tem / a;
        }

        for (int j = 22; j < 41; j++)
        {
            int tem = 0;
            for (int i = 0; i < GridView1.Rows.Count; i++)
            {
                if (GridView1.Rows[i].Cells[j].Text == "0")
                {
                    GridView1.Rows[i].Cells[j].BackColor = System.Drawing.Color.FromArgb(000, 000,255);
                    int k = j - 22;
                    GridView1.Rows[i].Cells[j].Text = k.ToString();
                    GridView1.Rows[i].Cells[j].ForeColor = System.Drawing.Color.FromArgb(255,255,255);
                    tem = tem + 1;
                }
            }
            num[j - 3] = tem;
            int a = RadioSelete();
            sum[j - 3] = 50 * tem / a;
        }

        for (int j = 41; j < 60; j++)
        {
            int tem = 0;
            for (int i = 0; i < GridView1.Rows.Count; i++)
            {
                if (GridView1.Rows[i].Cells[j].Text == "0")
                {
                    GridView1.Rows[i].Cells[j].BackColor = System.Drawing.Color.FromArgb(221,000, 000);
                    int k = j - 41;
                    GridView1.Rows[i].Cells[j].Text = k.ToString();
                    GridView1.Rows[i].Cells[j].ForeColor = System.Drawing.Color.FromArgb(255, 255, 255);
                    tem = tem + 1;
                }
            }
            num[j - 3] = tem;
            int a = RadioSelete();
            sum[j - 3] = 50 * tem / a;
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
        output.Write("<td rowspan='2'>期  数</td>");
        output.Write("<td rowspan='2'>开奖号码</td>");
        output.Write("<td rowspan='2'>和<br>值</td>");
        output.Write("<td rowspan='1' colspan ='19' align ='center' valign='middle'><strong><font Font-Bold='false'>百位　＋　十位</font></strong></td>");
        output.Write("<td rowspan='1' colspan ='19' align ='center' valign='middle'><strong><font Font-Bold='false'>十位　＋　个位</font></strong></td>");
        output.Write("<td rowspan='1' colspan ='19' align ='center' valign='middle'><strong><font Font-Bold='false'>百位　＋　个位</font></strong></td>");

        output.Write("<tr align ='center' valign='middle'>");
        for (int j = 0; j < 3; j++)
        {
            for (int i = 0; i < 19; i++)
            {
                output.Write("<td bgcolor ='#E1EFFF'><strong><font color ='#DD0000' Font-Bold='false'>"); 
                output.Write(i.ToString() + "</font></strong></td>");
            }
        }

    }

    protected void DrawGridFooter(HtmlTextWriter output, Control ctl)
    {
        output.Write("<td rowspan = '5' colspan='2'>预测方法：<br>用鼠标点击<br>方格即可显示<br>为开奖号色</td>");
        output.Write("<td>&nbsp</td>");

        for (int i = 0; i < 19; i++)
        {
            output.Write("<td bgcolor='#E3EAF9' onClick=Style(this,'#DD0000','#E3EAF9') style='color:#E3EAF9;'>");
            output.Write(i.ToString() + "</td>");
        }
        for (int i = 0; i < 19; i++)
        {
            output.Write("<td bgcolor='#DCEDD3' onClick=Style(this,'#0000FF','#DCEDD3') style='color:#DCEDD3;'>");
            output.Write(i.ToString() + "</td>");
        }

        for (int i = 0; i < 19; i++)
        {
            output.Write("<td bgcolor='#FFE1F0' onClick=Style(this,'#DD0000','#FFE1F0') style='color:#FFE1F0;'>");
            output.Write(i.ToString() + "</td>");
        }

        for (int J = 0; J < 4; J++)
        {
            output.Write("<tr align ='center' valign='middle'>");
            output.Write("<td bgcolor ='#FFFFFF'>&nbsp</td>");

            for (int i = 0; i < 19; i++)
            {
                output.Write("<td bgcolor='#E3EAF9' onClick=Style(this,'#DD0000','#E3EAF9') style='color:#E3EAF9;'>");
                output.Write(i.ToString() + "</td>");
            }
            for (int i = 0; i < 19; i++)
            {
                output.Write("<td bgcolor='#DCEDD3' onClick=Style(this,'#0000FF','#DCEDD3') style='color:#DCEDD3;'>");
                output.Write(i.ToString() + "</td>");
            }

            for (int i = 0; i < 19; i++)
            {
                output.Write("<td bgcolor='#FFE1F0' onClick=Style(this,'#DD0000','#FFE1F0') style='color:#FFE1F0;'>");
                output.Write(i.ToString() + "</td>");
            }
        }

            output.Write("<tr align = 'center' valign='middle'>");
            output.Write("<td height='50'>出现次数</td>");
            output.Write("<td>&nbsp</td>");
            output.Write("<td>&nbsp</td>");

            for (int i = 0; i < 57; i++)
            {
                output.Write("<td bgcolor='#FFFFFF' align='center' valign='bottom'>");
                output.Write(num[i].ToString() + "<br><img src='../image/01[1].gif' width='8' height='" + sum[i].ToString() + "' title='" + num[i].ToString() + "'></td>");
            }

            output.Write("<tr align = 'center' valign='middle'>");
            output.Write("<td height='20'>期　数</td>");
            output.Write("<td>奖号</td>");
            output.Write("<td>和值</td>");
            for (int j = 0; j < 3; j++)
            {
                for (int i = 0; i < 19; i++)
                {
                    output.Write("<td bgcolor ='#E1EFFF'><strong><font color ='#DD0000' Font-Bold='false'>");
                    output.Write(i.ToString() + "</font></strong></td>");
                }
            }

        }

    protected void RadioButton1_CheckedChanged1(object sender, EventArgs e)
    {
        DataTable dt = BLL.PL3_WH_SeleteByNum(100);
        GridView1.DataSource = dt;
        GridView1.DataBind();
        ColorBind();
    }
    protected void RadioButton2_CheckedChanged(object sender, EventArgs e)
    {
        DataTable dt = BLL.PL3_WH_SeleteByNum(50);
        GridView1.DataSource = dt;
        GridView1.DataBind();
        ColorBind();

    }
    protected void RadioButton3_CheckedChanged(object sender, EventArgs e)
    {
        DataTable dt = BLL.PL3_WH_SeleteByNum(30);
        GridView1.DataSource = dt;
        GridView1.DataBind();
        ColorBind();
    }
    protected void RadioButton4_CheckedChanged1(object sender, EventArgs e)
    {
        DataTable dt = BLL.PL3_WH_SeleteByNum(20);
        GridView1.DataSource = dt;
        GridView1.DataBind();
        ColorBind();
    }
    protected void RadioButton5_CheckedChanged1(object sender, EventArgs e)
    {
        DataTable dt = BLL.PL3_WH_SeleteByNum(10);
        GridView1.DataSource = BLL.PL3_WH_SeleteByNum(10);
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
