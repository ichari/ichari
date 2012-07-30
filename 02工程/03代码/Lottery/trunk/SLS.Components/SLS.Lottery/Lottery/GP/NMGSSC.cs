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
        /// 江西时时彩
        /// </summary>
        public partial class NMGSSC : LotteryBase
        {
            #region 静态变量

            public const int PlayType_D = 7901;  //常规买法,注意，5星复式表示含5星，4星，3星，2星，1星共4注，复式的概念和传统彩票不同
            public const int PlayType_F = 7902;

            public const int PlayType_ZH = 7903;  //组合玩法，和传统彩票复式概念一样

            public const int PlayType_DX = 7904;  //猜大小

            public const int PlayType_5X_TXD = 7905; //五星通选单式
            public const int PlayType_5X_TXF = 7906; //五星通选复式

            public const int PlayType_2X_ZuD = 7907; //二星组选单式
            public const int PlayType_2X_ZuF = 7908; //二星组选复式
            public const int PlayType_2X_ZuB = 7909; // 二星组选包点
            public const int PlayType_2X_ZuBD = 7910; // 二星组选包胆

            public const int ID = 79;
            public const string sID = "79";
            public const string Name = "内蒙古时时彩";
            public const string Code = "NMGSSC";
            public const double MaxMoney = 200000;
            #endregion

            public NMGSSC()
            {
                id = 79;
                name = "内蒙古时时彩";
                code = "NMGSSC";
            }

            public override bool CheckPlayType(int play_type)
            {
                return ((play_type >= 7901) && (play_type <= 7910));
            }

            public override PlayType[] GetPlayTypeList()
            {
                PlayType[] Result = new PlayType[10];

                Result[0] = new PlayType(PlayType_D, "单式");
                Result[1] = new PlayType(PlayType_F, "复式");
                Result[2] = new PlayType(PlayType_ZH, "组合玩法");
                Result[3] = new PlayType(PlayType_DX, "猜大小");
                Result[4] = new PlayType(PlayType_5X_TXD, "五星通选单式");
                Result[5] = new PlayType(PlayType_5X_TXF, "五星通选复式");
                Result[6] = new PlayType(PlayType_2X_ZuD, "二星组选单式");
                Result[7] = new PlayType(PlayType_2X_ZuF, "二星组选复式");
                Result[8] = new PlayType(PlayType_2X_ZuB, "二星组选包点");
                Result[9] = new PlayType(PlayType_2X_ZuBD, "二星组选包胆");

                return Result;
            }

            public override string BuildNumber(int Num, int Type)       //Type: 5 = 5星, 4 = 4星, 3 = 3星, 2 = 2星, 1 = 1星, -1 = 大小单双
            {
                if ((Type != 5) && (Type != 4) && (Type != 3) && (Type != 2) && (Type != 1) && (Type != -1))
                    Type = 5;

                if (Type == -1) //大小单双
                {
                    return BuildNumber_DX(Num);
                }

                return BuildNumber_54321(Num, Type);
            }
            #region BuilderNumber 的具体方法
            private string BuildNumber_54321(int Num, int Type)
            {
                System.Random rd = new Random();
                StringBuilder sb = new StringBuilder();

                for (int i = 0; i < Num; i++)
                {
                    string LotteryNumber = "";

                    for (int j = Type; j < 5; j++)
                        LotteryNumber += "-";

                    for (int j = 0; j < Type; j++)
                        LotteryNumber += rd.Next(0, 9 + 1).ToString();

                    sb.Append(LotteryNumber.Trim() + "\n");
                }

                string Result = sb.ToString();
                Result = Result.Substring(0, Result.Length - 1);

                return Result;

            }
            private string BuildNumber_DX(int Num)   //大小单双
            {
                System.Random rd = new Random();
                StringBuilder sb = new StringBuilder();

                for (int i = 0; i < Num; i++)
                {
                    string LotteryNumber = "";

                    for (int j = 0; j < 2; j++)
                        LotteryNumber += "大小单双".Substring(rd.Next(0, 3 + 1), 1);

                    sb.Append(LotteryNumber.Trim() + "\n");
                }

                string Result = sb.ToString();
                Result = Result.Substring(0, Result.Length - 1);

                return Result;
            }
            #endregion

            public override string[] ToSingle(string Number, ref string CanonicalNumber, int PlayType)   //复式取单式, 后面 ref 参数是将彩票规范化，如：103(00)... 变成1030...
            {
                if (PlayType == PlayType_D)
                    return ToSingle_D(Number, ref CanonicalNumber);

                if (PlayType == PlayType_F)
                    return ToSingle_F(Number, ref CanonicalNumber);

                if (PlayType == PlayType_ZH)
                    return ToSingle_ZH(Number, ref CanonicalNumber);

                if (PlayType == PlayType_DX)
                    return ToSingle_DX(Number, ref CanonicalNumber);

                if ((PlayType == PlayType_5X_TXD) || (PlayType == PlayType_5X_TXF))
                    return ToSingle_5X_TX(Number, ref CanonicalNumber);

                if (PlayType == PlayType_2X_ZuD)
                    return ToSingle_2X_ZuD(Number, ref CanonicalNumber);

                if (PlayType == PlayType_2X_ZuF)
                    return ToSingle_2X_ZuF(Number, ref CanonicalNumber);

                if (PlayType == PlayType_2X_ZuB)
                    return ToSingle_2X_ZuB(Number, ref CanonicalNumber);

                if (PlayType == PlayType_2X_ZuBD)
                    return ToSingle_2X_ZuBD(Number, ref CanonicalNumber);

                return null;
            }
            #region ToSingle 的具体方法
            private string[] ToSingle_D(string Number, ref string CanonicalNumber)
            {
                CanonicalNumber = "";

                Regex regex = new Regex(@"^(?<L0>(\d)|(-))(?<L1>(\d)|(-))(?<L2>(\d)|(-))(?<L3>(\d)|(-))(?<L4>(\d))", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                Match m = regex.Match(Number);
                for (int i = 0; i < 5; i++)
                {
                    string Locate = m.Groups["L" + i.ToString()].ToString().Trim();
                    if (Locate == "")
                    {
                        CanonicalNumber = "";
                        return null;
                    }

                    CanonicalNumber += Locate;
                }

                Regex[] regex54321 = new Regex[5];
                regex54321[0] = new Regex(@"^(\d){5}", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                regex54321[1] = new Regex(@"^-(\d){4}", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                regex54321[2] = new Regex(@"^--(\d){3}", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                regex54321[3] = new Regex(@"^---(\d){2}", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                regex54321[4] = new Regex(@"^----\d", RegexOptions.IgnoreCase | RegexOptions.Compiled);

                bool isMatch = false;
                for (int i = 0; i < 5; i++)
                {
                    if (regex54321[i].IsMatch(CanonicalNumber))
                    {
                        isMatch = true;
                        break;
                    }
                }

                if (!isMatch)
                    return null;

                string[] Result = new string[1];
                Result[0] = CanonicalNumber;

                return Result;
            }
            private string[] ToSingle_F(string Number, ref string CanonicalNumber)
            {
                string[] t_strs = ToSingle_D(Number, ref CanonicalNumber);

                if ((t_strs == null) || (t_strs.Length != 1))
                    return null;

                Regex[] regex54321 = new Regex[5];
                regex54321[0] = new Regex(@"^(\d){5}", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                regex54321[1] = new Regex(@"^-(\d){4}", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                regex54321[2] = new Regex(@"^--(\d){3}", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                regex54321[3] = new Regex(@"^---(\d){2}", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                regex54321[4] = new Regex(@"^----\d", RegexOptions.IgnoreCase | RegexOptions.Compiled);

                string[] Result = null;

                if (regex54321[0].IsMatch(CanonicalNumber))
                {
                    Result = new string[5];

                    Result[0] = CanonicalNumber;
                    Result[1] = "-" + CanonicalNumber.Substring(1, 4);
                    Result[2] = "--" + CanonicalNumber.Substring(2, 3);
                    Result[3] = "---" + CanonicalNumber.Substring(3, 2);
                    Result[4] = "----" + CanonicalNumber.Substring(4, 1);
                }

                else if (regex54321[1].IsMatch(CanonicalNumber))
                {
                    Result = new string[4];

                    Result[0] = CanonicalNumber;
                    Result[1] = "--" + CanonicalNumber.Substring(2, 3);
                    Result[2] = "---" + CanonicalNumber.Substring(3, 2);
                    Result[3] = "----" + CanonicalNumber.Substring(4, 1);
                }

                else if (regex54321[2].IsMatch(CanonicalNumber))
                {
                    Result = new string[3];

                    Result[0] = CanonicalNumber;
                    Result[1] = "---" + CanonicalNumber.Substring(3, 2);
                    Result[2] = "----" + CanonicalNumber.Substring(4, 1);
                }

                else if (regex54321[3].IsMatch(CanonicalNumber))
                {
                    Result = new string[2];

                    Result[0] = CanonicalNumber;
                    Result[1] = "----" + CanonicalNumber.Substring(4, 1);
                }

                else if (regex54321[4].IsMatch(CanonicalNumber))
                {
                    Result = new string[1];

                    Result[0] = CanonicalNumber;
                }

                return Result;
            }
            private string[] ToSingle_ZH(string Number, ref string CanonicalNumber)
            {
                string[] Locate = new string[5];
                CanonicalNumber = "";

                Regex regex = new Regex(@"^(?<L0>([\d-])|([(][\d]+?[)]))(?<L1>([\d-])|([(][\d]+?[)]))(?<L2>([\d-])|([(][\d]+?[)]))(?<L3>([\d-])|([(][\d]+?[)]))(?<L4>(\d)|([(][\d]+?[)]))", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                Match m = regex.Match(Number);
                for (int i = 0; i < 5; i++)
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

                                    string str_4_Canonical = "";
                                    string[] strs_4 = ToSingle_D(str_4, ref str_4_Canonical);

                                    if ((strs_4 == null) || (strs_4.Length < 1))
                                        continue;

                                    al.Add(str_4);
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
            private string[] ToSingle_DX(string Number, ref string CanonicalNumber)	//猜大小
            {
                CanonicalNumber = "";

                Regex regex = new Regex(@"^(?<L0>([大小单双]))(?<L1>([大小单双]))", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                Match m = regex.Match(Number);
                for (int i = 0; i < 2; i++)
                {
                    string Locate = m.Groups["L" + i.ToString()].ToString().Trim();
                    if (Locate == "")
                    {
                        CanonicalNumber = "";
                        return null;
                    }

                    CanonicalNumber += Locate;
                }

                string[] Result = new string[1];
                Result[0] = CanonicalNumber;

                return Result;
            }
            private string[] ToSingle_5X_TX(string Number, ref string CanonicalNumber)  // 五星通选 复式取单式, 后面 ref 参数是将彩票规范化，如：10(223)45 变成10(23)45
            {
                string[] Locate = new string[5];
                CanonicalNumber = "";

                Regex regex = new Regex(@"^(?<L0>(\d)|([(][\d]+?[)]))(?<L1>(\d)|([(][\d]+?[)]))(?<L2>(\d)|([(][\d]+?[)]))(?<L3>(\d)|([(][\d]+?[)]))(?<L4>(\d)|([(][\d]+?[)]))", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                Match m = regex.Match(Number);
                for (int i = 0; i < 5; i++)
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
                                    al.Add(str_4);
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
            private string[] ToSingle_2X_ZuD(string Number, ref string CanonicalNumber)//二星组选 单式
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
                {
                    for (int j = i + 1; j < n; j++)
                    {
                        al.Add(strs[i].ToString() + strs[j].ToString());
                    }
                }

                #endregion

                string[] Result = new string[al.Count];
                for (int i = 0; i < al.Count; i++)
                    Result[i] = al[i].ToString();
                return Result;
            }
            private string[] ToSingle_2X_ZuF(string Number, ref string CanonicalNumber)//二星组选 复式
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
                for (int i = 0; i < n; i++)
                {
                    for (int j = i; j < n; j++)
                    {
                        al.Add(strs[i].ToString() + strs[j].ToString());
                    }
                }

                #endregion

                string[] Result = new string[al.Count];
                for (int i = 0; i < al.Count; i++)
                    Result[i] = al[i].ToString();
                return Result;
            }
            private string[] ToSingle_2X_ZuB(string sBill, ref string CanonicalNumber)//二星组选包点
            {
                int Bill = Shove._Convert.StrToInt(sBill, -1);
                CanonicalNumber = "";

                ArrayList al = new ArrayList();

                #region 循环取单式

                if ((Bill < 0) || (Bill > 18))
                {
                    CanonicalNumber = "";
                    return null;
                }
                else
                {
                    for (int i = 0; i <= 9; i++)
                    {
                        for (int j = i; j <= 9; j++)
                        {
                            if (i + j == Bill)
                                al.Add(i.ToString() + j.ToString());
                        }
                    }
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
            private string[] ToSingle_2X_ZuBD(string sBill, ref string CanonicalNumber)//二星组选包胆
            {
                CanonicalNumber = FilterRepeated(sBill.Trim());

                if (CanonicalNumber.Length < 1)
                {
                    CanonicalNumber = "";
                    return null;
                }

                if (CanonicalNumber.Length > 2)
                {
                    CanonicalNumber = CanonicalNumber.Substring(0, 2);
                }

                char[] strs = CanonicalNumber.ToCharArray();

                ArrayList al = new ArrayList();

                #region 循环取单式

                int n = strs.Length;
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j <= 9; j++)
                    {
                        al.Add(strs[i].ToString() + j.ToString());
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
                Description = "";
                WinMoneyNoWithTax = 0;

                if ((WinMoneyList == null) || (WinMoneyList.Length < 22)) //奖金参数排列顺序(5 4 3 2 1星，猜大小,二星组选, 五星选123等奖
                    return -3;

                int WinCount5X = 0, WinCount4X = 0, WinCount3X = 0, WinCount2X = 0, WinCount1X = 0;
                int WinCountDX = 0;
                int WinCount_2X_Zu = 0, WinCount_2X_Zu_DZH = 0;
                int WinCount_5XTX_1 = 0, WinCount_5XTX_2 = 0, WinCount_5XTX_3 = 0;

                if (PlayType == PlayType_D)
                    return ComputeWin_D(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[0], WinMoneyList[1], WinMoneyList[2], WinMoneyList[3], WinMoneyList[4], WinMoneyList[5], WinMoneyList[6], WinMoneyList[7], WinMoneyList[8], WinMoneyList[9], ref WinCount5X, ref WinCount4X, ref WinCount3X, ref WinCount2X, ref WinCount1X);

                if (PlayType == PlayType_F)
                    return ComputeWin_F(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[0], WinMoneyList[1], WinMoneyList[2], WinMoneyList[3], WinMoneyList[4], WinMoneyList[5], WinMoneyList[6], WinMoneyList[7], WinMoneyList[8], WinMoneyList[9], ref WinCount5X, ref WinCount4X, ref WinCount3X, ref WinCount2X, ref WinCount1X);

                if (PlayType == PlayType_ZH)
                    return ComputeWin_ZH(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[0], WinMoneyList[1], WinMoneyList[2], WinMoneyList[3], WinMoneyList[4], WinMoneyList[5], WinMoneyList[6], WinMoneyList[7], WinMoneyList[8], WinMoneyList[9], ref WinCount5X, ref WinCount4X, ref WinCount3X, ref WinCount2X, ref WinCount1X);

                if (PlayType == PlayType_DX)
                    return ComputeWin_DX(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[10], WinMoneyList[11], ref WinCountDX);

                if ((PlayType == PlayType_5X_TXD) || (PlayType == PlayType_5X_TXF))
                    return ComputeWin_5X_TX(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[16], WinMoneyList[17], WinMoneyList[18], WinMoneyList[19], WinMoneyList[20], WinMoneyList[21], ref WinCount_5XTX_1, ref WinCount_5XTX_2, ref WinCount_5XTX_3);

                if (PlayType == PlayType_2X_ZuD)
                    return ComputeWin_2X_ZuD(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[12], WinMoneyList[13], ref WinCount_2X_Zu);

                if (PlayType == PlayType_2X_ZuF)
                    return ComputeWin_2X_ZuF(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[12], WinMoneyList[13], WinMoneyList[14], WinMoneyList[15], ref WinCount_2X_Zu, ref WinCount_2X_Zu_DZH);

                if (PlayType == PlayType_2X_ZuB)
                    return ComputeWin_2X_ZuB(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[12], WinMoneyList[13], WinMoneyList[14], WinMoneyList[15], ref WinCount_2X_Zu, ref WinCount_2X_Zu_DZH);

                if (PlayType == PlayType_2X_ZuBD)
                    return ComputeWin_2X_ZuBD(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[12], WinMoneyList[13], WinMoneyList[14], WinMoneyList[15], ref WinCount_2X_Zu, ref WinCount_2X_Zu_DZH);

                return -4;
            }
            #region ComputeWin  的具体方法
            private double ComputeWin_D(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney1, double WinMoneyNoWithTax1, double WinMoney2, double WinMoneyNoWithTax2, double WinMoney3, double WinMoneyNoWithTax3, double WinMoney4, double WinMoneyNoWithTax4, double WinMoney5, double WinMoneyNoWithTax5, ref int WinCount5X, ref int WinCount4X, ref int WinCount3X, ref int WinCount2X, ref int WinCount1X)
            {
                WinCount5X = 0;
                WinCount4X = 0;
                WinCount3X = 0;
                WinCount2X = 0;
                WinCount1X = 0;

                WinNumber = WinNumber.Trim();
                if (WinNumber.Length < 5)	//3: "12345"
                    return -1;
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
                    string[] Lottery = ToSingle_D(Lotterys[ii], ref t_str);
                    if (Lottery == null)
                        continue;
                    if (Lottery.Length < 1)
                        continue;

                    Regex[] regex54321 = new Regex[5];
                    regex54321[0] = new Regex(@"^(\d){5}", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                    regex54321[1] = new Regex(@"^-(\d){4}", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                    regex54321[2] = new Regex(@"^--(\d){3}", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                    regex54321[3] = new Regex(@"^---(\d){2}", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                    regex54321[4] = new Regex(@"^----\d", RegexOptions.IgnoreCase | RegexOptions.Compiled);

                    for (int i = 0; i < Lottery.Length; i++)
                    {
                        if (regex54321[0].IsMatch(Lottery[i]))
                        {
                            if (Lottery[i] == WinNumber)
                            {
                                WinCount5X++;
                                WinMoney += WinMoney1;
                                WinMoneyNoWithTax += WinMoneyNoWithTax1;

                                continue;
                            }
                        }

                        if (regex54321[1].IsMatch(Lottery[i]))
                        {
                            if (Lottery[i].Substring(1, 4) == WinNumber.Substring(1, 4))
                            {
                                WinCount4X++;
                                WinMoney += WinMoney2;
                                WinMoneyNoWithTax += WinMoneyNoWithTax2;

                                continue;
                            }
                        }

                        if (regex54321[2].IsMatch(Lottery[i]))
                        {
                            if (Lottery[i].Substring(2, 3) == WinNumber.Substring(2, 3))
                            {
                                WinCount3X++;
                                WinMoney += WinMoney3;
                                WinMoneyNoWithTax += WinMoneyNoWithTax3;

                                continue;
                            }
                        }

                        if (regex54321[3].IsMatch(Lottery[i]))
                        {
                            if (Lottery[i].Substring(3, 2) == WinNumber.Substring(3, 2))
                            {
                                WinCount2X++;
                                WinMoney += WinMoney4;
                                WinMoneyNoWithTax += WinMoneyNoWithTax4;

                                continue;
                            }
                        }

                        if (regex54321[4].IsMatch(Lottery[i]))
                        {
                            if (Lottery[i].Substring(4, 1) == WinNumber.Substring(4, 1))
                            {
                                WinCount1X++;
                                WinMoney += WinMoney5;
                                WinMoneyNoWithTax += WinMoneyNoWithTax5;

                                continue;
                            }
                        }
                    }
                }

                Description = "";

                if (WinCount5X > 0)
                {
                    MergeWinDescription(ref Description, "五星奖" + WinCount5X.ToString() + "注");
                }

                if (WinCount4X > 0)
                {
                    MergeWinDescription(ref Description, "四星奖" + WinCount4X.ToString() + "注");
                }

                if (WinCount3X > 0)
                {
                    MergeWinDescription(ref Description, "三星奖" + WinCount3X.ToString() + "注");
                }

                if (WinCount2X > 0)
                {
                    MergeWinDescription(ref Description, "二星奖" + WinCount2X.ToString() + "注");
                }

                if (WinCount1X > 0)
                {
                    MergeWinDescription(ref Description, "一星奖" + WinCount1X.ToString() + "注");
                }

                if (Description != "")
                    Description += "。";

                return WinMoney;
            }
            private double ComputeWin_F(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney1, double WinMoneyNoWithTax1, double WinMoney2, double WinMoneyNoWithTax2, double WinMoney3, double WinMoneyNoWithTax3, double WinMoney4, double WinMoneyNoWithTax4, double WinMoney5, double WinMoneyNoWithTax5, ref int WinCount5X, ref int WinCount4X, ref int WinCount3X, ref int WinCount2X, ref int WinCount1X)
            {
                WinCount5X = 0;
                WinCount4X = 0;
                WinCount3X = 0;
                WinCount2X = 0;
                WinCount1X = 0;

                WinNumber = WinNumber.Trim();
                if (WinNumber.Length < 5)	//3: "12345"
                    return -1;
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
                    string[] Lottery = ToSingle_F(Lotterys[ii], ref t_str);
                    if (Lottery == null)
                        continue;
                    if (Lottery.Length < 1)
                        continue;

                    Regex[] regex54321 = new Regex[5];
                    regex54321[0] = new Regex(@"^(\d){5}", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                    regex54321[1] = new Regex(@"^-(\d){4}", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                    regex54321[2] = new Regex(@"^--(\d){3}", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                    regex54321[3] = new Regex(@"^---(\d){2}", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                    regex54321[4] = new Regex(@"^----\d", RegexOptions.IgnoreCase | RegexOptions.Compiled);

                    for (int i = 0; i < Lottery.Length; i++)
                    {
                        if (regex54321[0].IsMatch(Lottery[i]))
                        {
                            if (Lottery[i] == WinNumber)
                            {
                                WinCount5X++;
                                WinMoney += WinMoney1;
                                WinMoneyNoWithTax += WinMoneyNoWithTax1;

                                continue;
                            }
                        }

                        if (regex54321[1].IsMatch(Lottery[i]))
                        {
                            if (Lottery[i].Substring(1, 4) == WinNumber.Substring(1, 4))
                            {
                                WinCount4X++;
                                WinMoney += WinMoney2;
                                WinMoneyNoWithTax += WinMoneyNoWithTax2;

                                continue;
                            }
                        }

                        if (regex54321[2].IsMatch(Lottery[i]))
                        {
                            if (Lottery[i].Substring(2, 3) == WinNumber.Substring(2, 3))
                            {
                                WinCount3X++;
                                WinMoney += WinMoney3;
                                WinMoneyNoWithTax += WinMoneyNoWithTax3;

                                continue;
                            }
                        }

                        if (regex54321[3].IsMatch(Lottery[i]))
                        {
                            if (Lottery[i].Substring(3, 2) == WinNumber.Substring(3, 2))
                            {
                                WinCount2X++;
                                WinMoney += WinMoney4;
                                WinMoneyNoWithTax += WinMoneyNoWithTax4;

                                continue;
                            }
                        }

                        if (regex54321[4].IsMatch(Lottery[i]))
                        {
                            if (Lottery[i].Substring(4, 1) == WinNumber.Substring(4, 1))
                            {
                                WinCount1X++;
                                WinMoney += WinMoney5;
                                WinMoneyNoWithTax += WinMoneyNoWithTax5;

                                continue;
                            }
                        }
                    }
                }

                Description = "";

                if (WinCount5X > 0)
                {
                    MergeWinDescription(ref Description, "五星奖" + WinCount5X.ToString() + "注");
                }

                if (WinCount4X > 0)
                {
                    MergeWinDescription(ref Description, "四星奖" + WinCount5X.ToString() + "注");
                }

                if (WinCount3X > 0)
                {
                    MergeWinDescription(ref Description, "三星奖" + WinCount3X.ToString() + "注");
                }

                if (WinCount2X > 0)
                {
                    MergeWinDescription(ref Description, "二星奖" + WinCount2X.ToString() + "注");
                }

                if (WinCount1X > 0)
                {
                    MergeWinDescription(ref Description, "一星奖" + WinCount1X.ToString() + "注");
                }

                if (Description != "")
                    Description += "。";

                return WinMoney;
            }
            private double ComputeWin_ZH(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney1, double WinMoneyNoWithTax1, double WinMoney2, double WinMoneyNoWithTax2, double WinMoney3, double WinMoneyNoWithTax3, double WinMoney4, double WinMoneyNoWithTax4, double WinMoney5, double WinMoneyNoWithTax5, ref int WinCount5X, ref int WinCount4X, ref int WinCount3X, ref int WinCount2X, ref int WinCount1X)
            {
                WinCount5X = 0;
                WinCount4X = 0;
                WinCount3X = 0;
                WinCount2X = 0;
                WinCount1X = 0;

                WinNumber = WinNumber.Trim();
                if (WinNumber.Length < 5)	//3: "12345"
                    return -1;
                string[] Lotterys = SplitLotteryNumber(Number);
                if (Lotterys == null)
                    return -2;
                if (Lotterys.Length < 1)
                    return -2;

                double WinMoney = 0;
                WinMoneyNoWithTax = 0;

                string[] Locate = new string[5];
                bool[] IsRight = new bool[5];
                int[] Num = new int[5];

                Regex regex = new Regex(@"^(?<L0>([\d-])|([(][\d]+?[)]))(?<L1>([\d-])|([(][\d]+?[)]))(?<L2>([\d-])|([(][\d]+?[)]))(?<L3>([\d-])|([(][\d]+?[)]))(?<L4>(\d)|([(][\d]+?[)]))", RegexOptions.IgnoreCase | RegexOptions.Compiled);

                for (int ii = 0; ii < Lotterys.Length; ii++)
                {
                    Match m = regex.Match(Lotterys[ii]);

                    for (int j = 0; j < 5; j++)
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

                    if (IsRight[4] && IsRight[3] && IsRight[2] && IsRight[1] && IsRight[0])
                    {
                        WinCount5X++;
                        WinMoney += WinMoney1;
                        WinMoneyNoWithTax += WinMoneyNoWithTax1;
                    }

                    if (Locate[0] != "-")
                    {
                        continue;
                    }

                    if (IsRight[4] && IsRight[3] && IsRight[2] && IsRight[1])
                    {
                        WinCount4X++;
                        WinMoney += WinMoney2;
                        WinMoneyNoWithTax += WinMoneyNoWithTax2;
                    }

                    if (Locate[1] != "-")
                    {
                        continue;
                    }

                    if (IsRight[4] && IsRight[3] && IsRight[2])
                    {
                        WinCount3X++;
                        WinMoney += WinMoney3;
                        WinMoneyNoWithTax += WinMoneyNoWithTax3;
                    }

                    if (Locate[2] != "-")
                    {
                        continue;
                    }

                    if (IsRight[4] && IsRight[3])
                    {
                        WinCount2X++;
                        WinMoney += WinMoney4;
                        WinMoneyNoWithTax += WinMoneyNoWithTax4;
                    }

                    if (Locate[3] != "-")
                    {
                        continue;
                    }

                    if (IsRight[4])
                    {
                        WinCount1X++;
                        WinMoney += WinMoney5;
                        WinMoneyNoWithTax += WinMoneyNoWithTax5;
                    }
                }

                Description = "";

                if (WinCount5X > 0)
                {
                    MergeWinDescription(ref Description, "五星奖" + WinCount5X.ToString() + "注");
                }

                if (WinCount4X > 0)
                {
                    MergeWinDescription(ref Description, "四星奖" + WinCount4X.ToString() + "注");
                }

                if (WinCount3X > 0)
                {
                    MergeWinDescription(ref Description, "三星奖" + WinCount3X.ToString() + "注");
                }

                if (WinCount2X > 0)
                {
                    MergeWinDescription(ref Description, "二星奖" + WinCount2X.ToString() + "注");
                }

                if (WinCount1X > 0)
                {
                    MergeWinDescription(ref Description, "一星奖" + WinCount1X.ToString() + "注");
                }

                if (Description != "")
                    Description += "。";

                return WinMoney;
            }
            private double ComputeWin_DX(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney1, double WinMoneyNoWithTax1, ref int WinCountDX)
            {
                WinCountDX = 0;

                WinNumber = WinNumber.Trim();
                if (WinNumber.Length < 5)	//12345
                    return -1;

                string[] Lotterys = SplitLotteryNumber(Number);
                if (Lotterys == null)
                    return -2;
                if (Lotterys.Length < 1)
                    return -2;

                string WinNumber_1 = "", WinNumber_2 = "";
                int Num = Shove._Convert.StrToInt(WinNumber.Substring(3, 1), 0);
                WinNumber_1 += (Num <= 4) ? "小" : "大";
                WinNumber_1 += ((Num % 2) == 0) ? "双" : "单";
                Num = Shove._Convert.StrToInt(WinNumber.Substring(4, 1), 0);
                WinNumber_2 += (Num <= 4) ? "小" : "大";
                WinNumber_2 += ((Num % 2) == 0) ? "双" : "单";

                double WinMoney = 0;
                WinMoneyNoWithTax = 0;

                for (int ii = 0; ii < Lotterys.Length; ii++)
                {
                    string t_str = "";
                    string[] Lottery = ToSingle_DX(Lotterys[ii], ref t_str);
                    if (Lottery == null)
                        continue;
                    if (Lottery.Length < 1)
                        continue;

                    for (int i = 0; i < Lottery.Length; i++)
                    {
                        if (Lottery[i].Length < 2)
                            continue;

                        if ((WinNumber_1.IndexOf(Lottery[i][0]) >= 0) && (WinNumber_2.IndexOf(Lottery[i][1]) >= 0))
                        {
                            WinCountDX++;
                            WinMoney += WinMoney1;
                            WinMoneyNoWithTax += WinMoneyNoWithTax1;
                        }
                    }
                }

                Description = "";

                if (WinCountDX > 0)
                {
                    MergeWinDescription(ref Description, "猜大小奖" + WinCountDX.ToString() + "注");
                }

                if (Description != "")
                    Description += "。";

                return WinMoney;
            }
            private double ComputeWin_5X_TX(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney1, double WinMoneyNoWithTax1, double WinMoney2, double WinMoneyNoWithTax2, double WinMoney3, double WinMoneyNoWithTax3, ref int WinCount_5XTX_1, ref int WinCount_5XTX_2, ref int WinCount_5XTX_3)
            {
                WinCount_5XTX_1 = 0;
                WinCount_5XTX_2 = 0;
                WinCount_5XTX_3 = 0;

                WinNumber = WinNumber.Trim();
                if (WinNumber.Length < 5)	//3: "12345"
                    return -1;
                string[] Lotterys = SplitLotteryNumber(Number);
                if (Lotterys == null)
                    return -2;
                if (Lotterys.Length < 1)
                    return -2;

                double WinMoney = 0;
                WinMoneyNoWithTax = 0;

                string[] Locate = new string[5];
                bool[] IsRight = new bool[5];
                int[] Num = new int[5];

                Regex regex = new Regex(@"^(?<L0>([\d-])|([(][\d]+?[)]))(?<L1>([\d-])|([(][\d]+?[)]))(?<L2>([\d-])|([(][\d]+?[)]))(?<L3>([\d-])|([(][\d]+?[)]))(?<L4>(\d)|([(][\d]+?[)]))", RegexOptions.IgnoreCase | RegexOptions.Compiled);

                for (int ii = 0; ii < Lotterys.Length; ii++)
                {
                    Match m = regex.Match(Lotterys[ii]);

                    for (int j = 0; j < 5; j++)
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

                    if (IsRight[4] && IsRight[3] && IsRight[2] && IsRight[1] && IsRight[0])
                    {
                        WinCount_5XTX_1++;
                        WinMoney += WinMoney1;
                        WinMoneyNoWithTax += WinMoneyNoWithTax1;
                    }

                    if (IsRight[4] && IsRight[3] && IsRight[2])
                    {
                        int Count = Num[0] * Num[1];

                        WinCount_5XTX_2 = WinCount_5XTX_2 + Count;
                        WinMoney += WinMoney2 * Count;
                        WinMoneyNoWithTax += WinMoneyNoWithTax2 * Count;
                    }

                    if (IsRight[0] && IsRight[1] && IsRight[2])
                    {
                        int Count = Num[4] * Num[3];

                        if (IsRight[4] && IsRight[3])
                        {
                            Count = Count - 1;
                        }

                        WinCount_5XTX_2 = WinCount_5XTX_2 + Count;
                        WinMoney += WinMoney2 * Count;
                        WinMoneyNoWithTax += WinMoneyNoWithTax2 * Count;
                    }

                    if (IsRight[4] && IsRight[3])
                    {
                        int Count = Num[0] * Num[1] * Num[2];

                        WinCount_5XTX_3 = WinCount_5XTX_3 + Count;
                        WinMoney += WinMoney3 * Count;
                        WinMoneyNoWithTax += WinMoneyNoWithTax3 * Count;
                    }

                    if (IsRight[0] && IsRight[1])
                    {
                        int Count = Num[2] * Num[3] * Num[4];

                        if (IsRight[4] && IsRight[3])
                        {
                            Count = Count - 1;
                        }

                        WinCount_5XTX_3 = WinCount_5XTX_3 + Count;
                        WinMoney += WinMoney3 * Count;
                        WinMoneyNoWithTax += WinMoneyNoWithTax3 * Count;
                    }
                }

                Description = "";

                if (WinCount_5XTX_1 > 0)
                {
                    MergeWinDescription(ref Description, "五星通选一等奖" + WinCount_5XTX_1.ToString() + "注");
                }

                if (WinCount_5XTX_2 > 0)
                {
                    MergeWinDescription(ref Description, "五星通选二等奖" + WinCount_5XTX_2.ToString() + "注");
                }

                if (WinCount_5XTX_3 > 0)
                {
                    MergeWinDescription(ref Description, "五星通选三等奖" + WinCount_5XTX_3.ToString() + "注");
                }

                if (Description != "")
                    Description += "。";

                return WinMoney;
            }
            private double ComputeWin_2X_ZuD(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney1, double WinMoneyNoWithTax1, ref int WinCount_2X_Zu)
            {
                WinCount_2X_Zu = 0;

                WinNumber = WinNumber.Trim();
                if (WinNumber.Length < 5)	//3: "12345"
                    return -1;
                string[] Lotterys = SplitLotteryNumber(Number);
                if (Lotterys == null)
                    return -2;
                if (Lotterys.Length < 1)
                    return -2;

                WinNumber = WinNumber.Substring(3, 2);  // 只需要后面2位

                double WinMoney = 0;
                WinMoneyNoWithTax = 0;

                for (int ii = 0; ii < Lotterys.Length; ii++)
                {
                    string t_str = "";
                    string[] Lottery = ToSingle_2X_ZuD(Lotterys[ii], ref t_str);
                    if (Lottery == null)
                        continue;
                    if (Lottery.Length < 1)
                        continue;

                    for (int i = 0; i < Lottery.Length; i++)
                    {
                        if (Lottery[i].Length < 2)
                            continue;

                        if (Sort(Lottery[i]) == Sort(WinNumber))
                        {
                            WinCount_2X_Zu++;
                            WinMoney += WinMoney1;
                            WinMoneyNoWithTax += WinMoneyNoWithTax1;
                        }
                    }
                }

                Description = "";

                if (WinCount_2X_Zu > 0)
                {
                    MergeWinDescription(ref Description, "二星组选奖" + WinCount_2X_Zu.ToString() + "注");
                }

                if (Description != "")
                    Description += "。";

                return WinMoney;
            }
            private double ComputeWin_2X_ZuF(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney1, double WinMoneyNoWithTax1, double WinMoney2, double WinMoneyNoWithTax2, ref int WinCount_2X_Zu, ref int WinCount_2X_Zu_DZH)
            {
                WinCount_2X_Zu = 0;
                WinCount_2X_Zu_DZH = 0;

                WinNumber = WinNumber.Trim();
                if (WinNumber.Length < 5)	//3: "12345"
                    return -1;
                string[] Lotterys = SplitLotteryNumber(Number);
                if (Lotterys == null)
                    return -2;
                if (Lotterys.Length < 1)
                    return -2;

                WinNumber = WinNumber.Substring(3, 2);  // 只需要后面2位
                bool isDZH = (WinNumber[0] == WinNumber[1]); // 是否对子号

                double WinMoney = 0;
                WinMoneyNoWithTax = 0;

                for (int ii = 0; ii < Lotterys.Length; ii++)
                {
                    string t_str = "";
                    string[] Lottery = ToSingle_2X_ZuF(Lotterys[ii], ref t_str);
                    if (Lottery == null)
                        continue;
                    if (Lottery.Length < 1)
                        continue;

                    for (int i = 0; i < Lottery.Length; i++)
                    {
                        if (Lottery[i].Length < 2)
                            continue;

                        if (Sort(Lottery[i]) == Sort(WinNumber))
                        {
                            if (!isDZH)
                            {
                                WinCount_2X_Zu++;
                                WinMoney += WinMoney1;
                                WinMoneyNoWithTax += WinMoneyNoWithTax1;
                            }
                            else
                            {
                                WinCount_2X_Zu_DZH++;
                                WinMoney += WinMoney2;
                                WinMoneyNoWithTax += WinMoneyNoWithTax2;
                            }
                        }
                    }
                }

                Description = "";

                if (WinCount_2X_Zu > 0)
                {
                    MergeWinDescription(ref Description, "二星组选奖" + WinCount_2X_Zu.ToString() + "注");
                }

                if (WinCount_2X_Zu_DZH > 0)
                {
                    MergeWinDescription(ref Description, "二星组选奖(对子号)" + WinCount_2X_Zu_DZH.ToString() + "注");
                }

                if (Description != "")
                    Description += "。";

                return WinMoney;
            }
            private double ComputeWin_2X_ZuB(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney1, double WinMoneyNoWithTax1, double WinMoney2, double WinMoneyNoWithTax2, ref int WinCount_2X_Zu, ref int WinCount_2X_Zu_DZH)
            {
                WinCount_2X_Zu = 0;
                WinCount_2X_Zu_DZH = 0;

                WinNumber = WinNumber.Trim();
                if (WinNumber.Length < 5)	//5: "12345"
                    return -1;
                string[] Lotterys = SplitLotteryNumber(Number);
                if (Lotterys == null)
                    return -2;
                if (Lotterys.Length < 1)
                    return -2;

                WinNumber = WinNumber.Substring(3, 2);  // 只需要后面2位
                bool isDZH = (WinNumber[0] == WinNumber[1]); // 是否对子号

                double WinMoney = 0;
                WinMoneyNoWithTax = 0;

                for (int ii = 0; ii < Lotterys.Length; ii++)
                {
                    string t_str = "";
                    string[] Lottery = ToSingle_2X_ZuB(Lotterys[ii], ref t_str);
                    if (Lottery == null)
                        continue;
                    if (Lottery.Length < 1)
                        continue;

                    for (int i = 0; i < Lottery.Length; i++)
                    {
                        if (Lottery[i].Length < 2)
                            continue;

                        if (Sort(Lottery[i]) == Sort(WinNumber))
                        {
                            if (!isDZH)
                            {
                                WinCount_2X_Zu++;
                                WinMoney += WinMoney1;
                                WinMoneyNoWithTax += WinMoneyNoWithTax1;
                            }
                            else
                            {
                                WinCount_2X_Zu_DZH++;
                                WinMoney += WinMoney2;
                                WinMoneyNoWithTax += WinMoneyNoWithTax2;
                            }
                        }
                    }
                }

                Description = "";

                if (WinCount_2X_Zu > 0)
                {
                    MergeWinDescription(ref Description, "二星组选奖" + WinCount_2X_Zu.ToString() + "注");
                }

                if (WinCount_2X_Zu_DZH > 0)
                {
                    MergeWinDescription(ref Description, "二星组选奖(对子号)" + WinCount_2X_Zu_DZH.ToString() + "注");
                }

                if (Description != "")
                    Description += "。";

                return WinMoney;
            }
            private double ComputeWin_2X_ZuBD(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney1, double WinMoneyNoWithTax1, double WinMoney2, double WinMoneyNoWithTax2, ref int WinCount_2X_Zu, ref int WinCount_2X_Zu_DZH)
            {
                WinCount_2X_Zu = 0;
                WinCount_2X_Zu_DZH = 0;

                WinNumber = WinNumber.Trim();
                if (WinNumber.Length < 5)	//5: "12345"
                    return -1;
                string[] Lotterys = SplitLotteryNumber(Number);
                if (Lotterys == null)
                    return -2;
                if (Lotterys.Length < 1)
                    return -2;

                WinNumber = WinNumber.Substring(3, 2);  // 只需要后面2位
                bool isDZH = (WinNumber[0] == WinNumber[1]); // 是否对子号

                double WinMoney = 0;
                WinMoneyNoWithTax = 0;

                for (int ii = 0; ii < Lotterys.Length; ii++)
                {
                    string t_str = "";
                    string[] Lottery = ToSingle_2X_ZuBD(Lotterys[ii], ref t_str);
                    if (Lottery == null)
                        continue;
                    if (Lottery.Length < 1)
                        continue;

                    for (int i = 0; i < Lottery.Length; i++)
                    {
                        if (Lottery[i].Length < 2)
                            continue;

                        if (Sort(Lottery[i]) == Sort(WinNumber))
                        {
                            if (!isDZH)
                            {
                                WinCount_2X_Zu++;
                                WinMoney += WinMoney1;
                                WinMoneyNoWithTax += WinMoneyNoWithTax1;
                            }
                            else
                            {
                                WinCount_2X_Zu_DZH++;
                                WinMoney += WinMoney2;
                                WinMoneyNoWithTax += WinMoneyNoWithTax2;
                            }
                        }
                    }
                }

                Description = "";

                if (WinCount_2X_Zu > 0)
                {
                    MergeWinDescription(ref Description, "二星组选奖" + WinCount_2X_Zu.ToString() + "注");
                }

                if (WinCount_2X_Zu_DZH > 0)
                {
                    MergeWinDescription(ref Description, "二星组选奖(对子号)" + WinCount_2X_Zu_DZH.ToString() + "注");
                }

                if (Description != "")
                    Description += "。";

                return WinMoney;
            }
            #endregion

            public override string AnalyseScheme(string Content, int PlayType)
            {
                if (PlayType == PlayType_D)
                    return AnalyseScheme_D(Content, PlayType);

                if (PlayType == PlayType_F)
                    return AnalyseScheme_F(Content, PlayType);

                if (PlayType == PlayType_ZH)
                    return AnalyseScheme_ZH(Content, PlayType);

                if (PlayType == PlayType_DX)
                    return AnalyseScheme_DX(Content, PlayType);

                if ((PlayType == PlayType_5X_TXD) || (PlayType == PlayType_5X_TXF))
                    return AnalyseScheme_5X_TX(Content, PlayType);

                if (PlayType == PlayType_2X_ZuD)
                    return AnalyseScheme_2X_ZuD(Content, PlayType);

                if (PlayType == PlayType_2X_ZuF)
                    return AnalyseScheme_2X_ZuF(Content, PlayType);

                if (PlayType == PlayType_2X_ZuB)
                    return AnalyseScheme_2X_ZuB(Content, PlayType);

                if (PlayType == PlayType_2X_ZuBD)
                    return AnalyseScheme_2X_ZuBD(Content, PlayType);

                return "";
            }
            #region AnalyseScheme 的具体方法
            private string AnalyseScheme_D(string Content, int PlayType)
            {
                string[] strs = Content.Split('\n');
                if (strs == null)
                    return "";
                if (strs.Length == 0)
                    return "";

                string Result = "";

                string RegexString = @"^(([\d])|([-])){4}[\d]";
                Regex regex = new Regex(RegexString, RegexOptions.Compiled | RegexOptions.IgnoreCase);

                for (int i = 0; i < strs.Length; i++)
                {
                    Match m = regex.Match(strs[i]);
                    if (m.Success)
                    {
                        string CanonicalNumber = "";
                        string[] singles = ToSingle_D(m.Value, ref CanonicalNumber);
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
            private string AnalyseScheme_F(string Content, int PlayType)
            {
                string[] strs = Content.Split('\n');
                if (strs == null)
                    return "";
                if (strs.Length == 0)
                    return "";

                string Result = "";

                string RegexString = @"^(([\d])|([-])){4}[\d]";
                Regex regex = new Regex(RegexString, RegexOptions.Compiled | RegexOptions.IgnoreCase);

                for (int i = 0; i < strs.Length; i++)
                {
                    Match m = regex.Match(strs[i]);
                    if (m.Success)
                    {
                        string CanonicalNumber = "";
                        string[] singles = ToSingle_F(m.Value, ref CanonicalNumber);
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
            private string AnalyseScheme_ZH(string Content, int PlayType)
            {
                string[] strs = Content.Split('\n');
                if (strs == null)
                    return "";
                if (strs.Length == 0)
                    return "";

                string Result = "";
                int Num = 1;
                string CanonicalNumber = "";

                string[] Locate = new string[5];

                Regex regex = new Regex(@"^(?<L0>([\d-])|([(][\d]+?[)]))(?<L1>([\d-])|([(][\d]+?[)]))(?<L2>([\d-])|([(][\d]+?[)]))(?<L3>([\d-])|([(][\d]+?[)]))(?<L4>(\d)|([(][\d]+?[)]))", RegexOptions.IgnoreCase | RegexOptions.Compiled);

                for (int i = 0; i < strs.Length; i++)
                {
                    Match m = regex.Match(strs[i]);

                    CanonicalNumber = "";

                    for (int j = 0; j < 5; j++)
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
            private string AnalyseScheme_DX(string Content, int PlayType)
            {
                string[] strs = Content.Split('\n');
                if (strs == null)
                    return "";
                if (strs.Length == 0)
                    return "";

                string Result = "";

                string RegexString = @"^([[]猜大小[]])*?([大小单双]){2}";
                Regex regex = new Regex(RegexString, RegexOptions.Compiled | RegexOptions.IgnoreCase);

                for (int i = 0; i < strs.Length; i++)
                {
                    Match m = regex.Match(strs[i]);
                    if (m.Success)
                    {
                        string CanonicalNumber = "";
                        string[] singles = ToSingle_DX(m.Value.Replace("[猜大小]", ""), ref CanonicalNumber);
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
            private string AnalyseScheme_5X_TX(string Content, int PlayType)
            {
                string[] strs = Content.Split('\n');
                if (strs == null)
                    return "";
                if (strs.Length == 0)
                    return "";

                string Result = "";
                int Num = 1;
                string CanonicalNumber = "";

                string[] Locate = new string[5];

                Regex regex = new Regex(@"^(?<L0>([\d-])|([(][\d]+?[)]))(?<L1>([\d-])|([(][\d]+?[)]))(?<L2>([\d-])|([(][\d]+?[)]))(?<L3>([\d-])|([(][\d]+?[)]))(?<L4>(\d)|([(][\d]+?[)]))", RegexOptions.IgnoreCase | RegexOptions.Compiled);

                for (int i = 0; i < strs.Length; i++)
                {
                    Match m = regex.Match(strs[i]);

                    CanonicalNumber = "";

                    for (int j = 0; j < 5; j++)
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
                                Locate[i] = FilterRepeated(Locate[j]);
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
            private string AnalyseScheme_2X_ZuD(string Content, int PlayType)
            {
                string[] strs = Content.Split('\n');

                if (strs == null)
                {
                    return "";
                }

                if (strs.Length == 0)
                {
                    return "";
                }

                string Result = "";

                string RegexString;

                if (PlayType == PlayType_2X_ZuD)
                {
                    RegexString = @"^(\d){2}";
                }
                else
                {
                    RegexString = @"^(\d){3,10}";
                }

                Regex regex = new Regex(RegexString, RegexOptions.Compiled | RegexOptions.IgnoreCase);

                for (int i = 0; i < strs.Length; i++)
                {
                    Match m = regex.Match(strs[i]);
                    if (m.Success)
                    {
                        string CanonicalNumber = "";
                        string[] singles = ToSingle_2X_ZuD(m.Value, ref CanonicalNumber);
                        if (singles == null)
                            continue;
                        if (singles.Length >= ((PlayType == PlayType_2X_ZuD) ? 1 : 2))
                        {
                            if (FilterRepeated(Sort(m.Value)).Length == 2)
                            {
                                if (PlayType != PlayType_2X_ZuF)
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
            private string AnalyseScheme_2X_ZuF(string Content, int PlayType)
            {
                string[] strs = Content.Split('\n');

                if (strs == null)
                {
                    return "";
                }

                if (strs.Length == 0)
                {
                    return "";
                }

                string Result = "";

                string RegexString;

                if (PlayType == PlayType_2X_ZuD)
                {
                    RegexString = @"^(\d){2}";
                }
                else
                {
                    RegexString = @"^(\d){3,10}";
                }

                Regex regex = new Regex(RegexString, RegexOptions.Compiled | RegexOptions.IgnoreCase);

                for (int i = 0; i < strs.Length; i++)
                {
                    Match m = regex.Match(strs[i]);
                    if (m.Success)
                    {
                        string CanonicalNumber = "";
                        string[] singles = ToSingle_2X_ZuF(m.Value, ref CanonicalNumber);
                        if (singles == null)
                            continue;
                        if (singles.Length >= ((PlayType == PlayType_2X_ZuD) ? 1 : 2))
                        {
                            if (FilterRepeated(Sort(m.Value)).Length == 2)
                            {
                                if (PlayType != PlayType_2X_ZuF)
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
            private string AnalyseScheme_2X_ZuB(string Content, int PlayType)
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
                        string[] singles = ToSingle_2X_ZuB(m.Value, ref CanonicalNumber);
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
            private string AnalyseScheme_2X_ZuBD(string Content, int PlayType)
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
                        string[] singles = ToSingle_2X_ZuBD(m.Value, ref CanonicalNumber);
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
                Regex regex = new Regex(@"^([\d]){5}", RegexOptions.Compiled | RegexOptions.IgnoreCase);

                return regex.IsMatch(Number);
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
                    if ((Result.IndexOf(NumberPart.Substring(i, 1)) == -1) && ("0123456789-".IndexOf(NumberPart.Substring(i, 1)) >= 0))
                        Result += NumberPart.Substring(i, 1);
                }
                return Sort(Result);
            }

            public override string ShowNumber(string Number)
            {
                return base.ShowNumber(Number, " ");
            }

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
                if (PlayTypeID == PlayType_ZH)
                {
                    return ToElectronicTicket_ZCW_ZH(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                if (PlayTypeID == PlayType_DX)
                {
                    return ToElectronicTicket_ZCW_DX(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                if (PlayTypeID == PlayType_5X_TXD)
                {
                    return ToElectronicTicket_ZCW_5X_TXD(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                if (PlayTypeID == PlayType_5X_TXF)
                {
                    return ToElectronicTicket_ZCW_5X_TXF(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                if (PlayTypeID == PlayType_2X_ZuD)
                {
                    return ToElectronicTicket_ZCW_2X_ZuD(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                if (PlayTypeID == PlayType_2X_ZuF)
                {
                    return ToElectronicTicket_ZCW_2X_ZuF(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                if (PlayTypeID == PlayType_2X_ZuB)
                {
                    return ToElectronicTicket_ZCW_2X_ZuB(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                if (PlayTypeID == PlayType_2X_ZuBD)
                {
                    return ToElectronicTicket_ZCW_2X_ZuBD(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }

                return null;
            }
            #region ToElectronicTicket_ZCW 的具体方法
            private Ticket[] ToElectronicTicket_ZCW_D(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                string[] strs = AnalyseScheme_D(Number, PlayTypeID).Split('\n');

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
                                if (string.IsNullOrEmpty(strs[i + M].ToString()))
                                {
                                    continue;
                                }

                                Numbers += strs[i + M].ToString().Split('|')[0];
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
                string[] strs = AnalyseScheme_F(Number, PlayTypeID).Split('\n');

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
                        if (string.IsNullOrEmpty(strs[i].ToString()))
                        {
                            continue;
                        }

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
            private Ticket[] ToElectronicTicket_ZCW_ZH(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                string[] strs = AnalyseScheme_ZH(Number, PlayTypeID).Split('\n');

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

                string TicketContent = "";
                int SaleType = 3;

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
                        if (string.IsNullOrEmpty(strs[i].ToString()))
                        {
                            continue;
                        }

                        string Numbers = "";
                        EachMoney = 0;

                        Numbers = strs[i].ToString().Split('|')[0];
                        EachMoney += 2 * double.Parse(strs[i].ToString().Split('|')[1]);

                        Money += EachMoney * EachMultiple;

                        TicketContent = ConvertFormatToElectronTicket_ZCW(PlayTypeID, Numbers);

                        if (TicketContent.Split('*').Length == 1)
                        {
                            SaleType = 10;
                        }
                        else if (TicketContent.Split('*').Length == 4)
                        {
                            SaleType = 2;
                        }
                        else
                        {
                            SaleType = 3;
                        }

                        al.Add(new Ticket(SaleType, TicketContent, EachMultiple, EachMoney * EachMultiple));
                    }
                }

                Ticket[] Tickets = new Ticket[al.Count];

                for (int i = 0; i < Tickets.Length; i++)
                {
                    Tickets[i] = (Ticket)al[i];
                }

                return Tickets;
            }
            private Ticket[] ToElectronicTicket_ZCW_DX(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                string[] strs = AnalyseScheme_DX(Number, PlayTypeID).Split('\n');

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
                                if (string.IsNullOrEmpty(strs[i + M].ToString()))
                                {
                                    continue;
                                }

                                Numbers += strs[i + M].ToString().Split('|')[0];
                                EachMoney += 2 * double.Parse(strs[i + M].ToString().Split('|')[1]);
                            }
                        }

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
            private Ticket[] ToElectronicTicket_ZCW_5X_TXD(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                string[] strs = AnalyseScheme_5X_TX(Number, PlayTypeID).Split('\n');

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
                                if (string.IsNullOrEmpty(strs[i + M].ToString()))
                                {
                                    continue;
                                }

                                Numbers += strs[i + M].ToString().Split('|')[0];
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
            private Ticket[] ToElectronicTicket_ZCW_5X_TXF(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                ArrayList al = new ArrayList();

                string[] strs = AnalyseSchemeToElectronicTicket_F(Number, PlayTypeID).Split('\n');

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
                                if (string.IsNullOrEmpty(strs[i + M].ToString()))
                                {
                                    continue;
                                }

                                Numbers += strs[i + M].ToString().Split('|')[0];
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
            private Ticket[] ToElectronicTicket_ZCW_2X_ZuD(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                string[] strs = AnalyseScheme_2X_ZuD(Number, PlayTypeID).Split('\n');

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
                                if (string.IsNullOrEmpty(strs[i + M].ToString()))
                                {
                                    continue;
                                }

                                Numbers += strs[i + M].ToString().Split('|')[0] + "\n";
                                EachMoney += 2 * double.Parse(strs[i + M].ToString().Split('|')[1]);
                            }
                        }

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
            private Ticket[] ToElectronicTicket_ZCW_2X_ZuF(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                string[] strs = AnalyseScheme_2X_ZuF(Number, PlayTypeID).Split('\n');

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
                        if (string.IsNullOrEmpty(strs[i].ToString()))
                        {
                            continue;
                        }

                        string Numbers = "";
                        EachMoney = 0;

                        Numbers = strs[i].ToString().Split('|')[0];
                        EachMoney += 2 * double.Parse(strs[i].ToString().Split('|')[1]);

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
            private Ticket[] ToElectronicTicket_ZCW_2X_ZuB(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                string[] strs = AnalyseScheme_2X_ZuB(Number, PlayTypeID).Split('\n');

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
                        if (string.IsNullOrEmpty(strs[i].ToString()))
                        {
                            continue;
                        }

                        string Numbers = "";
                        EachMoney = 0;

                        Numbers = strs[i].ToString().Split('|')[0];
                        EachMoney += 2 * double.Parse(strs[i].ToString().Split('|')[1]);

                        Money += EachMoney * EachMultiple;

                        al.Add(new Ticket(7, ConvertFormatToElectronTicket_ZCW(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));
                    }
                }

                Ticket[] Tickets = new Ticket[al.Count];

                for (int i = 0; i < Tickets.Length; i++)
                {
                    Tickets[i] = (Ticket)al[i];
                }

                return Tickets;
            }
            private Ticket[] ToElectronicTicket_ZCW_2X_ZuBD(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                string[] strs = AnalyseScheme_2X_ZuBD(Number, PlayTypeID).Split('\n');

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
                        if (string.IsNullOrEmpty(strs[i].ToString()))
                        {
                            continue;
                        }

                        string Numbers = "";
                        EachMoney = 0;

                        Numbers = strs[i].ToString().Split('|')[0];
                        EachMoney += 2 * double.Parse(strs[i].ToString().Split('|')[1]);

                        Money += EachMoney * EachMultiple;

                        al.Add(new Ticket(9, ConvertFormatToElectronTicket_ZCW(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));
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

                if ((PlayTypeID == PlayType_2X_ZuD) || (PlayTypeID == PlayType_2X_ZuF) || (PlayTypeID == PlayType_2X_ZuB) || (PlayTypeID == PlayType_2X_ZuBD))
                {
                    Ticket = Number;
                }

                if (PlayTypeID == PlayType_D || PlayTypeID == PlayType_F || PlayTypeID == PlayType_5X_TXD || PlayTypeID == PlayType_5X_TXF)
                {
                    string[] strs = Number.Replace("-","").Split('\n');

                    for (int j = 0; j < strs.Length; j++)
                    {
                        for (int i = 0; i < strs[j].Length; i++)
                        {
                            if (i % 5 == 0 && i > 0)
                            {
                                Ticket = Ticket.Substring(0, Ticket.Length - 1);
                                Ticket += "\n" + strs[j].Substring(i, 1) + "*";
                            }
                            else
                            {
                                Ticket += strs[j].Substring(i, 1) + "*";
                            }
                        }

                        if (Ticket.EndsWith("*"))
                            Ticket = Ticket.Substring(0, Ticket.Length - 1);

                        Ticket += "\n";
                    }

                    if (Ticket.EndsWith("\n"))
                        Ticket = Ticket.Substring(0, Ticket.Length - 1);
                }

                if (PlayTypeID == PlayType_ZH)
                {
                    string[] Locate = new string[5];

                    Regex regex = new Regex(@"^(?<L0>([\d-])|([(][\d]+?[)]))(?<L1>([\d-])|([(][\d]+?[)]))(?<L2>([\d-])|([(][\d]+?[)]))(?<L3>([\d-])|([(][\d]+?[)]))(?<L4>(\d)|([(][\d]+?[)]))", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                    Match m = regex.Match(Number);

                    for (int i = 0; i < 5; i++)
                    {
                        Locate[i] = m.Groups["L" + i.ToString()].ToString().Trim();

                        if (Locate[i] == "-")
                        {
                            continue;
                        }

                        if (Locate[i].Length > 1)
                        {
                            Locate[i] = Locate[i].Substring(1, Locate[i].Length - 2);
                        }

                        Ticket += Locate[i].ToString() + "*";
                    }

                    if (Ticket.EndsWith("*"))
                        Ticket = Ticket.Substring(0, Ticket.Length - 1);
                }

                if (PlayTypeID == PlayType_DX)
                {
                    Number = Number.Replace("大", "9").Replace("小", "0").Replace("单", "1").Replace("双", "2");

                    string[] strs = Number.Split('\n');

                    for (int j = 0; j < strs.Length; j++)
                    {
                        for (int i = 0; i < strs[j].Length; i++)
                        {
                            if (i % 2 == 0 && i > 0)
                            {
                                Ticket = Ticket.Substring(0, Ticket.Length - 1);
                                Ticket += "\n" + strs[j].Substring(i, 1) + "*";
                            }
                            else
                            {
                                Ticket += strs[j].Substring(i, 1) + "*";
                            }
                        }

                        if (Ticket.EndsWith("*"))
                            Ticket = Ticket.Substring(0, Ticket.Length - 1);

                        Ticket += "\n";
                    }

                    if (Ticket.EndsWith("\n"))
                        Ticket = Ticket.Substring(0, Ticket.Length - 1);
                }

                if (Ticket.EndsWith("*"))
                    Ticket = Ticket.Substring(0, Ticket.Length - 1);

                return Ticket;
            }

            private string AnalyseSchemeToElectronicTicket_F(string Content, int PlayType)
            {

                string[] strs = Content.Split('\n');
                if (strs == null)
                    return "";
                if (strs.Length == 0)
                    return "";

                string Result = "";

                string RegexString;

                if (PlayType == PlayType_5X_TXD)
                {
                    RegexString = @"^([\d]){5}";
                }
                else
                {
                    RegexString = @"^(([\d])|([(][\d]{1,10}[)])){5}";
                }

                Regex regex = new Regex(RegexString, RegexOptions.Compiled | RegexOptions.IgnoreCase);

                for (int i = 0; i < strs.Length; i++)
                {
                    Match m = regex.Match(strs[i]);
                    if (m.Success)
                    {
                        string CanonicalNumber = "";
                        string[] singles = ToSingle_5X_TX(m.Value, ref CanonicalNumber);
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
            #endregion
        }
    }
}