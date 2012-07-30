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

	public class FreeCardService : BaseService<FreeCard>, Ichari.IService.IFreeCardService
	{
	    private IFreeCardRepository _freecardRepository;

	    
	    public FreeCardService()
	    {
	        this._freecardRepository = new FreeCardRepository();
	        base._repository = this._freecardRepository;
	    }
	    
	    public FreeCardService(System.Data.Objects.ObjectContext context)
	    {
	        this._freecardRepository = new FreeCardRepository(context);
	        base._repository = this._freecardRepository;
	    }
        /// <summary>
        /// 领用电子卡卷前先占住一张卡卷
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public FreeCard Delivery(Model.Enum.FreeCardType type)
        {
            var one = _freecardRepository.GetQueryList(
                    t => t.CardType == (int)type
                        && t.IsEnabled == true
                        && t.IsCatch == false
                        && t.IsCost == false).FirstOrDefault();
                if (one == null)
                {
                    throw new Exception("电子卡卷为空");
                }
                one.IsCatch = true;
                _freecardRepository.Save();
                return one;
        }
	}
}
