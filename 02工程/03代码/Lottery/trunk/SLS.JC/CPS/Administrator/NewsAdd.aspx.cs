using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;

public partial class CPS_Administrator_NewsAdd : AdminPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            trContent.Visible = false;
            BindData();
        }
    }

    #region Web 窗体设计器生成的代码

    protected override void OnLoad(EventArgs e)
    {
        RequestLoginPage = Shove._Web.Utility.GetUrl() + "/Cps/Administrator/Default.aspx";

        RequestCompetences = Competences.BuildCompetencesList(Competences.CpsManage);

        base.OnLoad(e);
    }

    #endregion

    private void BindData()
    {
        tbDateTime.Text = System.DateTime.Now.ToString();

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
    }

    protected void btnAdd_Click(object sender, System.EventArgs e)
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

        long NewsID = -1;
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
        Results = DAL.Procedures.P_NewsAdd(_Site.ID, int.Parse(Shove._Web.Utility.GetRequest("TypeID")), dt, Title, UC, Image, cbisShow.Checked, (Image != ""), cbisCanComments.Checked, cbisCommend.Checked, cbisHot.Checked, ReadCount, ref NewsID, ref ReturnDescription);

        if (Results < 0)
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", this.GetType().BaseType.FullName);

            return;
        }

        if (NewsID < 0)
        {
            Shove._Web.JavaScript.Alert(this.Page, ReturnDescription);

            return;
        }

        Shove._Web.Cache.ClearCache("CPS_Default_BindNews");
        Shove._Web.Cache.ClearCache("CPS_News_BindNews");

        this.Response.Redirect("News.aspx?TypeID=" + Shove._Web.Utility.GetRequest("TypeID"), true);
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
