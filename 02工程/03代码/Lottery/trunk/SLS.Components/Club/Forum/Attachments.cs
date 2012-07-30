using System;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

using Discuz.Common;
using Discuz.Data;
using Discuz.Entity;
using Discuz.Config;
using Discuz.Common.Xml;
using System.Xml;
namespace Discuz.Forum
{
	/// <summary>
	/// 附件操作类
	/// </summary>
	public class Attachments
	{

		/// <summary>
		/// 产生附件
		/// </summary>
		/// <param name="attachmentinfo">附件描述类数组</param>
		/// <returns>附件id数组</returns>
		public static int[] CreateAttachments(AttachmentInfo[] attachmentinfo)
		{
			int acount = attachmentinfo.Length;
			int icount = 0;
			int tid = 0;
			int pid = 0;
			int[] aid = new int[acount];
			int attType = 1;//普通附件,2为图片附件

			for (int i = 0; i < acount; i++)
			{
                if (attachmentinfo[i] != null && attachmentinfo[i].Sys_noupload.Equals(""))
				{
                    aid[i] = DatabaseProvider.GetInstance().CreateAttachment(attachmentinfo[i]);
					icount ++ ;
					tid = attachmentinfo[i].Tid;
					pid = attachmentinfo[i].Pid;
				    attachmentinfo[i].Aid = aid[i];
					if (attachmentinfo[i].Filetype.ToLower().StartsWith("image"))
						attType = 2;
				}
			}

			if (icount > 0)
			{
                DatabaseProvider.GetInstance().UpdateTopicAttachmentType(tid, attType);                
                DatabaseProvider.GetInstance().UpdatePostAttachmentType(pid, Posts.GetPostTableID(tid), attType);
			}
			
			return aid;
		}


		/// <summary>
		/// 获得指定附件的描述信息
		/// </summary>
		/// <param name="aid">附件id</param>
		/// <returns>描述信息</returns>
		public static AttachmentInfo GetAttachmentInfo(int aid)
		{
            return LoadSingleAttachmentInfo(DatabaseProvider.GetInstance().GetAttachmentInfo(aid), true);
		}

		/// <summary>
		/// 获得指定帖子的附件个数
		/// </summary>
		/// <param name="pid">帖子ID</param>
		/// <returns>附件个数</returns>
		public static int GetAttachmentCountByPid(int pid)
		{
            return DatabaseProvider.GetInstance().GetAttachmentCountByPid(pid);
        }

		/// <summary>
		/// 获得指定主题的附件个数
		/// </summary>
		/// <param name="tid">主题ID</param>
		/// <returns>附件个数</returns>
		public static int GetAttachmentCountByTid(int tid)
		{
            return DatabaseProvider.GetInstance().GetAttachmentCountByTid(tid);
        }

		/// <summary>
		/// 获得指定帖子的附件
		/// </summary>
		/// <param name="pid">帖子ID</param>
		/// <returns>帖子信息</returns>
		public static DataTable GetAttachmentListByPid(int pid)
		{
            return DatabaseProvider.GetInstance().GetAttachmentListByPid(pid);
		}

		/// <summary>
		/// 将系统设置的附件类型以DataTable的方式存入缓存
		/// </summary>
		/// <returns>系统设置的附件类型</returns>
		public static DataTable GetAttachmentType()
		{
			Discuz.Cache.DNTCache cache = Discuz.Cache.DNTCache.GetCacheService();
			DataTable dt = cache.RetrieveObject("/ForumSetting/AttachmentType") as DataTable;
			if (dt == null)
			{
                dt = DatabaseProvider.GetInstance().GetAttachmentType();

				cache.AddObject("/ForumSetting/AttachmentType", dt);
			}
			return dt;

		}

		/// <summary>
		/// 获得系统允许的附件类型和大小, 格式为: 扩展名,最大允许大小\r\n扩展名,最大允许大小\r\n.......
		/// </summary>
		/// <returns></returns>
		public static string GetAttachmentTypeArray(string filterexpression)
		{
			DataTable dt = GetAttachmentType();
			StringBuilder sb = new StringBuilder();
			foreach(DataRow dr in dt.Select(filterexpression))
			{
				sb.Append(dr["extension"].ToString());
				sb.Append(",");
				sb.Append(dr["maxsize"].ToString());
				sb.Append("\r\n");
			}
			return sb.ToString().Trim();

		}

