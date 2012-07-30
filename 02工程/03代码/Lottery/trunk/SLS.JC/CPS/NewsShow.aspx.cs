using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class CPS_NewsShow : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindNewsDetail();
        }
    }

    private void BindNewsDetail()
    {
        long id = Shove._Convert.StrToLong(Shove._Web.Utility.GetRequest("ID"), -1);

        if (id < 0)
        {
            PF.GoError(ErrorNumber.Unknow, "您访问的数据不存在，可能是参数错误或内容已经被删除！", this.GetType().BaseType.FullName);

            return;
        }

        DataTable dt = new DAL.Views.V_News().Open("Title,DateTime,Content, TypeName", "ID=" + id + "", "");

        if (dt == null)
        {
            PF.GoError(ErrorNumber.Unknow, "数据库繁忙，请重试", this.GetType().BaseType.FullName);

            return;
        }

        if (dt.Rows.Count == 0)
        {
            PF.GoError(ErrorNumber.Unknow, "您访问的数据不存在，可能是参数错误或内容已经被删除！", this.GetType().BaseType.FullName);

            return;
        }

        DataRow dr = dt.Rows[0];

        lbDate.Text = Shove._Convert.StrToDateTime(dr["DateTime"].ToString(), DateTime.Now.ToString()).ToString("yyyy-MM-dd");
        lbTitle.Text = dr["Title"].ToString();
        lbDetail.Text = dr["Content"].ToString();
        lblType.Text = dr["TypeName"].ToString();
    }
}
