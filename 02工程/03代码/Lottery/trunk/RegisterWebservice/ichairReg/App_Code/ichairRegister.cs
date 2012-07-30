using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

/// <summary>
///ichairRegister 的摘要说明
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
//若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消对下行的注释。 
// [System.Web.Script.Services.ScriptService]
public class ichairRegister : System.Web.Services.WebService
{

    public ichairRegister()
    {

        //如果使用设计的组件，请取消注释以下行 
        //InitializeComponent(); 
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="Name">用户名</param>
    /// <param name="Password">密码</param>
    /// <param name="Email">邮件</param>
    /// <param name="RealityName">真实姓名</param>
    /// <returns>返回1表示成功否则失败</returns>
    [WebMethod]
    public string UserRegister(string Name, string Password, string Email, string RealityName)
    {
        long CpsID = -1;
        long CommenderID = -1;
        string Memo = "";
        Users user = new Users(1);
        user.Name = Name;
        user.Password = Password;
        user.Email = Email;
        user.RealityName = RealityName;
        user.UserType = 2;

        user.CommenderID = CommenderID;
        user.CpsID = CpsID;
        user.Memo = Memo;
        string ReturnDescription=string.Empty;
        int Result = user.Add(ref ReturnDescription); 
        if (Result < 0)
        {
            new Log("Users").Write("会员注册不成功：" + ReturnDescription);
            return "会员注册不成功：" + ReturnDescription;

        }
        Result = user.Login(ref ReturnDescription);
        if (Result < 0)
        {
            new Log("Users").Write("注册成功后登录失败：" + ReturnDescription);
            return "注册成功后登录失败：" + ReturnDescription;
        }
        return user.ID.ToString();
    }


}
