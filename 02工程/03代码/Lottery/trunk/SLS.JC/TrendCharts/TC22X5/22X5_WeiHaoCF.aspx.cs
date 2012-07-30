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

public partial class TC22X5_22X5_WeiHaoCF : System.Web.UI.Page
{
    int[] num = new int[10];
    int[] sum = new int[10];

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

        GridView1.DataSource = BLL.TC22X5_WeiHaoCF_SeleteByNum(30);
        GridView1.DataBind();
    }

    private void GridViewbindColor()
    {
        for (int i = 0; i < (GridView1.Rows.Count); i++)
        {
            for (int j = 2; j < (GridView1.Columns.Count); j++)
            {
               if (GridView1.Rows[i].Cells[j].Text == "0")
                {
                    GridView1.Rows[i].Cells[j].Text = "&nbsp;";
                }

                if (GridView1.Rows[i].Cells[j].Text == "1")
                {
                    GridView1.Rows[i].Cells[j].ForeColor = System.Drawing.Color.Black;
                    GridView1.Rows[i].Cells[j].BackColor = System.Drawing.Color.FromArgb(223, 223, 223);
                }

                if (GridView1.Rows[i].Cells[j].Text == "2")
                {
                    GridView1.Rows[i].Cells[j].ForeColor = System.Drawing.Color.White;
                    GridView1.Rows[i].Cells[j].BackColor = System.Drawing.Color.FromArgb(221, 000, 000);
                }

                if (GridView1.Rows[i].Cells[j].Text == "3")
                {
                    GridView1.Rows[i].Cells[j].ForeColor = System.Drawing.Color.White;
                    GridView1.Rows[i].Cells[j].BackColor = System.Drawing.Color.FromArgb(000, 000, 255);
                }
            }
        }
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
            k = 30;
        }

        if (RadioButton3.Checked == true)
        {
            k = 50;
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

    private void DrawGridFooter(HtmlTextWriter output, Control ctl)
    {
        int n = RadioSelete();

        output.Write("<td width = '100px' height='50px'>");
        output.Write("出现次数</td>");

        output.Write("<td width = '100px'>");
        output.Write("&nbsp</td>");

        for (int k = 2; k < GridView1.Columns.Count; k++)
        {
            int a = 0;

            for (int m = 0; m < GridView1.Rows.Count; m++)
            {
                if (GridView1.Rows[m].Cells[k].Text == "0" || GridView1.Rows[m].Cells[k].Text == "1")
                {
                    a = a + 0;
                    num[k - 2] = a;
                    sum[k - 2] = a * 50 / n;
                }

                if (GridView1.Rows[m].Cells[k].Text == "2" || GridView1.Rows[m].Cells[k].Text == "3" || GridView1.Rows[m].Cells[k].Text == "4" || GridView1.Rows[m].Cells[k].Text == "5")
                {
                    a = a + 1;
                    num[k - 2] = a;
                    sum[k - 2] = a * 50 / n;
                }
            }

            output.Write("<td align='center' valign='bottom'>");
            output.Write(num[k - 2].ToString() + "<br><img src='../image/01[1].gif' height='" + sum[k - 2].ToString() + "px' title = '" + num[k - 2].ToString() + "' width= '8px'></td>");
        }
    }

    protected void GridView1_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.SetRenderMethodDelegate(new RenderMethod(DrawGridFooter));
        }
    }

    protected void RadioButton1_CheckedChanged1(object sender, EventArgs e)
    {
        DataTable dt = BLL.TC22X5_WeiHaoCF_SeleteByNum(100);
        GridView1.DataSource = dt;
        GridView1.DataBind();
        GridViewbindColor();
    }

    protected void RadioButton2_CheckedChanged(object sender, EventArgs e)
    {
        DataTable dt = BLL.TC22X5_WeiHaoCF_SeleteByNum(50);
        GridView1.DataSource = dt;
        GridView1.DataBind();
        GridViewbindColor();
    }

    protected void RadioButton3_CheckedChanged(object sender, EventArgs e)
    {
        DataTable dt = BLL.TC22X5_WeiHaoCF_SeleteByNum(30);
        GridView1.DataSource = dt;
        GridView1.DataBind();
        GridViewbindColor();
    }

    protected void RadioButton4_CheckedChanged1(object sender, EventArgs e)
    {
        DataTable dt = BLL.TC22X5_WeiHaoCF_SeleteByNum(20);
        GridView1.DataSource = dt;
        GridView1.DataBind();
        GridViewbindColor();
    }

    protected void RadioButton5_CheckedChanged1(object sender, EventArgs e)
    {
        DataTable dt = BLL.TC22X5_WeiHaoCF_SeleteByNum(10);
        GridView1.DataSource = dt;
        GridView1.DataBind();
        GridViewbindColor();
    }
}
