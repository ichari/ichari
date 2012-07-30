using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Ichari.Model;
using Ichari.Model.Admin;

namespace Ichari.IService  
{

	public interface IFreeCardService : IService<FreeCard>
	{
        /// <summary>
        /// 领用电子卡卷前先占住一张卡卷
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        FreeCard Delivery(Model.Enum.FreeCardType type);
	}
}
