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
        /// 双色球
        /// </summary>
        public partial class SSQ : LotteryBase
        {
            #region 静态变量
            public const int PlayType_D = 501;
            public const int PlayType_F = 502;
            public const int PlayType_DanT = 503;

            public const int ID = 5;
            public const string sID = "5";
            public const string Name = "双色球";
            public const string Code = "SSQ";
            public const double MaxMoney = 1240320;
            #endregion

            public SSQ()
            {
                id = 5;
                name = "双色球";
                code = "SSQ";
            }

            public override bool CheckPlayType(int play_type)
            {
                return ((play_type >= 501) && (play_type <= 503));
            }

            public override PlayType[] GetPlayTypeList()
            {
                PlayType[] Result = new PlayType[3];

                Result[0] = new PlayType(PlayType_D, "单式");
                Result[1] = new PlayType(PlayType_F, "复式");
                Result[2] = new PlayType(PlayType_DanT, "胆拖");

                return Result;
            }

            public override string BuildNumber(int Red, int Blue, int Num)	//id = 5
            {
                System.Random rd = new Random();
                StringBuilder sb = new StringBuilder();
                ArrayList al_r = new ArrayList();
                ArrayList al_b = new ArrayList();

                for (int i = 0; i < Num; i++)
                {
                    al_r.Clear();
                    al_b.Clear();
                    for (int j = 0; j < Red; j++)
                    {
                        int Ball = 0;
                        while ((Ball == 0) || isExistBall(al_r, Ball))
                            Ball = rd.Next(1, 33 + 1);
                        al_r.Add(Ball.ToString().PadLeft(2, '0'));
                    }

                    for (int j = 0; j < Blue; j++)
                    {
                        int Ball = 0;
                        while ((Ball == 0) || isExistBall(al_b, Ball))
                            Ball = rd.Next(1, 16 + 1);
                        al_b.Add(Ball.ToString().PadLeft(2, '0'));
                    }

                    CompareToAscii compare = new CompareToAscii();
                    al_r.Sort(compare);
                    al_b.Sort(compare);

                    string LotteryNumber = "";
                    for (int j = 0; j < al_r.Count; j++)
                        LotteryNumber += al_r[j].ToString() + " ";
                    LotteryNumber += "+ ";
                    for (int j = 0; j < al_b.Count; j++)
                        LotteryNumber += al_b[j].ToString() + " ";

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
                string[] strs = Number.Split('+');
                CanonicalNumber = "";

                if (strs.Length != 2)
                {
                    CanonicalNumber = "";
                    return null;
                }

                string[] Red = FilterRepeated(strs[0].Trim().Split(' '), 33);
                string[] Blue = FilterRepeated(strs[1].Trim().Split(' '), 16);

                if ((Red.Length < 6) || (Blue.Length < 1) || (Red.Length > 20))
                {
                    CanonicalNumber = "";
                    return null;
                }

                for (int i = 0; i < Red.Length; i++)
                    CanonicalNumber += (Red[i] + " ");
                CanonicalNumber += "+ ";
                for (int i = 0; i < Blue.Length; i++)
                    CanonicalNumber += (Blue[i] + " ");
                CanonicalNumber = CanonicalNumber.Trim();

                ArrayList al = new ArrayList();

                #region 循环取单式

                int n = Red.Length;
                for (int i = 0; i < n - 5; i++)
                    for (int j = i + 1; j < n - 4; j++)
                        for (int k = j + 1; k < n - 3; k++)
                            for (int x = k + 1; x < n - 2; x++)
                                for (int y = x + 1; y < n - 1; y++)
                                    for (int z = y + 1; z < n; z++)
                                        for (int b = 0; b < Blue.Length; b++)
                                            al.Add(Red[i] + " " + Red[j] + " " + Red[k] + " " + Red[x] + " " + Red[y] + " " + Red[z] + " + " + Blue[b]);

                #endregion

                string[] Result = new string[al.Count];
                for (int i = 0; i < al.Count; i++)
                    Result[i] = al[i].ToString();
                return Result;
            }
            private string[] ToSingle_DanT(string Number, ref string CanonicalNumber)	//胆拖取单式, 后面 ref 参数是将彩票规范化，如：01 01 02... 变成 01 02...
            {
                string[] strs = Number.Split('+');
                CanonicalNumber = "";

                if (strs.Length != 2)
                {
                    CanonicalNumber = "";
                    return null;
                }

                string[] RedDan = FilterRepeated(strs[0].Trim().Split(',')[0].Trim().Split(' '), 33);
                string[] RedTuo = FilterRepeated(strs[0].Trim().Split(',')[1].Trim().Split(' '), 33);
                string[] Blue = FilterRepeated(strs[1].Trim().Split(' '), 16);

                string[] TempRedDan = FilterRepeated(RedDan, RedTuo);

                if (((TempRedDan.Length + RedTuo.Length) < 7) || (Blue.Length < 1) || (TempRedDan.Length > 5))
                {
                    CanonicalNumber = "";
                    return null;
                }

                for (int i = 0; i < TempRedDan.Length; i++)
                    CanonicalNumber += (TempRedDan[i] + " ");
                CanonicalNumber += ", ";
                for (int i = 0; i < RedTuo.Length; i++)
                    CanonicalNumber += (RedTuo[i] + " ");
                CanonicalNumber += "+ ";
                for (int i = 0; i < Blue.Length; i++)
                    CanonicalNumber += (Blue[i] + " ");
                CanonicalNumber = CanonicalNumber.Trim();

                string[] Red = new string[TempRedDan.Length + RedTuo.Length];
                string Numbers = "";
                for (int i = 0; i < TempRedDan.Length; i++)
                {
                    Red[i] = TempRedDan[i];
                    Numbers += TempRedDan[i] + " ";
                }
                for (int i = TempRedDan.Length; i < (TempRedDan.Length + RedTuo.Length); i++)
                    Red[i] = RedTuo[i - TempRedDan.Length];

                ArrayList al = new ArrayList();

                #region 循环取单式

                int m = TempRedDan.Length;
                int n = Red.Length;

                if (m == 1)
                {
                    for (int j = m; j < n - 4; j++)
                        for (int k = j + 1; k < n - 3; k++)
                            for (int x = k + 1; x < n - 2; x++)
                                for (int y = x + 1; y < n - 1; y++)
                                    for (int z = y + 1; z < n; z++)
                                        for (int b = 0; b < Blue.Length; b++)
                                        {
                                            al.Add(Numbers + Red[j] + " " + Red[k] + " " + Red[x] + " " + Red[y] + " " + Red[z] + " + " + Blue[b]);
                                        }
                }
                else if (m == 2)
                {
                    for (int k = m; k < n - 3; k++)
                        for (int x = k + 1; x < n - 2; x++)
                            for (int y = x + 1; y < n - 1; y++)
                                for (int z = y + 1; z < n; z++)
                                    for (int b = 0; b < Blue.Length; b++)
                                    {
                                        al.Add(Numbers + Red[k] + " " + Red[x] + " " + Red[y] + " " + Red[z] + " + " + Blue[b]);
                                    }
                }
                else if (m == 3)
                {
                    for (int x = m; x < n - 2; x++)
                        for (int y = x + 1; y < n - 1; y++)
                            for (int z = y + 1; z < n; z++)
                                for (int b = 0; b < Blue.Length; b++)
                                {
                                    al.Add(Numbers + Red[x] + " " + Red[y] + " " + Red[z] + " + " + Blue[b]);
                                }
                }
                else if (m == 4)
                {
                    for (int y = m; y < n - 1; y++)
                        for (int z = y + 1; z < n; z++)
                            for (int b = 0; b < Blue.Length; b++)
                            {
                                al.Add(Numbers + Red[y] + " " + Red[z] + " + " + Blue[b]);
                            }
                }
                else if (m == 5)
                {
                    for (int z = m; z < n; z++)
                        for (int b = 0; b < Blue.Length; b++)
                        {
                            al.Add(Numbers + Red[z] + " + " + Blue[b]);
                        }
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
                if ((WinMoneyList == null) || (WinMoneyList.Length < 14))    // 奖金参数排列顺序 一等, 二等, 三等, 四等, 五等, 六等,　快乐星期天
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
                if (WinNumber.Length < 22)	//22: "01 02 03 04 05 06 + 01" 快乐星期天: "01 02 03 04 05 06 + 01 02" 重大节日: "01 02 03 04 05 06 + 01 02 03 04"
                    return -1;
                string[] Lotterys = SplitLotteryNumber(Number);
                if (Lotterys == null)
                    return -2;
                if (Lotterys.Length < 1)
                    return -2;

                string WinNumber_Red = WinNumber.Substring(0, 18);
                string WinNumber_Blue = WinNumber.Substring(20, WinNumber.Length - 20).Trim();

                int Description1 = 0, Description2 = 0, Description3 = 0, Description4 = 0, Description5 = 0, Description6 = 0, DescriptionSunday = 0;
                double WinMoney = 0;
                WinMoneyNoWithTax = 0;
                Description = "";

                string RegexString;
                if (PlayType == PlayType_D)
                    RegexString = @"^(\d\d\s){6}[+]\s\d\d";
                else
                    RegexString = @"^(\d\d\s){6,20}[+](\s\d\d){1,16}";
                Regex regex = new Regex(RegexString, RegexOptions.Compiled | RegexOptions.IgnoreCase);

                int DescriptionSum = 0;
                int RedRight = 0;
                int BlueRightSunday = 0;

                bool BlueRight = false;

                string[] Red = null;
                string[] Blue = null;

                for (int ii = 0; ii < Lotterys.Length; ii++)
                {
                    RedRight = 0;
                    DescriptionSum = 0;
                    BlueRightSunday = 0;
                    BlueRight = false;

                    Match m = regex.Match(Lotterys[ii]);

                    if (!m.Success)
                    {
                        continue;
                    }

                    Red = FilterRepeated(m.Value.Trim().Split('+')[0].Trim().Split(' '), 33);
                    Blue = FilterRepeated(m.Value.Trim().Split('+')[1].Trim().Split(' '), 16);

                    if (Red.Length < 6)
                    {
                        continue;
                    }

                    if (Blue.Length < 1)
                    {
                        continue;
                    }

                    foreach (string str in Red)
                    {
                        if (!string.IsNullOrEmpty(str))
                        {
                            if (WinNumber_Red.IndexOf(str.Trim()) >= 0)
                            {
                                RedRight++;
                            }
                        }
                    }

                    foreach (string str in Blue)
                    {
                        if (!string.IsNullOrEmpty(str))
                        {
                            if (WinNumber_Blue.Substring(0, 2) == str.Trim())
                            {
                                BlueRight = true;
                            }

                            if (WinNumber_Blue.IndexOf(" " + str.Trim()) >= 0)
                            {
                                BlueRightSunday++;
                            }
                        }
                    }

                    if ((!BlueRight) && (RedRight < 3))
                    {
                        continue;
                    }

                    if ((RedRight == 6) && BlueRight)
                    {
                        DescriptionSum++;
                        Description1++;
                        WinMoney += WinMoney1;
                        WinMoneyNoWithTax += WinMoneyNoWithTax1;
                    }

                    if (RedRight == 6)
                    {
                        int Count = Blue.Length;

                        if (BlueRight && (Count > 1))
                        {
                            Description2 = Description2 + (Count - 1);
                            WinMoney += WinMoney2 * (Count - 1);
                            WinMoneyNoWithTax += WinMoneyNoWithTax2 * (Count - 1);
                        }
                        else if (!BlueRight)
                        {
                            Description2 = Description2 + Count;
                            WinMoney += WinMoney2 * Count;
                            WinMoneyNoWithTax += WinMoneyNoWithTax2 * Count;
                        }
                    }

                    if ((Red.Length == RedRight) && (RedRight == 6))
                    {
                        continue;
                    }

                    if ((RedRight >= 5) && BlueRight && (Red.Length - RedRight > 0))
                    {
                        int Count = GetNum(5, RedRight) * GetNum(1, Red.Length - RedRight);

                        Description3 = Description3 + Count;
                        WinMoney += WinMoney3 * Count;
                        WinMoneyNoWithTax += WinMoneyNoWithTax3 * Count;

                        DescriptionSum += Count;
                    }

                    if ((RedRight >= 5) && (Red.Length - RedRight > 0))
                    {
                        int Count = GetNum(5, RedRight) * GetNum(1, Red.Length - RedRight);

                        int BlueCount = 0;

                        if (BlueRight && (Blue.Length > 1))
                        {
                            BlueCount = Count * (Blue.Length - 1);

                            Description4 = Description4 + BlueCount;
                            WinMoney += WinMoney4 * BlueCount;
                            WinMoneyNoWithTax += WinMoneyNoWithTax4 * BlueCount;
                        }
                        else if (!BlueRight)
                        {
                            BlueCount = Count * Blue.Length;

                            Description4 = Description4 + BlueCount;
                            WinMoney += WinMoney4 * BlueCount;
                            WinMoneyNoWithTax += WinMoneyNoWithTax4 * BlueCount;
                        }

                        if (BlueRightSunday > 0)	//快乐星期天
                        {
                            DescriptionSunday = DescriptionSunday + Count * BlueRightSunday;
                            WinMoney += WinMoney7 * Count * BlueRightSunday;
                            WinMoneyNoWithTax += WinMoneyNoWithTax7 * Count * BlueRightSunday;
                        }
                    }

                    if ((RedRight == Red.Length - 1) && (RedRight == 5))
                    {
                        continue;
                    }

                    if ((RedRight >= 4) && BlueRight && (Red.Length - RedRight > 1))
                    {
                        int Count = GetNum(4, RedRight) * GetNum(2, Red.Length - RedRight);

                        Description4 = Description4 + Count;
                        WinMoney += WinMoney4 * Count;
                        WinMoneyNoWithTax += WinMoneyNoWithTax4 * Count;

                        if (BlueRightSunday > 0)	//快乐星期天
                        {
                            DescriptionSunday = DescriptionSunday + Count * BlueRightSunday;
                            WinMoney += WinMoney7 * Count * BlueRightSunday;
                            WinMoneyNoWithTax += WinMoneyNoWithTax7 * Count * BlueRightSunday;
                        }

                        DescriptionSum += Count;
                    }

                    if ((RedRight >= 4) && (Red.Length - RedRight > 1))
                    {
                        int Count = GetNum(4, RedRight) * GetNum(2, Red.Length - RedRight);

                        int BlueCount = 0;

                        if (BlueRight && (Blue.Length > 1))
                        {
                            BlueCount = Count * (Blue.Length - 1);

                            Description5 = Description5 + BlueCount;
                            WinMoney += WinMoney5 * BlueCount;
                            WinMoneyNoWithTax += WinMoneyNoWithTax5 * BlueCount;
                        }
                        else if(!BlueRight)
                        {
                            BlueCount = Count * Blue.Length;

                            Description5 = Description5 + BlueCount;
                            WinMoney += WinMoney5 * BlueCount;
                            WinMoneyNoWithTax += WinMoneyNoWithTax5 * BlueCount;
                        }
                    }

                    if ((RedRight == Red.Length - 2) && (RedRight == 4))
                    {
                        continue;
                    }

                    if ((RedRight >= 3) && BlueRight && (Red.Length - RedRight > 2))
                    {
                        int Count = GetNum(3, RedRight) * GetNum(3, Red.Length - RedRight);

                        Description5 = Description5 + Count;
                        WinMoney += WinMoney5 * Count;
                        WinMoneyNoWithTax += WinMoneyNoWithTax5 * Count;

                        DescriptionSum += Count;
                    }

                    if (BlueRight)
                    {
                        int Count = GetNum(6, Red.Length) - DescriptionSum;

                        Description6 = Description6 + Count;
                        WinMoney += WinMoney6 * Count;
                        WinMoneyNoWithTax += WinMoneyNoWithTax6 * Count;
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
                if (DescriptionSunday > 0)
                {
                    if (Description != "")
                        Description += "，";
                    Description += "快乐星期天特别奖" + DescriptionSunday.ToString() + "注";
                }

                if (Description != "")
                    Description += "。";

                return WinMoney;
            }
            private double ComputeWin_DanT(int PlayType, string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney1, double WinMoneyNoWithTax1, double WinMoney2, double WinMoneyNoWithTax2, double WinMoney3, double WinMoneyNoWithTax3, double WinMoney4, double WinMoneyNoWithTax4, double WinMoney5, double WinMoneyNoWithTax5, double WinMoney6, double WinMoneyNoWithTax6, double WinMoney7, double WinMoneyNoWithTax7)
            {
                WinNumber = WinNumber.Trim();
                if (WinNumber.Length < 22)	//22: "01 02 03 04 05 06 + 01" 快乐星期天: "01 02 03 04 05 06 + 01 02" 重大节日: "01 02 03 04 05 06 + 01 02 03 04"
                    return -1;
                string[] Lotterys = SplitLotteryNumber(Number);
                if (Lotterys == null)
                    return -2;
                if (Lotterys.Length < 1)
                    return -2;

                string WinNumber_Red = WinNumber.Substring(0, 18);
                string WinNumber_Blue = WinNumber.Substring(20, WinNumber.Length - 20).Trim();

                int Description1 = 0, Description2 = 0, Description3 = 0, Description4 = 0, Description5 = 0, Description6 = 0, DescriptionSunday = 0;
                double WinMoney = 0;
                WinMoneyNoWithTax = 0;
                Description = "";

                string RegexString;
                RegexString = @"^(\d\d\s){1,5}[,](\s)(\d\d\s){2,32}[+](\s\d\d){1,16}";
                Regex regex = new Regex(RegexString, RegexOptions.Compiled | RegexOptions.IgnoreCase);

                int DescriptionSum = 0;
                int Red_DRight = 0;
                int Red_TRight = 0;
                int BlueRightSunday = 0;
                bool BlueRight = false;

                string[] Red_D = null;
                string[] Red_T = null;
                string[] Blue = null;

                for (int ii = 0; ii < Lotterys.Length; ii++)
                {
                    Red_DRight = 0;
                    Red_TRight = 0;
                    DescriptionSum = 0;
                    BlueRight = false;

                    Match m = regex.Match(Lotterys[ii]);
                    if (!m.Success)
                    {
                        continue;
                    }

                    Red_D = FilterRepeated(m.Value.Split('+')[0].Trim().Split(',')[0].Trim().Split(' '), 33);
                    Red_T = FilterRepeated(m.Value.Split('+')[0].Trim().Split(',')[1].Trim().Split(' '), 33);
                    Blue = FilterRepeated(m.Value.Trim().Split('+')[1].Trim().Split(' '), 16);

                    if (Red_D.Length + Red_T.Length < 7)
                    {
                        continue;
                    }

                    if (Blue.Length < 1)
                    {
                        continue;
                    }

                    foreach (string str in Red_D)
                    {
                        if (!string.IsNullOrEmpty(str))
                        {
                            if (WinNumber_Red.IndexOf(str.Trim()) >= 0)
                            {
                                Red_DRight++;
                            }
                        }
                    }

                    foreach (string str in Red_T)
                    {
                        if (!string.IsNullOrEmpty(str))
                        {
                            if (WinNumber_Red.IndexOf(str.Trim()) >= 0)
                            {
                                Red_TRight++;
                            }
                        }
                    }

                    foreach (string str in Blue)
                    {
                        if (!string.IsNullOrEmpty(str))
                        {
                            if (WinNumber_Blue.Substring(0, 2) == str.Trim())
                            {
                                BlueRight = true;
                            }

                            if (WinNumber_Blue.IndexOf(" " + str.Trim()) >= 0)
                            {
                                BlueRightSunday++;
                            }
                        }
                    }

                    if ((!BlueRight) && (Red_DRight + Red_TRight < 3))
                    {
                        continue;
                    }

                    if ((Red_DRight + Red_TRight == 6) && BlueRight && (Red_DRight == Red_D.Length))
                    {
                        Description1++;
                        WinMoney += WinMoney1;
                        WinMoneyNoWithTax += WinMoneyNoWithTax1;
                        DescriptionSum++;
                    }

                    if ((Red_DRight + Red_TRight == 6) && (Red_D.Length == Red_DRight))
                    {
                        int Count = Blue.Length;

                        if (BlueRight && (Count > 1))
                        {
                            Description2 = Description2 + (Count - 1);
                            WinMoney += WinMoney2 * (Count - 1);
                            WinMoneyNoWithTax += WinMoneyNoWithTax2 * (Count - 1);
                        }
                        else if (!BlueRight)
                        {
                            Description2 = Description2 + Count;
                            WinMoney += WinMoney2 * Count;
                            WinMoneyNoWithTax += WinMoneyNoWithTax2 * Count;
                        }
                    }

                    if (Red_DRight == 6)
                    {
                        continue;
                    }

                    if ((Red_DRight + Red_TRight >= 5) && BlueRight && (Red_D.Length - Red_DRight <= 1))
                    {
                        int Count = 0;

                        if (Red_D.Length == Red_DRight)
                        {
                            Count = GetNum(5 - Red_DRight, Red_TRight) * GetNum(1, Red_T.Length - Red_TRight);
                        }
                        else
                        {
                            Count = 1;
                        }

                        Description3 = Description3 + Count;
                        WinMoney += WinMoney3 * Count;
                        WinMoneyNoWithTax += WinMoneyNoWithTax3 * Count;
                        DescriptionSum = DescriptionSum + Count;
                    }

                    if (Red_DRight + Red_TRight >= 5 && (Red_D.Length - Red_DRight <= 1))
                    {
                        int Count = 0;

                        int BlueCount = 0;

                        if (BlueRight && (Blue.Length > 1))
                        {
                            BlueCount = (Blue.Length - 1); 
                        }
                        else if (!BlueRight)
                        {
                            BlueCount =  Blue.Length;
                        }
                        else if (BlueRight && (Blue.Length == 1))
                        {
                            BlueCount = 0;
                        }

                        if (Red_D.Length == Red_DRight)
                        {
                            Count = GetNum(5 - Red_DRight, Red_TRight) * GetNum(1, Red_T.Length - Red_TRight);
                        }
                        else
                        {
                            Count = 1;
                        }

                        if (BlueCount > 0)
                        {
                            Description4 = Description4 + Count * BlueCount;
                            WinMoney += WinMoney4 * Count * BlueCount;
                            WinMoneyNoWithTax += WinMoneyNoWithTax4 * Count * BlueCount;
                        }

                        if (BlueRightSunday > 0)	//快乐星期天
                        {
                            DescriptionSunday = DescriptionSunday + Count * BlueRightSunday;
                            WinMoney += WinMoney7 * Count * BlueRightSunday;
                            WinMoneyNoWithTax += WinMoneyNoWithTax7 * Count * BlueRightSunday;
                        }
                    }

                    if (Red_DRight == 5)
                    {
                        continue;
                    }

                    if ((Red_DRight + Red_TRight >= 4) && BlueRight && (Red_D.Length - Red_DRight <= 2))
                    {
                        int Count = 0;

                        if (Red_D.Length == Red_DRight + 2)
                        {
                            Count = 1;
                        }
                        else
                        {
                            Count = GetNum(4 - Red_DRight, Red_TRight) * GetNum(2 - Red_D.Length + Red_DRight, Red_T.Length - Red_TRight);
                        }

                        Description4 = Description4 + Count;
                        WinMoney += WinMoney4 * Count;
                        WinMoneyNoWithTax += WinMoneyNoWithTax4 * Count;
                        DescriptionSum = DescriptionSum + Count;

                        // 快乐星期天
                        DescriptionSunday = DescriptionSunday + Count * BlueRightSunday;
                        WinMoney += WinMoney7 * Count * BlueRightSunday;
                        WinMoneyNoWithTax += WinMoneyNoWithTax7 * Count * BlueRightSunday;
                    }

                    if ((Red_DRight + Red_TRight >= 4) && (Red_D.Length - Red_DRight <= 2))
                    {
                        int Count = 0;

                        int BlueCount = 0;

                        if (BlueRight && (Blue.Length > 1))
                        {
                            BlueCount = (Blue.Length - 1);
                        }
                        else if (!BlueRight)
                        {
                            BlueCount = Blue.Length;
                        }
                        else if (BlueRight && (Blue.Length == 1))
                        {
                            BlueCount = 0;
                        }

                        if (Red_D.Length == Red_DRight + 2)
                        {
                            Count = 1;
                        }
                        else
                        {
                            Count = GetNum(4 - Red_DRight, Red_TRight) * GetNum(2 - Red_D.Length + Red_DRight, Red_T.Length - Red_TRight);
                        }

                        if (BlueCount > 0)
                        {
                            Description5 = Description5 + BlueCount * Count;
                            WinMoney += WinMoney5 * BlueCount * Count;
                            WinMoneyNoWithTax += WinMoneyNoWithTax5 * BlueCount * Count;
                        }
                    }

                    if (Red_DRight == 4)
                    {
                        continue;
                    }

                    if ((Red_DRight + Red_TRight >= 3) && BlueRight && (Red_D.Length - Red_DRight <= 3))
                    {
                        int Count = 0;

                        if (Red_D.Length == Red_DRight + 3)
                        {
                            Count = GetNum(3 - Red_DRight, Red_TRight);
                        }
                        else
                        {
                            Count = GetNum(3 - Red_DRight, Red_TRight) * GetNum(3 - Red_D.Length + Red_DRight, Red_T.Length - Red_TRight);
                        }

                        Description5 = Description5 + Count;
                        WinMoney += WinMoney5 * Count;
                        WinMoneyNoWithTax += WinMoneyNoWithTax5 * Count;
                        DescriptionSum = DescriptionSum + Count;
                    }

                    if (BlueRight && (Red_DRight < 3))
                    {
                        int Count = GetNum(6 - Red_D.Length, Red_T.Length) - DescriptionSum;

                        Description6 = Description6 + Count;
                        WinMoney += WinMoney6 * Count;
                        WinMoneyNoWithTax += WinMoneyNoWithTax6 * Count;
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
                if (DescriptionSunday > 0)
                {
                    if (Description != "")
                        Description += "，";
                    Description += "快乐星期天特别奖" + DescriptionSunday.ToString() + "注";
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
                    RegexString = @"^(\d\d\s){6}[+]\s\d\d";
                else
                    RegexString = @"^(\d\d\s){6,32}[+](\s\d\d){1,16}";
                Regex regex = new Regex(RegexString, RegexOptions.Compiled | RegexOptions.IgnoreCase);

                for (int i = 0; i < strs.Length; i++)
                {
                    Match m = regex.Match(strs[i]);

                    if (!m.Success)
                    {
                        continue;
                    }

                    if (PlayType == PlayType_D)
                    {
                        string CanonicalNumber = "";
                        string[] singles = ToSingle(m.Value, ref CanonicalNumber, PlayType);

                        if (singles == null)
                        {
                            continue;
                        }

                        Result += CanonicalNumber + "|" + singles.Length.ToString() + "\n";

                        continue;
                    }

                    string RedNumber = "";
                    string BlueNumber = "";

                    string[] Red = FilterRepeated(m.Value.Trim().Split('+')[0].Trim().Split(' '), 33);
                    string[] Blue = FilterRepeated(m.Value.Trim().Split('+')[1].Trim().Split(' '), 16);

                    if (Red.Length + Blue.Length < 8)
                    {
                        continue;
                    }

                    for (int j = 0; j < Red.Length; j++)
                    {
                        RedNumber += Red[j] + " ";
                    }

                    for (int j = 0; j < Blue.Length; j++)
                    {
                        BlueNumber += Blue[j] + " ";
                    }

                    Result += RedNumber + "+ " + BlueNumber.Trim() + "|" + (GetNum(6, Red.Length) * Blue.Length).ToString() + "\n";
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

                RegexString = @"^(\d\d\s){1,5}[,](\s)(\d\d\s){2,32}[+](\s\d\d){1,16}";
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
                    string Blue_Number = "";

                    string[] Red_D = FilterRepeated(m.Value.Split('+')[0].Trim().Split(',')[0].Trim().Split(' '), 33);
                    string[] Red_T = FilterRepeated(m.Value.Split('+')[0].Trim().Split(',')[1].Trim().Split(' '), 33);
                    string[] Blue = FilterRepeated(m.Value.Trim().Split('+')[1].Trim().Split(' '), 16);

                    if (Red_D.Length + Red_T .Length + Blue.Length < 8)
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

                    for (int j = 0; j < Blue.Length; j++)
                    {
                        Blue_Number += Blue[j] + " ";
                    }

                    Result += Red_D_Number + ", " + Red_T_Number + "+ " + Blue_Number + "|" + (GetNum(6 - Red_D.Length, Red_T.Length) * Blue.Length).ToString() + "\n";
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
                Regex regex = new Regex(@"^(\d\d\s){6}[+](\s\d\d){1,4}", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                if (!regex.IsMatch(Number))
                    return false;

                string t_str = "";
                string[] WinLotteryNumber = ToSingle(Number, ref t_str, PlayType_D);

                if ((WinLotteryNumber == null) || (WinLotteryNumber.Length < 1) || (WinLotteryNumber.Length > 4))
                    return false;

                return true;
            }

            private string[] FilterRepeated(string[] NumberPart, int MaxBall)
            {
                ArrayList al = new ArrayList();
                for (int i = 0; i < NumberPart.Length; i++)
                {
                    int Ball = Shove._Convert.StrToInt(NumberPart[i], -1);
                    if ((Ball >= 1) && (Ball <= MaxBall) && (!isExistBall(al, Ball)))
                        al.Add(NumberPart[i]);
                }

                CompareToAscii compare = new CompareToAscii();
                al.Sort(compare);

                string[] Result = new string[al.Count];
                for (int i = 0; i < al.Count; i++)
                    Result[i] = al[i].ToString().PadLeft(2, '0');

                return Result;
            }

            private string[] FilterRepeated(string[] NumberPart1, string[] NumberPart2)
            {
                ArrayList al2 = new ArrayList();
                for (int i = 0; i < NumberPart2.Length; i++)
                {
                    al2.Add(NumberPart2[i]);
                }

                ArrayList al1 = new ArrayList();
                for (int i = 0; i < NumberPart1.Length; i++)
                {
                    int Ball = Shove._Convert.StrToInt(NumberPart1[i], -1);
                    if (!isExistBall(al2, Ball))
                        al1.Add(NumberPart1[i]);
                }

                CompareToAscii compare = new CompareToAscii();
                al1.Sort(compare);

                string[] Result = new string[al1.Count];
                for (int i = 0; i < al1.Count; i++)
                    Result[i] = al1[i].ToString().PadLeft(2, '0');

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
                    case "福彩投注系统2.2":
                        if (PlayTypeID == PlayType_D)
                        {
                            return GetPrintKeyList_FCTZST2_2_D(Numbers);
                        }
                        if (PlayTypeID == PlayType_F)
                        {
                            return GetPrintKeyList_FCTZST2_2_F(Numbers);
                        }
                        break;
                    case "FCR8000":
                        if (PlayTypeID == PlayType_D)
                        {
                            return GetPrintKeyList_FCR8000_D(Numbers);
                        }
                        if (PlayTypeID == PlayType_F)
                        {
                            return GetPrintKeyList_FCR8000_F(Numbers);
                        }
                        break;
                    case "LT-E":
                        if (PlayTypeID == PlayType_D)
                        {
                            return GetPrintKeyList_LT_E_D(Numbers);
                        }
                        if (PlayTypeID == PlayType_F)
                        {
                            return GetPrintKeyList_LT_E_F(Numbers);
                        }
                        break;
                    case "LT-E02":
                        if (PlayTypeID == PlayType_D)
                        {
                            return GetPrintKeyList_LT_E02_D(Numbers);
                        }
                        if (PlayTypeID == PlayType_F)
                        {
                            return GetPrintKeyList_LT_E02_F(Numbers);
                        }
                        break;
                    case "SN-3000CQA":
                        if (PlayTypeID == PlayType_D)
                        {
                            return GetPrintKeyList_SN_3000CQA_D(Numbers);
                        }
                        if (PlayTypeID == PlayType_F)
                        {
                            return GetPrintKeyList_SN_3000CQA_F(Numbers);
                        }
                        break;
                    case "SN-2000":
                        if (PlayTypeID == PlayType_D)
                        {
                            return GetPrintKeyList_SN_2000_D(Numbers);
                        }
                        if (PlayTypeID == PlayType_F)
                        {
                            return GetPrintKeyList_SN_2000_F(Numbers);
                        }
                        break;
                    case "SN_3000CG":
                        if (PlayTypeID == PlayType_D)
                        {
                            return GetPrintKeyList_SN_3000CG_D(Numbers);
                        }
                        if (PlayTypeID == PlayType_F)
                        {
                            return GetPrintKeyList_SN_3000CG_F(Numbers);
                        }
                        break;
                }

                return "";
            }
            #region GetPrintKeyList 的具体方法
            private string GetPrintKeyList_FCTZST2_2_D(string[] Numbers)
            {
                string KeyList = "";

                foreach (string Number in Numbers)
                {
                    string str = Number.Replace("+", "").Replace(" ", "").Replace("\r", "").Replace("\n", "");
                    if (str.Length != 14)
                    {
                        continue;
                    }

                    foreach (char ch in str)
                    {
                        KeyList += "[" + ch.ToString() + "]";
                    }
                }

                return KeyList;
            }
            private string GetPrintKeyList_FCTZST2_2_F(string[] Numbers)
            {
                string KeyList = "";

                foreach (string Number in Numbers)
                {
                    string[] strs = Number.Split('+');

                    if (strs.Length != 2)
                    {
                        return "";
                    }

                    string Red = strs[0].Replace(" ", "");
                    string Blue = strs[1].Replace(" ", "").Replace("\r", "").Replace("\n", "");

                    foreach (char ch in Blue)
                    {
                        KeyList += "[" + ch.ToString() + "]";
                    }

                    KeyList += "[ENTER]";

                    foreach (char ch in Red)
                    {
                        KeyList += "[" + ch.ToString() + "]";
                    }
                }

                return KeyList;
            }

            private string GetPrintKeyList_FCR8000_D(string[] Numbers)
            {
                string KeyList = "";

                foreach (string Number in Numbers)
                {
                    string str = Number.Replace("+", "").Replace(" ", "").Replace("\r", "").Replace("\n", "");
                    if (str.Length != 14)
                    {
                        continue;
                    }

                    foreach (char ch in str)
                    {
                        KeyList += "[" + ch.ToString() + "]";
                    }
                }

                return KeyList;
            }
            private string GetPrintKeyList_FCR8000_F(string[] Numbers)
            {
                string KeyList = "";

                foreach (string Number in Numbers)
                {
                    string[] strs = Number.Split('+');

                    if (strs.Length != 2)
                    {
                        return "";
                    }

                    string Red = strs[0].Replace(" ", "");
                    string Blue = strs[1].Replace(" ", "").Replace("\r", "").Replace("\n", "");

                    foreach (char ch in Blue)
                    {
                        KeyList += "[" + ch.ToString() + "]";
                    }

                    KeyList += "[ENTER]";

                    foreach (char ch in Red)
                    {
                        KeyList += "[" + ch.ToString() + "]";
                    }
                }

                return KeyList;
            }

            private string GetPrintKeyList_LT_E_D(string[] Numbers)
            {
                string KeyList = "";

                foreach (string Number in Numbers)
                {
                    string str = Number.Replace("+", "").Replace(" ", "").Replace("\r", "").Replace("\n", "");

                    str = str.Replace("0", "O").Replace("1", "Q").Replace("2", "R").Replace("3", "1").Replace("4", "S").Replace("5", "T").Replace("6", "4").Replace("7", "U").Replace("8", "V").Replace("9", "7");

                    if (str.Length != 14)
                    {
                        continue;
                    }

                    foreach (char ch in str)
                    {
                        KeyList += "[" + ch.ToString() + "]";
                    }
                }

                return KeyList;
            }
            private string GetPrintKeyList_LT_E_F(string[] Numbers)
            {
                string KeyList = "";

                foreach (string Number in Numbers)
                {
                    string[] strs = Number.Split('+');

                    if (strs.Length != 2)
                    {
                        return "";
                    }

                    string Red = strs[0].Replace(" ", "");
                    string Blue = strs[1].Replace(" ", "").Replace("\r", "").Replace("\n", "");

                    KeyList += "[" + Convert.ToString((int)(Red.Length / 2)) + "]";
                    KeyList += "[" + Shove._Convert.Chr(21).ToString() + "]";

                    KeyList += "[" + Convert.ToString((int)(Blue.Length / 2)) + "]";
                    KeyList += "[" + Shove._Convert.Chr(21).ToString() + "]";


                    foreach (char ch in Red)
                    {
                        KeyList += "[" + ch.ToString() + "]";
                    }

                    KeyList += "[" + Shove._Convert.Chr(21).ToString() + "]";

                    foreach (char ch in Blue)
                    {
                        KeyList += "[" + ch.ToString() + "]";
                    }
                }

                KeyList = KeyList.Replace("0", "O").Replace("1", "Q").Replace("2", "R").Replace("3", "1").Replace("4", "S").Replace("5", "T").Replace("6", "4").Replace("7", "U").Replace("8", "V").Replace("9", "7");
                return KeyList;
            }

            private string GetPrintKeyList_LT_E02_D(string[] Numbers)
            {
                string KeyList = "";

                foreach (string Number in Numbers)
                {
                    string str = Number.Replace("+", "").Replace(" ", "").Replace("\r", "").Replace("\n", "");

                    str = str.Replace("0", "O").Replace("1", "3").Replace("2", "A").Replace("3", "B").Replace("4", "6").Replace("5", "C").Replace("6", "D").Replace("7", "9").Replace("8", "E").Replace("9", "F");

                    if (str.Length != 14)
                    {
                        continue;
                    }

                    foreach (char ch in str)
                    {
                        KeyList += "[" + ch.ToString() + "]";
                    }
                }

                return KeyList;
            }
            private string GetPrintKeyList_LT_E02_F(string[] Numbers)
            {
                string KeyList = "";

                foreach (string Number in Numbers)
                {
                    string[] strs = Number.Split('+');

                    if (strs.Length != 2)
                    {
                        return "";
                    }

                    string Red = strs[0].Replace(" ", "");
                    string Blue = strs[1].Replace(" ", "").Replace("\r", "").Replace("\n", "");

                    KeyList += "[" + Convert.ToString((int)(Red.Length / 2)) + "]";
                    KeyList += "[" + Shove._Convert.Chr(21).ToString() + "]";

                    KeyList += "[" + Convert.ToString((int)(Blue.Length / 2)) + "]";
                    KeyList += "[" + Shove._Convert.Chr(21).ToString() + "]";


                    foreach (char ch in Red)
                    {
                        KeyList += "[" + ch.ToString() + "]";
                    }

                    KeyList += "[" + Shove._Convert.Chr(21).ToString() + "]";

                    foreach (char ch in Blue)
                    {
                        KeyList += "[" + ch.ToString() + "]";
                    }
                }

                KeyList = KeyList.Replace("0", "O").Replace("1", "3").Replace("2", "A").Replace("3", "B").Replace("4", "6").Replace("5", "C").Replace("6", "D").Replace("7", "9").Replace("8", "E").Replace("9", "F");
                return KeyList;
            }

            private string GetPrintKeyList_SN_3000CQA_D(string[] Numbers)
            {
                string KeyList = "";

                foreach (string Number in Numbers)
                {
                    string str = Number.Replace("+", "").Replace(" ", "").Replace("\r", "").Replace("\n", "");
                    if (str.Length != 14)
                    {
                        continue;
                    }

                    foreach (char ch in str)
                    {
                        KeyList += "[" + ch.ToString() + "]";
                    }
                }

                return KeyList;
            }
            private string GetPrintKeyList_SN_3000CQA_F(string[] Numbers)
            {
                string KeyList = "";

                foreach (string Number in Numbers)
                {
                    string[] strs = Number.Split('+');

                    if (strs.Length != 2)
                    {
                        return "";
                    }

                    string Red = strs[0].Replace(" ", "");
                    string Blue = strs[1].Replace(" ", "").Replace("\r", "").Replace("\n", "");

                    foreach (char ch in Blue)
                    {
                        KeyList += "[" + ch.ToString() + "]";
                    }

                    KeyList += "[ENTER]";

                    foreach (char ch in Red)
                    {
                        KeyList += "[" + ch.ToString() + "]";
                    }
                }

                return KeyList;
            }

            private string GetPrintKeyList_SN_2000_D(string[] Numbers)
            {
                string KeyList = "";

                foreach (string Number in Numbers)
                {
                    string str = Number.Replace("+", "").Replace(" ", "").Replace("\r", "").Replace("\n", "");
                    if (str.Length != 14)
                    {
                        continue;
                    }

                    foreach (char ch in str)
                    {
                        KeyList += "[" + ch.ToString() + "]";
                    }
                }

                return KeyList;
            }
            private string GetPrintKeyList_SN_2000_F(string[] Numbers)
            {
                string KeyList = "";

                foreach (string Number in Numbers)
                {
                    string[] strs = Number.Split('+');

                    if (strs.Length != 2)
                    {
                        return "";
                    }

                    string Red = strs[0].Replace(" ", "");
                    string Blue = strs[1].Replace(" ", "").Replace("\r", "").Replace("\n", "");

                    foreach (char ch in Red)
                    {
                        KeyList += "[" + ch.ToString() + "]";
                    }

                    KeyList += "[ENTER]";

                    foreach (char ch in Blue)
                    {
                        KeyList += "[" + ch.ToString() + "]";
                    }
                }

                return KeyList;
            }

            private string GetPrintKeyList_SN_3000CG_D(string[] Numbers)
            {
                string KeyList = "";

                foreach (string Number in Numbers)
                {
                    string str = Number.Replace("+", "").Replace(" ", "").Replace("\r", "").Replace("\n", "");
                    if (str.Length != 14)
                    {
                        continue;
                    }

                    foreach (char ch in str)
                    {
                        KeyList += "[" + ch.ToString() + "]";
                    }
                }

                return KeyList;
            }
            private string GetPrintKeyList_SN_3000CG_F(string[] Numbers)
            {
                string KeyList = "";

                foreach (string Number in Numbers)
                {
                    string[] strs = Number.Split('+');

                    if (strs.Length != 2)
                    {
                        return "";
                    }

                    string Red = strs[0].Replace(" ", "");
                    string Blue = strs[1].Replace(" ", "").Replace("\r", "").Replace("\n", "");

                    foreach (char ch in Red)
                    {
                        KeyList += "[" + ch.ToString() + "]";
                    }

                    KeyList += "[ENTER]";

                    foreach (char ch in Blue)
                    {
                        KeyList += "[" + ch.ToString() + "]";
                    }

                    KeyList += "[ENTER]";
                }

                return KeyList;
            }
            #endregion

            //参数说明 PlayTypeID:玩法类别; Number:投注号码; Multiple: 倍数; MaxMultiple:最大倍数;
            public override Ticket[] ToElectronicTicket_HPCQ(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                if (PlayTypeID == PlayType_D)
                {
                    return ToElectronicTicket_HPCQ_D(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                if (PlayTypeID == PlayType_F)
                {
                    return ToElectronicTicket_HPCQ_F(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                if (PlayTypeID == PlayType_DanT)
                {
                    return ToElectronicTicket_HPCQ_DT(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                return null;
            }
            #region ToElectronicTicket_HPCQ 的具体方法
            private Ticket[] ToElectronicTicket_HPCQ_D(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
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

                        Money = EachMoney * EachMultiple;

                        al.Add(new Ticket(101, ConvertFormatToElectronTicket_HPCQ(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));
                    }
                }

                Ticket[] Tickets = new Ticket[al.Count];

                for (int i = 0; i < Tickets.Length; i++)
                {
                    Tickets[i] = (Ticket)al[i];
                }

                return Tickets;

            }
            private Ticket[] ToElectronicTicket_HPCQ_F(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
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

                        Money = EachMoney * EachMultiple;

                        al.Add(new Ticket(102, ConvertFormatToElectronTicket_HPCQ(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));
                    }
                }

                Ticket[] Tickets = new Ticket[al.Count];

                for (int i = 0; i < Tickets.Length; i++)
                {
                    Tickets[i] = (Ticket)al[i];
                }

                return Tickets;
            }
            private Ticket[] ToElectronicTicket_HPCQ_DT(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                string[] strs = AnalyseSchemeToElectronicTicket_DT(Number, PlayTypeID).Split('\n');

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

                        Money = EachMoney * EachMultiple;

                        al.Add(new Ticket(101, ConvertFormatToElectronTicket_HPCQ(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));
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
            private string ConvertFormatToElectronTicket_HPCQ(int PlayTypeID, string Number)
            {
                Number = Number.Trim();

                string Ticket = "";

                if (PlayTypeID == PlayType_D || PlayTypeID == PlayType_F || PlayTypeID == PlayType_DanT)
                {
                    Ticket = Number.Replace(" ", ",").Replace(",+,", "#");
                }

                return Ticket;
            }
            #endregion

            //参数说明 PlayTypeID:玩法类别; Number:投注号码; Multiple: 倍数; MaxMultiple:最大倍数;
            public override Ticket[] ToElectronicTicket_HPSH(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                if (PlayTypeID == PlayType_D)
                {
                    return ToElectronicTicket_HPSH_D(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                if (PlayTypeID == PlayType_F)
                {
                    return ToElectronicTicket_HPSH_F(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                if (PlayTypeID == PlayType_DanT)
                {
                    return ToElectronicTicket_HPSH_DT(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
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

                        al.Add(new Ticket(101, ConvertFormatToElectronTicket_HPSH(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));
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

                        al.Add(new Ticket(102, ConvertFormatToElectronTicket_HPSH(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));
                    }
                }

                Ticket[] Tickets = new Ticket[al.Count];

                for (int i = 0; i < Tickets.Length; i++)
                {
                    Tickets[i] = (Ticket)al[i];
                }

                return Tickets;
            }
            private Ticket[] ToElectronicTicket_HPSH_DT(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                string[] strs = AnalyseSchemeToElectronicTicket_DT(Number, PlayTypeID).Split('\n');

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
                                if (strs[i + M] == "")
                                {
                                    continue;
                                }

                                try
                                {
                                    Numbers += strs[i + M].ToString().Split('|')[0] + "\n";
                                    EachMoney += 2 * double.Parse(strs[i + M].ToString().Split('|')[1]);
                                }
                                catch
                                {
                                }
                            }
                        }

                        Money += EachMoney * EachMultiple;

                        al.Add(new Ticket(101, ConvertFormatToElectronTicket_HPCQ(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));
                    }
                }

                Ticket[] Tickets = new Ticket[al.Count];

                for (int i = 0; i < Tickets.Length; i++)
                {
                    Tickets[i] = (Ticket)al[i];
                }

                return Tickets;
            }

            private string ConvertFormatToElectronTicket_HPSH(int PlayTypeID, string Number)
            {
                Number = Number.Trim();

                string Ticket = "";

                if (PlayTypeID == PlayType_D || PlayTypeID == PlayType_F)
                {
                    Ticket = Number.Replace(" ", ",").Replace(",+,", "#");
                }

                return Ticket;
            }
            #endregion

            public override Ticket[] ToElectronicTicket_HPJX(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                if (PlayTypeID == PlayType_D)
                {
                    return ToElectronicTicket_HPJX_D(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                if (PlayTypeID == PlayType_F)
                {
                    return ToElectronicTicket_HPJX_F(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                if (PlayTypeID == PlayType_DanT)
                {
                    return ToElectronicTicket_HPJX_DT(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                return null;
            }
            #region ToElectronicTicket_HPJX 的具体方法
            private Ticket[] ToElectronicTicket_HPJX_D(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
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

                        al.Add(new Ticket(101, ConvertFormatToElectronTicket_HPJX(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));
                    }
                }

                Ticket[] Tickets = new Ticket[al.Count];

                for (int i = 0; i < Tickets.Length; i++)
                {
                    Tickets[i] = (Ticket)al[i];
                }

                return Tickets;

            }
            private Ticket[] ToElectronicTicket_HPJX_F(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
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

                        al.Add(new Ticket(102, ConvertFormatToElectronTicket_HPJX(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));
                    }
                }

                Ticket[] Tickets = new Ticket[al.Count];

                for (int i = 0; i < Tickets.Length; i++)
                {
                    Tickets[i] = (Ticket)al[i];
                }

                return Tickets;
            }
            private Ticket[] ToElectronicTicket_HPJX_DT(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                string[] strs = AnalyseSchemeToElectronicTicket_DT(Number, PlayTypeID).Split('\n');

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
                                if (strs[i + M] == "")
                                {
                                    continue;
                                }

                                try
                                {
                                    Numbers += strs[i + M].ToString().Split('|')[0] + "\n";
                                    EachMoney += 2 * double.Parse(strs[i + M].ToString().Split('|')[1]);
                                }
                                catch
                                {
                                }
                            }
                        }

                        Money += EachMoney * EachMultiple;

                        al.Add(new Ticket(101, ConvertFormatToElectronTicket_HPCQ(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));
                    }
                }

                Ticket[] Tickets = new Ticket[al.Count];

                for (int i = 0; i < Tickets.Length; i++)
                {
                    Tickets[i] = (Ticket)al[i];
                }

                return Tickets;
            }

            private string ConvertFormatToElectronTicket_HPJX(int PlayTypeID, string Number)
            {
                Number = Number.Trim();

                string Ticket = "";

                if (PlayTypeID == PlayType_D || PlayTypeID == PlayType_F)
                {
                    Ticket = Number.Replace(" ", ",").Replace(",+,", "#");
                }

                return Ticket;
            }
            #endregion

            //参数说明 PlayTypeID:玩法类别; Number:投注号码; Multiple: 倍数; MaxMultiple:最大倍数;
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
                    Ticket = Number.Replace(" ", "").Replace("+", "|").Replace(",", "*");
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
                if (PlayTypeID == PlayType_DanT)
                {
                    return ToElectronicTicket_DYJ_DT(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
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
            private Ticket[] ToElectronicTicket_DYJ_DT(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                string[] strs = AnalyseSchemeToElectronicTicket_DT(Number, PlayTypeID).Split('\n');

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
                                if (strs[i + M] == "")
                                {
                                    continue;
                                }

                                try
                                {
                                    Numbers += strs[i + M].ToString().Split('|')[0] + ";\n";
                                    EachMoney += 2 * double.Parse(strs[i + M].ToString().Split('|')[1]);
                                }
                                catch
                                {
                                }
                            }
                        }

                        Money += EachMoney * EachMultiple;

                        al.Add(new Ticket(101, ConvertFormatToElectronTicket_HPCQ(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));
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
                    Ticket = Number.Replace(" + ", "-");
                }

                return Ticket;
            }
            #endregion

            private string AnalyseSchemeToElectronicTicket_DT(string Content, int PlayType)
            {
                string[] strs = Content.Split('\n');
                if (strs == null)
                    return "";
                if (strs.Length == 0)
                    return "";

                string Result = "";

                string RegexString;

                RegexString = @"(\d\d\s){1,5}[,](\s)(\d\d\s){2,31}[+](\s\d\d){1,16}";
                Regex regex = new Regex(RegexString, RegexOptions.Compiled | RegexOptions.IgnoreCase);

                for (int i = 0; i < strs.Length; i++)
                {
                    Match m = regex.Match(strs[i]);
                    if (m.Success)
                    {
                        string CanonicalNumber = "";
                        string[] singles = ToSingle_DanT(m.Value, ref CanonicalNumber);
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
        }
    }
}
