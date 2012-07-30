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

public partial class TrendCharts_CJDLT_CJDLT_jima : System.Web.UI.Page
{
    int[] num = new int[35];
    int[] sum = new int[35];
    int[] tem = new int[9];

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
        dt = BLL.CJDLT_jima_SeleteByNum(30);
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
                    GridView1.Rows[i].Cells[j].BackColor = System.Drawing.Color.FromArgb(225, 000, 000);
                    GridView1.Rows[i].Cells[j].ForeColor = System.Drawing.Color.FromArgb(255, 255, 255);

                    int kk = j - 1;
                    string a = "0" + kk.ToString();
                    GridView1.Rows[i].Cells[j].Text = a.ToString();

                    tem = tem + 1;
                }
            }
            num[j - 2] = tem;
            int e = RadioSelete();
            sum[j - 2] = 50 * tem / e;
        }

        for (int j = 11; j < 37; j++)
        {
            int tem = 0;
            for (int i = 0; i < GridView1.Rows.Count; i++)
            {
                if (GridView1.Rows[i].Cells[j].Text != "0")
                {
                    GridView1.Rows[i].Cells[j].BackColor = System.Drawing.Color.FromArgb(225, 000, 000);
                    GridView1.Rows[i].Cells[j].ForeColor = System.Drawing.Color.FromArgb(255, 255, 255);
                    int g = j - 1;
                    GridView1.Rows[i].Cells[j].Text = g.ToString();
                    tem = tem + 1;
                }
            }
            num[j - 2] = tem;
            int e = RadioSelete();
            sum[j - 2] = 50 * tem / e;
        }

        for (int i = 0; i < GridView1.Rows.Count; i++)
        {
            for (int j = 2; j < 37; j++)
            {
                if (GridView1.Rows[i].Cells[j].Text == "0")
                {
                    GridView1.Rows[i].Cells[j].Text = "&nbsp";
                }
            }

            for (int j = 39; j < 46; j++)
            {
                if (GridView1.Rows[i].Cells[j].Text == "0")
                {
                    GridView1.Rows[i].Cells[j].Text = "&nbsp";
                }
            }
        }

        for (int i = 0; i < GridView1.Rows.Count; i++)
        {
            tem[0] = tem[0] + Shove._Convert.StrToInt(GridView1.Rows[i].Cells[37].Text, 0);
            tem[1] = tem[1] + Shove._Convert.StrToInt(GridView1.Rows[i].Cells[38].Text, 0);
            tem[2] = tem[2] + Shove._Convert.StrToInt(GridView1.Rows[i].Cells[39].Text, 0);
            tem[3] = tem[3] + Shove._Convert.StrToInt(GridView1.Rows[i].Cells[40].Text, 0);
            tem[4] = tem[4] + Shove._Convert.StrToInt(GridView1.Rows[i].Cells[41].Text, 0);
            tem[5] = tem[5] + Shove._Convert.StrToInt(GridView1.Rows[i].Cells[42].Text, 0);
            tem[6] = tem[6] + Shove._Convert.StrToInt(GridView1.Rows[i].Cells[43].Text, 0);
            tem[7] = tem[7] + Shove._Convert.StrToInt(GridView1.Rows[i].Cells[44].Text, 0);
            tem[8] = tem[8] + Shove._Convert.StrToInt(GridView1.Rows[i].Cells[45].Text, 0);
        }
    }

    protected void RadioButton1_CheckedChanged1(object sender, EventArgs e)
    {
        DataTable dt = BLL.CJDLT_jima_SeleteByNum(100);
        GridView1.DataSource = dt;
        GridView1.DataBind();
        ColorBind();
    }
    protected void RadioButton2_CheckedChanged(object sender, EventArgs e)
    {
        DataTable dt = BLL.CJDLT_jima_SeleteByNum(50);
        GridView1.DataSource = dt;
        GridView1.DataBind();
        ColorBind();

    }
    protected void RadioButton3_CheckedChanged(object sender, EventArgs e)
    {
        DataTable dt = BLL.CJDLT_jima_SeleteByNum(30);
        GridView1.DataSource = dt;
        GridView1.DataBind();
        ColorBind();
    }
    protected void RadioButton4_CheckedChanged1(object sender, EventArgs e)
    {
        DataTable dt = BLL.CJDLT_jima_SeleteByNum(20);
        GridView1.DataSource = dt;
        GridView1.DataBind();
        ColorBind();
    }
    protected void RadioButton5_CheckedChanged1(object sender, EventArgs e)
    {
        DataTable dt = BLL.CJDLT_jima_SeleteByNum(10);
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
        output.Write("<td colspan='44' align='center' valign='middle'><strong><font color='#FF0000'>前区号码</font></strong></td>");

        output.Write("<tr align='center' valign='middle'>");

        output.Write("<td colspan='5'>一区</td>");
        output.Write("<td colspan='5'>二区</td>");
        output.Write("<td colspan='5'>三区</td>");
        output.Write("<td colspan='5'>四区</td>");
        output.Write("<td colspan='5'>五区</td>");
        output.Write("<td colspan='5'>六区</td>");
        output.Write("<td colspan='5'>七区</td>");
        output.Write("<td colspan='2'>奇偶</td>");
        output.Write("<td colspan='7'>区域分布</td>");

        output.Write("<tr align='center' valign='middle'>");

        for (int i = 1; i < 10; i++)
        {
            output.Write("<td><font color='#FF0000'>");
            output.Write(0 + i.ToString() + "</font></td>");
        }
        for (int i = 10; i < 36; i++)
        {
            output.Write("<td ><font color='#FF0000'>");
            output.Write(i.ToString() + "</font></td>");
        }

        output.Write("<td>奇</td>");
        output.Write("<td >偶</td>");
        output.Write("<td >一区</td>");
        output.Write("<td >二区</td>");
        output.Write("<td >三区</td>");
        output.Write("<td >四区</td>");
        output.Write("<td >五区</td>");
        output.Write("<td >六区</td>");
        output.Write("<td >七区</td>");       
    }

    protected void DrawGridFooter(HtmlTextWriter output, Control ctl)
    {
        output.Write("<td align='center' valign='middle'>序号</td>");
        output.Write("<td height='50px' align='center' valign='middle'>出现次数</td>");

        for (int i = 0; i < 35; i++)
        {
            output.Write("<td align='center' valign='bottom'>");
            output.Write(num[i].ToString() + "<br><img src='../../Images/01[1].gif' height='" + sum[i].ToString() + "'' title='" + num[i].ToString() + "' width='8px'></td>");
        }

        for (int i = 0; i < 9; i++)
        {
            output.Write("<td align='center' valign='bottom'>");
            output.Write(tem[i].ToString()+"</td>");
        }

        output.Write("<tr align='center' valign='middle'>");
        output.Write("<td>序号</td>");
        output.Write("<td>期 数</td>");

        for (int i = 1; i < 10; i++)
        {
            output.Write("<td><font color='#FF0000'>");
            output.Write(0 + i.ToString() + "</font></td>");
        }

        for (int i = 10; i < 36; i++)
        {
            output.Write("<td ><font color='#FF0000'>");
            output.Write(i.ToString() + "</font></td>");
        }

        output.Write("<td>奇</td>");
        output.Write("<td >偶</td>");
        output.Write("<td >一区</td>");
        output.Write("<td >二区</td>");
        output.Write("<td >三区</td>");
        output.Write("<td >四区</td>");
        output.Write("<td >五区</td>");
        output.Write("<td >六区</td>");
        output.Write("<td >七区</td>");
    }

    private int RadioSelete()
    {
        int k = 1;

        if (RadioButton1.Checked == true)
        {

            k = 100;
        }

        if (RadioButton2.Checked == true)
        {
            k = 50;
        }

        if (RadioButton3.Checked == true)
        {
            k = 30;
        }

        if (RadioButton4.Checked == true)
        {
            k = 20;
        }

        if (RadioButton5.Checked == true)
        {
            k = 10;
        }

        return k;

    }
}
