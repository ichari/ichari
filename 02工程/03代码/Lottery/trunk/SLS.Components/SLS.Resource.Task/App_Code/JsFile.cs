using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace SLS.Resource.Task
{
    /// <summary>
    /// 生成JS文件
    /// </summary>
    public class JsFile
    {

        private string PathName;
        private string FileName;

        /// <summary>
        /// 构造 Log
        /// </summary>
        /// <param name="pathname">相对于网站根目录的相对路径</param>
        public JsFile(string pathname, string fileName)
        {
            if (String.IsNullOrEmpty(pathname))
            {
                throw new Exception("没有初始化 Log 类的 PathName 变量");
            }

            PathName = pathname;

            if (!Directory.Exists(PathName))
            {
                try
                {
                    Directory.CreateDirectory(PathName);
                }
                catch { }
            }

            FileName = PathName + @"\" + fileName;
        }

        public void Write(string Message)
        {
            if (String.IsNullOrEmpty(FileName))
            {
                return;
            }

            using (FileStream fs = new FileStream(FileName, FileMode.Create, FileAccess.Write, FileShare.Write))
            {
                StreamWriter writer = new StreamWriter(fs, System.Text.Encoding.GetEncoding("utf-8"));

                try
                {
                    writer.WriteLine(Message);
                }
                catch { }

                writer.Close();
            }
        }

    }
}
