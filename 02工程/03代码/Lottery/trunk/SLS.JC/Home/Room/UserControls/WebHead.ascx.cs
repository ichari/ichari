using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;

public partial class Home_Room_UserControls_WebHead : System.Web.UI.UserControl
{
    public string Name = "";
    protected System.Text.StringBuilder sbWin = new StringBuilder();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            Users _User = Users.GetCurrentUser(1);

            if (_User != null)
            {
                head_HidUserID.Value = _User.ID.ToString();
                Name = _User.Name;
            }
            BindWin();
        }
    }


    private void BindWin()
    {
        // ** 注: 此处缓存与CelebrityHall命名一致
        DataTable dt = Shove._Web.Cache.GetCacheAsDataTable("DataCache_CelebrityHall_Hero");

        if (dt == null)
        {

            string sql = "select  top 21 row_number() over (order by (b.WinMoneyNoWithTax) desc) as Ranking, u.ID as UserID,l.ID as LotteryID,s.ID as SchemeID, u.Name as UserName,l.Name as Name,i.Name as IsuseName ,s.IsuseID, b.WinMoneyNoWithTax as [Money]"
                + "from T_BuyDetails b "
                + "inner join T_Users u on b.UserID = u.ID "
                + "inner join T_Schemes s on b.SchemeID = s.ID "
                + "inner join T_Isuses i on s.IsuseID = i.ID "
                + "inner join T_Lotteries l on i.LotteryID = l.ID "
                + "where b.[WinMoneyNoWithTax] > 100"
                + "order by  s.DateTime desc";

            dt = Shove.Database.MSSQL.Select(sql);

            if (dt == null)
            {
                PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试(130)", this.GetType().BaseType.FullName);

                return;
            }

            if (dt.Rows.Count > 0)
            {
                // 1200M
                Shove._Web.Cache.SetCache("DataCache_CelebrityHall_Hero", dt, 1200);
            }
        }

        // 拼接html
        int indexCount = 0;
        System.Text.StringBuilder sb = new StringBuilder();
        foreach (DataRow dr in dt.Rows)
        {
            // modified 3/31/12 CTS to reflect the fix on scro_top section
            if (indexCount % 3 == 0 && indexCount != 0)
            {
                sb.Append("</ul>");
            }

            if (indexCount % 3 == 0)
            {
                sb.Append("<ul><div style=\"width:33px;height:28px;float:left;padding-top:7px;\"><img src=\"/Images/laba.gif\"></div>");
            }
            try
            {
                sb.Append("<li><a href=\"/Home/Room/Scheme.aspx?id=" + dr["SchemeID"].ToString() + "\" target=\"_blank\"><span class=\"blue\">" + dr["UserName"].ToString() + "</span> 喜中" + dr["Name"].ToString() + " <span style=\"color:red\">" + dr["Money"].ToString().Split('.')[0].ToString() + "</span>元</a> </li>");
            }
            catch { }

            indexCount++;
        }

        sbWin = sb;
    }
}
