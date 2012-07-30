using System;
using System.Reflection;
using System.Text;
using System.IO;
using System.Threading;
using System.Web;
using System.Xml.Serialization;

using Discuz.Common;
using Discuz.Data;
using Discuz.Plugin.Mail;
using Discuz.Config;

namespace Discuz.Forum
{
    /// <summary>
    /// Discuz!NT邮件发送类的调用封装类
    /// </summary>
    public class Emails
    {
        protected static GeneralConfigInfo configinfo = GeneralConfigs.GetConfig();

        protected static ISmtpMail ESM;

        protected static EmailConfigInfo emailinfo = EmailConfigs.GetConfig();

        static Emails()
        {
            if (emailinfo.DllFileName.ToLower().IndexOf(".dll") <= 0)
            {
                emailinfo.DllFileName = emailinfo.DllFileName + ".dll";
            }
            try
            {
                //读取相应的DLL信息
                Assembly asm = Assembly.LoadFrom(HttpRuntime.BinDirectory + emailinfo.DllFileName);
                ESM = (ISmtpMail)Activator.CreateInstance(asm.GetType(emailinfo.PluginNameSpace, false, true));
            }
            catch
            {
                try
                {
                    //读取相应的DLL信息
                    Assembly asm = Assembly.LoadFrom(Utils.GetMapPath("/bin/" + emailinfo.DllFileName));
                    ESM = (ISmtpMail)Activator.CreateInstance(asm.GetType(emailinfo.PluginNameSpace, false, true));
                }
                catch
                {
                    ESM = new SmtpMail();
                }
            }
        }


        //重设置当前邮件发送类的实例对象
        public static void ReSetISmtpMail()
        {
            try
            {
                emailinfo = EmailConfigs.GetConfig();
                //读取相应的DLL信息
                Assembly asm = Assembly.LoadFrom(HttpRuntime.BinDirectory + emailinfo.DllFileName);
                ESM = (ISmtpMail)Activator.CreateInstance(asm.GetType(emailinfo.PluginNameSpace, false, true));
            }
            catch
            {
                try
                {
                    //读取相应的DLL信息
                    Assembly asm = Assembly.LoadFrom(Utils.GetMapPath("/bin/" + emailinfo.DllFileName));
                    ESM = (ISmtpMail)Activator.CreateInstance(asm.GetType(emailinfo.PluginNameSpace, false, true));
                }
                catch
                {
                    ESM = new SmtpMail();
                }
            }
        }


        /// <summary>
        /// 定义邮件内容函数
        /// </summary>
        /// <param name="UserName">用户名</param>
        /// <param name="Email">EMAIL地址</param>
        /// <param name="pass">相应注册用户的密码(暂时无设置)</param>
        /// <returns></returns>
        public static bool DiscuzSmtpMail(string UserName, string Email, string pass)
        {
            string forumurl = "http://" + DNTRequest.GetCurrentFullHost() + ("/").ToLower();

            try
            {
                ESM.RecipientName = UserName;//设定收件人姓名
                ESM.AddRecipient(Email);//设定收件人地址(必须填写)。
                ESM.MailDomainPort = emailinfo.Port;
                ESM.From = emailinfo.Sysemail;
                ESM.FromName = configinfo.Webtitle;
                ESM.Html = true;
                ESM.Subject = "已成功创建你的 " + configinfo.Forumtitle + "帐户,请查收.";

                StringBuilder body = new StringBuilder();
                body.Append(emailinfo.Emailcontent.Replace("{webtitle}", configinfo.Webtitle));
                body.Replace("{weburl}", "<a href=" + configinfo.Weburl + ">" + configinfo.Weburl + "</a>");
                body.Replace("{forumurl}", "<a href=" + forumurl + ">" + forumurl + "</a>");
                body.Replace("{forumtitle}", configinfo.Forumtitle);

                ESM.Body = "<pre style=\"width:100%;word-wrap:break-word\">" + body.ToString() + "</pre>";
                ESM.MailDomain = emailinfo.Smtp;
                ESM.MailServerUserName = emailinfo.Username;
                ESM.MailServerPassWord = emailinfo.Password;

                //开始发送
                return ESM.Send();
            }
            catch
            {
                return false;
            }
        }


