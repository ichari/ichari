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

public partial class Admin_UserDistillProcessing : AdminPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            
            BindBankType();

            //初始情况
            hfCurPayType.Value = "支付宝";
            PayByAlipay.Attributes["class"] = "SelectedTab";
            PayByBank.Attributes["class"] = "NotSelectedTab";
            AllPay.Attributes["class"] = "NotSelectedTab";
            BindData();

        }
    }

    #region Web 窗体设计器生成的代码

    override protected void OnInit(EventArgs e)
    {
        isRequestLogin = true;
        RequestLoginPage = this.Request.Url.AbsoluteUri;

        RequestCompetences = Competences.BuildCompetencesList(Competences.Finance);

        base.OnInit(e);
    }

    #endregion

    private void BindBankType()
    {
        string cacheKeyBankType = "Admin_UserDistillUnsuccess_BankType";
        DataTable dt ;
        dt = Shove._Web.Cache.GetCacheAsDataTable(cacheKeyBankType);
        if (dt == null)
        {
            dt = (new DAL.Tables.T_Banks()).Open("", "", "[Name]");
        }
        ddlAccountType.DataSource = dt;
        ddlAccountType.DataTextField = "Name";
        ddlAccountType.DataValueField = "Name";
        ddlAccountType.DataBind();

        ddlAccountType.Items.Insert(0,"");
        ddlAccountType.Items.Add("支付宝");

    }

    private void BindData()
    {
       
        DataTable dt;

        string condition = GetFilterCondition();
        dt = new DAL.Views.V_UserDistills().Open("", condition, "[DateTime] desc");



        if (dt == null)
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", "Admin_FinanceDistill");

            return;
        }

        PF.DataGridBindData(g, dt, gPager);

        //根据条件显示或隐藏列
        for(int i=0;i<g.Columns.Count;i++)
        {
            if (g.Columns[i].HeaderText == "提款银行卡帐号" || g.Columns[i].HeaderText == "开户银行" || g.Columns[i].HeaderText == "支行名称"
               || g.Columns[i].HeaderText == "所在省" || g.Columns[i].HeaderText == "所在市" || g.Columns[i].HeaderText == "持卡人姓名")
            {
                g.Columns[i].Visible = (hfCurPayType.Value == "所有" || hfCurPayType.Value == "银行卡") ? true : false;
            }
            else if (g.Columns[i].HeaderText == "支付宝账号")
            {
                g.Columns[i].Visible = true;
                g.Columns[i].Visible = (hfCurPayType.Value == "所有" || hfCurPayType.Value == "支付宝") ? true : false;
            }
        }
            
 
    }

    private string GetFilterCondition()
    {
        string condiction = " Result=11 ";
        if (hfCurPayType.Value == "银行卡")//提款到银行卡
        {
            condiction += " and DistillType =2 ";
        }
        else if (hfCurPayType.Value == "支付宝")//不是银行就是:支付宝
        {
            condiction += " and DistillType =1 ";
        }

        if (tbUserName.Text.Trim() != "")
        {
            condiction += " and Name='" + Shove._Web.Utility.FilteSqlInfusion(tbUserName.Text.Trim()) + "' ";
        }

        if (ddlAccountType.Text.Trim() != "")
        {
            if (ddlAccountType.Text != "支付宝")
            {
                condiction += " and BankTypeName = '" + ddlAccountType.Text + "'";//银行类型
            }
            else
            {
                condiction += " and DistillType =1 ";//支付宝
            }
        }

        return condiction;
    }

    protected void g_ItemDataBound(object sender, DataGridItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.EditItem)
        {

        }
    }

    protected void gPager_PageWillChange(object Sender, Shove.Web.UI.PageChangeEventArgs e)
    {
        BindData();
    }

    protected void gPager_SortBefore(object source, DataGridSortCommandEventArgs e)
    {
        BindData();
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {


        BindData();
    }


    protected void btnDownLoadExcel_Click(object sender, EventArgs e)
    {
        //DataView dv =(DataView) g.;
    }

    protected void lbtnPayByAlipay_Click(object sender, EventArgs e)
    {
        hfCurPayType.Value = "支付宝";
        BindData();

        PayByAlipay.Attributes["class"] = "SelectedTab";
        PayByBank.Attributes["class"] = "NotSelectedTab";
        AllPay.Attributes["class"] = "NotSelectedTab";
    }
    
    protected void lbtnPayByBank_Click(object sender, EventArgs e)
    {
        hfCurPayType.Value = "银行卡";
        BindData();

        PayByAlipay.Attributes["class"] = "NotSelectedTab";
        PayByBank.Attributes["class"] = "SelectedTab";
        AllPay.Attributes["class"] = "NotSelectedTab";
    }
    protected void lbtnAllPay_Click(object sender, EventArgs e)
    {
        hfCurPayType.Value = "所有";
        BindData();

        PayByAlipay.Attributes["class"] = "NotSelectedTab";
        PayByBank.Attributes["class"] = "NotSelectedTab";
        AllPay.Attributes["class"] = "SelectedTab";
    }
    protected void ddlAccountType_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindData();
    }

    protected void g_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        long distillID = Shove._Convert.StrToLong(g.DataKeys[e.Item.ItemIndex].ToString(), -1);
        long distillUserID = Shove._Convert.StrToLong(e.Item.Cells[0].Text, -1);
        if (e.CommandName == "Pay")//确认已经线下付款
        {
            int results = -1;
            int returnValue = 0;
            string returnDescription = "";
            results = DAL.Procedures.P_DistillPaySuccess(_Site.ID, distillUserID, distillID, "已付款", _User.ID, ref returnValue, ref returnDescription);
            if (results < 0)
            {
                Shove._Web.JavaScript.Alert(this.Page, "出错!请联技术人员!");
                return;
            }
            if (returnValue < 0)
            {
                Shove._Web.JavaScript.Alert(this.Page, "出错!请联技术人员:" + returnDescription);
                return;
            }

            BindData();
            Shove._Web.JavaScript.Alert(this.Page, "操作成功.");
        }
    }
}
