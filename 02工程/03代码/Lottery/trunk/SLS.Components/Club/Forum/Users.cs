using System;
using System.Data;
using System.Data.Common;
using System.Text;

using Discuz.Common;
using Discuz.Data;
using Discuz.Config;
using Discuz.Entity;
using System.Collections;
using Discuz.Cache;
using System.Web;
using System.Security.Cryptography;

namespace Discuz.Forum
{
    /// <summary>
    /// 用户操作类
    /// </summary>
    public class Users
    {

        /// <summary>
        /// 返回指定用户的信息
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <returns>用户信息</returns>
        public static IDataReader GetUserInfoToReader(int uid)
        {
            return DatabaseProvider.GetInstance().GetUserInfoToReader(uid);
        }

        /// <summary>
        /// 获取简短用户信息
        /// </summary>
        /// <param name="uid">用id</param>
        /// <returns>用户简短信息</returns>
        public static IDataReader GetShortUserInfoToReader(int uid)
        {
            return DatabaseProvider.GetInstance().GetShortUserInfoToReader(uid);
        }


        /// <summary>
        /// 返回指定用户的完整信息
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <returns>用户信息</returns>
        public static UserInfo GetUserInfo(int uid)
        {
            UserInfo userinfo = new UserInfo();

            IDataReader reader = DatabaseProvider.GetInstance().GetUserInfoToReader(uid);

            if (reader.Read())
            {
                userinfo.Uid = Int32.Parse(reader["uid"].ToString());
                userinfo.Username = reader["username"].ToString();
                userinfo.Nickname = reader["nickname"].ToString();
                userinfo.Password = reader["password"].ToString();
                userinfo.Spaceid = Int32.Parse(reader["spaceid"].ToString());
                userinfo.Secques = reader["secques"].ToString();
                userinfo.Gender = Int32.Parse(reader["gender"].ToString());
                userinfo.Adminid = Int32.Parse(reader["adminid"].ToString() == "" ? "0" : reader["adminid"].ToString());
                userinfo.Groupid = Int16.Parse(reader["groupid"].ToString() == "" ? "10" : reader["groupid"].ToString());
                userinfo.Groupexpiry = Int32.Parse(reader["groupexpiry"].ToString());
                userinfo.Extgroupids = reader["extgroupids"].ToString();
                userinfo.Regip = reader["regip"].ToString();
                userinfo.Joindate = Utils.GetStandardDateTime(reader["joindate"].ToString());
                userinfo.Lastip = reader["lastip"].ToString();
                userinfo.Lastvisit = Utils.GetStandardDateTime(reader["lastvisit"].ToString());
                userinfo.Lastactivity = Utils.GetStandardDateTime(reader["lastactivity"].ToString());
                userinfo.Lastpost = Utils.GetStandardDateTime(reader["lastpost"].ToString());
                userinfo.Lastpostid = Int32.Parse(reader["lastpostid"].ToString() == "" ? "-1" : reader["lastpostid"].ToString());
                userinfo.Lastposttitle = reader["lastposttitle"].ToString();
                userinfo.Posts = Int32.Parse(reader["posts"].ToString() == "" ? "0" : reader["posts"].ToString());
                userinfo.Digestposts = Int16.Parse(reader["digestposts"].ToString() == "" ? "0" : reader["digestposts"].ToString());
                userinfo.Oltime = Int32.Parse(reader["oltime"].ToString() == "" ? "0" : reader["oltime"].ToString());
                userinfo.Pageviews = Int32.Parse(reader["pageviews"].ToString() == "" ? "0" : reader["pageviews"].ToString());
                userinfo.Credits = Int32.Parse(reader["credits"].ToString() == "" ? "0" : reader["credits"].ToString());
                userinfo.Extcredits1 = float.Parse(reader["extcredits1"].ToString());
                userinfo.Extcredits2 = float.Parse(reader["extcredits2"].ToString());
                userinfo.Extcredits3 = float.Parse(reader["extcredits3"].ToString());
                userinfo.Extcredits4 = float.Parse(reader["extcredits4"].ToString());
                userinfo.Extcredits5 = float.Parse(reader["extcredits5"].ToString());
                userinfo.Extcredits6 = float.Parse(reader["extcredits6"].ToString());
                userinfo.Extcredits7 = float.Parse(reader["extcredits7"].ToString());
                userinfo.Extcredits8 = float.Parse(reader["extcredits8"].ToString());
                userinfo.Avatarshowid = Int32.Parse(reader["avatarshowid"].ToString() == "" ? "0" : reader["avatarshowid"].ToString());
                userinfo.Medals = reader["medals"].ToString();
                userinfo.Email = reader["email"].ToString();
                userinfo.Bday = reader["bday"].ToString();
                userinfo.Sigstatus = Int32.Parse(reader["sigstatus"].ToString());
                userinfo.Tpp = Int32.Parse(reader["tpp"].ToString());
                userinfo.Ppp = Int32.Parse(reader["ppp"].ToString());
                userinfo.Templateid = Int16.Parse(reader["templateid"].ToString() == "" ? "1" : reader["templateid"].ToString());
                userinfo.Pmsound = Int32.Parse(reader["pmsound"].ToString());
                userinfo.Showemail = Int32.Parse(reader["showemail"].ToString());
                userinfo.Newsletter = (ReceivePMSettingType)Int32.Parse(reader["newsletter"].ToString());
                userinfo.Invisible = Int32.Parse(reader["invisible"].ToString());
                //__userinfo.Timeoffset = reader["timeoffset"].ToString();
                userinfo.Newpm = Int32.Parse(reader["newpm"].ToString());
                userinfo.Newpmcount = Int32.Parse(reader["newpmcount"].ToString());
                userinfo.Accessmasks = Int32.Parse(reader["accessmasks"].ToString());
                userinfo.Onlinestate = Int32.Parse(reader["onlinestate"].ToString());
                //
                userinfo.Website = reader["website"].ToString();
                userinfo.Icq = reader["icq"].ToString();
                userinfo.Qq = reader["qq"].ToString();
                userinfo.Yahoo = reader["yahoo"].ToString();
                userinfo.Msn = reader["msn"].ToString();
                userinfo.Skype = reader["skype"].ToString();
                userinfo.Location = reader["location"].ToString();
                userinfo.Customstatus = reader["customstatus"].ToString();
                userinfo.Avatar = reader["avatar"].ToString();
                userinfo.Avatarwidth = Int32.Parse(reader["avatarwidth"].ToString() == "" ? "50" : reader["avatarwidth"].ToString());
                userinfo.Avatarheight = Int32.Parse(reader["avatarheight"].ToString() == "" ? "50" : reader["avatarheight"].ToString());
                userinfo.Bio = reader["bio"].ToString();
                userinfo.Signature = reader["signature"].ToString();
                userinfo.Sightml = reader["sightml"].ToString();
                userinfo.Authstr = reader["authstr"].ToString();
                userinfo.Authtime = reader["authtime"].ToString();
                userinfo.Authflag = Shove._Convert.StrToShort(reader["authflag"].ToString(), 0);
                userinfo.Realname = reader["realname"].ToString();
                userinfo.Idcard = reader["idcard"].ToString();
                userinfo.Mobile = reader["mobile"].ToString();
                userinfo.Phone = reader["phone"].ToString();

                reader.Close();
                return userinfo;
            }
            reader.Close();
            return null;
        }

