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
        /// 好彩1
        /// </summary>
        public partial class HC1 : LotteryBase
        {
            #region 静态变量
            public const int PlayType_ShuZi = 8001;       //数字
            public const int PlayType_ShengXiao = 8002;   //生肖    
            public const int PlayType_SiJi = 8003;        //四季
            public const int PlayType_FangWei = 8004;     //方位

            public const int ID = 80;
            public const string sID = "80";
            public const string Name = "好彩1";           //名称
            public const string Code = "HC1";             //编码
            public const double MaxMoney = 20000;         //最大金额
            #endregion

            public HC1()   //构造函数初始化
            {
                id = 80;
                name = "好彩1";
                code = "HC1";
            }

            public override bool CheckPlayType(int play_type) //检查玩法 
            {
                return ((play_type >= 8001) && (play_type <= 8004));
            }

            public override PlayType[] GetPlayTypeList()
            {
                PlayType[] Result = new PlayType[4];

                Result[0] = new PlayType(PlayType_ShuZi, "数字");
                Result[1] = new PlayType(PlayType_ShengXiao, "生肖");
                Result[2] = new PlayType(PlayType_SiJi, "四季");
                Result[3] = new PlayType(PlayType_FangWei, "方位");

                return Result;
            }

            public override string BuildNumber(int Num)  //随机选号  num注数
            {
                System.Random rd = new Random();
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < Num; i++)
                {
                    string LotteryNumber = "";
                    for (int j = 0; j < 1; j++)
                    {
                        LotteryNumber = rd.Next(1, 36 + 1).ToString();  //返回从1到36中的随机随
                        LotteryNumber.PadLeft(2, '0');                         //左边用0填充直到lotteryNumber的长度为2
                    }
                    sb.Append(LotteryNumber.Trim() + "\n");
                }

                string Result = sb.ToString();
                Result = Result.Substring(0, Result.Length - 1);
                return Result;
            }

            public override string[] ToSingle(string Number, ref string CanonicalNumber, int PlayType)	// 后面 ref 参数是将彩票规范化，如：01 01 02... 变成 01 02...
            {
                if (PlayType == PlayType_ShuZi)
                    return ToSingle_ShuZi(Number, ref CanonicalNumber);

                if (PlayType == PlayType_ShengXiao)
                    return ToSingle_ShengXiao(Number, ref CanonicalNumber);

                if (PlayType == PlayType_FangWei)
                    return ToSingle_FangWei(Number, ref CanonicalNumber);

                if (PlayType == PlayType_SiJi)
                    return ToSingle_SiJi(Number, ref CanonicalNumber);

                return null;
            }

            #region ToSingle的具体实现
            private string[] ToSingle_Mixed(string Number, ref string CanonicalNumber)
            {
                CanonicalNumber = "";

                string PreFix = GetLotteryNumberPreFix(Number);        //校验是否有前缀[],如果存在则取出来,否则返回null

                if (Number.StartsWith("[数字]"))
                {
                    return MergeLotteryNumberPreFix(ToSingle_ShuZi(FilterPreFix(Number), ref CanonicalNumber), PreFix);
                }

                if (Number.StartsWith("[生肖]"))
                {
                    return MergeLotteryNumberPreFix(ToSingle_ShengXiao(FilterPreFix(Number), ref CanonicalNumber), PreFix);
                }

                if (Number.StartsWith("[方位]"))
                {
                    return MergeLotteryNumberPreFix(ToSingle_FangWei(FilterPreFix(Number), ref CanonicalNumber), PreFix);
                }

                if (Number.StartsWith("[四季]"))
                {
                    return MergeLotteryNumberPreFix(ToSingle_SiJi(FilterPreFix(Number), ref CanonicalNumber), PreFix);
                }

                return null;
            }

            private string[] ToSingle_ShuZi(string Number, ref string CanonicalNumber)
            {
                string[] strs = FilterRepeated(Number.Trim().Split(' '));  //把校验后的投注内容添加到数组strs
                CanonicalNumber = "";

                if (strs.Length < 1)
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
                for (int i = 0; i < n; i++)
                    al.Add(strs[i]);

                #endregion

                string[] Result = new string[al.Count];
                for (int i = 0; i < al.Count; i++)
                    Result[i] = al[i].ToString();
                return Result;
            }

            private string[] ToSingle_ShengXiao(string Number, ref string CanonicalNumber)	//对传进来的投注内容和正则表达式里面的内容做对比，如果存在就返回
            {
                CanonicalNumber = "";
                string[] strs = Number.Trim().Split(' ');
                ArrayList al = new ArrayList();

                Regex regex = new Regex(@"^(?<L0>([鼠牛虎兔龙蛇马羊猴鸡狗猪]))", RegexOptions.IgnoreCase | RegexOptions.Compiled);

                for (int i = 0; i < strs.Length; i++)
                {
                    Match m = regex.Match(strs[i]);
                    string Locate = m.Groups["L" + i.ToString()].ToString().Trim();
                    if (Locate == "")
                    {
                        CanonicalNumber = "";
                        return null;
                    }

                    CanonicalNumber += Locate;
                    al.Add(CanonicalNumber);
                }

                string[] Result = new string[al.Count];
                for (int i = 0; i < al.Count; i++)
                {
                    Result[i] = al[i].ToString();
                }
                return Result;
            }

            private string[] ToSingle_FangWei(string Number, ref string CanonicalNumber)
            {
                CanonicalNumber = "";
                ArrayList al = new ArrayList();
                string[] strs = Number.Trim().Split(' ');
                Regex regex = new Regex(@"^(?<L0>([东南西北]))", RegexOptions.IgnoreCase | RegexOptions.Compiled);

                for (int i = 0; i < strs.Length; i++)
                {
                    Match m = regex.Match(strs[i]);
                    string Locate = m.Groups["L" + i.ToString()].ToString().Trim();
                    if (Locate == "")
                    {
                        CanonicalNumber = "";
                        return null;
                    }

                    CanonicalNumber += Locate;
                    al.Add(CanonicalNumber);
                }

                string[] Result = new string[al.Count];
                for (int i = 0; i < al.Count; i++)
                {
                    Result[i] = al[i].ToString();
                }
                return Result;
            }

            private string[] ToSingle_SiJi(string Number, ref string CanonicalNumber)
            {
                CanonicalNumber = "";
                ArrayList al = new ArrayList();
                string[] strs = Number.Trim().Split(' ');

                Regex regex = new Regex(@"^(?<L0>([春夏秋冬]))", RegexOptions.IgnoreCase | RegexOptions.Compiled);

                for (int i = 0; i < strs.Length; i++)
                {
                    Match m = regex.Match(strs[i]);
                    string Locate = m.Groups["L" + i.ToString()].ToString().Trim();
                    if (Locate == "")
                    {
                        CanonicalNumber = "";
                        return null;
                    }

                    CanonicalNumber += Locate;
                    al.Add(CanonicalNumber);
                }

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
                int WinCountShuZi = 0;
                int WinCountShengXiao = 0;
                int WinCountFangWei = 0;
                int WinCountSiJi = 0;

                if ((WinMoneyList == null) || (WinMoneyList.Length < 8))     //判断奖金列表是否存在，是否小于4
                    return -3;

                if (PlayType == PlayType_ShuZi)
                    return ComputeWin_ShuZi(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[0], WinMoneyList[1], ref WinCountShuZi);

                if (PlayType == PlayType_ShengXiao)
                    return ComputeWin_ShengXiao(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[2], WinMoneyList[3], ref WinCountShengXiao);

                if (PlayType == PlayType_FangWei)
                    return ComputeWin_FangWei(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[4], WinMoneyList[5], ref WinCountFangWei);

                if (PlayType == PlayType_SiJi)
                    return ComputeWin_SiJi(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[6], WinMoneyList[7], ref WinCountSiJi);
                return -4;
            }

            #region ComputeWin  的具体方法
            private double ComputeWin_Mixed(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney1, double WinMoneyNoWithTax1)
            {
                string[] Lotterys = SplitLotteryNumbers(Number);
                if (Lotterys == null)
                {
                    return -2;
                }

                if (Lotterys.Length < 1)
                {
                    return -2;
                }

                double WinMoney = 0;

                //奖金的参数列表(数字奖，生肖奖，方位奖，四季奖)
                int WinCount1 = 0, WinCount2 = 0, WinCount3 = 0, WinCount4 = 0;

                for (int ii = 0; ii < Lotterys.Length; ii++)
                {
                    int WinCountShuZi = 0;
                    int WinCountShengXiao = 0;
                    int WinCountFangWei = 0;
                    int WinCountSiJi = 0;

                    double t_WinMoneyNoWithTax = 0;

                    if (Lotterys[ii].StartsWith("[数字]"))
                    {
                        //奖金
                        WinMoney += ComputeWin_ShuZi(FilterPreFix(Lotterys[ii]), WinNumber, ref Description, ref t_WinMoneyNoWithTax, WinMoney1, WinMoneyNoWithTax1, ref WinCountShuZi);

                        WinMoneyNoWithTax += t_WinMoneyNoWithTax;
                        WinCount1 += WinCountShuZi;
                    }
                    else if (Lotterys[ii].StartsWith("[生肖]"))
                    {
                        WinMoney += ComputeWin_ShengXiao(FilterPreFix(Lotterys[ii]), WinNumber, ref Description, ref t_WinMoneyNoWithTax, WinMoney1, WinMoneyNoWithTax1, ref WinCountShengXiao);

                        WinMoneyNoWithTax += t_WinMoneyNoWithTax;
                        WinCount2 += WinCountShengXiao;
                    }
                    else if (Lotterys[ii].StartsWith("[方位]"))
                    {
                        WinMoney += ComputeWin_FangWei(FilterPreFix(Lotterys[ii]), WinNumber, ref Description, ref t_WinMoneyNoWithTax, WinMoney1, WinMoneyNoWithTax1, ref WinCountFangWei);

                        WinMoneyNoWithTax += t_WinMoneyNoWithTax;
                        WinCount3 += WinCountFangWei;
                    }
                    else if (Lotterys[ii].StartsWith("[四季]"))
                    {
                        WinMoney += ComputeWin_SiJi(FilterPreFix(Lotterys[ii]), WinNumber, ref Description, ref t_WinMoneyNoWithTax, WinMoney1, WinMoneyNoWithTax1, ref WinCountSiJi);

                        WinMoneyNoWithTax += t_WinMoneyNoWithTax;
                        WinCount4 += WinCountSiJi;
                    }
                }

                #region 构建中奖描述

                Description = "";

                if (WinCount1 > 0)
                {
                    MergeWinDescription(ref Description, "数字奖" + WinCount1.ToString() + "注");
                }
                if (WinCount2 > 0)
                {
                    MergeWinDescription(ref Description, "生肖奖" + WinCount2.ToString() + "注");
                }
                if (WinCount3 > 0)
                {
                    MergeWinDescription(ref Description, "方位奖" + WinCount3.ToString() + "注");
                }
                if (WinCount4 > 0)
                {
                    MergeWinDescription(ref Description, "四季奖" + WinCount4.ToString() + "注");
                }
                #endregion

                return WinMoney;
            }

            private double ComputeWin_ShuZi(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney1, double WinMoneyNoWithTax1, ref int WinCount1)
            {
                WinCount1 = 0;

                WinNumber = WinNumber.Trim();
                if (WinNumber.Length < 1)	//对开奖号的判断
                    return -1;
                string[] Lotterys = SplitLotteryNumbers(Number);
                if (Lotterys == null)
                    return -2;
                if (Lotterys.Length < 1)
                    return -2;

                double WinMoney = 0;
                WinMoneyNoWithTax = 0;

                for (int ii = 0; ii < Lotterys.Length; ii++)
                {
                    string t_str = "";
                    string[] Lottery = ToSingle_ShuZi(Lotterys[ii], ref t_str);  //把处理好的投注内容添加到Lottery
                    if (Lottery == null)
                        continue;
                    if (Lottery.Length < 1)
                        continue;

                    Regex[] regexHC = new Regex[1];
                    regexHC[0] = new Regex(@"^(\d){2}", RegexOptions.IgnoreCase | RegexOptions.Compiled);

                    for (int i = 0; i < Lottery.Length; i++)
                    {
                        if (regexHC[0].IsMatch(Lottery[i]))  //把处理好的投注内容和验证了的内容进行比较
                        {
                            if (WinNumber.Contains(Lottery[i]))     //判断是否等于开奖号码,然后给予相应的注数,奖金,税后奖
                            {
                                WinCount1++;
                                WinMoney += WinMoney1;
                                WinMoneyNoWithTax += WinMoneyNoWithTax1;

                                continue;
                            }
                        }
                    }
                }

                Description = "";

                if (WinCount1 > 0)   //如果注数大于1，就把描述及中奖注数显示出来
                {
                    MergeWinDescription(ref Description, "数字奖" + WinCount1.ToString() + "注");
                }

                if (Description != "")
                    Description += "。";

                return WinMoney;
            }

            private string[] SplitLotteryNumbers(string Number)
            {
                string[] s = Number.Split(' ');
                if (s.Length == 0)
                    return null;
                for (int i = 0; i < s.Length; i++)
                    s[i] = s[i].Trim();
                return s;
            }

            private double ComputeWin_ShengXiao(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney1, double WinMoneyNoWithTax1, ref int WinCount1)
            {
                WinCount1 = 0;

                WinNumber = WinNumber.Trim();
                if (WinNumber.Length < 1)	//3: "12345"
                    return -1;
                string[] Lotterys = SplitLotteryNumbers(Number);
                if (Lotterys == null)
                    return -2;
                if (Lotterys.Length < 1)
                    return -2;

                double WinMoney = 0;
                WinMoneyNoWithTax = 0;

                for (int ii = 0; ii < Lotterys.Length; ii++)
                {
                    string t_str = "";
                    string[] Lottery = ToSingle_ShengXiao(Lotterys[ii], ref t_str);
                    if (Lottery == null)
                        continue;
                    if (Lottery.Length < 1)
                        continue;

                    Regex[] regexHC = new Regex[1];
                    regexHC[0] = new Regex(@"^(?<L0>([鼠牛虎兔龙蛇马羊猴鸡狗猪]))", RegexOptions.IgnoreCase | RegexOptions.Compiled);

                    for (int i = 0; i < Lottery.Length; i++)
                    {
                        if (regexHC[0].IsMatch(Lottery[i]))
                        {
                            if (WinNumber.Contains(Lottery[i]))
                            {
                                WinCount1++;
                                WinMoney += WinMoney1;
                                WinMoneyNoWithTax += WinMoneyNoWithTax1;

                                continue;
                            }
                        }
                    }
                }

                Description = "";

                if (WinCount1 > 0)
                {
                    MergeWinDescription(ref Description, "生肖奖" + WinCount1.ToString() + "注");
                }

                if (Description != "")
                    Description += "。";

                return WinMoney;
            }

            private double ComputeWin_FangWei(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney1, double WinMoneyNoWithTax1, ref int WinCount1)
            {
                WinCount1 = 0;

                WinNumber = WinNumber.Trim();
                if (WinNumber.Length < 1)	//3: "12345"
                    return -1;
                string[] Lotterys = SplitLotteryNumbers(Number);
                if (Lotterys == null)
                    return -2;
                if (Lotterys.Length < 1)
                    return -2;

                double WinMoney = 0;
                WinMoneyNoWithTax = 0;

                for (int ii = 0; ii < Lotterys.Length; ii++)
                {
                    string t_str = "";
                    string[] Lottery = ToSingle_FangWei(Lotterys[ii], ref t_str);
                    if (Lottery == null)
                        continue;
                    if (Lottery.Length < 1)
                        continue;

                    Regex[] regexHC = new Regex[1];
                    regexHC[0] = new Regex(@"^(?<L0>([东南西北]))", RegexOptions.IgnoreCase | RegexOptions.Compiled);

                    for (int i = 0; i < Lottery.Length; i++)
                    {
                        if (regexHC[0].IsMatch(Lottery[i]))
                        {
                            if (WinNumber.Contains(Lottery[i]))
                            {
                                WinCount1++;
                                WinMoney += WinMoney1;
                                WinMoneyNoWithTax += WinMoneyNoWithTax1;

                                continue;
                            }
                        }
                    }
                }

                Description = "";

                if (WinCount1 > 0)
                {
                    MergeWinDescription(ref Description, "方位奖" + WinCount1.ToString() + "注");
                }

                if (Description != "")
                    Description += "。";

                return WinMoney;
            }

            private double ComputeWin_SiJi(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney1, double WinMoneyNoWithTax1, ref int WinCount1)
            {
                WinCount1 = 0;

                WinNumber = WinNumber.Trim();
                if (WinNumber.Length < 1)
                    return -1;
                string[] Lotterys = SplitLotteryNumbers(Number);
                if (Lotterys == null)
                    return -2;
                if (Lotterys.Length < 1)
                    return -2;

                double WinMoney = 0;
                WinMoneyNoWithTax = 0;

                for (int ii = 0; ii < Lotterys.Length; ii++)
                {
                    string t_str = "";
                    string[] Lottery = ToSingle_SiJi(Lotterys[ii], ref t_str);
                    if (Lottery == null)
                        continue;
                    if (Lottery.Length < 1)
                        continue;

                    Regex[] regexHC = new Regex[1];
                    regexHC[0] = new Regex(@"^(?<L0>([春夏秋冬]))", RegexOptions.IgnoreCase | RegexOptions.Compiled);

                    for (int i = 0; i < Lottery.Length; i++)
                    {
                        if (regexHC[0].IsMatch(Lottery[i]))
                        {
                            if (WinNumber.Contains(Lottery[i]))
                            {
                                WinCount1++;
                                WinMoney += WinMoney1;
                                WinMoneyNoWithTax += WinMoneyNoWithTax1;

                                continue;
                            }
                        }
                    }
                }

                Description = "";

                if (WinCount1 > 0)
                {
                    MergeWinDescription(ref Description, "四季奖" + WinCount1.ToString() + "注");
                }

                if (Description != "")
                    Description += "。";

                return WinMoney;
            }
            #endregion

            public override string AnalyseScheme(string Content, int PlayType)
            {
                if (PlayType == PlayType_ShuZi)
                    return AnalyseScheme_ShuZi(Content, PlayType);

                if (PlayType == PlayType_ShengXiao)
                    return AnalyseScheme_ShengXiao(Content, PlayType);

                if (PlayType == PlayType_FangWei)
                    return AnalyseScheme_FangWei(Content, PlayType);

                if (PlayType == PlayType_SiJi)
                    return AnalyseScheme_SiJi(Content, PlayType);

                return null;
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

                    if (Lotterys[ii].StartsWith("[数字]"))
                    {
                        t_Result += AnalyseScheme_ShuZi(Lotterys[ii], PlayType_ShuZi);
                    }

                    if (Lotterys[ii].StartsWith("[生肖]"))
                    {
                        t_Result += AnalyseScheme_ShengXiao(Lotterys[ii], PlayType_ShengXiao);
                    }

                    if (Lotterys[ii].StartsWith("[方位]"))
                    {
                        t_Result += AnalyseScheme_FangWei(Lotterys[ii], PlayType_FangWei);
                    }

                    if (Lotterys[ii].StartsWith("[四季]"))
                    {
                        t_Result += AnalyseScheme_SiJi(Lotterys[ii], PlayType_SiJi);
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

            private string AnalyseScheme_ShuZi(string Content, int PlayType)
            {
                string[] strs = Content.Split('\n');
                if (strs == null)
                    return "";
                if (strs.Length == 0)
                    return "";

                string Result = "";
                string regexF = @"^((\d\d\s){1,36}\d\d)|(\d\d)";
                Regex regex = new Regex(regexF, RegexOptions.Compiled | RegexOptions.IgnoreCase);

                for (int i = 0; i < strs.Length; i++)
                {
                    Match m = regex.Match(strs[i]);
                    if (m.Success)
                    {
                        string CanonicalNumber = "";
                        string[] singles = ToSingle_ShuZi(m.Value, ref CanonicalNumber);
                        if (singles == null)
                            continue;
                        if ((singles.Length >= 1) && (singles.Length <= (MaxMoney / 2)))
                            Result += CanonicalNumber + "|" + singles.Length.ToString() + "\n";
                    }
                }

                if (Result.EndsWith("\n"))
                    Result = Result.Substring(0, Result.Length - 1);
                return Result;
            }

            private string AnalyseScheme_ShengXiao(string Content, int PlayType)
            {
                string[] strs = Content.Split(' ');
                if (strs == null)
                    return "";
                if (strs.Length == 0)
                    return "";

                string Result = "";
                Regex regex = new Regex(@"^(?<L0>([鼠牛虎兔龙蛇马羊猴鸡狗猪]))", RegexOptions.Compiled | RegexOptions.IgnoreCase);

                for (int i = 0; i < strs.Length; i++)
                {
                    Match m = regex.Match(strs[i]);
                    if (m.Success)
                    {
                        string CanonicalNumber = "";
                        string[] singles = ToSingle_ShengXiao(m.Value.Replace("[生肖]", ""), ref CanonicalNumber);
                        if (singles == null)
                            continue;
                        if ((singles.Length >= 1) && (singles.Length <= (MaxMoney / 2)))
                            Result += CanonicalNumber + "|" + singles.Length.ToString() + "\n";
                    }
                }

                if (Result.EndsWith("\n"))
                    Result = Result.Substring(0, Result.Length - 1);
                return Result;
            }

            private string AnalyseScheme_FangWei(string Content, int PlayType)
            {
                string[] strs = Content.Split(' ');
                if (strs == null)
                    return "";
                if (strs.Length == 0)
                    return "";

                string Result = "";

                Regex regex = new Regex(@"^(?<L0>([东南西北]))", RegexOptions.Compiled | RegexOptions.IgnoreCase);

                for (int i = 0; i < strs.Length; i++)
                {
                    Match m = regex.Match(strs[i]);
                    if (m.Success)
                    {
                        string CanonicalNumber = "";
                        string[] singles = ToSingle_FangWei(m.Value.Replace("[方位]", ""), ref CanonicalNumber);
                        if (singles == null)
                            continue;
                        if ((singles.Length >= 1) && (singles.Length <= (MaxMoney / 2)))
                            Result += CanonicalNumber + "|" + singles.Length.ToString() + "\n";
                    }
                }

                if (Result.EndsWith("\n"))
                    Result = Result.Substring(0, Result.Length - 1);
                return Result;
            }

            private string AnalyseScheme_SiJi(string Content, int PlayType)
            {
                string[] strs = Content.Split(' ');
                if (strs == null)
                    return "";
                if (strs.Length == 0)
                    return "";

                string Result = "";
                Regex regex = new Regex(@"^(?<L0>([春夏秋冬]))", RegexOptions.Compiled | RegexOptions.IgnoreCase);

                for (int i = 0; i < strs.Length; i++)
                {
                    Match m = regex.Match(strs[i]);
                    if (m.Success)
                    {
                        string CanonicalNumber = "";
                        string[] singles = ToSingle_SiJi(m.Value.Replace("[四季]", ""), ref CanonicalNumber);
                        if (singles == null)
                            continue;
                        if ((singles.Length >= 1) && (singles.Length <= (MaxMoney / 2)))
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
                Regex regex = new Regex(@"^([\d]){2}", RegexOptions.Compiled | RegexOptions.IgnoreCase);

                return regex.IsMatch(Number);
            }

            private string[] FilterRepeated(string[] NumberPart)  //36
            {
                ArrayList al = new ArrayList();
                for (int i = 0; i < NumberPart.Length; i++)
                {
                    int Ball = Shove._Convert.StrToInt(NumberPart[i], -1);    //对投注内容取整赋值给ball
                    if ((Ball >= 1) && (Ball <= 36) && (!isExistBall(al, Ball)))   //判断ball是否存在以及是否在范围内
                        al.Add(NumberPart[i]);
                }

                CompareToAscii compare = new CompareToAscii();
                al.Sort(compare);

                string[] Result = new string[al.Count];
                for (int i = 0; i < al.Count; i++)
                    Result[i] = al[i].ToString().PadLeft(2, '0');

                return Result;
            }

            private string[] FilterString(string[] Number)
            {
                ArrayList al = new ArrayList();
                for (int i = 0; i < Number.Length; i++)
                {
                    string Ball = Number[i].ToString();
                    for (int j = 0; j < Ball.Trim().Length; j++)
                    {
                        int Balls = Shove._Convert.StrToInt(Ball[j].ToString(), -1);
                        if (j < 2)
                        {
                            int Ballss = Shove._Convert.StrToInt(Ball[j + 1].ToString(), -1);
                            if (Balls > 0 && Balls <= 36 && (36 / (Ballss - Balls)) == 3)
                            {
                                al.Add(Ball);
                            }
                        }
                        else
                        {
                            int Ballss = Shove._Convert.StrToInt(Ball[j - 1].ToString(), -1);
                            if (Balls >= 1 && Balls <= 36 && (36 / (Balls - Ballss)) == 3)
                            {
                                al.Add(Ball);
                            }
                        }
                    }
                }
                string[] Result = new string[al.Count];
                for (int i = 0; i < al.Count; i++)
                    Result[i] = al[i].ToString();

                return Result;
            }

            private string ConvertSNumber(string lottryNumber)
            {
                switch (lottryNumber)
                {
                    case "鼠":
                        return "01 13 25";
                    case "牛":
                        return "02 14 26";
                    case "虎":
                        return "03 15 27";
                    case "兔":
                        return "04 16 28";
                    case "龙":
                        return "05 17 29";
                    case "蛇":
                        return "06 18 30";
                    case "马":
                        return "07 19 31";
                    case "羊":
                        return "08 20 32";
                    case "猴":
                        return "09 21 33";
                    case "鸡":
                        return "10 22 34";
                    case "狗":
                        return "11 23 35";
                    case "猪":
                        return "12 24 36";
                    default:
                        return "";
                }
            }

            private string ConvertZNumber(string lottryNumber)
            {
                switch (lottryNumber)
                {
                    case "01 13 25":
                        return "鼠";
                    case "02 14 26":
                        return "牛";
                    case "03 15 27":
                        return "虎";
                    case "04 16 28":
                        return "兔";
                    case "05 17 29":
                        return "龙";
                    case "06 18 30":
                        return "蛇";
                    case "07 19 31":
                        return "马";
                    case "08 20 32":
                        return "羊";
                    case "09 21 33":
                        return "猴";
                    case "10 22 34":
                        return "鸡";
                    case "11 23 35":
                        return "狗";
                    case "12 24 36":
                        return "猪";
                    default:
                        return "";
                }
            }

            public override string ShowNumber(string Number)
            {
                return base.ShowNumber(Number, "");
            }
        }
    }
}
