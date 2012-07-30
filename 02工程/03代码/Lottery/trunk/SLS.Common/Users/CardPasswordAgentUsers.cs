using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

using Shove.Database;

namespace SLS.Common
{
    public class CardPasswordAgentUsers
    {
        #region 成员变量

        public int ID;
        public string Password;
        public string Name;
        public string Company;
        public string Url;
        public short State;

        #endregion

        public CardPasswordAgentUsers()
        {
            ID = -1;
            Name = "";
            Password = "";
            Company = "";
            Url = "";
            State = 0;
        }

        // 正常用户登录
        public int Login(ref string ReturnDescription)
        {
            DataTable dt = new SLS.Dal.Tables.T_CardPasswordAgents().Open(PF.ConnectString,"", "ID=" + ID, "");

            if ((dt == null) || (dt.Rows.Count < 1))
            {
                ReturnDescription = "用户不存在";

                return -1;
            }

            if (dt.Rows[0]["Password"].ToString() != PF.EncryptPassword(Password))
            {
                ReturnDescription = "密码错误";

                return -2;
            }

            //if (!Shove._Convert.StrToBool(dt.Rows[0]["State"].ToString(), false))
            if (dt.Rows[0]["State"].ToString() != "1")
            {
                ReturnDescription = "代理商帐号已经过期";

                return -2;
            }

            Name = dt.Rows[0]["Name"].ToString();
            Password = dt.Rows[0]["Password"].ToString();
            Company = dt.Rows[0]["Company"].ToString();
            Url = dt.Rows[0]["Url"].ToString();
            State = Shove._Convert.StrToShort(dt.Rows[0]["State"].ToString(), 0);

            // 校验成功
            SaveUserIDToCookie();

            return 0;
        }

        // 注销
        public int Logout(ref string ReturnDescription)
        {
            ReturnDescription = "";

            RemoveUserIDFromCookie();

            return 0;
        }

        public int GetInformationByID(ref string ReturnDescription)
        {
            if (ID < 0)
            {
                throw new Exception("CardPassword 尚未初始化到具体的数据实例上，请先使用 GetInformation 等获取数据信息");
            }

            DataTable dt = new SLS.Dal.Tables.T_CardPasswordAgents().Open(PF.ConnectString,"", "[ID] = " + ID, "");

            if ((dt == null) || (dt.Rows.Count < 1))
            {
                ReturnDescription = "数据库读写错误";

                return -1;
            }

            Name = dt.Rows[0]["Name"].ToString();
            Password = dt.Rows[0]["Password"].ToString();
            Company = dt.Rows[0]["Company"].ToString();
            Url = dt.Rows[0]["Url"].ToString();
            State = Shove._Convert.StrToShort(dt.Rows[0]["State"].ToString(), 0);

            return 0;
        }

        public int GetInformationByName(ref string ReturnDescription)
        {
            if (Name == "")
            {
                throw new Exception("CardPassword 尚未初始化到具体的数据实例上，请先使用 GetInformation 等获取数据信息");
            }

            DataTable dt = new SLS.Dal.Tables.T_CardPasswordAgents().Open(PF.ConnectString,"", "[Name] = " + Shove._Web.Utility.FilteSqlInfusion(Name), "");

            if ((dt == null) || (dt.Rows.Count < 1))
            {
                ReturnDescription = "数据库读写错误";

                return -1;
            }

            ID = Shove._Convert.StrToInt(dt.Rows[0]["ID"].ToString(), 0);
            Password = dt.Rows[0]["Password"].ToString();
            Company = dt.Rows[0]["Company"].ToString();
            Url = dt.Rows[0]["Url"].ToString();
            State = Shove._Convert.StrToShort(dt.Rows[0]["State"].ToString(), 0);

            return 0;
        }

        public int EditByID(ref string ReturnDescription)
        {
            if (ID < 0)
            {
                throw new Exception("CardPassword 尚未初始化到具体的数据实例上，请先使用 GetInformation 等获取数据信息");
            }

            DataTable dt = new SLS.Dal.Tables.T_CardPasswordAgents().Open(PF.ConnectString,"", "[ID] = " + ID, "");

            if ((dt == null) || (dt.Rows.Count < 1))
            {
                ReturnDescription = "数据库读写错误";

                return -1;
            }

            SLS.Dal.Tables.T_CardPasswordAgents t_CardPasswordAgents = new SLS.Dal.Tables.T_CardPasswordAgents();

            t_CardPasswordAgents.Company.Value = Company;
            t_CardPasswordAgents.Password.Value = Password;
            t_CardPasswordAgents.Name.Value = Name;
            t_CardPasswordAgents.State.Value = State;
            t_CardPasswordAgents.Url.Value = Url;

            t_CardPasswordAgents.Update(PF.ConnectString,"[ID] = " + ID.ToString());

            return 0;
        }

        private void SaveUserIDToCookie()
        {
            string Key = (System.Web.HttpContext.Current.Session.SessionID + "").ToLower() + "_CardPasswordAgentUsers";

            // 写入 Cookie
            HttpCookie cookieUser = new HttpCookie(Key, Shove._Security.Encrypt.Encrypt3DES(PF.GetCallCert(), Shove._Security.Encrypt.EncryptString(PF.GetCallCert(), ID.ToString()), PF.DesKey));

            try
            {
                HttpContext.Current.Response.Cookies.Add(cookieUser);
            }
            catch { }
        }

        private void RemoveUserIDFromCookie()
        {
            string Key = (System.Web.HttpContext.Current.Session.SessionID + "").ToLower() + "_CardPasswordAgentUsers";

            HttpCookie cookieUser = new HttpCookie(Key);
            cookieUser.Value = "";
            cookieUser.Expires = DateTime.Now.AddYears(-20);

            try
            {
                HttpContext.Current.Response.Cookies.Add(cookieUser);
            }
            catch { }
        }

        public static CardPasswordAgentUsers GetCurrentUser()
        {
            string Key = (System.Web.HttpContext.Current.Session.SessionID + "").ToLower() + "_CardPasswordAgentUsers";

            // 从 Cookie 中取出 UserID
            HttpCookie cookieUser = HttpContext.Current.Request.Cookies[Key];

            if ((cookieUser == null) || (String.IsNullOrEmpty(cookieUser.Value)))
            {
                return null;
            }

            string CookieUserID = cookieUser.Value;

            try
            {
                CookieUserID = Shove._Security.Encrypt.UnEncryptString(PF.GetCallCert(), Shove._Security.Encrypt.Decrypt3DES(PF.GetCallCert(), CookieUserID, PF.DesKey));
            }
            catch
            {
                CookieUserID = "";
            }

            if (String.IsNullOrEmpty(CookieUserID))
            {
                return null;
            }

            int UserID = Shove._Convert.StrToInt(CookieUserID, -1);

            if (UserID < 1)
            {
                return null;
            }

            CardPasswordAgentUsers cardPasswordUsers = new CardPasswordAgentUsers();

            cardPasswordUsers.ID = UserID;

            string ReturnDescription = "";
            int Result = cardPasswordUsers.GetInformationByID(ref ReturnDescription);

            if (Result < 0)
            {
                return null;
            }

            return cardPasswordUsers;
        }

        public void Clone(CardPasswordAgentUsers cardPasswordUsers)
        {
            cardPasswordUsers.ID = ID;
            cardPasswordUsers.Company = Company;
            cardPasswordUsers.Password = Password;
            cardPasswordUsers.Name = Name;
            cardPasswordUsers.State = State;
            cardPasswordUsers.Url = Url;
        }
    }
}