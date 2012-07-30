using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Shove.Database;

public partial class Home_Room_ViewMessage : RoomPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            bindData();
        }
    }

    #region Web 窗体设计器生成的代码

    override protected void OnInit(EventArgs e)
    {
        RequestLoginPage = this.Request.Url.AbsoluteUri;

        base.OnInit(e);
    }

    #endregion

    private void bindData()
    {
        long id = Shove._Convert.StrToLong(Shove._Web.Utility.GetRequest("ID"), -1);

        DataTable dt = Shove.Database.MSSQL.Select("select Content from T_StationSMS where isShow = 1 and Type = 2 and AimID = " + _User.ID.ToString() + " and ID = " + id.ToString());

        if (dt == null || dt.Rows.Count == 0)
        {
            PF.GoError(ErrorNumber.NoData,"你要查看的站内信不存在或数据已被删除！",this.GetType().BaseType.FullName);

            return;
        }

        lbAim.Text = "系统消息";
        lbContent.Text = dt.Rows[0]["Content"].ToString();

        try
        {
            string sql = "update T_StationSMS set isRead = 1 where ID = @ID";
            MSSQL.ExecuteNonQuery(sql, new MSSQL.Parameter("ID", SqlDbType.BigInt, 0, ParameterDirection.Input, id));
        
            Shove._Web.Cache.ClearCache("MemberMessage_System" + _User.ID.ToString());
        }
        catch (Exception e)
        {
            new Log("System").Write("查看短消息页面ViewMessage.aspx，修改消息状态出错" + e.Message);
        }
    }
}
