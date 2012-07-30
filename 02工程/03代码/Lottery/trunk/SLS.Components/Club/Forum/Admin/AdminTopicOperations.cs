using System;

namespace Discuz.Forum
{
	/// <summary>
	/// AdminTopicOperationFactory 的摘要说明。
	/// 后台主题操作类
	/// </summary>
	public class AdminTopicOperations : Discuz.Forum.TopicAdmins
	{
		public AdminTopicOperations()
		{
		}

		public static int DeleteAttachmentByTid(string topicidlist)
		{
			return Discuz.Forum.Attachments.DeleteAttachmentByTid(topicidlist);
		}
	}
}
