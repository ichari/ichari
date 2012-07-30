using System;

namespace Discuz.Entity
{
	/// <summary>
	/// 附件信息描述类
	/// </summary>
	public class AttachmentInfo
	{
		
		private int m_aid;	//附件aid
		private int m_uid;	//对应的帖子书posterid
		private int m_tid;	//对应的主题tid
		private int m_pid;	//对应的帖子pid
		private string m_postdatetime;	//发布时间
		private int m_readperm;	//所需阅读权限
		private string m_filename;	//存储文件名
		private string m_description;	//描述
		private string m_filetype;	//文件类型
		private long m_filesize;	//文件尺寸
		private string m_attachment;	//附件原始文件名
		private int m_downloads;	//下载次数

		private int m_sys_index;  //非数据库字段,用来代替上传文件所对应上传组件(file)的Index
		private string m_sys_noupload; //非数据库字段,用来存放未被上传的文件名

		///<summary>
		///附件aid
		///</summary>
		public int Aid
		{
			get { return m_aid;}
			set { m_aid = value;}
		}
		///<summary>
		///对应的帖子书posterid
		///</summary>
		public int Uid
		{
			get { return m_uid;}
			set { m_uid = value;}
		}
		///<summary>
		///对应的主题tid
		///</summary>
		public int Tid
		{
			get { return m_tid;}
			set { m_tid = value;}
		}
		///<summary>
		///对应的帖子pid
		///</summary>
		public int Pid
		{
			get { return m_pid;}
			set { m_pid = value;}
		}
		///<summary>
		///发布时间
		///</summary>
		public string Postdatetime
		{
			get { return m_postdatetime;}
			set { m_postdatetime = value;}
		}
		///<summary>
		///所需阅读权限
		///</summary>
		public int Readperm
		{
			get { return m_readperm;}
			set { m_readperm = value;}
		}
		///<summary>
		///存储文件名
		///</summary>
		public string Filename
		{
			get { return m_filename;}
			set { m_filename = value;}
		}
		///<summary>
		///描述
		///</summary>
		public string Description
		{
			get { return m_description;}
			set { m_description = value;}
		}
		///<summary>
		///文件类型
		///</summary>
		public string Filetype
		{
			get { return m_filetype;}
			set { m_filetype = value;}
		}
		///<summary>
		///文件尺寸
		///</summary>
		public long Filesize
		{
			get { return m_filesize;}
			set { m_filesize = value;}
		}
		///<summary>
		///附件原始文件名
		///</summary>
		public string Attachment
		{
			get { return m_attachment;}
			set { m_attachment = value;}
		}
		///<summary>
		///下载次数
		///</summary>
		public int Downloads
		{
			get { return m_downloads;}
			set { m_downloads = value;}
		}


		///<summary>
		///非数据库字段,用来代替上传文件所对应上传组件(file)的Index
		///</summary>
		public int Sys_index
		{
			get { return m_sys_index;}
			set { m_sys_index = value;}
		}

		///<summary>
		///非数据库字段,用来存放未被上传的文件名
		///</summary>
		public string Sys_noupload
		{
			get { return m_sys_noupload;}
			set { m_sys_noupload = value;}
		}

	}
}
