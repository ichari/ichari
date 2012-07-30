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

	public class PrizeService : BaseService<Prize>, Ichari.IService.IPrizeService
	{
	    private IPrizeRepository _prizeRepository;
	    
	    public PrizeService()
	    {
	        this._prizeRepository = new PrizeRepository();
	        base._repository = this._prizeRepository;
	    }
	    
	    public PrizeService(System.Data.Objects.ObjectContext context)
	    {
	        this._prizeRepository = new PrizeRepository(context);
	        base._repository = this._prizeRepository;
	    }

        public List<Prize> SelectPrizeList(bool selectAllPrize)
        {
            if (selectAllPrize)
                return this.GetQueryList(o => o.IsEnabled == true).OrderBy(o => o.Angle).ToList();
            else
                return this.GetQueryList(o => o.PrizeCount > o.UsedCount && o.IsEnabled == true).OrderBy(o => o.Angle).ToList();
        }

        public List<Prize> SelectRandomPrizeList(int prz_num, bool padHighChance)
        {
            IQueryable<Prize> qList = this.GetQueryList(o => o.PrizeCount > o.UsedCount && o.IsEnabled == true);
            
            if (qList.Count() == prz_num || qList.Count() == 0)
                return qList.ToList();
            else if (qList.Count() > prz_num)
            {
                List<Prize> rList = new List<Prize>();
                LinkedList<Prize> pList = new LinkedList<Prize>(qList);
                Random rnd = new Random();
                for (int i = 0; i < prz_num; i++)
                {
                    int x = rnd.Next(0, pList.Count);
                    rList.Add(pList.ElementAt(x));
                    pList.Remove(pList.ElementAt(x));
                }
                return rList;
            }
            else
            {
                List<Prize> rList;
                if (padHighChance)
                    rList = qList.OrderByDescending(o => o.Probability).ToList();
                else
                    rList = qList.ToList();
                int i = 0;
                while (rList.Count < prz_num)
                {
                    rList.Add(rList[i]);
                    i++;
                    if (i >= qList.Count())
                        i = 0;
                }
                return rList;
            }
        }
    
        public Prize SelectWinningPrize(List<Prize> pList)
        {
            int total = 0;
            if (pList.Count == 0)
                return null;
            List<int> przCnt = new List<int>();
            List<int> prbLst = new List<int>();
            for (int i = 0; i < pList.Count; i++)
            {
                if (pList[i].PrizeCount > pList[i].UsedCount)
                {
                    total += pList[i].Probability;
                    przCnt.Add(i);
                    prbLst.Add(total);
                }
            }
            if (przCnt.Count < 1)
                return null;
            Random rnd = new Random();
            int winNum = rnd.Next(0, total);
            for (int x = 0; x < przCnt.Count; x++)
            {
                if (winNum < prbLst[x])
                    return pList[przCnt[x]];
            }
            return null;
        }
	}
}
