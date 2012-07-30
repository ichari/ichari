using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections;

using Shove.Database;
using System.Text;
using System.Security.Cryptography;
using System.IO;

[Serializable]
public class Users
{
    #region 成员变量

    private long _id;
    private long _siteid;
    private string _password;
    private string _passwordadv;

    public long ID
    {
        set
        {
            _id = value;
            cps.OwnerUserID = value;

            string ReturnDescription = "";

            if (this.GetUserInformationByID(ref ReturnDescription) < 0)
            {
                _id = -1;
                cps.OwnerUserID = -1;
            }
        }
        get
        {
            return _id;
        }
    }

    public long SiteID
    {
        set
        {
            _siteid = value;

            Site.ID = value;

            string ReturnDescription = "";

            Site.GetSiteInformationByID(ref ReturnDescription);

            cps.SiteID = value;
        }
        get
        {
            return _siteid;
        }
    }

    public string Name;
    public string NickName;
    public string RealityName;

    public string Password
    {
        set
        {
            _password = PF.EncryptPassword(value);
        }
        get
        {
            return _password;
        }
    }

    public string PasswordAdv
    {
        set
        {
            _passwordadv = PF.EncryptPassword(value);
        }
        get
        {
            return _passwordadv;
        }
    }

    public string Sex;
    public DateTime BirthDay;
    public string IDCardNumber;
    public int CityID;
    public string Address;
    public string Email;
    public bool isEmailValided;
    public string QQ;
    public bool isQQValided;
    public string Telephone;
    public string Mobile;
    public bool isMobileValided;

    public bool isPrivacy;
    public bool isCanLogin;

    public DateTime RegisterTime;
    public DateTime LastLoginTime;
    public string LastLoginIP;
    public int LoginCount;

    public short UserType;

    public short BankType;
    public string BankName;
    public string BankCardNumber;

    public double Balance;
    public double Freeze;
    public double ScoringOfSelfBuy;
    public double ScoringOfCommendBuy;
    public double Scoring;
    public double Bonus;
    public double Reward;

    public short Level;

    public long CommenderID;
    public long CpsID;
    public bool isAlipayCps;

    public string AlipayID;
    public string AlipayName;
    public bool isAlipayNameValided;

    public string Memo;
    public double BonusThisMonth;
    public double BonusAllow;
    public double BonusUse;
    public double PromotionMemberBonusScale;
    public double PromotionSiteBonusScale;

    public Cps cps;

    public Sites Site = new Sites();

    public Competences Competences;

    public string OwnerSites;

    public int ComeFrom;
    public bool IsCrossLogin;

    public string VisitSource;

    public string HeadUrl = "";
    public string SecurityQuestion;
    public string SecurityAnswer;
    public string FriendList;
    public string Reason;
    #endregion

    #region 索引器

    public Users this[long siteid, long id]
    {
        get
        {
            Users user = new Users(siteid);

            user.ID = id;

            return user;
        }
    }

    public Users this[long siteid, string name]
    {
        get
        {
            Users user = new Users(siteid);

            user.Name = name;

            string ReturnDescription = "";

            if (user.GetUserInformationByName(ref ReturnDescription) < 0)
            {
                return null;
            }

            return user;
        }
    }

    #endregion

    public Users()
    {
        throw new Exception("不能用无参数的 Users 类的构造来申明实例。此无参数构造函数是为了能使其序列化。");
    }

    public Users(long siteid)
    {
        cps = new Cps(this);
        Competences = new Competences(this);


        SiteID = siteid;

        _id = -1;

        Name = "";
        NickName = "";
        RealityName = "";
        Password = "";
        PasswordAdv = "";

        Sex = "男";
        BirthDay = DateTime.Parse("1980-01-01");
        IDCardNumber = "";
        CityID = 1;
        Address = "";
        Email = "";
        isEmailValided = false;
        QQ = "";
        isQQValided = false;
        Telephone = "";
        Mobile = "";
        isMobileValided = false;
        isPrivacy = false;
        isCanLogin = true;
        RegisterTime = DateTime.Now;
        LastLoginTime = DateTime.Now;

        try
        {
            LastLoginIP = System.Web.HttpContext.Current.Request.UserHostAddress;
        }
        catch
        {
            LastLoginIP = "";
        }

        LoginCount = 0;
        UserType = 1;

        BankType = 1;
        BankName = "";
        BankCardNumber = "";

        Balance = 0;
        Freeze = 0;
        ScoringOfSelfBuy = 0;
        ScoringOfCommendBuy = 0;
        Scoring = 0;

        Level = 0;

        CommenderID = -1;
        CpsID = -1;
        isAlipayCps = false;

        Bonus = 0;
        Reward = 0;

        AlipayID = "";
        AlipayName = "";
        isAlipayNameValided = false;

        Memo = "";
        BonusThisMonth = 0;
        BonusAllow = 0;
        BonusUse = 0;
        PromotionMemberBonusScale = 0;
        PromotionSiteBonusScale = 0;

        OwnerSites = "";

        ComeFrom = 0;
        IsCrossLogin = false;

        VisitSource = "";

        HeadUrl = "";
        SecurityQuestion = "";
        SecurityAnswer = "";
        FriendList = "";
        Reason = "";
    }

    public int Add(ref string ReturnDescription)
    {
        RegisterTime = DateTime.Now;
        ReturnDescription = "";

        int ReturnValue = DAL.Procedures.P_UserAdd(SiteID, Name, RealityName, Password, PasswordAdv, CityID, Sex, BirthDay, IDCardNumber,
            Address, Email, isEmailValided, QQ,isQQValided, Telephone, Mobile, isMobileValided, isPrivacy, UserType, BankType, BankName, BankCardNumber, CommenderID, CpsID, AlipayName, Memo, VisitSource, ref _id, ref ReturnDescription);

        if (ReturnValue < 0)
        {
            ReturnDescription = "数据库读写错误";

            return -1;
        }

        if (_id < 0)
        {
            return (int)_id;
        }

        ID = _id;

        //Send Email
        string EmailSubject = "", EmailBody = "";

        if (UserType == 1)
        {
            Site.SiteNotificationTemplates.SplitEmailTemplate(Site.SiteNotificationTemplates[NotificationManners.Email, NotificationTypes.Register], ref EmailSubject, ref EmailBody);

            if ((EmailSubject != "") && (EmailBody != ""))
            {
                EmailSubject = EmailSubject.Replace("[UserName]", Name);

                EmailBody = EmailBody.Replace("[UserName]", Name);
                EmailBody = EmailBody.Replace("[UserPassword]", Password);
                EmailBody = EmailBody.Replace("[UserEmail]", Email);

                PF.SendEmail(Site, Email, EmailSubject, EmailBody);
            }
        }
        else
        {
            Site.SiteNotificationTemplates.SplitEmailTemplate(Site.SiteNotificationTemplates[NotificationManners.Email, NotificationTypes.RegisterAdv], ref EmailSubject, ref EmailBody);

            if ((EmailSubject != "") && (EmailBody != "") && (DAL.Functions.F_GetIsSendNotification(SiteID, NotificationManners.Email, NotificationTypes.RegisterAdv, ID)))
            {
                EmailSubject = EmailSubject.Replace("[UserRealityName]", RealityName);

                EmailBody = EmailBody.Replace("[UserRealityName]", RealityName);
                EmailBody = EmailBody.Replace("[UserName]", Name);
                EmailBody = EmailBody.Replace("[UserPassword]", Password);
                EmailBody = EmailBody.Replace("[UserPassword_2]", PasswordAdv);
                EmailBody = EmailBody.Replace("[UserEmail]", Email);
                EmailBody = EmailBody.Replace("[UserIDCardNumber]", IDCardNumber);
                EmailBody = EmailBody.Replace("[UserCity]", DAL.Functions.F_GetProvinceCity(CityID));
                EmailBody = EmailBody.Replace("[UserSex]", Sex);
                EmailBody = EmailBody.Replace("[UserBirthDay]", BirthDay.ToShortDateString());
                EmailBody = EmailBody.Replace("[UserTelphone]", Telephone);
                EmailBody = EmailBody.Replace("[UserMobile]", Mobile);
                EmailBody = EmailBody.Replace("[UserQQ]", QQ);
                EmailBody = EmailBody.Replace("[UserAddress]", Address);
                EmailBody = EmailBody.Replace("[UserBankType]", DAL.Functions.F_GetBankTypeName(BankType));
                EmailBody = EmailBody.Replace("[UserBankName]", BankName);
                EmailBody = EmailBody.Replace("[UserBankCardNumber]", BankCardNumber);

                PF.SendEmail(Site, Email, EmailSubject, EmailBody);
            }
        }

        return (int)_id;
    }

    #region 修改用户资料

    public int EditByID(ref string ReturnDescription)
    {
        if (ID < 0)
        {
            throw new Exception("Users 尚未初始化到具体的数据实例上，请先使用 GetUserInformation 等获取数据信息");
        }

        int ReturnValue = -1;
        ReturnDescription = "";

        int Result = DAL.Procedures.P_UserEditByID(SiteID, ID, Name, RealityName, Password, PasswordAdv, CityID, Sex, BirthDay, IDCardNumber,
            Address, Email, isEmailValided, QQ,isQQValided, Telephone, Mobile, isMobileValided, isPrivacy, isCanLogin, UserType, BankType, BankName, BankCardNumber,
            ScoringOfSelfBuy, ScoringOfCommendBuy, Level, AlipayID, AlipayName, isAlipayNameValided, PromotionMemberBonusScale, PromotionSiteBonusScale, IsCrossLogin,Reason,
            ref ReturnValue, ref ReturnDescription);

        if (Result < 0)
        {
            ReturnDescription = "数据库读写错误";

            return -1;
        }

        if (ReturnValue < 0)
        {
            return ReturnValue;
        }

        SendNotificationForUserEdit();

        return 0;
    }

