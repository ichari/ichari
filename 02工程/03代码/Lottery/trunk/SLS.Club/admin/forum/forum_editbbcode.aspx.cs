using System;
using System.Data;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Collections;

using Discuz.Control;
using Discuz.Common;
using Discuz.Forum;
using Discuz.Config;
using Discuz.Data;


namespace Discuz.Web.Admin
{
    /// <summary>
    /// 编辑Discuz!NT代码
    /// </summary>
    
#if NET1
    public class editbbcode : AdminPage
#else
    public partial class editbbcode : AdminPage
#endif
    {

#if NET1
        #region 控件声明
        protected Discuz.Control.RadioButtonList available;
        protected Discuz.Control.UpFile icon;
        protected Discuz.Control.TextBox tag;
        protected Discuz.Web.Admin.TextareaResize replacement;
        protected Discuz.Web.Admin.TextareaResize example;
        protected Discuz.Web.Admin.TextareaResize explanation;
        protected Discuz.Control.TextBox param;
        protected Discuz.Control.TextBox nest;
        protected Discuz.Web.Admin.TextareaResize paramsdescript;
        protected Discuz.Web.Admin.TextareaResize paramsdefvalue;
        protected Discuz.Control.Hint Hint1;
        protected Discuz.Control.Button UpdateBBCodeInfo;
        protected Discuz.Control.Button DeleteBBCode;
        #endregion
#endif

        protected void Page_Load(object sender, EventArgs e)
        {

            if (DNTRequest.GetString("id") == "")
            {
                Response.Redirect("forum_bbcodegrid.aspx");
            }
            else
            {
                if (!IsPostBack)
                {
                    icon.UpFilePath = Server.MapPath(icon.UpFilePath);
                    LoadAnnounceInf(DNTRequest.GetInt("id", -1));
                }
            }
        }

        public void LoadAnnounceInf(int id)
        {
            #region 加载当前Discuz!NT代码相关信息

            //DataTable dt = DbHelper.ExecuteDataset("SELECT * FROM [" + BaseConfigs.GetTablePrefix + "bbcodes] WHERE [id]=" + id.ToString()).Tables[0];
            DataTable dt = DatabaseProvider.GetInstance().GetBBCode(id);
            if (dt.Rows.Count > 0)
            {
                available.SelectedValue = dt.Rows[0]["available"].ToString();
                tag.Text = dt.Rows[0]["tag"].ToString();
                replacement.Text = dt.Rows[0]["replacement"].ToString();
                example.Text = dt.Rows[0]["example"].ToString();
                explanation.Text = dt.Rows[0]["explanation"].ToString();
                paramsdescript.Text = dt.Rows[0]["paramsdescript"].ToString();
                paramsdefvalue.Text = dt.Rows[0]["paramsdefvalue"].ToString();
                nest.Text = dt.Rows[0]["nest"].ToString();
                param.Text = dt.Rows[0]["params"].ToString();
                icon.Text = dt.Rows[0]["icon"].ToString();
                ViewState["inco"] = dt.Rows[0]["icon"].ToString();
            }

            #endregion
        }

        private void UpdateBBCodeInfo_Click(object sender, EventArgs e)
        {
            #region 更新当前Discuz!NT代码信息

            if (this.CheckCookie())
            {

                //string inco = icon.UpdateFile();
                //string sqlstring = string.Format("UPDATE [" + BaseConfigs.GetTablePrefix + "bbcodes] SET [available]='{1}',tag='{2}', icon='{3}',replacement='{4}',example='{5}',explanation='{6}',params='{7}',nest='{8}',paramsdescript='{9}',paramsdefvalue='{10}'  WHERE [id]={0}",
                //    DNTRequest.GetString("id"),
                //    available.SelectedValue,
                //    Regex.Replace(tag.Text.Replace("<", "").Replace(">", ""), @"^[\>]|[\{]|[\}]|[\[]|[\]]|[\']|[\.]", ""),
                //    inco != "" ? inco : ViewState["inco"].ToString(),
                //    replacement.Text,
                //    example.Text,
                //    explanation.Text,
                //    param.Text,
                //    nest.Text,
                //    paramsdescript.Text,
                //    paramsdefvalue.Text);

                //DbHelper.ExecuteNonQuery(sqlstring);
                SortedList sl = new SortedList();
                sl.Add("参数个数", param.Text);
                sl.Add("嵌套次数", nest.Text);
                

                foreach (DictionaryEntry s in sl)
                {
                    if (!Utils.IsInt(s.Value.ToString()))
                    {
                        base.RegisterStartupScript("", "<script>alert('输入错误:" + s.Key.ToString() + ",只能是0或者正整数');window.location.href='forum_editbbcode.aspx';</script>");
                        return;
                    }
                }
                string filepath = icon.UpdateFile();
                if (filepath=="")
                {
                 filepath = ViewState["inco"].ToString();
                }
              
                

                DatabaseProvider.GetInstance().UpdateBBCCode(
                    int.Parse(available.SelectedValue),
                    Regex.Replace(tag.Text.Replace("<", "").Replace(">", ""), @"^[\>]|[\{]|[\}]|[\[]|[\]]|[\']|[\.]", ""),
                    filepath,
                    replacement.Text,
                    example.Text,
                    explanation.Text,
                    param.Text,
                    nest.Text,
                    paramsdescript.Text,
                    paramsdefvalue.Text,
                    DNTRequest.GetInt("id", 0)
                    );

                AdminVistLogs.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "更新Discuz!NT代码", "TAB为:" + tag.Text);
                AdminCaches.ReSetCustomEditButtonList();
                base.RegisterStartupScript( "PAGE", "window.location.href='forum_bbcodegrid.aspx';");
            }

            #endregion
        }

        private void DeleteBBCode_Click(object sender, EventArgs e)
        {
            #region 删除当前Discuz!NT代码信息

            if (this.CheckCookie())
            {
                //DbHelper.ExecuteNonQuery("DELETE FROM [" + BaseConfigs.GetTablePrefix + "bbcodes] WHERE [id]=" + DNTRequest.GetString("id"));
                DatabaseProvider.GetInstance().DeleteBBCode(DNTRequest.GetString("id"));
                AdminVistLogs.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "删除Discuz!NT代码", "TAB为:" + tag.Text);
                AdminCaches.ReSetCustomEditButtonList();
                base.RegisterStartupScript( "PAGE", "window.location.href='forum_bbcodegrid.aspx';");
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
            this.UpdateBBCodeInfo.Click += new EventHandler(this.UpdateBBCodeInfo_Click);
            this.DeleteBBCode.Click += new EventHandler(this.DeleteBBCode_Click);
        }

        #endregion
    }
}