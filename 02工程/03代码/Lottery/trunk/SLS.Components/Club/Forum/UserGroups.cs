using System;
using System.Data;
using System.Data.Common;

using Discuz.Common;
using Discuz.Data;
using Discuz.Config;
using System.Text;
using Discuz.Entity;

namespace Discuz.Forum
{
    /// <summary>
    /// 用户组操作类
    /// </summary>
    public class UserGroups
    {


        /// <summary>
        /// 获得用户组数据
        /// </summary>
        /// <returns>用户组数据</returns>
        public static UserGroupInfo[] GetUserGroupList()
        {
            Discuz.Cache.DNTCache cache = Discuz.Cache.DNTCache.GetCacheService();
            UserGroupInfo[] infoArray = cache.RetrieveObject("/UserGroupList") as UserGroupInfo[];
            if (infoArray == null)
            {
                DataTable dt = DatabaseProvider.GetInstance().GetUserGroups();
                infoArray = new UserGroupInfo[dt.Rows.Count];
                UserGroupInfo info;
                int Index = 0;
                foreach (DataRow dr in dt.Rows)
                {
                    info = new UserGroupInfo();
                    info.Groupid = Int16.Parse(dr["groupid"].ToString());
                    info.Radminid = Int32.Parse(dr["radminid"].ToString());
                    info.Type = Int16.Parse(dr["type"].ToString());
                    info.System = Int16.Parse(dr["system"].ToString());
                    info.Color = dr["color"].ToString().Trim();
                    info.Grouptitle = dr["grouptitle"].ToString().Trim();
                    info.Creditshigher = Int32.Parse(dr["creditshigher"].ToString());
                    info.Creditslower = Int32.Parse(dr["creditslower"].ToString());
                    info.Stars = Int32.Parse(dr["stars"].ToString());
                    info.Groupavatar = dr["groupavatar"].ToString();
                    info.Readaccess = Int32.Parse(dr["readaccess"].ToString());
                    info.Allowvisit = Int32.Parse(dr["allowvisit"].ToString());
                    info.Allowpost = Int32.Parse(dr["allowpost"].ToString());
                    info.Allowreply = Int32.Parse(dr["allowreply"].ToString());
                    info.Allowpostpoll = Int32.Parse(dr["allowpostpoll"].ToString());
                    info.Allowdirectpost = Int32.Parse(dr["allowdirectpost"].ToString());
                    info.Allowgetattach = Int32.Parse(dr["allowgetattach"].ToString());
                    info.Allowpostattach = Int32.Parse(dr["allowpostattach"].ToString());
                    info.Allowvote = Int32.Parse(dr["allowvote"].ToString());
                    info.Allowmultigroups = Int32.Parse(dr["allowmultigroups"].ToString());
                    info.Allowsearch = Int32.Parse(dr["allowsearch"].ToString());
                    info.Allowavatar = Int32.Parse(dr["allowavatar"].ToString());
                    info.Allowcstatus = Int32.Parse(dr["allowcstatus"].ToString());
                    info.Allowuseblog = Int32.Parse(dr["allowuseblog"].ToString());
                    info.Allowinvisible = Int32.Parse(dr["allowinvisible"].ToString());
                    info.Allowtransfer = Int32.Parse(dr["allowtransfer"].ToString());
                    info.Allowsetreadperm = Int32.Parse(dr["allowsetreadperm"].ToString());
                    info.Allowsetattachperm = Int32.Parse(dr["allowsetattachperm"].ToString());
                    info.Allowhidecode = Int32.Parse(dr["allowhidecode"].ToString());
                    info.Allowhtml = Int32.Parse(dr["allowhtml"].ToString());
                    info.Allowcusbbcode = Int32.Parse(dr["allowcusbbcode"].ToString());
                    info.Allownickname = Int32.Parse(dr["allownickname"].ToString());
                    info.Allowsigbbcode = Int32.Parse(dr["allowsigbbcode"].ToString());
                    info.Allowsigimgcode = Int32.Parse(dr["allowsigimgcode"].ToString());
                    info.Allowviewpro = Int32.Parse(dr["allowviewpro"].ToString());
                    info.Allowviewstats = Int32.Parse(dr["allowviewstats"].ToString());
                    info.Disableperiodctrl = Int32.Parse(dr["disableperiodctrl"].ToString());
                    info.Reasonpm = Int32.Parse(dr["reasonpm"].ToString());
                    info.Maxprice = Int16.Parse(dr["maxprice"].ToString());
                    info.Maxpmnum = Int16.Parse(dr["maxpmnum"].ToString());
                    info.Maxsigsize = Int16.Parse(dr["maxsigsize"].ToString());
                    info.Maxattachsize = Int32.Parse(dr["maxattachsize"].ToString());
                    info.Maxsizeperday = Int32.Parse(dr["maxsizeperday"].ToString());
                    info.Attachextensions = dr["attachextensions"].ToString().Trim();
                    info.Raterange = dr["raterange"].ToString().Trim();
                    info.Allowspace = Int16.Parse(dr["allowspace"].ToString());
                    info.Maxspaceattachsize = Int32.Parse(dr["maxspaceattachsize"].ToString());
                    info.Maxspacephotosize = Int32.Parse(dr["maxspacephotosize"].ToString());
                    info.Allowbonus = Int32.Parse(dr["allowbonus"].ToString());
                    info.Allowdebate = Int32.Parse(dr["allowdebate"].ToString());
                    info.Minbonusprice = Int16.Parse(dr["minbonusprice"].ToString());
                    info.Maxbonusprice = Int16.Parse(dr["maxbonusprice"].ToString());
                    info.Allowtrade = Int32.Parse(dr["allowtrade"].ToString());
                    info.Allowdiggs = Int32.Parse(dr["allowdiggs"].ToString());
                    infoArray[Index] = info;
                    Index++;
                }

                cache.AddObject("/UserGroupList", infoArray);
            }
            return infoArray;
        }


