using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

public partial class Home_Room_SchemeUpload : RoomPageBase
{
    public string strLotteryNumber, strSchemeFileName, strLotteryID, strPlayType, strPlayTypeName, strPanelNum, strIsuse;
    public string strLotteryName;

    protected void Page_Load(object sender, System.EventArgs e)
    {
        if (!this.IsPostBack)
        {
            SLS.Lottery lottery = new SLS.Lottery();

            int LotteryID = Shove._Convert.StrToInt(Shove._Web.Utility.GetRequest("id"), -1);

            if (!lottery.ValidID(LotteryID))
            {
                return;
            }

            tbLotteryID.Value = LotteryID.ToString();
            strLotteryID = LotteryID.ToString();

            strIsuse = "";
            try
            {
                strIsuse = Shove._Web.Utility.GetRequest("Isuse");
            }
            catch
            { }
            tbIsuse.Value = strIsuse;

            int PlayType = Shove._Convert.StrToInt(Shove._Web.Utility.GetRequest("PlayType"), -1);
            if (PlayType < 0)
                return;

            bool PlayTypeRight = false;

            strLotteryName = lottery[LotteryID].name;
            PlayTypeRight = lottery[LotteryID].CheckPlayType(PlayType);

            if (!PlayTypeRight)
                return;

            tbPlayType.Value = PlayType.ToString();
            strPlayType = PlayType.ToString();

            tbSchemeFileName.Value = "null";
            tbLotteryNumber.Value = "null";
            strSchemeFileName = "null";
            strLotteryNumber = "null";
        }
    }

    #region Web 窗体设计器生成的代码

    override protected void OnInit(EventArgs e)
    {
        RequestLoginPage = this.Request.Url.AbsoluteUri;

        isRequestLogin = false;
        isAtFramePageLogin = false;

        base.OnInit(e);
    }

    #endregion

    protected void btnfileUp_Click(object sender, System.EventArgs e)
    {
        string UploadFileName = btnfile.Value;

        if (String.IsNullOrEmpty(UploadFileName))
        {
            Shove._Web.JavaScript.Alert(this.Page, "请先选择一个文件再上传。");

            return;
        }

        if (!UploadFileName.Trim().ToLower().EndsWith(".txt"))
        {
            Shove._Web.JavaScript.Alert(this.Page, "只能上传 .txt 文本类型的文件。");

            return;
        }

        tbSchemeFileName.Value = "null";
        tbLotteryNumber.Value = "null";
        strSchemeFileName = "null";
        strLotteryNumber = "null";

        strPlayTypeName = new SLS.Lottery().GetPlayTypeName(int.Parse(tbPlayType.Value));
        strLotteryName = new SLS.Lottery()[int.Parse(tbLotteryID.Value)].name;

        string NewFileName = "";

        if (Shove._IO.File.UploadFile(this.Page, btnfile, "../../Temp/", ref NewFileName, "text") != 0)
        {
            Shove._Web.JavaScript.Alert(this.Page, "方案上传失败。");

            return;
        }

        string FileName = this.Server.MapPath("../../Temp/" + NewFileName);

        string Content = Shove._Convert.ToDBC(System.IO.File.ReadAllText(FileName, System.Text.Encoding.Default)).Trim();

        if (Content == "")
        {
            System.IO.File.Delete(FileName);
            Shove._Web.JavaScript.Alert(this.Page, "方案文件没有任何内容，请重新选择。");

            return;
        }

        tbSchemeFileName.Value = NewFileName;
        strSchemeFileName = NewFileName;

        //分析
        int LotteryID = Shove._Convert.StrToInt(tbLotteryID.Value, -1);
        if (!new SLS.Lottery().ValidID(LotteryID))
        {
            System.IO.File.Delete(FileName);

            tbSchemeFileName.Value = "null";
            strSchemeFileName = "null";

            Shove._Web.JavaScript.Alert(this.Page, "方案上传失败。");

            return;
        }

        int PlayType = int.Parse(tbPlayType.Value);
        if (LotteryID == 61)
        {
            Content = FmtContent(Content);
        }

        tbLotteryNumber.Value = new SLS.Lottery()[LotteryID].AnalyseScheme(Content, PlayType);

        strLotteryNumber = tbLotteryNumber.Value.Trim();

        string[] Schemes = strLotteryNumber.Split(new String[] { "\n" }, StringSplitOptions.None);
        strLotteryNumber = "";

        foreach (string s in Schemes)
        {
            if (s.Split('|').Length > 2)
            {
                strLotteryNumber += s.Substring(0, s.LastIndexOf("|")).Trim();
            }
            else
            {
                strLotteryNumber += s.Split('|')[0];
            }
        }
        if (strLotteryNumber == "")
        {
            System.IO.File.Delete(FileName);

            tbLotteryNumber.Value = "null";
            strLotteryNumber = "null";
            tbSchemeFileName.Value = "null";
            strSchemeFileName = "null";

            Shove._Web.JavaScript.Alert(this.Page, "从方案文件中没有提取到符合书写规则的投注内容。");
        }
        else
        {
            System.IO.File.Delete(FileName);

            if (strLotteryNumber.Replace(" ", "").Replace("\n", "") != Content.Replace(" ", "").Replace("\n", "").Replace("\r", "").Replace("\r\n", ""))
            {
                Shove._Web.JavaScript.Alert(this.Page, "过滤掉了您上传方案中不符合格式的投注方案，请核对！");
            }

            //Shove._Web.JavaScript.Alert(this.Page, "方案上传成功。（注：系统只从方案中提取遵循书写规则的投注内容，如果系统提取的结果与您方案文件不一致，请检查方案文件是否完全遵循了书写规则）");
        }
    }

    //时时彩上传方案缺位"-"自动补齐
    private string FmtContent(string content)
    {
        string []temp = new string[]{};
        string newContent = "";
        content.Replace("-", "");
        string[] list = content.Split(new String[] { "\r\n" }, StringSplitOptions.None);
        for (int i = 0; i < list.Length; i++)
        {
            if (list[i].IndexOf("(") >= 0 && list[i].IndexOf(")") >= 0)
            {
                int endChar = list[i].IndexOf(")");
                int beginChar = list[i].IndexOf("(");
                if ((list[i].Length-((endChar - beginChar) +1)+1) == 5)
                {
                    newContent +=list[i] + "\r\n";
                }
                else
                {
                    newContent += GetFmtString(5 - (list[i].Length-((endChar - beginChar) +1)+1)) + list[i] + "\r\n";
                }
            }
            else
            {
                newContent += GetFmtString(5-list[i].Length)+list[i]+"\r\n";
            }
        }
        return newContent.Substring(0,newContent.LastIndexOf("\r\n"));
    }
    //产生上传方案中的"-"
    private string GetFmtString(int charCount)
    {
        string temp = "";
        for (int i = 0; i < charCount; i++)
        {
            temp += "-";
        }
        return temp;
    }
}