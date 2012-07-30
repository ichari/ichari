using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace SLS.SilverLight.FilterShrink.Model
{
    static class Result
    {
        public static string Win = "3";//赢
        public static string Level = "1";//平
        public static string Lose = "0";//负
        public static string Default = string.Empty;//缺省值 
    }

    public class Netball
    {
        public int Id { get; set; }//id
        public string Number { get; set; }//场次
        public string HomeField { get; set; }//主场球队
        public int ConcessionBall { get; set; }//让球值
        public string VisitingField { get; set; }//客场球队
        public string FristField { get; set; }//主场结局
        public string NextField { get; set; }//次场结局
        public string LastField { get; set; }//末场结局
        public double FristOdds { get; set; } //主场赔率
        public double NextOdds { get; set; } //次场赔率
        public double LastOdds { get; set; }//末场赔率

        #region 胜
        public double S10 { get; set; }
        public double S20 { get; set; }
        public double S21 { get; set; }
        public double S30 { get; set; }
        public double S31 { get; set; }
        public double S32 { get; set; }
        public double S40 { get; set; }
        public double S41 { get; set; }
        public double S42 { get; set; }
        public double S50 { get; set; }
        public double S51 { get; set; }
        public double S52 { get; set; }
        public double Sother { get; set; }
        #endregion

        #region 平
        public double P00 { get; set; }
        public double P11 { get; set; }
        public double P22 { get; set; }
        public double P33 { get; set; }
        public double Pother { get; set; }
        #endregion

        #region 负
        public double F01 { get; set; }
        public double F02 { get; set; }
        public double F12 { get; set; }
        public double F03 { get; set; }
        public double F13 { get; set; }
        public double F23 { get; set; }
        public double F04 { get; set; }
        public double F14 { get; set; }
        public double F24 { get; set; }
        public double F05 { get; set; }
        public double F15 { get; set; }
        public double F25 { get; set; }
        public double Fother { get; set; }
        #endregion

        #region 总进球的赔率
        public string SimpleNum { get; set; }
        public double In0 { get; set; }
        public double In1 { get; set; }
        public double In2 { get; set; }
        public double In3 { get; set; }
        public double In4 { get; set; }
        public double In5 { get; set; }
        public double In6 { get; set; }
        public double In7 { get; set; }
        #endregion

        #region 半全场的赔率
        public double SS { get; set; }
        public double SP { get; set; }
        public double SF { get; set; }
        public double PS { get; set; }
        public double PP { get; set; }
        public double PF { get; set; }
        public double FS { get; set; }
        public double FP { get; set; }
        public double FF { get; set; }
        #endregion

        public Netball()
        {
            Id = -1;
            Number = "";
            HomeField = "";
            ConcessionBall = 0;
            VisitingField = "";
            FristField = "";
            NextField = "";
            LastField = "";
            FristOdds = 0.00;
            NextOdds = 0.00;
            LastOdds = 0.00;
            S10 = 0.00;
            S20 = 0.00;
            S21 = 0.00;
            S30 = 0.00;
            S31 = 0.00;
            S32 = 0.00;
            S40 = 0.00;
            S41 = 0.00;
            S42 = 0.00;
            S50 = 0.00;
            S51 = 0.00;
            S52 = 0.00;
            Sother = 0.00;

            P00 = 0.00;
            P11 = 0.00;
            P22 = 0.00;
            P33 = 0.00;
            Pother = 0.00;

            F01 = 0.00;
            F02 = 0.00;
            F03 = 0.00;
            F04 = 0.00;
            F05 = 0.00;
            F12 = 0.00;
            F13 = 0.00;
            F14 = 0.00;
            F15 = 0.00;
            F23 = 0.00;
            F24 = 0.00;
            F25 = 0.00;
            Fother = 0.00;

            In0 = 0.00;
            In1 = 0.00;
            In2 = 0.00;
            In3 = 0.00;
            In4 = 0.00;
            In5 = 0.00;
            In6 = 0.00;
            In7 = 0.00;
            SimpleNum = "";

            SS = 0.00;
            SP = 0.00;
            SF = 0.00;
            PS = 0.00;
            PP = 0.00;
            PF = 0.00;
            FS = 0.00;
            FP = 0.00;
            FF = 0.00;
        }

        public Netball(int Id,
                       string Number,
                       string HomeField,
                       int ConcessionBall,
                       string VisitingField,
                       string FristField,
                       string NextField,
                       string LastField,
                       double FristOdds,
                       double NextOdds,
                       double LastOdds,
                       double S10,
                        double S20,
                        double S21,
                        double S30,
                        double S31,
                        double S32,
                        double S40,
                        double S41,
                        double S42,
                        double S50,
                        double S51,
                        double S52,
                        double Sother,
                        double P00,
                        double P11,
                        double P22,
                        double P33,
                        double Pother,
                        double F01,
                        double F02,
                        double F03,
                        double F04,
                        double F05,
                        double F12,
                        double F13,
                        double F14,
                        double F15,
                        double F23,
                        double F24,
                        double F25,
                        double Fother,

                        double In0,
                        double In1,
                        double In2,
                        double In3,
                        double In4,
                        double In5,
                        double In6,
                        double In7,
                        string SimpleNum,

                        double SS,
                        double SP,
                        double SF,
                        double PS,
                        double PP,
                        double PF,
                        double FS,
                        double FP,
                        double FF
            )
        {
            // 在此点下面插入创建对象所需的代码。
            this.Id = Id;
            this.Number = Number;
            this.HomeField = HomeField;
            this.ConcessionBall = ConcessionBall;
            this.VisitingField = VisitingField;
            this.FristField = FristField;
            this.NextField = NextField;
            this.LastField = LastField;
            this.FristOdds = FristOdds;
            this.NextOdds = NextOdds;
            this.LastOdds = LastOdds;

            this.S10 = S10;
            this.S20 = S20;
            this.S21 = S21;
            this.S30 = S30;
            this.S31 = S31;
            this.S32 = S32;
            this.S40 = S40;
            this.S41 = S41;
            this.S42 = S42;
            this.S50 = S50;
            this.S51 = S51;
            this.S52 = S52;
            this.Sother = Sother;

            this.P00 = P00;
            this.P11 = P11;
            this.P22 = P22;
            this.P33 = P33;
            this.Pother = Pother;

            this.F01 = F01;
            this.F02 = F02;
            this.F03 = F03;
            this.F04 = F04;
            this.F05 = F05;
            this.F12 = F12;
            this.F13 = F13;
            this.F14 = F14;
            this.F15 = F15;
            this.F23 = F23;
            this.F24 = F24;
            this.F25 = F25;
            this.Fother = Fother;

            this.In0 = In0;
            this.In1 = In1;
            this.In2 = In2;
            this.In3 = In3;
            this.In4 = In4;
            this.In5 = In5;
            this.In6 = In6;
            this.In7 = In7;
            this.SimpleNum = SimpleNum;

            this.SS = SS;
            this.SP = SP;
            this.SF = SF;
            this.PS = PS;
            this.PP = PP;
            this.PF = PF;
            this.FS = FS;
            this.FP = FP;
            this.FF = FF;
        }

        #region AB玩法几串几,传一个AB/AA/AC……返回几串几
        //AB玩法几串几,传一个AB/AA/AC……返回几串几
        public static string GetGameType(string Num)
        {
            switch (Num)
            {
                case "A0":
                    return "单关";
                case "AA":
                    return "2串1";
                case "AB":
                    return "3串1";
                case "AE":
                    return "4串1";
                case "AJ":
                    return "5串1";
                case "AQ":
                    return "6串1";
                case "BA":
                    return "7串1";
                case "BG":
                    return "8串1";
                default:
                    return "";
            }
        }

        public static string GetBuyWays(int Num)
        {
            switch (Num)
            {
                case 2:
                    return "AA";
                case 3:
                    return "AB";
                case 4:
                    return "AE";
                case 5:
                    return "AJ";
                case 6:
                    return "AQ";
                case 7:
                    return "BA";
                case 8:
                    return "BG";
                default:
                    return "AA";
            }
        }
        #endregion

        //比赛比分信息转换
        public static string GetScoreInfo(string Score)
        {
            switch (Score)
            {
                case "1":
                    return "10";
                case "2":
                    return "20";
                case "3":
                    return "21";
                case "4":
                    return "30";
                case "5":
                    return "31";
                case "6":
                    return "32";
                case "7":
                    return "40";
                case "8":
                    return "41";
                case "9":
                    return "42";
                case "10":
                    return "50";
                case "11":
                    return "51";
                case "12":
                    return "52";
                case "13":
                    return "WW";
                case "14":
                    return "00";
                case "15":
                    return "11";
                case "16":
                    return "22";
                case "17":
                    return "33";
                case "18":
                    return "DD";
                case "19":
                    return "01";
                case "20":
                    return "02";
                case "21":
                    return "12";
                case "22":
                    return "03";
                case "23":
                    return "13";
                case "24":
                    return "23";
                case "25":
                    return "04";
                case "26":
                    return "14";
                case "27":
                    return "24";
                case "28":
                    return "05";
                case "29":
                    return "15";
                case "30":
                    return "25";
                case "31":
                    return "LL";
                default:
                    return "";
            }
        }

        //比赛比分信息转换
        public static string GetLotterNumber(string Score)
        {
            switch (Score)
            {
                case "10":
                    return "1";
                case "20":
                    return "2";
                case "21":
                    return "3";
                case "30":
                    return "4";
                case "31":
                    return "5";
                case "32":
                    return "6";
                case "40":
                    return "7";
                case "41":
                    return "8";
                case "42":
                    return "9";
                case "50":
                    return "10";
                case "51":
                    return "11";
                case "52":
                    return "12";
                case "WW":
                    return "13";
                case "00":
                    return "14";
                case "11":
                    return "15";
                case "22":
                    return "16";
                case "33":
                    return "17";
                case "DD":
                    return "18";
                case "01":
                    return "19";
                case "02":
                    return "20";
                case "12":
                    return "21";
                case "03":
                    return "22";
                case "13":
                    return "23";
                case "23":
                    return "24";
                case "04":
                    return "25";
                case "14":
                    return "26";
                case "24":
                    return "27";
                case "05":
                    return "28";
                case "15":
                    return "29";
                case "25":
                    return "30";
                case "LL":
                    return "31";
                default:
                    return "";
            }
        }

        //进球总数信息转换
        public static string GetSumGoal(string Goal)
        {
            switch (Goal)
            {
                case "1":
                    return "0";
                case "2":
                    return "1";
                case "3":
                    return "2";
                case "4":
                    return "3";
                case "5":
                    return "4";
                case "6":
                    return "5";
                case "7":
                    return "6";
                case "8":
                    return "7";
            }
            return "";
        }

        //进球总数信息转换
        public static string GetSumScore(string Goal)
        {
            switch (Goal)
            {
                case "0":
                    return "1";
                case "1":
                    return "2";
                case "2":
                    return "3";
                case "3":
                    return "4";
                case "4":
                    return "5";
                case "5":
                    return "6";
                case "6":
                    return "7";
                case "7":
                    return "8";
            }
            return "";
        }

        //半全场信息转换
        public static string GetHalfAllField(string Field)
        {
            switch (Field)
            {
                case "1":
                    return "33";
                case "2":
                    return "31";
                case "3":
                    return "30";
                case "4":
                    return "13";
                case "5":
                    return "11";
                case "6":
                    return "10";
                case "7":
                    return "03";
                case "8":
                    return "01";
                case "9":
                    return "00";
                default:
                    return "";

            }
        }

        //半全场信息转换
        public static string GetHalfAllScore(string Score)
        {
            switch (Score)
            {
                case "33":
                    return "1";
                case "31":
                    return "2";
                case "30":
                    return "3";
                case "13":
                    return "4";
                case "11":
                    return "5";
                case "10":
                    return "6";
                case "03":
                    return "7";
                case "01":
                    return "8";
                case "00":
                    return "9";
                default:
                    return "";

            }
        }

        //字符串转换成double
        public static double RetuntDouble(string Num)
        {
            double Results = 0.00;

            double.TryParse(Num, out Results);

            return Results;
        }
    }
}