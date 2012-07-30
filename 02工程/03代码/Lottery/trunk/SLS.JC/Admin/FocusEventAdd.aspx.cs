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
using System.Text.RegularExpressions;

public partial class Admin_FocusEventAdd : AdminPageBase
{
    // 是否加载新模版
    private bool isLoadTemplate = true;   

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            BindDataForYearMonth();
            BindData();
            BindTemplate();
        }
    }

    #region Web 窗体设计器生成的代码

    override protected void OnInit(EventArgs e)
    {
        RequestLoginPage = this.Request.Url.AbsoluteUri;

        RequestCompetences = Competences.BuildCompetencesList(Competences.FillContent);

        base.OnInit(e);
    }

    #endregion

    private void BindDataForYearMonth()
    {
        ddlYear.Items.Clear();

        DateTime dt = DateTime.Now;
        int Year = dt.Year;
        int Month = dt.Month;

        if (Year < PF.SystemStartYear)
        {
            return;
        }

        for (int i = PF.SystemStartYear; i <= Year; i++)
        {
            ddlYear.Items.Add(new ListItem(i.ToString() + "年", i.ToString()));
        }

        ddlYear.SelectedIndex = ddlYear.Items.Count - 1;

        ddlMonth.SelectedIndex = Month - 1;
    }

    private void BindData()
    {
        hID.Value = Shove._Web.Utility.GetRequest("ID");

        if (hID.Value != "")
        {
            DataTable dt = new DAL.Tables.T_FocusEvent().Open("", "ID=" + hID.Value, "");

            if (dt == null || dt.Rows.Count == 0)
            {
                PF.GoError(ErrorNumber.NoData, "数据不存在或已被删除！", this.GetType().BaseType.FullName);

                return;
            }

            tbTitle.Text = dt.Rows[0]["Title"].ToString();
            tbContent.Value = dt.Rows[0]["Content"].ToString();
            cbIsMaster.Checked = Shove._Convert.StrToBool(dt.Rows[0]["IsShow"].ToString(), false);
            ddlYear.SelectedIndex = ddlYear.Items.Count - 1 - (DateTime.Now.Year - Shove._Convert.StrToInt(dt.Rows[0]["Year"].ToString(), 0));
            ddlMonth.SelectedIndex = Shove._Convert.StrToInt(dt.Rows[0]["Month"].ToString(), 0) - 1;
            
            isLoadTemplate = false;
        }

        ddlImage.Items.Clear();
        ddlImage.Items.Add("--选择图片--");

        string UploadPath = this.Server.MapPath("../Private/" + _Site.ID.ToString() + "/NewsImages");

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
    }

    private void BindTemplate()
    {
        if (isLoadTemplate)
        {
            // 找到 模版静态页面
            string templatePath = this.Server.MapPath("/Html/FocustTemplate/Foucs.html");
            string htmlContent = Shove._IO.File.ReadFile(templatePath);
            tbContent.Value = htmlContent;
        }
    }

    protected void btnAdd_Click(object sender, System.EventArgs e)
    {
        string Title = tbTitle.Text.Trim();

        if (string.IsNullOrEmpty(Title))
        {
            Shove._Web.JavaScript.Alert(this.Page, "请输入标题。");

            return;
        }

        string Content = tbContent.Value.Trim();

        if (string.IsNullOrEmpty(Content))
        {
            Shove._Web.JavaScript.Alert(this.Page, "请输入内容。");

            return;
        }

        string Image = "";

        if (tbImage.Value.Trim() != "")
        {
            string UploadPath = this.Server.MapPath("../Private/" + _Site.ID.ToString() + "/NewsImages");

            if (!System.IO.Directory.Exists(UploadPath))
            {
                System.IO.Directory.CreateDirectory(UploadPath);
            }

            if (Shove._IO.File.UploadFile(this.Page, tbImage, "../Private/" + _Site.ID.ToString() + "/NewsImages/", ref Image, true, "image") != 0)
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

        DAL.Tables.T_FocusEvent f = new DAL.Tables.T_FocusEvent();

        f.Title.Value = Title;
        f.Content.Value = Content;
        f.IsShow.Value = cbIsMaster.Checked;
        f.ImageUrl.Value = Image;
        f.Year.Value = ddlYear.SelectedValue;
        f.Month.Value = ddlMonth.SelectedValue;

        if (hID.Value == "")
        {
            if (string.IsNullOrEmpty(Image))
            {
                Shove._Web.JavaScript.Alert(this.Page, "请选择图片！");

                return;
            }

            f.Insert();
        }
        else
        {
            f.Update("ID=" + hID.Value);
        }

        Shove._Web.Cache.ClearCache("Admin_FocusEventAdd");
        // 清除前台缓存
        Shove._Web.Cache.ClearCache("Focus_Default_Year_" + ddlYear.SelectedValue.Trim() + "_Month_" + ddlMonth.SelectedValue.Trim());
        this.Response.Redirect("FocusEvent.aspx", true);
    }

}
