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

public partial class TC22X5_22X5_HMLR : System.Web.UI.Page
{
    int[] num = new int[22];
    int[] sum = new int[22];
    int[] a = new int[22];
    int[] b = new int[22];

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            GridViewBind();
            GridView2Bind();
            GridView2.Style["border-collapse"] = "";
            GridView1.Style["border-collapse"] = "";
        }
    }

    protected void GridViewBind()
    {
        DataTable dt = new DataTable();
        dt = BLL.TC22X5_lengre_SeleteByNum(30);
        GridView1.DataSource = dt;
        GridView1.DataBind();
        
    }

    protected void GridView2Bind()
    {
        DataTable dt = new DataTable();

        dt = BLL.TC22X5_lengrejj_SeleteByNum(30);

        DataTable dt1 = new DataTable();

        dt1.Columns.Add("11", typeof(System.String));

        DataRow dr = dt1.NewRow();
        dr[0] = "aa";

        dt1.Rows.Add(dr);

        GridView2.DataSource = dt1;
        GridView2.DataBind();

        for (int i = 0; i < 22; i++)
        {
            a[i] = Shove._Convert.StrToInt(dt.Rows[i]["id"].ToString(), 0);
            b[i] = Shove._Convert.StrToInt(dt.Rows[i]["num"].ToString(), 0);

        }
    }

    protected void RadioButton1_CheckedChanged1(object sender, EventArgs e)
    {
        DataTable dt = BLL.TC22X5_lengre_SeleteByNum(100);
    
        DataTable dt1 = BLL.TC22X5_lengrejj_SeleteByNum(100);
     
        DataTable dt2 = new DataTable();
       
        dt2.Columns.Add("aa", typeof(System.String));

        DataRow dr = dt2.NewRow();
        dr[0] = "11";
        dt2.Rows.Add(dr);

        GridView1.DataSource = dt;
        GridView1.DataBind();

        GridView2.DataSource = dt2;
        GridView2.DataBind();
        for (int i = 0; i < 22; i++)
        {
            a[i] = Shove._Convert.StrToInt(dt1.Rows[i]["id"].ToString(), 0);
            b[i] = Shove._Convert.StrToInt(dt1.Rows[i]["num"].ToString(), 0);

        }
    }
    protected void RadioButton2_CheckedChanged(object sender, EventArgs e)
    {
        DataTable dt = BLL.TC22X5_lengre_SeleteByNum(50);

        DataTable dt1 = new DataTable();
        dt1 = BLL.TC22X5_lengrejj_SeleteByNum(50);
        GridView1.DataSource = dt;
        GridView1.DataBind();

        DataTable dt2 = new DataTable();
        dt2.Columns.Add("num", typeof(System.String));
        DataRow dr = dt2.NewRow();
        dr[0] = "12";
        dt2.Rows.Add(dr);

        GridView2.DataSource = dt2;
        GridView2.DataBind();

        for (int i = 0; i < 22; i++)
        {
            a[i] = Shove._Convert.StrToInt(dt1.Rows[i]["id"].ToString(), 0);
            b[i] = Shove._Convert.StrToInt(dt1.Rows[i]["num"].ToString(), 0);

        }
    }
    protected void RadioButton3_CheckedChanged(object sender, EventArgs e)
    {
        DataTable dt = BLL.TC22X5_lengre_SeleteByNum(30);
        GridView1.DataSource = dt;
        GridView1.DataBind();

        DataTable dt1 = new DataTable();
        dt1 = BLL.TC22X5_lengrejj_SeleteByNum(30);

        DataTable dt2 = new DataTable();
        dt2.Columns.Add("num", typeof(System.String));
        DataRow dr = dt2.NewRow();
        
        dr[0] = "12";
        dt2.Rows.Add(dr);

        GridView2.DataSource = dt2;
        GridView2.DataBind();

        for (int i = 0; i < 22; i++)
        {
            a[i] = Shove._Convert.StrToInt(dt1.Rows[i]["id"].ToString(), 0);
            b[i] = Shove._Convert.StrToInt(dt1.Rows[i]["num"].ToString(), 0);

        }
    }
    protected void RadioButton4_CheckedChanged1(object sender, EventArgs e)
    {
        DataTable dt = BLL.TC22X5_lengre_SeleteByNum(20);
        GridView1.DataSource = dt;
        GridView1.DataBind();

        DataTable dt1 = new DataTable();
        dt1 = BLL.TC22X5_lengrejj_SeleteByNum(20);
        DataTable dt2 = new DataTable();
        dt2.Columns.Add("num", typeof(System.String));
        DataRow dr = dt2.NewRow();
       
        dr[0] = "12";
        dt2.Rows.Add(dr);

        GridView2.DataSource = dt2;
        GridView2.DataBind();

        for (int i = 0; i < 22; i++)
        {
            a[i] = Shove._Convert.StrToInt(dt1.Rows[i]["id"].ToString(), 0);
            b[i] = Shove._Convert.StrToInt(dt1.Rows[i]["num"].ToString(), 0);

        }
    }
    protected void RadioButton5_CheckedChanged1(object sender, EventArgs e)
    {
        DataTable dt = BLL.TC22X5_lengre_SeleteByNum(10);
        GridView1.DataSource = dt;
        GridView1.DataBind();
        DataTable dt1 = new DataTable();

        dt1 = BLL.TC22X5_lengrejj_SeleteByNum(10);

        DataTable dt2 = new DataTable();
        dt2.Columns.Add("num", typeof(System.String));
        DataRow dr = dt2.NewRow();
      
        dr[0] = "12";
        dt2.Rows.Add(dr);

        GridView2.DataSource = dt2;
        GridView2.DataBind();

        for (int i = 0; i < 22; i++)
        {
            a[i] = Shove._Convert.StrToInt(dt1.Rows[i]["id"].ToString(), 0);
            b[i] = Shove._Convert.StrToInt(dt1.Rows[i]["num"].ToString(), 0);

        }
    }

    protected void GridView1_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.SetRenderMethodDelegate(new RenderMethod(DrawGridFooter));
        }
    }

    protected void DrawGridFooter(HtmlTextWriter output, Control ctl)
    {
        for(int j = 1;j < 23;j++)
        {
            int k = j - 1;

            num[k] = Shove._Convert.StrToInt(GridView1.Rows[0].Cells[j].Text, 0);
           
            int a = RadioSelete();

            sum[k] = num[k] * 150 / a;
        }

        output.Write("<td bgcolor='#F7F7F7' height='140px' width='60px'><br><font color='#5A5A5A'>75%</font><br><br><font color='#5A5A5A'>50%</font><br><br><font color='#5A5A5A'>25%</font><br><br></td>");

        for (int i = 0; i < 22; i++)
        {
            output.Write("<td align='center' valign='bottom' background='../Image/line[1].gif'>");
            output.Write("<img src='../Image/01[1].gif' height='" + sum[i].ToString() + "'' title='" + num[i].ToString() + "' width='8px'></td>");

        }

        output.Write("<tr align='center'>");
        output.Write("<td bgcolor='#F7F7F7'>号码</td>");
        for (int i = 1; i < 10; i++)
        {
            output.Write("<td align='center' >");
            output.Write(0 + i.ToString() + "</td>");
        }

        for (int i = 10; i < 23; i++)
        {
            output.Write("<td align='center'>");
            output.Write(i.ToString() + "</td>");
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

    protected void GridView2_RowCreate(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.SetRenderMethodDelegate(new RenderMethod(DrawGridRow));
        }
    }

    private void DrawGridRow(HtmlTextWriter output,Control ctl)
    {
        int c = RadioSelete();
        int[] h = new int[22];

        for (int i = 0; i < 22; i++)
        {
            h[i] = a[i] * 150 / c;
        }

        output.Write("<td bgcolor='#F7F7F7' width='68px'> 出现次数</td>");

        for (int i = 0; i < 22; i++)
        {
            output.Write("<td width='28px'>");
            output.Write(a[i].ToString() + "</td>");
        }

        output.Write("<tr align='center' valign='middle'>");
        output.Write("<td bgcolor='#F7F7F7' height='140px' width='68px'><br><font color='#5A5A5A'>75%</font><br><br><font color='#5A5A5A'>50%</font><br><br><font color='#5A5A5A'>25%</font><br><br></td>");
        for (int i = 0; i < 22; i++)
        {

            output.Write("<td align='center' valign='bottom' background='../Image/line[1].gif' width='28px'>");
            output.Write("<Img src='../Image/01[1].gif ' height='"+ h[i].ToString() +"' width='8' title='"+ a[i].ToString() +"'></td>");
        }

        output.Write("<tr align='center' valign='middle'>");
        output.Write("<td bgcolor='F7F7F7' width='28px'> 号码</td>");
        for (int i = 0; i < 22; i++)
        {
            output.Write("<td>");
            output.Write(b[i].ToString() + "</td>");
        }           
    }
}
