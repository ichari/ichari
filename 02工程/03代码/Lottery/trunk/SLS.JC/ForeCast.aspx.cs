using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;

public partial class ForeCast : SitePageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            tdjczq.InnerHtml = BindNewsForLottery(72, "竞彩足球", "zq.png");

            tdjclq.InnerHtml = BindNewsForLottery(73, "竞彩篮球", "lq.png");

            tdCJDLT.InnerHtml = BindNewsForLottery(39, "超级大乐透", "dlt.jpg");

            tdPL.InnerHtml = BindNewsForLottery(63, "排列3/5", "pl.jpg");

            tdqxc.InnerHtml = BindNewsForLottery(3, "七星彩", "qxc.jpg");

            td22x5.InnerHtml = BindNewsForLottery(9, "22选5", "22x5.jpg");

            td1.InnerHtml = BindNewsForLottery(0, "足彩资讯", "zc.gif");

            td2.InnerHtml = BindNewsForLottery(0, "欧冠资讯", "og.jpg");

            td3.InnerHtml = BindNewsForLottery(0, "英超资讯", "yc.jpg");

            td4.InnerHtml = BindNewsForLottery(0, "西甲资讯", "xj.jpg");

            td5.InnerHtml = BindNewsForLottery(0, "意甲资讯", "yj.jpg");

            td6.InnerHtml = BindNewsForLottery(0, "德甲资讯", "dj.jpg");
        }
    }

    private string BindNewsForLottery(int LotteryID, string TypeName,string imageName)
    {
        string Key = "ForeCast_BindNewsForLottery" + TypeName;

        DataTable dt = Shove._Web.Cache.GetCacheAsDataTable(Key);

        if (dt == null)
        {
            string sql = "";

            sql = "select Top 12 a.ID, Title,Content,DateTime,b.Name as TypeName from (select ID,Title,Content,TypeID,[DateTime],isCommend from dbo.T_News where isShow = 1 ) a "
                   + "inner join dbo.T_NewsTypes b "
                   + "on a.TypeID = b.ID "
                   + "where b.Name = @Name "
                   + "order by isCommend,DateTime desc";
            dt = Shove.Database.MSSQL.Select(sql,
                new Shove.Database.MSSQL.Parameter("Name", SqlDbType.VarChar, 0, ParameterDirection.Input, TypeName));

            if (dt == null || dt.Rows.Count == 0)
            {
                return "";
            }

            Shove._Web.Cache.SetCache("ForeCast_BindNewsForLottery", dt, 1200);
        }

        StringBuilder sb = new StringBuilder();

        sb.Append("<table width='95%' border='0' cellspacing='0' cellpadding='0' style='margin-top: 5px;margin-bottom: 5px;' valign='top'>");

        string Title = "";
        string color = "";
        int i = 0;
        foreach (DataRow dr in dt.Rows)
        {
            i++;

            Title = dr["Title"].ToString().Trim();

            if (i == 1)
            { // 存放大图片

                if (Title.Contains("<font class=red12>"))
                {
                    color = "red12";
                }
                if (Title.Contains("<font class=black12>"))
                {
                    color = "black12";
                }

                Title = Title.Replace("<font class=red12>", "").Replace("<font class=black12>", "").Replace("</font>", "");

                sb.Append("<tr><td colspan=\"2\">")
                  .Append("<table border=\"0\" width=\"305px\" style=\"text-align:left;\">")
                  .Append("<tr><td rowspan=\"2\" width=\"95\"><img src=\"Images/ForeCast/" + imageName + " \" width=\"89\" height=\"84\" /></td>")
                  .Append("<td style=\"padding-top:2px\" height='24'><b><a href=\"");
                #region
                // 效验 内容与地执行
                Regex regex = new Regex(@"^http://([\w-]+\.)+[\w-]+(/[\w-./?%&=]*)?$", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                Match m = regex.Match(dr["Content"].ToString());

                if (m.Success)
                {
                    sb.Append(dr["Content"].ToString());
                }
                else
                {
                    sb.Append("../Home/Web/ShowNews.aspx?ID=" + dr["ID"].ToString());
                }
                #endregion
                sb.Append("\"  target='_blank'><font class = '" + color + "'>" + Shove._String.Cut(Title, 14) + "</font></a></b></td></tr>");
                //&ldquo;
                string content = DelHTML(dr["Content"].ToString().Replace("&ldquo;", "“").Replace("&rdquo;", "”"));

                sb.Append("<tr><td  valign=\"top\" ><div style=\"color:gray; width:212px;table-layout:fixed;word-break:break-all;word-wrap:break-word;\">&nbsp;&nbsp;&nbsp;&nbsp;" + Shove._String.Cut(content, 40) + "</div></td>")
                  .Append("</tr></table></td></tr>");
            }
            #region
            else if (i <= 12)
            {
                if ((Title.IndexOf("<font class=red12>") > -1 || Title.IndexOf("<font class=black12>") > -1))
                {
                    if (Title.Contains("<font class=red12>"))
                    {
                        color = "red12";
                    }
                    if (Title.Contains("<font class=black12>"))
                    {
                        color = "black12";
                    }

                    Title = Title.Replace("<font class=red12>", "").Replace("<font class=black12>", "").Replace("</font>", "");

                    sb.Append("<tr>")
                        .Append("<td width='6%' height='24'>")
                        .Append("<img src=\"Home/Room/images/icon_jiantou_2.gif\" width=\"9\" height=\"9\" />")
                        .Append("</td>")
                        .Append("<td height='24' align='left' class='black12'>")
                        .Append("<table><tr><td>")
                        .Append("<a href='");
                    #region
                    Regex regex = new Regex(@"^http://([\w-]+\.)+[\w-]+(/[\w-./?%&=]*)?$", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                    Match m = regex.Match(dr["Content"].ToString());

                    if (m.Success)
                    {
                        sb.Append(dr["Content"].ToString());
                    }
                    else
                    {
                        sb.Append("../Home/Web/ShowNews.aspx?ID=" + dr["ID"].ToString());
                    }
                    #endregion
                    sb.Append("' target='_blank'><font class = '" + color + "'>")

                         .Append(Shove._String.Cut(Title, 19))
                         .Append("</font>")
                        .Append("</a></td><td>")
                        .Append(Shove._Convert.StrToDateTime(dr["DateTime"].ToString(), DateTime.Now.Date.ToString()).ToString("MM-dd"))
                        .Append("</td></tr></table></td></tr>");
                }
            #endregion
                else
                {
                    sb.Append("<tr>")
                       .Append("<td width='6%' height='24'>")
                       .Append("<img src=\"Home/Room/images/icon_jiantou_2.gif\" width=\"9\" height=\"9\" />")
                       .Append("</td>")
                       .Append("<td height='24' align='left' class='black12'>")
                        .Append("<table border='0'><tr><td width='253px'>")
                        .Append("<a href='");
                    #region 
                    Regex regex = new Regex(@"^http://([\w-]+\.)+[\w-]+(/[\w-./?%&=]*)?$", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                    Match m = regex.Match(dr["Content"].ToString());

                    if (m.Success)
                    {
                        sb.Append(dr["Content"].ToString());
                    }
                    else
                    {
                        sb.Append("../Home/Web/ShowNews.aspx?ID=" + dr["ID"].ToString());
                    }
                    #endregion
                    sb.Append("' target='_blank'>" + Shove._String.Cut(Title, 19))
                      .Append("</a></td><td>")
                      .Append(Shove._Convert.StrToDateTime(dr["DateTime"].ToString(), DateTime.Now.Date.ToString()).ToString("MM-dd"))
                      .Append("</td></tr></table></td></tr>");
                }
            }
            else
            {
                break;
            }
        }

        sb.Append("</table>");

        return sb.ToString();
    }


    #region Tool

    /// <summary>
    /// 将HTML去除  
    /// </summary>
    public string DelHTML(string Htmlstring)//将HTML去除  
    {
        #region
        //删除脚本 
        Htmlstring = System.Text.RegularExpressions.Regex.Replace(Htmlstring, @"<script[^>]*?>.*?</script>", "", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        //删除HTML 
        Htmlstring = System.Text.RegularExpressions.Regex.Replace(Htmlstring, @"<(.[^>]*)>", "", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        Htmlstring = System.Text.RegularExpressions.Regex.Replace(Htmlstring, @"([\r\n])[\s]+", "", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        Htmlstring = System.Text.RegularExpressions.Regex.Replace(Htmlstring, @"-->", "", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        Htmlstring = System.Text.RegularExpressions.Regex.Replace(Htmlstring, @"<!--.*", "", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        Htmlstring = System.Text.RegularExpressions.Regex.Replace(Htmlstring, @"<A>.*</A>", "");
        Htmlstring = System.Text.RegularExpressions.Regex.Replace(Htmlstring, @"<[a-zA-Z]*=\.[a-zA-Z]*\?[a-zA-Z]+=\d&\w=%[a-zA-Z]*|", "");
        Htmlstring = System.Text.RegularExpressions.Regex.Replace(Htmlstring, @"&(quot|#34);", "\"", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        Htmlstring = System.Text.RegularExpressions.Regex.Replace(Htmlstring, @"&(amp|#38);", "&", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        Htmlstring = System.Text.RegularExpressions.Regex.Replace(Htmlstring, @"&(lt|#60);", "<", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        Htmlstring = System.Text.RegularExpressions.Regex.Replace(Htmlstring, @"&(gt|#62);", ">", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        Htmlstring = System.Text.RegularExpressions.Regex.Replace(Htmlstring, @"&(nbsp|#160);", " ", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        Htmlstring = System.Text.RegularExpressions.Regex.Replace(Htmlstring, @"&(iexcl|#161);", "\xa1", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        Htmlstring = System.Text.RegularExpressions.Regex.Replace(Htmlstring, @"&(cent|#162);", "\xa2", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        Htmlstring = System.Text.RegularExpressions.Regex.Replace(Htmlstring, @"&(pound|#163);", "\xa3", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        Htmlstring = System.Text.RegularExpressions.Regex.Replace(Htmlstring, @"&(copy|#169);", "\xa9", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        Htmlstring = System.Text.RegularExpressions.Regex.Replace(Htmlstring, @"&#(\d+);", "", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        Htmlstring.Replace("<", "");
        Htmlstring.Replace(">", "");
        Htmlstring.Replace("\r\n", "");
        Htmlstring = HttpContext.Current.Server.HtmlEncode(Htmlstring).Trim();
        #endregion
        return Htmlstring;
    }

    #endregion
}
