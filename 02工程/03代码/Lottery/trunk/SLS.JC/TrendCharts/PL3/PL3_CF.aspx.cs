using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;


public partial class View_PL3_CF : TrendChartPageBase
{
    #region 图片高赋初值

    #endregion

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
        //DataSet ds = new DataSet();
        //ds = BLL.PL3_Select();
        DataTable dt = BLL.PL3_SeleteByNum(30);
        GridView1.DataSource = BLL.PL3_SeleteByNum(30);
        GridView1.DataBind();
    }

    private void GridViewbindColor()
    {
        for (int i = 0; i < (GridView1.Rows.Count); i++)
            for (int j = 2; j < (GridView1.Columns.Count); j++)
            {
                GridView1.Rows[i].Cells[1].ForeColor = System.Drawing.Color.Blue;
                // GridView1.Rows[i].Cells[1].Font.Bold.ToString();

                if (GridView1.Rows[i].Cells[j].Text == "0")
                {
                   // GridView1.Rows[i].Cells[j].ForeColor = System.Drawing.Color.White;
                    GridView1.Rows[i].Cells[j].Text = "&nbsp;";
                }
                if (GridView1.Rows[i].Cells[j].Text == "1")
                {
                    GridView1.Rows[i].Cells[j].ForeColor = System.Drawing.Color.Black;
                    GridView1.Rows[i].Cells[j].BackColor = System.Drawing.Color.FromArgb(238, 238, 238);
                }
                if (GridView1.Rows[i].Cells[j].Text == "2")
                {
                    GridView1.Rows[i].Cells[j].ForeColor = System.Drawing.Color.White;
                    GridView1.Rows[i].Cells[j].BackColor = System.Drawing.Color.FromArgb(221, 000, 000);
                }
                if (GridView1.Rows[i].Cells[j].Text == "3")
                {
                    GridView1.Rows[i].Cells[j].ForeColor = System.Drawing.Color.White;
                    GridView1.Rows[i].Cells[j].BackColor = System.Drawing.Color.FromArgb(000, 000, 255);
                }
            }
      }

#region
    //private void bigImgageHeight()
    //{
    //    int num = Shove._Convert.StrToInt(GridView1.Rows[0].Cells[5].Text.ToString(), 0);

    //    for (int i = 1; i < GridView1.Rows.Count-1; i++)
    //    {
    //        if (num <= Shove._Convert.StrToInt(GridView1.Rows[i].Cells[5].Text.ToString(), 0))
    //        {
    //            num = Shove._Convert.StrToInt(GridView1.Rows[i].Cells[5].Text.ToString(), 0);
    //        }

    //    }

       

    //        if (Shove._Convert.StrToInt(GridView1.Rows[i].Cells[2].Text.ToString(), 0) >= Shove._Convert.StrToInt(GridView1.Rows[i + 1].Cells[2].Text.ToString(), 0))
    //        //{
    //        //    num = Shove._Convert.StrToInt(GridView1.Rows[i].Cells[2].Text.ToString(), 0);
    //           int Num = Shove._Convert.StrToInt(GridView1.Rows[i + 1].Cells[2].Text.ToString(), 0);
    //        //    Num = Shove._Convert.StrToInt(GridView1.Rows[i].Cells[2].Text.ToString(), 0);
    //        //}
    //        //else
    //        //{
    //        //    num = Shove._Convert.StrToInt(GridView1.Rows[i + 1].Cells[2].Text.ToString(), 0);
    //        //}
    //    }
    //}
#endregion 


 private int RadioSelete()
    {
        int k = 1;

        if (RadioButton1.Checked == true)
        {

            k = 80;
        }

        if (RadioButton3.Checked == true)
        {
            k = 25;
        }

        if (RadioButton2.Checked == true)
        {
            k = 40;
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


#region 设置脚部和输出页面图片高度和显示出现次数

 private void DrawGridFooter(HtmlTextWriter output, Control ctl)
    {
        int n = RadioSelete();

        output.Write("<td width = '100px' height='50px'>");
        output.Write("出现次数</td>");

        output.Write("<td width = '100px'>");
        output.Write("&nbsp</td>");

        for (int k = 2; k < GridView1.Columns.Count; k++)
        {
            int a = 0;

            for (int m = 0; m < GridView1.Rows.Count; m++)
            {
                if (GridView1.Rows[m].Cells[k].Text == "0" || GridView1.Rows[m].Cells[k].Text == "1")
                {
                    a = a + 0;
                    num[k - 2] = a ;
                    sum[k - 2] = a * 50 / n;
                }

                if (GridView1.Rows[m].Cells[k].Text == "2" || GridView1.Rows[m].Cells[k].Text == "3")
                {
                    a = a + 1;
                    num[k - 2] = a;
                    sum[k - 2] = a * 50 / n;
                }
            }
               
            output.Write("<td align='center' valign='bottom'>");
            output.Write( num[k - 2].ToString() + "<br><img src='../image/01[1].gif' height='" + sum[k - 2].ToString() + "px' title = '" + num[k - 2].ToString() + "' width= '8px'></td>");   
        }
    }

#endregion

#region 按钮绑定

    protected void GridView1_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.SetRenderMethodDelegate(new RenderMethod(DrawGridFooter));
        }

        //if (e.Row.RowType == DataControlRowType.Header)
        //{
        //    e.Row.SetRenderMethodDelegate(new RenderMethod(DrawGridHeader));
        //}
    }

    protected void RadioButton1_CheckedChanged1(object sender, EventArgs e)
    {
        DataTable  dt = BLL.PL3_SeleteByNum(100);
        GridView1.DataSource = dt;
        GridView1.DataBind();
        GridViewbindColor();
    }

    protected void RadioButton3_CheckedChanged(object sender, EventArgs e)
    {
        DataTable dt = BLL.PL3_SeleteByNum(30);
        GridView1.DataSource = dt;
        GridView1.DataBind();
        GridViewbindColor();
    }

    protected void RadioButton2_CheckedChanged(object sender, EventArgs e)
    {
        DataTable dt = BLL.PL3_SeleteByNum(50);
        GridView1.DataSource = dt;
        GridView1.DataBind();
        GridViewbindColor();
    }

    protected void RadioButton4_CheckedChanged1(object sender, EventArgs e)
    {
        DataTable dt = BLL.PL3_SeleteByNum(20);
        GridView1.DataSource = dt;
        GridView1.DataBind();
        GridViewbindColor();
    }

    protected void RadioButton5_CheckedChanged1(object sender, EventArgs e)
    {
        DataTable dt = BLL.PL3_SeleteByNum(10);
        GridView1.DataSource = dt;
        GridView1.DataBind();
        GridViewbindColor();
    }

    #endregion 

}