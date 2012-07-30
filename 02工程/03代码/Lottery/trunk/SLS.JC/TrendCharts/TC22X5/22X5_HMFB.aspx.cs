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

public partial class TC22X5_22X5 : System.Web.UI.Page
{

    int[] num = new int[22];
    int[] sum = new int[22];
    int m = 0;
    int mm = 0;
    int n = 0;
    int nn = 0;
    int nnn = 0;
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
        dt = BLL.TC22X5_HMFB_SeleteByNum(30);
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

        for (int i = 0; i < GridView1.Rows.Count; i++)
        {
            m = m + Shove._Convert.StrToInt(GridView1.Rows[i].Cells[24].Text, 0);
            mm = mm + Shove._Convert.StrToInt(GridView1.Rows[i].Cells[25].Text, 0);
            n = n + Shove._Convert.StrToInt(GridView1.Rows[i].Cells[29].Text, 0);
            nn = nn + Shove._Convert.StrToInt(GridView1.Rows[i].Cells[30].Text, 0);
            nnn = nnn + Shove._Convert.StrToInt(GridView1.Rows[i].Cells[31].Text, 0);

            for (int j = 29; j < 32; j++)
            {
                if (GridView1.Rows[i].Cells[j].Text == "0")
                {
                    GridView1.Rows[i].Cells[j].Text = "&nbsp;";
                }
            }
        }

