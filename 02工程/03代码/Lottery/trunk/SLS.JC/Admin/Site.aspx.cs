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

public partial class Admin_Site : AdminPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            BindData();

            btnOK.Visible = _User.Competences.IsOwnedCompetences(Competences.BuildCompetencesList(Competences.Options));
        }
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
        tbName.Text = _Site.Name;
        imgON.ImageUrl = (_Site.ON ? "../Images/Admin/Run.gif" : "../Images/Admin/Stop.gif");
        imgON.ToolTip = (_Site.ON ? "正在运行" : "已停止");

        tbCompany.Text = _Site.Company;
        tbAddress.Text = _Site.Address;
        tbPostCode.Text = _Site.PostCode;
        tbResponsiblePerson.Text = _Site.ResponsiblePerson;
        tbContactPerson.Text = _Site.ContactPerson;
        tbTelephone.Text = _Site.Telephone;
        tbFax.Text = _Site.Fax;
        tbMobile.Text = _Site.Mobile;
        tbEmail.Text = _Site.Email;
        tbQQ.Text = _Site.QQ;
        tbServiceTelephone.Text = _Site.ServiceTelephone;
        tbICPCert.Text = _Site.ICPCert;
        tbBonusScale.Text = _Site.BonusScale.ToString();
        tbMaxSubSites.Text = _Site.MaxSubSites.ToString();
        tbUrls.Text = _Site.Urls;

        BindDataForLottery();
        BindDataForLotteryQuickBuy();

        tbCompany.Enabled = true;
        tbResponsiblePerson.Enabled = true;
    }

    private void BindDataForLottery()
    {
        DataTable dt = new DAL.Tables.T_Lotteries().Open("[ID], [Name]", "[ID] in (" + (_Site.UseLotteryListRestrictions == "" ? "-1" : _Site.UseLotteryListRestrictions) + ")", "[ID]");

        if (dt == null)
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", this.GetType().BaseType.FullName);

            return;
        }

        g.DataSource = dt;
        g.DataBind();

        g.Columns[2].Visible = false;

        // 选中已经设置的彩种
        for (int i = 0; i < g.Rows.Count; i++)
        {
            CheckBox cbisUsed = (CheckBox)g.Rows[i].Cells[1].FindControl("cbisUsed");

            if (_Site.UseLotteryListRestrictions == "")
            {
                cbisUsed.Checked = false;

                continue;
            }

            int LotteryID = Shove._Convert.StrToInt(g.Rows[i].Cells[2].Text, -1);

            if (LotteryID < 1)
            {
                continue;
            }

            cbisUsed.Checked = (("," + _Site.UseLotteryList + ",").IndexOf("," + LotteryID.ToString() + ",") >= 0);
        }
    }

    private void BindDataForLotteryQuickBuy()
    {
        DataTable dt = new DAL.Tables.T_Lotteries().Open("[ID], [Name]", "[ID] in (" + (_Site.UseLotteryListRestrictions == "" ? "-1" : _Site.UseLotteryListRestrictions) + ")", "[ID]");

        if (dt == null)
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", this.GetType().BaseType.FullName);

            return;
        }

        g2.DataSource = dt;
        g2.DataBind();

        g2.Columns[2].Visible = false;

        // 选中已经设置的快速投注彩种
        for (int i = 0; i < g2.Rows.Count; i++)
        {
            CheckBox cbisUsed = (CheckBox)g2.Rows[i].Cells[1].FindControl("cbisUsed");

            if (_Site.UseLotteryListRestrictions == "")
            {
                cbisUsed.Checked = false;

                continue;
            }

            int LotteryID = Shove._Convert.StrToInt(g2.Rows[i].Cells[2].Text, -1);

            if (LotteryID < 1)
            {
                continue;
            }

            cbisUsed.Checked = (("," + _Site.UseLotteryListQuickBuy + ",").IndexOf("," + LotteryID.ToString() + ",") >= 0);
        }
    }

    protected void btnOK_Click(object sender, EventArgs e)
    {
        #region 判断输入是否完整

        if (tbCompany.Text.Trim() == "")
        {
            Shove._Web.JavaScript.Alert(this.Page, "请输入公司名称。");

            return;
        }

        if (tbAddress.Text.Trim() == "")
        {
            Shove._Web.JavaScript.Alert(this.Page, "请输入公司地址。");

            return;
        }

        if (tbPostCode.Text.Trim() == "")
        {
            Shove._Web.JavaScript.Alert(this.Page, "请输入邮政编码。");

            return;
        }

        if (tbResponsiblePerson.Text.Trim() == "")
        {
            Shove._Web.JavaScript.Alert(this.Page, "请输入负责人姓名。");

            return;
        }

        if (tbContactPerson.Text.Trim() == "")
        {
            Shove._Web.JavaScript.Alert(this.Page, "请输入联系人姓名。");

            return;
        }

        if (tbTelephone.Text.Trim() == "")
        {
            Shove._Web.JavaScript.Alert(this.Page, "请输入电话号码。");

            return;
        }

        if (tbMobile.Text.Trim() == "")
        {
            Shove._Web.JavaScript.Alert(this.Page, "请输入手机号码。");

            return;
        }

        if (tbEmail.Text.Trim() == "")
        {
            Shove._Web.JavaScript.Alert(this.Page, "请输入有效的 Email。");

            return;
        }

        if (tbServiceTelephone.Text.Trim() == "")
        {
            Shove._Web.JavaScript.Alert(this.Page, "请输入技术服务电话号码。");

            return;
        }

        if (tbUrls.Text.Trim() == "")
        {
            Shove._Web.JavaScript.Alert(this.Page, "请输入站点域名列表。");

            return;
        }

        #endregion

        // 开始保存
        Sites ts = new Sites();
        _Site.Clone(ts);

        _Site.Name = tbName.Text.Trim();
        _Site.Company = tbCompany.Text.Trim();
        _Site.ResponsiblePerson = tbResponsiblePerson.Text.Trim();
        _Site.Address = tbAddress.Text.Trim();
        _Site.PostCode = Shove._Convert.ToDBC(tbPostCode.Text.Trim()).Trim();
        _Site.ContactPerson = tbContactPerson.Text.Trim();
        _Site.Telephone = Shove._Convert.ToDBC(tbTelephone.Text.Trim()).Trim();
        _Site.Fax = Shove._Convert.ToDBC(tbFax.Text.Trim()).Trim();
        _Site.Mobile = Shove._Convert.ToDBC(tbMobile.Text.Trim()).Trim();
        _Site.Email = tbEmail.Text.Trim();
        _Site.QQ = Shove._Convert.ToDBC(tbQQ.Text.Trim()).Trim();
        _Site.ServiceTelephone = Shove._Convert.ToDBC(tbServiceTelephone.Text.Trim()).Trim();
        _Site.ICPCert = tbICPCert.Text.Trim();
        _Site.Urls = Shove._Convert.ToDBC(tbUrls.Text).Trim();

        _Site.UseLotteryList = "";

        for (int i = 0; i < g.Rows.Count; i++)
        {
            if (((CheckBox)g.Rows[i].Cells[1].FindControl("cbisUsed")).Checked)
            {
                string LotteryID = g.Rows[i].Cells[2].Text;

                _Site.UseLotteryList += (_Site.UseLotteryList == "" ? "" : ",") + LotteryID;
            }
        }

        _Site.UseLotteryListQuickBuy = "";

        for (int i = 0; i < g2.Rows.Count; i++)
        {
            if (((CheckBox)g2.Rows[i].Cells[1].FindControl("cbisUsed")).Checked)
            {
                string LotteryID = g2.Rows[i].Cells[2].Text;

                _Site.UseLotteryListQuickBuy += (_Site.UseLotteryListQuickBuy == "" ? "" : ",") + LotteryID;
            }
        }

        string ReturnDescription = "";

        if (_Site.EditByID(ref ReturnDescription) < 0)
        {
            ts.Clone(_Site);
            Shove._Web.JavaScript.Alert(this.Page, ReturnDescription);

            return;
        }

        Shove._Web.Cache.ClearCache(_Site.ID.ToString() + "Lotteries");
        Shove._Web.Cache.ClearCache("Site_UseLotteryList" + _Site.ID);

        Shove._Web.JavaScript.Alert(this.Page, "站点资料已经保存成功。");
    }
}
