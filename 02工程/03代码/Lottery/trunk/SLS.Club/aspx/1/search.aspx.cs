using System;
using System.Data;
using System.Data.SqlClient;
using Discuz.Common;
using Discuz.Forum;
using Discuz.Web.UI;
using Discuz.Entity;

namespace Discuz.Web
{
	/// <summary>
	/// 搜索页面
	/// </summary>
	public partial class search : PageBase
    {
        #region 页面变量
        /// <summary>
        /// 版块列表
        /// </summary>
		public string forumlist;
        /// <summary>
        /// 搜索缓存Id
        /// </summary>
		public int searchid;
        /// <summary>
        /// 当前页码
        /// </summary>
		public int pageid;
        /// <summary>
        /// 主题数量
        /// </summary>
		public int topiccount;
        /// <summary>
        /// 分页数量
        /// </summary>
		public int pagecount;
        /// <summary>
        /// 分页页码链接
        /// </summary>
		public string pagenumbers;
        /// <summary>
        /// 搜索结果数量
        /// </summary>
		public int searchresultcount;
        /// <summary>
        /// 搜索出的主题列表
        /// </summary>
		public DataTable topiclist;
        /// <summary>
        /// 帖子分表列表
        /// </summary>
		public DataTable tablelist;
        /// <summary>
        /// 当此值为true时,显示搜索结果提示
        /// </summary>
		public bool searchpost;
        /// <summary>
        /// 搜索类型
        /// </summary>
		public string type = "post";
        /// <summary>
        /// 当前主题页码
        /// </summary>
        public int topicpageid;
        /// <summary>
        /// 主题分页总数
        /// </summary>
        public int topicpagecount;
        /// <summary>
        /// 主题分页页码链接
        /// </summary>
        public string topicpagenumbers;
        #endregion

        protected override void ShowPage()
		{
			pagetitle = "搜索";
			searchid = DNTRequest.GetInt("searchid", -1);
			searchresultcount = 0;
			
			if (usergroupinfo.Allowsearch == 0)
			{
				AddErrLine("您当前的身份 \"" + usergroupinfo.Grouptitle + "\" 没有搜索的权限");
				return;
			}

            if (usergroupinfo.Allowsearch == 2 && DNTRequest.GetInt("keywordtype", 0) == 1)
            {
                AddErrLine("您当前的身份 \"" + usergroupinfo.Grouptitle + "\" 没有全文搜索的权限");
                return;
            }

			searchpost = false;
			if (!ispost)
			{
				tablelist = Posts.GetAllPostTableName();
			}
			
			if (DNTRequest.IsPost() || DNTRequest.GetString("posterid") != "")
			{

				searchpost = true;
				//　如果当前用户非管理员并且论坛设定了禁止全文搜索时间段，当前时间如果在其中的一个时间段内，不允许全文搜索
				if (useradminid != 1 && DNTRequest.GetInt("keywordtype", 0) == 1 && usergroupinfo.Disableperiodctrl != 1)
				{
					string visittime = "";
                    if (Scoresets.BetweenTime(config.Searchbanperiods, out visittime))
					{
						AddErrLine("在此时间段( " + visittime + " )内用户不可以进行全文搜索");
						return;
					}
					
				}

				if (useradminid != 1)
				{
					//判断一分钟内搜索的次数是不是超过限制值
					if (!Statistics.CheckSearchCount(config.Maxspm))
					{
						AddErrLine("抱歉,系统在一分钟内搜索的次数超过了系统安全设置的上限,请稍候再试");
						return;
					}


					int Interval = Utils.StrDateDiffSeconds(lastsearchtime, config.Searchctrl);
					if (Interval <= 0)
					{
						AddErrLine("系统规定搜索间隔为" + config.Searchctrl.ToString() + "秒, 您还需要等待 " + (Interval * -1).ToString() + " 秒");
						return;
					}

					//不是管理员，则如果设置搜索扣金币时扣除用户金币
					if (UserCredits.UpdateUserCreditsBySearch(base.userid) == -1)
					{
						AddErrLine("您的金币不足, 不能执行搜索操作");
						return;
					}
				}

				int posterid = DNTRequest.GetInt("posterid", -1);
				int searchtime = DNTRequest.GetInt("searchtime", 0);
				string searchforumid = DNTRequest.GetString("searchforumid").Trim();
				string[] forumidlist = Utils.SplitString(searchforumid, ",");

				if (DNTRequest.GetString("keyword").Equals("") && DNTRequest.GetString("poster").Equals("") && DNTRequest.GetString("posterid").Equals(""))
				{
					AddErrLine("关键字和用户名不能同时为空");
					return;
				}
			
				if (posterid > 0)
				{
                    if (!Discuz.Forum.Users.Exists(posterid))
					{
						AddErrLine("指定的用户ID不存在");
						return;
					}
				}
				else if (!DNTRequest.GetString("poster").Equals(""))
				{
					posterid = Discuz.Forum.Users.GetUserID(DNTRequest.GetString("poster"));
					if (posterid == -1)
					{
						AddErrLine("搜索用户名不存在");
						return;
					}
				}
			
				if (!searchforumid.Equals(""))
				{
					foreach (string strid in forumidlist)
					{
						if (!Utils.IsNumeric(strid))
						{
							AddErrLine("非法的搜索版块ID");
							
							return;
						}
					}
				}

				type = DNTRequest.GetString("type").ToLower();
                int keywordtype = DNTRequest.GetInt("keywordtype", 0);
				if (type == "author")
                    keywordtype = 8;

                if (DNTRequest.GetString("keyword") == string.Empty && posterid > 0 && type == string.Empty)
                {
                    type = "author";
                    keywordtype = 8;
                }

				if (type != "")
				{
					if (!Utils.InArray(type, "post,digest,spacepost,album,author"))// (type != "topic") && (type != "digest") && (type != "post"))
					{
						AddErrLine("非法的参数信息");
						return;
					}
        		}

				int posttableid = DNTRequest.GetInt("posttableid", 0);

                searchid = Searches.Search(posttableid, userid, usergroupid, DNTRequest.GetString("keyword").Trim(), posterid, type, searchforumid, keywordtype, searchtime, DNTRequest.GetInt("searchtimetype", 0), DNTRequest.GetInt("resultorder", 0), DNTRequest.GetInt("resultordertype", 0));

                switch (keywordtype)
                { 
                    case 2:
                        type = "spacepost";
                        break;
                    case 3:
                        type = "album";
                        break;
                    case 8:
                        type = "author";
                        break;
                    default:
                        type = string.Empty;
                        break;
                }
				if (searchid > 0)
				{
					SetUrl("search.aspx?type=" + type + "&searchid=" + searchid.ToString());					
                	
					SetMetaRefresh();
					SetShowBackLink(false);
					AddMsgLine("搜索完毕, 稍后将转到搜索结果页面");					
				}
				else
				{
					AddMsgLine("抱歉, 没有搜索到符合要求的记录");
				}
				return;
			}
			else
			{
				searchid = DNTRequest.GetInt("searchid", -1);
				if (searchid > 0)
				{
					pageid = DNTRequest.GetInt("page", 1);
	
					type = DNTRequest.GetString("type").ToLower();
					if (type != "")
					{
						if (!Utils.InArray(type, "topic,spacepost,album,author"))// (type != "topic") && (type != "digest") && (type != "post"))
						{
							AddErrLine("非法的参数信息");
							return;
						}
					}

					int posttableid = DNTRequest.GetInt("posttableid", 0);
                    CalculateCurrentPage();
                    
                    switch (type)
                    { 
                        case "author":
                            topicpageid = DNTRequest.GetInt("topicpage", 1);
                            topiclist = Searches.GetSearchCacheList(posttableid, searchid, 16, topicpageid, out topiccount, "");

                            topicpageid = CalculateCurrentPage(topiccount, topicpageid, out topicpagecount);

                            //得到页码链接
                            pagenumbers = Utils.GetPageNumbers(topicpageid, topicpagecount, "search.aspx?type=" + type + "&searchid=" + searchid.ToString(), 8, "topicpage");
                            
                            return;
                        default:
                            topiclist = Searches.GetSearchCacheList(topicpageid, searchid, 16, pageid, out topiccount, type);

                            //得到页码链接
                            pagenumbers = Utils.GetPageNumbers(pageid, pagecount, "search.aspx?type=" + type + "&searchid=" + searchid.ToString(), 8);
                            break;
                    }

                    if (topiccount == 0)
					{
						AddErrLine("不存在的searchid");
						return;
					}
                    
                    

					return;
			
				}
				else
				{
					forumlist = Caches.GetForumListBoxOptionsCache();
				}
			}

		}

