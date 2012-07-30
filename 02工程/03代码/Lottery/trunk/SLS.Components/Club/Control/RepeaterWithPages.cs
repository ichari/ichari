using System.Data;
using System.Web.UI;
using System.ComponentModel;

using Discuz.Common;
using Discuz.Data;

namespace Discuz.Control
{
	
	/// <summary>
	/// RepeaterWithcurrentpages 控件。
	/// </summary>
	[DefaultProperty("Text"),ToolboxData("<{0}:RepeaterWithcurrentpages runat=server></{0}:RepeaterWithcurrentpages>")]
    public class RepeaterWithPages : Discuz.Control.Repeater
	{
        /// <summary>
        /// 构造函数
        /// </summary>
	 	public RepeaterWithPages():base()
		{
		}

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="sqlstring">查询字符串</param>
		public RepeaterWithPages(string sqlstring):this()
		{
			base.AddTableData(sqlstring);
		}

	    /// <summary>
	    /// 获得记录数
	    /// </summary>
	    /// <param name="sql"></param>
	    /// <returns></returns>
		public int GetRecordCount(string sql)
		{
            return DbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0].Rows.Count;
		}

	
        /// <summary>
        /// 记录数属性
        /// </summary>
		[Bindable(true),Category("Appearance"),DefaultValue("1")] 
		public int RecordCount 
		{
			get
			{
				return (int)ViewState["RecordCount"];
			}

			set
			{
				ViewState["RecordCount"] = value;
			}
		}

        /// <summary>
        /// 页面大小属性
        /// </summary>
		[Bindable(true),Category("Appearance"),DefaultValue("16")] 
		public int PageSize 
		{
			get
			{
				return (int)ViewState["PageSize"];
			}

			set
			{
				ViewState["PageSize"] = value;
			}
		}
	
        /// <summary>
        /// 页面链接属性
        /// </summary>
		public  string PageLink
		{
			get
			{
				return (string)ViewState["PageLink"];
			}

			set
			{
				ViewState["PageLink"] = value;
			}
		}


		/// <summary> 
        /// 输出html,在浏览器中显示控件
		/// </summary>
		/// <param name="output"> 要写出到的 HTML 编写器 </param>
		protected override void Render(HtmlTextWriter output)
		{
			
			base.Render(output);
			output.WriteLine("<br />"+PageLink);
		}

        /// <summary>
        /// 分页函数
        /// </summary>
        /// <param name="currentpage">当前页数</param>
        /// <param name="url">Url参数</param>
        /// <returns></returns>
		public string Pagination(int currentpage, string url)
		{
			return Pagination(this.RecordCount,this.PageSize,currentpage,url);
		}
	
		/// <summary>
		/// 分页函数
		/// </summary>
		/// <param name="recordcount">总记录数</param>
		/// <param name="pagesize">每页记录数</param>
		/// <param name="currentpage">当前页数</param>
		/// <param name="url">Url参数</param>
        /// <returns></returns>
		public string Pagination(int recordcount, int pagesize, int currentpage, string url)
		{
			int allcurrentpage = 0;
			int next = 0;
			int pre = 0;
			int startcount = 0;
			int endcount = 0;
			string currentpagestr = "";

			if (currentpage < 1) { currentpage = 1; }
			//计算总页数
			if (pagesize != 0)
			{
				allcurrentpage = (recordcount / pagesize);
				allcurrentpage = ((recordcount % pagesize) != 0 ? allcurrentpage + 1 : allcurrentpage);
				allcurrentpage = (allcurrentpage == 0 ? 1 : allcurrentpage);
			}
			next = currentpage + 1;
			pre = currentpage - 1;
			startcount = (currentpage + 5) > allcurrentpage ? allcurrentpage - 9 : currentpage - 4;//中间页起始序号
			//中间页终止序号
			endcount = currentpage < 5 ? 10 : currentpage + 5;
			if (startcount < 1) { startcount = 1; } //为了避免输出的时候产生负数，设置如果小于1就从序号1开始
			if (allcurrentpage < endcount) { endcount = allcurrentpage; }//页码+5的可能性就会产生最终输出序号大于总页码，那么就要将其控制在页码数之内
			currentpagestr = "共" + allcurrentpage + "页&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";

			currentpagestr += currentpage > 1 ? "<a href=\"" + url + "?currentpage=1\">首页</a>&nbsp;&nbsp;<a href=\"" + url + "?currentpage=" + pre + "\">上一页</a>" : "首页 上一页";
			//中间页处理，这个增加时间复杂度，减小空间复杂度
			for (int i = startcount; i <= endcount; i++)
			{
				currentpagestr += currentpage == i ? "&nbsp;&nbsp;<font color=\"#ff0000\">" + i + "</font>" : "&nbsp;&nbsp;<a href=\"" + url + "?currentpage=" + i + "\">" + i + "</a>";
			}
			currentpagestr += currentpage != allcurrentpage ? "&nbsp;&nbsp;<a href=\"" + url + "?currentpage=" + next + "\">下一页</a>&nbsp;&nbsp;<a href=\"" + url + "?currentpage=" + allcurrentpage + "\">末页</a>" : " 下一页 末页";
            PageLink=currentpagestr;
			return currentpagestr;
		}
	
	}
}
