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

public partial class Admin_UserDistillSuccess : AdminPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        
        if (!this.IsPostBack)
        {
            tbBeginTime.Text=(new DateTime(DateTime.Now.Year,DateTime.Now.Month,1)).ToString("yyyy-MM-dd");
            tbEndTime.Text = DateTime.Now.ToString("yyyy-MM-dd");
            
            BindBankType();

            //初始情况
            hfCurPayType.Value = "支付宝";
            PayByAlipay.Attributes["class"] = "SelectedTab";
            PayByBank.Attributes["class"] = "NotSelectedTab";
            AllPay.Attributes["class"] = "NotSelectedTab";
            BindData(true);

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

    private void BindData(bool IsReload)
    {
        lblTotalDistillMoney.Text = "0";
        lblTotalFormalitiesFees.Text = "0";
        lblTotalPayMoney.Text = "0";

        DateTime fromDate = DateTime.Now;
        DateTime toDate = DateTime.Now;
        if (!DateTime.TryParse(tbBeginTime.Text, out fromDate) || !DateTime.TryParse(tbEndTime.Text, out toDate))
        {
            Shove._Web.JavaScript.Alert(this.Page, "请输入正确的日期范围!");
            return;
        }

        DataTable dt;
        string cacheKey = "Admin_UserDistillSuccess_Data_" + tbBeginTime.Text + "_" + tbEndTime.Text;
       
        string condition = GetFilterCondition();
        dt = new DAL.Views.V_UserDistills().Open("", condition, "[DateTime] desc");
        if (dt == null)
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", "Admin_FinanceDistill");

            return;
        }

        //统计 合计值
        
        
        double totalDistillMoney = 0;
        double totalFormalitiesFees = 0;
        for (int row = 0; row < dt.Rows.Count; row++)
        {
            totalDistillMoney += Shove._Convert.StrToDouble(dt.Rows[row]["Money"].ToString(), 0);
            totalFormalitiesFees += Shove._Convert.StrToDouble(dt.Rows[row]["FormalitiesFees"].ToString(), 0);
        }
        lblTotalDistillMoney.Text = totalDistillMoney.ToString();
        lblTotalFormalitiesFees.Text = totalFormalitiesFees.ToString();
        lblTotalPayMoney.Text = (totalDistillMoney - totalFormalitiesFees).ToString();

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
        string condiction = " Result=1 ";
        if (tbBeginTime.Text.Trim() != "" && tbEndTime.Text.Trim() != "")
        {
            DateTime fromDate = DateTime.Now;
            DateTime toDate = DateTime.Now;
            if (DateTime.TryParse(tbBeginTime.Text, out fromDate) && DateTime.TryParse(tbEndTime.Text, out toDate))
            {
                if (rbByDistillTime.Checked)//按申请提款时间
                {
                    condiction += " and ( DateTime >= '" + fromDate.ToString("yyyy-MM-dd") + "' and DateTime <= '" + toDate.ToString("yyyy-MM-dd") + " 23:59:59' )";
                }
                else //按最后处理时间(付款时间)
                {
                    condiction += " and ( HandleDateTime >= '" + fromDate.ToString("yyyy-MM-dd") + "' and HandleDateTime <= '" + toDate.ToString("yyyy-MM-dd") + " 23:59:59' )";
                }
            }
            else
            {
                Shove._Web.JavaScript.Alert(this.Page, "请输入正确的日期格式:2008-8-8");
            }
        }

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
        BindData(false);
    }

    protected void gPager_SortBefore(object source, DataGridSortCommandEventArgs e)
    {
        BindData(false);
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        if (tbBeginTime.Text.Trim() != "" && tbEndTime.Text.Trim() != "")
        {
            DateTime fromDate = DateTime.Now;
            DateTime toDate = DateTime.Now;
            if (!DateTime.TryParse(tbBeginTime.Text, out fromDate) && !DateTime.TryParse(tbEndTime.Text, out toDate))
            {
                Shove._Web.JavaScript.Alert(this.Page, "请输入正确的日期格式:2008-8-8");
                return;
            }
        }

        BindData(false);
    }


    protected void btnDownLoadExcel_Click(object sender, EventArgs e)
    {
        //DataView dv =(DataView) g.;
    }

    protected void lbtnPayByAlipay_Click(object sender, EventArgs e)
    {
        hfCurPayType.Value = "支付宝";
        BindData(false);

        PayByAlipay.Attributes["class"] = "SelectedTab";
        PayByBank.Attributes["class"] = "NotSelectedTab";
        AllPay.Attributes["class"] = "NotSelectedTab";
    }
    
    protected void lbtnPayByBank_Click(object sender, EventArgs e)
    {
        hfCurPayType.Value = "银行卡";
        BindData(false);

        PayByAlipay.Attributes["class"] = "NotSelectedTab";
        PayByBank.Attributes["class"] = "SelectedTab";
        AllPay.Attributes["class"] = "NotSelectedTab";
    }
    protected void lbtnAllPay_Click(object sender, EventArgs e)
    {
        hfCurPayType.Value = "所有";
        BindData(false);

        PayByAlipay.Attributes["class"] = "NotSelectedTab";
        PayByBank.Attributes["class"] = "NotSelectedTab";
        AllPay.Attributes["class"] = "SelectedTab";
    }
    protected void ddlAccountType_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindData(false);
    }
}
