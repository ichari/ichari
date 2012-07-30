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

public partial class TC22X5_22X5_LH : System.Web.UI.Page
{
    public int[] sum = new int[5];
    public string[] tr = new string[5];
    public int tem = 0;
    public int temp = 0;
  
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
        GridView1.DataSource = BLL.TC22X5_LH_SeleteByNum(30);
        GridView1.DataBind();
    }

    private void GridViewbindColor()
    {
      for (int k = GridView1.Rows.Count; k > 0; k--)
        {
            int j = GridView1.Rows.Count - k;
            GridView1.Rows[j].Cells[0].Text = k.ToString();
        }

       tem = GridView1.Rows.Count;

        for (int i = 4; i < 9; i++)
        {
            int num = 0;
            for (int j = 0; j < GridView1.Rows.Count; j++)
            {
                if (GridView1.Rows[j].Cells[i].Text == "0")
                {
                    num = num + 1;
                }
            }
            sum[i - 4] = num;
        }

        float a = sum[0] + sum[1] + sum[2] + sum[3] + sum[4];
        float b = (sum[0] / a) * 100;
        float c = (sum[1] / a) * 100;
        float d = (sum[2] / a) * 100;
        float e = (sum[3] / a) * 100;
        float f = (sum[4] / a) * 100;
        string x = b.ToString();
        string y = c.ToString();
        string z = d.ToString();
        string mm = e.ToString();
        string n = f.ToString();

        string[] str ={ x, y, z, mm, n };

        for (int k = 0; k < 5; k++)
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
            for (int j = 4; j < 9; j++)
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
        }
    }

    #region 按钮事件
    protected void RadioButton1_CheckedChanged1(object sender, EventArgs e)
    {
        DataTable dt = BLL.TC22X5_LH_SeleteByNum(100);
        GridView1.DataSource = dt;
        GridView1.DataBind();
        GridViewbindColor();
    }

    protected void RadioButton2_CheckedChanged(object sender, EventArgs e)
    {
        DataTable dt = BLL.TC22X5_LH_SeleteByNum(50);
        GridView1.DataSource = dt;
        GridView1.DataBind();
        GridViewbindColor();
    }

    protected void RadioButton3_CheckedChanged(object sender, EventArgs e)
    {
        DataTable dt = BLL.TC22X5_LH_SeleteByNum(30);
        GridView1.DataSource = dt;
        GridView1.DataBind();
        GridViewbindColor();
    }

    protected void RadioButton4_CheckedChanged1(object sender, EventArgs e)
    {
        DataTable dt = BLL.TC22X5_LH_SeleteByNum(20);
        GridView1.DataSource = dt;
        GridView1.DataBind();
        GridViewbindColor();
    }

    protected void RadioButton5_CheckedChanged1(object sender, EventArgs e)
    {
        DataTable dt = BLL.TC22X5_LH_SeleteByNum(10);
        GridView1.DataSource = dt;
        GridView1.DataBind();
        GridViewbindColor();
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
        output.Write("<td rowspan ='2'align ='center' vlign='middle'>序  号</td>");
        output.Write("<td rowspan ='2'align ='center' vlign='middle'>期　数</td>");
        output.Write("<td rowspan ='2'align ='center' vlign='middle'>体彩22选5开奖号码</td>");
        output.Write("<td rowspan ='2'align ='center' vlign='middle' ><font color='#FF0000'>");
        output.Write("连号数</font></td>");
        output.Write("<TD align ='center' vlign='middle'colspan ='5'bgcolor='#99CC99'><font color='#FF0000'>连号个数</font></td>");
      
        output.Write("<tr align ='center' vlign='middle'>");
        output.Write("<td align ='center' vlign='middle' bgcolor='#DCEDDC'>0</td>");
        output.Write("<td align ='center' vlign='middle' bgcolor='#DFD0DF'>1</td>");
        output.Write("<td align ='center' vlign='middle' bgcolor='#DFDFFF'>2</td>");
        output.Write("<td align ='center' vlign='middle' bgcolor='#D9F5FF'>3</td>");
        output.Write("<td align ='center' vlign='middle' bgcolor='#F1F7D2'>4</td>");
    }

    private void DrawGridFooter(HtmlTextWriter output, Control ctl)
    {
        output.Write("<td align='center' vlign='middle'>序 号</td>");
        output.Write("<td align='center' vlign='middle'>期 数</td>");
        output.Write("<td align='center' vlign='middle'>体彩22选5开奖号码</td>");
        output.Write("<td align='center' vlign='middle'><font color='FF0000'> 连号数</font></td>");
        output.Write("<td bgcolor='#DCEDDC'>");
        output.Write("0</td>");
        output.Write("<td bgcolor='#DFD0DF'>1</td>");
        output.Write("<td bgcolor='#DFDFFF'>2</td>");
        output.Write("<td bgcolor='#D9F5FF'>3</td>");
        output.Write("<td bgcolor='#F1F7D2'>4</td>");

    }
}
