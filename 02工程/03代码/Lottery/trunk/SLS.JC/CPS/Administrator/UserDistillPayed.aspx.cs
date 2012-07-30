using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Shove.Database;

public partial class Cps_Administrator_UserDistillPayed :AdminPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!this.IsPostBack)
        {
            tbBeginTime.Text = (new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1)).ToString("yyyy-MM-dd");
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
        RequestLoginPage = Shove._Web.Utility.GetUrl() + "/Cps/Administrator/Default.aspx";


        RequestCompetences = Competences.BuildCompetencesList(Competences.Finance);

        base.OnInit(e);
    }

    #endregion

    private void BindBankType()
    {
        DataTable dt = (new DAL.Tables.T_Banks()).Open("", "", "[Name]");

        ddlAccountType.DataSource = dt;
        ddlAccountType.DataTextField = "Name";
        ddlAccountType.DataValueField = "Name";
        ddlAccountType.DataBind();

        ddlAccountType.Items.Insert(0, "");
        ddlAccountType.Items.Add(new ListItem("支付宝", "支付宝"));

        ddlAccountType.Items.Add(new ListItem("快钱", "快钱"));

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

        string condition = GetFilterCondition();
        DataTable dt = new DAL.Views.V_UserDistills().Open("", condition, "[DateTime] desc");

        if (dt == null)
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", "Admin_UserDistillSuccess");

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
        for (int i = 0; i < g.Columns.Count; i++)
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
            else if (g.Columns[i].HeaderText == "快钱账号")
            {
                g.Columns[i].Visible = true;
                g.Columns[i].Visible = (hfCurPayType.Value == "所有" || hfCurPayType.Value == "快钱") ? true : false;
            }
        }
    }

    private string GetFilterCondition()
    {
        string condiction = " IsCps = 1 and Result=1 ";
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
        else if (hfCurPayType.Value == "快钱")
        {
            condiction += " and DistillType =3 ";
        }

        if (tbUserName.Text.Trim() != "")
        {
            condiction += " and Name='" + Shove._Web.Utility.FilteSqlInfusion(tbUserName.Text.Trim()) + "' ";
        }

        if (ddlAccountType.Text.Trim() != "" && ddlAccountType.Text.Trim() != "支付宝" && ddlAccountType.Text.Trim() != "快钱")
        {
            condiction += " and BankTypeName = '" + ddlAccountType.Text + "'";//银行类型
        }

        return condiction;
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

    protected void lbtnPayByAlipay_Click(object sender, EventArgs e)
    {
        hfCurPayType.Value = "支付宝";
        BindData(false);

        PayBy99Bill.Attributes["class"] = "NotSelectedTab";
        PayByAlipay.Attributes["class"] = "SelectedTab";
        PayByBank.Attributes["class"] = "NotSelectedTab";
        AllPay.Attributes["class"] = "NotSelectedTab";
    }

    protected void lbtnPayBy99Bill_Click(object sender, EventArgs e)
    {
        hfCurPayType.Value = "快钱";
        BindData(false);

        PayBy99Bill.Attributes["class"] = "SelectedTab";
        PayByAlipay.Attributes["class"] = "NotSelectedTab";
        PayByBank.Attributes["class"] = "NotSelectedTab";
        AllPay.Attributes["class"] = "NotSelectedTab";
    }

    protected void lbtnPayByBank_Click(object sender, EventArgs e)
    {
        hfCurPayType.Value = "银行卡";
        BindData(false);

        PayBy99Bill.Attributes["class"] = "NotSelectedTab";
        PayByAlipay.Attributes["class"] = "NotSelectedTab";
        PayByBank.Attributes["class"] = "SelectedTab";
        AllPay.Attributes["class"] = "NotSelectedTab";
    }

    protected void lbtnAllPay_Click(object sender, EventArgs e)
    {
        hfCurPayType.Value = "所有";
        BindData(false);

        PayBy99Bill.Attributes["class"] = "NotSelectedTab";
        PayByAlipay.Attributes["class"] = "NotSelectedTab";
        PayByBank.Attributes["class"] = "NotSelectedTab";
        AllPay.Attributes["class"] = "SelectedTab";
    }

    protected void ddlAccountType_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindData(false);
    }
}
