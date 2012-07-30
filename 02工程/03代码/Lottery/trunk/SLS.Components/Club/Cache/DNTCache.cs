using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Xml;
using System.IO;

using XmlElement = System.Xml.XmlElement;
using Discuz.Entity;
using Discuz.Common;
using Discuz.Config;
using Discuz.Data;
using System.Text.RegularExpressions;

namespace Discuz.Cache
{
    /// <summary>
    /// Discuz!NT缓存类
    /// 对Discuz!NT论坛缓存进行全局控制管理
    /// </summary>
    public class DNTCache
    {
        private static XmlElement objectXmlMap;
        private static ICacheStrategy cs;
        private static volatile DNTCache instance = null;
        private static object lockHelper = new object();
        private static XmlDocument rootXml = new XmlDocument();

        private static System.Timers.Timer cacheConfigTimer = new System.Timers.Timer(15000);

        /// <summary>
        /// 构造函数
        /// </summary>
        private DNTCache()
        {
            cs = new DefaultCacheStrategy();
            objectXmlMap = rootXml.CreateElement("Cache");
            //建立内部XML文档.
            rootXml.AppendChild(objectXmlMap);

            //LogVisitor clv = new CacheLogVisitor();
            //cs.Accept(clv);

            cacheConfigTimer.AutoReset = true;
            cacheConfigTimer.Enabled = true;
            cacheConfigTimer.Elapsed += new System.Timers.ElapsedEventHandler(Timer_Elapsed);
            cacheConfigTimer.Start();
        }

        private static void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            //检查并移除相应的缓存项
            instance = CachesFileMonitor.CheckAndRemoveCache(instance);
        }

        /// <summary>
        /// 单体模式返回当前类的实例
        /// </summary>
        /// <returns></returns>
        public static DNTCache GetCacheService()
        {

            if (instance == null)
            {
                lock (lockHelper)
                {
                    if (instance == null)
                    {
                        instance = CachesFileMonitor.CheckAndRemoveCache(new DNTCache());
                    }
                }
            }

            return instance;
        }

        /// <summary>
        /// 添加单值路径,在XML映射文档中加入当前对象信息
        /// </summary>
        /// <param name="xpath">分级对象的路径 </param>
        /// <param name="o">被缓存的对象</param>
        public virtual void AddSingleObject(string xpath, object o)
        {
            AddObject(xpath + "/Single", o);
        }


        /// <summary>
        /// 添加单值路径,在XML映射文档中加入当前对象信息
        /// </summary>
        /// <param name="xpath">分级对象的路径 </param>
        /// <param name="o">被缓存的对象</param>
        public virtual void AddSingleObject(string xpath, object o, string[] files)
        {
            AddObject(xpath + "/Single", o, files);
        }


        /// <summary>
        /// 添加多点路径,在当前的键上插入多值的域
        /// </summary>
        /// <param name="xpath">键值</param>
        /// <param name="objValue">插入的数组</param>
        /// <returns></returns>
        public virtual bool AddMultiObjects(string xpath, object[] objValue)
        {
            lock (lockHelper)
            {
                if (xpath != null && xpath != "" && xpath.Length > 0 && objValue != null)
                {

                    for (int i = 0; i < objValue.Length; i++)
                    {
                        AddObject(xpath + "/Multi/_" + i.ToString(), objValue[i]);
                    }

                    return true;
                }
                return false;
            }
        }




        /// <summary>
        /// 添加多点路径,在当前的键上插入多值的域
        /// </summary>
        /// <param name="xpath">键值</param>
        /// <param name="objValue">插入的数组</param>
        /// <returns></returns>
        public virtual bool AddMultiObjects(string xpath, object[] objValue, string[] files)
        {
            lock (lockHelper)
            {
                if (xpath != null && xpath != "" && xpath.Length != 0 && objValue != null)
                {

                    for (int i = 0; i < objValue.Length; i++)
                    {
                        AddObject(xpath + "/Multi/_" + i.ToString(), objValue[i], files);
                    }
                }
                return true;
            }
        }


        /// <summary>
        /// 缓存标志位类型
        /// </summary>
        private enum CacheFlag
        {
            /// <summary>
            /// 缓存失败
            /// </summary>
            Cachefails,

            /// <summary>
            /// 缓存中无数据
            /// </summary>
            CacheNoData,

