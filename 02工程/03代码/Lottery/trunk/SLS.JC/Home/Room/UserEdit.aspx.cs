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
using System.Text;

public partial class Home_Room_UserEdit : RoomPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        AjaxPro.Utility.RegisterTypeForAjax(typeof(Home_Room_UserEdit), this.Page);
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
        //if (string.IsNullOrEmpty(_User.SecurityAnswer))
        //{
        //    Response.Write("<script type='text/javascript'>alert('为了您的安全，请先设置安全问题。');window.location='/Home/Room/SafeSet.aspx?FromUrl=UserEdit.aspx';</script>");
        //}
        lbUserName.Text = "*".PadLeft(_User.Name.Length - 1, '*') + _User.Name.Substring(_User.Name.Length - 1);
        if (_User.RealityName != "")
        {
            tbRealityName.Enabled = false;
            tbRealityName.Text = _User.RealityName;
            lbIsRealityNameValided.Text = "*已绑定";
            lbIsRealityNameValided.CssClass = "red12";
        }
        else
        {
            tbRealityName.Enabled = true;
            tbRealityName.Text = "";
            lbIsRealityNameValided.Text = "*未绑定";
        }
        lbQuestion.Text = string.IsNullOrEmpty(_User.SecurityQuestion) ? "*未设定" : _User.SecurityQuestion;
        lbSecAns.Text = "".PadLeft(_User.SecurityAnswer.Length, '*');
        ddlCity.City_id = _User.CityID;
        rbSexM.Checked = (_User.Sex == "男");
        rbSexW.Checked = (_User.Sex != "男");
        tbBirthday.Text = _User.BirthDay.ToString("yyyy-MM-dd");
        tbAddress.Text = _User.Address;
        tbEmail.Text = _User.Email;
        tbMobile.Text = _User.Mobile;
        lbIsEmailValided.Text = _User.isEmailValided ? "<span class=\"red12\">*已激活</span>" : "*未激活";
        //labIsMobileVailded.Text = (_User.isMobileValided ? "<font color='red'>已绑定</font>" : "未绑定") + "&nbsp;&nbsp;<a href='UserMobileBind.aspx'>申请绑定或修改绑定</a>";

        //DataTable dt = new DAL.Tables.T_Banks().Open("", "", "[Order]");
        if(!string.IsNullOrEmpty(_User.IDCardNumber))
        {
            tbIDCardNumber.Enabled = false;
            tbIDCardNumber.Text = _User.IDCardNumber.Substring(0, 4) + _User.IDCardNumber.Substring(_User.IDCardNumber.Length - 4, 4).PadLeft(10, '*');
            lbIsIdCardNumberValided.Text = "*已绑定";
            lbIsIdCardNumberValided.CssClass = "red12";
        }
        else
        {
            tbIDCardNumber.Enabled = true;
            tbIDCardNumber.Text = "";
            lbIsIdCardNumberValided.Text = "*未绑定";
        }
        hdIDCardNumber.Value = tbIDCardNumber.Text;
    }

    protected void btnOK_Click(object sender, EventArgs e)
    {
        lbErrPwd.Visible = false;
        if (_User == null)
        {
            return;
        }
        if (string.IsNullOrEmpty(tbVerPwd.Text))
        {
            Shove._Web.JavaScript.Alert(this.Page, "请填写您的密码。");
            lbErrPwd.Text = "*";
            lbErrPwd.Visible = true;
            tbVerPwd.Focus();
            return;
        }
        if (!PF.EncryptPassword(tbVerPwd.Text).Equals(_User.Password))
        {
            Shove._Web.JavaScript.Alert(this.Page, "密码不正确，请重新输入密码。");
            tbVerPwd.Focus();
            return;
        }
        if (string.IsNullOrEmpty(_User.RealityName) && string.IsNullOrEmpty(tbRealityName.Text))
        {
            Shove._Web.JavaScript.Alert(this.Page, "请输入真实姓名。");
            return;
        }

        if (string.IsNullOrEmpty(_User.IDCardNumber) && !string.IsNullOrEmpty(tbIDCardNumber.Text))
        {
            if (!Shove._String.Valid.isIDCardNumber(tbIDCardNumber.Text) && !Shove._String.Valid.isIDCardNumber_Hongkong(tbIDCardNumber.Text) &&
                !Shove._String.Valid.isIDCardNumber_Macau(tbIDCardNumber.Text) && !Shove._String.Valid.isIDCardNumber_Taiwan(tbIDCardNumber.Text) && 
                !Shove._String.Valid.isIDCardNumber_Singapore(tbIDCardNumber.Text))
            {
                Shove._Web.JavaScript.Alert(this.Page, "身份证号码输入有误！");
                return;
            }

            _User.IDCardNumber = Shove._Web.Utility.FilteSqlInfusion(tbIDCardNumber.Text);
            //根据当前的身份证号查询此身份证号的历史用户的CPSID是否为"来宝商家"CSPID-839,是,则把当前会员的CPSID也置为宝商家CSPID-839
            if (tbIDCardNumber.Text.Trim() != "")
            {
                object resObj = Shove.Database.MSSQL.ExecuteScalar("select 1 from T_Users where IDCardNumber='" + Shove._Web.Utility.FilteSqlInfusion(tbIDCardNumber.Text) + "' and CpsID=839 ", new Shove.Database.MSSQL.Parameter[0]);
                if (resObj != null && _User.CpsID != 839)
                {
                    _User.CpsID = 839;
                }
            }
        }

        if (tbEmail.Text.Trim() == "")
        {
            Shove._Web.JavaScript.Alert(this.Page, "请输入电子邮件地址。");
            tbEmail.Focus();
            return;
        }

        if (!Shove._String.Valid.isEmail(tbEmail.Text.Trim()))
        {
            Shove._Web.JavaScript.Alert(this.Page, "电子邮件地址格式不正确。");
            tbEmail.Focus();
            return;
        }

        Users tu = new Users(_Site.ID);
        _User.Clone(tu);
        
        if (string.IsNullOrEmpty(_User.RealityName))
        {
            _User.RealityName = Shove._Web.Utility.FilteSqlInfusion(tbRealityName.Text);
        }

        _User.CityID = ddlCity.City_id;
        _User.Sex = rbSexM.Checked ? "男" : (rbSexW.Checked ? "女" : "");
        _User.BirthDay = Shove._Convert.StrToDateTime(tbBirthday.Text.Trim(), "1980-1-1");
        _User.Address = tbAddress.Text.Trim();
        _User.Mobile = tbMobile.Text;
        _User.isMobileValided = string.IsNullOrEmpty(tbMobile.Text) ? false : true;
        if (_User.Email != Shove._Convert.ToDBC(tbEmail.Text).Trim()) //改变了邮箱，需要重新验证
        {
            _User.isEmailValided = false;
        }
        _User.Email = Shove._Convert.ToDBC(tbEmail.Text).Trim();
       
        string ReturnDescription = "";
        int Result = _User.EditByID(ref ReturnDescription);
        if (Result < 0)
        {
            new Log("Users").Write("修改用户基本资料失败："+ ReturnDescription);
            tu.Clone(_User);
            Shove._Web.JavaScript.Alert(this.Page, ReturnDescription);
            return;
        }
        //同步修改至主站
        var log = new Log("Users");
        try
        {
            var r = SyncUserInfoFromLottery(_User.ID, _User.RealityName, _User.IDCardNumber, _User.Email);
            if (r == "1")
            {
                log.Write("同步修改用户基本资料成功");
            }
            else { 
                log.Write("同步修改用户基本资料失败： " + r);
            }
        }
        catch(Exception ex) 
        {
            log.Write("同步修改用户基本资料异常： " + ex.Message);
        }
        //同步修改至主站 end

        string FromUrl = Shove._Web.Utility.GetRequest("FromUrl");
        if (FromUrl == "")
        {
            FromUrl = "UserEdit.aspx";
        }
        Shove._Web.JavaScript.Alert(this.Page, "用户资料已经保存成功。", FromUrl);
    }

    private int CheckUserName(string name)
    {
        if (!PF.CheckUserName(name))
        {
            return -1;
        }

        DataTable dt = new DAL.Tables.T_Users().Open("ID", "Name = '" + Shove._Web.Utility.FilteSqlInfusion(name) + "'", "");

        if (dt != null && dt.Rows.Count > 0)
        {
            return -2;
        }

        if (Shove._String.GetLength(name) < 5 || Shove._String.GetLength(name) > 16)
        {
            return -3;
        }

        return 0;
    }

    /// <summary>
    /// 校验用户是否可用
    /// </summary>
    /// <returns></returns>
    [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
    public int CheckUserNameAjax(string name)
    {
        if (!PF.CheckUserName(name))
        {
            return -1;
        }

        DataTable dt = new DAL.Tables.T_Users().Open("ID", "Name = '" + Shove._Web.Utility.FilteSqlInfusion(name) + "'", "");

        if (dt != null && dt.Rows.Count > 0)
        {
            return -2;
        }

        if (Shove._String.GetLength(name) < 5 || Shove._String.GetLength(name) > 16)
        {
            return -3;
        }

        return 0;
    }

    //protected void ddlQuestion_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    if (ddlQuestion.SelectedValue == "自定义问题")
    //    {
    //        trMQ.Visible = true;
    //    }
    //    else
    //    {
    //        trMQ.Visible = false;
    //    }
    //}

    #region 同步修改至主站
    /// <summary>
    /// 从彩票频道更新用户信息，同步至主站
    /// </summary>
    /// <returns></returns>
    public string SyncUserInfoFromLottery(long lotteryUserId,string trueName,string idCardNo,string email)
    { 
        var reqstr = new StringBuilder();
        reqstr.Append(string.Format("lotUserId={0}&tn={1}&cardno={2}&email={3}"
                        ,lotteryUserId,Server.UrlEncode(trueName),idCardNo,email));
        
        var apiUrl = string.Format("{0}/api/updateuserinfo", System.Configuration.ConfigurationManager.AppSettings["JsHeaderUrl"]);
        var r = PF.Post(apiUrl, reqstr.ToString(), 5);
        return r;
    }
    #endregion
}
