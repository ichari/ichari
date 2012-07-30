using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
///SessionCenter 的摘要说明
/// </summary>
public class SynchronizeSessionID
{
    private System.Web.UI.Page page;

    public SynchronizeSessionID(System.Web.UI.Page _page)
    {
        if (_page == null)
        {
            throw new Exception("SynchronizeSessionID 类需要一个实例化的 System.Web.UI.Page 类型参数。");
        }

        page = _page;
    }

    public string GenSign(params string[] inputs)
    {
        string SourceString = "";

        foreach (string str in inputs)
        {
            SourceString += str;
        }

        return Shove._Security.Encrypt.MD5(SourceString + PF.CenterMD5Key);
    }

    public bool ValidSign(System.Web.HttpRequest request)
    {
        string SourceString = "";
        string Sign = "";

        foreach (string Key in request.QueryString.AllKeys)
        {
            if (String.Compare(Key, "Sign", StringComparison.OrdinalIgnoreCase) == 0)
            {
                Sign = HttpUtility.UrlDecode(request[Key]);
            }
            else
            {
                SourceString += HttpUtility.UrlDecode(request[Key]);
            }
        }

        return (String.Compare(Sign, Shove._Security.Encrypt.MD5(SourceString + PF.CenterMD5Key), StringComparison.OrdinalIgnoreCase) == 0);
    }
}