		/// <summary>
		/// 获得当前设置允许的附件类型
		/// </summary>
		/// <returns>逗号间隔的附件类型字符串</returns>
		public static string GetAttachmentTypeString(string filterexpression)
		{
			DataTable dt = GetAttachmentType();
			StringBuilder sb = new StringBuilder();
			foreach (DataRow dr in dt.Select(filterexpression))
			{
				if (!sb.ToString().Equals(""))
				{
					sb.Append(",");
				}
				sb.Append(dr["extension"].ToString());
			}
			return sb.ToString().Trim();

		}


		/// <summary>
		/// 更新附件下载次数
		/// </summary>
		/// <param name="aid">附件id</param>
		public static void UpdateAttachmentDownloads(int aid)
		{
            DatabaseProvider.GetInstance().UpdateAttachmentDownloads(aid);
		}

		/// <summary>
		/// 更新主题中的附件标志
		/// </summary>
		/// <param name="tid">主题id</param>
		public static void UpdateTopicAttachment(int tid)
		{
            int attachmentCount = DatabaseProvider.GetInstance().GetAttachmentCountByTid(tid);
			
            DatabaseProvider.GetInstance().UpdateTopicAttachment(tid, attachmentCount > 0 ? 1: 0);
		}

		/// <summary>
		/// 更新主题中的附件标志
		/// </summary>
		/// <param name="tidlist">以逗号分割的主题id列表</param>
		public static void UpdateTopicAttachment(string tidlist)
		{
			string[] tid = Utils.SplitString(tidlist, ",");
			for (int i = 0; i < tid.Length; i++)
			{
				UpdateTopicAttachment(Utils.StrToInt(tid[i].Trim(), -1));
			}
		}

		/// <summary>
		/// 删除指定主题的所有附件
		/// </summary>
		/// <param name="tid">主题tid</param>
		/// <returns>删除个数</returns>
		public static int DeleteAttachmentByTid(int tid)
		{
            IDataReader reader = DatabaseProvider.GetInstance().GetAttachmentListByTid(tid);
			if (reader != null)
			{
				while (reader.Read())
				{
                   File.Delete(AppDomain.CurrentDomain.BaseDirectory + "upload/" + reader["filename"].ToString());
				}
				reader.Close();
			}

            DatabaseProvider.GetInstance().DelMyAttachmentByTid(tid.ToString());
            return DatabaseProvider.GetInstance().DeleteAttachmentByTid(tid);
		}

		/// <summary>
		/// 删除指定主题的所有附件
		/// </summary>
		/// <param name="tidlist">版块tid列表</param>
		/// <returns>删除个数</returns>
		public static int DeleteAttachmentByTid(string tidlist)
		{
			if (!Utils.IsNumericArray(tidlist.Split(',')))
			{
				return -1;
			}

            IDataReader reader = DatabaseProvider.GetInstance().GetAttachmentListByTid(tidlist);

			if (reader != null)
			{
				while (reader.Read())
				{
                    if (reader["filename"].ToString().Trim().ToLower().IndexOf("http") < 0)
                    {
                        File.Delete(AppDomain.CurrentDomain.BaseDirectory + "upload/" + reader["filename"].ToString());
                    }
				}
				reader.Close();
			}

            DatabaseProvider.GetInstance().DelMyAttachmentByTid(tidlist);
            return DatabaseProvider.GetInstance().DeleteAttachmentByTid(tidlist);
		}


		/// <summary>
		/// 删除指定附件
		/// </summary>
		/// <param name="aid">附件aid</param>
		/// <returns>删除个数</returns>
		public static int DeleteAttachment(int aid)
		{			
            IDataReader reader = DatabaseProvider.GetInstance().GetAttachmentInfo(aid);
			int tid = 0;
			int pid = 0;
			if (reader != null)
			{
				if (reader.Read())
				{
                    if (reader["filename"].ToString().Trim().ToLower().IndexOf("http") < 0)
                    {
                        string attachmentFilePath = AppDomain.CurrentDomain.BaseDirectory + "upload/" + reader["filename"].ToString();
                            File.Delete(attachmentFilePath);
                    }
					tid = Utils.StrToInt(reader["tid"],0);
					pid = Utils.StrToInt(reader["pid"],0);
				}
				reader.Close();
			}

            int reval = DatabaseProvider.GetInstance().DeleteAttachment(aid);
           
            DeleteAttachment(aid.ToString(), pid, tid);

			return reval;
			
		}