        /// <summary>
        /// 读取指定组的信息
        /// </summary>
        /// <param name="groupid">组id</param>
        /// <returns>组信息</returns>
        public static UserGroupInfo GetUserGroupInfo(int groupid)
        {
            UserGroupInfo[] infoArray = GetUserGroupList();

            // 如果用户组id为7则为游客
            if (groupid == 7)
            {
                return infoArray[6];
            }

            // id为索引
            int id = 0;
            foreach (UserGroupInfo info in infoArray)
            {
                if (info.Groupid == groupid)
                {
                    return infoArray[id];
                }
                id++;
            }
            // 如果查找不到则为游客
            return infoArray[6];

        }



        /// <summary>
        /// 通过组ID得到允许的评分范围,如果无设置则返回空表
        /// </summary>
        /// <param name="groupid">组ID</param>
        /// <returns>评分范围</returns>
        public static DataTable GroupParticipateScore(int groupid)
        {
            DataTable dt = DatabaseProvider.GetInstance().GetUserGroupRateRange(groupid);
            if (dt.Rows.Count > 0)
            {

                if (dt.Rows[0][0].ToString().Trim() == "")
                {
                    //当用户组未设置允许的评分范围时返回空表
                    return (DataTable)null;
                }
                else
                {

                    //创建并初始化表结构
                    DataTable templateDT = new DataTable("templateDT");

                    templateDT.Columns.Add("id", Type.GetType("System.Int32"));
                    //是否参与积分字段
                    templateDT.Columns.Add("available", Type.GetType("System.Boolean"));
                    //积分代号
                    templateDT.Columns.Add("ScoreCode", Type.GetType("System.Int32"));
                    //积分名称
                    templateDT.Columns.Add("ScoreName", Type.GetType("System.String"));
                    //评分最小值
                    templateDT.Columns.Add("Min", Type.GetType("System.String"));
                    //评分最大值
                    templateDT.Columns.Add("Max", Type.GetType("System.String"));
                    //24小时最大评分数
                    templateDT.Columns.Add("MaxInDay", Type.GetType("System.String"));

                    //options HTML代码 
                    templateDT.Columns.Add("Options", Type.GetType("System.String"));
                    //templateDT.Columns["Options"].MaxLength = 2000;

                    //向表中加载默认设置
                    for (int rowcount = 0; rowcount < 8; rowcount++)
                    {
                        DataRow dr = templateDT.NewRow();
                        dr["id"] = rowcount + 1;
                        dr["available"] = false;
                        dr["ScoreCode"] = rowcount + 1;
                        dr["ScoreName"] = "";
                        dr["Min"] = "";
                        dr["Max"] = "";
                        dr["MaxInDay"] = "";
                        templateDT.Rows.Add(dr);
                    }

                    //通过CONFIG文件得到相关的ScoreName名称设置
                    DataRow scoresetname = Scoresets.GetScoreSet().Rows[0];
                    for (int count = 0; count < 8; count++)
                    {
                        if ((scoresetname[count + 2].ToString().Trim() != "") && (scoresetname[count + 2].ToString().Trim() != "0"))
                        {
                            templateDT.Rows[count]["ScoreName"] = scoresetname[count + 2].ToString().Trim();
                        }
                    }

                    //用数据库中的记录更新已装入的默认数据
                    int i = 0;
                    foreach (string raterangestr in dt.Rows[0][0].ToString().Trim().Split('|'))
                    {
                        if (raterangestr.Trim() != "")
                        {
                            string[] scoredata = raterangestr.Split(',');
                            //判断是否参与积分字段的数据判断
                            if (scoredata[1].Trim() == "True")
                            {
                                templateDT.Rows[i]["available"] = true;
                            }
                            //更新其它字段
                            templateDT.Rows[i]["Min"] = scoredata[4].Trim();
                            templateDT.Rows[i]["Max"] = scoredata[5].Trim();
                            templateDT.Rows[i]["MaxInDay"] = scoredata[6].Trim();
                        }
                        i++;
                    }
                    return templateDT;
                }
            }

            //当用户组不存在时返回空
            return null;

        }


