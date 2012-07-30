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
        /// 新疆时时彩
        /// </summary>
        public partial class XJSSC : LotteryBase
        {
            #region 静态变量
            public const int PlayType_Mixed = 6600; //混合投注，包含下面所有的玩法，号码前加 [单式]、[复式]、[组合玩法]、[猜大小]、[二星组选单式]、[二星组选复式]、[二星组选分位]、[二星组选包点]、[二星组选包胆]、[三星组选包点]...

            public const int PlayType_D = 6601;  //常规买法,注意，5星复式表示含5星，3星，2星，1星共4注，复式的概念和传统彩票不同
            public const int PlayType_F = 6602;

            public const int PlayType_ZH = 6603;  //组合玩法，和传统彩票复式概念一样

            public const int PlayType_DX = 6604;  //猜大小

            public const int PlayType_5X_TXD = 6605; //五星通选单式
            public const int PlayType_5X_TXF = 6606; //五星通选复式

            public const int PlayType_2X_ZuD = 6607; //二星组选单式
            public const int PlayType_2X_ZuF = 6608; //二星组选复式
            public const int PlayType_2X_ZuFW = 6609; //二星组选分位
            public const int PlayType_2X_ZuB = 6610; // 二星组选包点
            public const int PlayType_2X_ZuBD = 6611; // 二星组选包胆

            public const int PlayType_3X_B = 6612; //三星包点

            public const int PlayType_2X_QW = 6613; //趣味二星
            public const int PlayType_2X_QWF = 6614; //趣味二星复式

            //stone 2009-06-30
            public const int PlayType_3X_Zu3D = 6615; //三星组3单式
            public const int PlayType_3X_Zu3F = 6616; //三星组3复式

            public const int PlayType_3X_Zu6D = 6617; //三星组6单式
            public const int PlayType_3X_Zu6F = 6618; //三星组6复式

            public const int PlayType_3X_ZHFS = 6619; //三星直选组合复式
            public const int PlayType_3X_ZuB = 6620;  //三星组选包胆
            public const int PlayType_3X_ZuBD = 6621; //三星组选包点

            public const int ID = 66;
            public const string sID = "66";
            public const string Name = "新疆时时彩";
            public const string Code = "SHSSL";
            public const double MaxMoney = 200000;
            #endregion

            public XJSSC()
            {
                id = 66;
                name = "新疆时时彩";
                code = "XJSSC";
            }

            public override bool CheckPlayType(int play_type)
            {
                return ((play_type >= 6600) && (play_type <= 6621));
            }

            public override PlayType[] GetPlayTypeList()
            {
                PlayType[] Result = new PlayType[22];

                Result[0] = new PlayType(PlayType_Mixed, "混合投注");
                Result[1] = new PlayType(PlayType_D, "单式");
                Result[2] = new PlayType(PlayType_F, "复式");
                Result[3] = new PlayType(PlayType_ZH, "组合玩法");
                Result[4] = new PlayType(PlayType_DX, "猜大小");
                Result[5] = new PlayType(PlayType_5X_TXD, "五星通选单式");
                Result[6] = new PlayType(PlayType_5X_TXF, "五星通选复式");
                Result[7] = new PlayType(PlayType_2X_ZuD, "二星组选单式");
                Result[8] = new PlayType(PlayType_2X_ZuF, "二星组选复式");
                Result[9] = new PlayType(PlayType_2X_ZuFW, "二星组选分位");
                Result[10] = new PlayType(PlayType_2X_ZuB, "二星组选包点");
                Result[11] = new PlayType(PlayType_2X_ZuBD, "二星组选包胆");
                Result[12] = new PlayType(PlayType_3X_B, "三星包点");
                Result[13] = new PlayType(PlayType_2X_QW, "趣味二星单式");
                Result[14] = new PlayType(PlayType_2X_QWF, "趣味二星复式");
                Result[15] = new PlayType(PlayType_3X_Zu3D, "三星组3单式");
                Result[16] = new PlayType(PlayType_3X_Zu3F, "三星组3复式");
                Result[17] = new PlayType(PlayType_3X_Zu6D, "三星组6单式");
                Result[18] = new PlayType(PlayType_3X_Zu6F, "三星组6复式");
                Result[19] = new PlayType(PlayType_3X_ZHFS, "三星直选组合复式");
                Result[20] = new PlayType(PlayType_3X_ZuB, "三星组选包胆");
                Result[21] = new PlayType(PlayType_3X_ZuBD, "三星组选包点");

                return Result;
            }

            public override string BuildNumber(int Num, int Type)       //Type: 5 = 5星, 3 = 3星, 2 = 2星, 1 = 1星, -1 = 大小单双
            {
                if ((Type != 5) && (Type != 3) && (Type != 2) && (Type != 1) && (Type != -1))
                    Type = 5;

                if (Type == -1) //大小单双
                {
                    return BuildNumber_DX(Num);
                }

                return BuildNumber_5321(Num, Type);
            }
            #region BuilderNumber 的具体方法
            private string BuildNumber_5321(int Num, int Type)
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
                if (PlayType == PlayType_Mixed)
                    return ToSingle_Mixed(Number, ref CanonicalNumber);

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

                if ((PlayType == PlayType_2X_ZuD) || (PlayType == PlayType_2X_ZuF))
                    return ToSingle_2X_ZuD_ZuF(Number, ref CanonicalNumber);

                if (PlayType == PlayType_2X_ZuFW)
                    return ToSingle_2X_ZuFW(Number, ref CanonicalNumber);

                if (PlayType == PlayType_2X_ZuB)
                    return ToSingle_2X_ZuB(Number, ref CanonicalNumber);

                if (PlayType == PlayType_2X_ZuBD)
                    return ToSingle_2X_ZuBD(Number, ref CanonicalNumber);

                if (PlayType == PlayType_3X_B)
                    return ToSingle_3X_B(Number, ref CanonicalNumber);

                //stone 2009-06-30
                if (PlayType == PlayType_3X_Zu3D)
                    return ToSingle_3X_Zu3D(Number, ref CanonicalNumber);

                if (PlayType == PlayType_3X_Zu3F)
                    return ToSingle_3X_Zu3F(Number, ref CanonicalNumber);

                if (PlayType == PlayType_3X_Zu6D)
                    return ToSingle_3X_Zu6D(Number, ref CanonicalNumber);

                if (PlayType == PlayType_3X_Zu6F)
                    return ToSingle_3X_Zu6F(Number, ref CanonicalNumber);

                if (PlayType == PlayType_3X_ZHFS)
                    return ToSingle_3X_ZHFS(Number, ref CanonicalNumber);

                if (PlayType == PlayType_3X_ZuB)
                    return ToSingle_3X_ZuB(Number, ref CanonicalNumber);

                if (PlayType == PlayType_3X_ZuBD)
                    return ToSingle_3X_ZuBD(Number, ref CanonicalNumber);

                if (PlayType == PlayType_2X_QW || PlayType == PlayType_2X_QWF)
                    return ToSingle_2X_QW(Number, ref CanonicalNumber);

                return null;
            }

            #region ToSingle 的具体方法

            private string[] ToSingle_Mixed(string Number, ref string CanonicalNumber)
            {
                CanonicalNumber = "";

                string PreFix = GetLotteryNumberPreFix(Number);

                if (Number.StartsWith("[单式]"))
                {
                    return MergeLotteryNumberPreFix(ToSingle_D(FilterPreFix(Number), ref CanonicalNumber), PreFix);
                }

                if (Number.StartsWith("[复式]"))
                {
                    return MergeLotteryNumberPreFix(ToSingle_F(FilterPreFix(Number), ref CanonicalNumber), PreFix);
                }

                if (Number.StartsWith("[组合玩法]"))
                {
                    return MergeLotteryNumberPreFix(ToSingle_ZH(FilterPreFix(Number), ref CanonicalNumber), PreFix);
                }

                if (Number.StartsWith("[猜大小]"))
                {
                    return MergeLotteryNumberPreFix(ToSingle_DX(FilterPreFix(Number), ref CanonicalNumber), PreFix);
                }

                if (Number.StartsWith("[五星通选单式]") || Number.StartsWith("[五星通选复式]"))
                {
                    return MergeLotteryNumberPreFix(ToSingle_5X_TX(FilterPreFix(Number), ref CanonicalNumber), PreFix);
                }

                if (Number.StartsWith("[二星组选单式]") || Number.StartsWith("[二星组选复式]"))
                {
                    return MergeLotteryNumberPreFix(ToSingle_2X_ZuD_ZuF(FilterPreFix(Number), ref CanonicalNumber), PreFix);
                }

                if (Number.StartsWith("[二星组选分位]"))
                {
                    return MergeLotteryNumberPreFix(ToSingle_2X_ZuFW(FilterPreFix(Number), ref CanonicalNumber), PreFix);
                }

                if (Number.StartsWith("[二星组选包点]"))
                {
                    return MergeLotteryNumberPreFix(ToSingle_2X_ZuB(FilterPreFix(Number), ref CanonicalNumber), PreFix);
                }

                if (Number.StartsWith("[二星组选包胆]"))
                {
                    return MergeLotteryNumberPreFix(ToSingle_2X_ZuBD(FilterPreFix(Number), ref CanonicalNumber), PreFix);
                }

                if (Number.StartsWith("[三星包点]"))
                {
                    return MergeLotteryNumberPreFix(ToSingle_3X_B(FilterPreFix(Number), ref CanonicalNumber), PreFix);
                }

                return null;
            }
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

                Regex[] regex5321 = new Regex[4];
                regex5321[0] = new Regex(@"^(\d){5}", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                regex5321[1] = new Regex(@"^--(\d){3}", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                regex5321[2] = new Regex(@"^---(\d){2}", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                regex5321[3] = new Regex(@"^----\d", RegexOptions.IgnoreCase | RegexOptions.Compiled);

                bool isMatch = false;
                for (int i = 0; i < 4; i++)
                {
                    if (regex5321[i].IsMatch(CanonicalNumber))
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

                Regex[] regex5321 = new Regex[4];
                regex5321[0] = new Regex(@"^(\d){5}", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                regex5321[1] = new Regex(@"^--(\d){3}", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                regex5321[2] = new Regex(@"^---(\d){2}", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                regex5321[3] = new Regex(@"^----\d", RegexOptions.IgnoreCase | RegexOptions.Compiled);

                string[] Result = null;

                if (regex5321[0].IsMatch(CanonicalNumber))
                {
                    Result = new string[4];

                    Result[0] = CanonicalNumber;
                    Result[1] = "--" + CanonicalNumber.Substring(2, 3);
                    Result[2] = "---" + CanonicalNumber.Substring(3, 2);
                    Result[3] = "----" + CanonicalNumber.Substring(4, 1);
                }

                else if (regex5321[1].IsMatch(CanonicalNumber))
                {
                    Result = new string[3];

                    Result[0] = CanonicalNumber;
                    Result[1] = "---" + CanonicalNumber.Substring(3, 2);
                    Result[2] = "----" + CanonicalNumber.Substring(4, 1);
                }

                else if (regex5321[2].IsMatch(CanonicalNumber))
                {
                    Result = new string[2];

                    Result[0] = CanonicalNumber;
                    Result[1] = "----" + CanonicalNumber.Substring(4, 1);
                }

                else if (regex5321[3].IsMatch(CanonicalNumber))
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
            private string[] ToSingle_2X_ZuD_ZuF(string Number, ref string CanonicalNumber)//二星组选 单式、复式
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
            private string[] ToSingle_2X_ZuFW(string Number, ref string CanonicalNumber) //二星组选分位
            {
                string[] Locate = new string[2];
                CanonicalNumber = "";

                Regex regex = new Regex(@"^(?<L0>(\d)|([(][\d]{2,10}[)]))(?<L1>(\d)|([(][\d]{2,10}[)]))", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                Match m = regex.Match(Number);

                if ((FilterRepeated(m.Groups["L0"].ToString()).Length < 2) && (FilterRepeated(m.Groups["L1"].ToString()).Length < 2))
                {
                    CanonicalNumber = "";

                    return null;
                }

                for (int i = 0; i < 2; i++)
                {
                    Locate[i] = m.Groups["L" + i.ToString()].ToString().Trim();

                    if (Locate[i].StartsWith("("))
                    {
                        Locate[i] = Locate[i].Substring(1, Locate[i].Length - 2);
                        Locate[i] = FilterRepeated(Locate[i]);
                    }

                    if (Locate[i].Length < 1)
                    {
                        CanonicalNumber = "";

                        return null;
                    }

                    if (Locate[i].Length > 1)
                    {
                        CanonicalNumber += "(" + Locate[i] + ")";
                    }
                    else
                    {
                        CanonicalNumber += Locate[i];
                    }
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
            private string[] ToSingle_3X_B(string sBill, ref string CanonicalNumber)//三星包点
            {
                int Bill = Shove._Convert.StrToInt(sBill, -1);
                CanonicalNumber = "";

                ArrayList al = new ArrayList();

                #region 循环取单式

                if ((Bill < 0) || (Bill > 27))
                {
                    CanonicalNumber = "";
                    return null;
                }
                else
                {
                    for (int i = 0; i <= 9; i++)
                    {
                        for (int j = 0; j <= 9; j++)
                        {
                            for (int k = 0; k <= 9; k++)
                            {
                                if (i + j + k == Bill)
                                    al.Add(i.ToString() + j.ToString() + k.ToString());
                            }
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
            private string[] ToSingle_2X_QW(string Number, ref string CanonicalNumber) //两星趣味
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

            //stone 2009-06-30
            private string[] ToSingle_3X_Zu3D(string Number, ref string CanonicalNumber) //组三单式
            {
                CanonicalNumber = "";

                string tNumber = FilterRepeated(Number.Trim());
                if (tNumber.Length != 2)
                {
                    CanonicalNumber = "";
                    return null;
                }

                CanonicalNumber = Number;
                string[] Result = new string[1];
                Result[0] = CanonicalNumber;

                return Result;
            }
            private string[] ToSingle_3X_Zu3F(string Number, ref string CanonicalNumber) //组三复式
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
                {
                    Result[i] = al[i].ToString();
                }
                return Result;
            }
            private string[] ToSingle_3X_Zu6D(string Number, ref string CanonicalNumber) //组六单式
            {
                CanonicalNumber = "";

                string tNumber = FilterRepeated(Number.Trim());
                if (tNumber.Length != 3)
                {
                    CanonicalNumber = "";
                    return null;
                }

                CanonicalNumber = Number;
                string[] Result = new string[1];
                Result[0] = CanonicalNumber;

                return Result;
            }
            private string[] ToSingle_3X_Zu6F(string Number, ref string CanonicalNumber) //组六复式
            {
                //CanonicalNumber = "";

                //Regex regex = new Regex(@"^(?<L0>(-))(?<L1>(-))(?<L2>(\d)|(-))(?<L3>(\d)|(-))(?<L4>(\d))", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                //Match m = regex.Match(Number);
                //for (int i = 0; i < 5; i++)
                //{
                //    string Locate = m.Groups["L" + i.ToString()].ToString().Trim();
                //    if (Locate == "")
                //    {
                //        CanonicalNumber = "";
                //        return null;
                //    }

                //    CanonicalNumber += Locate;
                //}

                //Regex regex3 = new Regex(@"^--(\d){3}", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                //bool isMatch = false;
                //if (regex3.IsMatch(CanonicalNumber))
                //{
                //    isMatch = true;
                //}

                //if (!isMatch)
                //{
                //    return null;
                //}

                //string[] Result = new string[1];
                //Result[0] = CanonicalNumber;

                //return Result;

                return ToSingle_Zu3D_Zu6(Number, ref CanonicalNumber);
            }
            private string[] ToSingle_Zu3D_Zu6(string Number, ref string CanonicalNumber)	//复式取单式, 后面 ref 参数是将彩票规范化，如：(10223) 变成1023
            {
                CanonicalNumber = FilterRepeated(Number.Trim());
                if (CanonicalNumber.Length < 2)
                {
                    CanonicalNumber = "";
                    return null;
                }

                CanonicalNumber = Number;

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

            private string[] ToSingle_3X_ZHFS(string Number, ref string CanonicalNumber) //三星直选组合复式
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

                for (int i = 0; i < strs.Length; i++)
                {
                    for (int j = 0; j < strs.Length; j++)
                    {
                        for (int k = 0; k < strs.Length; k++)
                        {
                            if (i != j && j != k && i != k && !al.Contains(strs[i].ToString() + strs[j].ToString() + strs[k].ToString()))
                            {
                                al.Add(strs[i].ToString() + strs[j].ToString() + strs[k].ToString());
                            }
                        }
                    }
                }

                #endregion

                string[] Result = new string[al.Count];
                for (int i = 0; i < al.Count; i++)
                {
                    Result[i] = al[i].ToString();
                }
                return Result;
            }
            private string[] ToSingle_3X_ZuB(string Number, ref string CanonicalNumber) //三星组选包胆
            {
                CanonicalNumber = Number;

                if ((CanonicalNumber.Length < 1) || (CanonicalNumber.Length > 2))
                {
                    CanonicalNumber = "";
                    return null;
                }

                char[] strs = CanonicalNumber.ToCharArray();

                ArrayList al = new ArrayList();

                #region 循环取单式

                int n = strs.Length;
                if (n == 1)
                {
                    for (int i = 0; i < n; i++)
                    {
                        for (int j = 0; j <= 9; j++)
                        {
                            for (int k = 0; k <= 9; k++)
                            {
                                if (strs[i].ToString() == j.ToString() && !al.Contains(strs[i].ToString() + j.ToString() + k.ToString()))
                                {
                                    al.Add(strs[i].ToString() + j.ToString() + k.ToString());
                                }

                                if (j.ToString() == k.ToString() && !al.Contains(strs[i].ToString() + j.ToString() + k.ToString()))
                                {
                                    al.Add(strs[i].ToString() + j.ToString() + k.ToString());
                                }

                                if (strs[i].ToString() != j.ToString() && strs[i].ToString() != k.ToString() && j.ToString() != k.ToString() && !al.Contains(Sort(strs[i].ToString() + j.ToString() + k.ToString())))
                                {
                                    al.Add(Sort(strs[i].ToString() + j.ToString() + k.ToString()));
                                }
                            }
                        }
                    }
                }
                else
                {
                    for (int j = 0; j <= 9; j++)
                    {
                        al.Add(Number + j.ToString());
                    }
                }

                #endregion

                string[] Result = new string[al.Count];
                for (int i = 0; i < al.Count; i++)
                    Result[i] = al[i].ToString();
                return Result;
            }
            private string[] ToSingle_3X_ZuBD(string Number, ref string CanonicalNumber) //三星组选包点
            {
                int Bill = Shove._Convert.StrToInt(Number, -1);
                CanonicalNumber = "";

                ArrayList al = new ArrayList();

                #region 循环取单式

                //选算出豹子
                for (int i = 0; i <= 9; i++)
                    for (int j = 0; j <= 9; j++)
                        for (int k = 0; k <= 9; k++)
                            if (i + j + k == Bill && i == j && j == k)
                                al.Add(i.ToString() + j.ToString() + k.ToString());


                if ((Bill < 1) || (Bill > 26))
                {
                    if (al.Count > 0)
                    {
                        string[] ResultBZ = new string[al.Count];
                        for (int i = 0; i < al.Count; i++)
                        {
                            ResultBZ[i] = al[i].ToString();
                        }

                        CanonicalNumber = Bill.ToString();

                        return ResultBZ;
                    }

                    CanonicalNumber = "";
                    return null;
                }
                else
                {
                    //组三
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
                    //组六
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
                Description = "";
                WinMoneyNoWithTax = 0;

                if ((WinMoneyList == null) || (WinMoneyList.Length < 20)) //奖金参数排列顺序(5 3 2 1星，猜大小,通二星组选,二星组选对子号,五星选123等奖
                    return -3;

                int WinCount5X = 0, WinCount3X = 0, WinCount2X = 0, WinCount1X = 0;
                int WinCountDX = 0;
                int WinCount_2X_Zu = 0, WinCount_2X_Zu_DZH = 0;
                int WinCount_5XTX_1 = 0, WinCount_5XTX_2 = 0, WinCount_5XTX_3 = 0, WinCount_2XQW_1 = 0, WinCount_2XQW_2 = 0;
                //int WinCount_3X_Zu3D = 0, WinCount_3X_Zu3F = 0, WinCount_3X_Zu6D = 0, WinCount_3X_Zu6F = 0, WinCount_3X_ZHFS = 0, WinCount_3X_ZX = 0;

                if (PlayType == PlayType_Mixed)
                    return ComputeWin_Mixed(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[0], WinMoneyList[1], WinMoneyList[2], WinMoneyList[3], WinMoneyList[4], WinMoneyList[5], WinMoneyList[6], WinMoneyList[7], WinMoneyList[8], WinMoneyList[9], WinMoneyList[10], WinMoneyList[11], WinMoneyList[12], WinMoneyList[13], WinMoneyList[14], WinMoneyList[15], WinMoneyList[16], WinMoneyList[17], WinMoneyList[18], WinMoneyList[19], WinMoneyList[20], WinMoneyList[21], WinMoneyList[22], WinMoneyList[23], WinMoneyList[24], WinMoneyList[25], WinMoneyList[26], WinMoneyList[27], WinMoneyList[28], WinMoneyList[29]);

                if (PlayType == PlayType_D)
                    return ComputeWin_D(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[0], WinMoneyList[1], WinMoneyList[2], WinMoneyList[3], WinMoneyList[4], WinMoneyList[5], WinMoneyList[6], WinMoneyList[7], ref WinCount5X, ref WinCount3X, ref WinCount2X, ref WinCount1X);

                if (PlayType == PlayType_F)
                    return ComputeWin_F(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[0], WinMoneyList[1], WinMoneyList[2], WinMoneyList[3], WinMoneyList[4], WinMoneyList[5], WinMoneyList[6], WinMoneyList[7], ref WinCount5X, ref WinCount3X, ref WinCount2X, ref WinCount1X);

                if (PlayType == PlayType_ZH)
                    return ComputeWin_ZH(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[0], WinMoneyList[1], WinMoneyList[2], WinMoneyList[3], WinMoneyList[4], WinMoneyList[5], WinMoneyList[6], WinMoneyList[7], ref WinCount5X, ref WinCount3X, ref WinCount2X, ref WinCount1X);

                if (PlayType == PlayType_DX)
                    return ComputeWin_DX(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[8], WinMoneyList[9], ref WinCountDX);

                if ((PlayType == PlayType_5X_TXD) || (PlayType == PlayType_5X_TXF))
                    return ComputeWin_5X_TX(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[14], WinMoneyList[15], WinMoneyList[16], WinMoneyList[17], WinMoneyList[18], WinMoneyList[19], ref WinCount_5XTX_1, ref WinCount_5XTX_2, ref WinCount_5XTX_3);

                if ((PlayType == PlayType_2X_ZuD) || (PlayType == PlayType_2X_ZuF))
                    return ComputeWin_2X_ZuD_ZuF(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[10], WinMoneyList[11], ref WinCount_2X_Zu);

                if (PlayType == PlayType_2X_ZuFW)
                    return ComputeWin_2X_ZuFW(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[10], WinMoneyList[11], WinMoneyList[12], WinMoneyList[13], ref WinCount_2X_Zu, ref WinCount_2X_Zu_DZH);

                if (PlayType == PlayType_2X_ZuB)
                    return ComputeWin_2X_ZuB(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[10], WinMoneyList[11], WinMoneyList[12], WinMoneyList[13], ref WinCount_2X_Zu, ref WinCount_2X_Zu_DZH);

                if (PlayType == PlayType_2X_ZuBD)
                    return ComputeWin_2X_ZuBD(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[10], WinMoneyList[11], WinMoneyList[12], WinMoneyList[13], ref WinCount_2X_Zu, ref WinCount_2X_Zu_DZH);

                if (PlayType == PlayType_3X_B)
                    return ComputeWin_3X_B(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[2], WinMoneyList[3], ref WinCount3X);

                //stone 2009-06-30 如果有则要调整奖金顺序
                if (PlayType == PlayType_2X_QW || PlayType == PlayType_2X_QWF)
                    return ComputeWin_2X_QW(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[20], WinMoneyList[21], WinMoneyList[22], WinMoneyList[23], ref WinCount_2XQW_1, ref WinCount_2XQW_2);

                //if (PlayType == PlayType_3X_Zu3D)
                //    return ComputeWin_3X_Zu3D(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[22], WinMoneyList[23], ref WinCount_3X_Zu3D);

                //if (PlayType == PlayType_3X_Zu3F)
                //    return ComputeWin_3X_Zu3F(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[22], WinMoneyList[23], ref WinCount_3X_Zu3F);

                //if (PlayType == PlayType_3X_Zu6D)
                //    return ComputeWin_3X_Zu6D(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[24], WinMoneyList[25], ref WinCount_3X_Zu6D);

                //if (PlayType == PlayType_3X_Zu6F)
                //    return ComputeWin_3X_Zu6F(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[24], WinMoneyList[25], ref WinCount_3X_Zu6F);

                //if (PlayType == PlayType_3X_ZHFS)
                //    return ComputeWin_3X_ZHFS(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[20], WinMoneyList[21], ref WinCount_3X_ZHFS);

                //if (PlayType == PlayType_3X_ZuB)
                //    return ComputeWin_3X_ZuB(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[20], WinMoneyList[21], WinMoneyList[22], WinMoneyList[23], WinMoneyList[24], WinMoneyList[25], ref WinCount_3X_ZX, ref WinCount_3X_Zu3D, ref WinCount_3X_Zu6D);

                //if (PlayType == PlayType_3X_ZuBD)
                //    return ComputeWin_3X_ZuBD(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[20], WinMoneyList[21], WinMoneyList[22], WinMoneyList[23], WinMoneyList[24], WinMoneyList[25], ref WinCount_3X_ZX, ref WinCount_3X_Zu3D, ref WinCount_3X_Zu6D);

                return -4;
            }

            #region ComputeWin  的具体方法

            private double ComputeWin_Mixed(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney1, double WinMoneyNoWithTax1, double WinMoney2, double WinMoneyNoWithTax2, double WinMoney3, double WinMoneyNoWithTax3, double WinMoney4, double WinMoneyNoWithTax4, double WinMoney5, double WinMoneyNoWithTax5, double WinMoney6, double WinMoneyNoWithTax6, double WinMoney7, double WinMoneyNoWithTax7, double WinMoney8, double WinMoneyNoWithTax8, double WinMoney9, double WinMoneyNoWithTax9, double WinMoney10, double WinMoneyNoWithTax10, double WinMoney11, double WinMoneyNoWithTax11, double WinMoney12, double WinMoneyNoWithTax12, double WinMoney13, double WinMoneyNoWithTax13, double WinMoney14, double WinMoneyNoWithTax14, double WinMoney15, double WinMoneyNoWithTax15)
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

                //奖金参数排列顺序(5 3 2 1星，猜大小,二星组选,二星组选对子号,五星通选123等奖)
                int WinCount1 = 0, WinCount2 = 0, WinCount3 = 0, WinCount4 = 0, WinCount5 = 0, WinCount6 = 0, WinCount7 = 0, WinCount8 = 0, WinCount9 = 0, WinCount10 = 0;

                for (int ii = 0; ii < Lotterys.Length; ii++)
                {
                    int WinCount5X = 0, WinCount3X = 0, WinCount2X = 0, WinCount1X = 0;
                    int WinCountDX = 0;
                    int WinCount_2X_Zu = 0, WinCount_2X_Zu_DZH = 0;
                    int WinCount_5XTX_1 = 0, WinCount_5XTX_2 = 0, WinCount_5XTX_3 = 0;

                    double t_WinMoneyNoWithTax = 0;

                    if (Lotterys[ii].StartsWith("[单式]"))
                    {
                        WinMoney += ComputeWin_D(FilterPreFix(Lotterys[ii]), WinNumber, ref Description, ref t_WinMoneyNoWithTax, WinMoney1, WinMoneyNoWithTax1, WinMoney2, WinMoneyNoWithTax2, WinMoney3, WinMoneyNoWithTax3, WinMoney4, WinMoneyNoWithTax4, ref WinCount5X, ref WinCount3X, ref WinCount2X, ref WinCount1X);

                        WinMoneyNoWithTax += t_WinMoneyNoWithTax;
                        WinCount1 += WinCount5X;
                        WinCount2 += WinCount3X;
                        WinCount3 += WinCount2X;
                        WinCount4 += WinCount1X;
                    }
                    else if (Lotterys[ii].StartsWith("[复式]"))
                    {
                        WinMoney += ComputeWin_F(FilterPreFix(Lotterys[ii]), WinNumber, ref Description, ref t_WinMoneyNoWithTax, WinMoney1, WinMoneyNoWithTax1, WinMoney2, WinMoneyNoWithTax2, WinMoney3, WinMoneyNoWithTax3, WinMoney4, WinMoneyNoWithTax4, ref WinCount5X, ref WinCount3X, ref WinCount2X, ref WinCount1X);

                        WinMoneyNoWithTax += t_WinMoneyNoWithTax;
                        WinCount1 += WinCount5X;
                        WinCount2 += WinCount3X;
                        WinCount3 += WinCount2X;
                        WinCount4 += WinCount1X;
                    }
                    else if (Lotterys[ii].StartsWith("[组合玩法]"))
                    {
                        WinMoney += ComputeWin_ZH(FilterPreFix(Lotterys[ii]), WinNumber, ref Description, ref t_WinMoneyNoWithTax, WinMoney1, WinMoneyNoWithTax1, WinMoney2, WinMoneyNoWithTax2, WinMoney3, WinMoneyNoWithTax3, WinMoney4, WinMoneyNoWithTax4, ref WinCount5X, ref WinCount3X, ref WinCount2X, ref WinCount1X);

                        WinMoneyNoWithTax += t_WinMoneyNoWithTax;
                        WinCount1 += WinCount5X;
                        WinCount2 += WinCount3X;
                        WinCount3 += WinCount2X;
                        WinCount4 += WinCount1X;
                    }
                    else if (Lotterys[ii].StartsWith("[猜大小]"))
                    {
                        WinMoney += ComputeWin_DX(FilterPreFix(Lotterys[ii]), WinNumber, ref Description, ref t_WinMoneyNoWithTax, WinMoney5, WinMoneyNoWithTax5, ref WinCountDX);

                        WinMoneyNoWithTax += t_WinMoneyNoWithTax;
                        WinCount5 += WinCountDX;
                    }
                    else if (Lotterys[ii].StartsWith("[五星通选单式]") || Lotterys[ii].StartsWith("[五星通选复式]"))
                    {
                        WinMoney += ComputeWin_5X_TX(FilterPreFix(Lotterys[ii]), WinNumber, ref Description, ref t_WinMoneyNoWithTax, WinMoney8, WinMoneyNoWithTax8, WinMoney9, WinMoneyNoWithTax9, WinMoney10, WinMoneyNoWithTax10, ref WinCount_5XTX_1, ref WinCount_5XTX_2, ref WinCount_5XTX_3);

                        WinMoneyNoWithTax += t_WinMoneyNoWithTax;
                        WinCount8 += WinCount_5XTX_1;
                        WinCount9 += WinCount_5XTX_2;
                        WinCount10 += WinCount_5XTX_3;
                    }
                    else if (Lotterys[ii].StartsWith("[二星组选单式]") || Lotterys[ii].StartsWith("[二星组选复式]"))
                    {
                        WinMoney += ComputeWin_2X_ZuD_ZuF(FilterPreFix(Lotterys[ii]), WinNumber, ref Description, ref t_WinMoneyNoWithTax, WinMoney6, WinMoneyNoWithTax6, ref WinCount_2X_Zu);

                        WinMoneyNoWithTax += t_WinMoneyNoWithTax;
                        WinCount6 += WinCount_2X_Zu;
                    }
                    else if (Lotterys[ii].StartsWith("[二星组选分位]"))
                    {
                        WinMoney += ComputeWin_2X_ZuFW(FilterPreFix(Lotterys[ii]), WinNumber, ref Description, ref t_WinMoneyNoWithTax, WinMoney6, WinMoneyNoWithTax6, WinMoney7, WinMoneyNoWithTax7, ref WinCount_2X_Zu, ref WinCount_2X_Zu_DZH);

                        WinMoneyNoWithTax += t_WinMoneyNoWithTax;
                        WinCount6 += WinCount_2X_Zu;
                        WinCount7 += WinCount_2X_Zu_DZH;
                    }
                    else if (Lotterys[ii].StartsWith("[二星组选包点]"))
                    {
                        WinMoney += ComputeWin_2X_ZuB(FilterPreFix(Lotterys[ii]), WinNumber, ref Description, ref t_WinMoneyNoWithTax, WinMoney6, WinMoneyNoWithTax6, WinMoney7, WinMoneyNoWithTax7, ref WinCount_2X_Zu, ref WinCount_2X_Zu_DZH);

                        WinMoneyNoWithTax += t_WinMoneyNoWithTax;
                        WinCount6 += WinCount_2X_Zu;
                        WinCount7 += WinCount_2X_Zu_DZH;
                    }
                    else if (Lotterys[ii].StartsWith("[二星组选包胆]"))
                    {
                        WinMoney += ComputeWin_2X_ZuBD(FilterPreFix(Lotterys[ii]), WinNumber, ref Description, ref t_WinMoneyNoWithTax, WinMoney6, WinMoneyNoWithTax6, WinMoney7, WinMoneyNoWithTax7, ref WinCount_2X_Zu, ref WinCount_2X_Zu_DZH);

                        WinMoneyNoWithTax += t_WinMoneyNoWithTax;
                        WinCount6 += WinCount_2X_Zu;
                        WinCount7 += WinCount_2X_Zu_DZH;
                    }
                    else if (Lotterys[ii].StartsWith("[三星包点]"))
                    {
                        WinMoney += ComputeWin_3X_B(FilterPreFix(Lotterys[ii]), WinNumber, ref Description, ref t_WinMoneyNoWithTax, WinMoney2, WinMoneyNoWithTax2, ref WinCount_5XTX_2);

                        WinMoneyNoWithTax += t_WinMoneyNoWithTax;
                        WinCount2 += WinCount_5XTX_2;
                    }
                }

                #region 构建中奖描述

                Description = "";

                if (WinCount1 > 0)
                {
                    MergeWinDescription(ref Description, "五星奖" + WinCount1.ToString() + "注");
                }
                if (WinCount2 > 0)
                {
                    MergeWinDescription(ref Description, "三星奖" + WinCount2.ToString() + "注");
                }
                if (WinCount3 > 0)
                {
                    MergeWinDescription(ref Description, "二星奖" + WinCount3.ToString() + "注");
                }
                if (WinCount4 > 0)
                {
                    MergeWinDescription(ref Description, "一星奖" + WinCount4.ToString() + "注");
                }
                if (WinCount5 > 0)
                {
                    MergeWinDescription(ref Description, "猜大小奖" + WinCount5.ToString() + "注");
                }
                if (WinCount6 > 0)
                {
                    MergeWinDescription(ref Description, "二星组选奖" + WinCount6.ToString() + "注");
                }
                if (WinCount7 > 0)
                {
                    MergeWinDescription(ref Description, "二星组选奖(对子号)" + WinCount7.ToString() + "注");
                }
                if (WinCount8 > 0)
                {
                    MergeWinDescription(ref Description, "五星通选一等奖" + WinCount8.ToString() + "注");
                }
                if (WinCount9 > 0)
                {
                    MergeWinDescription(ref Description, "五星通选二等奖" + WinCount9.ToString() + "注");
                }
                if (WinCount10 > 0)
                {
                    MergeWinDescription(ref Description, "五星通选三等奖" + WinCount10.ToString() + "注");
                }

                #endregion

                return WinMoney;
            }
            private double ComputeWin_D(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney1, double WinMoneyNoWithTax1, double WinMoney2, double WinMoneyNoWithTax2, double WinMoney3, double WinMoneyNoWithTax3, double WinMoney4, double WinMoneyNoWithTax4, ref int WinCount5X, ref int WinCount3X, ref int WinCount2X, ref int WinCount1X)
            {
                WinCount5X = 0;
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

                    Regex[] regex5321 = new Regex[4];
                    regex5321[0] = new Regex(@"^(\d){5}", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                    regex5321[1] = new Regex(@"^--(\d){3}", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                    regex5321[2] = new Regex(@"^---(\d){2}", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                    regex5321[3] = new Regex(@"^----\d", RegexOptions.IgnoreCase | RegexOptions.Compiled);

                    for (int i = 0; i < Lottery.Length; i++)
                    {
                        if (regex5321[0].IsMatch(Lottery[i]))
                        {
                            if (Lottery[i] == WinNumber)
                            {
                                WinCount5X++;
                                WinMoney += WinMoney1;
                                WinMoneyNoWithTax += WinMoneyNoWithTax1;

                                continue;
                            }
                        }

                        if (regex5321[1].IsMatch(Lottery[i]))
                        {
                            if (Lottery[i].Substring(2, 3) == WinNumber.Substring(2, 3))
                            {
                                WinCount3X++;
                                WinMoney += WinMoney2;
                                WinMoneyNoWithTax += WinMoneyNoWithTax2;

                                continue;
                            }
                        }

                        if (regex5321[2].IsMatch(Lottery[i]))
                        {
                            if (Lottery[i].Substring(3, 2) == WinNumber.Substring(3, 2))
                            {
                                WinCount2X++;
                                WinMoney += WinMoney3;
                                WinMoneyNoWithTax += WinMoneyNoWithTax3;

                                continue;
                            }
                        }

                        if (regex5321[3].IsMatch(Lottery[i]))
                        {
                            if (Lottery[i].Substring(4, 1) == WinNumber.Substring(4, 1))
                            {
                                WinCount1X++;
                                WinMoney += WinMoney4;
                                WinMoneyNoWithTax += WinMoneyNoWithTax4;

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
            private double ComputeWin_F(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney1, double WinMoneyNoWithTax1, double WinMoney2, double WinMoneyNoWithTax2, double WinMoney3, double WinMoneyNoWithTax3, double WinMoney4, double WinMoneyNoWithTax4, ref int WinCount5X, ref int WinCount3X, ref int WinCount2X, ref int WinCount1X)
            {
                WinCount5X = 0;
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

                    Regex[] regex5321 = new Regex[4];
                    regex5321[0] = new Regex(@"^(\d){5}", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                    regex5321[1] = new Regex(@"^--(\d){3}", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                    regex5321[2] = new Regex(@"^---(\d){2}", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                    regex5321[3] = new Regex(@"^----\d", RegexOptions.IgnoreCase | RegexOptions.Compiled);

                    for (int i = 0; i < Lottery.Length; i++)
                    {
                        if (regex5321[0].IsMatch(Lottery[i]))
                        {
                            if (Lottery[i] == WinNumber)
                            {
                                WinCount5X++;
                                WinMoney += WinMoney1;
                                WinMoneyNoWithTax += WinMoneyNoWithTax1;

                                continue;
                            }
                        }

                        if (regex5321[1].IsMatch(Lottery[i]))
                        {
                            if (Lottery[i].Substring(2, 3) == WinNumber.Substring(2, 3))
                            {
                                WinCount3X++;
                                WinMoney += WinMoney2;
                                WinMoneyNoWithTax += WinMoneyNoWithTax2;

                                continue;
                            }
                        }

                        if (regex5321[2].IsMatch(Lottery[i]))
                        {
                            if (Lottery[i].Substring(3, 2) == WinNumber.Substring(3, 2))
                            {
                                WinCount2X++;
                                WinMoney += WinMoney3;
                                WinMoneyNoWithTax += WinMoneyNoWithTax3;

                                continue;
                            }
                        }

                        if (regex5321[3].IsMatch(Lottery[i]))
                        {
                            if (Lottery[i].Substring(4, 1) == WinNumber.Substring(4, 1))
                            {
                                WinCount1X++;
                                WinMoney += WinMoney4;
                                WinMoneyNoWithTax += WinMoneyNoWithTax4;

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
            private double ComputeWin_ZH(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney1, double WinMoneyNoWithTax1, double WinMoney2, double WinMoneyNoWithTax2, double WinMoney3, double WinMoneyNoWithTax3, double WinMoney4, double WinMoneyNoWithTax4, ref int WinCount5X, ref int WinCount3X, ref int WinCount2X, ref int WinCount1X)
            {
                WinCount5X = 0;
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
                    string[] Lottery = ToSingle_ZH(Lotterys[ii], ref t_str);
                    if (Lottery == null)
                        continue;
                    if (Lottery.Length < 1)
                        continue;

                    Regex[] regex5321 = new Regex[4];
                    regex5321[0] = new Regex(@"^(\d){5}", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                    regex5321[1] = new Regex(@"^--(\d){3}", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                    regex5321[2] = new Regex(@"^---(\d){2}", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                    regex5321[3] = new Regex(@"^----\d", RegexOptions.IgnoreCase | RegexOptions.Compiled);

                    for (int i = 0; i < Lottery.Length; i++)
                    {
                        if (regex5321[0].IsMatch(Lottery[i]))
                        {
                            if (Lottery[i] == WinNumber)
                            {
                                WinCount5X++;
                                WinMoney += WinMoney1;
                                WinMoneyNoWithTax += WinMoneyNoWithTax1;

                                continue;
                            }
                        }

                        if (regex5321[1].IsMatch(Lottery[i]))
                        {
                            if (Lottery[i].Substring(2, 3) == WinNumber.Substring(2, 3))
                            {
                                WinCount3X++;
                                WinMoney += WinMoney2;
                                WinMoneyNoWithTax += WinMoneyNoWithTax2;

                                continue;
                            }
                        }

                        if (regex5321[2].IsMatch(Lottery[i]))
                        {
                            if (Lottery[i].Substring(3, 2) == WinNumber.Substring(3, 2))
                            {
                                WinCount2X++;
                                WinMoney += WinMoney3;
                                WinMoneyNoWithTax += WinMoneyNoWithTax3;

                                continue;
                            }
                        }

                        if (regex5321[3].IsMatch(Lottery[i]))
                        {
                            if (Lottery[i].Substring(4, 1) == WinNumber.Substring(4, 1))
                            {
                                WinCount1X++;
                                WinMoney += WinMoney4;
                                WinMoneyNoWithTax += WinMoneyNoWithTax4;

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

                for (int ii = 0; ii < Lotterys.Length; ii++)
                {
                    string t_str = "";
                    string[] Lottery = ToSingle_5X_TX(Lotterys[ii], ref t_str);
                    if (Lottery == null)
                        continue;
                    if (Lottery.Length < 1)
                        continue;

                    for (int i = 0; i < Lottery.Length; i++)
                    {
                        if (Lottery[i].Length < 5)
                            continue;

                        // 三个奖兼中兼得

                        if (Lottery[i] == WinNumber)
                        {
                            WinCount_5XTX_1++;
                            WinMoney += WinMoney1;
                            WinMoneyNoWithTax += WinMoneyNoWithTax1;
                        }

                        if ((Lottery[i].Substring(2, 3) == WinNumber.Substring(2, 3)) || (Lottery[i].Substring(0, 3) == WinNumber.Substring(0, 3)))
                        {
                            WinCount_5XTX_2++;
                            WinMoney += WinMoney2;
                            WinMoneyNoWithTax += WinMoneyNoWithTax2;
                        }

                        if ((Lottery[i].Substring(3, 2) == WinNumber.Substring(3, 2)) || (Lottery[i].Substring(0, 2) == WinNumber.Substring(0, 2)))
                        {
                            WinCount_5XTX_3++;
                            WinMoney += WinMoney3;
                            WinMoneyNoWithTax += WinMoneyNoWithTax3;
                        }
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
            private double ComputeWin_2X_ZuD_ZuF(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney1, double WinMoneyNoWithTax1, ref int WinCount_2X_Zu)
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
                    string[] Lottery = ToSingle_2X_ZuD_ZuF(Lotterys[ii], ref t_str);
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
            private double ComputeWin_2X_ZuFW(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney1, double WinMoneyNoWithTax1, double WinMoney2, double WinMoneyNoWithTax2, ref int WinCount_2X_Zu, ref int WinCount_2X_Zu_DZH)
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
                    string[] Lottery = ToSingle_2X_ZuFW(Lotterys[ii], ref t_str);
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
            private double ComputeWin_3X_B(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney1, double WinMoneyNoWithTax1, ref int WinCount_5XTX_2)
            {
                WinCount_5XTX_2 = 0;

                WinNumber = WinNumber.Trim();
                if (WinNumber.Length < 5)	//5: "12345"
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
                    string[] Lottery = ToSingle_3X_B(Lotterys[ii], ref t_str);
                    if (Lottery == null)
                        continue;
                    if (Lottery.Length < 1)
                        continue;

                    for (int i = 0; i < Lottery.Length; i++)
                    {
                        if (Lottery[i].Length < 3)
                            continue;

                        if (Lottery[i] == WinNumber.Substring(2, 3))
                        {
                            WinCount_5XTX_2++;
                            WinMoney += WinMoney1;
                            WinMoneyNoWithTax += WinMoneyNoWithTax1;
                        }
                    }
                }

                Description = "";

                if (WinCount_5XTX_2 > 0)
                {
                    MergeWinDescription(ref Description, "三星奖" + WinCount_5XTX_2.ToString() + "注");
                }

                if (Description != "")
                    Description += "。";

                return WinMoney;
            }
            private double ComputeWin_2X_QW(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney1, double WinMoneyNoWithTax1, double WinMoney2, double WinMoneyNoWithTax2, ref int WinCount_2XQW_1, ref int WinCount_2XQW_2)
            {
                WinCount_2XQW_1 = 0;
                WinCount_2XQW_2 = 0;

                WinNumber = WinNumber.Trim();
                if (WinNumber.Length < 5)	//5: "12345"
                    return -1;
                string[] Lotterys = SplitLotteryNumber(Number);
                if (Lotterys == null)
                    return -2;
                if (Lotterys.Length < 1)
                    return -2;

                double WinMoney = 0;
                WinMoneyNoWithTax = 0;

                WinNumber = WinNumber.Substring(2, 3);

                for (int ii = 0; ii < Lotterys.Length; ii++)
                {
                    string t_str = "";
                    string[] Lottery = ToSingle_2X_QW(Lotterys[ii], ref t_str);
                    if (Lottery == null)
                        continue;
                    if (Lottery.Length < 1)
                        continue;

                    for (int i = 0; i < Lottery.Length; i++)
                    {
                        if (Lottery[i].Length < 3)
                            continue;

                        // 两个奖兼中兼得
                        if (Lottery[i].Substring(1, 2) == WinNumber.Substring(1, 2))
                        {
                            WinCount_2XQW_2++;
                            WinMoney += WinMoney2;
                            WinMoneyNoWithTax += WinMoneyNoWithTax2;

                            if (((int.Parse(Lottery[i].Substring(0, 1)) <= 4) && (int.Parse(WinNumber.Substring(0, 1)) <= 4)) || ((int.Parse(Lottery[i].Substring(0, 1)) > 4) && (int.Parse(WinNumber.Substring(0, 1)) > 4)))
                            {
                                WinCount_2XQW_1++;
                                WinMoney += WinMoney1;
                                WinMoneyNoWithTax += WinMoneyNoWithTax1;
                            }
                        }
                    }
                }

                Description = "";

                if (WinCount_2XQW_1 > 0)
                {
                    MergeWinDescription(ref Description, "趣味二星一等奖" + WinCount_2XQW_1.ToString() + "注");
                }

                if (WinCount_2XQW_2 > 0)
                {
                    MergeWinDescription(ref Description, "趣味二星二等奖" + WinCount_2XQW_2.ToString() + "注");
                }

                if (Description != "")
                    Description += "。";

                return WinMoney;
            }
            
            //private double ComputeWin_3X_Zu3D(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney1, double WinMoneyNoWithTax1, ref int WinCount_3X_Zu3D)
            //{
            //    WinCount_3X_Zu3D = 0;

            //    WinNumber = WinNumber.Trim();
            //    if (WinNumber.Length < 5)	//5: "12345"
            //        return -1;
            //    string[] Lotterys = SplitLotteryNumber(Number);
            //    if (Lotterys == null)
            //        return -2;
            //    if (Lotterys.Length < 1)
            //        return -2;

            //    double WinMoney = 0;
            //    WinMoneyNoWithTax = 0;

            //    WinNumber = WinNumber.Substring(2, 3);

            //    for (int ii = 0; ii < Lotterys.Length; ii++)
            //    {
            //        string t_str = "";
            //        string[] Lottery = ToSingle_3X_Zu3D(Lotterys[ii], ref t_str);
            //        if (Lottery == null)
            //            continue;
            //        if (Lottery.Length < 1)
            //            continue;

            //        for (int i = 0; i < Lottery.Length; i++)
            //        {
            //            if (Lottery[i].Length < 3)
            //                continue;

            //            if (Sort(Lottery[i]) == Sort(WinNumber))
            //            {
            //                WinCount_3X_Zu3D++;
            //                WinMoney += WinMoney1;
            //                WinMoneyNoWithTax += WinMoneyNoWithTax1;
            //            }
            //        }
            //    }

            //    Description = "";

            //    if (WinCount_3X_Zu3D > 0)
            //    {
            //        MergeWinDescription(ref Description, "三星组3奖" + WinCount_3X_Zu3D.ToString() + "注");
            //    }

            //    if (Description != "")
            //        Description += "。";

            //    return WinMoney;
            //}
            //private double ComputeWin_3X_Zu3F(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney1, double WinMoneyNoWithTax1, ref int WinCount_3X_Zu3F)
            //{
            //    WinCount_3X_Zu3F = 0;

            //    WinNumber = WinNumber.Trim();
            //    if (WinNumber.Length < 5)	//5: "12345"
            //        return -1;
            //    string[] Lotterys = SplitLotteryNumber(Number);
            //    if (Lotterys == null)
            //        return -2;
            //    if (Lotterys.Length < 1)
            //        return -2;

            //    double WinMoney = 0;
            //    WinMoneyNoWithTax = 0;

            //    WinNumber = WinNumber.Substring(2, 3);

            //    for (int ii = 0; ii < Lotterys.Length; ii++)
            //    {
            //        string t_str = "";
            //        string[] Lottery = ToSingle_3X_Zu3F(Lotterys[ii], ref t_str);
            //        if (Lottery == null)
            //            continue;
            //        if (Lottery.Length < 1)
            //            continue;

            //        for (int i = 0; i < Lottery.Length; i++)
            //        {
            //            if (Lottery[i].Length < 3)
            //                continue;

            //            if (Sort(Lottery[i]) == Sort(WinNumber))
            //            {
            //                WinCount_3X_Zu3F++;
            //                WinMoney += WinMoney1;
            //                WinMoneyNoWithTax += WinMoneyNoWithTax1;
            //            }
            //        }
            //    }

            //    Description = "";

            //    if (WinCount_3X_Zu3F > 0)
            //    {
            //        MergeWinDescription(ref Description, "三星组3奖" + WinCount_3X_Zu3F.ToString() + "注");
            //    }

            //    if (Description != "")
            //        Description += "。";

            //    return WinMoney;
            //}
            //private double ComputeWin_3X_Zu6D(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney1, double WinMoneyNoWithTax1, ref int WinCount_3X_Zu6D)
            //{
            //    WinCount_3X_Zu6D = 0;

            //    WinNumber = WinNumber.Trim();
            //    if (WinNumber.Length < 5)	//5: "12345"
            //        return -1;
            //    string[] Lotterys = SplitLotteryNumber(Number);
            //    if (Lotterys == null)
            //        return -2;
            //    if (Lotterys.Length < 1)
            //        return -2;

            //    double WinMoney = 0;
            //    WinMoneyNoWithTax = 0;

            //    WinNumber = WinNumber.Substring(2, 3);

            //    for (int ii = 0; ii < Lotterys.Length; ii++)
            //    {
            //        string t_str = "";
            //        string[] Lottery = ToSingle_3X_Zu6D(Lotterys[ii], ref t_str);
            //        if (Lottery == null)
            //            continue;
            //        if (Lottery.Length < 1)
            //            continue;

            //        for (int i = 0; i < Lottery.Length; i++)
            //        {
            //            if (Lottery[i].Length < 3)
            //                continue;

            //            if (Sort(Lottery[i]) == Sort(WinNumber))
            //            {
            //                WinCount_3X_Zu6D++;
            //                WinMoney += WinMoney1;
            //                WinMoneyNoWithTax += WinMoneyNoWithTax1;
            //            }
            //        }
            //    }

            //    Description = "";

            //    if (WinCount_3X_Zu6D > 0)
            //    {
            //        MergeWinDescription(ref Description, "三星组6奖" + WinCount_3X_Zu6D.ToString() + "注");
            //    }

            //    if (Description != "")
            //        Description += "。";

            //    return WinMoney;
            //}
            //private double ComputeWin_3X_Zu6F(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney1, double WinMoneyNoWithTax1, ref int WinCount_3X_Zu6F)
            //{
            //    WinCount_3X_Zu6F = 0;

            //    WinNumber = WinNumber.Trim();
            //    if (WinNumber.Length < 5)	//5: "12345"
            //        return -1;
            //    string[] Lotterys = SplitLotteryNumber(Number);
            //    if (Lotterys == null)
            //        return -2;
            //    if (Lotterys.Length < 1)
            //        return -2;

            //    double WinMoney = 0;
            //    WinMoneyNoWithTax = 0;

            //    WinNumber = WinNumber.Substring(2, 3);

            //    for (int ii = 0; ii < Lotterys.Length; ii++)
            //    {
            //        string t_str = "";
            //        string[] Lottery = ToSingle_3X_Zu6F(Lotterys[ii], ref t_str);
            //        if (Lottery == null)
            //            continue;
            //        if (Lottery.Length < 1)
            //            continue;

            //        for (int i = 0; i < Lottery.Length; i++)
            //        {
            //            if (Lottery[i].Length < 3)
            //                continue;

            //            if (Sort(Lottery[i]) == Sort(WinNumber))
            //            {
            //                WinCount_3X_Zu6F++;
            //                WinMoney += WinMoney1;
            //                WinMoneyNoWithTax += WinMoneyNoWithTax1;
            //            }
            //        }
            //    }

            //    Description = "";

            //    if (WinCount_3X_Zu6F > 0)
            //    {
            //        MergeWinDescription(ref Description, "三星组6奖" + WinCount_3X_Zu6F.ToString() + "注");
            //    }

            //    if (Description != "")
            //        Description += "。";

            //    return WinMoney;
            //}
            //private double ComputeWin_3X_ZHFS(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney1, double WinMoneyNoWithTax1, ref int WinCount_3X_ZHFS)
            //{
            //    WinCount_3X_ZHFS = 0;

            //    WinNumber = WinNumber.Trim();
            //    if (WinNumber.Length < 5)	//5: "12345"
            //        return -1;
            //    string[] Lotterys = SplitLotteryNumber(Number);
            //    if (Lotterys == null)
            //        return -2;
            //    if (Lotterys.Length < 1)
            //        return -2;

            //    double WinMoney = 0;
            //    WinMoneyNoWithTax = 0;

            //    WinNumber = WinNumber.Substring(2, 3);

            //    for (int ii = 0; ii < Lotterys.Length; ii++)
            //    {
            //        string t_str = "";
            //        string[] Lottery = ToSingle_3X_ZHFS(Lotterys[ii], ref t_str);
            //        if (Lottery == null)
            //            continue;
            //        if (Lottery.Length < 1)
            //            continue;

            //        for (int i = 0; i < Lottery.Length; i++)
            //        {
            //            if (Lottery[i].Length < 3)
            //                continue;

            //            if (Lottery[i] == WinNumber)
            //            {
            //                WinCount_3X_ZHFS++;
            //                WinMoney += WinMoney1;
            //                WinMoneyNoWithTax += WinMoneyNoWithTax1;
            //            }
            //        }
            //    }

            //    Description = "";

            //    if (WinCount_3X_ZHFS > 0)
            //    {
            //        MergeWinDescription(ref Description, "三星直选奖" + WinCount_3X_ZHFS.ToString() + "注");
            //    }

            //    if (Description != "")
            //        Description += "。";

            //    return WinMoney;
            //}
            //private double ComputeWin_3X_ZuB(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney1, double WinMoneyNoWithTax1, double WinMoney2, double WinMoneyNoWithTax2, double WinMoney3, double WinMoneyNoWithTax3, ref int WinCount_3X_ZX, ref int WinCount_3X_Zu3D, ref int WinCount_3X_Zu6D)
            //{
            //    WinCount_3X_ZX = 0;
            //    WinCount_3X_Zu3D = 0;
            //    WinCount_3X_Zu6D = 0;

            //    WinNumber = WinNumber.Trim();
            //    if (WinNumber.Length < 5)	//5: "12345"
            //        return -1;
            //    string[] Lotterys = SplitLotteryNumber(Number);
            //    if (Lotterys == null)
            //        return -2;
            //    if (Lotterys.Length < 1)
            //        return -2;

            //    double WinMoney = 0;
            //    WinMoneyNoWithTax = 0;

            //    WinNumber = WinNumber.Substring(2, 3);

            //    for (int ii = 0; ii < Lotterys.Length; ii++)
            //    {
            //        string t_str = "";
            //        string[] Lottery = ToSingle_3X_ZuB(Lotterys[ii], ref t_str);
            //        if (Lottery == null)
            //            continue;
            //        if (Lottery.Length < 1)
            //            continue;

            //        for (int i = 0; i < Lottery.Length; i++)
            //        {
            //            if (FilterRepeated(Sort(Lottery[i])).Length == 1)
            //            {
            //                if (Sort(Lottery[i]) == Sort(WinNumber))
            //                {
            //                    WinCount_3X_ZX++;
            //                    WinMoney += WinMoney1;
            //                    WinMoneyNoWithTax += WinMoneyNoWithTax1;
            //                }

            //                continue;
            //            }

            //            if (FilterRepeated(Sort(Lottery[i])).Length == 2)
            //            {
            //                if (Sort(Lottery[i]) == Sort(WinNumber))
            //                {
            //                    WinCount_3X_Zu3D++;
            //                    WinMoney += WinMoney2;
            //                    WinMoneyNoWithTax += WinMoneyNoWithTax2;
            //                }

            //                continue;
            //            }

            //            if (Lottery[i].Length < 3)
            //                continue;

            //            if (Sort(Lottery[i]) == Sort(WinNumber))
            //            {
            //                WinCount_3X_Zu6D++;
            //                WinMoney += WinMoney3;
            //                WinMoneyNoWithTax += WinMoneyNoWithTax3;
            //            }
            //        }
            //    }

            //    Description = "";

            //    if (WinCount_3X_ZX > 0)
            //    {
            //        MergeWinDescription(ref Description, "三星直选奖" + WinCount_3X_ZX.ToString() + "注");
            //    }

            //    if (WinCount_3X_Zu3D > 0)
            //    {
            //        MergeWinDescription(ref Description, "三星组3奖" + WinCount_3X_Zu3D.ToString() + "注");
            //    }

            //    if (WinCount_3X_Zu6D > 0)
            //    {
            //        MergeWinDescription(ref Description, "三星组6奖" + WinCount_3X_Zu6D.ToString() + "注");
            //    }

            //    if (Description != "")
            //        Description += "。";

            //    return WinMoney;
            //}
            //private double ComputeWin_3X_ZuBD(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney1, double WinMoneyNoWithTax1, double WinMoney2, double WinMoneyNoWithTax2, double WinMoney3, double WinMoneyNoWithTax3, ref int WinCount_3X_ZX, ref int WinCount_3X_Zu3D, ref int WinCount_3X_Zu6D)
            //{
            //    WinCount_3X_ZX = 0;
            //    WinCount_3X_Zu3D = 0;
            //    WinCount_3X_Zu6D = 0;

            //    WinNumber = WinNumber.Trim();
            //    if (WinNumber.Length < 5)	//5: "12345"
            //        return -1;
            //    string[] Lotterys = SplitLotteryNumber(Number);
            //    if (Lotterys == null)
            //        return -2;
            //    if (Lotterys.Length < 1)
            //        return -2;

            //    double WinMoney = 0;
            //    WinMoneyNoWithTax = 0;

            //    WinNumber = WinNumber.Substring(2, 3);

            //    for (int ii = 0; ii < Lotterys.Length; ii++)
            //    {
            //        string t_str = "";
            //        string[] Lottery = ToSingle_3X_ZuBD(Lotterys[ii], ref t_str);
            //        if (Lottery == null)
            //            continue;
            //        if (Lottery.Length < 1)
            //            continue;

            //        for (int i = 0; i < Lottery.Length; i++)
            //        {
            //            if (FilterRepeated(Sort(Lottery[i])).Length == 1)
            //            {
            //                if (Sort(Lottery[i]) == Sort(WinNumber))
            //                {
            //                    WinCount_3X_ZX++;
            //                    WinMoney += WinMoney1;
            //                    WinMoneyNoWithTax += WinMoneyNoWithTax1;
            //                }

            //                continue;
            //            }

            //            if (FilterRepeated(Sort(Lottery[i])).Length == 2)
            //            {
            //                if (Sort(Lottery[i]) == Sort(WinNumber))
            //                {
            //                    WinCount_3X_Zu3D++;
            //                    WinMoney += WinMoney2;
            //                    WinMoneyNoWithTax += WinMoneyNoWithTax2;
            //                }

            //                continue;
            //            }

            //            if (Lottery[i].Length < 3)
            //                continue;

            //            if (Sort(Lottery[i]) == Sort(WinNumber))
            //            {
            //                WinCount_3X_Zu6D++;
            //                WinMoney += WinMoney3;
            //                WinMoneyNoWithTax += WinMoneyNoWithTax3;
            //            }
            //        }
            //    }

            //    Description = "";

            //    if (WinCount_3X_ZX > 0)
            //    {
            //        MergeWinDescription(ref Description, "三星直选奖" + WinCount_3X_ZX.ToString() + "注");
            //    }

            //    if (WinCount_3X_Zu3D > 0)
            //    {
            //        MergeWinDescription(ref Description, "三星组3奖" + WinCount_3X_Zu3D.ToString() + "注");
            //    }

            //    if (WinCount_3X_Zu6D > 0)
            //    {
            //        MergeWinDescription(ref Description, "三星组6奖" + WinCount_3X_Zu6D.ToString() + "注");
            //    }

            //    if (Description != "")
            //        Description += "。";

            //    return WinMoney;
            //}

            #endregion

            public override string AnalyseScheme(string Content, int PlayType)
            {
                if (PlayType == PlayType_Mixed)
                    return AnalyseScheme_Mixed(Content, PlayType);

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

                if ((PlayType == PlayType_2X_ZuD) || (PlayType == PlayType_2X_ZuF))
                    return AnalyseScheme_2X_ZuD_ZuF(Content, PlayType);

                if (PlayType == PlayType_2X_ZuFW)
                    return AnalyseScheme_2X_ZuFW(Content, PlayType);

                if (PlayType == PlayType_2X_ZuB)
                    return AnalyseScheme_2X_ZuB(Content, PlayType);

                if (PlayType == PlayType_2X_ZuBD)
                    return AnalyseScheme_2X_ZuBD(Content, PlayType);

                if (PlayType == PlayType_3X_B)
                    return AnalyseScheme_3X_B(Content, PlayType);

                //stone 2009-06-30
                if (PlayType == PlayType_3X_Zu3D)
                    return AnalyseScheme_3X_Zu3D(Content, PlayType);

                if (PlayType == PlayType_3X_Zu3F)
                    return AnalyseScheme_3X_Zu3F(Content, PlayType);

                if (PlayType == PlayType_3X_Zu6D)
                    return AnalyseScheme_3X_Zu6D(Content, PlayType);

                if (PlayType == PlayType_3X_Zu6F)
                    return AnalyseScheme_3X_Zu6F(Content, PlayType);

                if (PlayType == PlayType_3X_ZHFS)
                    return AnalyseScheme_3X_ZHFS(Content, PlayType);

                if (PlayType == PlayType_3X_ZuB)
                    return AnalyseScheme_3X_ZuB(Content, PlayType);

                if (PlayType == PlayType_3X_ZuBD)
                    return AnalyseScheme_3X_ZuBD(Content, PlayType);

                if (PlayType == PlayType_2X_QW || PlayType == PlayType_2X_QWF)
                    return AnalyseScheme_2X_QW(Content, PlayType);

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

                    if (Lotterys[ii].StartsWith("[单式]"))
                    {
                        t_Result += AnalyseScheme_D(Lotterys[ii], PlayType_D);
                    }

                    if (Lotterys[ii].StartsWith("[复式]"))
                    {
                        t_Result += AnalyseScheme_F(Lotterys[ii], PlayType_F);
                    }

                    if (Lotterys[ii].StartsWith("[组合玩法]"))
                    {
                        t_Result += AnalyseScheme_ZH(Lotterys[ii], PlayType_ZH);
                    }

                    if (Lotterys[ii].StartsWith("[猜大小]"))
                    {
                        t_Result += AnalyseScheme_DX(Lotterys[ii], PlayType_DX);
                    }

                    if (Lotterys[ii].StartsWith("[五星通选单式]"))
                    {
                        t_Result += AnalyseScheme_5X_TX(Lotterys[ii], PlayType_5X_TXD);
                    }

                    if (Lotterys[ii].StartsWith("[五星通选复式]"))
                    {
                        t_Result += AnalyseScheme_5X_TX(Lotterys[ii], PlayType_5X_TXF);
                    }

                    if (Lotterys[ii].StartsWith("[二星组选单式]"))
                    {
                        t_Result += AnalyseScheme_2X_ZuD_ZuF(Lotterys[ii], PlayType_2X_ZuD);
                    }

                    if (Lotterys[ii].StartsWith("[二星组选复式]"))
                    {
                        t_Result += AnalyseScheme_2X_ZuD_ZuF(Lotterys[ii], PlayType_2X_ZuF);
                    }

                    if (Lotterys[ii].StartsWith("[二星组选分位]"))
                    {
                        t_Result += AnalyseScheme_2X_ZuFW(Lotterys[ii], PlayType_2X_ZuFW);
                    }

                    if (Lotterys[ii].StartsWith("[二星组选包点]"))
                    {
                        t_Result += AnalyseScheme_2X_ZuB(Lotterys[ii], PlayType_2X_ZuB);
                    }

                    if (Lotterys[ii].StartsWith("[二星组选包胆]"))
                    {
                        t_Result += AnalyseScheme_2X_ZuBD(Lotterys[ii], PlayType_2X_ZuBD);
                    }

                    if (Lotterys[ii].StartsWith("[三星包点]"))
                    {
                        t_Result += AnalyseScheme_3X_B(Lotterys[ii], PlayType_3X_B);
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

                string RegexString = @"^(([\d-])|([(][\d]+?[)])){4}(([\d])|([(][\d]+?[)]))";
                Regex regex = new Regex(RegexString, RegexOptions.Compiled | RegexOptions.IgnoreCase);

                for (int i = 0; i < strs.Length; i++)
                {
                    Match m = regex.Match(strs[i]);
                    if (m.Success)
                    {
                        string CanonicalNumber = "";
                        string[] singles = ToSingle_ZH(m.Value, ref CanonicalNumber);
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
                        if ((singles.Length >= ((PlayType == PlayType_5X_TXD) ? 1 : 2)) && (singles.Length <= (MaxMoney / 2)))
                            Result += CanonicalNumber + "|" + singles.Length.ToString() + "\n";
                    }
                }

                if (Result.EndsWith("\n"))
                    Result = Result.Substring(0, Result.Length - 1);
                return Result;
            }
            private string AnalyseScheme_2X_ZuD_ZuF(string Content, int PlayType)
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
                        string[] singles = ToSingle_2X_ZuD_ZuF(m.Value, ref CanonicalNumber);
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
            private string AnalyseScheme_2X_ZuFW(string Content, int PlayType)
            {
                string[] strs = Content.Trim().Split('\n');
                if (strs == null)
                {
                    return "";
                }

                if (strs.Length == 0)
                {
                    return "";
                }

                string Result = "";

                string RegexString = @"^([(](\d){2,10}[)][(](\d){2,10}[)])|([\d][(](\d){2,10}[)])|([(](\d){2,10}[)][\d])";
                Regex regex = new Regex(RegexString, RegexOptions.Compiled | RegexOptions.IgnoreCase);

                for (int i = 0; i < strs.Length; i++)
                {
                    strs[i] = FilterPreFix(strs[i]);

                    Match m = regex.Match(strs[i]);
                    if (m.Success)
                    {
                        string CanonicalNumber = "";
                        string[] singles = ToSingle_2X_ZuFW(m.Value, ref CanonicalNumber);
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
            private string AnalyseScheme_3X_B(string Content, int PlayType)
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
                        string[] singles = ToSingle_3X_B(m.Value, ref CanonicalNumber);
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
            private string AnalyseScheme_3X_Zu3D(string Content, int PlayType)
            {
                string[] strs = Content.Split('\n');
                if (strs == null)
                    return "";
                if (strs.Length == 0)
                    return "";

                string Result = "";

                string RegexString = @"^[\d]{3}";
                Regex regex = new Regex(RegexString, RegexOptions.Compiled | RegexOptions.IgnoreCase);
                for (int i = 0; i < strs.Length; i++)
                {
                    Match m = regex.Match(strs[i]);
                    if (m.Success)
                    {
                        string CanonicalNumber = "";
                        string[] singles = ToSingle_3X_Zu3D(m.Value, ref CanonicalNumber);
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
            private string AnalyseScheme_3X_Zu3F(string Content, int PlayType)
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
                        string[] singles = ToSingle_3X_Zu3F(m.Value, ref CanonicalNumber);
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
            private string AnalyseScheme_3X_Zu6D(string Content, int PlayType)
            {
                string[] strs = Content.Split('\n');
                if (strs == null)
                    return "";
                if (strs.Length == 0)
                    return "";

                string Result = "";

                string RegexString = @"^[\d]{3}";
                Regex regex = new Regex(RegexString, RegexOptions.Compiled | RegexOptions.IgnoreCase);
                for (int i = 0; i < strs.Length; i++)
                {
                    Match m = regex.Match(strs[i]);
                    if (m.Success)
                    {
                        string CanonicalNumber = "";
                        string[] singles = ToSingle_3X_Zu6D(m.Value, ref CanonicalNumber);
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
            private string AnalyseScheme_3X_Zu6F(string Content, int PlayType)
            {
                string[] strs = Content.Split('\n');
                if (strs == null)
                    return "";
                if (strs.Length == 0)
                    return "";

                string Result = "";
                Regex regex = new Regex(@"^([\d]){3,}", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                for (int i = 0; i < strs.Length; i++)
                {
                    Match m = regex.Match(strs[i]);
                    if (m.Success)
                    {
                        string CanonicalNumber = "";
                        string[] singles = ToSingle_3X_Zu6F(m.Value, ref CanonicalNumber);
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
            private string AnalyseScheme_3X_ZHFS(string Content, int PlayType)
            {
                string[] strs = Content.Split('\n');
                if (strs == null)
                    return "";
                if (strs.Length == 0)
                    return "";

                string Result = "";
                Regex regex = new Regex(@"^([\d]){3,}", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                for (int i = 0; i < strs.Length; i++)
                {
                    Match m = regex.Match(strs[i]);
                    if (m.Success)
                    {
                        string CanonicalNumber = "";
                        string[] singles = ToSingle_3X_ZHFS(m.Value, ref CanonicalNumber);
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
            private string AnalyseScheme_3X_ZuB(string Content, int PlayType)
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
                        string[] singles = ToSingle_3X_ZuB(m.Value, ref CanonicalNumber);
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
            private string AnalyseScheme_3X_ZuBD(string Content, int PlayType)
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
                        string[] singles = ToSingle_3X_ZuBD(m.Value, ref CanonicalNumber);
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
            private string AnalyseScheme_2X_QW(string Content, int PlayType)
            {
                string[] strs = Content.Split('\n');
                if (strs == null)
                    return "";
                if (strs.Length == 0)
                    return "";

                string Result = "";

                string RegexString;
                if (PlayType == PlayType_2X_QW)
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
                        string[] singles = ToSingle_2X_QW(m.Value, ref CanonicalNumber);
                        if (singles == null)
                            continue;
                        if (singles.Length >= ((PlayType == PlayType_2X_QW) ? 1 : 2))
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

            //stone 2009-06-30 加的玩法打票相关的未处理
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
                    case "LT-E":
                        if (PlayTypeID == PlayType_D)
                        {
                            return GetPrintKeyList_LT_E_D(Numbers);
                        }
                        if (PlayTypeID == PlayType_F)
                        {
                            return GetPrintKeyList_LT_E_F(Numbers);
                        }
                        if (PlayTypeID == PlayType_ZH)
                        {
                            return GetPrintKeyList_LT_E_Zu(Numbers);
                        }
                        if (PlayTypeID == PlayType_DX)
                        {
                            return GetPrintKeyList_LT_E_DX(Numbers);
                        }
                        break;
                }

                return "";
            }
            #region GetPrintKeyList 的具体方法
            private string GetPrintKeyList_LT_E_D(string[] Numbers)
            {
                string KeyList = "";

                foreach (string Number in Numbers)
                {

                    string PrintNumber = Number.Replace(" ", "").Replace("\r", "").Replace("\n", "").Replace("-", "");

                    PrintNumber = PrintNumber.Replace("0", "O").Replace("1", "Q").Replace("2", "R").Replace("3", "1").Replace("4", "S").Replace("5", "T").Replace("6", "4").Replace("7", "U").Replace("8", "V").Replace("9", "7");

                    if (PrintNumber.Length == 1)
                    {
                        KeyList += "[Q]";
                    }
                    if (PrintNumber.Length == 2)
                    {
                        KeyList += "[R]";
                    }
                    if (PrintNumber.Length == 3)
                    {
                        KeyList += "[3]";
                    }
                    if (PrintNumber.Length == 5)
                    {
                        KeyList += "[1]";
                    }

                    foreach (char ch in PrintNumber)
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

                    string PrintNumber = Number.Replace(" ", "").Replace("\r", "").Replace("\n", "").Replace("-", "");

                    PrintNumber = PrintNumber.Replace("0", "O").Replace("1", "Q").Replace("2", "R").Replace("3", "1").Replace("4", "S").Replace("5", "T").Replace("6", "4").Replace("7", "U").Replace("8", "V").Replace("9", "7");

                    if (PrintNumber.Length == 2)
                    {
                        KeyList += "[Q]";
                    }
                    if (PrintNumber.Length == 3)
                    {
                        KeyList += "[R]";
                    }
                    if (PrintNumber.Length == 5)
                    {
                        KeyList += "[1]";
                    }

                    foreach (char ch in PrintNumber)
                    {
                        KeyList += "[" + ch.ToString() + "]";
                    }
                }

                return KeyList;
            }
            private string GetPrintKeyList_LT_E_Zu(string[] Numbers)
            {
                string KeyList = "";

                foreach (string Number in Numbers)
                {
                    int PrintNumber = 0;


                    for (int i = 0; i < Number.Length; i++)
                    {
                        if (Number.Substring(i, 1) == "-")
                        {
                            PrintNumber++;
                        }
                    }

                    if (PrintNumber == 0)
                    {
                        KeyList += "[X]";
                    }
                    if (PrintNumber == 2)
                    {
                        KeyList += "[T]";
                    }
                    if (PrintNumber == 3)
                    {
                        KeyList += "[S]";
                    }

                    KeyList += PrintNumber.ToString();

                    foreach (char ch in Number)
                    {
                        KeyList += "[" + ch.ToString() + "]";
                    }
                }

                KeyList = KeyList.Replace("0", "O").Replace("1", "Q").Replace("2", "R").Replace("3", "1").Replace("4", "S").Replace("5", "T").Replace("6", "4").Replace("7", "U").Replace("8", "V").Replace("9", "7").Replace("X", "4");


                return KeyList;
            }
            private string GetPrintKeyList_LT_E_DX(string[] Numbers)
            {
                string KeyList = "";

                foreach (string Number in Numbers)
                {

                    foreach (char ch in Number)
                    {
                        if (ch.ToString() == "小")
                        {
                            KeyList += "[Q]";
                        }
                        if (ch.ToString() == "大")
                        {
                            KeyList += "[R]";
                        }
                        if (ch.ToString() == "单")
                        {
                            KeyList += "[S]";
                        }
                        if (ch.ToString() == "双")
                        {
                            KeyList += "[双]";           //福彩机键盘码3对应于电脑键盘码1    
                        }

                        KeyList += "[" + ch.ToString() + "]";
                    }
                }

                KeyList = KeyList.Replace("0", "O").Replace("1", "Q").Replace("2", "R").Replace("3", "1").Replace("4", "S").Replace("5", "T").Replace("6", "4").Replace("7", "U").Replace("8", "V").Replace("9", "7").Replace("双", "1");

                return KeyList;
            }

            #endregion
        }
    }
}