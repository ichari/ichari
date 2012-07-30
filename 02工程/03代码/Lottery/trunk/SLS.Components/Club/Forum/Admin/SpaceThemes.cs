using System.Xml;

namespace Discuz.Forum
{
    /// <summary>
    /// 论坛模板操作类
    /// </summary>
    public class SpaceThemes
    {
        /// <summary>
        /// 模板说明结构, 每个模板目录下均可使用指定结构的xml文件来说明该模板的基本信息
        /// </summary>
        public struct SpaceThemeAboutInfo
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


        /// <summary>
        /// 从模板说明文件中获得模板说明信息
        /// </summary>
        /// <param name="xmlPath">模板路径(不包含文件名)</param>
        /// <returns>模板说明信息</returns>
        public static SpaceThemeAboutInfo GetThemeAboutInfo(string xmlPath)
        {
            SpaceThemeAboutInfo __aboutinfo = new SpaceThemeAboutInfo();
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
            return __aboutinfo;
        }
    }
}
