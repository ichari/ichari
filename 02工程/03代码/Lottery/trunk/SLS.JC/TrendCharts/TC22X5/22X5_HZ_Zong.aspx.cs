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

public partial class TC22X5_22X5_HZ_Zong : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            GridViewBind();

            GridView1.Style["border-collapse"] = "";
        }
    }

    private void GridViewBind()
    {
        GridView1.DataSource = BLL.TC22X5_HZZong_SeleteByNum(30);
        GridView1.DataBind();
    }

    protected void RadioButton1_CheckedChanged1(object sender, EventArgs e)
    {
        DataTable dt = BLL.TC22X5_HZZong_SeleteByNum(100);

        GridView1.DataSource = dt;

        GridView1.DataBind();

    }

    protected void RadioButton2_CheckedChanged(object sender, EventArgs e)
    {
        DataTable dt = BLL.TC22X5_HZZong_SeleteByNum(50);

        GridView1.DataSource = dt;

        GridView1.DataBind();

    }

    protected void RadioButton3_CheckedChanged(object sender, EventArgs e)
    {
        DataTable dt = BLL.TC22X5_HZZong_SeleteByNum(30);

        GridView1.DataSource = dt;

        GridView1.DataBind();

    }

    protected void RadioButton4_CheckedChanged1(object sender, EventArgs e)
    {
        DataTable dt = BLL.TC22X5_HZZong_SeleteByNum(20);

        GridView1.DataSource = dt;

        GridView1.DataBind();

    }

    protected void RadioButton5_CheckedChanged1(object sender, EventArgs e)
    {
        DataTable dt = BLL.TC22X5_HZZong_SeleteByNum(10);

        GridView1.DataSource = dt;

        GridView1.DataBind();

    }

    protected void GridView1_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            e.Row.SetRenderMethodDelegate(new RenderMethod(DrawGridHeader));
        }
    }

    private void DrawGridHeader(HtmlTextWriter output, Control ctl)
    {
        output.Write("<td width='60' rowspan='2' align='center' valign='middle'>期  数</td>");

        output.Write("<td width='100' rowspan='2' align='center' valign='middle'>");
        output.Write("开奖号码</td>");

        output.Write("<td colspan='2' bgcolor='#A4DCFD' align='center' valign='middle'>体彩22选5</td>");

        output.Write("<tr align='center' valign='middle'>");

        output.Write("<td bgcolor='#E1EFFF'><font color='#FF0000'>和值</font></td>");
        output.Write("<td bgcolor='#E8F5FF'><font color='#FF0000'>均值</font></td>");
    }
}
