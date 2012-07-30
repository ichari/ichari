#if NET1
#else
using System;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using Discuz.Data;
using Discuz.Config;
using Discuz.Common;
using Discuz.Entity;
using System.Text.RegularExpressions;
using SQLDMO;
using System.Collections.Generic;


namespace Discuz.Data.SqlServer
{
    public partial class DataProvider : IDataProvider
    {
        private string GetSqlstringByPostDatetime(string sqlstring, DateTime postdatetimeStart, DateTime postdatetimeEnd)
        {
            //日期需要改成参数，以后需要重构！需要先修改后台传递参数方式
            if (postdatetimeStart.ToString() != "")
            {
                sqlstring += " AND [postdatetime]>='" + postdatetimeStart.ToString("yyyy-MM-dd HH:mm:ss") + "'";
            }

            if (postdatetimeEnd.ToString() != "")
            {
                sqlstring += " AND [postdatetime]<='" + postdatetimeEnd.AddDays(1).ToString("yyyy-MM-dd HH:mm:ss") + "'";
            }
            return sqlstring;
        }


        public DataTable GetAdsTable()
        {
            return DbHelper.ExecuteDataset(CommandType.Text, "SELECT [advid], [type], [displayorder], [targets], [parameters], [code] FROM [" + BaseConfigs.GetTablePrefix + "advertisements] WHERE [available]=1 AND [starttime] <='" + DateTime.Now.ToShortDateString() + "' AND [endtime] >='" + DateTime.Now.ToShortDateString() + "' ORDER BY [displayorder] DESC, [advid] DESC").Tables[0];
        }

        /// <summary>
        /// 获得全部指定时间段内的公告列表
        /// </summary>
        /// <param name="starttime">开始时间</param>
        /// <param name="endtime">结束时间</param>
        /// <returns>公告列表</returns>
        public DataTable GetAnnouncementList(string starttime, string endtime)
        {
            DbParameter[] parms = {
										   DbHelper.MakeInParam("@starttime",(DbType)SqlDbType.DateTime, 8, DateTime.Parse(starttime)),
										   DbHelper.MakeInParam("@endtime",(DbType)SqlDbType.DateTime, 8, DateTime.Parse(endtime))
									   };
            return DbHelper.ExecuteDataset(CommandType.Text, "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "announcements] WHERE [starttime] <=@starttime AND [endtime] >=@starttime ORDER BY [displayorder], [id] DESC", parms).Tables[0];
        }

        /// <summary>
        /// 获得全部指定时间段内的前n条公告列表
        /// </summary>
        /// <param name="starttime">开始时间</param>
        /// <param name="endtime">结束时间</param>
        /// <param name="maxcount">最大记录数,小于0返回全部</param>
        /// <returns>公告列表</returns>
        public DataTable GetSimplifiedAnnouncementList(string starttime, string endtime, int maxcount)
        {
            DbParameter[] parms = {
										   DbHelper.MakeInParam("@starttime",(DbType)SqlDbType.DateTime, 8, DateTime.Parse(starttime)),
										   DbHelper.MakeInParam("@endtime",(DbType)SqlDbType.DateTime, 8, DateTime.Parse(endtime))
									   };
            string topstr = " TOP " + maxcount;
            if (maxcount < 0)
                topstr = "";
            string sqlstr = "SELECT" + topstr + " [id], [title], [poster], [posterid],[starttime] FROM [" + BaseConfigs.GetTablePrefix + "announcements] WHERE [starttime] <=@starttime AND [endtime] >=@starttime ORDER BY [displayorder], [id] DESC";

            return DbHelper.ExecuteDataset(CommandType.Text, sqlstr, parms).Tables[0];
        }


        public int AddAnnouncement(string poster, int posterid, string title, int displayorder, string starttime, string endtime, string message)
        {
            DbParameter[] parms = {
                                        DbHelper.MakeInParam("@poster", (DbType)SqlDbType.NVarChar, 20, poster),
                                        DbHelper.MakeInParam("@posterid", (DbType)SqlDbType.Int, 4, posterid),
                                        DbHelper.MakeInParam("@title", (DbType)SqlDbType.NVarChar, 250, title),
                                        DbHelper.MakeInParam("@displayorder", (DbType)SqlDbType.Int, 4, displayorder),
                                        DbHelper.MakeInParam("@starttime", (DbType)SqlDbType.DateTime, 8, DateTime.Parse(starttime)),
                                        DbHelper.MakeInParam("@endtime", (DbType)SqlDbType.DateTime, 8, DateTime.Parse(endtime)),
                                        DbHelper.MakeInParam("@message", (DbType)SqlDbType.NText, 0, message)
                                    };
            string sqlstring = "INSERT INTO [" + BaseConfigs.GetTablePrefix + "announcements] ([poster],[posterid],[title],[displayorder],[starttime],[endtime],[message]) VALUES(@poster, @posterid, @title, @displayorder, @starttime, @endtime, @message)";
            //this.username,
            //this.userid,
            //title.Text,
            //displayorder.Text,
            //starttime.Text,
            //endtime.Text,
            //message.Text);
            return DbHelper.ExecuteNonQuery(CommandType.Text, sqlstring, parms);
        }

        public string GetAnnouncements()
        {
            return "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "announcements] ORDER BY [id] ASC";
        }

        public void DeleteAnnouncements(string idlist)
        {
            string sqlstring = "DELETE FROM [" + BaseConfigs.GetTablePrefix + "announcements] WHERE [id] IN(" + idlist + ")";

            DbHelper.ExecuteNonQuery(CommandType.Text, sqlstring);
        }

        public DataTable GetAnnouncement(int id)
        {
            DbParameter parm = DbHelper.MakeInParam("@id", (DbType)SqlDbType.Int, 4, id);
            return DbHelper.ExecuteDataset(CommandType.Text, "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "announcements] WHERE [id]=@id", parm).Tables[0];
        }

        public void UpdateAnnouncement(int id, string poster, string title, int displayorder, string starttime, string endtime, string message)
        {
            DbParameter[] parms = {
                                        DbHelper.MakeInParam("@id", (DbType)SqlDbType.Int, 4, id),
                                        DbHelper.MakeInParam("@poster", (DbType)SqlDbType.NVarChar, 20, poster),
                                        DbHelper.MakeInParam("@title", (DbType)SqlDbType.NVarChar, 250, title),
                                        DbHelper.MakeInParam("@displayorder", (DbType)SqlDbType.Int, 4, displayorder),
                                        DbHelper.MakeInParam("@starttime", (DbType)SqlDbType.DateTime, 8, DateTime.Parse(starttime)),
                                        DbHelper.MakeInParam("@endtime", (DbType)SqlDbType.DateTime, 8, DateTime.Parse(endtime)),
                                        DbHelper.MakeInParam("@message", (DbType)SqlDbType.NText, 0, message)
                                    };
            string sqlstring = "UPDATE [" + BaseConfigs.GetTablePrefix + "announcements] SET [displayorder]=@displayorder,[title]=@title, [poster]=@poster,[starttime]=@starttime,[endtime]=@endtime,[message]=@message WHERE [id]=@id";

            DbHelper.ExecuteNonQuery(CommandType.Text, sqlstring, parms);
        }

        public void DeleteAnnouncement(int id)
        {
            DbParameter parm = DbHelper.MakeInParam("@id", (DbType)SqlDbType.Int, 4, id);

            DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE FROM [" + BaseConfigs.GetTablePrefix + "announcements] WHERE [id]=@id", parm);
        }


        /// <summary>
        /// 获得公共可见板块列表
        /// </summary>
        /// <returns></returns>
        public IDataReader GetVisibleForumList()
        {
            return DbHelper.ExecuteReader(CommandType.Text, string.Format("SELECT [name], [fid], [layer] FROM [{0}forums] WHERE [parentid] NOT IN (SELECT fid FROM [{0}forums] WHERE [status] < 1 AND [layer] = 0) AND [status] > 0 AND [displayorder] >=0 ORDER BY [displayorder]", BaseConfigs.GetTablePrefix));
        }

        public IDataReader GetOnlineGroupIconList()
        {
            return DbHelper.ExecuteReader(CommandType.Text, "SELECT [title], [img] FROM [" + BaseConfigs.GetTablePrefix + "onlinelist] WHERE [img]<> '' ORDER BY [displayorder]");
        }

