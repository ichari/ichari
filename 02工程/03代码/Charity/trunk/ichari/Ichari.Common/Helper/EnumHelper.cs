using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.Reflection;
using System.ComponentModel;


namespace Ichari.Common.Helper
{
    public static class EnumHelper
    {
        /// <summary>
        /// 把enum所有属性放到IEnumerable SelectListItem里
        /// </summary>
        /// <typeparam name="T">枚举的类型</typeparam>
        /// <param name="selectedValue">SelectList的选中项</param>
        /// <param name="isIndex">是否加入“请选择”到第一项的位置</param>
        /// <returns></returns>
        public static IEnumerable<SelectListItem> EnumToSelectList<T>(int? selectedValue, bool isIndex)
        {
            if (selectedValue == null)
            {
                return new SelectList(EnumDescriptions(typeof(T), isIndex), "Key", "Value");
            }
            else
            {
                return new SelectList(EnumDescriptions(typeof(T), isIndex), "Key", "Value", selectedValue.Value);
            }


        }
        /// <summary>
        /// 根据bool型获取是否要"请选择" 枚举列表   true是需要
        /// </summary>
        /// <param name="tEnum"></param>
        /// <param name="isIndex"></param>
        /// <returns></returns>
        private static Dictionary<string, string> EnumDescriptions(System.Type tEnum, bool isIndex)
        {
            Array values = System.Enum.GetValues(tEnum);
            Dictionary<string, string> list = new Dictionary<string, string>();
            if (isIndex)
            {
                list.Add("", "请选择");
            }
            foreach (var i in values)
            {
                string name = System.Enum.GetName(tEnum, i);
                string descr = string.Empty;

                FieldInfo fieldInfo = tEnum.GetField(name);
                DescriptionAttribute[] attributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);

                DisplayAttribute[] attrlist = (DisplayAttribute[])fieldInfo.GetCustomAttributes(typeof(DisplayAttribute), false);
                if (attributes != null && attributes.Length > 0)
                {
                    descr = attributes[0].Description;
                }
                else if (attrlist != null && attrlist.Length > 0)
                {
                    //descr = Resource.Lang.ResourceManager.GetString(attrlist[0].Name, Resource.Lang.Culture);
                    descr = attrlist[0].Name;
                }
                else
                {
                    descr = i.ToString();
                }

                list.Add(i.GetHashCode().ToString(), descr);
            }
            return list;
        }

        /// <summary>
        /// 根据bool型获取是否要"请选择" 枚举列表   true是需要
        /// </summary>
        /// <param name="tEnum"></param>
        /// <param name="isIndex"></param>
        /// <returns></returns>
        public static List<int> EnumValues(System.Type tEnum)
        {
            Array values = System.Enum.GetValues(tEnum);
            List<int> list = new List<int>();

            foreach (var i in values)
            {
                list.Add(i.GetHashCode());
            }
            return list;
        }

        /// <summary>
        /// 传入枚举类型，以及相应的value 获取description。
        /// </summary>
        /// <param name="tEnum"></param>
        /// <param name="isIndex"></param>
        /// <returns></returns>
        public static string GetEnumDisplayName<T>(int key)
        {
            string Description = string.Empty;
            Dictionary<string, string> dEnumList = EnumDescriptions(typeof(T), false);
            foreach (var i in dEnumList)
            {
                if (key.ToString() == i.Key)
                {
                    Description = i.Value;
                }
            }
            return Description;
        }
        /// <summary>
        /// 传入枚举类型，以及相应的value 获取description。
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static int? GetEnumKey<T>(string name)
        {
            if (string.IsNullOrEmpty(name))
                return null;
            Dictionary<string, string> dEnumList = EnumDescriptions(typeof(T), false);
            foreach (var i in dEnumList)
            {
                if (name.Trim().ToLower() == i.Value.ToLower())
                {
                    return int.Parse(i.Key);
                }
            }
            return null;
        }
    }
}
