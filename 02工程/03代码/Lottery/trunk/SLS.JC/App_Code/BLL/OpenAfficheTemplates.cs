using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using Shove.Database;

/// <summary>
/// OpenAfficheTemplates 的摘要说明
/// </summary>
public class OpenAfficheTemplates
{
    public OpenAfficheTemplates()
    {
        //
        // TODO: 在此处添加构造函数逻辑
        //
    }

    public string this[int LotteryID]
    {
        get
        {
            object obj = MSSQL.ExecuteScalar("select OpenAfficheTemplate from T_Lotteries where [ID] = " + LotteryID.ToString());

            if (obj == null)
            {
                return "";
            }
            
            return obj.ToString();
        }
        set
        {
            MSSQL.ExecuteNonQuery("update T_Lotteries set OpenAfficheTemplate = @OpenAfficheTemplate where [ID] = " + LotteryID.ToString(),
                new MSSQL.Parameter("OpenAfficheTemplate", SqlDbType.VarChar, 0, ParameterDirection.Input, value));
        }
    }
}
