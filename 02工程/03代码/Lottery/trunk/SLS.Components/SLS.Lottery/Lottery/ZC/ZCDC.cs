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
        /// 足彩单场
        /// </summary>
        public partial class ZCDC : LotteryBase
        {
            #region 静态变量
            public const int PlayType_SPF = 4501;
            public const int PlayType_ZJQ = 4502;
            public const int PlayType_SXDS = 4503;
            public const int PlayType_ZQBF = 4504;
            public const int PlayType_BQCSPF = 4505;
            public const int ID = 45;
            public const string sID = "45";
            public const string Name = "足彩单场";
            public const string Code = "ZCDC";
            public const double MaxMoney = 200000;
            #endregion

            public ZCDC()
            {
                id = 45;
                name = "足彩单场";
                code = "ZCDC";
            }

            public override bool CheckPlayType(int play_type)
            {
                return ((play_type >= 4501) && (play_type <= 4505));
            }

            public override PlayType[] GetPlayTypeList()
            {
                PlayType[] Result = new PlayType[5];

                Result[0] = new PlayType(PlayType_SPF, "胜平负");
                Result[1] = new PlayType(PlayType_ZJQ, "总进球");
                Result[2] = new PlayType(PlayType_SXDS, "上下单双");
                Result[3] = new PlayType(PlayType_ZQBF, "正确比分");
                Result[4] = new PlayType(PlayType_BQCSPF, "半全场胜平负");

                return Result;
            }

            // 复式取单式, 后面 ref 参数是将方案规范化 例如："4501;[5(3)|15(3)|17(0)|19(0)|21(0)];[N2]";
            // (Number表示传入的方案，CanonicalNumber表示整理后的方案，PlayType购买类型)复式取单式, 后面 ref 参数是将方案规范化
            // 例如："4501;[5(3)|15(3)|17(0)|19(0)|21(0)];[N2]"(购买方法;[场次1(结果1,结果2)|场次2(结果1)];[购买方式1及其购买倍数1,购买方式2及其购买倍数2])
            // 购买方式中1表示单关，2表示双关，3表示三关，4-9、A-W表示串，购买方式只有一个字符，后面的字符表示倍数
            // 例如：11表示单关的1倍
            public override string[] ToSingle(string Scheme, ref string CanonicalNumber, int CompetitionCount)
            {
                //此方法的CompetitionCount表示本期的比赛的总场数

                //Scheme = "4501;[5(3)|15(3)|17(0)|19(0)|21(0)|22(0)];[11,21,32]"; //"4501;[5(3)|6(3)|7(0)|8(0)|9(0)|10(0)|11(3)|12(3)|13(0)|14(0)|15(0)|16(0)|17(0)|21(0)|22(0)];[11,21,32]";
                //Scheme = "4501;[5(3)|6(3)|7(0)|8(0)|9(0)|10(0)|11(3)|12(3)|13(0)|14(0)|15(0)|16(0)|17(0)|21(0)|22(0)];[11,21,32]";
                //Scheme = "4501;[5(3)|15(3)|17(0)|19(0)|21(0)];[N2]";
                //Scheme = "4501;[1(3,0)];[A2,B1]";
                //string Scheme = Number;  //方案字符串
                CanonicalNumber = "";
                int SchemeLength = Scheme.Split(';').Length;
                if (SchemeLength != 3)
                {
                    CanonicalNumber = "";
                    return null;
                }

                string strPlayType = Scheme.Trim().Split(';')[0].ToString();
                string BuyNumber = Scheme.Trim().Split(';')[1].ToString();
                string Numbers = BuyNumber.Substring(1, BuyNumber.Length - 1).Substring(0, BuyNumber.Length - 2).ToString().Trim();
                if (Numbers == "")
                {
                    CanonicalNumber = "";
                    return null;
                }

                string BuyWays = Scheme.Trim().Split(';')[2].ToString().Substring(1, Scheme.Trim().Split(';')[2].ToString().Length - 1).Substring(0, Scheme.Trim().Split(';')[2].ToString().Length - 2).ToString().Trim();
                if (BuyWays == "")
                {
                    CanonicalNumber = "";
                    return null;
                }

                int GamesNumber = Numbers.Split('|').Length;
                int WaysNumber = BuyWays.Split(',').Length;
                string[] Locate = new string[GamesNumber];
                string[] LocateWays = new string[WaysNumber];

                string ConfirmationString = "";
                string BuyResult = "";

                string WaysResult = "";
                string BuyWaysResult = "";
                int TempWaysMultiples = 0;

                if (strPlayType == "4501")    //胜平负
                {
                    for (int i = 0; i < GamesNumber; i++)
                    {
                        ConfirmationString += @"^(?<L" + i.ToString() + @"^>[\d]{1,2}[(][310]([,][310]){0,2}[)])[|]"; //方案格式：4501;[1(3)|5(3,1)|7(0)];[A2,B1]
                    }
                    ConfirmationString = ConfirmationString.Substring(0, ConfirmationString.Length - 3);
                }
                else if (strPlayType == "4502")   //总进球
                {
                    for (int i = 0; i < GamesNumber; i++)
                    {
                        ConfirmationString += @"^(?<L" + i.ToString() + @"^>[\d]{1,2}[(][01234567]([,][01234567]){0,7}[)])[|]";
                    }
                    ConfirmationString = ConfirmationString.Substring(0, ConfirmationString.Length - 3);
                }
                else if (strPlayType == "4503")   //上下单双
                {
                    for (int i = 0; i < GamesNumber; i++)
                    {
                        ConfirmationString += @"^(?<L" + i.ToString() + @"^>[\d]{1,2}[(][1234]([,][1234]){0,3}[)])[|]";
                    }
                    ConfirmationString = ConfirmationString.Substring(0, ConfirmationString.Length - 3);
                }
                else if (strPlayType == "4504")   //正确比分
                {
                    for (int i = 0; i < GamesNumber; i++)
                    {
                        ConfirmationString += @"^(?<L" + i.ToString() + @"^>[\d]{1,2}[(][\d]{1,2}([,][\d]{1,2}){0,24}[)])[|]";
                    }
                    ConfirmationString = ConfirmationString.Substring(0, ConfirmationString.Length - 3);
                }
                else if (strPlayType == "4505")   //半全场胜平负
                {
                    for (int i = 0; i < GamesNumber; i++)
                    {
                        ConfirmationString += @"^(?<L" + i.ToString() + @"^>[\d]{1,2}[(][\d]([,][\d]){0,8}[)])[|]";
                    }
                    ConfirmationString = ConfirmationString.Substring(0, ConfirmationString.Length - 3);
                }

                Regex regex = new Regex(ConfirmationString, RegexOptions.IgnoreCase | RegexOptions.Compiled);
                Match m = regex.Match(Numbers);
                for (int i = 0; i < GamesNumber; i++)
                {
                    Locate[i] = m.Groups["L" + i.ToString()].ToString().Trim();
                    if (Locate[i] == "")
                    {
                        CanonicalNumber = "";
                        return null;
                    }

                    //判断购买的场次大于当期的总场数
                    if ((CompetitionCount) < int.Parse(Locate[i].Substring(0, (Locate[i].IndexOf('('))).ToString()))
                    {
                        CanonicalNumber = "";
                        return null;
                    }

                    BuyResult = Locate[i].Substring(Locate[i].IndexOf('(') + 1, (Locate[i].IndexOf(')') - Locate[i].IndexOf('(') - 1));

                    if (BuyResult.Length > 0)
                    {
                        BuyResult = FilterRepeated(BuyResult, strPlayType);

                        if (BuyResult == "")
                        {
                            CanonicalNumber = "";
                            return null;
                        }
                    }

                    if (BuyResult.Length > 0)
                    {
                        CanonicalNumber += Locate[i].Substring(0, (Locate[i].IndexOf('('))) + "(" + BuyResult + ")|";
                    }
                }

                if (CanonicalNumber.Length < 4)  //方案中至少买有一场比赛 例如：1(3)
                {
                    CanonicalNumber = "";
                    return null;
                }

                CanonicalNumber = FilterRepeatedScheme(CanonicalNumber.Substring(0, CanonicalNumber.Length - 1));
                GamesNumber = 0;    //将原来的场数清0

                BuyResult = "";
                ConfirmationString = "";
                for (int j = 0; j < WaysNumber; j++)
                {
                    //志方修改
                    ConfirmationString += @"^(?<L" + j.ToString() + @"^>[123456789ABCDEFGHIJKLMNOPQRSTUVW]{1}[\d]{1,4})[,]"; //方案格式：4501;[1(3)|5(3,1)|7(0)];[A2,B1]
                }
                ConfirmationString = ConfirmationString.Substring(0, ConfirmationString.Length - 3);

                Regex regexWays = new Regex(ConfirmationString, RegexOptions.IgnoreCase | RegexOptions.Compiled);
                Match mWays = regexWays.Match(BuyWays);
                for (int k = 0; k < WaysNumber; k++)
                {
                    LocateWays[k] = mWays.Groups["L" + k.ToString()].ToString().Trim();
                    if (LocateWays[k] == "")
                    {
                        CanonicalNumber = "";
                        return null;
                    }

                    BuyResult = LocateWays[k].Substring(1, (LocateWays[k].Length - 1));

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
                        WaysResult += LocateWays[k].Substring(0, 1).ToUpper() + BuyWaysResult.ToString() + ",";
                    }
                }

                if (WaysResult.Length < 2)   //至少有一种过关方式 例如：11：表示单关，1倍
                {
                    CanonicalNumber = "";
                    return null;
                }

                WaysResult = FilterRepeatedWaysResult(WaysResult.Substring(0, WaysResult.Length - 1));
                WaysNumber = 0; //将原来的购买方式倍数清0

                //复式变单式，并取得购买注数和相应的倍数
                ArrayList all = new ArrayList();
                GamesNumber = CanonicalNumber.Split('|').Length;    //重新获取分析过后的比赛场次
                WaysNumber = WaysResult.Split(',').Length;  //重新获取分析过后的购买方式及倍数
                if (GamesNumber == 1)
                {
                    string[] LocateBuyResult = new string[GamesNumber];
                    string[] Screenings = new string[GamesNumber];

                    for (int i = 0; i < GamesNumber; i++)
                    {
                        Locate[i] = CanonicalNumber.Split('|')[i].ToString();

                        LocateBuyResult[i] = Locate[i].Substring(Locate[i].IndexOf('(') + 1, (Locate[i].IndexOf(')') - Locate[i].IndexOf('(') - 1));

                        Screenings[i] = Locate[i].Substring(0, Locate[i].IndexOf('('));
                    }

                    #region 获取买法倍数
                    string strWays = WaysResult.Substring(0, 1);
                    if (WaysNumber > 1)
                    {
                        CanonicalNumber = "";
                        return null;
                    }
                    else if (strWays != "1")
                    {
                        CanonicalNumber = "";
                        return null;
                    }

                    TempWaysMultiples = 0;
                    try
                    {
                        TempWaysMultiples = Convert.ToInt32(WaysResult.Substring(1, (WaysResult.Length - 1)));
                    }
                    catch
                    {
                        TempWaysMultiples = 0;
                    }

                    if (TempWaysMultiples <= 0)
                    {
                        CanonicalNumber = "";
                        return null;
                    }
                    #endregion

                    //取1关，并返回购买注数和购买倍数
                    string[] sg1 = getAll1G(GamesNumber, Locate, LocateBuyResult, Screenings, TempWaysMultiples);
                    for (int i = 0; i < sg1.Length; i++)
                    {
                        all.Add(sg1[i].ToString());
                    }
                }
                else if (GamesNumber > 1)
                {
                    string[] LocateBuyResult = new string[GamesNumber];
                    string[] Screenings = new string[GamesNumber];

                    for (int i = 0; i < GamesNumber; i++)
                    {
                        Locate[i] = CanonicalNumber.Split('|')[i].ToString();

                        LocateBuyResult[i] = Locate[i].Substring(Locate[i].IndexOf('(') + 1, (Locate[i].IndexOf(')') - Locate[i].IndexOf('(') - 1));

                        Screenings[i] = Locate[i].Substring(0, Locate[i].IndexOf('('));
                    }

                    if ((strPlayType == "4502" || strPlayType == "4503" || strPlayType == "4505") && (GamesNumber > 6))
                    {
                        CanonicalNumber = "";
                        return null;
                    }

                    if ((strPlayType == "4504") && (GamesNumber > 3))
                    {
                        CanonicalNumber = "";
                        return null;
                    }

                    string[] LocateWaysAndMultiples = new string[WaysNumber];   //购买方式及倍数
                    string[] LocateWaysType = new string[WaysNumber];   //购买方式
                    string[] WaysMultiples = new string[WaysNumber];    //购买倍数

                    for (int i = 0; i < WaysNumber; i++)
                    {
                        LocateWaysAndMultiples[i] = WaysResult.Split(',')[i].ToString();
                        LocateWaysType[i] = LocateWaysAndMultiples[i].Substring(0, 1);
                        WaysMultiples[i] = LocateWaysAndMultiples[i].Substring(1, (LocateWaysAndMultiples[i].Length - 1));

                        #region     获取方案某一个购买方式的购买注数和倍数
                        switch (LocateWaysType[i])
                        {
                            case "1":   //单关购买注数和对应倍数
                                string[] sg1 = getAll1G(GamesNumber, Locate, LocateBuyResult, Screenings, int.Parse(WaysMultiples[i].ToString()));
                                for (int j = 0; j < sg1.Length; j++)
                                {
                                    all.Add(sg1[j].ToString());
                                }
                                break;
                            case "2":   //双关购买注数和对应倍数
                                string[] sg2 = getAll2G(GamesNumber, Locate, LocateBuyResult, Screenings, int.Parse(WaysMultiples[i].ToString()));
                                for (int j = 0; j < sg2.Length; j++)
                                {
                                    all.Add(sg2[j].ToString());
                                }
                                break;
                            case "3":   //三关购买注数和对应倍数
                                string[] sg3 = getAll3G(GamesNumber, Locate, LocateBuyResult, Screenings, int.Parse(WaysMultiples[i].ToString()));
                                for (int j = 0; j < sg3.Length; j++)
                                {
                                    all.Add(sg3[j].ToString());
                                }
                                break;
                            case "4":   //2串1购买注数和对应倍数
                                string[] sg2c1 = getAll2G(GamesNumber, Locate, LocateBuyResult, Screenings, int.Parse(WaysMultiples[i].ToString()));
                                for (int j = 0; j < sg2c1.Length; j++)
                                {
                                    all.Add(sg2c1[j].ToString());
                                }
                                break;
                            case "5":   //2串3购买注数和对应倍数
                                string[] sg1_2c3 = getAll1G(GamesNumber, Locate, LocateBuyResult, Screenings, int.Parse(WaysMultiples[i].ToString()));
                                for (int j = 0; j < sg1_2c3.Length; j++)
                                {
                                    all.Add(sg1_2c3[j].ToString());
                                }

                                string[] sg2_2c3 = getAll2G(GamesNumber, Locate, LocateBuyResult, Screenings, int.Parse(WaysMultiples[i].ToString()));
                                for (int j = 0; j < sg2_2c3.Length; j++)
                                {
                                    all.Add(sg2_2c3[j].ToString());
                                }
                                break;
                            case "6":   //3串1购买注数和对应倍数
                                string[] sg3_3c1 = getAll3G(GamesNumber, Locate, LocateBuyResult, Screenings, int.Parse(WaysMultiples[i].ToString()));
                                for (int j = 0; j < sg3_3c1.Length; j++)
                                {
                                    all.Add(sg3_3c1[j].ToString());
                                }
                                break;
                            case "7":   //3串4购买注数和对应倍数
                                string[] sg2_3c4 = getAll2G(GamesNumber, Locate, LocateBuyResult, Screenings, int.Parse(WaysMultiples[i].ToString()));
                                for (int j = 0; j < sg2_3c4.Length; j++)
                                {
                                    all.Add(sg2_3c4[j].ToString());
                                }

                                string[] sg3_3c4 = getAll3G(GamesNumber, Locate, LocateBuyResult, Screenings, int.Parse(WaysMultiples[i].ToString()));
                                for (int j = 0; j < sg3_3c4.Length; j++)
                                {
                                    all.Add(sg3_3c4[j].ToString());
                                }
                                break;
                            case "8":    //3串7购买注数和对应倍数
                                string[] sg1_3c7 = getAll1G(GamesNumber, Locate, LocateBuyResult, Screenings, int.Parse(WaysMultiples[i].ToString()));
                                for (int j = 0; j < sg1_3c7.Length; j++)
                                {
                                    all.Add(sg1_3c7[j].ToString());
                                }

                                string[] sg2_3c7 = getAll2G(GamesNumber, Locate, LocateBuyResult, Screenings, int.Parse(WaysMultiples[i].ToString()));
                                for (int j = 0; j < sg2_3c7.Length; j++)
                                {
                                    all.Add(sg2_3c7[j].ToString());
                                }

                                string[] sg3_3c7 = getAll3G(GamesNumber, Locate, LocateBuyResult, Screenings, int.Parse(WaysMultiples[i].ToString()));
                                for (int j = 0; j < sg3_3c7.Length; j++)
                                {
                                    all.Add(sg3_3c7[j].ToString());
                                }
                                break;
                            case "9":    //4串1购买注数和对应倍数
                                string[] sg4_4c1 = getAll4G(GamesNumber, Locate, LocateBuyResult, Screenings, int.Parse(WaysMultiples[i].ToString()));
                                for (int j = 0; j < sg4_4c1.Length; j++)
                                {
                                    all.Add(sg4_4c1[j].ToString());
                                }
                                break;
                            case "A":   //4串5购买注数和对应倍数
                                string[] sg3_4c5 = getAll3G(GamesNumber, Locate, LocateBuyResult, Screenings, int.Parse(WaysMultiples[i].ToString()));
                                for (int j = 0; j < sg3_4c5.Length; j++)
                                {
                                    all.Add(sg3_4c5[j].ToString());
                                }

                                string[] sg4_4c5 = getAll4G(GamesNumber, Locate, LocateBuyResult, Screenings, int.Parse(WaysMultiples[i].ToString()));
                                for (int j = 0; j < sg4_4c5.Length; j++)
                                {
                                    all.Add(sg4_4c5[j].ToString());
                                }
                                break;
                            case "B":   //4串11购买注数和对应倍数
                                string[] sg2_4c11 = getAll2G(GamesNumber, Locate, LocateBuyResult, Screenings, int.Parse(WaysMultiples[i].ToString()));
                                for (int j = 0; j < sg2_4c11.Length; j++)
                                {
                                    all.Add(sg2_4c11[j].ToString());
                                }
                                string[] sg3_4c11 = getAll3G(GamesNumber, Locate, LocateBuyResult, Screenings, int.Parse(WaysMultiples[i].ToString()));
                                for (int j = 0; j < sg3_4c11.Length; j++)
                                {
                                    all.Add(sg3_4c11[j].ToString());
                                }

                                string[] sg4_4c11 = getAll4G(GamesNumber, Locate, LocateBuyResult, Screenings, int.Parse(WaysMultiples[i].ToString()));
                                for (int j = 0; j < sg4_4c11.Length; j++)
                                {
                                    all.Add(sg4_4c11[j].ToString());
                                }
                                break;
                            case "C":   //4串15购买注数和对应倍数
                                string[] sg1_4c15 = getAll1G(GamesNumber, Locate, LocateBuyResult, Screenings, int.Parse(WaysMultiples[i].ToString()));
                                for (int j = 0; j < sg1_4c15.Length; j++)
                                {
                                    all.Add(sg1_4c15[j].ToString());
                                }

                                string[] sg2_4c15 = getAll2G(GamesNumber, Locate, LocateBuyResult, Screenings, int.Parse(WaysMultiples[i].ToString()));
                                for (int j = 0; j < sg2_4c15.Length; j++)
                                {
                                    all.Add(sg2_4c15[j].ToString());
                                }

                                string[] sg3_4c15 = getAll3G(GamesNumber, Locate, LocateBuyResult, Screenings, int.Parse(WaysMultiples[i].ToString()));
                                for (int j = 0; j < sg3_4c15.Length; j++)
                                {
                                    all.Add(sg3_4c15[j].ToString());
                                }

                                string[] sg4_4c15 = getAll4G(GamesNumber, Locate, LocateBuyResult, Screenings, int.Parse(WaysMultiples[i].ToString()));
                                for (int j = 0; j < sg4_4c15.Length; j++)
                                {
                                    all.Add(sg4_4c15[j].ToString());
                                }
                                break;
                            case "D":   //5串1购买注数和对应倍数
                                string[] sg5_5c1 = getAll5G(GamesNumber, Locate, LocateBuyResult, Screenings, int.Parse(WaysMultiples[i].ToString()));
                                for (int j = 0; j < sg5_5c1.Length; j++)
                                {
                                    all.Add(sg5_5c1[j].ToString());
                                }
                                break;
                            case "E":   //5串6购买注数和对应倍数
                                string[] sg4_5c6 = getAll4G(GamesNumber, Locate, LocateBuyResult, Screenings, int.Parse(WaysMultiples[i].ToString()));
                                for (int j = 0; j < sg4_5c6.Length; j++)
                                {
                                    all.Add(sg4_5c6[j].ToString());
                                }

                                string[] sg5_5c6 = getAll5G(GamesNumber, Locate, LocateBuyResult, Screenings, int.Parse(WaysMultiples[i].ToString()));
                                for (int j = 0; j < sg5_5c6.Length; j++)
                                {
                                    all.Add(sg5_5c6[j].ToString());
                                }
                                break;
                            case "F":   //5串16购买注数和对应倍数
                                string[] sg3_5c16 = getAll3G(GamesNumber, Locate, LocateBuyResult, Screenings, int.Parse(WaysMultiples[i].ToString()));
                                for (int j = 0; j < sg3_5c16.Length; j++)
                                {
                                    all.Add(sg3_5c16[j].ToString());
                                }

                                string[] sg4_5c16 = getAll4G(GamesNumber, Locate, LocateBuyResult, Screenings, int.Parse(WaysMultiples[i].ToString()));
                                for (int j = 0; j < sg4_5c16.Length; j++)
                                {
                                    all.Add(sg4_5c16[j].ToString());
                                }

                                string[] sg5_5c16 = getAll5G(GamesNumber, Locate, LocateBuyResult, Screenings, int.Parse(WaysMultiples[i].ToString()));
                                for (int j = 0; j < sg5_5c16.Length; j++)
                                {
                                    all.Add(sg5_5c16[j].ToString());
                                }
                                break;
                            case "G":   //5串26购买注数和对应倍数
                                string[] sg2_5c26 = getAll2G(GamesNumber, Locate, LocateBuyResult, Screenings, int.Parse(WaysMultiples[i].ToString()));
                                for (int j = 0; j < sg2_5c26.Length; j++)
                                {
                                    all.Add(sg2_5c26[j].ToString());
                                }

                                string[] sg3_5c26 = getAll3G(GamesNumber, Locate, LocateBuyResult, Screenings, int.Parse(WaysMultiples[i].ToString()));
                                for (int j = 0; j < sg3_5c26.Length; j++)
                                {
                                    all.Add(sg3_5c26[j].ToString());
                                }

                                string[] sg4_5c26 = getAll4G(GamesNumber, Locate, LocateBuyResult, Screenings, int.Parse(WaysMultiples[i].ToString()));
                                for (int j = 0; j < sg4_5c26.Length; j++)
                                {
                                    all.Add(sg4_5c26[j].ToString());
                                }

                                string[] sg5_5c26 = getAll5G(GamesNumber, Locate, LocateBuyResult, Screenings, int.Parse(WaysMultiples[i].ToString()));
                                for (int j = 0; j < sg5_5c26.Length; j++)
                                {
                                    all.Add(sg5_5c26[j].ToString());
                                }
                                break;
                            case "H":   //5串31购买注数和对应倍数
                                string[] sg1_5c31 = getAll1G(GamesNumber, Locate, LocateBuyResult, Screenings, int.Parse(WaysMultiples[i].ToString()));
                                for (int j = 0; j < sg1_5c31.Length; j++)
                                {
                                    all.Add(sg1_5c31[j].ToString());
                                }

                                string[] sg2_5c31 = getAll2G(GamesNumber, Locate, LocateBuyResult, Screenings, int.Parse(WaysMultiples[i].ToString()));
                                for (int j = 0; j < sg2_5c31.Length; j++)
                                {
                                    all.Add(sg2_5c31[j].ToString());
                                }

                                string[] sg3_5c31 = getAll3G(GamesNumber, Locate, LocateBuyResult, Screenings, int.Parse(WaysMultiples[i].ToString()));
                                for (int j = 0; j < sg3_5c31.Length; j++)
                                {
                                    all.Add(sg3_5c31[j].ToString());
                                }

                                string[] sg4_5c31 = getAll4G(GamesNumber, Locate, LocateBuyResult, Screenings, int.Parse(WaysMultiples[i].ToString()));
                                for (int j = 0; j < sg4_5c31.Length; j++)
                                {
                                    all.Add(sg4_5c31[j].ToString());
                                }

                                string[] sg5_5c31 = getAll5G(GamesNumber, Locate, LocateBuyResult, Screenings, int.Parse(WaysMultiples[i].ToString()));
                                for (int j = 0; j < sg5_5c31.Length; j++)
                                {
                                    all.Add(sg5_5c31[j].ToString());
                                }
                                break;
                            case "I":   //6串1购买注数和对应倍数
                                string[] sg6_6c1 = getAll6G(GamesNumber, Locate, LocateBuyResult, Screenings, int.Parse(WaysMultiples[i].ToString()));
                                for (int j = 0; j < sg6_6c1.Length; j++)
                                {
                                    all.Add(sg6_6c1[j].ToString());
                                }
                                break;
                            case "J":   //6串7购买注数和对应倍数
                                string[] sg5_6c7 = getAll5G(GamesNumber, Locate, LocateBuyResult, Screenings, int.Parse(WaysMultiples[i].ToString()));
                                for (int j = 0; j < sg5_6c7.Length; j++)
                                {
                                    all.Add(sg5_6c7[j].ToString());
                                }

                                string[] sg6_6c7 = getAll6G(GamesNumber, Locate, LocateBuyResult, Screenings, int.Parse(WaysMultiples[i].ToString()));
                                for (int j = 0; j < sg6_6c7.Length; j++)
                                {
                                    all.Add(sg6_6c7[j].ToString());
                                }
                                break;
                            case "K":   //6串22购买注数和对应倍数
                                string[] sg4_6c22 = getAll4G(GamesNumber, Locate, LocateBuyResult, Screenings, int.Parse(WaysMultiples[i].ToString()));
                                for (int j = 0; j < sg4_6c22.Length; j++)
                                {
                                    all.Add(sg4_6c22[j].ToString());
                                }

                                string[] sg5_6c22 = getAll5G(GamesNumber, Locate, LocateBuyResult, Screenings, int.Parse(WaysMultiples[i].ToString()));
                                for (int j = 0; j < sg5_6c22.Length; j++)
                                {
                                    all.Add(sg5_6c22[j].ToString());
                                }

                                string[] sg6_6c22 = getAll6G(GamesNumber, Locate, LocateBuyResult, Screenings, int.Parse(WaysMultiples[i].ToString()));
                                for (int j = 0; j < sg6_6c22.Length; j++)
                                {
                                    all.Add(sg6_6c22[j].ToString());
                                }
                                break;
                            case "L":   //6串42购买注数和对应倍数
                                string[] sg3_6c42 = getAll3G(GamesNumber, Locate, LocateBuyResult, Screenings, int.Parse(WaysMultiples[i].ToString()));
                                for (int j = 0; j < sg3_6c42.Length; j++)
                                {
                                    all.Add(sg3_6c42[j].ToString());
                                }

                                string[] sg4_6c42 = getAll4G(GamesNumber, Locate, LocateBuyResult, Screenings, int.Parse(WaysMultiples[i].ToString()));
                                for (int j = 0; j < sg4_6c42.Length; j++)
                                {
                                    all.Add(sg4_6c42[j].ToString());
                                }

                                string[] sg5_6c42 = getAll5G(GamesNumber, Locate, LocateBuyResult, Screenings, int.Parse(WaysMultiples[i].ToString()));
                                for (int j = 0; j < sg5_6c42.Length; j++)
                                {
                                    all.Add(sg5_6c42[j].ToString());
                                }

                                string[] sg6_6c42 = getAll6G(GamesNumber, Locate, LocateBuyResult, Screenings, int.Parse(WaysMultiples[i].ToString()));
                                for (int j = 0; j < sg6_6c42.Length; j++)
                                {
                                    all.Add(sg6_6c42[j].ToString());
                                }
                                break;
                            case "M":   //6串57购买注数和对应倍数
                                string[] sg2_6c57 = getAll2G(GamesNumber, Locate, LocateBuyResult, Screenings, int.Parse(WaysMultiples[i].ToString()));
                                for (int j = 0; j < sg2_6c57.Length; j++)
                                {
                                    all.Add(sg2_6c57[j].ToString());
                                }

                                string[] sg3_6c57 = getAll3G(GamesNumber, Locate, LocateBuyResult, Screenings, int.Parse(WaysMultiples[i].ToString()));
                                for (int j = 0; j < sg3_6c57.Length; j++)
                                {
                                    all.Add(sg3_6c57[j].ToString());
                                }

                                string[] sg4_6c57 = getAll4G(GamesNumber, Locate, LocateBuyResult, Screenings, int.Parse(WaysMultiples[i].ToString()));
                                for (int j = 0; j < sg4_6c57.Length; j++)
                                {
                                    all.Add(sg4_6c57[j].ToString());
                                }

                                string[] sg5_6c57 = getAll5G(GamesNumber, Locate, LocateBuyResult, Screenings, int.Parse(WaysMultiples[i].ToString()));
                                for (int j = 0; j < sg5_6c57.Length; j++)
                                {
                                    all.Add(sg5_6c57[j].ToString());
                                }

                                string[] sg6_6c57 = getAll6G(GamesNumber, Locate, LocateBuyResult, Screenings, int.Parse(WaysMultiples[i].ToString()));
                                for (int j = 0; j < sg6_6c57.Length; j++)
                                {
                                    all.Add(sg6_6c57[j].ToString());
                                }
                                break;
                            case "N":   //6串63购买注数和对应倍数
                                string[] sg1_6c63 = getAll1G(GamesNumber, Locate, LocateBuyResult, Screenings, int.Parse(WaysMultiples[i].ToString()));
                                for (int j = 0; j < sg1_6c63.Length; j++)
                                {
                                    all.Add(sg1_6c63[j].ToString());
                                }

                                string[] sg2_6c63 = getAll2G(GamesNumber, Locate, LocateBuyResult, Screenings, int.Parse(WaysMultiples[i].ToString()));
                                for (int j = 0; j < sg2_6c63.Length; j++)
                                {
                                    all.Add(sg2_6c63[j].ToString());
                                }

                                string[] sg3_6c63 = getAll3G(GamesNumber, Locate, LocateBuyResult, Screenings, int.Parse(WaysMultiples[i].ToString()));
                                for (int j = 0; j < sg3_6c63.Length; j++)
                                {
                                    all.Add(sg3_6c63[j].ToString());
                                }

                                string[] sg4_6c63 = getAll4G(GamesNumber, Locate, LocateBuyResult, Screenings, int.Parse(WaysMultiples[i].ToString()));
                                for (int j = 0; j < sg4_6c63.Length; j++)
                                {
                                    all.Add(sg4_6c63[j].ToString());
                                }

                                string[] sg5_6c63 = getAll5G(GamesNumber, Locate, LocateBuyResult, Screenings, int.Parse(WaysMultiples[i].ToString()));
                                for (int j = 0; j < sg5_6c63.Length; j++)
                                {
                                    all.Add(sg5_6c63[j].ToString());
                                }

                                string[] sg6_6c63 = getAll6G(GamesNumber, Locate, LocateBuyResult, Screenings, int.Parse(WaysMultiples[i].ToString()));
                                for (int j = 0; j < sg6_6c63.Length; j++)
                                {
                                    all.Add(sg6_6c63[j].ToString());
                                }
                                break;
                            default:    //7-15串1购买注数和对应倍数
                                string[] mc1 = getAllMC1(GamesNumber, Locate, LocateBuyResult, Screenings, int.Parse(WaysMultiples[i].ToString()));
                                for (int j = 0; j < mc1.Length; j++)
                                {
                                    all.Add(mc1[j].ToString());
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
                    for (int j = 0; j < LocateBuyResult[i].Split(',').Length; j++)
                    {
                        al.Add(Screenings[i] + "(" + LocateBuyResult[i].Split(',')[j].ToString() + ");" + TempWaysMultiples);
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

            private string[] getAllMC1(int GamesNumber, string[] Locate, string[] LocateBuyResult, string[] Screenings, int TempWaysMultiples)
            {
                ArrayList al = new ArrayList(); //取所有的六关，先存入ArrayList对象
                int Temp = 0;
                for (int i_0 = 0; i_0 < LocateBuyResult[Temp].Split(',').Length; i_0++)
                {
                    string str_0 = Screenings[Temp] + "(" + LocateBuyResult[Temp].Split(',')[i_0].ToString() + ")|";
                    int Temp1 = Temp + 1;
                    if (Temp1 < GamesNumber)
                    {
                        for (int i_1 = 0; i_1 < LocateBuyResult[Temp1].Split(',').Length; i_1++)
                        {
                            string str_1 = str_0 + Screenings[Temp1] + "(" + LocateBuyResult[Temp1].Split(',')[i_1].ToString() + ")|";
                            int Temp2 = Temp1 + 1;
                            if (Temp2 < GamesNumber)
                            {
                                for (int i_2 = 0; i_2 < LocateBuyResult[Temp2].Split(',').Length; i_2++)
                                {
                                    string str_2 = str_1 + Screenings[Temp2] + "(" + LocateBuyResult[Temp2].Split(',')[i_2].ToString() + ")|";
                                    int Temp3 = Temp2 + 1;
                                    if (Temp3 < GamesNumber)
                                    {
                                        for (int i_3 = 0; i_3 < LocateBuyResult[Temp3].Split(',').Length; i_3++)
                                        {
                                            string str_3 = str_2 + Screenings[Temp3] + "(" + LocateBuyResult[Temp3].Split(',')[i_3].ToString() + ")|";
                                            int Temp4 = Temp3 + 1;
                                            if (Temp4 < GamesNumber)
                                            {
                                                for (int i_4 = 0; i_4 < LocateBuyResult[Temp4].Split(',').Length; i_4++)
                                                {
                                                    string str_4 = str_3 + Screenings[Temp4] + "(" + LocateBuyResult[Temp4].Split(',')[i_4].ToString() + ")|";
                                                    int Temp5 = Temp4 + 1;
                                                    if (Temp5 < GamesNumber)
                                                    {
                                                        for (int i_5 = 0; i_5 < LocateBuyResult[Temp5].Split(',').Length; i_5++)
                                                        {
                                                            string str_5 = str_4 + Screenings[Temp5] + "(" + LocateBuyResult[Temp5].Split(',')[i_5].ToString() + ")|";
                                                            int Temp6 = Temp5 + 1;
                                                            if (Temp6 < GamesNumber)
                                                            {
                                                                for (int i_6 = 0; i_6 < LocateBuyResult[Temp6].Split(',').Length; i_6++)
                                                                {
                                                                    string str_6 = str_5 + Screenings[Temp6] + "(" + LocateBuyResult[Temp6].Split(',')[i_6].ToString() + ")|";
                                                                    int Temp7 = Temp6 + 1;
                                                                    if (Temp7 < GamesNumber)
                                                                    {
                                                                        for (int i_7 = 0; i_7 < LocateBuyResult[Temp7].Split(',').Length; i_7++)
                                                                        {
                                                                            string str_7 = str_6 + Screenings[Temp7] + "(" + LocateBuyResult[Temp7].Split(',')[i_7].ToString() + ")|";
                                                                            int Temp8 = Temp7 + 1;
                                                                            if (Temp8 < GamesNumber)
                                                                            {
                                                                                for (int i_8 = 0; i_8 < LocateBuyResult[Temp8].Split(',').Length; i_8++)
                                                                                {
                                                                                    string str_8 = str_7 + Screenings[Temp8] + "(" + LocateBuyResult[Temp8].Split(',')[i_8].ToString() + ")|";
                                                                                    int Temp9 = Temp8 + 1;
                                                                                    if (Temp9 < GamesNumber)
                                                                                    {
                                                                                        for (int i_9 = 0; i_9 < LocateBuyResult[Temp9].Split(',').Length; i_9++)
                                                                                        {
                                                                                            string str_9 = str_8 + Screenings[Temp9] + "(" + LocateBuyResult[Temp9].Split(',')[i_9].ToString() + ")|";
                                                                                            int Temp10 = Temp9 + 1;
                                                                                            if (Temp10 < GamesNumber)
                                                                                            {
                                                                                                for (int i_10 = 0; i_10 < LocateBuyResult[Temp10].Split(',').Length; i_10++)
                                                                                                {
                                                                                                    string str_10 = str_9 + Screenings[Temp10] + "(" + LocateBuyResult[Temp10].Split(',')[i_10].ToString() + ")|";
                                                                                                    int Temp11 = Temp10 + 1;
                                                                                                    if (Temp11 < GamesNumber)
                                                                                                    {
                                                                                                        for (int i_11 = 0; i_11 < LocateBuyResult[Temp11].Split(',').Length; i_11++)
                                                                                                        {
                                                                                                            string str_11 = str_10 + Screenings[Temp11] + "(" + LocateBuyResult[Temp11].Split(',')[i_11].ToString() + ")|";
                                                                                                            int Temp12 = Temp11 + 1;
                                                                                                            if (Temp12 < GamesNumber)
                                                                                                            {
                                                                                                                for (int i_12 = 0; i_12 < LocateBuyResult[Temp12].Split(',').Length; i_12++)
                                                                                                                {
                                                                                                                    string str_12 = str_11 + Screenings[Temp12] + "(" + LocateBuyResult[Temp12].Split(',')[i_12].ToString() + ")|";
                                                                                                                    int Temp13 = Temp12 + 1;
                                                                                                                    if (Temp13 < GamesNumber)
                                                                                                                    {
                                                                                                                        for (int i_13 = 0; i_13 < LocateBuyResult[Temp13].Split(',').Length; i_13++)
                                                                                                                        {
                                                                                                                            string str_13 = str_12 + Screenings[Temp13] + "(" + LocateBuyResult[Temp13].Split(',')[i_13].ToString() + ")|";
                                                                                                                            int Temp14 = Temp13 + 1;
                                                                                                                            if (Temp14 < GamesNumber)
                                                                                                                            {
                                                                                                                                for (int i_14 = 0; i_14 < LocateBuyResult[Temp14].Split(',').Length; i_14++)
                                                                                                                                {
                                                                                                                                    string str_14 = str_13 + Screenings[Temp14] + "(" + LocateBuyResult[Temp14].Split(',')[i_14].ToString() + ")|";
                                                                                                                                    al.Add(str_14.Substring(0, str_14.Length - 1) + ";" + TempWaysMultiples);
                                                                                                                                }  //14    
                                                                                                                            }
                                                                                                                            else
                                                                                                                            {
                                                                                                                                al.Add(str_13.Substring(0, str_13.Length - 1) + ";" + TempWaysMultiples);
                                                                                                                            }
                                                                                                                        }   //13
                                                                                                                    }
                                                                                                                    else
                                                                                                                    {
                                                                                                                        al.Add(str_12.Substring(0, str_12.Length - 1) + ";" + TempWaysMultiples);
                                                                                                                    }
                                                                                                                }   //12
                                                                                                            }
                                                                                                            else
                                                                                                            {
                                                                                                                al.Add(str_11.Substring(0, str_11.Length - 1) + ";" + TempWaysMultiples);
                                                                                                            }
                                                                                                        }   //11
                                                                                                    }
                                                                                                    else
                                                                                                    {
                                                                                                        al.Add(str_10.Substring(0, str_10.Length - 1) + ";" + TempWaysMultiples);
                                                                                                    }
                                                                                                }   //10
                                                                                            }
                                                                                            else
                                                                                            {
                                                                                                al.Add(str_9.Substring(0, str_9.Length - 1) + ";" + TempWaysMultiples);
                                                                                            }
                                                                                        }   //9
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        al.Add(str_8.Substring(0, str_8.Length - 1) + ";" + TempWaysMultiples);
                                                                                    }
                                                                                }   //8
                                                                            }
                                                                            else
                                                                            {
                                                                                al.Add(str_7.Substring(0, str_7.Length - 1) + ";" + TempWaysMultiples);
                                                                            }
                                                                        }   //7
                                                                    }
                                                                    else
                                                                    {
                                                                        al.Add(str_6.Substring(0, str_6.Length - 1) + ";" + TempWaysMultiples);
                                                                    }
                                                                }   //6
                                                            }
                                                            else
                                                            {
                                                                al.Add(str_5.Substring(0, str_5.Length - 1) + ";" + TempWaysMultiples);
                                                            }
                                                        }   //5
                                                    }
                                                    else
                                                    {
                                                        al.Add(str_4.Substring(0, str_4.Length - 1) + ";" + TempWaysMultiples);
                                                    }
                                                }   //4
                                            }
                                            else
                                            {
                                                al.Add(str_3.Substring(0, str_3.Length - 1) + ";" + TempWaysMultiples);
                                            }
                                        }   //3
                                    }
                                    else
                                    {
                                        al.Add(str_2.Substring(0, str_2.Length - 1) + ";" + TempWaysMultiples);
                                    }
                                }   //2
                            }
                            else
                            {
                                al.Add(str_1.Substring(0, str_1.Length - 1) + ";" + TempWaysMultiples);
                            }
                        }   //1
                    }
                    else
                    {
                        al.Add(str_0.Substring(0, str_0.Length - 1) + ";" + TempWaysMultiples);
                    }
                }   //0

                string[] Result = new string[al.Count];
                for (int i = 0; i < al.Count; i++)
                {
                    Result[i] = al[i].ToString();
                }

                return Result;
            }
            #endregion

            //(Number表示传入的方案，CanonicalNumber表示整理后的方案，PlayType购买类型)复式取单式 CompetitionCount 当期比赛的总场数
            public override double ComputeWin(string Scheme, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, int PlayType, int CompetitionCount, string NoSignificance)
            {
                if (Scheme.Length < 16) //每个方案最短长度为 例如：4501;[5(3)];[11]
                    return -3;

                if ((PlayType < 4501) || (PlayType > 4505))
                {
                    return -2;
                }

                if ((PlayType == 4501) || (PlayType == 4502) || (PlayType == 4503) || (PlayType == 4504) || (PlayType == 4505))
                {
                    return ComputeWinMethods(Scheme, WinNumber, ref Description, ref WinMoneyNoWithTax, PlayType, CompetitionCount);
                }

                return -4;
            }
            #region ComputeWin 的具体方法
            private double ComputeWinMethods(string Scheme, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, int PlayType, int CompetitionCount)	//计算中奖
            {
                //WinNumber--彩票开奖结果格式为（顺序为－4501(胜平负);4502(总进球);4503(上下单双);4504(正确比分);4505(半全场胜平负)）：场次1(结果,SP值)|场次2(结果,SP值);场次1(结果,SP值)|场次2(结果,SP值) 我们规定结果全部用数字表示
                string CanonicalNumber = "";
                string[] BuyResult = ToSingle(Scheme, ref CanonicalNumber, CompetitionCount);   //ToSingle(string Scheme, ref string CanonicalNumber, int CompetitionCount)
                if (BuyResult == null || BuyResult.Length < 1)
                {
                    return -2;
                }

                int WaysMultiples = 0;   //购买倍数
                int ResultLength = 0;    //购买的某注的长度，用来判断是单场还是过关
                double WinMoney = 0;
                double SpIndex = 1.0;    //比赛开奖的SP值
                double TempSpValue = 1.0;     //统计比赛开奖的SP值
                string Lottery = "";     //每场比赛

                bool ComparisonResult = false;
                Description = "";
                int intright = 0;   //中奖场数
                int intRefund = 0;  //退票处理的场数

                for (int ii = 0; ii < BuyResult.Length; ii++)
                {
                    ComparisonResult = false;
                    Lottery = BuyResult[ii].Split(';')[0];

                    try
                    {
                        WaysMultiples = int.Parse(BuyResult[ii].Split(';')[1].ToString()); //获得本注的购买倍数
                    }
                    catch
                    {
                        WaysMultiples = 0;
                    }

                    TempSpValue = 1;
                    string[] Result = Lottery.Split('|');
                    ResultLength = Result.Length;
                    for (int i = 0; i < ResultLength; i++)
                    {
                        if (Result[i].Length < 4)
                        {
                            continue;
                        }

                        int BuyNumberOfShowings = 0;
                        try
                        {
                            BuyNumberOfShowings = int.Parse(Result[i].Substring(0, Result[i].IndexOf('(')).ToString());
                        }
                        catch
                        {
                            BuyNumberOfShowings = 0;
                        }

                        string BuyLotteryTicketResult = Result[i].Substring(Result[i].IndexOf('(') + 1, (Result[i].IndexOf(')') - Result[i].IndexOf('(') - 1)).ToString();

                        SpIndex = 1.0;
                        ComparisonResult = ComparedWithResult(BuyNumberOfShowings, BuyLotteryTicketResult, WinNumber, PlayType, ResultLength, ref SpIndex);
                        if (ComparisonResult)
                        {
                            if ((Result.Length == 1) && (SpIndex < 1.538) && (SpIndex > 0))     //保底
                            {
                                SpIndex = 1.538;
                            }

                            TempSpValue = TempSpValue * Math.Round(SpIndex, 4);
                        }
                        else
                        {
                            break;
                        }
                    }

                    if (ComparisonResult)
                    {
                        if ((Result.Length == 1) && (TempSpValue == -1))    //退票相当与中奖保底法返还
                        {
                            WinMoney += 2 * WaysMultiples;
                            intRefund++;
                        }
                        else
                        {
                            intright++;
                            WinMoney += Math.Round(TempSpValue * 0.65, 4) * 2 * WaysMultiples;
                            /*
                             * 税后奖金
                             */
                            if (Math.Round(TempSpValue * 0.65, 4) * 2 >= 10000)
                            {
                                WinMoneyNoWithTax += Math.Round(TempSpValue * 0.65, 4) * 2 * WaysMultiples * 0.8;
                            }
                            else
                            {
                                WinMoneyNoWithTax += Math.Round(TempSpValue * 0.65, 4) * 2 * WaysMultiples;
                            }
                        }
                    }
                }

                if (intRefund > 0)
                {
                    if (Description != "")
                    {
                        Description += "，";
                    }
                    Description = "退票" + intRefund.ToString() + "注（场）";
                }

                if (intright > 0)
                {
                    if (Description != "")
                    {
                        Description += "，";
                    }
                    Description = "中奖" + intright.ToString() + "注";
                }

                WinMoneyNoWithTax = Math.Round(WinMoneyNoWithTax, 2);

                return Math.Round(WinMoney, 2);
            }
            private bool ComparedWithResult(int BuyNumberOfShowings, string BuyLotteryTicketResult, string WinNumber, int PlayType, int ResultLength, ref double SpIndex)
            {
                bool ComparedResult = false;
                string[] Result = WinNumber.Split(';');  //彩票开奖结果格式为（顺序为－4501;4502;4503;4504;4505）：场次1(结果,SP值)|场次2(结果,SP值);场次1(结果,SP值)|场次2(结果,SP值)
                string WinNumberList = "";
                if (PlayType == 4501)
                {
                    WinNumberList = Result[0];
                }
                else if (PlayType == 4502)
                {
                    WinNumberList = Result[1];
                }
                else if (PlayType == 4503)
                {
                    WinNumberList = Result[2];
                }
                else if (PlayType == 4504)
                {
                    WinNumberList = Result[3];
                }
                else if (PlayType == 4505)
                {
                    WinNumberList = Result[4];
                }
                else
                {
                    return ComparedResult;
                }

                string[] NumberOfShowingsData = WinNumberList.Split('|');
                if ((BuyNumberOfShowings - 1) > NumberOfShowingsData.Length)     //如果购买的场次大于当期的总场数SP值返回0
                {
                    SpIndex = 0;
                    ComparedResult = false;

                    return ComparedResult;
                }

                int intNumberOfShowings = int.Parse(NumberOfShowingsData[BuyNumberOfShowings - 1].Substring(0, NumberOfShowingsData[BuyNumberOfShowings - 1].IndexOf('(')).ToString());      //获得与某注对应的开奖结果中的比赛场次
                string LotteryTicketResult = NumberOfShowingsData[BuyNumberOfShowings - 1].Substring((NumberOfShowingsData[BuyNumberOfShowings - 1].IndexOf('(') + 1), (NumberOfShowingsData[BuyNumberOfShowings - 1].IndexOf(',') - NumberOfShowingsData[BuyNumberOfShowings - 1].IndexOf('(') - 1)).ToString();        //获得与某注对应的开奖结果中的比赛场次对应的开奖结果

                if ((BuyNumberOfShowings == intNumberOfShowings) && ((BuyLotteryTicketResult == LotteryTicketResult) || (LotteryTicketResult == "*")))   //比较购买的某注与开奖的场次和结果是否相同
                {
                    /*
                     * 比赛特殊情况处理
                     *（1）在当期单场玩法销售前，部分和全部场次因故发生变化，则更改相应场次变化。
                     *（2）在当期单场玩法销售后，如果部分场次因故提前进行，则该场比赛销售时间相应提前结束，其余场次销售时间不变；
                     * 如果部分场次因故延期，超过原定开奖时间12小时，则该场比赛在过关投注中的所有选项视为选中SP为1；单场投注按推票处理；
                     * 未超过12小时的，按实际比赛结果计奖。
                     *（3）在7日内办理退票手续
                     */
                    if ((ResultLength <= 1) && (LotteryTicketResult == "*"))     //单关的退票
                    {
                        SpIndex = -1;
                        ComparedResult = true;
                    }
                    else if ((ResultLength > 1) && (LotteryTicketResult == "*"))
                    {
                        SpIndex = 1;
                        ComparedResult = true;
                    }
                    else
                    {
                        //SpIndex = double.Parse(NumberOfShowingsData[BuyNumberOfShowings - 1].Substring((NumberOfShowingsData[BuyNumberOfShowings - 1].IndexOf(',') + 1), (NumberOfShowingsData[BuyNumberOfShowings - 1].IndexOf(')') - NumberOfShowingsData[BuyNumberOfShowings - 1].IndexOf(',') - 1)).ToString().Substring(0, NumberOfShowingsData[BuyNumberOfShowings - 1].Substring((NumberOfShowingsData[BuyNumberOfShowings - 1].IndexOf(',') + 1), (NumberOfShowingsData[BuyNumberOfShowings - 1].IndexOf(')') - NumberOfShowingsData[BuyNumberOfShowings - 1].IndexOf(',') - 1)).ToString().IndexOf('.') + 5));         //获得与某注对应的开奖结果中的比赛场次对应的SP值
                        SpIndex = double.Parse(NumberOfShowingsData[BuyNumberOfShowings - 1].Substring((NumberOfShowingsData[BuyNumberOfShowings - 1].IndexOf(',') + 1), (NumberOfShowingsData[BuyNumberOfShowings - 1].IndexOf(')') - NumberOfShowingsData[BuyNumberOfShowings - 1].IndexOf(',') - 1)).ToString());         //获得与某注对应的开奖结果中的比赛场次对应的SP值
                        ComparedResult = true;
                    }
                }

                return ComparedResult;
            }
            #endregion

            /// <summary>
            /// 分析方案是否格式正确
            /// </summary>
            /// <param name="Scheme">方案内容</param>
            /// <param name="CompetitionCount">当期比赛的总场数</param>
            /// <returns>返回方案的彩票注数|方案的最大倍数|方案总金额</returns>
            public override string AnalyseScheme(string Scheme, int CompetitionCount)
            {
                int maxMultiple = 0;

                //验证并获取注数
                string CanonicalNumber = "";
                string[] strs = ToSingle(Scheme, ref CanonicalNumber, CompetitionCount);

                if (strs == null || strs.Length == 0)
                    return "";

                for (int i = 0; i < strs.Length; i++)
                {
                    int temp = Shove._Convert.StrToInt(strs[i].Split(';')[1], 0);
                    maxMultiple = temp > maxMultiple ? temp : maxMultiple;
                }

                return strs.Length.ToString() + "|" + maxMultiple.ToString() + "|" + (strs.Length * maxMultiple * 2).ToString("N");
            }

            public override bool AnalyseWinNumber(string Number, int CompetitionCount)
            {
                string ConfirmationString = "";
                string ConfirmationString1 = "";
                string ConfirmationString2 = "";
                string ConfirmationString3 = "";
                string ConfirmationString4 = "";
                string ConfirmationString5 = "";

                //4501
                for (int i = 0; i < CompetitionCount; i++)
                {
                    ConfirmationString1 += @"^(?<L" + i.ToString() + @"^>[\d]{1,2}[(][310*][,][\d]{1,}([.][\d]{1,}){0,1}[)])[|]"; //方案格式：4501;[1(3)|5(3,1)|7(0)];[A2,B1]
                }
                ConfirmationString1 = ConfirmationString1.Substring(0, ConfirmationString1.Length - 3);

                //4502
                for (int i = 0; i < CompetitionCount; i++)
                {
                    ConfirmationString2 += @"^(?<L" + (CompetitionCount + i).ToString() + @"^>[\d]{1,2}[(][01234567*][,][\d]{1,}([.][\d]{1,}){0,1}[)])[|]";
                }
                ConfirmationString2 = ConfirmationString2.Substring(0, ConfirmationString2.Length - 3);

                //4503
                for (int i = 0; i < CompetitionCount; i++)
                {
                    ConfirmationString3 += @"^(?<L" + ((CompetitionCount * 2) + i).ToString() + @"^>[\d]{1,2}[(][1234*][,][\d]{1,}([.][\d]{1,}){0,1}[)])[|]";
                }
                ConfirmationString3 = ConfirmationString3.Substring(0, ConfirmationString3.Length - 3);

                //4504
                for (int i = 0; i < CompetitionCount; i++)
                {
                    ConfirmationString4 += @"^(?<L" + ((CompetitionCount * 3) + i).ToString() + @"^>[\d]{1,2}[(]([\d]{1,2}|[*])[,][\d]{1,}([.][\d]{1,}){0,1}[)])[|]";
                }
                ConfirmationString4 = ConfirmationString4.Substring(0, ConfirmationString4.Length - 3);

                //4505
                for (int i = 0; i < CompetitionCount; i++)
                {
                    ConfirmationString5 += @"^(?<L" + ((CompetitionCount * 4) + i).ToString() + @"^>[\d]{1,2}[(][123456789*][,][\d]{1,}([.][\d]{1,}){0,1}[)])[|]";
                }
                ConfirmationString5 = ConfirmationString5.Substring(0, ConfirmationString5.Length - 3);

                ConfirmationString = ConfirmationString1 + ";" + ConfirmationString2 + ";" + ConfirmationString3 + ";" + ConfirmationString4 + ";" + ConfirmationString5;

                Regex regex = new Regex(ConfirmationString, RegexOptions.Compiled | RegexOptions.IgnoreCase);
                if (!regex.IsMatch(Number))
                {
                    return false;
                }

                return true;
            }

            // 获得方案的方案的总注数
            public int GetBettingNumber(string Scheme, ref string CanonicalNumber, int CompetitionCount) //(Scheme表示传入的方案，CanonicalNumber表示整理后的方案，CompetitionCount 当期比赛的总场数)复式取单式
            {
                string[] BuyResult = ToSingle(Scheme, ref CanonicalNumber, CompetitionCount);
                if (BuyResult == null || BuyResult.Length < 1)
                {
                    return -2;
                }

                return BuyResult.Length;
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
                    if (("013".IndexOf(NumberPartSort[i]) >= 0) && (strPlayType == "4501"))
                    {
                        ReconstructionNumberPart += NumberPartSort[i] + ",";
                    }
                    else if (("01234567".IndexOf(NumberPartSort[i]) >= 0) && (strPlayType == "4502"))     //0-7
                    {
                        ReconstructionNumberPart += NumberPartSort[i] + ",";
                    }
                    else if (("1234".IndexOf(NumberPartSort[i]) >= 0) && (strPlayType == "4503"))     //1-4
                    {
                        ReconstructionNumberPart += NumberPartSort[i] + ",";
                    }
                    else if (strPlayType == "4504") //1-25
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
                        if ((TemNumberPart > 0) && (TemNumberPart < 26))
                        {
                            ReconstructionNumberPart += NumberPartSort[i] + ",";
                        }
                    }
                    else if (("123456789".IndexOf(NumberPartSort[i]) >= 0) && (strPlayType == "4505"))    //1-9
                    {
                        ReconstructionNumberPart += NumberPartSort[i] + ",";
                    }
                }

                ReconstructionNumberPart = ReconstructionNumberPart.Substring(0, ReconstructionNumberPart.Length - 1);
                return ReconstructionNumberPart;
            }

            // 方案场次排列
            private string FilterRepeatedScheme(string Scheme)
            {
                string[] SchemeSort = Scheme.Split('|');
                if (SchemeSort.Length == 1)
                {
                    return Scheme;
                }

                string TemSchemeSort = "";
                string ReconstructionScheme = "";
                int intScreeningsJ = 0;
                int intScreeningsJ1 = 0;
                for (int i = 0; i < SchemeSort.Length; i++)
                {
                    for (int j = 1; j < SchemeSort.Length - i; j++)
                    {
                        try
                        {
                            intScreeningsJ = int.Parse(SchemeSort[j].Substring(0, (SchemeSort[j].IndexOf('('))));
                        }
                        catch
                        {
                            intScreeningsJ = 0;
                        }

                        try
                        {
                            intScreeningsJ1 = int.Parse(SchemeSort[j - 1].Substring(0, (SchemeSort[j - 1].IndexOf('('))));
                        }
                        catch
                        {
                            intScreeningsJ1 = 0;
                        }

                        if (intScreeningsJ < intScreeningsJ1)
                        {
                            TemSchemeSort = SchemeSort[j - 1];
                            SchemeSort[j - 1] = SchemeSort[j];
                            SchemeSort[j] = TemSchemeSort;
                        }
                    }
                }

                for (int i = 0; i < SchemeSort.Length; i++)
                {
                    ReconstructionScheme += SchemeSort[i] + "|";
                }

                ReconstructionScheme = ReconstructionScheme.Substring(0, ReconstructionScheme.Length - 1);
                return ReconstructionScheme;
            }

            // 方案购买方式排列
            private string FilterRepeatedWaysResult(string WaysResult)
            {
                string[] WaysResultSort = WaysResult.Split(',');
                if (WaysResultSort.Length == 1)
                {
                    return WaysResult;
                }

                string TemWaysResultSort = "";
                string ReconstructionWaysResult = "";
                int intWaysResultJ = 0;
                int intWaysResultJ1 = 0;
                for (int i = 0; i < WaysResultSort.Length; i++)
                {
                    for (int j = 1; j < WaysResultSort.Length - i; j++)
                    {
                        try
                        {
                            intWaysResultJ = int.Parse(Shove._Convert.Asc(char.Parse(WaysResultSort[j].Substring(0, 1).ToString())).ToString());
                        }
                        catch
                        {
                            intWaysResultJ = 0;
                        }

                        try
                        {
                            intWaysResultJ1 = int.Parse(Shove._Convert.Asc(char.Parse(WaysResultSort[j - 1].Substring(0, 1).ToString())).ToString());
                        }
                        catch
                        {
                            intWaysResultJ1 = 0;
                        }

                        if (intWaysResultJ < intWaysResultJ1)
                        {
                            TemWaysResultSort = WaysResultSort[j - 1];
                            WaysResultSort[j - 1] = WaysResultSort[j];
                            WaysResultSort[j] = TemWaysResultSort;
                        }
                    }
                }

                for (int i = 0; i < WaysResultSort.Length; i++)
                {
                    ReconstructionWaysResult += WaysResultSort[i] + ",";
                }

                ReconstructionWaysResult = ReconstructionWaysResult.Substring(0, ReconstructionWaysResult.Length - 1);
                return ReconstructionWaysResult;
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
                if (SchemeLength != 3)
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
                    switch (LocateWaysAndMultiples[j].ToString().Substring(0, 1))
                    {
                        case "1":   //单关购买注数和对应倍数
                            if (CnLocateWaysAndMultiples != "")
                            {
                                CnLocateWaysAndMultiples += "，";
                            }
                            CnLocateWaysAndMultiples += "单关  " + LocateWaysAndMultiples[j].Substring(1, (LocateWaysAndMultiples[j].Length - 1)) + " 倍";
                            break;
                        case "2":   //双关购买注数和对应倍数
                            if (CnLocateWaysAndMultiples != "")
                            {
                                CnLocateWaysAndMultiples += "，";
                            }
                            CnLocateWaysAndMultiples += "双关  " + LocateWaysAndMultiples[j].Substring(1, (LocateWaysAndMultiples[j].Length - 1)) + " 倍";
                            break;
                        case "3":   //三关购买注数和对应倍数
                            if (CnLocateWaysAndMultiples != "")
                            {
                                CnLocateWaysAndMultiples += "，";
                            }
                            CnLocateWaysAndMultiples += "三关  " + LocateWaysAndMultiples[j].Substring(1, (LocateWaysAndMultiples[j].Length - 1)) + " 倍";
                            break;
                        case "4":   //2串1购买注数和对应倍数
                            if (CnLocateWaysAndMultiples != "")
                            {
                                CnLocateWaysAndMultiples += "，";
                            }
                            CnLocateWaysAndMultiples += "2串1  " + LocateWaysAndMultiples[j].Substring(1, (LocateWaysAndMultiples[j].Length - 1)) + " 倍";
                            break;
                        case "5":   //2串3购买注数和对应倍数
                            if (CnLocateWaysAndMultiples != "")
                            {
                                CnLocateWaysAndMultiples += "，";
                            }
                            CnLocateWaysAndMultiples += "2串3  " + LocateWaysAndMultiples[j].Substring(1, (LocateWaysAndMultiples[j].Length - 1)) + " 倍";
                            break;
                        case "6":   //3串1购买注数和对应倍数
                            if (CnLocateWaysAndMultiples != "")
                            {
                                CnLocateWaysAndMultiples += "，";
                            }
                            CnLocateWaysAndMultiples += "3串1  " + LocateWaysAndMultiples[j].Substring(1, (LocateWaysAndMultiples[j].Length - 1)) + " 倍";
                            break;
                        case "7":   //3串4购买注数和对应倍数
                            if (CnLocateWaysAndMultiples != "")
                            {
                                CnLocateWaysAndMultiples += "，";
                            }
                            CnLocateWaysAndMultiples += "3串4  " + LocateWaysAndMultiples[j].Substring(1, (LocateWaysAndMultiples[j].Length - 1)) + " 倍";
                            break;
                        case "8":    //3串7购买注数和对应倍数
                            if (CnLocateWaysAndMultiples != "")
                            {
                                CnLocateWaysAndMultiples += "，";
                            }
                            CnLocateWaysAndMultiples += "3串7  " + LocateWaysAndMultiples[j].Substring(1, (LocateWaysAndMultiples[j].Length - 1)) + " 倍";
                            break;
                        case "9":    //4串1购买注数和对应倍数
                            if (CnLocateWaysAndMultiples != "")
                            {
                                CnLocateWaysAndMultiples += "，";
                            }
                            CnLocateWaysAndMultiples += "4串1  " + LocateWaysAndMultiples[j].Substring(1, (LocateWaysAndMultiples[j].Length - 1)) + " 倍";
                            break;
                        case "A":   //4串5购买注数和对应倍数
                            if (CnLocateWaysAndMultiples != "")
                            {
                                CnLocateWaysAndMultiples += "，";
                            }
                            CnLocateWaysAndMultiples += "4串5  " + LocateWaysAndMultiples[j].Substring(1, (LocateWaysAndMultiples[j].Length - 1)) + " 倍";
                            break;
                        case "B":   //4串11购买注数和对应倍数
                            if (CnLocateWaysAndMultiples != "")
                            {
                                CnLocateWaysAndMultiples += "，";
                            }
                            CnLocateWaysAndMultiples += "4串11  " + LocateWaysAndMultiples[j].Substring(1, (LocateWaysAndMultiples[j].Length - 1)) + " 倍";
                            break;
                        case "C":   //4串15购买注数和对应倍数
                            if (CnLocateWaysAndMultiples != "")
                            {
                                CnLocateWaysAndMultiples += "，";
                            }
                            CnLocateWaysAndMultiples += "4串15  " + LocateWaysAndMultiples[j].Substring(1, (LocateWaysAndMultiples[j].Length - 1)) + " 倍";
                            break;
                        case "D":   //5串1购买注数和对应倍数
                            if (CnLocateWaysAndMultiples != "")
                            {
                                CnLocateWaysAndMultiples += "，";
                            }
                            CnLocateWaysAndMultiples += "5串1  " + LocateWaysAndMultiples[j].Substring(1, (LocateWaysAndMultiples[j].Length - 1)) + " 倍";
                            break;
                        case "E":   //5串6购买注数和对应倍数
                            if (CnLocateWaysAndMultiples != "")
                            {
                                CnLocateWaysAndMultiples += "，";
                            }
                            CnLocateWaysAndMultiples += "5串6  " + LocateWaysAndMultiples[j].Substring(1, (LocateWaysAndMultiples[j].Length - 1)) + " 倍";
                            break;
                        case "F":   //5串16购买注数和对应倍数
                            if (CnLocateWaysAndMultiples != "")
                            {
                                CnLocateWaysAndMultiples += "，";
                            }
                            CnLocateWaysAndMultiples += "5串16  " + LocateWaysAndMultiples[j].Substring(1, (LocateWaysAndMultiples[j].Length - 1)) + " 倍";
                            break;
                        case "G":   //5串26购买注数和对应倍数
                            if (CnLocateWaysAndMultiples != "")
                            {
                                CnLocateWaysAndMultiples += "，";
                            }
                            CnLocateWaysAndMultiples += "5串26  " + LocateWaysAndMultiples[j].Substring(1, (LocateWaysAndMultiples[j].Length - 1)) + " 倍";
                            break;
                        case "H":   //5串31购买注数和对应倍数
                            if (CnLocateWaysAndMultiples != "")
                            {
                                CnLocateWaysAndMultiples += "，";
                            }
                            CnLocateWaysAndMultiples += "5串31  " + LocateWaysAndMultiples[j].Substring(1, (LocateWaysAndMultiples[j].Length - 1)) + " 倍";
                            break;
                        case "I":   //6串1购买注数和对应倍数
                            if (CnLocateWaysAndMultiples != "")
                            {
                                CnLocateWaysAndMultiples += "，";
                            }
                            CnLocateWaysAndMultiples += "6串1  " + LocateWaysAndMultiples[j].Substring(1, (LocateWaysAndMultiples[j].Length - 1)) + " 倍";
                            break;
                        case "J":   //6串7购买注数和对应倍数
                            if (CnLocateWaysAndMultiples != "")
                            {
                                CnLocateWaysAndMultiples += "，";
                            }
                            CnLocateWaysAndMultiples += "6串7  " + LocateWaysAndMultiples[j].Substring(1, (LocateWaysAndMultiples[j].Length - 1)) + " 倍";
                            break;
                        case "K":   //6串22购买注数和对应倍数
                            if (CnLocateWaysAndMultiples != "")
                            {
                                CnLocateWaysAndMultiples += "，";
                            }
                            CnLocateWaysAndMultiples += "6串22  " + LocateWaysAndMultiples[j].Substring(1, (LocateWaysAndMultiples[j].Length - 1)) + " 倍";
                            break;
                        case "L":   //6串42购买注数和对应倍数
                            if (CnLocateWaysAndMultiples != "")
                            {
                                CnLocateWaysAndMultiples += "，";
                            }
                            CnLocateWaysAndMultiples += "6串42  " + LocateWaysAndMultiples[j].Substring(1, (LocateWaysAndMultiples[j].Length - 1)) + " 倍";
                            break;
                        case "M":   //6串57购买注数和对应倍数
                            if (CnLocateWaysAndMultiples != "")
                            {
                                CnLocateWaysAndMultiples += "，";
                            }
                            CnLocateWaysAndMultiples += "6串57  " + LocateWaysAndMultiples[j].Substring(1, (LocateWaysAndMultiples[j].Length - 1)) + " 倍";
                            break;
                        case "N":   //6串63购买注数和对应倍数
                            if (CnLocateWaysAndMultiples != "")
                            {
                                CnLocateWaysAndMultiples += "，";
                            }
                            CnLocateWaysAndMultiples += "6串63  " + LocateWaysAndMultiples[j].Substring(1, (LocateWaysAndMultiples[j].Length - 1)) + " 倍";
                            break;
                        case "O":   //7串1购买注数和对应倍数
                            if (CnLocateWaysAndMultiples != "")
                            {
                                CnLocateWaysAndMultiples += "，";
                            }
                            CnLocateWaysAndMultiples += "7串1  " + LocateWaysAndMultiples[j].Substring(1, (LocateWaysAndMultiples[j].Length - 1)) + " 倍";
                            break;
                        case "P":   //8串1购买注数和对应倍数
                            if (CnLocateWaysAndMultiples != "")
                            {
                                CnLocateWaysAndMultiples += "，";
                            }
                            CnLocateWaysAndMultiples += "8串1  " + LocateWaysAndMultiples[j].Substring(1, (LocateWaysAndMultiples[j].Length - 1)) + " 倍";
                            break;
                        case "Q":   //9串1购买注数和对应倍数
                            if (CnLocateWaysAndMultiples != "")
                            {
                                CnLocateWaysAndMultiples += "，";
                            }
                            CnLocateWaysAndMultiples += "9串1  " + LocateWaysAndMultiples[j].Substring(1, (LocateWaysAndMultiples[j].Length - 1)) + " 倍";
                            break;
                        case "R":   //10串1购买注数和对应倍数
                            if (CnLocateWaysAndMultiples != "")
                            {
                                CnLocateWaysAndMultiples += "，";
                            }
                            CnLocateWaysAndMultiples += "10串1  " + LocateWaysAndMultiples[j].Substring(1, (LocateWaysAndMultiples[j].Length - 1)) + " 倍";
                            break;
                        case "S":   //11串1购买注数和对应倍数
                            if (CnLocateWaysAndMultiples != "")
                            {
                                CnLocateWaysAndMultiples += "，";
                            }
                            CnLocateWaysAndMultiples += "11串1  " + LocateWaysAndMultiples[j].Substring(1, (LocateWaysAndMultiples[j].Length - 1)) + " 倍";
                            break;
                        case "T":   //12串1购买注数和对应倍数
                            if (CnLocateWaysAndMultiples != "")
                            {
                                CnLocateWaysAndMultiples += "，";
                            }
                            CnLocateWaysAndMultiples += "12串1  " + LocateWaysAndMultiples[j].Substring(1, (LocateWaysAndMultiples[j].Length - 1)) + " 倍";
                            break;
                        case "U":   //13串1购买注数和对应倍数
                            if (CnLocateWaysAndMultiples != "")
                            {
                                CnLocateWaysAndMultiples += "，";
                            }
                            CnLocateWaysAndMultiples += "13串1  " + LocateWaysAndMultiples[j].Substring(1, (LocateWaysAndMultiples[j].Length - 1)) + " 倍";
                            break;
                        case "V":   //14串1购买注数和对应倍数
                            if (CnLocateWaysAndMultiples != "")
                            {
                                CnLocateWaysAndMultiples += "，";
                            }
                            CnLocateWaysAndMultiples += "14串1  " + LocateWaysAndMultiples[j].Substring(1, (LocateWaysAndMultiples[j].Length - 1)) + " 倍";
                            break;
                        case "W":   //15串1购买注数和对应倍数
                            if (CnLocateWaysAndMultiples != "")
                            {
                                CnLocateWaysAndMultiples += "，";
                            }
                            CnLocateWaysAndMultiples += "15串1  " + LocateWaysAndMultiples[j].Substring(1, (LocateWaysAndMultiples[j].Length - 1)) + " 倍";
                            break;
                        default:
                            CnLocateWaysAndMultiples += " <font color='red'>读取错误！</font>";
                            break;
                    }
                }

                return true;
            }

            public override string ShowNumber(string Number)
            {
                return base.ShowNumber(Number, "");
            }

            public override string ToElectronicTicket_BJDC(int PlayTypeID, string Number, ref string TicketNumber, ref int NewPlayTypeID, ref string Rule, ref int Multiple, ref double Money, ref string GameNoList, ref string PassMode, ref int TicketCount)
            {
                GetNewPlayTypeID(PlayTypeID, ref NewPlayTypeID, ref Rule);

                int SchemeLength = Number.Split(';').Length;

                if (SchemeLength != 3)
                {
                    return "";
                }

                string BuyNumber = Number.Trim().Split(';')[1].ToString();
                string NumberInfo = Number.Trim().Split(';')[2].ToString();

                PassMode = GetPassMode(NumberInfo.Substring(0, 1));
                Multiple = Shove._Convert.StrToInt(NumberInfo.Substring(1), 0);

                string Numbers = BuyNumber.Substring(1, BuyNumber.Length - 1).Substring(0, BuyNumber.Length - 2).ToString().Trim();

                if (Numbers == "")
                {
                    return "";
                }

                string[] Math = Numbers.Split('|');

                if (Math.Length < 1)
                {
                    return "";
                }

                TicketNumber = "";
                GameNoList = "";

                string NO = "";

                for (int i = 0; i < Math.Length; i++)
                {
                    NO = Math[i].Substring(Math[i].IndexOf('(') + 1, (Math[i].IndexOf(')') - Math[i].IndexOf('(') - 1));

                    GameNoList += NO + ",";

                    TicketNumber += NO;

                    TicketNumber += "[";

                    TicketNumber += PrizeInfo(PlayTypeID, Math[i].Substring(0, Math[i].IndexOf('(')));

                    TicketNumber += "]";
                }

                GameNoList = GameNoList.Substring(GameNoList.Length - 1);

                return "";
            }

            private void GetNewPlayTypeID(int PlayTypeID, ref int NewPlayTypeID, ref string Rule)
            {
                switch (PlayTypeID)
                {
                    case 4501:
                        NewPlayTypeID = 44;
                        Rule = "单场胜平负";
                        break;
                    case 4502:
                        NewPlayTypeID = 45;
                        Rule = "单场进球数";
                        break;
                    case 4503:
                        NewPlayTypeID = 46;
                        Rule = "单场上下单双";
                        break;
                    case 4504:
                        NewPlayTypeID = 47;
                        Rule = "单场比分";
                        break;
                    case 4505:
                        NewPlayTypeID = 48;
                        Rule = "单场半全场胜平负";
                        break;
                    default:
                        NewPlayTypeID = 0;
                        Rule = "";
                        break;
                }
            }

            private string GetPassMode(string passmode)
            {
                return passmode.Replace("1", "单关").Replace("2", "双关").Replace("3", "三关").Replace("4", "2串1").Replace("5", "2串3").Replace("6", "3串1")
                               .Replace("7", "3串4").Replace("8", "3串7").Replace("9", "4串1").Replace("A", "4串4").Replace("B", "4串4").Replace("C", "4串11")
                               .Replace("D", "4串15").Replace("E", "5串1").Replace("F", "5串6").Replace("G", "5串16").Replace("H", "5串31").Replace("I", "6串1")
                               .Replace("J", "6串7").Replace("K", "6串22").Replace("L", "6串42").Replace("M", "6串57").Replace("N", "6串63");
            }

            private string PrizeInfo(int PlayTypeID, string PrizeInfo)
            {
                switch (PlayTypeID)
                {
                    case 4501:
                        return PrizeInfo.Replace("3", "胜").Replace("1", "平").Replace("0", "负");
                    case 4502:
                        return PrizeInfo;
                    case 4503:
                        return PrizeInfo.Replace("1", "上单").Replace("2", "上双").Replace("3", "下单").Replace("4", "下双");
                    case 4504:
                        return PrizeInfo.Replace("1", "1:0").Replace("2", "2:0").Replace("3", "2:1").Replace("4", "3:0").Replace("5", "3:1")
                                        .Replace("6", "3:2").Replace("7", "4:0").Replace("8", "4:1").Replace("9", "4:2").Replace("10", "胜其他")
                                        .Replace("11", "0:0").Replace("12", "1:1").Replace("13", "2:2").Replace("14", "3:3").Replace("15", "平其他")
                                        .Replace("16", "0:1").Replace("17", "0:2").Replace("18", "1:2").Replace("19", "0:3").Replace("20", "1:3")
                                        .Replace("21", "2:3").Replace("22", "0:4").Replace("23", "1:4").Replace("24", "2:4").Replace("25", "负其他");
                    case 4505:
                        return PrizeInfo.Replace("1", "胜-胜").Replace("2", "胜-平").Replace("3", "胜-负").Replace("4", "平-胜").Replace("5", "平-平")
                                        .Replace("6", "平-负").Replace("7", "负-胜").Replace("8", "负-平").Replace("9", "负-负");
                    default:
                        return PrizeInfo;
                }
            }
        }
    }
}