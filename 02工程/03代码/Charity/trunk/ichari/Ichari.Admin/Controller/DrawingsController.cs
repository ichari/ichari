using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

using Ichari.Model;
using Ichari.Uow;
using Ichari.Admin.ViewModel;
using Ichari.Model.Admin;
using Ichari.Model.Validation;
using Ichari.Model.Dto;

namespace Ichari.Admin
{
    public class DrawingsController : BaseController
    {
        private IDrawingsUow _uow;
        private IAdminUow _auow;

        public DrawingsController() { }

        public DrawingsController(IDrawingsUow uow, IAdminUow auow)
        {
            _uow = uow;
            _auow = auow;
        }

        public ActionResult Index(int? source,int? pageIndex,string orderNo,bool? isWin)
        {
            if (Request.HttpMethod == "POST")
                pageIndex = 1;
            source = source ?? (int)Ichari.Model.Enum.GameSource.ChangeOfCare;
            var dvm = new DrawingsViewModel();

            if (pageIndex.HasValue)
                dvm.PageIndex = pageIndex.Value;

            var dlist = _uow.GetDrawList(source.Value);
            if (!string.IsNullOrEmpty(orderNo))
                dlist = dlist.Where(t => t.OrderNo == orderNo);
            if (isWin.HasValue)
                dlist = dlist.Where(t => t.IsWin == isWin.Value);

            dvm.PageList = new Common.Helper.PageList<DrawListDto>(dlist.OrderByDescending(t => t.DrawTime), dvm.PageIndex, dvm.PageCount);
            //dvm.DrawingsList = new Common.Helper.PageList<Drawings>(dlist.OrderByDescending(t => t.CreateTime), dvm.PageIndex, dvm.PageCount);
            dvm.SubMenuList = GetSubMenuList();
            ViewData["isWin"] = isWin;
            ViewData["source"] = source;
            ViewData["orderNo"] = orderNo;
            return View(dvm);
        }
        #region Prize
        public ActionResult Prize(int? pageIndex)
        {
            var pvm = new PrizeViewModel();

            if (pageIndex.HasValue)
                pvm.PageIndex = pageIndex.Value;
            var plist = _uow.PrizeService.GetQueryList().OrderBy(o => o.Id);
            pvm.PrizeList = new Common.Helper.PageList<Prize>(plist, pvm.PageIndex, pvm.PageCount);
            pvm.SubMenuList = GetSubMenuList();

            return View(pvm);
        }

        public ActionResult EditPrize(int id)
        {
            var model = _uow.PrizeService.Get(t => t.Id == id);
            return View(model);
        }

        private IList<SelectListItem> GetCatList()
        {
            var pcl = _uow.PrizeCategoryService.GetList().OrderBy(o => o.Id);
            PrizeCategory[] apc = pcl.ToArray();
            IList<SelectListItem> catList = new List<SelectListItem>();
            SelectListItem sli = new SelectListItem();
            sli.Text = "";
            sli.Value = "";
            sli.Selected = true;
            catList.Add(sli);
            foreach (PrizeCategory pc in apc)
            {
                sli = new SelectListItem();
                sli.Text = pc.Name;
                sli.Value = pc.Id.ToString();
                catList.Add(sli);
            }
            return catList;
        }

        public ActionResult AddPrize(int? prizeId)
        {
            ViewData["CatList"] = GetCatList();
            if (prizeId.HasValue) {
                var p = _uow.PrizeService.Get(t => t.Id == prizeId.Value);
                return View(p);
            }
            return View();
        }

        [HttpPost]
        public ActionResult AddPrize(Prize newPrize)
        {
            if (newPrize.Id == 0)
            {
                newPrize.IsEnabled = true;
                int catID = newPrize.CategoryId;
                Prize old_pz = _uow.PrizeService.Get(o => o.CategoryId == catID && o.Name == newPrize.Name);
                ViewData["CatList"] = GetCatList();
                if (old_pz != null)
                {
                    ModelState.AddModelError("", "奖品已存在");
                    return View();
                }
                _uow.PrizeService.Add(newPrize);
            }
            else {
                var oldPz = _uow.PrizeService.Get(t => t.Id == newPrize.Id);
                oldPz.CategoryId = newPrize.CategoryId;
                oldPz.Name = newPrize.Name;
                oldPz.ImgUrl = newPrize.ImgUrl;
                oldPz.PrizeCount = newPrize.PrizeCount;
                oldPz.Probability = newPrize.Probability;
                oldPz.Angle = newPrize.Angle;
                oldPz.IsEnabled = newPrize.IsEnabled;
                oldPz.IsVirtual = newPrize.IsVirtual;
                oldPz.FreeCardType = newPrize.FreeCardType;
            }
            _uow.Commit();

            return RedirectToAction("Prize");
        }
        public ActionResult IncPrizeQty(long id)
        {
            Prize p = _uow.PrizeService.Get(o => o.Id == id);
            if (p != null)
            {
                int qty;
                if (int.TryParse(Request.QueryString["q"], out qty))
                {
                    p.PrizeCount += qty;
                    _uow.Commit();
                }
            }
            return RedirectToAction("Prize");
        }
        #endregion

