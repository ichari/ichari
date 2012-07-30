using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

using System.Text;
using System.IO;
using System.Security.Cryptography;
using Shove.Database;
using System.Text.RegularExpressions;

public partial class Home_Room_WinNotice1 :Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DataTable dtFc = BindNewWinNumber(6);//福彩3D
            if (dtFc.Rows.Count > 0 && dtFc != null)
            {
                lblFC3D.Text = dtFc.Rows[0]["Name"].ToString();
                spFCWinNumber.InnerHtml = "<font class='redfont'>" + dtFc.Rows[0]["WinLotteryNumber"].ToString() + "</font>";

                DataTable dtTestNumber = new DAL.Tables.T_TestNumber().Open("", "IsuseID=" + dtFc.Rows[0]["ID"].ToString(), "");

                if (dtTestNumber == null)
                {
                    return;
                }

                if (dtTestNumber.Rows.Count > 0)
                {
                    spFCTest.InnerHtml = dtTestNumber.Rows[0]["TestNumber"].ToString();
                }
            }

            DataTable dtSSQ = BindNewWinNumber(5);//双色球
            if (dtSSQ.Rows.Count > 0 && dtSSQ != null)
            {
                lbSSQ.Text = dtSSQ.Rows[0]["Name"].ToString();
                spSSQWinNumber.InnerHtml = "<font class='redfont'>" + dtSSQ.Rows[0]["WinLotteryNumber"].ToString() + "</font>";
            }

            DataTable dtQLC = BindNewWinNumber(13);//七乐彩
            if (dtQLC.Rows.Count > 0 && dtQLC != null)
            {
                lbQLC.Text = dtQLC.Rows[0]["Name"].ToString();
                spQLCWinNumber.InnerHtml = "<font class='redfont'>" + dtQLC.Rows[0]["WinLotteryNumber"].ToString() + "</font>";
            }

            DataTable dtHNFC22X5 = BindNewWinNumber(69);//河南福彩22选5
            if (dtHNFC22X5.Rows.Count > 0 && dtHNFC22X5 != null)
            {
                lbHNFC22X5.Text = dtHNFC22X5.Rows[0]["Name"].ToString();
                spHNFC22X5WinNumber.InnerHtml = "<font class='redfont'>" + dtHNFC22X5.Rows[0]["WinLotteryNumber"].ToString() + "</font>";
            }

            DataTable dtTCDTL = BindNewWinNumber(39);//超级大透乐
            if (dtTCDTL.Rows.Count > 0 && dtTCDTL != null)
            {
                lbTCDTL.Text = dtTCDTL.Rows[0]["Name"].ToString();
                spTCDTLWinNumber.InnerHtml = "<font class='redfont'>" + dtTCDTL.Rows[0]["WinLotteryNumber"].ToString() + "</font>";
            }

            DataTable dtPL3 = BindNewWinNumber(63);//排列3
            if (dtPL3.Rows.Count > 0 && dtPL3 != null)
            {
                lbPl3.Text = dtPL3.Rows[0]["Name"].ToString();
                spPl3WinNumber.InnerHtml = "<font class='redfont'>" + dtPL3.Rows[0]["WinLotteryNumber"].ToString() + "</font>";
            }

            DataTable dtPL5 = BindNewWinNumber(64);//排列5
            if (dtPL5.Rows.Count > 0 && dtPL5 != null)
            {
                lbPl5.Text = dtPL5.Rows[0]["Name"].ToString();
                spPl5WinNumber.InnerHtml = "<font class='redfont'>" + dtPL5.Rows[0]["WinLotteryNumber"].ToString() + "</font>";
            }

            DataTable dtQxC = BindNewWinNumber(3);//七星彩
            if (dtQxC.Rows.Count > 0 && dtQxC != null)
            {
                lbQxC.Text = dtQxC.Rows[0]["Name"].ToString();
                spQxCWinNumber.InnerHtml = "<font class='redfont'>" + dtQxC.Rows[0]["WinLotteryNumber"].ToString() + "</font>";
            }

            DataTable dtTC22_5 = BindNewWinNumber(9);//体彩22选5
            if (dtTC22_5.Rows.Count > 0 && dtTC22_5 != null)
            {
                lbTC22_5.Text = dtTC22_5.Rows[0]["Name"].ToString();
                spTC22_5WinNumber.InnerHtml = "<font class='redfont'>" + dtTC22_5.Rows[0]["WinLotteryNumber"].ToString() + "</font>";
            }

            DataTable dtSFCRJ = BindNewWinNumber(74);//胜负彩(任九)
            if (dtSFCRJ.Rows.Count > 0 && dtSFCRJ != null)
            {
                lblSFCRJ.Text = dtSFCRJ.Rows[0]["Name"].ToString();
                spSFCRJWinNumber.InnerHtml = "<font class='redfont'>" + dtSFCRJ.Rows[0]["WinLotteryNumber"].ToString() + "</font>";
            }

            DataTable dtLCBQC = BindNewWinNumber(15);//六场半全场
            if (dtLCBQC.Rows.Count > 0 && dtLCBQC != null)
            {
                lblLCBQC.Text = dtLCBQC.Rows[0]["Name"].ToString();
                spLCBQCWinNumber.InnerHtml = "<font class='redfont'>" + dtLCBQC.Rows[0]["WinLotteryNumber"].ToString() + "</font>";
            }

            DataTable dtSCJQC = BindNewWinNumber(2);//四场进球彩
            if (dtSCJQC.Rows.Count > 0 && dtSCJQC != null)
            {
                lblSCJQC.Text = dtSCJQC.Rows[0]["Name"].ToString();
                spSCJQC.InnerHtml = "<font class='redfont'>" + dtSCJQC.Rows[0]["WinLotteryNumber"].ToString() + "</font>";
            }

        }
    }

    /// <summary>
    /// 绑定全国最新开奖号码
    /// </summary>
    /// <param name="LotteryId">彩种ID</param>
    protected DataTable BindNewWinNumber(int LotteryId)
    {
        DataTable dt = null;

        string strCmd = @"select top 1 ID, Name,WinLotteryNumber from T_Isuses where LotteryId =@LotteryID and isnull(WinLotteryNumber, '') <> '' order by EndTime desc";

        if (dt == null || dt.Rows.Count <= 0)
        {
            dt = MSSQL.Select(strCmd.ToString(),
              new MSSQL.Parameter("LotteryID", SqlDbType.BigInt, 0, ParameterDirection.Input, LotteryId));
        }

        return dt;
    }
}
