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
using System.Security.Cryptography;
using System.Text;

public partial class Home_Room_PromoteUserReg : SitePageBase
{

    private string KeyPromotionUserID = "SLS.TWZT.PromotionUserID";
    private string KeyHongbaoPromotionID = "SLS.TWZT.HongbaoPromotionID";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            if (_User != null)
            {
                Response.Redirect("/Default.aspx");
            }
            bool isUseCheckCode = _Site.SiteOptions["Opt_isUseCheckCode"].ToBoolean(true);
            //CheckCode1.Visible = isUseCheckCode;
            //CheckCode2.Visible = isUseCheckCode;

            //new Login().SetCheckCode(_Site, ShoveCheckCode1);
            //new Login().SetCheckCode(_Site, ShoveCheckCode2);

            if (_User != null)
            {
                Response.Redirect("../../Default.aspx");
            }

            //处理用户推荐
            pnlShowInfoWithHongbao.Visible = false;
            pnlShowInfoWithoutHongbao.Visible = false;
            pnlShowInfoPromotion.Visible = false;

            SetCommenderName("");
            SetCommenderGifMoney("0");

            long commenderID = -1;
            long hongbaoPromotionID = -1;
            string paramID = Shove._Web.Utility.GetRequest("id");
            string sign = Shove._Web.Utility.GetRequest("Sign"); //签名串
            if (string.IsNullOrEmpty(sign) && !string.IsNullOrEmpty(paramID)) //"普通推荐会员"
            {
                if (paramID.Length != 11)
                {
                    pnlShowInfoPromotion.Visible = true;
                    lblShowInfoPromotion.Text = "无效的推荐人ID.用户可以正常注册会员!";
                }
                else
                {
                    pnlShowInfoWithoutHongbao.Visible = true;
                    lblAgreeTip.Visible = false;
                    commenderID = Shove._Convert.StrToLong(paramID.Substring(0, 10), -1);
                    object tempOjb = Shove.Database.MSSQL.ExecuteScalar("select name from T_Users where ID=" + commenderID, new Shove.Database.MSSQL.Parameter[0]);
                    if (tempOjb == null || tempOjb.ToString() == "")
                    {
                        pnlShowInfoPromotion.Visible = true;
                        lblShowInfoPromotion.Text = "不存在推荐人的ID.用户可以正常注册会员!";
                    }
                    else
                    {
                        SetCommenderName(tempOjb.ToString());
                    }

                }

            }
            else //红包推广
            {
                if (new SynchronizeSessionID(this).ValidSign(this.Request))//验证URL签名
                {
                    commenderID = Shove._Convert.StrToLong(Shove._Web.Utility.GetRequest("UserID"), -1);
                    hongbaoPromotionID = Shove._Convert.StrToLong(Shove._Web.Utility.GetRequest("id"), -1);
                    //判断当前红包推荐是否可用
                    DataTable dtPromotion = Shove.Database.MSSQL.Select("select * from T_UserHongbaoPromotion where ID=" + hongbaoPromotionID + " and UserID=" + commenderID, new Shove.Database.MSSQL.Parameter[0]);

                    if (dtPromotion != null && dtPromotion.Rows.Count > 0)
                    {
                        string commenderName = "";  //推荐人姓名
                        long acceptUserID = -1;     //检查是否存在已经使用用户ID
                        DateTime ExpiryDate;        //红包链接有效期
                        double hongbaoMoney = 0;    //红包钱数
                        double commenderBalance = 0;//红包钱数

                        commenderName = Shove.Database.MSSQL.ExecuteScalar("select name from T_Users where ID=" + commenderID, new Shove.Database.MSSQL.Parameter[0]).ToString();
                        acceptUserID = Shove._Convert.StrToLong(dtPromotion.Rows[0]["AcceptUserID"].ToString(), -1);
                        ExpiryDate = dtPromotion.Rows[0]["ExpiryDate"] == null ? DateTime.MinValue : Convert.ToDateTime(dtPromotion.Rows[0]["ExpiryDate"]);
                        hongbaoMoney = Shove._Convert.StrToDouble(dtPromotion.Rows[0]["Money"].ToString(), 0);
                        object objBalance = Shove.Database.MSSQL.ExecuteScalar("select Balance from T_Users where ID=" + commenderID.ToString(), new Shove.Database.MSSQL.Parameter[0]);
                        commenderBalance = Shove._Convert.StrToDouble(objBalance.ToString(), 0);
                        //检查是否已经被他人使用
                        if (acceptUserID > 0)
                        {
                            pnlShowInfoPromotion.Visible = true;
                            lblShowInfoPromotion.Text = "欢迎您通过你的好友 " + commenderName + " 的推荐来到" + _Site.Name + "上购彩中心，此推荐链接送出的红包已经被他人领取。您可以继续注册用户，但不会获得推荐人送出的彩票红包。但我们热情期待您加入,共同博击1000万的大奖!";
                            lblAgreeTip.Visible = false;
                        }
                        //检查此红包推荐链接是否过期
                        else if (ExpiryDate < DateTime.Now)
                        {
                            pnlShowInfoPromotion.Visible = true;
                            lblShowInfoPromotion.Text = "欢迎您通过你的好友 " + commenderName + " 的推荐来到" + _Site.Name + "上购彩中心，由于此推荐注册链接已经过期,继续注册不会获得推荐者送出的红包.但我们热情期待您加入,共同博击1000万的大奖!";
                            lblAgreeTip.Visible = false;
                        }
                        //检查推荐人余额
                        else if (commenderBalance < hongbaoMoney)
                        {
                            pnlShowInfoPromotion.Visible = true;
                            lblShowInfoPromotion.Text = "欢迎您通过你的好友 " + commenderName + " 的推荐来到" + _Site.Name + "上购彩中心，由于推荐人余额不足,继续注册不会获得推荐者送出的红包.但我们热情期待您加入,共同博击1000万的大奖!";
                            lblAgreeTip.Visible = false;
                        }
                        else
                        {
                            pnlShowInfoWithHongbao.Visible = true;
                            SetCommenderName(Shove.Database.MSSQL.ExecuteScalar("select name from T_Users where ID=" + commenderID, new Shove.Database.MSSQL.Parameter[0]).ToString());
                            SetCommenderGifMoney(hongbaoMoney.ToString());
                        }
                    }
                    else
                    {
                        pnlShowInfoPromotion.Visible = true;
                        lblShowInfoPromotion.Text = "无效的推荐链接。我们热情期待您加入,共同博击1000万的大奖!";
                    }
                }
                else
                {
                    pnlShowInfoPromotion.Visible = true;
                    lblShowInfoPromotion.Text = "红包推广推荐链接已被他人修改过。我们热情期待您加入,共同博击1000万的大奖!";
                }
            }

