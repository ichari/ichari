using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Text.RegularExpressions;

namespace SLS
{
    public partial class Lottery
    {

    /// <summary>
    /// 广西快乐十分
    /// </summary>
    public partial class GXKLSF : LotteryBase
    {
        #region 静态变量
        public const int PlayType_HYT = 7601;
        public const int PlayType_HY1 = 7602;
        public const int PlayType_HY2 = 7603;
        public const int PlayType_Z_HY3 = 7604;
        public const int PlayType_T_HY3 = 7605;
        public const int PlayType_B_HY3 = 7606;
        public const int PlayType_Z_HY4 = 7607;
        public const int PlayType_T_HY4 = 7608;
        public const int PlayType_B_HY4 = 7609;
        public const int PlayType_Z_HY5 = 7610;
        public const int PlayType_T_HY5 = 7611;
        public const int PlayType_B_HY5 = 7612;

        public const int ID = 76;
        public const string sID = "76";
        public const string Name = "广西快乐十分";
        public const string Code = "GXKLSF";
        public const double MaxMoney = 2000000;
        #endregion

        public GXKLSF()
        {
            id = 76;
            name = "广西快乐十分";
            code = "GXKLSF";
        }

        public override bool CheckPlayType(int play_type) //id = 76
        {
            return ((play_type >= 7601) && (play_type <= 7612));
        }

        public override PlayType[] GetPlayTypeList()
        {
            PlayType[] Result = new PlayType[12];

            Result[0] = new PlayType(PlayType_HYT, "好运特");
            Result[1] = new PlayType(PlayType_HY1, "好运一");
            Result[2] = new PlayType(PlayType_HY2, "好运二");
            Result[3] = new PlayType(PlayType_Z_HY3, "好运三");
            Result[4] = new PlayType(PlayType_T_HY3, "通选好运三");
            Result[5] = new PlayType(PlayType_B_HY3, "包选好运三");
            Result[6] = new PlayType(PlayType_Z_HY4, "好运四");
            Result[7] = new PlayType(PlayType_T_HY4, "通选好运四");
            Result[8] = new PlayType(PlayType_B_HY4, "包选好运四");
            Result[9] = new PlayType(PlayType_Z_HY5, "好运五");
            Result[10] = new PlayType(PlayType_T_HY5, "通选好运五");
            Result[11] = new PlayType(PlayType_B_HY5, "包选好运五");

            return Result;
        }

        public override string BuildNumber(int Num, int Type)
        {
            return "";
        }

