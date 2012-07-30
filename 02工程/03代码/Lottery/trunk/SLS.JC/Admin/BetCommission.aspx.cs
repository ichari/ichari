using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;

public partial class Admin_BetCommission : AdminPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            BindDataForLottery();
        }
    }

    private void BindDataForLottery()
    {
        DataTable dt = new DAL.Tables.T_BetCommission().Open("", "", "[ID]");

        if (dt == null)
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", this.GetType().BaseType.FullName);

            return;
        }

        g.DataSource = dt;
        g.DataBind();

        g.Columns[3].Visible = false;
        g.Columns[4].Visible = false;
        g.Columns[5].Visible = false;

        // 选中已经设置的彩种
        for (int i = 0; i < g.Rows.Count; i++)
        {
            CheckBox cbisUsed = (CheckBox)g.Rows[i].Cells[1].FindControl("cbisUsed");

            cbisUsed.Checked = Shove._Convert.StrToBool(g.Rows[i].Cells[4].Text, false);

            TextBox tbCommission = (TextBox)g.Rows[i].Cells[2].FindControl("tbCommission");

            tbCommission.Text = Shove._Convert.StrToDouble(g.Rows[i].Cells[5].Text, 0).ToString("F2");
        }
    }

    protected void btnOK_Click(object sender, EventArgs e)
    {
        DAL.Tables.T_BetCommission t_BetCommission = new DAL.Tables.T_BetCommission();

        for (int i = 0; i < g.Rows.Count; i++)
        {
            string ID = g.Rows[i].Cells[3].Text;
            string Commission = ((TextBox)g.Rows[i].Cells[2].FindControl("tbCommission")).Text;

            if (Shove._Convert.StrToFloat(Commission, 0) > 1)
            {
                Shove._Web.JavaScript.Alert(this.Page, "设置比较错误。");

                return;
            }

            t_BetCommission.Status_ON.Value = ((CheckBox)g.Rows[i].Cells[1].FindControl("cbisUsed")).Checked;
            t_BetCommission.Commission.Value = Shove._Convert.StrToFloat(Commission, 0);
            t_BetCommission.Update("ID=" + ID);
        }

        Shove._Web.JavaScript.Alert(this.Page, "合买佣金设置已经完成。");
    }
}
