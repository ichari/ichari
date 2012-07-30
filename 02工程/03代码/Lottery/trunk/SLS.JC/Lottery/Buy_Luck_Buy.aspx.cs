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
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Text.RegularExpressions;


public partial class Buy_Luck_Buy : RoomPageBase
{

    public int LotteryID = 0;
    public string lotteryName = "";
    public string LoginTopSrcUrl;
    public string LoginBackSrcUrl = "";
    public string RegisterUrl = "";
    public string ForgetPwdUrl = "";
    public string AlipayLoginUrl = "";
    public string LoginIframeUrl = "";
    public string AlipayLoginSrcUrl = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        AjaxPro.Utility.RegisterTypeForAjax(typeof(Buy_Luck_Buy), this.Page);
        LoginTopSrcUrl = ResolveUrl("~/Home/Room/images/login_top.jpg");
        LoginBackSrcUrl = ResolveUrl("~/Home/Room/images/login_back.jpg");
        RegisterUrl = ResolveUrl("~/UserReg.aspx");
        ForgetPwdUrl = ResolveUrl("~/ForgetPassword.aspx");
        AlipayLoginUrl = ResolveUrl("~/Home/Room/Login.aspx");
        LoginIframeUrl = ResolveUrl("~/Home/Room/UserLoginDialog.aspx");
        AlipayLoginSrcUrl = ResolveUrl("~/Home/Room/images/zfb_button2.jpg");

        if (_User != null)
        {
            HidUserID.Value = _User.ID.ToString();
        }

