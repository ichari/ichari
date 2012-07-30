using System;
using System.Web.UI;

using Discuz.Common;
using Discuz.Forum;

namespace Discuz.Web.Admin
{
	/// <summary>
	/// 更新缓存
	/// </summary>
    public partial class global_refreshallcache : Page
	{
		private void Page_Load(object sender, EventArgs e)
		{
			if (!Page.IsPostBack)
			{
				int opnumber = DNTRequest.GetInt("opnumber", 0);
				int result = -1;

				#region 根据缓存更新选项更新相应的缓存数据

				switch (opnumber)
				{
					case 1:
					{
                        //重设管理组信息
						AdminCaches.ReSetAdminGroupList();
						result = 2;
						break;
					}
					case 2:
					{
                        //重设用户组信息
                        AdminCaches.ReSetUserGroupList();
						result = 3;
						break;
					}
					case 3:
					{
                        //重设版主信息
                        AdminCaches.ReSetModeratorList();
						result = 4;
						break;
					}

					case 4:
					{
                        //重设指定时间内的公告列表
                        AdminCaches.ReSetAnnouncementList();
                        AdminCaches.ReSetSimplifiedAnnouncementList();
						result = 5;
						break;
					}
					case 5:
					{
                        //重设第一条公告
                        AdminCaches.ReSetSimplifiedAnnouncementList();
						result = 6;
						break;
					}
					case 6:
					{
                        //重设版块下拉列表
                        AdminCaches.ReSetForumListBoxOptions();
						result = 7;
						break;
					}
					case 7:
					{
                        //重设表情
                        AdminCaches.ReSetSmiliesList();
						result = 8;
						break;
					}
					case 8:
					{
                        //重设主题图标
                        AdminCaches.ReSetIconsList();
						result = 9;
						break;
					}
					case 9:
					{
                        //重设自定义标签
                        AdminCaches.ReSetCustomEditButtonList();
						result = 10;
						break;
					}
					case 10:
					{
                        //重设论坛基本设置
						//AdminCaches.ReSetConfig();
						result = 11;
						break;
					}
					case 11:
					{
                        //重设论坛金币
                        AdminCaches.ReSetScoreset();
						result = 12;
						break;
					}
					case 12:
					{
                        //重设地址对照表
                        AdminCaches.ReSetSiteUrls();
						result = 13;
						break;
					}
					case 13:
					{
                        //重设论坛统计信息
                        AdminCaches.ReSetStatistics();
						result = 14;
						break;
					}
					case 14:
					{
                        //重设系统允许的附件类型和大小
                        AdminCaches.ReSetAttachmentTypeArray();
						result = 15;
						break;
					}
					case 15:
					{
                        //重设模板列表的下拉框html
                        AdminCaches.ReSetTemplateListBoxOptionsCache();
						result = 16;
						break;
					}
					case 16:
					{
                        //重设在线用户列表图例
                        AdminCaches.ReSetOnlineGroupIconList();
						result = 17;
						break;
					}
					case 17:
					{
                        //重设友情链接列表
                        AdminCaches.ReSetForumLinkList();
						result = 18;
						break;
					}
					case 18:
					{
                        //重设脏字过滤列表
                        AdminCaches.ReSetBanWordList();
						result = 19;
						break;
					}
					case 19:
					{
                        //重设论坛列表
                        AdminCaches.ReSetForumList();
						result = 20;
						break;
					}
					case 20:
					{
                        //重设在线用户信息
                        AdminCaches.ReSetOnlineUserTable();
						result = 21;
						break;
					}
					case 21:
					{
                        //重设论坛整体RSS及指定版块RSS
                        AdminCaches.ReSetRss();
						result = 22;
						break;
					}
					case 22:
					{
                        //重设论坛整体RSS
                        AdminCaches.ReSetRssXml();
						result = 23;
						break;
					}
					case 23:
					{
                        //重设模板ID列表
                        AdminCaches.ReSetValidTemplateIDList();
						result = 24;
						break;
					}
					case 24:
					{
                        //重设有效用户表扩展字段
                        AdminCaches.ReSetValidScoreName();
						result = 25;
						break;
					}
					case 25:
					{
                        //重设勋章列表
                        AdminCaches.ReSetMedalsList();
						result = 26;
						break;
					}
					case 26:
					{
                        //重设数据链接串和表前缀
                        AdminCaches.ReSetDBlinkAndTablePrefix();
						result = 27;
						break;
					}
					case 27:
					{
                        //重设帖子列表
                        AdminCaches.ReSetAllPostTableName();
						result = 28;
						break;
					}
					case 28:
					{
                        //重设最后帖子表
                        AdminCaches.ReSetLastPostTableName();
						result = 29;
						break;
					}
					case 29:
					{
                        //重设广告列表
                        AdminCaches.ReSetAdsList();
						result = 30;
						break;
					}
					case 30:
					{
                        //重设用户上一次执行搜索操作时间
                        AdminCaches.ReSetStatisticsSearchtime();
						result = 31;
						break;
					}
					case 31:
					{
                        //重设用户一分钟内搜索次数
                        AdminCaches.ReSetStatisticsSearchcount();
						result = 32;
						break;
					}
					case 32:
					{
                        //重设用户头象列表
                        AdminCaches.ReSetCommonAvatarList();
						result = 33;
						break;
					}
					case 33:
					{
                        //重设干扰码字符串
						AdminCaches.ReSetJammer();
						result = 34;
						break;
					}
					case 34:
					{
                        //重设魔力列表
						AdminCaches.ReSetMagicList();
						result = 35;
						break;
					}
					case 35:
					{
                        //重设兑换比率可交易金币策略
						AdminCaches.ReSetScorePaySet();
						result = 36;
						break;
					}
					case 36:
					{
                        //重设当前帖子表相关信息
						AdminCaches.ReSetPostTableInfo();
						result = 37;
						break;
					}
					case 37:
					{
                        //重设全部版块精华主题列表
						AdminCaches.ReSetDigestTopicList(16);
						result = 38;
						break;
					}
					case 38:
					{
                        //重设全部版块热帖主题列表
						AdminCaches.ReSetHotTopicList(16, 30);
						result = 39;
						break;
					}
					case 39:
					{
                        //重设最近主题列表
						AdminCaches.ReSetRecentTopicList(16);
						result = 40;
						break;
					}
					case 41:
					{
                        //重设在线用户表
						OnlineUsers.InitOnlineList();
						result = 42;
						break;
					}
                    case 42:
			        {
                        //重设导航弹出菜单
			            AdminCaches.ReSetNavPopupMenu();
			            result = -1;
			            break;
			        }
				}

				#endregion

				Response.Write(result);
				Response.ExpiresAbsolute = DateTime.Now.AddSeconds(-1);
				Response.Expires = -1;
				Response.End();
			}
		}

		#region Web 窗体设计器生成的代码

		protected override void OnInit(EventArgs e)
		{
			InitializeComponent();
			base.OnInit(e);
		}

		private void InitializeComponent()
		{
			this.Load += new EventHandler(this.Page_Load);
		}

		#endregion
	}
}
