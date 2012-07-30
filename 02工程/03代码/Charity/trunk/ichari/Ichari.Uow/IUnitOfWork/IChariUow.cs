using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Ichari.IService;
using Ichari.Model;
using Ichari.Model.Dto;

namespace Ichari.Uow
{
    public interface IChariUow : IUnitOfWork
    {
        IUserInfoService UserInfoService { get; set; }
        /// <summary>
        /// 爱心零钱支付记录Service
        /// </summary>
        ILoveChangeService LoveChangeService { get; set; }

        IOrderService OrderService { get; set; }
        IOrderDetailService OrderDetailService { get; set; }
        IPayLogService PayLogService { get; set; }
        IAccountService AccountService { get; set; }
        IAccountLogService AccountLogService { get; set; }
        IContentService ContentService { get; set; }
        IDrawingsService DrawingsService { get; set; }
        IPrizeService PrizeService { get; set; }
        IAddressService AddressService { get; set; }
        IFreeCardService FreeCardService { get; set; }

        IQueryable<UserAccountDto> GetAccountList(string userName);

        UserAccountDto GetUserAccountDto(int accountId);

        List<WinnerListDto> GetWinnerPrizeList();

        DrawDetailDto GetDrawDetail(int drawId);
    }
}
