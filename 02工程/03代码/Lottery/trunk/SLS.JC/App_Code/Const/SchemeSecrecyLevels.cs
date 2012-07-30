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
/// 方案保密的种类
/// </summary>
public class SchemeSecrecyLevels
{
    /// <summary>
    /// 0 - 不保密
    /// </summary>
    public const short UnSecrecy = 0;
    /// <summary>
    /// 1 - 保密到截止投注时
    /// </summary>
    public const short ToDeadline = 1;
    /// <summary>
    /// 2 - 保密到开奖时
    /// </summary>
    public const short ToOpen = 2;
    /// <summary>
    /// 3 - 永远保密
    /// </summary>
    public const short Always = 3;

    public SchemeSecrecyLevels()
    {
        //
        // TODO: 在此处添加构造函数逻辑
        //
    }
}