        /// <summary>
        /// 返回指定用户的简短信息
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <returns>用户信息</returns>
        public static ShortUserInfo GetShortUserInfo(int uid)
        {
            ShortUserInfo userinfo = new ShortUserInfo();

            IDataReader reader = DatabaseProvider.GetInstance().GetShortUserInfoToReader(uid);

            if (reader.Read())
            {
                userinfo.Uid = Int32.Parse(reader["uid"].ToString());
                userinfo.Username = reader["username"].ToString();
                userinfo.Nickname = reader["nickname"].ToString();
                userinfo.Password = reader["password"].ToString();
                userinfo.Spaceid = Int32.Parse(reader["spaceid"].ToString());
                userinfo.Secques = reader["secques"].ToString();
                userinfo.Gender = Int32.Parse(reader["gender"].ToString());
                userinfo.Adminid = Int32.Parse(reader["adminid"].ToString());
                userinfo.Groupid = Int16.Parse(reader["groupid"].ToString());
                userinfo.Groupexpiry = Int32.Parse(reader["groupexpiry"].ToString());
                userinfo.Extgroupids = reader["extgroupids"].ToString();
                userinfo.Regip = reader["regip"].ToString();
                userinfo.Joindate = reader["joindate"].ToString();
                userinfo.Lastip = reader["lastip"].ToString();
                userinfo.Lastvisit = reader["lastvisit"].ToString();
                userinfo.Lastactivity = reader["lastactivity"].ToString();
                userinfo.Lastpost = reader["lastpost"].ToString();
                userinfo.Lastpostid = Int32.Parse(reader["lastpostid"].ToString());
                userinfo.Lastposttitle = reader["lastposttitle"].ToString();
                userinfo.Posts = Int32.Parse(reader["posts"].ToString());
                userinfo.Digestposts = Int16.Parse(reader["digestposts"].ToString());
                userinfo.Oltime = Int32.Parse(reader["oltime"].ToString());
                userinfo.Pageviews = Int32.Parse(reader["pageviews"].ToString());
                userinfo.Credits = Int32.Parse(reader["credits"].ToString());
                userinfo.Extcredits1 = float.Parse(reader["extcredits1"].ToString());
                userinfo.Extcredits2 = float.Parse(reader["extcredits2"].ToString());
                userinfo.Extcredits3 = float.Parse(reader["extcredits3"].ToString());
                userinfo.Extcredits4 = float.Parse(reader["extcredits4"].ToString());
                userinfo.Extcredits5 = float.Parse(reader["extcredits5"].ToString());
                userinfo.Extcredits6 = float.Parse(reader["extcredits6"].ToString());
                userinfo.Extcredits7 = float.Parse(reader["extcredits7"].ToString());
                userinfo.Extcredits8 = float.Parse(reader["extcredits8"].ToString());
                userinfo.Avatarshowid = Int32.Parse(reader["avatarshowid"].ToString());
                userinfo.Email = reader["email"].ToString();
                userinfo.Bday = reader["bday"].ToString();
                userinfo.Sigstatus = Int32.Parse(reader["sigstatus"].ToString());
                userinfo.Tpp = Int32.Parse(reader["tpp"].ToString());
                userinfo.Ppp = Int32.Parse(reader["ppp"].ToString());
                userinfo.Templateid = Int16.Parse(reader["templateid"].ToString());
                userinfo.Pmsound = Int32.Parse(reader["pmsound"].ToString());
                userinfo.Showemail = Int32.Parse(reader["showemail"].ToString());
                userinfo.Newsletter = (ReceivePMSettingType)Int32.Parse(reader["newsletter"].ToString());
                userinfo.Invisible = Int32.Parse(reader["invisible"].ToString());
                //__userinfo.Timeoffset = reader["timeoffset"].ToString();
                userinfo.Newpm = Int32.Parse(reader["newpm"].ToString());
                userinfo.Newpmcount = Int32.Parse(reader["newpmcount"].ToString());
                userinfo.Accessmasks = Int32.Parse(reader["accessmasks"].ToString());
                userinfo.Onlinestate = Int32.Parse(reader["onlinestate"].ToString());

                reader.Close();
                return userinfo;
            }
            reader.Close();
            return null;
        }

