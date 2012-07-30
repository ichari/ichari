using System;
using System.Data;
using System.Text;

#if NET1
#else
using Discuz.Common.Generic;
#endif

using Discuz.Entity;
using System.Data.Common;

namespace Discuz.Data
{
    public interface IDataProvider
    {
        /// <summary>
        /// 添加广告信息
        /// </summary>
        /// <param name="available"></param>
        /// <param name="type"></param>
        /// <param name="displayorder"></param>
        /// <param name="title"></param>
        /// <param name="targets"></param>
        /// <param name="parameters"></param>
        /// <param name="code"></param>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        void AddAdInfo(int available, string type, int displayorder, string title, string targets, string parameters,
                       string code, string starttime, string endtime);
        /// <summary>
        /// 添加通告
        /// </summary>
        /// <param name="poster"></param>
        /// <param name="posterid"></param>
        /// <param name="title"></param>
        /// <param name="displayorder"></param>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        int AddAnnouncement(string poster, int posterid, string title, int displayorder, string starttime,
                            string endtime, string message);
        /// <summary>
        /// 添加附件类型
        /// </summary>
        /// <param name="extension"></param>
        /// <param name="maxsize"></param>
        void AddAttchType(string extension, string maxsize);
        /// <summary>
        /// 添加自定义Discuz!NT代码
        /// </summary>
        /// <param name="available"></param>
        /// <param name="tag"></param>
        /// <param name="icon"></param>
        /// <param name="replacement"></param>
        /// <param name="example"></param>
        /// <param name="explanation"></param>
        /// <param name="param"></param>
        /// <param name="nest"></param>
        /// <param name="paramsdescript"></param>
        /// <param name="paramsdefvalue"></param>
        void AddBBCCode(int available, string tag, string icon, string replacement, string example, string explanation,
                        string param, string nest, string paramsdescript, string paramsdefvalue);
        /// <summary>
        /// 添加积分日志
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="fromto"></param>
        /// <param name="sendcredits"></param>
        /// <param name="receivecredits"></param>
        /// <param name="send"></param>
        /// <param name="receive"></param>
        /// <param name="paydate"></param>
        /// <param name="operation"></param>
        /// <returns></returns>
        int AddCreditsLog(int uid, int fromto, int sendcredits, int receivecredits, float send, float receive,
                          string paydate, int operation);
        /// <summary>
        /// 添加错误登录次数
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        int AddErrLoginCount(string ip);
        /// <summary>
        /// 添加错误登录记录
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        int AddErrLoginRecord(string ip);
        /// <summary>
        /// 添加友情链接
        /// </summary>
        /// <param name="displayorder"></param>
        /// <param name="name"></param>
        /// <param name="url"></param>
        /// <param name="note"></param>
        /// <param name="logo"></param>
        /// <returns></returns>
        int AddForumLink(int displayorder, string name, string url, string note, string logo);
        /// <summary>
        /// 添加勋章
        /// </summary>
        /// <param name="medalid"></param>
        /// <param name="name"></param>
        /// <param name="available"></param>
        /// <param name="image"></param>
        void AddMedal(int medalid, string name, int available, string image);
        /// <summary>
        /// 添加勋章日志
        /// </summary>
        /// <param name="adminid"></param>
        /// <param name="adminname"></param>
        /// <param name="ip"></param>
        /// <param name="username"></param>
        /// <param name="uid"></param>
        /// <param name="actions"></param>
        /// <param name="medals"></param>
        /// <param name="reason"></param>
        void AddMedalslog(int adminid, string adminname, string ip, string username, int uid, string actions, int medals,
                          string reason);
        /// <summary>
        /// 添加版主
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="fid"></param>
        /// <param name="displayorder"></param>
        /// <param name="inherited"></param>
        void AddModerator(int uid, int fid, int displayorder, int inherited);
        /// <summary>
        /// 添加在线用户组图例
        /// </summary>
        /// <param name="grouptitle"></param>
        void AddOnlineList(string grouptitle);
        /// <summary>
        /// 添加在线用户
        /// </summary>
        /// <param name="onlineuserinfo"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        int AddOnlineUser(OnlineUserInfo onlineuserinfo, int timeout);
        /// <summary>
        /// 增加父版块主题数
        /// </summary>
        /// <param name="fpidlist"></param>
        /// <param name="topics"></param>
        /// <param name="posts"></param>
        void AddParentForumTopics(string fpidlist, int topics, int posts);
        /// <summary>
        /// 添加积分交易日志
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="tid"></param>
        /// <param name="posterid"></param>
        /// <param name="price"></param>
        /// <param name="netamount"></param>
        /// <returns></returns>
        int AddPaymentLog(int uid, int tid, int posterid, int price, float netamount);
        /// <summary>
        /// 添加帖子分表
        /// </summary>
        /// <param name="description"></param>
        /// <param name="mintid"></param>
        /// <param name="maxtid"></param>
        void AddPostTableToTableList(string description, int mintid, int maxtid);
        /// <summary>
        /// 添加表情
        /// </summary>
        /// <param name="id"></param>
        /// <param name="displayorder"></param>
        /// <param name="type"></param>
        /// <param name="code"></param>
        /// <param name="url"></param>
        void AddSmiles(int id, int displayorder, int type, string code, string url);
        /// <summary>
        /// 添加模板
        /// </summary>
        /// <param name="templateName"></param>
        /// <param name="directory"></param>
        /// <param name="copyright"></param>
        /// <returns></returns>
        int AddTemplate(string templateName, string directory, string copyright);
        /// <summary>
        /// 添加模板
        /// </summary>
        /// <param name="name"></param>
        /// <param name="directory"></param>
        /// <param name="copyright"></param>
        /// <param name="author"></param>
        /// <param name="createdate"></param>
        /// <param name="ver"></param>
        /// <param name="fordntver"></param>
        void AddTemplate(string name, string directory, string copyright, string author, string createdate, string ver,
                         string fordntver);
        /// <summary>
        /// 添加用户组
        /// </summary>
        /// <param name="usergroupinfo"></param>
        /// <param name="Creditshigher"></param>
        /// <param name="Creditslower"></param>
        void AddUserGroup(UserGroupInfo usergroupinfo, int Creditshigher, int Creditslower);
        /// <summary>
        /// 添加访问日志
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="username"></param>
        /// <param name="groupid"></param>
        /// <param name="grouptitle"></param>
        /// <param name="ip"></param>
        /// <param name="actions"></param>
        /// <param name="others"></param>
        void AddVisitLog(int uid, string username, int groupid, string grouptitle, string ip, string actions,
                         string others);
        /// <summary>
        /// 添加词语过滤
        /// </summary>
        /// <param name="username"></param>
        /// <param name="find"></param>
        /// <param name="replacement"></param>
        /// <returns></returns>
        int AddWord(string username, string find, string replacement);
        /// <summary>
        /// 备份数据库
        /// </summary>
        /// <param name="backuppath"></param>
        /// <param name="ServerName"></param>
        /// <param name="UserName"></param>
        /// <param name="Password"></param>
        /// <param name="strDbName"></param>
        /// <param name="strFileName"></param>
        /// <returns></returns>
        string BackUpDatabase(string backuppath, string ServerName, string UserName, string Password, string strDbName,
                              string strFileName);
        /// <summary>
        /// 批量设置版块信息
        /// </summary>
        /// <param name="foruminfo"></param>
        /// <param name="bsp"></param>
        /// <param name="fidlist"></param>
        /// <returns></returns>
        bool BatchSetForumInf(ForumInfo foruminfo, BatchSetParams bsp, string fidlist);
        /// <summary>
        /// 购买主题
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="tid"></param>
        /// <param name="posterid"></param>
        /// <param name="price"></param>
        /// <param name="netamount"></param>
        /// <param name="creditsTrans"></param>
        void BuyTopic(int uid, int tid, int posterid, int price, float netamount, int creditsTrans);
        /// <summary>
        /// 更改用户管理权限Id
        /// </summary>
        /// <param name="adminid"></param>
        /// <param name="groupid"></param>
        void ChangeUserAdminidByGroupid(int adminid, int groupid);
        /// <summary>
        /// 更改用户组
        /// </summary>
        /// <param name="soureceusergroupid"></param>
        /// <param name="targetusergroupid"></param>
        void ChangeUsergroup(int soureceusergroupid, int targetusergroupid);
        /// <summary>
        /// 更改用户组
        /// </summary>
        /// <param name="groupid"></param>
        /// <param name="uidlist"></param>
        void ChangeUserGroupByUid(int groupid, string uidlist);
        /// <summary>
        /// 检查动网兼容模式密码和安全问题
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        IDataReader CheckDvBbsPasswordAndSecques(string username, string password);
        /// <summary>
        /// 检查Email和安全问题
        /// </summary>
        /// <param name="username"></param>
        /// <param name="email"></param>
        /// <param name="secques"></param>
        /// <returns></returns>
        IDataReader CheckEmailAndSecques(string username, string email, string secques);
        /// <summary>
        /// 检查收藏是否已经存在
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="tid"></param>
        /// <returns></returns>
        int CheckFavoritesIsIN(int uid, int tid);
        /// <summary>
        /// 检查收藏是否已经存在
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="tid"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        int CheckFavoritesIsIN(int uid, int tid, byte type);
        /// <summary>
        /// 检查密码
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="originalpassword"></param>
        /// <returns></returns>
        IDataReader CheckPassword(string username, string password, bool originalpassword);
        /// <summary>
        /// 检查密码
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="password"></param>
        /// <param name="originalpassword"></param>
        /// <returns></returns>
        IDataReader CheckPassword(int uid, string password, bool originalpassword);
        /// <summary>
        /// 检查密码和安全问题
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="originalpassword"></param>
        /// <param name="secques"></param>
        /// <returns></returns>
        IDataReader CheckPasswordAndSecques(string username, string password, bool originalpassword, string secques);
        /// <summary>
        /// 检查评分状态
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="pid"></param>
        /// <returns></returns>
        string CheckRateState(int userid, string pid);
        /// <summary>
        /// 检查用户积分是否足够
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="values"></param>
        /// <param name="pos"></param>
        /// <param name="mount"></param>
        /// <returns></returns>
        bool CheckUserCreditsIsEnough(int uid, DataRow values, int pos, int mount);
        /// <summary>
        /// 检查用户积分是否足够
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        bool CheckUserCreditsIsEnough(int uid, float[] values);
        /// <summary>
        /// 检查用户验证码
        /// </summary>
        /// <param name="olid"></param>
        /// <param name="verifycode"></param>
        /// <param name="newverifycode"></param>
        /// <returns></returns>
        bool CheckUserVerifyCode(int olid, string verifycode, string newverifycode);
        /// <summary>
        /// 清空所有用户的认证串
        /// </summary>
        void ClearAllUserAuthstr();
        /// <summary>
        /// 清空指定用户的认证串
        /// </summary>
        /// <param name="uidlist"></param>
        void ClearAuthstrByUidlist(string uidlist);
        /// <summary>
        /// 清空数据库日志
        /// </summary>
        /// <param name="dbname"></param>
        void ClearDBLog(string dbname);
        /// <summary>
        /// 清除用户所发的帖子
        /// </summary>
        /// <param name="uid"></param>
        void ClearPosts(int uid);
        /// <summary>
        /// 合并版块
        /// </summary>
        /// <param name="sourcefid"></param>
        /// <param name="targetfid"></param>
        /// <param name="fidlist"></param>
        void CombinationForums(string sourcefid, string targetfid, string fidlist);
        /// <summary>
        /// 合并用户
        /// </summary>
        /// <param name="posttablename"></param>
        /// <param name="targetuserinfo"></param>
        /// <param name="srcuserinfo"></param>
        void CombinationUser(string posttablename, UserInfo targetuserinfo, UserInfo srcuserinfo);
        /// <summary>
        /// 确认全文索引是否启用
        /// </summary>
        void ConfirmFullTextEnable();
        /// <summary>
        /// 复制主题链接
        /// </summary>
        /// <param name="oldfid"></param>
        /// <param name="topiclist"></param>
        /// <returns></returns>
        int CopyTopicLink(int oldfid, string topiclist);
        /// <summary>
        /// 创建管理组
        /// </summary>
        /// <param name="admingroupsInfo"></param>
        /// <returns></returns>
        int CreateAdminGroupInfo(AdminGroupInfo admingroupsInfo);
        /// <summary>
        /// 创建附件
        /// </summary>
        /// <param name="attachmentinfo"></param>
        /// <returns></returns>
        int CreateAttachment(AttachmentInfo attachmentinfo);
        /// <summary>
        /// 创建收藏
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="tid"></param>
        /// <returns></returns>
        int CreateFavorites(int uid, int tid);
        /// <summary>
        /// 创建收藏
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="tid"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        int CreateFavorites(int uid, int tid, byte type);
        /// <summary>
        /// 创建全文索引
        /// </summary>
        /// <param name="dbname"></param>
        /// <returns></returns>
        int CreateFullTextIndex(string dbname);
        /// <summary>
        /// 创建在线表
        /// </summary>
        /// <returns></returns>
        int CreateOnlineTable();
        /// <summary>
        /// 创建或填充索引
        /// </summary>
        /// <param name="DbName"></param>
        /// <param name="postsid"></param>
        /// <returns></returns>
        bool CreateORFillIndex(string DbName, string postsid);
        /// <summary>
        /// 创建投票
        /// </summary>
        /// <param name="tid"></param>
        /// <param name="polltype"></param>
        /// <param name="itemcount"></param>
        /// <param name="itemnamelist"></param>
        /// <param name="itemvaluelist"></param>
        /// <param name="enddatetime"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        ////bool CreatePoll(int tid, int polltype, int itemcount, string itemnamelist, string itemvaluelist,
        //string enddatetime, int userid);
        int CreatePoll(PollInfo pollinfo);
        int CreatePollOption(PollOptionInfo polloptioninfo);
        bool UpdatePollOption(PollOptionInfo polloptioninfo);
        /// <summary>
        /// 删除指定的投票项
        /// </summary>
        /// <param name="polloptioninfo"></param>
        bool DeletePollOption(PollOptionInfo polloptioninfo);
        /// <summary>
        /// 创建帖子
        /// </summary>
        /// <param name="postinfo"></param>
        /// <param name="posttableid"></param>
        /// <returns></returns>
        int CreatePost(PostInfo postinfo, string posttableid);
        /// <summary>
        /// 创建帖子存储过程
        /// </summary>
        /// <param name="sqltemplate"></param>
        void CreatePostProcedure(string sqltemplate);
        /// <summary>
        /// 创建帖子表
        /// </summary>
        /// <param name="tablename"></param>
        void CreatePostTableAndIndex(string tablename);
        /// <summary>
        /// 创建短消息
        /// </summary>
        /// <param name="privatemessageinfo"></param>
        /// <param name="savetosentbox"></param>
        /// <returns></returns>
        int CreatePrivateMessage(PrivateMessageInfo __privatemessageinfo, int savetosentbox);
        /// <summary>
        /// 创建搜索缓存
        /// </summary>
        /// <param name="cacheinfo"></param>
        /// <returns></returns>
        int CreateSearchCache(SearchCacheInfo cacheinfo);
        /// <summary>
        /// 创建主题
        /// </summary>
        /// <param name="topicinfo"></param>
        /// <returns></returns>
        int CreateTopic(TopicInfo topicinfo);
        /// <summary>
        /// 创建用户
        /// </summary>
        /// <param name="userinfo"></param>
        /// <returns></returns>
        int CreateUser(UserInfo __userinfo);
        /// <summary>
        /// 减少新的短消息数
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="subval"></param>
        /// <returns></returns>
        int DecreaseNewPMCount(int uid, int subval);
        /// <summary>
        /// 删除管理组
        /// </summary>
        /// <param name="groupid"></param>
        void DeleteAdminGroup(int groupid);
        /// <summary>
        /// 删除管理组
        /// </summary>
        /// <param name="admingid"></param>
        /// <returns></returns>
        int DeleteAdminGroupInfo(short admingid);
        /// <summary>
        /// 删除管理组
        /// </summary>
        /// <param name="admingid"></param>
        void DeleteAdminGroupInfo(int admingid);
        /// <summary>
        /// 删除广告
        /// </summary>
        /// <param name="aidlist"></param>
        void DeleteAdvertisement(string aidlist);
        /// <summary>
        /// 删除通告
        /// </summary>
        /// <param name="id"></param>
        void DeleteAnnouncement(int id);
        /// <summary>
        /// 删除通告
        /// </summary>
        /// <param name="idlist"></param>
        void DeleteAnnouncements(string idlist);
        /// <summary>
        /// 删除附件
        /// </summary>
        /// <param name="aidList"></param>
        /// <returns></returns>
        int DeleteAttachment(string aidList);
        /// <summary>
        /// 删除附件
        /// </summary>
        /// <param name="aid"></param>
        /// <returns></returns>
        int DeleteAttachment(int aid);
        /// <summary>
        /// 删除附件
        /// </summary>
        /// <param name="tid"></param>
        /// <returns></returns>
        int DeleteAttachmentByTid(int tid);
        /// <summary>
        /// 删除附件
        /// </summary>
        /// <param name="tidlist"></param>
        /// <returns></returns>
        int DeleteAttachmentByTid(string tidlist);
        /// <summary>
        /// 删除附件类型
        /// </summary>
        /// <param name="attchtypeidlist"></param>
        void DeleteAttchType(string attchtypeidlist);
        /// <summary>
        /// 删除待认证用户
        /// </summary>
        void DeleteAuditUser();
        /// <summary>
        /// 删除自定义Discuz!NT代码
        /// </summary>
        /// <param name="idlist"></param>
        void DeleteBBCode(string idlist);
        /// <summary>
        /// 删除关闭的主题
        /// </summary>
        /// <param name="fid"></param>
        /// <param name="topiclist"></param>
        /// <returns></returns>
        int DeleteClosedTopics(int fid, string topiclist);
        /// <summary>
        /// 删除错误登录日志
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        int DeleteErrLoginRecord(string ip);
        /// <summary>
        /// 删除过期的搜索缓存
        /// </summary>
        void DeleteExpriedSearchCache();
        /// <summary>
        /// 删除收藏
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="fidlist"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        int DeleteFavorites(int uid, string fidlist, byte type);
        /// <summary>
        /// 删除友情链接
        /// </summary>
        /// <param name="forumlinkidlist"></param>
        /// <returns></returns>
        int DeleteForumLink(string forumlinkidlist);
        /// <summary>
        /// 删除板块
        /// </summary>
        /// <param name="postname"></param>
        /// <param name="fid"></param>
        void DeleteForumsByFid(string postname, string fid);
        /// <summary>
        /// 删除勋章
        /// </summary>
        /// <param name="medailidlist"></param>
        void DeleteMedalById(string medailidlist);
        /// <summary>
        /// 删除勋章日志
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        bool DeleteMedalLog(string condition);
        /// <summary>
        /// 删除勋章日志
        /// </summary>
        /// <returns></returns>
        bool DeleteMedalLog();
        /// <summary>
        /// 删除版主
        /// </summary>
        /// <param name="uid"></param>
        void DeleteModerator(int uid);
        /// <summary>
        /// 删除版主
        /// </summary>
        /// <param name="fid"></param>
        void DeleteModeratorByFid(int fid);
        /// <summary>
        /// 删除版主日志
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        bool DeleteModeratorLog(string condition);
        /// <summary>
        /// 删除在线图例
        /// </summary>
        /// <param name="groupid"></param>
        void DeleteOnlineList(int groupid);
        /// <summary>
        /// 删除交易日志
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        bool DeletePaymentLog(string condition);
        /// <summary>
        /// 删除交易日志
        /// </summary>
        /// <returns></returns>
        bool DeletePaymentLog();
        /// <summary>
        /// 删除帖子
        /// </summary>
        /// <param name="posttableid"></param>
        /// <param name="pid"></param>
        /// <returns></returns>
        int DeletePost(string posttableid, int pid, bool chanageposts);
        /// <summary>
        /// 删除帖子
        /// </summary>
        /// <param name="tabid"></param>
        /// <param name="posterid"></param>
        void DeletePostByPosterid(int tabid, int posterid);
        /// <summary>
        /// 删除短消息
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="pmid"></param>
        /// <returns></returns>
        int DeletePrivateMessage(int userid, int pmid);
        /// <summary>
        /// 删除短消息
        /// </summary>
        /// <param name="isnew"></param>
        /// <param name="postdatetime"></param>
        /// <param name="msgfromlist"></param>
        /// <param name="lowerupper"></param>
        /// <param name="subject"></param>
        /// <param name="message"></param>
        /// <param name="isupdateusernewpm"></param>
        /// <returns></returns>
        string DeletePrivateMessages(bool isnew, string postdatetime, string msgfromlist, bool lowerupper,
                                     string subject, string message, bool isupdateusernewpm);
        /// <summary>
        /// 删除短消息
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="pmidlist"></param>
        /// <returns></returns>
        int DeletePrivateMessages(int userid, string pmidlist);
        /// <summary>
        /// 删除评分日志
        /// </summary>
        /// <returns></returns>
        bool DeleteRateLog();
        /// <summary>
        /// 删除评分日志
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        bool DeleteRateLog(string condition);
        /// <summary>
        /// 删除在线表中的一行
        /// </summary>
        /// <param name="olid"></param>
        /// <returns></returns>
        int DeleteRows(int olid);
        /// <summary>
        /// 删除在线表中的一行
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        int DeleteRowsByIP(string ip);
        /// <summary>
        /// 删除表情
        /// </summary>
        /// <param name="idlist"></param>
        /// <returns></returns>
        int DeleteSmilies(string idlist);
        /// <summary>
        /// 删除表情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        string DeleteSmily(int id);
        /// <summary>
        /// 删除模板项
        /// </summary>
        /// <param name="templateidlist"></param>
        void DeleteTemplateItem(string templateidlist);
        /// <summary>
        /// 删除模板项
        /// </summary>
        /// <param name="templateid"></param>
        void DeleteTemplateItem(int templateid);
        /// <summary>
        /// 删除主题
        /// </summary>
        /// <param name="tid"></param>
        /// <returns></returns>
        int DeleteTopic(int tid);
        /// <summary>
        /// 删除主题
        /// </summary>
        /// <param name="posterid"></param>
        void DeleteTopicByPosterid(int posterid);
        /// <summary>
        /// 删除主题
        /// </summary>
        /// <param name="tid"></param>
        /// <param name="posttablename"></param>
        /// <returns></returns>
        bool DeleteTopicByTid(int tid, string posttablename);
        /// <summary>
        /// 删除指定主题
        /// </summary>
        /// <param name="topiclist">要删除的主题ID列表</param>
        /// <param name="posttableid">所以分表的ID</param>
        /// <param name="chanageposts">删除帖时是否要减版块帖数</param>
        /// <returns></returns>
        int DeleteTopicByTidList(string topiclist, string posttableid,bool chanageposts);
        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="uidlist"></param>
        void DeleteUserByUidlist(string uidlist);
        /// <summary>
        /// 删除用户组
        /// </summary>
        /// <param name="groupid"></param>
        void DeleteUserGroup(int groupid);
        /// <summary>
        /// 删除用户组
        /// </summary>
        /// <param name="groupid"></param>
        void DeleteUserGroupInfo(int groupid);
        /// <summary>
        /// 合并用户组
        /// </summary>
        /// <param name="sourceusergroupid"></param>
        /// <param name="targetusergroupid"></param>
        void CombinationUsergroupScore(int sourceusergroupid, int targetusergroupid);
        /// <summary>
        /// 删除访问日志
        /// </summary>
        void DeleteVisitLogs();
        /// <summary>
        /// 删除访问日志
        /// </summary>
        /// <param name="condition"></param>
        void DeleteVisitLogs(string condition);
        /// <summary>
        /// 删除词语过滤
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        int DeleteWord(int id);
        /// <summary>
        /// 删除词语过滤
        /// </summary>
        /// <param name="idlist"></param>
        /// <returns></returns>
        int DeleteWords(string idlist);
        /// <summary>
        /// 删除用户信息
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="delposts"></param>
        /// <param name="delpms"></param>
        /// <returns></returns>
        bool DelUserAllInf(int uid, bool delposts, bool delpms);
        /// <summary>
        /// 是否存在用户
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        bool Exists(int uid);
        /// <summary>
        /// 是否存在用户
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        bool Exists(string username);
        /// <summary>
        /// 是否存在用户
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        bool ExistsByIP(string ip);
        /// <summary>
        /// 是否存在过滤词
        /// </summary>
        /// <param name="find"></param>
        /// <returns></returns>
        bool ExistWord(string find);
        /// <summary>
        /// 查找用户Email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        IDataReader FindUserEmail(string email);
        /// <summary>
        /// 判断管理组是否存在
        /// </summary>
        /// <param name="admingid"></param>
        /// <returns></returns>
        DataTable GetAdmingid(int admingid);
        /// <summary>
        /// 获取管理组信息
        /// </summary>
        /// <param name="groupid"></param>
        /// <returns></returns>
        DataTable GetAdminGroup(int groupid);
        /// <summary>
        /// 获取管理组信息
        /// </summary>
        /// <param name="admingid"></param>
        /// <returns></returns>
        DataTable GetAdmingroupByAdmingid(int admingid);
        /// <summary>
        /// 获取管理组信息SQL语句
        /// </summary>
        /// <returns></returns>
        string GetAdminGroupInfoSql();
        /// <summary>
        /// 获取管理组列表
        /// </summary>
        /// <returns></returns>
        DataTable GetAdminGroupList();
        /// <summary>
        /// 获取管理组名称
        /// </summary>
        /// <returns></returns>
        string GetAdminUserGroupTitle();
        /// <summary>
        /// 获取广告
        /// </summary>
        /// <returns></returns>
        DataTable GetAdsTable();
        /// <summary>
        /// 获取广告
        /// </summary>
        /// <param name="aid"></param>
        /// <returns></returns>
        DataTable GetAdvertisement(int aid);
        /// <summary>
        /// 获取广告
        /// </summary>
        /// <returns></returns>
        string GetAdvertisements();
        /// <summary>
        /// 获取所有板块列表
        /// </summary>
        /// <returns></returns>
        DataTable GetAllForumList();
        /// <summary>
        /// 获取板块统计信息
        /// </summary>
        /// <returns></returns>
        IDataReader GetAllForumStatistics();
        /// <summary>
        /// 获取所有帖子分表名
        /// </summary>
        /// <returns></returns>
        DataSet GetAllPostTableName();
        /// <summary>
        /// 获取模板列表
        /// </summary>
        /// <param name="templatePath"></param>
        /// <returns></returns>
        DataTable GetAllTemplateList(string templatePath);
        /// <summary>
        /// 获取主题数
        /// </summary>
        /// <param name="fid"></param>
        /// <returns></returns>
        int GetAllTopicCount(int fid);
        /// <summary>
        /// 获取主题数
        /// </summary>
        /// <param name="fid"></param>
        /// <param name="state"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        int GetTopicCount(int fid, int state, string condition);
        /// <summary>
        /// 获取通告
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        DataTable GetAnnouncement(int id);
        /// <summary>
        /// 获取通告
        /// </summary>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <returns></returns>
        DataTable GetAnnouncementList(string starttime, string endtime);
        /// <summary>
        /// 获取通告
        /// </summary>
        /// <returns></returns>
        string GetAnnouncements();
        /// <summary>
        /// 获取简洁版首页版块列表
        /// </summary>
        /// <returns></returns>
        DataTable GetArchiverForumIndexList();
        /// <summary>
        /// 获取附件数
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        int GetAttachCount(int pid);
        /// <summary>
        /// 获取附件数
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        int GetAttachmentCountByPid(int pid);
        /// <summary>
        /// 获取附件数
        /// </summary>
        /// <param name="tid"></param>
        /// <returns></returns>
        int GetAttachmentCountByTid(int tid);
        /// <summary>
        /// 获取附件信息
        /// </summary>
        /// <param name="aid"></param>
        /// <returns></returns>
        IDataReader GetAttachmentInfo(int aid);
        /// <summary>
        /// 获取附件列表
        /// </summary>
        /// <param name="aidList"></param>
        /// <returns></returns>
        IDataReader GetAttachmentList(string aidList);
        /// <summary>
        /// 获取附件列表
        /// </summary>
        /// <param name="pidList"></param>
        /// <returns></returns>
        IDataReader GetAttachmentListByPid(string pidList);
        /// <summary>
        /// 获取附件列表
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        DataTable GetAttachmentListByPid(int pid);
        /// <summary>
        /// 获取附件列表
        /// </summary>
        /// <param name="tidlist"></param>
        /// <returns></returns>
        IDataReader GetAttachmentListByTid(string tidlist);
        /// <summary>
        /// 获取附件列表
        /// </summary>
        /// <param name="tid"></param>
        /// <returns></returns>
        IDataReader GetAttachmentListByTid(int tid);
        /// <summary>
        /// 获取附件列表
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        DataTable GetAttachmentsByPid(int pid);
        /// <summary>
        /// 获取附件类型
        /// </summary>
        /// <returns></returns>
        DataTable GetAttachmentType();
        /// <summary>
        /// 获取附件类型
        /// </summary>
        /// <returns></returns>
        DataTable GetAttachTypes();
        /// <summary>
        /// 获取附件类型
        /// </summary>
        /// <returns></returns>
        DataSet GetAttchType();
        /// <summary>
        /// 获取附件类型SQL语句
        /// </summary>
        /// <returns></returns>
        string GetAttchTypeSql();
        /// <summary>
        /// 获取待认证用户
        /// </summary>
        /// <returns></returns>
        string GetAudituserSql();
        /// <summary>
        /// 获取待认证用户Email
        /// </summary>
        /// <returns></returns>
        DataTable GetAuditUserEmail();
        /// <summary>
        /// 获取待认证用户Uid
        /// </summary>
        /// <returns></returns>
        DataSet GetAudituserUid();
        /// <summary>
        /// 获取可用勋章
        /// </summary>
        /// <returns></returns>
        DataTable GetAvailableMedal();
        /// <summary>
        /// 获取屏蔽词列表
        /// </summary>
        /// <returns></returns>
        DataTable GetBanWordList();
        /// <summary>
        /// 获取Discuz!NT代码
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        DataSet GetBBCCodeById(int id);
        /// <summary>
        /// 获取Discuz!NT代码
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        DataTable GetBBCode(int id);
        /// <summary>
        /// 获取Discuz!NT代码
        /// </summary>
        /// <returns></returns>
        DataTable GetBBCode();
        /// <summary>
        /// 获取积分日志
        /// </summary>
        /// <param name="pagesize"></param>
        /// <param name="currentpage"></param>
        /// <param name="uid"></param>
        /// <returns></returns>
        DataTable GetCreditsLogList(int pagesize, int currentpage, int uid);
        /// <summary>
        /// 获取积分日志数
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        int GetCreditsLogRecordCount(int uid);
        /// <summary>
        /// 获取当前帖子分表数
        /// </summary>
        /// <param name="currentPostTableId"></param>
        /// <returns></returns>
        int GetCurrentPostTableRecordCount(int currentPostTableId);
        /// <summary>
        /// 获取自定义按钮列表
        /// </summary>
        /// <returns></returns>
        IDataReader GetCustomEditButtonList();
        /// <summary>
        /// 获取自定义按钮列表
        /// </summary>
        /// <returns></returns>
        DataTable GetCustomEditButtonListWithTable();
        /// <summary>
        /// 获取帖子分表Id
        /// </summary>
        /// <returns></returns>
        DataRowCollection GetDatechTableIds();
        /// <summary>
        /// 获取数据库名称
        /// </summary>
        /// <returns></returns>
        string GetDbName();
        /// <summary>
        /// 获取帖子分表Id
        /// </summary>
        /// <returns></returns>
        DataTable GetDetachTableId();
        /// <summary>
        /// 获取错误登录记录
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        DataTable GetErrLoginRecordByIP(string ip);
        /// <summary>
        /// 获取收藏数
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        int GetFavoritesCount(int uid);
        /// <summary>
        /// 获取收藏数
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="typeid"></param>
        /// <returns></returns>
        int GetFavoritesCount(int uid, int typeid);
        /// <summary>
        /// 获取收藏列表
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="pagesize"></param>
        /// <param name="pageindex"></param>
        /// <returns></returns>
        DataTable GetFavoritesList(int uid, int pagesize, int pageindex);
        /// <summary>
        /// 获取收藏列表
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="pagesize"></param>
        /// <param name="pageindex"></param>
        /// <param name="typeid"></param>
        /// <returns></returns>
        DataTable GetFavoritesList(int uid, int pagesize, int pageindex, int typeid);
        /// <summary>
        /// 递归指定论坛版块下的所有子版块
        /// </summary>
        /// <param name="parentid"></param>
        /// <returns></returns>
        DataTable GetFidInForumsByParentid(int parentid);
        /// <summary>
        /// 获取主题第一个图片附件
        /// </summary>
        /// <param name="tid">主题id</param>
        /// <returns></returns>
        IDataReader GetFirstImageAttachByTid(int tid);
        /// <summary>
        /// 获取第一个帖子Id
        /// </summary>
        /// <param name="tid"></param>
        /// <param name="posttableid"></param>
        /// <returns></returns>
        int GetFirstPostId(int tid, string posttableid);
        /// <summary>
        /// 获取关注主题列表
        /// </summary>
        /// <param name="count">数量</param>
        /// <param name="views">最小浏览量</param>
        /// <param name="fid">板块ID</param>
        /// <param name="starttime">起始时间</param>
        /// <param name="orderfieldname">排序字段</param>
        /// <param name="visibleForum">板块范围(逗号分隔)</param>
        /// <param name="isdigest">是否精华</param>
        /// <param name="onlyimg">是否仅取带有图片附件的帖子</param>
        /// <returns></returns>
        DataTable GetFocusTopicList(int count, int views, int fid, string starttime, string orderfieldname,
                                    string visibleForum, bool isdigest, bool onlyimg);
        /// <summary>
        /// 获取板块信息
        /// </summary>
        /// <param name="fid"></param>
        /// <returns></returns>
        DataRow GetForum(int fid);
        /// <summary>
        /// 获取板块列表
        /// </summary>
        /// <param name="parentid"></param>
        /// <returns></returns>
        DataTable GetForumByParentid(int parentid);
        /// <summary>
        /// 获取板块指定字段内容
        /// </summary>
        /// <param name="fid"></param>
        /// <param name="fieldname"></param>
        /// <returns></returns>
        DataTable GetForumField(int fid, string fieldname);
        /// <summary>
        /// 获取板块Id列表
        /// </summary>
        /// <returns></returns>
        DataRowCollection GetForumIdList();
        /// <summary>
        /// 获取首页板块列表
        /// </summary>
        /// <returns></returns>
        IDataReader GetForumIndexList();
        /// <summary>
        /// 获取首页板块列表
        /// </summary>
        /// <returns></returns>
        DataTable GetForumIndexListTable();
        /// <summary>
        /// 获取板块信息
        /// </summary>
        /// <param name="fid"></param>
        /// <returns></returns>
        DataTable GetForumInformation(int fid);
        /// <summary>
        /// 获取板块最后一帖
        /// </summary>
        /// <param name="fid"></param>
        /// <param name="posttablename"></param>
        /// <param name="topiccount"></param>
        /// <param name="postcount"></param>
        /// <param name="lasttid"></param>
        /// <param name="lasttitle"></param>
        /// <param name="lastpost"></param>
        /// <param name="lastposterid"></param>
        /// <param name="lastposter"></param>
        /// <param name="todaypostcount"></param>
        /// <returns></returns>
        IDataReader GetForumLastPost(int fid, string posttablename, int topiccount, int postcount, int lasttid,
                                      string lasttitle, string lastpost, int lastposterid, string lastposter,
                                      int todaypostcount);
        /// <summary>
        /// 获取友情链接列表
        /// </summary>
        /// <returns></returns>
        DataTable GetForumLinkList();
        /// <summary>
        /// 获取友情链接
        /// </summary>
        /// <returns></returns>
        string GetForumLinks();
        /// <summary>
        /// 获取板块列表
        /// </summary>
        /// <returns></returns>
        DataTable GetForumList();
        /// <summary>
        /// 获取板块列表
        /// </summary>
        /// <returns></returns>
        DataTable GetForumListTable();
        /// <summary>
        /// 获取板块新主题
        /// </summary>
        /// <param name="fid"></param>
        /// <returns></returns>
        IDataReader GetForumNewTopics(int fid);
        /// <summary>
        /// 获取板块在线用户列表
        /// </summary>
        /// <param name="forumid"></param>
        /// <returns></returns>
        IDataReader GetForumOnlineUserList(int forumid);
        /// <summary>
        /// 获取板块在线用户列表
        /// </summary>
        /// <param name="forumid"></param>
        /// <returns></returns>
        DataTable GetForumOnlineUserListTable(int forumid);
        /// <summary>
        /// 获取板块列表
        /// </summary>
        /// <returns></returns>
        DataTable GetForums();
        /// <summary>
        /// 获取板块列表
        /// </summary>
        /// <param name="start_fid"></param>
        /// <param name="end_fid"></param>
        /// <returns></returns>
        IDataReader GetForums(int start_fid, int end_fid);
        /// <summary>
        /// 获取板块最大显示顺序
        /// </summary>
        /// <param name="parentid"></param>
        /// <returns></returns>
        DataTable GetForumsMaxDisplayOrder(int parentid);
        /// <summary>
        /// 获取板块最大显示顺序
        /// </summary>
        /// <returns></returns>
        int GetForumsMaxDisplayOrder();
        /// <summary>
        /// 获取板块父Id
        /// </summary>
        /// <param name="fid"></param>
        /// <returns></returns>
        int GetForumsParentidByFid(int fid);
        /// <summary>
        /// 获取板块列表
        /// </summary>
        /// <returns></returns>
        DataTable GetForumsTable();
        /// <summary>
        /// 获取板块统计信息
        /// </summary>
        /// <param name="fid"></param>
        /// <returns></returns>
        IDataReader GetForumStatistics(int fid);
        /// <summary>
        /// 获取板块树形
        /// </summary>
        /// <returns></returns>
        string GetForumsTree();
        /// <summary>
        /// 获取用户组数
        /// </summary>
        /// <param name="Creditshigher"></param>
        /// <returns></returns>
        int GetGroupCountByCreditsLower(int Creditshigher);
        /// <summary>
        /// 获取用户组信息
        /// </summary>
        /// <returns></returns>
        string GetGroupInfo();
        /// <summary>
        /// 获取表情分类字符串
        /// </summary>
        /// <returns></returns>
        string GetIcons();
        /// <summary>
        /// 获取最后帖子
        /// </summary>
        /// <param name="tid"></param>
        /// <param name="posttableid"></param>
        /// <returns></returns>
        IDataReader GetLastPost(int tid, int posttableid);
        /// <summary>
        /// 获取板块最后帖子
        /// </summary>
        /// <param name="fid"></param>
        /// <param name="posttablename"></param>
        /// <returns></returns>
        IDataReader GetLastPostByFid(int fid, string posttablename);
        /// <summary>
        /// 获取最后帖子
        /// </summary>
        /// <param name="tid"></param>
        /// <param name="posttablename"></param>
        /// <returns></returns>
        DataTable GetLastPostByTid(int tid, string posttablename);
        /// <summary>
        /// 获取最后帖子列表
        /// </summary>
        /// <param name="_postpramsinfo"></param>
        /// <param name="posttablename"></param>
        /// <returns></returns>
        DataTable GetLastPostList(PostpramsInfo _postpramsinfo, string posttablename);
        /// <summary>
        /// 获取最新注册用户信息
        /// </summary>
        /// <param name="lastuserid"></param>
        /// <param name="lastusername"></param>
        /// <returns></returns>
        bool GetLastUserInfo(out string lastuserid, out string lastusername);
        /// <summary>
        /// 获取魔法表情
        /// </summary>
        /// <returns></returns>
        DataTable GetMagicList();
        /// <summary>
        /// 获取顶级板块列表
        /// </summary>
        /// <returns></returns>
        DataTable GetMainForum();
        /// <summary>
        /// 获取主题帖
        /// </summary>
        /// <param name="posttablename"></param>
        /// <param name="tid"></param>
        /// <returns></returns>
        DataTable GetMainPostByTid(string posttablename, int tid);
        /// <summary>
        /// 获取板块最大和最小主题Id
        /// </summary>
        /// <param name="fid"></param>
        /// <returns></returns>
        IDataReader GetMaxAndMinTid(int fid);
        /// <summary>
        /// 获取用户最大和最小主题Id
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        IDataReader GetMaxAndMinTidByUid(int uid);
        /// <summary>
        /// 获取最大积分下限
        /// </summary>
        /// <returns></returns>
        DataTable GetMaxCreditLower();
        /// <summary>
        /// 获取最大板块Id
        /// </summary>
        /// <returns></returns>
        int GetMaxForumId();
        /// <summary>
        /// 获取最大勋章Id
        /// </summary>
        /// <returns></returns>
        int GetMaxMedalId();
        /// <summary>
        /// 获取最大帖子分表Id
        /// </summary>
        /// <param name="posttablename"></param>
        /// <returns></returns>
        int GetMaxPostTableTid(string posttablename);
        /// <summary>
        /// 获取最大表情Id
        /// </summary>
        /// <returns></returns>
        int GetMaxSmiliesId();
        /// <summary>
        /// 获取最大帖子分表Id
        /// </summary>
        /// <returns></returns>
        int GetMaxTableListId();
        /// <summary>
        /// 获取最大模板Id
        /// </summary>
        /// <returns></returns>
        int GetMaxTemplateId();
        /// <summary>
        /// 获取最大主题Id
        /// </summary>
        /// <returns></returns>
        DataTable GetMaxTid();
        /// <summary>
        /// 获取最大用户组Id
        /// </summary>
        /// <returns></returns>
        int GetMaxUserGroupId();
        /// <summary>
        /// 获取勋章
        /// </summary>
        /// <returns></returns>
        DataTable GetMedal();
        /// <summary>
        /// 获取勋章Sql语句
        /// </summary>
        /// <returns></returns>
        string GetMedalSql();
        /// <summary>
        /// 获取勋章日志列表
        /// </summary>
        /// <param name="pagesize"></param>
        /// <param name="currentpage"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        DataTable GetMedalLogList(int pagesize, int currentpage, string condition);
        /// <summary>
        /// 获取勋章日志列表
        /// </summary>
        /// <param name="pagesize"></param>
        /// <param name="currentpage"></param>
        /// <returns></returns>
        DataTable GetMedalLogList(int pagesize, int currentpage);
        /// <summary>
        /// 获取勋章日志数
        /// </summary>
        /// <returns></returns>
        int GetMedalLogListCount();
        /// <summary>
        /// 获取勋章日志数
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        int GetMedalLogListCount(string condition);
        /// <summary>
        /// 获取勋章列表
        /// </summary>
        /// <returns></returns>
        DataTable GetMedalsList();
        /// <summary>
        /// 获取最小的积分上限
        /// </summary>
        /// <returns></returns>
        DataTable GetMinCreditHigher();
        /// <summary>
        /// 获取最小的帖子分表Id
        /// </summary>
        /// <param name="posttablename"></param>
        /// <returns></returns>
        int GetMinPostTableTid(string posttablename);
        /// <summary>
        /// 获取版主信息
        /// </summary>
        /// <param name="moderator"></param>
        /// <returns></returns>
        DataTable GetModeratorInfo(string moderator);
        /// <summary>
        /// 获取版主列表
        /// </summary>
        /// <returns></returns>
        DataTable GetModeratorList();
        /// <summary>
        /// 获取管理日志
        /// </summary>
        /// <param name="pagesize"></param>
        /// <param name="currentpage"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        DataTable GetModeratorLogList(int pagesize, int currentpage, string condition);
        /// <summary>
        /// 获取管理日志数
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        int GetModeratorLogListCount(string condition);
        /// <summary>
        /// 获取管理日志数
        /// </summary>
        /// <returns></returns>
        int GetModeratorLogListCount();
        /// <summary>
        /// 获取版主
        /// </summary>
        /// <param name="oldusername"></param>
        /// <returns></returns>
        DataRowCollection GetModerators(string oldusername);
        /// <summary>
        /// 获取版主
        /// </summary>
        /// <param name="fid"></param>
        /// <returns></returns>
        DataRowCollection GetModerators(int fid);
        /// <summary>
        /// 获取版主
        /// </summary>
        /// <param name="oldusername"></param>
        /// <returns></returns>
        DataTable GetModeratorsTable(string oldusername);
        /// <summary>
        /// 获取新短消息数
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        int GetNewPMCount(int userid);
        /// <summary>
        /// 获取新主题
        /// </summary>
        /// <param name="forumidlist"></param>
        /// <returns></returns>
        IDataReader GetNewTopics(string forumidlist);
        /// <summary>
        /// 获取在线用户数
        /// </summary>
        /// <returns></returns>
        int GetOnlineAllUserCount();
        /// <summary>
        /// 获取在线图例
        /// </summary>
        /// <returns></returns>
        IDataReader GetOnlineGroupIconList();
        /// <summary>
        /// 获取在线图例
        /// </summary>
        /// <returns></returns>
        DataTable GetOnlineGroupIconTable();
        /// <summary>
        /// 获取在线列表
        /// </summary>
        /// <returns></returns>
        DataSet GetOnlineList();
        /// <summary>
        /// 获取在线用户信息
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        DataTable GetOnlineUser(int userid, string password);
        /// <summary>
        /// 获取在线用户信息
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="ip"></param>
        /// <returns></returns>
        DataTable GetOnlineUserByIP(int userid, string ip);
        /// <summary>
        /// 获取在线用户数
        /// </summary>
        /// <returns></returns>
        int GetOnlineUserCount();
        /// <summary>
        /// 获取在线用户列表
        /// </summary>
        /// <returns></returns>
        IDataReader GetOnlineUserList();
        /// <summary>
        /// 获取在线用户列表
        /// </summary>
        /// <returns></returns>
        DataTable GetOnlineUserListTable();
        /// <summary>
        /// 获取其它普通用户组
        /// </summary>
        /// <param name="exceptgroupid"></param>
        /// <returns></returns>
        DataTable GetOthersCommonUserGroup(int exceptgroupid);
        /// <summary>
        /// 获取板块父Id
        /// </summary>
        /// <param name="fid"></param>
        /// <returns></returns>
        DataTable GetParentIdByFid(int fid);
        /// <summary>
        /// 获取收入记录
        /// </summary>
        /// <param name="pagesize"></param>
        /// <param name="currentpage"></param>
        /// <param name="uid"></param>
        /// <returns></returns>
        DataTable GetPayLogInList(int pagesize, int currentpage, int uid);
        /// <summary>
        /// 获取支出记录
        /// </summary>
        /// <param name="pagesize"></param>
        /// <param name="currentpage"></param>
        /// <param name="uid"></param>
        /// <returns></returns>
        DataTable GetPayLogOutList(int pagesize, int currentpage, int uid);
        /// <summary>
        /// 获取主题购买记录
        /// </summary>
        /// <param name="pagesize"></param>
        /// <param name="currentpage"></param>
        /// <param name="tid"></param>
        /// <returns></returns>
        DataTable GetPaymentLogByTid(int pagesize, int currentpage, int tid);
        /// <summary>
        /// 获取主题购买记录数
        /// </summary>
        /// <param name="tid"></param>
        /// <returns></returns>
        int GetPaymentLogByTidCount(int tid);
        /// <summary>
        /// 获取收入记录数
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        int GetPaymentLogInRecordCount(int uid);
        /// <summary>
        /// 获取交易记录列表
        /// </summary>
        /// <param name="pagesize"></param>
        /// <param name="currentpage"></param>
        /// <returns></returns>
        DataTable GetPaymentLogList(int pagesize, int currentpage);
        /// <summary>
        /// 获取交易记录列表
        /// </summary>
        /// <param name="pagesize"></param>
        /// <param name="currentpage"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        DataTable GetPaymentLogList(int pagesize, int currentpage, string condition);
        /// <summary>
        /// 获取交易记录数
        /// </summary>
        /// <returns></returns>
        int GetPaymentLogListCount();
        /// <summary>
        /// 获取交易记录数
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        int GetPaymentLogListCount(string condition);
        /// <summary>
        /// 获取支出记录数
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        int GetPaymentLogOutRecordCount(int uid);
        /// <summary>
        /// 获取投票
        /// </summary>
        /// <param name="tid"></param>
        /// <returns></returns>
        IDataReader GetPollAndOptions(int tid);
        /// <summary>
        /// 获取投票截止时间
        /// </summary>
        /// <param name="tid"></param>
        /// <returns></returns>
        string GetPollEnddatetime(int tid);
        /// <summary>
        /// 获取投票列表
        /// </summary>
        /// <param name="tid"></param>
        /// <returns></returns>
        IDataReader GetPollList(int tid);
        /// <summary>
        /// 获取投票选项列表
        /// </summary>
        /// <param name="tid"></param>
        /// <returns></returns>
        IDataReader GetPollOptionList(int tid);
        /// <summary>
        /// 获取投票类型
        /// </summary>
        /// <param name="tid"></param>
        /// <returns></returns>
        int GetPollType(int tid);
        /// <summary>
        /// 获取投过票的用户列表
        /// </summary>
        /// <param name="tid"></param>
        /// <returns></returns>
        string GetPollUserNameList(int tid);
        /// <summary>
        /// 获取帖子
        /// </summary>
        /// <param name="posttablename"></param>
        /// <param name="pid"></param>
        /// <returns></returns>
        DataTable GetPost(string posttablename, int pid);
        /// <summary>
        /// 获取板块帖子数
        /// </summary>
        /// <param name="fid"></param>
        /// <param name="posttablename"></param>
        /// <returns></returns>
        int GetPostCount(int fid, string posttablename);
        /// <summary>
        /// 获取主题帖子数
        /// </summary>
        /// <param name="tid"></param>
        /// <param name="posttablename"></param>
        /// <returns></returns>
        int GetPostCountByTid(int tid, string posttablename);
        /// <summary>
        /// 获取帖子数
        /// </summary>
        /// <param name="posttableid"></param>
        /// <param name="tid"></param>
        /// <param name="posterid"></param>
        /// <returns></returns>
        int GetPostCount(string posttableid, int tid, int posterid);
        /// <summary>
        /// 获取帖子数
        /// </summary>
        /// <param name="tid"></param>
        /// <param name="condition"></param>
        /// <param name="posttableid"></param>
        /// <returns></returns>
        int GetPostCount(int tid, string condition, string posttableid);
        /// <summary>
        /// 获取帖子数
        /// </summary>
        /// <param name="posttablename"></param>
        /// <returns></returns>
        int GetPostCount(string posttablename);
        /// <summary>
        /// 获取帖子数
        /// </summary>
        /// <param name="fid"></param>
        /// <param name="posttableid"></param>
        /// <returns></returns>
        int GetPostCount(int fid, int posttableid);
        /// <summary>
        /// 获取帖子数
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="posttablename"></param>
        /// <returns></returns>
        int GetPostCountByUid(int uid, string posttablename);
        /// <summary>
        /// 获取帖子数
        /// </summary>
        /// <param name="postsid"></param>
        /// <returns></returns>
        DataTable GetPostCountFromIndex(string postsid);
        /// <summary>
        /// 获取帖子数
        /// </summary>
        /// <param name="postsid"></param>
        /// <returns></returns>
        DataTable GetPostCountTable(string postsid);
        /// <summary>
        /// 获取帖子信息
        /// </summary>
        /// <param name="posttableid"></param>
        /// <param name="pid"></param>
        /// <returns></returns>
        IDataReader GetPostInfo(string posttableid, int pid);
        /// <summary>
        /// 获取帖子所处的Layer
        /// </summary>
        /// <param name="currentPostTableId"></param>
        /// <param name="postid"></param>
        /// <returns></returns>
        DataTable GetPostLayer(int currentPostTableId, int postid);
        /// <summary>
        /// 获取帖子列表
        /// </summary>
        /// <param name="_postpramsinfo"></param>
        /// <param name="posttablename"></param>
        /// <returns></returns>
        IDataReader GetPostList(PostpramsInfo _postpramsinfo, string posttablename);
        /// <summary>
        /// 获取帖子列表
        /// </summary>
        /// <param name="topiclist"></param>
        /// <param name="posttableid"></param>
        /// <returns></returns>
        DataSet GetPostList(string topiclist, string[] posttableid);
        /// <summary>
        /// 获取帖子列表
        /// </summary>
        /// <param name="_postpramsinfo"></param>
        /// <param name="posttablename"></param>
        /// <returns></returns>
        IDataReader GetPostListByCondition(PostpramsInfo _postpramsinfo, string posttablename);
        /// <summary>
        /// 获取帖子标题列表
        /// </summary>
        /// <param name="Tid"></param>
        /// <param name="posttablename"></param>
        /// <returns></returns>
        DataTable GetPostListTitle(int Tid, string posttablename);
        /// <summary>
        /// 获取帖子评分记录
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="displayRateCount"></param>
        /// <returns></returns>
        DataTable GetPostRateList(int pid, int displayRateCount);
        /// <summary>
        /// 获取帖子列表
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="pagesize"></param>
        /// <param name="pageindex"></param>
        /// <param name="posttablename"></param>
        /// <returns></returns>
        DataSet GetPosts(int pid, int pagesize, int pageindex, string posttablename);
        /// <summary>
        /// 获取帖子数
        /// </summary>
        /// <param name="posttableid"></param>
        /// <returns></returns>
        int GetPostsCount(string posttableid);
        /// <summary>
        /// 获取帖子分表列表
        /// </summary>
        /// <returns></returns>
        DataTable GetPostTableList();
        /// <summary>
        /// 获取帖子树型
        /// </summary>
        /// <param name="tid"></param>
        /// <param name="posttableid"></param>
        /// <returns></returns>
        DataTable GetPostTree(int tid, string posttableid);
        /// <summary>
        /// 获取短消息数量
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="folder"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        int GetPrivateMessageCount(int userid, int folder, int state);
        /// <summary>
        /// 获取短消息信息
        /// </summary>
        /// <param name="pmid"></param>
        /// <returns></returns>
        IDataReader GetPrivateMessageInfo(int pmid);
        /// <summary>
        /// 获取短消息列表
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="folder"></param>
        /// <param name="pagesize"></param>
        /// <param name="pageindex"></param>
        /// <param name="inttype"></param>
        /// <returns></returns>
        IDataReader GetPrivateMessageList(int userid, int folder, int pagesize, int pageindex, int inttype);
        /// <summary>
        /// 获取组RadminId
        /// </summary>
        /// <param name="groupid"></param>
        /// <returns></returns>
        int GetRAdminIdByGroup(int groupid);
        /// <summary>
        /// 获取组RadminId
        /// </summary>
        /// <param name="groupid"></param>
        /// <returns></returns>
        int GetRadminidByGroupid(int groupid);
        /// <summary>
        /// 获取评分日志数
        /// </summary>
        /// <returns></returns>
        int GetRateLogCount();
        /// <summary>
        /// 获取评分日志数
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        int GetRateLogCount(string condition);
        /// <summary>
        /// 获取评分范围
        /// </summary>
        /// <param name="scoreid"></param>
        /// <returns></returns>
        DataRowCollection GetRateRange(int scoreid);
        /// <summary>
        /// 获取评分范围
        /// </summary>
        /// <param name="groupid"></param>
        /// <returns></returns>
        DataTable GetRaterangeByGroupid(int groupid);
        /// <summary>
        /// 获取搜索缓存
        /// </summary>
        /// <param name="searchid"></param>
        /// <returns></returns>
        DataTable GetSearchCache(int searchid);
        /// <summary>
        /// 获取精华主题列表
        /// </summary>
        /// <param name="pagesize"></param>
        /// <param name="strTids"></param>
        /// <returns></returns>
        DataTable GetSearchDigestTopicsList(int pagesize, string strTids);
        /// <summary>
        /// 获取按帖子搜索的主题列表
        /// </summary>
        /// <param name="pagesize"></param>
        /// <param name="strTids"></param>
        /// <param name="postablename"></param>
        /// <returns></returns>
        DataTable GetSearchPostsTopicsList(int pagesize, string strTids, string postablename);
        /// <summary>
        /// 获取按主题搜索的主题列表
        /// </summary>
        /// <param name="pagesize"></param>
        /// <param name="strTids"></param>
        /// <returns></returns>
        DataTable GetSearchTopicsList(int pagesize, string strTids);
        /// <summary>
        /// 获取简要板块信息
        /// </summary>
        /// <returns></returns>
        DataTable GetShortForums();
        /// <summary>
        /// 获取简要用户信息
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        IDataReader GetShortUserInfoToReader(int uid);
        /// <summary>
        /// 获取简要通告列表
        /// </summary>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <param name="maxcount"></param>
        /// <returns></returns>
        DataTable GetSimplifiedAnnouncementList(string starttime, string endtime, int maxcount);
        /// <summary>
        /// 获取单个帖子
        /// </summary>
        /// <param name="_Attachments"></param>
        /// <param name="_postpramsinfo"></param>
        /// <param name="posttableid"></param>
        /// <returns></returns>
        IDataReader GetSinglePost(out IDataReader _Attachments, PostpramsInfo _postpramsinfo, string posttableid);
        /// <summary>
        /// 获取表情
        /// </summary>
        /// <returns></returns>
        string GetSmilies();
        /// <summary>
        /// 获取表情
        /// </summary>
        /// <returns></returns>
        IDataReader GetSmiliesList();
        /// <summary>
        /// 获取表情
        /// </summary>
        /// <returns></returns>
        DataTable GetSmiliesListDataTable();
        /// <summary>
        /// 获取表情
        /// </summary>
        /// <returns></returns>
        DataTable GetSmiliesListWithoutType();
        /// <summary>
        /// 获取表情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        DataRow GetSmilieTypeById(string id);
        /// <summary>
        /// 获取表情
        /// </summary>
        /// <returns></returns>
        DataTable GetSmilieTypes();
        /// <summary>
        /// 获取特殊组信息SQL语句
        /// </summary>
        /// <returns></returns>
        string GetSpecialGroupInfoSql();
        /// <summary>
        /// 得到指定帖子分表的全文索引建立(填充)语句
        /// </summary>
        /// <param name="tablename"></param>
        /// <param name="dbname"></param>
        /// <returns></returns>
        string GetSpecialTableFullIndexSQL(string tablename, string dbname);
        /// <summary>
        /// 得到指定帖子分表的全文索引建立(填充)语句
        /// </summary>
        /// <param name="tablename"></param>
        /// <returns></returns>
        string GetSpecialTableFullIndexSQL(string tablename);
        /// <summary>
        /// 获取统计信息
        /// </summary>
        /// <returns></returns>
        DataRow GetStatisticsRow();
        /// <summary>
        /// 获取子版块列表
        /// </summary>
        /// <param name="fid"></param>
        /// <returns></returns>
        IDataReader GetSubForumReader(int fid);
        /// <summary>
        /// 获取子版块列表
        /// </summary>
        /// <param name="fid"></param>
        /// <returns></returns>
        DataTable GetSubForumTable(int fid);
        /// <summary>
        /// 获取系统组信息SQL语句
        /// </summary>
        /// <returns></returns>
        string GetSystemGroupInfoSql();
        /// <summary>
        /// 获取帖子分表Id列表
        /// </summary>
        /// <returns></returns>
        DataRowCollection GetTableListIds();
        /// <summary>
        /// 获取帖子分表信息
        /// </summary>
        /// <returns></returns>
        DataTable GetTableListInfo();
        /// <summary>
        /// 获取目标板块名
        /// </summary>
        /// <param name="targets"></param>
        /// <returns></returns>
        DataRowCollection GetTargetsForumName(string targets);
        /// <summary>
        /// 获取模板信息
        /// </summary>
        /// <returns></returns>
        string GetTemplates();
        /// <summary>
        /// 获取管理日志被操作的Tid字符串
        /// </summary>
        /// <param name="postDateTime"></param>
        /// <returns></returns>
        DataTable GetTidForModeratormanagelogByPostdatetime(DateTime postDateTime);
        /// <summary>
        /// 根据时间获取管理日志标题
        /// </summary>
        /// <param name="moderatorname"></param>
        /// <returns></returns>
        DataTable GetTitleForModeratormanagelogByModeratorname(string moderatorname);
        /// <summary>
        /// 根据时间获取管理日志标题
        /// </summary>
        /// <param name="startDateTime"></param>
        /// <param name="endDateTime"></param>
        /// <returns></returns>
        DataTable GetTitleForModeratormanagelogByPostdatetime(DateTime startDateTime, DateTime endDateTime);
        /// <summary>
        /// 获取今日帖数
        /// </summary>
        /// <param name="fid"></param>
        /// <param name="posttableid"></param>
        /// <returns></returns>
        int GetTodayPostCount(int fid, int posttableid);
        /// <summary>
        /// 获取今日帖数
        /// </summary>
        /// <param name="fid"></param>
        /// <param name="posttablename"></param>
        /// <returns></returns>
        int GetTodayPostCount(int fid, string posttablename);
        /// <summary>
        /// 获取今日帖数
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="posttablename"></param>
        /// <returns></returns>
        int GetTodayPostCountByUid(int uid, string posttablename);
        /// <summary>
        /// 获取顶级板块信息
        /// </summary>
        /// <param name="fid"></param>
        /// <returns></returns>
        DataTable GetTopForum(int fid);
        /// <summary>
        /// 获取顶级板块Id列表
        /// </summary>
        /// <param name="lastfid"></param>
        /// <param name="statcount"></param>
        /// <returns></returns>
        IDataReader GetTopForumFids(int lastfid, int statcount);
        /// <summary>
        /// 获取主题数
        /// </summary>
        /// <param name="fid"></param>
        /// <returns></returns>
        int GetTopicCount(int fid);
        /// <summary>
        /// 获取主题数
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        int GetTopicCount(string condition);
        /// <summary>
        /// 获取主题所属板块Id
        /// </summary>
        /// <param name="tidlist"></param>
        /// <returns></returns>
        DataTable GetTopicFidByTid(string tidlist);
        /// <summary>
        /// 获取主题信息
        /// </summary>
        /// <param name="tid"></param>
        /// <param name="fid"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        IDataReader GetTopicInfo(int tid, int fid, byte mode);
        /// <summary>
        /// 获取主题列表
        /// </summary>
        /// <param name="topiclist"></param>
        /// <param name="displayorder"></param>
        /// <returns></returns>
        DataTable GetTopicList(string topiclist, int displayorder);
        /// <summary>
        /// 获取主题列表
        /// </summary>
        /// <param name="forumid"></param>
        /// <param name="pageid"></param>
        /// <param name="tpp"></param>
        /// <returns></returns>
        DataTable GetTopicList(int forumid, int pageid, int tpp);
        /// <summary>
        /// 获取主题管理日志列表
        /// </summary>
        /// <param name="tid"></param>
        /// <returns></returns>
        IDataReader GetTopicListModeratorLog(int tid);
        /// <summary>
        /// 获取主题列表
        /// </summary>
        /// <param name="start_tid"></param>
        /// <param name="end_tid"></param>
        /// <returns></returns>
        IDataReader GetTopics(int start_tid, int end_tid);
        /// <summary>
        /// 获取主题列表
        /// </summary>
        /// <param name="fid"></param>
        /// <param name="pagesize"></param>
        /// <param name="pageindex"></param>
        /// <param name="startnum"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        IDataReader GetTopics(int fid, int pagesize, int pageindex, int startnum, string condition);
        /// <summary>
        /// 获取主题列表
        /// </summary>
        /// <param name="fid"></param>
        /// <param name="pagesize"></param>
        /// <param name="pageindex"></param>
        /// <param name="startnum"></param>
        /// <param name="condition"></param>
        /// <param name="orderby"></param>
        /// <param name="ascdesc"></param>
        /// <returns></returns>
        IDataReader GetTopicsByDate(int fid, int pagesize, int pageindex, int startnum, string condition,
                                     string orderby, int ascdesc);
        /// <summary>
        /// 获取主题列表
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        IDataReader GetTopicsByReplyUserId(int userId, int pageIndex, int pageSize);
        /// <summary>
        /// 获取主题列表
        /// </summary>
        /// <param name="pagesize"></param>
        /// <param name="pageindex"></param>
        /// <param name="startnum"></param>
        /// <param name="condition"></param>
        /// <param name="ascdesc"></param>
        /// <returns></returns>
        IDataReader GetTopicsByType(int pagesize, int pageindex, int startnum, string condition, int ascdesc);
        /// <summary>
        /// 获取主题列表
        /// </summary>
        /// <param name="pagesize"></param>
        /// <param name="pageindex"></param>
        /// <param name="startnum"></param>
        /// <param name="condition"></param>
        /// <param name="orderby"></param>
        /// <param name="ascdesc"></param>
        /// <returns></returns>
        IDataReader GetTopicsByTypeDate(int pagesize, int pageindex, int startnum, string condition, string orderby,
                                     int ascdesc);
        /// <summary>
        /// 获取主题列表
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        IDataReader GetTopicsByUserId(int userId, int pageIndex, int pageSize);
        /// <summary>
        /// 获取主题数
        /// </summary>
        /// <returns></returns>
        int GetTopicsCount();
        /// <summary>
        /// 获取主题数
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        int GetTopicsCountbyReplyUserId(int userId);
        /// <summary>
        /// 获取主题数
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        int GetTopicsCountbyUserId(int userId);
        /// <summary>
        /// 获取主题状态
        /// </summary>
        /// <param name="topiclist"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        int GetTopicStatus(string topiclist, string field);
        /// <summary>
        /// 获取主题Id列表
        /// </summary>
        /// <param name="tidlist"></param>
        /// <param name="fid"></param>
        /// <returns></returns>
        DataTable GetTopicTidByFid(string tidlist, int fid);
        /// <summary>
        /// 获取主题Tid列表
        /// </summary>
        /// <param name="statcount"></param>
        /// <param name="lasttid"></param>
        /// <returns></returns>
        IDataReader GetTopicTids(int statcount, int lasttid);
        /// <summary>
        /// 获取主题分类列表
        /// </summary>
        /// <returns></returns>
        DataTable GetTopicTypeList();
        /// <summary>
        /// 获取置顶主题列表
        /// </summary>
        /// <param name="fid"></param>
        /// <returns></returns>
        DataSet GetTopTopicList(int fid);
        /// <summary>
        /// 获取置顶主题列表
        /// </summary>
        /// <param name="fid"></param>
        /// <param name="pagesize"></param>
        /// <param name="pageindex"></param>
        /// <param name="tids"></param>
        /// <returns></returns>
        IDataReader GetTopTopics(int fid, int pagesize, int pageindex, string tids);
        /// <summary>
        /// 获取指定数量的用户
        /// </summary>
        /// <param name="statcount"></param>
        /// <param name="lastuid"></param>
        /// <returns></returns>
        IDataReader GetTopUsers(int statcount, int lastuid);
        /// <summary>
        /// 获取Uid和AdminId
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        DataTable GetUidAdminIdByUsername(string username);
        /// <summary>
        /// 获取Uid
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        int GetuidByusername(string username);
        /// <summary>
        /// 获取Uid
        /// </summary>
        /// <param name="currentfid"></param>
        /// <param name="uid"></param>
        /// <returns></returns>
        DataTable GetUidInModeratorsByUid(int currentfid, int uid);
        /// <summary>
        /// 获取Uid
        /// </summary>
        /// <param name="fidlist"></param>
        /// <returns></returns>
        DataTable GetUidModeratorByFid(string fidlist);
        /// <summary>
        /// 获取未审核的主题SQL语句
        /// </summary>
        /// <returns></returns>
        string GetUnauditNewTopicSQL();
        /// <summary>
        /// 获取未审核的帖子SQL语句
        /// </summary>
        /// <param name="currentPostTableId"></param>
        /// <returns></returns>
        string GetUnauditPostSQL(int currentPostTableId);
        /// <summary>
        /// 获取用户上传的文件大小
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        int GetUploadFileSizeByUserId(int uid);
        /// <summary>
        /// 获取用户数
        /// </summary>
        /// <returns></returns>
        int GetUserCount();
        /// <summary>
        /// 获取有管理权限的用户数
        /// </summary>
        /// <returns></returns>
        int GetUserCountByAdmin();
        /// <summary>
        /// 获取指定组的用户Email地址
        /// </summary>
        /// <param name="groupidlist"></param>
        /// <returns></returns>
        DataTable GetUserEmailByGroupid(string groupidlist);
        /// <summary>
        /// 获取用户Email地址
        /// </summary>
        /// <param name="uidlist"></param>
        /// <returns></returns>
        DataTable GetUserEmailByUidlist(string uidlist);
        /// <summary>
        /// 获取用户扩展积分
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="extid"></param>
        /// <returns></returns>
        float GetUserExtCredits(int uid, int extid);
        /// <summary>
        /// 获取用户组
        /// </summary>
        /// <param name="groupid"></param>
        /// <returns></returns>
        DataTable GetUserGroup(int groupid);
        /// <summary>
        /// 获取用户组
        /// </summary>
        /// <returns></returns>
        string GetUserGroup();
        /// <summary>
        /// 获取用户组
        /// </summary>
        /// <param name="Creditshigher"></param>
        /// <returns></returns>
        DataTable GetUserGroupByCreditshigher(int Creditshigher);
        /// <summary>
        /// 获取用户组
        /// </summary>
        /// <param name="Creditshigher"></param>
        /// <param name="Creditslower"></param>
        /// <returns></returns>
        DataTable GetUserGroupByCreditsHigherAndLower(int Creditshigher, int Creditslower);
        /// <summary>
        /// 获取用户组
        /// </summary>
        /// <param name="groupid"></param>
        /// <returns></returns>
        DataTable GetUserGroupCreditsLowerAndHigher(int groupid);
        /// <summary>
        /// 获取用户组
        /// </summary>
        /// <param name="groupid"></param>
        /// <returns></returns>
        DataTable GetUserGroupExceptGroupid(int groupid);
        /// <summary>
        /// 获取用户组
        /// </summary>
        /// <param name="groupid"></param>
        /// <returns></returns>
        DataTable GetUserGroupInfoByGroupid(int groupid);
        /// <summary>
        /// 获取用户组的RadminId
        /// </summary>
        /// <param name="groupid"></param>
        /// <returns></returns>
        string GetUserGroupRAdminId(int groupid);
        /// <summary>
        /// 获取用户组评分权限
        /// </summary>
        /// <param name="groupid"></param>
        /// <returns></returns>
        DataTable GetUserGroupRateRange(int groupid);
        /// <summary>
        /// 获取用户组列表
        /// </summary>
        /// <returns></returns>
        DataTable GetUserGroups();
        /// <summary>
        /// 获取用户组列表字符串
        /// </summary>
        /// <returns></returns>
        string GetUserGroupsStr();
        /// <summary>
        /// 获取用户组名列表
        /// </summary>
        /// <returns></returns>
        DataTable GetUserGroupsTitle();
        /// <summary>
        /// 获取用户组名称
        /// </summary>
        /// <returns></returns>
        string GetUserGroupTitle();
        /// <summary>
        /// 获取除游客组之外的用户组名称
        /// </summary>
        /// <returns></returns>
        DataTable GetUserGroupWithOutGuestTitle();
        /// <summary>
        /// 获取用户Id
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        IDataReader GetUserID(string username);
        /// <summary>
        /// 根据验证字串获取用户Id
        /// </summary>
        /// <param name="authstr"></param>
        /// <returns></returns>
        DataTable GetUserIdByAuthStr(string authstr);
        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        DataTable GetUserInfo(string username, string password);
        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        DataTable GetUserInfo(int userid);
        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        IDataReader GetUserInfoByIP(string ip);
        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        IDataReader GetUserInfoToReader(int uid);
        /// <summary>
        /// 获取用户注册日期
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        IDataReader GetUserJoinDate(int uid);
        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <param name="pagesize"></param>
        /// <param name="pageindex"></param>
        /// <param name="orderby"></param>
        /// <param name="ordertype"></param>
        /// <returns></returns>
        DataTable GetUserList(int pagesize, int pageindex, string orderby, string ordertype);
        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <param name="pagesize"></param>
        /// <param name="currentpage"></param>
        /// <returns></returns>
        DataTable GetUserList(int pagesize, int currentpage);
        /// <summary>
        /// 获取用户列表和帖子列表
        /// </summary>
        /// <param name="postlist"></param>
        /// <param name="posttableid"></param>
        /// <returns></returns>
        IDataReader GetUserListWithPostList(string postlist, string posttableid);
        /// <summary>
        /// 获取用户列表和主题列表
        /// </summary>
        /// <param name="topiclist"></param>
        /// <param name="losslessdel"></param>
        /// <returns></returns>
        IDataReader GetUserListWithTopicList(string topiclist, int losslessdel);
        /// <summary>
        /// 获取用户列表和主题列表
        /// </summary>
        /// <param name="topiclist"></param>
        /// <returns></returns>
        IDataReader GetUserListWithTopicList(string topiclist);
        /// <summary>
        /// 获取用户名
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        IDataReader GetUserName(int uid);
        /// <summary>
        /// 获取用户名
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        DataTable GetUserNameByUid(int uid);
        /// <summary>
        /// 获取用户名
        /// </summary>
        /// <param name="groupidlist"></param>
        /// <returns></returns>
        DataTable GetUserNameListByGroupid(string groupidlist);
        /// <summary>
        /// 获取新的短消息数
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        int GetUserNewPMCount(int uid);
        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <param name="start_uid"></param>
        /// <param name="end_uid"></param>
        /// <returns></returns>
        IDataReader GetUsers(int start_uid, int end_uid);
        /// <summary>
        /// 获取今日用户评分
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        IDataReader GetUserTodayRate(int uid);
        /// <summary>
        /// 获取可用模板Id列表
        /// </summary>
        /// <returns></returns>
        DataTable GetValidTemplateIDList();
        /// <summary>
        /// 获取可用模板列表
        /// </summary>
        /// <returns></returns>
        DataTable GetValidTemplateList();
        /// <summary>
        /// 获取可见板块列表
        /// </summary>
        /// <returns></returns>
        IDataReader GetVisibleForumList();
        /// <summary>
        /// 获取访问日志数
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        int GetVisitLogCount(string condition);
        /// <summary>
        /// 获取访问日志数
        /// </summary>
        /// <returns></returns>
        int GetVisitLogCount();
        /// <summary>
        /// 获取访问日志列表
        /// </summary>
        /// <param name="pagesize"></param>
        /// <param name="currentpage"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        DataTable GetVisitLogList(int pagesize, int currentpage, string condition);
        /// <summary>
        /// 获取访问日志列表
        /// </summary>
        /// <param name="pagesize"></param>
        /// <param name="currentpage"></param>
        /// <returns></returns>
        DataTable GetVisitLogList(int pagesize, int currentpage);
        /// <summary>
        /// 获取屏蔽词语
        /// </summary>
        /// <returns></returns>
        string GetWords();
        /// <summary>
        /// 返回是否存在最后注册的用户
        /// </summary>
        /// <param name="lastuserid"></param>
        /// <returns></returns>
        bool HasStatisticsByLastUserId(int lastuserid);
        /// <summary>
        /// 是否在指定板块内的主题
        /// </summary>
        /// <param name="topicidlist"></param>
        /// <param name="fid"></param>
        /// <returns></returns>
        bool InSameForum(string topicidlist, int fid);
        /// <summary>
        /// 插入板块信息
        /// </summary>
        /// <param name="foruminfo"></param>
        /// <returns></returns>
        int InsertForumsInf(ForumInfo __foruminfo);
        /// <summary>
        /// 插入管理信息
        /// </summary>
        /// <param name="fid"></param>
        /// <param name="moderators"></param>
        /// <param name="displayorder"></param>
        /// <param name="inherited"></param>
        void InsertForumsModerators(string fid, string moderators, int displayorder, int inherited);
        /// <summary>
        /// 插入管理日志
        /// </summary>
        /// <param name="moderatoruid"></param>
        /// <param name="moderatorname"></param>
        /// <param name="groupid"></param>
        /// <param name="grouptitle"></param>
        /// <param name="ip"></param>
        /// <param name="postdatetime"></param>
        /// <param name="fid"></param>
        /// <param name="fname"></param>
        /// <param name="tid"></param>
        /// <param name="title"></param>
        /// <param name="actions"></param>
        /// <param name="reason"></param>
        /// <returns></returns>
        bool InsertModeratorLog(string moderatoruid, string moderatorname, string groupid, string grouptitle, string ip,
                                string postdatetime, string fid, string fname, string tid, string title, string actions,
                                string reason);
        /// <summary>
        /// 插入评分日志
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="userid"></param>
        /// <param name="username"></param>
        /// <param name="extid"></param>
        /// <param name="score"></param>
        /// <param name="reason"></param>
        /// <returns></returns>
        int InsertRateLog(int pid, int userid, string username, int extid, int score, string reason);
        /// <summary>
        /// 是否是主题购买者
        /// </summary>
        /// <param name="tid"></param>
        /// <param name="uid"></param>
        /// <returns></returns>
        bool IsBuyer(int tid, int uid);
        /// <summary>
        /// 是否是允许的扩展名
        /// </summary>
        /// <param name="extensionname"></param>
        /// <returns></returns>
        bool IsExistExtensionInAttachtypes(string extensionname);
        /// <summary>
        /// 是否已存在的勋章
        /// </summary>
        /// <param name="medalid"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        bool IsExistMedalAwardRecord(int medalid, int userid);
        /// <summary>
        /// 是否存在子版块
        /// </summary>
        /// <param name="fid"></param>
        /// <returns></returns>
        bool IsExistSubForum(int fid);
        /// <summary>
        /// 是否是回复者
        /// </summary>
        /// <param name="tid"></param>
        /// <param name="uid"></param>
        /// <param name="posttableid"></param>
        /// <returns></returns>
        bool IsReplier(int tid, int uid, string posttableid);
        /// <summary>
        /// 是否是系统组
        /// </summary>
        /// <param name="groupid"></param>
        /// <returns></returns>
        bool IsSystemOrTemplateUserGroup(int groupid);
        /// <summary>
        /// 移动版块位置
        /// </summary>
        /// <param name="currentfid"></param>
        /// <param name="targetfid"></param>
        /// <param name="isaschildnode"></param>
        /// <param name="extname"></param>
        void MovingForumsPos(string currentfid, string targetfid, bool isaschildnode, string extname);
        /// <summary>
        /// 通过待验证的主题
        /// </summary>
        /// <param name="postTableName"></param>
        /// <param name="tidlist"></param>
        void PassAuditNewTopic(string postTableName, string tidlist);
        /// <summary>
        /// 通过待验证的帖子
        /// </summary>
        /// <param name="currentPostTableId"></param>
        /// <param name="pidlist"></param>
        void PassPost(int currentPostTableId, string pidlist);
        /// <summary>
        /// 获取评分记录
        /// </summary>
        /// <param name="pagesize"></param>
        /// <param name="currentpage"></param>
        /// <param name="posttablename"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        DataTable RateLogList(int pagesize, int currentpage, string posttablename, string condition);
        /// <summary>
        /// 获取评分记录
        /// </summary>
        /// <param name="pagesize"></param>
        /// <param name="currentpage"></param>
        /// <param name="posttablename"></param>
        /// <returns></returns>
        DataTable RateLogList(int pagesize, int currentpage, string posttablename);
        /// <summary>
        /// 修复主题
        /// </summary>
        /// <param name="topiclist"></param>
        /// <param name="posttable"></param>
        /// <returns></returns>
        int RepairTopics(string topiclist, string posttable);
        /// <summary>
        /// 清除主题里面已经移走的主题
        /// </summary>
        void ReSetClearMove();
        /// <summary>
        /// 重置错误登录数
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        int ResetErrLoginCount(string ip);
        /// <summary>
        /// 重设用户密码
        /// </summary>
        /// <param name="password"></param>
        /// <param name="uid"></param>
        void ResetPasswordUid(string password, int uid);
        /// <summary>
        /// 重置统计信息
        /// </summary>
        /// <param name="UserCount"></param>
        /// <param name="TopicsCount"></param>
        /// <param name="PostCount"></param>
        /// <param name="lastuserid"></param>
        /// <param name="lastusername"></param>
        void ReSetStatistic(int UserCount, int TopicsCount, int PostCount, string lastuserid, string lastusername);
        /// <summary>
        /// 重置主题类型
        /// </summary>
        /// <param name="topictypeid"></param>
        /// <param name="topiclist"></param>
        /// <returns></returns>
        int ResetTopicTypes(int topictypeid, string topiclist);
        /// <summary>
        /// 重设用户精华帖数
        /// </summary>
        /// <param name="userid"></param>
        void ResetUserDigestPosts(int userid);
        /// <summary>
        /// 还原数据库
        /// </summary>
        /// <param name="backuppath"></param>
        /// <param name="ServerName"></param>
        /// <param name="UserName"></param>
        /// <param name="Password"></param>
        /// <param name="strDbName"></param>
        /// <param name="strFileName"></param>
        /// <returns></returns>
        string RestoreDatabase(string backuppath, string ServerName, string UserName, string Password, string strDbName,
                               string strFileName);
        /// <summary>
        /// 运行SQL语句
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        string RunSql(string sql);
        /// <summary>
        /// 保存板块信息
        /// </summary>
        /// <param name="foruminfo"></param>
        void SaveForumsInfo(ForumInfo __foruminfo);
        /// <summary>
        /// 搜索
        /// </summary>
        /// <param name="posttableid"></param>
        /// <param name="userid"></param>
        /// <param name="usergroupid"></param>
        /// <param name="keyword"></param>
        /// <param name="posterid"></param>
        /// <param name="type"></param>
        /// <param name="searchforumid"></param>
        /// <param name="keywordtype"></param>
        /// <param name="searchtime"></param>
        /// <param name="searchtimetype"></param>
        /// <param name="resultorder"></param>
        /// <param name="resultordertype"></param>
        /// <returns></returns>
        int Search(int posttableid, int userid, int usergroupid, string keyword, int posterid, string type,
                   string searchforumid, int keywordtype, int searchtime, int searchtimetype, int resultorder,
                   int resultordertype);
        /// <summary>
        /// 搜索附件
        /// </summary>
        /// <param name="forumid"></param>
        /// <param name="posttablename"></param>
        /// <param name="filesizemin"></param>
        /// <param name="filesizemax"></param>
        /// <param name="downloadsmin"></param>
        /// <param name="downloadsmax"></param>
        /// <param name="postdatetime"></param>
        /// <param name="filename"></param>
        /// <param name="description"></param>
        /// <param name="poster"></param>
        /// <returns></returns>
        string SearchAttachment(int forumid, string posttablename, string filesizemin, string filesizemax,
                                string downloadsmin, string downloadsmax, string postdatetime, string filename,
                                string description, string poster);
        /// <summary>
        /// 搜索勋章日志
        /// </summary>
        /// <param name="postdatetimeStart"></param>
        /// <param name="postdatetimeEnd"></param>
        /// <param name="Username"></param>
        /// <param name="reason"></param>
        /// <returns></returns>
        string SearchMedalLog(DateTime postdatetimeStart, DateTime postdatetimeEnd, string Username, string reason);
        /// <summary>
        /// 搜索管理日志
        /// </summary>
        /// <param name="postdatetimeStart"></param>
        /// <param name="postdatetimeEnd"></param>
        /// <param name="Username"></param>
        /// <param name="others"></param>
        /// <returns></returns>
        string SearchModeratorManageLog(DateTime postdatetimeStart, DateTime postdatetimeEnd, string Username,
                                        string others);
        /// <summary>
        /// 搜索支付记录
        /// </summary>
        /// <param name="postdatetimeStart"></param>
        /// <param name="postdatetimeEnd"></param>
        /// <param name="Username"></param>
        /// <returns></returns>
        string SearchPaymentLog(DateTime postdatetimeStart, DateTime postdatetimeEnd, string Username);
        /// <summary>
        /// 搜索帖子
        /// </summary>
        /// <param name="forumid"></param>
        /// <param name="posttableid"></param>
        /// <param name="postdatetimeStart"></param>
        /// <param name="postdatetimeEnd"></param>
        /// <param name="poster"></param>
        /// <param name="lowerupper"></param>
        /// <param name="ip"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        string SearchPost(int forumid, string posttableid, DateTime postdatetimeStart, DateTime postdatetimeEnd,
                          string poster, bool lowerupper, string ip, string message);
        /// <summary>
        /// 搜索评分日志
        /// </summary>
        /// <param name="postdatetimeStart"></param>
        /// <param name="postdatetimeEnd"></param>
        /// <param name="Username"></param>
        /// <param name="others"></param>
        /// <returns></returns>
        string SearchRateLog(DateTime postdatetimeStart, DateTime postdatetimeEnd, string Username, string others);
        /// <summary>
        /// 搜索帖子审核
        /// </summary>
        /// <param name="fid"></param>
        /// <param name="poster"></param>
        /// <param name="title"></param>
        /// <param name="moderatorname"></param>
        /// <param name="postdatetimeStart"></param>
        /// <param name="postdatetimeEnd"></param>
        /// <param name="deldatetimeStart"></param>
        /// <param name="deldatetimeEnd"></param>
        /// <returns></returns>
        string SearchTopicAudit(int fid, string poster, string title, string moderatorname, DateTime postdatetimeStart,
                                DateTime postdatetimeEnd, DateTime deldatetimeStart, DateTime deldatetimeEnd);
        /// <summary>
        /// 搜索主题
        /// </summary>
        /// <param name="forumid"></param>
        /// <param name="keyword"></param>
        /// <param name="displayorder"></param>
        /// <param name="digest"></param>
        /// <param name="attachment"></param>
        /// <param name="poster"></param>
        /// <param name="lowerupper"></param>
        /// <param name="viewsmin"></param>
        /// <param name="viewsmax"></param>
        /// <param name="repliesmax"></param>
        /// <param name="repliesmin"></param>
        /// <param name="rate"></param>
        /// <param name="lastpost"></param>
        /// <param name="postdatetimeStart"></param>
        /// <param name="postdatetimeEnd"></param>
        /// <returns></returns>
        string SearchTopics(int forumid, string keyword, string displayorder, string digest, string attachment,
                            string poster, bool lowerupper, string viewsmin, string viewsmax, string repliesmax,
                            string repliesmin, string rate, string lastpost, DateTime postdatetimeStart,
                            DateTime postdatetimeEnd);
        /// <summary>
        /// 搜索访问日志
        /// </summary>
        /// <param name="postdatetimeStart"></param>
        /// <param name="postdatetimeEnd"></param>
        /// <param name="Username"></param>
        /// <param name="others"></param>
        /// <returns></returns>
        string SearchVisitLog(DateTime postdatetimeStart, DateTime postdatetimeEnd, string Username, string others);
        /// <summary>
        /// 发送短消息
        /// </summary>
        /// <param name="msgfrom"></param>
        /// <param name="msgfromid"></param>
        /// <param name="msgto"></param>
        /// <param name="msgtoid"></param>
        /// <param name="folder"></param>
        /// <param name="subject"></param>
        /// <param name="postdatetime"></param>
        /// <param name="message"></param>
        void SendPMToUser(string msgfrom, int msgfromid, string msgto, int msgtoid, int folder, string subject,
                          DateTime postdatetime, string message);
        /// <summary>
        /// 设置管理组信息
        /// </summary>
        /// <param name="admingroupsInfo"></param>
        /// <returns></returns>
        int SetAdminGroupInfo(AdminGroupInfo __admingroupsInfo);
        /// <summary>
        /// 设置勋章为可用
        /// </summary>
        /// <param name="available"></param>
        /// <param name="medailidlist"></param>
        void SetAvailableForMedal(int available, string medailidlist);
        /// <summary>
        /// 设置Discuz!NT代码为可用
        /// </summary>
        /// <param name="idlist"></param>
        /// <param name="status"></param>
        void SetBBCodeAvailableStatus(string idlist, int status);
        /// <summary>
        /// 设置主题置顶状态
        /// </summary>
        /// <param name="topiclist"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        bool SetDisplayorder(string topiclist, int value);
        /// <summary>
        /// 设置版块层级
        /// </summary>
        /// <param name="layer"></param>
        /// <param name="parentidlist"></param>
        /// <param name="fid"></param>
        void SetForumslayer(int layer, string parentidlist, int fid);
        /// <summary>
        /// 设置板块路径
        /// </summary>
        /// <param name="pathlist"></param>
        /// <param name="fid"></param>
        void SetForumsPathList(string pathlist, int fid);
        /// <summary>
        /// 设置版主
        /// </summary>
        /// <param name="moderator"></param>
        void SetModerator(string moderator);
        /// <summary>
        /// 设置新主题属性
        /// </summary>
        /// <param name="topicid"></param>
        /// <param name="Replies"></param>
        /// <param name="lastpostid"></param>
        /// <param name="lastposterid"></param>
        /// <param name="lastposter"></param>
        /// <param name="lastpost"></param>
        void SetNewTopicProperty(int topicid, int Replies, int lastpostid, int lastposterid, string lastposter,
                                 DateTime lastpost);
        /// <summary>
        /// 设置主帖
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="tid"></param>
        /// <param name="postid"></param>
        /// <param name="posttableid"></param>
        void SetPrimaryPost(string subject, int tid, string[] postid, string posttableid);
        /// <summary>
        /// 设置短消息状态
        /// </summary>
        /// <param name="pmid"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        int SetPrivateMessageState(int pmid, byte state);
        /// <summary>
        /// 设置真实的主题数
        /// </summary>
        /// <param name="fid"></param>
        /// <returns></returns>
        int SetRealCurrentTopics(int fid);
        /// <summary>
        /// 设置版块状态
        /// </summary>
        /// <param name="status"></param>
        /// <param name="fid"></param>
        void SetStatusInForum(int status, int fid);
        /// <summary>
        /// 禁言用户
        /// </summary>
        /// <param name="uidlist"></param>
        void SetStopTalkUser(string uidlist);
        /// <summary>
        /// 关闭主题
        /// </summary>
        /// <param name="topiclist"></param>
        /// <param name="intValue"></param>
        /// <returns></returns>
        int SetTopicClose(string topiclist, short intValue);
        /// <summary>
        /// 设置主题状态
        /// </summary>
        /// <param name="topiclist"></param>
        /// <param name="field"></param>
        /// <param name="intValue"></param>
        /// <returns></returns>
        int SetTopicStatus(string topiclist, string field, byte intValue);
        /// <summary>
        /// 设置主题状态
        /// </summary>
        /// <param name="topiclist"></param>
        /// <param name="field"></param>
        /// <param name="intValue"></param>
        /// <returns></returns>
        int SetTopicStatus(string topiclist, string field, int intValue);
        /// <summary>
        /// 设置主题状态
        /// </summary>
        /// <param name="topiclist"></param>
        /// <param name="field"></param>
        /// <param name="intValue"></param>
        /// <returns></returns>
        int SetTopicStatus(string topiclist, string field, string intValue);
        /// <summary>
        /// 设置主题分类Id
        /// </summary>
        /// <param name="topiclist"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        bool SetTypeid(string topiclist, int value);
        /// <summary>
        /// 设置用户新短消息数
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="pmnum"></param>
        /// <returns></returns>
        int SetUserNewPMCount(int uid, int pmnum);
        /// <summary>
        /// 设置用户在线状态
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="onlinestate"></param>
        /// <returns></returns>
        int SetUserOnlineState(int uid, int onlinestate);
        /// <summary>
        /// 收缩数据库
        /// </summary>
        /// <param name="shrinksize"></param>
        /// <param name="dbname"></param>
        void ShrinkDataBase(string shrinksize, string dbname);
        /// <summary>
        /// 开始填充全文索引
        /// </summary>
        /// <param name="dbname"></param>
        /// <returns></returns>
        int StartFullIndex(string dbname);
        /// <summary>
        /// 测试全文索引
        /// </summary>
        /// <param name="posttableid"></param>
        void TestFullTextIndex(int posttableid);
        /// <summary>
        /// 更新用户动作
        /// </summary>
        /// <param name="olid"></param>
        /// <param name="action"></param>
        /// <param name="fid"></param>
        /// <param name="forumname"></param>
        /// <param name="tid"></param>
        /// <param name="topictitle"></param>
        void UpdateAction(int olid, int action, int fid, string forumname, int tid, string topictitle);
        /// <summary>
        /// 更新用户动作
        /// </summary>
        /// <param name="olid"></param>
        /// <param name="action"></param>
        /// <param name="inid"></param>
        void UpdateAction(int olid, int action, int inid);
        /// <summary>
        /// 更新广告
        /// </summary>
        /// <param name="aid"></param>
        /// <param name="available"></param>
        /// <param name="type"></param>
        /// <param name="displayorder"></param>
        /// <param name="title"></param>
        /// <param name="targets"></param>
        /// <param name="parameters"></param>
        /// <param name="code"></param>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <returns></returns>
        int UpdateAdvertisement(int aid, int available, string type, int displayorder, string title, string targets,
                                string parameters, string code, string starttime, string endtime);
        /// <summary>
        /// 更新广告可用状态
        /// </summary>
        /// <param name="aidlist"></param>
        /// <param name="available"></param>
        /// <returns></returns>
        int UpdateAdvertisementAvailable(string aidlist, int available);
        /// <summary>
        /// 更新通告
        /// </summary>
        /// <param name="id"></param>
        /// <param name="poster"></param>
        /// <param name="title"></param>
        /// <param name="displayorder"></param>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <param name="message"></param>
        void UpdateAnnouncement(int id, string poster, string title, int displayorder, string starttime, string endtime,
                                string message);
        /// <summary>
        /// 更新通告作者
        /// </summary>
        /// <param name="posterid"></param>
        /// <param name="poster"></param>
        void UpdateAnnouncementPoster(int posterid, string poster);
        /// <summary>
        /// 更新附件
        /// </summary>
        /// <param name="attachmentInfo"></param>
        /// <returns></returns>
        int UpdateAttachment(AttachmentInfo attachmentInfo);
        /// <summary>
        /// 更新附件
        /// </summary>
        /// <param name="aid"></param>
        /// <param name="readperm"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        int UpdateAttachment(int aid, int readperm, string description);
        /// <summary>
        /// 更新附件
        /// </summary>
        /// <param name="aid"></param>
        void UpdateAttachmentDownloads(int aid);
        /// <summary>
        /// 更新附件到另一主题
        /// </summary>
        /// <param name="oldtid"></param>
        /// <param name="newtid"></param>
        /// <returns></returns>
        int UpdateAttachmentTidToAnotherTopic(int oldtid, int newtid);
        /// <summary>
        /// 更新允许的附件类型
        /// </summary>
        /// <param name="extension"></param>
        /// <param name="maxsize"></param>
        /// <param name="id"></param>
        void UpdateAttchType(string extension, string maxsize, int id);
        /// <summary>
        /// 更新认证串
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="authstr"></param>
        /// <param name="authflag"></param>
        void UpdateAuthStr(int uid, string authstr, int authflag);
        /// <summary>
        /// 更新Discuz!NT代码
        /// </summary>
        /// <param name="available"></param>
        /// <param name="tag"></param>
        /// <param name="icon"></param>
        /// <param name="replacement"></param>
        /// <param name="example"></param>
        /// <param name="explanation"></param>
        /// <param name="param"></param>
        /// <param name="nest"></param>
        /// <param name="paramsdescript"></param>
        /// <param name="paramsdefvalue"></param>
        /// <param name="id"></param>
        void UpdateBBCCode(int available, string tag, string icon, string replacement, string example,
                           string explanation, string param, string nest, string paramsdescript, string paramsdefvalue,
                           int id);
        /// <summary>
        /// 更新帖子分表
        /// </summary>
        /// <param name="fid"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        int UpdateDetachTable(int fid, string description);
        /// <summary>
        /// 更新版块显示顺序
        /// </summary>
        /// <param name="displayorder"></param>
        /// <param name="fid"></param>
        void UpdateDisplayorderInForumByFid(int displayorder, int fid);
        /// <summary>
        /// 更新Email验证信息
        /// </summary>
        /// <param name="authstr"></param>
        /// <param name="authtime"></param>
        /// <param name="uid"></param>
        void UpdateEmailValidateInfo(string authstr, DateTime authtime, int uid);
        /// <summary>
        /// 更新版块信息
        /// </summary>
        /// <param name="fid"></param>
        /// <param name="name"></param>
        /// <param name="subforumcount"></param>
        /// <param name="displayorder"></param>
        /// <returns></returns>
        int UpdateForum(int fid, string name, int subforumcount, int displayorder);
        /// <summary>
        /// 更新版块信息
        /// </summary>
        /// <param name="fid"></param>
        /// <param name="topiccount"></param>
        /// <param name="postcount"></param>
        /// <param name="lasttid"></param>
        /// <param name="lasttitle"></param>
        /// <param name="lastpost"></param>
        /// <param name="lastposterid"></param>
        /// <param name="lastposter"></param>
        /// <param name="todaypostcount"></param>
        void UpdateForum(int fid, int topiccount, int postcount, int lasttid, string lasttitle, string lastpost,
                         int lastposterid, string lastposter, int todaypostcount);
        /// <summary>
        /// 更新版块和用户模板Id
        /// </summary>
        /// <param name="templateidlist"></param>
        void UpdateForumAndUserTemplateId(string templateidlist);
        /// <summary>
        /// 更新版块字段
        /// </summary>
        /// <param name="fid"></param>
        /// <param name="fieldname"></param>
        /// <param name="fieldvalue"></param>
        /// <returns></returns>
        int UpdateForumField(int fid, string fieldname, string fieldvalue);
        /// <summary>
        /// 更新版块字段
        /// </summary>
        /// <param name="fid"></param>
        /// <param name="fieldname"></param>
        /// <returns></returns>
        int UpdateForumField(int fid, string fieldname);
        /// <summary>
        /// 更新论坛友情链接
        /// </summary>
        /// <param name="id"></param>
        /// <param name="displayorder"></param>
        /// <param name="name"></param>
        /// <param name="url"></param>
        /// <param name="note"></param>
        /// <param name="logo"></param>
        /// <returns></returns>
        int UpdateForumLink(int id, int displayorder, string name, string url, string note, string logo);
        /// <summary>
        /// 更新版块显示顺序
        /// </summary>
        /// <param name="minDisplayOrder"></param>
        void UpdateForumsDisplayOrder(int minDisplayOrder);
        /// <summary>
        /// 更改用户所属用户组
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="groupid"></param>
        void UpdateGroupid(int userid, int groupid);
        /// <summary>
        /// 更新隐身状态
        /// </summary>
        /// <param name="olid"></param>
        /// <param name="invisible"></param>
        void UpdateInvisible(int olid, int invisible);
        /// <summary>
        /// 更新在线用户IP
        /// </summary>
        /// <param name="olid"></param>
        /// <param name="ip"></param>
        void UpdateIP(int olid, string ip);
        /// <summary>
        /// 更新用户最后活动时间
        /// </summary>
        /// <param name="olid"></param>
        void UpdateLastTime(int olid);
        /// <summary>
        /// 更新勋章
        /// </summary>
        /// <param name="medalid"></param>
        /// <param name="name"></param>
        /// <param name="image"></param>
        void UpdateMedal(int medalid, string name, string image);
        /// <summary>
        /// 更新勋章
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="medals"></param>
        void UpdateMedals(int uid, string medals);
        /// <summary>
        /// 更新勋章日志
        /// </summary>
        /// <param name="newactions"></param>
        /// <param name="postdatetime"></param>
        /// <param name="reason"></param>
        /// <param name="oldactions"></param>
        /// <param name="medals"></param>
        /// <param name="uid"></param>
        void UpdateMedalslog(string newactions, DateTime postdatetime, string reason, string oldactions, int medals,
                             int uid);
        /// <summary>
        /// 更新勋章日志
        /// </summary>
        /// <param name="actions"></param>
        /// <param name="postdatetime"></param>
        /// <param name="reason"></param>
        /// <param name="uid"></param>
        void UpdateMedalslog(string actions, DateTime postdatetime, string reason, int uid);
        /// <summary>
        /// 更新勋章日志
        /// </summary>
        /// <param name="newactions"></param>
        /// <param name="postdatetime"></param>
        /// <param name="reason"></param>
        /// <param name="oldactions"></param>
        /// <param name="medalidlist"></param>
        /// <param name="uid"></param>
        void UpdateMedalslog(string newactions, DateTime postdatetime, string reason, string oldactions,
                             string medalidlist, int uid);
        /// <summary>
        /// 更新帖子分表最大最小主题Id
        /// </summary>
        /// <param name="posttablename"></param>
        /// <param name="posttableid"></param>
        /// <returns></returns>
        int UpdateMinMaxField(string posttablename, int posttableid);
        /// <summary>
        /// 更新版块版主
        /// </summary>
        /// <param name="fid"></param>
        /// <param name="moderators"></param>
        void UpdateModerators(int fid, string moderators);
        /// <summary>
        /// 更新在线表
        /// </summary>
        /// <param name="groupid"></param>
        /// <param name="displayorder"></param>
        /// <param name="img"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        int UpdateOnlineList(int groupid, int displayorder, string img, string title);
        /// <summary>
        /// 更新在线表
        /// </summary>
        /// <param name="usergroupinfo"></param>
        void UpdateOnlineList(UserGroupInfo __usergroupinfo);
        /// <summary>
        /// 更新密码
        /// </summary>
        /// <param name="olid"></param>
        /// <param name="password"></param>
        void UpdatePassword(int olid, string password);
        /// <summary>
        /// 更新短消息接受人
        /// </summary>
        /// <param name="msgtoid"></param>
        /// <param name="msgto"></param>
        void UpdatePMReceiver(int msgtoid, string msgto);
        /// <summary>
        /// 更新短消息发送人
        /// </summary>
        /// <param name="msgfromid"></param>
        /// <param name="msgfrom"></param>
        void UpdatePMSender(int msgfromid, string msgfrom);
        /// <summary>
        /// 更新投票信息
        /// </summary>
        /// <param name="tid"></param>
        /// <param name="polltype"></param>
        /// <param name="itemcount"></param>
        /// <param name="itemnamelist"></param>
        /// <param name="itemvaluelist"></param>
        /// <param name="enddatetime"></param>
        /// <returns></returns>
        //bool UpdatePoll(int tid, int polltype, int itemcount, string itemnamelist, string itemvaluelist,
        //                string enddatetime);
        bool UpdatePoll(PollInfo pollinfo);
        /// <summary>
        /// 更新投票信息
        /// </summary>
        /// <param name="tid"></param>
        /// <param name="usernamelist"></param>
        /// <param name="newselitemidlist"></param>
        /// <returns></returns>
        //int UpdatePoll(int tid, string usernamelist, StringBuilder newselitemidlist);
        /// <summary>
        /// 更新帖子信息
        /// </summary>
        /// <param name="postsInfo"></param>
        /// <param name="posttableid"></param>
        /// <returns></returns>
        int UpdatePost(PostInfo __postsInfo, string posttableid);
        /// <summary>
        /// 更新帖子信息
        /// </summary>
        /// <param name="topiclist"></param>
        /// <param name="fid"></param>
        /// <param name="posttable"></param>
        void UpdatePost(string topiclist, int fid, string posttable);
        /// <summary>
        /// 更新帖子是否包含附件
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="postTableId"></param>
        /// <param name="hasAttachment"></param>
        /// <returns></returns>
        int UpdatePostAttachment(int pid, string postTableId, int hasAttachment);
        /// <summary>
        /// 更新帖子附件类型
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="postTableId"></param>
        /// <param name="attType"></param>
        /// <returns></returns>
        int UpdatePostAttachmentType(int pid, string postTableId, int attType);
        /// <summary>
        /// 更新发送短消息时间
        /// </summary>
        /// <param name="olid"></param>
        void UpdatePostPMTime(int olid);
        /// <summary>
        /// 更新帖子作者
        /// </summary>
        /// <param name="posterid"></param>
        /// <param name="poster"></param>
        /// <param name="posttableid"></param>
        void UpdatePostPoster(int posterid, string poster, string posttableid);
        /// <summary>
        /// 更新帖子评分
        /// </summary>
        /// <param name="tid"></param>
        /// <param name="postidlist"></param>
        /// <param name="posttableid"></param>
        /// <returns></returns>
        int UpdatePostRateTimes(int tid, string postidlist, string posttableid);
        /// <summary>
        /// 更新帖子评分
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="rate"></param>
        /// <param name="posttableid"></param>
        /// <returns></returns>
        int UpdatePostRate(int pid, float rate, string posttableid);
        /// <summary>
        /// 取消帖子评分
        /// </summary>
        /// <param name="postidlist"></param>
        /// <param name="posttableid"></param>
        /// <returns></returns>
        int CancelPostRate(string postidlist, string posttableid);
        /// <summary>
        /// 更新帖子所属主题
        /// </summary>
        /// <param name="postidlist"></param>
        /// <param name="tid"></param>
        /// <param name="posttableid"></param>
        void UpdatePostTid(string postidlist, int tid, string posttableid);
        /// <summary>
        /// 更新帖子到另一主题
        /// </summary>
        /// <param name="oldtid"></param>
        /// <param name="newtid"></param>
        /// <param name="posttableid"></param>
        /// <returns></returns>
        int UpdatePostTidToAnotherTopic(int oldtid, int newtid, string posttableid);
        /// <summary>
        /// 更新最后发帖时间
        /// </summary>
        /// <param name="olid"></param>
        void UpdatePostTime(int olid);
        /// <summary>
        /// 更新评分范围
        /// </summary>
        /// <param name="raterange"></param>
        /// <param name="groupid"></param>
        void UpdateRateRange(string raterange, int groupid);
        /// <summary>
        /// 更新评分范围
        /// </summary>
        /// <param name="raterange"></param>
        /// <param name="groupid"></param>
        void UpdateRaterangeByGroupid(string raterange, int groupid);
        /// <summary>
        /// 更新最后搜索时间
        /// </summary>
        /// <param name="olid"></param>
        void UpdateSearchTime(int olid);
        /// <summary>
        /// 更新表情
        /// </summary>
        /// <param name="id"></param>
        /// <param name="displayorder"></param>
        /// <param name="type"></param>
        /// <param name="code"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        int UpdateSmilies(int id, int displayorder, int type, string code, string url);
        /// <summary>
        /// 更新统计信息
        /// </summary>
        /// <param name="param"></param>
        /// <param name="strValue"></param>
        /// <returns></returns>
        int UpdateStatistics(string param, string strValue);
        /// <summary>
        /// 更新统计信息
        /// </summary>
        /// <param name="param"></param>
        /// <param name="intValue"></param>
        /// <returns></returns>
        int UpdateStatistics(string param, int intValue);
        /// <summary>
        /// 更新最后注册用户
        /// </summary>
        /// <param name="lastuserid"></param>
        /// <param name="lastusername"></param>
        void UpdateStatisticsLastUserName(int lastuserid, string lastusername);
        /// <summary>
        /// 更新版块状态
        /// </summary>
        /// <param name="fidlist"></param>
        void UpdateStatusByFidlist(string fidlist);
        /// <summary>
        /// 更新版块状态
        /// </summary>
        /// <param name="fidlist"></param>
        void UpdateStatusByFidlistOther(string fidlist);
        /// <summary>
        /// 更新子版块数
        /// </summary>
        /// <param name="subforumcount"></param>
        /// <param name="fid"></param>
        void UpdateSubForumCount(int subforumcount, int fid);
        /// <summary>
        /// 更新子版块数
        /// </summary>
        /// <param name="fid"></param>
        void UpdateSubForumCount(int fid);
        /// <summary>
        /// 更新主题信息
        /// </summary>
        /// <param name="tid"></param>
        /// <param name="topicinfo"></param>
        /// <returns></returns>
        int UpdateTopic(int tid, TopicInfo __topicinfo);
        /// <summary>
        /// 更新主题信息
        /// </summary>
        /// <param name="tid"></param>
        /// <param name="postcount"></param>
        /// <param name="lastpostid"></param>
        /// <param name="lastpost"></param>
        /// <param name="lastposterid"></param>
        /// <param name="poster"></param>
        void UpdateTopic(int tid, int postcount, int lastpostid, string lastpost, int lastposterid, string poster);
        /// <summary>
        /// 更新主题信息
        /// </summary>
        /// <param name="topiclist"></param>
        /// <param name="fid"></param>
        /// <returns></returns>
        int UpdateTopic(string topiclist, int fid);
        /// <summary>
        /// 更新主题信息
        /// </summary>
        /// <param name="topicinfo"></param>
        /// <returns></returns>
        int UpdateTopic(TopicInfo topicinfo);
        /// <summary>
        /// 更新主题信息
        /// </summary>
        /// <param name="topicinfo"></param>
        /// <returns></returns>
        bool UpdateTopicAllInfo(TopicInfo topicinfo);
        /// <summary>
        /// 更新主题附件
        /// </summary>
        /// <param name="tid"></param>
        /// <param name="hasAttachment"></param>
        /// <returns></returns>
        int UpdateTopicAttachment(int tid, int hasAttachment);
        /// <summary>
        /// 更新主题附件类型
        /// </summary>
        /// <param name="tid"></param>
        /// <param name="attType"></param>
        /// <returns></returns>
        int UpdateTopicAttachmentType(int tid, int attType);
        /// <summary>
        /// 更新主题是否需要回复可见
        /// </summary>
        /// <param name="tid"></param>
        /// <returns></returns>
        int UpdateTopicHide(int tid);
        /// <summary>
        /// 更新主题图标
        /// </summary>
        /// <param name="tid"></param>
        /// <param name="iconid"></param>
        /// <returns></returns>
        int UpdateTopicIconID(int tid, int iconid);
        /// <summary>
        /// 更新主题最后发贴人
        /// </summary>
        /// <param name="lastposterid"></param>
        /// <param name="lastposter"></param>
        void UpdateTopicLastPoster(int lastposterid, string lastposter);
        /// <summary>
        /// 更新主题最后发帖人Id
        /// </summary>
        /// <param name="tid"></param>
        void UpdateTopicLastPosterId(int tid);
        /// <summary>
        /// 更新主题管理信息
        /// </summary>
        /// <param name="topiclist"></param>
        /// <param name="moderated"></param>
        /// <returns></returns>
        int UpdateTopicModerated(string topiclist, int moderated);
        /// <summary>
        /// 更新主题发帖人
        /// </summary>
        /// <param name="posterid"></param>
        /// <param name="poster"></param>
        void UpdateTopicPoster(int posterid, string poster);
        /// <summary>
        /// 更新主题售价
        /// </summary>
        /// <param name="tid"></param>
        /// <param name="price"></param>
        /// <returns></returns>
        int UpdateTopicPrice(int tid, int price);
        /// <summary>
        /// 更新主题售价
        /// </summary>
        /// <param name="tid"></param>
        /// <returns></returns>
        int UpdateTopicPrice(int tid);
        /// <summary>
        /// 更新主题阅读权限
        /// </summary>
        /// <param name="tid"></param>
        /// <param name="readperm"></param>
        /// <returns></returns>
        int UpdateTopicReadperm(int tid, int readperm);
        /// <summary>
        /// 更新主题回复数
        /// </summary>
        /// <param name="tid"></param>
        /// <param name="posttableid"></param>
        void UpdateTopicReplies(int tid, string posttableid);
        /// <summary>
        /// 更新主题回复数
        /// </summary>
        /// <param name="tid"></param>
        /// <param name="topicreplies"></param>
        /// <returns></returns>
        int UpdateTopicReplies(int tid, int topicreplies);
        /// <summary>
        /// 更新主题标题
        /// </summary>
        /// <param name="tid"></param>
        /// <param name="topictitle"></param>
        /// <returns></returns>
        int UpdateTopicTitle(int tid, string topictitle);
        /// <summary>
        /// 更新主题浏览量
        /// </summary>
        /// <param name="tid"></param>
        /// <param name="viewcount"></param>
        /// <returns></returns>
        int UpdateTopicViewCount(int tid, int viewcount);
        /// <summary>
        /// 更新用户信息
        /// </summary>
        /// <param name="userinfo"></param>
        void UpdateUserAllInfo(UserInfo userinfo);
        /// <summary>
        /// 更新用户个性设置
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="avatar"></param>
        /// <param name="avatarwidth"></param>
        /// <param name="avatarheight"></param>
        /// <param name="templateid"></param>
        void UpdateUserPreference(int uid, string avatar, int avatarwidth, int avatarheight, int templateid);
        /// <summary>
        /// 更新用户积分
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="credits"></param>
        /// <param name="extcredits1"></param>
        /// <param name="extcredits2"></param>
        /// <param name="extcredits3"></param>
        /// <param name="extcredits4"></param>
        /// <param name="extcredits5"></param>
        /// <param name="extcredits6"></param>
        /// <param name="extcredits7"></param>
        /// <param name="extcredits8"></param>
        void UpdateUserCredits(int userid, float credits, float extcredits1, float extcredits2, float extcredits3,
                               float extcredits4, float extcredits5, float extcredits6, float extcredits7,
                               float extcredits8);
        /// <summary>
        /// 更新用户积分
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="extcreditsid"></param>
        /// <param name="score"></param>
        void UpdateUserCredits(int userid, int extcreditsid, float score);
        /// <summary>
        /// 更新用户积分
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="credits"></param>
        void UpdateUserCredits(int uid, string credits);
        /// <summary>
        /// 更新用户积分
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="values"></param>
        void UpdateUserCredits(int uid, float[] values);
        /// <summary>
        /// 更新用户积分
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="values"></param>
        /// <param name="pos"></param>
        /// <param name="mount"></param>
        void UpdateUserCredits(int uid, DataRow values, int pos, int mount);
        /// <summary>
        /// 更新用户精华数
        /// </summary>
        /// <param name="useridlist"></param>
        /// <returns></returns>
        int UpdateUserDigest(string useridlist);
        /// <summary>
        /// 更新用户扩展积分
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="extid"></param>
        /// <param name="pos"></param>
        void UpdateUserExtCredits(int uid, int extid, float pos);
        /// <summary>
        /// 更新用户字段
        /// </summary>
        /// <param name="userinfo"></param>
        /// <param name="signature"></param>
        /// <param name="authstr"></param>
        /// <param name="sightml"></param>
        void UpdateUserField(UserInfo userinfo, string signature, string authstr, string sightml);
        /// <summary>
        /// 更新用户论坛设置
        /// </summary>
        /// <param name="userinfo"></param>
        void UpdateUserForumSetting(UserInfo userinfo);
        /// <summary>
        /// 更新用户组
        /// </summary>
        /// <param name="usergroupinfo"></param>
        /// <param name="Creditshigher"></param>
        /// <param name="Creditslower"></param>
        void UpdateUserGroup(UserGroupInfo usergroupinfo, int Creditshigher, int Creditslower);
        /// <summary>
        /// 更新用户组
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="groupid"></param>
        void UpdateUserGroup(int uid, int groupid);
        /// <summary>
        /// 更新用户组
        /// </summary>
        /// <param name="currentGroupID"></param>
        /// <param name="Creditslower"></param>
        void UpdateUserGroupCreditsHigher(int currentGroupID, int Creditslower);
        /// <summary>
        /// 更新用户组
        /// </summary>
        /// <param name="currentCreditsHigher"></param>
        /// <param name="Creditshigher"></param>
        void UpdateUserGroupCreidtsLower(int currentCreditsHigher, int Creditshigher);
        /// <summary>
        /// 更新用户组
        /// </summary>
        /// <param name="groupid"></param>
        void UpdateUserGroupLowerAndHigherToLimit(int groupid);
        /// <summary>
        /// 更新用户组
        /// </summary>
        /// <param name="Creditshigher"></param>
        /// <param name="Creditslower"></param>
        void UpdateUserGroupsCreditsHigherByCreditsHigher(int Creditshigher, int Creditslower);
        /// <summary>
        /// 更新用户组
        /// </summary>
        /// <param name="Creditslower"></param>
        /// <param name="Creditshigher"></param>
        void UpdateUserGroupsCreditsLowerByCreditsLower(int Creditslower, int Creditshigher);
        /// <summary>
        /// 更新用户最后访问时间
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="ip"></param>
        void UpdateUserLastvisit(int uid, string ip);
        /// <summary>
        /// 更新用户在线信息
        /// </summary>
        /// <param name="groupid"></param>
        /// <param name="userid"></param>
        void UpdateUserOnlineInfo(int groupid, int userid);
        /// <summary>
        /// 更新用户在线信息
        /// </summary>
        /// <param name="uidlist"></param>
        /// <param name="onlinestate"></param>
        /// <param name="activitytime"></param>
        void UpdateUserOnlineStateAndLastActivity(string uidlist, int onlinestate, string activitytime);
        /// <summary>
        /// 更新用户在线信息
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="onlinestate"></param>
        /// <param name="activitytime"></param>
        void UpdateUserOnlineStateAndLastActivity(int uid, int onlinestate, string activitytime);
        /// <summary>
        /// 更新用户在线信息
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="onlinestate"></param>
        /// <param name="activitytime"></param>
        void UpdateUserOnlineStateAndLastVisit(int uid, int onlinestate, string activitytime);
        /// <summary>
        /// 更新用户在线信息
        /// </summary>
        /// <param name="uidlist"></param>
        /// <param name="onlinestate"></param>
        /// <param name="activitytime"></param>
        void UpdateUserOnlineStateAndLastVisit(string uidlist, int onlinestate, string activitytime);
        /// <summary>
        /// 更新用户在线时间
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="activitytime"></param>
        void UpdateUserLastActivity(int uid, string activitytime);
        /// <summary>
        /// 更新用户其他信息
        /// </summary>
        /// <param name="groupid"></param>
        /// <param name="userid"></param>
        void UpdateUserOtherInfo(int groupid, int userid);
        /// <summary>
        /// 更新用户密码
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="password"></param>
        /// <param name="originalpassword"></param>
        void UpdateUserPassword(int uid, string password, bool originalpassword);
        /// <summary>
        /// 更新用户发帖数
        /// </summary>
        /// <param name="postcount"></param>
        /// <param name="userid"></param>
        void UpdateUserPostCount(int postcount, int userid);
        /// <summary>
        /// 更新用户个人设置
        /// </summary>
        /// <param name="userinfo"></param>
        void UpdateUserProfile(UserInfo userinfo);
        /// <summary>
        /// 更新用户安全提示设置
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="secques"></param>
        void UpdateUserSecques(int uid, string secques);
        /// <summary>
        /// 更新用户空间Id
        /// </summary>
        /// <param name="userid"></param>
        void UpdateUserSpaceId(int userid);
        /// <summary>
        /// 更新用户空间Id
        /// </summary>
        /// <param name="spaceid"></param>
        /// <param name="userid"></param>
        void UpdateUserSpaceId(int spaceid, int userid);
        /// <summary>
        /// 更新屏蔽词语设置
        /// </summary>
        /// <param name="id"></param>
        /// <param name="find"></param>
        /// <param name="replacement"></param>
        /// <returns></returns>
        int UpdateWord(int id, string find, string replacement);
        /// <summary>
        /// 保存统计信息
        /// </summary>
        /// <param name="dr"></param>
        void SaveStatisticsRow(DataRow dr);
        /// <summary>
        /// 删除所有词语过滤
        /// </summary>
        void DeleteBadWords();
        /// <summary>
        /// 更新词语过滤
        /// </summary>
        /// <param name="find"></param>
        /// <param name="replacement"></param>
        void UpdateBadWords(string find, string replacement);
        /// <summary>
        /// 增加词语过滤
        /// </summary>
        /// <param name="username"></param>
        /// <param name="find"></param>
        /// <param name="replacement"></param>
        void InsertBadWords(string username, string find, string replacement);
        int GetBadWordId(string find);

        bool IsExistTopicType(string typename, int currenttypeid);
        string GetTopicTypes();
        DataTable GetExistTopicTypeOfForum();
        void UpdateTopicTypeForForum(string topictypes, int fid);
        void UpdateTopicTypes(string name, int displayorder, string description, int typeid);
        bool IsExistTopicType(string typename);
        void AddTopicTypes(string typename, int displayorder, string description);
        int GetMaxTopicTypesId();
        void DeleteTopicTypesByTypeidlist(string typeidlist);
        DataTable GetForumNameIncludeTopicType();
        DataTable GetForumTopicType();
        void ClearTopicTopicType(int typeid);
        string GetTopicTypeInfo();
        string GetTemplateName();
        DataTable GetAttachType();
        void UpdatePermUserListByFid(string permuserlist, int fid);
        bool IsExistSmilieCode(string code, int currentid);
        int UpdateSmiliesPart(string code, int displayorder, int id);
        string GetSmilieByType(int id);
        DataTable GetSmiliesInfoByType(int type);

        DataTable GetTopicListByCondition(string postname, int forumid, string posterlist, string keylist, string startdate,
                                          string enddate, int pageSize, int currentPage);

        int GetTopicListCountByCondition(string postname, int forumid, string posterlist, string keylist, string startdate,
                                         string enddate);

        DataTable GetTopicListByTidlist(string posttableid, string tidlist);


        DataTable GetWebSiteAggForumTopicList(string showtype, int topnumber);
        /// <summary>
        /// 获取热门版块
        /// </summary>
        /// <param name="topnumber">获取数量</param>
        /// <returns></returns>
        DataTable GetWebSiteAggHotForumList(int topnumber);
        IDataReader GetTopicsIdentifyItem();


        IDataReader GetHelpList(int id);
        IDataReader ShowHelp(int id);
        IDataReader GetHelpClass();
        void AddHelp(string title, string message, int pid, int orderby);
        void DelHelp(string idlist);
        void ModHelp(int id, string title, string message, int pid, int orderby);
        int HelpCount();
        string BindHelpType();
        void UpOrder(string orderby, string id);



        int GetUserIdByRewriteName(string rewritename);

        void IdentifyTopic(string topiclist, int identify);
        void UpdateUserPMSetting(UserInfo user);
        String AddTableData();

        IDataReader GetUserInfoByName(string username);
        string ResetTopTopicListSql(int layer, string fid, string parentidlist);
        string showforumcondition(int sqlid, int cond);
        string Global_UserGrid_GetCondition(string getstring);
        int Global_UserGrid_RecordCount();
        int Global_UserGrid_RecordCount(string condition);
        string Global_UserGrid_SearchCondition(bool islike, bool ispostdatetime, string username, string nickname, string UserGroup, string emai, string credits_start, string credits_end, string lastip, string posts, string digestposts, string uid, string joindateStart, string joindateEnd);
        DataTable Global_UserGrid_Top2(string searchcondition);
        DataTable UserList(int pagesize, int currentpage, string condition);
        System.Collections.ArrayList CheckDbFree();
        void DbOptimize(string tablelist);
        void UpdateTopic(int tid, string title, int posterid, string poster);
        //string AnnounceSerachBind(string poster, string title, string postdatetimeStart, string postdatetimeEnd);
        void UpdateAdminUsergroup(string targetadminusergroup, string sourceadminusergroup);
        void UpdateUserCredits(string formula);
        DataTable MailListTable(string usernamelist);
        string DelVisitLogCondition(string deletemod, string visitid, string deletenum, string deletefrom);
        string AttachDataBind(string condition, string postname);
        DataTable GetAttachDataTable(string condition, string postname);
        bool AuditTopicCount(string condition);
        string AuditTopicBindStr(string condition);
        DataTable AuditTopicBind(string condition);
        string AuditNewUserClear(string searchuser, string regbefore, string regip);
        string DelMedalLogCondition(string deletemode, string id, string deleteNum, string deleteFrom);
        DataTable MedalsTable(string medalid);
        string DelModeratorManageCondition(string deletemode, string id, string deleteNum, string deleteFrom);
        DataTable GroupNameTable(string groupid);
        string PaymentLogCondition(string deletemode, string id, string deleteNum, string deleteFrom);
        string PostGridBind(string posttablename, string condition);
        string DelRateScoreLogCondition(string deletemode, string id, string deleteNum, string deleteFrom);
        string GetTopicCountCondition(out string type, string gettype, int getnewtopic);
        void UpdatePostSP();
        void CreateStoreProc(int tablelistmaxid);
        void DeleteSmilyByType(int type);
        void UpdateMyTopic();
        void UpdateMyPost();
        string GetAllIdentifySql();
        DataTable GetAllIdentify();

        bool UpdateIdentifyById(int id, string name);
        bool AddIdentify(string name, string filename);
        void DeleteIdentify(string idlist);
        DataTable GetExistMedalList();


        IDataReader GetSitemapNewTopics(string p);
        void UpdateUserGroupTitleAndCreditsByGroupid(int groupid, string grouptitle, int creditslower, int creditshigher);
        string GetRateLogCountCondition(int userid, string postidlist);
        DataTable GetOtherPostId(string postidlist, int topicid, int postid);
        int GetSpecifyForumTemplateCount();
        int DeleteAttachmentByPid(int pid);
        IDataReader GetOnlineUser(int olid);

        int GetOlidByUid(int uid);
        DataTable GetUsers(string idlist);


        IDataReader GetAttachmentByUid(int uid, string extlist, int pageIndex, int pageSize);
        int GetUserAttachmentCount(int uid);
        int GetUserAttachmentCount(int uid, string extlist);
        IDataReader GetAttachmentByUid(int uid, int pageIndex, int pageSize);
        void DelMyAttachmentByTid(string tidlist);
        void DelMyAttachmentByPid(string pidlist);
        void DelMyAttachmentByAid(string aidlist);

        /// <summary>
        /// 创建主题标签(已存在的标签不会被创建)
        /// </summary>
        /// <param name="tags">标签, 以半角空格分隔</param>
        /// <param name="topicid">主题Id</param>
        /// <param name="userid">用户Id</param>
        /// <param name="curdatetime">提交时间</param>
        void CreateTopicTags(string tags, int topicid, int userid, string curdatetime);
        /// <summary>
        /// 获取主题所包含的Tag
        /// </summary>
        /// <param name="topicid">主题Id</param>
        /// <returns></returns>
        IDataReader GetTagsListByTopic(int topicid);
        /// <summary>
        /// 获取论坛热门标签
        /// </summary>
        /// <param name="count">数量</param>
        /// <returns></returns>
        IDataReader GetHotTagsListForForum(int count);
        /// <summary>
        /// 获取Tag信息
        /// </summary>
        /// <param name="tagid"></param>
        /// <returns></returns>
        IDataReader GetTagInfo(int tagid);
        /// <summary>
        /// 根据Tag获取主题
        /// </summary>
        /// <param name="tagid">TagId</param>
        /// <param name="pageindex">页码</param>
        /// <param name="pagesize">页大小</param>
        /// <returns></returns>
        IDataReader GetTopicListByTag(int tagid, int pageindex, int pagesize);
        /// <summary>
        /// 获取相关主题
        /// </summary>
        /// <param name="topicid">主题Id</param>
        /// <param name="count">数量</param>
        /// <returns></returns>
        IDataReader GetRelatedTopics(int topicid, int count);
        /// <summary>
        /// 获取使用指定Tag的主题数
        /// </summary>
        /// <returns></returns>
        int GetTopicsCountByTag(int tagid);
        /// <summary>
        /// 设置上次任务计划的执行时间
        /// </summary>
        /// <param name="key">任务的标识</param>
        /// <param name="servername">主机名</param>
        /// <param name="lastexecuted">最后执行时间</param>
        void SetLastExecuteScheduledEventDateTime(string key, string servername, DateTime lastexecuted);
        /// <summary>
        /// 获取上次任务计划的执行时间
        /// </summary>
        /// <param name="key">任务的标识</param>
        /// <param name="servername">主机名</param>
        /// <returns></returns>
        DateTime GetLastExecuteScheduledEventDateTime(string key, string servername);
        /// <summary>
        /// 整理相关主题表
        /// </summary>
        void NeatenRelateTopics();
        /// <summary>
        /// 删除主题的相关标签
        /// </summary>
        /// <param name="topicid"></param>
        void DeleteTopicTags(int topicid);
        /// <summary>
        /// 删除主题的相关主题记录
        /// </summary>
        /// <param name="topicid"></param>
        void DeleteRelatedTopics(int topicid);
        /// <summary>
        /// 更新昨日发帖数
        /// </summary>
        void UpdateYesterdayPosts(string posttableid);
        /// <summary>
        /// 返回论坛Tag列表
        /// </summary>
        /// <param name="tagkey">查询关键字</param>
        /// <returns>返回Sql语句</returns>
        string GetForumTagsSql(string tagkey,int type);
        /// <summary>
        /// 获得一定范围内的主题数
        /// </summary>
        /// <param name="from"></param>
        /// <param name="end"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        string GetTopicNumber(string tagname,int from, int end, int type);
        /// <summary>
        /// 更新Tag
        /// </summary>
        /// <param name="tagid"></param>
        /// <param name="orderid"></param>
        /// <param name="color"></param>
        void UpdateForumTags(int tagid, int orderid, string color);
        /// <summary>
        /// 获取开放论坛的列表
        /// </summary>
        /// <returns></returns>
        DataTable GetOpenForumList();



        DataTable GetExtractTopic(string inForumList, int itemCount, string givenTids, string keyword,
            string tags, string typesList, string digestList, string displayorderList, string orderBy);


        /// <summary>
        /// 增加悬赏日志
        /// </summary>
        /// <param name="tid">主题Id</param>
        /// <param name="authorid">悬赏者Id</param>
        /// <param name="winerid">获奖者Id</param>
        /// <param name="winnerName">获奖者用户名</param>
        /// <param name="postid">帖子Id</param>
        /// <param name="bonus">奖励积分</param>
        /// <param name="extid">进行悬赏时的交易积分</param>
        /// <param name="isbest">是否是最佳答案</param>
        void AddBonusLog(int tid, int authorid, int winerid, string winnerName, int postid, int bonus, int extid, int isbest);
        /// <summary>
        /// 更新指定主题的magic值
        /// </summary>
        /// <param name="tid">主题Id</param>
        /// <param name="magic">Magic值</param>
        void UpdateMagicValue(int tid, int magic);
        /// <summary>
        /// 获取主题给分日志
        /// </summary>
        /// <param name="tid">主题Id</param>
        /// <returns></returns>
        IDataReader GetTopicBonusLogs(int tid);
        /// <summary>
        /// 更新统计表
        /// </summary>
        /// <param name="type"></param>
        /// <param name="variable"></param>
        /// <param name="count"></param>
        void UpdateStats(string type, string variable, int count);
        /// <summary>
        /// 更新统计变量
        /// </summary>
        /// <param name="type"></param>
        /// <param name="variable"></param>
        /// <param name="value"></param>
        void UpdateStatVars(string type, string variable, string value);
        /// <summary>
        /// 获得所有统计信息
        /// </summary>
        /// <returns></returns>
        IDataReader GetAllStats();
        /// <summary>
        /// 获得指定类型统计
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        IDataReader GetStatsByType(string type);
        /// <summary>
        /// 获得指定类型统计
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        IDataReader GetStatVarsByType(string type);
        /// <summary>
        /// 获得所有统计
        /// </summary>
        /// <returns></returns>
        IDataReader GetAllStatVars();
        /// <summary>
        /// 删除旧的帖数统计
        /// </summary>
        void DeleteOldDayposts();
        /// <summary>
        /// 获得首贴时间
        /// </summary>
        /// <returns></returns>
        DateTime GetPostStartTime();
        /// <summary>
        /// 统计板块数量
        /// </summary>
        /// <returns></returns>
        int GetForumCount();
        /// <summary>
        /// 获得今日发贴数
        /// </summary>
        /// <returns></returns>
        int GetTodayPostCount(string posttableid);
        /// <summary>
        /// 获得今日新用户数
        /// </summary>
        /// <returns></returns>
        int GetTodayNewMemberCount();
        /// <summary>
        /// 获得管理员数量
        /// </summary>
        /// <returns></returns>
        int GetAdminCount();
        /// <summary>
        /// 获得未发帖的会员数
        /// </summary>
        /// <returns></returns>
        int GetNonPostMemCount();
        /// <summary>
        /// 获得本日最佳会员
        /// </summary>
        /// <param name="posttableid"></param>
        /// <param name="bestmem"></param>
        /// <param name="bestmemposts"></param>
        IDataReader GetBestMember(string posttableid);
        /// <summary>
        /// 获得每月帖数统计
        /// </summary>
        /// <returns></returns>
        IDataReader GetMonthPostsStats(string posttableid);
        /// <summary>
        /// 获得30天内的每日发帖统计
        /// </summary>
        /// <returns></returns>
        IDataReader GetDayPostsStats(string posttableid);
        /// <summary>
        /// 获得热门主题
        /// </summary>
        /// <returns></returns>
        IDataReader GetHotTopics(int count);
        /// <summary>
        /// 获得热门回复主题
        /// </summary>
        /// <returns></returns>
        IDataReader GetHotReplyTopics(int count);
        /// <summary>
        /// 获得主题数板块排行榜
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        IDataReader GetForumsByTopicCount(int count);
        /// <summary>
        /// 获得发帖数板块排行榜
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        IDataReader GetForumsByPostCount(int count);
        /// <summary>
        /// 获得30天发帖数排行榜
        /// </summary>
        /// <param name="count"></param>
        /// <param name="posttableid"></param>
        /// <returns></returns>
        IDataReader GetForumsByMonthPostCount(int count, string posttableid);
        /// <summary>
        /// 获得当日发帖板块排行榜
        /// </summary>
        /// <param name="count"></param>
        /// <param name="posttableid"></param>
        /// <returns></returns>
        IDataReader GetForumsByDayPostCount(int count, string posttableid);
        /// <summary>
        /// 获得用户排行
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        IDataReader GetUsersRank(int count, string posttableid, string type);
        /// <summary>
        /// 获得用户排行
        /// </summary>
        /// <param name="filed">当月还是总在线时间</param>
        /// <returns></returns>
        IDataReader GetUserByOnlineTime(string filed);
        /// <summary>
        /// 获取主题中每条获奖帖子的得分记录
        /// </summary>
        /// <param name="tid"></param>
        /// <returns></returns>
        IDataReader GetTopicBonusLogsByPost(int tid);
        /// <summary>
        /// 获得超版权限以上的管理人员
        /// </summary>
        /// <returns></returns>
        IDataReader GetSuperModerators();
        /// <summary>
        /// 获得版主
        /// </summary>
        /// <param name="uids"></param>
        /// <returns></returns>
        IDataReader GetModeratorsDetails(string uids, int oltimespan);
        /// <summary>
        /// 更新统计数据
        /// </summary>
        /// <param name="visitorsadd"></param>
        void UpdateStatCount(string browser, string os, string visitorsadd);
        void LessenTotalUsers();

        IDataReader GetPostDebate(int tid);

        /// <summary>
        /// 增加辩论主题扩展信息
        /// </summary>
        /// <param name="debatetopic"></param>
        void AddDebateTopic(DebateInfo debatetopic);
        /// <summary>
        /// 更新主题类型标识字段
        /// </summary>
        /// <param name="tid">主题Id</param>
        /// <param name="special">主题类型</param>
        void UpdateTopicSpecial(int tid, byte special);
        /// <summary>
        /// 获取所有开放版块，即获取条件为autoclose=0 and password='' and redirect=''的版块
        /// </summary>
        /// <returns></returns>
        DataTable GetAllOpenForum();
        /// <summary>
        /// 获取辩论主题附加信息
        /// </summary>
        /// <param name="tid"></param>
        /// <returns></returns>
        IDataReader GetDebateTopic(int tid);
        /// <summary>
        /// 获取最热辩论的JSON数据
        /// </summary>
        /// <param name="hotfield"></param>
        /// <param name="defhotcount"></param>
        /// <param name="getcount"></param>
        /// <returns></returns>
        IDataReader GetHotDebatesList(string hotfield, int defhotcount, int getcount);
        /// <summary>
        /// 获取推荐辩论的JSON数据
        /// </summary>
        /// <param name="tidlist"></param>
        /// <returns></returns>
        IDataReader GetRecommendDebates(string tidlist);
        /// <summary>
        /// 创建参加辩论帖子的扩展信息
        /// </summary>
        /// <param name="dpei"></param>
        void CreateDebatePostExpand(DebatePostExpandInfo dpei);
        /// <summary>
        /// 增加点评信息
        /// </summary>
        /// <param name="tid"></param>
        /// <param name="tableid"></param>
        /// <param name="commentmsg"></param>
        void AddCommentDabetas(int tid, int tableid, string commentmsg);

        /// <summary>
        /// 增加顶
        /// </summary>
        /// <param name="tid"></param>
        /// <param name="pid"></param>
        /// <param name="field"></param>
        /// <param name="ip"></param>
        /// <param name="userinfo"></param>
        void AddDebateDigg(int tid, int pid, int field, string ip, UserInfo userinfo);
        /// <summary>
        /// 判断是否可以顶
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        bool AllowDiggs(int pid, int userid);

        /// <summary>
        /// 获取辩论中某一方的帖子列表
        /// </summary>
        /// <returns></returns>
        IDataReader GetDebatePostList(int tid, int opinion, int pageSize, int pageIndex, string postTableName, PostOrderType postOrderType);
        /// <summary>
        /// 获取指定版块下的最新回复
        /// </summary>
        /// <param name="fid">指定的版块</param>
        /// <param name="count">返回记录数</param>
        /// <param name="posttablename">当前分表名称</param>
        /// <param name="visibleForum">可见版块列表</param>
        /// <returns></returns>
        DataTable GetLastPostList(int fid, int count, string posttablename, string visibleForum);
        /// <summary>
        /// 得到用户顶过的记录
        /// </summary>
        /// <param name="tid"></param>
        /// <param name="uid"></param>
        /// <returns></returns>
        IDataReader GetUesrDiggs(int tid, int uid);
        /// <summary>
        /// 修复辩论主题的支持数
        /// </summary>
        /// <param name="tid">主题id</param>
        /// <param name="debateOpinion">辩论所持观点</param>
        int ReviseDebateTopicDiggs(int tid, int debateOpinion);
        /// <summary>
        /// 获取辩论帖的支持数
        /// </summary>
        /// <param name="pidlist"></param>
        /// <returns></returns>
        IDataReader GetDebatePostDiggs(string pidlist);
        /// <summary>
        /// 获取指定版块的最新回复主题id
        /// </summary>
        /// <param name="foruminfo">指定的版块信息</param>
        /// <param name="visibleForum">可见版块列表</param>
        /// <returns></returns>
        int GetLastPostTid(ForumInfo foruminfo, string visibleForum);
        /// <summary>
        /// 更新指定版块的最新发帖信息
        /// </summary>
        /// <param name="foruminfo">当前版块信息</param>
        /// <param name="postinfo">要更新的帖子信息</param>
        /// <returns></returns>
        void UpdateLastPost(ForumInfo foruminfo, PostInfo postinfo);
        void SetPostsBanned(int tableid, string postlistid, int invisible);
        /// <summary>
        /// 更新在线时间统计
        /// </summary>
        /// <param name="oltimespan">时间增加量</param>
        /// <param name="uid">用户id</param>
        void UpdateOnlineTime(int oltimespan, int uid);
        /// <summary>
        /// 重置每月在线时间(清零)
        /// </summary>
        void ResetThismonthOnlineTime();
        /// <summary>
        /// 同步users表oltime
        /// </summary>
        /// <param name="uid"></param>
        void SynchronizeOltime(int uid);
        void SetTopicsBump(string tidlist, int type);
        int GetPostId();
        /// <summary>
        /// 重载GETPOSTLIST
        /// </summary>
        /// <param name="postlist"></param>
        /// <param name="tableid"></param>
        /// <returns></returns>
        DataTable GetPostList(string postlist, int tableid);
        /// <summary>
        /// 获取指定的主题过滤的条件
        /// </summary>
        /// <param name="filter">过滤类型</param>
        /// <returns></returns>
        string GetTopicFilterCondition(string filter);
        /// <summary>
        /// 得到模板信息
        /// </summary>
        /// <returns></returns>
        string GetTemplateInfo();
        /// <summary>
        /// 获取用户组中设置的图片空间最大尺寸
        /// </summary>
        /// <returns></returns>
        DataTable GetUserGroupMaxspacephotosize();
        /// <summary>
        /// 获取用户组中设置的空间附件最大尺寸
        /// </summary>
        /// <returns></returns>
        DataTable GetUserGroupMaxspaceattachsize();

        /// <summary>
        /// 获得要过滤或转换的词条
        /// </summary>
        /// <returns></returns>
        DataTable GetBadWords();
        /// <summary>
        /// 清除用户的SpaceId
        /// </summary>
        /// <param name="uid">要清除的用户</param>
        void ClearUserSpace(int uid);
        int GetDebatesPostCount(int tid, int debateOpinion);
        void DeleteDebatePost(int tid, string opinion, int pid);
        IDataReader GetSinglePost(int tid, int posttableid);
        /// <summary>
        /// 返回所有开放版块列表Sql
        /// </summary>
        /// <returns>开放版块列表Sql</returns>
        string GetOpenForumListSql();

        /// <summary>
        /// 修改配置文件表(dnt_Configs)中的内容
        /// </summary>
        /// <param name="content"></param>
        void ModifyConfigs(string content);

        /// <summary>
        /// 获取cache.config中的cfgGeneral
        /// </summary>
        /// <returns></returns>
        string GetDntConfigs();

        /// <summary>
        /// 会员从社区登录时，往T_System_Login表中添加记录
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="userid"></param>
        /// <param name="ipAddress"></param>
        /// <param name="description"></param>
        void ClubLoginLog(int siteId,int userid,string ipAddress,int description);

        /// <summary>
        /// 获取换编辑器之前的最后一个帖子ID
        /// </summary>
        /// <param name="dateTime">升级前时间</param>
        /// <param name="tableid">帖子附表ID</param>
        /// <returns></returns>
        int GetLastPostID(DateTime dateTime, string tableid);
    }
}
