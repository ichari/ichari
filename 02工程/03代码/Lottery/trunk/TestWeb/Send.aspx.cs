using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Linq;
using System.IO;
using System.Text;

namespace TestWeb
{
    public partial class Send : System.Web.UI.Page
    {
        

        protected void Page_Load(object sender, EventArgs e)
        {
            SLS.Notify.NotifyWrapper.ConnectString = "server=218.249.153.73;Uid=syslotto;Pwd=$lottery888;Database=SLS_JC;";            
        }
        /// <summary>
        /// 奖期查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSend_Click(object sender, EventArgs e)
        {
            

            var wrap = new SLS.Notify.NotifyWrapper(false)
            {
                ApiRequestWrap = new SLS.Notify.ApiRequest() { 
                    AgentId = "800060",
                    AgentPwd = "xicaigufen",
                    ApiUrl = "http://221.123.166.226:7070/billservice/sltAPI",
                    TransType = SLS.Notify.CommandType.IssueQuery
                }
            };
            tbResult.Text = wrap.Current.Send(tbQueryIssueLotTypeId.Text,tbQueryIssueNo.Text);
            
        }

        private string GetMd5(string source)
        {
            var md5 = System.Security.Cryptography.MD5.Create();
            var bs = System.Text.Encoding.Default.GetBytes(source);
            var hash = md5.ComputeHash(bs);

            StringBuilder s = new StringBuilder();
            foreach (var item in hash) {
                s.Append(item.ToString("x2"));
            }

            return s.ToString();
        }

        protected void btnParse_Click(object sender, EventArgs e)
        {
            //解析收到的XML            
            var xdoc = XDocument.Parse(tbRecieve.Text);           
            var q = xdoc.Descendants("issue").FirstOrDefault();
            
            var dic = q.Attributes().ToDictionary(t => t.Name.LocalName);

            lblLotoId.Text = dic.ContainsKey("lotoid") ? dic["lotoid"].Value : "";
            
            
        }
        /// <summary>
        /// 向出票商发送开奖请求
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSendOpen_Click(object sender, EventArgs e)
        {
            
            
        }
        /// <summary>
        /// 模拟开奖
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnMoni_Click(object sender, EventArgs e)
        {
            SLS.Notify.NotifyWrapper.ConnectString = "server=218.249.153.73;Uid=syslotto;Pwd=$lottery888;Database=SLS_JC;";

            var wrap = new SLS.Notify.NotifyWrapper(false)
            {
                ApiRequestWrap = new SLS.Notify.ApiRequest() { 
                    AgentId = "800060",
                    AgentPwd = "xicaigufen",
                    ApiUrl = "http://221.123.166.226:7070/billservice/sltAPI",
                    TransType = SLS.Notify.CommandType.OpenIssueQuery
                },
                MoniOpenXml = tbMoni.Text
            };
            wrap.Current.Send(TextBox3.Text, TextBox4.Text);

            lblTxt.Text = "业务处理成功";
        }
        /// <summary>
        /// 抓取第三方网站数据开奖
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSniffer_Click(object sender, EventArgs e)
        { 
            SLS.Notify.NotifyWrapper.ConnectString = "server=218.249.153.73;Uid=syslotto;Pwd=$lottery888;Database=SLS_JC;";            

            var wrap = new SLS.Notify.NotifyWrapper(true);
            wrap.Current.Send(tbLotoTypeId.Text,tbIssueNo.Text);
        }
    }
}