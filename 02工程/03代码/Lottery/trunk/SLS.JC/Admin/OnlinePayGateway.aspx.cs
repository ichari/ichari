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

public partial class Admin_OnlinePayGateway : AdminPageBase
{
    SystemOptions so = new SystemOptions();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
           
            BindData();

            btnOK.Visible = _User.Competences.IsOwnedCompetences(Competences.BuildCompetencesList(Competences.Options));
        }

        tbAlipayKey.Attributes.Add("value", tbAlipayKey.Text);
        tbAlipayOutKey.Attributes.Add("value", tbAlipayOutKey.Text);

        tbOnlinePay_Alipay_ForUserDistill_MD5Key.Attributes.Add("value", tbOnlinePay_Alipay_ForUserDistill_MD5Key.Text);
        tbOnlinePay_Alipay_ForUserDistill_MD5Key_ForPayOut.Attributes.Add("value", tbOnlinePay_Alipay_ForUserDistill_MD5Key_ForPayOut.Text);

        tb99BillKey.Attributes.Add("value", tb99BillKey.Text);
        tb99BillOutKey.Attributes.Add("value", tb99BillOutKey.Text);
        tb99BillQueryKey.Attributes.Add("value", tb99BillQueryKey.Text);

        tbTenpayKey.Attributes.Add("value", tbTenpayKey.Text);
        tbTenpayOutKey.Attributes.Add("value", tbTenpayOutKey.Text);
        tbCBPayMentKey.Attributes.Add("value", tbCBPayMentKey.Text);
        tbCBPayMentOutKey.Attributes.Add("value", tbCBPayMentOutKey.Text);

        tbYeePayKey.Attributes.Add("value", tbYeePayKey.Text);
        tbYeePayKey.Attributes.Add("value", tbYeePayKey.Text);

    }

    #region Web 窗体设计器生成的代码

    override protected void OnInit(EventArgs e)
    {
        RequestLoginPage = this.Request.Url.AbsoluteUri;

        RequestCompetences = Competences.BuildCompetencesList(Competences.Options,Competences.QueryData);

        base.OnInit(e);
    }

    #endregion

    private void BindData()
    {
        SystemOptions so = new SystemOptions();
        // 中国银联
        tbChinaUnionName.Text = so["OnlinePay_ChinaUnion_UserName"].ToString("");
        tbChinaUnionNumber.Text = so["OnlinePay_ChinaUnion_UserNumber"].ToString("");
        tbChinaUnionKey.Text = so["OnlinePay_ChinaUnion_MD5"].ToString("");
        tbChinaUnionPayCommisionRate.Text = so["OnlinePay_ChinaUnion_CommissionScale"].ToString("0.00");        
        cbChinaUnionStatus.Checked = so["OnlinePay_ChinaUnion_Status_ON"].ToBoolean(false);

        // 支付宝
        tbAlipayName.Text = so["OnlinePay_Alipay_UserName"].ToString("");
        tbAlipayNumber.Text = so["OnlinePay_Alipay_UserNumber"].ToString("");
        tbAlipayKey.Text = so["OnlinePay_Alipay_MD5Key"].ToString("");
        tbAlipayPayFormalitiesFeesScale.Text = so["OnlinePay_Alipay_PayFormalitiesFeesScale"].ToString("0.00");
        tbAlipayOutKey.Text = so["OnlinePayOut_Alipay_MD5Key"].ToString("");
        tbAlipayDistillFormalitiesFeesScale.Text = so["OnlinePayOut_Alipay_DistillFormalitiesFeesScale"].ToString("0.00");
        cbAlipayON.Checked = so["OnlinePay_Alipay_Status_ON"].ToBoolean(false);

        // 支付宝(会员提款)
        tbOnlinePay_Alipay_ForUserDistill_UserName.Text = so["OnlinePay_Alipay_ForUserDistill_UserName"].ToString("");
        tbOnlinePay_Alipay_ForUserDistill_UserNumber.Text = so["OnlinePay_Alipay_ForUserDistill_UserNumber"].ToString("");
        tbOnlinePay_Alipay_ForUserDistill_MD5Key.Text = so["OnlinePay_Alipay_ForUserDistill_MD5Key"].ToString("");
        tbOnlinePay_Alipay_ForUserDistill_MD5Key_ForPayOut.Text = so["OnlinePay_Alipay_ForUserDistill_MD5Key_ForPayOut"].ToString("");
        cbOnlinePay_Alipay_ForUserDistill_Status_ON.Checked = so["OnlinePay_Alipay_ForUserDistill_Status_ON"].ToBoolean(false);

        // 快钱
        tb99BillName.Text = so["OnlinePay_99Bill_UserName"].ToString("");
        tb99BillNumber.Text = so["OnlinePay_99Bill_UserNumber"].ToString("");
        tb99BillKey.Text = so["OnlinePay_99Bill_MD5Key"].ToString("");
        tb99BillPayFormalitiesFeesScale.Text = so["OnlinePay_99Bill_PayFormalitiesFeesScale"].ToString("0.00");
        tb99BillOutKey.Text = so["OnlinePayOut_99Bill_MD5Key"].ToString("");
        tb99BillQueryKey.Text = so["OnlinePay_99Bill_QueryMD5Key"].ToString("");
        tb99BillDistillFormalitiesFeesScale.Text = so["OnlinePayOut_99Bill_DistillFormalitiesFeesScale"].ToString("0.00");
        cb99BillON.Checked = so["OnlinePay_99Bill_Status_ON"].ToBoolean(false);

        // 财付通
        tbTenpayName.Text = so["OnlinePay_Tenpay_UserName"].ToString("");
        tbTenpayNumber.Text = so["OnlinePay_Tenpay_UserNumber"].ToString("");
        tbTenpayKey.Text = so["OnlinePay_Tenpay_MD5Key"].ToString("");
        tbTenpayPayFormalitiesFeesScale.Text = so["OnlinePay_Tenpay_PayFormalitiesFeesScale"].ToString("0.00");
        tbTenpayOutKey.Text = so["OnlinePayOut_Tenpay_MD5Key"].ToString("");
        tbTenpayDistillFormalitiesFeesScale.Text = so["OnlinePayOut_Tenpay_DistillFormalitiesFeesScale"].ToString("0.00");
        cbTenpayON.Checked = so["OnlinePay_Tenpay_Status_ON"].ToBoolean(false);

        // 网银在线
        tbCBPayMentName.Text = so["OnlinePay_CBPayMent_UserName"].ToString("");
        tbCBPayMentNumber.Text = so["OnlinePay_CBPayMent_UserNumber"].ToString("");
        tbCBPayMentKey.Text = so["OnlinePay_CBPayMent_MD5Key"].ToString("");
        tbCBPayMentPayFormalitiesFeesScale.Text = so["OnlinePay_CBPayMent_PayFormalitiesFeesScale"].ToString("0.00");
        tbCBPayMentOutKey.Text = so["OnlinePayOut_CBPayMent_MD5Key"].ToString("");
        tbCBPayMentDistillFormalitiesFeesScale.Text = so["OnlinePayOut_CBPayMent_DistillFormalitiesFeesScale"].ToString("0.00");
        cbCBPayMentON.Checked = so["OnlinePay_CBPayMent_Status_ON"].ToBoolean(false);

        // 易宝
        tbYeePayName.Text = so["OnlinePay_YeePay_UserName"].ToString("");
        tbYeePayNumber.Text = so["OnlinePay_YeePay_UserNumber"].ToString("");
        tbYeePayKey.Text = so["OnlinePay_YeePay_MD5Key"].ToString("");
        tbYeePayFormalitiesFeesScale.Text = so["OnlinePay_YeePay_PayFormalitiesFeesScale"].ToString("0.00");
        tbYeePayOutKey.Text = so["OnlinePayOut_YeePay_MD5Key"].ToString("");
        tbYeePayDistillFormalitiesFeesScale.Text = so["OnlinePayOut_YeePay_DistillFormalitiesFeesScale"].ToString("0.00");
        cbYeePayON.Checked = so["OnlinePay_YeePay_Status_ON"].ToBoolean(false);

        //神州行(007卡)
        tb007Ka_FormalitiesFees.Text = so["OnlinePay_007Ka_FormalitiesFees"].ToString("0.00");
        tb007Ka_MerAccount.Text = so["OnlinePay_007Ka_MerAccount"].ToString("");
        tb007Ka_MerchantId.Text = so["OnlinePay_007Ka_MerchantId"].ToString("");
        cb007KaON.Checked = so["OnlinePay_007Ka_Status_ON"].ToBoolean(false);
    }

    protected void btnOK_Click(object sender, EventArgs e)
    {
        SystemOptions so = new SystemOptions();

        try
        {
            // 中国银联
            so["OnlinePay_ChinaUnion_UserName"] = new OptionValue(tbChinaUnionName.Text);
            so["OnlinePay_ChinaUnion_UserNumber"] = new OptionValue(tbChinaUnionNumber.Text);
            so["OnlinePay_ChinaUnion_MD5"] = new OptionValue(tbChinaUnionKey.Text);
            so["OnlinePay_ChinaUnion_CommissionScale"] = new OptionValue(Shove._Convert.StrToDouble(tbChinaUnionPayCommisionRate.Text, 0));
            so["OnlinePay_ChinaUnion_Status_ON"] = new OptionValue(cbChinaUnionStatus.Checked);

            // 支付宝
            so["OnlinePay_Alipay_UserName"] = new OptionValue(tbAlipayName.Text);
            so["OnlinePay_Alipay_UserNumber"] = new OptionValue(tbAlipayNumber.Text);
            so["OnlinePay_Alipay_MD5Key"] = new OptionValue(tbAlipayKey.Text);
            so["OnlinePay_Alipay_PayFormalitiesFeesScale"] = new OptionValue(Shove._Convert.StrToDouble(tbAlipayPayFormalitiesFeesScale.Text, 0));
            so["OnlinePayOut_Alipay_MD5Key"] = new OptionValue(tbAlipayOutKey.Text);
            so["OnlinePayOut_Alipay_DistillFormalitiesFeesScale"] = new OptionValue(Shove._Convert.StrToDouble(tbAlipayDistillFormalitiesFeesScale.Text, 0));
            so["OnlinePay_Alipay_Status_ON"] = new OptionValue(cbAlipayON.Checked);


            // 支付宝(会员提款)
            so["OnlinePay_Alipay_ForUserDistill_UserName"] = new OptionValue(tbOnlinePay_Alipay_ForUserDistill_UserName.Text);
            so["OnlinePay_Alipay_ForUserDistill_UserNumber"] = new OptionValue(tbOnlinePay_Alipay_ForUserDistill_UserNumber.Text);
            so["OnlinePay_Alipay_ForUserDistill_MD5Key"] = new OptionValue(tbOnlinePay_Alipay_ForUserDistill_MD5Key.Text);
            so["OnlinePay_Alipay_ForUserDistill_MD5Key_ForPayOut"] = new OptionValue(tbOnlinePay_Alipay_ForUserDistill_MD5Key_ForPayOut.Text);
            so["OnlinePay_Alipay_ForUserDistill_Status_ON"] = new OptionValue(cbOnlinePay_Alipay_ForUserDistill_Status_ON.Checked);


            // 快钱
            so["OnlinePay_99Bill_UserName"] = new OptionValue(tb99BillName.Text);
            so["OnlinePay_99Bill_UserNumber"] = new OptionValue(tb99BillNumber.Text);
            so["OnlinePay_99Bill_MD5Key"] = new OptionValue(tb99BillKey.Text);
            so["OnlinePay_99Bill_PayFormalitiesFeesScale"] = new OptionValue(Shove._Convert.StrToDouble(tb99BillPayFormalitiesFeesScale.Text, 0));
            so["OnlinePayOut_99Bill_MD5Key"] = new OptionValue(tb99BillOutKey.Text);
            so["OnlinePay_99Bill_QueryMD5Key"] = new OptionValue(tb99BillQueryKey.Text);
            so["OnlinePayOut_99Bill_DistillFormalitiesFeesScale"] = new OptionValue(Shove._Convert.StrToDouble(tb99BillDistillFormalitiesFeesScale.Text, 0));
            so["OnlinePay_99Bill_Status_ON"] = new OptionValue(cb99BillON.Checked);

            // 财付通
            so["OnlinePay_Tenpay_UserName"] = new OptionValue(tbTenpayName.Text);
            so["OnlinePay_Tenpay_UserNumber"] = new OptionValue(tbTenpayNumber.Text);
            so["OnlinePay_Tenpay_MD5Key"] = new OptionValue(tbTenpayKey.Text);
            so["OnlinePay_Tenpay_PayFormalitiesFeesScale"] = new OptionValue(Shove._Convert.StrToDouble(tbTenpayPayFormalitiesFeesScale.Text, 0));
            so["OnlinePayOut_Tenpay_MD5Key"] = new OptionValue(tbTenpayOutKey.Text);
            so["OnlinePayOut_Tenpay_DistillFormalitiesFeesScale"] = new OptionValue(Shove._Convert.StrToDouble(tbTenpayDistillFormalitiesFeesScale.Text, 0));
            so["OnlinePay_Tenpay_Status_ON"] = new OptionValue(cbTenpayON.Checked);

            // 网银在线
            so["OnlinePay_CBPayMent_UserName"] = new OptionValue(tbCBPayMentName.Text);
            so["OnlinePay_CBPayMent_UserNumber"] = new OptionValue(tbCBPayMentNumber.Text);
            so["OnlinePay_CBPayMent_MD5Key"] = new OptionValue(tbCBPayMentKey.Text);
            so["OnlinePay_CBPayMent_PayFormalitiesFeesScale"] = new OptionValue(Shove._Convert.StrToDouble(tbCBPayMentPayFormalitiesFeesScale.Text, 0));
            so["OnlinePayOut_CBPayMent_MD5Key"] = new OptionValue(tbCBPayMentOutKey.Text);
            so["OnlinePayOut_CBPayMent_DistillFormalitiesFeesScale"] = new OptionValue(Shove._Convert.StrToDouble(tbCBPayMentDistillFormalitiesFeesScale.Text, 0));
            so["OnlinePay_CBPayMent_Status_ON"] = new OptionValue(cbCBPayMentON.Checked);

            // 易宝
            so["OnlinePay_YeePay_UserName"] = new OptionValue(tbYeePayName.Text);
            so["OnlinePay_YeePay_UserNumber"] = new OptionValue(tbYeePayNumber.Text);
            so["OnlinePay_YeePay_MD5Key"] = new OptionValue(tbYeePayKey.Text);
            so["OnlinePay_YeePayy_PayFormalitiesFeesScale"] = new OptionValue(Shove._Convert.StrToDouble(tbYeePayFormalitiesFeesScale.Text, 0));
            so["OnlinePayOut_YeePay_MD5Key"] = new OptionValue(tbYeePayOutKey.Text);
            so["OnlinePayOut_YeePay_DistillFormalitiesFeesScale"] = new OptionValue(Shove._Convert.StrToDouble(tbYeePayDistillFormalitiesFeesScale.Text, 0));
            so["OnlinePay_YeePay_Status_ON"] = new OptionValue(cbYeePayON.Checked);

            //神州行(007卡)
            so["OnlinePay_007Ka_FormalitiesFees"] = new OptionValue(tb007Ka_FormalitiesFees.Text);
            so["OnlinePay_007Ka_MerAccount"] = new OptionValue(tb007Ka_MerAccount.Text);
            so["OnlinePay_007Ka_MerchantId"] = new OptionValue(tb007Ka_MerchantId.Text);
            so["OnlinePay_007Ka_Status_ON"] = new OptionValue(cb007KaON.Checked);
        }
        catch (Exception exception)
        {
            PF.GoError(ErrorNumber.Unknow, exception.Message, "Admin_OnlinePayGateway");

            return;
        }

        Shove._Web.JavaScript.Alert(this.Page, "设置保存成功。");
    }
}
