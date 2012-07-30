using System;
using System.Data;
using System.IO;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Text;

using Discuz.Config;
using Discuz.Control;
using Discuz.Common;
using Discuz.Forum;

using Button = Discuz.Control.Button;
using CheckBoxList = Discuz.Control.CheckBoxList;
using Repeater = Discuz.Control.Repeater;

namespace Discuz.Web.Admin
{
    /// <summary>
    /// 模板生成页面
    /// </summary>
    
#if NET1
    public class templatetree : AdminPage
#else
    public partial class templatetree : AdminPage
#endif
    {

#if NET1
        #region 控件声明
        protected Discuz.Control.TabControl TabControl1;
        protected Discuz.Control.TabPage tabPage22;
        protected Discuz.Control.CheckBoxList TreeView1;
        protected Discuz.Control.Button CreateTemplate;
        protected Discuz.Control.Button DeleteTemplateFile;
        protected Discuz.Control.TabPage tabPage33;
        protected Discuz.Control.Repeater TreeView2;
        protected System.Web.UI.WebControls.Label lblClientSideCheck;
        protected System.Web.UI.WebControls.Label lblCheckedNodes;
        protected System.Web.UI.WebControls.Label lblServerSideCheck;
        #endregion
#endif


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            { }
        }

        private DataTable LoadTemplateFileDT()
        {
            #region 装入模板文件
            DataTable templatefilelist = new DataTable("templatefilelist");

            templatefilelist.Columns.Add("fullfilename", Type.GetType("System.String"));
            templatefilelist.Columns.Add("filename", Type.GetType("System.String"));
            templatefilelist.Columns.Add("id", Type.GetType("System.Int32"));
            templatefilelist.Columns.Add("extension", Type.GetType("System.String"));
            templatefilelist.Columns.Add("parentid", Type.GetType("System.String"));
            templatefilelist.Columns.Add("filepath", Type.GetType("System.String"));
            templatefilelist.Columns.Add("filedescription", Type.GetType("System.String"));

            string path = DNTRequest.GetString("path");
            DirectoryInfo dirinfo = new DirectoryInfo(Server.MapPath("../../templates/" + path));
            int i = 1;
            string extname;
            foreach (FileSystemInfo file in dirinfo.GetFileSystemInfos())
            {
                if (file != null)
                {
                    extname = file.Extension.ToLower();
                    if (extname == ".htm" || extname == ".config")
                    {
                        DataRow dr = templatefilelist.NewRow();

                        dr["id"] = i;
                        dr["filename"] = file.Name.Substring(0, file.Name.LastIndexOf("."));
                        if(extname == ".htm")
                            dr["fullfilename"] = path + "\\" + dr["filename"] + ".htm";
                        else
                            dr["fullfilename"] = path + "\\" + dr["filename"] + ".config";
                        dr["extension"] = file.Extension.ToLower();
                        dr["parentid"] = "0";
                        dr["filepath"] = path;
                        dr["filedescription"] = "";
                        templatefilelist.Rows.Add(dr);
                        i++;
                    }
                }
            }

            foreach (DataRow dr in templatefilelist.Rows)
            {
                foreach (DataRow childdr in templatefilelist.Select("filename like '" + dr["filename"].ToString() + "_%%'"))
                {
                    if (dr["filename"].ToString() != childdr["filename"].ToString())
                    {
                        childdr["parentid"] = dr["id"].ToString();
                    }
                }
            }

            return templatefilelist;
            #endregion
        }


