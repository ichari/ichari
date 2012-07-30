using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Ichari.Model;
using Ichari.Model.Admin;

namespace Ichari.IService  
{

	public interface IPrizeService : IService<Prize>
	{
        List<Prize> SelectPrizeList(bool selectAllPrize);

        List<Prize> SelectRandomPrizeList(int prz_num, bool padHighChance);

        Prize SelectWinningPrize(List<Prize> pList);
	}
}
