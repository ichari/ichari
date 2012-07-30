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

public partial class View_PL3_DZX : TrendChartPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            databind();
            GridViewbindColor();
            GridView1.Style["border-collapse"] = "";
        }
    }

    protected void databind()
    {
        GridView1.DataSource = BLL.PL3_DZX_SeleteByNum(30);
        GridView1.DataBind();
    }

    #region 按钮事件
    protected void RadioButton1_CheckedChanged1(object sender, EventArgs e)
    {
        DataTable dt = BLL.PL3_DZX_SeleteByNum(100);
        GridView1.DataSource = dt;
        GridView1.DataBind();
        GridViewbindColor();

    }

    protected void RadioButton2_CheckedChanged(object sender, EventArgs e)
    {
        DataTable dt = BLL.PL3_DZX_SeleteByNum(50);
        GridView1.DataSource = dt;
        GridView1.DataBind();
        GridViewbindColor();

    }

    protected void RadioButton3_CheckedChanged(object sender, EventArgs e)
    {
        DataTable dt = BLL.PL3_DZX_SeleteByNum(30);
        GridView1.DataSource = dt;
        GridView1.DataBind();
        GridViewbindColor();
        //BindNumber();
    }

    protected void RadioButton4_CheckedChanged1(object sender, EventArgs e)
    {
        DataTable dt = BLL.PL3_DZX_SeleteByNum(20);
        GridView1.DataSource = dt;
        GridView1.DataBind();
        GridViewbindColor();
        //BindNumber();
    }

    protected void RadioButton5_CheckedChanged1(object sender, EventArgs e)
    {
        DataTable dt = BLL.PL3_DZX_SeleteByNum(10);
        GridView1.DataSource = dt;
        GridView1.DataBind();
        GridViewbindColor();
        //BindNumber();
    }

    #endregion

    protected void GridViewbindColor()
    {
        for (int j = 2; j < 17; j++)
            for (int i = 0; i < GridView1.Rows.Count; i++)
            {
                if (GridView1.Rows[i].Cells[j].Text == null)
                {
                    GridView1.Rows[i].Cells[j].Text = "&nbsp";

                }
                if(GridView1.Rows[i].Cells[j].Text == "大" || GridView1.Rows[i].Cells[j].Text=="小"||GridView1.Rows[i].Cells[j].Text == "中")
                {
                   GridView1.Rows[i].Cells[j].BackColor = System.Drawing.Color.FromArgb(221, 000, 000);
                   GridView1.Rows[i].Cells[j].ForeColor = System.Drawing.Color.FromArgb(255, 255, 255);
                }
            }
        for (int j = 17; j < 21; j++)
        {
            int num = j - 17;
            for (int i = 0; i < GridView1.Rows.Count; i++)
            {
                if (GridView1.Rows[i].Cells[j].Text == "0")
                {
                    GridView1.Rows[i].Cells[j].BackColor = System.Drawing.Color.FromArgb(000, 000, 204);
                    GridView1.Rows[i].Cells[j].ForeColor = System.Drawing.Color.FromArgb(255, 255, 255);
                    GridView1.Rows[i].Cells[j].Text = num.ToString();
                }
                else
                {
                    GridView1.Rows[i].Cells[j].ForeColor = System.Drawing.Color.FromArgb(128, 128, 128);
                }
            }
        }

        for (int j = 21; j < 25; j++)
        {
            int num = j - 21;
            for (int i = 0; i < GridView1.Rows.Count; i++)
            {
                if (GridView1.Rows[i].Cells[j].Text == "0")
                {
                    GridView1.Rows[i].Cells[j].BackColor = System.Drawing.Color.FromArgb(221, 000, 000);
                    GridView1.Rows[i].Cells[j].ForeColor = System.Drawing.Color.FromArgb(255, 255, 255);
                    GridView1.Rows[i].Cells[j].Text = num.ToString();
                }
                else
                {
                    GridView1.Rows[i].Cells[j].ForeColor = System.Drawing.Color.FromArgb(179, 179, 179);
                }
            }
        }

        for (int j = 25; j < 29; j++)
        {
            int num = j - 25;
            for (int i = 0; i < GridView1.Rows.Count; i++)
            {
                if (GridView1.Rows[i].Cells[j].Text == "0")
                {
                    GridView1.Rows[i].Cells[j].BackColor = System.Drawing.Color.FromArgb(000, 000, 204);
                    GridView1.Rows[i].Cells[j].ForeColor = System.Drawing.Color.FromArgb(255, 255, 255);
                    GridView1.Rows[i].Cells[j].Text = num.ToString();
                }
                else
                {
                    GridView1.Rows[i].Cells[j].ForeColor = System.Drawing.Color.FromArgb(128, 128, 128);
                }
            }
        }

        for (int i = 0; i < GridView1.Rows.Count; i++)
            for (int j = 29; j < 39; j++)
            {
                if (GridView1.Rows[i].Cells[j].Text == "0")
                {
                    GridView1.Rows[i].Cells[j].BackColor = System.Drawing.Color.FromArgb(051, 128, 189);
                    GridView1.Rows[i].Cells[j].ForeColor = System.Drawing.Color.FromArgb(255, 255, 255);
                }
                else
                {
                    GridView1.Rows[i].Cells[j].ForeColor = System.Drawing.Color.FromArgb(179, 179, 179);
                }
            }

        for (int i = 0; i < GridView1.Rows.Count; i++)
        {
            if (GridView1.Rows[i].Cells[29].Text == "0")
            {
                GridView1.Rows[i].Cells[29].Text = "A";
            }
            if (GridView1.Rows[i].Cells[30].Text == "0")
            {
                GridView1.Rows[i].Cells[30].Text = "B";
            }
            if (GridView1.Rows[i].Cells[31].Text == "0")
            {
                GridView1.Rows[i].Cells[31].Text = "C";
            }
            if (GridView1.Rows[i].Cells[32].Text == "0")
            {
                GridView1.Rows[i].Cells[32].Text = "D";
            }
            if (GridView1.Rows[i].Cells[33].Text == "0")
            {
                GridView1.Rows[i].Cells[33].Text = "E";
            }
            if (GridView1.Rows[i].Cells[34].Text == "0")
            {
                GridView1.Rows[i].Cells[34].Text = "F";
            }
            if (GridView1.Rows[i].Cells[35].Text == "0")
            {
                GridView1.Rows[i].Cells[35].Text = "G";
            }
            if (GridView1.Rows[i].Cells[36].Text == "0")
            {
                GridView1.Rows[i].Cells[36].Text = "H";
            }
            if (GridView1.Rows[i].Cells[37].Text == "0")
            {
                GridView1.Rows[i].Cells[37].Text = "I";
            }
            if (GridView1.Rows[i].Cells[38].Text == "0")
            {
                GridView1.Rows[i].Cells[38].Text = "J";
            }
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

    protected void DrawGridFooter(HtmlTextWriter output, Control ctl)
    {
        //output.Write("<tr align='center' valign='middle'>");

        output.Write("<td colspan='2' rowspan='5'>预测方法：<br>用鼠标点击<br>方格即可显示<br>为开奖号色</td>");

        output.Write("<td bgcolor='#FCDFC5'>&nbsp;</td>");
        output.Write("<td bgcolor='#FCDFC5'onClick=Style(this,'#DD0000','#FCDFC5') style='color:#FCDFC5'>大</td>");
        output.Write("<td bgcolor='#FCDFC5'onClick=Style(this,'#DD0000','#FCDFC5') style='color:#FCDFC5'>中</td>");
        output.Write("<td bgcolor='#FCDFC5'onClick=Style(this,'#DD0000','#FCDFC5') style='color:#FCDFC5'>小</td>");
        output.Write("<td bgcolor='#FCDFC5'>&nbsp;</td>");

        output.Write("<td bgcolor='#DDFFE6'>&nbsp;</td>");
        output.Write("<td bgcolor='#DDFFE6'onClick=Style(this,'#DD0000','#DDFFE6') style='color:#DDFFE6'>大</td>");
        output.Write("<td bgcolor='#DDFFE6'onClick=Style(this,'#DD0000','#DDFFE6') style='color:#DDFFE6'>中</td>");
        output.Write("<td bgcolor='#DDFFE6'onClick=Style(this,'#DD0000','#DDFFE6') style='color:#DDFFE6'>小</td>");
        output.Write("<td bgcolor='#DDFFE6'>&nbsp;</td>");

        output.Write("<td bgcolor='#C1E7FF'>&nbsp;</td>");
        output.Write("<td bgcolor='#C1E7FF'onClick=Style(this,'#DD0000','#C1E7FF') style='color:#C1E7FF'>大</td>");
        output.Write("<td bgcolor='#C1E7FF'onClick=Style(this,'#DD0000','#C1E7FF') style='color:#C1E7FF'>中</td>");
        output.Write("<td bgcolor='#C1E7FF'onClick=Style(this,'#DD0000','#C1E7FF') style='color:#C1E7FF'>小</td>");
        output.Write("<td bgcolor='#C1E7FF'>&nbsp;</td>");



        output.Write("<td bgcolor='#FFCC33' onClick=Style(this,'#0000CC','#FFCC33') style='color:#FFCC33'>0</td>");
        output.Write("<td bgcolor='#FFCC33' onClick=Style(this,'#0000CC','#FFCC33') style='color:#FFCC33'>1</td>");
        output.Write("<td bgcolor='#FFCC33' onClick=Style(this,'#0000CC','#FFCC33') style='color:#FFCC33'>2</td>");
        output.Write("<td bgcolor='#FFCC33' onClick=Style(this,'#0000CC','#FFCC33') style='color:#FFCC33'>3</td>");


        for (int i = 0; i < 4; i++)
        {
            output.Write("<td bgcolor='#FFFFFF'onclick=Style(this,'#DD0000','#FFFFFF') style='color:#FFFFFF'>");
            output.Write(i.ToString() + "</td>");
        }

        for (int i = 0; i < 4; i++)
        {
            output.Write("<td bgcolor='#A6D7C1'onclick=Style(this,'#0000CC','#A6D7C1') style='color:#A6D7C1'>");
            output.Write(i.ToString() + "</td>");
        }

        output.Write("<td bgcolor='#FFFFFF'onclick=Style(this,'#3380BD','#FFFFFF') style='color:#FFFFFF'>A</TD>");
        output.Write("<td bgcolor='#FFFFFF'onclick=Style(this,'#3380BD','#FFFFFF') style='color:#FFFFFF'>B</TD>");
        output.Write("<td bgcolor='#FFFFFF'onclick=Style(this,'#3380BD','#FFFFFF') style='color:#FFFFFF'>C</TD>");
        output.Write("<td bgcolor='#FFFFFF'onclick=Style(this,'#3380BD','#FFFFFF') style='color:#FFFFFF'>D</TD>");
        output.Write("<td bgcolor='#FFFFFF'onclick=Style(this,'#3380BD','#FFFFFF') style='color:#FFFFFF'>E</TD>");
        output.Write("<td bgcolor='#FFFFFF'onclick=Style(this,'#3380BD','#FFFFFF') style='color:#FFFFFF'>F</TD>");
        output.Write("<td bgcolor='#FFFFFF'onclick=Style(this,'#3380BD','#FFFFFF') style='color:#FFFFFF'>G</TD>");
        output.Write("<td bgcolor='#FFFFFF'onclick=Style(this,'#3380BD','#FFFFFF') style='color:#FFFFFF'>H</TD>");
        output.Write("<td bgcolor='#FFFFFF'onclick=Style(this,'#3380BD','#FFFFFF') style='color:#FFFFFF'>I</TD>");
        output.Write("<td bgcolor='#FFFFFF'onclick=Style(this,'#3380BD','#FFFFFF') style='color:#FFFFFF'>J</TD>");

        for (int n = 0; n < 4; n++)
        {
            output.Write("<tr align='center' valign='middle'>");

            output.Write("<td bgcolor='#FCDFC5'>&nbsp;</td>");
            output.Write("<td bgcolor='#FCDFC5'onClick=Style(this,'#DD0000','#FCDFC5') style='color:#FCDFC5'>大</td>");
            output.Write("<td bgcolor='#FCDFC5'onClick=Style(this,'#DD0000','#FCDFC5') style='color:#FCDFC5'>中</td>");
            output.Write("<td bgcolor='#FCDFC5'onClick=Style(this,'#DD0000','#FCDFC5') style='color:#FCDFC5'>小</td>");
            output.Write("<td bgcolor='#FCDFC5'>&nbsp;</td>");

            output.Write("<td bgcolor='#DDFFE6'>&nbsp;</td>");
            output.Write("<td bgcolor='#DDFFE6'onClick=Style(this,'#DD0000','#DDFFE6') style='color:#DDFFE6'>大</td>");
            output.Write("<td bgcolor='#DDFFE6'onClick=Style(this,'#DD0000','#DDFFE6') style='color:#DDFFE6'>中</td>");
            output.Write("<td bgcolor='#DDFFE6'onClick=Style(this,'#DD0000','#DDFFE6') style='color:#DDFFE6'>小</td>");
            output.Write("<td bgcolor='#DDFFE6'>&nbsp;</td>");

            output.Write("<td bgcolor='#C1E7FF'>&nbsp;</td>");
            output.Write("<td bgcolor='#C1E7FF'onClick=Style(this,'#DD0000','#C1E7FF') style='color:#C1E7FF'>大</td>");
            output.Write("<td bgcolor='#C1E7FF'onClick=Style(this,'#DD0000','#C1E7FF') style='color:#C1E7FF'>中</td>");
            output.Write("<td bgcolor='#C1E7FF'onClick=Style(this,'#DD0000','#C1E7FF') style='color:#C1E7FF'>小</td>");
            output.Write("<td bgcolor='#C1E7FF'>&nbsp;</td>");



            output.Write("<td bgcolor='#FFCC33' onClick=Style(this,'#0000CC','#FFCC33') style='color:#FFCC33'>0</td>");
            output.Write("<td bgcolor='#FFCC33' onClick=Style(this,'#0000CC','#FFCC33') style='color:#FFCC33'>1</td>");
            output.Write("<td bgcolor='#FFCC33' onClick=Style(this,'#0000CC','#FFCC33') style='color:#FFCC33'>2</td>");
            output.Write("<td bgcolor='#FFCC33' onClick=Style(this,'#0000CC','#FFCC33') style='color:#FFCC33'>3</td>");


            for (int i = 0; i < 4; i++)
            {
                output.Write("<td bgcolor='#FFFFFF'onclick=Style(this,'#DD0000','#FFFFFF') style='color:#FFFFFF'>");
                output.Write(i.ToString() + "</td>");
            }

            for (int i = 0; i < 4; i++)
            {
                output.Write("<td bgcolor='#A6D7C1'onclick=Style(this,'#0000CC','#A6D7C1') style='color:#A6D7C1'>");
                output.Write(i.ToString() + "</td>");
            }

            output.Write("<td bgcolor='#FFFFFF'onclick=Style(this,'#3380BD','#FFFFFF') style='color:#FFFFFF'>A</TD>");
            output.Write("<td bgcolor='#FFFFFF'onclick=Style(this,'#3380BD','#FFFFFF') style='color:#FFFFFF'>B</TD>");
            output.Write("<td bgcolor='#FFFFFF'onclick=Style(this,'#3380BD','#FFFFFF') style='color:#FFFFFF'>C</TD>");
            output.Write("<td bgcolor='#FFFFFF'onclick=Style(this,'#3380BD','#FFFFFF') style='color:#FFFFFF'>D</TD>");
            output.Write("<td bgcolor='#FFFFFF'onclick=Style(this,'#3380BD','#FFFFFF') style='color:#FFFFFF'>E</TD>");
            output.Write("<td bgcolor='#FFFFFF'onclick=Style(this,'#3380BD','#FFFFFF') style='color:#FFFFFF'>F</TD>");
            output.Write("<td bgcolor='#FFFFFF'onclick=Style(this,'#3380BD','#FFFFFF') style='color:#FFFFFF'>G</TD>");
            output.Write("<td bgcolor='#FFFFFF'onclick=Style(this,'#3380BD','#FFFFFF') style='color:#FFFFFF'>H</TD>");
            output.Write("<td bgcolor='#FFFFFF'onclick=Style(this,'#3380BD','#FFFFFF') style='color:#FFFFFF'>I</TD>");
            output.Write("<td bgcolor='#FFFFFF'onclick=Style(this,'#3380BD','#FFFFFF') style='color:#FFFFFF'>J</TD>");
        }

        // --最后两行的添加

        output.Write("<tr align='center' vlign = 'middle'>");
        output.Write("<td>&nbsp</td>");
        output.Write("<td>&nbsp</td>");
        output.Write("<td bgcolor='#FCDFC5'>&nbsp;</td>");
        output.Write("<td bgcolor='#FCDFC5'><strong><font color='DD0000'>大</font></strong></td>");
        output.Write("<td bgcolor='#FCDFC5'><strong><font color='DD0000'>中</font></strong></td>");
        output.Write("<td bgcolor='#FCDFC5'><strong><font color='DD0000'>小</font></strong></td>");
        output.Write("<td bgcolor='#FCDFC5'>&nbsp;</td>");

        output.Write("<td bgcolor='#DDFFE6'>&nbsp;</td>");
        output.Write("<td bgcolor='#DDFFE6'><strong><font color='DD0000'>大</font></strong></td>");
        output.Write("<td bgcolor='#DDFFE6'><strong><font color='DD0000'>中</font></strong></td>");
        output.Write("<td bgcolor='#DDFFE6'><strong><font color='DD0000'>小</font></strong></td>");
        output.Write("<td bgcolor='#DDFFE6'>&nbsp;</td>");

        output.Write("<td bgcolor='#C1E7FF'>&nbsp;</td>");
        output.Write("<td bgcolor='#C1E7FF'><strong><font color='DD0000'>大</font></strong></td>");
        output.Write("<td bgcolor='#C1E7FF'><strong><font color='DD0000'>中</font></strong></td>");
        output.Write("<td bgcolor='#C1E7FF'><strong><font color='DD0000'>小</font></strong></td>");
        output.Write("<td bgcolor='#C1E7FF'>&nbsp;</td>");

        output.Write("<td bgcolor='#FFCC33'><strong><font color='DD0000'>0</font></strong></td>");
        output.Write("<td bgcolor='#FFCC33'><strong><font color='DD0000'>1</font></strong></td>");
        output.Write("<td bgcolor='#FFCC33'><strong><font color='DD0000'>2</font></strong></td>");
        output.Write("<td bgcolor='#FFCC33'><strong><font color='DD0000'>3</font></strong></td>");

        output.Write("<td bgcolor='#FFFFFF'><strong><font color='DD0000'>0</font></strong></td>");
        output.Write("<td bgcolor='#FFFFFF'><strong><font color='DD0000'>1</font></strong></td>");
        output.Write("<td bgcolor='#FFFFFF'><strong><font color='DD0000'>2</font></strong></td>");
        output.Write("<td bgcolor='#FFFFFF'><strong><font color='DD0000'>3</font></strong></td>");

        output.Write("<td bgcolor='#A6D7C1'><strong><font color='DD0000'>0</font></strong></td>");
        output.Write("<td bgcolor='#A6D7C1'><strong><font color='DD0000'>1</font></strong></td>");
        output.Write("<td bgcolor='#A6D7C1'><strong><font color='DD0000'>2</font></strong></td>");
        output.Write("<td bgcolor='#A6D7C1'><strong><font color='DD0000'>3</font></strong></td>");

        output.Write("<td bgcolor='#C9E1BB'>A</TD>");
        output.Write("<td bgcolor='#C9E1BB'>B</TD>");
        output.Write("<td bgcolor='#C9E1BB'>C</TD>");
        output.Write("<td bgcolor='#C9E1BB'>D</TD>");
        output.Write("<td bgcolor='#C9E1BB'>E</TD>");
        output.Write("<td bgcolor='#C9E1BB'>F</TD>");
        output.Write("<td bgcolor='#C9E1BB'>G</TD>");
        output.Write("<td bgcolor='#C9E1BB'>H</TD>");
        output.Write("<td bgcolor='#C9E1BB'>I</TD>");
        output.Write("<td bgcolor='#C9E1BB'>J</TD>");

        output.Write("<tr align='center' vlign='center'>");
        output.Write("<td>期 数</td>");
        output.Write("<td>开奖号码</td>");
        output.Write("<td colspan ='5'bgcolor='#C1E7FF'>百位</td>");
        output.Write("<td colspan ='5'bgcolor='#DDFFE6'>十位</td>");
        output.Write("<td colspan ='5'bgcolor='#C1E7FF'>个位</td>");
        output.Write("<td colspan ='4'bgcolor='#FFCC33'<strong><font color='#DD0000'>大码个数</font></strong>");
        output.Write("<td colspan ='4'bgcolor='#FFFFFF'<strong><font color='#DD0000'>中码个数</font></strong>");
        output.Write("<td colspan ='4'bgcolor='#A6D7C1'<strong><font color='#DD0000'>小码个数</font></strong>");

        output.Write("<td colspan ='10'bgcolor='#C9E1BB'>总体走试分析</td>");

    }

    protected void DrawGridHeader(HtmlTextWriter output, Control ctl)
    {
        output.Write("<td rowspan ='2' align ='center' vlign='middle'>期  数</td>");
        output.Write("<td rowspan ='2'>开奖号码</td>");

        output.Write("<td colspan = '5'>百位</td>");
        output.Write("<td colspan = '5'>十位</td>");
        output.Write("<td colspan = '5'>个位</td>");

        output.Write("<td colspan = '4'><strong><font color='#DD0000'>大码个数</font></strong></td>");
        output.Write("<td colspan = '4'><strong><font color='#DD0000'>中码个数</font></strong></td>");
        output.Write("<td colspan = '4'><strong><font color='#DD0000'>小码个数</font></strong></td>");
        output.Write("<td colspan = '10'>总体走试分析</td>");

        output.Write("<tr align ='center' vlign='center'>");

        output.Write("<td>&nbsp;</td>");
        output.Write("<td>大</td>");
        output.Write("<td>中</td>");
        output.Write("<td>小</td>");
        output.Write("<td>&nbsp;</td>");

        output.Write("<td>&nbsp;</td>");
        output.Write("<td>大</td>");
        output.Write("<td>中</td>");
        output.Write("<td>小</td>");
        output.Write("<td>&nbsp</td>");

        output.Write("<td>&nbsp;</td>");
        output.Write("<td>大</td>");
        output.Write("<td>中</td>");
        output.Write("<td>小</td>");
        output.Write("<td>&nbsp;</td>");

        output.Write("<td>0</td>");
        output.Write("<td>1</td>");
        output.Write("<td>2</td>");
        output.Write("<td>3</td>");

        output.Write("<td>0</td>");
        output.Write("<td>1</td>");
        output.Write("<td>2</td>");
        output.Write("<td>3</td>");

        output.Write("<td>0</td>");
        output.Write("<td>1</td>");
        output.Write("<td>2</td>");
        output.Write("<td>3</td>");

        output.Write("<td>A</td>");
        output.Write("<td>B</td>");
        output.Write("<td>C</td>");
        output.Write("<td>D</td>");
        output.Write("<td>E</td>");
        output.Write("<td>F</td>");
        output.Write("<td>G</td>");
        output.Write("<td>H</td>");
        output.Write("<td>I</td>");
        output.Write("<td>J</td>");
    }

}
