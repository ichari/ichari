using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Text;

public partial class JCZC_FilterShrink : System.Web.UI.Page
{
    public string Number;
    public string ID;

    protected void Page_Load(object sender, EventArgs e)
    {
        string request;

        using (Stream MyStream = Request.InputStream)
        {
            byte[] _tmpData = new byte[MyStream.Length];
            MyStream.Read(_tmpData, 0, _tmpData.Length);
            request = Encoding.UTF8.GetString(_tmpData);
        }

        Number = System.Web.HttpUtility.UrlDecode(request);

        if (Number.IndexOf("=") >= 0)
        {
            Number = Number.Substring(Number.IndexOf("=") + 1);
        }

        if (Number.Split(';').Length < 3)
        {
            Shove._Web.JavaScript.Alert(this.Page, "参数传递错误，请重新发起过滤！");

            return;
        }

        Users _User = Users.GetCurrentUser(1);

        ID = "-1";

        if (_User != null)
        {
            ID = _User.ID.ToString();
        }

        int PlayType = Shove._Convert.StrToInt(Number.Split(';')[0], 0);

        switch (PlayType)
        {
            case 7201:
                xaml1.InitParameters = "Page=1";
                break;
            case 7202:
                xaml1.InitParameters = "Page=2";
                break;
            case 7203:
                xaml1.InitParameters = "Page=3";
                break;
            case 7204:
                xaml1.InitParameters = "Page=4";
                break;
            default:
                xaml1.InitParameters = "Page=1";
                break;
        }
    }
}
