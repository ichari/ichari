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
/// 通知的方式 1 手机短信 2 Email 3 站内信
/// </summary>
public class NotificationManners
{
    /// <summary>
    /// 通知方式：手机短信
    /// </summary>
    public const short SMS = 1;
    /// <summary>
    /// 通知方式：Email
    /// </summary>
    public const short Email = 2;
    /// <summary>
    /// 通知方式：站内信
    /// </summary>
    public const short StationSMS = 3;

    /// <summary>
    /// 通知方式最小值，用于比较
    /// </summary>
    public const short Min = 1;
    /// <summary>
    /// 通知方式最大值，用于比较
    /// </summary>
    public const short Max = 3;

    public NotificationManners()
    {
        //
        // TODO: 在此处添加构造函数逻辑
        //
    }
}
