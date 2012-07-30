using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;
using Shove.Database;

public partial class Home_Room_SYYDJ_WinNotice : System.Web.UI.Page
{
    public int width = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            GetWinNotice();
        }
    }

    private void GetWinNotice()
    {
        string key = "GetWinNoticeByID62";

        DataTable dt = Shove._Web.Cache.GetCacheAsDataTable(key);

        StringBuilder sb = new StringBuilder();
        
        if (dt == null)
        {
            sb.AppendLine(@"select a.ID, WinMoney, b.Name as PlayTypeName,c.Name as InitiateName from                            
(select top 12 ID,InitiateUserID,PlayTypeID,WinMoney
from T_Schemes a where WinMoney >0 and PlayTypeID in(select ID from T_PlayTypes b where LotteryID = 62) order by ID desc)a
inner join T_PlayTypes b on a.PlayTypeID = b.ID inner join T_Users c on a.InitiateUserID = c.ID");
            dt = MSSQL.Select(sb.ToString());

            if (dt == null)
            {
                PF.GoError(ErrorNumber.DataReadWrite, "数据读写错错误", this.GetType().BaseType.FullName);

                return;
            }

            Shove._Web.Cache.SetCache(key,dt, 1800);
        }

        sb = new StringBuilder();

        foreach (DataRow dr in dt.Rows)
        {
            sb.Append("&nbsp;&nbsp;&nbsp;<a target=\"_blank\" href=\"Scheme.aspx?id=").Append(dr["ID"].ToString()).Append("\">")
                .Append("<span class=\"hui12\">恭喜&nbsp;<span class =\"red12_2\">")
                .Append(Shove._String.Cut(dr["InitiateName"].ToString(), 4))
                .Append("</span>&nbsp;喜中")
                .Append("&nbsp;" + dr["PlayTypeName"].ToString())
                .Append("&nbsp奖金&nbsp</span><span class =\"red12_2\">")
                .Append(Shove._Convert.StrToDouble(dr["WinMoney"].ToString(), 0).ToString("N"))
                .Append("&nbsp</span><span class=\"hui12\">元</span></a>");
        }

        if (sb.ToString() == "")
        {
            sb.Append("<span class=\"hui12\">暂无中奖信息</span>");
        }

        width = sb.Length * 2 ;
        divWinNotice.InnerHtml = sb.ToString();

    }
}
