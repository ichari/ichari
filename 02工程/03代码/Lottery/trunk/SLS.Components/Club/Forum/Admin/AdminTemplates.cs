using System;
using System.IO;
using System.Data;
using System.Data.Common;

using Discuz.Common;
using Discuz.Data;
using Discuz.Config;
using Discuz.Entity;

namespace Discuz.Forum
{
	/// <summary>
	/// 后台模板操作类
	/// </summary>
	public class AdminTemplates : Templates
	{

		/// <summary>
		/// 添加新的模板项
		/// </summary>
		/// <param name="templateName">模板名称</param>
		/// <param name="directory">模板文件所在目录</param>
		/// <param name="copyright">模板版权文字</param>
		/// <returns>模板id</returns>
		public static int CreateTemplateItem(string templateName, string directory, string copyright)
		{
            return DatabaseProvider.GetInstance().AddTemplate(templateName, directory, copyright);	
        }



		/// <summary>
		/// 删除指定的模板项
		/// </summary>
		/// <param name="templateid">模板id</param>
		public static void DeleteTemplateItem(int templateid)
		{
            DatabaseProvider.GetInstance().DeleteTemplateItem(templateid);
		}



		/// <summary>
		/// 删除指定的模板项列表,
		/// </summary>
		/// <param name="templateidlist">格式为： 1,2,3</param>
		public static void DeleteTemplateItem(string templateidlist)
		{
            DatabaseProvider.GetInstance().DeleteTemplateItem(templateidlist);
		}



		/// <summary>
		/// 获得所有在模板目录下的模板列表(即:子目录名称)
		/// </summary>
		/// <param name="templatePath">模板所在路径</param>
		/// <example>GetAllTemplateList(Utils.GetMapPath(@"..\..\templates\"))</example>
		/// <returns>模板列表</returns>
		public static DataTable GetAllTemplateList(string templatePath)
		{
            DataTable dt = DatabaseProvider.GetInstance().GetAllTemplateList(templatePath);

			dt.Columns.Add("valid", Type.GetType("System.Int16"));
			foreach (DataRow dr in dt.Rows)
			{
				dr["valid"] = 1;
			}

			DirectoryInfo dirinfo = new DirectoryInfo(templatePath);

            int count = DatabaseProvider.GetInstance().GetMaxTemplateId() + 1;
			foreach (DirectoryInfo dir in dirinfo.GetDirectories())
			{
				if (dir != null)
				{
                    if (dir.FullName.IndexOf(".svn") > 0)
                    {
                        continue;
                    }

					bool itemexist = false;
					foreach (DataRow dr in dt.Rows)
					{
						if (dr["directory"].ToString() == dir.Name)
						{
							itemexist = true;
							break;
						}
					}
					if (!itemexist)
					{
						DataRow dr = dt.NewRow();
						// 子目录名
						dr["templateid"] = count.ToString();
						dr["directory"] = dir.Name;
						// 是否是前台有效模板
						dr["valid"] = 0;

						TemplateAboutInfo __aboutinfo = GetTemplateAboutInfo(dir.FullName);
						// 模板名称
						dr["name"] = __aboutinfo.name;
						// 作者
						dr["author"] = __aboutinfo.author;
						// 创建日期
						dr["createdate"] = __aboutinfo.createdate;
						// 模板版本
						dr["ver"] = __aboutinfo.ver;
						// 适用的论坛版本
						dr["fordntver"] = __aboutinfo.fordntver;
						// 版权
						dr["copyright"] = __aboutinfo.copyright;
						dt.Rows.Add(dr);
						count++;
					}

				}
			}

			dt.AcceptChanges();

			return dt;
		}


	}
}