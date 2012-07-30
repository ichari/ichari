using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

public partial class Home_Room_QuestionList : RoomPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {

        //BindData();

        if (!this.IsPostBack)
        {
            labUserName.Text = _User.Name;

            long QuestionID = Shove._Convert.StrToLong(Shove._Web.Utility.GetRequest("id"), 0);

            if (QuestionID > 0)
            {
                DataTable dt = new DAL.Views.V_Questions().Open("", "id = " + QuestionID.ToString(), "");

                if (dt == null || dt.Rows.Count == 0)
                {
                    PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", "Room_QuestionList");

                    return;
                }

                if (dt.Rows.Count > 0)
                {
                	labContent.Text = dt.Rows[0]["Content"].ToString();
                    labAnswer.Text = dt.Rows[0]["Answer"].ToString();

                    short AnswerStatus = Shove._Convert.StrToShort(dt.Rows[0]["AnswerStatus"].ToString(), 0);
                    if (AnswerStatus == 0)
                    {
                        labAnswerDateTime.Text = "未答复";
                    }
                    else if (AnswerStatus == 1)
                    {
                        labAnswerDateTime.Text = "处理中";
                    }
                    else
                    {
                        labAnswerDateTime.Text = "(答复时间：" + dt.Rows[0]["AnswerDateTime"].ToString() + ")";
                    }
                }
            }

            btnType_1_Click(btnType_1, e);
        }
    }

    #region Web 窗体设计器生成的代码

    override protected void OnInit(EventArgs e)
    {
        RequestLoginPage = this.Request.Url.AbsoluteUri;

        base.OnInit(e);
    }

    #endregion

    private void BindData()
    {
        string sType = "1";

        try
        {
            sType = ViewState["MemberQuestionList_Type"].ToString();
        }
        catch { }

        int Type = Shove._Convert.StrToInt(sType, 1);

        if ((Type < 1) || (Type > 3))
        {
            Type = 1;
        }

        BindData(Type);
    }

    private void BindData(int Type)
    {
        DataTable dt = Shove._Web.Cache.GetCacheAsDataTable("MemberQuestionList_" + Type.ToString() + "_" + _User.ID.ToString());

        if (dt == null)
        {
            switch (Type)
            {
                case 1:
                    dt = new DAL.Views.V_Questions().Open("", "SiteID = " + _Site.ID.ToString() + " and UserID = " + _User.ID.ToString() + " and UseType = 1", "[DateTime] desc");
                    break;
                case 2:
                    dt = new DAL.Views.V_Questions().Open("", "SiteID = " + _Site.ID.ToString() + " and UserID = " + _User.ID.ToString() + " and UseType = 1 and AnswerStatus = 0", "[DateTime] desc");
                    break;
                case 3:
                    dt = new DAL.Views.V_Questions().Open("", "SiteID = " + _Site.ID.ToString() + " and UserID = " + _User.ID.ToString() + " and UseType = 1 and AnswerStatus = 1", "[DateTime] desc");
                    break;
            }

            if (dt == null)
            {
                PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", this.GetType().FullName);

                return;
            }

            Shove._Web.Cache.SetCache("MemberQuestionList_" + Type.ToString() + "_" + _User.ID.ToString(), dt);
        }

        PF.DataGridBindData(g, dt, gPager);
    }

    protected void btnType_1_Click(object sender, System.EventArgs e)
    {
        int Type = Shove._Convert.StrToInt(((LinkButton)sender).ID.Substring(8, 1), 1);

        ViewState["MemberQuestionList_Type"] = Type;

        BindData(Type);
    }

    protected void g_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.EditItem)
        {
            e.Item.Cells[2].Text = "<a href='QuestionList.aspx?id=" + e.Item.Cells[4].Text + "'><font color=\"#330099\">" + e.Item.Cells[2].Text + "</Font></a>";

            short AnswerStatus = Shove._Convert.StrToShort(e.Item.Cells[5].Text, 0);
            if (AnswerStatus == 0)
            {
                e.Item.Cells[3].Text = "未答复";
            }
            else if (AnswerStatus == 1)
            {
                e.Item.Cells[3].Text = "处理中";
            }
            else
            {
                e.Item.Cells[3].Text = "<font color='red'>已答复</font>";
            }
        }
    }

    protected void gPager_PageWillChange(object Sender, Shove.Web.UI.PageChangeEventArgs e)
    {
        BindData();
    }
}