        for (int j = 2; j < 11; j++)
        {
            int tem = 0;
            for (int i = 0; i < GridView1.Rows.Count; i++)
            {
                if (GridView1.Rows[i].Cells[j].Text == "0")
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

        for (int j = 11; j < 24; j++)
        {
            int tem = 0;
            for (int i = 0; i < GridView1.Rows.Count; i++)
            {
                if (GridView1.Rows[i].Cells[j].Text == "0")
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
    }

    protected void RadioButton1_CheckedChanged1(object sender, EventArgs e)
    {
        DataTable dt = BLL.TC22X5_HMFB_SeleteByNum(100);
        GridView1.DataSource = dt;
        GridView1.DataBind();
        ColorBind();
    }
    protected void RadioButton2_CheckedChanged(object sender, EventArgs e)
    {
        DataTable dt = BLL.TC22X5_HMFB_SeleteByNum(50);
        GridView1.DataSource = dt;
        GridView1.DataBind();
        ColorBind();

    }
    protected void RadioButton3_CheckedChanged(object sender, EventArgs e)
    {
        DataTable dt = BLL.TC22X5_HMFB_SeleteByNum(30);
        GridView1.DataSource = dt;
        GridView1.DataBind();
        ColorBind();
    }
    protected void RadioButton4_CheckedChanged1(object sender, EventArgs e)
    {
        DataTable dt = BLL.TC22X5_HMFB_SeleteByNum(20);
        GridView1.DataSource = dt;
        GridView1.DataBind();
        ColorBind();
    }
    protected void RadioButton5_CheckedChanged1(object sender, EventArgs e)
    {
        DataTable dt = BLL.TC22X5_HMFB_SeleteByNum(10);
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
        output.Write("<td colspan='30' align='center' valign='middle'><strong><font color='#FF0000'>开奖号码区</font></strong></td>");
        output.Write("<tr align='center' valign='middle'>");
        output.Write("<td colspan='7'>一区</td>");
        output.Write("<td colspan='7'>二区</td>");
        output.Write("<td colspan='8'>三区</td>");
        output.Write("<td colspan='2'>奇偶</td>");
        output.Write("<td colspan='2'>大小</td>");
        output.Write("<td  rowspan='2' bgcolor='#B3EEA8'>和值</td>");
        output.Write("<td colspan='3'>区域分布</td>");
        output.Write("<tr align='center' valign='middle'>");
        for (int i = 1; i < 8; i++)
        {
            output.Write("<td bgcolor='#C0F3C9'><font color='#FF0000'>");
            output.Write(0 + i.ToString() + "</font></td>");
        }
        for (int i = 8; i < 10; i++)
        {
            output.Write("<td bgcolor='#FFCCFF'><font color='#FF0000'>");
            output.Write(0 + i.ToString() + "</font></td>");
        }
        for (int i = 10; i < 15; i++)
        {
            output.Write("<td bgcolor='#FFCCFF'><font color='#FF0000'>");
            output.Write(i.ToString() + "</font></td>");
        }
        for (int i = 15; i < 23; i++)
        {
            output.Write("<td bgcolor='#C0F3C9'><font color='#FF0000'>");
            output.Write(i.ToString() + "</font></td>");
        }
        output.Write("<td bgcolor='#FFE67D'>奇</td>");
        output.Write("<td bgcolor='#FFE67D'>偶</td>");
        output.Write("<td bgcolor='#FFF7D2'>大</td>");
        output.Write("<td bgcolor='#FFF7D2'>小</td>");
        output.Write("<td>一区</td>");
        output.Write("<td>二区</td>");
        output.Write("<td>三区</td>");
    }

    protected void DrawGridFooter(HtmlTextWriter output, Control ctl)
    {
        output.Write("<td colspan='2' rowspan='5'>预测方法：<br>用鼠标点击<br>方格即可显示<br>为开奖号色</td>");
    
        for (int i = 1; i < 8; i++)
        {
            output.Write("<td bgcolor='#EBFCEF' onClick=Style(this,'#DD0000','#EBFCEF') style='color:#EBFCEF;'>");
            output.Write(0 + i.ToString() + "</td>");

        }

        for (int i = 8; i < 10; i++)
        {
            output.Write("<td bgcolor='#FEEFF5' onClick=Style(this,'#DD0000','#FEEFF5') style='color:#FEEFF5;'>");
            output.Write(0 + i.ToString() + "</td>");
        }

        for (int i = 10; i < 15; i++)
        {
            output.Write("<td bgcolor='#FEEFF5' onClick=Style(this,'#DD0000','#FEEFF5') style='color:#FEEFF5;'>");
            output.Write(i.ToString() + "</td>");
        }

        for (int i = 15; i < 23; i++)
        {
            output.Write("<td bgcolor='#EBFCEF' onClick=Style(this,'#DD0000','#EBFCEF') style='color:#EBFCEF;'>");
            output.Write(i.ToString() + "</td>");

        }
        for (int i = 0; i < 8; i++)
        {
            output.Write("<td>&nbsp</td>");
        }

        for (int j = 0; j < 4; j++)
        {
            output.Write("<tr align='center' valign='middle'>");
            for (int i = 1; i < 8; i++)
            {
                output.Write("<td bgcolor='#EBFCEF' onClick=Style(this,'#DD0000','#EBFCEF') style='color:#EBFCEF;'>");
                output.Write(0 + i.ToString() + "</td>");

            }

            for (int i = 8; i < 10; i++)
            {
                output.Write("<td bgcolor='#FEEFF5' onClick=Style(this,'#DD0000','#FEEFF5') style='color:#FEEFF5;'>");
                output.Write(0 + i.ToString() + "</td>");
            }

            for (int i = 10; i < 15; i++)
            {
                output.Write("<td bgcolor='#FEEFF5' onClick=Style(this,'#DD0000','#FEEFF5') style='color:#FEEFF5;'>");
                output.Write(i.ToString() + "</td>");
            }

            for (int i = 15; i < 23; i++)
            {
                output.Write("<td bgcolor='#EBFCEF' onClick=Style(this,'#DD0000','#EBFCEF') style='color:#EBFCEF;'>");
                output.Write(i.ToString() + "</td>");

            }
            for (int i = 0; i < 8; i++)
            {
                output.Write("<td>&nbsp</td>");
            }
        }

        output.Write("<tr align='center' valign='middle'>");
        output.Write("<td height='50px' colspan='2' align='center' valign='middle'>出现次数</td>");
        
        for (int i = 0; i < 22; i++)
        {
            output.Write("<td align='center' valign='bottom'>");
            output.Write(num[i].ToString() + "<br><img src='../Image/01[1].gif' height='" + sum[i].ToString() + "'' title='" + num[i].ToString() + "' width='8px'></td>");

        }

        output.Write("<td valign='bottom'>");
        output.Write(m.ToString() + "</td>");
        output.Write("<td valign='bottom'>");
        output.Write(mm.ToString()+"</td>");        
        output.Write("<td>&nbsp</td>");
        output.Write("<td>&nbsp</td>");
        output.Write("<td>&nbsp</td>");

        output.Write("<td valign='bottom'>");
        output.Write(n.ToString() + "</td>");
        output.Write("<td valign='bottom'>");
        output.Write(nn.ToString() + "</td>");
        output.Write("<td valign='bottom'>");
        output.Write(nnn.ToString() + "</td>");

        output.Write("<tr align='center' valign='middle'>");
        output.Write("<td>序号</td>");
        output.Write("<td>期 数</td>");

        for (int i = 1; i < 8; i++)
        {
            output.Write("<td bgcolor='#EBFCEF'><font color='#FF0000'>");
            output.Write(0 + i.ToString() + "</font></td>");
        }
        output.Write("<td ><font color='#FF0000'>08</font></td>");
        output.Write("<td ><font color='#FF0000'>09</font></td>");
        for (int i = 10; i < 23; i++)
        {
            output.Write("<td bgcolor='#ffffff'><font color='#FF0000'>");
            output.Write(0 + i.ToString() + "</font></td>");
        }
        output.Write("<td>奇</td>");
        output.Write("<td>偶</td>");
        output.Write("<td>大</td>");
        output.Write("<td>小</td>");
        output.Write("<td>和值</td>");
        output.Write("<td>一区</td>");
        output.Write("<td>二区</td>");
        output.Write("<td>三区</td>");
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
