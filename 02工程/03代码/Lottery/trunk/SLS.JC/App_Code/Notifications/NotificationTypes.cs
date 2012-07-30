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
/// 通知的种类
/// </summary>
public class NotificationTypes
{
    /// <summary>
    /// 当注册普通会员成功时，发送(接收)通知
    /// </summary>
    public const string Register = "Register";
    /// <summary>
    /// 当注册高级会员成功时，发送(接收)通知
    /// </summary>
    public const string RegisterAdv = "RegisterAdv";
    /// <summary>
    /// 当忘记密码，并申请找回时，发送(接收)通知
    /// </summary>
    public const string ForgetPassword = "ForgetPassword";
    /// <summary>
    /// 当用户资料被修改时，发送(接收)通知
    /// </summary>
    public const string UserEdit = "UserEdit";
    /// <summary>
    /// 当用户高级资料被修改时，发送(接收)通知
    /// </summary>
    public const string UserEditAdv = "UserEditAdv";
    /// <summary>
    /// 当用户发起一个方案时，发送(接收)通知
    /// </summary>
    public const string InitiateScheme = "InitiateScheme";
    /// <summary>
    /// 当用户入伙一个方案时，发送(接收)通知
    /// </summary>
    public const string JoinScheme = "JoinScheme";
    /// <summary>
    /// 当用户发起一个追号任务时，发送(接收)通知
    /// </summary>
    public const string InitiateChaseTask = "InitiateChaseTask";
    /// <summary>
    /// 当用户的追号任务其中某期到达执行时间，并被系统自动执行时，发送(接收)通知
    /// </summary>
    public const string ExecChaseTaskDetail = "ExecChaseTaskDetail";
    /// <summary>
    /// 当用户发出提款申请时，发送(接收)通知
    /// </summary>
    public const string TryDistill = "TryDistill";
    /// <summary>
    /// 当用户发出的提款申请被受理时，发送(接收)通知
    /// </summary>
    public const string DistillAccept = "DistillAccept";
    /// <summary>
    /// 当用户发出的提款申请被拒绝时，发送(接收)通知
    /// </summary>
    public const string DistillNoAccept = "DistillNoAccept";
    /// <summary>
    /// 当用户从某方案中撤单时(不是撤销整个方案)，发送(接收)通知
    /// </summary>
    public const string Quash = "Quash";
    /// <summary>
    /// 当用户作为发起人，撤销整个方案时，发送(接收)通知
    /// </summary>
    public const string QuashScheme = "QuashScheme";
    /// <summary>
    /// 当用户撤销整个追号任务中的某些期(追号明细任务)时，发送(接收)通知
    /// </summary>
    public const string QuashChaseTaskDetail = "QuashChaseTaskDetail";
    /// <summary>
    /// 当用户撤销整个追号任务时，发送(接收)通知
    /// </summary>
    public const string QuashChaseTask = "QuashChaseTask";
    /// <summary>
    /// 当用户中奖时，发送(接收)通知
    /// </summary>
    public const string Win = "Win";
    /// <summary>
    /// 当用户申请手机验证时，发送(接收)的通知
    /// </summary>
    public const string MobileValid = "MobileValid";
    /// <summary>
    /// 当用户手机通过验证时，发送(接收)的通知
    /// </summary>
    public const string MobileValided = "MobileValided";

    /// <summary>
    /// 当用户发起追号套餐，发送(接收)的通知
    /// </summary>
    public const string IntiateCustomChase = "IntiateCustomChase";

    /// <summary>
    /// 当用户发起的追号套餐中奖时，发送(接收)的通知
    /// </summary>
    public const string CustomChaseWin = "CustomChaseWin";

    public NotificationTypes()
    {
        //
        // TODO: 在此处添加构造函数逻辑
        //
    }

    public string this[short Index]
    {
        get
        {
            switch (Index)
            {
                case 1:
                    return Register;
                case 2:
                    return RegisterAdv;
                case 3:
                    return ForgetPassword;
                case 4:
                    return UserEdit;
                case 5:
                    return UserEditAdv;
                case 6:
                    return InitiateScheme;
                case 7:
                    return JoinScheme;
                case 8:
                    return InitiateChaseTask;
                case 9:
                    return ExecChaseTaskDetail;
                case 10:
                    return TryDistill;
                case 11:
                    return DistillAccept;
                case 12:
                    return DistillNoAccept;
                case 13:
                    return Quash;
                case 14:
                    return QuashScheme;
                case 15:
                    return QuashChaseTaskDetail;
                case 16:
                    return QuashChaseTask;
                case 17:
                    return Win;
                case 18:
                    return MobileValid;
                case 19:
                    return MobileValided;
                case 20:
                    return IntiateCustomChase;
                case 21:
                    return CustomChaseWin;
                default:
                    return "";
            }
        }
    }
}
