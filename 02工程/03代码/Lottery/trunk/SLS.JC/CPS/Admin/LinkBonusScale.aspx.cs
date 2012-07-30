using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class CPS_Admin_LinkBonusScale : CpsPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            BindData();
        }
    }

    #region Web 窗体设计器生成的代码

    override protected void OnInit(EventArgs e)
    {
        isCpsRequestLogin = true;
        RequestLoginPage = Shove._Web.Utility.GetUrl() + "/CPS/Admin/Default.aspx?SubPage=LinkBonusScale.aspx";

        base.OnInit(e);
    }

    #endregion

    private void BindData()
    {
        DataTable dt = new DAL.Tables.T_UnionLinkScale().Open("ID,UnionID,SiteLinkPID,BonusScale", "UnionID=" + _User.ID, "SiteLinkPID");

        DataTable dtLink = new DAL.Tables.T_Users().Open("distinct [Memo]", "Memo<>'' and CpsID=" + _User.cps.ID, "[Memo]");
       
        double scale = _Site.SiteOptions["BonusScale"].ToDouble(0.02);
       
        for (int i = 0; i < dtLink.Rows.Count; i++)
        {
            string pid = dtLink.Rows[i][0].ToString();
            DataRow[] drList = dt.Select("SiteLinkPID='" + pid + "'");
            if (drList.Length == 0)
            {
                DataRow dr = dt.NewRow();
                dr["ID"] = 0;
                dr["UnionID"] = _User.cps.ID;
                dr["SiteLinkPID"] = pid;
                dr["BonusScale"] = scale;
                dt.Rows.Add(dr);
            }
        }

        DataTable dtTemp = GetNewDataTable(dt, "SiteLinkPID like '%" + tbPID.Text.Trim() + "%'");

        PF.DataGridBindData(g, dtTemp, gPager);
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        BindData();
    }

    private DataTable GetNewDataTable(DataTable dt, string condition)
    {
        DataTable newdt = new DataTable();
        newdt = dt.Clone();
        DataRow[] dr = dt.Select(condition);
        for (int i = 0; i < dr.Length; i++)
            newdt.ImportRow((DataRow)dr[i]);

        return newdt;
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        foreach (DataGridItem gridItem in g.Items)
        {
            string id = gridItem.Cells[0].Text;
            string pid = gridItem.Cells[1].Text;
            TextBox tb = (TextBox)gridItem.Cells[2].Controls[1];
            string scale = tb.Text;
          
            UpdateBouneSale(id, pid, scale);
        }

        BindData();
        Shove._Web.JavaScript.Alert(this.Page, "数据保存完毕！");
    }

    private void UpdateBouneSale(string id, string pid, string scale)
    {
        decimal temp;
        bool isNum = decimal.TryParse(scale, out temp);
        if (!isNum)
            return;

        string sql = "";
        if (id == "0")
        {
            sql = string.Format("Insert into T_UnionLinkScale (UnionID,SiteLinkPID,BonusScale) Values({0}, '{1}', {2})", _User.ID, pid, scale);
        }
        else
        {
            sql = string.Format("Update T_UnionLinkScale Set BonusScale={0} where ID={1}", scale, id);
        }
        
        Shove.Database.MSSQL.ExecuteNonQuery(sql, new Shove.Database.MSSQL.Parameter[] { null });
    }
}
