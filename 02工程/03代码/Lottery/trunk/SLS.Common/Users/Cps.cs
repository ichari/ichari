using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace SLS.Common
{
    /// <summary>
    /// Cps 的摘要说明
    /// </summary>
    public class Cps
    {
        public long ID;
        public long SiteID;
        public long OwnerUserID;
        public string Name;

        public string Url;
        public string LogoUrl;

        public double BonusScale;

        public bool ON;
        public bool IsShow;

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
        public string MD5Key;
        public short Type;
        public string DomainName;
        public long ParentID;
        public long OperatorID;
        public long CommendID;
        public DateTime DateTime;

        public String Content;

        public Users User;

        public Cps()
        {
            ID = -1;

            SiteID = -1;
            OwnerUserID = -1;
            Name = "";

            Url = "";
            LogoUrl = "";

            BonusScale = 0;

            ON = false;
            IsShow = true;

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
            MD5Key = "";
            Type = -1;
            DomainName = "";
            ParentID = -1;
            OperatorID = 1;
            CommendID = -1;
            DateTime = DateTime.Now;

            Content = null;

        }

        public Cps(Users user)
        {
            User = user;

            ID = -1;
            SiteID = user.SiteID;
            OwnerUserID = user.ID;
            Name = "";

            Url = "";
            LogoUrl = "";

            BonusScale = 0;

            ON = false;
            IsShow = true;

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
            MD5Key = "";
            Type = -1;
            DomainName = "";
            ParentID = -1;
            OperatorID = 1;
            CommendID = -1;
            DateTime = DateTime.Now;

            Content = null;
        }

        public int Add(ref string ReturnDescription)
        {
            if ((SiteID < 0) || (OwnerUserID < 0))
            {
                throw new Exception("Cps 尚未初始化 SiteID、 OwnerUserID 变量，无法增加数据");
            }

            ReturnDescription = "";

            int ReturnValue = SLS.Dal.Procedures.P_CpsAdd(PF.ConnectString,User.Site.ID, OwnerUserID, Name, Url, LogoUrl, BonusScale, ON, Company, Address, PostCode, ResponsiblePerson,
                ContactPerson, Telephone, Fax, Mobile, Email, QQ, ServiceTelephone, MD5Key, Type, ParentID, DomainName, OperatorID, CommendID, ref ID, ref ReturnDescription);

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

        public int Try(ref string ReturnDescription)
        {
            if (SiteID < 0 || OwnerUserID < 0)
            {
                throw new Exception("Cps 尚未初始化 SiteID、 OwnerUserID 变量，无法增加数据");
            }

            ReturnDescription = "";

            int ReturnValue = SLS.Dal.Procedures.P_CpsTry(PF.ConnectString,User.Site.ID, OwnerUserID, Content, Name, Url, LogoUrl, Company, Address, PostCode, ResponsiblePerson, ContactPerson, Telephone, Fax, Mobile, Email, QQ, ServiceTelephone, MD5Key, Type, DomainName, ParentID, BonusScale, CommendID, ref ID, ref ReturnDescription);

            if (ReturnValue < 0)
            {

                return -1;
            }

            //if (ID < 0)
            //{
            //    return (int)ID;
            //}

            return 0;
        }

        public int EditByID(ref string ReturnDescription)
        {
            if ((ID < 0) || (SiteID < 0) || (OwnerUserID < 0))
            {
                throw new Exception("Cps 尚未初始化到具体的数据实例上，请先使用 GetCpsInformation 等获取数据信息");
            }

            int ReturnValue = -1;
            ReturnDescription = "";

            int Result = SLS.Dal.Procedures.P_CpsEdit(PF.ConnectString,User.Site.ID, ID, Name, Url, LogoUrl, BonusScale, ON,
               Company, Address, PostCode, ResponsiblePerson, ContactPerson, Telephone, Fax, Mobile, Email, QQ, ServiceTelephone, MD5Key, Type, DomainName, IsShow, ref ReturnValue, ref ReturnDescription);

            if (Result < 0)
            {
                ReturnDescription = "数据库读写错误";

                return -1;
            }

            return ReturnValue;
        }

        public int GetCpsInformationByOwnerUserID(ref string ReturnDescription)
        {
            if ((SiteID < 0) || (OwnerUserID < 0))
            {
                throw new Exception("Cps 尚未初始化 SiteID、 OwnerUserID 变量，无法获取信息");
            }

            int ReturnValue = -1;
            ReturnDescription = "";

            Cps tp = new Cps();
            Clone(tp);

            int Result = SLS.Dal.Procedures.P_GetCpsInformationByOwnerUserID(PF.ConnectString,SiteID, OwnerUserID, ref ID, ref Name, ref DateTime, ref Url, ref LogoUrl, ref BonusScale, ref ON,
                ref Company, ref Address, ref PostCode, ref ResponsiblePerson, ref ContactPerson, ref Telephone, ref Fax, ref Mobile, ref Email, ref QQ, ref ServiceTelephone, ref MD5Key,
                ref Type, ref ParentID, ref DomainName, ref ReturnValue, ref ReturnDescription);

            if (Result < 0)
            {
                tp.Clone(this);

                ReturnDescription = "数据库读写错误";

                return -1;
            }

            if (ReturnValue < 0)
            {
                tp.Clone(this);

                return ReturnValue;
            }

            return 0;
        }

        public int GetCpsInformationByID(ref string ReturnDescription)
        {
            if ((SiteID < 0) || (ID < 0))
            {
                throw new Exception("Cps 尚未初始化 SiteID、 ID 变量，无法获取信息");
            }

            int ReturnValue = -1;
            ReturnDescription = "";

            Cps tp = new Cps();
            Clone(tp);

            int Result = SLS.Dal.Procedures.P_GetCpsInformationByID(PF.ConnectString,SiteID, ID, ref OwnerUserID, ref Name, ref DateTime, ref Url, ref LogoUrl, ref BonusScale, ref ON,
                ref Company, ref Address, ref PostCode, ref ResponsiblePerson, ref ContactPerson, ref Telephone, ref Fax, ref Mobile, ref Email, ref QQ, ref ServiceTelephone, ref MD5Key,
                ref Type, ref ParentID, ref DomainName, ref ReturnValue, ref ReturnDescription);

            if (Result < 0)
            {
                tp.Clone(this);

                ReturnDescription = "数据库读写错误";

                return -1;
            }

            if (ReturnValue < 0)
            {
                tp.Clone(this);

                return ReturnValue;
            }

            return 0;
        }

        public void Clone(Cps cps)
        {
            cps.ID = ID;
            cps.SiteID = SiteID;
            cps.OwnerUserID = OwnerUserID;
            cps.Name = Name;
            cps.DateTime = DateTime;

            cps.Url = Url;
            cps.LogoUrl = LogoUrl;

            cps.BonusScale = BonusScale;

            cps.ON = ON;

            cps.Company = Company;
            cps.Address = Address;
            cps.PostCode = PostCode;
            cps.ResponsiblePerson = ResponsiblePerson;
            cps.ContactPerson = ContactPerson;
            cps.Telephone = Telephone;
            cps.Fax = Fax;
            cps.Mobile = Mobile;
            cps.Email = Email;
            cps.QQ = QQ;
            cps.ServiceTelephone = ServiceTelephone;
            cps.MD5Key = MD5Key;
            cps.Type = Type;
            cps.DomainName = DomainName;
            cps.ParentID = ParentID;
            cps.OperatorID = OperatorID;
            cps.CommendID = CommendID;

            cps.Content = Content;

            cps.User = User;
        }
    }
}