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
        /// 龙江P62
        /// </summary>
        public partial class LJP62 : LotteryBase
        {
            #region 静态变量
            public const int PlayType_D = 801;
            public const int PlayType_F = 802;

            public const int ID = 8;
            public const string sID = "8";
            public const string Name = "龙江P62";
            public const string Code = "LJP62";
            public const double MaxMoney = 20000;
            #endregion

            public LJP62()
            {
                id = 8;
                name = "龙江P62";
                code = "LJP62";
            }

            public override bool CheckPlayType(int play_type)
            {
                return ((play_type >= 801) && (play_type <= 802));
            }

            public override PlayType[] GetPlayTypeList()
            {
                PlayType[] Result = new PlayType[2];

                Result[0] = new PlayType(PlayType_D, "单式");
                Result[1] = new PlayType(PlayType_F, "复式");

                return Result;
            }

            public override string BuildNumber(int Num)	//id = 8
            {
                System.Random rd = new Random();
                StringBuilder sb = new StringBuilder();

                for (int i = 0; i < Num; i++)
                {
                    string LotteryNumber = "";
                    for (int j = 0; j < 6; j++)
                        LotteryNumber += rd.Next(0, 9 + 1).ToString();
                    LotteryNumber += "+" + rd.Next(0, 1 + 1).ToString();

                    sb.Append(LotteryNumber.Trim() + "\n");
                }

                string Result = sb.ToString();
                Result = Result.Substring(0, Result.Length - 1);
                return Result;
            }

            public override string[] ToSingle(string Number, ref string CanonicalNumber, int PlayType)	//复式取单式, 后面 ref 参数是将彩票规范化，如：103(00)... 变成1030...
            {
                string[] strs = Number.Split('+');
                CanonicalNumber = "";

                if (strs.Length != 2)
                {
                    CanonicalNumber = "";
                    return null;
                }

                string[] Red = new string[6];
                Regex regex = new Regex(@"^(?<L0>(\d)|([(][\d]+?[)]))(?<L1>(\d)|([(][\d]+?[)]))(?<L2>(\d)|([(][\d]+?[)]))(?<L3>(\d)|([(][\d]+?[)]))(?<L4>(\d)|([(][\d]+?[)]))(?<L5>(\d)|([(][\d]+?[)]))", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                Match m = regex.Match(strs[0].Trim());
                for (int i = 0; i < 6; i++)
                {
                    Red[i] = m.Groups["L" + i.ToString()].ToString().Trim();
                    if (Red[i] == "")
                    {
                        CanonicalNumber = "";
                        return null;
                    }
                    if (Red[i].Length > 1)
                    {
                        Red[i] = Red[i].Substring(1, Red[i].Length - 2);
                        if (Red[i].Length > 1)
                            Red[i] = FilterRepeated(Red[i]);
                        if (Red[i] == "")
                        {
                            CanonicalNumber = "";
                            return null;
                        }
                    }
                    if (Red[i].Length > 1)
                        CanonicalNumber += "(" + Red[i] + ")";
                    else
                        CanonicalNumber += Red[i];
                }

                CanonicalNumber += "+";

                string Blue = strs[1].Trim();
                if (Blue.Length > 1)
                {
                    Blue = Blue.Substring(1, Blue.Length - 2);
                    if (Blue.Length > 1)
                        Blue = FilterRepeated(Blue);
                    if (Blue == "")
                    {
                        CanonicalNumber = "";
                        return null;
                    }
                    if (Blue.Length > 2)
                    {
                        CanonicalNumber = "";
                        return null;
                    }
                }
                for (int i = 0; i < Blue.Length; i++)
                {
                    if ((Blue[i] != '0') && (Blue[i] != '1'))
                    {
                        CanonicalNumber = "";
                        return null;
                    }
                }
                if (Blue.Length > 1)
                    CanonicalNumber += "(" + Blue + ")";
                else
                    CanonicalNumber += Blue;

                ArrayList al = new ArrayList();

                #region 循环取单式

                for (int i_0 = 0; i_0 < Red[0].Length; i_0++)
                {
                    string str_0 = Red[0][i_0].ToString();
                    for (int i_1 = 0; i_1 < Red[1].Length; i_1++)
                    {
                        string str_1 = str_0 + Red[1][i_1].ToString();
                        for (int i_2 = 0; i_2 < Red[2].Length; i_2++)
                        {
                            string str_2 = str_1 + Red[2][i_2].ToString();
                            for (int i_3 = 0; i_3 < Red[3].Length; i_3++)
                            {
                                string str_3 = str_2 + Red[3][i_3].ToString();
                                for (int i_4 = 0; i_4 < Red[4].Length; i_4++)
                                {
                                    string str_4 = str_3 + Red[4][i_4].ToString();
                                    for (int i_5 = 0; i_5 < Red[5].Length; i_5++)
                                    {
                                        string str_5 = str_4 + Red[5][i_5].ToString();
                                        for (int j = 0; j < Blue.Length; j++)
                                            al.Add(str_5 + "+" + Blue[j].ToString());
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
                if (WinNumber.Length < 8)	//8: 123456+1
                    return -1;
                string[] Lotterys = SplitLotteryNumber(Number);
                if (Lotterys == null)
                    return -2;
                if (Lotterys.Length < 1)
                    return -2;
                if ((WinMoneyList == null) || (WinMoneyList.Length < 12))
                    return -3;

                int Description1 = 0, Description2 = 0, Description3 = 0, Description4 = 0, Description5 = 0, Description6 = 0;
                double WinMoney = 0;
                WinMoneyNoWithTax = 0;
                Description = "";

                for (int ii = 0; ii < Lotterys.Length; ii++)
                {
                    string t_str = "";
                    string[] Lottery = ToSingle(Lotterys[ii], ref t_str, PlayType);
                    if (Lottery == null)
                        continue;
                    if (Lottery.Length < 1)
                        continue;

                    for (int i = 0; i < Lottery.Length; i++)
                    {
                        if (Lottery[i].Length < 8)
                            continue;
                        if (Lottery[i] == WinNumber)
                        {
                            Description1++;
                            WinMoney += WinMoneyList[0];
                            WinMoneyNoWithTax += WinMoneyList[1];
                            continue;
                        }

                        if (Lottery[i].Substring(0, 6) == WinNumber.Substring(0, 6))
                        {
                            Description2++;
                            WinMoney += WinMoneyList[2];
                            WinMoneyNoWithTax += WinMoneyList[3];
                            continue;
                        }

                        int j;
                        bool End = false;
                        for (j = 0; j <= 1; j++)
                        {
                            if (Lottery[i].Substring(j, 5) == WinNumber.Substring(j, 5))
                            {
                                Description3++;
                                WinMoney += WinMoneyList[4];
                                WinMoneyNoWithTax += WinMoneyList[5];
                                End = true;
                                break;
                            }
                        }
                        if (End)
                            continue;

                        for (j = 0; j <= 2; j++)
                        {
                            if (Lottery[i].Substring(j, 4) == WinNumber.Substring(j, 4))
                            {
                                Description4++;
                                WinMoney += WinMoneyList[6];
                                WinMoneyNoWithTax += WinMoneyList[7];
                                End = true;
                                break;
                            }
                        }
                        if (End)
                            continue;

                        for (j = 0; j <= 3; j++)
                        {
                            if (Lottery[i].Substring(j, 3) == WinNumber.Substring(j, 3))
                            {
                                Description5++;
                                WinMoney += WinMoneyList[8];
                                WinMoneyNoWithTax += WinMoneyList[9];
                                End = true;
                                break;
                            }
                        }
                        if (End)
                            continue;

                        int Right = 0;
                        for (j = 0; j < 6; j++)
                        {
                            if (Lottery[i][j] == WinNumber[j])
                                Right++;
                        }
                        if (Right >= 2)
                        {
                            Description6++;
                            WinMoney += WinMoneyList[10];
                            WinMoneyNoWithTax += WinMoneyList[11];
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
                if (Description4 > 0)
                {
                    if (Description != "")
                        Description += "，";
                    Description += "四等奖" + Description4.ToString() + "注";
                }
                if (Description5 > 0)
                {
                    if (Description != "")
                        Description += "，";
                    Description += "五等奖" + Description5.ToString() + "注";
                }
                if (Description6 > 0)
                {
                    if (Description != "")
                        Description += "，";
                    Description += "六等奖" + Description6.ToString() + "注";
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

                string RegexString;
                if (PlayType == PlayType_D)
                    RegexString = @"^(\d){6}[+][01]";
                else
                    RegexString = @"^((\d)|([(]\d{1,10}[)])){6}[+](([01])|([(]([01]){1,2}[)]))";
                Regex regex = new Regex(RegexString, RegexOptions.Compiled | RegexOptions.IgnoreCase);

                for (int i = 0; i < strs.Length; i++)
                {
                    Match m = regex.Match(strs[i]);
                    if (m.Success)
                    {
                        string CanonicalNumber = "";
                        string[] singles = ToSingle(m.Value, ref CanonicalNumber, PlayType);
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

            public override bool AnalyseWinNumber(string Number)
            {
                Regex regex = new Regex(@"([0123456789]){6}[+][01]", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                if (!regex.IsMatch(Number))
                    return false;

                string t_str = "";
                string[] WinLotteryNumber = ToSingle(Number, ref t_str, PlayType_D);

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
        }
    }
}