        /// <summary>
        /// 根据IP查找用户
        /// </summary>
        /// <param name="ip">ip地址</param>
        /// <returns>用户信息</returns>
        public static UserInfo GetUserInfoByIP(string ip)
        {
            UserInfo userinfo = new UserInfo();

            IDataReader reader = DatabaseProvider.GetInstance().GetUserInfoByIP(ip);
            if (reader.Read())
            {
                userinfo.Uid = Int32.Parse(reader["uid"].ToString());
                userinfo.Username = reader["username"].ToString();
                userinfo.Nickname = reader["nickname"].ToString();
                userinfo.Password = reader["password"].ToString();
                userinfo.Secques = reader["secques"].ToString();
                userinfo.Gender = Int32.Parse(reader["gender"].ToString());
                userinfo.Adminid = Int32.Parse(reader["adminid"].ToString());
                userinfo.Groupid = Int16.Parse(reader["groupid"].ToString());
                userinfo.Groupexpiry = Int32.Parse(reader["groupexpiry"].ToString());
                userinfo.Extgroupids = reader["extgroupids"].ToString();
                userinfo.Regip = reader["regip"].ToString();
                userinfo.Joindate = reader["joindate"].ToString();
                userinfo.Lastip = reader["lastip"].ToString();
                userinfo.Lastvisit = reader["lastvisit"].ToString();
                userinfo.Lastactivity = reader["lastactivity"].ToString();
                userinfo.Lastpost = reader["lastpost"].ToString();
                userinfo.Lastpostid = Int32.Parse(reader["lastpostid"].ToString());
                userinfo.Lastposttitle = reader["lastposttitle"].ToString();
                userinfo.Posts = Int32.Parse(reader["posts"].ToString());
                userinfo.Digestposts = Int16.Parse(reader["digestposts"].ToString());
                userinfo.Oltime = Int32.Parse(reader["oltime"].ToString());
                userinfo.Pageviews = Int32.Parse(reader["pageviews"].ToString());
                userinfo.Credits = Int32.Parse(reader["credits"].ToString());
                userinfo.Extcredits1 = float.Parse(reader["extcredits1"].ToString());
                userinfo.Extcredits2 = float.Parse(reader["extcredits2"].ToString());
                userinfo.Extcredits3 = float.Parse(reader["extcredits3"].ToString());
                userinfo.Extcredits4 = float.Parse(reader["extcredits4"].ToString());
                userinfo.Extcredits5 = float.Parse(reader["extcredits5"].ToString());
                userinfo.Extcredits6 = float.Parse(reader["extcredits6"].ToString());
                userinfo.Extcredits7 = float.Parse(reader["extcredits7"].ToString());
                userinfo.Extcredits8 = float.Parse(reader["extcredits8"].ToString());
                userinfo.Avatarshowid = Int32.Parse(reader["avatarshowid"].ToString());
                userinfo.Medals = reader["medals"].ToString();
                userinfo.Email = reader["email"].ToString();
                userinfo.Bday = reader["bday"].ToString();
                userinfo.Sigstatus = Int32.Parse(reader["sigstatus"].ToString());
                userinfo.Tpp = Int32.Parse(reader["tpp"].ToString());
                userinfo.Ppp = Int32.Parse(reader["ppp"].ToString());
                userinfo.Templateid = Int16.Parse(reader["templateid"].ToString());
                userinfo.Pmsound = Int32.Parse(reader["pmsound"].ToString());
                userinfo.Showemail = Int32.Parse(reader["showemail"].ToString());
                userinfo.Newsletter = (ReceivePMSettingType)Int32.Parse(reader["newsletter"].ToString());
                userinfo.Invisible = Int32.Parse(reader["invisible"].ToString());
                //__userinfo.Timeoffset = reader["timeoffset"].ToString();
                userinfo.Newpm = Int32.Parse(reader["newpm"].ToString());
                userinfo.Newpmcount = Int32.Parse(reader["newpmcount"].ToString());
                userinfo.Accessmasks = Int32.Parse(reader["accessmasks"].ToString());
                userinfo.Onlinestate = Int32.Parse(reader["onlinestate"].ToString());
                //
                userinfo.Website = reader["website"].ToString();
                userinfo.Icq = reader["icq"].ToString();
                userinfo.Qq = reader["qq"].ToString();
                userinfo.Yahoo = reader["yahoo"].ToString();
                userinfo.Msn = reader["msn"].ToString();
                userinfo.Skype = reader["skype"].ToString();
                userinfo.Location = reader["location"].ToString();
                userinfo.Customstatus = reader["customstatus"].ToString();
                userinfo.Avatar = reader["avatar"].ToString();
                userinfo.Avatarwidth = Int32.Parse(reader["avatarwidth"].ToString());
                userinfo.Avatarheight = Int32.Parse(reader["avatarheight"].ToString());
                userinfo.Bio = reader["bio"].ToString();
                userinfo.Signature = reader["signature"].ToString();
                userinfo.Sightml = reader["sightml"].ToString();
                userinfo.Authstr = reader["authstr"].ToString();
                reader.Close();
                return userinfo;
            }
            reader.Close();
            return null;
        }

