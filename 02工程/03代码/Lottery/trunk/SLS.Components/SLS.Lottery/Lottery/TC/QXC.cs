using System;
using System.Collections;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Data;

namespace SLS
{
    public partial class Lottery
    {
        /// <summary>
        /// 七星彩
        /// </summary>
        public partial class QXC : LotteryBase
        {
            #region 静态变量
            public const int PlayType_D = 301;
            public const int PlayType_F = 302;

            public const int ID = 3;
            public const string sID = "3";
            public const string Name = "七星彩";
            public const string Code = "QXC";
            public const double MaxMoney = 20000;
            #endregion

            public QXC()
            {
                id = 3;
                name = "七星彩";
                code = "QXC";
            }

            public override bool CheckPlayType(int play_type)
            {
                return ((play_type >= 301) && (play_type <= 302));
            }

            public override PlayType[] GetPlayTypeList()
            {
                PlayType[] Result = new PlayType[2];

                Result[0] = new PlayType(PlayType_D, "单式");
                Result[1] = new PlayType(PlayType_F, "复式");

                return Result;
            }

            public override string BuildNumber(int Num)	//id = 3
            {
                System.Random rd = new Random();
                StringBuilder sb = new StringBuilder();

                for (int i = 0; i < Num; i++)
                {
                    string LotteryNumber = "";
                    for (int j = 0; j < 7; j++)
                        LotteryNumber += rd.Next(0, 9 + 1).ToString();

                    sb.Append(LotteryNumber.Trim() + "\n");
                }

                string Result = sb.ToString();
                Result = Result.Substring(0, Result.Length - 1);
                return Result;
            }

