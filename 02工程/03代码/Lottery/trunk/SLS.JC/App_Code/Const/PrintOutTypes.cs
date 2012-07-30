using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

/// <summary>
/// PrintOutTypes 的摘要说明
/// </summary>
public class PrintOutTypes
{
    public const short Local = 0;
    
    public const short Interface_HPCQ = 101;
    public const short Interface_HPSH = 102;

    public const short Interface_XGCQ = 201;
    public const short Interface_XGSH = 202;

    public const short Interface_SunBJ = 301;
    public PrintOutTypes()
    {
        //
        // TODO: 在此处添加构造函数逻辑
        //
    }
}
