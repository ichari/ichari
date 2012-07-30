using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Discuz.Common;
using Discuz.Config;

namespace Discuz.Forum.ScheduledEvents
{
    public sealed class EventLogs
    {
        public static void WriteFailedLog(string logContent)
        {
            new Log("Forum").Write(logContent);

            #region Discuz 原始方法

            //string LogPath = AppDomain.CurrentDomain.BaseDirectory + @"\App_Log\";
            //if (!Directory.Exists(LogPath))
            //{
            //    Directory.CreateDirectory(LogPath);
            //}
            //string LogFileName = LogPath + DateTime.Now.Date.ToString("yyyy-MM-dd") + ".txt";
            //StringBuilder builder = new StringBuilder();
            //builder.Append(DateTime.Now);
            //builder.Append("\t");
            //builder.Append(Environment.MachineName);
            //builder.Append("\t");
            //builder.Append(logContent);
            //builder.Append("\r\n");
            //using (FileStream fs = new FileStream(LogFileName, FileMode.Append, FileAccess.Write, FileShare.Write))
            //{
            //    StreamWriter writer = new StreamWriter(fs, System.Text.Encoding.GetEncoding("GBK"));
            //    writer.WriteLine(builder.ToString());
            //    writer.Close();
            //}

            #endregion
        }

        public class Log
        {
            private string PathName;
            private string FileName;

            /// <summary>
            /// 构造 Log
            /// </summary>
            /// <param name="pathname">相对于网站根目录 App_Log 目录的相对路径，如： System， 就相当于 ~/App_Log/System/</param>
            public Log(string pathname)
            {
                if (String.IsNullOrEmpty(pathname))
                {
                    throw new Exception("没有初始化 Log 类的 PathName 变量");
                }

                PathName = System.AppDomain.CurrentDomain.BaseDirectory + "App_Log\\" + pathname;

                if (!Directory.Exists(PathName))
                {
                    try
                    {
                        Directory.CreateDirectory(PathName);
                    }
                    catch { }
                }

                if (!Directory.Exists(PathName))
                {
                    PathName = System.AppDomain.CurrentDomain.BaseDirectory + "App_Log";

                    if (!Directory.Exists(PathName))
                    {
                        try
                        {
                            Directory.CreateDirectory(PathName);
                        }
                        catch { }
                    }

                    if (!Directory.Exists(PathName))
                    {
                        PathName = System.AppDomain.CurrentDomain.BaseDirectory;
                    }
                }

                FileName = PathName + "\\" + DateTime.Now.ToString("yyyy-MM-dd") + ".log";
            }

            public void Write(string Message)
            {
                using (FileStream fs = new FileStream(FileName, FileMode.Append, FileAccess.Write, FileShare.Write))
                {
                    StreamWriter writer = new StreamWriter(fs, System.Text.Encoding.GetEncoding("GBK"));

                    writer.WriteLine(System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":" + System.DateTime.Now.Millisecond.ToString() + "\t\t" + Message + "\r\n");
                    writer.Close();
                }
            }
        }
    }
}