            /// <summary>
            /// 缓存中有数据
            /// </summary>
            CacheHaveData
        }

        /// <summary>
        /// 添加表类型数据缓存
        /// </summary>
        /// <param name="xpath">分级对象的路径</param>
        /// <param name="dt">缓存的数据</param>
        public virtual void AddObject(string xpath, DataTable dt)
        {
            lock (lockHelper)
            {
                if (dt.Rows.Count > 0)
                {
                    AddObject(xpath + "flag", CacheFlag.CacheHaveData);
                }
                else
                {
                    AddObject(xpath + "flag", CacheFlag.CacheNoData);
                }
                AddObject(xpath, (object)dt);
            }
        }

        /// <summary>
        /// 添加字符串类型数据缓存
        /// </summary>
        /// <param name="xpath">分级对象的路径</param>
        /// <param name="str">缓存的数据</param>
        public virtual void AddObject(string xpath, string str)
        {
            lock (lockHelper)
            {
                if ((str != null) && (str != ""))
                {
                    AddObject(xpath + "flag", CacheFlag.CacheHaveData);
                }
                else
                {
                    AddObject(xpath + "flag", CacheFlag.CacheNoData);
                }
                AddObject(xpath, (object)str);
            }
        }

        /// <summary>
        /// 添加数据行类型数据缓存
        /// </summary>
        /// <param name="xpath">分级对象的路径</param>
        /// <param name="dr">缓存的数据</param>
        public virtual void AddObject(string xpath, DataRow dr)
        {
            lock (lockHelper)
            {
                if (dr == null)
                {
                    AddObject(xpath + "flag", CacheFlag.CacheHaveData);
                }
                else
                {
                    AddObject(xpath + "flag", CacheFlag.CacheNoData);
                }
                AddObject(xpath, (object)dr);
            }
        }

        /// <summary>
        /// 添加int类型数据缓存
        /// </summary>
        /// <param name="xpath">分级对象的路径</param>
        /// <param name="intvalue">缓存的数据</param>
        public virtual void AddObject(string xpath, int intvalue)
        {
            lock (lockHelper)
            {
                if (intvalue.Equals(null))
                {
                    AddObject(xpath + "flag", CacheFlag.CacheHaveData);
                }
                else
                {
                    AddObject(xpath + "flag", CacheFlag.CacheNoData);
                }
                AddObject(xpath, (object)intvalue);
            }
        }

        /// <summary>
        /// 添加CustomEditorButtonInfo类型数据缓存
        /// </summary>
        /// <param name="xpath">分级对象的路径</param>
        /// <param name="cuebiArray">缓存的数据</param>
        public virtual void AddObject(string xpath, CustomEditorButtonInfo[] cuebiArray)
        {
            lock (lockHelper)
            {
                if (cuebiArray.Length > 0)
                {
                    AddObject(xpath + "flag", CacheFlag.CacheHaveData);
                }
                else
                {
                    AddObject(xpath + "flag", CacheFlag.CacheNoData);
                }
                AddObject(xpath, (object)cuebiArray);
            }
        }

        /// <summary>
        /// 添加字符串数组类型数据缓存
        /// </summary>
        /// <param name="xpath">分级对象的路径</param>
        /// <param name="strArray">缓存的数据</param>
        public virtual void AddObject(string xpath, string[] strArray)
        {
            lock (lockHelper)
            {
                if (strArray.Length > 0)
                {
                    AddObject(xpath + "flag", CacheFlag.CacheHaveData);
                }
                else
                {
                    AddObject(xpath + "flag", CacheFlag.CacheNoData);
                }
                AddObject(xpath, (object)strArray);
            }
        }


        /// <summary>
        /// 添加userGroupInfoArray类型数据缓存
        /// </summary>
        /// <param name="xpath">分级对象的路径</param>
        /// <param name="userGroupInfoArray">缓存的数据</param>
        public virtual void AddObject(string xpath, UserGroupInfo[] userGroupInfoArray)
        {
            lock (lockHelper)
            {
                if (userGroupInfoArray.Length > 0)
                {
                    AddObject(xpath + "flag", CacheFlag.CacheHaveData);
                }
                else
                {
                    AddObject(xpath + "flag", CacheFlag.CacheNoData);
                }
                AddObject(xpath, (object)userGroupInfoArray);
            }
        }


