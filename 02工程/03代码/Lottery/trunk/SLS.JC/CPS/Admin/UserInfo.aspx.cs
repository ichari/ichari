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

public partial class Cps_Admin_UserInfo : CpsPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            long UserID = Shove._Convert.StrToLong(Shove._Web.Utility.GetRequest("id"), -1);

            if (UserID < 1)
            {
                PF.GoError(ErrorNumber.Unknow, "参数错误", this.GetType().BaseType.FullName);

                return;
            }

            Users t_User = new Users(_Site.ID);
            t_User.ID = UserID;

            tbRealityName.Text = t_User.RealityName;
            tbPhone.Text = t_User.Telephone;
            tbMobile.Text = t_User.Mobile;
            tbQQNum.Text = t_User.QQ;
            tbEmail.Text = t_User.Email;
            tbAddress.Text = t_User.Address;
        }
    }

    #region Web 窗体设计器生成的代码

    override protected void OnInit(EventArgs e)
    {
        isCpsRequestLogin = true;
        RequestLoginPage = Shove._Web.Utility.GetUrl() + "/CPS/Admin/Default.aspx?SubPage=UserInfo.aspx";

        base.OnInit(e);
    }

    #endregion
}
