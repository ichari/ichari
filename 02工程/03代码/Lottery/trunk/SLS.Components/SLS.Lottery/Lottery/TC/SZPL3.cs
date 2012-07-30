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
        /// 体彩排列3
        /// </summary>
        public partial class SZPL3 : LotteryBase
        {
            #region 静态变量
            public const int PlayType_3_ZhiD = 6301;
            public const int PlayType_3_ZhiF = 6302;
            public const int PlayType_3_ZuD = 6303;
            public const int PlayType_3_Zu6F = 6304;
            public const int PlayType_3_Zu3F = 6305;
            public const int PlayType_3_ZhiH = 6306;
            public const int PlayType_3_ZuH = 6307;

            public const int ID = 63;
            public const string sID = "63";
            public const string Name = "数字排列3";
            public const string Code = "SZPL3";
            public const double MaxMoney = 20000;
            #endregion

            public SZPL3()
            {
                id = 63;
                name = "数字排列3";
                code = "SZPL3";
            }

            public override bool CheckPlayType(int play_type)
            {
                return ((play_type >= 6301) && (play_type <= 6307));
            }

            public override PlayType[] GetPlayTypeList()
            {
                PlayType[] Result = new PlayType[7];

                Result[0] = new PlayType(PlayType_3_ZhiD, "排列3直选单式");
                Result[1] = new PlayType(PlayType_3_ZhiF, "排列3直选复式");
                Result[2] = new PlayType(PlayType_3_ZuD, "排列3组选单式");
                Result[3] = new PlayType(PlayType_3_Zu6F, "排列3组选6复式");
                Result[4] = new PlayType(PlayType_3_Zu3F, "排列3组选3复式");
                Result[5] = new PlayType(PlayType_3_ZhiH, "排列3直选和值");
                Result[6] = new PlayType(PlayType_3_ZuH, "排列3组选和值");

                return Result;
            }

            public override string BuildNumber(int Num)	//id = 63,num = 注数
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
                if ((PlayType == PlayType_3_ZhiD) || (PlayType == PlayType_3_ZhiF))
                    return ToSingle_3(Number, ref CanonicalNumber);

                if ((PlayType == PlayType_3_ZuD) || (PlayType == PlayType_3_Zu6F))
                    return ToSingle_Zu3D_Zu6(Number, ref CanonicalNumber);

                if ((PlayType == PlayType_3_Zu3F))
                    return ToSingle_Zu3F(Number, ref CanonicalNumber);

                if ((PlayType == PlayType_3_ZhiH))
                    return ToSingle_ZhiH(Number, ref CanonicalNumber);

                if ((PlayType == PlayType_3_ZuH))
                    return ToSingle_ZuH(Number, ref CanonicalNumber);

                return null;
            }
            #region ToSingle 的具体方法
            private string[] ToSingle_3(string Number, ref string CanonicalNumber)	//复式取单式, 后面 ref 参数是将彩票规范化，如：10(223) 变成10(23)
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
            private string[] ToSingle_Zu3D_Zu6(string Number, ref string CanonicalNumber)	//复式取单式, 后面 ref 参数是将彩票规范化，如：10223) 变成1023
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
            private string[] ToSingle_ZuH(string sBill, ref string CanonicalNumber)	//组选6和值取单式,后面 ref 参数是将彩票规范化，如：05) 变成 5
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
                if ((PlayType == PlayType_3_ZhiD) || (PlayType == PlayType_3_ZhiF))
                    return ComputeWin_3(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[0], WinMoneyList[1]);

                if ((PlayType == PlayType_3_ZuD) || (PlayType == PlayType_3_Zu6F))
                    return ComputeWin_Zu3D_Zu6(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[2], WinMoneyList[3], WinMoneyList[4], WinMoneyList[5]);

                if (PlayType == PlayType_3_Zu3F)
                    return ComputeWin_Zu3F(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[2], WinMoneyList[3]);

                if (PlayType == PlayType_3_ZhiH)
                    return ComputeWin_ZhiH(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[0], WinMoneyList[1]);

                if (PlayType == PlayType_3_ZuH)
                    return ComputeWin_ZuH(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[2], WinMoneyList[3], WinMoneyList[4], WinMoneyList[5]);

                return -4;
            }
            #region ComputeWin 的具体方法
            private double ComputeWin_3(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney1, double WinMoneyNoWithTax1)	//计算中奖
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

                int Description1 = 0;
                double WinMoney = 0;
                WinMoneyNoWithTax = 0;
                Description = "";

                for (int ii = 0; ii < Lotterys.Length; ii++)
                {
                    string t_str = "";
                    string[] Lottery = ToSingle_3(Lotterys[ii], ref t_str);
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
                WinNumber = WinNumber.Substring(0, 3);

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
            private double ComputeWin_ZhiH(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney1, double WinMoneyNoWithTax1)	//计算中奖
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

                int Description1 = 0;
                double WinMoney = 0;
                WinMoneyNoWithTax = 0;
                Description = "";

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
            private double ComputeWin_ZuH(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney1, double WinMoneyNoWithTax1, double WinMoney2, double WinMoneyNoWithTax2)	//计算中奖
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
                if ((PlayType == PlayType_3_ZhiD) || (PlayType == PlayType_3_ZhiF))
                    return AnalyseScheme_3(Content, PlayType);

                if ((PlayType == PlayType_3_ZuD) || (PlayType == PlayType_3_Zu6F))
                    return AnalyseScheme_Zu3D_Zu6(Content, PlayType);

                if (PlayType == PlayType_3_Zu3F)
                    return AnalyseScheme_Zu3F(Content, PlayType);

                if (PlayType == PlayType_3_ZhiH)
                    return AnalyseScheme_ZhiH(Content, PlayType);

                if (PlayType == PlayType_3_ZuH)
                    return AnalyseScheme_ZuH(Content, PlayType);

                return "";
            }
            #region AnalyseScheme 的具体方法
            private string AnalyseScheme_3(string Content, int PlayType)
            {
                string[] strs = Content.Split('\n');
                if (strs == null)
                    return "";
                if (strs.Length == 0)
                    return "";

                string Result = "";

                string RegexString;
                if (PlayType == PlayType_3_ZhiD)
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
                        string[] singles = ToSingle_3(m.Value, ref CanonicalNumber);
                        if (singles == null)
                            continue;
                        if (singles.Length >= ((PlayType == PlayType_3_ZhiD) ? 1 : 2))
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
                if (PlayType == PlayType_3_ZuD)
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
                        if (singles.Length >= ((PlayType == PlayType_3_ZuD) ? 1 : 2))
                        {
                            if (FilterRepeated(Sort(m.Value)).Length == 2)
                            {
                                if (PlayType != PlayType_3_Zu6F)
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
            #endregion

            public override bool AnalyseWinNumber(string Number)
            {
                string t_str = "";
                string[] WinLotteryNumber = ToSingle_3(Number, ref t_str);

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
                    case "CR_YTCII2":
                        if (PlayTypeID == PlayType_3_ZhiD)//排3只单
                        {
                            return GetPrintKeyList_CR_YTCII2_ZhiD(Numbers);
                        }
                        if (PlayTypeID == PlayType_3_ZhiF)//排3只复
                        {
                            return GetPrintKeyList_CR_YTCII2_3_ZhiF(Numbers);
                        }
                        if (PlayTypeID == PlayType_3_ZuD)   //组选单式
                        {
                            return GetPrintKeyList_CR_YTCII2_ZhiD(Numbers);
                        }
                        if (PlayTypeID == PlayType_3_Zu3F)//排3组3复式
                        {
                            return GetPrintKeyList_CR_YTCII2_ZhiD(Numbers);
                        }
                        if (PlayTypeID == PlayType_3_Zu6F)//排3组6复式
                        {
                            return GetPrintKeyList_CR_YTCII2_ZhiD(Numbers);
                        }
                        if ((PlayTypeID == PlayType_3_ZhiH) || (PlayTypeID == PlayType_3_ZuH))//和值
                        {
                            return GetPrintKeyList_CR_YTCII2_H(Numbers);
                        }
                        break;
                    case "TCBJYTD":
                        if (PlayTypeID == PlayType_3_ZhiD)//排3只单
                        {
                            return GetPrintKeyList_TCBJYTD_ZhiD(Numbers);
                        }
                        if (PlayTypeID == PlayType_3_ZhiF)//排3只复
                        {
                            return GetPrintKeyList_TCBJYTD_3_ZhiF(Numbers);
                        }
                        if (PlayTypeID == PlayType_3_ZuD)   //组选单式
                        {
                            return GetPrintKeyList_TCBJYTD_ZhiD(Numbers);
                        }
                        if (PlayTypeID == PlayType_3_Zu3F)//排3组3复式
                        {
                            return GetPrintKeyList_TCBJYTD_ZhiD(Numbers);
                        }
                        if (PlayTypeID == PlayType_3_Zu6F)//排3组6复式
                        {
                            return GetPrintKeyList_TCBJYTD_ZhiD(Numbers);
                        }
                        if ((PlayTypeID == PlayType_3_ZhiH) || (PlayTypeID == PlayType_3_ZuH))//和值
                        {
                            return GetPrintKeyList_TCBJYTD_H(Numbers);
                        }
                        break;
                    case "TGAMPOS4000":
                        if (PlayTypeID == PlayType_3_ZhiD)//排3只单
                        {
                            return GetPrintKeyList_TGAMPOS4000_ZhiD(Numbers);
                        }
                        if (PlayTypeID == PlayType_3_ZhiF)//排3只复
                        {
                            return GetPrintKeyList_TGAMPOS4000_3_ZhiF(Numbers);
                        }
                        if (PlayTypeID == PlayType_3_ZuD)  //组选单式
                        {
                            return GetPrintKeyList_TGAMPOS4000_ZhiD(Numbers);
                        }
                        if (PlayTypeID == PlayType_3_Zu3F)//排3组3复式
                        {
                            return GetPrintKeyList_TGAMPOS4000_ZhiD(Numbers);
                        }
                        if (PlayTypeID == PlayType_3_Zu6F)//排3组6复式
                        {
                            return GetPrintKeyList_TGAMPOS4000_ZhiD(Numbers);
                        }
                        if ((PlayTypeID == PlayType_3_ZhiH) || (PlayTypeID == PlayType_3_ZuH))//和值
                        {
                            return GetPrintKeyList_TGAMPOS4000_H(Numbers);
                        }
                        break;
                    case "CP86":
                        if (PlayTypeID == PlayType_3_ZhiD)//排3只单
                        {
                            return GetPrintKeyList_CP86_ZhiD(Numbers);
                        }
                        if (PlayTypeID == PlayType_3_ZhiF)//排3只复
                        {
                            return GetPrintKeyList_CP86_ZhiF(Numbers);
                        }
                        if (PlayTypeID == PlayType_3_ZuD)  //组选单式
                        {
                            return GetPrintKeyList_CP86_ZhiD(Numbers);
                        }
                        if (PlayTypeID == PlayType_3_Zu3F)//排3组3复式
                        {
                            return GetPrintKeyList_CP86_ZhiD(Numbers);
                        }
                        if (PlayTypeID == PlayType_3_Zu6F)//排3组6复式
                        {
                            return GetPrintKeyList_CP86_ZhiD(Numbers);
                        }
                        if ((PlayTypeID == PlayType_3_ZhiH) || (PlayTypeID == PlayType_3_ZuH))//和值
                        {
                            return GetPrintKeyList_CP86_H(Numbers);
                        }
                        break;
                    case "MODEL_4000":
                        if (PlayTypeID == PlayType_3_ZhiD)//排3只单
                        {
                            return GetPrintKeyList_MODEL_4000_ZhiD(Numbers);
                        }
                        if (PlayTypeID == PlayType_3_ZhiF)//排3只复
                        {
                            return GetPrintKeyList_MODEL_4000_3_ZhiF(Numbers);
                        }
                        if (PlayTypeID == PlayType_3_ZuD)  //组选单式
                        {
                            return GetPrintKeyList_MODEL_4000_ZhiD(Numbers);
                        }
                        if (PlayTypeID == PlayType_3_Zu3F)//排3组3复式
                        {
                            return GetPrintKeyList_MODEL_4000_ZhiD(Numbers);
                        }
                        if (PlayTypeID == PlayType_3_Zu6F)//排3组6复式
                        {
                            return GetPrintKeyList_MODEL_4000_ZhiD(Numbers);
                        }
                        if ((PlayTypeID == PlayType_3_ZhiH) || (PlayTypeID == PlayType_3_ZuH))//和值
                        {
                            return GetPrintKeyList_MODEL_4000_H(Numbers);
                        }
                        break;
                    case "CORONISTPT":
                        if (PlayTypeID == PlayType_3_ZhiD)//排3只单
                        {
                            return GetPrintKeyList_CORONISTPT_ZhiD(Numbers);
                        }
                        if (PlayTypeID == PlayType_3_ZhiF)//排3只复
                        {
                            return GetPrintKeyList_CORONISTPT_3_ZhiF(Numbers);
                        }
                        if (PlayTypeID == PlayType_3_ZuD)  //组选单式
                        {
                            return GetPrintKeyList_CORONISTPT_ZhiD(Numbers);
                        }
                        if (PlayTypeID == PlayType_3_Zu3F)//排3组3复式
                        {
                            return GetPrintKeyList_CORONISTPT_ZhiD(Numbers);
                        }
                        if (PlayTypeID == PlayType_3_Zu6F)//排3组6复式
                        {
                            return GetPrintKeyList_CORONISTPT_ZhiD(Numbers);
                        }
                        if ((PlayTypeID == PlayType_3_ZhiH) || (PlayTypeID == PlayType_3_ZuH))//和值
                        {
                            return GetPrintKeyList_CORONISTPT_H(Numbers);
                        }
                        break;
                    case "RS6500":
                        if (PlayTypeID == PlayType_3_ZhiD)//排3只单
                        {
                            return GetPrintKeyList_RS6500_ZhiD(Numbers);
                        }
                        if (PlayTypeID == PlayType_3_ZhiF)//排3只复
                        {
                            return GetPrintKeyList_RS6500_3_ZhiF(Numbers);
                        }
                        if (PlayTypeID == PlayType_3_ZuD)   //组选单式
                        {
                            return GetPrintKeyList_RS6500_ZhiD(Numbers);
                        }
                        if (PlayTypeID == PlayType_3_Zu3F)//排3组3复式
                        {
                            return GetPrintKeyList_RS6500_ZhiD(Numbers);
                        }
                        if (PlayTypeID == PlayType_3_Zu6F)//排3组6复式
                        {
                            return GetPrintKeyList_RS6500_ZhiD(Numbers);
                        }
                        if ((PlayTypeID == PlayType_3_ZhiH) || (PlayTypeID == PlayType_3_ZuH))//和值
                        {
                            return GetPrintKeyList_RS6500_H(Numbers);
                        }
                        break;
                    case "ks230":
                        if (PlayTypeID == PlayType_3_ZhiD)//排3只单
                        {
                            return GetPrintKeyList_ks230_ZhiD(Numbers);
                        }
                        if (PlayTypeID == PlayType_3_ZhiF)//排3只复
                        {
                            return GetPrintKeyList_ks230_3_ZhiF(Numbers);
                        }
                        if (PlayTypeID == PlayType_3_ZuD)   //组选单式
                        {
                            return GetPrintKeyList_ks230_ZhiD(Numbers);
                        }
                        if (PlayTypeID == PlayType_3_Zu3F)//排3组3复式
                        {
                            return GetPrintKeyList_ks230_ZhiD(Numbers);
                        }
                        if (PlayTypeID == PlayType_3_Zu6F)//排3组6复式
                        {
                            return GetPrintKeyList_ks230_ZhiD(Numbers);
                        }
                        if ((PlayTypeID == PlayType_3_ZhiH) || (PlayTypeID == PlayType_3_ZuH))//和值
                        {
                            return GetPrintKeyList_ks230_H(Numbers);
                        }
                        break;
                    case "LA-600A":
                        if (PlayTypeID == PlayType_3_ZhiD)//排3只单
                        {
                            return GetPrintKeyList_LA_600A_ZhiD(Numbers);
                        }
                        if (PlayTypeID == PlayType_3_ZhiF)//排3只复
                        {
                            return GetPrintKeyList_LA_600A_3_ZhiF(Numbers);
                        }
                        if (PlayTypeID == PlayType_3_ZuD)   //组选单式
                        {
                            return GetPrintKeyList_LA_600A_ZhiD(Numbers);
                        }
                        if (PlayTypeID == PlayType_3_Zu3F)//排3组3复式
                        {
                            return GetPrintKeyList_LA_600A_ZhiD(Numbers);
                        }
                        if (PlayTypeID == PlayType_3_Zu6F)//排3组6复式
                        {
                            return GetPrintKeyList_LA_600A_ZhiD(Numbers);
                        }
                        if ((PlayTypeID == PlayType_3_ZhiH) || (PlayTypeID == PlayType_3_ZuH))//和值
                        {
                            return GetPrintKeyList_LA_600A_H(Numbers);
                        }
                        break;
                    case "TSP700_II":
                        if (PlayTypeID == PlayType_3_ZhiD)//排3只单
                        {
                            return GetPrintKeyList_TSP700_II_ZhiD(Numbers);
                        }
                        if (PlayTypeID == PlayType_3_ZhiF)//排3只复
                        {
                            return GetPrintKeyList_TSP700_II_3_ZhiF(Numbers);
                        }
                        if (PlayTypeID == PlayType_3_ZuD)   //组选单式
                        {
                            return GetPrintKeyList_TSP700_II_ZhiD(Numbers);
                        }
                        if (PlayTypeID == PlayType_3_Zu3F)//排3组3复式
                        {
                            return GetPrintKeyList_TSP700_II_ZhiD(Numbers);
                        }
                        if (PlayTypeID == PlayType_3_Zu6F)//排3组6复式
                        {
                            return GetPrintKeyList_TSP700_II_ZhiD(Numbers);
                        }
                        if ((PlayTypeID == PlayType_3_ZhiH) || (PlayTypeID == PlayType_3_ZuH))//和值
                        {
                            return GetPrintKeyList_TSP700_II_H(Numbers);
                        }
                        break;
                }

                return "";
            }
            #region GetPrintKeyList 的具体方法
            private string GetPrintKeyList_CR_YTCII2_ZhiD(string[] Numbers)
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
            private string GetPrintKeyList_CR_YTCII2_3_ZhiF(string[] Numbers)
            {
                string KeyList = "";

                foreach (string Number in Numbers)
                {
                    Regex regex = new Regex(@"^(?<L0>(\d)|([(][\d]+?[)]))(?<L1>(\d)|([(][\d]+?[)]))(?<L2>(\d)|([(][\d]+?[)]))", RegexOptions.IgnoreCase | RegexOptions.Compiled);
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
            private string GetPrintKeyList_CR_YTCII2_H(string[] Numbers)
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
            private string GetPrintKeyList_CR_YTCII2_5_F(string[] Numbers)
            {
                string KeyList = "";

                foreach (string Number in Numbers)
                {
                    Regex regex = new Regex(@"^(?<L0>(\d)|([(][\d]+?[)]))(?<L1>(\d)|([(][\d]+?[)]))(?<L2>(\d)|([(][\d]+?[)]))(?<L3>(\d)|([(][\d]+?[)]))(?<L4>(\d)|([(][\d]+?[)]))", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                    Match m = regex.Match(Number);

                    for (int i = 0; i < 5; i++)
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

                        if (i < 4)
                            KeyList += "[↓]";
                    }
                }

                return KeyList;
            }

            private string GetPrintKeyList_TCBJYTD_ZhiD(string[] Numbers)
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
            private string GetPrintKeyList_TCBJYTD_3_ZhiF(string[] Numbers)
            {
                string KeyList = "";

                foreach (string Number in Numbers)
                {
                    Regex regex = new Regex(@"^(?<L0>(\d)|([(][\d]+?[)]))(?<L1>(\d)|([(][\d]+?[)]))(?<L2>(\d)|([(][\d]+?[)]))", RegexOptions.IgnoreCase | RegexOptions.Compiled);
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
                            KeyList += "[→]";
                    }
                }

                return KeyList;
            }
            private string GetPrintKeyList_TCBJYTD_H(string[] Numbers)
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
            private string GetPrintKeyList_TCBJYTD_5_F(string[] Numbers)
            {
                string KeyList = "";

                foreach (string Number in Numbers)
                {
                    Regex regex = new Regex(@"^(?<L0>(\d)|([(][\d]+?[)]))(?<L1>(\d)|([(][\d]+?[)]))(?<L2>(\d)|([(][\d]+?[)]))(?<L3>(\d)|([(][\d]+?[)]))(?<L4>(\d)|([(][\d]+?[)]))", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                    Match m = regex.Match(Number);

                    for (int i = 0; i < 5; i++)
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

                        if (i < 4)
                            KeyList += "[→]";
                    }
                }

                return KeyList;
            }

            private string GetPrintKeyList_TGAMPOS4000_ZhiD(string[] Numbers)
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
            private string GetPrintKeyList_TGAMPOS4000_3_ZhiF(string[] Numbers)
            {
                string KeyList = "";

                foreach (string Number in Numbers)
                {
                    Regex regex = new Regex(@"^(?<L0>(\d)|([(][\d]+?[)]))(?<L1>(\d)|([(][\d]+?[)]))(?<L2>(\d)|([(][\d]+?[)]))", RegexOptions.IgnoreCase | RegexOptions.Compiled);
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
            private string GetPrintKeyList_TGAMPOS4000_H(string[] Numbers)
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
            private string GetPrintKeyList_TGAMPOS4000_5_F(string[] Numbers)
            {
                string KeyList = "";

                foreach (string Number in Numbers)
                {
                    Regex regex = new Regex(@"^(?<L0>(\d)|([(][\d]+?[)]))(?<L1>(\d)|([(][\d]+?[)]))(?<L2>(\d)|([(][\d]+?[)]))(?<L3>(\d)|([(][\d]+?[)]))(?<L4>(\d)|([(][\d]+?[)]))", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                    Match m = regex.Match(Number);

                    for (int i = 0; i < 5; i++)
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

                        if (i < 4)
                            KeyList += "[↓]";
                    }
                }

                return KeyList;
            }

            private string GetPrintKeyList_CP86_ZhiD(string[] Numbers)
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
            private string GetPrintKeyList_CP86_ZhiF(string[] Numbers)
            {
                string KeyList = "";

                foreach (string Number in Numbers)
                {
                    Regex regex = new Regex(@"^(?<L0>(\d)|([(][\d]+?[)]))(?<L1>(\d)|([(][\d]+?[)]))(?<L2>(\d)|([(][\d]+?[)]))", RegexOptions.IgnoreCase | RegexOptions.Compiled);
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
            private string GetPrintKeyList_CP86_H(string[] Numbers)
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
            private string GetPrintKeyList_CP86_5_F(string[] Numbers)
            {
                string KeyList = "";

                foreach (string Number in Numbers)
                {
                    Regex regex = new Regex(@"^(?<L0>(\d)|([(][\d]+?[)]))(?<L1>(\d)|([(][\d]+?[)]))(?<L2>(\d)|([(][\d]+?[)]))(?<L3>(\d)|([(][\d]+?[)]))(?<L4>(\d)|([(][\d]+?[)]))", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                    Match m = regex.Match(Number);

                    for (int i = 0; i < 5; i++)
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

                        if (i < 4)
                            KeyList += "[↓]";
                    }
                }

                return KeyList;
            }

            private string GetPrintKeyList_MODEL_4000_ZhiD(string[] Numbers)
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
            private string GetPrintKeyList_MODEL_4000_3_ZhiF(string[] Numbers)
            {
                string KeyList = "";

                foreach (string Number in Numbers)
                {
                    Regex regex = new Regex(@"^(?<L0>(\d)|([(][\d]+?[)]))(?<L1>(\d)|([(][\d]+?[)]))(?<L2>(\d)|([(][\d]+?[)]))", RegexOptions.IgnoreCase | RegexOptions.Compiled);
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
            private string GetPrintKeyList_MODEL_4000_H(string[] Numbers)
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
            private string GetPrintKeyList_MODEL_4000_5_F(string[] Numbers)
            {
                string KeyList = "";

                foreach (string Number in Numbers)
                {
                    Regex regex = new Regex(@"^(?<L0>(\d)|([(][\d]+?[)]))(?<L1>(\d)|([(][\d]+?[)]))(?<L2>(\d)|([(][\d]+?[)]))(?<L3>(\d)|([(][\d]+?[)]))(?<L4>(\d)|([(][\d]+?[)]))", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                    Match m = regex.Match(Number);

                    for (int i = 0; i < 5; i++)
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

                        if (i < 4)
                            KeyList += "[↓]";
                    }
                }

                return KeyList;
            }

            private string GetPrintKeyList_CORONISTPT_ZhiD(string[] Numbers)
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
            private string GetPrintKeyList_CORONISTPT_3_ZhiF(string[] Numbers)
            {
                string KeyList = "";

                foreach (string Number in Numbers)
                {
                    Regex regex = new Regex(@"^(?<L0>(\d)|([(][\d]+?[)]))(?<L1>(\d)|([(][\d]+?[)]))(?<L2>(\d)|([(][\d]+?[)]))", RegexOptions.IgnoreCase | RegexOptions.Compiled);
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
            private string GetPrintKeyList_CORONISTPT_H(string[] Numbers)
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
            private string GetPrintKeyList_CORONISTPT_5_F(string[] Numbers)
            {
                string KeyList = "";

                foreach (string Number in Numbers)
                {
                    Regex regex = new Regex(@"^(?<L0>(\d)|([(][\d]+?[)]))(?<L1>(\d)|([(][\d]+?[)]))(?<L2>(\d)|([(][\d]+?[)]))(?<L3>(\d)|([(][\d]+?[)]))(?<L4>(\d)|([(][\d]+?[)]))", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                    Match m = regex.Match(Number);

                    for (int i = 0; i < 5; i++)
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

                        if (i < 4)
                            KeyList += "[↓]";
                    }
                }

                return KeyList;
            }

            private string GetPrintKeyList_RS6500_ZhiD(string[] Numbers)
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
            private string GetPrintKeyList_RS6500_3_ZhiF(string[] Numbers)
            {
                string KeyList = "";

                foreach (string Number in Numbers)
                {
                    Regex regex = new Regex(@"^(?<L0>(\d)|([(][\d]+?[)]))(?<L1>(\d)|([(][\d]+?[)]))(?<L2>(\d)|([(][\d]+?[)]))", RegexOptions.IgnoreCase | RegexOptions.Compiled);
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
            private string GetPrintKeyList_RS6500_H(string[] Numbers)
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
            private string GetPrintKeyList_RS6500_5_F(string[] Numbers)
            {
                string KeyList = "";

                foreach (string Number in Numbers)
                {
                    Regex regex = new Regex(@"^(?<L0>(\d)|([(][\d]+?[)]))(?<L1>(\d)|([(][\d]+?[)]))(?<L2>(\d)|([(][\d]+?[)]))(?<L3>(\d)|([(][\d]+?[)]))(?<L4>(\d)|([(][\d]+?[)]))", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                    Match m = regex.Match(Number);

                    for (int i = 0; i < 5; i++)
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

                        if (i < 4)
                            KeyList += "[↓]";
                    }
                }

                return KeyList;
            }

            private string GetPrintKeyList_ks230_ZhiD(string[] Numbers)
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
            private string GetPrintKeyList_ks230_3_ZhiF(string[] Numbers)
            {
                string KeyList = "";

                foreach (string Number in Numbers)
                {
                    Regex regex = new Regex(@"^(?<L0>(\d)|([(][\d]+?[)]))(?<L1>(\d)|([(][\d]+?[)]))(?<L2>(\d)|([(][\d]+?[)]))", RegexOptions.IgnoreCase | RegexOptions.Compiled);
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
            private string GetPrintKeyList_ks230_H(string[] Numbers)
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
            private string GetPrintKeyList_ks230_5_F(string[] Numbers)
            {
                string KeyList = "";

                foreach (string Number in Numbers)
                {
                    Regex regex = new Regex(@"^(?<L0>(\d)|([(][\d]+?[)]))(?<L1>(\d)|([(][\d]+?[)]))(?<L2>(\d)|([(][\d]+?[)]))(?<L3>(\d)|([(][\d]+?[)]))(?<L4>(\d)|([(][\d]+?[)]))", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                    Match m = regex.Match(Number);

                    for (int i = 0; i < 5; i++)
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

                        if (i < 4)
                            KeyList += "[↓]";
                    }
                }

                return KeyList;
            }

            private string GetPrintKeyList_LA_600A_ZhiD(string[] Numbers)
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
            private string GetPrintKeyList_LA_600A_3_ZhiF(string[] Numbers)
            {
                string KeyList = "";

                foreach (string Number in Numbers)
                {
                    Regex regex = new Regex(@"^(?<L0>(\d)|([(][\d]+?[)]))(?<L1>(\d)|([(][\d]+?[)]))(?<L2>(\d)|([(][\d]+?[)]))", RegexOptions.IgnoreCase | RegexOptions.Compiled);
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
            private string GetPrintKeyList_LA_600A_H(string[] Numbers)
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
            private string GetPrintKeyList_LA_600A_5_F(string[] Numbers)
            {
                string KeyList = "";

                foreach (string Number in Numbers)
                {
                    Regex regex = new Regex(@"^(?<L0>(\d)|([(][\d]+?[)]))(?<L1>(\d)|([(][\d]+?[)]))(?<L2>(\d)|([(][\d]+?[)]))(?<L3>(\d)|([(][\d]+?[)]))(?<L4>(\d)|([(][\d]+?[)]))", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                    Match m = regex.Match(Number);

                    for (int i = 0; i < 5; i++)
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

                        if (i < 4)
                            KeyList += "[↓]";
                    }
                }

                return KeyList;
            }

            private string GetPrintKeyList_TSP700_II_ZhiD(string[] Numbers)
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
            private string GetPrintKeyList_TSP700_II_3_ZhiF(string[] Numbers)
            {
                string KeyList = "";

                foreach (string Number in Numbers)
                {
                    Regex regex = new Regex(@"^(?<L0>(\d)|([(][\d]+?[)]))(?<L1>(\d)|([(][\d]+?[)]))(?<L2>(\d)|([(][\d]+?[)]))", RegexOptions.IgnoreCase | RegexOptions.Compiled);
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
            private string GetPrintKeyList_TSP700_II_H(string[] Numbers)
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
            #endregion

            //参数说明 PlayTypeID:玩法类别; Number:投注号码; Multiple: 倍数; MaxMultiple:最大倍数;
            public override Ticket[] ToElectronicTicket_HPSD(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                if (PlayTypeID == PlayType_3_ZhiD)
                {
                    return ToElectronicTicket_HPSD_Zhi_D(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                if ((PlayTypeID == PlayType_3_ZhiF) || (PlayTypeID == PlayType_3_ZhiH))
                {
                    return ToElectronicTicket_HPSD_Zhi_F(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                if (PlayTypeID == PlayType_3_ZuD)
                {
                    return ToElectronicTicket_HPSD_Zu_D(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                if ((PlayTypeID == PlayType_3_Zu3F) || (PlayTypeID == PlayType_3_Zu6F) || (PlayTypeID == PlayType_3_ZuH))
                {
                    return ToElectronicTicket_HPSD_Zu_F(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                return null;
            }
            #region ToElectronicTicket_HPSD 的具体方法
            private Ticket[] ToElectronicTicket_HPSD_Zhi_D(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
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
                                EachMoney += 2 * 1;
                            }
                        }

                        Money += EachMoney * EachMultiple;

                        al.Add(new Ticket(201, ConvertFormatToElectronTicket_HPSD(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));
                    }
                }

                Ticket[] Tickets = new Ticket[al.Count];

                for (int i = 0; i < Tickets.Length; i++)
                {
                    Tickets[i] = (Ticket)al[i];
                }

                return Tickets;
            }
            private Ticket[] ToElectronicTicket_HPSD_Zhi_F(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                string CanonicalNumber = "";
                Money = 0;
                ArrayList al = new ArrayList();

                for (int j = 0; j < Number.Split('\n').Length; j++)
                {
                    string[] strs = ToSingle(Number.Split('\n')[j], ref CanonicalNumber, PlayTypeID);

                    if (strs == null)
                    {
                        return null;
                    }
                    if (strs.Length == 0)
                    {
                        return null;
                    }

                    int MultipleNum = 0;

                    if ((Multiple % MaxMultiple) != 0)
                    {
                        MultipleNum = (Multiple - (Multiple % MaxMultiple)) / MaxMultiple + 1;
                    }
                    else
                    {
                        MultipleNum = Multiple / MaxMultiple;
                    }

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
                                    EachMoney += 2 * 1;
                                }
                            }

                            Money += EachMoney * EachMultiple;

                            al.Add(new Ticket(201, ConvertFormatToElectronTicket_HPSD(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));
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
            private Ticket[] ToElectronicTicket_HPSD_Zu_D(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
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
                                EachMoney += 2 * 1;
                            }
                        }

                        Money += EachMoney * EachMultiple;

                        al.Add(new Ticket(202, ConvertFormatToElectronTicket_HPSD(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));
                    }
                }

                Ticket[] Tickets = new Ticket[al.Count];

                for (int i = 0; i < Tickets.Length; i++)
                {
                    Tickets[i] = (Ticket)al[i];
                }

                return Tickets;
            }
            private Ticket[] ToElectronicTicket_HPSD_Zu_F(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                string CanonicalNumber = "";

                ArrayList al = new ArrayList();
                Money = 0;
                for (int j = 0; j < Number.Split('\n').Length; j++)
                {
                    string[] strs = ToSingle(Number.Split('\n')[j], ref CanonicalNumber, PlayTypeID);

                    if (strs == null)
                    {
                        return null;
                    }
                    if (strs.Length == 0)
                    {
                        return null;
                    }

                    int MultipleNum = 0;

                    if ((Multiple % MaxMultiple) != 0)
                    {
                        MultipleNum = (Multiple - (Multiple % MaxMultiple)) / MaxMultiple + 1;
                    }
                    else
                    {
                        MultipleNum = Multiple / MaxMultiple;
                    }

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
                                    EachMoney += 2 * 1;
                                }
                            }

                            Money += EachMoney * EachMultiple;

                            al.Add(new Ticket(202, ConvertFormatToElectronTicket_HPSD(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));
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

            private string ConvertFormatToElectronTicket_HPSD(int PlayTypeID, string Number)
            {
                Number = Number.Trim();

                string Ticket = "";

                string[] strs = Number.Split('\n');

                for (int i = 0; i < strs.Length; i++)
                {
                    for (int j = 0; j < strs[i].Length; j++)
                    {

                        if (PlayTypeID == PlayType_3_ZuD)
                        {
                            strs[i] = Sort(strs[i]);
                        }

                        Ticket += strs[i].Substring(j, 1) + ",";
                    }

                    if (Ticket.EndsWith(","))
                    {
                        Ticket = Ticket.Substring(0, Ticket.Length - 1);
                    }

                    Ticket = Ticket + "\n";
                }

                if (Ticket.EndsWith("\n"))
                {
                    Ticket = Ticket.Substring(0, Ticket.Length - 1);
                }

                return Ticket;
            }
            #endregion

            public override Ticket[] ToElectronicTicket_ZCW(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                if (PlayTypeID == PlayType_3_ZhiD)
                {
                    return ToElectronicTicket_ZCW_ZhiD(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                if (PlayTypeID == PlayType_3_ZhiF)
                {
                    return ToElectronicTicket_ZCW_ZhiF(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                if (PlayTypeID == PlayType_3_ZhiH)
                {
                    return ToElectronicTicket_ZCW_ZhiH(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                if (PlayTypeID == PlayType_3_ZuD)
                {
                    return ToElectronicTicket_ZCW_ZuD(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                if (PlayTypeID == PlayType_3_Zu3F)
                {
                    return ToElectronicTicket_ZCW_Zu3F(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                if  (PlayTypeID == PlayType_3_Zu6F)
                {
                    return ToElectronicTicket_ZCW_Zu6F(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                if (PlayTypeID == PlayType_3_ZuH)
                {
                    return ToElectronicTicket_ZCW_ZuH(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                return null;
            }
            #region ToElectronicTicket_ZCW 的具体方法
            private Ticket[] ToElectronicTicket_ZCW_ZhiD(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
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
                                EachMoney += 2 * 1;
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
            private Ticket[] ToElectronicTicket_ZCW_ZhiF(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
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
            private Ticket[] ToElectronicTicket_ZCW_ZhiH(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
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
            private Ticket[] ToElectronicTicket_ZCW_ZuD(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
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
                                EachMoney += 2 * 1;
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
            private Ticket[] ToElectronicTicket_ZCW_ZuH(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
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

                        al.Add(new Ticket(6, ConvertFormatToElectronTicket_ZCW(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));
                    }
                }

                Ticket[] Tickets = new Ticket[al.Count];

                for (int i = 0; i < Tickets.Length; i++)
                {
                    Tickets[i] = (Ticket)al[i];
                }

                return Tickets;
            }

            private string ConvertFormatToElectronTicket_ZCW(int PlayTypeID, string Number)
            {
                Number = Number.Trim();

                string Ticket = "";

                if (PlayTypeID == PlayType_3_ZhiD)
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
                else if (PlayTypeID == PlayType_3_Zu6F || PlayTypeID == PlayType_3_Zu3F || PlayTypeID == PlayType_3_ZuH || PlayTypeID == PlayType_3_ZhiH || PlayTypeID == PlayType_3_ZuD)
                {
                    Ticket = Number;
                }
                else if (PlayTypeID == PlayType_3_ZhiF)
                {
                    string[] Locate = new string[3];

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

                        Ticket += Locate[i] + "*";
                    }
                }

                if (Ticket.EndsWith("*"))
                    Ticket = Ticket.Substring(0, Ticket.Length - 1);

                return Ticket;
            }
            #endregion

            //参数说明 PlayTypeID:玩法类别; Number:投注号码; Multiple: 倍数; MaxMultiple:最大倍数;
            public override Ticket[] ToElectronicTicket_ZZYTC(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                if (PlayTypeID == PlayType_3_ZhiD)
                {
                    return ToElectronicTicket_ZZYTC_Zhi_D(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                if (PlayTypeID == PlayType_3_ZhiF)
                {
                    return ToElectronicTicket_ZZYTC_Zhi_F(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                if (PlayTypeID == PlayType_3_ZhiH)
                {
                    return ToElectronicTicket_ZZYTC_Zhi_H(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                if (PlayTypeID == PlayType_3_ZuD)
                {
                    return ToElectronicTicket_ZZYTC_Zu_D(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                if (PlayTypeID == PlayType_3_Zu3F)
                {
                    return ToElectronicTicket_ZZYTC_Zu3_F(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                if (PlayTypeID == PlayType_3_Zu6F)
                {
                    return ToElectronicTicket_ZZYTC_Zu6_F(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                if (PlayTypeID == PlayType_3_ZuH)
                {
                    return ToElectronicTicket_ZZYTC_Zu_H(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                return null;
            }
            #region ToElectronicTicket_ZZYTC 的具体方法
            private Ticket[] ToElectronicTicket_ZZYTC_Zhi_D(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
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
                                EachMoney += 2 * 1;
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
            private Ticket[] ToElectronicTicket_ZZYTC_Zhi_F(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
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
            private Ticket[] ToElectronicTicket_ZZYTC_Zhi_H(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
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

                        al.Add(new Ticket(2, ConvertFormatToElectronTicket_ZZYTC(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));
                    }
                }

                Ticket[] Tickets = new Ticket[al.Count];

                for (int i = 0; i < Tickets.Length; i++)
                {
                    Tickets[i] = (Ticket)al[i];
                }

                return Tickets;
            }
            private Ticket[] ToElectronicTicket_ZZYTC_Zu_D(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
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
                                EachMoney += 2 * 1;
                            }
                        }

                        Money += EachMoney * EachMultiple;

                        al.Add(new Ticket(3, ConvertFormatToElectronTicket_ZZYTC(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));
                    }
                }

                Ticket[] Tickets = new Ticket[al.Count];

                for (int i = 0; i < Tickets.Length; i++)
                {
                    Tickets[i] = (Ticket)al[i];
                }

                return Tickets;
            }
            private Ticket[] ToElectronicTicket_ZZYTC_Zu3_F(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
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

                        al.Add(new Ticket(5, ConvertFormatToElectronTicket_ZZYTC(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));
                    }
                }

                Ticket[] Tickets = new Ticket[al.Count];

                for (int i = 0; i < Tickets.Length; i++)
                {
                    Tickets[i] = (Ticket)al[i];
                }

                return Tickets;
            }
            private Ticket[] ToElectronicTicket_ZZYTC_Zu6_F(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
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

                        al.Add(new Ticket(4, ConvertFormatToElectronTicket_ZZYTC(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));
                    }
                }

                Ticket[] Tickets = new Ticket[al.Count];

                for (int i = 0; i < Tickets.Length; i++)
                {
                    Tickets[i] = (Ticket)al[i];
                }

                return Tickets;
            }
            private Ticket[] ToElectronicTicket_ZZYTC_Zu_H(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
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

                        al.Add(new Ticket(6, ConvertFormatToElectronTicket_ZZYTC(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));
                    }
                }

                Ticket[] Tickets = new Ticket[al.Count];

                for (int i = 0; i < Tickets.Length; i++)
                {
                    Tickets[i] = (Ticket)al[i];
                }

                return Tickets;
            }

            private string ConvertFormatToElectronTicket_ZZYTC(int PlayTypeID, string Number)
            {
                Number = Number.Trim();

                string Ticket = "";

                string[] strs = Number.Split('\n');

                for (int i = 0; i < strs.Length; i++)
                {
                    if (PlayTypeID == PlayType_3_ZhiH || PlayTypeID == PlayType_3_ZuH)
                    {
                        Ticket += strs[i];
                    }

                    if (PlayTypeID == PlayType_3_ZhiD || PlayTypeID == PlayType_3_Zu3F || PlayTypeID == PlayType_3_Zu6F || PlayTypeID == PlayType_3_ZuD)
                    {
                        for (int j = 0; j < strs[i].Length; j++)
                        {
                            Ticket += strs[i].Substring(j, 1) + "*";
                        }
                    }

                    if (PlayTypeID == PlayType_3_ZhiF)
                    {
                        string[] Locate = new string[3];

                        Regex regex = new Regex(@"^(?<L0>(\d)|([(][\d]+?[)]))(?<L1>(\d)|([(][\d]+?[)]))(?<L2>(\d)|([(][\d]+?[)]))", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                        Match m = regex.Match(strs[i]);

                        for (int j = 0; j < 3; j++)
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
                    }

                    if (Ticket.EndsWith("*"))
                    {
                        Ticket = Ticket.Substring(0, Ticket.Length - 1);
                    }

                    Ticket = Ticket + "\n";
                }

                if (Ticket.EndsWith("\n"))
                {
                    Ticket = Ticket.Substring(0, Ticket.Length - 1);
                }

                return Ticket;
            }
            #endregion

            public override Ticket[] ToElectronicTicket_DYJ(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                if (PlayTypeID == PlayType_3_ZhiD)
                {
                    return ToElectronicTicket_DYJ_Zhi_D(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                if ((PlayTypeID == PlayType_3_ZhiF) || (PlayTypeID == PlayType_3_ZhiH))
                {
                    return ToElectronicTicket_DYJ_Zhi_F(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                if (PlayTypeID == PlayType_3_ZuD)
                {
                    return ToElectronicTicket_DYJ_Zu_D(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                if ((PlayTypeID == PlayType_3_Zu3F) || (PlayTypeID == PlayType_3_Zu6F) || (PlayTypeID == PlayType_3_ZuH))
                {
                    return ToElectronicTicket_DYJ_Zu_F(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                return null;
            }
            #region ToElectronicTicket_DYJ 的具体方法
            private Ticket[] ToElectronicTicket_DYJ_Zhi_D(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
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
                                if (strs[i + M].ToString().Split('|').Length < 2)
                                {
                                    continue;
                                }

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
            private Ticket[] ToElectronicTicket_DYJ_Zhi_F(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
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

                string CanonicalNumber = "";

                ArrayList t_al = new ArrayList();

                if (PlayTypeID == PlayType_3_ZhiF)
                {
                    for (int i = 0; i < strs.Length; i++)
                    {
                        string[] SingleNumber = ToSingle_3(strs[i].Split('|')[0], ref CanonicalNumber);

                        for (int j = 0; j < SingleNumber.Length; j++)
                        {
                            t_al.Add(SingleNumber[j]);
                        }
                    }
                }
                if (PlayTypeID == PlayType_3_ZhiH)
                {
                    for (int i = 0; i < strs.Length; i++)
                    {
                        string[] SingleNumber = ToSingle_ZhiH(strs[i].Split('|')[0], ref CanonicalNumber);

                        for (int j = 0; j < SingleNumber.Length; j++)
                        {
                            t_al.Add(SingleNumber[j]);
                        }
                    }
                }

                ArrayList al = new ArrayList();

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

                    for (int i = 0; i < t_al.Count; i += 5)
                    {
                        string Numbers = "";
                        EachMoney = 0;

                        for (int M = 0; M < 5; M++)
                        {
                            if ((i + M) < t_al.Count)
                            {
                                if (t_al[i + M].ToString().Length < 2)
                                {
                                    continue;
                                }

                                Numbers += t_al[i + M].ToString() + "\n";
                                EachMoney = 2 * (M + 1);
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
            private Ticket[] ToElectronicTicket_DYJ_Zu_D(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
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
                                if (strs[i + M].ToString().Split('|').Length < 2)
                                {
                                    continue;
                                }

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
            private Ticket[] ToElectronicTicket_DYJ_Zu_F(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
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

            private string ConvertFormatToElectronTicket_DYJ(int PlayTypeID, string Number)
            {
                Number = Number.Trim();

                string CanonicalNumber = "";

                string Ticket = "";

                if ((PlayTypeID == PlayType_3_ZhiD) || (PlayTypeID == PlayType_3_ZuD) || (PlayTypeID == PlayType_3_ZhiF) || (PlayTypeID == PlayType_3_ZhiH))
                {
                    string[] strs = Number.Split('\n');

                    for (int i = 0; i < strs.Length; i++)
                    {
                        if (PlayTypeID == PlayType_3_ZhiD)
                        {
                            Ticket += "1|";
                        }
                        else if (PlayTypeID == PlayType_3_ZuD)
                        {
                            Ticket += "6|";
                        }
                        else if (PlayTypeID == PlayType_3_ZhiF)
                        {
                            Ticket += "1|";
                        }
                        else if (PlayTypeID == PlayType_3_ZhiH)
                        {
                            Ticket += "s1|";
                        }

                        for (int j = 0; j < strs[i].Length; j++)
                        {
                            if (PlayTypeID == PlayType_3_ZuD)
                            {
                                strs[i] = Sort(strs[i]);
                            }

                            Ticket += strs[i].Substring(j, 1) + ",";
                        }

                        if (Ticket.EndsWith(","))
                        {
                            Ticket = Ticket.Substring(0, Ticket.Length - 1);
                        }

                        Ticket = Ticket + ";\n";
                    }

                    if (Ticket.EndsWith("\n"))
                    {
                        Ticket = Ticket.Substring(0, Ticket.Length - 1);
                    }
                }

                if (PlayTypeID == PlayType_3_Zu6F)
                {
                    string[] SingleNumber = ToSingle_Zu3D_Zu6(Number, ref CanonicalNumber);

                    Ticket = "F6|" + GetFormateOfElectronTicket(SingleNumber) + ";";
                }

                if (PlayTypeID == PlayType_3_Zu3F)
                {
                    string[] SingleNumber = ToSingle_Zu3F(Number, ref CanonicalNumber);

                    Ticket = "F3|" + GetFormateOfElectronTicket(SingleNumber) + ";";
                }

                if (PlayTypeID == PlayType_3_ZuH)
                {
                    string[] SingleNumber = ToSingle_ZuH(Number, ref CanonicalNumber);

                    Ticket = "6|" + GetFormateOfElectronTicket(SingleNumber) + ";";
                }

                return Ticket;
            }
            #endregion

            public override Ticket[] ToElectronicTicket_BJCP(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                if (PlayTypeID == PlayType_3_ZhiD)
                {
                    return ToElectronicTicket_BJCP_ZhiD(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                if (PlayTypeID == PlayType_3_ZhiF)
                {
                    return ToElectronicTicket_BJCP_ZhiF(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                if (PlayTypeID == PlayType_3_ZhiH)
                {
                    return ToElectronicTicket_BJCP_ZhiH(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                if (PlayTypeID == PlayType_3_ZuD)
                {
                    return ToElectronicTicket_BJCP_ZuD(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                if (PlayTypeID == PlayType_3_Zu3F)
                {
                    return ToElectronicTicket_BJCP_Zu3F(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                if (PlayTypeID == PlayType_3_Zu6F)
                {
                    return ToElectronicTicket_BJCP_Zu6F(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                if (PlayTypeID == PlayType_3_ZuH)
                {
                    return ToElectronicTicket_BJCP_ZuH(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                return null;
            }
            #region ToElectronicTicket_BJCP 的具体方法
            private Ticket[] ToElectronicTicket_BJCP_ZhiD(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
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
                                EachMoney += 2 * 1;
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
            private Ticket[] ToElectronicTicket_BJCP_ZhiF(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
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
            private Ticket[] ToElectronicTicket_BJCP_ZhiH(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
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
            private Ticket[] ToElectronicTicket_BJCP_ZuD(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
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
                                EachMoney += 2 * 1;
                            }
                        }

                        Money += EachMoney * EachMultiple;

                        al.Add(new Ticket(3, ConvertFormatToElectronTicket_BJCP(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));
                    }
                }

                Ticket[] Tickets = new Ticket[al.Count];

                for (int i = 0; i < Tickets.Length; i++)
                {
                    Tickets[i] = (Ticket)al[i];
                }

                return Tickets;
            }
            private Ticket[] ToElectronicTicket_BJCP_Zu3F(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
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

                        al.Add(new Ticket(5, ConvertFormatToElectronTicket_BJCP(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));
                    }
                }

                Ticket[] Tickets = new Ticket[al.Count];

                for (int i = 0; i < Tickets.Length; i++)
                {
                    Tickets[i] = (Ticket)al[i];
                }

                return Tickets;
            }
            private Ticket[] ToElectronicTicket_BJCP_Zu6F(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
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

                        al.Add(new Ticket(4, ConvertFormatToElectronTicket_BJCP(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));
                    }
                }

                Ticket[] Tickets = new Ticket[al.Count];

                for (int i = 0; i < Tickets.Length; i++)
                {
                    Tickets[i] = (Ticket)al[i];
                }

                return Tickets;
            }
            private Ticket[] ToElectronicTicket_BJCP_ZuH(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
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

                        al.Add(new Ticket(6, ConvertFormatToElectronTicket_BJCP(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));
                    }
                }

                Ticket[] Tickets = new Ticket[al.Count];

                for (int i = 0; i < Tickets.Length; i++)
                {
                    Tickets[i] = (Ticket)al[i];
                }

                return Tickets;
            }

            private string ConvertFormatToElectronTicket_BJCP(int PlayTypeID, string Number)
            {
                Number = Number.Trim();

                string Ticket = "";

                if (PlayTypeID == PlayType_3_ZhiD)
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
                else if (PlayTypeID == PlayType_3_Zu6F || PlayTypeID == PlayType_3_Zu3F || PlayTypeID == PlayType_3_ZuH || PlayTypeID == PlayType_3_ZhiH || PlayTypeID == PlayType_3_ZuD)
                {
                    Ticket = Number;
                }
                else if (PlayTypeID == PlayType_3_ZhiF)
                {
                    string[] Locate = new string[3];

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

                        Ticket += Locate[i] + "*";
                    }
                }

                if (Ticket.EndsWith("*"))
                    Ticket = Ticket.Substring(0, Ticket.Length - 1);

                return Ticket;
            }
            #endregion

            public override Ticket[] ToElectronicTicket_CTTCSD(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                if (PlayTypeID == PlayType_3_ZhiD)
                {
                    return ToElectronicTicket_CTTCSD_Zhi_D(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                if (PlayTypeID == PlayType_3_ZhiF)
                {
                    return ToElectronicTicket_CTTCSD_Zhi_F(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                if (PlayTypeID == PlayType_3_ZhiH)
                {
                    return ToElectronicTicket_CTTCSD_Zhi_H(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                if (PlayTypeID == PlayType_3_ZuD)
                {
                    return ToElectronicTicket_CTTCSD_Zu_D(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                if (PlayTypeID == PlayType_3_Zu3F)
                {
                    return ToElectronicTicket_CTTCSD_Zu3_F(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                if (PlayTypeID == PlayType_3_Zu6F)
                {
                    return ToElectronicTicket_CTTCSD_Zu6_F(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                if (PlayTypeID == PlayType_3_ZuH)
                {
                    return ToElectronicTicket_CTTCSD_Zu_H(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                return null;
            }
            #region ToElectronicTicket_CTTCSD 的具体方法
            private Ticket[] ToElectronicTicket_CTTCSD_Zhi_D(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
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
                                EachMoney += 2 * 1;
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
            private Ticket[] ToElectronicTicket_CTTCSD_Zhi_F(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
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

                        Numbers = "*" + strs[i].ToString().Split('|')[0];
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
            private Ticket[] ToElectronicTicket_CTTCSD_Zhi_H(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
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

                        Numbers = "**" + strs[i].ToString().Split('|')[0].PadLeft(2, '0');
                        EachMoney += 2 * double.Parse(strs[i].ToString().Split('|')[1]);

                        Money += EachMoney * EachMultiple;

                        al.Add(new Ticket(2, ConvertFormatToElectronTicket_CTTCSD(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));
                    }
                }

                Ticket[] Tickets = new Ticket[al.Count];

                for (int i = 0; i < Tickets.Length; i++)
                {
                    Tickets[i] = (Ticket)al[i];
                }

                return Tickets;
            }
            private Ticket[] ToElectronicTicket_CTTCSD_Zu_D(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
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
                                EachMoney += 2 * 1;
                            }
                        }

                        Money += EachMoney * EachMultiple;

                        al.Add(new Ticket(3, ConvertFormatToElectronTicket_CTTCSD(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));
                    }
                }

                Ticket[] Tickets = new Ticket[al.Count];

                for (int i = 0; i < Tickets.Length; i++)
                {
                    Tickets[i] = (Ticket)al[i];
                }

                return Tickets;
            }
            private Ticket[] ToElectronicTicket_CTTCSD_Zu3_F(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
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

                        Numbers = "**" + strs[i].ToString().Split('|')[0];
                        EachMoney += 2 * double.Parse(strs[i].ToString().Split('|')[1]);

                        Money += EachMoney * EachMultiple;

                        al.Add(new Ticket(5, ConvertFormatToElectronTicket_CTTCSD(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));
                    }
                }

                Ticket[] Tickets = new Ticket[al.Count];

                for (int i = 0; i < Tickets.Length; i++)
                {
                    Tickets[i] = (Ticket)al[i];
                }

                return Tickets;
            }
            private Ticket[] ToElectronicTicket_CTTCSD_Zu6_F(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
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

                        Numbers = "**" + strs[i].ToString().Split('|')[0];
                        EachMoney += 2 * double.Parse(strs[i].ToString().Split('|')[1]);

                        Money += EachMoney * EachMultiple;

                        al.Add(new Ticket(4, ConvertFormatToElectronTicket_CTTCSD(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));
                    }
                }

                Ticket[] Tickets = new Ticket[al.Count];

                for (int i = 0; i < Tickets.Length; i++)
                {
                    Tickets[i] = (Ticket)al[i];
                }

                return Tickets;
            }
            private Ticket[] ToElectronicTicket_CTTCSD_Zu_H(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
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

                        Numbers = "**" + strs[i].ToString().Split('|')[0].PadLeft(2, '0');
                        EachMoney += 2 * double.Parse(strs[i].ToString().Split('|')[1]);

                        Money += EachMoney * EachMultiple;

                        al.Add(new Ticket(6, ConvertFormatToElectronTicket_CTTCSD(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));
                    }
                }

                Ticket[] Tickets = new Ticket[al.Count];

                for (int i = 0; i < Tickets.Length; i++)
                {
                    Tickets[i] = (Ticket)al[i];
                }

                return Tickets;
            }

            private string ConvertFormatToElectronTicket_CTTCSD(int PlayTypeID, string Number)
            {
                Number = Number.Trim();

                string Ticket = "";

                string[] strs = Number.Split('\n');

                for (int i = 0; i < strs.Length; i++)
                {
                    if (PlayTypeID == PlayType_3_Zu3F || PlayTypeID == PlayType_3_Zu6F)
                    {
                        Ticket += Sort(strs[i]);
                    }

                    if (PlayTypeID == PlayType_3_ZhiH || PlayTypeID == PlayType_3_ZuH)
                    {
                        Ticket += strs[i];
                    }

                    if (PlayTypeID == PlayType_3_ZhiD || PlayTypeID == PlayType_3_ZuD)
                    {
                        for (int j = 0; j < strs[i].Length; j++)
                        {
                            Ticket += strs[i].Substring(j, 1) + "*";
                        }

                        if (Ticket.EndsWith("*"))
                        {
                            Ticket = Ticket.Substring(0, Ticket.Length - 1);
                        }
                    }

                    if (PlayTypeID == PlayType_3_ZhiF)
                    {
                        string[] Locate = new string[3];

                        Regex regex = new Regex(@"^(?<L0>(\d)|([(][\d]+?[)]))(?<L1>(\d)|([(][\d]+?[)]))(?<L2>(\d)|([(][\d]+?[)]))", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                        Match m = regex.Match(strs[i]);

                        for (int j = 0; j < 3; j++)
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
                    }

                    if (Ticket.EndsWith("*"))
                    {
                        Ticket = Ticket.Substring(0, Ticket.Length - 1);
                    }

                    Ticket = Ticket + "\n";
                }

                if (Ticket.EndsWith("\n"))
                {
                    Ticket = Ticket.Substring(0, Ticket.Length - 1);
                }

                return Ticket;
            }
            #endregion

            #region GetFormateOfElectronTicket 转换成电子票单式模式
            private string GetFormateOfElectronTicket(string[] SingleNumber)
            {
                string ticket = "";

                string[] TempTicket = new String[SingleNumber.Length];

                for (int p = 0; p < SingleNumber.Length; p++)
                {
                    for (int q = 0; q < SingleNumber[p].Length; q++)
                    {
                        TempTicket[p] += SingleNumber[p].Substring(q, 1) + ",";
                    }

                    ticket += TempTicket[p].Substring(0, TempTicket[p].Length - 1) + "\n";
                }

                if (ticket.EndsWith("\n"))
                {
                    ticket = ticket.Substring(0, ticket.Length - 1);
                }

                return ticket;
            }
            #endregion
        }
    }
}
