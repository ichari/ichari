using System;
using System.Data;
using System.Data.Common;

using Discuz.Common;
using Discuz.Data;
using Discuz.Config;
using Discuz.Entity;

namespace Discuz.Forum
{
	/// <summary>
	/// 短消息操作类
	/// </summary>
	public class PrivateMessages
	{
		/// <summary>
		/// 负责发送新用户注册欢迎信件的用户名称, 该名称同时不允许用户注册
		/// </summary>
		public const string SystemUserName = "系统";
		
		
		/// <summary>
		/// 获得指定ID的短消息的内容
		/// </summary>
		/// <param name="pmid">短消息pmid</param>
		/// <returns>短消息内容</returns>
		public static PrivateMessageInfo GetPrivateMessageInfo(int pmid)
		{
			PrivateMessageInfo __privatemessageinfo = new PrivateMessageInfo();

            IDataReader reader = DatabaseProvider.GetInstance().GetPrivateMessageInfo(pmid);
			if(reader.Read())
			{
				__privatemessageinfo.Pmid = pmid;
				__privatemessageinfo.Msgfrom = reader["msgfrom"].ToString();
				__privatemessageinfo.Msgfromid = int.Parse(reader["msgfromid"].ToString());
				__privatemessageinfo.Msgto = reader["msgto"].ToString();
				__privatemessageinfo.Msgtoid = int.Parse(reader["msgtoid"].ToString());
				__privatemessageinfo.Folder = Int16.Parse(reader["folder"].ToString());
				__privatemessageinfo.New = int.Parse(reader["new"].ToString());
				__privatemessageinfo.Subject = reader["subject"].ToString();
				__privatemessageinfo.Postdatetime = reader["postdatetime"].ToString();
				__privatemessageinfo.Message = reader["message"].ToString();
				reader.Close();
				return __privatemessageinfo;
			}
			reader.Close();
			return null;

		}


	

		/// <summary>
		/// 得到当用户的短消息数量
		/// </summary>
		/// <param name="userid">用户ID</param>
		/// <param name="folder">所属文件夹(0:收件箱,1:发件箱,2:草稿箱)</param>
		/// <returns>短消息数量</returns>
		public static int GetPrivateMessageCount(int userid, int folder)
		{
			return GetPrivateMessageCount(userid,folder,-1);
		}

		/// <summary>
		/// 得到当用户的短消息数量
		/// </summary>
		/// <param name="userid">用户ID</param>
		/// <param name="folder">所属文件夹(0:收件箱,1:发件箱,2:草稿箱)</param>
		/// <param name="state">短消息状态(0:已读短消息、1:未读短消息、-1:全部短消息)</param>
		/// <returns>短消息数量</returns>
		public static int GetPrivateMessageCount(int userid, int folder,int state)
		{
            return DatabaseProvider.GetInstance().GetPrivateMessageCount(userid, folder, state);
		}

		/// <summary>
		/// 创建短消息
		/// </summary>
		/// <param name="__privatemessageinfo">短消息内容</param>
		/// <param name="savetosentbox">设置短消息是否在发件箱保留(0为不保留, 1为保留)</param>
		/// <returns>短消息在数据库中的pmid</returns>
		public static int CreatePrivateMessage(PrivateMessageInfo __privatemessageinfo, int savetosentbox)
		{
            return DatabaseProvider.GetInstance().CreatePrivateMessage(__privatemessageinfo, savetosentbox);    
        }


		/// <summary>
		/// 删除指定用户的短信息
		/// </summary>
		/// <param name="userid">用户ID</param>
		/// <param name="pmitemid">要删除的短信息列表(数组)</param>
		/// <returns>删除记录数</returns>
		public static int DeletePrivateMessage(int userid, string[] pmitemid)
		{
			foreach (string id in pmitemid)
			{
				if (!Utils.IsNumeric(id))
				{
					return -1;
				}
			}

			string pmidlist = String.Join(",",pmitemid);

            int reval = DatabaseProvider.GetInstance().DeletePrivateMessages(userid, pmidlist);
            if (reval > 0)
			{
                int newpmcount = DatabaseProvider.GetInstance().GetNewPMCount(userid);
				Users.SetUserNewPMCount(userid,newpmcount);
			}

			return reval;

		}

		/// <summary>
		/// 删除指定用户的一条短信息
		/// </summary>
		/// <param name="userid">用户ID</param>
		/// <param name="pmid">要删除的短信息ID</param>
		/// <returns>删除记录数</returns>
		public static int DeletePrivateMessage(int userid,int pmid)
		{
            int reval = DatabaseProvider.GetInstance().DeletePrivateMessage(userid, pmid);
			if (reval > 0)
			{
                int newpmcount = DatabaseProvider.GetInstance().GetNewPMCount(userid);
				Users.SetUserNewPMCount(userid,newpmcount);
			}

			return reval;

		}

		/// <summary>
		/// 设置短信息状态
		/// </summary>
		/// <param name="pmid">短信息ID</param>
		/// <param name="state">状态值</param>
		/// <returns>更新记录数</returns>
		public static int SetPrivateMessageState(int pmid,byte state)
		{
            return DatabaseProvider.GetInstance().SetPrivateMessageState(pmid, state);
        }

#if NET1

        #region 短信息集合函数

