using System;

namespace Discuz.Entity
{
	/// <summary>
	/// ShowtopicPageAttachmentInfo 的摘要说明。
	/// </summary>
	public class ShowtopicPageAttachmentInfo : AttachmentInfo
	{
		private int m_getattachperm; //下载附件权限
		private int m_attachimgpost; //附件是否为图片
		private int m_allowread; //附件是否允许读取
		private string m_preview = string.Empty; //预览信息
		
		/// <summary>
		/// 下载附件权限
		/// </summary>
		public int Getattachperm
		{
			get { return m_getattachperm;}
			set { m_getattachperm = value;}
		}

		/// <summary>
		/// 附件是否为图片
		/// </summary>
		public int Attachimgpost
		{
			get { return m_attachimgpost;}
			set { m_attachimgpost = value;}
		}

		/// <summary>
		/// 附件是否允许读取
		/// </summary>
		public int Allowread
		{
			get { return m_allowread;}
			set { m_allowread = value;}
		}
		
		/// <summary>
		/// 预览信息
		/// </summary>
		public string Preview
		{
		    get { return m_preview; }
		    set { m_preview = value; }
		}

	}
}
