using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Shove.Web.UI;

public partial class Home_Room_MyPromotion_HongBaoRecord : RoomPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindData();
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
        string Key = "Home_Room_MyPromotion_HongBaoRecord_BindData" + _User.ID.ToString();

        DataTable dt = Shove._Web.Cache.GetCacheAsDataTable(Key);

        if (dt == null)
        {
            string cmd = "select a.*,Name as UserName from T_UserHongbaoPromotion a left join  T_Users b on a.AcceptUserID = b.ID where UserID = " + _User.ID.ToString() + " order by CreateDate desc";

            dt = Shove.Database.MSSQL.Select(cmd);

            if (dt == null)
            {
                PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", this.GetType().BaseType.FullName);

                return;
            }

            Shove._Web.Cache.SetCache(Key, dt);
        }

        DataTable dtData = dt.Clone();

        DataRow[] drs = dt.Select(GetCondition());

        for(int i=0;i<drs.Length;i++)
        {
            dtData.Rows.Add(drs[i].ItemArray);
        }

        PF.DataGridBindData(g, dtData, gPager);

        if (rblType.SelectedValue == "1"||rblType.SelectedValue=="3")
        {
            g.Columns[6].Visible = true;
        }
        else
        {
            g.Columns[6].Visible = false;
        }
    }

    private string GetCondition()
    {
        string condition = "";

        string datetime = DateTime.Now.ToString();

        switch (rblType.SelectedValue)
        {
            case "2":
                {
                    condition = "AcceptUserID<>-1";
                } break;
            case "4":
                {
                    condition = "AcceptUserID=-1 and ExpiryDate<'"+datetime+"'";
                } break;
            case "3":
                {
                    condition = "AcceptUserID=-1 and ExpiryDate>='" + datetime + "'";
                } break;
            default:
                {
                    condition = "1=1";
                } break;

         
        }

        return condition;
    }

    protected void gPager_PageWillChange(object Sender, Shove.Web.UI.PageChangeEventArgs e)
    {
        BindData();
    }

    protected void gPager_SortBefore(object source, System.Web.UI.WebControls.DataGridSortCommandEventArgs e)
    {
        BindData();
    }

    protected void g_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        if (e.CommandName == "Deletes")
        {
            DAL.Tables.T_UserHongbaoPromotion uhp = new DAL.Tables.T_UserHongbaoPromotion();

            uhp.Delete("ID="+e.Item.Cells[7].Text+"");

            Shove._Web.Cache.ClearCache("Home_Room_MyPromotion_HongBaoRecord" + _User.ID.ToString());
            BindData();
        }
    }

    protected void g_ItemDataBound(object sender, DataGridItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.EditItem || e.Item.ItemType == ListItemType.Item)
        {
            ShoveLinkButton btnCopy = e.Item.FindControl("btnCopy") as ShoveLinkButton;

            string url = e.Item.Cells[5].Text;

            btnCopy.Attributes.Add("onclick","copy('"+url+"')");

            e.Item.Cells[5].Text = Shove._String.Cut(url, 30);

            ShoveLinkButton btnDelete = e.Item.FindControl("btnDelete") as ShoveLinkButton;

            if (e.Item.Cells[8].Text != "-1")
            {
                btnDelete.Visible = false;
            }
        }
    }

    protected void rblType_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindData();
    }
}
