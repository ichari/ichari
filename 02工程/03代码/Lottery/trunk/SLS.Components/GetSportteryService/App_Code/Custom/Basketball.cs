using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using GetSportteryService;

namespace GetSporttery
{
    class Basketball
    {
        public Basketball()
        {

        }

        GetSportteryService.Log log = new GetSportteryService.Log("Sporttery");

        #region 生成SQL命令

        /// <summary>
        /// 胜负过关奖金
        /// </summary>
        /// <param name="Html"></param>
        /// <param name="Count"></param>
        /// <returns></returns>
        public string Getmnl_list(string Html, out int Count)
        {
            int IStart = 0;
            int ILen = 0;
            Count = 0;

            string[] HtmlTr = null;
            string[] HtmlTd = null;
            string id = string.Empty;
            string MainWin = string.Empty;
            string Mainlose = string.Empty;
            string StopSellTime = string.Empty;
            string MatchNumber = string.Empty;
            string Game = string.Empty;
            string MainTeam = string.Empty;
            string GuestTeam = string.Empty;

            StringBuilder sb = new StringBuilder();
            sb.Append(" delete T_PassRateBasket where StopSellTime<GETDATE(); update T_PassRateBasket set IsMnl = 0 ; ");

            Other.GetStrScope(Html, "<DIV CLASS=\"box-tbl-a\">", "</TABLE>", out IStart, out ILen);

            if (ILen == 0)
            {
                return "";
            }

            Html = Html.Substring(IStart, ILen);

            HtmlTr = SplitString(Html, "<TR>");

            for (int i = 1; i < HtmlTr.Length; i++)
            {
                HtmlTd = SplitString(HtmlTr[i], "</TD>");
                if (HtmlTd.Length < 5)
                    continue;
                MatchNumber = HtmlTd[0].Split('>')[1];
                Game = HtmlTd[1].Split('>')[1];
                GuestTeam = HtmlTd[2].Split('>')[2].Split('<')[0];
                MainTeam = HtmlTd[2].Split('>')[4].Split('<')[0];
                id = SplitString(HtmlTd[2], "m=")[1].Split('"')[0];
                StopSellTime = HtmlTd[3].Split('>')[1];

                int Month = Shove._Convert.StrToInt(StopSellTime.Split('-')[1], 0);
                string OldYear = StopSellTime.Split('-')[0];
                int Year = DateTime.Now.Year;

                if (Month < DateTime.Now.Month)
                {
                    Year = Year + 1;
                }

                StopSellTime = StopSellTime.Replace(OldYear + "-" + Month.ToString().PadLeft(2, '0') + "-", Year.ToString() +"-" + Month.ToString().PadLeft(2, '0') + "-") + ":00";

                Mainlose = HtmlTd[4].Split('>')[2];
                MainWin = HtmlTd[5].Split('>')[2];
                sb.Append(" exec P_BasketSfgg '" + MainTeam + "','" + GuestTeam + "','" + MatchNumber + "','" + Game + "','" + StopSellTime + "'," + MainWin + "," + Mainlose + ", 0, '';");
            }
            
            return sb.ToString();
        }

        /// <summary>
        /// 得到过关让分胜负
        /// </summary>
        /// <param name="Html"></param>
        /// <param name="Count"></param>
        /// <returns></returns>
        public string Gethdc_list(string Html, out int Count)
        {
            int IStart = 0;
            int ILen = 0;
            Count = 0;

            string[] HtmlTr = null;
            string[] HtmlTd = null;

            StringBuilder sb = new StringBuilder();
            sb.Append(" delete T_PassRateBasket where StopSellTime<GETDATE(); update T_PassRateBasket set IsHdc = 0 ; ");

            string LetMainLose = string.Empty;
            string LetMainWin = string.Empty;
            string letscore = string.Empty;
            string StopSellTime = string.Empty;
            string MatchNumber = string.Empty;
            string Game = string.Empty;
            string MainTeam = string.Empty;
            string GuestTeam = string.Empty;

            Other.GetStrScope(Html, "<DIV CLASS=\"box-tbl-a\">", "</TABLE", out IStart, out ILen);

            Html = Html.Substring(IStart, ILen);

            HtmlTr = SplitString(Html, "<TR>");
            for (int i = 1; i < HtmlTr.Length; i++)
            {
                HtmlTd = SplitString(HtmlTr[i], "</TD>");
                if (HtmlTd.Length < 5)
                    continue;

                MatchNumber = HtmlTd[0].Split('>')[1];
                Game = HtmlTd[1].Split('>')[1];
                GuestTeam = HtmlTd[2].Split('>')[2].Split('<')[0];

                if (GuestTeam.IndexOf("(") >= 0)
                {
                    GuestTeam = GuestTeam.Substring(0, GuestTeam.IndexOf("("));
                }

                MainTeam = HtmlTd[2].Split('>')[4].Split('<')[0];

                if (MainTeam.IndexOf("(") >= 0)
                {
                    MainTeam = MainTeam.Substring(0, MainTeam.IndexOf("("));
                }

                StopSellTime = HtmlTd[3].Split('>')[1];
                int Month = Shove._Convert.StrToInt(StopSellTime.Split('-')[1], 0);
                string OldYear = StopSellTime.Split('-')[0];
                int Year = DateTime.Now.Year;

                if (Month < DateTime.Now.Month)
                {
                    Year = Year + 1;
                }

                StopSellTime = StopSellTime.Replace(OldYear + "-" + Month.ToString().PadLeft(2, '0') + "-", Year.ToString() + "-" + Month.ToString().PadLeft(2, '0') + "-") + ":00";

                letscore = HtmlTd[2].Split('(')[1].Split(')')[0].Replace("+", "");
                LetMainLose = HtmlTd[4].Split('>')[2];
                LetMainWin = HtmlTd[5].Split('>')[2];

                sb.Append(" exec P_BasketRfSfgg '" + MainTeam + "','" + GuestTeam + "','" + MatchNumber + "','" + Game + "','" + StopSellTime + "'," + LetMainLose + "," + LetMainWin + "," + letscore + ",0, '';");

                sb.Append(" Update T_MatchBasket set Givewinlosescore = '" + letscore + "' where ID = (select MatchID from T_PassRateBasket where MatchNumber = '" + MatchNumber + "')");
            }
            return sb.ToString();
        }

