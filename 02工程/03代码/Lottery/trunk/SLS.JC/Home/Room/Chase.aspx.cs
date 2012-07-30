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

public partial class Home_Room_Chase : RoomPageBase
{
    public string LotteryID;

    public string LotteryName, PlayTypeName;
    public string IframeUrl;


    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            LotteryID = Shove._Web.Utility.GetRequest("LotteryID");

            BindPlay();
        }
    }

    #region Web 窗体设计器生成的代码

    override protected void OnInit(EventArgs e)
    {
        RequestLoginPage = this.Request.Url.AbsoluteUri;

        base.OnInit(e);
    }

    #endregion

    private void BindPlay()
    {
        DataTable dt = new DAL.Views.V_PlayTypes().Open("ID, LotteryID, Name, LotteryName, BuyFileName, Price", "LotteryID=" + Shove._Convert.StrToInt(LotteryID,-1).ToString(), "[ID]");

        if (dt == null || dt.Rows.Count == 0)
        {
            PF.GoError(ErrorNumber.Unknow, "此玩法没有开通", "Room_Chase");
            return;
        }

        LotteryName = dt.Rows[0]["LotteryName"].ToString();

        PlayTypeName = dt.Rows[0]["Name"].ToString();

        IframeUrl = dt.Rows[0]["BuyFileName"].ToString().Replace("Surrogate", "ChasePage").Replace("[Lottery]", dt.Rows[0]["LotteryID"].ToString()).Replace("[PlayType]", dt.Rows[0]["ID"].ToString());

        int j = 0;
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            LbPlay.Text += "<a href =" + dt.Rows[i]["BuyFileName"].ToString().Replace("Surrogate", "ChasePage").Replace("[Lottery]", dt.Rows[i]["LotteryID"].ToString()).Replace("[PlayType]", dt.Rows[i]["ID"].ToString()) + "&Price=" +dt.Rows[i]["Price"].ToString() + " target='iframeplay' class='hei'>" + dt.Rows[i]["Name"].ToString() + "追号</a>&nbsp;&nbsp;&nbsp;&nbsp;";

            if ((j % 6) == 0 && j > 0)
            {
                LbPlay.Text = LbPlay.Text.Trim() + " </br>";
            }

            j++;
        }
    }
}
