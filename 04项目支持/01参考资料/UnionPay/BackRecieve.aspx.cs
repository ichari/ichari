using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.unionpay.upop.sdk;

public partial class Home_Room_OnlinePay_UnionPay_BackRecieve : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // 要使用各种Srv必须先使用LoadConf载入配置
        UPOPSrv.LoadConf(Server.MapPath("~/App_Data/conf.xml.config"));

        // 使用Post过来的内容构造SrvResponse
        SrvResponse resp = new SrvResponse(Util.NameValueCollection2StrDict(Request.Form));

        // 收到回应后做后续处理（这里写入文件，仅供演示）
        System.IO.StreamWriter sw = new System.IO.StreamWriter(Server.MapPath("~/notify_data.txt"));
        string sOP = "";
        
        if (resp.ResponseCode != SrvResponse.RESP_SUCCESS)
        {
            sw.WriteLine("error in parsing response message! code:" + resp.ResponseCode);
            sOP = "error in parsing response message! code:" + resp.ResponseCode + Environment.NewLine;
        }
        else
        {
            foreach (string k in resp.Fields.Keys)
            {
                sw.WriteLine(k + "\t = " + resp.Fields[k]);
                sOP = k + "\t = " + resp.Fields[k] + Environment.NewLine;
            }
        }

        sw.Close();
        Response.Write(sOP);
    }
}