        /// <summary>
        /// 得到过关胜分差
        /// </summary>
        /// <param name="Html"></param>
        /// <param name="Count"></param>
        /// <returns></returns>
        public string Getwnm_list(string Html, out int Count)
        {
            int IStart = 0;
            int ILen = 0;
            Count = 0;

            string[] HtmlTr = null;
            string[] HtmlTd = null;
            StringBuilder sb = new StringBuilder();
            sb.Append(" delete T_PassRateBasket where StopSellTime<GETDATE(); update T_PassRateBasket set IsWnm = 0 ; ");

            string DifferGuest1_5 = string.Empty;
            string DifferGuest6_10 = string.Empty;
            string DifferGuest11_15 = string.Empty;
            string DifferGuest16_20 = string.Empty;
            string DifferGuest21_25 = string.Empty;
            string DifferGuest26 = string.Empty;
            string DifferMain1_5 = string.Empty;
            string DifferMain6_10 = string.Empty;
            string DifferMain11_15 = string.Empty;
            string DifferMain16_20 = string.Empty;
            string DifferMain21_25 = string.Empty;
            string DifferMain26 = string.Empty;
            string StopSellTime = string.Empty;
            string MatchNumber = string.Empty;
            string Game = string.Empty;
            string MainTeam = string.Empty;
            string GuestTeam = string.Empty;

            Other.GetStrScope(Html, "", "", out IStart, out ILen);
            Other.GetStrScope(Html, "<DIV CLASS=\"box-tbl-a\">", "</TABLE", out IStart, out ILen);

            Html = Html.Substring(IStart, ILen);

            HtmlTr = SplitString(Html, "<TR>");
            for (int i = 1; i < HtmlTr.Length; i++)
            {
                HtmlTd = SplitString(HtmlTr[i], "</TD>");
                if (HtmlTd.Length < 10)
                    continue;

                MatchNumber = HtmlTd[0].Split('>')[1];
                Game = HtmlTd[1].Split('>')[1];
                GuestTeam = HtmlTd[2].Split('>')[2].Split('<')[0];
                MainTeam = HtmlTd[2].Split('>')[5].Split('<')[0];
                StopSellTime = HtmlTd[3].Split('>')[1];
                int Month = Shove._Convert.StrToInt(StopSellTime.Split('-')[1], 0);
                string OldYear = StopSellTime.Split('-')[0];
                int Year = DateTime.Now.Year;

                if (Month < DateTime.Now.Month)
                {
                    Year = Year + 1;
                }

                StopSellTime = StopSellTime.Replace(OldYear + "-" + Month.ToString().PadLeft(2, '0') + "-", Year.ToString() + "-" + Month.ToString().PadLeft(2, '0') + "-") + ":00";

                DifferGuest1_5 = HtmlTd[5].Split('>')[2].Split('<')[0];
                DifferMain1_5 = HtmlTd[5].Split('>')[4];
                DifferGuest6_10 = HtmlTd[6].Split('>')[2].Split('<')[0];
                DifferMain6_10 = HtmlTd[6].Split('>')[4];
                DifferGuest11_15 = HtmlTd[7].Split('>')[2].Split('<')[0];
                DifferMain11_15 = HtmlTd[7].Split('>')[4];
                DifferGuest16_20 = HtmlTd[8].Split('>')[2].Split('<')[0];
                DifferMain16_20 = HtmlTd[8].Split('>')[4];
                DifferGuest21_25 = HtmlTd[9].Split('>')[2].Split('<')[0];
                DifferMain21_25 = HtmlTd[9].Split('>')[4];
                DifferGuest26 = HtmlTd[10].Split('>')[2].Split('<')[0];
                DifferMain26 = HtmlTd[10].Split('>')[4];

                sb.Append(" exec P_BasketSfcgg '" + MainTeam + "','" + GuestTeam + "','" + MatchNumber + "','" + Game + "','" + StopSellTime + "'," + DifferGuest1_5 + "," + DifferGuest6_10 + "," + DifferGuest11_15 + "," + DifferGuest16_20 + "," + DifferGuest21_25 + "," + DifferGuest26 + ",");
                sb.Append(DifferMain1_5 + "," + DifferMain6_10 + "," + DifferMain11_15 + "," + DifferMain16_20 + "," + DifferMain21_25 + "," + DifferMain26 + ", 0, '';");
                Count++;
            }

            return sb.ToString();
        }

        /// <summary>
        /// 得到大小分
        /// </summary>
        /// <param name="Html"></param>
        /// <param name="Count"></param>
        /// <returns></returns>
        public string Gethilo_list(string Html, out int Count)
        {
            int IStart = 0;
            int ILen = 0;
            Count = 0;

            string[] HtmlTr = null;
            string[] HtmlTd = null;
            StringBuilder sb = new StringBuilder();

            string id = string.Empty;
            string Big = string.Empty;
            string Small = string.Empty;
            string BigSmallscore = string.Empty;

            string StopSellTime = string.Empty;
            string MatchNumber = string.Empty;
            string Game = string.Empty;
            string MainTeam = string.Empty;
            string GuestTeam = string.Empty;

            Other.GetStrScope(Html, "<DIV CLASS=\"box-tbl-a\">", "</TABLE", out IStart, out ILen);

            Html = Html.Substring(IStart, ILen);

            HtmlTr = SplitString(Html, "<TR>");
            for (int i = 1; i < HtmlTr.Length; i++)
            {
                HtmlTd = SplitString(HtmlTr[i], "</TD>");
                if (HtmlTd.Length < 5)
                    continue;

                MatchNumber = HtmlTd[0].Split('>')[1];
                Game = HtmlTd[1].Split('>')[1];
                GuestTeam = GetString(HtmlTd[2].Split('>')[2].Split('<')[0]);
                MainTeam = GetString(HtmlTd[2].Split('>')[4].Split('<')[0]);
                StopSellTime = HtmlTd[3].Split('>')[1];
                int Month = Shove._Convert.StrToInt(StopSellTime.Split('-')[1], 0);
                string OldYear = StopSellTime.Split('-')[0];
                int Year = DateTime.Now.Year;

                if (Month < DateTime.Now.Month)
                {
                    Year = Year + 1;
                }

                StopSellTime = StopSellTime.Replace(OldYear + "-" + Month.ToString().PadLeft(2, '0') + "-", Year.ToString() + "-" + Month.ToString().PadLeft(2, '0') + "-") + ":00";

                BigSmallscore = Regex.Match(HtmlTd[2], @"[\d.]*</A>", RegexOptions.IgnoreCase).Value.Replace("</A>", "");
                Big = HtmlTd[4].Split('>')[2];
                Small = HtmlTd[5].Split('>')[2];

                sb.Append(" exec P_BasketDXFgg '" + MainTeam + "','" + GuestTeam + "','" + MatchNumber + "','" + Game + "','" + StopSellTime + "'," + Big + "," + Small + "," + BigSmallscore + ", 0 ,'';");
                sb.Append(" Update T_MatchBasket set bigsmallscore = '" + BigSmallscore + "' where ID = (select MatchID from T_PassRateBasket where MatchNumber = '" + MatchNumber + "')");
                Count++;
            }
            return sb.ToString();
        }

