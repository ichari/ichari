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

public partial class TC22X5_22X5_YS : System.Web.UI.Page
{
    int[] num = new int[28];

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
        dt = BLL.TC22X5_YS_SeleteByNum(30);
        GridView1.DataSource = dt;
        GridView1.DataBind();
    }

    protected void ColorBind()
    {
        for (int j = 2; j < 30; j++)
        {
            int k = 0;
            for (int i = 0; i < GridView1.Rows.Count; i++)
            {
                if (GridView1.Rows[i].Cells[j].Text == "0")
                {
                    GridView1.Rows[i].Cells[j].Text = "&nbsp;";
                }

                if (GridView1.Rows[i].Cells[j].Text == "1")
                {
                    k = k + 1;
                   
                    GridView1.Rows[i].Cells[j].ForeColor = System.Drawing.Color.FromArgb(255, 000, 000);
                }
                if (GridView1.Rows[i].Cells[j].Text == "2")
                {
                    k = k + 2;

                    GridView1.Rows[i].Cells[j].ForeColor = System.Drawing.Color.FromArgb(000, 000,255);
                }
                if (GridView1.Rows[i].Cells[j].Text == "3")
                {
                    k = k + 3;

                    GridView1.Rows[i].Cells[j].ForeColor = System.Drawing.Color.FromArgb(255,153,000);
                }
                if (GridView1.Rows[i].Cells[j].Text == "4")
                {
                    k = k + 4;

                    GridView1.Rows[i].Cells[j].ForeColor = System.Drawing.Color.FromArgb(051, 153, 000);
                }
                if (GridView1.Rows[i].Cells[j].Text == "5")
                {
                    k = k + 5;
            
                    GridView1.Rows[i].Cells[j].ForeColor = System.Drawing.Color.FromArgb(212, 208, 200);
                }
            }
            num[j - 2] = k;
        }
    }

    protected void RadioButton1_CheckedChanged1(object sender, EventArgs e)
    {
        DataTable dt = BLL.TC22X5_YS_SeleteByNum(100);
        GridView1.DataSource = dt;
        GridView1.DataBind();
        ColorBind();
    }
    protected void RadioButton2_CheckedChanged(object sender, EventArgs e)
    {
        DataTable dt = BLL.TC22X5_YS_SeleteByNum(50);
        GridView1.DataSource = dt;
        GridView1.DataBind();
        ColorBind();

    }
    protected void RadioButton3_CheckedChanged(object sender, EventArgs e)
    {
        DataTable dt = BLL.TC22X5_YS_SeleteByNum(30);
        GridView1.DataSource = dt;
        GridView1.DataBind();
        ColorBind();
    }
    protected void RadioButton4_CheckedChanged1(object sender, EventArgs e)
    {
        DataTable dt = BLL.TC22X5_YS_SeleteByNum(20);
        GridView1.DataSource = dt;
        GridView1.DataBind();
        ColorBind();
    }
    protected void RadioButton5_CheckedChanged1(object sender, EventArgs e)
    {
        DataTable dt = BLL.TC22X5_YS_SeleteByNum(10);
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
       
        output.Write("<td rowspan='2' align='center' valign='middle'>期数</td>");
        output.Write("<td rowspan='2' align='center' valign='middle'>开奖号码</td>");
        output.Write("<td colspan='11'>除11余数</td>");
        output.Write("<td colspan='9'>除9余数</td>");
        output.Write("<td colspan='5'>除5余数</td>");
         output.Write("<td colspan='3'>除3余数</td>");

        output.Write("<tr align='center' valign='middle'>");
 
        for (int i = 0; i < 11; i++)
        {
            output.Write("<td >");
             output.Write(i.ToString()+"</td >");
        }
        for (int i = 0; i < 9; i++)
        {
            output.Write("<td bgcolor='#EDFFD7'>");
            output.Write(i.ToString() + "</td>");
        }
        for (int i = 0; i < 5; i++)
        {
            output.Write("<td>");
            output.Write(i.ToString() + "</td>");
        }
        for (int i = 0; i < 3; i++)
        {
            output.Write("<td bgcolor='#D2E1F0'>");
            output.Write(i.ToString() + "</td>");
        }
    }

    protected void DrawGridFooter(HtmlTextWriter output, Control ctl)
    {
        output.Write("<td>出现次数</td>");
          output.Write("<td>&nbsp;</td>");

        for(int i = 0; i < 28;i++)
        {
           output.Write("<td bgcolor='#F3F3F3'>");
           output.Write(num[i].ToString() + "</td>");
        }
    }
}