        /// <summary>
        /// 定义邮件内容函数
        /// </summary>
        /// <param name="UserName">用户名</param>
        /// <param name="Email">EMAIL地址</param>
        /// <param name="pass">相应注册用户的密码(暂时无设置)</param>
        /// <param name="authstr">相应注册用户的激活链接串参数</param>
        /// <returns></returns>
        public static bool DiscuzSmtpMail(string UserName, string Email, string pass, string authstr)
        {
            string forumurl = "http://" + DNTRequest.GetCurrentFullHost() + ("/").ToLower();

            try
            {
                ESM.RecipientName = UserName;//设定收件人姓名
                ESM.AddRecipient(Email);
                ESM.MailDomainPort = emailinfo.Port;
                ESM.From = emailinfo.Sysemail;
                ESM.FromName = configinfo.Webtitle;
                ESM.Html = true;
                ESM.Subject = "已成功创建你的 " + configinfo.Forumtitle + "帐户,请查收.";

                StringBuilder body = new StringBuilder();
                body.Append(emailinfo.Emailcontent.Replace("{webtitle}", configinfo.Webtitle));
                body.Replace("{weburl}", "<a href=" + configinfo.Weburl + ">" + configinfo.Weburl + "</a>");
                body.Replace("{forumurl}", "<a href=" + forumurl + ">" + forumurl + "</a>");
                body.Replace("{forumtitle}", configinfo.Forumtitle);

                ESM.Body = "<pre style=\"width:100%;word-wrap:break-word\">" + body.ToString() + "\r\n\r\n" + "激活您帐户的链接为:<a href=" + forumurl + "activationuser.aspx?authstr=" + authstr.Trim() + "  target=_blank>" + forumurl + "activationuser.aspx?authstr=" + authstr.Trim() + "</a></pre>";
                ESM.MailDomain = emailinfo.Smtp;
                ESM.MailServerUserName = emailinfo.Username;
                ESM.MailServerPassWord = emailinfo.Password;

                return ESM.Send();
            }
            catch
            {
                return false;
            }
        }


        /// <summary>
        /// 邮件通知服务
        /// </summary>
        /// <param name="Email">email地址</param>
        /// <param name="title">邮件的标题</param>
        /// <param name="body">邮件内容</param>
        /// <returns></returns>
        public static bool SendEmailNotify(string Email, string title, string body)
        {
            try
            {

                ESM.AddRecipient(Email);
                ESM.MailDomainPort = emailinfo.Port;
                ESM.From = emailinfo.Sysemail;//设定发件人地址(必须填写)
                ESM.FromName = configinfo.Webtitle;
                ESM.Html = true;//设定正文是否HTML格式。
                ESM.Subject = title;

                ESM.Body = "<pre style=\"width:100%;word-wrap:break-word\">" + body.ToString() + "</pre>";
                ESM.MailDomain = emailinfo.Smtp;
                ESM.MailServerUserName = emailinfo.Username;
                ESM.MailServerPassWord = emailinfo.Password;

                //开始发送
                return ESM.Send();
            }
            catch
            {
                return false;
            }
        }



        /// <summary>
        /// 用户邮件发送
        /// </summary>
        /// <param name="Email">email地址</param>
        /// <param name="title">邮件的标题</param>
        /// <param name="body">邮件内容</param>
        /// <returns></returns>
        public static bool DiscuzSmtpMailToUser(string Email, string title, string body)
        {

            try
            {
                ESM.AddRecipient(Email);

                ESM.MailDomainPort = emailinfo.Port;
                ESM.From = emailinfo.Sysemail;//设定发件人地址(必须填写)
                ESM.FromName = configinfo.Webtitle;
                ESM.Html = true;//设定正文是否HTML格式。
                ESM.Subject = title;


                ESM.Body = "<pre style=\"width:100%;word-wrap:break-word\">" + body.ToString() + "</pre>";
                ESM.MailDomain = emailinfo.Smtp;//"mail.discuz.com";//"smtp.163.com";
                //也可将将SMTP信息一次设置完成。写成
                ESM.MailServerUserName = emailinfo.Username;
                ESM.MailServerPassWord = emailinfo.Password;

                //开始发送
                return ESM.Send();
            }
            catch
            {
                return false;
            }
        }



    }


    //多线程发送邮箱类
    public class EmailMultiThread : Emails
    {

        private string m_username = "";

        private string m_email = "";

        private string m_title = "";

        private string m_body = "";

        private bool m_issuccess = false;

        public string UserName
        {
            get { return m_username; }
        }

        public string Email
        {
            get { return m_email; }
        }

        public string Title
        {
            get { return m_title; }
        }

        public string Body
        {
            get { return m_body; }
        }

        public bool IsSuccess
        {
            get { return m_issuccess; }
            set { m_issuccess = value; }
        }

        public EmailMultiThread(string UserName, string Email, string Title, string Body)
        {
            m_username = UserName;
            m_email = Email;
            m_title = Title;
            m_body = Body;
        }

        public void Send()
        {
            lock (emailinfo)
            {
                try
                {
                    ESM.MailDomainPort = emailinfo.Port;
                    ESM.AddRecipient(this.Email);
                    ESM.RecipientName = this.UserName;//设定收件人姓名

                    ESM.From = emailinfo.Sysemail;
                    ESM.FromName = configinfo.Webtitle;
                    ESM.Html = true;
                    ESM.Subject = this.Title;
                    ESM.Body = "<pre style=\"width:100%;word-wrap:break-word\">" + this.Body.ToString() + "</pre>";
                    ESM.MailDomain = emailinfo.Smtp;
                    ESM.MailServerUserName = emailinfo.Username;
                    ESM.MailServerPassWord = emailinfo.Password;

                    //开始发送
                    this.IsSuccess = ESM.Send();

                }
                catch
                {}

            }
            Thread.CurrentThread.Abort();
        }
    }

}
