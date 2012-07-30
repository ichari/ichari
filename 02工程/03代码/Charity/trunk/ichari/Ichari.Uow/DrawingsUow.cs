using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Ichari.IService;
using Ichari.Service;
using Ichari.Model.Dto;

namespace Ichari.Uow
{
    public partial class DrawingsUow : BaseUnitOfWork, IDrawingsUow
    {
        public IDrawingsService DrawingsService { get; set; }
        public IPrizeService PrizeService { get; set; }
        public IPrizeCategoryService PrizeCategoryService { get; set; }
        public IFreeCardService FreeCardService { get; set; }
        public IUserInfoService UserInfoService { get; set; }
        public ILoveChangeService LoveChangeService {get;set;}
        public IOrderService OrderService { get; set; }

        public DrawingsUow(string entityName) 
            : base(entityName)
        {
            this.Initialize();
        }

        public override void Initialize()
        {
            this.DrawingsService = new DrawingsService(base._context);
            this.PrizeService = new PrizeService(base._context);
            this.PrizeCategoryService = new PrizeCategoryService(base._context);
            this.FreeCardService = new FreeCardService(base._context);
            this.UserInfoService = new UserInfoService(base._context);
            this.LoveChangeService = new LoveChangeService(base._context);
            this.OrderService = new OrderService(base._context);
        }

        public IQueryable<DrawListDto> GetDrawList(int source)
        {
            //throw new NotImplementedException();
            if (source == (int)Ichari.Model.Enum.GameSource.ChangeOfCare)
            {
                var r = from d in DrawingsService.GetQueryList()
                        join o in LoveChangeService.GetQueryList() on d.OrderNo equals o.UnionOrder
                        join p in PrizeService.GetQueryList() on d.PrizeId equals p.Id into px
                        from gp in px.DefaultIfEmpty()
                        join u in UserInfoService.GetQueryList() on d.UserId equals u.Id into ux
                        from g in ux.DefaultIfEmpty()
                        select new DrawListDto()
                        {
                            Id = d.Id,
                            PrizeId = gp.Id,
                            PrizeName = gp.Name,
                            UserId = g.Id,
                            UserName = g.UserName,
                            IsConfirmed = d.IsConfirrmed,
                            IsWin = d.IsWinner,
                            IsHandled = d.IsHandled ?? false,
                            Source = d.Source,
                            DrawTime = d.CreateTime,
                            OrderNo = o.UnionOrder,
                            OrderState = o.State

                        };
                return r;
            }
            else if (source == (int)Ichari.Model.Enum.GameSource.IchariDonation)
            { 
                var r = from d in DrawingsService.GetQueryList()
                        join o in OrderService.GetQueryList() on d.OrderNo equals o.TradeNo
                        join p in PrizeService.GetQueryList() on d.PrizeId equals p.Id into px
                        from gp in px.DefaultIfEmpty()
                        join u in UserInfoService.GetQueryList() on d.UserId equals u.Id into x
                        from g in x.DefaultIfEmpty()
                        select new DrawListDto()
                        {
                            Id = d.Id,
                            PrizeId = gp.Id,
                            PrizeName = gp.Name,
                            UserId = g.Id,
                            UserName = g.UserName,
                            IsConfirmed = d.IsConfirrmed,
                            IsWin = d.IsWinner,
                            IsHandled = d.IsHandled ?? false,
                            Source = d.Source,
                            DrawTime = d.CreateTime,
                            OrderNo = o.TradeNo,
                            OrderState = o.Status
                        };
                return r;
            }
            return null;
        }
    }
}
