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
        /// 东方6+1
        /// </summary>
        public partial class DF6J1 : LotteryBase
        {
            #region 静态变量
            public const int PlayType_D = 5801;
            public const int PlayType_F = 5802;

            public const int ID = 58;
            public const string sID = "58";
            public const string Name = "东方6+1";
            public const string Code = "DF6J1";
            public const double MaxMoney = 20000;
            #endregion

            public DF6J1()
            {
                id = 58;
                name = "东方6+1";
                code = "DF6J1";
            }

            public override bool CheckPlayType(int play_type)
            {
                return ((play_type >= 5801) && (play_type <= 5802));
            }

            public override PlayType[] GetPlayTypeList()
            {
                PlayType[] Result = new PlayType[2];

                Result[0] = new PlayType(PlayType_D, "单式");
                Result[1] = new PlayType(PlayType_F, "复式");

                return Result;
            }

            public override string BuildNumber(int Num)	//id = 58
            {
                System.Random rd = new Random();
                StringBuilder sb = new StringBuilder();

                for (int i = 0; i < Num; i++)
                {
                    string LotteryNumber = "";
                    for (int j = 0; j < 6; j++)
                    {
                        LotteryNumber += rd.Next(0, 9 + 1).ToString();
                    }

                    LotteryNumber += "+" + "鼠牛虎兔龙蛇马羊猴鸡狗猪"[rd.Next(0, 11 + 1)].ToString();

                    sb.Append(LotteryNumber.Trim() + "\n");
                }

                string Result = sb.ToString();
                Result = Result.Substring(0, Result.Length - 1);
                return Result;
            }

            public override string[] ToSingle(string Number, ref string CanonicalNumber, int PlayType)	//复式取单式, 后面 ref 参数是将彩票规范化，如：103(00)...变成1030...
            {
                string[] Locate = new string[7];
                CanonicalNumber = "";

                Regex regex = new Regex(@"^(?<L0>(\d)|([(][\d]+?[)]))(?<L1>(\d)|([(][\d]+?[)]))(?<L2>(\d)|([(][\d]+?[)]))(?<L3>(\d)|([(][\d]+?[)]))(?<L4>(\d)|([(][\d]+?[)]))(?<L5>(\d)|([(][\d]+?[)]))[+](?<L6>([鼠牛虎兔龙蛇马羊猴鸡狗猪]+))", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                Match m = regex.Match(Number);
                for (int i = 0; i < 6; i++)
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

                Locate[6] = m.Groups["L6"].ToString().Trim();
                if (Locate[6] == "")
                {
                    CanonicalNumber = "";
                    return null;
                }

                if (Locate[6].Length > 1)
                {
                    Locate[6] = FilterRepeated_SX(Locate[6]);
                    if (Locate[6] == "")
                    {
                        CanonicalNumber = "";
                        return null;
                    }
                }

                CanonicalNumber += "+" + Locate[6];

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
                                    for (int i_5 = 0; i_5 < Locate[5].Length; i_5++)
                                    {
                                        string str_5 = str_4 + Locate[5][i_5].ToString();
                                        for (int i_6 = 0; i_6 < Locate[6].Length; i_6++)
                                        {
                                            string str_6 = str_5 + "+" + Locate[6][i_6].ToString();
                                            al.Add(str_6);
                                        }
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

            public override double ComputeWin(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, int PlayType, params double[] WinMoneyList)
            {
                WinNumber = WinNumber.Trim();
                if (WinNumber.Length < 8)	//8: 123456+猪
                    return -1;
                string[] Lotterys = SplitLotteryNumber(Number);
                if (Lotterys == null)
                    return -2;
                if (Lotterys.Length < 1)
                    return -2;
                if ((WinMoneyList == null) || (WinMoneyList.Length < 12))
                    return -3;

                int Description1 = 0, Description2 = 0, Description3 = 0, Description4 = 0, Description5 = 0, Description6 = 0;
                double WinMoney = 0;
                WinMoneyNoWithTax = 0;
                Description = "";

                for (int ii = 0; ii < Lotterys.Length; ii++)
                {
                    string[] Locate = new string[7];
                    Regex regex = new Regex(@"^(?<L0>(\d)|([(][\d]+?[)]))(?<L1>(\d)|([(][\d]+?[)]))(?<L2>(\d)|([(][\d]+?[)]))(?<L3>(\d)|([(][\d]+?[)]))(?<L4>(\d)|([(][\d]+?[)]))(?<L5>(\d)|([(][\d]+?[)]))[+](?<L6>([鼠牛虎兔龙蛇马羊猴鸡狗猪]+))", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                    Match m = regex.Match(Lotterys[ii]);

                    bool Full = true;
                    int RedRight = 0;
                    int BlueRight = 0;

                    for (int j = 0; j < 6; j++)
                    {
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

                        if (Locate[j].IndexOf(WinNumber.Substring(j, 1)) >= 0)
                        {
                            RedRight++;
                        }
                    }

                    Locate[6] = m.Groups["L6"].ToString().Trim();
                    if (Locate[6] == "")
                    {
                        continue;
                    }

                    if (Locate[6].Length > 1)
                    {
                        Locate[6] = FilterRepeated_SX(Locate[6]);
                        if (Locate[6] == "")
                        {
                            continue;
                        }
                    }

                    if (Locate[6].IndexOf(WinNumber.Substring(7,1)) >= 0)
                    {
                        BlueRight = 1;
                    }

                    if (!Full)
                    {
                        continue;
                    }

                    if (RedRight < 3 && BlueRight == 0)
                    {
                        continue;
                    }

                    string t_str = "";
                    string[] Lottery = ToSingle(Lotterys[ii], ref t_str, PlayType);
                    if (Lottery == null)
                        continue;
                    if (Lottery.Length < 1)
                        continue;

                    for (int i = 0; i < Lottery.Length; i++)
                    {
                        RedRight = 0;
                        BlueRight = 0;

                        m = regex.Match(Lottery[i]);

                        for (int j = 0; j < 6; j++)
                        {
                            Locate[j] = m.Groups["L" + j.ToString()].ToString().Trim();

                            if (Locate[j].IndexOf(WinNumber.Substring(j, 1)) >= 0)
                            {
                                RedRight++;
                            }
                        }

                        Locate[6] = m.Groups["L6"].ToString().Trim();

                        if (Locate[6].IndexOf(WinNumber.Substring(7, 1)) >= 0)
                        {
                            BlueRight = 1;
                        }

                        if (Lottery[i].Length < 8)
                            continue;

                        if (RedRight < 3 && BlueRight == 0)
                        {
                            continue;
                        }

                        if (Lottery[i] == WinNumber)
                        {
                            Description1++;
                            WinMoney += WinMoneyList[0];
                            WinMoneyNoWithTax += WinMoneyList[1];
                            continue;
                        }

                        if (RedRight == 6)
                        {
                            Description2++;
                            WinMoney += WinMoneyList[2];
                            WinMoneyNoWithTax += WinMoneyList[3];
                            continue;
                        }

                        if ((RedRight == 5) && (BlueRight == 1))
                        {
                            Description3++;
                            WinMoney += WinMoneyList[4];
                            WinMoneyNoWithTax += WinMoneyList[5];
                            continue;
                        }

                        if (((RedRight == 4) && (BlueRight == 1)) || (RedRight == 5))
                        {
                            Description4++;
                            WinMoney += WinMoneyList[6];
                            WinMoneyNoWithTax += WinMoneyList[7];
                            continue;
                        }

                        if (((RedRight == 3) && (BlueRight == 1)) || (RedRight == 4))
                        {
                            Description5++;
                            WinMoney += WinMoneyList[8];
                            WinMoneyNoWithTax += WinMoneyList[9];
                            continue;
                        }

                        if ((BlueRight == 1) || (RedRight == 3))
                        {
                            Description6++;
                            WinMoney += WinMoneyList[10];
                            WinMoneyNoWithTax += WinMoneyList[11];
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
                if (Description != "")
                    Description += "。";

                return WinMoney;
            }

            public override string AnalyseScheme(string Content, int PlayType)
            {
                string[] strs = Content.Split('\n');
                if (strs == null)
                    return "";
                if (strs.Length == 0)
                    return "";

                string Result = "";
                int Num = 1;
                string CanonicalNumber = "";

                string[] Locate = new string[7];
                CanonicalNumber = "";

                Regex regex = new Regex(@"^(?<L0>(\d)|([(][\d]+?[)]))(?<L1>(\d)|([(][\d]+?[)]))(?<L2>(\d)|([(][\d]+?[)]))(?<L3>(\d)|([(][\d]+?[)]))(?<L4>(\d)|([(][\d]+?[)]))(?<L5>(\d)|([(][\d]+?[)]))[+](?<L6>([鼠牛虎兔龙蛇马羊猴鸡狗猪]+))", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                
                for (int i = 0; i < strs.Length; i++)
                {
                    CanonicalNumber = "";

                    Match m = regex.Match(strs[i]);
                    for (int j = 0; j < 6; j++)
                    {
                        Locate[j] = m.Groups["L" + j.ToString()].ToString().Trim();
                        if (Locate[j] == "")
                        {
                            CanonicalNumber = "";
                            return null;
                        }

                        if (Locate[j].Length > 1)
                        {
                            Locate[j] = Locate[j].Substring(1, Locate[j].Length - 2);
                            if (Locate[j].Length > 1)
                                Locate[j] = FilterRepeated(Locate[j]);
                            if (Locate[j] == "")
                            {
                                CanonicalNumber = "";
                                return null;
                            }

                            Num = Locate[j].Length * Num;
                        }

                        if (Locate[j].Length > 1)
                            CanonicalNumber += "(" + Locate[j] + ")";
                        else
                            CanonicalNumber += Locate[j];
                    }

                    Locate[6] = m.Groups["L6"].ToString().Trim();
                    if (Locate[6] == "")
                    {
                        CanonicalNumber = "";
                        return null;
                    }

                    if (Locate[6].Length > 1)
                    {
                        Locate[6] = FilterRepeated_SX(Locate[6]);
                        if (Locate[6] == "")
                        {
                            CanonicalNumber = "";
                            return null;
                        }

                        Num = Locate[6].Length * Num;
                    }

                    CanonicalNumber += "+" + Locate[6];

                    Result += CanonicalNumber + "|" + Num.ToString() + "\n";
                }

                if (Result.EndsWith("\n"))
                    Result = Result.Substring(0, Result.Length - 1);
                return Result;
            }

            public override bool AnalyseWinNumber(string Number)
            {
                Regex regex = new Regex(@"^([\d]){6}[+][鼠牛虎兔龙蛇马羊猴鸡狗猪]", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                if (!regex.IsMatch(Number))
                    return false;

                string t_str = "";
                string[] WinLotteryNumber = ToSingle(Number, ref t_str, PlayType_D);

                if ((WinLotteryNumber == null) || (WinLotteryNumber.Length != 1))
                    return false;

                return true;
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
                    if ((Result.IndexOf(NumberPart.Substring(i, 1)) == -1) && ("0123456789".IndexOf(NumberPart.Substring(i, 1)) >= 0))
                        Result += NumberPart.Substring(i, 1);
                }
                return Sort(Result);
            }

            private string FilterRepeated_SX(string NumberPart)
            {
                string Result = "";
                for (int i = 0; i < NumberPart.Length; i++)
                {
                    if ((Result.IndexOf(NumberPart.Substring(i, 1)) == -1) && ("鼠牛虎兔龙蛇马羊猴鸡狗猪".IndexOf(NumberPart.Substring(i, 1)) >= 0))
                        Result += NumberPart.Substring(i, 1);
                }
                return Result;
            }

            public override string ShowNumber(string Number)
            {
                return base.ShowNumber(Number, " ");
            }

            //参数说明 PlayTypeID:玩法类别; Number:投注号码; Multiple: 倍数; MaxMultiple:最大倍数;
            public override Ticket[] ToElectronicTicket_HPSH(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                if (PlayTypeID == PlayType_D)
                {
                    return ToElectronicTicket_HPSH_D(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                else if (PlayTypeID == PlayType_F)
                {
                    return ToElectronicTicket_HPSH_F(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
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

            // 转换为需要的彩票格式
            private string ConvertFormatToElectronTicket_HPSH(int PlayTypeID, string Number)
            {
                Number = Number.Trim();

                string[] Locate = new string[7];

                string Ticket = "";

                if (PlayTypeID == PlayType_D || PlayTypeID == PlayType_F)
                {
                    string[] strs = Number.Split('\n');

                    for (int j = 0; j < strs.Length; j++)
                    {
                        Regex regex = new Regex(@"(?<L0>(\d)|([(][\d]+?[)]))(?<L1>(\d)|([(][\d]+?[)]))(?<L2>(\d)|([(][\d]+?[)]))(?<L3>(\d)|([(][\d]+?[)]))(?<L4>(\d)|([(][\d]+?[)]))(?<L5>(\d)|([(][\d]+?[)]))[+](?<L6>([鼠牛虎兔龙蛇马羊猴鸡狗猪]+))", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                        Match m = regex.Match(strs[j]);
                        for (int i = 0; i < 6; i++)
                        {
                            Locate[i] = m.Groups["L" + i.ToString()].ToString().Trim();
                            if (Locate[i] == "")
                            {
                                return "";
                            }

                            if (Locate[i].Length > 1)
                            {
                                Locate[i] = Locate[i].Substring(1, Locate[i].Length - 2);
                                if (Locate[i].Length > 1)
                                    Locate[i] = FilterRepeated(Locate[i]);
                                if (Locate[i] == "")
                                {
                                    return "";
                                }
                            }

                            Ticket += Locate[i] + ",";
                        }

                        if (Ticket.EndsWith(","))
                            Ticket = Ticket.Substring(0, Ticket.Length - 1);

                        Locate[6] = m.Groups["L6"].ToString().Trim();
                        if (Locate[6] == "")
                        {
                            return "";
                        }

                        if (Locate[6].Length > 0)
                        {
                            Locate[6] = FilterRepeated_SX(Locate[6]);
                            if (Locate[6] == "")
                            {
                                return "";
                            }

                            Ticket += "#";

                            for (int i = 0; i < Locate[6].Length; i++)
                            {
                                Ticket += Locate[6].Substring(i, 1).Replace("鼠", "01").Replace("牛", "02").Replace("虎", "03").Replace("兔", "04").Replace("龙", "05").Replace("蛇", "06").Replace("马", "07").Replace("羊", "08").Replace("猴", "09").Replace("鸡", "10").Replace("狗", "11").Replace("猪", "12") + ",";
                            }
                        }

                        if (Ticket.EndsWith(","))
                            Ticket = Ticket.Substring(0, Ticket.Length - 1);

                        Ticket += "\n";
                    }
                }

                if (Ticket.EndsWith("\n"))
                    Ticket = Ticket.Substring(0, Ticket.Length - 1);

                return Ticket;
            }
            #endregion

            public override Ticket[] ToElectronicTicket_HPJX(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                if (PlayTypeID == PlayType_D)
                {
                    return ToElectronicTicket_HPJX_D(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                else if (PlayTypeID == PlayType_F)
                {
                    return ToElectronicTicket_HPJX_F(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
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

            // 转换为需要的彩票格式
            private string ConvertFormatToElectronTicket_HPJX(int PlayTypeID, string Number)
            {
                Number = Number.Trim();

                string[] Locate = new string[7];

                string Ticket = "";

                if (PlayTypeID == PlayType_D || PlayTypeID == PlayType_F)
                {
                    string[] strs = Number.Split('\n');

                    for (int j = 0; j < strs.Length; j++)
                    {
                        Regex regex = new Regex(@"(?<L0>(\d)|([(][\d]+?[)]))(?<L1>(\d)|([(][\d]+?[)]))(?<L2>(\d)|([(][\d]+?[)]))(?<L3>(\d)|([(][\d]+?[)]))(?<L4>(\d)|([(][\d]+?[)]))(?<L5>(\d)|([(][\d]+?[)]))[+](?<L6>([鼠牛虎兔龙蛇马羊猴鸡狗猪]+))", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                        Match m = regex.Match(strs[j]);
                        for (int i = 0; i < 6; i++)
                        {
                            Locate[i] = m.Groups["L" + i.ToString()].ToString().Trim();
                            if (Locate[i] == "")
                            {
                                return "";
                            }

                            if (Locate[i].Length > 1)
                            {
                                Locate[i] = Locate[i].Substring(1, Locate[i].Length - 2);
                                if (Locate[i].Length > 1)
                                    Locate[i] = FilterRepeated(Locate[i]);
                                if (Locate[i] == "")
                                {
                                    return "";
                                }
                            }

                            Ticket += Locate[i] + ",";
                        }

                        if (Ticket.EndsWith(","))
                            Ticket = Ticket.Substring(0, Ticket.Length - 1);

                        Locate[6] = m.Groups["L6"].ToString().Trim();
                        if (Locate[6] == "")
                        {
                            return "";
                        }

                        if (Locate[6].Length > 0)
                        {
                            Locate[6] = FilterRepeated_SX(Locate[6]);
                            if (Locate[6] == "")
                            {
                                return "";
                            }

                            Ticket += "#";

                            for (int i = 0; i < Locate[6].Length; i++)
                            {
                                Ticket += Locate[6].Substring(i, 1).Replace("鼠", "01").Replace("牛", "02").Replace("虎", "03").Replace("兔", "04").Replace("龙", "05").Replace("蛇", "06").Replace("马", "07").Replace("羊", "08").Replace("猴", "09").Replace("鸡", "10").Replace("狗", "11").Replace("猪", "12") + ",";
                            }
                        }

                        if (Ticket.EndsWith(","))
                            Ticket = Ticket.Substring(0, Ticket.Length - 1);

                        Ticket += "\n";
                    }
                }

                if (Ticket.EndsWith("\n"))
                    Ticket = Ticket.Substring(0, Ticket.Length - 1);

                return Ticket;
            }
            #endregion
        }
    }
}