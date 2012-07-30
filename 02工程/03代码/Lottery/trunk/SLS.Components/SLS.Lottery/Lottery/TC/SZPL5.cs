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
        /// 体彩排列5
        /// </summary>
        public partial class SZPL5 : LotteryBase
        {
            #region 静态变量
            public const int PlayType_5_D = 6401;
            public const int PlayType_5_F = 6402;

            public const int ID = 64;
            public const string sID = "64";
            public const string Name = "体彩排列5";
            public const string Code = "SZPL5";
            public const double MaxMoney = 20000;
            #endregion

            public SZPL5()
            {
                id = 64;
                name = "体彩排列5";
                code = "SZPL5";
            }

            public override bool CheckPlayType(int play_type)
            {
                return ((play_type >= 6401) && (play_type <= 6402));
            }

            public override PlayType[] GetPlayTypeList()
            {
                PlayType[] Result = new PlayType[2];

                Result[0] = new PlayType(PlayType_5_D, "排列5单式");
                Result[1] = new PlayType(PlayType_5_F, "排列5复式");

                return Result;
            }

            public override string BuildNumber(int Num)	//id = 64,num = 注数
            {
                System.Random rd = new Random();
                StringBuilder sb = new StringBuilder();

                for (int i = 0; i < Num; i++)
                {
                    string LotteryNumber = "";
                    for (int j = 0; j < 5; j++)
                        LotteryNumber += rd.Next(0, 9 + 1).ToString();

                    sb.Append(LotteryNumber.Trim() + "\n");
                }

                string Result = sb.ToString();
                Result = Result.Substring(0, Result.Length - 1);
                return Result;
            }

            public override string[] ToSingle(string Number, ref string CanonicalNumber, int PlayType)
            {
                if ((PlayType == PlayType_5_D) || (PlayType == PlayType_5_F))
                    return ToSingle_5(Number, ref CanonicalNumber);

                return null;
            }
            #region ToSingle 的具体方法
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
            #endregion

            public override double ComputeWin(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, int PlayType, params double[] WinMoneyList)
            {
                if ((PlayType == PlayType_5_D) || (PlayType == PlayType_5_F))
                    return ComputeWin_5(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[0], WinMoneyList[1]);

                return -4;
            }
            #region ComputeWin 的具体方法
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

                string[] Locate = new string[5];

                int RedRright = 0;

                Regex regex = new Regex(@"^(?<L0>([\d])|([(][\d]{1,10}[)]))(?<L1>([\d])|([(][\d]{1,10}[)]))(?<L2>([\d])|([(][\d]{1,10}[)]))(?<L3>([\d])|([(][\d]{1,10}[)]))(?<L4>([\d])|([(][\d]{1,10}[)]))", RegexOptions.IgnoreCase | RegexOptions.Compiled);

                for (int ii = 0; ii < Lotterys.Length; ii++)
                {
                    RedRright = 0;

                    Match m = regex.Match(Lotterys[ii]);

                    for (int j = 0; j < 5; j++)
                    {
                        Locate[j] = m.Groups["L" + j.ToString()].ToString().Trim();

                        if (Locate[j] == "")
                        {
                            continue;
                        }

                        if (Locate[j].IndexOf(WinNumber.Substring(j, 1)) >= 0)
                        {
                            RedRright++;
                        }
                    }

                    if (RedRright < 5)
                    {
                        continue;
                    }

                    Description1++;
                    WinMoney += WinMoney1;
                    WinMoneyNoWithTax += WinMoneyNoWithTax1;
                }

                if (Description1 > 0)
                    Description = "排列5直选奖" + Description1.ToString() + "注。";

                return WinMoney;
            }
            #endregion

            public override string AnalyseScheme(string Content, int PlayType)
            {
                if ((PlayType == PlayType_5_D) || (PlayType == PlayType_5_F))
                    return AnalyseScheme_5(Content, PlayType);

                return "";
            }

            #region AnalyseScheme 的具体方法
            private string AnalyseScheme_5(string Content, int PlayType)
            {
                if (string.IsNullOrEmpty(Content))
                {
                    return "";
                }

                string[] strs = Content.Split('\n');
                if (strs == null)
                    return "";
                if (strs.Length == 0)
                    return "";

                string Result = "";
                int Num = 1;
                string CanonicalNumber = "";
                string RegexString = "";

                string[] Locate = new string[5];

                if (PlayType == PlayType_5_D)
                {
                    RegexString = @"^([\d]){5}";
                }
                else
                {
                    RegexString = @"^(?<L0>([\d])|([(][\d]{1,10}[)]))(?<L1>([\d])|([(][\d]{1,10}[)]))(?<L2>([\d])|([(][\d]{1,10}[)]))(?<L3>([\d])|([(][\d]{1,10}[)]))(?<L4>([\d])|([(][\d]{1,10}[)]))";
                }

                Regex regex = new Regex(RegexString, RegexOptions.IgnoreCase | RegexOptions.Compiled);

                for (int i = 0; i < strs.Length; i++)
                {
                    Match m = regex.Match(strs[i]);

                    if (!m.Success)
                    {
                        continue;
                    }

                    if (PlayType == PlayType_5_D)
                    {
                        string[] singles = ToSingle(m.Value, ref CanonicalNumber, PlayType);
                        if (singles == null)
                            continue;
                        Result += CanonicalNumber + "|" + singles.Length.ToString() + "\n";

                        continue;
                    }

                    CanonicalNumber = "";
                    Num = 1;

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
                        if (PlayTypeID == PlayType_5_D)//排5单式
                        {
                            return GetPrintKeyList_LA_600A_ZhiD(Numbers);
                        }
                        if (PlayTypeID == PlayType_5_F)//排5复式
                        {
                            return GetPrintKeyList_LA_600A_5_F(Numbers);
                        }
                        break;
                    case "TSP700_II":
                        if (PlayTypeID == PlayType_5_D)//排5单式
                        {
                            return GetPrintKeyList_TSP700_II_ZhiD(Numbers);
                        }
                        if (PlayTypeID == PlayType_5_F)//排5复式
                        {
                            return GetPrintKeyList_TSP700_II_5_F(Numbers);
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

            private string GetPrintKeyList_TSP700_II_ZhiD(string[] Numbers)
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
            private string GetPrintKeyList_TSP700_II_5_F(string[] Numbers)
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

            //参数说明 PlayTypeID:玩法类别; Number:投注号码; Multiple: 倍数; MaxMultiple:最大倍数;
            public override Ticket[] ToElectronicTicket_HPSD(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                if (PlayTypeID == PlayType_5_D)
                {
                    return ToElectronicTicket_HPSD_D(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                if (PlayTypeID == PlayType_5_F)
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
                        if (strs[i].ToString().Split('|').Length < 2)
                        {
                            continue;
                        }

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

                if (PlayTypeID == PlayType_5_D)
                {
                    string[] strs = Number.Split('\n');

                    for (int i = 0; i < strs.Length; i++)
                    {
                        for (int j = 0; j < strs[i].Length; j++)
                        {
                            Ticket += strs[i].Substring(j, 1) + ",";
                        }

                        if (Ticket.EndsWith(","))
                        {
                            Ticket = Ticket.Substring(0, Ticket.Length - 1);
                        }

                        Ticket = Ticket + "\n";
                    }

                    if (Ticket.EndsWith("\n"))
                    {
                        Ticket = Ticket.Substring(0, Ticket.Length - 1);
                    }
                }

                if (PlayTypeID == PlayType_5_F)
                {
                    string[] strs = Number.Split('\n');

                    for (int i = 0; i < strs.Length; i++)
                    {
                        string[] Locate = new string[5];

                        Regex regex = new Regex(@"^(?<L0>(\d)|([(][\d]+?[)]))(?<L1>(\d)|([(][\d]+?[)]))(?<L2>(\d)|([(][\d]+?[)]))(?<L3>(\d)|([(][\d]+?[)]))(?<L4>(\d)|([(][\d]+?[)]))", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                        Match m = regex.Match(strs[i]);

                        for (int j = 0; j < 5; j++)
                        {
                            Locate[j] = m.Groups["L" + j.ToString()].ToString().Trim();

                            if (Locate[j] == "")
                            {
                                return "";
                            }

                            if (Locate[j].Length > 1)
                            {
                                Locate[j] = Locate[j].Substring(1, Locate[j].Length - 2);
                                if (Locate[j].Length > 1)
                                    Locate[j] = FilterRepeated(Locate[j]);

                                if (Locate[j] == "")
                                {
                                    return "";
                                }
                            }

                            Ticket += Locate[j] + ",";
                        }

                        if (Ticket.EndsWith(","))
                        {
                            Ticket = Ticket.Substring(0, Ticket.Length - 1);
                        }

                        Ticket = Ticket + "\n";
                    }
                }

                if (Ticket.EndsWith("\n"))
                {
                    Ticket = Ticket.Substring(0, Ticket.Length - 1);
                }

                return Ticket;
            }
            #endregion

            public override Ticket[] ToElectronicTicket_ZCW(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                if (PlayTypeID == PlayType_5_D)
                {
                    return ToElectronicTicket_ZCW_D(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                if (PlayTypeID == PlayType_5_F)
                {
                    return ToElectronicTicket_ZCW_F(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
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
                        if (strs[i].ToString().Split('|').Length < 2)
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

            private string ConvertFormatToElectronTicket_ZCW(int PlayTypeID, string Number)
            {
                Number = Number.Trim();

                string Ticket = "";

                if (PlayTypeID == PlayType_5_D)
                {
                    string[] strs = Number.Split('\n');

                    for (int i = 0; i < strs.Length; i++)
                    {
                        for (int j = 0; j < strs[i].Length; j++)
                        {
                            Ticket += strs[i].Substring(j, 1) + "*";
                        }

                        if (Ticket.EndsWith("*"))
                        {
                            Ticket = Ticket.Substring(0, Ticket.Length - 1);
                        }

                        Ticket = Ticket + "\n";
                    }

                    if (Ticket.EndsWith("\n"))
                    {
                        Ticket = Ticket.Substring(0, Ticket.Length - 1);
                    }
                }

                if (PlayTypeID == PlayType_5_F)
                {
                    string[] strs = Number.Split('\n');

                    for (int i = 0; i < strs.Length; i++)
                    {
                        string[] Locate = new string[5];

                        Regex regex = new Regex(@"^(?<L0>(\d)|([(][\d]+?[)]))(?<L1>(\d)|([(][\d]+?[)]))(?<L2>(\d)|([(][\d]+?[)]))(?<L3>(\d)|([(][\d]+?[)]))(?<L4>(\d)|([(][\d]+?[)]))", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                        Match m = regex.Match(strs[i]);

                        for (int j = 0; j < 5; j++)
                        {
                            Locate[j] = m.Groups["L" + j.ToString()].ToString().Trim();

                            if (Locate[j] == "")
                            {
                                return "";
                            }

                            if (Locate[j].Length > 1)
                            {
                                Locate[j] = Locate[j].Substring(1, Locate[j].Length - 2);
                                if (Locate[j].Length > 1)
                                    Locate[j] = FilterRepeated(Locate[j]);

                                if (Locate[j] == "")
                                {
                                    return "";
                                }
                            }

                            Ticket += Locate[j] + "*";
                        }

                        if (Ticket.EndsWith("*"))
                        {
                            Ticket = Ticket.Substring(0, Ticket.Length - 1);
                        }

                        Ticket = Ticket + "\n";
                    }
                }

                if (Ticket.EndsWith("\n"))
                {
                    Ticket = Ticket.Substring(0, Ticket.Length - 1);
                }

                return Ticket;
            }
            #endregion

            public override Ticket[] ToElectronicTicket_ZZYTC(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                if (PlayTypeID == PlayType_5_D)
                {
                    return ToElectronicTicket_ZZYTC_D(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                if (PlayTypeID == PlayType_5_F)
                {
                    return ToElectronicTicket_ZZYTC_F(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
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
                        if (strs[i].ToString().Split('|').Length < 2)
                        {
                            continue;
                        }

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

            private string ConvertFormatToElectronTicket_ZZYTC(int PlayTypeID, string Number)
            {
                Number = Number.Trim();

                string Ticket = "";

                if (PlayTypeID == PlayType_5_D)
                {
                    string[] strs = Number.Split('\n');

                    for (int i = 0; i < strs.Length; i++)
                    {
                        for (int j = 0; j < strs[i].Length; j++)
                        {
                            Ticket += strs[i].Substring(j, 1) + "*";
                        }

                        if (Ticket.EndsWith("*"))
                        {
                            Ticket = Ticket.Substring(0, Ticket.Length - 1);
                        }

                        Ticket = Ticket + "\n";
                    }

                    if (Ticket.EndsWith("\n"))
                    {
                        Ticket = Ticket.Substring(0, Ticket.Length - 1);
                    }
                }

                if (PlayTypeID == PlayType_5_F)
                {
                    string[] strs = Number.Split('\n');

                    for (int i = 0; i < strs.Length; i++)
                    {
                        string[] Locate = new string[5];

                        Regex regex = new Regex(@"^(?<L0>(\d)|([(][\d]+?[)]))(?<L1>(\d)|([(][\d]+?[)]))(?<L2>(\d)|([(][\d]+?[)]))(?<L3>(\d)|([(][\d]+?[)]))(?<L4>(\d)|([(][\d]+?[)]))", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                        Match m = regex.Match(strs[i]);

                        for (int j = 0; j < 5; j++)
                        {
                            Locate[j] = m.Groups["L" + j.ToString()].ToString().Trim();

                            if (Locate[j] == "")
                            {
                                return "";
                            }

                            if (Locate[j].Length > 1)
                            {
                                Locate[j] = Locate[j].Substring(1, Locate[j].Length - 2);
                                if (Locate[j].Length > 1)
                                    Locate[j] = FilterRepeated(Locate[j]);

                                if (Locate[j] == "")
                                {
                                    return "";
                                }
                            }

                            Ticket += Locate[j] + "*";
                        }

                        if (Ticket.EndsWith("*"))
                        {
                            Ticket = Ticket.Substring(0, Ticket.Length - 1);
                        }

                        Ticket = Ticket + "\n";
                    }
                }

                if (Ticket.EndsWith("\n"))
                {
                    Ticket = Ticket.Substring(0, Ticket.Length - 1);
                }

                return Ticket;
            }
            #endregion

            public override Ticket[] ToElectronicTicket_DYJ(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                if (PlayTypeID == PlayType_5_D)
                {
                    return ToElectronicTicket_DYJ_D(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                if (PlayTypeID == PlayType_5_F)
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

                                Numbers += strs[i + M].ToString().Split('|')[0] + "\n";
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
                        if (strs[i].ToString().Split('|').Length < 2)
                        {
                            continue;
                        }

                        string Numbers = "";
                        EachMoney = 0;

                        Numbers = strs[i].ToString().Split('|')[0];
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

                if (PlayTypeID == PlayType_5_D)
                {
                    string[] strs = Number.Split('\n');

                    for (int i = 0; i < strs.Length; i++)
                    {
                        for (int j = 0; j < strs[i].Length; j++)
                        {
                            Ticket += strs[i].Substring(j, 1) + ",";
                        }

                        if (Ticket.EndsWith(","))
                        {
                            Ticket = Ticket.Substring(0, Ticket.Length - 1);
                        }

                        Ticket = Ticket + "\n";
                    }

                    if (Ticket.EndsWith("\n"))
                    {
                        Ticket = Ticket.Substring(0, Ticket.Length - 1);
                    }

                    Ticket = Ticket + ";";
                }

                if (PlayTypeID == PlayType_5_F)
                {
                    string[] strs = Number.Split('\n');

                    for (int i = 0; i < strs.Length; i++)
                    {
                        string[] Locate = new string[5];

                        Regex regex = new Regex(@"^(?<L0>(\d)|([(][\d]+?[)]))(?<L1>(\d)|([(][\d]+?[)]))(?<L2>(\d)|([(][\d]+?[)]))(?<L3>(\d)|([(][\d]+?[)]))(?<L4>(\d)|([(][\d]+?[)]))", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                        Match m = regex.Match(strs[i]);

                        for (int j = 0; j < 5; j++)
                        {
                            Locate[j] = m.Groups["L" + j.ToString()].ToString().Trim();

                            if (Locate[j] == "")
                            {
                                return "";
                            }

                            if (Locate[j].Length > 1)
                            {
                                Locate[j] = Locate[j].Substring(1, Locate[j].Length - 2);
                                if (Locate[j].Length > 1)
                                    Locate[j] = FilterRepeated(Locate[j]);

                                if (Locate[j] == "")
                                {
                                    return "";
                                }
                            }

                            Ticket += Locate[j] + ",";
                        }

                        if (Ticket.EndsWith(","))
                        {
                            Ticket = Ticket.Substring(0, Ticket.Length - 1);
                        }

                        Ticket = Ticket + ";\n";
                    }
                }

                if (Ticket.EndsWith("\n"))
                {
                    Ticket = Ticket.Substring(0, Ticket.Length - 1);
                }

                return Ticket;
            }
            #endregion

            public override Ticket[] ToElectronicTicket_BJCP(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                if (PlayTypeID == PlayType_5_D)
                {
                    return ToElectronicTicket_BJCP_D(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                if (PlayTypeID == PlayType_5_F)
                {
                    return ToElectronicTicket_BJCP_F(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
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
                        if (strs[i].ToString().Split('|').Length < 2)
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

            private string ConvertFormatToElectronTicket_BJCP(int PlayTypeID, string Number)
            {
                Number = Number.Trim();

                string Ticket = "";

                if (PlayTypeID == PlayType_5_D)
                {
                    string[] strs = Number.Split('\n');

                    for (int i = 0; i < strs.Length; i++)
                    {
                        for (int j = 0; j < strs[i].Length; j++)
                        {
                            Ticket += strs[i].Substring(j, 1) + "*";
                        }

                        if (Ticket.EndsWith("*"))
                        {
                            Ticket = Ticket.Substring(0, Ticket.Length - 1);
                        }

                        Ticket = Ticket + "\n";
                    }

                    if (Ticket.EndsWith("\n"))
                    {
                        Ticket = Ticket.Substring(0, Ticket.Length - 1);
                    }
                }

                if (PlayTypeID == PlayType_5_F)
                {
                    string[] strs = Number.Split('\n');

                    for (int i = 0; i < strs.Length; i++)
                    {
                        string[] Locate = new string[5];

                        Regex regex = new Regex(@"^(?<L0>(\d)|([(][\d]+?[)]))(?<L1>(\d)|([(][\d]+?[)]))(?<L2>(\d)|([(][\d]+?[)]))(?<L3>(\d)|([(][\d]+?[)]))(?<L4>(\d)|([(][\d]+?[)]))", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                        Match m = regex.Match(strs[i]);

                        for (int j = 0; j < 5; j++)
                        {
                            Locate[j] = m.Groups["L" + j.ToString()].ToString().Trim();

                            if (Locate[j] == "")
                            {
                                return "";
                            }

                            if (Locate[j].Length > 1)
                            {
                                Locate[j] = Locate[j].Substring(1, Locate[j].Length - 2);
                                if (Locate[j].Length > 1)
                                    Locate[j] = FilterRepeated(Locate[j]);

                                if (Locate[j] == "")
                                {
                                    return "";
                                }
                            }

                            Ticket += Locate[j] + "*";
                        }

                        if (Ticket.EndsWith("*"))
                        {
                            Ticket = Ticket.Substring(0, Ticket.Length - 1);
                        }

                        Ticket = Ticket + "\n";
                    }
                }

                if (Ticket.EndsWith("\n"))
                {
                    Ticket = Ticket.Substring(0, Ticket.Length - 1);
                }

                return Ticket;
            }
            #endregion

            public override Ticket[] ToElectronicTicket_CTTCSD(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                if (PlayTypeID == PlayType_5_D)
                {
                    return ToElectronicTicket_CTTCSD_D(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                if (PlayTypeID == PlayType_5_F)
                {
                    return ToElectronicTicket_CTTCSD_F(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
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
                        if (strs[i].ToString().Split('|').Length < 2)
                        {
                            continue;
                        }

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

            private string ConvertFormatToElectronTicket_CTTCSD(int PlayTypeID, string Number)
            {
                Number = Number.Trim();

                string Ticket = "";

                if (PlayTypeID == PlayType_5_D)
                {
                    string[] strs = Number.Split('\n');

                    for (int i = 0; i < strs.Length; i++)
                    {
                        for (int j = 0; j < strs[i].Length; j++)
                        {
                            Ticket += strs[i].Substring(j, 1) + "*";
                        }

                        if (Ticket.EndsWith("*"))
                        {
                            Ticket = Ticket.Substring(0, Ticket.Length - 1);
                        }

                        Ticket = Ticket + "\n";
                    }

                    if (Ticket.EndsWith("\n"))
                    {
                        Ticket = Ticket.Substring(0, Ticket.Length - 1);
                    }
                }

                if (PlayTypeID == PlayType_5_F)
                {
                    string[] strs = Number.Split('\n');

                    for (int i = 0; i < strs.Length; i++)
                    {
                        string[] Locate = new string[5];

                        Regex regex = new Regex(@"^(?<L0>(\d)|([(][\d]+?[)]))(?<L1>(\d)|([(][\d]+?[)]))(?<L2>(\d)|([(][\d]+?[)]))(?<L3>(\d)|([(][\d]+?[)]))(?<L4>(\d)|([(][\d]+?[)]))", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                        Match m = regex.Match(strs[i]);

                        for (int j = 0; j < 5; j++)
                        {
                            Locate[j] = m.Groups["L" + j.ToString()].ToString().Trim();

                            if (Locate[j] == "")
                            {
                                return "";
                            }

                            if (Locate[j].Length > 1)
                            {
                                Locate[j] = Locate[j].Substring(1, Locate[j].Length - 2);
                                if (Locate[j].Length > 1)
                                    Locate[j] = FilterRepeated(Locate[j]);

                                if (Locate[j] == "")
                                {
                                    return "";
                                }
                            }

                            Ticket += Locate[j] + "*";
                        }

                        if (Ticket.EndsWith("*"))
                        {
                            Ticket = Ticket.Substring(0, Ticket.Length - 1);
                        }

                        Ticket = Ticket + "\n";
                    }
                }

                if (Ticket.EndsWith("\n"))
                {
                    Ticket = Ticket.Substring(0, Ticket.Length - 1);
                }

                return Ticket;
            }
            #endregion
        }
    }
}