        /// <summary>
        /// 添加SmiliesInfo类型数据缓存
        /// </summary>
        /// <param name="xpath">分级对象的路径</param>
        /// <param name="smiliesInfoArray">缓存的数据</param>
        public virtual void AddObject(string xpath, SmiliesInfo[] smiliesInfoArray)
        {
            lock (lockHelper)
            {
                if (smiliesInfoArray.Length > 0)
                {
                    AddObject(xpath + "flag", CacheFlag.CacheHaveData);
                }
                else
                {
                    AddObject(xpath + "flag", CacheFlag.CacheNoData);
                }
                AddObject(xpath, (object)smiliesInfoArray);
            }
        }


        /// <summary>
        /// 添加ModeratorInfo类型数据缓存
        /// </summary>
        /// <param name="xpath">分级对象的路径</param>
        /// <param name="moderatorInfoArray">缓存的数据</param>
        public virtual void AddObject(string xpath, ModeratorInfo[] moderatorInfoArray)
        {
            lock (lockHelper)
            {
                if (moderatorInfoArray.Length > 0)
                {
                    AddObject(xpath + "flag", CacheFlag.CacheHaveData);
                }
                else
                {
                    AddObject(xpath + "flag", CacheFlag.CacheNoData);
                }
                AddObject(xpath, (object)moderatorInfoArray);
            }
        }

        /// <summary>
        /// 添加ForumInfo类型数据缓存
        /// </summary>
        /// <param name="xpath">分级对象的路径</param>
        /// <param name="forumInfoArray">缓存的数据</param>
        public virtual void AddObject(string xpath, ForumInfo[] forumInfoArray)
        {
            lock (lockHelper)
            {
                if (forumInfoArray.Length > 0)
                {
                    AddObject(xpath + "flag", CacheFlag.CacheHaveData);
                }
                else
                {
                    AddObject(xpath + "flag", CacheFlag.CacheNoData);
                }
                AddObject(xpath, (object)forumInfoArray);
            }
        }


        /// <summary>
        /// 添加AdminGroupInfo类型数据缓存
        /// </summary>
        /// <param name="xpath">分级对象的路径</param>
        /// <param name="admingroupinfo">缓存的数据</param>
        public virtual void AddObject(string xpath, AdminGroupInfo[] admingroupinfo)
        {
            lock (lockHelper)
            {
                if (admingroupinfo.Length > 0)
                {
                    AddObject(xpath + "flag", CacheFlag.CacheHaveData);
                }
                else
                {
                    AddObject(xpath + "flag", CacheFlag.CacheNoData);
                }
                AddObject(xpath, (object)admingroupinfo);
            }
        }

        /// <summary>
        /// 添加SortedList类型数据缓存
        /// </summary>
        /// <param name="xpath">分级对象的路径</param>
        /// <param name="__sortedlist">缓存的数据</param>
#if NET1
        public virtual void AddObject(string xpath, SortedList __sortedlist)    
#else
        public virtual void AddObject(string xpath, Discuz.Common.Generic.SortedList<int, object> __sortedlist)
#endif
        {
            lock (lockHelper)
            {
                if (__sortedlist.Count > 0)
                {
                    AddObject(xpath + "flag", CacheFlag.CacheHaveData);
                }
                else
                {
                    AddObject(xpath + "flag", CacheFlag.CacheNoData);
                }
                AddObject(xpath, (object)__sortedlist);
            }
        }

        /// <summary>
        /// 添加CollectionBase类型数据缓存
        /// </summary>
        /// <param name="xpath">分级对象的路径</param>
        /// <param name="__collection">缓存的数据</param>
        public virtual void AddObject(string xpath, CollectionBase __collection)
        {
            lock (lockHelper)
            {
                if (__collection.Count > 0)
                {
                    AddObject(xpath + "flag", CacheFlag.CacheHaveData);
                }
                else
                {
                    AddObject(xpath + "flag", CacheFlag.CacheNoData);
                }
                AddObject(xpath, (object)__collection);
            }
        }


        /// <summary>
        /// 添加ICollection类型数据缓存
        /// </summary>
        /// <param name="xpath">分级对象的路径</param>
        /// <param name="__collection">缓存的数据</param>
        public virtual void AddObject(string xpath, ICollection __collection)
        {
            lock (lockHelper)
            {
                if (__collection.Count > 0)
                {
                    AddObject(xpath + "flag", CacheFlag.CacheHaveData);
                }
                else
                {
                    AddObject(xpath + "flag", CacheFlag.CacheNoData);
                }
                AddObject(xpath, (object)__collection);
            }
        }