            public override string[] ToSingle(string Number, ref string CanonicalNumber, int PlayType)	//复式取单式, 后面 ref 参数是将彩票规范化，如：123(445)(11)... 变成123(45)1...
            {
                string[] Locate = new string[7];
                CanonicalNumber = "";

                Regex regex = new Regex(@"^(?<L0>(\d)|([(][\d]+?[)]))(?<L1>(\d)|([(][\d]+?[)]))(?<L2>(\d)|([(][\d]+?[)]))(?<L3>(\d)|([(][\d]+?[)]))(?<L4>(\d)|([(][\d]+?[)]))(?<L5>(\d)|([(][\d]+?[)]))(?<L6>(\d)|([(][\d]+?[)]))", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                Match m = regex.Match(Number);
                for (int i = 0; i < 7; i++)
                {
                    Locate[i] = m.Groups["L" + i.ToString()].ToString().Trim();
                    if (Locate[i] == "")
                    {
                        CanonicalNumber = "";
                        return null;
                    }
                    if (Locate[i].Length > 1)
                    {
                        Locate[i] = Locate[i].Substring(1, Locate[i].Length - 2);
                        if (Locate[i].Length > 1)
                            Locate[i] = FilterRepeated(Locate[i]);
                        if (Locate[i] == "")
                        {
                            CanonicalNumber = "";
                            return null;
                        }
                    }
                    if (Locate[i].Length > 1)
                        CanonicalNumber += "(" + Locate[i] + ")";
                    else
                        CanonicalNumber += Locate[i];
                }

                ArrayList al = new ArrayList();

                #region 循环取单式
                for (int i_0 = 0; i_0 < Locate[0].Length; i_0++)
                {
                    string str_0 = Locate[0][i_0].ToString();
                    for (int i_1 = 0; i_1 < Locate[1].Length; i_1++)
                    {
                        string str_1 = str_0 + Locate[1][i_1].ToString();
                        for (int i_2 = 0; i_2 < Locate[2].Length; i_2++)
                        {
                            string str_2 = str_1 + Locate[2][i_2].ToString();
                            for (int i_3 = 0; i_3 < Locate[3].Length; i_3++)
                            {
                                string str_3 = str_2 + Locate[3][i_3].ToString();
                                for (int i_4 = 0; i_4 < Locate[4].Length; i_4++)
                                {
                                    string str_4 = str_3 + Locate[4][i_4].ToString();
                                    for (int i_5 = 0; i_5 < Locate[5].Length; i_5++)
                                    {
                                        string str_5 = str_4 + Locate[5][i_5].ToString();
                                        for (int i_6 = 0; i_6 < Locate[6].Length; i_6++)
                                        {
                                            string str_6 = str_5 + Locate[6][i_6].ToString();
                                            al.Add(str_6);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                #endregion

                string[] Result = new string[al.Count];
                for (int i = 0; i < al.Count; i++)
                    Result[i] = al[i].ToString();
                return Result;
            }

            public override double ComputeWin(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, int PlayType, params double[] WinMoneyList)
            {
                WinNumber = WinNumber.Trim();
                if (WinNumber.Length < 7)
                    return -1;
                string[] Lotterys = SplitLotteryNumber(Number);
                if (Lotterys == null)
                    return -2;
                if (Lotterys.Length < 1)
                    return -2;
                if ((WinMoneyList == null) || (WinMoneyList.Length < 12))
                    return -3;

                int Description0 = 0, Description1 = 0, Description2 = 0, Description3 = 0, Description4 = 0, Description5 = 0;
                double WinMoney = 0;
                WinMoneyNoWithTax = 0;
                Description = "";

                string[] Locate = new string[7];
                bool[] IsRight = new bool[7];
                int[] Num = new int[7];

                Regex regex = new Regex(@"^(?<L0>([\d])|([(][\d]+?[)]))(?<L1>([\d])|([(][\d]+?[)]))(?<L2>([\d])|([(][\d]+?[)]))(?<L3>([\d])|([(][\d]+?[)]))(?<L4>(\d)|([(][\d]+?[)]))(?<L5>(\d)|([(][\d]+?[)]))(?<L6>(\d)|([(][\d]+?[)]))", RegexOptions.IgnoreCase | RegexOptions.Compiled);

                for (int ii = 0; ii < Lotterys.Length; ii++)
                {
                    Match m = regex.Match(Lotterys[ii]);

                    for (int j = 0; j < 7; j++)
                    {
                        IsRight[j] = false;
                        Num[j] = 0;

                        Locate[j] = m.Groups["L" + j.ToString()].ToString().Trim();

                        if (Locate[j] == "")
                        {
                            continue;
                        }

                        if (Locate[j].Length > 1)
                        {
                            Locate[j] = Locate[j].Substring(1, Locate[j].Length - 2);

                            if (Locate[j].Length > 1)
                            {
                                Locate[j] = FilterRepeated(Locate[j]);
                            }

                            if (Locate[j] == "")
                            {
                                continue;
                            }
                        }

                        Num[j] = Locate[j].Length;

                        IsRight[j] = Locate[j].IndexOf(WinNumber.Substring(j, 1)) >= 0;
                    }

                    #region 一等奖
                    if (IsRight[6] && IsRight[5] && IsRight[4] && IsRight[3] && IsRight[2] && IsRight[1] && IsRight[0])
                    {
                        Description0++;
                        WinMoney += WinMoneyList[0];
                        WinMoneyNoWithTax += WinMoneyList[1];

                        continue;
                    }
                    #endregion

                    #region 二等奖
                    if (IsRight[5] && IsRight[4] && IsRight[3] && IsRight[2] && IsRight[1] && IsRight[0])
                    {
                        int Count = Num[6];

                        if ((IsRight[6]) && Count > 1)
                        {
                            Description1 = Description1 + (Count - 1);
                            WinMoney += WinMoneyList[2] * (Count - 1);
                            WinMoneyNoWithTax += WinMoneyList[3] * (Count - 1);
                        }
                        else if (!IsRight[6])
                        {
                            Description1 = Description1 + Count;
                            WinMoney += WinMoneyList[2] * Count;
                            WinMoneyNoWithTax += WinMoneyList[3] * Count;
                        }

                        continue;
                    }
                    else if (IsRight[6] && IsRight[5] && IsRight[4] && IsRight[3] && IsRight[2] && IsRight[1])
                    {
                        int Count = Num[0];

                        if ((IsRight[0]) && Count > 1)
                        {
                            Description1 = Description1 + (Count - 1);
                            WinMoney += WinMoneyList[2] * (Count - 1);
                            WinMoneyNoWithTax += WinMoneyList[3] * (Count - 1);
                        }
                        else if (!IsRight[0])
                        {
                            Description1 = Description1 + Count;
                            WinMoney += WinMoneyList[2] * Count;
                            WinMoneyNoWithTax += WinMoneyList[3] * Count;
                        }

                        continue;
                    }

                    #endregion

                    #region 三等奖

                    if (IsRight[6] && IsRight[5] && IsRight[4] && IsRight[3] && IsRight[2])
                    {
                        int Count = Num[0];

                        if ((IsRight[1]) && Num[1] > 1)
                        {
                            Count = Count * (Num[1] - 1);
                        }
                        else if (!IsRight[1])
                        {
                            Count = Count * Num[1];
                        }
                        else if (IsRight[1])
                        {
                            Count = 0;
                        }

                        if (Count > 0)
                        {
                            Description2 = Description2 +Count ;
                            WinMoney += WinMoneyList[4] * Count;
                            WinMoneyNoWithTax += WinMoneyList[5] * Count;

                            continue;
                        }
                    }
                    else if (IsRight[5] && IsRight[4] && IsRight[3] && IsRight[2] && IsRight[1])
                    {
                        int Count = 0;

                        if ((IsRight[0]) && Num[0] > 1)
                        {
                            Count = Num[0] - 1;
                        }
                        else if (!IsRight[0])
                        {
                            Count = Num[0];
                        }
                        else
                        {
                            Count = 0;
                        }

                        if ((IsRight[6]) && Num[6] > 1)
                        {
                            Count = Count * (Num[6] - 1);
                        }
                        else if (!IsRight[6])
                        {
                            Count = Count * Num[6];
                        }
                        else
                        {
                            Count = 0;
                        }

                        if (Count > 0)
                        {
                            Description2 = Description2 + Count;
                            WinMoney += WinMoneyList[4] * Count;
                            WinMoneyNoWithTax += WinMoneyList[5] * Count;

                            continue;
                        }
                    }
                    else if (IsRight[4] && IsRight[3] && IsRight[2] && IsRight[1] && IsRight[0])
                    {
                        int Count = Num[6];

                        if ((IsRight[5]) && Num[5] > 1)
                        {
                            Count = Count * (Num[5] - 1);
                        }
                        else if (!IsRight[5])
                        {
                            Count = Count * Num[5];
                        }
                        else if (IsRight[5])
                        {
                            Count = 0;
                        }

                        if (Count > 0)
                        {
                            Description2 = Description2 + Count;
                            WinMoney += WinMoneyList[4] * Count;
                            WinMoneyNoWithTax += WinMoneyList[5] * Count;

                            continue;
                        }
                    }

                    #endregion

                    #region 四等奖

                    if (IsRight[6] && IsRight[5] && IsRight[4] && IsRight[3])
                    {
                        int Count = Num[0] * Num[1];

                        if ((IsRight[2]) && Num[2] > 1)
                        {
                            Count = Count * (Num[2] - 1);
                        }
                        else if (!IsRight[2])
                        {
                            Count = Count * Num[2];
                        }
                        else if (IsRight[2])
                        {
                            Count = 0;
                        }

                        if (Count > 0)
                        {
                            Description3 = Description3 + Count;
                            WinMoney += WinMoneyList[6] * Count;
                            WinMoneyNoWithTax += WinMoneyList[7] * Count;

                            continue;
                        }

                    }else if (IsRight[5] && IsRight[4] && IsRight[3] && IsRight[2])
                    {
                        int Count = Num[0];

                        if ((IsRight[1]) && Num[1] > 1)
                        {
                            Count = Count * (Num[1] - 1);
                        }
                        else if (!IsRight[1])
                        {
                            Count = Count * Num[1];
                        }
                        else if (IsRight[1])
                        {
                            Count = 0;
                        }

                        if ((IsRight[6]) && Num[6] > 1)
                        {
                            Count = Count * (Num[6] - 1);
                        }
                        else if (!IsRight[6])
                        {
                            Count = Count * Num[6];
                        }
                        else if (IsRight[6])
                        {
                            Count = 0;
                        }

                        Description3 = Description3 + Count;
                        WinMoney += WinMoneyList[6] * Count;
                        WinMoneyNoWithTax += WinMoneyList[7] * Count;

                        continue;

                    }else if (IsRight[4] && IsRight[3] && IsRight[2] && IsRight[1])
                    {
                        int Count = Num[6];

                        if ((IsRight[0]) && Num[0] > 1)
                        {
                            Count = Count * (Num[0] - 1);
                        }
                        else if (!IsRight[0])
                        {
                            Count = Count * Num[0];
                        }
                        else if (IsRight[0])
                        {
                            Count = 0;
                        }

                        if ((IsRight[5]) && Num[5] > 1)
                        {
                            Count = Count * (Num[5] - 1);
                        }
                        else if (!IsRight[5])
                        {
                            Count = Count * Num[5];
                        }
                        else if (IsRight[5])
                        {
                            Count = 0;
                        }

                        if (Count > 0)
                        {
                            Description3 = Description3 + Count;
                            WinMoney += WinMoneyList[6] * Count;
                            WinMoneyNoWithTax += WinMoneyList[7] * Count;

                            continue;
                        }
                    }
                    else if (IsRight[3] && IsRight[2] && IsRight[1] && IsRight[0])
                    {
                        int Count = Num[6] * Num[5];

                        if ((IsRight[4]) && Num[4] > 1)
                        {
                            Count = Count * (Num[4] - 1);
                        }
                        else if (!IsRight[4])
                        {
                            Count = Count * Num[4];
                        }
                        else if (IsRight[4])
                        {
                            Count = 0;
                        }

                        if (Count > 0)
                        {
                            Description3 = Description3 + Count;
                            WinMoney += WinMoneyList[6] * Count;
                            WinMoneyNoWithTax += WinMoneyList[7] * Count;

                            continue;
                        }
                    }

                    #endregion

                    #region 五等奖

                    if (IsRight[6] && IsRight[5] && IsRight[4])
                    {
                        int Count = Num[0] * Num[1] * Num[2];

                        if ((IsRight[3]) && Num[3] > 1)
                        {
                            Count = Count * (Num[3] - 1);
                        }
                        else if (!IsRight[3])
                        {
                            Count = Count * Num[3];
                        }
                        else if (IsRight[3])
                        {
                            Count = 0;
                        }

                        if (Count > 0)
                        {
                            Description4 = Description4 + Count;
                            WinMoney += WinMoneyList[8] * Count;
                            WinMoneyNoWithTax += WinMoneyList[9] * Count;

                            continue;
                        }

                    }else if (IsRight[5] && IsRight[4] && IsRight[3])
                    {
                        int Count = Num[0] * Num[1];

                        if ((IsRight[6]) && Num[6] > 1)
                        {
                            Count = Count * (Num[6] - 1);
                        }
                        else if (!IsRight[6])
                        {
                            Count = Count * Num[6];
                        }
                        else if (IsRight[6])
                        {
                            Count = 0;
                        }

                        if ((IsRight[2]) && Num[2] > 1)
                        {
                            Count = Count * (Num[2] - 1);
                        }
                        else if (!IsRight[2])
                        {
                            Count = Count * Num[2];
                        }
                        else if (IsRight[2])
                        {
                            Count = 0;
                        }

                        if (Count > 0)
                        {
                            Description4 = Description4 + Count;
                            WinMoney += WinMoneyList[8] * Count;
                            WinMoneyNoWithTax += WinMoneyList[9] * Count;

                            continue;
                        }

                    }else if (IsRight[4] && IsRight[3] && IsRight[2])
                    {
                        int Count = Num[0] * Num[6];

                        if ((IsRight[5]) && Num[5] > 1)
                        {
                            Count = Count * (Num[5] - 1);
                        }
                        else if (!IsRight[5])
                        {
                            Count = Count * Num[5];
                        }
                        else if (IsRight[5])
                        {
                            Count = 0;
                        }

                        if ((IsRight[1]) && Num[1] > 1)
                        {
                            Count = Count * (Num[1] - 1);
                        }
                        else if (!IsRight[1])
                        {
                            Count = Count * Num[1];
                        }
                        else if (IsRight[1])
                        {
                            Count = 0;
                        }

                        if (Count > 0)
                        {
                            Description4 = Description4 + Count;
                            WinMoney += WinMoneyList[8] * Count;
                            WinMoneyNoWithTax += WinMoneyList[9] * Count;

                            continue;
                        }

                    }else if (IsRight[3] && IsRight[2] && IsRight[1])
                    {
                        int Count = Num[5] * Num[6];

                        if ((IsRight[4]) && Num[4] > 1)
                        {
                            Count = Count * (Num[4] - 1);
                        }
                        else if (!IsRight[4])
                        {
                            Count = Count * Num[4];
                        }
                        else if (IsRight[4])
                        {
                            Count = 0;
                        }

                        if ((IsRight[0]) && Num[0] > 1)
                        {
                            Count = Count * (Num[0] - 1);
                        }
                        else if (!IsRight[0])
                        {
                            Count = Count * Num[0];
                        }
                        else if (IsRight[0])
                        {
                            Count = 0;
                        }

                        if (Count > 0)
                        {
                            Description4 = Description4 + Count;
                            WinMoney += WinMoneyList[8] * Count;
                            WinMoneyNoWithTax += WinMoneyList[9] * Count;

                            continue;
                        }
                    }
                    else if (IsRight[2] && IsRight[1] && IsRight[0])
                    {
                        int Count = Num[6] * Num[5] * Num[4];

                        if ((IsRight[3]) && Num[3] > 1)
                        {
                            Count = Count * (Num[3] - 1);
                        }
                        else if (!IsRight[3])
                        {
                            Count = Count * Num[3];
                        }
                        else if (IsRight[3])
                        {
                            Count = 0;
                        }

                        if (Count > 0)
                        {
                            Description4 = Description4 + Count;
                            WinMoney += WinMoneyList[8] * Count;
                            WinMoneyNoWithTax += WinMoneyList[9] * Count;

                            continue;
                        }
                    }

                    #endregion

                    #region 六等奖
                    if (IsRight[6] && IsRight[5])
                    {
                        int Count = Num[3] * Num[2] * Num[1] * Num[0];

                        if ((IsRight[4]) && Num[4] > 1)
                        {
                            Count = Count * (Num[4] - 1);
                        }
                        else if (!IsRight[4])
                        {
                            Count = Count * Num[4];
                        }
                        else if (IsRight[4])
                        {
                            Count = 0;
                        }

                        if (Count > 0)
                        {
                            Description5 = Description5 + Count;
                            WinMoney += WinMoneyList[10] * Count;
                            WinMoneyNoWithTax += WinMoneyList[11] * Count;

                            continue;
                        }
                    }else if (IsRight[5] && IsRight[4])
                    {
                        int Count = Num[2] * Num[1] * Num[0];

                        if ((IsRight[3]) && Num[3] > 1)
                        {
                            Count = Count * (Num[3] - 1);
                        }
                        else if (!IsRight[3])
                        {
                            Count = Count * Num[3];
                        }
                        else if (IsRight[3])
                        {
                            Count = 0;
                        }

                        if ((IsRight[6]) && Num[6] > 1)
                        {
                            Count = Count * (Num[6] - 1);
                        }
                        else if (!IsRight[6])
                        {
                            Count = Count * Num[6];
                        }
                        else if (IsRight[6])
                        {
                            Count = 0;
                        }

                        if (Count > 0)
                        {
                            Description5 = Description5 + Count;
                            WinMoney += WinMoneyList[10] * Count;
                            WinMoneyNoWithTax += WinMoneyList[11] * Count;

                            continue;
                        }
                    }else if (IsRight[4] && IsRight[3])
                    {
                        int Count = Num[6] * Num[1] * Num[0];

                        if ((IsRight[5]) && Num[5] > 1)
                        {
                            Count = Count * (Num[5] - 1);
                        }
                        else if (!IsRight[5])
                        {
                            Count = Count * Num[5];
                        }
                        else if (IsRight[5])
                        {
                            Count = 0;
                        }

                        if ((IsRight[2]) && Num[2] > 1)
                        {
                            Count = Count * (Num[2] - 1);
                        }
                        else if (!IsRight[2])
                        {
                            Count = Count * Num[2];
                        }
                        else if (IsRight[2])
                        {
                            Count = 0;
                        }

                        if (Count > 0)
                        {
                            Description5 = Description5 + Count;
                            WinMoney += WinMoneyList[10] * Count;
                            WinMoneyNoWithTax += WinMoneyList[11] * Count;

                            continue;
                        }
                    }else if (IsRight[3] && IsRight[2])
                    {
                        int Count = Num[6] * Num[5] * Num[0];

                        if ((IsRight[4]) && Num[4] > 1)
                        {
                            Count = Count * (Num[4] - 1);
                        }
                        else if (!IsRight[4])
                        {
                            Count = Count * Num[4];
                        }
                        else if (IsRight[4])
                        {
                            Count = 0;
                        }

                        if ((IsRight[1]) && Num[1] > 1)
                        {
                            Count = Count * (Num[1] - 1);
                        }
                        else if (!IsRight[1])
                        {
                            Count = Count * Num[1];
                        }
                        else if (IsRight[1])
                        {
                            Count = 0;
                        }

                        if (Count > 0)
                        {
                            Description5 = Description5 + Count;
                            WinMoney += WinMoneyList[10] * Count;
                            WinMoneyNoWithTax += WinMoneyList[11] * Count;

                            continue;
                        }
                    }else if (IsRight[2] && IsRight[1])
                    {
                        int Count = Num[6] * Num[5] * Num[4];

                        if ((IsRight[3]) && Num[3] > 1)
                        {
                            Count = Count * (Num[3] - 1);
                        }
                        else if (!IsRight[3])
                        {
                            Count = Count * Num[3];
                        }
                        else if (IsRight[3])
                        {
                            Count = 0;
                        }

                        if ((IsRight[0]) && Num[0] > 1)
                        {
                            Count = Count * (Num[0] - 1);
                        }
                        else if (!IsRight[0])
                        {
                            Count = Count * Num[0];
                        }
                        else if (IsRight[0])
                        {
                            Count = 0;
                        }

                        if (Count > 0)
                        {
                            Description5 = Description5 + Count;
                            WinMoney += WinMoneyList[10] * Count;
                            WinMoneyNoWithTax += WinMoneyList[11] * Count;

                            continue;
                        }
                    }
                    else if (IsRight[1] && IsRight[0])
                    {
                        int Count = Num[6] * Num[5] * Num[4] * Num[3];

                        if ((IsRight[2]) && Num[2] > 1)
                        {
                            Count = Count * (Num[2] - 1);
                        }
                        else if (!IsRight[2])
                        {
                            Count = Count * Num[2];
                        }
                        else if (IsRight[2])
                        {
                            Count = 0;
                        }

                        if (Count > 0)
                        {
                            Description5 = Description5 + Count;
                            WinMoney += WinMoneyList[10] * Count;
                            WinMoneyNoWithTax += WinMoneyList[11] * Count;

                            continue;
                        }
                    }
                    #endregion
                }

                if (Description0 > 0)
                    Description = "一等奖" + Description0.ToString() + "注";
                if (Description1 > 0)
                {
                    if (Description != "")
                        Description += "，";
                    Description += "二等奖" + Description1.ToString() + "注";
                }
                if (Description2 > 0)
                {
                    if (Description != "")
                        Description += "，";
                    Description += "三等奖" + Description2.ToString() + "注";
                }
                if (Description3 > 0)
                {
                    if (Description != "")
                        Description += "，";
                    Description += "四等奖" + Description3.ToString() + "注";
                }
                if (Description4 > 0)
                {
                    if (Description != "")
                        Description += "，";
                    Description += "五等奖" + Description4.ToString() + "注";
                }
                if (Description5 > 0)
                {
                    if (Description != "")
                        Description += "，";
                    Description += "六等奖" + Description5.ToString() + "注";
                }
                if (Description != "")
                    Description += "。";

                return WinMoney;
            }

            public override string AnalyseScheme(string Content, int PlayType)
            {
                string[] strs = Content.Split('\n');
                if (strs == null)
                    return "";
                if (strs.Length == 0)
                    return "";

                string Result = "";
                int Num = 1;
                string CanonicalNumber = "";
                string RegexString = "";

                string[] Locate = new string[7];

                if (PlayType == PlayType_D)
                {
                    RegexString = @"^([\d]){7}";
                }
                else
                {
                    RegexString = @"^(?<L0>([\d])|([(][\d]+?[)]))(?<L1>([\d])|([(][\d]+?[)]))(?<L2>([\d])|([(][\d]+?[)]))(?<L3>([\d])|([(][\d]+?[)]))(?<L4>(\d)|([(][\d]+?[)]))(?<L5>(\d)|([(][\d]+?[)]))(?<L6>(\d)|([(][\d]+?[)]))";
                }

                Regex regex = new Regex(RegexString, RegexOptions.IgnoreCase | RegexOptions.Compiled);

                for (int i = 0; i < strs.Length; i++)
                {
                    Match m = regex.Match(strs[i]);

                    if (!m.Success)
                    {
                        continue;
                    }

                    if (PlayType == PlayType_D)
                    {
                        string[] singles = ToSingle(m.Value, ref CanonicalNumber, PlayType);
                        if (singles == null)
                            continue;
                        Result += CanonicalNumber + "|" + singles.Length.ToString() + "\n";

                        continue;
                    }

                    Num = 1;
                    CanonicalNumber = "";

                    for (int j = 0; j < 7; j++)
                    {
                        Locate[j] = m.Groups["L" + j.ToString()].ToString().Trim();

                        if (Locate[j] == "")
                        {
                            CanonicalNumber = "";
                            continue;
                        }

                        if (Locate[j].Length > 1)
                        {
                            Locate[j] = Locate[j].Substring(1, Locate[j].Length - 2);

                            if (Locate[j].Length > 1)
                            {
                                Locate[j] = FilterRepeated(Locate[j]);
                            }

                            if (Locate[j] == "")
                            {
                                CanonicalNumber = "";
                                continue;
                            }
                        }

                        if (Locate[j].Length > 1)
                        {
                            CanonicalNumber += "(" + Locate[j] + ")";

                            Num = Num * Locate[j].Length;
                        }
                        else
                        {
                            CanonicalNumber += Locate[j];
                        }
                    }

                    Result += CanonicalNumber + "|" + Num.ToString() + "\n";
                }

                if (Result.EndsWith("\n"))
                    Result = Result.Substring(0, Result.Length - 1);
                return Result;
            }

            public override bool AnalyseWinNumber(string Number)
            {
                string t_str = "";
                string[] WinLotteryNumber = ToSingle(Number, ref t_str, PlayType_D);

                if ((WinLotteryNumber == null) || (WinLotteryNumber.Length != 1))
                    return false;

                return true;
            }

            public override int GetNum(int Number1, int Number2)
            {
                if (Number2 < Number1)
                {
                    return -1;
                }

                if (Number2 == Number1)
                {
                    return 1;
                }

                int i = 1;
                int j = 1;

                while (Number1 > 0)
                {
                    i = i * (Number2 + 1 - Number1);
                    j = j * (Number1);

                    Number1--;
                }

                return i / j;
            }

            private string FilterRepeated(string NumberPart)
            {
                string Result = "";
                for (int i = 0; i < NumberPart.Length; i++)
                {
                    if ((Result.IndexOf(NumberPart.Substring(i, 1)) == -1) && ("0123456789".IndexOf(NumberPart.Substring(i, 1)) >= 0))
                        Result += NumberPart.Substring(i, 1);
                }
                return Sort(Result);
            }

            public override string ShowNumber(string Number)
            {
                return base.ShowNumber(Number, " ");
            }

            public override string GetPrintKeyList(string Number, int PlayTypeID, string LotteryMachine)    //根据出票机类型，转换出需要发送到彩票机的 Key 列表
            {
                Number = Number.Trim();
                if (Number == "")
                {
                    return "";
                }

                string[] Numbers = Number.Split('\n');
                if ((Numbers == null) || (Numbers.Length < 1))
                {
                    return "";
                }

                switch (LotteryMachine)
                {
                    case "CR_YTCII2":
                        if (PlayTypeID == PlayType_D)
                        {
                            return GetPrintKeyList_CR_YTCII2_D(Numbers);
                        }
                        if (PlayTypeID == PlayType_F)
                        {
                            return GetPrintKeyList_CR_YTCII2_F(Numbers);
                        }
                        break;
                    case "TCBJYTD":
                        if (PlayTypeID == PlayType_D)
                        {
                            return GetPrintKeyList_TCBJYTD_D(Numbers);
                        }
                        if (PlayTypeID == PlayType_F)
                        {
                            return GetPrintKeyList_TCBJYTD_F(Numbers);
                        }
                        break;
                    case "TGAMPOS4000":
                        if (PlayTypeID == PlayType_D)
                        {
                            return GetPrintKeyList_TGAMPOS4000_D(Numbers);
                        }
                        if (PlayTypeID == PlayType_F)
                        {
                            return GetPrintKeyList_TGAMPOS4000_F(Numbers);
                        }
                        break;
                    case "CP86":
                        if (PlayTypeID == PlayType_D)
                        {
                            return GetPrintKeyList_CP86_D(Numbers);
                        }
                        if (PlayTypeID == PlayType_F)
                        {
                            return GetPrintKeyList_CP86_F(Numbers);
                        }
                        break;
                    case "MODEL_4000":
                        if (PlayTypeID == PlayType_D)
                        {
                            return GetPrintKeyList_MODEL_4000_D(Numbers);
                        }
                        if (PlayTypeID == PlayType_F)
                        {
                            return GetPrintKeyList_MODEL_4000_F(Numbers);
                        }
                        break;
                    case "CORONISTPT":
                        if (PlayTypeID == PlayType_D)
                        {
                            return GetPrintKeyList_CORONISTPT_D(Numbers);
                        }
                        if (PlayTypeID == PlayType_F)
                        {
                            return GetPrintKeyList_CORONISTPT_F(Numbers);
                        }
                        break;
                    case "RS6500":
                        if (PlayTypeID == PlayType_D)
                        {
                            return GetPrintKeyList_RS6500_D(Numbers);
                        }
                        if (PlayTypeID == PlayType_F)
                        {
                            return GetPrintKeyList_RS6500_F(Numbers);
                        }
                        break;
                    case "ks230":
                        if (PlayTypeID == PlayType_D)
                        {
                            return GetPrintKeyList_ks230_D(Numbers);
                        }
                        if (PlayTypeID == PlayType_F)
                        {
                            return GetPrintKeyList_ks230_F(Numbers);
                        }
                        break;
                    case "LA-600A":
                        if (PlayTypeID == PlayType_D)
                        {
                            return GetPrintKeyList_LA_600A_D(Numbers);
                        }
                        if (PlayTypeID == PlayType_F)
                        {
                            return GetPrintKeyList_LA_600A_F(Numbers);
                        }
                        break;
                }

                return "";
            }
            #region GetPrintKeyList 的具体方法
            private string GetPrintKeyList_CR_YTCII2_D(string[] Numbers)
            {
                string KeyList = "";

                foreach (string Number in Numbers)
                {
                    foreach (char ch in Number.Replace(" ", "").Replace("\r", "").Replace("\n", ""))
                    {
                        KeyList += "[" + ch.ToString() + "]";
                    }
                }

                return KeyList;
            }
            private string GetPrintKeyList_CR_YTCII2_F(string[] Numbers)
            {
                string KeyList = "";

                foreach (string Number in Numbers)
                {
                    Regex regex = new Regex(@"^(?<L0>(\d)|([(][\d]+?[)]))(?<L1>(\d)|([(][\d]+?[)]))(?<L2>(\d)|([(][\d]+?[)]))(?<L3>(\d)|([(][\d]+?[)]))(?<L4>(\d)|([(][\d]+?[)]))(?<L5>(\d)|([(][\d]+?[)]))(?<L6>(\d)|([(][\d]+?[)]))", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                    Match m = regex.Match(Number);

                    for (int i = 0; i < 7; i++)
                    {
                        string Locate = m.Groups["L" + i.ToString()].ToString().Trim();

                        if (Locate.Length == 1)
                        {
                            KeyList += "[" + Locate + "]";
                        }
                        else
                        {
                            Locate = Locate.Substring(1, Locate.Length - 2);

                            foreach (char ch in Locate)
                            {
                                KeyList += "[" + ch.ToString() + "]";
                            }
                        }

                        if (i < 6)
                            KeyList += "[↓]";
                    }
                }

                return KeyList;
            }

            private string GetPrintKeyList_TCBJYTD_D(string[] Numbers)
            {
                string KeyList = "";

                foreach (string Number in Numbers)
                {
                    foreach (char ch in Number.Replace(" ", "").Replace("\r", "").Replace("\n", ""))
                    {
                        KeyList += "[" + ch.ToString() + "]";
                    }
                }

                return KeyList;
            }
            private string GetPrintKeyList_TCBJYTD_F(string[] Numbers)
            {
                string KeyList = "";

                foreach (string Number in Numbers)
                {
                    Regex regex = new Regex(@"^(?<L0>(\d)|([(][\d]+?[)]))(?<L1>(\d)|([(][\d]+?[)]))(?<L2>(\d)|([(][\d]+?[)]))(?<L3>(\d)|([(][\d]+?[)]))(?<L4>(\d)|([(][\d]+?[)]))(?<L5>(\d)|([(][\d]+?[)]))(?<L6>(\d)|([(][\d]+?[)]))", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                    Match m = regex.Match(Number);

                    for (int i = 0; i < 7; i++)
                    {
                        string Locate = m.Groups["L" + i.ToString()].ToString().Trim();

                        if (Locate.Length == 1)
                        {
                            KeyList += "[" + Locate + "]";
                        }
                        else
                        {
                            Locate = Locate.Substring(1, Locate.Length - 2);

                            foreach (char ch in Locate)
                            {
                                KeyList += "[" + ch.ToString() + "]";
                            }
                        }

                        if (i < 6)
                            KeyList += "[→]";
                    }
                }

                return KeyList;
            }

            private string GetPrintKeyList_TGAMPOS4000_D(string[] Numbers)
            {
                string KeyList = "";

                foreach (string Number in Numbers)
                {
                    foreach (char ch in Number.Replace(" ", "").Replace("\r", "").Replace("\n", ""))
                    {
                        KeyList += "[" + ch.ToString() + "]";
                    }
                }

                return KeyList;
            }
            private string GetPrintKeyList_TGAMPOS4000_F(string[] Numbers)
            {
                string KeyList = "";

                foreach (string Number in Numbers)
                {
                    Regex regex = new Regex(@"^(?<L0>(\d)|([(][\d]+?[)]))(?<L1>(\d)|([(][\d]+?[)]))(?<L2>(\d)|([(][\d]+?[)]))(?<L3>(\d)|([(][\d]+?[)]))(?<L4>(\d)|([(][\d]+?[)]))(?<L5>(\d)|([(][\d]+?[)]))(?<L6>(\d)|([(][\d]+?[)]))", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                    Match m = regex.Match(Number);

                    for (int i = 0; i < 7; i++)
                    {
                        string Locate = m.Groups["L" + i.ToString()].ToString().Trim();

                        if (Locate.Length == 1)
                        {
                            KeyList += "[" + Locate + "]";
                        }
                        else
                        {
                            Locate = Locate.Substring(1, Locate.Length - 2);

                            foreach (char ch in Locate)
                            {
                                KeyList += "[" + ch.ToString() + "]";
                            }
                        }

                        if (i < 6)
                            KeyList += "[↓]";
                    }
                }

                return KeyList;
            }

            private string GetPrintKeyList_CP86_D(string[] Numbers)
            {
                string KeyList = "";

                foreach (string Number in Numbers)
                {
                    foreach (char ch in Number.Replace(" ", "").Replace("\r", "").Replace("\n", ""))
                    {
                        KeyList += "[" + ch.ToString() + "]";
                    }
                }

                return KeyList;
            }
            private string GetPrintKeyList_CP86_F(string[] Numbers)
            {
                string KeyList = "";

                foreach (string Number in Numbers)
                {
                    Regex regex = new Regex(@"^(?<L0>(\d)|([(][\d]+?[)]))(?<L1>(\d)|([(][\d]+?[)]))(?<L2>(\d)|([(][\d]+?[)]))(?<L3>(\d)|([(][\d]+?[)]))(?<L4>(\d)|([(][\d]+?[)]))(?<L5>(\d)|([(][\d]+?[)]))(?<L6>(\d)|([(][\d]+?[)]))", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                    Match m = regex.Match(Number);

                    for (int i = 0; i < 7; i++)
                    {
                        string Locate = m.Groups["L" + i.ToString()].ToString().Trim();

                        if (Locate.Length == 1)
                        {
                            KeyList += "[" + Locate + "]";
                        }
                        else
                        {
                            Locate = Locate.Substring(1, Locate.Length - 2);

                            foreach (char ch in Locate)
                            {
                                KeyList += "[" + ch.ToString() + "]";
                            }
                        }

                        if (i < 6)
                            KeyList += "[↓]";
                    }
                }

                return KeyList;
            }

            private string GetPrintKeyList_MODEL_4000_D(string[] Numbers)
            {
                string KeyList = "";

                foreach (string Number in Numbers)
                {
                    foreach (char ch in Number.Replace(" ", "").Replace("\r", "").Replace("\n", ""))
                    {
                        KeyList += "[" + ch.ToString() + "]";
                    }
                }

                return KeyList;
            }
            private string GetPrintKeyList_MODEL_4000_F(string[] Numbers)
            {
                string KeyList = "";

                foreach (string Number in Numbers)
                {
                    Regex regex = new Regex(@"^(?<L0>(\d)|([(][\d]+?[)]))(?<L1>(\d)|([(][\d]+?[)]))(?<L2>(\d)|([(][\d]+?[)]))(?<L3>(\d)|([(][\d]+?[)]))(?<L4>(\d)|([(][\d]+?[)]))(?<L5>(\d)|([(][\d]+?[)]))(?<L6>(\d)|([(][\d]+?[)]))", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                    Match m = regex.Match(Number);

                    for (int i = 0; i < 7; i++)
                    {
                        string Locate = m.Groups["L" + i.ToString()].ToString().Trim();

                        if (Locate.Length == 1)
                        {
                            KeyList += "[" + Locate + "]";
                        }
                        else
                        {
                            Locate = Locate.Substring(1, Locate.Length - 2);

                            foreach (char ch in Locate)
                            {
                                KeyList += "[" + ch.ToString() + "]";
                            }
                        }

                        if (i < 6)
                            KeyList += "[↓]";
                    }
                }

                return KeyList;
            }

            private string GetPrintKeyList_CORONISTPT_D(string[] Numbers)
            {
                string KeyList = "";

                foreach (string Number in Numbers)
                {
                    foreach (char ch in Number.Replace(" ", "").Replace("\r", "").Replace("\n", ""))
                    {
                        KeyList += "[" + ch.ToString() + "]";
                    }
                }

                return KeyList;
            }
            private string GetPrintKeyList_CORONISTPT_F(string[] Numbers)
            {
                string KeyList = "";

                foreach (string Number in Numbers)
                {
                    Regex regex = new Regex(@"^(?<L0>(\d)|([(][\d]+?[)]))(?<L1>(\d)|([(][\d]+?[)]))(?<L2>(\d)|([(][\d]+?[)]))(?<L3>(\d)|([(][\d]+?[)]))(?<L4>(\d)|([(][\d]+?[)]))(?<L5>(\d)|([(][\d]+?[)]))(?<L6>(\d)|([(][\d]+?[)]))", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                    Match m = regex.Match(Number);

                    for (int i = 0; i < 7; i++)
                    {
                        string Locate = m.Groups["L" + i.ToString()].ToString().Trim();

                        if (Locate.Length == 1)
                        {
                            KeyList += "[" + Locate + "]";
                        }
                        else
                        {
                            Locate = Locate.Substring(1, Locate.Length - 2);

                            foreach (char ch in Locate)
                            {
                                KeyList += "[" + ch.ToString() + "]";
                            }
                        }

                        if (i < 6)
                            KeyList += "[↓]";
                    }
                }

                return KeyList;
            }

            private string GetPrintKeyList_RS6500_D(string[] Numbers)
            {
                string KeyList = "";

                foreach (string Number in Numbers)
                {
                    foreach (char ch in Number.Replace(" ", "").Replace("\r", "").Replace("\n", ""))
                    {
                        KeyList += "[" + ch.ToString() + "]";
                    }
                }

                return KeyList;
            }
            private string GetPrintKeyList_RS6500_F(string[] Numbers)
            {
                string KeyList = "";

                foreach (string Number in Numbers)
                {
                    Regex regex = new Regex(@"^(?<L0>(\d)|([(][\d]+?[)]))(?<L1>(\d)|([(][\d]+?[)]))(?<L2>(\d)|([(][\d]+?[)]))(?<L3>(\d)|([(][\d]+?[)]))(?<L4>(\d)|([(][\d]+?[)]))(?<L5>(\d)|([(][\d]+?[)]))(?<L6>(\d)|([(][\d]+?[)]))", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                    Match m = regex.Match(Number);

                    for (int i = 0; i < 7; i++)
                    {
                        string Locate = m.Groups["L" + i.ToString()].ToString().Trim();

                        if (Locate.Length == 1)
                        {
                            KeyList += "[" + Locate + "]";
                        }
                        else
                        {
                            Locate = Locate.Substring(1, Locate.Length - 2);

                            foreach (char ch in Locate)
                            {
                                KeyList += "[" + ch.ToString() + "]";
                            }
                        }

                        if (i < 6)
                            KeyList += "[↓]";
                    }
                }

                return KeyList;
            }

            private string GetPrintKeyList_ks230_D(string[] Numbers)
            {
                string KeyList = "";

                foreach (string Number in Numbers)
                {
                    foreach (char ch in Number.Replace(" ", "").Replace("\r", "").Replace("\n", ""))
                    {
                        KeyList += "[" + ch.ToString() + "]";
                    }
                }

                return KeyList;
            }
            private string GetPrintKeyList_ks230_F(string[] Numbers)
            {
                string KeyList = "";

                foreach (string Number in Numbers)
                {
                    Regex regex = new Regex(@"^(?<L0>(\d)|([(][\d]+?[)]))(?<L1>(\d)|([(][\d]+?[)]))(?<L2>(\d)|([(][\d]+?[)]))(?<L3>(\d)|([(][\d]+?[)]))(?<L4>(\d)|([(][\d]+?[)]))(?<L5>(\d)|([(][\d]+?[)]))(?<L6>(\d)|([(][\d]+?[)]))", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                    Match m = regex.Match(Number);

                    for (int i = 0; i < 7; i++)
                    {
                        string Locate = m.Groups["L" + i.ToString()].ToString().Trim();

                        if (Locate.Length == 1)
                        {
                            KeyList += "[" + Locate + "]";
                        }
                        else
                        {
                            Locate = Locate.Substring(1, Locate.Length - 2);

                            foreach (char ch in Locate)
                            {
                                KeyList += "[" + ch.ToString() + "]";
                            }
                        }

                        if (i < 6)
                            KeyList += "[↓]";
                    }
                }

                return KeyList;
            }

            private string GetPrintKeyList_LA_600A_D(string[] Numbers)
            {
                string KeyList = "";

                foreach (string Number in Numbers)
                {
                    foreach (char ch in Number.Replace(" ", "").Replace("\r", "").Replace("\n", ""))
                    {
                        KeyList += "[" + ch.ToString() + "]";
                    }
                }

                return KeyList;
            }
            private string GetPrintKeyList_LA_600A_F(string[] Numbers)
            {
                string KeyList = "";

                foreach (string Number in Numbers)
                {
                    Regex regex = new Regex(@"^(?<L0>(\d)|([(][\d]+?[)]))(?<L1>(\d)|([(][\d]+?[)]))(?<L2>(\d)|([(][\d]+?[)]))(?<L3>(\d)|([(][\d]+?[)]))(?<L4>(\d)|([(][\d]+?[)]))(?<L5>(\d)|([(][\d]+?[)]))(?<L6>(\d)|([(][\d]+?[)]))", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                    Match m = regex.Match(Number);

                    for (int i = 0; i < 7; i++)
                    {
                        string Locate = m.Groups["L" + i.ToString()].ToString().Trim();

                        if (Locate.Length == 1)
                        {
                            KeyList += "[" + Locate + "]";
                        }
                        else
                        {
                            Locate = Locate.Substring(1, Locate.Length - 2);

                            foreach (char ch in Locate)
                            {
                                KeyList += "[" + ch.ToString() + "]";
                            }
                        }

                        if (i < 6)
                            KeyList += "[↓]";
                    }
                }

                return KeyList;
            }
            #endregion

            //参数说明 PlayTypeID:玩法类别; Number:投注号码; Multiple: 倍数; MaxMultiple:最大倍数;
            public override Ticket[] ToElectronicTicket_HPSD(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                if (PlayTypeID == PlayType_D)
                {
                    return ToElectronicTicket_HPSD_D(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                if (PlayTypeID == PlayType_F)
                {
                    return ToElectronicTicket_HPSD_F(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                return null;
            }
            #region ToElectronicTicket_HPSD 的具体方法
            private Ticket[] ToElectronicTicket_HPSD_D(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                string[] strs = AnalyseScheme(Number, PlayTypeID).Split('\n');

                if (strs == null)
                {
                    return null;
                }
                if (strs.Length == 0)
                {
                    return null;
                }

                Money = 0;

                int MultipleNum = 0;

                if ((Multiple % MaxMultiple) != 0)
                {
                    MultipleNum = (Multiple - (Multiple % MaxMultiple)) / MaxMultiple + 1;
                }
                else
                {
                    MultipleNum = Multiple / MaxMultiple;
                }

                ArrayList al = new ArrayList();

                int EachMultiple = 1;
                double EachMoney = 0;

                for (int n = 1; n < MultipleNum + 1; n++)
                {
                    if ((n * MaxMultiple) < Multiple)
                    {
                        EachMultiple = MaxMultiple;
                    }
                    else
                    {
                        EachMultiple = Multiple - (n - 1) * MaxMultiple;
                    }

                    for (int i = 0; i < strs.Length; i += 5)
                    {
                        string Numbers = "";
                        EachMoney = 0;

                        for (int M = 0; M < 5; M++)
                        {
                            if ((i + M) < strs.Length)
                            {
                                Numbers += strs[i + M].ToString().Split('|')[0] + "\n";
                                EachMoney += 2 * double.Parse(strs[i + M].ToString().Split('|')[1]);
                            }
                        }

                        Money += EachMoney * EachMultiple;

                        al.Add(new Ticket(101, ConvertFormatToElectronTicket_HPSD(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));
                    }
                }

                Ticket[] Tickets = new Ticket[al.Count];

                for (int i = 0; i < Tickets.Length; i++)
                {
                    Tickets[i] = (Ticket)al[i];
                }

                return Tickets;

            }
            private Ticket[] ToElectronicTicket_HPSD_F(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                string[] strs = AnalyseScheme(Number, PlayTypeID).Split('\n');

                if (strs == null)
                {
                    return null;
                }
                if (strs.Length == 0)
                {
                    return null;
                }

                Money = 0;

                int MultipleNum = 0;

                if ((Multiple % MaxMultiple) != 0)
                {
                    MultipleNum = (Multiple - (Multiple % MaxMultiple)) / MaxMultiple + 1;
                }
                else
                {
                    MultipleNum = Multiple / MaxMultiple;
                }

                ArrayList al = new ArrayList();

                int EachMultiple = 1;
                double EachMoney = 0;

                for (int n = 1; n < MultipleNum + 1; n++)
                {
                    if ((n * MaxMultiple) < Multiple)
                    {
                        EachMultiple = MaxMultiple;
                    }
                    else
                    {
                        EachMultiple = Multiple - (n - 1) * MaxMultiple;
                    }

                    for (int i = 0; i < strs.Length; i++)
                    {
                        string Numbers = "";
                        EachMoney = 0;

                        Numbers = strs[i].ToString().Split('|')[0];
                        EachMoney += 2 * double.Parse(strs[i].ToString().Split('|')[1]);

                        Money += EachMoney * EachMultiple;

                        al.Add(new Ticket(102, ConvertFormatToElectronTicket_HPSD(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));
                    }
                }

                Ticket[] Tickets = new Ticket[al.Count];

                for (int i = 0; i < Tickets.Length; i++)
                {
                    Tickets[i] = (Ticket)al[i];
                }

                return Tickets;
            }

            // 转换为需要的彩票格式
            private string ConvertFormatToElectronTicket_HPSD(int PlayTypeID, string Number)
            {
                Number = Number.Trim();

                string Ticket = "";

                if (PlayTypeID == PlayType_D)
                {
                    string[] strs = Number.Split('\n');

                    for (int i = 0; i < strs.Length; i++)
                    {
                        for (int j = 0; j < strs[i].Length; j++)
                        {
                            Ticket += strs[i].Substring(j, 1) + ",";
                        }

                        if (Ticket.EndsWith(","))
                        {
                            Ticket = Ticket.Substring(0, Ticket.Length - 1);
                        }

                        Ticket = Ticket + "\n";
                    }
                }

                if (PlayTypeID == PlayType_F)
                {
                    string[] strs = Number.Split('\n');

                    for (int i = 0; i < strs.Length; i++)
                    {
                        string[] Locate = new string[7];

                        Regex regex = new Regex(@"^(?<L0>(\d)|([(][\d]+?[)]))(?<L1>(\d)|([(][\d]+?[)]))(?<L2>(\d)|([(][\d]+?[)]))(?<L3>(\d)|([(][\d]+?[)]))(?<L4>(\d)|([(][\d]+?[)]))(?<L5>(\d)|([(][\d]+?[)]))(?<L6>(\d)|([(][\d]+?[)]))", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                        Match m = regex.Match(strs[i]);

                        for (int j = 0; j < 7; j++)
                        {
                            Locate[j] = m.Groups["L" + j.ToString()].ToString().Trim();

                            if (Locate[j] == "")
                            {
                                return "";
                            }

                            if (Locate[j].Length > 1)
                            {
                                Locate[j] = Locate[j].Substring(1, Locate[j].Length - 2);
                                if (Locate[j].Length > 1)
                                    Locate[j] = FilterRepeated(Locate[j]);

                                if (Locate[j] == "")
                                {
                                    return "";
                                }
                            }

                            Ticket += Locate[j] + ",";
                        }

                        if (Ticket.EndsWith(","))
                        {
                            Ticket = Ticket.Substring(0, Ticket.Length - 1);
                        }

                        Ticket = Ticket + "\n";
                    }
                }

                if (Ticket.EndsWith(","))
                {
                    Ticket = Ticket.Substring(0, Ticket.Length - 1);
                }

                return Ticket;
            }
            #endregion

            public override Ticket[] ToElectronicTicket_ZCW(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                if (PlayTypeID == PlayType_D)
                {
                    return ToElectronicTicket_ZCW_D(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                if (PlayTypeID == PlayType_F)
                {
                    return ToElectronicTicket_ZCW_F(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                return null;
            }
            #region ToElectronicTicket_ZCW 的具体方法
            private Ticket[] ToElectronicTicket_ZCW_D(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                string[] strs = AnalyseScheme(Number, PlayTypeID).Split('\n');

                if (strs == null)
                {
                    return null;
                }
                if (strs.Length == 0)
                {
                    return null;
                }

                Money = 0;

                int MultipleNum = 0;

                if ((Multiple % MaxMultiple) != 0)
                {
                    MultipleNum = (Multiple - (Multiple % MaxMultiple)) / MaxMultiple + 1;
                }
                else
                {
                    MultipleNum = Multiple / MaxMultiple;
                }

                ArrayList al = new ArrayList();

                int EachMultiple = 1;
                double EachMoney = 0;

                for (int n = 1; n < MultipleNum + 1; n++)
                {
                    if ((n * MaxMultiple) < Multiple)
                    {
                        EachMultiple = MaxMultiple;
                    }
                    else
                    {
                        EachMultiple = Multiple - (n - 1) * MaxMultiple;
                    }

                    for (int i = 0; i < strs.Length; i += 5)
                    {
                        string Numbers = "";
                        EachMoney = 0;

                        for (int M = 0; M < 5; M++)
                        {
                            if ((i + M) < strs.Length)
                            {
                                Numbers += strs[i + M].ToString().Split('|')[0] + "\n";
                                EachMoney += 2 * double.Parse(strs[i + M].ToString().Split('|')[1]);
                            }
                        }

                        Money += EachMoney * EachMultiple;

                        al.Add(new Ticket(0, ConvertFormatToElectronTicket_ZCW(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));
                    }
                }

                Ticket[] Tickets = new Ticket[al.Count];

                for (int i = 0; i < Tickets.Length; i++)
                {
                    Tickets[i] = (Ticket)al[i];
                }

                return Tickets;

            }
            private Ticket[] ToElectronicTicket_ZCW_F(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                string[] strs = AnalyseScheme(Number, PlayTypeID).Split('\n');

                if (strs == null)
                {
                    return null;
                }
                if (strs.Length == 0)
                {
                    return null;
                }

                Money = 0;

                int MultipleNum = 0;

                if ((Multiple % MaxMultiple) != 0)
                {
                    MultipleNum = (Multiple - (Multiple % MaxMultiple)) / MaxMultiple + 1;
                }
                else
                {
                    MultipleNum = Multiple / MaxMultiple;
                }

                ArrayList al = new ArrayList();

                int EachMultiple = 1;
                double EachMoney = 0;

                for (int n = 1; n < MultipleNum + 1; n++)
                {
                    if ((n * MaxMultiple) < Multiple)
                    {
                        EachMultiple = MaxMultiple;
                    }
                    else
                    {
                        EachMultiple = Multiple - (n - 1) * MaxMultiple;
                    }

                    for (int i = 0; i < strs.Length; i++)
                    {
                        string Numbers = "";
                        EachMoney = 0;

                        Numbers = strs[i].ToString().Split('|')[0];
                        EachMoney += 2 * double.Parse(strs[i].ToString().Split('|')[1]);

                        Money += EachMoney * EachMultiple;

                        al.Add(new Ticket(1, ConvertFormatToElectronTicket_ZCW(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));
                    }
                }

                Ticket[] Tickets = new Ticket[al.Count];

                for (int i = 0; i < Tickets.Length; i++)
                {
                    Tickets[i] = (Ticket)al[i];
                }

                return Tickets;
            }

            // 转换为需要的彩票格式
            private string ConvertFormatToElectronTicket_ZCW(int PlayTypeID, string Number)
            {
                Number = Number.Trim();

                string Ticket = "";

                if (PlayTypeID == PlayType_D)
                {
                    string[] strs = Number.Split('\n');

                    for (int i = 0; i < strs.Length; i++)
                    {
                        for (int j = 0; j < strs[i].Length; j++)
                        {
                            Ticket += strs[i].Substring(j, 1) + "*";
                        }

                        if (Ticket.EndsWith("*"))
                        {
                            Ticket = Ticket.Substring(0, Ticket.Length - 1);
                        }

                        Ticket = Ticket + "\n";
                    }
                }

                if (PlayTypeID == PlayType_F)
                {
                    string[] strs = Number.Split('\n');

                    for (int i = 0; i < strs.Length; i++)
                    {
                        string[] Locate = new string[7];

                        Regex regex = new Regex(@"^(?<L0>(\d)|([(][\d]+?[)]))(?<L1>(\d)|([(][\d]+?[)]))(?<L2>(\d)|([(][\d]+?[)]))(?<L3>(\d)|([(][\d]+?[)]))(?<L4>(\d)|([(][\d]+?[)]))(?<L5>(\d)|([(][\d]+?[)]))(?<L6>(\d)|([(][\d]+?[)]))", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                        Match m = regex.Match(strs[i]);

                        for (int j = 0; j < 7; j++)
                        {
                            Locate[j] = m.Groups["L" + j.ToString()].ToString().Trim();

                            if (Locate[j] == "")
                            {
                                return "";
                            }

                            if (Locate[j].Length > 1)
                            {
                                Locate[j] = Locate[j].Substring(1, Locate[j].Length - 2);
                                if (Locate[j].Length > 1)
                                    Locate[j] = FilterRepeated(Locate[j]);

                                if (Locate[j] == "")
                                {
                                    return "";
                                }
                            }

                            Ticket += Locate[j] + "*";
                        }

                        if (Ticket.EndsWith("*"))
                        {
                            Ticket = Ticket.Substring(0, Ticket.Length - 1);
                        }

                        Ticket = Ticket + "\n";
                    }
                }

                if (Ticket.EndsWith("*"))
                {
                    Ticket = Ticket.Substring(0, Ticket.Length - 1);
                }

                return Ticket;
            }
            #endregion

            public override Ticket[] ToElectronicTicket_ZZYTC(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                if (PlayTypeID == PlayType_D)
                {
                    return ToElectronicTicket_ZZYTC_D(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                if (PlayTypeID == PlayType_F)
                {
                    return ToElectronicTicket_ZZYTC_F(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                return null;
            }
            #region ToElectronicTicket_ZZYTC 的具体方法
            private Ticket[] ToElectronicTicket_ZZYTC_D(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                string[] strs = AnalyseScheme(Number, PlayTypeID).Split('\n');

                if (strs == null)
                {
                    return null;
                }
                if (strs.Length == 0)
                {
                    return null;
                }

                Money = 0;

                int MultipleNum = 0;

                if ((Multiple % MaxMultiple) != 0)
                {
                    MultipleNum = (Multiple - (Multiple % MaxMultiple)) / MaxMultiple + 1;
                }
                else
                {
                    MultipleNum = Multiple / MaxMultiple;
                }

                ArrayList al = new ArrayList();

                int EachMultiple = 1;
                double EachMoney = 0;

                for (int n = 1; n < MultipleNum + 1; n++)
                {
                    if ((n * MaxMultiple) < Multiple)
                    {
                        EachMultiple = MaxMultiple;
                    }
                    else
                    {
                        EachMultiple = Multiple - (n - 1) * MaxMultiple;
                    }

                    for (int i = 0; i < strs.Length; i += 5)
                    {
                        string Numbers = "";
                        EachMoney = 0;

                        for (int M = 0; M < 5; M++)
                        {
                            if ((i + M) < strs.Length)
                            {
                                Numbers += strs[i + M].ToString().Split('|')[0] + "\n";
                                EachMoney += 2 * double.Parse(strs[i + M].ToString().Split('|')[1]);
                            }
                        }

                        Money += EachMoney * EachMultiple;

                        al.Add(new Ticket(0, ConvertFormatToElectronTicket_ZZYTC(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));
                    }
                }

                Ticket[] Tickets = new Ticket[al.Count];

                for (int i = 0; i < Tickets.Length; i++)
                {
                    Tickets[i] = (Ticket)al[i];
                }

                return Tickets;

            }
            private Ticket[] ToElectronicTicket_ZZYTC_F(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                string[] strs = AnalyseScheme(Number, PlayTypeID).Split('\n');

                if (strs == null)
                {
                    return null;
                }
                if (strs.Length == 0)
                {
                    return null;
                }

                Money = 0;

                int MultipleNum = 0;

                if ((Multiple % MaxMultiple) != 0)
                {
                    MultipleNum = (Multiple - (Multiple % MaxMultiple)) / MaxMultiple + 1;
                }
                else
                {
                    MultipleNum = Multiple / MaxMultiple;
                }

                ArrayList al = new ArrayList();

                int EachMultiple = 1;
                double EachMoney = 0;

                for (int n = 1; n < MultipleNum + 1; n++)
                {
                    if ((n * MaxMultiple) < Multiple)
                    {
                        EachMultiple = MaxMultiple;
                    }
                    else
                    {
                        EachMultiple = Multiple - (n - 1) * MaxMultiple;
                    }

                    for (int i = 0; i < strs.Length; i++)
                    {
                        string Numbers = "";
                        EachMoney = 0;

                        Numbers = strs[i].ToString().Split('|')[0];
                        EachMoney += 2 * double.Parse(strs[i].ToString().Split('|')[1]);

                        Money += EachMoney * EachMultiple;

                        al.Add(new Ticket(1, ConvertFormatToElectronTicket_ZZYTC(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));
                    }
                }

                Ticket[] Tickets = new Ticket[al.Count];

                for (int i = 0; i < Tickets.Length; i++)
                {
                    Tickets[i] = (Ticket)al[i];
                }

                return Tickets;
            }

            // 转换为需要的彩票格式
            private string ConvertFormatToElectronTicket_ZZYTC(int PlayTypeID, string Number)
            {
                Number = Number.Trim();

                string Ticket = "";

                if (PlayTypeID == PlayType_D)
                {
                    string[] strs = Number.Split('\n');

                    for (int i = 0; i < strs.Length; i++)
                    {
                        for (int j = 0; j < strs[i].Length; j++)
                        {
                            Ticket += strs[i].Substring(j, 1) + "*";
                        }

                        if (Ticket.EndsWith("*"))
                        {
                            Ticket = Ticket.Substring(0, Ticket.Length - 1);
                        }

                        Ticket = Ticket + "\n";
                    }
                }

                if (PlayTypeID == PlayType_F)
                {
                    string[] strs = Number.Split('\n');

                    for (int i = 0; i < strs.Length; i++)
                    {
                        string[] Locate = new string[7];

                        Regex regex = new Regex(@"^(?<L0>(\d)|([(][\d]+?[)]))(?<L1>(\d)|([(][\d]+?[)]))(?<L2>(\d)|([(][\d]+?[)]))(?<L3>(\d)|([(][\d]+?[)]))(?<L4>(\d)|([(][\d]+?[)]))(?<L5>(\d)|([(][\d]+?[)]))(?<L6>(\d)|([(][\d]+?[)]))", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                        Match m = regex.Match(strs[i]);

                        for (int j = 0; j < 7; j++)
                        {
                            Locate[j] = m.Groups["L" + j.ToString()].ToString().Trim();

                            if (Locate[j] == "")
                            {
                                return "";
                            }

                            if (Locate[j].Length > 1)
                            {
                                Locate[j] = Locate[j].Substring(1, Locate[j].Length - 2);
                                if (Locate[j].Length > 1)
                                    Locate[j] = FilterRepeated(Locate[j]);

                                if (Locate[j] == "")
                                {
                                    return "";
                                }
                            }

                            Ticket += Locate[j] + "*";
                        }

                        if (Ticket.EndsWith("*"))
                        {
                            Ticket = Ticket.Substring(0, Ticket.Length - 1);
                        }

                        Ticket = Ticket + "\n";
                    }
                }

                if (Ticket.EndsWith(","))
                {
                    Ticket = Ticket.Substring(0, Ticket.Length - 1);
                }

                return Ticket;
            }
            #endregion

            public override Ticket[] ToElectronicTicket_DYJ(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                if (PlayTypeID == PlayType_D)
                {
                    return ToElectronicTicket_DYJ_D(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                if (PlayTypeID == PlayType_F)
                {
                    return ToElectronicTicket_DYJ_F(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                return null;
            }
            #region ToElectronicTicket_DYJ 的具体方法
            private Ticket[] ToElectronicTicket_DYJ_D(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                string[] strs = AnalyseScheme(Number, PlayTypeID).Split('\n');

                if (strs == null)
                {
                    return null;
                }
                if (strs.Length == 0)
                {
                    return null;
                }

                Money = 0;

                int MultipleNum = 0;

                if ((Multiple % MaxMultiple) != 0)
                {
                    MultipleNum = (Multiple - (Multiple % MaxMultiple)) / MaxMultiple + 1;
                }
                else
                {
                    MultipleNum = Multiple / MaxMultiple;
                }

                ArrayList al = new ArrayList();

                int EachMultiple = 1;
                double EachMoney = 0;

                for (int n = 1; n < MultipleNum + 1; n++)
                {
                    if ((n * MaxMultiple) < Multiple)
                    {
                        EachMultiple = MaxMultiple;
                    }
                    else
                    {
                        EachMultiple = Multiple - (n - 1) * MaxMultiple;
                    }

                    for (int i = 0; i < strs.Length; i += 5)
                    {
                        string Numbers = "";
                        EachMoney = 0;

                        for (int M = 0; M < 5; M++)
                        {
                            if ((i + M) < strs.Length)
                            {
                                Numbers += strs[i + M].ToString().Split('|')[0] + "\n";
                                EachMoney += 2 * double.Parse(strs[i + M].ToString().Split('|')[1]);
                            }
                        }

                        Money += EachMoney * EachMultiple;

                        al.Add(new Ticket(101, ConvertFormatToElectronTicket_DYJ(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));
                    }
                }

                Ticket[] Tickets = new Ticket[al.Count];

                for (int i = 0; i < Tickets.Length; i++)
                {
                    Tickets[i] = (Ticket)al[i];
                }

                return Tickets;

            }
            private Ticket[] ToElectronicTicket_DYJ_F(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                string[] strs = AnalyseScheme(Number, PlayTypeID).Split('\n');

                if (strs == null)
                {
                    return null;
                }
                if (strs.Length == 0)
                {
                    return null;
                }

                Money = 0;

                int MultipleNum = 0;

                if ((Multiple % MaxMultiple) != 0)
                {
                    MultipleNum = (Multiple - (Multiple % MaxMultiple)) / MaxMultiple + 1;
                }
                else
                {
                    MultipleNum = Multiple / MaxMultiple;
                }

                ArrayList al = new ArrayList();

                int EachMultiple = 1;
                double EachMoney = 0;

                for (int n = 1; n < MultipleNum + 1; n++)
                {
                    if ((n * MaxMultiple) < Multiple)
                    {
                        EachMultiple = MaxMultiple;
                    }
                    else
                    {
                        EachMultiple = Multiple - (n - 1) * MaxMultiple;
                    }

                    for (int i = 0; i < strs.Length; i++)
                    {
                        string Numbers = "";
                        EachMoney = 0;

                        Numbers = strs[i].ToString().Split('|')[0];
                        EachMoney += 2 * double.Parse(strs[i].ToString().Split('|')[1]);

                        Money += EachMoney * EachMultiple;

                        al.Add(new Ticket(102, ConvertFormatToElectronTicket_DYJ(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));
                    }
                }

                Ticket[] Tickets = new Ticket[al.Count];

                for (int i = 0; i < Tickets.Length; i++)
                {
                    Tickets[i] = (Ticket)al[i];
                }

                return Tickets;
            }

            private string ConvertFormatToElectronTicket_DYJ(int PlayTypeID, string Number)
            {
                Number = Number.Trim();

                string Ticket = "";

                if (PlayTypeID == PlayType_D)
                {
                    string[] strs = Number.Split('\n');

                    for (int i = 0; i < strs.Length; i++)
                    {
                        for (int j = 0; j < strs[i].Length; j++)
                        {
                            Ticket += strs[i].Substring(j, 1) + ",";
                        }

                        if (Ticket.EndsWith(","))
                        {
                            Ticket = Ticket.Substring(0, Ticket.Length - 1);
                        }

                        Ticket = Ticket + ";\n";
                    }
                }

                if (PlayTypeID == PlayType_F)
                {
                    string[] strs = Number.Split('\n');

                    for (int i = 0; i < strs.Length; i++)
                    {
                        string[] Locate = new string[7];

                        Regex regex = new Regex(@"^(?<L0>(\d)|([(][\d]+?[)]))(?<L1>(\d)|([(][\d]+?[)]))(?<L2>(\d)|([(][\d]+?[)]))(?<L3>(\d)|([(][\d]+?[)]))(?<L4>(\d)|([(][\d]+?[)]))(?<L5>(\d)|([(][\d]+?[)]))(?<L6>(\d)|([(][\d]+?[)]))", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                        Match m = regex.Match(strs[i]);

                        for (int j = 0; j < 7; j++)
                        {
                            Locate[j] = m.Groups["L" + j.ToString()].ToString().Trim();

                            if (Locate[j] == "")
                            {
                                return "";
                            }

                            if (Locate[j].Length > 1)
                            {
                                Locate[j] = Locate[j].Substring(1, Locate[j].Length - 2);
                                if (Locate[j].Length > 1)
                                    Locate[j] = FilterRepeated(Locate[j]);

                                if (Locate[j] == "")
                                {
                                    return "";
                                }
                            }

                            Ticket += Locate[j] + ",";
                        }

                        if (Ticket.EndsWith(","))
                        {
                            Ticket = Ticket.Substring(0, Ticket.Length - 1);
                        }

                        Ticket = Ticket + ";\n";
                    }
                }

                if (Ticket.EndsWith(","))
                {
                    Ticket = Ticket.Substring(0, Ticket.Length - 1);
                }

                return Ticket;
            }
            #endregion

            public override Ticket[] ToElectronicTicket_CTTCSD(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                if (PlayTypeID == PlayType_D)
                {
                    return ToElectronicTicket_CTTCSD_D(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                if (PlayTypeID == PlayType_F)
                {
                    return ToElectronicTicket_CTTCSD_F(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                return null;
            }
            #region ToElectronicTicket_CTTCSD 的具体方法
            private Ticket[] ToElectronicTicket_CTTCSD_D(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                string[] strs = AnalyseScheme(Number, PlayTypeID).Split('\n');

                if (strs == null)
                {
                    return null;
                }
                if (strs.Length == 0)
                {
                    return null;
                }

                Money = 0;

                int MultipleNum = 0;

                if ((Multiple % MaxMultiple) != 0)
                {
                    MultipleNum = (Multiple - (Multiple % MaxMultiple)) / MaxMultiple + 1;
                }
                else
                {
                    MultipleNum = Multiple / MaxMultiple;
                }

                ArrayList al = new ArrayList();

                int EachMultiple = 1;
                double EachMoney = 0;

                for (int n = 1; n < MultipleNum + 1; n++)
                {
                    if ((n * MaxMultiple) < Multiple)
                    {
                        EachMultiple = MaxMultiple;
                    }
                    else
                    {
                        EachMultiple = Multiple - (n - 1) * MaxMultiple;
                    }

                    for (int i = 0; i < strs.Length; i += 5)
                    {
                        string Numbers = "";
                        EachMoney = 0;

                        for (int M = 0; M < 5; M++)
                        {
                            if ((i + M) < strs.Length)
                            {
                                Numbers += strs[i + M].ToString().Split('|')[0] + "\n";
                                EachMoney += 2 * double.Parse(strs[i + M].ToString().Split('|')[1]);
                            }
                        }

                        Money += EachMoney * EachMultiple;

                        al.Add(new Ticket(0, ConvertFormatToElectronTicket_CTTCSD(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));
                    }
                }

                Ticket[] Tickets = new Ticket[al.Count];

                for (int i = 0; i < Tickets.Length; i++)
                {
                    Tickets[i] = (Ticket)al[i];
                }

                return Tickets;

            }
            private Ticket[] ToElectronicTicket_CTTCSD_F(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                string[] strs = AnalyseScheme(Number, PlayTypeID).Split('\n');

                if (strs == null)
                {
                    return null;
                }
                if (strs.Length == 0)
                {
                    return null;
                }

                Money = 0;

                int MultipleNum = 0;

                if ((Multiple % MaxMultiple) != 0)
                {
                    MultipleNum = (Multiple - (Multiple % MaxMultiple)) / MaxMultiple + 1;
                }
                else
                {
                    MultipleNum = Multiple / MaxMultiple;
                }

                ArrayList al = new ArrayList();

                int EachMultiple = 1;
                double EachMoney = 0;

                for (int n = 1; n < MultipleNum + 1; n++)
                {
                    if ((n * MaxMultiple) < Multiple)
                    {
                        EachMultiple = MaxMultiple;
                    }
                    else
                    {
                        EachMultiple = Multiple - (n - 1) * MaxMultiple;
                    }

                    for (int i = 0; i < strs.Length; i++)
                    {
                        string Numbers = "";
                        EachMoney = 0;

                        Numbers = strs[i].ToString().Split('|')[0];
                        EachMoney += 2 * double.Parse(strs[i].ToString().Split('|')[1]);

                        Money += EachMoney * EachMultiple;

                        al.Add(new Ticket(1, ConvertFormatToElectronTicket_CTTCSD(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));
                    }
                }

                Ticket[] Tickets = new Ticket[al.Count];

                for (int i = 0; i < Tickets.Length; i++)
                {
                    Tickets[i] = (Ticket)al[i];
                }

                return Tickets;
            }

            // 转换为需要的彩票格式
            private string ConvertFormatToElectronTicket_CTTCSD(int PlayTypeID, string Number)
            {
                Number = Number.Trim();

                string Ticket = "";

                if (PlayTypeID == PlayType_D)
                {
                    string[] strs = Number.Split('\n');

                    for (int i = 0; i < strs.Length; i++)
                    {
                        for (int j = 0; j < strs[i].Length; j++)
                        {
                            Ticket += strs[i].Substring(j, 1) + "*";
                        }

                        if (Ticket.EndsWith("*"))
                        {
                            Ticket = Ticket.Substring(0, Ticket.Length - 1);
                        }

                        Ticket = Ticket + "\n";
                    }
                }

                if (PlayTypeID == PlayType_F)
                {
                    string[] strs = Number.Split('\n');

                    for (int i = 0; i < strs.Length; i++)
                    {
                        string[] Locate = new string[7];

                        Regex regex = new Regex(@"^(?<L0>(\d)|([(][\d]+?[)]))(?<L1>(\d)|([(][\d]+?[)]))(?<L2>(\d)|([(][\d]+?[)]))(?<L3>(\d)|([(][\d]+?[)]))(?<L4>(\d)|([(][\d]+?[)]))(?<L5>(\d)|([(][\d]+?[)]))(?<L6>(\d)|([(][\d]+?[)]))", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                        Match m = regex.Match(strs[i]);

                        for (int j = 0; j < 7; j++)
                        {
                            Locate[j] = m.Groups["L" + j.ToString()].ToString().Trim();

                            if (Locate[j] == "")
                            {
                                return "";
                            }

                            if (Locate[j].Length > 1)
                            {
                                Locate[j] = Locate[j].Substring(1, Locate[j].Length - 2);
                                if (Locate[j].Length > 1)
                                    Locate[j] = FilterRepeated(Locate[j]);

                                if (Locate[j] == "")
                                {
                                    return "";
                                }
                            }

                            Ticket += Locate[j] + "*";
                        }

                        if (Ticket.EndsWith("*"))
                        {
                            Ticket = Ticket.Substring(0, Ticket.Length - 1);
                        }

                        Ticket = Ticket + "\n";
                    }
                }

                if (Ticket.EndsWith(","))
                {
                    Ticket = Ticket.Substring(0, Ticket.Length - 1);
                }

                return Ticket;
            }
            #endregion

            public override Ticket[] ToElectronicTicket_BJCP(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                if (PlayTypeID == PlayType_D)
                {
                    return ToElectronicTicket_BJCP_D(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                if (PlayTypeID == PlayType_F)
                {
                    return ToElectronicTicket_BJCP_F(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                return null;
            }
            #region ToElectronicTicket_BJCP 的具体方法
            private Ticket[] ToElectronicTicket_BJCP_D(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                string[] strs = AnalyseScheme(Number, PlayTypeID).Split('\n');

                if (strs == null)
                {
                    return null;
                }
                if (strs.Length == 0)
                {
                    return null;
                }

                Money = 0;

                int MultipleNum = 0;

                if ((Multiple % MaxMultiple) != 0)
                {
                    MultipleNum = (Multiple - (Multiple % MaxMultiple)) / MaxMultiple + 1;
                }
                else
                {
                    MultipleNum = Multiple / MaxMultiple;
                }

                ArrayList al = new ArrayList();

                int EachMultiple = 1;
                double EachMoney = 0;

                for (int n = 1; n < MultipleNum + 1; n++)
                {
                    if ((n * MaxMultiple) < Multiple)
                    {
                        EachMultiple = MaxMultiple;
                    }
                    else
                    {
                        EachMultiple = Multiple - (n - 1) * MaxMultiple;
                    }

                    for (int i = 0; i < strs.Length; i += 5)
                    {
                        string Numbers = "";
                        EachMoney = 0;

                        for (int M = 0; M < 5; M++)
                        {
                            if ((i + M) < strs.Length)
                            {
                                Numbers += strs[i + M].ToString().Split('|')[0] + "\n";
                                EachMoney += 2 * double.Parse(strs[i + M].ToString().Split('|')[1]);
                            }
                        }

                        Money += EachMoney * EachMultiple;

                        al.Add(new Ticket(0, ConvertFormatToElectronTicket_BJCP(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));
                    }
                }

                Ticket[] Tickets = new Ticket[al.Count];

                for (int i = 0; i < Tickets.Length; i++)
                {
                    Tickets[i] = (Ticket)al[i];
                }

                return Tickets;

            }
            private Ticket[] ToElectronicTicket_BJCP_F(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                string[] strs = AnalyseScheme(Number, PlayTypeID).Split('\n');

                if (strs == null)
                {
                    return null;
                }
                if (strs.Length == 0)
                {
                    return null;
                }

                Money = 0;

                int MultipleNum = 0;

                if ((Multiple % MaxMultiple) != 0)
                {
                    MultipleNum = (Multiple - (Multiple % MaxMultiple)) / MaxMultiple + 1;
                }
                else
                {
                    MultipleNum = Multiple / MaxMultiple;
                }

                ArrayList al = new ArrayList();

                int EachMultiple = 1;
                double EachMoney = 0;

                for (int n = 1; n < MultipleNum + 1; n++)
                {
                    if ((n * MaxMultiple) < Multiple)
                    {
                        EachMultiple = MaxMultiple;
                    }
                    else
                    {
                        EachMultiple = Multiple - (n - 1) * MaxMultiple;
                    }

                    for (int i = 0; i < strs.Length; i++)
                    {
                        string Numbers = "";
                        EachMoney = 0;

                        Numbers = strs[i].ToString().Split('|')[0];
                        EachMoney += 2 * double.Parse(strs[i].ToString().Split('|')[1]);

                        Money += EachMoney * EachMultiple;

                        al.Add(new Ticket(1, ConvertFormatToElectronTicket_BJCP(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));
                    }
                }

                Ticket[] Tickets = new Ticket[al.Count];

                for (int i = 0; i < Tickets.Length; i++)
                {
                    Tickets[i] = (Ticket)al[i];
                }

                return Tickets;
            }

            // 转换为需要的彩票格式
            private string ConvertFormatToElectronTicket_BJCP(int PlayTypeID, string Number)
            {
                Number = Number.Trim();

                string Ticket = "";

                if (PlayTypeID == PlayType_D)
                {
                    string[] strs = Number.Split('\n');

                    for (int i = 0; i < strs.Length; i++)
                    {
                        for (int j = 0; j < strs[i].Length; j++)
                        {
                            Ticket += strs[i].Substring(j, 1) + "*";
                        }

                        if (Ticket.EndsWith("*"))
                        {
                            Ticket = Ticket.Substring(0, Ticket.Length - 1);
                        }

                        Ticket = Ticket + "\n";
                    }
                }

                if (PlayTypeID == PlayType_F)
                {
                    string[] strs = Number.Split('\n');

                    for (int i = 0; i < strs.Length; i++)
                    {
                        string[] Locate = new string[7];

                        Regex regex = new Regex(@"^(?<L0>(\d)|([(][\d]+?[)]))(?<L1>(\d)|([(][\d]+?[)]))(?<L2>(\d)|([(][\d]+?[)]))(?<L3>(\d)|([(][\d]+?[)]))(?<L4>(\d)|([(][\d]+?[)]))(?<L5>(\d)|([(][\d]+?[)]))(?<L6>(\d)|([(][\d]+?[)]))", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                        Match m = regex.Match(strs[i]);

                        for (int j = 0; j < 7; j++)
                        {
                            Locate[j] = m.Groups["L" + j.ToString()].ToString().Trim();

                            if (Locate[j] == "")
                            {
                                return "";
                            }

                            if (Locate[j].Length > 1)
                            {
                                Locate[j] = Locate[j].Substring(1, Locate[j].Length - 2);
                                if (Locate[j].Length > 1)
                                    Locate[j] = FilterRepeated(Locate[j]);

                                if (Locate[j] == "")
                                {
                                    return "";
                                }
                            }

                            Ticket += Locate[j] + "*";
                        }

                        if (Ticket.EndsWith("*"))
                        {
                            Ticket = Ticket.Substring(0, Ticket.Length - 1);
                        }

                        Ticket = Ticket + "\n";
                    }
                }

                if (Ticket.EndsWith("*"))
                {
                    Ticket = Ticket.Substring(0, Ticket.Length - 1);
                }

                return Ticket;
            }
            #endregion
        }
    }
}
