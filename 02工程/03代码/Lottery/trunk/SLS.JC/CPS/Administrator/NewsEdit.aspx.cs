using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text.RegularExpressions;

public partial class CPS_Administrator_NewsEdit : AdminPageBase
{
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

        RequestCompetences = Competences.BuildCompetencesList(Competences.CpsManage);

        base.OnInit(e);
    }

    #endregion

    private void BindData()
    {
        long NewsID = Shove._Convert.StrToLong(Shove._Web.Utility.GetRequest("id"), -1);

        if (NewsID < 0)
        {
            PF.GoError(ErrorNumber.Unknow, "参数错误", this.GetType().BaseType.FullName);

            return;
        }

        tbID.Text = NewsID.ToString();

        ddlImage.Items.Clear();
        ddlImage.Items.Add("--选择图片--");

        string UploadPath = this.Server.MapPath("../../Private/" + _Site.ID.ToString() + "/NewsImages");

        if (!System.IO.Directory.Exists(UploadPath))
        {
            System.IO.Directory.CreateDirectory(UploadPath);
        }
        else
        {
            string[] FileList = Shove._IO.File.GetFileList(UploadPath);

            if (FileList != null)
            {
                for (int i = 0; i < FileList.Length; i++)
                {
                    ddlImage.Items.Add(FileList[i]);
                }
            }
        }

        DataTable dt = new DAL.Tables.T_News().Open("", "SiteID = " + _Site.ID.ToString() + " and [ID] = " + NewsID.ToString(), "");

        if (dt == null)
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", this.GetType().BaseType.FullName);

            return;
        }

        if (dt.Rows.Count > 0)
        {
            tbDateTime.Text = dt.Rows[0]["DateTime"].ToString();
            tbReadCount.Text = dt.Rows[0]["ReadCount"].ToString();
            cbisShow.Checked = Shove._Convert.StrToBool(dt.Rows[0]["isShow"].ToString(), true);
            cbisCanComments.Checked = Shove._Convert.StrToBool(dt.Rows[0]["isCanComments"].ToString(), true);
            cbisCommend.Checked = Shove._Convert.StrToBool(dt.Rows[0]["isCommend"].ToString(), false);
            cbisHot.Checked = Shove._Convert.StrToBool(dt.Rows[0]["isHot"].ToString(), false);
            tbTitle.Text = dt.Rows[0]["Title"].ToString();
            if (tbTitle.Text.IndexOf("<font class=red12>") > -1)
            {
                ddlTitleColor.Items[1].Selected = true;
                ddlTitleColor.Items[2].Selected = false;
                ddlTitleColor.Items[0].Selected = false;
                tbTitle.Text = tbTitle.Text.Replace("<font class=red12>", "").Replace("</font>", "");
            }
            else if (tbTitle.Text.IndexOf("<font class=black12>") > -1)
            {
                ddlTitleColor.Items[2].Selected = true;
                ddlTitleColor.Items[1].Selected = false;
                ddlTitleColor.Items[0].Selected = false;
                tbTitle.Text = tbTitle.Text.Replace("<font class=black12>", "").Replace("</font>", "");
            }
            else
            {
                ddlTitleColor.Items[0].Selected = true;
                ddlTitleColor.Items[2].Selected = false;
                ddlTitleColor.Items[1].Selected = false;
            }

            tbContent.Value = dt.Rows[0]["Content"].ToString();

            Regex regex = new Regex(@"^http://([\w-]+\.)+[\w-]+(/[\w-./?%&=]*)?$", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            Match m = regex.Match(dt.Rows[0]["Content"].ToString());

            if (m.Success)
            {
                trUrl.Visible = true;
                trContent.Visible = false;
                rblType.SelectedIndex = 0;
                tbUrl.Text = dt.Rows[0]["Content"].ToString();
            }
            else
            {
                trContent.Visible = true;
                trUrl.Visible = false;
                rblType.SelectedIndex = 1;
                tbContent.Value = dt.Rows[0]["Content"].ToString();
            }

            Shove.ControlExt.SetDownListBoxText(ddlImage, dt.Rows[0]["ImageUrl"].ToString());
            tbOldImage.Text = dt.Rows[0]["ImageUrl"].ToString().Trim();

            if (dt.Rows[0]["ImageUrl"].ToString().Trim() == "")
            {
                cbNoEditImage.Checked = false;
                cbNoEditImage.Visible = false;
            }

            hidTypeID.Value = dt.Rows[0]["TypeID"].ToString();
        }

    }

    protected void btnSave_Click(object sender, System.EventArgs e)
    {
        DateTime dt = System.DateTime.Now;

        try
        {
            dt = System.DateTime.Parse(tbDateTime.Text);
        }
        catch
        {
            Shove._Web.JavaScript.Alert(this.Page, "时间格式错误，请输入如“" + dt.ToString() + "”的时间格式。");

            return;
        }

        long ReadCount = Shove._Convert.StrToLong(tbReadCount.Text, -1);

        if (ReadCount < 0)
        {
            Shove._Web.JavaScript.Alert(this.Page, "请输入正确的已阅读次数。");

            return;
        }

        string Title = tbTitle.Text.Trim();

        if (Title == "")
        {
            Shove._Web.JavaScript.Alert(this.Page, "请输入标题。");

            return;
        }

        string Image = "";

        if (cbNoEditImage.Checked)
        {
            Image = tbOldImage.Text;
        }
        else
        {
            if (tbImage.Value.Trim() != "")
            {
                string UploadPath = this.Server.MapPath("../../Private/" + _Site.ID.ToString() + "/NewsImages");

                if (!System.IO.Directory.Exists(UploadPath))
                {
                    System.IO.Directory.CreateDirectory(UploadPath);
                }

                if (Shove._IO.File.UploadFile(this.Page, tbImage, "../../Private/" + _Site.ID.ToString() + "/NewsImages/", ref Image, true, "image") != 0)
                {
                    Shove._Web.JavaScript.Alert(this.Page, "图片文件上传错误！");

                    return;
                }
            }
            else
            {
                if (ddlImage.SelectedIndex > 0)
                {
                    Image = ddlImage.SelectedItem.Text;
                }
            }
        }

        int ReturnValue = -1;
        string ReturnDescription = "";
        string UC = "";

        if (rblType.SelectedValue == "1")
        {
            UC = tbUrl.Text.Trim();

            Regex regex = new Regex(@"^http://([\w-]+\.)+[\w-]+(/[\w-./?%&=]*)?$", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            Match m = regex.Match(UC);

            if (!m.Success)
            {
                Shove._Web.JavaScript.Alert(this, "地址格式错误，请仔细检查。");

                return;
            }
        }
        else
        {
            UC = tbContent.Value.Trim();
        }

        if (string.IsNullOrEmpty(UC))
        {
            Shove._Web.JavaScript.Alert(this.Page, "请输入内容！");

            return;
        }

        if (ddlTitleColor.SelectedValue == "red")
        {
            Title = "<font class='red12'>" + Title + "</font>";
        }
        else if (ddlTitleColor.SelectedValue == "black")
        {
            Title = "<font class='black12'>" + Title + "</font>";
        }

        if (Shove._String.GetLength(Title) > 100)
        {
            Shove._Web.JavaScript.Alert(this.Page, "标题长度超过限制！");

            return;
        }

        int Results = -1;
        Results = DAL.Procedures.P_NewsEdit(_Site.ID, long.Parse(tbID.Text), int.Parse(hidTypeID.Value), dt, Title, UC, Image, cbisShow.Checked, (Image != ""), cbisCanComments.Checked, cbisCommend.Checked, cbisHot.Checked, ReadCount, ref ReturnValue, ref ReturnDescription);
        if (Results < 0)
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", this.GetType().BaseType.FullName);

            return;
        }

        if (ReturnValue < 0)
        {
            Shove._Web.JavaScript.Alert(this.Page, ReturnDescription);

            return;
        }

        Shove._Web.Cache.ClearCache("CPS_Default_BindNews");
        Shove._Web.Cache.ClearCache("CPS_News_BindNews");

        this.Response.Redirect("News.aspx?TypeID=" + hidTypeID.Value, true);
    }

    protected void rblType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rblType.SelectedValue == "1")
        {
            trUrl.Visible = true;
            trContent.Visible = false;
        }
        else
        {
            trUrl.Visible = false;
            trContent.Visible = true;
        }
    }
}
