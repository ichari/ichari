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
        /// 天天彩选4
        /// </summary>
        public partial class TTCX4 : LotteryBase
        {
            #region 静态变量
            public const int PlayType_ZhiD = 6001;
            public const int PlayType_ZhiF = 6002;
            public const int PlayType_ZuD = 6003;
           // public const int PlayType_ZuF = 6004;
           
            public const int ID = 60;
            public const string sID = "60";
            public const string Name = "天天彩选4";
            public const string Code = "TTCX4";
            public const double MaxMoney = 10000;
            #endregion

            public TTCX4()
            {
                id = 60;
                name = "天天彩选4";
                code = "TTCX4";
            }

            public override bool CheckPlayType(int play_type)
            {
                return ((play_type >= 6001) && (play_type <= 6003));
            }

            public override PlayType[] GetPlayTypeList()
            {
                PlayType[] Result = new PlayType[3];

                Result[0] = new PlayType(PlayType_ZhiD, "直选单式");
                Result[1] = new PlayType(PlayType_ZhiF, "直选复式");
                Result[2] = new PlayType(PlayType_ZuD, "组选单复式");
               // Result[3] = new PlayType(PlayType_ZuF, "组选复式");
              
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
                        LotteryNumber += rd.Next(0, 9 + 1).ToString();
                    }

                    sb.Append(LotteryNumber.Trim() + "\n");
                }

                string Result = sb.ToString();
                Result = Result.Substring(0, Result.Length - 1);
                return Result;
            }

            public override string[] ToSingle(string Number, ref string CanonicalNumber, int PlayType)
            {
                if ((PlayType == PlayType_ZhiD) || (PlayType == PlayType_ZhiF))
                    return ToSingle_Zhi(Number, ref CanonicalNumber);

                if (PlayType == PlayType_ZuD)
                    return ToSingle_Zu(Number, ref CanonicalNumber);
            
                return null;
            }

            #region ToSingle 的具体方法（复试取单式一组（组选或者单选号码））
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
            private string[] ToSingle_Zu(string Number, ref string CanonicalNumber)	//复式取单式, 后面 ref 参数是将彩票规范化，如：(10223) 变成1023
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
                ArrayList al2 = new ArrayList();

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

                for (int k = 0; k < al.Count; k++)
                {
                    if (FilterRepeated(Sort(al[k].ToString())).Length == 1)
                    {
                        al.Remove(al[k]);
                    }
                }

                 for (int kk = 0; kk < al.Count; kk++)
                {
                    al[kk] = Sort(al[kk].ToString());
                }

                 for (int jj = 0; jj < al.Count; jj++)
                 {
                     if (al2.IndexOf(al[jj]) == -1)
                     {
                         al2.Add(al[jj]);
                     }
                 }

                 #region
              //if (al.Count > 1)
              //  {
              //      for (int i = 0; i < al.Count-1; i++)
              //      {
              //         // char[] charnum1 = Sort(al[i].ToString()).ToCharArray();

              //          for (int j = i+1; j < al.Count; j++)
              //          {
              //              //char[] charnum2 = Sort(al[j].ToString()).ToCharArray();

              //              //if ((charnum1[0] == charnum2[0]) && (charnum1[1] == charnum2[1])
              //              //    && (charnum1[2] == charnum2[2] ) && (charnum1[3] == charnum2[3]))
              //              //{
              //              //    //把相同的（数字一样不需要考虑位置的号码取出来单独存放）
              //              //    al.Remove(al[j]);
              //              //}

              //              if (al[i] == al[j] || al[i].Equals(al[j]))
              //              {
              //                  BuildNumber(4);
              //                  al2.Add(j);
              //              }
              //          }
              //      }

              //      for (int k = 0; k < al2.Count; k++)
              //      {
              //         // int i = Shove._Convert.StrToInt(al2[k].ToString(), -1);
              //          int i = Convert.ToInt32(al2[k].ToString());
              //          al.Remove(al[i]);
              //      }
              //  }
                 #endregion

               #endregion
            
                string[] Result = new string[al2.Count];
                for (int i = 0; i < al2.Count; i++)
                    Result[i] = al2[i].ToString();
                return Result;
            }
            #endregion

            public override double ComputeWin(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, int PlayType, params double[] WinMoneyList)
            {
                if ((WinMoneyList == null) || (WinMoneyList.Length < 10))    // 奖金参数排列顺序 zhi, zu3 zu6
                    return -3;

                if ((PlayType == PlayType_ZhiD) || (PlayType == PlayType_ZhiF))
                    return ComputeWin_Zhi(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[0], WinMoneyList[1]);

                if (PlayType == PlayType_ZuD)
                    return ComputeWin_Zu(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[2], WinMoneyList[3], WinMoneyList[4], WinMoneyList[5],WinMoneyList[6], WinMoneyList[7], WinMoneyList[8], WinMoneyList[9]);

                return -4;
            }
            #region ComputeWin 的具体方法
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
            private double ComputeWin_Zu(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney1, double WinMoneyNoWithTax1, double WinMoney2, double WinMoneyNoWithTax2, double WinMoney3, double WinMoneyNoWithTax3, double WinMoney4, double WinMoneyNoWithTax4)	//计算中奖
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

                int Description1 = 0, Description2 = 0,Description3 = 0, Description4 = 0;
                double WinMoney = 0;
                WinMoneyNoWithTax = 0;
                Description = "";

                for (int ii = 0; ii < Lotterys.Length; ii++)
                {
                    string t_str = "";

                    if (Lotterys[ii].Length < 4)
                        continue;
                  
                    string[] Lottery = ToSingle_Zu(Lotterys[ii], ref t_str); //每一种可能中奖的号码
                    if (Lottery == null)
                        continue;
                    if (Lottery.Length < 1)
                        continue;


                    for (int i = 0; i < Lottery.Length; i++)
                    {
                        if (Lottery[i].Length < 4)
                            continue;

                        if (Sort(Lottery[i]) == Sort(WinNumber) || Sort(Lottery[i]).Equals(Sort(WinNumber)))
                        {
                            //计算组4注数(3个号码相同));

                            if ((FilterRepeated(Sort(Lottery[i])).Length == 2) && Sort(Lottery[i]).Substring(1, 1) == Sort(Lottery[i]).Substring(2, 1))
                            {
                                Description1++;
                                WinMoney += WinMoney1;
                                WinMoneyNoWithTax += WinMoneyNoWithTax1;
                            }
                            
                            //计算组6注数(两两相同)
                            if ((FilterRepeated(Sort(Lottery[i])).Length == 2) && Sort(Lottery[i]).Substring(1, 1) != Sort(Lottery[i]).Substring(2, 1))
                            {
                                Description2++;
                                WinMoney += WinMoney2;
                                WinMoneyNoWithTax += WinMoneyNoWithTax2;
                            }

                            //计算组12注数(两相同两不同)
                            if (FilterRepeated(Sort(Lottery[i])).Length == 3)
                            {
                                Description3++;
                                WinMoney += WinMoney3;
                                WinMoneyNoWithTax += WinMoneyNoWithTax3;
                            }

                            //计算组24注数(4不相同)
                            if (FilterRepeated(Sort(Lottery[i])).Length == 4)
                            {
                                Description4++;
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
                if ((PlayType == PlayType_ZhiD) || (PlayType == PlayType_ZhiF))
                    return AnalyseScheme_Zhi(Content, PlayType);

                if (PlayType == PlayType_ZuD)
                    return AnalyseScheme_Zu(Content, PlayType);

                return "";
            }

            #region AnalyseScheme 的具体方法  //(多组组选号码）
            private string AnalyseScheme_Zhi(string Content, int PlayType)
            {
                string[] strs = Content.Split('\n');
                if (strs == null)
                    return "";
                if (strs.Length == 0)
                    return "";

                string Result = "";

                string RegexString;
                if (PlayType == PlayType_ZhiD)
                    RegexString = @"^([\d]){4}";
                else
                    RegexString = @"^(([\d])|([(][\d]{1,10}[)])){4}";
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
                        if (singles.Length >= ((PlayType == PlayType_ZhiD) ? 1 : 2))
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

                string RegexString = @"^(([\d])|([(][\d]{2,10}[)])){4}";                             //@"([\d]){3,10}";
                Regex regex = new Regex(RegexString, RegexOptions.Compiled | RegexOptions.IgnoreCase);

                for (int i = 0; i < strs.Length; i++)
                {
                    Match m = regex.Match(strs[i]);
                    if (m.Success)
                    {
                        string CanonicalNumber = "";
                        string[] singles = ToSingle_Zu(m.Value, ref CanonicalNumber);
                        if (singles == null)
                            continue;
                        if (singles.Length >= ((PlayType == PlayType_ZuD) ? 1 : 2))
                        {
                            Result += CanonicalNumber + "|" + singles.Length.ToString() + "\n";
                        }
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

            public override string ShowNumber(string Number)
            {
                return base.ShowNumber(Number, " ");
            }

            //参数说明 PlayTypeID:玩法类别; Number:投注号码; Multiple: 倍数; MaxMultiple:最大倍数;
            public override Ticket[] ToElectronicTicket_HPSH(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                if (PlayTypeID == PlayType_ZhiD)
                {
                    return ToElectronicTicket_HPSH_D(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                else if (PlayTypeID == PlayType_ZhiF)
                {
                    return ToElectronicTicket_HPSH_F(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                else if (PlayTypeID == PlayType_ZuD)
                {
                    return ToElectronicTicket_HPSH_Zu(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
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

                        al.Add(new Ticket(201, ConvertFormatToElectronTicket_HPSH(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));
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

                        al.Add(new Ticket(208, ConvertFormatToElectronTicket_HPSH(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));
                    }
                }

                Ticket[] Tickets = new Ticket[al.Count];

                for (int i = 0; i < Tickets.Length; i++)
                {
                    Tickets[i] = (Ticket)al[i];
                }

                return Tickets;
            }
            private Ticket[] ToElectronicTicket_HPSH_Zu(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                string[] strs = AnalyseSchemeToElectronicTicket_Zu(Number, PlayTypeID).Split('\n');

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

                        al.Add(new Ticket(202, ConvertFormatToElectronTicket_HPSH(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));
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

                string[] Locate = new string[4];

                string Ticket = "";

                if (PlayTypeID == PlayType_ZhiD || PlayTypeID == PlayType_ZuD || PlayTypeID == PlayType_ZhiF)
                {
                    string[] strs = Number.Split('\n');

                    for (int j = 0; j < strs.Length; j++)
                    {

                        Regex regex = new Regex(@"(?<L0>(\d)|([(][\d]+?[)]))(?<L1>(\d)|([(][\d]+?[)]))(?<L2>(\d)|([(][\d]+?[)]))(?<L3>(\d)|([(][\d]+?[)]))", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                        Match m = regex.Match(strs[j]);
                        for (int i = 0; i < 4; i++)
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

                        Ticket += "\n";
                    }
                }

                if (Ticket.EndsWith("\n"))
                    Ticket = Ticket.Substring(0, Ticket.Length - 1);

                return Ticket;
            }

            private string AnalyseSchemeToElectronicTicket_Zu(string Content, int PlayType)
            {
                string[] strs = Content.Split('\n');
                if (strs == null)
                    return "";
                if (strs.Length == 0)
                    return "";

                string Result = "";

                string RegexString;

                RegexString = @"(([\d])|([(][\d]{1,10}[)])){4}";                            //@"([\d]){3,10}";
                Regex regex = new Regex(RegexString, RegexOptions.Compiled | RegexOptions.IgnoreCase);

                for (int i = 0; i < strs.Length; i++)
                {
                    Match m = regex.Match(strs[i]);
                    if (m.Success)
                    {
                        string CanonicalNumber = "";
                        string[] singles = ToSingle_Zu(m.Value, ref CanonicalNumber);
                        if (singles == null)
                            continue;

                        if (singles.Length >= 1)
                        {
                            for (int j = 0; j < singles.Length; j++)
                            {
                                Result += singles[j] + "|1\n";
                            }
                        }
                    }
                }

                if (Result.EndsWith("\n"))
                    Result = Result.Substring(0, Result.Length - 1);
                return Result;
            }

            public override string HPSH_ToElectronicTicket(int PlayTypeID, string Number, ref string TicketNumber, ref int NewPlayTypeID)
            {
                if ((PlayTypeID == PlayType_ZhiD) || (PlayTypeID == PlayType_ZhiF) || (PlayTypeID == PlayType_ZuD))
                {
                    return HPSH_ToElectronicTicket(PlayTypeID, Number, ref TicketNumber, ref NewPlayTypeID);
                }

                return "";

            }

            private string HPSH_ToElectronicTicket_D(int PlayTypeID, string Number, ref string TicketNumber, ref int NewPlayTypeID)
            {
                Number = Number.Trim();

                string Ticket = "";

                string[] strs = Number.Split(',');

                foreach (string str in strs)
                {
                    if (str.Length > 1)
                    {
                        Ticket += "(" + str + ")";

                        continue;
                    }

                    Ticket += str;
                }

                NewPlayTypeID = PlayTypeID;

                return Ticket;
            }

            #endregion
        }
    }
}
