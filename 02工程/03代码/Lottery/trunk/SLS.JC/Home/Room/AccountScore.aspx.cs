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

public partial class Home_Room_AccountScore : AdminPageBase
{


    int outScoreCount = 0;
    int inScoreCount = 0;
    double outScoreMoney = 0;
    double inScoreMoney = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        BindScoring();
    }
    protected void gScoring_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.EditItem)
        {
            double money;

            //e.Item.Cells[3].Text = "提款";

            string operatorType = e.Item.Cells[3].Text;
            switch (operatorType)
            {
                //1 购彩奖积分 2 下级购彩奖积分 3 系统奖励积分 4 手工增加积分 5 中奖赠送积分 101 兑换积分 201 惩罚扣积分

                case "1":
                    e.Item.Cells[3].Text = "购彩奖积分";
                    money = Shove._Convert.StrToDouble(e.Item.Cells[1].Text, 0);
                    e.Item.Cells[1].Text = (money == 0) ? "" : money.ToString("N");
                    if (money != 0)
                    {
                        inScoreCount++;
                        inScoreMoney += money;
                    }
                    break;
                case "2":
                    e.Item.Cells[3].Text = "下级购彩奖积分";
                    money = Shove._Convert.StrToDouble(e.Item.Cells[1].Text, 0);
                    e.Item.Cells[1].Text = (money == 0) ? "" : money.ToString("N");
                    if (money != 0)
                    {
                        inScoreCount++;
                        inScoreMoney += money;
                    }
                    break;
                case "3":
                    e.Item.Cells[3].Text = "系统奖励积分";
                    money = Shove._Convert.StrToDouble(e.Item.Cells[1].Text, 0);
                    e.Item.Cells[1].Text = (money == 0) ? "" : money.ToString("N");
                    if (money != 0)
                    {
                        inScoreCount++;
                        inScoreMoney += money;
                    }
                    break;
                case "4":
                    e.Item.Cells[3].Text = "手工增加积分";
                    money = Shove._Convert.StrToDouble(e.Item.Cells[1].Text, 0);
                    e.Item.Cells[1].Text = (money == 0) ? "" : money.ToString("N");
                    if (money != 0)
                    {
                        inScoreCount++;
                        inScoreMoney += money;
                    }
                    break;
                case "5":
                    e.Item.Cells[3].Text = "中奖赠送积分";
                    money = Shove._Convert.StrToDouble(e.Item.Cells[1].Text, 0);
                    e.Item.Cells[1].Text = (money == 0) ? "" : money.ToString("N");
                    if (money != 0)
                    {
                        inScoreCount++;
                        inScoreMoney += money;
                    }
                    break;
                case "101":
                    e.Item.Cells[3].Text = "兑换积分";
                    money = Shove._Convert.StrToDouble(e.Item.Cells[1].Text, 0);
                    e.Item.Cells[2].Text = (money == 0) ? "" : money.ToString("N");
                    if (money != 0)
                    {
                        e.Item.Cells[1].Text = "";
                        outScoreCount++;
                        outScoreMoney += money;
                    }
                    break;
                case "201":
                    e.Item.Cells[3].Text = "惩罚扣积分";
                    money = Shove._Convert.StrToDouble(e.Item.Cells[1].Text, 0);
                    e.Item.Cells[2].Text = (money == 0) ? "" : money.ToString("N");
                    if (money != 0)
                    {
                        e.Item.Cells[1].Text = "";
                        outScoreCount++;
                        outScoreMoney += money;
                    }
                    break;
                default:
                    e.Item.Cells[3].Text = "";
                    money = Shove._Convert.StrToDouble(e.Item.Cells[1].Text, 0);
                    e.Item.Cells[2].Text = (money == 0) ? "" : money.ToString("N");
                    if (money != 0)
                    {
                        outScoreCount++;
                        outScoreMoney += money;
                    }
                    break;
            }
        }
    }

    private void BindScoring()
    {
        if (_User == null)
        {
            return;
        }

        string CacheKeyName = "Room_UserScoring_" + _User.ID.ToString();

        DataTable dt = Shove._Web.Cache.GetCacheAsDataTable(CacheKeyName);

        if (dt == null)
        {
            dt = new DAL.Views.V_UserScoringDetails().Open("ID,[DateTime],[Scoring],OperatorType", "[UserID] = " + _User.ID.ToString() + "", "[DateTime] desc, [ID]");

            if (dt == null)
            {
                PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试(732)", this.GetType().FullName);

                return;
            }

            Shove._Web.Cache.SetCache(CacheKeyName, dt, 60);
        }

        gScoring.DataSource = dt;
        gScoring.DataBind();

        this.lblScoreInCount.Text = inScoreCount.ToString();
        this.lblScoreOutCount.Text = outScoreCount.ToString();
        this.lblScoreInMoney.Text = inScoreMoney.ToString("N");
        this.lblScoreOutMoney.Text = outScoreMoney.ToString("N");
    }
    #region Web 窗体设计器生成的代码

    override protected void OnInit(EventArgs e)
    {
        RequestLoginPage = this.Request.Url.AbsoluteUri;

        base.OnInit(e);
    }

    #endregion

    protected void g_SortCommand(object source, DataGridSortCommandEventArgs e)
    {
        DataGrid grid = (DataGrid)source;
        switch (grid.ID)
        {
            
            case "gScoring":
                BindScoring();
                break;
        }

    }
    protected void gPager_PageWillChange(object Sender, Shove.Web.UI.PageChangeEventArgs e)
    {
        string gPagerId = ((Shove.Web.UI.ShoveGridPager)(Sender)).ID;
        switch (gPagerId)
        {
          
            case "gPagerScoring":
                hdCurDiv.Value = "divScoring";
                BindScoring();
                break;
        }
    }
}
