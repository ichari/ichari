using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Home_Room_Distill : RoomPageBase
{
    public string IsCps = "0";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            IsCps = Shove._Web.Utility.GetRequest("IsCps");

            if (string.IsNullOrEmpty(IsCps))
            {
                IsCps = "0";
            }

            if (IsCps == "1")
            {
                UserMyIcaile1.CurrentPage = "pmRedeem";
                tdDistill.InnerHtml = "我的推广佣金";
                imgDistill.Src = "images/icon_13.gif";
            }
        }
    }

    #region Web 窗体设计器生成的代码

    override protected void OnInit(EventArgs e)
    {
        RequestLoginPage = this.Request.Url.AbsoluteUri;

        base.OnInit(e);
    }

    #endregion
}