        private void CalculateCurrentPage()
        {
            //获取总页数
            pagecount = topiccount % 16 == 0 ? topiccount / 16 : topiccount / 16 + 1;
            if (pagecount == 0)
            {
                pagecount = 1;
            }
            //修正请求页数中可能的错误
            if (pageid < 1)
            {
                pageid = 1;
            }
            if (pageid > pagecount)
            {
                pageid = pagecount;
            }
        }
        private int CalculateCurrentPage(int listcount, int pageid, out int pagecount)
        {
            //int pagecount;
            //获取总页数
            pagecount = listcount % 16 == 0 ? listcount / 16 : listcount / 16 + 1;
            if (pagecount == 0)
            {
                pagecount = 1;
            }
            //修正请求页数中可能的错误
            if (pageid < 1)
            {
                pageid = 1;
            }
            if (pageid > pagecount)
            {
                pageid = pagecount;
            }
            return pageid;
        }

        override protected void OnLoad(EventArgs e)
        {

            /* 
                This page was created by Discuz!NT Template Engine at 2008/10/13 15:54:40.
                本页面代码由Discuz!NT模板引擎生成于 2008/10/13 15:54:40. 
            */

            base.OnLoad(e);


            templateBuilder.Append("<div id=\"foruminfo\">\r\n");
            templateBuilder.Append("	<div id=\"nav\">\r\n");
            templateBuilder.Append("		<a href=\"" + config.Forumurl.ToString().Trim() + "\">" + config.Forumtitle.ToString().Trim() + "</a> &raquo; <strong>搜索</strong>\r\n");
            templateBuilder.Append("	</div>\r\n");
            templateBuilder.Append("	<div id=\"headsearch\">\r\n");
            templateBuilder.Append("		<div id=\"search\">\r\n");

            if (usergroupinfo.Allowsearch > 0)
            {


                if (searchid != -1 || searchpost)
                {


                    templateBuilder.Append("			<form method=\"post\" action=\"search.aspx\" target=\"_blank\" onsubmit=\"bind_keyword(this);\">\r\n");
                    templateBuilder.Append("				<input type=\"hidden\" name=\"poster\" />\r\n");
                    templateBuilder.Append("				<input type=\"hidden\" name=\"keyword\" />\r\n");
                    templateBuilder.Append("				<input type=\"hidden\" name=\"type\" value=\"\" />\r\n");
                    templateBuilder.Append("				<input id=\"keywordtype\" type=\"hidden\" name=\"keywordtype\" value=\"0\"/>\r\n");
                    templateBuilder.Append("				<div id=\"searchbar\">\r\n");
                    templateBuilder.Append("					<dl>\r\n");
                    templateBuilder.Append("						<dt id=\"quicksearch\" class=\"s2\" onclick=\"showMenu(this.id, false);\" onmouseover=\"MouseCursor(this);\">帖子标题</dt>\r\n");
                    templateBuilder.Append("						<dd class=\"textinput\"><input type=\"text\" name=\"keywordf\" value=\"\" class=\"text\"/></dd>\r\n");
                    templateBuilder.Append("						<dd><input name=\"searchsubmit\" type=\"submit\" value=\"\" class=\"s3\"/></dd>\r\n");
                    templateBuilder.Append("					</dl>\r\n");
                    templateBuilder.Append("				</div>\r\n");
                    templateBuilder.Append("			</form>\r\n");
                    templateBuilder.Append("			<script type=\"text/javascript\">function bind_keyword(form){if(form.keywordtype.value=='8'){form.keyword.value='';form.poster.value=form.keywordf.value; } else { form.poster.value=''; form.keyword.value=form.keywordf.value;if(form.keywordtype.value == '2')form.type.value = 'spacepost';if(form.keywordtype.value == '3')form.type.value = 'album';}}</" + "script>\r\n");



                }	//end if


            }	//end if

            templateBuilder.Append("		</div>\r\n");
            templateBuilder.Append("	</div>\r\n");
            templateBuilder.Append("</div>\r\n");

            if (page_err == 0)
            {


                if (searchpost)
                {


                    templateBuilder.Append("<div class=\"box message\">\r\n");
                    templateBuilder.Append("	<h1>彩友提示信息</h1>\r\n");
                    templateBuilder.Append("	<p>" + msgbox_text.ToString() + "</p>\r\n");

                    if (msgbox_url != "")
                    {

                        templateBuilder.Append("	<p><a href=\"" + msgbox_url.ToString() + "\">如果浏览器没有转向, 请点击这里.</a></p>\r\n");

                    }	//end if

                    templateBuilder.Append("</div>\r\n");
                    templateBuilder.Append("</div>\r\n");



                }
                else
                {


                    if (searchid == -1)
                    {

                        templateBuilder.Append("<div id=\"options_item\">\r\n");
                        templateBuilder.Append("	<div id=\"postoptions\">\r\n");
                        templateBuilder.Append("		<table cellpadding=\"0\" cellspacing=\"0\" border=\"0\">\r\n");
                        templateBuilder.Append("		<tbody>\r\n");
                        templateBuilder.Append("		<tr>\r\n");
                        templateBuilder.Append("			<th><label for=\"posttableid\">选择分表</label></th>\r\n");
                        templateBuilder.Append("			<td>\r\n");
                        templateBuilder.Append("				<select name=\"posttableid\" id=\"posttableid\">\r\n");

                        int table__loop__id = 0;
                        foreach (DataRow table in tablelist.Rows)
                        {
                            table__loop__id++;

                            templateBuilder.Append("					<option value=\"" + table["id"].ToString().Trim() + "\">" + table["description"].ToString().Trim() + "\r\n");

                            if (Utils.StrToInt(table__loop__id, 0) == 1)
                            {

                                templateBuilder.Append("(当前使用)\r\n");

                            }	//end if

                            templateBuilder.Append("</option>\r\n");

                        }	//end loop

                        templateBuilder.Append("				</select>\r\n");
                        templateBuilder.Append("			</td>\r\n");
                        templateBuilder.Append("		</tr>\r\n");
                        templateBuilder.Append("		</tbody>\r\n");
                        templateBuilder.Append("		<tbody>\r\n");
                        templateBuilder.Append("		<tr>\r\n");
                        templateBuilder.Append("			<th><label for=\"searchtime\">时间</label></th>\r\n");
                        templateBuilder.Append("			<td>\r\n");
                        templateBuilder.Append("				<select name=\"searchtime\" id=\"searchtime\">\r\n");
                        templateBuilder.Append("				  <option value=\"0\" selected=\"selected\">全部时间</option>\r\n");
                        templateBuilder.Append("				  <option value=\"-1\">1天前</option>\r\n");
                        templateBuilder.Append("				  <option value=\"-2\">2天前</option>\r\n");
                        templateBuilder.Append("				  <option value=\"-3\">3天前</option>\r\n");
                        templateBuilder.Append("				  <option value=\"-7\">1周前</option>\r\n");
                        templateBuilder.Append("				  <option value=\"-30\">1个月前</option>\r\n");
                        templateBuilder.Append("				  <option value=\"-90\">3个月前</option>\r\n");
                        templateBuilder.Append("				  <option value=\"-180\">半年前</option>\r\n");
                        templateBuilder.Append("				  <option value=\"-365\">1年前</option>\r\n");
                        templateBuilder.Append("				</select>\r\n");
                        templateBuilder.Append("				  <input type=\"radio\" name=\"searchtimetype\" value=\"1\" />\r\n");
                        templateBuilder.Append("				之前\r\n");
                        templateBuilder.Append("				<input name=\"searchtimetype\" type=\"radio\" value=\"0\" checked />\r\n");
                        templateBuilder.Append("				之后\r\n");
                        templateBuilder.Append("			</td>\r\n");
                        templateBuilder.Append("		</tr>\r\n");
                        templateBuilder.Append("		</tbody>\r\n");
                        templateBuilder.Append("		<tbody>\r\n");
                        templateBuilder.Append("		<tr>\r\n");
                        templateBuilder.Append("			<th><label for=\"resultorder\">结果排序</label></th>\r\n");
                        templateBuilder.Append("			<td>\r\n");
                        templateBuilder.Append("				<select name=\"resultorder\" id=\"resultorder\">\r\n");
                        templateBuilder.Append("				  <option value=\"0\" selected=\"selected\">最后回复时间</option>\r\n");
                        templateBuilder.Append("				  <option value=\"1\">发表时间</option>\r\n");
                        templateBuilder.Append("				  <option value=\"2\">回复数量</option>\r\n");
                        templateBuilder.Append("				  <option value=\"3\">查看次数</option>\r\n");
                        templateBuilder.Append("				</select>\r\n");
                        templateBuilder.Append("				<input type=\"radio\" name=\"resultordertype\" value=\"1\" />\r\n");
                        templateBuilder.Append("				升序\r\n");
                        templateBuilder.Append("				<input name=\"resultordertype\" type=\"radio\" value=\"0\" checked />\r\n");
                        templateBuilder.Append("				降序\r\n");
                        templateBuilder.Append("			</td>\r\n");
                        templateBuilder.Append("		</tr>\r\n");
                        templateBuilder.Append("		</tbody>\r\n");
                        templateBuilder.Append("		<tbody>\r\n");
                        templateBuilder.Append("		<tr>\r\n");
                        templateBuilder.Append("			<th><label for=\"searchforumid\">搜索范围</label></th>\r\n");
                        templateBuilder.Append("			<td>\r\n");
                        templateBuilder.Append("				<select name=\"searchforumid\" size=\"12\" style=\"width:450px\" multiple=\"multiple\" id=\"searchforumid\">\r\n");
                        templateBuilder.Append("				 <option selected value=\"\">---------- 所有版块(默认) ----------</option>\r\n");
                        templateBuilder.Append("					<!--模版中所有版块的下拉框中一定要加入value=\"\"否则会提示没有选择版块-->\r\n");
                        templateBuilder.Append("					" + forumlist.ToString() + "\r\n");
                        templateBuilder.Append("				 </select>\r\n");
                        templateBuilder.Append("				 <p>(按Ctrl或Shift键可以多选,不选择)</p>\r\n");
                        templateBuilder.Append("			</td>\r\n");
                        templateBuilder.Append("		</tr>\r\n");
                        templateBuilder.Append("		</tbody>\r\n");
                        templateBuilder.Append("		</table>\r\n");
                        templateBuilder.Append("	</div>\r\n");
                        templateBuilder.Append("	<div id=\"spacepostoptions\">\r\n");
                        templateBuilder.Append("		<table cellpadding=\"0\" cellspacing=\"0\" border=\"0\">\r\n");
                        templateBuilder.Append("		<tbody>\r\n");
                        templateBuilder.Append("		<tr>\r\n");
                        templateBuilder.Append("			<th><label for=\"searchtime\">时间</label></th>\r\n");
                        templateBuilder.Append("			<td>\r\n");
                        templateBuilder.Append("				<select name=\"searchtime\" id=\"searchtime\">\r\n");
                        templateBuilder.Append("				  <option value=\"0\" selected=\"selected\">全部时间</option>\r\n");
                        templateBuilder.Append("				  <option value=\"-1\">1天前</option>\r\n");
                        templateBuilder.Append("				  <option value=\"-2\">2天前</option>\r\n");
                        templateBuilder.Append("				  <option value=\"-3\">3天前</option>\r\n");
                        templateBuilder.Append("				  <option value=\"-7\">1周前</option>\r\n");
                        templateBuilder.Append("				  <option value=\"-30\">1个月前</option>\r\n");
                        templateBuilder.Append("				  <option value=\"-90\">3个月前</option>\r\n");
                        templateBuilder.Append("				  <option value=\"-180\">半年前</option>\r\n");
                        templateBuilder.Append("				  <option value=\"-365\">1年前</option>\r\n");
                        templateBuilder.Append("				</select>\r\n");
                        templateBuilder.Append("				  <input type=\"radio\" name=\"searchtimetype\" value=\"1\" />\r\n");
                        templateBuilder.Append("				之前\r\n");
                        templateBuilder.Append("				<input name=\"searchtimetype\" type=\"radio\" value=\"0\" checked />\r\n");
                        templateBuilder.Append("				之后\r\n");
                        templateBuilder.Append("			</td>\r\n");
                        templateBuilder.Append("		</tr>\r\n");
                        templateBuilder.Append("		</tbody>\r\n");
                        templateBuilder.Append("		<tbody>\r\n");
                        templateBuilder.Append("		<tr>\r\n");
                        templateBuilder.Append("			<th><label for=\"resultorder\">结果排序</label></th>\r\n");
                        templateBuilder.Append("			<td>\r\n");
                        templateBuilder.Append("				<select name=\"resultorder\" id=\"resultorder\">\r\n");
                        templateBuilder.Append("				  <option value=\"0\" selected=\"selected\">发表时间</option>\r\n");
                        templateBuilder.Append("				  <option value=\"1\">回复数量</option>\r\n");
                        templateBuilder.Append("				  <option value=\"2\">查看次数</option>\r\n");
                        templateBuilder.Append("				</select>\r\n");
                        templateBuilder.Append("				<input type=\"radio\" name=\"resultordertype\" value=\"1\" />\r\n");
                        templateBuilder.Append("				升序\r\n");
                        templateBuilder.Append("				<input name=\"resultordertype\" type=\"radio\" value=\"0\" checked />\r\n");
                        templateBuilder.Append("				降序\r\n");
                        templateBuilder.Append("			</td>\r\n");
                        templateBuilder.Append("		</tr>\r\n");
                        templateBuilder.Append("		</tbody>\r\n");
                        templateBuilder.Append("		</table>\r\n");
                        templateBuilder.Append("	</div>\r\n");
                        templateBuilder.Append("	<div id=\"albumoptions\">\r\n");
                        templateBuilder.Append("		<table cellpadding=\"0\" cellspacing=\"0\" border=\"0\">\r\n");
                        templateBuilder.Append("		<tbody>\r\n");
                        templateBuilder.Append("		<tr>\r\n");
                        templateBuilder.Append("			<th><label for=\"searchtime\">时间</label></th>\r\n");
                        templateBuilder.Append("			<td>\r\n");
                        templateBuilder.Append("				<select name=\"searchtime\" id=\"searchtime\">\r\n");
                        templateBuilder.Append("				  <option value=\"0\" selected=\"selected\">全部时间</option>\r\n");
                        templateBuilder.Append("				  <option value=\"-1\">1天前</option>\r\n");
                        templateBuilder.Append("				  <option value=\"-2\">2天前</option>\r\n");
                        templateBuilder.Append("				  <option value=\"-3\">3天前</option>\r\n");
                        templateBuilder.Append("				  <option value=\"-7\">1周前</option>\r\n");
                        templateBuilder.Append("				  <option value=\"-30\">1个月前</option>\r\n");
                        templateBuilder.Append("				  <option value=\"-90\">3个月前</option>\r\n");
                        templateBuilder.Append("				  <option value=\"-180\">半年前</option>\r\n");
                        templateBuilder.Append("				  <option value=\"-365\">1年前</option>\r\n");
                        templateBuilder.Append("				</select>\r\n");
                        templateBuilder.Append("				  <input type=\"radio\" name=\"searchtimetype\" value=\"1\" />\r\n");
                        templateBuilder.Append("				之前\r\n");
                        templateBuilder.Append("				<input name=\"searchtimetype\" type=\"radio\" value=\"0\" checked />\r\n");
                        templateBuilder.Append("				之后\r\n");
                        templateBuilder.Append("			</td>\r\n");
                        templateBuilder.Append("		</tr>\r\n");
                        templateBuilder.Append("		</tbody>\r\n");
                        templateBuilder.Append("		<tbody>\r\n");
                        templateBuilder.Append("		<tr>\r\n");
                        templateBuilder.Append("			<th><label for=\"resultorder\">结果排序</label></th>\r\n");
                        templateBuilder.Append("			<td>\r\n");
                        templateBuilder.Append("				<select name=\"resultorder\" id=\"resultorder\">\r\n");
                        templateBuilder.Append("				  <option value=\"0\" selected=\"selected\">创建时间</option>\r\n");
                        templateBuilder.Append("				</select>\r\n");
                        templateBuilder.Append("				<input type=\"radio\" name=\"resultordertype\" value=\"1\" />\r\n");
                        templateBuilder.Append("				升序\r\n");
                        templateBuilder.Append("				<input name=\"resultordertype\" type=\"radio\" value=\"0\" checked />\r\n");
                        templateBuilder.Append("				降序\r\n");
                        templateBuilder.Append("			</td>\r\n");
                        templateBuilder.Append("		</tr>\r\n");
                        templateBuilder.Append("		</tbody>\r\n");
                        templateBuilder.Append("		</table>\r\n");
                        templateBuilder.Append("	</div>\r\n");
                        templateBuilder.Append("</div>\r\n");
                        templateBuilder.Append("<form id=\"postpm\" name=\"postpm\" method=\"post\" onsubmit=\"if(this.chkAuthor.checked)$('type').value='author';return true;\" action=\"\">\r\n");
                        templateBuilder.Append("<DIV class=\"mainbox formbox\">\r\n");
                        templateBuilder.Append("	<h1>搜索</h1>\r\n");
                        templateBuilder.Append("	<TABLE cellSpacing=\"0\" cellPadding=\"0\" summary=\"搜索\">\r\n");
                        templateBuilder.Append("		<tbody>\r\n");
                        templateBuilder.Append("		<tr id=\"divkeyword\">\r\n");
                        templateBuilder.Append("			<th><label for=\"keyword\">关键词</label></th>\r\n");
                        templateBuilder.Append("			<td><input name=\"keyword\" type=\"text\" id=\"keyword\" size=\"45\" />&nbsp;&nbsp;多个关键词间用英文空格分开</td>\r\n");
                        templateBuilder.Append("		</tr>\r\n");
                        templateBuilder.Append("		</tbody>\r\n");
                        templateBuilder.Append("		<tbody>\r\n");
                        templateBuilder.Append("		<tr>\r\n");
                        templateBuilder.Append("			<th><label for=\"poster\">作者</label></th>\r\n");
                        templateBuilder.Append("			<td>\r\n");
                        templateBuilder.Append("			<input name=\"poster\" type=\"text\" id=\"poster\" size=\"45\" />\r\n");
                        templateBuilder.Append("			<input type=\"checkbox\" value=\"1\" id=\"chkAuthor\" name=\"chkAuthor\" onclick=\"checkauthoroption(this);\" />搜索该作者所有帖子及相关内容\r\n");
                        templateBuilder.Append("			</td>\r\n");
                        templateBuilder.Append("		</tr>\r\n");
                        templateBuilder.Append("		</tbody>\r\n");
                        templateBuilder.Append("	</table>\r\n");
                        templateBuilder.Append("	<table cellSpacing=\"0\" cellPadding=\"0\" summary=\"搜索选项\">\r\n");
                        templateBuilder.Append("		<thead>\r\n");
                        templateBuilder.Append("		<tr>\r\n");
                        templateBuilder.Append("			<th id=\"divsearchoption\">搜索选项</th>\r\n");
                        templateBuilder.Append("			<td>&nbsp;</td>\r\n");
                        templateBuilder.Append("		</tr>\r\n");
                        templateBuilder.Append("		</thead>\r\n");
                        templateBuilder.Append("		<tbody id=\"divsearchtype\">\r\n");
                        templateBuilder.Append("		<tr>\r\n");
                        templateBuilder.Append("			<th><label for=\"poster\">搜索类型</label></th>\r\n");
                        templateBuilder.Append("			<td>\r\n");
                        templateBuilder.Append("			    <input type=\"hidden\" name=\"type\" value=\"post\" id=\"type\" />\r\n");
                        templateBuilder.Append("				<input name=\"keywordtype\" type=\"radio\" value=\"0\" checked onclick=\"changeoption('post');\" />\r\n");
                        templateBuilder.Append("				帖子标题搜索\r\n");

                        if (usergroupinfo.Allowsearch == 1)
                        {

                            templateBuilder.Append("					<input type=\"radio\" name=\"keywordtype\" value=\"1\" onclick=\"changeoption('post');\" />\r\n");
                            templateBuilder.Append("				内容搜索\r\n");

                        }	//end if

                        templateBuilder.Append("			</td>\r\n");
                        templateBuilder.Append("		</tr>\r\n");
                        templateBuilder.Append("		</tbody>\r\n");
                        templateBuilder.Append("	</table>\r\n");
                        templateBuilder.Append("	<div id=\"options\" style=\"margin-top:-1px;\"></div>\r\n");
                        templateBuilder.Append("	<script type=\"text/javascript\" src=\"javascript/template_search.js\"></" + "script>	\r\n");
                        templateBuilder.Append("	<table cellSpacing=\"0\" cellPadding=\"0\" summary=\"搜索类型\" style=\"margin-top:-1px;\">\r\n");
                        templateBuilder.Append("		<tbody>\r\n");
                        templateBuilder.Append("		<tr>\r\n");
                        templateBuilder.Append("			<th>&nbsp;</th>\r\n");
                        templateBuilder.Append("			<td>\r\n");
                        templateBuilder.Append("				 <input name=\"submit\" type=\"submit\" id=\"submit\" value=\"执行搜索\" />\r\n");
                        templateBuilder.Append("			</td>\r\n");
                        templateBuilder.Append("		</tr>\r\n");
                        templateBuilder.Append("		</tbody>\r\n");
                        templateBuilder.Append("	</table>\r\n");
                        templateBuilder.Append("</div>\r\n");
                        templateBuilder.Append("</form>\r\n");
                        templateBuilder.Append("</div>\r\n");

                    }
                    else
                    {
                        if (type == "")
                        {

                            templateBuilder.Append("<div class=\"pages_btns\">\r\n");
                            templateBuilder.Append("	<div class=\"pages\">\r\n");
                            templateBuilder.Append("		<em>" + pageid.ToString() + "/" + pagecount.ToString() + "页</em>" + pagenumbers.ToString() + "\r\n");
                            templateBuilder.Append("		<kbd>跳转<input name=\"gopage\" type=\"text\" id=\"gopage\" onKeyDown=\"if(event.keyCode==13) {\r\n");
                            templateBuilder.Append("	window.location='search.aspx?searchid=" + searchid.ToString() + "&page='+this.value;}\"  size=\"4\" maxlength=\"9\"/>页</kbd>\r\n");
                            templateBuilder.Append("	</div>\r\n");
                            templateBuilder.Append("	<span class=\"postbtn\">共搜索到" + topiccount.ToString() + "个符合条件的帖子</span>\r\n");
                            templateBuilder.Append("</div>\r\n");
                            templateBuilder.Append("<DIV class=\"mainbox forumlist\">\r\n");
                            templateBuilder.Append("	<h1>搜索结果</h1>\r\n");
                            templateBuilder.Append("	<table cellSpacing=\"0\" cellPadding=\"0\" summary=\"搜索结果\">\r\n");
                            templateBuilder.Append("		<thead class=\"category\">\r\n");
                            templateBuilder.Append("		<tr>\r\n");
                            templateBuilder.Append("			<th>标题</th>\r\n");
                            templateBuilder.Append("			<td>所在版块</td>\r\n");
                            templateBuilder.Append("			<td>作者</td>\r\n");
                            templateBuilder.Append("			<td class=\"nums\">回复</td>\r\n");
                            templateBuilder.Append("			<td class=\"nums\">查看</td>\r\n");
                            templateBuilder.Append("			<td>最后发表</td>\r\n");
                            templateBuilder.Append("		</tr>\r\n");
                            templateBuilder.Append("		</thead>\r\n");

                            int topic__loop__id = 0;
                            foreach (DataRow topic in topiclist.Rows)
                            {
                                topic__loop__id++;

                                templateBuilder.Append("		<tbody>\r\n");
                                templateBuilder.Append("			<tr>\r\n");
                                templateBuilder.Append("				<td>\r\n");
                                aspxrewriteurl = this.ShowTopicAspxRewrite(topic["tid"].ToString().Trim(), 0);

                                templateBuilder.Append("					<a href=\"" + aspxrewriteurl.ToString() + "\" target=\"_blank\">" + topic["title"].ToString().Trim() + "</a></td>\r\n");
                                templateBuilder.Append("				<td>\r\n");
                                aspxrewriteurl = this.ShowForumAspxRewrite(topic["fid"].ToString().Trim(), 0);

                                templateBuilder.Append("					<a href=\"" + aspxrewriteurl.ToString() + "\" target=\"_parent\">" + topic["forumname"].ToString().Trim() + "</a>\r\n");
                                templateBuilder.Append("				</td>\r\n");
                                templateBuilder.Append("				<td>\r\n");
                                templateBuilder.Append("					<h4>\r\n");

                                if (Utils.StrToInt(topic["posterid"].ToString().Trim(), 0) == -1)
                                {

                                    templateBuilder.Append("						游客\r\n");

                                }
                                else
                                {

                                    aspxrewriteurl = this.UserInfoAspxRewrite(topic["posterid"].ToString().Trim());

                                    templateBuilder.Append("						<a href=\"" + aspxrewriteurl.ToString() + "\" target=\"_parent\">" + topic["poster"].ToString().Trim() + "</a>\r\n");

                                }	//end if

                                templateBuilder.Append("</h4>\r\n");
                                templateBuilder.Append("					<em>\r\n");
                                templateBuilder.Append(Convert.ToDateTime(topic["postdatetime"].ToString().Trim()).ToString("yyyy.MM.dd HH:mm"));
                                templateBuilder.Append("</em>\r\n");
                                templateBuilder.Append("				</td>\r\n");
                                templateBuilder.Append("				<td class=\"nums\">" + topic["replies"].ToString().Trim() + "</td>\r\n");
                                templateBuilder.Append("				<td class=\"nums\">" + topic["views"].ToString().Trim() + "</td>\r\n");
                                templateBuilder.Append("				<td>\r\n");
                                templateBuilder.Append("						<em><a href=\"showtopic.aspx?topicid=" + topic["tid"].ToString().Trim() + "&page=end\" target=\"_blank\">\r\n");
                                templateBuilder.Append(Convert.ToDateTime(topic["lastpost"].ToString().Trim()).ToString("yyyy.MM.dd HH:mm"));
                                templateBuilder.Append("</a></em>\r\n");
                                templateBuilder.Append("						<cite>\r\n");

                                if (Utils.StrToInt(topic["posterid"].ToString().Trim(), 0) == -1)
                                {

                                    templateBuilder.Append("							游客\r\n");

                                }
                                else
                                {

                                    templateBuilder.Append("							<a href=\"{UserInfoAspxRewrite(topic[lastposterid])}\" target=\"_blank\">" + topic["lastposter"].ToString().Trim() + "</a>\r\n");

                                }	//end if

                                templateBuilder.Append("						</cite>\r\n");
                                templateBuilder.Append("				</td>\r\n");
                                templateBuilder.Append("			</tr>\r\n");
                                templateBuilder.Append("		</tbody>\r\n");

                            }	//end loop

                            templateBuilder.Append("	</table>\r\n");
                            templateBuilder.Append("</div>\r\n");
                            templateBuilder.Append("<div class=\"pages_btns\">\r\n");
                            templateBuilder.Append("	<div class=\"pages\">\r\n");
                            templateBuilder.Append("		<em>" + pageid.ToString() + "/" + pagecount.ToString() + "页</em>" + pagenumbers.ToString() + "\r\n");
                            templateBuilder.Append("		<kbd>跳转<input name=\"gopage\" type=\"text\" id=\"gopage\" onKeyDown=\"if(event.keyCode==13) {\r\n");
                            templateBuilder.Append("	window.location='search.aspx?searchid=" + searchid.ToString() + "&page='+this.value;}\"  size=\"4\" maxlength=\"9\"/>页</kbd>\r\n");
                            templateBuilder.Append("	</div>\r\n");
                            templateBuilder.Append("	<span class=\"postbtn\">共搜索到" + topiccount.ToString() + "个符合条件的帖子</span>\r\n");
                            templateBuilder.Append("</div>\r\n");

                        }	//end if


                        if (type == "author")
                        {

                            templateBuilder.Append("<div class=\"pages_btns\">\r\n");
                            templateBuilder.Append("	<div class=\"pages\">\r\n");
                            templateBuilder.Append("		<em>" + topicpageid.ToString() + "/" + topicpagecount.ToString() + "页</em>" + pagenumbers.ToString() + "\r\n");
                            templateBuilder.Append("		<kbd>跳转<input name=\"gopage\" type=\"text\" id=\"gopage\" onKeyDown=\"if(event.keyCode==13) {\r\n");
                            templateBuilder.Append("	window.location='search.aspx?type=author&searchid=" + searchid.ToString() + "&topicpage='+this.value;}\"  size=\"4\" maxlength=\"9\"/>页</kbd>\r\n");
                            templateBuilder.Append("	</div>\r\n");
                            templateBuilder.Append("	<span class=\"postbtn\">共搜索到" + topiccount.ToString() + "个符合条件的帖子</span>\r\n");
                            templateBuilder.Append("</div>\r\n");


                            templateBuilder.Append("<div class=\"searchtab\">\r\n");
                            templateBuilder.Append("		<a id=\"result1\" class=\"currenttab\" onmouseover=\"javascript:doClick_result(this)\" href=\"###\">帖子搜索结果</a>\r\n");


                            templateBuilder.Append("</div>\r\n");
                            

                            templateBuilder.Append("<div id=\"resultid1\" style=\"display:block;\">\r\n");
                            templateBuilder.Append("	<DIV class=\"mainbox forumlist\">\r\n");
                            templateBuilder.Append("	<h1>帖子搜索结果(共" + topiccount.ToString() + "个)</h1>\r\n");
                            templateBuilder.Append("	<table cellSpacing=\"0\" cellPadding=\"0\" summary=\"帖子搜索结果\">\r\n");
                            templateBuilder.Append("		<thead class=\"category\">\r\n");
                            templateBuilder.Append("		<tr>\r\n");
                            templateBuilder.Append("			<th>标题</th>\r\n");
                            templateBuilder.Append("			<td>所在版块</td>\r\n");
                            templateBuilder.Append("			<td>作者</td>\r\n");
                            templateBuilder.Append("			<td class=\"nums\">回复</td>\r\n");
                            templateBuilder.Append("			<td class=\"nums\">查看</td>\r\n");
                            templateBuilder.Append("			<td>最后发表</td>\r\n");
                            templateBuilder.Append("		</tr>\r\n");
                            templateBuilder.Append("		</thead>\r\n");

                            int topic__loop__id = 0;
                            foreach (DataRow topic in topiclist.Rows)
                            {
                                topic__loop__id++;

                                topicpagenumbers = topic__loop__id.ToString();

                                templateBuilder.Append("		<tbody>\r\n");
                                templateBuilder.Append("			<tr>\r\n");
                                templateBuilder.Append("				<th>\r\n");
                                aspxrewriteurl = this.ShowTopicAspxRewrite(topic["tid"].ToString().Trim(), 0);

                                templateBuilder.Append("					<a href=\"" + aspxrewriteurl.ToString() + "\" target=\"_blank\">" + topic["title"].ToString().Trim() + "</a>\r\n");
                                templateBuilder.Append("				</th>\r\n");
                                templateBuilder.Append("				<td>\r\n");
                                aspxrewriteurl = this.ShowForumAspxRewrite(topic["fid"].ToString().Trim(), 0);

                                templateBuilder.Append("					<a href=\"" + aspxrewriteurl.ToString() + "\" target=\"_parent\">" + topic["forumname"].ToString().Trim() + "</a>\r\n");
                                templateBuilder.Append("				</td>\r\n");
                                templateBuilder.Append("				<td>\r\n");
                                templateBuilder.Append("					<p>\r\n");

                                if (Utils.StrToInt(topic["posterid"].ToString().Trim(), 0) == -1)
                                {

                                    templateBuilder.Append("						游客\r\n");

                                }
                                else
                                {

                                    aspxrewriteurl = this.UserInfoAspxRewrite(topic["posterid"].ToString().Trim());

                                    templateBuilder.Append("						<a href=\"" + aspxrewriteurl.ToString() + "\" target=\"_parent\">" + topic["poster"].ToString().Trim() + "</a>\r\n");

                                }	//end if

                                templateBuilder.Append("</p>\r\n");
                                templateBuilder.Append("					<em>\r\n");
                                templateBuilder.Append(Convert.ToDateTime(topic["postdatetime"].ToString().Trim()).ToString("yyyy.MM.dd HH:mm"));
                                templateBuilder.Append("</em>\r\n");
                                templateBuilder.Append("				</td>\r\n");
                                templateBuilder.Append("				<td class=\"nums\">" + topic["replies"].ToString().Trim() + "</td>\r\n");
                                templateBuilder.Append("				<td class=\"nums\">" + topic["views"].ToString().Trim() + "</td>\r\n");
                                templateBuilder.Append("				<td>\r\n");
                                templateBuilder.Append("						<em><a href=\"showtopic.aspx?topicid=" + topic["tid"].ToString().Trim() + "&topicpage=end\" target=\"_blank\">\r\n");
                                templateBuilder.Append(Convert.ToDateTime(topic["lastpost"].ToString().Trim()).ToString("yyyy.MM.dd HH:mm"));
                                templateBuilder.Append("</a></em>\r\n");
                                templateBuilder.Append("						<cite>\r\n");

                                if (Utils.StrToInt(topic["posterid"].ToString().Trim(), 0) == -1)
                                {

                                    templateBuilder.Append("							游客\r\n");

                                }
                                else
                                {

                                    templateBuilder.Append("							<a href=\"{UserInfoAspxRewrite(topic[lastposterid])}\" target=\"_blank\">" + topic["lastposter"].ToString().Trim() + "</a>\r\n");

                                }	//end if

                                templateBuilder.Append("						</cite>\r\n");
                                templateBuilder.Append("				</td>\r\n");
                                templateBuilder.Append("			</tr>\r\n");
                                templateBuilder.Append("		</tbody>\r\n");

                            }	//end loop

                            templateBuilder.Append("	</table>\r\n");
                            templateBuilder.Append("	</div>\r\n");

                            templateBuilder.Append("<div class=\"pages_btns\">\r\n");
                            templateBuilder.Append("	<div class=\"pages\">\r\n");
                            templateBuilder.Append("		<em>" + topicpageid.ToString() + "/" + topicpagecount.ToString() + "页</em>" + pagenumbers.ToString() + "\r\n");
                            templateBuilder.Append("		<kbd>跳转<input name=\"gopage\" type=\"text\" id=\"gopage\" onKeyDown=\"if(event.keyCode==13) {\r\n");
                            templateBuilder.Append("	window.location='search.aspx?type=author&searchid=" + searchid.ToString() + "&topicpageid='+this.value;}\"  size=\"4\" maxlength=\"9\"/>页</kbd>\r\n");
                            templateBuilder.Append("	</div>\r\n");
                            templateBuilder.Append("	<span class=\"postbtn\">共搜索到" + topiccount.ToString() + "个符合条件的帖子</span>\r\n");
                            templateBuilder.Append("</div>\r\n");

                            
                            //templateBuilder.Append("	<div class=\"pages_btns\">\r\n");
                            //templateBuilder.Append("		<div class=\"pages\">\r\n");
                            //templateBuilder.Append("			<em>" + topicpageid.ToString() + "/" + topicpagecount.ToString() + "页</em>");
                            //templateBuilder.Append("		</div>\r\n");
                            //templateBuilder.Append("	</div>\r\n");
                            templateBuilder.Append("</div>\r\n");

                            templateBuilder.Append("<script type=\"text/javascript\">\r\n");
                            templateBuilder.Append("switch (getQueryString('show'))\r\n");
                            templateBuilder.Append("{	\r\n");
                            //templateBuilder.Append("	case 'album':\r\n");
                            //templateBuilder.Append("		doClick_result($('result3'));\r\n");
                            //templateBuilder.Append("		break;\r\n");
                            //templateBuilder.Append("	case 'blog':\r\n");
                            //templateBuilder.Append("		doClick_result($('result2'));\r\n");
                            //templateBuilder.Append("		break;\r\n");
                            templateBuilder.Append("	case 'topic':\r\n");
                            templateBuilder.Append("	default:\r\n");
                            templateBuilder.Append("		doClick_result($('result1'));\r\n");
                            templateBuilder.Append("		break;\r\n");
                            templateBuilder.Append("}\r\n");
                            templateBuilder.Append("function doClick_result(o){\r\n");
                            //templateBuilder.Append("	o.className=\"currenttab\";\r\n");
                            //templateBuilder.Append("	var j;\r\n");
                            //templateBuilder.Append("	var id;\r\n");
                            //templateBuilder.Append("	var e;\r\n");
                            //templateBuilder.Append("	for(var i=1;i<=3;i++){\r\n");
                            //templateBuilder.Append("		id =\"result\"+i;\r\n");
                            //templateBuilder.Append("		j = document.getElementById(id);\r\n");
                            //templateBuilder.Append("		e = document.getElementById(\"resultid\"+i);\r\n");
                            //templateBuilder.Append("		if(id != o.id){\r\n");
                            //templateBuilder.Append("			j.className=\"\";\r\n");
                            //templateBuilder.Append("			e.style.display = \"none\";\r\n");
                            //templateBuilder.Append("		}else{\r\n");
                            //templateBuilder.Append("			e.style.display = \"block\";\r\n");
                            //templateBuilder.Append("		}\r\n");
                            //templateBuilder.Append("	}\r\n");
                            templateBuilder.Append("document.getElementById('result1').style.display = \"block\";\r\n");
                            templateBuilder.Append("}\r\n");
                            templateBuilder.Append("</" + "script>\r\n");

                        }	//end if

                        templateBuilder.Append("</div>\r\n");

                    }	//end if


                }	//end if


            }
            else
            {


                templateBuilder.Append("<div class=\"box message\">\r\n");
                templateBuilder.Append("	<h1>出现了" + page_err.ToString() + "个错误</h1>\r\n");
                templateBuilder.Append("	<p>" + msgbox_text.ToString() + "</p>\r\n");
                templateBuilder.Append("	<p class=\"errorback\">\r\n");
                templateBuilder.Append("		<script type=\"text/javascript\">\r\n");
                templateBuilder.Append("			if(" + msgbox_showbacklink.ToString() + ")\r\n");
                templateBuilder.Append("			{\r\n");
                templateBuilder.Append("				document.write(\"<a href=\\\"" + msgbox_backlink.ToString() + "\\\">返回上一步</a> &nbsp; &nbsp;|&nbsp; &nbsp  \");\r\n");
                templateBuilder.Append("			}\r\n");
                templateBuilder.Append("		</" + "script>\r\n");
                templateBuilder.Append("		<a href=\"forumindex.aspx\">论坛首页</a>\r\n");

                if (usergroupid == 7)
                {

                    templateBuilder.Append("		 &nbsp; &nbsp|&nbsp; &nbsp; <a href=\"register.aspx\">注册</a>\r\n");

                }	//end if

                templateBuilder.Append("	</p>\r\n");
                templateBuilder.Append("</div>\r\n");
                templateBuilder.Append("</div>\r\n");



            }	//end if



            if (footerad != "")
            {

                templateBuilder.Append("<!--底部广告显示-->\r\n");
                templateBuilder.Append("<div id=\"ad_footerbanner\">" + footerad.ToString() + "</div>\r\n");
                templateBuilder.Append("<!--底部广告结束-->\r\n");

            }	//end if

            templateBuilder.Append(Discuz.Web.UI.PageElement.Bottom);
            templateBuilder.Append("<ul class=\"popupmenu_popup headermenu_popup\" id=\"stats_menu\" style=\"display: none\">\r\n");
            templateBuilder.Append("	<li><a href=\"stats.aspx\">基本状况</a></li>\r\n");

            if (config.Statstatus == 1)
            {

                templateBuilder.Append("	<li><a href=\"stats.aspx?type=views\">流量统计</a></li>\r\n");
                templateBuilder.Append("	<li><a href=\"stats.aspx?type=client\">客户软件</a></li>\r\n");

            }	//end if

            templateBuilder.Append("	<li><a href=\"stats.aspx?type=posts\">发帖量记录</a></li>\r\n");
            templateBuilder.Append("	<li><a href=\"stats.aspx?type=forumsrank\">版块排行</a></li>\r\n");
            templateBuilder.Append("	<li><a href=\"stats.aspx?type=topicsrank\">主题排行</a></li>\r\n");
            templateBuilder.Append("	<li><a href=\"stats.aspx?type=postsrank\">发帖排行</a></li>\r\n");
            templateBuilder.Append("	<li><a href=\"stats.aspx?type=creditsrank\">金币排行</a></li>\r\n");

            if (config.Oltimespan > 0)
            {

                templateBuilder.Append("	<li><a href=\"stats.aspx?type=onlinetime\">在线时间</a></li>\r\n");

            }	//end if

            templateBuilder.Append("</ul>\r\n");
            templateBuilder.Append("<ul class=\"popupmenu_popup headermenu_popup\" id=\"my_menu\" style=\"display: none\">\r\n");
            templateBuilder.Append("	<li><a href=\"mytopics.aspx\">我的主题</a></li>\r\n");
            templateBuilder.Append("	<li><a href=\"myposts.aspx\">我的帖子</a></li>\r\n");
            templateBuilder.Append("	<li><a href=\"search.aspx?posterid=" + userid.ToString() + "&amp;type=digest\">我的精华</a></li>\r\n");
            templateBuilder.Append("	<li><a href=\"myattachment.aspx\">我的附件</a></li>\r\n");

            if (userid > 0)
            {

                templateBuilder.Append("	<li><a href=\"usercpsubscribe.aspx\">我的收藏</a></li>\r\n");

            }	//end if

            templateBuilder.Append("	<script type=\"text/javascript\" src=\"javascript/mymenu.js\"></" + "script>\r\n");
            templateBuilder.Append("</ul>\r\n");
            templateBuilder.Append("<ul class=\"popupmenu_popup\" id=\"viewpro_menu\" style=\"display: none\">\r\n");

            if (useravatar != "")
            {

                templateBuilder.Append("		<img src=\"" + useravatar.ToString() + "\"/>\r\n");

            }	//end if

            aspxrewriteurl = this.UserInfoAspxRewrite(userid);

            templateBuilder.Append("	<li class=\"popuser\"><a href=\"" + aspxrewriteurl.ToString() + "\">我的资料</a></li>\r\n");

            templateBuilder.Append("</ul>\r\n");
            templateBuilder.Append("<div id=\"quicksearch_menu\" class=\"searchmenu\" style=\"display: none;\">\r\n");
            templateBuilder.Append("	<div onclick=\"document.getElementById('keywordtype').value='0';document.getElementById('quicksearch').innerHTML='帖子标题';document.getElementById('quicksearch_menu').style.display='none';\" onmouseover=\"MouseCursor(this);\">帖子标题</div>\r\n");


            templateBuilder.Append("	<div onclick=\"document.getElementById('keywordtype').value='8';document.getElementById('quicksearch').innerHTML='作&nbsp;&nbsp;者';document.getElementById('quicksearch_menu').style.display='none';\" onmouseover=\"MouseCursor(this);\">作&nbsp;&nbsp;者</div>\r\n");
            templateBuilder.Append("</div>\r\n");



            templateBuilder.Append("</div>\r\n");
            templateBuilder.Append("</body>\r\n");
            templateBuilder.Append("</html>\r\n");



            Response.Write(templateBuilder.ToString());
        }
    }
}