        /// <summary>
        /// 根据IP查找用户
        /// </summary>
        /// <param name="ip">ip地址</param>
        /// <returns>用户信息</returns>
        public static ShortUserInfo GetShortUserInfoByIP(string ip)
        {
            ShortUserInfo __userinfo = new ShortUserInfo();

            IDataReader reader = DatabaseProvider.GetInstance().GetUserInfoByIP(ip);

            if (reader.Read())
            {
                __userinfo.Uid = Int32.Parse(reader["uid"].ToString());
                __userinfo.Username = reader["username"].ToString();
                __userinfo.Nickname = reader["nickname"].ToString();
                __userinfo.Password = reader["password"].ToString();
                __userinfo.Secques = reader["secques"].ToString();
                __userinfo.Gender = Int32.Parse(reader["gender"].ToString());
                __userinfo.Adminid = Int32.Parse(reader["adminid"].ToString());
                __userinfo.Groupid = Int16.Parse(reader["groupid"].ToString());
                __userinfo.Groupexpiry = Int32.Parse(reader["groupexpiry"].ToString());
                __userinfo.Extgroupids = reader["extgroupids"].ToString();
                __userinfo.Regip = reader["regip"].ToString();
                __userinfo.Joindate = reader["joindate"].ToString();
                __userinfo.Lastip = reader["lastip"].ToString();
                __userinfo.Lastvisit = reader["lastvisit"].ToString();
                __userinfo.Lastactivity = reader["lastactivity"].ToString();
                __userinfo.Lastpost = reader["lastpost"].ToString();
                __userinfo.Lastpostid = Int32.Parse(reader["lastpostid"].ToString());
                __userinfo.Lastposttitle = reader["lastposttitle"].ToString();
                __userinfo.Posts = Int32.Parse(reader["posts"].ToString());
                __userinfo.Digestposts = Int16.Parse(reader["digestposts"].ToString());
                __userinfo.Oltime = Int32.Parse(reader["oltime"].ToString());
                __userinfo.Pageviews = Int32.Parse(reader["pageviews"].ToString());
                __userinfo.Credits = Int32.Parse(reader["credits"].ToString());
                __userinfo.Extcredits1 = float.Parse(reader["extcredits1"].ToString());
                __userinfo.Extcredits2 = float.Parse(reader["extcredits2"].ToString());
                __userinfo.Extcredits3 = float.Parse(reader["extcredits3"].ToString());
                __userinfo.Extcredits4 = float.Parse(reader["extcredits4"].ToString());
                __userinfo.Extcredits5 = float.Parse(reader["extcredits5"].ToString());
                __userinfo.Extcredits6 = float.Parse(reader["extcredits6"].ToString());
                __userinfo.Extcredits7 = float.Parse(reader["extcredits7"].ToString());
                __userinfo.Extcredits8 = float.Parse(reader["extcredits8"].ToString());
                __userinfo.Avatarshowid = Int32.Parse(reader["avatarshowid"].ToString());
                __userinfo.Email = reader["email"].ToString();
                __userinfo.Bday = reader["bday"].ToString();
                __userinfo.Sigstatus = Int32.Parse(reader["sigstatus"].ToString());
                __userinfo.Tpp = Int32.Parse(reader["tpp"].ToString());
                __userinfo.Ppp = Int32.Parse(reader["ppp"].ToString());
                __userinfo.Templateid = Int16.Parse(reader["templateid"].ToString());
                __userinfo.Pmsound = Int32.Parse(reader["pmsound"].ToString());
                __userinfo.Showemail = Int32.Parse(reader["showemail"].ToString());
                __userinfo.Newsletter = (ReceivePMSettingType)Int32.Parse(reader["newsletter"].ToString());
                __userinfo.Invisible = Int32.Parse(reader["invisible"].ToString());
                //__userinfo.Timeoffset = reader["timeoffset"].ToString();
                __userinfo.Newpm = Int32.Parse(reader["newpm"].ToString());
                __userinfo.Newpmcount = Int32.Parse(reader["newpmcount"].ToString());
                __userinfo.Accessmasks = Int32.Parse(reader["accessmasks"].ToString());
                __userinfo.Onlinestate = Int32.Parse(reader["onlinestate"].ToString());
                //

                reader.Close();
                return __userinfo;
            }
            reader.Close();
            return null;
        }

        /// <summary>
        /// 根据用户id返回用户名
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <returns>用户名</returns>
        public static string GetUserName(int uid)
        {
            IDataReader reader = DatabaseProvider.GetInstance().GetUserName(uid);
            string username = "";
            if (reader.Read())
            {
                username = reader[0].ToString();
            }
            reader.Close();
            return username;
        }

        /// <summary>
        /// 根据用户id返回用户注册日期
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <returns>用户注册日期</returns>
        public static string GetUserJoinDate(int uid)
        {
            IDataReader reader = DatabaseProvider.GetInstance().GetUserJoinDate(uid);
            string userjoindate = "";
            if (reader.Read())
            {
                userjoindate = reader[0].ToString();
            }
            reader.Close();
            return userjoindate;
        }

        /// <summary>
        /// 根据用户名返回用户id
        /// </summary>
        /// <param name="username">用户名</param>
        /// <returns>用户id</returns>
        public static int GetUserID(string username)
        {
            IDataReader reader = DatabaseProvider.GetInstance().GetUserID(username);
            int userid = -1;
            if (reader.Read())
            {
                userid = Int32.Parse(reader[0].ToString());
            }
            reader.Close();
            return userid;
        }

