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
        /// 体彩超级大乐透
        /// </summary>
        public partial class TCCJDLT : LotteryBase
        {
            #region 静态变量
            public const int PlayType_D = 3901;
            public const int PlayType_F = 3902;
            public const int PlayType_ZJ_D = 3903;
            public const int PlayType_ZJ_F = 3904;
            public const int PlayType_2_D = 3905;
            public const int PlayType_2_F = 3906;
            public const int PlayType_DT = 3907;
            public const int PlayType_ZJ_DT = 3908;

            public const int ID = 39;
            public const string sID = "39";
            public const string Name = "体彩超级大乐透";
            public const string Code = "TCCJDLT";
            public const double MaxMoney = 30000;
            #endregion

            public TCCJDLT()
            {
                id = 39;
                name = "体彩超级大乐透";
                code = "TCCJDLT";
            }

            public override bool CheckPlayType(int play_type)
            {
                return ((play_type >= 3901) && (play_type <= 3908));
            }

            public override PlayType[] GetPlayTypeList()
            {
                PlayType[] Result = new PlayType[8];

                Result[0] = new PlayType(PlayType_D, "单式");
                Result[1] = new PlayType(PlayType_F, "复式");
                Result[2] = new PlayType(PlayType_ZJ_D, "追加单式");
                Result[3] = new PlayType(PlayType_ZJ_F, "追加复式");
                Result[4] = new PlayType(PlayType_2_D, "12选2单式");
                Result[5] = new PlayType(PlayType_2_F, "12选2复式");
                Result[6] = new PlayType(PlayType_DT, "胆托");
                Result[7] = new PlayType(PlayType_ZJ_DT, "追号胆托");

                return Result;
            }

            public override string BuildNumber(int Red, int Blue, int Num)	//id = 39
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
                            Ball = rd.Next(1, 35 + 1);
                        al_r.Add(Ball.ToString().PadLeft(2, '0'));
                    }

                    for (int j = 0; j < Blue; j++)
                    {
                        int Ball = 0;
                        while ((Ball == 0) || isExistBall(al_b, Ball))
                            Ball = rd.Next(1, 12 + 1);
                        al_b.Add(Ball.ToString().PadLeft(2, '0'));
                    }

                    CompareToAscii compare = new CompareToAscii();
                    al_r.Sort(compare);
                    al_b.Sort(compare);

                    string LotteryNumber = "";
                    for (int j = 0; j < al_r.Count; j++)
                    {
                        LotteryNumber += al_r[j].ToString() + " ";
                    }

                    if (LotteryNumber != "")
                    {
                        LotteryNumber += "+ ";
                    }

                    for (int j = 0; j < al_b.Count; j++)
                    {
                        LotteryNumber += al_b[j].ToString() + " ";
                    }

                    sb.Append(LotteryNumber.Trim() + "\n");
                }

                string Result = sb.ToString();
                Result = Result.Substring(0, Result.Length - 1);
                return Result;
            }

            public override string[] ToSingle(string Number, ref string CanonicalNumber, int PlayType)
            {
                if ((PlayType == PlayType_D) || (PlayType == PlayType_F) || (PlayType == PlayType_ZJ_D) || (PlayType == PlayType_ZJ_F))
                    return ToSingle_0(Number, ref CanonicalNumber);

                if ((PlayType == PlayType_2_D) || (PlayType == PlayType_2_F))
                    return ToSingle_12X2(Number, ref CanonicalNumber);

                if ((PlayType == PlayType_DT) || (PlayType == PlayType_ZJ_DT))
                    return ToSingle_DanT(Number, ref CanonicalNumber);

                return null;
            }
            #region ToSingle 的具体方法
            private string[] ToSingle_0(string Number, ref string CanonicalNumber)	//复式取单式, 后面 ref 参数是将彩票规范化，如：01 01 02... 变成 01 02...
            {
                string[] strs = Number.Split('+');
                CanonicalNumber = "";

                if (strs.Length != 2)
                {
                    CanonicalNumber = "";
                    return null;
                }

                string[] Red = FilterRepeated(strs[0].Trim().Split(' '), 35);
                string[] Blue = FilterRepeated(strs[1].Trim().Split(' '), 12);

                if ((Red.Length < 5) || (Blue.Length < 2))
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
                int m = Blue.Length;

                for (int i = 0; i < n - 4; i++)
                    for (int j = i + 1; j < n - 3; j++)
                        for (int k = j + 1; k < n - 2; k++)
                            for (int x = k + 1; x < n - 1; x++)
                                for (int y = x + 1; y < n; y++)

                                    for (int z = 0; z < m - 1; z++)
                                        for (int a = z + 1; a < m; a++)
                                            al.Add(Red[i] + " " + Red[j] + " " + Red[k] + " " + Red[x] + " " + Red[y] + " + " + Blue[z] + " " + Blue[a]);

                #endregion

                string[] Result = new string[al.Count];
                for (int i = 0; i < al.Count; i++)
                    Result[i] = al[i].ToString();
                return Result;
            }
            private string[] ToSingle_12X2(string Number, ref string CanonicalNumber)	//复式取单式, 后面 ref 参数是将彩票规范化，如：01 01 02... 变成 01 02...
            {
                string[] strs = FilterRepeated(Number.Trim().Split(' '), 12);
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
            private string[] ToSingle_DanT(string Number, ref string CanonicalNumber)	//胆拖取单式, 后面 ref 参数是将彩票规范化，如：01 01 02... 变成 01 02...
            {
                string[] strs = Number.Split('+');
                CanonicalNumber = "";

                if (strs.Length != 2)
                {
                    CanonicalNumber = "";
                    return null;
                }

                string[] RedDan = FilterRepeated(strs[0].Trim().Split(',')[0].Trim().Split(' '), 35);
                string[] RedTuo = FilterRepeated(strs[0].Trim().Split(',')[1].Trim().Split(' '), 35);
                string[] Blue = FilterRepeated(strs[1].Trim().Split(' '), 12);

                string[] TempRedDan = FilterRepeated(RedDan, RedTuo);

                if (((TempRedDan.Length + RedTuo.Length) < 6) || (Blue.Length < 2) || (TempRedDan.Length > 4))
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
                    for (int k = m; k < n - 3; k++)
                        for (int x = k + 1; x < n - 2; x++)
                            for (int y = x + 1; y < n - 1; y++)
                                for (int z = y + 1; z < n; z++)
                                    for (int b = 0; b < Blue.Length - 1; b++)
                                        for (int a = b + 1; a < Blue.Length; a++)
                                        {
                                            al.Add(Numbers + Red[k] + " " + Red[x] + " " + Red[y] + " " + Red[z] + " + " + Blue[b] + " " + Blue[a]);
                                        }
                }
                else if (m == 2)
                {
                    for (int x = m; x < n - 2; x++)
                        for (int y = x + 1; y < n - 1; y++)
                            for (int z = y + 1; z < n; z++)
                                for (int b = 0; b < Blue.Length - 1; b++)
                                    for (int a = b + 1; a < Blue.Length; a++)
                                    {
                                        al.Add(Numbers + Red[x] + " " + Red[y] + " " + Red[z] + " + " + Blue[b] + " " + Blue[a]);
                                    }
                }
                else if (m == 3)
                {
                    for (int y = m; y < n - 1; y++)
                        for (int z = y + 1; z < n; z++)
                            for (int b = 0; b < Blue.Length - 1; b++)
                                for (int a = b + 1; a < Blue.Length; a++)
                                {
                                    al.Add(Numbers + Red[y] + " " + Red[z] + " + " + Blue[b] + " " + Blue[a]);
                                }
                }
                else if (m == 4)
                {
                    for (int z = m; z < n; z++)
                        for (int b = 0; b < Blue.Length - 1; b++)
                            for (int a = b + 1; a < Blue.Length; a++)
                            {
                                al.Add(Numbers + Red[z] + " + " + Blue[b] + " " + Blue[a]);
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
                if ((WinMoneyList == null) || (WinMoneyList.Length < 30))
                    return -3;

                if ((PlayType == PlayType_D) || (PlayType == PlayType_F))
                    return ComputeWin_0(PlayType, Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[0], WinMoneyList[1], WinMoneyList[4], WinMoneyList[5], WinMoneyList[8], WinMoneyList[9], WinMoneyList[12], WinMoneyList[13], WinMoneyList[16], WinMoneyList[17], WinMoneyList[20], WinMoneyList[21], WinMoneyList[24], WinMoneyList[25], WinMoneyList[28], WinMoneyList[29]);

                if ((PlayType == PlayType_ZJ_D) || (PlayType == PlayType_ZJ_F))
                    return ComputeWin_ZJ(PlayType, Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[0], WinMoneyList[1], WinMoneyList[2], WinMoneyList[3], WinMoneyList[4], WinMoneyList[5], WinMoneyList[6], WinMoneyList[7], WinMoneyList[8], WinMoneyList[9], WinMoneyList[10], WinMoneyList[11], WinMoneyList[12], WinMoneyList[13], WinMoneyList[14], WinMoneyList[15], WinMoneyList[16], WinMoneyList[17], WinMoneyList[18], WinMoneyList[19], WinMoneyList[20], WinMoneyList[21], WinMoneyList[22], WinMoneyList[23], WinMoneyList[24], WinMoneyList[25], WinMoneyList[26], WinMoneyList[27], WinMoneyList[28], WinMoneyList[29]);

                if ((PlayType == PlayType_2_D) || (PlayType == PlayType_2_F))
                    return ComputeWin_12X2(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[30], WinMoneyList[31]);

                if (PlayType == PlayType_DT)
                    return ComputeWin_DT(PlayType, Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[0], WinMoneyList[1], WinMoneyList[4], WinMoneyList[5], WinMoneyList[8], WinMoneyList[9], WinMoneyList[12], WinMoneyList[13], WinMoneyList[16], WinMoneyList[17], WinMoneyList[20], WinMoneyList[21], WinMoneyList[24], WinMoneyList[25], WinMoneyList[28], WinMoneyList[29]);

                if (PlayType == PlayType_ZJ_DT)
                    return ComputeWin_ZJ_DT(PlayType, Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[0], WinMoneyList[1], WinMoneyList[2], WinMoneyList[3], WinMoneyList[4], WinMoneyList[5], WinMoneyList[6], WinMoneyList[7], WinMoneyList[8], WinMoneyList[9], WinMoneyList[10], WinMoneyList[11], WinMoneyList[12], WinMoneyList[13], WinMoneyList[14], WinMoneyList[15], WinMoneyList[16], WinMoneyList[17], WinMoneyList[18], WinMoneyList[19], WinMoneyList[20], WinMoneyList[21], WinMoneyList[22], WinMoneyList[23], WinMoneyList[24], WinMoneyList[25], WinMoneyList[26], WinMoneyList[27], WinMoneyList[28], WinMoneyList[29]);

                return -4;
            }
            #region ComputeWin 的具体方法
            private double ComputeWin_0(int PlayType, string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney1, double WinMoneyNoWithTax1, double WinMoney2, double WinMoneyNoWithTax2, double WinMoney3, double WinMoneyNoWithTax3, double WinMoney4, double WinMoneyNoWithTax4, double WinMoney5, double WinMoneyNoWithTax5, double WinMoney6, double WinMoneyNoWithTax6, double WinMoney7, double WinMoneyNoWithTax7, double WinMoney8, double WinMoneyNoWithTax8)
            {
                WinNumber = WinNumber.Trim();
                if (WinNumber.Length < 22)	//22: "01 02 03 04 05 + 01 02" 
                    return -1;
                string[] Lotterys = SplitLotteryNumber(Number);
                if (Lotterys == null)
                    return -2;
                if (Lotterys.Length < 1)
                    return -2;

                string WinNumber_Red = WinNumber.Substring(0, 15);
                string WinNumber_Blue = WinNumber.Substring(17, 5).Trim();

                int Description1 = 0, Description2 = 0, Description3 = 0, Description4 = 0, Description5 = 0, Description6 = 0, Description7 = 0, Description8 = 0;
                double WinMoney = 0;
                WinMoneyNoWithTax = 0;
                Description = "";

               string RegexString;
               if (PlayType == PlayType_D)
                   RegexString = @"^(\d\d\s){5}[+](\s\d\d){2}";
               else
                   RegexString = @"^(\d\d\s){5,35}[+](\s\d\d){2,12}";
                Regex regex = new Regex(RegexString, RegexOptions.Compiled | RegexOptions.IgnoreCase);

                int RedRight = 0;

                int BlueRight = 0;

                string[] Red = null;
                string[] Blue = null;

                for (int ii = 0; ii < Lotterys.Length; ii++)
                {
                    RedRight = 0;
                    BlueRight = 0;

                    Match m = regex.Match(Lotterys[ii]);

                    if (!m.Success)
                    {
                        continue;
                    }

                    Red = FilterRepeated(m.Value.Trim().Split('+')[0].Trim().Split(' '), 35);
                    Blue = FilterRepeated(m.Value.Trim().Split('+')[1].Trim().Split(' '), 12);

                    if (Red.Length < 5)
                    {
                        continue;
                    }

                    if (Blue.Length < 2)
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
                            if (WinNumber_Blue.IndexOf(str.Trim()) >= 0)
                            {
                                BlueRight++;
                            }
                        }
                    }

                    if (((RedRight == 0) && (BlueRight == 0)) || (RedRight + BlueRight) < 2)
                    {
                        continue;
                    }

                    if ((RedRight == 5) && (BlueRight == 2))
                    {
                        Description1++;
                        WinMoney += WinMoney1;
                        WinMoneyNoWithTax += WinMoneyNoWithTax1;
                    }

                    if ((RedRight == 5) && (BlueRight >= 1))
                    {
                        int Count = 0;

                        Count = GetNum(1, BlueRight) * GetNum(1, Blue.Length - BlueRight);

                        if (Count > 0)
                        {
                            Description2 = Description2 + Count;
                            WinMoney += WinMoney2 * Count;
                            WinMoneyNoWithTax += WinMoneyNoWithTax2 * Count;
                        }
                    }

                    if ((RedRight == 5) && (Blue.Length - BlueRight > 1))
                    {
                        int Count = GetNum(2, Blue.Length - BlueRight);

                        if (Count > 0)
                        {
                            Description3 = Description3 + Count;
                            WinMoney += WinMoney3 * Count;
                            WinMoneyNoWithTax += WinMoneyNoWithTax3 * Count;
                        }
                    }

                    if ((RedRight >= 4) && (BlueRight == 2))
                    {
                        int Count = GetNum(4, RedRight) * GetNum(1, Red.Length - RedRight);

                        if (Count > 0)
                        {
                            Description4 = Description4 + Count;
                            WinMoney += WinMoney4 * Count;
                            WinMoneyNoWithTax += WinMoneyNoWithTax4 * Count;
                        }
                    }

                    if ((RedRight >= 4) && (BlueRight >= 1) && (Red.Length - RedRight > 0) && (Blue.Length - BlueRight > 0))
                    {
                        int Count = GetNum(4, RedRight) * GetNum(1, Red.Length - RedRight) * GetNum(1, BlueRight) * GetNum(1, Blue.Length - BlueRight);

                        if (Count > 0)
                        {
                            Description5 = Description5 + Count;
                            WinMoney += WinMoney5 * Count;
                            WinMoneyNoWithTax += WinMoneyNoWithTax5 * Count;
                        }
                    }

                    if ((RedRight >= 4) && (Blue.Length - BlueRight > 1) && (Red.Length - RedRight > 0))
                    {
                        int Count = GetNum(4, RedRight) * GetNum(1, Red.Length - RedRight) * GetNum(2, Blue.Length - BlueRight);

                        if (Count > 0)
                        {
                            Description6 = Description6 + Count;
                            WinMoney += WinMoney6 * Count;
                            WinMoneyNoWithTax += WinMoneyNoWithTax6 * Count;
                        }
                    }

                    if ((RedRight >= 3) && (BlueRight >= 2) && (Red.Length - RedRight > 1))
                    {
                        int Count = GetNum(3, RedRight) * GetNum(2, Red.Length - RedRight);

                        if (Count > 0)
                        {
                            Description6 = Description6 + Count;
                            WinMoney += WinMoney6 * Count;
                            WinMoneyNoWithTax += WinMoneyNoWithTax6 * Count;
                        }
                    }

                    if ((RedRight >= 3) && (BlueRight >= 1) && (Red.Length - RedRight > 1) && (Blue.Length - BlueRight > 0))
                    {
                        int Count = GetNum(3, RedRight) * GetNum(2, Red.Length - RedRight) * GetNum(1, BlueRight) * GetNum(1, Blue.Length - BlueRight);

                        if (Count > 0)
                        {
                            Description7 = Description7 + Count;
                            WinMoney += WinMoney7 * Count;
                            WinMoneyNoWithTax += WinMoneyNoWithTax7 * Count;
                        }
                    }

                    if ((RedRight >= 2) && (BlueRight >= 2) && (Red.Length - RedRight > 2))
                    {
                         int Count = GetNum(2, RedRight) * GetNum(3, Red.Length - RedRight);

                         if (Count > 0)
                         {
                             Description7 = Description7 + Count;
                             WinMoney += WinMoney7 * Count;
                             WinMoneyNoWithTax += WinMoneyNoWithTax7 * Count;
                         }
                    }

                    if ((RedRight >= 3) && (Blue.Length - BlueRight > 1) && (Red.Length - RedRight > 1))
                    {
                        int Count = GetNum(3, RedRight) * GetNum(2, Red.Length - RedRight) * GetNum(2, Blue.Length - BlueRight);

                        if (Count > 0)
                        {
                            Description8 =Description8 +  Count;
                            WinMoney += WinMoney8 * Count;
                            WinMoneyNoWithTax += WinMoneyNoWithTax8 * Count;
                        }
                    }

                    if ((RedRight >= 2) && (BlueRight >= 1) && (Red.Length - RedRight > 2) && (Blue.Length - BlueRight > 0))
                    {
                        int Count = GetNum(2, RedRight) * GetNum(3, Red.Length - RedRight) * GetNum(1, BlueRight) * GetNum(1, Blue.Length - BlueRight);

                        if (Count > 0)
                        {
                            Description8 = Description8 + Count;
                            WinMoney += WinMoney8 * Count;
                            WinMoneyNoWithTax += WinMoneyNoWithTax8 * Count;
                        }
                    }

                    if ((BlueRight >= 2) && (Red.Length - RedRight) > 3)
                    {
                        int Count = 0;

                        if (RedRight > 0)
                        {
                            Count = GetNum(1, RedRight) * GetNum(4, Red.Length -  RedRight);
                        }

                        if (Red.Length - RedRight > 4)
                        {
                            Count += GetNum(5, Red.Length - RedRight);
                        }

                        if (Count > 0)
                        {
                            Description8 = Description8 + Count;
                            WinMoney += WinMoney8 * Count;
                            WinMoneyNoWithTax += WinMoneyNoWithTax8 * Count;
                        }

                    }
                }

                if (Description1 > 0)
                {
                    if (Description != "")
                        Description += "，";
                    Description = "一等奖" + Description1.ToString() + "注";
                }
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
                if (Description8 > 0)
                {
                    if (Description != "")
                        Description += "，";
                    Description += "八等奖" + Description8.ToString() + "注";
                }

                if (Description != "")
                    Description += "。";

                return WinMoney;
            }
            private double ComputeWin_ZJ(int PlayType, string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney1, double WinMoneyNoWithTax1, double ZJ_WinMoney1, double ZJ_WinMoneyNoWithTax1, double WinMoney2, double WinMoneyNoWithTax2, double ZJ_WinMoney2, double ZJ_WinMoneyNoWithTax2, double WinMoney3, double WinMoneyNoWithTax3, double ZJ_WinMoney3, double ZJ_WinMoneyNoWithTax3, double WinMoney4, double WinMoneyNoWithTax4, double ZJ_WinMoney4, double ZJ_WinMoneyNoWithTax4, double WinMoney5, double WinMoneyNoWithTax5, double ZJ_WinMoney5, double ZJ_WinMoneyNoWithTax5, double WinMoney6, double WinMoneyNoWithTax6, double ZJ_WinMoney6, double ZJ_WinMoneyNoWithTax6, double WinMoney7, double WinMoneyNoWithTax7, double ZJ_WinMoney7, double ZJ_WinMoneyNoWithTax7, double WinMoney8, double WinMoneyNoWithTax8)
            {
                WinNumber = WinNumber.Trim();
                if (WinNumber.Length < 22)	//22: "01 02 03 04 05 + 01 02" 
                    return -1;
                string[] Lotterys = SplitLotteryNumber(Number);
                if (Lotterys == null)
                    return -2;
                if (Lotterys.Length < 1)
                    return -2;

                string WinNumber_Red = WinNumber.Substring(0, 15);
                string WinNumber_Blue = WinNumber.Substring(17, 5).Trim();

                int Description1 = 0, Description2 = 0, Description3 = 0, Description4 = 0, Description5 = 0, Description6 = 0, Description7 = 0, Description8 = 0;
                double WinMoney = 0;
                WinMoneyNoWithTax = 0;
                Description = "";

                string RegexString;
                if (PlayType == PlayType_D)
                    RegexString = @"^(\d\d\s){5}[+](\s\d\d){2}";
                else
                    RegexString = @"^(\d\d\s){5,35}[+](\s\d\d){2,12}";
                Regex regex = new Regex(RegexString, RegexOptions.Compiled | RegexOptions.IgnoreCase);

                int RedRight = 0;

                int BlueRight = 0;

                string[] Red = null;
                string[] Blue = null;

                for (int ii = 0; ii < Lotterys.Length; ii++)
                {
                    RedRight = 0;
                    BlueRight = 0;

                    Match m = regex.Match(Lotterys[ii]);

                    if (!m.Success)
                    {
                        continue;
                    }

                    Red = FilterRepeated(m.Value.Trim().Split('+')[0].Trim().Split(' '), 35);
                    Blue = FilterRepeated(m.Value.Trim().Split('+')[1].Trim().Split(' '), 12);

                    if (Red.Length < 5)
                    {
                        continue;
                    }

                    if (Blue.Length < 2)
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
                            if (WinNumber_Blue.IndexOf(str.Trim()) >= 0)
                            {
                                BlueRight++;
                            }
                        }
                    }

                    if ((RedRight == 0) && (BlueRight == 0))
                    {
                        continue;
                    }

                    if ((RedRight == 5) && (BlueRight == 2))
                    {
                        Description1++;
                        WinMoney += ZJ_WinMoney1;
                        WinMoneyNoWithTax += ZJ_WinMoneyNoWithTax1;
                    }

                    if ((RedRight == 5) && (BlueRight >= 1))
                    {
                        int Count = 0;

                        Count = GetNum(1, BlueRight) * GetNum(1, Blue.Length - BlueRight);

                        if (Count > 0)
                        {
                            Description2 = Description2 + Count;
                            WinMoney += ZJ_WinMoney2 * Count;
                            WinMoneyNoWithTax += ZJ_WinMoneyNoWithTax2 * Count;
                        }
                    }

                    if ((RedRight == 5) && (Blue.Length - BlueRight > 1))
                    {
                        int Count = GetNum(2, Blue.Length - BlueRight);

                        if (Count > 0)
                        {
                            Description3 = Description3 + Count;
                            WinMoney += ZJ_WinMoney3 * Count;
                            WinMoneyNoWithTax += ZJ_WinMoneyNoWithTax3 * Count;
                        }
                    }

                    if ((RedRight >= 4) && (BlueRight == 2))
                    {
                        int Count = GetNum(4, RedRight) * GetNum(1, Red.Length - RedRight);

                        if (Count > 0)
                        {
                            Description4 = Description4 + Count;
                            WinMoney += ZJ_WinMoney4 * Count;
                            WinMoneyNoWithTax += ZJ_WinMoneyNoWithTax4 * Count;
                        }
                    }

                    if ((RedRight >= 4) && (BlueRight >= 1) && (Red.Length - RedRight > 0) && (Blue.Length - BlueRight > 0))
                    {
                        int Count = GetNum(4, RedRight) * GetNum(1, Red.Length - RedRight) * GetNum(1, BlueRight) * GetNum(1, Blue.Length - BlueRight);

                        if (Count > 0)
                        {
                            Description5 = Description5 + Count;
                            WinMoney += ZJ_WinMoney5 * Count;
                            WinMoneyNoWithTax += ZJ_WinMoneyNoWithTax5 * Count;
                        }
                    }

                    if ((RedRight >= 4) && (Blue.Length - BlueRight > 1) && (Red.Length - RedRight > 0))
                    {
                        int Count = GetNum(4, RedRight) * GetNum(1, Red.Length - RedRight) * GetNum(2, Blue.Length - BlueRight);

                        if (Count > 0)
                        {
                            Description6 = Description6 + Count;
                            WinMoney += ZJ_WinMoney6 * Count;
                            WinMoneyNoWithTax += ZJ_WinMoneyNoWithTax6 * Count;
                        }
                    }

                    if ((RedRight >= 3) && (BlueRight >= 2) && (Red.Length - RedRight > 1))
                    {
                        int Count = GetNum(3, RedRight) * GetNum(2, Red.Length - RedRight);

                        if (Count > 0)
                        {
                            Description6 = Description6 + Count;
                            WinMoney += ZJ_WinMoney6 * Count;
                            WinMoneyNoWithTax += ZJ_WinMoneyNoWithTax6 * Count;
                        }
                    }

                    if ((RedRight >= 3) && (BlueRight >= 1) && (Red.Length - RedRight > 1) && (Blue.Length - BlueRight > 0))
                    {
                        int Count = GetNum(3, RedRight) * GetNum(2, Red.Length - RedRight) * GetNum(1, BlueRight) * GetNum(1, Blue.Length - BlueRight);

                        if (Count > 0)
                        {
                            Description7 = Description7 + Count;
                            WinMoney += ZJ_WinMoney7 * Count;
                            WinMoneyNoWithTax += ZJ_WinMoneyNoWithTax7 * Count;
                        }
                    }

                    if ((RedRight >= 2) && (BlueRight >= 2) && (Red.Length - RedRight > 2))
                    {
                        int Count = GetNum(2, RedRight) * GetNum(3, Red.Length - RedRight);

                        if (Count > 0)
                        {
                            Description7 = Description7 + Count;
                            WinMoney += ZJ_WinMoney7 * Count;
                            WinMoneyNoWithTax += ZJ_WinMoneyNoWithTax7 * Count;
                        }
                    }

                    if ((RedRight >= 3) && (Blue.Length - BlueRight > 1) && (Red.Length - RedRight > 1))
                    {
                        int Count = GetNum(3, RedRight) * GetNum(2, Red.Length - RedRight) * GetNum(2, Blue.Length - BlueRight);

                        if (Count > 0)
                        {
                            Description8 = Description8 + Count;
                            WinMoney += WinMoney8 * Count;
                            WinMoneyNoWithTax += WinMoneyNoWithTax8 * Count;
                        }
                    }

                    if ((RedRight >= 2) && (BlueRight >= 1) && (Red.Length - RedRight > 2) && (Blue.Length - BlueRight > 0))
                    {
                        int Count = GetNum(2, RedRight) * GetNum(3, Red.Length - RedRight) * GetNum(1, BlueRight) * GetNum(1, Blue.Length - BlueRight);

                        if (Count > 0)
                        {
                            Description8 = Description8 + Count;
                            WinMoney += WinMoney8 * Count;
                            WinMoneyNoWithTax += WinMoneyNoWithTax8 * Count;
                        }
                    }

                    if ((BlueRight >= 2) && (Red.Length - RedRight) > 3)
                    {
                        int Count = 0;

                        if (RedRight > 0)
                        {
                            Count = GetNum(1, RedRight) * GetNum(4, Red.Length - RedRight);
                        }

                        if (Red.Length - RedRight > 4)
                        {
                            Count += GetNum(5, Red.Length - RedRight);
                        }

                        if (Count > 0)
                        {
                            Description8 = Description8 + Count;
                            WinMoney += WinMoney8 * Count;
                            WinMoneyNoWithTax += WinMoneyNoWithTax8 * Count;
                        }

                    }
                }

                if (Description1 > 0)
                {
                    if (Description != "")
                        Description += "，";
                    Description = "追加一等奖" + Description1.ToString() + "注";
                }
                if (Description2 > 0)
                {
                    if (Description != "")
                        Description += "，";
                    Description += "追加二等奖" + Description2.ToString() + "注";
                }
                if (Description3 > 0)
                {
                    if (Description != "")
                        Description += "，";
                    Description += "追加三等奖" + Description3.ToString() + "注";
                }
                if (Description4 > 0)
                {
                    if (Description != "")
                        Description += "，";
                    Description += "追加四等奖" + Description4.ToString() + "注";
                }
                if (Description5 > 0)
                {
                    if (Description != "")
                        Description += "，";
                    Description += "追加五等奖" + Description5.ToString() + "注";
                }
                if (Description6 > 0)
                {
                    if (Description != "")
                        Description += "，";
                    Description += "追加六等奖" + Description6.ToString() + "注";
                }
                if (Description7 > 0)
                {
                    if (Description != "")
                        Description += "，";
                    Description += "追加七等奖" + Description7.ToString() + "注";
                }
                if (Description8 > 0)
                {
                    if (Description != "")
                        Description += "，";
                    Description += "八等奖" + Description8.ToString() + "注";
                }

                if (Description != "")
                    Description += "。";

                return WinMoney;
            }
            private double ComputeWin_12X2(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney1, double WinMoneyNoWithTax1)
            {
                WinNumber = WinNumber.Trim();
                if (WinNumber.Length < 22)	//22: "01 02 03 04 05 + 01 02" 
                    return -1;
                string[] Lotterys = SplitLotteryNumber(Number);
                if (Lotterys == null)
                    return -2;
                if (Lotterys.Length < 1)
                    return -2;

                string WinNumber_Blue = WinNumber.Substring(17, 5).Trim();

                int Description1 = 0;
                double WinMoney = 0;
                WinMoneyNoWithTax = 0;
                Description = "";

                for (int ii = 0; ii < Lotterys.Length; ii++)
                {
                    string t_str = "";
                    string[] Lottery = ToSingle_12X2(Lotterys[ii], ref t_str);
                    if (Lottery == null)
                        continue;
                    if (Lottery.Length < 1)
                        continue;

                    for (int i = 0; i < Lottery.Length; i++)
                    {
                        if (Lottery[i].Length < 2)
                            continue;

                        string[] Blue = new string[2];
                        Regex regex = new Regex(@"^(?<B0>\d\d\s)(?<B1>\d\d)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                        Match m = regex.Match(Lottery[i]);

                        int BlueRight = 0;
                        bool Full = true;

                        for (int k = 0; k < 2; k++)
                        {
                            Blue[k] = m.Groups["B" + k.ToString()].ToString().Trim();
                            if (Blue[k] == "")
                            {
                                Full = false;
                                break;
                            }
                            if (WinNumber_Blue.IndexOf(Blue[k]) >= 0)
                                BlueRight++;
                        }

                        if (!Full)
                            continue;

                        if ((BlueRight == 2))
                        {
                            Description1++;
                            WinMoney += WinMoney1;
                            WinMoneyNoWithTax += WinMoneyNoWithTax1;
                            continue;
                        }
                    }
                }

                if (Description1 > 0)
                    Description = "12选2" + Description1.ToString() + "注";

                return WinMoney;
            }
            private double ComputeWin_DT(int PlayType, string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney1, double WinMoneyNoWithTax1, double WinMoney2, double WinMoneyNoWithTax2, double WinMoney3, double WinMoneyNoWithTax3, double WinMoney4, double WinMoneyNoWithTax4, double WinMoney5, double WinMoneyNoWithTax5, double WinMoney6, double WinMoneyNoWithTax6, double WinMoney7, double WinMoneyNoWithTax7, double WinMoney8, double WinMoneyNoWithTax8)
            {
                WinNumber = WinNumber.Trim();
                if (WinNumber.Length < 22)	//22: "01 02 03 04 05 + 01 02" 
                    return -1;
                string[] Lotterys = SplitLotteryNumber(Number);
                if (Lotterys == null)
                    return -2;
                if (Lotterys.Length < 1)
                    return -2;

                string WinNumber_Red = WinNumber.Substring(0, 15);
                string WinNumber_Blue = WinNumber.Substring(17, 5).Trim();

                int Description1 = 0, Description2 = 0, Description3 = 0, Description4 = 0, Description5 = 0, Description6 = 0, Description7 = 0, Description8 = 0;
                double WinMoney = 0;
                WinMoneyNoWithTax = 0;
                Description = "";

                for (int ii = 0; ii < Lotterys.Length; ii++)
                {
                    string t_str = "";
                    string[] Lottery = ToSingle_DanT(Lotterys[ii], ref t_str);
                    if (Lottery == null)
                        continue;
                    if (Lottery.Length < 1)
                        continue;

                    for (int i = 0; i < Lottery.Length; i++)
                    {
                        if (Lottery[i].Length < 22)
                            continue;

                        string[] Red = new string[5];
                        string[] Blue = new string[2];

                        Regex regex = new Regex(@"^(?<R0>\d\d\s)(?<R1>\d\d\s)(?<R2>\d\d\s)(?<R3>\d\d\s)(?<R4>\d\d\s)[+]\s(?<B0>\d\d\s)(?<B1>\d\d)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                        Match m = regex.Match(Lottery[i]);

                        int RedRight = 0;
                        int BlueRight = 0;
                        bool Full = true;

                        for (int j = 0; j < 5; j++)
                        {
                            Red[j] = m.Groups["R" + j.ToString()].ToString().Trim();
                            if (Red[j] == "")
                            {
                                Full = false;
                                break;
                            }
                            if (WinNumber_Red.IndexOf(Red[j] + " ") >= 0)
                                RedRight++;
                        }

                        for (int k = 0; k < 2; k++)
                        {
                            Blue[k] = m.Groups["B" + k.ToString()].ToString().Trim();
                            if (Blue[k] == "")
                            {
                                Full = false;
                                break;
                            }
                            if (WinNumber_Blue.IndexOf(Blue[k]) >= 0)
                                BlueRight++;
                        }

                        if (!Full)
                            continue;

                        if ((RedRight == 5) && (BlueRight == 2))
                        {
                            Description1++;
                            WinMoney += WinMoney1;
                            WinMoneyNoWithTax += WinMoneyNoWithTax1;
                            continue;
                        }
                        if ((RedRight == 5) && (BlueRight == 1))
                        {
                            Description2++;
                            WinMoney += WinMoney2;
                            WinMoneyNoWithTax += WinMoneyNoWithTax2;
                            continue;
                        }
                        if ((RedRight == 5))
                        {
                            Description3++;
                            WinMoney += WinMoney3;
                            WinMoneyNoWithTax += WinMoneyNoWithTax3;
                            continue;
                        }
                        if ((RedRight == 4) && (BlueRight == 2))
                        {
                            Description4++;
                            WinMoney += WinMoney4;
                            WinMoneyNoWithTax += WinMoneyNoWithTax4;
                            continue;
                        }
                        if ((RedRight == 4) && (BlueRight == 1))
                        {
                            Description5++;
                            WinMoney += WinMoney5;
                            WinMoneyNoWithTax += WinMoneyNoWithTax5;
                            continue;
                        }
                        if ((RedRight == 4) || ((RedRight == 3) && (BlueRight == 2)))
                        {
                            Description6++;
                            WinMoney += WinMoney6;
                            WinMoneyNoWithTax += WinMoneyNoWithTax6;
                            continue;
                        }
                        if (((RedRight == 3) && (BlueRight == 1)) || ((RedRight == 2) && (BlueRight == 2)))
                        {
                            Description7++;
                            WinMoney += WinMoney7;
                            WinMoneyNoWithTax += WinMoneyNoWithTax7;
                            continue;
                        }
                        if ((RedRight == 3) || ((RedRight == 2) && (BlueRight == 1)) || ((RedRight == 1) && (BlueRight == 2)) || (BlueRight == 2))
                        {
                            Description8++;
                            WinMoney += WinMoney8;
                            WinMoneyNoWithTax += WinMoneyNoWithTax8;
                            continue;
                        }
                    }
                }

                if (Description1 > 0)
                {
                    if (Description != "")
                        Description += "，";
                    Description = "一等奖" + Description1.ToString() + "注";
                }
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
                if (Description8 > 0)
                {
                    if (Description != "")
                        Description += "，";
                    Description += "八等奖" + Description8.ToString() + "注";
                }

                if (Description != "")
                    Description += "。";

                return WinMoney;
            }
            private double ComputeWin_ZJ_DT(int PlayType, string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney1, double WinMoneyNoWithTax1, double ZJ_WinMoney1, double ZJ_WinMoneyNoWithTax1, double WinMoney2, double WinMoneyNoWithTax2, double ZJ_WinMoney2, double ZJ_WinMoneyNoWithTax2, double WinMoney3, double WinMoneyNoWithTax3, double ZJ_WinMoney3, double ZJ_WinMoneyNoWithTax3, double WinMoney4, double WinMoneyNoWithTax4, double ZJ_WinMoney4, double ZJ_WinMoneyNoWithTax4, double WinMoney5, double WinMoneyNoWithTax5, double ZJ_WinMoney5, double ZJ_WinMoneyNoWithTax5, double WinMoney6, double WinMoneyNoWithTax6, double ZJ_WinMoney6, double ZJ_WinMoneyNoWithTax6, double WinMoney7, double WinMoneyNoWithTax7, double ZJ_WinMoney7, double ZJ_WinMoneyNoWithTax7, double WinMoney8, double WinMoneyNoWithTax8)
            {
                WinNumber = WinNumber.Trim();
                if (WinNumber.Length < 22)	//22: "01 02 03 04 05 + 01 02" 
                    return -1;
                string[] Lotterys = SplitLotteryNumber(Number);
                if (Lotterys == null)
                    return -2;
                if (Lotterys.Length < 1)
                    return -2;

                string WinNumber_Red = WinNumber.Substring(0, 15);
                string WinNumber_Blue = WinNumber.Substring(17, 5).Trim();

                int Description1 = 0, Description2 = 0, Description3 = 0, Description4 = 0, Description5 = 0, Description6 = 0, Description7 = 0, Description8 = 0;
                double WinMoney = 0;
                WinMoneyNoWithTax = 0;
                Description = "";

                for (int ii = 0; ii < Lotterys.Length; ii++)
                {
                    string t_str = "";
                    string[] Lottery = ToSingle_DanT(Lotterys[ii], ref t_str);
                    if (Lottery == null)
                        continue;
                    if (Lottery.Length < 1)
                        continue;

                    for (int i = 0; i < Lottery.Length; i++)
                    {
                        if (Lottery[i].Length < 22)
                            continue;

                        string[] Red = new string[5];
                        string[] Blue = new string[2];

                        Regex regex = new Regex(@"^(?<R0>\d\d\s)(?<R1>\d\d\s)(?<R2>\d\d\s)(?<R3>\d\d\s)(?<R4>\d\d\s)[+]\s(?<B0>\d\d\s)(?<B1>\d\d)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                        Match m = regex.Match(Lottery[i]);

                        int RedRight = 0;
                        int BlueRight = 0;
                        bool Full = true;

                        for (int j = 0; j < 5; j++)
                        {
                            Red[j] = m.Groups["R" + j.ToString()].ToString().Trim();
                            if (Red[j] == "")
                            {
                                Full = false;
                                break;
                            }
                            if (WinNumber_Red.IndexOf(Red[j] + " ") >= 0)
                                RedRight++;
                        }

                        for (int k = 0; k < 2; k++)
                        {
                            Blue[k] = m.Groups["B" + k.ToString()].ToString().Trim();
                            if (Blue[k] == "")
                            {
                                Full = false;
                                break;
                            }
                            if (WinNumber_Blue.IndexOf(Blue[k]) >= 0)
                                BlueRight++;
                        }

                        if (!Full)
                            continue;

                        if ((RedRight == 5) && (BlueRight == 2))
                        {
                            Description1++;
                            WinMoney += WinMoney1 + ZJ_WinMoney1;
                            WinMoneyNoWithTax += WinMoneyNoWithTax1 + ZJ_WinMoneyNoWithTax1;
                            continue;
                        }
                        if ((RedRight == 5) && (BlueRight == 1))
                        {
                            Description2++;
                            WinMoney += WinMoney2 + ZJ_WinMoney2;
                            WinMoneyNoWithTax += WinMoneyNoWithTax2 + ZJ_WinMoneyNoWithTax2;
                            continue;
                        }
                        if ((RedRight == 5))
                        {
                            Description3++;
                            WinMoney += WinMoney3 + ZJ_WinMoney3;
                            WinMoneyNoWithTax += WinMoneyNoWithTax3 + ZJ_WinMoneyNoWithTax3;
                            continue;
                        }
                        if ((RedRight == 4) && (BlueRight == 2))
                        {
                            Description4++;
                            WinMoney += WinMoney4 + ZJ_WinMoney4;
                            WinMoneyNoWithTax += WinMoneyNoWithTax4 + ZJ_WinMoneyNoWithTax4;
                            continue;
                        }
                        if ((RedRight == 4) && (BlueRight == 1))
                        {
                            Description5++;
                            WinMoney += WinMoney5 + ZJ_WinMoney5;
                            WinMoneyNoWithTax += WinMoneyNoWithTax5 + ZJ_WinMoneyNoWithTax5;
                            continue;
                        }
                        if ((RedRight == 4) || ((RedRight == 3) && (BlueRight == 2)))
                        {
                            Description6++;
                            WinMoney += WinMoney6 + ZJ_WinMoney6;
                            WinMoneyNoWithTax += WinMoneyNoWithTax6 + ZJ_WinMoneyNoWithTax6;
                            continue;
                        }
                        if (((RedRight == 3) && (BlueRight == 1)) || ((RedRight == 2) && (BlueRight == 2)))
                        {
                            Description7++;
                            WinMoney += WinMoney7 + ZJ_WinMoney7;
                            WinMoneyNoWithTax += WinMoneyNoWithTax7 + ZJ_WinMoneyNoWithTax7;
                            continue;
                        }
                        if ((RedRight == 3) || ((RedRight == 2) && (BlueRight == 1)) || ((RedRight == 1) && (BlueRight == 2)) || (BlueRight == 2))
                        {
                            Description8++;
                            WinMoney += WinMoney8;
                            WinMoneyNoWithTax += WinMoneyNoWithTax8;
                            continue;
                        }
                    }
                }

                if (Description1 > 0)
                {
                    if (Description != "")
                        Description += "，";
                    Description = "追加一等奖" + Description1.ToString() + "注";
                }
                if (Description2 > 0)
                {
                    if (Description != "")
                        Description += "，";
                    Description += "追加二等奖" + Description2.ToString() + "注";
                }
                if (Description3 > 0)
                {
                    if (Description != "")
                        Description += "，";
                    Description += "追加三等奖" + Description3.ToString() + "注";
                }
                if (Description4 > 0)
                {
                    if (Description != "")
                        Description += "，";
                    Description += "追加四等奖" + Description4.ToString() + "注";
                }
                if (Description5 > 0)
                {
                    if (Description != "")
                        Description += "，";
                    Description += "追加五等奖" + Description5.ToString() + "注";
                }
                if (Description6 > 0)
                {
                    if (Description != "")
                        Description += "，";
                    Description += "追加六等奖" + Description6.ToString() + "注";
                }
                if (Description7 > 0)
                {
                    if (Description != "")
                        Description += "，";
                    Description += "追加七等奖" + Description7.ToString() + "注";
                }
                if (Description8 > 0)
                {
                    if (Description != "")
                        Description += "，";
                    Description += "八等奖" + Description8.ToString() + "注";
                }

                if (Description != "")
                    Description += "。";

                return WinMoney;
            }
            #endregion

            public override string AnalyseScheme(string Content, int PlayType)
            {

                if ((PlayType == PlayType_D) || (PlayType == PlayType_F))
                    return AnalyseScheme_0(Content, PlayType);

                if ((PlayType == PlayType_ZJ_D) || (PlayType == PlayType_ZJ_F))
                    return AnalyseScheme_ZJ(Content, PlayType);

                if ((PlayType == PlayType_2_D) || (PlayType == PlayType_2_F))
                    return AnalyseScheme_12X2(Content, PlayType);

                if (PlayType == PlayType_DT)
                    return AnalyseScheme_DT(Content, PlayType);

                if (PlayType == PlayType_ZJ_DT)
                    return AnalyseScheme_ZJ_DT(Content, PlayType);

                return null;
            }
            #region AnalyseScheme 的具体方法
            private string AnalyseScheme_0(string Content, int PlayType)
            {
                string[] strs = Content.Split('\n');
                if (strs == null)
                    return "";
                if (strs.Length == 0)
                    return "";

                string Result = "";

                string RegexString;
                if (PlayType == PlayType_D)
                    RegexString = @"^(\d\d\s){5}[+](\s\d\d){2}";
                else
                    RegexString = @"^(\d\d\s){5,35}[+](\s\d\d){2,12}";
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

                    string[] Red = FilterRepeated(m.Value.Trim().Split('+')[0].Trim().Split(' '), 35);
                    string[] Blue = FilterRepeated(m.Value.Trim().Split('+')[1].Trim().Split(' '), 12);

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

                    Result += RedNumber + "+ " + BlueNumber.Trim() + "|" + (GetNum(5, Red.Length) * GetNum(2,Blue.Length)).ToString() + "\n";
                }

                if (Result.EndsWith("\n"))
                    Result = Result.Substring(0, Result.Length - 1);
                return Result;
            }
            private string AnalyseScheme_ZJ(string Content, int PlayType)
            {
                string[] strs = Content.Split('\n');
                if (strs == null)
                    return "";
                if (strs.Length == 0)
                    return "";

                string Result = "";

                string RegexString;
                if (PlayType == PlayType_ZJ_D)
                    RegexString = @"^(\d\d\s){5}[+](\s\d\d){2}";
                else
                    RegexString = @"^(\d\d\s){5,35}[+](\s\d\d){2,12}";
                Regex regex = new Regex(RegexString, RegexOptions.Compiled | RegexOptions.IgnoreCase);

                for (int i = 0; i < strs.Length; i++)
                {
                    Match m = regex.Match(strs[i]);

                    if (!m.Success)
                    {
                        continue;
                    }

                    if (PlayType == PlayType_ZJ_D)
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

                    string[] Red = FilterRepeated(m.Value.Trim().Split('+')[0].Trim().Split(' '), 35);
                    string[] Blue = FilterRepeated(m.Value.Trim().Split('+')[1].Trim().Split(' '), 12);

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

                    Result += RedNumber + "+ " + BlueNumber.Trim() + "|" + (GetNum(5, Red.Length) * GetNum(2, Blue.Length)).ToString() + "\n";
                }

                if (Result.EndsWith("\n"))
                    Result = Result.Substring(0, Result.Length - 1);
                return Result;
            }
            private string AnalyseScheme_12X2(string Content, int PlayType)
            {
                string[] strs = Content.Split('\n');
                if (strs == null)
                    return "";
                if (strs.Length == 0)
                    return "";

                string Result = "";

                string RegexString;
                if (PlayType == PlayType_2_D)
                    RegexString = @"^\d\d\s\d\d";
                else
                    RegexString = @"^(\d\d\s){2,11}\d\d";
                Regex regex = new Regex(RegexString, RegexOptions.Compiled | RegexOptions.IgnoreCase);

                for (int i = 0; i < strs.Length; i++)
                {
                    Match m = regex.Match(strs[i]);
                    if (m.Success)
                    {
                        string CanonicalNumber = "";
                        string[] singles = ToSingle_12X2(m.Value, ref CanonicalNumber);
                        if (singles == null)
                            continue;
                        if ((singles.Length >= ((PlayType == PlayType_2_D) ? 1 : 2)))
                            Result += CanonicalNumber + "|" + singles.Length.ToString() + "\n";
                    }
                }

                if (Result.EndsWith("\n"))
                    Result = Result.Substring(0, Result.Length - 1);
                return Result;
            }
            private string AnalyseScheme_DT(string Content, int PlayType)
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

                    string[] Red_D = FilterRepeated(m.Value.Split('+')[0].Trim().Split(',')[0].Trim().Split(' '), 35);
                    string[] Red_T = FilterRepeated(m.Value.Split('+')[0].Trim().Split(',')[1].Trim().Split(' '), 35);
                    string[] Blue = FilterRepeated(m.Value.Trim().Split('+')[1].Trim().Split(' '), 16);

                    if (Red_D.Length + Red_T.Length + Blue.Length < 8)
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

                    Result += Red_D_Number + ", " + Red_T_Number + "+ " + Blue_Number + "|" + (GetNum(5 - Red_D.Length, Red_T.Length) * GetNum(2, Blue.Length)).ToString() + "\n";
                }

                if (Result.EndsWith("\n"))
                    Result = Result.Substring(0, Result.Length - 1);
                return Result;
            }
            private string AnalyseScheme_ZJ_DT(string Content, int PlayType)
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

                    string[] Red_D = FilterRepeated(m.Value.Split('+')[0].Trim().Split(',')[0].Trim().Split(' '), 35);
                    string[] Red_T = FilterRepeated(m.Value.Split('+')[0].Trim().Split(',')[1].Trim().Split(' '), 35);
                    string[] Blue = FilterRepeated(m.Value.Trim().Split('+')[1].Trim().Split(' '), 16);

                    if (Red_D.Length + Red_T.Length + Blue.Length < 8)
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

                    Result += Red_D_Number + ", " + Red_T_Number + "+ " + Blue_Number + "|" + (GetNum(5 - Red_D.Length, Red_T.Length) * GetNum(2, Blue.Length)).ToString() + "\n";
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
                Regex regex = new Regex(@"^(\d\d\s){5}[+](\s\d\d){2}", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                if (!regex.IsMatch(Number))
                    return false;

                string t_str = "";
                string[] WinLotteryNumber = ToSingle(Number, ref t_str, PlayType_D);

                if ((WinLotteryNumber == null) || (WinLotteryNumber.Length != 1))
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
                    case "CR_YTCII2":
                        if ((PlayTypeID == PlayType_D) || (PlayTypeID == PlayType_ZJ_D))
                        {
                            return GetPrintKeyList_CR_YTCII2_D(Numbers);
                        }
                        if ((PlayTypeID == PlayType_F) || (PlayTypeID == PlayType_ZJ_F))
                        {
                            return GetPrintKeyList_CR_YTCII2_F(Numbers);
                        }
                        if ((PlayTypeID == PlayType_2_D) || (PlayTypeID == PlayType_2_F))
                        {
                            return GetPrintKeyList_CR_YTCII2_12X2(Numbers);
                        }

                        break;
                    case "TGAMPOS4000":
                        if (PlayTypeID == PlayType_D)
                        {
                            return GetPrintKeyList_TGAMPOS4000_D(Numbers);
                        }
                        if (PlayTypeID == PlayType_ZJ_D)
                        {
                            return GetPrintKeyList_TGAMPOS4000_ZJ_D(Numbers);
                        }
                        if (PlayTypeID == PlayType_F)
                        {
                            return GetPrintKeyList_TGAMPOS4000_F(Numbers);
                        }
                        if (PlayTypeID == PlayType_ZJ_F)
                        {
                            return GetPrintKeyList_TGAMPOS4000_ZJ_F(Numbers);
                        }
                        if ((PlayTypeID == PlayType_2_D) || (PlayTypeID == PlayType_2_F))
                        {
                            return GetPrintKeyList_TGAMPOS4000_12X2(Numbers);
                        }

                        break;
                    case "CP86":
                        if ((PlayTypeID == PlayType_D) || (PlayTypeID == PlayType_ZJ_D))
                        {
                            return GetPrintKeyList_CP86_D(Numbers);
                        }
                        if ((PlayTypeID == PlayType_F) || (PlayTypeID == PlayType_ZJ_F))
                        {
                            return GetPrintKeyList_CP86_F(Numbers);
                        }
                        if ((PlayTypeID == PlayType_2_D) || (PlayTypeID == PlayType_2_F))
                        {
                            return GetPrintKeyList_CP86_12X2(Numbers);
                        }

                        break;

                    case "MODEL_4000":
                        if (PlayTypeID == PlayType_D)
                        {
                            return GetPrintKeyList_MODEL_4000_D(Numbers);
                        }
                        if (PlayTypeID == PlayType_ZJ_D)
                        {
                            return GetPrintKeyList_MODEL_4000_ZJ_D(Numbers);
                        }
                        if (PlayTypeID == PlayType_F)
                        {
                            return GetPrintKeyList_MODEL_4000_F(Numbers);
                        }
                        if (PlayTypeID == PlayType_ZJ_F)
                        {
                            return GetPrintKeyList_MODEL_4000_ZJ_F(Numbers);
                        }
                        if ((PlayTypeID == PlayType_2_D) || (PlayTypeID == PlayType_2_F))
                        {
                            return GetPrintKeyList_MODEL_4000_12X2(Numbers);
                        }
                        break;
                    case "CORONISTPT":
                        if (PlayTypeID == PlayType_D)
                        {
                            return GetPrintKeyList_CORONISTPT_D(Numbers);
                        }
                        if (PlayTypeID == PlayType_ZJ_D)
                        {
                            return GetPrintKeyList_CORONISTPT_ZJ_D(Numbers);
                        }
                        if (PlayTypeID == PlayType_F)
                        {
                            return GetPrintKeyList_CORONISTPT_F(Numbers);
                        }
                        if (PlayTypeID == PlayType_ZJ_F)
                        {
                            return GetPrintKeyList_CORONISTPT_ZJ_F(Numbers);
                        }
                        if ((PlayTypeID == PlayType_2_D) || (PlayTypeID == PlayType_2_F))
                        {
                            return GetPrintKeyList_CORONISTPT_12X2(Numbers);
                        }
                        break;
                    case "RS6500":
                        if (PlayTypeID == PlayType_D)
                        {
                            return GetPrintKeyList_RS6500_D(Numbers);
                        }
                        if (PlayTypeID == PlayType_ZJ_D)
                        {
                            return GetPrintKeyList_RS6500_ZJ_D(Numbers);
                        }
                        if (PlayTypeID == PlayType_F)
                        {
                            return GetPrintKeyList_RS6500_F(Numbers);
                        }
                        if (PlayTypeID == PlayType_ZJ_F)
                        {
                            return GetPrintKeyList_RS6500_ZJ_F(Numbers);
                        }
                        break;
                    case "ks230":
                        if ((PlayTypeID == PlayType_D) || (PlayTypeID == PlayType_ZJ_D))
                        {
                            return GetPrintKeyList_ks230_D(Numbers);
                        }
                        if ((PlayTypeID == PlayType_F) || (PlayTypeID == PlayType_ZJ_F))
                        {
                            return GetPrintKeyList_ks230_F(Numbers);
                        }
                        if ((PlayTypeID == PlayType_2_D) || (PlayTypeID == PlayType_2_F))
                        {
                            return GetPrintKeyList_ks230_12X2(Numbers);
                        }
                        break;
                    case "LA-600A":
                        if ((PlayTypeID == PlayType_D) || (PlayTypeID == PlayType_ZJ_D))
                        {
                            return GetPrintKeyList_LA_600A_D(Numbers);
                        }
                        if ((PlayTypeID == PlayType_F) || (PlayTypeID == PlayType_ZJ_F))
                        {
                            return GetPrintKeyList_LA_600A_F(Numbers);
                        }
                        if ((PlayTypeID == PlayType_2_D) || (PlayTypeID == PlayType_2_F))
                        {
                            return GetPrintKeyList_LA_600A_12X2(Numbers);
                        }
                        break;
                    case "TSP700_II":
                        if ((PlayTypeID == PlayType_D) || (PlayTypeID == PlayType_ZJ_D))
                        {
                            return GetPrintKeyList_TSP700_II_D(Numbers);
                        }
                        if ((PlayTypeID == PlayType_F) || (PlayTypeID == PlayType_ZJ_F))
                        {
                            return GetPrintKeyList_TSP700_II_F(Numbers);
                        }
                        if ((PlayTypeID == PlayType_2_D) || (PlayTypeID == PlayType_2_F))
                        {
                            return GetPrintKeyList_TSP700_II_12X2(Numbers);
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
                        if (ch == '+')
                        {
                            continue;
                        }
                        else
                        {
                            KeyList += "[" + ch.ToString() + "]";
                        }

                    }
                }

                return KeyList;
            }
            private string GetPrintKeyList_CR_YTCII2_F(string[] Numbers)
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
            private string GetPrintKeyList_CR_YTCII2_12X2(string[] Numbers)
            {
                string KeyList = "";

                foreach (string Number in Numbers)
                {
                    foreach (char ch in Number.Replace(" ", "").Replace("\r", "").Replace("\n", ""))
                    {
                        {
                            KeyList += "[" + ch.ToString() + "]";
                        }

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
                        if (ch == '+')
                        {
                            continue;
                        }
                        else
                        {
                            KeyList += "[" + ch.ToString() + "]";
                        }

                    }
                }

                return KeyList;
            }
            private string GetPrintKeyList_TGAMPOS4000_F(string[] Numbers)
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

                    KeyList += "[↓][↓]";

                    foreach (char ch in Blue)
                    {
                        KeyList += "[" + ch.ToString() + "]";
                    }
                }

                return KeyList;
            }
            private string GetPrintKeyList_TGAMPOS4000_ZJ_D(string[] Numbers)
            {
                string KeyList = "";

                foreach (string Number in Numbers)
                {
                    foreach (char ch in Number.Replace(" ", "").Replace("\r", "").Replace("\n", ""))
                    {
                        if (ch == '+')
                        {
                            continue;
                        }
                        else
                        {
                            KeyList += "[" + ch.ToString() + "]";
                        }

                    }
                }

                KeyList += "[+]";

                return KeyList;
            }
            private string GetPrintKeyList_TGAMPOS4000_ZJ_F(string[] Numbers)
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

                    KeyList += "[↓][↓]";

                    foreach (char ch in Blue)
                    {
                        KeyList += "[" + ch.ToString() + "]";
                    }
                }

                KeyList += "[+]";

                return KeyList;
            }
            private string GetPrintKeyList_TGAMPOS4000_12X2(string[] Numbers)
            {
                string KeyList = "";

                foreach (string Number in Numbers)
                {
                    foreach (char ch in Number.Replace(" ", "").Replace("\r", "").Replace("\n", ""))
                    {
                        {
                            KeyList += "[" + ch.ToString() + "]";
                        }

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
                        if (ch == '+')
                        {
                            continue;
                        }
                        else
                        {
                            KeyList += "[" + ch.ToString() + "]";
                        }

                    }
                }

                return KeyList;
            }
            private string GetPrintKeyList_CP86_F(string[] Numbers)
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
            private string GetPrintKeyList_CP86_12X2(string[] Numbers)
            {
                string KeyList = "";

                foreach (string Number in Numbers)
                {
                    foreach (char ch in Number.Replace(" ", "").Replace("\r", "").Replace("\n", ""))
                    {
                        {
                            KeyList += "[" + ch.ToString() + "]";
                        }

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
                        if (ch == '+')
                        {
                            continue;
                        }
                        else
                        {
                            KeyList += "[" + ch.ToString() + "]";
                        }

                    }
                }

                return KeyList;
            }
            private string GetPrintKeyList_MODEL_4000_F(string[] Numbers)
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

                    KeyList += "[↓][↓]";

                    foreach (char ch in Blue)
                    {
                        KeyList += "[" + ch.ToString() + "]";
                    }
                }

                return KeyList;
            }
            private string GetPrintKeyList_MODEL_4000_ZJ_D(string[] Numbers)
            {
                string KeyList = "";

                foreach (string Number in Numbers)
                {
                    foreach (char ch in Number.Replace(" ", "").Replace("\r", "").Replace("\n", ""))
                    {
                        if (ch == '+')
                        {
                            continue;
                        }
                        else
                        {
                            KeyList += "[" + ch.ToString() + "]";
                        }

                    }
                }

                KeyList += "[+]";

                return KeyList;
            }
            private string GetPrintKeyList_MODEL_4000_ZJ_F(string[] Numbers)
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

                    KeyList += "[↓][↓]";

                    foreach (char ch in Blue)
                    {
                        KeyList += "[" + ch.ToString() + "]";
                    }
                }

                KeyList += "[+]";

                return KeyList;
            }
            private string GetPrintKeyList_MODEL_4000_12X2(string[] Numbers)
            {
                string KeyList = "";

                foreach (string Number in Numbers)
                {
                    foreach (char ch in Number.Replace(" ", "").Replace("\r", "").Replace("\n", ""))
                    {
                        {
                            KeyList += "[" + ch.ToString() + "]";
                        }

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
                        if (ch == '+')
                        {
                            continue;
                        }
                        else
                        {
                            KeyList += "[" + ch.ToString() + "]";
                        }

                    }
                }

                return KeyList;
            }
            private string GetPrintKeyList_CORONISTPT_F(string[] Numbers)
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

                    KeyList += "[↓]";

                    foreach (char ch in Blue)
                    {
                        KeyList += "[" + ch.ToString() + "]";
                    }
                }

                return KeyList;
            }
            private string GetPrintKeyList_CORONISTPT_ZJ_D(string[] Numbers)
            {
                string KeyList = "";

                foreach (string Number in Numbers)
                {
                    foreach (char ch in Number.Replace(" ", "").Replace("\r", "").Replace("\n", ""))
                    {
                        if (ch == '+')
                        {
                            continue;
                        }
                        else
                        {
                            KeyList += "[" + ch.ToString() + "]";
                        }

                    }
                }

                KeyList = KeyList + "[A]";
                return KeyList;
            }
            private string GetPrintKeyList_CORONISTPT_ZJ_F(string[] Numbers)
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

                    KeyList += "[↓]";

                    foreach (char ch in Blue)
                    {
                        KeyList += "[" + ch.ToString() + "]";
                    }
                }

                KeyList = KeyList + "[A]";

                return KeyList;
            }
            private string GetPrintKeyList_CORONISTPT_12X2(string[] Numbers)
            {
                string KeyList = "";

                foreach (string Number in Numbers)
                {
                    foreach (char ch in Number.Replace(" ", "").Replace("\r", "").Replace("\n", ""))
                    {
                        {
                            KeyList += "[" + ch.ToString() + "]";
                        }

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
                        if (ch == '+')
                        {
                            continue;
                        }
                        else
                        {
                            KeyList += "[" + ch.ToString() + "]";
                        }

                    }
                }

                return KeyList;
            }
            private string GetPrintKeyList_RS6500_F(string[] Numbers)
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

                    KeyList += "[↓]";

                    foreach (char ch in Blue)
                    {
                        KeyList += "[" + ch.ToString() + "]";
                    }
                }

                return KeyList;
            }
            private string GetPrintKeyList_RS6500_ZJ_D(string[] Numbers)
            {
                string KeyList = "";

                foreach (string Number in Numbers)
                {
                    foreach (char ch in Number.Replace(" ", "").Replace("\r", "").Replace("\n", ""))
                    {
                        if (ch == '+')
                        {
                            continue;
                        }
                        else
                        {
                            KeyList += "[" + ch.ToString() + "]";
                        }

                    }
                }

                KeyList = KeyList + "[A]";
                return KeyList;
            }
            private string GetPrintKeyList_RS6500_ZJ_F(string[] Numbers)
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

                    KeyList += "[↓]";

                    foreach (char ch in Blue)
                    {
                        KeyList += "[" + ch.ToString() + "]";
                    }
                }

                KeyList = KeyList + "[A]";

                return KeyList;
            }

            private string GetPrintKeyList_ks230_D(string[] Numbers)
            {
                string KeyList = "";

                foreach (string Number in Numbers)
                {
                    foreach (char ch in Number.Replace(" ", "").Replace("\r", "").Replace("\n", ""))
                    {
                        if (ch == '+')
                        {
                            continue;
                        }
                        else
                        {
                            KeyList += "[" + ch.ToString() + "]";
                        }

                    }
                }

                return KeyList;
            }
            private string GetPrintKeyList_ks230_F(string[] Numbers)
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
            private string GetPrintKeyList_ks230_12X2(string[] Numbers)
            {
                string KeyList = "";

                foreach (string Number in Numbers)
                {
                    foreach (char ch in Number.Replace(" ", "").Replace("\r", "").Replace("\n", ""))
                    {
                        {
                            KeyList += "[" + ch.ToString() + "]";
                        }

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
                        if (ch == '+')
                        {
                            continue;
                        }
                        else
                        {
                            KeyList += "[" + ch.ToString() + "]";
                        }

                    }
                }

                return KeyList;
            }
            private string GetPrintKeyList_LA_600A_F(string[] Numbers)
            {
                string KeyList = "";

                foreach (string Number in Numbers)
                {
                    string[] strs = Number.Split('+');

                    if (strs.Length != 2)
                    {
                        return "";
                    }

                    KeyList += "[↓]";

                    string Red = strs[0].Replace(" ", "");
                    string Blue = strs[1].Replace(" ", "").Replace("\r", "").Replace("\n", "");

                    foreach (char ch in Red)
                    {
                        KeyList += "[" + ch.ToString() + "]";
                    }

                    KeyList += "[↓][↓]";

                    foreach (char ch in Blue)
                    {
                        KeyList += "[" + ch.ToString() + "]";
                    }
                }

                return KeyList;
            }
            private string GetPrintKeyList_LA_600A_12X2(string[] Numbers)
            {
                string KeyList = "";

                foreach (string Number in Numbers)
                {
                    foreach (char ch in Number.Replace(" ", "").Replace("\r", "").Replace("\n", ""))
                    {
                        {
                            KeyList += "[" + ch.ToString() + "]";
                        }

                    }
                }

                return KeyList;
            }

            private string GetPrintKeyList_TSP700_II_D(string[] Numbers)
            {
                string KeyList = "";

                foreach (string Number in Numbers)
                {
                    foreach (char ch in Number.Replace(" ", "").Replace("\r", "").Replace("\n", ""))
                    {
                        if (ch == '+')
                        {
                            continue;
                        }
                        else
                        {
                            KeyList += "[" + ch.ToString() + "]";
                        }

                    }
                }

                return KeyList;
            }
            private string GetPrintKeyList_TSP700_II_F(string[] Numbers)
            {
                string KeyList = "";

                foreach (string Number in Numbers)
                {
                    string[] strs = Number.Split('+');

                    if (strs.Length != 2)
                    {
                        return "";
                    }

                    KeyList += "[↓]";

                    string Red = strs[0].Replace(" ", "");
                    string Blue = strs[1].Replace(" ", "").Replace("\r", "").Replace("\n", "");

                    foreach (char ch in Red)
                    {
                        KeyList += "[" + ch.ToString() + "]";
                    }

                    KeyList += "[↓][↓]";

                    foreach (char ch in Blue)
                    {
                        KeyList += "[" + ch.ToString() + "]";
                    }
                }

                return KeyList;
            }
            private string GetPrintKeyList_TSP700_II_12X2(string[] Numbers)
            {
                string KeyList = "";

                foreach (string Number in Numbers)
                {
                    foreach (char ch in Number.Replace(" ", "").Replace("\r", "").Replace("\n", ""))
                    {
                        {
                            KeyList += "[" + ch.ToString() + "]";
                        }

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
                if (PlayTypeID == PlayType_ZJ_D)
                {
                    return ToElectronicTicket_HPSD_ZJ_D(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                if (PlayTypeID == PlayType_ZJ_F)
                {
                    return ToElectronicTicket_HPSD_ZJ_F(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                if (PlayTypeID == PlayType_2_D)
                {
                    return ToElectronicTicket_HPSD_2_D(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                if (PlayTypeID == PlayType_2_F)
                {
                    return ToElectronicTicket_HPSD_2_F(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
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

                        if (strs[i].ToString().Split('|').Length < 2)
                        {
                            continue;
                        }

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
            private Ticket[] ToElectronicTicket_HPSD_ZJ_D(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
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
                                EachMoney += 3 * double.Parse(strs[i].ToString().Split('|')[1]);
                            }
                        }

                        Money += EachMoney * EachMultiple;

                        al.Add(new Ticket(103, ConvertFormatToElectronTicket_HPSD(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));
                    }
                }

                Ticket[] Tickets = new Ticket[al.Count];

                for (int i = 0; i < Tickets.Length; i++)
                {
                    Tickets[i] = (Ticket)al[i];
                }

                return Tickets;

            }
            private Ticket[] ToElectronicTicket_HPSD_ZJ_F(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
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

                        if (strs[i].ToString().Split('|').Length < 2)
                        {
                            continue;
                        }

                        Numbers = strs[i].ToString().Split('|')[0];
                        EachMoney += 3 * double.Parse(strs[i].ToString().Split('|')[1]);

                        Money += EachMoney * EachMultiple;

                        al.Add(new Ticket(104, ConvertFormatToElectronTicket_HPSD(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));
                    }
                }

                Ticket[] Tickets = new Ticket[al.Count];

                for (int i = 0; i < Tickets.Length; i++)
                {
                    Tickets[i] = (Ticket)al[i];
                }

                return Tickets;
            }
            private Ticket[] ToElectronicTicket_HPSD_2_D(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
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

                        al.Add(new Ticket(105, ConvertFormatToElectronTicket_HPSD(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));
                    }
                }

                Ticket[] Tickets = new Ticket[al.Count];

                for (int i = 0; i < Tickets.Length; i++)
                {
                    Tickets[i] = (Ticket)al[i];
                }

                return Tickets;

            }
            private Ticket[] ToElectronicTicket_HPSD_2_F(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                ArrayList al = new ArrayList();
                Money = 0;

                string CanonicalNumber = "";

                string[] t_Numbers  = AnalyseScheme(Number, PlayTypeID).Split('\n');

                for (int k = 0; k < t_Numbers.Length; k++)
                {
                    string[] strs = ToSingle(t_Numbers[k].Split('|')[0], ref CanonicalNumber, PlayTypeID);

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
                                    Numbers += strs[i + M].ToString() + "\n";
                                    EachMoney += 2;
                                }
                            }

                            Money += EachMoney * EachMultiple;

                            al.Add(new Ticket(105, ConvertFormatToElectronTicket_HPSD(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));
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

                if (PlayTypeID == PlayType_D || PlayTypeID == PlayType_F || PlayTypeID == PlayType_ZJ_D || PlayTypeID == PlayType_ZJ_F || PlayTypeID == PlayType_2_D || PlayTypeID == PlayType_2_F)
                {
                    Ticket = Number.Replace(" ", ",").Replace(",+,", "#");
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
                if (PlayTypeID == PlayType_ZJ_D)
                {
                    return ToElectronicTicket_ZCW_ZJ_D(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                if (PlayTypeID == PlayType_ZJ_F)
                {
                    return ToElectronicTicket_ZCW_ZJ_F(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                if (PlayTypeID == PlayType_2_D)
                {
                    return ToElectronicTicket_ZCW_2_D(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                if (PlayTypeID == PlayType_2_F)
                {
                    return ToElectronicTicket_ZCW_2_F(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                if (PlayTypeID == PlayType_DT)
                {
                    return ToElectronicTicket_ZCW_DT(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                if (PlayTypeID == PlayType_ZJ_DT)
                {
                    return ToElectronicTicket_ZCW_ZJ_DT(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
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

                        if (strs[i].ToString().Split('|').Length < 2)
                        {
                            continue;
                        }

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
            private Ticket[] ToElectronicTicket_ZCW_ZJ_D(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
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
                                EachMoney += 3 * double.Parse(strs[i].ToString().Split('|')[1]);
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
            private Ticket[] ToElectronicTicket_ZCW_ZJ_F(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
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

                        if (strs[i].ToString().Split('|').Length < 2)
                        {
                            continue;
                        }

                        Numbers = strs[i].ToString().Split('|')[0];
                        EachMoney += 3 * double.Parse(strs[i].ToString().Split('|')[1]);

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
            private Ticket[] ToElectronicTicket_ZCW_2_D(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
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
            private Ticket[] ToElectronicTicket_ZCW_2_F(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                ArrayList al = new ArrayList();
                Money = 0;

                string CanonicalNumber = "";

                string[] t_Numbers = AnalyseScheme(Number, PlayTypeID).Split('\n');

                for (int k = 0; k < t_Numbers.Length; k++)
                {
                    string[] strs = ToSingle(t_Numbers[k].Split('|')[0], ref CanonicalNumber, PlayTypeID);

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
                                    Numbers += strs[i + M].ToString() + "\n";
                                    EachMoney += 2;
                                }
                            }

                            Money += EachMoney * EachMultiple;

                            al.Add(new Ticket(1, ConvertFormatToElectronTicket_ZCW(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));
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

                        if (strs[i].ToString().Split('|').Length < 2)
                        {
                            continue;
                        }

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
            private Ticket[] ToElectronicTicket_ZCW_ZJ_DT(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
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

                        if (strs[i].ToString().Split('|').Length < 2)
                        {
                            continue;
                        }

                        Numbers = strs[i].ToString().Split('|')[0];
                        EachMoney += 3 * double.Parse(strs[i].ToString().Split('|')[1]);

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

                if (PlayTypeID == PlayType_DT || PlayTypeID == PlayType_ZJ_DT)
                {
                    Ticket = Number.Replace(" ", "").Replace("+", "|*").Replace(",", "*");
                }
                else
                {
                    Ticket = Number.Replace(" ", "").Replace("+", "|").Replace(",", "*");
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
                if (PlayTypeID == PlayType_ZJ_D)
                {
                    return ToElectronicTicket_ZZYTC_ZJ_D(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                if (PlayTypeID == PlayType_ZJ_F)
                {
                    return ToElectronicTicket_ZZYTC_ZJ_F(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                if (PlayTypeID == PlayType_2_D)
                {
                    return ToElectronicTicket_ZZYTC_2_D(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                if (PlayTypeID == PlayType_2_F)
                {
                    return ToElectronicTicket_ZZYTC_2_F(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
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

                        if (strs[i].ToString().Split('|').Length < 2)
                        {
                            continue;
                        }

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
            private Ticket[] ToElectronicTicket_ZZYTC_ZJ_D(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
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
                                EachMoney += 3 * double.Parse(strs[i].ToString().Split('|')[1]);
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
            private Ticket[] ToElectronicTicket_ZZYTC_ZJ_F(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
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

                        if (strs[i].ToString().Split('|').Length < 2)
                        {
                            continue;
                        }

                        Numbers = strs[i].ToString().Split('|')[0];
                        EachMoney += 3 * double.Parse(strs[i].ToString().Split('|')[1]);

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
            private Ticket[] ToElectronicTicket_ZZYTC_2_D(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
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
            private Ticket[] ToElectronicTicket_ZZYTC_2_F(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
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

                        if (strs[i].ToString().Split('|').Length < 2)
                        {
                            continue;
                        }

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

            private string ConvertFormatToElectronTicket_ZZYTC(int PlayTypeID, string Number)
            {
                Number = Number.Trim();

                string Ticket = "";

                if (PlayTypeID == PlayType_D || PlayTypeID == PlayType_F || PlayTypeID == PlayType_ZJ_D || PlayTypeID == PlayType_ZJ_F || PlayTypeID == PlayType_2_D || PlayTypeID == PlayType_2_F)
                {
                    Ticket = Number.Replace(" ", "*").Replace("*+*", "|");
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
                if (PlayTypeID == PlayType_ZJ_D)
                {
                    return ToElectronicTicket_DYJ_ZJ_D(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                if (PlayTypeID == PlayType_ZJ_F)
                {
                    return ToElectronicTicket_DYJ_ZJ_F(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                if (PlayTypeID == PlayType_2_D)
                {
                    return ToElectronicTicket_DYJ_2_D(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                if (PlayTypeID == PlayType_2_F)
                {
                    return ToElectronicTicket_DYJ_2_F(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
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

                        if (strs[i].ToString().Split('|').Length < 2)
                        {
                            continue;
                        }

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
            private Ticket[] ToElectronicTicket_DYJ_ZJ_D(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
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
                                EachMoney += 3 * double.Parse(strs[i].ToString().Split('|')[1]);
                            }
                        }

                        Money += EachMoney * EachMultiple;

                        al.Add(new Ticket(103, ConvertFormatToElectronTicket_DYJ(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));
                    }
                }

                Ticket[] Tickets = new Ticket[al.Count];

                for (int i = 0; i < Tickets.Length; i++)
                {
                    Tickets[i] = (Ticket)al[i];
                }

                return Tickets;

            }
            private Ticket[] ToElectronicTicket_DYJ_ZJ_F(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
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

                        if (strs[i].ToString().Split('|').Length < 2)
                        {
                            continue;
                        }

                        Numbers = strs[i].ToString().Split('|')[0] + ";";
                        EachMoney += 3 * double.Parse(strs[i].ToString().Split('|')[1]);

                        Money += EachMoney * EachMultiple;

                        al.Add(new Ticket(104, ConvertFormatToElectronTicket_DYJ(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));
                    }
                }

                Ticket[] Tickets = new Ticket[al.Count];

                for (int i = 0; i < Tickets.Length; i++)
                {
                    Tickets[i] = (Ticket)al[i];
                }

                return Tickets;
            }
            private Ticket[] ToElectronicTicket_DYJ_2_D(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
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

                        al.Add(new Ticket(105, ConvertFormatToElectronTicket_DYJ(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));
                    }
                }

                Ticket[] Tickets = new Ticket[al.Count];

                for (int i = 0; i < Tickets.Length; i++)
                {
                    Tickets[i] = (Ticket)al[i];
                }

                return Tickets;

            }
            private Ticket[] ToElectronicTicket_DYJ_2_F(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
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

                        if (strs[i].ToString().Split('|').Length < 2)
                        {
                            continue;
                        }

                        Numbers = strs[i].ToString().Split('|')[0] + ";";
                        EachMoney += 2 * double.Parse(strs[i].ToString().Split('|')[1]);

                        Money += EachMoney * EachMultiple;

                        al.Add(new Ticket(105, ConvertFormatToElectronTicket_DYJ(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));
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

                if (PlayTypeID == PlayType_D || PlayTypeID == PlayType_F || PlayTypeID == PlayType_ZJ_D || PlayTypeID == PlayType_ZJ_F || PlayTypeID == PlayType_2_D || PlayTypeID == PlayType_2_F)
                {
                    Ticket = Number.Replace(" + ", "-");
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
                if (PlayTypeID == PlayType_ZJ_D)
                {
                    return ToElectronicTicket_CTTCSD_ZJ_D(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                if (PlayTypeID == PlayType_ZJ_F)
                {
                    return ToElectronicTicket_CTTCSD_ZJ_F(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                if (PlayTypeID == PlayType_2_D)
                {
                    return ToElectronicTicket_CTTCSD_2_D(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                if (PlayTypeID == PlayType_2_F)
                {
                    return ToElectronicTicket_CTTCSD_2_F(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
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

                        if (strs[i].ToString().Split('|').Length < 2)
                        {
                            continue;
                        }

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
            private Ticket[] ToElectronicTicket_CTTCSD_ZJ_D(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
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
                                EachMoney += 3 * double.Parse(strs[i].ToString().Split('|')[1]);
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
            private Ticket[] ToElectronicTicket_CTTCSD_ZJ_F(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
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

                        if (strs[i].ToString().Split('|').Length < 2)
                        {
                            continue;
                        }

                        Numbers = "*" + strs[i].ToString().Split('|')[0];
                        EachMoney += 3 * double.Parse(strs[i].ToString().Split('|')[1]);

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
            private Ticket[] ToElectronicTicket_CTTCSD_2_D(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
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
            private Ticket[] ToElectronicTicket_CTTCSD_2_F(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
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

                        if (strs[i].ToString().Split('|').Length < 2)
                        {
                            continue;
                        }

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

            private string ConvertFormatToElectronTicket_CTTCSD(int PlayTypeID, string Number)
            {
                Number = Number.Trim();

                string Ticket = "";

                if (PlayTypeID == PlayType_D || PlayTypeID == PlayType_ZJ_D || PlayTypeID == PlayType_2_D)
                {
                    Ticket = Number.Replace(" ", "").Replace("+", "|");
                }

                if (PlayTypeID == PlayType_F || PlayTypeID == PlayType_ZJ_F ||  PlayTypeID == PlayType_2_F)
                {
                    Ticket = Number.Replace(" ", "").Replace("+", "|*");
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
                if (PlayTypeID == PlayType_ZJ_D)
                {
                    return ToElectronicTicket_BJCP_ZJ_D(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                if (PlayTypeID == PlayType_ZJ_F)
                {
                    return ToElectronicTicket_BJCP_ZJ_F(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                if (PlayTypeID == PlayType_2_D)
                {
                    return ToElectronicTicket_BJCP_2_D(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                if (PlayTypeID == PlayType_2_F)
                {
                    return ToElectronicTicket_BJCP_2_F(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                if (PlayTypeID == PlayType_DT)
                {
                    return ToElectronicTicket_BJCP_DT(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                if (PlayTypeID == PlayType_ZJ_DT)
                {
                    return ToElectronicTicket_BJCP_ZJ_DT(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
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

                        if (strs[i].ToString().Split('|').Length < 2)
                        {
                            continue;
                        }

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
            private Ticket[] ToElectronicTicket_BJCP_ZJ_D(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
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
                                EachMoney += 3 * double.Parse(strs[i].ToString().Split('|')[1]);
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
            private Ticket[] ToElectronicTicket_BJCP_ZJ_F(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
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

                        if (strs[i].ToString().Split('|').Length < 2)
                        {
                            continue;
                        }

                        Numbers = strs[i].ToString().Split('|')[0];
                        EachMoney += 3 * double.Parse(strs[i].ToString().Split('|')[1]);

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
            private Ticket[] ToElectronicTicket_BJCP_2_D(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
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
            private Ticket[] ToElectronicTicket_BJCP_2_F(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                ArrayList al = new ArrayList();
                Money = 0;

                string CanonicalNumber = "";

                string[] t_Numbers = AnalyseScheme(Number, PlayTypeID).Split('\n');

                for (int k = 0; k < t_Numbers.Length; k++)
                {
                    string[] strs = ToSingle(t_Numbers[k].Split('|')[0], ref CanonicalNumber, PlayTypeID);

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
                                    Numbers += strs[i + M].ToString() + "\n";
                                    EachMoney += 2;
                                }
                            }

                            Money += EachMoney * EachMultiple;

                            al.Add(new Ticket(1, ConvertFormatToElectronTicket_BJCP(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));
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

                        if (strs[i].ToString().Split('|').Length < 2)
                        {
                            continue;
                        }

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
            private Ticket[] ToElectronicTicket_BJCP_ZJ_DT(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
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

                        if (strs[i].ToString().Split('|').Length < 2)
                        {
                            continue;
                        }

                        Numbers = strs[i].ToString().Split('|')[0];
                        EachMoney += 3 * double.Parse(strs[i].ToString().Split('|')[1]);

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

                if (PlayTypeID == PlayType_DT || PlayTypeID == PlayType_ZJ_DT)
                {
                    Ticket = Number.Replace(" ", "").Replace("+", "|*").Replace(",", "*");
                }
                else
                {
                    Ticket = Number.Replace(" ", "").Replace("+", "|").Replace(",", "*");
                }

                return Ticket;
            }
            #endregion
        }
    }
}