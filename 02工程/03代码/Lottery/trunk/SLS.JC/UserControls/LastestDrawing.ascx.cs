using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class UserControls_LastestDrawing : System.Web.UI.UserControl
{
    public int LotteryID { get; set; }
    public string LotteryName { get; set; }
    public string LotteryUrl { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        
    }

    public void LoadLottoery(string DrawNum, string WinNum)
    {
        lbLottoName.Text = LotteryName;
        hlLotto.NavigateUrl = LotteryUrl;
        lbDrawNumber.Text = DrawNum;
        
        string[] wNum = WinNum.Split('+');
        string[] rdNum = wNum[0].Trim().Split(' ');
        LiteralControl lcRed = new LiteralControl();    
        for (int i = 0; i < rdNum.Length; i++)
        {
            lcRed.Text += "<span class=\"b b_png b_p2\">" + rdNum[i].Trim() + "</span>";
        }
        phWinNumRed.Controls.Add(lcRed);
        if (WinNum.IndexOf('+') > 0)
        {
            LiteralControl lcBlue = new LiteralControl();
            string[] blNum = wNum[1].Trim().Split(' ');
            for (int i = 0; i < blNum.Length; i++)
            {
                lcBlue.Text += "<span class=\"b b_png b_p1\">" + blNum[i].Trim() + "</span>";
            }
            phWinNumBlue.Controls.Add(lcBlue);
        }
    }
}