        /// <summary>
        /// 删除指定附件id的附件同时更新主题和帖子中的附件个数
        /// </summary>
        /// <param name="aid">附件id</param>
        /// <param name="pid">附件所属帖子id</param>
        /// <param name="tid">附件所属主题id</param>
        private static void DeleteAttachment(string aidlist, int pid, int tid)
        {
            DatabaseProvider.GetInstance().DelMyAttachmentByAid(aidlist);
			if (tid > 0)
			{
                int attcount = DatabaseProvider.GetInstance().GetAttachmentCountByPid(pid);
                if (attcount <= 0)
				{
                    DatabaseProvider.GetInstance().UpdatePostAttachment(pid, Posts.GetPostTableID(tid), 0);	
                }

                attcount = DatabaseProvider.GetInstance().GetAttachmentCountByTid(tid);
                if (attcount <= 0)
				{
                    DatabaseProvider.GetInstance().UpdateTopicAttachment(tid, 0);	
                }
			}
        }


        /// <summary>
        /// 更新附件信息
		/// </summary>
		/// <param name="attachmentInfo">附件对象</param>
		/// <returns>返回被更新的数量</returns>
		public static int UpdateAttachment(AttachmentInfo attachmentInfo)
		{
            return DatabaseProvider.GetInstance().UpdateAttachment(attachmentInfo);			
		}

		/// <summary>
        /// 更新附件信息
		/// </summary>
		/// <param name="aid">附件Id</param>
		/// <param name="readperm">阅读权限</param>
		/// <param name="description">描述</param>
		/// <returns>返回被更新的数量</returns>
		public static int UpdateAttachment(int aid, int readperm, string description)
		{
            return DatabaseProvider.GetInstance().UpdateAttachment(aid, readperm, description);
		}

		/// <summary>
		/// 批量删除附件
		/// </summary>
		/// <param name="aidList">附件Id，以英文逗号分割</param>
		/// <returns>返回被删除的个数</returns>
		public static int DeleteAttachment(string aidList)
		{
            IDataReader reader = DatabaseProvider.GetInstance().GetAttachmentList(aidList);
			int tid = 0;
			int pid = 0;
			while (reader.Read())
			{
                if (reader["filename"].ToString().Trim().ToLower().IndexOf("http") < 0)
                {
                    string attachmentFilePath = AppDomain.CurrentDomain.BaseDirectory + "upload/" + reader["filename"].ToString();
                    if (Utils.FileExists(attachmentFilePath))
                    {
                        try
                        {
                            FileInfo fileInfo = new FileInfo(attachmentFilePath);
                            fileInfo.Attributes = FileAttributes.Normal;
                            fileInfo.Delete();
                        }
                        catch { }
                    }
                }

				tid = Utils.StrToInt(reader["tid"],0);
				pid = Utils.StrToInt(reader["pid"],0);
			}
			reader.Close();

            int reval = DatabaseProvider.GetInstance().DeleteAttachment(aidList);

            DeleteAttachment(aidList, pid, tid);

			return reval;
		}


		/// <summary>
		/// 获得上传附件文件的大小
		/// </summary>
		/// <param name="uid">用户id</param>
		/// <returns></returns>
		public static int GetUploadFileSizeByuserid(int uid)
		{
            return DatabaseProvider.GetInstance().GetUploadFileSizeByUserId(uid);	
        }

