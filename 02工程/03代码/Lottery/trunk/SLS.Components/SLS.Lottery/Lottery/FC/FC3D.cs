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
        /// 福彩3D
        /// </summary>
        public partial class FC3D : LotteryBase
        {
            #region 静态变量
            public const int PlayType_ZhiD = 601;
            public const int PlayType_ZhiF = 602;
            public const int PlayType_ZuD = 603;
            public const int PlayType_Zu6F = 604;
            public const int PlayType_Zu3F = 605;
            public const int PlayType_ZhiB = 606;
            public const int PlayType_ZuB = 607;

            public const int ID = 6;
            public const string sID = "6";
            public const string Name = "福彩3D";
            public const string Code = "FC3D";
            public const double MaxMoney = 20000;
            #endregion

            public FC3D()
            {
                id = 6;
                name = "福彩3D";
                code = "FC3D";
            }

            public override bool CheckPlayType(int play_type)
            {
                return ((play_type >= 601) && (play_type <= 607));
            }

            public override PlayType[] GetPlayTypeList()
            {
                PlayType[] Result = new PlayType[7];

                Result[0] = new PlayType(PlayType_ZhiD, "直选单式");
                Result[1] = new PlayType(PlayType_ZhiF, "直选复式");
                Result[2] = new PlayType(PlayType_ZuD, "组选单式");
                Result[3] = new PlayType(PlayType_Zu6F, "组选6复式");
                Result[4] = new PlayType(PlayType_Zu3F, "组选3复式");
                Result[5] = new PlayType(PlayType_ZhiB, "直选包点");
                Result[6] = new PlayType(PlayType_ZuB, "组选包点");

                return Result;
            }

            public override string BuildNumber(int Num)	//id = 6
            {
                System.Random rd = new Random();
                StringBuilder sb = new StringBuilder();

                for (int i = 0; i < Num; i++)
                {
                    string LotteryNumber = "";
                    for (int j = 0; j < 3; j++)
                        LotteryNumber += rd.Next(0, 9 + 1).ToString();

                    sb.Append(LotteryNumber.Trim() + "\n");
                }

                string Result = sb.ToString();
                Result = Result.Substring(0, Result.Length - 1);
                return Result;
            }

            public override string[] ToSingle(string Number, ref string CanonicalNumber, int PlayType)
            {
                if ((PlayType == PlayType_ZhiD) || (PlayType == PlayType_ZhiF))
                    return ToSingle_Zhi(Number, ref CanonicalNumber);

                if ((PlayType == PlayType_ZuD) || (PlayType == PlayType_Zu6F))
                    return ToSingle_Zu3D_Zu6(Number, ref CanonicalNumber);

                if (PlayType == PlayType_Zu3F)
                    return ToSingle_Zu3F(Number, ref CanonicalNumber);

                if (PlayType == PlayType_ZhiB)
                    return ToSingle_ZhiB(Number, ref CanonicalNumber);

                if (PlayType == PlayType_ZuB)
                    return ToSingle_ZuB(Number, ref CanonicalNumber);

                return null;
            }
            #region ToSingle 的具体方法
            private string[] ToSingle_Zhi(string Number, ref string CanonicalNumber)	//复式取单式, 后面 ref 参数是将彩票规范化，如：10(122) 变成10(12)
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
            private string[] ToSingle_Zu3F(string Number, ref string CanonicalNumber)	//组选3取单式,后面 ref 参数是将彩票规范化，如：(10223) 变成1023，由于单式是3个数，复式可以是2个数，所以，这个函数不同于其他彩种的类似函数，单式不能使用这个函数转换。
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
            private string[] ToSingle_ZhiB(string sBill, ref string CanonicalNumber)	//直选包点取单式,后面 ref 参数是将彩票规范化，如：05) 变成 5
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
            private string[] ToSingle_ZuB(string sBill, ref string CanonicalNumber)	//组选6包点取单式,后面 ref 参数是将彩票规范化，如：05) 变成 5
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
            #endregion

            public override double ComputeWin(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, int PlayType, params double[] WinMoneyList)
            {
                if ((WinMoneyList == null) || (WinMoneyList.Length < 6))    // 奖金参数排列顺序 zhi, zu3 zu6
                    return -3;

                if ((PlayType == PlayType_ZhiD) || (PlayType == PlayType_ZhiF))
                    return ComputeWin_Zhi(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[0], WinMoneyList[1]);

                if ((PlayType == PlayType_ZuD) || (PlayType == PlayType_Zu6F))
                    return ComputeWin_Zu3D_Zu6(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[2], WinMoneyList[3], WinMoneyList[4], WinMoneyList[5]);

                if (PlayType == PlayType_Zu3F)
                    return ComputeWin_Zu3F(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[2], WinMoneyList[3]);

                if (PlayType == PlayType_ZhiB)
                    return ComputeWin_ZhiB(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[0], WinMoneyList[1]);

                if (PlayType == PlayType_ZuB)
                    return ComputeWin_ZuB(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[2], WinMoneyList[3], WinMoneyList[4], WinMoneyList[5]);

                return -4;
            }
            #region ComputeWin 的具体方法
            private double ComputeWin_Zhi(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney1, double WinMoneyNoWithTax1)	//计算中奖
            {
                WinNumber = WinNumber.Trim();
                if (WinNumber.Length < 3)	//3: "123"
                    return -1;
                string[] Lotterys = SplitLotteryNumber(Number);
                if (Lotterys == null)
                    return -2;
                if (Lotterys.Length < 1)
                    return -2;

                int Description1 = 0;
                double WinMoney = 0;
                WinMoneyNoWithTax = 0;
                Description = "";

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
                            Description1++;
                            WinMoney += WinMoney1;
                            WinMoneyNoWithTax += WinMoneyNoWithTax1;
                        }
                    }
                }

                if (Description1 > 0)
                    Description = "单选奖" + Description1.ToString() + "注。";

                return WinMoney;
            }
            private double ComputeWin_Zu3D_Zu6(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney1, double WinMoneyNoWithTax1, double WinMoney2, double WinMoneyNoWithTax2)	//计算中奖
            {
                WinNumber = WinNumber.Trim();
                if (WinNumber.Length < 3)	//3: "123"
                    return -1;
                WinNumber = WinNumber.Substring(0, 3);

                string[] Lotterys = SplitLotteryNumber(Number);
                if (Lotterys == null)
                    return -2;
                if (Lotterys.Length < 1)
                    return -2;

                int Description1 = 0, Description2 = 0;
                double WinMoney = 0;
                WinMoneyNoWithTax = 0;
                Description = "";

                for (int ii = 0; ii < Lotterys.Length; ii++)
                {
                    string t_str = "";

                    if (Lotterys[ii].Length < 3)
                        continue;
                    if (FilterRepeated(Sort(Lotterys[ii])).Length == 2)
                    {
                        if (Sort(Lotterys[ii]) == Sort(WinNumber))
                        {
                            Description1++;
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
                            Description2++;
                            WinMoney += WinMoney2;
                            WinMoneyNoWithTax += WinMoneyNoWithTax2;
                        }
                    }
                }

                if (Description1 > 0)
                    Description = "组选3奖" + Description1.ToString() + "注。";
                if (Description2 > 0)
                    Description = "组选6奖" + Description2.ToString() + "注。";

                return WinMoney;
            }
            private double ComputeWin_Zu3F(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney1, double WinMoneyNoWithTax1)	//计算中奖
            {
                WinNumber = WinNumber.Trim();
                if (WinNumber.Length < 3)	//3: "223"
                    return -1;
                string[] Lotterys = SplitLotteryNumber(Number);
                if (Lotterys == null)
                    return -2;
                if (Lotterys.Length < 1)
                    return -2;

                int Description1 = 0;
                double WinMoney = 0;
                WinMoneyNoWithTax = 0;
                Description = "";

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
                            Description1++;
                            WinMoney += WinMoney1;
                            WinMoneyNoWithTax += WinMoneyNoWithTax1;
                        }
                    }
                }

                if (Description1 > 0)
                    Description = "组选3奖" + Description1.ToString() + "注。";

                return WinMoney;
            }
            private double ComputeWin_ZhiB(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney1, double WinMoneyNoWithTax1)	//计算中奖
            {
                WinNumber = WinNumber.Trim();
                if (WinNumber.Length < 3)	//3: "123"
                    return -1;
                string[] Lotterys = SplitLotteryNumber(Number);
                if (Lotterys == null)
                    return -2;
                if (Lotterys.Length < 1)
                    return -2;

                int Description1 = 0;
                double WinMoney = 0;
                WinMoneyNoWithTax = 0;
                Description = "";

                for (int ii = 0; ii < Lotterys.Length; ii++)
                {
                    string t_str = "";
                    string[] Lottery = ToSingle_ZhiB(Lotterys[ii], ref t_str);
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
                            Description1++;
                            WinMoney += WinMoney1;
                            WinMoneyNoWithTax += WinMoneyNoWithTax1;
                        }
                    }
                }

                if (Description1 > 0)
                    Description = "单选奖" + Description1.ToString() + "注。";

                return WinMoney;
            }
            private double ComputeWin_ZuB(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney1, double WinMoneyNoWithTax1, double WinMoney2, double WinMoneyNoWithTax2)	//计算中奖
            {
                WinNumber = WinNumber.Trim();
                if (WinNumber.Length < 3)	//3: "123"
                    return -1;
                WinNumber = WinNumber.Substring(0, 3);

                string[] Lotterys = SplitLotteryNumber(Number);
                if (Lotterys == null)
                    return -2;
                if (Lotterys.Length < 1)
                    return -2;

                int Description1 = 0, Description2 = 0;
                double WinMoney = 0;
                WinMoneyNoWithTax = 0;
                Description = "";

                for (int ii = 0; ii < Lotterys.Length; ii++)
                {
                    string t_str = "";
                    string[] Lottery = ToSingle_ZuB(Lotterys[ii], ref t_str);
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
                                Description1++;
                                WinMoney += WinMoney1;
                                WinMoneyNoWithTax += WinMoneyNoWithTax1;
                            }
                            else
                            {
                                Description2++;
                                WinMoney += WinMoney2;
                                WinMoneyNoWithTax += WinMoneyNoWithTax2;
                            }
                        }
                    }
                }

                if (Description1 > 0)
                    Description = "组选3奖" + Description1.ToString() + "注。";
                if (Description2 > 0)
                    Description = "组选6奖" + Description2.ToString() + "注。";

                return WinMoney;
            }
            #endregion

            public override string AnalyseScheme(string Content, int PlayType)
            {
                if ((PlayType == PlayType_ZhiD) || (PlayType == PlayType_ZhiF))
                    return AnalyseScheme_Zhi(Content, PlayType);

                if ((PlayType == PlayType_ZuD) || (PlayType == PlayType_Zu6F))
                    return AnalyseScheme_Zu3D_Zu6(Content, PlayType);

                if (PlayType == PlayType_Zu3F)
                    return AnalyseScheme_Zu3F(Content, PlayType);

                if (PlayType == PlayType_ZhiB)
                    return AnalyseScheme_ZhiB(Content, PlayType);

                if (PlayType == PlayType_ZuB)
                    return AnalyseScheme_ZuB(Content, PlayType);

                return "";
            }
            #region AnalyseScheme 的具体方法
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
            private string AnalyseScheme_ZhiB(string Content, int PlayType)
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
                        string[] singles = ToSingle_ZhiB(m.Value, ref CanonicalNumber);
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
            private string AnalyseScheme_ZuB(string Content, int PlayType)
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
                        string[] singles = ToSingle_ZuB(m.Value, ref CanonicalNumber);
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
            #endregion

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
                    case "福彩投注系统2.2":
                        if (PlayTypeID == PlayType_ZhiD)//直单
                        {
                            return GetPrintKeyList_FCTZST2_2_ZhiD(Numbers);
                        }
                        if (PlayTypeID == PlayType_ZhiF)//直复
                        {
                            return GetPrintKeyList_FCTZST2_2_ZhiF(Numbers);
                        }
                        if (PlayTypeID == PlayType_ZuD)//组选单
                        {
                            return GetPrintKeyList_FCTZST2_2_ZhiD(Numbers);
                        }
                        if (PlayTypeID == PlayType_Zu6F)//组6复式
                        {
                            return GetPrintKeyList_FCTZST2_2_ZhiD(Numbers);
                        }
                        if (PlayTypeID == PlayType_Zu3F)//组3复式
                        {
                            return GetPrintKeyList_FCTZST2_2_ZhiD(Numbers);
                        }
                        if ((PlayTypeID == PlayType_ZhiB) || (PlayTypeID == PlayType_ZuB))   //包点
                        {
                            return GetPrintKeyList_FCTZST2_2_B(Numbers);
                        }
                        break;
                    case "FCR8000":
                        if (PlayTypeID == PlayType_ZhiD)//直单
                        {
                            return GetPrintKeyList_FCR8000_ZhiD(Numbers);
                        }
                        if (PlayTypeID == PlayType_ZhiF)//直复
                        {
                            return GetPrintKeyList_FCR8000_ZhiF(Numbers);
                        }
                        if (PlayTypeID == PlayType_ZuD)//组选单
                        {
                            return GetPrintKeyList_FCR8000_ZhiD(Numbers);
                        }
                        if (PlayTypeID == PlayType_Zu6F)//组6复式
                        {
                            return GetPrintKeyList_FCR8000_ZhiD(Numbers);
                        }
                        if (PlayTypeID == PlayType_Zu3F)//组3复式
                        {
                            return GetPrintKeyList_FCR8000_ZhiD(Numbers);
                        }
                        if ((PlayTypeID == PlayType_ZhiB) || (PlayTypeID == PlayType_ZuB))   //包点
                        {
                            return GetPrintKeyList_FCR8000_B(Numbers);
                        }
                        break;
                    case "LT-E":
                        if (PlayTypeID == PlayType_ZhiD)//直单
                        {
                            return GetPrintKeyList_LT_E_ZhiD(Numbers);
                        }
                        if (PlayTypeID == PlayType_ZhiF)//直复
                        {
                            return GetPrintKeyList_LT_E_ZhiF(Numbers);
                        }
                        if (PlayTypeID == PlayType_ZuD) //组选单
                        {
                            return GetPrintKeyList_LT_E_ZhiD(Numbers);
                        }
                        if (PlayTypeID == PlayType_Zu6F)//组6复式
                        {
                            return GetPrintKeyList_LT_E_ZuF(Numbers);
                        }
                        if (PlayTypeID == PlayType_Zu3F)//组3复式
                        {
                            return GetPrintKeyList_LT_E_ZuF(Numbers);
                        }
                        if ((PlayTypeID == PlayType_ZhiB) || (PlayTypeID == PlayType_ZuB))   //包点
                        {
                            return GetPrintKeyList_LT_E_B(Numbers);
                        }
                        break;
                    case "LT-E02":
                        if (PlayTypeID == PlayType_ZhiD)//直单
                        {
                            return GetPrintKeyList_LT_E02_ZhiD(Numbers);
                        }
                        if (PlayTypeID == PlayType_ZhiF)//直复
                        {
                            return GetPrintKeyList_LT_E02_ZhiF(Numbers);
                        }
                        if (PlayTypeID == PlayType_ZuD)//组选单
                        {
                            return GetPrintKeyList_LT_E02_ZhiD(Numbers);
                        }
                        if (PlayTypeID == PlayType_Zu6F)//组6复式
                        {
                            return GetPrintKeyList_LT_E02_ZuF(Numbers);
                        }
                        if (PlayTypeID == PlayType_Zu3F)//组3复式
                        {
                            return GetPrintKeyList_LT_E02_ZuF(Numbers);
                        }
                        if ((PlayTypeID == PlayType_ZhiB) || (PlayTypeID == PlayType_ZuB))   //包点
                        {
                            return GetPrintKeyList_LT_E02_B(Numbers);
                        }
                        break;
                    case "SN-3000CQA":
                        if (PlayTypeID == PlayType_ZhiD)//直单
                        {
                            return GetPrintKeyList_SN_3000CQA_ZhiD(Numbers);
                        }
                        if (PlayTypeID == PlayType_ZhiF)//直复
                        {
                            return GetPrintKeyList_SN_3000CQA_ZhiF(Numbers);
                        }
                        if (PlayTypeID == PlayType_ZuD)//组选单
                        {
                            return GetPrintKeyList_SN_3000CQA_ZhiD(Numbers);
                        }
                        if (PlayTypeID == PlayType_Zu6F)//组6复式
                        {
                            return GetPrintKeyList_SN_3000CQA_ZhiD(Numbers);
                        }
                        if (PlayTypeID == PlayType_Zu3F)//组3复式
                        {
                            return GetPrintKeyList_SN_3000CQA_ZhiD(Numbers);
                        }
                        if ((PlayTypeID == PlayType_ZhiB) || (PlayTypeID == PlayType_ZuB))   //包点
                        {
                            return GetPrintKeyList_SN_3000CQA_B(Numbers);
                        }
                        break;
                    case "SN-2000":
                        if (PlayTypeID == PlayType_ZhiD)//直单
                        {
                            return GetPrintKeyList_SN_2000_ZhiD(Numbers);
                        }
                        if (PlayTypeID == PlayType_ZhiF)//直复
                        {
                            return GetPrintKeyList_SN_2000_ZhiD(Numbers);
                        }
                        if (PlayTypeID == PlayType_ZuD)//组选单
                        {
                            return GetPrintKeyList_SN_2000_ZhiD(Numbers);
                        }
                        if (PlayTypeID == PlayType_Zu6F)//组6复式
                        {
                            return GetPrintKeyList_SN_2000_ZhiD(Numbers);
                        }
                        if (PlayTypeID == PlayType_Zu3F)//组3复式
                        {
                            return GetPrintKeyList_SN_2000_ZhiD(Numbers);
                        }
                        if ((PlayTypeID == PlayType_ZhiB) || (PlayTypeID == PlayType_ZuB))   //包点
                        {
                            return GetPrintKeyList_SN_2000_ZhiD(Numbers);
                        }
                        break;
                    case "SN_3000CG":
                        if (PlayTypeID == PlayType_ZhiD)//直单
                        {
                            return GetPrintKeyList_SN_3000CG_ZhiD(Numbers);
                        }
                        if (PlayTypeID == PlayType_ZhiF)//直复
                        {
                            return GetPrintKeyList_SN_3000CG_ZhiF(Numbers);
                        }
                        if (PlayTypeID == PlayType_ZuD)//组选单
                        {
                            return GetPrintKeyList_SN_3000CG_ZhiD(Numbers);
                        }
                        if (PlayTypeID == PlayType_Zu6F)//组6复式
                        {
                            return GetPrintKeyList_SN_3000CG_Zu6F(Numbers);
                        }
                        if (PlayTypeID == PlayType_Zu3F)//组3复式
                        {
                            return GetPrintKeyList_SN_3000CG_Zu6F(Numbers);
                        }
                        if ((PlayTypeID == PlayType_ZhiB) || (PlayTypeID == PlayType_ZuB))   //包点
                        {
                            return GetPrintKeyList_SN_3000CG_Zu6F(Numbers);
                        }
                        break;
                }

                return "";
            }
            #region GetPrintKeyList 的具体方法
            private string GetPrintKeyList_FCTZST2_2_ZhiD(string[] Numbers)
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
            private string GetPrintKeyList_FCTZST2_2_ZhiF(string[] Numbers)
            {
                string KeyList = "";

                foreach (string Number in Numbers)
                {
                    Regex regex = new Regex(@"(?<L0>(\d)|([(][\d]+?[)]))(?<L1>(\d)|([(][\d]+?[)]))(?<L2>(\d)|([(][\d]+?[)]))", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                    Match m = regex.Match(Number);

                    for (int i = 0; i < 3; i++)
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

                        if (i < 2)
                            KeyList += "[↓]";
                    }
                }

                return KeyList;
            }
            private string GetPrintKeyList_FCTZST2_2_B(string[] Numbers)
            {
                string KeyList = "";

                foreach (string Number in Numbers)
                {
                    string str = Number.Replace(" ", "").Replace("\r", "").Replace("\n", "").PadLeft(2, '0');
                    foreach (char ch in str)
                    {
                        KeyList += "[" + ch.ToString() + "]";
                    }
                }

                return KeyList;
            }

            private string GetPrintKeyList_FCR8000_ZhiD(string[] Numbers)
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
            private string GetPrintKeyList_FCR8000_ZhiF(string[] Numbers)
            {
                string KeyList = "";

                foreach (string Number in Numbers)
                {
                    Regex regex = new Regex(@"(?<L0>(\d)|([(][\d]+?[)]))(?<L1>(\d)|([(][\d]+?[)]))(?<L2>(\d)|([(][\d]+?[)]))", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                    Match m = regex.Match(Number);

                    for (int i = 0; i < 3; i++)
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

                        if (i < 2)
                            KeyList += "[ENTER]";
                    }
                }

                return KeyList;
            }
            private string GetPrintKeyList_FCR8000_B(string[] Numbers)
            {
                string KeyList = "";

                foreach (string Number in Numbers)
                {
                    string str = Number.Replace(" ", "").Replace("\r", "").Replace("\n", "").PadLeft(2, '0');
                    foreach (char ch in str)
                    {
                        KeyList += "[" + ch.ToString() + "]";
                    }
                }

                return KeyList;
            }

            private string GetPrintKeyList_LT_E_ZhiD(string[] Numbers)
            {
                string KeyList = "";

                foreach (string Number in Numbers)
                {

                    foreach (char ch in Number.Replace(" ", "").Replace("\r", "").Replace("\n", ""))
                    {
                        KeyList += "[" + ch.ToString() + "]";
                    }
                }

                KeyList = KeyList.Replace("0", "O").Replace("1", "Q").Replace("2", "R").Replace("3", "1").Replace("4", "S").Replace("5", "T").Replace("6", "4").Replace("7", "U").Replace("8", "V").Replace("9", "7");

                return KeyList;
            }
            private string GetPrintKeyList_LT_E_ZhiF(string[] Numbers)
            {
                string KeyList = "";

                foreach (string Number in Numbers)
                {
                    Regex regex = new Regex(@"(?<L0>(\d)|([(][\d]+?[)]))(?<L1>(\d)|([(][\d]+?[)]))(?<L2>(\d)|([(][\d]+?[)]))", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                    Match m = regex.Match(Number);

                    for (int i = 0; i < 3; i++)
                    {
                        string Locate = m.Groups["L" + i.ToString()].ToString().Trim();

                        if (Locate.Length == 1)
                        {
                            KeyList += "[" + Locate + "]";
                        }
                        else
                        {
                            Locate = Locate.Substring(1, Locate.Length - 2);

                            Locate = Locate.Replace("0", "O").Replace("1", "Q").Replace("2", "R").Replace("3", "1").Replace("4", "S").Replace("5", "T").Replace("6", "4").Replace("7", "U").Replace("8", "V").Replace("9", "7");

                            foreach (char ch in Locate)
                            {
                                KeyList += "[" + ch.ToString() + "]";
                            }
                        }

                        if (i < 2)
                            KeyList += "[8]";
                    }
                }

                return KeyList;
            }
            private string GetPrintKeyList_LT_E_ZuF(string[] Numbers)
            {
                string KeyList = "";

                foreach (string Number in Numbers)
                {
                    KeyList += "[" + Convert.ToString((Number.Replace(" ", "").Replace("\r", "").Replace("\n", "").Length)) + "]";
                    KeyList += "[ENTER]";

                    foreach (char ch in Number.Replace(" ", "").Replace("\r", "").Replace("\n", ""))
                    {
                        KeyList += "[" + ch.ToString() + "]";
                    }
                }

                KeyList = KeyList.Replace("0", "O").Replace("1", "Q").Replace("2", "R").Replace("3", "1").Replace("4", "S").Replace("5", "T").Replace("6", "4").Replace("7", "U").Replace("8", "V").Replace("9", "7");

                return KeyList;
            }
            private string GetPrintKeyList_LT_E_B(string[] Numbers)
            {
                string KeyList = "";

                foreach (string Number in Numbers)
                {
                    string str = Number.Replace(" ", "").Replace("\r", "").Replace("\n", "").PadLeft(2, '0');

                    str = str.Replace("0", "O").Replace("1", "Q").Replace("2", "R").Replace("3", "1").Replace("4", "S").Replace("5", "T").Replace("6", "4").Replace("7", "U").Replace("8", "V").Replace("9", "7");

                    foreach (char ch in str)
                    {
                        KeyList += "[" + ch.ToString() + "]";
                    }
                }

                return KeyList;
            }

            private string GetPrintKeyList_LT_E02_ZhiD(string[] Numbers)
            {
                string KeyList = "";

                foreach (string Number in Numbers)
                {

                    foreach (char ch in Number.Replace(" ", "").Replace("\r", "").Replace("\n", ""))
                    {
                        KeyList += "[" + ch.ToString() + "]";
                    }
                }

                KeyList = KeyList.Replace("0", "O").Replace("1", "3").Replace("2", "A").Replace("3", "B").Replace("4", "6").Replace("5", "C").Replace("6", "D").Replace("7", "9").Replace("8", "E").Replace("9", "F");

                return KeyList;
            }
            private string GetPrintKeyList_LT_E02_ZhiF(string[] Numbers)
            {
                string KeyList = "";

                foreach (string Number in Numbers)
                {
                    Regex regex = new Regex(@"(?<L0>(\d)|([(][\d]+?[)]))(?<L1>(\d)|([(][\d]+?[)]))(?<L2>(\d)|([(][\d]+?[)]))", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                    Match m = regex.Match(Number);

                    for (int i = 0; i < 3; i++)
                    {
                        string Locate = m.Groups["L" + i.ToString()].ToString().Trim();

                        if (Locate.Length == 1)
                        {
                            KeyList += "[" + Locate + "]";
                        }
                        else
                        {
                            Locate = Locate.Substring(1, Locate.Length - 2);

                            Locate = Locate.Replace("0", "O").Replace("1", "3").Replace("2", "A").Replace("3", "B").Replace("4", "6").Replace("5", "C").Replace("6", "D").Replace("7", "9").Replace("8", "E").Replace("9", "F");

                            foreach (char ch in Locate)
                            {
                                KeyList += "[" + ch.ToString() + "]";
                            }
                        }

                        if (i < 2)
                            KeyList += "[8]";
                    }
                }

                return KeyList;
            }
            private string GetPrintKeyList_LT_E02_ZuF(string[] Numbers)
            {
                string KeyList = "";

                foreach (string Number in Numbers)
                {
                    KeyList += "[" + Convert.ToString((Number.Replace(" ", "").Replace("\r", "").Replace("\n", "").Length)) + "]";
                    KeyList += "[ENTER]";

                    foreach (char ch in Number.Replace(" ", "").Replace("\r", "").Replace("\n", ""))
                    {
                        KeyList += "[" + ch.ToString() + "]";
                    }
                }

                KeyList = KeyList.Replace("0", "O").Replace("1", "3").Replace("2", "A").Replace("3", "B").Replace("4", "6").Replace("5", "C").Replace("6", "D").Replace("7", "9").Replace("8", "E").Replace("9", "F");

                return KeyList;
            }
            private string GetPrintKeyList_LT_E02_B(string[] Numbers)
            {
                string KeyList = "";

                foreach (string Number in Numbers)
                {
                    string str = Number.Replace(" ", "").Replace("\r", "").Replace("\n", "").PadLeft(2, '0');

                    str = str.Replace("0", "O").Replace("1", "3").Replace("2", "A").Replace("3", "B").Replace("4", "6").Replace("5", "C").Replace("6", "D").Replace("7", "9").Replace("8", "E").Replace("9", "F");

                    foreach (char ch in str)
                    {
                        KeyList += "[" + ch.ToString() + "]";
                    }
                }

                return KeyList;
            }

            private string GetPrintKeyList_SN_3000CQA_ZhiD(string[] Numbers)
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
            private string GetPrintKeyList_SN_3000CQA_ZhiF(string[] Numbers)
            {
                string KeyList = "";

                foreach (string Number in Numbers)
                {
                    Regex regex = new Regex(@"(?<L0>(\d)|([(][\d]+?[)]))(?<L1>(\d)|([(][\d]+?[)]))(?<L2>(\d)|([(][\d]+?[)]))", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                    Match m = regex.Match(Number);

                    for (int i = 0; i < 3; i++)
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

                        if (i < 2)
                            KeyList += "[↓]";
                    }
                }

                return KeyList;
            }
            private string GetPrintKeyList_SN_3000CQA_B(string[] Numbers)
            {
                string KeyList = "";

                foreach (string Number in Numbers)
                {
                    string str = Number.Replace(" ", "").Replace("\r", "").Replace("\n", "").PadLeft(2, '0');
                    foreach (char ch in str)
                    {
                        KeyList += "[" + ch.ToString() + "]";
                    }
                }

                return KeyList;
            }

            private string GetPrintKeyList_SN_2000_ZhiD(string[] Numbers)
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

            private string GetPrintKeyList_SN_3000CG_ZhiD(string[] Numbers)
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
            private string GetPrintKeyList_SN_3000CG_ZhiF(string[] Numbers)
            {
                string KeyList = "";

                foreach (string Number in Numbers)
                {
                    Regex regex = new Regex(@"(?<L0>(\d)|([(][\d]+?[)]))(?<L1>(\d)|([(][\d]+?[)]))(?<L2>(\d)|([(][\d]+?[)]))", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                    Match m = regex.Match(Number);

                    for (int i = 0; i < 3; i++)
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

                        KeyList += "[Enter]";
                    }
                }

                return KeyList;
            }
            private string GetPrintKeyList_SN_3000CG_Zu6F(string[] Numbers)
            {
                string KeyList = "";

                foreach (string Number in Numbers)
                {
                    foreach (char ch in Number.Replace(" ", "").Replace("\r", "").Replace("\n", ""))
                    {
                        KeyList += "[" + ch.ToString() + "]";
                    }
                }

                KeyList += "[Enter]";
                return KeyList;
            }
            #endregion

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
                else if (PlayTypeID == PlayType_ZuD || PlayTypeID == PlayType_Zu6F)
                {
                    return ToElectronicTicket_HPSH_Zu(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                else if (PlayTypeID == PlayType_ZhiB)
                {
                    return ToElectronicTicket_HPSH_ZhiB(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                else if (PlayTypeID == PlayType_Zu3F)
                {
                    return ToElectronicTicket_HPSH_Zu3F(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                else if (PlayTypeID == PlayType_ZuB)
                {
                    return ToElectronicTicket_HPSH_ZuB(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
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

                        al.Add(new Ticket(201, ConvertFormatToElectronTicket_HPSH(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));
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

                        al.Add(new Ticket(208, ConvertFormatToElectronTicket_HPSH(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));
                    }
                }

                Ticket[] Tickets = new Ticket[al.Count];

                for (int i = 0; i < Tickets.Length; i++)
                {
                    Tickets[i] = (Ticket)al[i];
                }

                return Tickets;
            }
            private Ticket[] ToElectronicTicket_HPSH_Zu(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                string[] strs = AnalyseSchemeToElectronicTicket_Zu3D_Zu6(Number, PlayTypeID).Split('\n');

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

                        al.Add(new Ticket(202, ConvertFormatToElectronTicket_HPSH(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));
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
                string[] strs = AnalyseSchemeToElectronicTicket_Zu3F(Number, PlayTypeID).Split('\n');

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

                        al.Add(new Ticket(202, ConvertFormatToElectronTicket_HPSH(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));
                    }
                }

                Ticket[] Tickets = new Ticket[al.Count];

                for (int i = 0; i < Tickets.Length; i++)
                {
                    Tickets[i] = (Ticket)al[i];
                }

                return Tickets;
            }
            private Ticket[] ToElectronicTicket_HPSH_ZuB(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                string[] strs = AnalyseSchemeToElectronicTicket_ZuB(Number, PlayTypeID).Split('\n');

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

                        al.Add(new Ticket(202, ConvertFormatToElectronTicket_HPSH(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));
                    }
                }

                Ticket[] Tickets = new Ticket[al.Count];

                for (int i = 0; i < Tickets.Length; i++)
                {
                    Tickets[i] = (Ticket)al[i];
                }

                return Tickets;
            }
            private Ticket[] ToElectronicTicket_HPSH_ZhiB(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
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

                        al.Add(new Ticket(204, Numbers, EachMultiple, EachMoney * EachMultiple));
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
            private string ConvertFormatToElectronTicket_HPSH(int PlayTypeID, string Number)
            {
                Number = Number.Trim();

                string Ticket = "";

                if (PlayTypeID == PlayType_ZhiD || PlayTypeID == PlayType_ZuD || PlayTypeID == PlayType_Zu6F || PlayTypeID == PlayType_Zu3F || PlayTypeID == PlayType_ZuB)
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
                else if (PlayTypeID == PlayType_ZhiB)
                {
                    Ticket = Number;
                }
                else if (PlayTypeID == PlayType_ZhiF)
                {
                    string[] Locate = new string[3];

                    Regex regex = new Regex(@"(?<L0>(\d)|([(][\d]+?[)]))(?<L1>(\d)|([(][\d]+?[)]))(?<L2>(\d)|([(][\d]+?[)]))", RegexOptions.IgnoreCase | RegexOptions.Compiled);
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

                return Ticket;
            }

            private string AnalyseSchemeToElectronicTicket_Zu3D_Zu6(string Content, int PlayType)
            {
                string[] strs = Content.Split('\n');
                if (strs == null)
                    return "";
                if (strs.Length == 0)
                    return "";

                string Result = "";

                string RegexString;

                RegexString = @"([\d]){3,10}";                              //@"([\d]){3,10}";
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

                        if (PlayType == PlayType_ZuD)
                        {
                            Result += m.Value + "|1\n";
                        }
                        else
                        {
                            if (singles.Length >= 1)
                            {
                                for (int j = 0; j < singles.Length; j++)
                                {
                                    Result += singles[j] + "|1\n";
                                }
                            }
                        }
                    }
                }

                if (Result.EndsWith("\n"))
                    Result = Result.Substring(0, Result.Length - 1);
                return Result;
            }
            private string AnalyseSchemeToElectronicTicket_Zu3F(string Content, int PlayType)
            {
                string[] strs = Content.Split('\n');
                if (strs == null)
                    return "";
                if (strs.Length == 0)
                    return "";

                string Result = "";

                Regex regex = new Regex(@"([\d]){2,}", RegexOptions.Compiled | RegexOptions.IgnoreCase);

                for (int i = 0; i < strs.Length; i++)
                {
                    Match m = regex.Match(strs[i]);
                    if (m.Success)
                    {
                        string CanonicalNumber = "";
                        string[] singles = ToSingle_Zu3F(m.Value, ref CanonicalNumber);
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
            private string AnalyseSchemeToElectronicTicket_ZuB(string Content, int PlayType)
            {
                string[] strs = Content.Split('\n');
                if (strs == null)
                    return "";
                if (strs.Length == 0)
                    return "";

                string Result = "";

                Regex regex = new Regex(@"([\d]){1,2}", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                for (int i = 0; i < strs.Length; i++)
                {
                    Match m = regex.Match(strs[i]);
                    if (m.Success)
                    {
                        string CanonicalNumber = "";
                        string[] singles = ToSingle_ZuB(m.Value, ref CanonicalNumber);
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
            #endregion

            public override Ticket[] ToElectronicTicket_ZCW(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                if (PlayTypeID == PlayType_ZhiD)
                {
                    return ToElectronicTicket_ZCW_D(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                else if (PlayTypeID == PlayType_ZhiF)
                {
                    return ToElectronicTicket_ZCW_F(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                else if (PlayTypeID == PlayType_ZuD)
                {
                    return ToElectronicTicket_ZCW_Zu(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                else if (PlayTypeID == PlayType_Zu6F)
                {
                    return ToElectronicTicket_ZCW_Zu6F(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                else if (PlayTypeID == PlayType_ZhiB)
                {
                    return ToElectronicTicket_ZCW_ZhiB(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                else if (PlayTypeID == PlayType_Zu3F)
                {
                    return ToElectronicTicket_ZCW_Zu3F(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                else if (PlayTypeID == PlayType_ZuB)
                {
                    return ToElectronicTicket_ZCW_ZuB(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
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
                                EachMoney += 2;
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
            private Ticket[] ToElectronicTicket_ZCW_Zu(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
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
                                EachMoney += 2;
                            }
                        }

                        Money += EachMoney * EachMultiple;

                        al.Add(new Ticket(3, ConvertFormatToElectronTicket_ZCW(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));
                    }
                }

                Ticket[] Tickets = new Ticket[al.Count];

                for (int i = 0; i < Tickets.Length; i++)
                {
                    Tickets[i] = (Ticket)al[i];
                }

                return Tickets;
            }
            private Ticket[] ToElectronicTicket_ZCW_Zu3F(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
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

                        al.Add(new Ticket(5, ConvertFormatToElectronTicket_ZCW(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));
                    }
                }

                Ticket[] Tickets = new Ticket[al.Count];

                for (int i = 0; i < Tickets.Length; i++)
                {
                    Tickets[i] = (Ticket)al[i];
                }

                return Tickets;
            }
            private Ticket[] ToElectronicTicket_ZCW_Zu6F(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
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

                        al.Add(new Ticket(4, ConvertFormatToElectronTicket_ZCW(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));
                    }
                }


                Ticket[] Tickets = new Ticket[al.Count];

                for (int i = 0; i < Tickets.Length; i++)
                {
                    Tickets[i] = (Ticket)al[i];
                }

                return Tickets;
            }
            private Ticket[] ToElectronicTicket_ZCW_ZuB(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                string[] strs = AnalyseSchemeToElectronicTicket_ZuB(Number, PlayTypeID).Split('\n');

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

                        al.Add(new Ticket(3, ConvertFormatToElectronTicket_ZCW(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));
                    }
                }

                Ticket[] Tickets = new Ticket[al.Count];

                for (int i = 0; i < Tickets.Length; i++)
                {
                    Tickets[i] = (Ticket)al[i];
                }

                return Tickets;
            }
            private Ticket[] ToElectronicTicket_ZCW_ZhiB(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
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

                        al.Add(new Ticket(2, Numbers, EachMultiple, EachMoney * EachMultiple));
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

                if (PlayTypeID == PlayType_ZhiD)
                {
                    string[] strs = Number.Split('\n');

                    for (int j = 0; j < strs.Length; j++)
                    {
                        for (int i = 0; i < strs[j].Length; i++)
                        {
                            Ticket += strs[j].Substring(i, 1) + "*";
                        }

                        if (Ticket.EndsWith("*"))
                            Ticket = Ticket.Substring(0, Ticket.Length - 1);

                        Ticket += "\n";
                    }

                    if (Ticket.EndsWith("\n"))
                        Ticket = Ticket.Substring(0, Ticket.Length - 1);

                }
                else if (PlayTypeID == PlayType_Zu6F || PlayTypeID == PlayType_Zu3F || PlayTypeID == PlayType_ZuB || PlayTypeID == PlayType_ZuD || PlayTypeID == PlayType_ZhiB)
                {
                    Ticket = Number;
                }
                else if (PlayTypeID == PlayType_ZhiF)
                {
                    string[] Locate = new string[3];

                    Regex regex = new Regex(@"(?<L0>(\d)|([(][\d]+?[)]))(?<L1>(\d)|([(][\d]+?[)]))(?<L2>(\d)|([(][\d]+?[)]))", RegexOptions.IgnoreCase | RegexOptions.Compiled);
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

                        Ticket += Locate[i] + "*";
                    }
                }

                if (Ticket.EndsWith("*"))
                    Ticket = Ticket.Substring(0, Ticket.Length - 1);

                return Ticket;
            }
            #endregion

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
                    return ToElectronicTicket_DYJ_Zu(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                else if (PlayTypeID == PlayType_ZhiB)
                {
                    return ToElectronicTicket_DYJ_ZhiB(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                else if (PlayTypeID == PlayType_Zu6F)
                {
                    return ToElectronicTicket_DYJ_Zu6F(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                else if (PlayTypeID == PlayType_Zu3F)
                {
                    return ToElectronicTicket_DYJ_Zu3F(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                else if (PlayTypeID == PlayType_ZuB)
                {
                    return ToElectronicTicket_DYJ_ZuB(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
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

                        al.Add(new Ticket(201, ConvertFormatToElectronTicket_DYJ(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));
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

                        al.Add(new Ticket(208, ConvertFormatToElectronTicket_DYJ(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));
                    }
                }

                Ticket[] Tickets = new Ticket[al.Count];

                for (int i = 0; i < Tickets.Length; i++)
                {
                    Tickets[i] = (Ticket)al[i];
                }

                return Tickets;
            }
            private Ticket[] ToElectronicTicket_DYJ_Zu(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                string[] strs = AnalyseSchemeToElectronicTicket_DYJ_Zu3D_Zu6(Number, PlayTypeID).Split('\n');

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

                        al.Add(new Ticket(202, ConvertFormatToElectronTicket_DYJ(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));
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
                string[] strs = AnalyseSchemeToElectronicTicket_DYJ_Zu3D_Zu6(Number, PlayTypeID).Split('\n');

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

                        al.Add(new Ticket(202, ConvertFormatToElectronTicket_DYJ(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));
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
                string[] strs = AnalyseSchemeToElectronicTicket_DYJ_Zu3F(Number, PlayTypeID).Split('\n');

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

                        al.Add(new Ticket(202, ConvertFormatToElectronTicket_DYJ(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));
                    }
                }

                Ticket[] Tickets = new Ticket[al.Count];

                for (int i = 0; i < Tickets.Length; i++)
                {
                    Tickets[i] = (Ticket)al[i];
                }

                return Tickets;
            }
            private Ticket[] ToElectronicTicket_DYJ_ZuB(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                string[] strs = AnalyseSchemeToElectronicTicket_DYJ_ZuB(Number, PlayTypeID).Split('\n');

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

                        al.Add(new Ticket(202, ConvertFormatToElectronTicket_DYJ(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));
                    }
                }

                Ticket[] Tickets = new Ticket[al.Count];

                for (int i = 0; i < Tickets.Length; i++)
                {
                    Tickets[i] = (Ticket)al[i];
                }

                return Tickets;
            }
            private Ticket[] ToElectronicTicket_DYJ_ZhiB(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
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

                        al.Add(new Ticket(204, ConvertFormatToElectronTicket_DYJ(PlayTypeID,Numbers), EachMultiple, EachMoney * EachMultiple));
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
            private string ConvertFormatToElectronTicket_DYJ(int PlayTypeID, string Number)
            {
                Number = Number.Trim();

                string Ticket = "";

                if (PlayTypeID == PlayType_ZhiD || PlayTypeID == PlayType_ZuD || PlayTypeID == PlayType_ZuB)
                {
                    string[] strs = Number.Split('\n');

                    for (int j = 0; j < strs.Length; j++)
                    {
                        if (PlayTypeID == PlayType_ZhiD)
                        {
                            Ticket += "1|";
                        }
                        else if (PlayTypeID == PlayType_ZuD)
                        {
                            Ticket += "6|";
                        }
                        else if (PlayTypeID == PlayType_ZuB)
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
                else if (PlayTypeID == PlayType_ZhiB)
                {
                    Ticket = "S1|" + Number + ";";
                }
                else if (PlayTypeID == PlayType_Zu6F)
                {
                    Ticket += "F6|" + Number + ";";
                }
                else if (PlayTypeID == PlayType_Zu3F)
                {
                    Ticket += "F3|" + Number + ";";
                }
                else if (PlayTypeID == PlayType_ZhiF)
                {
                    Ticket += "1|";

                    string[] Locate = new string[3];

                    Regex regex = new Regex(@"(?<L0>(\d)|([(][\d]+?[)]))(?<L1>(\d)|([(][\d]+?[)]))(?<L2>(\d)|([(][\d]+?[)]))", RegexOptions.IgnoreCase | RegexOptions.Compiled);
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

                    if (Ticket.EndsWith(","))
                        Ticket = Ticket.Substring(0, Ticket.Length - 1);

                    Ticket += ";";
                }

                if (Ticket.EndsWith(","))
                    Ticket = Ticket.Substring(0, Ticket.Length - 1);

                return Ticket;
            }

            private string AnalyseSchemeToElectronicTicket_DYJ_Zu3D_Zu6(string Content, int PlayType)
            {
                string[] strs = FilterRepeated(Content).Split('\n');
                if (strs == null)
                    return "";
                if (strs.Length == 0)
                    return "";

                string Result = "";

                string RegexString;

                RegexString = @"([\d]){3,10}";                              //@"([\d]){3,10}";
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

                        if (PlayType == PlayType_ZuD)
                        {
                            Result += CanonicalNumber + "|1\n";
                        }
                        else
                        {
                            Result += CanonicalNumber + "|" + singles.Length.ToString() + "\n";
                        }
                    }
                }

                if (Result.EndsWith("\n"))
                    Result = Result.Substring(0, Result.Length - 1);
                return Result;
            }
            private string AnalyseSchemeToElectronicTicket_DYJ_Zu3F(string Content, int PlayType)
            {
                string[] strs = FilterRepeated(Content).Split('\n');
                if (strs == null)
                    return "";
                if (strs.Length == 0)
                    return "";

                string Result = "";

                Regex regex = new Regex(@"([\d]){2,}", RegexOptions.Compiled | RegexOptions.IgnoreCase);

                for (int i = 0; i < strs.Length; i++)
                {
                    Match m = regex.Match(strs[i]);
                    if (m.Success)
                    {
                        string CanonicalNumber = "";
                        string[] singles = ToSingle_Zu3F(m.Value, ref CanonicalNumber);
                        if (singles == null)
                            continue;

                        Result += CanonicalNumber + "|" + singles.Length.ToString() + "\n";
                    }
                }

                if (Result.EndsWith("\n"))
                    Result = Result.Substring(0, Result.Length - 1);
                return Result;
            }
            private string AnalyseSchemeToElectronicTicket_DYJ_ZuB(string Content, int PlayType)
            {
                string[] strs = FilterRepeated(Content).Split('\n');
                if (strs == null)
                    return "";
                if (strs.Length == 0)
                    return "";

                string Result = "";

                Regex regex = new Regex(@"([\d]){1,2}", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                for (int i = 0; i < strs.Length; i++)
                {
                    Match m = regex.Match(strs[i]);
                    if (m.Success)
                    {
                        string CanonicalNumber = "";
                        string[] singles = ToSingle_ZuB(m.Value, ref CanonicalNumber);
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
            #endregion
        }
    }
}
