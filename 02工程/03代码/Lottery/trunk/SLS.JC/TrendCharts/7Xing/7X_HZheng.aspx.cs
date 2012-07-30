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

public partial class TrendCharts_7Xing_7X_HZheng : TrendChartPageBase
{
    int[] num = new int[42];
    int[] sum = new int[42];

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            GridViewbind();
            GridViewbindColor();
            GridView1.Style["border-collapse"] = "";
        }
    }

    private void GridViewbind()
    {
        GridView1.DataSource = BLL.X7_HZHeng_SeleteByNum(30);
        GridView1.DataBind();
    }

    private void GridViewbindColor()
    {
        for (int i = 0; i < (GridView1.Rows.Count); i++)
            for (int j = 3; j < (GridView1.Columns.Count); j++)
            {
                if (GridView1.Rows[i].Cells[j].Text == "")
                {
                    GridView1.Rows[i].Cells[j].Text = "&nbsp;";
                }
                if (GridView1.Rows[i].Cells[j].Text != "&nbsp;")
                {
                    GridView1.Rows[i].Cells[j].ForeColor = System.Drawing.Color.White;
                    GridView1.Rows[i].Cells[j].BackColor = System.Drawing.Color.FromArgb(255,000,000);
                }
            }

        for (int k = 3; k < 45; k++)
        {
            int a = 0;

            for (int m = 0; m < GridView1.Rows.Count; m++)
            {
                if (GridView1.Rows[m].Cells[k].Text !="&nbsp;")
                {
                    a = a + 1;
                }
            }
            num[k - 3] = a;
            int nn = RadioSelete();
            sum[k - 3] = a * 50 / nn;
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

    private void DrawGridFooter(HtmlTextWriter output, Control ctl)
    {
        int n = RadioSelete();

        output.Write("<td width = '60px' height='50px'>");
        output.Write("出现次数</td>");

        output.Write("<td>&nbsp;</TD>");
        output.Write("<td width = '30px'>&nbsp;</TD>");

        for (int k = 3; k < 45; k++)
        {
            output.Write("<td align='center' valign='bottom'>");
            output.Write(num[k - 3].ToString() + "<br><img src='../image/01[1].gif' height='" + sum[k - 3].ToString() + "px' title = '" + num[k - 3].ToString() + "' width= '8px'></td>");
        }


        output.Write("<tr align='center' valign='middle' height='13px'>");
        output.Write("<td height='13px'>期 数</td>");

        output.Write("<td height='13px'>开奖号码</td>");
        output.Write("<td height='13px'>和值</td>");

        for (int i = 10; i < 52; i++)
        {
            output.Write("<td bgcolor='#E1EFFF' width='14px'><font color='#FF0000'>");
            output.Write(i.ToString() + "</font></td>");
        }
    }

    protected void DrawGridHeader(HtmlTextWriter output, Control ctl)
    {
        output.Write("<td rowspan='2'>期 数</td>");
        
        output.Write("<td rowspan='2'>开奖号码</td>");
        output.Write("<td rowspan='2'>和值</td>");
        output.Write("<td align='center' valign='middle' colspan='42' rowspan='1'>七星彩和值区</td>");

        output.Write("<tr align='center' valign='middle' height='13px'>");
        for (int i = 10; i < 52; i++)
        {
            output.Write("<td bgcolor='#E1EFFF' height='13px'><font color='#FF0000'>");
            output.Write(i.ToString() + "</font></td>");
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

    protected void RadioButton1_CheckedChanged1(object sender, EventArgs e)
    {
        DataTable dt = BLL.X7_HZHeng_SeleteByNum(100);
        GridView1.DataSource = dt;
        GridView1.DataBind();
        GridViewbindColor();
    }

    protected void RadioButton3_CheckedChanged(object sender, EventArgs e)
    {
        DataTable dt = BLL.X7_HZHeng_SeleteByNum(30);
        GridView1.DataSource = dt;
        GridView1.DataBind();
        GridViewbindColor();
    }

    protected void RadioButton2_CheckedChanged(object sender, EventArgs e)
    {
        DataTable dt = BLL.X7_HZHeng_SeleteByNum(50);
        GridView1.DataSource = dt;
        GridView1.DataBind();
        GridViewbindColor();
    }

    protected void RadioButton4_CheckedChanged1(object sender, EventArgs e)
    {
        DataTable dt = BLL.X7_HZHeng_SeleteByNum(20);
        GridView1.DataSource = dt;
        GridView1.DataBind();
        GridViewbindColor();
    }

    protected void RadioButton5_CheckedChanged1(object sender, EventArgs e)
    {
        DataTable dt = BLL.X7_HZHeng_SeleteByNum(10);
        GridView1.DataSource = dt;
        GridView1.DataBind();
        GridViewbindColor();
    }
}
