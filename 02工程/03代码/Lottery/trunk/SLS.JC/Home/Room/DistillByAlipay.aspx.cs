using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Home_Room_DistillByAlipay : RoomPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        
        if (!IsPostBack)
        {
            if (_User != null)
            {
                HidIsCps.Value = Shove._Web.Utility.GetRequest("IsCps");
                if (!_User.isAlipayNameValided)
                {
                    Response.Write("<script type='text/javascript'>alert('您还没有绑定支付宝帐号,请先绑定支账号.谢谢！');parent.window.location='BindAlipay.aspx?FromUrl=Distill.aspx&Type=1&IsCps=" + HidIsCps.Value + "'</script>");
                    return;
                }

                if (string.IsNullOrEmpty(HidIsCps.Value))
                {
                    HidIsCps.Value = "0";
                }

                if (HidIsCps.Value == "1")
                {
                    DataTable dt = Shove.Database.MSSQL.Select("select Balance from T_Users where ID = " + _User.ID.ToString() + "");

                    if (dt == null || dt.Rows.Count == 0)
                    {
                        return;
                    }

                    lbMoney.Text = dt.Rows[0]["Balance"].ToString();
                }
                else
                {
                    lbMoney.Text = _User.Balance.ToString();
                }

                if (Shove._Web.Utility.GetRequest("Step") == "3")
                {
                    ShowOrHiddenPanel(3);
                }
                else
                {
                    if (string.IsNullOrEmpty(_User.SecurityQuestion))
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
    protected void btnNext_Click(object sender, EventArgs e)
    {
        if (_User == null)
        {
            return;
        }

        if (_User.RealityName == "")
        {
            Response.Write("<script type='text/javascript'>alert('请完善您的基本资料，真实姓名不能为空，谢谢！');parent.window.location='UserEdit.aspx?FromUrl=Distill.aspx&Type=1&IsCps="+HidIsCps.Value+"';</script>");
        }

        if (!_User.isAlipayNameValided)
        {
            Response.Write("<script type='text/javascript'>alert('您的支付宝帐号未绑定，不能提款，谢谢！');parent.window.location='BindAlipay.aspx?FromUrl=Distill.aspx&Type=1&IsCps=" + HidIsCps.Value + "'</script>");
        }

        if (this.lbQuestion.Text == "")
        {
            Response.Write("<script type='text/javascript'>alert('为了您的账户安全，请先设置安全保护问题，谢谢！');parent.window.location='SafeSet.aspx?FromUrl=Distill.aspx&Type=1&IsCps=" + HidIsCps.Value + "';</script>");
            return;
        }

        double realMoney = Shove._Convert.StrToDouble(this.lbMoney.Text, 0);
        double distillMoney = Shove._Convert.StrToDouble(this.tbMoney.Text, 0);
        if (distillMoney <= 0.00 || distillMoney > realMoney)
        {
            Shove._Web.JavaScript.Alert(this.Page, "请输入正确的提款金额， 您的可提金额为：" + realMoney + "元。谢谢！");
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
        ShowOrHiddenPanel(4);
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
       
        string ReturnDescription = "";
        double Money = Shove._Convert.StrToDouble(this.lblDistillMoney.Text, 0);
        double formalitiesFees = CalculateDistillFormalitiesFees( Money);//计算手续费
        if ( formalitiesFees >= Money)
        {
            Shove._Web.JavaScript.Alert(this.Page, "提款失败:所需手续费大于或等于提款金额,实付金额 = 提款金额- 手续费.");

            return;
        }

        bool IsCps = false;

        int Result = _User.Distill(1, Money, formalitiesFees, "", "", "", _User.AlipayID, _User.AlipayName, "支付宝提款",IsCps, ref ReturnDescription);
        if (Result < 0)
        {
            Shove._Web.JavaScript.Alert(this.Page, ReturnDescription);

            return;
        }
        
        btnOK.Enabled = false;//防止多次操作

        //清除提款明细页面的数据缓存
        string cacheKeyName = "Home_Room_DistillDetail_" + _User.ID.ToString();
        Shove._Web.Cache.ClearCache(cacheKeyName);

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
    private void bindAlipay()
    {
        if (!_User.isAlipayNameValided)
        {
            string info = "（需更改绑定支付宝账号，请根据“<a href='BindAlipay.aspx?FromUrl=Distill.aspx?Type=1&IsCps=" + HidIsCps.Value + "' target='_parent'><span class='blue12_2'>修改绑定支付宝账号</span></a>”流程处理。）";
            labBindState.InnerHtml = info;
        }

        string AlipayName = _User.AlipayName;

        if (AlipayName.IndexOf("@") > 0)
        {
            string[] arrName = AlipayName.Split('@');
            AlipayName = "*******" + arrName[0].Substring(arrName[0].Length - 1, 1) + "@" + arrName[1];
        }
        else if (AlipayName.Length == 11)
        {
            AlipayName = "*******" + AlipayName.Substring(AlipayName.Length - 4, 4);
        }

        lbAlipayName.Text = AlipayName;

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
                bindAlipay();
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
    //private bool IsFirstDistill()
    //{ 
      
    //    if (_User == null)
    //    {
    //        return false;
    //    }

    //    string CacheKeyName = "Room_UserDistills_" + _User.ID.ToString();

    //    DataTable dt =  new DAL.Views.V_UserDistills().Open("ID,[DateTime],[Money],FormalitiesFees,Result,Memo", "[UserID] = " + _User.ID.ToString() + "", "[DateTime] desc, [ID]");

    //    if (dt != null && dt.Rows.Count > 0)
    //    {
    //        return false;
    //    }
    //    else
    //    {
    //        return true;
    //    }
    //}

    /// <summary>
    /// 指定的提款方式,计算所需的提款手续费
    /// </summary>
    /// <param name="DistillMoney">提款金额</param>
    /// <returns></returns>
    /* 
        支付宝账户  实时到帐  5000元以内的1块钱，5000元以上每笔10元手续费。
    */
    protected static double CalculateDistillFormalitiesFees(double DistillMoney)
    {
        double formalitiesFees = 0;

        if (DistillMoney < 10000)
        {
            formalitiesFees = 1;
        }
        else
        {
            formalitiesFees = 10;
        }
        return formalitiesFees;
    }
}
