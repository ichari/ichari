using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_CardPasswordAgentsAdd : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btnOK_Click(object sender, EventArgs e)
    {
        int AgentNo = Shove._Convert.StrToInt(tbAgentNO.Text, 0);
        if (AgentNo < 1000 || AgentNo > 9999)
        {
            Shove._Web.JavaScript.Alert(this.Page, "请输入4位长度的编号");
            return;
        }
        string AgentName = Shove._Web.Utility.FilteSqlInfusion(tbAgentName.Text);
        if (AgentName == "")
        {
            Shove._Web.JavaScript.Alert(this.Page, "请输入用户名");
            return;
        }

        double Money = Shove._Convert.StrToDouble(Shove._Web.Utility.FilteSqlInfusion(tbMoney.Text), 0);
        if (Money <= 0)
        {
            Shove._Web.JavaScript.Alert(this.Page, "请输入金额");
            return;
        }

        string password = tbAgentPassword.Text;
        if (password == "" || password.Length < 6)
        {
            Shove._Web.JavaScript.Alert(this.Page, "请输入至少6位长度的密码");
            return;
        }

        string sql = "INSERT INTO [T_CardPasswordAgents]([ID],[Name],[Password],[Company],[Url],[State],[Balance])VALUES(" + AgentNo + ",'" + AgentName + "','" + Shove._Web.Utility.FilteSqlInfusion(PF.EncryptPassword(password)) + "','" + Shove._Web.Utility.FilteSqlInfusion(tbAgentCompanyName.Text) + "','" + Shove._Web.Utility.FilteSqlInfusion(tbAgentSiteName.Text) + "',1,'" + Money +"')";
        int Result = Shove.Database.MSSQL.ExecuteNonQuery(sql);
        if (Result < 0)
        {
            Shove._Web.JavaScript.Alert(this.Page, "代理商添加失败!");
        }
        else
        {
            Shove._Web.JavaScript.Alert(this.Page, "代理商添加成功!");
        }
        
    }
}
