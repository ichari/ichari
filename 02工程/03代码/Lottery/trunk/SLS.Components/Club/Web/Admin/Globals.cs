using System;
using System.Data;
using System.IO;
using System.Collections;
using Discuz.Forum;
using Discuz.Common;
using Discuz.Config;

namespace Discuz.Web.Admin.App_Code
{
    public class Globals
    {
        public static void BuildTemplate(string directorypath)
        {
            int templateid = Convert.ToInt32(AdminTemplates.GetAllTemplateList(AppDomain.CurrentDomain.BaseDirectory + "templates/").Select("directory='" + directorypath + "'")[0]["templateid"].ToString());

            Hashtable ht = new Hashtable();
            GetTemplates("default", ht);

            if (directorypath != "default")
            {
                GetTemplates(directorypath, ht);
            }

            ForumPageTemplate forumpagetemplate = new ForumPageTemplate();

            foreach (string key in ht.Keys)
            {
                string filename = key.Split('.')[0];
                forumpagetemplate.GetTemplate("/", directorypath, filename, 1, templateid);
            }

        }

        private static Hashtable GetTemplates(string directorypath, Hashtable ht)
        {
            DirectoryInfo dirinfo = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory + "templates/" + directorypath + "/");

            foreach (FileSystemInfo file in dirinfo.GetFileSystemInfos())
            {
                if (file != null && (file.Extension.ToLower().Equals(".htm") || file.Extension.ToLower().Equals(".config")) && file.Name.IndexOf("_") != 0)
                {
                    ht[file.Name] = file;
                }
            }
            return ht;
        }

    }
}