        private DataTable LoadOtherFileDT()
        {
            #region 装入其它文件
            DataTable otherfilelist = new DataTable("otherfilelist");

            otherfilelist.Columns.Add("id", Type.GetType("System.Int32"));
            otherfilelist.Columns.Add("filename", Type.GetType("System.String"));
            otherfilelist.Columns.Add("extension", Type.GetType("System.String"));
            otherfilelist.Columns.Add("parentid", Type.GetType("System.String"));
            otherfilelist.Columns.Add("filepath", Type.GetType("System.String"));
            otherfilelist.Columns.Add("filedescription", Type.GetType("System.String"));

            string path = DNTRequest.GetString("path");
            DirectoryInfo dirinfo = new DirectoryInfo(Server.MapPath("../../templates/" + path));
            int i = 1;
            string extname;
            foreach (FileSystemInfo file in dirinfo.GetFileSystemInfos())
            {
                if (file != null)
                {
                    extname = file.Extension.ToLower();
                    if (extname.IndexOf("js") > 0 || extname.IndexOf("css") > 0 || extname.IndexOf("xml") > 0 || extname.IndexOf(".as") > 0 || extname.IndexOf(".html") > 0)
                    {
                        DataRow dr = otherfilelist.NewRow();

                        dr["id"] = i;
                        dr["filename"] = file.Name.Substring(0, file.Name.LastIndexOf("."));
                        dr["extension"] = file.Extension.ToLower();
                        dr["parentid"] = "0";
                        dr["filepath"] = path;
                        dr["filedescription"] = "";
                        otherfilelist.Rows.Add(dr);
                        i++;
                    }
                }
            }

            foreach (DataRow dr in otherfilelist.Rows)
            {
                foreach (DataRow childdr in otherfilelist.Select("filename like '" + dr["filename"].ToString() + "_%%'"))
                {
                    if (dr["filename"].ToString() != childdr["filename"].ToString())
                    {
                        childdr["parentid"] = dr["id"].ToString();
                    }
                }
            }

            foreach (DataRow dr in otherfilelist.Rows)
            {
                //string imgstr = "";
                //if (dr["extension"].ToString().IndexOf("js") > 0) imgstr = "../images/js.gif";
                //if (dr["extension"].ToString().IndexOf("xml") > 0) imgstr = "../images/xml.gif";
                //if (dr["extension"].ToString().IndexOf("css") > 0) imgstr = "../images/css.gif";
                //if (dr["extension"].ToString().IndexOf("aspx") > 0) imgstr = "../images/aspx.gif";
                //if (dr["extension"].ToString().IndexOf("ascx") > 0) imgstr = "../images/ascx.gif";
                string ext = dr["extension"].ToString().Substring(1);
                dr["filename"] = "<img src=\"../images/" + ext + ".gif\" border=\"0\"> <a href=\"global_templatesedit.aspx?path=" + dr["filepath"].ToString().Replace(" ", "%20") + "&filename=" + dr["filename"].ToString() + dr["extension"].ToString() + "&templateid=" + DNTRequest.GetString("templateid") + "&templatename=" + DNTRequest.GetString("templatename").Replace(" ", "%20") + "\" title=\"" + ext + "文件\">" + dr["filename"].ToString().Trim() + "</a>";
            }

            return otherfilelist;
            #endregion
        }

        private void DeleteTemplateFile_Click(object sender, EventArgs e)
        {
            #region 删除模板文件
            if (this.CheckCookie())
            {
                string templatepathlist = TreeView1.GetSelectString(",");

                if (templatepathlist == "")
                {
                    base.RegisterStartupScript( "", "<script>alert('您未选中任何模板');</script>");
                    return;
                }

                foreach (string templatepath in templatepathlist.Split(','))
                {
                    DeleteFile(templatepath);
                }
                base.RegisterStartupScript( "PAGE", "window.location.href='global_templatetree.aspx?templateid=" + Request.Params["templateid"] + "&path=" + Request.Params["path"] + "&templatename=" + Request.Params["templatename"] + "';");
            }
            #endregion
        }


