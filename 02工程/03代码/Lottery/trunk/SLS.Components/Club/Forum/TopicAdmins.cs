using System;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Text;

using Discuz.Common;
using Discuz.Data;
using Discuz.Config;
using Discuz.Entity;
using System.Collections;

namespace Discuz.Forum
{
    /// <summary>
    /// 主题管理操作类
    /// </summary>
    public class TopicAdmins
    {
        /// <summary>
        /// 设置主题指定字段的属性值
        /// </summary>
        /// <param name="topiclist">要设置的主题列表</param>
        /// <param name="field">要设置的字段</param>
        /// <param name="intValue">属性值</param>
        /// <returns>更新主题个数</returns>
        private static int SetTopicStatus(string topiclist, string field, int intValue)
        {
            if (!Utils.InArray(field.ToLower().Trim(), "displayorder,highlight,digest"))
            {
                return -1;
            }

            if (!Utils.IsNumericArray(topiclist.Split(',')))
            {
                return -1;
            }

            return DatabaseProvider.GetInstance().SetTopicStatus(topiclist, field, intValue);
        }



        /// <summary>
        /// 设置主题指定字段的属性值
        /// </summary>
        /// <param name="topiclist">要设置的主题列表</param>
        /// <param name="field">要设置的字段</param>
        /// <param name="intValue">属性值</param>
        /// <returns>更新主题个数</returns>
        private static int SetTopicStatus(string topiclist, string field, byte intValue)
        {
            if (!Utils.InArray(field.ToLower().Trim(), "displayorder,highlight,digest"))
            {
                return -1;
            }

            if (!Utils.IsNumericArray(topiclist.Split(',')))
            {
                return -1;
            }

            return DatabaseProvider.GetInstance().SetTopicStatus(topiclist, field, intValue);
        }



        /// <summary>
        /// 设置主题指定字段的属性值(字符型)
        /// </summary>
        /// <param name="topiclist">要设置的主题列表</param>
        /// <param name="field">要设置的字段</param>
        /// <param name="intValue">属性值</param>
        /// <returns>更新主题个数</returns>
        private static int SetTopicStatus(string topiclist, string field, string intValue)
        {
            if ((",displayorder,highlight,digest,").IndexOf("," + field.ToLower().Trim() + ",") < 0)
            {
                return -1;
            }

            if (!Utils.IsNumericArray(topiclist.Split(',')))
            {
                return -1;
            }

            return DatabaseProvider.GetInstance().SetTopicStatus(topiclist, field, intValue);
        }


        /// <summary>
        /// 将主题置顶/解除置顶
        /// </summary>
        /// <param name="topiclist">要设置的主题列表</param>
        /// <param name="intValue">置顶级别( 0 为解除置顶)</param>
        /// <returns>更新主题个数</returns>
        public static int SetTopTopicList(int fid, string topiclist, short intValue)
        {
            if (SetTopicStatus(topiclist, "displayorder", intValue) > 0)
            {
                if (ResetTopTopicList(fid) == 1)
                {
                    return 1;
                }
            }
            if (Utils.FileExists(AppDomain.CurrentDomain.BaseDirectory + "/cache/topic/" + fid.ToString() + ".xml"))
            {
                File.Delete(AppDomain.CurrentDomain.BaseDirectory + "/cache/topic/" + fid.ToString() + ".xml");
            }
            return -1;
        }

        /// <summary>
        /// 重新生成置顶主题
        /// </summary>
        /// <param name="fid">主题ID</param>
        /// <param name="intValue"></param>
        /// <returns></returns>
        public static int ResetTopTopicList(int fid)
        {
            DataSet ds = DatabaseProvider.GetInstance().GetTopTopicList(fid);
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataTable topTable = DatabaseProvider.GetInstance().GetShortForums();
                    int[] fidIndex = null;
                    if (topTable != null)
                    {
                        if (topTable.Rows.Count > 0)
                        {
                            fidIndex = new int[Utils.StrToInt(topTable.Rows[0]["fid"], 0) + 1];
                            int index = 0;
                            foreach (DataRow dr in topTable.Rows)
                            {
                                fidIndex[Utils.StrToInt(dr["fid"], 0)] = index;
                                index++;
                            }
                        }
                    }

                    ds.DataSetName = "topics";
                    ds.Tables[0].TableName = "topic";
                    int tidCount = 0;
                    int tid0Count = 0;
                    int tid1Count = 0;
                    int tid2Count = 0;
                    int tid3Count = 0;

                    StringBuilder sbTop3 = new StringBuilder();
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        if (Utils.StrToInt(dr["displayorder"], 0) == 3)
                        {
                            if (sbTop3.Length > 0)
                            {
                                sbTop3.Append(",");
                            }

                            if (fidIndex != null && fidIndex.Length >= Utils.StrToInt(dr["fid"], 0))
                            {
                                sbTop3.Append(dr["tid"]);
                                tidCount++;
                                topTable.Rows[fidIndex[Utils.StrToInt(dr["fid"], 0)]]["tid3count"] = Utils.StrToInt(topTable.Rows[fidIndex[Utils.StrToInt(dr["fid"], 0)]]["tid3count"], 0) + 1;
                            }

                        }
                        else
                        {
                            if (fidIndex != null && fidIndex.Length >= Utils.StrToInt(dr["fid"], 0))
                            {
                                if (Utils.StrToInt(dr["displayorder"], 0) != 2)
                                {
                                    topTable.Rows[fidIndex[Utils.StrToInt(dr["fid"], 0)]]["tidlist"] = topTable.Rows[fidIndex[Utils.StrToInt(dr["fid"], 0)]]["tidlist"].ToString() + "," + dr["tid"].ToString();
                                    topTable.Rows[fidIndex[Utils.StrToInt(dr["fid"], 0)]]["tidcount"] = Utils.StrToInt(topTable.Rows[fidIndex[Utils.StrToInt(dr["fid"], 0)]]["tidcount"], 0) + 1;
                                }
                                else
                                {
                                    topTable.Rows[fidIndex[Utils.StrToInt(dr["fid"], 0)]]["tid2list"] = topTable.Rows[fidIndex[Utils.StrToInt(dr["fid"], 0)]]["tid2list"].ToString() + "," + dr["tid"].ToString();
                                    topTable.Rows[fidIndex[Utils.StrToInt(dr["fid"], 0)]]["tid2count"] = Utils.StrToInt(topTable.Rows[fidIndex[Utils.StrToInt(dr["fid"], 0)]]["tid2count"], 0) + 1;
                                }
                            }
                        }
                    }

