using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Reflection;
using System.IO;

namespace SLS.Notify
{
    /// <summary>
    /// 主动抓取第三方网站数据提供类
    /// </summary>
    internal class SnifferLotteryDataProvider : LotteryDataProvider
    {
        public override string GetData(int lotoTypeId,string issueNo)
        {
            var ass = Assembly.GetExecutingAssembly();
            //载入第三方URL资源          
            var sm = new StreamReader(ass.GetManifestResourceStream("SLS.Notify.Xml.OpenUrl.xml"));
            var xdoc = XDocument.Parse(sm.ReadToEnd());

            var urlElements = xdoc.Descendants("url");
            var httpElement = urlElements.Where(t => t.Attribute("lotoId").Value == lotoTypeId.ToString()).FirstOrDefault();
            if (httpElement == null)
                throw new SLS.Common.ElectronicException(string.Format("未找到相应彩种的抓取地址：{0}",lotoTypeId));

            var httpUrl = httpElement.Attribute("value").Value;
            var encode = httpElement.Attribute("encode").Value;
            var httpResp = SLS.Common.PF.GetHtml(httpUrl, encode, 10);
            return ParseXml(httpResp);
        }

        /// <summary>
        /// 将抓取到的HTML转换成内部使用XML格式
        /// </summary>
        /// <param name="responseStr"></param>
        /// <returns></returns>
        private string ParseXml(string responseStr)
        {            
            //载入模板
            var ass = Assembly.GetExecutingAssembly();                  
            var sm = new StreamReader(ass.GetManifestResourceStream("SLS.Notify.Xml.OpenTemplate.xml"));

            var xmlResutl = AnalyzeHtmlString(responseStr);
            var s = string.Format(sm.ReadToEnd()
                                    , xmlResutl.LotoTypeId
                                    , xmlResutl.IssueNo
                                    , xmlResutl.St
                                    , xmlResutl.Et
                                    , xmlResutl.WinNumber
                                    , xmlResutl.SaleAmount
                                    , xmlResutl.Balance
                                    , xmlResutl.WinNameType
                                    , xmlResutl.WinMoney
                                    , xmlResutl.WinCount);


            return s;
        }

        private OpenLotteryXmlResult AnalyzeHtmlString(string html)
        {
            var r = new OpenLotteryXmlResult();
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(html);
            //找到中奖号码
            var red = doc.DocumentNode.SelectNodes("//li[@class='ball_red']");
            var blue = doc.DocumentNode.SelectNodes("//li[@class='ball_blue']");
            var winNumber = new StringBuilder();
            foreach (var item in red) {
                winNumber.Append(item.InnerText + " ");
            }
            winNumber.Append("+ " + blue[0].InnerText);
            r.WinNumber = winNumber.ToString();
            //本期销量和奖池滚存
            var saleNodes = doc.DocumentNode.SelectNodes("//span[@class='cfont1 ']");
            r.SaleAmount = saleNodes[0].InnerText;  //格式333,462,942元
            r.Balance = saleNodes[1].InnerText;
            //开奖详情
            var detailNodes = doc.DocumentNode.SelectNodes("//table[@class='kj_tablelist02'][1]//tr[@align='center']");

            return r;
        }

        
    }
}