        /// <summary>
        /// 通过组ID和UID得到允许的评分范围,如果无设置则返回空表
        /// </summary>
        /// <param name="uid">用户ID</param>
        /// <param name="gid">用户组别</param>
        /// <returns>ID和UID允许的评分范围</returns>
        public static DataTable GroupParticipateScore(int uid, int gid)
        {
            DataTable dt = GroupParticipateScore(gid);
            int[] extcredits = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            StringBuilder sb = new StringBuilder();
            int offset = 1;
            if (dt != null)
            {
                IDataReader reader = DatabaseProvider.GetInstance().GetUserTodayRate(uid);
                while (reader.Read())
                {
                    extcredits[Utils.StrToInt(reader["extcredits"], 0)] = Utils.StrToInt(reader["todayrate"], 0);
                }
                reader.Close();

                DataRow dr = null;
                for (int r = dt.Rows.Count - 1; r >= 0; r--)
                {
                    dr = dt.Rows[r];
                    if (!Convert.ToBoolean(dr["available"]))
                    {
                        dr.Delete();
                        continue;
                    }
                    dr["MaxInDay"] = Utils.StrToInt(dr["MaxInDay"], 0) - extcredits[Utils.StrToInt(dr["ScoreCode"], 0)];
                    if (Utils.StrToInt(dr["MaxInDay"], 0) <= 0)
                    {
                        dr.Delete();
                        continue;
                    }

                    offset = Convert.ToInt32(Math.Abs(Math.Ceiling((Utils.StrToInt(dr["Max"], 0) - Utils.StrToInt(dr["Min"], 0)) / 32.0)));

                    sb.Remove(0, sb.Length);
                    for (int i = Utils.StrToInt(dr["Min"], 0); i <= Utils.StrToFloat(dr["Max"], 0); i += offset)
                    {
                        if (Math.Abs(i) <= Utils.StrToInt(dr["MaxInDay"], 0))
                        {
                            sb.Append("\n<option value=\"");
                            sb.Append(i);
                            sb.Append("\">");
                            sb.Append(i > 0 ? "+" : "");
                            sb.Append(i);
                            sb.Append("</option>");
                        }
                    }
                    dr["Options"] = sb.ToString();
                }
                dt.AcceptChanges();
            }
            if (dt == null)
            {
                return new DataTable();
            }
            return dt;

        }

    }
}
