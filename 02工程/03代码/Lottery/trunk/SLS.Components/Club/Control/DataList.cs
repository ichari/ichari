using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;

using Discuz.Common;
using Discuz.Data;

namespace Discuz.Control
{
	/// <summary>
	/// DataList 控件。
	/// </summary>
	[DefaultProperty("Text"),ToolboxData("<{0}:DataList runat=server></{0}:DataList>")]
	public class DataList : System.Web.UI.WebControls.DataList
	{
        /// <summary>
        /// 构造函数
        /// </summary>
		public DataList(): base()
		{
			this.Height=30; 
			this.BorderStyle=BorderStyle.Dotted; 
			this.BorderWidth=0; 
		}

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="sqlstring">查询字符串</param>
		public DataList(string sqlstring): this()
		{
			AddTableData(this.SqlText);
		}

        /// <summary>
        /// 添加表数据
        /// </summary>
		public void AddTableData()
		{
			if((this.SqlText!=null)&&(this.SqlText!=""))
			{
				AddTableData(this.SqlText);
			}
		}

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="sqlstring">查询字符串</param>
		public void AddTableData(string sqlstring)
		{
            this.DataSource = DbHelper.ExecuteDataset(CommandType.Text, sqlstring).Tables[0].DefaultView;
			this.DataBind();			
		}

        #region SQL字符串

        /// <summary>
        /// SQL字符串变量
        /// </summary>
        private string sqltext;


        /// <summary>
        /// SQL字符串属性
        /// </summary>
        [Bindable(true), Category("Appearance"), DefaultValue("")]
        public string SqlText
        {
            get
            {
                return sqltext;
            }

            set
            {
                sqltext = value;
            }
        }

        #endregion


	
	}
}
