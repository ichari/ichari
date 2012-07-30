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

using System.Text;
public partial class Admin_UserDetail : AdminPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        AjaxPro.Utility.RegisterTypeForAjax(typeof(Admin_UserDetail), this.Page);
        if (!this.IsPostBack)
        {
            //BindDataForBankType();
            BindData();
            GetEnableCompetence();

            //btnResetPassword.Visible = _User.Competences.IsOwnedCompetences(Competences.BuildCompetencesList(Competences.MemberManagement));
            //btnSave.Visible = btnResetPassword.Visible;
            //btnUserAccount.Visible = btnResetPassword.Visible;
        }
    }

    #region Web 窗体设计器生成的代码

    override protected void OnInit(EventArgs e)
    {
        RequestLoginPage = this.Request.Url.AbsoluteUri;

        RequestCompetences = Competences.BuildCompetencesList(Competences.MemberManagement, Competences.QueryData);

        base.OnInit(e);
    }

    #endregion

    //private void BindDataForBankType()
    //{
    //    DataTable dt = new DAL.Tables.T_Banks().Open("", "", "[Order]");

    //    Shove.ControlExt.FillDropDownList(ddlUserBankType, dt, "Name", "ID");
    //}

    private void GetEnableCompetence()
    {
        DataTable dt = Shove.Database.MSSQL.Select("select * from T_UserInGroups  where GroupID =1 and UserID=" + _User.ID + " ");

        if (dt != null && dt.Rows.Count > 0)
        {
            this.btnSave.Visible = true;
            this.EmptyQuestn.Visible = true;
            this.btnResetPassword.Visible = true;

        }
        else
        {
            this.btnSave.Visible = false;
            this.EmptyQuestn.Visible = false;
            this.btnResetPassword.Visible = false;

        }
    }

    private void BindData()
    {
        long SiteID = Shove._Convert.StrToLong(Shove._Web.Utility.GetRequest("SiteID"), -1);
        long UserID = Shove._Convert.StrToLong(Shove._Web.Utility.GetRequest("id"), -1);

        if ((SiteID < 1) || (UserID < 1))
        {
            this.Response.Redirect("Users.aspx", true);

            return;
        }

        DataTable dt = new DAL.Views.V_Users().Open("", "SiteID = " + SiteID.ToString() + " and [ID] = " + UserID.ToString(), "");

        if (dt == null)
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", "Admin_UserDetail");

            return;
        }

        if (dt.Rows.Count < 1)
        {
            PF.GoError(ErrorNumber.Unknow, "用户不存在", "Admin_UserDetail");

            return;
        }

        tbSiteID.Text = SiteID.ToString();
        tbUserID.Text = UserID.ToString();

        Users tu = new Users(SiteID)[SiteID, UserID];

        if (tu.ID < 1)
        {
            PF.GoError(ErrorNumber.Unknow, "用户不存在", "Admin_UserDetail");

            return;
        }

        tbUserName.Text = tu.Name;
        tbUserEmail.Text = tu.Email;

        if (tu.UserType == 1)
        {
            rbUserType1.Checked = true;
        }
        else if (tu.UserType == 2)
        {
            rbUserType2.Checked = true;
        }
        else if (tu.UserType == 3)
        {
            rbUserType3.Checked = true;
        }

        tbUserRealityName.Text = tu.RealityName;
        tbUserIDCardNumber.Text = Shove._Convert.ToDBC(tu.IDCardNumber);
        ddlUserCity.City_id = tu.CityID;
        tbUserTelephone.Text = Shove._Convert.ToDBC(tu.Telephone);
        tbUserMobile.Text = Shove._Convert.ToDBC(tu.Mobile);
        cbUserMobileValid.Checked = (tu.isMobileValided && (tu.Mobile != ""));
        tbUserEmail.Text = tu.Email;
        tbUserQQ.Text = Shove._Convert.ToDBC(tu.QQ);
        tbUserAddress.Text = tu.Address;

        if (tu.Sex == "男")
        {
            rbUserSexM.Checked = true;
        }
        else if (tu.Sex == "女")
        {
            rbUserSexW.Checked = true;
        }

        tbUserBirthDay.Text = tu.BirthDay.ToShortDateString();
        //Shove.ControlExt.SetDownListBoxTextFromValue(ddlUserBankType, tu.BankType.ToString());
        //tbUserBankName.Text = tu.BankName;
        tbUserBankCardNumber.Text = Shove._Convert.ToDBC(tu.BankCardNumber).Trim();
        tbScoringOfSelfBuy.Text = tu.ScoringOfSelfBuy.ToString();
        tbScoringOfCommendBuy.Text = tu.ScoringOfCommendBuy.ToString();
        cbPrivacy.Checked = tu.isPrivacy;
        cbisCanLogin.Checked = tu.isCanLogin;


        string bankTypeName = "";
        string bankName = "";
        string bankCardNumber = "";
        string bankInProvinceName = "";
        string bankInCityName = "";
        string bankUserName = "";
        int returnValue = -1;
        string returnDescription = "";

        if (DAL.Procedures.P_GetUserBankDetail(SiteID, UserID, ref bankTypeName, ref bankName, ref bankCardNumber, ref bankInProvinceName, ref bankInCityName, ref bankUserName, ref returnValue, ref returnDescription) < 0)
        {
            Shove._Web.JavaScript.Alert(this.Page, returnDescription);
            return;
        }

        tbUserCradName.Text = bankUserName;
        tbUserBankCardNumber.Text = bankCardNumber;

        hfBankInProvince.Value = bankInProvinceName;
        hfBankInCity.Value = bankInCityName;
        hfBankTypeName.Value = bankTypeName;
        hfBankName.Value = bankName;


        /*
            tbPromotionMemberBonusScale.Text = tu.PromotionMemberBonusScale.ToString();
            tbPromotionSiteBonusScale.Text = tu.PromotionSiteBonusScale.ToString();
    */
        btnResetPassword.Enabled = (_User.Competences.IsOwnedCompetences(Competences.Administrator));
    }

    protected void btnUserAccount_Click(object sender, EventArgs e)
    {
        this.Response.Redirect("UserAccountDetail.aspx?SiteID=" + tbSiteID.Text + "&ID=" + tbUserID.Text);
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        

        long SiteID = Shove._Convert.StrToLong(tbSiteID.Text, -1);
        long UserID = Shove._Convert.StrToLong(tbUserID.Text, -1);

        string bankInProvinceName = Request.Form["selProvince"]==null? "" : Request.Form["selProvince"].ToString();//银行所在省
        string bankInCityName =Request.Form["selCity"]==null? "" : Request.Form["selCity"].ToString();        //银行所在市
        string bankTypeName = Request.Form["selBankTypeName"]==null? "":Request.Form["selBankTypeName"].ToString();   //银行类名
        string bankName = Request.Form["selBankName"]==null? "" : Request.Form["selBankName"].ToString();           //银行支行名
        string bankCardNumber = tbUserBankCardNumber.Text.Trim();
        string bankUserName = Shove._Web.Utility.FilteSqlInfusion(this.tbUserCradName.Text.Trim());

        bool needBindBankFlag = (bankInProvinceName != "" || bankInCityName != "" || bankTypeName != "" || bankName != "" || bankCardNumber != "" || bankUserName != "");


        if ((SiteID < 1) || (UserID < 1))
        {
            PF.GoError(ErrorNumber.Unknow, "参数错误", "Admin_UserDetail");

            return;
        }

        Users tu = new Users(SiteID)[SiteID, UserID];

        if (tu.ID < 1)
        {
            PF.GoError(ErrorNumber.Unknow, "用户不存在", "Admin_UserDetail");

            return;
        }

        if (tbUserEmail.Text.Trim() == "")
        {
            Shove._Web.JavaScript.Alert(this.Page, "请输入电子邮件地址。");
            return;
        }

        if (!Shove._String.Valid.isEmail(tbUserEmail.Text.Trim()))
        {
            Shove._Web.JavaScript.Alert(this.Page, "电子邮件地址格式不正确。");
            return;
        }

        if (rbUserType2.Checked)
        {
            if (tbUserRealityName.Text.Trim() == "")
            {
                Shove._Web.JavaScript.Alert(this.Page, "请输入真实姓名。");

                return;
            }

            if (tbUserIDCardNumber.Text.Trim() == "")
            {
                Shove._Web.JavaScript.Alert(this.Page, "请输入身份证号。");

                return;
            }

            if ((!Shove._String.Valid.isIDCardNumber(tbUserIDCardNumber.Text.Trim())) && (!Shove._String.Valid.isIDCardNumber_Hongkong(tbUserIDCardNumber.Text.Trim()))
            && (!Shove._String.Valid.isIDCardNumber_Macau(tbUserIDCardNumber.Text.Trim())) && (!Shove._String.Valid.isIDCardNumber_Singapore(tbUserIDCardNumber.Text.Trim()))
            && (!Shove._String.Valid.isIDCardNumber_Taiwan(tbUserIDCardNumber.Text.Trim())))
            {
                Shove._Web.JavaScript.Alert(this.Page, "身份证号格式不正确。");

                return;
            }

            try
            {
                DateTime.Parse(tbUserBirthDay.Text.Trim());
            }
            catch
            {
                Shove._Web.JavaScript.Alert(this.Page, "请输入正确的生日。");

                return;
            }

            if (needBindBankFlag)
            {
                if (bankName == "")
                {
                    Shove._Web.JavaScript.Alert(this.Page, "请输入开户银行。");

                    return;
                }
                if (bankCardNumber == "")
                {
                    Shove._Web.JavaScript.Alert(this.Page, "请输入银行卡号。");

                    return;
                }
                if (!Shove._String.Valid.isBankCardNumber(bankCardNumber))
                {
                    Shove._Web.JavaScript.Alert(this.Page, "银行卡号格式不正确。");

                    return;
                }
                if (bankUserName=="")
                {
                    Shove._Web.JavaScript.Alert(this.Page, "请输入持卡人姓名。");

                    return;
                }
            }
            

            double ScoringOfSelfBuy = Shove._Convert.StrToDouble(tbScoringOfSelfBuy.Text, -1);

            if (ScoringOfSelfBuy < 0)
            {
                Shove._Web.JavaScript.Alert(this.Page, "请输入正确的购彩积分比例。");

                return;
            }

            double ScoringOfCommendBuy = Shove._Convert.StrToDouble(tbScoringOfCommendBuy.Text, -1);

            if (ScoringOfCommendBuy < 0)
            {
                Shove._Web.JavaScript.Alert(this.Page, "请输入正确的下级购彩积分比例。");

                return;
            }
            /*
                        double PromotionMemberBonusScale = Shove._Convert.StrToDouble(tbPromotionMemberBonusScale.Text, -1);

                        if (PromotionMemberBonusScale < 0 || PromotionMemberBonusScale > 1)
                        {
                            Shove._Web.JavaScript.Alert(this.Page, "请输入正确的推广会员的佣金比例。");

                            return;
                        }

                        double PromotionSiteBonusScale = Shove._Convert.StrToDouble(tbPromotionSiteBonusScale.Text, -1);

                        if (PromotionSiteBonusScale < 0 || PromotionSiteBonusScale > 1)
                        {
                            Shove._Web.JavaScript.Alert(this.Page, "请输入正确的推广站长的佣金比例。");

                            return;
                        }
                        */
        }

        tu.UserType = (short)(rbUserType2.Checked ? 2 : rbUserType3.Checked ? 3 : 1);

        tu.Email = tbUserEmail.Text.Trim();
        tu.RealityName = tbUserRealityName.Text.Trim();
        tu.IDCardNumber = Shove._Convert.ToDBC(tbUserIDCardNumber.Text.Trim()).Trim();
        tu.CityID = ddlUserCity.City_id;
        tu.Telephone = Shove._Convert.ToDBC(tbUserTelephone.Text.Trim()).Trim();
        tu.Mobile = Shove._Convert.ToDBC(tbUserMobile.Text.Trim()).Trim();
        tu.isMobileValided = (cbUserMobileValid.Checked && (tu.Mobile != ""));
        tu.QQ = Shove._Convert.ToDBC(tbUserQQ.Text.Trim()).Trim();
        tu.Address = tbUserAddress.Text.Trim();
        tu.Sex = rbUserSexM.Checked ? "男" : (rbUserSexW.Checked ? "女" : "");
        tu.BirthDay = DateTime.Parse(tbUserBirthDay.Text.Trim());
        //tu.BankType = short.Parse(ddlUserBankType.SelectedValue);
        tu.BankName = bankName;
        tu.BankCardNumber = Shove._Convert.ToDBC(tbUserBankCardNumber.Text.Trim()).Trim();
        tu.ScoringOfSelfBuy = Shove._Convert.StrToDouble(tbScoringOfSelfBuy.Text, 0);
        tu.ScoringOfCommendBuy = Shove._Convert.StrToDouble(tbScoringOfCommendBuy.Text, 0);
        tu.isPrivacy = cbPrivacy.Checked;
        tu.isCanLogin = cbisCanLogin.Checked;





        /*
        tu.PromotionMemberBonusScale = Shove._Convert.StrToDouble(tbPromotionMemberBonusScale.Text, 0);
        tu.PromotionSiteBonusScale = Shove._Convert.StrToDouble(tbPromotionSiteBonusScale.Text, 0);
        */

        string ReturnDescription = "";
        int returnValue = -1;

        if (tu.EditByID(ref ReturnDescription) < 0)
        {
            Shove._Web.JavaScript.Alert(this.Page, ReturnDescription);

            return;
        }

        //绑定银行资料
        if (needBindBankFlag)
        { 
            if (DAL.Procedures.P_UserBankDetailEdit(SiteID, UserID, bankTypeName, bankName, bankCardNumber, bankInProvinceName, bankInCityName, bankUserName, ref returnValue, ref ReturnDescription) < 0)
            {
                tu.Clone(_User);
                Shove._Web.JavaScript.Alert(this.Page, ReturnDescription);

                return;
            }
            hfBankInProvince.Value = bankInProvinceName;
            hfBankInCity.Value = bankInCityName;
            hfBankTypeName.Value = bankTypeName;
            hfBankName.Value = bankName;
        }

        Shove._Web.JavaScript.Alert(this.Page, "用户资料已经保存成功。");
    }

    protected void btnResetPassword_Click(object sender, EventArgs e)
    {
        long SiteID = Shove._Convert.StrToLong(tbSiteID.Text, -1);
        long UserID = Shove._Convert.StrToLong(tbUserID.Text, -1);

        if ((SiteID < 1) || (UserID < 1))
        {
            PF.GoError(ErrorNumber.Unknow, "参数错误", "Admin_UserDetail");

            return;
        }

        Users tu = new Users(SiteID)[SiteID, UserID];

        if (tu.ID < 1)
        {
            PF.GoError(ErrorNumber.Unknow, "用户不存在", "Admin_UserDetail");

            return;
        }

        string Password = GetRandPassword();

        tu.Password = Password;
        tu.PasswordAdv = Password;

        string ReturnDescription = "";

        if (tu.EditByID(ref ReturnDescription) < 0)
        {
            Shove._Web.JavaScript.Alert(this.Page, ReturnDescription);

            return;
        }

        Shove._Web.JavaScript.Alert(this.Page, "用户密码已经被重置为：" + Password + "，请牢记。");
    }

    private string GetRandPassword()
    {
        string CharSet = "0123456789";
        string Password = "";
        Random rand = new Random(DateTime.Now.Millisecond);

        for (int i = 0; i < 6; i++)
        {
            Password += CharSet[rand.Next(0, 9)].ToString();
        }

        return Password;
    }
    protected void EmptyQuestn_Click(object sender, EventArgs e)
    {
        long SiteID = Shove._Convert.StrToLong(tbSiteID.Text, -1);
        long UserID = Shove._Convert.StrToLong(tbUserID.Text, -1);

        if ((SiteID < 1) || (UserID < 1))
        {
            PF.GoError(ErrorNumber.Unknow, "参数错误", "Admin_UserDetail");

            return;
        }

        Users tu = new Users(SiteID)[SiteID, UserID];

        if (tu.ID < 1)
        {
            PF.GoError(ErrorNumber.Unknow, "用户不存在", "Admin_UserDetail");

            return;
        }

        DAL.Tables.T_Users user = new DAL.Tables.T_Users();

        user.SecurityQuestion.Value = "";
        user.SecurityAnswer.Value = "";

        long Result = user.Update("ID=" + UserID);

        if (Result < 0)
        {
            Shove._Web.JavaScript.Alert(this.Page, "清空安全问题失败");

            return;
        }
        Shove._Web.JavaScript.Alert(this.Page, "清空安全问题成功");
    }



    [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
    public string GetProvinceList()
    {
        string BANK_PROVINCE_LIST = "Home_Room_BindBankCard_BankInProvince";

        string provinceList = Shove._Web.Cache.GetCacheAsString(BANK_PROVINCE_LIST, "");
        if (string.IsNullOrEmpty(provinceList))
        {
            DataTable dt = Shove.Database.MSSQL.Select("select distinct ProvinceName from T_BankDetails order by ProvinceName ", new Shove.Database.MSSQL.Parameter[0]);
            StringBuilder sb = new StringBuilder();
            foreach (DataRow row in dt.Rows)
            {
                sb.Append(row["ProvinceName"].ToString() + "|");
            }
            provinceList = sb.ToString();
            Shove._Web.Cache.SetCache(BANK_PROVINCE_LIST, provinceList);
        }

        return provinceList;

    }

    [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
    public string GetCityList(string ProvinceName)
    {
        string BANK_PROVINCE_CITY_LIST = "BANK_PROVINCE_CITY_LIST" + ProvinceName;

        string cityList = Shove._Web.Cache.GetCacheAsString(BANK_PROVINCE_CITY_LIST, "");
        if (string.IsNullOrEmpty(cityList))
        {
            DataTable dt = Shove.Database.MSSQL.Select("select distinct CityName from T_BankDetails where ProvinceName='" + Shove._Web.Utility.FilteSqlInfusion(ProvinceName) + "' order by CityName ", new Shove.Database.MSSQL.Parameter[0]);
            StringBuilder sb = new StringBuilder();
            foreach (DataRow row in dt.Rows)
            {
                sb.Append(row["CityName"].ToString() + "|");
            }
            cityList = sb.ToString();
            Shove._Web.Cache.SetCache(BANK_PROVINCE_CITY_LIST, cityList);
        }

        return cityList;

    }

    [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
    public string GetBankTypeList()
    {
        string cacheKey = "Home_Room_BindBankCard_GetBankTypeList";

        string listStr = Shove._Web.Cache.GetCacheAsString(cacheKey, "");
        if (string.IsNullOrEmpty(listStr))
        {
            DataTable dt = Shove.Database.MSSQL.Select("select distinct  BankTypeName  from T_BankDetails order by BankTypeName ", new Shove.Database.MSSQL.Parameter[0]);
            StringBuilder sb = new StringBuilder();
            foreach (DataRow row in dt.Rows)
            {
                sb.Append(row["BankTypeName"].ToString() + "|");
            }
            listStr = sb.ToString();
            Shove._Web.Cache.SetCache(cacheKey, listStr, 600);
        }

        return listStr;

    }


    [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
    public string GetBankBranchNameList(string cityName, string bankTypeName)
    {
        string cacheKey = "Home_Room_BindBankCard_GetBankBranchNameList_" + cityName + "_" + bankTypeName;

        string listStr = Shove._Web.Cache.GetCacheAsString(cacheKey, "");
        if (string.IsNullOrEmpty(listStr))
        {
            DataTable dt = Shove.Database.MSSQL.Select("select   BankName  from T_BankDetails where BankTypeName='" + bankTypeName + "' and CityName='" + cityName + "'   order by BankName ", new Shove.Database.MSSQL.Parameter[0]);
            StringBuilder sb = new StringBuilder();
            foreach (DataRow row in dt.Rows)
            {
                sb.Append(row["BankName"].ToString() + "|");
            }
            listStr = sb.ToString();
            Shove._Web.Cache.SetCache(cacheKey, listStr, 600);
        }

        return listStr;

    }
}
