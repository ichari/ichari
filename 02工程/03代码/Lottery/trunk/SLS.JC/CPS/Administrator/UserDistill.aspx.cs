using System;
using System.IO;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Web.Configuration;
using System.Text;
using System.Security.Cryptography;
using System.Net;

using Shove.Alipay;

public partial class Cps_Administrator_UserDistill : AdminPageBase
{

    SystemOptions so = new SystemOptions();

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
        RequestLoginPage = Shove._Web.Utility.GetUrl() + "/Cps/Administrator/Default.aspx";

        RequestCompetences = Competences.BuildCompetencesList(Competences.DistillMoney);

        base.OnInit(e);
    }

    #endregion

    private void BindData()
    {
        DataTable dt = new DAL.Views.V_UserDistills().Open("", "IsCps = 1 and Result = " + HandleResult.Trying.ToString(), "[DateTime]");

        if (dt == null)
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", this.GetType().BaseType.FullName);

            return;
        }

        g.DataSource = dt;
        g.DataBind();

        labTip.Visible = (dt.Rows.Count == 0);
    }

    protected void g_ItemCommand(object source, System.Web.UI.WebControls.DataListCommandEventArgs e)
    {
        long SiteID = Shove._Convert.StrToLong(((HtmlInputHidden)e.Item.FindControl("tbSiteID")).Value, -1);
        long id = Shove._Convert.StrToLong(((HtmlInputHidden)e.Item.FindControl("tbID")).Value, -1);
        long UserID = Shove._Convert.StrToLong(((HtmlInputHidden)e.Item.FindControl("tbUserID")).Value, -1);
        double Money = Math.Round(Shove._Convert.StrToDouble(((HtmlInputHidden)e.Item.FindControl("tbMoney")).Value, 0), 2);

        HtmlInputHidden tbBankName = (HtmlInputHidden)(e.Item.FindControl("tbBankName"));
        HtmlInputHidden tbBankUserName = (HtmlInputHidden)(e.Item.FindControl("tbBankUserName"));
        HtmlInputHidden tbBankCardNumber = (HtmlInputHidden)(e.Item.FindControl("tbBankCardNumber"));
        HtmlInputHidden tbProvince = (HtmlInputHidden)(e.Item.FindControl("tbProvince"));
        HtmlInputHidden tbCity = (HtmlInputHidden)(e.Item.FindControl("tbCity"));
        HtmlInputHidden tbRealityName = (HtmlInputHidden)(e.Item.FindControl("tbRealityName"));
        HtmlInputHidden tbAlipayID = (HtmlInputHidden)(e.Item.FindControl("tbAlipayID"));
        HtmlInputHidden tbAlipayName = (HtmlInputHidden)(e.Item.FindControl("tbAlipayName"));
        HtmlInputHidden tbMemo = (HtmlInputHidden)(e.Item.FindControl("tbMemo"));
        HtmlInputHidden tbIsCps = (HtmlInputHidden)(e.Item.FindControl("tbIsCps"));

        double DistillFormalitiesFeesScale;

        if ((SiteID < 0) || (id < 0) || (UserID < 0))
        {
            PF.GoError(ErrorNumber.Unknow, "参数错误", this.GetType().BaseType.FullName);

            return;
        }

        Users tu = new Users(SiteID)[SiteID, UserID];

        if (tu == null)
        {
            PF.GoError(ErrorNumber.Unknow, "参数错误", this.GetType().BaseType.FullName);

            return;
        }

        if (e.CommandName == "btnNoAccept")
        {
            TextBox tb = (TextBox)e.Item.FindControl("tbMemo1");

            if (tb.Text.Trim() == "")
            {
                Shove._Web.JavaScript.Alert(this.Page, "请输入拒绝理由。");

                return;
            }

            string ReturnDescription = "";

            if (tu.DistillNoAccept(id, tb.Text.Trim(), _User.ID, ref ReturnDescription) < 0)
            {
                PF.GoError(ErrorNumber.Unknow, ReturnDescription, this.GetType().BaseType.FullName);

                return;
            }

            BindData();

            return;
        }

        if (e.CommandName == "btnAccept")
        {
            TextBox tb = (TextBox)e.Item.FindControl("tbMemo2");

            if (tu.Freeze < Money)
            {
                Shove._Web.JavaScript.Alert(this.Page, "用户冻结账户余额不足以提款。");

                return;
            }

            //吴波修改..
            //主要出现的问题是:后台处理提款的时候,若是支付宝用户则会出现 "用户没有输入开户名"，"用户没有输入开户银行"，"用户没有输入银行卡号" 的提示
            if (tbBankCardNumber.Value != "")
            {
                if (tbBankUserName.Value == "")
                {
                    Shove._Web.JavaScript.Alert(this.Page, "用户没有输入开户名。");

                    return;
                }
                if (tbBankName.Value == "")
                {
                    Shove._Web.JavaScript.Alert(this.Page, "用户没有输入开户银行。");

                    return;
                }
                if (tbBankCardNumber.Value == "")
                {
                    Shove._Web.JavaScript.Alert(this.Page, "用户没有输入银行卡号。");

                    return;
                }
            }
            //添加了 判断支付宝账号的验证
            else
            {
                if (tbAlipayName.Value == "" )
                {
                    Shove._Web.JavaScript.Alert(this.Page, "用户没有输入支付宝账号");

                    return;
                }

            }

            if (tbAlipayName.Value != "")
            {
                DistillFormalitiesFeesScale = so["OnlinePay_Alipay_ForUserDistill_MD5Key_ForPayOut"].ToDouble(0);
            }
            else
            {
                DistillFormalitiesFeesScale = so["OnlinePayOut_99Bill_DistillFormalitiesFeesScale"].ToDouble(0);
            }

            if (DistillFormalitiesFeesScale >= 1)
            {
                Shove._Web.JavaScript.Alert(this.Page, "提款手续费比例设置错误。");

                return;
            }

            string ReturnDescription = "";

            if (tu.DistillAccept(id, tbBankUserName.Value, tbBankName.Value, tbBankCardNumber.Value, tbAlipayID.Value, tbAlipayName.Value, tb.Text.Trim(), _User.ID, ref ReturnDescription) < 0)
            {
                Shove._Web.JavaScript.Alert(this.Page, "出错:" + ReturnDescription);

                return;

            }

            Shove._Web.JavaScript.Alert(this.Page, "已经接受提款! 请到待付款一览表进行付款操作.");
            BindData();

            return;
        }
    }
}
