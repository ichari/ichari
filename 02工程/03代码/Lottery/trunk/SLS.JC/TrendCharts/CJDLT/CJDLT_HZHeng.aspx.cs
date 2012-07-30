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

public partial class TrendCharts_CJDLT_CJDLT_HZHeng : System.Web.UI.Page
{
    int[] num = new int[151];
    int[] sum = new int[151];

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
        GridView1.DataSource = BLL.CJDLT_HZHeng_SeleteByNum(30);
        GridView1.DataBind();
    }

    private void GridViewbindColor()
    {
        for (int i = 0; i < (GridView1.Rows.Count); i++)
            for (int j = 2; j < (GridView1.Columns.Count); j++)
            {
                if (GridView1.Rows[i].Cells[j].Text == "")
                {
                    GridView1.Rows[i].Cells[j].Text = "&nbsp;";
                }
                if (GridView1.Rows[i].Cells[j].Text != "&nbsp;")
                {
                    GridView1.Rows[i].Cells[j].ForeColor = System.Drawing.Color.White;
                    GridView1.Rows[i].Cells[j].BackColor = System.Drawing.Color.FromArgb(255, 000, 000);
                }
            }

        for (int k = 2; k < 151; k++)
        {
            int a = 0;

            for (int m = 0; m < GridView1.Rows.Count; m++)
            {
                if (GridView1.Rows[m].Cells[k].Text != "&nbsp;")
                {
                    a = a + 1;
                }
            }
            num[k - 2] = a;
            int nn = RadioSelete();
            sum[k - 2] = a * 50 / nn;
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
            k = 20;
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

        output.Write("<td height='50px' bgcolor='#F2F2F2'>出现次数</td>");

        output.Write("<td bgcolor='#F2F2F2'>&nbsp;</td>");

        for (int k = 0; k < 151; k++)
        {
            output.Write("<td align='center' valign='bottom'>");
            output.Write(num[k].ToString() + "<br><img src='../../images/01[1].gif' height='" + sum[k].ToString() + "px' title = '" + num[k].ToString() + "' width= '8px'></td>");
        }


        output.Write("<tr align='center' valign='middle'>");
        output.Write("<td bgcolor='#F2F2F2'>期 数</td>");

        output.Write("<td bgcolor='#F2F2F2'>开奖号码</td>");

        for (int i = 15; i < 166; i++)
        {
            output.Write("<td bgcolor='#E1EFFF' width='10px'><font color='#FF0000'>");
            output.Write(i.ToString() + "</font></td>");
        }
    }

    protected void DrawGridHeader(HtmlTextWriter output, Control ctl)
    {
        output.Write("<td rowspan='2'bgcolor='#F2F2F2'>期 数</td>");

        output.Write("<td rowspan='2'bgcolor='#F2F2F2'>开奖号码</td>");

        output.Write("<td align='center' valign='middle' colspan='151' rowspan='1' bgcolor='#F2F2F2'>超级大乐透和值区</td>");

        output.Write("<tr align='center' valign='middle'>");
        for (int i = 15; i < 166; i++)
        {
            output.Write("<td bgcolor='#E1EFFF' align='center' valign='middle'><font color='#FF0000'>");
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
        DataTable dt = BLL.CJDLT_HZHeng_SeleteByNum(100);
        GridView1.DataSource = dt;
        GridView1.DataBind();
        GridViewbindColor();
    }

 protected void RadioButton2_CheckedChanged(object sender, EventArgs e)
    {
        DataTable dt = BLL.CJDLT_HZHeng_SeleteByNum(50);
        GridView1.DataSource = dt;
        GridView1.DataBind();
        GridViewbindColor();
    }

    protected void RadioButton3_CheckedChanged(object sender, EventArgs e)
    {
        DataTable dt = BLL.CJDLT_HZHeng_SeleteByNum(30);
        GridView1.DataSource = dt;
        GridView1.DataBind();
        GridViewbindColor();
    }

    protected void RadioButton4_CheckedChanged1(object sender, EventArgs e)
    {
        DataTable dt = BLL.CJDLT_HZHeng_SeleteByNum(20);
        GridView1.DataSource = dt;
        GridView1.DataBind();
        GridViewbindColor();
    }

    protected void RadioButton5_CheckedChanged1(object sender, EventArgs e)
    {
        DataTable dt = BLL.CJDLT_HZHeng_SeleteByNum(10);
        GridView1.DataSource = dt;
        GridView1.DataBind();
        GridViewbindColor();
    }

    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //Label lb = (Label)e.Row.Cells[1].FindControl("LotteryNumber");

            //lb.Text = lb.Text.Replace(" ", "");

            e.Row.Cells[1].Text = e.Row.Cells[1].Text.ToString().Replace(" ", "&nbsp;");
        }
    }
}