		/// <summary>
		/// 过滤临时内容中的本地临时标签
		/// </summary>
		/// <param name="aid">广告id</param>
		/// <param name="attachmentinfo">附件信息列表</param>
		/// <param name="tempMessage">临时信息内容</param>
		/// <returns>过滤结果</returns>
		public static string FilterLocalTags(int[] aid, AttachmentInfo[] attachmentinfo, string tempMessage)
		{
			Match m;
			Regex r;
			for (int i = 0; i < aid.Length; i++)
			{
				if (aid[i] > 0)
				{

					r = new Regex(@"\[localimg=(\d{1,}),(\d{1,})\]" + attachmentinfo[i].Sys_index + @"\[\/localimg\]", RegexOptions.IgnoreCase);
					for (m = r.Match(tempMessage); m.Success; m = m.NextMatch()) 
					{
						tempMessage = tempMessage.Replace(m.Groups[0].ToString(), "[attachimg]" + aid[i] + "[/attachimg]");
					}

					r = new Regex(@"\[local\]" + attachmentinfo[i].Sys_index + @"\[\/local\]", RegexOptions.IgnoreCase);
					for (m = r.Match(tempMessage); m.Success; m = m.NextMatch()) 
					{
						tempMessage = tempMessage.Replace(m.Groups[0].ToString(), "[attach]" + aid[i] + "[/attach]");
					}

				}
			}

			tempMessage = Regex.Replace(tempMessage, @"\[localimg=(\d{1,}),\s*(\d{1,})\][\s\S]+?\[/localimg\]", string.Empty, RegexOptions.IgnoreCase);
			tempMessage = Regex.Replace(tempMessage, @"\[local\][\s\S]+?\[/local\]", string.Empty, RegexOptions.IgnoreCase);
			return tempMessage;
		}

		/// <summary>
		/// 绑定附件数组中的参数，返回无效附件个数
		/// </summary>
		/// <param name="attachmentinfo">附件类型</param>
		/// <param name="postid">帖子id</param>
		/// <param name="msg">原有提示信息</param>
		/// <param name="topicid">主题id</param>
		/// <param name="userid">用户id</param>
		/// <returns>无效附件个数</returns>
		public static int BindAttachment(AttachmentInfo[] attachmentinfo, int postid, StringBuilder msg, int topicid, int userid)
		{
			int acount = attachmentinfo.Length;
			// 附件查看权限
			string[] readperm = String.IsNullOrEmpty(DNTRequest.GetString("readperm")) ? null : DNTRequest.GetString("readperm").Split(',');
			string[] attachdesc = DNTRequest.GetString("attachdesc") == null ? null : DNTRequest.GetString("attachdesc").Split(',');
			string[] localid = DNTRequest.GetString("localid") == null ? null : DNTRequest.GetString("localid").Split(',');
			//设置无效附件计数器，将在下面UserCreditsFactory.UpdateUserCreditsByUploadAttachment方法中减去该计数器的值
			int errorAttachment = 0;
            int i_readperm = 0;
			for (int i = 0; i < acount; i++)
			{
				if (attachmentinfo[i] != null)
				{
					if (Utils.IsNumeric(localid[i+1]))
						attachmentinfo[i].Sys_index = Convert.ToInt32(localid[i+1]);

					attachmentinfo[i].Uid = userid;
					attachmentinfo[i].Tid = topicid;
					attachmentinfo[i].Pid = postid;
					attachmentinfo[i].Postdatetime = Utils.GetDateTime();
					attachmentinfo[i].Readperm = 0;
					if (readperm != null)
					{
                        i_readperm = Utils.StrToInt(readperm[i + 1], 0);
                        //当为最大阅读仅限(255)时
                        i_readperm = i_readperm > 255 ? 255 : i_readperm;
                        attachmentinfo[i].Readperm = i_readperm;
					}

					if (attachdesc != null && !attachdesc[i+1].Equals(""))
					{
						attachmentinfo[i].Description = Utils.HtmlEncode(attachdesc[i+1]);
					}

					if (!attachmentinfo[i].Sys_noupload.Equals(""))
					{
						msg.Append("<tr><td align=\"left\">");
						msg.Append(attachmentinfo[i].Attachment);
						msg.Append("</td>");
						msg.Append("<td align=\"left\">");
						msg.Append(attachmentinfo[i].Sys_noupload);
						msg.Append("</td></tr>");
						errorAttachment++;
					}
				}
			}
			return errorAttachment;
		}

		/// <summary>
		/// 取得主题贴的第一个图片附件
		/// </summary>
		/// <param name="tid">主题id</param>
		public static AttachmentInfo GetFirstImageAttachByTid(int tid)
		{
     		return LoadSingleAttachmentInfo(DatabaseProvider.GetInstance().GetFirstImageAttachByTid(tid));
		}

        /// <summary>
        /// 将单个附件DataRow转换为AttachmentInfo类
        /// </summary>
        /// <param name="drAttach">单个附件DataRow</param>
        /// <returns>AttachmentInfo类</returns>
        private static AttachmentInfo LoadSingleAttachmentInfo(IDataReader drAttach)
        {
            return LoadSingleAttachmentInfo(drAttach, false);
        }

