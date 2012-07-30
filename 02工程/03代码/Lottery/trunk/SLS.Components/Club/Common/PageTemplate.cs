using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Reflection;


namespace Discuz.Common
{
	/// <summary>
	/// Template为页面模板类.
	/// </summary>
	public abstract class PageTemplate
	{
		public static Regex[] r = new Regex[23];

		static PageTemplate()
		{

            RegexOptions options = Utils.GetRegexCompiledOptions();

			r[0] = new Regex(@"<%template ([^\[\]\{\}\s]+)%>", options);

			r[1] = new Regex(@"<%loop ((\(([a-zA-Z]+)\) )?)([^\[\]\{\}\s]+) ([^\[\]\{\}\s]+)%>", options);

			r[2] = new Regex(@"<%\/loop%>", options);

			r[3] = new Regex(@"<%while ([^\[\]\{\}\s]+)%>", options);

			r[4] = new Regex(@"<%\/while ([^\[\]\{\}\s]+)%>", options);

			r[5] = new Regex(@"<%if (?:\s*)(([^\s]+)((?:\s*)(\|\||\&\&)(?:\s*)([^\s]+))?)(?:\s*)%>", options);
			//r[5] = new Regex(@"<%if ([^\s]+)%>", options);

			r[6] = new Regex(@"<%else(( (?:\s*)if (?:\s*)(([^\s]+)((?:\s*)(\|\||\&\&)(?:\s*)([^\s]+))?))?)(?:\s*)%>", options);

			r[7] = new Regex(@"<%\/if%>", options);

			//解析{var.a}
			r[8] = new Regex(@"(\{strtoint\(([^\s]+?)\)\})", options);

			//解析{request[a]}
			r[9] = new Regex(@"(<%urlencode\(([^\s]+?)\)%>)", options);

			//解析{var[a]}
			r[10] = new Regex(@"(<%datetostr\(([^\s]+?),(.*?)\)%>)", options);
			r[11] = new Regex(@"(\{([^\.\[\]\{\}\s]+)\.([^\[\]\{\}\s]+)\})", options);

			//解析普通变量{}
			r[12] = new Regex(@"(\{request\[([^\[\]\{\}\s]+)\]\})", options);

			//解析==表达式
			r[13] = new Regex(@"(\{([^\[\]\{\}\s]+)\[([^\[\]\{\}\s]+)\]\})", options);

			//解析==表达式
			r[14] = new Regex(@"({([^\[\]/\{\}='\s]+)})", options);

			//解析普通变量{}
			r[15] = new Regex(@"({([^\[\]/\{\}='\s]+)})", options);

			//解析==表达式
			r[16] = new Regex(@"(([=|>|<|!]=)\\" + "\"" + @"([^\s]*)\\" + "\")", options);
			
			//命名空间
            r[17] = new Regex(@"<%namespace (?:""?)([\s\S]+?)(?:""?)%>", options);
			
			//C#代码
			r[18] = new Regex(@"<%csharp%>([\s\S]+?)<%/csharp%>", options);

			//set标签
			r[19] = new Regex(@"<%set ((\(([a-zA-Z]+)\))?)(?:\s*)\{([^\s]+)\}(?:\s*)=(?:\s*)(.*?)(?:\s*)%>", options);

            //截取字符串
            r[20] = new Regex(@"(<%getsubstring\(([^\s]+?),(.\d*?),([^\s]+?)\)%>)", options);
          
            //repeat标签
            r[21] = new Regex(@"<%repeat\(([^\s]+?)(?:\s*),(?:\s*)([^\s]+?)\)%>", options);

            //继承类Inherits
            r[22] = new Regex(@"<%inherits (?:""?)([\s\S]+?)(?:""?)%>", options);
    
		}


