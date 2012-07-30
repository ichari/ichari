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
        /// 江西福彩3D
        /// </summary>
        public partial class JXFC3D : LotteryBase
        {
            #region 静态变量
            public const int PlayType_ZhiD = 6701;
            public const int PlayType_Zu3D = 6702;
            public const int PlayType_Zu6D = 6703;
            public const int PlayType_ZhiH = 6704;
            public const int PlayType_Zu3H = 6705;
            public const int PlayType_Zu6H = 6706;
            public const int PlayType_ZuheH = 6707;
            public const int PlayType_ZhiBW = 6708;
            public const int PlayType_ZhiF = 6709;
            public const int PlayType_Zu3F = 6710;
            public const int PlayType_Zu6F = 6711;
            public const int PlayType_ZuheF = 6712;
            public const int PlayType_ZhiBC = 6713;
            public const int PlayType_ZhiECH = 6714;
            public const int PlayType_ZhiDT = 6715;
            public const int PlayType_ZuheDT = 6716;
            public const int PlayType_ZhiKD = 6717;
            public const int PlayType_Zu3KD = 6718;
            public const int PlayType_Zu6KD = 6719;
            public const int PlayType_ZuheKD = 6720;
            public const int PlayType_ZuheHSW = 6721;
            public const int PlayType_ZhiJO = 6722;
            public const int PlayType_Zu3JO = 6723;
            public const int PlayType_Zu6JO = 6724;
            public const int PlayType_ZuheJO = 6725;
            public const int PlayType_ZhiDX = 6726;
            public const int PlayType_Zu3DX = 6727;
            public const int PlayType_Zu6DX = 6728;
            public const int PlayType_ZuheDX = 6729;


            public const int ID = 67;
            public const string sID = "67";
            public const string Name = "江西福彩3D";
            public const string Code = " JXFC3D";
            public const double MaxMoney = 20000;
            #endregion

            public JXFC3D()
            {
                id = 67;
                name = "江西福彩3D";
                code = "JXFC3D";
            }

            public override bool CheckPlayType(int play_type)
            {
                return ((play_type >= 6701) && (play_type <= 6729));
            }

            public override PlayType[] GetPlayTypeList()
            {
                PlayType[] Result = new PlayType[29];

                Result[0] = new PlayType(PlayType_ZhiD, "直选单式");
                Result[1] = new PlayType(PlayType_Zu3D, "组选3单式");
                Result[2] = new PlayType(PlayType_Zu6D, "组选6单式");
                Result[3] = new PlayType(PlayType_ZhiH, "直选和值");
                Result[4] = new PlayType(PlayType_Zu3H, "组选3和值");
                Result[5] = new PlayType(PlayType_Zu6H, "组选6和值");
                Result[6] = new PlayType(PlayType_ZuheH, "组合和值");
                Result[7] = new PlayType(PlayType_ZhiBW, "直选按位包号");
                Result[8] = new PlayType(PlayType_ZhiF, "直选复式");
                Result[9] = new PlayType(PlayType_Zu3F, "组选3复式");
                Result[10] = new PlayType(PlayType_Zu6F, "组选6复式");
                Result[11] = new PlayType(PlayType_ZuheF, "组合复式");
                Result[12] = new PlayType(PlayType_ZhiBC, "直选包串");
                Result[13] = new PlayType(PlayType_ZhiECH, "直选二重号");
                Result[14] = new PlayType(PlayType_ZhiDT, "直选胆拖");
                Result[15] = new PlayType(PlayType_ZuheDT, "组合胆拖");
                Result[16] = new PlayType(PlayType_ZhiKD, "直选跨度");
                Result[17] = new PlayType(PlayType_Zu3KD, "组选3跨度");
                Result[18] = new PlayType(PlayType_Zu6KD, "组选6跨度");
                Result[19] = new PlayType(PlayType_ZuheKD, "组合跨度");
                Result[20] = new PlayType(PlayType_ZuheHSW, "和数尾组合");
                Result[21] = new PlayType(PlayType_ZhiJO, "直选奇偶");
                Result[22] = new PlayType(PlayType_Zu3JO, "组选3奇偶");
                Result[23] = new PlayType(PlayType_Zu6JO, "组选6奇偶");
                Result[24] = new PlayType(PlayType_ZuheJO, "组合奇偶");
                Result[25] = new PlayType(PlayType_ZhiDX, "直选大小");
                Result[26] = new PlayType(PlayType_Zu3DX, "组选3大小");
                Result[27] = new PlayType(PlayType_Zu6DX, "组选6大小");
                Result[28] = new PlayType(PlayType_ZuheDX, "组合大小");

                return Result;
            }

            public override string BuildNumber(int Num)//id = 67
            {
                System.Random rd = new Random();
                StringBuilder sb = new StringBuilder();

                for (int i = 0; i < Num; i++)
                {
                    string LotteryNumber = "";
                    for (int j = 0; j < 3; j++)
                        LotteryNumber += rd.Next(0, 9 + 1).ToString();

                    sb.Append(LotteryNumber.Trim() + "\n");
                }

                string Result = sb.ToString();
                Result = Result.Substring(0, Result.Length - 1);
                return Result;
            }

            public override string[] ToSingle(string Number, ref string CanonicalNumber, int PlayType)
            {
                if ((PlayType == PlayType_ZhiD) || (PlayType == PlayType_ZhiBW))
                    return ToSingle_ZhiD_BW(Number, ref CanonicalNumber);

                if (PlayType == PlayType_ZhiF)
                    return ToSingle_ZhiF(Number, ref CanonicalNumber);

                if ((PlayType == PlayType_Zu3D) || (PlayType == PlayType_Zu6D) || (PlayType == PlayType_Zu6F))
                    return ToSingle_Zu3D_Zu6(Number, ref CanonicalNumber);

                if (PlayType == PlayType_Zu3F)
                    return ToSingle_Zu3F(Number, ref CanonicalNumber);

                if (PlayType == PlayType_ZuheF)
                    return ToSingle_ZuheF(Number, ref CanonicalNumber);

                if (PlayType == PlayType_ZhiH)
                    return ToSingle_ZhiH(Number, ref CanonicalNumber);

                if (PlayType == PlayType_Zu3H)
                    return ToSingle_Zu3H(Number, ref CanonicalNumber);

                if (PlayType == PlayType_Zu6H)
                    return ToSingle_Zu6H(Number, ref CanonicalNumber);

                if (PlayType == PlayType_ZuheH)
                    return ToSingle_ZuheH(Number, ref CanonicalNumber);

                if (PlayType == PlayType_ZhiBC)
                    return ToSingle_ZhiBC(Number, ref CanonicalNumber);

                if (PlayType == PlayType_ZhiECH)
                    return ToSingle_ZhiECH(Number, ref CanonicalNumber);

                if (PlayType == PlayType_ZhiDT)
                    return ToSingle_ZhiDT(Number, ref CanonicalNumber);

                if (PlayType == PlayType_ZuheDT)
                    return ToSingle_ZuheDT(Number, ref CanonicalNumber);

                if (PlayType == PlayType_ZhiKD)
                    return ToSingle_ZhiKD(Number, ref CanonicalNumber);

                if (PlayType == PlayType_Zu3KD)
                    return ToSingle_Zu3KD(Number, ref CanonicalNumber);

                if (PlayType == PlayType_Zu6KD)
                    return ToSingle_Zu6KD(Number, ref CanonicalNumber);

                if (PlayType == PlayType_ZuheKD)
                    return ToSingle_ZuheKD(Number, ref CanonicalNumber);

                if (PlayType == PlayType_ZuheHSW)
                    return ToSingle_ZuheHSW(Number, ref CanonicalNumber);

                if (PlayType == PlayType_ZhiJO)
                {
                    Number = Number.Replace("偶", "0").Replace("奇", "1");
                    return ToSingle_ZhiJO(Number, ref CanonicalNumber);
                }

                if (PlayType == PlayType_Zu3JO)
                {
                    Number = Number.Replace("偶", "0").Replace("奇", "1");
                    return ToSingle_Zu3JO(Number, ref CanonicalNumber);
                }

                if (PlayType == PlayType_Zu6JO)
                {
                    Number = Number.Replace("偶", "0").Replace("奇", "1");
                    return ToSingle_Zu6JO(Number, ref CanonicalNumber);
                }

                if (PlayType == PlayType_ZuheJO)
                {
                    Number = Number.Replace("偶", "0").Replace("奇", "1");
                    return ToSingle_ZuheJO(Number, ref CanonicalNumber);
                }

                if (PlayType == PlayType_ZhiDX)
                {
                    Number = Number.Replace("小", "0").Replace("大", "1");
                    return ToSingle_ZhiDX(Number, ref CanonicalNumber);
                }

                if (PlayType == PlayType_Zu3DX)
                {
                    Number = Number.Replace("小", "0").Replace("大", "1");
                    return ToSingle_Zu3DX(Number, ref CanonicalNumber);
                }

                if (PlayType == PlayType_Zu6DX)
                {
                    Number = Number.Replace("小", "0").Replace("大", "1");
                    return ToSingle_Zu6DX(Number, ref CanonicalNumber);
                }

                if (PlayType == PlayType_ZuheDX)
                {
                    Number = Number.Replace("小", "0").Replace("大", "1");
                    return ToSingle_ZuheDX(Number, ref CanonicalNumber);
                }

                return null;
            }
            #region ToSingle 的具体方法
            private string[] ToSingle_ZhiD_BW(string Number, ref string CanonicalNumber)//直选取单式, 后面 ref 参数是将彩票规范化，如：10(122) 变成10(12)
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

                if (al.Count == 0)
                    return null;

                string[] Result = new string[al.Count];
                for (int i = 0; i < al.Count; i++)
                    Result[i] = al[i].ToString();
                return Result;
            }
            private string[] ToSingle_Zu3D_Zu6(string Number, ref string CanonicalNumber)//组3组6取单式, 后面 ref 参数是将彩票规范化，如：0223 变成023
            {
                CanonicalNumber = FilterRepeated(Number.Trim());
                if (CanonicalNumber.Length < 2)
                {
                    CanonicalNumber = "";
                    return null;
                }

                CanonicalNumber = Number.Trim();

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

                if (al.Count == 0)
                    return null;

                string[] Result = new string[al.Count];
                for (int i = 0; i < al.Count; i++)
                    Result[i] = al[i].ToString();
                return Result;
            }
            private string[] ToSingle_ZhiH(string sBill, ref string CanonicalNumber)//直选和值取单式,后面 ref 参数是将彩票规范化，如：05 变成 5
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
            private string[] ToSingle_Zu3H(string sBill, ref string CanonicalNumber)//组选3和值取单式,后面 ref 参数是将彩票规范化，如：05 变成 5
            {
                int Bill = Shove._Convert.StrToInt(sBill, -1);
                CanonicalNumber = "";

                if ((Bill < 1) || (Bill > 26))
                {
                    CanonicalNumber = "";
                    return null;
                }

                CanonicalNumber = Bill.ToString();

                ArrayList al = new ArrayList();

                #region 循环取单式

                for (int i = 0; i <= 9; i++)
                {
                    for (int j = 0; j <= 9; j++)
                    {
                        if (i == j)
                            continue;
                        if (i + i + j == Bill)
                            al.Add(i.ToString() + i.ToString() + j.ToString());
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
            private string[] ToSingle_Zu6H(string sBill, ref string CanonicalNumber)//组选6和值取单式,后面 ref 参数是将彩票规范化，如：05 变成 5
            {
                int Bill = Shove._Convert.StrToInt(sBill, -1);
                CanonicalNumber = "";

                if ((Bill < 3) || (Bill > 24))
                {
                    CanonicalNumber = "";
                    return null;
                }

                CanonicalNumber = Bill.ToString();

                ArrayList al = new ArrayList();

                #region 循环取单式

                for (int i = 0; i <= 7; i++)
                    for (int j = i + 1; j <= 8; j++)
                        for (int k = j + 1; k <= 9; k++)
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
            private string[] ToSingle_ZuheH(string sBill, ref string CanonicalNumber)//组选和值取单式,后面 ref 参数是将彩票规范化，如：05 变成 5
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

                //直选
                for (int i = 0; i <= 9; i++)
                {
                    if (i + i + i == Bill)
                        al.Add(i.ToString() + i.ToString() + i.ToString());
                }

                //组选3
                for (int i = 0; i <= 9; i++)
                {
                    for (int j = 0; j <= 9; j++)
                    {
                        if (i == j)
                            continue;
                        if (i + i + j == Bill)
                            al.Add(i.ToString() + i.ToString() + j.ToString());
                    }
                }

                //组选6
                for (int i = 0; i <= 7; i++)
                    for (int j = i + 1; j <= 8; j++)
                        for (int k = j + 1; k <= 9; k++)
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
            private string[] ToSingle_ZhiF(string Number, ref string CanonicalNumber)//组合复式取单式, 后面 ref 参数是将彩票规范化，如：10223 变成1023
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

                for (int x = 0; x < n; x++)
                    for (int y = 0; y < n; y++)
                        for (int z = 0; z < n; z++)
                            if ((x != y) && (x != z) && (y != z))
                            {
                                al.Add(strs[x].ToString() + strs[y].ToString() + strs[z].ToString());
                            }

                #endregion

                if (al.Count == 0)
                    return null;

                string[] Result = new string[al.Count];
                for (int m = 0; m < al.Count; m++)
                    Result[m] = al[m].ToString();
                return Result;
            }
            private string[] ToSingle_Zu3F(string Number, ref string CanonicalNumber)//组选3复式取单式,后面 ref 参数是将彩票规范化，如：10223 变成1023
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
            private string[] ToSingle_ZuheF(string Number, ref string CanonicalNumber)//组合复式取单式, 后面 ref 参数是将彩票规范化，如：10223 变成1023
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

                //组选3数字
                for (int p = 0; p < n - 1; p++)
                    for (int q = p + 1; q < n; q++)
                    {
                        al.Add(strs[p].ToString() + strs[p].ToString() + strs[q].ToString());
                        al.Add(strs[p].ToString() + strs[q].ToString() + strs[q].ToString());
                    }

                //相同的数字
                for (int x = 0; x < n; x++)
                    for (int y = 0; y < n; y++)
                        for (int z = 0; z < n; z++)
                            if ((x == y) && (x == z) && (y == z))
                            {
                                al.Add(strs[x].ToString() + strs[y].ToString() + strs[z].ToString());
                            }

                //组选6数字
                for (int i = 0; i < n - 2; i++)
                    for (int j = i + 1; j < n - 1; j++)
                        for (int k = j + 1; k < n; k++)
                            al.Add(strs[i].ToString() + strs[j].ToString() + strs[k].ToString());

                #endregion

                if (al.Count == 0)
                    return null;

                string[] Result = new string[al.Count];
                for (int m = 0; m < al.Count; m++)
                    Result[m] = al[m].ToString();
                return Result;
            }
            private string[] ToSingle_ZhiBC(string Number, ref string CanonicalNumber)	//直选包串取单式, 后面 ref 参数是将彩票规范化，如：10223 变成1023
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

                for (int i = 0; i < n; i++)
                    for (int j = 0; j < n; j++)
                        for (int k = 0; k < n; k++)
                            al.Add(strs[i].ToString() + strs[j].ToString() + strs[k].ToString());

                #endregion

                if (al.Count == 0)
                    return null;

                string[] Result = new string[al.Count];
                for (int i = 0; i < al.Count; i++)
                    Result[i] = al[i].ToString();
                return Result;
            }
            private string[] ToSingle_ZhiECH(string Number, ref string CanonicalNumber)//直选二重号取单式, 后面 ref 参数是将彩票规范化，如：10223 变成1023
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

                for (int i = 0; i < n; i++)
                    for (int j = 0; j < n; j++)
                        if (i != j)
                        {
                            al.Add(strs[i].ToString() + strs[i].ToString() + strs[j].ToString());
                            al.Add(strs[i].ToString() + strs[j].ToString() + strs[i].ToString());
                            al.Add(strs[j].ToString() + strs[i].ToString() + strs[i].ToString());
                        }

                #endregion

                if (al.Count == 0)
                    return null;

                string[] Result = new string[al.Count];
                for (int i = 0; i < al.Count; i++)
                    Result[i] = al[i].ToString();
                return Result;
            }
            private string[] ToSingle_ZhiDT(string Number, ref string CanonicalNumber)	//直选胆拖取单式, 后面 ref 参数是将彩票规范化，如：123,345 变成23,45
            {
                string[] Locate = new string[2];
                CanonicalNumber = "";

                Regex regex = new Regex(@"^(?<L0>(\d){1,2})[,](?<L1>(\d){2,})", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                Match m = regex.Match(Number);

                for (int i = 0; i < 2; i++)
                {
                    Locate[i] = m.Groups["L" + i.ToString()].ToString().Trim();
                    if (Locate[i] == "")
                    {
                        CanonicalNumber = "";
                        return null;
                    }
                }

                char[] strs_Dan = Locate[0].ToCharArray();
                char[] strs_Tuo = FilterRepeated(Locate[1]).ToCharArray();

                for (int i = 0; i < strs_Tuo.Length; i++)
                {
                    if (Locate[0].IndexOf(strs_Tuo[i]) >= 0)
                    {
                        CanonicalNumber = "";
                        return null;
                    }
                }

                CanonicalNumber = Locate[0] + "," + FilterRepeated(Locate[1]);

                ArrayList al = new ArrayList();

                #region 循环取单式

                int DanLength = strs_Dan.Length;
                int TuoLength = strs_Tuo.Length;

                if (DanLength == 1)
                {
                    al.Add(strs_Dan[0].ToString() + strs_Dan[0].ToString() + strs_Dan[0].ToString());

                    for (int p = 0; p < TuoLength; p++)
                    {
                        al.Add(strs_Dan[0].ToString() + strs_Dan[0].ToString() + strs_Tuo[p].ToString());
                        al.Add(strs_Dan[0].ToString() + strs_Tuo[p].ToString() + strs_Dan[0].ToString());
                        al.Add(strs_Tuo[p].ToString() + strs_Dan[0].ToString() + strs_Dan[0].ToString());
                        for (int q = 0; q < TuoLength; q++)
                        {
                            al.Add(strs_Dan[0].ToString() + strs_Tuo[p].ToString() + strs_Tuo[q].ToString());
                            al.Add(strs_Tuo[p].ToString() + strs_Dan[0].ToString() + strs_Tuo[q].ToString());
                            al.Add(strs_Tuo[p].ToString() + strs_Tuo[q].ToString() + strs_Dan[0].ToString());
                        }
                    }
                }
                else if (DanLength == 2)
                {
                    //胆码组合 
                    if (strs_Dan[0] != strs_Dan[1])
                    {
                        for (int i = 0; i < DanLength; i++)
                            for (int j = 0; j < DanLength; j++)
                                if (i != j)
                                {
                                    al.Add(strs_Dan[i].ToString() + strs_Dan[i].ToString() + strs_Dan[j].ToString());
                                    al.Add(strs_Dan[i].ToString() + strs_Dan[j].ToString() + strs_Dan[i].ToString());
                                    al.Add(strs_Dan[j].ToString() + strs_Dan[i].ToString() + strs_Dan[i].ToString());
                                }
                    }
                    else
                    {
                        al.Add(strs_Dan[0].ToString() + strs_Dan[0].ToString() + strs_Dan[0].ToString());
                    }

                    //胆码托码组合
                    if (strs_Dan[0] != strs_Dan[1])
                    {
                        for (int x = 0; x < DanLength; x++)
                            for (int y = 0; y < DanLength; y++)
                                for (int z = 0; z < TuoLength; z++)
                                    if (x != y)
                                    {
                                        al.Add(strs_Dan[x].ToString() + strs_Dan[y].ToString() + strs_Tuo[z].ToString());
                                        al.Add(strs_Dan[x].ToString() + strs_Tuo[z].ToString() + strs_Dan[y].ToString());
                                        al.Add(strs_Tuo[z].ToString() + strs_Dan[x].ToString() + strs_Dan[y].ToString());
                                    }
                    }
                    else
                    {
                        for (int z = 0; z < TuoLength; z++)
                        {
                            al.Add(strs_Dan[0].ToString() + strs_Dan[0].ToString() + strs_Tuo[z].ToString());
                            al.Add(strs_Dan[0].ToString() + strs_Tuo[z].ToString() + strs_Dan[0].ToString());
                            al.Add(strs_Tuo[z].ToString() + strs_Dan[0].ToString() + strs_Dan[0].ToString());
                        }
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
            private string[] ToSingle_ZuheDT(string Number, ref string CanonicalNumber)//组合胆拖取单式, 后面 ref 参数是将彩票规范化，如：123,345 变成23,45
            {
                string[] Locate = new string[2];
                CanonicalNumber = "";

                Regex regex = new Regex(@"^(?<L0>(\d){1,2})[,](?<L1>(\d){2,})", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                Match m = regex.Match(Number);

                for (int i = 0; i < 2; i++)
                {
                    Locate[i] = m.Groups["L" + i.ToString()].ToString().Trim();
                    if (Locate[i] == "")
                    {
                        CanonicalNumber = "";
                        return null;
                    }
                }

                char[] strs_Dan = Locate[0].ToCharArray();
                char[] strs_Tuo = FilterRepeated(Locate[1]).ToCharArray();

                for (int i = 0; i < strs_Tuo.Length; i++)
                {
                    if (Locate[0].IndexOf(strs_Tuo[i]) >= 0)
                    {
                        CanonicalNumber = "";
                        return null;
                    }
                }

                CanonicalNumber = Locate[0] + "," + FilterRepeated(Locate[1]);

                ArrayList al = new ArrayList();

                #region 循环取单式

                int DanLength = strs_Dan.Length;
                int TuoLength = strs_Tuo.Length;

                if (DanLength == 1)
                {
                    //豹子号
                    al.Add(strs_Dan[0].ToString() + strs_Dan[0].ToString() + strs_Dan[0].ToString());

                    //组选3组合
                    for (int i = 0; i < TuoLength; i++)
                    {
                        al.Add(strs_Dan[0].ToString() + strs_Dan[0].ToString() + strs_Tuo[i].ToString());
                        al.Add(strs_Dan[0].ToString() + strs_Tuo[i].ToString() + strs_Tuo[i].ToString());
                    }

                    //组选6组合
                    for (int i = 0; i < TuoLength - 1; i++)
                        for (int j = i + 1; j < TuoLength; j++)
                            al.Add(strs_Dan[0].ToString() + strs_Tuo[i].ToString() + strs_Tuo[j].ToString());
                }
                else if (DanLength == 2)
                {
                    //胆码组合
                    if (strs_Dan[0] != strs_Dan[1])
                    {
                        al.Add(strs_Dan[0].ToString() + strs_Dan[0].ToString() + strs_Dan[1].ToString());
                        al.Add(strs_Dan[0].ToString() + strs_Dan[1].ToString() + strs_Dan[1].ToString());
                    }
                    else
                    {
                        al.Add(strs_Dan[0].ToString() + strs_Dan[0].ToString() + strs_Dan[0].ToString());
                    }

                    //胆码托码组合
                    for (int i = 0; i < TuoLength; i++)
                        al.Add(strs_Dan[0].ToString() + strs_Dan[1].ToString() + strs_Tuo[i].ToString());
                }

                #endregion

                if (al.Count == 0)
                    return null;

                string[] Result = new string[al.Count];
                for (int i = 0; i < al.Count; i++)
                    Result[i] = al[i].ToString();
                return Result;
            }
            private string[] ToSingle_ZhiKD(string sBill, ref string CanonicalNumber)//直选跨度取单式,后面 ref 参数是将彩票规范化，如：05 变成5
            {
                int Bill = Shove._Convert.StrToInt(sBill, -1);
                CanonicalNumber = "";

                if ((Bill < 0) || (Bill > 9))
                {
                    CanonicalNumber = "";
                    return null;
                }

                CanonicalNumber = Bill.ToString();

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

                if (alnumber.Count == 0)
                    return null;

                string[] Result = new string[alnumber.Count];

                for (int i = 0; i < alnumber.Count; i++)
                {
                    Result[i] = alnumber[i].ToString();
                }

                return Result;
            }
            private string[] ToSingle_Zu3KD(string sBill, ref string CanonicalNumber)//组选3跨度取单式,后面 ref 参数是将彩票规范化，如：05 变成5
            {
                int Bill = Shove._Convert.StrToInt(sBill, -1);
                CanonicalNumber = "";

                if ((Bill < 1) || (Bill > 9))
                {
                    CanonicalNumber = "";
                    return null;
                }

                CanonicalNumber = Bill.ToString();

                #region 循环取单式
                ArrayList al = new ArrayList();

                for (int i = 0; i < (10 - int.Parse(CanonicalNumber)); i++)
                {
                    al.Add(i.ToString() + (int.Parse(CanonicalNumber) + i).ToString() + (int.Parse(CanonicalNumber) + i).ToString());
                    al.Add(i.ToString() + i.ToString() + (int.Parse(CanonicalNumber) + i).ToString());
                }
                #endregion

                if (al.Count == 0)
                    return null;

                string[] Result = new string[al.Count];

                for (int i = 0; i < al.Count; i++)
                {
                    Result[i] = al[i].ToString();
                }

                return Result;
            }
            private string[] ToSingle_Zu6KD(string sBill, ref string CanonicalNumber)//组6跨度取单式,后面 ref 参数是将彩票规范化，如：05 变成5
            {
                int Bill = Shove._Convert.StrToInt(sBill, -1);
                CanonicalNumber = "";

                if ((Bill < 2) || (Bill > 9))
                {
                    CanonicalNumber = "";
                    return null;
                }

                CanonicalNumber = Bill.ToString();

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

                if (al.Count == 0)
                    return null;

                string[] Result = new string[al.Count];

                for (int i = 0; i < al.Count; i++)
                {
                    Result[i] = al[i].ToString();
                }

                return Result;
            }
            private string[] ToSingle_ZuheKD(string sBill, ref string CanonicalNumber)//组合跨度取单式,后面 ref 参数是将彩票规范化，如：05 变成5
            {
                int Bill = Shove._Convert.StrToInt(sBill, -1);
                CanonicalNumber = "";

                if ((Bill < 1) || (Bill > 9))
                {
                    CanonicalNumber = "";
                    return null;
                }

                CanonicalNumber = Bill.ToString();

                #region 循环取单式
                ArrayList al = new ArrayList();

                //组选3
                for (int i = 0; i < (10 - int.Parse(CanonicalNumber)); i++)
                {
                    al.Add(i.ToString() + (int.Parse(CanonicalNumber) + i).ToString() + (int.Parse(CanonicalNumber) + i).ToString());
                    al.Add(i.ToString() + i.ToString() + (int.Parse(CanonicalNumber) + i).ToString());
                }

                //组选6
                for (int i = 0; i < (10 - int.Parse(CanonicalNumber)); i++)
                {
                    for (int j = i + 1; j < (int.Parse(CanonicalNumber) + i); j++)
                    {
                        al.Add(i.ToString() + j.ToString() + (int.Parse(CanonicalNumber) + i).ToString());
                    }
                }

                #endregion

                if (al.Count == 0)
                    return null;

                string[] Result = new string[al.Count];

                for (int i = 0; i < al.Count; i++)
                {
                    Result[i] = al[i].ToString();
                }

                return Result;
            }
            private string[] ToSingle_ZuheHSW(string sBill, ref string CanonicalNumber)//和数尾组合取单式,后面 ref 参数是将彩票规范化，如：05 变成5
            {
                int Bill = Shove._Convert.StrToInt(sBill, -1);
                CanonicalNumber = "";

                if ((Bill < 0) || (Bill > 9))
                {
                    CanonicalNumber = "";
                    return null;
                }

                CanonicalNumber = Bill.ToString();

                #region 循环取单式
                ArrayList al = new ArrayList();

                //单选和组选3
                for (int n = 0; n < 3; n++)
                {
                    for (int i = 0; i < 10; i++)
                    {
                        for (int j = 0; j < 10; j++)
                        {
                            if ((i + j + j) == Int32.Parse((n == 0 ? null : n.ToString()) + CanonicalNumber))
                                al.Add(i.ToString() + j.ToString() + j.ToString());
                        }
                    }
                }

                //组选6
                for (int n = 0; n < 3; n++)
                {
                    for (int i = 0; i < 8; i++)
                    {
                        for (int j = i + 1; j < 9; j++)
                        {
                            for (int k = j + 1; k < 10; k++)
                            {
                                if ((i + j + k) == Int32.Parse((n == 0 ? null : n.ToString()) + CanonicalNumber))
                                    al.Add(i.ToString() + j.ToString() + k.ToString());
                            }
                        }
                    }
                }

                #endregion

                if (al.Count == 0)
                    return null;

                string[] Result = new string[al.Count];

                for (int i = 0; i < al.Count; i++)
                {
                    Result[i] = al[i].ToString();
                }

                return Result;
            }
            private string[] ToSingle_ZhiJO(string Number, ref string CanonicalNumber)//直选奇偶取单式,后面 ref 参数是将彩票规范化，如：0113 变成011
            {
                Regex regexNumber = new Regex(@"^[0-1][0-1][0-1]{1}", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                Match mNumber = regexNumber.Match(Number);

                if (mNumber.Success)
                {
                    Number = Number.Substring(0, 3);
                    CanonicalNumber = Number;
                }
                else
                {
                    CanonicalNumber = "";
                    return null;
                }

                Number = Number.Replace("1", "(13579)").Replace("0", "(02468)");

                string[] Locate = new string[3];

                Regex regex = new Regex(@"^(?<L0>(\d)|([(][\d]+?[)]))(?<L1>(\d)|([(][\d]+?[)]))(?<L2>(\d)|([(][\d]+?[)]))", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                Match m = regex.Match(Number);
                for (int i = 0; i < 3; i++)
                {
                    Locate[i] = m.Groups["L" + i.ToString()].ToString().Trim();
                    if (Locate[i] == "")
                    {
                        return null;
                    }
                    if (Locate[i].Length > 1)
                    {
                        Locate[i] = Locate[i].Substring(1, Locate[i].Length - 2);
                        if (Locate[i].Length > 1)
                            Locate[i] = FilterRepeated(Locate[i]);
                        if (Locate[i] == "")
                        {
                            return null;
                        }
                    }
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

                if (al.Count == 0)
                    return null;

                string[] Result = new string[al.Count];
                for (int i = 0; i < al.Count; i++)
                    Result[i] = al[i].ToString();
                return Result;
            }
            private string[] ToSingle_Zu3JO(string Number, ref string CanonicalNumber)//组选3奇偶取单式,后面 ref 参数是将彩票规范化，如：0113 变成011
            {
                CanonicalNumber = "";

                Regex regexNumber = new Regex(@"^[0-1][0-1][0-1]{1}", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                Match mNumber = regexNumber.Match(Number);

                if (mNumber.Success)
                {
                    Number = Number.Substring(0, 3);
                    CanonicalNumber = Number;
                }
                else
                {
                    CanonicalNumber = "";
                    return null;
                }

                char[] strs = Number.ToCharArray();

                Number = Number.Replace("1", "(13579)").Replace("0", "(02468)");

                string[] Locate = new string[3];

                Regex regex = new Regex(@"^(?<L0>(\d)|([(][\d]+?[)]))(?<L1>(\d)|([(][\d]+?[)]))(?<L2>(\d)|([(][\d]+?[)]))", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                Match m = regex.Match(Number);
                for (int i = 0; i < 3; i++)
                {
                    Locate[i] = m.Groups["L" + i.ToString()].ToString().Trim();
                    Locate[i] = Locate[i].Substring(1, Locate[i].Length - 2);
                    Locate[i] = FilterRepeated(Locate[i]);
                }

                ArrayList al = new ArrayList();

                #region 循环取单式

                //000;111
                if ((strs[0] == strs[1]) && (strs[0] == strs[2]))
                {
                    for (int i_0 = 0; i_0 < Locate[0].Length; i_0++)
                    {
                        for (int i_1 = 0; i_1 < Locate[1].Length; i_1++)
                        {
                            if (i_0 != i_1)
                            {
                                al.Add(Locate[0][i_0].ToString() + Locate[0][i_0].ToString() + Locate[1][i_1].ToString());
                            }
                        }
                    }
                }

                //001;110
                if ((strs[0] == strs[1]) && (strs[0] != strs[2]))
                {
                    for (int i_0 = 0; i_0 < Locate[0].Length; i_0++)
                    {
                        for (int i_2 = 0; i_2 < Locate[2].Length; i_2++)
                        {
                            al.Add(Locate[0][i_0].ToString() + Locate[0][i_0].ToString() + Locate[2][i_2].ToString());
                        }
                    }
                }

                //010;101
                if ((strs[0] == strs[2]) && (strs[0] != strs[1]))
                {
                    for (int i_0 = 0; i_0 < Locate[0].Length; i_0++)
                    {
                        for (int i_1 = 0; i_1 < Locate[1].Length; i_1++)
                        {
                            al.Add(Locate[0][i_0].ToString() + Locate[0][i_0].ToString() + Locate[1][i_1].ToString());
                        }
                    }
                }

                //011;100
                if ((strs[1] == strs[2]) && (strs[0] != strs[1]))
                {
                    for (int i_1 = 0; i_1 < Locate[1].Length; i_1++)
                    {
                        for (int i_0 = 0; i_0 < Locate[0].Length; i_0++)
                        {
                            al.Add(Locate[0][i_0].ToString() + Locate[1][i_1].ToString() + Locate[1][i_1].ToString());
                        }
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
            private string[] ToSingle_Zu6JO(string Number, ref string CanonicalNumber)//组选6奇偶取单式,后面 ref 参数是将彩票规范化，如：0113 变成011
            {
                CanonicalNumber = "";

                Regex regexNumber = new Regex(@"^[0-1][0-1][0-1]{1}", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                Match mNumber = regexNumber.Match(Number);

                if (mNumber.Success)
                {
                    Number = Number.Substring(0, 3);
                    CanonicalNumber = Number;
                }
                else
                {
                    CanonicalNumber = "";
                    return null;
                }

                char[] strs = Number.ToCharArray();

                Number = Number.Replace("1", "(13579)").Replace("0", "(02468)");

                string[] Locate = new string[3];

                Regex regex = new Regex(@"^(?<L0>(\d)|([(][\d]+?[)]))(?<L1>(\d)|([(][\d]+?[)]))(?<L2>(\d)|([(][\d]+?[)]))", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                Match m = regex.Match(Number);
                for (int i = 0; i < 3; i++)
                {
                    Locate[i] = m.Groups["L" + i.ToString()].ToString().Trim();
                    Locate[i] = Locate[i].Substring(1, Locate[i].Length - 2);
                    Locate[i] = FilterRepeated(Locate[i]);
                }

                ArrayList al = new ArrayList();

                #region 循环取单式

                //000;111
                if ((strs[0] == strs[1]) && (strs[0] == strs[2]))
                {
                    for (int i_0 = 0; i_0 < Locate[0].Length - 2; i_0++)
                    {
                        for (int i_1 = i_0 + 1; i_1 < Locate[1].Length - 1; i_1++)
                        {
                            for (int i_2 = i_1 + 1; i_2 < Locate[2].Length; i_2++)
                            {
                                al.Add(Locate[0][i_0].ToString() + Locate[1][i_1].ToString() + Locate[2][i_2].ToString());
                            }
                        }
                    }
                }

                //001;110
                if ((strs[0] == strs[1]) && (strs[0] != strs[2]))
                {
                    for (int i_0 = 0; i_0 < Locate[0].Length - 1; i_0++)
                    {
                        for (int i_1 = i_0 + 1; i_1 < Locate[1].Length; i_1++)
                        {
                            for (int i_2 = 0; i_2 < Locate[2].Length; i_2++)
                            {
                                al.Add(Locate[0][i_0].ToString() + Locate[1][i_1].ToString() + Locate[2][i_2].ToString());
                            }
                        }
                    }
                }

                //010;101
                if ((strs[0] == strs[2]) && (strs[0] != strs[1]))
                {
                    for (int i_0 = 0; i_0 < Locate[0].Length - 1; i_0++)
                    {
                        for (int i_1 = 0; i_1 < Locate[1].Length; i_1++)
                        {
                            for (int i_2 = i_0 + 1; i_2 < Locate[2].Length; i_2++)
                            {
                                al.Add(Locate[0][i_0].ToString() + Locate[1][i_1].ToString() + Locate[2][i_2].ToString());
                            }
                        }
                    }
                }

                //011;100
                if ((strs[1] == strs[2]) && (strs[0] != strs[1]))
                {
                    for (int i_0 = 0; i_0 < Locate[0].Length; i_0++)
                    {
                        for (int i_1 = 0; i_1 < Locate[1].Length - 1; i_1++)
                        {
                            for (int i_2 = i_1 + 1; i_2 < Locate[2].Length; i_2++)
                            {
                                al.Add(Locate[0][i_0].ToString() + Locate[1][i_1].ToString() + Locate[2][i_2].ToString());
                            }
                        }
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
            private string[] ToSingle_ZuheJO(string Number, ref string CanonicalNumber)//组合奇偶取单式,后面 ref 参数是将彩票规范化，如：0113 变成011
            {
                CanonicalNumber = "";

                Regex regexNumber = new Regex(@"^[0-1][0-1][0-1]{1}", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                Match mNumber = regexNumber.Match(Number);

                if (mNumber.Success)
                {
                    Number = Number.Substring(0, 3);
                    CanonicalNumber = Number;
                }
                else
                {
                    CanonicalNumber = "";
                    return null;
                }

                char[] strs = Number.ToCharArray();

                Number = Number.Replace("1", "(13579)").Replace("0", "(02468)");

                string[] Locate = new string[3];

                Regex regex = new Regex(@"^(?<L0>(\d)|([(][\d]+?[)]))(?<L1>(\d)|([(][\d]+?[)]))(?<L2>(\d)|([(][\d]+?[)]))", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                Match m = regex.Match(Number);
                for (int i = 0; i < 3; i++)
                {
                    Locate[i] = m.Groups["L" + i.ToString()].ToString().Trim();
                    Locate[i] = Locate[i].Substring(1, Locate[i].Length - 2);
                    Locate[i] = FilterRepeated(Locate[i]);
                }

                ArrayList al = new ArrayList();

                #region 循环取单式

                #region 000;111
                if ((strs[0] == strs[1]) && (strs[0] == strs[2]))
                {
                    //叠加
                    for (int i_0 = 0; i_0 < Locate[0].Length; i_0++)
                    {
                        al.Add(Locate[0][i_0].ToString() + Locate[0][i_0].ToString() + Locate[0][i_0].ToString());
                    }

                    //组6
                    for (int i_0 = 0; i_0 < Locate[0].Length - 2; i_0++)
                    {
                        for (int i_1 = i_0 + 1; i_1 < Locate[1].Length - 1; i_1++)
                        {
                            for (int i_2 = i_1 + 1; i_2 < Locate[2].Length; i_2++)
                            {
                                al.Add(Locate[0][i_0].ToString() + Locate[1][i_1].ToString() + Locate[2][i_2].ToString());
                            }
                        }
                    }

                    //组3
                    for (int i_0 = 0; i_0 < Locate[0].Length; i_0++)
                    {
                        for (int i_1 = 0; i_1 < Locate[1].Length; i_1++)
                        {
                            if (i_0 != i_1)
                            {
                                al.Add(Locate[0][i_0].ToString() + Locate[0][i_0].ToString() + Locate[1][i_1].ToString());
                            }
                        }
                    }
                }
                #endregion

                #region 001;110
                if ((strs[0] == strs[1]) && (strs[0] != strs[2]))
                {
                    //组6
                    for (int i_0 = 0; i_0 < Locate[0].Length - 1; i_0++)
                    {
                        for (int i_1 = i_0 + 1; i_1 < Locate[1].Length; i_1++)
                        {
                            for (int i_2 = 0; i_2 < Locate[2].Length; i_2++)
                            {
                                al.Add(Locate[0][i_0].ToString() + Locate[1][i_1].ToString() + Locate[2][i_2].ToString());
                            }
                        }
                    }

                    //组3
                    for (int i_0 = 0; i_0 < Locate[0].Length; i_0++)
                    {
                        for (int i_2 = 0; i_2 < Locate[2].Length; i_2++)
                        {
                            al.Add(Locate[0][i_0].ToString() + Locate[0][i_0].ToString() + Locate[2][i_2].ToString());
                        }
                    }
                }
                #endregion

                #region 010;101
                if ((strs[0] == strs[2]) && (strs[0] != strs[1]))
                {
                    //组6
                    for (int i_0 = 0; i_0 < Locate[0].Length - 1; i_0++)
                    {
                        for (int i_1 = 0; i_1 < Locate[1].Length; i_1++)
                        {
                            for (int i_2 = i_0 + 1; i_2 < Locate[2].Length; i_2++)
                            {
                                al.Add(Locate[0][i_0].ToString() + Locate[1][i_1].ToString() + Locate[2][i_2].ToString());
                            }
                        }
                    }

                    //组3
                    for (int i_0 = 0; i_0 < Locate[0].Length; i_0++)
                    {
                        for (int i_1 = 0; i_1 < Locate[1].Length; i_1++)
                        {
                            al.Add(Locate[0][i_0].ToString() + Locate[0][i_0].ToString() + Locate[1][i_1].ToString());
                        }
                    }
                }
                #endregion

                #region 011;100
                if ((strs[1] == strs[2]) && (strs[0] != strs[1]))
                {
                    //组6
                    for (int i_0 = 0; i_0 < Locate[0].Length; i_0++)
                    {
                        for (int i_1 = 0; i_1 < Locate[1].Length - 1; i_1++)
                        {
                            for (int i_2 = i_1 + 1; i_2 < Locate[2].Length; i_2++)
                            {
                                al.Add(Locate[0][i_0].ToString() + Locate[1][i_1].ToString() + Locate[2][i_2].ToString());
                            }
                        }
                    }

                    //组3
                    for (int i_1 = 0; i_1 < Locate[1].Length; i_1++)
                    {
                        for (int i_0 = 0; i_0 < Locate[0].Length; i_0++)
                        {
                            al.Add(Locate[1][i_1].ToString() + Locate[1][i_1].ToString() + Locate[0][i_0].ToString());
                        }
                    }
                }
                #endregion

                #endregion

                if (al.Count == 0)
                    return null;

                string[] Result = new string[al.Count];
                for (int i = 0; i < al.Count; i++)
                    Result[i] = al[i].ToString();
                return Result;
            }
            private string[] ToSingle_ZhiDX(string Number, ref string CanonicalNumber)//直选大小取单式,后面 ref 参数是将彩票规范化，如：0113 变成011
            {
                Regex regexNumber = new Regex(@"^[0-1][0-1][0-1]{1}", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                Match mNumber = regexNumber.Match(Number);

                if (mNumber.Success)
                {
                    Number = Number.Substring(0, 3);
                    CanonicalNumber = Number;
                }
                else
                {
                    CanonicalNumber = "";
                    return null;
                }

                Number = Number.Replace("1", "(56789)").Replace("0", "(01234)");

                string[] Locate = new string[3];

                Regex regex = new Regex(@"^(?<L0>(\d)|([(][\d]+?[)]))(?<L1>(\d)|([(][\d]+?[)]))(?<L2>(\d)|([(][\d]+?[)]))", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                Match m = regex.Match(Number);
                for (int i = 0; i < 3; i++)
                {
                    Locate[i] = m.Groups["L" + i.ToString()].ToString().Trim();
                    if (Locate[i] == "")
                    {
                        return null;
                    }
                    if (Locate[i].Length > 1)
                    {
                        Locate[i] = Locate[i].Substring(1, Locate[i].Length - 2);
                        if (Locate[i].Length > 1)
                            Locate[i] = FilterRepeated(Locate[i]);
                        if (Locate[i] == "")
                        {
                            return null;
                        }
                    }
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

                if (al.Count == 0)
                    return null;

                string[] Result = new string[al.Count];
                for (int i = 0; i < al.Count; i++)
                    Result[i] = al[i].ToString();
                return Result;
            }
            private string[] ToSingle_Zu3DX(string Number, ref string CanonicalNumber)//组选3大小取单式,后面 ref 参数是将彩票规范化，如：0113 变成011
            {
                CanonicalNumber = "";

                char[] strs = Number.ToCharArray();

                Regex regexNumber = new Regex(@"^[0-1][0-1][0-1]{1}", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                Match mNumber = regexNumber.Match(Number);

                if (mNumber.Success)
                {
                    if (!((strs[0] == strs[1]) && (strs[0] == strs[2]) && (strs[1] == strs[2])))
                    {
                        if ((strs[0] == '1') || (strs[2] == '0'))
                        {
                            CanonicalNumber = "";
                            return null;
                        }
                    }

                    Number = Number.Substring(0, 3);
                    CanonicalNumber = Number;
                }
                else
                {
                    CanonicalNumber = "";
                    return null;
                }

                Number = Number.Replace("1", "(56789)").Replace("0", "(01234)");

                string[] Locate = new string[3];

                Regex regex = new Regex(@"^(?<L0>(\d)|([(][\d]+?[)]))(?<L1>(\d)|([(][\d]+?[)]))(?<L2>(\d)|([(][\d]+?[)]))", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                Match m = regex.Match(Number);
                for (int i = 0; i < 3; i++)
                {
                    Locate[i] = m.Groups["L" + i.ToString()].ToString().Trim();
                    Locate[i] = Locate[i].Substring(1, Locate[i].Length - 2);
                    Locate[i] = FilterRepeated(Locate[i]);
                }

                ArrayList al = new ArrayList();

                #region 循环取单式

                //000;111
                if ((strs[0] == strs[1]) && (strs[0] == strs[2]))
                {
                    for (int i_0 = 0; i_0 < Locate[0].Length; i_0++)
                    {
                        for (int i_1 = 0; i_1 < Locate[1].Length; i_1++)
                        {
                            if (i_0 != i_1)
                            {
                                al.Add(Locate[0][i_0].ToString() + Locate[0][i_0].ToString() + Locate[1][i_1].ToString());
                            }
                        }
                    }
                }

                //001;110
                if ((strs[0] == strs[1]) && (strs[0] != strs[2]))
                {
                    for (int i_0 = 0; i_0 < Locate[0].Length; i_0++)
                    {
                        for (int i_2 = 0; i_2 < Locate[2].Length; i_2++)
                        {
                            al.Add(Locate[0][i_0].ToString() + Locate[0][i_0].ToString() + Locate[2][i_2].ToString());
                        }
                    }
                }

                //011;100
                if ((strs[1] == strs[2]) && (strs[0] != strs[1]))
                {
                    for (int i_1 = 0; i_1 < Locate[1].Length; i_1++)
                    {
                        for (int i_0 = 0; i_0 < Locate[0].Length; i_0++)
                        {
                            al.Add(Locate[0][i_0].ToString() + Locate[1][i_1].ToString() + Locate[1][i_1].ToString());
                        }
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
            private string[] ToSingle_Zu6DX(string Number, ref string CanonicalNumber)//组选6大小取单式,后面 ref 参数是将彩票规范化，如：0113 变成011
            {
                CanonicalNumber = "";

                char[] strs = Number.ToCharArray();

                Regex regexNumber = new Regex(@"^[0-1][0-1][0-1]{1}", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                Match mNumber = regexNumber.Match(Number);

                if (mNumber.Success)
                {
                    if (!((strs[0] == strs[1]) && (strs[0] == strs[2]) && (strs[1] == strs[2])))
                    {
                        if ((strs[0] == '1') || (strs[2] == '0'))
                        {
                            CanonicalNumber = "";
                            return null;
                        }
                    }

                    Number = Number.Substring(0, 3);
                    CanonicalNumber = Number;
                }
                else
                {
                    CanonicalNumber = "";
                    return null;
                }

                Number = Number.Replace("1", "(56789)").Replace("0", "(01234)");

                string[] Locate = new string[3];

                Regex regex = new Regex(@"^(?<L0>(\d)|([(][\d]+?[)]))(?<L1>(\d)|([(][\d]+?[)]))(?<L2>(\d)|([(][\d]+?[)]))", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                Match m = regex.Match(Number);
                for (int i = 0; i < 3; i++)
                {
                    Locate[i] = m.Groups["L" + i.ToString()].ToString().Trim();
                    Locate[i] = Locate[i].Substring(1, Locate[i].Length - 2);
                    Locate[i] = FilterRepeated(Locate[i]);
                }

                ArrayList al = new ArrayList();

                #region 循环取单式

                //000;111
                if ((strs[0] == strs[1]) && (strs[0] == strs[2]))
                {
                    for (int i_0 = 0; i_0 < Locate[0].Length - 2; i_0++)
                    {
                        for (int i_1 = i_0 + 1; i_1 < Locate[1].Length - 1; i_1++)
                        {
                            for (int i_2 = i_1 + 1; i_2 < Locate[2].Length; i_2++)
                            {
                                al.Add(Locate[0][i_0].ToString() + Locate[1][i_1].ToString() + Locate[2][i_2].ToString());
                            }
                        }
                    }
                }

                //001;110
                if ((strs[0] == strs[1]) && (strs[0] != strs[2]))
                {
                    for (int i_0 = 0; i_0 < Locate[0].Length - 1; i_0++)
                    {
                        for (int i_1 = i_0 + 1; i_1 < Locate[1].Length; i_1++)
                        {
                            for (int i_2 = 0; i_2 < Locate[2].Length; i_2++)
                            {
                                al.Add(Locate[0][i_0].ToString() + Locate[1][i_1].ToString() + Locate[2][i_2].ToString());
                            }
                        }
                    }
                }

                //011;100
                if ((strs[1] == strs[2]) && (strs[0] != strs[1]))
                {
                    for (int i_0 = 0; i_0 < Locate[0].Length; i_0++)
                    {
                        for (int i_1 = 0; i_1 < Locate[1].Length - 1; i_1++)
                        {
                            for (int i_2 = i_1 + 1; i_2 < Locate[2].Length; i_2++)
                            {
                                al.Add(Locate[0][i_0].ToString() + Locate[1][i_1].ToString() + Locate[2][i_2].ToString());
                            }
                        }
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
            private string[] ToSingle_ZuheDX(string Number, ref string CanonicalNumber)//组合大小取单式,后面 ref 参数是将彩票规范化，如：0113 变成011
            {
                CanonicalNumber = "";

                char[] strs = Number.ToCharArray();

                Regex regexNumber = new Regex(@"^[0-1][0-1][0-1]{1}", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                Match mNumber = regexNumber.Match(Number);

                if (mNumber.Success)
                {
                    if (!((strs[0] == strs[1]) && (strs[0] == strs[2]) && (strs[1] == strs[2])))
                    {
                        if ((strs[0] == '1') || (strs[2] == '0'))
                        {
                            CanonicalNumber = "";
                            return null;
                        }
                    }

                    Number = Number.Substring(0, 3);
                    CanonicalNumber = Number;
                }
                else
                {
                    CanonicalNumber = "";
                    return null;
                }

                Number = Number.Replace("1", "(56789)").Replace("0", "(01234)");

                string[] Locate = new string[3];

                Regex regex = new Regex(@"^(?<L0>(\d)|([(][\d]+?[)]))(?<L1>(\d)|([(][\d]+?[)]))(?<L2>(\d)|([(][\d]+?[)]))", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                Match m = regex.Match(Number);
                for (int i = 0; i < 3; i++)
                {
                    Locate[i] = m.Groups["L" + i.ToString()].ToString().Trim();
                    Locate[i] = Locate[i].Substring(1, Locate[i].Length - 2);
                    Locate[i] = FilterRepeated(Locate[i]);
                }

                ArrayList al = new ArrayList();

                #region 循环取单式

                #region 000;111
                if ((strs[0] == strs[1]) && (strs[0] == strs[2]))
                {
                    //叠加
                    for (int i_0 = 0; i_0 < Locate[0].Length; i_0++)
                    {
                        al.Add(Locate[0][i_0].ToString() + Locate[0][i_0].ToString() + Locate[0][i_0].ToString());
                    }

                    //组6
                    for (int i_0 = 0; i_0 < Locate[0].Length - 2; i_0++)
                    {
                        for (int i_1 = i_0 + 1; i_1 < Locate[1].Length - 1; i_1++)
                        {
                            for (int i_2 = i_1 + 1; i_2 < Locate[2].Length; i_2++)
                            {
                                al.Add(Locate[0][i_0].ToString() + Locate[1][i_1].ToString() + Locate[2][i_2].ToString());
                            }
                        }
                    }

                    //组3
                    for (int i_0 = 0; i_0 < Locate[0].Length; i_0++)
                    {
                        for (int i_1 = 0; i_1 < Locate[1].Length; i_1++)
                        {
                            if (i_0 != i_1)
                            {
                                al.Add(Locate[0][i_0].ToString() + Locate[0][i_0].ToString() + Locate[1][i_1].ToString());
                            }
                        }
                    }
                }
                #endregion

                #region 001;110
                if ((strs[0] == strs[1]) && (strs[0] != strs[2]))
                {
                    //组6
                    for (int i_0 = 0; i_0 < Locate[0].Length - 1; i_0++)
                    {
                        for (int i_1 = i_0 + 1; i_1 < Locate[1].Length; i_1++)
                        {
                            for (int i_2 = 0; i_2 < Locate[2].Length; i_2++)
                            {
                                al.Add(Locate[0][i_0].ToString() + Locate[1][i_1].ToString() + Locate[2][i_2].ToString());
                            }
                        }
                    }

                    //组3
                    for (int i_0 = 0; i_0 < Locate[0].Length; i_0++)
                    {
                        for (int i_2 = 0; i_2 < Locate[2].Length; i_2++)
                        {
                            al.Add(Locate[0][i_0].ToString() + Locate[0][i_0].ToString() + Locate[2][i_2].ToString());
                        }
                    }
                }
                #endregion

                #region 011;100
                if ((strs[1] == strs[2]) && (strs[0] != strs[1]))
                {
                    //组6
                    for (int i_0 = 0; i_0 < Locate[0].Length; i_0++)
                    {
                        for (int i_1 = 0; i_1 < Locate[1].Length - 1; i_1++)
                        {
                            for (int i_2 = i_1 + 1; i_2 < Locate[2].Length; i_2++)
                            {
                                al.Add(Locate[0][i_0].ToString() + Locate[1][i_1].ToString() + Locate[2][i_2].ToString());
                            }
                        }
                    }

                    //组3
                    for (int i_1 = 0; i_1 < Locate[1].Length; i_1++)
                    {
                        for (int i_0 = 0; i_0 < Locate[0].Length; i_0++)
                        {
                            al.Add(Locate[1][i_1].ToString() + Locate[1][i_1].ToString() + Locate[0][i_0].ToString());
                        }
                    }
                }
                #endregion

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
                if ((WinMoneyList == null) || (WinMoneyList.Length < 6))    // 奖金参数排列顺序 zhi, zu3 zu6
                    return -3;

                if ((PlayType == PlayType_ZhiD) || (PlayType == PlayType_ZhiBW))
                    return ComputeWin_ZhiD_BW(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[0], WinMoneyList[1]);

                if ((PlayType == PlayType_Zu3D) || (PlayType == PlayType_Zu6D) || (PlayType == PlayType_Zu6F))
                    return ComputeWin_Zu3D_Zu6(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[2], WinMoneyList[3], WinMoneyList[4], WinMoneyList[5]);

                if (PlayType == PlayType_ZhiH)
                    return ComputeWin_ZhiH(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[0], WinMoneyList[1]);

                if (PlayType == PlayType_Zu3H)
                    return ComputeWin_Zu3H(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[2], WinMoneyList[3]);

                if (PlayType == PlayType_Zu6H)
                    return ComputeWin_Zu6H(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[4], WinMoneyList[5]);

                if (PlayType == PlayType_ZuheH)
                    return ComputeWin_ZuheH(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[0], WinMoneyList[1], WinMoneyList[2], WinMoneyList[3], WinMoneyList[4], WinMoneyList[5]);

                if (PlayType == PlayType_ZhiF)
                    return ComputeWin_ZhiF(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[0], WinMoneyList[1]);

                if (PlayType == PlayType_Zu3F)
                    return ComputeWin_Zu3F(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[2], WinMoneyList[3]);

                if (PlayType == PlayType_ZuheF)
                    return ComputeWin_ZuheF(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[0], WinMoneyList[1], WinMoneyList[2], WinMoneyList[3], WinMoneyList[4], WinMoneyList[5]);

                if (PlayType == PlayType_ZhiBC)
                    return ComputeWin_ZhiBC(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[0], WinMoneyList[1]);

                if (PlayType == PlayType_ZhiECH)
                    return ComputeWin_ZhiECH(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[0], WinMoneyList[1]);

                if (PlayType == PlayType_ZhiDT)
                    return ComputeWin_ZhiDT(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[0], WinMoneyList[1]);

                if (PlayType == PlayType_ZuheDT)
                    return ComputeWin_ZuheDT(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[0], WinMoneyList[1], WinMoneyList[2], WinMoneyList[3], WinMoneyList[4], WinMoneyList[5]);

                if (PlayType == PlayType_ZhiKD)
                    return ComputeWin_ZhiKD(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[0], WinMoneyList[1]);

                if (PlayType == PlayType_Zu3KD)
                    return ComputeWin_Zu3KD(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[2], WinMoneyList[3]);

                if (PlayType == PlayType_Zu6KD)
                    return ComputeWin_Zu6KD(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[4], WinMoneyList[5]);

                if (PlayType == PlayType_ZuheKD)
                    return ComputeWin_ZuheKD(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[2], WinMoneyList[3], WinMoneyList[4], WinMoneyList[5]);

                if (PlayType == PlayType_ZuheHSW)
                    return ComputeWin_ZuheHSW(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[2], WinMoneyList[3], WinMoneyList[4], WinMoneyList[5]);

                if (PlayType == PlayType_ZhiJO)
                {
                    Number = Number.Replace("偶", "0").Replace("奇", "1");
                    return ComputeWin_ZhiJO(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[0], WinMoneyList[1]);
                }

                if (PlayType == PlayType_Zu3JO)
                {
                    Number = Number.Replace("偶", "0").Replace("奇", "1");
                    return ComputeWin_Zu3JO(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[2], WinMoneyList[3]);
                }

                if (PlayType == PlayType_Zu6JO)
                {
                    Number = Number.Replace("偶", "0").Replace("奇", "1");
                    return ComputeWin_Zu6JO(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[4], WinMoneyList[5]);
                }

                if (PlayType == PlayType_ZuheJO)
                {
                    Number = Number.Replace("偶", "0").Replace("奇", "1");
                    return ComputeWin_ZuheJO(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[2], WinMoneyList[3], WinMoneyList[4], WinMoneyList[5]);
                }

                if (PlayType == PlayType_ZhiDX)
                {
                    Number = Number.Replace("小", "0").Replace("大", "1");
                    return ComputeWin_ZhiDX(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[0], WinMoneyList[1]);
                }

                if (PlayType == PlayType_Zu3DX)
                {
                    Number = Number.Replace("小", "0").Replace("大", "1");
                    return ComputeWin_Zu3DX(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[2], WinMoneyList[3]);
                }

                if (PlayType == PlayType_Zu6DX)
                {
                    Number = Number.Replace("小", "0").Replace("大", "1");
                    return ComputeWin_Zu6DX(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[4], WinMoneyList[5]);
                }

                if (PlayType == PlayType_ZuheDX)
                {
                    Number = Number.Replace("小", "0").Replace("大", "1");
                    return ComputeWin_ZuheDX(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[2], WinMoneyList[3], WinMoneyList[4], WinMoneyList[5]);
                }

                return -4;
            }
            #region ComputeWin 的具体方法
            private double ComputeWin_ZhiD_BW(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney1, double WinMoneyNoWithTax1)	//计算中奖
            {
                WinNumber = WinNumber.Trim();
                if (WinNumber.Length < 3)	//3: "123"
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
                    string[] Lottery = ToSingle_ZhiD_BW(Lotterys[ii], ref t_str);
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
                    Description = "直选奖" + Description1.ToString() + "注。";

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
            private double ComputeWin_ZhiH(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney1, double WinMoneyNoWithTax1)	//计算中奖
            {
                WinNumber = WinNumber.Trim();
                if (WinNumber.Length < 3)	//3: "223"
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
                    Description = "直选和值奖" + Description1.ToString() + "注。";

                return WinMoney;
            }
            private double ComputeWin_Zu3H(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney1, double WinMoneyNoWithTax1)	//计算中奖
            {
                WinNumber = WinNumber.Trim();
                if (WinNumber.Length < 3)	//3: "123"
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
                    string[] Lottery = ToSingle_Zu3H(Lotterys[ii], ref t_str);
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
                    Description = "组选3和值奖" + Description1.ToString() + "注。";

                return WinMoney;
            }
            private double ComputeWin_Zu6H(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney1, double WinMoneyNoWithTax1)	//计算中奖
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
                    string[] Lottery = ToSingle_Zu6H(Lotterys[ii], ref t_str);
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
                    Description = "组选6和值奖" + Description1.ToString() + "注。";

                return WinMoney;
            }
            private double ComputeWin_ZuheH(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney1, double WinMoneyNoWithTax1, double WinMoney2, double WinMoneyNoWithTax2, double WinMoney3, double WinMoneyNoWithTax3)	//计算中奖
            {
                WinNumber = WinNumber.Trim();
                if (WinNumber.Length < 3)	//3: "123"
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

                string FixWinNumber = FilterRepeated(WinNumber);

                for (int ii = 0; ii < Lotterys.Length; ii++)
                {
                    string t_str = "";
                    string[] Lottery = ToSingle_ZuheH(Lotterys[ii], ref t_str);
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
                            if (FixWinNumber.Length == 1)
                            {
                                Description1++;
                                WinMoney += WinMoney1;
                                WinMoneyNoWithTax += WinMoneyNoWithTax1;
                            }
                            else if (FixWinNumber.Length == 2)
                            {
                                Description1++;
                                WinMoney += WinMoney2;
                                WinMoneyNoWithTax += WinMoneyNoWithTax2;
                            }
                            else if (FixWinNumber.Length == 3)
                            {
                                Description1++;
                                WinMoney += WinMoney3;
                                WinMoneyNoWithTax += WinMoneyNoWithTax3;
                            }
                        }
                    }
                }

                if (Description1 > 0)
                    Description = "组合和值奖" + Description1.ToString() + "注。";

                return WinMoney;
            }
            private double ComputeWin_ZhiF(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney1, double WinMoneyNoWithTax1)	//计算中奖
            {
                WinNumber = WinNumber.Trim();
                if (WinNumber.Length < 3)	//3: "223"
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
                    string[] Lottery = ToSingle_ZhiF(Lotterys[ii], ref t_str);
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
                    Description = "直选复式奖" + Description1.ToString() + "注。";

                return WinMoney;
            }
            private double ComputeWin_Zu3F(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney1, double WinMoneyNoWithTax1)	//计算中奖
            {
                WinNumber = WinNumber.Trim();
                if (WinNumber.Length < 3)	//3: "123"
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
                    Description = "组3复式奖" + Description1.ToString() + "注。";

                return WinMoney;
            }
            private double ComputeWin_ZuheF(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney1, double WinMoneyNoWithTax1, double WinMoney2, double WinMoneyNoWithTax2, double WinMoney3, double WinMoneyNoWithTax3)	//计算中奖
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

                string FixWinNumber = FilterRepeated(WinNumber);

                for (int ii = 0; ii < Lotterys.Length; ii++)
                {
                    string t_str = "";
                    string[] Lottery = ToSingle_ZuheF(Lotterys[ii], ref t_str);
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
                            if (FixWinNumber.Length == 1)
                            {
                                Description1++;
                                WinMoney += WinMoney1;
                                WinMoneyNoWithTax += WinMoneyNoWithTax1;
                            }
                            else if (FixWinNumber.Length == 2)
                            {
                                Description1++;
                                WinMoney += WinMoney2;
                                WinMoneyNoWithTax += WinMoneyNoWithTax2;
                            }
                            else if (FixWinNumber.Length == 3)
                            {
                                Description1++;
                                WinMoney += WinMoney3;
                                WinMoneyNoWithTax += WinMoneyNoWithTax3;
                            }
                        }
                    }
                }

                if (Description1 > 0)
                    Description = "组合复式奖" + Description1.ToString() + "注。";

                return WinMoney;
            }
            private double ComputeWin_ZhiBC(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney1, double WinMoneyNoWithTax1)	//计算中奖
            {
                WinNumber = WinNumber.Trim();
                if (WinNumber.Length < 3)	//3: "123"
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
                    string[] Lottery = ToSingle_ZhiBC(Lotterys[ii], ref t_str);
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
                    Description = "直选包串奖" + Description1.ToString() + "注。";

                return WinMoney;
            }
            private double ComputeWin_ZhiECH(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney1, double WinMoneyNoWithTax1)	//计算中奖
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
                    string[] Lottery = ToSingle_ZhiECH(Lotterys[ii], ref t_str);
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
                    Description = "直选二重号奖" + Description1.ToString() + "注。";

                return WinMoney;
            }
            private double ComputeWin_ZhiDT(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney1, double WinMoneyNoWithTax1)	//计算中奖
            {
                WinNumber = WinNumber.Trim();
                if (WinNumber.Length < 3)	//3: "223"
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
                    string[] Lottery = ToSingle_ZhiDT(Lotterys[ii], ref t_str);
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
                    Description = "直选胆拖奖" + Description1.ToString() + "注。";

                return WinMoney;
            }
            private double ComputeWin_ZuheDT(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney1, double WinMoneyNoWithTax1, double WinMoney2, double WinMoneyNoWithTax2, double WinMoney3, double WinMoneyNoWithTax3)	//计算中奖
            {
                WinNumber = WinNumber.Trim();
                if (WinNumber.Length < 3)	//3: "123"
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

                string FixWinNumber = FilterRepeated(WinNumber);

                for (int ii = 0; ii < Lotterys.Length; ii++)
                {
                    string t_str = "";
                    string[] Lottery = ToSingle_ZuheDT(Lotterys[ii], ref t_str);
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
                            if (FixWinNumber.Length == 1)
                            {
                                Description1++;
                                WinMoney += WinMoney1;
                                WinMoneyNoWithTax += WinMoneyNoWithTax1;
                            }
                            else if (FixWinNumber.Length == 2)
                            {
                                Description1++;
                                WinMoney += WinMoney2;
                                WinMoneyNoWithTax += WinMoneyNoWithTax2;
                            }
                            else if (FixWinNumber.Length == 3)
                            {
                                Description1++;
                                WinMoney += WinMoney3;
                                WinMoneyNoWithTax += WinMoneyNoWithTax3;
                            }
                        }
                    }
                }

                if (Description1 > 0)
                    Description = "组合胆拖奖" + Description1.ToString() + "注。";

                return WinMoney;
            }
            private double ComputeWin_ZhiKD(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney1, double WinMoneyNoWithTax1)	//计算中奖
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
                    string[] Lottery = ToSingle_ZhiKD(Lotterys[ii], ref t_str);
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
                    Description = "直选跨度奖" + Description1.ToString() + "注。";

                return WinMoney;
            }
            private double ComputeWin_Zu3KD(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney1, double WinMoneyNoWithTax1)	//计算中奖
            {
                WinNumber = WinNumber.Trim();
                if (WinNumber.Length < 3)	//3: "123"
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
                    string[] Lottery = ToSingle_Zu3KD(Lotterys[ii], ref t_str);
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
                    Description = "组3跨度奖" + Description1.ToString() + "注。";

                return WinMoney;
            }
            private double ComputeWin_Zu6KD(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney1, double WinMoneyNoWithTax1)	//计算中奖
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
                    string[] Lottery = ToSingle_Zu6KD(Lotterys[ii], ref t_str);
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
                    Description = "组6跨度奖" + Description1.ToString() + "注。";

                return WinMoney;
            }
            private double ComputeWin_ZuheKD(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney1, double WinMoneyNoWithTax1, double WinMoney2, double WinMoneyNoWithTax2)	//计算中奖
            {
                WinNumber = WinNumber.Trim();
                if (WinNumber.Length < 3)	//3: "223"
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

                string FixWinNumber = FilterRepeated(WinNumber);

                for (int ii = 0; ii < Lotterys.Length; ii++)
                {
                    string t_str = "";
                    string[] Lottery = ToSingle_ZuheKD(Lotterys[ii], ref t_str);
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
                            if (FixWinNumber.Length == 2)
                            {
                                Description1++;
                                WinMoney += WinMoney1;
                                WinMoneyNoWithTax += WinMoneyNoWithTax1;
                            }
                            else if (FixWinNumber.Length == 3)
                            {
                                Description1++;
                                WinMoney += WinMoney2;
                                WinMoneyNoWithTax += WinMoneyNoWithTax2;
                            }
                        }
                    }
                }

                if (Description1 > 0)
                    Description = "组合跨度奖" + Description1.ToString() + "注。";

                return WinMoney;
            }
            private double ComputeWin_ZuheHSW(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney1, double WinMoneyNoWithTax1, double WinMoney2, double WinMoneyNoWithTax2)	//计算中奖
            {
                WinNumber = WinNumber.Trim();
                if (WinNumber.Length < 3)	//3: "123"
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

                string FixWinNumber = FilterRepeated(WinNumber);

                for (int ii = 0; ii < Lotterys.Length; ii++)
                {
                    string t_str = "";
                    string[] Lottery = ToSingle_ZuheHSW(Lotterys[ii], ref t_str);
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
                            if ((FixWinNumber.Length == 1) || (FixWinNumber.Length == 2))
                            {
                                Description1++;
                                WinMoney += WinMoney1;
                                WinMoneyNoWithTax += WinMoneyNoWithTax1;
                            }
                            else if (FixWinNumber.Length == 3)
                            {
                                Description1++;
                                WinMoney += WinMoney2;
                                WinMoneyNoWithTax += WinMoneyNoWithTax2;
                            }
                        }
                    }
                }

                if (Description1 > 0)
                    Description = "和数尾组合奖" + Description1.ToString() + "注。";

                return WinMoney;
            }
            private double ComputeWin_ZhiJO(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney1, double WinMoneyNoWithTax1)	//计算中奖
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
                    string[] Lottery = ToSingle_ZhiJO(Lotterys[ii], ref t_str);
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
                    Description = "直选奇偶奖" + Description1.ToString() + "注。";

                return WinMoney;
            }
            private double ComputeWin_Zu3JO(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney1, double WinMoneyNoWithTax1)	//计算中奖
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
                    string[] Lottery = ToSingle_Zu3JO(Lotterys[ii], ref t_str);
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
                    Description = "组3奇偶奖" + Description1.ToString() + "注。";

                return WinMoney;
            }
            private double ComputeWin_Zu6JO(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney1, double WinMoneyNoWithTax1)	//计算中奖
            {
                WinNumber = WinNumber.Trim();
                if (WinNumber.Length < 3)	//3: "223"
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
                    string[] Lottery = ToSingle_Zu6JO(Lotterys[ii], ref t_str);
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
                    Description = "组6奇偶奖" + Description1.ToString() + "注。";

                return WinMoney;
            }
            private double ComputeWin_ZuheJO(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney1, double WinMoneyNoWithTax1, double WinMoney2, double WinMoneyNoWithTax2)	//计算中奖
            {
                WinNumber = WinNumber.Trim();
                if (WinNumber.Length < 3)	//3: "123"
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

                string FixWinNumber = FilterRepeated(WinNumber);

                for (int ii = 0; ii < Lotterys.Length; ii++)
                {
                    string t_str = "";
                    string[] Lottery = ToSingle_ZuheJO(Lotterys[ii], ref t_str);
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
                            if ((FixWinNumber.Length == 1) || (FixWinNumber.Length == 2))
                            {
                                Description1++;
                                WinMoney += WinMoney1;
                                WinMoneyNoWithTax += WinMoneyNoWithTax1;
                            }
                            else if (FixWinNumber.Length == 3)
                            {
                                Description1++;
                                WinMoney += WinMoney2;
                                WinMoneyNoWithTax += WinMoneyNoWithTax2;
                            }
                        }
                    }
                }

                if (Description1 > 0)
                    Description = "组合奇偶奖" + Description1.ToString() + "注。";

                return WinMoney;
            }
            private double ComputeWin_ZhiDX(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney1, double WinMoneyNoWithTax1)	//计算中奖
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
                    string[] Lottery = ToSingle_ZhiDX(Lotterys[ii], ref t_str);
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
                    Description = "直选大小奖" + Description1.ToString() + "注。";

                return WinMoney;
            }
            private double ComputeWin_Zu3DX(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney1, double WinMoneyNoWithTax1)	//计算中奖
            {
                WinNumber = WinNumber.Trim();
                if (WinNumber.Length < 3)	//3: "123"
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
                    string[] Lottery = ToSingle_Zu3DX(Lotterys[ii], ref t_str);
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
                    Description = "组3大小奖" + Description1.ToString() + "注。";

                return WinMoney;
            }
            private double ComputeWin_Zu6DX(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney1, double WinMoneyNoWithTax1)	//计算中奖
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
                    string[] Lottery = ToSingle_Zu6DX(Lotterys[ii], ref t_str);
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
                    Description = "组6大小奖" + Description1.ToString() + "注。";

                return WinMoney;
            }
            private double ComputeWin_ZuheDX(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney1, double WinMoneyNoWithTax1, double WinMoney2, double WinMoneyNoWithTax2)	//计算中奖
            {
                WinNumber = WinNumber.Trim();
                if (WinNumber.Length < 3)	//3: "223"
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

                string FixWinNumber = FilterRepeated(WinNumber);

                for (int ii = 0; ii < Lotterys.Length; ii++)
                {
                    string t_str = "";
                    string[] Lottery = ToSingle_ZuheDX(Lotterys[ii], ref t_str);
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
                            if ((FixWinNumber.Length == 1) || (FixWinNumber.Length == 2))
                            {
                                Description1++;
                                WinMoney += WinMoney1;
                                WinMoneyNoWithTax += WinMoneyNoWithTax1;
                            }
                            else if (FixWinNumber.Length == 3)
                            {
                                Description1++;
                                WinMoney += WinMoney2;
                                WinMoneyNoWithTax += WinMoneyNoWithTax2;
                            }
                        }
                    }
                }

                if (Description1 > 0)
                    Description = "组合大小奖" + Description1.ToString() + "注。";

                return WinMoney;
            }

            #endregion

            public override string AnalyseScheme(string Content, int PlayType)
            {
                if ((PlayType == PlayType_ZhiD) || (PlayType == PlayType_ZhiBW))
                    return AnalyseScheme_ZhiD_BW(Content, PlayType);

                if ((PlayType == PlayType_Zu3D) || (PlayType == PlayType_Zu6D) || (PlayType == PlayType_Zu6F))
                    return AnalyseScheme_Zu3D_Zu6(Content, PlayType);

                if (PlayType == PlayType_ZhiH)
                    return AnalyseScheme_ZhiH(Content, PlayType);

                if (PlayType == PlayType_Zu3H)
                    return AnalyseScheme_Zu3H(Content, PlayType);

                if (PlayType == PlayType_Zu6H)
                    return AnalyseScheme_Zu6H(Content, PlayType);

                if (PlayType == PlayType_ZuheH)
                    return AnalyseScheme_ZuheH(Content, PlayType);

                if (PlayType == PlayType_ZhiF)
                    return AnalyseScheme_ZhiF(Content, PlayType);

                if (PlayType == PlayType_Zu3F)
                    return AnalyseScheme_Zu3F(Content, PlayType);

                if (PlayType == PlayType_ZuheF)
                    return AnalyseScheme_ZuheF(Content, PlayType);

                if (PlayType == PlayType_ZhiBC)
                    return AnalyseScheme_ZhiBC(Content, PlayType);

                if (PlayType == PlayType_ZhiECH)
                    return AnalyseScheme_ZhiECH(Content, PlayType);

                if (PlayType == PlayType_ZhiDT)
                    return AnalyseScheme_ZhiDT(Content, PlayType);

                if (PlayType == PlayType_ZuheDT)
                    return AnalyseScheme_ZuheDT(Content, PlayType);

                if (PlayType == PlayType_ZhiKD)
                    return AnalyseScheme_ZhiKD(Content, PlayType);

                if (PlayType == PlayType_Zu3KD)
                    return AnalyseScheme_Zu3KD(Content, PlayType);

                if (PlayType == PlayType_Zu6KD)
                    return AnalyseScheme_Zu6KD(Content, PlayType);

                if (PlayType == PlayType_ZuheKD)
                    return AnalyseScheme_ZuheKD(Content, PlayType);

                if (PlayType == PlayType_ZuheHSW)
                    return AnalyseScheme_ZuheHSW(Content, PlayType);

                if (PlayType == PlayType_ZhiJO)
                {
                    Content = Content.Replace("偶", "0").Replace("奇", "1");
                    return AnalyseScheme_ZhiJO(Content, PlayType);
                }

                if (PlayType == PlayType_Zu3JO)
                {
                    Content = Content.Replace("偶", "0").Replace("奇", "1");
                    return AnalyseScheme_Zu3JO(Content, PlayType);
                }

                if (PlayType == PlayType_Zu6JO)
                {
                    Content = Content.Replace("偶", "0").Replace("奇", "1");
                    return AnalyseScheme_Zu6JO(Content, PlayType);
                }

                if (PlayType == PlayType_ZuheJO)
                {
                    Content = Content.Replace("偶", "0").Replace("奇", "1");
                    return AnalyseScheme_ZuheJO(Content, PlayType);
                }

                if (PlayType == PlayType_ZhiDX)
                {
                    Content = Content.Replace("小", "0").Replace("大", "1");
                    return AnalyseScheme_ZhiDX(Content, PlayType);
                }

                if (PlayType == PlayType_Zu3DX)
                {
                    Content = Content.Replace("小", "0").Replace("大", "1");
                    return AnalyseScheme_Zu3DX(Content, PlayType);
                }

                if (PlayType == PlayType_Zu6DX)
                {
                    Content = Content.Replace("小", "0").Replace("大", "1");
                    return AnalyseScheme_Zu6DX(Content, PlayType);
                }

                if (PlayType == PlayType_ZuheDX)
                {
                    Content = Content.Replace("小", "0").Replace("大", "1");
                    return AnalyseScheme_ZuheDX(Content, PlayType);
                }

                return "";
            }
            #region AnalyseScheme 的具体方法
            private string AnalyseScheme_ZhiD_BW(string Content, int PlayType)
            {
                string[] strs = Content.Split('\n');
                if (strs == null)
                    return "";
                if (strs.Length == 0)
                    return "";

                string Result = "";

                string RegexString;
                if (PlayType == PlayType_ZhiD)
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
                        string[] singles = ToSingle_ZhiD_BW(m.Value, ref CanonicalNumber);
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
            private string AnalyseScheme_Zu3D_Zu6(string Content, int PlayType)
            {
                string[] strs = Content.Split('\n');
                if (strs == null)
                    return "";
                if (strs.Length == 0)
                    return "";

                string Result = "";

                string RegexString;
                if ((PlayType == PlayType_Zu3D) || (PlayType == PlayType_Zu6D))
                    RegexString = @"^([\d]){3}";
                else
                    RegexString = @"^([\d]){4,10}";
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
                        if (singles.Length >= (((PlayType == PlayType_Zu3D) || (PlayType == PlayType_Zu6D)) ? 1 : 2))
                        {
                            if (FilterRepeated(Sort(m.Value)).Length == 2)
                            {
                                if (PlayType != PlayType_Zu6F)
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
            private string AnalyseScheme_ZhiH(string Content, int PlayType)
            {
                string[] strs = Content.Split('\n');
                if (strs == null)
                    return "";
                if (strs.Length == 0)
                    return "";

                string Result = "";
                Regex regex = new Regex(@"^([2][0-7]{1})|([1][\d]{1})|([\d]{1})", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                for (int i = 0; i < strs.Length; i++)
                {
                    Match m = regex.Match(strs[i]);
                    if (m.Success)
                    {
                        string CanonicalNumber = "";
                        string[] singles = ToSingle_ZhiH(m.Value, ref CanonicalNumber);
                        if (singles == null)
                            continue;
                        if (singles.Length > 0)
                            Result += CanonicalNumber + "|" + singles.Length.ToString() + "\n";
                    }
                }

                if (Result.EndsWith("\n"))
                    Result = Result.Substring(0, Result.Length - 1);
                return Result;
            }
            private string AnalyseScheme_Zu3H(string Content, int PlayType)
            {
                string[] strs = Content.Split('\n');
                if (strs == null)
                    return "";
                if (strs.Length == 0)
                    return "";

                string Result = "";
                Regex regex = new Regex(@"^([2][0-7]{1})|([1][\d]{1})|([\d]{1})", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                for (int i = 0; i < strs.Length; i++)
                {
                    Match m = regex.Match(strs[i]);
                    if (m.Success)
                    {
                        string CanonicalNumber = "";
                        string[] singles = ToSingle_Zu3H(m.Value, ref CanonicalNumber);
                        if (singles == null)
                            continue;
                        if (singles.Length > 0)
                            Result += CanonicalNumber + "|" + singles.Length.ToString() + "\n";
                    }
                }

                if (Result.EndsWith("\n"))
                    Result = Result.Substring(0, Result.Length - 1);
                return Result;
            }
            private string AnalyseScheme_Zu6H(string Content, int PlayType)
            {
                string[] strs = Content.Split('\n');
                if (strs == null)
                    return "";
                if (strs.Length == 0)
                    return "";

                string Result = "";
                Regex regex = new Regex(@"^([2][0-7]{1})|([1][\d]{1})|([\d]{1})", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                for (int i = 0; i < strs.Length; i++)
                {
                    Match m = regex.Match(strs[i]);
                    if (m.Success)
                    {
                        string CanonicalNumber = "";
                        string[] singles = ToSingle_Zu6H(m.Value, ref CanonicalNumber);
                        if (singles == null)
                            continue;
                        if (singles.Length > 0)
                            Result += CanonicalNumber + "|" + singles.Length.ToString() + "\n";
                    }
                }

                if (Result.EndsWith("\n"))
                    Result = Result.Substring(0, Result.Length - 1);
                return Result;
            }
            private string AnalyseScheme_ZuheH(string Content, int PlayType)
            {
                string[] strs = Content.Split('\n');
                if (strs == null)
                    return "";
                if (strs.Length == 0)
                    return "";

                string Result = "";
                Regex regex = new Regex(@"^([2][0-7]{1})|([1][\d]{1})|([\d]{1})", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                for (int i = 0; i < strs.Length; i++)
                {
                    Match m = regex.Match(strs[i]);
                    if (m.Success)
                    {
                        string CanonicalNumber = "";
                        string[] singles = ToSingle_ZuheH(m.Value, ref CanonicalNumber);
                        if (singles == null)
                            continue;
                        if (singles.Length > 0)
                            Result += CanonicalNumber + "|" + singles.Length.ToString() + "\n";
                    }
                }

                if (Result.EndsWith("\n"))
                    Result = Result.Substring(0, Result.Length - 1);
                return Result;
            }
            private string AnalyseScheme_ZhiF(string Content, int PlayType)
            {
                string[] strs = Content.Split('\n');
                if (strs == null)
                    return "";
                if (strs.Length == 0)
                    return "";

                string Result = "";
                Regex regex = new Regex(@"^([\d]){3,10}", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                for (int i = 0; i < strs.Length; i++)
                {
                    Match m = regex.Match(strs[i]);
                    if (m.Success)
                    {
                        string CanonicalNumber = "";
                        string[] singles = ToSingle_ZhiF(m.Value, ref CanonicalNumber);
                        if (singles == null)
                            continue;
                        if (singles.Length >= 0)
                            Result += CanonicalNumber + "|" + singles.Length.ToString() + "\n";
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
                Regex regex = new Regex(@"^([\d]){2,10}", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                for (int i = 0; i < strs.Length; i++)
                {
                    Match m = regex.Match(strs[i]);
                    if (m.Success)
                    {
                        string CanonicalNumber = "";
                        string[] singles = ToSingle_Zu3F(m.Value, ref CanonicalNumber);
                        if (singles == null)
                            continue;
                        if (singles.Length >= 0)
                            Result += CanonicalNumber + "|" + singles.Length.ToString() + "\n";
                    }
                }

                if (Result.EndsWith("\n"))
                    Result = Result.Substring(0, Result.Length - 1);
                return Result;
            }
            private string AnalyseScheme_ZuheF(string Content, int PlayType)
            {
                string[] strs = Content.Split('\n');
                if (strs == null)
                    return "";
                if (strs.Length == 0)
                    return "";

                string Result = "";
                Regex regex = new Regex(@"^([\d]){2,10}", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                for (int i = 0; i < strs.Length; i++)
                {
                    Match m = regex.Match(strs[i]);
                    if (m.Success)
                    {
                        string CanonicalNumber = "";
                        string[] singles = ToSingle_ZuheF(m.Value, ref CanonicalNumber);
                        if (singles == null)
                            continue;
                        if (singles.Length >= 0)
                            Result += CanonicalNumber + "|" + singles.Length.ToString() + "\n";
                    }
                }

                if (Result.EndsWith("\n"))
                    Result = Result.Substring(0, Result.Length - 1);
                return Result;
            }
            private string AnalyseScheme_ZhiBC(string Content, int PlayType)
            {
                string[] strs = Content.Split('\n');
                if (strs == null)
                    return "";
                if (strs.Length == 0)
                    return "";

                string Result = "";
                Regex regex = new Regex(@"^([\d]){2,10}", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                for (int i = 0; i < strs.Length; i++)
                {
                    Match m = regex.Match(strs[i]);
                    if (m.Success)
                    {
                        string CanonicalNumber = "";
                        string[] singles = ToSingle_ZhiBC(m.Value, ref CanonicalNumber);
                        if (singles == null)
                            continue;
                        if (singles.Length > 0)
                            Result += CanonicalNumber + "|" + singles.Length.ToString() + "\n";
                    }
                }

                if (Result.EndsWith("\n"))
                    Result = Result.Substring(0, Result.Length - 1);
                return Result;
            }
            private string AnalyseScheme_ZhiECH(string Content, int PlayType)
            {
                string[] strs = Content.Split('\n');
                if (strs == null)
                    return "";
                if (strs.Length == 0)
                    return "";

                string Result = "";
                Regex regex = new Regex(@"^([\d]){2,10}", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                for (int i = 0; i < strs.Length; i++)
                {
                    Match m = regex.Match(strs[i]);
                    if (m.Success)
                    {
                        string CanonicalNumber = "";
                        string[] singles = ToSingle_ZhiECH(m.Value, ref CanonicalNumber);
                        if (singles == null)
                            continue;
                        if (singles.Length > 0)
                            Result += CanonicalNumber + "|" + singles.Length.ToString() + "\n";
                    }
                }

                if (Result.EndsWith("\n"))
                    Result = Result.Substring(0, Result.Length - 1);
                return Result;
            }
            private string AnalyseScheme_ZhiDT(string Content, int PlayType)
            {
                string[] strs = Content.Split('\n');
                if (strs == null)
                    return "";
                if (strs.Length == 0)
                    return "";

                string Result = "";
                Regex regex = new Regex(@"^([\d]){1,2}[,]([\d]){2,}", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                for (int i = 0; i < strs.Length; i++)
                {
                    Match m = regex.Match(strs[i]);
                    if (m.Success)
                    {
                        string CanonicalNumber = "";
                        string[] singles = ToSingle_ZhiDT(m.Value, ref CanonicalNumber);
                        if (singles == null)
                            continue;
                        if (singles.Length > 0)
                            Result += CanonicalNumber + "|" + singles.Length.ToString() + "\n";
                    }
                }

                if (Result.EndsWith("\n"))
                    Result = Result.Substring(0, Result.Length - 1);
                return Result;
            }
            private string AnalyseScheme_ZuheDT(string Content, int PlayType)
            {
                string[] strs = Content.Split('\n');
                if (strs == null)
                    return "";
                if (strs.Length == 0)
                    return "";

                string Result = "";
                Regex regex = new Regex(@"^([\d]){1,2}[,]([\d]){2,}", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                for (int i = 0; i < strs.Length; i++)
                {
                    Match m = regex.Match(strs[i]);
                    if (m.Success)
                    {
                        string CanonicalNumber = "";
                        string[] singles = ToSingle_ZuheDT(m.Value, ref CanonicalNumber);
                        if (singles == null)
                            continue;
                        if (singles.Length > 0)
                            Result += CanonicalNumber + "|" + singles.Length.ToString() + "\n";
                    }
                }

                if (Result.EndsWith("\n"))
                    Result = Result.Substring(0, Result.Length - 1);
                return Result;
            }
            private string AnalyseScheme_ZhiKD(string Content, int PlayType)
            {
                string[] strs = Content.Split('\n');
                if (strs == null)
                    return "";
                if (strs.Length == 0)
                    return "";

                string Result = "";
                Regex regex = new Regex(@"^([\d]){1}", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                for (int i = 0; i < strs.Length; i++)
                {
                    Match m = regex.Match(strs[i]);
                    if (m.Success)
                    {
                        string CanonicalNumber = "";
                        string[] singles = ToSingle_ZhiKD(m.Value, ref CanonicalNumber);
                        if (singles == null)
                            continue;
                        if (singles.Length > 0)
                            Result += CanonicalNumber + "|" + singles.Length.ToString() + "\n";
                    }
                }

                if (Result.EndsWith("\n"))
                    Result = Result.Substring(0, Result.Length - 1);
                return Result;
            }
            private string AnalyseScheme_Zu3KD(string Content, int PlayType)
            {
                string[] strs = Content.Split('\n');
                if (strs == null)
                    return "";
                if (strs.Length == 0)
                    return "";

                string Result = "";
                Regex regex = new Regex(@"^([1-9]){1}", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                for (int i = 0; i < strs.Length; i++)
                {
                    Match m = regex.Match(strs[i]);
                    if (m.Success)
                    {
                        string CanonicalNumber = "";
                        string[] singles = ToSingle_Zu3KD(m.Value, ref CanonicalNumber);
                        if (singles == null)
                            continue;
                        if (singles.Length > 0)
                            Result += CanonicalNumber + "|" + singles.Length.ToString() + "\n";
                    }
                }

                if (Result.EndsWith("\n"))
                    Result = Result.Substring(0, Result.Length - 1);
                return Result;
            }
            private string AnalyseScheme_Zu6KD(string Content, int PlayType)
            {
                string[] strs = Content.Split('\n');
                if (strs == null)
                    return "";
                if (strs.Length == 0)
                    return "";

                string Result = "";
                Regex regex = new Regex(@"^([2-9]){1}", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                for (int i = 0; i < strs.Length; i++)
                {
                    Match m = regex.Match(strs[i]);
                    if (m.Success)
                    {
                        string CanonicalNumber = "";
                        string[] singles = ToSingle_Zu6KD(m.Value, ref CanonicalNumber);
                        if (singles == null)
                            continue;
                        if (singles.Length > 0)
                            Result += CanonicalNumber + "|" + singles.Length.ToString() + "\n";
                    }
                }

                if (Result.EndsWith("\n"))
                    Result = Result.Substring(0, Result.Length - 1);
                return Result;
            }
            private string AnalyseScheme_ZuheKD(string Content, int PlayType)
            {
                string[] strs = Content.Split('\n');
                if (strs == null)
                    return "";
                if (strs.Length == 0)
                    return "";

                string Result = "";
                Regex regex = new Regex(@"^([1-9]){1}", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                for (int i = 0; i < strs.Length; i++)
                {
                    Match m = regex.Match(strs[i]);
                    if (m.Success)
                    {
                        string CanonicalNumber = "";
                        string[] singles = ToSingle_ZuheKD(m.Value, ref CanonicalNumber);
                        if (singles == null)
                            continue;
                        if (singles.Length > 0)
                            Result += CanonicalNumber + "|" + singles.Length.ToString() + "\n";
                    }
                }

                if (Result.EndsWith("\n"))
                    Result = Result.Substring(0, Result.Length - 1);
                return Result;
            }
            private string AnalyseScheme_ZuheHSW(string Content, int PlayType)
            {
                string[] strs = Content.Split('\n');
                if (strs == null)
                    return "";
                if (strs.Length == 0)
                    return "";

                string Result = "";
                Regex regex = new Regex(@"^([\d]){1}", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                for (int i = 0; i < strs.Length; i++)
                {
                    Match m = regex.Match(strs[i]);
                    if (m.Success)
                    {
                        string CanonicalNumber = "";
                        string[] singles = ToSingle_ZuheHSW(m.Value, ref CanonicalNumber);
                        if (singles == null)
                            continue;
                        if (singles.Length > 0)
                            Result += CanonicalNumber + "|" + singles.Length.ToString() + "\n";
                    }
                }

                if (Result.EndsWith("\n"))
                    Result = Result.Substring(0, Result.Length - 1);
                return Result;
            }
            private string AnalyseScheme_ZhiJO(string Content, int PlayType)
            {
                string[] strs = Content.Split('\n');
                if (strs == null)
                    return "";
                if (strs.Length == 0)
                    return "";

                string Result = "";
                Regex regex = new Regex(@"^([0-1]){3}", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                for (int i = 0; i < strs.Length; i++)
                {
                    Match m = regex.Match(strs[i]);
                    if (m.Success)
                    {
                        string CanonicalNumber = "";
                        string[] singles = ToSingle_ZhiJO(m.Value, ref CanonicalNumber);
                        if (singles == null)
                            continue;
                        if (singles.Length > 0)
                            Result += CanonicalNumber + "|" + singles.Length.ToString() + "\n";
                    }
                }

                if (Result.EndsWith("\n"))
                    Result = Result.Substring(0, Result.Length - 1);
                return Result;
            }
            private string AnalyseScheme_Zu3JO(string Content, int PlayType)
            {
                string[] strs = Content.Split('\n');
                if (strs == null)
                    return "";
                if (strs.Length == 0)
                    return "";

                string Result = "";
                Regex regex = new Regex(@"^([0-1]){3}", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                for (int i = 0; i < strs.Length; i++)
                {
                    Match m = regex.Match(strs[i]);
                    if (m.Success)
                    {
                        string CanonicalNumber = "";
                        string[] singles = ToSingle_Zu3JO(m.Value, ref CanonicalNumber);
                        if (singles == null)
                            continue;
                        if (singles.Length > 0)
                            Result += CanonicalNumber + "|" + singles.Length.ToString() + "\n";
                    }
                }

                if (Result.EndsWith("\n"))
                    Result = Result.Substring(0, Result.Length - 1);
                return Result;
            }
            private string AnalyseScheme_Zu6JO(string Content, int PlayType)
            {
                string[] strs = Content.Split('\n');
                if (strs == null)
                    return "";
                if (strs.Length == 0)
                    return "";

                string Result = "";
                Regex regex = new Regex(@"^([0-1]){3}", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                for (int i = 0; i < strs.Length; i++)
                {
                    Match m = regex.Match(strs[i]);
                    if (m.Success)
                    {
                        string CanonicalNumber = "";
                        string[] singles = ToSingle_Zu6JO(m.Value, ref CanonicalNumber);
                        if (singles == null)
                            continue;
                        if (singles.Length > 0)
                            Result += CanonicalNumber + "|" + singles.Length.ToString() + "\n";
                    }
                }

                if (Result.EndsWith("\n"))
                    Result = Result.Substring(0, Result.Length - 1);
                return Result;
            }
            private string AnalyseScheme_ZuheJO(string Content, int PlayType)
            {
                string[] strs = Content.Split('\n');
                if (strs == null)
                    return "";
                if (strs.Length == 0)
                    return "";

                string Result = "";
                Regex regex = new Regex(@"^([0-1]){3}", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                for (int i = 0; i < strs.Length; i++)
                {
                    Match m = regex.Match(strs[i]);
                    if (m.Success)
                    {
                        string CanonicalNumber = "";
                        string[] singles = ToSingle_ZuheJO(m.Value, ref CanonicalNumber);
                        if (singles == null)
                            continue;
                        if (singles.Length > 0)
                            Result += CanonicalNumber + "|" + singles.Length.ToString() + "\n";
                    }
                }

                if (Result.EndsWith("\n"))
                    Result = Result.Substring(0, Result.Length - 1);
                return Result;
            }
            private string AnalyseScheme_ZhiDX(string Content, int PlayType)
            {
                string[] strs = Content.Split('\n');
                if (strs == null)
                    return "";
                if (strs.Length == 0)
                    return "";

                string Result = "";
                Regex regex = new Regex(@"^([0-1]{3})", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                for (int i = 0; i < strs.Length; i++)
                {
                    Match m = regex.Match(strs[i]);
                    if (m.Success)
                    {
                        string CanonicalNumber = "";
                        string[] singles = ToSingle_ZhiDX(m.Value, ref CanonicalNumber);
                        if (singles == null)
                            continue;
                        if (singles.Length > 0)
                            Result += CanonicalNumber + "|" + singles.Length.ToString() + "\n";
                    }
                }

                if (Result.EndsWith("\n"))
                    Result = Result.Substring(0, Result.Length - 1);
                return Result;
            }
            private string AnalyseScheme_Zu3DX(string Content, int PlayType)
            {
                string[] strs = Content.Split('\n');
                if (strs == null)
                    return "";
                if (strs.Length == 0)
                    return "";

                string Result = "";
                Regex regex = new Regex(@"^([0-1]{3})", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                for (int i = 0; i < strs.Length; i++)
                {
                    Match m = regex.Match(strs[i]);
                    if (m.Success)
                    {
                        string CanonicalNumber = "";
                        string[] singles = ToSingle_Zu3DX(m.Value, ref CanonicalNumber);
                        if (singles == null)
                            continue;
                        if (singles.Length > 0)
                            Result += CanonicalNumber + "|" + singles.Length.ToString() + "\n";
                    }
                }

                if (Result.EndsWith("\n"))
                    Result = Result.Substring(0, Result.Length - 1);
                return Result;
            }
            private string AnalyseScheme_Zu6DX(string Content, int PlayType)
            {
                string[] strs = Content.Split('\n');
                if (strs == null)
                    return "";
                if (strs.Length == 0)
                    return "";

                string Result = "";
                Regex regex = new Regex(@"^([0-1]{3})", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                for (int i = 0; i < strs.Length; i++)
                {
                    Match m = regex.Match(strs[i]);
                    if (m.Success)
                    {
                        string CanonicalNumber = "";
                        string[] singles = ToSingle_Zu6DX(m.Value, ref CanonicalNumber);
                        if (singles == null)
                            continue;
                        if (singles.Length > 0)
                            Result += CanonicalNumber + "|" + singles.Length.ToString() + "\n";
                    }
                }

                if (Result.EndsWith("\n"))
                    Result = Result.Substring(0, Result.Length - 1);
                return Result;
            }
            private string AnalyseScheme_ZuheDX(string Content, int PlayType)
            {
                string[] strs = Content.Split('\n');
                if (strs == null)
                    return "";
                if (strs.Length == 0)
                    return "";

                string Result = "";
                Regex regex = new Regex(@"^([0-1]{3})", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                for (int i = 0; i < strs.Length; i++)
                {
                    Match m = regex.Match(strs[i]);
                    if (m.Success)
                    {
                        string CanonicalNumber = "";
                        string[] singles = ToSingle_ZuheDX(m.Value, ref CanonicalNumber);
                        if (singles == null)
                            continue;
                        if (singles.Length > 0)
                            Result += CanonicalNumber + "|" + singles.Length.ToString() + "\n";
                    }
                }

                if (Result.EndsWith("\n"))
                    Result = Result.Substring(0, Result.Length - 1);
                return Result;
            }


            //private string AnalyseScheme_ZhiH(string Content, int PlayType)
            //{
            //    string[] strs = Content.Split('\n');
            //    if (strs == null)
            //        return "";
            //    if (strs.Length == 0)
            //        return "";

            //    string Result = "";
            //    Regex regex = new Regex(@"^([\d]){1,2}", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            //    for (int i = 0; i < strs.Length; i++)
            //    {
            //        Match m = regex.Match(strs[i]);
            //        if (m.Success)
            //        {
            //            string CanonicalNumber = "";
            //            string[] singles = ToSingle_ZhiH(m.Value, ref CanonicalNumber);
            //            if (singles == null)
            //                continue;
            //            if (singles.Length >= 1)
            //                Result += CanonicalNumber + "|" + singles.Length.ToString() + "\n";
            //        }
            //    }

            //    if (Result.EndsWith("\n"))
            //        Result = Result.Substring(0, Result.Length - 1);
            //    return Result;
            //}
            //private string AnalyseScheme_ZuH(string Content, int PlayType)
            //{
            //    string[] strs = Content.Split('\n');
            //    if (strs == null)
            //        return "";
            //    if (strs.Length == 0)
            //        return "";

            //    string Result = "";
            //    Regex regex = new Regex(@"^([\d]){1,2}", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            //    for (int i = 0; i < strs.Length; i++)
            //    {
            //        Match m = regex.Match(strs[i]);
            //        if (m.Success)
            //        {
            //            string CanonicalNumber = "";
            //            string[] singles = ToSingle_ZuH(m.Value, ref CanonicalNumber);
            //            if (singles == null)
            //                continue;
            //            if (singles.Length >= 1)
            //                Result += CanonicalNumber + "|" + singles.Length.ToString() + "\n";
            //        }
            //    }

            //    if (Result.EndsWith("\n"))
            //        Result = Result.Substring(0, Result.Length - 1);
            //    return Result;
            //}
            #endregion

            public override bool AnalyseWinNumber(string Number)
            {
                string t_str = "";
                string[] WinLotteryNumber = ToSingle_ZhiD_BW(Number, ref t_str);

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

            //参数说明 PlayTypeID:玩法类别; Number:投注号码; Multiple: 倍数; MaxMultiple:最大倍数;
            public override Ticket[] ToElectronicTicket_HPJX(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                if (PlayTypeID == PlayType_ZhiD)
                {
                    return ToElectronicTicket_HPJX_ZhiD(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                else if (PlayTypeID == PlayType_Zu3D)
                {
                    return ToElectronicTicket_HPJX_Zu3D(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                else if (PlayTypeID == PlayType_Zu6D)
                {
                    return ToElectronicTicket_HPJX_Zu6D(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                else if (PlayTypeID == PlayType_ZhiH)
                {
                    return ToElectronicTicket_HPJX_ZhiH(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                else if (PlayTypeID == PlayType_Zu3H)
                {
                    return ToElectronicTicket_HPJX_Zu3H(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                else if (PlayTypeID == PlayType_Zu6H)
                {
                    return ToElectronicTicket_HPJX_Zu6H(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                else if (PlayTypeID == PlayType_ZuheH)
                {
                    return ToElectronicTicket_HPJX_ZuheH(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                else if (PlayTypeID == PlayType_ZhiBW)
                {
                    return ToElectronicTicket_HPJX_ZhiBW(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                else if (PlayTypeID == PlayType_ZhiF)
                {
                    return ToElectronicTicket_HPJX_ZhiF(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                else if (PlayTypeID == PlayType_Zu3F)
                {
                    return ToElectronicTicket_HPJX_Zu3F(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                else if (PlayTypeID == PlayType_Zu6F)
                {
                    return ToElectronicTicket_HPJX_Zu6F(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                else if (PlayTypeID == PlayType_ZuheF)
                {
                    return ToElectronicTicket_HPJX_ZuheF(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                else if (PlayTypeID == PlayType_ZhiBC)
                {
                    return ToElectronicTicket_HPJX_ZhiBC(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                else if (PlayTypeID == PlayType_ZhiECH)
                {
                    return ToElectronicTicket_HPJX_ZhiECH(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                else if (PlayTypeID == PlayType_ZhiDT)
                {
                    return ToElectronicTicket_HPJX_ZhiDT(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                else if (PlayTypeID == PlayType_ZuheDT)
                {
                    return ToElectronicTicket_HPJX_ZuheDT(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                else if (PlayTypeID == PlayType_ZhiKD)
                {
                    return ToElectronicTicket_HPJX_ZhiKD(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                else if (PlayTypeID == PlayType_Zu3KD)
                {
                    return ToElectronicTicket_HPJX_Zu3KD(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                else if (PlayTypeID == PlayType_Zu6KD)
                {
                    return ToElectronicTicket_HPJX_Zu6KD(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                else if (PlayTypeID == PlayType_ZuheKD)
                {
                    return ToElectronicTicket_HPJX_ZuheKD(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                else if (PlayTypeID == PlayType_ZuheHSW)
                {
                    return ToElectronicTicket_HPJX_ZuheHSW(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                else if (PlayTypeID == PlayType_ZhiJO)
                {
                    return ToElectronicTicket_HPJX_ZhiJO(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                else if (PlayTypeID == PlayType_Zu3JO)
                {
                    return ToElectronicTicket_HPJX_Zu3JO(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                else if (PlayTypeID == PlayType_Zu6JO)
                {
                    return ToElectronicTicket_HPJX_Zu6JO(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                else if (PlayTypeID == PlayType_ZuheJO)
                {
                    return ToElectronicTicket_HPJX_ZuheJO(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                else if (PlayTypeID == PlayType_ZhiDX)
                {
                    return ToElectronicTicket_HPJX_ZhiDX(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                else if (PlayTypeID == PlayType_Zu3DX)
                {
                    return ToElectronicTicket_HPJX_Zu3DX(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                else if (PlayTypeID == PlayType_Zu6DX)
                {
                    return ToElectronicTicket_HPJX_Zu6DX(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }
                else if (PlayTypeID == PlayType_ZuheDX)
                {
                    return ToElectronicTicket_HPJX_ZuheDX(PlayTypeID, Number, Multiple, MaxMultiple, ref Money);
                }


                return null;
            }
            #region ToElectronicTicket_HPJX 的具体方法
            private Ticket[] ToElectronicTicket_HPJX_ZhiD(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
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

                        al.Add(new Ticket(201, ConvertFormatToElectronTicket_HPJX(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));
                    }
                }

                Ticket[] Tickets = new Ticket[al.Count];

                for (int i = 0; i < Tickets.Length; i++)
                {
                    Tickets[i] = (Ticket)al[i];
                }

                return Tickets;
            }
            private Ticket[] ToElectronicTicket_HPJX_Zu3D(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
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

                        al.Add(new Ticket(202, ConvertFormatToElectronTicket_HPJX(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));
                    }
                }

                Ticket[] Tickets = new Ticket[al.Count];

                for (int i = 0; i < Tickets.Length; i++)
                {
                    Tickets[i] = (Ticket)al[i];
                }

                return Tickets;
            }
            private Ticket[] ToElectronicTicket_HPJX_Zu6D(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
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

                        al.Add(new Ticket(203, ConvertFormatToElectronTicket_HPJX(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));
                    }
                }

                Ticket[] Tickets = new Ticket[al.Count];

                for (int i = 0; i < Tickets.Length; i++)
                {
                    Tickets[i] = (Ticket)al[i];
                }

                return Tickets;
            }
            private Ticket[] ToElectronicTicket_HPJX_ZhiH(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
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

                        al.Add(new Ticket(204, ConvertFormatToElectronTicket_HPJX(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));
                    }
                }

                Ticket[] Tickets = new Ticket[al.Count];

                for (int i = 0; i < Tickets.Length; i++)
                {
                    Tickets[i] = (Ticket)al[i];
                }

                return Tickets;
            }
            private Ticket[] ToElectronicTicket_HPJX_Zu3H(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
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

                        al.Add(new Ticket(205, ConvertFormatToElectronTicket_HPJX(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));
                    }
                }

                Ticket[] Tickets = new Ticket[al.Count];

                for (int i = 0; i < Tickets.Length; i++)
                {
                    Tickets[i] = (Ticket)al[i];
                }

                return Tickets;
            }
            private Ticket[] ToElectronicTicket_HPJX_Zu6H(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
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

                        al.Add(new Ticket(206, ConvertFormatToElectronTicket_HPJX(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));
                    }
                }

                Ticket[] Tickets = new Ticket[al.Count];

                for (int i = 0; i < Tickets.Length; i++)
                {
                    Tickets[i] = (Ticket)al[i];
                }

                return Tickets;
            }
            private Ticket[] ToElectronicTicket_HPJX_ZuheH(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
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

                        al.Add(new Ticket(207, ConvertFormatToElectronTicket_HPJX(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));
                    }
                }

                Ticket[] Tickets = new Ticket[al.Count];

                for (int i = 0; i < Tickets.Length; i++)
                {
                    Tickets[i] = (Ticket)al[i];
                }

                return Tickets;
            }
            private Ticket[] ToElectronicTicket_HPJX_ZhiBW(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
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

                        al.Add(new Ticket(208, ConvertFormatToElectronTicket_HPJX(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));
                    }
                }

                Ticket[] Tickets = new Ticket[al.Count];

                for (int i = 0; i < Tickets.Length; i++)
                {
                    Tickets[i] = (Ticket)al[i];
                }

                return Tickets;
            }
            private Ticket[] ToElectronicTicket_HPJX_ZhiF(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
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

                        al.Add(new Ticket(209, ConvertFormatToElectronTicket_HPJX(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));
                    }
                }

                Ticket[] Tickets = new Ticket[al.Count];

                for (int i = 0; i < Tickets.Length; i++)
                {
                    Tickets[i] = (Ticket)al[i];
                }

                return Tickets;
            }
            private Ticket[] ToElectronicTicket_HPJX_Zu3F(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
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

                        al.Add(new Ticket(210, ConvertFormatToElectronTicket_HPJX(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));
                    }
                }

                Ticket[] Tickets = new Ticket[al.Count];

                for (int i = 0; i < Tickets.Length; i++)
                {
                    Tickets[i] = (Ticket)al[i];
                }

                return Tickets;
            }
            private Ticket[] ToElectronicTicket_HPJX_Zu6F(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
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

                        al.Add(new Ticket(211, ConvertFormatToElectronTicket_HPJX(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));
                    }
                }

                Ticket[] Tickets = new Ticket[al.Count];

                for (int i = 0; i < Tickets.Length; i++)
                {
                    Tickets[i] = (Ticket)al[i];
                }

                return Tickets;
            }
            private Ticket[] ToElectronicTicket_HPJX_ZuheF(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
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

                        al.Add(new Ticket(212, ConvertFormatToElectronTicket_HPJX(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));
                    }
                }

                Ticket[] Tickets = new Ticket[al.Count];

                for (int i = 0; i < Tickets.Length; i++)
                {
                    Tickets[i] = (Ticket)al[i];
                }

                return Tickets;
            }
            private Ticket[] ToElectronicTicket_HPJX_ZhiBC(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
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

                        al.Add(new Ticket(213, ConvertFormatToElectronTicket_HPJX(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));
                    }
                }

                Ticket[] Tickets = new Ticket[al.Count];

                for (int i = 0; i < Tickets.Length; i++)
                {
                    Tickets[i] = (Ticket)al[i];
                }

                return Tickets;
            }
            private Ticket[] ToElectronicTicket_HPJX_ZhiECH(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
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

                        al.Add(new Ticket(214, ConvertFormatToElectronTicket_HPJX(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));
                    }
                }

                Ticket[] Tickets = new Ticket[al.Count];

                for (int i = 0; i < Tickets.Length; i++)
                {
                    Tickets[i] = (Ticket)al[i];
                }

                return Tickets;
            }
            private Ticket[] ToElectronicTicket_HPJX_ZhiDT(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
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

                        al.Add(new Ticket(215, ConvertFormatToElectronTicket_HPJX(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));
                    }
                }

                Ticket[] Tickets = new Ticket[al.Count];

                for (int i = 0; i < Tickets.Length; i++)
                {
                    Tickets[i] = (Ticket)al[i];
                }

                return Tickets;
            }
            private Ticket[] ToElectronicTicket_HPJX_ZuheDT(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
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

                        al.Add(new Ticket(216, ConvertFormatToElectronTicket_HPJX(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));
                    }
                }

                Ticket[] Tickets = new Ticket[al.Count];

                for (int i = 0; i < Tickets.Length; i++)
                {
                    Tickets[i] = (Ticket)al[i];
                }

                return Tickets;
            }
            private Ticket[] ToElectronicTicket_HPJX_ZhiKD(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
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

                        al.Add(new Ticket(217, ConvertFormatToElectronTicket_HPJX(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));
                    }
                }

                Ticket[] Tickets = new Ticket[al.Count];

                for (int i = 0; i < Tickets.Length; i++)
                {
                    Tickets[i] = (Ticket)al[i];
                }

                return Tickets;
            }
            private Ticket[] ToElectronicTicket_HPJX_Zu3KD(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
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

                        al.Add(new Ticket(218, ConvertFormatToElectronTicket_HPJX(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));
                    }
                }

                Ticket[] Tickets = new Ticket[al.Count];

                for (int i = 0; i < Tickets.Length; i++)
                {
                    Tickets[i] = (Ticket)al[i];
                }

                return Tickets;
            }
            private Ticket[] ToElectronicTicket_HPJX_Zu6KD(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
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

                        al.Add(new Ticket(219, ConvertFormatToElectronTicket_HPJX(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));
                    }
                }

                Ticket[] Tickets = new Ticket[al.Count];

                for (int i = 0; i < Tickets.Length; i++)
                {
                    Tickets[i] = (Ticket)al[i];
                }

                return Tickets;
            }
            private Ticket[] ToElectronicTicket_HPJX_ZuheKD(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
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

                        al.Add(new Ticket(220, ConvertFormatToElectronTicket_HPJX(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));
                    }
                }

                Ticket[] Tickets = new Ticket[al.Count];

                for (int i = 0; i < Tickets.Length; i++)
                {
                    Tickets[i] = (Ticket)al[i];
                }

                return Tickets;
            }
            private Ticket[] ToElectronicTicket_HPJX_ZuheHSW(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
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

                        al.Add(new Ticket(221, ConvertFormatToElectronTicket_HPJX(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));
                    }
                }

                Ticket[] Tickets = new Ticket[al.Count];

                for (int i = 0; i < Tickets.Length; i++)
                {
                    Tickets[i] = (Ticket)al[i];
                }

                return Tickets;
            }
            private Ticket[] ToElectronicTicket_HPJX_ZhiJO(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
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

                        al.Add(new Ticket(222, ConvertFormatToElectronTicket_HPJX(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));
                    }
                }

                Ticket[] Tickets = new Ticket[al.Count];

                for (int i = 0; i < Tickets.Length; i++)
                {
                    Tickets[i] = (Ticket)al[i];
                }

                return Tickets;
            }
            private Ticket[] ToElectronicTicket_HPJX_Zu3JO(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
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

                        al.Add(new Ticket(223, ConvertFormatToElectronTicket_HPJX(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));
                    }
                }

                Ticket[] Tickets = new Ticket[al.Count];

                for (int i = 0; i < Tickets.Length; i++)
                {
                    Tickets[i] = (Ticket)al[i];
                }

                return Tickets;
            }
            private Ticket[] ToElectronicTicket_HPJX_Zu6JO(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
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

                        al.Add(new Ticket(224, ConvertFormatToElectronTicket_HPJX(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));
                    }
                }

                Ticket[] Tickets = new Ticket[al.Count];

                for (int i = 0; i < Tickets.Length; i++)
                {
                    Tickets[i] = (Ticket)al[i];
                }

                return Tickets;
            }
            private Ticket[] ToElectronicTicket_HPJX_ZuheJO(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
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

                        al.Add(new Ticket(225, ConvertFormatToElectronTicket_HPJX(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));
                    }
                }

                Ticket[] Tickets = new Ticket[al.Count];

                for (int i = 0; i < Tickets.Length; i++)
                {
                    Tickets[i] = (Ticket)al[i];
                }

                return Tickets;
            }
            private Ticket[] ToElectronicTicket_HPJX_ZhiDX(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
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

                        al.Add(new Ticket(226, ConvertFormatToElectronTicket_HPJX(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));
                    }
                }

                Ticket[] Tickets = new Ticket[al.Count];

                for (int i = 0; i < Tickets.Length; i++)
                {
                    Tickets[i] = (Ticket)al[i];
                }

                return Tickets;
            }
            private Ticket[] ToElectronicTicket_HPJX_Zu3DX(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
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

                        al.Add(new Ticket(227, ConvertFormatToElectronTicket_HPJX(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));
                    }
                }

                Ticket[] Tickets = new Ticket[al.Count];

                for (int i = 0; i < Tickets.Length; i++)
                {
                    Tickets[i] = (Ticket)al[i];
                }

                return Tickets;
            }
            private Ticket[] ToElectronicTicket_HPJX_Zu6DX(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
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

                        al.Add(new Ticket(228, ConvertFormatToElectronTicket_HPJX(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));
                    }
                }

                Ticket[] Tickets = new Ticket[al.Count];

                for (int i = 0; i < Tickets.Length; i++)
                {
                    Tickets[i] = (Ticket)al[i];
                }

                return Tickets;
            }
            private Ticket[] ToElectronicTicket_HPJX_ZuheDX(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
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

                        al.Add(new Ticket(229, ConvertFormatToElectronTicket_HPJX(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));
                    }
                }

                Ticket[] Tickets = new Ticket[al.Count];

                for (int i = 0; i < Tickets.Length; i++)
                {
                    Tickets[i] = (Ticket)al[i];
                }

                return Tickets;
            }
            #endregion

            //投注格式转换成电子票格式
            private string ConvertFormatToElectronTicket_HPJX(int PlayTypeID, string Number)
            {
                Number = Number.Trim();

                string Ticket = "";

                if ((PlayTypeID == PlayType_ZhiH) || (PlayTypeID == PlayType_Zu3H) || (PlayTypeID == PlayType_Zu6H) || (PlayTypeID == PlayType_ZuheH) || (PlayTypeID == PlayType_ZhiKD) || (PlayTypeID == PlayType_Zu3KD) || (PlayTypeID == PlayType_Zu6KD) || (PlayTypeID == PlayType_ZuheKD))
                {
                    Ticket = Number;
                }
                else if ((PlayTypeID == PlayType_ZhiDT) || (PlayTypeID == PlayType_ZuheDT))
                {
                    string[] strs = Number.Split('\n');

                    for (int i = 0; i < strs.Length; i++)
                    {
                        for (int j = 0; j < strs[i].Length; j++)
                        {
                            Ticket += strs[i].Substring(j, 1) + ",";
                        }

                        Ticket = Ticket.Substring(0, Ticket.Length - 1).Replace(",,,", "#0#") + "\n";
                    }

                    if (Ticket.EndsWith("\n"))
                    {
                        Ticket = Ticket.Substring(0, Ticket.Length - 1);
                    }
                }
                else if (PlayTypeID == PlayType_ZhiBW)
                {
                    string[] Locate = new string[3];

                    Regex regex = new Regex(@"^(?<L0>(\d)|([(][\d]+?[)]))(?<L1>(\d)|([(][\d]+?[)]))(?<L2>(\d)|([(][\d]+?[)]))", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                    Match m = regex.Match(Number);
                    for (int i = 0; i < 3; i++)
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
                }
                else
                {
                    string[] strs = Number.Split('\n');

                    for (int i = 0; i < strs.Length; i++)
                    {
                        for (int j = 0; j < strs[i].Length; j++)
                        {
                            Ticket += strs[i].Substring(j, 1) + ",";
                        }

                        Ticket = Ticket.Substring(0, Ticket.Length - 1) + "\n";
                    }

                    if (Ticket.EndsWith("\n"))
                    {
                        Ticket = Ticket.Substring(0, Ticket.Length - 1);
                    }
                }

                return Ticket;
            }

        }
    }
}
