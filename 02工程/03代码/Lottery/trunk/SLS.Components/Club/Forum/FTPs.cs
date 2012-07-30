using System;
using System.Text;

using Discuz.Common;
using Discuz.Config;

namespace Discuz.Forum
{
    
    /// <summary>
    /// FTP操作类
    /// </summary>
    public class FTPs
    {
        #region 声明上传信息(静态)对象

        private static FTPConfigInfo m_forumattach;

        private static FTPConfigInfo m_spaceattach;

        private static FTPConfigInfo m_albumattach;

        private static FTPConfigInfo m_mallattach;

        #endregion

        private static string m_configfilepath = AppDomain.CurrentDomain.BaseDirectory + "config/ftp.config";
   
        /// <summary>
        /// 程序刚加载时ftp.config文件修改时间
        /// </summary>
        private static DateTime m_fileoldchange;

        /// <summary>
        /// 最近ftp.config文件修改时间
        /// </summary>
        private static DateTime m_filenewchange;


        private static object lockhelper = new object();

        /// <summary>
        /// FTP信息枚举类型
        /// </summary>
        public enum FTPUploadEnum
        {
            ForumAttach = 1, //论坛附件
            SpaceAttach = 2, //空间附件
            AlbumAttach = 3,  //相册附件
            MallAttach = 4 //商场附件
        }

        /// <summary>
        /// 静态构造函数(用于初始化对象和变量)
        /// </summary>
        static FTPs()
        {
            SetFtpConfigInfo();
            m_fileoldchange = System.IO.File.GetLastWriteTime(m_configfilepath);
        }


        /// <summary>
        /// FTP配置文件监视方法
        /// </summary>
        private static void FtpFileMonitor()
        {
            //获取文件最近修改时间 
            m_filenewchange = System.IO.File.GetLastWriteTime(m_configfilepath);
            
            //当ftp.config修改时间发生变化时
            if (m_fileoldchange != m_filenewchange)
            {
                lock (lockhelper)
                {
                    if (m_fileoldchange != m_filenewchange)
                    {
                        //当文件发生修改(时间变化)则重新设置相关FTP信息对象
                        SetFtpConfigInfo();
                        m_fileoldchange = m_filenewchange;
                    }
                }
            }
        }

        /// <summary>
        /// 设置FTP对象信息
        /// </summary>
        private static void SetFtpConfigInfo()
        {
            FTPConfigInfoCollection ftpconfiginfocollection = (FTPConfigInfoCollection)SerializationHelper.Load(typeof(FTPConfigInfoCollection), m_configfilepath);

            FTPConfigInfoCollection.FTPConfigInfoCollectionEnumerator fcice = ftpconfiginfocollection.GetEnumerator();

            //遍历集合并设置相应的FTP信息(静态)对象
            while (fcice.MoveNext())
            {
                if (fcice.Current.Name == "ForumAttach")
                {
                    m_forumattach = fcice.Current;
                    continue;
                }

                if (fcice.Current.Name == "SpaceAttach")
                {
                    m_spaceattach = fcice.Current;
                    continue;
                }

                if (fcice.Current.Name == "AlbumAttach")
                {
                    m_albumattach = fcice.Current;
                    continue;
                }

                if (fcice.Current.Name == "MallAttach")
                {
                    m_mallattach = fcice.Current;
                    continue;
                }
            }
        }

        /// <summary>
        /// 论坛附件FTP信息
        /// </summary>
        public static FTPConfigInfo GetForumAttachInfo
        {
            get
            {
                FtpFileMonitor();
                return m_forumattach;
            }
        }

        /// <summary>
        /// 空间附件FTP信息
        /// </summary>
        public static FTPConfigInfo GetSpaceAttachInfo
        {
            get
            {
                FtpFileMonitor();
                return m_spaceattach;
            }
        }

        /// <summary>
        /// 相册附件FTP信息
        /// </summary>
        public static FTPConfigInfo GetAlbumAttachInfo
        {
            get
            {
                FtpFileMonitor();
                return m_albumattach;
            }
        }

        /// <summary>
        /// 相册附件FTP信息
        /// </summary>
        public static FTPConfigInfo GetMallAttachInfo
        {
            get
            {
                FtpFileMonitor();
                return m_mallattach;
            }
        }

        public FTPs()
        { }


       
        #region 异步FTP上传文件

        private delegate bool delegateUpLoadFile(string path, string file, FTPUploadEnum ftpuploadname);

        //异步FTP上传文件代理
        private delegateUpLoadFile upload_aysncallback;

        public void AsyncUpLoadFile(string path, string file, FTPUploadEnum ftpuploadname)
        {
            upload_aysncallback = new delegateUpLoadFile(UpLoadFile);
            upload_aysncallback.BeginInvoke(path, file, ftpuploadname, null, null);
        }