        /// <summary>
        /// 获得友情链接列表
        /// </summary>
        /// <returns>友情链接列表</returns>
        public DataTable GetForumLinkList()
        {
            return DbHelper.ExecuteDataset(CommandType.Text, @"SELECT [name],[url],[note],[displayorder]+10000 AS [displayorder],[logo] FROM [" + BaseConfigs.GetTablePrefix + @"forumlinks] WHERE [displayorder] > 0 AND [logo] = '' 
																	UNION SELECT [name],[url],[note],[displayorder],[logo] FROM [" + BaseConfigs.GetTablePrefix + @"forumlinks] WHERE [displayorder] > 0 AND [logo] <> '' ORDER BY [displayorder]").Tables[0];
        }

        /// <summary>
        /// 获得脏字过滤列表
        /// </summary>
        /// <returns></returns>
        public DataTable GetBanWordList()
        {
            return DbHelper.ExecuteDataset(CommandType.Text, "SELECT [find], [replacement] FROM [" + BaseConfigs.GetTablePrefix + "words]").Tables[0];
        }

        /// <summary>
        /// 获得勋章列表
        /// </summary>
        /// <returns>获得勋章列表</returns>
        public DataTable GetMedalsList()
        {
            return DbHelper.ExecuteDataset(CommandType.Text, "SELECT [medalid], [name], [image],[available]  FROM [" + BaseConfigs.GetTablePrefix + "medals] ORDER BY [medalid]").Tables[0];
        }

        /// <summary>
        /// 获得魔法表情列表
        /// </summary>
        /// <returns>魔法表情列表</returns>
        public DataTable GetMagicList()
        {
            return DbHelper.ExecuteDataset(CommandType.Text, "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "magic] ORDER BY [id]").Tables[0];
        }

        /// <summary>
        /// 获得主题类型列表
        /// </summary>
        /// <returns></returns>
        public DataTable GetTopicTypeList()
        {
            return DbHelper.ExecuteDataset(CommandType.Text, "SELECT [typeid],[name] FROM [" + BaseConfigs.GetTablePrefix + "topictypes] ORDER BY [displayorder]").Tables[0];
        }

        /// <summary>
        /// 添加积分转帐兑换记录
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="fromto">来自/到</param>
        /// <param name="sendcredits">付出积分类型</param>
        /// <param name="receivecredits">得到积分类型</param>
        /// <param name="send">付出积分数额</param>
        /// <param name="receive">得到积分数额</param>
        /// <param name="paydate">时间</param>
        /// <param name="operation">积分操作(1=兑换, 2=转帐)</param>
        /// <returns>执行影响的行</returns>
        public int AddCreditsLog(int uid, int fromto, int sendcredits, int receivecredits, float send, float receive, string paydate, int operation)
        {

            DbParameter[] parms = {
									   DbHelper.MakeInParam("@uid",(DbType)SqlDbType.Int,4,uid),
									   DbHelper.MakeInParam("@fromto",(DbType)SqlDbType.Int,4,fromto),
									   DbHelper.MakeInParam("@sendcredits",(DbType)SqlDbType.Int,4,sendcredits),
									   DbHelper.MakeInParam("@receivecredits",(DbType)SqlDbType.Int,4,receivecredits),
									   DbHelper.MakeInParam("@send",(DbType)SqlDbType.Float,4,send),
									   DbHelper.MakeInParam("@receive",(DbType)SqlDbType.Float,4,receive),
									   DbHelper.MakeInParam("@paydate",(DbType)SqlDbType.VarChar,0,paydate),
									   DbHelper.MakeInParam("@operation",(DbType)SqlDbType.Int,4,operation)
								   };

            return DbHelper.ExecuteNonQuery(CommandType.Text, "INSERT INTO [" + BaseConfigs.GetTablePrefix + "creditslog] ([uid],[fromto],[sendcredits],[receivecredits],[send],[receive],[paydate],[operation]) VALUES(@uid,@fromto,@sendcredits,@receivecredits,@send,@receive,@paydate,@operation)", parms);

        }

        /// <summary>
        /// 返回指定范围的积分日志
        /// </summary>
        /// <param name="pagesize">页大小</param>
        /// <param name="currentpage">当前页数</param>
        /// <param name="uid">用户id</param>
        /// <returns>积分日志</returns>
        public DataTable GetCreditsLogList(int pagesize, int currentpage, int uid)
        {
            int pagetop = (currentpage - 1) * pagesize;
            string sqlstring;
            if (currentpage == 1)
            {
                //select c.*,ufrom.username as fromuser ,uto.username as touser from dnt_creditslog c,dnt_users ufrom, dnt_users uto where c.uid=ufrom.uid AND c.fromto=uto.uid
// AND (c.uid=1 or c.fromto =1)
                sqlstring = string.Format("SELECT TOP {0} [c].*, [ufrom].[username] AS [fromuser], [uto].[username] AS [touser] FROM [{1}creditslog] AS [c], [{1}users] AS [ufrom], [{1}users] AS [uto] WHERE [c].[uid]=[ufrom].[uid] AND [c].[fromto]=[uto].[uid] AND ([c].[uid]={2} OR [c].[fromto] = {2})  ORDER BY [id] DESC", pagesize, BaseConfigs.GetTablePrefix, uid);
            }
            else
            {
                sqlstring = string.Format("SELECT TOP {0} [c].*, [ufrom].[username] AS [fromuser], [uto].[username] AS [touser] FROM [{1}creditslog] AS [c], [{1}users] AS [ufrom], [{1}users] AS [uto] WHERE [id] < (SELECT MIN([id])  FROM (SELECT TOP {2} [id] FROM [{1}creditslog] WHERE [{1}creditslog].[uid]={3}  OR [{1}creditslog].[fromto]={3} ORDER BY [id] DESC) AS tblTmp ) AND [c].[uid]=[ufrom].[uid] AND [c].[fromto]=[uto].[uid] AND ([c].[uid]={3} OR [c].[fromto] = {3}) ORDER BY [c].[id] DESC", pagesize, BaseConfigs.GetTablePrefix, pagetop, uid);
            }

            return DbHelper.ExecuteDataset(CommandType.Text, sqlstring).Tables[0];
        }

        /// <summary>
        /// 获得指定用户的积分交易历史记录总条数
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <returns>历史记录总条数</returns>
        public int GetCreditsLogRecordCount(int uid)
        {
            return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, string.Format("SELECT COUNT(1) FROM [{0}creditslog] WHERE [uid]={1} OR [fromto]={1}", BaseConfigs.GetTablePrefix, uid)), 0);
        }


        public string GetTableStruct()
        {
            #region 数据表查询语句

            string SqlString = null;
            SqlString = "SELECT 表名=case when a.colorder=1 then d.name else '' end,";
            SqlString += "表说明=case when a.colorder=1 then isnull(f.value,'') else '' end,";
            SqlString += " 字段序号=a.colorder,";
            SqlString += " 字段名=a.name,";
            SqlString += " 标识=case when COLUMNPROPERTY( a.id,a.name,'IsIdentity')=1 then '√'else '' end,";
            SqlString += " 主键=case when exists(SELECT 1 FROM sysobjects where xtype='PK' and name in (";
            SqlString += " SELECT name FROM sysindexes WHERE indid in(";
            SqlString += "   SELECT indid FROM sysindexkeys WHERE id = a.id AND colid=a.colid";
            SqlString += "  ))) then '√' else '' end,";
            SqlString += " 类型=b.name,";
            SqlString += " 占用字节数=a.length,";
            SqlString += " 长度=COLUMNPROPERTY(a.id,a.name,'PRECISION'),";
            SqlString += " 小数位数=isnull(COLUMNPROPERTY(a.id,a.name,'Scale'),0),";
            SqlString += " 允许空=case when a.isnullable=1 then '√'else '' end,";
            SqlString += " 默认值=isnull(e.text,''),";
            SqlString += " 字段说明=isnull(g.[value],'')";
            SqlString += "FROM syscolumns a";
            SqlString += " left join systypes b on a.xtype=b.xusertype";
            SqlString += " inner join sysobjects d on a.id=d.id  and d.xtype='U' and  d.name<>'dtproperties'";
            SqlString += " left join syscomments e on a.cdefault=e.id";
            SqlString += " left join sysproperties g on a.id=g.id and a.colid=g.smallid  ";
            SqlString += " left join sysproperties f on d.id=f.id and f.smallid=0";
            //SqlString+="--where d.name='要查询的表'    --如果只查询指定表,加上此条件";
            SqlString += " order by a.id,a.colorder";
            return SqlString;

            #endregion
        }

        public void ShrinkDataBase(string shrinksize, string dbname)
        {
            string SqlString = null;

            SqlString += "SET NOCOUNT ON ";

            SqlString += "DECLARE @LogicalFileName sysname, @MaxMinutes INT, @NewSize INT ";
            SqlString += "USE [" + dbname + "] -- 要操作的数据库名 ";
            SqlString += "SELECT @LogicalFileName = '" + dbname + "_log', -- 日志文件名 ";
            SqlString += "@MaxMinutes = 10, -- Limit on time allowed to wrap log. ";
            SqlString += "@NewSize = 1 -- 你想设定的日志文件的大小(M) ";
            SqlString += "-- Setup / initialize ";
            SqlString += "DECLARE @OriginalSize int ";
            SqlString += "SELECT @OriginalSize = " + shrinksize;
            SqlString += "FROM sysfiles ";
            SqlString += "WHERE name = @LogicalFileName ";
            SqlString += "SELECT 'Original Size of ' + db_name() + ' LOG is ' + ";
            SqlString += "CONVERT(VARCHAR(30),@OriginalSize) + ' 8K pages or ' + ";
            SqlString += "CONVERT(VARCHAR(30),(@OriginalSize*8/1024)) + 'MB' ";
            SqlString += "FROM sysfiles ";
            SqlString += "WHERE name = @LogicalFileName ";
            SqlString += "CREATE TABLE DummyTrans ";
            SqlString += "(DummyColumn char (8000) not null) ";
            SqlString += "DECLARE @Counter INT, ";
            SqlString += "@StartTime DATETIME, ";
            SqlString += "@TruncLog VARCHAR(255) ";
            SqlString += "SELECT @StartTime = GETDATE(), ";
            SqlString += "@TruncLog = 'BACKUP LOG ' + db_name() + ' WITH TRUNCATE_ONLY' ";
            SqlString += "DBCC SHRINKFILE (@LogicalFileName, @NewSize) ";
            SqlString += "EXEC (@TruncLog) ";
            SqlString += "-- Wrap the log if necessary. ";
            SqlString += "WHILE @MaxMinutes > DATEDIFF (mi, @StartTime, GETDATE()) -- time has not expired ";
            SqlString += "AND @OriginalSize = (SELECT size FROM sysfiles WHERE name = @LogicalFileName) ";
            SqlString += "AND (@OriginalSize * 8 /1024) > @NewSize ";
            SqlString += "BEGIN -- Outer loop. ";
            SqlString += "SELECT @Counter = 0 ";
            SqlString += "WHILE ((@Counter < @OriginalSize / 16) AND (@Counter < 50000)) ";
            SqlString += "BEGIN -- update ";
            SqlString += "INSERT DummyTrans VALUES ('Fill Log') ";
            SqlString += "DELETE DummyTrans ";
            SqlString += "SELECT @Counter = @Counter + 1 ";
            SqlString += "END ";
            SqlString += "EXEC (@TruncLog) ";
            SqlString += "END ";
            SqlString += "SELECT 'Final Size of ' + db_name() + ' LOG is ' + ";
            SqlString += "CONVERT(VARCHAR(30),size) + ' 8K pages or ' + ";
            SqlString += "CONVERT(VARCHAR(30),(size*8/1024)) + 'MB' ";
            SqlString += "FROM sysfiles ";
            SqlString += "WHERE name = @LogicalFileName ";
            SqlString += "DROP TABLE DummyTrans ";
            SqlString += "SET NOCOUNT OFF ";

            DbHelper.ExecuteDataset(CommandType.Text, SqlString);
        }

        public void ClearDBLog(string dbname)
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@DBName", (DbType)SqlDbType.VarChar, 50, dbname),
			};
            DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, "" + BaseConfigs.GetTablePrefix + "shrinklog", parms);
        }

        public string RunSql(string sql)
        {
            string errorInfo = string.Empty;
            if (sql != "")
            {
                SqlConnection conn = new SqlConnection(DbHelper.ConnectionString);
                conn.Open();
                string[] sqlArray = Utils.SplitString(sql, "--/* Discuz!NT SQL Separator */--");
                foreach (string sqlStr in sqlArray)
                {
                    if(sqlStr.Trim() == string.Empty)   //当读到空的Sql语句则继续
                    {
                        continue;
                    }
                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        try
                        {
                            DbHelper.ExecuteNonQuery(CommandType.Text, sqlStr);
                            trans.Commit();
                        }
                        catch (Exception ex)
                        {
                            trans.Rollback();
                            string message = ex.Message.Replace("'", " ");
                            message = message.Replace("\\", "/");
                            message = message.Replace("\r\n", "\\r\\n");
                            message = message.Replace("\r", "\\r");
                            message = message.Replace("\n", "\\n");
                            errorInfo += message + "<br />";
                        }
                    }
                }
              conn.Close();
            }
            return errorInfo;
        }

        //得到数据库的名称
        public string GetDbName()
        {
            string connectionString = BaseConfigs.GetDBConnectString;
            foreach (string info in connectionString.Split(';'))
            {
                if (info.ToLower().IndexOf("initial catalog") >= 0 || info.ToLower().IndexOf("database") >= 0)
                {
                    return info.Split('=')[1].Trim();
                }
            }
            return "dnt";
        }


        /// <summary>
        /// 创建并填充指定帖子分表id全文索引
        /// </summary>
        /// <param name="DbName">数据库名称</param>
        /// <param name="postsid">当前帖子表的id</param>
        /// <returns></returns>
        public bool CreateORFillIndex(string DbName, string postsid)
        {
            StringBuilder sb = new StringBuilder();

            string currenttablename = BaseConfigs.GetTablePrefix + "posts" + postsid;

            try
            {
                //如果有全文索引则进行填充,如果没有就抛出异常
                sb.Remove(0, sb.Length);
                DbHelper.ExecuteNonQuery("SELECT TOP 1 [pid] FROM [" + currenttablename + "] WHERE CONTAINS([message],'asd') ORDER BY [pid] ASC");

                sb.Append("EXECUTE sp_fulltext_catalog 'pk_" + currenttablename + "_msg','start_full'; \r\n");
                sb.Append("WHILE fulltextcatalogproperty('pk_" + currenttablename + "_msg','populateStatus')<>0 \r\n");
                sb.Append("BEGIN \r\n");
                sb.Append("WAITFOR DELAY '0:5:30' \r\n");
                sb.Append("END \r\n");
                DbHelper.ExecuteNonQuery(sb.ToString());

                return true;
            }
            catch
            {
                try
                {
                    #region 构建全文索引

                    sb.Remove(0, sb.Length);
                    sb.Append("IF(SELECT DATABASEPROPERTY('[" + DbName + "]','isfulltextenabled'))=0  EXECUTE sp_fulltext_database 'enable';");

                    try
                    { //此处删除以确保全文索引目录和系统表中的数据同步
                        sb.Append("EXECUTE sp_fulltext_table '[" + currenttablename + "]', 'drop' ;");
                        sb.Append("EXECUTE sp_fulltext_catalog 'pk_" + currenttablename + "_msg','drop';");
                        DbHelper.ExecuteNonQuery(sb.ToString());
                    }
                    catch
                    {
                        ;
                    }
                    finally
                    {
                        //执行全文填充语句
                        sb.Remove(0, sb.Length);
                        sb.Append("EXECUTE sp_fulltext_catalog 'pk_" + currenttablename + "_msg','create';");
                        sb.Append("EXECUTE sp_fulltext_table '[" + currenttablename + "]','create','pk_" + currenttablename + "_msg','pk_" + currenttablename + "';");
                        sb.Append("EXECUTE sp_fulltext_column '[" + currenttablename + "]','message','add';");
                        sb.Append("EXECUTE sp_fulltext_table '[" + currenttablename + "]','activate';");
                        sb.Append("EXECUTE sp_fulltext_catalog 'pk_" + currenttablename + "_msg','start_full';");
                        DbHelper.ExecuteNonQuery(sb.ToString());
                    }
                    return true;

                    #endregion
                }
                catch (SqlException ex)
                {
                    string message = ex.Message.Replace("'", " ");
                    message = message.Replace("\\", "/");
                    message = message.Replace("\r\n", "\\r\\n");
                    message = message.Replace("\r", "\\r");
                    message = message.Replace("\n", "\\n");
                    return true;
                }
            }
        }


        /// <summary>
        /// 得到指定帖子分表的全文索引建立(填充)语句
        /// </summary>
        /// <param name="tablename"></param>
        /// <returns></returns>
        public string GetSpecialTableFullIndexSQL(string tablename)
        {
            #region 建表

            StringBuilder sb = new StringBuilder();
            sb.Append("IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[" + tablename + "]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)  DROP TABLE [" + tablename + "];");
            sb.Append("CREATE TABLE [" + tablename + "] ([pid] [int] NOT NULL ,[fid] [int] NOT NULL ," +
                "[tid] [int] NOT NULL ,[parentid] [int] NOT NULL ,[layer] [int] NOT NULL ,[poster] [nvarchar] (20) NOT NULL ," +
                "[posterid] [int] NOT NULL ,[title] [nvarchar] (80) NOT NULL ,[postdatetime] [smalldatetime] NOT NULL ," +
                "[message] [ntext] NOT NULL ,[ip] [nvarchar] (15) NOT NULL ," +
                "[lastedit] [nvarchar] (50) NOT NULL ,[invisible] [int] NOT NULL ,[usesig] [int] NOT NULL ,[htmlon] [int] NOT NULL ," +
                "[smileyoff] [int] NOT NULL ,[parseurloff] [int] NOT NULL ,[bbcodeoff] [int] NOT NULL ,[attachment] [int] NOT NULL ,[rate] [int] NOT NULL ," +
                "[ratetimes] [int] NOT NULL ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]");
            sb.Append(";");
            sb.Append("ALTER TABLE [" + tablename + "] WITH NOCHECK ADD CONSTRAINT [PK_" + tablename + "] PRIMARY KEY  CLUSTERED ([pid])  ON [PRIMARY]");
            sb.Append(";");

            sb.Append("ALTER TABLE [" + tablename + "] ADD ");
            sb.Append("CONSTRAINT [DF_" + tablename + "_pid] DEFAULT (0) FOR [pid],");
            sb.Append("CONSTRAINT [DF_" + tablename + "_parentid] DEFAULT (0) FOR [parentid],CONSTRAINT [DF_" + tablename + "_layer] DEFAULT (0) FOR [layer],");
            sb.Append("CONSTRAINT [DF_" + tablename + "_poster] DEFAULT ('') FOR [poster],CONSTRAINT [DF_" + tablename + "_posterid] DEFAULT (0) FOR [posterid],");
            sb.Append("CONSTRAINT [DF_" + tablename + "_postdatetime] DEFAULT (getdate()) FOR [postdatetime],CONSTRAINT [DF_" + tablename + "_message] DEFAULT ('') FOR [message],");
            sb.Append("CONSTRAINT [DF_" + tablename + "_ip] DEFAULT ('') FOR [ip],CONSTRAINT [DF_" + tablename + "_lastedit] DEFAULT ('') FOR [lastedit],");
            sb.Append("CONSTRAINT [DF_" + tablename + "_invisible] DEFAULT (0) FOR [invisible],CONSTRAINT [DF_" + tablename + "_usesig] DEFAULT (0) FOR [usesig],");
            sb.Append("CONSTRAINT [DF_" + tablename + "_htmlon] DEFAULT (0) FOR [htmlon],CONSTRAINT [DF_" + tablename + "_smileyoff] DEFAULT (0) FOR [smileyoff],");
            sb.Append("CONSTRAINT [DF_" + tablename + "_parseurloff] DEFAULT (0) FOR [parseurloff],CONSTRAINT [DF_" + tablename + "_bbcodeoff] DEFAULT (0) FOR [bbcodeoff],");
            sb.Append("CONSTRAINT [DF_" + tablename + "_attachment] DEFAULT (0) FOR [attachment],CONSTRAINT [DF_" + tablename + "_rate] DEFAULT (0) FOR [rate],");
            sb.Append("CONSTRAINT [DF_" + tablename + "_ratetimes] DEFAULT (0) FOR [ratetimes]");

            sb.Append(";");
            sb.Append("CREATE  INDEX [parentid] ON [" + tablename + "]([parentid]) ON [PRIMARY]");
            sb.Append(";");

            sb.Append("CREATE  UNIQUE  INDEX [showtopic] ON [" + tablename + "]([tid], [invisible], [pid]) ON [PRIMARY]");
            sb.Append(";");


            sb.Append("CREATE  INDEX [treelist] ON [" + tablename + "]([tid], [invisible], [parentid]) ON [PRIMARY]");
            sb.Append(";");

            #endregion

            #region 建全文索引

            sb.Append("USE " + GetDbName() + " \r\n");
            sb.Append("EXECUTE sp_fulltext_database 'enable'; \r\n");
            sb.Append("IF(SELECT DATABASEPROPERTY('[" + GetDbName() + "]','isfulltextenabled'))=0  EXECUTE sp_fulltext_database 'enable';");
            sb.Append("IF EXISTS (SELECT * FROM sysfulltextcatalogs WHERE name ='pk_" + tablename + "_msg')  EXECUTE sp_fulltext_catalog 'pk_" + tablename + "_msg','drop';");
            sb.Append("IF EXISTS (SELECT * FROM sysfulltextcatalogs WHERE name ='pk_" + tablename + "_msg')  EXECUTE sp_fulltext_table '[" + tablename + "]', 'drop' ;");
            sb.Append("EXECUTE sp_fulltext_catalog 'pk_" + tablename + "_msg','create';");
            sb.Append("EXECUTE sp_fulltext_table '[" + tablename + "]','create','pk_" + tablename + "_msg','pk_" + tablename + "';");
            sb.Append("EXECUTE sp_fulltext_column '[" + tablename + "]','message','add';");
            sb.Append("EXECUTE sp_fulltext_table '[" + tablename + "]','activate';");
            sb.Append("EXECUTE sp_fulltext_catalog 'pk_" + tablename + "_msg','start_full';");

            #endregion

            return sb.ToString();
        }


        /// <summary>
        /// 以DataReader返回自定义编辑器按钮列表
        /// </summary>
        /// <returns></returns>
        public IDataReader GetCustomEditButtonList()
        {
            return DbHelper.ExecuteReader(CommandType.Text, "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "bbcodes] WHERE [available] = 1");
        }

        /// <summary>
        /// 以DataTable返回自定义按钮列表
        /// </summary>
        /// <returns></returns>
        public DataTable GetCustomEditButtonListWithTable()
        {
            DataSet ds = DbHelper.ExecuteDataset(CommandType.Text, "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "bbcodes] WHERE [available] = 1");
            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    return ds.Tables[0];
                }
            }
            ds.Dispose();
            return null;
        }

        public DataRowCollection GetTableListIds()
        {
            return DbHelper.ExecuteDataset(CommandType.Text, "SELECT [id] FROM [" + BaseConfigs.GetTablePrefix + "tablelist]").Tables[0].Rows;
        }



        public void UpdateAnnouncementPoster(int posterid, string poster)
        {
            DbParameter[] parms = { 
                                        DbHelper.MakeInParam("@posterid", (DbType)SqlDbType.Int, 4, posterid),
                                        DbHelper.MakeInParam("@poster", (DbType)SqlDbType.VarChar, 20, poster)
                                    };

            DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "announcements] SET [poster]=@poster WHERE [posterid]=@posterid", parms);
        }

        public bool HasStatisticsByLastUserId(int lastuserid)
        {
            DbParameter parm = DbHelper.MakeInParam("@lastuserid", (DbType)SqlDbType.Int, 4, lastuserid);
            return DbHelper.ExecuteDataset(CommandType.Text, "SELECT TOP 1 [lastuserid] FROM  [" + BaseConfigs.GetTablePrefix + "statistics]  WHERE [lastuserid]=@lastuserid",parm).Tables[0].Rows.Count > 0;
        }

        public void UpdateStatisticsLastUserName(int lastuserid, string lastusername)
        {
            DbParameter[] parms = { 
                                        DbHelper.MakeInParam("@lastuserid", (DbType)SqlDbType.Int, 4, lastuserid),
                                        DbHelper.MakeInParam("@lastusername", (DbType)SqlDbType.VarChar, 20, lastusername)
                                    };

            DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "statistics] SET [lastusername]=@lastusername WHERE [lastuserid]=@lastuserid", parms);
        }

        public void AddVisitLog(int uid, string username, int groupid, string grouptitle, string ip, string actions, string others)
        {
            string sqlstring = "INSERT INTO [" + BaseConfigs.GetTablePrefix + "adminvisitlog] ([uid],[username],[groupid],[grouptitle],[ip],[actions],[others]) VALUES (@uid,@username,@groupid,@grouptitle,@ip,@actions,@others)";

            DbParameter[] parms = {
					DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4, uid),
					DbHelper.MakeInParam("@username", (DbType)SqlDbType.VarChar, 50, username),
					DbHelper.MakeInParam("@groupid", (DbType)SqlDbType.Int, 4, groupid),
					DbHelper.MakeInParam("@grouptitle", (DbType)SqlDbType.VarChar, 50, grouptitle),
					DbHelper.MakeInParam("@ip", (DbType)SqlDbType.VarChar, 15, ip),
					DbHelper.MakeInParam("@actions", (DbType)SqlDbType.VarChar, 100, actions),
					DbHelper.MakeInParam("@others", (DbType)SqlDbType.VarChar, 200, others)
				};

            DbHelper.ExecuteNonQuery(CommandType.Text, sqlstring, parms);
        }

        public void DeleteVisitLogs()
        {
            DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE FROM [" + BaseConfigs.GetTablePrefix + "adminvisitlog] ");
        }

        public void DeleteVisitLogs(string condition)
        {
            DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE FROM [" + BaseConfigs.GetTablePrefix + "adminvisitlog] WHERE " + condition);
        }

        /// <summary>
        /// 得到当前指定页数的后台访问日志记录(表)
        /// </summary>
        /// <param name="pagesize">当前分页的尺寸大小</param>
        /// <param name="currentpage">当前页码</param>
        /// <returns></returns>
        public DataTable GetVisitLogList(int pagesize, int currentpage)
        {
            int pagetop = (currentpage - 1) * pagesize;

            if (currentpage == 1)
            {
                return DbHelper.ExecuteDataset(CommandType.Text, "SELECT TOP " + pagesize.ToString() + " * FROM [" + BaseConfigs.GetTablePrefix + "adminvisitlog] ORDER BY [visitid] DESC").Tables[0];
            }
            else
            {
                string sqlstring = "SELECT TOP " + pagesize.ToString() + " * FROM [" + BaseConfigs.GetTablePrefix + "adminvisitlog]  WHERE [visitid] < (SELECT MIN([visitid]) FROM (SELECT TOP " + pagetop + " [visitid] FROM [" + BaseConfigs.GetTablePrefix + "adminvisitlog] ORDER BY [visitid] DESC) AS tblTmp )  ORDER BY [visitid] DESC";
                return DbHelper.ExecuteDataset(CommandType.Text, sqlstring).Tables[0];
            }
        }

        /// <summary>
        /// 得到当前指定条件和页数的后台访问日志记录(表)
        /// </summary>
        /// <param name="pagesize">当前分页的尺寸大小</param>
        /// <param name="currentpage">当前页码</param>
        /// <param name="condition">查询条件</param>
        /// <returns></returns>
        public DataTable GetVisitLogList(int pagesize, int currentpage, string condition)
        {
            int pagetop = (currentpage - 1) * pagesize;

            if (currentpage == 1)
            {
                return DbHelper.ExecuteDataset(CommandType.Text, "SELECT TOP " + pagesize.ToString() + " * FROM [" + BaseConfigs.GetTablePrefix + "adminvisitlog] WHERE " + condition + " ORDER BY [visitid] DESC").Tables[0];
            }
            else
            {
                string sqlstring = "SELECT TOP " + pagesize.ToString() + " * FROM [" + BaseConfigs.GetTablePrefix + "adminvisitlog]  WHERE [visitid] < (SELECT MIN([visitid])  FROM (SELECT TOP " + pagetop + " [visitid] FROM [" + BaseConfigs.GetTablePrefix + "adminvisitlog] WHERE " + condition + " ORDER BY [visitid] DESC) AS tblTmp ) AND " + condition + " ORDER BY [visitid] DESC";
                return DbHelper.ExecuteDataset(CommandType.Text, sqlstring).Tables[0];
            }
        }

        public int GetVisitLogCount()
        {
            return Convert.ToInt32(DbHelper.ExecuteDataset(CommandType.Text, "SELECT COUNT(visitid) FROM [" + BaseConfigs.GetTablePrefix + "adminvisitlog]").Tables[0].Rows[0][0].ToString());
        }

        public int GetVisitLogCount(string condition)
        {
            return Convert.ToInt32(DbHelper.ExecuteDataset(CommandType.Text, "SELECT COUNT(visitid) FROM [" + BaseConfigs.GetTablePrefix + "adminvisitlog] WHERE " + condition).Tables[0].Rows[0][0].ToString());
        }

        public void UpdateForumAndUserTemplateId(string templateidlist)
        {
            DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "forums] SET [templateid]=0 WHERE [templateid] IN(" + templateidlist + ")");
            DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "users] SET [templateid]=0 WHERE [templateid] IN(" + templateidlist + ")");
        }

        public void AddTemplate(string name, string directory, string copyright, string author, string createdate, string ver, string fordntver)
        {
            DbParameter[] parms = { 
                                        DbHelper.MakeInParam("@name", (DbType)SqlDbType.NVarChar, 50, name),
                                        DbHelper.MakeInParam("@directory", (DbType)SqlDbType.NVarChar, 100, directory),
                                        DbHelper.MakeInParam("@copyright", (DbType)SqlDbType.NVarChar, 100, copyright),
                                        DbHelper.MakeInParam("@author", (DbType)SqlDbType.NVarChar, 100, author),
                                        DbHelper.MakeInParam("@createdate", (DbType)SqlDbType.NVarChar, 50, createdate),
                                        DbHelper.MakeInParam("@ver", (DbType)SqlDbType.NVarChar, 100, ver),
                                        DbHelper.MakeInParam("@fordntver", (DbType)SqlDbType.NVarChar, 100, fordntver)
                                    };
            string sql = "INSERT INTO [" + BaseConfigs.GetTablePrefix + "templates] ([name],[directory],[copyright],[author],[createdate],[ver],[fordntver]) VALUES(@name,@directory,@copyright,@author,@createdate,@ver,@fordntver)";

            DbHelper.ExecuteNonQuery(CommandType.Text, sql, parms);
        }

        /// <summary>
        /// 添加新的模板项
        /// </summary>
        /// <param name="templateName">模板名称</param>
        /// <param name="directory">模板文件所在目录</param>
        /// <param name="copyright">模板版权文字</param>
        /// <returns>模板id</returns>
        public int AddTemplate(string templateName, string directory, string copyright)
        {
            DbParameter[] parms = {
				DbHelper.MakeInParam("@templatename", (DbType)SqlDbType.VarChar, 0, templateName),
				DbHelper.MakeInParam("@directory", (DbType)SqlDbType.VarChar, 0, directory),
				DbHelper.MakeInParam("@copyright", (DbType)SqlDbType.VarChar, 0, copyright),

			};
            return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, "INSERT INTO [" + BaseConfigs.GetTablePrefix + "templates]([templatename],[directory],[copyright]) VALUES(@templatename, @directory, @copyright);SELECT SCOPE_IDENTITY()", parms), -1);
        }

        /// <summary>
        /// 删除指定的模板项
        /// </summary>
        /// <param name="templateid">模板id</param>
        public void DeleteTemplateItem(int templateid)
        {
            DbParameter[] parms = {
				DbHelper.MakeInParam("@templateid", (DbType)SqlDbType.Int, 4, templateid)
			};
            DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE FROM [" + BaseConfigs.GetTablePrefix + "templates] WHERE [templateid]=@templateid");
        }

        /// <summary>
        /// 删除指定的模板项列表,
        /// </summary>
        /// <param name="templateidlist">格式为： 1,2,3</param>
        public void DeleteTemplateItem(string templateidlist)
        {
            DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE FROM [" + BaseConfigs.GetTablePrefix + "templates] WHERE [templateid] IN (" + templateidlist + ")");
        }

        /// <summary>
        /// 获得所有在模板目录下的模板列表(即:子目录名称)
        /// </summary>
        /// <param name="templatePath">模板所在路径</param>
        /// <example>GetAllTemplateList(Utils.GetMapPath(@"..\..\templates\"))</example>
        /// <returns>模板列表</returns>
        public DataTable GetAllTemplateList(string templatePath)
        {
            return DbHelper.ExecuteDataset(CommandType.Text, "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "templates] ORDER BY [templateid]").Tables[0];
        }


        public int GetMaxTemplateId()
        {
            return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, "SELECT ISNULL(MAX(templateid), 0) FROM " + BaseConfigs.GetTablePrefix + "templates"), 0);
        }

        /// <summary>
        /// 插入版主管理日志记录
        /// </summary>
        /// <param name="moderatorname">版主名</param>
        /// <param name="grouptitle">所属组的ID</param>
        /// <param name="ip">客户端的IP</param>
        /// <param name="fname">版块的名称</param>
        /// <param name="title">主题的名称</param>
        /// <param name="actions">动作</param>
        /// <param name="reason">原因</param>
        /// <returns></returns>
        public bool InsertModeratorLog(string moderatoruid, string moderatorname, string groupid, string grouptitle, string ip, string postdatetime, string fid, string fname, string tid, string title, string actions, string reason)
        {
            try
            {
                DbParameter[] parms = { 
                                        DbHelper.MakeInParam("@moderatoruid", (DbType)SqlDbType.Int, 4, moderatoruid),
                                        DbHelper.MakeInParam("@moderatorname", (DbType)SqlDbType.NVarChar, 50, moderatorname),
                                        DbHelper.MakeInParam("@groupid", (DbType)SqlDbType.Int, 4, groupid),
                                        DbHelper.MakeInParam("@grouptitle", (DbType)SqlDbType.NVarChar, 50, grouptitle),
                                        DbHelper.MakeInParam("@ip", (DbType)SqlDbType.VarChar, 15, ip),
                                        DbHelper.MakeInParam("@postdatetime", (DbType)SqlDbType.DateTime, 8, DateTime.Parse(postdatetime)),
                                        DbHelper.MakeInParam("@fid", (DbType)SqlDbType.Int, 4, fid),
                                        DbHelper.MakeInParam("@fname", (DbType)SqlDbType.NVarChar, 100, fname),
                                        DbHelper.MakeInParam("@tid", (DbType)SqlDbType.Int, 8, tid),
                                        DbHelper.MakeInParam("@title", (DbType)SqlDbType.VarChar, 200, title),
                                        DbHelper.MakeInParam("@actions", (DbType)SqlDbType.VarChar, 50, actions),
                                        DbHelper.MakeInParam("@reason", (DbType)SqlDbType.NVarChar, 200, reason)
                                    };

                string sqlstring = "INSERT INTO [" + BaseConfigs.GetTablePrefix +
                                   "moderatormanagelog] ([moderatoruid],[moderatorname],[groupid],[grouptitle],[ip],[postdatetime],[fid],[fname],[tid],[title],[actions],[reason]) VALUES (@moderatoruid, @moderatorname, @groupid, @grouptitle,@ip,@postdatetime,@fid,@fname,@tid,@title,@actions,@reason)";
                DbHelper.ExecuteNonQuery(CommandType.Text, sqlstring, parms);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 按指定条件删除日志
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <returns></returns>
        public bool DeleteModeratorLog(string condition)
        {
            try
            {
                DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE FROM [" + BaseConfigs.GetTablePrefix + "moderatormanagelog] WHERE " + condition);
                return true;
            }
            catch
            {
                return false;
            }

        }

        /// <summary>
        /// 得到当前指定条件和页数的前台管理日志记录(表)
        /// </summary>
        /// <param name="pagesize">当前分页的尺寸大小</param>
        /// <param name="currentpage">当前页码</param>
        /// <param name="condition">查询条件</param>
        /// <returns></returns>
        public DataTable GetModeratorLogList(int pagesize, int currentpage, string condition)
        {
            int pagetop = (currentpage - 1) * pagesize;
            string sqlstring;

            if (condition != "") condition = " WHERE " + condition;

            if (currentpage == 1)
            {
                sqlstring = "SELECT TOP " + pagesize.ToString() + " * FROM [" + BaseConfigs.GetTablePrefix + "moderatormanagelog]  " + condition + "  ORDER BY [id] DESC";
            }
            else
            {
                sqlstring = "SELECT TOP " + pagesize.ToString() + " * FROM [" + BaseConfigs.GetTablePrefix + "moderatormanagelog]  WHERE [id] < (SELECT MIN([id])  FROM (SELECT TOP " + pagetop + " [id] FROM [" + BaseConfigs.GetTablePrefix + "moderatormanagelog]  " + condition + " ORDER BY [id] DESC) AS tblTmp ) " + (condition.Replace("WHERE", "") == "" ? "" : "AND " + condition.Replace("WHERE", "")) + " ORDER BY [id] DESC";
            }

            return DbHelper.ExecuteDataset(CommandType.Text, sqlstring).Tables[0];
        }

        /// <summary>
        /// 得到前台管理日志记录数
        /// </summary>
        /// <returns></returns>
        public int GetModeratorLogListCount()
        {
            return Convert.ToInt32(DbHelper.ExecuteDataset(CommandType.Text, "SELECT COUNT([id]) FROM [" + BaseConfigs.GetTablePrefix + "moderatormanagelog]").Tables[0].Rows[0][0].ToString());
        }

        /// <summary>
        /// 得到指定查询条件下的前台管理日志数
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <returns></returns>
        public int GetModeratorLogListCount(string condition)
        {
            return Convert.ToInt32(DbHelper.ExecuteDataset(CommandType.Text, "SELECT COUNT([id]) FROM [" + BaseConfigs.GetTablePrefix + "moderatormanagelog] WHERE " + condition).Tables[0].Rows[0][0].ToString());
        }

        /// <summary>
        /// 删除日志
        /// </summary>
        /// <returns></returns>
        public bool DeleteMedalLog()
        {
            try
            {
                DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE FROM [" + BaseConfigs.GetTablePrefix + "medalslog] ");
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 按指定条件删除日志
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <returns></returns>
        public bool DeleteMedalLog(string condition)
        {
            try
            {
                DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE FROM [" + BaseConfigs.GetTablePrefix + "medalslog] WHERE " + condition);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 得到当前指定页数的勋章日志记录(表)
        /// </summary>
        /// <param name="pagesize">当前分页的尺寸大小</param>
        /// <param name="currentpage">当前页码</param>
        /// <returns></returns>
        public DataTable GetMedalLogList(int pagesize, int currentpage)
        {
            int pagetop = (currentpage - 1) * pagesize;
            string sqlstring;
            if (currentpage == 1)
            {
                sqlstring = "SELECT TOP " + pagesize.ToString() + " * FROM [" + BaseConfigs.GetTablePrefix + "medalslog] ORDER BY [id] DESC";
            }
            else
            {
                sqlstring = "SELECT TOP " + pagesize.ToString() + " * FROM [" + BaseConfigs.GetTablePrefix + "medalslog]  WHERE [id] < (SELECT MIN([id])  FROM (SELECT TOP " + pagetop + " [id] FROM [" + BaseConfigs.GetTablePrefix + "medalslog] ORDER BY [id] DESC) AS tblTmp )  ORDER BY [id] DESC";
            }

            return DbHelper.ExecuteDataset(CommandType.Text, sqlstring).Tables[0];
        }

        /// <summary>
        /// 得到当前指定条件和页数的勋章日志记录(表)
        /// </summary>
        /// <param name="pagesize">当前分页的尺寸大小</param>
        /// <param name="currentpage">当前页码</param>
        /// <param name="condition">查询条件</param>
        /// <returns></returns>
        public DataTable GetMedalLogList(int pagesize, int currentpage, string condition)
        {
            int pagetop = (currentpage - 1) * pagesize;
            string sqlstring;
            if (currentpage == 1)
            {
                sqlstring = "SELECT TOP " + pagesize.ToString() + " * FROM [" + BaseConfigs.GetTablePrefix + "medalslog] WHERE " + condition + " ORDER BY [id] DESC";
            }
            else
            {
                sqlstring = "SELECT TOP " + pagesize.ToString() + " * FROM [" + BaseConfigs.GetTablePrefix + "medalslog]  WHERE [id] < (SELECT MIN([id])  FROM (SELECT TOP " + pagetop + " [id] FROM [" + BaseConfigs.GetTablePrefix + "medalslog] WHERE " + condition + " ORDER BY [id] DESC) AS tblTmp ) AND " + condition + " ORDER BY [id] DESC";
            }

            return DbHelper.ExecuteDataset(CommandType.Text, sqlstring).Tables[0];
        }

        /// <summary>
        /// 得到缓存日志记录数
        /// </summary>
        /// <returns></returns>
        public int GetMedalLogListCount()
        {
            return Convert.ToInt32(DbHelper.ExecuteDataset(CommandType.Text, "SELECT COUNT([id]) FROM [" + BaseConfigs.GetTablePrefix + "medalslog]").Tables[0].Rows[0][0].ToString());
        }

        /// <summary>
        /// 得到指定查询条件下的勋章日志数
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <returns></returns>
        public int GetMedalLogListCount(string condition)
        {
            return Convert.ToInt32(DbHelper.ExecuteDataset(CommandType.Text, "SELECT COUNT([id]) FROM [" + BaseConfigs.GetTablePrefix + "medalslog] WHERE " + condition).Tables[0].Rows[0][0].ToString());
        }


        /// <summary>
        /// 根据IP获取错误登录记录
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public DataTable GetErrLoginRecordByIP(string ip)
        {
            DbParameter[] parms = {
										 DbHelper.MakeInParam("@ip",(DbType)SqlDbType.Char,15, ip),
			                        };
            return DbHelper.ExecuteDataset(CommandType.Text, "SELECT TOP 1 [errcount], [lastupdate] FROM [" + BaseConfigs.GetTablePrefix + "failedlogins] WHERE [ip]=@ip", parms).Tables[0];
        }

        /// <summary>
        /// 增加指定IP的错误记录数
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public int AddErrLoginCount(string ip)
        {
            DbParameter[] parms = {
										 DbHelper.MakeInParam("@ip",(DbType)SqlDbType.Char,15, ip),
			                        };
            return DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "failedlogins] SET [errcount]=[errcount]+1, [lastupdate]=GETDATE() WHERE [ip]=@ip", parms);
        }

        /// <summary>
        /// 增加指定IP的错误记录
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public int AddErrLoginRecord(string ip)
        {
            DbParameter[] parms = {
										 DbHelper.MakeInParam("@ip",(DbType)SqlDbType.Char,15, ip),
			                        };
            return DbHelper.ExecuteNonQuery(CommandType.Text, "INSERT INTO [" + BaseConfigs.GetTablePrefix + "failedlogins] ([ip], [errcount], [lastupdate]) VALUES(@ip, 1, GETDATE())", parms);
        }

        /// <summary>
        /// 将指定IP的错误登录次数重置为1
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public int ResetErrLoginCount(string ip)
        {
            DbParameter[] parms = {
										 DbHelper.MakeInParam("@ip",(DbType)SqlDbType.Char,15, ip),
			                        };
            return DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "failedlogins] SET [errcount]=1, [lastupdate]=GETDATE() WHERE [ip]=@ip", parms);
        }

        /// <summary>
        /// 删除指定IP或者超过15分钟的记录
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public int DeleteErrLoginRecord(string ip)
        {
            DbParameter[] parms = {
										 DbHelper.MakeInParam("@ip",(DbType)SqlDbType.Char,15, ip),
			};
            return DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE FROM [" + BaseConfigs.GetTablePrefix + "failedlogins] WHERE [ip]=@ip OR DATEDIFF(n,[lastupdate], GETDATE()) > 15", parms);
        }

        public int GetPostCount(string posttablename)
        {
            return Convert.ToInt32(DbHelper.ExecuteDataset(CommandType.Text, "SELECT COUNT([pid]) FROM [" + posttablename + "]").Tables[0].Rows[0][0].ToString());
        }

        public DataTable GetPostTableList()
        {
            return DbHelper.ExecuteDataset(CommandType.Text, "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "tablelist]").Tables[0];
        }

        public int UpdateDetachTable(int fid, string description)
        {
            DbParameter[] parms = {
                                        DbHelper.MakeInParam("@fid", (DbType)SqlDbType.Int, 4, fid),
                                        DbHelper.MakeInParam("@description", (DbType)SqlDbType.NVarChar, 50, description)
                                    };
            string sql = "UPDATE [" + BaseConfigs.GetTablePrefix + "tablelist] SET [description]=@description  Where [id]=@fid";
            //fid, description);
            return DbHelper.ExecuteNonQuery(CommandType.Text, sql, parms);
        }

        public int StartFullIndex(string dbname)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("USE " + dbname + ";");
            sb.Append("EXECUTE sp_fulltext_database 'enable';");
            return DbHelper.ExecuteNonQuery(CommandType.Text, sb.ToString());
        }
        public void CreatePostTableAndIndex(string tablename)
        {
            DbHelper.ExecuteNonQuery(CommandType.Text, GetSpecialTableFullIndexSQL(tablename, GetDbName()));
        }


        /// <summary>
        /// 得到指定帖子分表的全文索引建立(填充)语句
        /// </summary>
        /// <param name="tablename"></param>
        /// <returns></returns>
        public string GetSpecialTableFullIndexSQL(string tablename, string dbname)
        {
            #region 建表

            StringBuilder sb = new StringBuilder();
            sb.Append("IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[" + tablename + "]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)  DROP TABLE [" + tablename + "];");
            sb.Append("CREATE TABLE [" + tablename + "] ([pid] [int] NOT NULL ,[fid] [int] NOT NULL ," +
                "[tid] [int] NOT NULL ,[parentid] [int] NOT NULL ,[layer] [int] NOT NULL ,[poster] [nvarchar] (20) NOT NULL ," +
                "[posterid] [int] NOT NULL ,[title] [nvarchar] (80) NOT NULL ,[postdatetime] [smalldatetime] NOT NULL ," +
                "[message] [ntext] NOT NULL ,[ip] [nvarchar] (15) NOT NULL ," +
                "[lastedit] [nvarchar] (50) NOT NULL ,[invisible] [int] NOT NULL ,[usesig] [int] NOT NULL ,[htmlon] [int] NOT NULL ," +
                "[smileyoff] [int] NOT NULL ,[parseurloff] [int] NOT NULL ,[bbcodeoff] [int] NOT NULL ,[attachment] [int] NOT NULL ,[rate] [int] NOT NULL ," +
                "[ratetimes] [int] NOT NULL ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]");
            sb.Append(";");
            sb.Append("ALTER TABLE [" + tablename + "] WITH NOCHECK ADD CONSTRAINT [PK_" + tablename + "] PRIMARY KEY  CLUSTERED ([pid])  ON [PRIMARY]");
            sb.Append(";");

            sb.Append("ALTER TABLE [" + tablename + "] ADD ");
            sb.Append("CONSTRAINT [DF_" + tablename + "_pid] DEFAULT (0) FOR [pid],");
            sb.Append("CONSTRAINT [DF_" + tablename + "_parentid] DEFAULT (0) FOR [parentid],CONSTRAINT [DF_" + tablename + "_layer] DEFAULT (0) FOR [layer],");
            sb.Append("CONSTRAINT [DF_" + tablename + "_poster] DEFAULT ('') FOR [poster],CONSTRAINT [DF_" + tablename + "_posterid] DEFAULT (0) FOR [posterid],");
            sb.Append("CONSTRAINT [DF_" + tablename + "_postdatetime] DEFAULT (getdate()) FOR [postdatetime],CONSTRAINT [DF_" + tablename + "_message] DEFAULT ('') FOR [message],");
            sb.Append("CONSTRAINT [DF_" + tablename + "_ip] DEFAULT ('') FOR [ip],CONSTRAINT [DF_" + tablename + "_lastedit] DEFAULT ('') FOR [lastedit],");
            sb.Append("CONSTRAINT [DF_" + tablename + "_invisible] DEFAULT (0) FOR [invisible],CONSTRAINT [DF_" + tablename + "_usesig] DEFAULT (0) FOR [usesig],");
            sb.Append("CONSTRAINT [DF_" + tablename + "_htmlon] DEFAULT (0) FOR [htmlon],CONSTRAINT [DF_" + tablename + "_smileyoff] DEFAULT (0) FOR [smileyoff],");
            sb.Append("CONSTRAINT [DF_" + tablename + "_parseurloff] DEFAULT (0) FOR [parseurloff],CONSTRAINT [DF_" + tablename + "_bbcodeoff] DEFAULT (0) FOR [bbcodeoff],");
            sb.Append("CONSTRAINT [DF_" + tablename + "_attachment] DEFAULT (0) FOR [attachment],CONSTRAINT [DF_" + tablename + "_rate] DEFAULT (0) FOR [rate],");
            sb.Append("CONSTRAINT [DF_" + tablename + "_ratetimes] DEFAULT (0) FOR [ratetimes]");

            sb.Append(";");
            sb.Append("CREATE  INDEX [parentid] ON [" + tablename + "]([parentid]) ON [PRIMARY]");
            sb.Append(";");

            sb.Append("CREATE  UNIQUE  INDEX [showtopic] ON [" + tablename + "]([tid], [invisible], [pid]) ON [PRIMARY]");
            sb.Append(";");


            sb.Append("CREATE  INDEX [treelist] ON [" + tablename + "]([tid], [invisible], [parentid]) ON [PRIMARY]");
            sb.Append(";");

            #endregion

            #region 建全文索引

            sb.Append("USE " + dbname + " \r\n");
            sb.Append("EXECUTE sp_fulltext_database 'enable'; \r\n");
            sb.Append("IF(SELECT DATABASEPROPERTY('[" + dbname + "]','isfulltextenabled'))=0  EXECUTE sp_fulltext_database 'enable';");
            sb.Append("IF EXISTS (SELECT * FROM sysfulltextcatalogs WHERE name ='pk_" + tablename + "_msg')  EXECUTE sp_fulltext_catalog 'pk_" + tablename + "_msg','drop';");
            sb.Append("IF EXISTS (SELECT * FROM sysfulltextcatalogs WHERE name ='pk_" + tablename + "_msg')  EXECUTE sp_fulltext_table '[" + tablename + "]', 'drop' ;");
            sb.Append("EXECUTE sp_fulltext_catalog 'pk_" + tablename + "_msg','create';");
            sb.Append("EXECUTE sp_fulltext_table '[" + tablename + "]','create','pk_" + tablename + "_msg','pk_" + tablename + "';");
            sb.Append("EXECUTE sp_fulltext_column '[" + tablename + "]','message','add';");
            sb.Append("EXECUTE sp_fulltext_table '[" + tablename + "]','activate';");
            sb.Append("EXECUTE sp_fulltext_catalog 'pk_" + tablename + "_msg','start_full';");

            #endregion

            return sb.ToString();
        }

        public void AddPostTableToTableList(string description, int mintid, int maxtid)
        {
            DbParameter[] parms = { 
                                        DbHelper.MakeInParam("@description", (DbType)SqlDbType.NVarChar, 50, description),
                                        DbHelper.MakeInParam("@mintid", (DbType)SqlDbType.Int, 4, mintid),
                                        DbHelper.MakeInParam("@maxtid", (DbType)SqlDbType.Int, 4, maxtid)
                                    };
            DbHelper.ExecuteNonQuery(CommandType.Text, "INSERT INTO [" + BaseConfigs.GetTablePrefix + "tablelist] ([description],[mintid],[maxtid]) VALUES(@description, @mintid, @maxtid)",parms);
        }

        public void CreatePostProcedure(string sqltemplate)
        {
            foreach (string sql in sqltemplate.Split('~'))
            {
                DbHelper.ExecuteNonQuery(CommandType.Text, sql);
            }
        }

        public DataTable GetMaxTid()
        {
            return DbHelper.ExecuteDataset(CommandType.Text, "SELECT MAX([tid]) FROM [" + BaseConfigs.GetTablePrefix + "topics]").Tables[0];
        }

        public DataTable GetPostCountFromIndex(string postsid)
        {
            return DbHelper.ExecuteDataset(CommandType.Text, "SELECT TOP 1 [rows] FROM [sysindexes] WHERE [name]='PK_" + BaseConfigs.GetTablePrefix + "posts" + postsid + "'").Tables[0];
        }

        public DataTable GetPostCountTable(string postsid)
        {
            return DbHelper.ExecuteDataset(CommandType.Text, "SELECT COUNT(pid) FROM [" + BaseConfigs.GetTablePrefix + "posts" + postsid + "]").Tables[0];
        }

        public void TestFullTextIndex(int posttableid)
        {
            DbHelper.ExecuteNonQuery(CommandType.Text, "SELECT TOP 1 [pid] FROM [" + BaseConfigs.GetTablePrefix + "posts" + posttableid + "] WHERE CONTAINS([message],'asd') ORDER BY [pid] ASC");
        }

        public DataRowCollection GetRateRange(int scoreid)
        {
            return DbHelper.ExecuteDataset(CommandType.Text, "SELECT [groupid], [raterange] FROM [" +
                                                     BaseConfigs.GetTablePrefix +
                                                     "usergroups] WHERE [raterange] LIKE '%" + scoreid + ",True,%'").
                            Tables[0].Rows;
        }

        public void UpdateRateRange(string raterange, int groupid)
        {
            DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix +
                                                  "usergroups] SET [raterange]='" +
                                                  raterange +
                                                  "' WHERE [groupid]=" + groupid);
        }

        public int GetMaxTableListId()
        {
            return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, "SELECT ISNULL(MAX([id]), 0) FROM " + BaseConfigs.GetTablePrefix + "tablelist"), 0);
        }

        public int GetMaxPostTableTid(string posttablename)
        {
            return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, "SELECT ISNULL(MAX([tid]), 0) FROM " + posttablename), 0) + 1;
        }

        public int GetMinPostTableTid(string posttablename)
        {
            return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, "SELECT ISNULL(MIN([tid]), 0) FROM " + posttablename), 0) + 1;
        }

        public void AddAdInfo(int available, string type, int displayorder, string title, string targets, string parameters, string code, string starttime, string endtime)
        {
            DbParameter[] parms = { 
                                        DbHelper.MakeInParam("@available", (DbType)SqlDbType.Int, 4, available),
                                        DbHelper.MakeInParam("@type", (DbType)SqlDbType.NVarChar, 50, type),
                                        DbHelper.MakeInParam("@displayorder", (DbType)SqlDbType.Int, 4, displayorder),
                                        DbHelper.MakeInParam("@title", (DbType)SqlDbType.NVarChar, 50, title),
                                        DbHelper.MakeInParam("@targets", (DbType)SqlDbType.NVarChar, 255, targets),
                                        DbHelper.MakeInParam("@parameters", (DbType)SqlDbType.NText, 0, parameters),
                                        DbHelper.MakeInParam("@code", (DbType)SqlDbType.NText, 0, code),
                                        DbHelper.MakeInParam("@starttime", (DbType)SqlDbType.DateTime, 8, DateTime.Parse(starttime)),
                                        DbHelper.MakeInParam("@endtime", (DbType)SqlDbType.DateTime, 8, DateTime.Parse(endtime))                                        
                                    };
            string sql = "INSERT INTO  [" + BaseConfigs.GetTablePrefix + "advertisements] ([available],[type],[displayorder],[title],[targets],[parameters],[code],[starttime],[endtime]) VALUES(@available,@type,@displayorder,@title,@targets,@parameters,@code,@starttime,@endtime)";

            DbHelper.ExecuteNonQuery(CommandType.Text, sql, parms);
        }

        public string GetAdvertisements()
        {
            return "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "advertisements] ORDER BY [advid] ASC";
        }

        public DataRowCollection GetTargetsForumName(string targets)
        {
            return DbHelper.ExecuteDataset(CommandType.Text, "SELECT [name] FROM [" + BaseConfigs.GetTablePrefix + "forums] WHERE [fid] IN(0" + targets + "0)").Tables[0].Rows;
        }

        public int UpdateAdvertisementAvailable(string aidlist, int available)
        {
            DbParameter[] parms = { 
                                        DbHelper.MakeInParam("@available", (DbType)SqlDbType.Int, 4, available)
                                    };
            string sql = "UPDATE [" + BaseConfigs.GetTablePrefix + "advertisements] SET [available]=@available  WHERE [advid] IN(" + aidlist + ")";

            return DbHelper.ExecuteNonQuery(CommandType.Text, sql, parms);
        }

        public int UpdateAdvertisement(int aid, int available, string type, int displayorder, string title, string targets, string parameters, string code, string starttime, string endtime)
        {
            DbParameter[] parms = {
                                        DbHelper.MakeInParam("@aid", (DbType)SqlDbType.Int, 4, aid),
                                        DbHelper.MakeInParam("@available", (DbType)SqlDbType.Int, 4, available),
                                        DbHelper.MakeInParam("@type", (DbType)SqlDbType.NVarChar, 50, type),
                                        DbHelper.MakeInParam("@displayorder", (DbType)SqlDbType.Int, 4, displayorder),
                                        DbHelper.MakeInParam("@title", (DbType)SqlDbType.NVarChar, 50, title),
                                        DbHelper.MakeInParam("@targets", (DbType)SqlDbType.NVarChar, 255, targets),
                                        DbHelper.MakeInParam("@parameters", (DbType)SqlDbType.NText, 0, parameters),
                                        DbHelper.MakeInParam("@code", (DbType)SqlDbType.NText, 0, code),
                                        DbHelper.MakeInParam("@starttime", (DbType)SqlDbType.DateTime, 8, DateTime.Parse(starttime)),
                                        DbHelper.MakeInParam("@endtime", (DbType)SqlDbType.DateTime, 8, DateTime.Parse(endtime))
                                    };
            string sql = "UPDATE [" + BaseConfigs.GetTablePrefix + "advertisements] SET [available]=@available,[type]=@type, [displayorder]=@displayorder,[title]=@title,[targets]=@targets,[parameters]=@parameters,[code]=@code,[starttime]=@starttime,[endtime]=@endtime WHERE [advid]=@aid";

            return DbHelper.ExecuteNonQuery(CommandType.Text, sql, parms);
        }

        public void DeleteAdvertisement(string aidlist)
        {
            string sql = "DELETE FROM [" + BaseConfigs.GetTablePrefix + "advertisements] WHERE [advid] IN (" + aidlist + ")";
            DbHelper.ExecuteNonQuery(CommandType.Text, sql);
        }

        public void BuyTopic(int uid, int tid, int posterid, int price, float netamount, int creditsTrans)
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@uid",(DbType)SqlDbType.Int,4,uid),
									   DbHelper.MakeInParam("@tid",(DbType)SqlDbType.Int,4,tid),
									   DbHelper.MakeInParam("@authorid",(DbType)SqlDbType.Int,4,posterid),
									   DbHelper.MakeInParam("@buydate",(DbType)SqlDbType.DateTime,4,DateTime.Now),
									   DbHelper.MakeInParam("@amount",(DbType)SqlDbType.Int,4,price),
									   DbHelper.MakeInParam("@netamount",(DbType)SqlDbType.Float,8,netamount)
								   };

            DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "users] SET [extcredits" + creditsTrans + "] = [extcredits" + creditsTrans + "] - " + price.ToString() + " WHERE [uid] = @uid", parms);
            DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "users] SET [extcredits" + creditsTrans + "] = [extcredits" + creditsTrans + "] + @netamount WHERE [uid] = @authorid", parms);
        }

        public int AddPaymentLog(int uid, int tid, int posterid, int price, float netamount)
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@uid",(DbType)SqlDbType.Int,4,uid),
									   DbHelper.MakeInParam("@tid",(DbType)SqlDbType.Int,4,tid),
									   DbHelper.MakeInParam("@authorid",(DbType)SqlDbType.Int,4,posterid),
									   DbHelper.MakeInParam("@buydate",(DbType)SqlDbType.DateTime,4,DateTime.Now),
									   DbHelper.MakeInParam("@amount",(DbType)SqlDbType.Int,4,price),
									   DbHelper.MakeInParam("@netamount",(DbType)SqlDbType.Float,8,netamount)
								   };
            return DbHelper.ExecuteNonQuery(CommandType.Text, "INSERT INTO [" + BaseConfigs.GetTablePrefix + "paymentlog] ([uid],[tid],[authorid],[buydate],[amount],[netamount]) VALUES(@uid,@tid,@authorid,@buydate,@amount,@netamount)", parms);
        }

        /// <summary>
        /// 判断用户是否已购买主题
        /// </summary>
        /// <param name="tid">主题id</param>
        /// <param name="uid">用户id</param>
        /// <returns></returns>
        public bool IsBuyer(int tid, int uid)
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@uid",(DbType)SqlDbType.Int,4,uid),
									   DbHelper.MakeInParam("@tid",(DbType)SqlDbType.Int,4,tid)
								   };
            return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, "SELECT [id] FROM [" + BaseConfigs.GetTablePrefix + "paymentlog] WHERE [tid] = @tid AND [uid]=@uid", parms), 0) > 0;
        }

        public DataTable GetPayLogInList(int pagesize, int currentpage, int uid)
        {
            int pagetop = (currentpage - 1) * pagesize;
            string sqlstring;
            if (currentpage == 1)
            {
                sqlstring = "SELECT TOP " + pagesize.ToString() + " [" + BaseConfigs.GetTablePrefix + "paymentlog].*, [" + BaseConfigs.GetTablePrefix + "topics].[fid] AS fid ,[" + BaseConfigs.GetTablePrefix + "topics].[postdatetime] AS postdatetime ,[" + BaseConfigs.GetTablePrefix + "topics].[poster] AS authorname, [" + BaseConfigs.GetTablePrefix + "topics].[title] AS title,[" + BaseConfigs.GetTablePrefix + "users].[username] AS UserName FROM [" + BaseConfigs.GetTablePrefix + "paymentlog] LEFT OUTER JOIN [" + BaseConfigs.GetTablePrefix + "topics] ON [" + BaseConfigs.GetTablePrefix + "paymentlog].[tid] = [" + BaseConfigs.GetTablePrefix + "topics].[tid] LEFT OUTER JOIN [" + BaseConfigs.GetTablePrefix + "users] ON [" + BaseConfigs.GetTablePrefix + "users].[uid] = [" + BaseConfigs.GetTablePrefix + "paymentlog].[uid] WHERE [" + BaseConfigs.GetTablePrefix + "paymentlog].[authorid]=" + uid + "  ORDER BY [id] DESC";
            }
            else
            {
                sqlstring = "SELECT TOP " + pagesize.ToString() + "[ " + BaseConfigs.GetTablePrefix + "paymentlog].*, [" + BaseConfigs.GetTablePrefix + "topics].[fid] AS fid ,[" + BaseConfigs.GetTablePrefix + "topics].[postdatetime] AS postdatetime ,[" + BaseConfigs.GetTablePrefix + "topics].[poster] AS authorname, [" + BaseConfigs.GetTablePrefix + "topics].[title] AS title,[" + BaseConfigs.GetTablePrefix + "users].[username] AS UserName FROM [" + BaseConfigs.GetTablePrefix + "paymentlog] LEFT OUTER JOIN [" + BaseConfigs.GetTablePrefix + "topics] ON [" + BaseConfigs.GetTablePrefix + "paymentlog].[tid] = [" + BaseConfigs.GetTablePrefix + "topics].[tid] LEFT OUTER JOIN [" + BaseConfigs.GetTablePrefix + "users] ON [" + BaseConfigs.GetTablePrefix + "users].[uid] = [" + BaseConfigs.GetTablePrefix + "paymentlog].[uid] WHERE [id] < (SELECT MIN([id]) FROM (SELECT TOP " + pagetop + " [id] FROM [" + BaseConfigs.GetTablePrefix + "paymentlog] WHERE [" + BaseConfigs.GetTablePrefix + "paymentlog].[authorid]=" + uid + " ORDER BY [id] DESC) AS tblTmp ) AND [" + BaseConfigs.GetTablePrefix + "paymentlog].[authorid]=" + uid + " ORDER BY [" + BaseConfigs.GetTablePrefix + "paymentlog].[id] DESC";
            }

            DataTable dt = DbHelper.ExecuteDataset(CommandType.Text, sqlstring).Tables[0];
            return dt;
        }

        /// <summary>
        /// 获取指定用户的收入日志记录数
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <returns></returns>
        public int GetPaymentLogInRecordCount(int uid)
        {
            return Convert.ToInt32(DbHelper.ExecuteDataset(CommandType.Text, "SELECT COUNT([id]) FROM [" + BaseConfigs.GetTablePrefix + "paymentlog] WHERE [authorid]=" + uid).Tables[0].Rows[0][0].ToString());
        }

        /// <summary>
        /// 返回指定用户的支出日志记录数
        /// </summary>
        /// <param name="pagesize">每页记录数</param>
        /// <param name="currentpage">当前页</param>
        /// <param name="uid">用户id</param>
        /// <returns></returns>
        public DataTable GetPayLogOutList(int pagesize, int currentpage, int uid)
        {
            int pagetop = (currentpage - 1) * pagesize;
            string sqlstring;
            if (currentpage == 1)
            {
                sqlstring = "SELECT TOP " + pagesize.ToString() + " [" + BaseConfigs.GetTablePrefix + "paymentlog].*, [" + BaseConfigs.GetTablePrefix + "topics].[fid] AS fid ,[" + BaseConfigs.GetTablePrefix + "topics].[postdatetime] AS postdatetime ,[" + BaseConfigs.GetTablePrefix + "topics].[poster] AS authorname, [" + BaseConfigs.GetTablePrefix + "topics].[title] AS title,[" + BaseConfigs.GetTablePrefix + "users].[username] AS UserName FROM [" + BaseConfigs.GetTablePrefix + "paymentlog] LEFT OUTER JOIN [" + BaseConfigs.GetTablePrefix + "topics] ON [" + BaseConfigs.GetTablePrefix + "paymentlog].[tid] = [" + BaseConfigs.GetTablePrefix + "topics].[tid] LEFT OUTER JOIN [" + BaseConfigs.GetTablePrefix + "users] ON [" + BaseConfigs.GetTablePrefix + "users].[uid] = [" + BaseConfigs.GetTablePrefix + "paymentlog].[uid] WHERE [" + BaseConfigs.GetTablePrefix + "paymentlog].[uid]=" + uid + "  ORDER BY [id] DESC";
            }
            else
            {
                sqlstring = "SELECT TOP " + pagesize.ToString() + " [" + BaseConfigs.GetTablePrefix + "paymentlog].*, [" + BaseConfigs.GetTablePrefix + "topics].[fid] AS fid ,[" + BaseConfigs.GetTablePrefix + "topics].[postdatetime] AS postdatetime ,[" + BaseConfigs.GetTablePrefix + "topics].[poster] AS authorname, [" + BaseConfigs.GetTablePrefix + "topics].[title] AS title,[" + BaseConfigs.GetTablePrefix + "users].[username] AS UserName FROM [" + BaseConfigs.GetTablePrefix + "paymentlog] LEFT OUTER JOIN [" + BaseConfigs.GetTablePrefix + "topics] ON [" + BaseConfigs.GetTablePrefix + "paymentlog].[tid] = [" + BaseConfigs.GetTablePrefix + "topics].[tid] LEFT OUTER JOIN [" + BaseConfigs.GetTablePrefix + "users] ON [" + BaseConfigs.GetTablePrefix + "users].[uid] = [" + BaseConfigs.GetTablePrefix + "paymentlog].[uid] WHERE [id] < (SELECT MIN([id])  FROM (SELECT TOP " + pagetop + " [id] FROM [" + BaseConfigs.GetTablePrefix + "paymentlog] WHERE [" + BaseConfigs.GetTablePrefix + "paymentlog].[uid]=" + uid + " ORDER BY [id] DESC) AS tblTmp ) AND [" + BaseConfigs.GetTablePrefix + "paymentlog].[uid]=" + uid + " ORDER BY [" + BaseConfigs.GetTablePrefix + "paymentlog].[id] DESC";
            }

            DataTable dt = DbHelper.ExecuteDataset(CommandType.Text, sqlstring).Tables[0];
            return dt;
        }

        /// <summary>
        /// 返回指定用户支出日志总数
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <returns></returns>
        public int GetPaymentLogOutRecordCount(int uid)
        {
            return Convert.ToInt32(DbHelper.ExecuteDataset(CommandType.Text, "SELECT COUNT([id]) FROM [" + BaseConfigs.GetTablePrefix + "paymentlog] WHERE [uid]=" + uid).Tables[0].Rows[0][0].ToString());
        }

        /// <summary>
        /// 获取指定主题的购买记录
        /// </summary>
        /// <param name="pagesize">每页记录数</param>
        /// <param name="currentpage">当前页数</param>
        /// <param name="tid">主题id</param>
        /// <returns></returns>
        public DataTable GetPaymentLogByTid(int pagesize, int currentpage, int tid)
        {
            int pagetop = (currentpage - 1) * pagesize;
            string sqlstring;
            if (currentpage == 1)
            {
                sqlstring = "SELECT TOP " + pagesize.ToString() + " [" + BaseConfigs.GetTablePrefix + "paymentlog].*, [" + BaseConfigs.GetTablePrefix + "users].[username] AS username FROM [" + BaseConfigs.GetTablePrefix + "paymentlog] LEFT OUTER JOIN [" + BaseConfigs.GetTablePrefix + "topics] ON [" + BaseConfigs.GetTablePrefix + "paymentlog].[tid] = [" + BaseConfigs.GetTablePrefix + "topics].[tid] LEFT OUTER JOIN [" + BaseConfigs.GetTablePrefix + "users] ON [" + BaseConfigs.GetTablePrefix + "users].[uid] = [" + BaseConfigs.GetTablePrefix + "paymentlog].[uid] WHERE [" + BaseConfigs.GetTablePrefix + "paymentlog].[tid]=" + tid + "  ORDER BY [id] DESC";
            }
            else
            {
                sqlstring = "SELECT TOP " + pagesize.ToString() + " [" + BaseConfigs.GetTablePrefix + "paymentlog].*, [" + BaseConfigs.GetTablePrefix + "users].[username] AS username FROM [" + BaseConfigs.GetTablePrefix + "paymentlog] LEFT OUTER JOIN [" + BaseConfigs.GetTablePrefix + "topics] ON [" + BaseConfigs.GetTablePrefix + "paymentlog].[tid] = [" + BaseConfigs.GetTablePrefix + "topics].[tid] LEFT OUTER JOIN [" + BaseConfigs.GetTablePrefix + "users] ON [" + BaseConfigs.GetTablePrefix + "users].[uid] = [" + BaseConfigs.GetTablePrefix + "paymentlog].[uid] WHERE [id] < (SELECT MIN([id])  FROM (SELECT TOP " + pagetop + " [id] FROM [" + BaseConfigs.GetTablePrefix + "paymentlog] WHERE [" + BaseConfigs.GetTablePrefix + "paymentlog].[tid]=" + tid + " ORDER BY [id] DESC) AS tblTmp ) AND [" + BaseConfigs.GetTablePrefix + "paymentlog].[tid]=" + tid + " ORDER BY [" + BaseConfigs.GetTablePrefix + "paymentlog].[id] DESC";
            }

            DataTable dt = DbHelper.ExecuteDataset(CommandType.Text, sqlstring).Tables[0];
            return dt;
        }

        /// <summary>
        /// 主题购买总次数
        /// </summary>
        /// <param name="tid">主题id</param>
        /// <returns></returns>
        public int GetPaymentLogByTidCount(int tid)
        {
            return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, "SELECT COUNT([id]) FROM [" + BaseConfigs.GetTablePrefix + "paymentlog] WHERE [tid]=" + tid), 0);
        }


        public void AddSmiles(int id, int displayorder, int type, string code, string url)
        {
            DbParameter[] parms = {
                                        DbHelper.MakeInParam("@id", (DbType)SqlDbType.Int, 4, id),
                                        DbHelper.MakeInParam("@displayorder", (DbType)SqlDbType.Int, 4, displayorder),
                                        DbHelper.MakeInParam("@type", (DbType)SqlDbType.Int, 4, type),
                                        DbHelper.MakeInParam("@code", (DbType)SqlDbType.NVarChar, 30, code),
                                        DbHelper.MakeInParam("@url", (DbType)SqlDbType.VarChar, 60, url)
                                    };


            string sql = "INSERT INTO [" + BaseConfigs.GetTablePrefix + "smilies] ([id],[displayorder],[type],[code],[url]) Values (@id,@displayorder,@type,@code,@url)";

            DbHelper.ExecuteNonQuery(CommandType.Text, sql, parms);
        }

        public string GetIcons()
        {
            return "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "smilies] WHERE [type]=1";
        }

        public string DeleteSmily(int id)
        {
            return "DELETE FROM [" + BaseConfigs.GetTablePrefix + "smilies] WHERE [id]=" + id;
        }

        public void DeleteSmilyByType(int type)
        {
            string sql = "DELETE FROM [" + BaseConfigs.GetTablePrefix + "smilies] WHERE [type]=" + type;
            DbHelper.ExecuteNonQuery(CommandType.Text,sql);
        }

        public int UpdateSmilies(int id, int displayorder, int type, string code, string url)
        {
            DbParameter[] parms = { 
                                        DbHelper.MakeInParam("@id", (DbType)SqlDbType.Int, 4, id),
                                        DbHelper.MakeInParam("@displayorder", (DbType)SqlDbType.Int, 4, displayorder),
                                        DbHelper.MakeInParam("@type", (DbType)SqlDbType.Int, 4, type),
                                        DbHelper.MakeInParam("@code", (DbType)SqlDbType.NVarChar, 30, code),
                                        DbHelper.MakeInParam("@url", (DbType)SqlDbType.VarChar, 60, url)
                                    };
            string sql = "UPDATE [" + BaseConfigs.GetTablePrefix + "smilies] SET [displayorder]=@displayorder,[type]=@type,[code]=@code,[url]=@url Where [id]=@id";

            return DbHelper.ExecuteNonQuery(CommandType.Text, sql, parms);
        }

        public int UpdateSmiliesPart(string code, int displayorder, int id)
        {
            DbParameter[] parms = { 
                                        DbHelper.MakeInParam("@id", (DbType)SqlDbType.Int, 4, id),
                                        DbHelper.MakeInParam("@displayorder", (DbType)SqlDbType.Int, 4, displayorder),
                                        DbHelper.MakeInParam("@code", (DbType)SqlDbType.NVarChar, 30, code)
                                    };
            string sql = "UPDATE [" + BaseConfigs.GetTablePrefix + "smilies] SET [code]=@code,[displayorder]=@displayorder WHERE [id]=@id";
            return DbHelper.ExecuteNonQuery(CommandType.Text, sql, parms);
        }

        public int DeleteSmilies(string idlist)
        {
            return DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE FROM [" + BaseConfigs.GetTablePrefix + "smilies]  WHERE [ID] IN(" + idlist + ")");
        }

        public string GetSmilies()
        {
            return "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "smilies] WHERE [type]=0";
        }

        public int GetMaxSmiliesId()
        {
            return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, "SELECT ISNULL(MAX(id), 0) FROM " + BaseConfigs.GetTablePrefix + "smilies"), 0) + 1;
        }

        public DataTable GetSmiliesInfoByType(int type)
        {
            DbParameter parm = DbHelper.MakeInParam("@type", (DbType)SqlDbType.Int, 4, type);
            string sql = "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "smilies] WHERE [type]=@type";
            return DbHelper.ExecuteDataset(CommandType.Text, sql, parm).Tables[0];
        }


        /// <summary>
        /// 得到表情符数据
        /// </summary>
        /// <returns>表情符数据</returns>
        public IDataReader GetSmiliesList()
        {
            IDataReader dr = DbHelper.ExecuteReader(System.Data.CommandType.Text, "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "smilies] WHERE [type]=0 ORDER BY [displayorder] DESC,[id] ASC");
            return dr;
        }

        /// <summary>
        /// 得到表情符数据
        /// </summary>
        /// <returns>表情符表</returns>
        public DataTable GetSmiliesListDataTable()
        {
            DataSet ds = DbHelper.ExecuteDataset(System.Data.CommandType.Text, "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "smilies] ORDER BY [type],[displayorder],[id]");
            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    return ds.Tables[0];
                }
            }
            return new DataTable();
        }

        /// <summary>
        /// 得到不带分类的表情符数据
        /// </summary>
        /// <returns>表情符表</returns>
        public DataTable GetSmiliesListWithoutType()
        {
            DataSet ds = DbHelper.ExecuteDataset(System.Data.CommandType.Text, "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "smilies] WHERE [type]<>0 ORDER BY [type],[displayorder],[id]");
            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    return ds.Tables[0];
                }
            }
            return new DataTable();
        }

        /// <summary>
        /// 获得表情分类列表
        /// </summary>
        /// <returns></returns>
        public DataTable GetSmilieTypes()
        {
            DataSet ds = DbHelper.ExecuteDataset(System.Data.CommandType.Text, "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "smilies] WHERE [type]=0 ORDER BY [displayorder],[id]");
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            return new DataTable();
        }

        public DataRow GetSmilieTypeById(string id)
        {
            DataSet ds = DbHelper.ExecuteDataset(System.Data.CommandType.Text, "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "smilies] WHERE [id]=" + id);
            if (ds != null && ds.Tables[0].Rows.Count == 1)
                return ds.Tables[0].Rows[0];
            else
                return null;
        }
        /// <summary>
        /// 获得统计列
        /// </summary>
        /// <returns>统计列</returns>
        public DataRow GetStatisticsRow()
        {
            return DbHelper.ExecuteDataset(CommandType.Text, "SELECT TOP 1 * FROM [" + BaseConfigs.GetTablePrefix + "statistics]").Tables[0].Rows[0];
        }

        /// <summary>
        /// 将缓存中的统计列保存到数据库
        /// </summary>
        public void SaveStatisticsRow(DataRow dr)
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@totaltopic", (DbType)SqlDbType.Int,4,Int32.Parse(dr["totaltopic"].ToString())),
									   DbHelper.MakeInParam("@totalpost",(DbType)SqlDbType.Int,4,Int32.Parse(dr["totalpost"].ToString())),
									   DbHelper.MakeInParam("@totalusers",(DbType)SqlDbType.Int,4,Int32.Parse(dr["totalusers"].ToString())),
									   DbHelper.MakeInParam("@lastusername",(DbType)SqlDbType.NChar,20,dr["totalusers"].ToString()),
									   DbHelper.MakeInParam("@lastuserid",(DbType)SqlDbType.Int,4,Int32.Parse(dr["highestonlineusercount"].ToString())),
									   DbHelper.MakeInParam("@highestonlineusercount",(DbType)SqlDbType.Int,4,Int32.Parse(dr["highestonlineusercount"].ToString())),
									   DbHelper.MakeInParam("@highestonlineusertime",(DbType)SqlDbType.SmallDateTime,4,DateTime.Parse(dr["highestonlineusertime"].ToString())),
									   DbHelper.MakeInParam("@highestposts",(DbType)SqlDbType.Int,4,Int32.Parse(dr["highestposts"].ToString())),
									   DbHelper.MakeInParam("@highestpostsdate",(DbType)SqlDbType.Char,10,dr["highestpostsdate"].ToString())
			};
            DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "statistics] SET [totaltopic]=@totaltopic,[totalpost]=@totalpost, [totalusers]=@totalusers, [lastusername]=@lastusername, [lastuserid]=@lastuserid, [highestonlineusercount]=@highestonlineusercount, [highestonlineusertime]=@highestonlineusertime, [highestposts]=@highestposts, [highestpostsdate]=@highestpostsdate", parms);
        }

        public void UpdateYesterdayPosts(string posttableid)
        {
            int max = Convert.ToInt32(posttableid);
            int min = max > 4 ? (max - 3) : 1;
            

            StringBuilder builder = new StringBuilder();

            for (int i = max; i >= min; i--)
            {
                if (i < max)
                    builder.Append(" UNION ");
                builder.AppendFormat("SELECT COUNT(1) AS [c] FROM [{0}posts{1}] WHERE [postdatetime] < '{2}' AND [postdatetime] > '{3}' AND [invisible]=0", BaseConfigs.GetTablePrefix, i.ToString(), DateTime.Now.ToString("yyyy-MM-dd"), DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd"));
            }
            string commandText = string.Format("SELECT SUM([c]) FROM ({0})t", builder.ToString());
            int yesterdayposts = Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, commandText), 0);



            int highestposts = Utils.StrToInt(GetStatisticsRow()["highestposts"], 0);
            //int yesterdayposts = Utils.StrToInt(GetStatisticsRow()["yesterdayposts"], 0);
            builder.Remove(0, builder.Length);
            builder.AppendFormat("UPDATE [{0}statistics] SET [yesterdayposts]=", BaseConfigs.GetTablePrefix);
            builder.Append(yesterdayposts.ToString());
            if (yesterdayposts > highestposts)
            {
                builder.Append(", [highestposts]=");
                builder.Append(yesterdayposts.ToString());
                builder.Append(", [highestpostsdate]='");
                builder.Append(DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd"));
                builder.Append("'");
            }

            DbHelper.ExecuteNonQuery(builder.ToString());

        }

        public IDataReader GetAllForumStatistics()
        {
            return DbHelper.ExecuteReader(CommandType.Text, string.Format("SELECT SUM([topics]) AS [topiccount],SUM([posts]) AS [postcount],SUM([todayposts])-(SELECT SUM([todayposts]) FROM [{0}forums] WHERE [lastpost] < CONVERT(CHAR(12),GETDATE(),101) AND [layer]=1) AS [todaypostcount] FROM [{0}forums] WHERE [layer]=1", BaseConfigs.GetTablePrefix));
        }

        public IDataReader GetForumStatistics(int fid)
        {
            DbParameter[] parms = { DbHelper.MakeInParam("@fid", (DbType)SqlDbType.Int,4, fid) };
            return DbHelper.ExecuteReader(CommandType.Text, string.Format("SELECT SUM([topics]) AS [topiccount],SUM([posts]) AS [postcount],SUM([todayposts])-(SELECT SUM([todayposts]) FROM [{0}forums] WHERE [lastpost] < CONVERT(CHAR(12),GETDATE(),101) AND [layer]=1 AND [fid] = @fid) AS [todaypostcount] FROM [{0}forums] WHERE [fid] = @fid AND [layer]=1", BaseConfigs.GetTablePrefix), parms);
        }

        /// <summary>
        /// 更新指定名称的统计项
        /// </summary>
        /// <param name="param">项目名称</param>
        /// <param name="Value">指定项的值</param>
        /// <returns>更新数</returns>
        public int UpdateStatistics(string param, int intValue)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            if (!param.Equals(""))
            {
                sb.Append("UPDATE [" + BaseConfigs.GetTablePrefix + "statistics] SET ");
                sb.Append(param);
                sb.Append(" = ");
                sb.Append(intValue);
            }
            return DbHelper.ExecuteNonQuery(CommandType.Text, sb.ToString());
        }

        /// <summary>
        /// 更新指定名称的统计项
        /// </summary>
        /// <param name="param">项目名称</param>
        /// <param name="Value">指定项的值</param>
        /// <returns>更新数</returns>
        public int UpdateStatistics(string param, string strValue)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            if (!param.Equals(""))
            {
                sb.Append("UPDATE [" + BaseConfigs.GetTablePrefix + "statistics] SET ");
                sb.Append(param);
                sb.Append(" = '");
                sb.Append(strValue);
                sb.Append("'");
            }
            return DbHelper.ExecuteNonQuery(CommandType.Text, sb.ToString());
        }

        /// <summary>
        /// 获得前台有效的模板列表
        /// </summary>
        /// <returns></returns>
        public DataTable GetValidTemplateList()
        {
            return DbHelper.ExecuteDataset(CommandType.Text, "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "templates] ORDER BY [templateid]").Tables[0];
        }

        /// <summary>
        /// 获得前台有效的模板ID列表
        /// </summary>
        /// <returns>模板ID列表</returns>
        public DataTable GetValidTemplateIDList()
        {
            return DbHelper.ExecuteDataset(CommandType.Text, "SELECT [templateid] FROM [" + BaseConfigs.GetTablePrefix + "templates] ORDER BY [templateid]").Tables[0];
        }

        public DataTable GetPost(string posttablename, int pid)
        {
            DbParameter parm = DbHelper.MakeInParam("@pid", (DbType)SqlDbType.Int, 4, pid);
            return DbHelper.ExecuteDataset(CommandType.Text, "SELECT TOP 1 *  FROM [" + posttablename + "] WHERE [pid]=@pid", parm).Tables[0];
        }

        public DataTable GetMainPostByTid(string posttablename, int tid)
        {
            DbParameter parm = DbHelper.MakeInParam("@tid", (DbType)SqlDbType.Int, 4, tid);
            return DbHelper.ExecuteDataset(CommandType.Text, "SELECT TOP 1 * FROM [" + posttablename + "] WHERE [layer]=0  AND [tid]=@tid", parm).Tables[0];
        }

        public DataTable GetAttachmentsByPid(int pid)
        {
            DbParameter parm = DbHelper.MakeInParam("@pid", (DbType)SqlDbType.Int, 4, pid);
            return DbHelper.ExecuteDataset(CommandType.Text, "SELECT [aid], [tid], [pid], [postdatetime], [readperm], [filename], [description], [filetype], [filesize], [attachment], [downloads] FROM [" + BaseConfigs.GetTablePrefix + "attachments] WHERE [pid]=@pid", parm).Tables[0];
        }

        public DataTable GetAdvertisement(int aid)
        {
            //此函数放在Advs.cs文件中较好
            DbParameter parm = DbHelper.MakeInParam("@aid", (DbType)SqlDbType.Int, 4, aid);
            return DbHelper.ExecuteDataset(CommandType.Text, "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "advertisements] WHERE [advid]=@aid", parm).Tables[0];
        }

        private string GetSearchTopicTitleSQL(int posterid, string searchforumid, int resultorder, int resultordertype, int digest, string keyword)
        {
            keyword = Regex.Replace(keyword, "--|;|'|\"", "", RegexOptions.Compiled | RegexOptions.Multiline);

            StringBuilder strKeyWord = new StringBuilder(keyword);

            // 替换转义字符
            strKeyWord.Replace("'", "''");
            strKeyWord.Replace("%", "[%]");
            strKeyWord.Replace("_", "[_]");
            strKeyWord.Replace("[", "[[]");

            StringBuilder strSQL = new StringBuilder();
            strSQL.AppendFormat("SELECT [tid] FROM [{0}topics] WHERE [displayorder]>=0", BaseConfigs.GetTablePrefix);

            if (posterid > 0)
            {
                strSQL.Append(" AND [posterid]=");
                strSQL.Append(posterid);
            }

            if (digest > 0)
            {
                strSQL.Append(" AND [digest]>0 ");
            }

            if (searchforumid != string.Empty)
            {
                strSQL.Append(" AND [fid] IN (");
                strSQL.Append(searchforumid);
                strSQL.Append(")");
            }

            string[] keywordlist = Utils.SplitString(strKeyWord.ToString(), " ");
            strKeyWord = new StringBuilder();

            if (keyword.Length > 0)
            {
                strKeyWord.Append(" AND (1=0 ");
                for (int i = 0; i < keywordlist.Length; i++)
                {
                    strKeyWord.Append(" OR [title] ");

                    strKeyWord.Append("LIKE '%");
                    strKeyWord.Append(RegEsc(keywordlist[i]));
                    strKeyWord.Append("%' ");
                }
                strKeyWord.Append(")");
            }

            strSQL.Append(strKeyWord.ToString());

            strSQL.Append(" ORDER BY ");
            switch (resultorder)
            {
                case 1:
                    strSQL.Append("[tid]");
                    break;
                case 2:
                    strSQL.Append("[replies]");
                    break;
                case 3:
                    strSQL.Append("[views]");
                    break;
                default:
                    strSQL.Append("[postdatetime]");
                    break;
            }

            if (resultordertype == 1)
            {
                strSQL.Append(" ASC");
            }
            else
            {
                strSQL.Append(" DESC");
            }

            return strSQL.ToString();
        }

        private string GetSearchPostContentSQL(int posterid, string searchforumid, int resultorder, int resultordertype, int searchtime, int searchtimetype, int posttableid, StringBuilder strKeyWord)
        {
            StringBuilder strSQL = new StringBuilder();

            string orderfield = "lastpost";
            switch (resultorder)
            {
                case 1:
                    orderfield = "tid";
                    break;
                case 2:
                    orderfield = "replies";
                    break;
                case 3:
                    orderfield = "views";
                    break;
                default:
                    orderfield = "lastpost";
                    break;
            }

            strSQL.AppendFormat("SELECT DISTINCT [{0}posts{1}].[tid],[{0}topics].[{2}] FROM [{0}posts{1}] LEFT JOIN [{0}topics] ON [{0}topics].[tid]=[{0}posts{1}].[tid] WHERE [{0}topics].[displayorder]>=0 AND ", BaseConfigs.GetTablePrefix, posttableid, orderfield);

            if (searchforumid != string.Empty)
            {
                strSQL.AppendFormat("[{0}posts{1}].[fid] IN (", BaseConfigs.GetTablePrefix, posttableid);
                strSQL.Append(searchforumid);
                strSQL.Append(") AND ");
            }

            if (posterid != -1)
            {
                strSQL.AppendFormat("[{0}posts{1}].[posterid]=", BaseConfigs.GetTablePrefix, posttableid);
                strSQL.Append(posterid);
                strSQL.Append(" AND ");
            }

            if (searchtime != 0)
            {
                strSQL.AppendFormat("[{0}posts{1}].[postdatetime]", BaseConfigs.GetTablePrefix, posttableid);
                if (searchtimetype == 1)
                {
                    strSQL.Append("<'");
                }
                else
                {
                    strSQL.Append(">'");
                }
                strSQL.Append(DateTime.Now.AddDays(searchtime).ToString("yyyy-MM-dd 00:00:00"));
                strSQL.Append("'AND ");
            }

            string[] keywordlist = Utils.SplitString(strKeyWord.ToString(), " ");
            strKeyWord = new StringBuilder();
            for (int i = 0; i < keywordlist.Length; i++)
            {
                strKeyWord.Append(" OR ");
                if (GeneralConfigs.GetConfig().Fulltextsearch == 1)
                {
                    strKeyWord.AppendFormat("CONTAINS(message, '\"*", BaseConfigs.GetTablePrefix, posttableid);
                    strKeyWord.Append(keywordlist[i]);
                    strKeyWord.Append("*\"') ");
                }
                else
                {
                    strKeyWord.AppendFormat("[{0}posts{1}].[message] LIKE '%", BaseConfigs.GetTablePrefix, posttableid);
                    strKeyWord.Append(RegEsc(keywordlist[i]));
                    strKeyWord.Append("%' ");
                }
            }

            strSQL.Append(strKeyWord.ToString().Substring(3));
            strSQL.AppendFormat("ORDER BY [{0}topics].", BaseConfigs.GetTablePrefix);

            switch (resultorder)
            {
                case 1:
                    strSQL.Append("[tid]");
                    break;
                case 2:
                    strSQL.Append("[replies]");
                    break;
                case 3:
                    strSQL.Append("[views]");
                    break;
                default:
                    strSQL.Append("[lastpost]");
                    break;
            }
            if (resultordertype == 1)
            {
                strSQL.Append(" ASC");
            }
            else
            {
                strSQL.Append(" DESC");
            }

            return strSQL.ToString();
        }

        private string GetSearchSpacePostTitleSQL(int posterid, int resultorder, int resultordertype, int searchtime, int searchtimetype, string keyword)
        {
            keyword = Regex.Replace(keyword, "--|;|'|\"", "", RegexOptions.Compiled | RegexOptions.Multiline);

            StringBuilder strKeyWord = new StringBuilder(keyword);

            // 替换转义字符
            strKeyWord.Replace("'", "''");
            strKeyWord.Replace("%", "[%]");
            strKeyWord.Replace("_", "[_]");
            strKeyWord.Replace("[", "[[]");

            StringBuilder strSQL = new StringBuilder();
            strSQL.AppendFormat("SELECT [postid] FROM [{0}spaceposts] WHERE [{0}spaceposts].[poststatus]=1 ", BaseConfigs.GetTablePrefix);
            
            if (posterid > 0)
            {
                strSQL.Append(" AND [uid]=");
                strSQL.Append(posterid);
            }

            if (searchtime != 0)
            {
                strSQL.AppendFormat(" AND [{0}spaceposts].[postdatetime]", BaseConfigs.GetTablePrefix);
                if (searchtimetype == 1)
                {
                    strSQL.Append("<'");
                }
                else
                {
                    strSQL.Append(">'");
                }
                strSQL.Append(DateTime.Now.AddDays(searchtime).ToString("yyyy-MM-dd 00:00:00"));
                strSQL.Append("' ");
            }

            string[] keywordlist = Utils.SplitString(strKeyWord.ToString(), " ");
            strKeyWord = new StringBuilder();

            if (keyword.Length > 0)
            {
                strKeyWord.Append(" AND (1=0 ");
                for (int i = 0; i < keywordlist.Length; i++)
                {
                    strKeyWord.Append(" OR [title] ");
                    strKeyWord.Append("LIKE '%");
                    strKeyWord.Append(RegEsc(keywordlist[i]));
                    strKeyWord.Append("%' ");
                }
                strKeyWord.Append(")");
            }

            strSQL.Append(strKeyWord.ToString());

            strSQL.Append(" ORDER BY ");
            switch (resultorder)
            {
                case 1:
                    strSQL.Append("[commentcount]");
                    break;
                case 2:
                    strSQL.Append("[views]");
                    break;
                default:
                    strSQL.Append("[postdatetime]");
                    break;
            }

            if (resultordertype == 1)
            {
                strSQL.Append(" ASC");
            }
            else
            {
                strSQL.Append(" DESC");
            }

            return strSQL.ToString();
        }

        private string GetSearchAlbumTitleSQL(int posterid, int resultorder, int resultordertype, int searchtime, int searchtimetype, string keyword)
        {
            keyword = Regex.Replace(keyword, "--|;|'|\"", "", RegexOptions.Compiled | RegexOptions.Multiline);

            StringBuilder strKeyWord = new StringBuilder(keyword);

            // 替换转义字符
            strKeyWord.Replace("'", "''");
            strKeyWord.Replace("%", "[%]");
            strKeyWord.Replace("_", "[_]");
            strKeyWord.Replace("[", "[[]");

            StringBuilder strSQL = new StringBuilder();
            strSQL.AppendFormat("SELECT [albumid] FROM [{0}albums] WHERE [{0}albums].[type]=0 ", BaseConfigs.GetTablePrefix);

            if (posterid > 0)
            {
                strSQL.Append(" AND [userid]=");
                strSQL.Append(posterid);
            }

            if (searchtime != 0)
            {
                strSQL.AppendFormat(" AND [{0}albums].[createdatetime]", BaseConfigs.GetTablePrefix);
                if (searchtimetype == 1)
                {
                    strSQL.Append("<'");
                }
                else
                {
                    strSQL.Append(">'");
                }
                strSQL.Append(DateTime.Now.AddDays(searchtime).ToString("yyyy-MM-dd 00:00:00"));
                strSQL.Append("' ");
            }

            string[] keywordlist = Utils.SplitString(strKeyWord.ToString(), " ");
            strKeyWord = new StringBuilder();

            if (keyword.Length > 0)
            {
                strKeyWord.Append(" AND (1=0 ");
                for (int i = 0; i < keywordlist.Length; i++)
                {
                    strKeyWord.Append(" OR [title] ");
                    strKeyWord.Append("LIKE '%");
                    strKeyWord.Append(RegEsc(keywordlist[i]));
                    strKeyWord.Append("%' ");
                }
                strKeyWord.Append(")");
            }

            strSQL.Append(strKeyWord.ToString());

            strSQL.Append(" ORDER BY ");
            switch (resultorder)
            {
                case 1:
                    strSQL.Append("[albumid]");
                    break;
                default:
                    strSQL.Append("[createdatetime]");
                    break;
            }

            if (resultordertype == 1)
            {
                strSQL.Append(" ASC");
            }
            else
            {
                strSQL.Append(" DESC");
            }

            return strSQL.ToString();
        }

        private string GetSearchByPosterSQL(int posterid, int posttableid)
        {
            if (posterid > 0)
            {
                string sql = string.Format(@"SELECT DISTINCT [tid], 'forum' AS [datafrom] FROM [{0}posts{1}] WHERE [posterid]={2} AND [tid] NOT IN (SELECT [tid] FROM [{0}topics] WHERE [posterid]={2} AND [displayorder]<0)", BaseConfigs.GetTablePrefix, posttableid, posterid);
                return sql;
            }
            return string.Empty;
        }

        private StringBuilder GetSearchByPosterResult(IDataReader reader)
        {
            StringBuilder strTids = new StringBuilder("<ForumTopics>");
            StringBuilder strAlbumids = new StringBuilder("<Albums>");
            StringBuilder strSpacePostids = new StringBuilder("<SpacePosts>");
            StringBuilder result = new StringBuilder();

            //if (reader != null)
            //{
                while (reader.Read())
                {
                    switch (reader[1].ToString())
                    {
                        case "forum":
                            strTids.AppendFormat("{0},", reader[0].ToString());
                            break;
                        case "album":
                            strAlbumids.AppendFormat("{0},", reader[0].ToString());
                            break;
                        case "spacepost":
                            strSpacePostids.AppendFormat("{0},", reader[0].ToString());
                            break;
                    }
                }
                reader.Close();
            //}
            if (strTids.ToString().EndsWith(","))
            {
                strTids.Length--;
            }
            if (strAlbumids.ToString().EndsWith(","))
            {
                strAlbumids.Length--;
            }
            if (strSpacePostids.ToString().EndsWith(","))
            {
                strSpacePostids.Length--;
            }
            strTids.Append("</ForumTopics>");
            strAlbumids.Append("</Albums>");
            strSpacePostids.Append("</SpacePosts>");

            result.Append(strTids.ToString());
            result.Append(strAlbumids.ToString());
            result.Append(strSpacePostids.ToString());

            return result;
        }
        /// <summary>
        /// 根据指定条件进行搜索
        /// </summary>
        /// <param name="posttableid">帖子表id</param>
        /// <param name="userid">用户id</param>
        /// <param name="usergroupid">用户组id</param>
        /// <param name="keyword">关键字</param>
        /// <param name="posterid">发帖者id</param>
        /// <param name="type">搜索类型</param>
        /// <param name="searchforumid">搜索版块id</param>
        /// <param name="keywordtype">关键字类型</param>
        /// <param name="searchtime">搜索时间</param>
        /// <param name="searchtimetype">搜索时间类型</param>
        /// <param name="resultorder">结果排序方式</param>
        /// <param name="resultordertype">结果类型类型</param>
        /// <returns>如果成功则返回searchid, 否则返回-1</returns>
        public int Search(int posttableid, int userid, int usergroupid, string keyword, int posterid, string type, string searchforumid, int keywordtype, int searchtime, int searchtimetype, int resultorder, int resultordertype)
        {

            // 超过30分钟的缓存纪录将被删除
            DatabaseProvider.GetInstance().DeleteExpriedSearchCache();
            string sql = string.Empty;
            StringBuilder strTids = new StringBuilder();
            SearchType searchType = SearchType.TopicTitle;

            switch (keywordtype)
            { 
                case 0:
                    searchType = SearchType.PostTitle;
                    if (type == "digest")
                    {
                        searchType = SearchType.DigestTopic;
                    }
                    break;
                case 1:
                    searchType = SearchType.PostContent;
                    break;
                case 2:
                    searchType = SearchType.SpacePostTitle;
                    break;
                case 8:
                    searchType = SearchType.ByPoster;
                    break;
            }
            switch (searchType)
            { 
                case SearchType.All:
                    break;
                case SearchType.DigestTopic:
                    sql = GetSearchTopicTitleSQL(posterid, searchforumid, resultorder, resultordertype, 1, keyword);
                    break;
                case SearchType.TopicTitle:
                    sql = GetSearchTopicTitleSQL(posterid, searchforumid, resultorder, resultordertype, 0, keyword);
                    break;
                case SearchType.PostTitle:
                    sql = GetSearchTopicTitleSQL(posterid, searchforumid, resultorder, resultordertype, 0, keyword);
                    break;
                case SearchType.PostContent:
                    sql = GetSearchPostContentSQL(posterid, searchforumid, resultorder, resultordertype, searchtime, searchtimetype, posttableid, new StringBuilder(keyword));
                    break;
                case SearchType.SpacePostTitle:
                    sql = GetSearchSpacePostTitleSQL(posterid, resultorder, resultordertype, searchtime, searchtimetype, keyword);
                    break;
                case SearchType.ByPoster:
                    sql = GetSearchByPosterSQL(posterid, posttableid);
                    break;
                default:
                    sql = GetSearchTopicTitleSQL(posterid, searchforumid, resultorder, resultordertype, 0, keyword);
                    break;
            }
            
            #region
            /*
            // 关键词与作者至少有一个条件不为空
            if (keyword.Equals(""))//按作者搜索
            {

                if (type == "digest")
                {
                    strSQL.AppendFormat("SELECT [tid] FROM [{0}topics] WHERE [digest]>0 AND [posterid]=", BaseConfigs.GetTablePrefix);
                    strSQL.Append(posterid);
                }

                else if (type == "post")
                {
                    strSQL.AppendFormat("SELECT [pid] FROM [{0}posts{1}] WHERE [posterid]=", BaseConfigs.GetTablePrefix, posttableid);
                    strSQL.Append(posterid);
                }
                else
                {
                    strSQL.AppendFormat("SELECT [tid] FROM [{0}topics] WHERE [posterid]=", BaseConfigs.GetTablePrefix);
                    strSQL.Append(posterid);
                }

                //所属板块判断
                if (!searchforumid.Equals(""))
                {
                    strSQL.Append(" AND [fid] IN (");
                    strSQL.Append(searchforumid);
                    strSQL.Append(")");
                }


                strSQL.Append(" ORDER BY ");

                switch (resultorder)
                {
                    case 1:
                        strSQL.Append("[tid]");
                        break;
                    case 2:
                        strSQL.Append("[replies]");
                        break;
                    case 3:
                        strSQL.Append("[views]");
                        break;
                    default:
                        strSQL.Append("[postdatetime]");
                        break;
                }
                if (resultordertype == 1)
                {
                    strSQL.Append(" ASC");
                }
                else
                {
                    strSQL.Append(" DESC");
                }

            }
            else
            {
                // 过滤危险字符
                keyword = Regex.Replace(keyword, "--|;|'|\"", "", RegexOptions.Compiled | RegexOptions.Multiline);

                StringBuilder strKeyWord = new StringBuilder(keyword);

                // 替换转义字符
                strKeyWord.Replace("'", "''");
                strKeyWord.Replace("%", "[%]");
                strKeyWord.Replace("_", "[_]");
                strKeyWord.Replace("[", "[[]");


                // 将SQL查询条件循序指定为"forumid, posterid, 搜索时间范围, 关键词"
                if (keywordtype == 0)
                {
                    strSQL.AppendFormat("SELECT [tid] FROM [{0}topics] WHERE", BaseConfigs.GetTablePrefix);
                    if (!searchforumid.Equals(""))
                    {
                        strSQL.Append(" [fid] IN (");
                        strSQL.Append(searchforumid);
                        strSQL.Append(") AND ");
                    }
                    if (posterid != -1)
                    {
                        strSQL.Append("[posterid]=");
                        strSQL.Append(posterid);
                        strSQL.Append(" AND ");
                    }

                    if (searchtime != 0)
                    {
                        strSQL.Append("[postdatetime]");
                        if (searchtimetype == 1)
                        {
                            strSQL.Append("<");
                        }
                        else
                        {
                            strSQL.Append(">'");
                        }
                        strSQL.Append(DateTime.Now.AddDays(searchtime * -1).ToString("yyyy-MM-dd 00:00:00"));
                        strSQL.Append("'AND ");
                    }

                    string[] keywordlist = Utils.SplitString(strKeyWord.ToString(), " ");
                    strKeyWord = new StringBuilder();
                    for (int i = 0; i < keywordlist.Length; i++)
                    {
                        strKeyWord.Append(" OR [title] ");

                        strKeyWord.Append("LIKE '%");
                        strKeyWord.Append(keywordlist[i]);
                        strKeyWord.Append("%' ");
                    }

                    strSQL.Append(strKeyWord.ToString().Substring(3));
                    strSQL.Append("ORDER BY ");

                    switch (resultorder)
                    {
                        case 1:
                            strSQL.Append("[tid]");
                            break;
                        case 2:
                            strSQL.Append("[replies]");
                            break;
                        case 3:
                            strSQL.Append("[views]");
                            break;
                        default:
                            strSQL.Append("[lastpost]");
                            break;
                    }
                    if (resultordertype == 1)
                    {
                        strSQL.Append(" ASC");
                    }
                    else
                    {
                        strSQL.Append(" DESC");
                    }
                }
                else
                {
                    string orderfield = "lastpost";
                    switch (resultorder)
                    {
                        case 1:
                            orderfield = "tid";
                            break;
                        case 2:
                            orderfield = "replies";
                            break;
                        case 3:
                            orderfield = "views";
                            break;
                        default:
                            orderfield = "lastpost";
                            break;
                    }


                    strSQL.AppendFormat("SELECT DISTINCT [{0}posts{1}].[tid],[{0}topics].[{2}] FROM [{0}posts{1}] LEFT JOIN [{0}topics] ON [{0}topics].[tid]=[{0}posts{1}].[tid] WHERE ", BaseConfigs.GetTablePrefix, posttableid, orderfield);
                    if (!searchforumid.Equals(""))
                    {
                        strSQL.AppendFormat("[{0}posts{1}].[fid] IN (", BaseConfigs.GetTablePrefix, posttableid);
                        strSQL.Append(searchforumid);
                        strSQL.Append(") AND ");
                    }
                    if (posterid != -1)
                    {
                        strSQL.AppendFormat("[{0}posts{1}].[posterid]=", BaseConfigs.GetTablePrefix, posttableid);
                        strSQL.Append(posterid);
                        strSQL.Append(" AND ");
                    }

                    if (searchtime != 0)
                    {
                        strSQL.AppendFormat("[{0}posts{1}].[postdatetime]", BaseConfigs.GetTablePrefix, posttableid);
                        if (searchtimetype == 1)
                        {
                            strSQL.Append("<");
                        }
                        else
                        {
                            strSQL.Append(">'");
                        }
                        strSQL.Append(DateTime.Now.AddDays(searchtime).ToString("yyyy-MM-dd 00:00:00"));
                        strSQL.Append("'AND ");
                    }

                    string[] keywordlist = Utils.SplitString(strKeyWord.ToString(), " ");
                    strKeyWord = new StringBuilder();
                    for (int i = 0; i < keywordlist.Length; i++)
                    {
                        strKeyWord.Append(" OR ");
                        if (GeneralConfigs.GetConfig().Fulltextsearch == 1)
                        {
                            strKeyWord.AppendFormat("CONTAINS(message, '\"*", BaseConfigs.GetTablePrefix, posttableid);
                            strKeyWord.Append(keywordlist[i]);
                            strKeyWord.Append("*\"') ");
                        }
                        else
                        {
                            strKeyWord.AppendFormat("[{0}posts{1}].[message] LIKE '%", BaseConfigs.GetTablePrefix, posttableid);
                            strKeyWord.Append(keywordlist[i]);
                            strKeyWord.Append("%' ");
                        }
                    }

                    strSQL.Append(strKeyWord.ToString().Substring(3));
                    strSQL.AppendFormat("ORDER BY [{0}topics].", BaseConfigs.GetTablePrefix);

                    switch (resultorder)
                    {
                        case 1:
                            strSQL.Append("[tid]");
                            break;
                        case 2:
                            strSQL.Append("[replies]");
                            break;
                        case 3:
                            strSQL.Append("[views]");
                            break;
                        default:
                            strSQL.Append("[lastpost]");
                            break;
                    }
                    if (resultordertype == 1)
                    {
                        strSQL.Append(" ASC");
                    }
                    else
                    {
                        strSQL.Append(" DESC");
                    }
                }
            }
            */
            #endregion

            if (sql == string.Empty)
            {
                return -1;
            }

            DbParameter[] prams2 = {
										DbHelper.MakeInParam("@searchstring",(DbType)SqlDbType.VarChar,255, sql),
										DbHelper.MakeInParam("@uid",(DbType)SqlDbType.Int,4,userid),
										DbHelper.MakeInParam("@groupid",(DbType)SqlDbType.Int,4,usergroupid)
									};
            int searchid = Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, string.Format(@"SELECT TOP 1 [searchid] FROM [{0}searchcaches] WHERE [searchstring]=@searchstring AND [groupid]=@groupid", BaseConfigs.GetTablePrefix), prams2), -1);

            if (searchid > -1)
            {
                return searchid;
            }

            IDataReader reader;
            try
            {
                reader = DbHelper.ExecuteReader(CommandType.Text, sql);
            }
            catch
            {
                ConfirmFullTextEnable();
                reader = DbHelper.ExecuteReader(CommandType.Text, sql);
            }

            int rowcount = 0;
            if (reader != null)
            {
                switch (searchType)
                { 
                    case SearchType.All:
                    case SearchType.DigestTopic:
                    case SearchType.TopicTitle:
                    case SearchType.PostTitle:
                    case SearchType.PostContent:
                        strTids.Append("<ForumTopics>");
                        break;
                    case SearchType.SpacePostTitle:
                        strTids.Append("<SpacePosts>");
                        break;
                    case SearchType.ByPoster:
                        strTids = GetSearchByPosterResult(reader);
                        SearchCacheInfo cacheinfo = new SearchCacheInfo();
                        cacheinfo.Keywords = keyword;
                        cacheinfo.Searchstring = sql;
                        cacheinfo.Postdatetime = Utils.GetDateTime();
                        cacheinfo.Topics = rowcount;
                        cacheinfo.Tids = strTids.ToString();
                        cacheinfo.Uid = userid;
                        cacheinfo.Groupid = usergroupid;
                        cacheinfo.Ip = DNTRequest.GetIP();
                        cacheinfo.Expiration = Utils.GetDateTime();

                        reader.Close();

                        return CreateSearchCache(cacheinfo);
                }
                while (reader.Read())
                {
                    strTids.Append(reader[0].ToString());
                    strTids.Append(",");
                    rowcount++;
                }
                if (rowcount > 0)
                {                    
                    strTids.Remove(strTids.Length - 1, 1);
                    switch (searchType)
                    {
                        case SearchType.All:
                        case SearchType.DigestTopic:
                        case SearchType.TopicTitle:
                        case SearchType.PostTitle:
                        case SearchType.PostContent:
                            strTids.Append("</ForumTopics>");
                            break;
                        case SearchType.SpacePostTitle:
                            strTids.Append("</SpacePosts>");
                            break;

                    }
                    SearchCacheInfo cacheinfo = new SearchCacheInfo();
                    cacheinfo.Keywords = keyword;
                    cacheinfo.Searchstring = sql;
                    cacheinfo.Postdatetime = Utils.GetDateTime();
                    cacheinfo.Topics = rowcount;
                    cacheinfo.Tids = strTids.ToString();
                    cacheinfo.Uid = userid;
                    cacheinfo.Groupid = usergroupid;
                    cacheinfo.Ip = DNTRequest.GetIP();
                    cacheinfo.Expiration = Utils.GetDateTime();

                    reader.Close();

                    return CreateSearchCache(cacheinfo);
                }
                reader.Close();
            }
            return -1;
        }     

        public string BackUpDatabase(string backuppath, string ServerName, string UserName, string Password, string strDbName, string strFileName)
        {
            SQLServer svr = new SQLServerClass();
            try
            {
                svr.Connect(ServerName, UserName, Password);
                Backup bak = new BackupClass();
                bak.Action = 0;
                bak.Initialize = true;
                bak.Files = backuppath + strFileName + ".config";
                bak.Database = strDbName;
                bak.SQLBackup(svr);
                return string.Empty;
            }
            catch(Exception ex)
            {
                string message = ex.Message.Replace("'", " ");
                message = message.Replace("\n", " ");
                message = message.Replace("\\", "/");
                return message;
            }
            finally
            {
                svr.DisConnect();
            }
        }

        public string RestoreDatabase(string backuppath, string ServerName, string UserName, string Password, string strDbName, string strFileName)
        {
            #region 数据库的恢复的代码

            SQLServer svr = new SQLServerClass();
            try
            {
                svr.Connect(ServerName, UserName, Password);
                QueryResults qr = svr.EnumProcesses(-1);
                int iColPIDNum = -1;
                int iColDbName = -1;
                for (int i = 1; i <= qr.Columns; i++)
                {
                    string strName = qr.get_ColumnName(i);
                    if (strName.ToUpper().Trim() == "SPID")
                    {
                        iColPIDNum = i;
                    }
                    else if (strName.ToUpper().Trim() == "DBNAME")
                    {
                        iColDbName = i;
                    }
                    if (iColPIDNum != -1 && iColDbName != -1)
                        break;
                }

                for (int i = 1; i <= qr.Rows; i++)
                {
                    int lPID = qr.GetColumnLong(i, iColPIDNum);
                    string strDBName = qr.GetColumnString(i, iColDbName);
                    if (strDBName.ToUpper() == strDbName.ToUpper())
                        svr.KillProcess(lPID);
                }


                Restore res = new RestoreClass();
                res.Action = 0;
                string path = backuppath + strFileName + ".config";
                res.Files = path;

                res.Database = strDbName;
                res.ReplaceDatabase = true;
                res.SQLRestore(svr);

                return string.Empty;
            }
            catch (Exception err)
            {
                string message = err.Message.Replace("'", " ");
                message = message.Replace("\n", " ");
                message = message.Replace("\\", "/");
                
                return message;
            }
            finally
            {
                svr.DisConnect();
            }

            #endregion
        }

        public string SearchVisitLog(DateTime postdatetimeStart, DateTime postdatetimeEnd, string Username, string others)
        {
            string sqlstring = null;
            sqlstring += " [visitid]>0";

            sqlstring = GetSqlstringByPostDatetime(sqlstring, postdatetimeStart, postdatetimeEnd);

            if (others != "")
            {
                sqlstring += " AND [others] LIKE '%" + RegEsc(others) + "%'";
            }

            if (Username != "")
            {
                sqlstring += " AND (";
                foreach (string word in Username.Split(','))
                {
                    if (word.Trim() != "")
                        sqlstring += " [username] LIKE '%" + RegEsc(word) + "%' OR ";
                }
                sqlstring = sqlstring.Substring(0, sqlstring.Length - 3) + ")";
            }

            return sqlstring;
        
        }


        public string SearchMedalLog(DateTime postdatetimeStart, DateTime postdatetimeEnd, string Username, string reason)
        {
            string sqlstring = null;
            sqlstring += " [id]>0";

            sqlstring = GetSqlstringByPostDatetime(sqlstring, postdatetimeStart, postdatetimeEnd);

            if (reason != "")
            {
                sqlstring += " AND [reason] LIKE '%" + RegEsc(reason) + "%'";
            }

            if (Username != "")
            {
                sqlstring += " AND (";
                foreach (string word in Username.Split(','))
                {
                    if (word.Trim() != "")
                        sqlstring += " [username] LIKE '%" + RegEsc(word) + "%' OR ";
                }
                sqlstring = sqlstring.Substring(0, sqlstring.Length - 3) + ")";
            }
            return sqlstring;
        }

        public string SearchModeratorManageLog(DateTime postdatetimeStart, DateTime postdatetimeEnd, string Username, string others)
        {
            string sqlstring = null;
            sqlstring += " [id]>0";

            sqlstring = GetSqlstringByPostDatetime(sqlstring, postdatetimeStart, postdatetimeEnd);

            if (others != "")
            {
                sqlstring += " AND [reason] LIKE '%" + RegEsc(others) + "%'";
            }

            if (Username != "")
            {
                sqlstring += " AND (";
                foreach (string word in Username.Split(','))
                {
                    if (word.Trim() != "")
                        sqlstring += " [moderatorname] LIKE '%" + RegEsc(word) + "%' OR ";
                }
                sqlstring = sqlstring.Substring(0, sqlstring.Length - 3) + ")";
            }

            return sqlstring;
        }

        public string SearchPaymentLog(DateTime postdatetimeStart, DateTime postdatetimeEnd, string Username)
        {

            string sqlstring = null;
            sqlstring += " [" + BaseConfigs.GetTablePrefix + "paymentlog].[id]>0";

            if (postdatetimeStart.ToString() != "")
            {
                sqlstring += " AND [" + BaseConfigs.GetTablePrefix + "paymentlog].[buydate]>='" + postdatetimeStart.ToString("yyyy-MM-dd HH:mm:ss") + "'";
            }

            if (postdatetimeEnd.ToString() != "")
            {
                sqlstring += " AND [" + BaseConfigs.GetTablePrefix + "paymentlog].[buydate]<='" + postdatetimeEnd.AddDays(1).ToString("yyyy-MM-dd HH:mm:ss") + "'";
            }

            if (Username != "")
            {
                string usernamesearch = " WHERE (";
                foreach (string word in Username.Split(','))
                {
                    if (word.Trim() != "")
                        usernamesearch += " [username] LIKE '%" + RegEsc(word) + "%' OR ";
                }
                usernamesearch = usernamesearch.Substring(0, usernamesearch.Length - 3) + ")";

                //找出当前用户名所属的UID
                DataTable dt = DbHelper.ExecuteDataset("SELECT [uid] From [" + BaseConfigs.GetTablePrefix + "users] " + usernamesearch).Tables[0];
                string uid = "-1";
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        uid += "," + dr["uid"].ToString();
                    }
                }
                sqlstring += " AND [" + BaseConfigs.GetTablePrefix + "paymentlog].[uid] IN(" + uid + ")";

            }

            return sqlstring;
        }

        public string SearchRateLog(DateTime postdatetimeStart, DateTime postdatetimeEnd, string Username, string others)
        {
            string sqlstring = null;
            sqlstring += " [id]>0";

            sqlstring = GetSqlstringByPostDatetime(sqlstring, postdatetimeStart, postdatetimeEnd);

            if (others != "")
            {
                sqlstring += " AND [reason] LIKE '%" + RegEsc(others) + "%'";
            }

            if (Username != "")
            {
                sqlstring += " AND (";
                foreach (string word in Username.Split(','))
                {
                    if (word.Trim() != "")
                        sqlstring += " [username] LIKE '%" + RegEsc(word) + "%' OR ";
                }
                sqlstring = sqlstring.Substring(0, sqlstring.Length - 3) + ")";
            }

            return sqlstring;
        }

        public string DeletePrivateMessages(bool isnew, string postdatetime, string msgfromlist, bool lowerupper, string subject, string message, bool isupdateusernewpm)
        {
            string sqlstring = null;
            sqlstring += "WHERE [pmid]>0";

            if (isnew)
            {
                sqlstring += " AND [new]=0";
            }

            if (postdatetime != "")
            {
                sqlstring += " AND DATEDIFF(day,postdatetime,getdate())>=" + postdatetime + "";
            }

            if (msgfromlist != "")
            {
                sqlstring += " AND (";
                foreach (string msgfrom in msgfromlist.Split(','))
                {
                    if (msgfrom.Trim() != "")
                    {
                        if (lowerupper)
                        {
                            sqlstring += " [msgfrom]='" + msgfrom + "' OR";
                        }
                        else
                        {
                            sqlstring += " [msgfrom] COLLATE Chinese_PRC_CS_AS_WS ='" + msgfrom + "' OR";

                        }
                    }
                }
                sqlstring = sqlstring.Substring(0, sqlstring.Length - 3) + ")";
            }

            if (subject != "")
            {
                sqlstring += " AND (";
                foreach (string sub in subject.Split(','))
                {
                    if (sub.Trim() != "")
                        sqlstring += " [subject] LIKE '%" + RegEsc(sub) + "%' OR ";
                }
                sqlstring = sqlstring.Substring(0, sqlstring.Length - 3) + ")";
            }

            if (message != "")
            {
                sqlstring += " AND (";
                foreach (string mess in message.Split(','))
                {
                    if (mess.Trim() != "")
                        sqlstring += " [message] LIKE '%" + RegEsc(mess) + "%' OR ";
                }
                sqlstring = sqlstring.Substring(0, sqlstring.Length - 3) + ")";
            }

            if (isupdateusernewpm)
            {
                DbHelper.ExecuteNonQuery("UPDATE [" + BaseConfigs.GetTablePrefix + "users] SET [newpm]=0 WHERE [uid] IN (SELECT [msgtoid] FROM [" + BaseConfigs.GetTablePrefix + "pms] " + sqlstring + ")");
            }

            DbHelper.ExecuteNonQuery("DELETE FROM [" + BaseConfigs.GetTablePrefix + "pms] " + sqlstring);

            return sqlstring;
        }

        public bool IsExistSmilieCode(string code, int currentid)
        {
            DbParameter[] parms = {
										DbHelper.MakeInParam("@code",(DbType)SqlDbType.NVarChar, 30, code),
										DbHelper.MakeInParam("@currentid",(DbType)SqlDbType.Int, 4, currentid)
									};
            string sql = "SELECT [id] FROM [" + BaseConfigs.GetTablePrefix + "smilies] WHERE [code]=@code AND [id]<>@currentid";
            return DbHelper.ExecuteDataset(CommandType.Text, sql, parms).Tables[0].Rows.Count != 0;
        }

        public string  GetSmilieByType(int id)
        {
            return "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "smilies] WHERE [type]=" + id;
        }


        public string AddTableData()
        {

            return "SELECT [groupid], [grouptitle] FROM [" + BaseConfigs.GetTablePrefix + "usergroups] WHERE [groupid]<=3 ORDER BY [groupid]";

        }

        public string Global_UserGrid_GetCondition(string getstring)
        {

            return "[" + BaseConfigs.GetTablePrefix + "users].[username]='" + getstring + "'";

        }

        public int Global_UserGrid_RecordCount()
        {

            return Convert.ToInt32(DbHelper.ExecuteDataset("SELECT COUNT(uid) FROM [" + BaseConfigs.GetTablePrefix + "users]").Tables[0].Rows[0][0].ToString());

        }


        public int Global_UserGrid_RecordCount(string condition)
        {

            return Convert.ToInt32(DbHelper.ExecuteDataset("SELECT COUNT(uid) FROM [" + BaseConfigs.GetTablePrefix + "users]  WHERE " + condition).Tables[0].Rows[0][0].ToString());

        }

        public string Global_UserGrid_SearchCondition(bool islike, bool ispostdatetime, string username, string nickname, string UserGroup, string email, string credits_start, string credits_end, string lastip, string posts, string digestposts, string uid, string joindateStart, string joindateEnd)
        {

            string searchcondition = " [" + BaseConfigs.GetTablePrefix + "users].[uid]>0 ";
            if (islike)
            {
                if (username != "") searchcondition += " AND [" + BaseConfigs.GetTablePrefix + "users].[username] like'%" + RegEsc(username) + "%'";
                if (nickname != "") searchcondition += " AND [" + BaseConfigs.GetTablePrefix + "users].[nickname] like'%" + RegEsc(nickname) + "%'";
            }
            else
            {
                if (username != "") searchcondition += " AND [" + BaseConfigs.GetTablePrefix + "users].[username] ='" + username + "'";
                if (nickname != "") searchcondition += " AND [" + BaseConfigs.GetTablePrefix + "users].[nickname] ='" + nickname + "'";
            }

            if (UserGroup != "0")
            {
                searchcondition += " AND [" + BaseConfigs.GetTablePrefix + "users].[groupid]=" + UserGroup;
            }

            if (email != "")
            {
                searchcondition += " AND [" + BaseConfigs.GetTablePrefix + "users].[email] LIKE '%" + RegEsc(email) + "%'";
            }

            if (credits_start != "")
            {
                searchcondition += " AND [" + BaseConfigs.GetTablePrefix + "users].[credits] >=" + credits_start;
            }

            if (credits_end != "")
            {
                searchcondition += " AND [" + BaseConfigs.GetTablePrefix + "users].[credits] <=" + credits_end;
            }

            if (lastip != "")
            {
                searchcondition += " AND [" + BaseConfigs.GetTablePrefix + "users].[lastip] LIKE '%" + RegEsc(lastip) + "%'";
            }

            if (posts != "")
            {
                searchcondition += " AND [" + BaseConfigs.GetTablePrefix + "users].[posts] >=" + posts;
            }


            if (digestposts != "")
            {
                searchcondition += " AND [" + BaseConfigs.GetTablePrefix + "users].[digestposts] >=" + digestposts;
            }

            if (uid != "")
            {
                uid = uid.Replace(", ", ",");

                if (uid.IndexOf(",") == 0)
                {
                    uid = uid.Substring(1, uid.Length - 1);
                }
                if (uid.LastIndexOf(",") == (uid.Length - 1))
                {
                    uid = uid.Substring(0, uid.Length - 1);
                }

                if (uid != "")
                {
                    searchcondition += " AND [" + BaseConfigs.GetTablePrefix + "users].[uid] IN(" + uid + ")";
                }

            }

            if (ispostdatetime)
            {
                searchcondition += " AND [" + BaseConfigs.GetTablePrefix + "users].[joindate] >='" + DateTime.Parse(joindateStart).ToString("yyyy-MM-dd HH:mm:ss") + "'";
                searchcondition += " AND [" + BaseConfigs.GetTablePrefix + "users].[joindate] <='" + DateTime.Parse(joindateEnd).ToString("yyyy-MM-dd HH:mm:ss") + "'";
            }


            return searchcondition;

        }


        public DataTable Global_UserGrid_Top2(string searchcondition)
        {

            return DbHelper.ExecuteDataset("SELECT TOP 2 [" + BaseConfigs.GetTablePrefix + "users].[uid]  FROM [" + BaseConfigs.GetTablePrefix + "users] WHERE " + searchcondition).Tables[0];

        }


        public System.Collections.ArrayList CheckDbFree()
        {
          
           
            return null;
        }

        public void DbOptimize(string tablelist)
        {
           
        }

        public void UpdateAdminUsergroup(string targetadminusergroup, string sourceadminusergroup)
        {

            DbHelper.ExecuteNonQuery("UPDATE [" + BaseConfigs.GetTablePrefix + "users] SET [groupid]=" + targetadminusergroup + " WHERE [groupid]=" + sourceadminusergroup);

        }

        public void UpdateUserCredits(string formula)
        {
            DbHelper.ExecuteNonQuery("UPDATE [" + BaseConfigs.GetTablePrefix + "users] SET [credits]=" + formula);

        }

        public DataTable MailListTable(string usernamelist)
        {

            string strwhere = " WHERE [Email] Is Not null AND (";
            foreach (string username in usernamelist.Split(','))
            {
                if (username.Trim() != "")
                    strwhere += " [username] LIKE '%" + RegEsc(username.Trim()) + "%' OR ";
            }
            strwhere = strwhere.Substring(0, strwhere.Length - 3) + ")";

            DataTable dt = DbHelper.ExecuteDataset("SELECT [username],[Email]  FROM [" + BaseConfigs.GetTablePrefix + "users] " + strwhere).Tables[0];
            return dt;
        }

        public IDataReader GetTagsListByTopic(int topicid)
        {
            string sql = string.Format("SELECT [{0}tags].* FROM [{0}tags], [{0}topictags] WHERE [{0}topictags].[tagid] = [{0}tags].[tagid] AND [{0}topictags].[tid] = @topicid ORDER BY [orderid]", BaseConfigs.GetTablePrefix);

            DbParameter parm = DbHelper.MakeInParam("@topicid", (DbType)SqlDbType.Int, 4, topicid);

            return DbHelper.ExecuteReader(CommandType.Text, sql, parm);
        }

        public IDataReader GetTagInfo(int tagid)
        {
            string sql = string.Format("SELECT * FROM [{0}tags] WHERE [tagid]=@tagid", BaseConfigs.GetTablePrefix);
            DbParameter parm = DbHelper.MakeInParam("@tagid", (DbType)SqlDbType.Int, 4, tagid);

            return DbHelper.ExecuteReader(CommandType.Text, sql, parm);
        }

        public void SetLastExecuteScheduledEventDateTime(string key, string servername, DateTime lastexecuted)
        {
            string sql = string.Format("{0}setlastexecutescheduledeventdatetime", BaseConfigs.GetTablePrefix);

            DbParameter[] parms = {
                DbHelper.MakeInParam("@key", (DbType)SqlDbType.VarChar, 100, key),
                DbHelper.MakeInParam("@servername", (DbType)SqlDbType.VarChar, 100, servername),
                DbHelper.MakeInParam("@lastexecuted", (DbType)SqlDbType.DateTime, 8, lastexecuted)
            };

            DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, sql, parms);
        }

        public DateTime GetLastExecuteScheduledEventDateTime(string key, string servername)
        {
            string sql = string.Format("{0}getlastexecutescheduledeventdatetime", BaseConfigs.GetTablePrefix);

            DbParameter[] parms = {
                DbHelper.MakeInParam("@key", (DbType)SqlDbType.VarChar, 100, key),
                DbHelper.MakeInParam("@servername", (DbType)SqlDbType.VarChar, 100, servername),
                DbHelper.MakeOutParam("@lastexecuted", (DbType)SqlDbType.DateTime, 8)
            };

            DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, sql, parms);

            return Convert.IsDBNull(parms[2].Value) ? DateTime.MinValue : Convert.ToDateTime(parms[2].Value);
        }

        public void UpdateStats(string type, string variable, int count)
        {
            DbParameter[] parms = {
                DbHelper.MakeInParam("@type", (DbType)SqlDbType.Char, 10, type),
                DbHelper.MakeInParam("@variable", (DbType)SqlDbType.Char, 20, variable),
                DbHelper.MakeInParam("@count", (DbType)SqlDbType.Int, 4, count)
            };
            string commandText = string.Format("UPDATE [{0}stats] SET [count]=[count]+@count WHERE [type]=@type AND [variable]=@variable", BaseConfigs.GetTablePrefix);
            int lines = DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
            if (lines == 0)
            {
                if (count == 0)
                {
                    parms[2].Value = 1;
                }
                commandText = string.Format("INSERT INTO [{0}stats] ([type],[variable],[count]) VALUES(@type, @variable, @count)", BaseConfigs.GetTablePrefix);
                DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
            }
        }

        public void UpdateStatVars(string type, string variable, string value)
        {
            DbParameter[] parms = {
                DbHelper.MakeInParam("@type", (DbType)SqlDbType.Char, 20, type),
                DbHelper.MakeInParam("@variable", (DbType)SqlDbType.Char, 20, variable),
                DbHelper.MakeInParam("@value", (DbType)SqlDbType.Text, 0, value)
            };
            string commandText = string.Format("UPDATE [{0}statvars] SET [value]=@value WHERE [type]=@type AND [variable]=@variable", BaseConfigs.GetTablePrefix);
            int lines = DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
            if (lines == 0)
            {
                commandText = string.Format("INSERT INTO [{0}statvars] ([type],[variable],[value]) VALUES(@type, @variable, @value)", BaseConfigs.GetTablePrefix);
                DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
            }
        }

        public IDataReader GetAllStats()
        {
            string commandText = string.Format("SELECT [type], [variable], [count] FROM [{0}stats] ORDER BY [type],[variable]", BaseConfigs.GetTablePrefix);
            return DbHelper.ExecuteReader(CommandType.Text, commandText);
        }

        public IDataReader GetAllStatVars()
        {
            string commandText = string.Format("SELECT [type], [variable], [value] FROM [{0}statvars] ORDER BY [type],[variable]", BaseConfigs.GetTablePrefix);
            return DbHelper.ExecuteReader(CommandType.Text, commandText);
        }

        public IDataReader GetStatsByType(string type)
        {
            DbParameter parm = DbHelper.MakeInParam("@type", (DbType)SqlDbType.Char, 10, type);
            string commandText = string.Format("SELECT [type], [variable], [count] FROM [{0}stats] WHERE [type]=@type ORDER BY [type],[variable]", BaseConfigs.GetTablePrefix);
            return DbHelper.ExecuteReader(CommandType.Text, commandText);
        }

        public IDataReader GetStatVarsByType(string type)
        {
            DbParameter parm = DbHelper.MakeInParam("@type", (DbType)SqlDbType.Char, 10, type);
            string commandText = string.Format("SELECT [type], [variable], [value] FROM [{0}statvars] WHERE [type]=@type ORDER BY [type],[variable]", BaseConfigs.GetTablePrefix);
            return DbHelper.ExecuteReader(CommandType.Text, commandText);
        }

        public void DeleteOldDayposts()
        {
            string commandText = string.Format("DELETE FROM {0}statvars WHERE [type]='dayposts' AND [variable]<'{1}'", BaseConfigs.GetTablePrefix, DateTime.Now.AddDays(-30).ToString("yyyy-MM-dd"));
            DbHelper.ExecuteNonQuery(CommandType.Text, commandText);
        }

        public DateTime GetPostStartTime()
        {
            string commandText = string.Format("SELECT MIN([postdatetime]) FROM [{0}posts1] WHERE [invisible]=0", BaseConfigs.GetTablePrefix);
            Object obj = DbHelper.ExecuteScalar(CommandType.Text, commandText);
            if (obj == null)
                return DateTime.Now;
            return Convert.ToDateTime(obj);
        }

        public int GetForumCount()
        {
            string commandText = string.Format("SELECT COUNT(1) FROM [{0}forums] WHERE [layer]>0 AND [status]>0", BaseConfigs.GetTablePrefix);
            return (int)DbHelper.ExecuteScalar(CommandType.Text, commandText);
        }

        public int GetTodayPostCount(string posttableid)
        {
            if (!Utils.IsNumeric(posttableid))
                posttableid = "1";
            string commandText = string.Format("SELECT COUNT(1) FROM [{0}posts{1}] WHERE [postdatetime]>='{2}' AND [invisible]=0", BaseConfigs.GetTablePrefix, posttableid, DateTime.Now.ToString("yyyy-MM-dd"));
            return (int)DbHelper.ExecuteScalar(CommandType.Text, commandText);
        }

        public int GetTodayNewMemberCount()
        {
            string commandText = string.Format("SELECT COUNT(1) FROM [{0}users] WHERE [joindate]>='{1}'", BaseConfigs.GetTablePrefix, DateTime.Now.ToString("yyyy-MM-dd"));
            return (int)DbHelper.ExecuteScalar(CommandType.Text, commandText);
        }

        public int GetAdminCount()
        {
            string commandText = string.Format("SELECT COUNT(1) FROM [{0}users] WHERE [adminid]>0", BaseConfigs.GetTablePrefix, DateTime.Now.ToString("yyyy-MM-dd"));
            return (int)DbHelper.ExecuteScalar(CommandType.Text, commandText);
        }

        public int GetNonPostMemCount()
        {
            string commandText = string.Format("SELECT COUNT(1) FROM [{0}users] WHERE [posts]=0", BaseConfigs.GetTablePrefix, DateTime.Now.ToString("yyyy-MM-dd"));
            return (int)DbHelper.ExecuteScalar(CommandType.Text, commandText);
        }

        public IDataReader GetBestMember(string posttableid)
        {
            if (!Utils.IsNumeric(posttableid))
                posttableid = "1";

            string commandText = string.Format("SELECT TOP 1 [poster], COUNT(1) AS [posts] FROM [{0}posts{1}] WHERE [postdatetime]>='{2}' AND [invisible]=0 AND [posterid]>0 GROUP BY [poster] ORDER BY [posts] DESC", BaseConfigs.GetTablePrefix, posttableid, DateTime.Now.ToString("yyyy-MM-dd"));
            return DbHelper.ExecuteReader(CommandType.Text, commandText);
        }

        public IDataReader GetMonthPostsStats(string posttableid)
        {
            if (!Utils.IsNumeric(posttableid))
                posttableid = "1";

            string commandText = string.Format("SELECT COUNT(1) AS [count],MONTH([postdatetime]) AS [month],YEAR([postdatetime]) AS [year] FROM [{0}posts{1}] GROUP BY MONTH([postdatetime]),YEAR([postdatetime]) ORDER BY YEAR([postdatetime]),MONTH([postdatetime])", BaseConfigs.GetTablePrefix, posttableid);
            return DbHelper.ExecuteReader(CommandType.Text, commandText);
        }

        public IDataReader GetDayPostsStats(string posttableid)
        {
            if (!Utils.IsNumeric(posttableid))
                posttableid = "1";

            string commandText = string.Format("SELECT COUNT(1) AS [count],YEAR([postdatetime]) AS [year],MONTH([postdatetime]) AS [month],DAY([postdatetime]) AS [day] FROM [{0}posts{1}] WHERE [invisible]=0 AND [postdatetime] > '{2}' GROUP BY DAY([postdatetime]), MONTH([postdatetime]),YEAR([postdatetime]) ORDER BY YEAR([postdatetime]),MONTH([postdatetime]),DAY([postdatetime])", BaseConfigs.GetTablePrefix, posttableid, DateTime.Now.AddDays(-30).ToString("yyyy-MM-dd"));
            return DbHelper.ExecuteReader(CommandType.Text, commandText);
        }

        public IDataReader GetForumsByTopicCount(int count)
        {
            string commandText = string.Format("SELECT TOP {0} [fid], [name], [topics] FROM [{1}forums] WHERE [status]>0 AND [layer]=0 ORDER BY [topics] DESC", count, BaseConfigs.GetTablePrefix);
            return DbHelper.ExecuteReader(CommandType.Text, commandText);
        }

        public IDataReader GetForumsByPostCount(int count)
        {
            string commandText = string.Format("SELECT TOP {0} [fid], [name], [posts] FROM [{1}forums] WHERE [status]>0 AND [layer]=0 ORDER BY [posts] DESC", count, BaseConfigs.GetTablePrefix);
            return DbHelper.ExecuteReader(CommandType.Text, commandText);
        }

        public IDataReader GetForumsByMonthPostCount(int count, string posttableid)
        {
            if (!Utils.IsNumeric(posttableid))
                posttableid = "1";

            string commandText = string.Format("SELECT DISTINCT TOP {0} [p].[fid], [f].[name], COUNT([pid]) AS [posts] FROM [{1}posts{2}] [p] LEFT JOIN [{1}forums] [f] ON [p].[fid]=[f].[fid] WHERE [postdatetime]>='{3}' AND [invisible]=0 AND [posterid]>0 GROUP BY [p].[fid], [f].[name] ORDER BY [posts] DESC", count, BaseConfigs.GetTablePrefix, posttableid, DateTime.Now.AddDays(-30).ToString("yyyy-MM-dd"));
            return DbHelper.ExecuteReader(CommandType.Text, commandText);
        }

        public IDataReader GetForumsByDayPostCount(int count, string posttableid)
        {
            if (!Utils.IsNumeric(posttableid))
                posttableid = "1";

            string commandText = string.Format("SELECT DISTINCT TOP {0} [p].[fid], [f].[name], COUNT([pid]) AS [posts] FROM [{1}posts{2}] [p] LEFT JOIN [{1}forums] [f] ON [p].[fid]=[f].[fid] WHERE [postdatetime]>='{3}' AND [invisible]=0 AND [posterid]>0 GROUP BY [p].[fid], [f].[name] ORDER BY [posts] DESC", count, BaseConfigs.GetTablePrefix, posttableid, DateTime.Now.ToString("yyyy-MM-dd"));
            return DbHelper.ExecuteReader(CommandType.Text, commandText);
        }

        public IDataReader GetUsersRank(int count, string posttableid, string type)
        {
            if (!Utils.IsNumeric(posttableid))
                posttableid = "1";
            string commandText = "";
            switch(type)
            {
                case "posts":
                    commandText = string.Format("SELECT TOP {0} [username], [uid], [posts] FROM [{1}users] ORDER BY [posts] DESC, [uid]", count, BaseConfigs.GetTablePrefix);
                    break;
                case "digestposts":
                    commandText = string.Format("SELECT TOP {0} [username], [uid], [digestposts] FROM [{1}users] ORDER BY [digestposts] DESC, [uid]", count, BaseConfigs.GetTablePrefix);
                    break;
                case "thismonth":
                    commandText = string.Format("SELECT DISTINCT TOP {0} [poster] AS [username], [posterid] AS [uid], COUNT(pid) AS [posts] FROM [{1}posts{2}] WHERE [postdatetime]>='{3}' AND [invisible]=0 AND [posterid]>0 GROUP BY [poster], [posterid] ORDER BY [posts] DESC", count, BaseConfigs.GetTablePrefix, posttableid, DateTime.Now.AddDays(-30).ToString("yyyy-MM-dd"));
                    break;
                case "today":
                    commandText = string.Format("SELECT DISTINCT TOP {0} [poster] AS [username], [posterid] AS [uid], COUNT(pid) AS [posts] FROM [{1}posts{2}] WHERE [postdatetime]>='{3}' AND [invisible]=0 AND [posterid]>0 GROUP BY [poster], [posterid] ORDER BY [posts] DESC", count, BaseConfigs.GetTablePrefix, posttableid, DateTime.Now.ToString("yyyy-MM-dd"));
                    break;

                case "credits":
                    commandText = string.Format("SELECT TOP {0} [username], [uid], [credits] FROM [{1}users] ORDER BY [credits] DESC, [uid]", count, BaseConfigs.GetTablePrefix);
                    break;
                case "extcredits1":
                    commandText = string.Format("SELECT TOP {0} [username], [uid], [extcredits1] FROM [{1}users] ORDER BY [extcredits1] DESC, [uid]", count, BaseConfigs.GetTablePrefix);
                    break;
                case "extcredits2":
                    commandText = string.Format("SELECT TOP {0} [username], [uid], [extcredits2] FROM [{1}users] ORDER BY [extcredits2] DESC, [uid]", count, BaseConfigs.GetTablePrefix);
                    break;
                case "extcredits3":
                    commandText = string.Format("SELECT TOP {0} [username], [uid], [extcredits3] FROM [{1}users] ORDER BY [extcredits3] DESC, [uid]", count, BaseConfigs.GetTablePrefix);
                    break;
                case "extcredits4":
                    commandText = string.Format("SELECT TOP {0} [username], [uid], [extcredits4] FROM [{1}users] ORDER BY [extcredits4] DESC, [uid]", count, BaseConfigs.GetTablePrefix);
                    break;
                case "extcredits5":
                    commandText = string.Format("SELECT TOP {0} [username], [uid], [extcredits5] FROM [{1}users] ORDER BY [extcredits5] DESC, [uid]", count, BaseConfigs.GetTablePrefix);
                    break;
                case "extcredits6":
                    commandText = string.Format("SELECT TOP {0} [username], [uid], [extcredits6] FROM [{1}users] ORDER BY [extcredits6] DESC, [uid]", count, BaseConfigs.GetTablePrefix);
                    break;
                case "extcredits7":
                    commandText = string.Format("SELECT TOP {0} [username], [uid], [extcredits7] FROM [{1}users] ORDER BY [extcredits7] DESC, [uid]", count, BaseConfigs.GetTablePrefix);
                    break;
                case "extcredits8":
                    commandText = string.Format("SELECT TOP {0} [username], [uid], [extcredits8] FROM [{1}users] ORDER BY [extcredits8] DESC, [uid]", count, BaseConfigs.GetTablePrefix);
                    break;
                //case "oltime":
                //    commandText = string.Format("SELECT TOP {0} [username], [uid], [oltime] FROM [{1}users] ORDER BY [oltime] DESC, [uid]", count, BaseConfigs.GetTablePrefix);
                //    break;
                default:
                    return null;

            }
            return DbHelper.ExecuteReader(CommandType.Text, commandText);
        }

        public IDataReader GetSuperModerators()
        {
            string commandText = string.Format("SELECT [fid], [uid] FROM [{0}moderators] WHERE [inherited]=0 ORDER BY [displayorder]", BaseConfigs.GetTablePrefix);
            return DbHelper.ExecuteReader(CommandType.Text, commandText);
        }

        public IDataReader GetModeratorsDetails(string uids, int oltimespan)
        {
            if (!Utils.IsNumericArray(uids.Split(',')))
                return null;
            string uidstr = "m.[uid] IN ({0}) OR ";
            if (uids == string.Empty)
                uidstr = string.Empty;
            string oltimeadd1 = string.Empty;
            string oltimeadd2 = string.Empty;
            if (oltimespan > 0)
            {
                oltimeadd1 = ", o.[thismonth] AS [thismonthol], o.[total] AS [totalol]";
                oltimeadd2 = string.Format(" LEFT JOIN [{0}onlinetime] o ON o.[uid]=m.[uid]", BaseConfigs.GetTablePrefix);
            }

            string commandText = string.Format("SELECT m.[uid], m.[username], m.[adminid], m.[lastactivity], m.[credits], m.[posts]{0} FROM [{1}users] m{2} WHERE {3}m.[adminid] IN (1, 2) ORDER BY m.[adminid]", oltimeadd1, BaseConfigs.GetTablePrefix, oltimeadd2, uidstr);
                //SELECT [uid], [username], [adminid], [lastactivity], [credits], [posts], [oltime] FROM [{0}users] WHERE {1}[adminid] IN (1, 2) ORDER BY [adminid]", BaseConfigs.GetTablePrefix, uidstr);
            return DbHelper.ExecuteReader(CommandType.Text, commandText);
        }

        public void UpdateStatCount(string browser, string os, string visitorsadd)
        {
            string month = DateTime.Now.Year + DateTime.Now.Month.ToString("00");
            string dayofweek = ((int)DateTime.Now.DayOfWeek).ToString();
            string commandText = string.Format("UPDATE [{0}stats] SET [count]=[count]+1 WHERE ([type]='total' AND [variable]='hits') {1} OR ([type]='month' AND [variable]='{2}') OR ([type]='week' AND [variable]='{3}') OR ([type]='hour' AND [variable]='{4}')", BaseConfigs.GetTablePrefix, visitorsadd, month, dayofweek, DateTime.Now.Hour.ToString("00"));
            int affectedrows = DbHelper.ExecuteNonQuery(CommandType.Text, commandText);

            int updaterows = visitorsadd.Trim() == string.Empty ? 4 : 7;
            if (updaterows > affectedrows)
            {
            //    commandText = string.Format("INSERT INTO [{0}stats] ([type], [variable], [count]) VALUES ('month', '{1}', 1)", BaseConfigs.GetTablePrefix, DateTime.Now.Month.ToString("00"));
            //    DbHelper.ExecuteNonQuery(CommandType.Text, commandText);
            //}
            //else
            //{
                UpdateStats("browser", browser, 0);
                UpdateStats("os", os, 0);
                UpdateStats("total", "members", 0);
                UpdateStats("total", "guests", 0);
                UpdateStats("total", "hits", 0);
                UpdateStats("month", month, 0);
                UpdateStats("week", dayofweek, 0);
                UpdateStats("hour", DateTime.Now.Hour.ToString("00"), 0);
            }
        }


        public void ModifyConfigs(string content)
        {
            string commandText = string.Format("SELECT cfgCache FROM [{0}Configs]", BaseConfigs.GetTablePrefix);
            IDataReader dr = DbHelper.ExecuteReader(CommandType.Text, commandText);
            if (dr.Read())
            {
                DbParameter[] parms = {
                DbHelper.MakeInParam("@cfgCache", (DbType)SqlDbType.VarChar,8000,content)
                };

                commandText = string.Format("UPDATE [{0}Configs] SET [cfgCache]= @cfgCache", BaseConfigs.GetTablePrefix);

                DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
            }
            else
            {
                DbParameter[] parms = {
                DbHelper.MakeInParam("@cfgGeneral", (DbType)SqlDbType.VarChar,20,""),
                DbHelper.MakeInParam("@cfgCache", (DbType)SqlDbType.VarChar, 8000, content),
                };
                commandText = string.Format("INSERT INTO [{0}Configs] values(@cfgGeneral,@cfgCache)", BaseConfigs.GetTablePrefix);

                DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
            }
            
        }


        public string GetDntConfigs()
        {
            string commandText = string.Format("SELECT cfgCache FROM [{0}Configs]", BaseConfigs.GetTablePrefix);
            IDataReader dr = DbHelper.ExecuteReader(CommandType.Text, commandText);
            if (dr.Read())
            {
                return dr["cfgCache"].ToString();
            }
            else
            {
                return "";
            }
        }
    }
}
#endif
