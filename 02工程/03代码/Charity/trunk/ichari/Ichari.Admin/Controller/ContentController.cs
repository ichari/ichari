using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

using Ichari.Uow;
using Ichari.Admin.ViewModel;
using Ichari.Model;
using Ichari.Model.Admin;
using Ichari.Model.Enum;
using Ichari.Common;
using Ichari.Common.Extensions;

namespace Ichari.Admin
{
    /// <summary>
    /// 内容管理
    /// </summary>
    public class ContentController : BaseController
    {
        private IContentUow _uow;

        public ContentController() { }

        public ContentController(IContentUow uow)
        {
            _uow = uow;
        }

        public ActionResult Index(string tag,int? pageIndex)
        {
            ViewData["tag"] = tag;
            var model = new BaseViewModel<Content>();
            if (pageIndex.HasValue)
                model.PageIndex = pageIndex.Value;
            var list = _uow.ContentService.GetQueryList(t => t.TagName == tag).OrderByDescending(t => t.Id);
            model.PageList = new Common.Helper.PageList<Content>(list, model.PageIndex, model.PageCount);
            return View(model);
        }

        public ActionResult Add(int? id)
        {
            if (id.HasValue)
            {
                var m = _uow.ContentService.Get(t => t.Id == id.Value);
                if (m != null)
                    return View(m); 
            }
            return View(new Content());
        }

        [HttpPost]
        public ActionResult Add(Content model)
        {
            if (ModelState.IsValid)
            {
                if (model.Id == 0)
                {
                    _uow.ContentService.Add(model);
                }
                else {
                    var old = _uow.ContentService.Get(t => t.Id == model.Id);
                    if (old != null) {
                        old.Title = model.Title;
                        old.Description = model.Description;
                        old.LinkUrl = model.LinkUrl;
                        old.PicUrl = model.PicUrl;
                        old.AltName = model.AltName;
                        old.TagName = model.TagName;
                        old.ContentType = model.ContentType;
                    }
                }
                _uow.Commit();
                //delete cache
                Ichari.Cache.CacheContainer<List<Content>>.DeleteByName("Content.GetList","zcdw_hzmt");
                return RedirectToAction("Index");
            }
            return View(model);
        }
    }
}