        /// <summary>
        /// 获得用户列表DataTable
        /// </summary>
        /// <param name="pagesize">每页记录数</param>
        /// <param name="pageindex">当前页数</param>
        /// <returns>用户列表DataTable</returns>
        public static DataTable GetUserList(int pagesize, int pageindex, string orderby, string ordertype)
        {
            DataTable dt = DatabaseProvider.GetInstance().GetUserList(pagesize, pageindex, orderby, ordertype);
            dt.Columns.Add("grouptitle");
            dt.Columns.Add("olimg");
            foreach (DataRow dataRow in dt.Rows)
            {
                UserGroupInfo group = UserGroups.GetUserGroupInfo(Utils.StrToInt(dataRow["groupid"], 0));
                if (group.Color == string.Empty)
                {
                    dataRow["grouptitle"] = group.Grouptitle;
                }
                else
                {
                    dataRow["grouptitle"] = string.Format("<font color='{1}'>{0}</font>", group.Grouptitle, group.Color);
                }
                dataRow["olimg"] = OnlineUsers.GetGroupImg(Utils.StrToInt(dataRow["groupid"], 0));
            }
            return dt;
        }


        /// <summary>
        /// 判断指定用户名是否已存在
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <returns>如果已存在该用户id则返回true, 否则返回false</returns>
        public static bool Exists(int uid)
        {
            return DatabaseProvider.GetInstance().Exists(uid);
        }

        /// <summary>
        /// 判断指定用户名是否已存在.
        /// </summary>
        /// <param name="username">用户名</param>
        /// <returns>如果已存在该用户名则返回true, 否则返回false</returns>
        public static bool Exists(string username)
        {
            return DatabaseProvider.GetInstance().Exists(username);
        }


        /// <summary>
        /// 是否有指定ip地址的用户注册
        /// </summary>
        /// <param name="ip">ip地址</param>
        /// <returns>存在返回true,否则返回false</returns>
        public static bool ExistsByIP(string ip)
        {
            return DatabaseProvider.GetInstance().ExistsByIP(ip);
        }

        /// <summary>
        /// 检测Email和安全项
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="email">email</param>
        /// <param name="questionid">问题id</param>
        /// <param name="answer">答案</param>
        /// <returns>如果正确则返回用户id, 否则返回-1</returns>
        public static int CheckEmailAndSecques(string username, string email, int questionid, string answer)
        {
            IDataReader reader = DatabaseProvider.GetInstance().CheckEmailAndSecques(username, email, ForumUtils.GetUserSecques(questionid, answer));
            int userid = -1;
            if (reader.Read())
            {
                userid = Int32.Parse(reader[0].ToString());
            }
            reader.Close();
            return userid;
        }


        /// <summary>
        /// 检测密码和安全项
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="originalpassword">是否非MD5密码</param>
        /// <param name="questionid">问题id</param>
        /// <param name="answer">答案</param>
        /// <returns>如果正确则返回用户id, 否则返回-1</returns>
        public static int CheckPasswordAndSecques(string username, string password, bool originalpassword, int questionid, string answer)
        {
            IDataReader reader = DatabaseProvider.GetInstance().CheckPasswordAndSecques(username, password, originalpassword, ForumUtils.GetUserSecques(questionid, answer));
            int userid = -1;
            if (reader.Read())
            {
                userid = Int32.Parse(reader[0].ToString());
            }
            reader.Close();
            return userid;
        }

        /// <summary>
        /// 检查密码
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="originalpassword">是否非MD5密码</param>
        /// <returns>如果正确则返回用户id, 否则返回-1</returns>
        public static int CheckPassword(string username, string password, bool originalpassword)
        {
            IDataReader reader = DatabaseProvider.GetInstance().CheckPassword(username, password, originalpassword);
            int userid = -1;
            if (reader.Read())
            {
                userid = Int32.Parse(reader[0].ToString());
            }
            reader.Close();
            return userid;
        }



        /// <summary>
        /// 检测DVBBS兼容模式的密码
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="questionid">问题id</param>
        /// <param name="answer">答案</param>
        /// <returns>如果正确则返回用户id, 否则返回-1</returns>
        public static int CheckDvBbsPasswordAndSecques(string username, string password, int questionid, string answer)
        {

            IDataReader reader = DatabaseProvider.GetInstance().CheckDvBbsPasswordAndSecques(username, password);
            int uid = -1;
            if (reader.Read())
            {
                if (reader["secques"].ToString().Trim() != ForumUtils.GetUserSecques(questionid, answer))
                {
                    return -1;
                }
                string pw = reader["password"].ToString().Trim();

                if (pw.Length > 16)
                {
                    if (Utils.MD5(password) == pw)
                    {
                        uid = Utils.StrToInt(reader["uid"].ToString(), -1);
                    }
                }
                else
                {
                    if (Utils.MD5(password).Substring(8, 16) == pw)
                    {
                        uid = Utils.StrToInt(reader["uid"].ToString(), -1);
                        UpdateUserPassword(uid, password);
                    }
                }
            }
            reader.Close();
            return uid;
        }

        /// <summary>
        /// 检测DVBBS兼容模式的密码
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <returns>如果正确则返回用户id, 否则返回-1</returns>
        public static int CheckDvBbsPassword(string username, string password)
        {

            IDataReader reader = DatabaseProvider.GetInstance().CheckDvBbsPasswordAndSecques(username, password);
            int uid = -1;
            if (reader.Read())
            {
                string pw = reader["password"].ToString().Trim();

                if (pw.Length > 16)
                {
                    if (Utils.MD5(password) == pw)
                    {
                        uid = Utils.StrToInt(reader["uid"].ToString(), -1);
                    }
                }
                else
                {
                    if (Utils.MD5(password).Substring(8, 16) == pw)
                    {
                        uid = Utils.StrToInt(reader["uid"].ToString(), -1);
                        UpdateUserPassword(uid, password);
                    }
                }
            }
            reader.Close();
            return uid;
        }