        /// <summary>
        /// 在XML映射文档中的指定路径,加入当前对象信息
        /// </summary>
        /// <param name="xpath">分级对象的路径 </param>
        /// <param name="o">被缓存的对象</param>
        public virtual void AddObject(string xpath, object o)
        {
            lock (lockHelper)
            {
                //当缓存到期时间为0或负值,则不再放入缓存
                if (cs.TimeOut <= 0) return;

                //整理XPATH表达式信息
                string newXpath = PrepareXpath(xpath);
                int separator = newXpath.LastIndexOf("/");
                //找到相关的组名
                string group = newXpath.Substring(0, separator);
                //找到相关的对象
                string element = newXpath.Substring(separator + 1);

                XmlNode groupNode = objectXmlMap.SelectSingleNode(group);

                //建立对象的唯一键值, 用以映射XML和缓存对象的键
                string objectId = "";

                XmlNode node = objectXmlMap.SelectSingleNode(PrepareXpath(xpath));
                if (node != null)
                {
                    objectId = node.Attributes["objectId"].Value;
                }

                if (objectId == "")
                {
                    groupNode = CreateNode(group);
                    objectId = Guid.NewGuid().ToString();
                    //建立新元素和一个属性 for this perticular object
                    XmlElement objectElement = objectXmlMap.OwnerDocument.CreateElement(element);
                    XmlAttribute objectAttribute = objectXmlMap.OwnerDocument.CreateAttribute("objectId");
                    objectAttribute.Value = objectId;
                    objectElement.Attributes.Append(objectAttribute);
                    //为XML文档建立新元素
                    groupNode.AppendChild(objectElement);
                }
                else
                {
                    //建立新元素和一个属性 for this perticular object
                    XmlElement objectElement = objectXmlMap.OwnerDocument.CreateElement(element);
                    XmlAttribute objectAttribute = objectXmlMap.OwnerDocument.CreateAttribute("objectId");
                    objectAttribute.Value = objectId;
                    objectElement.Attributes.Append(objectAttribute);
                    //为XML文档建立新元素
                    groupNode.ReplaceChild(objectElement, node);
                }

                //向缓存加入新的对象
                cs.AddObject(objectId, o);
            }
        }



        /// <summary>
        /// 在XML映射文档中的指定路径,加入当前对象信息
        /// </summary>
        /// <param name="xpath">分级对象的路径 </param>
        /// <param name="o">被缓存的对象</param>
        public virtual void AddObject(string xpath, object o, string[] files)
        {
            xpath = xpath.Replace(" ", "_SPACE_");    //如果xpath中出现空格，则将空格替换为_SPACE_
            lock (lockHelper)
            {
                //当缓存到期时间为0或负值,则不再放入缓存
                if (cs.TimeOut <= 0) return;

                //整理XPATH表达式信息
                string newXpath = PrepareXpath(xpath);
                int separator = newXpath.LastIndexOf("/");
                //找到相关的组名
                string group = newXpath.Substring(0, separator);
                //找到相关的对象
                string element = newXpath.Substring(separator + 1);

                XmlNode groupNode = objectXmlMap.SelectSingleNode(group);
                //建立对象的唯一键值, 用以映射XML和缓存对象的键
                string objectId = "";

                XmlNode node = objectXmlMap.SelectSingleNode(PrepareXpath(xpath));
                if (node != null)
                {
                    objectId = node.Attributes["objectId"].Value;
                }
                if (objectId == "")
                {
                    groupNode = CreateNode(group);
                    objectId = Guid.NewGuid().ToString();
                    //建立新元素和一个属性 for this perticular object
                    XmlElement objectElement = objectXmlMap.OwnerDocument.CreateElement(element);
                    XmlAttribute objectAttribute = objectXmlMap.OwnerDocument.CreateAttribute("objectId");
                    objectAttribute.Value = objectId;
                    objectElement.Attributes.Append(objectAttribute);
                    //为XML文档建立新元素
                    groupNode.AppendChild(objectElement);
                }
                else
                {
                    //建立新元素和一个属性 for this perticular object
                    XmlElement objectElement = objectXmlMap.OwnerDocument.CreateElement(element);
                    XmlAttribute objectAttribute = objectXmlMap.OwnerDocument.CreateAttribute("objectId");
                    objectAttribute.Value = objectId;
                    objectElement.Attributes.Append(objectAttribute);
                    //为XML文档建立新元素
                    groupNode.ReplaceChild(objectElement, node);
                }

                //向缓存加入新的对象
                cs.AddObjectWithFileChange(objectId, o, files);
            }
        }