        public bool DeleteFile(string filename)
        {
            #region 删除文件
            if (Utils.FileExists(Utils.GetMapPath(@"..\..\templates\" + filename)))
            {
                File.Delete(Utils.GetMapPath(@"..\..\templates\" + filename));
                return true;
            }
            return false;
            #endregion
        }


        private void CreateTemplate_Click(object sender, EventArgs e)
        {
            #region 建立文件
            if (this.CheckCookie())
            {
                string templatepathlist = TreeView1.GetSelectString(",");

                if (templatepathlist == "")
                {
                    base.RegisterStartupScript( "", "<script>alert('您未选中任何模板');</script>");
                    return;
                }
                if (DNTRequest.GetString("chkall") == "")   //非全部生成
                {
                    templatepathlist = RemadeTemplatePathList(templatepathlist);
                }
                int templateid = DNTRequest.GetInt("templateid", 1);
                string skinname = "";
                string templateName = "";
                string[] tempstr;
                int updatecount = 0;
                ForumPageTemplate forumpagetemplate = new ForumPageTemplate();

                foreach (string templatepath in templatepathlist.Split(','))
                {
                    tempstr = templatepath.Split('\\');
                    skinname = tempstr[0];
                    templateName = tempstr[tempstr.Length - 1];
                    tempstr = templateName.Split('.');
                    if ((tempstr[tempstr.Length - 1].ToLower().Equals("htm") || (tempstr[tempstr.Length - 1].ToLower().Equals("config"))) && (templateName.IndexOf("_") != 0))
                    {
                        templateName = tempstr[0];
                        forumpagetemplate.GetTemplate("/",skinname, templateName, 1, templateid);
                        updatecount++;
                    }
                }
                //用cookies保存最近常生成的模板
                //templatepathlist = templatepathlist.Replace(DNTRequest.GetString("templatename") + "\\", "");
                //System.Web.HttpCookie commontemplate = new System.Web.HttpCookie("commontemplate");
                //if (templatepathlist.Split(',').Length > 5)
                //{
                //    string commontemplatevalue = "";
                //    string[] templatepathlistarray = templatepathlist.Split(',');
                //    for (int i = 0; i < 5; i++)
                //    {
                //        commontemplatevalue += templatepathlistarray[i] + ",";
                //    }
                //    commontemplate.Value = commontemplatevalue.TrimEnd(',');
                //}
                //else
                //{
                //    commontemplate.Value = templatepathlist;
                //}
                //commontemplate.Expires = DateTime.Now.AddYears(1);
                //Response.AppendCookie(commontemplate);


                base.RegisterStartupScript( "PAGETemplate", "共" + updatecount + " 个模板已更新");
                //base.CallBaseRegisterStartupScript("form1", "<script>window.location.href='global_templatetree.aspx?templateid=" + Request.Params["templateid"] + "&path=" + Request.Params["path"].Replace(" ", "%20") + "&templatename=" + Request.Params["templatename"].Replace(" ", "%20") + "';</script>");
            }
            #endregion
        }
        /// <summary>
        /// 重新生成包含有头文件的文件列表
        /// </summary>
        /// <param name="templatepathlist">已经选择的文件列表</param>
        /// <returns>返回处理完毕的文件列表</returns>
        private string RemadeTemplatePathList(string templatepathlist)
        {
            #region 生成头文件的列表文件
            string result = templatepathlist = templatepathlist.ToLower();
            if (result.IndexOf("\\_") == -1)  //列表中如果没有头文件则直接返回
            {
                return result;
            }
            foreach (string templatepath in templatepathlist.Split(','))
            {
                string[] tempstr = templatepath.Split('\\');
                string skinname = tempstr[0];
                string templateName = tempstr[tempstr.Length - 1];
                tempstr = templateName.Split('.');
                if (!tempstr[tempstr.Length - 1].ToLower().Equals("htm")) continue;  //非模板文件继续
                if (!templateName.StartsWith("_")) continue;   //非头文件继续处理下个文件
                DirectoryInfo dirinfo = new DirectoryInfo(Server.MapPath("../../templates/" + skinname));
                foreach (FileSystemInfo file in dirinfo.GetFileSystemInfos("*.htm"))
                {
                    if (file.Name.StartsWith("_")) continue; //如果是头文件则不处理
                    using (StreamReader objReader = new StreamReader(Server.MapPath("../../templates/" + skinname + "/" + file.Name), Encoding.UTF8))
                    {
                        if (objReader.ReadToEnd().IndexOf("<%template " + tempstr[0] + "%>") != -1)  //找到包含该头文件的文件
                        {
                            if (result.IndexOf(file.Name) == -1)    //如果当前文件未在列表中，则包含入当前列表
                            {
                                result += "," + skinname + "\\" + file.Name;
                            }
                        }
                        objReader.Close();
                    }
                }
            }
            return result;
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
            this.CreateTemplate.Click += new EventHandler(this.CreateTemplate_Click);
            this.DeleteTemplateFile.Click += new EventHandler(this.DeleteTemplateFile_Click);
            this.Load += new EventHandler(this.Page_Load);

            DataTable dt = LoadTemplateFileDT();
            foreach (DataRow dr in dt.Rows)
            {
                string ext = dr["extension"].ToString().Substring(1);
                dr["filename"] = "<img src=../images/" + ext + ".gif border=\"0\"  style=\"position:relative;top:5 px;height:16 px\"> " + dr["filename"].ToString().Trim() 
                    + " <a href=\"global_templatesedit.aspx?path=" + dr["filepath"].ToString().Replace(" ", "%20") + "&filename=" + dr["filename"].ToString() 
                    + dr["extension"].ToString() + "&templateid=" + DNTRequest.GetString("templateid") + "&templatename=" + DNTRequest.GetString("templatename").Replace(" ", "%20") 
                    + "\" title=\"编辑" + dr["filename"].ToString().Trim() + "." + ext + "模板文件\"><img src='../images/editfile.gif' border='0'/></a>";
            }
            TreeView1.AddTableData(dt);
            for (int i = 0; i < TreeView1.Items.Count; i++)
            {
                TreeView1.Items[i].Attributes.Add("onclick", "checkedEnabledButton1(form,'TabControl1:tabPage22:CreateTemplate','TabControl1:tabPage22:DeleteTemplateFile')");
                TreeView1.Items[i].Attributes.Add("value",TreeView1.Items[i].Value);
            }

            TreeView2.DataSource = LoadOtherFileDT();
            TreeView2.DataBind();
        }

        #endregion
    }
}