        /// <summary>
        /// 判断用户密码是否正确
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="originalpassword">是否为未MD5密码</param>
        /// <param name="groupid">用户组ID</param>
        /// <param name="adminid">管理组ID</param>
        /// <returns>如果正确则返回uid</returns>
        public static int CheckPassword(string username, string password, bool originalpassword, out int groupid, out int adminid)
        {
            IDataReader reader = DatabaseProvider.GetInstance().CheckPassword(username, password, originalpassword);

            int uid = -1;
            groupid = 7;
            adminid = 0;

            if (reader.Read())
            {
                uid = Utils.StrToInt(reader[0].ToString(), -1);
                groupid = Utils.StrToInt(reader[1].ToString(), -1);
                adminid = Utils.StrToInt(reader[2].ToString(), -1);

            }
            reader.Close();
            return uid;
        }

        /// <summary>
        /// 检测密码
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="password">密码</param>
        /// <param name="originalpassword">是否非MD5密码</param>
        /// <param name="groupid">用户组id</param>
        /// <param name="adminid">管理id</param>
        /// <returns>如果用户密码正确则返回uid, 否则返回-1</returns>
        public static int CheckPassword(int uid, string password, bool originalpassword, out int groupid, out int adminid)
        {
            IDataReader reader = DatabaseProvider.GetInstance().CheckPassword(uid, password, originalpassword);

            uid = -1;
            groupid = 7;
            adminid = 0;

            if (reader.Read())
            {
                uid = Utils.StrToInt(reader[0].ToString(), -1);
                groupid = Utils.StrToInt(reader[1].ToString(), -1);
                adminid = Utils.StrToInt(reader[2].ToString(), -1);

            }
            reader.Close();
            return uid;
        }


        /// <summary>
        /// 判断指定用户密码是否正确.
        /// </summary>
        /// <param name="uid">用户ID</param>
        /// <param name="password">用户密码</param>
        /// <returns>如果用户密码正确则返回true, 否则返回false</returns>
        public static int CheckPassword(int uid, string password, bool originalpassword)
        {
            IDataReader reader = DatabaseProvider.GetInstance().CheckPassword(uid, password, originalpassword);

            uid = -1;
            if (reader.Read())
            {
                uid = Utils.StrToInt(reader[0].ToString(), -1);
            }
            reader.Close();
            return uid;
        }

        /// <summary>
        /// 根据指定的email查找用户并返回用户uid
        /// </summary>
        /// <param name="email">email地址</param>
        /// <returns>用户uid</returns>
        public static int FindUserEmail(string email)
        {
            IDataReader reader = DatabaseProvider.GetInstance().FindUserEmail(email);

            int uid = -1;
            if (reader.Read())
            {
                uid = Utils.StrToInt(reader[0].ToString(), -1);
            }
            reader.Close();
            return uid;
        }


        /// <summary>
        /// 得到论坛中用户总数
        /// </summary>
        /// <returns>用户总数</returns>
        public static int GetUserCount()
        {
            return DatabaseProvider.GetInstance().GetUserCount();
        }

        /// <summary>
        /// 得到论坛中用户总数
        /// </summary>
        /// <returns>用户总数</returns>
        public static int GetUserCountByAdmin()
        {
            return DatabaseProvider.GetInstance().GetUserCountByAdmin();
        }

        /// <summary>
        /// 创建新用户.
        /// </summary>
        /// <param name="__userinfo">用户信息</param>
        /// <returns>返回用户ID, 如果已存在该用户名则返回-1</returns>
        public static int CreateUser(UserInfo __userinfo)
        {
            if (Exists(__userinfo.Username))
            {
                return -1;
            }

            return DatabaseProvider.GetInstance().CreateUser(__userinfo);
        }

        /// <summary>
        /// 更新权限验证字符串
        /// </summary>
        /// <param name="uid">用户id</param>
        public static void UpdateAuthStr(int uid)
        {

            string authstr = ForumUtils.CreateAuthStr(20);

            UpdateAuthStr(uid, authstr, 1);
        }

        /// <summary>
        /// 更新权限验证字符串
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="authstr">验证串</param>
        /// <param name="authflag">验证标志</param>
        public static void UpdateAuthStr(int uid, string authstr, int authflag)
        {
            DatabaseProvider.GetInstance().UpdateAuthStr(uid, authstr, authflag);
        }


        /// <summary>
        /// 更新指定用户的个人资料
        /// </summary>
        /// <param name="__userinfo">用户信息</param>
        /// <returns>如果用户不存在则为false, 否则为true</returns>
        public static bool UpdateUserProfile(UserInfo __userinfo)
        {
            if (!Exists(__userinfo.Uid))
            {
                return false;
            }

            DatabaseProvider.GetInstance().UpdateUserProfile(__userinfo);
            return true;
        }

        /// <summary>
        /// 更新用户论坛设置
        /// </summary>
        /// <param name="__userinfo">用户信息</param>
        /// <returns>如果用户不存在则返回false, 否则返回true</returns>
        public static bool UpdateUserForumSetting(UserInfo __userinfo)
        {
            if (!Exists(__userinfo.Uid))
            {
                return false;
            }

            DatabaseProvider.GetInstance().UpdateUserForumSetting(__userinfo);
            return true;
        }

        /// <summary>
        /// 修改用户自定义积分字段的值
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="extid">扩展字段序号(1-8)</param>
        /// <param name="pos">增加的数值(可以是负数)</param>
        /// <returns>执行是否成功</returns>
        public static void UpdateUserExtCredits(int uid, int extid, float pos)
        {
            DatabaseProvider.GetInstance().UpdateUserExtCredits(uid, extid, pos);
        }

