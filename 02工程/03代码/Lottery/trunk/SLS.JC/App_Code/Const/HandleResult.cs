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
/// 各种申请的处理结果
/// </summary>
public class HandleResult
{
    /// <summary>
    /// 申请中
    /// </summary>
    public const short Trying = 0;
    /// <summary>
    /// 已受理
    /// </summary>
    public const short Accepted = 1;
    /// <summary>
    /// 已答复
    /// </summary>
    public const short Answered = 2;
    /// <summary>
    /// 已拒绝
    /// </summary>
    public const short NoAcception = -1;

    public HandleResult()
    {
        //
        // TODO: 在此处添加构造函数逻辑
        //
    }
}
