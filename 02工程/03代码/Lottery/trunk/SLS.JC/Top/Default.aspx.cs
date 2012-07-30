using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Top_Default : SitePageBase
{
    public string ImageUrl = "";
    public int LotteryID = 0;
    public string lot = "";
    public string DTime = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        lot = Shove._Web.Utility.GetRequest("lot").ToString();
        //((HiddenField)phHead.FindControl("currentMenu")).Value = "mHome";
        if (string.IsNullOrEmpty(lot))
        {
            lot = "jczq";
        }

        ImageUrl = "Images/"+ lot +".jpg";

        switch (lot)
        {
            case "sfc":
                LotteryID = 74;
                break;
            case "rj":
                LotteryID = 75;
                break;
            case "6cbq":
                LotteryID = 15;
                break;
            case "4cjq":
                LotteryID = 2;
                break;
            case "dlt":
                LotteryID = 39;
                break;
            case "pls":
                LotteryID = 63;
                break;
            case "qxc":
                LotteryID = 3;
                break;
            case "22x5":
                LotteryID = 9;
                break;
            case "jczq":
                LotteryID = 72;
                break;
            case "jclq":
                LotteryID = 73;
                break;
            default:
                LotteryID = 72;
                break;
        }

        DTime = DateTime.Now.ToString("yyyy-MM-dd");
    }
}