                    if (topTable != null)
                    {
                        if (topTable.Rows.Count > 0)
                        {
                            foreach (DataRow dr in topTable.Rows)
                            {
                                dr["temptidlist"] = sbTop3.ToString() + dr["tidlist"].ToString() + dr["tid2list"].ToString();

                                tid1Count = Utils.StrToInt(dr["tidcount"], 0);
                                tid2Count = Utils.StrToInt(dr["tid2count"], 0);
                                tid3Count = Utils.StrToInt(dr["tid3count"], 0);

                                tid0Count = tid1Count + tid2Count + tid3Count;

                                dr["tidcount"] = tid1Count + tidCount + Utils.StrToInt(dr["tid2count"], 0);

                                string filterexpress = DatabaseProvider.GetInstance().ResetTopTopicListSql(Utils.StrToInt(dr["layer"], 0), dr["fid"].ToString().Trim(), dr["parentidlist"].ToString().Trim());

                                //switch (Utils.StrToInt(dr["layer"], 0))
                                //{
                                //    case 0:
                                //        filterexpress = "[fid]<>" + dr["fid"].ToString().Trim() + " AND (',' + TRIM([parentidlist]) + ',' LIKE '%," + dr["fid"].ToString().Trim() + ",%')";
                                //        break;
                                //    case 1:
                                //        filterexpress = dr["parentidlist"].ToString().Trim();
                                //        filterexpress = "[fid]<>" + dr["fid"].ToString().Trim() + " AND ([fid]=" + filterexpress + " OR (',' + TRIM([parentidlist]) + ',' LIKE '%," + dr["parentidlist"].ToString().Trim() + ",%'))";
                                //        break;
                                //    default:
                                //        filterexpress = dr["parentidlist"].ToString().Trim();
                                //        filterexpress = Utils.CutString(filterexpress, 0, filterexpress.IndexOf(","));
                                //        filterexpress = "[fid]<>" + dr["fid"].ToString().Trim() + " AND ([fid]=" + filterexpress + " OR (',' + TRIM([parentidlist]) + ',' LIKE '%," + filterexpress + ",%'))";
                                //        break;
                                //}

                                foreach (DataRow drTemp in topTable.Select(filterexpress))
                                {
                                    if (!drTemp["tid2list"].ToString().Equals(""))
                                    {
                                        dr["temptidlist"] = dr["temptidlist"].ToString() + drTemp["tid2list"].ToString();
                                        dr["tidcount"] = Utils.StrToInt(drTemp["tid2count"], 0) + Utils.StrToInt(dr["tidcount"], 0);
                                        tid2Count = tid2Count + Utils.StrToInt(drTemp["tid2count"], 0);
                                    }
                                }

                                tid0Count = Utils.StrToInt(dr["tidcount"], 0) - tid0Count;

                                if (ds.Tables.Count == 1)
                                {
                                    ds.Tables.Add();
                                    ds.Tables[1].Columns.Add("tid", Type.GetType("System.String"));
                                    ds.Tables[1].Columns.Add("tidCount", Type.GetType("System.Int32"));
                                    ds.Tables[1].Columns.Add("tid0Count", Type.GetType("System.Int32"));
                                    ds.Tables[1].Columns.Add("tid1Count", Type.GetType("System.Int32"));
                                    ds.Tables[1].Columns.Add("tid2Count", Type.GetType("System.Int32"));
                                    ds.Tables[1].Columns.Add("tid3Count", Type.GetType("System.Int32"));
                                    ds.Tables[1].TableName = "fidtopic";
                                    DataRow dr1 = ds.Tables[1].NewRow();
                                    dr1["tid"] = dr["temptidlist"];
                                    dr1["tidCount"] = dr["tidcount"];
                                    dr1["tid0Count"] = tid0Count;
                                    dr1["tid1Count"] = tid1Count;
                                    dr1["tid2Count"] = tid2Count;
                                    dr1["tid3Count"] = tidCount;
                                    ds.Tables[1].Rows.Add(dr1);
                                }
                                else
                                {
                                    ds.Tables[1].Rows[0]["tid"] = dr["temptidlist"];
                                    ds.Tables[1].Rows[0]["tidCount"] = dr["tidcount"];
                                    ds.Tables[1].Rows[0]["tid0Count"] = tid0Count;
                                    ds.Tables[1].Rows[0]["tid1Count"] = tid1Count;
                                    ds.Tables[1].Rows[0]["tid2Count"] = tid2Count;
                                    ds.Tables[1].Rows[0]["tid3Count"] = tidCount;
                                }

                                DataSet tempDS = new DataSet();
                                tempDS.Tables.Add(ds.Tables[1].Copy());
                                tempDS.DataSetName = "topics";
                                tempDS.Tables[0].TableName = "topic";
                                if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "/cache/topic/"))
                                    Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "/cache/topic/");
                                tempDS.WriteXml(AppDomain.CurrentDomain.BaseDirectory + "/cache/topic/" + dr["fid"].ToString() + ".xml", XmlWriteMode.WriteSchema);
                                tempDS.Clear();
                                tempDS.Dispose();

