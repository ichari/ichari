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
        /// 十一运夺金
        /// </summary>
        public partial class HN11X5 : LotteryBase
        {
            #region 静态变量

            public const int PlayType_RX1 = 7701;
            public const int PlayType_RX2 = 7702;
            public const int PlayType_RX3 = 7703;
            public const int PlayType_RX4 = 7704;
            public const int PlayType_RX5 = 7705;
            public const int PlayType_RX6 = 7706;
            public const int PlayType_RX7 = 7707;
            public const int PlayType_RX8 = 7708;
            public const int PlayType_ZhiQ2 = 7709;
            public const int PlayType_ZhiQ3 = 7710;
            public const int PlayType_ZuQ2 = 7711;
            public const int PlayType_ZuQ3 = 7712;

            public const int ID = 77;
            public const string sID = "77";
            public const string Name = "11选5";
            public const string Code = "HN11X5";
            public const double MaxMoney = 200000;
            #endregion

            public HN11X5()
            {
                id = 77;
                name = "11选5";
                code = "HN11X5";
            }

            public override bool CheckPlayType(int play_type)
            {
                return ((play_type >= 7701) && (play_type <= 7712));
            }

            public override PlayType[] GetPlayTypeList()
            {
                PlayType[] Result = new PlayType[12];

                Result[0] = new PlayType(PlayType_RX1, "任选一");
                Result[1] = new PlayType(PlayType_RX2, "任选二");
                Result[2] = new PlayType(PlayType_RX3, "任选三");
                Result[3] = new PlayType(PlayType_RX4, "任选四");
                Result[4] = new PlayType(PlayType_RX5, "任选五");
                Result[5] = new PlayType(PlayType_RX6, "任选六");
                Result[6] = new PlayType(PlayType_RX7, "任选七");
                Result[7] = new PlayType(PlayType_RX8, "任选八");
                Result[8] = new PlayType(PlayType_ZhiQ2, "直选前二");
                Result[9] = new PlayType(PlayType_ZhiQ3, "直选前三");
                Result[10] = new PlayType(PlayType_ZuQ2, "组选前二");
                Result[11] = new PlayType(PlayType_ZuQ3, "组选前三");

                return Result;
            }

            public override string BuildNumber(int Num, int Type)
            {
                System.Random rd = new Random();
                StringBuilder sb = new StringBuilder();

                for (int i = 0; i < Num; i++)
                {
                    string LotteryNumber = "";

                    for (int j = 0; j < Type; j++)
                        LotteryNumber += rd.Next(1, 11).ToString();

                    sb.Append(LotteryNumber.Trim() + "\n");
                }

                string Result = sb.ToString();
                Result = Result.Substring(0, Result.Length - 1);

                return Result;
            }

            public override string[] ToSingle(string Number, ref string CanonicalNumber, int PlayType)   //复式取单式, 后面 ref 参数是将彩票规范化，
            {
                if (PlayType == PlayType_RX1)
                    return ToSingle_RX1(Number, ref CanonicalNumber);

                if (PlayType == PlayType_RX2)
                    return ToSingle_RX2(Number, ref CanonicalNumber);

                if (PlayType == PlayType_RX3)
                    return ToSingle_RX3(Number, ref CanonicalNumber);

                if (PlayType == PlayType_RX4)
                    return ToSingle_RX4(Number, ref CanonicalNumber);

                if (PlayType == PlayType_RX5)
                    return ToSingle_RX5(Number, ref CanonicalNumber);

                if (PlayType == PlayType_RX6)
                    return ToSingle_RX6(Number, ref CanonicalNumber);

                if (PlayType == PlayType_RX7)
                    return ToSingle_RX7(Number, ref CanonicalNumber);

                if (PlayType == PlayType_RX8)
                    return ToSingle_RX8(Number, ref CanonicalNumber);

                if (PlayType == PlayType_ZhiQ2)
                    return ToSingle_ZhiQ2(Number, ref CanonicalNumber);

                if (PlayType == PlayType_ZhiQ3)
                    return ToSingle_ZhiQ3(Number, ref CanonicalNumber);

                if (PlayType == PlayType_ZuQ2)
                    return ToSingle_ZuQ2(Number, ref CanonicalNumber);

                if (PlayType == PlayType_ZuQ3)
                    return ToSingle_ZuQ3(Number, ref CanonicalNumber);

                return null;
            }
            #region ToSingle 的具体方法
            private string[] ToSingle_RX1(string Number, ref string CanonicalNumber)
            {
                string[] strs = FilterRepeated(Number.Trim().Split(' '), 11);
                CanonicalNumber = "";

                if (strs.Length < 1)
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
                for (int i = 0; i < n; i++)
                    al.Add(strs[i]);

                #endregion

                string[] Result = new string[al.Count];
                for (int i = 0; i < al.Count; i++)
                    Result[i] = al[i].ToString();
                return Result;
            }
            private string[] ToSingle_RX2(string Number, ref string CanonicalNumber)
            {
                string[] strs = FilterRepeated(Number.Trim().Split(' '), 11);
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
            private string[] ToSingle_RX3(string Number, ref string CanonicalNumber)
            {
                string[] strs = FilterRepeated(Number.Trim().Split(' '), 11);
                CanonicalNumber = "";

                if (strs.Length < 3)
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
                for (int i = 0; i < n - 2; i++)
                    for (int j = i + 1; j < n - 1; j++)
                        for (int k = j + 1; k < n; k++)
                            al.Add(strs[i] + " " + strs[j] + " " + strs[k]);

                #endregion

                string[] Result = new string[al.Count];
                for (int i = 0; i < al.Count; i++)
                    Result[i] = al[i].ToString();
                return Result;
            }
            private string[] ToSingle_RX4(string Number, ref string CanonicalNumber)
            {
                string[] strs = FilterRepeated(Number.Trim().Split(' '), 11);
                CanonicalNumber = "";

                if (strs.Length < 4)
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
                for (int i = 0; i < n - 3; i++)
                    for (int j = i + 1; j < n - 2; j++)
                        for (int k = j + 1; k < n - 1; k++)
                            for (int x = k + 1; x < n; x++)
                                al.Add(strs[i] + " " + strs[j] + " " + strs[k] + " " + strs[x]);

                #endregion

                string[] Result = new string[al.Count];
                for (int i = 0; i < al.Count; i++)
                    Result[i] = al[i].ToString();
                return Result;
            }
            private string[] ToSingle_RX5(string Number, ref string CanonicalNumber)
            {
                string[] strs = FilterRepeated(Number.Trim().Split(' '), 11);
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
            private string[] ToSingle_RX6(string Number, ref string CanonicalNumber)
            {
                string[] strs = FilterRepeated(Number.Trim().Split(' '), 11);
                CanonicalNumber = "";

                if (strs.Length < 6)
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
                for (int i = 0; i < n - 5; i++)
                    for (int j = i + 1; j < n - 4; j++)
                        for (int k = j + 1; k < n - 3; k++)
                            for (int x = k + 1; x < n - 2; x++)
                                for (int y = x + 1; y < n - 1; y++)
                                    for (int z = y + 1; z < n; z++)
                                        al.Add(strs[i] + " " + strs[j] + " " + strs[k] + " " + strs[x] + " " + strs[y] + " " + strs[z]);

                #endregion

                string[] Result = new string[al.Count];
                for (int i = 0; i < al.Count; i++)
                    Result[i] = al[i].ToString();
                return Result;
            }
            private string[] ToSingle_RX7(string Number, ref string CanonicalNumber)
            {
                string[] strs = FilterRepeated(Number.Trim().Split(' '), 11);
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
            private string[] ToSingle_RX8(string Number, ref string CanonicalNumber)
            {
                string[] strs = FilterRepeated(Number.Trim().Split(' '), 11);
                CanonicalNumber = "";

                if (strs.Length < 8)
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
                for (int i = 0; i < n - 7; i++)
                    for (int j = i + 1; j < n - 6; j++)
                        for (int k = j + 1; k < n - 5; k++)
                            for (int x = k + 1; x < n - 4; x++)
                                for (int y = x + 1; y < n - 3; y++)
                                    for (int z = y + 1; z < n - 2; z++)
                                        for (int a = z + 1; a < n - 1; a++)
                                            for (int b = a + 1; b < n; b++)
                                                al.Add(strs[i] + " " + strs[j] + " " + strs[k] + " " + strs[x] + " " + strs[y] + " " + strs[z] + " " + strs[a] + " " + strs[b]);

                #endregion

                string[] Result = new string[al.Count];
                for (int i = 0; i < al.Count; i++)
                    Result[i] = al[i].ToString();
                return Result;
            }
            private string[] ToSingle_ZhiQ2(string Number, ref string CanonicalNumber)
            {
                string[] Q1 = FilterRepeated(Number.Trim().Split('|')[0].Trim().Split(' '), 11);
                string[] Q2 = FilterRepeated(Number.Trim().Split('|')[1].Trim().Split(' '), 11);

                if ((Q1.Length < 1) && (Q2.Length < 1))
                {
                    CanonicalNumber = "";
                    return null;
                }

                for (int i = 0; i < Q1.Length; i++)
                    CanonicalNumber += (Q1[i] + " ");

                CanonicalNumber = CanonicalNumber.Trim();

                CanonicalNumber += "|";
                for (int i = 0; i < Q2.Length; i++)
                    CanonicalNumber += (Q2[i] + " ");

                CanonicalNumber = CanonicalNumber.Trim();

                ArrayList al = new ArrayList();

                #region 循环取单式

                int m = Q1.Length;
                int n = Q2.Length;

                for (int i = 0; i < m; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        if (Q1[i] != Q2[j])
                        {
                            al.Add(Q1[i] + " " + Q2[j]);
                        }
                    }
                }
                #endregion

                string[] Result = new string[al.Count];
                for (int i = 0; i < al.Count; i++)
                    Result[i] = al[i].ToString();
                return Result;
            }
            private string[] ToSingle_ZhiQ3(string Number, ref string CanonicalNumber)
            {
                string[] Q1 = FilterRepeated(Number.Trim().Split('|')[0].Trim().Split(' '), 11);
                string[] Q2 = FilterRepeated(Number.Trim().Split('|')[1].Trim().Split(' '), 11);
                string[] Q3 = FilterRepeated(Number.Trim().Split('|')[2].Trim().Split(' '), 11);

                if ((Q1.Length < 1) && (Q2.Length < 1) && (Q3.Length < 1))
                {
                    CanonicalNumber = "";
                    return null;
                }

                for (int i = 0; i < Q1.Length; i++)
                    CanonicalNumber += (Q1[i] + " ");

                CanonicalNumber = CanonicalNumber.Trim();

                CanonicalNumber += "|";
                for (int i = 0; i < Q2.Length; i++)
                    CanonicalNumber += (Q2[i] + " ");

                CanonicalNumber = CanonicalNumber.Trim();

                CanonicalNumber += "|";
                for (int i = 0; i < Q3.Length; i++)
                    CanonicalNumber += (Q3[i] + " ");

                ArrayList al = new ArrayList();

                #region 循环取单式

                int m = Q1.Length;
                int n = Q2.Length;
                int x = Q3.Length;

                for (int i = 0; i < m; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        for (int z = 0; z < x; z++)
                        {
                            if (Q1[i] != Q2[j] && Q2[j] != Q3[z] && Q3[z] != Q1[i])
                            {
                                al.Add(Q1[i] + " " + Q2[j] + " " + Q3[z]);
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
            private string[] ToSingle_ZuQ2(string Number, ref string CanonicalNumber)
            {
                string[] strs = FilterRepeated(Number.Trim().Split(' '), 11);
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
            private string[] ToSingle_ZuQ3(string Number, ref string CanonicalNumber)
            {
                string[] strs = FilterRepeated(Number.Trim().Split(' '), 11);
                CanonicalNumber = "";

                if (strs.Length < 3)
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
                for (int i = 0; i < n - 2; i++)
                    for (int j = i + 1; j < n - 1; j++)
                        for (int k = j + 1; k < n; k++)
                            al.Add(strs[i] + " " + strs[j] + " " + strs[k]);

                #endregion

                string[] Result = new string[al.Count];
                for (int i = 0; i < al.Count; i++)
                    Result[i] = al[i].ToString();
                return Result;
            }
            #endregion

            public override double ComputeWin(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, int PlayType, params double[] WinMoneyList)
            {
                Description = "";
                WinMoneyNoWithTax = 0;

                if ((WinMoneyList == null) || (WinMoneyList.Length < 24)) //奖金参数排列顺序(1 2 3 4 5 6 7 8 任选奖，直选前2, 直选前3, 组选前2, 组选前3
                    return -3;

                int WinCountRX1 = 0, WinCountRX2 = 0, WinCountRX3 = 0, WinCountRX4 = 0, WinCountRX5 = 0, WinCountRX6 = 0, WinCountRX7 = 0, WinCountRX8 = 0;
                int WinCount_ZhiQ2 = 0, WinCount_ZhiQ3 = 0;
                int WinCount_ZuQ2 = 0, WinCount_ZuQ3 = 0;

                if (PlayType == PlayType_RX1)
                    return ComputeWin_RX1(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[0], WinMoneyList[1], ref WinCountRX1);

                if (PlayType == PlayType_RX2)
                    return ComputeWin_RX2(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[2], WinMoneyList[3], ref WinCountRX2);

                if (PlayType == PlayType_RX3)
                    return ComputeWin_RX3(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[4], WinMoneyList[5], ref WinCountRX3);

                if (PlayType == PlayType_RX4)
                    return ComputeWin_RX4(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[6], WinMoneyList[7], ref WinCountRX4);

                if (PlayType == PlayType_RX5)
                    return ComputeWin_RX5(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[8], WinMoneyList[9], ref WinCountRX5);

                if (PlayType == PlayType_RX6)
                    return ComputeWin_RX6(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[10], WinMoneyList[11], ref WinCountRX6);

                if (PlayType == PlayType_RX7)
                    return ComputeWin_RX7(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[12], WinMoneyList[13], ref WinCountRX7);

                if (PlayType == PlayType_RX8)
                    return ComputeWin_RX8(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[14], WinMoneyList[15], ref WinCountRX8);

                if (PlayType == PlayType_ZhiQ2)
                    return ComputeWin_ZhiQ2(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[16], WinMoneyList[17], ref WinCount_ZhiQ2);

                if (PlayType == PlayType_ZhiQ3)
                    return ComputeWin_ZhiQ3(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[18], WinMoneyList[19], ref WinCount_ZhiQ3);

                if (PlayType == PlayType_ZuQ2)
                    return ComputeWin_ZuQ2(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[20], WinMoneyList[21], ref WinCount_ZuQ2);

                if (PlayType == PlayType_ZuQ3)
                    return ComputeWin_ZuQ3(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[22], WinMoneyList[23], ref WinCount_ZuQ3);


                return -4;
            }
            #region ComputeWin  的具体方法
            private double ComputeWin_RX1(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney1, double WinMoneyNoWithTax1, ref int WinCountRX1)
            {
                WinNumber = WinNumber.Trim();
                if (WinNumber.Length < 14)	//14: "01 02 03 04 05"
                    return -1;
                string[] Lotterys = SplitLotteryNumber(Number);
                if (Lotterys == null)
                    return -2;
                if (Lotterys.Length < 1)
                    return -2;

                WinCountRX1 = 0;
                double WinMoney = 0;
                WinMoneyNoWithTax = 0;
                Description = "";

                for (int ii = 0; ii < Lotterys.Length; ii++)
                {
                    string t_str = "";
                    string[] Lottery = ToSingle_RX1(Lotterys[ii], ref t_str);
                    if (Lottery == null)
                        continue;
                    if (Lottery.Length < 1)
                        continue;

                    for (int i = 0; i < Lottery.Length; i++)
                    {
                        string[] Red = new string[1];
                        Regex regex = new Regex(@"^(?<R0>\d\d)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                        Match m = regex.Match(Lottery[i]);
                        int j;
                        int Count = 0;
                        bool Full = true;
                        for (j = 0; j < 1; j++)
                        {
                            Red[j] = m.Groups["R" + j.ToString()].ToString().Trim();
                            if (Red[j] == "")
                            {
                                Full = false;
                                break;
                            }
                            if (WinNumber.Substring(0, 2) == Red[j])
                                Count++;
                        }
                        if (!Full)
                            continue;

                        if (Count == 1)
                        {
                            WinCountRX1++;
                            WinMoney += WinMoney1;
                            WinMoneyNoWithTax += WinMoneyNoWithTax1;
                            continue;
                        }
                    }
                }

                if (WinCountRX1 > 0)
                {
                    Description = "任选一奖" + WinCountRX1.ToString() + "注";
                }

                if (Description != "")
                    Description += "。";

                return WinMoney;
            }
            private double ComputeWin_RX2(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney2, double WinMoneyNoWithTax2, ref int WinCountRX2)
            {
                WinNumber = WinNumber.Trim();
                if (WinNumber.Length < 14)	//14: "01 02 03 04 05"
                    return -1;
                string[] Lotterys = SplitLotteryNumber(Number);
                if (Lotterys == null)
                    return -2;
                if (Lotterys.Length < 1)
                    return -2;

                WinCountRX2 = 0;
                double WinMoney = 0;
                WinMoneyNoWithTax = 0;
                Description = "";

                string[] Ball = null;

                string RegexString = @"^((\d\d\s){1,11}\d\d)";
                Regex regex = new Regex(RegexString, RegexOptions.Compiled | RegexOptions.IgnoreCase);

                for (int ii = 0; ii < Lotterys.Length; ii++)
                {
                    Match m = regex.Match(Lotterys[ii]);

                    if (!m.Success)
                    {
                        continue;
                    }

                    Ball = FilterRepeated(m.Value.Trim().Split(' '), 11);

                    if (Ball.Length < 2)
                    {
                        continue;
                    }

                    int BallRight = 0;

                    foreach (string str in Ball)
                    {
                        if (!string.IsNullOrEmpty(str))
                        {
                            if (WinNumber.IndexOf(str.Trim()) >= 0)
                            {
                                BallRight++;
                            }
                        }
                    }

                    if (BallRight < 2)
                    {
                        continue;
                    }

                    int Count = GetNum(2, BallRight);

                    if (Count > 0)
                    {
                        WinCountRX2 = WinCountRX2 + Count;
                        WinMoney += WinMoney2 * Count;
                        WinMoneyNoWithTax += WinMoneyNoWithTax2 * Count;
                        continue;
                    }
                }

                if (WinCountRX2 > 0)
                {
                    Description = "任选二奖" + WinCountRX2.ToString() + "注";
                }

                if (Description != "")
                    Description += "。";

                return WinMoney;
            }
            private double ComputeWin_RX3(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney3, double WinMoneyNoWithTax3, ref int WinCountRX3)
            {
                WinNumber = WinNumber.Trim();
                if (WinNumber.Length < 14)	//14: "01 02 03 04 05"
                    return -1;
                string[] Lotterys = SplitLotteryNumber(Number);
                if (Lotterys == null)
                    return -2;
                if (Lotterys.Length < 1)
                    return -2;

                WinCountRX3 = 0;
                double WinMoney = 0;
                WinMoneyNoWithTax = 0;
                Description = "";

                string[] Ball = null;

                string RegexString = @"^((\d\d\s){2,11}\d\d)";
                Regex regex = new Regex(RegexString, RegexOptions.Compiled | RegexOptions.IgnoreCase);

                for (int ii = 0; ii < Lotterys.Length; ii++)
                {
                    Match m = regex.Match(Lotterys[ii]);

                    if (!m.Success)
                    {
                        continue;
                    }

                    Ball = FilterRepeated(m.Value.Trim().Split(' '), 11);

                    if (Ball.Length < 3)
                    {
                        continue;
                    }

                    int BallRight = 0;

                    foreach (string str in Ball)
                    {
                        if (!string.IsNullOrEmpty(str))
                        {
                            if (WinNumber.IndexOf(str.Trim()) >= 0)
                            {
                                BallRight++;
                            }
                        }
                    }

                    if (BallRight < 3)
                    {
                        continue;
                    }

                    int Count = GetNum(3, BallRight);

                    if (Count > 0)
                    {
                        WinCountRX3 = WinCountRX3 + Count;
                        WinMoney += WinMoney3 * Count;
                        WinMoneyNoWithTax += WinMoneyNoWithTax3 * Count;
                        continue;
                    }
                }

                if (WinCountRX3 > 0)
                {
                    Description = "任选三奖" + WinCountRX3.ToString() + "注";
                }

                if (Description != "")
                    Description += "。";

                return WinMoney;
            }
            private double ComputeWin_RX4(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney4, double WinMoneyNoWithTax4, ref int WinCountRX4)
            {
                WinNumber = WinNumber.Trim();
                if (WinNumber.Length < 14)	//14: "01 02 03 04 05"
                    return -1;
                string[] Lotterys = SplitLotteryNumber(Number);
                if (Lotterys == null)
                    return -2;
                if (Lotterys.Length < 1)
                    return -2;

                WinCountRX4 = 0;
                double WinMoney = 0;
                WinMoneyNoWithTax = 0;
                Description = "";

                string[] Ball = null;

                string RegexString = @"^((\d\d\s){3,11}\d\d)";
                Regex regex = new Regex(RegexString, RegexOptions.Compiled | RegexOptions.IgnoreCase);

                for (int ii = 0; ii < Lotterys.Length; ii++)
                {
                    Match m = regex.Match(Lotterys[ii]);

                    if (!m.Success)
                    {
                        continue;
                    }

                    Ball = FilterRepeated(m.Value.Trim().Split(' '), 11);

                    if (Ball.Length < 4)
                    {
                        continue;
                    }

                    int BallRight = 0;

                    foreach (string str in Ball)
                    {
                        if (!string.IsNullOrEmpty(str))
                        {
                            if (WinNumber.IndexOf(str.Trim()) >= 0)
                            {
                                BallRight++;
                            }
                        }
                    }

                    if (BallRight < 4)
                    {
                        continue;
                    }

                    int Count = GetNum(4, BallRight);

                    if (Count > 0)
                    {
                        WinCountRX4 = WinCountRX4 + Count;
                        WinMoney += WinMoney4 * Count;
                        WinMoneyNoWithTax += WinMoneyNoWithTax4 * Count;
                        continue;
                    }
                }

                if (WinCountRX4 > 0)
                {
                    Description = "任选四奖" + WinCountRX4.ToString() + "注";
                }

                if (Description != "")
                    Description += "。";

                return WinMoney;
            }
            private double ComputeWin_RX5(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney5, double WinMoneyNoWithTax5, ref int WinCountRX5)
            {
                WinNumber = WinNumber.Trim();
                if (WinNumber.Length < 14)	//14: "01 02 03 04 05"
                    return -1;
                string[] Lotterys = SplitLotteryNumber(Number);
                if (Lotterys == null)
                    return -2;
                if (Lotterys.Length < 1)
                    return -2;

                WinCountRX5 = 0;
                double WinMoney = 0;
                WinMoneyNoWithTax = 0;
                Description = "";

                string[] Ball = null;

                string RegexString = @"^((\d\d\s){4,11}\d\d)";
                Regex regex = new Regex(RegexString, RegexOptions.Compiled | RegexOptions.IgnoreCase);

                for (int ii = 0; ii < Lotterys.Length; ii++)
                {
                    Match m = regex.Match(Lotterys[ii]);

                    if (!m.Success)
                    {
                        continue;
                    }

                    Ball = FilterRepeated(m.Value.Trim().Split(' '), 11);

                    if (Ball.Length < 5)
                    {
                        continue;
                    }

                    int BallRight = 0;

                    foreach (string str in Ball)
                    {
                        if (!string.IsNullOrEmpty(str))
                        {
                            if (WinNumber.IndexOf(str.Trim()) >= 0)
                            {
                                BallRight++;
                            }
                        }
                    }

                    if (BallRight < 5)
                    {
                        continue;
                    }

                    int Count = GetNum(5, BallRight);

                    if (Count > 0)
                    {
                        WinCountRX5 = WinCountRX5 + Count;
                        WinMoney += WinMoney5 * Count;
                        WinMoneyNoWithTax += WinMoneyNoWithTax5 * Count;
                        continue;
                    }
                }

                if (WinCountRX5 > 0)
                {
                    Description = "任选五奖" + WinCountRX5.ToString() + "注";
                }

                if (Description != "")
                    Description += "。";

                return WinMoney;
            }
            private double ComputeWin_RX6(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney6, double WinMoneyNoWithTax6, ref int WinCountRX6)
            {
                WinNumber = WinNumber.Trim();
                if (WinNumber.Length < 14)	//14: "01 02 03 04 05"
                    return -1;
                string[] Lotterys = SplitLotteryNumber(Number);
                if (Lotterys == null)
                    return -2;
                if (Lotterys.Length < 1)
                    return -2;

                WinCountRX6 = 0;
                double WinMoney = 0;
                WinMoneyNoWithTax = 0;
                Description = "";

                string[] Ball = null;

                string RegexString = @"^((\d\d\s){5,11}\d\d)";
                Regex regex = new Regex(RegexString, RegexOptions.Compiled | RegexOptions.IgnoreCase);

                for (int ii = 0; ii < Lotterys.Length; ii++)
                {
                    Match m = regex.Match(Lotterys[ii]);

                    if (!m.Success)
                    {
                        continue;
                    }

                    Ball = FilterRepeated(m.Value.Trim().Split(' '), 11);

                    if (Ball.Length < 6)
                    {
                        continue;
                    }

                    int BallRight = 0;

                    foreach (string str in Ball)
                    {
                        if (!string.IsNullOrEmpty(str))
                        {
                            if (WinNumber.IndexOf(str.Trim()) >= 0)
                            {
                                BallRight++;
                            }
                        }
                    }

                    if (BallRight < 5)
                    {
                        continue;
                    }

                    int Count = GetNum(5, BallRight) * GetNum(1, Ball.Length - BallRight);

                    if (Count > 0)
                    {
                        WinCountRX6 = WinCountRX6 + Count;
                        WinMoney += WinMoney6 * Count;
                        WinMoneyNoWithTax += WinMoneyNoWithTax6 * Count;
                        continue;
                    }
                }

                if (WinCountRX6 > 0)
                {
                    Description = "任选六奖" + WinCountRX6.ToString() + "注";
                }

                if (Description != "")
                    Description += "。";

                return WinMoney;
            }
            private double ComputeWin_RX7(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney7, double WinMoneyNoWithTax7, ref int WinCountRX7)
            {
                WinNumber = WinNumber.Trim();
                if (WinNumber.Length < 14)	//14: "01 02 03 04 05"
                    return -1;
                string[] Lotterys = SplitLotteryNumber(Number);
                if (Lotterys == null)
                    return -2;
                if (Lotterys.Length < 1)
                    return -2;

                WinCountRX7 = 0;
                double WinMoney = 0;
                WinMoneyNoWithTax = 0;
                Description = "";

                string[] Ball = null;

                string RegexString = @"^((\d\d\s){6,11}\d\d)";
                Regex regex = new Regex(RegexString, RegexOptions.Compiled | RegexOptions.IgnoreCase);

                for (int ii = 0; ii < Lotterys.Length; ii++)
                {
                    Match m = regex.Match(Lotterys[ii]);

                    if (!m.Success)
                    {
                        continue;
                    }

                    Ball = FilterRepeated(m.Value.Trim().Split(' '), 11);

                    if (Ball.Length < 7)
                    {
                        continue;
                    }

                    int BallRight = 0;

                    foreach (string str in Ball)
                    {
                        if (!string.IsNullOrEmpty(str))
                        {
                            if (WinNumber.IndexOf(str.Trim()) >= 0)
                            {
                                BallRight++;
                            }
                        }
                    }

                    if (BallRight < 5)
                    {
                        continue;
                    }

                    int Count = GetNum(5, BallRight) * GetNum(2, Ball.Length - BallRight);

                    if (Count > 0)
                    {
                        WinCountRX7 = WinCountRX7 + Count;
                        WinMoney += WinMoney7 * Count;
                        WinMoneyNoWithTax += WinMoneyNoWithTax7 * Count;
                        continue;
                    }
                }

                if (WinCountRX7 > 0)
                {
                    Description = "任选七奖" + WinCountRX7.ToString() + "注";
                }

                if (Description != "")
                    Description += "。";

                return WinMoney;
            }
            private double ComputeWin_RX8(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney8, double WinMoneyNoWithTax8, ref int WinCountRX8)
            {
                WinNumber = WinNumber.Trim();
                if (WinNumber.Length < 14)	//14: "01 02 03 04 05"
                    return -1;
                string[] Lotterys = SplitLotteryNumber(Number);
                if (Lotterys == null)
                    return -2;
                if (Lotterys.Length < 1)
                    return -2;

                WinCountRX8 = 0;
                double WinMoney = 0;
                WinMoneyNoWithTax = 0;
                Description = "";

                string[] Ball = null;

                string RegexString = @"^((\d\d\s){7,11}\d\d)";
                Regex regex = new Regex(RegexString, RegexOptions.Compiled | RegexOptions.IgnoreCase);

                for (int ii = 0; ii < Lotterys.Length; ii++)
                {
                    Match m = regex.Match(Lotterys[ii]);

                    if (!m.Success)
                    {
                        continue;
                    }

                    Ball = FilterRepeated(m.Value.Trim().Split(' '), 11);

                    if (Ball.Length < 8)
                    {
                        continue;
                    }

                    int BallRight = 0;

                    foreach (string str in Ball)
                    {
                        if (!string.IsNullOrEmpty(str))
                        {
                            if (WinNumber.IndexOf(str.Trim()) >= 0)
                            {
                                BallRight++;
                            }
                        }
                    }

                    if (BallRight < 5)
                    {
                        continue;
                    }

                    int Count = GetNum(5, BallRight) * GetNum(3, Ball.Length - BallRight);

                    if (Count > 0)
                    {
                        WinCountRX8 = WinCountRX8 + Count;
                        WinMoney += WinMoney8 * Count;
                        WinMoneyNoWithTax += WinMoneyNoWithTax8 * Count;
                        continue;
                    }
                }

                if (WinCountRX8 > 0)
                {
                    Description = "任选八奖" + WinCountRX8.ToString() + "注";
                }

                if (Description != "")
                    Description += "。";

                return WinMoney;
            }

            private double ComputeWin_ZhiQ2(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney9, double WinMoneyNoWithTax9, ref int WinCount_ZhiQ2)
            {
                WinNumber = WinNumber.Trim();
                if (WinNumber.Length < 14)	//14: "01 02 03 04 05"
                    return -1;
                string[] Lotterys = SplitLotteryNumber(Number);
                if (Lotterys == null)
                    return -2;
                if (Lotterys.Length < 1)
                    return -2;

                WinCount_ZhiQ2 = 0;
                double WinMoney = 0;
                WinMoneyNoWithTax = 0;
                Description = "";

                for (int ii = 0; ii < Lotterys.Length; ii++)
                {
                    string t_str = "";
                    string[] Lottery = ToSingle_ZhiQ2(Lotterys[ii], ref t_str);
                    if (Lottery == null)
                        continue;
                    if (Lottery.Length < 1)
                        continue;

                    for (int i = 0; i < Lottery.Length; i++)
                    {
                        bool Full = false;

                        string[] Red = new string[2];
                        Regex regex = new Regex(@"^(?<R0>\d\d\s)(?<R1>\d\d)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                        if (regex.IsMatch(Lottery[i]))
                        {
                            if (WinNumber.Substring(0, 5) == Lottery[i])
                                Full = true;
                        }

                        if (Full)
                        {
                            WinCount_ZhiQ2++;
                            WinMoney += WinMoney9;
                            WinMoneyNoWithTax += WinMoneyNoWithTax9;
                            continue;
                        }
                    }
                }

                if (WinCount_ZhiQ2 > 0)
                {
                    Description = "直选前二奖" + WinCount_ZhiQ2.ToString() + "注";
                }

                if (Description != "")
                    Description += "。";

                return WinMoney;
            }
            private double ComputeWin_ZhiQ3(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney10, double WinMoneyNoWithTax10, ref int WinCount_ZhiQ3)
            {
                WinNumber = WinNumber.Trim();
                if (WinNumber.Length < 14)	//14: "01 02 03 04 05"
                    return -1;
                string[] Lotterys = SplitLotteryNumber(Number);
                if (Lotterys == null)
                    return -2;
                if (Lotterys.Length < 1)
                    return -2;

                WinCount_ZhiQ3 = 0;
                double WinMoney = 0;
                WinMoneyNoWithTax = 0;
                Description = "";

                for (int ii = 0; ii < Lotterys.Length; ii++)
                {
                    string t_str = "";
                    string[] Lottery = ToSingle_ZhiQ3(Lotterys[ii], ref t_str);
                    if (Lottery == null)
                        continue;
                    if (Lottery.Length < 1)
                        continue;

                    for (int i = 0; i < Lottery.Length; i++)
                    {
                        bool Full = false;

                        string[] Red = new string[3];
                        Regex regex = new Regex(@"^(?<R0>\d\d\s)(?<R1>\d\d\s)(?<R2>\d\d)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                        if (regex.IsMatch(Lottery[i]))
                        {
                            if (WinNumber.Substring(0, 8) == Lottery[i])
                                Full = true;
                        }

                        if (Full)
                        {
                            WinCount_ZhiQ3++;
                            WinMoney += WinMoney10;
                            WinMoneyNoWithTax += WinMoneyNoWithTax10;
                            continue;
                        }
                    }
                }

                if (WinCount_ZhiQ3 > 0)
                {
                    Description = "直选前三奖" + WinCount_ZhiQ3.ToString() + "注";
                }

                if (Description != "")
                    Description += "。";

                return WinMoney;
            }
            private double ComputeWin_ZuQ2(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney11, double WinMoneyNoWithTax11, ref int WinCount_ZuQ2)
            {
                WinNumber = WinNumber.Trim();
                if (WinNumber.Length < 14)	//14: "01 02 03 04 05"
                    return -1;
                string[] Lotterys = SplitLotteryNumber(Number);
                if (Lotterys == null)
                    return -2;
                if (Lotterys.Length < 1)
                    return -2;

                WinCount_ZuQ2 = 0;
                double WinMoney = 0;
                WinMoneyNoWithTax = 0;
                Description = "";

                for (int ii = 0; ii < Lotterys.Length; ii++)
                {
                    string t_str = "";
                    string[] Lottery = ToSingle_ZuQ2(Lotterys[ii], ref t_str);
                    if (Lottery == null)
                        continue;
                    if (Lottery.Length < 1)
                        continue;

                    for (int i = 0; i < Lottery.Length; i++)
                    {
                        int RedRight = 0;

                        string[] Red = new string[2];
                        Regex regex = new Regex(@"^(?<R0>\d\d\s)(?<R1>\d\d)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                        Match m = regex.Match(Lottery[i]);

                        for (int j = 0; j < 2; j++)
                        {
                            Red[j] = m.Groups["R" + j.ToString()].ToString().Trim();

                            if (Red[j] == "")
                            {
                                break;
                            }

                            if (WinNumber.Substring(0, 5).IndexOf(Red[j]) >= 0)
                                RedRight++;
                        }

                        if (RedRight == 2)
                        {
                            WinCount_ZuQ2++;
                            WinMoney += WinMoney11;
                            WinMoneyNoWithTax += WinMoneyNoWithTax11;
                            continue;
                        }
                    }
                }

                if (WinCount_ZuQ2 > 0)
                {
                    Description = "组选前二奖" + WinCount_ZuQ2.ToString() + "注";
                }

                if (Description != "")
                    Description += "。";

                return WinMoney;
            }
            private double ComputeWin_ZuQ3(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney12, double WinMoneyNoWithTax12, ref int WinCount_ZuQ3)
            {
                WinNumber = WinNumber.Trim();
                if (WinNumber.Length < 14)	//14: "01 02 03 04 05"
                    return -1;
                string[] Lotterys = SplitLotteryNumber(Number);
                if (Lotterys == null)
                    return -2;
                if (Lotterys.Length < 1)
                    return -2;

                WinCount_ZuQ3 = 0;
                double WinMoney = 0;
                WinMoneyNoWithTax = 0;
                Description = "";

                for (int ii = 0; ii < Lotterys.Length; ii++)
                {
                    string t_str = "";
                    string[] Lottery = ToSingle_ZuQ3(Lotterys[ii], ref t_str);
                    if (Lottery == null)
                        continue;
                    if (Lottery.Length < 1)
                        continue;

                    for (int i = 0; i < Lottery.Length; i++)
                    {
                        int RedRight = 0;

                        string[] Red = new string[3];
                        Regex regex = new Regex(@"^(?<R0>\d\d\s)(?<R1>\d\d\s)(?<R2>\d\d)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                        Match m = regex.Match(Lottery[i]);

                        for (int j = 0; j < 3; j++)
                        {
                            Red[j] = m.Groups["R" + j.ToString()].ToString().Trim();

                            if (Red[j] == "")
                            {
                                break;
                            }

                            if (WinNumber.Substring(0, 8).IndexOf(Red[j]) >= 0)
                                RedRight++;
                        }

                        if (RedRight == 3)
                        {
                            WinCount_ZuQ3++;
                            WinMoney += WinMoney12;
                            WinMoneyNoWithTax += WinMoneyNoWithTax12;
                            continue;
                        }
                    }
                }

                if (WinCount_ZuQ3 > 0)
                {
                    Description = "组选前三奖" + WinCount_ZuQ3.ToString() + "注";
                }

                if (Description != "")
                    Description += "。";

                return WinMoney;
            }
            #endregion

            public override string AnalyseScheme(string Content, int PlayType)
            {
                if (PlayType == PlayType_RX1)
                    return AnalyseScheme_RX1(Content, PlayType);

                if (PlayType == PlayType_RX2)
                    return AnalyseScheme_RX2(Content, PlayType);

                if (PlayType == PlayType_RX3)
                    return AnalyseScheme_RX3(Content, PlayType);

                if (PlayType == PlayType_RX4)
                    return AnalyseScheme_RX4(Content, PlayType);

                if (PlayType == PlayType_RX5)
                    return AnalyseScheme_RX5(Content, PlayType);

                if (PlayType == PlayType_RX6)
                    return AnalyseScheme_RX6(Content, PlayType);

                if (PlayType == PlayType_RX7)
                    return AnalyseScheme_RX7(Content, PlayType);

                if (PlayType == PlayType_RX8)
                    return AnalyseScheme_RX8(Content, PlayType);

                if (PlayType == PlayType_ZhiQ2)
                    return AnalyseScheme_ZhiQ2(Content, PlayType);

                if (PlayType == PlayType_ZhiQ3)
                    return AnalyseScheme_ZhiQ3(Content, PlayType);

                if (PlayType == PlayType_ZuQ2)
                    return AnalyseScheme_ZuQ2(Content, PlayType);

                if (PlayType == PlayType_ZuQ3)
                    return AnalyseScheme_ZuQ3(Content, PlayType);

                return "";
            }
            #region AnalyseScheme 的具体方法
            private string AnalyseScheme_RX1(string Content, int PlayType)
            {
                string[] strs = Content.Split('\n');
                if (strs == null)
                    return "";
                if (strs.Length == 0)
                    return "";

                string Result = "";

                string RegexString = @"^(\d\d)";
                Regex regex = new Regex(RegexString, RegexOptions.Compiled | RegexOptions.IgnoreCase);

                for (int i = 0; i < strs.Length; i++)
                {
                    Match m = regex.Match(strs[i]);
                    if (m.Success)
                    {
                        string CanonicalNumber = "";
                        string[] singles = ToSingle_RX1(m.Value, ref CanonicalNumber);
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
            private string AnalyseScheme_RX2(string Content, int PlayType)
            {
                string[] strs = Content.Split('\n');
                if (strs == null)
                    return "";
                if (strs.Length == 0)
                    return "";

                string Result = "";

                string RegexString = @"^((\d\d\s){1,11}\d\d)";
                Regex regex = new Regex(RegexString, RegexOptions.Compiled | RegexOptions.IgnoreCase);

                for (int i = 0; i < strs.Length; i++)
                {
                    Match m = regex.Match(strs[i]);
                    if (!m.Success)
                    {
                        continue;
                    }

                    string CanonicalNumber = "";
                    string[] Number = FilterRepeated(m.Value.Trim().Split(' '), 11);

                    if (Number.Length < 2)
                    {
                        continue;
                    }

                    for (int j = 0; j < Number.Length; j++)
                    {
                        CanonicalNumber += Number[j] + " ";
                    }

                    Result += CanonicalNumber + "|" + GetNum(2, Number.Length).ToString() + "\n";
                }

                if (Result.EndsWith("\n"))
                    Result = Result.Substring(0, Result.Length - 1);
                return Result;
            }
            private string AnalyseScheme_RX3(string Content, int PlayType)
            {
                string[] strs = Content.Split('\n');
                if (strs == null)
                    return "";
                if (strs.Length == 0)
                    return "";

                string Result = "";

                string RegexString = @"^((\d\d\s){2,11}\d\d)";
                Regex regex = new Regex(RegexString, RegexOptions.Compiled | RegexOptions.IgnoreCase);

                for (int i = 0; i < strs.Length; i++)
                {
                    Match m = regex.Match(strs[i]);
                    if (!m.Success)
                    {
                        continue;
                    }

                    string CanonicalNumber = "";
                    string[] Number = FilterRepeated(m.Value.Trim().Split(' '), 11);

                    if (Number.Length < 3)
                    {
                        continue;
                    }

                    for (int j = 0; j < Number.Length; j++)
                    {
                        CanonicalNumber += Number[j] + " ";
                    }

                    Result += CanonicalNumber + "|" + GetNum(3, Number.Length).ToString() + "\n";
                }

                if (Result.EndsWith("\n"))
                    Result = Result.Substring(0, Result.Length - 1);
                return Result;
            }
            private string AnalyseScheme_RX4(string Content, int PlayType)
            {
                string[] strs = Content.Split('\n');
                if (strs == null)
                    return "";
                if (strs.Length == 0)
                    return "";

                string Result = "";

                string RegexString = @"^((\d\d\s){3,11}\d\d)";
                Regex regex = new Regex(RegexString, RegexOptions.Compiled | RegexOptions.IgnoreCase);

                for (int i = 0; i < strs.Length; i++)
                {
                    Match m = regex.Match(strs[i]);
                    if (!m.Success)
                    {
                        continue;
                    }

                    string CanonicalNumber = "";
                    string[] Number = FilterRepeated(m.Value.Trim().Split(' '), 11);

                    if (Number.Length < 4)
                    {
                        continue;
                    }

                    for (int j = 0; j < Number.Length; j++)
                    {
                        CanonicalNumber += Number[j] + " ";
                    }

                    Result += CanonicalNumber + "|" + GetNum(4, Number.Length).ToString() + "\n";
                }

                if (Result.EndsWith("\n"))
                    Result = Result.Substring(0, Result.Length - 1);
                return Result;
            }
            private string AnalyseScheme_RX5(string Content, int PlayType)
            {
                string[] strs = Content.Split('\n');
                if (strs == null)
                    return "";
                if (strs.Length == 0)
                    return "";

                string Result = "";

                string RegexString = @"^((\d\d\s){4,11}\d\d)";
                Regex regex = new Regex(RegexString, RegexOptions.Compiled | RegexOptions.IgnoreCase);

                for (int i = 0; i < strs.Length; i++)
                {
                    Match m = regex.Match(strs[i]);
                    if (!m.Success)
                    {
                        continue;
                    }

                    string CanonicalNumber = "";
                    string[] Number = FilterRepeated(m.Value.Trim().Split(' '), 11);

                    if (Number.Length < 5)
                    {
                        continue;
                    }

                    for (int j = 0; j < Number.Length; j++)
                    {
                        CanonicalNumber += Number[j] + " ";
                    }

                    Result += CanonicalNumber + "|" + GetNum(5, Number.Length).ToString() + "\n";
                }

                if (Result.EndsWith("\n"))
                    Result = Result.Substring(0, Result.Length - 1);
                return Result;
            }
            private string AnalyseScheme_RX6(string Content, int PlayType)
            {
                string[] strs = Content.Split('\n');
                if (strs == null)
                    return "";
                if (strs.Length == 0)
                    return "";

                string Result = "";

                string RegexString = @"^((\d\d\s){5,11}\d\d)";
                Regex regex = new Regex(RegexString, RegexOptions.Compiled | RegexOptions.IgnoreCase);

                for (int i = 0; i < strs.Length; i++)
                {
                    Match m = regex.Match(strs[i]);
                    if (!m.Success)
                    {
                        continue;
                    }

                    string CanonicalNumber = "";
                    string[] Number = FilterRepeated(m.Value.Trim().Split(' '), 11);

                    if (Number.Length < 6)
                    {
                        continue;
                    }

                    for (int j = 0; j < Number.Length; j++)
                    {
                        CanonicalNumber += Number[j] + " ";
                    }

                    Result += CanonicalNumber + "|" + GetNum(6, Number.Length).ToString() + "\n";
                }

                if (Result.EndsWith("\n"))
                    Result = Result.Substring(0, Result.Length - 1);
                return Result;
            }
            private string AnalyseScheme_RX7(string Content, int PlayType)
            {
                string[] strs = Content.Split('\n');
                if (strs == null)
                    return "";
                if (strs.Length == 0)
                    return "";

                string Result = "";

                string RegexString = @"^((\d\d\s){6,11}\d\d)";
                Regex regex = new Regex(RegexString, RegexOptions.Compiled | RegexOptions.IgnoreCase);

                for (int i = 0; i < strs.Length; i++)
                {
                    Match m = regex.Match(strs[i]);
                    if (!m.Success)
                    {
                        continue;
                    }

                    string CanonicalNumber = "";
                    string[] Number = FilterRepeated(m.Value.Trim().Split(' '), 11);

                    if (Number.Length < 7)
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
            private string AnalyseScheme_RX8(string Content, int PlayType)
            {
                string[] strs = Content.Split('\n');
                if (strs == null)
                    return "";
                if (strs.Length == 0)
                    return "";

                string Result = "";

                string RegexString = @"^((\d\d\s){7,11}\d\d)";
                Regex regex = new Regex(RegexString, RegexOptions.Compiled | RegexOptions.IgnoreCase);

                for (int i = 0; i < strs.Length; i++)
                {
                    Match m = regex.Match(strs[i]);
                    if (!m.Success)
                    {
                        continue;
                    }

                    string CanonicalNumber = "";
                    string[] Number = FilterRepeated(m.Value.Trim().Split(' '), 11);

                    if (Number.Length < 8)
                    {
                        continue;
                    }

                    for (int j = 0; j < Number.Length; j++)
                    {
                        CanonicalNumber += Number[j] + " ";
                    }

                    Result += CanonicalNumber + "|" + GetNum(8, Number.Length).ToString() + "\n";
                }

                if (Result.EndsWith("\n"))
                    Result = Result.Substring(0, Result.Length - 1);
                return Result;
            }
            private string AnalyseScheme_ZhiQ2(string Content, int PlayType)
            {
                string[] strs = Content.Split('\n');
                if (strs == null)
                    return "";
                if (strs.Length == 0)
                    return "";

                string Result = "";

                string RegexString = @"^((\d\d\s){0,8}(\d\d))[|]((\d\d\s){0,9}\d\d)";
                Regex regex = new Regex(RegexString, RegexOptions.Compiled | RegexOptions.IgnoreCase);

                for (int i = 0; i < strs.Length; i++)
                {
                    Match m = regex.Match(strs[i]);
                    if (m.Success)
                    {
                        string CanonicalNumber = "";
                        string[] singles = ToSingle_ZhiQ2(m.Value, ref CanonicalNumber);
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
            private string AnalyseScheme_ZhiQ3(string Content, int PlayType)
            {
                string[] strs = Content.Split('\n');
                if (strs == null)
                    return "";
                if (strs.Length == 0)
                    return "";

                string Result = "";

                string RegexString = @"^((\d\d\s){0,8}\d\d)[|]((\d\d\s){0,8}\d\d)[|]((\d\d\s){0,8}\d\d)";
                Regex regex = new Regex(RegexString, RegexOptions.Compiled | RegexOptions.IgnoreCase);

                for (int i = 0; i < strs.Length; i++)
                {
                    Match m = regex.Match(strs[i]);
                    if (m.Success)
                    {
                        string CanonicalNumber = "";
                        string[] singles = ToSingle_ZhiQ3(m.Value, ref CanonicalNumber);
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
            private string AnalyseScheme_ZuQ2(string Content, int PlayType)
            {
                string[] strs = Content.Split('\n');
                if (strs == null)
                    return "";
                if (strs.Length == 0)
                    return "";

                string Result = "";

                string RegexString = @"^(\d\d\s){1,10}\d\d";
                Regex regex = new Regex(RegexString, RegexOptions.Compiled | RegexOptions.IgnoreCase);

                for (int i = 0; i < strs.Length; i++)
                {
                    Match m = regex.Match(strs[i]);
                    if (m.Success)
                    {
                        string CanonicalNumber = "";
                        string[] singles = ToSingle_ZuQ2(m.Value, ref CanonicalNumber);
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
            private string AnalyseScheme_ZuQ3(string Content, int PlayType)
            {
                string[] strs = Content.Split('\n');
                if (strs == null)
                    return "";
                if (strs.Length == 0)
                    return "";

                string Result = "";

                string RegexString = @"^(\d\d\s){2,10}\d\d";
                Regex regex = new Regex(RegexString, RegexOptions.Compiled | RegexOptions.IgnoreCase);

                for (int i = 0; i < strs.Length; i++)
                {
                    Match m = regex.Match(strs[i]);
                    if (m.Success)
                    {
                        string CanonicalNumber = "";
                        string[] singles = ToSingle_ZuQ3(m.Value, ref CanonicalNumber);
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
                Regex regex = new Regex(@"^((\d\d\s){4}\d\d)", RegexOptions.Compiled | RegexOptions.IgnoreCase);

                return regex.IsMatch(Number);
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

            public override string ShowNumber(string Number)
            {
                return base.ShowNumber(Number, " ");
            }

            //参数说明 PlayTypeID:玩法类别; Number:投注号码; Multiple: 倍数; MaxMultiple:最大倍数;
            public override Ticket[] ToElectronicTicket_ZCW(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                if (PlayTypeID == PlayType_RX1)
                {
                    return ToElectronicTicket_ZCW_RX1(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                if (PlayTypeID == PlayType_RX2)
                {
                    return ToElectronicTicket_ZCW_RX2(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                if (PlayTypeID == PlayType_RX3)
                {
                    return ToElectronicTicket_ZCW_RX3(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                if (PlayTypeID == PlayType_RX4)
                {
                    return ToElectronicTicket_ZCW_RX4(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                if (PlayTypeID == PlayType_RX5)
                {
                    return ToElectronicTicket_ZCW_RX5(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                if (PlayTypeID == PlayType_RX6)
                {
                    return ToElectronicTicket_ZCW_RX6(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                if (PlayTypeID == PlayType_RX7)
                {
                    return ToElectronicTicket_ZCW_RX7(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                if (PlayTypeID == PlayType_RX8)
                {
                    return ToElectronicTicket_ZCW_RX8(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                if (PlayTypeID == PlayType_ZhiQ2)
                {
                    return ToElectronicTicket_ZCW_ZhiQ2(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                if (PlayTypeID == PlayType_ZhiQ3)
                {
                    return ToElectronicTicket_ZCW_ZhiQ3(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                if (PlayTypeID == PlayType_ZuQ2)
                {
                    return ToElectronicTicket_ZCW_ZuQ2(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                if (PlayTypeID == PlayType_ZuQ3)
                {
                    return ToElectronicTicket_ZCW_ZuQ3(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                return null;
            }
            #region ToElectronicTicket_ZCW 的具体方法
            private Ticket[] ToElectronicTicket_ZCW_RX1(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                string[] strs = AnalyseScheme_RX1(Number, PlayTypeID).Split('\n');

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
            private Ticket[] ToElectronicTicket_ZCW_RX2(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                string[] strs = AnalyseScheme_RX2(Number, PlayTypeID).Split('\n');

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

                    string Numbers = "";

                    EachMoney = 0;

                    string t_Numbers = "";
                    double t_EachMoney = 0;

                    int M = 0;

                    for (int i = 0; i < strs.Length; i++)
                    {
                        if (strs[i].ToString().Split('|').Length < 2)
                        {
                            continue;
                        }

                        if (double.Parse(strs[i].ToString().Split('|')[1]) == 1)
                        {
                            Numbers += strs[i].ToString().Split('|')[0] + "\n";
                            EachMoney += 2 * double.Parse(strs[i].ToString().Split('|')[1]);

                            M++;

                            if ((M == 5) || (i == strs.Length - 1))
                            {
                                Money += EachMoney * EachMultiple;

                                al.Add(new Ticket(0, ConvertFormatToElectronTicket_ZCW(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));

                                Numbers = "";
                                EachMoney = 0;

                                M = 0;
                            }
                        }
                        else
                        {
                            t_Numbers = strs[i].ToString().Split('|')[0] + "\n";
                            t_EachMoney = 2 * double.Parse(strs[i].ToString().Split('|')[1]);

                            Money += t_EachMoney * EachMultiple;

                            al.Add(new Ticket(1, ConvertFormatToElectronTicket_ZCW(PlayTypeID, t_Numbers), EachMultiple, t_EachMoney * EachMultiple));

                        }
                    }

                    if (Numbers != "")
                    {
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
            private Ticket[] ToElectronicTicket_ZCW_RX3(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                string[] strs = AnalyseScheme_RX3(Number, PlayTypeID).Split('\n');

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

                    string Numbers = "";

                    EachMoney = 0;

                    string t_Numbers = "";
                    double t_EachMoney = 0;

                    int M = 0;

                    for (int i = 0; i < strs.Length; i++)
                    {
                        if (strs[i].ToString().Split('|').Length < 2)
                        {
                            continue;
                        }

                        if (double.Parse(strs[i].ToString().Split('|')[1]) == 1)
                        {
                            Numbers += strs[i].ToString().Split('|')[0] + "\n";
                            EachMoney += 2 * double.Parse(strs[i].ToString().Split('|')[1]);

                            M++;

                            if ((M == 5) || (i == strs.Length - 1))
                            {
                                Money += EachMoney * EachMultiple;

                                al.Add(new Ticket(0, ConvertFormatToElectronTicket_ZCW(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));

                                Numbers = "";
                                EachMoney = 0;

                                M = 0;
                            }
                        }
                        else
                        {
                            t_Numbers = strs[i].ToString().Split('|')[0] + "\n";
                            t_EachMoney = 2 * double.Parse(strs[i].ToString().Split('|')[1]);

                            Money += t_EachMoney * EachMultiple;

                            al.Add(new Ticket(1, ConvertFormatToElectronTicket_ZCW(PlayTypeID, t_Numbers), EachMultiple, t_EachMoney * EachMultiple));

                        }
                    }

                    if (Numbers != "")
                    {
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
            private Ticket[] ToElectronicTicket_ZCW_RX4(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                string[] strs = AnalyseScheme_RX4(Number, PlayTypeID).Split('\n');

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

                    string Numbers = "";

                    EachMoney = 0;

                    string t_Numbers = "";
                    double t_EachMoney = 0;

                    int M = 0;

                    for (int i = 0; i < strs.Length; i++)
                    {
                        if (strs[i].ToString().Split('|').Length < 2)
                        {
                            continue;
                        }

                        if (double.Parse(strs[i].ToString().Split('|')[1]) == 1)
                        {
                            Numbers += strs[i].ToString().Split('|')[0] + "\n";
                            EachMoney += 2 * double.Parse(strs[i].ToString().Split('|')[1]);

                            M++;

                            if ((M == 5) || (i == strs.Length - 1))
                            {
                                Money += EachMoney * EachMultiple;

                                al.Add(new Ticket(0, ConvertFormatToElectronTicket_ZCW(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));

                                Numbers = "";
                                EachMoney = 0;

                                M = 0;
                            }
                        }
                        else
                        {
                            t_Numbers = strs[i].ToString().Split('|')[0] + "\n";
                            t_EachMoney = 2 * double.Parse(strs[i].ToString().Split('|')[1]);

                            Money += t_EachMoney * EachMultiple;

                            al.Add(new Ticket(1, ConvertFormatToElectronTicket_ZCW(PlayTypeID, t_Numbers), EachMultiple, t_EachMoney * EachMultiple));

                        }
                    }

                    if (Numbers != "")
                    {
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
            private Ticket[] ToElectronicTicket_ZCW_RX5(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                string[] strs = AnalyseScheme_RX5(Number, PlayTypeID).Split('\n');

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

                    string Numbers = "";

                    EachMoney = 0;

                    string t_Numbers = "";
                    double t_EachMoney = 0;

                    int M = 0;

                    for (int i = 0; i < strs.Length; i++)
                    {
                        if (strs[i].ToString().Split('|').Length < 2)
                        {
                            continue;
                        }

                        if (double.Parse(strs[i].ToString().Split('|')[1]) == 1)
                        {
                            Numbers += strs[i].ToString().Split('|')[0] + "\n";
                            EachMoney += 2 * double.Parse(strs[i].ToString().Split('|')[1]);

                            M++;

                            if ((M == 5) || (i == strs.Length - 1))
                            {
                                Money += EachMoney * EachMultiple;

                                al.Add(new Ticket(0, ConvertFormatToElectronTicket_ZCW(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));

                                Numbers = "";
                                EachMoney = 0;

                                M = 0;
                            }
                        }
                        else
                        {
                            t_Numbers = strs[i].ToString().Split('|')[0] + "\n";
                            t_EachMoney = 2 * double.Parse(strs[i].ToString().Split('|')[1]);

                            Money += t_EachMoney * EachMultiple;

                            al.Add(new Ticket(1, ConvertFormatToElectronTicket_ZCW(PlayTypeID, t_Numbers), EachMultiple, t_EachMoney * EachMultiple));

                        }
                    }

                    if (Numbers != "")
                    {
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
            private Ticket[] ToElectronicTicket_ZCW_RX6(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                string[] strs = AnalyseScheme_RX6(Number, PlayTypeID).Split('\n');

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

                    string Numbers = "";

                    EachMoney = 0;

                    string t_Numbers = "";
                    double t_EachMoney = 0;

                    int M = 0;

                    for (int i = 0; i < strs.Length; i++)
                    {
                        if (strs[i].ToString().Split('|').Length < 2)
                        {
                            continue;
                        }

                        if (double.Parse(strs[i].ToString().Split('|')[1]) == 1)
                        {
                            Numbers += strs[i].ToString().Split('|')[0] + "\n";
                            EachMoney += 2 * double.Parse(strs[i].ToString().Split('|')[1]);

                            M++;

                            if ((M == 5) || (i == strs.Length - 1))
                            {
                                Money += EachMoney * EachMultiple;

                                al.Add(new Ticket(0, ConvertFormatToElectronTicket_ZCW(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));

                                Numbers = "";
                                EachMoney = 0;

                                M = 0;
                            }
                        }
                        else
                        {
                            t_Numbers = strs[i].ToString().Split('|')[0] + "\n";
                            t_EachMoney = 2 * double.Parse(strs[i].ToString().Split('|')[1]);

                            Money += t_EachMoney * EachMultiple;

                            al.Add(new Ticket(1, ConvertFormatToElectronTicket_ZCW(PlayTypeID, t_Numbers), EachMultiple, t_EachMoney * EachMultiple));

                        }
                    }

                    if (Numbers != "")
                    {
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
            private Ticket[] ToElectronicTicket_ZCW_RX7(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                string[] strs = AnalyseScheme_RX7(Number, PlayTypeID).Split('\n');

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

                    string Numbers = "";

                    EachMoney = 0;

                    string t_Numbers = "";
                    double t_EachMoney = 0;

                    int M = 0;

                    for (int i = 0; i < strs.Length; i++)
                    {
                        if (strs[i].ToString().Split('|').Length < 2)
                        {
                            continue;
                        }

                        if (double.Parse(strs[i].ToString().Split('|')[1]) == 1)
                        {
                            Numbers += strs[i].ToString().Split('|')[0] + "\n";
                            EachMoney += 2 * double.Parse(strs[i].ToString().Split('|')[1]);

                            M++;

                            if ((M == 5) || (i == strs.Length - 1))
                            {
                                Money += EachMoney * EachMultiple;

                                al.Add(new Ticket(0, ConvertFormatToElectronTicket_ZCW(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));

                                Numbers = "";
                                EachMoney = 0;

                                M = 0;
                            }
                        }
                        else
                        {
                            t_Numbers = strs[i].ToString().Split('|')[0] + "\n";
                            t_EachMoney = 2 * double.Parse(strs[i].ToString().Split('|')[1]);

                            Money += t_EachMoney * EachMultiple;

                            al.Add(new Ticket(1, ConvertFormatToElectronTicket_ZCW(PlayTypeID, t_Numbers), EachMultiple, t_EachMoney * EachMultiple));

                        }
                    }

                    if (Numbers != "")
                    {
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
            private Ticket[] ToElectronicTicket_ZCW_RX8(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                string[] strs = AnalyseScheme_RX8(Number, PlayTypeID).Split('\n');

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

                    string Numbers = "";

                    EachMoney = 0;

                    string t_Numbers = "";
                    double t_EachMoney = 0;

                    int M = 0;

                    for (int i = 0; i < strs.Length; i++)
                    {
                        if (strs[i].ToString().Split('|').Length < 2)
                        {
                            continue;
                        }

                        if (double.Parse(strs[i].ToString().Split('|')[1]) == 1)
                        {
                            Numbers += strs[i].ToString().Split('|')[0] + "\n";
                            EachMoney += 2 * double.Parse(strs[i].ToString().Split('|')[1]);

                            M++;

                            if ((M == 5) || (i == strs.Length - 1))
                            {
                                Money += EachMoney * EachMultiple;

                                al.Add(new Ticket(0, ConvertFormatToElectronTicket_ZCW(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));

                                Numbers = "";
                                EachMoney = 0;

                                M = 0;
                            }
                        }
                        else
                        {
                            t_Numbers = strs[i].ToString().Split('|')[0] + "\n";
                            t_EachMoney = 2 * double.Parse(strs[i].ToString().Split('|')[1]);

                            Money += t_EachMoney * EachMultiple;

                            al.Add(new Ticket(0, ConvertFormatToElectronTicket_ZCW(PlayTypeID, t_Numbers), EachMultiple, t_EachMoney * EachMultiple));

                        }
                    }

                    if (Numbers != "")
                    {
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
            private Ticket[] ToElectronicTicket_ZCW_ZhiQ2(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                string[] strs = AnalyseScheme_ZhiQ2(Number, PlayTypeID).Split('\n');

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

                    string Numbers = "";

                    EachMoney = 0;

                    string t_Numbers = "";
                    double t_EachMoney = 0;

                    int M = 0;

                    for (int i = 0; i < strs.Length; i++)
                    {
                        if (string.IsNullOrEmpty(strs[i].ToString()))
                        {
                            continue;
                        }

                        if (strs[i].ToString().Split('|').Length < 3)
                        {
                            continue;
                        }

                        if (double.Parse(strs[i].ToString().Split('|')[2]) == 1)
                        {
                            Numbers += strs[i].ToString().Split('|')[0] + "|" + strs[i].ToString().Split('|')[1] + "\n";
                            EachMoney += 2 * double.Parse(strs[i].ToString().Split('|')[2]);

                            M++;

                            if ((M == 5) || (i == strs.Length - 1))
                            {
                                Money += EachMoney * EachMultiple;

                                al.Add(new Ticket(0, ConvertFormatToElectronTicket_ZCW(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));

                                Numbers = "";
                                EachMoney = 0;

                                M = 0;
                            }
                        }
                        else
                        {
                            t_Numbers = strs[i].ToString().Split('|')[0] + "|" + strs[i].ToString().Split('|')[1] + "\n";
                            t_EachMoney = 2 * double.Parse(strs[i].ToString().Split('|')[2]);

                            Money += t_EachMoney * EachMultiple;

                            al.Add(new Ticket(1, ConvertFormatToElectronTicket_ZCW(PlayTypeID, t_Numbers.Replace(" ", ",")), EachMultiple, t_EachMoney * EachMultiple));

                        }
                    }

                    if (Numbers != "")
                    {
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
            private Ticket[] ToElectronicTicket_ZCW_ZhiQ3(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                string[] strs = AnalyseScheme_ZhiQ3(Number, PlayTypeID).Split('\n');

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

                string t_Numbers = "";
                double t_EachMoney = 0;

                int M = 0;

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

                    string Numbers = "";

                    EachMoney = 0;

                    for (int i = 0; i < strs.Length; i++)
                    {
                        if (string.IsNullOrEmpty(strs[i].ToString()))
                        {
                            continue;
                        }

                        if (strs[i].ToString().Split('|').Length < 4)
                        {
                            continue;
                        }

                        if (double.Parse(strs[i].ToString().Split('|')[3]) == 1)
                        {
                            Numbers += strs[i].ToString().Split('|')[0] + "|" + strs[i].ToString().Split('|')[1] + "|" + strs[i].ToString().Split('|')[2] + "\n";
                            EachMoney += 2 * double.Parse(strs[i].ToString().Split('|')[3]);

                            M++;

                            if ((M == 5) || (i == strs.Length - 1))
                            {
                                Money += EachMoney * EachMultiple;

                                al.Add(new Ticket(0, ConvertFormatToElectronTicket_ZCW(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));

                                Numbers = "";
                                EachMoney = 0;

                                M = 0;
                            }
                        }
                        else
                        {
                            t_Numbers = strs[i].ToString().Split('|')[0] + "|" + strs[i].ToString().Split('|')[1] + "|" + strs[i].ToString().Split('|')[2] + "\n";
                            t_EachMoney = 2 * double.Parse(strs[i].ToString().Split('|')[3]);

                            Money += t_EachMoney * EachMultiple;

                            al.Add(new Ticket(1, ConvertFormatToElectronTicket_ZCW(PlayTypeID, t_Numbers.Replace(" ", ",")), EachMultiple, t_EachMoney * EachMultiple));

                        }
                    }

                    if (Numbers != "")
                    {
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
            private Ticket[] ToElectronicTicket_ZCW_ZuQ2(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                string[] strs = AnalyseScheme_ZuQ2(Number, PlayTypeID).Split('\n');

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

                    string Numbers = "";

                    EachMoney = 0;

                    string t_Numbers = "";
                    double t_EachMoney = 0;

                    int M = 0;

                    for (int i = 0; i < strs.Length; i++)
                    {
                        if (strs[i].ToString().Split('|').Length < 2)
                        {
                            continue;
                        }

                        if (double.Parse(strs[i].ToString().Split('|')[1]) == 1)
                        {
                            Numbers += strs[i].ToString().Split('|')[0] + "\n";
                            EachMoney += 2 * double.Parse(strs[i].ToString().Split('|')[1]);

                            M++;

                            if ((M == 5) || (i == strs.Length - 1))
                            {
                                Money += EachMoney * EachMultiple;

                                al.Add(new Ticket(0, ConvertFormatToElectronTicket_ZCW(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));

                                Numbers = "";
                                EachMoney = 0;

                                M = 0;
                            }
                        }
                        else
                        {
                            t_Numbers = strs[i].ToString().Split('|')[0] + "\n";
                            t_EachMoney = 2 * double.Parse(strs[i].ToString().Split('|')[1]);

                            Money += t_EachMoney * EachMultiple;

                            al.Add(new Ticket(1, ConvertFormatToElectronTicket_ZCW(PlayTypeID, t_Numbers), EachMultiple, t_EachMoney * EachMultiple));

                        }
                    }

                    if (Numbers != "")
                    {
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
            private Ticket[] ToElectronicTicket_ZCW_ZuQ3(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                string[] strs = AnalyseScheme_ZuQ3(Number, PlayTypeID).Split('\n');

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

                    string Numbers = "";

                    EachMoney = 0;

                    string t_Numbers = "";
                    double t_EachMoney = 0;

                    int M = 0;

                    for (int i = 0; i < strs.Length; i++)
                    {
                        if (strs[i].ToString().Split('|').Length < 2)
                        {
                            continue;
                        }

                        if (double.Parse(strs[i].ToString().Split('|')[1]) == 1)
                        {
                            Numbers += strs[i].ToString().Split('|')[0] + "\n";
                            EachMoney += 2 * double.Parse(strs[i].ToString().Split('|')[1]);

                            M++;

                            if ((M == 5) || (i == strs.Length - 1))
                            {
                                Money += EachMoney * EachMultiple;

                                al.Add(new Ticket(0, ConvertFormatToElectronTicket_ZCW(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));

                                Numbers = "";
                                EachMoney = 0;

                                M = 0;
                            }
                        }
                        else
                        {
                            t_Numbers = strs[i].ToString().Split('|')[0] + "\n";
                            t_EachMoney = 2 * double.Parse(strs[i].ToString().Split('|')[1]);

                            Money += t_EachMoney * EachMultiple;

                            al.Add(new Ticket(1, ConvertFormatToElectronTicket_ZCW(PlayTypeID, t_Numbers), EachMultiple, t_EachMoney * EachMultiple));

                        }
                    }

                    if (Numbers != "")
                    {
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

            // 转换为需要的彩票格式
            private string ConvertFormatToElectronTicket_ZCW(int PlayTypeID, string Number)
            {
                Number = Number.Trim();

                string Ticket = "";

                if (PlayTypeID == PlayType_RX1 || PlayTypeID == PlayType_RX2 || PlayTypeID == PlayType_RX3 || PlayTypeID == PlayType_RX4 || PlayTypeID == PlayType_RX5 || PlayTypeID == PlayType_RX6 || PlayTypeID == PlayType_RX7 || PlayTypeID == PlayType_RX8)
                {
                    Ticket = Number.Replace(" ", "");
                }

                if ((PlayTypeID == PlayType_ZhiQ2) || (PlayTypeID == PlayType_ZhiQ3))
                {
                    Ticket = Number.Replace(" ", "").Replace("|", "");
                }

                if ((PlayTypeID == PlayType_ZuQ2) || (PlayTypeID == PlayType_ZuQ3))
                {
                    Ticket = Number.Replace(" ", "").Replace("|", "");
                }

                return Ticket;
            }
            #endregion

            public override Ticket[] ToElectronicTicket_ZZYTC(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                if (PlayTypeID == PlayType_RX1)
                {
                    return ToElectronicTicket_ZZYTC_RX1(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                if (PlayTypeID == PlayType_RX2)
                {
                    return ToElectronicTicket_ZZYTC_RX2(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                if (PlayTypeID == PlayType_RX3)
                {
                    return ToElectronicTicket_ZZYTC_RX3(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                if (PlayTypeID == PlayType_RX4)
                {
                    return ToElectronicTicket_ZZYTC_RX4(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                if (PlayTypeID == PlayType_RX5)
                {
                    return ToElectronicTicket_ZZYTC_RX5(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                if (PlayTypeID == PlayType_RX6)
                {
                    return ToElectronicTicket_ZZYTC_RX6(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                if (PlayTypeID == PlayType_RX7)
                {
                    return ToElectronicTicket_ZZYTC_RX7(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                if (PlayTypeID == PlayType_RX8)
                {
                    return ToElectronicTicket_ZZYTC_RX8(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                if (PlayTypeID == PlayType_ZhiQ2)
                {
                    return ToElectronicTicket_ZZYTC_ZhiQ2(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                if (PlayTypeID == PlayType_ZhiQ3)
                {
                    return ToElectronicTicket_ZZYTC_ZhiQ3(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                if (PlayTypeID == PlayType_ZuQ2)
                {
                    return ToElectronicTicket_ZZYTC_ZuQ2(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                if (PlayTypeID == PlayType_ZuQ3)
                {
                    return ToElectronicTicket_ZZYTC_ZuQ3(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                return null;
            }
            #region ToElectronicTicket_ZZYTC 的具体方法
            private Ticket[] ToElectronicTicket_ZZYTC_RX1(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                string[] strs = AnalyseScheme_RX1(Number, PlayTypeID).Split('\n');

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
            private Ticket[] ToElectronicTicket_ZZYTC_RX2(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                string[] strs = AnalyseScheme_RX2(Number, PlayTypeID).Split('\n');

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

                    string Numbers = "";

                    EachMoney = 0;

                    string t_Numbers = "";
                    double t_EachMoney = 0;

                    int M = 0;

                    for (int i = 0; i < strs.Length; i++)
                    {
                        if (strs[i].ToString().Split('|').Length < 2)
                        {
                            continue;
                        }

                        if (double.Parse(strs[i].ToString().Split('|')[1]) == 1)
                        {
                            Numbers += strs[i].ToString().Split('|')[0] + "\n";
                            EachMoney += 2 * double.Parse(strs[i].ToString().Split('|')[1]);

                            M++;

                            if ((M == 5) || (i == strs.Length - 1))
                            {
                                Money += EachMoney * EachMultiple;

                                al.Add(new Ticket(0, ConvertFormatToElectronTicket_ZZYTC(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));

                                Numbers = "";
                                EachMoney = 0;

                                M = 0;
                            }
                        }
                        else
                        {
                            t_Numbers = strs[i].ToString().Split('|')[0] + "\n";
                            t_EachMoney = 2 * double.Parse(strs[i].ToString().Split('|')[1]);

                            Money += t_EachMoney * EachMultiple;

                            al.Add(new Ticket(1, ConvertFormatToElectronTicket_ZZYTC(PlayTypeID, t_Numbers), EachMultiple, t_EachMoney * EachMultiple));

                        }
                    }

                    if (Numbers != "")
                    {
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
            private Ticket[] ToElectronicTicket_ZZYTC_RX3(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                string[] strs = AnalyseScheme_RX3(Number, PlayTypeID).Split('\n');

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

                    string Numbers = "";

                    EachMoney = 0;

                    string t_Numbers = "";
                    double t_EachMoney = 0;

                    int M = 0;

                    for (int i = 0; i < strs.Length; i++)
                    {
                        if (strs[i].ToString().Split('|').Length < 2)
                        {
                            continue;
                        }

                        if (double.Parse(strs[i].ToString().Split('|')[1]) == 1)
                        {
                            Numbers += strs[i].ToString().Split('|')[0] + "\n";
                            EachMoney += 2 * double.Parse(strs[i].ToString().Split('|')[1]);

                            M++;

                            if ((M == 5) || (i == strs.Length - 1))
                            {
                                Money += EachMoney * EachMultiple;

                                al.Add(new Ticket(0, ConvertFormatToElectronTicket_ZZYTC(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));

                                Numbers = "";
                                EachMoney = 0;

                                M = 0;
                            }
                        }
                        else
                        {
                            t_Numbers = strs[i].ToString().Split('|')[0] + "\n";
                            t_EachMoney = 2 * double.Parse(strs[i].ToString().Split('|')[1]);

                            Money += t_EachMoney * EachMultiple;

                            al.Add(new Ticket(1, ConvertFormatToElectronTicket_ZZYTC(PlayTypeID, t_Numbers), EachMultiple, t_EachMoney * EachMultiple));

                        }
                    }

                    if (Numbers != "")
                    {
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
            private Ticket[] ToElectronicTicket_ZZYTC_RX4(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                string[] strs = AnalyseScheme_RX4(Number, PlayTypeID).Split('\n');

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

                    string Numbers = "";

                    EachMoney = 0;

                    string t_Numbers = "";
                    double t_EachMoney = 0;

                    int M = 0;

                    for (int i = 0; i < strs.Length; i++)
                    {
                        if (strs[i].ToString().Split('|').Length < 2)
                        {
                            continue;
                        }

                        if (double.Parse(strs[i].ToString().Split('|')[1]) == 1)
                        {
                            Numbers += strs[i].ToString().Split('|')[0] + "\n";
                            EachMoney += 2 * double.Parse(strs[i].ToString().Split('|')[1]);

                            M++;

                            if ((M == 5) || (i == strs.Length - 1))
                            {
                                Money += EachMoney * EachMultiple;

                                al.Add(new Ticket(0, ConvertFormatToElectronTicket_ZZYTC(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));

                                Numbers = "";
                                EachMoney = 0;

                                M = 0;
                            }
                        }
                        else
                        {
                            t_Numbers = strs[i].ToString().Split('|')[0] + "\n";
                            t_EachMoney = 2 * double.Parse(strs[i].ToString().Split('|')[1]);

                            Money += t_EachMoney * EachMultiple;

                            al.Add(new Ticket(1, ConvertFormatToElectronTicket_ZZYTC(PlayTypeID, t_Numbers), EachMultiple, t_EachMoney * EachMultiple));

                        }
                    }

                    if (Numbers != "")
                    {
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
            private Ticket[] ToElectronicTicket_ZZYTC_RX5(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                string[] strs = AnalyseScheme_RX5(Number, PlayTypeID).Split('\n');

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

                    string Numbers = "";

                    EachMoney = 0;

                    string t_Numbers = "";
                    double t_EachMoney = 0;

                    int M = 0;

                    for (int i = 0; i < strs.Length; i++)
                    {
                        if (strs[i].ToString().Split('|').Length < 2)
                        {
                            continue;
                        }

                        if (double.Parse(strs[i].ToString().Split('|')[1]) == 1)
                        {
                            Numbers += strs[i].ToString().Split('|')[0] + "\n";
                            EachMoney += 2 * double.Parse(strs[i].ToString().Split('|')[1]);

                            M++;

                            if ((M == 5) || (i == strs.Length - 1))
                            {
                                Money += EachMoney * EachMultiple;

                                al.Add(new Ticket(0, ConvertFormatToElectronTicket_ZZYTC(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));

                                Numbers = "";
                                EachMoney = 0;

                                M = 0;
                            }
                        }
                        else
                        {
                            t_Numbers = strs[i].ToString().Split('|')[0] + "\n";
                            t_EachMoney = 2 * double.Parse(strs[i].ToString().Split('|')[1]);

                            Money += t_EachMoney * EachMultiple;

                            al.Add(new Ticket(1, ConvertFormatToElectronTicket_ZZYTC(PlayTypeID, t_Numbers), EachMultiple, t_EachMoney * EachMultiple));

                        }
                    }

                    if (Numbers != "")
                    {
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
            private Ticket[] ToElectronicTicket_ZZYTC_RX6(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                string[] strs = AnalyseScheme_RX6(Number, PlayTypeID).Split('\n');

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

                    string Numbers = "";

                    EachMoney = 0;

                    string t_Numbers = "";
                    double t_EachMoney = 0;

                    int M = 0;

                    for (int i = 0; i < strs.Length; i++)
                    {
                        if (strs[i].ToString().Split('|').Length < 2)
                        {
                            continue;
                        }

                        if (double.Parse(strs[i].ToString().Split('|')[1]) == 1)
                        {
                            Numbers += strs[i].ToString().Split('|')[0] + "\n";
                            EachMoney += 2 * double.Parse(strs[i].ToString().Split('|')[1]);

                            M++;

                            if ((M == 5) || (i == strs.Length - 1))
                            {
                                Money += EachMoney * EachMultiple;

                                al.Add(new Ticket(0, ConvertFormatToElectronTicket_ZZYTC(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));

                                Numbers = "";
                                EachMoney = 0;

                                M = 0;
                            }
                        }
                        else
                        {
                            t_Numbers = strs[i].ToString().Split('|')[0] + "\n";
                            t_EachMoney = 2 * double.Parse(strs[i].ToString().Split('|')[1]);

                            Money += t_EachMoney * EachMultiple;

                            al.Add(new Ticket(1, ConvertFormatToElectronTicket_ZZYTC(PlayTypeID, t_Numbers), EachMultiple, t_EachMoney * EachMultiple));

                        }
                    }

                    if (Numbers != "")
                    {
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
            private Ticket[] ToElectronicTicket_ZZYTC_RX7(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                string[] strs = AnalyseScheme_RX7(Number, PlayTypeID).Split('\n');

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

                    string Numbers = "";

                    EachMoney = 0;

                    string t_Numbers = "";
                    double t_EachMoney = 0;

                    int M = 0;

                    for (int i = 0; i < strs.Length; i++)
                    {
                        if (strs[i].ToString().Split('|').Length < 2)
                        {
                            continue;
                        }

                        if (double.Parse(strs[i].ToString().Split('|')[1]) == 1)
                        {
                            Numbers += strs[i].ToString().Split('|')[0] + "\n";
                            EachMoney += 2 * double.Parse(strs[i].ToString().Split('|')[1]);

                            M++;

                            if ((M == 5) || (i == strs.Length - 1))
                            {
                                Money += EachMoney * EachMultiple;

                                al.Add(new Ticket(0, ConvertFormatToElectronTicket_ZZYTC(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));

                                Numbers = "";
                                EachMoney = 0;

                                M = 0;
                            }
                        }
                        else
                        {
                            t_Numbers = strs[i].ToString().Split('|')[0] + "\n";
                            t_EachMoney = 2 * double.Parse(strs[i].ToString().Split('|')[1]);

                            Money += t_EachMoney * EachMultiple;

                            al.Add(new Ticket(1, ConvertFormatToElectronTicket_ZZYTC(PlayTypeID, t_Numbers), EachMultiple, t_EachMoney * EachMultiple));

                        }
                    }

                    if (Numbers != "")
                    {
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
            private Ticket[] ToElectronicTicket_ZZYTC_RX8(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                string[] strs = AnalyseScheme_RX8(Number, PlayTypeID).Split('\n');

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

                    string Numbers = "";

                    EachMoney = 0;

                    string t_Numbers = "";
                    double t_EachMoney = 0;

                    int M = 0;

                    for (int i = 0; i < strs.Length; i++)
                    {
                        if (strs[i].ToString().Split('|').Length < 2)
                        {
                            continue;
                        }

                        if (double.Parse(strs[i].ToString().Split('|')[1]) == 1)
                        {
                            Numbers += strs[i].ToString().Split('|')[0] + "\n";
                            EachMoney += 2 * double.Parse(strs[i].ToString().Split('|')[1]);

                            M++;

                            if ((M == 5) || (i == strs.Length - 1))
                            {
                                Money += EachMoney * EachMultiple;

                                al.Add(new Ticket(0, ConvertFormatToElectronTicket_ZZYTC(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));

                                Numbers = "";
                                EachMoney = 0;

                                M = 0;
                            }
                        }
                        else
                        {
                            t_Numbers = strs[i].ToString().Split('|')[0] + "\n";
                            t_EachMoney = 2 * double.Parse(strs[i].ToString().Split('|')[1]);

                            Money += t_EachMoney * EachMultiple;

                            al.Add(new Ticket(1, ConvertFormatToElectronTicket_ZZYTC(PlayTypeID, t_Numbers), EachMultiple, t_EachMoney * EachMultiple));

                        }
                    }

                    if (Numbers != "")
                    {
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
            private Ticket[] ToElectronicTicket_ZZYTC_ZhiQ2(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                string[] strs = AnalyseScheme_ZhiQ2(Number, PlayTypeID).Split('\n');

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

                    string Numbers = "";

                    EachMoney = 0;

                    string t_Numbers = "";
                    double t_EachMoney = 0;

                    int M = 0;

                    for (int i = 0; i < strs.Length; i++)
                    {
                        if (string.IsNullOrEmpty(strs[i].ToString()))
                        {
                            continue;
                        }

                        if (strs[i].ToString().Split('|').Length < 3)
                        {
                            continue;
                        }

                        if (double.Parse(strs[i].ToString().Split('|')[2]) == 1)
                        {
                            Numbers += strs[i].ToString().Split('|')[0] + "|" + strs[i].ToString().Split('|')[1] + "\n";
                            EachMoney += 2 * double.Parse(strs[i].ToString().Split('|')[2]);

                            M++;

                            if ((M == 5) || (i == strs.Length - 1))
                            {
                                Money += EachMoney * EachMultiple;

                                al.Add(new Ticket(0, ConvertFormatToElectronTicket_ZZYTC(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));

                                Numbers = "";
                                EachMoney = 0;

                                M = 0;
                            }
                        }
                        else
                        {
                            t_Numbers = strs[i].ToString().Split('|')[0] + "|" + strs[i].ToString().Split('|')[1] + "\n";
                            t_EachMoney = 2 * double.Parse(strs[i].ToString().Split('|')[2]);

                            Money += t_EachMoney * EachMultiple;

                            al.Add(new Ticket(1, ConvertFormatToElectronTicket_ZZYTC(PlayTypeID, t_Numbers.Replace(" ", ",")), EachMultiple, t_EachMoney * EachMultiple));

                        }
                    }

                    if (Numbers != "")
                    {
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
            private Ticket[] ToElectronicTicket_ZZYTC_ZhiQ3(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                string[] strs = AnalyseScheme_ZhiQ3(Number, PlayTypeID).Split('\n');

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

                string t_Numbers = "";
                double t_EachMoney = 0;

                int M = 0;

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

                    string Numbers = "";

                    EachMoney = 0;

                    for (int i = 0; i < strs.Length; i++)
                    {
                        if (string.IsNullOrEmpty(strs[i].ToString()))
                        {
                            continue;
                        }

                        if (strs[i].ToString().Split('|').Length < 4)
                        {
                            continue;
                        }

                        if (double.Parse(strs[i].ToString().Split('|')[3]) == 1)
                        {
                            Numbers += strs[i].ToString().Split('|')[0] + "|" + strs[i].ToString().Split('|')[1] + "|" + strs[i].ToString().Split('|')[2] + "\n";
                            EachMoney += 2 * double.Parse(strs[i].ToString().Split('|')[3]);

                            M++;

                            if ((M == 5) || (i == strs.Length - 1))
                            {
                                Money += EachMoney * EachMultiple;

                                al.Add(new Ticket(0, ConvertFormatToElectronTicket_ZZYTC(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));

                                Numbers = "";
                                EachMoney = 0;

                                M = 0;
                            }
                        }
                        else
                        {
                            t_Numbers = strs[i].ToString().Split('|')[0] + "|" + strs[i].ToString().Split('|')[1] + "|" + strs[i].ToString().Split('|')[2] + "\n";
                            t_EachMoney = 2 * double.Parse(strs[i].ToString().Split('|')[3]);

                            Money += t_EachMoney * EachMultiple;

                            al.Add(new Ticket(1, ConvertFormatToElectronTicket_ZZYTC(PlayTypeID, t_Numbers.Replace(" ", ",")), EachMultiple, t_EachMoney * EachMultiple));

                        }
                    }

                    if (Numbers != "")
                    {
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
            private Ticket[] ToElectronicTicket_ZZYTC_ZuQ2(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                string[] strs = AnalyseScheme_ZuQ2(Number, PlayTypeID).Split('\n');

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

                    string Numbers = "";

                    EachMoney = 0;

                    string t_Numbers = "";
                    double t_EachMoney = 0;

                    int M = 0;

                    for (int i = 0; i < strs.Length; i++)
                    {
                        if (strs[i].ToString().Split('|').Length < 2)
                        {
                            continue;
                        }

                        if (double.Parse(strs[i].ToString().Split('|')[1]) == 1)
                        {
                            Numbers += strs[i].ToString().Split('|')[0] + "\n";
                            EachMoney += 2 * double.Parse(strs[i].ToString().Split('|')[1]);

                            M++;

                            if ((M == 5) || (i == strs.Length - 1))
                            {
                                Money += EachMoney * EachMultiple;

                                al.Add(new Ticket(0, ConvertFormatToElectronTicket_ZZYTC(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));

                                Numbers = "";
                                EachMoney = 0;

                                M = 0;
                            }
                        }
                        else
                        {
                            t_Numbers = strs[i].ToString().Split('|')[0] + "\n";
                            t_EachMoney = 2 * double.Parse(strs[i].ToString().Split('|')[1]);

                            Money += t_EachMoney * EachMultiple;

                            al.Add(new Ticket(1, ConvertFormatToElectronTicket_ZZYTC(PlayTypeID, t_Numbers), EachMultiple, t_EachMoney * EachMultiple));

                        }
                    }

                    if (Numbers != "")
                    {
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
            private Ticket[] ToElectronicTicket_ZZYTC_ZuQ3(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                string[] strs = AnalyseScheme_ZuQ3(Number, PlayTypeID).Split('\n');

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

                    string Numbers = "";

                    EachMoney = 0;

                    string t_Numbers = "";
                    double t_EachMoney = 0;

                    int M = 0;

                    for (int i = 0; i < strs.Length; i++)
                    {
                        if (strs[i].ToString().Split('|').Length < 2)
                        {
                            continue;
                        }

                        if (double.Parse(strs[i].ToString().Split('|')[1]) == 1)
                        {
                            Numbers += strs[i].ToString().Split('|')[0] + "\n";
                            EachMoney += 2 * double.Parse(strs[i].ToString().Split('|')[1]);

                            M++;

                            if ((M == 5) || (i == strs.Length - 1))
                            {
                                Money += EachMoney * EachMultiple;

                                al.Add(new Ticket(0, ConvertFormatToElectronTicket_ZZYTC(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));

                                Numbers = "";
                                EachMoney = 0;

                                M = 0;
                            }
                        }
                        else
                        {
                            t_Numbers = strs[i].ToString().Split('|')[0] + "\n";
                            t_EachMoney = 2 * double.Parse(strs[i].ToString().Split('|')[1]);

                            Money += t_EachMoney * EachMultiple;

                            al.Add(new Ticket(1, ConvertFormatToElectronTicket_ZZYTC(PlayTypeID, t_Numbers), EachMultiple, t_EachMoney * EachMultiple));

                        }
                    }

                    if (Numbers != "")
                    {
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

            // 转换为需要的彩票格式
            private string ConvertFormatToElectronTicket_ZZYTC(int PlayTypeID, string Number)
            {
                Number = Number.Trim();

                string Ticket = "";

                if (PlayTypeID == PlayType_RX1 || PlayTypeID == PlayType_RX2 || PlayTypeID == PlayType_RX3 || PlayTypeID == PlayType_RX4 || PlayTypeID == PlayType_RX5 || PlayTypeID == PlayType_RX6 || PlayTypeID == PlayType_RX7 || PlayTypeID == PlayType_RX8)
                {
                    Ticket = Number.Replace(" ", "");
                }

                if ((PlayTypeID == PlayType_ZhiQ2) || (PlayTypeID == PlayType_ZhiQ3))
                {
                    Ticket = Number.Replace(" ", "").Replace("|", "");
                }

                if ((PlayTypeID == PlayType_ZuQ2) || (PlayTypeID == PlayType_ZuQ3))
                {
                    Ticket = Number.Replace(" ", "").Replace("|", "");
                }

                return Ticket;
            }
            #endregion
        }
    }
}