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

public partial class View_PL3_JO : TrendChartPageBase
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
        dt = BLL.PL3_JO_SeleteByNum(30);
        GridView1.DataSource = dt;
        GridView1.DataBind();
    }
    #region 颜色控制

    protected void ColorBind()
    {
        for (int i = 0; i < GridView1.Rows.Count; i++)
        {
            GridView1.Rows[i].Cells[1].ForeColor = System.Drawing.Color.FromArgb(000, 000, 255);
        }

        for (int i = 0; i < GridView1.Rows.Count; i++)
        {
            for (int j = 2; j < 7; j++)
            {
                GridView1.Rows[i].Cells[j].BackColor = System.Drawing.Color.FromArgb(252, 223, 197);

                if (GridView1.Rows[i].Cells[j].Text == "奇" || GridView1.Rows[i].Cells[j].Text == "偶")
                {
                    GridView1.Rows[i].Cells[j].BackColor = System.Drawing.Color.FromArgb(221, 000, 000);
                    GridView1.Rows[i].Cells[j].ForeColor = System.Drawing.Color.White;
                }
            }
        }

        for (int i = 0; i < GridView1.Rows.Count; i++)
        {
            for (int j = 7; j < 12; j++)
            {
                GridView1.Rows[i].Cells[j].BackColor = System.Drawing.Color.FromArgb(221, 255, 230);

                if (GridView1.Rows[i].Cells[j].Text == "奇" || GridView1.Rows[i].Cells[j].Text == "偶")
                {
                    GridView1.Rows[i].Cells[j].BackColor = System.Drawing.Color.FromArgb(221, 000, 000);
                    GridView1.Rows[i].Cells[j].ForeColor = System.Drawing.Color.White;
                }
            }
        }

        for (int i = 0; i < GridView1.Rows.Count; i++)
        {
            for (int j = 12; j < 17; j++)
            {
                GridView1.Rows[i].Cells[j].BackColor = System.Drawing.Color.FromArgb(193, 231, 255);

                if (GridView1.Rows[i].Cells[j].Text == "奇" || GridView1.Rows[i].Cells[j].Text == "偶")
                {
                    GridView1.Rows[i].Cells[j].BackColor = System.Drawing.Color.FromArgb(221, 000, 000);
                    GridView1.Rows[i].Cells[j].ForeColor = System.Drawing.Color.White;
                }
            }
        }

        for (int i = 0; i < GridView1.Rows.Count; i++)
        {
            for (int j = 17; j < 21; j++)
            {
                GridView1.Rows[i].Cells[j].BackColor = System.Drawing.Color.FromArgb(255, 204, 051);

                if (GridView1.Rows[i].Cells[j].Text == "0")
                {
                    GridView1.Rows[i].Cells[j].BackColor = System.Drawing.Color.FromArgb(000, 000, 204);
                    GridView1.Rows[i].Cells[j].ForeColor = System.Drawing.Color.White;
                }
            }
        }

        for (int i = 0; i < GridView1.Rows.Count; i++)
        {
            if (GridView1.Rows[i].Cells[17].Text == "0")
            {
                GridView1.Rows[i].Cells[17].Text = "A";
                GridView1.Rows[i].Cells[17].ForeColor = System.Drawing.Color.White;

            }

            if (GridView1.Rows[i].Cells[18].Text == "0")
            {
                GridView1.Rows[i].Cells[18].Text = "B";
                GridView1.Rows[i].Cells[18].ForeColor = System.Drawing.Color.White;
            }

            if (GridView1.Rows[i].Cells[19].Text == "0")
            {
                GridView1.Rows[i].Cells[19].Text = "C";
                GridView1.Rows[i].Cells[19].ForeColor = System.Drawing.Color.White;
            }

            if (GridView1.Rows[i].Cells[20].Text == "0")
            {
                GridView1.Rows[i].Cells[20].Text = "D";
                GridView1.Rows[i].Cells[20].ForeColor = System.Drawing.Color.White;
            }
        }

        for (int i = 0; i < GridView1.Rows.Count; i++)
        {
            for (int j = 21; j < 29; j++)
            {
                GridView1.Rows[i].Cells[j].BackColor = System.Drawing.Color.FromArgb(216, 234, 206);

                if (GridView1.Rows[i].Cells[j].Text == "0")
                {
                    GridView1.Rows[i].Cells[j].BackColor = System.Drawing.Color.FromArgb(051, 128, 189);
                }
            }
        }

        for (int i = 0; i < GridView1.Rows.Count; i++)
        {
            if (GridView1.Rows[i].Cells[21].Text == "0")
            {
                GridView1.Rows[i].Cells[21].Text = "0";
                GridView1.Rows[i].Cells[21].ForeColor = System.Drawing.Color.White;

            }

            if (GridView1.Rows[i].Cells[22].Text == "0")
            {
                GridView1.Rows[i].Cells[22].Text = "1";
                GridView1.Rows[i].Cells[22].ForeColor = System.Drawing.Color.White;

            }

            if (GridView1.Rows[i].Cells[23].Text == "0")
            {
                GridView1.Rows[i].Cells[23].Text = "2";
                GridView1.Rows[i].Cells[23].ForeColor = System.Drawing.Color.White;

            }

            if (GridView1.Rows[i].Cells[24].Text == "0")
            {
                GridView1.Rows[i].Cells[24].Text = "3";
                GridView1.Rows[i].Cells[24].ForeColor = System.Drawing.Color.White;

            }

            if (GridView1.Rows[i].Cells[25].Text == "0")
            {
                GridView1.Rows[i].Cells[25].Text = "4";
                GridView1.Rows[i].Cells[25].ForeColor = System.Drawing.Color.White;

            }

            if (GridView1.Rows[i].Cells[26].Text == "0")
            {
                GridView1.Rows[i].Cells[26].Text = "5";
                GridView1.Rows[i].Cells[26].ForeColor = System.Drawing.Color.White;

            }

            if (GridView1.Rows[i].Cells[27].Text == "0")
            {
                GridView1.Rows[i].Cells[27].Text = "6";
                GridView1.Rows[i].Cells[27].ForeColor = System.Drawing.Color.White;

            }

            if (GridView1.Rows[i].Cells[28].Text == "0")
            {
                GridView1.Rows[i].Cells[28].Text = "7";
                GridView1.Rows[i].Cells[28].ForeColor = System.Drawing.Color.White;

            }
        }

        for (int i = 0; i < GridView1.Rows.Count; i++)
        {
            for (int j = 17; j < 22; j++)
            {
                if (GridView1.Rows[i].Cells[j].BackColor == System.Drawing.Color.FromArgb(255, 204, 051))
                {
                    GridView1.Rows[i].Cells[j].ForeColor = System.Drawing.Color.FromArgb(128, 128, 128);
                }
            }
        }

        for (int i = 0; i < GridView1.Rows.Count; i++)
        {
            for (int j = 21; j < 29; j++)
            {
                if (GridView1.Rows[i].Cells[j].BackColor == System.Drawing.Color.FromArgb(216, 234, 206))
                {
                    GridView1.Rows[i].Cells[j].ForeColor = System.Drawing.Color.FromArgb(170, 170, 170);
                }
            }
        }
    }

    #endregion

    #region 按纽选择事件

    protected void RadioButton1_CheckedChanged1(object sender, EventArgs e)
    {
        DataTable dt = BLL.PL3_JO_SeleteByNum(100);
        GridView1.DataSource = dt;
        GridView1.DataBind();
        ColorBind();
    }
    protected void RadioButton2_CheckedChanged(object sender, EventArgs e)
    {
        DataTable dt = BLL.PL3_JO_SeleteByNum(50);
        GridView1.DataSource = dt;
        GridView1.DataBind();
        ColorBind();

    }
    protected void RadioButton3_CheckedChanged(object sender, EventArgs e)
    {
        DataTable dt = BLL.PL3_JO_SeleteByNum(30);
        GridView1.DataSource = dt;
        GridView1.DataBind();
        ColorBind();
    }
    protected void RadioButton4_CheckedChanged1(object sender, EventArgs e)
    {
        DataTable dt = BLL.PL3_JO_SeleteByNum(20);
        GridView1.DataSource = dt;
        GridView1.DataBind();
        ColorBind();
    }
    protected void RadioButton5_CheckedChanged1(object sender, EventArgs e)
    {
        DataTable dt = BLL.PL3_JO_SeleteByNum(10);
        GridView1.DataSource = dt;
        GridView1.DataBind();
        ColorBind();
    }

    #endregion

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

        output.Write("<td colspan = '5'>百位</td>");
        output.Write("<td colspan = '5'>十位</td>");
        output.Write("<td colspan = '5'>个位</td>");

        output.Write("<td colspan = '4'><strong><font color='#DD0000'>组选组合</font></strong></td>");
        output.Write("<td colspan = '8'>单选组合</td>");

        output.Write("<tr align ='center' vlign='center'>");

        output.Write("<td>&nbsp;</td>");
        output.Write("<td>奇</td>");
        output.Write("<td>&nbsp;</td>");
        output.Write("<td>偶</td>");
        output.Write("<td>&nbsp;</td>");

        output.Write("<td>&nbsp;</td>");
        output.Write("<td>奇</td>");
        output.Write("<td>&nbsp</td>");
        output.Write("<td>偶</td>");
        output.Write("<td>&nbsp</td>");

        output.Write("<td>&nbsp;</td>");
        output.Write("<td>奇</td>");
        output.Write("<td>&nbsp;</td>");
        output.Write("<td>偶</td>");
        output.Write("<td>&nbsp;</td>");

        output.Write("<td>A</td>");
        output.Write("<td>B</td>");
        output.Write("<td>C</td>");
        output.Write("<td>D</td>");

        output.Write("<td>0</td>");
        output.Write("<td>1</td>");
        output.Write("<td>2</td>");
        output.Write("<td>3</td>");
        output.Write("<td>4</td>");
        output.Write("<td>5</td>");
        output.Write("<td>6</td>");
        output.Write("<td>7</td>");

    }

    protected void DrawGridFooter(HtmlTextWriter output, Control ctl)
    {
        //output.Write("<tr align='center' valign='middle'>");

        output.Write("<td colspan='2' rowspan='5'>预测方法：<br>用鼠标点击<br>方格即可显示<br>为开奖号色</td>");

        output.Write("<td bgcolor='#FCDFC5'>&nbsp;</td>");
        output.Write("<td bgcolor='#FCDFC5'onClick=Style(this,'#DD0000','#FCDFC5') style='color:#FCDFC5'>奇</td>");
        output.Write("<td bgcolor='#FCDFC5'>&nbsp;</td>");
        output.Write("<td bgcolor='#FCDFC5'onClick=Style(this,'#DD0000','#FCDFC5') style='color:#FCDFC5'>偶</td>");
        output.Write("<td bgcolor='#FCDFC5'>&nbsp;</td>");

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



        output.Write("<td bgcolor='#FFCC33' onClick=Style(this,'#0000CC','#FFCC33') style='color:#FFCC33'>A</td>");
        output.Write("<td bgcolor='#FFCC33' onClick=Style(this,'#0000CC','#FFCC33') style='color:#FFCC33'>B</td>");
        output.Write("<td bgcolor='#FFCC33' onClick=Style(this,'#0000CC','#FFCC33') style='color:#FFCC33'>C</td>");
        output.Write("<td bgcolor='#FFCC33' onClick=Style(this,'#0000CC','#FFCC33') style='color:#FFCC33'>D</td>");


        for (int i = 0; i < 8; i++)
        {
            output.Write("<td bgcolor='#D8EACE'onclick=Style(this,'#3380BD','#D8EACE') style='color:#D8EACE'>");
            output.Write(i.ToString() + "</td>");
        }


        for (int n = 0; n < 4; n++)
        {
            output.Write("<tr align='center' valign='middle'>");

            output.Write("<td bgcolor='#FCDFC5'>&nbsp;</td>");
            output.Write("<td bgcolor='#FCDFC5'onClick=Style(this,'#DD0000','#FCDFC5') style='color:#FCDFC5'>奇</td>");
            output.Write("<td bgcolor='#FCDFC5'>&nbsp;</td>");
            output.Write("<td bgcolor='#FCDFC5'onClick=Style(this,'#DD0000','#FCDFC5') style='color:#FCDFC5'>偶</td>");
            output.Write("<td bgcolor='#FCDFC5'>&nbsp;</td>");

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

            output.Write("<td bgcolor='#FFCC33'onClick=Style(this,'#0000CC','#FFCC33') style='color:#FFCC33;'>A</td>");
            output.Write("<td bgcolor='#FFCC33'onClick=Style(this,'#0000CC','#FFCC33') style='color:#FFCC33;'>B</td>");
            output.Write("<td bgcolor='#FFCC33'onClick=Style(this,'#0000CC','#FFCC33') style='color:#FFCC33;'>C</td>");
            output.Write("<td bgcolor='#FFCC33'onClick=Style(this,'#0000CC','#FFCC33') style='color:#FFCC33;'>d</td>");

            for (int i = 0; i < 8; i++)
            {
                output.Write("<td bgcolor='#D8EACE'onclick=Style(this,'#3380BD','#D8EACE') style='color:#D8EACE'>");
                output.Write(i.ToString() + "</td>");
            }


        }

        // --最后两行的添加

        output.Write("<tr align='center' vlign = 'middle'>");
        output.Write("<td>&nbsp</td>");
        output.Write("<td>&nbsp</td>");
        output.Write("<td bgcolor='#FCDFC5'>&nbsp;</td>");
        output.Write("<td bgcolor='#FCDFC5'><strong><font color='DD0000'>奇</font></strong></td>");
        output.Write("<td bgcolor='#FCDFC5'>&nbsp;</td>");
        output.Write("<td bgcolor='#FCDFC5'><strong><font color='DD0000'>偶</font></strong></td>");
        output.Write("<td bgcolor='#FCDFC5'>&nbsp;</td>");

        output.Write("<td bgcolor='#DDFFE6'>&nbsp;</td>");
        output.Write("<td bgcolor='#DDFFE6'><strong><font color='DD0000'>奇</font></strong></td>");
        output.Write("<td bgcolor='#DDFFE6'>&nbsp;</td>");
        output.Write("<td bgcolor='#DDFFE6'><strong><font color='DD0000'>偶</font></strong></td>");
        output.Write("<td bgcolor='#DDFFE6'>&nbsp;</td>");

        output.Write("<td bgcolor='#C1E7FF'>&nbsp;</td>");
        output.Write("<td bgcolor='#C1E7FF'><strong><font color='DD0000'>奇</font></strong></td>");
        output.Write("<td bgcolor='#C1E7FF'>&nbsp;</td>");
        output.Write("<td bgcolor='#C1E7FF'><strong><font color='DD0000'>偶</font></strong></td>");
        output.Write("<td bgcolor='#C1E7FF'>&nbsp;</td>");

        output.Write("<td bgcolor='#FFCC33'><strong><font color='DD0000'>A</font></strong></td>");
        output.Write("<td bgcolor='#FFCC33'><strong><font color='DD0000'>B</font></strong></td>");
        output.Write("<td bgcolor='#FFCC33'><strong><font color='DD0000'>C</font></strong></td>");
        output.Write("<td bgcolor='#FFCC33'><strong><font color='DD0000'>D</font></strong></td>");

        for (int a = 0; a < 8; a++)
        {
            output.Write("<td bgcolor='#D8EACE'><strong><font color ='DD0000'>");
            output.Write(a.ToString() + "</font></strong></td>");
        }

        output.Write("<tr align='center' vlign='center'>");
        output.Write("<td>期数</td>");
        output.Write("<td>开奖号码</td>");
        output.Write("<td colspan ='5'bgcolor='#C1E7FF'>百位</td>");
        output.Write("<td colspan ='5'bgcolor='#DDFFE6'>十位</td>");
        output.Write("<td colspan ='5'bgcolor='#C1E7FF'>个位</td>");
        output.Write("<td colspan ='4'bgcolor='#FFCC33'<strong><font color='#DD0000'>");
        output.Write("组选组合</td>");
        output.Write("<td colspan ='8'bgcolor='#C9E1BB'>单选大小组合</td>");

    }
}
