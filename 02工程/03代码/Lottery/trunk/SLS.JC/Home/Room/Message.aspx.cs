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

using Shove.Database;
using System.Text;

public partial class Home_Room_Message : RoomPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            BindDataForSystemMessage();

        }
    }

    #region Web 窗体设计器生成的代码

    override protected void OnInit(EventArgs e)
    {
        RequestLoginPage = this.Request.Url.AbsoluteUri;

        base.OnInit(e);
    }

    #endregion

    private void BindDataForSystemMessage()
    {
        DataTable dt = Shove._Web.Cache.GetCacheAsDataTable("MemberMessage_System" + _User.ID.ToString());

        if (dt == null)
        {

            string sql = "select '系统消息' as SourceUserName,ID,DateTime,Content,isRead from T_StationSMS where isShow = 1 and Type = 2 and AimID = " + _User.ID.ToString() + " order by isRead,  DateTime asc";
            dt = Shove.Database.MSSQL.Select(sql);

            if (dt == null)
            {
                PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", "Room_Message");

                return;
            }

            Shove._Web.Cache.SetCache("MemberMessage_System" + _User.ID.ToString(), dt);
        }

        PF.DataGridBindData(g1, dt, gPager1);
    }

    protected void gPager1_PageWillChange(object Sender, Shove.Web.UI.PageChangeEventArgs e)
    {
        BindDataForSystemMessage();
    }

    protected void g1_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        if (e.CommandName == "Del")
        {
            long ID = Shove._Convert.StrToLong(e.Item.Cells[5].Text, 0);

            if (MSSQL.ExecuteNonQuery("update T_StationSMS set isShow = 0 where [ID] = " + ID.ToString()) < 0)
            {
                PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", "Room_Message");

                return;
            }

            Shove._Web.Cache.ClearCache("MemberMessage_System" + _User.ID.ToString());

            BindDataForSystemMessage();

            return;
        }
        else if (e.CommandName == "View")
        {
            long ID = Shove._Convert.StrToLong(e.Item.Cells[5].Text, 0);
            Response.Redirect("ViewMessage.aspx?ID=" + ID);
        }
    }

    protected void g1_ItemDataBound(object sender, DataGridItemEventArgs e)
    {
        CheckBox cbisRead = (CheckBox)e.Item.Cells[4].FindControl("cbisRead");
        if (cbisRead != null)
        {
            cbisRead.Checked = Shove._Convert.StrToBool(e.Item.Cells[6].Text, false);
            if (!cbisRead.Checked)
            {
                cbisRead.Enabled = true;
            }
        }

        e.Item.Cells[1].Text = Shove._String.Cut(e.Item.Cells[1].Text, 20);
    }

    private void UpdateStationSMSStatus(int id)
    {
        try
        {
            string sql = "update T_StationSMS set isRead = 1 where ID = @ID";
            MSSQL.ExecuteNonQuery(sql, new MSSQL.Parameter("ID", SqlDbType.BigInt, 0, ParameterDirection.Input, id));
            
            Shove._Web.Cache.ClearCache("MemberMessage_System" + _User.ID.ToString());
            BindDataForSystemMessage();
           
        }
        catch (Exception e)
        {
            new Log("System").Write("短消息页面Message.aspx，修改消息状态出错" + e.Message);
        }
    }

    protected void cbisRead_CheckedChanged(object sender, EventArgs e)
    {
        string str = ((CheckBox)sender).ClientID.Substring(7);
        int rowIndex = Shove._Convert.StrToInt(str.Substring(0, str.IndexOf("_")), -1) - 3;
        if (rowIndex > gPager1.PageSize)
        {
            rowIndex = rowIndex % gPager1.PageSize;
        }
        UpdateStationSMSStatus(Shove._Convert.StrToInt(g1.Items[rowIndex].Cells[5].Text, -1));
    }
}