        public override string[] ToSingle(string Number, ref string CanonicalNumber, int PlayType)	// 后面 ref 参数是将彩票规范化，如：01 01 02... 变成 01 02...
        {
            if (PlayType == PlayType_HYT)
                return ToSingle_HYT(Number, ref CanonicalNumber);

            if (PlayType == PlayType_HY1)
                return ToSingle_HY1(Number, ref CanonicalNumber);

            if (PlayType == PlayType_HY2)
                return ToSingle_HY2(Number, ref CanonicalNumber);

            if (PlayType == PlayType_Z_HY3 || PlayType == PlayType_T_HY3)
                return ToSingle_HY3(Number, ref CanonicalNumber);

            if (PlayType == PlayType_B_HY3)
                return ToSingle_B_HY3(Number, ref CanonicalNumber);

            if (PlayType == PlayType_Z_HY4 || PlayType == PlayType_T_HY4)
                return ToSingle_HY4(Number, ref CanonicalNumber);

            if (PlayType == PlayType_B_HY4)
                return ToSingle_B_HY4(Number, ref CanonicalNumber);

            if (PlayType == PlayType_Z_HY5 || PlayType == PlayType_T_HY5)
                return ToSingle_HY5(Number, ref CanonicalNumber);

            if (PlayType == PlayType_B_HY5)
                return ToSingle_B_HY5(Number, ref CanonicalNumber);

            return null;
        }
        #region ToSingle 的具体方法
        private string[] ToSingle_HYT(string Number, ref string CanonicalNumber)
        {
            string[] strs = FilterRepeated(Number.Trim().Split(' '), 21);
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
        private string[] ToSingle_HY1(string Number, ref string CanonicalNumber)
        {
            string[] strs = FilterRepeated(Number.Trim().Split(' '), 21);
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
        private string[] ToSingle_HY2(string Number, ref string CanonicalNumber)
        {
            string[] strs = FilterRepeated(Number.Trim().Split(' '), 21);
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
        private string[] ToSingle_HY3(string Number, ref string CanonicalNumber)
        {
            string[] strs = FilterRepeated(Number.Trim().Split(' '), 21);
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
        private string[] ToSingle_HY4(string Number, ref string CanonicalNumber)
        {
            string[] strs = FilterRepeated(Number.Trim().Split(' '), 21);
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
        private string[] ToSingle_HY5(string Number, ref string CanonicalNumber)
        {
            string[] strs = FilterRepeated(Number.Trim().Split(' '), 21);
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
        private string[] ToSingle_B_HY3(string Number, ref string CanonicalNumber)
        {
            string[] strs = FilterRepeated(Number.Trim().Split(' '), 21);
            CanonicalNumber = "";

            if (strs.Length < 3)
            {
                CanonicalNumber = "";
                return null;
            }

            for (int i = 0; i < strs.Length; i++)
                CanonicalNumber += (strs[i] + " ");
            CanonicalNumber = CanonicalNumber.Trim();

            string[] Result = new string[1];
            Result[0] = CanonicalNumber;
            return Result;
        }
        private string[] ToSingle_B_HY4(string Number, ref string CanonicalNumber)
        {
            string[] strs = FilterRepeated(Number.Trim().Split(' '), 21);
            CanonicalNumber = "";

            if (strs.Length < 3)
            {
                CanonicalNumber = "";
                return null;
            }

            for (int i = 0; i < strs.Length; i++)
                CanonicalNumber += (strs[i] + " ");
            CanonicalNumber = CanonicalNumber.Trim();

            string[] Result = new string[1];
            Result[0] = CanonicalNumber;

            return Result;
        }
        private string[] ToSingle_B_HY5(string Number, ref string CanonicalNumber)
        {
            string[] strs = FilterRepeated(Number.Trim().Split(' '), 21);
            CanonicalNumber = "";

            if (strs.Length < 3)
            {
                CanonicalNumber = "";
                return null;
            }

            for (int i = 0; i < strs.Length; i++)
                CanonicalNumber += (strs[i] + " ");
            CanonicalNumber = CanonicalNumber.Trim();

            string[] Result = new string[1];
            Result[0] = CanonicalNumber;

            return Result;
        }
        #endregion

        public override double ComputeWin(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, int PlayType, params double[] WinMoneyList)
        {
            Description = "";
            WinMoneyNoWithTax = 0;

            if ((WinMoneyList == null) || (WinMoneyList.Length < 39)) //奖金参数排列顺序直选奖(6)，通选奖(8)，包选奖(25)
                return -3;

            int WinCount_HYT = 0;
            int WinCount_Z_HY1 = 0, WinCount_Z_HY2 = 0, WinCount_Z_HY3 = 0, WinCount_Z_HY4 = 0, WinCount_Z_HY5 = 0;
            int WinCount_T_HY3_Z2 = 0, WinCount_T_HY3_Z3 = 0, WinCount_T_HY4_Z2 = 0, WinCount_T_HY4_Z3 = 0, WinCount_T_HY4_Z4 = 0, WinCount_T_HY5_Z3 = 0, WinCount_T_HY5_Z4 = 0, WinCount_T_HY5_Z5 = 0;
            int WinCount_B_HY3_4Z3 = 0, WinCount_B_HY3_4Z4 = 0, WinCount_B_HY3_5Z3 = 0, WinCount_B_HY3_5Z4 = 0, WinCount_B_HY3_5Z5 = 0, WinCount_B_HY3_6Z3 = 0, WinCount_B_HY3_6Z4 = 0, WinCount_B_HY3_6Z5 = 0;
            int WinCount_B_HY4_5Z4 = 0, WinCount_B_HY4_5Z5 = 0, WinCount_B_HY4_6Z4 = 0, WinCount_B_HY4_6Z5 = 0, WinCount_B_HY4_7Z4 = 0, WinCount_B_HY4_7Z5 = 0, WinCount_B_HY4_8Z4 = 0, WinCount_B_HY4_8Z5 = 0;
            int WinCount_B_HY4_9Z4 = 0, WinCount_B_HY4_9Z5 = 0, WinCount_B_HY4_10Z4 = 0, WinCount_B_HY4_10Z5 = 0;
            int WinCount_B_HY5_6Z5 = 0, WinCount_B_HY5_7Z5 = 0, WinCount_B_HY5_8Z5 = 0, WinCount_B_HY5_9Z5 = 0, WinCount_B_HY5_10Z5 = 0;

            if (PlayType == PlayType_HYT)
                return ComputeWin_HYT(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[0], WinMoneyList[1], ref WinCount_HYT);

            if (PlayType == PlayType_HY1)
                return ComputeWin_HY1(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[2], WinMoneyList[3], ref WinCount_Z_HY1);

            if (PlayType == PlayType_HY2)
                return ComputeWin_HY2(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[4], WinMoneyList[5], ref WinCount_Z_HY2);

            if (PlayType == PlayType_Z_HY3)
                return ComputeWin_Z_HY3(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[6], WinMoneyList[7], ref WinCount_Z_HY3);

            if (PlayType == PlayType_T_HY3)
                return ComputeWin_T_HY3(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[12], WinMoneyList[13], WinMoneyList[14], 
                    WinMoneyList[15], ref WinCount_T_HY3_Z2, ref WinCount_T_HY3_Z3);

            if (PlayType == PlayType_B_HY3)
                return ComputeWin_B_HY3(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[28], WinMoneyList[29], WinMoneyList[30],
                    WinMoneyList[31], WinMoneyList[32], WinMoneyList[33], WinMoneyList[34], WinMoneyList[35], WinMoneyList[36], WinMoneyList[37],
                     WinMoneyList[38], WinMoneyList[39], WinMoneyList[40], WinMoneyList[41], WinMoneyList[42], WinMoneyList[43], ref WinCount_B_HY3_4Z3,
                    ref WinCount_B_HY3_4Z4, ref WinCount_B_HY3_5Z3, ref WinCount_B_HY3_5Z4, ref WinCount_B_HY3_5Z5, ref WinCount_B_HY3_6Z3, ref WinCount_B_HY3_6Z4,
                    ref WinCount_B_HY3_6Z5);

            if (PlayType == PlayType_Z_HY4)
                return ComputeWin_Z_HY4(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[8], WinMoneyList[9], ref WinCount_Z_HY4);

            if (PlayType == PlayType_T_HY4)
                return ComputeWin_T_HY4(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[16], WinMoneyList[17], WinMoneyList[18], 
                    WinMoneyList[19], WinMoneyList[20], WinMoneyList[21], ref WinCount_T_HY4_Z2, ref WinCount_T_HY4_Z3, ref WinCount_T_HY4_Z4);

            if (PlayType == PlayType_B_HY4)
                return ComputeWin_B_HY4(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[44], WinMoneyList[45], WinMoneyList[46],
                    WinMoneyList[47], WinMoneyList[48], WinMoneyList[49], WinMoneyList[50], WinMoneyList[51], WinMoneyList[52], WinMoneyList[53], WinMoneyList[54],
                    WinMoneyList[55], WinMoneyList[56], WinMoneyList[57], WinMoneyList[58], WinMoneyList[59], WinMoneyList[60], WinMoneyList[61], WinMoneyList[62], 
                    WinMoneyList[63], WinMoneyList[64], WinMoneyList[65], WinMoneyList[66], WinMoneyList[67], ref WinCount_B_HY4_5Z4,ref WinCount_B_HY4_5Z5, 
                    ref WinCount_B_HY4_6Z4, ref WinCount_B_HY4_6Z5, ref WinCount_B_HY4_7Z4, ref WinCount_B_HY4_7Z5, ref WinCount_B_HY4_8Z4, ref WinCount_B_HY4_8Z5, 
                    ref WinCount_B_HY4_9Z4, ref WinCount_B_HY4_9Z5, ref WinCount_B_HY4_10Z4,ref WinCount_B_HY4_10Z5);

            if (PlayType == PlayType_Z_HY5)
                return ComputeWin_Z_HY5(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[10], WinMoneyList[11], ref WinCount_Z_HY5);

            if (PlayType == PlayType_T_HY5)
                return ComputeWin_T_HY5(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[22], WinMoneyList[23], WinMoneyList[24],
                    WinMoneyList[25], WinMoneyList[26], WinMoneyList[27], ref WinCount_T_HY5_Z3, ref WinCount_T_HY5_Z4, ref WinCount_T_HY5_Z5);

            if (PlayType == PlayType_B_HY5)
                return ComputeWin_B_HY5(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, WinMoneyList[68], WinMoneyList[69], WinMoneyList[70],
                    WinMoneyList[71], WinMoneyList[72], WinMoneyList[73], WinMoneyList[74], WinMoneyList[75], WinMoneyList[76],
                    WinMoneyList[77], ref WinCount_B_HY5_6Z5,ref WinCount_B_HY5_7Z5, ref WinCount_B_HY5_8Z5, ref WinCount_B_HY5_9Z5, ref WinCount_B_HY5_10Z5);

            return -4;
        }
        #region ComputeWin  的具体方法
        private double ComputeWin_HYT(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney1, double WinMoneyNoWithTax1, ref int WinCount_HYT)
        {
            WinNumber = WinNumber.Trim();
            if (WinNumber.Length < 14)	//14: "01 02 03 04 05"
                return -1;
            string[] Lotterys = SplitLotteryNumber(Number);
            if (Lotterys == null)
                return -2;
            if (Lotterys.Length < 1)
                return -2;

            WinCount_HYT = 0;
            double WinMoney = 0;
            WinMoneyNoWithTax = 0;
            Description = "";

            for (int ii = 0; ii < Lotterys.Length; ii++)
            {
                string t_str = "";
                string[] Lottery = ToSingle_HYT(Lotterys[ii], ref t_str);
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
                        if (WinNumber.Substring(WinNumber.Length - 2) == Red[j])
                            Count++;
                    }
                    if (!Full)
                        continue;

                    if (Count == 1)
                    {
                        WinCount_HYT++;
                        WinMoney += WinMoney1;
                        WinMoneyNoWithTax += WinMoneyNoWithTax1;
                        continue;
                    }
                }
            }

            if (WinCount_HYT > 0)
            {
                Description = "直选好运特奖" + WinCount_HYT.ToString() + "注";
            }

            if (Description != "")
                Description += "。";

            return WinMoney;
        }
        private double ComputeWin_HY1(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney2, double WinMoneyNoWithTax2, ref int WinCount_Z_HY1)
        {
            WinNumber = WinNumber.Trim();
            if (WinNumber.Length < 14)	//14: "01 02 03 04 05"
                return -1;
            string[] Lotterys = SplitLotteryNumber(Number);
            if (Lotterys == null)
                return -2;
            if (Lotterys.Length < 1)
                return -2;

            WinCount_Z_HY1 = 0;
            double WinMoney = 0;
            WinMoneyNoWithTax = 0;
            Description = "";

            for (int ii = 0; ii < Lotterys.Length; ii++)
            {
                string t_str = "";
                string[] Lottery = ToSingle_HY1(Lotterys[ii], ref t_str);
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
                        if (WinNumber.IndexOf(Red[j].Trim()) >= 0)
                            Count++;
                    }
                    if (!Full)
                        continue;

                    if (Count == 1)
                    {
                        WinCount_Z_HY1++;
                        WinMoney += WinMoney2;
                        WinMoneyNoWithTax += WinMoneyNoWithTax2;
                        continue;
                    }
                }
            }

            if (WinCount_Z_HY1 > 0)
            {
                Description = "直选好运一奖" + WinCount_Z_HY1.ToString() + "注";
            }

            if (Description != "")
                Description += "。";

            return WinMoney;
        }
        private double ComputeWin_HY2(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney3, double WinMoneyNoWithTax3, ref int WinCount_Z_HY2)
        {
            WinNumber = WinNumber.Trim();
            if (WinNumber.Length < 14)	//14: "01 02 03 04 05"
                return -1;
            string[] Lotterys = SplitLotteryNumber(Number);
            if (Lotterys == null)
                return -2;
            if (Lotterys.Length < 1)
                return -2;

            WinCount_Z_HY2 = 0;
            double WinMoney = 0;
            WinMoneyNoWithTax = 0;
            Description = "";

            string[] Ball = null;

            string RegexString = @"^((\d\d\s){1,20}\d\d)";
            Regex regex = new Regex(RegexString, RegexOptions.Compiled | RegexOptions.IgnoreCase);

            for (int ii = 0; ii < Lotterys.Length; ii++)
            {
                Match m = regex.Match(Lotterys[ii]);

                if (!m.Success)
                {
                    continue;
                }

                Ball = FilterRepeated(m.Value.Trim().Split(' '), 21);

                if (Ball.Length < 2)
                {
                    continue;
                }

                int BallRight = 0;

                foreach (string str in Ball)
                {
                    if (!string.IsNullOrEmpty(str))
                    {
                        if (WinNumber.IndexOf(str.Trim()) >= 0)
                        {
                            BallRight++;
                        }
                    }
                }

                if (BallRight < 2)
                {
                    continue;
                }

                int Count = GetNum(2, BallRight);

                if (Count > 0)
                {
                    WinCount_Z_HY2 = WinCount_Z_HY2 + Count;
                    WinMoney += WinMoney3 * Count;
                    WinMoneyNoWithTax += WinMoneyNoWithTax3 * Count;
                    continue;
                }
            }

            if (WinCount_Z_HY2 > 0)
            {
                Description = "直选好运二奖" + WinCount_Z_HY2.ToString() + "注";
            }

            if (Description != "")
                Description += "。";

            return WinMoney;
        }
        private double ComputeWin_Z_HY3(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney4, double WinMoneyNoWithTax4, ref int WinCount_Z_HY3)
        {
            WinNumber = WinNumber.Trim();
            if (WinNumber.Length < 14)	//14: "01 02 03 04 05"
                return -1;
            string[] Lotterys = SplitLotteryNumber(Number);
            if (Lotterys == null)
                return -2;
            if (Lotterys.Length < 1)
                return -2;

            WinCount_Z_HY3 = 0;
            double WinMoney = 0;
            WinMoneyNoWithTax = 0;
            Description = "";

            string[] Ball = null;

            string RegexString = @"^((\d\d\s){2,20}\d\d)";
            Regex regex = new Regex(RegexString, RegexOptions.Compiled | RegexOptions.IgnoreCase);

            for (int ii = 0; ii < Lotterys.Length; ii++)
            {
                Match m = regex.Match(Lotterys[ii]);

                if (!m.Success)
                {
                    continue;
                }

                Ball = FilterRepeated(m.Value.Trim().Split(' '), 21);

                if (Ball.Length < 3)
                {
                    continue;
                }

                int BallRight = 0;

                foreach (string str in Ball)
                {
                    if (!string.IsNullOrEmpty(str))
                    {
                        if (WinNumber.IndexOf(str.Trim()) >= 0)
                        {
                            BallRight++;
                        }
                    }
                }

                if (BallRight < 3)
                {
                    continue;
                }

                int Count = GetNum(3, BallRight);

                if (Count > 0)
                {
                    WinCount_Z_HY3 = WinCount_Z_HY3 + Count;
                    WinMoney += WinMoney4 * Count;
                    WinMoneyNoWithTax += WinMoneyNoWithTax4 * Count;
                    continue;
                }
            }

            if (WinCount_Z_HY3 > 0)
            {
                Description = "直选好运三奖" + WinCount_Z_HY3.ToString() + "注";
            }

            if (Description != "")
                Description += "。";

            return WinMoney;
        }
        private double ComputeWin_Z_HY4(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney5, double WinMoneyNoWithTax5, ref int WinCount_Z_HY4)
        {
            WinNumber = WinNumber.Trim();
            if (WinNumber.Length < 14)	//14: "01 02 03 04 05"
                return -1;
            string[] Lotterys = SplitLotteryNumber(Number);
            if (Lotterys == null)
                return -2;
            if (Lotterys.Length < 1)
                return -2;

            WinCount_Z_HY4 = 0;
            double WinMoney = 0;
            WinMoneyNoWithTax = 0;
            Description = "";

            string[] Ball = null;

            string RegexString = @"^((\d\d\s){3,20}\d\d)";
            Regex regex = new Regex(RegexString, RegexOptions.Compiled | RegexOptions.IgnoreCase);

            for (int ii = 0; ii < Lotterys.Length; ii++)
            {
                Match m = regex.Match(Lotterys[ii]);

                if (!m.Success)
                {
                    continue;
                }

                Ball = FilterRepeated(m.Value.Trim().Split(' '), 21);

                if (Ball.Length < 4)
                {
                    continue;
                }

                int BallRight = 0;

                foreach (string str in Ball)
                {
                    if (!string.IsNullOrEmpty(str))
                    {
                        if (WinNumber.IndexOf(str.Trim()) >= 0)
                        {
                            BallRight++;
                        }
                    }
                }

                if (BallRight < 4)
                {
                    continue;
                }

                int Count = GetNum(4, BallRight);

                if (Count > 0)
                {
                    WinCount_Z_HY4 = WinCount_Z_HY4 + Count;
                    WinMoney += WinMoney5 * Count;
                    WinMoneyNoWithTax += WinMoneyNoWithTax5 * Count;
                    continue;
                }
            }

            if (WinCount_Z_HY4 > 0)
            {
                Description = "直选好运四奖" + WinCount_Z_HY4.ToString() + "注";
            }

            if (Description != "")
                Description += "。";

            return WinMoney;
        }
        private double ComputeWin_Z_HY5(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney6, double WinMoneyNoWithTax6, ref int WinCount_Z_HY5)
        {
            WinNumber = WinNumber.Trim();
            if (WinNumber.Length < 14)	//14: "01 02 03 04 05"
                return -1;
            string[] Lotterys = SplitLotteryNumber(Number);
            if (Lotterys == null)
                return -2;
            if (Lotterys.Length < 1)
                return -2;

            WinCount_Z_HY5 = 0;
            double WinMoney = 0;
            WinMoneyNoWithTax = 0;
            Description = "";

            string[] Ball = null;

            string RegexString = @"^((\d\d\s){4,20}\d\d)";
            Regex regex = new Regex(RegexString, RegexOptions.Compiled | RegexOptions.IgnoreCase);

            for (int ii = 0; ii < Lotterys.Length; ii++)
            {
                Match m = regex.Match(Lotterys[ii]);

                if (!m.Success)
                {
                    continue;
                }

                Ball = FilterRepeated(m.Value.Trim().Split(' '), 21);

                if (Ball.Length < 5)
                {
                    continue;
                }

                int BallRight = 0;

                foreach (string str in Ball)
                {
                    if (!string.IsNullOrEmpty(str))
                    {
                        if (WinNumber.IndexOf(str.Trim()) >= 0)
                        {
                            BallRight++;
                        }
                    }
                }

                if (BallRight < 5)
                {
                    continue;
                }

                int Count = GetNum(5, BallRight);

                if (Count > 0)
                {
                    WinCount_Z_HY5 = WinCount_Z_HY5 + Count;
                    WinMoney += WinMoney6 * Count;
                    WinMoneyNoWithTax += WinMoneyNoWithTax6 * Count;
                    continue;
                }
            }

            if (WinCount_Z_HY5 > 0)
            {
                Description = "直选好运五奖" + WinCount_Z_HY5.ToString() + "注";
            }

            if (Description != "")
                Description += "。";

            return WinMoney;
        }
        private double ComputeWin_T_HY3(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney7, double WinMoneyNoWithTax7, double WinMoney8,
            double WinMoneyNoWithTax8, ref int WinCount_T_HY3_Z2, ref int WinCount_T_HY3_Z3)
        {
            WinNumber = WinNumber.Trim();
            if (WinNumber.Length < 14)	//14: "01 02 03 04 05"
                return -1;
            string[] Lotterys = SplitLotteryNumber(Number);
            if (Lotterys == null)
                return -2;
            if (Lotterys.Length < 1)
                return -2;

            WinCount_T_HY3_Z2 = 0;
            WinCount_T_HY3_Z3 = 0;
            double WinMoney = 0;
            WinMoneyNoWithTax = 0;
            Description = "";

            string[] Ball = null;

            string RegexString = @"^((\d\d\s){2,20}\d\d)";
            Regex regex = new Regex(RegexString, RegexOptions.Compiled | RegexOptions.IgnoreCase);

            for (int ii = 0; ii < Lotterys.Length; ii++)
            {
                Match m = regex.Match(Lotterys[ii]);

                if (!m.Success)
                {
                    continue;
                }

                Ball = FilterRepeated(m.Value.Trim().Split(' '), 21);

                if (Ball.Length < 3)
                {
                    continue;
                }

                int BallRight = 0;

                foreach (string str in Ball)
                {
                    if (!string.IsNullOrEmpty(str))
                    {
                        if (WinNumber.IndexOf(str.Trim()) >= 0)
                        {
                            BallRight++;
                        }
                    }
                }

                if (BallRight < 2)
                {
                    continue;
                }

                if (BallRight >= 3)
                {
                    int Count = GetNum(3, BallRight);

                    if (Count > 0)
                    {
                        WinCount_T_HY3_Z3 = WinCount_T_HY3_Z3 + Count;
                        WinMoney += WinMoney7 * Count;
                        WinMoneyNoWithTax += WinMoneyNoWithTax7 * Count;
                    }
                }

                if (Ball.Length - BallRight == 0)
                {
                    continue;
                }

                if (BallRight >= 2)
                {
                    int Count = GetNum(2, BallRight) * (Ball.Length - BallRight);

                    if (Count > 0)
                    {
                        WinCount_T_HY3_Z2 = WinCount_T_HY3_Z2 + Count;
                        WinMoney += WinMoney7 * Count;
                        WinMoneyNoWithTax += WinMoneyNoWithTax7 * Count;
                        continue;
                    }
                }
            }

            if (WinCount_T_HY3_Z3 > 0)
            {
                Description = "通选三中三奖" + WinCount_T_HY3_Z3.ToString() + "注";
            }

            if (WinCount_T_HY3_Z2 > 0)
            {
                Description += "通选三中二奖" + WinCount_T_HY3_Z2.ToString() + "注";
            }

            if (Description != "")
                Description += "。";

            return WinMoney;
        }

        private double ComputeWin_T_HY4(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney9, double WinMoneyNoWithTax9, double WinMoney10,
            double WinMoneyNoWithTax10, double WinMoney11,double WinMoneyNoWithTax11, ref int WinCount_T_HY4_Z2, ref int WinCount_T_HY4_Z3, ref int WinCount_T_HY4_Z4)
        {
            WinNumber = WinNumber.Trim();
            if (WinNumber.Length < 14)	//14: "01 02 03 04 05"
                return -1;
            string[] Lotterys = SplitLotteryNumber(Number);
            if (Lotterys == null)
                return -2;
            if (Lotterys.Length < 1)
                return -2;

            WinCount_T_HY4_Z2 = 0;
            WinCount_T_HY4_Z3 = 0;
            WinCount_T_HY4_Z4 = 0;
            double WinMoney = 0;
            WinMoneyNoWithTax = 0;
            Description = "";

            string[] Ball = null;

            string RegexString = @"^((\d\d\s){3,20}\d\d)";
            Regex regex = new Regex(RegexString, RegexOptions.Compiled | RegexOptions.IgnoreCase);

            for (int ii = 0; ii < Lotterys.Length; ii++)
            {
                Match m = regex.Match(Lotterys[ii]);

                if (!m.Success)
                {
                    continue;
                }

                Ball = FilterRepeated(m.Value.Trim().Split(' '), 21);

                if (Ball.Length < 4)
                {
                    continue;
                }

                int BallRight = 0;

                foreach (string str in Ball)
                {
                    if (!string.IsNullOrEmpty(str))
                    {
                        if (WinNumber.IndexOf(str.Trim()) >= 0)
                        {
                            BallRight++;
                        }
                    }
                }

                if (BallRight < 2)
                {
                    continue;
                }

                if (BallRight >= 4)
                {
                    int Count = GetNum(4, BallRight);

                    if (Count > 0)
                    {
                        WinCount_T_HY4_Z4 = WinCount_T_HY4_Z4 + Count;
                        WinMoney += WinMoney11 * Count;
                        WinMoneyNoWithTax += WinMoneyNoWithTax11 * Count;
                    }
                }

                if (Ball.Length - BallRight == 0)
                {
                    continue;
                }

                if (BallRight >= 3)
                {
                    int Count = GetNum(3, BallRight) * (Ball.Length - BallRight);

                    if (Count > 0)
                    {
                        WinCount_T_HY4_Z3 = WinCount_T_HY4_Z3 + Count;
                        WinMoney += WinMoney10 * Count;
                        WinMoneyNoWithTax += WinMoneyNoWithTax10 * Count;
                    }
                }

                if (Ball.Length - BallRight == 1)
                {
                    continue;
                }

                if (BallRight >= 2)
                {
                    int Count = GetNum(2, BallRight) * GetNum(2, Ball.Length - BallRight);

                    if (Count > 0)
                    {
                        WinCount_T_HY4_Z2 = WinCount_T_HY4_Z2 + Count;
                        WinMoney += WinMoney9 * Count;
                        WinMoneyNoWithTax += WinMoneyNoWithTax9 * Count;
                        continue;
                    }
                }
            }

            if (WinCount_T_HY4_Z4 > 0)
            {
                Description = "通选四中四奖" + WinCount_T_HY4_Z4.ToString() + "注";
            }

            if (WinCount_T_HY4_Z3 > 0)
            {
                Description += "通选四中三奖" + WinCount_T_HY4_Z3.ToString() + "注";
            }

            if (WinCount_T_HY4_Z2 > 0)
            {
                Description += "通选四中二奖" + WinCount_T_HY4_Z2.ToString() + "注";
            }

            if (Description != "")
                Description += "。";

            return WinMoney;
        }

        private double ComputeWin_T_HY5(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney12, double WinMoneyNoWithTax12, double WinMoney13,
            double WinMoneyNoWithTax13, double WinMoney14, double WinMoneyNoWithTax14, ref int WinCount_T_HY5_Z3, ref int WinCount_T_HY5_Z4, ref int WinCount_T_HY5_Z5)
        {
            WinNumber = WinNumber.Trim();
            if (WinNumber.Length < 14)	//14: "01 02 03 04 05"
                return -1;
            string[] Lotterys = SplitLotteryNumber(Number);
            if (Lotterys == null)
                return -2;
            if (Lotterys.Length < 1)
                return -2;

            WinCount_T_HY5_Z3 = 0;
            WinCount_T_HY5_Z4 = 0;
            WinCount_T_HY5_Z5 = 0;
            double WinMoney = 0;
            WinMoneyNoWithTax = 0;
            Description = "";

            string[] Ball = null;

            string RegexString = @"^((\d\d\s){4,20}\d\d)";
            Regex regex = new Regex(RegexString, RegexOptions.Compiled | RegexOptions.IgnoreCase);

            for (int ii = 0; ii < Lotterys.Length; ii++)
            {
                Match m = regex.Match(Lotterys[ii]);

                if (!m.Success)
                {
                    continue;
                }

                Ball = FilterRepeated(m.Value.Trim().Split(' '), 21);

                if (Ball.Length < 5)
                {
                    continue;
                }

                int BallRight = 0;

                foreach (string str in Ball)
                {
                    if (!string.IsNullOrEmpty(str))
                    {
                        if (WinNumber.IndexOf(str.Trim()) >= 0)
                        {
                            BallRight++;
                        }
                    }
                }

                if (BallRight < 3)
                {
                    continue;
                }

                if (BallRight == 5)
                {
                    WinCount_T_HY5_Z5++;
                    WinMoney += WinMoney14;
                    WinMoneyNoWithTax += WinMoneyNoWithTax14;
                }

                if (BallRight >= 4)
                {
                    int Count = GetNum(4, BallRight) * (Ball.Length - BallRight);

                    if (Count > 0)
                    {
                        WinCount_T_HY5_Z4 = WinCount_T_HY5_Z4 + Count;
                        WinMoney += WinMoney13 * Count;
                        WinMoneyNoWithTax += WinMoneyNoWithTax13 * Count;
                    }
                }

                if (BallRight >= 3)
                {
                    int Count = GetNum(3, BallRight) * GetNum(2, Ball.Length - BallRight);

                    if (Count > 0)
                    {
                        WinCount_T_HY5_Z3 = WinCount_T_HY5_Z3 + Count;
                        WinMoney += WinMoney12 * Count;
                        WinMoneyNoWithTax += WinMoneyNoWithTax12 * Count;
                        continue;
                    }
                }
            }

            if (WinCount_T_HY5_Z5 > 0)
            {
                Description = "通选五中五奖" + WinCount_T_HY5_Z5.ToString() + "注";
            }

            if (WinCount_T_HY5_Z4 > 0)
            {
                Description += "通选五中四奖" + WinCount_T_HY5_Z4.ToString() + "注";
            }

            if (WinCount_T_HY5_Z3 > 0)
            {
                Description += "通选五中三奖" + WinCount_T_HY5_Z3.ToString() + "注";
            }

            if (Description != "")
                Description += "。";

            return WinMoney;
        }

        private double ComputeWin_B_HY3(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney15, double WinMoneyNoWithTax15, double WinMoney16,
            double WinMoneyNoWithTax16, double WinMoney17, double WinMoneyNoWithTax17, double WinMoney18, double WinMoneyNoWithTax18, double WinMoney19, double WinMoneyNoWithTax19,
            double WinMoney20, double WinMoneyNoWithTax20, double WinMoney21, double WinMoneyNoWithTax21, double WinMoney22, double WinMoneyNoWithTax22, ref int WinCount_B_HY3_4Z3,
            ref int WinCount_B_HY3_4Z4, ref int WinCount_B_HY3_5Z3, ref int WinCount_B_HY3_5Z4, ref int WinCount_B_HY3_5Z5, ref int WinCount_B_HY3_6Z3, ref int WinCount_B_HY3_6Z4, ref int WinCount_B_HY3_6Z5)
        {
            WinNumber = WinNumber.Trim();
            if (WinNumber.Length < 14)	//14: "01 02 03 04 05"
                return -1;
            string[] Lotterys = SplitLotteryNumber(Number);
            if (Lotterys == null)
                return -2;
            if (Lotterys.Length < 1)
                return -2;

            WinCount_B_HY3_4Z3 = 0;
            WinCount_B_HY3_4Z4 = 0;
            WinCount_B_HY3_5Z3 = 0;
            WinCount_B_HY3_5Z4 = 0;
            WinCount_B_HY3_5Z5 = 0;
            WinCount_B_HY3_6Z3 = 0;
            WinCount_B_HY3_6Z4 = 0;
            WinCount_B_HY3_6Z5 = 0;
            double WinMoney = 0;
            WinMoneyNoWithTax = 0;
            Description = "";

            string[] Ball = null;

            string RegexString = @"^((\d\d\s){3,6}\d\d)";
            Regex regex = new Regex(RegexString, RegexOptions.Compiled | RegexOptions.IgnoreCase);

            for (int ii = 0; ii < Lotterys.Length; ii++)
            {
                Match m = regex.Match(Lotterys[ii]);

                if (!m.Success)
                {
                    continue;
                }

                Ball = FilterRepeated(m.Value.Trim().Split(' '), 21);

                if (Ball.Length < 4)
                {
                    continue;
                }

                int BallRight = 0;

                foreach (string str in Ball)
                {
                    if (!string.IsNullOrEmpty(str))
                    {
                        if (WinNumber.IndexOf(str.Trim()) >= 0)
                        {
                            BallRight++;
                        }
                    }
                }

                if (BallRight < 3)
                {
                    continue;
                }

                if (BallRight == 3)
                {
                    if (Ball.Length == 4)
                    {
                        WinCount_B_HY3_4Z3++;
                        WinMoney += WinMoney15;
                        WinMoneyNoWithTax += WinMoneyNoWithTax15;
                        continue;
                    }

                    if (Ball.Length == 5)
                    {
                        WinCount_B_HY3_5Z3++;
                        WinMoney += WinMoney17;
                        WinMoneyNoWithTax += WinMoneyNoWithTax17;
                        continue;
                    }

                    if (Ball.Length == 6)
                    {
                        WinCount_B_HY3_6Z3++;
                        WinMoney += WinMoney20;
                        WinMoneyNoWithTax += WinMoneyNoWithTax20;
                        continue;
                    }
                }

                if (BallRight == 4)
                {
                    if (Ball.Length == 4)
                    {
                        WinCount_B_HY3_4Z4++;
                        WinMoney += WinMoney16;
                        WinMoneyNoWithTax += WinMoneyNoWithTax16;
                        continue;
                    }

                    if (Ball.Length == 5)
                    {
                        WinCount_B_HY3_5Z4++;
                        WinMoney += WinMoney18;
                        WinMoneyNoWithTax += WinMoneyNoWithTax18;
                        continue;
                    }

                    if (Ball.Length == 6)
                    {
                        WinCount_B_HY3_6Z4++;
                        WinMoney += WinMoney21;
                        WinMoneyNoWithTax += WinMoneyNoWithTax21;
                        continue;
                    }
                }

                if (BallRight == 5)
                {
                    if (Ball.Length == 5)
                    {
                        WinCount_B_HY3_5Z5++;
                        WinMoney += WinMoney19;
                        WinMoneyNoWithTax += WinMoneyNoWithTax19;
                        continue;
                    }

                    if (Ball.Length == 6)
                    {
                        WinCount_B_HY3_6Z5++;
                        WinMoney += WinMoney22;
                        WinMoneyNoWithTax += WinMoneyNoWithTax22;
                        continue;
                    }
                }
            }

            if (WinCount_B_HY3_4Z3 > 0)
            {
                Description += "包选好运三四中三奖" + WinCount_B_HY3_4Z3.ToString() + "注";
            }

            if (WinCount_B_HY3_5Z3 > 0)
            {
                Description += "包选好运三五中三奖" + WinCount_B_HY3_5Z3.ToString() + "注";
            }

            if (WinCount_B_HY3_6Z3 > 0)
            {
                Description += "包选好运三六中三奖" + WinCount_B_HY3_6Z3.ToString() + "注";
            }

            if (WinCount_B_HY3_4Z4 > 0)
            {
                Description += "包选好运三四中四奖" + WinCount_B_HY3_4Z4.ToString() + "注";
            }

            if (WinCount_B_HY3_5Z4 > 0)
            {
                Description += "包选好运三五中四奖" + WinCount_B_HY3_5Z4.ToString() + "注";
            }

            if (WinCount_B_HY3_6Z4 > 0)
            {
                Description += "包选好运三六中四奖" + WinCount_B_HY3_6Z4.ToString() + "注";
            }

            if (WinCount_B_HY3_5Z5 > 0)
            {
                Description += "包选好运三五中五奖" + WinCount_B_HY3_6Z4.ToString() + "注";
            }

            if (WinCount_B_HY3_6Z5 > 0)
            {
                Description += "包选好运三六中五奖" + WinCount_B_HY3_6Z5.ToString() + "注";
            }

            if (Description != "")
                Description += "。";

            return WinMoney;
        }

        private double ComputeWin_B_HY4(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney23, double WinMoneyNoWithTax23, double WinMoney24,
            double WinMoneyNoWithTax24, double WinMoney25, double WinMoneyNoWithTax25, double WinMoney26, double WinMoneyNoWithTax26, double WinMoney27, double WinMoneyNoWithTax27,
            double WinMoney28, double WinMoneyNoWithTax28, double WinMoney29, double WinMoneyNoWithTax29, double WinMoney30, double WinMoneyNoWithTax30, double WinMoney31, double WinMoneyNoWithTax31,
            double WinMoney32, double WinMoneyNoWithTax32, double WinMoney33, double WinMoneyNoWithTax33, double WinMoney34, double WinMoneyNoWithTax34, 
            ref int WinCount_B_HY4_5Z4,ref int WinCount_B_HY4_5Z5, ref int WinCount_B_HY4_6Z4, ref int WinCount_B_HY4_6Z5, ref int WinCount_B_HY4_7Z4, ref int WinCount_B_HY4_7Z5, ref int WinCount_B_HY4_8Z4,
            ref int WinCount_B_HY4_8Z5, ref int WinCount_B_HY4_9Z4, ref int WinCount_B_HY4_9Z5, ref int WinCount_B_HY4_10Z4, ref int WinCount_B_HY4_10Z5)
        {
            WinNumber = WinNumber.Trim();
            if (WinNumber.Length < 14)	//14: "01 02 03 04 05"
                return -1;
            string[] Lotterys = SplitLotteryNumber(Number);
            if (Lotterys == null)
                return -2;
            if (Lotterys.Length < 1)
                return -2;

            WinCount_B_HY4_5Z4 = 0;
            WinCount_B_HY4_5Z5 = 0;
            WinCount_B_HY4_6Z4 = 0;
            WinCount_B_HY4_6Z5 = 0;
            WinCount_B_HY4_7Z4 = 0;
            WinCount_B_HY4_7Z5 = 0;
            WinCount_B_HY4_8Z4 = 0;
            WinCount_B_HY4_8Z5 = 0;
            WinCount_B_HY4_9Z4 = 0;
            WinCount_B_HY4_9Z5 = 0;
            WinCount_B_HY4_10Z4 = 0;
            WinCount_B_HY4_10Z5 = 0;

            double WinMoney = 0;
            WinMoneyNoWithTax = 0;
            Description = "";

            string[] Ball = null;

            string RegexString = @"^((\d\d\s){4,9}\d\d)";
            Regex regex = new Regex(RegexString, RegexOptions.Compiled | RegexOptions.IgnoreCase);

            for (int ii = 0; ii < Lotterys.Length; ii++)
            {
                Match m = regex.Match(Lotterys[ii]);

                if (!m.Success)
                {
                    continue;
                }

                Ball = FilterRepeated(m.Value.Trim().Split(' '), 21);

                if (Ball.Length < 5)
                {
                    continue;
                }

                int BallRight = 0;

                foreach (string str in Ball)
                {
                    if (!string.IsNullOrEmpty(str))
                    {
                        if (WinNumber.IndexOf(str.Trim()) >= 0)
                        {
                            BallRight++;
                        }
                    }
                }

                if (BallRight < 4)
                {
                    continue;
                }

                if (BallRight == 4)
                {
                    if (Ball.Length == 5)
                    {
                        WinCount_B_HY4_5Z4++;
                        WinMoney += WinMoney23;
                        WinMoneyNoWithTax += WinMoneyNoWithTax23;
                        continue;
                    }

                    if (Ball.Length == 6)
                    {
                        WinCount_B_HY4_6Z4++;
                        WinMoney += WinMoney25;
                        WinMoneyNoWithTax += WinMoneyNoWithTax25;
                        continue;
                    }

                    if (Ball.Length == 7)
                    {
                        WinCount_B_HY4_7Z4++;
                        WinMoney += WinMoney27;
                        WinMoneyNoWithTax += WinMoneyNoWithTax27;
                        continue;
                    }

                    if (Ball.Length == 8)
                    {
                        WinCount_B_HY4_8Z4++;
                        WinMoney += WinMoney29;
                        WinMoneyNoWithTax += WinMoneyNoWithTax29;
                        continue;
                    }

                    if (Ball.Length == 9)
                    {
                        WinCount_B_HY4_9Z4++;
                        WinMoney += WinMoney31;
                        WinMoneyNoWithTax += WinMoneyNoWithTax31;
                        continue;
                    }

                    if (Ball.Length == 10)
                    {
                        WinCount_B_HY4_10Z4++;
                        WinMoney += WinMoney33;
                        WinMoneyNoWithTax += WinMoneyNoWithTax33;
                        continue;
                    }
                }

                if (BallRight == 5)
                {
                    if (Ball.Length == 5)
                    {
                        WinCount_B_HY4_5Z5++;
                        WinMoney += WinMoney24;
                        WinMoneyNoWithTax += WinMoneyNoWithTax24;
                        continue;
                    }

                    if (Ball.Length == 6)
                    {
                        WinCount_B_HY4_6Z5++;
                        WinMoney += WinMoney26;
                        WinMoneyNoWithTax += WinMoneyNoWithTax26;
                        continue;
                    }

                    if (Ball.Length == 7)
                    {
                        WinCount_B_HY4_7Z5++;
                        WinMoney += WinMoney28;
                        WinMoneyNoWithTax += WinMoneyNoWithTax28;
                        continue;
                    }

                    if (Ball.Length == 8)
                    {
                        WinCount_B_HY4_8Z5++;
                        WinMoney += WinMoney30;
                        WinMoneyNoWithTax += WinMoneyNoWithTax30;
                        continue;
                    }

                    if (Ball.Length == 9)
                    {
                        WinCount_B_HY4_9Z5++;
                        WinMoney += WinMoney32;
                        WinMoneyNoWithTax += WinMoneyNoWithTax32;
                        continue;
                    }

                    if (Ball.Length == 10)
                    {
                        WinCount_B_HY4_10Z5++;
                        WinMoney += WinMoney34;
                        WinMoneyNoWithTax += WinMoneyNoWithTax34;
                        continue;
                    }
                }
            }

            if (WinCount_B_HY4_5Z4 > 0)
            {
                Description += "包选好运四五中四奖" + WinCount_B_HY4_5Z4.ToString() + "注";
            }

            if (WinCount_B_HY4_6Z4 > 0)
            {
                Description += "包选好运四六中四奖" + WinCount_B_HY4_6Z4.ToString() + "注";
            }

            if (WinCount_B_HY4_7Z4 > 0)
            {
                Description += "包选好运四七中四奖" + WinCount_B_HY4_7Z4.ToString() + "注";
            }

            if (WinCount_B_HY4_8Z4 > 0)
            {
                Description += "包选好运四八中四奖" + WinCount_B_HY4_8Z4.ToString() + "注";
            }

            if (WinCount_B_HY4_9Z4 > 0)
            {
                Description += "包选好运四九中四奖" + WinCount_B_HY4_9Z4.ToString() + "注";
            }

            if (WinCount_B_HY4_10Z4 > 0)
            {
                Description += "包选好运四十中四奖" + WinCount_B_HY4_10Z4.ToString() + "注";
            }

            if (WinCount_B_HY4_5Z5 > 0)
            {
                Description += "包选好运四五中五奖" + WinCount_B_HY4_5Z5.ToString() + "注";
            }

            if (WinCount_B_HY4_6Z5 > 0)
            {
                Description += "包选好运四六中五奖" + WinCount_B_HY4_6Z5.ToString() + "注";
            }

            if (WinCount_B_HY4_7Z5 > 0)
            {
                Description += "包选好运四七中五奖" + WinCount_B_HY4_7Z5.ToString() + "注";
            }

            if (WinCount_B_HY4_8Z5 > 0)
            {
                Description += "包选好运四八中五奖" + WinCount_B_HY4_8Z5.ToString() + "注";
            }

            if (WinCount_B_HY4_9Z5 > 0)
            {
                Description += "包选好运四九中五奖" + WinCount_B_HY4_9Z5.ToString() + "注";
            }

            if (WinCount_B_HY4_10Z5 > 0)
            {
                Description += "包选好运四十中五奖" + WinCount_B_HY4_10Z5.ToString() + "注";
            }

            if (Description != "")
                Description += "。";

            return WinMoney;
        }

        private double ComputeWin_B_HY5(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, double WinMoney35, double WinMoneyNoWithTax35, double WinMoney36,
            double WinMoneyNoWithTax36, double WinMoney37, double WinMoneyNoWithTax37, double WinMoney38, double WinMoneyNoWithTax38, double WinMoney39, double WinMoneyNoWithTax39, ref int WinCount_B_HY5_6Z5,
            ref int WinCount_B_HY5_7Z5, ref int WinCount_B_HY5_8Z5, ref int WinCount_B_HY5_9Z5, ref int WinCount_B_HY5_10Z5)
        {
            WinNumber = WinNumber.Trim();
            if (WinNumber.Length < 14)	//14: "01 02 03 04 05"
                return -1;
            string[] Lotterys = SplitLotteryNumber(Number);
            if (Lotterys == null)
                return -2;
            if (Lotterys.Length < 1)
                return -2;

            WinCount_B_HY5_6Z5 = 0;
            WinCount_B_HY5_7Z5 = 0;
            WinCount_B_HY5_8Z5 = 0;
            WinCount_B_HY5_9Z5 = 0;
            WinCount_B_HY5_10Z5 = 0;

            double WinMoney = 0;
            WinMoneyNoWithTax = 0;
            Description = "";

            string[] Ball = null;

            string RegexString = @"^((\d\d\s){5,9}\d\d)";
            Regex regex = new Regex(RegexString, RegexOptions.Compiled | RegexOptions.IgnoreCase);

            for (int ii = 0; ii < Lotterys.Length; ii++)
            {
                Match m = regex.Match(Lotterys[ii]);

                if (!m.Success)
                {
                    continue;
                }

                Ball = FilterRepeated(m.Value.Trim().Split(' '), 21);

                if (Ball.Length < 5)
                {
                    continue;
                }

                int BallRight = 0;

                foreach (string str in Ball)
                {
                    if (!string.IsNullOrEmpty(str))
                    {
                        if (WinNumber.IndexOf(str.Trim()) >= 0)
                        {
                            BallRight++;
                        }
                    }
                }

                if (BallRight < 4)
                {
                    continue;
                }

                if (BallRight == 5)
                {
                    if (Ball.Length == 6)
                    {
                        WinCount_B_HY5_6Z5++;
                        WinMoney += WinMoney35;
                        WinMoneyNoWithTax += WinMoneyNoWithTax35;
                        continue;
                    }

                    if (Ball.Length == 7)
                    {
                        WinCount_B_HY5_7Z5++;
                        WinMoney += WinMoney36;
                        WinMoneyNoWithTax += WinMoneyNoWithTax36;
                        continue;
                    }

                    if (Ball.Length == 8)
                    {
                        WinCount_B_HY5_8Z5++;
                        WinMoney += WinMoney37;
                        WinMoneyNoWithTax += WinMoneyNoWithTax37;
                        continue;
                    }

                    if (Ball.Length == 9)
                    {
                        WinCount_B_HY5_9Z5++;
                        WinMoney += WinMoney38;
                        WinMoneyNoWithTax += WinMoneyNoWithTax38;
                        continue;
                    }

                    if (Ball.Length == 10)
                    {
                        WinCount_B_HY5_10Z5++;
                        WinMoney += WinMoney39;
                        WinMoneyNoWithTax += WinMoneyNoWithTax39;
                        continue;
                    }
                }
            }

            if (WinCount_B_HY5_6Z5 > 0)
            {
                Description += "包选好运五六中五奖" + WinCount_B_HY5_6Z5.ToString() + "注";
            }

            if (WinCount_B_HY5_7Z5 > 0)
            {
                Description += "包选好运五七中五奖" + WinCount_B_HY5_7Z5.ToString() + "注";
            }

            if (WinCount_B_HY5_8Z5 > 0)
            {
                Description += "包选好运五八中五奖" + WinCount_B_HY5_8Z5.ToString() + "注";
            }

            if (WinCount_B_HY5_9Z5 > 0)
            {
                Description += "包选好运五九中五奖" + WinCount_B_HY5_9Z5.ToString() + "注";
            }

            if (WinCount_B_HY5_10Z5 > 0)
            {
                Description += "包选好运五十中五奖" + WinCount_B_HY5_10Z5.ToString() + "注";
            }

            if (Description != "")
                Description += "。";

            return WinMoney;
        }

        #endregion

        public override string AnalyseScheme(string Content, int PlayType)
        {
            if (PlayType == PlayType_HYT)
                return AnalyseScheme_HYT(Content, PlayType);

            if (PlayType == PlayType_HY1)
                return AnalyseScheme_HY1(Content, PlayType);

            if (PlayType == PlayType_HY2)
                return AnalyseScheme_HY2(Content, PlayType);

            if (PlayType == PlayType_Z_HY3 || PlayType == PlayType_T_HY3)
                return AnalyseScheme_HY3(Content, PlayType);

            if (PlayType == PlayType_B_HY3)
                return AnalyseScheme_B_HY3(Content, PlayType);

            if (PlayType == PlayType_Z_HY4 || PlayType == PlayType_T_HY4)
                return AnalyseScheme_HY4(Content, PlayType);

            if (PlayType == PlayType_B_HY4)
                return AnalyseScheme_B_HY4(Content, PlayType);

            if (PlayType == PlayType_Z_HY5 || PlayType == PlayType_T_HY5)
                return AnalyseScheme_HY5(Content, PlayType);

            if (PlayType == PlayType_B_HY5)
                return AnalyseScheme_B_HY5(Content, PlayType);

            return "";
        }
        #region AnalyseScheme 的具体方法
        private string AnalyseScheme_HYT(string Content, int PlayType)
        {
            string[] strs = Content.Split('\n');
            if (strs == null)
                return "";
            if (strs.Length == 0)
                return "";

            string Result = "";

            string RegexString = @"^(\d\d\s){0,20}\d\d";
            Regex regex = new Regex(RegexString, RegexOptions.Compiled | RegexOptions.IgnoreCase);

            for (int i = 0; i < strs.Length; i++)
            {
                Match m = regex.Match(strs[i]);
                if (m.Success)
                {
                    string CanonicalNumber = "";
                    string[] singles = ToSingle_HYT(m.Value, ref CanonicalNumber);
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
        private string AnalyseScheme_HY1(string Content, int PlayType)
        {
            string[] strs = Content.Split('\n');
            if (strs == null)
                return "";
            if (strs.Length == 0)
                return "";

            string Result = "";

            string RegexString = @"^(\d\d\s){0,20}\d\d";
            Regex regex = new Regex(RegexString, RegexOptions.Compiled | RegexOptions.IgnoreCase);

            for (int i = 0; i < strs.Length; i++)
            {
                Match m = regex.Match(strs[i]);
                if (m.Success)
                {
                    string CanonicalNumber = "";
                    string[] singles = ToSingle_HY1(m.Value, ref CanonicalNumber);
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
        private string AnalyseScheme_HY2(string Content, int PlayType)
        {
            string[] strs = Content.Split('\n');
            if (strs == null)
                return "";
            if (strs.Length == 0)
                return "";

            string Result = "";

            string RegexString = @"^((\d\d\s){1,20}\d\d)";
            Regex regex = new Regex(RegexString, RegexOptions.Compiled | RegexOptions.IgnoreCase);

            for (int i = 0; i < strs.Length; i++)
            {
                Match m = regex.Match(strs[i]);
                if (!m.Success)
                {
                    continue;
                }

                string CanonicalNumber = "";
                string[] Number = FilterRepeated(m.Value.Trim().Split(' '), 21);

                if (Number.Length < 2)
                {
                    continue;
                }

                for (int j = 0; j < Number.Length; j++)
                {
                    CanonicalNumber += Number[j] + " ";
                }

                Result += CanonicalNumber + "|" + GetNum(2, Number.Length).ToString() + "\n";
            }

            if (Result.EndsWith("\n"))
                Result = Result.Substring(0, Result.Length - 1);
            return Result;
        }
        private string AnalyseScheme_HY3(string Content, int PlayType)
        {
            string[] strs = Content.Split('\n');
            if (strs == null)
                return "";
            if (strs.Length == 0)
                return "";

            string Result = "";

            string RegexString = @"^((\d\d\s){2,20}\d\d)";
            Regex regex = new Regex(RegexString, RegexOptions.Compiled | RegexOptions.IgnoreCase);

            for (int i = 0; i < strs.Length; i++)
            {
                Match m = regex.Match(strs[i]);
                if (!m.Success)
                {
                    continue;
                }

                string CanonicalNumber = "";
                string[] Number = FilterRepeated(m.Value.Trim().Split(' '), 21);

                if (Number.Length < 3)
                {
                    continue;
                }

                for (int j = 0; j < Number.Length; j++)
                {
                    CanonicalNumber += Number[j] + " ";
                }

                Result += CanonicalNumber + "|" + GetNum(3, Number.Length).ToString() + "\n";
            }

            if (Result.EndsWith("\n"))
                Result = Result.Substring(0, Result.Length - 1);
            return Result;
        }
        private string AnalyseScheme_HY4(string Content, int PlayType)
        {
            string[] strs = Content.Split('\n');
            if (strs == null)
                return "";
            if (strs.Length == 0)
                return "";

            string Result = "";

            string RegexString = @"^((\d\d\s){3,20}\d\d)";
            Regex regex = new Regex(RegexString, RegexOptions.Compiled | RegexOptions.IgnoreCase);

            for (int i = 0; i < strs.Length; i++)
            {
                Match m = regex.Match(strs[i]);
                if (!m.Success)
                {
                    continue;
                }

                string CanonicalNumber = "";
                string[] Number = FilterRepeated(m.Value.Trim().Split(' '), 21);

                if (Number.Length < 4)
                {
                    continue;
                }

                for (int j = 0; j < Number.Length; j++)
                {
                    CanonicalNumber += Number[j] + " ";
                }

                Result += CanonicalNumber + "|" + GetNum(4, Number.Length).ToString() + "\n";
            }

            if (Result.EndsWith("\n"))
                Result = Result.Substring(0, Result.Length - 1);
            return Result;
        }
        private string AnalyseScheme_HY5(string Content, int PlayType)
        {
            string[] strs = Content.Split('\n');
            if (strs == null)
                return "";
            if (strs.Length == 0)
                return "";

            string Result = "";

            string RegexString = @"^((\d\d\s){4,20}\d\d)";
            Regex regex = new Regex(RegexString, RegexOptions.Compiled | RegexOptions.IgnoreCase);

            for (int i = 0; i < strs.Length; i++)
            {
                Match m = regex.Match(strs[i]);
                if (!m.Success)
                {
                    continue;
                }

                string CanonicalNumber = "";
                string[] Number = FilterRepeated(m.Value.Trim().Split(' '), 21);

                if (Number.Length < 5)
                {
                    continue;
                }

                for (int j = 0; j < Number.Length; j++)
                {
                    CanonicalNumber += Number[j] + " ";
                }

                Result += CanonicalNumber + "|" + GetNum(5, Number.Length).ToString() + "\n";
            }

            if (Result.EndsWith("\n"))
                Result = Result.Substring(0, Result.Length - 1);
            return Result;
        }
        private string AnalyseScheme_B_HY3(string Content, int PlayType)
        {
            string[] strs = Content.Split('\n');
            if (strs == null)
                return "";
            if (strs.Length == 0)
                return "";

            string Result = "";

            string RegexString = @"^((\d\d\s){3,20}\d\d)";
            Regex regex = new Regex(RegexString, RegexOptions.Compiled | RegexOptions.IgnoreCase);

            for (int i = 0; i < strs.Length; i++)
            {
                Match m = regex.Match(strs[i]);
                if (!m.Success)
                {
                    continue;
                }

                string CanonicalNumber = "";
                string[] Number = FilterRepeated(m.Value.Trim().Split(' '), 21);

                if (Number.Length < 4)
                {
                    continue;
                }

                for (int j = 0; j < Number.Length; j++)
                {
                    CanonicalNumber += Number[j] + " ";
                }

                Result += CanonicalNumber + "|1\n";
            }

            if (Result.EndsWith("\n"))
                Result = Result.Substring(0, Result.Length - 1);
            return Result;
        }
        private string AnalyseScheme_B_HY4(string Content, int PlayType)
        {
            string[] strs = Content.Split('\n');
            if (strs == null)
                return "";
            if (strs.Length == 0)
                return "";

            string Result = "";

            string RegexString = @"^((\d\d\s){4,20}\d\d)";
            Regex regex = new Regex(RegexString, RegexOptions.Compiled | RegexOptions.IgnoreCase);

            for (int i = 0; i < strs.Length; i++)
            {
                Match m = regex.Match(strs[i]);
                if (!m.Success)
                {
                    continue;
                }

                string CanonicalNumber = "";
                string[] Number = FilterRepeated(m.Value.Trim().Split(' '), 21);

                if (Number.Length < 5)
                {
                    continue;
                }

                for (int j = 0; j < Number.Length; j++)
                {
                    CanonicalNumber += Number[j] + " ";
                }

                Result += CanonicalNumber + "|1\n";
            }

            if (Result.EndsWith("\n"))
                Result = Result.Substring(0, Result.Length - 1);
            return Result;
        }
        private string AnalyseScheme_B_HY5(string Content, int PlayType)
        {
            string[] strs = Content.Split('\n');
            if (strs == null)
                return "";
            if (strs.Length == 0)
                return "";

            string Result = "";

            string RegexString = @"^((\d\d\s){5,20}\d\d)";
            Regex regex = new Regex(RegexString, RegexOptions.Compiled | RegexOptions.IgnoreCase);

            for (int i = 0; i < strs.Length; i++)
            {
                Match m = regex.Match(strs[i]);
                if (!m.Success)
                {
                    continue;
                }

                string CanonicalNumber = "";
                string[] Number = FilterRepeated(m.Value.Trim().Split(' '), 21);

                if (Number.Length < 6)
                {
                    continue;
                }

                for (int j = 0; j < Number.Length; j++)
                {
                    CanonicalNumber += Number[j] + " ";
                }

                Result += CanonicalNumber + "|1\n";
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
            string[] WinLotteryNumber = FilterRepeated(Number.Split(' '), 21);

            if ((WinLotteryNumber == null) || (WinLotteryNumber.Length != 5))
                return false;

            return true;
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
            return base.ShowNumber(Number, "");
        }
    }
    }
}