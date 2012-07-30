using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Hoom_Room_DistillBybank : RoomPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string outputSystemError = "";
            //if (!CheckUserBindBank(ref outputSystemError))
            //{
            //    if (outputSystemError != "")
            //    {
            //        Response.Write("<script type='text/javascript'>alert('" + outputSystemError + "');</script>");
            //        form1.Visible = false;
            //        return;
            //    }
            //    Response.Write("<script type='text/javascript'>alert('请完善您的绑定银行卡资料，谢谢！');parent.window.location='BindBankCard.aspx'</script>");
            //    return;
            //}
            
            if (_User != null)
            {
                HidIsCps.Value = Shove._Web.Utility.GetRequest("IsCps");

                if (string.IsNullOrEmpty(HidIsCps.Value))
                {
                    HidIsCps.Value = "0";
                }

                lbMoney.Text = _User.Balance.ToString();

                if (Shove._Web.Utility.GetRequest("Step") == "3")
                {
                    ShowOrHiddenPanel(3);
                }
                else
                {
                    if (IsFirstDistill())
                    {
                        ShowOrHiddenPanel(1);
                    }
                    else
                    {
                        ShowOrHiddenPanel(3);
                    }
                }
            }
        }
    }

    #region Web 窗体设计器生成的代码

    override protected void OnInit(EventArgs e)
    {
        isRequestLogin = true;
        RequestLoginPage = this.Request.Url.AbsoluteUri;

        base.OnInit(e);
    }

    #endregion
    //检查用户是否已经完整绑定银行资料
    protected bool CheckUserBindBank(ref string OutputSystemError)
    {
        string bankTypeName = "";
        string bankName = "";
        string bankCardNumber = "";
        string bankInProvinceName = "";
        string bankInCityName = "";
        string bankUserName = "";

        int returnValue = 0;
        string returnDescription = "";

        if (DAL.Procedures.P_GetUserBankDetail(_Site.ID, _User.ID, ref bankTypeName, ref bankName, ref bankCardNumber, ref bankInProvinceName, ref bankInCityName, ref bankUserName, ref returnValue, ref returnDescription) < 0)
        {
            OutputSystemError = "获取用户绑定分行信息出错";
            return false;
        }
        if(returnValue!=0)
        {
            OutputSystemError = returnDescription;
            return false;
        }
        if (string.IsNullOrEmpty(bankTypeName) || string.IsNullOrEmpty(bankName) || string.IsNullOrEmpty(bankCardNumber) || string.IsNullOrEmpty(bankInProvinceName) || string.IsNullOrEmpty(bankInCityName) || string.IsNullOrEmpty(bankUserName))
        {
            return false;
        }

        if (!Shove._String.Valid.isBankCardNumber(bankCardNumber))
        {
            return false;
        }
        return true;
    }

    protected void btnNext_Click(object sender, EventArgs e)
    {
        if (_User == null)
        {
            return;
        }
        if (_User.RealityName == "")
        {
            Response.Write("<script type='text/javascript'>alert('请完善您的基本资料，真实姓名不能为空，谢谢！');parent.window.location='UserEdit.aspx?FromUrl=Distill.aspx&Type=2&IsCps=" + HidIsCps.Value + "'</script>");
        }

        string bankTypeName = "";
        string bankName = "";
        string bankCardNumber = "";
        string bankInProvinceName = "";
        string bankInCityName = "";
        string bankUserName = "";

        int returnValue = 0;
        string returnDescription = "";

        if (DAL.Procedures.P_GetUserBankDetail(_Site.ID, _User.ID, ref bankTypeName, ref bankName, ref bankCardNumber, ref bankInProvinceName, ref bankInCityName, ref bankUserName, ref returnValue, ref returnDescription) < 0)
        {
            Shove._Web.JavaScript.Alert(this.Page, "获取用户绑定分行信息出错");
            return;
        }
        if (string.IsNullOrEmpty(bankTypeName) || string.IsNullOrEmpty(bankName) || string.IsNullOrEmpty(bankCardNumber) || string.IsNullOrEmpty(bankInProvinceName) || string.IsNullOrEmpty(bankInCityName) || string.IsNullOrEmpty(bankUserName))
        {
            Response.Write("<script type='text/javascript'>alert('请先绑定您的银行卡帐号,并完善填写相关资料，谢谢！');parent.window.location='BindBankCard.aspx?FromUrl=Distill.aspx&Type=2&IsCps=" + HidIsCps.Value + "'</script>");
        }
        if (this.lbQuestion.Text == "")
        {
            Response.Write("<script type='text/javascript'>alert('为了您的账户安全，请先设置安全保护问题，谢谢！');parent.window.location='SafeSet.aspx?FromUrl=Distill.aspx&Type=2&IsCps=" + HidIsCps.Value + "';</script>");
            return;
        }

        double realMoney = Shove._Convert.StrToDouble(this.lbMoney.Text, 0);
        double distillMoney = Shove._Convert.StrToDouble(this.tbMoney.Text, 0);
        if (distillMoney <= 0.00 || distillMoney > realMoney)
        {
            Shove._Web.JavaScript.Alert(this.Page, "请输入正确的提款金额， 您的可提金额为：" + realMoney + "元。谢谢！");
            return;
        }
        if (distillMoney > 500000)//500000以上请联系客服人员
        {
            Shove._Web.JavaScript.Alert(this.Page, "500000以上的提款请联系客服人员。谢谢！");
            return;
        }
        
        if (this.tbName.Text != _User.RealityName)
        {
            Shove._Web.JavaScript.Alert(this.Page, "请检查您的真实姓名是否填写正确，谢谢！");

            return;
        }

        if (tbMyA.Text.Trim() != _User.SecurityAnswer)
        {
            Shove._Web.JavaScript.Alert(this.Page, "安全保护问题回答错误。");

            return;
        }

        this.lblDistillMoney.Text = distillMoney.ToString();
        this.lblLastMoney.Text = (realMoney - distillMoney).ToString();
        pnlFirst.Visible = false;
        pnlNext.Visible = true;
    }
    protected void lbReturn_Click(object sender, EventArgs e)
    {
        ShowOrHiddenPanel(3);
    }

    protected void btnOK_Click(object sender, EventArgs e)
    {
        if (_User == null)
        {
            return;
        }
        string bankTypeName = "";
        string bankName = "";
        string bankCardNumber = "";
        string bankInProvinceName = "";
        string bankInCityName = "";
        string bankUserName = "";

        int returnValue = 0;
        string returnDescription = "";

        if (DAL.Procedures.P_GetUserBankDetail(_Site.ID, _User.ID, ref bankTypeName, ref bankName, ref bankCardNumber, ref bankInProvinceName, ref bankInCityName, ref bankUserName, ref returnValue, ref returnDescription) < 0)
        {
            Response.Write("<script type='text/javascript'>alert('出错!');</script>");
            return;
        }
        if (returnValue != 0)
        {
            Response.Write("<script type='text/javascript'>alert('出错!');</script>");
            return;
        }
       
        string ReturnDescription = "";
        double Money = Shove._Convert.StrToDouble(this.lblDistillMoney.Text, 0);
        double formalitiesFees = CalculateDistillFormalitiesFees(Money);//计算手续费

        if (formalitiesFees >= Money)
        {
            Shove._Web.JavaScript.Alert(this.Page, "提款失败:所需手续费大于或等于提款金额,实付金额 = 提款金额- 手续费.");

            return;
        }

        bool IsCps = false;

        int Result = _User.Distill(2, Money, formalitiesFees, tbName.Text.Trim(), _User.BankName, _User.BankCardNumber, "", "", "银行卡提款", IsCps, ref ReturnDescription);

        if (Result < 0)
        {
            Shove._Web.JavaScript.Alert(this.Page, "申请提款失败。");

            return;
        }
        btnOK.Enabled = false;//防止多次操作

        //清除提款明细页面的数据缓存
        string CacheKeyName = "Home_Room_DistillDetail_" + _User.ID.ToString();
        Shove._Web.Cache.ClearCache(CacheKeyName);

        Response.Write("<script type='text/javascript'>alert('申请提款成功。');parent.clickLottery(parent.document.getElementById('PayDistill'));</script>");
    }
    protected void btnSafeSetNext_Click(object sender, EventArgs e)
    {
        string Question = ddlQuestion.SelectedValue;

        if (tbOAnswer.Text.Trim() != _User.SecurityAnswer)
        {
            Shove._Web.JavaScript.Alert(this.Page, "原安全问题回答错误");

            return;
        }

        if (Question == "自定义问题")
        {
            Question = Shove._Web.Utility.FilteSqlInfusion(tbMyQuestion.Text.Trim());

            if (Question == "")
            {
                Shove._Web.JavaScript.Alert(this.Page, "请输入安全问题");

                return;
            }

            Question = "自定义问题|" + Question;
        }
        else
        {
            Question = ddlQuestion.SelectedValue;
        }

        string Answer = Shove._Web.Utility.FilteSqlInfusion(tbAnswer.Text.Trim());

        if (Answer == "")
        {
            Shove._Web.JavaScript.Alert(this.Page, "请输入答案");

            return;
        }

        DAL.Tables.T_Users user = new DAL.Tables.T_Users();

        user.SecurityQuestion.Value = Question;
        user.SecurityAnswer.Value = Answer;

        long Result = user.Update("ID=" + _User.ID.ToString());

        if (Result < 0)
        {
            Shove._Web.JavaScript.Alert(this.Page, "设置安全问题失败");

            return;
        }
        Response.Write("<script type='text/javascript'>alert('设置安全问题成功。请注意安全保护问题是最重要的安全凭证，为了您的安全，请牢牢记住您的安全保护问题。');</script>");
        ShowOrHiddenPanel(2);
    }

    protected void btnUserEditNext_Click(object sender, EventArgs e)
    {
        if (_User == null)
        {
            return;
        }

        if (_User.RealityName == "" && this.tbRealityName.Text.Trim() == "")
        {
            Shove._Web.JavaScript.Alert(this.Page, "请输入真实姓名。");
            return;
        }


        if (tbIdIDCardNumber.Visible)
        {
            string tempIdCardNumber = this.tbIdIDCardNumber.Text;
            if (!Shove._String.Valid.isIDCardNumber(tempIdCardNumber) && !Shove._String.Valid.isIDCardNumber_Hongkong(tempIdCardNumber) &&
                !Shove._String.Valid.isIDCardNumber_Macau(tempIdCardNumber) && !Shove._String.Valid.isIDCardNumber_Singapore(tempIdCardNumber)
                && !Shove._String.Valid.isIDCardNumber_Taiwan(tempIdCardNumber))
            {
                Shove._Web.JavaScript.Alert(this.Page, "身份证号码输入有误！");
                return;
            }
        }

        Users tu = new Users(_Site.ID);
        _User.Clone(tu);

        if (this.tbRealityName.Visible)
        {
            _User.RealityName = Shove._Web.Utility.FilteSqlInfusion(this.tbRealityName.Text);
        }
        if (tbIdIDCardNumber.Visible)
        {
            _User.IDCardNumber = Shove._Web.Utility.FilteSqlInfusion(this.tbIdIDCardNumber.Text);
        }
        string ReturnDescription = "";

        if (_User.EditByID(ref ReturnDescription) < 0)
        {
            tu.Clone(_User);
            Shove._Web.JavaScript.Alert(this.Page, ReturnDescription);

            return;
        }
        if (this.tbRealityName.Visible && tbIdIDCardNumber.Visible)
        {
            Response.Write("<script type='text/javascript'>alert('完善会员资料成功！');</script>");
        }
        ShowOrHiddenPanel(3);
    }
    private void bindBankCard()
    {
        string bankTypeName = "";
        string bankName = "";
        string bankCardNumber = "";
        string bankInProvinceName = "";
        string bankInCityName = "";
        string bankUserName = "";

        int returnValue = 0;
        string returnDescription = "";

        if (DAL.Procedures.P_GetUserBankDetail(_Site.ID, _User.ID, ref bankTypeName, ref bankName, ref bankCardNumber, ref bankInProvinceName, ref bankInCityName, ref bankUserName, ref returnValue, ref returnDescription) < 0)
        {
            Response.Write("<script type='text/javascript'>alert('出错!');</script>");
            return ;
        }
        if (returnValue != 0)
        {
            Response.Write("<script type='text/javascript'>alert('出错!');</script>");
            return;
        }


        if (!Shove._String.Valid.isBankCardNumber(bankCardNumber))
        {
            string info = "（需更改绑定银行卡账号，请根据“<a href='BindBankCard.aspx?FromUrl=Distill.aspx?Type=2&IsCps=" + HidIsCps.Value + "' target='_parent'><span class='blue12_2'>修改绑定银行卡账号</span></a>”流程处理。）";
            labBindState.InnerHtml = info;
            trBankCardRealityName.Visible = false;
            trBankName.Visible = false;
        }
        else
        {
            lbBankNo.Text = "************" + bankCardNumber.Substring(bankCardNumber.Length - 4, 4);
            lbBankCardRealityName.Text = "*".PadLeft(bankUserName.Length - 1, '*') + bankUserName.Substring(bankUserName.Length - 1);
            lbBankInProvince.Text = bankInProvinceName;
            lbBankInCity.Text = bankInCityName;
            lbBankTypeName.Text = bankTypeName;
            lbBankName.Text =bankName.Length<=4? bankName :  "*".PadLeft(bankName.Length - 4, '*') + bankName.Substring(bankName.Length - 4);
        }

        if (_User.SecurityQuestion.StartsWith("自定义问题|"))
        {
            lbQuestion.Text = _User.SecurityQuestion.Remove(0, 6);
        }
        else
        {
            lbQuestion.Text = _User.SecurityQuestion;
        }

        if (lbQuestion.Text == "")
        {
            lbQuestionInfo.Text = "设置安全保护问题";
        }
        else
        {
            lbQuestionInfo.Text = "修改安全保护问题";
        }
    }

    private void bindSafeSet()
    {
        ddlQuestion.DataSource = DataCache.SecurityQuestions;
        ddlQuestion.DataBind();

        if (_User.SecurityQuestion == "")
        {
            trOldAns.Visible = false;
            trOldQue.Visible = false;
        }
        else
        {
            lbOQuestion.Text = _User.SecurityQuestion;
            trOldAns.Visible = true;
            trOldQue.Visible = true;
        }
    }

    private void bindUserInfo()
    {
        if (_User.RealityName != "")
        {
            this.tbRealityName.Visible = false;
            this.lbRealityName.Visible = true;
            this.lbRealityName.Text = "***";
            this.lbIsRealityNameValided.Text = "已绑定";
        }
        else
        {
            this.tbRealityName.Visible = true;
            this.lbRealityName.Visible = false;
            this.tbRealityName.Text = "";
            this.lbIsRealityNameValided.Text = "未绑定";
        }


        try
        {
            if (_User.IDCardNumber.Length == 15)
            {
                lbIdCardNumber.Visible = true;
                tbIdIDCardNumber.Visible = false;
                lbIdCardNumber.Text = _User.IDCardNumber.Substring(0, 6) + "*****" + _User.IDCardNumber.Substring(10, 4);
                lbIsIdCardNumberValided.Text = "已绑定";

            }
            else
            {
                lbIdCardNumber.Visible = true;
                tbIdIDCardNumber.Visible = false;
                lbIdCardNumber.Text = _User.IDCardNumber.Substring(0, 6) + "********" + _User.IDCardNumber.Substring(14, 4);
                lbIsIdCardNumberValided.Text = "已绑定";
            }
        }
        catch
        {
            lbIdCardNumber.Visible = false;
            tbIdIDCardNumber.Visible = true;
            lbIdCardNumber.Text = "";
            lbIsIdCardNumberValided.Text = "未绑定";
        }
    }
    private void ShowOrHiddenPanel(int step)
    {
        switch (step)
        {
            case 1:
                pnlSafeSet.Visible = true;
                pnlUserEdit.Visible = false;
                pnlFirst.Visible = false;
                pnlNext.Visible = false;
                bindSafeSet();
                break;
            case 2:
                pnlSafeSet.Visible = false;
                pnlUserEdit.Visible = true;
                pnlFirst.Visible = false;
                pnlNext.Visible = false;
                bindUserInfo();
                break;
            case 3:
                pnlSafeSet.Visible = false;
                pnlUserEdit.Visible = false;
                pnlFirst.Visible = true;
                pnlNext.Visible = false;
                bindBankCard();
                break;
            case 4:
                pnlSafeSet.Visible = false;
                pnlUserEdit.Visible = false;
                pnlFirst.Visible = false;
                pnlNext.Visible = true;
                break;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <returns>返回 true 表示是第一次提款 返回false 表示不是第一次提款</returns>
    private bool IsFirstDistill()
    {

        if (_User == null)
        {
            return false;
        }

        string CacheKeyName = "Room_UserDistills_" + _User.ID.ToString();

        DataTable dt = new DAL.Views.V_UserDistills().Open("ID,[DateTime],[Money],FormalitiesFees,Result,Memo", "[UserID] = " + _User.ID.ToString() + "", "[DateTime] desc, [ID]");

        if (dt != null && dt.Rows.Count > 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    protected void ddlQuestion_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlQuestion.SelectedValue == "自定义问题")
        {
            trMQ.Visible = true;
        }
        else
        {
            trMQ.Visible = false;
        }
    }

    /// <summary>
    /// 银行卡提款方式,计算所需的提款手续费
    /// </summary>
    /// <returns></returns>
    /*  小于500元手续费每笔1元；500-50000（含）以内的手续费每笔5元；50001－200000（含）手续费每笔10元；200001以上的按每笔1%收取手续费。
    */
    protected static double CalculateDistillFormalitiesFees(double DistillMoney)
    {
        double formalitiesFees = 0;

        if (DistillMoney<500) 
        {
            formalitiesFees = 1;
        }
        else if (DistillMoney >= 500 && DistillMoney <= 50000)
        {
            formalitiesFees = 5;
        }
        else if (DistillMoney > 50000 && DistillMoney <= 200000)
        {
            formalitiesFees =10;
        }
        else if (DistillMoney > 200000 && DistillMoney <= 500000)
        {
            formalitiesFees = 20;
        }
        else if (DistillMoney > 500000)
        {
            formalitiesFees = DistillMoney * 0.01;
        }
        return formalitiesFees;
    }
}