		/// <summary>
		/// 获得模板字符串. 首先查找缓存. 如果不在缓存中则从设置中的模板路径来读取模板文件.
		/// 模板文件的路径在Web.config文件中设置.
		/// 如果读取文件成功则会将内容放于缓存中.
		/// </summary>
		/// <param name="skinName">模板名</param>
		/// <param name="templateName">模板文件的文件名称, 也是缓存中的模板名称.</param>
		/// <param name="nest">嵌套次数</param>
		/// <param name="templateid">模板id</param>
		/// <returns>string值,如果失败则为"",成功则为模板内容的string</returns>
		public virtual string GetTemplate(string forumpath,string skinName, string templateName, int nest,int templateid)
		{
			StringBuilder strReturn = new StringBuilder();
			if (nest < 1)
			{
				nest = 1;
			}
			else if (nest > 5)
			{
				return "";
			}


			string extNamespace = "";
			//string csharpCode = "";
            //生成模板config优先，其次是htm
            string configPathFormatStr = "{0}{1}{2}{3}{4}.config";
			string htmlPathFormatStr = "{0}{1}{2}{3}{4}.htm";
            string configFilePath = string.Format(configPathFormatStr, Utils.GetMapPath(forumpath + "templates"), System.IO.Path.DirectorySeparatorChar, skinName, System.IO.Path.DirectorySeparatorChar, templateName);
            string htmlFilePath = string.Format(htmlPathFormatStr, Utils.GetMapPath(forumpath + "templates"), System.IO.Path.DirectorySeparatorChar, skinName, System.IO.Path.DirectorySeparatorChar, templateName);
            string createFilePath = "";
            string inherits = "Discuz.Web." + templateName;
            //如果指定风格的config模板文件存在
            if (System.IO.File.Exists(configFilePath))
            {
                createFilePath = configFilePath;    //生成config文件模板
            }
            else if (System.IO.File.Exists(htmlFilePath))    //如果指定风格的htm模板文件存在
            {
                createFilePath = htmlFilePath;
            }
            else			//如果指定风格的config和htm模板文件都不存在...
            {
                //默认风格的模板是否存在...
                createFilePath = string.Format(htmlPathFormatStr, Utils.GetMapPath(forumpath + "templates"), System.IO.Path.DirectorySeparatorChar, "default", System.IO.Path.DirectorySeparatorChar, templateName);
                if (!System.IO.File.Exists(createFilePath))
                {
                    return "";
                }
            }

            using (System.IO.StreamReader objReader = new System.IO.StreamReader(createFilePath, Encoding.UTF8))
			{
				System.Text.StringBuilder textOutput = new System.Text.StringBuilder();
				
				textOutput.Append(objReader.ReadToEnd());
				objReader.Close();

				//处理命名空间
				if (nest == 1)
				{
                    //命名空间
                    foreach (Match m in r[17].Matches(textOutput.ToString()))
                    {
                        extNamespace += "\r\n<%@ Import namespace=\"" + m.Groups[1].ToString() + "\" %>";
                        textOutput.Replace(m.Groups[0].ToString(), string.Empty);
                    }

                    //inherits
                    foreach (Match m in r[22].Matches(textOutput.ToString()))
                    {
                        inherits = m.Groups[1].ToString();
                        textOutput.Replace(m.Groups[0].ToString(), string.Empty);
                        break;
                    }
                    if ("\"".Equals(inherits))
                    {
                        inherits = "Discuz.Forum.PageBase";
                    }

				}
				//处理Csharp语句
				foreach (Match m in r[18].Matches(textOutput.ToString()))
				{
					//csharpCode += "\r\n" + m.Groups[1].ToString() + "\r\n";
					textOutput.Replace(m.Groups[0].ToString(), m.Groups[0].ToString().Replace("\r\n", "\r\t\r"));
				}

				textOutput.Replace("\r\n", "\r\r\r");
				textOutput.Replace("<%", "\r\r\n<%");
				textOutput.Replace("%>", "%>\r\r\n");

				textOutput.Replace("<%csharp%>\r\r\n", "<%csharp%>").Replace("\r\r\n<%/csharp%>", "<%/csharp%>");
				

				string[] strlist = Utils.SplitString(textOutput.ToString(), "\r\r\n");
				int count = strlist.GetUpperBound(0);
		
				for (int i = 0; i <= count; i++)
				{
					strReturn.Append(ConvertTags(nest,forumpath, skinName, strlist[i], templateid));
				}
			}
			if (nest == 1)
			{
                //strReturn = strReturn.Replace("\r\r\r\r\r\r", "\r\r\r");   //Codebehind=\"{0}.aspx.cs\" 
                string template = string.Format("<%@ Page language=\"c#\" AutoEventWireup=\"false\" EnableViewState=\"false\" Inherits=\"{0}\" %>\r\n<%@ Import namespace=\"System.Data\" %>\r\n<%@ Import namespace=\"Discuz.Common\" %>\r\n<%@ Import namespace=\"Discuz.Forum\" %>\r\n<%@ Import namespace=\"Discuz.Entity\" %>\r\n{1}\r\n<script runat=\"server\">\r\noverride protected void OnInit(EventArgs e)\r\n{{\r\n\r\n\t/* \r\n\t\tThis page was created by Discuz!NT Template Engine at {2}.\r\n\t\t本页面代码由Discuz!NT模板引擎生成于 {2}. \r\n\t*/\r\n\r\n\tbase.OnInit(e);\r\n{3}\r\n\tResponse.Write(templateBuilder.ToString());\r\n}}\r\n</script>\r\n", inherits, extNamespace, DateTime.Now.ToString(), strReturn.ToString());

				string pageDir = Utils.GetMapPath(forumpath + "aspx\\" + templateid.ToString() + "\\");
				if (!Directory.Exists(pageDir))
				{
					Utils.CreateDir(pageDir);
				}

				string outputPath = pageDir  + templateName + ".aspx";
				
					
				
				using (FileStream fs = new FileStream(outputPath, FileMode.Create,FileAccess.ReadWrite, FileShare.ReadWrite))
				{
					Byte[] info = System.Text.Encoding.UTF8.GetBytes(template);
					fs.Write(info, 0, info.Length);
					fs.Close();
				}
				
			}
			return strReturn.ToString();
		}
		