        /// <summary>
		/// 将单个附件DataRow转换为AttachmentInfo类
		/// </summary>
        /// <param name="drAttach">单个附件DataRow</param>
        /// <param name="drAttach">是否返回原始路径</param>
        /// <returns>AttachmentInfo类</returns>
		private static AttachmentInfo LoadSingleAttachmentInfo(IDataReader drAttach, bool isOriginalFilename)
        {
			AttachmentInfo attach = new AttachmentInfo();
            if (drAttach.Read())
            {
                attach.Aid = Convert.ToInt32(drAttach["aid"]);
                attach.Uid = Convert.ToInt32(drAttach["uid"]);
                attach.Tid = Convert.ToInt32(drAttach["tid"]);
                attach.Pid = Convert.ToInt32(drAttach["pid"]);
                attach.Postdatetime = drAttach["postdatetime"].ToString();
                attach.Readperm = Convert.ToInt32(drAttach["readperm"]);

                if (isOriginalFilename)
                {
                    attach.Filename = drAttach["filename"].ToString();
                }
                else if (drAttach["filename"].ToString().Trim().ToLower().IndexOf("http") < 0)
                {
                    attach.Filename = "/upload/" + drAttach["filename"].ToString().Trim().Replace("\\", "/");
                }
                else
                {
                    attach.Filename = drAttach["filename"].ToString().Trim().Replace("\\", "/");
                }
                attach.Description = drAttach["description"].ToString().Trim();
                attach.Filetype = drAttach["filetype"].ToString().Trim();
                attach.Attachment = drAttach["attachment"].ToString().Trim();
                attach.Filesize = Convert.ToInt32(drAttach["filesize"]);
                attach.Downloads = Convert.ToInt32(drAttach["downloads"]);
            }
            drAttach.Close();
			return attach;
		}

		/// <summary>
		/// 根据主题id得到缩略图附件对象
		/// </summary>
		/// <param name="tid">主题id</param>
		/// <param name="maxsize">最大图片尺寸</param>
		/// <param name="type">缩略图类型</param>
		/// <returns>缩略图附件对象</returns>
		public static AttachmentInfo GetThumbnailAttachByTid(int tid, int maxsize, ThumbnailType type)
		{
			int theMaxsize = 300;
			if (maxsize < theMaxsize)
			{
				theMaxsize = maxsize;
			}
			string attCachePath = string.Format("{0}cache/thumbnail/t_{1}_{2}_{3}.jpg", "/", tid, theMaxsize, (int)type);
			AttachmentInfo attach = GetFirstImageAttachByTid(tid);
			if (attach == null)
				return null;
			attach.Attachment = attCachePath;
			string attPhyCachePath = Utils.GetMapPath(attCachePath);
			if (!File.Exists(attPhyCachePath))
			{			
				CreateTopicAttThumbnail(attPhyCachePath, type, Utils.GetMapPath(attach.Filename), theMaxsize);
			}
			return attach;
		}

		/// <summary>
		/// 根据主题id得到缩略图地址
		/// </summary>
		/// <param name="tid">主题id</param>
		/// <param name="maxsize">最大图片尺寸</param>
		/// <param name="type">缩略图类型</param>
		/// <returns>缩略图地址</returns>
		public static string GetThumbnailByTid(int tid, int maxsize, ThumbnailType type)
		{
			int theMaxsize = 300;
			if (maxsize < theMaxsize)
			{
				theMaxsize = maxsize;
			}
			string attCachePath = string.Format("{0}cache/thumbnail/t_{1}_{2}_{3}.jpg", "/", tid, theMaxsize, (int)type);
			string attPhyCachePath = Utils.GetMapPath(attCachePath);
			if (!File.Exists(attPhyCachePath))
			{			
				AttachmentInfo attach = GetFirstImageAttachByTid(tid);
				if (attach == null)
					return "";
				attach.Attachment = attCachePath;

				CreateTopicAttThumbnail(attPhyCachePath, type, Utils.GetMapPath(attach.Filename), theMaxsize);
			}
			return attCachePath;
		}

