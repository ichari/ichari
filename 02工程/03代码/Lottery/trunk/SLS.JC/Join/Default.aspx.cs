using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.Web.UI.HtmlControls;

public partial class Join_Default : System.Web.UI.Page
{
    public string innerHTML = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        ((HtmlInputHidden)WebHead1.FindControl("currentMenu")).Value = "mJoin";
        if (!this.IsPostBack)
        {
            BindCollect();
        }
    }

    /// <summary>
    /// 绑定名人合买
    /// </summary>
    private void BindCollect()
    {
        DataTable dt = Shove._Web.Cache.GetCacheAsDataTable("DataCache_CelebrityHall_Collects");

        if (dt == null)
        {
            dt = Shove.Database.MSSQL.Select("select s.ID,u.ID as UserID,s.ID as SchemeID,l.ID as LotteryID ,s.[Money],s.Share,s.BuyedShare,i.Name,l.Name as LotteryName,u.Name as UserName,[WinMoney]=(select SUM(WinMoneyNoWithTax) from T_Schemes where InitiateUserID = s.InitiateUserID and WinMoney > 0 and DateTime > dateAdd(mm,-1,getdate())) from T_Schemes as s ,T_Isuses as i,T_PlayTypes as p,T_Lotteries as l,T_Users as u where s.Share > s.BuyedShare and s.QuashStatus = 0 and s.PlayTypeID = p.ID  and s.IsuseID = i.ID and l.ID = p.LotteryID and s.InitiateUserID in (select UserID from T_Personages group by UserID) and u.ID in (select UserID from T_Personages group by UserID) and u.ID = s.initiateUserID");

            if (dt == null)
            {
                PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试(131)", this.GetType().BaseType.FullName);

                return;
            }

            if (dt.Rows.Count > 0)
            {
                // 1200M
                Shove._Web.Cache.SetCache("DataCache_CelebrityHall_Collects", dt, 1200);
            }
        }

        if (dt == null)
        {
            return;
        }

        if (dt.Rows.Count < 1)
        {
            return;
        }

        // 区分UserID
        string UserIDState = "";

        StringBuilder sb = new StringBuilder();

        sb.AppendLine("<div class=\"mrhm_list\" id=\"_mr_info\"><div class=\"LeftBotton\" id=\"LeftArr\"></div><div></div><div class=\"Cont\" id=\"ISL_Cont_1\"><ul>");

        int index = 0;
        bool state = false;

        foreach (DataRow dr in dt.Rows)
        {
            if (!UserIDState.Equals(dr["UserID"].ToString()))
            {
                index = 0;
                if (state)
                {
                    sb.AppendLine("</div></li>");
                }
                else
                {
                    state = true;
                }

                sb.AppendLine("<li><div class=\"mr_info\"><h2><img src=\"images/icon.gif\" width=\"58\" height=\"58\" /></h2>")
                  .AppendLine("<dl><h3><a href=\"../Home/Web/Score.aspx?id=" + dr["UserID"] + "&LotteryID=" + dr["LotteryID"] + " \" target=\"_blank\">" + dr["UserName"] + "</a></h3><dt>")
                  .AppendLine("<span></span>近期中奖 <em>" + Shove._Convert.StrToDouble(dr["WinMoney"].ToString(), 0).ToString("F2") + "</em>元</dt><dt></dt></dl>")
                  .AppendLine("<div class=\"mr_fangan_tit\"><dl><a href=\"../Home/Web/Score.aspx?id=" + dr["UserID"] + "&LotteryID=" + dr["LotteryID"] + " \" target=\"_blank\">更多&gt;&gt;</a></dl><h3>他的方案</h3></div>")
                  .AppendLine("<div class=\"mr_fangan_list\">");

                // 计算进度
                double sacle = Shove._Convert.StrToDouble(dr["BuyedShare"].ToString(), 0) / Shove._Convert.StrToDouble(dr["Share"].ToString(), 0) * 100;

                sb.AppendLine("<dl style=\"width:210px\" ><span><a href=\"../Home/Room/Scheme.aspx?id=" + dr["SchemeID"] + "\" target=\"_blank\">抢购</a></span><a href=\"../Home/Room/Scheme.aspx?id=" + dr["SchemeID"] + "\" target=\"_blank\">" + dr["LotteryName"] + "" + dr["Name"] + "期</a> <em>￥" + Shove._Convert.StrToDouble(dr["Money"].ToString(), 0).ToString("F2") + "</em> 进度<em>" + sacle.ToString("F2") + "%</em></dl>");

            }
            else
            {
                if (index < 2)
                {
                    double sacle = Shove._Convert.StrToDouble(dr["BuyedShare"].ToString(), 0) / Shove._Convert.StrToDouble(dr["Share"].ToString(), 0) * 100;

                    sb.AppendLine("<dl style=\"width:210px\" ><span><a href=\"../Home/Room/Scheme.aspx?id=" + dr["SchemeID"] + "\" target=\"_blank\" >抢购</a></span><a href=\"../Home/Room/Scheme.aspx?id=" + dr["SchemeID"] + "\" target=\"_blank\">" + dr["LotteryName"] + "" + dr["Name"] + "期</a> <em>￥" + Shove._Convert.StrToDouble(dr["Money"].ToString(), 0).ToString("F2") + "</em> 进度<em>" + sacle.ToString("F1") + "%</em></dl>");

                }
                index++;
            }

            UserIDState = dr["UserID"].ToString();


        }
        sb.AppendLine("</div></div></li>");
        sb.AppendLine("</ul></div><div class=\"RightBotton\" id=\"RightArr\"></div></div>");
        innerHTML = sb.ToString();
    }
}
