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
using Discuz.Web.Admin;

public partial class admin_global_global_News_Add : AdminPage
{
    string NPIsusID = Shove._Web.Utility.GetRequest("id");

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            BindDataForNewsTypes();

            BindData();

            trContent.Visible = false;
        }
    }

    private void BindDataForNewsTypes()
    {

        DataTable dt = new DAL.Tables.T_NewsTypes().Open("", "SiteID = 1", "[ID]");

        Shove.ControlExt.FillDropDownList(ddlTypes, dt, "Name", "ID");

        string TypeID = Shove._Convert.StrToLong(Shove._Web.Utility.GetRequest("TypeID"), -1).ToString();

        if (TypeID != "-1")
        {
            Shove.ControlExt.SetDownListBoxTextFromValue(ddlTypes, TypeID);
        }
    }

    private void BindData()
    {
        tbDateTime.Text = System.DateTime.Now.ToString();

        ddlImage.Items.Clear();
        ddlImage.Items.Add("--选择图片--");

        string UploadPath = this.Server.MapPath("../../Cache/thumbnail/");

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
            string UploadPath = this.Server.MapPath("../../Cache/thumbnail/");

            if (!System.IO.Directory.Exists(UploadPath))
            {
                System.IO.Directory.CreateDirectory(UploadPath);
            }

            if (Shove._IO.File.UploadFile(this.Page, tbImage, "../../Cache/thumbnail/", ref Image, true, "image") != 0)
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
            UC = tbContent.Text.Trim();
        }
        if (Title.IndexOf("</font>") > -1)
        {
            Title = (Title.Substring(Title.IndexOf(">") + 1, Title.IndexOf("</font>") - Title.IndexOf(">") - 1));
        }

        if (ddlTitleColor.SelectedValue == "red")
        {
            Title = "<font class='red12'>" + Title + "</font>";
        }
        else if (ddlTitleColor.SelectedValue == "black")
        {
            Title = "<font class='black12'>" + Title + "</font>";
        }
        else
        {
            if (Title.IndexOf("</font>") > -1)
            {
                Title = (Title.Substring(Title.IndexOf(">") + 1, Title.IndexOf("</font>") - Title.IndexOf(">") - 1));
            }
        }

        int Results = -1;
        Results = DAL.Procedures.P_NewsAdd(1, int.Parse(ddlTypes.SelectedValue), dt, Title, UC, Image, cbisShow.Checked, (Image != ""), cbisCanComments.Checked, cbisCommend.Checked, cbisHot.Checked, ReadCount, Shove._Convert.StrToInt(NPIsusID, 0), ref NewsID, ref ReturnDescription);

        //if (Results < 0)
        //{
        //    PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", this.GetType().BaseType.FullName);

        //    return;
        //}

        if (NewsID < 0)
        {
            Shove._Web.JavaScript.Alert(this.Page, ReturnDescription);

            return;
        }

        string cacheKey = "";
        switch (ddlTypes.SelectedItem.Text)
        {
            case "中奖故事":
                cacheKey = "dt_Club_News";
                break;
        }

        if (cacheKey != "")
        {
            Shove._Web.Cache.ClearCache(cacheKey);
        }
        Response.Redirect("global_News.aspx");
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