            if (commenderID > 0)
            {
                // 把推荐会员ID写入Session
                Session[KeyPromotionUserID] = commenderID.ToString();
                if (hongbaoPromotionID > 0)
                {
                    Session[KeyHongbaoPromotionID] = hongbaoPromotionID.ToString();
                }
                else
                {
                    Session[KeyHongbaoPromotionID] = null;
                }

            }
            else
            {
                pnlShowInfoPromotion.Visible = true;
                lblShowInfoPromotion.Text = "无效的推荐链接。继续注册，我们热情期待您加入,共同博击1000万的大奖!";
                return;
            }
        }
    }

    protected void btnReg_Click(object sender, EventArgs e)
    {
        if (tbRegCheckCode.Text.Trim() == "")
        {
            Shove._Web.JavaScript.Alert(this.Page, "请输入验证码！");

            return;
        }

        string RegCode = tbRegCheckCode.Text.Trim().ToLower();

        if (Shove._Web.Cache.GetCacheAsString("CheckCode_" + Request.Cookies["ASP.NET_SessionId"].Value, "") != Shove._Security.Encrypt.MD5(PF.GetCallCert() + RegCode))
        {
            Shove._Web.JavaScript.Alert(this.Page, "验证码输入错误，请重新输入！");

            return;
        }
        // 通过验证

        long CpsID = -1;
        long CommenderID = -1;
        long HongbaoPromotionID = -1;

        if (Session[KeyPromotionUserID] != null)
        {
            CommenderID = Shove._Convert.StrToLong(Session[KeyPromotionUserID].ToString(), -1);
        }
        if (Session[KeyHongbaoPromotionID] != null)
        {
            HongbaoPromotionID = Shove._Convert.StrToLong(Session[KeyHongbaoPromotionID].ToString(), -1);
        }

        //检查推荐人是否为CPS商家,是就把此会员标记CSPID
        object tempOjb = Shove.Database.MSSQL.ExecuteScalar("select ID from T_Cps where OwnerUserID=" + CommenderID, new Shove.Database.MSSQL.Parameter[0]);
        if (tempOjb != null)
        {
            CpsID = Shove._Convert.StrToLong(tempOjb.ToString(), -1);
        }

        System.Threading.Thread.Sleep(500);

        string Name = TBUserName.Text.Trim();
        string Password = TBPwdOne.Text.Trim();

        Users user = new Users(_Site.ID);
        user.Name = Name;
        user.Password = Password;
        user.UserType = 2;

        if (CpsID > 0)//推荐人为cps商家就填CpsID字段
        {
            user.CommenderID = -1;
            user.CpsID = CpsID;
        }
        else
        {
            user.CommenderID = CommenderID;
            user.CpsID = -1;
        }

        //添加会员
        string ReturnDescription = "";
        int Result = user.Add(ref ReturnDescription);
        if (Result < 0)
        {
            Shove._Web.JavaScript.Alert(this, ReturnDescription);
            return;
        }


        string regReultInfo = "<span style='font-size: 14px; color: #CC3300; font-weight: bold;'>注册成功,恭喜您成为" + _Site.Name + "的高级会员!让我们一起共同搏击1000万大奖!</span><br/>";

        //注册成功,领取推荐人的红包
        if (HongbaoPromotionID > 0)
        {
            //判断当前红包推荐是否可用
            DataTable dtPromotion = Shove.Database.MSSQL.Select("select * from T_UserHongbaoPromotion where ID=" + HongbaoPromotionID + " and UserID=" + CommenderID, new Shove.Database.MSSQL.Parameter[0]);
            if (dtPromotion != null && dtPromotion.Rows.Count > 0)
            {
                double hongbaoMoney = 0;    //红包钱数
                hongbaoMoney = Shove._Convert.StrToDouble(dtPromotion.Rows[0]["Money"].ToString(), 0);
                int returnValue = -1;
                DAL.Procedures.P_AcceptUserHongbaoPromotion(CommenderID, user.ID, HongbaoPromotionID, ref returnValue, ref ReturnDescription);
                if (returnValue != 0)
                {
                    regReultInfo = "<span style='font-size: 14px; color: #CC3300; font-weight: bold;'>注册成功,恭喜您成为" + _Site.Name + "的高级会员!</span><br/>由以下原因未能获得推荐者的彩票红包：" + ReturnDescription + "<br/>";
                }
                else
                {
                    regReultInfo = "<span style='font-size: 14px; color: #CC3300; font-weight: bold;'>注册成功,恭喜您成为" + _Site.Name + "的高级会员,并获得" + hongbaoMoney.ToString() + "元彩票红包!" + hongbaoMoney.ToString() + "元已注入您的" + _Site.Name + "现金帐户,您可以到我们的购彩页面购买彩票啦,祝君好运!" + "</span><br/>";
                }
            }
        }


        //登录
        Result = user.Login(ref ReturnDescription);

        if (Result < 0)
        {
            Shove._Web.JavaScript.Alert(this, ReturnDescription);

            return;
        }

        //显示注册结果信息
        //MultiView1.ActiveViewIndex = 1;
        //divRegResultInfo.InnerHtml = regReultInfo;

        #region
        //long CpsID = -1;
        //long CommenderID = -1;
        //string Memo = "";

        //FirstUrl firstUrl = new FirstUrl();
        //string URL = firstUrl.Get();

        //if (!URL.StartsWith("http://"))
        //{
        //    URL = "http://" + URL;
        //    URL = URL.Split('?'.ToString().ToCharArray())[0];
        //}

        //DataTable dt = new DAL.Tables.T_Cps().Open("id, [ON], [Name]", "SiteID = " + _Site.ID.ToString() + " and( DomainName = '" + URL + "' or DomainName='" + Shove._Web.Utility.GetUrl() + "')", "");

        //if (Shove._Convert.StrToLong(firstUrl.CpsID, -1) > 0) //读取第一次访问页面时保存的CPS ID
        //{
        //    CpsID = Shove._Convert.StrToLong(firstUrl.CpsID, -1);
        //}
        //else if ((dt != null) && (dt.Rows.Count > 0))
        //{
        //    CpsID = Shove._Convert.StrToLong(dt.Rows[0]["ID"].ToString(), -1);
        //    Memo = firstUrl.PID;//联盟商推广URL的PID
        //}

        //System.Threading.Thread.Sleep(500);

        //string Name = TBUserName.Text.Trim();
        //string Password = TBPwdOne.Text.Trim();
        //string Password2 = TBPwdTwo.Text.Trim();

        //Users user = new Users(_Site.ID);

        //user.Name = Name;
        //user.Password = Password;
        //user.UserType = 2;

        //dt = new DAL.Tables.T_Users().Open("", "id=" + Shove._Web.Utility.GetRequest("CID") + " and Name='" + Shove._Web.Utility.GetRequest("CN") + "'", "");

        //if ((dt != null) && (dt.Rows.Count == 1))
        //{
        //    CommenderID = Shove._Convert.StrToLong(Shove._Web.Utility.GetRequest("CID"), -1);
        //}

        //user.CommenderID = CommenderID;
        //user.CpsID = CpsID;
        //user.Memo = Memo;

        //string ReturnDescription = "";
        //int Result = user.Add(ref ReturnDescription);

        //if (Result < 0)
        //{
        //    new Log("Users").Write("会员注册不成功：" + ReturnDescription);
        //    Shove._Web.JavaScript.Alert(this, ReturnDescription);

        //    return;
        //}

        //Result = user.Login(ref ReturnDescription);

        //if (Result < 0)
        //{
        //    new Log("Users").Write("注册成功后登录失败：" + ReturnDescription);
        //    Shove._Web.JavaScript.Alert(this, ReturnDescription);

        //    return;
        //}
        #endregion
        Response.Redirect("/Home/Room/UserRegSuccess.aspx");
    }


    #region Tool
    private void SetCommenderName(string name)
    {
        lblCommenderName1.Text = name;
        lblCommenderName2.Text = name;
        lblCommenderName3.Text = name;
    }

    private void SetCommenderGifMoney(string moneyValue)
    {
        lblGifMoney1.Text = moneyValue;
    }
    #endregion
}
