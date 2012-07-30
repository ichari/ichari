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

public partial class TrendCharts_CJDLT_CJDLT_TeMa : System.Web.UI.Page
{
    int[] num = new int[18];

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
        dt = BLL.CJDLT_TeMa_SeleteByNum(30);
        GridView1.DataSource = dt;
        GridView1.DataBind();
    }

    protected void ColorBind()
    {
        int k = GridView1.Rows.Count;

        for (k = GridView1.Rows.Count; k > 0; k--)
        {
            int j = GridView1.Rows.Count - k;
            GridView1.Rows[j].Cells[0].Text = k.ToString();
        }

        for (int j = 2; j < 11; j++)
        {
            int tem = 0;
            for (int i = 0; i < GridView1.Rows.Count; i++)
            {
                if (GridView1.Rows[i].Cells[j].Text != "0")
                {
                    GridView1.Rows[i].Cells[j].BackColor = System.Drawing.Color.FromArgb(000, 000,255);
                    GridView1.Rows[i].Cells[j].ForeColor = System.Drawing.Color.FromArgb(255, 255, 255);

                    int kk = j - 1;
                    string a = "0" + kk.ToString();
                    GridView1.Rows[i].Cells[j].Text = a.ToString();

                    tem = tem + 1;
                }

                else
                {
                    GridView1.Rows[i].Cells[j].Text ="&nbsp;";
                }
            }
            num[j - 2] = tem;
        }

        for (int j = 11; j < 14; j++)
        {
            int tem = 0;
            for (int i = 0; i < GridView1.Rows.Count; i++)
            {
                if (GridView1.Rows[i].Cells[j].Text != "0")
                {
                    GridView1.Rows[i].Cells[j].BackColor = System.Drawing.Color.FromArgb(000, 000,255);
                    GridView1.Rows[i].Cells[j].ForeColor = System.Drawing.Color.FromArgb(255, 255, 255);
                    int g = j - 1;
                    GridView1.Rows[i].Cells[j].Text = g.ToString();
                    tem = tem + 1;
                }

                else
                {
                    GridView1.Rows[i].Cells[j].Text = "&nbsp;";
                }
            }
            num[j - 2] = tem;
        }

        for (int j = 14; j < 20; j++)
        {
            int w = 0;

            for (int i = 0; i < GridView1.Rows.Count; i++)
            {
                if (GridView1.Rows[i].Cells[j].Text == "1")
                {
                    w = w + 1;
                }

                if (GridView1.Rows[i].Cells[j].Text == "2")
                {
                    w = w + 2;
                }

                if (j > 15)
                {
                    if (GridView1.Rows[i].Cells[j].Text == "0")
                    {
                        GridView1.Rows[i].Cells[j].Text = "&nbsp";
                    }
                }
            }
            num[j-2] = w ;
        }
    }

    protected void RadioButton1_CheckedChanged1(object sender, EventArgs e)
    {
        DataTable dt = BLL.CJDLT_TeMa_SeleteByNum(100);
        GridView1.DataSource = dt;
        GridView1.DataBind();
        ColorBind();
    }
    protected void RadioButton2_CheckedChanged(object sender, EventArgs e)
    {
        DataTable dt = BLL.CJDLT_TeMa_SeleteByNum(50);
        GridView1.DataSource = dt;
        GridView1.DataBind();
        ColorBind();

    }
    protected void RadioButton3_CheckedChanged(object sender, EventArgs e)
    {
        DataTable dt = BLL.CJDLT_TeMa_SeleteByNum(30);
        GridView1.DataSource = dt;
        GridView1.DataBind();
        ColorBind();
    }
    protected void RadioButton4_CheckedChanged1(object sender, EventArgs e)
    {
        DataTable dt = BLL.CJDLT_TeMa_SeleteByNum(20);
        GridView1.DataSource = dt;
        GridView1.DataBind();
        ColorBind();
    }
    protected void RadioButton5_CheckedChanged1(object sender, EventArgs e)
    {
        DataTable dt = BLL.CJDLT_TeMa_SeleteByNum(10);
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
        output.Write("<td rowspan='3'　align='center' valign='middle'>序号</td>");
        output.Write("<td rowspan='3' align='center' valign='middle'>期数</td>");
        output.Write("<td colspan='18' align='center' valign='middle'><strong><font color='#0000FF'>后区号码</font></strong></td>");

        output.Write("<tr align='center' valign='middle'>");

        output.Write("<td colspan='3'>A</td>");
        output.Write("<td colspan='3'>B</td>");
        output.Write("<td colspan='3'>C</td>");
        output.Write("<td colspan='3'>D</td>");
        output.Write("<td colspan='2'>奇偶比</td>");
        output.Write("<td colspan='4'>区域分布</td>");

        output.Write("<tr align='center' valign='middle'>");

        for (int i = 1; i < 10; i++)
        {
            output.Write("<td><font color='#0000FF'>");
            output.Write(0 + i.ToString() + "</font></td>");
        }

        for (int i = 10; i < 13; i++)
        {
            output.Write("<td ><font color='#0000FF'>");
            output.Write(i.ToString() + "</font></td>");
        }

        output.Write("<td>奇</td>");
        output.Write("<td >偶</td>");
        output.Write("<td >一区</td>");
        output.Write("<td >二区</td>");
        output.Write("<td >三区</td>");
        output.Write("<td >四区</td>");
    }

    protected void DrawGridFooter(HtmlTextWriter output, Control ctl)
    {
        output.Write("<td align='center' valign='middle'>序号</td>");
        output.Write("<td align='center' valign='middle'>出现次数</td>");

        for (int i = 0; i < 18; i++)
        {
            output.Write("<td align='center' valign='middle'>");
            output.Write(num[i].ToString() + "</td>");

        }
    }
}




