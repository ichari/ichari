using System;
using System.Xml;
using System.Text;
using System.Data;
using System.Data.Common;

using Discuz.Common;
using Discuz.Data;
using Discuz.Config;
using Discuz.Entity;

namespace Discuz.Forum
{
	/// <summary>
	/// 论坛模板操作类
	/// </summary>
	public class Templates
	{
		/// <summary>
		/// 模板说明结构, 每个模板目录下均可使用指定结构的xml文件来说明该模板的基本信息
		/// </summary>
		public struct TemplateAboutInfo
		{
			/// <summary>
			/// 模板名称
			/// </summary>
			public string name;
			/// <summary>
			/// 作者
			/// </summary>
			public string author;
			/// <summary>
			/// 创建日期
			/// </summary>
			public string createdate;
			/// <summary>
			/// 模板版本
			/// </summary>
			public string ver;
			/// <summary>
			/// 模板适用的论坛版本
			/// </summary>
			public string fordntver;
			/// <summary>
			/// 版权文字
			/// </summary>
			public string copyright;

		}


		private static object SynObject=new object();


		/// <summary>
		/// 从模板变量文件中获得模板变量值的信息
		/// </summary>
		/// <param name="templatename">模板变量文件名)</param>
		/// <returns>模板变量表</returns>
		public static DataTable GetTemplateVariable1(string templatename)
		{
				
			///存放变量信息的文件 templatevariable.xml是否存在,不存在返回空表
			if (!System.IO.File.Exists(AppDomain.CurrentDomain.BaseDirectory + "templates/" +templatename+"/templatevariable.xml"))
			{
				return (DataTable) null;
			}
			else
			{
				using(DataSet ds=new DataSet())
				{
                    ds.ReadXml(AppDomain.CurrentDomain.BaseDirectory + "templates/" + templatename + "/templatevariable.xml");
					return ds.Tables[0];
				}
			}

		}


		
		/// <summary>
		/// 从模板说明文件中获得模板说明信息
		/// </summary>
		/// <param name="xmlPath">模板路径(不包含文件名)</param>
		/// <returns>模板说明信息</returns>
		public static TemplateAboutInfo GetTemplateAboutInfo(string xmlPath)
		{
			TemplateAboutInfo __aboutinfo = new TemplateAboutInfo();
			__aboutinfo.name = "";
			__aboutinfo.author = "";
			__aboutinfo.createdate = "";
			__aboutinfo.ver = "";
			__aboutinfo.fordntver = "";
			__aboutinfo.copyright = "";
			
			///存放关于信息的文件 about.xml是否存在,不存在返回空串
			if (!System.IO.File.Exists(xmlPath + @"\about.xml"))
			{
				return __aboutinfo;

			}


			XmlDocument xml = new XmlDocument();

			xml.Load(xmlPath + @"\about.xml");

            try
            {
                XmlNode root = xml.SelectSingleNode("about");
                foreach (XmlNode n in root.ChildNodes)
                {
                    if (n.NodeType != XmlNodeType.Comment && n.Name.ToLower() == "template")
                    {
                        XmlAttribute name = n.Attributes["name"];
                        XmlAttribute author = n.Attributes["author"];
                        XmlAttribute createdate = n.Attributes["createdate"];
                        XmlAttribute ver = n.Attributes["ver"];
                        XmlAttribute fordntver = n.Attributes["fordntver"];
                        XmlAttribute copyright = n.Attributes["copyright"];

                        if (name != null)
                        {
                            __aboutinfo.name = name.Value.ToString();
                        }

                        if (author != null)
                        {
                            __aboutinfo.author = author.Value.ToString();
                        }

                        if (createdate != null)
                        {
                            __aboutinfo.createdate = createdate.Value.ToString();
                        }

                        if (ver != null)
                        {
                            __aboutinfo.ver = ver.Value.ToString();
                        }

                        if (fordntver != null)
                        {
                            __aboutinfo.fordntver = fordntver.Value.ToString();
                        }

                        if (copyright != null)
                        {
                            __aboutinfo.copyright = copyright.Value.ToString();
                        }


                    }
                }
            }
            catch
            {
                __aboutinfo = new TemplateAboutInfo();
            }
			return __aboutinfo;
		}
		
		/// <summary>
		/// 获得前台有效的模板列表
		/// </summary>
		/// <returns>模板列表</returns>
		public static DataTable GetValidTemplateList()
		{
			lock(SynObject)
			{
				Discuz.Cache.DNTCache cache = Discuz.Cache.DNTCache.GetCacheService();
				DataTable dt = cache.RetrieveObject("/TemplateList") as DataTable;
				if (dt == null)
				{
                    dt = DatabaseProvider.GetInstance().GetValidTemplateList();
                    cache.AddObject("/TemplateList", dt);
				}
				return dt;
			}
		}

	
		/// <summary>
		/// 获得前台有效的模板ID列表
		/// </summary>
		/// <returns>模板ID列表</returns>
		public static string GetValidTemplateIDList()
		{
			lock(SynObject)
			{
				Discuz.Cache.DNTCache cache = Discuz.Cache.DNTCache.GetCacheService();
				string templateidlist = cache.RetrieveObject("/TemplateIDList") as string;
				if (templateidlist == null)
				{
                    DataTable dt = DatabaseProvider.GetInstance().GetValidTemplateIDList();
                    StringBuilder sb = new StringBuilder();
					foreach(DataRow dr in dt.Rows)
					{
						sb.Append(",");
						sb.Append(dr[0].ToString());
					}
                   
                    try
                    {
                        if (!Utils.StrIsNullOrEmpty(sb.ToString()))
                        {
                            templateidlist = sb.ToString().Substring(1);
                        }
                        cache.AddObject("/TemplateIDList", templateidlist);
                    }
                    finally
                    {
                        dt.Dispose();
                    }
				}
	 			return templateidlist;
	     	}
		}

		/// <summary>
		/// 获得指定的模板信息
		/// </summary>
		/// <param name="templateid">皮肤id</param>
		/// <returns></returns>
		public static TemplateInfo GetTemplateItem(int templateid)
		{
			if (templateid <=0)
			{
				return null;
			}

			TemplateInfo __templateinfo = new TemplateInfo();


			DataRow[] dr = GetValidTemplateList().Select("templateid = " + templateid.ToString());
			if (dr.Length>0)
			{
				__templateinfo.Templateid = Int16.Parse(dr[0]["templateid"].ToString());
				__templateinfo.Name = dr[0]["name"].ToString();
				__templateinfo.Directory = dr[0]["directory"].ToString();
				__templateinfo.Copyright = dr[0]["copyright"].ToString();
			}
			else
			{
				return null;
			}
			return __templateinfo;
		}

	}
}
