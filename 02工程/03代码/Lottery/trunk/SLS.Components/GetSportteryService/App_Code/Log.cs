using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.IO;

namespace GetSportteryService
{
    /// <summary>
    ///Log 的摘要说明
    /// </summary>
    public class Log
    {
        private string PathName;
        private string FileName;

        /// <summary>
        /// 构造 Log
        /// </summary>
        /// <param name="pathname">相对于当前程序目录下 LogFiles 目录的相对路径，如：System， 就相当于 .\LogFiles\System\</param>
        public Log(string pathname)
        {
            if (String.IsNullOrEmpty(pathname))
            {
                throw new Exception("没有初始化 Log 类的 PathName 变量");
            }

            PathName = System.AppDomain.CurrentDomain.BaseDirectory + "LogFiles\\" + pathname;

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
                PathName = System.AppDomain.CurrentDomain.BaseDirectory + "LogFiles";

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
            if (String.IsNullOrEmpty(FileName))
            {
                return;
            }

            using (FileStream fs = new FileStream(FileName, FileMode.Append, FileAccess.Write, FileShare.Write))
            {
                StreamWriter writer = new StreamWriter(fs, System.Text.Encoding.GetEncoding("GBK"));

                try
                {
                    writer.WriteLine(System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":" + System.DateTime.Now.Millisecond.ToString() + "\t\t" + Message + "");
                }
                catch { }

                writer.Close();
            }
        }
    }
}