        /// <summary>
        /// 通过分级地址返回单值路径上被缓存的对象
        /// </summary>
        /// <param name="xpath">分级对象的路径</param>
        /// <returns>被缓存的对象</returns>
        public virtual object RetrieveSingleObject(string xpath)
        {
            xpath = xpath.Replace(" ", "_SPACE_");    //如果xpath中出现空格，则将空格替换为_SPACE_
            object o = null;
            XmlNode node = objectXmlMap.SelectSingleNode(PrepareXpath(xpath + "/Single"));
            //如果分级地址存在,则返回,否则为NULL
            if (node != null)
            {
                string objectId = node.Attributes["objectId"].Value;
                //通过缓存策略返回对象
                o = cs.RetrieveObject(objectId);
            }
            return o;
        }


        /// <summary>
        /// 返回当前路径的多值域部分
        /// </summary>
        /// <param name="xpath">分级对象的路径</param>
        /// <returns></returns>
        public virtual object[] RetrieveMultiObjects(string xpath)
        {
            return RetrieveObjectList(xpath + "/Multi");
        }



        /// <summary>
        /// 返回指定XML路径下的数据项
        /// </summary>
        /// <param name="xpath">分级对象的路径</param>
        /// <returns></returns>
        public virtual object RetrieveOriginObject(string xpath)
        {
            XmlNode node = objectXmlMap.SelectSingleNode(PrepareXpath(xpath));
            if (node != null)
            {
                string objectId = node.Attributes["objectId"].Value;

                return cs.RetrieveObject(objectId);
            }
            return null;
        }


        /// <summary>
        /// 取得指定XML路径下的数据项
        /// </summary>
        /// <param name="xpath">分级对象的路径</param>
        /// <returns></returns>
        public virtual object RetrieveObject(string xpath)
        {
            try
            {
                object cacheObject = RetrieveOriginObject(xpath);

                CacheFlag cf = (CacheFlag)RetrieveOriginObject(xpath + "flag");


                //当标志位中有数据时
                if (cf == CacheFlag.CacheHaveData)
                {
                    string otype = cacheObject.GetType().Name.ToString();

                    //当缓存类型是数据表类型时
                    if (otype.IndexOf("Table") > 0)
                    {
                        System.Data.DataTable dt = cacheObject as DataTable;
                        if ((dt == null) || (dt.Rows.Count == 0))
                        {
                            return null;
                        }
                        else
                        {
                            return cacheObject;
                        }
                    }

                    //当缓存类型是CustomEditorButtonInfo[],string[],UserGroupInfo[],ModeratorInfo[],SmiliesInfo[],ForumInfo[]
                    if (otype.IndexOf("[]") > 0)
                    {
                        if (((Array)cacheObject).Length == 0)
                        {
                            return null;
                        }
                        else
                        {
                            return cacheObject;
                        }
                    }

                     //当缓存类型是CustomEditorButtonInfo[],string[],UserGroupInfo[],ModeratorInfo[],SmiliesInfo[],ForumInfo[]
                    if (otype.IndexOf("Collection") > 0)
                    {
                        if (((CollectionBase)cacheObject).Count == 0)
                        {
                            return null;
                        }
                        else
                        {
                            return cacheObject;
                        }
                    }

                     //当缓存类型是字符串类型时
                    if (otype == "String")
                    {
                        if (cacheObject.ToString() == "")
                        {
                            return null;
                        }
                        else
                        {
                            return cacheObject;
                        }
                    }

                    //当缓存类型是数据行
                    if (otype.IndexOf("Row") >= 0)
                    {
                        if (cacheObject == null)
                        {
                            return null;
                        }
                        else
                        {
                            return cacheObject;
                        }
                    }

                    //当缓存类型是int
                    if (otype.IndexOf("Int") >= 0)
                    {
                        if (((int)cacheObject).Equals(null))
                        {
                            return null;
                        }
                        else
                        {
                            return cacheObject;
                        }
                    }

                    //当缓存类型是SortedList
                    if (otype.IndexOf("SortedList") >= 0)
                    {
#if NET1
						if(((SortedList)cacheObject).Count==0)
#else
                        if (((Discuz.Common.Generic.SortedList<int, object>)cacheObject).Count == 0)
#endif
                        {
                            return null;
                        }
                        else
                        {
                            return cacheObject;
                        }
                    }

                    //当缓存类型是CollectionBase
                    if (otype.IndexOf("CollectionBase") >= 0)
                    {
                        if (((CollectionBase)cacheObject).Count == 0)
                        {
                            return null;
                        }
                        else
                        {
                            return cacheObject;
                        }
                    }
                }

                return cacheObject;
            }
            catch
            {
                return null;
            }
        }


