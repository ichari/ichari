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

public partial class View_PL3_LH : TrendChartPageBase
{
    public int[] sum = new int[3];
    public string [] tr = new string[3];
    public int tem = 0;
    public int temp = 0;
    public  int qq = 0;
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
        GridView1.DataSource = BLL.PL3_LH_SeleteByNum(30);
        GridView1.DataBind();
    }

    private void GridViewbindColor()
    {
        tem = GridView1.Rows.Count;

        for (int i = 3; i < 6; i++)
        {
            int num = 0;
            for (int j = 0;j< GridView1.Rows.Count; j++)
            {
                if (GridView1.Rows[j].Cells[i].Text == "0")
                {
                    num = num + 1;
                }                    
            }
            sum[i - 3] = num;

        }

        float a = sum[0] + sum[1] + sum[2];
        float b =(sum[0] / a) * 100;
        float c =(sum[1] / a) * 100;
        float d = (sum[2] / a) * 100;
        string x = b.ToString();
        string y = c.ToString();
        string z = d.ToString();
        string[] str ={x,y,z};

        for (int k = 0; k < 3; k++)
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
        
        //tr[0] = (Shove._String.Cut(x,2)).Substring(0,4)+ "%";
        //tr[1] = (Shove._String.Cut(y, 2)).Substring(0,4) + "%";
        //tr[2] = (Shove._String.Cut(z, 2)).Substring(0,4) + "%";
    
      
        for (int i = 0; i< GridView1.Rows.Count; i++)
        {
            if (GridView1.Rows[i].Cells[2].Text == "0")
            {
                GridView1.Rows[i].Cells[2].Text = "&nbsp";
            }

            for (int j = 3; j < 6; j++)
            {
              
                if (GridView1.Rows[i].Cells[j].Text == "-1")
                {
                    GridView1.Rows[i].Cells[j].Text = "&nbsp";
                }
                if (GridView1.Rows[i].Cells[j].Text == "0")
                {
                
                    GridView1.Rows[i].Cells[j].Text = " ●";
                    //GridView1.Rows[i].Cells[j].Style.Add(HtmlTextWriterStyle.Color, "#CC0000");
                    GridView1.Rows[i].Cells[j].ForeColor = System.Drawing.Color.FromArgb(204, 000, 000);
                }             
            }

            for (int k = 6; k <= 7; k++)
            {
                if (GridView1.Rows[i].Cells[k].Text == "降")
                {
                    temp = temp + 1;
                    GridView1.Rows[i].Cells[k].ForeColor = System.Drawing.Color.FromArgb(000,000,255);
                }
                if (GridView1.Rows[i].Cells[k].Text == "升")
                {
                    qq = qq + 1;
                    GridView1.Rows[i].Cells[k].ForeColor = System.Drawing.Color.FromArgb(255, 000, 000);
                }
            }           
        }

        float m = qq + temp;
        float l = (temp / m )*100;
        float g = (qq / m) * 100;
        string ll=l.ToString();
        string gg=g.ToString();
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
                    bbb[k] = hh[k].Substring(0, 4) + "%";
                }
                else
                {
                    bbb[k] = hh[k].Substring(0, 5) + "%";
                }
            }
        }
    }

    #region 按钮事件
    protected void RadioButton1_CheckedChanged1(object sender, EventArgs e)
    {
        DataTable dt = BLL.PL3_LH_SeleteByNum(100);
        GridView1.DataSource = dt;
        GridView1.DataBind();
        GridViewbindColor();
        //BindNumber();
    }

    protected void RadioButton2_CheckedChanged(object sender, EventArgs e)
    {
        DataTable dt = BLL.PL3_LH_SeleteByNum(50);
        GridView1.DataSource = dt;
        GridView1.DataBind();
        GridViewbindColor();
        //BindNumber();
    }

    protected void RadioButton3_CheckedChanged(object sender, EventArgs e)
    {
        DataTable dt = BLL.PL3_LH_SeleteByNum(30);
        GridView1.DataSource = dt;
        GridView1.DataBind();
        GridViewbindColor();
        //BindNumber();
    }

    protected void RadioButton4_CheckedChanged1(object sender, EventArgs e)
    {
        DataTable dt = BLL.PL3_LH_SeleteByNum(20);
        GridView1.DataSource = dt;
        GridView1.DataBind();
        GridViewbindColor();
        //BindNumber();
    }

    protected void RadioButton5_CheckedChanged1(object sender, EventArgs e)
    {
        DataTable dt = BLL.PL3_LH_SeleteByNum(10);
        GridView1.DataSource = dt;
        GridView1.DataBind();
        GridViewbindColor();
        //BindNumber();
    }

    #endregion

    //public void BindNumber()
    //{
       
    //    for (int j = 3; j < 6; j++)
    //    {
    //        int ss = 0;

    //        for (int i = 0; i < GridView1.Rows.Count; i++)
    //        {
    //            //if (GridView1.Rows[i].Cells[j].Text == "●" && GridView1.Rows[i].Cells[j].ForeColor == System.Drawing.Color.FromArgb(204,000,000))
    //            //if(GridView1.Rows[i].Cells[j].Text !="0")
    //            if (GridView1.Rows[i].Cells[j].Text != null)
    //            {
    //                ss = ss + 1;
    //            }                
    //        }
    //        sum[j-3] = ss;
    //    }
       

    //    //float a = sum[0] + sum[1] + sum[2];
    //    //float b = (sum[0] / a) * 100;
    //    //float c = (sum[1] / a) * 100;
    //    //float d = (sum[2] / a) * 100;
    //    //tr[0] = b.ToString() + "%";
    //    //tr[1] = c.ToString() + "%";
    //    //tr[2] = d.ToString() + "%";
    //}

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
        output.Write("<td rowspan ='2'align ='center' vlign='middle'>开奖号码</td>");
        output.Write("<td rowspan ='2'align ='center' vlign='middle' ><strong><font color='#FF0000'>");
        output.Write("连号数</font></strong></td>");
        output.Write("<TD align ='center' vlign='middle'colspan ='3'bgcolor='#99CC99'><strong><font color='#FF0000'>连号个数</font></strong></td>");
        output.Write("<td align ='center' vlign='middle'colspan ='2' bgcolor='#FFCC99'><strong><font color='#FF0000'>排序</font></strong></td>");

        output.Write("<tr align ='center' vlign='middle'>");
        output.Write("<td align ='center' vlign='middle' bgcolor='#DCEDDC'>0</td>");
        output.Write("<td align ='center' vlign='middle' bgcolor='#DFD0DF'>1</td>");
        output.Write("<td align ='center' vlign='middle' bgcolor='#DFDFFF'>2</td>");
        output.Write("<td align ='center' vlign='middle' bgcolor='#D9F5FF'>1-2</td>");
        output.Write("<td align ='center' vlign='middle' bgcolor='#F1F7D2'>2-3</td>");


    }

    private void DrawGridFooter(HtmlTextWriter output, Control ctl)
    {
        int[] aa = new int[5];
        int tem =GridView1.Rows.Count;
        int num = Shove._Convert.StrToInt(GridView1.Rows[tem - 1].Cells[0].Text, 0);
        aa[0] = num + 1;
        aa[1] = aa[0] + 1;
        aa[2] = aa[1] + 1;
        aa[3] = aa[2] + 1;
        aa[4] = aa[3] + 1;

          output.Write("<td align='center' vlign='middle'>");
          output.Write(aa[0].ToString()+"</td>");
          output.Write("<td rowspan ='5'>预测方法：用鼠标<br>点击方格即可显示<br>为开奖号色</td>");
          output.Write("<td>&nbsp;");

        for (int i = 0; i < 3; i++)
        {
            output.Write("<td  style='color:#FFFFFF;' bgcolor='#FFFFFF' onClick=Style(this,'#FFFFFF','#FFFFFF')>●</td>");
        }
        for (int k = 0; k < 2; k++)
        {
            output.Write("<td>&nbsp;");
        }


        for (int j = 0; j < 4; j++)
        {
            output.Write("<tr align='center' vlign='middle'>");
            output.Write("<td align='center' vlign='middle'>");
            output.Write(aa[j+1].ToString() + "</td>");
            output.Write("<td>&nbsp;");

            for (int i = 0; i < 3; i++)
            {
                output.Write("<td  style='color:#FFFFFF;' bgcolor='#FFFFFF' onClick=Style(this,'#FFFFFF','#FFFFFF')>●</td>");
            }
            for (int k = 0; k < 2; k++)
            {
                output.Write("<td>&nbsp;");
            }

        }

        //最后一行
        output.Write("<tr align='center' vlign='middle'>");
        output.Write("<td align='center' vlign='middle'>期数</td>");
        output.Write("<td align='center' vlign='middle'>排列3开奖号码</td>");
        output.Write("<td align='center' vlign='middle'><strong><font color='FF0000'> 连号数</font></strong></td>");
        output.Write("<td bgcolor='#DCEDDC'>");
        output.Write("0</td>");
        output.Write("<td bgcolor='#DFD0DF'>1</td>");
        output.Write("<td bgcolor='#DFDFFF'>2</td>");
        output.Write("<td  bgcolor='#D9F5FF'>1-2</td>");
        output.Write("<td  bgcolor='#F1F7D2'>2-3</td>");

    }

   
}
