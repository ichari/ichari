using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.IO;

using Discuz.Forum;
using Discuz.Config;
using Discuz.Common;
using Discuz.Data;

using Discuz.Cache;

namespace Discuz.Web.Admin
{

#if NET1
    public class smiliemanage : AdminPage
#else
    public partial class smiliemanage : AdminPage
#endif
    {

#if NET1
        #region 控件声明
        protected Discuz.Control.DataGrid smilesgrid;
        protected System.Web.UI.WebControls.Literal dirinfoList;
        protected Discuz.Control.Button SubmitButton;
		protected Discuz.Control.Button  SaveSmiles;
        #endregion
#endif

        private ArrayList dirList = new ArrayList();

        private void Page_Load(object sender, System.EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                SmilesGridBind();
            }
            SmilesListBind();
        }

        private void SmilesGridBind()
        {
            #region 绑定数据
            string emptySmilieList = Smilies.ClearEmptySmilieType();
            DataTable dt = Smilies.GetSmilieTypes();
            DirectoryInfo[] dirInfo = GetSmilesDirList();
            foreach (DirectoryInfo dir in dirInfo)
            {
                dirList.Add(dir.Name);
            }
            string d = "";
            foreach (DataRow dr in dt.Rows)
            {
                dirList.Remove(dr["url"]);
                d += dr["code"].ToString() + ",";
            }
            smilesgrid.TableHeaderName = "论坛表情列表";
            smilesgrid.BindData(DatabaseProvider.GetInstance().GetSmilies());
            ViewState["dir"] = d;
            ViewState["dirList"] = dirList;
            if (emptySmilieList != "")
            {
                base.RegisterStartupScript("", "<script>alert('" + emptySmilieList + " 为空,已经被移除!');</script>");
            }
            #endregion
        }

        private DirectoryInfo[] GetSmilesDirList()
        {
            #region 获取表情下所有文件夹
            string path = AppDomain.CurrentDomain.BaseDirectory + "editor/images/smilies";
            DirectoryInfo dirinfo = new DirectoryInfo(path);
            return dirinfo.GetDirectories();
            #endregion
        }

        private void SmilesListBind()
        {
            #region 绑定表情文件内的内容
            dirinfoList.Text = "";
            dirList = (ArrayList)ViewState["dirList"];
            if (dirList.Count == 0)
            {
                SubmitButton.Visible = false;
            }
            else
            {
                int i = 1;
                foreach (string dir in dirList)
                {
                    dirinfoList.Text += "<tr class='mouseoutstyle' onmouseover='this.className=\"mouseoverstyle\"' onmouseout='this.className=\"mouseoutstyle\"' >\n";
                    dirinfoList.Text += "<td nowrap='nowrap' style='border: 1px solid rgb(234, 233, 225); width: 20px;'><input type='checkbox' id='id" + i + "' name='id" + i + "' value='" + i + "'/></td>\n";
                    dirinfoList.Text += "<td align='center' nowrap='nowrap' style='border-color:#EAE9E1;border-width:1px;border-style:solid;'><input type='text' id='group" + i + "' name='group" + i + "' value='" + dir + "' class=\"FormBase\" onfocus=\"this.className='FormFocus';\" onblur=\"this.className='FormBase';\" /></td>\n";
                    dirinfoList.Text += "<td align='center' nowrap='nowrap' style='border-color:#EAE9E1;border-width:1px;border-style:solid;'><input type='text' id='order" + i + "' name='order" + i + "' value='" + i + "' class=\"FormBase\" onfocus=\"this.className='FormFocus';\" onblur=\"this.className='FormBase';\" /></td>\n";
                    dirinfoList.Text += "<td align='center' nowrap='nowrap' style='border-color:#EAE9E1;border-width:1px;border-style:solid;'><input type='hidden' name='url" + i + "' value='" + dir + "' />" + dir + "</td>\n";
                    dirinfoList.Text += "</tr>\n";
                    i++;
                }
            }
            #endregion
        }

