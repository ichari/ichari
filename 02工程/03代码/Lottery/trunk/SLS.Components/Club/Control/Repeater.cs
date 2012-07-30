using System.Data;
using System.Web.UI;
using System.ComponentModel;

using Discuz.Common;
using Discuz.Data;

namespace Discuz.Control
{
	/// <summary>
	/// Repeater 控件。
	/// </summary>
	[DefaultProperty("Text"),ToolboxData("<{0}:Repeater runat=server></{0}:Repeater>")]
	public class Repeater : System.Web.UI.WebControls.Repeater
	{
        /// <summary>
        /// 构造函数
        /// </summary>
        public Repeater(): base()
		{
		}

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="sqlstring">查询字符串</param>
		public Repeater(string sqlstring): this()
		{
			AddTableData(sqlstring);
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
        /// 添加表数据
        /// </summary>
        /// <param name="sqlstring">查询字符串</param>
		public void AddTableData(string sqlstring)
		{
            this.DataSource = DbHelper.ExecuteDataset(CommandType.Text, sqlstring).Tables[0];
			this.DataBind();			
		}


	    /// <summary>
        /// SQL字符串属性
	    /// </summary>
		[Bindable(true),Category("Appearance"),DefaultValue("")] 
		public string SqlText 
		{
			get
			{
				return (string)ViewState["SqlText"];
			}

			set
			{
				ViewState["SqlText"] = value;
			}
		}

	}
}
