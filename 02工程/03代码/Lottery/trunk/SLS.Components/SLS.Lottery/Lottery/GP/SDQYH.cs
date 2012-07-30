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
        /// 山东群英会
        /// </summary>
        public partial class SDQYH : LotteryBase
        {
            #region 静态变量

            public const int PlayType_RX1 = 7101;
            public const int PlayType_RX2 = 7102;
            public const int PlayType_RX3 = 7103;
            public const int PlayType_RX4 = 7104;
            public const int PlayType_RX5 = 7105;
            public const int PlayType_RX6 = 7106;
            public const int PlayType_RX7 = 7107;
            public const int PlayType_RX8 = 7108;
            public const int PlayType_RX9 = 7109;
            public const int PlayType_RX10 = 7110;
            public const int PlayType_W1 = 7111;
            public const int PlayType_W2 = 7112;
            public const int PlayType_W3 = 7113;
            public const int PlayType_W4 = 7114;
            public const int PlayType_H1 = 7115;
            public const int PlayType_H2 = 7116;
            public const int PlayType_H3 = 7117;

            public const int ID = 71;
            public const string sID = "71";
            public const string Name = "山东群英会";
            public const string Code = "SDQYH";
            public const double MaxMoney = 200000;
            #endregion

            public SDQYH()
            {
                id = 71;
                name = "山东群英会";
                code = "SDQYH";
            }

            public override bool CheckPlayType(int play_type)
            {
                return ((play_type >= 7101) && (play_type <= 7117));
            }

            public override PlayType[] GetPlayTypeList()
            {
                PlayType[] Result = new PlayType[17];

                Result[0] = new PlayType(PlayType_RX1, "任选一");
                Result[1] = new PlayType(PlayType_RX2, "任选二");
                Result[2] = new PlayType(PlayType_RX3, "任选三");
                Result[3] = new PlayType(PlayType_RX4, "任选四");
                Result[4] = new PlayType(PlayType_RX5, "任选五");
                Result[5] = new PlayType(PlayType_RX6, "任选六");
                Result[6] = new PlayType(PlayType_RX7, "任选七");
                Result[7] = new PlayType(PlayType_RX8, "任选八");
                Result[8] = new PlayType(PlayType_RX9, "任选九");
                Result[9] = new PlayType(PlayType_RX10, "任选十");
                Result[10] = new PlayType(PlayType_W1, "围一");
                Result[11] = new PlayType(PlayType_W2, "围二");
                Result[12] = new PlayType(PlayType_W3, "围三");
                Result[13] = new PlayType(PlayType_W4, "围四");
                Result[14] = new PlayType(PlayType_H1, "顺一");
                Result[15] = new PlayType(PlayType_H2, "顺二");
                Result[16] = new PlayType(PlayType_H3, "顺三");

                return Result;
            }

            public override string BuildNumber(int Num, int Type)
            {
                System.Random rd = new Random();
                StringBuilder sb = new StringBuilder();

                for (int i = 0; i < Num; i++)
                {
                    string LotteryNumber = "";

                    for (int j = 0; j < Type; j++)
                        LotteryNumber += rd.Next(1, 11).ToString();

                    sb.Append(LotteryNumber.Trim() + "\n");
                }

                string Result = sb.ToString();
                Result = Result.Substring(0, Result.Length - 1);

                return Result;
            }

            public override string[] ToSingle(string Number, ref string CanonicalNumber, int PlayType)   //复式取单式, 后面 ref 参数是将彩票规范化，
            {
                if (PlayType == PlayType_RX1)
                    return ToSingle_RX1(Number, ref CanonicalNumber);

                if (PlayType == PlayType_RX2)
                    return ToSingle_RX2(Number, ref CanonicalNumber);

                if (PlayType == PlayType_RX3)
                    return ToSingle_RX3(Number, ref CanonicalNumber);

                if (PlayType == PlayType_RX4)
                    return ToSingle_RX4(Number, ref CanonicalNumber);

                if (PlayType == PlayType_RX5)
                    return ToSingle_RX5(Number, ref CanonicalNumber);

                if (PlayType == PlayType_RX6)
                    return ToSingle_RX6(Number, ref CanonicalNumber);

                if (PlayType == PlayType_RX7)
                    return ToSingle_RX7(Number, ref CanonicalNumber);

                if (PlayType == PlayType_RX8)
                    return ToSingle_RX8(Number, ref CanonicalNumber);

                if (PlayType == PlayType_RX9)
                    return ToSingle_RX9(Number, ref CanonicalNumber);

                if (PlayType == PlayType_RX10)
                    return ToSingle_RX10(Number, ref CanonicalNumber);

                if (PlayType == PlayType_W1)
                    return ToSingle_W1(Number, ref CanonicalNumber);

                if (PlayType == PlayType_W2)
                    return ToSingle_W2(Number, ref CanonicalNumber);

                if (PlayType == PlayType_W3)
                    return ToSingle_W3(Number, ref CanonicalNumber);

                if (PlayType == PlayType_W4)
                    return ToSingle_W4(Number, ref CanonicalNumber);

                if (PlayType == PlayType_H1)
                    return ToSingle_H1(Number, ref CanonicalNumber);

                if (PlayType == PlayType_H2)
                    return ToSingle_H2(Number, ref CanonicalNumber);

                if (PlayType == PlayType_H3)
                    return ToSingle_H3(Number, ref CanonicalNumber);

                return null;
            }
            #region ToSingle 的具体方法
            private string[] ToSingle_RX1(string Number, ref string CanonicalNumber)
            {
                string[] strs = FilterRepeated(Number.Trim().Split(' '), 23);
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
            private string[] ToSingle_RX2(string Number, ref string CanonicalNumber)
            {
                string[] strs = FilterRepeated(Number.Trim().Split(' '), 23);
                CanonicalNumber = "";

                if (strs.Length < 2)
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
                for (int i = 0; i < n - 1; i++)
                    for (int j = i + 1; j < n; j++)
                        al.Add(strs[i] + " " + strs[j]);

                #endregion

                string[] Result = new string[al.Count];
                for (int i = 0; i < al.Count; i++)
                    Result[i] = al[i].ToString();
                return Result;
            }
            private string[] ToSingle_RX3(string Number, ref string CanonicalNumber)
            {
                string[] strs = FilterRepeated(Number.Trim().Split(' '), 23);
                CanonicalNumber = "";

                if (strs.Length < 3)
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
                for (int i = 0; i < n - 2; i++)
                    for (int j = i + 1; j < n - 1; j++)
                        for (int k = j + 1; k < n; k++)
                            al.Add(strs[i] + " " + strs[j] + " " + strs[k]);

                #endregion

                string[] Result = new string[al.Count];
                for (int i = 0; i < al.Count; i++)
                    Result[i] = al[i].ToString();
                return Result;
            }
            private string[] ToSingle_RX4(string Number, ref string CanonicalNumber)
            {
                string[] strs = FilterRepeated(Number.Trim().Split(' '), 23);
                CanonicalNumber = "";

                if (strs.Length < 4)
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
                for (int i = 0; i < n - 3; i++)
                    for (int j = i + 1; j < n - 2; j++)
                        for (int k = j + 1; k < n - 1; k++)
                            for (int x = k + 1; x < n; x++)
                                al.Add(strs[i] + " " + strs[j] + " " + strs[k] + " " + strs[x]);

                #endregion

                string[] Result = new string[al.Count];
                for (int i = 0; i < al.Count; i++)
                    Result[i] = al[i].ToString();
                return Result;
            }
            private string[] ToSingle_RX5(string Number, ref string CanonicalNumber)
            {
                string[] strs = FilterRepeated(Number.Trim().Split(' '), 23);
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
            private string[] ToSingle_RX6(string Number, ref string CanonicalNumber)
            {
                string[] strs = FilterRepeated(Number.Trim().Split(' '), 23);
                CanonicalNumber = "";

                if (strs.Length < 6)
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
                for (int i = 0; i < n - 5; i++)
                    for (int j = i + 1; j < n - 4; j++)
                        for (int k = j + 1; k < n - 3; k++)
                            for (int x = k + 1; x < n - 2; x++)
                                for (int y = x + 1; y < n - 1; y++)
                                    for (int z = y + 1; z < n; z++)
                                        al.Add(strs[i] + " " + strs[j] + " " + strs[k] + " " + strs[x] + " " + strs[y] + " " + strs[z]);

                #endregion

                string[] Result = new string[al.Count];
                for (int i = 0; i < al.Count; i++)
                    Result[i] = al[i].ToString();
                return Result;
            }
            private string[] ToSingle_RX7(string Number, ref string CanonicalNumber)
            {
                string[] strs = FilterRepeated(Number.Trim().Split(' '), 23);
                CanonicalNumber = "";

                if (strs.Length < 7)
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
                for (int i = 0; i < n - 6; i++)
                    for (int j = i + 1; j < n - 5; j++)
                        for (int k = j + 1; k < n - 4; k++)
                            for (int x = k + 1; x < n - 3; x++)
                                for (int y = x + 1; y < n - 2; y++)
                                    for (int z = y + 1; z < n - 1; z++)
                                        for (int a = z + 1; a < n; a++)
                                            al.Add(strs[i] + " " + strs[j] + " " + strs[k] + " " + strs[x] + " " + strs[y] + " " + strs[z] + " " + strs[a]);

                #endregion

                string[] Result = new string[al.Count];
                for (int i = 0; i < al.Count; i++)
                    Result[i] = al[i].ToString();
                return Result;
            }
            private string[] ToSingle_RX8(string Number, ref string CanonicalNumber)
            {
                string[] strs = FilterRepeated(Number.Trim().Split(' '), 23);
                CanonicalNumber = "";

                if (strs.Length < 8)
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
                for (int i = 0; i < n - 7; i++)
                    for (int j = i + 1; j < n - 6; j++)
                        for (int k = j + 1; k < n - 5; k++)
                            for (int x = k + 1; x < n - 4; x++)
                                for (int y = x + 1; y < n - 3; y++)
                                    for (int z = y + 1; z < n - 2; z++)
                                        for (int a = z + 1; a < n - 1; a++)
                                            for (int b = a + 1; b < n; b++)
                                                al.Add(strs[i] + " " + strs[j] + " " + strs[k] + " " + strs[x] + " " + strs[y] + " " + strs[z] + " " + strs[a] + " " + strs[b]);

                #endregion

                string[] Result = new string[al.Count];
                for (int i = 0; i < al.Count; i++)
                    Result[i] = al[i].ToString();
                return Result;
            }
            private string[] ToSingle_RX9(string Number, ref string CanonicalNumber)
            {
                string[] strs = FilterRepeated(Number.Trim().Split(' '), 23);
                CanonicalNumber = "";

                if (strs.Length < 9)
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
                for (int i = 0; i < n - 8; i++)
                    for (int j = i + 1; j < n - 7; j++)
                        for (int k = j + 1; k < n - 6; k++)
                            for (int x = k + 1; x < n - 5; x++)
                                for (int y = x + 1; y < n - 4; y++)
                                    for (int z = y + 1; z < n -3; z++)
                                        for (int a = z + 1; a < n -2; a++)
                                            for (int b = a + 1; b < n - 1; b++)
                                            for (int c = b + 1; c < n; c++)
                                                al.Add(strs[i] + " " + strs[j] + " " + strs[k] + " " + strs[x] + " " + strs[y] + " " + strs[z] + " " + strs[a] + " " + strs[b] + " " + strs[c]);

                #endregion

                string[] Result = new string[al.Count];
                for (int i = 0; i < al.Count; i++)
                    Result[i] = al[i].ToString();
                return Result;
            }
            private string[] ToSingle_RX10(string Number, ref string CanonicalNumber)
            {
                string[] strs = FilterRepeated(Number.Trim().Split(' '), 23);
                CanonicalNumber = "";

                if (strs.Length < 10)
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
                for (int i = 0; i < n - 9; i++)
                    for (int j = i + 1; j < n - 8; j++)
                        for (int k = j + 1; k < n - 7; k++)
                            for (int x = k + 1; x < n - 6; x++)
                                for (int y = x + 1; y < n - 5; y++)
                                    for (int z = y + 1; z < n - 4; z++)
                                        for (int a = z + 1; a < n - 3; a++)
                                            for (int b = a + 1; b < n - 2; b++)
                                                for (int c = b + 1; c < n - 1; c++)
                                                    for (int d = c + 1; d < n; d++)
                                                        al.Add(strs[i] + " " + strs[j] + " " + strs[k] + " " + strs[x] + " " + strs[y] + " " + strs[z] + " " + strs[a] + " " + strs[b] + " " + strs[c] + " " + strs[d]);

                #endregion

                string[] Result = new string[al.Count];
                for (int i = 0; i < al.Count; i++)
                    Result[i] = al[i].ToString();
                return Result;
            }
            private string[] ToSingle_W1(string Number, ref string CanonicalNumber)
            {
                string[] strs = FilterRepeated(Number.Trim().Split(' '), 23);
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
            private string[] ToSingle_W2(string Number, ref string CanonicalNumber)
            {
                string[] strs = FilterRepeated(Number.Trim().Split(' '), 23);
                CanonicalNumber = "";

                if (strs.Length < 2)
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
                for (int i = 0; i < n - 1; i++)
                    for (int j = i + 1; j < n; j++)
                        al.Add(strs[i] + " " + strs[j]);

                #endregion

                string[] Result = new string[al.Count];
                for (int i = 0; i < al.Count; i++)
                    Result[i] = al[i].ToString();
                return Result;
            }
            private string[] ToSingle_W3(string Number, ref string CanonicalNumber)
            {
                string[] strs = FilterRepeated(Number.Trim().Split(' '), 23);
                CanonicalNumber = "";

                if (strs.Length < 3)
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
                for (int i = 0; i < n - 2; i++)
                    for (int j = i + 1; j < n - 1; j++)
                        for (int k = j + 1; k < n; k++)
                            al.Add(strs[i] + " " + strs[j] + " " + strs[k]);

                #endregion

                string[] Result = new string[al.Count];
                for (int i = 0; i < al.Count; i++)
                    Result[i] = al[i].ToString();
                return Result;
            }
            private string[] ToSingle_W4(string Number, ref string CanonicalNumber)
            {
                string[] strs = FilterRepeated(Number.Trim().Split(' '), 23);
                CanonicalNumber = "";

                if (strs.Length < 4)
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
                for (int i = 0; i < n - 3; i++)
                    for (int j = i + 1; j < n - 2; j++)
                        for (int k = j + 1; k < n - 1; k++)
                            for (int l = k + 1; l < n; l++)
                                al.Add(strs[i] + " " + strs[j] + " " + strs[k] + " " + strs[l]);

                #endregion

                string[] Result = new string[al.Count];
                for (int i = 0; i < al.Count; i++)
                    Result[i] = al[i].ToString();
                return Result;
            }
            private string[] ToSingle_H1(string Number, ref string CanonicalNumber)
            {
                string[] strs = FilterRepeated(Number.Trim().Split(' '), 23);
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
            private string[] ToSingle_H2(string Number, ref string CanonicalNumber)
            {
                string[] Q1 = FilterRepeated(Number.Trim().Split('|')[0].Trim().Split(' '), 23);
                string[] Q2 = FilterRepeated(Number.Trim().Split('|')[1].Trim().Split(' '), 23);

                if ((Q1.Length < 1) && (Q2.Length < 1))
                {
                    CanonicalNumber = "";
                    return null;
                }

                for (int i = 0; i < Q1.Length; i++)
                    CanonicalNumber += (Q1[i] + " ");

                CanonicalNumber = CanonicalNumber.Trim();

                CanonicalNumber += "|";
                for (int i = 0; i < Q2.Length; i++)
                    CanonicalNumber += (Q2[i] + " ");

                CanonicalNumber = CanonicalNumber.Trim();

                ArrayList al = new ArrayList();

                #region 循环取单式

                int m = Q1.Length;
                int n = Q2.Length;

                for (int i = 0; i < m; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        if (Q1[i] != Q2[j])
                        {
                            al.Add(Q1[i] + " " + Q2[j]);
                        }
                    }
                }
                #endregion

                string[] Result = new string[al.Count];
                for (int i = 0; i < al.Count; i++)
                    Result[i] = al[i].ToString();
                return Result;
            }
            private string[] ToSingle_H3(string Number, ref string CanonicalNumber)
            {
                string[] Q1 = FilterRepeated(Number.Trim().Split('|')[0].Trim().Split(' '), 23);
                string[] Q2 = FilterRepeated(Number.Trim().Split('|')[1].Trim().Split(' '), 23);
                string[] Q3 = FilterRepeated(Number.Trim().Split('|')[2].Trim().Split(' '), 23);

                if ((Q1.Length < 1) && (Q2.Length < 1) && (Q3.Length < 1))
                {
                    CanonicalNumber = "";
                    return null;
                }

                for (int i = 0; i < Q1.Length; i++)
                    CanonicalNumber += (Q1[i] + " ");

                CanonicalNumber = CanonicalNumber.Trim();

                CanonicalNumber += "|";
                for (int i = 0; i < Q2.Length; i++)
                    CanonicalNumber += (Q2[i] + " ");

                CanonicalNumber = CanonicalNumber.Trim();

                CanonicalNumber += "|";
                for (int i = 0; i < Q3.Length; i++)
                    CanonicalNumber += (Q3[i] + " ");

                ArrayList al = new ArrayList();

                #region 循环取单式

                int m = Q1.Length;
                int n = Q2.Length;
                int x = Q3.Length;

                for (int i = 0; i < m; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        for (int z = 0; z < x; z++)
                        {
                            if (Q1[i] != Q2[j] && Q2[j] != Q3[z] && Q3[z] != Q1[i])
                            {
                                al.Add(Q1[i] + " " + Q2[j] + " " + Q3[z]);
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
                Description = "";
                WinMoneyNoWithTax = 0;

                if ((WinMoneyList == null) || (WinMoneyList.Length < 42)) //奖金参数排列顺序(1 2 3中3 3中2 4中4 4中3 5中5 5中4 5中3 6 7 8 9 10 任选奖，围1, 围2, 围3, 围4, 顺1, 顺2, 顺3
                    return -3;

                if (PlayType == PlayType_RX1)
                    return ComputeWin_RX1(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[0], WinMoneyList[1]);

                if (PlayType == PlayType_RX2)
                    return ComputeWin_RX2(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[2], WinMoneyList[3]);

                if (PlayType == PlayType_RX3)
                    return ComputeWin_RX3(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[4], WinMoneyList[5], WinMoneyList[6], WinMoneyList[7]);

                if (PlayType == PlayType_RX4)
                    return ComputeWin_RX4(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[8], WinMoneyList[9], WinMoneyList[10], WinMoneyList[11]);

                if (PlayType == PlayType_RX5)
                    return ComputeWin_RX5(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[12], WinMoneyList[13], WinMoneyList[14], WinMoneyList[15], WinMoneyList[16], WinMoneyList[17]);

                if (PlayType == PlayType_RX6)
                    return ComputeWin_RX6(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[18], WinMoneyList[19]);

                if (PlayType == PlayType_RX7)
                    return ComputeWin_RX7(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[20], WinMoneyList[21]);

                if (PlayType == PlayType_RX8)
                    return ComputeWin_RX8(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[22], WinMoneyList[23]);

                if (PlayType == PlayType_RX9)
                    return ComputeWin_RX9(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[24], WinMoneyList[25]);

                if (PlayType == PlayType_RX10)
                    return ComputeWin_RX10(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[26], WinMoneyList[27]);

                if (PlayType == PlayType_W1)
                    return ComputeWin_W1(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[28], WinMoneyList[29]);

                if (PlayType == PlayType_W2)
                    return ComputeWin_W2(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[30], WinMoneyList[31]);

                if (PlayType == PlayType_W3)
                    return ComputeWin_W3(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[32], WinMoneyList[33]);

                if (PlayType == PlayType_W4)
                    return ComputeWin_W4(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[34], WinMoneyList[35]);

                if (PlayType == PlayType_H1)
                    return ComputeWin_H1(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[36], WinMoneyList[37]);

                if (PlayType == PlayType_H2)
                    return ComputeWin_H2(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[38], WinMoneyList[39]);

                if (PlayType == PlayType_H3)
                    return ComputeWin_H3(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[40], WinMoneyList[41]);


                return -4;
            }
            #region ComputeWin  的具体方法
            private double ComputeWin_RX1(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney1, double WinMoneyNoWithTax1)
            {
                WinNumber = WinNumber.Trim();
                if (WinNumber.Length < 14)	//14: "01 02 03 04 05"
                    return -1;
                string[] Lotterys = SplitLotteryNumber(Number);
                if (Lotterys == null)
                    return -2;
                if (Lotterys.Length < 1)
                    return -2;

                int WinCountRX1 = 0;
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
                        string[] Red = new string[1];
                        Regex regex = new Regex(@"^(?<R0>\d\d)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                        Match m = regex.Match(Lottery[i]);
                        int j;
                        int Count = 0;
                        bool Full = true;
                        for (j = 0; j < 1; j++)
                        {
                            Red[j] = m.Groups["R" + j.ToString()].ToString().Trim();
                            if (Red[j] == "")
                            {
                                Full = false;
                                break;
                            }
                            if (WinNumber.IndexOf(Red[j]) >= 0)
                                Count++;
                        }
                        if (!Full)
                            continue;

                        if (Count == 1)
                        {
                            WinCountRX1++;
                            WinMoney += WinMoney1;
                            WinMoneyNoWithTax += WinMoneyNoWithTax1;
                            continue;
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
            private double ComputeWin_RX2(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney2, double WinMoneyNoWithTax2)
            {
                WinNumber = WinNumber.Trim();
                if (WinNumber.Length < 14)	//14: "01 02 03 04 05"
                    return -1;
                string[] Lotterys = SplitLotteryNumber(Number);
                if (Lotterys == null)
                    return -2;
                if (Lotterys.Length < 1)
                    return -2;

                int WinCountRX2 = 0;
                double WinMoney = 0;
                WinMoneyNoWithTax = 0;
                Description = "";

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
                        string[] Red = new string[2];
                        Regex regex = new Regex(@"^(?<R0>\d\d\s)(?<R1>\d\d)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                        Match m = regex.Match(Lottery[i]);
                        int j;
                        int Count = 0;
                        bool Full = true;
                        for (j = 0; j < 2; j++)
                        {
                            Red[j] = m.Groups["R" + j.ToString()].ToString().Trim();
                            if (Red[j] == "")
                            {
                                Full = false;
                                break;
                            }
                            if (WinNumber.IndexOf(Red[j]) >= 0)
                                Count++;
                        }
                        if (!Full)
                            continue;

                        if (Count == 2)
                        {
                            WinCountRX2++;
                            WinMoney += WinMoney2;
                            WinMoneyNoWithTax += WinMoneyNoWithTax2;
                            continue;
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
            private double ComputeWin_RX3(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney3, double WinMoneyNoWithTax3, double WinMoney4, double WinMoneyNoWithTax4)
            {
                WinNumber = WinNumber.Trim();
                if (WinNumber.Length < 14)	//14: "01 02 03 04 05"
                    return -1;
                string[] Lotterys = SplitLotteryNumber(Number);
                if (Lotterys == null)
                    return -2;
                if (Lotterys.Length < 1)
                    return -2;

                int WinCountRX3Z3 = 0, WinCountRX3Z2 = 0;
                double WinMoney = 0;
                WinMoneyNoWithTax = 0;
                Description = "";

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
                        string[] Red = new string[3];
                        Regex regex = new Regex(@"^(?<R0>\d\d\s)(?<R1>\d\d\s)(?<R2>\d\d)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                        Match m = regex.Match(Lottery[i]);
                        int j;
                        int Count = 0;
                        bool Full = true;
                        for (j = 0; j < 3; j++)
                        {
                            Red[j] = m.Groups["R" + j.ToString()].ToString().Trim();
                            if (Red[j] == "")
                            {
                                Full = false;
                                break;
                            }
                            if (WinNumber.IndexOf(Red[j]) >= 0)
                                Count++;
                        }
                        if (!Full)
                            continue;

                        if (Count == 3)
                        {
                            WinCountRX3Z3++;
                            WinMoney += WinMoney3;
                            WinMoneyNoWithTax += WinMoneyNoWithTax3;
                            continue;
                        }

                        if (Count == 2)
                        {
                            WinCountRX3Z2++;
                            WinMoney += WinMoney4;
                            WinMoneyNoWithTax += WinMoneyNoWithTax4;
                            continue;
                        }
                    }
                }

                if (WinCountRX3Z3 > 0)
                {
                    Description += "任选三中3奖" + WinCountRX3Z3.ToString() + "注";
                }

                if (WinCountRX3Z2 > 0)
                {
                    Description += "任选三中2奖" + WinCountRX3Z2.ToString() + "注";
                }

                if (Description != "")
                    Description += "。";

                return WinMoney;
            }
            private double ComputeWin_RX4(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney5, double WinMoneyNoWithTax5, double WinMoney6, double WinMoneyNoWithTax6)
            {
                WinNumber = WinNumber.Trim();
                if (WinNumber.Length < 14)	//14: "01 02 03 04 05"
                    return -1;
                string[] Lotterys = SplitLotteryNumber(Number);
                if (Lotterys == null)
                    return -2;
                if (Lotterys.Length < 1)
                    return -2;

                int WinCountRX4Z4 = 0, WinCountRX4Z3 = 0;
                double WinMoney = 0;
                WinMoneyNoWithTax = 0;
                Description = "";

                for (int ii = 0; ii < Lotterys.Length; ii++)
                {
                    string t_str = "";
                    string[] Lottery = ToSingle_RX4(Lotterys[ii], ref t_str);
                    if (Lottery == null)
                        continue;
                    if (Lottery.Length < 1)
                        continue;

                    for (int i = 0; i < Lottery.Length; i++)
                    {
                        string[] Red = new string[4];
                        Regex regex = new Regex(@"^(?<R0>\d\d\s)(?<R1>\d\d\s)(?<R2>\d\d\s)(?<R3>\d\d)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                        Match m = regex.Match(Lottery[i]);
                        int j;
                        int Count = 0;
                        bool Full = true;
                        for (j = 0; j < 4; j++)
                        {
                            Red[j] = m.Groups["R" + j.ToString()].ToString().Trim();
                            if (Red[j] == "")
                            {
                                Full = false;
                                break;
                            }
                            if (WinNumber.IndexOf(Red[j]) >= 0)
                                Count++;
                        }
                        if (!Full)
                            continue;

                        if (Count == 4)
                        {
                            WinCountRX4Z4++;
                            WinMoney += WinMoney5;
                            WinMoneyNoWithTax += WinMoneyNoWithTax5;
                            continue;
                        }

                        if (Count == 3)
                        {
                            WinCountRX4Z3++;
                            WinMoney += WinMoney6;
                            WinMoneyNoWithTax += WinMoneyNoWithTax6;
                            continue;
                        }
                    }
                }

                if (WinCountRX4Z4 > 0)
                {
                    Description += "任选四中4奖" + WinCountRX4Z4.ToString() + "注";
                }

                if (WinCountRX4Z3 > 0)
                {
                    Description += "任选四中3奖" + WinCountRX4Z3.ToString() + "注";
                }

                if (Description != "")
                    Description += "。";

                return WinMoney;
            }
            private double ComputeWin_RX5(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney7, double WinMoneyNoWithTax7, double WinMoney8, double WinMoneyNoWithTax8, double WinMoney9, double WinMoneyNoWithTax9)
            {
                WinNumber = WinNumber.Trim();
                if (WinNumber.Length < 14)	//14: "01 02 03 04 05"
                    return -1;
                string[] Lotterys = SplitLotteryNumber(Number);
                if (Lotterys == null)
                    return -2;
                if (Lotterys.Length < 1)
                    return -2;

                int WinCountRX5Z5 = 0, WinCountRX5Z4 = 0, WinCountRX5Z3 = 0;
                double WinMoney = 0;
                WinMoneyNoWithTax = 0;
                Description = "";

                for (int ii = 0; ii < Lotterys.Length; ii++)
                {
                    string t_str = "";
                    string[] Lottery = ToSingle_RX5(Lotterys[ii], ref t_str);
                    if (Lottery == null)
                        continue;
                    if (Lottery.Length < 1)
                        continue;

                    for (int i = 0; i < Lottery.Length; i++)
                    {
                        string[] Red = new string[5];
                        Regex regex = new Regex(@"^(?<R0>\d\d\s)(?<R1>\d\d\s)(?<R2>\d\d\s)(?<R3>\d\d\s)(?<R4>\d\d)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                        Match m = regex.Match(Lottery[i]);
                        int j;
                        int Count = 0;
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
                                Count++;
                        }
                        if (!Full)
                            continue;

                        if (Count == 5)
                        {
                            WinCountRX5Z5++;
                            WinMoney += WinMoney7;
                            WinMoneyNoWithTax += WinMoneyNoWithTax7;
                            continue;
                        }

                        if (Count == 4)
                        {
                            WinCountRX5Z4++;
                            WinMoney += WinMoney8;
                            WinMoneyNoWithTax += WinMoneyNoWithTax8;
                            continue;
                        }

                        if (Count == 3)
                        {
                            WinCountRX5Z3++;
                            WinMoney += WinMoney9;
                            WinMoneyNoWithTax += WinMoneyNoWithTax9;
                            continue;
                        }
                    }
                }

                if (WinCountRX5Z5 > 0)
                {
                    Description += "任选五中5奖" + WinCountRX5Z5.ToString() + "注";
                }

                if (WinCountRX5Z4 > 0)
                {
                    Description += "任选五中4奖" + WinCountRX5Z4.ToString() + "注";
                }

                if (WinCountRX5Z3 > 0)
                {
                    Description += "任选五中3奖" + WinCountRX5Z3.ToString() + "注";
                }

                if (Description != "")
                    Description += "。";

                return WinMoney;
            }
            private double ComputeWin_RX6(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney10, double WinMoneyNoWithTax10)
            {
                WinNumber = WinNumber.Trim();
                if (WinNumber.Length < 14)	//14: "01 02 03 04 05"
                    return -1;
                string[] Lotterys = SplitLotteryNumber(Number);
                if (Lotterys == null)
                    return -2;
                if (Lotterys.Length < 1)
                    return -2;

                int WinCountRX6 = 0;
                double WinMoney = 0;
                WinMoneyNoWithTax = 0;
                Description = "";

                for (int ii = 0; ii < Lotterys.Length; ii++)
                {
                    string t_str = "";
                    string[] Lottery = ToSingle_RX6(Lotterys[ii], ref t_str);
                    if (Lottery == null)
                        continue;
                    if (Lottery.Length < 1)
                        continue;

                    for (int i = 0; i < Lottery.Length; i++)
                    {
                        string[] Red = new string[6];
                        Regex regex = new Regex(@"^(?<R0>\d\d\s)(?<R1>\d\d\s)(?<R2>\d\d\s)(?<R3>\d\d\s)(?<R4>\d\d\s)(?<R5>\d\d)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                        Match m = regex.Match(Lottery[i]);
                        int j;
                        int Count = 0;
                        bool Full = true;
                        for (j = 0; j < 6; j++)
                        {
                            Red[j] = m.Groups["R" + j.ToString()].ToString().Trim();
                            if (Red[j] == "")
                            {
                                Full = false;
                                break;
                            }
                            if (WinNumber.IndexOf(Red[j]) >= 0)
                                Count++;
                        }
                        if (!Full)
                            continue;

                        if (Count == 5)
                        {
                            WinCountRX6++;
                            WinMoney += WinMoney10;
                            WinMoneyNoWithTax += WinMoneyNoWithTax10;
                            continue;
                        }
                    }
                }

                if (WinCountRX6 > 0)
                {
                    Description = "任选六奖" + WinCountRX6.ToString() + "注";
                }

                if (Description != "")
                    Description += "。";

                return WinMoney;
            }
            private double ComputeWin_RX7(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney11, double WinMoneyNoWithTax11)
            {
                WinNumber = WinNumber.Trim();
                if (WinNumber.Length < 14)	//14: "01 02 03 04 05"
                    return -1;
                string[] Lotterys = SplitLotteryNumber(Number);
                if (Lotterys == null)
                    return -2;
                if (Lotterys.Length < 1)
                    return -2;

                int WinCountRX7 = 0;
                double WinMoney = 0;
                WinMoneyNoWithTax = 0;
                Description = "";

                for (int ii = 0; ii < Lotterys.Length; ii++)
                {
                    string t_str = "";
                    string[] Lottery = ToSingle_RX7(Lotterys[ii], ref t_str);
                    if (Lottery == null)
                        continue;
                    if (Lottery.Length < 1)
                        continue;

                    for (int i = 0; i < Lottery.Length; i++)
                    {
                        string[] Red = new string[7];
                        Regex regex = new Regex(@"^(?<R0>\d\d\s)(?<R1>\d\d\s)(?<R2>\d\d\s)(?<R3>\d\d\s)(?<R4>\d\d\s)(?<R5>\d\d\s)(?<R6>\d\d)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                        Match m = regex.Match(Lottery[i]);
                        int j;
                        int Count = 0;
                        bool Full = true;
                        for (j = 0; j < 7; j++)
                        {
                            Red[j] = m.Groups["R" + j.ToString()].ToString().Trim();
                            if (Red[j] == "")
                            {
                                Full = false;
                                break;
                            }
                            if (WinNumber.IndexOf(Red[j]) >= 0)
                                Count++;
                        }
                        if (!Full)
                            continue;

                        if (Count == 5)
                        {
                            WinCountRX7++;
                            WinMoney += WinMoney11;
                            WinMoneyNoWithTax += WinMoneyNoWithTax11;
                            continue;
                        }
                    }
                }

                if (WinCountRX7 > 0)
                {
                    Description = "任选七奖" + WinCountRX7.ToString() + "注";
                }

                if (Description != "")
                    Description += "。";

                return WinMoney;
            }
            private double ComputeWin_RX8(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney12, double WinMoneyNoWithTax12)
            {
                WinNumber = WinNumber.Trim();
                if (WinNumber.Length < 14)	//14: "01 02 03 04 05"
                    return -1;
                string[] Lotterys = SplitLotteryNumber(Number);
                if (Lotterys == null)
                    return -2;
                if (Lotterys.Length < 1)
                    return -2;

                int WinCountRX8 = 0;
                double WinMoney = 0;
                WinMoneyNoWithTax = 0;
                Description = "";

                for (int ii = 0; ii < Lotterys.Length; ii++)
                {
                    string t_str = "";
                    string[] Lottery = ToSingle_RX8(Lotterys[ii], ref t_str);
                    if (Lottery == null)
                        continue;
                    if (Lottery.Length < 1)
                        continue;

                    for (int i = 0; i < Lottery.Length; i++)
                    {
                        string[] Red = new string[8];
                        Regex regex = new Regex(@"^(?<R0>\d\d\s)(?<R1>\d\d\s)(?<R2>\d\d\s)(?<R3>\d\d\s)(?<R4>\d\d\s)(?<R5>\d\d\s)(?<R6>\d\d\s)(?<R7>\d\d)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                        Match m = regex.Match(Lottery[i]);
                        int j;
                        int Count = 0;
                        bool Full = true;
                        for (j = 0; j < 8; j++)
                        {
                            Red[j] = m.Groups["R" + j.ToString()].ToString().Trim();
                            if (Red[j] == "")
                            {
                                Full = false;
                                break;
                            }
                            if (WinNumber.IndexOf(Red[j]) >= 0)
                                Count++;
                        }
                        if (!Full)
                            continue;

                        if (Count == 5)
                        {
                            WinCountRX8++;
                            WinMoney += WinMoney12;
                            WinMoneyNoWithTax += WinMoneyNoWithTax12;
                            continue;
                        }
                    }
                }

                if (WinCountRX8 > 0)
                {
                    Description = "任选八奖" + WinCountRX8.ToString() + "注";
                }

                if (Description != "")
                    Description += "。";

                return WinMoney;
            }
            private double ComputeWin_RX9(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney13, double WinMoneyNoWithTax13)
            {
                WinNumber = WinNumber.Trim();
                if (WinNumber.Length < 14)	//14: "01 02 03 04 05"
                    return -1;
                string[] Lotterys = SplitLotteryNumber(Number);
                if (Lotterys == null)
                    return -2;
                if (Lotterys.Length < 1)
                    return -2;

                int WinCountRX9 = 0;
                double WinMoney = 0;
                WinMoneyNoWithTax = 0;
                Description = "";

                for (int ii = 0; ii < Lotterys.Length; ii++)
                {
                    string t_str = "";
                    string[] Lottery = ToSingle_RX9(Lotterys[ii], ref t_str);
                    if (Lottery == null)
                        continue;
                    if (Lottery.Length < 1)
                        continue;

                    for (int i = 0; i < Lottery.Length; i++)
                    {
                        string[] Red = new string[9];
                        Regex regex = new Regex(@"^(?<R0>\d\d\s)(?<R1>\d\d\s)(?<R2>\d\d\s)(?<R3>\d\d\s)(?<R4>\d\d\s)(?<R5>\d\d\s)(?<R6>\d\d\s)(?<R7>\d\d\s)(?<R8>\d\d)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                        Match m = regex.Match(Lottery[i]);
                        int j;
                        int Count = 0;
                        bool Full = true;
                        for (j = 0; j < 9; j++)
                        {
                            Red[j] = m.Groups["R" + j.ToString()].ToString().Trim();
                            if (Red[j] == "")
                            {
                                Full = false;
                                break;
                            }
                            if (WinNumber.IndexOf(Red[j]) >= 0)
                                Count++;
                        }
                        if (!Full)
                            continue;

                        if (Count == 5)
                        {
                            WinCountRX9++;
                            WinMoney += WinMoney13;
                            WinMoneyNoWithTax += WinMoneyNoWithTax13;
                            continue;
                        }
                    }
                }

                if (WinCountRX9 > 0)
                {
                    Description = "任选九奖" + WinCountRX9.ToString() + "注";
                }

                if (Description != "")
                    Description += "。";

                return WinMoney;
            }
            private double ComputeWin_RX10(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney14, double WinMoneyNoWithTax14)
            {
                WinNumber = WinNumber.Trim();
                if (WinNumber.Length < 14)	//14: "01 02 03 04 05"
                    return -1;
                string[] Lotterys = SplitLotteryNumber(Number);
                if (Lotterys == null)
                    return -2;
                if (Lotterys.Length < 1)
                    return -2;

                int WinCountRX10 = 0;
                double WinMoney = 0;
                WinMoneyNoWithTax = 0;
                Description = "";

                for (int ii = 0; ii < Lotterys.Length; ii++)
                {
                    string t_str = "";
                    string[] Lottery = ToSingle_RX10(Lotterys[ii], ref t_str);
                    if (Lottery == null)
                        continue;
                    if (Lottery.Length < 1)
                        continue;

                    for (int i = 0; i < Lottery.Length; i++)
                    {
                        string[] Red = new string[10];
                        Regex regex = new Regex(@"^(?<R0>\d\d\s)(?<R1>\d\d\s)(?<R2>\d\d\s)(?<R3>\d\d\s)(?<R4>\d\d\s)(?<R5>\d\d\s)(?<R6>\d\d\s)(?<R7>\d\d\s)(?<R8>\d\d\s)(?<R9>\d\d)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                        Match m = regex.Match(Lottery[i]);
                        int j;
                        int Count = 0;
                        bool Full = true;
                        for (j = 0; j < 10; j++)
                        {
                            Red[j] = m.Groups["R" + j.ToString()].ToString().Trim();
                            if (Red[j] == "")
                            {
                                Full = false;
                                break;
                            }
                            if (WinNumber.IndexOf(Red[j]) >= 0)
                                Count++;
                        }
                        if (!Full)
                            continue;

                        if (Count == 5)
                        {
                            WinCountRX10++;
                            WinMoney += WinMoney14;
                            WinMoneyNoWithTax += WinMoneyNoWithTax14;
                            continue;
                        }
                    }
                }

                if (WinCountRX10 > 0)
                {
                    Description = "任选十奖" + WinCountRX10.ToString() + "注";
                }

                if (Description != "")
                    Description += "。";

                return WinMoney;
            }

            private double ComputeWin_W1(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney15, double WinMoneyNoWithTax15)
            {
                WinNumber = WinNumber.Trim();
                if (WinNumber.Length < 14)	//14: "01 02 03 04 05"
                    return -1;
                string[] Lotterys = SplitLotteryNumber(Number);
                if (Lotterys == null)
                    return -2;
                if (Lotterys.Length < 1)
                    return -2;

                int WinCount_W1 = 0;
                double WinMoney = 0;
                WinMoneyNoWithTax = 0;
                Description = "";

                for (int ii = 0; ii < Lotterys.Length; ii++)
                {
                    string t_str = "";
                    string[] Lottery = ToSingle_W1(Lotterys[ii], ref t_str);
                    if (Lottery == null)
                        continue;
                    if (Lottery.Length < 1)
                        continue;

                    for (int i = 0; i < Lottery.Length; i++)
                    {
                        int RedRight = 0;

                        string[] Red = new string[1];
                        Regex regex = new Regex(@"^(?<R0>\d\d)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                        Match m = regex.Match(Lottery[i]);

                        for (int j = 0; j < 1; j++)
                        {
                            Red[j] = m.Groups["R" + j.ToString()].ToString().Trim();

                            if (Red[j] == "")
                            {
                                break;
                            }

                            if (WinNumber.Substring(0, 2).IndexOf(Red[j]) >= 0)
                                RedRight++;
                        }

                        if (RedRight == 1)
                        {
                            WinCount_W1++;
                            WinMoney += WinMoney15;
                            WinMoneyNoWithTax += WinMoneyNoWithTax15;
                            continue;
                        }
                    }
                }

                if (WinCount_W1 > 0)
                {
                    Description = "围一奖" + WinCount_W1.ToString() + "注";
                }

                if (Description != "")
                    Description += "。";

                return WinMoney;
            }
            private double ComputeWin_W2(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney16, double WinMoneyNoWithTax16)
            {
                WinNumber = WinNumber.Trim();
                if (WinNumber.Length < 14)	//14: "01 02 03 04 05"
                    return -1;
                string[] Lotterys = SplitLotteryNumber(Number);
                if (Lotterys == null)
                    return -2;
                if (Lotterys.Length < 1)
                    return -2;

                int WinCount_W2 = 0;
                double WinMoney = 0;
                WinMoneyNoWithTax = 0;
                Description = "";

                for (int ii = 0; ii < Lotterys.Length; ii++)
                {
                    string t_str = "";
                    string[] Lottery = ToSingle_W2(Lotterys[ii], ref t_str);
                    if (Lottery == null)
                        continue;
                    if (Lottery.Length < 1)
                        continue;

                    for (int i = 0; i < Lottery.Length; i++)
                    {
                        int RedRight = 0;

                        string[] Red = new string[2];
                        Regex regex = new Regex(@"^(?<R0>\d\d\s)(?<R1>\d\d)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                        Match m = regex.Match(Lottery[i]);

                        for (int j = 0; j < 2; j++)
                        {
                            Red[j] = m.Groups["R" + j.ToString()].ToString().Trim();

                            if (Red[j] == "")
                            {
                                break;
                            }

                            if (WinNumber.Substring(0, 5).IndexOf(Red[j]) >= 0)
                                RedRight++;
                        }

                        if (RedRight == 2)
                        {
                            WinCount_W2++;
                            WinMoney += WinMoney16;
                            WinMoneyNoWithTax += WinMoneyNoWithTax16;
                            continue;
                        }
                    }
                }

                if (WinCount_W2 > 0)
                {
                    Description = "围二奖" + WinCount_W2.ToString() + "注";
                }

                if (Description != "")
                    Description += "。";

                return WinMoney;
            }
            private double ComputeWin_W3(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney17, double WinMoneyNoWithTax17)
            {
                WinNumber = WinNumber.Trim();
                if (WinNumber.Length < 14)	//14: "01 02 03 04 05"
                    return -1;
                string[] Lotterys = SplitLotteryNumber(Number);
                if (Lotterys == null)
                    return -2;
                if (Lotterys.Length < 1)
                    return -2;

                int WinCount_W3 = 0;
                double WinMoney = 0;
                WinMoneyNoWithTax = 0;
                Description = "";

                for (int ii = 0; ii < Lotterys.Length; ii++)
                {
                    string t_str = "";
                    string[] Lottery = ToSingle_W3(Lotterys[ii], ref t_str);
                    if (Lottery == null)
                        continue;
                    if (Lottery.Length < 1)
                        continue;

                    for (int i = 0; i < Lottery.Length; i++)
                    {
                        int RedRight = 0;

                        string[] Red = new string[3];
                        Regex regex = new Regex(@"^(?<R0>\d\d\s)(?<R1>\d\d\s)(?<R2>\d\d)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                        Match m = regex.Match(Lottery[i]);

                        for (int j = 0; j < 3; j++)
                        {
                            Red[j] = m.Groups["R" + j.ToString()].ToString().Trim();

                            if (Red[j] == "")
                            {
                                break;
                            }

                            if (WinNumber.Substring(0, 8).IndexOf(Red[j]) >= 0)
                                RedRight++;
                        }

                        if (RedRight == 3)
                        {
                            WinCount_W3++;
                            WinMoney += WinMoney17;
                            WinMoneyNoWithTax += WinMoneyNoWithTax17;
                            continue;
                        }
                    }
                }

                if (WinCount_W3 > 0)
                {
                    Description = "围三奖" + WinCount_W3.ToString() + "注";
                }

                if (Description != "")
                    Description += "。";

                return WinMoney;
            }
            private double ComputeWin_W4(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney18, double WinMoneyNoWithTax18)
            {
                WinNumber = WinNumber.Trim();
                if (WinNumber.Length < 14)	//14: "01 02 03 04 05"
                    return -1;
                string[] Lotterys = SplitLotteryNumber(Number);
                if (Lotterys == null)
                    return -2;
                if (Lotterys.Length < 1)
                    return -2;

                int WinCount_W4 = 0;
                double WinMoney = 0;
                WinMoneyNoWithTax = 0;
                Description = "";

                for (int ii = 0; ii < Lotterys.Length; ii++)
                {
                    string t_str = "";
                    string[] Lottery = ToSingle_W4(Lotterys[ii], ref t_str);
                    if (Lottery == null)
                        continue;
                    if (Lottery.Length < 1)
                        continue;

                    for (int i = 0; i < Lottery.Length; i++)
                    {
                        int RedRight = 0;

                        string[] Red = new string[4];
                        Regex regex = new Regex(@"^(?<R0>\d\d\s)(?<R1>\d\d\s)(?<R2>\d\d\s)(?<R3>\d\d)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                        Match m = regex.Match(Lottery[i]);

                        for (int j = 0; j < 4; j++)
                        {
                            Red[j] = m.Groups["R" + j.ToString()].ToString().Trim();

                            if (Red[j] == "")
                            {
                                break;
                            }

                            if (WinNumber.Substring(0, 11).IndexOf(Red[j]) >= 0)
                                RedRight++;
                        }

                        if (RedRight == 4)
                        {
                            WinCount_W4++;
                            WinMoney += WinMoney18;
                            WinMoneyNoWithTax += WinMoneyNoWithTax18;
                            continue;
                        }
                    }
                }

                if (WinCount_W4 > 0)
                {
                    Description = "围四奖" + WinCount_W4.ToString() + "注";
                }

                if (Description != "")
                    Description += "。";

                return WinMoney;
            }

            private double ComputeWin_H1(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney19, double WinMoneyNoWithTax19)
            {
                WinNumber = WinNumber.Trim();
                if (WinNumber.Length < 14)	//14: "01 02 03 04 05"
                    return -1;
                string[] Lotterys = SplitLotteryNumber(Number);
                if (Lotterys == null)
                    return -2;
                if (Lotterys.Length < 1)
                    return -2;

                int WinCount_H1 = 0;
                double WinMoney = 0;
                WinMoneyNoWithTax = 0;
                Description = "";

                for (int ii = 0; ii < Lotterys.Length; ii++)
                {
                    string t_str = "";
                    string[] Lottery = ToSingle_H1(Lotterys[ii], ref t_str);
                    if (Lottery == null)
                        continue;
                    if (Lottery.Length < 1)
                        continue;

                    for (int i = 0; i < Lottery.Length; i++)
                    {
                        string[] Red = new string[1];
                        Regex regex = new Regex(@"^(?<R0>\d\d)", RegexOptions.IgnoreCase | RegexOptions.Compiled);

                        if (regex.IsMatch(Lottery[i]))
                        {
                            if (WinNumber.Substring(0, 2) == Lottery[i])
                            {
                                WinCount_H1++;
                                WinMoney += WinMoney19;
                                WinMoneyNoWithTax += WinMoneyNoWithTax19;
                                continue;
                            }
                        }
                    }
                }

                if (WinCount_H1 > 0)
                {
                    Description = "顺一奖" + WinCount_H1.ToString() + "注";
                }

                if (Description != "")
                    Description += "。";

                return WinMoney;
            }
            private double ComputeWin_H2(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney20, double WinMoneyNoWithTax20)
            {
                WinNumber = WinNumber.Trim();
                if (WinNumber.Length < 14)	//14: "01 02 03 04 05"
                    return -1;
                string[] Lotterys = SplitLotteryNumber(Number);
                if (Lotterys == null)
                    return -2;
                if (Lotterys.Length < 1)
                    return -2;

                int WinCount_H2 = 0;
                double WinMoney = 0;
                WinMoneyNoWithTax = 0;
                Description = "";

                for (int ii = 0; ii < Lotterys.Length; ii++)
                {
                    string t_str = "";
                    string[] Lottery = ToSingle_H2(Lotterys[ii], ref t_str);
                    if (Lottery == null)
                        continue;
                    if (Lottery.Length < 1)
                        continue;

                    for (int i = 0; i < Lottery.Length; i++)
                    {
                        bool Full = false;

                        string[] Red = new string[2];
                        Regex regex = new Regex(@"^(?<R0>\d\d\s)(?<R1>\d\d)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                        if (regex.IsMatch(Lottery[i]))
                        {
                            if (WinNumber.Substring(0, 5) == Lottery[i])
                                Full = true;
                        }

                        if (Full)
                        {
                            WinCount_H2++;
                            WinMoney += WinMoney20;
                            WinMoneyNoWithTax += WinMoneyNoWithTax20;
                            continue;
                        }
                    }
                }

                if (WinCount_H2 > 0)
                {
                    Description = "顺二奖" + WinCount_H2.ToString() + "注";
                }

                if (Description != "")
                    Description += "。";

                return WinMoney;
            }
            private double ComputeWin_H3(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney21, double WinMoneyNoWithTax21)
            {
                WinNumber = WinNumber.Trim();
                if (WinNumber.Length < 14)	//14: "01 02 03 04 05"
                    return -1;
                string[] Lotterys = SplitLotteryNumber(Number);
                if (Lotterys == null)
                    return -2;
                if (Lotterys.Length < 1)
                    return -2;

                int WinCount_H3 = 0;
                double WinMoney = 0;
                WinMoneyNoWithTax = 0;
                Description = "";

                for (int ii = 0; ii < Lotterys.Length; ii++)
                {
                    string t_str = "";
                    string[] Lottery = ToSingle_H3(Lotterys[ii], ref t_str);
                    if (Lottery == null)
                        continue;
                    if (Lottery.Length < 1)
                        continue;

                    for (int i = 0; i < Lottery.Length; i++)
                    {
                        bool Full = false;

                        string[] Red = new string[3];
                        Regex regex = new Regex(@"^(?<R0>\d\d\s)(?<R1>\d\d\s)(?<R2>\d\d)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                        if (regex.IsMatch(Lottery[i]))
                        {
                            if (WinNumber.Substring(0, 8) == Lottery[i])
                                Full = true;
                        }

                        if (Full)
                        {
                            WinCount_H3++;
                            WinMoney += WinMoney21;
                            WinMoneyNoWithTax += WinMoneyNoWithTax21;
                            continue;
                        }
                    }
                }

                if (WinCount_H3 > 0)
                {
                    Description = "顺三奖" + WinCount_H3.ToString() + "注";
                }

                if (Description != "")
                    Description += "。";

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

                if (PlayType == PlayType_RX4)
                    return AnalyseScheme_RX4(Content, PlayType);

                if (PlayType == PlayType_RX5)
                    return AnalyseScheme_RX5(Content, PlayType);

                if (PlayType == PlayType_RX6)
                    return AnalyseScheme_RX6(Content, PlayType);

                if (PlayType == PlayType_RX7)
                    return AnalyseScheme_RX7(Content, PlayType);

                if (PlayType == PlayType_RX8)
                    return AnalyseScheme_RX8(Content, PlayType);

                if (PlayType == PlayType_RX9)
                    return AnalyseScheme_RX9(Content, PlayType);

                if (PlayType == PlayType_RX10)
                    return AnalyseScheme_RX10(Content, PlayType);

                if (PlayType == PlayType_W1)
                    return AnalyseScheme_W1(Content, PlayType);

                if (PlayType == PlayType_W2)
                    return AnalyseScheme_W2(Content, PlayType);

                if (PlayType == PlayType_W3)
                    return AnalyseScheme_W3(Content, PlayType);

                if (PlayType == PlayType_W4)
                    return AnalyseScheme_W4(Content, PlayType);

                if (PlayType == PlayType_H1)
                    return AnalyseScheme_H1(Content, PlayType);

                if (PlayType == PlayType_H2)
                    return AnalyseScheme_H2(Content, PlayType);

                if (PlayType == PlayType_H3)
                    return AnalyseScheme_H3(Content, PlayType);

                return "";
            }
            #region AnalyseScheme 的具体方法
            private string AnalyseScheme_RX1(string Content, int PlayType)
            {
                string[] strs = Content.Split('\n');
                if (strs == null)
                    return "";
                if (strs.Length == 0)
                    return "";

                string Result = "";

                string RegexString = @"^(\d\d)";
                Regex regex = new Regex(RegexString, RegexOptions.Compiled | RegexOptions.IgnoreCase);

                for (int i = 0; i < strs.Length; i++)
                {
                    Match m = regex.Match(strs[i]);
                    if (m.Success)
                    {
                        string CanonicalNumber = "";
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

                string RegexString = @"^((\d\d\s){1,22}\d\d)";
                Regex regex = new Regex(RegexString, RegexOptions.Compiled | RegexOptions.IgnoreCase);

                for (int i = 0; i < strs.Length; i++)
                {
                    Match m = regex.Match(strs[i]);
                    if (m.Success)
                    {
                        string CanonicalNumber = "";
                        string[] singles = ToSingle_RX2(m.Value, ref CanonicalNumber);
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
            private string AnalyseScheme_RX3(string Content, int PlayType)
            {
                string[] strs = Content.Split('\n');
                if (strs == null)
                    return "";
                if (strs.Length == 0)
                    return "";

                string Result = "";

                string RegexString = @"^((\d\d\s){2,22}\d\d)";
                Regex regex = new Regex(RegexString, RegexOptions.Compiled | RegexOptions.IgnoreCase);

                for (int i = 0; i < strs.Length; i++)
                {
                    Match m = regex.Match(strs[i]);
                    if (m.Success)
                    {
                        string CanonicalNumber = "";
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
            private string AnalyseScheme_RX4(string Content, int PlayType)
            {
                string[] strs = Content.Split('\n');
                if (strs == null)
                    return "";
                if (strs.Length == 0)
                    return "";

                string Result = "";

                string RegexString = @"^((\d\d\s){3,22}\d\d)";
                Regex regex = new Regex(RegexString, RegexOptions.Compiled | RegexOptions.IgnoreCase);

                for (int i = 0; i < strs.Length; i++)
                {
                    Match m = regex.Match(strs[i]);
                    if (m.Success)
                    {
                        string CanonicalNumber = "";
                        string[] singles = ToSingle_RX4(m.Value, ref CanonicalNumber);
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
            private string AnalyseScheme_RX5(string Content, int PlayType)
            {
                string[] strs = Content.Split('\n');
                if (strs == null)
                    return "";
                if (strs.Length == 0)
                    return "";

                string Result = "";

                string RegexString = @"^((\d\d\s){4,22}\d\d)";
                Regex regex = new Regex(RegexString, RegexOptions.Compiled | RegexOptions.IgnoreCase);

                for (int i = 0; i < strs.Length; i++)
                {
                    Match m = regex.Match(strs[i]);
                    if (m.Success)
                    {
                        string CanonicalNumber = "";
                        string[] singles = ToSingle_RX5(m.Value, ref CanonicalNumber);
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
            private string AnalyseScheme_RX6(string Content, int PlayType)
            {
                string[] strs = Content.Split('\n');
                if (strs == null)
                    return "";
                if (strs.Length == 0)
                    return "";

                string Result = "";

                string RegexString = @"^((\d\d\s){5,22}\d\d)";
                Regex regex = new Regex(RegexString, RegexOptions.Compiled | RegexOptions.IgnoreCase);

                for (int i = 0; i < strs.Length; i++)
                {
                    Match m = regex.Match(strs[i]);
                    if (m.Success)
                    {
                        string CanonicalNumber = "";
                        string[] singles = ToSingle_RX6(m.Value, ref CanonicalNumber);
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
            private string AnalyseScheme_RX7(string Content, int PlayType)
            {
                string[] strs = Content.Split('\n');
                if (strs == null)
                    return "";
                if (strs.Length == 0)
                    return "";

                string Result = "";

                string RegexString = @"^((\d\d\s){6,22}\d\d)";
                Regex regex = new Regex(RegexString, RegexOptions.Compiled | RegexOptions.IgnoreCase);

                for (int i = 0; i < strs.Length; i++)
                {
                    Match m = regex.Match(strs[i]);
                    if (m.Success)
                    {
                        string CanonicalNumber = "";
                        string[] singles = ToSingle_RX7(m.Value, ref CanonicalNumber);
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
            private string AnalyseScheme_RX8(string Content, int PlayType)
            {
                string[] strs = Content.Split('\n');
                if (strs == null)
                    return "";
                if (strs.Length == 0)
                    return "";

                string Result = "";

                string RegexString = @"^((\d\d\s){7,22}\d\d)";
                Regex regex = new Regex(RegexString, RegexOptions.Compiled | RegexOptions.IgnoreCase);

                for (int i = 0; i < strs.Length; i++)
                {
                    Match m = regex.Match(strs[i]);
                    if (m.Success)
                    {
                        string CanonicalNumber = "";
                        string[] singles = ToSingle_RX8(m.Value, ref CanonicalNumber);
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
            private string AnalyseScheme_RX9(string Content, int PlayType)
            {
                string[] strs = Content.Split('\n');
                if (strs == null)
                    return "";
                if (strs.Length == 0)
                    return "";

                string Result = "";

                string RegexString = @"^((\d\d\s){8,22}\d\d)";
                Regex regex = new Regex(RegexString, RegexOptions.Compiled | RegexOptions.IgnoreCase);

                for (int i = 0; i < strs.Length; i++)
                {
                    Match m = regex.Match(strs[i]);
                    if (m.Success)
                    {
                        string CanonicalNumber = "";
                        string[] singles = ToSingle_RX9(m.Value, ref CanonicalNumber);
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
            private string AnalyseScheme_RX10(string Content, int PlayType)
            {
                string[] strs = Content.Split('\n');
                if (strs == null)
                    return "";
                if (strs.Length == 0)
                    return "";

                string Result = "";

                string RegexString = @"^((\d\d\s){9,22}\d\d)";
                Regex regex = new Regex(RegexString, RegexOptions.Compiled | RegexOptions.IgnoreCase);

                for (int i = 0; i < strs.Length; i++)
                {
                    Match m = regex.Match(strs[i]);
                    if (m.Success)
                    {
                        string CanonicalNumber = "";
                        string[] singles = ToSingle_RX10(m.Value, ref CanonicalNumber);
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
            private string AnalyseScheme_W1(string Content, int PlayType)
            {
                string[] strs = Content.Split('\n');
                if (strs == null)
                    return "";
                if (strs.Length == 0)
                    return "";

                string Result = "";

                string RegexString = @"^(\d\d)";
                Regex regex = new Regex(RegexString, RegexOptions.Compiled | RegexOptions.IgnoreCase);

                for (int i = 0; i < strs.Length; i++)
                {
                    Match m = regex.Match(strs[i]);
                    if (m.Success)
                    {
                        string CanonicalNumber = "";
                        string[] singles = ToSingle_W1(m.Value, ref CanonicalNumber);
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
            private string AnalyseScheme_W2(string Content, int PlayType)
            {
                string[] strs = Content.Split('\n');
                if (strs == null)
                    return "";
                if (strs.Length == 0)
                    return "";

                string Result = "";

                string RegexString = @"^(\d\d\s){1,22}\d\d";
                Regex regex = new Regex(RegexString, RegexOptions.Compiled | RegexOptions.IgnoreCase);

                for (int i = 0; i < strs.Length; i++)
                {
                    Match m = regex.Match(strs[i]);
                    if (m.Success)
                    {
                        string CanonicalNumber = "";
                        string[] singles = ToSingle_W2(m.Value, ref CanonicalNumber);
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
            private string AnalyseScheme_W3(string Content, int PlayType)
            {
                string[] strs = Content.Split('\n');
                if (strs == null)
                    return "";
                if (strs.Length == 0)
                    return "";

                string Result = "";

                string RegexString = @"^(\d\d\s){2,22}\d\d";
                Regex regex = new Regex(RegexString, RegexOptions.Compiled | RegexOptions.IgnoreCase);

                for (int i = 0; i < strs.Length; i++)
                {
                    Match m = regex.Match(strs[i]);
                    if (m.Success)
                    {
                        string CanonicalNumber = "";
                        string[] singles = ToSingle_W3(m.Value, ref CanonicalNumber);
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
            private string AnalyseScheme_W4(string Content, int PlayType)
            {
                string[] strs = Content.Split('\n');
                if (strs == null)
                    return "";
                if (strs.Length == 0)
                    return "";

                string Result = "";

                string RegexString = @"^(\d\d\s){3,22}\d\d";
                Regex regex = new Regex(RegexString, RegexOptions.Compiled | RegexOptions.IgnoreCase);

                for (int i = 0; i < strs.Length; i++)
                {
                    Match m = regex.Match(strs[i]);
                    if (m.Success)
                    {
                        string CanonicalNumber = "";
                        string[] singles = ToSingle_W4(m.Value, ref CanonicalNumber);
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
            private string AnalyseScheme_H1(string Content, int PlayType)
            {
                string[] strs = Content.Split('\n');
                if (strs == null)
                    return "";
                if (strs.Length == 0)
                    return "";

                string Result = "";

                string RegexString = @"^(\d\d)";
                Regex regex = new Regex(RegexString, RegexOptions.Compiled | RegexOptions.IgnoreCase);

                for (int i = 0; i < strs.Length; i++)
                {
                    Match m = regex.Match(strs[i]);
                    if (m.Success)
                    {
                        string CanonicalNumber = "";
                        string[] singles = ToSingle_H1(m.Value, ref CanonicalNumber);
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
            private string AnalyseScheme_H2(string Content, int PlayType)
            {
                string[] strs = Content.Split('\n');
                if (strs == null)
                    return "";
                if (strs.Length == 0)
                    return "";

                string Result = "";

                string RegexString = @"^((\d\d\s){0,21}(\d\d))[|]((\d\d\s){0,21}\d\d)";
                Regex regex = new Regex(RegexString, RegexOptions.Compiled | RegexOptions.IgnoreCase);

                for (int i = 0; i < strs.Length; i++)
                {
                    Match m = regex.Match(strs[i]);
                    if (m.Success)
                    {
                        string CanonicalNumber = "";
                        string[] singles = ToSingle_H2(m.Value, ref CanonicalNumber);
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
            private string AnalyseScheme_H3(string Content, int PlayType)
            {
                string[] strs = Content.Split('\n');
                if (strs == null)
                    return "";
                if (strs.Length == 0)
                    return "";

                string Result = "";

                string RegexString = @"^((\d\d\s){0,20}\d\d)[|]((\d\d\s){0,20}\d\d)[|]((\d\d\s){0,20}\d\d)";
                Regex regex = new Regex(RegexString, RegexOptions.Compiled | RegexOptions.IgnoreCase);

                for (int i = 0; i < strs.Length; i++)
                {
                    Match m = regex.Match(strs[i]);
                    if (m.Success)
                    {
                        string CanonicalNumber = "";
                        string[] singles = ToSingle_H3(m.Value, ref CanonicalNumber);
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
            #endregion

            public override bool AnalyseWinNumber(string Number)
            {
                Regex regex = new Regex(@"^((\d\d\s){4}\d\d)", RegexOptions.Compiled | RegexOptions.IgnoreCase);

                return regex.IsMatch(Number);
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
                    Result[i] = al[i].ToString().PadLeft(2, '0');

                return Result;
            }

            public override string ShowNumber(string Number)
            {
                return base.ShowNumber(Number, " ");
            }
        }
    }
}