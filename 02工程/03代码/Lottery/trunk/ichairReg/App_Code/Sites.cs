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
/// Sites 的摘要说明
/// </summary>
[Serializable]
public class Sites
{
    #region 成员变量

    private string _urls;

    public long ID;
    public long ParentID;
    public long OwnerUserID;
    public string Name;
    public string LogoUrl;
    public string Company;
    public string Address;
    public string PostCode;
    public string ResponsiblePerson;
    public string ContactPerson;
    public string Telephone;
    public string Fax;
    public string Mobile;
    public string Email;
    public string QQ;
    public string ServiceTelephone;
    public string ICPCert;
    public short Level;
    public bool ON;
    public double BonusScale;
    public int MaxSubSites;
    public string UseLotteryListRestrictions;
    public string UseLotteryList;
    public string UseLotteryListQuickBuy;

    public long AdministratorID;
    public string Urls
    {
        get
        {
            return _urls;
        }
        set
        {
            _urls = value;

            Url = "";
            try
            {
                Url = value.Split(',')[0];
            }
            catch { }
        }
    }
    public string Url;

    public SiteNotificationTemplates SiteNotificationTemplates;
    public SiteOptions SiteOptions;

    public string PageTitle;
    public string PageKeywords;

    #endregion

    #region 索引器

    public Sites this[long siteid]
    {
        get
        {
            Sites site = new Sites();

            site.ID = siteid;

            string ReturnDescription = "";

            if (site.GetSiteInformationByID(ref ReturnDescription) < 0)
            {
                return null;
            }

            return site;
        }
    }

    public Sites this[string url]
    {
        get
        {
            string SystemPreFix = "Site_";
            Sites site = null;

            try
            {
                site = (Sites)System.Web.HttpContext.Current.Application[SystemPreFix + url];
            }
            catch { }

            if (site != null)
            {
                return site;
            }

            site = new Sites();

            site.Url = url;
            string ReturnDescription = "";

            if (site.GetSiteInformationByUrl(ref ReturnDescription) < 0)
            {
                return null;
            }

            try
            {
                System.Web.HttpContext.Current.Application.Lock();
                System.Web.HttpContext.Current.Application.Add(SystemPreFix + url, site);
            }
            catch { }
            finally
            {
                try
                {
                    System.Web.HttpContext.Current.Application.UnLock();
                }
                catch { }
            }

            return site;
        }
    }

    #endregion

    public Sites()
    {
        SiteNotificationTemplates = new SiteNotificationTemplates(this);
        SiteOptions = new SiteOptions(this);

        ID = -1;

        ParentID = -1;
        OwnerUserID = -1;

        Name = "";
        LogoUrl = "";
        Company = "";
        Address = "";
        PostCode = "";
        ResponsiblePerson = "";
        ContactPerson = "";
        Telephone = "";
        Fax = "";
        Mobile = "";
        Email = "";
        QQ = "";
        ServiceTelephone = "";
        ICPCert = "";
        Level = -1;
        ON = false;
        BonusScale = 0;
        MaxSubSites = 0;
        UseLotteryList = "";

        AdministratorID = -1;

        _urls = "";
        Url = "";

        PageTitle = "";
        PageKeywords = "";
    }

    public Sites(long siteid)
    {
        SiteNotificationTemplates = new SiteNotificationTemplates(this);
        SiteOptions = new SiteOptions(this);

        ID = siteid;

        ParentID = -1;
        OwnerUserID = -1;

        Name = "";
        LogoUrl = "";
        Company = "";
        Address = "";
        PostCode = "";
        ResponsiblePerson = "";
        ContactPerson = "";
        Telephone = "";
        Fax = "";
        Mobile = "";
        Email = "";
        QQ = "";
        ServiceTelephone = "";
        ICPCert = "";
        Level = -1;
        ON = false;
        BonusScale = 0;
        MaxSubSites = 0;
        UseLotteryListRestrictions = "";
        UseLotteryList = "";
        UseLotteryListQuickBuy = "";

        AdministratorID = -1;

        _urls = "";
        Url = "";

        PageTitle = "";
        PageKeywords = "";
    }

    public int Add(ref string ReturnDescription)
    {
        if ((ParentID < 0) || (OwnerUserID < 0))
        {
            throw new Exception("Sites 尚未初始化 ParentID、OwnerUserID 变量，无法增加数据");
        }

        ReturnDescription = "";

        int ReturnValue = DAL.Procedures.P_SiteAdd(ParentID, OwnerUserID, Name, LogoUrl, Company, Address, PostCode, ResponsiblePerson, ContactPerson,
            Telephone, Fax, Mobile, Email, QQ, ServiceTelephone, ICPCert, Level, ON, BonusScale, MaxSubSites, UseLotteryListRestrictions, UseLotteryList, UseLotteryListQuickBuy, Urls,
            ref AdministratorID, ref ID, ref ReturnDescription);

        if (ReturnValue < 0)
        {
            ReturnDescription = "数据库读写错误";

            return -1;
        }

        if (ID < 0)
        {
            return (int)ID;
        }

        return 0;
    }

