using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;

namespace SLS.SilverLight.FilterShrink.Model
{
    public class LotteryNum
    {
        public int Id { get; set; }
        public string LotteryNums { get; set; }


        public LotteryNum()
        {
            Id = -1;
            LotteryNums = "";
        }

        public LotteryNum(int Id, string LotteryNum)
        {
            this.Id = Id;
            this.LotteryNums = LotteryNum;
        }

        public static List<LotteryNum> GetLotteryNumList()
        {
            return new List<LotteryNum> {new LotteryNum(1,"330"),
                                        new LotteryNum(2,"310"),
                                        new LotteryNum(3,"331"),
                                        new LotteryNum(4,"110"),
                                        new LotteryNum(5,"101"),
                                        new LotteryNum(6,"113"),
                                        new LotteryNum(7,"133")
            };
        }
    }
}