		/// <summary>
		/// 转换标签
		/// </summary>
		/// <param name="nest">深度</param>
		/// <param name="skinName">模板名称</param>
		/// <param name="inputStr">模板内容</param>
		/// <param name="templateid">模板id</param>
		/// <returns></returns>
		private string ConvertTags(int nest,string forumpath, string skinName, string inputStr, int templateid)
		{
			string strReturn = "";
			bool IsCodeLine;
			//Regex r;
			string strTemplate;
			strTemplate = inputStr.Replace("\\", "\\\\");
			strTemplate = strTemplate.Replace("\"", "\\\"");
			strTemplate = strTemplate.Replace("</script>", "</\" + \"script>");
			//strTemplate = strlist[i];
			IsCodeLine = false;
		

			foreach (Match m in r[0].Matches(strTemplate))
			{
				IsCodeLine = true;
                strTemplate = strTemplate.Replace(m.Groups[0].ToString(), "\r\n" + GetTemplate(forumpath,skinName, m.Groups[1].ToString(), nest + 1, templateid) + "\r\n");
			}

			//r = new Regex(@"<%loop ([^\[\]\{\}\s]+) ([^\[\]\{\}\s]+)%>", RegexOptions.IgnoreCase|RegexOptions.Singleline|RegexOptions.Compiled);
			foreach (Match m in r[1].Matches(strTemplate))
			{
				IsCodeLine = true;
				if (m.Groups[3].ToString() == "")
				{
					strTemplate = strTemplate.Replace(m.Groups[0].ToString(),
						string.Format("\r\n\tint {0}__loop__id=0;\r\n\tforeach(DataRow {0} in {1}.Rows)\r\n\t{{\r\n\t\t{0}__loop__id++;\r\n", m.Groups[4].ToString(), m.Groups[5].ToString()));
				}
				else
				{
					strTemplate = strTemplate.Replace(m.Groups[0].ToString(),
						string.Format("\r\n\tint {1}__loop__id=0;\r\n\tforeach({0} {1} in {2})\r\n\t{{\r\n\t\t{1}__loop__id++;\r\n", m.Groups[3].ToString(), m.Groups[4].ToString(), m.Groups[5].ToString()));
				}
			}

			//r = new Regex(@"<%\/loop%>", RegexOptions.IgnoreCase|RegexOptions.Singleline|RegexOptions.Compiled);
			foreach (Match m in r[2].Matches(strTemplate))
			{
				IsCodeLine = true;
				strTemplate = strTemplate.Replace(m.Groups[0].ToString(),
					"\r\n\t}\t//end loop\r\n");
			}

			//r = new Regex(@"<%while ([^\[\]\{\}\s]+)%>", RegexOptions.IgnoreCase|RegexOptions.Singleline|RegexOptions.Compiled);
			foreach (Match m in r[3].Matches(strTemplate))
			{
				IsCodeLine = true;
				strTemplate = strTemplate.Replace(m.Groups[0].ToString(),
					string.Format("\r\n\tint {0}__loop__id=0;\r\nwhile({0}.Read())\r\n\t{{\r\n{0}__loop__id++;\r\n", m.Groups[1].ToString()));
			}

			//r = new Regex(@"<%\/while ([^\[\]\{\}\s]+)%>", RegexOptions.IgnoreCase|RegexOptions.Singleline|RegexOptions.Compiled);
			foreach (Match m in r[4].Matches(strTemplate))
			{
				IsCodeLine = true;
				strTemplate = strTemplate.Replace(m.Groups[0].ToString(),
					"\r\n\t}\t//end while\r\n" + m.Groups[1] + ".Close();\r\n");
			}

			//r = new Regex(@"<%if ([^\s]+)%>", RegexOptions.IgnoreCase|RegexOptions.Singleline|RegexOptions.Compiled);
			foreach (Match m in r[5].Matches(strTemplate))
			{
				IsCodeLine = true;
				strTemplate = strTemplate.Replace(m.Groups[0].ToString(),
					"\r\n\tif (" + m.Groups[1].ToString().Replace("\\\"", "\"") + ")\r\n\t{\r\n");
			}


			//r = new Regex(@"<%else%>", RegexOptions.IgnoreCase|RegexOptions.Singleline|RegexOptions.Compiled);
			foreach (Match m in r[6].Matches(strTemplate))
			{
				IsCodeLine = true;
				if (m.Groups[1].ToString() == string.Empty)
				{
					strTemplate = strTemplate.Replace(m.Groups[0].ToString(),
					"\r\n\t}\r\n\telse\r\n\t{\r\n");
				}
				else
				{
					strTemplate = strTemplate.Replace(m.Groups[0].ToString(),
						"\r\n\t}\r\n\telse if (" + m.Groups[3].ToString().Replace("\\\"", "\"") + ")\r\n\t{\r\n");
				}
			}

			//r = new Regex(@"<%\/if%>", RegexOptions.IgnoreCase|RegexOptions.Singleline|RegexOptions.Compiled);
			foreach (Match m in r[7].Matches(strTemplate))
			{
				IsCodeLine = true;
				strTemplate = strTemplate.Replace(m.Groups[0].ToString(),
					"\r\n\t}\t//end if\r\n");
			}

			//解析set
			//
			foreach (Match m in r[19].Matches(strTemplate.ToString()))
			{
				IsCodeLine = true;
				string type = "";
				if (m.Groups[3].ToString() != string.Empty)
				{
					type = m.Groups[3].ToString();
				}
				strTemplate = strTemplate.Replace(m.Groups[0].ToString(),
                    string.Format("\t{0} {1} = {2};\r\n\t", type, m.Groups[4].ToString(), m.Groups[5].ToString()).Replace("\\\"", "\"")
					);
			}

            //解析repeat @"<%repeat\(([^\s]+?)(?:\s*),(?:\s*)([0-9]*)\)%>"
            foreach (Match m in r[21].Matches(strTemplate.ToString()))
            {
                IsCodeLine = true;
                //if (Utils.StrToInt(m.Groups[2], 0) > 0)
                //{
 				    strTemplate = strTemplate.Replace(m.Groups[0].ToString(),
                    "\tfor (int i = 0; i < " + m.Groups[2] + "; i++)\r\n\t{\r\n\t\ttemplateBuilder.Append(" + m.Groups[1].ToString().Replace("\\\"", "\"").Replace("\\\\", "\\") + ");\r\n\t}\r\n");

                //}
            }




			//r = new Regex(@"(\{strtoint\(([^\s]+?)\)\})", RegexOptions.IgnoreCase|RegexOptions.Singleline|RegexOptions.Compiled);
			foreach (Match m in r[8].Matches(strTemplate))
			{
				strTemplate = strTemplate.Replace(m.Groups[0].ToString(),
					"Utils.StrToInt(" + m.Groups[2].ToString() + ", 0)");
			}
			//r[8].Replace(strTemplate, "Utils.StrToInt($2, 0)");

			//r = new Regex(@"(<%urlencode\(([^\s]+?)\)%>)", RegexOptions.IgnoreCase|RegexOptions.Singleline|RegexOptions.Compiled);
			foreach (Match m in r[9].Matches(strTemplate))
			{
				IsCodeLine = true;
				strTemplate = strTemplate.Replace(m.Groups[0].ToString(),
					"templateBuilder.Append(Utils.UrlEncode(" + m.Groups[2].ToString() + "));");
			}

			//r = new Regex(@"(<%datetostr\(([^\s]+?),(.*?)\)%>)", RegexOptions.IgnoreCase|RegexOptions.Singleline|RegexOptions.Compiled);
			foreach (Match m in r[10].Matches(strTemplate))
			{
				IsCodeLine = true;
				strTemplate = strTemplate.Replace(m.Groups[0].ToString(),
					string.Format("\ttemplateBuilder.Append(Convert.ToDateTime({0}).ToString(\"{1}\"));", m.Groups[2].ToString(), m.Groups[3].ToString().Replace("\\\"", string.Empty)));
			}

            //解析substring
            //
            foreach (Match m in r[20].Matches(strTemplate))
            {
                IsCodeLine = true;
                strTemplate = strTemplate.Replace(m.Groups[0].ToString(),
                    string.Format("\ttemplateBuilder.Append(Utils.GetUnicodeSubString({0},{1},\"{2}\"));", m.Groups[2].ToString(), m.Groups[3].ToString(), m.Groups[4].ToString().Replace("\\\"", string.Empty)));

                //  strTemplate = strTemplate.Replace(m.Groups[0].ToString(),
                //      string.Format("templateBuilder.Append(Convert.ToDateTime({0}).ToString(\"{1}\"));", m.Groups[2].ToString(), m.Groups[3].ToString().Replace("\\\"", string.Empty)));

            }
					
			//解析{var.a}
			//r = new Regex(@"(\{([^\.\[\]\{\}\s]+)\.([^\[\]\{\}\s]+)\})", RegexOptions.IgnoreCase|RegexOptions.Singleline|RegexOptions.Compiled);
			foreach (Match m in r[11].Matches(strTemplate))
			{
				if (IsCodeLine)
				{
					strTemplate = strTemplate.Replace(m.Groups[0].ToString(),
						string.Format("{0}.{1}{2}", m.Groups[2].ToString(), Utils.CutString(m.Groups[3].ToString(), 0, 1).ToUpper(), m.Groups[3].ToString().Substring(1, m.Groups[3].ToString().Length - 1)));
				}
				else
				{
					strTemplate = strTemplate.Replace(m.Groups[0].ToString(),
						string.Format("\" + {0}.{1}{2}.ToString().Trim() + \"", m.Groups[2].ToString(), Utils.CutString(m.Groups[3].ToString(), 0, 1).ToUpper(), m.Groups[3].ToString().Substring(1, m.Groups[3].ToString().Length - 1)));
				}

			}

			//解析{request[a]}
			//r = new Regex(@"(\{request\[([^\[\]\{\}\s]+)\]\})", RegexOptions.IgnoreCase|RegexOptions.Singleline|RegexOptions.Compiled);
			foreach (Match m in r[12].Matches(strTemplate))
			{
				if (IsCodeLine)
				{
					strTemplate = strTemplate.Replace(m.Groups[0].ToString(), "DNTRequest.GetString(\"" + m.Groups[2].ToString() + "\")");
				}
				else
				{
					strTemplate = strTemplate.Replace(m.Groups[0].ToString(), string.Format("\" + DNTRequest.GetString(\"{0}\") + \"", m.Groups[2].ToString()));
				}

			}

			//解析{var[a]}
			//r = new Regex(@"(\{([^\[\]\{\}\s]+)\[([^\[\]\{\}\s]+)\]\})", RegexOptions.IgnoreCase|RegexOptions.Singleline|RegexOptions.Compiled);
			foreach (Match m in r[13].Matches(strTemplate))
			{
				if (IsCodeLine)
				{
					if (Utils.IsNumeric(m.Groups[3].ToString()))
					{
						strTemplate = strTemplate.Replace(m.Groups[0].ToString(), m.Groups[2].ToString() + "[" + m.Groups[3].ToString() + "].ToString().Trim()");
					}
					else
					{
						if (m.Groups[3].ToString() == "_id")
						{
							strTemplate = strTemplate.Replace(m.Groups[0].ToString(), m.Groups[2].ToString() + "__loop__id");
						}
						else
						{
							strTemplate = strTemplate.Replace(m.Groups[0].ToString(), m.Groups[2].ToString() + "[\"" + m.Groups[3].ToString() + "\"].ToString().Trim()");
						}
					}
				}
				else
				{
					if (Utils.IsNumeric(m.Groups[3].ToString()))
					{
						strTemplate = strTemplate.Replace(m.Groups[0].ToString(), string.Format("\" + {0}[{1}].ToString().Trim() + \"", m.Groups[2].ToString(), m.Groups[3].ToString()));
					}
					else
					{
						if (m.Groups[3].ToString() == "_id")
						{
							strTemplate = strTemplate.Replace(m.Groups[0].ToString(), string.Format("\" + {0}__loop__id.ToString() + \"", m.Groups[2].ToString()));
						}
						else
						{
							strTemplate = strTemplate.Replace(m.Groups[0].ToString(), string.Format("\" + {0}[\"{1}\"].ToString().Trim() + \"", m.Groups[2].ToString(), m.Groups[3].ToString()));
						}
					}
				}
				//strTemplate = "\"" + strTemplate + "\\r\\n\");\r\n";
			}

			strTemplate = ReplaceSpecialTemplate(forumpath,skinName,strTemplate);

			//r = new Regex(@"({([^\[\]/\{\}='\s]+)})", RegexOptions.IgnoreCase|RegexOptions.Singleline|RegexOptions.Compiled);
			foreach (Match m in r[14].Matches(strTemplate))
			{
				if (m.Groups[0].ToString() == "{commonversion}")
				{
					strTemplate = strTemplate.Replace(m.Groups[0].ToString(), Utils.GetAssemblyVersion());
				}
			}
                    
					
			
			//解析普通变量{}
			//r = new Regex(@"({([^\[\]/\{\}='\s]+)})", RegexOptions.IgnoreCase|RegexOptions.Singleline|RegexOptions.Compiled);
			foreach (Match m in r[15].Matches(strTemplate))
			{
				//IsCodeLine = false;
				if (IsCodeLine)
				{
					strTemplate = strTemplate.Replace(m.Groups[0].ToString(),
						m.Groups[2].ToString());
				}
				else
				{
					strTemplate = strTemplate.Replace(m.Groups[0].ToString(),
						string.Format("\" + {0}.ToString() + \"", m.Groups[2].ToString().Trim()));
				}

			}
					
					
			//解析==表达式
			//r = new Regex(@"(([>=|==|<=|!=])\\" + "\"" + @"([^\[\]\{\}\s]*)\\" + "\")", RegexOptions.IgnoreCase|RegexOptions.Singleline|RegexOptions.Compiled);
			//r = new Regex(@"(([=|>|<|!]=)\\" + "\"" + @"([^\s]*)\\" + "\")", RegexOptions.IgnoreCase|RegexOptions.Singleline|RegexOptions.Compiled);
			foreach (Match m in r[16].Matches(strTemplate))
			{
				strTemplate = strTemplate.Replace(m.Groups[0].ToString(),
					m.Groups[2].ToString() + "\"" + m.Groups[3].ToString() + "\"");

			}
			//r[16].Replace(strTemplate, "$2\"$3\"");
				
				
			//解析csharpcode
			//
			foreach (Match m in r[18].Matches(strTemplate.ToString()))
			{
				IsCodeLine = true;
				strTemplate = strTemplate.Replace(m.Groups[0].ToString(), m.Groups[1].ToString().Replace("\r\t\r", "\r\n\t").Replace("\\\"", "\""));
			}

           
					
			//HttpContext.Current.Response.Write(i.ToString() + "* " + HttpUtility.HtmlEncode(strTemplate) + "<br />");
			if (IsCodeLine)
			{
				strReturn = strTemplate + "\r\n";
				//System.Web.HttpContext.Current.Response.Write(" " + i.ToString() + strTemplate);
			}
			else
			{
				if (strTemplate.Trim() != "")
				{
					StringBuilder sb = new StringBuilder();
					foreach (string temp in Utils.SplitString(strTemplate,"\r\r\r"))
					{
						if (temp.Trim() == "")
							continue;
						sb.Append("\ttemplateBuilder.Append(\"" + temp + "\\r\\n\");\r\n");
					}
					strReturn = sb.ToString();
				}
				//System.Web.HttpContext.Current.Response.Write(" *" + i.ToString());
			}
			return strReturn;
		}



		/// <summary>
		/// 解析特殊变量
		/// </summary>
		/// <returns></returns>
		public abstract string ReplaceSpecialTemplate(string forumpath,string skinName,string strTemplate);
		

	}



}