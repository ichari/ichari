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

public partial class View_PL3_KD : TrendChartPageBase
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
        GridView1.DataSource = BLL.PL3_KD_SeleteByNum(30);
        GridView1.DataBind();
    }

    private void GridViewbindColor()
    {
        for (int j = 3; j < GridView1.Columns.Count; j++)
        {
            int tem = 0;

            for (int i = 0; i < GridView1.Rows.Count; i++)
            {
              if (GridView1.Rows[i].Cells[j].Text == "0")
                {
                    GridView1.Rows[i].Cells[j].BackColor = System.Drawing.Color.FromArgb(221, 000, 000);
                    int d = j - 3;
                    GridView1.Rows[i].Cells[j].Text = d.ToString();
                    GridView1.Rows[i].Cells[j].ForeColor = System.Drawing.Color.FromArgb(255,255,255);
                  
                    tem = tem + 1;
                }
            }
            num[j - 3] = tem;

            int m = RadioSelete();

            sum[j - 3] =( num[j - 3] * 50) / m;
        }
    }

    protected void RadioButton1_CheckedChanged1(object sender, EventArgs e)
    {
        DataTable dt = BLL.PL3_KD_SeleteByNum(100);
        GridView1.DataSource = dt;
        GridView1.DataBind();
        GridViewbindColor();
    }

    protected void RadioButton2_CheckedChanged(object sender, EventArgs e)
    {
        DataTable dt = BLL.PL3_KD_SeleteByNum(50);
        GridView1.DataSource = dt;
        GridView1.DataBind();
        GridViewbindColor();
    }

    protected void RadioButton3_CheckedChanged(object sender, EventArgs e)
    {
        DataTable dt = BLL.PL3_KD_SeleteByNum(30);
        GridView1.DataSource = dt;
        GridView1.DataBind();
        GridViewbindColor();
    }

    protected void RadioButton4_CheckedChanged1(object sender, EventArgs e)
    {
        DataTable dt = BLL.PL3_KD_SeleteByNum(20);
        GridView1.DataSource = dt;
        GridView1.DataBind();
        GridViewbindColor();
    }

    protected void RadioButton5_CheckedChanged1(object sender, EventArgs e)
    {
        DataTable dt = BLL.PL3_KD_SeleteByNum(10);
        GridView1.DataSource = dt;
        GridView1.DataBind();
        GridViewbindColor();
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

    private void DrawGridHeader(HtmlTextWriter output, Control ctl)
    {
        output.Write("<td rowspan='2'>期  数</td>");
        output.Write("<td rowspan='2'>开奖号码</td>");
        output.Write("<td colspan='11' align='center' valign='middle'>排列3跨度分布</td>");
         
        output.Write("<tr align='center' valign='middle'>");
        output.Write("<td bgcolor='#E4F1D8'><strong><font Color='#DD0000'>跨度</font></strong></td>");

        for(int i = 0;i< 10 ;i++)
        {
            output.Write("<td bgcolor='#E1EFFF' align='center' valign='middle' <strong><font Color='#DD0000'>");
            output.Write(i.ToString() + "</font></strong></td>");
        }
    }

    private void DrawGridFooter(HtmlTextWriter output, Control ctl)
    {
        output.Write("<td colspan='2' rowspan='5'>预测方法：用鼠标点击方格<BR>即可显示为开奖号色</td>");
        output.Write("<TD bgcolor ='#E4F1D8'>&nbsp</td>");

        for (int i = 0; i < 10; i++)
        {
            output.Write("<td bgcolor ='#E1EFFF'onClick=Style(this,'#DD0000','#E1EFFF') style='color:#E1EFFF' ForeColor='#FFFFFF'>");
            output.Write(i.ToString() + "</td>");
        }

        
        for (int j = 0; j < 4; j++)
        {
        output.Write("<tr align='center' valign ='middle'>");
        output.Write("<td bgcolor ='#E4F1D8'>&nbsp</td>");
            for (int i = 0; i < 10; i++)
            {              
                output.Write("<td bgcolor ='#E1EFFF'onClick=Style(this,'#DD0000','#E1EFFF') style='color:#E1EFFF' ForeColor='#FFFFFF'>");
                output.Write(i.ToString() + "</td>");
            }
        }

        output.Write("<tr align='center' valign ='middle'>");

        output.Write("<td align='center' valign ='middle' height='50'>出现次数</td>");
        output.Write("<td colspan ='2'>&nbsp</td>");
        for (int i = 0; i < 10; i++)
        {
            output.Write("<td align='center' valign='bottom'>");
            output.Write(num[i].ToString() + "<br><img src='../image/01[1].gif' height='" + sum[i].ToString() + "px' title = '" + num[i].ToString() + "' width= '8px'></td>");
        }

        output.Write("<tr align='center' valign ='middle'>");
        output.Write("<td align='center' valign ='middle'>期  数</td>");
        output.Write("<td align='center' valign ='middle'>开奖号码</td>");
        output.Write("<td bgcolor='#E4F1D8'><strong><font Color='#DD0000'>跨度</font></strong></td>");
        for (int i = 0; i < 10; i++)
        {
            output.Write("<td bgcolor ='#E1EFFF'<strong><font Color='#DD0000'>");
            output.Write(i.ToString() + "</font></strong></td>");
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
            k = 25;
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

}
