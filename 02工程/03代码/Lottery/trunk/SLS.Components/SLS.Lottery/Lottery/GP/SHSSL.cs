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
        /// 上海时时乐
        /// </summary>
        public partial class SHSSL : LotteryBase
        {
            #region 静态变量
            public const int PlayType_Mixed = 2900; //混合投注，包含下面所有的玩法，号码前加 [直选单式]、[直选复式]、[组选单式]、[组选6复式]...

            public const int PlayType_ZhiD = 2901;  //直选单式
            public const int PlayType_ZhiF = 2902;  //直选复式

            public const int PlayType_ZuD = 2903;   //组选单式

            public const int PlayType_Zu6F = 2904;  //组选6复式
            public const int PlayType_Zu3F = 2905;  //组选3复式

            public const int PlayType_ZhiH = 2906;  //直选和值
            public const int PlayType_ZuH = 2907;  //组选和值

            public const int PlayType_Q2D = 2908;   //前2单式
            public const int PlayType_Q2F = 2909;   //前2复式

            public const int PlayType_H2D = 2910;   //后2单式
            public const int PlayType_H2F = 2911;   //后2复式

            public const int PlayType_Q1D = 2912;   //前1单式
            public const int PlayType_Q1F = 2913;   //前1复式

            public const int PlayType_H1D = 2914;   //后1单式
            public const int PlayType_H1F = 2915;   //后1复式

            public const int ID = 29;
            public const string sID = "29";
            public const string Name = "上海时时乐";
            public const string Code = "SHSSL";
            public const double MaxMoney = 20000;
            #endregion

            public SHSSL()
            {
                id = 29;
                name = "上海时时乐";
                code = "SHSSL";
            }

            public override bool CheckPlayType(int play_type)
            {
                return ((play_type >= 2900) && (play_type <= 2915));
            }

            public override PlayType[] GetPlayTypeList()
            {
                PlayType[] Result = new PlayType[16];

                Result[0] = new PlayType(PlayType_Mixed, "混合投注");
                Result[1] = new PlayType(PlayType_ZhiD, "直选单式");
                Result[2] = new PlayType(PlayType_ZhiF, "直选复式");
                Result[3] = new PlayType(PlayType_ZuD, "组选单式");
                Result[4] = new PlayType(PlayType_Zu6F, "组选6复式");
                Result[5] = new PlayType(PlayType_Zu3F, "组选3复式");
                Result[6] = new PlayType(PlayType_ZhiH, "直选和值");
                Result[7] = new PlayType(PlayType_ZuH, "组选和值");
                Result[8] = new PlayType(PlayType_Q2D, "前2单式");
                Result[9] = new PlayType(PlayType_Q2F, "前2复式");
                Result[10] = new PlayType(PlayType_H2D, "后2单式");
                Result[11] = new PlayType(PlayType_H2F, "后2复式");
                Result[12] = new PlayType(PlayType_Q1D, "前1单式");
                Result[13] = new PlayType(PlayType_Q1F, "前1复式");
                Result[14] = new PlayType(PlayType_H1D, "后1单式");
                Result[15] = new PlayType(PlayType_H1F, "后1复式");

                return Result;
            }

            public override string BuildNumber(int Num)	//id = 29
            {
                System.Random rd = new Random();
                StringBuilder sb = new StringBuilder();

                for (int i = 0; i < Num; i++)
                {
                    string LotteryNumber = "";
                    for (int j = 0; j < 3; j++)
                    {
                        LotteryNumber += rd.Next(0, 9 + 1).ToString();
                    }

                    sb.Append(LotteryNumber.Trim() + "\n");
                }

                string Result = sb.ToString();
                Result = Result.Substring(0, Result.Length - 1);

                return Result;
            }

            public override string[] ToSingle(string Number, ref string CanonicalNumber, int PlayType)
            {
                if (PlayType == PlayType_Mixed)   //混合投注
                    return ToSingle_Mixed(Number, ref CanonicalNumber);

                if ((PlayType == PlayType_ZhiD) || (PlayType == PlayType_ZhiF))
                    return ToSingle_Zhi(Number, ref CanonicalNumber);

                if ((PlayType == PlayType_ZuD) || (PlayType == PlayType_Zu6F))
                    return ToSingle_Zu3D_Zu6(Number, ref CanonicalNumber);

                if ((PlayType == PlayType_Zu3F))
                    return ToSingle_Zu3F(Number, ref CanonicalNumber);

                if ((PlayType == PlayType_ZhiH))
                    return ToSingle_ZhiH(Number, ref CanonicalNumber);

                if ((PlayType == PlayType_ZuH))
                    return ToSingle_ZuH(Number, ref CanonicalNumber);

                if ((PlayType == PlayType_Q2D) || (PlayType == PlayType_Q2F) || (PlayType == PlayType_H2D) || (PlayType == PlayType_H2F))
                    return ToSingle_2(Number, ref CanonicalNumber);

                if ((PlayType == PlayType_Q1D) || (PlayType == PlayType_Q1F) || (PlayType == PlayType_H1D) || (PlayType == PlayType_H1F))
                    return ToSingle_1(Number, ref CanonicalNumber);

                return null;
            }
            #region ToSingle 的具体方法
            private string[] ToSingle_Mixed(string Number, ref string CanonicalNumber)
            {
                CanonicalNumber = "";

                string PreFix = GetLotteryNumberPreFix(Number);

                if (Number.StartsWith("[直选单式]") || Number.StartsWith("[直选复式]"))
                {
                    return MergeLotteryNumberPreFix(ToSingle_Zhi(FilterPreFix(Number), ref CanonicalNumber), PreFix);
                }

                if (Number.StartsWith("[组选单式]") || Number.StartsWith("[组选6复式]"))
                {
                    return MergeLotteryNumberPreFix(ToSingle_Zu3D_Zu6(FilterPreFix(Number), ref CanonicalNumber), PreFix);
                }

                if (Number.StartsWith("[组选3复式]"))
                {
                    return MergeLotteryNumberPreFix(ToSingle_Zu3F(FilterPreFix(Number), ref CanonicalNumber), PreFix);
                }

                if (Number.StartsWith("[直选和值]"))
                {
                    return MergeLotteryNumberPreFix(ToSingle_ZhiH(FilterPreFix(Number), ref CanonicalNumber), PreFix);
                }

                if (Number.StartsWith("[组选和值]"))
                {
                    return MergeLotteryNumberPreFix(ToSingle_ZuH(FilterPreFix(Number), ref CanonicalNumber), PreFix);
                }

                if (Number.StartsWith("[前2单式]") || Number.StartsWith("[前2复式]") || Number.StartsWith("[后2单式]") || Number.StartsWith("[后2复式]"))
                {
                    return MergeLotteryNumberPreFix(ToSingle_2(FilterPreFix(Number), ref CanonicalNumber), PreFix);
                }

                if (Number.StartsWith("[前1单式]") || Number.StartsWith("[前1复式]") || Number.StartsWith("[后1单式]") || Number.StartsWith("[后1复式]"))
                {
                    return MergeLotteryNumberPreFix(ToSingle_1(FilterPreFix(Number), ref CanonicalNumber), PreFix);
                }

                return null;
            }
            private string[] ToSingle_Zhi(string Number, ref string CanonicalNumber)	//直选复式取单式, 后面 ref 参数是将彩票规范化，如：10(223) 变成10(23)
            {
                string[] Locate = new string[3];
                CanonicalNumber = "";

                Regex regex = new Regex(@"^(?<L0>(\d)|([(][\d]+?[)]))(?<L1>(\d)|([(][\d]+?[)]))(?<L2>(\d)|([(][\d]+?[)]))", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                Match m = regex.Match(Number);
                for (int i = 0; i < 3; i++)
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
                            al.Add(str_2);
                        }
                    }
                }
                #endregion

                string[] Result = new string[al.Count];
                for (int i = 0; i < al.Count; i++)
                    Result[i] = al[i].ToString();
                return Result;
            }
            private string[] ToSingle_Zu3D_Zu6(string Number, ref string CanonicalNumber)	//复式取单式, 后面 ref 参数是将彩票规范化，如：(10223) 变成1023
            {
                CanonicalNumber = FilterRepeated(Number.Trim());
                if (CanonicalNumber.Length < 2)
                {
                    CanonicalNumber = "";
                    return null;
                }

                char[] strs = CanonicalNumber.ToCharArray();

                ArrayList al = new ArrayList();

                #region 循环取单式

                int n = strs.Length;

                if (n == 2)
                {
                    for (int i = 0; i < n - 1; i++)
                        for (int j = i + 1; j < n; j++)
                        {
                            al.Add(strs[i].ToString() + strs[i].ToString() + strs[j].ToString());
                            al.Add(strs[i].ToString() + strs[j].ToString() + strs[j].ToString());
                        }

                }
                else
                {
                    for (int i = 0; i < n - 2; i++)
                        for (int j = i + 1; j < n - 1; j++)
                            for (int k = j + 1; k < n; k++)
                                al.Add(strs[i].ToString() + strs[j].ToString() + strs[k].ToString());
                }
                #endregion

                string[] Result = new string[al.Count];
                for (int i = 0; i < al.Count; i++)
                    Result[i] = al[i].ToString();
                return Result;
            }
            private string[] ToSingle_Zu3F(string Number, ref string CanonicalNumber)	//组选3取单式,后面 ref 参数是将彩票规范化，如：10223) 变成1023，由于单式是3个数，复式可以是2个数，所以，这个函数不同于其他彩种的类似函数，单式不能使用这个函数转换。
            {
                CanonicalNumber = FilterRepeated(Number.Trim());
                if (CanonicalNumber.Length < 2)
                {
                    CanonicalNumber = "";
                    return null;
                }

                char[] strs = CanonicalNumber.ToCharArray();

                ArrayList al = new ArrayList();

                #region 循环取单式

                int n = strs.Length;
                for (int i = 0; i < n - 1; i++)
                    for (int j = i + 1; j < n; j++)
                    {
                        al.Add(strs[i].ToString() + strs[i].ToString() + strs[j].ToString());
                        al.Add(strs[i].ToString() + strs[j].ToString() + strs[j].ToString());
                    }

                #endregion

                string[] Result = new string[al.Count];
                for (int i = 0; i < al.Count; i++)
                    Result[i] = al[i].ToString();
                return Result;
            }
            private string[] ToSingle_ZhiH(string sBill, ref string CanonicalNumber)	//直选和值取单式,后面 ref 参数是将彩票规范化，如：05) 变成 5
            {
                int Bill = Shove._Convert.StrToInt(sBill, -1);
                CanonicalNumber = "";

                if ((Bill < 0) || (Bill > 27))
                {
                    CanonicalNumber = "";
                    return null;
                }

                CanonicalNumber = Bill.ToString();

                ArrayList al = new ArrayList();

                #region 循环取单式

                for (int i = 0; i <= 9; i++)
                    for (int j = 0; j <= 9; j++)
                        for (int k = 0; k <= 9; k++)
                            if (i + j + k == Bill)
                                al.Add(i.ToString() + j.ToString() + k.ToString());

                #endregion

                if (al.Count == 0)
                    return null;

                string[] Result = new string[al.Count];
                for (int i = 0; i < al.Count; i++)
                    Result[i] = al[i].ToString();
                return Result;
            }
            private string[] ToSingle_ZuH(string sBill, ref string CanonicalNumber)	//组选和值取单式,后面 ref 参数是将彩票规范化，如：05) 变成 5
            {
                int Bill = Shove._Convert.StrToInt(sBill, -1);
                CanonicalNumber = "";

                ArrayList al = new ArrayList();

                #region 循环取单式

                if ((Bill < 1) || (Bill > 26))
                {
                    CanonicalNumber = "";
                    return null;
                }
                else
                {
                    for (int i = 0; i <= 9; i++)
                        for (int j = 0; j <= 9; j++)
                        {
                            if (i == j)
                                continue;
                            if (i + i + j == Bill)
                                al.Add(i.ToString() + i.ToString() + j.ToString());
                        }
                }

                if ((Bill >= 3) && (Bill <= 24))
                {
                    for (int i = 0; i <= 7; i++)
                        for (int j = i + 1; j <= 8; j++)
                            for (int k = j + 1; k <= 9; k++)
                                if (i + j + k == Bill)
                                    al.Add(i.ToString() + j.ToString() + k.ToString());
                }

                #endregion

                CanonicalNumber = Bill.ToString();

                if (al.Count == 0)
                    return null;

                string[] Result = new string[al.Count];
                for (int i = 0; i < al.Count; i++)
                    Result[i] = al[i].ToString();
                return Result;
            }
            private string[] ToSingle_2(string Number, ref string CanonicalNumber)      //前2后2复式取单式, 后面 ref 参数是将彩票规范化，如：1(223) 变成1(23)
            {
                string[] Locate = new string[2];
                CanonicalNumber = "";

                Regex regex = new Regex(@"^(?<L0>(\d)|([(][\d]+?[)]))(?<L1>(\d)|([(][\d]+?[)]))", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                Match m = regex.Match(Number);
                for (int i = 0; i < 2; i++)
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
                        al.Add(str_1);
                    }
                }
                #endregion

                string[] Result = new string[al.Count];
                for (int i = 0; i < al.Count; i++)
                    Result[i] = al[i].ToString();
                return Result;
            }
            private string[] ToSingle_1(string Number, ref string CanonicalNumber)      //前1后1复式取单式, 后面 ref 参数是将彩票规范化，如：(10223) 变成(1023)
            {
                char[] chars = FilterRepeated(Number.Trim()).ToCharArray();
                CanonicalNumber = "";

                if (chars.Length < 1)
                {
                    CanonicalNumber = "";
                    return null;
                }

                string[] strs = new string[chars.Length];
                for (int i = 0; i < chars.Length; i++)
                {
                    strs[i] = chars[i].ToString();
                    CanonicalNumber += chars[i].ToString();
                }

                return strs;
            }
            #endregion

            public override double ComputeWin(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, int PlayType, params double[] WinMoneyList)
            {
                Description = "";
                WinMoneyNoWithTax = 0;

                if ((WinMoneyList == null) || (WinMoneyList.Length < 14)) //奖金参数排列顺序zhi(0,1),zu3(2,3),zu6(4,5) 前2(6,7)后2(8,9),前1(10,11)后1(12,13)
                    return -3;

                int WinCount = 0;
                int WinCount_Zu3 = 0;
                int WinCount_Zu6 = 0;

                if (PlayType == PlayType_Mixed)   //混合投注
                    return ComputeWin_Mixed(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[0], WinMoneyList[1], WinMoneyList[2], WinMoneyList[3], WinMoneyList[4], WinMoneyList[5], WinMoneyList[6], WinMoneyList[7], WinMoneyList[8], WinMoneyList[9], WinMoneyList[10], WinMoneyList[11], WinMoneyList[12], WinMoneyList[13]);

                if ((PlayType == PlayType_ZhiD) || (PlayType == PlayType_ZhiF))
                    return ComputeWin_Zhi(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[0], WinMoneyList[1], ref WinCount);

                if ((PlayType == PlayType_ZuD) || (PlayType == PlayType_Zu6F))
                    return ComputeWin_Zu3D_Zu6(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[2], WinMoneyList[3], WinMoneyList[4], WinMoneyList[5], ref WinCount_Zu3, ref WinCount_Zu6);

                if (PlayType == PlayType_Zu3F)
                    return ComputeWin_Zu3F(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[2], WinMoneyList[3], ref WinCount);

                if (PlayType == PlayType_ZhiH)
                    return ComputeWin_ZhiH(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[0], WinMoneyList[1], ref WinCount);

                if (PlayType == PlayType_ZuH)
                    return ComputeWin_ZuH(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[2], WinMoneyList[3], WinMoneyList[4], WinMoneyList[5], ref WinCount_Zu3, ref WinCount_Zu6);

                if ((PlayType == PlayType_Q2D) || (PlayType == PlayType_Q2F))
                    return ComputeWin_Q2(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[6], WinMoneyList[7], ref WinCount);

                if ((PlayType == PlayType_H2D) || (PlayType == PlayType_H2F))
                    return ComputeWin_H2(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[8], WinMoneyList[9], ref WinCount);

                if ((PlayType == PlayType_Q1D) || (PlayType == PlayType_Q1F))
                    return ComputeWin_Q1(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[10], WinMoneyList[11], ref WinCount);

                if ((PlayType == PlayType_H1D) || (PlayType == PlayType_H1F))
                    return ComputeWin_H1(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[12], WinMoneyList[13], ref WinCount);

                return -4;
            }
            #region ComputeWin  的具体方法
            private double ComputeWin_Mixed(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney1, double WinMoneyNoWithTax1, double WinMoney2, double WinMoneyNoWithTax2, double WinMoney3, double WinMoneyNoWithTax3, double WinMoney4, double WinMoneyNoWithTax4, double WinMoney5, double WinMoneyNoWithTax5, double WinMoney6, double WinMoneyNoWithTax6, double WinMoney7, double WinMoneyNoWithTax7)
            {
                string[] Lotterys = SplitLotteryNumber(Number);
                if (Lotterys == null)
                {
                    return -2;
                }

                if (Lotterys.Length < 1)
                {
                    return -2;
                }

                double WinMoney = 0;

                int WinCount1 = 0, WinCount2 = 0, WinCount3 = 0, WinCount4 = 0, WinCount5 = 0, WinCount6 = 0, WinCount7 = 0;

                for (int ii = 0; ii < Lotterys.Length; ii++)
                {
                    int WinCount = 0;
                    int WinCount_Zu3 = 0;
                    int WinCount_Zu6 = 0;

                    double t_WinMoneyNoWithTax = 0;

                    if (Lotterys[ii].StartsWith("[直选单式]") || Lotterys[ii].StartsWith("[直选复式]"))
                    {
                        WinMoney += ComputeWin_Zhi(FilterPreFix(Lotterys[ii]), WinNumber, ref Description, ref t_WinMoneyNoWithTax, WinMoney1, WinMoneyNoWithTax1, ref WinCount);

                        WinMoneyNoWithTax += t_WinMoneyNoWithTax;
                        WinCount1 += WinCount;
                    }
                    else if (Lotterys[ii].StartsWith("[组选单式]") || Lotterys[ii].StartsWith("[组选6复式]"))
                    {
                        WinMoney += ComputeWin_Zu3D_Zu6(FilterPreFix(Lotterys[ii]), WinNumber, ref Description, ref t_WinMoneyNoWithTax, WinMoney2, WinMoneyNoWithTax2, WinMoney3, WinMoneyNoWithTax3, ref WinCount_Zu3, ref WinCount_Zu6);

                        WinMoneyNoWithTax += t_WinMoneyNoWithTax;
                        WinCount2 += WinCount_Zu3;
                        WinCount3 += WinCount_Zu6;
                    }
                    else if (Lotterys[ii].StartsWith("[组选3复式]"))
                    {
                        WinMoney += ComputeWin_Zu3F(FilterPreFix(Lotterys[ii]), WinNumber, ref Description, ref t_WinMoneyNoWithTax, WinMoney2, WinMoneyNoWithTax2, ref WinCount);

                        WinMoneyNoWithTax += t_WinMoneyNoWithTax;
                        WinCount2 += WinCount;
                    }
                    else if (Lotterys[ii].StartsWith("[直选和值]"))
                    {
                        WinMoney += ComputeWin_ZhiH(FilterPreFix(Lotterys[ii]), WinNumber, ref Description, ref t_WinMoneyNoWithTax, WinMoney1, WinMoneyNoWithTax1, ref WinCount);

                        WinMoneyNoWithTax += t_WinMoneyNoWithTax;
                        WinCount1 += WinCount;
                    }
                    else if (Lotterys[ii].StartsWith("[组选和值]"))
                    {
                        WinMoney += ComputeWin_ZuH(FilterPreFix(Lotterys[ii]), WinNumber, ref Description, ref t_WinMoneyNoWithTax, WinMoney2, WinMoneyNoWithTax2, WinMoney3, WinMoneyNoWithTax3, ref WinCount_Zu3, ref WinCount_Zu6);

                        WinMoneyNoWithTax += t_WinMoneyNoWithTax;
                        WinCount2 += WinCount_Zu3;
                        WinCount3 += WinCount_Zu6;
                    }
                    else if (Lotterys[ii].StartsWith("[前2单式]") || Lotterys[ii].StartsWith("[前2复式]"))
                    {
                        WinMoney += ComputeWin_Q2(FilterPreFix(Lotterys[ii]), WinNumber, ref Description, ref t_WinMoneyNoWithTax, WinMoney4, WinMoneyNoWithTax4, ref WinCount);

                        WinMoneyNoWithTax += t_WinMoneyNoWithTax;
                        WinCount4 += WinCount;
                    }
                    else if (Lotterys[ii].StartsWith("[后2单式]") || Lotterys[ii].StartsWith("[后2复式]"))
                    {
                        WinMoney += ComputeWin_H2(FilterPreFix(Lotterys[ii]), WinNumber, ref Description, ref t_WinMoneyNoWithTax, WinMoney5, WinMoneyNoWithTax5, ref WinCount);

                        WinMoneyNoWithTax += t_WinMoneyNoWithTax;
                        WinCount5 += WinCount;
                    }
                    else if (Lotterys[ii].StartsWith("[前1单式]") || Lotterys[ii].StartsWith("[前1复式]"))
                    {
                        WinMoney += ComputeWin_Q1(FilterPreFix(Lotterys[ii]), WinNumber, ref Description, ref t_WinMoneyNoWithTax, WinMoney6, WinMoneyNoWithTax6, ref WinCount);

                        WinMoneyNoWithTax += t_WinMoneyNoWithTax;
                        WinCount6 += WinCount;
                    }
                    else if (Lotterys[ii].StartsWith("[后1单式]") || Lotterys[ii].StartsWith("[后1复式]"))
                    {
                        WinMoney += ComputeWin_H1(FilterPreFix(Lotterys[ii]), WinNumber, ref Description, ref t_WinMoneyNoWithTax, WinMoney7, WinMoneyNoWithTax7, ref WinCount);

                        WinMoneyNoWithTax += t_WinMoneyNoWithTax;
                        WinCount7 += WinCount;
                    }
                }

                Description = "";

                if (WinCount1 > 0)
                {
                    MergeWinDescription(ref Description, "单选奖" + WinCount1.ToString() + "注");
                }
                if (WinCount2 > 0)
                {
                    MergeWinDescription(ref Description, "组3奖" + WinCount2.ToString() + "注");
                }
                if (WinCount3 > 0)
                {
                    MergeWinDescription(ref Description, "组6奖" + WinCount3.ToString() + "注");
                }
                if (WinCount4 > 0)
                {
                    MergeWinDescription(ref Description, "前2奖" + WinCount4.ToString() + "注");
                }
                if (WinCount5 > 0)
                {
                    MergeWinDescription(ref Description, "后2奖" + WinCount5.ToString() + "注");
                }
                if (WinCount6 > 0)
                {
                    MergeWinDescription(ref Description, "前1奖" + WinCount6.ToString() + "注");
                }
                if (WinCount7 > 0)
                {
                    MergeWinDescription(ref Description, "后1奖" + WinCount7.ToString() + "注");
                }

                return WinMoney;
            }
            private double ComputeWin_Zhi(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney1, double WinMoneyNoWithTax1, ref int WinCount)	//计算中奖
            {
                WinCount = 0;

                WinNumber = WinNumber.Trim();
                if (WinNumber.Length < 3)	//3: "123"
                    return -1;
                WinNumber = WinNumber.Substring(0, 3);

                string[] Lotterys = SplitLotteryNumber(Number);
                if (Lotterys == null)
                    return -2;
                if (Lotterys.Length < 1)
                    return -2;

                double WinMoney = 0;
                WinMoneyNoWithTax = 0;

                for (int ii = 0; ii < Lotterys.Length; ii++)
                {
                    string t_str = "";
                    string[] Lottery = ToSingle_Zhi(Lotterys[ii], ref t_str);
                    if (Lottery == null)
                        continue;
                    if (Lottery.Length < 1)
                        continue;

                    for (int i = 0; i < Lottery.Length; i++)
                    {
                        if (Lottery[i].Length < 3)
                            continue;

                        if (Lottery[i] == WinNumber)
                        {
                            WinCount++;
                            WinMoney += WinMoney1;
                            WinMoneyNoWithTax += WinMoneyNoWithTax1;
                        }
                    }
                }

                if (WinCount > 0)
                {
                    Description = "直选奖" + WinCount.ToString() + "注。";
                }

                return WinMoney;
            }
            private double ComputeWin_Zu3D_Zu6(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney1, double WinMoneyNoWithTax1, double WinMoney2, double WinMoneyNoWithTax2, ref int WinCount_Zu3, ref int WinCount_Zu6)	//计算中奖
            {
                WinCount_Zu3 = 0;
                WinCount_Zu6 = 0;

                WinNumber = WinNumber.Trim();
                if (WinNumber.Length < 3)	//3: "123"
                    return -1;
                WinNumber = WinNumber.Substring(0, 3);

                string[] Lotterys = SplitLotteryNumber(Number);
                if (Lotterys == null)
                    return -2;
                if (Lotterys.Length < 1)
                    return -2;

                double WinMoney = 0;
                WinMoneyNoWithTax = 0;

                for (int ii = 0; ii < Lotterys.Length; ii++)
                {
                    string t_str = "";

                    if (Lotterys[ii].Length < 3)
                        continue;
                    if (FilterRepeated(Sort(Lotterys[ii])).Length == 2)
                    {
                        if (Sort(Lotterys[ii]) == Sort(WinNumber))
                        {
                            WinCount_Zu3++;
                            WinMoney += WinMoney1;
                            WinMoneyNoWithTax += WinMoneyNoWithTax1;
                        }

                        continue;
                    }

                    string[] Lottery = ToSingle_Zu3D_Zu6(Lotterys[ii], ref t_str);
                    if (Lottery == null)
                        continue;
                    if (Lottery.Length < 1)
                        continue;
                    for (int i = 0; i < Lottery.Length; i++)
                    {
                        if (Lottery[i].Length < 3)
                            continue;

                        if (Sort(Lottery[i]) == Sort(WinNumber))
                        {
                            WinCount_Zu6++;
                            WinMoney += WinMoney2;
                            WinMoneyNoWithTax += WinMoneyNoWithTax2;
                        }
                    }
                }

                Description = "";

                if (WinCount_Zu3 > 0)
                {
                    MergeWinDescription(ref Description, "组选3奖" + WinCount_Zu3.ToString() + "注");
                }

                if (WinCount_Zu6 > 0)
                {
                    MergeWinDescription(ref Description, "组选6奖" + WinCount_Zu6.ToString() + "注");
                }

                if (Description != "")
                {
                    Description += "。";
                }

                return WinMoney;
            }
            private double ComputeWin_Zu3F(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney1, double WinMoneyNoWithTax1, ref int WinCount)	//计算中奖
            {
                WinCount = 0;

                WinNumber = WinNumber.Trim();
                if (WinNumber.Length < 3)	//3: "223"
                    return -1;
                WinNumber = WinNumber.Substring(0, 3);

                string[] Lotterys = SplitLotteryNumber(Number);
                if (Lotterys == null)
                    return -2;
                if (Lotterys.Length < 1)
                    return -2;

                double WinMoney = 0;
                WinMoneyNoWithTax = 0;

                for (int ii = 0; ii < Lotterys.Length; ii++)
                {
                    string t_str = "";
                    string[] Lottery = ToSingle_Zu3F(Lotterys[ii], ref t_str);
                    if (Lottery == null)
                        continue;
                    if (Lottery.Length < 1)
                        continue;

                    for (int i = 0; i < Lottery.Length; i++)
                    {
                        if (Lottery[i].Length < 3)
                            continue;

                        if (Sort(Lottery[i]) == Sort(WinNumber))
                        {
                            WinCount++;
                            WinMoney += WinMoney1;
                            WinMoneyNoWithTax += WinMoneyNoWithTax1;
                        }
                    }
                }

                if (WinCount > 0)
                {
                    Description = "组选3奖" + WinCount.ToString() + "注。";
                }

                return WinMoney;
            }
            private double ComputeWin_ZhiH(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney1, double WinMoneyNoWithTax1, ref int WinCount)	//计算中奖
            {
                WinCount = 0;

                WinNumber = WinNumber.Trim();
                if (WinNumber.Length < 3)	//3: "123"
                    return -1;
                WinNumber = WinNumber.Substring(0, 3);

                string[] Lotterys = SplitLotteryNumber(Number);
                if (Lotterys == null)
                    return -2;
                if (Lotterys.Length < 1)
                    return -2;

                double WinMoney = 0;
                WinMoneyNoWithTax = 0;

                for (int ii = 0; ii < Lotterys.Length; ii++)
                {
                    string t_str = "";
                    string[] Lottery = ToSingle_ZhiH(Lotterys[ii], ref t_str);
                    if (Lottery == null)
                        continue;
                    if (Lottery.Length < 1)
                        continue;

                    for (int i = 0; i < Lottery.Length; i++)
                    {
                        if (Lottery[i].Length < 3)
                            continue;

                        if (Lottery[i] == WinNumber)
                        {
                            WinCount++;
                            WinMoney += WinMoney1;
                            WinMoneyNoWithTax += WinMoneyNoWithTax1;
                        }
                    }
                }

                if (WinCount > 0)
                {
                    Description = "单选奖" + WinCount.ToString() + "注。";
                }

                return WinMoney;
            }
            private double ComputeWin_ZuH(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney1, double WinMoneyNoWithTax1, double WinMoney2, double WinMoneyNoWithTax2, ref int WinCount_Zu3, ref int WinCount_Zu6)	//计算中奖
            {
                WinCount_Zu3 = 0;
                WinCount_Zu6 = 0;

                WinNumber = WinNumber.Trim();
                if (WinNumber.Length < 3)	//3: "123"
                    return -1;
                WinNumber = WinNumber.Substring(0, 3);

                string[] Lotterys = SplitLotteryNumber(Number);
                if (Lotterys == null)
                    return -2;
                if (Lotterys.Length < 1)
                    return -2;

                double WinMoney = 0;
                WinMoneyNoWithTax = 0;

                for (int ii = 0; ii < Lotterys.Length; ii++)
                {
                    string t_str = "";
                    string[] Lottery = ToSingle_ZuH(Lotterys[ii], ref t_str);
                    if (Lottery == null)
                        continue;
                    if (Lottery.Length < 1)
                        continue;

                    for (int i = 0; i < Lottery.Length; i++)
                    {
                        if (Lottery[i].Length < 3)
                            continue;

                        if (Sort(Lottery[i]) == Sort(WinNumber))
                        {
                            if (FilterRepeated(Lottery[i]).Length == 2)
                            {
                                WinCount_Zu3++;
                                WinMoney += WinMoney1;
                                WinMoneyNoWithTax += WinMoneyNoWithTax1;
                            }
                            else
                            {
                                WinCount_Zu6++;
                                WinMoney += WinMoney2;
                                WinMoneyNoWithTax += WinMoneyNoWithTax2;
                            }
                        }
                    }
                }

                Description = "";

                if (WinCount_Zu3 > 0)
                {
                    MergeWinDescription(ref Description, "组选3奖" + WinCount_Zu3.ToString() + "注");
                }

                if (WinCount_Zu6 > 0)
                {
                    MergeWinDescription(ref Description, "组选6奖" + WinCount_Zu6.ToString() + "注");
                }

                if (Description != "")
                {
                    Description += "。";
                }

                return WinMoney;
            }
            private double ComputeWin_Q2(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney1, double WinMoneyNoWithTax1, ref int WinCount)
            {
                WinCount = 0;

                WinNumber = WinNumber.Trim();
                if (WinNumber.Length < 3)	//3: "123"
                    return -1;
                WinNumber = WinNumber.Substring(0, 2);

                string[] Lotterys = SplitLotteryNumber(Number);
                if (Lotterys == null)
                    return -2;
                if (Lotterys.Length < 1)
                    return -2;

                double WinMoney = 0;
                WinMoneyNoWithTax = 0;

                for (int ii = 0; ii < Lotterys.Length; ii++)
                {
                    string t_str = "";
                    string[] Lottery = ToSingle_2(Lotterys[ii], ref t_str);
                    if (Lottery == null)
                        continue;
                    if (Lottery.Length < 1)
                        continue;

                    for (int i = 0; i < Lottery.Length; i++)
                    {
                        if (Lottery[i].Length < 2)
                            continue;

                        if (Lottery[i] == WinNumber)
                        {
                            WinCount++;
                            WinMoney += WinMoney1;
                            WinMoneyNoWithTax += WinMoneyNoWithTax1;
                        }
                    }
                }

                if (WinCount > 0)
                {
                    Description = "前2奖" + WinCount.ToString() + "注。";
                }

                return WinMoney;
            }
            private double ComputeWin_H2(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney1, double WinMoneyNoWithTax1, ref int WinCount)
            {
                WinCount = 0;

                WinNumber = WinNumber.Trim();
                if (WinNumber.Length < 3)	//3: "123"
                    return -1;
                WinNumber = WinNumber.Substring(1, 2);

                string[] Lotterys = SplitLotteryNumber(Number);
                if (Lotterys == null)
                    return -2;
                if (Lotterys.Length < 1)
                    return -2;

                double WinMoney = 0;
                WinMoneyNoWithTax = 0;

                for (int ii = 0; ii < Lotterys.Length; ii++)
                {
                    string t_str = "";
                    string[] Lottery = ToSingle_2(Lotterys[ii], ref t_str);
                    if (Lottery == null)
                        continue;
                    if (Lottery.Length < 1)
                        continue;

                    for (int i = 0; i < Lottery.Length; i++)
                    {
                        if (Lottery[i].Length < 2)
                            continue;

                        if (Lottery[i] == WinNumber)
                        {
                            WinCount++;
                            WinMoney += WinMoney1;
                            WinMoneyNoWithTax += WinMoneyNoWithTax1;
                        }
                    }
                }

                if (WinCount > 0)
                {
                    Description = "后2奖" + WinCount.ToString() + "注。";
                }

                return WinMoney;
            }
            private double ComputeWin_Q1(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney1, double WinMoneyNoWithTax1, ref int WinCount)
            {
                WinCount = 0;

                WinNumber = WinNumber.Trim();
                if (WinNumber.Length < 3)	//3: "123"
                    return -1;
                WinNumber = WinNumber.Substring(0, 1);

                string[] Lotterys = SplitLotteryNumber(Number);
                if (Lotterys == null)
                    return -2;
                if (Lotterys.Length < 1)
                    return -2;

                double WinMoney = 0;
                WinMoneyNoWithTax = 0;

                for (int ii = 0; ii < Lotterys.Length; ii++)
                {
                    string t_str = "";
                    string[] Lottery = ToSingle_1(Lotterys[ii], ref t_str);
                    if (Lottery == null)
                        continue;
                    if (Lottery.Length < 1)
                        continue;

                    for (int i = 0; i < Lottery.Length; i++)
                    {
                        if (Lottery[i].Length < 1)
                            continue;

                        if (Lottery[i] == WinNumber)
                        {
                            WinCount++;
                            WinMoney += WinMoney1;
                            WinMoneyNoWithTax += WinMoneyNoWithTax1;
                        }
                    }
                }

                if (WinCount > 0)
                {
                    Description = "前1奖" + WinCount.ToString() + "注。";
                }

                return WinMoney;
            }
            private double ComputeWin_H1(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney1, double WinMoneyNoWithTax1, ref int WinCount)
            {
                WinCount = 0;

                WinNumber = WinNumber.Trim();
                if (WinNumber.Length < 3)	//3: "123"
                    return -1;
                WinNumber = WinNumber.Substring(2, 1);

                string[] Lotterys = SplitLotteryNumber(Number);
                if (Lotterys == null)
                    return -2;
                if (Lotterys.Length < 1)
                    return -2;

                double WinMoney = 0;
                WinMoneyNoWithTax = 0;

                for (int ii = 0; ii < Lotterys.Length; ii++)
                {
                    string t_str = "";
                    string[] Lottery = ToSingle_1(Lotterys[ii], ref t_str);
                    if (Lottery == null)
                        continue;
                    if (Lottery.Length < 1)
                        continue;

                    for (int i = 0; i < Lottery.Length; i++)
                    {
                        if (Lottery[i].Length < 1)
                            continue;

                        if (Lottery[i] == WinNumber)
                        {
                            WinCount++;
                            WinMoney += WinMoney1;
                            WinMoneyNoWithTax += WinMoneyNoWithTax1;
                        }
                    }
                }

                if (WinCount > 0)
                {
                    Description = "后1奖" + WinCount.ToString() + "注。";
                }

                return WinMoney;
            }
            #endregion

            public override string AnalyseScheme(string Content, int PlayType)
            {
                if (PlayType == PlayType_Mixed)   //混合投注
                    return AnalyseScheme_Mixed(Content, PlayType);

                if ((PlayType == PlayType_ZhiD) || (PlayType == PlayType_ZhiF))
                    return AnalyseScheme_Zhi(Content, PlayType);

                if ((PlayType == PlayType_ZuD) || (PlayType == PlayType_Zu6F))
                    return AnalyseScheme_Zu3D_Zu6(Content, PlayType);

                if (PlayType == PlayType_Zu3F)
                    return AnalyseScheme_Zu3F(Content, PlayType);

                if (PlayType == PlayType_ZhiH)
                    return AnalyseScheme_ZhiH(Content, PlayType);

                if (PlayType == PlayType_ZuH)
                    return AnalyseScheme_ZuH(Content, PlayType);

                if ((PlayType == PlayType_Q2D) || (PlayType == PlayType_Q2F) || (PlayType == PlayType_H2D) || (PlayType == PlayType_H2F))
                    return AnalyseScheme_2(Content, PlayType);

                if ((PlayType == PlayType_Q1D) || (PlayType == PlayType_Q1F) || (PlayType == PlayType_H1D) || (PlayType == PlayType_H1F))
                    return AnalyseScheme_1(Content, PlayType);

                return "";
            }
            #region AnalyseScheme 的具体方法
            private string AnalyseScheme_Mixed(string Content, int PlayType)
            {
                string[] Lotterys = SplitLotteryNumber(Content);

                if (Lotterys == null)
                {
                    return "";
                }

                if (Lotterys.Length < 1)
                {
                    return "";
                }

                string Result = "";

                for (int ii = 0; ii < Lotterys.Length; ii++)
                {
                    string PreFix = GetLotteryNumberPreFix(Lotterys[ii]);
                    string t_Result = "";

                    if (Lotterys[ii].StartsWith("[直选单式]"))
                    {
                        t_Result = AnalyseScheme_Zhi(Lotterys[ii], PlayType_ZhiD);
                    }

                    if (Lotterys[ii].StartsWith("[直选复式]"))
                    {
                        t_Result = AnalyseScheme_Zhi(Lotterys[ii], PlayType_ZhiF);
                    }

                    if (Lotterys[ii].StartsWith("[组选单式]"))
                    {
                        t_Result = AnalyseScheme_Zu3D_Zu6(Lotterys[ii], PlayType_ZuD);
                    }

                    if (Lotterys[ii].StartsWith("[组选6复式]"))
                    {
                        t_Result = AnalyseScheme_Zu3D_Zu6(Lotterys[ii], PlayType_Zu6F);
                    }

                    if (Lotterys[ii].StartsWith("[组选3复式]"))
                    {
                        t_Result = AnalyseScheme_Zu3F(Lotterys[ii], PlayType_Zu3F);
                    }

                    if (Lotterys[ii].StartsWith("[直选和值]"))
                    {
                        t_Result = AnalyseScheme_ZhiH(Lotterys[ii], PlayType_ZhiH);
                    }

                    if (Lotterys[ii].StartsWith("[组选和值]"))
                    {
                        t_Result = AnalyseScheme_ZuH(Lotterys[ii], PlayType_ZuH);
                    }

                    if (Lotterys[ii].StartsWith("[前2单式]"))
                    {
                        t_Result = AnalyseScheme_2(Lotterys[ii], PlayType_Q2D);
                    }

                    if (Lotterys[ii].StartsWith("[前2复式]"))
                    {
                        t_Result = AnalyseScheme_2(Lotterys[ii], PlayType_Q2F);
                    }

                    if (Lotterys[ii].StartsWith("[后2单式]"))
                    {
                        t_Result = AnalyseScheme_2(Lotterys[ii], PlayType_H2D);
                    }

                    if (Lotterys[ii].StartsWith("[后2复式]"))
                    {
                        t_Result = AnalyseScheme_2(Lotterys[ii], PlayType_H2F);
                    }

                    if (Lotterys[ii].StartsWith("[前1单式]"))
                    {
                        t_Result = AnalyseScheme_1(Lotterys[ii], PlayType_Q1D);
                    }

                    if (Lotterys[ii].StartsWith("[前1复式]"))
                    {
                        t_Result = AnalyseScheme_1(Lotterys[ii], PlayType_Q1F);
                    }

                    if (Lotterys[ii].StartsWith("[后1单式]"))
                    {
                        t_Result = AnalyseScheme_1(Lotterys[ii], PlayType_H1D);
                    }

                    if (Lotterys[ii].StartsWith("[后1复式]"))
                    {
                        t_Result = AnalyseScheme_1(Lotterys[ii], PlayType_H1F);
                    }

                    if (t_Result != "")
                    {
                        Result += PreFix + t_Result + "\n";
                    }
                }

                if (Result.EndsWith("\n"))
                {
                    Result = Result.Substring(0, Result.Length - 1);
                }

                return Result;
            }
            private string AnalyseScheme_Zhi(string Content, int PlayType)
            {
                string[] strs = Content.Split('\n');
                if (strs == null)
                    return "";
                if (strs.Length == 0)
                    return "";

                string Result = "";

                string RegexString;
                if (PlayType == PlayType_ZhiD)
                    RegexString = @"^([\d]){3}";
                else
                    RegexString = @"^(([\d])|([(][\d]{1,10}[)])){3}";
                Regex regex = new Regex(RegexString, RegexOptions.Compiled | RegexOptions.IgnoreCase);

                for (int i = 0; i < strs.Length; i++)
                {
                    Match m = regex.Match(strs[i]);
                    if (m.Success)
                    {
                        string CanonicalNumber = "";
                        string[] singles = ToSingle_Zhi(m.Value, ref CanonicalNumber);
                        if (singles == null)
                            continue;
                        if (singles.Length >= ((PlayType == PlayType_ZhiD) ? 1 : 2))
                            Result += CanonicalNumber + "|" + singles.Length.ToString() + "\n";
                    }
                }

                if (Result.EndsWith("\n"))
                    Result = Result.Substring(0, Result.Length - 1);
                return Result;
            }
            private string AnalyseScheme_Zu3D_Zu6(string Content, int PlayType)
            {
                string[] strs = Content.Split('\n');
                if (strs == null)
                    return "";
                if (strs.Length == 0)
                    return "";

                string Result = "";

                string RegexString;
                if (PlayType == PlayType_ZuD)
                    RegexString = @"^([\d]){3}";
                else
                    RegexString = @"^([\d]){3,10}";
                Regex regex = new Regex(RegexString, RegexOptions.Compiled | RegexOptions.IgnoreCase);

                for (int i = 0; i < strs.Length; i++)
                {
                    Match m = regex.Match(strs[i]);
                    if (m.Success)
                    {
                        string CanonicalNumber = "";
                        string[] singles = ToSingle_Zu3D_Zu6(m.Value, ref CanonicalNumber);
                        if (singles == null)
                            continue;
                        if (singles.Length >= ((PlayType == PlayType_ZuD) ? 1 : 2))
                        {
                            if (FilterRepeated(Sort(m.Value)).Length == 2)
                            {
                                if (PlayType != PlayType_Zu6F)
                                {
                                    Result += m.Value + "|1\n";
                                }
                            }
                            else
                            {
                                Result += CanonicalNumber + "|" + singles.Length.ToString() + "\n";
                            }
                        }
                    }
                }

                if (Result.EndsWith("\n"))
                    Result = Result.Substring(0, Result.Length - 1);
                return Result;
            }
            private string AnalyseScheme_Zu3F(string Content, int PlayType)
            {
                string[] strs = Content.Split('\n');
                if (strs == null)
                    return "";
                if (strs.Length == 0)
                    return "";

                string Result = "";
                Regex regex = new Regex(@"^([\d]){2,}", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                for (int i = 0; i < strs.Length; i++)
                {
                    Match m = regex.Match(strs[i]);
                    if (m.Success)
                    {
                        string CanonicalNumber = "";
                        string[] singles = ToSingle_Zu3F(m.Value, ref CanonicalNumber);
                        if (singles == null)
                            continue;
                        if (singles.Length >= 2)
                            Result += CanonicalNumber + "|" + singles.Length.ToString() + "\n";
                    }
                }

                if (Result.EndsWith("\n"))
                    Result = Result.Substring(0, Result.Length - 1);
                return Result;
            }
            private string AnalyseScheme_ZhiH(string Content, int PlayType)
            {
                string[] strs = Content.Split('\n');
                if (strs == null)
                    return "";
                if (strs.Length == 0)
                    return "";

                string Result = "";
                Regex regex = new Regex(@"^([\d]){1,2}", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                for (int i = 0; i < strs.Length; i++)
                {
                    Match m = regex.Match(strs[i]);
                    if (m.Success)
                    {
                        string CanonicalNumber = "";
                        string[] singles = ToSingle_ZhiH(m.Value, ref CanonicalNumber);
                        if (singles == null)
                            continue;
                        if (singles.Length >= 1)
                            Result += CanonicalNumber + "|" + singles.Length.ToString() + "\n";
                    }
                }

                if (Result.EndsWith("\n"))
                    Result = Result.Substring(0, Result.Length - 1);
                return Result;
            }
            private string AnalyseScheme_ZuH(string Content, int PlayType)
            {
                string[] strs = Content.Split('\n');
                if (strs == null)
                    return "";
                if (strs.Length == 0)
                    return "";

                string Result = "";
                Regex regex = new Regex(@"^([\d]){1,2}", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                for (int i = 0; i < strs.Length; i++)
                {
                    Match m = regex.Match(strs[i]);
                    if (m.Success)
                    {
                        string CanonicalNumber = "";
                        string[] singles = ToSingle_ZuH(m.Value, ref CanonicalNumber);
                        if (singles == null)
                            continue;
                        if (singles.Length >= 1)
                            Result += CanonicalNumber + "|" + singles.Length.ToString() + "\n";
                    }
                }

                if (Result.EndsWith("\n"))
                    Result = Result.Substring(0, Result.Length - 1);
                return Result;
            }
            private string AnalyseScheme_2(string Content, int PlayType)
            {
                string[] strs = Content.Split('\n');
                if (strs == null)
                    return "";
                if (strs.Length == 0)
                    return "";

                string Result = "";

                string RegexString;
                if ((PlayType == PlayType_Q2D) || (PlayType == PlayType_H2D))
                    RegexString = @"^([\d]){2}";
                else
                    RegexString = @"^(([\d])|([(][\d]{1,10}[)])){2}";
                Regex regex = new Regex(RegexString, RegexOptions.Compiled | RegexOptions.IgnoreCase);

                for (int i = 0; i < strs.Length; i++)
                {
                    Match m = regex.Match(strs[i]);
                    if (m.Success)
                    {
                        string CanonicalNumber = "";
                        string[] singles = ToSingle_2(m.Value, ref CanonicalNumber);
                        if (singles == null)
                            continue;
                        if (singles.Length >= (((PlayType == PlayType_Q2D) || (PlayType == PlayType_H2D)) ? 1 : 2))
                            Result += CanonicalNumber + "|" + singles.Length.ToString() + "\n";
                    }
                }

                if (Result.EndsWith("\n"))
                    Result = Result.Substring(0, Result.Length - 1);
                return Result;
            }
            private string AnalyseScheme_1(string Content, int PlayType)
            {
                string[] strs = Content.Split('\n');
                if (strs == null)
                    return "";
                if (strs.Length == 0)
                    return "";

                string Result = "";

                string RegexString;
                if ((PlayType == PlayType_Q1D) || (PlayType == PlayType_H1D))
                    RegexString = @"^([\d]){1}";
                else
                    RegexString = @"^([\d]){1,10}";
                Regex regex = new Regex(RegexString, RegexOptions.Compiled | RegexOptions.IgnoreCase);

                for (int i = 0; i < strs.Length; i++)
                {
                    Match m = regex.Match(strs[i]);
                    if (m.Success)
                    {
                        string CanonicalNumber = "";
                        string[] singles = ToSingle_1(m.Value, ref CanonicalNumber);
                        if (singles == null)
                            continue;
                        if (singles.Length >= (((PlayType == PlayType_Q1D) || (PlayType == PlayType_H1D)) ? 1 : 2))
                            Result += CanonicalNumber + "|" + singles.Length.ToString() + "\n";
                    }
                }

                if (Result.EndsWith("\n"))
                    Result = Result.Substring(0, Result.Length - 1);
                return Result;
            }
            #endregion

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

            public override bool AnalyseWinNumber(string Number)
            {
                string t_str = "";
                string[] WinLotteryNumber = ToSingle_Zhi(Number, ref t_str);

                if ((WinLotteryNumber == null) || (WinLotteryNumber.Length != 1))
                    return false;

                return true;
            }

            private string FilterRepeated(string NumberPart)
            {
                string Result = "";
                for (int i = 0; i < NumberPart.Length; i++)
                {
                    if ((Result.IndexOf(NumberPart.Substring(i, 1)) == -1) && ("-0123456789".IndexOf(NumberPart.Substring(i, 1)) >= 0))
                        Result += NumberPart.Substring(i, 1);
                }
                return Sort(Result);
            }

            public override string ShowNumber(string Number)
            {
                return base.ShowNumber(Number, " ");
            }

            //参数说明 PlayTypeID:玩法类别; Number:投注号码; Multiple: 倍数; MaxMultiple:最大倍数;
            public override Ticket[] ToElectronicTicket_HPSH(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                if (PlayTypeID == PlayType_ZhiD)
                {
                    return ToElectronicTicket_HPSH_D(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                else if (PlayTypeID == PlayType_ZhiF)
                {
                    return ToElectronicTicket_HPSH_F(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                else if (PlayTypeID == PlayType_ZuD)
                {
                    return ToElectronicTicket_HPSH_ZuD(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                else if (PlayTypeID == PlayType_ZhiH)
                {
                    return ToElectronicTicket_HPSH_ZhiH(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                else if (PlayTypeID == PlayType_ZuH)
                {
                    return ToElectronicTicket_HPSH_ZuH(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                else if (PlayTypeID == PlayType_Zu6F)
                {
                    return ToElectronicTicket_HPSH_Zu6F(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                else if (PlayTypeID == PlayType_Zu3F)
                {
                    return ToElectronicTicket_HPSH_Zu3F(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                else if (PlayTypeID == PlayType_Q2D || PlayTypeID == PlayType_H2D)
                {
                    return ToElectronicTicket_HPSH_2D(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                else if (PlayTypeID == PlayType_Q2F || PlayTypeID == PlayType_H2F)
                {
                    return ToElectronicTicket_HPSH_2F(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                else if (PlayTypeID == PlayType_Q1D || PlayTypeID == PlayType_H1D)
                {
                    return ToElectronicTicket_HPSH_1D(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                else if (PlayTypeID == PlayType_Q1F || PlayTypeID == PlayType_H1F)
                {
                    return ToElectronicTicket_HPSH_1F(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                return null;
            }
            #region ToElectronicTicket_HPSH 的具体方法
            private Ticket[] ToElectronicTicket_HPSH_D(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
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

                        al.Add(new Ticket(201, ConvertFormatToElectronTicket_HPSH_D(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));
                    }
                }

                Ticket[] Tickets = new Ticket[al.Count];

                for (int i = 0; i < Tickets.Length; i++)
                {
                    Tickets[i] = (Ticket)al[i];
                }

                return Tickets;

            }
            private Ticket[] ToElectronicTicket_HPSH_F(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
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

                        al.Add(new Ticket(208, ConvertFormatToElectronTicket_HPSH_F(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));
                    }
                }

                Ticket[] Tickets = new Ticket[al.Count];

                for (int i = 0; i < Tickets.Length; i++)
                {
                    Tickets[i] = (Ticket)al[i];
                }

                return Tickets;
            }
            private Ticket[] ToElectronicTicket_HPSH_ZuD(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
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

                        al.Add(new Ticket(202, ConvertFormatToElectronTicket_HPSH_D(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));
                    }
                }

                Ticket[] Tickets = new Ticket[al.Count];

                for (int i = 0; i < Tickets.Length; i++)
                {
                    Tickets[i] = (Ticket)al[i];
                }

                return Tickets;
            }
            private Ticket[] ToElectronicTicket_HPSH_ZhiH(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                string[] strs = AnalyseSchemeToElectronicTicket_ZhiH(Number, PlayTypeID).Split('\n');

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

                        al.Add(new Ticket(201, ConvertFormatToElectronTicket_HPSH_D(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));
                    }
                }

                Ticket[] Tickets = new Ticket[al.Count];

                for (int i = 0; i < Tickets.Length; i++)
                {
                    Tickets[i] = (Ticket)al[i];
                }

                return Tickets;
            }
            private Ticket[] ToElectronicTicket_HPSH_ZuH(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                string[] strs = AnalyseSchemeToElectronicTicket_ZuH(Number, PlayTypeID).Split('\n');

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

                        al.Add(new Ticket(202, ConvertFormatToElectronTicket_HPSH_D(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));
                    }
                }

                Ticket[] Tickets = new Ticket[al.Count];

                for (int i = 0; i < Tickets.Length; i++)
                {
                    Tickets[i] = (Ticket)al[i];
                }

                return Tickets;
            }
            private Ticket[] ToElectronicTicket_HPSH_Zu6F(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                string[] strs = AnalyseSchemeToElectronTicket_Zu6F(Number, PlayTypeID).Split('\n');

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

                        al.Add(new Ticket(202, ConvertFormatToElectronTicket_HPSH_D(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));
                    }
                }

                Ticket[] Tickets = new Ticket[al.Count];

                for (int i = 0; i < Tickets.Length; i++)
                {
                    Tickets[i] = (Ticket)al[i];
                }

                return Tickets;
            }
            private Ticket[] ToElectronicTicket_HPSH_Zu3F(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                string[] strs = AnalyseSchemeToElectronTicket_Zu3F(Number, PlayTypeID).Split('\n');

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

                        al.Add(new Ticket(202, ConvertFormatToElectronTicket_HPSH_D(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));
                    }
                }

                Ticket[] Tickets = new Ticket[al.Count];

                for (int i = 0; i < Tickets.Length; i++)
                {
                    Tickets[i] = (Ticket)al[i];
                }

                return Tickets;
            }
            private Ticket[] ToElectronicTicket_HPSH_2D(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                string[] strs = AnalyseSchemeToElectronTicket_2D(Number, PlayTypeID).Split('\n');

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

                        al.Add(new Ticket(201, ConvertFormatToElectronTicket_HPSH_2D(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));
                    }
                }

                Ticket[] Tickets = new Ticket[al.Count];

                for (int i = 0; i < Tickets.Length; i++)
                {
                    Tickets[i] = (Ticket)al[i];
                }

                return Tickets;
            }
            private Ticket[] ToElectronicTicket_HPSH_2F(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                string[] strs = AnalyseSchemeToElectronTicket_2F(Number, PlayTypeID).Split('\n');

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

                        al.Add(new Ticket(208, ConvertFormatToElectronTicket_HPSH_2F(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));
                    }
                }

                Ticket[] Tickets = new Ticket[al.Count];

                for (int i = 0; i < Tickets.Length; i++)
                {
                    Tickets[i] = (Ticket)al[i];
                }

                return Tickets;
            }
            private Ticket[] ToElectronicTicket_HPSH_1D(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                string[] strs = AnalyseScheme_1(Number, PlayTypeID).Split('\n');

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

                        al.Add(new Ticket(201, ConvertFormatToElectronTicket_HPSH_1(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));
                    }
                }

                Ticket[] Tickets = new Ticket[al.Count];

                for (int i = 0; i < Tickets.Length; i++)
                {
                    Tickets[i] = (Ticket)al[i];
                }

                return Tickets;
            }
            private Ticket[] ToElectronicTicket_HPSH_1F(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                string[] strs = AnalyseScheme_1(Number, PlayTypeID).Split('\n');

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

                        al.Add(new Ticket(208, ConvertFormatToElectronTicket_HPSH_1(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));
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
            private string ConvertFormatToElectronTicket_HPSH_D(int PlayTypeID, string Number)
            {
                Number = Number.Trim();

                string Ticket = "";

                if (PlayTypeID == PlayType_ZhiD || PlayTypeID == PlayType_ZhiH || PlayTypeID == PlayType_Zu6F || PlayTypeID == PlayType_Zu3F || PlayTypeID == PlayType_ZuH || PlayTypeID == PlayType_ZuD)
                {
                    string[] strs = Number.Split('\n');

                    for (int j = 0; j < strs.Length; j++)
                    {
                        for (int i = 0; i < strs[j].Length; i++)
                        {
                            Ticket += strs[j].Substring(i, 1) + ",";
                        }

                        if (Ticket.EndsWith(","))
                            Ticket = Ticket.Substring(0, Ticket.Length - 1);

                        Ticket += "\n";
                    }

                    if (Ticket.EndsWith("\n"))
                        Ticket = Ticket.Substring(0, Ticket.Length - 1);
                }

                if (Ticket.EndsWith(","))
                    Ticket = Ticket.Substring(0, Ticket.Length - 1);

                Ticket = Ticket.Replace("-", "_");

                return Ticket;
            }
            private string ConvertFormatToElectronTicket_HPSH_F(int PlayTypeID, string Number)
            {
                Number = Number.Trim();

                string[] Locate = new string[3];

                string Ticket = "";

                if (PlayTypeID == PlayType_ZhiF)
                {
                    Regex regex = new Regex(@"^(?<L0>(\d)|([(][\d]+?[)]))(?<L1>(\d)|([(][\d]+?[)]))(?<L2>(\d)|([(][\d]+?[)]))", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                    Match m = regex.Match(Number);
                    for (int i = 0; i < 3; i++)
                    {
                        Locate[i] = m.Groups["L" + i.ToString()].ToString().Trim();
                        if (Locate[i] == "")
                        {
                            return "";
                        }
                        if (Locate[i].Length > 1)
                        {
                            Locate[i] = Locate[i].Substring(1, Locate[i].Length - 2);
                            if (Locate[i].Length > 1)
                                Locate[i] = FilterRepeated(Locate[i]);
                            if (Locate[i] == "")
                            {
                                return "";
                            }
                        }

                        Ticket += Locate[i] + ",";
                    }
                }

                if (Ticket.EndsWith(","))
                    Ticket = Ticket.Substring(0, Ticket.Length - 1);

                Ticket = Ticket.Replace("-", "_");

                return Ticket;
            }
            private string ConvertFormatToElectronTicket_HPSH_2D(int PlayTypeID, string Number)
            {
                Number = Number.Trim();

                string Ticket = "";

                if (PlayTypeID == PlayType_Q2D)
                {
                    string[] strs = Number.Split('\n');

                    for (int j = 0; j < strs.Length; j++)
                    {
                        for (int i = 0; i < strs[j].Length; i++)
                        {
                            Ticket += strs[j].Substring(i, 1) + ",";
                        }

                        Ticket += "-\n";

                    }

                    if (Ticket.EndsWith("\n"))
                        Ticket = Ticket.Substring(0, Ticket.Length - 1);
                }
                else
                {
                    string[] strs = Number.Split('\n');

                    for (int j = 0; j < strs.Length; j++)
                    {
                        Ticket += "-,";

                        for (int i = 0; i < strs[j].Length; i++)
                        {
                            Ticket += strs[j].Substring(i, 1) + ",";
                        }

                        if (Ticket.EndsWith(","))
                            Ticket = Ticket.Substring(0, Ticket.Length - 1);

                        Ticket += "\n";

                    }

                    if (Ticket.EndsWith("\n"))
                        Ticket = Ticket.Substring(0, Ticket.Length - 1);
                }

                if (Ticket.EndsWith(","))
                    Ticket = Ticket.Substring(0, Ticket.Length - 1);

                Ticket = Ticket.Replace("-", "_");

                return Ticket;
            }
            private string ConvertFormatToElectronTicket_HPSH_2F(int PlayTypeID, string Number)
            {
                Number = Number.Trim();

                string[] Locate = new string[2];

                string Ticket = "";

                if (PlayTypeID == PlayType_H2F)
                {
                    Ticket = "-,";
                }

                Regex regex = new Regex(@"^(?<L0>(\d)|([(][\d]+?[)]))(?<L1>(\d)|([(][\d]+?[)]))", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                Match m = regex.Match(Number);
                for (int i = 0; i < 2; i++)
                {
                    Locate[i] = m.Groups["L" + i.ToString()].ToString().Trim();
                    if (Locate[i] == "")
                    {
                        return "";
                    }
                    if (Locate[i].Length > 1)
                    {
                        Locate[i] = Locate[i].Substring(1, Locate[i].Length - 2);
                        if (Locate[i].Length > 1)
                            Locate[i] = FilterRepeated(Locate[i]);
                        if (Locate[i] == "")
                        {
                            return "";
                        }
                    }

                    Ticket += Locate[i] + ",";
                }

                if (PlayTypeID == PlayType_Q2F)
                {
                    Ticket += "-,";
                }

                if (Ticket.EndsWith(","))
                    Ticket = Ticket.Substring(0, Ticket.Length - 1);

                Ticket = Ticket.Replace("-", "_");

                return Ticket;
            }
            private string ConvertFormatToElectronTicket_HPSH_1(int PlayTypeID, string Number)
            {
                string Ticket = "";

                if (Number.EndsWith("\n"))
                    Number = Number.Substring(0, Number.Length - 1);

                string[] strs = Number.Split('\n');

                for (int j = 0; j < strs.Length; j++)
                {
                    if (PlayTypeID == PlayType_H1D || PlayTypeID == PlayType_H1F)
                    {
                        Ticket += "-,-,";
                    }

                    Ticket += strs[j].ToString();

                    if (PlayTypeID == PlayType_Q1D || PlayTypeID == PlayType_Q1F)
                    {
                        Ticket += ",-,-";
                    }

                    Ticket += "\n";
                }

                if (Ticket.EndsWith("\n"))
                    Ticket = Ticket.Substring(0, Ticket.Length - 1);

                Ticket = Ticket.Replace("-", "_");

                return Ticket;
            }

            private string AnalyseSchemeToElectronicTicket_ZhiH(string Content, int PlayType)
            {
                string[] strs = Content.Split('\n');
                if (strs == null)
                    return "";
                if (strs.Length == 0)
                    return "";

                string Result = "";
                Regex regex = new Regex(@"^([\d]){1,2}", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                for (int i = 0; i < strs.Length; i++)
                {
                    Match m = regex.Match(strs[i]);
                    if (m.Success)
                    {
                        string CanonicalNumber = "";
                        string[] singles = ToSingle_ZhiH(m.Value, ref CanonicalNumber);
                        if (singles == null)
                            continue;
                        if (singles.Length >= 1)
                        {
                            for (int j = 0; j < singles.Length; j++)
                            {
                                Result += singles[j] + "|1\n";
                            }
                        }   
                    }
                }

                if (Result.EndsWith("\n"))
                    Result = Result.Substring(0, Result.Length - 1);
                return Result;
            }
            private string AnalyseSchemeToElectronicTicket_ZuH(string Content, int PlayType)
            {
                string[] strs = Content.Split('\n');
                if (strs == null)
                    return "";
                if (strs.Length == 0)
                    return "";

                string Result = "";
                Regex regex = new Regex(@"^([\d]){1,2}", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                for (int i = 0; i < strs.Length; i++)
                {
                    Match m = regex.Match(strs[i]);
                    if (m.Success)
                    {
                        string CanonicalNumber = "";
                        string[] singles = ToSingle_ZuH(m.Value, ref CanonicalNumber);
                        if (singles == null)
                            continue;
                        if (singles.Length >= 1)
                        {
                            for (int j = 0; j < singles.Length; j++)
                            {
                                Result += singles[j] + "|1\n";
                            }
                        }
                    }
                }

                if (Result.EndsWith("\n"))
                    Result = Result.Substring(0, Result.Length - 1);
                return Result;
            }
            private string AnalyseSchemeToElectronTicket_Zu6F(string Content, int PlayType)
            {
                string[] strs = Content.Split('\n');
                if (strs == null)
                    return "";
                if (strs.Length == 0)
                    return "";

                string Result = "";

                string RegexString;
                if (PlayType == PlayType_ZuD)
                    RegexString = @"^([\d]){3}";
                else
                    RegexString = @"^([\d]){3,10}";
                Regex regex = new Regex(RegexString, RegexOptions.Compiled | RegexOptions.IgnoreCase);

                for (int i = 0; i < strs.Length; i++)
                {
                    Match m = regex.Match(strs[i]);
                    if (m.Success)
                    {
                        string CanonicalNumber = "";
                        string[] singles = ToSingle_Zu3D_Zu6(m.Value, ref CanonicalNumber);
                        if (singles == null)
                            continue;
                        if (singles.Length >= ((PlayType == PlayType_ZuD) ? 1 : 2))
                        {
                            for (int j = 0; j < singles.Length; j++)
                            {
                                Result += singles[j] + "|1\n";
                            }
                        }
                    }
                }

                if (Result.EndsWith("\n"))
                    Result = Result.Substring(0, Result.Length - 1);
                return Result;
            }
            private string AnalyseSchemeToElectronTicket_Zu3F(string Content, int PlayType)
            {
                string[] strs = Content.Split('\n');
                if (strs == null)
                    return "";
                if (strs.Length == 0)
                    return "";

                string Result = "";
                Regex regex = new Regex(@"^([\d]){2,}", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                for (int i = 0; i < strs.Length; i++)
                {
                    Match m = regex.Match(strs[i]);
                    if (m.Success)
                    {
                        string CanonicalNumber = "";
                        string[] singles = ToSingle_Zu3F(m.Value, ref CanonicalNumber);
                        if (singles == null)
                            continue;
                        if (singles.Length >= 2)
                        {
                            for (int j = 0; j < singles.Length; j++)
                            {
                                Result += singles[j] + "|1\n";
                            }
                        }
                    }
                }

                if (Result.EndsWith("\n"))
                    Result = Result.Substring(0, Result.Length - 1);
                return Result;
            }
            private string AnalyseSchemeToElectronTicket_2D(string Content, int PlayType)
            {
                string[] strs = Content.Split('\n');
                if (strs == null)
                    return "";
                if (strs.Length == 0)
                    return "";

                string Result = "";

                string RegexString;
                if ((PlayType == PlayType_Q2D) || (PlayType == PlayType_H2D))
                    RegexString = @"^([\d]){2}";
                else
                    RegexString = @"^(([\d])|([(][\d]{1,10}[)])){2}";
                Regex regex = new Regex(RegexString, RegexOptions.Compiled | RegexOptions.IgnoreCase);

                for (int i = 0; i < strs.Length; i++)
                {
                    Match m = regex.Match(strs[i]);
                    if (m.Success)
                    {
                        string CanonicalNumber = "";
                        string[] singles = ToSingle_2(m.Value, ref CanonicalNumber);
                        if (singles == null)
                            continue;

                        for (int j = 0; j < singles.Length; j++)
                        {
                            Result += singles[j] + "|1\n";
                        }
                    }
                }

                if (Result.EndsWith("\n"))
                    Result = Result.Substring(0, Result.Length - 1);
                return Result;
            }
            private string AnalyseSchemeToElectronTicket_2F(string Content, int PlayType)
            {
                string[] strs = Content.Split('\n');
                if (strs == null)
                    return "";
                if (strs.Length == 0)
                    return "";

                string Result = "";

                string RegexString;
                if ((PlayType == PlayType_Q2D) || (PlayType == PlayType_H2D))
                    RegexString = @"^([\d]){2}";
                else
                    RegexString = @"^(([\d])|([(][\d]{1,10}[)])){2}";
                Regex regex = new Regex(RegexString, RegexOptions.Compiled | RegexOptions.IgnoreCase);

                for (int i = 0; i < strs.Length; i++)
                {
                    Match m = regex.Match(strs[i]);
                    if (m.Success)
                    {
                        string CanonicalNumber = "";
                        string[] singles = ToSingle_2(m.Value, ref CanonicalNumber);
                        if (singles == null)
                            continue;

                        Result += CanonicalNumber + "|" + singles.Length.ToString() + "\n";
                    }
                }

                if (Result.EndsWith("\n"))
                    Result = Result.Substring(0, Result.Length - 1);
                return Result;
            }

            public override string HPSH_ToElectronicTicket(int PlayTypeID, string Number, ref string TicketNumber, ref int NewPlayTypeID)
            {
                if ((PlayTypeID == PlayType_ZhiD) || (PlayTypeID == PlayType_ZhiH))
                {
                    return HPSH_ToElectronicTicket_D(PlayTypeID, Number, ref TicketNumber, ref NewPlayTypeID);
                }
                else if (PlayTypeID == PlayType_ZhiF)
                {
                    return HPSH_ToElectronicTicket_F(PlayTypeID, Number, ref TicketNumber, ref NewPlayTypeID);
                }
                else if ((PlayTypeID == PlayType_ZuD) || (PlayTypeID == PlayType_ZuH) || (PlayTypeID == PlayType_Zu6F) || (PlayTypeID == PlayType_Zu3F))
                {
                    return HPSH_ToElectronicTicket_ZuD(PlayTypeID, Number, ref TicketNumber, ref NewPlayTypeID);
                }
                else if ((PlayTypeID == PlayType_Q2D || PlayTypeID == PlayType_H2D) || (PlayTypeID == PlayType_Q1D || PlayTypeID == PlayType_H1D) || (PlayTypeID == PlayType_Q2F || PlayTypeID == PlayType_H2F))
                {
                    return HPSH_ToElectronicTicket_2D(PlayTypeID, Number, ref TicketNumber, ref NewPlayTypeID);
                }
                else if (PlayTypeID == PlayType_Q1F || PlayTypeID == PlayType_H1F)
                {
                    return HPSH_ToElectronicTicket_1F(PlayTypeID, Number, ref TicketNumber, ref NewPlayTypeID);
                }

                return "";
            }

            private string HPSH_ToElectronicTicket_D(int PlayTypeID, string Number, ref string TicketNumber, ref int NewPlayTypeID)
            {
                string t_TicketNumber = "";

                TicketNumber = "";

                t_TicketNumber = HPSH_ConvertFormatToElectronTicket(PlayTypeID, Number);

                string[] strs = t_TicketNumber.Split('\n');

                foreach (string str in strs)
                {
                    TicketNumber = TicketNumber + str + "\n";
                }

                NewPlayTypeID = PlayType_ZhiD;

                return "";

            }

            private string HPSH_ToElectronicTicket_F(int PlayTypeID, string Number, ref string TicketNumber, ref int NewPlayTypeID)
            {
                string t_TicketNumber = "";

                TicketNumber = "";

                t_TicketNumber = HPSH_ConvertFormatToElectronTicket(PlayTypeID, Number);

                string[] strs = t_TicketNumber.Split('\n');

                foreach (string str in strs)
                {
                    TicketNumber = TicketNumber + str + "\n";
                }

                NewPlayTypeID = PlayType_ZhiF;

                return "";

            }

            private string HPSH_ToElectronicTicket_ZuD(int PlayTypeID, string Number, ref string TicketNumber, ref int NewPlayTypeID)
            {
                string t_TicketNumber = "";

                TicketNumber = "";

                t_TicketNumber = HPSH_ConvertFormatToElectronTicket(PlayTypeID, Number);

                string[] strs = t_TicketNumber.Split('\n');

                foreach (string str in strs)
                {
                    TicketNumber = TicketNumber + str + "\n";
                }

                NewPlayTypeID = PlayType_ZuD;

                return "";

            }

            private string HPSH_ToElectronicTicket_2D(int PlayTypeID, string Number, ref string TicketNumber, ref int NewPlayTypeID)
            {
                string t_TicketNumber = "";

                TicketNumber = "";

                t_TicketNumber = HPSH_ConvertFormatToElectronTicket(PlayTypeID, Number);

                string[] strs = t_TicketNumber.Split('\n');

                foreach (string str in strs)
                {
                    TicketNumber = TicketNumber + str + "\n";
                }

                NewPlayTypeID = PlayTypeID;

                return "";

            }

            private string HPSH_ToElectronicTicket_1F(int PlayTypeID, string Number, ref string TicketNumber, ref int NewPlayTypeID)
            {
                string t_TicketNumber = "";

                TicketNumber = "";

                t_TicketNumber = HPSH_ConvertFormatToElectronTicket_1F(PlayTypeID, Number);

                string[] strs = t_TicketNumber.Split('\n');

                foreach (string str in strs)
                {
                    TicketNumber = TicketNumber + str + "\n";
                }

                NewPlayTypeID = PlayTypeID;

                return "";

            }

            private string HPSH_ConvertFormatToElectronTicket(int PlayTypeID, string Number)
            {
                Number = Number.Trim();

                string Ticket = "";

                string[] strs = Number.Split(',');

                foreach (string str in strs)
                {
                    if (str.Length > 1)
                    {
                        Ticket += "(" + str + ")";

                        continue;
                    }

                    if (str == "_")
                    {
                        continue;
                    }

                    Ticket += str; 
                }

                return Ticket;
            }

            private string HPSH_ConvertFormatToElectronTicket_1F(int PlayTypeID, string Number)
            {
                return Number.Trim();
            }
            #endregion

            //参数说明 PlayTypeID:玩法类别; Number:投注号码; Multiple: 倍数; MaxMultiple:最大倍数;
            public override Ticket[] ToElectronicTicket_DYJ(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                if (PlayTypeID == PlayType_ZhiD)
                {
                    return ToElectronicTicket_DYJ_D(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                else if (PlayTypeID == PlayType_ZhiF)
                {
                    return ToElectronicTicket_DYJ_F(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                else if (PlayTypeID == PlayType_ZuD)
                {
                    return ToElectronicTicket_DYJ_ZuD(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                else if (PlayTypeID == PlayType_ZhiH)
                {
                    return ToElectronicTicket_DYJ_ZhiH(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                else if (PlayTypeID == PlayType_ZuH)
                {
                    return ToElectronicTicket_DYJ_ZuH(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                else if (PlayTypeID == PlayType_Zu6F)
                {
                    return ToElectronicTicket_DYJ_Zu6F(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                else if (PlayTypeID == PlayType_Zu3F)
                {
                    return ToElectronicTicket_DYJ_Zu3F(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                else if (PlayTypeID == PlayType_Q2D || PlayTypeID == PlayType_H2D)
                {
                    return ToElectronicTicket_DYJ_2D(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                else if (PlayTypeID == PlayType_Q2F || PlayTypeID == PlayType_H2F)
                {
                    return ToElectronicTicket_DYJ_2F(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                else if (PlayTypeID == PlayType_Q1D || PlayTypeID == PlayType_H1D)
                {
                    return ToElectronicTicket_DYJ_1D(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                else if (PlayTypeID == PlayType_Q1F || PlayTypeID == PlayType_H1F)
                {
                    return ToElectronicTicket_DYJ_1F(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
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

                        al.Add(new Ticket(201, ConvertFormatToElectronTicket_DYJ_D(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));
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

                        al.Add(new Ticket(208, ConvertFormatToElectronTicket_DYJ_F(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));
                    }
                }

                Ticket[] Tickets = new Ticket[al.Count];

                for (int i = 0; i < Tickets.Length; i++)
                {
                    Tickets[i] = (Ticket)al[i];
                }

                return Tickets;
            }
            private Ticket[] ToElectronicTicket_DYJ_ZuD(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
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

                        al.Add(new Ticket(202, ConvertFormatToElectronTicket_DYJ_D(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));
                    }
                }

                Ticket[] Tickets = new Ticket[al.Count];

                for (int i = 0; i < Tickets.Length; i++)
                {
                    Tickets[i] = (Ticket)al[i];
                }

                return Tickets;
            }
            private Ticket[] ToElectronicTicket_DYJ_ZhiH(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                string[] strs = AnalyseSchemeToElectronicTicket_DYJ_ZhiH(Number, PlayTypeID).Split('\n');

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

                        al.Add(new Ticket(201, ConvertFormatToElectronTicket_DYJ_D(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));
                    }
                }

                Ticket[] Tickets = new Ticket[al.Count];

                for (int i = 0; i < Tickets.Length; i++)
                {
                    Tickets[i] = (Ticket)al[i];
                }

                return Tickets;
            }
            private Ticket[] ToElectronicTicket_DYJ_ZuH(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                string[] strs = AnalyseSchemeToElectronicTicket_DYJ_ZuH(Number, PlayTypeID).Split('\n');

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

                        al.Add(new Ticket(202, ConvertFormatToElectronTicket_DYJ_D(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));
                    }
                }

                Ticket[] Tickets = new Ticket[al.Count];

                for (int i = 0; i < Tickets.Length; i++)
                {
                    Tickets[i] = (Ticket)al[i];
                }

                return Tickets;
            }
            private Ticket[] ToElectronicTicket_DYJ_Zu6F(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                string[] strs = AnalyseSchemeToElectronTicket_DYJ_Zu6F(Number, PlayTypeID).Split('\n');

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

                        al.Add(new Ticket(202, ConvertFormatToElectronTicket_DYJ_D(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));
                    }
                }

                Ticket[] Tickets = new Ticket[al.Count];

                for (int i = 0; i < Tickets.Length; i++)
                {
                    Tickets[i] = (Ticket)al[i];
                }

                return Tickets;
            }
            private Ticket[] ToElectronicTicket_DYJ_Zu3F(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                string[] strs = AnalyseSchemeToElectronTicket_DYJ_Zu3F(Number, PlayTypeID).Split('\n');

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

                        al.Add(new Ticket(202, ConvertFormatToElectronTicket_DYJ_D(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));
                    }
                }

                Ticket[] Tickets = new Ticket[al.Count];

                for (int i = 0; i < Tickets.Length; i++)
                {
                    Tickets[i] = (Ticket)al[i];
                }

                return Tickets;
            }
            private Ticket[] ToElectronicTicket_DYJ_2D(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                string[] strs = AnalyseSchemeToElectronTicket_DYJ_2D(Number, PlayTypeID).Split('\n');

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

                        al.Add(new Ticket(201, ConvertFormatToElectronTicket_DYJ_2D(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));
                    }
                }

                Ticket[] Tickets = new Ticket[al.Count];

                for (int i = 0; i < Tickets.Length; i++)
                {
                    Tickets[i] = (Ticket)al[i];
                }

                return Tickets;
            }
            private Ticket[] ToElectronicTicket_DYJ_2F(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                string[] strs = AnalyseSchemeToElectronTicket_DYJ_2F(Number, PlayTypeID).Split('\n');

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

                        if (strs[i].ToString().Split('|')[0].Length > 2)
                        {
                            Numbers = strs[i].ToString().Split('|')[0];
                            EachMoney += 2 * double.Parse(strs[i].ToString().Split('|')[1]);

                            Money += EachMoney * EachMultiple;

                            al.Add(new Ticket(208, ConvertFormatToElectronTicket_DYJ_2F(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));
                        }
                    }
                }

                Ticket[] Tickets = new Ticket[al.Count];

                for (int i = 0; i < Tickets.Length; i++)
                {
                    Tickets[i] = (Ticket)al[i];
                }

                return Tickets;
            }
            private Ticket[] ToElectronicTicket_DYJ_1D(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                string[] strs = AnalyseScheme_1(Number, PlayTypeID).Split('\n');

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

                        al.Add(new Ticket(201, ConvertFormatToElectronTicket_DYJ_1(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));
                    }
                }

                Ticket[] Tickets = new Ticket[al.Count];

                for (int i = 0; i < Tickets.Length; i++)
                {
                    Tickets[i] = (Ticket)al[i];
                }

                return Tickets;
            }
            private Ticket[] ToElectronicTicket_DYJ_1F(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                string[] strs = AnalyseScheme_1(Number, PlayTypeID).Split('\n');

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

                        if (strs[i].ToString().Split('|')[0].Length > 1)
                        {
                            Numbers = strs[i].ToString().Split('|')[0];
                            EachMoney += 2 * double.Parse(strs[i].ToString().Split('|')[1]);

                            Money += EachMoney * EachMultiple;

                            al.Add(new Ticket(208, ConvertFormatToElectronTicket_DYJ_1(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));
                        }
                    }
                }

                Ticket[] Tickets = new Ticket[al.Count];

                for (int i = 0; i < Tickets.Length; i++)
                {
                    Tickets[i] = (Ticket)al[i];
                }

                return Tickets;
            }

            private string AnalyseSchemeToElectronicTicket_DYJ_ZhiH(string Content, int PlayType)
            {
                string[] strs = Content.Split('\n');
                if (strs == null)
                    return "";
                if (strs.Length == 0)
                    return "";

                string Result = "";
                Regex regex = new Regex(@"^([\d]){1,2}", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                for (int i = 0; i < strs.Length; i++)
                {
                    Match m = regex.Match(strs[i]);
                    if (m.Success)
                    {
                        string CanonicalNumber = "";
                        string[] singles = ToSingle_ZhiH(m.Value, ref CanonicalNumber);
                        if (singles == null)
                            continue;
                        if (singles.Length >= 1)
                        {
                            Result += CanonicalNumber + "|" + singles.Length.ToString() + "\n";
                        }
                    }
                }

                if (Result.EndsWith("\n"))
                    Result = Result.Substring(0, Result.Length - 1);
                return Result;
            }
            private string AnalyseSchemeToElectronicTicket_DYJ_ZuH(string Content, int PlayType)
            {
                string[] strs = Content.Split('\n');
                if (strs == null)
                    return "";
                if (strs.Length == 0)
                    return "";

                string Result = "";
                Regex regex = new Regex(@"^([\d]){1,2}", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                for (int i = 0; i < strs.Length; i++)
                {
                    Match m = regex.Match(strs[i]);
                    if (m.Success)
                    {
                        string CanonicalNumber = "";
                        string[] singles = ToSingle_ZuH(m.Value, ref CanonicalNumber);
                        if (singles == null)
                            continue;
                        if (singles.Length >= 1)
                        {
                            for (int j = 0; j < singles.Length; j++)
                            {
                                Result += singles[j] + "|1\n";
                            }
                        }
                    }
                }

                if (Result.EndsWith("\n"))
                    Result = Result.Substring(0, Result.Length - 1);
                return Result;
            }
            private string AnalyseSchemeToElectronTicket_DYJ_Zu6F(string Content, int PlayType)
            {
                string[] strs = Content.Split('\n');
                if (strs == null)
                    return "";
                if (strs.Length == 0)
                    return "";

                string Result = "";

                string RegexString;
                if (PlayType == PlayType_ZuD)
                    RegexString = @"^([\d]){3}";
                else
                    RegexString = @"^([\d]){3,10}";
                Regex regex = new Regex(RegexString, RegexOptions.Compiled | RegexOptions.IgnoreCase);

                for (int i = 0; i < strs.Length; i++)
                {
                    Match m = regex.Match(strs[i]);
                    if (m.Success)
                    {
                        string CanonicalNumber = "";
                        string[] singles = ToSingle_Zu3D_Zu6(m.Value, ref CanonicalNumber);
                        if (singles == null)
                            continue;
                        if (singles.Length >= ((PlayType == PlayType_ZuD) ? 1 : 2))
                        {
                            Result += CanonicalNumber + "|" + singles.Length.ToString() + "\n";
                        }
                    }
                }

                if (Result.EndsWith("\n"))
                    Result = Result.Substring(0, Result.Length - 1);
                return Result;
            }
            private string AnalyseSchemeToElectronTicket_DYJ_Zu3F(string Content, int PlayType)
            {
                string[] strs = Content.Split('\n');
                if (strs == null)
                    return "";
                if (strs.Length == 0)
                    return "";

                string Result = "";
                Regex regex = new Regex(@"^([\d]){2,}", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                for (int i = 0; i < strs.Length; i++)
                {
                    Match m = regex.Match(strs[i]);
                    if (m.Success)
                    {
                        string CanonicalNumber = "";
                        string[] singles = ToSingle_Zu3F(m.Value, ref CanonicalNumber);
                        if (singles == null)
                            continue;
                        if (singles.Length >= 2)
                        {
                            Result += CanonicalNumber + "|" + singles.Length.ToString() + "\n";
                        }
                    }
                }

                if (Result.EndsWith("\n"))
                    Result = Result.Substring(0, Result.Length - 1);
                return Result;
            }
            private string AnalyseSchemeToElectronTicket_DYJ_2D(string Content, int PlayType)
            {
                string[] strs = Content.Split('\n');
                if (strs == null)
                    return "";
                if (strs.Length == 0)
                    return "";

                string Result = "";

                string RegexString;
                if ((PlayType == PlayType_Q2D) || (PlayType == PlayType_H2D))
                    RegexString = @"^([\d]){2}";
                else
                    RegexString = @"^(([\d])|([(][\d]{1,10}[)])){2}";
                Regex regex = new Regex(RegexString, RegexOptions.Compiled | RegexOptions.IgnoreCase);

                for (int i = 0; i < strs.Length; i++)
                {
                    Match m = regex.Match(strs[i]);
                    if (m.Success)
                    {
                        string CanonicalNumber = "";
                        string[] singles = ToSingle_2(m.Value, ref CanonicalNumber);
                        if (singles == null)
                            continue;

                        Result += CanonicalNumber + "|" + singles.Length.ToString() + "\n";
                    }
                }

                if (Result.EndsWith("\n"))
                    Result = Result.Substring(0, Result.Length - 1);
                return Result;
            }
            private string AnalyseSchemeToElectronTicket_DYJ_2F(string Content, int PlayType)
            {
                string[] strs = Content.Split('\n');
                if (strs == null)
                    return "";
                if (strs.Length == 0)
                    return "";

                string Result = "";

                string RegexString;
                if ((PlayType == PlayType_Q2D) || (PlayType == PlayType_H2D))
                    RegexString = @"^([\d]){2}";
                else
                    RegexString = @"^(([\d])|([(][\d]{1,10}[)])){2}";
                Regex regex = new Regex(RegexString, RegexOptions.Compiled | RegexOptions.IgnoreCase);

                for (int i = 0; i < strs.Length; i++)
                {
                    Match m = regex.Match(strs[i]);
                    if (m.Success)
                    {
                        string CanonicalNumber = "";
                        string[] singles = ToSingle_2(m.Value, ref CanonicalNumber);
                        if (singles == null)
                            continue;

                        Result += CanonicalNumber + "|" + singles.Length.ToString() + "\n";
                    }
                }

                if (Result.EndsWith("\n"))
                    Result = Result.Substring(0, Result.Length - 1);
                return Result;
            }

            // 转换为需要的彩票格式
            private string ConvertFormatToElectronTicket_DYJ_D(int PlayTypeID, string Number)
            {
                Number = Number.Trim();

                string Ticket = "";

                if (PlayTypeID == PlayType_ZhiD || PlayTypeID == PlayType_Zu6F || PlayTypeID == PlayType_Zu3F || PlayTypeID == PlayType_ZuH || PlayTypeID == PlayType_ZuD)
                {
                    string[] strs = Number.Split('\n');

                    for (int j = 0; j < strs.Length; j++)
                    {
                        if (PlayTypeID == PlayType_ZhiD)
                        {
                            Ticket += "1|";
                        }
                        else if (PlayTypeID == PlayType_Zu6F)
                        {
                            Ticket += "F6|";
                        }
                        else if (PlayTypeID == PlayType_Zu3F)
                        {
                            Ticket += "F3|";
                        }
                        else if (PlayTypeID == PlayType_ZuH)
                        {
                            Ticket += "6|";
                        }
                        else if (PlayTypeID == PlayType_ZuD)
                        {
                            Ticket += "6|";
                        }

                        for (int i = 0; i < strs[j].Length; i++)
                        {
                            Ticket += strs[j].Substring(i, 1) + ",";
                        }

                        if (Ticket.EndsWith(","))
                            Ticket = Ticket.Substring(0, Ticket.Length - 1);

                        Ticket += ";\n";
                    }

                    if (Ticket.EndsWith("\n"))
                        Ticket = Ticket.Substring(0, Ticket.Length - 1);
                }
                else if (PlayTypeID == PlayType_ZhiH)
                {
                    Ticket += "S1|" + Number + ";";
                }

                if (Ticket.EndsWith(","))
                    Ticket = Ticket.Substring(0, Ticket.Length - 1);

                Ticket = Ticket.Replace("-", "_");

                return Ticket;
            }
            private string ConvertFormatToElectronTicket_DYJ_F(int PlayTypeID, string Number)
            {
                Number = Number.Trim();

                string[] Locate = new string[3];

                string Ticket = "";

                if (PlayTypeID == PlayType_ZhiF)
                {
                    Regex regex = new Regex(@"^(?<L0>(\d)|([(][\d]+?[)]))(?<L1>(\d)|([(][\d]+?[)]))(?<L2>(\d)|([(][\d]+?[)]))", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                    Match m = regex.Match(Number);

                    Ticket += "1|";

                    for (int i = 0; i < 3; i++)
                    {
                        Locate[i] = m.Groups["L" + i.ToString()].ToString().Trim();
                        if (Locate[i] == "")
                        {
                            return "";
                        }
                        if (Locate[i].Length > 1)
                        {
                            Locate[i] = Locate[i].Substring(1, Locate[i].Length - 2);
                            if (Locate[i].Length > 1)
                                Locate[i] = FilterRepeated(Locate[i]);
                            if (Locate[i] == "")
                            {
                                return "";
                            }
                        }

                        Ticket += Locate[i] + ",";
                    }
                }

                if (Ticket.EndsWith(","))
                    Ticket = Ticket.Substring(0, Ticket.Length - 1);

                Ticket = Ticket.Replace("-", "_") + ";";

                return Ticket;
            }
            private string ConvertFormatToElectronTicket_DYJ_2D(int PlayTypeID, string Number)
            {
                Number = Number.Trim();

                string Ticket = "";

                if (PlayTypeID == PlayType_Q2D)
                {
                    string[] strs = Number.Split('\n');

                    for (int j = 0; j < strs.Length; j++)
                    {
                        Ticket += "2D|";

                        for (int i = 0; i < strs[j].Length; i++)
                        {
                            Ticket += strs[j].Substring(i, 1) + ",";
                        }

                        Ticket += "-;\n";

                    }

                    if (Ticket.EndsWith("\n"))
                        Ticket = Ticket.Substring(0, Ticket.Length - 1);
                }
                else
                {
                    string[] strs = Number.Split('\n');

                    for (int j = 0; j < strs.Length; j++)
                    {
                        Ticket += "2D|";

                        Ticket += "-,";

                        for (int i = 0; i < strs[j].Length; i++)
                        {
                            Ticket += strs[j].Substring(i, 1) + ",";
                        }

                        if (Ticket.EndsWith(","))
                            Ticket = Ticket.Substring(0, Ticket.Length - 1);

                        Ticket += ";\n";

                    }

                    if (Ticket.EndsWith("\n"))
                        Ticket = Ticket.Substring(0, Ticket.Length - 1);
                }

                if (Ticket.EndsWith(","))
                    Ticket = Ticket.Substring(0, Ticket.Length - 1);

                Ticket = Ticket.Replace("-", "_");

                return Ticket;
            }
            private string ConvertFormatToElectronTicket_DYJ_2F(int PlayTypeID, string Number)
            {
                Number = Number.Trim();

                string[] Locate = new string[2];

                string Ticket = "";

                Ticket += "2D|";

                if (PlayTypeID == PlayType_H2F)
                {
                    Ticket += "-,";
                }

                Regex regex = new Regex(@"^(?<L0>(\d)|([(][\d]+?[)]))(?<L1>(\d)|([(][\d]+?[)]))", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                Match m = regex.Match(Number);
                for (int i = 0; i < 2; i++)
                {
                    Locate[i] = m.Groups["L" + i.ToString()].ToString().Trim();
                    if (Locate[i] == "")
                    {
                        return "";
                    }
                    if (Locate[i].Length > 1)
                    {
                        Locate[i] = Locate[i].Substring(1, Locate[i].Length - 2);
                        if (Locate[i].Length > 1)
                            Locate[i] = FilterRepeated(Locate[i]);
                        if (Locate[i] == "")
                        {
                            return "";
                        }
                    }

                    Ticket += Locate[i] + ",";
                }

                if (PlayTypeID == PlayType_Q2F)
                {
                    Ticket += "-,";
                }

                if (Ticket.EndsWith(","))
                    Ticket = Ticket.Substring(0, Ticket.Length - 1);

                Ticket = Ticket.Replace("-", "_") + ";";

                return Ticket;
            }
            private string ConvertFormatToElectronTicket_DYJ_1(int PlayTypeID, string Number)
            {
                string Ticket = "";

                if (Number.EndsWith("\n"))
                    Number = Number.Substring(0, Number.Length - 1);

                string[] strs = Number.Split('\n');

                for (int j = 0; j < strs.Length; j++)
                {
                    Ticket += "1D|";

                    if (PlayTypeID == PlayType_H1D || PlayTypeID == PlayType_H1F)
                    {
                        Ticket += "-,-,";
                    }

                    Ticket += strs[j].ToString();

                    if (PlayTypeID == PlayType_Q1D || PlayTypeID == PlayType_Q1F)
                    {
                        Ticket += ",-,-";
                    }

                    Ticket += ";\n";
                }

                if (Ticket.EndsWith("\n"))
                    Ticket = Ticket.Substring(0, Ticket.Length - 1);

                Ticket = Ticket.Replace("-", "_");

                return Ticket;
            }
            #endregion
        }
    }
}