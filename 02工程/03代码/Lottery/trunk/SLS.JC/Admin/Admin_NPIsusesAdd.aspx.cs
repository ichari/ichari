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

public partial class Admin_Admin_NPIsusesAdd : AdminPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            HidID.Value = Shove._Web.Utility.GetRequest("ID");

            if (!string.IsNullOrEmpty(HidID.Value))
            {
                DataTable dt = new DAL.Tables.T_NewsPaperIsuses().Open("", "ID=" + HidID.Value, "");

                if (dt == null || dt.Rows.Count == 0)
                {
                    PF.GoError(ErrorNumber.DataReadWrite, "期号不存在！", this.GetType().BaseType.FullName);

                    return;
                }

                DataRow dr = dt.Rows[0];
                tbStartTime.Text = dr["StartTime"].ToString();
                tbEndTime.Text = dr["EndTime"].ToString();
                tbIsuse.Text = dr["Name"].ToString();
                tbContent.Value = dr["NPMessage"].ToString();
                btnAdd.Text = "修改";
            }
        }
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        int Isuse = 0;
        try
        {
            Isuse = Shove._Convert.StrToInt(tbIsuse.Text.Trim(), 0);
        }
        catch { }

        if (Isuse == 0)
        {
            Shove._Web.JavaScript.Alert(this.Page, "彩友报期号只能是整数！");

            return;
        }

        System.DateTime StartTime, EndTime;

        try
        {
            StartTime = Convert.ToDateTime(tbStartTime.Text);
        }
        catch
        {
            Shove._Web.JavaScript.Alert(this.Page, "开始时间格式输入错误！");

            return;
        }

        try
        {
            EndTime = Convert.ToDateTime(tbEndTime.Text);
        }
        catch
        {
            Shove._Web.JavaScript.Alert(this.Page, "截止时间格式输入错误！");

            return;
        }

        if (EndTime < StartTime)
        {
            Shove._Web.JavaScript.Alert(this.Page, "截止时间应该在开始时间之后！");

            return;
        }

        string Message = Shove._Convert.ToTextCode(tbContent.Value.Trim());

        if (Message == "")
        {
            Shove._Web.JavaScript.Alert(this.Page, "请输入开奖信息！");

            return;
        }

        if (HidID.Value == "")
        {
            DataTable dt = new DAL.Tables.T_NewsPaperIsuses().Open("[ID]", "[Name] = '" + Isuse.ToString().PadLeft(tbIsuse.Text.Length, '0') + "'", "");

            if (dt == null)
            {
                PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", "Admin_Admin_NPIsusesAdd");

                return;
            }

            if (dt.Rows.Count > 0)
            {
                Shove._Web.JavaScript.Alert(this.Page, "期号已经存在，请不要输入重名期号！");

                return;
            }

            DAL.Tables.T_NewsPaperIsuses dt1 = new DAL.Tables.T_NewsPaperIsuses();

            dt1.Name.Value = Isuse.ToString().PadLeft(tbIsuse.Text.Length, '0');
            dt1.StartTime.Value = StartTime.ToString("yyyy-MM-dd");
            dt1.EndTime.Value = EndTime.ToString("yyyy-MM-dd");
            dt1.NPMessage.Value = Message;

            if (dt1.Insert() < 0)
            {
                Shove._Web.JavaScript.Alert(this.Page, "添加彩友报期号失败！");
                return;
            }
            else
            {
                Shove._Web.Cache.ClearCache("Home_Room_NewsPaper_BindNewsPaper_" + this.HidID.Value);

                Shove._Web.JavaScript.Alert(this.Page, "添加期号成功！");
            }
        }
        else
        {
            DataTable dt = new DAL.Tables.T_NewsPaperIsuses().Open("[ID]", "[Name] = '" + Isuse.ToString().PadLeft(tbIsuse.Text.Length, '0') + "' and ID<>" + HidID.Value + "", "");

            if (dt == null)
            {
                PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", "Admin_IsuseAdd");

                return;
            }

            if (dt.Rows.Count > 0)
            {
                Shove._Web.JavaScript.Alert(this.Page, "期号已经存在，请不要输入重名期号！");

                return;
            }

            DAL.Tables.T_NewsPaperIsuses dt1 = new DAL.Tables.T_NewsPaperIsuses();

            dt1.Name.Value = Isuse.ToString().PadLeft(tbIsuse.Text.Length, '0');
            dt1.StartTime.Value = StartTime;
            dt1.EndTime.Value = EndTime;
            dt1.NPMessage.Value = Message;

            if (dt1.Update("ID=" + HidID.Value) < 0)
            {
                Shove._Web.JavaScript.Alert(this.Page, "修改失败！");

                return;
            }

            Shove._Web.Cache.ClearCache("Home_Room_NewsPaper_BindNewsPaper_" + this.HidID.Value);

            Shove._Web.JavaScript.Alert(this.Page, "修改成功！");
        }

       
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        this.Response.Redirect("Admin_NewsPaper.aspx");
    }
}
