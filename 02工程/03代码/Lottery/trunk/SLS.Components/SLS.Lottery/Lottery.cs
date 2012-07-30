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
        public class PlayType
        {
            public int ID;
            public string Name;

            public PlayType(int id, string name)
            {
                ID = id;
                Name = name;
            }
        }

        public class Ticket
        {
            public int PlayTypeID;
            public string Number;
            public int Multiple;
            public double Money;

            public Ticket(int playtype_id, string number, int multiple, double money)
            {
                PlayTypeID = playtype_id;
                Multiple = multiple;
                Number = number;
                Money = money;
            }

            public override string ToString()
            {
                return PlayTypeID.ToString() + "," + Multiple.ToString() + "," + Money.ToString() + "," + Number.Replace("\r\n", "\t").Replace("\n", "\t") + ";";
            }
        }

        public class LotteryBase
        {
            public int id;
            public string name;
            public string code;

            #region 虚拟方法
            public virtual bool CheckPlayType(int play_type)
            {
                return false;
            }

            public virtual string BuildNumber(int Num)
            {
                return "";
            }
            public virtual string BuildNumber(int Num, int Type)
            {
                return "";
            }
            public virtual string BuildNumber(int Red, int Blue, int Num)
            {
                return "";
            }

            public virtual string[] ToSingle(string Number, ref string CanonicalNumber, int PlayType)
            {
                return null;
            }

            public virtual double ComputeWin(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, int PlayType, params double[] WinMoneyList)
            {
                return 0;
            }
            public virtual double ComputeWin(string Scheme, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, int PlayType, int CompetitionCount, string NoSignificance)
            {
                return 0;
            }

            public virtual string AnalyseScheme(string Content, int PlayType)
            {
                return "";
            }

            public virtual bool AnalyseWinNumber(string Number)
            {
                return true;
            }
            public virtual bool AnalyseWinNumber(string Number, int CompetitionCount)
            {
                return true;
            }

            public virtual int GetNum(int Number1, int Number2)
            {
                return 0;
            }

            public virtual string ShowNumber(string Number)
            {
                return "";
            }

            public virtual PlayType[] GetPlayTypeList()
            {
                return null;
            }

            public virtual string GetPrintKeyList(string Number, int PlayType_id, string LotteryMachine)
            {
                return "";
            }

            //足彩单场专用
            public virtual bool GetSchemeSplit(string Scheme, ref string BuyContent, ref string CnLocateWaysAndMultiples)
            {
                return true;
            }

            public virtual Ticket[] ToElectronicTicket_HPCQ(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                return null;
            }

            public virtual Ticket[] ToElectronicTicket_HPSH(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                return null;
            }

            public virtual Ticket[] ToElectronicTicket_HPJX(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                return null;
            }

            public virtual Ticket[] ToElectronicTicket_HPSD(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                return null;
            }

            public virtual Ticket[] ToElectronicTicket_DYJ(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                return null;
            }

            public virtual Ticket[] ToElectronicTicket_ZCW(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                return null;
            }

            public virtual Ticket[] ToElectronicTicket_ZZYTC(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                return null;
            }

            public virtual Ticket[] ToElectronicTicket_CTTCSD(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                return null;
            }

            public virtual string HPSH_ToElectronicTicket(int PlayTypeID, string Number, ref string TicketNumber, ref int NewPlayTypeID)
            {
                return "";
            }

            public virtual string HPJX_ToElectronicTicket(int PlayTypeID, string Number, ref string TicketNumber, ref int NewPlayTypeID)
            {
                return "";
            }

            public virtual Ticket[] ToElectronicTicket_XGCQ(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                return null;
            }

            public virtual Ticket[] ToElectronicTicket_XGSH(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                return null;
            }

            public virtual Ticket[] ToElectronicTicket_BJCP(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                return null;
            }

            //北京单场
            public virtual string ToElectronicTicket_BJDC(int PlayTypeID, string Number, ref string TicketNumber, ref int NewPlayTypeID, ref string Rule, ref int Multiple, ref double Money, ref string GameNoList, ref string PassMode, ref int TicketCount)
            {
                return "";
            }

            #endregion

            protected class CompareToAscii : IComparer
            {
                int IComparer.Compare(Object x, Object y)
                {
                    return ((new CaseInsensitiveComparer()).Compare(x, y));
                }
            }

            protected bool isExistBall(ArrayList al, int Ball)
            {
                if (al.Count == 0)
                    return false;
                for (int i = 0; i < al.Count; i++)
                    if (int.Parse(al[i].ToString()) == Ball)
                        return true;
                return false;
            }

            protected string Sort(string str)
            {
                char[] ch = str.ToCharArray();
                Array.Sort(ch, new CompareToAscii());
                return new string(ch);
            }

            protected string[] SplitLotteryNumber(string Number)
            {
                string[] s = Number.Split('\n');
                if (s.Length == 0)
                    return null;
                for (int i = 0; i < s.Length; i++)
                    s[i] = s[i].Trim();
                return s;
            }

            /// <summary>
            /// 合并中奖描述
            /// </summary>
            protected void MergeWinDescription(ref string WinDescription, string AddDescription)
            {
                if (WinDescription != "")
                {
                    WinDescription += "，";
                }

                WinDescription += AddDescription;
            }

            /// <summary>
            /// 过滤掉彩票号前面以 [] 说明玩法的 [] 部分，如：时时乐，时时彩等使用
            /// </summary>
            protected string FilterPreFix(string Number)
            {
                if (!Number.StartsWith("[") && (Number.IndexOf("]") < 0))
                {
                    return Number;
                }

                return Number.Split(']')[1];
            }

            /// <summary>
            /// 获取彩票号前面以 [] 说明玩法的 [] 部分，如：时时乐，时时彩等使用
            /// </summary>
            protected string GetLotteryNumberPreFix(string Number)
            {
                if ((Number == null) || (Number == "") || (!Number.StartsWith("[")))
                {
                    return "";
                }

                return Number.Split(']')[0] + "]";
            }

            /// <summary>
            /// 合并彩票号前面以 [] 说明玩法的 [] 部分，如：时时乐，时时彩等使用
            /// </summary>
            protected string[] MergeLotteryNumberPreFix(string[] Numbers, string PreFix)
            {
                if ((Numbers == null) || (Numbers.Length == 0))
                {
                    return Numbers;
                }

                for (int i = 0; i < Numbers.Length; i++)
                {
                    Numbers[i] = PreFix + Numbers[i];
                }

                return Numbers;
            }

            /// <summary>
            /// 将开奖好按一定的格式输出给表现层,有些彩种会原样输出，有些彩种会增加空格等等。
            /// </summary>
            public string ShowNumber(string Number, string SpaceMark)
            {
                if (SpaceMark == "")
                    return Number;

                Number = Number.Replace(" ", "");
                string Result = "";
                for (int i = 0; i < Number.Length; i++)
                    Result += Number[i].ToString() + SpaceMark;

                return Result.Trim();
            }

        }

        public LotteryBase this[int Index]
        {
            get
            {
                switch (Index)
                {
                    case 1:
                        return new SFC();
                    case 2:
                        return new JQC();
                    case 3:
                        return new QXC();
                    case 4:
                        return new SZPL();
                    case 5:
                        return new SSQ();
                    case 6:
                        return new FC3D();
                    case 7:
                        return new LJ36X7();
                    case 8:
                        return new LJP62();
                    case 9:
                        return new TC22X5();
                    case 10:
                        return new FZ36X7();
                    case 11:
                        return new CTFC32X7();
                    case 12:
                        return new CTFC22X5();
                    case 13:
                        return new QLC();
                    case 14:
                        return new TC29X7();
                    case 15:
                        return new LCBQC();
                    case 16:
                        return new NYFC36X7();
                    case 17:
                        return new NYFC26X5();
                    case 18:
                        return new SJFC21X5();
                    case 19:
                        return new LCDC();
                    case 20:
                        return new SZFC35X7();
                    case 21:
                        return new ZJ15X5();
                    case 22:
                        return new ZJFC4J1();
                    case 23:
                        return new HNFC22X5();
                    case 24:
                        return new DFDLT();
                    case 25:
                        return new AHFC25X5();
                    case 26:
                        return new AHFC15X5();
                    case 27:
                        return new QLFC23X5();
                    case 28:
                        return new CQSSC();
                    case 29:
                        return new SHSSL();
                    case 30:
                        return new FJFC20X5();
                    case 31:
                        return new AHFC5WS();
                    case 32:
                        return new SZKL8();
                    case 33:
                        return new BJKL8();
                    case 34:
                        return new SHKENO();
                    case 35:
                        return new FJTC31X7();
                    case 36:
                        return new FJTC36X7();
                    case 37:
                        return new FJTC22X5();
                    case 38:
                        return new LNFC35X7();
                    case 39:
                        return new TCCJDLT();
                    case 40:
                        return new ZJTC20X5();
                    case 41:
                        return new ZJTC6J1();
                    case 42:
                        return new LJFC22X5();
                    case 43:
                        return new LJTC6J1();
                    case 44:
                        return new TTL22X5();
                    case 45:
                        return new ZCDC();
                    case 46:
                        return new TJFC15X5();
                    case 47:
                        return new LNFC25X4();
                    case 48:
                        return new HBKLPK();
                    case 49:
                        return new SDKLPK();
                    case 50:
                        return new HeBKLPK();
                    case 51:
                        return new AHKLPK();
                    case 52:
                        return new HLJKLPK();
                    case 53:
                        return new LLKLPK();
                    case 54:
                        return new SXKLPK();
                    case 55:
                        return new ZJKLPK();
                    case 56:
                        return new SCKLPK();
                    case 57:
                        return new ShXKLPK();
                    case 58:
                        return new DF6J1();
                    case 59:
                        return new HD15X5();
                    case 60:
                        return new TTCX4();
                    case 61:
                        return new JXSSC();
                    case 62:
                        return new SYYDJ();
                    case 63:
                        return new SZPL3();
                    case 64:
                        return new SZPL5();
                    case 65:
                        return new TC31X7();
                    case 66:
                        return new XJSSC();
                    case 67:
                        return new JXFC3D();
                    case 68:
                        return new HNKY481();
                    case 69:
                        return new ZYFC22X5();
                    case 70:
                        return new JX11X5();
                    case 71:
                        return new SDQYH();
                    case 72:
                        return new JCZQ();
                    case 73:
                        return new JCLQ();
                    case 74:
                        return new ZCSFC();
                    case 75:
                        return new ZCRJC();
                    case 76:
                        return new GXKLSF();
                    case 77:
                        return new HN11X5();
                    case 78:
                        return new GD11X5();
                    case 79:
                        return new NMGSSC();
                    case 80:
                        return new HC1();
                    case 81:
                        return new GX11X5();
                }
                return null;
            }
        }

        public LotteryBase this[string Name_or_Code_or_ID]
        {
            get
            {
                LotteryBase[] lotterys = GetLotterys();

                foreach (LotteryBase lottery in lotterys)
                {
                    if ((lottery.name == Name_or_Code_or_ID) || (lottery.code == Name_or_Code_or_ID) || (lottery.id.ToString() == Name_or_Code_or_ID))
                    {
                        return lottery;
                    }
                }

                return null;
            }
        }

        public LotteryBase[] GetLotterys()
        {
            int LotteryCount = 1;

            while (this[LotteryCount] != null)
            {
                LotteryCount++;
            }

            LotteryBase[] Lotterys = new LotteryBase[LotteryCount - 1];

            for (int i = 0; i < Lotterys.Length; i++)
            {
                Lotterys[i] = this[i + 1];
            }

            return Lotterys;
        }

        public string GetPlayTypeName(int PlayType)
        {
            LotteryBase[] lotterys = GetLotterys();

            foreach (LotteryBase lb in lotterys)
            {
                PlayType[] lbts = lb.GetPlayTypeList();

                foreach (PlayType lbt in lbts)
                {
                    if (lbt.ID == PlayType)
                    {
                        return lbt.Name;
                    }
                }
            }

            return "";
        }

        public int GetMaxLotteryID()
        {
            return this[GetLotterys().Length].id;
        }

        public bool ValidID(int LotteryID)
        {
            if ((LotteryID < 1) || (LotteryID > GetMaxLotteryID()))
            {
                return false;
            }

            return true;
        }
    }
}