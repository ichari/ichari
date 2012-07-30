using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Ichari.Model;
using Ichari.Model.Admin;

namespace Ichari.IService  
{

	public interface IContentService : IService<Content>
	{
        /// <summary>
        /// 获取图文链，先从缓存获取
        /// </summary>
        /// <param name="tags">标签字符串，使用“_”分割</param>
        /// <returns></returns>
        List<Content> GetListByTagsWithCache(string tags);
	}
}
