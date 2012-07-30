using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

using System.IO;

/// <summary>
///HmtlManage 的摘要说明
/// </summary>
public class HmtlManage
{
    public HmtlManage()
    {
        //
        //TODO: 在此处添加构造函数逻辑
        //
    }

    public static string GetHtml(string path)
    {
        if (!File.Exists(path))
        {
            new Log("System").Write("未找到文件 " + path);

            return "";
        }

        FileInfo fi = new FileInfo(path);
        DateTime htmlTime = fi.LastWriteTime;
        string appKey = "Html_" + fi.Name;

        object obj = HttpContext.Current.Application[appKey];

        string html = "";
        DateTime appTime = htmlTime;

        if (obj == null)
        {
            StreamReader sr = new StreamReader(path, System.Text.Encoding.GetEncoding("GBK"));
            html = sr.ReadToEnd();

            sr.Close();
        }
        else
        {
            object[] objArr = (object[])obj;
            html = (string)objArr[0];
            appTime = (DateTime)objArr[1];
        }

        if (htmlTime != appTime)
        {
            object[] objArr = new object[] { html, appTime };

            HttpContext.Current.Application.Lock();
            HttpContext.Current.Application[appKey] = objArr;
            HttpContext.Current.Application.UnLock();
        }

        return html;
    }
}
