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

public partial class TC22X5_22X5_HZ_Heng : System.Web.UI.Page
{
    int[] num = new int[51];
    int[] sum = new int[51];

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
        GridView1.DataSource = BLL.TC22X5_HZ_Heng_SeleteByNum(30);
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

        for (int k = 2; k < 53; k++)
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
            k = 50;
        }

        if (RadioButton2.Checked == true)
        {
            k = 30;
        }

        if (RadioButton3.Checked == true)
        {
            k = 20;
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

        output.Write("<td height='60px'>出现次数</td>");
        
        output.Write("<td>&nbsp;</TD>");
       
        for (int k = 3; k < 54; k++)
        {
            output.Write("<td align='center' valign='bottom'>");
            output.Write(num[k - 3].ToString() + "<br><img src='../image/01[1].gif' height='" + sum[k - 3].ToString() + "px' title = '" + num[k - 3].ToString() + "' width= '8px'></td>");
        }


        output.Write("<tr align='center' valign='middle'>");
        output.Write("<td>期 数</td>");

        output.Write("<td>开奖号码</td>");
       
        for (int i = 35; i < 86; i++)
        {
            output.Write("<td bgcolor='#E1EFFF' width='10px'><font color='#FF0000'>");
            output.Write(i.ToString() + "</font></td>");
        }
    }

    protected void DrawGridHeader(HtmlTextWriter output, Control ctl)
    {
        output.Write("<td rowspan='2'>期 数</td>");

        output.Write("<td rowspan='2'>开奖号码</td>");
      
        output.Write("<td align='center' valign='middle' colspan='51' rowspan='1'>体彩22选5和值区</td>");

        output.Write("<tr align='center' valign='middle'>");
        for (int i = 35; i < 86; i++)
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
        DataTable dt = BLL.TC22X5_HZ_Heng_SeleteByNum(100);
        GridView1.DataSource = dt;
        GridView1.DataBind();
        GridViewbindColor();
    }

    protected void RadioButton2_CheckedChanged(object sender, EventArgs e)
    {
        DataTable dt = BLL.TC22X5_HZ_Heng_SeleteByNum(50);
        GridView1.DataSource = dt;
        GridView1.DataBind();
        GridViewbindColor();
    }

    protected void RadioButton3_CheckedChanged(object sender, EventArgs e)
    {
        DataTable dt = BLL.TC22X5_HZ_Heng_SeleteByNum(30);
        GridView1.DataSource = dt;
        GridView1.DataBind();
        GridViewbindColor();
    }

    protected void RadioButton4_CheckedChanged1(object sender, EventArgs e)
    {
        DataTable dt = BLL.TC22X5_HZ_Heng_SeleteByNum(20);
        GridView1.DataSource = dt;
        GridView1.DataBind();
        GridViewbindColor();
    }

    protected void RadioButton5_CheckedChanged1(object sender, EventArgs e)
    {
        DataTable dt = BLL.TC22X5_HZ_Heng_SeleteByNum(10);
        GridView1.DataSource = dt;
        GridView1.DataBind();
        GridViewbindColor();
    }
}