    public int EditByName(ref string ReturnDescription)
    {
        if (ID < 0)
        {
            throw new Exception("Users 尚未初始化到具体的数据实例上，请先使用 GetUserInformation 等获取数据信息");
        }

        int ReturnValue = -1;
        ReturnDescription = "";

        int Result = DAL.Procedures.P_UserEditByName(SiteID, ID, Name, RealityName, Password, PasswordAdv, CityID, Sex, BirthDay, IDCardNumber,
            Address, Email, isEmailValided, QQ,isQQValided, Telephone, Mobile, isMobileValided, isPrivacy, isCanLogin, UserType, BankType, BankName, BankCardNumber,
            ScoringOfSelfBuy, ScoringOfCommendBuy, Level, AlipayID, AlipayName, isAlipayNameValided, PromotionMemberBonusScale, PromotionSiteBonusScale, IsCrossLogin,Reason,
            ref ReturnValue, ref ReturnDescription);

        if (Result < 0)
        {
            ReturnDescription = "数据库读写错误";

            return -1;
        }

        if (ReturnValue < 0)
        {
            return ReturnValue;
        }

        SendNotificationForUserEdit();

        return 0;
    }

    private void SendNotificationForUserEdit()
    {
        if (UserType == 1)
        {
            //Send Email
            if ((Email != "") && (DAL.Functions.F_GetIsSendNotification(SiteID, NotificationManners.Email, NotificationTypes.UserEdit, ID)))
            {
                string EmailSubject = "", EmailBody = "";

                Site.SiteNotificationTemplates.SplitEmailTemplate(Site.SiteNotificationTemplates[NotificationManners.Email, NotificationTypes.UserEdit], ref EmailSubject, ref EmailBody);

                if ((EmailSubject != "") && (EmailBody != ""))
                {
                    EmailSubject = EmailSubject.Replace("[UserName]", Name);

                    EmailBody = EmailBody.Replace("[UserName]", Name);
                    EmailBody = EmailBody.Replace("[UserPassword]", Password);
                    EmailBody = EmailBody.Replace("[UserEmail]", Email);

                    PF.SendEmail(Site, Email, EmailSubject, EmailBody);
                }
            }

            return;
        }

        //Send Email
        if ((Email != "") && (DAL.Functions.F_GetIsSendNotification(SiteID, NotificationManners.Email, NotificationTypes.UserEditAdv, ID)))
        {
            string EmailSubject = "", EmailBody = "";

            Site.SiteNotificationTemplates.SplitEmailTemplate(Site.SiteNotificationTemplates[NotificationManners.Email, NotificationTypes.UserEditAdv], ref EmailSubject, ref EmailBody);

            if ((EmailSubject != "") && (EmailBody != ""))
            {
                StringBuilder sb = new StringBuilder();

                sb.Append("select BankTypeName,BankName, BankCardNumber, BankInProvinceName, BankInCityName, BankUserName from  T_userBankBindDetails  ")
                    .Append(" where  UserID = " + ID.ToString() + " ");

                DataTable dt = Shove.Database.MSSQL.Select(sb.ToString());

                if ((dt != null) && dt.Rows.Count > 0)
                {
                    EmailBody = EmailBody.Replace("[UserBankType]", dt.Rows[0]["BankTypeName"].ToString().Trim());
                    EmailBody = EmailBody.Replace("[UserBankName]", dt.Rows[0]["BankName"].ToString().Trim());
                    EmailBody = EmailBody.Replace("[UserBankCardNumber]", dt.Rows[0]["BankCardNumber"].ToString().Trim());
                }

                EmailSubject = EmailSubject.Replace("[UserRealityName]", RealityName);

                EmailBody = EmailBody.Replace("[UserRealityName]", RealityName);
                EmailBody = EmailBody.Replace("[UserName]", Name);
                EmailBody = EmailBody.Replace("[UserPassword]", Password);
                EmailBody = EmailBody.Replace("[UserPassword_2]", PasswordAdv);
                EmailBody = EmailBody.Replace("[UserEmail]", Email);
                EmailBody = EmailBody.Replace("[UserIDCardNumber]", IDCardNumber);
                EmailBody = EmailBody.Replace("[UserCity]", DAL.Functions.F_GetProvinceCity(CityID));
                EmailBody = EmailBody.Replace("[UserSex]", Sex);
                EmailBody = EmailBody.Replace("[UserBirthDay]", BirthDay.ToShortDateString());
                EmailBody = EmailBody.Replace("[UserTelephone]", Telephone);
                EmailBody = EmailBody.Replace("[UserMobile]", Mobile);
                EmailBody = EmailBody.Replace("[UserQQ]", QQ);
                EmailBody = EmailBody.Replace("[UserAddress]", Address);

                PF.SendEmail(Site, Email, EmailSubject, EmailBody);
            }
        }

        //Send SMS
        if ((Mobile != "") && isMobileValided && (DAL.Functions.F_GetIsSendNotification(SiteID, NotificationManners.SMS, NotificationTypes.UserEditAdv, ID)))
        {
            string Body = Site.SiteNotificationTemplates[NotificationManners.SMS, NotificationTypes.UserEditAdv];

            if (Body != "")
            {
                Body = Body.Replace("[UserRealityName]", RealityName);

                PF.SendSMS(Site, ID, Mobile, Body);
            }
        }

        //Send StationSMS
        if (DAL.Functions.F_GetIsSendNotification(SiteID, NotificationManners.StationSMS, NotificationTypes.UserEditAdv, ID))
        {
            string Body = Site.SiteNotificationTemplates[NotificationManners.StationSMS, NotificationTypes.UserEditAdv];

            if (Body != "")
            {
                Body = Body.Replace("[UserRealityName]", RealityName);

                PF.SendStationSMS(Site, Site.AdministratorID, ID, StationSMSTypes.SystemMessage, Body);
            }
        }
    }

    #endregion

    #region 用户登录

    // 正常用户登录
    public int Login(ref string ReturnDescription)
    {
        int ReturnValue = -1;
        ReturnDescription = "";

        int Result = DAL.Procedures.P_Login(SiteID, Name, Password, LastLoginIP, ref _id, ref _passwordadv, ref RealityName, ref CityID, ref Sex, ref BirthDay,
            ref IDCardNumber, ref Address, ref Email, ref isEmailValided, ref QQ,ref isQQValided, ref Telephone, ref Mobile, ref isMobileValided, ref isPrivacy, ref isCanLogin, ref RegisterTime, ref LastLoginTime,
            ref LastLoginIP, ref LoginCount, ref UserType, ref BankType, ref BankName, ref BankCardNumber, ref Balance, ref Freeze, ref ScoringOfSelfBuy, ref ScoringOfCommendBuy,
            ref Scoring, ref Level, ref CommenderID, ref CpsID, ref AlipayID, ref AlipayName, ref isAlipayNameValided, ref Bonus, ref Reward, ref Memo, ref BonusThisMonth, ref BonusAllow, ref BonusUse, ref PromotionMemberBonusScale, ref PromotionSiteBonusScale,
            ref ReturnValue, ref ReturnDescription);

        if (Result < 0)
        {
            return Result;
        }

        if (ReturnValue < 0)
        {
            return ReturnValue;
        }

        ID = _id;

        // 校验成功
        SaveUserIDToCookie();

        return 0;
    }

