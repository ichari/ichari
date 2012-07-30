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

public partial class Admin_NewsAdd : AdminPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            BindDataForNewsTypes();

            BindData();
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

    private void BindDataForNewsTypes()
    {
        DataTable dt = new DAL.Tables.T_NewsTypes().Open("", "SiteID = " + _Site.ID.ToString(), "[ID]");

        if (dt == null)
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", this.GetType().BaseType.FullName);

            return;
        }

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

        long NewsID = -1;
        string ReturnDescription = "";
        string UC = tbContent.Value.Trim();

        //Regex regex = new Regex(@"([\w-]+\.)+[\w-]+.([^a-z])(/[\w- ./?%&=]*)?|[a-zA-Z0-9\-\.][\w-]+.([^a-z])(/[\w- ./?%&=]*)?", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        //Match m = regex.Match(UC);

        //if (!m.Success)
        //{
        //    Shove._Web.JavaScript.Alert(this, "地址格式错误，请仔细检查。");

        //    return;
        //}
        if (rblType.SelectedValue == "1")
        {
            UC = tbUrl.Text.Trim();

            Regex regex = new Regex(@"([\w-]+\.)+[\w-]+.([^a-z])(/[\w- ./?%&=]*)?|[a-zA-Z0-9\-\.][\w-]+.([^a-z])(/[\w- ./?%&=]*)?", RegexOptions.IgnoreCase | RegexOptions.Compiled);
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
        Results = DAL.Procedures.P_NewsAdd(_Site.ID, int.Parse(ddlTypes.SelectedValue), dt, Title, UC, Image, cbisShow.Checked, (Image != ""), cbisCanComments.Checked, cbisCommend.Checked, cbisHot.Checked, ReadCount, ref NewsID, ref ReturnDescription);

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

        string type = ddlTypes.SelectedItem.Text.Trim();

        if (type == "福彩资讯" || type == "体彩资讯" || type == "足彩资讯")
        {
            Shove._Web.Cache.ClearCache("Default_GetNews");
        }

        if (type.Contains("3D"))
        {
            Shove._Web.Cache.ClearCache("Home_Room_Buy_BindNewsForLotterys6");
            Shove._Web.Cache.ClearCache("Home_Room_Buy_BindNewsForLottery6");
        }

        if (type.Contains("双色球"))
        {
            Shove._Web.Cache.ClearCache("Home_Room_Buy_BindNewsForLotterys5");
            Shove._Web.Cache.ClearCache("Home_Room_Buy_BindNewsForLottery5");
        }

        if (type.Contains("玩法攻略"))
        {
            Shove._Web.Cache.ClearCache("Default_BindWFGL");
        }

        string cacheKey = "";
        string TypeName = ddlTypes.SelectedItem.Text;
        if (TypeName.Contains("热门人物追踪"))
        {
            cacheKey = "Home_Room_JoinAllBuy_BindNews";
        }

        else if (TypeName.Contains("时时乐资讯"))
        {
            cacheKey = DataCache.LotteryNews + "29";
        }
        else if (TypeName.Contains("十一运夺金资讯"))
        {
            cacheKey = DataCache.LotteryNews + "62";
        }
        else if (TypeName.Contains("双色球资讯"))
        {
            cacheKey = DataCache.LotteryNews + "5";
        }
        else if (TypeName.Contains("3D资讯"))
        {
            cacheKey = DataCache.LotteryNews + "6";
        }
        else if (TypeName.Contains("超级大乐透资讯"))
        {
            cacheKey = DataCache.LotteryNews + "39";
        }
        else if (TypeName.Contains("排列3/5资讯"))
        {
            cacheKey = DataCache.LotteryNews + "63";
            Shove._Web.Cache.ClearCache(DataCache.LotteryNews + "64");
        }
        else if (TypeName.Contains("足彩资讯"))
        {
            cacheKey = DataCache.LotteryNews + "1";
            Shove._Web.Cache.ClearCache(DataCache.LotteryNews + "2");
            Shove._Web.Cache.ClearCache(DataCache.LotteryNews + "15");
        }
        // 附加清除
        if (type.Contains("擂台新闻"))
        {
            Shove._Web.Cache.ClearCache("DataCache_Challenge_72_News");
        }

        if (type.Contains("名人堂新闻"))
        {
            Shove._Web.Cache.ClearCache("DataCache_CelebrityHall_News");
        }

        if (type.Contains("竞技彩资讯") || type.Contains("数字彩资讯"))
        {
            Shove._Web.Cache.ClearCache("Default_BindSportsNews");
        }

        if (type.Contains("竞彩足球"))
        {
            Shove._Web.Cache.ClearCache("ForeCast_BindNewsForLottery竞彩足球");
        }
        if (type.Contains("竞彩篮球"))
        {
            Shove._Web.Cache.ClearCache("ForeCast_BindNewsForLottery竞彩篮球");
        }
        if (type.Contains("超级大乐透"))
        {
            Shove._Web.Cache.ClearCache("ForeCast_BindNewsForLottery超级大乐透");
        }
        if (type.Contains("排列3/5"))
        {
            Shove._Web.Cache.ClearCache("ForeCast_BindNewsForLottery排列3/5");
        }
        if (type.Contains("七星彩"))
        {
            Shove._Web.Cache.ClearCache("ForeCast_BindNewsForLottery七星彩");
        }
        if (type.Contains("22选5"))
        {
            Shove._Web.Cache.ClearCache("ForeCast_BindNewsForLottery22选5");
        }
        if (type.Contains("竞彩足球"))
        {
            Shove._Web.Cache.ClearCache("ForeCast_BindNewsForLottery竞彩足球");
        }

        if (type.Contains("足彩资讯"))
        {
            Shove._Web.Cache.ClearCache("ForeCast_BindNewsForLottery足彩资讯");
        }

        if (type.Contains("欧冠资讯"))
        {
            Shove._Web.Cache.ClearCache("ForeCast_BindNewsForLottery欧冠资讯");
        }
        if (type.Contains("英超资讯"))
        {
            Shove._Web.Cache.ClearCache("ForeCast_BindNewsForLottery英超资讯");
        }
        if (type.Contains("西甲资讯"))
        {
            Shove._Web.Cache.ClearCache("ForeCast_BindNewsForLottery西甲资讯");
        }
        if (type.Contains("意甲资讯"))
        {
            Shove._Web.Cache.ClearCache("ForeCast_BindNewsForLottery意甲资讯");
        }
        if (type.Contains("德甲资讯"))
        {
            Shove._Web.Cache.ClearCache("ForeCast_BindNewsForLottery德甲资讯");
        }

        if (cacheKey != "")
        {
            Shove._Web.Cache.ClearCache(cacheKey);
        }

        this.Response.Redirect("News.aspx?TypeID=" + ddlTypes.SelectedValue, true);
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
