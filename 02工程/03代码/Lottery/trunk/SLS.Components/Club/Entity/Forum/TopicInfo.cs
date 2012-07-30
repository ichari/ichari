using System;

namespace Discuz.Entity
{
	/// <summary>
	/// 主题信息描述类
	/// </summary>
	public class TopicInfo
    {
        #region 成员变量
        private int m_tid;	//主题tid
		private int m_fid;	//版块fid
		private int m_iconid;	//主题图标id
		private int m_typeid;	//主题分类id
		private int m_readperm;	//阅读权限
		private int m_price;	//主题出售价格积分
		private string m_poster;	//作者
		private int m_posterid;	//作者uid
		private string m_title;	//标题
		private string m_postdatetime;	//发布时间
		private string m_lastpost;	//最后回复时间
		private int m_lastpostid;	//最后回复帖子ID
		private string m_lastposter;	//最后回复用户名
		private int m_lastposterid;	//最后回复用户名ID
		private int m_views;	//查看数
		private int m_replies;	//回复数
		private int m_displayorder;	//>0为置顶,<0不显示,==0正常   -1为回收站   -2待审核
		private string m_highlight;	//主题高亮识别号
		private int m_digest;	//精华级别,1~3
		private int m_rate;	//评分分数
		private int m_hide;	//是否为回复可见帖
        //private int m_poll;	//是否是投票贴
		private int m_attachment;	//是否含有附件
		private int m_moderated;	//是否被执行管理操作
		private int m_closed;	//是否关闭,如果数值>1,值代表转向目标主题的tid
		private int m_magic;	//魔法id
        private int m_identify; //鉴定id
        private byte m_special;//主题类型, 0=普通主题, 1=投票帖, 2=正在进行的悬赏帖, 3=结束的悬赏帖, 4=辩论帖
        #endregion 成员变量

        #region 属性
        ///<summary>
		///主题tid
		///</summary>
		public int Tid
		{
			get { return m_tid;}
			set { m_tid = value;}
		}
		///<summary>
		///版块fid
		///</summary>
		public int Fid
		{
			get { return m_fid;}
			set { m_fid = value;}
		}
		///<summary>
		///主题图标id
		///</summary>
		public int Iconid
		{
			get { return m_iconid;}
			set { m_iconid = value;}
		}
		///<summary>
		///主题分类id
		///</summary>
		public int Typeid
		{
			get { return m_typeid;}
			set { m_typeid = value;}
		}
		///<summary>
		///阅读权限
		///</summary>
		public int Readperm
		{
			get { return m_readperm;}
			set { m_readperm = value;}
		}
		///<summary>
		///主题出售价格积分
		///</summary>
		public int Price
		{
			get { return m_price;}
			set { m_price = value;}
		}
		///<summary>
		///作者
		///</summary>
		public string Poster
		{
			get { return m_poster.Trim();}
			set { m_poster = value;}
		}
		///<summary>
		///作者uid
		///</summary>
		public int Posterid
		{
			get { return m_posterid;}
			set { m_posterid = value;}
		}
		///<summary>
		///标题
		///</summary>
		public string Title
		{
			get { return m_title.Trim();}
			set { m_title = value;}
		}
		///<summary>
		///发布时间
		///</summary>
		public string Postdatetime
		{
			get { return m_postdatetime;}
			set { m_postdatetime = value;}
		}
		///<summary>
		///最后回复时间
		///</summary>
		public string Lastpost
		{
			get { return m_lastpost;}
			set { m_lastpost = value;}
		}
		///<summary>
		///最后回复帖子ID
		///</summary>
		public int Lastpostid
		{
			get { return m_lastpostid;}
			set { m_lastpostid = value;}
		}
		///<summary>
		///最后回复用户名
		///</summary>
		public string Lastposter
		{
			get { return m_lastposter;}
			set { m_lastposter = value;}
		}

		///<summary>
		///最后回复用户名ID
		///</summary>
		public int Lastposterid
		{
			get { return m_lastposterid;}
			set { m_lastposterid = value;}
		}
		///<summary>
		///查看数
		///</summary>
		public int Views
		{
			get { return m_views;}
			set { m_views = value;}
		}
		///<summary>
		///回复数
		///</summary>
		public int Replies
		{
			get { return m_replies;}
			set { m_replies = value;}
		}
		///<summary>
		///>0为置顶,<0不显示,==0正常
		///</summary>
		public int Displayorder
		{
			get { return m_displayorder;}
			set { m_displayorder = value;}
		}
		///<summary>
		///主题高亮识别号
		///</summary>
		public string Highlight
		{
			get { return m_highlight;}
			set { m_highlight = value;}
		}
		///<summary>
		///精华级别,1~3
		///</summary>
		public int Digest
		{
			get { return m_digest;}
			set { m_digest = value;}
		}
		///<summary>
		///评分分数
		///</summary>
		public int Rate
		{
			get { return m_rate;}
			set { m_rate = value;}
		}
		///<summary>
		///是否为回复可见帖
		///</summary>
		public int Hide
		{
			get { return m_hide;}
			set { m_hide = value;}
		}
		///<summary>
		///是否是投票贴
		///</summary>
        //public int Poll
        //{
        //    get { return m_poll;}
        //    set { m_poll = value;}
        //}
		///<summary>
		///是否含有附件
		///</summary>
		public int Attachment
		{
			get { return m_attachment;}
			set { m_attachment = value;}
		}
		///<summary>
		///是否被执行管理操作
		///</summary>
		public int Moderated
		{
			get { return m_moderated;}
			set { m_moderated = value;}
		}

		///<summary>
		///是否关闭,如果数值>1,值代表转向目标主题的tid
		///</summary>
		public int Closed
		{
			get { return m_closed;}
			set { m_closed = value;}
		}

		///<summary>
        ///魔法id,按照附加位/htmltitle(1位)/magic(3位)/tag(1位)/以后扩展（未知位数） 的方式来存储
		///</summary>
		public int Magic
		{
			get { return m_magic;}
			set { m_magic = value;}
		}

        /// <summary>
        /// 鉴定Id
        /// </summary>
        public int Identify
        {
            get { return m_identify; }
            set { m_identify = value; }
        }

        /// <summary>
        /// 0=普通主题, 1=投票帖, 2=正在进行的悬赏帖, 3=结束的悬赏帖, 4=辩论帖
        /// </summary>
        public byte Special
        {
            get { return m_special; }
            set { m_special = value; }
        }
        #endregion 属性
    }
}