        public void SubmitButton_Click(object sender, EventArgs e)
        {
            #region 提交所选的表情分类
            for (int i = 1; i <= dirList.Count; i++)
            {
                if (DNTRequest.GetFormString("id" + i) != null && DNTRequest.GetFormString("id" + i) != "")
                {
                    DatabaseProvider.GetInstance().AddSmiles(DatabaseProvider.GetInstance().GetMaxSmiliesId(),
                                                 DNTRequest.GetInt("order" + i,0),
                                                 0,
                                                 DNTRequest.GetFormString("group" + i),
                                                 DNTRequest.GetFormString("url" + i));
                    //将新增表情分类中的表情入库
                    int maxSmilieId = DatabaseProvider.GetInstance().GetMaxSmiliesId() - 1;
                    int order = 1;
                    string url = DNTRequest.GetFormString("url" + i);
                    ArrayList fileList = GetSmilesFileList(DNTRequest.GetFormString("url" + i));
                    foreach (string file in fileList)
                    {
                        if (file.ToLower() == "thumbs.db")  //过滤掉thumbs.db文件
                            continue;
                        DatabaseProvider.GetInstance().AddSmiles(DatabaseProvider.GetInstance().GetMaxSmiliesId(), 
                                                order,
                                                maxSmilieId,
                                                "[:em" + file.Substring(0,file.Length-4) + ":]",
                                                url + "/" + file);
                        order++;
                    }
                }
            }
            UpdateSmiliesCache();
            base.RegisterStartupScript( "", "<script>window.location.href='forum_smiliemanage.aspx';</script>");
            #endregion
        }

        private ArrayList GetSmilesFileList(string smilesPath)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "editor/images/smilies/" + smilesPath;
            DirectoryInfo dir = new DirectoryInfo(path);
            if (!dir.Exists)
            {
                throw new IOException("分类文件夹不存在!");
            }
            FileInfo[] files = dir.GetFiles();
            ArrayList temp = new ArrayList();
            foreach (FileInfo file in files)
            {
                temp.Add(file.Name);
            }
            return temp;
        }

        private void SaveSmiles_Click(object sender, EventArgs e)
       {
           #region 保存对表情信息的编辑
           if (this.CheckCookie())
            {
                int rowid = -1;
                bool error = false;
                foreach (object o in smilesgrid.GetKeyIDArray())
                {
                    string id = o.ToString();
                    string code = smilesgrid.GetControlValue(rowid, "code");
                    string displayorder = smilesgrid.GetControlValue(rowid, "displayorder");
                    if (code == "" || !Utils.IsNumeric(displayorder) || DatabaseProvider.GetInstance().IsExistSmilieCode(code, int.Parse(id)))
                    {
                        error = true;
                        continue;
                    }
                    DatabaseProvider.GetInstance().UpdateSmiliesPart(code, int.Parse(displayorder), int.Parse(id));
                    rowid++;
                }
                UpdateSmiliesCache();
                if(error)
                    base.RegisterStartupScript("", "<script>alert('某些记录输入不完整或数据库中已存在相同的表情组名称');window.location.href='forum_smiliemanage.aspx';</script>");
                else
                    base.RegisterStartupScript("", "<script>window.location.href='forum_smiliemanage.aspx';</script>");
            }
           #endregion
        }

        protected void DelRec_Click(object sender, EventArgs e)
        {
            #region 删除所选的表情分类
            if (this.CheckCookie())
            {
                string idlist = DNTRequest.GetString("id");
                foreach (string id in idlist.Split(','))
                {
                    smilesgrid.DeleteByString(DatabaseProvider.GetInstance().DeleteSmily(int.Parse(id)));
                    DatabaseProvider.GetInstance().DeleteSmilyByType(int.Parse(id));
                }
                UpdateSmiliesCache();
                base.RegisterStartupScript("", "<script>window.location.href='forum_smiliemanage.aspx';</script>");
            }
            #endregion
        }
        
        private void UpdateSmiliesCache()
        {
            #region 更新表情缓存
            DNTCache.GetCacheService().RemoveObject("/Forum/UI/SmiliesList");
            DNTCache.GetCacheService().RemoveObject("/Forum/UI/SmiliesListFirstPage");
            DNTCache.GetCacheService().RemoveObject("/Forum/UI/SmiliesListWithInfo");
            #endregion
        }

        #region Web 窗体设计器生成的代码
        override protected void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.SubmitButton.Click += new EventHandler(this.SubmitButton_Click);
            this.SubmitButton.Attributes.Add("onclick", "return validate()");
            this.SaveSmiles.Click += new EventHandler(this.SaveSmiles_Click);
            smilesgrid.ColumnSpan = 5;
            smilesgrid.AllowPaging = false;
        }
        #endregion
    }
}
