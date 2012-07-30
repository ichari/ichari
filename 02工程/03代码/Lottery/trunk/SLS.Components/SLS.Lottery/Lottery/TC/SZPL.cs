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
        /// 数字排列
        /// </summary>
        public partial class SZPL : LotteryBase
        {
            #region 静态变量
            public const int PlayType_3_ZhiD = 401;
            public const int PlayType_3_ZhiF = 402;
            public const int PlayType_3_ZuD = 403;
            public const int PlayType_3_Zu6F = 404;
            public const int PlayType_3_Zu3F = 405;
            public const int PlayType_3_ZhiH = 406;
            public const int PlayType_3_ZuH = 407;
            public const int PlayType_5_D = 408;
            public const int PlayType_5_F = 409;

            public const int ID = 4;
            public const string sID = "4";
            public const string Name = "数字排列";
            public const string Code = "SZPL";
            public const double MaxMoney = 20000;
            #endregion

            public SZPL()
            {
                id = 4;
                name = "数字排列";
                code = "SZPL";
            }

            public override bool CheckPlayType(int play_type)
            {
                return ((play_type >= 401) && (play_type <= 409));
            }

            public override PlayType[] GetPlayTypeList()
            {
                PlayType[] Result = new PlayType[9];

                Result[0] = new PlayType(PlayType_3_ZhiD, "排列3直选单式");
                Result[1] = new PlayType(PlayType_3_ZhiF, "排列3直选复式");
                Result[2] = new PlayType(PlayType_3_ZuD, "排列3组选单式");
                Result[3] = new PlayType(PlayType_3_Zu6F, "排列3组选6复式");
                Result[4] = new PlayType(PlayType_3_Zu3F, "排列3组选3复式");
                Result[5] = new PlayType(PlayType_3_ZhiH, "排列3直选和值");
                Result[6] = new PlayType(PlayType_3_ZuH, "排列3组选和值");
                Result[7] = new PlayType(PlayType_5_D, "排列5单式");
                Result[8] = new PlayType(PlayType_5_F, "排列5复式");

                return Result;
            }

            public override string BuildNumber(int Num, int Type)	//id = 4, Type = 3 表示排列3, = 5 表示排列5
            {
                if ((Type != 3) && (Type != 5))
                    Type = 5;

                System.Random rd = new Random();
                StringBuilder sb = new StringBuilder();

                for (int i = 0; i < Num; i++)
                {
                    string LotteryNumber = "";
                    for (int j = 0; j < Type; j++)
                        LotteryNumber += rd.Next(0, 9 + 1).ToString();

                    sb.Append(LotteryNumber.Trim() + "\n");
                }

                string Result = sb.ToString();
                Result = Result.Substring(0, Result.Length - 1);
                return Result;
            }

            public override string[] ToSingle(string Number, ref string CanonicalNumber, int PlayType)
            {
                if ((PlayType == PlayType_3_ZhiD) || (PlayType == PlayType_3_ZhiF))
                    return ToSingle_3(Number, ref CanonicalNumber);

                if ((PlayType == PlayType_3_ZuD) || (PlayType == PlayType_3_Zu6F))
                    return ToSingle_Zu3D_Zu6(Number, ref CanonicalNumber);

                if ((PlayType == PlayType_3_Zu3F))
                    return ToSingle_Zu3F(Number, ref CanonicalNumber);

                if ((PlayType == PlayType_3_ZhiH))
                    return ToSingle_ZhiH(Number, ref CanonicalNumber);

                if ((PlayType == PlayType_3_ZuH))
                    return ToSingle_ZuH(Number, ref CanonicalNumber);

                if ((PlayType == PlayType_5_D) || (PlayType == PlayType_5_F))
                    return ToSingle_5(Number, ref CanonicalNumber);

                return null;
            }
            #region ToSingle 的具体方法
            private string[] ToSingle_3(string Number, ref string CanonicalNumber)	//复式取单式, 后面 ref 参数是将彩票规范化，如：10(223) 变成10(23)
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
            private string[] ToSingle_Zu3D_Zu6(string Number, ref string CanonicalNumber)	//复式取单式, 后面 ref 参数是将彩票规范化，如：10223) 变成1023
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
            private string[] ToSingle_Zu3F(string Number, ref string CanonicalNumber)	//组选3取单式,后面 ref 参数是将彩票规范化，如：10223) 变成1023，由于单式是3个数，复式可以是2个数，所以，这个函数不同于其他彩种的类似函数，单式不能使用这个函数转换。
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
                    Result[i] = al[i].ToString();
                return Result;
            }
            private string[] ToSingle_ZhiH(string sBill, ref string CanonicalNumber)	//直选和值取单式,后面 ref 参数是将彩票规范化，如：05) 变成 5
            {
                int Bill = Shove._Convert.StrToInt(sBill, -1);
                CanonicalNumber = "";

                if ((Bill < 0) || (Bill > 27))
                {
                    CanonicalNumber = "";
                    return null;
                }

                CanonicalNumber = Bill.ToString();

                ArrayList al = new ArrayList();

                #region 循环取单式

                for (int i = 0; i <= 9; i++)
                    for (int j = 0; j <= 9; j++)
                        for (int k = 0; k <= 9; k++)
                            if (i + j + k == Bill)
                                al.Add(i.ToString() + j.ToString() + k.ToString());

                #endregion

                if (al.Count == 0)
                    return null;

                string[] Result = new string[al.Count];
                for (int i = 0; i < al.Count; i++)
                    Result[i] = al[i].ToString();
                return Result;
            }
            private string[] ToSingle_ZuH(string sBill, ref string CanonicalNumber)	//组选6和值取单式,后面 ref 参数是将彩票规范化，如：05) 变成 5
            {
                int Bill = Shove._Convert.StrToInt(sBill, -1);
                CanonicalNumber = "";

                ArrayList al = new ArrayList();

                #region 循环取单式

                if ((Bill < 1) || (Bill > 26))
                {
                    CanonicalNumber = "";
                    return null;
                }
                else
                {
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
            private string[] ToSingle_5(string Number, ref string CanonicalNumber)	//复式取单式, 后面 ref 参数是将彩票规范化，如：10(223)45 变成10(23)45
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
            private string[] ToSingle_3_ZhiDanT(string Number, ref string CanonicalNumber)//直选胆拖,后面 ref 参数是将彩票规范化，如：1,334 变成134, 143, 314, 341, 413, 431
            {
                string[] Locate = new string[2];
                CanonicalNumber = "";

                Regex regex = new Regex(@"^(?<L0>(\d){1,2})[,](?<L1>(\d){1,9})", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                Match m = regex.Match(Number);

                for (int i = 0; i < 2; i++)
                {
                    Locate[i] = m.Groups["L" + i.ToString()].ToString().Trim();
                    if (Locate[i] == "")
                    {
                        CanonicalNumber = "";
                        return null;
                    }

                    if (Locate[i].Length > 0)
                    {
                        CanonicalNumber += Locate[i] + ",";
                    }

                    Locate[i] = FilterRepeated(Locate[i]);
                }

                if ((Locate[0].Length + Locate[1].Length) < 4)
                {
                    CanonicalNumber = "";
                    return null;
                }

                CanonicalNumber = CanonicalNumber.Substring(0, CanonicalNumber.Length - 1);

                char[] strs = Locate[1].ToCharArray();

                ArrayList al = new ArrayList();

                int n = strs.Length;

                if (Locate[0].Length == 1)
                {
                    for (int i = 0; i < n - 1; i++)
                        for (int j = i + 1; j < n; j++)
                            al.Add(Locate[0] + strs[i].ToString() + strs[j].ToString());
                }
                else
                {
                    for (int i = 0; i < n; i++)
                        al.Add(Locate[0] + strs[i]);
                }

                #region 循环取单式
                ArrayList alnumber = new ArrayList();

                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        for (int k = 0; k < 3; k++)
                        {
                            for (int a = 0; a < al.Count; a++)
                            {
                                if ((i != j) && (j != k) && (k != i))
                                {
                                    char[] strsal = al[a].ToString().ToCharArray();
                                    alnumber.Add(strsal[i].ToString() + strsal[j].ToString() + strsal[k].ToString());
                                }
                            }
                        }
                    }
                }
                #endregion

                string[] Result = new string[alnumber.Count];
                for (int i = 0; i < alnumber.Count; i++)
                    Result[i] = alnumber[i].ToString();
                return Result;
            }
            private string[] ToSingle_3_Zu6DanT(string Number, ref string CanonicalNumber)//组选6胆拖,后面 ref 参数是将彩票规范化，如：1,334 变成134
            {
                CanonicalNumber = "";

                Regex regex = new Regex(@"^(\d){1,10}", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                Match m = regex.Match(Number);

                if (m.Success)
                {
                    #region 循环取单式
                    ArrayList al = new ArrayList();

                    for (int i = 0; i < 9; i++)
                    {
                        for (int j = i + 1; j < 10; j++)
                        {
                            for (int k = 0; k < m.Value.Length; k++)
                            {
                                if ((i != int.Parse(m.Value.Substring(k, 1))) && (j != int.Parse(m.Value.Substring(k, 1))))
                                {
                                    al.Add(i.ToString() + j.ToString() + m.Value.Substring(k, 1));
                                }
                            }
                        }
                    }

                    #endregion

                    CanonicalNumber = m.Value;

                    string[] Result = new string[al.Count];
                    for (int i = 0; i < al.Count; i++)
                        Result[i] = al[i].ToString();
                    return Result;
                }

                return null;
            }
            private string[] ToSingle_3_Zu3DanT(string Number, ref string CanonicalNumber)//组选3胆拖,后面 ref 参数是将彩票规范化，如：1,334 变成133, 144, 113, 114
            {
                CanonicalNumber = "";

                Regex regex = new Regex(@"^(\d){1,10}", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                Match m = regex.Match(Number);

                if (m.Success)
                {
                    #region 循环取单式
                    ArrayList al = new ArrayList();

                    for (int i = 0; i < 10; i++)
                    {
                        for (int k = 0; k < m.Value.Length; k++)
                        {
                            if (i != int.Parse(m.Value.Substring(k, 1)))
                            {
                                al.Add(i.ToString() + i.ToString() + m.Value.Substring(k, 1));
                                al.Add(i.ToString() + m.Value.Substring(k, 1) + m.Value.Substring(k, 1));
                            }
                        }
                    }

                    #endregion

                    CanonicalNumber = m.Value;

                    string[] Result = new string[al.Count];
                    for (int i = 0; i < al.Count; i++)
                        Result[i] = al[i].ToString();
                    return Result;
                }

                return null;
            }
            private string[] ToSingle_DWD(string Number, ref string CanonicalNumber)//定位胆,后面 ref 参数是将彩票规范化，如：1,4,_ 变成1,_,_  , _,4,_
            {
                string[] Locate = new string[3];
                CanonicalNumber = "";

                Regex regex = new Regex(@"^(?<L0>((\d){1,10})|[-])[,](?<L1>((\d){1,10})|[-])[,](?<L2>((\d){1,10})|[-])", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                Match m = regex.Match(Number);

                for (int i = 0; i < 3; i++)
                {
                    Locate[i] = m.Groups["L" + i.ToString()].ToString().Trim();

                    if (Locate[i] == "")
                    {
                        CanonicalNumber = "";
                        return null;
                    }

                    Locate[i] = FilterRepeated(Locate[i]);
                    CanonicalNumber += Locate[i] + ",";
                }

                CanonicalNumber = CanonicalNumber.Substring(0, (CanonicalNumber.Length - 1));

                if (CanonicalNumber == "-,-,-")
                {
                    CanonicalNumber = "";
                }

                #region 循环取单式
                ArrayList al = new ArrayList();

                for (int i = 0; i < 3; i++)
                {
                    if (Locate[i].Length > 1)
                    {
                        if (i == 0)
                        {
                            for (int j = 0; j < Locate[0].Length; j++)
                            {
                                al.Add(Locate[i].Substring(j, 1) + ",-,-");
                            }
                        }

                        if (i == 1)
                        {
                            for (int j = 0; j < Locate[1].Length; j++)
                            {
                                al.Add("-," + Locate[i].Substring(j, 1) + ",-");
                            }
                        }

                        if (i == 2)
                        {
                            for (int j = 0; j < Locate[2].Length; j++)
                            {
                                al.Add("-,-," + Locate[i].Substring(j, 1));
                            }
                        }

                    }
                    else if (Locate[i] != "-")
                    {
                        if (i == 0)
                        {
                            al.Add(Locate[0] + ",-,-");
                        }

                        if (i == 1)
                        {

                            al.Add("-," + Locate[1] + ",-");

                        }

                        if (i == 2)
                        {
                            al.Add("-,-," + Locate[2]);
                        }
                    }
                }

                #endregion

                string[] Result = new string[al.Count];
                for (int i = 0; i < al.Count; i++)
                    Result[i] = al[i].ToString();
                return Result;
            }
            private string[] ToSingle_BuDWD(string Number, ref string CanonicalNumber)//不定位胆,后面 ref 参数是将彩票规范化，如：1334 变成134
            {
                CanonicalNumber = "";

                Regex regex = new Regex(@"^((\d){1,10})", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                Match m = regex.Match(Number);

                if (m.Success)
                {
                    CanonicalNumber = FilterRepeated(m.Value);
                }

                #region 循环取单式
                ArrayList al = new ArrayList();

                for (int i = 0; i < CanonicalNumber.Length; i++)
                {
                    al.Add(CanonicalNumber.Substring(i, 1));
                }
                #endregion
                string[] Result = new string[al.Count];

                for (int i = 0; i < al.Count; i++)
                {
                    Result[i] = al[i].ToString();
                }

                return Result;
            }
            private string[] ToSingle_2(string Number, ref string CanonicalNumber)      //前2后2复式取单式, 后面 ref 参数是将彩票规范化，如：1(223) 变成1(23)
            {
                string[] Locate = new string[2];
                CanonicalNumber = "";

                Regex regex = new Regex(@"^(?<L0>(\d)|([(][\d]+?[)]))(?<L1>(\d)|([(][\d]+?[)]))", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                Match m = regex.Match(Number);
                for (int i = 0; i < 2; i++)
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
                        al.Add(str_1);
                    }
                }
                #endregion

                string[] Result = new string[al.Count];
                for (int i = 0; i < al.Count; i++)
                    Result[i] = al[i].ToString();
                return Result;
            }
            private string[] ToSingle_13(string Number, ref string CanonicalNumber)      //首尾复式取单式, 后面 ref 参数是将彩票规范化，如：1-(223) 变成1-(23)
            {
                string[] Locate = new string[2];
                CanonicalNumber = "";

                Regex regex = new Regex(@"^(?<L0>(\d)|([(][\d]+?[)]))[-](?<L1>(\d)|([(][\d]+?[)]))", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                Match m = regex.Match(Number);
                for (int i = 0; i < 2; i++)
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

                    if (i == 0)
                    {
                        CanonicalNumber += "-";
                    }
                }

                ArrayList al = new ArrayList();

                #region 循环取单式
                for (int i_0 = 0; i_0 < Locate[0].Length; i_0++)
                {
                    string str_0 = Locate[0][i_0].ToString();
                    for (int i_1 = 0; i_1 < Locate[1].Length; i_1++)
                    {
                        string str_1 = str_0 + "-" + Locate[1][i_1].ToString();
                        al.Add(str_1);
                    }
                }
                #endregion

                string[] Result = new string[al.Count];
                for (int i = 0; i < al.Count; i++)
                    Result[i] = al[i].ToString();
                return Result;
            }
            private string[] ToSingle_ZhiKD(string Number, ref string CanonicalNumber)//直选跨度,后面 ref 参数是将彩票规范化，如：1334 变成1
            {
                CanonicalNumber = "";

                Regex regex = new Regex(@"^((\d){1})", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                Match m = regex.Match(Number);

                if (m.Success)
                {
                    CanonicalNumber = m.Value;
                }

                ArrayList al = new ArrayList();

                for (int i = 0; i < (10 - int.Parse(CanonicalNumber)); i++)
                {
                    for (int j = i + 1; j < (int.Parse(CanonicalNumber) + i); j++)
                    {
                        al.Add(i.ToString() + j.ToString() + (int.Parse(CanonicalNumber) + i).ToString());
                    }
                }

                #region 循环取单式
                ArrayList alnumber = new ArrayList();

                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        for (int k = 0; k < 3; k++)
                        {
                            for (int a = 0; a < al.Count; a++)
                            {
                                if ((i != j) && (j != k) && (k != i))
                                {
                                    char[] strsal = al[a].ToString().ToCharArray();
                                    alnumber.Add(strsal[i].ToString() + strsal[j].ToString() + strsal[k].ToString());
                                }
                            }
                        }
                    }
                }

                for (int i = 0; i < (10 - int.Parse(CanonicalNumber)); i++)
                {
                    if (int.Parse(CanonicalNumber) == 0)
                    {
                        alnumber.Add(i.ToString() + i.ToString() + i.ToString());
                    }
                    else
                    {
                        alnumber.Add(i.ToString() + (int.Parse(CanonicalNumber) + i).ToString() + (int.Parse(CanonicalNumber) + i).ToString());
                        alnumber.Add(i.ToString() + i.ToString() + (int.Parse(CanonicalNumber) + i).ToString());
                        alnumber.Add((int.Parse(CanonicalNumber) + i).ToString() + (int.Parse(CanonicalNumber) + i).ToString() + i.ToString());
                        alnumber.Add((int.Parse(CanonicalNumber) + i).ToString() + i.ToString() + i.ToString());
                        alnumber.Add((int.Parse(CanonicalNumber) + i).ToString() + i.ToString() + (int.Parse(CanonicalNumber) + i).ToString());
                        alnumber.Add(i.ToString() + (int.Parse(CanonicalNumber) + i).ToString() + i.ToString());
                    }
                }
                #endregion

                string[] Result = new string[alnumber.Count];

                for (int i = 0; i < alnumber.Count; i++)
                {
                    Result[i] = alnumber[i].ToString();
                }

                return Result;
            }
            private string[] ToSingle_Zu3KD(string Number, ref string CanonicalNumber)//组3跨度,后面 ref 参数是将彩票规范化，如：1334 变成1
            {
                CanonicalNumber = "";

                Regex regex = new Regex(@"^((\d){1})", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                Match m = regex.Match(Number);

                if (m.Success)
                {
                    CanonicalNumber = m.Value;
                }

                #region 循环取单式
                ArrayList al = new ArrayList();

                for (int i = 0; i < (10 - int.Parse(CanonicalNumber)); i++)
                {
                    al.Add(i.ToString() + (int.Parse(CanonicalNumber) + i).ToString() + (int.Parse(CanonicalNumber) + i).ToString());
                    al.Add(i.ToString() + i.ToString() + (int.Parse(CanonicalNumber) + i).ToString());
                }
                #endregion

                string[] Result = new string[al.Count];

                for (int i = 0; i < al.Count; i++)
                {
                    Result[i] = al[i].ToString();
                }

                return Result;
            }
            private string[] ToSingle_Zu6KD(string Number, ref string CanonicalNumber)//组6跨度,后面 ref 参数是将彩票规范化，如：1334 变成1
            {
                CanonicalNumber = "";

                Regex regex = new Regex(@"^((\d){1})", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                Match m = regex.Match(Number);

                if (m.Success)
                {
                    CanonicalNumber = m.Value;
                }

                #region 循环取单式
                ArrayList al = new ArrayList();

                for (int i = 0; i < (10 - int.Parse(CanonicalNumber)); i++)
                {
                    for (int j = i + 1; j < (int.Parse(CanonicalNumber) + i); j++)
                    {
                        al.Add(i.ToString() + j.ToString() + (int.Parse(CanonicalNumber) + i).ToString());
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
            private string[] ToSingle_DX_1WDW(string Number, ref string CanonicalNumber)//大小单双一位定位竞猜,后面 ref 参数是将彩票规范化
            {
                CanonicalNumber = "";

                Regex regex = new Regex(@"^(?<L0>([大小单双])|(-))(?<L1>([大小单双])|(-))(?<L2>([大小单双])|(-))", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                Match m = regex.Match(Number);
                for (int i = 0; i < 3; i++)
                {
                    string Locate = m.Groups["L" + i.ToString()].ToString().Trim();
                    if (Locate == "")
                    {
                        CanonicalNumber = "";
                        return null;
                    }

                    CanonicalNumber += Locate;
                }

                if (CanonicalNumber.Replace("-", "").Length != 1)
                {
                    CanonicalNumber = "";

                    return null;
                }

                string[] Result = new string[1];
                Result[0] = CanonicalNumber;

                return Result;
            }
            private string[] ToSingle_DX_H3WDW(string Number, ref string CanonicalNumber)//大小单双后三位定位直选,后面 ref 参数是将彩票规范化
            {
                CanonicalNumber = "";

                Regex regex = new Regex(@"^(?<L0>([大小单双]))(?<L1>([大小单双]))(?<L2>([大小单双]))", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                Match m = regex.Match(Number);
                for (int i = 0; i < 3; i++)
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
            #endregion

            public override double ComputeWin(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, int PlayType, params double[] WinMoneyList)
            {
                if ((WinMoneyList == null) || (WinMoneyList.Length < 8))    // 奖金参数排列顺序 zhi, zu3 zu6, 排列5
                    return -3;

                if ((PlayType == PlayType_3_ZhiD) || (PlayType == PlayType_3_ZhiF))
                    return ComputeWin_3(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[0], WinMoneyList[1]);

                if ((PlayType == PlayType_3_ZuD) || (PlayType == PlayType_3_Zu6F))
                    return ComputeWin_Zu3D_Zu6(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[2], WinMoneyList[3], WinMoneyList[4], WinMoneyList[5]);

                if (PlayType == PlayType_3_Zu3F)
                    return ComputeWin_Zu3F(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[2], WinMoneyList[3]);

                if (PlayType == PlayType_3_ZhiH)
                    return ComputeWin_ZhiH(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[0], WinMoneyList[1]);

                if (PlayType == PlayType_3_ZuH)
                    return ComputeWin_ZuH(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[2], WinMoneyList[3], WinMoneyList[4], WinMoneyList[5]);

                if ((PlayType == PlayType_5_D) || (PlayType == PlayType_5_F))
                    return ComputeWin_5(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[6], WinMoneyList[7]);

                return -4;
            }
            #region ComputeWin 的具体方法
            private double ComputeWin_3(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney1, double WinMoneyNoWithTax1)	//计算中奖
            {
                WinNumber = WinNumber.Trim();
                if (WinNumber.Length < 3)	//3: "123"
                    return -1;
                WinNumber = WinNumber.Substring(0, 3);

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
                    string[] Lottery = ToSingle_3(Lotterys[ii], ref t_str);
                    if (Lottery == null)
                        continue;
                    if (Lottery.Length < 1)
                        continue;

                    for (int i = 0; i < Lottery.Length; i++)
                    {
                        if (Lottery[i].Length < 3)
                            continue;

                        if (Lottery[i] == WinNumber)
                        {
                            Description1++;
                            WinMoney += WinMoney1;
                            WinMoneyNoWithTax += WinMoneyNoWithTax1;
                        }
                    }
                }

                if (Description1 > 0)
                    Description = "单选奖" + Description1.ToString() + "注。";

                return WinMoney;
            }
            private double ComputeWin_Zu3D_Zu6(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney1, double WinMoneyNoWithTax1, double WinMoney2, double WinMoneyNoWithTax2)	//计算中奖
            {
                WinNumber = WinNumber.Trim();
                if (WinNumber.Length < 3)	//3: "123"
                    return -1;
                WinNumber = WinNumber.Substring(0, 3);

                string[] Lotterys = SplitLotteryNumber(Number);
                if (Lotterys == null)
                    return -2;
                if (Lotterys.Length < 1)
                    return -2;

                int Description1 = 0, Description2 = 0;
                double WinMoney = 0;
                WinMoneyNoWithTax = 0;
                Description = "";

                for (int ii = 0; ii < Lotterys.Length; ii++)
                {
                    string t_str = "";

                    if (Lotterys[ii].Length < 3)
                        continue;
                    if (FilterRepeated(Sort(Lotterys[ii])).Length == 2)
                    {
                        if (Sort(Lotterys[ii]) == Sort(WinNumber))
                        {
                            Description1++;
                            WinMoney += WinMoney1;
                            WinMoneyNoWithTax += WinMoneyNoWithTax1;
                        }

                        continue;
                    }

                    string[] Lottery = ToSingle_Zu3D_Zu6(Lotterys[ii], ref t_str);
                    if (Lottery == null)
                        continue;
                    if (Lottery.Length < 1)
                        continue;
                    for (int i = 0; i < Lottery.Length; i++)
                    {
                        if (Lottery[i].Length < 3)
                            continue;

                        if (Sort(Lottery[i]) == Sort(WinNumber))
                        {
                            Description2++;
                            WinMoney += WinMoney2;
                            WinMoneyNoWithTax += WinMoneyNoWithTax2;
                        }
                    }
                }

                if (Description1 > 0)
                    Description = "组选3奖" + Description1.ToString() + "注。";
                if (Description2 > 0)
                    Description = "组选6奖" + Description2.ToString() + "注。";

                return WinMoney;
            }
            private double ComputeWin_Zu3F(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney1, double WinMoneyNoWithTax1)	//计算中奖
            {
                WinNumber = WinNumber.Trim();
                if (WinNumber.Length < 3)	//3: "223"
                    return -1;
                WinNumber = WinNumber.Substring(0, 3);

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
                    string[] Lottery = ToSingle_Zu3F(Lotterys[ii], ref t_str);
                    if (Lottery == null)
                        continue;
                    if (Lottery.Length < 1)
                        continue;

                    for (int i = 0; i < Lottery.Length; i++)
                    {
                        if (Lottery[i].Length < 3)
                            continue;

                        if (Sort(Lottery[i]) == Sort(WinNumber))
                        {
                            Description1++;
                            WinMoney += WinMoney1;
                            WinMoneyNoWithTax += WinMoneyNoWithTax1;
                        }
                    }
                }

                if (Description1 > 0)
                    Description = "组选3奖" + Description1.ToString() + "注。";

                return WinMoney;
            }
            private double ComputeWin_ZhiH(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney1, double WinMoneyNoWithTax1)	//计算中奖
            {
                WinNumber = WinNumber.Trim();
                if (WinNumber.Length < 3)	//3: "123"
                    return -1;
                WinNumber = WinNumber.Substring(0, 3);

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
                    string[] Lottery = ToSingle_ZhiH(Lotterys[ii], ref t_str);
                    if (Lottery == null)
                        continue;
                    if (Lottery.Length < 1)
                        continue;

                    for (int i = 0; i < Lottery.Length; i++)
                    {
                        if (Lottery[i].Length < 3)
                            continue;

                        if (Lottery[i] == WinNumber)
                        {
                            Description1++;
                            WinMoney += WinMoney1;
                            WinMoneyNoWithTax += WinMoneyNoWithTax1;
                        }
                    }
                }

                if (Description1 > 0)
                    Description = "单选奖" + Description1.ToString() + "注。";

                return WinMoney;
            }
            private double ComputeWin_ZuH(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney1, double WinMoneyNoWithTax1, double WinMoney2, double WinMoneyNoWithTax2)	//计算中奖
            {
                WinNumber = WinNumber.Trim();
                if (WinNumber.Length < 3)	//3: "123"
                    return -1;
                WinNumber = WinNumber.Substring(0, 3);

                string[] Lotterys = SplitLotteryNumber(Number);
                if (Lotterys == null)
                    return -2;
                if (Lotterys.Length < 1)
                    return -2;

                int Description1 = 0, Description2 = 0;
                double WinMoney = 0;
                WinMoneyNoWithTax = 0;
                Description = "";

                for (int ii = 0; ii < Lotterys.Length; ii++)
                {
                    string t_str = "";
                    string[] Lottery = ToSingle_ZuH(Lotterys[ii], ref t_str);
                    if (Lottery == null)
                        continue;
                    if (Lottery.Length < 1)
                        continue;

                    for (int i = 0; i < Lottery.Length; i++)
                    {
                        if (Lottery[i].Length < 3)
                            continue;

                        if (Sort(Lottery[i]) == Sort(WinNumber))
                        {
                            if (FilterRepeated(Lottery[i]).Length == 2)
                            {
                                Description1++;
                                WinMoney += WinMoney1;
                                WinMoneyNoWithTax += WinMoneyNoWithTax1;
                            }
                            else
                            {
                                Description2++;
                                WinMoney += WinMoney2;
                                WinMoneyNoWithTax += WinMoneyNoWithTax2;
                            }
                        }
                    }
                }

                if (Description1 > 0)
                    Description = "组选3奖" + Description1.ToString() + "注。";
                if (Description2 > 0)
                    Description = "组选6奖" + Description2.ToString() + "注。";

                return WinMoney;
            }
            private double ComputeWin_5(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney1, double WinMoneyNoWithTax1)	//计算中奖
            {
                WinNumber = WinNumber.Trim();
                if (WinNumber.Length < 5)	//3: "12345"
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
                    string[] Lottery = ToSingle_5(Lotterys[ii], ref t_str);
                    if (Lottery == null)
                        continue;
                    if (Lottery.Length < 1)
                        continue;

                    for (int i = 0; i < Lottery.Length; i++)
                    {
                        if (Lottery[i].Length < 5)
                            continue;

                        if (Lottery[i] == WinNumber)
                        {
                            Description1++;
                            WinMoney += WinMoney1;
                            WinMoneyNoWithTax += WinMoneyNoWithTax1;
                        }
                    }
                }

                if (Description1 > 0)
                    Description = "排列5直选奖" + Description1.ToString() + "注。";

                return WinMoney;
            }
            #endregion

            public override string AnalyseScheme(string Content, int PlayType)
            {
                if ((PlayType == PlayType_3_ZhiD) || (PlayType == PlayType_3_ZhiF))
                    return AnalyseScheme_3(Content, PlayType);

                if ((PlayType == PlayType_3_ZuD) || (PlayType == PlayType_3_Zu6F))
                    return AnalyseScheme_Zu3D_Zu6(Content, PlayType);

                if (PlayType == PlayType_3_Zu3F)
                    return AnalyseScheme_Zu3F(Content, PlayType);

                if (PlayType == PlayType_3_ZhiH)
                    return AnalyseScheme_ZhiH(Content, PlayType);

                if (PlayType == PlayType_3_ZuH)
                    return AnalyseScheme_ZuH(Content, PlayType);

                if ((PlayType == PlayType_5_D) || (PlayType == PlayType_5_F))
                    return AnalyseScheme_5(Content, PlayType);

                return "";
            }

            #region AnalyseScheme 的具体方法
            private string AnalyseScheme_3(string Content, int PlayType)
            {
                string[] strs = Content.Split('\n');
                if (strs == null)
                    return "";
                if (strs.Length == 0)
                    return "";

                string Result = "";

                string RegexString;
                if (PlayType == PlayType_3_ZhiD)
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
                        string[] singles = ToSingle_3(m.Value, ref CanonicalNumber);
                        if (singles == null)
                            continue;
                        if (singles.Length >= ((PlayType == PlayType_3_ZhiD) ? 1 : 2))
                            Result += CanonicalNumber + "|" + singles.Length.ToString() + "\n";
                    }
                }

                if (Result.EndsWith("\n"))
                    Result = Result.Substring(0, Result.Length - 1);
                return Result;
            }
            private string AnalyseScheme_Zu3D_Zu6(string Content, int PlayType)
            {
                string[] strs = Content.Split('\n');
                if (strs == null)
                    return "";
                if (strs.Length == 0)
                    return "";

                string Result = "";

                string RegexString;
                if (PlayType == PlayType_3_ZuD)
                    RegexString = @"^([\d]){3}";
                else
                    RegexString = @"^([\d]){3,10}";
                Regex regex = new Regex(RegexString, RegexOptions.Compiled | RegexOptions.IgnoreCase);

                for (int i = 0; i < strs.Length; i++)
                {
                    Match m = regex.Match(strs[i]);
                    if (m.Success)
                    {
                        string CanonicalNumber = "";
                        string[] singles = ToSingle_Zu3D_Zu6(m.Value, ref CanonicalNumber);
                        if (singles == null)
                            continue;
                        if (singles.Length >= ((PlayType == PlayType_3_ZuD) ? 1 : 2))
                        {
                            if (FilterRepeated(Sort(m.Value)).Length == 2)
                            {
                                if (PlayType != PlayType_3_Zu6F)
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
            private string AnalyseScheme_Zu3F(string Content, int PlayType)
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
                        string[] singles = ToSingle_Zu3F(m.Value, ref CanonicalNumber);
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
            private string AnalyseScheme_ZhiH(string Content, int PlayType)
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
                        string[] singles = ToSingle_ZhiH(m.Value, ref CanonicalNumber);
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
            private string AnalyseScheme_ZuH(string Content, int PlayType)
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
                        string[] singles = ToSingle_ZuH(m.Value, ref CanonicalNumber);
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
            private string AnalyseScheme_5(string Content, int PlayType)
            {
                string[] strs = Content.Split('\n');
                if (strs == null)
                    return "";
                if (strs.Length == 0)
                    return "";

                string Result = "";

                string RegexString;
                if (PlayType == PlayType_5_D)
                    RegexString = @"^([\d]){5}";
                else
                    RegexString = @"^(([\d])|([(][\d]{1,10}[)])){5}";
                Regex regex = new Regex(RegexString, RegexOptions.Compiled | RegexOptions.IgnoreCase);

                for (int i = 0; i < strs.Length; i++)
                {
                    Match m = regex.Match(strs[i]);
                    if (m.Success)
                    {
                        string CanonicalNumber = "";
                        string[] singles = ToSingle_5(m.Value, ref CanonicalNumber);
                        if (singles == null)
                            continue;
                        if ((singles.Length >= ((PlayType == PlayType_5_D) ? 1 : 2)) && (singles.Length <= (MaxMoney / 2)))
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
                string[] WinLotteryNumber = ToSingle_5(Number, ref t_str);

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
                        if (PlayTypeID == PlayType_3_ZhiD)//排3只单
                        {
                            return GetPrintKeyList_CR_YTCII2_ZhiD(Numbers);
                        }
                        if (PlayTypeID == PlayType_3_ZhiF)//排3只复
                        {
                            return GetPrintKeyList_CR_YTCII2_3_ZhiF(Numbers);
                        }
                        if (PlayTypeID == PlayType_3_ZuD)   //组选单式
                        {
                            return GetPrintKeyList_CR_YTCII2_ZhiD(Numbers);
                        }
                        if (PlayTypeID == PlayType_3_Zu3F)//排3组3复式
                        {
                            return GetPrintKeyList_CR_YTCII2_ZhiD(Numbers);
                        }
                        if (PlayTypeID == PlayType_3_Zu6F)//排3组6复式
                        {
                            return GetPrintKeyList_CR_YTCII2_ZhiD(Numbers);
                        }
                        if ((PlayTypeID == PlayType_3_ZhiH) || (PlayTypeID == PlayType_3_ZuH))//和值
                        {
                            return GetPrintKeyList_CR_YTCII2_H(Numbers);
                        }
                        if (PlayTypeID == PlayType_5_D) //排5单式
                        {
                            return GetPrintKeyList_CR_YTCII2_ZhiD(Numbers);
                        }
                        if (PlayTypeID == PlayType_5_F)//排5复式
                        {
                            return GetPrintKeyList_CR_YTCII2_5_F(Numbers);
                        }
                        break;
                    case "TCBJYTD":
                        if (PlayTypeID == PlayType_3_ZhiD)//排3只单
                        {
                            return GetPrintKeyList_TCBJYTD_ZhiD(Numbers);
                        }
                        if (PlayTypeID == PlayType_3_ZhiF)//排3只复
                        {
                            return GetPrintKeyList_TCBJYTD_3_ZhiF(Numbers);
                        }
                        if (PlayTypeID == PlayType_3_ZuD)   //组选单式
                        {
                            return GetPrintKeyList_TCBJYTD_ZhiD(Numbers);
                        }
                        if (PlayTypeID == PlayType_3_Zu3F)//排3组3复式
                        {
                            return GetPrintKeyList_TCBJYTD_ZhiD(Numbers);
                        }
                        if (PlayTypeID == PlayType_3_Zu6F)//排3组6复式
                        {
                            return GetPrintKeyList_TCBJYTD_ZhiD(Numbers);
                        }
                        if ((PlayTypeID == PlayType_3_ZhiH) || (PlayTypeID == PlayType_3_ZuH))//和值
                        {
                            return GetPrintKeyList_TCBJYTD_H(Numbers);
                        }
                        if (PlayTypeID == PlayType_5_D) //排5单式
                        {
                            return GetPrintKeyList_TCBJYTD_ZhiD(Numbers);
                        }
                        if (PlayTypeID == PlayType_5_F)//排5复式
                        {
                            return GetPrintKeyList_TCBJYTD_5_F(Numbers);
                        }
                        break;
                    case "TGAMPOS4000":
                        if (PlayTypeID == PlayType_3_ZhiD)//排3只单
                        {
                            return GetPrintKeyList_TGAMPOS4000_ZhiD(Numbers);
                        }
                        if (PlayTypeID == PlayType_3_ZhiF)//排3只复
                        {
                            return GetPrintKeyList_TGAMPOS4000_3_ZhiF(Numbers);
                        }
                        if (PlayTypeID == PlayType_3_ZuD)  //组选单式
                        {
                            return GetPrintKeyList_TGAMPOS4000_ZhiD(Numbers);
                        }
                        if (PlayTypeID == PlayType_3_Zu3F)//排3组3复式
                        {
                            return GetPrintKeyList_TGAMPOS4000_ZhiD(Numbers);
                        }
                        if (PlayTypeID == PlayType_3_Zu6F)//排3组6复式
                        {
                            return GetPrintKeyList_TGAMPOS4000_ZhiD(Numbers);
                        }
                        if ((PlayTypeID == PlayType_3_ZhiH) || (PlayTypeID == PlayType_3_ZuH))//和值
                        {
                            return GetPrintKeyList_TGAMPOS4000_H(Numbers);
                        }
                        if (PlayTypeID == PlayType_5_D) //排5单式
                        {
                            return GetPrintKeyList_TGAMPOS4000_ZhiD(Numbers);
                        }
                        if (PlayTypeID == PlayType_5_F)//排5复式
                        {
                            return GetPrintKeyList_TGAMPOS4000_5_F(Numbers);
                        }
                        break;
                    case "CP86":
                        if (PlayTypeID == PlayType_3_ZhiD)//排3只单
                        {
                            return GetPrintKeyList_CP86_ZhiD(Numbers);
                        }
                        if (PlayTypeID == PlayType_3_ZhiF)//排3只复
                        {
                            return GetPrintKeyList_CP86_ZhiF(Numbers);
                        }
                        if (PlayTypeID == PlayType_3_ZuD)  //组选单式
                        {
                            return GetPrintKeyList_CP86_ZhiD(Numbers);
                        }
                        if (PlayTypeID == PlayType_3_Zu3F)//排3组3复式
                        {
                            return GetPrintKeyList_CP86_ZhiD(Numbers);
                        }
                        if (PlayTypeID == PlayType_3_Zu6F)//排3组6复式
                        {
                            return GetPrintKeyList_CP86_ZhiD(Numbers);
                        }
                        if ((PlayTypeID == PlayType_3_ZhiH) || (PlayTypeID == PlayType_3_ZuH))//和值
                        {
                            return GetPrintKeyList_CP86_H(Numbers);
                        }
                        if (PlayTypeID == PlayType_5_D) //排5单式
                        {
                            return GetPrintKeyList_CP86_ZhiD(Numbers);
                        }
                        if (PlayTypeID == PlayType_5_F)//排5复式
                        {
                            return GetPrintKeyList_CP86_5_F(Numbers);
                        }
                        break;
                    case "MODEL_4000":
                        if (PlayTypeID == PlayType_3_ZhiD)//排3只单
                        {
                            return GetPrintKeyList_MODEL_4000_ZhiD(Numbers);
                        }
                        if (PlayTypeID == PlayType_3_ZhiF)//排3只复
                        {
                            return GetPrintKeyList_MODEL_4000_3_ZhiF(Numbers);
                        }
                        if (PlayTypeID == PlayType_3_ZuD)  //组选单式
                        {
                            return GetPrintKeyList_MODEL_4000_ZhiD(Numbers);
                        }
                        if (PlayTypeID == PlayType_3_Zu3F)//排3组3复式
                        {
                            return GetPrintKeyList_MODEL_4000_ZhiD(Numbers);
                        }
                        if (PlayTypeID == PlayType_3_Zu6F)//排3组6复式
                        {
                            return GetPrintKeyList_MODEL_4000_ZhiD(Numbers);
                        }
                        if ((PlayTypeID == PlayType_3_ZhiH) || (PlayTypeID == PlayType_3_ZuH))//和值
                        {
                            return GetPrintKeyList_MODEL_4000_H(Numbers);
                        }
                        if (PlayTypeID == PlayType_5_D) //排5单式
                        {
                            return GetPrintKeyList_MODEL_4000_ZhiD(Numbers);
                        }
                        if (PlayTypeID == PlayType_5_F)//排5复式
                        {
                            return GetPrintKeyList_MODEL_4000_5_F(Numbers);
                        }
                        break;
                    case "CORONISTPT":
                        if (PlayTypeID == PlayType_3_ZhiD)//排3只单
                        {
                            return GetPrintKeyList_CORONISTPT_ZhiD(Numbers);
                        }
                        if (PlayTypeID == PlayType_3_ZhiF)//排3只复
                        {
                            return GetPrintKeyList_CORONISTPT_3_ZhiF(Numbers);
                        }
                        if (PlayTypeID == PlayType_3_ZuD)  //组选单式
                        {
                            return GetPrintKeyList_CORONISTPT_ZhiD(Numbers);
                        }
                        if (PlayTypeID == PlayType_3_Zu3F)//排3组3复式
                        {
                            return GetPrintKeyList_CORONISTPT_ZhiD(Numbers);
                        }
                        if (PlayTypeID == PlayType_3_Zu6F)//排3组6复式
                        {
                            return GetPrintKeyList_CORONISTPT_ZhiD(Numbers);
                        }
                        if ((PlayTypeID == PlayType_3_ZhiH) || (PlayTypeID == PlayType_3_ZuH))//和值
                        {
                            return GetPrintKeyList_CORONISTPT_H(Numbers);
                        }
                        if (PlayTypeID == PlayType_5_D) //排5单式
                        {
                            return GetPrintKeyList_CORONISTPT_ZhiD(Numbers);
                        }
                        if (PlayTypeID == PlayType_5_F)//排5复式
                        {
                            return GetPrintKeyList_CORONISTPT_5_F(Numbers);
                        }
                        break;
                    case "RS6500":
                        if (PlayTypeID == PlayType_3_ZhiD)//排3只单
                        {
                            return GetPrintKeyList_RS6500_ZhiD(Numbers);
                        }
                        if (PlayTypeID == PlayType_3_ZhiF)//排3只复
                        {
                            return GetPrintKeyList_RS6500_3_ZhiF(Numbers);
                        }
                        if (PlayTypeID == PlayType_3_ZuD)   //组选单式
                        {
                            return GetPrintKeyList_RS6500_ZhiD(Numbers);
                        }
                        if (PlayTypeID == PlayType_3_Zu3F)//排3组3复式
                        {
                            return GetPrintKeyList_RS6500_ZhiD(Numbers);
                        }
                        if (PlayTypeID == PlayType_3_Zu6F)//排3组6复式
                        {
                            return GetPrintKeyList_RS6500_ZhiD(Numbers);
                        }
                        if ((PlayTypeID == PlayType_3_ZhiH) || (PlayTypeID == PlayType_3_ZuH))//和值
                        {
                            return GetPrintKeyList_RS6500_H(Numbers);
                        }
                        if (PlayTypeID == PlayType_5_D) //排5单式
                        {
                            return GetPrintKeyList_RS6500_ZhiD(Numbers);
                        }
                        if (PlayTypeID == PlayType_5_F)//排5复式
                        {
                            return GetPrintKeyList_RS6500_5_F(Numbers);
                        }
                        break;
                    case "ks230":
                        if (PlayTypeID == PlayType_3_ZhiD)//排3只单
                        {
                            return GetPrintKeyList_ks230_ZhiD(Numbers);
                        }
                        if (PlayTypeID == PlayType_3_ZhiF)//排3只复
                        {
                            return GetPrintKeyList_ks230_3_ZhiF(Numbers);
                        }
                        if (PlayTypeID == PlayType_3_ZuD)   //组选单式
                        {
                            return GetPrintKeyList_ks230_ZhiD(Numbers);
                        }
                        if (PlayTypeID == PlayType_3_Zu3F)//排3组3复式
                        {
                            return GetPrintKeyList_ks230_ZhiD(Numbers);
                        }
                        if (PlayTypeID == PlayType_3_Zu6F)//排3组6复式
                        {
                            return GetPrintKeyList_ks230_ZhiD(Numbers);
                        }
                        if ((PlayTypeID == PlayType_3_ZhiH) || (PlayTypeID == PlayType_3_ZuH))//和值
                        {
                            return GetPrintKeyList_ks230_H(Numbers);
                        }
                        if (PlayTypeID == PlayType_5_D)//排5单式
                        {
                            return GetPrintKeyList_ks230_ZhiD(Numbers);
                        }
                        if (PlayTypeID == PlayType_5_F)//排5复式
                        {
                            return GetPrintKeyList_ks230_5_F(Numbers);
                        }
                        break;
                    case "LA-600A":
                        if (PlayTypeID == PlayType_3_ZhiD)//排3只单
                        {
                            return GetPrintKeyList_LA_600A_ZhiD(Numbers);
                        }
                        if (PlayTypeID == PlayType_3_ZhiF)//排3只复
                        {
                            return GetPrintKeyList_LA_600A_3_ZhiF(Numbers);
                        }
                        if (PlayTypeID == PlayType_3_ZuD)   //组选单式
                        {
                            return GetPrintKeyList_LA_600A_ZhiD(Numbers);
                        }
                        if (PlayTypeID == PlayType_3_Zu3F)//排3组3复式
                        {
                            return GetPrintKeyList_LA_600A_ZhiD(Numbers);
                        }
                        if (PlayTypeID == PlayType_3_Zu6F)//排3组6复式
                        {
                            return GetPrintKeyList_LA_600A_ZhiD(Numbers);
                        }
                        if ((PlayTypeID == PlayType_3_ZhiH) || (PlayTypeID == PlayType_3_ZuH))//和值
                        {
                            return GetPrintKeyList_LA_600A_H(Numbers);
                        }
                        if (PlayTypeID == PlayType_5_D)//排5单式
                        {
                            return GetPrintKeyList_LA_600A_ZhiD(Numbers);
                        }
                        if (PlayTypeID == PlayType_5_F)//排5复式
                        {
                            return GetPrintKeyList_LA_600A_5_F(Numbers);
                        }
                        break;
                }

                return "";
            }
            #region GetPrintKeyList 的具体方法
            private string GetPrintKeyList_CR_YTCII2_ZhiD(string[] Numbers)
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
            private string GetPrintKeyList_CR_YTCII2_3_ZhiF(string[] Numbers)
            {
                string KeyList = "";

                foreach (string Number in Numbers)
                {
                    Regex regex = new Regex(@"^(?<L0>(\d)|([(][\d]+?[)]))(?<L1>(\d)|([(][\d]+?[)]))(?<L2>(\d)|([(][\d]+?[)]))", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                    Match m = regex.Match(Number);

                    for (int i = 0; i < 3; i++)
                    {
                        string Locate = m.Groups["L" + i.ToString()].ToString().Trim();

                        if (Locate.Length == 1)
                        {
                            KeyList += "[" + Locate + "]";
                        }
                        else
                        {
                            Locate = Locate.Substring(1, Locate.Length - 2);

                            foreach (char ch in Locate)
                            {
                                KeyList += "[" + ch.ToString() + "]";
                            }
                        }

                        if (i < 2)
                            KeyList += "[↓]";
                    }
                }

                return KeyList;
            }
            private string GetPrintKeyList_CR_YTCII2_H(string[] Numbers)
            {
                string KeyList = "";

                foreach (string Number in Numbers)
                {
                    string str = Number.Replace(" ", "").Replace("\r", "").Replace("\n", "").PadLeft(2, '0');
                    foreach (char ch in str)
                    {
                        KeyList += "[" + ch.ToString() + "]";
                    }
                }

                return KeyList;
            }
            private string GetPrintKeyList_CR_YTCII2_5_F(string[] Numbers)
            {
                string KeyList = "";

                foreach (string Number in Numbers)
                {
                    Regex regex = new Regex(@"^(?<L0>(\d)|([(][\d]+?[)]))(?<L1>(\d)|([(][\d]+?[)]))(?<L2>(\d)|([(][\d]+?[)]))(?<L3>(\d)|([(][\d]+?[)]))(?<L4>(\d)|([(][\d]+?[)]))", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                    Match m = regex.Match(Number);

                    for (int i = 0; i < 5; i++)
                    {
                        string Locate = m.Groups["L" + i.ToString()].ToString().Trim();

                        if (Locate.Length == 1)
                        {
                            KeyList += "[" + Locate + "]";
                        }
                        else
                        {
                            Locate = Locate.Substring(1, Locate.Length - 2);

                            foreach (char ch in Locate)
                            {
                                KeyList += "[" + ch.ToString() + "]";
                            }
                        }

                        if (i < 4)
                            KeyList += "[↓]";
                    }
                }

                return KeyList;
            }

            private string GetPrintKeyList_TCBJYTD_ZhiD(string[] Numbers)
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
            private string GetPrintKeyList_TCBJYTD_3_ZhiF(string[] Numbers)
            {
                string KeyList = "";

                foreach (string Number in Numbers)
                {
                    Regex regex = new Regex(@"^(?<L0>(\d)|([(][\d]+?[)]))(?<L1>(\d)|([(][\d]+?[)]))(?<L2>(\d)|([(][\d]+?[)]))", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                    Match m = regex.Match(Number);

                    for (int i = 0; i < 3; i++)
                    {
                        string Locate = m.Groups["L" + i.ToString()].ToString().Trim();

                        if (Locate.Length == 1)
                        {
                            KeyList += "[" + Locate + "]";
                        }
                        else
                        {
                            Locate = Locate.Substring(1, Locate.Length - 2);

                            foreach (char ch in Locate)
                            {
                                KeyList += "[" + ch.ToString() + "]";
                            }
                        }

                        if (i < 2)
                            KeyList += "[→]";
                    }
                }

                return KeyList;
            }
            private string GetPrintKeyList_TCBJYTD_H(string[] Numbers)
            {
                string KeyList = "";

                foreach (string Number in Numbers)
                {
                    string str = Number.Replace(" ", "").Replace("\r", "").Replace("\n", "").PadLeft(2, '0');
                    foreach (char ch in str)
                    {
                        KeyList += "[" + ch.ToString() + "]";
                    }
                }

                return KeyList;
            }
            private string GetPrintKeyList_TCBJYTD_5_F(string[] Numbers)
            {
                string KeyList = "";

                foreach (string Number in Numbers)
                {
                    Regex regex = new Regex(@"^(?<L0>(\d)|([(][\d]+?[)]))(?<L1>(\d)|([(][\d]+?[)]))(?<L2>(\d)|([(][\d]+?[)]))(?<L3>(\d)|([(][\d]+?[)]))(?<L4>(\d)|([(][\d]+?[)]))", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                    Match m = regex.Match(Number);

                    for (int i = 0; i < 5; i++)
                    {
                        string Locate = m.Groups["L" + i.ToString()].ToString().Trim();

                        if (Locate.Length == 1)
                        {
                            KeyList += "[" + Locate + "]";
                        }
                        else
                        {
                            Locate = Locate.Substring(1, Locate.Length - 2);

                            foreach (char ch in Locate)
                            {
                                KeyList += "[" + ch.ToString() + "]";
                            }
                        }

                        if (i < 4)
                            KeyList += "[→]";
                    }
                }

                return KeyList;
            }

            private string GetPrintKeyList_TGAMPOS4000_ZhiD(string[] Numbers)
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
            private string GetPrintKeyList_TGAMPOS4000_3_ZhiF(string[] Numbers)
            {
                string KeyList = "";

                foreach (string Number in Numbers)
                {
                    Regex regex = new Regex(@"^(?<L0>(\d)|([(][\d]+?[)]))(?<L1>(\d)|([(][\d]+?[)]))(?<L2>(\d)|([(][\d]+?[)]))", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                    Match m = regex.Match(Number);

                    for (int i = 0; i < 3; i++)
                    {
                        string Locate = m.Groups["L" + i.ToString()].ToString().Trim();

                        if (Locate.Length == 1)
                        {
                            KeyList += "[" + Locate + "]";
                        }
                        else
                        {
                            Locate = Locate.Substring(1, Locate.Length - 2);

                            foreach (char ch in Locate)
                            {
                                KeyList += "[" + ch.ToString() + "]";
                            }
                        }

                        if (i < 2)
                            KeyList += "[↓]";
                    }
                }

                return KeyList;
            }
            private string GetPrintKeyList_TGAMPOS4000_H(string[] Numbers)
            {
                string KeyList = "";

                foreach (string Number in Numbers)
                {
                    string str = Number.Replace(" ", "").Replace("\r", "").Replace("\n", "").PadLeft(2, '0');
                    foreach (char ch in str)
                    {
                        KeyList += "[" + ch.ToString() + "]";
                    }
                }

                return KeyList;
            }
            private string GetPrintKeyList_TGAMPOS4000_5_F(string[] Numbers)
            {
                string KeyList = "";

                foreach (string Number in Numbers)
                {
                    Regex regex = new Regex(@"^(?<L0>(\d)|([(][\d]+?[)]))(?<L1>(\d)|([(][\d]+?[)]))(?<L2>(\d)|([(][\d]+?[)]))(?<L3>(\d)|([(][\d]+?[)]))(?<L4>(\d)|([(][\d]+?[)]))", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                    Match m = regex.Match(Number);

                    for (int i = 0; i < 5; i++)
                    {
                        string Locate = m.Groups["L" + i.ToString()].ToString().Trim();

                        if (Locate.Length == 1)
                        {
                            KeyList += "[" + Locate + "]";
                        }
                        else
                        {
                            Locate = Locate.Substring(1, Locate.Length - 2);

                            foreach (char ch in Locate)
                            {
                                KeyList += "[" + ch.ToString() + "]";
                            }
                        }

                        if (i < 4)
                            KeyList += "[↓]";
                    }
                }

                return KeyList;
            }

            private string GetPrintKeyList_CP86_ZhiD(string[] Numbers)
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
            private string GetPrintKeyList_CP86_ZhiF(string[] Numbers)
            {
                string KeyList = "";

                foreach (string Number in Numbers)
                {
                    Regex regex = new Regex(@"^(?<L0>(\d)|([(][\d]+?[)]))(?<L1>(\d)|([(][\d]+?[)]))(?<L2>(\d)|([(][\d]+?[)]))", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                    Match m = regex.Match(Number);

                    for (int i = 0; i < 3; i++)
                    {
                        string Locate = m.Groups["L" + i.ToString()].ToString().Trim();

                        if (Locate.Length == 1)
                        {
                            KeyList += "[" + Locate + "]";
                        }
                        else
                        {
                            Locate = Locate.Substring(1, Locate.Length - 2);

                            foreach (char ch in Locate)
                            {
                                KeyList += "[" + ch.ToString() + "]";
                            }
                        }

                        if (i < 2)
                            KeyList += "[↓]";
                    }
                }

                return KeyList;
            }
            private string GetPrintKeyList_CP86_H(string[] Numbers)
            {
                string KeyList = "";

                foreach (string Number in Numbers)
                {
                    string str = Number.Replace(" ", "").Replace("\r", "").Replace("\n", "").PadLeft(2, '0');
                    foreach (char ch in str)
                    {
                        KeyList += "[" + ch.ToString() + "]";
                    }
                }

                return KeyList;
            }
            private string GetPrintKeyList_CP86_5_F(string[] Numbers)
            {
                string KeyList = "";

                foreach (string Number in Numbers)
                {
                    Regex regex = new Regex(@"^(?<L0>(\d)|([(][\d]+?[)]))(?<L1>(\d)|([(][\d]+?[)]))(?<L2>(\d)|([(][\d]+?[)]))(?<L3>(\d)|([(][\d]+?[)]))(?<L4>(\d)|([(][\d]+?[)]))", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                    Match m = regex.Match(Number);

                    for (int i = 0; i < 5; i++)
                    {
                        string Locate = m.Groups["L" + i.ToString()].ToString().Trim();

                        if (Locate.Length == 1)
                        {
                            KeyList += "[" + Locate + "]";
                        }
                        else
                        {
                            Locate = Locate.Substring(1, Locate.Length - 2);

                            foreach (char ch in Locate)
                            {
                                KeyList += "[" + ch.ToString() + "]";
                            }
                        }

                        if (i < 4)
                            KeyList += "[↓]";
                    }
                }

                return KeyList;
            }

            private string GetPrintKeyList_MODEL_4000_ZhiD(string[] Numbers)
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
            private string GetPrintKeyList_MODEL_4000_3_ZhiF(string[] Numbers)
            {
                string KeyList = "";

                foreach (string Number in Numbers)
                {
                    Regex regex = new Regex(@"^(?<L0>(\d)|([(][\d]+?[)]))(?<L1>(\d)|([(][\d]+?[)]))(?<L2>(\d)|([(][\d]+?[)]))", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                    Match m = regex.Match(Number);

                    for (int i = 0; i < 3; i++)
                    {
                        string Locate = m.Groups["L" + i.ToString()].ToString().Trim();

                        if (Locate.Length == 1)
                        {
                            KeyList += "[" + Locate + "]";
                        }
                        else
                        {
                            Locate = Locate.Substring(1, Locate.Length - 2);

                            foreach (char ch in Locate)
                            {
                                KeyList += "[" + ch.ToString() + "]";
                            }
                        }

                        if (i < 2)
                            KeyList += "[↓]";
                    }
                }

                return KeyList;
            }
            private string GetPrintKeyList_MODEL_4000_H(string[] Numbers)
            {
                string KeyList = "";

                foreach (string Number in Numbers)
                {
                    string str = Number.Replace(" ", "").Replace("\r", "").Replace("\n", "").PadLeft(2, '0');
                    foreach (char ch in str)
                    {
                        KeyList += "[" + ch.ToString() + "]";
                    }
                }

                return KeyList;
            }
            private string GetPrintKeyList_MODEL_4000_5_F(string[] Numbers)
            {
                string KeyList = "";

                foreach (string Number in Numbers)
                {
                    Regex regex = new Regex(@"^(?<L0>(\d)|([(][\d]+?[)]))(?<L1>(\d)|([(][\d]+?[)]))(?<L2>(\d)|([(][\d]+?[)]))(?<L3>(\d)|([(][\d]+?[)]))(?<L4>(\d)|([(][\d]+?[)]))", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                    Match m = regex.Match(Number);

                    for (int i = 0; i < 5; i++)
                    {
                        string Locate = m.Groups["L" + i.ToString()].ToString().Trim();

                        if (Locate.Length == 1)
                        {
                            KeyList += "[" + Locate + "]";
                        }
                        else
                        {
                            Locate = Locate.Substring(1, Locate.Length - 2);

                            foreach (char ch in Locate)
                            {
                                KeyList += "[" + ch.ToString() + "]";
                            }
                        }

                        if (i < 4)
                            KeyList += "[↓]";
                    }
                }

                return KeyList;
            }

            private string GetPrintKeyList_CORONISTPT_ZhiD(string[] Numbers)
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
            private string GetPrintKeyList_CORONISTPT_3_ZhiF(string[] Numbers)
            {
                string KeyList = "";

                foreach (string Number in Numbers)
                {
                    Regex regex = new Regex(@"^(?<L0>(\d)|([(][\d]+?[)]))(?<L1>(\d)|([(][\d]+?[)]))(?<L2>(\d)|([(][\d]+?[)]))", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                    Match m = regex.Match(Number);

                    for (int i = 0; i < 3; i++)
                    {
                        string Locate = m.Groups["L" + i.ToString()].ToString().Trim();

                        if (Locate.Length == 1)
                        {
                            KeyList += "[" + Locate + "]";
                        }
                        else
                        {
                            Locate = Locate.Substring(1, Locate.Length - 2);

                            foreach (char ch in Locate)
                            {
                                KeyList += "[" + ch.ToString() + "]";
                            }
                        }

                        if (i < 2)
                            KeyList += "[↓]";
                    }
                }

                return KeyList;
            }
            private string GetPrintKeyList_CORONISTPT_H(string[] Numbers)
            {
                string KeyList = "";

                foreach (string Number in Numbers)
                {
                    string str = Number.Replace(" ", "").Replace("\r", "").Replace("\n", "").PadLeft(2, '0');
                    foreach (char ch in str)
                    {
                        KeyList += "[" + ch.ToString() + "]";
                    }
                }

                return KeyList;
            }
            private string GetPrintKeyList_CORONISTPT_5_F(string[] Numbers)
            {
                string KeyList = "";

                foreach (string Number in Numbers)
                {
                    Regex regex = new Regex(@"^(?<L0>(\d)|([(][\d]+?[)]))(?<L1>(\d)|([(][\d]+?[)]))(?<L2>(\d)|([(][\d]+?[)]))(?<L3>(\d)|([(][\d]+?[)]))(?<L4>(\d)|([(][\d]+?[)]))", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                    Match m = regex.Match(Number);

                    for (int i = 0; i < 5; i++)
                    {
                        string Locate = m.Groups["L" + i.ToString()].ToString().Trim();

                        if (Locate.Length == 1)
                        {
                            KeyList += "[" + Locate + "]";
                        }
                        else
                        {
                            Locate = Locate.Substring(1, Locate.Length - 2);

                            foreach (char ch in Locate)
                            {
                                KeyList += "[" + ch.ToString() + "]";
                            }
                        }

                        if (i < 4)
                            KeyList += "[↓]";
                    }
                }

                return KeyList;
            }

            private string GetPrintKeyList_RS6500_ZhiD(string[] Numbers)
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
            private string GetPrintKeyList_RS6500_3_ZhiF(string[] Numbers)
            {
                string KeyList = "";

                foreach (string Number in Numbers)
                {
                    Regex regex = new Regex(@"^(?<L0>(\d)|([(][\d]+?[)]))(?<L1>(\d)|([(][\d]+?[)]))(?<L2>(\d)|([(][\d]+?[)]))", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                    Match m = regex.Match(Number);

                    for (int i = 0; i < 3; i++)
                    {
                        string Locate = m.Groups["L" + i.ToString()].ToString().Trim();

                        if (Locate.Length == 1)
                        {
                            KeyList += "[" + Locate + "]";
                        }
                        else
                        {
                            Locate = Locate.Substring(1, Locate.Length - 2);

                            foreach (char ch in Locate)
                            {
                                KeyList += "[" + ch.ToString() + "]";
                            }
                        }

                        if (i < 2)
                            KeyList += "[↓]";
                    }
                }

                return KeyList;
            }
            private string GetPrintKeyList_RS6500_H(string[] Numbers)
            {
                string KeyList = "";

                foreach (string Number in Numbers)
                {
                    string str = Number.Replace(" ", "").Replace("\r", "").Replace("\n", "").PadLeft(2, '0');
                    foreach (char ch in str)
                    {
                        KeyList += "[" + ch.ToString() + "]";
                    }
                }

                return KeyList;
            }
            private string GetPrintKeyList_RS6500_5_F(string[] Numbers)
            {
                string KeyList = "";

                foreach (string Number in Numbers)
                {
                    Regex regex = new Regex(@"^(?<L0>(\d)|([(][\d]+?[)]))(?<L1>(\d)|([(][\d]+?[)]))(?<L2>(\d)|([(][\d]+?[)]))(?<L3>(\d)|([(][\d]+?[)]))(?<L4>(\d)|([(][\d]+?[)]))", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                    Match m = regex.Match(Number);

                    for (int i = 0; i < 5; i++)
                    {
                        string Locate = m.Groups["L" + i.ToString()].ToString().Trim();

                        if (Locate.Length == 1)
                        {
                            KeyList += "[" + Locate + "]";
                        }
                        else
                        {
                            Locate = Locate.Substring(1, Locate.Length - 2);

                            foreach (char ch in Locate)
                            {
                                KeyList += "[" + ch.ToString() + "]";
                            }
                        }

                        if (i < 4)
                            KeyList += "[↓]";
                    }
                }

                return KeyList;
            }

            private string GetPrintKeyList_ks230_ZhiD(string[] Numbers)
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
            private string GetPrintKeyList_ks230_3_ZhiF(string[] Numbers)
            {
                string KeyList = "";

                foreach (string Number in Numbers)
                {
                    Regex regex = new Regex(@"^(?<L0>(\d)|([(][\d]+?[)]))(?<L1>(\d)|([(][\d]+?[)]))(?<L2>(\d)|([(][\d]+?[)]))", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                    Match m = regex.Match(Number);

                    for (int i = 0; i < 3; i++)
                    {
                        string Locate = m.Groups["L" + i.ToString()].ToString().Trim();

                        if (Locate.Length == 1)
                        {
                            KeyList += "[" + Locate + "]";
                        }
                        else
                        {
                            Locate = Locate.Substring(1, Locate.Length - 2);

                            foreach (char ch in Locate)
                            {
                                KeyList += "[" + ch.ToString() + "]";
                            }
                        }

                        if (i < 2)
                            KeyList += "[↓]";
                    }
                }

                return KeyList;
            }
            private string GetPrintKeyList_ks230_H(string[] Numbers)
            {
                string KeyList = "";

                foreach (string Number in Numbers)
                {
                    string str = Number.Replace(" ", "").Replace("\r", "").Replace("\n", "").PadLeft(2, '0');
                    foreach (char ch in str)
                    {
                        KeyList += "[" + ch.ToString() + "]";
                    }
                }

                return KeyList;
            }
            private string GetPrintKeyList_ks230_5_F(string[] Numbers)
            {
                string KeyList = "";

                foreach (string Number in Numbers)
                {
                    Regex regex = new Regex(@"^(?<L0>(\d)|([(][\d]+?[)]))(?<L1>(\d)|([(][\d]+?[)]))(?<L2>(\d)|([(][\d]+?[)]))(?<L3>(\d)|([(][\d]+?[)]))(?<L4>(\d)|([(][\d]+?[)]))", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                    Match m = regex.Match(Number);

                    for (int i = 0; i < 5; i++)
                    {
                        string Locate = m.Groups["L" + i.ToString()].ToString().Trim();

                        if (Locate.Length == 1)
                        {
                            KeyList += "[" + Locate + "]";
                        }
                        else
                        {
                            Locate = Locate.Substring(1, Locate.Length - 2);

                            foreach (char ch in Locate)
                            {
                                KeyList += "[" + ch.ToString() + "]";
                            }
                        }

                        if (i < 4)
                            KeyList += "[↓]";
                    }
                }

                return KeyList;
            }

            private string GetPrintKeyList_LA_600A_ZhiD(string[] Numbers)
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
            private string GetPrintKeyList_LA_600A_3_ZhiF(string[] Numbers)
            {
                string KeyList = "";

                foreach (string Number in Numbers)
                {
                    Regex regex = new Regex(@"^(?<L0>(\d)|([(][\d]+?[)]))(?<L1>(\d)|([(][\d]+?[)]))(?<L2>(\d)|([(][\d]+?[)]))", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                    Match m = regex.Match(Number);

                    for (int i = 0; i < 3; i++)
                    {
                        string Locate = m.Groups["L" + i.ToString()].ToString().Trim();

                        if (Locate.Length == 1)
                        {
                            KeyList += "[" + Locate + "]";
                        }
                        else
                        {
                            Locate = Locate.Substring(1, Locate.Length - 2);

                            foreach (char ch in Locate)
                            {
                                KeyList += "[" + ch.ToString() + "]";
                            }
                        }

                        if (i < 2)
                            KeyList += "[↓]";
                    }
                }

                return KeyList;
            }
            private string GetPrintKeyList_LA_600A_H(string[] Numbers)
            {
                string KeyList = "";

                foreach (string Number in Numbers)
                {
                    string str = Number.Replace(" ", "").Replace("\r", "").Replace("\n", "").PadLeft(2, '0');
                    foreach (char ch in str)
                    {
                        KeyList += "[" + ch.ToString() + "]";
                    }
                }

                return KeyList;
            }
            private string GetPrintKeyList_LA_600A_5_F(string[] Numbers)
            {
                string KeyList = "";

                foreach (string Number in Numbers)
                {
                    Regex regex = new Regex(@"^(?<L0>(\d)|([(][\d]+?[)]))(?<L1>(\d)|([(][\d]+?[)]))(?<L2>(\d)|([(][\d]+?[)]))(?<L3>(\d)|([(][\d]+?[)]))(?<L4>(\d)|([(][\d]+?[)]))", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                    Match m = regex.Match(Number);

                    for (int i = 0; i < 5; i++)
                    {
                        string Locate = m.Groups["L" + i.ToString()].ToString().Trim();

                        if (Locate.Length == 1)
                        {
                            KeyList += "[" + Locate + "]";
                        }
                        else
                        {
                            Locate = Locate.Substring(1, Locate.Length - 2);

                            foreach (char ch in Locate)
                            {
                                KeyList += "[" + ch.ToString() + "]";
                            }
                        }

                        if (i < 4)
                            KeyList += "[↓]";
                    }
                }

                return KeyList;
            }
            #endregion
        }
    }
}
