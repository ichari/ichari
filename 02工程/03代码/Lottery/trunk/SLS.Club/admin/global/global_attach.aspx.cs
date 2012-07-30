using System;
using System.Drawing;
using System.Drawing.Text;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Collections;

using Discuz.Common;
using Discuz.Forum;
using Discuz.Config;


namespace Discuz.Web.Admin
{
    /// <summary>
    /// 附件设置. 
    /// </summary> 
    
#if NET1
    public class attach : AdminPage
#else
    public partial class attach : AdminPage
#endif
    {

#if NET1
        #region 控件声明
        protected Discuz.Control.RadioButtonList attachimgpost;
        protected Discuz.Control.RadioButtonList attachrefcheck;
        protected Discuz.Control.RadioButtonList attachsave;
        protected Discuz.Control.RadioButtonList watermarktype;
        protected Discuz.Control.TextBox watermarktext;
        protected Discuz.Control.TextBox watermarkpic;
        protected Discuz.Control.DropDownList watermarkfontname;
        protected Discuz.Control.TextBox watermarkfontsize;
        protected Discuz.Control.RadioButtonList showattachmentpath;
        protected Discuz.Control.TextBox attachimgquality;
        protected Discuz.Control.TextBox attachimgmaxheight;
        protected Discuz.Control.TextBox attachimgmaxwidth;
        protected Discuz.Control.TextBox watermarktransparency;
        protected Discuz.Control.Button SaveInfo;

        protected System.Web.UI.WebControls.Literal position;
        #endregion
#endif



        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                LoadConfigInfo();
            }
        }

        public void LoadConfigInfo()
        {
            #region 加载配置信息

            GeneralConfigInfo __configinfo = GeneralConfigs.Deserialize(Server.MapPath("../../config/general.config"));
            attachrefcheck.SelectedValue = __configinfo.Attachrefcheck.ToString();
            attachsave.SelectedValue = __configinfo.Attachsave.ToString();
            attachimgpost.SelectedValue = __configinfo.Attachimgpost.ToString();
            watermarktype.SelectedValue = __configinfo.Watermarktype.ToString();
            showattachmentpath.SelectedValue = __configinfo.Showattachmentpath.ToString();
            attachimgmaxheight.Text = __configinfo.Attachimgmaxheight.ToString();
            attachimgmaxwidth.Text = __configinfo.Attachimgmaxwidth.ToString();
            attachimgquality.Text = __configinfo.Attachimgquality.ToString();
            watermarkfontsize.Text = __configinfo.Watermarkfontsize.ToString();
            watermarktext.Text = __configinfo.Watermarktext.ToString();
            watermarkpic.Text = __configinfo.Watermarkpic.ToString();
            watermarktransparency.Text = __configinfo.Watermarktransparency.ToString();

            LoadPosition(__configinfo.Watermarkstatus);

            LoadSystemFont();

            try
            {
                watermarkfontname.SelectedValue = __configinfo.Watermarkfontname.ToString();
            }
            catch
            {
                watermarkfontname.SelectedIndex = 0;
            }

            #endregion
        }

        private void LoadSystemFont()
        {
            #region 加载系统字体
            watermarkfontname.Items.Clear();
            InstalledFontCollection fonts = new InstalledFontCollection();
            foreach (FontFamily family in fonts.Families)
            {
                watermarkfontname.Items.Add(new ListItem(family.Name, family.Name));
            }

            #endregion
        }

        public void LoadPosition(int selectid)
        {
            #region 加载水印设置界面

            position.Text = "<table width=\"256\" height=\"207\" border=\"0\" background=\"../images/flower.jpg\">";
            for (int i = 1; i < 10; i++)
            {
                if (i % 3 == 1) position.Text += "<tr>";
                if (selectid == i)
                {
                    position.Text += "<td width=\"33%\" align=\"center\" style=\"vertical-align:middle;\"><input type=\"radio\" id=\"watermarkstatus\" name=\"watermarkstatus\" value=\"" + i + "\" checked><b>#" + i + "</b></td>";
                }
                else
                {
                    position.Text += "<td width=\"33%\" align=\"center\" style=\"vertical-align:middle;\"><input type=\"radio\" id=\"watermarkstatus\" name=\"watermarkstatus\" value=\"" + i + "\" ><b>#" + i + "</b></td>";
                }
                if (i % 3 == 0) position.Text += "</tr>";

            }

            position.Text += "</table><input type=\"radio\" id=\"watermarkstatus\" name=\"watermarkstatus\" value=\"0\" ";
            if (selectid == 0)
            {
                position.Text += " checked";
            }
            position.Text += ">不启用水印功能";

            #endregion
        }

        private void SaveInfo_Click(object sender, EventArgs e)
        {
            #region 保存设置信息

            if (this.CheckCookie())
            {

                Hashtable ht = new Hashtable();
                ht.Add("图片附件文字水印大小", watermarkfontsize.Text);
                ht.Add("JPG图片质量", attachimgquality.Text);
                ht.Add("图片最大高度", attachimgmaxheight.Text);
                ht.Add("图片最大宽度", attachimgmaxwidth.Text);
               
                foreach (DictionaryEntry de in ht)
                {
                    if (!Utils.IsInt(de.Value.ToString()))
                    {
                        base.RegisterStartupScript("", "<script>alert('输入错误," + de.Key.ToString() + "只能是0或者正整数');window.location.href='global_attach.aspx';</script>");
                        return;
                    }
                
                }

               

                if (Convert.ToInt16(attachimgquality.Text) > 100 || (Convert.ToInt16(attachimgquality.Text) < 0))
                {
                    base.RegisterStartupScript( "", "<script>alert('JPG图片质量只能在0-100之间');window.location.href='global_attach.aspx';</script>");
                    return;
                }

                if (Convert.ToInt16(watermarktransparency.Text) > 10 || Convert.ToInt16(watermarktransparency.Text) < 1)
                {
                    base.RegisterStartupScript( "", "<script>alert('图片水印透明度取值范围1-10');window.location.href='global_attach.aspx';</script>");
                    return;
                }

                if (Convert.ToInt16(watermarkfontsize.Text) <= 0)
                {
                    base.RegisterStartupScript( "", "<script>alert('图片附件添加文字水印的大小必须大于0');window.location.href='global_attach.aspx';</script>");
                    return;
                }


                if (Convert.ToInt16(attachimgmaxheight.Text) < 0)
                {
                    base.RegisterStartupScript( "", "<script>alert('图片最大高度必须大于或等于0');window.location.href='global_attach.aspx';</script>");
                    return;
                }

                if (Convert.ToInt16(attachimgmaxwidth.Text) < 0)
                {
                    base.RegisterStartupScript( "", "<script>alert('图片最大宽度必须大于或等于0');window.location.href='global_attach.aspx';</script>");
                    return;
                }


                GeneralConfigInfo __configinfo = GeneralConfigs.Deserialize(Server.MapPath("../../config/general.config"));
                __configinfo.Attachrefcheck = Convert.ToInt32(attachrefcheck.SelectedValue);
                __configinfo.Attachsave = Convert.ToInt32(attachsave.SelectedValue);
                __configinfo.Watermarkstatus = DNTRequest.GetInt("watermarkstatus", 0);
                __configinfo.Attachimgpost = Convert.ToInt32(attachimgpost.SelectedValue);
                __configinfo.Watermarktype = Convert.ToInt16(watermarktype.SelectedValue);
                __configinfo.Showattachmentpath = Convert.ToInt32(showattachmentpath.SelectedValue);
                __configinfo.Attachimgmaxheight = Convert.ToInt32(attachimgmaxheight.Text);
                __configinfo.Attachimgmaxwidth = Convert.ToInt32(attachimgmaxwidth.Text);
                __configinfo.Attachimgquality = Convert.ToInt32(attachimgquality.Text);
                __configinfo.Watermarktext = watermarktext.Text;
                __configinfo.Watermarkpic = watermarkpic.Text;
                __configinfo.Watermarkfontname = watermarkfontname.SelectedValue;
                __configinfo.Watermarkfontsize = Convert.ToInt32(watermarkfontsize.Text);
                __configinfo.Watermarktransparency = Convert.ToInt16(watermarktransparency.Text);

                GeneralConfigs.Serialiaze(__configinfo, Server.MapPath("../../config/general.config"));

                AdminVistLogs.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "附件设置", "");
                base.RegisterStartupScript( "PAGE", "window.location.href='global_attach.aspx';");
            }

            #endregion
        }

        #region 把VIEWSTATE写入容器

        protected override void SavePageStateToPersistenceMedium(object viewState)
        {
            base.DiscuzForumSavePageState(viewState);
        }

        protected override object LoadPageStateFromPersistenceMedium()
        {
            return base.DiscuzForumLoadPageState();
        }

        #endregion

        #region Web 窗体设计器生成的代码

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.SaveInfo.Click += new EventHandler(this.SaveInfo_Click);
            this.Load += new EventHandler(this.Page_Load);

        }

        #endregion
    }
}