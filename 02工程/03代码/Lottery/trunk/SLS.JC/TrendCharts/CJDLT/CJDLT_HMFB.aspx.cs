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

public partial class TrendCharts_CJDLT_CJDLT_HMFB : System.Web.UI.Page
{
    int[] num = new int[47];
    int[] sum = new int[47];
   
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
        dt = BLL.CJDLT_HMFB_SeleteByNum(30);
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

        for (int j = 11; j < 37; j++)
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

        for (int j = 41; j < 53; j++)
        {
            int tem = 0;
            for (int i = 0; i < GridView1.Rows.Count; i++)
            {
                if (GridView1.Rows[i].Cells[j].Text == "0")
                {
                    GridView1.Rows[i].Cells[j].BackColor = System.Drawing.Color.FromArgb(000, 000,255);
                    GridView1.Rows[i].Cells[j].ForeColor = System.Drawing.Color.FromArgb(255, 255, 255);
                    if (j < 50)
                    {
                        int kk = j - 40;
                        string a = "0" + kk.ToString();
                        GridView1.Rows[i].Cells[j].Text = a.ToString();

                    }
                    if (j > 49)
                    {
                        int g = j - 40;
                        GridView1.Rows[i].Cells[j].Text = g.ToString();
                    }
                    tem = tem + 1;

                }
            }
            num[j - 6] = tem;
            int e = RadioSelete();
            sum[j - 6] = 50 * tem / e;
        }        
    }

    protected void RadioButton1_CheckedChanged1(object sender, EventArgs e)
    {
        DataTable dt = BLL.CJDLT_HMFB_SeleteByNum(100);
        GridView1.DataSource = dt;
        GridView1.DataBind();
        ColorBind();
    }
    protected void RadioButton2_CheckedChanged(object sender, EventArgs e)
    {
        DataTable dt = BLL.CJDLT_HMFB_SeleteByNum(50);
        GridView1.DataSource = dt;
        GridView1.DataBind();
        ColorBind();

    }
    protected void RadioButton3_CheckedChanged(object sender, EventArgs e)
    {
        DataTable dt = BLL.CJDLT_HMFB_SeleteByNum(30);
        GridView1.DataSource = dt;
        GridView1.DataBind();
        ColorBind();
    }
    protected void RadioButton4_CheckedChanged1(object sender, EventArgs e)
    {
        DataTable dt = BLL.CJDLT_HMFB_SeleteByNum(20);
        GridView1.DataSource = dt;
        GridView1.DataBind();
        ColorBind();
    }
    protected void RadioButton5_CheckedChanged1(object sender, EventArgs e)
    {
        DataTable dt = BLL.CJDLT_HMFB_SeleteByNum(10);
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
        output.Write("<td colspan='35' align='center' valign='middle'><strong><font color='#FF0000'>前区号码</font></strong></td>");
        output.Write("<td rowspan='2' colspan='4'　align='center' valign='middle'><strong><font color='#FF0000'>指标区</font></strong></td>");
        output.Write("<td colspan='12' align='center' valign='middle'><strong><font color='#0000FF'>后区号码</font></strong></td>");

        output.Write("<tr align='center' valign='middle'>");

        output.Write("<td colspan='5'>一区</td>");
        output.Write("<td colspan='5'>二区</td>");
        output.Write("<td colspan='5'>三区</td>");
        output.Write("<td colspan='5'>四区</td>");
        output.Write("<td colspan='5'>五区</td>");
        output.Write("<td colspan='5'>六区</td>");
        output.Write("<td colspan='5'>七区</td>");

        output.Write("<td colspan='3'>一区</td>");
        output.Write("<td colspan='3'>二区</td>");
        output.Write("<td colspan='3'>三区</td>");
        output.Write("<td colspan='3'>四区</td>");

        output.Write("<tr align='center' valign='middle'>");
        for (int i = 1; i < 6; i++)
        {
            output.Write("<td bgcolor='#FFF4D2'><font color='#FF0000'>");
            output.Write(0 + i.ToString() + "</font></td>");
        }
        for (int i = 6; i < 10; i++)
        {
            output.Write("<td bgcolor='#DDF9FE'><font color='#FF0000'>");
            output.Write(0 + i.ToString() + "</font></td>");
        }

        output.Write("<td bgcolor='#DDF9FE'><font color='#FF0000'>10</font></td>");

        for (int i = 11; i < 16; i++)
        {
            output.Write("<td bgcolor='#FEEBE9'><font color='#FF0000'>");
            output.Write(i.ToString() + "</font></td>");
        }

        for (int i = 16; i < 21; i++)
        {
            output.Write("<td bgcolor='#FFF4D2'><font color='#FF0000'>");
            output.Write(i.ToString() + "</font></td>");
        }
        for (int i = 21; i < 26; i++)
        {
            output.Write("<td bgcolor='#DDF9FE'><font color='#FF0000'>");
            output.Write( i.ToString() + "</font></td>");
        }

        for (int i = 26; i < 31; i++)
        {
            output.Write("<td bgcolor='#FEEBE9'><font color='#FF0000'>");
            output.Write(i.ToString() + "</font></td>");
        }
        for (int i = 31; i < 36; i++)
        {
            output.Write("<td bgcolor='#FFF4D2'><font color='#FF0000'>");
            output.Write(i.ToString() + "</font></td>");
        }
       
        output.Write("<td>和值</td>");
        output.Write("<td >大小</td>");
        output.Write("<td >奇偶</td>");
        output.Write("<td >尾和</td>");


        
        for (int i = 1; i < 7; i++)
        {
            output.Write("<td bgcolor='#E1E6FF'><font color='#0000FF'>");
            output.Write(0 + i.ToString() + "</font></td>");
        }

        output.Write("<td bgcolor='#BCD5FE'><font color='#0000FF'>07</font></td>");
        output.Write("<td bgcolor='#BCD5FE'><font color='#0000FF'>08</font></td>");
        output.Write("<td bgcolor='#BCD5FE'><font color='#0000FF'>09</font></td>");
        output.Write("<td bgcolor='#BCD5FE'><font color='#0000FF'>10</font></td>");
        output.Write("<td bgcolor='#BCD5FE'><font color='#0000FF'>11</font></td>");
        output.Write("<td bgcolor='#BCD5FE'><font color='#0000FF'>12</font></td>");
    }

    protected void DrawGridFooter(HtmlTextWriter output, Control ctl)
    {
        output.Write("<td colspan='2' rowspan='5'>预测方法：<br>用鼠标点击<br>方格即可显示<br>为开奖号色</td>");

        for (int i = 1; i < 6; i++)
        {
            output.Write("<td bgcolor='#FFF4D2' onClick=Style(this,'#DD0000','#FFF4D2') style='color:#FFF4D2;'>");
            output.Write(0 + i.ToString() + "</td>");
        }

        for (int i = 6; i < 10; i++)
        {
            output.Write("<td bgcolor='#DDF9FE' onClick=Style(this,'#DD0000','#DDF9FE') style='color:#DDF9FE;'>");
            output.Write(0 + i.ToString() + "</td>");
        }

        output.Write("<td bgcolor='#DDF9FE' onClick=Style(this,'#DD0000','#DDF9FE') style='color:#DDF9FE;'>10</td>");
       
        for (int i = 11; i < 16; i++)
        {
            output.Write("<td bgcolor='#FEEBE9' onClick=Style(this,'#DD0000','#FEEBE9') style='color:#FEEBE9;'>");
            output.Write(i.ToString() + "</td>");
        }

        for (int i = 16; i < 21; i++)
        {
            output.Write("<td bgcolor='#FFF4D2' onClick=Style(this,'#DD0000','#FFF4D2') style='color:#FFF4D2;'>");
            output.Write(i.ToString() + "</td>");
        }

        for (int i = 21; i < 26; i++)
        {
            output.Write("<td bgcolor='#DDF9FE' onClick=Style(this,'#DD0000','#DDF9FE') style='color:#DDF9FE;'>");
            output.Write(i.ToString() + "</td>");
        }

        for (int i = 26; i < 31; i++)
        {
            output.Write("<td bgcolor='#FEEBE9' onClick=Style(this,'#DD0000','#FEEBE9') style='color:#FEEBE9;'>");
            output.Write(i.ToString() + "</td>");
        }

        for (int i = 31; i < 36; i++)
        {
            output.Write("<td bgcolor='#FFF4D2' onClick=Style(this,'#DD0000','#FFF4D2') style='color:#FFF4D2;'>");
            output.Write(i.ToString() + "</td>");
        }

        for (int i = 0; i < 4; i++)
        {
            output.Write("<td>&nbsp;</td>");
        }
        for(int i = 1; i < 7;i++)
        {
            output.Write("<td bgcolor='#E1E6FF' onClick=Style(this,'#0000FF','#E1E6FF') style='color:#E1E6FF;'>");
            output.Write(0+ i.ToString() + "</td>");
        }
        output.Write("<td bgcolor='#BCD5FE' onClick=Style(this,'#0000FF','#BCD5FE') style='color:#BCD5FE;'>07</TD>");
        output.Write("<td bgcolor='#BCD5FE' onClick=Style(this,'#0000FF','#BCD5FE') style='color:#BCD5FE;'>08</TD>");
        output.Write("<td bgcolor='#BCD5FE' onClick=Style(this,'#0000FF','#BCD5FE') style='color:#BCD5FE;'>09</TD>");
        output.Write("<td bgcolor='#BCD5FE' onClick=Style(this,'#0000FF','#BCD5FE') style='color:#BCD5FE;'>10</TD>");
        output.Write("<td bgcolor='#BCD5FE' onClick=Style(this,'#0000FF','#BCD5FE') style='color:#BCD5FE;'>11</TD>");
        output.Write("<td bgcolor='#BCD5FE' onClick=Style(this,'#0000FF','#BCD5FE') style='color:#BCD5FE;'>12</TD>");

        for (int j = 0; j < 4; j++)
        {
            output.Write("<tr align='center' valign='middle'>");
            for (int i = 1; i < 6; i++)
            {
                output.Write("<td bgcolor='#FFF4D2' onClick=Style(this,'#DD0000','#FFF4D2') style='color:#FFF4D2;'>");
                output.Write(0 + i.ToString() + "</td>");
            }

            for (int i = 6; i < 10; i++)
            {
                output.Write("<td bgcolor='#DDF9FE' onClick=Style(this,'#DD0000','#DDF9FE') style='color:#DDF9FE;'>");
                output.Write(0 + i.ToString() + "</td>");
            }

            output.Write("<td bgcolor='#DDF9FE' onClick=Style(this,'#DD0000','#DDF9FE') style='color:#DDF9FE;'>10</td>");

            for (int i = 11; i < 16; i++)
            {
                output.Write("<td bgcolor='#FEEBE9' onClick=Style(this,'#DD0000','#FEEBE9') style='color:#FEEBE9;'>");
                output.Write(i.ToString() + "</td>");
            }

            for (int i = 16; i < 21; i++)
            {
                output.Write("<td bgcolor='#FFF4D2' onClick=Style(this,'#DD0000','#FFF4D2') style='color:#FFF4D2;'>");
                output.Write(i.ToString() + "</td>");
            }

            for (int i = 21; i < 26; i++)
            {
                output.Write("<td bgcolor='#DDF9FE' onClick=Style(this,'#DD0000','#DDF9FE') style='color:#DDF9FE;'>");
                output.Write(i.ToString() + "</td>");
            }

            for (int i = 26; i < 31; i++)
            {
                output.Write("<td bgcolor='#FEEBE9' onClick=Style(this,'#DD0000','#FEEBE9') style='color:#FEEBE9;'>");
                output.Write(i.ToString() + "</td>");
            }

            for (int i = 31; i < 36; i++)
            {
                output.Write("<td bgcolor='#FFF4D2' onClick=Style(this,'#DD0000','#FFF4D2') style='color:#FFF4D2;'>");
                output.Write(i.ToString() + "</td>");
            }

            for (int i = 0; i < 4; i++)
            {
                output.Write("<td>&nbsp;</td>");
            }
            for (int i = 1; i < 7; i++)
            {
                output.Write("<td bgcolor='#E1E6FF' onClick=Style(this,'#0000FF','#E1E6FF') style='color:#E1E6FF;'>");
                output.Write(0 + i.ToString() + "</td>");
            }
            output.Write("<td bgcolor='#BCD5FE' onClick=Style(this,'#0000FF','#BCD5FE') style='color:#BCD5FE;'>07</TD>");
            output.Write("<td bgcolor='#BCD5FE' onClick=Style(this,'#0000FF','#BCD5FE') style='color:#BCD5FE;'>08</TD>");
            output.Write("<td bgcolor='#BCD5FE' onClick=Style(this,'#0000FF','#BCD5FE') style='color:#BCD5FE;'>09</TD>");
            output.Write("<td bgcolor='#BCD5FE' onClick=Style(this,'#0000FF','#BCD5FE') style='color:#BCD5FE;'>10</TD>");
            output.Write("<td bgcolor='#BCD5FE' onClick=Style(this,'#0000FF','#BCD5FE') style='color:#BCD5FE;'>11</TD>");
            output.Write("<td bgcolor='#BCD5FE' onClick=Style(this,'#0000FF','#BCD5FE') style='color:#BCD5FE;'>12</TD>");
        }
       
        output.Write("<tr align='center' valign='middle'>");
        output.Write("<td align='center' valign='middle'>序号</td>");
        output.Write("<td height='50px' align='center' valign='middle'>出现次数</td>");

        for (int i = 0; i < 35; i++)
        {
            output.Write("<td align='center' valign='bottom'>");
            output.Write(num[i].ToString() + "<br><img src='../../Images/01[1].gif' height='" + sum[i].ToString() + "'' title='" + num[i].ToString() + "' width='8px'></td>");

        }

        for (int i = 0; i < 4; i++)
        {
            output.Write("<td>&nbsp;</td>");
        }

        for (int i = 35; i < 47; i++)
        {
            output.Write("<td align='center' valign='bottom'>");
            output.Write(num[i].ToString() + "<br><img src='../../Images/01[1].gif' height='" + sum[i].ToString() + "'' title='" + num[i].ToString() + "' width='8px'></td>");

        }
        output.Write("<tr align='center' valign='middle'>");
        output.Write("<td>序号</td>");
        output.Write("<td>出现次数</td>");

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
        for (int i = 0; i < 4; i++)
        {
            output.Write("<td>&nbsp;</td>");
        }
        for (int i = 1; i < 10; i++)
        {
            output.Write("<td><font color='#0000FF'>");
            output.Write(0 + i.ToString() + "</font></td>");
        }
        for (int i = 10; i < 13; i++)
        {
            output.Write("<td><font color='#0000FF'>");
            output.Write(i.ToString() + "</font></td>");
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
