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

public partial class Admin_Competence : AdminPageBase
{
    private CheckBox[] cbGroup = new CheckBox[10];
    private CheckBox[] cbCompetence = new CheckBox[19];

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            BindData();

            btnSave.Visible = _User.Competences.IsOwnedCompetences(Competences.BuildCompetencesList(Competences.Competence));
            btnAdd.Visible = btnSave.Visible;
        }
    }

    #region Web 窗体设计器生成的代码

    override protected void OnInit(EventArgs e)
    {
        RequestLoginPage = this.Request.Url.AbsoluteUri;

        RequestCompetences = Competences.BuildCompetencesList(Competences.Competence,Competences.QueryData);

        base.OnInit(e);
    }

    #endregion

    private void BindData()
    {
        DataTable dt = DAL.Functions.F_GetManagers(_Site.ID);

        if (dt == null)
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", "Admin_Competence");

            return;
        }

        lbUser.DataSource = dt;
        lbUser.DataTextField = "Name";
        lbUser.DataValueField = "ID";

        lbUser.DataBind();

        if (lbUser.Items.Count > 0)
        {
            lbUser.SelectedIndex = 0;
            lbUser_SelectedIndexChanged(lbUser, new EventArgs());
        }
    }

    private void BindCheckBox()
    {
        for (int i = 0; i < 10; i++)
        {
            cbGroup[i] = (CheckBox)this.FindControl("cbGroup" + (i + 1).ToString());
        }

        for (int i = 0; i < 19; i++)
        {
            cbCompetence[i] = (CheckBox)this.FindControl("cbCompetence" + (i + 1).ToString());
        }
    }

    private void SetCheckBoxChecked(bool Value)
    {
        if ((cbGroup[0] == null) || (cbCompetence[0] == null))
        {
            BindCheckBox();
        }

        for (int i = 0; i < 10; i++)
        {
            cbGroup[i].Checked = Value;
        }

        for (int i = 0; i < 19; i++)
        {
            cbCompetence[i].Checked = Value;
        }
    }

    private void SetCheckBoxEnabled(bool Value)
    {
        if ((cbGroup[0] == null) || (cbCompetence[0] == null))
        {
            BindCheckBox();
        }

        for (int i = 0; i < 10; i++)
        {
            cbGroup[i].Enabled = Value;
        }

        for (int i = 0; i < 19; i++)
        {
            cbCompetence[i].Enabled = Value;
        }
    }

    protected void lbUser_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (lbUser.Items.Count < 1)
        {
            return;
        }

        BindCheckBox();
        SetCheckBoxChecked(false);

        DataTable dt = new DAL.Tables.T_UserInGroups().Open("distinct GroupID", "UserID=" +  Shove._Web.Utility.FilteSqlInfusion(lbUser.SelectedValue), "");

        if (dt == null)
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", "Admin_Competence");

            return;
        }

        foreach (DataRow dr in dt.Rows)
        {
            cbGroup[int.Parse(dr["GroupID"].ToString()) - 1].Checked = true;
        }

        dt = new DAL.Tables.T_CompetencesOfUsers().Open("distinct CompetenceID", "UserID=" +  Shove._Web.Utility.FilteSqlInfusion(lbUser.SelectedValue), "");

        if (dt == null)
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", "Admin_Competence");

            return;
        }

        foreach (DataRow dr in dt.Rows)
        {
            cbCompetence[int.Parse(dr["CompetenceID"].ToString()) - 1].Checked = true;
        }

        // 超级管理员的权限不能修改。
        if (_Site.AdministratorID == long.Parse(lbUser.SelectedValue))  //if ((_Site.Level == SiteLevels.MasterSite) && (lbUser.SelectedValue == "1"))
        {
            btnSave.Enabled = false;

            SetCheckBoxEnabled(false);
        }
        else
        {
            btnSave.Enabled = true;

            SetCheckBoxEnabled(true);
        }
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        if (tbUserName.Text.Trim() == "")
        {
            Shove._Web.JavaScript.Alert(this.Page, "用户名不能为空！");

            return;
        }

        if (lbUser.Items.FindByText(tbUserName.Text.Trim()) != null)
        {
            Shove._Web.JavaScript.Alert(this.Page, "用户名已经存在！");

            return;
        }

        Users tu = new Users(_Site.ID)[_Site.ID, tbUserName.Text.Trim()];

        if (tu == null)
        {
            Shove._Web.JavaScript.Alert(this.Page, "用户名不存在！");

            return;
        }

        lbUser.Items.Add(new ListItem(tu.Name, tu.ID.ToString()));

        lbUser.SelectedIndex = lbUser.Items.Count - 1;
        lbUser_SelectedIndexChanged(lbUser, new EventArgs());
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (lbUser.Items.Count < 1)
        {
            return;
        }

        BindCheckBox();

        long UserID = long.Parse(lbUser.SelectedValue);

        string GroupList = "";

        for (int i = 0; i < 10; i++)
        {
            if (cbGroup[i].Checked)
            {
                GroupList += "[" + (i + 1).ToString() + "]";
            }
        }

        string CompetenceList = "";

        for (int i = 0; i < 19; i++)
        {
            if (cbCompetence[i].Checked)
            {
                CompetenceList += "[" + (i + 1).ToString() + "]";
            }
        }

        int ReturnValue = -1;
        string ReturnDescription = "";

        int Results = -1;
        Results = DAL.Procedures.P_SetUserCompetences(_Site.ID, UserID, CompetenceList, GroupList, ref ReturnValue, ref ReturnDescription);
        if (Results < 0)
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", "Admin_Competence");

            return;
        }

        if (ReturnValue < 0)
        {
            PF.GoError(ErrorNumber.Unknow, ReturnDescription, "Admin_Competence");

            return;
        }

        Shove._Web.JavaScript.Alert(this.Page, "权限已经保存。");
    }
}