        /// <summary>
        /// 返回一个当前路径下的通过AddObject()方法加入的对象列表
        /// </summary>
        /// <param name="xpath">分级地址</param>
        /// <returns>返回对象数组</returns>
        public virtual object[] RetrieveObjectList(string xpath)
        {
            XmlNode group = objectXmlMap.SelectSingleNode(PrepareXpath(xpath));
            XmlNodeList results = group.SelectNodes(PrepareXpath(xpath) + "/*[@objectId]");
            ArrayList objects = new ArrayList();
            string objectId = null;
            //搜索节点并通过缓存策略链接在对象列表中的对象到存储的对象
            foreach (XmlNode result in results)
            {
                objectId = result.Attributes["objectId"].Value;
                objects.Add(cs.RetrieveObject(objectId));
            }
            //转换 ArrayList 为 object[]
            return (object[])objects.ToArray(typeof(Object));
        }


        /// <summary>
        /// 通过指定的路径删除缓存中的单值部分对象
        /// </summary>
        /// <param name="xpath">分级对象的路径</param>
        public virtual void RemoveSingleObject(string xpath)
        {
            try
            {
                //去掉当前键的子节点部分
                XmlNode result = objectXmlMap.SelectSingleNode(PrepareXpath(xpath + "/Single/"));
                //检查路径是否指向一个组或一个被缓存的实例元素
                if (result.HasChildNodes)
                {
                    //删除所有对象和子结点的信息
                    XmlNodeList objects = result.SelectNodes("*[@objectId]");
                    string objectId = "";
                    foreach (XmlNode node in objects)
                    {
                        objectId = node.Attributes["objectId"].Value;
                        node.ParentNode.RemoveChild(node);
                        //删除对象
                        cs.RemoveObject(objectId);
                    }
                }
                else
                {
                    //删除元素结点和相关的对象
                    string objectId = result.Attributes["objectId"].Value;
                    result.ParentNode.RemoveChild(result);
                    cs.RemoveObject(objectId);
                }
            }
            catch
            {    //如出错误表明当前路径不存在
            }
        }

        /// <summary>
        /// 通过指定的路径删除缓存中的多值部分对象
        /// </summary>
        /// <param name="xpath">分级对象的路径</param>
        public virtual void RemoveMultiObjects(string xpath)
        {
            try
            {
                XmlNode result = objectXmlMap.SelectSingleNode(PrepareXpath(xpath + "/Multi/"));
                //检查路径是否指向一个组或一个被缓存的实例元素
                if (result.HasChildNodes)
                {
                    //删除所有对象和子结点的信息
                    XmlNodeList objects = result.SelectNodes("*[@objectId]");
                    string objectId = "";
                    foreach (XmlNode node in objects)
                    {
                        objectId = node.Attributes["objectId"].Value;
                        node.ParentNode.RemoveChild(node);
                        //删除对象
                        cs.RemoveObject(objectId);
                    }
                }
                else
                {
                    //删除元素结点和相关的对象
                    string objectId = result.Attributes["objectId"].Value;
                    result.ParentNode.RemoveChild(result);
                    cs.RemoveObject(objectId);
                }
            }
            catch
            {    //如出错误表明当前路径不存在
            }
        }


        /// <summary>
        /// 通过指定的路径删除缓存中的对象
        /// </summary>
        /// <param name="xpath">分级对象的路径</param>
        public virtual void RemoveObject(string xpath)
        {
            lock (lockHelper)
            {
                RemoveObject(xpath, true);
            }
        }


