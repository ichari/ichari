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
        /// 河南快赢481
        /// </summary>
        public partial class HNKY481 : LotteryBase
        {
            #region 静态变量
            public const int PlayType_RX1 = 6801;
            public const int PlayType_RX2 = 6802;
            public const int PlayType_RX3 = 6803;

            public const int PlayType_X4ZXD = 6804;
            public const int PlayType_X4ZXF = 6805;

            public const int PlayType_X4ZX24 = 6806;
            public const int PlayType_X4ZX24_F = 6807;

            public const int PlayType_X4ZX12 = 6808;
            public const int PlayType_X4ZX12_F = 6809;

            public const int PlayType_X4ZX6 = 6810;
            public const int PlayType_X4ZX6_F = 6811;

            public const int PlayType_X4ZX4 = 6812;
            public const int PlayType_X4ZX4_F = 6813;

            public const int ID = 68;
            public const string sID = "68";
            public const string Name = "快赢481";
            public const string Code = "HNKY481";
            public const double MaxMoney = 20000;
            #endregion

            public HNKY481()
            {
                id = 68;
                name = "快赢481";
                code = "HNKY481";
            }

            public override bool CheckPlayType(int play_type)
            {
                return ((play_type >= 6801) && (play_type <= 6813));
            }

            public override PlayType[] GetPlayTypeList()
            {
                PlayType[] Result = new PlayType[13];

                Result[0] = new PlayType(PlayType_RX1, "任选一");
                Result[1] = new PlayType(PlayType_RX2, "任选二");
                Result[2] = new PlayType(PlayType_RX3, "任选三");

                Result[3] = new PlayType(PlayType_X4ZXD, "直选单式");
                Result[4] = new PlayType(PlayType_X4ZXF, "直选复式");

                Result[5] = new PlayType(PlayType_X4ZX24, "组选24单式");
                Result[6] = new PlayType(PlayType_X4ZX24_F, "组选24复式");

                Result[7] = new PlayType(PlayType_X4ZX12, "组选12单式");
                Result[8] = new PlayType(PlayType_X4ZX12_F, "组选12复式");

                Result[9] = new PlayType(PlayType_X4ZX6, "组选6单式");
                Result[10] = new PlayType(PlayType_X4ZX6_F, "组选6复式");

                Result[11] = new PlayType(PlayType_X4ZX4, "组选4单式");
                Result[12] = new PlayType(PlayType_X4ZX4_F, "组选4复式");

                return Result;
            }

            public override string BuildNumber(int Num)	//id = 60
            {
                System.Random rd = new Random();
                StringBuilder sb = new StringBuilder();

                for (int i = 0; i < Num; i++)
                {
                    string LotteryNumber = "";
                    for (int j = 0; j < 4; j++)
                    {
                        LotteryNumber += rd.Next(1, 8).ToString();
                    }

                    sb.Append(LotteryNumber.Trim() + "\n");
                }

                string Result = sb.ToString();
                Result = Result.Substring(0, Result.Length - 1);
                return Result;
            }

            public override string[] ToSingle(string Number, ref string CanonicalNumber, int PlayType)
            {
                if (PlayType == PlayType_RX1)
                    return ToSingle_RX1(Number, ref CanonicalNumber);

                if (PlayType == PlayType_RX2)
                    return ToSingle_RX2(Number, ref CanonicalNumber);

                if (PlayType == PlayType_RX3)
                    return ToSingle_RX3(Number, ref CanonicalNumber);

                if (PlayType == PlayType_X4ZXD || PlayType == PlayType_X4ZXF)
                    return ToSingle_Zhi(Number, ref CanonicalNumber);

                if (PlayType == PlayType_X4ZX24)
                    return ToSingle_Zu24(Number, ref CanonicalNumber);

                if (PlayType == PlayType_X4ZX12)
                    return ToSingle_Zu12(Number, ref CanonicalNumber);

                if (PlayType == PlayType_X4ZX6)
                    return ToSingle_Zu6(Number, ref CanonicalNumber);

                if (PlayType == PlayType_X4ZX4)
                    return ToSingle_Zu4(Number, ref CanonicalNumber);

                if (PlayType == PlayType_X4ZX24_F)
                    return ToSingle_Zu24_F(Number, ref CanonicalNumber);

                if (PlayType == PlayType_X4ZX12_F)
                    return ToSingle_Zu12_F(Number, ref CanonicalNumber);

                if (PlayType == PlayType_X4ZX6_F)
                    return ToSingle_Zu6_F(Number, ref CanonicalNumber);

                if (PlayType == PlayType_X4ZX4_F)
                    return ToSingle_Zu4_F(Number, ref CanonicalNumber);

                return null;
            }

            #region ToSingle 的具体方法（复试取单式一组（组选或者单选号码））
            private string[] ToSingle_RX1(string Number, ref string CanonicalNumber)
            {
                string[] Locate = new string[4];
                CanonicalNumber = "";

                Regex regex = new Regex(@"^(?<L0>(\d)|(_)|([(][\d]+?[)]))(?<L1>(\d)|(_)|([(][\d]+?[)]))(?<L2>(\d)|(_)|([(][\d]+?[)]))(?<L3>(\d)|(_)|([(][\d]+?[)]))", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                Match m = regex.Match(Number);
                for (int i = 0; i < 4; i++)
                {
                    Locate[i] = m.Groups["L" + i.ToString()].ToString().Trim();
                    if (Locate[i] == "")
                    {
                        CanonicalNumber = "";
                        return null;
                    }
                    if (Locate[i].Length > 1)
                    {
                        Locate[i] = Locate[i].Substring(1, Locate[i].Length - 2);//去掉括号；
                        if (Locate[i].Length > 1)
                            Locate[i] = FilterRepeated(Locate[i]);//过滤重复号
                        if (Locate[i] == "")
                        {
                            CanonicalNumber = "";
                            return null;
                        }
                    }
                    if (Locate[i].Length > 1)
                        CanonicalNumber += "(" + Locate[i] + ")";                   //10（110）变成10（10）
                    else
                        CanonicalNumber += Locate[i];                               //10（11）变成101
                }


                #region 循环取单式
                ArrayList al = new ArrayList();

                //第0位
                for (int i_0 = 0; i_0 < Locate[0].Length; i_0++)
                {
                    string str_0 = Locate[0][i_0].ToString();
                    if (Shove._Convert.StrToInt(Locate[0][i_0].ToString(), 0) > 0)
                    {
                        al.Add(str_0 + "___");
                    }
                }
                //第1位
                for (int i_1 = 0; i_1 < Locate[1].Length; i_1++)
                {
                    string str_1 = Locate[1][i_1].ToString();
                    if (Shove._Convert.StrToInt(Locate[1][i_1].ToString(), 0) > 0)
                    {
                        al.Add("_" + str_1 + "__");
                    }
                }
                //第2位
                for (int i_2 = 0; i_2 < Locate[2].Length; i_2++)
                {
                    string str_2 = Locate[2][i_2].ToString();
                    if (Shove._Convert.StrToInt(Locate[2][i_2].ToString(), 0) > 0)
                    {
                        al.Add("__" + str_2 + "_");
                    }

                }
                //第3位
                for (int i_3 = 0; i_3 < Locate[3].Length; i_3++)
                {
                    string str_3 = Locate[3][i_3].ToString();
                    if (Shove._Convert.StrToInt(Locate[3][i_3].ToString(), 0) > 0)
                    {
                        al.Add("___" + str_3);
                    }
                }
                #endregion

                string[] Result = new string[al.Count];
                for (int i = 0; i < al.Count; i++)
                    Result[i] = al[i].ToString();
                return Result;
            }

            private string[] ToSingle_RX2(string Number, ref string CanonicalNumber)
            {
                string[] Locate = new string[4];
                CanonicalNumber = "";

                Regex regex = new Regex(@"^(?<L0>(\d)|(_)|([(][\d]+?[)]))(?<L1>(\d)|(_)|([(][\d]+?[)]))(?<L2>(\d)|(_)|([(][\d]+?[)]))(?<L3>(\d)|(_)|([(][\d]+?[)]))", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                Match m = regex.Match(Number);
                for (int i = 0; i < 4; i++)
                {
                    Locate[i] = m.Groups["L" + i.ToString()].ToString().Trim();
                    if (Locate[i] == "")
                    {
                        CanonicalNumber = "";
                        return null;
                    }
                    if (Locate[i].Length > 1)
                    {
                        Locate[i] = Locate[i].Substring(1, Locate[i].Length - 2);//去掉括号；
                        if (Locate[i].Length > 1)
                            Locate[i] = FilterRepeated(Locate[i]);//过滤重复号
                        if (Locate[i] == "")
                        {
                            CanonicalNumber = "";
                            return null;
                        }
                    }
                    if (Locate[i].Length > 1)
                        CanonicalNumber += "(" + Locate[i] + ")";                   //10（110）变成10（10）
                    else
                        CanonicalNumber += Locate[i];                               //10（11）变成101
                }

                #region 循环取单式
                ArrayList al = new ArrayList();

                //0，1 位
                for (int i_0 = 0; i_0 < Locate[0].Length; i_0++)
                {
                    string str_0 = Locate[0].Substring(i_0, 1);
                    if (str_0 != "_")
                    {
                        for (int i_1 = 0; i_1 < Locate[1].Length; i_1++)
                        {
                            string str_1 = Locate[1].Substring(i_1, 1);
                            if (str_1 != "_")
                            {
                                al.Add(str_0 + str_1 + "__");
                            }
                        }
                    }
                }

                //0，2 位
                for (int i_0 = 0; i_0 < Locate[0].Length; i_0++)
                {
                    string str_0 = Locate[0].Substring(i_0, 1);
                    if (str_0 != "_")
                    {
                        for (int i_2 = 0; i_2 < Locate[2].Length; i_2++)
                        {
                            string str_2 = Locate[2].Substring(i_2, 1);
                            if (str_2 != "_")
                            {
                                al.Add(str_0 + "_" + str_2 + "_");
                            }
                        }
                    }
                }
                //0，3 位
                for (int i_0 = 0; i_0 < Locate[0].Length; i_0++)
                {
                    string str_0 = Locate[0].Substring(i_0, 1);
                    if (str_0 != "_")
                    {
                        for (int i_3 = 0; i_3 < Locate[3].Length; i_3++)
                        {
                            string str_3 = Locate[3].Substring(i_3, 1);
                            if (str_3 != "_")
                            {
                                al.Add(str_0 + "__" + str_3);
                            }

                        }
                    }
                }
                //1，2 位
                for (int i_1 = 0; i_1 < Locate[1].Length; i_1++)
                {
                    string str_1 = Locate[1].Substring(i_1, 1);
                    if (str_1 != "_")
                    {
                        for (int i_2 = 0; i_2 < Locate[2].Length; i_2++)
                        {
                            string str_2 = Locate[2].Substring(i_2, 1);
                            if (str_2 != "_")
                            {
                                al.Add("_" + str_1 + str_2 + "_");
                            }
                        }
                    }
                }
                //1，3 位
                for (int i_1 = 0; i_1 < Locate[1].Length; i_1++)
                {
                    string str_1 = Locate[1].Substring(i_1, 1);
                    if (str_1 != "_")
                    {
                        for (int i_3 = 0; i_3 < Locate[3].Length; i_3++)
                        {
                            string str_3 = Locate[3].Substring(i_3, 1);
                            if (str_3 != "_")
                            {
                                al.Add("_" + str_1 + "_" + str_3);
                            }

                        }
                    }
                }
                //2,3位
                for (int i_2 = 0; i_2 < Locate[2].Length; i_2++)
                {
                    string str_2 = Locate[2].Substring(i_2, 1);
                    if (str_2 != "_")
                    {
                        for (int i_3 = 0; i_3 < Locate[3].Length; i_3++)
                        {
                            string str_3 = Locate[3].Substring(i_3, 1);
                            if (str_3 != "_")
                            {
                                al.Add("__" + str_2 + str_3);
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

            private string[] ToSingle_RX3(string Number, ref string CanonicalNumber)
            {
                string[] Locate = new string[4];
                CanonicalNumber = "";

                Regex regex = new Regex(@"^(?<L0>(\d)|(_)|([(][\d]+?[)]))(?<L1>(\d)|(_)|([(][\d]+?[)]))(?<L2>(\d)|(_)|([(][\d]+?[)]))(?<L3>(\d)|(_)|([(][\d]+?[)]))", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                Match m = regex.Match(Number);
                for (int i = 0; i < 4; i++)
                {
                    Locate[i] = m.Groups["L" + i.ToString()].ToString().Trim();
                    if (Locate[i] == "")
                    {
                        CanonicalNumber = "";
                        return null;
                    }
                    if (Locate[i].Length > 1)
                    {
                        Locate[i] = Locate[i].Substring(1, Locate[i].Length - 2);//去掉括号；
                        if (Locate[i].Length > 1)
                            Locate[i] = FilterRepeated(Locate[i]);//过滤重复号
                        if (Locate[i] == "")
                        {
                            CanonicalNumber = "";
                            return null;
                        }
                    }
                    if (Locate[i].Length > 1)
                        CanonicalNumber += "(" + Locate[i] + ")";                   //10（110）变成10（10）
                    else
                        CanonicalNumber += Locate[i];                               //10（11）变成101
                }

                #region 循环取单式
                ArrayList al = new ArrayList();

                //0，1，2 位
                for (int i_0 = 0; i_0 < Locate[0].Length; i_0++)
                {
                    string str_0 = Locate[0].Substring(i_0, 1);
                    if (str_0 != "_")
                    {
                        for (int i_1 = 0; i_1 < Locate[1].Length; i_1++)
                        {
                            string str_1 = Locate[1].Substring(i_1, 1);
                            if (str_1 != "_")
                            {
                                for (int i_2 = 0; i_2 < Locate[2].Length; i_2++)
                                {
                                    string str_2 = Locate[2].Substring(i_2, 1);
                                    if (str_2 != "_")
                                    {
                                        al.Add(str_0 + str_1 + str_2 + "_");
                                    }
                                }
                            }
                        }
                    }
                }

                //0，1，3 位
                for (int i_0 = 0; i_0 < Locate[0].Length; i_0++)
                {
                    string str_0 = Locate[0].Substring(i_0, 1);
                    if (str_0 != "_")
                    {
                        for (int i_1 = 0; i_1 < Locate[1].Length; i_1++)
                        {
                            string str_1 = Locate[1].Substring(i_1, 1);
                            if (str_1 != "_")
                            {
                                for (int i_3 = 0; i_3 < Locate[3].Length; i_3++)
                                {
                                    string str_3 = Locate[3].Substring(i_3, 1);
                                    if (str_3 != "_")
                                    {
                                        al.Add(str_0 + str_1 + "_" + str_3);
                                    }
                                }
                            }
                        }
                    }
                }

                //0，2，3 位
                for (int i_0 = 0; i_0 < Locate[0].Length; i_0++)
                {
                    string str_0 = Locate[0].Substring(i_0, 1);
                    if (str_0 != "_")
                    {
                        for (int i_2 = 0; i_2 < Locate[2].Length; i_2++)
                        {
                            string str_2 = Locate[2].Substring(i_2, 1);
                            if (str_2 != "_")
                            {
                                for (int i_3 = 0; i_3 < Locate[3].Length; i_3++)
                                {
                                    string str_3 = Locate[3].Substring(i_3, 1);
                                    if (str_3 != "_")
                                    {
                                        al.Add(str_0 + "_" + str_2 + str_3);
                                    }
                                }
                            }
                        }
                    }
                }
                //1，2，3 位
                for (int i_1 = 0; i_1 < Locate[1].Length; i_1++)
                {
                    string str_1 = Locate[1].Substring(i_1, 1);
                    if (str_1 != "_")
                    {
                        for (int i_2 = 0; i_2 < Locate[2].Length; i_2++)
                        {
                            string str_2 = Locate[2].Substring(i_2, 1);
                            if (str_2 != "_")
                            {
                                for (int i_3 = 0; i_3 < Locate[3].Length; i_3++)
                                {
                                    string str_3 = Locate[3].Substring(i_3, 1);
                                    if (str_3 != "_")
                                    {
                                        al.Add("_" + str_1 + str_2 + str_3);
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

            private string[] ToSingle_Zhi(string Number, ref string CanonicalNumber)	//复式取单式, 后面 ref 参数是将彩票规范化，如：10(122) 变成10(12)
            {
                string[] Locate = new string[4];
                CanonicalNumber = "";

                Regex regex = new Regex(@"^(?<L0>(\d)|([(][\d]+?[)]))(?<L1>(\d)|([(][\d]+?[)]))(?<L2>(\d)|([(][\d]+?[)]))(?<L3>(\d)|([(][\d]+?[)]))", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                Match m = regex.Match(Number);
                for (int i = 0; i < 4; i++)
                {
                    Locate[i] = m.Groups["L" + i.ToString()].ToString().Trim();
                    if (Locate[i] == "")
                    {
                        CanonicalNumber = "";
                        return null;
                    }
                    if (Locate[i].Length > 1)
                    {
                        Locate[i] = Locate[i].Substring(1, Locate[i].Length - 2);//去掉括号；
                        if (Locate[i].Length > 1)
                            Locate[i] = FilterRepeated(Locate[i]);//过滤重复号
                        if (Locate[i] == "")
                        {
                            CanonicalNumber = "";
                            return null;
                        }
                    }
                    if (Locate[i].Length > 1)
                        CanonicalNumber += "(" + Locate[i] + ")";                   //10（110）变成10（10）
                    else
                        CanonicalNumber += Locate[i];                               //10（11）变成101
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
                                al.Add(str_3);
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
            private string[] ToSingle_Zu(string Number, ref string CanonicalNumber, int PlayType)
            {
                if (PlayType == PlayType_X4ZX24)
                {
                    return ToSingle_Zu24(Number, ref CanonicalNumber);
                }

                if (PlayType == PlayType_X4ZX12)
                {
                    return ToSingle_Zu12(Number, ref CanonicalNumber);
                }

                if (PlayType == PlayType_X4ZX6)
                {
                    return ToSingle_Zu6(Number, ref CanonicalNumber);
                }

                if (PlayType == PlayType_X4ZX4)
                {
                    return ToSingle_Zu4(Number, ref CanonicalNumber);
                }

                if (PlayType == PlayType_X4ZX24_F)
                {
                    return ToSingle_Zu24_F(Number, ref CanonicalNumber);
                }

                if (PlayType == PlayType_X4ZX12_F)
                {
                    return ToSingle_Zu12_F(Number, ref CanonicalNumber);
                }

                if (PlayType == PlayType_X4ZX6_F)
                {
                    return ToSingle_Zu6_F(Number, ref CanonicalNumber);
                }

                if (PlayType == PlayType_X4ZX4_F)
                {
                    return ToSingle_Zu4_F(Number, ref CanonicalNumber);
                }

                return null;
            }
            private string[] ToSingle_Zu24(string Number, ref string CanonicalNumber)	//复式取单式, 后面 ref 参数是将彩票规范化，如：(10223) 变成1023
            {
                string[] Locate = new string[4];
                CanonicalNumber = "";

                Regex regex = new Regex(@"^(?<L0>(\d)|([(][\d]+?[)]))(?<L1>(\d)|([(][\d]+?[)]))(?<L2>(\d)|([(][\d]+?[)]))(?<L3>(\d)|([(][\d]+?[)]))", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                Match m = regex.Match(Number);
                for (int i = 0; i < 4; i++)
                {
                    Locate[i] = m.Groups["L" + i.ToString()].ToString().Trim();
                    if (Locate[i] == "")
                    {
                        CanonicalNumber = "";
                        return null;
                    }
                    if (Locate[i].Length > 1)
                    {
                        Locate[i] = Locate[i].Substring(1, Locate[i].Length - 2);//去掉括号；
                        if (Locate[i].Length > 1)
                            Locate[i] = FilterRepeated(Locate[i]);//过滤重复号
                        if (Locate[i] == "")
                        {
                            CanonicalNumber = "";
                            return null;
                        }
                    }
                    if (Locate[i].Length > 1)
                        CanonicalNumber += "(" + Locate[i] + ")";                   //10（110）变成10（10）
                    else
                        CanonicalNumber += Locate[i];                               //10（11）变成101
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
                                if (str_3.Length < 4)
                                    continue;

                                //计算组24注数(4不相同)
                                if (FilterRepeated(str_3).Length == 4)
                                {
                                    al.Add(str_3);
                                }
                            }
                        }
                    }
                }

                for (int k = 0; k < al.Count; k++)
                {
                    if (FilterRepeated(Sort(al[k].ToString())).Length == 1)
                    {
                        al.Remove(al[k]);
                    }
                }

                #endregion

                string[] Result = new string[al.Count];
                for (int i = 0; i < al.Count; i++)
                    Result[i] = al[i].ToString();
                return Result;
            }
            private string[] ToSingle_Zu12(string Number, ref string CanonicalNumber)	//复式取单式, 后面 ref 参数是将彩票规范化，如：(10223) 变成1023
            {
                string[] Locate = new string[4];
                CanonicalNumber = "";

                Regex regex = new Regex(@"^(?<L0>(\d)|([(][\d]+?[)]))(?<L1>(\d)|([(][\d]+?[)]))(?<L2>(\d)|([(][\d]+?[)]))(?<L3>(\d)|([(][\d]+?[)]))", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                Match m = regex.Match(Number);
                for (int i = 0; i < 4; i++)
                {
                    Locate[i] = m.Groups["L" + i.ToString()].ToString().Trim();
                    if (Locate[i] == "")
                    {
                        CanonicalNumber = "";
                        return null;
                    }
                    if (Locate[i].Length > 1)
                    {
                        Locate[i] = Locate[i].Substring(1, Locate[i].Length - 2);//去掉括号；
                        if (Locate[i].Length > 1)
                            Locate[i] = FilterRepeated(Locate[i]);//过滤重复号
                        if (Locate[i] == "")
                        {
                            CanonicalNumber = "";
                            return null;
                        }
                    }
                    if (Locate[i].Length > 1)
                        CanonicalNumber += "(" + Locate[i] + ")";                   //10（110）变成10（10）
                    else
                        CanonicalNumber += Locate[i];                               //10（11）变成101
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
                                if (str_3.Length < 4)
                                    continue;

                                //计算组12注数(两相同两不同)
                                if (FilterRepeated(str_3).Length == 3)
                                {
                                    al.Add(str_3);
                                }
                            }
                        }
                    }
                }

                for (int k = 0; k < al.Count; k++)
                {
                    if (FilterRepeated(Sort(al[k].ToString())).Length == 1)
                    {
                        al.Remove(al[k]);
                    }
                }

                #endregion

                string[] Result = new string[al.Count];
                for (int i = 0; i < al.Count; i++)
                    Result[i] = al[i].ToString();
                return Result;
            }
            private string[] ToSingle_Zu6(string Number, ref string CanonicalNumber)	//复式取单式, 后面 ref 参数是将彩票规范化，如：(10223) 变成1023
            {
                string[] Locate = new string[4];
                CanonicalNumber = "";

                Regex regex = new Regex(@"^(?<L0>(\d)|([(][\d]+?[)]))(?<L1>(\d)|([(][\d]+?[)]))(?<L2>(\d)|([(][\d]+?[)]))(?<L3>(\d)|([(][\d]+?[)]))", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                Match m = regex.Match(Number);
                for (int i = 0; i < 4; i++)
                {
                    Locate[i] = m.Groups["L" + i.ToString()].ToString().Trim();
                    if (Locate[i] == "")
                    {
                        CanonicalNumber = "";
                        return null;
                    }
                    if (Locate[i].Length > 1)
                    {
                        Locate[i] = Locate[i].Substring(1, Locate[i].Length - 2);//去掉括号；
                        if (Locate[i].Length > 1)
                            Locate[i] = FilterRepeated(Locate[i]);//过滤重复号
                        if (Locate[i] == "")
                        {
                            CanonicalNumber = "";
                            return null;
                        }
                    }
                    if (Locate[i].Length > 1)
                        CanonicalNumber += "(" + Locate[i] + ")";                   //10（110）变成10（10）
                    else
                        CanonicalNumber += Locate[i];                               //10（11）变成101
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
                                if (str_3.Length < 4)
                                    continue;

                                //计算组6注数(两两相同)
                                if ((FilterRepeated(str_3).Length == 2) && Sort(str_3).Substring(1, 1) != Sort(str_3).Substring(2, 1))
                                {
                                    al.Add(str_3);
                                }
                            }
                        }
                    }
                }

                for (int k = 0; k < al.Count; k++)
                {
                    if (FilterRepeated(Sort(al[k].ToString())).Length == 1)
                    {
                        al.Remove(al[k]);
                    }
                }

                #endregion

                string[] Result = new string[al.Count];
                for (int i = 0; i < al.Count; i++)
                    Result[i] = al[i].ToString();
                return Result;
            }
            private string[] ToSingle_Zu4(string Number, ref string CanonicalNumber)	//复式取单式, 后面 ref 参数是将彩票规范化，如：(10223) 变成1023
            {
                string[] Locate = new string[4];
                CanonicalNumber = "";

                Regex regex = new Regex(@"^(?<L0>(\d)|([(][\d]+?[)]))(?<L1>(\d)|([(][\d]+?[)]))(?<L2>(\d)|([(][\d]+?[)]))(?<L3>(\d)|([(][\d]+?[)]))", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                Match m = regex.Match(Number);
                for (int i = 0; i < 4; i++)
                {
                    Locate[i] = m.Groups["L" + i.ToString()].ToString().Trim();
                    if (Locate[i] == "")
                    {
                        CanonicalNumber = "";
                        return null;
                    }
                    if (Locate[i].Length > 1)
                    {
                        Locate[i] = Locate[i].Substring(1, Locate[i].Length - 2);//去掉括号；
                        if (Locate[i].Length > 1)
                            Locate[i] = FilterRepeated(Locate[i]);//过滤重复号
                        if (Locate[i] == "")
                        {
                            CanonicalNumber = "";
                            return null;
                        }
                    }
                    if (Locate[i].Length > 1)
                        CanonicalNumber += "(" + Locate[i] + ")";                   //10（110）变成10（10）
                    else
                        CanonicalNumber += Locate[i];                               //10（11）变成101
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
                                if (str_3.Length < 4)
                                    continue;
                                //计算组4注数(3个号码相同));

                                if ((FilterRepeated(Sort(str_3)).Length == 2) && Sort(str_3).Substring(1, 1) == Sort(str_3).Substring(2, 1))
                                {
                                    al.Add(str_3);
                                }
                            }
                        }
                    }
                }

                for (int k = 0; k < al.Count; k++)
                {
                    if (FilterRepeated(Sort(al[k].ToString())).Length == 1)
                    {
                        al.Remove(al[k]);
                    }
                }

                #endregion

                string[] Result = new string[al.Count];
                for (int i = 0; i < al.Count; i++)
                    Result[i] = al[i].ToString();
                return Result;
            }

            private string[] ToSingle_Zu24_F(string Number, ref string CanonicalNumber)	//复式取单式, 后面 ref 参数是将彩票规范化，如：(10223) 变成1023
            {
                CanonicalNumber = FilterRepeated(Number.Trim());
                if (CanonicalNumber.Length < 5)
                {
                    CanonicalNumber = "";
                    return null;
                }

                CanonicalNumber = Number;

                char[] strs = CanonicalNumber.ToCharArray();

                ArrayList al = new ArrayList();

                #region 循环取单式

                int n = strs.Length;

                for (int i = 0; i < n - 3; i++)
                    for (int j = i + 1; j < n - 2; j++)
                        for (int k = j + 1; k < n-1; k++)
                            for (int l = k + 1; l < n; l++ )
                                al.Add(strs[i].ToString() + strs[j].ToString() + strs[k].ToString() + strs[l].ToString());
               
                #endregion

                string[] Result = new string[al.Count];
                for (int i = 0; i < al.Count; i++)
                    Result[i] = al[i].ToString();
                return Result;
            }
            private string[] ToSingle_Zu12_F(string Number, ref string CanonicalNumber)	//复式取单式, 后面 ref 参数是将彩票规范化，如：(10223) 变成1023
            {
                CanonicalNumber = FilterRepeated(Number.Trim());
                if (CanonicalNumber.Length < 3)
                {
                    CanonicalNumber = "";
                    return null;
                }

                CanonicalNumber = Number;

                char[] strs = CanonicalNumber.ToCharArray();

                ArrayList al = new ArrayList();

                #region 循环取单式

                int n = strs.Length;

                for (int i = 0; i < n - 2; i++)
                    for (int j = i + 1; j < n - 1; j++)
                        for (int k = j + 1; k < n; k++)
                        {
                            al.Add(strs[i].ToString() + strs[j].ToString() + strs[k].ToString() + strs[k].ToString());
                            al.Add(strs[i].ToString() + strs[j].ToString() + strs[j].ToString() + strs[k].ToString());
                            al.Add(strs[i].ToString() + strs[i].ToString() + strs[j].ToString() + strs[k].ToString());
                        }

                #endregion

                string[] Result = new string[al.Count];
                for (int i = 0; i < al.Count; i++)
                    Result[i] = al[i].ToString();
                return Result;
            }
            private string[] ToSingle_Zu6_F(string Number, ref string CanonicalNumber)	//复式取单式, 后面 ref 参数是将彩票规范化，如：(10223) 变成1023
            {
                CanonicalNumber = FilterRepeated(Number.Trim());
                if (CanonicalNumber.Length < 3)
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
                        al.Add(strs[i].ToString() + strs[i].ToString() + strs[j].ToString() + strs[j].ToString());
                    }

                #endregion

                string[] Result = new string[al.Count];
                for (int i = 0; i < al.Count; i++)
                    Result[i] = al[i].ToString();
                return Result;
            }
            private string[] ToSingle_Zu4_F(string Number, ref string CanonicalNumber)	//复式取单式, 后面 ref 参数是将彩票规范化，如：(10223) 变成1023
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
                        al.Add(strs[i].ToString() + strs[i].ToString() + strs[i].ToString() + strs[j].ToString());
                        al.Add(strs[i].ToString() + strs[j].ToString() + strs[j].ToString() + strs[j].ToString());
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
                if ((WinMoneyList == null) || (WinMoneyList.Length < 16))    // 奖金参数排列顺序 zhi, zu3 zu6
                    return -3;

                int WinCountRX1 = 0, WinCountRX2 = 0, WinCountRX3 = 0;

                if (PlayType == PlayType_RX1)
                    return ComputeWin_RX1(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[0], WinMoneyList[1], ref WinCountRX1);

                if (PlayType == PlayType_RX2)
                    return ComputeWin_RX2(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[2], WinMoneyList[3], ref WinCountRX2);

                if (PlayType == PlayType_RX3)
                    return ComputeWin_RX3(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[4], WinMoneyList[5], ref WinCountRX3);

                if ((PlayType == PlayType_X4ZXD) || (PlayType == PlayType_X4ZXF))
                    return ComputeWin_Zhi(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[6], WinMoneyList[7]);

                if (PlayType == PlayType_X4ZX24 || PlayType == PlayType_X4ZX12 || PlayType == PlayType_X4ZX6 || PlayType == PlayType_X4ZX4 || PlayType == PlayType_X4ZX24_F || PlayType == PlayType_X4ZX12_F || PlayType == PlayType_X4ZX6_F || PlayType == PlayType_X4ZX4_F)
                    return ComputeWin_Zu(PlayType, Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[8], WinMoneyList[9], WinMoneyList[10], WinMoneyList[11], WinMoneyList[12], WinMoneyList[13], WinMoneyList[14], WinMoneyList[15]);

                return -4;
            }
            #region ComputeWin 的具体方法
            private double ComputeWin_RX1(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney1, double WinMoneyNoWithTax1, ref int WinCountRX1)
            {
                WinNumber = WinNumber.Trim();
                if (WinNumber.Length < 4)	//4: "1234"
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
                        for (int k = 0; k < 4; k++)
                        {
                            if (WinNumber.Substring(k, 1) == Lottery[i].Substring(k, 1))
                            {
                                WinCountRX1++;
                                WinMoney += WinMoney1;
                                WinMoneyNoWithTax += WinMoneyNoWithTax1;
                                continue;
                            }
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
                if (WinNumber.Length < 4)	//4: "1234"
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

                int count = 0;
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
                        count = 0;
                        for (int k = 0; k < 4; k++)
                        {
                            if (WinNumber.Substring(k, 1) == Lottery[i].Substring(k, 1))
                            {
                                count++;
                            }
                        }
                        if (count == 2)
                        {
                            WinCountRX2++;
                            WinMoney += WinMoney2;
                            WinMoneyNoWithTax += WinMoneyNoWithTax2;
                        }
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
                if (WinNumber.Length < 4)	//4: "1234"
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

                int count = 0;

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
                        count = 0;
                        for (int k = 0; k < 4; k++)
                        {
                            if (WinNumber.Substring(k, 1) == Lottery[i].Substring(k, 1))
                            {
                                count++;
                            }
                        }
                        if (count == 3)
                        {
                            WinCountRX3++;
                            WinMoney += WinMoney3;
                            WinMoneyNoWithTax += WinMoneyNoWithTax3;
                        }
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

            private double ComputeWin_Zhi(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney1, double WinMoneyNoWithTax1)	//计算中奖
            {
                WinNumber = WinNumber.Trim();
                if (WinNumber.Length < 4)	//3: "1234"
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
                    string[] Lottery = ToSingle_Zhi(Lotterys[ii], ref t_str);
                    if (Lottery == null)
                        continue;
                    if (Lottery.Length < 1)
                        continue;

                    for (int i = 0; i < Lottery.Length; i++)
                    {
                        if (Lottery[i].Length < 4)
                            continue;

                        if (Lottery[i] == WinNumber) //4个号码全中才有奖
                        {
                            Description1++;               //注数
                            WinMoney += WinMoney1;
                            WinMoneyNoWithTax += WinMoneyNoWithTax1;
                        }
                    }
                }

                if (Description1 > 0)
                    Description = "单选奖" + Description1.ToString() + "注。";

                return WinMoney;
            }
            private double ComputeWin_Zu(int PlayType, string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney1, double WinMoneyNoWithTax1, double WinMoney2, double WinMoneyNoWithTax2, double WinMoney3, double WinMoneyNoWithTax3, double WinMoney4, double WinMoneyNoWithTax4)	//计算中奖
            {
                WinNumber = WinNumber.Trim();
                if (WinNumber.Length < 4)	//3: "1234"
                    return -1;
                WinNumber = WinNumber.Substring(0, 4);

                string[] Lotterys = SplitLotteryNumber(Number);//Lotterys存放的是一组复式或者单市号码
                if (Lotterys == null)
                    return -2;
                if (Lotterys.Length < 1)
                    return -2;

                int Description1 = 0, Description2 = 0, Description3 = 0, Description4 = 0;
                double WinMoney = 0;
                WinMoneyNoWithTax = 0;
                Description = "";

                for (int ii = 0; ii < Lotterys.Length; ii++)
                {
                    string t_str = "";
                    string[] Lottery = ToSingle_Zu(Lotterys[ii], ref t_str, PlayType);
                    if (Lottery == null)
                        continue;
                    if (Lottery.Length < 1)
                        continue;

                    for (int i = 0; i < Lottery.Length; i++)
                    {
                        if (Sort(Lottery[i]) == Sort(WinNumber) || Sort(Lottery[i]).Equals(Sort(WinNumber)))
                        {
                            if (Lottery[i].Length < 4)
                                continue;

                            //计算组24注数(4不相同)
                            if (FilterRepeated(Lottery[i]).Length == 4)
                            {
                                Description4++;
                                WinMoney += WinMoney1;
                                WinMoneyNoWithTax += WinMoneyNoWithTax1;
                            }

                            //计算组12注数(两相同两不同)
                            if (FilterRepeated(Lottery[i]).Length == 3)
                            {
                                Description3++;
                                WinMoney += WinMoney2;
                                WinMoneyNoWithTax += WinMoneyNoWithTax2;
                            }

                            //计算组6注数(两两相同)
                            if ((FilterRepeated(Lottery[i]).Length == 2) && Sort(Lottery[i]).Substring(1, 1) != Sort(Lottery[i]).Substring(2, 1))
                            {
                                Description2++;
                                WinMoney += WinMoney3;
                                WinMoneyNoWithTax += WinMoneyNoWithTax3;
                            }

                            //计算组4注数(3个号码相同));
                            if ((FilterRepeated(Sort(Lottery[i])).Length == 2) && Sort(Lottery[i]).Substring(1, 1) == Sort(Lottery[i]).Substring(2, 1))
                            {
                                Description1++;
                                WinMoney += WinMoney4;
                                WinMoneyNoWithTax += WinMoneyNoWithTax4;
                            }
                        }
                    }
                }

                if (Description1 > 0)
                    Description = "组选4奖" + Description1.ToString() + "注。";
                if (Description2 > 0)
                    Description = "组选6奖" + Description2.ToString() + "注。";
                if (Description3 > 0)
                    Description = "组选12奖" + Description3.ToString() + "注。";
                if (Description4 > 0)
                    Description = "组选24奖" + Description4.ToString() + "注。";

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

                if ((PlayType == PlayType_X4ZXD) || (PlayType == PlayType_X4ZXF))
                    return AnalyseScheme_Zhi(Content, PlayType);

                if (PlayType == PlayType_X4ZX24 || PlayType == PlayType_X4ZX12 || PlayType == PlayType_X4ZX6 || PlayType == PlayType_X4ZX4)
                    return AnalyseScheme_Zu(Content, PlayType);

                if (PlayType == PlayType_X4ZX24_F || PlayType == PlayType_X4ZX12_F || PlayType == PlayType_X4ZX6_F || PlayType == PlayType_X4ZX4_F)
                    return AnalyseScheme_ZuF(Content, PlayType);

                return "";
            }

            #region AnalyseScheme 的具体方法  //(多组组选号码）
            private string AnalyseScheme_RX1(string Content, int PlayType)
            {
                string[] strs = Content.Split('\n');
                if (strs == null)
                    return "";
                if (strs.Length == 0)
                    return "";

                string Result = "";

                string RegexString = @"^(?<L0>(\d)|(_)|([(][\d]+?[)]))(?<L1>(\d)|(_)|([(][\d]+?[)]))(?<L2>(\d)|(_)|([(][\d]+?[)]))(?<L3>(\d)|(_)|([(][\d]+?[)]))";
                Regex regex = new Regex(RegexString, RegexOptions.Compiled | RegexOptions.IgnoreCase);

                for (int i = 0; i < strs.Length; i++)
                {
                    Match m = regex.Match(strs[i]);
                    if (m.Success)
                    {
                        string CanonicalNumber = strs[i];
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

                string RegexString = @"^(?<L0>(\d)|(_)|([(][\d]+?[)]))(?<L1>(\d)|(_)|([(][\d]+?[)]))(?<L2>(\d)|(_)|([(][\d]+?[)]))(?<L3>(\d)|(_)|([(][\d]+?[)]))";
                Regex regex = new Regex(RegexString, RegexOptions.Compiled | RegexOptions.IgnoreCase);

                for (int i = 0; i < strs.Length; i++)
                {
                    Match m = regex.Match(strs[i]);
                    if (m.Success)
                    {
                        string CanonicalNumber = strs[i];
                        string[] singles = ToSingle_RX2(m.Value, ref CanonicalNumber);
                        if (singles == null)
                            continue;
                        if (singles.Length >= 1)
                            Result += CanonicalNumber + "|" + singles.Length + "\n";
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

                //string RegexString = @"^((\d\d\s){2,10}\d\d)";
                string RegexString = @"^(?<L0>(\d)|(_)|([(][\d]+?[)]))(?<L1>(\d)|(_)|([(][\d]+?[)]))(?<L2>(\d)|(_)|([(][\d]+?[)]))(?<L3>(\d)|(_)|([(][\d]+?[)]))";
                Regex regex = new Regex(RegexString, RegexOptions.Compiled | RegexOptions.IgnoreCase);

                for (int i = 0; i < strs.Length; i++)
                {
                    Match m = regex.Match(strs[i]);
                    if (m.Success)
                    {
                        string CanonicalNumber = strs[i];
                        string[] singles = ToSingle_RX3(m.Value, ref CanonicalNumber);
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
            private string AnalyseScheme_Zhi(string Content, int PlayType)
            {
                string[] strs = Content.Split('\n');
                if (strs == null)
                    return "";
                if (strs.Length == 0)
                    return "";

                string Result = "";

                string RegexString;
                if (PlayType == PlayType_X4ZXD)
                    RegexString = @"^([\d]){4}";
                else
                    RegexString = @"^(([\d])|([(][\d]{1,8}[)])){4}";
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
                        if (singles.Length >= ((PlayType == PlayType_X4ZXD) ? 1 : 2))
                            Result += CanonicalNumber + "|" + singles.Length.ToString() + "\n";
                    }
                }

                if (Result.EndsWith("\n"))
                    Result = Result.Substring(0, Result.Length - 1);
                return Result;
            }
            private string AnalyseScheme_Zu(string Content, int PlayType)
            {
                string[] strs = Content.Split('\n');
                if (strs == null)
                    return "";
                if (strs.Length == 0)
                    return "";

                string Result = "";

                string RegexString = @"^(([\d])|([(][\d]{1,8}[)])){4}";                             //@"^([\d]){3,10}";
                Regex regex = new Regex(RegexString, RegexOptions.Compiled | RegexOptions.IgnoreCase);

                for (int i = 0; i < strs.Length; i++)
                {
                    Match m = regex.Match(strs[i]);
                    if (m.Success)
                    {
                        string CanonicalNumber = strs[i];
                        string[] singles = ToSingle(m.Value, ref CanonicalNumber, PlayType);
                        if (singles == null)
                            continue;
                        if (singles.Length >= 1)
                        {
                            Result += CanonicalNumber + "|" + singles.Length.ToString() + "\n";
                        }
                    }
                }

                if (Result.EndsWith("\n"))
                    Result = Result.Substring(0, Result.Length - 1);
                return Result;
            }

            private string AnalyseScheme_ZuF(string Content, int PlayType)
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
                        string[] singles = ToSingle(m.Value, ref CanonicalNumber, PlayType);
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

            #endregion

            public override bool AnalyseWinNumber(string Number)
            {
                string t_str = "";
                string[] WinLotteryNumber = ToSingle_Zhi(Number, ref t_str);

                if ((WinLotteryNumber == null) || (WinLotteryNumber.Length != 1))
                    return false;

                return true;
            }

            //过滤重复号
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
                    //Result[i] = al[i].ToString().PadLeft(2, '0');
                    Result[i] = al[i].ToString();
                return Result;
            }

            public override string ShowNumber(string Number)
            {
                return base.ShowNumber(Number, " ");
            }

            private int getCount(ArrayList list, string value)
            {
                int cnt = 0;
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].ToString() == value)
                    {
                        cnt++;
                    }
                }
                return cnt;
            }
        }
    }
}
