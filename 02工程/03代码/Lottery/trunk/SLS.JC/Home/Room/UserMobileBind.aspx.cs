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

public partial class Home_Room_UserMobileBind : RoomPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
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
        labName.Text = _User.Name;
        labUserType.Text = ((_User.UserType == 1) ? "普通用户" : ((_User.UserType == 3) ? "VIP" : "高级用户"));
        labLevel.Text = _User.Level.ToString();

        if (_User.RealityName != "")
        {
            this.tbRealityName.Text = "*".PadLeft(_User.RealityName.Length - 1, '*') + _User.RealityName.Substring(_User.RealityName.Length - 1);
        }
        else
        {
            this.tbRealityName.Text = "";
        }

        try
        {
            if (_User.isMobileValided)
            {
                tbMobile.Text = _User.Mobile.Substring(0, 3) + "*****" + _User.Mobile.Substring(8, 3);
            }
            else
            {
                tbMobile.Text = "";
            }
        }
        catch
        {
            tbMobile.Text = "";
        }
        labIsMobileVailded.Text = (_User.isMobileValided ? "<font color='red'>已绑定成功</font>" : "未绑定");

        btnBind.Enabled = !_User.isMobileValided;
        btnReBind.Enabled = _User.isMobileValided;
    }

    protected void btnBind_Click(object sender, EventArgs e)
    {
        this.Response.Redirect("MobileReg.aspx", true);
    }
}