        /// <summary>
        /// 通过指定的路径删除缓存中的对象
        /// </summary>
        /// <param name="xpath">分级对象的路径</param>
        /// <param name="writeconfig">是否写入文件</param>
        public virtual void RemoveObject(string xpath, bool writeconfig)
        {
            lock (lockHelper)
            {
                try
                {
                    if (writeconfig)
                    {
                        CachesFileMonitor.UpdateCacheItem(xpath);
                    }

                    XmlNode result = objectXmlMap.SelectSingleNode(PrepareXpath(xpath));
                    //检查路径是否指向一个组或一个被缓存的实例元素
                    if (result.HasChildNodes)
                    {
                        //删除所有对象和子结点的信息
                        XmlNodeList objects = result.SelectNodes("*[@objectId]");
                        string objectId = "";
                        foreach (XmlNode node in objects)
                        {
                            objectId = node.Attributes["objectId"].Value;
                            node.ParentNode.RemoveChild(node);
                            //删除对象
                            cs.RemoveObject(objectId);
                        }
                    }
                    else
                    {
                        //删除元素结点和相关的对象
                        string objectId = result.Attributes["objectId"].Value;
                        result.ParentNode.RemoveChild(result);
                        cs.RemoveObject(objectId);
                    }

                    //检查并移除相应的缓存项

                }
                catch
                {    //如出错误表明当前路径不存在
                }

            }
        }

        /// <summary>
        /// 对象树形分级对象节点
        /// </summary>
        /// <param name="xpath">分级路径 location</param>
        /// <returns></returns>
        private XmlNode CreateNode(string xpath)
        {
            lock (lockHelper)
            {
                string[] xpathArray = xpath.Split('/');
                string root = "";
                XmlNode parentNode = objectXmlMap;
                //建立相关节点
                for (int i = 1; i < xpathArray.Length; i++)
                {
                    XmlNode node = objectXmlMap.SelectSingleNode(root + "/" + xpathArray[i]);
                    // 如果当前路径不存在则建立,否则设置当前路径到它的子路径上
                    if (node == null)
                    {
                        XmlElement newElement = objectXmlMap.OwnerDocument.CreateElement(xpathArray[i]);
                        parentNode.AppendChild(newElement);
                    }
                    //设置低一级的路径
                    root = root + "/" + xpathArray[i];
                    parentNode = objectXmlMap.SelectSingleNode(root);
                }
                return parentNode;
            }
        }

        /// <summary>
        /// 整理 xpath 确保 '/'被删除 is removed
        /// </summary>
        /// <param name="xpath">分级地址</param>
        /// <returns></returns>
        private string PrepareXpath(string xpath)
        {
            lock (lockHelper)
            {
                string[] xpathArray = xpath.Split('/');
                xpath = "/Cache";
                foreach (string s in xpathArray)
                {
                    if (s != "")
                    {
                        xpath = xpath + "/" + s;
                    }
                }
                return xpath;
            }
        }

        /// <summary>
        /// 加载指定的缓存策略
        /// </summary>
        /// <param name="ics"></param>
        public void LoadCacheStrategy(ICacheStrategy ics)
        {
            lock (lockHelper)
            {
                cs = ics;
            }
        }