	    /// <summary>
	    /// 创建主题附件缩略图
	    /// </summary>
	    /// <param name="attPhyCachePath">缓存文件路径</param>
	    /// <param name="type">缩略图类型</param>
	    /// <param name="attPhyPath">附件物理路径</param>
	    /// <param name="theMaxsize">最大尺寸</param>
		private static void CreateTopicAttThumbnail(string attPhyCachePath, ThumbnailType type, string attPhyPath, int theMaxsize)
		{
			if (!File.Exists(attPhyPath))
				return;
			string cachedir = AppDomain.CurrentDomain.BaseDirectory + "cache/thumbnail/";
			if (!Directory.Exists(cachedir))
			{
			    try
			    {
				    Utils.CreateDir(cachedir);
				}
				catch
				{
				    throw new Exception("请检查程序目录下cache文件夹的用户权限！");
				}
			}
			DirectoryInfo dir = new DirectoryInfo(cachedir);
			FileInfo[] files = dir.GetFiles();

			if (files.Length > 1500)
			{	
				QuickSort(files, 0, files.Length - 1);

				for (int i = files.Length-1; i >= 1400; i--)
				{
					try
					{
						files[i].Delete();
					}
					catch
					{}
				}
			}
            try
            {
                switch (type)
                {
                    case ThumbnailType.Square:
                        Thumbnail.MakeSquareImage(attPhyPath, attPhyCachePath, theMaxsize);
                        break;
                    case ThumbnailType.Thumbnail:
                        Thumbnail.MakeThumbnailImage(attPhyPath, attPhyCachePath, theMaxsize, theMaxsize);
                        break;
                    default:
                        Thumbnail.MakeSquareImage(attPhyPath, attPhyCachePath, theMaxsize);
                        break;
                }

                ////当支持FTP上传附件时
                //if (FTPConfigs.GetConfig().Allowuploadattach)
                //{
                //    FTPs ftps = new FTPs();
                //    ftps.AsyncUpLoadFile(attPhyPath);
                //}
            }
            catch
            {
            }
		}

		#region Helper
		/// <summary>
		/// 快速排序算法
		/// </summary>
		/// 快速排序为不稳定排序,时间复杂度O(nlog2n),为同数量级中最快的排序方法
		/// <param name="arr">划分的数组</param>
		/// <param name="low">数组低端上标</param>
		/// <param name="high">数组高端下标</param>
		/// <returns></returns>
		private static int Partition(FileInfo[] arr, int low, int high)
		{
			//进行一趟快速排序,返回中心轴记录位置
			// arr[0] = arr[low];
			FileInfo pivot = arr[low];//把中心轴置于arr[0]
			while (low < high)
			{
				while (low < high && arr[high].CreationTime <= pivot.CreationTime)
					--high;
				//将比中心轴记录小的移到低端
				Swap(ref arr[high], ref arr[low]);
				while (low < high && arr[low].CreationTime >= pivot.CreationTime)
					++low;
				Swap(ref arr[high], ref arr[low]);
				//将比中心轴记录大的移到高端
			}
			arr[low] = pivot; //中心轴移到正确位置
			return low;  //返回中心轴位置
		}
		private static void Swap(ref FileInfo i, ref FileInfo j)
		{
			FileInfo t;
			t = i;
			i = j;
			j = t;
		}

		/// <summary>
		/// 快速排序算法
		/// </summary>
		/// 快速排序为不稳定排序,时间复杂度O(nlog2n),为同数量级中最快的排序方法
		/// <param name="arr">划分的数组</param>
		/// <param name="low">数组低端上标</param>
		/// <param name="high">数组高端下标</param>
		private static void QuickSort(FileInfo[] arr, int low, int high)
		{
			if (low <= high - 1)//当 arr[low,high]为空或只一个记录无需排序
			{
				int pivot = Partition(arr, low, high);
				QuickSort(arr, low, pivot - 1);
				QuickSort(arr, pivot + 1, high);

			}
		}

		#endregion

        /// <summary>
        /// 根据帖子ID删除附件
        /// </summary>
        /// <param name="pid">帖子ID</param>
        public static void DeleteAttachmentByPid(int pid)
        {
            DataTable dtAttach = GetAttachmentListByPid(pid);
            if (dtAttach != null)
            {
                foreach (DataRow dr in dtAttach.Rows)
                {
                    if (dr["filename"].ToString().Trim().ToLower().IndexOf("http") < 0)
                    {
                        File.Delete(AppDomain.CurrentDomain.BaseDirectory + "upload/" + dr["filename"].ToString());
                    }
                }
            }
            DatabaseProvider.GetInstance().DelMyAttachmentByPid(pid.ToString());
            DatabaseProvider.GetInstance().DeleteAttachmentByPid(pid);
        }

