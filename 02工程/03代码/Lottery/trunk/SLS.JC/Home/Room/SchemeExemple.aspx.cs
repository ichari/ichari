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

public partial class Home_Room_SchemeExemple : RoomPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        int id = Shove._Convert.StrToInt(Shove._Web.Utility.GetRequest("id"), -1);

        if (id < 0)
        {
            PF.GoError(ErrorNumber.Unknow, "参数错误", "Room_SchemeExemple");

            return;
        }

        if (!IsPostBack)
        {
            BindData(id);
        }
    }

    #region Web 窗体设计器生成的代码

    override protected void OnInit(EventArgs e)
    {
        isRequestLogin = false;
        isAtFramePageLogin = false;

        base.OnInit(e);
    }

    #endregion

    private void BindData(int id)
    {
        DataTable dt = new DAL.Tables.T_Lotteries().Open("[ID],[Name],[SchemeExemple]", "[ID] = " + id.ToString() + " and [ID] in (" + (_Site.UseLotteryList == "" ? "-1" : _Site.UseLotteryList) + ")", "");

        if (dt == null)
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", "Room_SchemeExemple");

            return;
        }

        if (dt.Rows.Count == 0)
        {
            Shove._Web.JavaScript.ClosePage(this.Page);

            return;
        }
        
        labContent.Text = dt.Rows[0]["SchemeExemple"].ToString().Replace("[SiteName]", _Site.Name).Replace("[SiteUrl]", _Site.Urls).Replace("[siteurl]", _Site.Urls);
    }
}
