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
        /// 四场进球彩
        /// </summary>
        public partial class JCZQ : LotteryBase
        {
            #region 静态变量
            public const int PlayType_SPF = 7201;
            public const int PlayType_BF = 7202;
            public const int PlayType_ZJQ = 7203;
            public const int PlayType_BQCSPF = 7204;
            public const int PlayType_DGBF = 7205;

            public const int ID = 72;
            public const string sID = "72";
            public const string Name = "竞彩足球";
            public const string Code = "JCZQ";
            public const double MaxMoney = 30000000;
            #endregion

            public JCZQ()
            {
                id = 72;
                name = "竞彩足球";
                code = "JCZQ";
            }

            public override bool CheckPlayType(int play_type)
            {
                return ((play_type >= 7201) && (play_type <= 7205));
            }

            public override PlayType[] GetPlayTypeList()
            {
                PlayType[] Result = new PlayType[5];

                Result[0] = new PlayType(PlayType_SPF, "胜平负");
                Result[1] = new PlayType(PlayType_BF, "比分");
                Result[2] = new PlayType(PlayType_ZJQ, "总进球");
                Result[3] = new PlayType(PlayType_BQCSPF, "半全场胜平负");
                Result[4] = new PlayType(PlayType_DGBF, "单关比分");

                return Result;
            }

            public override string BuildNumber(int Num)	//id = 72
            {
                return "";
            }

            public override string[] ToSingle(string Number, ref string CanonicalNumber, int PlayType)	//复式取单式, 后面 ref 参数是将彩票规范化，如：103(00)... 变成1030...
            {
                int SchemeLength = Number.Split(';').Length;

                if (SchemeLength < 3)
                {
                    return null;
                }

                string strPlayType = Number.Trim().Split(';')[0].ToString();
                string BuyNumber = Number.Trim().Split(';')[1].ToString();
                string Numbers = BuyNumber.Substring(1, BuyNumber.Length - 1).Substring(0, BuyNumber.Length - 2).ToString().Trim();

                if (Numbers == "")
                {
                    return null;
                }

                #region 投注对阵分析

                string NumbersDan = "";
                string NumbersTuo = "";

                if (Numbers.IndexOf("]") >= 0)
                {
                    NumbersDan = Numbers.Replace("][", "&").Split('&')[0];
                    NumbersTuo = Numbers.Replace("][", "&").Split('&')[1];
                }
                else
                {
                    NumbersTuo = Number;
                }

                int GamesNumberDan = NumbersDan.Split('|').Length;
                int GamesNumberTuo = NumbersTuo.Split('|').Length;

                if (string.IsNullOrEmpty(NumbersDan.Split('|')[0]))
                {
                    GamesNumberDan = 0;
                }

                if (string.IsNullOrEmpty(NumbersTuo.Split('|')[0]))
                {
                    GamesNumberTuo = 0;
                }

                string[] LocateDan = new string[GamesNumberDan];
                string[] LocateTuo = new string[GamesNumberTuo];

                string ConfirmationString = "";
                string BuyResult = "";

                if (strPlayType == "7201")    //胜平负
                {
                    if (GamesNumberDan > 0)
                    {
                        for (int i = 0; i < GamesNumberDan; i++)
                        {
                            ConfirmationString += @"(?<L" + i.ToString() + @">[\d]{1,9}[(][321]([,][321]){0,2}[)])[|]"; //方案格式：4501;[1(3)|5(3,1)|7(0)];[A2,B1]
                        }
                        ConfirmationString = ConfirmationString.Substring(0, ConfirmationString.Length - 3) + "[]][[]";
                    }

                    for (int i = GamesNumberDan; i < GamesNumberTuo + GamesNumberDan; i++)
                    {
                        ConfirmationString += @"(?<L" + i.ToString() + @">[\d]{1,9}[(][321]([,][321]){0,2}[)])[|]"; //方案格式：4501;[1(3)|5(3,1)|7(0)];[A2,B1]
                    }
                    ConfirmationString = ConfirmationString.Substring(0, ConfirmationString.Length - 3);
                }
                else if (strPlayType == "7202")   //比分
                {
                    if (GamesNumberDan > 0)
                    {
                        for (int i = 0; i < GamesNumberDan; i++)
                        {
                            ConfirmationString += @"(?<L" + i.ToString() + @">[\d]{1,9}[(][\d]{1,2}([,][\d]{1,2}){0,30}[)])[|]";
                        }
                        ConfirmationString = ConfirmationString.Substring(0, ConfirmationString.Length - 3) + "[]][[]";
                    }

                    for (int i = GamesNumberDan; i < GamesNumberTuo + GamesNumberDan; i++)
                    {
                        ConfirmationString += @"(?<L" + i.ToString() + @">[\d]{1,9}[(][\d]{1,2}([,][\d]{1,2}){0,30}[)])[|]";
                    }
                    ConfirmationString = ConfirmationString.Substring(0, ConfirmationString.Length - 3);
                }
                else if (strPlayType == "7203")   //总进球
                {
                    if (GamesNumberDan > 0)
                    {
                        for (int i = 0; i < GamesNumberDan; i++)
                        {
                            ConfirmationString += @"(?<L" + i.ToString() + @">[\d]{1,9}[(][12345678]([,][12345678]){0,8}[)])[|]";
                        }
                        ConfirmationString = ConfirmationString.Substring(0, ConfirmationString.Length - 3) + "[]][[]";
                    }

                    for (int i = GamesNumberDan; i < GamesNumberTuo + GamesNumberDan; i++)
                    {
                        ConfirmationString += @"(?<L" + i.ToString() + @">[\d]{1,9}[(][12345678]([,][12345678]){0,8}[)])[|]";
                    }
                    ConfirmationString = ConfirmationString.Substring(0, ConfirmationString.Length - 3);
                }
                else if (strPlayType == "7204")   //半全场胜平负
                {
                    if (GamesNumberDan > 0)
                    {
                        for (int i = 0; i < GamesNumberDan; i++)
                        {
                            ConfirmationString += @"(?<L" + i.ToString() + @">[\d]{1,9}[(][123456789]([,][123456789]){0,8}[)])[|]";
                        }
                        ConfirmationString = ConfirmationString.Substring(0, ConfirmationString.Length - 3) + "[]][[]";
                    }

                    for (int i = GamesNumberDan; i < GamesNumberTuo + GamesNumberDan; i++)
                    {
                        ConfirmationString += @"(?<L" + i.ToString() + @">[\d]{1,9}[(][123456789]([,][123456789]){0,8}[)])[|]";
                    }
                    ConfirmationString = ConfirmationString.Substring(0, ConfirmationString.Length - 3);
                }
                else if (strPlayType == "7205")   //单关比分
                {
                    if (GamesNumberDan > 0)
                    {
                        for (int i = 0; i < GamesNumberDan; i++)
                        {
                            ConfirmationString += @"(?<L" + i.ToString() + @">[\d]{1,9}[(][\d]{1,2}([,][\d]{1,2}){0,30}[)])[|]";
                        }
                        ConfirmationString = ConfirmationString.Substring(0, ConfirmationString.Length - 3) + "[]][[]";
                    }

                    for (int i = GamesNumberDan; i < GamesNumberTuo + GamesNumberDan; i++)
                    {
                        ConfirmationString += @"(?<L" + i.ToString() + @">[\d]{1,9}[(][\d]{1,2}([,][\d]{1,2}){0,30}[)])[|]";
                    }
                    ConfirmationString = ConfirmationString.Substring(0, ConfirmationString.Length - 3);
                }

                Regex regex = new Regex(ConfirmationString, RegexOptions.IgnoreCase | RegexOptions.Compiled);
                Match m = regex.Match(Numbers);

                for (int i = 0; i < GamesNumberDan; i++)
                {
                    LocateDan[i] = m.Groups["L" + i.ToString()].ToString().Trim();
                    if (LocateDan[i] == "")
                    {
                        return null;
                    }

                    BuyResult = LocateDan[i].Substring(LocateDan[i].IndexOf('(') + 1, (LocateDan[i].IndexOf(')') - LocateDan[i].IndexOf('(') - 1));

                    if (BuyResult.Length > 0)
                    {
                        BuyResult = FilterRepeated(BuyResult, strPlayType);

                        if (BuyResult == "")
                        {
                            return null;
                        }
                    }
                }

                for (int i = GamesNumberDan; i < GamesNumberDan + GamesNumberTuo; i++)
                {
                    LocateTuo[i - GamesNumberDan] = m.Groups["L" + i.ToString()].ToString().Trim();

                    if (LocateTuo[i - GamesNumberDan] == "")
                    {
                        return null;
                    }

                    BuyResult = LocateTuo[i - GamesNumberDan].Substring(LocateTuo[i - GamesNumberDan].IndexOf('(') + 1, (LocateTuo[i - GamesNumberDan].IndexOf(')') - LocateTuo[i - GamesNumberDan].IndexOf('(') - 1));

                    if (BuyResult.Length > 0)
                    {
                        BuyResult = FilterRepeated(BuyResult, strPlayType);

                        if (BuyResult == "")
                        {
                            return null;
                        }
                    }
                }

                #endregion

                #region 过关方式分析
                string BuyWays = Number.Trim().Split(';')[2].ToString().Substring(1, Number.Trim().Split(';')[2].ToString().Length - 1).Substring(0, Number.Trim().Split(';')[2].ToString().Length - 2).ToString().Trim();
                if (BuyWays == "")
                {
                    return null;
                }

                int WaysNumber = BuyWays.Split(',').Length;
                string[] LocateWays = new string[WaysNumber];
                string WaysResult = "";
                string BuyWaysResult = "";
                int TempWaysMultiples = 0;

                ConfirmationString = "";

                for (int j = 0; j < WaysNumber; j++)
                {
                    ConfirmationString += @"(?<L" + j.ToString() + @">[123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ]{1,1000}[\d]{1,5})[,]"; //方案格式：4501;[1(3)|5(3,1)|7(0)];[A2,B1]
                }
                ConfirmationString = ConfirmationString.Substring(0, ConfirmationString.Length - 3);

                Regex regexWays = new Regex(ConfirmationString, RegexOptions.IgnoreCase | RegexOptions.Compiled);
                Match mWays = regexWays.Match(BuyWays);
                for (int k = 0; k < WaysNumber; k++)
                {
                    LocateWays[k] = mWays.Groups["L" + k.ToString()].ToString().Trim();
                    if (LocateWays[k] == "")
                    {
                        return null;
                    }

                    BuyResult = LocateWays[k].Substring(2, (LocateWays[k].Length - 2));

                    if (BuyResult.Length > 0)
                    {
                        try
                        {
                            TempWaysMultiples = Convert.ToInt32(BuyResult);
                        }
                        catch
                        {
                            TempWaysMultiples = 0;
                        }

                        if (TempWaysMultiples > 0)
                        {
                            BuyWaysResult = BuyResult;
                        }
                        else
                        {
                            BuyWaysResult = "";
                        }
                    }
                    if (BuyWaysResult.Length > 0)
                    {
                        WaysResult += LocateWays[k].Substring(0, 2).ToUpper() + BuyWaysResult.ToString() + ",";
                    }
                }

                if (WaysResult.EndsWith(","))
                {
                    WaysResult = WaysResult.Substring(0, WaysResult.Length - 1);
                }

                if (WaysResult.Length < 2)   //至少有一种过关方式 例如：11：表示单关，1倍
                {
                    return null;
                }

                int Len = 0;

                if (LocateDan.Length > 0)
                {
                    Len = Shove._Convert.StrToInt(Number.Trim().Split(';')[3].Substring(1, 1), LocateDan.Length);

                    if (Len > LocateDan.Length)
                    {
                        return null;
                    }
                }


                //复式变单式，并取得购买注数和相应的倍数
                ArrayList al = new ArrayList();

                WaysNumber = 0; //将原来的购买方式倍数清0
                WaysNumber = WaysResult.Split(',').Length;  //重新获取分析过后的购买方式及倍数

                int Num = 0;
                string[] NumberDan = null;
                string[] NumberTuo = null;

                for (int i = 0; i < WaysNumber; i++)
                {
                    if ("A0".IndexOf(LocateWays[i].Substring(0, 2).ToUpper()) >= 0)
                    {
                        al.Add(Number);
                    }
                    else if ("AA".IndexOf(LocateWays[i].Substring(0, 2).ToUpper()) >= 0 && Len < 2)
                    {
                        Num = LocateDan.Length > 2 ? 2 : LocateDan.Length;

                        for (int j = Len; j < Num + 1; j++)
                        {
                            NumberDan = NumberList(LocateDan, j);
                            NumberTuo = NumberList(LocateTuo, 2 - j);

                            if (NumberDan != null && NumberDan.Length > 0)
                            {
                                for (int p = 0; p < NumberDan.Length; p++)
                                {
                                    for (int x = 0; x < NumberTuo.Length; x++)
                                    {
                                        al.Add(strPlayType + ";" + "[" + NumberDan[p] + "|" + NumberTuo[x] + "]" + ";[" + LocateWays[i] + "]");
                                    }
                                }
                            }
                            else
                            {
                                for (int x = 0; x < NumberTuo.Length; x++)
                                {
                                    al.Add(strPlayType + ";" + "[" + NumberTuo[x] + "]" + ";[" + LocateWays[i] + "]");
                                }
                            }
                        }
                    }
                    else if ("AB AC AD ".IndexOf(LocateWays[i].Substring(0, 2).ToUpper()) >= 0 && Len < 3)
                    {
                        Num = LocateDan.Length > 3 ? 3 : LocateDan.Length;

                        for (int j = Len; j < Num + 1; j++)
                        {
                            NumberDan = NumberList(LocateDan, j);
                            NumberTuo = NumberList(LocateTuo, 3 - j);

                            if (NumberDan != null && NumberDan.Length > 0)
                            {
                                for (int p = 0; p < NumberDan.Length; p++)
                                {
                                    for (int x = 0; x < NumberTuo.Length; x++)
                                    {
                                        al.Add(strPlayType + ";" + "[" + NumberDan[p] + "|" + NumberTuo[x] + "]" + ";[" + LocateWays[i] + "]");
                                    }
                                }
                            }
                            else
                            {
                                for (int x = 0; x < NumberTuo.Length; x++)
                                {
                                    al.Add(strPlayType + ";" + "[" + NumberTuo[x] + "]" + ";[" + LocateWays[i] + "]");
                                }
                            }
                        }
                    }
                    else if ("AE AF AG AH AI ".IndexOf(LocateWays[i].Substring(0, 2).ToUpper()) >= 0 && Len < 4)
                    {
                        Num = LocateDan.Length > 4 ? 4 : LocateDan.Length;

                        for (int j = Len; j < Num + 1; j++)
                        {
                            NumberDan = NumberList(LocateDan, j);
                            NumberTuo = NumberList(LocateTuo, 4 - j);

                            if (NumberDan != null && NumberDan.Length > 0)
                            {
                                for (int p = 0; p < NumberDan.Length; p++)
                                {
                                    for (int x = 0; x < NumberTuo.Length; x++)
                                    {
                                        al.Add(strPlayType + ";" + "[" + NumberDan[p] + "|" + NumberTuo[x] + "]" + ";[" + LocateWays[i] + "]");
                                    }
                                }
                            }
                            else
                            {
                                for (int x = 0; x < NumberTuo.Length; x++)
                                {
                                    al.Add(strPlayType + ";" + "[" + NumberTuo[x] + "]" + ";[" + LocateWays[i] + "]");
                                }
                            }
                        }
                    }
                    else if ("AJ AK AL AM AN AO AP ".IndexOf(LocateWays[i].Substring(0, 2).ToUpper()) >= 0 && Len < 5)
                    {
                        Num = LocateDan.Length > 5 ? 5 : LocateDan.Length;

                        for (int j = Len; j < Num + 1; j++)
                        {
                            NumberDan = NumberList(LocateDan, j);
                            NumberTuo = NumberList(LocateTuo, 5 - j);

                            if (NumberDan != null && NumberDan.Length > 0)
                            {
                                for (int p = 0; p < NumberDan.Length; p++)
                                {
                                    for (int x = 0; x < NumberTuo.Length; x++)
                                    {
                                        al.Add(strPlayType + ";" + "[" + NumberDan[p] + "|" + NumberTuo[x] + "]" + ";[" + LocateWays[i] + "]");
                                    }
                                }
                            }
                            else
                            {
                                for (int x = 0; x < NumberTuo.Length; x++)
                                {
                                    al.Add(strPlayType + ";" + "[" + NumberTuo[x] + "]" + ";[" + LocateWays[i] + "]");
                                }
                            }
                        }
                    }
                    else if ("AQ AR AS AT AU AV AW AX AY AZ".IndexOf(LocateWays[i].Substring(0, 2).ToUpper()) >= 0 && Len < 6)
                    {
                        Num = LocateDan.Length > 6 ? 6 : LocateDan.Length;

                        for (int j = Len; j < Num + 1; j++)
                        {
                            NumberDan = NumberList(LocateDan, j);
                            NumberTuo = NumberList(LocateTuo, 6 - j);

                            if (NumberDan != null && NumberDan.Length > 0)
                            {
                                for (int p = 0; p < NumberDan.Length; p++)
                                {
                                    for (int x = 0; x < NumberTuo.Length; x++)
                                    {
                                        al.Add(strPlayType + ";" + "[" + NumberDan[p] + "|" + NumberTuo[x] + "]" + ";[" + LocateWays[i] + "]");
                                    }
                                }
                            }
                            else
                            {
                                for (int x = 0; x < NumberTuo.Length; x++)
                                {
                                    al.Add(strPlayType + ";" + "[" + NumberTuo[x] + "]" + ";[" + LocateWays[i] + "]");
                                }
                            }
                        }
                    }
                    else if ("BA BB BC BD BE BF".IndexOf(LocateWays[i].Substring(0, 2).ToUpper()) >= 0 && Len < 7)
                    {
                        Num = LocateDan.Length > 7 ? 7 : LocateDan.Length;

                        for (int j = Len; j < Num + 1; j++)
                        {
                            NumberDan = NumberList(LocateDan, j);
                            NumberTuo = NumberList(LocateTuo, 7 - j);

                            if (NumberDan != null && NumberDan.Length > 0)
                            {
                                for (int p = 0; p < NumberDan.Length; p++)
                                {
                                    for (int x = 0; x < NumberTuo.Length; x++)
                                    {
                                        al.Add(strPlayType + ";" + "[" + NumberDan[p] + "|" + NumberTuo[x] + "]" + ";[" + LocateWays[i] + "]");
                                    }
                                }
                            }
                            else
                            {
                                for (int x = 0; x < NumberTuo.Length; x++)
                                {
                                    al.Add(strPlayType + ";" + "[" + NumberTuo[x] + "]" + ";[" + LocateWays[i] + "]");
                                }
                            }
                        }
                    }
                    else if ("BG BH BI BJ BK BL BM".IndexOf(LocateWays[i].Substring(0, 2).ToUpper()) >= 0 && Len < 8)
                    {
                        Num = LocateDan.Length > 8 ? 8 : LocateDan.Length;

                        for (int j = Len; j < Num + 1; j++)
                        {
                            NumberDan = NumberList(LocateDan, j);
                            NumberTuo = NumberList(LocateTuo, 8 - j);

                            if (NumberDan != null && NumberDan.Length > 0)
                            {
                                for (int p = 0; p < NumberDan.Length; p++)
                                {
                                    for (int x = 0; x < NumberTuo.Length; x++)
                                    {
                                        al.Add(strPlayType + ";" + "[" + NumberDan[p] + "|" + NumberTuo[x] + "]" + ";[" + LocateWays[i] + "]");
                                    }
                                }
                            }
                            else
                            {
                                for (int x = 0; x < NumberTuo.Length; x++)
                                {
                                    al.Add(strPlayType + ";" + "[" + NumberTuo[x] + "]" + ";[" + LocateWays[i] + "]");
                                }
                            }
                        }
                    }

                }

                #endregion

                //复式变单式，并取得购买注数和相应的倍数
                ArrayList all = new ArrayList();

                int GamesNumber = 0;
                string strCanonicalNumber = "";

                for (int p = 0; p < al.Count; p++)
                {
                    GamesNumber = 0;
                    strCanonicalNumber = al[p].ToString().Split(';')[1].Substring(1, al[p].ToString().Split(';')[1].Length - 2);
                    WaysResult = al[p].ToString().Split(';')[2].Substring(1, al[p].ToString().Split(';')[2].Length - 2);

                    WaysNumber = 0; //将原来的购买方式倍数清0

                    GamesNumber = strCanonicalNumber.Split('|').Length;    //重新获取分析过后的比赛场次
                    WaysNumber = WaysResult.Split(',').Length;  //重新获取分析过后的购买方式及倍数
                    string[] Locate = new string[GamesNumber];

                    string[] LocateBuyResult = new string[GamesNumber];
                    string[] Screenings = new string[GamesNumber];

                    for (int i = 0; i < GamesNumber; i++)
                    {
                        Locate[i] = strCanonicalNumber.Split('|')[i].ToString();

                        LocateBuyResult[i] = Locate[i].Substring(Locate[i].IndexOf('(') + 1, (Locate[i].IndexOf(')') - Locate[i].IndexOf('(') - 1));

                        Screenings[i] = Locate[i].Substring(0, Locate[i].IndexOf('('));
                    }

                    string[] LocateWaysAndMultiples = new string[WaysNumber];   //购买方式及倍数
                    string[] LocateWaysType = new string[WaysNumber];   //购买方式
                    string[] WaysMultiples = new string[WaysNumber];    //购买倍数
                    string[] sg = null;

                    for (int i = 0; i < WaysNumber; i++)
                    {
                        LocateWaysAndMultiples[i] = WaysResult.Split(',')[i].ToString();
                        LocateWaysType[i] = LocateWaysAndMultiples[i].Substring(0, 2);
                        WaysMultiples[i] = LocateWaysAndMultiples[i].Substring(2, (LocateWaysAndMultiples[i].Length - 2));

                        #region     获取方案某一个购买方式的购买注数和倍数
                        switch (LocateWaysType[i])
                        {
                            case "A0":   //单式买注数和对应倍数
                                sg = getAll1G(GamesNumber, Locate, LocateBuyResult, Screenings, int.Parse(WaysMultiples[i].ToString()));
                                for (int j = 0; j < sg.Length; j++)
                                {
                                    all.Add(strPlayType + ";" + "[" + sg[j].ToString().Split(';')[0] + "]" + ";[" + LocateWaysAndMultiples[i] + "]");
                                }
                                break;

                            case "AA":   //2串1购买注数和对应倍数
                                sg = getAll2G(GamesNumber, Locate, LocateBuyResult, Screenings, int.Parse(WaysMultiples[i].ToString()));
                                for (int j = 0; j < sg.Length; j++)
                                {
                                    all.Add(strPlayType + ";" + "[" + sg[j].ToString().Split(';')[0] + "]" + ";[" + LocateWaysAndMultiples[i] + "]");
                                }
                                break;
                            case "AB":   //3串1购买注数和对应倍数
                                sg = getAll3G(GamesNumber, Locate, LocateBuyResult, Screenings, int.Parse(WaysMultiples[i].ToString()));
                                for (int j = 0; j < sg.Length; j++)
                                {
                                    all.Add(strPlayType + ";" + "[" + sg[j].ToString().Split(';')[0] + "]" + ";[" + LocateWaysAndMultiples[i] + "]");
                                }
                                break;
                            case "AC":   //3串3购买注数和对应倍数
                                sg = getAll2G(GamesNumber, Locate, LocateBuyResult, Screenings, int.Parse(WaysMultiples[i].ToString()));
                                for (int j = 0; j < sg.Length; j++)
                                {
                                    all.Add(strPlayType + ";" + "[" + sg[j].ToString().Split(';')[0] + "]" + ";[" + LocateWaysAndMultiples[i] + "]");
                                }
                                break;
                            case "AD":   //3串4购买注数和对应倍数
                                sg = getAll2G(GamesNumber, Locate, LocateBuyResult, Screenings, int.Parse(WaysMultiples[i].ToString()));
                                for (int j = 0; j < sg.Length; j++)
                                {
                                    all.Add(strPlayType + ";" + "[" + sg[j].ToString().Split(';')[0] + "]" + ";[" + LocateWaysAndMultiples[i] + "]");
                                }

                                sg = getAll3G(GamesNumber, Locate, LocateBuyResult, Screenings, int.Parse(WaysMultiples[i].ToString()));
                                for (int j = 0; j < sg.Length; j++)
                                {
                                    all.Add(strPlayType + ";" + "[" + sg[j].ToString().Split(';')[0] + "]" + ";[" + LocateWaysAndMultiples[i] + "]");
                                }
                                break;
                            case "AE":    //4串1购买注数和对应倍数
                                sg = getAll4G(GamesNumber, Locate, LocateBuyResult, Screenings, int.Parse(WaysMultiples[i].ToString()));
                                for (int j = 0; j < sg.Length; j++)
                                {
                                    all.Add(strPlayType + ";" + "[" + sg[j].ToString().Split(';')[0] + "]" + ";[" + LocateWaysAndMultiples[i] + "]");
                                }
                                break;
                            case "AF":   //4串4购买注数和对应倍数
                                sg = getAll3G(GamesNumber, Locate, LocateBuyResult, Screenings, int.Parse(WaysMultiples[i].ToString()));
                                for (int j = 0; j < sg.Length; j++)
                                {
                                    all.Add(strPlayType + ";" + "[" + sg[j].ToString().Split(';')[0] + "]" + ";[" + LocateWaysAndMultiples[i] + "]");
                                }
                                break;
                            case "AG":   //4串5购买注数和对应倍数
                                sg = getAll3G(GamesNumber, Locate, LocateBuyResult, Screenings, int.Parse(WaysMultiples[i].ToString()));
                                for (int j = 0; j < sg.Length; j++)
                                {
                                    all.Add(strPlayType + ";" + "[" + sg[j].ToString().Split(';')[0] + "]" + ";[" + LocateWaysAndMultiples[i] + "]");
                                }

                                sg = getAll4G(GamesNumber, Locate, LocateBuyResult, Screenings, int.Parse(WaysMultiples[i].ToString()));
                                for (int j = 0; j < sg.Length; j++)
                                {
                                    all.Add(strPlayType + ";" + "[" + sg[j].ToString().Split(';')[0] + "]" + ";[" + LocateWaysAndMultiples[i] + "]");
                                }
                                break;
                            case "AH":   //4串6购买注数和对应倍数
                                sg = getAll2G(GamesNumber, Locate, LocateBuyResult, Screenings, int.Parse(WaysMultiples[i].ToString()));
                                for (int j = 0; j < sg.Length; j++)
                                {
                                    all.Add(strPlayType + ";" + "[" + sg[j].ToString().Split(';')[0] + "]" + ";[" + LocateWaysAndMultiples[i] + "]");
                                }
                                break;
                            case "AI":   //4串11购买注数和对应倍数
                                sg = getAll2G(GamesNumber, Locate, LocateBuyResult, Screenings, int.Parse(WaysMultiples[i].ToString()));
                                for (int j = 0; j < sg.Length; j++)
                                {
                                    all.Add(strPlayType + ";" + "[" + sg[j].ToString().Split(';')[0] + "]" + ";[" + LocateWaysAndMultiples[i] + "]");
                                }
                                sg = getAll3G(GamesNumber, Locate, LocateBuyResult, Screenings, int.Parse(WaysMultiples[i].ToString()));
                                for (int j = 0; j < sg.Length; j++)
                                {
                                    all.Add(strPlayType + ";" + "[" + sg[j].ToString().Split(';')[0] + "]" + ";[" + LocateWaysAndMultiples[i] + "]");
                                }

                                sg = getAll4G(GamesNumber, Locate, LocateBuyResult, Screenings, int.Parse(WaysMultiples[i].ToString()));
                                for (int j = 0; j < sg.Length; j++)
                                {
                                    all.Add(strPlayType + ";" + "[" + sg[j].ToString().Split(';')[0] + "]" + ";[" + LocateWaysAndMultiples[i] + "]");
                                }
                                break;
                            case "AJ":   //5串1购买注数和对应倍数
                                sg = getAll5G(GamesNumber, Locate, LocateBuyResult, Screenings, int.Parse(WaysMultiples[i].ToString()));
                                for (int j = 0; j < sg.Length; j++)
                                {
                                    all.Add(strPlayType + ";" + "[" + sg[j].ToString().Split(';')[0] + "]" + ";[" + LocateWaysAndMultiples[i] + "]");
                                }
                                break;
                            case "AK":   //5串5购买注数和对应倍数
                                sg = getAll4G(GamesNumber, Locate, LocateBuyResult, Screenings, int.Parse(WaysMultiples[i].ToString()));
                                for (int j = 0; j < sg.Length; j++)
                                {
                                    all.Add(strPlayType + ";" + "[" + sg[j].ToString().Split(';')[0] + "]" + ";[" + LocateWaysAndMultiples[i] + "]");
                                }
                                break;
                            case "AL":   //5串6购买注数和对应倍数
                                sg = getAll4G(GamesNumber, Locate, LocateBuyResult, Screenings, int.Parse(WaysMultiples[i].ToString()));
                                for (int j = 0; j < sg.Length; j++)
                                {
                                    all.Add(strPlayType + ";" + "[" + sg[j].ToString().Split(';')[0] + "]" + ";[" + LocateWaysAndMultiples[i] + "]");
                                }

                                sg = getAll5G(GamesNumber, Locate, LocateBuyResult, Screenings, int.Parse(WaysMultiples[i].ToString()));
                                for (int j = 0; j < sg.Length; j++)
                                {
                                    all.Add(strPlayType + ";" + "[" + sg[j].ToString().Split(';')[0] + "]" + ";[" + LocateWaysAndMultiples[i] + "]");
                                }
                                break;
                            case "AM":   //5串10购买注数和对应倍数
                                sg = getAll2G(GamesNumber, Locate, LocateBuyResult, Screenings, int.Parse(WaysMultiples[i].ToString()));
                                for (int j = 0; j < sg.Length; j++)
                                {
                                    all.Add(strPlayType + ";" + "[" + sg[j].ToString().Split(';')[0] + "]" + ";[" + LocateWaysAndMultiples[i] + "]");
                                }
                                break;
                            case "AN":   //5串16购买注数和对应倍数
                                sg = getAll3G(GamesNumber, Locate, LocateBuyResult, Screenings, int.Parse(WaysMultiples[i].ToString()));
                                for (int j = 0; j < sg.Length; j++)
                                {
                                    all.Add(strPlayType + ";" + "[" + sg[j].ToString().Split(';')[0] + "]" + ";[" + LocateWaysAndMultiples[i] + "]");
                                }

                                sg = getAll4G(GamesNumber, Locate, LocateBuyResult, Screenings, int.Parse(WaysMultiples[i].ToString()));
                                for (int j = 0; j < sg.Length; j++)
                                {
                                    all.Add(strPlayType + ";" + "[" + sg[j].ToString().Split(';')[0] + "]" + ";[" + LocateWaysAndMultiples[i] + "]");
                                }

                                sg = getAll5G(GamesNumber, Locate, LocateBuyResult, Screenings, int.Parse(WaysMultiples[i].ToString()));
                                for (int j = 0; j < sg.Length; j++)
                                {
                                    all.Add(strPlayType + ";" + "[" + sg[j].ToString().Split(';')[0] + "]" + ";[" + LocateWaysAndMultiples[i] + "]");
                                }
                                break;
                            case "AO":   //5串20购买注数和对应倍数
                                sg = getAll2G(GamesNumber, Locate, LocateBuyResult, Screenings, int.Parse(WaysMultiples[i].ToString()));
                                for (int j = 0; j < sg.Length; j++)
                                {
                                    all.Add(strPlayType + ";" + "[" + sg[j].ToString().Split(';')[0] + "]" + ";[" + LocateWaysAndMultiples[i] + "]");
                                }

                                sg = getAll3G(GamesNumber, Locate, LocateBuyResult, Screenings, int.Parse(WaysMultiples[i].ToString()));
                                for (int j = 0; j < sg.Length; j++)
                                {
                                    all.Add(strPlayType + ";" + "[" + sg[j].ToString().Split(';')[0] + "]" + ";[" + LocateWaysAndMultiples[i] + "]");
                                }
                                break;
                            case "AP":   //5串26购买注数和对应倍数
                                sg = getAll2G(GamesNumber, Locate, LocateBuyResult, Screenings, int.Parse(WaysMultiples[i].ToString()));
                                for (int j = 0; j < sg.Length; j++)
                                {
                                    all.Add(strPlayType + ";" + "[" + sg[j].ToString().Split(';')[0] + "]" + ";[" + LocateWaysAndMultiples[i] + "]");
                                }

                                sg = getAll3G(GamesNumber, Locate, LocateBuyResult, Screenings, int.Parse(WaysMultiples[i].ToString()));
                                for (int j = 0; j < sg.Length; j++)
                                {
                                    all.Add(strPlayType + ";" + "[" + sg[j].ToString().Split(';')[0] + "]" + ";[" + LocateWaysAndMultiples[i] + "]");
                                }

                                sg = getAll4G(GamesNumber, Locate, LocateBuyResult, Screenings, int.Parse(WaysMultiples[i].ToString()));
                                for (int j = 0; j < sg.Length; j++)
                                {
                                    all.Add(strPlayType + ";" + "[" + sg[j].ToString().Split(';')[0] + "]" + ";[" + LocateWaysAndMultiples[i] + "]");
                                }

                                sg = getAll5G(GamesNumber, Locate, LocateBuyResult, Screenings, int.Parse(WaysMultiples[i].ToString()));
                                for (int j = 0; j < sg.Length; j++)
                                {
                                    all.Add(strPlayType + ";" + "[" + sg[j].ToString().Split(';')[0] + "]" + ";[" + LocateWaysAndMultiples[i] + "]");
                                }
                                break;
                            case "AQ":   //6串1购买注数和对应倍数
                                sg = getAll6G(GamesNumber, Locate, LocateBuyResult, Screenings, int.Parse(WaysMultiples[i].ToString()));
                                for (int j = 0; j < sg.Length; j++)
                                {
                                    all.Add(strPlayType + ";" + "[" + sg[j].ToString().Split(';')[0] + "]" + ";[" + LocateWaysAndMultiples[i] + "]");
                                }
                                break;
                            case "AR":   //6串6购买注数和对应倍数
                                sg = getAll5G(GamesNumber, Locate, LocateBuyResult, Screenings, int.Parse(WaysMultiples[i].ToString()));
                                for (int j = 0; j < sg.Length; j++)
                                {
                                    all.Add(strPlayType + ";" + "[" + sg[j].ToString().Split(';')[0] + "]" + ";[" + LocateWaysAndMultiples[i] + "]");
                                }
                                break;
                            case "AS":   //6串7购买注数和对应倍数
                                sg = getAll5G(GamesNumber, Locate, LocateBuyResult, Screenings, int.Parse(WaysMultiples[i].ToString()));
                                for (int j = 0; j < sg.Length; j++)
                                {
                                    all.Add(strPlayType + ";" + "[" + sg[j].ToString().Split(';')[0] + "]" + ";[" + LocateWaysAndMultiples[i] + "]");
                                }

                                sg = getAll6G(GamesNumber, Locate, LocateBuyResult, Screenings, int.Parse(WaysMultiples[i].ToString()));
                                for (int j = 0; j < sg.Length; j++)
                                {
                                    all.Add(strPlayType + ";" + "[" + sg[j].ToString().Split(';')[0] + "]" + ";[" + LocateWaysAndMultiples[i] + "]");
                                }
                                break;
                            case "AT":   //6串15购买注数和对应倍数
                                sg = getAll2G(GamesNumber, Locate, LocateBuyResult, Screenings, int.Parse(WaysMultiples[i].ToString()));
                                for (int j = 0; j < sg.Length; j++)
                                {
                                    all.Add(strPlayType + ";" + "[" + sg[j].ToString().Split(';')[0] + "]" + ";[" + LocateWaysAndMultiples[i] + "]");
                                }
                                break;
                            case "AU":   //6串20购买注数和对应倍数
                                sg = getAll3G(GamesNumber, Locate, LocateBuyResult, Screenings, int.Parse(WaysMultiples[i].ToString()));
                                for (int j = 0; j < sg.Length; j++)
                                {
                                    all.Add(strPlayType + ";" + "[" + sg[j].ToString().Split(';')[0] + "]" + ";[" + LocateWaysAndMultiples[i] + "]");
                                }
                                break;
                            case "AV":   //6串22购买注数和对应倍数
                                sg = getAll4G(GamesNumber, Locate, LocateBuyResult, Screenings, int.Parse(WaysMultiples[i].ToString()));
                                for (int j = 0; j < sg.Length; j++)
                                {
                                    all.Add(strPlayType + ";" + "[" + sg[j].ToString().Split(';')[0] + "]" + ";[" + LocateWaysAndMultiples[i] + "]");
                                }

                                sg = getAll5G(GamesNumber, Locate, LocateBuyResult, Screenings, int.Parse(WaysMultiples[i].ToString()));
                                for (int j = 0; j < sg.Length; j++)
                                {
                                    all.Add(strPlayType + ";" + "[" + sg[j].ToString().Split(';')[0] + "]" + ";[" + LocateWaysAndMultiples[i] + "]");
                                }

                                sg = getAll6G(GamesNumber, Locate, LocateBuyResult, Screenings, int.Parse(WaysMultiples[i].ToString()));
                                for (int j = 0; j < sg.Length; j++)
                                {
                                    all.Add(strPlayType + ";" + "[" + sg[j].ToString().Split(';')[0] + "]" + ";[" + LocateWaysAndMultiples[i] + "]");
                                }
                                break;
                            case "AW":   //6串35购买注数和对应倍数
                                sg = getAll2G(GamesNumber, Locate, LocateBuyResult, Screenings, int.Parse(WaysMultiples[i].ToString()));
                                for (int j = 0; j < sg.Length; j++)
                                {
                                    all.Add(strPlayType + ";" + "[" + sg[j].ToString().Split(';')[0] + "]" + ";[" + LocateWaysAndMultiples[i] + "]");
                                }

                                sg = getAll3G(GamesNumber, Locate, LocateBuyResult, Screenings, int.Parse(WaysMultiples[i].ToString()));
                                for (int j = 0; j < sg.Length; j++)
                                {
                                    all.Add(strPlayType + ";" + "[" + sg[j].ToString().Split(';')[0] + "]" + ";[" + LocateWaysAndMultiples[i] + "]");
                                }
                                break;
                            case "AX":   //6串42购买注数和对应倍数
                                sg = getAll3G(GamesNumber, Locate, LocateBuyResult, Screenings, int.Parse(WaysMultiples[i].ToString()));
                                for (int j = 0; j < sg.Length; j++)
                                {
                                    all.Add(strPlayType + ";" + "[" + sg[j].ToString().Split(';')[0] + "]" + ";[" + LocateWaysAndMultiples[i] + "]");
                                }

                                sg = getAll4G(GamesNumber, Locate, LocateBuyResult, Screenings, int.Parse(WaysMultiples[i].ToString()));
                                for (int j = 0; j < sg.Length; j++)
                                {
                                    all.Add(strPlayType + ";" + "[" + sg[j].ToString().Split(';')[0] + "]" + ";[" + LocateWaysAndMultiples[i] + "]");
                                }

                                sg = getAll5G(GamesNumber, Locate, LocateBuyResult, Screenings, int.Parse(WaysMultiples[i].ToString()));
                                for (int j = 0; j < sg.Length; j++)
                                {
                                    all.Add(strPlayType + ";" + "[" + sg[j].ToString().Split(';')[0] + "]" + ";[" + LocateWaysAndMultiples[i] + "]");
                                }

                                sg = getAll6G(GamesNumber, Locate, LocateBuyResult, Screenings, int.Parse(WaysMultiples[i].ToString()));
                                for (int j = 0; j < sg.Length; j++)
                                {
                                    all.Add(strPlayType + ";" + "[" + sg[j].ToString().Split(';')[0] + "]" + ";[" + LocateWaysAndMultiples[i] + "]");
                                }
                                break;
                            case "AY":   //6串50购买注数和对应倍数
                                sg = getAll2G(GamesNumber, Locate, LocateBuyResult, Screenings, int.Parse(WaysMultiples[i].ToString()));
                                for (int j = 0; j < sg.Length; j++)
                                {
                                    all.Add(strPlayType + ";" + "[" + sg[j].ToString().Split(';')[0] + "]" + ";[" + LocateWaysAndMultiples[i] + "]");
                                }

                                sg = getAll3G(GamesNumber, Locate, LocateBuyResult, Screenings, int.Parse(WaysMultiples[i].ToString()));
                                for (int j = 0; j < sg.Length; j++)
                                {
                                    all.Add(strPlayType + ";" + "[" + sg[j].ToString().Split(';')[0] + "]" + ";[" + LocateWaysAndMultiples[i] + "]");
                                }

                                sg = getAll4G(GamesNumber, Locate, LocateBuyResult, Screenings, int.Parse(WaysMultiples[i].ToString()));
                                for (int j = 0; j < sg.Length; j++)
                                {
                                    all.Add(strPlayType + ";" + "[" + sg[j].ToString().Split(';')[0] + "]" + ";[" + LocateWaysAndMultiples[i] + "]");
                                }
                                break;
                            case "AZ":   //6串57购买注数和对应倍数
                                sg = getAll2G(GamesNumber, Locate, LocateBuyResult, Screenings, int.Parse(WaysMultiples[i].ToString()));
                                for (int j = 0; j < sg.Length; j++)
                                {
                                    all.Add(strPlayType + ";" + "[" + sg[j].ToString().Split(';')[0] + "]" + ";[" + LocateWaysAndMultiples[i] + "]");
                                }

                                sg = getAll3G(GamesNumber, Locate, LocateBuyResult, Screenings, int.Parse(WaysMultiples[i].ToString()));
                                for (int j = 0; j < sg.Length; j++)
                                {
                                    all.Add(strPlayType + ";" + "[" + sg[j].ToString().Split(';')[0] + "]" + ";[" + LocateWaysAndMultiples[i] + "]");
                                }

                                sg = getAll4G(GamesNumber, Locate, LocateBuyResult, Screenings, int.Parse(WaysMultiples[i].ToString()));
                                for (int j = 0; j < sg.Length; j++)
                                {
                                    all.Add(strPlayType + ";" + "[" + sg[j].ToString().Split(';')[0] + "]" + ";[" + LocateWaysAndMultiples[i] + "]");
                                }

                                sg = getAll5G(GamesNumber, Locate, LocateBuyResult, Screenings, int.Parse(WaysMultiples[i].ToString()));
                                for (int j = 0; j < sg.Length; j++)
                                {
                                    all.Add(strPlayType + ";" + "[" + sg[j].ToString().Split(';')[0] + "]" + ";[" + LocateWaysAndMultiples[i] + "]");
                                }

                                sg = getAll6G(GamesNumber, Locate, LocateBuyResult, Screenings, int.Parse(WaysMultiples[i].ToString()));
                                for (int j = 0; j < sg.Length; j++)
                                {
                                    all.Add(strPlayType + ";" + "[" + sg[j].ToString().Split(';')[0] + "]" + ";[" + LocateWaysAndMultiples[i] + "]");
                                }
                                break;
                            case "BA":   //7串1购买注数和对应倍数
                                sg = getAll7G(GamesNumber, Locate, LocateBuyResult, Screenings, int.Parse(WaysMultiples[i].ToString()));
                                for (int j = 0; j < sg.Length; j++)
                                {
                                    all.Add(strPlayType + ";" + "[" + sg[j].ToString().Split(';')[0] + "]" + ";[" + LocateWaysAndMultiples[i] + "]");
                                }
                                break;
                            case "BB":   //7串7购买注数和对应倍数
                                sg = getAll6G(GamesNumber, Locate, LocateBuyResult, Screenings, int.Parse(WaysMultiples[i].ToString()));
                                for (int j = 0; j < sg.Length; j++)
                                {
                                    all.Add(strPlayType + ";" + "[" + sg[j].ToString().Split(';')[0] + "]" + ";[" + LocateWaysAndMultiples[i] + "]");
                                }
                                break;
                            case "BC":   //7串8购买注数和对应倍数
                                sg = getAll6G(GamesNumber, Locate, LocateBuyResult, Screenings, int.Parse(WaysMultiples[i].ToString()));
                                for (int j = 0; j < sg.Length; j++)
                                {
                                    all.Add(strPlayType + ";" + "[" + sg[j].ToString().Split(';')[0] + "]" + ";[" + LocateWaysAndMultiples[i] + "]");
                                }

                                sg = getAll7G(GamesNumber, Locate, LocateBuyResult, Screenings, int.Parse(WaysMultiples[i].ToString()));
                                for (int j = 0; j < sg.Length; j++)
                                {
                                    all.Add(strPlayType + ";" + "[" + sg[j].ToString().Split(';')[0] + "]" + ";[" + LocateWaysAndMultiples[i] + "]");
                                }
                                break;
                            case "BD":   //7串21购买注数和对应倍数
                                sg = getAll5G(GamesNumber, Locate, LocateBuyResult, Screenings, int.Parse(WaysMultiples[i].ToString()));
                                for (int j = 0; j < sg.Length; j++)
                                {
                                    all.Add(strPlayType + ";" + "[" + sg[j].ToString().Split(';')[0] + "]" + ";[" + LocateWaysAndMultiples[i] + "]");
                                }
                                break;
                            case "BE":   //7串35购买注数和对应倍数
                                sg = getAll4G(GamesNumber, Locate, LocateBuyResult, Screenings, int.Parse(WaysMultiples[i].ToString()));
                                for (int j = 0; j < sg.Length; j++)
                                {
                                    all.Add(strPlayType + ";" + "[" + sg[j].ToString().Split(';')[0] + "]" + ";[" + LocateWaysAndMultiples[i] + "]");
                                }
                                break;
                            case "BF":   //7串120购买注数和对应倍数
                                sg = getAll2G(GamesNumber, Locate, LocateBuyResult, Screenings, int.Parse(WaysMultiples[i].ToString()));
                                for (int j = 0; j < sg.Length; j++)
                                {
                                    all.Add(strPlayType + ";" + "[" + sg[j].ToString().Split(';')[0] + "]" + ";[" + LocateWaysAndMultiples[i] + "]");
                                }

                                sg = getAll3G(GamesNumber, Locate, LocateBuyResult, Screenings, int.Parse(WaysMultiples[i].ToString()));
                                for (int j = 0; j < sg.Length; j++)
                                {
                                    all.Add(strPlayType + ";" + "[" + sg[j].ToString().Split(';')[0] + "]" + ";[" + LocateWaysAndMultiples[i] + "]");
                                }

                                sg = getAll4G(GamesNumber, Locate, LocateBuyResult, Screenings, int.Parse(WaysMultiples[i].ToString()));
                                for (int j = 0; j < sg.Length; j++)
                                {
                                    all.Add(strPlayType + ";" + "[" + sg[j].ToString().Split(';')[0] + "]" + ";[" + LocateWaysAndMultiples[i] + "]");
                                }

                                sg = getAll5G(GamesNumber, Locate, LocateBuyResult, Screenings, int.Parse(WaysMultiples[i].ToString()));
                                for (int j = 0; j < sg.Length; j++)
                                {
                                    all.Add(strPlayType + ";" + "[" + sg[j].ToString().Split(';')[0] + "]" + ";[" + LocateWaysAndMultiples[i] + "]");
                                }

                                sg = getAll6G(GamesNumber, Locate, LocateBuyResult, Screenings, int.Parse(WaysMultiples[i].ToString()));
                                for (int j = 0; j < sg.Length; j++)
                                {
                                    all.Add(strPlayType + ";" + "[" + sg[j].ToString().Split(';')[0] + "]" + ";[" + LocateWaysAndMultiples[i] + "]");
                                }

                                sg = getAll7G(GamesNumber, Locate, LocateBuyResult, Screenings, int.Parse(WaysMultiples[i].ToString()));
                                for (int j = 0; j < sg.Length; j++)
                                {
                                    all.Add(strPlayType + ";" + "[" + sg[j].ToString().Split(';')[0] + "]" + ";[" + LocateWaysAndMultiples[i] + "]");
                                }
                                break;
                            case "BG":   //8串1购买注数和对应倍数
                                sg = getAll8G(GamesNumber, Locate, LocateBuyResult, Screenings, int.Parse(WaysMultiples[i].ToString()));
                                for (int j = 0; j < sg.Length; j++)
                                {
                                    all.Add(strPlayType + ";" + "[" + sg[j].ToString().Split(';')[0] + "]" + ";[" + LocateWaysAndMultiples[i] + "]");
                                }
                                break;
                            case "BH":   //8串8购买注数和对应倍数
                                sg = getAll7G(GamesNumber, Locate, LocateBuyResult, Screenings, int.Parse(WaysMultiples[i].ToString()));
                                for (int j = 0; j < sg.Length; j++)
                                {
                                    all.Add(strPlayType + ";" + "[" + sg[j].ToString().Split(';')[0] + "]" + ";[" + LocateWaysAndMultiples[i] + "]");
                                }
                                break;
                            case "BI":   //8串9购买注数和对应倍数
                                sg = getAll7G(GamesNumber, Locate, LocateBuyResult, Screenings, int.Parse(WaysMultiples[i].ToString()));
                                for (int j = 0; j < sg.Length; j++)
                                {
                                    all.Add(strPlayType + ";" + "[" + sg[j].ToString().Split(';')[0] + "]" + ";[" + LocateWaysAndMultiples[i] + "]");
                                }

                                sg = getAll8G(GamesNumber, Locate, LocateBuyResult, Screenings, int.Parse(WaysMultiples[i].ToString()));
                                for (int j = 0; j < sg.Length; j++)
                                {
                                    all.Add(strPlayType + ";" + "[" + sg[j].ToString().Split(';')[0] + "]" + ";[" + LocateWaysAndMultiples[i] + "]");
                                }
                                break;
                            case "BJ":   //8串28购买注数和对应倍数
                                sg = getAll6G(GamesNumber, Locate, LocateBuyResult, Screenings, int.Parse(WaysMultiples[i].ToString()));
                                for (int j = 0; j < sg.Length; j++)
                                {
                                    all.Add(strPlayType + ";" + "[" + sg[j].ToString().Split(';')[0] + "]" + ";[" + LocateWaysAndMultiples[i] + "]");
                                }
                                break;
                            case "BK":   //8串56购买注数和对应倍数
                                sg = getAll5G(GamesNumber, Locate, LocateBuyResult, Screenings, int.Parse(WaysMultiples[i].ToString()));
                                for (int j = 0; j < sg.Length; j++)
                                {
                                    all.Add(strPlayType + ";" + "[" + sg[j].ToString().Split(';')[0] + "]" + ";[" + LocateWaysAndMultiples[i] + "]");
                                }
                                break;
                            case "BL":   //8串70购买注数和对应倍数
                                sg = getAll4G(GamesNumber, Locate, LocateBuyResult, Screenings, int.Parse(WaysMultiples[i].ToString()));
                                for (int j = 0; j < sg.Length; j++)
                                {
                                    all.Add(strPlayType + ";" + "[" + sg[j].ToString().Split(';')[0] + "]" + ";[" + LocateWaysAndMultiples[i] + "]");
                                }
                                break;
                            case "BM":   //8串247购买注数和对应倍数
                                sg = getAll2G(GamesNumber, Locate, LocateBuyResult, Screenings, int.Parse(WaysMultiples[i].ToString()));
                                for (int j = 0; j < sg.Length; j++)
                                {
                                    all.Add(strPlayType + ";" + "[" + sg[j].ToString().Split(';')[0] + "]" + ";[" + LocateWaysAndMultiples[i] + "]");
                                }

                                sg = getAll3G(GamesNumber, Locate, LocateBuyResult, Screenings, int.Parse(WaysMultiples[i].ToString()));
                                for (int j = 0; j < sg.Length; j++)
                                {
                                    all.Add(strPlayType + ";" + "[" + sg[j].ToString().Split(';')[0] + "]" + ";[" + LocateWaysAndMultiples[i] + "]");
                                }

                                sg = getAll4G(GamesNumber, Locate, LocateBuyResult, Screenings, int.Parse(WaysMultiples[i].ToString()));
                                for (int j = 0; j < sg.Length; j++)
                                {
                                    all.Add(strPlayType + ";" + "[" + sg[j].ToString().Split(';')[0] + "]" + ";[" + LocateWaysAndMultiples[i] + "]");
                                }

                                sg = getAll5G(GamesNumber, Locate, LocateBuyResult, Screenings, int.Parse(WaysMultiples[i].ToString()));
                                for (int j = 0; j < sg.Length; j++)
                                {
                                    all.Add(strPlayType + ";" + "[" + sg[j].ToString().Split(';')[0] + "]" + ";[" + LocateWaysAndMultiples[i] + "]");
                                }

                                sg = getAll6G(GamesNumber, Locate, LocateBuyResult, Screenings, int.Parse(WaysMultiples[i].ToString()));
                                for (int j = 0; j < sg.Length; j++)
                                {
                                    all.Add(strPlayType + ";" + "[" + sg[j].ToString().Split(';')[0] + "]" + ";[" + LocateWaysAndMultiples[i] + "]");
                                }

                                sg = getAll7G(GamesNumber, Locate, LocateBuyResult, Screenings, int.Parse(WaysMultiples[i].ToString()));
                                for (int j = 0; j < sg.Length; j++)
                                {
                                    all.Add(strPlayType + ";" + "[" + sg[j].ToString().Split(';')[0] + "]" + ";[" + LocateWaysAndMultiples[i] + "]");
                                }

                                sg = getAll8G(GamesNumber, Locate, LocateBuyResult, Screenings, int.Parse(WaysMultiples[i].ToString()));
                                for (int j = 0; j < sg.Length; j++)
                                {
                                    all.Add(strPlayType + ";" + "[" + sg[j].ToString().Split(';')[0] + "]" + ";[" + LocateWaysAndMultiples[i] + "]");
                                }
                                break;
                        }
                        #endregion
                    }

                }

                string[] Result = new string[all.Count];

                for (int i = 0; i < all.Count; i++)
                {
                    Result[i] = all[i].ToString();
                }
                return Result;
            }

            #region 取单关,二关...六关，M串1

            private string[] getAll1G(int GamesNumber, string[] Locate, string[] LocateBuyResult, string[] Screenings, int TempWaysMultiples)
            {
                ArrayList al = new ArrayList(); //取所有的双关，先存入ArrayList对象
                for (int i = 0; i < GamesNumber; i++)
                {
                    for (int i_0 = 0; i_0 < LocateBuyResult[i].Split(',').Length; i_0++)
                    {
                        al.Add(Screenings[i] + "(" + LocateBuyResult[i].Split(',')[i_0].ToString() + ");" + TempWaysMultiples);
                    }
                }

                string[] Result = new string[al.Count];
                for (int i = 0; i < al.Count; i++)
                {
                    Result[i] = al[i].ToString();
                }

                return Result;
            }

            private string[] getAll2G(int GamesNumber, string[] Locate, string[] LocateBuyResult, string[] Screenings, int TempWaysMultiples)
            {
                ArrayList al = new ArrayList(); //取所有的双关，先存入ArrayList对象
                for (int i = 0; i < GamesNumber; i++)
                {
                    for (int i_0 = 0; i_0 < LocateBuyResult[i].Split(',').Length; i_0++)
                    {
                        for (int j = i + 1; j < GamesNumber; j++)
                        {
                            for (int j_0 = 0; j_0 < LocateBuyResult[j].Split(',').Length; j_0++)
                            {
                                al.Add(Screenings[i] + "(" + LocateBuyResult[i].Split(',')[i_0].ToString() + ")|" + Screenings[j] + "(" + LocateBuyResult[j].Split(',')[j_0].ToString() + ");" + TempWaysMultiples);
                            }
                        }
                    }
                }

                string[] Result = new string[al.Count];
                for (int i = 0; i < al.Count; i++)
                {
                    Result[i] = al[i].ToString();
                }

                return Result;
            }

            private string[] getAll3G(int GamesNumber, string[] Locate, string[] LocateBuyResult, string[] Screenings, int TempWaysMultiples)
            {
                ArrayList al = new ArrayList(); //取所有的三关，先存入ArrayList对象
                for (int i = 0; i < GamesNumber; i++)
                {
                    for (int i_0 = 0; i_0 < LocateBuyResult[i].Split(',').Length; i_0++)
                    {
                        for (int j = i + 1; j < GamesNumber; j++)
                        {
                            for (int j_0 = 0; j_0 < LocateBuyResult[j].Split(',').Length; j_0++)
                            {
                                for (int k = j + 1; k < GamesNumber; k++)
                                {
                                    for (int k_0 = 0; k_0 < LocateBuyResult[k].Split(',').Length; k_0++)
                                    {
                                        al.Add(Screenings[i] + "(" + LocateBuyResult[i].Split(',')[i_0].ToString() + ")|" + Screenings[j] + "(" + LocateBuyResult[j].Split(',')[j_0].ToString() + ")|" + Screenings[k] + "(" + LocateBuyResult[k].Split(',')[k_0].ToString() + ");" + TempWaysMultiples);
                                    }
                                }
                            }
                        }
                    }
                }

                string[] Result = new string[al.Count];
                for (int i = 0; i < al.Count; i++)
                {
                    Result[i] = al[i].ToString();
                }

                return Result;
            }

            private string[] getAll4G(int GamesNumber, string[] Locate, string[] LocateBuyResult, string[] Screenings, int TempWaysMultiples)
            {
                ArrayList al = new ArrayList(); //取所有的四关，先存入ArrayList对象
                for (int i = 0; i < GamesNumber; i++)
                {
                    for (int i_0 = 0; i_0 < LocateBuyResult[i].Split(',').Length; i_0++)
                    {
                        for (int j = i + 1; j < GamesNumber; j++)
                        {
                            for (int j_0 = 0; j_0 < LocateBuyResult[j].Split(',').Length; j_0++)
                            {
                                for (int k = j + 1; k < GamesNumber; k++)
                                {
                                    for (int k_0 = 0; k_0 < LocateBuyResult[k].Split(',').Length; k_0++)
                                    {
                                        for (int x = k + 1; x < GamesNumber; x++)
                                        {
                                            for (int x_0 = 0; x_0 < LocateBuyResult[x].Split(',').Length; x_0++)
                                            {
                                                al.Add(Screenings[i] + "(" + LocateBuyResult[i].Split(',')[i_0].ToString() + ")|" + Screenings[j] + "(" + LocateBuyResult[j].Split(',')[j_0].ToString() + ")|" + Screenings[k] + "(" + LocateBuyResult[k].Split(',')[k_0].ToString() + ")|" + Screenings[x] + "(" + LocateBuyResult[x].Split(',')[x_0].ToString() + ");" + TempWaysMultiples);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }


                string[] Result = new string[al.Count];
                for (int i = 0; i < al.Count; i++)
                {
                    Result[i] = al[i].ToString();
                }

                return Result;
            }

            private string[] getAll5G(int GamesNumber, string[] Locate, string[] LocateBuyResult, string[] Screenings, int TempWaysMultiples)
            {
                ArrayList al = new ArrayList(); //取所有的五关，先存入ArrayList对象
                for (int i = 0; i < GamesNumber; i++)
                {
                    for (int i_0 = 0; i_0 < LocateBuyResult[i].Split(',').Length; i_0++)
                    {
                        for (int j = i + 1; j < GamesNumber; j++)
                        {
                            for (int j_0 = 0; j_0 < LocateBuyResult[j].Split(',').Length; j_0++)
                            {
                                for (int k = j + 1; k < GamesNumber; k++)
                                {
                                    for (int k_0 = 0; k_0 < LocateBuyResult[k].Split(',').Length; k_0++)
                                    {
                                        for (int x = k + 1; x < GamesNumber; x++)
                                        {
                                            for (int x_0 = 0; x_0 < LocateBuyResult[x].Split(',').Length; x_0++)
                                            {
                                                for (int y = x + 1; y < GamesNumber; y++)
                                                {
                                                    for (int y_0 = 0; y_0 < LocateBuyResult[y].Split(',').Length; y_0++)
                                                    {
                                                        al.Add(Screenings[i] + "(" + LocateBuyResult[i].Split(',')[i_0].ToString() + ")|" + Screenings[j] + "(" + LocateBuyResult[j].Split(',')[j_0].ToString() + ")|" + Screenings[k] + "(" + LocateBuyResult[k].Split(',')[k_0].ToString() + ")|" + Screenings[x] + "(" + LocateBuyResult[x].Split(',')[x_0].ToString() + ")|" + Screenings[y] + "(" + LocateBuyResult[y].Split(',')[y_0].ToString() + ");" + TempWaysMultiples);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                string[] Result = new string[al.Count];
                for (int i = 0; i < al.Count; i++)
                {
                    Result[i] = al[i].ToString();
                }

                return Result;
            }

            private string[] getAll6G(int GamesNumber, string[] Locate, string[] LocateBuyResult, string[] Screenings, int TempWaysMultiples)
            {
                ArrayList al = new ArrayList(); //取所有的六关，先存入ArrayList对象
                for (int i = 0; i < GamesNumber; i++)
                {
                    for (int i_0 = 0; i_0 < LocateBuyResult[i].Split(',').Length; i_0++)
                    {
                        for (int j = i + 1; j < GamesNumber; j++)
                        {
                            for (int j_0 = 0; j_0 < LocateBuyResult[j].Split(',').Length; j_0++)
                            {
                                for (int k = j + 1; k < GamesNumber; k++)
                                {
                                    for (int k_0 = 0; k_0 < LocateBuyResult[k].Split(',').Length; k_0++)
                                    {
                                        for (int x = k + 1; x < GamesNumber; x++)
                                        {
                                            for (int x_0 = 0; x_0 < LocateBuyResult[x].Split(',').Length; x_0++)
                                            {
                                                for (int y = x + 1; y < GamesNumber; y++)
                                                {
                                                    for (int y_0 = 0; y_0 < LocateBuyResult[y].Split(',').Length; y_0++)
                                                    {
                                                        for (int z = y + 1; z < GamesNumber; z++)
                                                        {
                                                            for (int z_0 = 0; z_0 < LocateBuyResult[z].Split(',').Length; z_0++)
                                                            {
                                                                al.Add(Screenings[i] + "(" + LocateBuyResult[i].Split(',')[i_0].ToString() + ")|" + Screenings[j] + "(" + LocateBuyResult[j].Split(',')[j_0].ToString() + ")|" + Screenings[k] + "(" + LocateBuyResult[k].Split(',')[k_0].ToString() + ")|" + Screenings[x] + "(" + LocateBuyResult[x].Split(',')[x_0].ToString() + ")|" + Screenings[y] + "(" + LocateBuyResult[y].Split(',')[y_0].ToString() + ")|" + Screenings[z] + "(" + LocateBuyResult[z].Split(',')[z_0].ToString() + ");" + TempWaysMultiples);
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                string[] Result = new string[al.Count];
                for (int i = 0; i < al.Count; i++)
                {
                    Result[i] = al[i].ToString();
                }

                return Result;
            }

            private string[] getAll7G(int GamesNumber, string[] Locate, string[] LocateBuyResult, string[] Screenings, int TempWaysMultiples)
            {
                ArrayList al = new ArrayList(); //取所有的七关，先存入ArrayList对象
                for (int i = 0; i < GamesNumber; i++)
                {
                    for (int i_0 = 0; i_0 < LocateBuyResult[i].Split(',').Length; i_0++)
                    {
                        for (int j = i + 1; j < GamesNumber; j++)
                        {
                            for (int j_0 = 0; j_0 < LocateBuyResult[j].Split(',').Length; j_0++)
                            {
                                for (int k = j + 1; k < GamesNumber; k++)
                                {
                                    for (int k_0 = 0; k_0 < LocateBuyResult[k].Split(',').Length; k_0++)
                                    {
                                        for (int x = k + 1; x < GamesNumber; x++)
                                        {
                                            for (int x_0 = 0; x_0 < LocateBuyResult[x].Split(',').Length; x_0++)
                                            {
                                                for (int y = x + 1; y < GamesNumber; y++)
                                                {
                                                    for (int y_0 = 0; y_0 < LocateBuyResult[y].Split(',').Length; y_0++)
                                                    {
                                                        for (int z = y + 1; z < GamesNumber; z++)
                                                        {
                                                            for (int z_0 = 0; z_0 < LocateBuyResult[z].Split(',').Length; z_0++)
                                                            {
                                                                for (int a = z + 1; a < GamesNumber; a++)
                                                                {
                                                                    for (int a_0 = 0; a_0 < LocateBuyResult[a].Split(',').Length; a_0++)
                                                                    {
                                                                        al.Add(Screenings[i] + "(" + LocateBuyResult[i].Split(',')[i_0].ToString() + ")|" + Screenings[j] + "(" + LocateBuyResult[j].Split(',')[j_0].ToString() + ")|" + Screenings[k] + "(" + LocateBuyResult[k].Split(',')[k_0].ToString() + ")|" + Screenings[x] + "(" + LocateBuyResult[x].Split(',')[x_0].ToString() + ")|" + Screenings[y] + "(" + LocateBuyResult[y].Split(',')[y_0].ToString() + ")|" + Screenings[z] + "(" + LocateBuyResult[z].Split(',')[z_0].ToString() + ")|" + Screenings[a] + "(" + LocateBuyResult[a].Split(',')[a_0].ToString() + ");" + TempWaysMultiples);
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                string[] Result = new string[al.Count];
                for (int i = 0; i < al.Count; i++)
                {
                    Result[i] = al[i].ToString();
                }

                return Result;
            }

            private string[] getAll8G(int GamesNumber, string[] Locate, string[] LocateBuyResult, string[] Screenings, int TempWaysMultiples)
            {
                ArrayList al = new ArrayList(); //取所有的八关，先存入ArrayList对象
                for (int i = 0; i < GamesNumber; i++)
                {
                    for (int i_0 = 0; i_0 < LocateBuyResult[i].Split(',').Length; i_0++)
                    {
                        for (int j = i + 1; j < GamesNumber; j++)
                        {
                            for (int j_0 = 0; j_0 < LocateBuyResult[j].Split(',').Length; j_0++)
                            {
                                for (int k = j + 1; k < GamesNumber; k++)
                                {
                                    for (int k_0 = 0; k_0 < LocateBuyResult[k].Split(',').Length; k_0++)
                                    {
                                        for (int x = k + 1; x < GamesNumber; x++)
                                        {
                                            for (int x_0 = 0; x_0 < LocateBuyResult[x].Split(',').Length; x_0++)
                                            {
                                                for (int y = x + 1; y < GamesNumber; y++)
                                                {
                                                    for (int y_0 = 0; y_0 < LocateBuyResult[y].Split(',').Length; y_0++)
                                                    {
                                                        for (int z = y + 1; z < GamesNumber; z++)
                                                        {
                                                            for (int z_0 = 0; z_0 < LocateBuyResult[z].Split(',').Length; z_0++)
                                                            {
                                                                for (int a = z + 1; a < GamesNumber; a++)
                                                                {
                                                                    for (int a_0 = 0; a_0 < LocateBuyResult[a].Split(',').Length; a_0++)
                                                                    {
                                                                        for (int b = a + 1; b < GamesNumber; b++)
                                                                        {
                                                                            for (int b_0 = 0; b_0 < LocateBuyResult[b].Split(',').Length; b_0++)
                                                                            {
                                                                                al.Add(Screenings[i] + "(" + LocateBuyResult[i].Split(',')[i_0].ToString() + ")|" + Screenings[j] + "(" + LocateBuyResult[j].Split(',')[j_0].ToString() + ")|" + Screenings[k] + "(" + LocateBuyResult[k].Split(',')[k_0].ToString() + ")|" + Screenings[x] + "(" + LocateBuyResult[x].Split(',')[x_0].ToString() + ")|" + Screenings[y] + "(" + LocateBuyResult[y].Split(',')[y_0].ToString() + ")|" + Screenings[z] + "(" + LocateBuyResult[z].Split(',')[z_0].ToString() + ")|" + Screenings[a] + "(" + LocateBuyResult[a].Split(',')[a_0].ToString() + ")|" + Screenings[b] + "(" + LocateBuyResult[b].Split(',')[b_0].ToString() + ");" + TempWaysMultiples);
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                string[] Result = new string[al.Count];
                for (int i = 0; i < al.Count; i++)
                {
                    Result[i] = al[i].ToString();
                }

                return Result;
            }
            #endregion

            public override double ComputeWin(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, int PlayType, params double[] WinMoneyList)
            {
                return 0;
            }

            public override string AnalyseScheme(string Content, int PlayType)
            {
                string CanonicalNumber = "";

                string[] strs = Content.Split('\n');

                if (strs == null)
                    return "";
                if (strs.Length == 0)
                    return "";

                string Result = "";

                for (int i = 0; i < strs.Length; i++)
                {
                    string[] Number = ToSingle(strs[i], ref CanonicalNumber, PlayType);

                    Result += strs[i] + "|" + Number.Length.ToString();

                }

                if (Result.EndsWith("\n"))
                    Result = Result.Substring(0, Result.Length - 1);
                return Result;
            }

            public override bool AnalyseWinNumber(string Number)
            {
                return true;
            }

            private string FilterRepeated(string NumberPart, string strPlayType)
            {
                string[] NumberPartSort = NumberPart.Split(',');
                if (NumberPartSort.Length == 1)
                {
                    return NumberPart;
                }

                string TemNumberPartSort = "";
                string ReconstructionNumberPart = "";
                int intNumberPartJ = 0;
                int intNumberPartJ1 = 0;
                for (int i = 0; i < NumberPartSort.Length; i++)
                {
                    for (int j = 1; j < NumberPartSort.Length - i; j++)
                    {
                        try
                        {
                            intNumberPartJ = int.Parse(NumberPartSort[j]);
                        }
                        catch
                        {
                            intNumberPartJ = -1;
                        }

                        try
                        {
                            intNumberPartJ1 = int.Parse(NumberPartSort[j - 1]);
                        }
                        catch
                        {
                            intNumberPartJ1 = -1;
                        }

                        if (intNumberPartJ < intNumberPartJ1)
                        {
                            TemNumberPartSort = NumberPartSort[j - 1];
                            NumberPartSort[j - 1] = NumberPartSort[j];
                            NumberPartSort[j] = TemNumberPartSort;
                        }
                    }
                }

                for (int i = 0; i < NumberPartSort.Length; i++)
                {
                    if (("123".IndexOf(NumberPartSort[i]) >= 0) && (strPlayType == "7201"))
                    {
                        ReconstructionNumberPart += NumberPartSort[i] + ",";
                    }
                    else if (strPlayType == "7202") //1-25
                    {
                        int TemNumberPart = 0;
                        try
                        {
                            TemNumberPart = int.Parse(NumberPartSort[i]);
                        }
                        catch
                        {
                            TemNumberPart = 0;
                        }
                        if ((TemNumberPart > 0) && (TemNumberPart < 32))
                        {
                            ReconstructionNumberPart += NumberPartSort[i] + ",";
                        }
                    }
                    else if (("12345678".IndexOf(NumberPartSort[i]) >= 0) && (strPlayType == "7203"))     //0-7
                    {
                        ReconstructionNumberPart += NumberPartSort[i] + ",";
                    }
                    else if (("123456789".IndexOf(NumberPartSort[i]) >= 0) && (strPlayType == "7204"))     //1-4
                    {
                        ReconstructionNumberPart += NumberPartSort[i] + ",";
                    }
                }

                //去重复
                string[] temp = ReconstructionNumberPart.Split(',');
                ReconstructionNumberPart = "";
                foreach (string t in temp)
                {
                    if (ReconstructionNumberPart.IndexOf(t + ",") == -1)
                    {
                        ReconstructionNumberPart += t + ",";
                    }
                }

                ReconstructionNumberPart = ReconstructionNumberPart.Substring(0, ReconstructionNumberPart.Length - 1);
                return ReconstructionNumberPart;
            }

            public override string ShowNumber(string Number)
            {
                return base.ShowNumber(Number, " ");
            }

            /// <summary>
            /// 提取方案的内容和购买方式
            /// </summary>
            /// <param name="Scheme">传入的方案</param>
            /// <param name="BuyContent">购买内容</param>
            /// <param name="CnLocateWaysAndMultiples">购买方式和相应倍数</param>
            /// <returns></returns>
            public override bool GetSchemeSplit(string Scheme, ref string BuyContent, ref string CnLocateWaysAndMultiples)
            {
                int SchemeLength = Scheme.Split(';').Length;
                if (SchemeLength < 3)
                {
                    BuyContent = "";
                    CnLocateWaysAndMultiples = "";
                    return false;
                }

                string strPlayType = Scheme.Trim().Split(';')[0].ToString();
                string BuyNumber = Scheme.Trim().Split(';')[1].ToString();
                BuyContent = BuyNumber.Substring(1, BuyNumber.Length - 1).Substring(0, BuyNumber.Length - 2).ToString().Trim();
                if (BuyContent == "")
                {
                    BuyContent = "";
                    CnLocateWaysAndMultiples = "";
                    return false;
                }

                string BuyWays = Scheme.Trim().Split(';')[2].ToString().Substring(1, Scheme.Trim().Split(';')[2].ToString().Length - 1).Substring(0, Scheme.Trim().Split(';')[2].ToString().Length - 2).ToString().Trim();
                if (BuyWays == "")
                {
                    BuyContent = "";
                    CnLocateWaysAndMultiples = "";
                    return false;
                }

                string[] LocateWaysAndMultiples = BuyWays.Split(',');
                for (int j = 0; j < LocateWaysAndMultiples.Length; j++)
                {
                    switch (LocateWaysAndMultiples[j].ToString().Substring(0, 2))
                    {
                        case "A0":   //单关购买注数和对应倍数
                            if (CnLocateWaysAndMultiples != "")
                            {
                                CnLocateWaysAndMultiples += "，";
                            }
                            CnLocateWaysAndMultiples += "单关  " + LocateWaysAndMultiples[j].Substring(2) + " 倍";
                            break;
                        case "AA":   //双关购买注数和对应倍数
                            if (CnLocateWaysAndMultiples != "")
                            {
                                CnLocateWaysAndMultiples += "，";
                            }
                            CnLocateWaysAndMultiples += "2串1  " + LocateWaysAndMultiples[j].Substring(2) + " 倍";
                            break;
                        case "AB":   //三关购买注数和对应倍数
                            if (CnLocateWaysAndMultiples != "")
                            {
                                CnLocateWaysAndMultiples += "，";
                            }
                            CnLocateWaysAndMultiples += "3串1  " + LocateWaysAndMultiples[j].Substring(2) + " 倍";
                            break;
                        case "AC":   //3串3购买注数和对应倍数
                            if (CnLocateWaysAndMultiples != "")
                            {
                                CnLocateWaysAndMultiples += "，";
                            }
                            CnLocateWaysAndMultiples += "3串3  " + LocateWaysAndMultiples[j].Substring(2) + " 倍";
                            break;
                        case "AD":   //3串4购买注数和对应倍数
                            if (CnLocateWaysAndMultiples != "")
                            {
                                CnLocateWaysAndMultiples += "，";
                            }
                            CnLocateWaysAndMultiples += "3串4 " + LocateWaysAndMultiples[j].Substring(2) + " 倍";
                            break;
                        case "AE":   //3串1购买注数和对应倍数
                            if (CnLocateWaysAndMultiples != "")
                            {
                                CnLocateWaysAndMultiples += "，";
                            }
                            CnLocateWaysAndMultiples += "4串1  " + LocateWaysAndMultiples[j].Substring(2) + " 倍";
                            break;
                        case "AF":   //3串4购买注数和对应倍数
                            if (CnLocateWaysAndMultiples != "")
                            {
                                CnLocateWaysAndMultiples += "，";
                            }
                            CnLocateWaysAndMultiples += "4串4  " + LocateWaysAndMultiples[j].Substring(2) + " 倍";
                            break;
                        case "AG":    //3串7购买注数和对应倍数
                            if (CnLocateWaysAndMultiples != "")
                            {
                                CnLocateWaysAndMultiples += "，";
                            }
                            CnLocateWaysAndMultiples += "4串5  " + LocateWaysAndMultiples[j].Substring(2) + " 倍";
                            break;
                        case "AH":    //4串1购买注数和对应倍数
                            if (CnLocateWaysAndMultiples != "")
                            {
                                CnLocateWaysAndMultiples += "，";
                            }
                            CnLocateWaysAndMultiples += "4串6  " + LocateWaysAndMultiples[j].Substring(2) + " 倍";
                            break;
                        case "AI":   //4串5购买注数和对应倍数
                            if (CnLocateWaysAndMultiples != "")
                            {
                                CnLocateWaysAndMultiples += "，";
                            }
                            CnLocateWaysAndMultiples += "4串11  " + LocateWaysAndMultiples[j].Substring(2) + " 倍";
                            break;
                        case "AJ":   //4串11购买注数和对应倍数
                            if (CnLocateWaysAndMultiples != "")
                            {
                                CnLocateWaysAndMultiples += "，";
                            }
                            CnLocateWaysAndMultiples += "5串1  " + LocateWaysAndMultiples[j].Substring(2) + " 倍";
                            break;
                        case "AK":   //4串15购买注数和对应倍数
                            if (CnLocateWaysAndMultiples != "")
                            {
                                CnLocateWaysAndMultiples += "，";
                            }
                            CnLocateWaysAndMultiples += "5串5  " + LocateWaysAndMultiples[j].Substring(2) + " 倍";
                            break;
                        case "AL":   //5串1购买注数和对应倍数
                            if (CnLocateWaysAndMultiples != "")
                            {
                                CnLocateWaysAndMultiples += "，";
                            }
                            CnLocateWaysAndMultiples += "5串6  " + LocateWaysAndMultiples[j].Substring(2) + " 倍";
                            break;
                        case "AM":   //5串6购买注数和对应倍数
                            if (CnLocateWaysAndMultiples != "")
                            {
                                CnLocateWaysAndMultiples += "，";
                            }
                            CnLocateWaysAndMultiples += "5串10  " + LocateWaysAndMultiples[j].Substring(2) + " 倍";
                            break;
                        case "AN":   //5串16购买注数和对应倍数
                            if (CnLocateWaysAndMultiples != "")
                            {
                                CnLocateWaysAndMultiples += "，";
                            }
                            CnLocateWaysAndMultiples += "5串16  " + LocateWaysAndMultiples[j].Substring(2) + " 倍";
                            break;
                        case "AO":   //5串26购买注数和对应倍数
                            if (CnLocateWaysAndMultiples != "")
                            {
                                CnLocateWaysAndMultiples += "，";
                            }
                            CnLocateWaysAndMultiples += "5串20  " + LocateWaysAndMultiples[j].Substring(2) + " 倍";
                            break;
                        case "AP":   //5串31购买注数和对应倍数
                            if (CnLocateWaysAndMultiples != "")
                            {
                                CnLocateWaysAndMultiples += "，";
                            }
                            CnLocateWaysAndMultiples += "5串26  " + LocateWaysAndMultiples[j].Substring(2) + " 倍";
                            break;
                        case "AQ":   //6串1购买注数和对应倍数
                            if (CnLocateWaysAndMultiples != "")
                            {
                                CnLocateWaysAndMultiples += "，";
                            }
                            CnLocateWaysAndMultiples += "6串1  " + LocateWaysAndMultiples[j].Substring(2) + " 倍";
                            break;
                        case "AR":   //6串7购买注数和对应倍数
                            if (CnLocateWaysAndMultiples != "")
                            {
                                CnLocateWaysAndMultiples += "，";
                            }
                            CnLocateWaysAndMultiples += "6串6  " + LocateWaysAndMultiples[j].Substring(2) + " 倍";
                            break;
                        case "AS":   //6串22购买注数和对应倍数
                            if (CnLocateWaysAndMultiples != "")
                            {
                                CnLocateWaysAndMultiples += "，";
                            }
                            CnLocateWaysAndMultiples += "6串7  " + LocateWaysAndMultiples[j].Substring(2) + " 倍";
                            break;
                        case "AT":   //6串22购买注数和对应倍数
                            if (CnLocateWaysAndMultiples != "")
                            {
                                CnLocateWaysAndMultiples += "，";
                            }
                            CnLocateWaysAndMultiples += "6串15  " + LocateWaysAndMultiples[j].Substring(2) + " 倍";
                            break;
                        case "AU":   //6串42购买注数和对应倍数
                            if (CnLocateWaysAndMultiples != "")
                            {
                                CnLocateWaysAndMultiples += "，";
                            }
                            CnLocateWaysAndMultiples += "6串20  " + LocateWaysAndMultiples[j].Substring(2) + " 倍";
                            break;
                        case "AV":   //6串57购买注数和对应倍数
                            if (CnLocateWaysAndMultiples != "")
                            {
                                CnLocateWaysAndMultiples += "，";
                            }
                            CnLocateWaysAndMultiples += "6串22  " + LocateWaysAndMultiples[j].Substring(2) + " 倍";
                            break;
                        case "AW":   //6串63购买注数和对应倍数
                            if (CnLocateWaysAndMultiples != "")
                            {
                                CnLocateWaysAndMultiples += "，";
                            }
                            CnLocateWaysAndMultiples += "6串35  " + LocateWaysAndMultiples[j].Substring(2) + " 倍";
                            break;
                        case "AX":   //7串1购买注数和对应倍数
                            if (CnLocateWaysAndMultiples != "")
                            {
                                CnLocateWaysAndMultiples += "，";
                            }
                            CnLocateWaysAndMultiples += "6串42  " + LocateWaysAndMultiples[j].Substring(2) + " 倍";
                            break;
                        case "AY":   //8串1购买注数和对应倍数
                            if (CnLocateWaysAndMultiples != "")
                            {
                                CnLocateWaysAndMultiples += "，";
                            }
                            CnLocateWaysAndMultiples += "6串50  " + LocateWaysAndMultiples[j].Substring(2) + " 倍";
                            break;
                        case "AZ":   //9串1购买注数和对应倍数
                            if (CnLocateWaysAndMultiples != "")
                            {
                                CnLocateWaysAndMultiples += "，";
                            }
                            CnLocateWaysAndMultiples += "6串57  " + LocateWaysAndMultiples[j].Substring(2) + " 倍";
                            break;
                        default:
                            CnLocateWaysAndMultiples += " <font color='red'>读取错误！</font>";
                            break;
                    }
                }

                return true;
            }

            public override Ticket[] ToElectronicTicket_CTTCSD(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                string[] strNumber = Number.Split('\n');

                if (strNumber == null)
                    return null;
                if (strNumber.Length == 0)
                    return null;

                string CanonicalNumber = "";
                Money = 0;

                ArrayList al = new ArrayList();

                for (int j = 0; j < strNumber.Length; j++)
                {
                    string[] str = ToElectronicTicketSingle(strNumber[j], PlayTypeID);

                    if (str == null)
                    {
                        return null;
                    }
                    if (str.Length == 0)
                    {
                        return null;
                    }

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

                        for (int i = 0; i < str.Length; i++)
                        {
                            string Numbers = "";
                            EachMoney = 0;

                            Numbers = str[i];

                            try
                            {
                                EachMoney += 2 * ToSingle(str[i], ref CanonicalNumber, PlayTypeID).Length;
                            }
                            catch { EachMoney += 2 * 1; }

                            Money += EachMoney * EachMultiple;

                            al.Add(new Ticket(GetTicketID(Numbers), Numbers, EachMultiple, EachMoney * EachMultiple));
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

            private string[] ToElectronicTicketSingle(string Number, int PlayTypeID)
            {
                int SchemeLength = Number.Split(';').Length;

                if (SchemeLength < 3)
                {
                    return null;
                }

                string strPlayType = Number.Trim().Split(';')[0].ToString();
                string BuyNumber = Number.Trim().Split(';')[1].ToString();
                string Numbers = BuyNumber.Substring(1, BuyNumber.Length - 1).Substring(0, BuyNumber.Length - 2).ToString().Trim();

                if (Numbers == "")
                {
                    return null;
                }

                #region 投注对阵分析

                string NumbersDan = "";
                string NumbersTuo = "";

                if (Numbers.IndexOf("]") >= 0)
                {
                    NumbersDan = Numbers.Replace("][", "&").Split('&')[0];
                    NumbersTuo = Numbers.Replace("][", "&").Split('&')[1];
                }
                else
                {
                    NumbersTuo = Number;
                }

                int GamesNumberDan = NumbersDan.Split('|').Length;
                int GamesNumberTuo = NumbersTuo.Split('|').Length;

                if (string.IsNullOrEmpty(NumbersDan.Split('|')[0]))
                {
                    GamesNumberDan = 0;
                }

                if (string.IsNullOrEmpty(NumbersTuo.Split('|')[0]))
                {
                    GamesNumberTuo = 0;
                }

                string[] LocateDan = new string[GamesNumberDan];
                string[] LocateTuo = new string[GamesNumberTuo];

                string ConfirmationString = "";
                string BuyResult = "";

                if (strPlayType == "7201")    //胜平负
                {
                    if (GamesNumberDan > 0)
                    {
                        for (int i = 0; i < GamesNumberDan; i++)
                        {
                            ConfirmationString += @"(?<L" + i.ToString() + @">[\d]{1,9}[(][321]([,][321]){0,2}[)])[|]"; //方案格式：4501;[1(3)|5(3,1)|7(0)];[A2,B1]
                        }
                        ConfirmationString = ConfirmationString.Substring(0, ConfirmationString.Length - 3) + "[]][[]";
                    }

                    for (int i = GamesNumberDan; i < GamesNumberTuo + GamesNumberDan; i++)
                    {
                        ConfirmationString += @"(?<L" + i.ToString() + @">[\d]{1,9}[(][321]([,][321]){0,2}[)])[|]"; //方案格式：4501;[1(3)|5(3,1)|7(0)];[A2,B1]
                    }
                    ConfirmationString = ConfirmationString.Substring(0, ConfirmationString.Length - 3);
                }
                else if (strPlayType == "7202")   //比分
                {
                    if (GamesNumberDan > 0)
                    {
                        for (int i = 0; i < GamesNumberDan; i++)
                        {
                            ConfirmationString += @"(?<L" + i.ToString() + @">[\d]{1,9}[(][\d]{1,2}([,][\d]{1,2}){0,30}[)])[|]";
                        }
                        ConfirmationString = ConfirmationString.Substring(0, ConfirmationString.Length - 3) + "[]][[]";
                    }

                    for (int i = GamesNumberDan; i < GamesNumberTuo + GamesNumberDan; i++)
                    {
                        ConfirmationString += @"(?<L" + i.ToString() + @">[\d]{1,9}[(][\d]{1,2}([,][\d]{1,2}){0,30}[)])[|]";
                    }
                    ConfirmationString = ConfirmationString.Substring(0, ConfirmationString.Length - 3);
                }
                else if (strPlayType == "7203")   //总进球
                {
                    if (GamesNumberDan > 0)
                    {
                        for (int i = 0; i < GamesNumberDan; i++)
                        {
                            ConfirmationString += @"(?<L" + i.ToString() + @">[\d]{1,9}[(][12345678]([,][12345678]){0,8}[)])[|]";
                        }
                        ConfirmationString = ConfirmationString.Substring(0, ConfirmationString.Length - 3) + "[]][[]";
                    }

                    for (int i = GamesNumberDan; i < GamesNumberTuo + GamesNumberDan; i++)
                    {
                        ConfirmationString += @"(?<L" + i.ToString() + @">[\d]{1,9}[(][12345678]([,][12345678]){0,8}[)])[|]";
                    }
                    ConfirmationString = ConfirmationString.Substring(0, ConfirmationString.Length - 3);
                }
                else if (strPlayType == "7204")   //半全场胜平负
                {
                    if (GamesNumberDan > 0)
                    {
                        for (int i = 0; i < GamesNumberDan; i++)
                        {
                            ConfirmationString += @"(?<L" + i.ToString() + @">[\d]{1,9}[(][123456789]([,][123456789]){0,8}[)])[|]";
                        }
                        ConfirmationString = ConfirmationString.Substring(0, ConfirmationString.Length - 3) + "[]][[]";
                    }

                    for (int i = GamesNumberDan; i < GamesNumberTuo + GamesNumberDan; i++)
                    {
                        ConfirmationString += @"(?<L" + i.ToString() + @">[\d]{1,9}[(][123456789]([,][123456789]){0,8}[)])[|]";
                    }
                    ConfirmationString = ConfirmationString.Substring(0, ConfirmationString.Length - 3);
                }
                else if (strPlayType == "7205")   //单关比分
                {
                    if (GamesNumberDan > 0)
                    {
                        for (int i = 0; i < GamesNumberDan; i++)
                        {
                            ConfirmationString += @"(?<L" + i.ToString() + @">[\d]{1,9}[(][\d]{1,2}([,][\d]{1,2}){0,30}[)])[|]";
                        }
                        ConfirmationString = ConfirmationString.Substring(0, ConfirmationString.Length - 3) + "[]][[]";
                    }

                    for (int i = GamesNumberDan; i < GamesNumberTuo + GamesNumberDan; i++)
                    {
                        ConfirmationString += @"(?<L" + i.ToString() + @">[\d]{1,9}[(][\d]{1,2}([,][\d]{1,2}){0,30}[)])[|]";
                    }
                    ConfirmationString = ConfirmationString.Substring(0, ConfirmationString.Length - 3);
                }

                Regex regex = new Regex(ConfirmationString, RegexOptions.IgnoreCase | RegexOptions.Compiled);
                Match m = regex.Match(Numbers);

                for (int i = 0; i < GamesNumberDan; i++)
                {
                    LocateDan[i] = m.Groups["L" + i.ToString()].ToString().Trim();
                    if (LocateDan[i] == "")
                    {
                        return null;
                    }

                    BuyResult = LocateDan[i].Substring(LocateDan[i].IndexOf('(') + 1, (LocateDan[i].IndexOf(')') - LocateDan[i].IndexOf('(') - 1));

                    if (BuyResult.Length > 0)
                    {
                        BuyResult = FilterRepeated(BuyResult, strPlayType);

                        if (BuyResult == "")
                        {
                            return null;
                        }
                    }
                }

                for (int i = GamesNumberDan; i < GamesNumberDan + GamesNumberTuo; i++)
                {
                    LocateTuo[i - GamesNumberDan] = m.Groups["L" + i.ToString()].ToString().Trim();

                    if (LocateTuo[i - GamesNumberDan] == "")
                    {
                        return null;
                    }

                    BuyResult = LocateTuo[i - GamesNumberDan].Substring(LocateTuo[i - GamesNumberDan].IndexOf('(') + 1, (LocateTuo[i - GamesNumberDan].IndexOf(')') - LocateTuo[i - GamesNumberDan].IndexOf('(') - 1));

                    if (BuyResult.Length > 0)
                    {
                        BuyResult = FilterRepeated(BuyResult, strPlayType);

                        if (BuyResult == "")
                        {
                            return null;
                        }
                    }
                }

                #endregion

                #region 过关方式分析
                string BuyWays = Number.Trim().Split(';')[2].ToString().Substring(1, Number.Trim().Split(';')[2].ToString().Length - 1).Substring(0, Number.Trim().Split(';')[2].ToString().Length - 2).ToString().Trim();
                if (BuyWays == "")
                {
                    return null;
                }

                int WaysNumber = BuyWays.Split(',').Length;
                string[] LocateWays = new string[WaysNumber];
                string WaysResult = "";
                string BuyWaysResult = "";
                int TempWaysMultiples = 0;

                ConfirmationString = "";

                for (int j = 0; j < WaysNumber; j++)
                {
                    ConfirmationString += @"(?<L" + j.ToString() + @">[123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ]{1,1000}[\d]{1,5})[,]"; //方案格式：4501;[1(3)|5(3,1)|7(0)];[A2,B1]
                }
                ConfirmationString = ConfirmationString.Substring(0, ConfirmationString.Length - 3);

                Regex regexWays = new Regex(ConfirmationString, RegexOptions.IgnoreCase | RegexOptions.Compiled);
                Match mWays = regexWays.Match(BuyWays);
                for (int k = 0; k < WaysNumber; k++)
                {
                    LocateWays[k] = mWays.Groups["L" + k.ToString()].ToString().Trim();
                    if (LocateWays[k] == "")
                    {
                        return null;
                    }

                    BuyResult = LocateWays[k].Substring(2, (LocateWays[k].Length - 2));

                    if (BuyResult.Length > 0)
                    {
                        try
                        {
                            TempWaysMultiples = Convert.ToInt32(BuyResult);
                        }
                        catch
                        {
                            TempWaysMultiples = 0;
                        }

                        if (TempWaysMultiples > 0)
                        {
                            BuyWaysResult = BuyResult;
                        }
                        else
                        {
                            BuyWaysResult = "";
                        }
                    }
                    if (BuyWaysResult.Length > 0)
                    {
                        WaysResult += LocateWays[k].Substring(0, 2).ToUpper() + BuyWaysResult.ToString() + ",";
                    }
                }

                if (WaysResult.EndsWith(","))
                {
                    WaysResult = WaysResult.Substring(0, WaysResult.Length - 1);
                }

                if (WaysResult.Length < 2)   //至少有一种过关方式 例如：11：表示单关，1倍
                {
                    return null;
                }

                int Len = 0;

                if (LocateDan.Length > 0)
                {
                    Len = Shove._Convert.StrToInt(Number.Trim().Split(';')[3].Substring(1, 1), LocateDan.Length);

                    if (Len > LocateDan.Length)
                    {
                        return null;
                    }
                }


                //复式变单式，并取得购买注数和相应的倍数
                ArrayList al = new ArrayList();

                WaysNumber = 0; //将原来的购买方式倍数清0
                WaysNumber = WaysResult.Split(',').Length;  //重新获取分析过后的购买方式及倍数

                int Num = 0;
                string[] NumberDan = null;
                string[] NumberTuo = null;

                for (int i = 0; i < WaysNumber; i++)
                {
                    if ("A0".IndexOf(LocateWays[i].Substring(0, 2).ToUpper()) >= 0)
                    {
                        al.Add(Number);
                    }
                    else if ("AA".IndexOf(LocateWays[i].Substring(0, 2).ToUpper()) >= 0 && Len < 2)
                    {
                        Num = LocateDan.Length > 2 ? 2 : LocateDan.Length;

                        for (int j = Len; j < Num + 1; j++)
                        {
                            NumberDan = NumberList(LocateDan, j);
                            NumberTuo = NumberList(LocateTuo, 2 - j);

                            if (NumberDan != null && NumberDan.Length > 0)
                            {
                                for (int p = 0; p < NumberDan.Length; p++)
                                {
                                    for (int x = 0; x < NumberTuo.Length; x++)
                                    {
                                        al.Add(strPlayType + ";" + "[" + NumberDan[p] + "|" + NumberTuo[x] + "]" + ";[" + LocateWays[i] + "]");
                                    }
                                }
                            }
                            else
                            {
                                for (int x = 0; x < NumberTuo.Length; x++)
                                {
                                    al.Add(strPlayType + ";" + "[" + NumberTuo[x] + "]" + ";[" + LocateWays[i] + "]");
                                }
                            }
                        }
                    }
                    else if ("AB AC AD ".IndexOf(LocateWays[i].Substring(0, 2).ToUpper()) >= 0 && Len < 3)
                    {
                        Num = LocateDan.Length > 3 ? 3 : LocateDan.Length;

                        for (int j = Len; j < Num + 1; j++)
                        {
                            NumberDan = NumberList(LocateDan, j);
                            NumberTuo = NumberList(LocateTuo, 3 - j);

                            if (NumberDan != null && NumberDan.Length > 0)
                            {
                                for (int p = 0; p < NumberDan.Length; p++)
                                {
                                    for (int x = 0; x < NumberTuo.Length; x++)
                                    {
                                        al.Add(strPlayType + ";" + "[" + NumberDan[p] + "|" + NumberTuo[x] + "]" + ";[" + LocateWays[i] + "]");
                                    }
                                }
                            }
                            else
                            {
                                for (int x = 0; x < NumberTuo.Length; x++)
                                {
                                    al.Add(strPlayType + ";" + "[" + NumberTuo[x] + "]" + ";[" + LocateWays[i] + "]");
                                }
                            }
                        }
                    }
                    else if ("AE AF AG AH AI ".IndexOf(LocateWays[i].Substring(0, 2).ToUpper()) >= 0 && Len < 4)
                    {
                        Num = LocateDan.Length > 4 ? 4 : LocateDan.Length;

                        for (int j = Len; j < Num + 1; j++)
                        {
                            NumberDan = NumberList(LocateDan, j);
                            NumberTuo = NumberList(LocateTuo, 4 - j);

                            if (NumberDan != null && NumberDan.Length > 0)
                            {
                                for (int p = 0; p < NumberDan.Length; p++)
                                {
                                    for (int x = 0; x < NumberTuo.Length; x++)
                                    {
                                        al.Add(strPlayType + ";" + "[" + NumberDan[p] + "|" + NumberTuo[x] + "]" + ";[" + LocateWays[i] + "]");
                                    }
                                }
                            }
                            else
                            {
                                for (int x = 0; x < NumberTuo.Length; x++)
                                {
                                    al.Add(strPlayType + ";" + "[" + NumberTuo[x] + "]" + ";[" + LocateWays[i] + "]");
                                }
                            }
                        }
                    }
                    else if ("AJ AK AL AM AN AO AP ".IndexOf(LocateWays[i].Substring(0, 2).ToUpper()) >= 0 && Len < 5)
                    {
                        Num = LocateDan.Length > 5 ? 5 : LocateDan.Length;

                        for (int j = Len; j < Num + 1; j++)
                        {
                            NumberDan = NumberList(LocateDan, j);
                            NumberTuo = NumberList(LocateTuo, 5 - j);

                            if (NumberDan != null && NumberDan.Length > 0)
                            {
                                for (int p = 0; p < NumberDan.Length; p++)
                                {
                                    for (int x = 0; x < NumberTuo.Length; x++)
                                    {
                                        al.Add(strPlayType + ";" + "[" + NumberDan[p] + "|" + NumberTuo[x] + "]" + ";[" + LocateWays[i] + "]");
                                    }
                                }
                            }
                            else
                            {
                                for (int x = 0; x < NumberTuo.Length; x++)
                                {
                                    al.Add(strPlayType + ";" + "[" + NumberTuo[x] + "]" + ";[" + LocateWays[i] + "]");
                                }
                            }
                        }
                    }
                    else if ("AQ AR AS AT AU AV AW AX AY AZ".IndexOf(LocateWays[i].Substring(0, 2).ToUpper()) >= 0 && Len < 6)
                    {
                        Num = LocateDan.Length > 6 ? 6 : LocateDan.Length;

                        for (int j = Len; j < Num + 1; j++)
                        {
                            NumberDan = NumberList(LocateDan, j);
                            NumberTuo = NumberList(LocateTuo, 6 - j);

                            if (NumberDan != null && NumberDan.Length > 0)
                            {
                                for (int p = 0; p < NumberDan.Length; p++)
                                {
                                    for (int x = 0; x < NumberTuo.Length; x++)
                                    {
                                        al.Add(strPlayType + ";" + "[" + NumberDan[p] + "|" + NumberTuo[x] + "]" + ";[" + LocateWays[i] + "]");
                                    }
                                }
                            }
                            else
                            {
                                for (int x = 0; x < NumberTuo.Length; x++)
                                {
                                    al.Add(strPlayType + ";" + "[" + NumberTuo[x] + "]" + ";[" + LocateWays[i] + "]");
                                }
                            }
                        }
                    }
                    else if ("BA BB BC BD BE BF".IndexOf(LocateWays[i].Substring(0, 2).ToUpper()) >= 0 && Len < 7)
                    {
                        Num = LocateDan.Length > 7 ? 7 : LocateDan.Length;

                        for (int j = Len; j < Num + 1; j++)
                        {
                            NumberDan = NumberList(LocateDan, j);
                            NumberTuo = NumberList(LocateTuo, 7 - j);

                            if (NumberDan != null && NumberDan.Length > 0)
                            {
                                for (int p = 0; p < NumberDan.Length; p++)
                                {
                                    for (int x = 0; x < NumberTuo.Length; x++)
                                    {
                                        al.Add(strPlayType + ";" + "[" + NumberDan[p] + "|" + NumberTuo[x] + "]" + ";[" + LocateWays[i] + "]");
                                    }
                                }
                            }
                            else
                            {
                                for (int x = 0; x < NumberTuo.Length; x++)
                                {
                                    al.Add(strPlayType + ";" + "[" + NumberTuo[x] + "]" + ";[" + LocateWays[i] + "]");
                                }
                            }
                        }
                    }
                    else if ("BG BH BI BJ BK BL BM".IndexOf(LocateWays[i].Substring(0, 2).ToUpper()) >= 0 && Len < 8)
                    {
                        Num = LocateDan.Length > 8 ? 8 : LocateDan.Length;

                        for (int j = Len; j < Num + 1; j++)
                        {
                            NumberDan = NumberList(LocateDan, j);
                            NumberTuo = NumberList(LocateTuo, 8 - j);

                            if (NumberDan != null && NumberDan.Length > 0)
                            {
                                for (int p = 0; p < NumberDan.Length; p++)
                                {
                                    for (int x = 0; x < NumberTuo.Length; x++)
                                    {
                                        al.Add(strPlayType + ";" + "[" + NumberDan[p] + "|" + NumberTuo[x] + "]" + ";[" + LocateWays[i] + "]");
                                    }
                                }
                            }
                            else
                            {
                                for (int x = 0; x < NumberTuo.Length; x++)
                                {
                                    al.Add(strPlayType + ";" + "[" + NumberTuo[x] + "]" + ";[" + LocateWays[i] + "]");
                                }
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

            private int GetTicketID(string Number)
            {
                string[] Numbers = Number.Split(';');

                if (Numbers.Length != 3)
                {
                    return 0;
                }

                switch (Numbers[2].Substring(1, 2))
                {
                    case "A0":
                        return 500;
                    case "AA":
                        return 502;
                    case "AB":
                        return 503;
                    case "AC":
                        return 526;
                    case "AD":
                        return 527;
                    case "AE":
                        return 504;
                    case "AF":
                        return 539;
                    case "AG":
                        return 540;
                    case "AH":
                        return 528;
                    case "AI":
                        return 529;
                    case "AJ":
                        return 505;
                    case "AK":
                        return 544;
                    case "AL":
                        return 545;
                    case "AM":
                        return 530;
                    case "AN":
                        return 541;
                    case "AO":
                        return 531;
                    case "AP":
                        return 532;
                    case "AQ":
                        return 506;
                    case "AR":
                        return 549;
                    case "AS":
                        return 550;
                    case "AT":
                        return 533;
                    case "AU":
                        return 542;
                    case "AV":
                        return 546;
                    case "AW":
                        return 534;
                    case "AX":
                        return 543;
                    case "AY":
                        return 535;
                    case "AZ":
                        return 536;
                    case "BA":
                        return 507;
                    case "BB":
                        return 553;
                    case "BC":
                        return 554;
                    case "BD":
                        return 551;
                    case "BE":
                        return 547;
                    case "BF":
                        return 537;
                    case "BG":
                        return 508;
                    case "BH":
                        return 556;
                    case "BI":
                        return 557;
                    case "BJ":
                        return 555;
                    case "BK":
                        return 552;
                    case "BL":
                        return 548;
                    case "BM":
                        return 538;
                    default:
                        return 0;
                }
            }

            /// <summary>
            /// 得到每组数据为 len 的个数
            /// </summary>
            /// <param name="Number">字符数组</param>
            /// <param name="Len">每组的个数</param>
            /// <returns></returns>
            private string[] NumberList(string[] Number, int Len)
            {
                if (Number.Length < 1)
                {
                    return null;
                }

                if (Len < 1)
                {
                    return null;
                }

                ArrayList al = new ArrayList();

                int N = Number.Length;

                switch (Len)
                {
                    case 1:
                        for (int i = 0; i < N; i++)
                        {
                            al.Add(Number[i]);
                        }
                        break;
                    case 2:
                        for (int i = 0; i < N - 1; i++)
                        {
                            for (int j = i + 1; j < N; j++)
                            {
                                al.Add(Number[i] + "|" + Number[j]);
                            }
                        }
                        break;
                    case 3:
                        for (int i = 0; i < N - 2; i++)
                        {
                            for (int j = i + 1; j < N - 1; j++)
                            {
                                for (int k = j + 1; k < N; k++)
                                {
                                    al.Add(Number[i] + "|" + Number[j] + "|" + Number[k]);
                                }
                            }
                        }
                        break;
                    case 4:
                        for (int i = 0; i < N - 3; i++)
                        {
                            for (int j = i + 1; j < N - 2; j++)
                            {
                                for (int k = j + 1; k < N - 1; k++)
                                {
                                    for (int l = k + 1; l < N; l++)
                                    {
                                        al.Add(Number[i] + "|" + Number[j] + "|" + Number[k] + "|" + Number[l]);
                                    }
                                }
                            }
                        }
                        break;
                    case 5:
                        for (int i = 0; i < N - 4; i++)
                        {
                            for (int j = i + 1; j < N - 3; j++)
                            {
                                for (int k = j + 1; k < N - 2; k++)
                                {
                                    for (int l = k + 1; l < N - 1; l++)
                                    {
                                        for (int m = l + 1; m < N; m++)
                                        {
                                            al.Add(Number[i] + "|" + Number[j] + "|" + Number[k] + "|" + Number[l] + "|" + Number[m]);
                                        }
                                    }
                                }
                            }
                        }
                        break;
                    case 6:
                        for (int i = 0; i < N - 5; i++)
                        {
                            for (int j = i + 1; j < N - 4; j++)
                            {
                                for (int k = j + 1; k < N - 3; k++)
                                {
                                    for (int l = k + 1; l < N - 2; l++)
                                    {
                                        for (int m = l + 1; m < N - 1; m++)
                                        {
                                            for (int p = m + 1; p < N; p++)
                                            {
                                                al.Add(Number[i] + "|" + Number[j] + "|" + Number[k] + "|" + Number[l] + "|" + Number[m] + "|" + Number[p]);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        break;
                    case 7:
                        for (int a = 0; a < N - 6; a++)
                        {
                            for (int i = a + 1; i < N - 5; i++)
                            {
                                for (int j = i + 1; j < N - 4; j++)
                                {
                                    for (int k = j + 1; k < N - 3; k++)
                                    {
                                        for (int l = k + 1; l < N - 2; l++)
                                        {
                                            for (int m = l + 1; m < N - 1; m++)
                                            {
                                                for (int p = m + 1; p < N; p++)
                                                {
                                                    al.Add(Number[a] + "|" + Number[i] + "|" + Number[j] + "|" + Number[k] + "|" + Number[l] + "|" + Number[m] + "|" + Number[p]);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        break;
                    case 8:
                        for (int b = 0; b < N - 7; b++)
                        {
                            for (int a = b + 1; a < N - 6; a++)
                            {
                                for (int i = a + 1; i < N - 5; i++)
                                {
                                    for (int j = i + 1; j < N - 4; j++)
                                    {
                                        for (int k = j + 1; k < N - 3; k++)
                                        {
                                            for (int l = k + 1; l < N - 2; l++)
                                            {
                                                for (int m = l + 1; m < N - 1; m++)
                                                {
                                                    for (int p = m + 1; p < N; p++)
                                                    {
                                                        al.Add(Number[b] + "|" + Number[a] + "|" + Number[i] + "|" + Number[j] + "|" + Number[k] + "|" + Number[l] + "|" + Number[m] + "|" + Number[p]);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        break;
                    default:
                        break;
                }

                string[] Result = new string[al.Count];
                for (int i = 0; i < al.Count; i++)
                    Result[i] = al[i].ToString();
                return Result;
            }
        }
    }
}
