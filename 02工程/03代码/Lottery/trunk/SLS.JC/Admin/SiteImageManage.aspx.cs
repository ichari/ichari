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

public partial class Admin_SiteImageManage : AdminPageBase
{

    protected string ClientFileName = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        // 清除服务端客户端缓存
        //Response.Buffer = true;
        //Response.ExpiresAbsolute = System.DateTime.Now.AddSeconds(-1);
        //Response.Expires = 0;
        //Response.Cache.SetCacheability(HttpCacheability.NoCache);
        //Response.CacheControl = "no-cache"; 

        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.Buffer = false;
        Response.CacheControl = "no-cache";
        Response.AddHeader("Pragma", "no-cache");
        Response.Expires = -1;

        if (!this.IsPostBack)
        {
            BindData();
            sfp1.Visible = false;
            img1.Visible = false;
        }
    }

    #region Web 窗体设计器生成的代码

    protected override void OnLoad(EventArgs e)
    {
        RequestLoginPage = this.Request.Url.AbsoluteUri;

        RequestCompetences = Competences.BuildCompetencesList(Competences.EditNews);

        base.OnLoad(e);
    }

    #endregion

    private void BindData()
    {
        // 数据库中的表： ID, PageName, ResourceName, ResourceUrl

        DataTable dt = new DAL.Tables.T_PageResources().Open("distinct PageName", "", "");

        if (dt == null)
        {
            return;
        }

        if (dt.Rows.Count < 1)
        {
            return;
        }


        ddlPageName.Items.Clear();

        ddlPageName.Items.Add(new ListItem("请选择页面", "-1"));

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            ddlPageName.Items.Add(new ListItem(dt.Rows[i]["PageName"].ToString(), (i + 1).ToString()));
        }
    }
    
    protected void ddlPageName_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlResourceName.Items.Clear();
        sfp1.Visible = false;
        img1.Visible = false;
        btnUpdate.Enabled = false;
        this.lblDescriptioin.Text = "";
        this.lblDescriptioin.Visible = false;

        if (ddlPageName.SelectedValue == "-1")
        {
            return;
        }

        // 查找数据库，显示相应文件图片页面路径
        object o = Shove.Database.MSSQL.ExecuteScalar("select top 1 SitePath from T_PageResources where PageName = '" + ddlPageName.SelectedItem.Text + "'");

        if (o != null)
        {
            lblDescriptioin.Text = "<br/>页面站点相关路径： " + o.ToString();
            lblDescriptioin.Visible = true;
        }
        else
        {
            lblDescriptioin.Text = "";
            lblDescriptioin.Visible = false;
        }
        
        DataTable dt = new DAL.Tables.T_PageResources().Open("ResourceName, ResourceUrl", "PageName = '" + ddlPageName.SelectedItem.Text + "'", "ResourceName");

        ddlResourceName.Items.Add(new ListItem("请选择一个资源", ""));

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            ddlResourceName.Items.Add(new ListItem(dt.Rows[i]["ResourceName"].ToString(), dt.Rows[i]["ResourceUrl"].ToString()));
        }        
    }

    protected void ddlResourceName_SelectedIndexChanged(object sender, EventArgs e)
    {
        sfp1.Visible = false;
        img1.Visible = false;
        btnUpdate.Enabled = false;

        if (ddlResourceName.SelectedValue == "")
        {
            return;
        }

        if (ddlResourceName.SelectedValue.EndsWith(".swf", StringComparison.OrdinalIgnoreCase))
        {
            sfp1.Visible = true;
            sfp1.Src = ddlResourceName.SelectedValue;
        }
        else
        {
            img1.Visible = true;
            img1.ImageUrl = ddlResourceName.SelectedValue;
        }

        btnUpdate.Enabled = true;

        // 截取
        string resourceName = ddlResourceName.SelectedValue;

        string imageName = resourceName.Substring(resourceName.LastIndexOf("/") + 1);

        ClientFileName = imageName;
    }

    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        if (uploadFile.Value == "")
        {
            Shove._Web.JavaScript.Alert(this.Page, "请选择一个文件后再确定上传。");

            return;
        }

        if (ddlResourceName.SelectedValue.EndsWith(".swf", StringComparison.OrdinalIgnoreCase))
        {
            sfp1.Visible = true;
            sfp1.Src = ddlResourceName.SelectedValue;
        }
        else
        {
            img1.Visible = true;
            img1.ImageUrl = ddlResourceName.SelectedValue;
        }

        // 效验上传路径
        string UploadPath = this.Server.MapPath(ddlResourceName.SelectedValue);
        
        if (!System.IO.File.Exists(UploadPath))
        {// 文件不存在
            Shove._Web.JavaScript.Alert(this.Page, "站点图片路径不存在，请检查");
            return;
        }
        // 截取
        string resourceName = ddlResourceName.SelectedValue;

        string imagePath = resourceName.Substring(0, resourceName.LastIndexOf("/") + 1);

        string imageName = resourceName.Substring(resourceName.LastIndexOf("/") + 1);


        // 上传
        if (uploadFile.Value.EndsWith(".swf", StringComparison.OrdinalIgnoreCase))
        {
            if (Shove._IO.File.UploadFile(this.Page, uploadFile, imagePath, imageName, true, "swf") != 0)
            { 
                Shove._Web.JavaScript.Alert(this.Page, "Flash文件上传错误！");

                return;
            }
            sfp1.Src = ddlResourceName.SelectedValue;
        }
        else
        {
            if (Shove._IO.File.UploadFile(this.Page, uploadFile, imagePath, imageName, true, "image") != 0)
            {
                Shove._Web.JavaScript.Alert(this.Page, "图片文件上传错误！");

                return;
            }
            img1.ImageUrl = ddlResourceName.SelectedValue;            
        }

        Shove._Web.JavaScript.Alert(this.Page, "图片上传成功");
    }
}