        #region Prize Category
        public ActionResult PrizeCategory(int? pageIndex)
        {
            var pcvm = new PrizeCategoryViewModel();
            
            if (pageIndex.HasValue)
                pcvm.PageIndex = pageIndex.Value;
            var pclist = _uow.PrizeCategoryService.GetQueryList().OrderBy(o => o.Id);
            pcvm.PrizeCategoryList = new Common.Helper.PageList<PrizeCategory>(pclist, pcvm.PageIndex, pcvm.PageCount);
            pcvm.SubMenuList = GetSubMenuList();

            return View(pcvm);
        }

        public ActionResult AddPrizeCategory()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddPrizeCategory(PrizeCatValidation pcv)
        {
            if (ModelState.IsValid)
            {
                PrizeCategory pc = _uow.PrizeCategoryService.Get(o => o.Name == pcv.Name);
                if (pc == null)
                {
                    pc = new PrizeCategory();
                    pc.Name = pcv.Name;
                    _uow.PrizeCategoryService.Add(pc);
                    _uow.Commit();

                    return RedirectToAction("PrizeCategory");
                }
                ModelState.AddModelError("", "类别已存在");
                return View();
            }

            return View();
        }
        #endregion
        
        

        public ActionResult Detail(int id)
        {            
            var vm = new DrawingsViewModel();
            var chariUow = _uow.GetUow<IChariUow,ChariUow>();
            vm.Detail = chariUow.GetDrawDetail(id);
            if (vm.Detail.Source == (int)Ichari.Model.Enum.GameSource.ChangeOfCare) {
                vm.UnionOrder = chariUow.LoveChangeService.Get(t => t.UnionOrder == vm.Detail.OrderNo);
            }
            else if (vm.Detail.Source == (int)Ichari.Model.Enum.GameSource.IchariDonation) {
                vm.ChariOrder = chariUow.OrderService.Get(t => t.TradeNo == vm.Detail.OrderNo);
            }
            return View(vm);
        }

        public ActionResult Freight(int id)
        {
            var chariUow = _uow.GetUow<IChariUow,ChariUow>();
            var draw = _uow.DrawingsService.Get(t => t.Id == id);
            if (draw.Source == (int)Ichari.Model.Enum.GameSource.ChangeOfCare) {
                var lc = chariUow.LoveChangeService.Get(t => t.UnionOrder == draw.OrderNo);
                lc.State = (int)Ichari.Model.Enum.OrderState.Freighted;
            }
            else if (draw.Source == (int)Ichari.Model.Enum.GameSource.IchariDonation) {
                var o = chariUow.OrderService.Get(t => t.TradeNo == draw.OrderNo);
                o.Status = (int)Ichari.Model.Enum.OrderState.Freighted;
            }
            chariUow.Commit();
            base.SetAlertMsg("操作成功");
            return RedirectToAction("Detail", new { id = id });
        }

        public ActionResult Card(int? pageIndex)
        {
            pageIndex = pageIndex ?? 1;            
            var model = new BaseViewModel<FreeCard>();
            model.PageIndex = pageIndex.Value;
            var list = _uow.FreeCardService.GetQueryList();
            model.PageList = new Common.Helper.PageList<FreeCard>(list.OrderByDescending(t => t.CreateTime), model.PageIndex, model.PageCount);

            return View(model);
        }

        public ActionResult AddCard()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddCard(int cardType,int cardCount)
        {
            if (cardCount <= 0)
                return View();

            var r = new Random();
            for (int i = 0; i < cardCount; i++) {
                var c = new FreeCard();
                c.Id = System.Guid.NewGuid();
                c.CardType = cardType;
                c.CardNo = Ichari.Common.WebUtils.GenTradeNo(r);
                c.Password = GenCardPwd(c.Id.ToString(), c.CardNo);
                c.CreateTime = DateTime.Now;
                c.IsEnabled = true;
                _uow.FreeCardService.Add(c);
            }
            _uow.Commit();
            return RedirectToAction("AddCard");
        }

        private string GenCardPwd(string guid,string cardNo)
        {
            return guid.Replace("-", "").Substring(0,20);
        }
    }
}
