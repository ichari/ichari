using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

public partial class Home_Room_ViewAccount : RoomPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            BindData();
        }
    }

    #region Web 窗体设计器生成的代码

    override protected void OnInit(EventArgs e)
    {
        RequestLoginPage = this.Request.Url.AbsoluteUri;

        base.OnInit(e);
    }

    #endregion

    private void BindData()
    {
        labUserType.Text = ((_User.UserType == 1) ? "普通用户" : ((_User.UserType == 3) ? "VIP" : "高级用户"));
        labBalance.Text = (_User.Balance + _User.Freeze).ToString("N") ;
        labFreeze.Text = _User.Freeze.ToString("N");
        labBalanceDo.Text = _User.Balance.ToString("N");
        labScoring.Text = _User.Scoring.ToString();

        if (_User.Freeze > 0)
        {
            lbFreezDetail.Visible = true;
        }
        else
        {
            lbFreezDetail.Visible = false;
        }       
    }
}