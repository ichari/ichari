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
        /// 齐鲁风采23选5
        /// </summary>
        public partial class QLFC23X5 : LotteryBase
        {
            #region 静态变量
            public const int PlayType_D = 2701;
            public const int PlayType_F = 2702;

            public const int PlayType_HYC2_D = 2703;
            public const int PlayType_HYC2_F = 2704;


            public const int ID = 27;
            public const string sID = "27";
            public const string Name = "齐鲁风采23选5";
            public const string Code = "QLFC23X5";
            public const double MaxMoney = 12376;
            #endregion

            public QLFC23X5()
            {
                id = 27;
                name = "齐鲁风采23选5";
                code = "QLFC23X5";
            }

            public override bool CheckPlayType(int play_type)
            {
                return ((play_type >= 2701) && (play_type <= 2704));
            }

            public override PlayType[] GetPlayTypeList()
            {
                PlayType[] Result = new PlayType[4];

                Result[0] = new PlayType(PlayType_D, "单式");
                Result[1] = new PlayType(PlayType_F, "复式");
                Result[2] = new PlayType(PlayType_HYC2_D, "好运二单式");
                Result[3] = new PlayType(PlayType_HYC2_F, "好运二复式");

                return Result;
            }

            public override string BuildNumber(int Num)	//id = 27
            {
                System.Random rd = new Random();
                StringBuilder sb = new StringBuilder();
                ArrayList al = new ArrayList();

                for (int i = 0; i < Num; i++)
                {
                    al.Clear();
                    for (int j = 0; j < 5; j++)
                    {
                        int Ball = 0;
                        while ((Ball == 0) || isExistBall(al, Ball))
                            Ball = rd.Next(1, 23 + 1);
                        al.Add(Ball.ToString().PadLeft(2, '0'));
                    }

                    CompareToAscii compare = new CompareToAscii();
                    al.Sort(compare);

                    string LotteryNumber = "";
                    for (int j = 0; j < al.Count; j++)
                        LotteryNumber += al[j].ToString() + " ";

                    sb.Append(LotteryNumber.Trim() + "\n");
                }

                string Result = sb.ToString();
                Result = Result.Substring(0, Result.Length - 1);
                return Result;
            }

            public override string[] ToSingle(string Number, ref string CanonicalNumber, int PlayType)	//复式取单式, 后面 ref 参数是将彩票规范化，如：01 01 02... 变成 01 02...
            {
                if ((PlayType == PlayType_D) || (PlayType == PlayType_F))
                    return ToSingle_Zhi(Number, ref CanonicalNumber);

                if ((PlayType == PlayType_HYC2_D) || (PlayType == PlayType_HYC2_F))
                    return ToSingle_HYC2(Number, ref CanonicalNumber);

                return null;
            }
            #region ToSingle 的具体方法
            private string[] ToSingle_Zhi(string Number, ref string CanonicalNumber)
            {
                string[] strs = FilterRepeated(Number.Trim().Split(' '));
                CanonicalNumber = "";

                if (strs.Length < 5)
                {
                    CanonicalNumber = "";
                    return null;
                }

                for (int i = 0; i < strs.Length; i++)
                    CanonicalNumber += (strs[i] + " ");
                CanonicalNumber = CanonicalNumber.Trim();

                ArrayList al = new ArrayList();

                #region 循环取单式

                int n = strs.Length;
                for (int i = 0; i < n - 4; i++)
                    for (int j = i + 1; j < n - 3; j++)
                        for (int k = j + 1; k < n - 2; k++)
                            for (int x = k + 1; x < n - 1; x++)
                                for (int y = x + 1; y < n; y++)
                                    al.Add(strs[i] + " " + strs[j] + " " + strs[k] + " " + strs[x] + " " + strs[y]);

                #endregion

                string[] Result = new string[al.Count];
                for (int i = 0; i < al.Count; i++)
                    Result[i] = al[i].ToString();
                return Result;
            }
            private string[] ToSingle_HYC2(string Number, ref string CanonicalNumber)
            {
                string[] strs = FilterRepeated(Number.Trim().Split(' '));
                CanonicalNumber = "";

                if (strs.Length < 2)
                {
                    CanonicalNumber = "";
                    return null;
                }

                for (int i = 0; i < strs.Length; i++)
                    CanonicalNumber += (strs[i] + " ");
                CanonicalNumber = CanonicalNumber.Trim();

                ArrayList al = new ArrayList();

                #region 循环取单式

                int n = strs.Length;
                for (int i = 0; i < n - 1; i++)
                    for (int j = i + 1; j < n; j++)
                        al.Add(strs[i] + " " + strs[j]);

                #endregion

                string[] Result = new string[al.Count];
                for (int i = 0; i < al.Count; i++)
                    Result[i] = al[i].ToString();
                return Result;
            }
            #endregion

            public override double ComputeWin(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, int PlayType, params double[] WinMoneyList)
            {
                if ((WinMoneyList == null) || (WinMoneyList.Length < 8))   //奖金顺序 1-3等奖，好运彩2奖
                    return -3;

                if ((PlayType == PlayType_D) || (PlayType == PlayType_F))
                    return ComputeWin_Zhi(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[0], WinMoneyList[1], WinMoneyList[2], WinMoneyList[3], WinMoneyList[4], WinMoneyList[5]);

                if ((PlayType == PlayType_HYC2_D) || (PlayType == PlayType_HYC2_F))
                    return ComputeWin_HYC2(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[6], WinMoneyList[7]);

                return -4;
            }
            #region ComputeWin 的具体方法
            private double ComputeWin_Zhi(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney1, double WinMoneyNoWithTax1, double WinMoney2, double WinMoneyNoWithTax2, double WinMoney3, double WinMoneyNoWithTax3)	//计算中奖
            {
                WinNumber = WinNumber.Trim();
                if (WinNumber.Length < 14)	//14: "01 02 03 04 05"
                    return -1;
                string[] Lotterys = SplitLotteryNumber(Number);
                if (Lotterys == null)
                    return -2;
                if (Lotterys.Length < 1)
                    return -2;

                int Description1 = 0, Description2 = 0, Description3 = 0;
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
                        if (Lottery[i].Length < 14)
                            continue;

                        string[] Red = new string[5];
                        Regex regex = new Regex(@"^(?<R0>\d\d\s)(?<R1>\d\d\s)(?<R2>\d\d\s)(?<R3>\d\d\s)(?<R4>\d\d)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                        Match m = regex.Match(Lottery[i]);
                        int j;
                        int RedRight = 0;
                        bool Full = true;
                        for (j = 0; j < 5; j++)
                        {
                            Red[j] = m.Groups["R" + j.ToString()].ToString().Trim();
                            if (Red[j] == "")
                            {
                                Full = false;
                                break;
                            }
                            if (WinNumber.IndexOf(Red[j]) >= 0)
                                RedRight++;
                        }
                        if (!Full)
                            continue;

                        if (RedRight == 5)
                        {
                            Description1++;
                            WinMoney += WinMoney1;
                            WinMoneyNoWithTax += WinMoneyNoWithTax1;
                            continue;
                        }
                        if (RedRight == 4)
                        {
                            Description2++;
                            WinMoney += WinMoney2;
                            WinMoneyNoWithTax += WinMoneyNoWithTax2;
                            continue;
                        }
                        if (RedRight == 3)
                        {
                            Description3++;
                            WinMoney += WinMoney3;
                            WinMoneyNoWithTax += WinMoneyNoWithTax3;
                            continue;
                        }
                    }
                }

                if (Description1 > 0)
                    Description = "一等奖" + Description1.ToString() + "注";
                if (Description2 > 0)
                {
                    if (Description != "")
                        Description += "，";
                    Description += "二等奖" + Description2.ToString() + "注";
                }
                if (Description3 > 0)
                {
                    if (Description != "")
                        Description += "，";
                    Description += "三等奖" + Description3.ToString() + "注";
                }
                if (Description != "")
                    Description += "。";

                return WinMoney;
            }
            private double ComputeWin_HYC2(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney1, double WinMoneyNoWithTax1)	//计算中奖
            {
                WinNumber = WinNumber.Trim();
                if (WinNumber.Length < 14)	//14: "01 02 03 04 05"
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
                    string[] Lottery = ToSingle_HYC2(Lotterys[ii], ref t_str);
                    if (Lottery == null)
                        continue;
                    if (Lottery.Length < 1)
                        continue;

                    for (int i = 0; i < Lottery.Length; i++)
                    {
                        if (Lottery[i].Length < 5)	//5: "01 02"
                            continue;

                        string[] Red = new string[2];
                        Regex regex = new Regex(@"^(?<R0>\d\d\s)(?<R1>\d\d)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                        Match m = regex.Match(Lottery[i]);

                        int RedRight = 0;
                        bool Full = true;
                        for (int j = 0; j < 2; j++)
                        {
                            Red[j] = m.Groups["R" + j.ToString()].ToString().Trim();
                            if (Red[j] == "")
                            {
                                Full = false;
                                break;
                            }
                            if (WinNumber.IndexOf(Red[j]) >= 0)
                                RedRight++;
                        }
                        if (!Full)
                            continue;

                        if (RedRight == 2)
                        {
                            Description1++;
                            WinMoney += WinMoney1;
                            WinMoneyNoWithTax += WinMoneyNoWithTax1;
                        }
                    }
                }

                if (Description1 > 0)
                    Description = "好运二奖" + Description1.ToString() + "注";

                if (Description != "")
                    Description += "。";

                return WinMoney;
            }
            #endregion

            public override string AnalyseScheme(string Content, int PlayType)
            {
                if ((PlayType == PlayType_D) || (PlayType == PlayType_F))
                    return AnalyseScheme_Zhi(Content, PlayType);

                if ((PlayType == PlayType_HYC2_D) || (PlayType == PlayType_HYC2_F))
                    return AnalyseScheme_HYC2(Content, PlayType);

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
                if (PlayType == PlayType_D)
                    RegexString = @"^(\d\d\s){4}\d\d";
                else
                    RegexString = @"^(\d\d\s){4,22}\d\d";
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
                        if ((singles.Length >= ((PlayType == PlayType_D) ? 1 : 2)) && (singles.Length <= (MaxMoney / 2)))
                            Result += CanonicalNumber + "|" + singles.Length.ToString() + "\n";
                    }
                }

                if (Result.EndsWith("\n"))
                    Result = Result.Substring(0, Result.Length - 1);
                return Result;
            }
            private string AnalyseScheme_HYC2(string Content, int PlayType)
            {
                string[] strs = Content.Split('\n');
                if (strs == null)
                    return "";
                if (strs.Length == 0)
                    return "";

                string Result = "";

                string RegexString;
                if (PlayType == PlayType_HYC2_D)
                    RegexString = @"^(\d\d\s\d\d)";
                else
                    RegexString = @"^(\d\d\s){1,22}\d\d";
                Regex regex = new Regex(RegexString, RegexOptions.Compiled | RegexOptions.IgnoreCase);

                for (int i = 0; i < strs.Length; i++)
                {
                    Match m = regex.Match(strs[i]);
                    if (m.Success)
                    {
                        string CanonicalNumber = "";
                        string[] singles = ToSingle_HYC2(m.Value, ref CanonicalNumber);
                        if (singles == null)
                            continue;
                        if ((singles.Length >= ((PlayType == PlayType_HYC2_D) ? 1 : 2)) && (singles.Length <= (MaxMoney / 2)))
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

            private string[] FilterRepeated(string[] NumberPart)
            {
                ArrayList al = new ArrayList();
                for (int i = 0; i < NumberPart.Length; i++)
                {
                    int Ball = Shove._Convert.StrToInt(NumberPart[i], -1);
                    if ((Ball >= 1) && (Ball <= 23) && (!isExistBall(al, Ball)))
                        al.Add(NumberPart[i]);
                }

                CompareToAscii compare = new CompareToAscii();
                al.Sort(compare);

                string[] Result = new string[al.Count];
                for (int i = 0; i < al.Count; i++)
                    Result[i] = al[i].ToString().PadLeft(2, '0');

                return Result;
            }

            public override string ShowNumber(string Number)
            {
                return base.ShowNumber(Number, "");
            }
        }
    }
}