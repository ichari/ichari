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

public partial class Home_Room_ExpertsReg : RoomPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (_Site.SiteOptions["Opt_Experts_Status_ON"].ToBoolean(false))
        {
            if (!this.IsPostBack)
            {
                BindData(cblLotteryListZC,"and [TypeID]=2");
                BindData(cblLotteryListFC," and ID in (5,58,59,6,13)");
            }
        }
    }

    #region Web 窗体设计器生成的代码

    override protected void OnInit(EventArgs e)
    {
        RequestLoginPage = this.Request.Url.AbsoluteUri;

        base.OnInit(e);
    }

    #endregion

    private void BindData(CheckBoxList list,string temp)
    {
        DataTable dt = new DAL.Tables.T_Lotteries().Open("[ID], [Name]", "[ID] in (" + (_Site.UseLotteryList == "" ? "-1" : _Site.UseLotteryList) + ") "+temp, "[Order]");

        if (dt == null)
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", this.GetType().BaseType.FullName);

            return;
        }

        list.Items.Clear();

        foreach (DataRow dr in dt.Rows)
        {
            list.Items.Add(new ListItem(dr["Name"].ToString(), dr["ID"].ToString()));
        }
    }

    protected void btnOK_Click(object sender, EventArgs e)
    {
        bool ListChecked = true;
        if (tbDescription.Text.Trim() == "")
        {
            Shove._Web.JavaScript.Alert(this.Page, "请输入申请描述！");

            return;
        }

        long NewExpertsTryID = 0;
        string ReturnDescription = "";

        string ReturnInfo = "";
        #region 获取选择的彩种
        foreach (ListItem li in cblLotteryListZC.Items)
        {

            if (li.Selected)
            {
                ListChecked = false;
                int Result = DAL.Procedures.P_ExpertsTry(_Site.ID, _User.ID, int.Parse(li.Value),Shove._Web.Utility.FilteSqlInfusion(tbDescription.Text.Trim()), 0.0, 0.0, ref NewExpertsTryID, ref ReturnDescription);

                if (NewExpertsTryID < 0)
                {
                    ReturnInfo += li.Text + " " + ReturnDescription + "\\n";

                    continue;
                }

                if (NewExpertsTryID > 0)
                {
                    ReturnInfo += li.Text + "申请信息提交成功\\n";
                }
            }
        }
        foreach (ListItem li in cblLotteryListFC.Items)
        {

            if (li.Selected)
            {
                ListChecked = false;
                int Result = DAL.Procedures.P_ExpertsTry(_Site.ID, _User.ID, int.Parse(li.Value), Shove._Web.Utility.FilteSqlInfusion(tbDescription.Text.Trim()), 0.0, 0.0, ref NewExpertsTryID, ref ReturnDescription);

                if (NewExpertsTryID < 0)
                {
                    ReturnInfo += li.Text + " " + ReturnDescription + "\\n";

                    continue;
                }

                if (NewExpertsTryID > 0)
                {
                    ReturnInfo += li.Text + "申请信息提交成功\\n";
                }
            }
        }

        #endregion

        if (ReturnInfo != "")
        {
            Shove._Web.JavaScript.Alert(this.Page, ReturnInfo);

            return;
        }
        if (ListChecked)
        {
            Shove._Web.JavaScript.Alert(this.Page, "您没有选择任何彩种");

            return;
        }
    }

}