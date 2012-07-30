using System;
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Web.UI;

using Discuz.Control;
using Discuz.Common;
using Discuz.Data;
using Discuz.Forum;
using Discuz.Config;


namespace Discuz.Web.Admin
{
    /// <summary>
    /// 日志管理
    /// </summary>
    
#if NET1
    public class logandshrinkdb : AdminPage
#else
    public partial class logandshrinkdb : AdminPage
#endif
    {

#if NET1
        #region 控件声明
        protected Discuz.Control.TextBox strDbName;
        protected Discuz.Control.TextBox size;
        protected Discuz.Control.Hint Hint1;
        protected Discuz.Control.Button ClearLog;
        protected Discuz.Control.Button ShrinkDB;
        #endregion
#endif

        protected void Page_Load(object sender, EventArgs e)
        {
            if (DbHelper.Provider.IsShrinkData() == true)
            {
                if (!Page.IsPostBack)
                {
                    if (!base.IsFounderUid(userid))
                    {
                        Response.Write(base.GetShowMessage());
                        Response.End();
                        return;
                    }
                    #region 绑定相关的数据库链接串属性

                    string connectionString = BaseConfigs.GetDBConnectString;
                    foreach (string info in connectionString.Split(';'))
                    {
                        if (info.ToLower().IndexOf("initial catalog") >= 0 || info.ToLower().IndexOf("database") >= 0)
                        {
                            strDbName.Text = info.Split('=')[1].Trim();
                            break;
                        }
                    }

                    #endregion
                }
            }
            else 
            {
                Response.Write("<script>alert('您所使用的数据库不支持此功能');</script>");
                Response.Write("<script>history.go(-1)</script>");
                Response.End();            
            }
        }

        public void ShrinkDateBase()
        {
            # region 收缩数据库函数

            try
            {
                string shrinksize = "";
                if (size.Text != "")
                {
                    shrinksize = size.Text;
                }
                else
                {
                    shrinksize = "0";
                }

                DatabaseProvider.GetInstance().ShrinkDataBase(shrinksize, strDbName.Text);
                base.RegisterStartupScript( "PAGE",  "window.location.href='global_logandshrinkdb.aspx';");
            }
            catch (Exception ex)
            {
                string message = ex.Message.Replace("'", " ");
                message = message.Replace("\\", "/");
                message = message.Replace("\r\n", "\\r\\n");
                message = message.Replace("\r", "\\r");
                message = message.Replace("\n", "\\n");

                base.RegisterStartupScript( "", "<script language=\"javascript\">alert('" + message + "!');window.location.href='global_logandshrinkdb.aspx';</script>");
            }

            #endregion
        }


        private void ClearLog_Click(object sender, EventArgs e)
        {
            #region 清除数据日志

            if (this.CheckCookie())
            {
                if (!base.IsFounderUid(userid))
                {
                    Response.Write(base.GetShowMessage());
                    Response.End();
                    return;
                }

                try
                {
                    Thread t = new Thread(new ThreadStart(ClearDBLog));
                    t.Start();
                    base.LoadRegisterStartupScript("PAGE", "window.location.href='global_logandshrinkdb.aspx';");
                }
                catch (Exception ex)
                {
                    string message = ex.Message.Replace("'", " ");
                    message = message.Replace("\\", "/");
                    message = message.Replace("\r\n", "\\r\\n");
                    message = message.Replace("\r", "\\r");
                    message = message.Replace("\n", "\\n");
                    base.RegisterStartupScript( "", "<script language=\"javascript\">alert('" + message + "!');window.location.href='global_logandshrinkdb.aspx';</script>");
                }
            }

            #endregion
        }

        public void ClearDBLog()
        {
            #region 线程清除日志
            DatabaseProvider.GetInstance().ClearDBLog(strDbName.Text);
            #endregion
        }

        private void ShrinkDB_Click(object sender, EventArgs e)
        {
            #region 收缩数据库

            if (this.CheckCookie())
            {
                if (!base.IsFounderUid(userid))
                {
                    Response.Write(base.GetShowMessage());
                    Response.End();
                    return;
                }

                Thread t = new Thread(new ThreadStart(ShrinkDateBase));
                t.Start();
                base.LoadRegisterStartupScript("PAGE", "window.location.href='global_logandshrinkdb.aspx';");
            }

            #endregion
        }

        #region Web 窗体设计器生成的代码

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.ClearLog.Click += new EventHandler(this.ClearLog_Click);
            this.ShrinkDB.Click += new EventHandler(this.ShrinkDB_Click);
            this.Load += new EventHandler(this.Page_Load);

            strDbName.IsReplaceInvertedComma = false;
        }

        #endregion

    }
}