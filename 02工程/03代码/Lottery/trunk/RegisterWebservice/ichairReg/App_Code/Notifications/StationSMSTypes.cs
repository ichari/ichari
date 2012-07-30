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
/// 站内信的类型
/// </summary>
public class StationSMSTypes
{
    /// <summary>
    /// 站内信的类型：用户发送的消息
    /// </summary>
    public const short UserMessage = 1;
    /// <summary>
    /// 站内信的类型：系统发送的消息
    /// </summary>
    public const short SystemMessage = 2;

    public StationSMSTypes()
    {
        //
        // TODO: 在此处添加构造函数逻辑
        //
    }
}
