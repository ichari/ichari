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

public partial class TC22X5_22X5_JO : System.Web.UI.Page
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
        dt = BLL.TC22X5_JO_SeleteByNum(30);
        GridView1.DataSource = dt;
        GridView1.DataBind();
    }

    protected void ColorBind()
    {
        for (int i = 0; i < GridView1.Rows.Count; i++)
        {
            for (int j = 3; j < 27; j++)
            {
                if (GridView1.Rows[i].Cells[j].Text == "偶" || GridView1.Rows[i].Cells[j].Text == "奇")
                {
                    GridView1.Rows[i].Cells[j].BackColor = System.Drawing.Color.FromArgb(221, 000, 000);
                    GridView1.Rows[i].Cells[j].ForeColor = System.Drawing.Color.White;
                }
            }
            for (int k = 27; k < 29; k++)
            {
                if (GridView1.Rows[i].Cells[k].Text == "0")
                {
                    GridView1.Rows[i].Cells[k].Text = "&nbsp;";
                }
            }
        }
    }

    protected void RadioButton1_CheckedChanged1(object sender, EventArgs e)
    {
        DataTable dt = BLL.TC22X5_JO_SeleteByNum(100);
        GridView1.DataSource = dt;
        GridView1.DataBind();
        ColorBind();
    }
    protected void RadioButton2_CheckedChanged(object sender, EventArgs e)
    {
        DataTable dt = BLL.TC22X5_JO_SeleteByNum(50);
        GridView1.DataSource = dt;
        GridView1.DataBind();
        ColorBind();

    }
    protected void RadioButton3_CheckedChanged(object sender, EventArgs e)
    {
        DataTable dt = BLL.TC22X5_JO_SeleteByNum(30);
        GridView1.DataSource = dt;
        GridView1.DataBind();
        ColorBind();
    }
    protected void RadioButton4_CheckedChanged1(object sender, EventArgs e)
    {
        DataTable dt = BLL.TC22X5_JO_SeleteByNum(20);
        GridView1.DataSource = dt;
        GridView1.DataBind();
        ColorBind();
    }
    protected void RadioButton5_CheckedChanged1(object sender, EventArgs e)
    {
        DataTable dt = BLL.TC22X5_JO_SeleteByNum(10);
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
        output.Write("<td rowspan ='2' align ='center' vlign='middle'>期  数</td>");
        output.Write("<td rowspan ='2'>开奖号码</td>");
        output.Write("<td colspan = '5'>第一位</td>");
        output.Write("<td colspan = '5'>第二位</td>");
        output.Write("<td colspan = '5'>第三位</td>");
        output.Write("<td colspan = '5'>第四位</td>");
        output.Write("<td colspan = '5'>第五位</td>");
        output.Write("<td rowspan ='2' width='30'>奇数个数</td>"); output.Write("<td rowspan ='2' width='30'>偶数个数</td>");
        output.Write("<td rowspan ='2' width='30'>奇偶比</td>");
        output.Write("<td rowspan ='2' width='30'>奇偶差值</td>");

        output.Write("<tr align ='center' vlign='center'>");
        for (int i = 0; i < 5; i++)
        {
            output.Write("<td>&nbsp;</td>");
            output.Write("<td>奇</td>");
            output.Write("<td>&nbsp;</td>");
            output.Write("<td>偶</td>");
            output.Write("<td>&nbsp;</td>");
        }
    }

    protected void DrawGridFooter(HtmlTextWriter output, Control ctl)
    {
        output.Write("<td colspan='2' rowspan='5'>预测方法：<br>用鼠标点击<br>方格即可显示<br>为开奖号色</td>");

        output.Write("<td bgcolor='#FCDFC5'>&nbsp;</td>");
        output.Write("<td bgcolor='#FCDFC5'onClick=Style(this,'#DD0000','#FCDFC5') style='color:#FCDFC5'>奇</td>");
        output.Write("<td bgcolor='#FCDFC5'>&nbsp;</td>");
        output.Write("<td bgcolor='#FCDFC5'onClick=Style(this,'#DD0000','#FCDFC5') style='color:#FCDFC5'>偶</td>");
        output.Write("<td bgcolor='#FCDFC5'>&nbsp;</td>");

        for (int i = 0; i < 2; i++)
        {
            output.Write("<td bgcolor='#DDFFE6'>&nbsp;</td>");
            output.Write("<td bgcolor='#DDFFE6'onClick=Style(this,'#DD0000','#DDFFE6') style='color:#DDFFE6'>奇</td>");
            output.Write("<td bgcolor='#DDFFE6'>&nbsp;</td>");
            output.Write("<td bgcolor='#DDFFE6'onClick=Style(this,'#DD0000','#DDFFE6') style='color:#DDFFE6'>偶</td>");
            output.Write("<td bgcolor='#DDFFE6'>&nbsp;</td>");

            output.Write("<td bgcolor='#C1E7FF'>&nbsp;</td>");
            output.Write("<td bgcolor='#C1E7FF'onClick=Style(this,'#DD0000','#C1E7FF') style='color:#C1E7FF'>奇</td>");
            output.Write("<td bgcolor='#C1E7FF'>&nbsp;</td>");
            output.Write("<td bgcolor='#C1E7FF'onClick=Style(this,'#DD0000','#C1E7FF') style='color:#C1E7FF'>偶</td>");
            output.Write("<td bgcolor='#C1E7FF'>&nbsp;</td>");
        }

        for (int i = 0; i < 4; i++)
        {
            output.Write("<td bgcolor='#EDFFD7'>&nbsp</td>");
        }

        for (int j = 0; j < 4; j++)
        {
            output.Write("<tr align='center' valign='middle'>");

            output.Write("<td bgcolor='#FCDFC5'>&nbsp;</td>");
            output.Write("<td bgcolor='#FCDFC5'onClick=Style(this,'#DD0000','#FCDFC5') style='color:#FCDFC5'>奇</td>");
            output.Write("<td bgcolor='#FCDFC5'>&nbsp;</td>");
            output.Write("<td bgcolor='#FCDFC5'onClick=Style(this,'#DD0000','#FCDFC5') style='color:#FCDFC5'>偶</td>");
            output.Write("<td bgcolor='#FCDFC5'>&nbsp;</td>");

            for (int i = 0; i < 2; i++)
            {
                output.Write("<td bgcolor='#DDFFE6'>&nbsp;</td>");
                output.Write("<td bgcolor='#DDFFE6'onClick=Style(this,'#DD0000','#DDFFE6') style='color:#DDFFE6'>奇</td>");
                output.Write("<td bgcolor='#DDFFE6'>&nbsp;</td>");
                output.Write("<td bgcolor='#DDFFE6'onClick=Style(this,'#DD0000','#DDFFE6') style='color:#DDFFE6'>偶</td>");
                output.Write("<td bgcolor='#DDFFE6'>&nbsp;</td>");

                output.Write("<td bgcolor='#C1E7FF'>&nbsp;</td>");
                output.Write("<td bgcolor='#C1E7FF'onClick=Style(this,'#DD0000','#C1E7FF') style='color:#C1E7FF'>奇</td>");
                output.Write("<td bgcolor='#C1E7FF'>&nbsp;</td>");
                output.Write("<td bgcolor='#C1E7FF'onClick=Style(this,'#DD0000','#C1E7FF') style='color:#C1E7FF'>偶</td>");
                output.Write("<td bgcolor='#C1E7FF'>&nbsp;</td>");
            }

            for (int i = 0; i < 4; i++)
            {
                output.Write("<td bgcolor='#EDFFD7'>&nbsp</td>");
            }
        }


        output.Write("<tr align='center' vlign = 'middle'>");
        output.Write("<td>&nbsp</td>");
        output.Write("<td>&nbsp</td>");
        output.Write("<td bgcolor='#FCDFC5'>&nbsp;</td>");
        output.Write("<td bgcolor='#FCDFC5'><font color='DD0000'>奇</font></td>");
        output.Write("<td bgcolor='#FCDFC5'>&nbsp;</td>");
        output.Write("<td bgcolor='#FCDFC5'><font color='DD0000'>偶</font></td>");
        output.Write("<td bgcolor='#FCDFC5'>&nbsp;</td>");

        for (int k = 0; k < 2; k++)
        {
            output.Write("<td bgcolor='#DDFFE6'>&nbsp;</td>");
            output.Write("<td bgcolor='#DDFFE6'><font color='DD0000'>奇</font></td>");
            output.Write("<td bgcolor='#DDFFE6'>&nbsp;</td>");
            output.Write("<td bgcolor='#DDFFE6'><font color='DD0000'>偶</font></td>");
            output.Write("<td bgcolor='#DDFFE6'>&nbsp;</td>");

            output.Write("<td bgcolor='#C1E7FF'>&nbsp;</td>");
            output.Write("<td bgcolor='#C1E7FF'><font color='DD0000'>奇</font></td>");
            output.Write("<td bgcolor='#C1E7FF'>&nbsp;</td>");
            output.Write("<td bgcolor='#C1E7FF'><font color='DD0000'>偶</font></td>");
            output.Write("<td bgcolor='#C1E7FF'>&nbsp;</td>");
        }
        for (int i = 0; i < 4; i++)
        {
            output.Write("<td bgcolor='#EDFFD7'>&nbsp</td>");
        }

        output.Write("<tr align='center' vlign='center'>");
        output.Write("<td>期  数</td>");
        output.Write("<td>开奖号码</td>");
        output.Write("<td colspan ='5'bgcolor='#FFC9C8'>第一位</td>");
        output.Write("<td colspan ='5'bgcolor='#DDFFE6'>第二位</td>");
        output.Write("<td colspan ='5'bgcolor='#C1E7FF'>第三位</td>");
        output.Write("<td colspan ='5'bgcolor='#C1E7FF'>第四位</td>");
        output.Write("<td colspan ='5'bgcolor='#C1E7FF'>第五位</td>");
        for (int i = 0; i < 4; i++)
        {
            output.Write("<td bgcolor='#EDFFD7'>&nbsp</td>");
        }

    }
}
