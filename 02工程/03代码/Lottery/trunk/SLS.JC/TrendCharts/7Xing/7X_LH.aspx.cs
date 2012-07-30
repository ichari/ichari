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

public partial class TrendCharts_7Xing_7X_LH : TrendChartPageBase
{
    public int[] sum = new int[7];
    public string[] tr = new string[7];
    public int tem = 0;
    public int temp = 0;
    public int qq = 0;
    public string[] bbb = new string[2];

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
        GridView1.DataSource = BLL.X7_LH_SeleteByNum(30);
        GridView1.DataBind();
    }

    private void GridViewbindColor()
    {
        tem = GridView1.Rows.Count;

        for (int i = 3; i < 10; i++)
        {
            int num = 0;
            for (int j = 0; j < GridView1.Rows.Count; j++)
            {
                if (GridView1.Rows[j].Cells[i].Text == "0")
                {
                    num = num + 1;
                }
            }
            sum[i - 3] = num;
        }

        float a = sum[0] + sum[1] + sum[2] + sum[3] + sum[4] + sum[5] + sum[6];
        float b = (sum[0] / a) * 100;
        float c = (sum[1] / a) * 100;
        float d = (sum[2] / a) * 100;
        float e = (sum[3] / a) * 100;
        float f = (sum[4] / a) * 100;
        float ee = (sum[5] / a) * 100;
        float ff = (sum[6] / a) * 100;
        string x = b.ToString();
        string y = c.ToString();
        string z = d.ToString();
        string mm = e.ToString();
        string n = f.ToString();
        string nn = ee.ToString();
        string nnn = ff.ToString();
        string[] str ={ x, y, z, mm, n,nn,nnn };

        for (int k = 0; k < 7; k++)
        {
            if (str[k].Contains(".") == false)
            {
                tr[k] = str[k] + "%";
            }

            else
            {
                if (str[k].IndexOf('.') == 1)
                {
                    tr[k] = str[k].Substring(0, 4) + "%";
                }
                else
                {
                    tr[k] = str[k].Substring(0, 5) + "%";
                }
            }
        }

        for (int i = 0; i < GridView1.Rows.Count; i++)
        {
            if (GridView1.Rows[i].Cells[2].Text == "0")
            {
                GridView1.Rows[i].Cells[2].Text = "&nbsp;";
            }
            if (GridView1.Rows[i].Cells[2].Text == "1")
            {
                GridView1.Rows[i].Cells[2].Text = "01";
            }


            for (int j = 3; j < 10; j++)
            {

                if (GridView1.Rows[i].Cells[j].Text == "0")
                {
                    GridView1.Rows[i].Cells[j].Text = "●";
                    GridView1.Rows[i].Cells[j].ForeColor = System.Drawing.Color.FromArgb(204, 000, 000);
                }
                
                if (GridView1.Rows[i].Cells[j].Text == "-1")
                {
                    GridView1.Rows[i].Cells[j].Text = "&nbsp;";
                }
            }

            for (int j = 10; j < 16; j++)
            {
                if (GridView1.Rows[i].Cells[j].Text == "升")
                {
                    qq = qq + 1;
                    GridView1.Rows[i].Cells[j].ForeColor = System.Drawing.Color.FromArgb(255, 000, 000);
                }

                if (GridView1.Rows[i].Cells[j].Text == "降")
                {
                    temp = temp + 1;
                    GridView1.Rows[i].Cells[j].ForeColor = System.Drawing.Color.FromArgb(000, 000, 255);
                }
            }

            float m = qq + temp;
            float l = (temp / m) * 100;
            float g = (qq / m) * 100;
            string ll = l.ToString();
            string gg = g.ToString();
            string[] hh ={ ll, gg };
            for (int k = 0; k < 2; k++)
            {
                if (hh[k].Contains(".") == false)
                {
                    bbb[k] = hh[k] + "%";
                }

                else
                {
                    if (hh[k].IndexOf('.') == 1)
                    {
                        bbb[k] = hh[k].Substring(0, 3) + "%";
                    }
                    else
                    {
                        bbb[k] = hh[k].Substring(0, 4) + "%";
                    }
                }
            }
        }
    }

    #region 按钮事件
    protected void RadioButton1_CheckedChanged1(object sender, EventArgs e)
    {
        DataTable dt = BLL.X7_LH_SeleteByNum(100);
        GridView1.DataSource = dt;
        GridView1.DataBind();
        GridViewbindColor();
        //BindNumber();
    }

    protected void RadioButton2_CheckedChanged(object sender, EventArgs e)
    {
        DataTable dt = BLL.X7_LH_SeleteByNum(50);
        GridView1.DataSource = dt;
        GridView1.DataBind();
        GridViewbindColor();
        //BindNumber();
    }

    protected void RadioButton3_CheckedChanged(object sender, EventArgs e)
    {
        DataTable dt = BLL.X7_LH_SeleteByNum(30);
        GridView1.DataSource = dt;
        GridView1.DataBind();
        GridViewbindColor();
        //BindNumber();
    }

    protected void RadioButton4_CheckedChanged1(object sender, EventArgs e)
    {
        DataTable dt = BLL.X7_LH_SeleteByNum(20);
        GridView1.DataSource = dt;
        GridView1.DataBind();
        GridViewbindColor();
        //BindNumber();
    }

    protected void RadioButton5_CheckedChanged1(object sender, EventArgs e)
    {
        DataTable dt = BLL.X7_LH_SeleteByNum(10);
        GridView1.DataSource = dt;
        GridView1.DataBind();
        GridViewbindColor();
        //BindNumber();
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

    private void DrawGridHeader(HtmlTextWriter output, Control ctl)
    {
        output.Write("<td rowspan ='2'align ='center' vlign='middle'>期　数</td>");
        output.Write("<td rowspan ='2'align ='center' vlign='middle'>七星彩开奖号码</td>");
        output.Write("<td rowspan ='2'align ='center' vlign='middle' ><font color='#FF0000'>");
        output.Write("连号数</font></td>");
        output.Write("<TD align ='center' vlign='middle'colspan ='7'bgcolor='#99CC99'><font color='#FF0000'>连号个数</font></td>");
        output.Write("<td align ='center' vlign='middle'colspan ='6' bgcolor='#FFCC99'><font color='#FF0000'>排序</font></td>");

        output.Write("<tr align ='center' vlign='middle'>");
        output.Write("<td align ='center' vlign='middle' bgcolor='#DCEDDC'>0</td>");
        output.Write("<td align ='center' vlign='middle' bgcolor='#DCEDDC'>1</td>");
        output.Write("<td align ='center' vlign='middle' bgcolor='#DCEDDC'>2</td>");
        output.Write("<td align ='center' vlign='middle' bgcolor='#FFC9C8'>3</td>");
        output.Write("<td align ='center' vlign='middle' bgcolor='#FFC9C8'>4</td>");
        output.Write("<td align ='center' vlign='middle' bgcolor='#E6FFE6'>5</td>");
        output.Write("<td align ='center' vlign='middle' bgcolor='#FFE1E1'>6</td>");

        output.Write("<td align ='center' vlign='middle' bgcolor='#D9F5FF'>1-2</td>");
        output.Write("<td align ='center' vlign='middle' bgcolor='#D9F5FF'>2-3</td>");
        output.Write("<td align ='center' vlign='middle' bgcolor='#F1F7D2'>3-4</td>");
        output.Write("<td align ='center' vlign='middle' bgcolor='#F1F7D2'>4-5</td>");
        output.Write("<td align ='center' vlign='middle' bgcolor='#E1E1FF'>5-6</td>");
        output.Write("<td align ='center' vlign='middle' bgcolor='#E6FFE6'>6-7</td>");
    }

    private void DrawGridFooter(HtmlTextWriter output, Control ctl)
    {
        output.Write("<td align='center' vlign='middle'>期 数</td>");
        output.Write("<td align='center' vlign='middle'>七星彩开奖号码</td>");
        output.Write("<td align='center' vlign='middle'><font color='FF0000'> 连号数</font></td>");
        output.Write("<td bgcolor='#DCEDDC'>");
        output.Write("0</td>");
        output.Write("<td bgcolor='#DCEDDC'>1</td>");
        output.Write("<td bgcolor='#DCEDDC'>2</td>");
        output.Write("<td bgcolor='#FFC9C8'>3</td>");
        output.Write("<td bgcolor='#FFC9C8'>4</td>");
        output.Write("<td align ='center' vlign='middle' bgcolor='#E6FFE6'>5</td>");
        output.Write("<td align ='center' vlign='middle' bgcolor='#FFE1E1'>6</td>");
        output.Write("<td  bgcolor='#D9F5FF'>1-2</td>");
        output.Write("<td  bgcolor='#D9F5FF'>2-3</td>");
        output.Write("<td  bgcolor='#F1F7D2'>3-4</td>");
        output.Write("<td  bgcolor='#F1F7D2'>4-5</td>");
        output.Write("<td align ='center' vlign='middle' bgcolor='#E1E1FF'>5-6</td>");
        output.Write("<td align ='center' vlign='middle' bgcolor='#E6FFE6'>6-7</td>");


    }

}