        /// <summary>
        /// 加载默认的缓存策略
        /// </summary>
        public void LoadDefaultCacheStrategy()
        {
            lock (lockHelper)
            {
                cs = new DefaultCacheStrategy();
            }
        }

    }

    /// <summary>
    /// 缓存文件监视器类
    /// </summary>
    public class CachesFileMonitor
    {
        private static string path = AppDomain.CurrentDomain.BaseDirectory + "cache/cache.config";

        /// <summary>
        /// 程序刚加载时cache.config文件修改时间
        /// </summary>
        private static DateTime cachefileoldchange;

        /// <summary>
        /// 最新general.config文件修改时间
        /// </summary>
        private static DateTime cachefilenewchange;



        private static object cachelockHelper = new object();

        /// <summary>
        /// 静态构造函数
        /// </summary>
        static CachesFileMonitor()
        {
            if (!Utils.FileExists(path))
            {
                string content = "<?xml version=\"1.0\" standalone=\"yes\"?>\r\n";
                content += "<cachetableremove>\r\n<table1 xpath=\"example\" removedatetime=\"2007-1-9 17:36:23\" />\r\n</cachetableremove>";

                using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
                {
                    Byte[] info = System.Text.Encoding.UTF8.GetBytes(content);
                    fs.Write(info, 0, info.Length);
                    fs.Close();
                }
            }
            cachefileoldchange = System.IO.File.GetLastWriteTime(path);
        }

        /// <summary>
        /// 检查和移除缓存
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static DNTCache CheckAndRemoveCache(DNTCache instance)//
        {
            //当程序运行中cache.config发生变化时则对缓存对象做删除的操作
            cachefilenewchange = System.IO.File.GetLastWriteTime(path);
            if (cachefileoldchange != cachefilenewchange)
            {
                lock (cachelockHelper)
                {
                    if (cachefileoldchange != cachefilenewchange)
                    {
                        //当有要清除的项时
                        DataSet dsSrc = new DataSet();
                        //dsSrc.ReadXml(path);

                        dsSrc = GetDataset();

                        foreach (DataRow dr in dsSrc.Tables[0].Rows)
                        {
                            if (dr["xpath"].ToString().Trim() != "")
                            {
                                DateTime removedatetime = DateTime.Now;
                                try
                                {
                                    removedatetime = Convert.ToDateTime(dr["removedatetime"].ToString().Trim());
                                }
                                catch
                                {
                                    ;
                                }

                                if (removedatetime > cachefilenewchange.AddSeconds(-2))
                                {
                                    string xpath = dr["xpath"].ToString().Trim();
                                    instance.RemoveObject(xpath, false);
                                }
                            }
                        }

                        cachefileoldchange = cachefilenewchange;

                        dsSrc.Dispose();
                    }
                }
            }
            return instance;
        }

        /// <summary>
        /// 更新或插入相应的缓存路径
        /// </summary>
        /// <param name="xpath"></param>
        public static void UpdateCacheItem(string xpath)
        {
            DataTable dt = new DataTable("cachetableremove");
            dt.Columns.Add("xpath", System.Type.GetType("System.String"));
            dt.Columns.Add("removedatetime", System.Type.GetType("System.DateTime"));

            //当有要清除的项时
            DataSet dsSrc = new DataSet();
            lock (cachelockHelper)
            {
                //dsSrc.ReadXml(path);

                dsSrc = GetDataset();

                bool nohasone = true;
                int count = dsSrc.Tables[0].Rows.Count;

                foreach (DataRow dr in dsSrc.Tables[0].Rows)
                {
                    if (dr["xpath"].ToString().Trim() == xpath)
                    {
                        dr["removedatetime"] = DateTime.Now.ToString();
                        nohasone = false;
                        break;
                    }
                }

                if (nohasone)
                {
                    DataRow dr = dsSrc.Tables[0].NewRow();
                    dr["xpath"] = xpath;
                    dr["removedatetime"] = DateTime.Now.ToString();
                    dsSrc.Tables[0].Rows.Add(dr);
                }

                //dsSrc.WriteXml(path);

                string content = dsSrc.GetXml();

                DatabaseProvider.GetInstance().ModifyConfigs(content);
                
                dsSrc.Dispose();
            }
        }

        private static DataSet GetDataset()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "cache/cache.config";
            DataSet dsSrc = new DataSet();
            DataTable dt = new DataTable();
            dt.Columns.Add("xpath", System.Type.GetType("System.String"));
            dt.Columns.Add("removedatetime", System.Type.GetType("System.DateTime"));
            string content = DatabaseProvider.GetInstance().GetDntConfigs();
            if (content == "")      //当dnt_Configs表中的cfgCathe字段中 没有数据时 直接读配置文件
            {
                dsSrc.ReadXml(path);
            }
            else                   //当dnt_Configs表中的cfgCathe字段内容不为空时 从数据库中去读取
            {
                XmlDocument xml = new XmlDocument();
                xml.LoadXml(content);

                XmlReaderSettings settings = new XmlReaderSettings();
                settings.ConformanceLevel = ConformanceLevel.Fragment;
                settings.IgnoreWhitespace = true;
                settings.IgnoreComments = true;
                XmlReader reader = XmlReader.Create(path, settings);


                //XmlWriterSettings settingsW = new XmlWriterSettings();
                //XmlWriter writer = XmlWriter.Create(path, settingsW);
                //xml.WriteTo(writer);
                dsSrc.ReadXml(reader);
            }
            return dsSrc;
        }
    }

}
