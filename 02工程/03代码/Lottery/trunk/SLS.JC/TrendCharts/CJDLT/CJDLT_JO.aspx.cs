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

public partial class TrendCharts_CJDLT_CJDLT_JO : System.Web.UI.Page
{
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
        dt = BLL.CJDLT_JO_SeleteByNum(30);
        GridView1.DataSource = dt;
        GridView1.DataBind();
    }

    protected void ColorBind()
    {
        for (int j = 3; j < 8; j++)
        {
            for (int i = 0; i < GridView1.Rows.Count; i++)
            {
                if (GridView1.Rows[i].Cells[j].Text == "奇")
                {
                    GridView1.Rows[i].Cells[j].ForeColor = System.Drawing.Color.FromArgb(255, 000, 000);
                }

                if (GridView1.Rows[i].Cells[j].Text == "偶")
                {
                    GridView1.Rows[i].Cells[j].ForeColor = System.Drawing.Color.FromArgb(000, 000,255);
                }
            }

        }
    }

    protected void RadioButton1_CheckedChanged1(object sender, EventArgs e)
    {
        DataTable dt = BLL.CJDLT_JO_SeleteByNum(100);
        GridView1.DataSource = dt;
        GridView1.DataBind();
        ColorBind();
    }
    protected void RadioButton2_CheckedChanged(object sender, EventArgs e)
    {
        DataTable dt = BLL.CJDLT_JO_SeleteByNum(50);
        GridView1.DataSource = dt;
        GridView1.DataBind();
        ColorBind();

    }
    protected void RadioButton3_CheckedChanged(object sender, EventArgs e)
    {
        DataTable dt = BLL.CJDLT_JO_SeleteByNum(30);
        GridView1.DataSource = dt;
        GridView1.DataBind();
        ColorBind();
    }
    protected void RadioButton4_CheckedChanged1(object sender, EventArgs e)
    {
        DataTable dt = BLL.CJDLT_JO_SeleteByNum(20);
        GridView1.DataSource = dt;
        GridView1.DataBind();
        ColorBind();
    }
    protected void RadioButton5_CheckedChanged1(object sender, EventArgs e)
    {
        DataTable dt = BLL.CJDLT_JO_SeleteByNum(10);
        GridView1.DataSource = dt;
        GridView1.DataBind();
        ColorBind();
    }

    protected void GridView1_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            e.Row.SetRenderMethodDelegate(new RenderMethod(DrawGridHeader));
        }
    }

    protected void DrawGridHeader(HtmlTextWriter output, Control ctl)
    {
        output.Write("<td rowspan='2' align='center' valign='middle'>期数</td>");
        output.Write("<td colspan='2' align='center' valign='middle'>开奖号码</td>");
        output.Write("<td colspan='5' align='center' valign='middle'>前区号码区</td>");
        output.Write("<td rowspan='2' align='center' valign='middle'>奇数个数</td>");
        output.Write("<td rowspan='2' align='center' valign='middle'>偶数个数</td>");
        output.Write("<td rowspan='2' align='center' valign='middle'>奇偶比</td>");
        output.Write("<td rowspan='2' align='center' valign='middle'>奇偶差值</td>");
        output.Write("<td colspan='2' align='center' valign='middle'>后区奇偶</td>");

        output.Write("<tr align='center' valign='middle'>");

        output.Write("<td>前区</td>");
        output.Write("<td>后区</td>");
        output.Write("<td><font color='#FF0000'>第一位</font></td>");
        output.Write("<td><font color='#FF0000'>第二位</font></td>");
        output.Write("<td><font color='#FF0000'>第三位</font></td>");
        output.Write("<td><font color='#FF0000'>第四位</font></td>");
        output.Write("<td><font color='#FF0000'>第五位</font></td>");
        output.Write("<td>奇</td>");
        output.Write("<td>偶</td>");
    }
}
