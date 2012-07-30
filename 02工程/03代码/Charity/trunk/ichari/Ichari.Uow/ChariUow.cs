using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Ichari.IService;
using Ichari.Service;
using Ichari.Model;
using Ichari.Model.Dto;

namespace Ichari.Uow
{
    public partial class ChariUow : BaseUnitOfWork , IChariUow
    {
        public IUserInfoService UserInfoService { get; set;}
        public ILoveChangeService LoveChangeService { get; set; }
        public IOrderService OrderService { get; set; }
        public IOrderDetailService OrderDetailService { get; set; }
        public IPayLogService PayLogService { get; set; }
        public IAccountService AccountService { get; set; }
        public IAccountLogService AccountLogService { get; set; }
        public IContentService ContentService { get; set; }
        public IDrawingsService DrawingsService { get; set; }
        public IPrizeService PrizeService { get; set; }
        public IAddressService AddressService { get; set; }
        public IFreeCardService FreeCardService { get; set; }

        public ChariUow(string entityName)
            : base(entityName)
        {
            this.Initialize();
        }

        public ChariUow(System.Data.Objects.ObjectContext ctx)
            : base(ctx)
        {
            this.Initialize();
        }

        public override void Initialize()
        {
            this.UserInfoService = new UserInfoService(base._context);
            this.LoveChangeService = new LoveChangeService(base._context);
            this.OrderService = new Ichari.Service.OrderService(base._context);
            this.OrderDetailService = new Ichari.Service.OrderDetailService(base._context);
            this.PayLogService = new Ichari.Service.PayLogService(base._context);
            this.AccountService = new AccountService(base._context);
            this.AccountLogService = new AccountLogService(base._context);
            this.ContentService = new ContentService(base._context);
            this.DrawingsService = new DrawingsService(base._context);
            this.PrizeService = new PrizeService(base._context);
            this.AddressService = new AddressService(base._context);
            this.FreeCardService = new FreeCardService(base._context);
        }

        public IQueryable<UserAccountDto> GetAccountList(string userName)
        {
            var r = from acc in this.AccountService.GetQueryList()
                    join u in this.UserInfoService.GetQueryList()
                    on acc.UserId equals u.Id
                    orderby acc.CreateTime descending

                    select new UserAccountDto { 
                        UserName = u.UserName,
                        Id = acc.Id,
                        Amount = acc.Amount,
                        FrozenAmount = acc.FrozenAmount,
                        CreateTime = acc.CreateTime,
                        UpdateTime = acc.UpdateTime,
                        UserId = acc.UserId
                    };
            if (!string.IsNullOrEmpty(userName))
                r = r.Where(t => t.UserName == userName);
            return r;

        }

        public UserAccountDto GetUserAccountDto(int accountId)
        {
            var r = from acc in this.AccountService.GetQueryList()
                    join u in this.UserInfoService.GetQueryList()
                    on acc.UserId equals u.Id
                    where acc.Id == accountId
                    select new UserAccountDto() { 
                        Id = acc.Id,
                        UserName = u.UserName,
                        Amount = acc.Amount,
                        FrozenAmount = acc.FrozenAmount,
                        CreateTime = acc.CreateTime,
                        UpdateTime = acc.UpdateTime,
                        UserId = acc.UserId

                    };
            return r.FirstOrDefault();
        }

        private const string WinnerListCacheKey = "WinnerList.GetList";
        /// <summary>
        /// Gets the winner list sorted by winning date, winner with no account is ignored
        /// </summary>
        /// <param name="num">Number of latest winner to return</param>
        /// <returns>List of winners and prize won in string (username;prize),</returns>
        public List<WinnerListDto> GetWinnerPrizeList()
        {
            List<WinnerListDto> wList = Ichari.Cache.CacheContainer<List<WinnerListDto>>.GetByName(WinnerListCacheKey);
            if (wList != null)
                return wList;
                
            IQueryable<WinnerListDto> winList = from d in this.DrawingsService.GetQueryList(o => o.IsWinner == true && o.IsConfirrmed == true).OrderByDescending(o => o.CreateTime).Take(10)
                          join p in this.PrizeService.GetQueryList() on d.PrizeId equals p.Id
                          join u in this.UserInfoService.GetQueryList() on d.UserId equals u.Id into wl
                          from w in wl.DefaultIfEmpty() 
                          select new WinnerListDto() {
                              UserName = w.UserName,
                              GameDate = d.CreateTime,
                              PrizeName = p.Name,
                          };
            wList = winList.ToList();
            for (int i = 0; i < wList.Count(); i++)
            {
                TimeSpan t = DateTime.Now.Subtract(wList[i].GameDate);
                wList[i].TimePassed = t.Days > 0 ? t.Days + "天前" : t.Hours > 0 ? t.Hours + "小时前" : "不久前";
            }
            Ichari.Cache.CacheContainer<List<WinnerListDto>>.SetByName(WinnerListCacheKey, wList);
            return wList;
        }

        public DrawDetailDto GetDrawDetail(int drawId)
        {
            var r = from dr in DrawingsService.GetQueryList()
                    join p in PrizeService.GetQueryList()
                    on dr.PrizeId equals p.Id
                    join a in AddressService.GetQueryList()
                    on dr.AddressId equals a.Id into xx
                    where dr.Id == drawId
                    from x in xx.DefaultIfEmpty()
                    select new DrawDetailDto
                    {
                        Id = dr.Id,
                        Area = x.Area,
                        City = x.City,
                        Email = x.Email,
                        Name = x.TrueName,
                        Phone = x.Mobile,
                        PrizeName = p.Name,
                        Province = x.Province,
                        Street = x.Address1,
                        Tel = x.Tel,
                        OrderNo = dr.OrderNo,
                        CreateTime = dr.CreateTime,
                        Source = dr.Source,
                        IsVirtual = p.IsVirtual
                    };
            return r.FirstOrDefault();
        }
    }
}