        /// <summary>
        /// 胜负过关奖金
        /// </summary>
        /// <param name="Html"></param>
        /// <param name="Count"></param>
        /// <returns></returns>
        public string Get500Wanmnl_list(string Html, out int Count)
        {
            int IStart = 0;
            int ILen = 0;
            Count = 0;

            string MainWin = string.Empty;
            string Mainlose = string.Empty;
            string MatchDate = string.Empty;
            string StopSellTime = string.Empty;
            string MatchNumber = string.Empty;
            string Game = string.Empty;
            string MainTeam = string.Empty;
            string GuestTeam = string.Empty;

            StringBuilder sb = new StringBuilder();
            sb.Append(" delete T_PassRateBasket where StopSellTime<GETDATE(); update T_PassRateBasket set IsMnl = 0 ; ");

            Other.GetStrScope(Html, "<DIV CLASS=\"dc_hs\"", "<DIV CLASS=\"dc_r\"", out IStart, out ILen);

            if (ILen == 0)
            {
                return "";
            }

            Html = Html.Substring(IStart, ILen);

            string[] days = Html.Split(new string[] { "<DIV CLASS=\"dc_hs\"" }, StringSplitOptions.RemoveEmptyEntries);
            string[] rows = null;
            string[] tds = null;

            Clutch clu = new Clutch();

            for (int i = 0; i < days.Length; i++)
            {
                rows = days[i].Split(new string[] { "</TR>" }, StringSplitOptions.RemoveEmptyEntries);

                for (int j = 0; j < rows.Length; j++)
                {
                    tds = rows[j].Split(new string[] { "</TD>" }, StringSplitOptions.RemoveEmptyEntries);
                    if (tds.Length == 13)
                    {
                        MatchNumber = clu.getHtmlText(tds[0], 2);
                        Game = clu.getHtmlText(tds[1], 2);
                        MainTeam = clu.getHtmlText(tds[5], 4);
                        GuestTeam = clu.getHtmlText(tds[3], 2);
                        StopSellTime = GetDateTime(tds[2].Substring(tds[2].IndexOf("match_time"))) + ":00";
                        MatchDate = GetDateTime(tds[2]) + ":00";

                        MainWin = clu.getHtmlText(tds[10], 2) == "" ? "0" : clu.getHtmlText(tds[10], 2);
                        Mainlose = clu.getHtmlText(tds[9], 2) == "" ? "0" : clu.getHtmlText(tds[9], 2);

                        sb.Append(" exec P_BasketSfgg '" + MainTeam + "','" + GuestTeam + "','" + MatchNumber + "','" + Game + "','" + MatchDate + "','" + StopSellTime + "'," + MainWin + "," + Mainlose + ", 0, '';");
                    }
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// 得到过关让分胜负
        /// </summary>
        /// <param name="Html"></param>
        /// <param name="Count"></param>
        /// <returns></returns>
        public string Get500Wanhdc_list(string Html, out int Count)
        {
            int IStart = 0;
            int ILen = 0;
            Count = 0;

            StringBuilder sb = new StringBuilder();
            sb.Append(" delete T_PassRateBasket where StopSellTime<GETDATE(); update T_PassRateBasket set IsHdc = 0 ; ");

            Other.GetStrScope(Html, "<DIV CLASS=\"dc_hs\"", "<DIV CLASS=\"dc_r\"", out IStart, out ILen);
            Html = Html.Substring(IStart, ILen);

            string LetMainLose = string.Empty;
            string LetMainWin = string.Empty;
            string letscore = string.Empty;
            string MatchDate = string.Empty;
            string StopSellTime = string.Empty;
            string MatchNumber = string.Empty;
            string Game = string.Empty;
            string MainTeam = string.Empty;
            string GuestTeam = string.Empty;

            string[] days = Html.Split(new string[] { "<DIV CLASS=\"dc_hs\"" }, StringSplitOptions.RemoveEmptyEntries);
            string[] rows = null;
            string[] tds = null;

            Clutch clu = new Clutch();

            for (int i = 0; i < days.Length; i++)
            {
                rows = days[i].Split(new string[] { "</TR>" }, StringSplitOptions.RemoveEmptyEntries);

                for (int j = 0; j < rows.Length; j++)
                {
                    tds = rows[j].Split(new string[] { "</TD>" }, StringSplitOptions.RemoveEmptyEntries);
                    if (tds.Length == 13)
                    {
                        MatchNumber = clu.getHtmlText(tds[0], 2);
                        Game = clu.getHtmlText(tds[1], 2);
                        MainTeam = clu.getHtmlText(tds[5], 4);
                        GuestTeam = clu.getHtmlText(tds[3], 2);

                        StopSellTime = GetDateTime(tds[2].Substring(tds[2].IndexOf("match_time"))) + ":00";
                        MatchDate = GetDateTime(tds[2]) + ":00";

                        letscore = clu.getHtmlText(tds[4], 4);
                        LetMainLose = clu.getHtmlText(tds[9], 2) == "" ? "0" : clu.getHtmlText(tds[9], 2);
                        LetMainWin = clu.getHtmlText(tds[10], 2) == "" ? "0" : clu.getHtmlText(tds[10], 2);

                        sb.Append(" exec P_BasketRfSfgg '" + MainTeam + "','" + GuestTeam + "','" + MatchNumber + "','" + Game + "','" + MatchDate + "','" + StopSellTime + "'," + LetMainLose + "," + LetMainWin + ",'" + letscore + "',0, '';");

                        sb.Append("update T_PassRateBasket set LetScore= '" + letscore + "' where MatchNumber= '" + MatchNumber + "' ; ");
                        sb.Append(" Update T_MatchBasket set Givewinlosescore = '" + letscore + "' where ID = (select MatchID from T_PassRateBasket where MatchNumber = '" + MatchNumber + "')");
                    }
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// 得到过关胜分差
        /// </summary>
        /// <param name="Html"></param>
        /// <param name="Count"></param>
        /// <returns></returns>
        public string Get500Wanwnm_list(string Html, out int Count)
        {
            int IStart = 0;
            int ILen = 0;
            Count = 0;

            StringBuilder sb = new StringBuilder();
            sb.Append(" delete T_PassRateBasket where StopSellTime<GETDATE(); update T_PassRateBasket set IsWnm = 0 ; ");

            string DifferGuest1_5 = string.Empty;
            string DifferGuest6_10 = string.Empty;
            string DifferGuest11_15 = string.Empty;
            string DifferGuest16_20 = string.Empty;
            string DifferGuest21_25 = string.Empty;
            string DifferGuest26 = string.Empty;
            string DifferMain1_5 = string.Empty;
            string DifferMain6_10 = string.Empty;
            string DifferMain11_15 = string.Empty;
            string DifferMain16_20 = string.Empty;
            string DifferMain21_25 = string.Empty;
            string DifferMain26 = string.Empty;
            string MatchDate = string.Empty;
            string StopSellTime = string.Empty;
            string MatchNumber = string.Empty;
            string Game = string.Empty;
            string MainTeam = string.Empty;
            string GuestTeam = string.Empty;

            Other.GetStrScope(Html, "<DIV CLASS=\"dc_hs\"", "<DIV CLASS=\"dc_r\"", out IStart, out ILen);

            Html = Html.Substring(IStart, ILen);

            string[] days = Html.Split(new string[] { "<DIV CLASS=\"dc_hs\"" }, StringSplitOptions.RemoveEmptyEntries);
            string[] rows = null;
            string[] rows_1 = null;
            string[] tds = null;
            string[] tds_1 = null;

            Clutch clu = new Clutch();

            for (int i = 0; i < days.Length; i++)
            {
                rows = Html.Split(new string[] { "ZID" }, StringSplitOptions.RemoveEmptyEntries);

                for (int j = 0; j < rows.Length; j++)
                {
                    tds = rows[j].Split(new string[] { "</TD>" }, StringSplitOptions.RemoveEmptyEntries);
                    if (tds.Length == 14)
                    {
                        rows_1 = rows[j + 1].Split(new string[] { "</TR>" }, StringSplitOptions.RemoveEmptyEntries);
                        tds_1 = rows_1[0].Split(new string[] { "</TD>" }, StringSplitOptions.RemoveEmptyEntries);

                        MatchNumber = clu.getHtmlText(tds[0], 2);
                        Game = clu.getHtmlText(tds[1], 2);
                        MainTeam = clu.getHtmlText(tds_1[0], 4);
                        GuestTeam = clu.getHtmlText(tds[3], 4);
                        StopSellTime = GetDateTime(tds[2].Substring(tds[2].IndexOf("match_time"))) + ":00";
                        MatchDate = GetDateTime(tds[2]) + ":00";

                        DifferGuest1_5 = clu.getHtmlText(tds[7], 2) == "" ? "0" : clu.getHtmlText(tds[7], 2);
                        DifferMain1_5 = clu.getHtmlText(tds_1[2], 2) == "" ? "0" : clu.getHtmlText(tds_1[2], 2);
                        DifferGuest6_10 = clu.getHtmlText(tds[8], 2) == "" ? "0" : clu.getHtmlText(tds[8], 2);
                        DifferMain6_10 = clu.getHtmlText(tds_1[3], 2) == "" ? "0" : clu.getHtmlText(tds_1[3], 2);
                        DifferGuest11_15 = clu.getHtmlText(tds[9], 2) == "" ? "0" : clu.getHtmlText(tds[9], 2);
                        DifferMain11_15 = clu.getHtmlText(tds_1[4], 2) == "" ? "0" : clu.getHtmlText(tds_1[4], 2);
                        DifferGuest16_20 = clu.getHtmlText(tds[10], 2) == "" ? "0" : clu.getHtmlText(tds[10], 2);
                        DifferMain16_20 = clu.getHtmlText(tds_1[5], 2) == "" ? "0" : clu.getHtmlText(tds_1[5], 2);
                        DifferGuest21_25 = clu.getHtmlText(tds[11], 2) == "" ? "0" : clu.getHtmlText(tds[11], 2);
                        DifferMain21_25 = clu.getHtmlText(tds_1[6], 2) == "" ? "0" : clu.getHtmlText(tds_1[6], 2);
                        DifferGuest26 = clu.getHtmlText(tds[12], 2) == "" ? "0" : clu.getHtmlText(tds[12], 2);
                        DifferMain26 = clu.getHtmlText(tds_1[7], 2) == "" ? "0" : clu.getHtmlText(tds_1[7], 2);

                        sb.Append(" exec P_BasketSfcgg '" + MainTeam + "','" + GuestTeam + "','" + MatchNumber + "','" + Game + "','" + MatchDate + "','" + StopSellTime + "'," + DifferGuest1_5 + "," + DifferGuest6_10 + "," + DifferGuest11_15 + "," + DifferGuest16_20 + "," + DifferGuest21_25 + "," + DifferGuest26 + ",");
                        sb.Append(DifferMain1_5 + "," + DifferMain6_10 + "," + DifferMain11_15 + "," + DifferMain16_20 + "," + DifferMain21_25 + "," + DifferMain26 + ", 0, '';");
                    }
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// 得到大小分
        /// </summary>
        /// <param name="Html"></param>
        /// <param name="Count"></param>
        /// <returns></returns>
        public string Get500Wanhilo_list(string Html, out int Count)
        {
            int IStart = 0;
            int ILen = 0;
            Count = 0;

            string MainWin = string.Empty;
            string Mainlose = string.Empty;
            string MatchDate = string.Empty;
            string StopSellTime = string.Empty;
            string MatchNumber = string.Empty;
            string Game = string.Empty;
            string MainTeam = string.Empty;
            string GuestTeam = string.Empty;
            string Big = string.Empty;
            string Small = string.Empty;
            string BigSmallscore = string.Empty;

            StringBuilder sb = new StringBuilder();

            Other.GetStrScope(Html, "<DIV CLASS=\"dc_hs\"", "<DIV CLASS=\"dc_r\"", out IStart, out ILen);

            if (ILen == 0)
            {
                return "";
            }

            Html = Html.Substring(IStart, ILen);

            string[] days = Html.Split(new string[] { "<DIV CLASS=\"dc_hs\"" }, StringSplitOptions.RemoveEmptyEntries);
            string[] rows = null;
            string[] tds = null;
            string date = string.Empty;

            Clutch clu = new Clutch();

            for (int i = 0; i < days.Length; i++)
            {
                rows = days[i].Split(new string[] { "</TR>" }, StringSplitOptions.RemoveEmptyEntries);
                date = clu.GetDate(rows[0]);

                for (int j = 0; j < rows.Length; j++)
                {
                    tds = rows[j].Split(new string[] { "</TD>" }, StringSplitOptions.RemoveEmptyEntries);
                    if (tds.Length == 13)
                    {
                        MatchNumber = clu.getHtmlText(tds[0], 2);
                        Game = clu.getHtmlText(tds[1], 2);
                        MainTeam = clu.getHtmlText(tds[5], 4);
                        GuestTeam = clu.getHtmlText(tds[3], 2);
                        StopSellTime = GetDateTime(tds[2].Substring(tds[2].IndexOf("match_time"))) + ":00";
                        MatchDate = GetDateTime(tds[2]) + ":00";

                        Big = clu.getHtmlText(tds[9], 2) == "" ? "0" : clu.getHtmlText(tds[9], 2);
                        Small = clu.getHtmlText(tds[10], 2) == "" ? "0" : clu.getHtmlText(tds[10], 2);
                        BigSmallscore = clu.getHtmlText(tds[4], 2) == "" ? "0" : clu.getHtmlText(tds[4], 2);


                        sb.Append(" exec P_BasketDXFgg '" + MainTeam + "','" + GuestTeam + "','" + MatchNumber + "','" + Game + "','" + MatchDate + "','" + StopSellTime + "'," + Big + "," + Small + "," + BigSmallscore + ", 0 ,'';");
                        sb.Append(" Update T_MatchBasket set bigsmallscore = '" + BigSmallscore + "' where ID = (select MatchID from T_PassRateBasket where MatchNumber = '" + MatchNumber + "')");
                    }
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// 单场胜负过关奖金
        /// </summary>
        /// <param name="Html"></param>
        /// <param name="Count"></param>
        /// <returns></returns>
        public string Getmnl_vp(string Html, out int Count)
        {
            int IStart = 0;
            int ILen = 0;
            Count = 0;

            string[] HtmlTr = null;
            string[] HtmlTd = null;
            string id = string.Empty;
            string MainWin = string.Empty;
            string Mainlose = string.Empty;
            string StopSellTime = string.Empty;
            string MatchNumber = string.Empty;
            string Game = string.Empty;
            string MainTeam = string.Empty;
            string GuestTeam = string.Empty;
            StringBuilder sb = new StringBuilder();

            sb.Append(" delete T_SingleRateBasket where StopSellTime<GETDATE(); update T_SingleRateBasket set IsMnl = 0 ; ");

            Other.GetStrScope(Html, "<DIV CLASS=\"box-tbl\">", "</TABLE>", out IStart, out ILen);

            if (ILen == 0)
            {
                return "";
            }

            Html = Html.Substring(IStart, ILen);

            HtmlTr = SplitString(Html, "<TR>");

            for (int i = 1; i < HtmlTr.Length; i++)
            {
                HtmlTd = SplitString(HtmlTr[i], "</TD>");
                if (HtmlTd.Length < 5)
                    continue;
                MatchNumber = HtmlTd[0].Split('>')[1];
                Game = HtmlTd[1].Split('>')[1];
                GuestTeam = HtmlTd[2].Split('>')[2].Split('<')[0];
                MainTeam = HtmlTd[2].Split('>')[4].Split('<')[0];
                StopSellTime = HtmlTd[3].Split('>')[1];
                int Month = Shove._Convert.StrToInt(StopSellTime.Split('-')[1], 0);
                string OldYear = StopSellTime.Split('-')[0];
                int Year = DateTime.Now.Year;

                if (Month < DateTime.Now.Month)
                {
                    Year = Year + 1;
                }

                StopSellTime = StopSellTime.Replace(OldYear + "-" + Month.ToString().PadLeft(2, '0') + "-", Year.ToString() + "-" + Month.ToString().PadLeft(2, '0') + "-") + ":00";
                Mainlose = GetSingleRate(HtmlTd[4]);
                MainWin = GetSingleRate(HtmlTd[5]);
                sb.Append(" exec P_BasketSfdg '" + MainTeam + "','" + GuestTeam + "','" + MatchNumber + "','" + Game + "','" + StopSellTime + "'," + MainWin + "," + Mainlose + ", 0, '';");
                Count++;
            }

            return sb.ToString();
        }

        /// <summary>
        /// 得到单场让分胜负
        /// </summary>
        /// <param name="Html"></param>
        /// <param name="Count"></param>
        /// <returns></returns>
        public string Gethdc_vp(string Html, out int Count)
        {
            int IStart = 0;
            int ILen = 0;
            Count = 0;

            string[] HtmlTr = null;
            string[] HtmlTd = null;
            StringBuilder sb = new StringBuilder();
            sb.Append("update T_SingleRateBasket set IsHdc = 0 ; ");
            string id = string.Empty;
            string LetMainLose = string.Empty;
            string LetMainWin = string.Empty;
            string letscore = string.Empty;
            string LetScoreMainLose = string.Empty;
            string LetScoreMainWin = string.Empty;
            string StopSellTime = string.Empty;
            string MatchNumber = string.Empty;
            string Game = string.Empty;
            string MainTeam = string.Empty;
            string GuestTeam = string.Empty;

            Other.GetStrScope(Html, "<DIV CLASS=\"box-tbl\">", "</TABLE", out IStart, out ILen);

            if (ILen == 0)
            {
                return "";
            }

            Html = Html.Substring(IStart, ILen);

            HtmlTr = SplitString(Html, "<TR>");
            for (int i = 1; i < HtmlTr.Length; i++)
            {
                HtmlTd = SplitString(HtmlTr[i], "</TD>");
                if (HtmlTd.Length < 7)
                {
                    continue;
                }
                MatchNumber = HtmlTd[0].Split('>')[1];
                Game = HtmlTd[1].Split('>')[1];
                GuestTeam = HtmlTd[2].Split('>')[2].Split('<')[0];
                MainTeam = HtmlTd[2].Split('>')[4].Split('<')[0];
                MainTeam = Regex.Replace(MainTeam, @"\(.+\)", "");
                letscore = HtmlTd[2].Split('(')[1].Split(')')[0].Replace("+", "");
                StopSellTime = HtmlTd[3].Split('>')[1];
                int Month = Shove._Convert.StrToInt(StopSellTime.Split('-')[1], 0);
                string OldYear = StopSellTime.Split('-')[0];
                int Year = DateTime.Now.Year;

                if (Month < DateTime.Now.Month)
                {
                    Year = Year + 1;
                }

                StopSellTime = StopSellTime.Replace(OldYear + "-" + Month.ToString().PadLeft(2, '0') + "-", Year.ToString() + "-" + Month.ToString().PadLeft(2, '0') + "-") + ":00";
                LetMainLose = GetSingleRate(HtmlTd[4]);
                LetMainWin = GetSingleRate(HtmlTd[5]);
                LetScoreMainLose = GetSingleRate(HtmlTd[6]);
                LetScoreMainWin = GetSingleRate(HtmlTd[7]);

                sb.Append(" exec P_BasketRfSfdg '" + MainTeam + "','" + GuestTeam + "','" + MatchNumber + "','" + Game + "','" + StopSellTime + "'," + LetMainLose + "," + LetMainWin + "," + LetScoreMainLose + "," + LetScoreMainWin + "," + letscore + ",0, '';");

                Count++;
            }
            return sb.ToString();
        }

        /// <summary>
        /// 得到单场胜负分差
        /// </summary>
        /// <param name="Html"></param>
        /// <param name="Count"></param>
        /// <returns></returns>
        public string Getwnm_vp(string Html, out int Count)
        {
            int IStart = 0;
            int ILen = 0;
            Count = 0;

            string[] HtmlTr = null;
            string[] HtmlTd = null;
            StringBuilder sb = new StringBuilder();

            sb.Append("update T_SingleRateBasket set IsWnm = 0 ; ");

            string StopSellTime = string.Empty;
            string MatchNumber = string.Empty;
            string Game = string.Empty;
            string MainTeam = string.Empty;
            string GuestTeam = string.Empty;

            string DifferGuest1_5 = string.Empty;
            string DifferGuest6_10 = string.Empty;
            string DifferGuest11_15 = string.Empty;
            string DifferGuest16_20 = string.Empty;
            string DifferGuest21_25 = string.Empty;
            string DifferGuest26 = string.Empty;
            string DifferMain1_5 = string.Empty;
            string DifferMain6_10 = string.Empty;
            string DifferMain11_15 = string.Empty;
            string DifferMain16_20 = string.Empty;
            string DifferMain21_25 = string.Empty;
            string DifferMain26 = string.Empty;

            Other.GetStrScope(Html, "", "", out IStart, out ILen);
            Other.GetStrScope(Html, "<DIV CLASS=\"box-tbl\">", "</TABLE", out IStart, out ILen);
            Html = Html.Substring(IStart, ILen);

            HtmlTr = SplitString(Html, "<TR>");
            for (int i = 1; i < HtmlTr.Length; i++)
            {
                HtmlTd = SplitString(HtmlTr[i], "</TD>");
                if (HtmlTd.Length < 10)
                {
                    continue;
                }

                MatchNumber = HtmlTd[0].Split('>')[1];
                Game = HtmlTd[1].Split('>')[1];
                GuestTeam = HtmlTd[2].Split('>')[2].Split('<')[0];
                MainTeam = HtmlTd[2].Split('>')[4].Split('<')[0];
                StopSellTime = HtmlTd[3].Split('>')[1];
                int Month = Shove._Convert.StrToInt(StopSellTime.Split('-')[1], 0);
                string OldYear = StopSellTime.Split('-')[0];
                int Year = DateTime.Now.Year;

                if (Month < DateTime.Now.Month)
                {
                    Year = Year + 1;
                }

                StopSellTime = StopSellTime.Replace(OldYear + "-" + Month.ToString().PadLeft(2, '0') + "-", Year.ToString() + "-" + Month.ToString().PadLeft(2, '0') + "-") + ":00";

                GetSinglewnm_vpRate(HtmlTd[5], out DifferGuest1_5, out DifferMain1_5);
                GetSinglewnm_vpRate(HtmlTd[6], out DifferGuest6_10, out DifferMain6_10);
                GetSinglewnm_vpRate(HtmlTd[7], out DifferGuest11_15, out DifferMain11_15);
                GetSinglewnm_vpRate(HtmlTd[8], out DifferGuest16_20, out DifferMain16_20);
                GetSinglewnm_vpRate(HtmlTd[9], out DifferGuest21_25, out DifferMain21_25);
                GetSinglewnm_vpRate(HtmlTd[10], out DifferGuest26, out DifferMain26);

                sb.Append(" exec P_BasketSfcdg '" + MainTeam + "','" + GuestTeam + "','" + MatchNumber + "','" + Game + "','" + StopSellTime + "'," + DifferGuest1_5 + "," + DifferGuest6_10 + "," + DifferGuest11_15 + "," + DifferGuest16_20 + "," + DifferGuest21_25 + "," + DifferGuest26 + ",");
                sb.Append(DifferMain1_5 + "," + DifferMain6_10 + "," + DifferMain11_15 + "," + DifferMain16_20 + "," + DifferMain21_25 + "," + DifferMain26 + ", 0, '';");
                Count++;
            }


            return sb.ToString();
        }

        /// <summary>
        /// 得到单场大小分
        /// </summary>
        /// <param name="Html"></param>
        /// <param name="Count"></param>
        /// <returns></returns>
        public string Gethilo_vp(string Html, out int Count)
        {
            int IStart = 0;
            int ILen = 0;
            Count = 0;

            string[] HtmlTr = null;
            string[] HtmlTd = null;

            StringBuilder sb = new StringBuilder();
            sb.Append("update T_SingleRateBasket set IsHilo = 0 ; ");

            string StopSellTime = string.Empty;
            string MatchDate = "";
            string MatchNumber = string.Empty;
            string Game = string.Empty;
            string MainTeam = string.Empty;
            string GuestTeam = string.Empty;

            string Big = string.Empty;
            string Small = string.Empty;
            string BigSmallscore = string.Empty;

            Other.GetStrScope(Html, "<DIV CLASS=\"box-tbl\">", "</TABLE", out IStart, out ILen);
            Html = Html.Substring(IStart, ILen);

            HtmlTr = SplitString(Html, new string[] { "<TR>", "<TR CLASS=\"tr1\">" });
            for (int i = 1; i < HtmlTr.Length; i++)
            {
                HtmlTd = SplitString(HtmlTr[i], "</TD>");
                if (HtmlTd.Length < 6)
                {
                    continue;
                }

                MatchNumber = HtmlTd[0].Split('>')[1];
                Game = HtmlTd[1].Split('>')[1];
                GuestTeam = HtmlTd[2].Split('>')[2].Split('<')[0];
                MainTeam = HtmlTd[2].Split('>')[4].Split('<')[0];
                StopSellTime = HtmlTd[3].Split('>')[1];
                MatchDate = HtmlTd[3].Split('>')[1];
                int Month = Shove._Convert.StrToInt(StopSellTime.Split('-')[1], 0);
                string OldYear = StopSellTime.Split('-')[0];
                int Year = DateTime.Now.Year;

                if (Month < DateTime.Now.Month)
                {
                    Year = Year + 1;
                }

                StopSellTime = StopSellTime.Replace(OldYear + "-" + Month.ToString().PadLeft(2, '0') + "-", Year.ToString() + "-" + Month.ToString().PadLeft(2, '0') + "-") + ":00";

                BigSmallscore = GetSingleRate(HtmlTd[4]);
                Big = GetSingleRate(HtmlTd[5]);
                Small = GetSingleRate(HtmlTd[6]);

                sb.Append(" exec P_BasketDXFdg '" + MainTeam + "','" + GuestTeam + "','" + MatchNumber + "','" + Game + "','" + MatchDate + "','" + StopSellTime + "'," + Big + "," + Small + "," + BigSmallscore + ", 0 ,'';");
                Count++;
            }
            return sb.ToString();
        }

        /// <summary>
        /// 胜负过关奖金
        /// </summary>
        /// <param name="Html"></param>
        /// <param name="Count"></param>
        /// <returns></returns>
        public string Get500Wanmnl_vp(string Html, out int Count)
        {
            int IStart = 0;
            int ILen = 0;
            Count = 0;

            string MainWin = string.Empty;
            string Mainlose = string.Empty;
            string MatchDate = string.Empty;
            string StopSellTime = string.Empty;
            string MatchNumber = string.Empty;
            string Game = string.Empty;
            string MainTeam = string.Empty;
            string GuestTeam = string.Empty;

            StringBuilder sb = new StringBuilder();
            sb.Append(" delete T_SingleRateBasket where StopSellTime<GETDATE(); update T_SingleRateBasket set IsMnl = 0 ; ");

            Other.GetStrScope(Html, "<DIV CLASS=\"dc_hs\"", "<DIV CLASS=\"dc_r\"", out IStart, out ILen);

            if (ILen == 0)
            {
                return "";
            }

            Html = Html.Substring(IStart, ILen);

            string[] days = Html.Split(new string[] { "<DIV CLASS=\"dc_hs\"" }, StringSplitOptions.RemoveEmptyEntries);
            string[] rows = null;
            string[] tds = null;

            Clutch clu = new Clutch();

            for (int i = 0; i < days.Length; i++)
            {
                rows = days[i].Split(new string[] { "</TR>" }, StringSplitOptions.RemoveEmptyEntries);

                for (int j = 0; j < rows.Length; j++)
                {
                    tds = rows[j].Split(new string[] { "</TD>" }, StringSplitOptions.RemoveEmptyEntries);
                    if (tds.Length == 13)
                    {
                        MatchNumber = clu.getHtmlText(tds[0], 2);
                        Game = clu.getHtmlText(tds[1], 2);
                        MainTeam = clu.getHtmlText(tds[5], 4);
                        GuestTeam = clu.getHtmlText(tds[3], 2);
                        StopSellTime = GetDateTime(tds[2].Substring(tds[2].IndexOf("match_time"))) + ":00";
                        MatchDate = GetDateTime(tds[2]) + ":00";

                        MainWin = clu.getHtmlText(tds[10], 2) == "" ? "0" : clu.getHtmlText(tds[10], 2);
                        Mainlose = clu.getHtmlText(tds[9], 2) == "" ? "0" : clu.getHtmlText(tds[9], 2);

                        sb.Append(" exec P_BasketSfdg '" + MainTeam + "','" + GuestTeam + "','" + MatchNumber + "','" + Game + "','" + MatchDate + "','" + StopSellTime + "'," + MainWin + "," + Mainlose + ", 0, '';");
                    }
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// 得到过关让分胜负
        /// </summary>
        /// <param name="Html"></param>
        /// <param name="Count"></param>
        /// <returns></returns>
        public string Get500Wanhdc_vp(string Html, out int Count)
        {
            int IStart = 0;
            int ILen = 0;
            Count = 0;

            StringBuilder sb = new StringBuilder();
            sb.Append(" delete T_SingleRateBasket where StopSellTime<GETDATE(); update T_SingleRateBasket set IsHdc = 0 ; ");

            Other.GetStrScope(Html, "<DIV CLASS=\"dc_hs\"", "<DIV CLASS=\"dc_r\"", out IStart, out ILen);
            Html = Html.Substring(IStart, ILen);

            string LetMainLose = string.Empty;
            string LetMainWin = string.Empty;
            string letscore = string.Empty;
            string MatchDate = string.Empty;
            string StopSellTime = string.Empty;
            string MatchNumber = string.Empty;
            string Game = string.Empty;
            string MainTeam = string.Empty;
            string GuestTeam = string.Empty;

            string[] days = Html.Split(new string[] { "<DIV CLASS=\"dc_hs\"" }, StringSplitOptions.RemoveEmptyEntries);
            string[] rows = null;
            string[] tds = null;

            Clutch clu = new Clutch();

            for (int i = 0; i < days.Length; i++)
            {
                rows = days[i].Split(new string[] { "</TR>" }, StringSplitOptions.RemoveEmptyEntries);

                for (int j = 0; j < rows.Length; j++)
                {
                    tds = rows[j].Split(new string[] { "</TD>" }, StringSplitOptions.RemoveEmptyEntries);
                    if (tds.Length == 13)
                    {
                        MatchNumber = clu.getHtmlText(tds[0], 2);
                        Game = clu.getHtmlText(tds[1], 2);
                        MainTeam = clu.getHtmlText(tds[5], 4);
                        GuestTeam = clu.getHtmlText(tds[3], 2);
                        StopSellTime = GetDateTime(tds[2].Substring(tds[2].IndexOf("match_time"))) + ":00";
                        MatchDate = GetDateTime(tds[2]) + ":00";

                        letscore = clu.getHtmlText(tds[4], 4);
                        LetMainLose = clu.getHtmlText(tds[9], 2) == "" ? "0" : clu.getHtmlText(tds[9], 2);
                        LetMainWin = clu.getHtmlText(tds[10], 2) == "" ? "0" : clu.getHtmlText(tds[10], 2);

                        sb.Append(" exec P_BasketRfSfdg '" + MainTeam + "','" + GuestTeam + "','" + MatchNumber + "','" + Game + "','" + MatchDate + "','" + StopSellTime + "'," + LetMainLose + "," + LetMainWin + ",0, 0,'" + letscore + "',0, '';");

                        sb.Append("update T_SingleRateBasket set LetScore= '" + letscore + "' where MatchNumber= '" + MatchNumber + "' ; ");
                    }
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// 得到过关胜分差
        /// </summary>
        /// <param name="Html"></param>
        /// <param name="Count"></param>
        /// <returns></returns>
        public string Get500Wanwnm_vp(string Html, out int Count)
        {

            int IStart = 0;
            int ILen = 0;
            Count = 0;

            StringBuilder sb = new StringBuilder();
            sb.Append(" delete T_SingleRateBasket where StopSellTime<GETDATE(); update T_SingleRateBasket set IsWnm = 0 ; ");

            string DifferGuest1_5 = string.Empty;
            string DifferGuest6_10 = string.Empty;
            string DifferGuest11_15 = string.Empty;
            string DifferGuest16_20 = string.Empty;
            string DifferGuest21_25 = string.Empty;
            string DifferGuest26 = string.Empty;
            string DifferMain1_5 = string.Empty;
            string DifferMain6_10 = string.Empty;
            string DifferMain11_15 = string.Empty;
            string DifferMain16_20 = string.Empty;
            string DifferMain21_25 = string.Empty;
            string DifferMain26 = string.Empty;
            string MatchDate = string.Empty;
            string StopSellTime = string.Empty;
            string MatchNumber = string.Empty;
            string Game = string.Empty;
            string MainTeam = string.Empty;
            string GuestTeam = string.Empty;

            Other.GetStrScope(Html, "<DIV CLASS=\"dc_hs\"", "<DIV CLASS=\"dc_r\"", out IStart, out ILen);

            Html = Html.Substring(IStart, ILen);

            string[] days = Html.Split(new string[] { "<DIV CLASS=\"dc_hs\"" }, StringSplitOptions.RemoveEmptyEntries);
            string[] rows = null;
            string[] rows_1 = null;
            string[] tds = null;
            string[] tds_1 = null;

            Clutch clu = new Clutch();

            for (int i = 0; i < days.Length; i++)
            {
                rows = Html.Split(new string[] { "ZID" }, StringSplitOptions.RemoveEmptyEntries);

                for (int j = 0; j < rows.Length; j++)
                {
                    tds = rows[j].Split(new string[] { "</TD>" }, StringSplitOptions.RemoveEmptyEntries);
                    if (tds.Length == 14)
                    {
                        rows_1 = rows[j + 1].Split(new string[] { "</TR>" }, StringSplitOptions.RemoveEmptyEntries);
                        tds_1 = rows_1[0].Split(new string[] { "</TD>" }, StringSplitOptions.RemoveEmptyEntries);

                        MatchNumber = clu.getHtmlText(tds[0], 2);
                        Game = clu.getHtmlText(tds[1], 2);
                        MainTeam = clu.getHtmlText(tds_1[0], 4);
                        GuestTeam = clu.getHtmlText(tds[3], 4);
                        StopSellTime = GetDateTime(tds[2].Substring(tds[2].IndexOf("match_time"))) + ":00";
                        MatchDate = GetDateTime(tds[2]) + ":00";

                        DifferGuest1_5 = clu.getHtmlText(tds[7], 2) == "" ? "0" : clu.getHtmlText(tds[7], 2);
                        DifferMain1_5 = clu.getHtmlText(tds_1[2], 2) == "" ? "0" : clu.getHtmlText(tds_1[2], 2);
                        DifferGuest6_10 = clu.getHtmlText(tds[8], 2) == "" ? "0" : clu.getHtmlText(tds[8], 2);
                        DifferMain6_10 = clu.getHtmlText(tds_1[3], 2) == "" ? "0" : clu.getHtmlText(tds_1[3], 2);
                        DifferGuest11_15 = clu.getHtmlText(tds[9], 2) == "" ? "0" : clu.getHtmlText(tds[9], 2);
                        DifferMain11_15 = clu.getHtmlText(tds_1[4], 2) == "" ? "0" : clu.getHtmlText(tds_1[4], 2);
                        DifferGuest16_20 = clu.getHtmlText(tds[10], 2) == "" ? "0" : clu.getHtmlText(tds[10], 2);
                        DifferMain16_20 = clu.getHtmlText(tds_1[5], 2) == "" ? "0" : clu.getHtmlText(tds_1[5], 2);
                        DifferGuest21_25 = clu.getHtmlText(tds[11], 2) == "" ? "0" : clu.getHtmlText(tds[11], 2);
                        DifferMain21_25 = clu.getHtmlText(tds_1[6], 2) == "" ? "0" : clu.getHtmlText(tds_1[6], 2);
                        DifferGuest26 = clu.getHtmlText(tds[12], 2) == "" ? "0" : clu.getHtmlText(tds[12], 2);
                        DifferMain26 = clu.getHtmlText(tds_1[7], 2) == "" ? "0" : clu.getHtmlText(tds_1[7], 2);

                        sb.Append(" exec P_BasketSfcdg '" + MainTeam + "','" + GuestTeam + "','" + MatchNumber + "','" + Game + "','" + MatchDate + "','" + StopSellTime + "'," + DifferGuest1_5 + "," + DifferGuest6_10 + "," + DifferGuest11_15 + "," + DifferGuest16_20 + "," + DifferGuest21_25 + "," + DifferGuest26 + ",");
                        sb.Append(DifferMain1_5 + "," + DifferMain6_10 + "," + DifferMain11_15 + "," + DifferMain16_20 + "," + DifferMain21_25 + "," + DifferMain26 + ", 0, '';");
                    }
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// 得到大小分
        /// </summary>
        /// <param name="Html"></param>
        /// <param name="Count"></param>
        /// <returns></returns>
        public string Get500Wanhilo_vp(string Html, out int Count)
        {
            int IStart = 0;
            int ILen = 0;
            Count = 0;

            string MainWin = string.Empty;
            string Mainlose = string.Empty;
            string MatchDate = string.Empty;
            string StopSellTime = string.Empty;
            string MatchNumber = string.Empty;
            string Game = string.Empty;
            string MainTeam = string.Empty;
            string GuestTeam = string.Empty;
            string Big = string.Empty;
            string Small = string.Empty;
            string BigSmallscore = string.Empty;

            StringBuilder sb = new StringBuilder();

            Other.GetStrScope(Html, "<DIV CLASS=\"dc_hs\"", "<DIV CLASS=\"dc_r\"", out IStart, out ILen);

            if (ILen == 0)
            {
                return "";
            }

            Html = Html.Substring(IStart, ILen);

            string[] days = Html.Split(new string[] { "<DIV CLASS=\"dc_hs\"" }, StringSplitOptions.RemoveEmptyEntries);
            string[] rows = null;
            string[] tds = null;
            string date = string.Empty;

            Clutch clu = new Clutch();

            for (int i = 0; i < days.Length; i++)
            {
                rows = days[i].Split(new string[] { "</TR>" }, StringSplitOptions.RemoveEmptyEntries);
                date = clu.GetDate(rows[0]);

                for (int j = 0; j < rows.Length; j++)
                {
                    tds = rows[j].Split(new string[] { "</TD>" }, StringSplitOptions.RemoveEmptyEntries);
                    if (tds.Length == 13)
                    {
                        MatchNumber = clu.getHtmlText(tds[0], 2);
                        Game = clu.getHtmlText(tds[1], 2);
                        MainTeam = clu.getHtmlText(tds[5], 4);
                        GuestTeam = clu.getHtmlText(tds[3], 2);
                        StopSellTime = GetDateTime(tds[2].Substring(tds[2].IndexOf("match_time"))) + ":00";
                        MatchDate = GetDateTime(tds[2]) + ":00";

                        Big = clu.getHtmlText(tds[10], 2) == "" ? "0" : clu.getHtmlText(tds[10], 2);
                        Small = clu.getHtmlText(tds[9], 2) == "" ? "0" : clu.getHtmlText(tds[9], 2);
                        BigSmallscore = clu.getHtmlText(tds[4], 2) == "" ? "0" : clu.getHtmlText(tds[4], 2);


                        sb.Append(" exec P_BasketDXFdg '" + MainTeam + "','" + GuestTeam + "','" + MatchNumber + "','" + Game + "','" + MatchDate + "','" + StopSellTime + "'," + Big + "," + Small + "," + BigSmallscore + ", 0 ,'';");
                    }
                }
            }

            return sb.ToString();
        }


        #endregion 生成SQL命令

        #region 辅助

        /// <summary>
        /// 得到过关URL和标题
        /// </summary>
        /// <returns></returns>
        public string[,] GetPassRateStrUrl()
        {
            string[,] strUrl = new string[4, 4];
            strUrl[0, 0] = "http://info.sporttery.cn/basketball/mnl_list.php";
            strUrl[0, 1] = "胜负";
            strUrl[1, 0] = "http://info.sporttery.cn/basketball/hdc_list.php";
            strUrl[1, 1] = "让分胜负";
            strUrl[2, 0] = "http://info.sporttery.cn/basketball/wnm_list.php";
            strUrl[2, 1] = "胜分差";
            strUrl[3, 0] = "http://info.sporttery.cn/basketball/hilo_list.php";
            strUrl[3, 1] = "大小分";

            return strUrl;
        }

        /// <summary>
        /// 得到过关URL和标题
        /// </summary>
        /// <returns></returns>
        public string[,] Get500WanPassRateStrUrl()
        {
            string[,] strUrl = new string[4, 4];
            strUrl[0, 0] = "http://trade.500wan.com/jclq/index.php?playid=274";
            strUrl[0, 1] = "胜负";
            strUrl[1, 0] = "http://trade.500wan.com/jclq/index.php?playid=275";
            strUrl[1, 1] = "让分胜负";
            strUrl[2, 0] = "http://trade.500wan.com/jclq/index.php?playid=276";
            strUrl[2, 1] = "胜分差";
            strUrl[3, 0] = "http://trade.500wan.com/jclq/index.php?playid=277";
            strUrl[3, 1] = "大小分";

            return strUrl;
        }

        /// <summary>
        /// 得到单场URL和标题
        /// </summary>
        /// <returns></returns>
        public string[,] GetSingleRateStrUrl()
        {
            string[,] strUrl = new string[4, 4];
            strUrl[0, 0] = "http://info.sporttery.cn/basketball/mnl_vp.php";
            strUrl[0, 1] = "胜负";
            strUrl[1, 0] = "http://info.sporttery.cn/basketball/hdc_vp.php";
            strUrl[1, 1] = "让分胜负";
            strUrl[2, 0] = "http://info.sporttery.cn/basketball/wnm_vp.php";
            strUrl[2, 1] = "胜分差";
            strUrl[3, 0] = "http://info.sporttery.cn/basketball/hilo_vp.php";
            strUrl[3, 1] = "大小分";

            return strUrl;
        }

        /// <summary>
        /// 得到单场URL和标题
        /// </summary>
        /// <returns></returns>
        public string[,] Get500WanSingleRateStrUrl()
        {
            string[,] strUrl = new string[4, 4];
            strUrl[0, 0] = "http://trade.500wan.com/jclq/index.php?playid=274&g=1";
            strUrl[0, 1] = "胜负";
            strUrl[1, 0] = "http://trade.500wan.com/jclq/index.php?playid=275&g=1";
            strUrl[1, 1] = "让分胜负";
            strUrl[2, 0] = "http://trade.500wan.com/jclq/index.php?playid=276&g=1";
            strUrl[2, 1] = "胜分差";
            strUrl[3, 0] = "http://trade.500wan.com/jclq/index.php?playid=277&g=1";
            strUrl[3, 1] = "大小分";

            return strUrl;
        }


        /// <summary>
        /// 单场奖金
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private string GetSingleRate(string str)
        {
            return str.IndexOf("尚") == -1 ? str.Split('>')[1].Split('<')[0] : "0";
        }

        /// <summary>
        /// 胜分差Rate
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private string GetSinglewnm_vpRate(string str, out string Rate1, out string Rate2)
        {
            Rate1 = string.Empty;
            Rate2 = string.Empty;
            Rate1 = str.Split('>')[1].Split('<')[0];

            if (!Regex.Match(Rate1, @"\d+").Success)
            {
                Rate1 = "0";
            }
            Rate2 = SplitString(str, "<BR />")[1].Split('<')[0];

            if (!Regex.Match(Rate2, @"\d+").Success)
            {
                Rate2 = "0";
            }
            return "";
        }

        /// <summary>
        /// 字符串分割字符串
        /// </summary>
        /// <param name="str"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        private string[] SplitString(string str, string separator)
        {
            return HTMLEdit.SplitString(str, separator);
        }

        /// <summary>
        /// 字符串分割字符串
        /// </summary>
        /// <param name="str"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        private string[] SplitString(string str, string[] Arrseparator)
        {
            return str.Split(Arrseparator, StringSplitOptions.RemoveEmptyEntries);
        }

        private string GetString(string str)
        {
            string t_str = "";

            for (int i = 0; i < str.Length; i++)
            {
                if (("1234567890.").IndexOf(str.Substring(i, 1)) >= 0)
                {
                    return t_str;
                }

                t_str += str.Substring(i, 1);
            }

            return t_str;
        }

        /// <summary>
        /// 过滤得到时间格式字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        string GetDateTime(string str)
        {
            return System.Text.RegularExpressions.Regex.Match(str, @"\d{4}-\d{2}-\d{2}\s\d{2}:\d{2}").Value;
        }

        #endregion 辅助
    }
}
