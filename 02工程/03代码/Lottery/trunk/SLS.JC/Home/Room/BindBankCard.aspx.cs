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

public partial class Home_Room_BindBankCard : RoomPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        AjaxPro.Utility.RegisterTypeForAjax(typeof(Home_Room_BindBankCard), this.Page);
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
        Label1.Text = _User.Name;
        labName.Text = _User.Name;

        string bankTypeName = "";
        string bankName = "";
        string bankCardNumber = "";
        string bankInProvince = "";
        string bankInCity = "";
        string bankUserName = "";
        StringBuilder sb = new StringBuilder();
        sb.Append("select BankTypeName,BankName, BankCardNumber, BankInProvinceName, BankInCityName, BankUserName from  T_userBankBindDetails  ")
            .Append(" where  UserID = " + _User.ID.ToString() + " ");

        DataTable dt = Shove.Database.MSSQL.Select(sb.ToString());
        if (dt != null && dt.Rows.Count > 0)
        {
            bankName = dt.Rows[0]["BankName"].ToString().Trim();
            bankCardNumber = dt.Rows[0]["BankCardNumber"].ToString().Trim();
            bankUserName = dt.Rows[0]["BankUserName"].ToString().Trim();
            bankTypeName = dt.Rows[0]["BankTypeName"].ToString().Trim();
            tbBankCardNumber.Text = bankCardNumber;
            tbBankCardNumberOK.Text = bankCardNumber;
            tbBankCardRealityName.Text = bankUserName;
            HidBankName.Value = bankName;
            bankInProvince = dt.Rows[0]["BankInProvinceName"].ToString().Trim();
            bankInCity = dt.Rows[0]["BankInCityName"].ToString().Trim();
            hfBankInProvince.Value = dt.Rows[0]["BankInProvinceName"].ToString().Trim();
            hfBankInCity.Value = dt.Rows[0]["BankInCityName"].ToString().Trim();
            hfBankTypeName.Value = dt.Rows[0]["BankTypeName"].ToString().Trim();
            hfBankName.Value = dt.Rows[0]["BankName"].ToString().Trim();
        }

        if (string.IsNullOrEmpty(bankCardNumber))
        {
            lbBankCardNumber.Text = _User.BankCardNumber;
            lbBankCardNumberOK.Text = _User.BankCardNumber;
        }
        else
        {
            lbBankCardNumber.Text = bankCardNumber.Length > 4 ? "************" + bankCardNumber.Substring(bankCardNumber.Length - 4, 4) : bankCardNumber;
            lbBankCardNumberOK.Text = bankCardNumber.Length > 4 ? "************" + bankCardNumber.Substring(bankCardNumber.Length - 4, 4) : bankCardNumber;
        }
        if (bankUserName.Length > 1)
        {
            lbBankCardRealityName.Text = "*".PadLeft(bankUserName.Length - 1, '*') + bankUserName.Substring(bankUserName.Length - 1);
        }

        //Shove._String.Valid.isBankCardNumber(bankCardNumber)
        if (bankCardNumber !="" && bankUserName != "" && bankTypeName!="" && bankName !="" && bankInProvince != "" && bankInCity != "")
        {
            hfIsBindFlag.Value = "true";//标记已经绑定
            labBindState.Text = "已绑定";
            lbStatus.Text = "您已经绑定";
            showTxtOrLbl(2);
        }
        else
        {
            hfIsBindFlag.Value = "false";//标记已经绑定
            labBindState.Text = "未绑定";
            lbStatus.Text = "您一旦绑定";
            tbBankCardNumber.Text = _User.BankCardNumber;   //取原来用户的字段
            showTxtOrLbl(1);
        }
    }

    private void showTxtOrLbl(int type)
    {
        if (type == 1)  //未绑定
        {
            this.tbBankCardNumber.Visible = true;
            this.tbBankCardNumberOK.Visible = true;
            this.tbBankCardRealityName.Visible = true;
            this.lbBankCardNumber.Visible = false;
            this.lbBankCardNumberOK.Visible = false;
            this.lbBankCardRealityName.Visible = false;
        }
        else//已绑定
        {
            this.tbBankCardNumber.Visible = false;
            this.tbBankCardNumberOK.Visible = false;
            this.tbBankCardRealityName.Visible = false;
            this.lbBankCardNumber.Visible = true;
            this.lbBankCardNumberOK.Visible = true;
            this.lbBankCardRealityName.Visible = true;
        }
    }

    protected void btnOK_Click(object sender, EventArgs e)
    {
        if (_User == null)
        {
            return;
        }
        if (string.IsNullOrEmpty(tbVerPwd.Text))
        {
            Shove._Web.JavaScript.Alert(this.Page, "请输入账户密码。");
            tbVerPwd.Focus();
            return;
        }
        if (!PF.EncryptPassword(tbVerPwd.Text).Equals(_User.Password))
        {
            Shove._Web.JavaScript.Alert(this.Page, "账户密码不正确。");
            tbVerPwd.Focus();
            return;
        }
        string bankInProvinceName = Request.Form["selProvince"]==null?"":Request.Form["selProvince"].ToString();
        string bankInCityName = Request.Form["selCity"]==null?"":Request.Form["selCity"].ToString();
        string bankTypeName = Request.Form["selBankTypeName"]==null?"":Request.Form["selBankTypeName"].ToString();
        string bankName = Request.Form["selBankName"]==null?"":Request.Form["selBankName"].ToString();
        string bankCardNumber = Shove._Web.Utility.FilteSqlInfusion(this.tbBankCardNumber.Text.Trim());
        string bankUserName = Shove._Web.Utility.FilteSqlInfusion(this.tbBankCardRealityName.Text.Trim());
        string bankCardNumberConform =  Shove._Web.Utility.FilteSqlInfusion(this.tbBankCardNumberOK.Text.Trim());

        if (_User.RealityName == "")
        {
            Response.Write("<script type='text/javascript'>alert('请完善您的基本资料，真实姓名不能为空，谢谢！');window.location='UserEdit.aspx?FromUrl=BindBankCard.aspx'</script>");
        }
 
        if (bankInProvinceName == "")
        {
            Shove._Web.JavaScript.Alert(this.Page, "请输入银行卡开户银行所在的省份！");
            return;
        }
        if (bankInCityName == "")
        {
            Shove._Web.JavaScript.Alert(this.Page, "请输入银行卡开户银行所在的城市！");
            return;
        }
        if (bankTypeName == "")
        {
            Shove._Web.JavaScript.Alert(this.Page, "请输入银行卡开户银行类型！");
            return;
        }
        if (bankName == "")
        {
            Shove._Web.JavaScript.Alert(this.Page, "请输入银行卡开户银行支行名称！");
            return;
        }

        if (bankCardNumber == "")
        {
            Shove._Web.JavaScript.Alert(this.Page, "请输入收款银行卡号！");
            return;
        }
        if (!Shove._String.Valid.isBankCardNumber(bankCardNumber))
        {
            Shove._Web.JavaScript.Alert(this.Page, "银行卡号输入有误！");
            return;
        }

        if (bankCardNumber != bankCardNumberConform)
        {
            Shove._Web.JavaScript.Alert(this.Page, "两次输入的银行卡号不一致，请确认后提交，谢谢！");
            return;
        }

        if (bankUserName == "")
        {
            Shove._Web.JavaScript.Alert(this.Page, "请输入持卡人真实姓名！");
            return;
        }
        if (bankUserName != _User.RealityName)
        {
            Shove._Web.JavaScript.Alert(this.Page, _Site.Name+"目前不支持设置非自己本人开户的银行卡帐户进行提款！");
            return;
        }

        //if (this.tbRealityName.Text.Trim() != _User.RealityName)
        //{
        //    Shove._Web.JavaScript.Alert(this.Page, "请核实您的真实姓名，谢谢！");
        //    return;
        //}

        System.Threading.Thread.Sleep(500);

        Users tu = new Users(_Site.ID);
        _User.Clone(tu);
        _User.BankName = bankName;
        _User.BankCardNumber = bankCardNumber;

        int returnValue = 0;
        string ReturnDescription = "";

        if (_User.EditByID(ref ReturnDescription) < 0)
        {
            tu.Clone(_User);
            Shove._Web.JavaScript.Alert(this.Page, ReturnDescription);
            return;
        }

        if (bankName == "" || (HidBankName1.Value != bankName && bankName.IndexOf("*") > -1))
        {
            Shove._Web.JavaScript.Alert(this.Page, "请输入正确的银行格式！");
            return;
        }
        else
        {
            if (HidBankName1.Value == bankName)
            {
                bankName = HidBankName.Value;
            }
        }

        //写入用户银行绑定资料
        if (DAL.Procedures.P_UserBankDetailEdit(_Site.ID, _User.ID, bankTypeName, bankName, bankCardNumber, bankInProvinceName, bankInCityName, bankUserName, ref returnValue, ref ReturnDescription) < 0)
        {
            tu.Clone(_User);
            Shove._Web.JavaScript.Alert(this.Page, ReturnDescription);
            return;
        }
        if (returnValue < 0)
        {
            tu.Clone(_User);
            Shove._Web.JavaScript.Alert(this.Page, ReturnDescription);
            return;
        }
        hfBankInProvince.Value = bankInProvinceName;
        hfBankInCity.Value = bankInCityName;
        hfBankTypeName.Value = bankTypeName;
        hfBankName.Value = bankName;

        string FromUrl = Shove._Web.Utility.GetRequest("FromUrl");
        if (FromUrl == "")
        {
            FromUrl = "BindBankCard.aspx";
        }
        else
        {
            if (Shove._Web.Utility.GetRequest("Type") != "")
            {
                FromUrl += "?Type=2";
            }
        }
        Shove._Web.JavaScript.Alert(this.Page, "银行卡绑定成功。", FromUrl);
    }

    protected void ddlProvince_SelectedIndexChanged(object sender, EventArgs e)
    {
        //BindCity();
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