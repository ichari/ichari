using System;
using System.Data;
using System.Web.UI.WebControls;
using System.Web.UI;

using Discuz.Common;
using Discuz.Forum;
using Button = Discuz.Control.Button;
using DataGrid = Discuz.Control.DataGrid;
using Discuz.Config;
using Discuz.Data;


namespace Discuz.Web.Admin
{
    /// <summary>
    /// 金币策略编辑
    /// </summary>
    public partial class allowparticipatescore : AdminPage
    {
        #region 控件声明

        protected Button SetAvailable;

        #endregion

        protected DataTable templateDT = new DataTable("templateDT");


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (DNTRequest.GetString("groupid") != "")
                {
                    BindData();
                }
                else
                {
                    Response.Write("<script>history.go(-1);</script>");
                    Response.End();
                }
            }
        }

        public void BindData()
        {
            #region 绑定数据

            DataGrid1.AllowCustomPaging = false;
            DataGrid1.TableHeaderName = "允许评分范围列表";
            DataGrid1.DataSource = LoadDataInfo();
            DataGrid1.DataBind();

            #endregion
        }

        protected void Sort_Grid(Object sender, DataGridSortCommandEventArgs e)
        {
            DataGrid1.Sort = e.SortExpression.ToString();
        }


        protected void DataGrid_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            DataGrid1.LoadCurrentPageIndex(e.NewPageIndex);
            BindData();
        }

        protected void DataGrid_Edit(Object sender, DataGridCommandEventArgs E)
        {
            if (ViewState["validrow"].ToString().IndexOf("," + E.Item.ItemIndex + ",") >= 0)
            {
                DataGrid1.EditItemIndex = (int)E.Item.ItemIndex;
                DataGrid1.DataSource = LoadDataInfo();
                DataGrid1.DataBind();
            }
            else
            {
                base.RegisterStartupScript( "", "<script>alert('操作失败,您所修改的金币行是无效的,具体操作请看注释!');window.location.href='global_allowparticipatescore.aspx?pagename=" + DNTRequest.GetString("pagename") + "&groupid=" + DNTRequest.GetString("groupid") + "';</script>");
                return;
            }
        }

        protected void DataGrid_Cancel(Object sender, DataGridCommandEventArgs E)
        {
            DataGrid1.EditItemIndex = -1;
            DataGrid1.DataSource = LoadDataInfo();
            DataGrid1.DataBind();
        }

        private DataTable LoadDataInfo()
        {
            #region 加载数据信息

            DataTable dt = DatabaseProvider.GetInstance().GetRaterangeByGroupid(DNTRequest.GetInt("groupid", 0));
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0][0].ToString().Trim() == "")
                {
                    return RemoveEmptyRows(NewParticipateScore());
                }
                else
                {
                    return RemoveEmptyRows(GroupParticipateScore(dt.Rows[0]["raterange"].ToString().Trim()));
                }
            }
            else
            {
                return (DataTable)null;
            }

            #endregion
        }

        private DataTable RemoveEmptyRows(DataTable dt)
        {
            DataRow[] drs = dt.Select("ScoreName=''");
            foreach (DataRow dr in drs)
            {
                dt.Rows.Remove(dr);
            }
            return dt;
        }

        public DataTable NewParticipateScore()
        {
            #region 初始化并装入默认数据

            templateDT.Columns.Clear();
            templateDT.Columns.Add("id", Type.GetType("System.Int32"));
            templateDT.Columns.Add("available", Type.GetType("System.Boolean"));
            templateDT.Columns.Add("ScoreCode", Type.GetType("System.String"));
            templateDT.Columns.Add("ScoreName", Type.GetType("System.String"));
            templateDT.Columns.Add("Min", Type.GetType("System.String"));
            templateDT.Columns.Add("Max", Type.GetType("System.String"));
            templateDT.Columns.Add("MaxInDay", Type.GetType("System.String"));

            for (int rowcount = 0; rowcount < 8; rowcount++)
            {
                DataRow dr = templateDT.NewRow();
                dr["id"] = rowcount + 1;
                dr["available"] = false;
                dr["ScoreCode"] = "extcredits" + Convert.ToString(rowcount + 1);
                dr["ScoreName"] = "";
                dr["Min"] = "";
                dr["Max"] = "";
                dr["MaxInDay"] = "";
                templateDT.Rows.Add(dr);
            }
            DataRow scoresetname = Scoresets.GetScoreSet().Rows[0];
            string validrow = "";

            for (int count = 0; count < 8; count++)
            {
                if ((scoresetname[count + 2].ToString().Trim() != "") && (scoresetname[count + 2].ToString().Trim() != "0"))
                {
                    templateDT.Rows[count]["ScoreName"] = scoresetname[count + 2].ToString().Trim();

                    validrow = validrow + "," + count;
                }

                if (IsValidScoreName(count + 1))
                {
                    validrow = validrow + "," + count;
                }
            }
            ViewState["validrow"] = validrow + ",";
            return templateDT;

            #endregion
        }

        public bool IsValidScoreName(int scoreid)
        {
            #region 是否是有效的金币名称

            bool isvalid = false;

            foreach (DataRow dr in Scoresets.GetScoreSet().Rows)
            {
                if ((dr["id"].ToString() != "1") && (dr["id"].ToString() != "2"))
                {
                    if (dr[scoreid + 1].ToString().Trim() != "0")
                    {
                        isvalid = true;
                        break;
                    }
                }
            }
            return isvalid;

            #endregion
        }

        public DataTable GroupParticipateScore(string raterange)
        {
            #region 用数据库中的记录更新已装入的默认数据

            NewParticipateScore();

            int i = 0;
            foreach (string raterangestr in raterange.Split('|'))
            {
                if (raterangestr.Trim() != "")
                {
                    string[] scoredata = raterangestr.Split(',');
                    if (scoredata[1].Trim() == "True")
                    {
                        templateDT.Rows[i]["available"] = true;
                    }
                    templateDT.Rows[i]["Min"] = scoredata[4].Trim();
                    templateDT.Rows[i]["Max"] = scoredata[5].Trim();
                    templateDT.Rows[i]["MaxInDay"] = scoredata[6].Trim();
                }
                i++;
            }
            return templateDT;

            #endregion
        }


        protected void DataGrid_Update(Object sender, DataGridCommandEventArgs E)
        {
            #region 编辑相关的金币设置信息

            string id = DataGrid1.DataKeys[(int)E.Item.ItemIndex].ToString();
            bool available = ((CheckBox)E.Item.FindControl("available")).Checked;
            string Min = ((TextBox)E.Item.Cells[5].Controls[0]).Text.Trim();
            string Max = ((TextBox)E.Item.Cells[6].Controls[0]).Text.Trim();
            string MaxInDay = ((TextBox)E.Item.Cells[7].Controls[0]).Text.Trim();

            LoadDataInfo();
            int count = Convert.ToInt32(id) - 1;
            if (available)
            {
                templateDT.Rows[count]["available"] = true;
            }
            else
            {
                templateDT.Rows[count]["available"] = false;
            }

            if (Min == "" || Max == "" || MaxInDay == "")
            {
                base.RegisterStartupScript( "", "<script>alert('评分的最小值,最大值以及24小时最大评分数不能为空.');window.location.href='global_allowparticipatescore.aspx?pagename=" + DNTRequest.GetString("pagename") + "&groupid=" + DNTRequest.GetString("groupid") + "';</script>");
                return;
            }

            if ((Min != "" && !Utils.IsNumeric(Min.Replace("-", ""))) || (Max != "" && !Utils.IsNumeric(Max.Replace("-", ""))) || (MaxInDay != "" && !Utils.IsNumeric(MaxInDay.Replace("-", ""))))
            {
                base.RegisterStartupScript( "", "<script>alert('输入的数据必须是数字.');window.location.href='global_allowparticipatescore.aspx?pagename=" + DNTRequest.GetString("pagename") + "&groupid=" + DNTRequest.GetString("groupid") + "';</script>");
                return;
            }

            if (Convert.ToInt32(Utils.SBCCaseToNumberic(Min)) >= Convert.ToInt32(Utils.SBCCaseToNumberic(Max)))
            {
                base.RegisterStartupScript( "", "<script>alert('评分的最小值必须小于评分最大值.');window.location.href='global_allowparticipatescore.aspx?pagename=" + DNTRequest.GetString("pagename") + "&groupid=" + DNTRequest.GetString("groupid") + "';</script>");
                return;
            }

            templateDT.Rows[count]["Min"] = Convert.ToInt32(Utils.SBCCaseToNumberic(Min));
            templateDT.Rows[count]["Max"] = Convert.ToInt32(Utils.SBCCaseToNumberic(Max));
            templateDT.Rows[count]["MaxInDay"] = Convert.ToInt32(Utils.SBCCaseToNumberic(MaxInDay));

            try
            {
                WriteScoreInf(templateDT);
                DataGrid1.EditItemIndex = -1;
                DataGrid1.DataSource = LoadDataInfo();
                DataGrid1.DataBind();
                base.RegisterStartupScript( "PAGE", "window.location.href='global_allowparticipatescore.aspx?pagename=" + DNTRequest.GetString("pagename") + "&groupid=" + DNTRequest.GetString("groupid") + "';");
            }
            catch
            {
                base.RegisterStartupScript( "", "<script>alert('无法更新数据库.');window.location.href='global_allowparticipatescore.aspx?pagename=" + DNTRequest.GetString("pagename") + "&groupid=" + DNTRequest.GetString("groupid") + "';</script>");
                return;
            }

            #endregion
        }


        public void WriteScoreInf(DataTable dt)
        {
            #region 向数据库中写入允许的评分范围内容

            string scorecontent = "";
            foreach (DataRow dr in dt.Rows)
            {
                scorecontent += dr["id"].ToString() + ",";
                scorecontent += dr["available"].ToString() + ",";
                scorecontent += dr["ScoreCode"].ToString() + ",";
                scorecontent += dr["ScoreName"].ToString() + ",";
                scorecontent += dr["Min"].ToString() + ",";
                scorecontent += dr["Max"].ToString() + ",";
                scorecontent += dr["MaxInDay"].ToString() + "|";
            }
            DatabaseProvider.GetInstance().UpdateRaterangeByGroupid(scorecontent.Substring(0, scorecontent.Length - 1), DNTRequest.GetInt("groupid",0));
            templateDT.Clear();

            AdminCaches.ReSetUserGroupList();

            #endregion
        }


        public bool GetAvailable(string available)
        {
            if (available == "True") return true;
            else return false;
        }

        public string GetImgLink(string available)
        {
            if (available == "True")
            {
                return "<div align=center><img src=../images/OK.gif /></div>";
            }
            else
            {
                return "<div align=center><img src=../images/Cancel.gif /></div>";
            }
        }

        private void DataGrid_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            #region 数据绑定显示长度控制

            if (e.Item.ItemType == ListItemType.EditItem)
            {
                TextBox t = (TextBox)e.Item.Cells[5].Controls[0];
                t.Attributes.Add("maxlength", "9");
                t.Attributes.Add("size", "6");

                t = (TextBox)e.Item.Cells[6].Controls[0];
                t.Attributes.Add("maxlength", "8");
                t.Attributes.Add("size", "6");

                t = (TextBox)e.Item.Cells[7].Controls[0];
                t.Attributes.Add("maxlength", "9");
                t.Attributes.Add("size", "6");
            }

            #endregion
        }

        #region Web Form Designer generated code

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.DataGrid1.ItemDataBound += new DataGridItemEventHandler(this.DataGrid_ItemDataBound);

            DataGrid1.LoadEditColumn();
        }

        #endregion
    }
}