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

public partial class TC22X5_22X5_WeiHao : System.Web.UI.Page
{
    int[] num=new int[22];
    int[] sum=new int[22];
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
        GridView1.DataSource = BLL.TC22X5_Weihao_SeleteByNum(30);
        GridView1.DataBind();
    }

    private void GridViewbindColor()
    {
      for(int j = 1;j < 23; j++)
      {
          int k = 0;

          for(int i = 0;i < GridView1.Rows.Count;i++)
          {
               if(GridView1.Rows[i].Cells[j].Text !="0")
              {
                 k = k + 1;
              }

              if (GridView1.Rows[i].Cells[j].Text == "0")
              {
                  GridView1.Rows[i].Cells[j].Text = "&nbsp;";
              }
          }
          num[j-1] = k;
          int a = RadioSelete();
          sum[j - 1] = 50 * k / a;
      }
    }

    #region 按钮事件
    protected void RadioButton1_CheckedChanged1(object sender, EventArgs e)
    {
        DataTable dt = BLL.TC22X5_Weihao_SeleteByNum(100);
        GridView1.DataSource = dt;
        GridView1.DataBind();
        GridViewbindColor();
    }

    protected void RadioButton2_CheckedChanged(object sender, EventArgs e)
    {
        DataTable dt = BLL.TC22X5_Weihao_SeleteByNum(50);
        GridView1.DataSource = dt;
        GridView1.DataBind();
        GridViewbindColor();
    }

    protected void RadioButton3_CheckedChanged(object sender, EventArgs e)
    {
        DataTable dt = BLL.TC22X5_Weihao_SeleteByNum(30);
        GridView1.DataSource = dt;
        GridView1.DataBind();
        GridViewbindColor();
    }

    protected void RadioButton4_CheckedChanged1(object sender, EventArgs e)
    {
        DataTable dt = BLL.TC22X5_Weihao_SeleteByNum(20);
        GridView1.DataSource = dt;
        GridView1.DataBind();
        GridViewbindColor();
    }

    protected void RadioButton5_CheckedChanged1(object sender, EventArgs e)
    {
        DataTable dt = BLL.TC22X5_Weihao_SeleteByNum(10);
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
        output.Write("<td rowspan ='2'align ='center' vlign='middle'>期数</td>");
     
        output.Write("<td colspan ='3'align ='center' vlign='middle' ><font color='#0000FF'>1 &nbsp;</font>区</td>");
        output.Write("<td colspan ='3'align ='center' vlign='middle' ><font color='#FF0000'>2 &nbsp;</font>区</td>");
        output.Write("<td colspan ='2'align ='center' vlign='middle' ><font color='#0000FF'>3 &nbsp;</font>区</td>");
        output.Write("<td colspan ='2'align ='center' vlign='middle' ><font color='#FF0000'>4 &nbsp;</font>区</td>");
        output.Write("<td colspan ='2'align ='center' vlign='middle' ><font color='#0000FF'>5 &nbsp;</font>区</td>");
        output.Write("<td colspan ='2'align ='center' vlign='middle' ><font color='#FF0000'>6 &nbsp;</font>区</td>");
        output.Write("<td colspan ='2'align ='center' vlign='middle' ><font color='#0000FF'>7 &nbsp;</font>区</td>");
        output.Write("<td colspan ='2'align ='center' vlign='middle' ><font color='#FF0000'>8 &nbsp;</font>区</td>");
        output.Write("<td colspan ='2'align ='center' vlign='middle' ><font color='#0000FF'>9 &nbsp;</font>区</td>");
        output.Write("<td colspan ='2'align ='center' vlign='middle' ><font color='#FF0000'>0 &nbsp;</font>区</td>");       
      
        output.Write("<tr align ='center' vlign='middle'>");
        output.Write("<td ><font color='#0000FF'>01</font></td>");
        output.Write("<td ><font color='#0000FF'>11</font></td>");
        output.Write("<td ><font color='#0000FF'>21</font></td>");
        output.Write("<td  bgcolor='#E7FEEB'><font color='#FF0000'>02</font></td>");
        output.Write("<td  bgcolor='#E7FEEB'><font color='#FF0000'>12</font></td>");
        output.Write("<td  bgcolor='#E7FEEB'><font color='#FF0000'>12</font></td>");
        output.Write("<td ><font color='#0000FF'>03</font></td>");
        output.Write("<td ><font color='#0000FF'>13</font></td>");
        output.Write("<td  bgcolor='#E7FEEB'><font color='#FF0000'>04</font></td>");
        output.Write("<td  bgcolor='#E7FEEB'><font color='#FF0000'>14</font></td>");
        output.Write("<td ><font color='#0000FF'>05</font></td>");
        output.Write("<td ><font color='#0000FF'>15</font></td>");
        output.Write("<td  bgcolor='#E7FEEB'><font color='#FF0000'>06</font></td>");
        output.Write("<td  bgcolor='#E7FEEB'><font color='#FF0000'>16</font></td>");
         output.Write("<td ><font color='#0000FF'>07</font></td>");
         output.Write("<td ><font color='#0000FF'>17</font></td>");
         output.Write("<td  bgcolor='#E7FEEB'><font color='#FF0000'>08</font></td>");
         output.Write("<td  bgcolor='#E7FEEB'><font color='#FF0000'>18</font></td>");
         output.Write("<td ><font color='#0000FF'>09</font></td>");
         output.Write("<td ><font color='#0000FF'>19</font></td>");
         output.Write("<td  bgcolor='#E7FEEB'><font color='#FF0000'>10</font></td>");
         output.Write("<td  bgcolor='#E7FEEB'><font color='#FF0000'>20</font></td>");
    }

    private void DrawGridFooter(HtmlTextWriter output, Control ctl)
    {
         output.Write("<td align='center' vlign='middle' height='50px'>出现次数</td>");
        for(int k = 0 ; k < 22;k++)
        {
            output.Write("<td align='center' valign='bottom'>");
            output.Write(num[k].ToString() + "<br><img src='../image/01[1].gif' height='" + sum[k].ToString() + "px' title = '" + num[k].ToString() + "' width= '8px'></td>");

        }

        output.Write("<tr align='center' valign='middle'>");
        output.Write("<td align='center' vlign='middle'>数字序号</td>");
        output.Write("<td ><font color='#0000FF'>01</font></td>");
        output.Write("<td ><font color='#0000FF'>11</font></td>");
        output.Write("<td ><font color='#0000FF'>21</font></td>");
        output.Write("<td  bgcolor='#E7FEEB'><font color='#FF0000'>02</font></td>");
        output.Write("<td  bgcolor='#E7FEEB'><font color='#FF0000'>12</font></td>");
        output.Write("<td  bgcolor='#E7FEEB'><font color='#FF0000'>12</font></td>");
        output.Write("<td ><font color='#0000FF'>03</font></td>");
        output.Write("<td ><font color='#0000FF'>13</font></td>");
        output.Write("<td  bgcolor='#E7FEEB'><font color='#FF0000'>04</font></td>");
        output.Write("<td  bgcolor='#E7FEEB'><font color='#FF0000'>14</font></td>");
        output.Write("<td ><font color='#0000FF'>05</font></td>");
        output.Write("<td ><font color='#0000FF'>15</font></td>");
        output.Write("<td  bgcolor='#E7FEEB'><font color='#FF0000'>06</font></td>");
        output.Write("<td  bgcolor='#E7FEEB'><font color='#FF0000'>16</font></td>");
        output.Write("<td ><font color='#0000FF'>07</font></td>");
        output.Write("<td ><font color='#0000FF'>17</font></td>");
        output.Write("<td  bgcolor='#E7FEEB'><font color='#FF0000'>08</font></td>");
        output.Write("<td  bgcolor='#E7FEEB'><font color='#FF0000'>18</font></td>");
        output.Write("<td ><font color='#0000FF'>09</font></td>");
        output.Write("<td ><font color='#0000FF'>19</font></td>");
        output.Write("<td  bgcolor='#E7FEEB'><font color='#FF0000'>10</font></td>");
        output.Write("<td  bgcolor='#E7FEEB'><font color='#FF0000'>20</font></td>");

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


