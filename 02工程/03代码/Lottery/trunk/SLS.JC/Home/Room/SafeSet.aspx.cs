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

public partial class Home_Room_SafeSet : RoomPageBase
{
    string key = "";
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!this.IsPostBack)
        {
            BindData();
            ddlQuestion.DataSource = DataCache.SecurityQuestions;
            ddlQuestion.DataBind();
            ShowEditQF();
            //判断是不是从邮箱点击验证过来的
            key = Shove._Web.Utility.GetRequest("Qkey").Trim();
            if (key != "")
            {
                GetUserQuestionAnswer();
            }
        }
    }

    private void GetUserQuestionAnswer()
    {
        System.Threading.Thread.Sleep(500);
        if (key == "" || key.Length <= 32)
        {
            Shove._Web.JavaScript.Alert(this.Page, "链接地址不合法,请核实.", "AccountDetail.aspx");

            return;
        }


        string sign = key.Substring(0, 16) + key.Substring(key.Length - 16, 16);

        key = key.Substring(16, key.Length - 32);

        try
        {
            if (sign != BitConverter.ToString(new MD5CryptoServiceProvider().ComputeHash(Encoding.UTF8.GetBytes(key))).Replace("-", ""))
            {
                Shove._Web.JavaScript.Alert(this.Page, "链接地址不合法,请核实", "AccountDetail.aspx");

                return;
            }
            key = Shove._Security.Encrypt.UnEncryptString(PF.GetCallCert(), key);

            long userID = -1;
            DateTime time = DateTime.Now.AddYears(-1);
            string question = "";
            string answer = "";
            string userName = "";
            int validedvount = 1;
            try
            {
                userID = Shove._Convert.StrToLong(key.Split(',')[0], 0);
                time = Convert.ToDateTime(key.Split(',')[1]);
                question = key.Split(',')[2];
                answer = key.Split(',')[3];
                userName = key.Split(',')[4];
                validedvount = Shove._Convert.StrToInt(key.Split(',')[5], 1);
            }
            catch { }

            if (userID != _User.ID || userName != _User.Name)
            {
                Shove._Web.JavaScript.Alert(this.Page, "登陆账号与所申请的账号不一致,请核实.", "../../UserLogin.aspx");

                return;
            }

            if (validedvount != 0)
            {
                Shove._Web.JavaScript.Alert(this.Page, "该地址已过期.", "AccountDetail.aspx");

                return;

            }
            DataTable dt = new DAL.Tables.T_UserEditQuestionAnswer().Open("QuestionAnswerState","UserID = " + _User.ID,"");
            if (dt.Rows.Count == 0)
            {
                Shove._Web.JavaScript.Alert(this.Page, "该地址已过期.", "AccountDetail.aspx");

                return;
            }
            else
            {
                if (dt.Rows[0][0].ToString() != "0")
                {
                    Shove._Web.JavaScript.Alert(this.Page, "该地址已过期.", "AccountDetail.aspx");

                    return;

                }
            }


            if (time.AddDays(1).CompareTo(DateTime.Now) < 0)
            {
                Shove._Web.JavaScript.Alert(this.Page, "该地址已过期.", "AccountDetail.aspx");

                return;
            }

            if (userID <= 0)
            {
                Shove._Web.JavaScript.Alert(this.Page, "非法访问", "../../UserLogin.aspx");

                return;
            }


            ShowEditQF2();
        }
        catch
        {
            Shove._Web.JavaScript.Alert(this.Page, "非法访问。", "../../UserLogin.aspx");
        }

    }

    #region Web 窗体设计器生成的代码

    override protected void OnInit(EventArgs e)
    {
        RequestLoginPage = this.Request.Url.AbsoluteUri;


        base.OnInit(e);
    }

    #endregion

    private void BindData()
    {
        tbName.Text = _User.Name;
        string fromUrl = Shove._Web.Utility.GetRequest("FromUrl");

        hdFromUrl.Value = fromUrl;

        if (_User.SecurityQuestion == "")
        {
            trOldAns.Visible = false;
            trOldQue.Visible = false;
        }
        else
        {
            lbOQuestion.Text = _User.SecurityQuestion;
            trOldAns.Visible = true;
            trOldQue.Visible = true;
        }
    }

    protected void btnOK_Click(object sender, EventArgs e)
    {
        string Question = ddlQuestion.SelectedValue;
        if (trOldQue.Visible == true)
        {
            if (tbOAnswer.Text.Trim() != _User.SecurityAnswer)
            {
                Shove._Web.JavaScript.Alert(this.Page, "原安全问题回答错误");

                return;
            }
        }
        

        if (Question == "自定义问题")
        {
            Question = Shove._Web.Utility.FilteSqlInfusion(tbMyQuestion.Text.Trim());

            if (Question == "")
            {
                Shove._Web.JavaScript.Alert(this.Page, "请输入安全问题");

                return;
            }

            Question = "自定义问题|" + Question;
        }
        else
        {
            Question = ddlQuestion.SelectedValue;
        }

        string Answer = Shove._Web.Utility.FilteSqlInfusion(tbAnswer.Text.Trim());

        if (Answer == "")
        {
            Shove._Web.JavaScript.Alert(this.Page, "请输入答案");

            return;
        }

        DAL.Tables.T_Users user = new DAL.Tables.T_Users();

        user.SecurityQuestion.Value = Question;
        user.SecurityAnswer.Value = Answer;

        long Result = user.Update("ID=" + _User.ID.ToString());

        if (Result < 0)
        {
            Shove._Web.JavaScript.Alert(this.Page, "设置安全问题失败");

            return;
        }
        //修改验证状态
        DAL.Tables.T_UserEditQuestionAnswer T_QF = new DAL.Tables.T_UserEditQuestionAnswer();
        string ReturnDescription = "";
        T_QF.QuestionAnswerState.Value = 1;

        Result = T_QF.Update("UserID=" + _User.ID);
        if (Result < 0)
        {
            PF.GoError(-1, ReturnDescription, this.GetType().FullName);

            return;
        }

        Response.Write("<script type='text/javascript'>alert('设置安全问题成功。请注意安全保护问题是最重要的安全凭证，为了您的安全，请牢牢记住您的安全保护问题。');window.location='" + this.hdFromUrl.Value + "'</script>");
        Response.End();

    }
    protected void ddlQuestion_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlQuestion.SelectedValue == "自定义问题")
        {
            trMQ.Visible = true;
        }
        else
        {
            trMQ.Visible = false;
        }
    }
    protected void btnGoEmail_Click(object sender, EventArgs e)
    {
        string passWord = Shove._Web.Utility.FilteSqlInfusion(tbPassWord.Text.ToString());
        string Email = _User.Email;
        string RealityName = Shove._Web.Utility.FilteSqlInfusion(tbRealityName.Text.ToString());
        string Question = _User.SecurityQuestion;
        string Answer = _User.SecurityAnswer;
        string userName = _User.Name;
        int ValidedCount = 0;
        if (RealityName == "")
        {
            Shove._Web.JavaScript.Alert(this.Page, "请输入真实姓名。");

            return;
        }

        if (RealityName != _User.RealityName)
        {
            Shove._Web.JavaScript.Alert(this.Page, "真实姓名输入有误,请核实。");

            return;
        }

        if (passWord == "")
        {
            Shove._Web.JavaScript.Alert(this.Page, "请输入密码。");

            return;
        }

        if (PF.EncryptPassword(passWord) != _User.Password)
        {
            Shove._Web.JavaScript.Alert(this.Page, "您输入的密码有误,请核实。");

            return;
        }

        if (Question == "" || Answer == "")
        {
            Shove._Web.JavaScript.Alert(this.Page, "您还未设置安全问题,无需重置。");

            return;
        }

        DataTable dt = new DAL.Tables.T_UserEditQuestionAnswer().Open("", "UserID=" + _User.ID, "");
        //实例化T_UserEditQuestionAnswer表
        DAL.Tables.T_UserEditQuestionAnswer T_QF = new DAL.Tables.T_UserEditQuestionAnswer();
        long Result = -1;
        string ReturnDescription = "";
        if (dt.Rows.Count > 0)
        {
            if (Shove._Convert.StrToDateTime(dt.Rows[0]["DateTime"].ToString(), "0000-00-00").ToString("yyyyMMdd") == DateTime.Now.ToString("yyyyMMdd"))
            {
                if (dt.Rows[0]["ValidedCount"].ToString() == "2")
                {
                    Shove._Web.JavaScript.Alert(this.Page, "您今天已重置两次安全问题了,请明天再来吧", "AccountDetail.aspx");

                    return;
                }
                else
                {
                    ValidedCount = Shove._Convert.StrToInt(dt.Rows[0]["ValidedCount"].ToString(), 1) + 1;
                }

            }
            else
            {
                ValidedCount = 1;
            }
            T_QF.ValidedCount.Value = ValidedCount;
            T_QF.QuestionAnswerState.Value = 0;
            Result = T_QF.Update("UserID=" + _User.ID);
            if (Result < 0)
            {
                PF.GoError(-1, ReturnDescription, this.GetType().FullName);

                return;
            }
        }
        else
        {
            T_QF.UserID.Value = _User.ID;
            T_QF.QuestionAnswerState.Value = 0;
            T_QF.ValidedCount.Value = 1;
            Result = T_QF.Insert();
            if (Result < 0)
            {
                PF.GoError(-1, ReturnDescription, this.GetType().FullName);

                return;
            }
        }

        string key = Shove._Security.Encrypt.EncryptString(PF.GetCallCert(), _User.ID.ToString() + "," + DateTime.Now.ToString() + "," + Question + "," + Answer + "," + userName + "," + T_QF.QuestionAnswerState.Value);
        MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
        string sign = BitConverter.ToString(md5.ComputeHash(Encoding.UTF8.GetBytes(key))).Replace("-", "");
        key = sign.Substring(0, 16) + key + sign.Substring(16, 16);
        string url = Shove._Web.Utility.GetUrl() + "/Home/Room/SafeSet.aspx?Qkey="+key;
        StringBuilder sb = new StringBuilder();

        sb.Append("<div style='font-weight:bold;'>尊敬的"+_Site.Name+"客户(").Append(_User.Name).Append("):</div>")
            .Append("<div>您好!</div>")
            .Append("<div>系统已收到您的安全问题重置，请点击链接<a href='").Append(url).Append("' target='_top'>").Append(url).Append("</a>校验您的身份。</div>")
            .Append("<div>为了您的安全，该邮件通知地址将在 24 小时后失效，谢谢合作。</div>")
            .Append("<div>此邮件由系统发出，请勿直接回复!</div>")
            .Append("<div>").Append(Shove._Web.Utility.GetUrlWithoutHttp()).Append(" 版权所有(C) 2008-2009</div>");

        if (PF.SendEmail(_Site, Email, "安全问题找回", sb.ToString()) == 0)
        {

            tbPassWord.Enabled = false;
            tbRealityName.Enabled = false;
            btnGoEmail.Enabled = false;
            lblTips.Text = "&nbsp;&nbsp;&nbsp;&nbsp;您好，系统已经发送一封验证邮件您的邮箱，请到您的信箱确认。";
        }
        else
        {
            new Log("System").Write(this.GetType().FullName + "发送邮件失败");
        }

    }
    /// <summary>
    /// 显示修改QF时的控件
    /// </summary>
    private void ShowEditQF()
    {
        tbOldQF.Visible = true;
        tbNewQF.Visible = true;
        Panel1.Visible = true;
        Panel2.Visible = false;
        tbUserRName.Visible = false;
    }

    /// <summary>
    /// 显示修改QF时的控件
    /// </summary>
    private void ShowEditQF2()
    {
        trOldQue.Visible = false;
        tbOldQF.Visible = false;
        tbNewQF.Visible = true;
        Panel1.Visible = true;
        Panel2.Visible = false;
        tbUserRName.Visible = false;
    }
    /// <summary>
    /// 显示 去Email绑定时 的 控件
    /// </summary>
    private void ShowGoEmail()
    {
        tbOldQF.Visible = false;
        tbNewQF.Visible = false;
        Panel1.Visible = false;
        Panel2.Visible = true;
        tbUserRName.Visible = true;
    }
    protected void btnGoReset_Click(object sender, EventArgs e)
    {
        if (!_User.isEmailValided)
        {
            Shove._Web.JavaScript.Alert(this.Page, "您还未绑定邮箱,请绑定后在进行安全问题的重置。", "UserEmailBind.aspx");

            return;
        }
        ShowGoEmail();
    }
    protected void btnReturn_Click(object sender, EventArgs e)
    {
        if (!btnGoEmail.Enabled)
        {
            Response.Redirect("AccountDetail.aspx");
        }
        ShowEditQF();
    }
}