    public int EditByID(ref string ReturnDescription)
    {
        if ((ID < 0) || ((ParentID < 0) && (ID != 1)) || (OwnerUserID < 0))
        {
            throw new Exception("Sites 尚未初始化到具体的数据实例上，请先使用 GetSiteInformation 等获取数据信息");
        }

        int ReturnValue = -1;
        ReturnDescription = "";

        int Result = DAL.Procedures.P_SiteEdit(ID, Name, LogoUrl, Company, Address, PostCode, ResponsiblePerson, ContactPerson,
            Telephone, Fax, Mobile, Email, QQ, ServiceTelephone, ICPCert, ON, BonusScale, MaxSubSites, UseLotteryListRestrictions, UseLotteryList, UseLotteryListQuickBuy, Urls,
            ref ReturnValue, ref ReturnDescription);

        if (Result < 0)
        {
            ReturnDescription = "数据库读写错误";

            return -1;
        }

        string SystemPreFix = "Site_";

        try
        {
            System.Web.HttpContext.Current.Application.Lock();

            foreach (string url in this.Urls.Split(','))
            {
                System.Web.HttpContext.Current.Application.Remove(SystemPreFix + url);
            }
        }
        catch { }
        finally
        {
            try
            {
                System.Web.HttpContext.Current.Application.UnLock();
            }
            catch { }
        }

        return ReturnValue;
    }

    public int GetSiteInformationByID(ref string ReturnDescription)
    {
        if (ID < 0)
        {
            throw new Exception("Sites 尚未初始化 ID 变量，无法获取信息");
        }

        int ReturnValue = -1;
        ReturnDescription = "";

        Sites ts = new Sites();
        Clone(ts);

        int Result = DAL.Procedures.P_GetSiteInformationByID(ID, ref ParentID, ref OwnerUserID, ref Name, ref LogoUrl, ref Company, ref Address, ref PostCode,
            ref ResponsiblePerson, ref ContactPerson, ref Telephone, ref Fax, ref Mobile, ref Email, ref QQ, ref ServiceTelephone, ref ICPCert, ref Level, ref ON,
            ref BonusScale, ref MaxSubSites, ref UseLotteryListRestrictions, ref UseLotteryList, ref UseLotteryListQuickBuy, ref _urls, ref AdministratorID, ref PageTitle, ref PageKeywords,
            ref ReturnValue, ref ReturnDescription);

        if (Result < 0)
        {
            ts.Clone(this);

            ReturnDescription = "数据库读写错误";

            return -1;
        }

        if (ReturnValue < 0)
        {
            ts.Clone(this);

            return ReturnValue;
        }

        Urls = _urls;

        return 0;
    }

    public int GetSiteInformationByUrl(ref string ReturnDescription)
    {
        if ((Url == null) || (Url == ""))
        {
            throw new Exception("Sites 尚未初始化 Url 变量，无法获取信息");
        }

        int ReturnValue = -1;
        ReturnDescription = "";

        Sites ts = new Sites();
        Clone(ts);

        int Result = DAL.Procedures.P_GetSiteInformationByUrl(Url, ref ID, ref ParentID, ref OwnerUserID, ref Name, ref LogoUrl, ref Company, ref Address, ref PostCode,
            ref ResponsiblePerson, ref ContactPerson, ref Telephone, ref Fax, ref Mobile, ref Email, ref QQ, ref ServiceTelephone, ref ICPCert, ref Level, ref ON,
            ref BonusScale, ref MaxSubSites, ref UseLotteryListRestrictions, ref UseLotteryList, ref UseLotteryListQuickBuy, ref _urls, ref AdministratorID, ref PageTitle, ref PageKeywords,
            ref ReturnValue, ref ReturnDescription);

        if (Result < 0)
        {
            ts.Clone(this);

            ReturnDescription = "数据库读写错误";

            return -1;
        }

        if (ReturnValue < 0)
        {
            ts.Clone(this);

            return ReturnValue;
        }

        Urls = _urls;

        return 0;
    }

    public void ReBuildUseLotteryList()
    {
        if (UseLotteryListRestrictions == "")
        {
            UseLotteryList = "";
            UseLotteryListQuickBuy = "";

            return;
        }

        // 重构使用彩种
        string[] strs = UseLotteryList.Split(',');

        if (strs.Length > 0)
        {
            UseLotteryList = "";

            foreach (string t in strs)
            {
                if (("," + UseLotteryListRestrictions + ",").IndexOf("," + t + ",") >= 0)
                {
                    UseLotteryList += (UseLotteryList == "" ? "" : ",") + t;
                }
            }
        }

        // 重构快速投注彩种
        strs = UseLotteryListQuickBuy.Split(',');

        if (strs.Length > 0)
        {
            UseLotteryListQuickBuy = "";

            foreach (string t in strs)
            {
                if (("," + UseLotteryListRestrictions + ",").IndexOf("," + t + ",") >= 0)
                {
                    UseLotteryListQuickBuy += (UseLotteryListQuickBuy == "" ? "" : ",") + t;
                }
            }
        }
    }

    public void Clone(Sites site)
    {
        site.ID = ID;
        site.ParentID = ParentID;
        site.OwnerUserID = OwnerUserID;
        site.Name = Name;
        site.LogoUrl = LogoUrl;
        site.Company = Company;
        site.Address = Address;
        site.PostCode = PostCode;
        site.ResponsiblePerson = ResponsiblePerson;
        site.ContactPerson = ContactPerson;
        site.Telephone = Telephone;
        site.Fax = Fax;
        site.Mobile = Mobile;
        site.Email = Email;
        site.QQ = QQ;
        site.ServiceTelephone = ServiceTelephone;
        site.ICPCert = ICPCert;
        site.Level = Level;
        site.ON = ON;
        site.BonusScale = BonusScale;
        site.MaxSubSites = MaxSubSites;
        site.UseLotteryListRestrictions = UseLotteryListRestrictions;
        site.UseLotteryList = UseLotteryList;
        site.UseLotteryListQuickBuy = UseLotteryListQuickBuy;
        site.AdministratorID = AdministratorID;

        site._urls = _urls;
        site.Url = Url;

        site.SiteNotificationTemplates = SiteNotificationTemplates;
        site.SiteOptions = SiteOptions;

        site.PageTitle = PageTitle;
        site.PageKeywords = PageKeywords;
    }
}