        /// <summary>
        /// 获得指定用户的指定积分扩展字段的值
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="extid">扩展字段序号(1-8)</param>
        /// <returns>值</returns>
        public static float GetUserExtCredits(int uid, int extid)
        {
            return DatabaseProvider.GetInstance().GetUserExtCredits(uid, extid);
        }

        /// <summary>
        /// 更新用户头像
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="avatar">头像</param>
        /// <param name="avatarwidth">头像宽度</param>
        /// <param name="avatarheight">头像高度</param>
        /// <param name="templateid">模板Id</param>
        /// <returns>如果用户不存在则返回false, 否则返回true</returns>
        public static bool UpdateUserPreference(int uid, string avatar, int avatarwidth, int avatarheight, int templateid)
        {
            if (!Exists(uid))
            {
                return false;
            }

            DatabaseProvider.GetInstance().UpdateUserPreference(uid, avatar, avatarwidth, avatarheight, templateid);
            return true;
        }

        /// <summary>
        /// 更新用户密码
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="password">用户密码</param>
        /// <returns>如果用户不存在则返回false, 否则返回true</returns>
        public static bool UpdateUserPassword(int uid, string password)
        {
            return UpdateUserPassword(uid, password, true);
        }

        /// <summary>
        /// 更新用户密码
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="password">密码</param>
        /// <param name="originalpassword">是否非MD5密码</param>
        /// <returns>成功返回true否则false</returns>
        public static bool UpdateUserPassword(int uid, string password, bool originalpassword)
        {
            if (!Exists(uid))
            {
                return false;
            }

            DatabaseProvider.GetInstance().UpdateUserPassword(uid, password, originalpassword);
            return true;
        }

        /// <summary>
        /// 更新用户安全问题
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="questionid">问题id</param>
        /// <param name="answer">答案</param>
        /// <returns>成功返回true否则false</returns>
        public static bool UpdateUserSecques(int uid, int questionid, string answer)
        {
            if (!Exists(uid))
            {
                return false;
            }

            DatabaseProvider.GetInstance().UpdateUserSecques(uid, ForumUtils.GetUserSecques(questionid, answer));
            return true;
        }


        /// <summary>
        /// 更新用户最后登录时间
        /// </summary>
        /// <param name="uid">用户id</param>
        public static void UpdateUserLastvisit(int uid, string ip)
        {
            DatabaseProvider.GetInstance().UpdateUserLastvisit(uid, ip);
        }

        /// <summary>
        /// 更新用户当前的在线状态
        /// </summary>
        /// <param name="uidlist">用户uid列表</param>
        /// <param name="state">当前在线状态(0:离线,1:在线)</param>
        public static void UpdateUserOnlineState(string uidlist, int state, string activitytime)
        {
            if (!Utils.IsNumericArray(uidlist.Split(',')))
            {
                return;
            }

            switch (state)
            {
                case 0:		//正常退出
                    DatabaseProvider.GetInstance().UpdateUserOnlineStateAndLastActivity(uidlist, 0, activitytime);
                    break;
                case 1:		//正常登录
                    DatabaseProvider.GetInstance().UpdateUserOnlineStateAndLastVisit(uidlist, 1, activitytime);
                    break;
                case 2:		//超时退出
                    DatabaseProvider.GetInstance().UpdateUserOnlineStateAndLastActivity(uidlist, 0, activitytime);
                    break;
                case 3:		//隐身登录
                    DatabaseProvider.GetInstance().UpdateUserOnlineStateAndLastVisit(uidlist, 0, activitytime);
                    break;
            }

        }

        /// <summary>
        /// 更新用户当前的在线状态
        /// </summary>
        /// <param name="uid">用户uid列表</param>
        /// <param name="state">当前在线状态(0:离线,1:在线)</param>
        public static void UpdateUserOnlineState(int uid, int state, string activitytime)
        {
            if (uid < 1)
            {
                return;
            }

            switch (state)
            {
                case 0:		//正常退出
                    DatabaseProvider.GetInstance().UpdateUserOnlineStateAndLastActivity(uid, 0, activitytime);
                    break;
                case 1:		//正常登录
                    DatabaseProvider.GetInstance().UpdateUserOnlineStateAndLastVisit(uid, 1, activitytime);
                    break;
                case 2:		//超时退出
                    DatabaseProvider.GetInstance().UpdateUserOnlineStateAndLastActivity(uid, 0, activitytime);
                    break;
                case 3:		//隐身登录
                    DatabaseProvider.GetInstance().UpdateUserOnlineStateAndLastVisit(uid, 0, activitytime);
                    break;
            }

        }


        /// <summary>
        /// 更新用户当前的在线时间和最后活动时间
        /// </summary>
        /// <param name="uid">用户uid</param>
        public static void UpdateUserOnlineTime(int uid, string activitytime)
        {
            if (uid < 1)
            {
                return;
            }

            DatabaseProvider.GetInstance().UpdateUserLastActivity(uid, activitytime);
        }

        /// <summary>
        /// 设置用户信息表中未读短消息的数量
        /// </summary>
        /// <param name="uid">用户ID</param>
        /// <param name="pmnum">短消息数量</param>
        /// <returns>更新记录个数</returns>
        public static int SetUserNewPMCount(int uid, int pmnum)
        {
            return DatabaseProvider.GetInstance().SetUserNewPMCount(uid, pmnum);
        }

        /// <summary>
        /// 更新指定用户的勋章信息
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="medals">勋章信息</param>
        public static void UpdateMedals(int uid, string medals)
        {
            DatabaseProvider.GetInstance().UpdateMedals(uid, medals);
        }



