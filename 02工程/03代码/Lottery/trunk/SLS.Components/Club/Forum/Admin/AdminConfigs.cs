using Discuz.Forum;
using Discuz.Config;

namespace Discuz.Forum
{
	/// <summary>
	/// AdminConfigFactory 的摘要说明。
	/// </summary>
	public class AdminConfigs : GeneralConfigs
	{
		/// <summary>
		/// 获取原始的缺省论坛设置
		/// </summary>
		/// <returns></returns>
        public static GeneralConfigInfo GetDefaultConifg()
		{
			GeneralConfigInfo __configinfo = new GeneralConfigInfo();

			__configinfo.Forumtitle = "论坛名称"; //论坛名称
			__configinfo.Forumurl = "/"; //论坛url地址
			__configinfo.Webtitle = "网站名称"; //网站名称
			__configinfo.Weburl = "/"; //论坛网站url地址
			__configinfo.Licensed = 1; //是否显示商业授权链接
			__configinfo.Icp = ""; //网站备案信息
			__configinfo.Closed = 0; //论坛关闭
			__configinfo.Closedreason = "抱歉!论坛暂时关闭,稍后才能访问."; //论坛关闭提示信息

			__configinfo.Passwordkey = ForumUtils.CreateAuthStr(16); //用户密码Key

			__configinfo.Regstatus = 1; //是否允许新用户注册
			__configinfo.Regadvance = 1; //注册时候是否显示高级选项
			__configinfo.Censoruser = "Administrator\r\nAdmin\r\n管理员\r\n版主"; //用户信息保留关键字
			__configinfo.Doublee = 0; //允许同一 Email 注册不同用户
			__configinfo.Regverify = 0; //新用户注册验证 0=不验证 1=email验证 2=人工验证
			__configinfo.Accessemail = ""; //Email允许地址
			__configinfo.Censoremail = ""; //Email禁止地址
			__configinfo.Hideprivate = 1; //隐藏无权访问的论坛
			__configinfo.Regctrl = 0; //IP 注册间隔限制(小时)
			__configinfo.Ipregctrl = ""; //特殊 IP 注册限制
			__configinfo.Ipaccess = ""; //IP访问列表
			__configinfo.Adminipaccess = ""; //管理员后台IP访问列表
			__configinfo.Newbiespan = 0; //新手见习期限(单位:小时)
			__configinfo.Welcomemsg = 1; //发送欢迎短消息
			__configinfo.Welcomemsgtxt = "欢迎您注册加入本论坛!"; //欢迎短消息内容
			__configinfo.Rules = 1; //是否显示注册许可协议
			__configinfo.Rulestxt = ""; //许可协议内容

			__configinfo.Templateid = 1; //默认论坛风格
			__configinfo.Hottopic = 15; //热门话题最低贴数
			__configinfo.Starthreshold = 5; //星星升级阀值
			__configinfo.Visitedforums = 10; //显示最近访问论坛数量
			__configinfo.Maxsigrows = 20; //最大签名高度(行)
			__configinfo.Moddisplay = 0; //版主显示方式 0=平面显示 1=下拉菜单
			__configinfo.Subforumsindex = 0; //首页是否显示论坛的下级子论坛
			__configinfo.Stylejump = 0; //显示风格下拉菜单
			__configinfo.Fastpost = 1; //快速发帖
			__configinfo.Showsignatures = 1; //是否显示签名
			__configinfo.Showavatars = 1; //是否显示头像
			__configinfo.Showimages = 1; //是否在帖子中显示图片

			__configinfo.Archiverstatus = 1; //启用 Archiver
			__configinfo.Seotitle = ""; //标题附加字
			__configinfo.Seokeywords = ""; //Meta Keywords
			__configinfo.Seodescription = ""; //Meta Description
			__configinfo.Seohead = ""; //其他头部信息

			__configinfo.Rssstatus = 1; //rssstatus
			__configinfo.Rssttl = 60; //RSS TTL(分钟)
			__configinfo.Nocacheheaders = 0; //禁止浏览器缓冲
			__configinfo.Fullmytopics = 0; //我的话题全文搜索 0=只搜索用户是主题发表者的主题 1=搜索用户是主题发表者或回复者的主题
			__configinfo.Debug = 1; //显示程序运行信息
			__configinfo.Rewriteurl = ""; //伪静态url的替换规则

			__configinfo.Whosonlinestatus = 3; //显示在线用户 0=不显示 1=仅在首页显示 2=仅在分论坛显示 3=在首页和分论坛显示
			__configinfo.Maxonlinelist = 300; //最多显示在线人数
			__configinfo.Userstatusby = 2; //衡量并显示用户头衔
			__configinfo.Forumjump = 1; //显示论坛跳转菜单
			__configinfo.Modworkstatus = 1; //论坛管理工作统计
			__configinfo.Maxmodworksmonths = 3; //管理记录保留时间(月)

			__configinfo.Seccodestatus = "register.aspx"; //使用验证码的页面列表,用","分隔 例如:register.aspx,login.aspx
			__configinfo.Maxonlines = 9000; //最大在线人数
			__configinfo.Postinterval = 20; //发帖灌水预防(秒)
			__configinfo.Searchctrl = 0; //搜索时间限制(秒)
			__configinfo.Maxspm = 0; //60 秒最大搜索次数

			__configinfo.Visitbanperiods = ""; //禁止访问时间段
			__configinfo.Postbanperiods = ""; //禁止发帖时间段
			__configinfo.Postmodperiods = ""; //发帖审核时间段
			__configinfo.Attachbanperiods = ""; //禁止下载附件时间段
			__configinfo.Searchbanperiods = ""; //禁止全文搜索时间段

			__configinfo.Memliststatus = 1; //允许查看会员列表
			__configinfo.Dupkarmarate = 0; //允许重复评分
			__configinfo.Minpostsize = 10; //帖子最小字数(字)
			__configinfo.Maxpostsize = 500000; //帖子最大字数(字)
			__configinfo.Tpp = 25; //每页主题数
			__configinfo.Ppp = 20; //每页帖子数
			__configinfo.Maxfavorites = 100; //收藏夹容量
			__configinfo.Maxavatarsize = 20480; //头像最大尺寸(字节)
			__configinfo.Maxavatarwidth = 120; //头像最大宽度(像素)
            __configinfo.Maxavatarheight = 120; //头像最大高度(像素);
			__configinfo.Maxpolloptions = 10; //投票最大选项数
			__configinfo.Maxattachments = 10; //最大允许的上传附件数

			__configinfo.Attachimgpost = 1; //帖子中显示图片附件
			__configinfo.Attachrefcheck = 0; //下载附件来路检查
			__configinfo.Attachsave = 3; //附件保存方式 0=全部存入同一目录 1=按论坛存入不同目录 2=按文件类型存入不同目录 3=按年月日存入不同目录
			__configinfo.Watermarkstatus = 0; //图片附件添加水印 0=不使用 1=左上 2=中上 3=右上 4=左中 ... 9=右下

			__configinfo.Karmaratelimit = 10; //评分时间限制(小时)
			__configinfo.Losslessdel = 5; //删贴不减积分时间期限(天)
			__configinfo.Edittimelimit = 0; //编辑帖子时间限制(分钟)
			__configinfo.Editedby = 1; //编辑帖子附加编辑记录
			__configinfo.Defaulteditormode = 1; //默认的编辑器模式 0=ubb代码编辑器 1=可视化编辑器
			__configinfo.Allowswitcheditor = 1; //是否允许切换编辑器模式
			__configinfo.Smileyinsert = 1; //显示可点击表情

			return __configinfo;

		}
	}
}