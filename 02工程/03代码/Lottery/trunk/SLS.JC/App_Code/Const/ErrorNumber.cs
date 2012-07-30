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
/// 错误号
/// </summary>
public class ErrorNumber
{
    /// <summary>
    /// 未知错误
    /// </summary>
    public const short Unknow = 1;
    /// <summary>
    /// 没有登录
    /// </summary>
    public const short NoLogin = 2;
    /// <summary>
    /// 没有足够的权限
    /// </summary>
    public const short NotEnoughCompetence = 3;
    /// <summary>
    /// 数据读写错误
    /// </summary>
    public const short DataReadWrite = 4;
    /// <summary>
    /// 账户余额不足
    /// </summary>
    public const short NotEnoughBalance = 5;
    /// <summary>
    /// 没有期号
    /// </summary>
    public const short NoIsuse = 6;
    /// <summary>
    /// 没有数据
    /// </summary>
    public const short NoData = 7;

    public ErrorNumber()
    {
        //
        // TODO: 在此处添加构造函数逻辑
        //
    }
}