        #endregion


        /// <summary>
        /// 普通FTP上传文件
        /// </summary>
        /// <param name="file">要FTP上传的文件</param>
        /// <returns>上传是否成功</returns>
        public bool UpLoadFile(string path, string file, FTPUploadEnum ftpuploadname)
        {
            FTP ftpupload = new FTP();
            //转换路径分割符为"/"
            path = path.Replace("\\", "/");
            path = path.StartsWith("/") ? path : "/" +path ;

            //删除file参数文件
            bool delfile = true;
            //根据上传名称确定上传的FTP服务器
            switch (ftpuploadname)
            {
                //论坛附件
                case FTPUploadEnum.ForumAttach:
                    {
                        ftpupload = new FTP(m_forumattach.Serveraddress, m_forumattach.Serverport, m_forumattach.Username, m_forumattach.Password, m_forumattach.Timeout);
                        path = m_forumattach.Uploadpath + path;
                        delfile = (m_forumattach.Reservelocalattach == 1) ? false : true;
                        break;
                    }

                //空间附件
                case FTPUploadEnum.SpaceAttach:
                    {
                        ftpupload = new FTP(m_spaceattach.Serveraddress, m_spaceattach.Serverport, m_spaceattach.Username, m_spaceattach.Password, m_spaceattach.Timeout);
                        path = m_spaceattach.Uploadpath + path;
                        delfile = (m_spaceattach.Reservelocalattach == 1) ? false : true;
                        break;
                    }

                //相册附件
                case FTPUploadEnum.AlbumAttach:
                    {
                        ftpupload = new FTP(m_albumattach.Serveraddress, m_albumattach.Serverport, m_albumattach.Username, m_albumattach.Password, m_albumattach.Timeout);
                        path = m_albumattach.Uploadpath + path;
                        delfile = (m_albumattach.Reservelocalattach == 1) ? false : true;
                        break;
                    }
                //商城附件
                case FTPUploadEnum.MallAttach:
                    {
                        ftpupload = new FTP(m_mallattach.Serveraddress, m_mallattach.Serverport, m_mallattach.Username, m_mallattach.Password, m_mallattach.Timeout);
                        path = m_mallattach.Uploadpath + path;
                        delfile = (m_mallattach.Reservelocalattach == 1) ? false : true;
                        break;
                    }
            }

            //切换到指定路径下,如果目录不存在,将创建
            if (!ftpupload.ChangeDir(path))
            {
                //ftpupload.MakeDir(path);
                foreach (string pathstr in path.Split('/'))
                {
                    if (pathstr.Trim() != "")
                    {
                        ftpupload.MakeDir(pathstr);
                        ftpupload.ChangeDir(pathstr);
                    }
                }
                
            }
            
            ftpupload.Connect();

            if (!ftpupload.IsConnected)
            {
                return false;
            }
            int perc = 0;

            //绑定要上传的文件
            if (!ftpupload.OpenUpload(file, System.IO.Path.GetFileName(file)))
            {
                ftpupload.Disconnect();
                return false;
            }

            //开始进行上传
            while (ftpupload.DoUpload() > 0)
            {
                perc = (int)(((ftpupload.BytesTotal) * 100) / ftpupload.FileSize);
            }

            ftpupload.Disconnect();

            //(如存在)删除指定目录下的文件
            if (delfile && Utils.FileExists(file))
            {
                System.IO.File.Delete(file);
            }
          

            if (perc >= 100)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// FTP连接测试
        /// </summary>
        /// <param name="Serveraddress">FTP服务器地址</param>
        /// <param name="Serverport">FTP端口</param>
        /// <param name="Username">用户名</param>
        /// <param name="Password">密码</param>
        /// <param name="Timeout">超时时间(秒)</param>
        /// <param name="uploadpath">附件保存路径</param>
        /// <param name="message">返回信息</param>
        /// <returns>是否可用</returns>
        public bool TestConnect(string Serveraddress, int Serverport, string Username, string Password, int Timeout, string uploadpath, ref string message)
        {
            FTP ftpupload = new FTP(Serveraddress, Serverport, Username, Password, Timeout);
            bool isvalid = ftpupload.Connect();
            if (!isvalid)
            {
                message = ftpupload.errormessage;
                return isvalid;
            }

            //切换到指定路径下,如果目录不存在,将创建
            if (!ftpupload.ChangeDir(uploadpath))
            {
                ftpupload.MakeDir(uploadpath);
                if (!ftpupload.ChangeDir(uploadpath))
                {
                    message += ftpupload.errormessage;
                    isvalid = false;
                }
            }
            
            return isvalid;
        }

     }
}
