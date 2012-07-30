using System;
using System.Text;
using System.Data.Common;
using System.Data;
using System.Collections;

using Discuz.Entity;
using Discuz.Data;

namespace Discuz.Forum
{
    public class Helps
    {
        /// <summary>
        /// 获取帮助列表
        /// </summary>
        /// <returns>帮助列表</returns>
        public static ArrayList GetHelpList()
        {
            IDataReader readclass = DatabaseProvider.GetInstance().GetHelpClass();
            ArrayList helplist = new ArrayList();

            if (readclass != null)
            {
                while (readclass.Read())
                {

                    IDataReader reader = DatabaseProvider.GetInstance().GetHelpList(int.Parse(readclass["id"].ToString()));

                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            HelpInfo info = new HelpInfo();
                            info.Id = int.Parse(reader["id"].ToString());
                            info.Title = reader["title"].ToString();
                            info.Message = reader["message"].ToString();
                            info.Pid = int.Parse(reader["pid"].ToString());
                            info.Orderby = int.Parse(reader["orderby"].ToString());
                            helplist.Add(info);
                        }
                        reader.Close();
                    }
                }
                readclass.Close();
            }
            return helplist;
        }

        /// <summary>
        /// 获取帮助内容
        /// </summary>
        /// <param name="id"></param>
        /// <returns>帮助内容</returns>
        public static HelpInfo getmessage(int id)
        {
            IDataReader reader = DatabaseProvider.GetInstance().ShowHelp(id);
            HelpInfo info = new HelpInfo();
            if (reader != null)
            {
                if (reader.Read())
                {
                    info.Title = reader["title"].ToString();
                    info.Message = reader["message"].ToString();
                    info.Pid = int.Parse(reader["pid"].ToString());
                    info.Orderby = int.Parse(reader["orderby"].ToString());
                    return info;
                }
                reader.Close();
            }
            return null;
        }

        /// <summary>
        /// 获取帮助数量
        /// </summary>
        /// <returns>帮助数量</returns>
        public static int helpcount()
        {

            return DatabaseProvider.GetInstance().HelpCount();
        }

        /// <summary>
        /// 更新帮助信息
        /// </summary>
        /// <param name="id">帮助ID</param>
        /// <param name="title">帮助标题</param>
        /// <param name="message">帮助内容</param>
        /// <param name="pid">帮助</param>
        /// <param name="orderby">排序方式</param>
        public static void updatehelp(int id, string title, string message, int pid, int orderby)
        {
            DatabaseProvider.GetInstance().ModHelp(id, title, message, pid, orderby);
        }

        /// <summary>
        /// 增加帮助
        /// </summary>
        /// <param name="title">帮助标题</param>
        /// <param name="message">帮助内容</param>
        /// <param name="pid">帮助</param>
        public static void addhelp(string title, string message, int pid)
        {
            int count = helpcount();
            DatabaseProvider.GetInstance().AddHelp(title, message, pid, count);
        }

        /// <summary>
        /// 删除帮助
        /// </summary>
        /// <param name="idlist">帮助ID序列</param>
        public static void delhelp(string idlist)
        {
            DatabaseProvider.GetInstance().DelHelp(idlist);

        }

        /// <summary>
        /// 返回帮助的分类列表的SQL语句
        /// </summary>
        /// <returns>帮助的分类列表的SQL语句</returns>
        public static string bindhelptype()
        {

            return DatabaseProvider.GetInstance().BindHelpType();
        }


        /// <summary>
        /// 通过PID来确定是否为分类
        /// </summary>
        /// <param name="pid">属于的分类ID</param>
        /// <returns>是否为分类</returns>
        public static bool choosepage(int pid)
        {

            if (pid == 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }


        /// <summary>
        /// 获取帮助分类以及相应帮助主题
        /// </summary>
        /// <param name="helpid"></param>
        /// <returns>帮助分类以及相应帮助主题</returns>
        public static ArrayList GetHelpList(int helpid)
        {
            IDataReader reader = DatabaseProvider.GetInstance().GetHelpList(helpid);
            ArrayList helplist = new ArrayList();
            if (reader != null)
            {
                while (reader.Read())
                {
                    HelpInfo info = new HelpInfo();
                    info.Id = int.Parse(reader["id"].ToString());
                    info.Title = reader["title"].ToString();
                    info.Message = reader["message"].ToString();
                    info.Pid = int.Parse(reader["pid"].ToString());
                    info.Orderby = int.Parse(reader["orderby"].ToString());
                    helplist.Add(info);
                }
                reader.Close();
            }
            return helplist.Count > 0 ? helplist : null;
        }
    }
}
