using System;
using System.Data;
using System.Web.UI.WebControls;
using System.Web.UI;

using Discuz.Control;
using Discuz.Common;
using Discuz.Forum;
using Button = Discuz.Control.Button;
using DropDownList = Discuz.Control.DropDownList;
using RadioButtonList = Discuz.Control.RadioButtonList;
using TextBox = Discuz.Control.TextBox;
using Discuz.Config;
using Discuz.Data;


namespace Discuz.Web.Admin
{
    /// <summary>
    /// 编辑广告
    /// </summary>
    
#if NET1
    public class editadvs : AdminPage
#else
    public partial class editadvs : AdminPage
#endif
    {

#if NET1
        #region 控件声明
        protected Discuz.Control.RadioButtonList available;
        protected Discuz.Control.DropDownList type;
        protected Discuz.Control.DropDownList parameters;
        protected Discuz.Control.TextBox displayorder;
        protected Discuz.Control.Calendar starttime;
        protected Discuz.Control.Calendar endtime;
        protected Discuz.Control.TextBox title;
        protected Discuz.Web.Admin.forumtree TargetFID;
        protected Discuz.Control.RadioButtonList inpostposition;
        protected Discuz.Control.ListBox inpostfloor;
        protected Discuz.Control.TextBox code;
        protected Discuz.Control.TextBox wordcontent;
        protected Discuz.Control.TextBox wordfont;
        protected Discuz.Control.TextBox wordlink;
        protected Discuz.Control.TextBox imgsrc;
        protected Discuz.Control.TextBox imglink;
        protected Discuz.Control.TextBox imgwidth;
        protected Discuz.Control.TextBox imgheight;
        protected Discuz.Control.TextBox imgtitle;
        protected Discuz.Control.TextBox flashwidth;
        protected Discuz.Control.TextBox flashheight;
        protected Discuz.Control.TextBox flashsrc;
        protected Discuz.Control.TextBox slwmvsrc;
        protected Discuz.Control.TextBox slimage;
        protected Discuz.Control.TextBox buttomimg;
        protected Discuz.Control.TextBox words1;
        protected Discuz.Control.TextBox words2;
        protected Discuz.Control.TextBox words3;
        protected Discuz.Control.Hint Hint1;
        protected Discuz.Control.Button UpdateADInfo;
        protected Discuz.Control.Button DeleteADInfo;
        #endregion
#endif


        #region 控件声明


        protected Button AddAdInfo;
        protected Literal msg;

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                
            }
        }


        /// <summary>
        /// 编辑广告绑定
        /// </summary>
        /// <param name="advid">广告ID</param>
        public void LoadAnnounceInf(int advid)
        {
            #region 加载相关广告信息
            GeneralConfigInfo __configinfo = GeneralConfigs.Deserialize(Server.MapPath("../../config/general.config"));
            for (int i = 1; i <= __configinfo.Ppp; i++)
            {
                inpostfloor.Items.Add(new ListItem(" >#" + i, i.ToString()));
            }
            DataTable dt = DatabaseProvider.GetInstance().GetAdvertisement(advid);
            if (dt.Rows.Count > 0)
            {
                displayorder.Text = dt.Rows[0]["displayorder"].ToString();
                available.SelectedValue = dt.Rows[0]["available"].ToString();
                type.SelectedValue = dt.Rows[0]["type"].ToString().Trim();
                tbtitle.Text = dt.Rows[0]["title"].ToString();

                //绑定广告有效的开始日期
                if (dt.Rows[0]["starttime"].ToString().IndexOf("1900") < 0)
                {
                    starttime.SelectedDate = Convert.ToDateTime(dt.Rows[0]["starttime"].ToString());
                }
                //绑定广告有效的结束日期
                if ((dt.Rows[0]["endtime"].ToString().IndexOf("1900") < 0) && (dt.Rows[0]["endtime"].ToString().IndexOf("2555") < 0))
                {
                    endtime.SelectedDate = Convert.ToDateTime(dt.Rows[0]["endtime"].ToString());
                }

                code.Text = dt.Rows[0]["code"].ToString().Trim();


                parameters.Items.Clear();
                parameters.Items.Add(new ListItem("代码", "htmlcode"));
                if ((type.SelectedValue != Convert.ToInt16(AdType.FloatAd).ToString()) && (type.SelectedValue != Convert.ToInt16(AdType.DoubleAd).ToString()))
                {
                    parameters.Items.Add(new ListItem("文字", "word"));
                }
                parameters.Items.Add(new ListItem("图片", "image"));
                parameters.Items.Add(new ListItem("flash", "flash"));


                //初始化参数
                string[] parameter = Utils.SplitString(dt.Rows[0]["parameters"].ToString().Trim(), "|", 9);
                parameters.SelectedValue = parameter[0].Trim();
                parameters.Attributes.Add("onChange", "showparameters();");
                wordlink.Text = parameter[4].Trim();
                wordcontent.Text = parameter[5].Trim();
                wordfont.Text = parameter[6].Trim();

                imgsrc.Text = parameter[1].Trim();
                imgwidth.Text = parameter[2].Trim();
                imgheight.Text = parameter[3].Trim();
                imglink.Text = parameter[4].Trim();
                imgtitle.Text = parameter[5].Trim();

                flashsrc.Text = parameter[1].Trim();
                flashwidth.Text = parameter[2].Trim();
                flashheight.Text = parameter[3].Trim();

                if (type.SelectedValue == Convert.ToInt16(AdType.InPostAd).ToString())
                {
                    inpostposition.SelectedValue = parameter[7].Trim();
                    string error = "";
                    foreach (string floor in parameter[8].Trim().Split(','))
                    {
                        if (Utils.StrToInt(floor, 0) > __configinfo.Ppp)
                        {
                            error += floor + ",";
                        }
                        else
                        {
                            foreach (ListItem li in inpostfloor.Items)
                            {
                                if (Utils.InArray(li.Value, parameter[8].Trim()))
                                {
                                    li.Selected = true;
                                }
                            }
                        }
                    }
                    if(error != "")
                    {
                        base.RegisterStartupScript("", "<script>window.onload = function(){alert('每页帖数已经改变，原#" + error.TrimEnd(',') + "层大于现在" + __configinfo.Ppp + "层');}</script>");
                    }
                }

                if (type.SelectedValue == Convert.ToInt16(AdType.MediaAd).ToString())
                {
                    slwmvsrc.Text = parameter[1].Trim();
                    slimage.Text = parameter[2].Trim();
                    buttomimg.Text = parameter[4].Trim();
                    words1.Text = parameter[5].Trim();
                    words2.Text = parameter[6].Trim();
                    words3.Text = parameter[7].Trim();
                }
            }

            #endregion
        }


        private void UpdateADInfo_Click(object sender, EventArgs e)
        {
            #region 编辑广告信息

            if (this.CheckCookie())
            {
                string targetlist = DNTRequest.GetString("TargetFID");

                if ((targetlist == "") || (targetlist == ","))
                {
                    base.RegisterStartupScript("", "<script>alert('请您先选取相关的投放范围,再点击提交按钮');showadhint(Form1.type.value);showparameters(Form1.parameters.value);</script>");
                    return;
                }

                string starttimestr = starttime.SelectedDate.ToString();
                string endtimestr = endtime.SelectedDate.ToString();
                if (starttimestr.IndexOf("1900") < 0 && endtimestr.IndexOf("1900") < 0)
                {
                    if (Convert.ToDateTime(starttime.SelectedDate.ToString()) >= Convert.ToDateTime(endtime.SelectedDate.ToString()))
                    {
                        base.RegisterStartupScript("", "<script>alert('生效时间应该早于结束时间');showadhint(Form1.type.value);showparameters(Form1.parameters.value);</script>");
                        return;
                    }
                }

                string code = string.Empty;

                if (type.SelectedValue == Convert.ToInt16(AdType.QuickEditorBgAd).ToString())
                {
                    code = imglink.Text + "\r";
                    code += imgsrc.Text;
                }
                else
                {
                    code = GetCode();
                }
                
                targetlist = targetlist.IndexOf("全部") >= 0 ? ",全部," : "," + targetlist + ",";
                DatabaseProvider.GetInstance().UpdateAdvertisement(DNTRequest.GetInt("advid", 0), Utils.StrToInt(available.SelectedValue, 0), type.SelectedValue, Utils.StrToInt(displayorder.Text, 0),
                                                        tbtitle.Text, targetlist, GetParameters(), code,
                                                        starttimestr.IndexOf("1900") >= 0 ? "1900-1-1" : starttimestr,
                                                        endtimestr.IndexOf("1900") >= 0 ? "2555-1-1" : endtimestr);

                 Discuz.Cache.DNTCache.GetCacheService().RemoveObject("/Forum/Advertisements");
                 Response.Redirect("global_advsgrid.aspx");
                 //base.RegisterStartupScript( "PAGE",  "window.location.href='global_advsgrid.aspx';");
                
            }

            #endregion
        }


        /// <summary>
        /// 根据选择不同的展现方式而返回不同的代码, 
        /// 格式为 showtype | src | width | height | link | title | font |
        /// </summary>
        /// <returns></returns>
        public string GetCode()
        {
            #region 根据选取的类型得到返回值

            string result = "";
            switch (parameters.SelectedValue)
            {
                case "htmlcode":
                    result = code.Text.Trim();
                    break;
                case "word":
                    result = "<a href=\"" + wordlink.Text.Trim() + "\" target=\"_blank\" style=\"font-size: " + wordfont.Text + "\">" + wordcontent.Text.Trim() + "</a>";
                    break;
                case "image":
                    result = "<a href=\"" + imglink.Text.Trim() + "\" target=\"_blank\"><img src=\"" + imgsrc.Text.Trim() + "\"" + (imgwidth.Text.Trim() == "" ? "" : " width=\"" + imgwidth.Text.Trim() + "\"") + (imgheight.Text.Trim() == "" ? "" : " height=\"" + imgheight.Text.Trim() + "\"") + " alt=\"" + imgtitle.Text.Trim() + "\" border=\"0\" /></a>";
                    break;
                case "flash":
                    result = "<embed wmode=\"opaque\"" + (flashwidth.Text.Trim() == "" ? "" : " width=\"" + flashwidth.Text.Trim() + "\"") + (flashheight.Text.Trim() == "" ? "" : " height=\"" + flashheight.Text.Trim() + "\"") + " src=\"" + flashsrc.Text.Trim() + "\" type=\"application/x-shockwave-flash\"></embed>";
                    break;
            }
            if (type.SelectedValue == Convert.ToInt16(AdType.MediaAd).ToString())
            {
                result = "<script type='text/javascript' src='templates/{0}/mediaad.js'></script><script type='text/javascript'>printMediaAD('{1}', {2});</script>";
            }
            return result;

            #endregion
        }

        /// <summary>
        /// 根据选择不同的展现方式而返回不同的代码, 
        /// 格式为 showtype | src | width | height | link | title | font |
        /// </summary>
        /// <returns></returns>
        public string GetParameters()
        {
            #region 根据选取的类型得到返回值

            string result = "";
            switch (parameters.SelectedValue)
            {
                case "htmlcode":
                    result = "htmlcode|||||||";
                    break;
                case "word":
                    result = "word| | | | " + wordlink.Text.Trim() + "|" + wordcontent.Text.Trim() + "|" + wordfont.Text + "|";
                    break;
                case "image":
                    result = "image|" + imgsrc.Text.Trim() + "|" + imgwidth.Text.Trim() + "|" + imgheight.Text.Trim() + "|" + imglink.Text.Trim() + "|" + imgtitle.Text.Trim() + "||";
                    break;
                case "flash":
                    result = "flash|" + flashsrc.Text.Trim() + "|" + flashwidth.Text.Trim() + "|" + flashheight.Text + "||||";
                    break;
            }
            if (type.SelectedValue == Convert.ToInt16(AdType.MediaAd).ToString())
            {
                result = "silverlight|" + slwmvsrc.Text.Trim() + "|" + slimage.Text.Trim() + "|" + slimage.Text + "|" + buttomimg.Text + "|" + words1.Text + "|" + words2.Text + "|" + words3.Text;
            }

            if (type.SelectedValue == Convert.ToInt16(AdType.InPostAd).ToString())
            {
                result += inpostposition.SelectedValue + "|" + GetMultipleSelectedValue(inpostfloor) + "|";
            }
            return result;

            #endregion
        }

        private string GetMultipleSelectedValue(Discuz.Control.ListBox lb)
        {
            #region 返回楼层选择列表
            string result = string.Empty;
            foreach (ListItem li in lb.Items)
            {
                if (li.Selected && li.Value != "-1")
                {
                    result += li.Value + ",";
                }
            }

            if (result.Length > 0)
                result = result.Substring(0, result.Length - 1);

            return result;
            #endregion
        }

        private void DeleteADInfo_Click(object sender, EventArgs e)
        {
            #region 删除广告信息

            if (this.CheckCookie())
            {
                DatabaseProvider.GetInstance().DeleteAdvertisement(DNTRequest.GetString("advid"));
                Discuz.Cache.DNTCache.GetCacheService().RemoveObject("/Forum/Advertisements");

                base.RegisterStartupScript( "PAGE", "window.location.href='global_advsgrid.aspx';");
            }

            #endregion
        }

        private void type_SelectedIndexChanged(object sender, EventArgs e)
        {
            #region 根据广告类型设置参数控件的数据项

            if ((type.SelectedValue == Convert.ToInt16(AdType.FloatAd).ToString()) ||
                (type.SelectedValue == Convert.ToInt16(AdType.DoubleAd).ToString()))
            {
                if (parameters.Items[1].Value == "word")
                {
                    parameters.Items.RemoveAt(1);
                }
            }
            else
            {
                if (parameters.Items[1].Value != "word")
                {
                    parameters.Items.Insert(1, new ListItem("文字", "word"));
                }
            }

            if (type.SelectedValue == Convert.ToInt16(AdType.QuickEditorBgAd).ToString())
            {
                for (int i = 0; i < parameters.Items.Count;i++ )
                {
                    if (parameters.Items[i].Value != "image")
                        parameters.Items.RemoveAt(i);
                }
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

        override protected void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.type.SelectedIndexChanged += new EventHandler(this.type_SelectedIndexChanged);

            this.UpdateADInfo.Click += new EventHandler(this.UpdateADInfo_Click);
            this.DeleteADInfo.Click += new EventHandler(this.DeleteADInfo_Click);
            this.Load += new EventHandler(this.Page_Load);

            tbtitle.AddAttributes("maxlength", "40");
            tbtitle.AddAttributes("size", "40");

            type.Items.Clear();
            //加载树
            type.Items.Add(new ListItem("请选择     ", "-1"));
            type.Items.Add(new ListItem("头部横幅广告", Convert.ToInt16(AdType.HeaderAd).ToString()));
            type.Items.Add(new ListItem("尾部横幅广告", Convert.ToInt16(AdType.FooterAd).ToString()));
            type.Items.Add(new ListItem("页内文字广告", Convert.ToInt16(AdType.PageWordAd).ToString()));
            type.Items.Add(new ListItem("帖内广告", Convert.ToInt16(AdType.InPostAd).ToString()));
            type.Items.Add(new ListItem("帖间通栏广告", Convert.ToInt16(AdType.PostLeaderboardAd).ToString()));
            type.Items.Add(new ListItem("浮动广告", Convert.ToInt16(AdType.FloatAd).ToString()));
            type.Items.Add(new ListItem("对联广告", Convert.ToInt16(AdType.DoubleAd).ToString()));
            //type.Items.Add(new ListItem("Silverlight媒体广告", Convert.ToInt16(AdType.MediaAd).ToString()));
            type.Items.Add(new ListItem("分类间广告", Convert.ToInt16(AdType.InForumAd).ToString()));
            type.Items.Add(new ListItem("快速发帖栏上方广告", Convert.ToInt16(AdType.QuickEditorAd).ToString()));
            type.Items.Add(new ListItem("快速编辑器背景广告", Convert.ToInt16(AdType.QuickEditorBgAd).ToString()));
            type.Attributes.Add("onChange", "showadhint();");

           
            if (DNTRequest.GetString("advid") == "")
            {
                Response.Redirect("advertisementsgrid.aspx");
            }
            else
            {
                LoadAnnounceInf(DNTRequest.GetInt("advid", -1));
            }
        }

        #endregion
    }
}