        /// <summary>
        /// 获取指定用户id的附件数
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <returns>附件数量</returns>
        public static int GetUserAttachmentCount(int uid)
        {
            return DatabaseProvider.GetInstance().GetUserAttachmentCount(uid);
        }

        /// <summary>
        /// 获取指定用户id的附件数
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="typeid">附件类型id</param>
        /// <returns>附件数量</returns>
        public static int GetUserAttachmentCount(int uid,int typeid)
        {
            return DatabaseProvider.GetInstance().GetUserAttachmentCount(uid, SetExtNamelist(typeid));
        }
      
        /// <summary>
        /// 按照附件分类返回用户的附件
        /// </summary>
        /// </summary>
        /// <param name="typeid">附件类型id</param>
        /// <returns>返回用户附件</returns>
        public static string SetExtNamelist(int typeid)
        {
            string newstring = "";
            foreach (string s in GetExtName(typeid).Split(','))
            {
                newstring += "'" + s + "',";
            }
            return newstring.Remove(newstring.Length - 1, 1);
        }

        public static Discuz.Common.Generic.List<MyAttachmentInfo> GetAttachmentByUid(int uid, int typeid, int pageIndex, int pageSize)
        {

            Discuz.Common.Generic.List<MyAttachmentInfo> myattachment = new Discuz.Common.Generic.List<MyAttachmentInfo>();
            if (pageIndex <= 0)
            {
                return myattachment;
            }
            else
            {
                IDataReader reader;
                if (typeid == 0)
                {

                    reader = DatabaseProvider.GetInstance().GetAttachmentByUid(uid, pageIndex, pageSize);
                }
                else
                {

                    reader = DatabaseProvider.GetInstance().GetAttachmentByUid(uid, SetExtNamelist(typeid), pageIndex, pageSize);
                }

                while (reader.Read())
                {
                    MyAttachmentInfo info = new MyAttachmentInfo();

                    info.Uid = Int32.Parse(reader["uid"].ToString());
                    info.Aid = Int32.Parse(reader["aid"].ToString());
                    info.Postdatetime = reader["postdatetime"].ToString();
                    info.Filename = reader["filename"].ToString().StartsWith("http://") ? reader["filename"].ToString() : "upload/" + reader["filename"].ToString().Replace("\\", "/");
                    info.Description = reader["description"].ToString();
                    info.Attachment = reader["attachment"].ToString();
                    info.SimpleName = Utils.ConvertSimpleFileName(reader["attachment"].ToString(), "...", 6, 3, 15);
                    info.Downloads = Int32.Parse(reader["downloads"].ToString());

                    myattachment.Add(info);
                }
                reader.Close();

                return myattachment;
            }
        }

       
        /// <summary>
        /// 获取附件类型列表
        /// </summary>
        /// <returns>附件类型列表</returns>
        public static Discuz.Common.Generic.List<AttachmentType> AttachTypeList()
        {
            Discuz.Common.Generic.List<AttachmentType> list = new Discuz.Common.Generic.List<AttachmentType>();

            foreach (AttachmentType act in MyAttachmentsTypeConfigs.GetConfig().AttachmentType)
            {
                AttachmentType MyAttachmentType = new AttachmentType();
                MyAttachmentType.TypeId = act.TypeId;
                MyAttachmentType.TypeName = act.TypeName;
                MyAttachmentType.ExtName = act.ExtName;
                list.Add(MyAttachmentType);
            }

            return list;
        }


        /// <summary>
        /// 获取我的附件指定类型的扩展名
        /// </summary>
        /// <param name="typeid">附件类型id</param>
        /// <returns>扩展名称</returns>
        public static string GetExtName(int typeid)
        {
            foreach (AttachmentType act in GetAttach())
            {
                if (act.TypeId == typeid)
                {
                    return act.ExtName;
                }
            }
            return "";
        }


        /// <summary>
        /// 获取我的附件类型信息
        /// </summary>
        /// <returns>我的附件类型信息</returns>
        public static AttachmentType[] GetAttach()
        {
            return MyAttachmentsTypeConfigs.GetConfig().AttachmentType;
        }

    }// class end
}