                                //
                                //									'要改的.
                                //
                                //
                                //									ds.Tables.Remove("topic");
                                //									
                                //									ds.Tables[0].TableName="topic";
                                //									ds.WriteXml(@Utils.GetMapPath(BaseConfigFactory.GetForumPath + "topic/" + fid.ToString() + ".xml"),System.Data.XmlWriteMode.WriteSchema);
                            }
                        }
                    }


                    topTable.Dispose();
                    ds.Clear();
                    ds.Dispose();
                    return 1;
                }
            }
            return 0;
        }
        /// <summary>
        /// 将主题高亮显示
        /// </summary>
        /// <param name="topiclist">要设置的主题列表</param>
        /// <param name="intValue">高亮样式及颜色( 0 为解除高亮显示)</param>
        /// <returns>更新主题个数</returns>
        public static int SetHighlight(string topiclist, string intValue)
        {
            return SetTopicStatus(topiclist, "highlight", intValue);
        }

        /// <summary>
        /// 根据得到给定主题的用户列表
        /// </summary>
        /// <param name="topiclist">主题列表</param>
        /// <param name="op">操作源(0:精华,1:删除)</param>
        /// <returns>用户列表</returns>
        private static string GetUserListWithTopiclist(string topiclist, int op)
        {
            if (!Utils.IsNumericArray(topiclist.Split(',')))
            {
                return "";
            }
            StringBuilder useridlist = new StringBuilder();

            GeneralConfigInfo configinfo = GeneralConfigs.GetConfig();
            IDataReader reader = null;
            if (op == 1)
            {
                if (configinfo.Losslessdel != 0)
                {
                    reader = DatabaseProvider.GetInstance().GetUserListWithTopicList(topiclist, configinfo.Losslessdel);
                }
                else
                {
                    reader = DatabaseProvider.GetInstance().GetUserListWithTopicList(topiclist);
                }
            }
            else
            {
                reader = DatabaseProvider.GetInstance().GetUserListWithTopicList(topiclist);
            }

            if (reader != null)
            {
                while (reader.Read())
                {
                    if (!useridlist.ToString().Equals(""))
                    {
                        useridlist.Append(",");
                    }
                    useridlist.Append(reader["posterid"].ToString());

                }
                reader.Close();
            }
            return useridlist.ToString();

        }



        /// <summary>
        /// 将主题设置精华/解除精华
        /// </summary>
        /// <param name="topiclist">要设置的主题列表</param>
        /// <param name="intValue">精华级别( 0 为解除精华)</param>
        /// <returns>更新主题个数</returns>
        public static int SetDigest(string topiclist, short intValue)
        {
            int mount = SetTopicStatus(topiclist, "digest", intValue);
            string useridlist = GetUserListWithTopiclist(topiclist, 0);
            if (mount > 0)
            {
                Users.UpdateUserDigest(useridlist);
                if (intValue > 0 && !useridlist.Equals(""))
                {
                    UserCredits.UpdateUserCreditsByDigest(useridlist, mount);
                }
            }
            return mount;
        }

        /// <summary>
        /// 将主题设置关闭/打开
        /// </summary>
        /// <param name="topiclist">要设置的主题列表</param>
        /// <param name="intValue">关闭/打开标志( 0 为打开,1 为关闭)</param>
        /// <returns>更新主题个数</returns>
        public static int SetClose(string topiclist, short intValue)
        {
            if (!Utils.IsNumericArray(topiclist.Split(',')))
            {
                return -1;
            }

            return DatabaseProvider.GetInstance().SetTopicClose(topiclist, intValue);
        }



        /// <summary>
        /// 获得主题指定字段的属性值
        /// </summary>
        /// <param name="topiclist">主题列表</param>
        /// <param name="field">要获得值的字段</param>
        /// <returns>主题指定字段的状态</returns>
        public static int GetTopicStatus(string topiclist, string field)
        {
            if ((",displayorder,digest,").IndexOf("," + field.ToLower().Trim() + ",") < 0)
            {
                return -1;
            }

            if (!Utils.IsNumericArray(topiclist.Split(',')))
            {
                return -1;
            }

            return DatabaseProvider.GetInstance().GetTopicStatus(topiclist, field);
        }




        /// <summary>
        /// 获得主题置顶状态
        /// </summary>
        /// <param name="topiclist">主题列表</param>
        /// <returns>置顶状态(单个主题返回真实状态,多个主题返回状态值累计)</returns>
        public static int GetDisplayorder(string topiclist)
        {
            return GetTopicStatus(topiclist, "displayorder");
        }

        /// <summary>
        /// 获得高亮样式及颜色
        /// </summary>
        /// <param name="topiclist">主题列表</param>
        /// <returns>高亮样式及颜色</returns>
        public static int GetHighlight(string topiclist)
        {
            return 0;
        }


        /// <summary>
        /// 获得主题精华状态
        /// </summary>
        /// <param name="topiclist">主题列表</param>
        /// <returns>精华状态(单个主题返回真实状态,多个主题返回状态值累计)</returns>
        public static int GetDigest(string topiclist)
        {
            return GetTopicStatus(topiclist, "digest");
        }


        //		/// <summary>
        //		/// 获得主题关闭状态
        //		/// </summary>
        //		/// <param name="topiclist">主题列表</param>
        //		/// <returns>精华状态(单个主题返回真实状态,多个主题返回状态值累计)</returns>
        //		public static int GetClose(string topiclist)
        //		{
        //			return GetTopicStatus(topiclist,"closed");
        //		}


        /// <summary>
        /// 在数据库中删除指定主题
        /// </summary>
        /// <param name="topiclist">主题列表</param>
        /// <param name="subtractCredits">是否减少用户积分(0不减少,1减少)</param>
        /// <returns>删除个数</returns>
        public static int DeleteTopics(string topiclist, int subtractCredits, bool reserveAttach)
        {
            if (!Utils.IsNumericArray(topiclist.Split(',')))
            {
                return -1;
            }


            GeneralConfigInfo configinfo = GeneralConfigs.GetConfig();


            DataTable dt = Topics.GetTopicList(topiclist);
            if (dt == null)
            {
                return -1;
            }
            foreach (DataRow dr in dt.Rows)
            {
                if (Utils.StrToInt(dr["digest"], 0) > 0)
                {
                    UserCredits.UpdateUserCreditsByDigest(Utils.StrToInt(dr["posterid"], 0), -1);
                }

            }


            dt = Posts.GetPostList(topiclist);
            if (dt != null)
            {
                int i = 0;
                int[] tuidlist = new int[dt.Rows.Count];
                int[] auidlist = new int[dt.Rows.Count];
                //int fid = -1;
                foreach (DataRow dr in dt.Rows)
                {
                    //fid = Utils.StrToInt(dr["fid"].ToString(), -1);
                    if (Utils.StrDateDiffHours(dr["postdatetime"].ToString(), configinfo.Losslessdel * 24) < 0)
                    {
                        if (Utils.StrToInt(dr["layer"], 0) == 0)
                        {
                            tuidlist[i] = Utils.StrToInt(dr["posterid"], 0);
                        }
                        else
                        {
                            auidlist[i] = Attachments.GetAttachmentCountByPid(Utils.StrToInt(dr["pid"], 0));
                        }
                    }
                    else
                    {
                        tuidlist[i] = 0;
                        auidlist[i] = 0;
                    }
                }
                //DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigFactory.GetTablePrefix + "forums] SET [topics]=[topics]-" + topiclist.Split(',').Length.ToString() + ", [posts]=[posts]-" + dt.Rows.Count.ToString() + " WHERE [fid]=" + fid.ToString());
                UserCredits.UpdateUserCreditsByDeleteTopic(tuidlist, auidlist, -1);

            }

            int reval = 0;
            foreach (string posttableid in Posts.GetPostTableIDList(topiclist))
            {
                reval = DatabaseProvider.GetInstance().DeleteTopicByTidList(topiclist, posttableid,true);
            }
            if (reval > 0 && !reserveAttach)
            {
                Attachments.DeleteAttachmentByTid(topiclist);
            }
            return reval;


        }


        public static int DeleteTopicsWithoutChangingCredits(string topiclist, bool reserveAttach)
        {
            if (!Utils.IsNumericArray(topiclist.Split(',')))
            {
                return -1;
            }
            int reval = -1;
            foreach (string posttableid in Posts.GetPostTableIDList(topiclist))
            {
                reval = DatabaseProvider.GetInstance().DeleteTopicByTidList(topiclist, posttableid,false);
            }
            if (reval > 0 && !reserveAttach)
            {
                Attachments.DeleteAttachmentByTid(topiclist);
            }
            return reval;
        }

        /// <summary>
        /// 在数据库中删除指定主题
        /// </summary>
        /// <param name="topiclist">主题列表</param>
        /// <returns>删除个数</returns>
        public static int DeleteTopics(string topiclist, bool reserveAttach)
        {
            return DeleteTopics(topiclist, 1, reserveAttach);
        }

        /// <summary>
        /// 在删除指定的主题
        /// </summary>
        /// <param name="topiclist">主题列表</param>
        /// <param name="toDustbin">指定主题删除形式(0：直接从数据库中删除,并删除与之关联的信息  1：只将其从论坛列表中删除(将displayorder字段置为-1)将其放入回收站中</param>
        /// <returns>删除个数</returns>
        public static int DeleteTopics(string topiclist, byte toDustbin, bool reserveAttach)
        {
            return toDustbin == 0 ? DeleteTopics(topiclist, reserveAttach) : SetTopicStatus(topiclist, "displayorder", -1);
        }


        /// <summary>
        /// 恢复回收站中的主题。
        /// </summary>
        /// <param name="topiclist">主题列表</param>
        /// <returns>恢复个数</returns>
        public static int RestoreTopics(string topiclist)
        {
            return SetTopicStatus(topiclist, "displayorder", 0);
        }


        /// <summary>
        /// 移动主题到指定版块
        /// </summary>
        /// <param name="topiclist">要移动的主题列表</param>
        /// <param name="fid">转到的版块ID</param>
        /// <param name="savelink">是否在原版块保留连接</param>
        /// <returns>更新记录数</returns>
        public static int MoveTopics(string topiclist, int fid, int oldfid, bool savelink)
        {
            if (topiclist.Trim() == "")
            {
                return -1;
            }
            string[] tidlist = Utils.SplitString(topiclist, ",");
            int intDelTidCount = 0;
            //int intMovePostCount = 0;
            foreach (string tid in tidlist)
            {
                if (!Utils.IsNumeric(tid))
                {
                    return -1;
                }
            }
            intDelTidCount += DatabaseProvider.GetInstance().DeleteClosedTopics(fid, topiclist);


            //转移帖子
            //
            MoveTopics(topiclist, fid, oldfid);

            //如果保存链接则复制一条记录到原版块
            if (savelink)
            {
                if (DatabaseProvider.GetInstance().CopyTopicLink(oldfid, topiclist) <= 0)
                {
                    return -2;
                }

                AdminForumStats.ReSetFourmTopicAPost(oldfid);
                Forums.SetRealCurrentTopics(oldfid);

                //				intMovePostCount = tidlist.Length + Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, "SELECT SUM([replies]) AS [totalreplies] FROM [" + BaseConfigFactory.GetTablePrefix + "topics] WHERE [tid] IN (" + topiclist + ")"), 0);

                //				int todaypost = 0;

                //				foreach (string tid in PostFactory.GetPostTableIDList(topiclist))
                //				{
                //					string posttable = PostFactory.GetPostTableName(Utils.StrToInt(tid, 0));
                //					todaypost = todaypost + Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, "SELECT COUNT([pid]) c FROM [" + posttable + "] WHERE [tid] IN (" + topiclist + ") AND DATEDIFF(day,postdatetime,getdate())=0"), 0);
                //
                //					///2.将原主题所对应的帖子转到新主题下。
                //					//sql = "UPDATE [" + posttable + "] SET [tid]=(SELECT [tid] AS [c] FROM [" + BaseConfigFactory.GetTablePrefix + "topics] [d] WHERE [d].[closed]=[" + posttable + "].[tid]) WHERE [tid] IN (" + topiclist + ")";
                //					//DbHelper.ExecuteNonQuery(CommandType.Text, sql);
                //				}


                ///更新论坛的主题数等
                //				StringBuilder sb = new StringBuilder();
                //				sb.Append("UPDATE [" + BaseConfigFactory.GetTablePrefix + "forums] SET [topics]=[topics]-");
                //				sb.Append(tidlist.Length.ToString());
                //				sb.Append(",[posts]=[posts]-");
                //				sb.Append(intMovePostCount.ToString());
                //				sb.Append(",[todayposts]=[todayposts]-");
                //				sb.Append(todaypost.ToString());
                //				sb.Append("WHERE [fid] =");
                //				sb.Append(tidlist.Length.ToString());
                //
                //
                //				///更新论坛的主题数等

                //				sb.Append(";UPDATE [" + BaseConfigFactory.GetTablePrefix + "forums] SET [topics]=[topics]+");
                //				sb.Append((tidlist.Length - intDelTidCount).ToString());
                //				sb.Append(",[posts]=[posts]+");
                //				sb.Append(intMovePostCount.ToString());
                //				sb.Append(",[todayposts]=[todayposts]+");
                //				sb.Append(todaypost.ToString());
                //				sb.Append("WHERE [fid] =");
                //				sb.Append(fid.ToString());


                //DbHelper.ExecuteNonQuery(CommandType.Text, sql);

                //sb.Append(";UPDATE [" + BaseConfigFactory.GetTablePrefix + "polls] SET [tid]=(SELECT [tid] AS [c] FROM [" + BaseConfigFactory.GetTablePrefix + "topics] WHERE [" + BaseConfigFactory.GetTablePrefix + "topics].[closed]=[" + BaseConfigFactory.GetTablePrefix + "polls].[tid]) WHERE [tid] IN (" + topiclist + ")");
                ///3.将原记录的closed改为新拷贝记录的tid,据新记录的closed确定
                //sb.Append(";UPDATE [" + BaseConfigFactory.GetTablePrefix + "topics] SET [closed]=(SELECT [tid] AS [c] FROM [" + BaseConfigFactory.GetTablePrefix + "topics] [d] WHERE [d].[closed]=[" + BaseConfigFactory.GetTablePrefix + "topics].[tid]) WHERE [tid] IN (" + topiclist + ")");
                //DbHelper.ExecuteNonQuery(CommandType.Text, sql);


                ///4.将新的记录的closed的值设置为 0
                //sb.Append(";UPDATE [" + BaseConfigFactory.GetTablePrefix + "topics] SET [closed]=0 WHERE [fid]=@fid AND [closed] IN (" + topiclist + ")");

                //将原来的fid改为新的fid
                //				sb.Append(";UPDATE [" + BaseConfigFactory.GetTablePrefix + "topics] SET [fid]=@fid WHERE [tid] IN (" + topiclist + ")");
                //
                //
                //				DbHelper.ExecuteNonQuery(CommandType.Text, sb.ToString(),  parms);
                //

            }
            //
            //			else
            //			{
            //				///用户执行主题直接转移
            //				return MoveTopics(topiclist, fid, oldfid);
            //
            //			}
            return 1;
        }

        /// <summary>
        /// 移动主题到指定版块
        /// </summary>
        /// <param name="topiclist">要移动的主题列表</param>
        /// <param name="fid">转到的版块ID</param>
        /// <returns>更新记录数</returns>
        public static int MoveTopics(string topiclist, int fid, int oldfid)
        {
            if (!Utils.IsNumericArray(topiclist.Split(',')))
            {
                return -1;
            }
            //更新帖子
            foreach (string tid in topiclist.Split(','))
            {
                string posttable = Posts.GetPostTableName(Utils.StrToInt(tid, 0));
                DatabaseProvider.GetInstance().UpdatePost(topiclist, fid, posttable);
            }

            //更新主题
            int reval = DatabaseProvider.GetInstance().UpdateTopic(topiclist, fid);
            if (reval > 0)
            {
                AdminForumStats.ReSetFourmTopicAPost(fid);
                AdminForumStats.ReSetFourmTopicAPost(oldfid);
                Forums.SetRealCurrentTopics(fid);
                Forums.SetRealCurrentTopics(oldfid);

            }

            //生成置顶帖
            ResetTopTopicList(fid);
            ResetTopTopicList(oldfid);
            return reval;
        }



        /// <summary>
        /// 复制主题
        /// </summary>
        /// <param name="topiclist">主题id列表</param>
        /// <param name="fid">目标版块id</param>
        /// <returns>更新记录数</returns>
        public static int CopyTopics(string topiclist, int fid)
        {
            int tid;
            if (!Utils.IsNumericArray(topiclist.Split(',')))
            {
                return -1;
            }

            int reval = 0;
            TopicInfo topicinfo = null;
            foreach (string topicid in topiclist.Split(','))
            {
                topicinfo = Topics.GetTopicInfo(Utils.StrToInt(topicid, 0));
                if (topicinfo != null)
                {
                    topicinfo.Fid = fid;
                    topicinfo.Readperm = 0;
                    topicinfo.Price = 0;
                    topicinfo.Postdatetime = Utils.GetDateTime();
                    topicinfo.Lastpost = Utils.GetDateTime();
                    topicinfo.Lastposter = Utils.GetDateTime();
                    topicinfo.Views = 0;
                    topicinfo.Replies = 0;
                    topicinfo.Displayorder = 0;
                    topicinfo.Highlight = "";
                    topicinfo.Digest = 0;
                    topicinfo.Rate = 0;
                    topicinfo.Hide = 0;
                    topicinfo.Special = 0;
                    topicinfo.Attachment = 0;
                    topicinfo.Moderated = 0;
                    topicinfo.Closed = 0;
                    tid = Topics.CreateTopic(topicinfo);
                    if (tid > 0)
                    {
                        PostInfo postinfo = Posts.GetPostInfo(tid, Posts.GetFirstPostId(Utils.StrToInt(topicid, 0)));
                        postinfo.Fid = topicinfo.Fid;
                        postinfo.Tid = tid;
                        postinfo.Parentid = 0;
                        postinfo.Layer = 0;
                        postinfo.Postdatetime = Utils.GetDateTime();
                        postinfo.Invisible = 0;
                        postinfo.Attachment = 0;
                        postinfo.Rate = 0;
                        postinfo.Ratetimes = 0;
                        postinfo.Message = UBB.ClearAttachUBB(postinfo.Message);
                        postinfo.Topictitle = topicinfo.Title;
                        int postid = Posts.CreatePost(postinfo);
                        if (postid > 0)
                        {
                            reval++;
                        }
                    }
                }
            }

            return reval;
        }


        /// <summary>
        /// 分割主题
        /// </summary>
        /// <param name="postidlist">帖子id列表</param>
        /// <param name="subject">主题</param>
        /// <param name="topicidlist">主题id列表</param>
        /// <returns>更新记录数</returns>
        public static int SplitTopics(string postidlist, string subject, string topicidlist)
        {
            int tid = 0;
            //验证要分割的帖子是否为有效PID号
            string[] postid = postidlist.Split(',');
            if (postid.Length <= 0)
            {
                return -1;
            }
            else
            {
                if (!Utils.IsNumericArray(postid))
                {
                    return -1;
                }
            }



            int topicid = Utils.StrToInt(topicidlist, 0); //将要被分割主题的tid	
            TopicInfo topicinfo = Topics.GetTopicInfo(topicid);
            PostInfo postinfo = Posts.GetPostInfo(topicid, Utils.StrToInt(postid[postid.Length - 1], 0));

            PostInfo firstpostinfo = Posts.GetPostInfo(topicid, Utils.StrToInt(postid[0], 0));

            topicinfo.Poster = firstpostinfo.Poster;
            topicinfo.Posterid = firstpostinfo.Posterid;
            topicinfo.Postdatetime = Utils.GetDateTime();
            topicinfo.Displayorder = 0;
            topicinfo.Highlight = "";
            topicinfo.Digest = 0;
            topicinfo.Rate = 0;
            topicinfo.Hide = 0;
            topicinfo.Special = 0;
            topicinfo.Attachment = 0;
            topicinfo.Moderated = 0;
            topicinfo.Closed = 0;
            topicinfo.Views = 0;
            if (topicinfo.Lastpostid != Utils.StrToInt(postid[postid.Length - 1], 0))
            {


                topicinfo.Lastpostid = Utils.StrToInt(postid[postid.Length - 1], 0);
                topicinfo.Lastposterid = postinfo.Posterid;
                topicinfo.Lastpost = postinfo.Postdatetime;
                topicinfo.Lastposter = postinfo.Poster;

                topicinfo.Replies = postid.Length - 1;

                topicinfo.Title = Utils.HtmlEncode(subject);
                tid = Topics.CreateTopic(topicinfo);
                DatabaseProvider.GetInstance().UpdatePostTid(postidlist, tid, Posts.GetPostTableID(tid));
                DatabaseProvider.GetInstance().SetPrimaryPost(subject, tid, postid, Posts.GetPostTableID(tid));


            }

            else
            {
                string list = "";
                //DbHelper.ExecuteDataset(CommandType.Text, "select * from dnt_posts1 where pid not in (" + postidlist + ") and tid=" + topicid + " order by pid desc").Tables[0];

                DataTable dt = DatabaseProvider.GetInstance().GetOtherPostId(postidlist, topicid, int.Parse(Posts.GetPostTableID()));
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    list = list + dt.Rows[i]["pid"].ToString() + ",";

                }

                list = list.Substring(0, list.Length - 1);


                topicinfo.Lastpostid = Utils.StrToInt(dt.Rows[0]["pid"].ToString(), 0);
                topicinfo.Lastposterid = Utils.StrToInt(dt.Rows[0]["Posterid"].ToString(), 0);
                topicinfo.Lastpost = dt.Rows[0]["Postdatetime"].ToString();
                topicinfo.Lastposter = dt.Rows[0]["Poster"].ToString();

                topicinfo.Replies = dt.Rows.Count - 1;


                tid = Topics.CreateTopic(topicinfo);



                DatabaseProvider.GetInstance().UpdatePostTid(list, tid, Posts.GetPostTableID(tid));

                DatabaseProvider.GetInstance().SetPrimaryPost(subject, topicinfo.Tid, postid, Posts.GetPostTableID(tid));
                Topics.UpdateTopicTitle(topicinfo.Tid, subject);



            }


            //DatabaseProvider.GetInstance().UpdatePostTid(postidlist, tid, Posts.GetPostTableID(tid));


            //DatabaseProvider.GetInstance().UpdatePostTid(postidlist, tid, Posts.GetPostTableID(tid));
            //DatabaseProvider.GetInstance().SetPrimaryPost(subject, tid, postid, Posts.GetPostTableID(tid));
            Topics.UpdateTopicReplies(topicid);
            Topics.UpdateTopicReplies(tid);

            return tid;








            //获得要被分割主题的信息
            //TopicInfo __topicinfo = Topics.GetTopicInfo(topicid);

            ////获得新主题的最后一个帖子的信息

            //PostInfo __postinfo = Posts.GetPostInfo(topicid, Utils.StrToInt(postid[postid.Length - 1], 0));

            //PostInfo postinfo = Posts.GetPostInfo(topicid, Utils.StrToInt(postid[0], 0));

            //if (__topicinfo.Lastpostid != Utils.StrToInt(postid[postid.Length - 1], 0))
            //{

            //     __topicinfo.Lastpostid = Utils.StrToInt(postid[postid.Length - 1], 0);

            //}



            //if (__topicinfo != null)
            //{
            //    __topicinfo.Title = Utils.HtmlEncode(subject);
            //    __topicinfo.Poster = postinfo.Poster;
            //    __topicinfo.Posterid = postinfo.Posterid;
            //    __topicinfo.Postdatetime = Utils.GetDateTime();
            //    //__topicinfo.Lastpostid = -1;
            //    __topicinfo.Lastposterid = __postinfo.Posterid;
            //    __topicinfo.Lastpost = __postinfo.Postdatetime;
            //    __topicinfo.Lastposter = __postinfo.Poster;
            //    __topicinfo.Views = 0;
            //    __topicinfo.Replies = postid.Length - 1;
            //    __topicinfo.Displayorder = 0;
            //    __topicinfo.Highlight = "";
            //    __topicinfo.Digest = 0;
            //    __topicinfo.Rate = 0;
            //    __topicinfo.Hide = 0;
            //    __topicinfo.Poll = 0;
            //    __topicinfo.Attachment = 0;
            //    __topicinfo.Moderated = 0;
            //    __topicinfo.Closed = 0;
            //}
            //创建新主题
            //tid = Topics.CreateTopic(__topicinfo);
            ////tid = DatabaseProvider.GetInstance().GetMaxTopicId();

            //if (tid > 0)
            //{

            //    //将分割出来的帖子的主题ID改为新主题ID
            //    DatabaseProvider.GetInstance().UpdatePostTid(postidlist, tid, Posts.GetPostTableID(tid));

            //    //将分割出来的帖子的第一个帖子设置为新主题的主帖
            //    DatabaseProvider.GetInstance().SetPrimaryPost(subject, tid, postid, Posts.GetPostTableID(tid));


            //    //更新原主题(被分割的主题)的信息
            //    int Replies = Posts.GetPostCount(topicid);
            //    if (Replies > 0)
            //    {
            //        Replies = Replies - 1;
            //    }
            //    DataTable dt = Posts.GetLastPostByTid(topicid);
            //    int lastpostid = 0;
            //    int lastposterid = 0;
            //    string lastposter = "";
            //    DateTime lastpost = DateTime.Parse(Utils.GetDateTime());

            //    if (dt != null)
            //    {
            //        if (dt.Rows.Count > 0)
            //        {
            //            if (Utils.StrToInt(dt.Rows[0]["layer"].ToString(), 0) == 0)
            //            {
            //                Replies = 0;
            //            }

            //            DataRow dr = dt.Rows[0];
            //            lastpostid = Utils.StrToInt(dr["pid"], 0);
            //            lastposterid = Utils.StrToInt(dr["posterid"], 0);
            //            lastpost = DateTime.Parse(dr["Postdatetime"].ToString());
            //            lastposter = dr["poster"].ToString();

            //        }
            //    }

            //    //if (__topicinfo != null)
            //    //{
            //    //    __topicinfo.Title = Utils.HtmlEncode(subject);
            //    //    __topicinfo.Poster = postinfo.Poster;
            //    //    __topicinfo.Posterid = postinfo.Posterid;
            //    //    __topicinfo.Postdatetime = Utils.GetDateTime();
            //    //    if (Utils.StrToInt(postid[postid.Length - 1], 0) == lastpostid)
            //    //    {
            //    //        __topicinfo.Lastpostid = lastpostid;
            //    //    }
            //    //    else
            //    //    {
            //    //        __topicinfo.Lastpostid = Utils.StrToInt(postid[postid.Length - 1], 0);
            //    //    }
            //    //        __topicinfo.Lastposterid = __postinfo.Posterid;
            //    //    __topicinfo.Lastpost = __postinfo.Postdatetime;
            //    //    __topicinfo.Lastposter = __postinfo.Poster;
            //    //    __topicinfo.Views = 0;
            //    //    __topicinfo.Replies = postid.Length - 1;
            //    //    __topicinfo.Displayorder = 0;
            //    //    __topicinfo.Highlight = "";
            //    //    __topicinfo.Digest = 0;
            //    //    __topicinfo.Rate = 0;
            //    //    __topicinfo.Hide = 0;
            //    //    __topicinfo.Poll = 0;
            //    //    __topicinfo.Attachment = 0;
            //    //    __topicinfo.Moderated = 0;
            //    //    __topicinfo.Closed = 0;
            //    //}
            //    ////创建新主题
            //    //Topics.CreateTopic(__topicinfo);
            //    DatabaseProvider.GetInstance().SetNewTopicProperty(topicid, Replies, lastpostid, lastposterid, lastposter, lastpost);
            //    //DbHelper.ExecuteNonQuery("UPDATE `" + BaseConfigs.GetTablePrefix + "topics` SET `lastpostid`=" + Utils.StrToInt(postid[postid.Length - 1], 0)+" where tid="+tid);
            //    //DatabaseProvider.GetInstance().SetNewLastPid(Utils.StrToInt(postid[postid.Length - 1], 0), tid);
            //    /*
            //     * 
            //     * 只选择layer为1的帖子时的处理方法。
            //    int parentid = 0;
            //    int layerPos = 0;

            //    for (int i=0;i<postid.Length;i++)
            //    {

            //        __postinfo = PostFactory.GetPostInfo(Utils.StrToInt(postid[i],0));
            //        if (i==0)
            //        {
            //            parentid = __postinfo.Pid;
            //            layerPos = 1;
            //        }
            //        else
            //        {
            //            parentid = __postinfo.Parentid;
            //            layerPos = 0;
            //        }

            //        DbParameter[] parms = 
            //                                {
            //                                    DbHelper.MakeInParam("@pid",(DbType)System.Data.SqlDbType.TinyInt,1,__postinfo.Pid),
            //                                    DbHelper.MakeInParam("@parentid",(DbType)System.Data.SqlDbType.TinyInt,1,parentid),
            //                                    DbHelper.MakeInParam("@layerpos",(DbType)System.Data.SqlDbType.TinyInt,1,layerPos),
            //                                    DbHelper.MakeInParam("@tid",(DbType)System.Data.SqlDbType.TinyInt,1,tid)
            //                                };
            //        reval=DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE ["+BaseConfigFactory.GetTablePrefix+"posts + PostFactory.GetPostTableID(tid) + "] SET [layer] = [layer] - @layerpos, [tid] = @tid,[parentid] = @parentid END WHERE [parentid] = @parentid AND [layer] > 0",parms);
            //    }
            //     * 
            //    */
            //}


        }

        /// <summary>
        /// 合并主题
        /// </summary>
        /// <param name="topiclist">主题id列表</param>
        /// <param name="othertid">被合并ip</param>
        /// <returns>更新记录数</returns>
        public static int MerrgeTopics(string topiclist, int othertid)
        {
            int tid = Utils.StrToInt(topiclist, 0);
            int reval = 0;
            //获得要被合并的主题的信息
            TopicInfo topicinfo = Topics.GetTopicInfo(othertid);
            TopicInfo topicinfo_new = Topics.GetTopicInfo(tid);

            if (topicinfo.Lastpostid == 0)
            {
                DatabaseProvider.GetInstance().UpdateTopicLastPosterId(topicinfo.Tid);
            }
            if (topicinfo_new.Lastpostid == 0)
            {
                DatabaseProvider.GetInstance().UpdateTopicLastPosterId(topicinfo_new.Tid);
            }



            reval = DatabaseProvider.GetInstance().UpdatePostTidToAnotherTopic(othertid, tid, Posts.GetPostTableID(tid));
            reval = DatabaseProvider.GetInstance().UpdatePostTidToAnotherTopic(tid, tid, Posts.GetPostTableID(tid));
            //更新附件从属
            reval = DatabaseProvider.GetInstance().UpdateAttachmentTidToAnotherTopic(othertid, tid);

            reval = DatabaseProvider.GetInstance().DeleteTopic(othertid);

            if (topicinfo != null)
            {
                if (Utils.StrToInt(topicinfo_new.Lastpostid, 0) < Utils.StrToInt(topicinfo.Lastpostid, 0))
                {
                    reval = DatabaseProvider.GetInstance().UpdateTopic(tid, topicinfo);
                }
                else
                {
                    reval = DatabaseProvider.GetInstance().UpdateTopicReplies(tid, topicinfo.Replies);
                }
            }

            //更新主题信息
            PostInfo primarypost = Posts.GetPostInfo(tid, Posts.GetFirstPostId(tid));
            DatabaseProvider.GetInstance().SetPrimaryPost(primarypost.Title, tid, new string[] { primarypost.Pid.ToString() }, Posts.GetPostTableID(tid));
            Topics.UpdateTopic(tid, primarypost.Title, primarypost.Posterid, primarypost.Poster);

            return reval;
        }

        /// <summary>
        /// 修复主题列表
        /// </summary>
        /// <param name="topiclist">主题id列表</param>
        /// <returns>更新记录数</returns>
        public static int RepairTopicList(string topiclist)
        {
            if (!Utils.IsNumericArray(topiclist.Split(',')))
            {
                return 0;
            }

            int revalcount = 0;

            string[] idlist = Posts.GetPostTableIDList(topiclist);
            string[] tidlist = topiclist.Split(',');
            for (int i = 0; i < idlist.Length; i++)
            {
                string posttable = BaseConfigs.GetTablePrefix + "posts" + (Utils.StrToInt(idlist[i], 0));
                int reval = DatabaseProvider.GetInstance().RepairTopics(tidlist[i], posttable);
                if (reval > 0)
                {
                    revalcount = reval + revalcount;
                    Attachments.UpdateTopicAttachment(topiclist);
                }


            }



            //foreach (string tid in Posts.GetPostTableIDList(topiclist))
            //{
            //    string posttable = BaseConfigs.GetTablePrefix + "posts" + (Utils.StrToInt(tid, 0));


            //    int reval = DatabaseProvider.GetInstance().RepairTopics(topiclist, posttable);
            //    if (reval > 0)
            //    {
            //        revalcount = reval + revalcount;
            //        Attachments.UpdateTopicAttachment(topiclist);
            //    }
            //}
            return revalcount;
        }




        /// <summary>
        /// 根据得到给定帖子id的用户列表
        /// </summary>
        /// <param name="postlist">帖子列表</param>
        /// <returns>用户列表</returns>
        private static string GetUserListWithPostlist(int tid, string postlist)
        {
            if (!Utils.IsNumericArray(postlist.Split(',')))
            {
                return "";
            }
            StringBuilder useridlist = new StringBuilder();
            IDataReader reader = DatabaseProvider.GetInstance().GetUserListWithPostList(postlist, Posts.GetPostTableID(tid));
            if (reader != null)
            {
                while (reader.Read())
                {
                    if (!useridlist.ToString().Equals(""))
                    {
                        useridlist.Append(",");
                    }
                    useridlist.Append(reader["posterid"].ToString());

                }
                reader.Close();
            }
            return useridlist.ToString();

        }

        /// <summary>
        /// 给指定帖子评分
        /// </summary>
        /// <param name="postidlist">帖子列表</param>
        /// <param name="score">要加／减的分值列表</param>
        /// <param name="extcredits">对应的扩展积分列表</param>
        /// <returns>更新数量</returns>
        public static int RatePosts(int tid, string postidlist, string score, string extcredits, int userid, string username, string reason)
        {
            if (!Utils.IsNumericArray(Utils.SplitString(postidlist, ",")))
            {
                return 0;
            }
            float[] extcreditslist = new float[] { 0, 0, 0, 0, 0, 0, 0, 0 };
            string[] tmpScorelist = Utils.SplitString(score, ",");
            string[] tmpExtcreditslist = Utils.SplitString(extcredits, ",");
            int tempExtc = 0;
            string posttableid = Posts.GetPostTableID(tid);
            for (int i = 0; i < tmpExtcreditslist.Length; i++)
            {
                tempExtc = Utils.StrToInt(tmpExtcreditslist[i], -1);
                if (tempExtc > 0 && tempExtc < extcreditslist.Length)
                {
                    extcreditslist[tempExtc - 1] = Utils.StrToInt(tmpScorelist[i], 0);
                    //if (Utils.StrToInt(tmpScorelist[i], 0) != 0)
                    //{
                    AdminRateLogs.InsertLog(postidlist, userid, username, tempExtc, Utils.StrToInt(tmpScorelist[i], 0), reason);

                    //更新相应帖子的积分数
                    foreach (string pid in Utils.SplitString(postidlist, ","))
                    {
                        if (pid.Trim() != string.Empty)
                        {
                            SetPostRate(posttableid,
                                        Utils.StrToInt(pid, 0),
                                        Utils.StrToInt(tmpExtcreditslist[i], 0),
                                        Utils.StrToFloat(tmpScorelist[i], (float)0),
                                        true);
                        }
                    }
                    //}
                }
            }


            string useridlist = GetUserListWithPostlist(tid, postidlist);

            return UserCredits.UpdateUserCreditsByRate(useridlist, extcreditslist);

        }

        /// <summary>
        /// 用当前的评分值通过一定兑换比例换算成积分后，更新相应贴子中的rate字段.
        /// </summary>
        /// <param name="postid">帖子ID</param>
        /// <param name="extid">扩展积分ID</param>
        /// <param name="score">分数</param>
        /// <param name="israte">true为评分，false为撤消评分</param>
        public static void SetPostRate(string posttableid, int postid, int extid, float score, bool israte)
        {
            if (score == 0)
            {
                return;
            }

            DataTable scorePaySet = Scoresets.GetRateScoreSet();
            if (scorePaySet.Rows.Count > 0)
            {
                if (scorePaySet.Rows[extid - 1]["name"].ToString().Trim() != "")
                {
                    float rate = Utils.StrToFloat(scorePaySet.Rows[extid - 1]["rate"].ToString(), (float)0);
                    
                    if (rate != 0)
                    {
                        rate = (rate * score);
                    }

                    //当是撤消积分
                    if (!israte)
                    {
                        rate = -1 * rate;
                    }

                   DatabaseProvider.GetInstance().UpdatePostRate(postid, rate, posttableid);
                   IDataReader reader=DatabaseProvider.GetInstance().GetPostInfo(posttableid, postid);
                   while (reader.Read())
                   {
                       if (Utils.StrToInt(reader["layer"], -1) == 0)
                       {
                           DatabaseProvider.GetInstance().SetTopicStatus(reader["tid"].ToString(), "rate", Utils.StrToInt(reader["rate"].ToString(),-1));
                       }
                   
                   }

                }
            }
        }

        /// <summary>
        /// 检查评分状态
        /// </summary>
        /// <param name="postidlist">帖子id列表</param>
        /// <param name="userid">用户id</param>
        /// <returns>被评分的帖子id字符串</returns>
        public static string CheckRateState(string postidlist, int userid)
        {
            string reval = "";
            string tempreval = "";
            foreach (string pid in Utils.SplitString(postidlist, ","))
            {
                tempreval = DatabaseProvider.GetInstance().CheckRateState(userid, pid);
                if (!tempreval.Equals(""))
                {
                    if (!reval.Equals(""))
                    {
                        reval = reval + ",";
                    }
                    reval = reval + tempreval;
                }

            }
            return reval;

        }


        /// <summary>
        /// 返回指定主题的最后一次操作
        /// </summary>
        /// <param name="tid">主题id</param>
        /// <returns>管理日志内容</returns>
        public static string GetTopicListModeratorLog(int tid)
        {
            string str = "";
            IDataReader reader = null;

            reader = DatabaseProvider.GetInstance().GetTopicListModeratorLog(tid);
            if (reader != null)
            {
                if (reader.Read())
                {
                    str = "本主题由 " + reader["grouptitle"].ToString() + " " + reader["moderatorname"].ToString() + " 于 " + reader["postdatetime"].ToString() + " 执行 " + reader["actions"].ToString() + " 操作";

                }
                reader.Close();
            }
            return str;
        }



        /// <summary>
        /// 重设主题类型
        /// </summary>
        /// <param name="topictypeid">主题类型</param>
        /// <param name="topiclist">要设置的主题列表</param>
        /// <returns></returns>
        public static int ResetTopicTypes(int topictypeid, string topiclist)
        {
            return DatabaseProvider.GetInstance().ResetTopicTypes(topictypeid, topiclist);
        }


        public static void IdentifyTopic(string topiclist, int identify)
        {
            if (!Utils.IsNumericArray(topiclist.Split(',')))
                return;
            DatabaseProvider.GetInstance().IdentifyTopic(topiclist, identify);
        }

        /// <summary>
        /// 撤消评分
        /// </summary>
        /// <param name="tid"></param>
        /// <param name="postidlist"></param>
        /// <param name="userid"></param>
        /// <param name="username"></param>
        /// <param name="reason"></param>
        public static void CancelRatePosts(string ratelogidlist, int tid, string pid, int userid, string username, int groupid, string grouptitle, int forumid, string forumname, string reason)
        {
            if (!Utils.IsNumeric(pid))
            {
                return;
            }

            string posttableid = Posts.GetPostTableID(tid);

            DataTable dt = AdminRateLogs.LogList(ratelogidlist.Split(',').Length, 1, "id IN(" + ratelogidlist + ")");//得到要删除的评分日志列表

            int rateduserid = Posts.GetPostInfo(tid, Utils.StrToInt(pid, 0)).Posterid; //被评分的用户的UID

            if (rateduserid <= 0)
            {
                return;
            }

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    SetPostRate(posttableid,
                                 Utils.StrToInt(pid, 0),
                                 Utils.StrToInt(dr["extcredits"].ToString(), 0),
                                 Utils.StrToFloat(dr["score"].ToString(), (float)0),
                                 false);

                    DatabaseProvider.GetInstance().UpdateUserCredits(
                        Utils.StrToInt(rateduserid, 0),
                        Utils.StrToInt(dr["extcredits"].ToString(), 0),
                        (-1) * Utils.StrToFloat(dr["score"].ToString(), (float)0)); //乘-1是要进行分值的反向操作
                }
            }


            AdminRateLogs.DeleteLog("[id] IN(" + ratelogidlist + ")");

            //当贴子已无评分记录时，则清空贴子相关的评分信息字段(rate,ratetimes)
            if (AdminRateLogs.LogList(1, 1, "pid = " + pid).Rows.Count == 0)
            {
                DatabaseProvider.GetInstance().CancelPostRate(pid, posttableid);
            }

            TopicInfo topicinfo = Topics.GetTopicInfo(tid);
            string topictitle = "暂无标题";
            if (topicinfo != null)
            {
                topictitle = topicinfo.Title;
            }

            DatabaseProvider.GetInstance().InsertModeratorLog(userid.ToString(),
                                                              username,
                                                              groupid.ToString(),
                                                              grouptitle,
                                                              Utils.GetRealIP(),
                                                              Utils.GetDateTime(),
                                                              forumid.ToString(),
                                                              forumname,
                                                              tid.ToString(),
                                                              topictitle,
                                                              "撤消评分",
                                                              reason);
        }

        public static void BumpTopics(string topiclist, int bumptype)
        {

            int lastpostid = 0;
            if (bumptype == 1)
            {
                string  [] tidlist = topiclist.Split(',');
                foreach (string tid in tidlist)
                {
                    lastpostid = DatabaseProvider.GetInstance().GetPostId();
                    DatabaseProvider.GetInstance().SetTopicsBump(tid, lastpostid);
                }
            }
            else
            {

                DatabaseProvider.GetInstance().SetTopicsBump(topiclist, lastpostid);
            }
        }


    } //class end


}