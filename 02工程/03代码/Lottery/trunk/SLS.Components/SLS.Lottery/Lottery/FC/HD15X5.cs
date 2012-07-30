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
        /// 华东15选5
        /// </summary>
        public partial class HD15X5 : LotteryBase
        {
            #region 静态变量
            public const int PlayType_D = 5901;
            public const int PlayType_F = 5902;
            public const int PlayType_DT = 5903;

            public const int ID = 59;
            public const string sID = "59";
            public const string Name = "华东15选5";
            public const string Code = "HD15X5";
            public const double MaxMoney = 6006;
            #endregion

            public HD15X5()
            {
                id = 59;
                name = "华东15选5";
                code = "HD15X5";
            }

            public override bool CheckPlayType(int play_type)
            {
                return ((play_type >= 5901) && (play_type <= 5903));
            }

            public override PlayType[] GetPlayTypeList()
            {
                PlayType[] Result = new PlayType[3];

                Result[0] = new PlayType(PlayType_D, "单式");
                Result[1] = new PlayType(PlayType_F, "复式");
                Result[2] = new PlayType(PlayType_DT, "胆拖");

                return Result;
            }

            public override string BuildNumber(int Num)	//id = 21
            {
                System.Random rd = new Random();
                StringBuilder sb = new StringBuilder();
                ArrayList al = new ArrayList();

                for (int i = 0; i < Num; i++)
                {
                    al.Clear();
                    for (int j = 0; j < 5; j++)
                    {
                        int Ball = 0;
                        while ((Ball == 0) || isExistBall(al, Ball))
                            Ball = rd.Next(1, 15 + 1);
                        al.Add(Ball.ToString().PadLeft(2, '0'));
                    }

                    CompareToAscii compare = new CompareToAscii();
                    al.Sort(compare);

                    string LotteryNumber = "";
                    for (int j = 0; j < al.Count; j++)
                        LotteryNumber += al[j].ToString() + " ";

                    sb.Append(LotteryNumber.Trim() + "\n");
                }

                string Result = sb.ToString();
                Result = Result.Substring(0, Result.Length - 1);
                return Result;
            }

            public override string[] ToSingle(string Number, ref string CanonicalNumber, int PlayType)
            {
                if (PlayType == PlayType_D)
                    return ToSingle_DF(Number, ref CanonicalNumber);

                if (PlayType == PlayType_F)
                    return ToSingle_DF(Number, ref CanonicalNumber);

                if (PlayType == PlayType_DT)
                    return ToSingle_DT(Number, ref CanonicalNumber);

                return null;
            }
            #region ToSingle的具体方法
            private string[] ToSingle_DF(string Number, ref string CanonicalNumber)	//复式取单式, 后面 ref 参数是将彩票规范化，如：01 01 02... 变成 01 02...
            {
                string[] strs = FilterRepeated(Number.Trim().Split(' '));
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
            private string[] ToSingle_DT(string Number, ref string CanonicalNumber)	//胆拖取单式, 后面 ref 参数是将彩票规范化，如：01 01 02... 变成 01 02...
            {
                string[] strs = Number.Split(',');
                CanonicalNumber = "";

                if (strs.Length != 2)
                {
                    CanonicalNumber = "";
                    return null;
                }

                string[] strs_Dan = FilterRepeated(strs[0].Trim().Split(' '));
                string[] strs_Tuo = FilterRepeated(strs[1].Trim().Split(' '));
                string[] FilterDan = FilterRepeated(strs_Dan, strs_Tuo);

                if ((FilterDan.Length + strs_Tuo.Length) < 5 || (FilterDan.Length < 1) || (FilterDan.Length > 4) || (strs_Tuo.Length < 2) || (strs_Tuo.Length > 14))
                {
                    CanonicalNumber = "";
                    return null;
                }

                for (int i = 0; i < FilterDan.Length; i++)
                    CanonicalNumber += (FilterDan[i] + " ");
                CanonicalNumber += ", ";
                for (int i = 0; i < strs_Tuo.Length; i++)
                    CanonicalNumber += (strs_Tuo[i] + " ");

                CanonicalNumber = CanonicalNumber.Trim();

                ArrayList al = new ArrayList();

                #region 循环取单式

                int DanLength = FilterDan.Length;
                int TuoLength = strs_Tuo.Length;

                if (DanLength == 1)
                {
                    for (int m = 0; m < TuoLength - 3; m++)
                        for (int n = m + 1; n < TuoLength - 2; n++)
                            for (int p = n + 1; p < TuoLength - 1; p++)
                                for (int q = p + 1; q < TuoLength; q++)
                                {
                                    al.Add(FilterDan[0].ToString() + " " + strs_Tuo[m].ToString() + " " + strs_Tuo[n].ToString() + " " + strs_Tuo[p].ToString() + " " + strs_Tuo[q].ToString());
                                }
                }
                else if (DanLength == 2)
                {
                    for (int m = 0; m < TuoLength - 2; m++)
                        for (int n = m + 1; n < TuoLength - 1; n++)
                            for (int p = n + 1; p < TuoLength; p++)
                            {
                                al.Add(FilterDan[0].ToString() + " " + FilterDan[1].ToString() + " " + strs_Tuo[m].ToString() + " " + strs_Tuo[n].ToString() + " " + strs_Tuo[p].ToString());
                            }
                }
                else if (DanLength == 3)
                {
                    for (int m = 0; m < TuoLength - 1; m++)
                        for (int n = m + 1; n < TuoLength; n++)
                        {
                            al.Add(FilterDan[0].ToString() + " " + FilterDan[1].ToString() + " " + FilterDan[2].ToString() + " " + strs_Tuo[m].ToString() + " " + strs_Tuo[n].ToString());
                        }
                }
                else if (DanLength == 4)
                {
                    for (int m = 0; m < TuoLength; m++)
                    {
                        al.Add(FilterDan[0].ToString() + " " + FilterDan[1].ToString() + " " + FilterDan[2].ToString() + " " + FilterDan[3].ToString() + " " + strs_Tuo[m].ToString());
                    }
                }

                #endregion

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
                WinNumber = WinNumber.Trim();
                if (WinNumber.Length < 14)	//14: "01 02 03 04 05"
                    return -1;
                string[] Lotterys = SplitLotteryNumber(Number);
                if (Lotterys == null)
                    return -2;
                if (Lotterys.Length < 1)
                    return -2;
                if ((WinMoneyList == null) || (WinMoneyList.Length < 6))
                    return -3;

                int Description0 = 0, Description1 = 0, Description2 = 0;
                double WinMoney = 0;
                WinMoneyNoWithTax = 0;
                Description = "";

                for (int ii = 0; ii < Lotterys.Length; ii++)
                {
                    string t_str = "";
                    string[] Lottery = ToSingle(Lotterys[ii], ref t_str, PlayType);
                    if (Lottery == null)
                        continue;
                    if (Lottery.Length < 1)
                        continue;

                    for (int i = 0; i < Lottery.Length; i++)
                    {
                        if (Lottery[i].Length < 14)
                            continue;

                        string[] Red = new string[5];
                        Regex regex = new Regex(@"^(?<R0>\d\d\s)(?<R1>\d\d\s)(?<R2>\d\d\s)(?<R3>\d\d\s)(?<R4>\d\d)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                        Match m = regex.Match(Lottery[i]);
                        int j;
                        int RedRight = 0;
                        bool Full = true;
                        for (j = 0; j < 5; j++)
                        {
                            Red[j] = m.Groups["R" + j.ToString()].ToString().Trim();
                            if (Red[j] == "")
                            {
                                Full = false;
                                break;
                            }
                            if (WinNumber.IndexOf(Red[j]) >= 0)
                                RedRight++;
                        }
                        if (!Full)
                            continue;

                        if (RedRight == 5)
                        {
                            if (isThreeContinuum(WinNumber))
                            {
                                Description0++;
                                WinMoney += WinMoneyList[0];
                                WinMoneyNoWithTax += WinMoneyList[1];
                            }

                            Description1++;
                            WinMoney += WinMoneyList[2];
                            WinMoneyNoWithTax += WinMoneyList[3];
                            continue;
                        }
                        if (RedRight == 4)
                        {
                            Description2++;
                            WinMoney += WinMoneyList[4];
                            WinMoneyNoWithTax += WinMoneyList[5];
                            continue;
                        }
                    }
                }

                if (Description0 > 0)
                    Description = "特等奖" + Description0.ToString() + "注";
                if (Description1 > 0)
                {
                    if (Description != "")
                        Description += "，";
                    Description += "一等奖" + Description1.ToString() + "注";
                }
                if (Description2 > 0)
                {
                    if (Description != "")
                        Description += "，";
                    Description += "二等奖" + Description2.ToString() + "注";
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

                string RegexString;
                if (PlayType == PlayType_D)
                    RegexString = @"^(\d\d\s){4}\d\d";
                else if (PlayType == PlayType_F)
                    RegexString = @"^(\d\d\s){4,14}\d\d";
                else
                    RegexString = @"^(\d\d\s){1,4}(,)(\s)(\d\d\s){1,13}\d\d";
                
                Regex regex = new Regex(RegexString, RegexOptions.Compiled | RegexOptions.IgnoreCase);

                for (int i = 0; i < strs.Length; i++)
                {
                    Match m = regex.Match(strs[i]);
                    if (m.Success)
                    {
                        string CanonicalNumber = "";
                        string[] singles = ToSingle(m.Value, ref CanonicalNumber, PlayType);
                        if (singles == null)
                            continue;
                        if ((singles.Length >= ((PlayType == PlayType_D) ? 1 : 2)) && (singles.Length <= (MaxMoney / 2)))
                            Result += CanonicalNumber + "|" + singles.Length.ToString() + "\n";
                    }
                }

                if (Result.EndsWith("\n"))
                    Result = Result.Substring(0, Result.Length - 1);
                return Result;
            }

            public override bool AnalyseWinNumber(string Number)
            {
                string t_str = "";
                string[] WinLotteryNumber = ToSingle(Number, ref t_str, PlayType_D);

                if (WinLotteryNumber == null)
                    return false;

                return true;
            }

            private string[] FilterRepeated(string[] NumberPart)
            {
                ArrayList al = new ArrayList();
                for (int i = 0; i < NumberPart.Length; i++)
                {
                    int Ball = Shove._Convert.StrToInt(NumberPart[i], -1);
                    if ((Ball >= 1) && (Ball <= 15) && (!isExistBall(al, Ball)))
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

            private bool isThreeContinuum(string Number)
            {
                string[] strs = FilterRepeated(Number.Split(' '));
                if (strs.Length < 5)
                    return false;

                int[] Red = new int[5];
                for (int i = 0; i < 5; i++)
                    Red[i] = int.Parse(strs[i]);

                for (int i = 0; i < 2; i++)
                {
                    if (((Red[i] + 1) == Red[i + 1]) && ((Red[i] + 2) == Red[i + 2]) && ((Red[i] + 3) == Red[i + 3]))
                        return true;
                }

                return false;
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
                    case "FCR8000":
                        if ((PlayTypeID == PlayType_D) || (PlayTypeID == PlayType_F))
                        {
                            return GetPrintKeyList_FCR8000(Numbers);
                        }
                        break;
                }

                return "";
            }
            #region GetPrintKeyList 的具体方法
            private string GetPrintKeyList_FCR8000(string[] Numbers)
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
                if (PlayTypeID == PlayType_DT)
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

                    if (strs.Length > 39)
                    {
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
                    else
                    {
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

                    if (strs.Length > 39)
                    {
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
                    else
                    {
                        for (int i = 0; i < strs.Length; i++)
                        {
                            string Numbers = "";
                            EachMoney = 0;

                            Numbers = strs[i].ToString().Split('|')[0];
                            EachMoney += 2 * double.Parse(strs[i].ToString().Split('|')[1]);

                            Money += EachMoney * EachMultiple;

                            al.Add(new Ticket(103, ConvertFormatToElectronTicket_HPSH(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));
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

            // 转换为需要的彩票格式
            private string ConvertFormatToElectronTicket_HPSH(int PlayTypeID, string Number)
            {
                Number = Number.Trim();

                string Ticket = "";

                if (PlayTypeID == PlayType_D || PlayTypeID == PlayType_F)
                {
                    Ticket = Number.Replace(" ", ",");
                }

                if (PlayTypeID == PlayType_DT)
                {
                    Ticket = Number.Replace(" ", ",").Replace(",,,","#00#");
                }

                return Ticket;
            }

            #endregion

            //参数说明 PlayTypeID:玩法类别; Number:投注号码; Multiple: 倍数; MaxMultiple:最大倍数;
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
                if (PlayTypeID == PlayType_DT)
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

                    if (strs.Length > 39)
                    {
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
                    else
                    {
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

                    if (strs.Length > 39)
                    {
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
                    else
                    {
                        for (int i = 0; i < strs.Length; i++)
                        {
                            string Numbers = "";
                            EachMoney = 0;

                            Numbers = strs[i].ToString().Split('|')[0];
                            EachMoney += 2 * double.Parse(strs[i].ToString().Split('|')[1]);

                            Money += EachMoney * EachMultiple;

                            al.Add(new Ticket(103, ConvertFormatToElectronTicket_HPJX(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));
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


            // 转换为需要的彩票格式
            private string ConvertFormatToElectronTicket_HPJX(int PlayTypeID, string Number)
            {
                Number = Number.Trim();

                string Ticket = "";

                if (PlayTypeID == PlayType_D || PlayTypeID == PlayType_F)
                {
                    Ticket = Number.Replace(" ", ",");
                }

                if (PlayTypeID == PlayType_DT)
                {
                    Ticket = Number.Replace(" ", ",").Replace(",,,", "#00#");
                }

                return Ticket;
            }

            #endregion

            private string AnalyseSchemeToElectronicTicket_F(string Content, int PlayType)
            {
                string[] strs = Content.Split('\n');
                if (strs == null)
                    return "";
                if (strs.Length == 0)
                    return "";

                string Result = "";

                string RegexString;
                if (PlayType == PlayType_D)
                    RegexString = @"(\d\d\s){4}\d\d";
                else if (PlayType == PlayType_F)
                    RegexString = @"(\d\d\s){4,14}\d\d";
                else
                    RegexString = @"(\d\d\s){1,4}(,)(\s)(\d\d\s){1,13}\d\d";
                Regex regex = new Regex(RegexString, RegexOptions.Compiled | RegexOptions.IgnoreCase);

                for (int i = 0; i < strs.Length; i++)
                {
                    Match m = regex.Match(strs[i]);
                    if (m.Success)
                    {
                        string CanonicalNumber = "";
                        string[] singles = ToSingle(m.Value, ref CanonicalNumber, PlayType);
                        if (singles == null)
                            continue;
                        if ((singles.Length >= ((PlayType == PlayType_D) ? 1 : 2)) && (singles.Length <= (MaxMoney / 2)))
                        {
                            if (PlayType == PlayType_F && singles.Length > 1287)
                            {
                                if (singles.Length >= 1)
                                {
                                    for (int j = 0; j < singles.Length; j++)
                                    {
                                        Result += singles[j] + "|1\n";
                                    }
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
        }
    }
}