using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class CPS_UserLogin : CpsPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            if (_User != null)
            {
                lbUserName.Text = "<strong>" + _User.Name + "</strong>";

                if (_User.Competences.CompetencesList != "")
                {
                    lbUserType.Text = "管理员";
                    trSupperManager.Visible = true;
                }
                else
                {
                    if (_User.cps.ID > 0)
                    {
                        trCpsLogin.Visible = true;

                        if (_User.cps.Type == 2)
                        {
                            lbUserType.Text = "代理商";
                        }
                    }
                    else
                    {
                        lbUserType.Text = "高级会员";

                        DataTable dt = new DAL.Tables.T_CpsTrys().Open("Type", "HandleResult = 0 and UserID=" + _User.ID.ToString(), "");

                        if(dt != null && dt.Rows.Count > 0)
                        {
                            trCheck.Visible = true;

                            if (Shove._Convert.StrToInt(dt.Rows[0]["Type"].ToString(), 0) == 2)
                            {
                                lbUserType.Text = "代理商";
                            }
                            else
                            {
                                lbUserType.Text = "推广员";
                            }
                        }
                        else
                        {
                            trApply.Visible = true;
                        }
                    }
                }

                NoLogin.Visible = false;
                Longining.Visible = true;
            }
        }
    }

    #region Web 窗体设计器生成的代码

    override protected void OnInit(EventArgs e)
    {
        isRequestLogin = false;

        base.OnInit(e);
    }

    #endregion

    protected void btnOK_Click(object sender, ImageClickEventArgs e)
    {
        string ReturnDescription = "";
        int Result = Result = new Login().LoginSubmit(this.Page, _Site, tbUserName.Text, tbPwd.Text, ref ReturnDescription);

        if (Result < 0)
        {
            Shove._Web.JavaScript.Alert(this.Page, ReturnDescription,"Login.aspx","top");

            return;
        }

        Response.Write("<script>window.top.location.href='Default.aspx'</script>");
    }

    protected void imgbtnLogout_Click(object sender, EventArgs e)
    {
        if (_User != null)
        {
            string ReturnDescription = "";
            _User.Logout(ref ReturnDescription);
        }

        Response.Write("<script>window.top.location.href='Default.aspx'</script>");
    }
}