        LotteryID = 39;
        lotteryName = "cjdlt";
    }

    #region Web 窗体设计器生成的代码

    override protected void OnInit(EventArgs e)
    {
        RequestLoginPage = Request.Url.AbsolutePath;

        isRequestLogin = false;
        base.OnInit(e);
    }

    #endregion
  
    [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
    public string GenerateLuckLotteryNumber(int LotteryID, string Type, string Name, string lotteryName)
    {
        if (string.IsNullOrEmpty(lotteryName))
        {
            lotteryName = "cjdlt";
        }
        string Key = "Home_Room_Buy_GenerateLuckLotteryNumber" + LotteryID.ToString();

        Type = Shove._Web.Utility.FilteSqlInfusion(Type);
        Name = Shove._Web.Utility.FilteSqlInfusion(Name);

        if (Type == "3")
        {
            try
            {
                DateTime time = Convert.ToDateTime(Name);
                Name = time.ToString("yyyy-MM-dd");

                if (time > DateTime.Now)
                {
                    return "出生日期不能超过当前日期！";
                }
            }
            catch
            {
                return "日期格式不正确！";
            }
        }

        DataTable dt = Shove._Web.Cache.GetCacheAsDataTable(Key);

        if (dt == null || dt.Rows.Count == 0)
        {
            dt = new DAL.Tables.T_LuckNumber().Open("", "datediff(d,getdate(),DateTime)=0 and LotteryID=" + LotteryID.ToString() + "", "");

            Shove._Web.Cache.SetCache(Key, dt, 3600);
        }

        string LotteryNumber = "";

        DataRow[] dr = dt.Select("Type=" + Type + " and Name='" + Name + "'");

        if (dr != null && dr.Length > 0)
        {
            LotteryNumber = dr[0]["LotteryNumber"].ToString();
        }
        else
        {           

            string num = "";
            switch (lotteryName)
            {
                case "cjdlt":
                    num = new SLS.Lottery()[LotteryID].BuildNumber(5, 2, 1);
                    break;
                case "pl3":
                    num = new SLS.Lottery()[LotteryID].BuildNumber(1);
                    break;
                case "ssq":
                    num = new SLS.Lottery()[LotteryID].BuildNumber(6, 1, 1);
                    break;
                case "3d":
                    num = new SLS.Lottery()[LotteryID].BuildNumber(1);
                    break;
                case "qlc":
                    num = new SLS.Lottery()[LotteryID].BuildNumber(1);
                    break;
                case "pl5":
                    num = new SLS.Lottery()[LotteryID].BuildNumber(1);
                    break;
                case "qxc":
                    num = new SLS.Lottery()[LotteryID].BuildNumber(1);
                    break;
                case "22x5":
                    num = new SLS.Lottery()[LotteryID].BuildNumber(1);
                    break;
                case "31x7":
                    num = new SLS.Lottery()[LotteryID].BuildNumber(1);
                    break;
            }

            LotteryNumber = num;

            DAL.Tables.T_LuckNumber ln = new DAL.Tables.T_LuckNumber();

            ln.LotteryID.Value = LotteryID;
            ln.LotteryNumber.Value = LotteryNumber;
            ln.Name.Value = Name;
            ln.Type.Value = Type;

            ln.Insert();
            ln.Delete("datediff(d,DateTime,getdate())>0");

            Shove._Web.Cache.ClearCache(Key);
        }

        string LuckLottery = "";

        switch (lotteryName)
        {
            case "cjdlt":
                LuckLottery = FormatLuckLotteryNumber_cjdlt(LotteryID, LotteryNumber);
                break;
            case "pl3":
                LuckLottery = FormatLuckLotteryNumber_PL3(LotteryID, LotteryNumber);
                break;
            case "ssq":
                LuckLottery = FormatLuckLotteryNumber_SSQ(LotteryID, LotteryNumber);
                break;
            case "3d":
                LuckLottery = FormatLuckLotteryNumber_3D(LotteryNumber);
                break;
            case "qlc":
                LuckLottery = FormatLuckLotteryNumber_QLC(LotteryNumber);
                break;
            case "pl5":
                LuckLottery = FormatLuckLotteryNumber_PL5(LotteryNumber);
                break;
            case "qxc":
                LuckLottery = FormatLuckLotteryNumber_QXC(LotteryID, LotteryNumber);
                break;
            case "22x5":
                LuckLottery = FormatLuckLotteryNumber_22x5(LotteryNumber);
                break;
            case "31x7":
                LuckLottery = FormatLuckLotteryNumber_31X7(LotteryID, LotteryNumber);
                break;
        }


        return LotteryNumber + "|" + LuckLottery;
    }

    [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
    public string returnHidValue(string id)
    {        
        //期号 过期时间 玩法ID
        string IsuseID = "";
        string IsuseEndTime = "";
        string playType = "";
        string val;

        id = Shove._Web.Utility.FilteSqlInfusion(id);
        DAL.Tables.T_Isuses Isuses = new DAL.Tables.T_Isuses();
        DataTable table = Isuses.Open("ID,EndTime", "LotteryID = " + id + " and (GETDATE() between StartTime and EndTime)", "");

        if (table != null && table.Rows.Count > 0)
        {
            IsuseID = table.Rows[0]["id"].ToString();
            IsuseEndTime = table.Rows[0]["EndTime"].ToString();
        }

        playType = id + "01";

        val = IsuseID + "|" + IsuseEndTime + "|" + playType;

        return val;
    }

    /// <summary>
    /// Ajax 登录
    /// </summary>
    /// <param name="UserName"></param>
    /// <param name="Password"></param>
    /// <param name="InputCheckCode">用户输入的验证码</param>
    /// <param name="CheckCode">从验证码控件中取得的验证码加密串</param>
    /// <param name="SiteID"></param>
    /// <returns></returns>
    [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
    public string Login(string UserName, string Password, string InputCheckCode, string CheckCode, long SiteID)
    {
        if (SiteID < 0)
        {
            return "0";
        }

        string ReturnDescription = "";

        Sites t_site = new Sites(SiteID);

        int Result = 0;

        try
        {
            Result = new Login().LoginSubmit(this.Page, t_site, UserName, Password, InputCheckCode, CheckCode, ref ReturnDescription);
        }
        catch
        {
            return "登录出现异常 ！";
        }

        if (Result < 0)
        {
            return (ReturnDescription == "" ? "登录出现异常 ！" : ReturnDescription);
        }

        Users _user = Users.GetCurrentUser(SiteID);

        return _user.ID.ToString();
    }


    #region 生成幸运数字
    //cjdlt
    private string FormatLuckLotteryNumber_cjdlt(int LotteryID, string LotteryNumber)
    {
        if (string.IsNullOrEmpty(LotteryNumber))
        {
            return "";
        }

        LotteryNumber = LotteryNumber.Replace(" + ", " ");

        return LotteryNumber;
    }

    //PL3
    private string FormatLuckLotteryNumber_PL3(int LotteryID, string LotteryNumber)
    {
        if (string.IsNullOrEmpty(LotteryNumber))
        {
            return "";
        }

        string Red = null;
        string Blue = null;

        Red = LotteryNumber.Split('+')[0];
        try
        {
            Blue = LotteryNumber.Split('+')[1];
        }
        catch { Blue = ""; }

        LotteryNumber = "";

        for (int i = 0; i < Red.Length; i++)
        {
            LotteryNumber += Red.Substring(i, 1) + " ";
        }

        LotteryNumber += Blue;

        LotteryNumber = LotteryNumber.Trim();

        return LotteryNumber;
    }
   
    //SSQ
    private string FormatLuckLotteryNumber_SSQ(int LotteryID, string LotteryNumber)
    {
        if (string.IsNullOrEmpty(LotteryNumber))
        {
            return "";
        }

        LotteryNumber = LotteryNumber.Replace(" + ", " ");

        return LotteryNumber;
    }

    //3d
    private string FormatLuckLotteryNumber_3D(string LotteryNumber)
    {
        if (string.IsNullOrEmpty(LotteryNumber))
        {
            return "";
        }

        string Red = null;
        string Blue = null;

        Red = LotteryNumber.Split('+')[0];
        try
        {
            Blue = LotteryNumber.Split('+')[1];
        }
        catch { Blue = ""; }

        LotteryNumber = "";

        for (int i = 0; i < Red.Length; i++)
        {
            LotteryNumber += Red.Substring(i, 1) + " ";
        }

        LotteryNumber += Blue;

        LotteryNumber = LotteryNumber.Trim();

        return LotteryNumber;
    }

    //qlc
    private string FormatLuckLotteryNumber_QLC(string LotteryNumber)
    {
        if (string.IsNullOrEmpty(LotteryNumber))
        {
            return "";
        }

        LotteryNumber = LotteryNumber.Replace(" + ", " ");

        return LotteryNumber;
    }

    //pl5
    private string FormatLuckLotteryNumber_PL5(string LotteryNumber)
    {
        if (string.IsNullOrEmpty(LotteryNumber))
        {
            return "";
        }

        string Red = null;
        string Blue = null;

        Red = LotteryNumber.Split('+')[0];
        try
        {
            Blue = LotteryNumber.Split('+')[1];
        }
        catch { Blue = ""; }

        LotteryNumber = "";

        for (int i = 0; i < Red.Length; i++)
        {
            LotteryNumber += Red.Substring(i, 1) + " ";
        }

        LotteryNumber += Blue;

        LotteryNumber = LotteryNumber.Trim();

        return LotteryNumber;
    }

    //qxc
    private string FormatLuckLotteryNumber_QXC(int LotteryID, string LotteryNumber)
    {
        if (string.IsNullOrEmpty(LotteryNumber))
        {
            return "";
        }

        string Red = null;
        string Blue = null;

        Red = LotteryNumber.Split('+')[0];
        try
        {
            Blue = LotteryNumber.Split('+')[1];
        }
        catch { Blue = ""; }

        LotteryNumber = "";

        for (int i = 0; i < Red.Length; i++)
        {
            LotteryNumber += Red.Substring(i, 1) + " ";
        }

        LotteryNumber += Blue;

        LotteryNumber = LotteryNumber.Trim();

        return LotteryNumber;
    }

    //22x5
    private string FormatLuckLotteryNumber_22x5(string LotteryNumber)
    {
        if (string.IsNullOrEmpty(LotteryNumber))
        {
            return "";
        }

        LotteryNumber = LotteryNumber.Replace(" + ", " ");

        return LotteryNumber;
    }

    //31x7
    private string FormatLuckLotteryNumber_31X7(int LotteryID, string LotteryNumber)
    {
        if (string.IsNullOrEmpty(LotteryNumber))
        {
            return "";
        }

        LotteryNumber = LotteryNumber.Replace(" + ", " ");

        return LotteryNumber;
    }

    #endregion 生成幸运数字
 
    /// <summary>
    /// 购买彩票
    /// </summary>
    /// <param name="_User"></param>
    private void Buy(Users _User)
    {
        if (_User == null)
        {
            Shove._Web.JavaScript.Alert(this.Page, "请您先登录,然后再进行购买!");

            return;
        }

        string HidIsuseID = Shove._Web.Utility.GetRequest("HidIsuseID");        //期号ID
        string HidIsuseEndTime = Shove._Web.Utility.GetRequest("HidIsuseEndTime");      //过期时间
        string playType = Shove._Web.Utility.GetRequest("tbPlayTypeID");                //玩法ID        
        string HidLotteryID = Shove._Web.Utility.GetRequest("HidLotteryID");                        //彩票ID
        string tb_LotteryNumber = Shove._Web.Utility.FilteSqlInfusion(Request["tb_LotteryNumber"]);     //彩票号
        string tb_hide_SumMoney = Shove._Web.Utility.GetRequest("HidPrice");                //总金额

        string tb_Share = "1";                    //份数
        string tb_BuyShare = "1";              //买多少份
        string tb_OpenUserList = "";          //招股对象
        string tb_Title = "";                        //方案名
        string tb_Description = "幸运投注";                //描述
        string tbAutoStopAtWinMoney = "0";
        string tbSecrecyLevel = "0";                      //保密级别        
        string tb_hide_SumNum = "1";                    //总份数
        string tb_Multiple = "1";                          //倍数
        string tb_hide_AssureMoney = "0";          //保底金额
        string tb_SchemeBonusScale = "4";          //佣金特殊值
        string tb_SchemeBonusScalec = "4";

        int Price = 2;

        if ((playType == "3903") || (playType == "3904"))
        {
            Price = 3;
        }
        else
        {
            Price = 2;
        }
        // 结束

        if (tb_Multiple == "")
        {
            tb_Multiple = "1";
        }

        double SumMoney = 0;
        int Share = 0;
        int BuyShare = 0;
        double AssureMoney = 0;
        int Multiple = 0;
        int SumNum = 0;
        short SecrecyLevel = 0;
        int PlayTypeID = 0;
        int LotteryID = 0;
        long IsuseID = 0;
        double AutoStopAtWinMoney = 0;
        double SchemeBonusScale = 0;
        double SchemeBonusScalec = 0;

        try
        {
            SumMoney = double.Parse(tb_hide_SumMoney);
            Share = int.Parse(tb_Share);
            BuyShare = int.Parse(tb_BuyShare);
            AssureMoney = double.Parse(tb_hide_AssureMoney);
            Multiple = int.Parse(tb_Multiple);
            SumNum = int.Parse(tb_hide_SumNum);
            SecrecyLevel = short.Parse(tbSecrecyLevel);
            PlayTypeID = int.Parse(playType);
            LotteryID = int.Parse(HidLotteryID);
            IsuseID = long.Parse(HidIsuseID);
            AutoStopAtWinMoney = double.Parse(tbAutoStopAtWinMoney);
            SchemeBonusScale = double.Parse(tb_SchemeBonusScale);
            SchemeBonusScalec = double.Parse(tb_SchemeBonusScalec);
        }
        catch
        {
            Shove._Web.JavaScript.Alert(this.Page, "输入有错误，请仔细检查。");

            return;
        }

        if ((SumMoney <= 0) || (SumNum < 1))
        {
            Shove._Web.JavaScript.Alert(this.Page, "输入有错误，请仔细检查。");

            return;
        }

        if (AssureMoney < 0)
        {
            Shove._Web.JavaScript.Alert(this.Page, "输入有错误，请仔细检查。");

            return;
        }

        if (Share < 1)
        {
            Shove._Web.JavaScript.Alert(this.Page, "输入有错误，请仔细检查。");

            return;
        }

        if ((BuyShare == Share) && (AssureMoney == 0))
        {
            Share = 1;
            BuyShare = 1;
        }

        if ((SumMoney / Share) < 1)
        {
            Shove._Web.JavaScript.Alert(this.Page, "每份金额最低不能少于 1 元。");

            return;
        }

        double BuyMoney = BuyShare * (SumMoney / Share) + AssureMoney;

        

        if (BuyMoney > PF.SchemeMaxBettingMoney)
        {
            Shove._Web.JavaScript.Alert(this.Page, "投注金额不能大于" + PF.SchemeMaxBettingMoney.ToString() + "，谢谢。");

            return;
        }

        if (Multiple > 999)
        {
            Shove._Web.JavaScript.Alert(this.Page, "投注倍数不能大于 999 倍，谢谢。");

            return;
        }
        //佣金比例的计算

        if (!(SchemeBonusScale >= 0 || SchemeBonusScale <= 10))
        {
            Shove._Web.JavaScript.Alert(this.Page, "佣金比例只能在0~10之间");

            return;
        }

        if (SchemeBonusScale.ToString().IndexOf("-") > -1 || SchemeBonusScale.ToString().IndexOf(".") > -1)
        {
            Shove._Web.JavaScript.Alert(this.Page, "佣金比例输入有误");

            return;
        }
        if (!(SchemeBonusScalec >= 0 || SchemeBonusScalec <= 10))
        {
            Shove._Web.JavaScript.Alert(this.Page, "佣金比例只能在0~10之间");

            return;
        }

        if (SchemeBonusScalec.ToString().IndexOf("-") > -1 || SchemeBonusScalec.ToString().IndexOf(".") > -1)
        {
            Shove._Web.JavaScript.Alert(this.Page, "佣金比例输入有误");

            return;
        }

        SchemeBonusScale = SchemeBonusScale / 100;
        SchemeBonusScalec = SchemeBonusScalec / 100;

        string LotteryNumber = tb_LotteryNumber;

        if (LotteryNumber[LotteryNumber.Length - 1] == '\n')
        {
            LotteryNumber = LotteryNumber.Substring(0, LotteryNumber.Length - 1);
        }

        #region 对彩票号码进行分析，判断注数

        SLS.Lottery slsLottery = new SLS.Lottery();
        string[] t_lotterys = SplitLotteryNumber(LotteryNumber);

        if ((t_lotterys == null) || (t_lotterys.Length < 1))
        {
            Shove._Web.JavaScript.Alert(this.Page, "选号发生异常，请重新选择号码投注，谢谢。(-694)");

            return;
        }

        int ValidNum = 0;

        foreach (string str in t_lotterys)
        {
            string Number = slsLottery[LotteryID].AnalyseScheme(str, PlayTypeID);

            if (string.IsNullOrEmpty(Number))
            {
                continue;
            }

            string[] str_s = Number.Split('|');

            if (str_s == null || str_s.Length < 1)
            {
                continue;
            }

            ValidNum += Shove._Convert.StrToInt(str_s[str_s.Length - 1], 0);
        }

        if (ValidNum != SumNum)
        {
            Shove._Web.JavaScript.Alert(this.Page, "选号发生异常，请重新选择号码投注，谢谢。");

            return;
        }

        #endregion


        if (DateTime.Now >= Shove._Convert.StrToDateTime(HidIsuseEndTime.Replace("/", "-"), DateTime.Now.AddDays(-1).ToString()))
        {
            Shove._Web.JavaScript.Alert(this.Page, "本期投注已截止，谢谢。");

            return;
        }

        if (Price * SumNum * Multiple != SumMoney)
        {
            Shove._Web.JavaScript.Alert(this.Page, "输入有错误，请仔细检查。");

            return;
        }

        string ReturnDescription = "";

        long SchemeID = _User.InitiateScheme(IsuseID, PlayTypeID, tb_Title.Trim() == "" ? "(无标题)" : tb_Title.Trim(), tb_Description.Trim(), LotteryNumber, "", Multiple, SumMoney, AssureMoney, Share, BuyShare, tb_OpenUserList.Trim(), short.Parse(SecrecyLevel.ToString()), SchemeBonusScale, ref ReturnDescription);

        if (SchemeID < 0)
        {
            PF.GoError(ErrorNumber.Unknow, ReturnDescription, this.GetType().FullName + "(-755)");

            return;
        }

        Shove._Web.Cache.ClearCache("Home_Room_CoBuy_BindDataForType" + IsuseID.ToString());
        Shove._Web.Cache.ClearCache("Home_Room_SchemeAll_BindData" + IsuseID.ToString());

        if (SumMoney > 50 && Share > 1)
        {
            Shove._Web.Cache.ClearCache("Home_Room_JoinAllBuy_BindData");
        }

        Response.Redirect("../Home/Room/UserBuySuccess.aspx?LotteryID=" + LotteryID.ToString() + "&&Money=" + BuyMoney.ToString() + "&SchemeID=" + SchemeID.ToString() + "");

        return;

    }

    protected void btn_OK_Click(object sender, EventArgs e)
    {
        Buy(_User);
    }

    private string[] SplitLotteryNumber(string Number)
    {
        string[] s = Number.Split('\n');
        if (s.Length == 0)
            return null;
        for (int i = 0; i < s.Length; i++)
            s[i] = s[i].Trim();
        return s;
    }
}
