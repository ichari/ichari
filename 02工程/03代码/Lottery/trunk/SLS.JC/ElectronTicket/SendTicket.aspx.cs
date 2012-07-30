using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Net;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using System.Xml;
using System.Configuration;

public partial class ElectronTicket_SendTicket : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        
    }

    protected void btn_Click(object sender, EventArgs e)
    {
        
    }
    protected void btnK_Click(object sender, EventArgs e)
    {
        string x = "<body><order username=\"\" lotoid=\"002\" issue=\"2012177\" areaid=\"\" orderno=\"80000120127515447893\"><userinfo realname=\"xyz\" mobile=\"13699288098\" email=\"123456@123.com\" cardtype=\"1\" cardno=\"342623198912207578\"/><ticket seq=\"2\">00-01-01,02,03,04,05,06#07|01,02,03,05,06,11#20-1-4</ticket></order></body>";//00-01-01,02,03,04,05,06#07&amp;01,02,04,06,08#11-1-4
        
        x = "<body><order username=\"\" lotoid=\"001\" issue=\"2012088\" areaid=\"\" orderno=\"12345675eweqqwqwe\">"
            + "<userinfo realname=\"\" mobile=\"13333333333\" email=\"23432432@163.com\" cardtype=\"1\" cardno=\"342623198912207578\"/>"
            + "<ticket seq=\"1\">00-01-01,02,03,04,05,06#07&amp;03,04,11,20,25,32#07-50-200</ticket>"
            + "<ticket seq=\"2\">00-01-01,02,03,04,05,06#07&amp;03,04,11,20,25,32#07-30-120</ticket>"
            + "<ticket seq=\"3\">00-01-03,04,11,20,25,32#07-4-8</ticket></order></body>";
        x = "<body><order username=\"xicaigufen\" lotoid=\"001\" issue=\"2011147\" areaid=\"00\" orderno=\"20120711182757000002012080\">"
            + "<userinfo realname=\"\" mobile=\"\" email=\"\" cardtype=\"1\" cardno=\"\" />"
            + "<ticket seq=\"00000\">00-01-02,03,13,15,23,25,27&amp;03,06,08,11,17,20,27&amp;09,12,15,18,21,23,24&amp;02,03,08,12,22,29,30&amp;08,10,15,18,20,26,28-1-10</ticket>"
           // + "<ticket seq=\"00001\">00-01-01,05,17,18,20,25,30&amp;09,12,14,16,17,21,26&amp;05,06,11,12,18,20,26&amp;05,06,13,26,27,29,30&amp;01,05,15,19,22,24,27-1-10</ticket>"
            //+ "<ticket seq=\"00002\">00-01-05,10,13,15,22,26,29&amp;06,09,15,16,22,25,27&amp;03,06,18,20,21,23,29&amp;06,15,18,20,24,25,27&amp;06,11,13,15,24,27,30-50-500</ticket>"
           // + "<ticket seq=\"00003\">00-01-05,10,13,15,22,26,29&amp;06,09,15,16,22,25,27&amp;03,06,18,20,21,23,29&amp;06,15,18,20,24,25,27&amp;06,11,13,15,24,27,30-50-500</ticket>"
            //+ "<ticket seq=\"00004\">00-01-05,10,13,15,22,26,29&amp;06,09,15,16,22,25,27&amp;03,06,18,20,21,23,29&amp;06,15,18,20,24,25,27&amp;06,11,13,15,24,27,30-11-110</ticket>"
            + "</order></body>";
        x = "<body><order username=\"xicaigufen\" lotoid=\"001\" issue=\"2011147\" areaid=\"00\" orderno=\"20120712101015000002012081\"><userinfo realname=\"\" mobile=\"\" email=\"\" cardtype=\"1\" cardno=\"\" /><ticket seq=\"00000\">00-01-01,02,04,08,23,24,27&amp;01,06,18,21,23,24,28&amp;02,07,13,16,19,25,27&amp;02,04,14,15,17,27,28&amp;05,06,09,10,13,15,22-1-10</ticket><ticket seq=\"00000\">00-01-10,11,16,17,18,20,27-1-2</ticket><ticket seq=\"00000\">00-02-03,05,06,08,11,15,16,19,22,23,27,29-1-1584</ticket><ticket seq=\"00000\">00-02-05,06,07,12,18,19,22,27-1-16</ticket></order></body>";
        x = tb.Text.Split('?')[1];
        string cmd = tb.Text.Split('?')[0];
        MD5CryptoServiceProvider md = new MD5CryptoServiceProvider();
        Encoding gb2312;
        gb2312 = Encoding.GetEncoding(936);
        byte[] mb = md.ComputeHash(gb2312.GetBytes("800060" + "xicaigufen" + x));
        string mdx;
        mdx = BitConverter.ToString(mb).Replace("-", "").ToLower();
        tbx.Text = mdx;
        string y;
        y = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><msg v=\"1.0\" id=\"20120711182757000002012080\"><ctrl><agentID>800060</agentID><cmd>" + cmd + "</cmd><timestamp>"
            + DateTime.Now.ToString("yyyyMMddHHmmss") + "</timestamp><md>" + mdx + "</md></ctrl>" + x + "</msg>";
        
        var sn = new SLS.Common.EtSunLotto();
        string resp = null;
        //y = "%3C%3F%78%6D%6C%20%76%65%72%73%69%6F%6E%3D%22%31%2E%30%22%20%65%6E%63%6F%64%69%6E%67%3D%22%55%54%46%2D%38%22%3F%3E%3C%6D%73%67%20%76%3D%22%31%2E%30%22%20%69%64%3D%22%31%33%34%31%39%39%32%39%37%38%36%36%34%22%3E%3C%63%74%72%6C%3E%3C%61%67%65%6E%74%49%44%3E%38%30%30%30%36%30%3C%2F%61%67%65%6E%74%49%44%3E%3C%63%6D%64%3E%32%30%30%31%3C%2F%63%6D%64%3E%3C%74%69%6D%65%73%74%61%6D%70%3E%31%33%34%31%39%39%32%39%37%38%36%36%39%3C%2F%74%69%6D%65%73%74%61%6D%70%3E%3C%6D%64%3E%63%39%38%64%65%32%32%35%39%65%33%64%65%64%64%36%38%64%36%64%64%37%32%63%38%31%64%64%35%66%31%30%3C%2F%6D%64%3E%3C%2F%63%74%72%6C%3E%3C%62%6F%64%79%3E%3C%6F%72%64%65%72%20%75%73%65%72%6E%61%6D%65%3D%22%22%20%6C%6F%74%6F%69%64%3D%22%30%30%31%22%20%69%73%73%75%65%3D%22%32%30%31%32%30%38%39%22%20%61%72%65%61%69%64%3D%22%22%20%6F%72%64%65%72%6E%6F%3D%22%31%32%33%34%35%36%37%38%65%77%65%71%71%77%71%77%65%22%3E%3C%75%73%65%72%69%6E%66%6F%20%72%65%61%6C%6E%61%6D%65%3D%22%E6%B5%8B%E8%AF%95%E6%B5%8B%E8%AF%95%22%20%6D%6F%62%69%6C%65%3D%22%31%33%33%33%33%33%33%33%33%33%33%22%20%65%6D%61%69%6C%3D%22%32%33%34%33%32%34%33%32%40%31%36%33%2E%63%6F%6D%22%20%63%61%72%64%74%79%70%65%3D%22%31%22%20%63%61%72%64%6E%6F%3D%22%33%34%32%36%32%33%31%39%38%39%31%32%32%30%37%35%37%38%22%2F%3E%3C%74%69%63%6B%65%74%20%73%65%71%3D%22%31%22%3E%30%30%2D%30%31%2D%30%31%2C%30%32%2C%30%33%2C%30%34%2C%30%35%2C%30%36%23%30%37%26%61%6D%70%3B%30%38%2C%30%39%2C%31%30%2C%31%31%2C%31%32%2C%31%33%23%31%34%2D%31%2D%34%3C%2F%74%69%63%6B%65%74%3E%3C%2F%6F%72%64%65%72%3E%3C%2F%62%6F%64%79%3E%3C%2F%6D%73%67%3E";

        tbK.Text = y + "\n" + HttpUtility.UrlEncode(y);
        byte[] bData = Encoding.UTF8.GetBytes("cmd=" + cmd + "&msg=" + HttpUtility.UrlEncode(y));
        HttpWebRequest req = null;
        req = (HttpWebRequest)WebRequest.Create(ConfigurationManager.AppSettings["SunPostAddr"]);
        req.Method = "POST";
        req.ContentType = "application/x-www-form-urlencoded";
        req.ContentLength = bData.Length;
        req.AllowWriteStreamBuffering = false;
       
        using (Stream writeStream = req.GetRequestStream())
        {
            writeStream.Write(bData, 0, bData.Length);
        }
        using (StreamReader sr = new StreamReader(req.GetResponse().GetResponseStream()))
        {
            resp = sr.ReadToEnd();
        }
        tbOut.Text = "=============== Response ===============\n" + resp;
        
        
    }
}