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

public partial class Admin_UserDistillGeneralLedger : AdminPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            

            //初始化
            tbBeginTime.Text = DateTime.Now.ToString("yyyy-MM") + "-1";
            tbEndTime.Text = DateTime.Now.ToString("yyyy-MM-dd");

            hfCurPayType.Value = "提款对帐单";//初始显示的标签内容
            DuiZhangDan.Visible = true;
            BankDetail.Visible = false;
            tdDuiZhangDan.Attributes["class"] = "SelectedTab";
            tdBankDetail.Attributes["class"] = "NotSelectedTab";

            BindDuiZhangDanData(false);

            BindBankDetailData(false);
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

    private void BindDuiZhangDanData(bool IsReload)
    {
        DateTime fromDate=DateTime.Now;
        DateTime toDate=DateTime.Now;
        if (tbBeginTime.Text.Trim() != "" && tbEndTime.Text.Trim()!="")
        {
            if (!DateTime.TryParse(tbBeginTime.Text.Trim(), out fromDate) || !DateTime.TryParse(tbEndTime.Text.Trim()+" 23:59:59", out toDate))
            {
                Shove._Web.JavaScript.Alert(this.Page, "请输入正确的日期格式:2009-08-08");
                return;
            }

        }


        string cacheKey_DuiZhangDan_StatisticDistillType = "Admin_UserDistillGeneralLedger_" + fromDate.ToString("yyyyMMdd") + toDate.ToString("yyyyMMdd");
        DataTable dt1 = Shove._Web.Cache.GetCacheAsDataTable(cacheKey_DuiZhangDan_StatisticDistillType);
        if (dt1 == null || IsReload)
        {
            DataSet ds=null;
            int returnValue=0;
            string returnDescription="";
            if (DAL.Procedures.P_GetDistillStatisticByDistillType(ref ds, _Site.ID, fromDate, toDate, ref returnValue, ref returnDescription) < 0)
            {
                PF.GoError(ErrorNumber.DataReadWrite, "数据读写错误", this.GetType().BaseType.FullName);
                return;
            }
            if (returnValue < 0 || returnDescription != "")
            {
                Shove._Web.JavaScript.Alert(this.Page, returnDescription);
                return;
            }
            dt1 = ds.Tables[0];
            Shove._Web.Cache.SetCache(cacheKey_DuiZhangDan_StatisticDistillType, dt1);
        }
        PF.DataGridBindData(dgDuiZhangDan, dt1);

        //绑定冲值和提款总额
        string cacheKey_DuiZhangDan_GetDistillMoneyAndAddMoney = "Admin_UserDistillGeneralLedger_cacheKeyGetDistillMoneyAndAddMoney_" + fromDate.ToString("yyyyMMdd") + toDate.ToString("yyyyMMdd");
        DataTable dt2 = Shove._Web.Cache.GetCacheAsDataTable(cacheKey_DuiZhangDan_GetDistillMoneyAndAddMoney);
        if (dt2 == null || IsReload)
        {
            DataSet ds = null;
            int returnValue = 0;
            string returnDescription = "";
            if (DAL.Procedures.P_GetDistillMoneyAndAddMoney(ref ds, _Site.ID, fromDate, toDate,0, ref returnValue, ref returnDescription) < 0)
            {
                PF.GoError(ErrorNumber.DataReadWrite, "数据读写错误", this.GetType().BaseType.FullName);
                return;
            }
            if (returnValue < 0 || returnDescription != "")
            {
                Shove._Web.JavaScript.Alert(this.Page, returnDescription);
                return;
            }
            dt2 = ds.Tables[0];
            Shove._Web.Cache.SetCache(cacheKey_DuiZhangDan_GetDistillMoneyAndAddMoney, dt2);
        }

        
        lblSumAddMoneyByDate1.Text = Shove._Convert.StrToDouble(dt2.Rows[0]["SumAddMoneyByDate"].ToString(), 0).ToString("N2");
        lblSumAddMoney1.Text = Shove._Convert.StrToDouble(dt2.Rows[0]["SumAddMoney"].ToString(), 0).ToString("N2");

        
        lblSumDistillMoneyByDate1.Text = Shove._Convert.StrToDouble(dt2.Rows[0]["SumDistillMoneyByDate"].ToString(), 0).ToString("N2");
        lblSumDistillMoney1.Text = Shove._Convert.StrToDouble(dt2.Rows[0]["SumDistillMoney"].ToString(), 0).ToString("N2");

        lblSumFormalitiesFeesByDate1.Text = Shove._Convert.StrToDouble(dt2.Rows[0]["SumFormalitiesFeesByDate"].ToString(), 0).ToString("N2");
        lblSumFormalitiesFees1.Text = Shove._Convert.StrToDouble(dt2.Rows[0]["SumFormalitiesFees"].ToString(), 0).ToString("N2");
        
    }

    private void BindBankDetailData(bool IsReload)
    {
        DateTime fromDate = DateTime.Now;
        DateTime toDate = DateTime.Now;
        if (tbBeginTime.Text.Trim() != "" && tbEndTime.Text.Trim() != "")
        {
            if (!DateTime.TryParse(tbBeginTime.Text.Trim(), out fromDate) || !DateTime.TryParse(tbEndTime.Text.Trim() + " 23:59:59", out toDate))
            {
                Shove._Web.JavaScript.Alert(this.Page, "请输入正确的日期格式:2009-08-08");
                return;
            }

        }


        string cacheKeyStatisticBankDetailData = "Admin_UserDistillGeneralLedger_BankDetailData_" + fromDate.ToString("yyyyMMdd") + toDate.ToString("yyyyMMdd");
        DataTable dt1 = Shove._Web.Cache.GetCacheAsDataTable(cacheKeyStatisticBankDetailData);
        if (dt1 == null || IsReload)
        {
            DataSet ds = null;
            int returnValue = 0;
            string returnDescription = "";
            if (DAL.Procedures.P_GetDistillStatisticByAccount(ref ds, _Site.ID, fromDate, toDate,"", ref returnValue, ref returnDescription) < 0)
            {
                PF.GoError(ErrorNumber.DataReadWrite, "数据读写错误", this.GetType().BaseType.FullName);
                return;
            }
            if (returnValue < 0 || returnDescription != "")
            {
                Shove._Web.JavaScript.Alert(this.Page, returnDescription);
                return;
            }
            dt1 = ds.Tables[0];
            Shove._Web.Cache.SetCache(cacheKeyStatisticBankDetailData, dt1);
        }
        PF.DataGridBindData(dgBankDetail, dt1);


        //绑定冲值和提款总额
        string cacheKey_BankDetailData_GetDistillMoneyAndAddMoney = "Admin_UserDistillGeneralLedger_cacheKeyGetDistillMoneyAndAddMoney_" + fromDate.ToString("yyyyMMdd") + toDate.ToString("yyyyMMdd");
        DataTable dt2 = Shove._Web.Cache.GetCacheAsDataTable(cacheKey_BankDetailData_GetDistillMoneyAndAddMoney);
        if (dt2 == null || IsReload)
        {
            DataSet ds = null;
            int returnValue = 0;
            string returnDescription = "";
            if (DAL.Procedures.P_GetDistillMoneyAndAddMoney(ref ds, _Site.ID, fromDate, toDate, 0, ref returnValue, ref returnDescription) < 0)
            {
                PF.GoError(ErrorNumber.DataReadWrite, "数据读写错误", this.GetType().BaseType.FullName);
                return;
            }
            if (returnValue < 0 || returnDescription != "")
            {
                Shove._Web.JavaScript.Alert(this.Page, returnDescription);
                return;
            }
            dt2 = ds.Tables[0];
            Shove._Web.Cache.SetCache(cacheKey_BankDetailData_GetDistillMoneyAndAddMoney, dt2);
        }

        lblSumAddMoneyByDate2.Text = Shove._Convert.StrToDouble(dt2.Rows[0]["SumAddMoneyByDate"].ToString(), 0).ToString("N2");
        lblSumAddMoney2.Text = Shove._Convert.StrToDouble(dt2.Rows[0]["SumAddMoney"].ToString(), 0).ToString("N2");


        lblSumDistillMoneyByDate2.Text = Shove._Convert.StrToDouble(dt2.Rows[0]["SumDistillMoneyByDate"].ToString(), 0).ToString("N2");
        lblSumDistillMoney2.Text = Shove._Convert.StrToDouble(dt2.Rows[0]["SumDistillMoney"].ToString(), 0).ToString("N2");

        lblSumFormalitiesFeesByDate2.Text = Shove._Convert.StrToDouble(dt2.Rows[0]["SumFormalitiesFeesByDate"].ToString(), 0).ToString("N2");
        lblSumFormalitiesFees2.Text = Shove._Convert.StrToDouble(dt2.Rows[0]["SumFormalitiesFees"].ToString(), 0).ToString("N2");
    }


    protected void btnSearch_Click(object sender, EventArgs e)
    {
        BindDuiZhangDanData(false);
       BindBankDetailData(false);
    }

    //提款对账单
    protected void lbtnDuiZhangDan_Click(object sender, EventArgs e)
    {
        DuiZhangDan.Visible = true;
        BankDetail.Visible = false;

        hfCurPayType.Value = "提款对帐单";//初始显示的标签内容
        tdDuiZhangDan.Attributes["class"] = "SelectedTab";
        tdBankDetail.Attributes["class"] = "NotSelectedTab";
    }
    //银行明细
    protected void lbtnBankDetail_Click(object sender, EventArgs e)
    {
        DuiZhangDan.Visible = false;
        BankDetail.Visible = true;

        hfCurPayType.Value = "银行明细";//初始显示的标签内容
        tdDuiZhangDan.Attributes["class"] = "NotSelectedTab";
        tdBankDetail.Attributes["class"] = "SelectedTab";
    }
   
}
