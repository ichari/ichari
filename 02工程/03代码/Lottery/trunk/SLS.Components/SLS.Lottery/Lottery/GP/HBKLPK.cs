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
        /// 湖北快乐扑克
        /// </summary>
        public partial class HBKLPK : LotteryBase
        {
            #region 静态变量
            public const int PlayType_Mixed = 4800; //混合投注
            public const int PlayType_RX1_D = 4801; //任选1单式
            public const int PlayType_RX1_F = 4802; //任选1复式
            public const int PlayType_RX2_D = 4803; //任选2单式
            public const int PlayType_RX2_F = 4804; //任选2复式
            public const int PlayType_RX3_D = 4805; //任选3单式
            public const int PlayType_RX3_F = 4806; //任选3复式
            public const int PlayType_X4_Zu24 = 4807; //选4组选24
            public const int PlayType_X4_Zu12 = 4808; //选4组选12
            public const int PlayType_X4_Zu6 = 4809; //选4组选6
            public const int PlayType_X4_Zu4 = 4810; //选4组选4
            public const int PlayType_X4_ZhiD = 4811; //选4直选单式
            public const int PlayType_X4_ZhiF = 4812; //选4直选复式

            public const int ID = 48;
            public const string sID = "48";
            public const string Name = "湖北快乐扑克";
            public const string Code = "HBKLPK";
            public const double MaxMoney = 200000;
            #endregion

            public HBKLPK()
            {
                id = 48;
                name = "湖北快乐扑克";
                code = "HBKLPK";
            }

            public override bool CheckPlayType(int play_type)
            {
                return ((play_type >= 4800) && (play_type <= 4812));
            }

            public override PlayType[] GetPlayTypeList()
            {
                PlayType[] Result = new PlayType[13];

                Result[0] = new PlayType(PlayType_Mixed, "混合投注");
                Result[1] = new PlayType(PlayType_RX1_D, "任选一单式");
                Result[2] = new PlayType(PlayType_RX1_F, "任选一复式");
                Result[3] = new PlayType(PlayType_RX2_D, "任选二单式");
                Result[4] = new PlayType(PlayType_RX2_F, "任选二复式");
                Result[5] = new PlayType(PlayType_RX3_D, "任选三单式");
                Result[6] = new PlayType(PlayType_RX3_F, "任选三复式");
                Result[7] = new PlayType(PlayType_X4_Zu24, "选四组选24");
                Result[8] = new PlayType(PlayType_X4_Zu12, "选四组选12");
                Result[9] = new PlayType(PlayType_X4_Zu6, "选四组选6");
                Result[10] = new PlayType(PlayType_X4_Zu4, "选四组选4");
                Result[11] = new PlayType(PlayType_X4_ZhiD, "选四直选单式");
                Result[12] = new PlayType(PlayType_X4_ZhiF, "选四组选复式");

                return Result;
            }

            public override string BuildNumber(int Num, int Type)
            {
                return "";
            }

            public override string[] ToSingle(string Number, ref string CanonicalNumber, int PlayType)   //复式取单式, 后面 ref 参数是将彩票规范化，如：103(00)... 变成1030...
            {
                if (PlayType == PlayType_Mixed)
                    return ToSingle_Mixed(Number, ref CanonicalNumber);

                if ((PlayType == PlayType_RX1_D) || (PlayType == PlayType_RX1_F))
                    return ToSingle_RX1(Number, ref CanonicalNumber);

                if ((PlayType == PlayType_RX2_D) || (PlayType == PlayType_RX2_F))
                    return ToSingle_RX2(Number, ref CanonicalNumber);

                if ((PlayType == PlayType_RX3_D) || (PlayType == PlayType_RX3_F))
                    return ToSingle_RX3(Number, ref CanonicalNumber);

                if (PlayType == PlayType_X4_Zu24)
                    return ToSingle_X4_Zu24(Number, ref CanonicalNumber);

                if (PlayType == PlayType_X4_Zu12)
                    return ToSingle_X4_Zu12(Number, ref CanonicalNumber);

                if (PlayType == PlayType_X4_Zu6)
                    return ToSingle_X4_Zu6(Number, ref CanonicalNumber);

                if (PlayType == PlayType_X4_Zu4)
                    return ToSingle_X4_Zu4(Number, ref CanonicalNumber);

                if ((PlayType == PlayType_X4_ZhiD) || (PlayType == PlayType_X4_ZhiF))
                    return ToSingle_X4_Zhi(Number, ref CanonicalNumber);

                return null;
            }
            #region ToSingle 的具体方法
            private string[] ToSingle_Mixed(string Number, ref string CanonicalNumber)
            {
                CanonicalNumber = "";

                string PreFix = GetLotteryNumberPreFix(Number);

                if (Number.StartsWith("[任选1单式]") || Number.StartsWith("[任选1复式]"))
                {
                    return MergeLotteryNumberPreFix(ToSingle_RX1(FilterPreFix(Number), ref CanonicalNumber), PreFix);
                }

                if (Number.StartsWith("[任选2单式]") || Number.StartsWith("[任选2复式]"))
                {
                    return MergeLotteryNumberPreFix(ToSingle_RX2(FilterPreFix(Number), ref CanonicalNumber), PreFix);
                }

                if (Number.StartsWith("[任选3单式]") || Number.StartsWith("[任选3复式]"))
                {
                    return MergeLotteryNumberPreFix(ToSingle_RX3(FilterPreFix(Number), ref CanonicalNumber), PreFix);
                }

                if (Number.StartsWith("[选4组选24]"))
                {
                    return MergeLotteryNumberPreFix(ToSingle_X4_Zu24(FilterPreFix(Number), ref CanonicalNumber), PreFix);
                }

                if (Number.StartsWith("[选4组选12]"))
                {
                    return MergeLotteryNumberPreFix(ToSingle_X4_Zu12(FilterPreFix(Number), ref CanonicalNumber), PreFix);
                }

                if (Number.StartsWith("[选4组选6]"))
                {
                    return MergeLotteryNumberPreFix(ToSingle_X4_Zu6(FilterPreFix(Number), ref CanonicalNumber), PreFix);
                }

                if (Number.StartsWith("[选4组选4]"))
                {
                    return MergeLotteryNumberPreFix(ToSingle_X4_Zu4(FilterPreFix(Number), ref CanonicalNumber), PreFix);
                }

                if (Number.StartsWith("[选4直选单式]") || Number.StartsWith("[选4直选复式]"))
                {
                    return MergeLotteryNumberPreFix(ToSingle_X4_Zhi(FilterPreFix(Number), ref CanonicalNumber), PreFix);
                }

                return null;
            }
            private string[] ToSingle_RX1(string Number, ref string CanonicalNumber)
            {
                string[] Locate = new string[4];
                CanonicalNumber = "";
                int Count = 0;

                Regex regex = new Regex(@"^(?<L0>((([2-9])|(10)|([AJQK_]))|([(]((([2-9])|(10)|([AJQK]))[,]){1,12}(([2-9])|(10)|([AJQK]))[)])))[,](?<L1>((([2-9])|(10)|([AJQK_]))|([(]((([2-9])|(10)|([AJQK]))[,]){1,12}(([2-9])|(10)|([AJQK]))[)])))[,](?<L2>((([2-9])|(10)|([AJQK_]))|([(]((([2-9])|(10)|([AJQK]))[,]){1,12}(([2-9])|(10)|([AJQK]))[)])))[,](?<L3>((([2-9])|(10)|([AJQK_]))|([(]((([2-9])|(10)|([AJQK]))[,]){1,12}(([2-9])|(10)|([AJQK]))[)])))", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                Match m = regex.Match(Number);
                for (int i = 0; i < 4; i++)
                {
                    Locate[i] = m.Groups["L" + i.ToString()].ToString().Trim();
                    if (Locate[i] == "")
                    {
                        CanonicalNumber = "";
                        return null;
                    }
                    if (Locate[i].Length > 1 && Locate[i] != "10")
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
                    if (Locate[i].Length > 1 && Locate[i] != "10")
                        CanonicalNumber += "(" + Locate[i] + ")" + ",";
                    else
                        CanonicalNumber += Locate[i] + ",";

                    if (Locate[i] != "_")
                    {
                        Regex RegexLocate = new Regex(@"^((([2-9])|(10)|([AJQK_]))|([(]((([2-9])|(10)|([AJQK]))[,]){1,12}(([2-9])|(10)|([AJQK]))[)]))", RegexOptions.IgnoreCase | RegexOptions.Compiled);

                        if (RegexLocate.IsMatch(Locate[i]))
                        {
                            Count++;
                        }
                    }
                }

                CanonicalNumber = CanonicalNumber.Substring(0, CanonicalNumber.Length - 1);

                for (int i = 0; i < 4; i++)
                {
                    Locate[i] = Locate[i].Replace(",", "").Replace("10", "$");   //临时性的替换
                }

                ArrayList al = new ArrayList();

                #region 循环取单式
                for (int i_0 = 0; i_0 < Locate[0].Length; i_0++)
                {
                    string str_0 = Locate[0][i_0].ToString();
                    if (str_0 != "_")
                    {
                        str_0 = str_0 + ",_,_,_";
                        al.Add(str_0);
                    }
                }

                for (int i_1 = 0; i_1 < Locate[1].Length; i_1++)
                {
                    string str_1 = Locate[1][i_1].ToString();
                    if (str_1 != "_")
                    {
                        str_1 = "_," + str_1 + ",_,_";
                        al.Add(str_1);
                    }
                }

                for (int i_2 = 0; i_2 < Locate[2].Length; i_2++)
                {
                    string str_2 = Locate[2][i_2].ToString();
                    if (str_2 != "_")
                    {
                        str_2 = "_,_," + str_2 + ",_";
                        al.Add(str_2);
                    }
                }

                for (int i_3 = 0; i_3 < Locate[3].Length; i_3++)
                {
                    string str_3 = Locate[3][i_3].ToString();
                    if (str_3 != "_")
                    {
                        str_3 = "_,_,_," + str_3;
                        al.Add(str_3);
                    }
                }


                #endregion

                string[] Result = new string[al.Count];
                for (int i = 0; i < al.Count; i++)
                    Result[i] = al[i].ToString().Replace("$", "10");
                return Result;
            }
            private string[] ToSingle_RX2(string Number, ref string CanonicalNumber)
            {
                string[] Locate = new string[4];
                CanonicalNumber = "";

                Regex regex = new Regex(@"^(?<L0>((([2-9])|(10)|([AJQK_]))|([(]((([2-9])|(10)|([AJQK]))[,]){1,12}(([2-9])|(10)|([AJQK]))[)])))[,](?<L1>((([2-9])|(10)|([AJQK_]))|([(]((([2-9])|(10)|([AJQK]))[,]){1,12}(([2-9])|(10)|([AJQK]))[)])))[,](?<L2>((([2-9])|(10)|([AJQK_]))|([(]((([2-9])|(10)|([AJQK]))[,]){1,12}(([2-9])|(10)|([AJQK]))[)])))[,](?<L3>((([2-9])|(10)|([AJQK_]))|([(]((([2-9])|(10)|([AJQK]))[,]){1,12}(([2-9])|(10)|([AJQK]))[)])))", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                Match m = regex.Match(Number);
                for (int i = 0; i < 4; i++)
                {
                    Locate[i] = m.Groups["L" + i.ToString()].ToString().Trim();
                    if (Locate[i] == "")
                    {
                        CanonicalNumber = "";
                        return null;
                    }
                    if (Locate[i].Length > 1 && Locate[i] != "10")
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
                    if (Locate[i].Length > 1 && Locate[i] != "10")
                        CanonicalNumber += "(" + Locate[i] + ")" + ",";
                    else
                        CanonicalNumber += Locate[i] + ",";
                }

                CanonicalNumber = CanonicalNumber.Substring(0, CanonicalNumber.Length - 1);

                for (int i = 0; i < 4; i++)
                {
                    Locate[i] = Locate[i].Replace(",", "").Replace("10", "$");  //临时性的替换

                    if (Locate[i] != "_")
                    {
                        Locate[i] += "_";
                    }
                }

                ArrayList al = new ArrayList();

                #region 循环取单式
                for (int i_0 = 0; i_0 < Locate[0].Length; i_0++)
                {
                    string str_0 = Locate[0][i_0].ToString();
                    for (int i_1 = 0; i_1 < Locate[1].Length; i_1++)
                    {
                        string str_1 = str_0 + "," + Locate[1][i_1].ToString();
                        for (int i_2 = 0; i_2 < Locate[2].Length; i_2++)
                        {
                            string str_2 = str_1 + "," + Locate[2][i_2].ToString();
                            for (int i_3 = 0; i_3 < Locate[3].Length; i_3++)
                            {
                                string str_3 = str_2 + "," + Locate[3][i_3].ToString();

                                if (str_3.Replace("_", "").Length != 5)
                                {
                                    continue;
                                }
                                al.Add(str_3);
                            }
                        }
                    }
                }
                #endregion

                string[] Result = new string[al.Count];
                for (int i = 0; i < al.Count; i++)
                    Result[i] = al[i].ToString().Replace("$", "10");
                return Result;
            }
            private string[] ToSingle_RX3(string Number, ref string CanonicalNumber)
            {
                string[] Locate = new string[4];
                CanonicalNumber = "";
                //int Count = 0;

                Regex regex = new Regex(@"^(?<L0>((([2-9])|(10)|([AJQK_]))|([(]((([2-9])|(10)|([AJQK]))[,]){1,12}(([2-9])|(10)|([AJQK]))[)])))[,](?<L1>((([2-9])|(10)|([AJQK_]))|([(]((([2-9])|(10)|([AJQK]))[,]){1,12}(([2-9])|(10)|([AJQK]))[)])))[,](?<L2>((([2-9])|(10)|([AJQK_]))|([(]((([2-9])|(10)|([AJQK]))[,]){1,12}(([2-9])|(10)|([AJQK]))[)])))[,](?<L3>((([2-9])|(10)|([AJQK_]))|([(]((([2-9])|(10)|([AJQK]))[,]){1,12}(([2-9])|(10)|([AJQK]))[)])))", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                Match m = regex.Match(Number);
                for (int i = 0; i < 4; i++)
                {
                    Locate[i] = m.Groups["L" + i.ToString()].ToString().Trim();
                    if (Locate[i] == "")
                    {
                        CanonicalNumber = "";
                        return null;
                    }
                    if (Locate[i].Length > 1 && Locate[i] != "10")
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
                    if (Locate[i].Length > 1 && Locate[i] != "10")
                        CanonicalNumber += "(" + Locate[i] + ")" + ",";
                    else
                        CanonicalNumber += Locate[i] + ",";
                }

                CanonicalNumber = CanonicalNumber.Substring(0, CanonicalNumber.Length - 1);

                for (int i = 0; i < 4; i++)
                {
                    Locate[i] = Locate[i].Replace(",", "").Replace("10", "$");  //临时性的替换

                    if (Locate[i] != "_")
                    {
                        Locate[i] = Locate[i] + "_";
                    }
                }

                ArrayList al = new ArrayList();

                #region 循环取单式
                for (int i_0 = 0; i_0 < Locate[0].Length; i_0++)
                {
                    string str_0 = Locate[0][i_0].ToString();
                    for (int i_1 = 0; i_1 < Locate[1].Length; i_1++)
                    {
                        string str_1 = str_0 + "," + Locate[1][i_1].ToString();
                        for (int i_2 = 0; i_2 < Locate[2].Length; i_2++)
                        {
                            string str_2 = str_1 + "," + Locate[2][i_2].ToString();
                            for (int i_3 = 0; i_3 < Locate[3].Length; i_3++)
                            {
                                string str_3 = str_2 + "," + Locate[3][i_3].ToString();

                                if (str_3.Replace("_", "").Length != 6)
                                {
                                    continue;
                                }
                                al.Add(str_3);
                            }
                        }
                    }
                }
                #endregion

                string[] Result = new string[al.Count];
                for (int i = 0; i < al.Count; i++)
                    Result[i] = al[i].ToString().Replace("$", "10");
                return Result;
            }
            private string[] ToSingle_X4_Zu24(string Number, ref string CanonicalNumber)
            {
                CanonicalNumber = FilterRepeated(Number.Trim());

                string[] strs = CanonicalNumber.Split(',');

                if (strs.Length < 4)
                {
                    CanonicalNumber = "";
                    return null;
                }

                ArrayList al = new ArrayList();
                int n = strs.Length;
                #region 循环取单式
                for (int i = 0; i < n - 3; i++)
                {
                    for (int j = i + 1; j < n - 2; j++)
                    {
                        for (int k = j + 1; k < n - 1; k++)
                        {
                            for (int l = k + 1; l < n; l++)
                            {
                                al.Add(strs[i].ToString() + "," + strs[j].ToString() + "," + strs[k].ToString() + "," + strs[l].ToString());
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
            private string[] ToSingle_X4_Zu12(string Number, ref string CanonicalNumber)
            {
                CanonicalNumber = "";

                if (FilterRepeated(Number).Split(',').Length != 3)
                {
                    CanonicalNumber = "";
                    return null;
                }

                Regex regex = new Regex(@"^(?<L0>(([2-9])|(10)|([AJQK_])))[,](?<L1>(([2-9])|(10)|([AJQK_])))[,](?<L2>(([2-9])|(10)|([AJQK_])))[,](?<L3>(([2-9])|(10)|([AJQK_])))", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                Match m = regex.Match(Number);
                for (int i = 0; i < 4; i++)
                {
                    string Locate = m.Groups["L" + i.ToString()].ToString().Trim();
                    if (Locate == "")
                    {
                        CanonicalNumber = "";
                        return null;
                    }

                    CanonicalNumber += Locate + ",";
                }

                CanonicalNumber = CanonicalNumber.Substring(0, CanonicalNumber.Length - 1);
                string[] Result = new string[1];
                Result[0] = CanonicalNumber;

                return Result;
            }
            private string[] ToSingle_X4_Zu6(string Number, ref string CanonicalNumber)
            {
                CanonicalNumber = "";

                if (FilterRepeated(Number).Split(',').Length != 2)
                {
                    CanonicalNumber = "";
                    return null;
                }

                if (((Number.Split(',')[0] != Number.Split(',')[1]) || (Number.Split(',')[2] != Number.Split(',')[3])) && ((Number.Split(',')[0] != Number.Split(',')[3]) || (Number.Split(',')[1] != Number.Split(',')[2])) && ((Number.Split(',')[0] != Number.Split(',')[2]) || (Number.Split(',')[1] != Number.Split(',')[3])))
                {
                    CanonicalNumber = "";
                    return null;
                }

                Regex regex = new Regex(@"^(?<L0>(([2-9])|(10)|([AJQK_])))[,](?<L1>(([2-9])|(10)|([AJQK_])))[,](?<L2>(([2-9])|(10)|([AJQK_])))[,](?<L3>(([2-9])|(10)|([AJQK_])))", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                Match m = regex.Match(Number);
                for (int i = 0; i < 4; i++)
                {
                    string Locate = m.Groups["L" + i.ToString()].ToString().Trim();
                    if (Locate == "")
                    {
                        CanonicalNumber = "";
                        return null;
                    }

                    CanonicalNumber += Locate + ",";
                }

                CanonicalNumber = CanonicalNumber.Substring(0, CanonicalNumber.Length - 1);
                string[] Result = new string[1];
                Result[0] = CanonicalNumber;

                return Result;
            }
            private string[] ToSingle_X4_Zu4(string Number, ref string CanonicalNumber)
            {
                CanonicalNumber = "";

                if (FilterRepeated(Number).Split(',').Length != 2)
                {
                    CanonicalNumber = "";
                    return null;
                }

                if (((Number.Split(',')[0] != Number.Split(',')[1]) || (Number.Split(',')[1] != Number.Split(',')[2])) && ((Number.Split(',')[1] != Number.Split(',')[2]) || (Number.Split(',')[2] != Number.Split(',')[3])) && ((Number.Split(',')[0] != Number.Split(',')[2]) || (Number.Split(',')[2] != Number.Split(',')[3])) && ((Number.Split(',')[0] != Number.Split(',')[1]) || (Number.Split(',')[1] != Number.Split(',')[3])))
                {
                    CanonicalNumber = "";
                    return null;
                }

                Regex regex = new Regex(@"^(?<L0>(([2-9])|(10)|([AJQK_])))[,](?<L1>(([2-9])|(10)|([AJQK_])))[,](?<L2>(([2-9])|(10)|([AJQK_])))[,](?<L3>(([2-9])|(10)|([AJQK_])))", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                Match m = regex.Match(Number);
                for (int i = 0; i < 4; i++)
                {
                    string Locate = m.Groups["L" + i.ToString()].ToString().Trim();
                    if (Locate == "")
                    {
                        CanonicalNumber = "";
                        return null;
                    }

                    CanonicalNumber += Locate + ",";
                }

                CanonicalNumber = CanonicalNumber.Substring(0, CanonicalNumber.Length - 1);
                string[] Result = new string[1];
                Result[0] = CanonicalNumber;

                return Result;
            }
            private string[] ToSingle_X4_Zhi(string Number, ref string CanonicalNumber)
            {
                string[] Locate = new string[4];
                CanonicalNumber = "";

                Regex regex = new Regex(@"^(?<L0>((([2-9])|(10)|([AJQK]))|([(]((([2-9])|(10)|([AJQK]))[,]){1,12}(([2-9])|(10)|([AJQK]))[)])))[,](?<L1>((([2-9])|(10)|([AJQK]))|([(]((([2-9])|(10)|([AJQK]))[,]){1,12}(([2-9])|(10)|([AJQK]))[)])))[,](?<L2>((([2-9])|(10)|([AJQK]))|([(]((([2-9])|(10)|([AJQK]))[,]){1,12}(([2-9])|(10)|([AJQK]))[)])))[,](?<L3>((([2-9])|(10)|([AJQK]))|([(]((([2-9])|(10)|([AJQK]))[,]){1,12}(([2-9])|(10)|([AJQK]))[)])))", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                Match m = regex.Match(Number);
                for (int i = 0; i < 4; i++)
                {
                    Locate[i] = m.Groups["L" + i.ToString()].ToString().Trim();
                    if (Locate[i] == "")
                    {
                        CanonicalNumber = "";
                        return null;
                    }
                    if (Locate[i].Length > 1 && Locate[i] != "10")
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
                    if (Locate[i].Length > 1 && Locate[i] != "10")
                        CanonicalNumber += "(" + Locate[i] + ")" + ",";
                    else
                        CanonicalNumber += Locate[i] + ",";
                }

                CanonicalNumber = CanonicalNumber.Substring(0, CanonicalNumber.Length - 1);

                for (int i = 0; i < 4; i++)
                {
                    Locate[i] = Locate[i].Replace(",", "").Replace("10", "$");  //临时性的替换
                }

                ArrayList al = new ArrayList();

                #region 循环取单式
                for (int i_0 = 0; i_0 < Locate[0].Length; i_0++)
                {
                    string str_0 = Locate[0][i_0].ToString();
                    for (int i_1 = 0; i_1 < Locate[1].Length; i_1++)
                    {
                        string str_1 = str_0 + "," + Locate[1][i_1].ToString();
                        for (int i_2 = 0; i_2 < Locate[2].Length; i_2++)
                        {
                            string str_2 = str_1 + "," + Locate[2][i_2].ToString();
                            for (int i_3 = 0; i_3 < Locate[3].Length; i_3++)
                            {
                                string str_3 = str_2 + "," + Locate[3][i_3].ToString();
                                al.Add(str_3);
                            }
                        }
                    }
                }
                #endregion

                string[] Result = new string[al.Count];
                for (int i = 0; i < al.Count; i++)
                    Result[i] = al[i].ToString().Replace("$", "10");
                return Result;
            }
            #endregion

            public override double ComputeWin(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, int PlayType, params double[] WinMoneyList)
            {
                Description = "";
                WinMoneyNoWithTax = 0;

                if ((WinMoneyList == null) || (WinMoneyList.Length < 20)) //奖金参数排列顺序RX1(0,1),RX2(2,3),RX3中3(4,5),RX3中2(6,7),X4中4(8,9),X4中3(10,11),X4Zu24(12,13),X4Zu12(14,15),X4Zu6(16,17),X4Zu4(18,19)
                    return -3;

                if (!AnalyseWinNumber(WinNumber))
                {
                    return -5;
                }

                int WinCount = 0;
                int WinCount_1 = 0;
                int WinCount_2 = 0;

                if (PlayType == PlayType_Mixed)   //混合投注
                    return ComputeWin_Mixed(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[0], WinMoneyList[1], WinMoneyList[2], WinMoneyList[3], WinMoneyList[4], WinMoneyList[5], WinMoneyList[6], WinMoneyList[7], WinMoneyList[8], WinMoneyList[9], WinMoneyList[10], WinMoneyList[11], WinMoneyList[12], WinMoneyList[13], WinMoneyList[14], WinMoneyList[15], WinMoneyList[16], WinMoneyList[17], WinMoneyList[18], WinMoneyList[19]);

                if ((PlayType == PlayType_RX1_D) || (PlayType == PlayType_RX1_F))
                    return ComputeWin_RX1(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[0], WinMoneyList[1], ref WinCount);

                if ((PlayType == PlayType_RX2_D) || (PlayType == PlayType_RX2_F))
                    return ComputeWin_RX2(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[2], WinMoneyList[3], ref WinCount);

                if ((PlayType == PlayType_RX3_D) || (PlayType == PlayType_RX3_F))
                    return ComputeWin_RX3(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[4], WinMoneyList[5], WinMoneyList[6], WinMoneyList[7], ref WinCount_1, ref WinCount_2);

                if ((PlayType == PlayType_X4_ZhiD) || (PlayType == PlayType_X4_ZhiF))
                    return ComputeWin_X4_Zhi(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[8], WinMoneyList[9], WinMoneyList[10], WinMoneyList[11], ref WinCount_1, ref WinCount_2);

                if (PlayType == PlayType_X4_Zu24)
                    return ComputeWin_X4_Zu24(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[12], WinMoneyList[13], ref WinCount);

                if (PlayType == PlayType_X4_Zu12)
                    return ComputeWin_X4_Zu12(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[14], WinMoneyList[15], ref WinCount);

                if (PlayType == PlayType_X4_Zu6)
                    return ComputeWin_X4_Zu6(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[16], WinMoneyList[17], ref WinCount);

                if (PlayType == PlayType_X4_Zu4)
                    return ComputeWin_X4_Zu4(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[18], WinMoneyList[19], ref WinCount);

                return -4;
            }
            #region ComputeWin 的具体方法
            private double ComputeWin_Mixed(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney1, double WinMoneyNoWithTax1, double WinMoney2, double WinMoneyNoWithTax2, double WinMoney3, double WinMoneyNoWithTax3, double WinMoney4, double WinMoneyNoWithTax4, double WinMoney5, double WinMoneyNoWithTax5, double WinMoney6, double WinMoneyNoWithTax6, double WinMoney7, double WinMoneyNoWithTax7, double WinMoney8, double WinMoneyNoWithTax8, double WinMoney9, double WinMoneyNoWithTax9, double WinMoney10, double WinMoneyNoWithTax10)
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

                int WinCount1 = 0, WinCount2 = 0, WinCount3 = 0, WinCount4 = 0, WinCount5 = 0, WinCount6 = 0, WinCount7 = 0, WinCount8 = 0, WinCount9 = 0, WinCount10 = 0;

                for (int ii = 0; ii < Lotterys.Length; ii++)
                {
                    int WinCount = 0;
                    int WinCount_1 = 0;
                    int WinCount_2 = 0;

                    double t_WinMoneyNoWithTax = 0;

                    if (Lotterys[ii].StartsWith("[任选一单式]") || Lotterys[ii].StartsWith("[任选一复式]"))
                    {
                        WinMoney += ComputeWin_RX1(FilterPreFix(Lotterys[ii]), WinNumber, ref Description, ref t_WinMoneyNoWithTax, WinMoney1, WinMoneyNoWithTax1, ref WinCount);

                        WinMoneyNoWithTax += t_WinMoneyNoWithTax;
                        WinCount1 += WinCount;
                    }
                    else if (Lotterys[ii].StartsWith("[任选二单式]") || Lotterys[ii].StartsWith("[任选二复式]"))
                    {
                        WinMoney += ComputeWin_RX2(FilterPreFix(Lotterys[ii]), WinNumber, ref Description, ref t_WinMoneyNoWithTax, WinMoney2, WinMoneyNoWithTax2, ref WinCount);

                        WinMoneyNoWithTax += t_WinMoneyNoWithTax;
                        WinCount2 += WinCount;
                    }
                    else if (Lotterys[ii].StartsWith("[任选三单式]") || Lotterys[ii].StartsWith("[任选三复式]"))
                    {
                        WinMoney += ComputeWin_RX3(FilterPreFix(Lotterys[ii]), WinNumber, ref Description, ref t_WinMoneyNoWithTax, WinMoney3, WinMoneyNoWithTax3, WinMoney4, WinMoneyNoWithTax4, ref WinCount_1, ref WinCount_2);

                        WinMoneyNoWithTax += t_WinMoneyNoWithTax;
                        WinCount3 += WinCount_1;
                        WinCount4 += WinCount_2;
                    }
                    else if (Lotterys[ii].StartsWith("[选四直选单式]") || Lotterys[ii].StartsWith("[选四直选复式]"))
                    {
                        WinMoney += ComputeWin_X4_Zhi(FilterPreFix(Lotterys[ii]), WinNumber, ref Description, ref t_WinMoneyNoWithTax, WinMoney5, WinMoneyNoWithTax5, WinMoney6, WinMoneyNoWithTax6, ref WinCount_1, ref WinCount_2);

                        WinMoneyNoWithTax += t_WinMoneyNoWithTax;
                        WinCount5 += WinCount_1;
                        WinCount6 += WinCount_2;
                    }
                    else if (Lotterys[ii].StartsWith("[选四组选24]"))
                    {
                        WinMoney += ComputeWin_X4_Zu24(FilterPreFix(Lotterys[ii]), WinNumber, ref Description, ref t_WinMoneyNoWithTax, WinMoney7, WinMoneyNoWithTax7, ref WinCount);

                        WinMoneyNoWithTax += t_WinMoneyNoWithTax;
                        WinCount7 += WinCount;
                    }
                    else if (Lotterys[ii].StartsWith("[选四组选12]"))
                    {
                        WinMoney += ComputeWin_X4_Zu12(FilterPreFix(Lotterys[ii]), WinNumber, ref Description, ref t_WinMoneyNoWithTax, WinMoney8, WinMoneyNoWithTax8, ref WinCount);

                        WinMoneyNoWithTax += t_WinMoneyNoWithTax;
                        WinCount8 += WinCount;
                    }
                    else if (Lotterys[ii].StartsWith("[选四组选6]"))
                    {
                        WinMoney += ComputeWin_X4_Zu6(FilterPreFix(Lotterys[ii]), WinNumber, ref Description, ref t_WinMoneyNoWithTax, WinMoney9, WinMoneyNoWithTax9, ref WinCount);

                        WinMoneyNoWithTax += t_WinMoneyNoWithTax;
                        WinCount9 += WinCount;
                    }
                    else if (Lotterys[ii].StartsWith("[选四组选4]"))
                    {
                        WinMoney += ComputeWin_X4_Zu4(FilterPreFix(Lotterys[ii]), WinNumber, ref Description, ref t_WinMoneyNoWithTax, WinMoney10, WinMoneyNoWithTax10, ref WinCount);

                        WinMoneyNoWithTax += t_WinMoneyNoWithTax;
                        WinCount10 += WinCount;
                    }
                }

                Description = "";

                if (WinCount1 > 0)
                {
                    MergeWinDescription(ref Description, "任选一" + WinCount1.ToString() + "注");
                }
                if (WinCount2 > 0)
                {
                    MergeWinDescription(ref Description, "任选二" + WinCount2.ToString() + "注");
                }
                if (WinCount3 > 0)
                {
                    MergeWinDescription(ref Description, "任选三中3 " + WinCount3.ToString() + "注");
                }
                if (WinCount4 > 0)
                {
                    MergeWinDescription(ref Description, "任选三中2 " + WinCount4.ToString() + "注");
                }
                if (WinCount5 > 0)
                {
                    MergeWinDescription(ref Description, "选四中4 " + WinCount5.ToString() + "注");
                }
                if (WinCount6 > 0)
                {
                    MergeWinDescription(ref Description, "选四中3 " + WinCount6.ToString() + "注");
                }
                if (WinCount7 > 0)
                {
                    MergeWinDescription(ref Description, "选四组选24 " + WinCount7.ToString() + "注");
                }
                if (WinCount8 > 0)
                {
                    MergeWinDescription(ref Description, "选四组选12 " + WinCount8.ToString() + "注");
                }
                if (WinCount9 > 0)
                {
                    MergeWinDescription(ref Description, "选四组选6 " + WinCount9.ToString() + "注");
                }
                if (WinCount10 > 0)
                {
                    MergeWinDescription(ref Description, "选四组选4 " + WinCount10.ToString() + "注");
                }

                return WinMoney;
            }
            private double ComputeWin_RX1(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney1, double WinMoneyNoWithTax1, ref int WinCount)	//计算中奖
            {
                WinCount = 0;

                WinNumber = WinNumber.Trim();

                if (WinNumber.Split(',').Length != 4)  //4: "K,J,9,A"
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
                    string[] Lottery = ToSingle_RX1(Lotterys[ii], ref t_str);
                    if (Lottery == null)
                        continue;
                    if (Lottery.Length < 1)
                        continue;

                    for (int i = 0; i < Lottery.Length; i++)
                    {
                        if (Lottery[i].Split(',').Length < 4)
                            continue;
                        for (int j = 0; j < 4; j++)
                        {
                            if (Lottery[i].Split(',')[j] == WinNumber.Split(',')[j])
                            {
                                WinCount++;
                                WinMoney += WinMoney1;
                                WinMoneyNoWithTax += WinMoneyNoWithTax1;
                            }
                        }
                    }
                }

                if (WinCount > 0)
                {
                    Description = "任选一" + WinCount.ToString() + "注。";
                }

                return WinMoney;
            }
            private double ComputeWin_RX2(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney1, double WinMoneyNoWithTax1, ref int WinCount)	//计算中奖
            {
                int Count = 0;

                WinCount = 0;

                WinNumber = WinNumber.Trim();
                if (WinNumber.Split(',').Length != 4)  //4: "K,J,9,A"
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
                    string[] Lottery = ToSingle_RX2(Lotterys[ii], ref t_str);
                    if (Lottery == null)
                        continue;
                    if (Lottery.Length < 1)
                        continue;

                    for (int i = 0; i < Lottery.Length; i++)
                    {
                        if (Lottery[i].Split(',').Length < 4)
                            continue;
                        Count = 0;
                        for (int j = 0; j < 4; j++)
                        {
                            if (Lottery[i].Split(',')[j] == WinNumber.Split(',')[j])
                            {
                                Count++;
                            }
                        }

                        if (Count == 2)
                        {
                            WinCount++;
                            WinMoney += WinMoney1;
                            WinMoneyNoWithTax += WinMoneyNoWithTax1;
                        }
                    }
                }

                if (WinCount > 0)
                {
                    Description = "任选二" + WinCount.ToString() + "注。";
                }

                return WinMoney;
            }
            private double ComputeWin_RX3(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney1, double WinMoneyNoWithTax1, double WinMoney2, double WinMoneyNoWithTax2, ref int WinCount_1, ref int WinCount_2)	//计算中奖
            {
                int Count = 0;

                WinCount_1 = 0;
                WinCount_2 = 0;

                WinNumber = WinNumber.Trim();
                if (WinNumber.Split(',').Length != 4)  //4: "K,J,9,A"
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
                    string[] Lottery = ToSingle_RX3(Lotterys[ii], ref t_str);
                    if (Lottery == null)
                        continue;
                    if (Lottery.Length < 1)
                        continue;

                    for (int i = 0; i < Lottery.Length; i++)
                    {
                        if (Lottery[i].Split(',').Length < 4)
                            continue;

                        Count = 0;
                        for (int j = 0; j < 4; j++)
                        {
                            if (Lottery[i].Split(',')[j] == WinNumber.Split(',')[j])
                            {
                                Count++;
                            }
                        }

                        if (Count == 3)
                        {
                            WinCount_1++;
                            WinMoney += WinMoney1;
                            WinMoneyNoWithTax += WinMoneyNoWithTax1;
                        }

                        if (Count == 2)
                        {
                            WinCount_2++;
                            WinMoney += WinMoney2;
                            WinMoneyNoWithTax += WinMoneyNoWithTax2;
                        }
                    }
                }

                if (WinCount_1 > 0)
                {
                    MergeWinDescription(ref Description, "任选三中3 " + WinCount_1.ToString() + "注");
                }
                if (WinCount_2 > 0)
                {
                    MergeWinDescription(ref Description, "任选三中2 " + WinCount_2.ToString() + "注");
                }

                if (Description != "")
                    Description += "。";

                return WinMoney;
            }
            private double ComputeWin_X4_Zu24(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney1, double WinMoneyNoWithTax1, ref int WinCount)	//计算中奖
            {
                WinCount = 0;

                WinNumber = WinNumber.Trim();
                if (WinNumber.Split(',').Length != 4)  //4: "K,J,9,A"
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
                    string[] Lottery = ToSingle_X4_Zu24(Lotterys[ii], ref t_str);
                    if (Lottery == null)
                        continue;
                    if (Lottery.Length < 1)
                        continue;

                    for (int i = 0; i < Lottery.Length; i++)
                    {
                        if (Lottery[i].Split(',').Length < 4)
                            continue;
                        if (SortKLPK(Lottery[i]) == SortKLPK(WinNumber))
                        {
                            WinCount++;
                            WinMoney += WinMoney1;
                            WinMoneyNoWithTax += WinMoneyNoWithTax1;
                        }
                    }
                }

                if (WinCount > 0)
                {
                    Description = "选四组选24 " + WinCount.ToString() + "注。";
                }

                return WinMoney;
            }
            private double ComputeWin_X4_Zu12(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney1, double WinMoneyNoWithTax1, ref int WinCount)	//计算中奖
            {
                WinCount = 0;

                WinNumber = WinNumber.Trim();
                if (WinNumber.Split(',').Length != 4)  //4: "K,J,9,A"
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
                    string[] Lottery = ToSingle_X4_Zu12(Lotterys[ii], ref t_str);
                    if (Lottery == null)
                        continue;
                    if (Lottery.Length < 1)
                        continue;

                    for (int i = 0; i < Lottery.Length; i++)
                    {
                        if (Lottery[i].Split(',').Length < 4)
                            continue;
                        if (SortKLPK(Lottery[i]) == SortKLPK(WinNumber))
                        {
                            WinCount++;
                            WinMoney += WinMoney1;
                            WinMoneyNoWithTax += WinMoneyNoWithTax1;
                        }
                    }
                }

                if (WinCount > 0)
                {
                    Description = "选四组选12 " + WinCount.ToString() + "注。";
                }

                return WinMoney;
            }
            private double ComputeWin_X4_Zu6(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney1, double WinMoneyNoWithTax1, ref int WinCount)	//计算中奖
            {
                WinCount = 0;

                WinNumber = WinNumber.Trim();
                if (WinNumber.Split(',').Length != 4)  //4: "K,J,9,A"
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
                    string[] Lottery = ToSingle_X4_Zu6(Lotterys[ii], ref t_str);
                    if (Lottery == null)
                        continue;
                    if (Lottery.Length < 1)
                        continue;

                    for (int i = 0; i < Lottery.Length; i++)
                    {
                        if (Lottery[i].Split(',').Length < 4)
                            continue;
                        if (SortKLPK(Lottery[i]) == SortKLPK(WinNumber))
                        {
                            WinCount++;
                            WinMoney += WinMoney1;
                            WinMoneyNoWithTax += WinMoneyNoWithTax1;
                        }
                    }
                }

                if (WinCount > 0)
                {
                    Description = "选四组选6 " + WinCount.ToString() + "注。";
                }

                return WinMoney;
            }
            private double ComputeWin_X4_Zu4(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney1, double WinMoneyNoWithTax1, ref int WinCount)	//计算中奖
            {
                WinCount = 0;

                WinNumber = WinNumber.Trim();
                if (WinNumber.Split(',').Length != 4)  //4: "K,J,9,A"
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
                    string[] Lottery = ToSingle_X4_Zu4(Lotterys[ii], ref t_str);
                    if (Lottery == null)
                        continue;
                    if (Lottery.Length < 1)
                        continue;

                    for (int i = 0; i < Lottery.Length; i++)
                    {
                        if (Lottery[i].Split(',').Length < 4)
                            continue;
                        if (SortKLPK(Lottery[i]) == SortKLPK(WinNumber))
                        {
                            WinCount++;
                            WinMoney += WinMoney1;
                            WinMoneyNoWithTax += WinMoneyNoWithTax1;
                        }
                    }
                }

                if (WinCount > 0)
                {
                    Description = "选四组选4 " + WinCount.ToString() + "注。";
                }

                return WinMoney;
            }
            private double ComputeWin_X4_Zhi(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney1, double WinMoneyNoWithTax1, double WinMoney2, double WinMoneyNoWithTax2, ref int WinCount_1, ref int WinCount_2)	//计算中奖
            {
                int Count = 0;

                WinCount_1 = 0;
                WinCount_2 = 0;

                WinNumber = WinNumber.Trim();
                if (WinNumber.Split(',').Length != 4)  //4: "K,J,9,A"
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
                    string[] Lottery = ToSingle_X4_Zhi(Lotterys[ii], ref t_str);
                    if (Lottery == null)
                        continue;
                    if (Lottery.Length < 1)
                        continue;

                    for (int i = 0; i < Lottery.Length; i++)
                    {
                        if (Lottery[i].Split(',').Length < 4)
                            continue;

                        Count = 0;
                        for (int j = 0; j < 4; j++)
                        {
                            if (Lottery[i].Split(',')[j] == WinNumber.Split(',')[j])
                            {
                                Count++;
                            }
                        }

                        if (Count == 4)
                        {
                            WinCount_1++;
                            WinMoney += WinMoney1;
                            WinMoneyNoWithTax += WinMoneyNoWithTax1;
                        }

                        if (Count == 3)
                        {
                            WinCount_2++;
                            WinMoney += WinMoney2;
                            WinMoneyNoWithTax += WinMoneyNoWithTax2;
                        }
                    }
                }

                if (WinCount_1 > 0)
                {
                    MergeWinDescription(ref Description, "任选四中4 " + WinCount_1.ToString() + "注");
                }
                if (WinCount_2 > 0)
                {
                    MergeWinDescription(ref Description, "任选四中3 " + WinCount_2.ToString() + "注");
                }

                if (Description != "")
                    Description += "。";

                return WinMoney;
            }
            #endregion

            public override string AnalyseScheme(string Content, int PlayType)
            {
                if (PlayType == PlayType_Mixed)   //混合投注
                    return AnalyseScheme_Mixed(Content, PlayType);

                if ((PlayType == PlayType_RX1_D) || (PlayType == PlayType_RX1_F))
                    return AnalyseScheme_RX1(Content, PlayType);

                if ((PlayType == PlayType_RX2_D) || (PlayType == PlayType_RX2_F))
                    return AnalyseScheme_RX2(Content, PlayType);

                if ((PlayType == PlayType_RX3_D) || (PlayType == PlayType_RX3_F))
                    return AnalyseScheme_RX3(Content, PlayType);

                if (PlayType == PlayType_X4_Zu24)
                    return AnalyseScheme_X4_Zu24(Content, PlayType);

                if (PlayType == PlayType_X4_Zu12)
                    return AnalyseScheme_X4_Zu12(Content, PlayType);

                if (PlayType == PlayType_X4_Zu6)
                    return AnalyseScheme_X4_Zu6(Content, PlayType);

                if (PlayType == PlayType_X4_Zu4)
                    return AnalyseScheme_X4_Zu4(Content, PlayType);

                if ((PlayType == PlayType_X4_ZhiD) || (PlayType == PlayType_X4_ZhiF))
                    return AnalyseScheme_X4_Zhi(Content, PlayType);

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

                    if (Lotterys[ii].StartsWith("[任选一单式]"))
                    {
                        t_Result = AnalyseScheme_RX1(FilterPreFix(Lotterys[ii]), PlayType_RX1_D);
                    }

                    if (Lotterys[ii].StartsWith("[任选一复式]"))
                    {
                        t_Result = AnalyseScheme_RX1(FilterPreFix(Lotterys[ii]), PlayType_RX1_F);
                    }

                    if (Lotterys[ii].StartsWith("[任选二单式]"))
                    {
                        t_Result = AnalyseScheme_RX2(FilterPreFix(Lotterys[ii]), PlayType_RX2_D);
                    }

                    if (Lotterys[ii].StartsWith("[任选二复式]"))
                    {
                        t_Result = AnalyseScheme_RX2(FilterPreFix(Lotterys[ii]), PlayType_RX2_F);
                    }

                    if (Lotterys[ii].StartsWith("[任选三单式]"))
                    {
                        t_Result = AnalyseScheme_RX3(FilterPreFix(Lotterys[ii]), PlayType_RX3_D);
                    }

                    if (Lotterys[ii].StartsWith("[任选三复式]"))
                    {
                        t_Result = AnalyseScheme_RX3(FilterPreFix(Lotterys[ii]), PlayType_RX3_F);
                    }

                    if (Lotterys[ii].StartsWith("[选四组选24]"))
                    {
                        t_Result = AnalyseScheme_X4_Zu24(FilterPreFix(Lotterys[ii]), PlayType_X4_Zu24);
                    }

                    if (Lotterys[ii].StartsWith("[选四组选12]"))
                    {
                        t_Result = AnalyseScheme_X4_Zu12(FilterPreFix(Lotterys[ii]), PlayType_X4_Zu12);
                    }

                    if (Lotterys[ii].StartsWith("[选四组选6]"))
                    {
                        t_Result = AnalyseScheme_X4_Zu6(FilterPreFix(Lotterys[ii]), PlayType_X4_Zu6);
                    }

                    if (Lotterys[ii].StartsWith("[选四组选4]"))
                    {
                        t_Result = AnalyseScheme_X4_Zu4(FilterPreFix(Lotterys[ii]), PlayType_X4_Zu4);
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
            private string AnalyseScheme_RX1(string Content, int PlayType)
            {
                string[] strs = Content.Split('\n');
                if (strs == null)
                    return "";
                if (strs.Length == 0)
                    return "";

                string Result = "";

                string RegexString;

                if (PlayType == PlayType_RX1_D)
                {
                    RegexString = @"^((([2-9])|(10)|([AJQK_]))[,]){3}(([2-9])|(10)|([AJQK_]))";
                }
                else
                {
                    RegexString = @"^(((([2-9])|(10)|([AJQK_]))|([(]((([2-9])|(10)|([AJQK]))[,]){1,12}(([2-9])|(10)|([AJQK]))[)]))[,]){3}((([2-9])|(10)|([AJQK_]))|([(]((([2-9])|(10)|([AJQK]))[,]){1,12}(([2-9])|(10)|([AJQK]))[)]))";
                }
                Regex regexstring = new Regex(RegexString, RegexOptions.Compiled | RegexOptions.IgnoreCase);

                for (int i = 0; i < strs.Length; i++)
                {
                    Match MString = regexstring.Match(strs[i].ToString());
                    if (MString.Success)
                    {
                        string CanonicalNumber = "";
                        string[] singles = ToSingle_RX1(strs[i], ref CanonicalNumber);
                        if (singles == null)
                            continue;
                        if ((singles.Length > 1) && (PlayType == PlayType_RX1_F))
                        {
                            Result += CanonicalNumber + "|" + singles.Length.ToString() + "\n";
                        }

                        if ((singles.Length == 1) && (PlayType == PlayType_RX1_D))
                        {
                            Result += CanonicalNumber + "|1\n";
                        }
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

                string RegexString;

                if (PlayType == PlayType_RX2_D)
                {
                    RegexString = @"^((([2-9])|(10)|([AJQK_]))[,]){3}(([2-9])|(10)|([AJQK_]))";
                }
                else
                {
                    RegexString = @"^(((([2-9])|(10)|([AJQK_]))|([(]((([2-9])|(10)|([AJQK]))[,]){1,12}(([2-9])|(10)|([AJQK]))[)]))[,]){3}((([2-9])|(10)|([AJQK_]))|([(]((([2-9])|(10)|([AJQK]))[,]){1,12}(([2-9])|(10)|([AJQK]))[)]))";
                }
                Regex regexstring = new Regex(RegexString, RegexOptions.Compiled | RegexOptions.IgnoreCase);

                for (int i = 0; i < strs.Length; i++)
                {

                    Match MString = regexstring.Match(strs[i].ToString());
                    if (MString.Success)
                    {
                        string CanonicalNumber = "";
                        string[] singles = ToSingle_RX2(strs[i], ref CanonicalNumber);
                        if (singles == null)
                            continue;

                        if ((singles.Length > 1) && (PlayType == PlayType_RX2_F))
                        {
                            Result += CanonicalNumber + "|" + singles.Length.ToString() + "\n";
                        }

                        if ((singles.Length == 1) && (PlayType == PlayType_RX2_D))
                        {
                            Result += CanonicalNumber + "|1\n";
                        }
                    }
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

                string RegexString;

                if (PlayType == PlayType_RX3_D)
                {
                    RegexString = @"^((([2-9])|(10)|([AJQK_]))[,]){3}(([2-9])|(10)|([AJQK_]))";
                }
                else
                {
                    RegexString = @"^(((([2-9])|(10)|([AJQK_]))|([(]((([2-9])|(10)|([AJQK]))[,]){1,12}(([2-9])|(10)|([AJQK]))[)]))[,]){3}((([2-9])|(10)|([AJQK_]))|([(]((([2-9])|(10)|([AJQK]))[,]){1,12}(([2-9])|(10)|([AJQK]))[)]))";
                }
                Regex regexstring = new Regex(RegexString, RegexOptions.Compiled | RegexOptions.IgnoreCase);

                for (int i = 0; i < strs.Length; i++)
                {

                    Match MString = regexstring.Match(strs[i].ToString());
                    if (MString.Success)
                    {
                        string CanonicalNumber = "";
                        string[] singles = ToSingle_RX3(strs[i], ref CanonicalNumber);
                        if (singles == null)
                            continue;

                        if ((singles.Length > 1) && (PlayType == PlayType_RX3_F))
                        {
                            Result += CanonicalNumber + "|" + singles.Length.ToString() + "\n";
                        }

                        if ((singles.Length == 1) && (PlayType == PlayType_RX3_D))
                        {
                            Result += CanonicalNumber + "|1\n";
                        }
                    }
                }

                if (Result.EndsWith("\n"))
                    Result = Result.Substring(0, Result.Length - 1);
                return Result;
            }
            private string AnalyseScheme_X4_Zu24(string Content, int PlayType)
            {
                string[] strs = Content.Split('\n');
                if (strs == null)
                    return "";
                if (strs.Length == 0)
                    return "";

                string Result = "";

                string RegexString = @"^((([2-9])|(10)|([AJQK]))[,]){3,12}(([2-9])|(10)|([AJQK]))";

                Regex regex = new Regex(RegexString, RegexOptions.Compiled | RegexOptions.IgnoreCase);

                for (int i = 0; i < strs.Length; i++)
                {
                    Match m = regex.Match(strs[i]);
                    if (m.Success)
                    {
                        string CanonicalNumber = "";
                        string[] singles = ToSingle_X4_Zu24(m.Value, ref CanonicalNumber);
                        if (singles == null)
                            continue;
                        Result += CanonicalNumber + "|" + singles.Length.ToString() + "\n";
                    }
                }

                if (Result.EndsWith("\n"))
                    Result = Result.Substring(0, Result.Length - 1);
                return Result;
            }
            private string AnalyseScheme_X4_Zu12(string Content, int PlayType)
            {
                string[] strs = Content.Split('\n');
                if (strs == null)
                    return "";
                if (strs.Length == 0)
                    return "";

                string Result = "";

                string RegexString = @"^((([2-9])|(10)|([AJQK]))[,]){3}(([2-9])|(10)|([AJQK]))";

                Regex regex = new Regex(RegexString, RegexOptions.Compiled | RegexOptions.IgnoreCase);

                for (int i = 0; i < strs.Length; i++)
                {
                    Match m = regex.Match(strs[i]);
                    if (m.Success)
                    {
                        string CanonicalNumber = "";
                        string[] singles = ToSingle_X4_Zu12(m.Value, ref CanonicalNumber);
                        if (singles == null)
                            continue;
                        Result += CanonicalNumber + "|" + singles.Length.ToString() + "\n";
                    }
                }

                if (Result.EndsWith("\n"))
                    Result = Result.Substring(0, Result.Length - 1);
                return Result;
            }
            private string AnalyseScheme_X4_Zu6(string Content, int PlayType)
            {
                string[] strs = Content.Split('\n');
                if (strs == null)
                    return "";
                if (strs.Length == 0)
                    return "";

                string Result = "";

                string RegexString = @"^((([2-9])|(10)|([AJQK]))[,]){3}(([2-9])|(10)|([AJQK]))";

                Regex regex = new Regex(RegexString, RegexOptions.Compiled | RegexOptions.IgnoreCase);

                for (int i = 0; i < strs.Length; i++)
                {
                    Match m = regex.Match(strs[i]);
                    if (m.Success)
                    {
                        string CanonicalNumber = "";
                        string[] singles = ToSingle_X4_Zu6(m.Value, ref CanonicalNumber);
                        if (singles == null)
                            continue;
                        Result += CanonicalNumber + "|" + singles.Length.ToString() + "\n";
                    }
                }

                if (Result.EndsWith("\n"))
                    Result = Result.Substring(0, Result.Length - 1);
                return Result;
            }
            private string AnalyseScheme_X4_Zu4(string Content, int PlayType)
            {
                string[] strs = Content.Split('\n');
                if (strs == null)
                    return "";
                if (strs.Length == 0)
                    return "";

                string Result = "";

                string RegexString = @"^((([2-9])|(10)|([AJQK]))[,]){3}(([2-9])|(10)|([AJQK]))";

                Regex regex = new Regex(RegexString, RegexOptions.Compiled | RegexOptions.IgnoreCase);

                for (int i = 0; i < strs.Length; i++)
                {
                    Match m = regex.Match(strs[i]);
                    if (m.Success)
                    {
                        string CanonicalNumber = "";
                        string[] singles = ToSingle_X4_Zu4(m.Value, ref CanonicalNumber);
                        if (singles == null)
                            continue;
                        Result += CanonicalNumber + "|" + singles.Length.ToString() + "\n";
                    }
                }

                if (Result.EndsWith("\n"))
                    Result = Result.Substring(0, Result.Length - 1);
                return Result;
            }
            private string AnalyseScheme_X4_Zhi(string Content, int PlayType)
            {

                string[] strs = Content.Split('\n');
                if (strs == null)
                    return "";
                if (strs.Length == 0)
                    return "";

                string Result = "";

                string RegexString;

                if (PlayType == PlayType_X4_ZhiD)
                {
                    RegexString = @"^((([2-9])|(10)|([AJQK]))[,]){3}(([2-9])|(10)|([AJQK]))";
                }
                else
                {
                    RegexString = @"^(((([2-9])|(10)|([AJQK]))|([(]((([2-9])|(10)|([AJQK]))[,]){1,12}(([2-9])|(10)|([AJQK]))[)]))[,]){3}((([2-9])|(10)|([AJQK]))|([(]((([2-9])|(10)|([AJQK]))[,]){1,12}(([2-9])|(10)|([AJQK]))[)]))";
                }
                Regex regexstring = new Regex(RegexString, RegexOptions.Compiled | RegexOptions.IgnoreCase);

                for (int i = 0; i < strs.Length; i++)
                {
                    Match MString = regexstring.Match(strs[i].ToString());
                    if (MString.Success)
                    {
                        string CanonicalNumber = "";
                        string[] singles = ToSingle_X4_Zhi(MString.Value, ref CanonicalNumber);
                        if (singles == null)
                            continue;
                        if (singles.Length >= ((PlayType == PlayType_X4_ZhiD) ? 1 : 2))
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
                Regex regex = new Regex(@"^((([2-9])|(10)|([AJQK]))[,]){3}(([2-9])|(10)|([AJQK]))", RegexOptions.Compiled | RegexOptions.IgnoreCase);

                return regex.IsMatch(Number);
            }

            private string FilterRepeated(string NumberPart)
            {
                string Result = "";
                string[] strs = NumberPart.Split(',');

                for (int i = 0; i < strs.Length; i++)
                {
                    if ("23456789AJQK10".IndexOf(strs[i]) >= 0 && Result.IndexOf(strs[i]) == -1)
                    {
                        Result += strs[i] + ",";
                    }
                }

                Result = Result.Substring(0, Result.Length - 1);

                return SortKLPK(Result);
            }

            public override string ShowNumber(string Number)
            {
                return base.ShowNumber(Number, " ");
            }

            private string SortKLPK(string Number)
            {
                string[] KLPK = new string[13];
                string Result = "";
                string[] strs = Number.Split(',');

                for (int i = 0; i < strs.Length; i++)
                {
                    if ("23456789AJQK10".IndexOf(strs[i]) >= 0)
                    {
                        if (strs[i] == "A")
                        {
                            KLPK[0] = "A";
                        }
                        else if (strs[i] == "2")
                        {
                            KLPK[1] = "2";
                        }
                        else if (strs[i] == "3")
                        {
                            KLPK[2] = "3";
                        }
                        else if (strs[i] == "4")
                        {
                            KLPK[3] = "4";
                        }
                        else if (strs[i] == "5")
                        {
                            KLPK[4] = "5";
                        }
                        else if (strs[i] == "6")
                        {
                            KLPK[5] = "6";
                        }
                        else if (strs[i] == "7")
                        {
                            KLPK[6] = "7";
                        }
                        else if (strs[i] == "8")
                        {
                            KLPK[7] = "8";
                        }
                        else if (strs[i] == "9")
                        {
                            KLPK[8] = "9";
                        }
                        else if (strs[i] == "10")
                        {
                            KLPK[9] = "10";
                        }
                        else if (strs[i] == "J")
                        {
                            KLPK[10] = "J";
                        }
                        else if (strs[i] == "Q")
                        {
                            KLPK[11] = "Q";
                        }
                        else if (strs[i] == "K")
                        {
                            KLPK[12] = "K";
                        }
                    }
                }

                for (int i = 0; i < 13; i++)
                {
                    if (KLPK[i] != null)
                    {
                        Result += KLPK[i] + ",";
                    }
                }

                Result = Result.Substring(0, Result.Length - 1);
                return Result;
            }
        }
    }
}
