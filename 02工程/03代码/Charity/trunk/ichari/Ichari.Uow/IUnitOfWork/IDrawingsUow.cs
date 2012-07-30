using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Ichari.IService;
using Ichari.Model.Dto;

namespace Ichari.Uow
{
    public interface IDrawingsUow : IUnitOfWork
    {
        IDrawingsService DrawingsService { get; set; }
        IPrizeService PrizeService { get; set; }
        IPrizeCategoryService PrizeCategoryService { get; set; }
        IFreeCardService FreeCardService { get; set; }
        IUserInfoService UserInfoService { get; set; }
        ILoveChangeService LoveChangeService { get; set; }
        IOrderService OrderService { get; set; }

        IQueryable<DrawListDto> GetDrawList(int source);
    }
}