    // 直接设置了登录状态
    public int LoginDirect(ref string ReturnDescription)
    {
        if (ID < 0)
        {
            throw new Exception("Users 尚未初始化到具体的数据实例上，请先使用 GetUserInformation 等获取数据信息");
        }

        ReturnDescription = "";

        if (!isCanLogin)
        {
            ReturnDescription = "用户被限制登录";

            return -1;
        }

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
    //encrypts the user id to cookie and stores it as key, value pair. can do own encryption to achieve Single Sing On
    private void SaveUserIDToCookie()
    {
        
        //string Key = ConfigurationManager.AppSettings["CookieName"].ToString();
        HttpCookie cookieLoginUserID = DataEncryption.MakeCookie("", ID);
        //HttpCookie cookieLoginUserID = new HttpCookie(Key, Shove._Security.Encrypt.Encrypt3DES(PF.GetCallCert(), Shove._Security.Encrypt.EncryptString(PF.GetCallCert(), ID.ToString()), PF.DesKey));
        //string cID = DataEncryption.EncryptAES(ID.ToString());
        //DataEncryption.SaveToCookies("", ID);
        //cookieLoginUserID.Values.Add("cp", cID);

        try
        {
            HttpContext.Current.Response.Cookies.Add(cookieLoginUserID);
        }
        catch { }
    }

    private void RemoveUserIDFromCookie()
    {
        //string Key = "lotto.ichari.com";
        HttpCookie cookieLoginUserID = DataEncryption.MakeCookie("", ID);
        //HttpCookie cookieLoginUserID = new HttpCookie(Key);
        cookieLoginUserID.Value = "";
        cookieLoginUserID.Expires = DateTime.Now.AddYears(-20);
        cookieLoginUserID.Path = "/";
        //cookieLoginUserID.Domain = ".ichari.com";

        try
        {
            HttpContext.Current.Response.Cookies.Add(cookieLoginUserID);
        }
        catch { }
    }

    public static Users GetCurrentUser(long siteid)
    {
        // 从 Cookie 中取出 UserID
        HttpCookie cookieUser = DataEncryption.GetCookie();
        if ((cookieUser == null) || (String.IsNullOrEmpty(cookieUser.Values[ConfigurationManager.AppSettings["LotteryCookieName"]])))
        {
            return null;
        }

        string CookieUserID = cookieUser.Values[ConfigurationManager.AppSettings["LotteryCookieName"]];
        try
        {
            //CookieUserID = Shove._Security.Encrypt.UnEncryptString(PF.GetCallCert(), Shove._Security.Encrypt.Decrypt3DES(PF.GetCallCert(), CookieUserID, PF.DesKey));
            CookieUserID = DataEncryption.DecryptAES(CookieUserID);
        }
        catch
        {
            CookieUserID = "";
        }

        long UserID = -1;
        Users t_user = null;

        if (!String.IsNullOrEmpty(CookieUserID))
        {
            UserID = Shove._Convert.StrToLong(CookieUserID, -1);

            if (UserID > 0)
            {
                t_user = new Users(siteid)[siteid, UserID];

                if (t_user != null)
                {
                    return t_user;
                }
            }
        }
        return null;
    }

    #endregion

    #region 获取用户信息

    public int GetUserInformationByID(ref string ReturnDescription)
    {
        if (ID < 0)
        {
            throw new Exception("Users 尚未初始化到具体的数据实例上，请先使用 GetUserInformation 等获取数据信息");
        }

        int ReturnValue = -1;
        ReturnDescription = "";

        Users tu = new Users(SiteID);
        Clone(tu);

        int Result = DAL.Procedures.P_GetUserInformationByID(ID, SiteID, ref Name, ref NickName, ref RealityName, ref _password, ref _passwordadv, ref CityID,
            ref Sex, ref BirthDay, ref IDCardNumber, ref Address, ref Email, ref isEmailValided, ref QQ,ref isQQValided, ref Telephone, ref Mobile, ref isMobileValided, ref isPrivacy, ref isCanLogin,
            ref RegisterTime, ref LastLoginTime, ref LastLoginIP, ref LoginCount, ref UserType, ref BankType, ref BankName, ref BankCardNumber, ref Balance,
            ref Freeze, ref ScoringOfSelfBuy, ref ScoringOfCommendBuy, ref Scoring, ref Level, ref CommenderID, ref CpsID, ref OwnerSites, ref AlipayID, ref Bonus, ref Reward,
            ref AlipayName, ref isAlipayNameValided, ref ComeFrom, ref IsCrossLogin, ref Memo, ref BonusThisMonth, ref BonusAllow, ref BonusUse,
            ref PromotionMemberBonusScale, ref PromotionSiteBonusScale, ref VisitSource, ref HeadUrl, ref SecurityQuestion, ref SecurityAnswer, ref FriendList, ref ReturnValue, ref ReturnDescription);

        if (Result < 0)
        {
            tu.Clone(this);

            ReturnDescription = "数据库读写错误";

            return -1;
        }

        if (ReturnValue < 0)
        {
            tu.Clone(this);

            return ReturnValue;
        }

        cps.OwnerUserID = ID;

        ReturnDescription = "";
        cps.GetCpsInformationByOwnerUserID(ref ReturnDescription);

        if ((cps.SiteID > 0) && (cps.ID < 0))
        {
            ReturnDescription = "";
        }

        return 0;
    }

    public int GetUserInformationByName(ref string ReturnDescription)
    {
        int ReturnValue = -1;
        ReturnDescription = "";

        Users tu = new Users(SiteID);
        Clone(tu);

        int Result = DAL.Procedures.P_GetUserInformationByName(Name, SiteID, ref _id, ref NickName, ref RealityName, ref _password, ref _passwordadv, ref CityID,
            ref Sex, ref BirthDay, ref IDCardNumber, ref Address, ref Email, ref isEmailValided, ref QQ,ref isQQValided, ref Telephone, ref Mobile, ref isMobileValided, ref isPrivacy, ref isCanLogin,
            ref RegisterTime, ref LastLoginTime, ref LastLoginIP, ref LoginCount, ref UserType, ref BankType, ref BankName, ref BankCardNumber, ref Balance,
            ref Freeze, ref ScoringOfSelfBuy, ref ScoringOfCommendBuy, ref Scoring, ref Level, ref CommenderID, ref CpsID, ref OwnerSites, ref AlipayID, ref Bonus, ref Reward,
            ref AlipayName, ref isAlipayNameValided, ref ComeFrom, ref isCanLogin, ref Memo, ref BonusThisMonth, ref BonusAllow, ref BonusUse, ref PromotionMemberBonusScale, ref PromotionSiteBonusScale,
            ref VisitSource, ref HeadUrl, ref SecurityQuestion, ref SecurityAnswer, ref FriendList, ref ReturnValue, ref ReturnDescription);

        if (Result < 0)
        {
            tu.Clone(this);

            ReturnDescription = "数据库读写错误";

            return -1;
        }

        if (ReturnValue < 0)
        {
            tu.Clone(this);

            return ReturnValue;
        }

        cps.OwnerUserID = ID;

        ReturnDescription = "";
        cps.GetCpsInformationByOwnerUserID(ref ReturnDescription);

        if ((cps.SiteID > 0) && (cps.ID < 0))
        {
            ReturnDescription = "";
        }

        return 0;
    }

    #endregion

    #region 充值

    // 在线支付后，增加电子货币
    public int AddUserBalance(double Money, double FormalitiesFees, string PayNumber, string PayBank, string Memo, ref string ReturnDescription)
    {
        if (ID < 0)
        {
            throw new Exception("Users 尚未初始化到具体的数据实例上，请先使用 GetUserInformation 等获取数据信息");
        }

        int ReturnValue = -1;
        ReturnDescription = "";

        int Result = DAL.Procedures.P_UserAddMoney(SiteID, ID, Money, FormalitiesFees, PayNumber, PayBank, Memo, ref ReturnValue, ref ReturnDescription);

        if (Result < 0)
        {
            ReturnDescription = "数据库读写错误";

            return -1;
        }

        if (ReturnValue < 0)
        {
            return ReturnValue;
        }

        string ReturnDescription_2 = "";
        GetUserInformationByID(ref ReturnDescription_2);

        //统计支付流量
        //PF.WriteOnlinePays(Site, PayBank, PayNumber, Money, FormalitiesFees, ID);

        return 0;
    }

    // 手动增加电子货币(后台直接为用户充值)
    public int AddUserBalanceManual(double Money, string Memo, long OperatorID, ref string ReturnDescription)
    {
        if (ID < 0)
        {
            throw new Exception("Users 尚未初始化到具体的数据实例上，请先使用 GetUserInformation 等获取数据信息");
        }

        int ReturnValue = -1;
        ReturnDescription = "";

        int Result = DAL.Procedures.P_UserAddMoneyManual(SiteID, ID, Money, Memo, OperatorID, ref ReturnValue, ref ReturnDescription);
        if (Result < 0)
        {
            ReturnDescription = "数据库读写错误";

            return -1;
        }

        if (ReturnValue < 0)
        {
            return ReturnValue;
        }

        string ReturnDescription_2 = "";
        GetUserInformationByID(ref ReturnDescription_2);

        return 0;
    }

    #endregion

    #region 购彩、撤单、追号、追号套餐

    public long InitiateScheme(long IsuseID, int PlayTypeID, string Title, string Description, string LotteryNumber, string UpdateloadFileContent, int Multiple,
        double Money, double AssureMoney, int Share, int BuyShare, string OpenUsers, short SecrecyLevel, double SchemeBonusScale, ref string ReturnDescription)
    {
        if (ID < 0)
        {
            throw new Exception("Users 尚未初始化到具体的数据实例上，请先使用 GetUserInformation 等获取数据信息");
        }

        ReturnDescription = "";

        if ((SecrecyLevel < 0) || (SecrecyLevel > 3))
        {
            SecrecyLevel = 0;
        }

        long SchemeID = -1;

        int Result = DAL.Procedures.P_InitiateScheme(SiteID, ID, IsuseID, PlayTypeID, Title, Description, LotteryNumber, UpdateloadFileContent,
            Multiple, Money, AssureMoney, Share, BuyShare, OpenUsers.Replace('，', ','), SecrecyLevel, SchemeBonusScale, ref SchemeID, ref ReturnDescription);

        if (Result < 0)
        {
            ReturnDescription = "数据库读写错误";

            return -1;
        }

        if (SchemeID < 0)
        {
            return SchemeID;
        }

        string ReturnDescription_2 = "";
        GetUserInformationByID(ref ReturnDescription_2);

        //Send Email
        if ((Email != "") && (DAL.Functions.F_GetIsSendNotification(SiteID, NotificationManners.Email, NotificationTypes.InitiateScheme, ID)))
        {
            string EmailSubject = "", EmailBody = "";

            Site.SiteNotificationTemplates.SplitEmailTemplate(Site.SiteNotificationTemplates[NotificationManners.Email, NotificationTypes.InitiateScheme], ref EmailSubject, ref EmailBody);

            if ((EmailSubject != "") && (EmailBody != ""))
            {
                EmailSubject = EmailSubject.Replace("[UserName]", Name);

                EmailBody = EmailBody.Replace("[UserName]", Name);
                EmailBody = EmailBody.Replace("[SchemeID]", SchemeID.ToString());
                 if (PlayTypeID >= 7201 && PlayTypeID < 7400)
                {
                    LotteryNumber = PF.GetScriptResTable(LotteryNumber);
                }
                else
                {
                    LotteryNumber = LotteryNumber.Replace("\n", "<BR />");
                }
                EmailBody = EmailBody.Replace("[Multiple]", Multiple.ToString());
                EmailBody = EmailBody.Replace("[Money]", Money.ToString("N"));
                EmailBody = EmailBody.Replace("[AssureMoney]", AssureMoney.ToString("N"));
                EmailBody = EmailBody.Replace("[Share]", Share.ToString());
                EmailBody = EmailBody.Replace("[BuyShare]", BuyShare.ToString());
                EmailBody = EmailBody.Replace("[OpenUserList]", OpenUsers);

                PF.SendEmail(Site, Email, EmailSubject, EmailBody);
            }
        }

        //Send SMS
        if ((Mobile != "") && isMobileValided && (DAL.Functions.F_GetIsSendNotification(SiteID, NotificationManners.SMS, NotificationTypes.InitiateScheme, ID)))
        {
            string Body = Site.SiteNotificationTemplates[NotificationManners.SMS, NotificationTypes.InitiateScheme];

            if (Body != "")
            {
                Body = Body.Replace("[UserName]", Name);
                Body = Body.Replace("[SchemeID]", SchemeID.ToString());

                PF.SendSMS(Site, ID, Mobile, Body);
            }
        }

        //Send StationSMS
        if (DAL.Functions.F_GetIsSendNotification(SiteID, NotificationManners.StationSMS, NotificationTypes.InitiateScheme, ID))
        {
            string Body = Site.SiteNotificationTemplates[NotificationManners.StationSMS, NotificationTypes.InitiateScheme];

            if (Body != "")
            {
                Body = Body.Replace("[UserName]", Name);
                Body = Body.Replace("[SchemeID]", SchemeID.ToString());

                PF.SendStationSMS(Site, Site.AdministratorID, ID, StationSMSTypes.SystemMessage, Body);
            }
        }

        return SchemeID;
    }

    public int JoinScheme(long SchemeID, int Share, ref string ReturnDescription)
    {
        if (ID < 0)
        {
            throw new Exception("Users 尚未初始化到具体的数据实例上，请先使用 GetUserInformation 等获取数据信息");
        }

        int ReturnValue = -1;
        ReturnDescription = "";

        int Result = DAL.Procedures.P_JoinScheme(Site.ID, ID, SchemeID, Share, false, ref ReturnValue, ref ReturnDescription);

        if (Result < 0)
        {
            ReturnDescription = "数据库读写错误";

            return -1;
        }

        if (ReturnValue < 0)
        {
            return ReturnValue;
        }

        string ReturnDescription_2 = "";
        GetUserInformationByID(ref ReturnDescription_2);

        //Send Email
        if ((Email != "") && (DAL.Functions.F_GetIsSendNotification(SiteID, NotificationManners.Email, NotificationTypes.JoinScheme, ID)))
        {
            string EmailSubject = "", EmailBody = "";

            Site.SiteNotificationTemplates.SplitEmailTemplate(Site.SiteNotificationTemplates[NotificationManners.Email, NotificationTypes.JoinScheme], ref EmailSubject, ref EmailBody);

            if ((EmailSubject != "") && (EmailBody != ""))
            {
                DataTable dt = new DAL.Tables.T_Schemes().Open("", "[id] = " + SchemeID.ToString(), "");

                string LotteryNumber = "";
                int Multiple = 0;
                double Money = 0;
                int SumShare = 0;

                if ((dt != null) && (dt.Rows.Count > 0))
                {
                    LotteryNumber = dt.Rows[0]["LotteryNumber"].ToString();
                    Multiple = Shove._Convert.StrToInt(dt.Rows[0]["Multiple"].ToString(), 0);
                    Money = Shove._Convert.StrToDouble(dt.Rows[0]["Money"].ToString(), 0);
                    SumShare = Shove._Convert.StrToInt(dt.Rows[0]["Share"].ToString(), 0);
                }

                if ((LotteryNumber != "") && (Multiple > 0) && (Money > 0) && (SumShare > 0))
                {
                    EmailSubject = EmailSubject.Replace("[UserName]", Name);

                    EmailBody = EmailBody.Replace("[UserName]", Name);
                    EmailBody = EmailBody.Replace("[SchemeID]", SchemeID.ToString());
                    EmailBody = EmailBody.Replace("[LotteryNumber]", LotteryNumber.Replace("\n", "<BR />"));
                    EmailBody = EmailBody.Replace("[Multiple]", Multiple.ToString());
                    EmailBody = EmailBody.Replace("[Money]", Money.ToString("N"));
                    EmailBody = EmailBody.Replace("[Share]", SumShare.ToString());
                    EmailBody = EmailBody.Replace("[BuyShare]", Share.ToString());

                    PF.SendEmail(Site, Email, EmailSubject, EmailBody);
                }
            }
        }

        //Send SMS
        if ((Mobile != "") && isMobileValided && (DAL.Functions.F_GetIsSendNotification(SiteID, NotificationManners.SMS, NotificationTypes.JoinScheme, ID)))
        {
            string Body = Site.SiteNotificationTemplates[NotificationManners.SMS, NotificationTypes.JoinScheme];

            if (Body != "")
            {
                Body = Body.Replace("[UserName]", Name);
                Body = Body.Replace("[SchemeID]", SchemeID.ToString());

                PF.SendSMS(Site, ID, Mobile, Body);
            }
        }

        //Send StationSMS
        if (DAL.Functions.F_GetIsSendNotification(SiteID, NotificationManners.StationSMS, NotificationTypes.JoinScheme, ID))
        {
            string Body = Site.SiteNotificationTemplates[NotificationManners.StationSMS, NotificationTypes.JoinScheme];

            if (Body != "")
            {
                Body = Body.Replace("[UserName]", Name);
                Body = Body.Replace("[SchemeID]", SchemeID.ToString());

                PF.SendStationSMS(Site, Site.AdministratorID, ID, StationSMSTypes.SystemMessage, Body);
            }
        }

        return 0;
    }

    public int QuashScheme(long SchemeID, bool isSystemQuashed, ref string ReturnDescription)
    {
        if (ID < 0)
        {
            throw new Exception("Users 尚未初始化到具体的数据实例上，请先使用 GetUserInformation 等获取数据信息");
        }

        int ReturnValue = -1;
        ReturnDescription = "";

        int Result = DAL.Procedures.P_QuashScheme(Site.ID, SchemeID, isSystemQuashed, false, ref ReturnValue, ref ReturnDescription);

        if (Result < 0)
        {
            ReturnDescription = "数据库读写错误";

            return -1;
        }

        if (ReturnValue < 0)
        {
            return ReturnValue;
        }

        string ReturnDescription_2 = "";
        GetUserInformationByID(ref ReturnDescription_2);

        //Send Email
        if ((Email != "") && isEmailValided && (DAL.Functions.F_GetIsSendNotification(SiteID, NotificationManners.Email, NotificationTypes.QuashScheme, ID)))
        {
            string EmailSubject = "", EmailBody = "";

            Site.SiteNotificationTemplates.SplitEmailTemplate(Site.SiteNotificationTemplates[NotificationManners.Email, NotificationTypes.QuashScheme], ref EmailSubject, ref EmailBody);

            if ((EmailSubject != "") && (EmailBody != ""))
            {
                EmailSubject = EmailSubject.Replace("[UserName]", Name);

                EmailBody = EmailBody.Replace("[UserName]", Name);
                EmailBody = EmailBody.Replace("[Scheme_id]", SchemeID.ToString());

                PF.SendEmail(Site, Email, EmailSubject, EmailBody);
            }
        }

        //Send SMS
        if ((Mobile != "") && isMobileValided && isSystemQuashed && (DAL.Functions.F_GetIsSendNotification(SiteID, NotificationManners.SMS, NotificationTypes.QuashScheme, ID)))
        {
            string Body = Site.SiteNotificationTemplates[NotificationManners.SMS, NotificationTypes.QuashScheme];

            if (Body != "")
            {
                Body = Body.Replace("[UserName]", Name);
                Body = Body.Replace("[Scheme_id]", SchemeID.ToString());

                PF.SendSMS(Site, ID, Mobile, Body);
            }
        }

        //Send StationSMS
        if (DAL.Functions.F_GetIsSendNotification(SiteID, NotificationManners.StationSMS, NotificationTypes.QuashScheme, ID))
        {
            string Body = Site.SiteNotificationTemplates[NotificationManners.StationSMS, NotificationTypes.QuashScheme];

            if (Body != "")
            {
                Body = Body.Replace("[UserName]", Name);
                Body = Body.Replace("[SchemeID]", SchemeID.ToString());

                PF.SendStationSMS(Site, Site.AdministratorID, ID, StationSMSTypes.SystemMessage, Body);
            }
        }

        return 0;
    }

    public int Quash(long BuyDetailID, ref string ReturnDescription)
    {
        if (ID < 0)
        {
            throw new Exception("Users 尚未初始化到具体的数据实例上，请先使用 GetUserInformation 等获取数据信息");
        }

        int ReturnValue = -1;
        ReturnDescription = "";

        int Result = DAL.Procedures.P_Quash(SiteID, BuyDetailID, ref ReturnValue, ref ReturnDescription);

        if (Result < 0)
        {
            ReturnDescription = "数据库读写错误";

            return -1;
        }

        if (ReturnValue < 0)
        {
            return ReturnValue;
        }

        string ReturnDescription_2 = "";
        GetUserInformationByID(ref ReturnDescription_2);

        //Send Email
        if ((Email != "") && (DAL.Functions.F_GetIsSendNotification(SiteID, NotificationManners.Email, NotificationTypes.Quash, ID)))
        {
            string EmailSubject = "", EmailBody = "";

            Site.SiteNotificationTemplates.SplitEmailTemplate(Site.SiteNotificationTemplates[NotificationManners.Email, NotificationTypes.Quash], ref EmailSubject, ref EmailBody);

            if ((EmailSubject != "") && (EmailBody != ""))
            {
                EmailSubject = EmailSubject.Replace("[UserName]", Name);

                EmailBody = EmailBody.Replace("[UserName]", Name);
                EmailBody = EmailBody.Replace("[LotteryBuyDetail_id]", BuyDetailID.ToString());

                PF.SendEmail(Site, Email, EmailSubject, EmailBody);
            }
        }

        //Send SMS
        if ((Mobile != "") && isMobileValided && (DAL.Functions.F_GetIsSendNotification(SiteID, NotificationManners.SMS, NotificationTypes.Quash, ID)))
        {
            string Body = Site.SiteNotificationTemplates[NotificationManners.SMS, NotificationTypes.Quash];

            if (Body != "")
            {
                Body = Body.Replace("[UserName]", Name);
                Body = Body.Replace("[LotteryBuyDetail_id]", BuyDetailID.ToString());

                PF.SendSMS(Site, ID, Mobile, Body);
            }
        }

        //Send StationSMS
        if (DAL.Functions.F_GetIsSendNotification(SiteID, NotificationManners.StationSMS, NotificationTypes.Quash, ID))
        {
            string Body = Site.SiteNotificationTemplates[NotificationManners.StationSMS, NotificationTypes.Quash];

            if (Body != "")
            {
                Body = Body.Replace("[UserName]", Name);
                Body = Body.Replace("[LotteryBuyDetail_id]", BuyDetailID.ToString());

                PF.SendStationSMS(Site, Site.AdministratorID, ID, StationSMSTypes.SystemMessage, Body);
            }
        }

        return 0;
    }

    public int InitiateChaseTask(string Title, string Description, int LotteryID, double StopWhenWinMoney, string DetailXML, string LotteryNumber,double SchemeBonusScalec, ref string ReturnDescription)
    {
        if (ID < 0)
        {
            throw new Exception("Users 尚未初始化到具体的数据实例上，请先使用 GetUserInformation 等获取数据信息");
        }

        ReturnDescription = "";

        long ChaseTaskID = -1;

        int Result = DAL.Procedures.P_InitiateChaseTask(Site.ID, ID, Title, Description, LotteryID, StopWhenWinMoney, DetailXML, LotteryNumber,SchemeBonusScalec, ref ChaseTaskID, ref ReturnDescription);

        if (Result < 0)
        {
            ReturnDescription = "数据库读写错误";

            return -1;
        }

        if (ChaseTaskID < 0)
        {
            return (int)ChaseTaskID;
        }

        string ReturnDescription_2 = "";
        GetUserInformationByID(ref ReturnDescription_2);

        //Send Email
        if ((Email != "") && (DAL.Functions.F_GetIsSendNotification(SiteID, NotificationManners.Email, NotificationTypes.InitiateChaseTask, ID)))
        {
            string EmailSubject = "", EmailBody = "";

            Site.SiteNotificationTemplates.SplitEmailTemplate(Site.SiteNotificationTemplates[NotificationManners.Email, NotificationTypes.InitiateChaseTask], ref EmailSubject, ref EmailBody);

            if ((EmailSubject != "") && (EmailBody != ""))
            {
                EmailSubject = EmailSubject.Replace("[UserName]", Name);

                EmailBody = EmailBody.Replace("[UserName]", Name);
                EmailBody = EmailBody.Replace("[Chase_id]", ChaseTaskID.ToString());

                PF.SendEmail(Site, Email, EmailSubject, EmailBody);
            }
        }

        //Send SMS
        if ((Mobile != "") && isMobileValided && (DAL.Functions.F_GetIsSendNotification(SiteID, NotificationManners.SMS, NotificationTypes.InitiateChaseTask, ID)))
        {
            string Body = Site.SiteNotificationTemplates[NotificationManners.SMS, NotificationTypes.InitiateChaseTask];

            if (Body != "")
            {
                Body = Body.Replace("[UserName]", Name);
                Body = Body.Replace("[Chase_id]", ChaseTaskID.ToString());

                PF.SendSMS(Site, ID, Mobile, Body);
            }
        }

        //Send StationSMS
        if (DAL.Functions.F_GetIsSendNotification(SiteID, NotificationManners.StationSMS, NotificationTypes.InitiateChaseTask, ID))
        {
            string Body = Site.SiteNotificationTemplates[NotificationManners.StationSMS, NotificationTypes.InitiateChaseTask];

            if (Body != "")
            {
                Body = Body.Replace("[UserName]", Name);
                Body = Body.Replace("[Chase_id]", ChaseTaskID.ToString());

                PF.SendStationSMS(Site, Site.AdministratorID, ID, StationSMSTypes.SystemMessage, Body);
            }
        }

        return (int)ChaseTaskID;
    }

    public int ExecChaseTaskDetail(long ChaseTaskDetailID, ref string ReturnDescription)
    {
        if (ID < 0)
        {
            throw new Exception("Users 尚未初始化到具体的数据实例上，请先使用 GetUserInformation 等获取数据信息");
        }

        int ReturnValue = -1;
        ReturnDescription = "";

        int Result = DAL.Procedures.P_ExecChaseTaskDetail(Site.ID, ChaseTaskDetailID, ref ReturnValue, ref ReturnDescription);

        if (Result < 0)
        {
            ReturnDescription = "数据库读写错误";

            return -1;
        }

        if (ReturnValue < 0)
        {
            return ReturnValue;
        }

        string ReturnDescription_2 = "";
        GetUserInformationByID(ref ReturnDescription_2);

        //Send Email
        if ((Email != "") && (DAL.Functions.F_GetIsSendNotification(SiteID, NotificationManners.Email, NotificationTypes.ExecChaseTaskDetail, ID)))
        {
            string EmailSubject = "", EmailBody = "";

            Site.SiteNotificationTemplates.SplitEmailTemplate(Site.SiteNotificationTemplates[NotificationManners.Email, NotificationTypes.ExecChaseTaskDetail], ref EmailSubject, ref EmailBody);

            if ((EmailSubject != "") && (EmailBody != ""))
            {
                EmailSubject = EmailSubject.Replace("[UserName]", Name);

                EmailBody = EmailBody.Replace("[UserName]", Name);
                EmailBody = EmailBody.Replace("[Chase_id]", ChaseTaskDetailID.ToString());

                PF.SendEmail(Site, Email, EmailSubject, EmailBody);
            }
        }

        //Send SMS
        if ((Mobile != "") && isMobileValided && (DAL.Functions.F_GetIsSendNotification(SiteID, NotificationManners.SMS, NotificationTypes.ExecChaseTaskDetail, ID)))
        {
            string Body = Site.SiteNotificationTemplates[NotificationManners.SMS, NotificationTypes.ExecChaseTaskDetail];

            if (Body != "")
            {
                Body = Body.Replace("[UserName]", Name);
                Body = Body.Replace("[Chase_id]", ChaseTaskDetailID.ToString());

                PF.SendSMS(Site, ID, Mobile, Body);
            }
        }

        //Send StationSMS
        if (DAL.Functions.F_GetIsSendNotification(SiteID, NotificationManners.StationSMS, NotificationTypes.ExecChaseTaskDetail, ID))
        {
            string Body = Site.SiteNotificationTemplates[NotificationManners.StationSMS, NotificationTypes.ExecChaseTaskDetail];

            if (Body != "")
            {
                Body = Body.Replace("[UserName]", Name);
                Body = Body.Replace("[Chase_id]", ChaseTaskDetailID.ToString());

                PF.SendStationSMS(Site, Site.AdministratorID, ID, StationSMSTypes.SystemMessage, Body);
            }
        }

        return 0;
    }

    public int QuashChaseTask(long ChaseTaskID, bool isSystemQuashed, ref string ReturnDescription)
    {
        if (ID < 0)
        {
            throw new Exception("Users 尚未初始化到具体的数据实例上，请先使用 GetUserInformation 等获取数据信息");
        }

        int ReturnValue = -1;
        ReturnDescription = "";

        int Result = DAL.Procedures.P_QuashChaseTask(Site.ID, ChaseTaskID, isSystemQuashed, ref ReturnValue, ref ReturnDescription);

        if (Result < 0)
        {
            ReturnDescription = "数据库读写错误";

            return -1;
        }

        if (ReturnValue < 0)
        {
            return ReturnValue;
        }

        string ReturnDescription_2 = "";
        GetUserInformationByID(ref ReturnDescription_2);

        //Send Email
        if ((Email != "") && (DAL.Functions.F_GetIsSendNotification(SiteID, NotificationManners.Email, NotificationTypes.QuashChaseTask, ID)))
        {
            string EmailSubject = "", EmailBody = "";

            Site.SiteNotificationTemplates.SplitEmailTemplate(Site.SiteNotificationTemplates[NotificationManners.Email, NotificationTypes.QuashChaseTask], ref EmailSubject, ref EmailBody);

            if ((EmailSubject != "") && (EmailBody != ""))
            {
                EmailSubject = EmailSubject.Replace("[UserName]", Name);

                EmailBody = EmailBody.Replace("[UserName]", Name);
                EmailBody = EmailBody.Replace("[Chase_id]", ChaseTaskID.ToString());

                PF.SendEmail(Site, Email, EmailSubject, EmailBody);
            }
        }

        //Send SMS
        if ((Mobile != "") && isMobileValided && (DAL.Functions.F_GetIsSendNotification(SiteID, NotificationManners.SMS, NotificationTypes.QuashChaseTask, ID)))
        {
            string Body = Site.SiteNotificationTemplates[NotificationManners.SMS, NotificationTypes.QuashChaseTask];

            if (Body != "")
            {
                Body = Body.Replace("[UserName]", Name);
                Body = Body.Replace("[Chase_id]", ChaseTaskID.ToString());

                PF.SendSMS(Site, ID, Mobile, Body);
            }
        }

        //Send StationSMS
        if (DAL.Functions.F_GetIsSendNotification(SiteID, NotificationManners.StationSMS, NotificationTypes.QuashChaseTask, ID))
        {
            string Body = Site.SiteNotificationTemplates[NotificationManners.StationSMS, NotificationTypes.QuashChaseTask];

            if (Body != "")
            {
                Body = Body.Replace("[UserName]", Name);
                Body = Body.Replace("[Chase_id]", ChaseTaskID.ToString());

                PF.SendStationSMS(Site, Site.AdministratorID, ID, StationSMSTypes.SystemMessage, Body);
            }
        }

        return 0;
    }

    public int QuashChaseTaskDetail(long ChaseTaskDetailID, bool isSystemQuashed, ref string ReturnDescription)
    {
        if (ID < 0)
        {
            throw new Exception("Users 尚未初始化到具体的数据实例上，请先使用 GetUserInformation 等获取数据信息");
        }

        int ReturnValue = -1;
        ReturnDescription = "";

        int Result = DAL.Procedures.P_QuashChaseTaskDetail(Site.ID, ChaseTaskDetailID, isSystemQuashed, ref ReturnValue, ref ReturnDescription);

        if (Result < 0)
        {
            ReturnDescription = "数据库读写错误";

            return -1;
        }

        if (ReturnValue < 0)
        {
            return ReturnValue;
        }

        string ReturnDescription_2 = "";
        GetUserInformationByID(ref ReturnDescription_2);

        //Send Email
        if ((Email != "") && (DAL.Functions.F_GetIsSendNotification(SiteID, NotificationManners.Email, NotificationTypes.QuashChaseTaskDetail, ID)))
        {
            string EmailSubject = "", EmailBody = "";

            Site.SiteNotificationTemplates.SplitEmailTemplate(Site.SiteNotificationTemplates[NotificationManners.Email, NotificationTypes.QuashChaseTaskDetail], ref EmailSubject, ref EmailBody);

            if ((EmailSubject != "") && (EmailBody != ""))
            {
                EmailSubject = EmailSubject.Replace("[UserName]", Name);

                EmailBody = EmailBody.Replace("[UserName]", Name);

                PF.SendEmail(Site, Email, EmailSubject, EmailBody);
            }
        }

        //Send SMS
        if ((Mobile != "") && isMobileValided && (DAL.Functions.F_GetIsSendNotification(SiteID, NotificationManners.SMS, NotificationTypes.QuashChaseTaskDetail, ID)))
        {
            string Body = Site.SiteNotificationTemplates[NotificationManners.SMS, NotificationTypes.QuashChaseTaskDetail];

            if (Body != "")
            {
                Body = Body.Replace("[UserName]", Name);

                PF.SendSMS(Site, ID, Mobile, Body);
            }
        }

        //Send StationSMS
        if (DAL.Functions.F_GetIsSendNotification(SiteID, NotificationManners.StationSMS, NotificationTypes.QuashChaseTaskDetail, ID))
        {
            string Body = Site.SiteNotificationTemplates[NotificationManners.StationSMS, NotificationTypes.QuashChaseTaskDetail];

            if (Body != "")
            {
                Body = Body.Replace("[UserName]", Name);

                PF.SendStationSMS(Site, Site.AdministratorID, ID, StationSMSTypes.SystemMessage, Body);
            }
        }

        return 0;
    }

    public int InitiateCustomChase(int LotteryID,int PlayTypeID,int Price, short Type,DateTime EndTime,int IsuseCount,int Multiple,int Nums,short BetType, string LotteryNumber,short StopType,double stopMoney,double Money,string Title,string ChaseXML, ref string ReturnDescription)
    {
        if (ID < 0)
        {
            throw new Exception("Users 尚未初始化到具体的数据实例上，请先使用 GetUserInformation 等获取数据信息");
        }

        ReturnDescription = "";

        int CustomChaseID = -1;

        int Result = DAL.Procedures.P_ChasesAdd(ID, LotteryID,PlayTypeID,Price, Type, DateTime.Now, EndTime, IsuseCount, Multiple, Nums, BetType, LotteryNumber, StopType, stopMoney, Money, Title, ChaseXML, ref CustomChaseID, ref ReturnDescription);

        if (Result < 0)
        {
            ReturnDescription = "数据库读写错误";

            return -1;
        }

        if (CustomChaseID < 0)
        {
            return (int)CustomChaseID;
        }

        string ReturnDescription_2 = "";
        GetUserInformationByID(ref ReturnDescription_2);

        //Send Email
        if ((Email != "") && isEmailValided && (DAL.Functions.F_GetIsSendNotification(SiteID, NotificationManners.Email, NotificationTypes.IntiateCustomChase, ID)))
        {
            string EmailSubject = "", EmailBody = "";

            Site.SiteNotificationTemplates.SplitEmailTemplate(Site.SiteNotificationTemplates[NotificationManners.Email, NotificationTypes.IntiateCustomChase], ref EmailSubject, ref EmailBody);

            if ((EmailSubject != "") && (EmailBody != ""))
            {
                EmailSubject = EmailSubject.Replace("[UserName]", Name);

                EmailBody = EmailBody.Replace("[UserName]", Name);
                EmailBody = EmailBody.Replace("[Chase_id]", CustomChaseID.ToString());

                PF.SendEmail(Site, Email, EmailSubject, EmailBody);
            }
        }

        //Send SMS
        if ((Mobile != "") && isMobileValided && (DAL.Functions.F_GetIsSendNotification(SiteID, NotificationManners.SMS, NotificationTypes.IntiateCustomChase, ID)))
        {
            string Body = Site.SiteNotificationTemplates[NotificationManners.SMS, NotificationTypes.IntiateCustomChase];

            if (Body != "")
            {
                Body = Body.Replace("[UserName]", Name);
                Body = Body.Replace("[Chase_id]", CustomChaseID.ToString());

                PF.SendSMS(Site, ID, Mobile, Body);
            }
        }

        //Send StationSMS
        if (DAL.Functions.F_GetIsSendNotification(SiteID, NotificationManners.StationSMS, NotificationTypes.IntiateCustomChase, ID))
        {
            string Body = Site.SiteNotificationTemplates[NotificationManners.StationSMS, NotificationTypes.IntiateCustomChase];

            if (Body != "")
            {
                Body = Body.Replace("[UserName]", Name);
                Body = Body.Replace("[ChaseID]", CustomChaseID.ToString());

                PF.SendStationSMS(Site, Site.AdministratorID, ID, StationSMSTypes.SystemMessage, Body);
            }
        }

        return (int)CustomChaseID;
    }

    #endregion

    #region 中奖

    public int Win(long SchemeID, double Money, ref string ReturnDescription)  // 中奖后，发送通知。注意：不写中奖数据，由后台统一计算、写入。
    {
        if (ID < 0)
        {
            throw new Exception("Users 尚未初始化到具体的数据实例上，请先使用 GetUserInformation 等获取数据信息");
        }

        ReturnDescription = "";

        //Send Email
        if ((Email != "") && (DAL.Functions.F_GetIsSendNotification(SiteID, NotificationManners.Email, NotificationTypes.Win, ID)))
        {
            string EmailSubject = "", EmailBody = "";

            Site.SiteNotificationTemplates.SplitEmailTemplate(Site.SiteNotificationTemplates[NotificationManners.Email, NotificationTypes.Win], ref EmailSubject, ref EmailBody);

            if ((EmailSubject != "") && (EmailBody != ""))
            {
                EmailSubject = EmailSubject.Replace("[UserName]", Name);

                EmailBody = EmailBody.Replace("[UserName]", Name);
                EmailBody = EmailBody.Replace("[SchemeID]", SchemeID.ToString());
                EmailBody = EmailBody.Replace("[Money]", Money.ToString("N"));

                PF.SendEmail(Site, Email, EmailSubject, EmailBody);
            }
        }

        //Send SMS
        if ((Mobile != "") && isMobileValided && (DAL.Functions.F_GetIsSendNotification(SiteID, NotificationManners.SMS, NotificationTypes.Win, ID)))
        {
            string Body = Site.SiteNotificationTemplates[NotificationManners.SMS, NotificationTypes.Win];

            if (Body != "")
            {
                Body = Body.Replace("[UserName]", Name);
                Body = Body.Replace("[SchemeID]", SchemeID.ToString());
                Body = Body.Replace("[Money]", Money.ToString("N"));

                PF.SendSMS(Site, ID, Mobile, Body);
            }
        }

        //Send StationSMS
        if (DAL.Functions.F_GetIsSendNotification(SiteID, NotificationManners.StationSMS, NotificationTypes.Win, ID))
        {
            string Body = Site.SiteNotificationTemplates[NotificationManners.StationSMS, NotificationTypes.Win];

            if (Body != "")
            {
                Body = Body.Replace("[UserName]", Name);
                Body = Body.Replace("[SchemeID]", SchemeID.ToString());
                Body = Body.Replace("[Money]", Money.ToString("N"));

                PF.SendStationSMS(Site, Site.AdministratorID, ID, StationSMSTypes.SystemMessage, Body);
            }
        }

        return 0;
    }

    #endregion

    #region 提款

    public int Distill(int DistillType, double Money, double FormalitiesFees, string BankUserName, string _BankName, string _BankCardNumber, string _AlipayID, string _AlipayName, string Memo, bool IsCps, ref string ReturnDescription)
    {
        if (ID < 0)
        {
            throw new Exception("Users 尚未初始化到具体的数据实例上，请先使用 GetUserInformation 等获取数据信息");
        }

        int ReturnValue = -1;
        ReturnDescription = "";

        int Result = DAL.Procedures.P_Distill(Site.ID, ID, DistillType, Money, FormalitiesFees, BankUserName, _BankName, _BankCardNumber, _AlipayID, _AlipayName, Memo, IsCps, ref ReturnValue, ref ReturnDescription);

        if (Result < 0)
        {
            ReturnDescription = "数据库读写错误";

            return -1;
        }

        if (ReturnValue < 0)
        {
            return ReturnValue;
        }

        string ReturnDescription_2 = "";
        GetUserInformationByID(ref ReturnDescription_2);

        //Send Email
        if ((Email != "") && (DAL.Functions.F_GetIsSendNotification(SiteID, NotificationManners.Email, NotificationTypes.TryDistill, ID)))
        {
            string EmailSubject = "", EmailBody = "";

            Site.SiteNotificationTemplates.SplitEmailTemplate(Site.SiteNotificationTemplates[NotificationManners.Email, NotificationTypes.TryDistill], ref EmailSubject, ref EmailBody);

            if ((EmailSubject != "") && (EmailBody != ""))
            {
                EmailSubject = EmailSubject.Replace("[UserName]", Name);

                EmailBody = EmailBody.Replace("[UserName]", Name);
                EmailBody = EmailBody.Replace("[Money]", Money.ToString());

                PF.SendEmail(Site, Email, EmailSubject, EmailBody);
            }
        }

        //Send SMS
        if ((Mobile != "") && isMobileValided && (DAL.Functions.F_GetIsSendNotification(SiteID, NotificationManners.SMS, NotificationTypes.TryDistill, ID)))
        {
            string Body = Site.SiteNotificationTemplates[NotificationManners.SMS, NotificationTypes.TryDistill];

            if (Body != "")
            {
                Body = Body.Replace("[UserName]", Name);
                Body = Body.Replace("[Money]", Money.ToString());

                PF.SendSMS(Site, ID, Mobile, Body);
            }
        }

        //Send StationSMS
        if (DAL.Functions.F_GetIsSendNotification(SiteID, NotificationManners.StationSMS, NotificationTypes.TryDistill, ID))
        {
            string Body = Site.SiteNotificationTemplates[NotificationManners.StationSMS, NotificationTypes.TryDistill];

            if (Body != "")
            {
                Body = Body.Replace("[UserName]", Name);
                Body = Body.Replace("[Money]", Money.ToString());

                PF.SendStationSMS(Site, Site.AdministratorID, ID, StationSMSTypes.SystemMessage, Body);
            }
        }

        return 0;
    }

    public int DistillQuash(long DistillID, ref string ReturnDescription)
    {
        if (ID < 0)
        {
            throw new Exception("Users 尚未初始化到具体的数据实例上，请先使用 GetUserInformation 等获取数据信息");
        }

        int ReturnValue = -1;
        ReturnDescription = "";

        int Result = DAL.Procedures.P_DistillQuash(Site.ID, ID, DistillID, ref ReturnValue, ref  ReturnDescription);

        if (Result < 0)
        {
            ReturnDescription = "数据库读写错误";

            return -1;
        }

        if (ReturnValue < 0)
        {
            return ReturnValue;
        }

        string ReturnDescription_2 = "";
        GetUserInformationByID(ref ReturnDescription_2);

        return 0;
    }

    public int DistillAccept(long DistillID, string PayName, string PayBank, string PayCardNumber, string _AlipayID, string _AlipayName, string Memo, long HandleOperatorID, ref string ReturnDescription)
    {
        if (ID < 0)
        {
            throw new Exception("Users 尚未初始化到具体的数据实例上，请先使用 GetUserInformation 等获取数据信息");
        }

        int ReturnValue = -1;
        ReturnDescription = "";

        int Result = DAL.Procedures.P_DistillAccept(Site.ID, ID, DistillID, PayName, PayBank, PayCardNumber, _AlipayID, _AlipayName, Memo, HandleOperatorID, ref ReturnValue, ref ReturnDescription);

        if (Result < 0)
        {
            ReturnDescription = "数据库读写错误";

            return -1;
        }

        if (ReturnValue < 0)
        {
            return ReturnValue;
        }

        string ReturnDescription_2 = "";
        GetUserInformationByID(ref ReturnDescription_2);

        //Send Email
        if ((Email != "") && (DAL.Functions.F_GetIsSendNotification(SiteID, NotificationManners.Email, NotificationTypes.DistillAccept, ID)))
        {
            string EmailSubject = "", EmailBody = "";

            Site.SiteNotificationTemplates.SplitEmailTemplate(Site.SiteNotificationTemplates[NotificationManners.Email, NotificationTypes.DistillAccept], ref EmailSubject, ref EmailBody);

            if ((EmailSubject != "") && (EmailBody != ""))
            {
                EmailSubject = EmailSubject.Replace("[UserName]", Name);

                EmailBody = EmailBody.Replace("[UserName]", Name);

                PF.SendEmail(Site, Email, EmailSubject, EmailBody);
            }
        }

        //Send SMS
        if ((Mobile != "") && isMobileValided && (DAL.Functions.F_GetIsSendNotification(SiteID, NotificationManners.SMS, NotificationTypes.DistillAccept, ID)))
        {
            string Body = Site.SiteNotificationTemplates[NotificationManners.SMS, NotificationTypes.DistillAccept];

            if (Body != "")
            {
                Body = Body.Replace("[UserName]", Name);

                PF.SendSMS(Site, ID, Mobile, Body);
            }
        }

        //Send StationSMS
        if (DAL.Functions.F_GetIsSendNotification(SiteID, NotificationManners.StationSMS, NotificationTypes.DistillAccept, ID))
        {
            string Body = Site.SiteNotificationTemplates[NotificationManners.StationSMS, NotificationTypes.DistillAccept];

            if (Body != "")
            {
                Body = Body.Replace("[UserName]", Name);

                PF.SendStationSMS(Site, Site.AdministratorID, ID, StationSMSTypes.SystemMessage, Body);
            }
        }

        return 0;
    }

    public int DistillNoAccept(long DistillID, string Memo, long HandleOperatorID, ref string ReturnDescription)
    {
        if (ID < 0)
        {
            throw new Exception("Users 尚未初始化到具体的数据实例上，请先使用 GetUserInformation 等获取数据信息");
        }

        int ReturnValue = -1;
        ReturnDescription = "";

        int Result = DAL.Procedures.P_DistillNoAccept(Site.ID, ID, DistillID, Memo, HandleOperatorID, ref ReturnValue, ref ReturnDescription);

        if (Result < 0)
        {
            ReturnDescription = "数据库读写错误";

            return -1;
        }

        if (ReturnValue < 0)
        {
            return ReturnValue;
        }

        string ReturnDescription_2 = "";
        GetUserInformationByID(ref ReturnDescription_2);

        //Send Email
        if ((Email != "") && (DAL.Functions.F_GetIsSendNotification(SiteID, NotificationManners.Email, NotificationTypes.DistillNoAccept, ID)))
        {
            string EmailSubject = "", EmailBody = "";

            Site.SiteNotificationTemplates.SplitEmailTemplate(Site.SiteNotificationTemplates[NotificationManners.Email, NotificationTypes.DistillNoAccept], ref EmailSubject, ref EmailBody);

            if ((EmailSubject != "") && (EmailBody != ""))
            {
                EmailSubject = EmailSubject.Replace("[UserName]", Name);

                EmailBody = EmailBody.Replace("[UserName]", Name);
                EmailBody = EmailBody.Replace("[Memo]", Memo);

                PF.SendEmail(Site, Email, EmailSubject, EmailBody);
            }
        }

        //Send SMS
        if ((Mobile != "") && isMobileValided && (DAL.Functions.F_GetIsSendNotification(SiteID, NotificationManners.SMS, NotificationTypes.DistillNoAccept, ID)))
        {
            string Body = Site.SiteNotificationTemplates[NotificationManners.SMS, NotificationTypes.DistillNoAccept];

            if (Body != "")
            {
                Body = Body.Replace("[UserName]", Name);
                Body = Body.Replace("[Memo]", Memo);

                PF.SendSMS(Site, ID, Mobile, Body);
            }
        }

        //Send StationSMS
        if (DAL.Functions.F_GetIsSendNotification(SiteID, NotificationManners.StationSMS, NotificationTypes.DistillNoAccept, ID))
        {
            string Body = Site.SiteNotificationTemplates[NotificationManners.StationSMS, NotificationTypes.DistillNoAccept];

            if (Body != "")
            {
                Body = Body.Replace("[UserName]", Name);
                Body = Body.Replace("[Memo]", Memo);

                PF.SendStationSMS(Site, Site.AdministratorID, ID, StationSMSTypes.SystemMessage, Body);
            }
        }

        return 0;
    }

    public int CpsDistill(double Money, double FormalitiesFees, string BankUserName, string _BankName, string _BankCardNumber,string Memo, ref string ReturnDescription)
    {
        if (ID < 0)
        {
            throw new Exception("Users 尚未初始化到具体的数据实例上，请先使用 GetUserInformation 等获取数据信息");
        }

        int ReturnValue = -1;
        ReturnDescription = "";

        int Result = DAL.Procedures.P_CpsDistill(Site.ID, ID, Money, FormalitiesFees, BankUserName, _BankName, _BankCardNumber, Memo, ref ReturnValue, ref ReturnDescription);

        if (Result < 0)
        {
            ReturnDescription = "数据库读写错误";

            return -1;
        }

        if (ReturnValue < 0)
        {
            return ReturnValue;
        }

        string ReturnDescription_2 = "";
        GetUserInformationByID(ref ReturnDescription_2);

        return 0;
    }

    public int CpsDistillQuash(long DistillID, ref string ReturnDescription)
    {
        if (ID < 0)
        {
            throw new Exception("Users 尚未初始化到具体的数据实例上，请先使用 GetUserInformation 等获取数据信息");
        }

        int ReturnValue = -1;
        ReturnDescription = "";

        int Result = DAL.Procedures.P_CpsDistillQuash(Site.ID, ID, DistillID, ref ReturnValue, ref  ReturnDescription);

        if (Result < 0)
        {
            ReturnDescription = "数据库读写错误";

            return -1;
        }

        if (ReturnValue < 0)
        {
            return ReturnValue;
        }

        string ReturnDescription_2 = "";
        GetUserInformationByID(ref ReturnDescription_2);

        return 0;
    }

    public int CpsDistillAccept(long DistillID, string PayName, string PayBank, string PayCardNumber, string Memo, long HandleOperatorID, ref string ReturnDescription)
    {
        if (ID < 0)
        {
            throw new Exception("Users 尚未初始化到具体的数据实例上，请先使用 GetUserInformation 等获取数据信息");
        }

        int ReturnValue = -1;
        ReturnDescription = "";

        int Result = DAL.Procedures.P_CpsDistillAccept(Site.ID, ID, DistillID, PayName, PayBank, PayCardNumber, Memo, HandleOperatorID, ref ReturnValue, ref ReturnDescription);

        if (Result < 0)
        {
            ReturnDescription = "数据库读写错误";

            return -1;
        }

        if (ReturnValue < 0)
        {
            return ReturnValue;
        }

        string ReturnDescription_2 = "";
        GetUserInformationByID(ref ReturnDescription_2);

        return 0;
    }

    public int CpsDistillNoAccept(long DistillID, string Memo, long HandleOperatorID, ref string ReturnDescription)
    {
        if (ID < 0)
        {
            throw new Exception("Users 尚未初始化到具体的数据实例上，请先使用 GetUserInformation 等获取数据信息");
        }

        int ReturnValue = -1;
        ReturnDescription = "";

        int Result = DAL.Procedures.P_CpsDistillNoAccept(Site.ID, ID, DistillID, Memo, HandleOperatorID, ref ReturnValue, ref ReturnDescription);

        if (Result < 0)
        {
            ReturnDescription = "数据库读写错误";

            return -1;
        }

        if (ReturnValue < 0)
        {
            return ReturnValue;
        }

        string ReturnDescription_2 = "";
        GetUserInformationByID(ref ReturnDescription_2);

        return 0;
    }

    #endregion

    #region 积分兑换

    public int ScoringExchange(double Scoring, ref string ReturnDescription)
    {
        if (ID < 0)
        {
            throw new Exception("Users 尚未初始化到具体的数据实例上，请先使用 GetUserInformation 等获取数据信息");
        }

        int ReturnValue = -1;
        ReturnDescription = "";

        int Result = DAL.Procedures.P_ScoringExchange(Site.ID, ID, Scoring, ref ReturnValue, ref ReturnDescription);

        if (Result < 0)
        {
            ReturnDescription = "数据库读写错误";

            return -1;
        }

        if (ReturnValue < 0)
        {
            return ReturnValue;
        }

        string ReturnDescription_2 = "";
        GetUserInformationByID(ref ReturnDescription_2);

        return 0;
    }

    #endregion

    #region 是否在招股对象范围之内、以及能够查看方案

    public bool isInOpenUsers(long SchemeID)
    {
        return isInOpenUsers(DAL.Functions.F_GetSchemeOpenUsers(SchemeID));
    }

    public bool isInOpenUsers(string OpenUsers) // OpenUsers:[1][2]...
    {
        if (ID < 0)
        {
            throw new Exception("Users 尚未初始化到具体的数据实例上，请先使用 GetUserInformation 等获取数据信息");
        }

        if (OpenUsers == "")
        {
            return true;
        }

        return (OpenUsers.IndexOf("[" + ID.ToString() + "]") >= 0);
    }

    public bool isCanViewSchemeContent(long SchemeID)
    {
        bool isInitiateUser = (ID == DAL.Functions.F_GetSchemeInitiateUserID(SiteID, SchemeID));
        bool isInOpenUsersList = isInOpenUsers(SchemeID);
        bool isHasOtherViewCompetences = isOwnedViewSchemeCompetence();

        return (isInitiateUser || isInOpenUsersList || isHasOtherViewCompetences);
    }

    public bool isOwnedViewSchemeCompetence()
    {
        return this.Competences.IsOwnedCompetences(Competences.BuildCompetencesList(Competences.Administrator, Competences.LotteryBuy, Competences.LotteryWin, Competences.Finance));
    }

    #endregion

    public void Clone(Users user)
    {
        user._id = _id;
        user._siteid = _siteid;
        user._password = _password;
        user._passwordadv = _passwordadv;

        user.Name = Name;
        user.RealityName = RealityName;

        user.Sex = Sex;
        user.BirthDay = BirthDay;
        user.IDCardNumber = IDCardNumber;
        user.CityID = CityID;
        user.Address = Address;
        user.Email = Email;
        user.isEmailValided = isEmailValided;
        user.QQ = QQ;
        user.isQQValided = isQQValided;
        user.Telephone = Telephone;
        user.Mobile = Mobile;
        user.isMobileValided = isMobileValided;

        user.isPrivacy = isPrivacy;
        user.isCanLogin = isCanLogin;

        user.RegisterTime = RegisterTime;
        user.LastLoginTime = LastLoginTime;
        user.LastLoginIP = LastLoginIP;
        user.LoginCount = LoginCount;

        user.UserType = UserType;

        user.BankType = BankType;
        user.BankName = BankName;
        user.BankCardNumber = BankCardNumber;

        user.Balance = Balance;
        user.Freeze = Freeze;
        user.ScoringOfSelfBuy = ScoringOfSelfBuy;
        user.ScoringOfCommendBuy = ScoringOfCommendBuy;
        user.Scoring = Scoring;
        user.Bonus = Bonus;
        user.Reward = Reward;

        user.Level = Level;

        user.CommenderID = CommenderID;
        user.CpsID = CpsID;
        user.cps = cps;
        user.isAlipayCps = isAlipayCps;

        user.AlipayID = AlipayID;
        user.AlipayName = AlipayName;
        user.isAlipayNameValided = isAlipayNameValided;

        user.Site = Site;

        user.Competences = Competences;

        user.OwnerSites = OwnerSites;

        user.VisitSource = VisitSource;
    }

    #region 得到推广URL
    public string GetPromotionURL(int type)
    {
        if (ID < 0)
        {
            throw new Exception("Users 尚未初始化到具体的数据实例上，请先使用 GetUserInformation 等获取数据信息");
        }

        string fileName = "PromoteUserReg.aspx";

        if (type == 1)
        {
            fileName = "PromoteCpsReg.aspx";
        }

        string url = Shove._Web.Utility.GetUrl() + "/Home/Room/" + fileName + "?id=" + ID.ToString().PadLeft(10, '0') + GetSign().ToString();

        return url;
    }

    public int GetSign()
    {
        /*1.把用户ID的位数和系数相乘的结果相加
        从高位到低位系数分别为8、2、5、7、1
         2.得到的结果除以8，取余数
         */
        int number = 0;

        int[] coefficient = new int[] { 8, 2, 5, 7, 1 };

        for (int i = 0; i < ID.ToString().Length; i++)
        {
            number += Shove._Convert.StrToInt(ID.ToString().Substring(i, 1), 0) * coefficient[i % 5];
        }

        number = number % 10;

        return number;
    }
    #endregion
}