        /// <summary>
        /// 将用户的未读短信息数量减小一个指定的值
        /// </summary>
        /// <param name="uid">用户ID</param>
        /// <param name="subval">短消息将要减小的值,负数为加</param>
        /// <returns>更新记录个数</returns>
        public static int DecreaseNewPMCount(int uid, int subval)
        {
            return DatabaseProvider.GetInstance().DecreaseNewPMCount(uid, subval);
        }


        /// <summary>
        /// 将用户的未读短信息数量减一
        /// </summary>
        /// <param name="uid">用户ID</param>
        /// <returns>更新记录个数</returns>
        public static int DecreaseNewPMCount(int uid)
        {
            return DecreaseNewPMCount(uid, 1);
        }

        /// <summary>
        /// 得到用户新短消息数量
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <returns>新短消息数</returns>
        public static int GetUserNewPMCount(int uid)
        {
            return DatabaseProvider.GetInstance().GetUserNewPMCount(uid);
        }

        /// <summary>
        /// 更新用户精华数
        /// </summary>
        /// <param name="useridlist">uid列表</param>
        /// <returns></returns>
        public static int UpdateUserDigest(string useridlist)
        {
            if (!Utils.IsNumericArray(Utils.SplitString(useridlist, ",")))
            {
                return 0;
            }
            return DatabaseProvider.GetInstance().UpdateUserDigest(useridlist);
        }

        /// <summary>
        /// 更新用户SpaceID
        /// </summary>
        /// <param name="spaceid">要更新的SpaceId</param>
        /// <param name="userid">要更新的UserId</param>
        /// <returns>是否更新成功</returns>
        public static bool UpdateUserSpaceId(int spaceid, int userid)
        {
            if (!Exists(userid))
            {
                return false;
            }

            DatabaseProvider.GetInstance().UpdateUserSpaceId(spaceid, userid);
            return true;
        }


        public static DataTable GetUserIdByAuthStr(string authstr)
        {
            return DatabaseProvider.GetInstance().GetUserIdByAuthStr(authstr);
        }

        /// <summary>
        /// 更新用户组
        /// </summary>
        /// <param name="uid">用户ID</param>
        /// <param name="groupID">用户组ID</param>
        public static void UpdateUserGroup(int uid, int groupID)
        {
            DatabaseProvider.GetInstance().UpdateUserGroup(uid, groupID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static Hashtable GetReportUsers()
        {
            DNTCache cache = DNTCache.GetCacheService();
            Hashtable ht = cache.RetrieveObject("/ReportUsers") as Hashtable;

            if (ht == null)
            {
                ht = new Hashtable();
                string groupidlist = GeneralConfigs.GetConfig().Reportusergroup;

                if (!Utils.IsNumericArray(groupidlist.Split(',')))
                    return ht;

                DataTable dt = DatabaseProvider.GetInstance().GetUsers(groupidlist);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ht[dt.Rows[i]["uid"]] = dt.Rows[i]["username"];
                }
            }

            return ht;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="rewritename"></param>
        /// <returns></returns>
        public static int GetUserIdByRewriteName(string rewritename)
        {
            return DatabaseProvider.GetInstance().GetUserIdByRewriteName(rewritename);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        public static void UpdateUserPMSetting(UserInfo user)
        {
            DatabaseProvider.GetInstance().UpdateUserPMSetting(user);
        }

        /// <summary>
        /// 写入 Cookie
        /// </summary>
        /// <param name="UserID"></param>
        public static void SaveUserIDToCookie(int UserID)
        {
            string Key = "SLS.Center.LoginUserID";

            HttpCookie cookieLoginUserID = new HttpCookie(Key, Shove._Security.Encrypt.Encrypt3DES(Utils.GetCallCert(), Shove._Security.Encrypt.EncryptString(Utils.GetCallCert(), UserID.ToString()), Utils.DesKey));
            cookieLoginUserID.Path = "/";


            try
            {
                HttpContext.Current.Response.Cookies.Add(cookieLoginUserID);
            }
            catch { }
        }

        /// <summary>
        /// 从 Cookie 中移除 UserID
        /// </summary>
        public static void RemoveUserIDFromCookie()
        {
            string Key = "SLS.Center.LoginUserID";

            HttpCookie cookieLoginUserID = new HttpCookie(Key);
            cookieLoginUserID.Value = "";
            cookieLoginUserID.Expires = DateTime.Now.AddYears(-20);
            cookieLoginUserID.Path = "/";

            try
            {
                HttpContext.Current.Response.Cookies.Add(cookieLoginUserID);
            }
            catch { }
        }

        /// <summary>
        /// 从 Cookie 中得到 UserID
        /// </summary>
        public static int GetUserIDFromCookie()
        {
            string Key = "SLS.Center.LoginUserID";

            HttpCookie cookieUser = HttpContext.Current.Request.Cookies[Key];

            if ((cookieUser == null) || (String.IsNullOrEmpty(cookieUser.Value)))
            {
                return -1;
            }

            string CookieUserID = cookieUser.Value;

            try
            {
                CookieUserID = Shove._Security.Encrypt.UnEncryptString(Common.Utils.GetCallCert(), Shove._Security.Encrypt.Decrypt3DES(Common.Utils.GetCallCert(), CookieUserID, Common.Utils.DesKey));
            }
            catch
            {
                return -1;
            }

            if (String.IsNullOrEmpty(CookieUserID))
            {
                return -1;
            }

            int UserID = Shove._Convert.StrToInt(CookieUserID, -1);
            
            return UserID;
        }
    }//class end
}
