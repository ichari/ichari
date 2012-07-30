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
        /// 体彩29选7
        /// </summary>
        public partial class TC31X7 : LotteryBase
        {
            #region 静态变量
            public const int PlayType_D = 6501;
            public const int PlayType_F = 6502;
            public const int PlayType_DanT = 6503;

            public const int ID = 65;
            public const string sID = "65";
            public const string Name = "体彩31选7";
            public const string Code = "TC31X7";
            public const double MaxMoney = 20000;
            #endregion

            public TC31X7()
            {
                id = 65;
                name = "体彩31选7";
                code = "TC31X7";
            }

            public override bool CheckPlayType(int play_type)
            {
                return ((play_type >= 6501) && (play_type <= 6503));
            }

            public override PlayType[] GetPlayTypeList()
            {
                PlayType[] Result = new PlayType[3];

                Result[0] = new PlayType(PlayType_D, "单式");
                Result[1] = new PlayType(PlayType_F, "复式");
                Result[2] = new PlayType(PlayType_DanT, "胆托");

                return Result;
            }

            public override string BuildNumber(int Num)	//id = 14
            {
                System.Random rd = new Random();
                StringBuilder sb = new StringBuilder();
                ArrayList al = new ArrayList();

                for (int i = 0; i < Num; i++)
                {
                    al.Clear();
                    int Ball;
                    for (int j = 0; j < 7; j++)
                    {
                        Ball = 0;
                        while ((Ball == 0) || isExistBall(al, Ball))
                            Ball = rd.Next(1, 31 + 1);
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

                if (PlayType == PlayType_DanT)
                    return ToSingle_DanT(Number, ref CanonicalNumber);

                return null;
            }

            #region ToSingle 的具体方法
            private string[] ToSingle_Zhi(string Number, ref string CanonicalNumber)	//复式取单式, 后面 ref 参数是将彩票规范化，如：01 01 02... 变成 01 02...
            {
                string[] strs = FilterRepeated(Number.Trim().Split(' '));
                CanonicalNumber = "";

                if (strs.Length < 7)
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
                for (int i = 0; i < n - 6; i++)
                    for (int j = i + 1; j < n - 5; j++)
                        for (int k = j + 1; k < n - 4; k++)
                            for (int x = k + 1; x < n - 3; x++)
                                for (int y = x + 1; y < n - 2; y++)
                                    for (int z = y + 1; z < n - 1; z++)
                                        for (int a = z + 1; a < n; a++)
                                            al.Add(strs[i] + " " + strs[j] + " " + strs[k] + " " + strs[x] + " " + strs[y] + " " + strs[z] + " " + strs[a]);

                #endregion

                string[] Result = new string[al.Count];
                for (int i = 0; i < al.Count; i++)
                    Result[i] = al[i].ToString();
                return Result;
            }
            private string[] ToSingle_DanT(string Number, ref string CanonicalNumber)	//胆拖取单式, 后面 ref 参数是将彩票规范化，如：01 01 02... 变成 01 02...
            {
                CanonicalNumber = "";

                string[] RedDan = FilterRepeated(Number.Trim().Split(',')[0].Trim().Split(' '));
                string[] RedTuo = FilterRepeated(Number.Trim().Split(',')[1].Trim().Split(' '));

                if (RedDan.Length + RedTuo.Length < 8)
                {
                    CanonicalNumber = "";
                    return null;
                }

                string Numbers = "";
                for (int i = 0; i < RedDan.Length; i++)
                {
                    Numbers += RedDan[i] + " ";
                }

                ArrayList al = new ArrayList();

                #region 循环取单式

                int m = RedDan.Length;
                int n = RedTuo.Length;

                if (m == 1)
                {
                    for (int j = 0; j < n - 5; j++)
                        for (int k = j + 1; k < n - 4; k++)
                            for (int x = k + 1; x < n - 3; x++)
                                for (int y = x + 1; y < n - 2; y++)
                                    for (int z = y + 1; z < n - 1; z++)
                                        for (int a = z + 1; a < n; a++)
                                            al.Add(Numbers + " " + RedTuo[j] + " " + RedTuo[k] + " " + RedTuo[x] + " " + RedTuo[y] + " " + RedTuo[z] + " " + RedTuo[a]);
                }
                else if (m == 2)
                {
                    for (int k = 0; k < n - 4; k++)
                        for (int x = k + 1; x < n - 3; x++)
                            for (int y = x + 1; y < n - 2; y++)
                                for (int z = y + 1; z < n - 1; z++)
                                    for (int a = z + 1; a < n; a++)
                                        al.Add(Numbers + " " + RedTuo[k] + " " + RedTuo[x] + " " + RedTuo[y] + " " + RedTuo[z] + " " + RedTuo[a]);
                }
                else if (m == 3)
                {
                    for (int x = 0; x < n - 3; x++)
                        for (int y = x + 1; y < n - 2; y++)
                            for (int z = y + 1; z < n - 1; z++)
                                for (int a = z + 1; a < n; a++)
                                    al.Add(Numbers + " " + RedTuo[x] + " " + RedTuo[y] + " " + RedTuo[z] + " " + RedTuo[a]);
                }
                else if (m == 4)
                {
                    for (int y = 0; y < n - 2; y++)
                        for (int z = y + 1; z < n - 1; z++)
                            for (int a = z + 1; a < n; a++)
                                al.Add(Numbers + " " + RedTuo[y] + " " + RedTuo[z] + " " + RedTuo[a]);
                }
                else if (m == 5)
                {
                    for (int z = 0; z < n - 1; z++)
                        for (int a = z + 1; a < n; a++)
                            al.Add(Numbers + " " + RedTuo[z] + " " + RedTuo[a]);
                }
                else if (m == 6)
                {
                    for (int a = 0; a < n; a++)
                        al.Add(Numbers + " " + RedTuo[a]);
                }

                #endregion

                string[] Result = new string[al.Count];
                for (int i = 0; i < al.Count; i++)
                    Result[i] = al[i].ToString();
                return Result;
            }
            #endregion

            public override double ComputeWin(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, int PlayType, params double[] WinMoneyList)
            {
                if ((WinMoneyList == null) || (WinMoneyList.Length < 14))    // 奖金参数排列顺序 一等, 二等, 三等, 四等, 五等, 六等,　七等奖
                    return -3;

                if ((PlayType == PlayType_D) || (PlayType == PlayType_F))
                    return ComputeWin_Zhi(PlayType, Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[0], WinMoneyList[1], WinMoneyList[2], WinMoneyList[3], WinMoneyList[4], WinMoneyList[5], WinMoneyList[6], WinMoneyList[7], WinMoneyList[8], WinMoneyList[9], WinMoneyList[10], WinMoneyList[11], WinMoneyList[12], WinMoneyList[13]);

                if (PlayType == PlayType_DanT)
                    return ComputeWin_DanT(PlayType, Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[0], WinMoneyList[1], WinMoneyList[2], WinMoneyList[3], WinMoneyList[4], WinMoneyList[5], WinMoneyList[6], WinMoneyList[7], WinMoneyList[8], WinMoneyList[9], WinMoneyList[10], WinMoneyList[11], WinMoneyList[12], WinMoneyList[13]);

                return -4;
            }
            #region ComputeWin 的具体方法
            private double ComputeWin_Zhi(int PlayType, string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney1, double WinMoneyNoWithTax1, double WinMoney2, double WinMoneyNoWithTax2, double WinMoney3, double WinMoneyNoWithTax3, double WinMoney4, double WinMoneyNoWithTax4, double WinMoney5, double WinMoneyNoWithTax5, double WinMoney6, double WinMoneyNoWithTax6, double WinMoney7, double WinMoneyNoWithTax7)
            {
                WinNumber = WinNumber.Trim();
                if (WinNumber.Length < 25)	//25: "01 02 03 04 05 06 07 + 08"
                    return -1;
                string[] Lotterys = SplitLotteryNumber(Number);
                if (Lotterys == null)
                    return -2;
                if (Lotterys.Length < 1)
                    return -2;

                string Blue = WinNumber.Substring(23, 2);

                int Description1 = 0, Description2 = 0, Description3 = 0, Description4 = 0, Description5 = 0, Description6 = 0, Description7 = 0;
                double WinMoney = 0;
                WinMoneyNoWithTax = 0;

                Description = "";

                string RegexString;
                if (PlayType == PlayType_D)
                    RegexString = @"^(\d\d\s){6}\d\d";
                else
                    RegexString = @"^(\d\d\s){6,30}\d\d";
                Regex regex = new Regex(RegexString, RegexOptions.Compiled | RegexOptions.IgnoreCase);

                string[] Red = null;

                for (int ii = 0; ii < Lotterys.Length; ii++)
                {
                    Match m = regex.Match(Lotterys[ii]);

                    if (!m.Success)
                    {
                        continue;
                    }

                    Red = FilterRepeated(m.Value.Trim().Split(' '));

                    if (Red.Length < 7)
                    {
                        continue;
                    }

                    int RedRight = 0;
                    bool BlueRight = false;

                    foreach (string str in Red)
                    {
                        if (!string.IsNullOrEmpty(str))
                        {
                            if (WinNumber.IndexOf(str.Trim() + " ") >= 0)
                            {
                                RedRight++;
                            }

                            if (Blue == str.Trim())
                            {
                                BlueRight = true;
                            }
                        }
                    }

                    if (RedRight < 4)
                    {
                        continue;
                    }

                    if (RedRight == 7)
                    {
                        Description1++;
                        WinMoney += WinMoney1;
                        WinMoneyNoWithTax += WinMoneyNoWithTax1;
                    }

                    if ((RedRight == Red.Length) && (RedRight == 7))
                    {
                        continue;
                    }

                    if ((RedRight >= 6) && BlueRight)
                    {
                        int Count = 1;

                        Count = GetNum(6, RedRight);

                        Description2 = Description2 + Count;
                        WinMoney += WinMoney2 * Count;
                        WinMoneyNoWithTax += WinMoneyNoWithTax2 * Count;
                    }

                    if (RedRight >= 6)
                    {
                        if (BlueRight && (Red.Length > 7))
                        {
                            int Count = GetNum(6, RedRight) * GetNum(1, Red.Length - RedRight - 1);

                            Description3 = Description3 + Count;
                            WinMoney += WinMoney3 * Count;
                            WinMoneyNoWithTax += WinMoneyNoWithTax3 * Count;
                        }
                        else if (!BlueRight)
                        {
                            int Count = GetNum(6, RedRight) * GetNum(1, Red.Length - RedRight);

                            Description3 = Description3 + Count;
                            WinMoney += WinMoney3 * Count;
                            WinMoneyNoWithTax += WinMoneyNoWithTax3 * Count;
                        }
                    }

                    if ((RedRight == Red.Length - 1) && (RedRight == 6))
                    {
                        continue;
                    }

                    if ((RedRight >= 5) && BlueRight)
                    {
                        int Count = 1;

                        if (Red.Length - RedRight > 1)
                        {
                            Count = GetNum(5, RedRight) * GetNum(1, Red.Length - RedRight - 1);
                        }

                        Description4 = Description4 + Count;
                        WinMoney += WinMoney4 * Count;
                        WinMoneyNoWithTax += WinMoneyNoWithTax4 * Count;
                    }

                    if (RedRight >= 5)
                    {
                        if (BlueRight && (Red.Length > 7) && (Red.Length - RedRight - 1 > 1))
                        {
                            int Count = GetNum(5, RedRight) * GetNum(2, Red.Length - RedRight - 1);

                            Description5 = Description5 + Count;
                            WinMoney += WinMoney5 * Count;
                            WinMoneyNoWithTax += WinMoneyNoWithTax5 * Count;
                        }
                        else if (!BlueRight && (Red.Length - RedRight > 1))
                        {
                            int Count = GetNum(5, RedRight) * GetNum(2, Red.Length - RedRight);

                            Description5 = Description5 + Count;
                            WinMoney += WinMoney5 * Count;
                            WinMoneyNoWithTax += WinMoneyNoWithTax5 * Count;
                        }
                    }

                    if (RedRight == Red.Length - 2 && (RedRight == 5))
                    {
                        continue;
                    }

                    if ((RedRight >= 4) && BlueRight)
                    {
                        int Count = 0;

                        if (Red.Length - RedRight - 1 > 1)
                        {
                            Count = GetNum(4, RedRight) * GetNum(2, Red.Length - RedRight - 1);
                        }

                        if (Count > 0)
                        {
                            Description6 = Description6 + Count;
                            WinMoney += WinMoney6 * Count;
                            WinMoneyNoWithTax += WinMoneyNoWithTax6 * Count;
                        }
                    }

                    if (RedRight >= 4)
                    {
                        if (BlueRight && (Red.Length > 7) && (Red.Length - RedRight - 1 > 3))
                        {
                            int Count = GetNum(4, RedRight) * GetNum(3, Red.Length - RedRight - 1);
                            Description7 = Description7 + Count;
                            WinMoney += WinMoney7 * Count;
                            WinMoneyNoWithTax += WinMoneyNoWithTax7 * Count;

                            continue;
                        }
                        else if (!BlueRight && (Red.Length - RedRight > 2))
                        {
                            int Count = GetNum(4, RedRight) * GetNum(3, Red.Length - RedRight);
                            Description7 = Description7 + Count;
                            WinMoney += WinMoney7 * Count;
                            WinMoneyNoWithTax += WinMoneyNoWithTax7 * Count;

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
                if (Description7 > 0)
                {
                    if (Description != "")
                        Description += "，";
                    Description += "七等奖" + Description7.ToString() + "注";
                }

                if (Description != "")
                    Description += "。";

                return WinMoney;
            }
            private double ComputeWin_DanT(int PlayType, string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney1, double WinMoneyNoWithTax1, double WinMoney2, double WinMoneyNoWithTax2, double WinMoney3, double WinMoneyNoWithTax3, double WinMoney4, double WinMoneyNoWithTax4, double WinMoney5, double WinMoneyNoWithTax5, double WinMoney6, double WinMoneyNoWithTax6, double WinMoney7, double WinMoneyNoWithTax7)
            {
                WinNumber = WinNumber.Trim();
                if (WinNumber.Length < 25)	//25: "01 02 03 04 05 06 07 + 08"
                    return -1;
                string[] Lotterys = SplitLotteryNumber(Number);
                if (Lotterys == null)
                    return -2;
                if (Lotterys.Length < 1)
                    return -2;

                string Blue = WinNumber.Substring(23, 2);

                int Description1 = 0, Description2 = 0, Description3 = 0, Description4 = 0, Description5 = 0, Description6 = 0, Description7 = 0;
                double WinMoney = 0;
                WinMoneyNoWithTax = 0;

                Description = "";

                string RegexString;
                RegexString = @"^(\d\d\s){1,6}[,](\s)(\d\d\s){1,29}(\d\d)";
                Regex regex = new Regex(RegexString, RegexOptions.Compiled | RegexOptions.IgnoreCase);

                int Red_DRight = 0;
                int Red_TRight = 0;
                bool BlueRight1 = false;
                bool BlueRight2 = false;

                string[] Red_D = null;
                string[] Red_T = null;

                for (int ii = 0; ii < Lotterys.Length; ii++)
                {

                    Red_DRight = 0;
                    Red_TRight = 0;
                    BlueRight1 = false;
                    BlueRight2 = false;

                    Match m = regex.Match(Lotterys[ii]);

                    if (!m.Success)
                    {
                        continue;
                    }

                    Red_D = FilterRepeated(m.Value.Split(',')[0].Trim().Split(' '));
                    Red_T = FilterRepeated(m.Value.Split(',')[1].Trim().Split(' '));

                    if (Red_D.Length + Red_T.Length < 8)
                    {
                        continue;
                    }

                    foreach (string str in Red_D)
                    {
                        if (!string.IsNullOrEmpty(str))
                        {
                            if (WinNumber.IndexOf(str.Trim() + " ") >= 0)
                            {
                                Red_DRight++;
                            }

                            if (Blue == str.Trim())
                            {
                                BlueRight1 = true;
                            }
                        }
                    }

                    foreach (string str in Red_T)
                    {
                        if (!string.IsNullOrEmpty(str))
                        {
                            if (WinNumber.IndexOf(str.Trim() + " ") >= 0)
                            {
                                Red_TRight++;
                            }

                            if (Blue == str.Trim())
                            {
                                BlueRight2 = true;
                            }
                        }
                    }

                    if (Red_D.Length + Red_TRight < 4)
                    {
                        continue;
                    }

                    if ((Red_DRight + Red_TRight == 7) && (Red_D.Length == Red_DRight))
                    {
                        Description1++;
                        WinMoney += WinMoney1;
                        WinMoneyNoWithTax += WinMoneyNoWithTax1;
                    }

                    if (Red_DRight == 7)
                    {
                        continue;
                    }

                    if ((Red_DRight + Red_TRight >= 6) && (BlueRight1 || BlueRight2) && (Red_D.Length - Red_DRight <= 1) && (Red_DRight <= 6))
                    {
                        int Count = 0;

                        Count = GetNum(6 - Red_DRight, Red_TRight);

                        Description2 = Description2 + Count;
                        WinMoney += WinMoney2 * Description2;
                        WinMoneyNoWithTax += WinMoneyNoWithTax2 * Description2;
                    }

                    if ((Red_DRight + Red_TRight >= 6) && !BlueRight1 && (Red_D.Length - Red_DRight <= 1) && (Red_DRight <= 6))
                    {
                        int Count = 0;

                        Count = GetNum(6 - Red_DRight, Red_TRight);

                        if (BlueRight2 && (Red_D.Length == Red_DRight) && (Red_D.Length + Red_T.Length > 8))
                        {
                            Count = Count * GetNum(1, Red_T.Length - Red_TRight - 1);
                        }
                        else if (!BlueRight2 && (Red_D.Length == Red_DRight) && (Red_D.Length + Red_T.Length >= 8))
                        {
                            Count = Count * GetNum(1, Red_T.Length - Red_TRight);
                        }

                        Description3 = Description3 + Count;
                        WinMoney += WinMoney3 * Count;
                        WinMoneyNoWithTax += WinMoneyNoWithTax3 * Count;
                    }

                    if (Red_DRight == 6)
                    {
                        continue;
                    }

                    if ((Red_DRight + Red_TRight >= 5) && (BlueRight1 || BlueRight2) && (Red_D.Length - Red_DRight <= 2) && (Red_DRight <= 5))
                    {
                        int Count = 0;

                        Count = GetNum(5 - Red_DRight, Red_TRight);

                        if (BlueRight1 && (Red_D.Length - Red_DRight == 1))
                        {
                            Count = Count * GetNum(1, Red_T.Length - Red_TRight);
                        }

                        if (BlueRight2 && Red_D.Length - Red_DRight == 0)
                        {
                            Count = Count * GetNum(1, Red_T.Length - Red_TRight - 1);
                        }

                        Description4 = Description4 + Count;
                        WinMoney += WinMoney4 * Count;
                        WinMoneyNoWithTax += WinMoneyNoWithTax4 * Count;
                    }

                    if ((Red_DRight + Red_TRight >= 5) && !BlueRight1 && (Red_D.Length - Red_DRight <= 2) && (Red_DRight <= 5))
                    {
                        int Count = 0;

                        Count = GetNum(5 - Red_DRight, Red_TRight);

                        if (BlueRight2 && (Red_D.Length - Red_DRight < 2) && (Red_D.Length + Red_T.Length > 8))
                        {
                            Count = Count * GetNum(7 - Red_D.Length - 5 + Red_DRight, Red_T.Length - Red_TRight - 1);
                        }
                        else if (!BlueRight2 && (Red_D.Length - Red_DRight < 2) && (Red_D.Length + Red_T.Length >= 8))
                        {
                            Count = Count * GetNum(7 - Red_D.Length - 5 + Red_DRight, Red_T.Length - Red_TRight);
                        }

                        Description5 = Description5 + Count;
                        WinMoney += WinMoney5 * Count;
                        WinMoneyNoWithTax += WinMoneyNoWithTax5 * Count;
                    }

                    if (Red_DRight == 5)
                    {
                        continue;
                    }

                    if ((Red_DRight + Red_TRight >= 4) && (BlueRight1 || BlueRight2) && (Red_D.Length - Red_DRight <= 3) && (Red_DRight <= 4))
                    {
                        int Count = 0;

                        Count = GetNum(4 - Red_DRight, Red_TRight);

                        if (BlueRight1 && (Red_D.Length - Red_DRight == 1))
                        {
                            Count = Count * GetNum(2, Red_T.Length - Red_TRight);
                        }

                        if (BlueRight2 && Red_D.Length - Red_DRight == 0)
                        {
                            Count = Count * GetNum(2, Red_T.Length - Red_TRight - 1);
                        }

                        Description6 = Description6 + Count;
                        WinMoney += WinMoney6 * Count;
                        WinMoneyNoWithTax += WinMoneyNoWithTax6 * Count;
                    }

                    if ((Red_DRight + Red_TRight >= 4) && !BlueRight1 && (Red_DRight <= 4))
                    {
                        int Count = 0;

                        Count = GetNum(4 - Red_DRight, Red_TRight);

                        if (BlueRight2 && (Red_D.Length - Red_DRight < 3) && (Red_D.Length + Red_T.Length > 8))
                        {
                            Count = Count * GetNum(7 - Red_D.Length - 4 + Red_DRight, Red_T.Length - Red_TRight - 1);
                        }
                        else if (!BlueRight2 && (Red_D.Length - Red_DRight < 3) && (Red_D.Length + Red_T.Length >= 8))
                        {
                            Count = Count * GetNum(7 - Red_D.Length - 4 + Red_DRight, Red_T.Length - Red_TRight);
                        }

                        Description7 = Description7 + Count;
                        WinMoney += WinMoney7 * Count;
                        WinMoneyNoWithTax += WinMoneyNoWithTax7 * Count;

                        continue;
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
                if (Description7 > 0)
                {
                    if (Description != "")
                        Description += "，";
                    Description += "七等奖" + Description7.ToString() + "注";
                }

                if (Description != "")
                    Description += "。";

                return WinMoney;
            }
            #endregion

            public override string AnalyseScheme(string Content, int PlayType)
            {
                if ((PlayType == PlayType_D) || (PlayType == PlayType_F))
                    return AnalyseScheme_Zhi(Content, PlayType);

                if (PlayType == PlayType_DanT)
                    return AnalyseScheme_DanT(Content, PlayType);

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
                    RegexString = @"^(\d\d\s){6}\d\d";
                else
                    RegexString = @"^(\d\d\s){6,31}\d\d";
                Regex regex = new Regex(RegexString, RegexOptions.Compiled | RegexOptions.IgnoreCase);

                for (int i = 0; i < strs.Length; i++)
                {
                    Match m = regex.Match(strs[i]);
                    if (!m.Success)
                    {
                        continue;
                    }

                    string CanonicalNumber = "";

                    if (PlayType == PlayType_D)
                    {
                        string[] singles = ToSingle(m.Value, ref CanonicalNumber, PlayType);

                        if (singles == null)
                        {
                            continue;
                        }

                        Result += CanonicalNumber + "|" + singles.Length.ToString() + "\n";

                        continue;
                    }

                    string[] Number = FilterRepeated(m.Value.Trim().Trim().Split(' '));

                    if (Number.Length < 8)
                    {
                        continue;
                    }

                    for (int j = 0; j < Number.Length; j++)
                    {
                        CanonicalNumber += Number[j] + " ";
                    }

                    Result += CanonicalNumber + "|" + GetNum(7, Number.Length).ToString() + "\n";
                }

                if (Result.EndsWith("\n"))
                    Result = Result.Substring(0, Result.Length - 1);
                return Result;
            }
            private string AnalyseScheme_DanT(string Content, int PlayType)
            {
                string[] strs = Content.Split('\n');
                if (strs == null)
                    return "";
                if (strs.Length == 0)
                    return "";

                string Result = "";

                string RegexString;

                RegexString = @"^(\d\d\s){1,6}[,](\s\d\d){1,28}(\s\d\d)";
                Regex regex = new Regex(RegexString, RegexOptions.Compiled | RegexOptions.IgnoreCase);

                for (int i = 0; i < strs.Length; i++)
                {
                    Match m = regex.Match(strs[i]);
                    if (!m.Success)
                    {
                        continue;
                    }
                    string Red_D_Number = "";
                    string Red_T_Number = "";

                    string[] Red_D = FilterRepeated(m.Value.Split(',')[0].Trim().Split(' '));
                    string[] Red_T = FilterRepeated(m.Value.Split(',')[1].Trim().Split(' '));

                    if (Red_D.Length + Red_T.Length < 8)
                    {
                        continue;
                    }

                    for (int j = 0; j < Red_D.Length; j++)
                    {
                        Red_D_Number += Red_D[j] + " ";
                    }

                    for (int j = 0; j < Red_T.Length; j++)
                    {
                        if (Red_D_Number.IndexOf(Red_T[j]) >= 0)
                        {
                            continue;
                        }

                        Red_T_Number += Red_T[j] + " ";
                    }

                    Result += Red_D_Number + ", " + Red_T_Number + "|" + GetNum(7 - Red_D.Length, Red_T.Length).ToString() + "\n";
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
                Regex regex = new Regex(@"^(\d\d\s){7}[+]\s\d\d", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                if (!regex.IsMatch(Number))
                    return false;

                string t_str = "";
                string[] WinLotteryNumber = ToSingle(Number.Substring(0, 20), ref t_str, PlayType_D);

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
                    if ((Ball >= 1) && (Ball <= 31) && (!isExistBall(al, Ball)))
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
                    case "TCBJYTD":
                        if ((PlayTypeID == PlayType_D) || (PlayTypeID == PlayType_F))
                        {
                            return GetPrintKeyList_TCBJYTD(Numbers);
                        }
                        break;
                    case "CORONISTPT":
                        if ((PlayTypeID == PlayType_D) || (PlayTypeID == PlayType_F))
                        {
                            return GetPrintKeyList_CORONISTPT(Numbers);
                        }
                        break;
                    case "CP86":
                        if ((PlayTypeID == PlayType_D) || (PlayTypeID == PlayType_F))
                        {
                            return GetPrintKeyList_CP86(Numbers);
                        }
                        break;
                    case "RS6500":
                        if ((PlayTypeID == PlayType_D) || (PlayTypeID == PlayType_F))
                        {
                            return GetPrintKeyList_RS6500(Numbers);
                        }
                        break;
                    case "ks230":
                        if ((PlayTypeID == PlayType_D) || (PlayTypeID == PlayType_F))
                        {
                            return GetPrintKeyList_ks230(Numbers);
                        }
                        break;
                }

                return "";
            }
            #region GetPrintKeyList 的具体方法
            private string GetPrintKeyList_TCBJYTD(string[] Numbers)
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

            private string GetPrintKeyList_CORONISTPT(string[] Numbers)
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

            private string GetPrintKeyList_CP86(string[] Numbers)
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

            private string GetPrintKeyList_RS6500(string[] Numbers)
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

            private string GetPrintKeyList_ks230(string[] Numbers)
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
                                if (strs[i + M].ToString().Split('|').Length < 2)
                                {
                                    continue;
                                }

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

            private string ConvertFormatToElectronTicket_HPSD(int PlayTypeID, string Number)
            {
                Number = Number.Trim();

                string Ticket = "";

                if (PlayTypeID == PlayType_D || PlayTypeID == PlayType_F)
                {
                    Ticket = Number.Replace(" ", ",");
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
                if (PlayTypeID == PlayType_DanT)
                {
                    return ToElectronicTicket_ZCW_DT(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
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
                                if (strs[i + M].ToString().Split('|').Length < 2)
                                {
                                    continue;
                                }

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
            private Ticket[] ToElectronicTicket_ZCW_DT(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
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

                        al.Add(new Ticket(2, ConvertFormatToElectronTicket_ZCW(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));
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

                if (PlayTypeID == PlayType_D || PlayTypeID == PlayType_F || PlayTypeID == PlayType_DanT)
                {
                    Ticket = Number.Replace(" ", "").Replace(",", "*");
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
                if (PlayTypeID == PlayType_DanT)
                {
                    return ToElectronicTicket_ZZYTC_DanT(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
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
                                if (strs[i + M].ToString().Split('|').Length < 2)
                                {
                                    continue;
                                }

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
            private Ticket[] ToElectronicTicket_ZZYTC_DanT(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
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

            private string ConvertFormatToElectronTicket_ZZYTC(int PlayTypeID, string Number)
            {
                Number = Number.Trim();

                string Ticket = "";

                if (PlayTypeID == PlayType_D || PlayTypeID == PlayType_F || PlayTypeID == PlayType_DanT)
                {
                    Ticket = Number.Replace(" ", "").Replace(",", "*");
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
                                if (strs[i + M].ToString().Split('|').Length < 2)
                                {
                                    continue;
                                }

                                Numbers += strs[i + M].ToString().Split('|')[0] + ";\n";
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

                        Numbers = strs[i].ToString().Split('|')[0] + ";";
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

                if (PlayTypeID == PlayType_D || PlayTypeID == PlayType_F)
                {
                    Ticket = Number;
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
                if (PlayTypeID == PlayType_DanT)
                {
                    return ToElectronicTicket_CTTCSD_DanT(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
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
                                if (strs[i + M].ToString().Split('|').Length < 2)
                                {
                                    continue;
                                }

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
            private Ticket[] ToElectronicTicket_CTTCSD_DanT(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
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

            private string ConvertFormatToElectronTicket_CTTCSD(int PlayTypeID, string Number)
            {
                Number = Number.Trim();

                string Ticket = "";

                if (PlayTypeID == PlayType_D || PlayTypeID == PlayType_F || PlayTypeID == PlayType_DanT)
                {
                    Ticket = Number.Replace(" ", "").Replace(",", "*");
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
                if (PlayTypeID == PlayType_DanT)
                {
                    return ToElectronicTicket_BJCP_DT(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
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
                                if (strs[i + M].ToString().Split('|').Length < 2)
                                {
                                    continue;
                                }

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
            private Ticket[] ToElectronicTicket_BJCP_DT(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
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

                        al.Add(new Ticket(2, ConvertFormatToElectronTicket_BJCP(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));
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

                if (PlayTypeID == PlayType_D || PlayTypeID == PlayType_F || PlayTypeID == PlayType_DanT)
                {
                    Ticket = Number.Replace(" ", "").Replace(",", "*");
                }

                return Ticket;
            }
            #endregion
        }
    }
}