#if NET1
#else
using System;
using System.Collections.Generic;
using System.Text;
using Discuz.Config;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;

namespace Discuz.Data.SqlServer
{
    public partial class DataProvider : IDataProvider
    {
        //取得分类
        public IDataReader GetHelpList(int id)
        {
            DbParameter[] parms = {
                                        DbHelper.MakeInParam("@id", (DbType)SqlDbType.Int, 4, id),
                                        
                                    };
            string sql = "SELECT [id],[title],[message],[pid],[orderby] FROM [" + BaseConfigs.GetTablePrefix + "help] WHERE [pid]=@id OR [id]=@id ORDER BY [pid] ASC, [orderby] ASC";

            return DbHelper.ExecuteReader(CommandType.Text, sql,parms);
    
        
        }


        public IDataReader ShowHelp(int id)
        {
            DbParameter[] parms = {
                                        DbHelper.MakeInParam("@id", (DbType)SqlDbType.Int, 4, id),
                                        
                                    };
            string sql = "SELECT [title],[message],[pid],[orderby] FROM [" + BaseConfigs.GetTablePrefix + "help] WHERE [id]=@id";
            return DbHelper.ExecuteReader(CommandType.Text, sql,parms);

        }


        public IDataReader GetHelpClass()
        {

            string sql = "SELECT [id] FROM [" + BaseConfigs.GetTablePrefix + "help] WHERE [pid]=0 ORDER BY [orderby] ASC";
            return DbHelper.ExecuteReader(CommandType.Text, sql);
        }
        


        public void AddHelp(string title,string message,int pid,int orderby)
        {
            DbParameter[] parms = {
                                        DbHelper.MakeInParam("@title", (DbType)SqlDbType.Char, 100, title),
                                        DbHelper.MakeInParam("@message", (DbType)SqlDbType.NText,0,message),
                                        DbHelper.MakeInParam("@pid", (DbType)SqlDbType.Int,4, pid),
                                        DbHelper.MakeInParam("@orderby", (DbType)SqlDbType.Int, 4, orderby)
                                        
                                    };
            string sql = "INSERT INTO [" + BaseConfigs.GetTablePrefix + "help]([title],[message],[pid],[orderby]) VALUES(@title,@message,@pid,@orderby)";
        DbHelper.ExecuteNonQuery(CommandType.Text,sql,parms);
        }

        public void DelHelp(string idlist)
        {
            //DbParameter[] parms = {
            //                            DbHelper.MakeInParam("@idlist", (DbType)SqlDbType.Int, 100, idlist)
                                       
                                        
            //                        };

            string sql = "DELETE FROM [" + BaseConfigs.GetTablePrefix + "help] WHERE [id] IN ("+idlist+") OR [pid] IN ("+idlist+")";
            DbHelper.ExecuteNonQuery(CommandType.Text, sql);
        }

        public void ModHelp(int id,string title,string message,int pid,int orderby)
        {
            DbParameter[] parms = {
                                        DbHelper.MakeInParam("@title", (DbType)SqlDbType.Char, 100, title),
                                        DbHelper.MakeInParam("@message", (DbType)SqlDbType.NText,0,message),
                                        DbHelper.MakeInParam("@pid", (DbType)SqlDbType.Int,4, pid),
                                        DbHelper.MakeInParam("@orderby", (DbType)SqlDbType.Int, 4, orderby),
                                        DbHelper.MakeInParam("@id", (DbType)SqlDbType.Int, 4, id)
                                        
                                    };

            string sql = "UPDATE [" + BaseConfigs.GetTablePrefix + "help] SET [title]=@title,[message]=@message,[pid]=@pid,[orderby]=@orderby WHERE [id]=@id";
            DbHelper.ExecuteNonQuery(CommandType.Text,sql,parms);
        
        }

        //public string  admingrid()
        //{
        //    string sql = "select *,case pid when 0 then id else pid end as o from dnt_help order by o";

        //    return sql;
        
        
        //}

        public int HelpCount()
        {
            string sql = "SELECT COUNT(*) FROM [" + BaseConfigs.GetTablePrefix + "help]";
            return int.Parse(DbHelper.ExecuteScalar(CommandType.Text, sql).ToString());
        
        }

        public string BindHelpType()

        {
            string sql = "SELECT [id],[title] FROM [" + BaseConfigs.GetTablePrefix + "help] WHERE [pid]=0 ORDER BY [orderby] ASC";
            return sql;
        
        }

        public void UpOrder(string orderby, string id)
        {
            DbParameter[] parms = {
                                        DbHelper.MakeInParam("@orderby", (DbType)SqlDbType.Char, 100, orderby),
                                        DbHelper.MakeInParam("@id", (DbType)SqlDbType.VarChar, 100,id),
                                        
                                        
                                    };

            string sql = "UPDATE [" + BaseConfigs.GetTablePrefix + "help] SET [ORDERBY]=@orderby  Where id=@id";
            DbHelper.ExecuteNonQuery(CommandType.Text, sql, parms);

        }

    }
}
#endif