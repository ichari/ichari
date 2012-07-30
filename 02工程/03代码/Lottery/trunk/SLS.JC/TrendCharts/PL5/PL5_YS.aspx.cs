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

public partial class PL5_PL5_YS : TrendChartPageBase
{
    int[] sum = new int[15];

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            GridViewBind();
            ColorBind();
            GridView1.Style["border-collapse"] = "";
        }
    }

    private void GridViewBind()
    {
        GridView1.DataSource = BLL.PL5_YS_SeleteByNum(30);
        GridView1.DataBind();
    }

    private void ColorBind()
    {
        for (int k = 0; k < GridView1.Rows.Count; k++)
        {
            for (int m = 2; m < 9; m++)
            {
                GridView1.Rows[k].Cells[m].BackColor = System.Drawing.Color.FromArgb(237, 255, 215);
            }
        }

        for (int k = 0; k < GridView1.Rows.Count; k++)
        {
            for (int m = 9; m < 14; m++)
            {
                GridView1.Rows[k].Cells[m].BackColor = System.Drawing.Color.FromArgb(255, 255, 255);
            }
        }

        for (int k = 0; k < GridView1.Rows.Count; k++)
        {
            for (int m = 14; m < 17; m++)
            {
                GridView1.Rows[k].Cells[m].BackColor = System.Drawing.Color.FromArgb(210, 225, 240);
            }
        }

        for (int i = 2; i < GridView1.Columns.Count; i++)
            for (int j = 0; j < GridView1.Rows.Count; j++)
            {
                GridView1.Rows[j].Cells[1].ForeColor = System.Drawing.Color.Blue;

                if (GridView1.Rows[j].Cells[i].Text == "")
                {
                    //GridView1.Rows[j].Cells[i].ForeColor = System.Drawing.Color.FromArgb(255, 000, 000);
                    GridView1.Rows[j].Cells[i].Text = " & nbsp; ";
                }

                if (GridView1.Rows[j].Cells[i].Text == "1")
                {
                    GridView1.Rows[j].Cells[i].ForeColor = System.Drawing.Color.FromArgb(255, 000, 000);
                }

                if (GridView1.Rows[j].Cells[i].Text == "2")
                {
                    GridView1.Rows[j].Cells[i].ForeColor = System.Drawing.Color.FromArgb(000, 000, 255);
                }

                if (GridView1.Rows[j].Cells[i].Text == "3")
                {
                    GridView1.Rows[j].Cells[i].ForeColor = System.Drawing.Color.FromArgb(128, 128, 064);
                }
            }
    }

    protected void RadioButton1_CheckedChanged1(object sender, EventArgs e)
    {
        DataTable dt = BLL.PL5_YS_SeleteByNum(100);
        GridView1.DataSource = dt;
        GridView1.DataBind();
        ColorBind();
    }

    protected void RadioButton2_CheckedChanged(object sender, EventArgs e)
    {
        DataTable dt = BLL.PL5_YS_SeleteByNum(50);
        GridView1.DataSource = dt;
        GridView1.DataBind();
        ColorBind();
    }

    protected void RadioButton3_CheckedChanged(object sender, EventArgs e)
    {
        DataTable dt = BLL.PL5_YS_SeleteByNum(30);
        GridView1.DataSource = dt;
        GridView1.DataBind();
        ColorBind();
    }

    protected void RadioButton4_CheckedChanged1(object sender, EventArgs e)
    {
        DataTable dt = BLL.PL5_YS_SeleteByNum(20);
        GridView1.DataSource = dt;
        GridView1.DataBind();
        ColorBind();
    }

    protected void RadioButton5_CheckedChanged1(object sender, EventArgs e)
    {
        DataTable dt = BLL.PL5_YS_SeleteByNum(10);
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

    private void DrawGridFooter(HtmlTextWriter output, Control ctl)
    {
        output.Write("<td >");
        output.Write("出现次数</td>");

        output.Write("<td>");
        output.Write("&nbsp</td>");

        for (int i = 2; i < 9; i++)
        {
            int num = 0;

            for (int j = 0; j < GridView1.Rows.Count; j++)
            {
                num = num + Shove._Convert.StrToInt(GridView1.Rows[j].Cells[i].Text.ToString(), 0);
            }

            sum[i - 2] = num;

            output.Write("<td align='center' style='background-color: #F3F3F3'>");
            output.Write(sum[i - 2].ToString() + "</td>");
        }

        for (int i = 9; i < 14; i++)
        {
            int Num = 0;

            for (int j = 0; j < GridView1.Rows.Count; j++)
            {
                Num = Num + Shove._Convert.StrToInt(GridView1.Rows[j].Cells[i].Text.ToString(), 0);
            }

            sum[i - 2] = Num;

            output.Write("<td align='center' style='background-color:#F3F3F3'>");
            output.Write(sum[i - 2].ToString() + "</td>");
        }

        for (int i = 14; i < 17; i++)
        {
            int tem = 0;

            for (int j = 0; j < GridView1.Rows.Count; j++)
            {
                tem = tem + Shove._Convert.StrToInt(GridView1.Rows[j].Cells[i].Text.ToString(), 0);
            }

            sum[i - 2] = tem;

            output.Write("<td align='center'style='background-color:#F3F3F3'>");
            output.Write(sum[i - 2].ToString() + "</td>");
        }
    }

    private void DrawGridHeader(HtmlTextWriter output, Control ctl)
    {
        //output.Write("<tr align='center' valign='middle'>");
        output.Write("<td width='60' rowspan='2'>期  数</td>");

        output.Write("<td width='60' rowspan='2'>");
        output.Write("开奖号码</td>");

        output.Write("<td colspan='7'>");
        output.Write("除以<strong><font color='#FF0000'>7</font></strong></td>");

        output.Write("<td colspan='5'>");
        output.Write("除以<strong><font color='#FF0000'>5</font></strong></td>");

        output.Write("<td colspan='3'>");
        output.Write("除以<strong><font color='#FF0000'>3</font></strong></td></tr>");

        output.Write("<tr align='center' valign='middle'>");

        output.Write("<td width='16' align='center' valign='middle' bgColor=#D9F9A4>");
        output.Write("0</td>");
        output.Write("<td align='center' valign='middle' bgColor=#D9F9A4>");
        output.Write("1</td>");
        output.Write("<td align='center' valign='middle' bgColor=#D9F9A4>");
        output.Write("2</td>");
        output.Write("<td align='center' valign='middle' bgColor=#D9F9A4>");
        output.Write("3</td>");
        output.Write("<td align='center' valign='middle' bgColor=#D9F9A4>");
        output.Write("4</td>");
        output.Write("<td align='center' valign='middle' bgColor=#D9F9A4>");
        output.Write("5</td>");
        output.Write("<td align='center' valign='middle' bgColor=#D9F9A4> ");
        output.Write("6</td>");


        output.Write("<td align='center' valign='middle' bgColor=#F6F6F6>");
        output.Write("0</td>");
        output.Write("<td align='center' valign='middle' bgColor=#F6F6F6>");
        output.Write("1</td>");
        output.Write("<td align='center' valign='middle' bgColor=#F6F6F6>");
        output.Write("2</td>");
        output.Write("<td align='center' valign='middle' bgColor=#F6F6F6 >");
        output.Write("3</td>");
        output.Write("<td align='center' valign='middle' bgColor=#F6F6F6>");
        output.Write("4</td>");

        output.Write("<td align='center' valign='middle' bgColor=#BBD1E8>");
        output.Write("0</td>");
        output.Write("<td align='center' valign='middle' bgColor=#BBD1E8>");
        output.Write("1</td>");
        output.Write("<td align='center' valign='middle' bgColor=#BBD1E8>");
        output.Write("2</td></tr>");
    }
}
