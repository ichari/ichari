using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Ichari.Model;
using Ichari.Model.Admin;
using Ichari.IRepository;
using Ichari.Repository;

namespace Ichari.Service  
{

	public class ContentService : BaseService<Content>, Ichari.IService.IContentService
    {
        #region CacheKey
        private string ContentListCacheKey = "Content.GetList";
        #endregion

        private IContentRepository _contentRepository;
	    
	    public ContentService()
	    {
	        this._contentRepository = new ContentRepository();
	        base._repository = this._contentRepository;
	    }
	    
	    public ContentService(System.Data.Objects.ObjectContext context)
	    {
	        this._contentRepository = new ContentRepository(context);
	        base._repository = this._contentRepository;
	    }

        public List<Content> GetListByTagsWithCache(string tags)
        {
            if (string.IsNullOrEmpty(tags))
                throw new ArgumentException("tags不能为空");

            var r = Ichari.Cache.CacheContainer<List<Content>>.GetByName(ContentListCacheKey, tags);
            if (r != null)
                return r;

            var tagArr = tags.Split('_');
            var list = _contentRepository.GetQueryList().Where(t => tagArr.Contains(t.TagName));
            if (list != null)
                Ichari.Cache.CacheContainer<List<Content>>.SetByName(ContentListCacheKey, list.ToList(), tags);
            return list == null ? null : list.ToList();
        }
	}
}