        /// <summary>
        /// 获得指定用户的短信息列表
        /// </summary>
        /// <param name="userid">用户ID</param>
        /// <param name="folder">短信息类型(0:收件箱,1:发件箱,2:草稿箱)</param>
        /// <param name="pagesize">每页显示短信息数</param>
        /// <param name="pageindex">当前要显示的页数</param>
        /// <param name="strwhere">筛选条件</param>
        /// <returns>短信息列表</returns>
	    public static PrivateMessageInfoCollection GetPrivateMessageCollection(int userid, int folder, int pagesize, int pageindex, int inttype)
        {
            PrivateMessageInfoCollection coll = new PrivateMessageInfoCollection();
            IDataReader reader = DatabaseProvider.GetInstance().GetPrivateMessageList(userid, folder, pagesize, pageindex, inttype);
            if (reader != null)
            {

                while (reader.Read())
                {
                    PrivateMessageInfo info = new PrivateMessageInfo();
                    info.Pmid = int.Parse(reader["pmid"].ToString());
                    info.Msgfrom = reader["msgfrom"].ToString();
                    info.Msgfromid = int.Parse(reader["msgfromid"].ToString());
                    info.Msgto = reader["msgto"].ToString();
                    info.Msgtoid = int.Parse(reader["msgtoid"].ToString());
                    info.Folder = Int16.Parse(reader["folder"].ToString());
                    info.New = int.Parse(reader["new"].ToString());
                    info.Subject = reader["subject"].ToString();
                    info.Postdatetime = reader["postdatetime"].ToString();
                    info.Message = reader["message"].ToString();
                    coll.Add(info);
                }
                reader.Close();
            }
            return coll;
        }



        /// <summary>
        /// 返回短标题的收件箱短消息列表
        /// </summary>
        /// <param name="userid">用户ID</param>
        /// <param name="pagesize">每页显示短信息数</param>
        /// <param name="pageindex">当前要显示的页数</param>
        /// <param name="strwhere">筛选条件</param>
        /// <returns></returns>
        public static PrivateMessageInfoCollection GetPrivateMessageCollectionForIndex(int userid, int pagesize, int pageindex, int  inttype)
        {
            PrivateMessageInfoCollection coll = GetPrivateMessageCollection(userid, 0, pagesize, pageindex, inttype);
            if (coll.Count > 0)
            {
                for (int i = 0; i < coll.Count; i++)
                {
                    coll[i].Message = Utils.GetSubString(coll[i].Message, 20, "...");
                }

            }
            return coll;
        }

        #endregion


#else

        #region 短信息泛型函数

        /// <summary>
        /// 获得指定用户的短信息列表
        /// </summary>
        /// <param name="userid">用户ID</param>
        /// <param name="folder">短信息类型(0:收件箱,1:发件箱,2:草稿箱)</param>
        /// <param name="pagesize">每页显示短信息数</param>
        /// <param name="pageindex">当前要显示的页数</param>
        /// <param name="strwhere">筛选条件</param>
        /// <returns>短信息列表</returns>
        public static Discuz.Common.Generic.List<PrivateMessageInfo> GetPrivateMessageCollection(int userid, int folder, int pagesize, int pageindex, int inttype)
        {
            Discuz.Common.Generic.List<PrivateMessageInfo> coll = new Discuz.Common.Generic.List<PrivateMessageInfo>();
            IDataReader reader = DatabaseProvider.GetInstance().GetPrivateMessageList(userid, folder, pagesize, pageindex, inttype);
            if (reader != null)
            {

                while (reader.Read())
                {
                    PrivateMessageInfo info = new PrivateMessageInfo();
                    info.Pmid = int.Parse(reader["pmid"].ToString());
                    info.Msgfrom = reader["msgfrom"].ToString();
                    info.Msgfromid = int.Parse(reader["msgfromid"].ToString());
                    info.Msgto = reader["msgto"].ToString();
                    info.Msgtoid = int.Parse(reader["msgtoid"].ToString());
                    info.Folder = Int16.Parse(reader["folder"].ToString());
                    info.New = int.Parse(reader["new"].ToString());
                    info.Subject = reader["subject"].ToString();
                    info.Postdatetime = reader["postdatetime"].ToString();
                    info.Message = reader["message"].ToString();
                    coll.Add(info);
                }
                reader.Close();
            }
            return coll;
        }



        /// <summary>
        /// 返回短标题的收件箱短消息列表
        /// </summary>
        /// <param name="userid">用户ID</param>
        /// <param name="pagesize">每页显示短信息数</param>
        /// <param name="pageindex">当前要显示的页数</param>
        /// <param name="strwhere">筛选条件</param>
        /// <returns>收件箱短消息列表</returns>
        public static Discuz.Common.Generic.List<PrivateMessageInfo> GetPrivateMessageCollectionForIndex(int userid, int pagesize, int pageindex, int  inttype)
        {
            Discuz.Common.Generic.List<PrivateMessageInfo> coll = GetPrivateMessageCollection(userid, 0, pagesize, pageindex, inttype);
            if (coll.Count > 0)
            {
                for (int i = 0; i < coll.Count; i++)
                {
                    coll[i].Message = Utils.GetSubString(coll[i].Message, 20, "...");
                }

            }
            return coll;
        }

        #endregion

#endif

    } //class end
}
