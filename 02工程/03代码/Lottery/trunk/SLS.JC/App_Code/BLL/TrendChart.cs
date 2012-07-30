using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Collections;

/// <summary>
///BBL1 的摘要说明
/// </summary>
public class TrendChart
{
    public TrendChart()
    {
        //
        //TODO: 在此处添加构造函数逻辑
        //
    }

    #region// 双色球综合分布

    public DataTable SSQ_ZHFB_Select(int num, string str1, string str2, ref string result)
    {
        DataTable dt = null;
        DataTable dt1 = null;

        dt = Shove._Web.Cache.GetCacheAsDataTable("SSQ_ZHFB_Select");

        if (dt == null || dt.Rows.Count < 1)
        {
            DataSet ds = null;

            DAL.Procedures.P_TrendChart_SSQ_HMFB(ref ds);

            if (ds == null || ds.Tables.Count < 1)
            {
                PF.GoError(ErrorNumber.Unknow, "参数错误或者数据库--无数据", this.GetType().BaseType.FullName);

                return dt1;
            }
            else
            {
                Shove._Web.Cache.SetCache("SSQ_ZHFB_Select", ds.Tables[0]);

                dt = ds.Tables[0];
            }
        }

        dt1 = dt.Clone();

        //通过期号来查询
        if (num == 0)
        {
            int i = Shove._Convert.StrToInt(str1, 0);
            int j = Shove._Convert.StrToInt(str2, 0);

            DataRow[] dr2 = dt.Select("Isuse>=" + i + " and  Isuse<=" + j);

            foreach (DataRow r in dr2)
            {
                dt1.Rows.Add(r.ItemArray);
            }
        }

        //通过按纽选择查询多少期
        else
        {
            if (num >= dt.Rows.Count)
            {
                num = dt.Rows.Count;
            }

            int ii = dt.Rows.Count;                           //总记录数
            int jj = ii - num;

            DataRow[] drr = dt.Select("id > " + jj + " and id <= " + ii);

            foreach (DataRow rr in drr)
            {
                dt1.Rows.Add(rr.ItemArray);
            }
        }

        return dt1;
    }

    public DataTable SSQ_ZH_Select(int num, string str1, string str2, ref string result)
    {
        DataTable dt = null;
        DataTable dt1 = null;

        dt = Shove._Web.Cache.GetCacheAsDataTable("SSQ_ZH_Select");

        if (dt == null || dt.Rows.Count < 1)
        {
            DataSet ds = null;
            DAL.Procedures.P_TrendChart_SSQ_ZH(ref ds);

            if (ds == null || ds.Tables.Count < 1)
            {
                PF.GoError(ErrorNumber.Unknow, "参数错误或者数据库--无数据", this.GetType().BaseType.FullName);

                return dt1;
            }
            else
            {
                Shove._Web.Cache.SetCache("SSQ_ZH_Select", ds.Tables[0]);

                dt = ds.Tables[0];
            }
        }

        dt1 = dt.Clone();

        //通过期号来查询
        if (num == 0)
        {
            int i = Shove._Convert.StrToInt(str1, 0);
            int j = Shove._Convert.StrToInt(str2, 0);

            DataRow[] dr2 = dt.Select("Isuse>=" + i + " and  Isuse<=" + j);

            foreach (DataRow r in dr2)
            {
                dt1.Rows.Add(r.ItemArray);
            }
        }

        //通过按纽选择查询多少期
        else
        {
            if (num >= dt.Rows.Count)
            {
                num = dt.Rows.Count;
            }

            int ii = dt.Rows.Count;                           //总记录数
            int jj = ii - num;

            DataRow[] drr = dt.Select("id > " + jj + " and id <= " + ii);

            foreach (DataRow rr in drr)
            {
                dt1.Rows.Add(rr.ItemArray);
            }
        }

        return dt1;
    }

    public DataTable SSQ_DX_Select(int num, string str1, string str2, ref string result)
    {
        DataTable dt = null;
        DataTable dt1 = null;

        dt = Shove._Web.Cache.GetCacheAsDataTable("SSQ_DX_Select");

        if (dt == null || dt.Rows.Count < 1)
        {
            DataSet ds = null;
            DAL.Procedures.P_TrendChart_SSQ_DX(ref ds);

            if (ds == null || ds.Tables.Count < 1)
            {
                PF.GoError(ErrorNumber.Unknow, "参数错误或者数据库--无数据", this.GetType().BaseType.FullName);

                return dt1;
            }
            else
            {
                Shove._Web.Cache.SetCache("SSQ_DX_Select", ds.Tables[0]);
                dt = ds.Tables[0];
            }
        }

        dt1 = dt.Clone();

        //通过期号来查询
        if (num == 0)
        {
            int i = Shove._Convert.StrToInt(str1, 0);
            int j = Shove._Convert.StrToInt(str2, 0);

            DataRow[] dr2 = dt.Select("Isuse>=" + i + " and  Isuse<=" + j);

            foreach (DataRow r in dr2)
            {
                dt1.Rows.Add(r.ItemArray);
            }
        }

        //通过按纽选择查询多少期
        else
        {
            if (num >= dt.Rows.Count)
            {
                num = dt.Rows.Count;
            }

            int ii = dt.Rows.Count;                           //总记录数
            int jj = ii - num;

            DataRow[] drr = dt.Select("id > " + jj + " and id <= " + ii);

            foreach (DataRow rr in drr)
            {
                dt1.Rows.Add(rr.ItemArray);
            }
        }

        return dt1;
    }

    public DataTable SSQ_JO_Select(int num, string str1, string str2, ref string result)
    {
        DataTable dt = null;
        DataTable dt1 = null;

        dt = Shove._Web.Cache.GetCacheAsDataTable("SSQ_JO_Select");

        if (dt == null || dt.Rows.Count < 1)
        {
            DataSet ds = null;
            DAL.Procedures.P_TrendChart_SSQ_JO(ref ds);

            if (ds == null || ds.Tables.Count < 1)
            {
                PF.GoError(ErrorNumber.Unknow, "参数错误或者数据库--无数据", this.GetType().BaseType.FullName);

                return dt1;
            }
            else
            {
                Shove._Web.Cache.SetCache("SSQ_JO_Select", ds.Tables[0]);
                dt = ds.Tables[0];
            }
        }

        dt1 = dt.Clone();

        //通过期号来查询
        if (num == 0)
        {
            int i = Shove._Convert.StrToInt(str1, 0);
            int j = Shove._Convert.StrToInt(str2, 0);

            DataRow[] dr2 = dt.Select("Isuse>=" + i + " and  Isuse<=" + j);

            foreach (DataRow r in dr2)
            {
                dt1.Rows.Add(r.ItemArray);
            }
        }

        //通过按纽选择查询多少期
        else
        {
            if (num >= dt.Rows.Count)
            {
                num = dt.Rows.Count;
            }

            int ii = dt.Rows.Count;                           //总记录数
            int jj = ii - num;

            DataRow[] drr = dt.Select("id > " + jj + " and id <= " + ii);

            foreach (DataRow rr in drr)
            {
                dt1.Rows.Add(rr.ItemArray);
            }
        }

        return dt1;
    }

    public DataTable SSQ_HL_Select(int num, string str1, string str2, ref string result)
    {
        DataTable dt = null;
        DataTable dt1 = null;

        dt = Shove._Web.Cache.GetCacheAsDataTable("SSQ_HL_Select");

        if (dt == null || dt.Rows.Count < 1)
        {
            DataSet ds = null;
            DAL.Procedures.P_TrendChart_SSQ_HL(ref ds);

            if (ds == null || ds.Tables.Count < 1)
            {
                PF.GoError(ErrorNumber.Unknow, "参数错误或者数据库--无数据", this.GetType().BaseType.FullName);

                return dt1;
            }
            else
            {
                Shove._Web.Cache.SetCache("SSQ_HL_Select", ds.Tables[0]);
                dt = ds.Tables[0];
            }
        }

        dt1 = dt.Clone();

        //通过期号来查询
        if (num == 0)
        {
            int i = Shove._Convert.StrToInt(str1, 0);
            int j = Shove._Convert.StrToInt(str2, 0);

            DataRow[] dr2 = dt.Select("Isuse>=" + i + " and  Isuse<=" + j);

            foreach (DataRow r in dr2)
            {
                dt1.Rows.Add(r.ItemArray);
            }
        }

        //通过按纽选择查询多少期
        else
        {
            if (num >= dt.Rows.Count)
            {
                num = dt.Rows.Count;
            }

            int ii = dt.Rows.Count;                           //总记录数
            int jj = ii - num;

            DataRow[] drr = dt.Select("id > " + jj + " and id <= " + ii);

            foreach (DataRow rr in drr)
            {
                dt1.Rows.Add(rr.ItemArray);
            }
        }

        return dt1;
    }

    //常规项目表
    public DataTable SSQ_CGXMB_Select(int num, string str1, string str2, ref string result)
    {
        DataTable dt = null;
        DataTable dt1 = null;

        dt = Shove._Web.Cache.GetCacheAsDataTable("SSQ_CGXMB_Select");

        if (dt == null || dt.Rows.Count < 1)
        {
            DataSet ds = null;
            DAL.Procedures.P_TrendChart_SSQ_CGXMB(ref ds);

            if (ds == null || ds.Tables.Count < 1)
            {
                PF.GoError(ErrorNumber.Unknow, "参数错误或者数据库--无数据", this.GetType().BaseType.FullName);

                return dt1;
            }
            else
            {
                Shove._Web.Cache.SetCache("SSQ_CGXMB_Select", ds.Tables[0]);

                dt = ds.Tables[0];
            }
        }

        dt1 = dt.Clone();

        //通过期号来查询
        if (num == 0)
        {
            int i = Shove._Convert.StrToInt(str1, 0);
            int j = Shove._Convert.StrToInt(str2, 0);

            DataRow[] dr2 = dt.Select("Isuse>=" + i + " and  Isuse<=" + j);

            foreach (DataRow r in dr2)
            {
                dt1.Rows.Add(r.ItemArray);
            }
        }

        //通过按纽选择查询多少期
        else
        {
            if (num >= dt.Rows.Count)
            {
                num = dt.Rows.Count;
            }

            int ii = dt.Rows.Count;                           //总记录数
            int jj = ii - num;

            DataRow[] drr = dt.Select("id > " + jj + " and id <= " + ii);

            foreach (DataRow rr in drr)
            {
                dt1.Rows.Add(rr.ItemArray);
            }
        }

        return dt1;
    }

    //篮球综合走试图
    public DataTable SSQ_BQZH_Select(int num, string str1, string str2, ref string result)
    {
        DataTable dt = null;
        DataTable dt1 = null;

        dt = Shove._Web.Cache.GetCacheAsDataTable("SSQ_BQZH_Select");

        if (dt == null || dt.Rows.Count < 1)
        {
            DataSet ds = null;
            DAL.Procedures.P_TrendChart_SSQ_BQZH(ref ds);

            if (ds == null || ds.Tables.Count < 1)
            {
                PF.GoError(ErrorNumber.Unknow, "参数错误或者数据库--无数据", this.GetType().BaseType.FullName);

                return dt1;
            }
            else
            {
                Shove._Web.Cache.SetCache("SSQ_BQZH_Select", ds.Tables[0]);

                dt = ds.Tables[0];
            }
        }

        dt1 = dt.Clone();

        //通过期号来查询
        if (num == 0)
        {
            int i = Shove._Convert.StrToInt(str1, 0);
            int j = Shove._Convert.StrToInt(str2, 0);

            DataRow[] dr2 = dt.Select("Isuse>=" + i + " and  Isuse<=" + j);

            foreach (DataRow r in dr2)
            {
                dt1.Rows.Add(r.ItemArray);
            }
        }

        //通过按纽选择查询多少期
        else
        {
            if (num >= dt.Rows.Count)
            {
                num = dt.Rows.Count;
            }

            int ii = dt.Rows.Count;                           //总记录数
            int jj = ii - num;

            DataRow[] drr = dt.Select("id > " + jj + " and id <= " + ii);

            foreach (DataRow rr in drr)
            {
                dt1.Rows.Add(rr.ItemArray);
            }
        }

        return dt1;
    }
    //3区分布图
    public DataTable SSQ_3QFB_Select(int num, string str1, string str2, ref string result)
    {
        DataTable dt = null;
        DataTable dt1 = null;

        dt = Shove._Web.Cache.GetCacheAsDataTable("SSQ_3QFB_Select");

        if (dt == null || dt.Rows.Count < 1)
        {
            DataSet ds = null;
            DAL.Procedures.P_TrendChart_SSQ_3FQ(ref ds);

            if (ds == null || ds.Tables.Count < 1)
            {
                PF.GoError(ErrorNumber.Unknow, "参数错误或者数据库--无数据", this.GetType().BaseType.FullName);

                return dt1;
            }
            else
            {
                Shove._Web.Cache.SetCache("SSQ_3QFB_Select", ds.Tables[0]);

                dt = ds.Tables[0];
            }
        }

        dt1 = dt.Clone();

        //通过期号来查询
        if (num == 0)
        {
            int i = Shove._Convert.StrToInt(str1, 0);
            int j = Shove._Convert.StrToInt(str2, 0);

            DataRow[] dr2 = dt.Select("Isuse>=" + i + " and  Isuse<=" + j);

            foreach (DataRow r in dr2)
            {
                dt1.Rows.Add(r.ItemArray);
            }
        }

        //通过按纽选择查询多少期
        else
        {
            if (num >= dt.Rows.Count)
            {
                num = dt.Rows.Count;
            }

            int ii = dt.Rows.Count;                           //总记录数
            int jj = ii - num;

            DataRow[] drr = dt.Select("id > " + jj + " and id <= " + ii);

            foreach (DataRow rr in drr)
            {
                dt1.Rows.Add(rr.ItemArray);
            }
        }

        return dt1;
    }

    #endregion

    #region 福彩3D

    //综合分布
    public DataTable FC3D_ZHFB_Select(int num, string str1, string str2, ref string result)
    {
        DataTable dt = null;
        DataTable dt1 = null;

        dt = Shove._Web.Cache.GetCacheAsDataTable("FC3D_ZHFB_Select");

        if (dt == null || dt.Rows.Count < 1)
        {
            DataSet ds = null;
            DAL.Procedures.P_TrendChart_3D_ZHFB(ref ds);

            if (ds == null || ds.Tables.Count < 1)
            {
                PF.GoError(ErrorNumber.Unknow, "参数错误或者数据库--无数据", this.GetType().BaseType.FullName);

                return dt1;
            }
            else
            {
                Shove._Web.Cache.SetCache("FC3D_ZHFB_Select", ds.Tables[0]);

                dt = ds.Tables[0];
            }
        }

        dt1 = dt.Clone();

        //通过期号来查询
        if (num == 0)
        {
            int i = Shove._Convert.StrToInt(str1, 0);
            int j = Shove._Convert.StrToInt(str2, 0);

            DataRow[] dr2 = dt.Select("Isuse>=" + i + " and  Isuse<=" + j);

            foreach (DataRow r in dr2)
            {
                dt1.Rows.Add(r.ItemArray);
            }
        }

        //通过按纽选择查询多少期
        else
        {
            if (num >= dt.Rows.Count)
            {
                num = dt.Rows.Count;
            }

            int ii = dt.Rows.Count;                           //总记录数
            int jj = ii - num;

            DataRow[] drr = dt.Select("id > " + jj + " and id <= " + ii);

            foreach (DataRow rr in drr)
            {
                dt1.Rows.Add(rr.ItemArray);
            }
        }

        return dt1;
    }

    //质合形态
    public DataTable FC3D_ZHXT_Select(int num, string str1, string str2, ref string result)
    {
        DataTable dt = null;
        DataTable dt1 = null;

        dt = Shove._Web.Cache.GetCacheAsDataTable("FC3D_ZHXT_Select");

        if (dt == null || dt.Rows.Count < 1)
        {
            DataSet ds = null;
            DAL.Procedures.P_TrendChart_3D_ZHXT(ref ds);

            if (ds == null || ds.Tables.Count < 1)
            {
                PF.GoError(ErrorNumber.Unknow, "参数错误或者数据库--无数据", this.GetType().BaseType.FullName);

                return dt1;
            }
            else
            {
                Shove._Web.Cache.SetCache("FC3D_ZHXT_Select", ds.Tables[0]);

                dt = ds.Tables[0];
            }
        }

        dt1 = dt.Clone();

        //通过期号来查询
        if (num == 0)
        {
            int i = Shove._Convert.StrToInt(str1, 0);
            int j = Shove._Convert.StrToInt(str2, 0);

            DataRow[] dr2 = dt.Select("Isuse>=" + i + " and  Isuse<=" + j);

            foreach (DataRow r in dr2)
            {
                dt1.Rows.Add(r.ItemArray);
            }
        }

        //通过按纽选择查询多少期
        else
        {
            if (num >= dt.Rows.Count)
            {
                num = dt.Rows.Count;
            }

            int ii = dt.Rows.Count;                           //总记录数
            int jj = ii - num;

            DataRow[] drr = dt.Select("id > " + jj + " and id <= " + ii);

            foreach (DataRow rr in drr)
            {
                dt1.Rows.Add(rr.ItemArray);
            }
        }

        return dt1;
    }

    //跨度形态
    public DataTable FC3D_KD_Select(int num, string str1, string str2, ref string result)
    {
        DataTable dt = null;
        DataTable dt1 = null;

        dt = Shove._Web.Cache.GetCacheAsDataTable("FC3D_KD_Select");

        if (dt == null || dt.Rows.Count < 1)
        {
            DataSet ds = null;
            DAL.Procedures.P_TrendChart_3D_KD(ref ds);

            if (ds == null || ds.Tables.Count < 1)
            {
                PF.GoError(ErrorNumber.Unknow, "参数错误或者数据库--无数据", this.GetType().BaseType.FullName);

                return dt1;
            }
            else
            {
                Shove._Web.Cache.SetCache("FC3D_KD_Select", ds.Tables[0]);

                dt = ds.Tables[0];
            }
        }

        dt1 = dt.Clone();

        //通过期号来查询
        if (num == 0)
        {
            int i = Shove._Convert.StrToInt(str1, 0);
            int j = Shove._Convert.StrToInt(str2, 0);

            DataRow[] dr2 = dt.Select("Isuse>=" + i + " and  Isuse<=" + j);

            foreach (DataRow r in dr2)
            {
                dt1.Rows.Add(r.ItemArray);
            }
        }

        //通过按纽选择查询多少期
        else
        {
            if (num >= dt.Rows.Count)
            {
                num = dt.Rows.Count;
            }

            int ii = dt.Rows.Count;                           //总记录数
            int jj = ii - num;

            DataRow[] drr = dt.Select("id > " + jj + " and id <= " + ii);

            foreach (DataRow rr in drr)
            {
                dt1.Rows.Add(rr.ItemArray);
            }
        }

        return dt1;
    }

    //和值形态
    public DataTable FC3D_HZ_Select(int num, string str1, string str2, ref string result)
    {
        DataTable dt = null;
        DataTable dt1 = null;

        dt = Shove._Web.Cache.GetCacheAsDataTable("FC3D_HZ_Select");

        if (dt == null || dt.Rows.Count < 1)
        {
            DataSet ds = null;
            DAL.Procedures.P_TrendChart_3D_HZ(ref ds);

            if (ds == null || ds.Tables.Count < 1)
            {
                PF.GoError(ErrorNumber.Unknow, "参数错误或者数据库--无数据", this.GetType().BaseType.FullName);

                return dt1;
            }
            else
            {
                Shove._Web.Cache.SetCache("FC3D_HZ_Select", ds.Tables[0]);

                dt = ds.Tables[0];
            }
        }

        dt1 = dt.Clone();

        //通过期号来查询
        if (num == 0)
        {
            int i = Shove._Convert.StrToInt(str1, 0);
            int j = Shove._Convert.StrToInt(str2, 0);

            DataRow[] dr2 = dt.Select("Isuse>=" + i + " and  Isuse<=" + j);

            foreach (DataRow r in dr2)
            {
                dt1.Rows.Add(r.ItemArray);
            }
        }

        //通过按纽选择查询多少期
        else
        {
            if (num >= dt.Rows.Count)
            {
                num = dt.Rows.Count;
            }

            int ii = dt.Rows.Count;                           //总记录数
            int jj = ii - num;

            DataRow[] drr = dt.Select("id > " + jj + " and id <= " + ii);

            foreach (DataRow rr in drr)
            {
                dt1.Rows.Add(rr.ItemArray);
            }
        }

        return dt1;
    }

    //大中小形态
    public DataTable FC3D_DZX_Select(int num, string str1, string str2, ref string result)
    {
        DataTable dt = null;
        DataTable dt1 = null;

        dt = Shove._Web.Cache.GetCacheAsDataTable("FC3D_DZX_Select");

        if (dt == null || dt.Rows.Count < 1)
        {
            DataSet ds = null;
            DAL.Procedures.P_TrendChart_3D_DZX(ref ds);

            if (ds == null || ds.Tables.Count < 1)
            {
                PF.GoError(ErrorNumber.Unknow, "参数错误或者数据库--无数据", this.GetType().BaseType.FullName);

                return dt1;
            }
            else
            {
                Shove._Web.Cache.SetCache("FC3D_DZX_Select", ds.Tables[0]);

                dt = ds.Tables[0];
            }
        }

        dt1 = dt.Clone();

        //通过期号来查询
        if (num == 0)
        {
            int i = Shove._Convert.StrToInt(str1, 0);
            int j = Shove._Convert.StrToInt(str2, 0);

            DataRow[] dr2 = dt.Select("Isuse>=" + i + " and  Isuse<=" + j);

            foreach (DataRow r in dr2)
            {
                dt1.Rows.Add(r.ItemArray);
            }
        }

        //通过按纽选择查询多少期
        else
        {
            if (num >= dt.Rows.Count)
            {
                num = dt.Rows.Count;
            }

            int ii = dt.Rows.Count;                           //总记录数
            int jj = ii - num;

            DataRow[] drr = dt.Select("id > " + jj + " and id <= " + ii);

            foreach (DataRow rr in drr)
            {
                dt1.Rows.Add(rr.ItemArray);
            }
        }

        return dt1;
    }

    //除3余数
    public DataTable FC3D_C3YS_Select(int num, string str1, string str2, ref string result)
    {
        DataTable dt = null;
        DataTable dt1 = null;

        dt = Shove._Web.Cache.GetCacheAsDataTable("FC3D_C3YS_Select");

        if (dt == null || dt.Rows.Count < 1)
        {
            DataSet ds = null;
            DAL.Procedures.P_TrendChart_3D_C3YS(ref ds);

            if (ds == null || ds.Tables.Count < 1)
            {
                PF.GoError(ErrorNumber.Unknow, "参数错误或者数据库--无数据", this.GetType().BaseType.FullName);

                return dt1;
            }
            else
            {
                Shove._Web.Cache.SetCache("FC3D_C3YS_Select", ds.Tables[0]);
                dt = ds.Tables[0];
            }
        }

        dt1 = dt.Clone();

        //通过期号来查询
        if (num == 0)
        {
            int i = Shove._Convert.StrToInt(str1, 0);
            int j = Shove._Convert.StrToInt(str2, 0);

            DataRow[] dr2 = dt.Select("Isuse>=" + i + " and  Isuse<=" + j);

            foreach (DataRow r in dr2)
            {
                dt1.Rows.Add(r.ItemArray);
            }
        }

        //通过按纽选择查询多少期
        else
        {
            if (num >= dt.Rows.Count)
            {
                num = dt.Rows.Count;
            }

            int ii = dt.Rows.Count;                           //总记录数
            int jj = ii - num;

            DataRow[] drr = dt.Select("id > " + jj + " and id <= " + ii);

            foreach (DataRow rr in drr)
            {
                dt1.Rows.Add(rr.ItemArray);
            }
        }

        return dt1;
    }

    //形态走势图
    public DataTable FC3D_XTZST_Select(int num, string str1, string str2, ref string result)
    {
        DataTable dt = null;
        DataTable dt1 = null;

        dt = Shove._Web.Cache.GetCacheAsDataTable("FC3D_XTZST_Select");

        if (dt == null || dt.Rows.Count < 1)
        {
            DataSet ds = null;
            DAL.Procedures.P_TrendChart_3D_XTZST(ref ds);

            if (ds == null || ds.Tables.Count < 1)
            {
                PF.GoError(ErrorNumber.Unknow, "参数错误或者数据库--无数据", this.GetType().BaseType.FullName);

                return dt1;
            }
            else
            {
                Shove._Web.Cache.SetCache("FC3D_XTZST_Select", ds.Tables[0]);

                dt = ds.Tables[0];
            }
        }

        dt1 = dt.Clone();

        //通过期号来查询
        if (num == 0)
        {
            int i = Shove._Convert.StrToInt(str1, 0);
            int j = Shove._Convert.StrToInt(str2, 0);

            DataRow[] dr2 = dt.Select("Isuse>=" + i + " and  Isuse<=" + j);

            foreach (DataRow r in dr2)
            {
                dt1.Rows.Add(r.ItemArray);
            }
        }

        //通过按纽选择查询多少期
        else
        {
            if (num >= dt.Rows.Count)
            {
                num = dt.Rows.Count;
            }

            int ii = dt.Rows.Count;                           //总记录数
            int jj = ii - num;

            DataRow[] drr = dt.Select("id > " + jj + " and id <= " + ii);

            foreach (DataRow rr in drr)
            {
                dt1.Rows.Add(rr.ItemArray);
            }
        }

        return dt1;
    }

    #endregion

    #region 7乐彩
    //7乐彩常规项目表
    public DataTable LC7_CGXMB_Select(int num, string str1, string str2, ref string result)
    {
        DataTable dt = null;
        DataTable dt1 = null;

        dt = Shove._Web.Cache.GetCacheAsDataTable("LC7_CGXMB_Select");

        if (dt == null || dt.Rows.Count < 1)
        {
            DataSet ds = null;
            DAL.Procedures.P_TrendChart_7LC_CGXMB(ref ds);

            if (ds == null || ds.Tables.Count < 1)
            {
                PF.GoError(ErrorNumber.Unknow, "参数错误或者数据库--无数据", this.GetType().BaseType.FullName);

                return dt1;
            }
            else
            {
                Shove._Web.Cache.SetCache("LC7_CGXMB_Select", ds.Tables[0]);

                dt = ds.Tables[0];
            }
        }

        dt1 = dt.Clone();

        //通过期号来查询
        if (num == 0)
        {
            int i = Shove._Convert.StrToInt(str1, 0);
            int j = Shove._Convert.StrToInt(str2, 0);

            DataRow[] dr2 = dt.Select("Isuse>=" + i + " and  Isuse<=" + j);

            foreach (DataRow r in dr2)
            {
                dt1.Rows.Add(r.ItemArray);
            }
        }

        //通过按纽选择查询多少期
        else
        {
            if (num >= dt.Rows.Count)
            {
                num = dt.Rows.Count;
            }

            int ii = dt.Rows.Count;                           //总记录数
            int jj = ii - num;

            DataRow[] drr = dt.Select("id > " + jj + " and id <= " + ii);

            foreach (DataRow rr in drr)
            {
                dt1.Rows.Add(rr.ItemArray);
            }
        }

        return dt1;
    }

    public DataTable LC7_ZHFB_Select(int num, string str1, string str2, ref string result)
    {
        DataTable dt = null;
        DataTable dt1 = null;

        dt = Shove._Web.Cache.GetCacheAsDataTable("LC7_ZHFB_Select");

        if (dt == null || dt.Rows.Count < 1)
        {
            DataSet ds = null;
            DAL.Procedures.P_TrendChart_7CL_HMFB(ref ds);

            if (ds == null || ds.Tables.Count < 1)
            {
                PF.GoError(ErrorNumber.Unknow, "参数错误或者数据库--无数据", this.GetType().BaseType.FullName);

                return dt1;
            }
            else
            {
                Shove._Web.Cache.SetCache("LC7_ZHFB_Select", ds.Tables[0]);

                dt = ds.Tables[0];
            }
        }

        dt1 = dt.Clone();

        //通过期号来查询
        if (num == 0)
        {
            int i = Shove._Convert.StrToInt(str1, 0);
            int j = Shove._Convert.StrToInt(str2, 0);

            DataRow[] dr2 = dt.Select("Isuse>=" + i + " and  Isuse<=" + j);

            foreach (DataRow r in dr2)
            {
                dt1.Rows.Add(r.ItemArray);
            }
        }

        //通过按纽选择查询多少期
        else
        {
            if (num >= dt.Rows.Count)
            {
                num = dt.Rows.Count;
            }

            int ii = dt.Rows.Count;                           //总记录数
            int jj = ii - num;

            DataRow[] drr = dt.Select("id > " + jj + " and id <= " + ii);

            foreach (DataRow rr in drr)
            {
                dt1.Rows.Add(rr.ItemArray);
            }
        }

        return dt1;
    }

    #endregion

    #region 4D

    //4D常规项目表
    public DataTable D4_CGXMB_Select(int num, string str1, string str2, ref string result)
    {
        DataTable dt = null;
        DataTable dt1 = null;

        dt = Shove._Web.Cache.GetCacheAsDataTable("D4_CGXMB_Select");

        if (dt == null || dt.Rows.Count < 1)
        {
            DataSet ds = null;
            DAL.Procedures.P_TrendChart_4D_CGXMB(ref ds);

            if (ds == null || ds.Tables.Count < 1)
            {
                PF.GoError(ErrorNumber.Unknow, "参数错误或者数据库--无数据", this.GetType().BaseType.FullName);

                return dt1;
            }
            else
            {
                Shove._Web.Cache.SetCache("D4_CGXMB_Select", ds.Tables[0]);

                dt = ds.Tables[0];
            }
        }

        dt1 = dt.Clone();

        //通过期号来查询
        if (num == 0)
        {
            int i = Shove._Convert.StrToInt(str1, 0);
            int j = Shove._Convert.StrToInt(str2, 0);

            DataRow[] dr2 = dt.Select("Isuse>=" + i + " and  Isuse<=" + j);

            foreach (DataRow r in dr2)
            {
                dt1.Rows.Add(r.ItemArray);
            }
        }

        //通过按纽选择查询多少期
        else
        {
            if (num >= dt.Rows.Count)
            {
                num = dt.Rows.Count;
            }

            int ii = dt.Rows.Count;                           //总记录数
            int jj = ii - num;

            DataRow[] drr = dt.Select("id > " + jj + " and id <= " + ii);

            foreach (DataRow rr in drr)
            {
                dt1.Rows.Add(rr.ItemArray);
            }
        }

        return dt1;
    }

    //4D号码走势图
    public DataTable D4_ZHFB_Select(int num, string str1, string str2, ref string result)
    {
        DataTable dt = null;
        DataTable dt1 = null;

        dt = Shove._Web.Cache.GetCacheAsDataTable("D4_ZHFB_Select");

        if (dt == null || dt.Rows.Count < 1)
        {
            DataSet ds = null;
            DAL.Procedures.P_TrendChart_4D_ZHFB(ref ds);

            if (ds == null || ds.Tables.Count < 1)
            {
                PF.GoError(ErrorNumber.Unknow, "参数错误或者数据库--无数据", this.GetType().BaseType.FullName);

                return dt1;
            }
            else
            {
                Shove._Web.Cache.SetCache("D4_ZHFB_Select", ds.Tables[0]);

                dt = ds.Tables[0];
            }
        }

        dt1 = dt.Clone();

        //通过期号来查询
        if (num == 0)
        {
            int i = Shove._Convert.StrToInt(str1, 0);
            int j = Shove._Convert.StrToInt(str2, 0);

            DataRow[] dr2 = dt.Select("Isuse>=" + i + " and  Isuse<=" + j);

            foreach (DataRow r in dr2)
            {
                dt1.Rows.Add(r.ItemArray);
            }
        }

        //通过按纽选择查询多少期
        else
        {
            if (num >= dt.Rows.Count)
            {
                num = dt.Rows.Count;
            }

            int ii = dt.Rows.Count;                           //总记录数
            int jj = ii - num;

            DataRow[] drr = dt.Select("id > " + jj + " and id <= " + ii);

            foreach (DataRow rr in drr)
            {
                dt1.Rows.Add(rr.ItemArray);
            }
        }

        return dt1;
    }

    #endregion

    #region 东方6+1
    //东方6+1 号码分布
    public DataTable DF61_ZHFB_Select(int num, string str1, string str2, ref string result)
    {
        DataTable dt = null;
        DataTable dt1 = null;

        dt = Shove._Web.Cache.GetCacheAsDataTable("DF61_ZHFB_Select");

        if (dt == null || dt.Rows.Count < 1)
        {
            DataSet ds = null;
            DAL.Procedures.P_TrendChart_DF6J1_ZHFB(ref ds);

            if (ds == null || ds.Tables.Count < 1)
            {
                PF.GoError(ErrorNumber.Unknow, "参数错误或者数据库--无数据", this.GetType().BaseType.FullName);

                return dt1;
            }
            else
            {
                Shove._Web.Cache.SetCache("DF61_ZHFB_Select", ds.Tables[0]);

                dt = ds.Tables[0];
            }
        }

        dt1 = dt.Clone();

        //通过期号来查询
        if (num == 0)
        {
            int i = Shove._Convert.StrToInt(str1, 0);
            int j = Shove._Convert.StrToInt(str2, 0);

            DataRow[] dr2 = dt.Select("Isuse>=" + i + " and  Isuse<=" + j);

            foreach (DataRow r in dr2)
            {
                dt1.Rows.Add(r.ItemArray);
            }
        }

        //通过按纽选择查询多少期
        else
        {
            if (num >= dt.Rows.Count)
            {
                num = dt.Rows.Count;
            }

            int ii = dt.Rows.Count;                           //总记录数
            int jj = ii - num;

            DataRow[] drr = dt.Select("id > " + jj + " and id <= " + ii);

            foreach (DataRow rr in drr)
            {
                dt1.Rows.Add(rr.ItemArray);
            }
        }

        return dt1;
    }

    #endregion

    #region 上海时时乐

    //上海时时乐012路
    public DataTable SHSSL_012_Select(int num, string str1, string str2, ref string result)
    {
        DataTable dt = null;
        DataTable dt1 = null;

        dt = Shove._Web.Cache.GetCacheAsDataTable("SHSSL_012_Select");

        if (dt == null || dt.Rows.Count < 1)
        {
            DataSet ds = null;
            DAL.Procedures.P_TrendChart_SHSSL_012(ref ds);

            if (ds == null || ds.Tables.Count < 1)
            {
                PF.GoError(ErrorNumber.Unknow, "参数错误或者数据库--无数据", this.GetType().BaseType.FullName);

                return dt1;
            }
            else
            {
                Shove._Web.Cache.SetCache("SHSSL_012_Select", ds.Tables[0]);

                dt = ds.Tables[0];
            }
        }

        dt1 = dt.Clone();

        //通过期号来查询
        if (num == 0)
        {
            int i = Shove._Convert.StrToInt(str1, 0);
            int j = Shove._Convert.StrToInt(str2, 0);

            DataRow[] dr2 = dt.Select("Isuse>=" + i + " and  Isuse<=" + j);

            foreach (DataRow r in dr2)
            {
                dt1.Rows.Add(r.ItemArray);
            }
        }

        //通过按纽选择查询多少期
        else
        {
            if (num >= dt.Rows.Count)
            {
                num = dt.Rows.Count;
            }

            int ii = dt.Rows.Count;                           //总记录数
            int jj = ii - num;

            DataRow[] drr = dt.Select("id > " + jj + " and id <= " + ii);

            foreach (DataRow rr in drr)
            {
                dt1.Rows.Add(rr.ItemArray);
            }
        }

        return dt1;
    }

    //上海时时乐和值
    public DataTable SHSSL_HZ_Select(int num, string str1, string str2, ref string result)
    {
        DataTable dt = null;
        DataTable dt1 = null;

        dt = Shove._Web.Cache.GetCacheAsDataTable("SHSSL_HZ_Select");

        if (dt == null || dt.Rows.Count < 1)
        {
            DataSet ds = null;
            DAL.Procedures.P_TrendChart_SHSSL_HZ(ref ds);

            if (ds == null || ds.Tables.Count < 1)
            {
                PF.GoError(ErrorNumber.Unknow, "参数错误或者数据库--无数据", this.GetType().BaseType.FullName);

                return dt1;
            }
            else
            {
                Shove._Web.Cache.SetCache("SHSSL_HZ_Select", ds.Tables[0]);

                dt = ds.Tables[0];
            }
        }

        dt1 = dt.Clone();

        //通过期号来查询
        if (num == 0)
        {
            int i = Shove._Convert.StrToInt(str1, 0);
            int j = Shove._Convert.StrToInt(str2, 0);

            DataRow[] dr2 = dt.Select("Isuse>=" + i + " and  Isuse<=" + j);

            foreach (DataRow r in dr2)
            {
                dt1.Rows.Add(r.ItemArray);
            }
        }

        //通过按纽选择查询多少期
        else
        {
            if (num >= dt.Rows.Count)
            {
                num = dt.Rows.Count;
            }

            int ii = dt.Rows.Count;                           //总记录数
            int jj = ii - num;

            DataRow[] drr = dt.Select("id > " + jj + " and id <= " + ii);

            foreach (DataRow rr in drr)
            {
                dt1.Rows.Add(rr.ItemArray);
            }
        }

        return dt1;
    }

    //上海时时乐大小
    public DataTable SHSSL_DX_Select(int num, string str1, string str2, ref string result)
    {
        DataTable dt = null;
        DataTable dt1 = null;

        dt = Shove._Web.Cache.GetCacheAsDataTable("SHSSL_DX_Select");

        if (dt == null || dt.Rows.Count < 1)
        {
            DataSet ds = null;
            DAL.Procedures.P_TrendChart_SHSSL_DX(ref ds);

            if (ds == null || ds.Tables.Count < 1)
            {
                PF.GoError(ErrorNumber.Unknow, "参数错误或者数据库--无数据", this.GetType().BaseType.FullName);

                return dt1;
            }
            else
            {
                Shove._Web.Cache.SetCache("SHSSL_DX_Select", ds.Tables[0]);

                dt = ds.Tables[0];
            }
        }

        dt1 = dt.Clone();

        //通过期号来查询
        if (num == 0)
        {
            int i = Shove._Convert.StrToInt(str1, 0);
            int j = Shove._Convert.StrToInt(str2, 0);

            DataRow[] dr2 = dt.Select("Isuse>=" + i + " and  Isuse<=" + j);

            foreach (DataRow r in dr2)
            {
                dt1.Rows.Add(r.ItemArray);
            }
        }

        //通过按纽选择查询多少期
        else
        {
            if (num >= dt.Rows.Count)
            {
                num = dt.Rows.Count;
            }

            int ii = dt.Rows.Count;                           //总记录数
            int jj = ii - num;

            DataRow[] drr = dt.Select("id > " + jj + " and id <= " + ii);

            foreach (DataRow rr in drr)
            {
                dt1.Rows.Add(rr.ItemArray);
            }
        }

        return dt1;
    }

    //上海时时乐奇偶
    public DataTable SHSSL_JO_Select(int num, string str1, string str2, ref string result)
    {
        DataTable dt = null;
        DataTable dt1 = null;

        dt = Shove._Web.Cache.GetCacheAsDataTable("SHSSL_JO_Select");

        if (dt == null || dt.Rows.Count < 1)
        {
            DataSet ds = null;
            DAL.Procedures.P_TrendChart_SHSSL_JO(ref ds);

            if (ds == null || ds.Tables.Count < 1)
            {
                PF.GoError(ErrorNumber.Unknow, "参数错误或者数据库--无数据", this.GetType().BaseType.FullName);

                return dt1;
            }
            else
            {
                Shove._Web.Cache.SetCache("SHSSL_JO_Select", ds.Tables[0]);

                dt = ds.Tables[0];
            }
        }

        dt1 = dt.Clone();

        //通过期号来查询
        if (num == 0)
        {
            int i = Shove._Convert.StrToInt(str1, 0);
            int j = Shove._Convert.StrToInt(str2, 0);

            DataRow[] dr2 = dt.Select("Isuse>=" + i + " and  Isuse<=" + j);

            foreach (DataRow r in dr2)
            {
                dt1.Rows.Add(r.ItemArray);
            }
        }

        //通过按纽选择查询多少期
        else
        {
            if (num >= dt.Rows.Count)
            {
                num = dt.Rows.Count;
            }

            int ii = dt.Rows.Count;                           //总记录数
            int jj = ii - num;

            DataRow[] drr = dt.Select("id > " + jj + " and id <= " + ii);

            foreach (DataRow rr in drr)
            {
                dt1.Rows.Add(rr.ItemArray);
            }
        }

        return dt1;
    }

    //上海时时乐质合
    public DataTable SHSSL_ZH_Select(int num, string str1, string str2, ref string result)
    {
        DataTable dt = null;
        DataTable dt1 = null;

        dt = Shove._Web.Cache.GetCacheAsDataTable("SHSSL_ZH_Select");

        if (dt == null || dt.Rows.Count < 1)
        {
            DataSet ds = null;
            DAL.Procedures.P_TrendChart_SHSSL_ZH(ref ds);

            if (ds == null || ds.Tables.Count < 1)
            {
                PF.GoError(ErrorNumber.Unknow, "参数错误或者数据库--无数据", this.GetType().BaseType.FullName);

                return dt1;
            }
            else
            {
                Shove._Web.Cache.SetCache("SHSSL_ZH_Select", ds.Tables[0]);

                dt = ds.Tables[0];
            }
        }

        dt1 = dt.Clone();

        //通过期号来查询
        if (num == 0)
        {
            int i = Shove._Convert.StrToInt(str1, 0);
            int j = Shove._Convert.StrToInt(str2, 0);

            DataRow[] dr2 = dt.Select("Isuse>=" + i + " and  Isuse<=" + j);

            foreach (DataRow r in dr2)
            {
                dt1.Rows.Add(r.ItemArray);
            }
        }

        //通过按纽选择查询多少期
        else
        {
            if (num >= dt.Rows.Count)
            {
                num = dt.Rows.Count;
            }

            int ii = dt.Rows.Count;                           //总记录数
            int jj = ii - num;

            DataRow[] drr = dt.Select("id > " + jj + " and id <= " + ii);

            foreach (DataRow rr in drr)
            {
                dt1.Rows.Add(rr.ItemArray);
            }
        }

        return dt1;
    }

    //上海时时乐综合分布
    public DataTable SHSSL_ZHFB_Select(int num, string str1, string str2, ref string result)
    {
        DataTable dt = null;
        DataTable dt1 = null;

        dt = Shove._Web.Cache.GetCacheAsDataTable("SHSSL_ZHFB_Select");

        if (dt == null || dt.Rows.Count < 1)
        {
            DataSet ds = null;
            DAL.Procedures.P_TrendChart_SHSSL_ZHFB(ref ds);

            if (ds == null || ds.Tables.Count < 1)
            {
                PF.GoError(ErrorNumber.Unknow, "参数错误或者数据库--无数据", this.GetType().BaseType.FullName);

                return dt1;
            }
            else
            {
                Shove._Web.Cache.SetCache("SHSSL_ZHFB_Select", ds.Tables[0]);

                dt = ds.Tables[0];
            }
        }

        dt1 = dt.Clone();

        //通过期号来查询
        if (num == 0)
        {
            int i = Shove._Convert.StrToInt(str1, 0);
            int j = Shove._Convert.StrToInt(str2, 0);

            DataRow[] dr2 = dt.Select("Isuse>=" + i + " and  Isuse<=" + j);

            foreach (DataRow r in dr2)
            {
                dt1.Rows.Add(r.ItemArray);
            }
        }

        //通过按纽选择查询多少期
        else
        {
            if (num >= dt.Rows.Count)
            {
                num = dt.Rows.Count;
            }

            int ii = dt.Rows.Count;                           //总记录数
            int jj = ii - num;

            DataRow[] drr = dt.Select("id > " + jj + " and id <= " + ii);

            foreach (DataRow rr in drr)
            {
                dt1.Rows.Add(rr.ItemArray);
            }
        }

        return dt1;
    }

    #endregion

    #region 15选5

    //15选5 常规项目表
    public DataTable C15X5_CGXMB_Select(int num, string str1, string str2, ref string result)
    {
        DataTable dt = null;
        DataTable dt1 = null;

        dt = Shove._Web.Cache.GetCacheAsDataTable("C15X5_CGXMB_Select");

        if (dt == null || dt.Rows.Count < 1)
        {
            DataSet ds = null;
            DAL.Procedures.P_TrendChart_15X5_CGXMB(ref ds);

            if (ds == null || ds.Tables.Count < 1)
            {
                PF.GoError(ErrorNumber.Unknow, "参数错误或者数据库--无数据", this.GetType().BaseType.FullName);

                return dt1;
            }
            else
            {
                Shove._Web.Cache.SetCache("C15X5_CGXMB_Select", ds.Tables[0]);

                dt = ds.Tables[0];
            }
        }

        dt1 = dt.Clone();

        //通过期号来查询
        if (num == 0)
        {
            int i = Shove._Convert.StrToInt(str1, 0);
            int j = Shove._Convert.StrToInt(str2, 0);

            DataRow[] dr2 = dt.Select("Isuse>=" + i + " and  Isuse<=" + j);

            foreach (DataRow r in dr2)
            {
                dt1.Rows.Add(r.ItemArray);
            }
        }

        //通过按纽选择查询多少期
        else
        {
            if (num >= dt.Rows.Count)
            {
                num = dt.Rows.Count;
            }

            int ii = dt.Rows.Count;                           //总记录数
            int jj = ii - num;

            DataRow[] drr = dt.Select("id > " + jj + " and id <= " + ii);

            foreach (DataRow rr in drr)
            {
                dt1.Rows.Add(rr.ItemArray);
            }
        }

        return dt1;
    }

    //15选5综合走势图
    public DataTable C15X5_ZHZST_Select(int num, string str1, string str2, ref string result)
    {
        DataTable dt = null;
        DataTable dt1 = null;

        dt = Shove._Web.Cache.GetCacheAsDataTable("C15X5_ZHZST_Select");

        if (dt == null || dt.Rows.Count < 1)
        {
            DataSet ds = null;
            DAL.Procedures.P_TrendChart_15X5_HMFB(ref ds);

            if (ds == null || ds.Tables.Count < 1)
            {
                PF.GoError(ErrorNumber.Unknow, "参数错误或者数据库--无数据", this.GetType().BaseType.FullName);

                return dt1;
            }
            else
            {
                Shove._Web.Cache.SetCache("C15X5_ZHZST_Select", ds.Tables[0]);

                dt = ds.Tables[0];
            }
        }

        dt1 = dt.Clone();

        //通过期号来查询
        if (num == 0)
        {
            int i = Shove._Convert.StrToInt(str1, 0);
            int j = Shove._Convert.StrToInt(str2, 0);

            DataRow[] dr2 = dt.Select("Isuse>=" + i + " and  Isuse<=" + j);

            foreach (DataRow r in dr2)
            {
                dt1.Rows.Add(r.ItemArray);
            }
        }

        //通过按纽选择查询多少期
        else
        {
            if (num >= dt.Rows.Count)
            {
                num = dt.Rows.Count;
            }

            int ii = dt.Rows.Count;                           //总记录数
            int jj = ii - num;

            DataRow[] drr = dt.Select("id > " + jj + " and id <= " + ii);

            foreach (DataRow rr in drr)
            {
                dt1.Rows.Add(rr.ItemArray);
            }
        }

        return dt1;
    }

    #endregion

    #region 江西时时彩

    public DataTable SSC_5X_ZHFBZST_Select(int num, string str1, string str2, ref string result)
    {
        DataTable dt = null;
        DataTable dt1 = null;

        dt = Shove._Web.Cache.GetCacheAsDataTable("SSC_5X_ZHFBZST_Select");

        if (dt == null || dt.Rows.Count < 1)
        {
            DataSet ds = null;
            DAL.Procedures.P_TrendChart_SSC_5XZHFBZST(ref ds);

            if (ds == null || ds.Tables.Count < 1)
            {
                PF.GoError(ErrorNumber.Unknow, "参数错误或者数据库--无数据", this.GetType().BaseType.FullName);

                return dt1;
            }
            else
            {
                Shove._Web.Cache.SetCache("SSC_5X_ZHFBZST_Select", ds.Tables[0]);

                dt = ds.Tables[0];
            }
        }

        dt1 = dt.Clone();

        //通过期号来查询
        if (num == 0)
        {
            int i = Shove._Convert.StrToInt(str1, 0);
            int j = Shove._Convert.StrToInt(str2, 0);

            DataRow[] dr2 = dt.Select("Isuse>=" + i + " and  Isuse<=" + j);

            foreach (DataRow r in dr2)
            {
                dt1.Rows.Add(r.ItemArray);
            }
        }

        //通过按纽选择查询多少期
        else
        {
            if (num >= dt.Rows.Count)
            {
                num = dt.Rows.Count;
            }

            int ii = dt.Rows.Count;                           //总记录数
            int jj = ii - num;

            DataRow[] drr = dt.Select("id > " + jj + " and id <= " + ii);

            foreach (DataRow rr in drr)
            {
                dt1.Rows.Add(rr.ItemArray);
            }
        }

        return dt1;
    }

    //五星 走势图
    public DataTable SSC_5X_ZST_Select(int num, string str1, string str2, ref string result)
    {
        DataTable dt = null;
        DataTable dt1 = null;

        dt = Shove._Web.Cache.GetCacheAsDataTable("SSC_5X_ZST_Select");

        if (dt == null || dt.Rows.Count < 1)
        {
            DataSet ds = null;
            DAL.Procedures.P_TrendChart_SSC_5XZST(ref ds);

            if (ds == null || ds.Tables.Count < 1)
            {
                PF.GoError(ErrorNumber.Unknow, "参数错误或者数据库--无数据", this.GetType().BaseType.FullName);

                return dt1;
            }
            else
            {
                Shove._Web.Cache.SetCache("SSC_5X_ZST_Select", ds.Tables[0]);

                dt = ds.Tables[0];
            }
        }

        dt1 = dt.Clone();

        //通过期号来查询
        if (num == 0)
        {
            int i = Shove._Convert.StrToInt(str1, 0);
            int j = Shove._Convert.StrToInt(str2, 0);

            DataRow[] dr2 = dt.Select("Isuse>=" + i + " and  Isuse<=" + j);

            foreach (DataRow r in dr2)
            {
                dt1.Rows.Add(r.ItemArray);
            }
        }

        //通过按纽选择查询多少期
        else
        {
            if (num >= dt.Rows.Count)
            {
                num = dt.Rows.Count;
            }

            int ii = dt.Rows.Count;                           //总记录数
            int jj = ii - num;

            DataRow[] drr = dt.Select("id > " + jj + " and id <= " + ii);

            foreach (DataRow rr in drr)
            {
                dt1.Rows.Add(rr.ItemArray);
            }
        }

        return dt1;
    }

    //五星和值走势图
    public DataTable SSC_5X_HZST_Select(int num, string str1, string str2, ref string result)
    {
        DataTable dt = null;
        DataTable dt1 = null;

        dt = Shove._Web.Cache.GetCacheAsDataTable("SSC_5X_HZST_Select");

        if (dt == null || dt.Rows.Count < 1)
        {
            DataSet ds = null;
            DAL.Procedures.P_TrendChart_SSC_5XHZZST(ref ds);

            if (ds == null || ds.Tables.Count < 1)
            {
                PF.GoError(ErrorNumber.Unknow, "参数错误或者数据库--无数据", this.GetType().BaseType.FullName);

                return dt1;
            }
            else
            {
                Shove._Web.Cache.SetCache("SSC_5X_HZST_Select", ds.Tables[0]);

                dt = ds.Tables[0];
            }
        }

        dt1 = dt.Clone();

        //通过期号来查询
        if (num == 0)
        {
            int i = Shove._Convert.StrToInt(str1, 0);
            int j = Shove._Convert.StrToInt(str2, 0);

            DataRow[] dr2 = dt.Select("Isuse>=" + i + " and  Isuse<=" + j);

            foreach (DataRow r in dr2)
            {
                dt1.Rows.Add(r.ItemArray);
            }
        }

        //通过按纽选择查询多少期
        else
        {
            if (num >= dt.Rows.Count)
            {
                num = dt.Rows.Count;
            }

            int ii = dt.Rows.Count;                           //总记录数
            int jj = ii - num;

            DataRow[] drr = dt.Select("id > " + jj + " and id <= " + ii);

            foreach (DataRow rr in drr)
            {
                dt1.Rows.Add(rr.ItemArray);
            }
        }

        return dt1;
    }

    //五星跨度走势图
    public DataTable SSC_5X_KDZST_Select(int num, string str1, string str2, ref string result)
    {
        DataTable dt = null;
        DataTable dt1 = null;

        dt = Shove._Web.Cache.GetCacheAsDataTable("SSC_5X_KDZST_Select");

        if (dt == null || dt.Rows.Count < 1)
        {
            DataSet ds = null;
            DAL.Procedures.P_TrendChart_SSC_5XKDZST(ref ds);

            if (ds == null || ds.Tables.Count < 1)
            {
                PF.GoError(ErrorNumber.Unknow, "参数错误或者数据库--无数据", this.GetType().BaseType.FullName);

                return dt1;
            }
            else
            {
                Shove._Web.Cache.SetCache("SSC_5X_KDZST_Select", ds.Tables[0]);

                dt = ds.Tables[0];
            }
        }

        dt1 = dt.Clone();

        //通过期号来查询
        if (num == 0)
        {
            int i = Shove._Convert.StrToInt(str1, 0);
            int j = Shove._Convert.StrToInt(str2, 0);

            DataRow[] dr2 = dt.Select("Isuse>=" + i + " and  Isuse<=" + j);

            foreach (DataRow r in dr2)
            {
                dt1.Rows.Add(r.ItemArray);
            }
        }

        //通过按纽选择查询多少期
        else
        {
            if (num >= dt.Rows.Count)
            {
                num = dt.Rows.Count;
            }

            int ii = dt.Rows.Count;                           //总记录数
            int jj = ii - num;

            DataRow[] drr = dt.Select("id > " + jj + " and id <= " + ii);

            foreach (DataRow rr in drr)
            {
                dt1.Rows.Add(rr.ItemArray);
            }
        }

        return dt1;
    }

    //五星平均值走势图
    public DataTable SSC_5X_PJZZST_Select(int num, string str1, string str2, ref string result)
    {
        DataTable dt = null;
        DataTable dt1 = null;

        dt = Shove._Web.Cache.GetCacheAsDataTable("SSC_5X_PJZZST_Select");

        if (dt == null || dt.Rows.Count < 1)
        {
            DataSet ds = null;
            DAL.Procedures.P_TrendChart_SSC_5XPJZZST(ref ds);

            if (ds == null || ds.Tables.Count < 1)
            {
                PF.GoError(ErrorNumber.Unknow, "参数错误或者数据库--无数据", this.GetType().BaseType.FullName);

                return dt1;
            }
            else
            {
                Shove._Web.Cache.SetCache("SSC_5X_PJZZST_Select", ds.Tables[0]);

                dt = ds.Tables[0];
            }
        }

        dt1 = dt.Clone();

        //通过期号来查询
        if (num == 0)
        {
            int i = Shove._Convert.StrToInt(str1, 0);
            int j = Shove._Convert.StrToInt(str2, 0);

            DataRow[] dr2 = dt.Select("Isuse>=" + i + " and  Isuse<=" + j);

            foreach (DataRow r in dr2)
            {
                dt1.Rows.Add(r.ItemArray);
            }
        }

        //通过按纽选择查询多少期
        else
        {
            if (num >= dt.Rows.Count)
            {
                num = dt.Rows.Count;
            }

            int ii = dt.Rows.Count;                           //总记录数
            int jj = ii - num;

            DataRow[] drr = dt.Select("id > " + jj + " and id <= " + ii);

            foreach (DataRow rr in drr)
            {
                dt1.Rows.Add(rr.ItemArray);
            }
        }

        return dt1;
    }

    //五星大小走势图
    public DataTable SSC_5X_DXZST_Select(int num, string str1, string str2, ref string result)
    {
        DataTable dt = null;
        DataTable dt1 = null;

        dt = Shove._Web.Cache.GetCacheAsDataTable("SSC_5X_DXZST_Select");

        if (dt == null || dt.Rows.Count < 1)
        {
            DataSet ds = null;
            DAL.Procedures.P_TrendChart_SSC_5XDXZST(ref ds);

            if (ds == null || ds.Tables.Count < 1)
            {
                PF.GoError(ErrorNumber.Unknow, "参数错误或者数据库--无数据", this.GetType().BaseType.FullName);

                return dt1;
            }
            else
            {
                Shove._Web.Cache.SetCache("SSC_5X_DXZST_Select", ds.Tables[0]);

                dt = ds.Tables[0];
            }
        }

        dt1 = dt.Clone();

        //通过期号来查询
        if (num == 0)
        {
            int i = Shove._Convert.StrToInt(str1, 0);
            int j = Shove._Convert.StrToInt(str2, 0);

            DataRow[] dr2 = dt.Select("Isuse>=" + i + " and  Isuse<=" + j);

            foreach (DataRow r in dr2)
            {
                dt1.Rows.Add(r.ItemArray);
            }
        }

        //通过按纽选择查询多少期
        else
        {
            if (num >= dt.Rows.Count)
            {
                num = dt.Rows.Count;
            }

            int ii = dt.Rows.Count;                           //总记录数
            int jj = ii - num;

            DataRow[] drr = dt.Select("id > " + jj + " and id <= " + ii);

            foreach (DataRow rr in drr)
            {
                dt1.Rows.Add(rr.ItemArray);
            }
        }

        return dt1;
    }

    //五星奇偶走势图
    public DataTable SSC_5X_JOZST_Select(int num, string str1, string str2, ref string result)
    {
        DataTable dt = null;
        DataTable dt1 = null;

        dt = Shove._Web.Cache.GetCacheAsDataTable("SSC_5X_JOZST_Select");

        if (dt == null || dt.Rows.Count < 1)
        {
            DataSet ds = null;
            DAL.Procedures.P_TrendChart_SSC_5XJOZST(ref ds);

            if (ds == null || ds.Tables.Count < 1)
            {
                PF.GoError(ErrorNumber.Unknow, "参数错误或者数据库--无数据", this.GetType().BaseType.FullName);

                return dt1;
            }
            else
            {
                Shove._Web.Cache.SetCache("SSC_5X_JOZST_Select", ds.Tables[0]);

                dt = ds.Tables[0];
            }
        }

        dt1 = dt.Clone();

        //通过期号来查询
        if (num == 0)
        {
            int i = Shove._Convert.StrToInt(str1, 0);
            int j = Shove._Convert.StrToInt(str2, 0);

            DataRow[] dr2 = dt.Select("Isuse>=" + i + " and  Isuse<=" + j);

            foreach (DataRow r in dr2)
            {
                dt1.Rows.Add(r.ItemArray);
            }
        }

        //通过按纽选择查询多少期
        else
        {
            if (num >= dt.Rows.Count)
            {
                num = dt.Rows.Count;
            }

            int ii = dt.Rows.Count;                           //总记录数
            int jj = ii - num;

            DataRow[] drr = dt.Select("id > " + jj + " and id <= " + ii);

            foreach (DataRow rr in drr)
            {
                dt1.Rows.Add(rr.ItemArray);
            }
        }

        return dt1;
    }

    //五星质合走势图
    public DataTable SSC_5X_ZHZST_Select(int num, string str1, string str2, ref string result)
    {
        DataTable dt = null;
        DataTable dt1 = null;

        dt = Shove._Web.Cache.GetCacheAsDataTable("SSC_5X_ZHZST_Select");

        if (dt == null || dt.Rows.Count < 1)
        {
            DataSet ds = null;
            DAL.Procedures.P_TrendChart_SSC_5XZHZST(ref ds);

            if (ds == null || ds.Tables.Count < 1)
            {
                PF.GoError(ErrorNumber.Unknow, "参数错误或者数据库--无数据", this.GetType().BaseType.FullName);

                return dt1;
            }
            else
            {
                Shove._Web.Cache.SetCache("SSC_5X_ZHZST_Select", ds.Tables[0]);

                dt = ds.Tables[0];
            }
        }

        dt1 = dt.Clone();

        //通过期号来查询
        if (num == 0)
        {
            int i = Shove._Convert.StrToInt(str1, 0);
            int j = Shove._Convert.StrToInt(str2, 0);

            DataRow[] dr2 = dt.Select("Isuse>=" + i + " and  Isuse<=" + j);

            foreach (DataRow r in dr2)
            {
                dt1.Rows.Add(r.ItemArray);
            }
        }

        //通过按纽选择查询多少期
        else
        {
            if (num >= dt.Rows.Count)
            {
                num = dt.Rows.Count;
            }

            int ii = dt.Rows.Count;                           //总记录数
            int jj = ii - num;

            DataRow[] drr = dt.Select("id > " + jj + " and id <= " + ii);

            foreach (DataRow rr in drr)
            {
                dt1.Rows.Add(rr.ItemArray);
            }
        }

        return dt1;
    }

    //-----------------------------------------------------------------------------------------------------

    //4星 走势图

    public DataTable SSC_4X_ZHFBZST_Select(int num, string str1, string str2, ref string result)
    {
        DataTable dt = null;
        DataTable dt1 = null;

        dt = Shove._Web.Cache.GetCacheAsDataTable("SSC_4X_ZHFBZST_Select");

        if (dt == null || dt.Rows.Count < 1)
        {
            DataSet ds = null;
            DAL.Procedures.P_TrendChart_SSC_4XZHFBZST(ref ds);

            if (ds == null || ds.Tables.Count < 1)
            {
                PF.GoError(ErrorNumber.Unknow, "参数错误或者数据库--无数据", this.GetType().BaseType.FullName);

                return dt1;
            }
            else
            {
                Shove._Web.Cache.SetCache("SSC_4X_ZHFBZST_Select", ds.Tables[0]);

                dt = ds.Tables[0];
            }
        }

        dt1 = dt.Clone();

        //通过期号来查询
        if (num == 0)
        {
            int i = Shove._Convert.StrToInt(str1, 0);
            int j = Shove._Convert.StrToInt(str2, 0);

            DataRow[] dr2 = dt.Select("Isuse>=" + i + " and  Isuse<=" + j);

            foreach (DataRow r in dr2)
            {
                dt1.Rows.Add(r.ItemArray);
            }
        }

        //通过按纽选择查询多少期
        else
        {
            if (num >= dt.Rows.Count)
            {
                num = dt.Rows.Count;
            }

            int ii = dt.Rows.Count;                           //总记录数
            int jj = ii - num;

            DataRow[] drr = dt.Select("id > " + jj + " and id <= " + ii);

            foreach (DataRow rr in drr)
            {
                dt1.Rows.Add(rr.ItemArray);
            }
        }

        return dt1;
    }

    //4星 走势图
    public DataTable SSC_4X_ZST_Select(int num, string str1, string str2, ref string result)
    {
        DataTable dt = null;
        DataTable dt1 = null;

        dt = Shove._Web.Cache.GetCacheAsDataTable("SSC_4X_ZST_Select");

        if (dt == null || dt.Rows.Count < 1)
        {
            DataSet ds = null;
            DAL.Procedures.P_TrendChart_SSC_4XZST(ref ds);

            if (ds == null || ds.Tables.Count < 1)
            {
                PF.GoError(ErrorNumber.Unknow, "参数错误或者数据库--无数据", this.GetType().BaseType.FullName);

                return dt1;
            }
            else
            {
                Shove._Web.Cache.SetCache("SSC_4X_ZST_Select", ds.Tables[0]);

                dt = ds.Tables[0];
            }
        }

        dt1 = dt.Clone();

        //通过期号来查询
        if (num == 0)
        {
            int i = Shove._Convert.StrToInt(str1, 0);
            int j = Shove._Convert.StrToInt(str2, 0);

            DataRow[] dr2 = dt.Select("Isuse>=" + i + " and  Isuse<=" + j);

            foreach (DataRow r in dr2)
            {
                dt1.Rows.Add(r.ItemArray);
            }
        }

        //通过按纽选择查询多少期
        else
        {
            if (num >= dt.Rows.Count)
            {
                num = dt.Rows.Count;
            }

            int ii = dt.Rows.Count;                           //总记录数
            int jj = ii - num;

            DataRow[] drr = dt.Select("id > " + jj + " and id <= " + ii);

            foreach (DataRow rr in drr)
            {
                dt1.Rows.Add(rr.ItemArray);
            }
        }

        return dt1;
    }

    //4星和值走势图
    public DataTable SSC_4X_HZST_Select(int num, string str1, string str2, ref string result)
    {
        DataTable dt = null;
        DataTable dt1 = null;

        dt = Shove._Web.Cache.GetCacheAsDataTable("SSC_4X_HZST_Select");

        if (dt == null || dt.Rows.Count < 1)
        {
            DataSet ds = null;
            DAL.Procedures.P_TrendChart_SSC_4XHZZST(ref ds);

            if (ds == null || ds.Tables.Count < 1)
            {
                PF.GoError(ErrorNumber.Unknow, "参数错误或者数据库--无数据", this.GetType().BaseType.FullName);

                return dt1;
            }
            else
            {
                Shove._Web.Cache.SetCache("SSC_4X_HZST_Select", ds.Tables[0]);

                dt = ds.Tables[0];
            }
        }

        dt1 = dt.Clone();

        //通过期号来查询
        if (num == 0)
        {
            int i = Shove._Convert.StrToInt(str1, 0);
            int j = Shove._Convert.StrToInt(str2, 0);

            DataRow[] dr2 = dt.Select("Isuse>=" + i + " and  Isuse<=" + j);

            foreach (DataRow r in dr2)
            {
                dt1.Rows.Add(r.ItemArray);
            }
        }

        //通过按纽选择查询多少期
        else
        {
            if (num >= dt.Rows.Count)
            {
                num = dt.Rows.Count;
            }

            int ii = dt.Rows.Count;                           //总记录数
            int jj = ii - num;

            DataRow[] drr = dt.Select("id > " + jj + " and id <= " + ii);

            foreach (DataRow rr in drr)
            {
                dt1.Rows.Add(rr.ItemArray);
            }
        }

        return dt1;
    }

    //4星跨度走势图
    public DataTable SSC_4X_KDZST_Select(int num, string str1, string str2, ref string result)
    {
        DataTable dt = null;
        DataTable dt1 = null;

        dt = Shove._Web.Cache.GetCacheAsDataTable("SSC_4X_KDZST_Select");

        if (dt == null || dt.Rows.Count < 1)
        {
            DataSet ds = null;
            DAL.Procedures.P_TrendChart_SSC_4XKDZST(ref ds);

            if (ds == null || ds.Tables.Count < 1)
            {
                PF.GoError(ErrorNumber.Unknow, "参数错误或者数据库--无数据", this.GetType().BaseType.FullName);

                return dt1;
            }
            else
            {
                Shove._Web.Cache.SetCache("SSC_4X_KDZST_Select", ds.Tables[0]);

                dt = ds.Tables[0];
            }
        }

        dt1 = dt.Clone();

        //通过期号来查询
        if (num == 0)
        {
            int i = Shove._Convert.StrToInt(str1, 0);
            int j = Shove._Convert.StrToInt(str2, 0);

            DataRow[] dr2 = dt.Select("Isuse>=" + i + " and  Isuse<=" + j);

            foreach (DataRow r in dr2)
            {
                dt1.Rows.Add(r.ItemArray);
            }
        }

        //通过按纽选择查询多少期
        else
        {
            if (num >= dt.Rows.Count)
            {
                num = dt.Rows.Count;
            }

            int ii = dt.Rows.Count;                           //总记录数
            int jj = ii - num;

            DataRow[] drr = dt.Select("id > " + jj + " and id <= " + ii);

            foreach (DataRow rr in drr)
            {
                dt1.Rows.Add(rr.ItemArray);
            }
        }

        return dt1;
    }

    //4星平均值走势图
    public DataTable SSC_4X_PJZZST_Select(int num, string str1, string str2, ref string result)
    {
        DataTable dt = null;
        DataTable dt1 = null;

        dt = Shove._Web.Cache.GetCacheAsDataTable("SSC_4X_PJZZST_Select");

        if (dt == null || dt.Rows.Count < 1)
        {
            DataSet ds = null;
            DAL.Procedures.P_TrendChart_SSC_4XPJZZST(ref ds);

            if (ds == null || ds.Tables.Count < 1)
            {
                PF.GoError(ErrorNumber.Unknow, "参数错误或者数据库--无数据", this.GetType().BaseType.FullName);

                return dt1;
            }
            else
            {
                Shove._Web.Cache.SetCache("SSC_4X_PJZZST_Select", ds.Tables[0]);

                dt = ds.Tables[0];
            }
        }

        dt1 = dt.Clone();

        //通过期号来查询
        if (num == 0)
        {
            int i = Shove._Convert.StrToInt(str1, 0);
            int j = Shove._Convert.StrToInt(str2, 0);

            DataRow[] dr2 = dt.Select("Isuse>=" + i + " and  Isuse<=" + j);

            foreach (DataRow r in dr2)
            {
                dt1.Rows.Add(r.ItemArray);
            }
        }

        //通过按纽选择查询多少期
        else
        {
            if (num >= dt.Rows.Count)
            {
                num = dt.Rows.Count;
            }

            int ii = dt.Rows.Count;                           //总记录数
            int jj = ii - num;

            DataRow[] drr = dt.Select("id > " + jj + " and id <= " + ii);

            foreach (DataRow rr in drr)
            {
                dt1.Rows.Add(rr.ItemArray);
            }
        }

        return dt1;
    }

    //4星大小走势图
    public DataTable SSC_4X_DXZST_Select(int num, string str1, string str2, ref string result)
    {
        DataTable dt = null;
        DataTable dt1 = null;

        dt = Shove._Web.Cache.GetCacheAsDataTable("SSC_4X_DXZST_Select");

        if (dt == null || dt.Rows.Count < 1)
        {
            DataSet ds = null;
            DAL.Procedures.P_TrendChart_SSC_4XDXZST(ref ds);

            if (ds == null || ds.Tables.Count < 1)
            {
                PF.GoError(ErrorNumber.Unknow, "参数错误或者数据库--无数据", this.GetType().BaseType.FullName);

                return dt1;
            }
            else
            {
                Shove._Web.Cache.SetCache("SSC_4X_DXZST_Select", ds.Tables[0]);

                dt = ds.Tables[0];
            }
        }

        dt1 = dt.Clone();

        //通过期号来查询
        if (num == 0)
        {
            int i = Shove._Convert.StrToInt(str1, 0);
            int j = Shove._Convert.StrToInt(str2, 0);

            DataRow[] dr2 = dt.Select("Isuse>=" + i + " and  Isuse<=" + j);

            foreach (DataRow r in dr2)
            {
                dt1.Rows.Add(r.ItemArray);
            }
        }

        //通过按纽选择查询多少期
        else
        {
            if (num >= dt.Rows.Count)
            {
                num = dt.Rows.Count;
            }

            int ii = dt.Rows.Count;                           //总记录数
            int jj = ii - num;

            DataRow[] drr = dt.Select("id > " + jj + " and id <= " + ii);

            foreach (DataRow rr in drr)
            {
                dt1.Rows.Add(rr.ItemArray);
            }
        }

        return dt1;
    }

    //4星奇偶走势图
    public DataTable SSC_4X_JOZST_Select(int num, string str1, string str2, ref string result)
    {
        DataTable dt = null;
        DataTable dt1 = null;

        dt = Shove._Web.Cache.GetCacheAsDataTable("SSC_4X_JOZST_Select");

        if (dt == null || dt.Rows.Count < 1)
        {
            DataSet ds = null;
            DAL.Procedures.P_TrendChart_SSC_4XJOZST(ref ds);

            if (ds == null || ds.Tables.Count < 1)
            {
                PF.GoError(ErrorNumber.Unknow, "参数错误或者数据库--无数据", this.GetType().BaseType.FullName);

                return dt1;
            }
            else
            {
                Shove._Web.Cache.SetCache("SSC_4X_JOZST_Select", ds.Tables[0]);

                dt = ds.Tables[0];
            }
        }

        dt1 = dt.Clone();

        //通过期号来查询
        if (num == 0)
        {
            int i = Shove._Convert.StrToInt(str1, 0);
            int j = Shove._Convert.StrToInt(str2, 0);

            DataRow[] dr2 = dt.Select("Isuse>=" + i + " and  Isuse<=" + j);

            foreach (DataRow r in dr2)
            {
                dt1.Rows.Add(r.ItemArray);
            }
        }

        //通过按纽选择查询多少期
        else
        {
            if (num >= dt.Rows.Count)
            {
                num = dt.Rows.Count;
            }

            int ii = dt.Rows.Count;                           //总记录数
            int jj = ii - num;

            DataRow[] drr = dt.Select("id > " + jj + " and id <= " + ii);

            foreach (DataRow rr in drr)
            {
                dt1.Rows.Add(rr.ItemArray);
            }
        }

        return dt1;
    }

    //4星质合走势图
    public DataTable SSC_4X_ZHZST_Select(int num, string str1, string str2, ref string result)
    {
        DataTable dt = null;
        DataTable dt1 = null;

        dt = Shove._Web.Cache.GetCacheAsDataTable("SSC_4X_ZHZST_Select");

        if (dt == null || dt.Rows.Count < 1)
        {
            DataSet ds = null;
            DAL.Procedures.P_TrendChart_SSC_4XZHZST(ref ds);

            if (ds == null || ds.Tables.Count < 1)
            {
                PF.GoError(ErrorNumber.Unknow, "参数错误或者数据库--无数据", this.GetType().BaseType.FullName);

                return dt1;
            }
            else
            {
                Shove._Web.Cache.SetCache("SSC_4X_ZHZST_Select", ds.Tables[0]);

                dt = ds.Tables[0];
            }
        }

        dt1 = dt.Clone();

        //通过期号来查询
        if (num == 0)
        {
            int i = Shove._Convert.StrToInt(str1, 0);
            int j = Shove._Convert.StrToInt(str2, 0);

            DataRow[] dr2 = dt.Select("Isuse>=" + i + " and  Isuse<=" + j);

            foreach (DataRow r in dr2)
            {
                dt1.Rows.Add(r.ItemArray);
            }
        }

        //通过按纽选择查询多少期
        else
        {
            if (num >= dt.Rows.Count)
            {
                num = dt.Rows.Count;
            }

            int ii = dt.Rows.Count;                           //总记录数
            int jj = ii - num;

            DataRow[] drr = dt.Select("id > " + jj + " and id <= " + ii);

            foreach (DataRow rr in drr)
            {
                dt1.Rows.Add(rr.ItemArray);
            }
        }

        return dt1;
    }

    //-----------------------------------------------------------------------------------------------------

    //3星 走势图

    public DataTable SSC_3X_ZHFBZST_Select(int num, string str1, string str2, ref string result)
    {
        DataTable dt = null;
        DataTable dt1 = null;

        dt = Shove._Web.Cache.GetCacheAsDataTable("SSC_3X_ZHFBZST_Select");

        if (dt == null || dt.Rows.Count < 1)
        {
            DataSet ds = null;
            DAL.Procedures.P_TrendChart_SSC_3XZHFBZST(ref ds);

            if (ds == null || ds.Tables.Count < 1)
            {
                PF.GoError(ErrorNumber.Unknow, "参数错误或者数据库--无数据", this.GetType().BaseType.FullName);

                return dt1;
            }
            else
            {
                Shove._Web.Cache.SetCache("SSC_3X_ZHFBZST_Select", ds.Tables[0]);

                dt = ds.Tables[0];
            }
        }

        dt1 = dt.Clone();

        //通过期号来查询
        if (num == 0)
        {
            int i = Shove._Convert.StrToInt(str1, 0);
            int j = Shove._Convert.StrToInt(str2, 0);

            DataRow[] dr2 = dt.Select("Isuse>=" + i + " and  Isuse<=" + j);

            foreach (DataRow r in dr2)
            {
                dt1.Rows.Add(r.ItemArray);
            }
        }

        //通过按纽选择查询多少期
        else
        {
            if (num >= dt.Rows.Count)
            {
                num = dt.Rows.Count;
            }

            int ii = dt.Rows.Count;                           //总记录数
            int jj = ii - num;

            DataRow[] drr = dt.Select("id > " + jj + " and id <= " + ii);

            foreach (DataRow rr in drr)
            {
                dt1.Rows.Add(rr.ItemArray);
            }
        }

        return dt1;
    }

    //3星 走势图
    public DataTable SSC_3X_ZST_Select(int num, string str1, string str2, ref string result)
    {
        DataTable dt = null;
        DataTable dt1 = null;

        dt = Shove._Web.Cache.GetCacheAsDataTable("SSC_3X_ZST_Select");

        if (dt == null || dt.Rows.Count < 1)
        {
            DataSet ds = null;
            DAL.Procedures.P_TrendChart_SSC_3XZST(ref ds);

            if (ds == null || ds.Tables.Count < 1)
            {
                PF.GoError(ErrorNumber.Unknow, "参数错误或者数据库--无数据", this.GetType().BaseType.FullName);

                return dt1;
            }
            else
            {
                Shove._Web.Cache.SetCache("SSC_3X_ZST_Select", ds.Tables[0]);

                dt = ds.Tables[0];
            }
        }

        dt1 = dt.Clone();

        //通过期号来查询
        if (num == 0)
        {
            int i = Shove._Convert.StrToInt(str1, 0);
            int j = Shove._Convert.StrToInt(str2, 0);

            DataRow[] dr2 = dt.Select("Isuse>=" + i + " and  Isuse<=" + j);

            foreach (DataRow r in dr2)
            {
                dt1.Rows.Add(r.ItemArray);
            }
        }

        //通过按纽选择查询多少期
        else
        {
            if (num >= dt.Rows.Count)
            {
                num = dt.Rows.Count;
            }

            int ii = dt.Rows.Count;                           //总记录数
            int jj = ii - num;

            DataRow[] drr = dt.Select("id > " + jj + " and id <= " + ii);

            foreach (DataRow rr in drr)
            {
                dt1.Rows.Add(rr.ItemArray);
            }
        }

        return dt1;
    }

    //3星和值走势图
    public DataTable SSC_3X_HZST_Select(int num, string str1, string str2, ref string result)
    {
        DataTable dt = null;
        DataTable dt1 = null;

        dt = Shove._Web.Cache.GetCacheAsDataTable("SSC_3X_HZST_Select");

        if (dt == null || dt.Rows.Count < 1)
        {
            DataSet ds = null;
            DAL.Procedures.P_TrendChart_SSC_3XHZZST(ref ds);

            if (ds == null || ds.Tables.Count < 1)
            {
                PF.GoError(ErrorNumber.Unknow, "参数错误或者数据库--无数据", this.GetType().BaseType.FullName);

                return dt1;
            }
            else
            {
                Shove._Web.Cache.SetCache("SSC_3X_HZST_Select", ds.Tables[0]);

                dt = ds.Tables[0];
            }
        }

        dt1 = dt.Clone();

        //通过期号来查询
        if (num == 0)
        {
            int i = Shove._Convert.StrToInt(str1, 0);
            int j = Shove._Convert.StrToInt(str2, 0);

            DataRow[] dr2 = dt.Select("Isuse>=" + i + " and  Isuse<=" + j);

            foreach (DataRow r in dr2)
            {
                dt1.Rows.Add(r.ItemArray);
            }
        }

        //通过按纽选择查询多少期
        else
        {
            if (num >= dt.Rows.Count)
            {
                num = dt.Rows.Count;
            }

            int ii = dt.Rows.Count;                           //总记录数
            int jj = ii - num;

            DataRow[] drr = dt.Select("id > " + jj + " and id <= " + ii);

            foreach (DataRow rr in drr)
            {
                dt1.Rows.Add(rr.ItemArray);
            }
        }

        return dt1;
    }

    //三星和值尾走势图；
    public DataTable SSC_3X_HZWST_Select(int num, string str1, string str2, ref string result)
    {
        DataTable dt = null;
        DataTable dt1 = null;

        dt = Shove._Web.Cache.GetCacheAsDataTable("SSC_3X_HZWST_Select");

        if (dt == null || dt.Rows.Count < 1)
        {
            DataSet ds = null;
            DAL.Procedures.P_TrendChart_SSC_3XHZWZST(ref ds);

            if (ds == null || ds.Tables.Count < 1)
            {
                PF.GoError(ErrorNumber.Unknow, "参数错误或者数据库--无数据", this.GetType().BaseType.FullName);

                return dt1;
            }
            else
            {
                Shove._Web.Cache.SetCache("SSC_3X_HZWST_Select", ds.Tables[0]);

                dt = ds.Tables[0];
            }
        }

        dt1 = dt.Clone();

        //通过期号来查询
        if (num == 0)
        {
            int i = Shove._Convert.StrToInt(str1, 0);
            int j = Shove._Convert.StrToInt(str2, 0);

            DataRow[] dr2 = dt.Select("Isuse>=" + i + " and  Isuse<=" + j);

            foreach (DataRow r in dr2)
            {
                dt1.Rows.Add(r.ItemArray);
            }
        }

        //通过按纽选择查询多少期
        else
        {
            if (num >= dt.Rows.Count)
            {
                num = dt.Rows.Count;
            }

            int ii = dt.Rows.Count;                           //总记录数
            int jj = ii - num;

            DataRow[] drr = dt.Select("id > " + jj + " and id <= " + ii);

            foreach (DataRow rr in drr)
            {
                dt1.Rows.Add(rr.ItemArray);
            }
        }

        return dt1;
    }

    //三星跨度走势图
    public DataTable SSC_3X_KDZST_Select(int num, string str1, string str2, ref string result)
    {
        DataTable dt = null;
        DataTable dt1 = null;

        dt = Shove._Web.Cache.GetCacheAsDataTable("SSC_3X_KDZST_Select");

        if (dt == null || dt.Rows.Count < 1)
        {
            DataSet ds = null;
            DAL.Procedures.P_TrendChart_SSC_3XKDZST(ref ds);

            if (ds == null || ds.Tables.Count < 1)
            {
                PF.GoError(ErrorNumber.Unknow, "参数错误或者数据库--无数据", this.GetType().BaseType.FullName);

                return dt1;
            }
            else
            {
                Shove._Web.Cache.SetCache("SSC_3X_KDZST_Select", ds.Tables[0]);

                dt = ds.Tables[0];
            }
        }

        dt1 = dt.Clone();

        //通过期号来查询
        if (num == 0)
        {
            int i = Shove._Convert.StrToInt(str1, 0);
            int j = Shove._Convert.StrToInt(str2, 0);

            DataRow[] dr2 = dt.Select("Isuse>=" + i + " and  Isuse<=" + j);

            foreach (DataRow r in dr2)
            {
                dt1.Rows.Add(r.ItemArray);
            }
        }

        //通过按纽选择查询多少期
        else
        {
            if (num >= dt.Rows.Count)
            {
                num = dt.Rows.Count;
            }

            int ii = dt.Rows.Count;                           //总记录数
            int jj = ii - num;

            DataRow[] drr = dt.Select("id > " + jj + " and id <= " + ii);

            foreach (DataRow rr in drr)
            {
                dt1.Rows.Add(rr.ItemArray);
            }
        }

        return dt1;
    }

    //3星大小走势图
    public DataTable SSC_3X_DXZST_Select(int num, string str1, string str2, ref string result)
    {
        DataTable dt = null;
        DataTable dt1 = null;

        dt = Shove._Web.Cache.GetCacheAsDataTable("SSC_3X_DXZST_Select");

        if (dt == null || dt.Rows.Count < 1)
        {
            DataSet ds = null;
            DAL.Procedures.P_TrendChart_SSC_3XDXZST(ref ds);

            if (ds == null || ds.Tables.Count < 1)
            {
                PF.GoError(ErrorNumber.Unknow, "参数错误或者数据库--无数据", this.GetType().BaseType.FullName);

                return dt1;
            }
            else
            {
                Shove._Web.Cache.SetCache("SSC_3X_DXZST_Select", ds.Tables[0]);

                dt = ds.Tables[0];
            }
        }

        dt1 = dt.Clone();

        //通过期号来查询
        if (num == 0)
        {
            int i = Shove._Convert.StrToInt(str1, 0);
            int j = Shove._Convert.StrToInt(str2, 0);

            DataRow[] dr2 = dt.Select("Isuse>=" + i + " and  Isuse<=" + j);

            foreach (DataRow r in dr2)
            {
                dt1.Rows.Add(r.ItemArray);
            }
        }

        //通过按纽选择查询多少期
        else
        {
            if (num >= dt.Rows.Count)
            {
                num = dt.Rows.Count;
            }

            int ii = dt.Rows.Count;                           //总记录数
            int jj = ii - num;

            DataRow[] drr = dt.Select("id > " + jj + " and id <= " + ii);

            foreach (DataRow rr in drr)
            {
                dt1.Rows.Add(rr.ItemArray);
            }
        }

        return dt1;
    }

    //3星奇偶走势图
    public DataTable SSC_3X_JOZST_Select(int num, string str1, string str2, ref string result)
    {
        DataTable dt = null;
        DataTable dt1 = null;

        dt = Shove._Web.Cache.GetCacheAsDataTable("SSC_3X_JOZST_Select");

        if (dt == null || dt.Rows.Count < 1)
        {
            DataSet ds = null;
            DAL.Procedures.P_TrendChart_SSC_3XJOZST(ref ds);

            if (ds == null || ds.Tables.Count < 1)
            {
                PF.GoError(ErrorNumber.Unknow, "参数错误或者数据库--无数据", this.GetType().BaseType.FullName);

                return dt1;
            }
            else
            {
                Shove._Web.Cache.SetCache("SSC_3X_JOZST_Select", ds.Tables[0]);

                dt = ds.Tables[0];
            }
        }

        dt1 = dt.Clone();

        //通过期号来查询
        if (num == 0)
        {
            int i = Shove._Convert.StrToInt(str1, 0);
            int j = Shove._Convert.StrToInt(str2, 0);

            DataRow[] dr2 = dt.Select("Isuse>=" + i + " and  Isuse<=" + j);

            foreach (DataRow r in dr2)
            {
                dt1.Rows.Add(r.ItemArray);
            }
        }

        //通过按纽选择查询多少期
        else
        {
            if (num >= dt.Rows.Count)
            {
                num = dt.Rows.Count;
            }

            int ii = dt.Rows.Count;                           //总记录数
            int jj = ii - num;

            DataRow[] drr = dt.Select("id > " + jj + " and id <= " + ii);

            foreach (DataRow rr in drr)
            {
                dt1.Rows.Add(rr.ItemArray);
            }
        }

        return dt1;
    }

    //3星单选012走势图
    public DataTable SSC_3X_DX_012_ZST_Select(int num, string str1, string str2, ref string result)
    {
        DataTable dt = null;
        DataTable dt1 = null;

        dt = Shove._Web.Cache.GetCacheAsDataTable("SSC_3X_DX_012_ZST_Select");

        if (dt == null || dt.Rows.Count < 1)
        {
            DataSet ds = null;
            DAL.Procedures.P_TrendChart_SSC_3X_DX012_ZST(ref ds);

            if (ds == null || ds.Tables.Count < 1)
            {
                PF.GoError(ErrorNumber.Unknow, "参数错误或者数据库--无数据", this.GetType().BaseType.FullName);

                return dt1;
            }
            else
            {
                Shove._Web.Cache.SetCache("SSC_3X_DX_012_ZST_Select", ds.Tables[0]);

                dt = ds.Tables[0];
            }
        }

        dt1 = dt.Clone();

        //通过期号来查询
        if (num == 0)
        {
            int i = Shove._Convert.StrToInt(str1, 0);
            int j = Shove._Convert.StrToInt(str2, 0);

            DataRow[] dr2 = dt.Select("Isuse>=" + i + " and  Isuse<=" + j);

            foreach (DataRow r in dr2)
            {
                dt1.Rows.Add(r.ItemArray);
            }
        }

        //通过按纽选择查询多少期
        else
        {
            if (num >= dt.Rows.Count)
            {
                num = dt.Rows.Count;
            }

            int ii = dt.Rows.Count;                           //总记录数
            int jj = ii - num;

            DataRow[] drr = dt.Select("id > " + jj + " and id <= " + ii);

            foreach (DataRow rr in drr)
            {
                dt1.Rows.Add(rr.ItemArray);
            }
        }

        return dt1;
    }

    //3星组选012走势图
    public DataTable SSC_3X_ZX_012_ZST_Select(int num, string str1, string str2, ref string result)
    {
        DataTable dt = null;
        DataTable dt1 = null;

        dt = Shove._Web.Cache.GetCacheAsDataTable("SSC_3X_ZX_012_ZST_Select");

        if (dt == null || dt.Rows.Count < 1)
        {
            DataSet ds = null;
            DAL.Procedures.P_TrendChart_SSC_3X_ZX012_ZST(ref ds);

            if (ds == null || ds.Tables.Count < 1)
            {
                PF.GoError(ErrorNumber.Unknow, "参数错误或者数据库--无数据", this.GetType().BaseType.FullName);

                return dt1;
            }
            else
            {
                Shove._Web.Cache.SetCache("SSC_3X_ZX_012_ZST_Select", ds.Tables[0]);

                dt = ds.Tables[0];
            }
        }

        dt1 = dt.Clone();

        //通过期号来查询
        if (num == 0)
        {
            int i = Shove._Convert.StrToInt(str1, 0);
            int j = Shove._Convert.StrToInt(str2, 0);

            DataRow[] dr2 = dt.Select("Isuse>=" + i + " and  Isuse<=" + j);

            foreach (DataRow r in dr2)
            {
                dt1.Rows.Add(r.ItemArray);
            }
        }

        //通过按纽选择查询多少期
        else
        {
            if (num >= dt.Rows.Count)
            {
                num = dt.Rows.Count;
            }

            int ii = dt.Rows.Count;                           //总记录数
            int jj = ii - num;

            DataRow[] drr = dt.Select("id > " + jj + " and id <= " + ii);

            foreach (DataRow rr in drr)
            {
                dt1.Rows.Add(rr.ItemArray);
            }
        }

        return dt1;
    }

    //3星质合走势图
    public DataTable SSC_3X_ZHZST_Select(int num, string str1, string str2, ref string result)
    {
        DataTable dt = null;
        DataTable dt1 = null;

        dt = Shove._Web.Cache.GetCacheAsDataTable("SSC_3X_ZHZST_Select");

        if (dt == null || dt.Rows.Count < 1)
        {
            DataSet ds = null;
            DAL.Procedures.P_TrendChart_SSC_3XZHZST(ref ds);

            if (ds == null || ds.Tables.Count < 1)
            {
                PF.GoError(ErrorNumber.Unknow, "参数错误或者数据库--无数据", this.GetType().BaseType.FullName);

                return dt1;
            }
            else
            {
                Shove._Web.Cache.SetCache("SSC_3X_ZHZST_Select", ds.Tables[0]);

                dt = ds.Tables[0];
            }
        }

        dt1 = dt.Clone();

        //通过期号来查询
        if (num == 0)
        {
            int i = Shove._Convert.StrToInt(str1, 0);
            int j = Shove._Convert.StrToInt(str2, 0);

            DataRow[] dr2 = dt.Select("Isuse>=" + i + " and  Isuse<=" + j);

            foreach (DataRow r in dr2)
            {
                dt1.Rows.Add(r.ItemArray);
            }
        }

        //通过按纽选择查询多少期
        else
        {
            if (num >= dt.Rows.Count)
            {
                num = dt.Rows.Count;
            }

            int ii = dt.Rows.Count;                           //总记录数
            int jj = ii - num;

            DataRow[] drr = dt.Select("id > " + jj + " and id <= " + ii);

            foreach (DataRow rr in drr)
            {
                dt1.Rows.Add(rr.ItemArray);
            }
        }

        return dt1;
    }

    //---------------------------------------------------------------------------------------------------------------------

    //2星标准综合走势图

    public DataTable SSC_2X_ZHFBZST_Select(int num, string str1, string str2, ref string result)
    {
        DataTable dt = null;
        DataTable dt1 = null;

        dt = Shove._Web.Cache.GetCacheAsDataTable("SSC_2X_ZHFBZST_Select");

        if (dt == null || dt.Rows.Count < 1)
        {
            DataSet ds = null;
            DAL.Procedures.P_TrendChart_SSC_2XZHFBZST(ref ds);

            if (ds == null || ds.Tables.Count < 1)
            {
                PF.GoError(ErrorNumber.Unknow, "参数错误或者数据库--无数据", this.GetType().BaseType.FullName);

                return dt1;
            }
            else
            {
                Shove._Web.Cache.SetCache("SSC_2X_ZHFBZST_Select", ds.Tables[0]);

                dt = ds.Tables[0];
            }
        }

        dt1 = dt.Clone();

        //通过期号来查询
        if (num == 0)
        {
            int i = Shove._Convert.StrToInt(str1, 0);
            int j = Shove._Convert.StrToInt(str2, 0);

            DataRow[] dr2 = dt.Select("Isuse>=" + i + " and  Isuse<=" + j);

            foreach (DataRow r in dr2)
            {
                dt1.Rows.Add(r.ItemArray);
            }
        }

        //通过按纽选择查询多少期
        else
        {
            if (num >= dt.Rows.Count)
            {
                num = dt.Rows.Count;
            }

            int ii = dt.Rows.Count;                           //总记录数
            int jj = ii - num;

            DataRow[] drr = dt.Select("id > " + jj + " and id <= " + ii);

            foreach (DataRow rr in drr)
            {
                dt1.Rows.Add(rr.ItemArray);
            }
        }

        return dt1;
    }

    //2星和值走势图
    public DataTable SSC_2X_HZZST_Select(int num, string str1, string str2, ref string result)
    {
        DataTable dt = null;
        DataTable dt1 = null;

        dt = Shove._Web.Cache.GetCacheAsDataTable("SSC_2X_HZZST_Select");

        if (dt == null || dt.Rows.Count < 1)
        {
            DataSet ds = null;
            DAL.Procedures.P_TrendChart_SSC_2XHZZST(ref ds);

            if (ds == null || ds.Tables.Count < 1)
            {
                PF.GoError(ErrorNumber.Unknow, "参数错误或者数据库--无数据", this.GetType().BaseType.FullName);

                return dt1;
            }
            else
            {
                Shove._Web.Cache.SetCache("SSC_2X_HZZST_Select", ds.Tables[0]);

                dt = ds.Tables[0];
            }
        }

        dt1 = dt.Clone();

        //通过期号来查询
        if (num == 0)
        {
            int i = Shove._Convert.StrToInt(str1, 0);
            int j = Shove._Convert.StrToInt(str2, 0);

            DataRow[] dr2 = dt.Select("Isuse>=" + i + " and  Isuse<=" + j);

            foreach (DataRow r in dr2)
            {
                dt1.Rows.Add(r.ItemArray);
            }
        }

        //通过按纽选择查询多少期
        else
        {
            if (num >= dt.Rows.Count)
            {
                num = dt.Rows.Count;
            }

            int ii = dt.Rows.Count;                           //总记录数
            int jj = ii - num;

            DataRow[] drr = dt.Select("id > " + jj + " and id <= " + ii);

            foreach (DataRow rr in drr)
            {
                dt1.Rows.Add(rr.ItemArray);
            }
        }

        return dt1;
    }

    //二星和尾走势图
    public DataTable SSC_2X_HZWZST_Select(int num, string str1, string str2, ref string result)
    {
        DataTable dt = null;
        DataTable dt1 = null;

        dt = Shove._Web.Cache.GetCacheAsDataTable("SSC_2X_HZWZST_Select");

        if (dt == null || dt.Rows.Count < 1)
        {
            DataSet ds = null;
            DAL.Procedures.P_TrendChart_SSC_2XHZWZST(ref ds);

            if (ds == null || ds.Tables.Count < 1)
            {
                PF.GoError(ErrorNumber.Unknow, "参数错误或者数据库--无数据", this.GetType().BaseType.FullName);

                return dt1;
            }
            else
            {
                Shove._Web.Cache.SetCache("SSC_2X_HZWZST_Select", ds.Tables[0]);

                dt = ds.Tables[0];
            }
        }

        dt1 = dt.Clone();

        //通过期号来查询
        if (num == 0)
        {
            int i = Shove._Convert.StrToInt(str1, 0);
            int j = Shove._Convert.StrToInt(str2, 0);

            DataRow[] dr2 = dt.Select("Isuse>=" + i + " and  Isuse<=" + j);

            foreach (DataRow r in dr2)
            {
                dt1.Rows.Add(r.ItemArray);
            }
        }

        //通过按纽选择查询多少期
        else
        {
            if (num >= dt.Rows.Count)
            {
                num = dt.Rows.Count;
            }

            int ii = dt.Rows.Count;                           //总记录数
            int jj = ii - num;

            DataRow[] drr = dt.Select("id > " + jj + " and id <= " + ii);

            foreach (DataRow rr in drr)
            {
                dt1.Rows.Add(rr.ItemArray);
            }
        }

        return dt1;
    }

    //二星均值走势图
    public DataTable SSC_2X_PJZZST_Select(int num, string str1, string str2, ref string result)
    {
        DataTable dt = null;
        DataTable dt1 = null;

        dt = Shove._Web.Cache.GetCacheAsDataTable("SSC_2X_PJZZST_Select");

        if (dt == null || dt.Rows.Count < 1)
        {
            DataSet ds = null;
            DAL.Procedures.P_TrendChart_SSC_2XPJZZST(ref ds);

            if (ds == null || ds.Tables.Count < 1)
            {
                PF.GoError(ErrorNumber.Unknow, "参数错误或者数据库--无数据", this.GetType().BaseType.FullName);

                return dt1;
            }
            else
            {
                Shove._Web.Cache.SetCache("SSC_2X_PJZZST_Select", ds.Tables[0]);

                dt = ds.Tables[0];
            }
        }

        dt1 = dt.Clone();

        //通过期号来查询
        if (num == 0)
        {
            int i = Shove._Convert.StrToInt(str1, 0);
            int j = Shove._Convert.StrToInt(str2, 0);

            DataRow[] dr2 = dt.Select("Isuse>=" + i + " and  Isuse<=" + j);

            foreach (DataRow r in dr2)
            {
                dt1.Rows.Add(r.ItemArray);
            }
        }

        //通过按纽选择查询多少期
        else
        {
            if (num >= dt.Rows.Count)
            {
                num = dt.Rows.Count;
            }

            int ii = dt.Rows.Count;                           //总记录数
            int jj = ii - num;

            DataRow[] drr = dt.Select("id > " + jj + " and id <= " + ii);

            foreach (DataRow rr in drr)
            {
                dt1.Rows.Add(rr.ItemArray);
            }
        }

        return dt1;
    }

    //二星跨度走势图
    public DataTable SSC_2X_KDZST_Select(int num, string str1, string str2, ref string result)
    {
        DataTable dt = null;
        DataTable dt1 = null;

        dt = Shove._Web.Cache.GetCacheAsDataTable("SSC_2X_KDZST_Select");

        if (dt == null || dt.Rows.Count < 1)
        {
            DataSet ds = null;
            DAL.Procedures.P_TrendChart_SSC_2XKDZST(ref ds);

            if (ds == null || ds.Tables.Count < 1)
            {
                PF.GoError(ErrorNumber.Unknow, "参数错误或者数据库--无数据", this.GetType().BaseType.FullName);

                return dt1;
            }
            else
            {
                Shove._Web.Cache.SetCache("SSC_2X_KDZST_Select", ds.Tables[0]);

                dt = ds.Tables[0];
            }
        }

        dt1 = dt.Clone();

        //通过期号来查询
        if (num == 0)
        {
            int i = Shove._Convert.StrToInt(str1, 0);
            int j = Shove._Convert.StrToInt(str2, 0);

            DataRow[] dr2 = dt.Select("Isuse>=" + i + " and  Isuse<=" + j);

            foreach (DataRow r in dr2)
            {
                dt1.Rows.Add(r.ItemArray);
            }
        }

        //通过按纽选择查询多少期
        else
        {
            if (num >= dt.Rows.Count)
            {
                num = dt.Rows.Count;
            }

            int ii = dt.Rows.Count;                           //总记录数
            int jj = ii - num;

            DataRow[] drr = dt.Select("id > " + jj + " and id <= " + ii);

            foreach (DataRow rr in drr)
            {
                dt1.Rows.Add(rr.ItemArray);
            }
        }

        return dt1;
    }

    //二星012路走势图
    public DataTable SSC_2X_012ZST_Select(int num, string str1, string str2, ref string result)
    {
        DataTable dt = null;
        DataTable dt1 = null;

        dt = Shove._Web.Cache.GetCacheAsDataTable("SSC_2X_012ZST_Select");

        if (dt == null || dt.Rows.Count < 1)
        {
            DataSet ds = null;
            DAL.Procedures.P_TrendChart_SSC_2X_012_ZST(ref ds);

            if (ds == null || ds.Tables.Count < 1)
            {
                PF.GoError(ErrorNumber.Unknow, "参数错误或者数据库--无数据", this.GetType().BaseType.FullName);

                return dt1;
            }
            else
            {
                Shove._Web.Cache.SetCache("SSC_2X_012ZST_Select", ds.Tables[0]);

                dt = ds.Tables[0];
            }
        }

        dt1 = dt.Clone();

        //通过期号来查询
        if (num == 0)
        {
            int i = Shove._Convert.StrToInt(str1, 0);
            int j = Shove._Convert.StrToInt(str2, 0);

            DataRow[] dr2 = dt.Select("Isuse>=" + i + " and  Isuse<=" + j);

            foreach (DataRow r in dr2)
            {
                dt1.Rows.Add(r.ItemArray);
            }
        }

        //通过按纽选择查询多少期
        else
        {
            if (num >= dt.Rows.Count)
            {
                num = dt.Rows.Count;
            }

            int ii = dt.Rows.Count;                           //总记录数
            int jj = ii - num;

            DataRow[] drr = dt.Select("id > " + jj + " and id <= " + ii);

            foreach (DataRow rr in drr)
            {
                dt1.Rows.Add(rr.ItemArray);
            }
        }

        return dt1;
    }

    //二星最大值走势图
    public DataTable SSC_2X_MAXZST_Select(int num, string str1, string str2, ref string result)
    {
        DataTable dt = null;
        DataTable dt1 = null;

        dt = Shove._Web.Cache.GetCacheAsDataTable("SSC_2X_MAXZST_Select");

        if (dt == null || dt.Rows.Count < 1)
        {
            DataSet ds = null;
            DAL.Procedures.P_TrendChart_SSC_2XMaxZST(ref ds);

            if (ds == null || ds.Tables.Count < 1)
            {
                PF.GoError(ErrorNumber.Unknow, "参数错误或者数据库--无数据", this.GetType().BaseType.FullName);

                return dt1;
            }
            else
            {
                Shove._Web.Cache.SetCache("SSC_2X_MAXZST_Select", ds.Tables[0]);

                dt = ds.Tables[0];
            }
        }

        dt1 = dt.Clone();

        //通过期号来查询
        if (num == 0)
        {
            int i = Shove._Convert.StrToInt(str1, 0);
            int j = Shove._Convert.StrToInt(str2, 0);

            DataRow[] dr2 = dt.Select("Isuse>=" + i + " and  Isuse<=" + j);

            foreach (DataRow r in dr2)
            {
                dt1.Rows.Add(r.ItemArray);
            }
        }

        //通过按纽选择查询多少期
        else
        {
            if (num >= dt.Rows.Count)
            {
                num = dt.Rows.Count;
            }

            int ii = dt.Rows.Count;                           //总记录数
            int jj = ii - num;

            DataRow[] drr = dt.Select("id > " + jj + " and id <= " + ii);

            foreach (DataRow rr in drr)
            {
                dt1.Rows.Add(rr.ItemArray);
            }
        }

        return dt1;
    }

    //二星最小值走势图
    public DataTable SSC_2X_MinZST_Select(int num, string str1, string str2, ref string result)
    {
        DataTable dt = null;
        DataTable dt1 = null;

        dt = Shove._Web.Cache.GetCacheAsDataTable("SSC_2X_MinZST_Select");

        if (dt == null || dt.Rows.Count < 1)
        {
            DataSet ds = null;
            DAL.Procedures.P_TrendChart_SSC_2XMINZST(ref ds);

            if (ds == null || ds.Tables.Count < 1)
            {
                PF.GoError(ErrorNumber.Unknow, "参数错误或者数据库--无数据", this.GetType().BaseType.FullName);

                return dt1;
            }
            else
            {
                Shove._Web.Cache.SetCache("SSC_2X_MinZST_Select", ds.Tables[0]);

                dt = ds.Tables[0];
            }
        }

        dt1 = dt.Clone();

        //通过期号来查询
        if (num == 0)
        {
            int i = Shove._Convert.StrToInt(str1, 0);
            int j = Shove._Convert.StrToInt(str2, 0);

            DataRow[] dr2 = dt.Select("Isuse>=" + i + " and  Isuse<=" + j);

            foreach (DataRow r in dr2)
            {
                dt1.Rows.Add(r.ItemArray);
            }
        }

        //通过按纽选择查询多少期
        else
        {
            if (num >= dt.Rows.Count)
            {
                num = dt.Rows.Count;
            }

            int ii = dt.Rows.Count;                           //总记录数
            int jj = ii - num;

            DataRow[] drr = dt.Select("id > " + jj + " and id <= " + ii);

            foreach (DataRow rr in drr)
            {
                dt1.Rows.Add(rr.ItemArray);
            }
        }

        return dt1;
    }

    //二星大小单双走势图
    public DataTable SSC_2X_DXDSZST_Select(int num, string str1, string str2, ref string result)
    {
        DataTable dt = null;
        DataTable dt1 = null;

        dt = Shove._Web.Cache.GetCacheAsDataTable("SSC_2X_DXDSZST_Select");

        if (dt == null || dt.Rows.Count < 1)
        {
            DataSet ds = null;
            DAL.Procedures.P_TrendChart_SSC_2XDXDSZST(ref ds);

            if (ds == null || ds.Tables.Count < 1)
            {
                PF.GoError(ErrorNumber.Unknow, "参数错误或者数据库--无数据", this.GetType().BaseType.FullName);

                return dt1;
            }
            else
            {
                Shove._Web.Cache.SetCache("SSC_2X_DXDSZST_Select", ds.Tables[0]);

                dt = ds.Tables[0];
            }
        }

        dt1 = dt.Clone();

        //通过期号来查询
        if (num == 0)
        {
            int i = Shove._Convert.StrToInt(str1, 0);
            int j = Shove._Convert.StrToInt(str2, 0);

            DataRow[] dr2 = dt.Select("Isuse>=" + i + " and  Isuse<=" + j);

            foreach (DataRow r in dr2)
            {
                dt1.Rows.Add(r.ItemArray);
            }
        }

        //通过按纽选择查询多少期
        else
        {
            if (num >= dt.Rows.Count)
            {
                num = dt.Rows.Count;
            }

            int ii = dt.Rows.Count;                           //总记录数
            int jj = ii - num;

            DataRow[] drr = dt.Select("id > " + jj + " and id <= " + ii);

            foreach (DataRow rr in drr)
            {
                dt1.Rows.Add(rr.ItemArray);
            }
        }

        return dt1;
    }

    #endregion

    #region 十一运夺金
    //=======分布走势
    public DataTable SYDJ_FBZS(int DaySpan, int Type, ref int ReturnValue, ref string returnDescription)
    {
        DataSet ds = null;
        DataTable dt = null;
        DataTable dtall = null;
        DateTime Date = System.DateTime.Now;

        dt = Shove._Web.Cache.GetCacheAsDataTable("SYDJ_FBZS");
        if (dt == null)
        {
            DAL.Procedures.P_TrendChart_11YDJ_WINNUM(ref ds, Date, 62, ref ReturnValue, ref returnDescription);

            if (ds == null)
            {
                return dt;
            }

            dt = ds.Tables[0];

            dt.Columns.Add("b1", typeof(int));
            dt.Columns.Add("b2", typeof(int));
            dt.Columns.Add("b3", typeof(int));
            dt.Columns.Add("b4", typeof(int));
            dt.Columns.Add("b5", typeof(int));
            dt.Columns.Add("b6", typeof(int));
            dt.Columns.Add("b7", typeof(int));
            dt.Columns.Add("b8", typeof(int));
            dt.Columns.Add("b9", typeof(int));
            dt.Columns.Add("b10", typeof(int));
            dt.Columns.Add("b11", typeof(int));

            dt.Columns.Add("jb0", typeof(int));
            dt.Columns.Add("jb1", typeof(int));
            dt.Columns.Add("jb2", typeof(int));
            dt.Columns.Add("jb3", typeof(int));
            dt.Columns.Add("jb4", typeof(int));
            dt.Columns.Add("jb5", typeof(int));

            dt.Columns.Add("xb0", typeof(int));
            dt.Columns.Add("xb1", typeof(int));
            dt.Columns.Add("xb2", typeof(int));
            dt.Columns.Add("xb3", typeof(int));
            dt.Columns.Add("xb4", typeof(int));
            dt.Columns.Add("xb5", typeof(int));

            dt.Columns.Add("zb0", typeof(int));
            dt.Columns.Add("zb1", typeof(int));
            dt.Columns.Add("zb2", typeof(int));
            dt.Columns.Add("zb3", typeof(int));
            dt.Columns.Add("zb4", typeof(int));
            dt.Columns.Add("zb5", typeof(int));



            for (int i = 0; i < dt.Rows.Count; i++)
            {
                for (int j = 8; j <= 18; j++)
                {
                    if (i == 0)
                    {
                        if ((j - 7) == Convert.ToInt32(dt.Rows[i][1]) || (j - 7) == Convert.ToInt32(dt.Rows[i][2]) || (j - 7) == Convert.ToInt32(dt.Rows[i][3]) || (j - 7) == Convert.ToInt32(dt.Rows[i][4]) || (j - 7) == Convert.ToInt32(dt.Rows[i][5]))
                        {
                            dt.Rows[i][j] = -1;
                        }
                        else
                        {
                            dt.Rows[i][j] = 1;
                        }
                    }
                    else
                    {
                        if ((j - 7) == Convert.ToInt32(dt.Rows[i][1]) || (j - 7) == Convert.ToInt32(dt.Rows[i][2]) || (j - 7) == Convert.ToInt32(dt.Rows[i][3]) || (j - 7) == Convert.ToInt32(dt.Rows[i][4]) || (j - 7) == Convert.ToInt32(dt.Rows[i][5]))
                        {

                            if (Convert.ToInt32(dt.Rows[i - 1][j]) > 0)
                            {
                                dt.Rows[i][j] = -1;
                            }
                            else
                            {
                                dt.Rows[i][j] = Convert.ToInt32(dt.Rows[i - 1][j]) - 1;
                            }
                        }
                        else
                        {
                            if (Convert.ToInt32(dt.Rows[i - 1][j]) > 0)
                            {
                                dt.Rows[i][j] = Convert.ToInt32(dt.Rows[i - 1][j]) + 1;
                            }
                            else
                            {
                                dt.Rows[i][j] = 1;
                            }

                        }
                    }
                }

                int frag = 0;
                int xfrag1 = 0;
                int zfrag2 = 0;
                if (i == 0)
                {
                    for (int m = 1; m <= 5; m++)
                    {
                        switch (dt.Rows[i][m].ToString())
                        {
                            case "01":
                                frag++;
                                xfrag1++;
                                zfrag2++;
                                break;
                            case "02":
                                xfrag1++;
                                zfrag2++;
                                break;
                            case "03":
                                frag++;
                                xfrag1++;
                                zfrag2++;
                                break;
                            case "04":
                                xfrag1++;
                                break;
                            case "05":
                                frag++;
                                zfrag2++;
                                xfrag1++;
                                break;
                            case "07":
                                frag++;
                                zfrag2++;
                                break;
                            case "09":
                                frag++;
                                break;
                            case "11":
                                frag++;
                                zfrag2++;
                                break;
                        }
                    }
                    for (int m = 0; m <= 5; m++)
                    {
                        if (m != frag)
                        {
                            dt.Rows[i][m + 19] = 1;
                        }
                        else
                        {
                            dt.Rows[i][m + 19] = -1;

                        }

                        if (m != xfrag1)
                        {
                            dt.Rows[i][m + 25] = 1;
                        }
                        else
                        {
                            dt.Rows[i][m + 25] = -1;
                        }

                        if (m != zfrag2)
                        {
                            dt.Rows[i][m + 31] = 1;
                        }
                        else
                        {
                            dt.Rows[i][m + 31] = -1;
                        }
                    }

                }
                else
                {
                    for (int m = 1; m <= 5; m++)
                    {
                        switch (dt.Rows[i][m].ToString())
                        {
                            case "01":
                                frag++;
                                xfrag1++;
                                zfrag2++;
                                break;
                            case "02":
                                xfrag1++;
                                zfrag2++;
                                break;
                            case "03":
                                frag++;
                                xfrag1++;
                                zfrag2++;
                                break;
                            case "04":
                                xfrag1++;
                                break;
                            case "05":
                                frag++;
                                zfrag2++;
                                xfrag1++;
                                break;
                            case "07":
                                frag++;
                                zfrag2++;
                                break;
                            case "09":
                                frag++;
                                break;
                            case "11":
                                frag++;
                                zfrag2++;
                                break;
                        }
                    }
                    for (int m = 0; m <= 5; m++)
                    {
                        if (m != frag)
                        {
                            if (Convert.ToInt32(dt.Rows[i - 1][m + 19]) <= 0)
                            {
                                dt.Rows[i][m + 19] = 1;
                            }
                            else
                            {
                                dt.Rows[i][m + 19] = Convert.ToInt32(dt.Rows[i - 1][m + 19]) + 1;
                            }

                        }
                        else
                        {
                            if (Convert.ToInt32(dt.Rows[i - 1][m + 19]) > 0)
                            {
                                dt.Rows[i][m + 19] = -1;
                            }
                            else
                            {
                                dt.Rows[i][m + 19] = Convert.ToInt32(dt.Rows[i - 1][m + 19]) - 1;
                            }

                        }
                        if (m != xfrag1)
                        {
                            if (Convert.ToInt32(dt.Rows[i - 1][m + 25]) <= 0)
                            {
                                dt.Rows[i][m + 25] = 1;
                            }
                            else
                            {
                                dt.Rows[i][m + 25] = Convert.ToInt32(dt.Rows[i - 1][m + 25]) + 1;
                            }

                        }
                        else
                        {
                            if (Convert.ToInt32(dt.Rows[i - 1][m + 25]) > 0)
                            {
                                dt.Rows[i][m + 25] = -1;
                            }
                            else
                            {
                                dt.Rows[i][m + 25] = Convert.ToInt32(dt.Rows[i - 1][m + 25]) - 1;
                            }
                        }
                        if (m != zfrag2)
                        {
                            if (Convert.ToInt32(dt.Rows[i - 1][m + 31]) <= 0)
                            {
                                dt.Rows[i][m + 31] = 1;
                            }
                            else
                            {
                                dt.Rows[i][m + 31] = Convert.ToInt32(dt.Rows[i - 1][m + 31]) + 1;
                            }


                        }
                        else
                        {
                            if (Convert.ToInt32(dt.Rows[i - 1][m + 31]) > 0)
                            {
                                dt.Rows[i][m + 31] = -1;
                            }
                            else
                            {
                                dt.Rows[i][m + 31] = Convert.ToInt32(dt.Rows[i - 1][m + 31]) - 1;
                            }
                        }
                    }
                }


            }
            Shove._Web.Cache.SetCache("SYDJ_FBZS", dt, 600);
        }
        dtall = dt.Clone();

        DataRow[] dr = null;
        if (Type == 1)
        {
            dr = dt.Select("DaySpan <=" + DaySpan, "ID ASC");
        }
        else
        {
            dr = dt.Select("DaySpan =" + DaySpan, "ID ASC");
        }
        foreach (DataRow d in dr)
        {
            dtall.Rows.Add(d.ItemArray);
        }

        dtall.Columns.Remove("ID");
        dtall.Columns.Remove("DaySpan");



        return dtall;
    }

    //========= 定位走势
    public DataTable SYDJ_DWZS(int DaySpan, int Type, int number, ref int ReturnValue, ref string returnDescription)
    {

        DataSet ds = null;
        DataTable dt = null;
        DataTable dtall = null;
        DateTime Date = System.DateTime.Now;


        dt = Shove._Web.Cache.GetCacheAsDataTable("SYDJ_DWZS");

        if (dt == null)
        {
            DAL.Procedures.P_TrendChart_11YDJ_WINNUM(ref ds, Date, 62, ref ReturnValue, ref returnDescription);

            if (ds == null)
            {
                return dt;
            }

            dt = ds.Tables[0];

            dt.Columns.Add("b1", typeof(int));
            dt.Columns.Add("b2", typeof(int));
            dt.Columns.Add("b3", typeof(int));
            dt.Columns.Add("b4", typeof(int));
            dt.Columns.Add("b5", typeof(int));
            dt.Columns.Add("b6", typeof(int));
            dt.Columns.Add("b7", typeof(int));
            dt.Columns.Add("b8", typeof(int));
            dt.Columns.Add("b9", typeof(int));
            dt.Columns.Add("b10", typeof(int));
            dt.Columns.Add("b11", typeof(int));

            dt.Columns.Add("sb0", typeof(int));
            dt.Columns.Add("sb1", typeof(int));
            dt.Columns.Add("sb2", typeof(int));
            dt.Columns.Add("sb3", typeof(int));
            dt.Columns.Add("sb4", typeof(int));
            dt.Columns.Add("sb5", typeof(int));

            dt.Columns.Add("yb0", typeof(int));
            dt.Columns.Add("yb1", typeof(int));
            dt.Columns.Add("yb2", typeof(int));

            dt.Columns.Add("cb0", typeof(int));
            dt.Columns.Add("cb1", typeof(int));
            dt.Columns.Add("cb2", typeof(int));
            dt.Columns.Add("cb3", typeof(int));

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                for (int j = 8; j <= 18; j++)
                {
                    if (i == 0)
                    {
                        if ((j - 7) == Convert.ToInt32(dt.Rows[i][number]))
                        {
                            dt.Rows[i][j] = -1;
                        }
                        else
                        {

                            dt.Rows[i][j] = 1;
                        }
                    }
                    else
                    {
                        if ((j - 7) == Convert.ToInt32(dt.Rows[i][number]))
                        {

                            if (Convert.ToInt32(dt.Rows[i - 1][j]) > 0)
                            {
                                dt.Rows[i][j] = -1;
                            }
                            else
                            {
                                dt.Rows[i][j] = Convert.ToInt32(dt.Rows[i - 1][j]) - 1;
                            }
                        }
                        else
                        {
                            if (Convert.ToInt32(dt.Rows[i - 1][j]) > 0)
                            {
                                dt.Rows[i][j] = Convert.ToInt32(dt.Rows[i - 1][j]) + 1;
                            }
                            else
                            {
                                dt.Rows[i][j] = 1;
                            }

                        }
                    }
                }

                if (i == 0)
                {
                    //===============数字特征
                    if (Convert.ToInt32(dt.Rows[i][number]) == 1 || Convert.ToInt32(dt.Rows[i][number]) == 3 || Convert.ToInt32(dt.Rows[i][number]) == 5)
                    {
                        dt.Rows[i][19] = -1;
                    }
                    else
                    {
                        dt.Rows[i][19] = 1;
                    }

                    if (Convert.ToInt32(dt.Rows[i][number]) == 2)
                    {
                        dt.Rows[i][20] = -1;
                    }
                    else
                    {
                        dt.Rows[i][20] = 1;
                    }

                    if (Convert.ToInt32(dt.Rows[i][number]) == 4)
                    {
                        dt.Rows[i][21] = -1;
                    }
                    else
                    {
                        dt.Rows[i][21] = 1;
                    }

                    if (Convert.ToInt32(dt.Rows[i][number]) == 7 || Convert.ToInt32(dt.Rows[i][number]) == 11)
                    {
                        dt.Rows[i][22] = -1;
                    }
                    else
                    {
                        dt.Rows[i][22] = 1;
                    }

                    if (Convert.ToInt32(dt.Rows[i][number]) == 9)
                    {
                        dt.Rows[i][23] = -1;
                    }
                    else
                    {
                        dt.Rows[i][23] = 1;
                    }

                    if (Convert.ToInt32(dt.Rows[i][number]) == 6 || Convert.ToInt32(dt.Rows[i][number]) == 8 || Convert.ToInt32(dt.Rows[i][number]) == 10)
                    {
                        dt.Rows[i][24] = -1;
                    }
                    else
                    {
                        dt.Rows[i][24] = 1;
                    }

                    //==============================除3
                    if (Convert.ToInt32(dt.Rows[i][number]) == 3 || Convert.ToInt32(dt.Rows[i][number]) == 6 || Convert.ToInt32(dt.Rows[i][number]) == 9)
                    {
                        dt.Rows[i][25] = -1;
                    }
                    else
                    {
                        dt.Rows[i][25] = 1;
                    }

                    if (Convert.ToInt32(dt.Rows[i][number]) == 1 || Convert.ToInt32(dt.Rows[i][number]) == 4 || Convert.ToInt32(dt.Rows[i][number]) == 7 || Convert.ToInt32(dt.Rows[i][number]) == 10)
                    {
                        dt.Rows[i][26] = -1;
                    }
                    else
                    {
                        dt.Rows[i][26] = 1;
                    }

                    if (Convert.ToInt32(dt.Rows[i][number]) == 2 || Convert.ToInt32(dt.Rows[i][number]) == 5 || Convert.ToInt32(dt.Rows[i][number]) == 8 || Convert.ToInt32(dt.Rows[i][number]) == 11)
                    {
                        dt.Rows[i][27] = -1;
                    }
                    else
                    {
                        dt.Rows[i][27] = 1;
                    }
                    //==============重邻间孤
                    dt.Rows[i][28] = 1;

                    dt.Rows[i][29] = 1;

                    dt.Rows[i][30] = 1;

                    dt.Rows[i][31] = 1;
                }
                else
                {
                    //===============数字特征
                    if (Convert.ToInt32(dt.Rows[i][number]) == 1 || Convert.ToInt32(dt.Rows[i][number]) == 3 || Convert.ToInt32(dt.Rows[i][number]) == 5)
                    {
                        if (Convert.ToInt32(dt.Rows[i - 1][19]) < 0)
                        {
                            dt.Rows[i][19] = Convert.ToInt32(dt.Rows[i - 1][19]) - 1;
                        }
                        else
                        {
                            dt.Rows[i][19] = -1;
                        }
                    }
                    else
                    {
                        if (Convert.ToInt32(dt.Rows[i - 1][19]) > 0)
                        {
                            dt.Rows[i][19] = Convert.ToInt32(dt.Rows[i - 1][19]) + 1;
                        }
                        else
                        {
                            dt.Rows[i][19] = 1;
                        }
                    }

                    if (Convert.ToInt32(dt.Rows[i][number]) == 2)
                    {
                        if (Convert.ToInt32(dt.Rows[i - 1][20]) < 0)
                        {
                            dt.Rows[i][20] = Convert.ToInt32(dt.Rows[i - 1][20]) - 1;
                        }
                        else
                        {
                            dt.Rows[i][20] = -1;
                        }

                    }
                    else
                    {
                        if (Convert.ToInt32(dt.Rows[i - 1][20]) > 0)
                        {
                            dt.Rows[i][20] = Convert.ToInt32(dt.Rows[i - 1][20]) + 1;
                        }
                        else
                        {
                            dt.Rows[i][20] = 1;
                        }

                    }

                    if (Convert.ToInt32(dt.Rows[i][number]) == 4)
                    {
                        if (Convert.ToInt32(dt.Rows[i - 1][21]) < 0)
                        {
                            dt.Rows[i][21] = Convert.ToInt32(dt.Rows[i - 1][21]) - 1;
                        }
                        else
                        {
                            dt.Rows[i][21] = -1;
                        }

                    }
                    else
                    {
                        if (Convert.ToInt32(dt.Rows[i - 1][21]) > 0)
                        {
                            dt.Rows[i][21] = Convert.ToInt32(dt.Rows[i - 1][21]) + 1;
                        }
                        else
                        {
                            dt.Rows[i][21] = 1;
                        }

                    }

                    if (Convert.ToInt32(dt.Rows[i][number]) == 7 || Convert.ToInt32(dt.Rows[i][number]) == 11)
                    {
                        if (Convert.ToInt32(dt.Rows[i - 1][22]) < 0)
                        {
                            dt.Rows[i][22] = Convert.ToInt32(dt.Rows[i - 1][22]) - 1;
                        }
                        else
                        {
                            dt.Rows[i][22] = -1;
                        }

                    }
                    else
                    {
                        if (Convert.ToInt32(dt.Rows[i - 1][22]) > 0)
                        {
                            dt.Rows[i][22] = Convert.ToInt32(dt.Rows[i - 1][22]) + 1;
                        }
                        else
                        {
                            dt.Rows[i][22] = 1;
                        }
                    }

                    if (Convert.ToInt32(dt.Rows[i][number]) == 9)
                    {
                        if (Convert.ToInt32(dt.Rows[i - 1][23]) < 0)
                        {
                            dt.Rows[i][23] = Convert.ToInt32(dt.Rows[i - 1][23]) - 1;
                        }
                        else
                        {
                            dt.Rows[i][23] = -1;
                        }

                    }
                    else
                    {
                        if (Convert.ToInt32(dt.Rows[i - 1][23]) > 0)
                        {
                            dt.Rows[i][23] = Convert.ToInt32(dt.Rows[i - 1][23]) + 1;
                        }
                        else
                        {
                            dt.Rows[i][23] = 1;
                        }

                    }

                    if (Convert.ToInt32(dt.Rows[i][number]) == 6 || Convert.ToInt32(dt.Rows[i][number]) == 8 || Convert.ToInt32(dt.Rows[i][number]) == 10)
                    {
                        if (Convert.ToInt32(dt.Rows[i - 1][24]) < 0)
                        {
                            dt.Rows[i][24] = Convert.ToInt32(dt.Rows[i - 1][24]) - 1;
                        }
                        else
                        {
                            dt.Rows[i][24] = -1;
                        }

                    }
                    else
                    {
                        if (Convert.ToInt32(dt.Rows[i - 1][24]) > 0)
                        {
                            dt.Rows[i][24] = Convert.ToInt32(dt.Rows[i - 1][24]) + 1;
                        }
                        else
                        {
                            dt.Rows[i][24] = 1;
                        }

                    }

                    //==============================除3
                    if (Convert.ToInt32(dt.Rows[i][number]) == 3 || Convert.ToInt32(dt.Rows[i][number]) == 6 || Convert.ToInt32(dt.Rows[i][number]) == 9)
                    {
                        if (Convert.ToInt32(dt.Rows[i - 1][25]) < 0)
                        {
                            dt.Rows[i][25] = Convert.ToInt32(dt.Rows[i - 1][25]) - 1;
                        }
                        else
                        {
                            dt.Rows[i][25] = -1;
                        }
                    }
                    else
                    {
                        if (Convert.ToInt32(dt.Rows[i - 1][25]) > 0)
                        {
                            dt.Rows[i][25] = Convert.ToInt32(dt.Rows[i - 1][25]) + 1;
                        }
                        else
                        {
                            dt.Rows[i][25] = 1;
                        }
                    }

                    if (Convert.ToInt32(dt.Rows[i][number]) == 1 || Convert.ToInt32(dt.Rows[i][number]) == 4 || Convert.ToInt32(dt.Rows[i][number]) == 7 || Convert.ToInt32(dt.Rows[i][number]) == 10)
                    {
                        if (Convert.ToInt32(dt.Rows[i - 1][26]) < 0)
                        {
                            dt.Rows[i][26] = Convert.ToInt32(dt.Rows[i - 1][26]) - 1;
                        }
                        else
                        {
                            dt.Rows[i][26] = -1;
                        }
                    }
                    else
                    {
                        if (Convert.ToInt32(dt.Rows[i - 1][26]) > 0)
                        {
                            dt.Rows[i][26] = Convert.ToInt32(dt.Rows[i - 1][26]) + 1;
                        }
                        else
                        {
                            dt.Rows[i][26] = 1;
                        }

                    }

                    if (Convert.ToInt32(dt.Rows[i][number]) == 2 || Convert.ToInt32(dt.Rows[i][number]) == 5 || Convert.ToInt32(dt.Rows[i][number]) == 8 || Convert.ToInt32(dt.Rows[i][number]) == 11)
                    {
                        if (Convert.ToInt32(dt.Rows[i - 1][27]) < 0)
                        {
                            dt.Rows[i][27] = Convert.ToInt32(dt.Rows[i - 1][27]) - 1;
                        }
                        else
                        {
                            dt.Rows[i][27] = -1;
                        }

                    }
                    else
                    {
                        if (Convert.ToInt32(dt.Rows[i - 1][27]) > 0)
                        {
                            dt.Rows[i][27] = Convert.ToInt32(dt.Rows[i - 1][27]) + 1;
                        }
                        else
                        {
                            dt.Rows[i][27] = 1;
                        }


                    }

                    //==============重邻间孤

                    if (Convert.ToInt32(dt.Rows[i][number]) == Convert.ToInt32(dt.Rows[i - 1][number]))
                    {
                        if (Convert.ToInt32(dt.Rows[i - 1][28]) < 0)
                        {
                            dt.Rows[i][28] = Convert.ToInt32(dt.Rows[i - 1][28]) - 1;
                        }
                        else
                        {
                            dt.Rows[i][28] = -1;
                        }
                    }
                    else
                    {
                        if (Convert.ToInt32(dt.Rows[i - 1][28]) > 0)
                        {
                            dt.Rows[i][28] = Convert.ToInt32(dt.Rows[i - 1][28]) + 1;
                        }
                        else
                        {
                            dt.Rows[i][28] = 1;
                        }
                    }

                    if (System.Math.Abs(Convert.ToInt32(dt.Rows[i][number]) - Convert.ToInt32(dt.Rows[i - 1][number])) == 1)
                    {
                        if (Convert.ToInt32(dt.Rows[i - 1][29]) < 0)
                        {
                            dt.Rows[i][29] = Convert.ToInt32(dt.Rows[i - 1][29]) - 1;
                        }
                        else
                        {
                            dt.Rows[i][29] = -1;
                        }
                    }
                    else
                    {
                        if (Convert.ToInt32(dt.Rows[i - 1][29]) > 0)
                        {
                            dt.Rows[i][29] = Convert.ToInt32(dt.Rows[i - 1][29]) + 1;
                        }
                        else
                        {
                            dt.Rows[i][29] = 1;
                        }
                    }

                    if (System.Math.Abs(Convert.ToInt32(dt.Rows[i][number]) - Convert.ToInt32(dt.Rows[i - 1][number])) == 2)
                    {
                        if (Convert.ToInt32(dt.Rows[i - 1][30]) < 0)
                        {
                            dt.Rows[i][30] = Convert.ToInt32(dt.Rows[i - 1][30]) - 1;
                        }
                        else
                        {
                            dt.Rows[i][30] = -1;
                        }
                    }
                    else
                    {
                        if (Convert.ToInt32(dt.Rows[i - 1][30]) > 0)
                        {
                            dt.Rows[i][30] = Convert.ToInt32(dt.Rows[i - 1][30]) + 1;
                        }
                        else
                        {
                            dt.Rows[i][30] = 1;
                        }
                    }

                    if (System.Math.Abs(Convert.ToInt32(dt.Rows[i][number]) - Convert.ToInt32(dt.Rows[i - 1][number])) > 2)
                    {
                        if (Convert.ToInt32(dt.Rows[i - 1][31]) < 0)
                        {
                            dt.Rows[i][31] = Convert.ToInt32(dt.Rows[i - 1][31]) - 1;
                        }
                        else
                        {
                            dt.Rows[i][31] = -1;
                        }
                    }
                    else
                    {
                        if (Convert.ToInt32(dt.Rows[i - 1][31]) > 0)
                        {
                            dt.Rows[i][31] = Convert.ToInt32(dt.Rows[i - 1][31]) + 1;
                        }
                        else
                        {
                            dt.Rows[i][31] = 1;
                        }
                    }


                }
            }
        }
        dtall = dt.Clone();

        DataRow[] dr = null;
        if (Type == 1)
        {
            dr = dt.Select("DaySpan <=" + DaySpan, "ID ASC");
        }
        else
        {
            dr = dt.Select("DaySpan =" + DaySpan, "ID ASC");
        }
        foreach (DataRow d in dr)
        {
            dtall.Rows.Add(d.ItemArray);
        }

        dtall.Columns.Remove("ID");
        dtall.Columns.Remove("DaySpan");



        return dtall;
    }

    //======= 和值分布
    public DataTable SYDJ_HZFB(int DaySpan, int Type, ref int ReturnValue, ref string returnDescription)
    {

        DataSet ds = null;
        DataTable dt = null;
        DataTable dtall = null;
        DateTime Date = System.DateTime.Now;

        dt = Shove._Web.Cache.GetCacheAsDataTable("SYDJ_HZFB");
        if (dt == null)
        {
            DAL.Procedures.P_TrendChart_11YDJ_WINNUM(ref ds, Date, 62, ref ReturnValue, ref returnDescription);

            if (ds == null)
            {
                return dt;
            }

            dt = ds.Tables[0];

            for (int i = 15; i <= 45; i++)
            {
                dt.Columns.Add("b" + i.ToString(), typeof(int));
            }

            for (int i = 0; i <= 9; i++)
            {
                dt.Columns.Add("b" + i.ToString(), typeof(int));
            }

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                for (int j = 15; j <= 45; j++)
                {
                    if (i == 0)
                    {
                        if (j == (Convert.ToInt32(dt.Rows[i][1]) + Convert.ToInt32(dt.Rows[i][2]) + Convert.ToInt32(dt.Rows[i][3]) + Convert.ToInt32(dt.Rows[i][4]) + Convert.ToInt32(dt.Rows[i][5])))
                        {
                            dt.Rows[i][j - 7] = -1;
                        }
                        else
                        {
                            dt.Rows[i][j - 7] = 1;
                        }
                    }
                    else
                    {
                        if (j == (Convert.ToInt32(dt.Rows[i][1]) + Convert.ToInt32(dt.Rows[i][2]) + Convert.ToInt32(dt.Rows[i][3]) + Convert.ToInt32(dt.Rows[i][4]) + Convert.ToInt32(dt.Rows[i][5])))
                        {
                            if (Convert.ToInt32(dt.Rows[i - 1][j - 7]) < 0)
                            {
                                dt.Rows[i][j - 7] = Convert.ToInt32(dt.Rows[i - 1][j - 7]) - 1;
                            }
                            else
                            {
                                dt.Rows[i][j - 7] = -1;
                            }
                        }
                        else
                        {
                            if (Convert.ToInt32(dt.Rows[i - 1][j - 7]) > 0)
                            {
                                dt.Rows[i][j - 7] = Convert.ToInt32(dt.Rows[i - 1][j - 7]) + 1;
                            }
                            else
                            {
                                dt.Rows[i][j - 7] = 1;
                            }

                        }
                    }
                }

                for (int j = 0; j < 10; j++)
                {
                    if (i == 0)
                    {
                        if (j.ToString() == Convert.ToString(Convert.ToInt32(dt.Rows[i][1]) + Convert.ToInt32(dt.Rows[i][2]) + Convert.ToInt32(dt.Rows[i][3]) + Convert.ToInt32(dt.Rows[i][4]) + Convert.ToInt32(dt.Rows[i][5])).Substring(1))
                        {
                            dt.Rows[i][j + 39] = -1;
                        }
                        else
                        {
                            dt.Rows[i][j + 39] = 1;
                        }
                    }
                    else
                    {
                        if (j.ToString() == Convert.ToString(Convert.ToInt32(dt.Rows[i][1]) + Convert.ToInt32(dt.Rows[i][2]) + Convert.ToInt32(dt.Rows[i][3]) + Convert.ToInt32(dt.Rows[i][4]) + Convert.ToInt32(dt.Rows[i][5])).Substring(1))
                        {
                            if (Convert.ToInt32(dt.Rows[i - 1][j + 39]) < 0)
                            {
                                dt.Rows[i][j + 39] = Convert.ToInt32(dt.Rows[i - 1][j + 39]) - 1;
                            }
                            else
                            {
                                dt.Rows[i][j + 39] = -1;
                            }
                        }
                        else
                        {
                            if (Convert.ToInt32(dt.Rows[i - 1][j + 39]) > 0)
                            {
                                dt.Rows[i][j + 39] = Convert.ToInt32(dt.Rows[i - 1][j + 39]) + 1;
                            }
                            else
                            {
                                dt.Rows[i][j + 39] = 1;
                            }
                        }
                    }
                }
            }
            Shove._Web.Cache.SetCache("SYDJ_HZFB", dt, 600);
        }
        dtall = dt.Clone();

        DataRow[] dr = null;
        if (Type == 1)
        {
            dr = dt.Select("DaySpan <=" + DaySpan, "ID ASC");
        }
        else
        {
            dr = dt.Select("DaySpan =" + DaySpan, "ID ASC");
        }
        foreach (DataRow d in dr)
        {
            dtall.Rows.Add(d.ItemArray);
        }

        dtall.Columns.Remove("ID");
        dtall.Columns.Remove("DaySpan");

        return dtall;
    }

    //=======  前二分布图
    public DataTable SYDJ_Q2FBT(int DaySpan, int Type, ref int ReturnValue, ref string returnDescription)
    {

        DataSet ds = null;
        DataTable dt = null;
        DataTable dtall = null;
        DateTime Date = System.DateTime.Now;

        dt = Shove._Web.Cache.GetCacheAsDataTable("SYDJ_Q2FBT");

        if (dt == null)
        {
            DAL.Procedures.P_TrendChart_11YDJ_WINNUM(ref ds, Date, 62, ref ReturnValue, ref returnDescription);

            if (ds == null)
            {
                return dt;
            }

            dt = ds.Tables[0];

            for (int i = 1; i <= 11; i++)
            {
                dt.Columns.Add("b" + i.ToString(), typeof(int));
            }

            for (int i = 1; i <= 11; i++)
            {
                dt.Columns.Add("bb" + i.ToString(), typeof(int));
            }

            for (int i = 1; i <= 11; i++)
            {
                dt.Columns.Add("bc" + i.ToString(), typeof(int));
            }

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (i == 0)
                {
                    for (int j = 0; j < 11; j++)
                    {
                        if (j == Convert.ToInt32(dt.Rows[i][1]) - 1 || j == Convert.ToInt32(dt.Rows[i][2]) - 1)
                        {
                            dt.Rows[i][j + 8] = -1;
                        }
                        else
                        {
                            dt.Rows[i][j + 8] = 1;
                        }
                    }

                    for (int j = 0; j < 11; j++)
                    {
                        if (j == Convert.ToInt32(dt.Rows[i][1]) - 1)
                        {
                            dt.Rows[i][j + 19] = -1;
                        }
                        else
                        {
                            dt.Rows[i][j + 19] = 1;
                        }
                    }

                    for (int j = 0; j < 11; j++)
                    {
                        if (j == Convert.ToInt32(dt.Rows[i][2]) - 1)
                        {
                            dt.Rows[i][j + 30] = -1;
                        }
                        else
                        {
                            dt.Rows[i][j + 30] = 1;
                        }
                    }
                }
                else
                {
                    for (int j = 0; j < 11; j++)
                    {
                        if (j == Convert.ToInt32(dt.Rows[i][1]) - 1 || j == Convert.ToInt32(dt.Rows[i][2]) - 1)
                        {
                            if (Convert.ToInt32(dt.Rows[i - 1][j + 8]) < 0)
                            {
                                dt.Rows[i][j + 8] = Convert.ToInt32(dt.Rows[i - 1][j + 8]) - 1;
                            }
                            else
                            {
                                dt.Rows[i][j + 8] = -1;
                            }
                        }
                        else
                        {
                            if (Convert.ToInt32(dt.Rows[i - 1][j + 8]) > 0)
                            {
                                dt.Rows[i][j + 8] = Convert.ToInt32(dt.Rows[i - 1][j + 8]) + 1;
                            }
                            else
                            {
                                dt.Rows[i][j + 8] = 1;
                            }

                        }
                    }

                    for (int j = 0; j < 11; j++)
                    {
                        if (j == Convert.ToInt32(dt.Rows[i][1]) - 1)
                        {
                            if (Convert.ToInt32(dt.Rows[i - 1][j + 19]) < 0)
                            {
                                dt.Rows[i][j + 19] = Convert.ToInt32(dt.Rows[i - 1][j + 19]) - 1;
                            }
                            else
                            {
                                dt.Rows[i][j + 19] = -1;
                            }
                        }
                        else
                        {
                            if (Convert.ToInt32(dt.Rows[i - 1][j + 19]) > 0)
                            {
                                dt.Rows[i][j + 19] = Convert.ToInt32(dt.Rows[i - 1][j + 19]) + 1;
                            }
                            else
                            {
                                dt.Rows[i][j + 19] = 1;
                            }

                        }
                    }

                    for (int j = 0; j < 11; j++)
                    {
                        if (j == Convert.ToInt32(dt.Rows[i][2]) - 1)
                        {
                            if (Convert.ToInt32(dt.Rows[i - 1][j + 30]) < 0)
                            {
                                dt.Rows[i][j + 30] = Convert.ToInt32(dt.Rows[i - 1][j + 30]) - 1;
                            }
                            else
                            {
                                dt.Rows[i][j + 30] = -1;
                            }

                        }
                        else
                        {
                            if (Convert.ToInt32(dt.Rows[i - 1][j + 30]) > 0)
                            {
                                dt.Rows[i][j + 30] = Convert.ToInt32(dt.Rows[i - 1][j + 30]) + 1;
                            }
                            else
                            {
                                dt.Rows[i][j + 30] = 1;
                            }
                        }
                    }
                }
            }
            Shove._Web.Cache.SetCache("SYDJ_Q2FBT", dt, 600);

        }

        dtall = dt.Clone();

        DataRow[] dr = null;
        if (Type == 1)
        {
            dr = dt.Select("DaySpan <=" + DaySpan, "ID ASC");
        }
        else
        {
            dr = dt.Select("DaySpan =" + DaySpan, "ID ASC");
        }
        foreach (DataRow d in dr)
        {
            dtall.Rows.Add(d.ItemArray);
        }

        dtall.Columns.Remove("ID");
        dtall.Columns.Remove("DaySpan");

        return dtall;
    }

    //=======   前二组选对应表
    public DataTable SYDJ_Q2ZXDYB(int DaySpan, int Type, ref int ReturnValue, ref string returnDescription)
    {

        DataSet ds = null;
        DataTable dt = null;
        DataTable dtall = null;
        DateTime Date = System.DateTime.Now;

        dt = Shove._Web.Cache.GetCacheAsDataTable("SYDJ_Q2ZXDYB");
        if (dt == null)
        {
            DAL.Procedures.P_TrendChart_11YDJ_WINNUM(ref ds, Date, 62, ref ReturnValue, ref returnDescription);

            if (ds == null)
            {
                return dt;
            }

            dt = ds.Tables[0];

            dt.Columns.Add("bh", typeof(int));

            for (int i = 3; i <= 21; i++)
            {
                dt.Columns.Add("b" + i.ToString(), typeof(int));
            }


            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i][8] = Convert.ToInt32(dt.Rows[i][1]) + Convert.ToInt32(dt.Rows[i][2]);

                if (i == 0)
                {
                    for (int j = 3; j <= 21; j++)
                    {
                        if (j == Convert.ToInt32(dt.Rows[i][1]) + Convert.ToInt32(dt.Rows[i][2]))
                        {
                            dt.Rows[i][j + 6] = -1;
                        }
                        else
                        {
                            dt.Rows[i][j + 6] = 1;
                        }
                    }
                }
                else
                {
                    for (int j = 3; j <= 21; j++)
                    {
                        if (j == Convert.ToInt32(dt.Rows[i][1]) + Convert.ToInt32(dt.Rows[i][2]))
                        {
                            if (Convert.ToInt32(dt.Rows[i - 1][j + 6]) < 0)
                            {
                                dt.Rows[i][j + 6] = Convert.ToInt32(dt.Rows[i - 1][j + 6]) - 1;
                            }
                            else
                            {
                                dt.Rows[i][j + 6] = -1;
                            }
                        }
                        else
                        {
                            if (Convert.ToInt32(dt.Rows[i - 1][j + 6]) > 0)
                            {
                                dt.Rows[i][j + 6] = Convert.ToInt32(dt.Rows[i - 1][j + 6]) + 1;
                            }
                            else
                            {
                                dt.Rows[i][j + 6] = 1;
                            }
                        }
                    }
                }
            }
            Shove._Web.Cache.SetCache("SYDJ_Q2ZXDYB", dt, 600);
        }

        dtall = dt.Clone();

        DataRow[] dr = null;
        if (Type == 1)
        {
            dr = dt.Select("DaySpan <=" + DaySpan, "ID ASC");
        }
        else
        {
            dr = dt.Select("DaySpan =" + DaySpan, "ID ASC");
        }
        foreach (DataRow d in dr)
        {
            dtall.Rows.Add(d.ItemArray);
        }

        dtall.Columns.Remove("ID");
        dtall.Columns.Remove("DaySpan");

        return dtall;
    }

    //=======   前二和值
    public DataTable SYDJ_Q2HZ(int DaySpan, int Type, ref int ReturnValue, ref string returnDescription)
    {

        DataSet ds = null;
        DataTable dt = null;
        DataTable dtall = null;
        DateTime Date = System.DateTime.Now;

        dt = Shove._Web.Cache.GetCacheAsDataTable("SYDJ_Q2HZ");
        if (dt == null)
        {
            DAL.Procedures.P_TrendChart_11YDJ_WINNUM(ref ds, Date, 62, ref ReturnValue, ref returnDescription);

            if (ds == null)
            {
                return dt;
            }

            dt = ds.Tables[0];

            for (int i = 3; i <= 21; i++)
            {
                dt.Columns.Add("b" + i.ToString(), typeof(int));
            }

            for (int i = 0; i <= 9; i++)
            {
                dt.Columns.Add("bb" + i.ToString(), typeof(int));
            }

            for (int i = 0; i <= 2; i++)
            {
                dt.Columns.Add("bc3" + i.ToString(), typeof(int));
            }

            for (int i = 0; i <= 3; i++)
            {
                dt.Columns.Add("bc4" + i.ToString(), typeof(int));
            }

            for (int i = 0; i <= 4; i++)
            {
                dt.Columns.Add("bc5" + i.ToString(), typeof(int));
            }

            dt.Columns.Add("bd1", typeof(int));
            dt.Columns.Add("bd2", typeof(int));
            dt.Columns.Add("bj1", typeof(int));
            dt.Columns.Add("bj2", typeof(int));

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (i == 0)
                {
                    for (int j = 3; j <= 21; j++)
                    {
                        if (j == Convert.ToInt32(dt.Rows[i][1]) + Convert.ToInt32(dt.Rows[i][2]))
                        {
                            dt.Rows[i][j + 5] = -1;
                        }
                        else
                        {
                            dt.Rows[i][j + 5] = 1;
                        }
                    }

                    for (int j = 0; j <= 9; j++)
                    {
                        if (j == (Convert.ToInt32(dt.Rows[i][1]) + Convert.ToInt32(dt.Rows[i][2])) % 10)
                        {
                            dt.Rows[i][j + 27] = -1;
                        }
                        else
                        {
                            dt.Rows[i][j + 27] = 1;
                        }
                    }

                    for (int j = 0; j < 3; j++)
                    {
                        if (j == (Convert.ToInt32(dt.Rows[i][1]) + Convert.ToInt32(dt.Rows[i][2])) % 3)
                        {
                            dt.Rows[i][j + 37] = -1;
                        }
                        else
                        {
                            dt.Rows[i][j + 37] = 1;
                        }
                    }

                    for (int j = 0; j < 4; j++)
                    {
                        if (j == (Convert.ToInt32(dt.Rows[i][1]) + Convert.ToInt32(dt.Rows[i][2])) % 4)
                        {
                            dt.Rows[i][j + 40] = -1;
                        }
                        else
                        {
                            dt.Rows[i][j + 40] = 1;
                        }
                    }

                    for (int j = 0; j < 5; j++)
                    {
                        if (j == (Convert.ToInt32(dt.Rows[i][1]) + Convert.ToInt32(dt.Rows[i][2])) % 5)
                        {
                            dt.Rows[i][j + 44] = -1;
                        }
                        else
                        {
                            dt.Rows[i][j + 44] = 1;
                        }
                    }

                    if ((Convert.ToInt32(dt.Rows[i][1]) + Convert.ToInt32(dt.Rows[i][2])) > 11)
                    {
                        dt.Rows[i][49] = -1;
                        dt.Rows[i][50] = 1;
                    }
                    else
                    {
                        dt.Rows[i][49] = 1;
                        dt.Rows[i][50] = -1;
                    }

                    if ((Convert.ToInt32(dt.Rows[i][1]) + Convert.ToInt32(dt.Rows[i][2])) % 2 == 1)
                    {
                        dt.Rows[i][51] = -1;
                        dt.Rows[i][52] = 1;
                    }
                    else
                    {
                        dt.Rows[i][51] = 1;
                        dt.Rows[i][52] = -1;
                    }
                }
                else
                {
                    for (int j = 3; j <= 21; j++)
                    {
                        if (j == Convert.ToInt32(dt.Rows[i][1]) + Convert.ToInt32(dt.Rows[i][2]))
                        {
                            if (Convert.ToInt32(dt.Rows[i - 1][j + 5]) > 0)
                            {
                                dt.Rows[i][j + 5] = -1;
                            }
                            else
                            {
                                dt.Rows[i][j + 5] = Convert.ToInt32(dt.Rows[i - 1][j + 5]) - 1;
                            }
                        }
                        else
                        {
                            if (Convert.ToInt32(dt.Rows[i - 1][j + 5]) < 0)
                            {
                                dt.Rows[i][j + 5] = 1;
                            }
                            else
                            {
                                dt.Rows[i][j + 5] = Convert.ToInt32(dt.Rows[i - 1][j + 5]) + 1;
                            }
                        }
                    }

                    for (int j = 0; j <= 9; j++)
                    {
                        if (j == (Convert.ToInt32(dt.Rows[i][1]) + Convert.ToInt32(dt.Rows[i][2])) % 10)
                        {
                            if (Convert.ToInt32(dt.Rows[i - 1][j + 27]) > 0)
                            {
                                dt.Rows[i][j + 27] = -1;
                            }
                            else
                            {
                                dt.Rows[i][j + 27] = Convert.ToInt32(dt.Rows[i - 1][j + 27]) - 1;
                            }
                        }
                        else
                        {
                            if (Convert.ToInt32(dt.Rows[i - 1][j + 27]) < 0)
                            {
                                dt.Rows[i][j + 27] = 1;
                            }
                            else
                            {
                                dt.Rows[i][j + 27] = Convert.ToInt32(dt.Rows[i - 1][j + 27]) + 1;
                            }
                        }
                    }

                    for (int j = 0; j < 3; j++)
                    {
                        if (j == (Convert.ToInt32(dt.Rows[i][1]) + Convert.ToInt32(dt.Rows[i][2])) % 3)
                        {
                            if (Convert.ToInt32(dt.Rows[i - 1][j + 37]) > 0)
                            {
                                dt.Rows[i][j + 37] = -1;
                            }
                            else
                            {
                                dt.Rows[i][j + 37] = Convert.ToInt32(dt.Rows[i - 1][j + 37]) - 1;
                            }

                        }
                        else
                        {
                            if (Convert.ToInt32(dt.Rows[i - 1][j + 37]) < 0)
                            {
                                dt.Rows[i][j + 37] = 1;
                            }
                            else
                            {
                                dt.Rows[i][j + 37] = Convert.ToInt32(dt.Rows[i - 1][j + 37]) + 1;
                            }

                        }
                    }

                    for (int j = 0; j < 4; j++)
                    {
                        if (j == (Convert.ToInt32(dt.Rows[i][1]) + Convert.ToInt32(dt.Rows[i][2])) % 4)
                        {
                            if (Convert.ToInt32(dt.Rows[i - 1][j + 40]) > 0)
                            {
                                dt.Rows[i][j + 40] = -1;
                            }
                            else
                            {
                                dt.Rows[i][j + 40] = Convert.ToInt32(dt.Rows[i - 1][j + 40]) - 1;
                            }

                        }
                        else
                        {
                            if (Convert.ToInt32(dt.Rows[i - 1][j + 40]) < 0)
                            {
                                dt.Rows[i][j + 40] = 1;
                            }
                            else
                            {
                                dt.Rows[i][j + 40] = Convert.ToInt32(dt.Rows[i - 1][j + 40]) + 1;
                            }

                        }
                    }

                    for (int j = 0; j < 5; j++)
                    {

                        if (j == (Convert.ToInt32(dt.Rows[i][1]) + Convert.ToInt32(dt.Rows[i][2])) % 5)
                        {
                            if (Convert.ToInt32(dt.Rows[i - 1][j + 44]) > 0)
                            {
                                dt.Rows[i][j + 44] = -1;
                            }
                            else
                            {
                                dt.Rows[i][j + 44] = Convert.ToInt32(dt.Rows[i - 1][j + 44]) - 1;
                            }

                        }
                        else
                        {
                            if (Convert.ToInt32(dt.Rows[i - 1][j + 44]) < 0)
                            {
                                dt.Rows[i][j + 44] = 1;
                            }
                            else
                            {
                                dt.Rows[i][j + 44] = Convert.ToInt32(dt.Rows[i - 1][j + 44]) + 1;
                            }
                        }
                    }

                    if ((Convert.ToInt32(dt.Rows[i][1]) + Convert.ToInt32(dt.Rows[i][2])) > 11)
                    {
                        if (Convert.ToInt32(dt.Rows[i - 1][49]) > 0)
                        {
                            dt.Rows[i][49] = -1;
                        }
                        else
                        {
                            dt.Rows[i][49] = Convert.ToInt32(dt.Rows[i - 1][49]) - 1;
                        }

                        if (Convert.ToInt32(dt.Rows[i - 1][50]) < 0)
                        {
                            dt.Rows[i][50] = 1;
                        }
                        else
                        {
                            dt.Rows[i][50] = Convert.ToInt32(dt.Rows[i - 1][50]) + 1;
                        }

                    }
                    else
                    {
                        if (Convert.ToInt32(dt.Rows[i - 1][49]) < 0)
                        {
                            dt.Rows[i][49] = 1;
                        }
                        else
                        {
                            dt.Rows[i][49] = Convert.ToInt32(dt.Rows[i - 1][49]) + 1;
                        }

                        if (Convert.ToInt32(dt.Rows[i - 1][50]) > 0)
                        {
                            dt.Rows[i][50] = -1;
                        }
                        else
                        {
                            dt.Rows[i][50] = Convert.ToInt32(dt.Rows[i - 1][50]) - 1;
                        }

                    }

                    if ((Convert.ToInt32(dt.Rows[i][1]) + Convert.ToInt32(dt.Rows[i][2])) % 2 == 1)
                    {
                        if (Convert.ToInt32(dt.Rows[i - 1][51]) > 0)
                        {
                            dt.Rows[i][51] = -1;
                        }
                        else
                        {
                            dt.Rows[i][51] = Convert.ToInt32(dt.Rows[i - 1][51]) - 1;
                        }

                        if (Convert.ToInt32(dt.Rows[i - 1][52]) < 0)
                        {
                            dt.Rows[i][52] = 1;
                        }
                        else
                        {
                            dt.Rows[i][52] = Convert.ToInt32(dt.Rows[i - 1][52]) + 1;
                        }

                    }
                    else
                    {
                        if (Convert.ToInt32(dt.Rows[i - 1][51]) < 0)
                        {
                            dt.Rows[i][51] = 1;
                        }
                        else
                        {
                            dt.Rows[i][51] = Convert.ToInt32(dt.Rows[i - 1][51]) + 1;
                        }

                        if (Convert.ToInt32(dt.Rows[i - 1][52]) > 0)
                        {
                            dt.Rows[i][52] = -1;
                        }
                        else
                        {
                            dt.Rows[i][52] = Convert.ToInt32(dt.Rows[i - 1][52]) - 1;
                        }
                    }
                }
            }
            Shove._Web.Cache.SetCache("SYDJ_Q2HZ", dt, 600);
        }

        dtall = dt.Clone();

        DataRow[] dr = null;
        if (Type == 1)
        {
            dr = dt.Select("DaySpan <=" + DaySpan, "ID ASC");
        }
        else
        {
            dr = dt.Select("DaySpan =" + DaySpan, "ID ASC");
        }
        foreach (DataRow d in dr)
        {
            dtall.Rows.Add(d.ItemArray);
        }

        dtall.Columns.Remove("ID");
        dtall.Columns.Remove("DaySpan");

        return dtall;
    }

    //=======   前三分位图 
    public DataTable SYDJ_Q3FWT(int DaySpan, int Type, ref int ReturnValue, ref string returnDescription)
    {

        DataSet ds = null;
        DataTable dt = null;
        DataTable dtall = null;
        DateTime Date = System.DateTime.Now;

        dt = Shove._Web.Cache.GetCacheAsDataTable("SYDJ_Q3FWT");

        if (dt == null)
        {

            DAL.Procedures.P_TrendChart_11YDJ_WINNUM(ref ds, Date, 62, ref ReturnValue, ref returnDescription);

            if (ds == null)
            {
                return dt;
            }

            dt = ds.Tables[0];


            for (int i = 1; i <= 11; i++)
            {
                dt.Columns.Add("b" + i.ToString(), typeof(int));
            }

            for (int i = 1; i <= 11; i++)
            {
                dt.Columns.Add("bb" + i.ToString(), typeof(int));
            }

            for (int i = 1; i <= 11; i++)
            {
                dt.Columns.Add("bc" + i.ToString(), typeof(int));
            }

            for (int i = 0; i <= 3; i++)
            {
                dt.Columns.Add("bd" + i.ToString(), typeof(int));
            }

            for (int i = 0; i <= 3; i++)
            {
                dt.Columns.Add("be" + i.ToString(), typeof(int));
            }


            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (i == 0)
                {
                    for (int j = 1; j <= 11; j++)
                    {
                        if (j == Convert.ToInt32(dt.Rows[i][1]))
                        {
                            dt.Rows[i][j + 7] = -1;
                        }
                        else
                        {
                            dt.Rows[i][j + 7] = 1;
                        }
                    }

                    for (int j = 1; j <= 11; j++)
                    {
                        if (j == Convert.ToInt32(dt.Rows[i][2]))
                        {
                            dt.Rows[i][j + 18] = -1;
                        }
                        else
                        {
                            dt.Rows[i][j + 18] = 1;
                        }
                    }

                    for (int j = 1; j <= 11; j++)
                    {
                        if (j == Convert.ToInt32(dt.Rows[i][3]))
                        {
                            dt.Rows[i][j + 29] = -1;
                        }
                        else
                        {
                            dt.Rows[i][j + 29] = 1;
                        }
                    }

                    int fragj = 0;
                    int fragd = 0;

                    for (int j = 1; j <= 3; j++)
                    {
                        if (Convert.ToInt32(dt.Rows[i][j]) % 2 == 1)
                        {
                            fragj++;
                        }
                        if (Convert.ToInt32(dt.Rows[i][j]) > 5)
                        {
                            fragd++;
                        }
                    }

                    for (int j = 0; j <= 3; j++)
                    {
                        if (j == fragj)
                        {
                            dt.Rows[i][j + 41] = -1;
                        }
                        else
                        {
                            dt.Rows[i][j + 41] = 1;
                        }
                    }

                    for (int j = 0; j <= 3; j++)
                    {
                        if (j == fragd)
                        {
                            dt.Rows[i][j + 45] = -1;
                        }
                        else
                        {
                            dt.Rows[i][j + 45] = 1;
                        }
                    }


                }
                else
                {
                    for (int j = 1; j <= 11; j++)
                    {
                        if (j == Convert.ToInt32(dt.Rows[i][1]))
                        {
                            if (Convert.ToInt32(dt.Rows[i - 1][j + 7]) < 0)
                            {
                                dt.Rows[i][j + 7] = Convert.ToInt32(dt.Rows[i - 1][j + 7]) - 1;
                            }
                            else
                            {
                                dt.Rows[i][j + 7] = -1;
                            }
                        }
                        else
                        {
                            if (Convert.ToInt32(dt.Rows[i - 1][j + 7]) > 0)
                            {
                                dt.Rows[i][j + 7] = Convert.ToInt32(dt.Rows[i - 1][j + 7]) + 1;
                            }
                            else
                            {
                                dt.Rows[i][j + 7] = 1;
                            }

                        }
                    }

                    for (int j = 1; j <= 11; j++)
                    {
                        if (j == Convert.ToInt32(dt.Rows[i][2]))
                        {
                            if (Convert.ToInt32(dt.Rows[i - 1][j + 18]) < 0)
                            {
                                dt.Rows[i][j + 18] = Convert.ToInt32(dt.Rows[i - 1][j + 18]) - 1;
                            }
                            else
                            {
                                dt.Rows[i][j + 18] = -1;
                            }

                        }
                        else
                        {
                            if (Convert.ToInt32(dt.Rows[i - 1][j + 18]) > 0)
                            {
                                dt.Rows[i][j + 18] = Convert.ToInt32(dt.Rows[i - 1][j + 18]) + 1;
                            }
                            else
                            {
                                dt.Rows[i][j + 18] = 1;
                            }
                        }
                    }

                    for (int j = 1; j <= 11; j++)
                    {
                        if (j == Convert.ToInt32(dt.Rows[i][3]))
                        {
                            if (Convert.ToInt32(dt.Rows[i - 1][j + 29]) < 0)
                            {
                                dt.Rows[i][j + 29] = Convert.ToInt32(dt.Rows[i - 1][j + 29]) - 1;
                            }
                            else
                            {
                                dt.Rows[i][j + 29] = -1;
                            }

                        }
                        else
                        {
                            if (Convert.ToInt32(dt.Rows[i - 1][j + 29]) > 0)
                            {
                                dt.Rows[i][j + 29] = Convert.ToInt32(dt.Rows[i - 1][j + 29]) + 1;
                            }
                            else
                            {
                                dt.Rows[i][j + 29] = 1;
                            }

                        }
                    }

                    int fragj = 0;
                    int fragd = 0;

                    for (int j = 1; j <= 3; j++)
                    {
                        if (Convert.ToInt32(dt.Rows[i][j]) % 2 == 1)
                        {
                            fragj++;
                        }
                        if (Convert.ToInt32(dt.Rows[i][j]) > 5)
                        {
                            fragd++;
                        }
                    }


                    for (int j = 0; j <= 3; j++)
                    {
                        if (j == fragj)
                        {
                            if (Convert.ToInt32(dt.Rows[i - 1][j + 41]) < 0)
                            {
                                dt.Rows[i][j + 41] = Convert.ToInt32(dt.Rows[i - 1][j + 41]) - 1;
                            }
                            else
                            {
                                dt.Rows[i][j + 41] = -1;
                            }
                        }
                        else
                        {
                            if (Convert.ToInt32(dt.Rows[i - 1][j + 41]) > 0)
                            {
                                dt.Rows[i][j + 41] = Convert.ToInt32(dt.Rows[i - 1][j + 41]) + 1;
                            }
                            else
                            {
                                dt.Rows[i][j + 41] = 1;
                            }
                        }
                    }

                    for (int j = 0; j <= 3; j++)
                    {
                        if (j == fragd)
                        {
                            if (Convert.ToInt32(dt.Rows[i - 1][j + 45]) < 0)
                            {
                                dt.Rows[i][j + 45] = Convert.ToInt32(dt.Rows[i - 1][j + 45]) - 1;
                            }
                            else
                            {
                                dt.Rows[i][j + 45] = -1;
                            }

                        }
                        else
                        {
                            if (Convert.ToInt32(dt.Rows[i - 1][j + 45]) > 0)
                            {
                                dt.Rows[i][j + 45] = Convert.ToInt32(dt.Rows[i - 1][j + 45]) + 1;
                            }
                            else
                            {
                                dt.Rows[i][j + 45] = 1;
                            }
                        }
                    }

                }
            }
            Shove._Web.Cache.SetCache("SYDJ_Q3FWT", dt, 600);
        }

        dtall = dt.Clone();

        DataRow[] dr = null;
        if (Type == 1)
        {
            dr = dt.Select("DaySpan <=" + DaySpan, "ID ASC");
        }
        else
        {
            dr = dt.Select("DaySpan =" + DaySpan, "ID ASC");
        }
        foreach (DataRow d in dr)
        {
            dtall.Rows.Add(d.ItemArray);
        }

        dtall.Columns.Remove("ID");
        dtall.Columns.Remove("DaySpan");

        return dtall;
    }

    //=======   前三分布图 
    public DataTable SYDJ_Q3FBT(int DaySpan, int Type, ref int ReturnValue, ref string returnDescription)
    {

        DataSet ds = null;
        DataTable dt = null;
        DataTable dtall = null;
        DateTime Date = System.DateTime.Now;

        dt = Shove._Web.Cache.GetCacheAsDataTable("SYDJ_Q3FBT");

        if (dt == null)
        {
            DAL.Procedures.P_TrendChart_11YDJ_WINNUM(ref ds, Date, 62, ref ReturnValue, ref returnDescription);

            if (ds == null)
            {
                return dt;
            }

            dt = ds.Tables[0];

            for (int i = 1; i <= 11; i++)
            {
                dt.Columns.Add("b" + i.ToString(), typeof(int));
            }

            for (int i = 1; i <= 8; i++)
            {
                dt.Columns.Add("bb" + i.ToString(), typeof(int));
            }

            for (int i = 1; i <= 8; i++)
            {
                dt.Columns.Add("bc" + i.ToString(), typeof(int));
            }

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (i == 0)
                {
                    for (int j = 1; j <= 11; j++)
                    {
                        if (j == Convert.ToInt32(dt.Rows[i][1]) || j == Convert.ToInt32(dt.Rows[i][2]) || j == Convert.ToInt32(dt.Rows[i][3]))
                        {
                            dt.Rows[i][j + 7] = -1;
                        }
                        else
                        {
                            dt.Rows[i][j + 7] = 1;
                        }
                    }

                    ArrayList a = new ArrayList();
                    a.Add("111");
                    a.Add("110");
                    a.Add("101");
                    a.Add("011");
                    a.Add("100");
                    a.Add("010");
                    a.Add("001");
                    a.Add("000");

                    for (int j = 1; j <= 8; j++)
                    {
                        if ((j - 1) == a.IndexOf(Convert.ToString(Convert.ToInt32(dt.Rows[i][1]) % 2) + Convert.ToString(Convert.ToInt32(dt.Rows[i][2]) % 2) + Convert.ToString(Convert.ToInt32(dt.Rows[i][3]) % 2)))
                        {
                            dt.Rows[i][j + 18] = -1;
                        }
                        else
                        {
                            dt.Rows[i][j + 18] = 1;
                        }
                    }

                    for (int j = 1; j <= 8; j++)
                    {
                        if ((8 - j) == a.IndexOf(Convert.ToString(Convert.ToInt32(dt.Rows[i][1]) > 5 ? "1" : "0") + Convert.ToString(Convert.ToInt32(dt.Rows[i][2]) > 5 ? "1" : "0") + Convert.ToString(Convert.ToInt32(dt.Rows[i][3]) > 5 ? "1" : "0")))
                        {
                            dt.Rows[i][j + 26] = -1;
                        }
                        else
                        {
                            dt.Rows[i][j + 26] = 1;
                        }
                    }

                }
                else
                {

                    for (int j = 1; j <= 11; j++)
                    {
                        if (j == Convert.ToInt32(dt.Rows[i][1]) || j == Convert.ToInt32(dt.Rows[i][2]) || j == Convert.ToInt32(dt.Rows[i][3]))
                        {
                            if (Convert.ToInt32(dt.Rows[i - 1][j + 7]) > 0)
                            {
                                dt.Rows[i][j + 7] = -1;
                            }
                            else
                            {
                                dt.Rows[i][j + 7] = Convert.ToInt32(dt.Rows[i - 1][j + 7]) - 1;
                            }
                        }
                        else
                        {
                            if (Convert.ToInt32(dt.Rows[i - 1][j + 7]) < 0)
                            {
                                dt.Rows[i][j + 7] = 1;
                            }
                            else
                            {
                                dt.Rows[i][j + 7] = Convert.ToInt32(dt.Rows[i - 1][j + 7]) + 1;
                            }

                        }
                    }

                    ArrayList a = new ArrayList();
                    a.Add("111");
                    a.Add("110");
                    a.Add("101");
                    a.Add("011");
                    a.Add("100");
                    a.Add("010");
                    a.Add("001");
                    a.Add("000");

                    for (int j = 1; j <= 8; j++)
                    {
                        if ((j - 1) == a.IndexOf(Convert.ToString(Convert.ToInt32(dt.Rows[i][1]) % 2) + Convert.ToString(Convert.ToInt32(dt.Rows[i][2]) % 2) + Convert.ToString(Convert.ToInt32(dt.Rows[i][3]) % 2)))
                        {
                            if (Convert.ToInt32(dt.Rows[i - 1][j + 18]) > 0)
                            {
                                dt.Rows[i][j + 18] = -1;
                            }
                            else
                            {
                                dt.Rows[i][j + 18] = Convert.ToInt32(dt.Rows[i - 1][j + 18]) - 1;
                            }

                        }
                        else
                        {
                            if (Convert.ToInt32(dt.Rows[i - 1][j + 18]) < 0)
                            {
                                dt.Rows[i][j + 18] = 1;
                            }
                            else
                            {
                                dt.Rows[i][j + 18] = Convert.ToInt32(dt.Rows[i - 1][j + 18]) + 1;
                            }

                        }
                    }

                    for (int j = 1; j <= 8; j++)
                    {
                        if ((8 - j) == a.IndexOf(Convert.ToString(Convert.ToInt32(dt.Rows[i][1]) > 5 ? "1" : "0") + Convert.ToString(Convert.ToInt32(dt.Rows[i][2]) > 5 ? "1" : "0") + Convert.ToString(Convert.ToInt32(dt.Rows[i][3]) > 5 ? "1" : "0")))
                        {
                            if (Convert.ToInt32(dt.Rows[i - 1][j + 26]) > 0)
                            {
                                dt.Rows[i][j + 26] = -1;
                            }
                            else
                            {
                                dt.Rows[i][j + 26] = Convert.ToInt32(dt.Rows[i - 1][j + 26]) - 1;
                            }
                        }
                        else
                        {
                            if (Convert.ToInt32(dt.Rows[i - 1][j + 26]) < 0)
                            {
                                dt.Rows[i][j + 26] = 1;
                            }
                            else
                            {
                                dt.Rows[i][j + 26] = Convert.ToInt32(dt.Rows[i - 1][j + 26]) + 1;
                            }

                        }
                    }

                }
            }
            Shove._Web.Cache.SetCache("SYDJ_Q3FBT", dt, 600);
        }
        dtall = dt.Clone();

        DataRow[] dr = null;
        if (Type == 1)
        {
            dr = dt.Select("DaySpan <=" + DaySpan, "ID ASC");
        }
        else
        {
            dr = dt.Select("DaySpan =" + DaySpan, "ID ASC");
        }
        foreach (DataRow d in dr)
        {
            dtall.Rows.Add(d.ItemArray);
        }

        dtall.Columns.Remove("ID");
        dtall.Columns.Remove("DaySpan");

        return dtall;
    }

    //=======   前三和值图 
    public DataTable SYDJ_Q3HZT(int DaySpan, int Type, ref int ReturnValue, ref string returnDescription)
    {

        DataSet ds = null;
        DataTable dt = null;
        DataTable dtall = null;
        DateTime Date = System.DateTime.Now;

        dt = Shove._Web.Cache.GetCacheAsDataTable("SYDJ_Q3HZT");

        if (dt == null)
        {

            DAL.Procedures.P_TrendChart_11YDJ_WINNUM(ref ds, Date, 62, ref ReturnValue, ref returnDescription);

            if (ds == null)
            {
                return dt;
            }

            dt = ds.Tables[0];


            for (int i = 6; i <= 30; i++)
            {
                dt.Columns.Add("b" + i.ToString(), typeof(int));
            }

            for (int i = 0; i <= 9; i++)
            {
                dt.Columns.Add("bb" + i.ToString(), typeof(int));
            }

            for (int i = 0; i <= 3; i++)
            {
                dt.Columns.Add("bc3" + i.ToString(), typeof(int));
            }

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (i == 0)
                {
                    for (int j = 6; j <= 30; j++)
                    {
                        if (j == Convert.ToInt32(dt.Rows[i][1]) + Convert.ToInt32(dt.Rows[i][2]) + Convert.ToInt32(dt.Rows[i][3]))
                        {
                            dt.Rows[i][j + 2] = -1;
                        }
                        else
                        {
                            dt.Rows[i][j + 2] = 1;
                        }
                    }

                    for (int j = 0; j <= 9; j++)
                    {
                        if (j == (Convert.ToInt32(dt.Rows[i][1]) + Convert.ToInt32(dt.Rows[i][2]) + Convert.ToInt32(dt.Rows[i][3])) % 10)
                        {
                            dt.Rows[i][j + 33] = -1;
                        }
                        else
                        {
                            dt.Rows[i][j + 33] = 1;
                        }
                    }

                    for (int j = 0; j < 3; j++)
                    {
                        if (j == (Convert.ToInt32(dt.Rows[i][1]) + Convert.ToInt32(dt.Rows[i][2]) + Convert.ToInt32(dt.Rows[i][3])) % 3)
                        {
                            dt.Rows[i][j + 43] = -1;
                        }
                        else
                        {
                            dt.Rows[i][j + 43] = 1;
                        }
                    }


                }
                else
                {
                    for (int j = 6; j <= 30; j++)
                    {
                        if (j == Convert.ToInt32(dt.Rows[i][1]) + Convert.ToInt32(dt.Rows[i][2]) + Convert.ToInt32(dt.Rows[i][3]))
                        {
                            if (Convert.ToInt32(dt.Rows[i - 1][j + 2]) > 0)
                            {
                                dt.Rows[i][j + 2] = -1;
                            }
                            else
                            {
                                dt.Rows[i][j + 2] = Convert.ToInt32(dt.Rows[i - 1][j + 2]) - 1;
                            }
                        }
                        else
                        {
                            if (Convert.ToInt32(dt.Rows[i - 1][j + 2]) < 0)
                            {
                                dt.Rows[i][j + 2] = 1;
                            }
                            else
                            {
                                dt.Rows[i][j + 2] = Convert.ToInt32(dt.Rows[i - 1][j + 2]) + 1;
                            }
                        }
                    }

                    for (int j = 0; j <= 9; j++)
                    {
                        if (j == (Convert.ToInt32(dt.Rows[i][1]) + Convert.ToInt32(dt.Rows[i][2]) + Convert.ToInt32(dt.Rows[i][3])) % 10)
                        {
                            if (Convert.ToInt32(dt.Rows[i - 1][j + 33]) > 0)
                            {
                                dt.Rows[i][j + 33] = -1;
                            }
                            else
                            {
                                dt.Rows[i][j + 33] = Convert.ToInt32(dt.Rows[i - 1][j + 33]) - 1;
                            }
                        }
                        else
                        {
                            if (Convert.ToInt32(dt.Rows[i - 1][j + 33]) < 0)
                            {
                                dt.Rows[i][j + 33] = 1;
                            }
                            else
                            {
                                dt.Rows[i][j + 33] = Convert.ToInt32(dt.Rows[i - 1][j + 33]) + 1;
                            }
                        }
                    }

                    for (int j = 0; j < 3; j++)
                    {
                        if (j == (Convert.ToInt32(dt.Rows[i][1]) + Convert.ToInt32(dt.Rows[i][2]) + Convert.ToInt32(dt.Rows[i][3])) % 3)
                        {
                            if (Convert.ToInt32(dt.Rows[i - 1][j + 43]) > 0)
                            {
                                dt.Rows[i][j + 43] = -1;
                            }
                            else
                            {
                                dt.Rows[i][j + 43] = Convert.ToInt32(dt.Rows[i - 1][j + 43]) - 1;
                            }

                        }
                        else
                        {
                            if (Convert.ToInt32(dt.Rows[i - 1][j + 43]) < 0)
                            {
                                dt.Rows[i][j + 43] = 1;
                            }
                            else
                            {
                                dt.Rows[i][j + 43] = Convert.ToInt32(dt.Rows[i - 1][j + 43]) + 1;
                            }

                        }
                    }

                }
            }
            Shove._Web.Cache.SetCache("SYDJ_Q3HZT", dt, 600);
        }
        dtall = dt.Clone();

        DataRow[] dr = null;
        if (Type == 1)
        {
            dr = dt.Select("DaySpan <=" + DaySpan, "ID ASC");
        }
        else
        {
            dr = dt.Select("DaySpan =" + DaySpan, "ID ASC");
        }
        foreach (DataRow d in dr)
        {
            dtall.Rows.Add(d.ItemArray);
        }

        dtall.Columns.Remove("ID");
        dtall.Columns.Remove("DaySpan");

        return dtall;
    }

    #endregion

    #region 河南11选5
    //=======分布走势
    public DataTable HN11X5_FBZS(int DaySpan, int Type, ref int ReturnValue, ref string returnDescription)
    {
        DataSet ds = null;
        DataTable dt = null;
        DataTable dtall = null;
        DateTime Date = System.DateTime.Now;

        dt = Shove._Web.Cache.GetCacheAsDataTable("HN11X5_FBZS");
        if (dt == null)
        {
            DAL.Procedures.P_TrendChart_11YDJ_WINNUM(ref ds, Date, 77, ref ReturnValue, ref returnDescription);

            if (ds == null)
            {
                return dt;
            }

            dt = ds.Tables[0];

            dt.Columns.Add("b1", typeof(int));
            dt.Columns.Add("b2", typeof(int));
            dt.Columns.Add("b3", typeof(int));
            dt.Columns.Add("b4", typeof(int));
            dt.Columns.Add("b5", typeof(int));
            dt.Columns.Add("b6", typeof(int));
            dt.Columns.Add("b7", typeof(int));
            dt.Columns.Add("b8", typeof(int));
            dt.Columns.Add("b9", typeof(int));
            dt.Columns.Add("b10", typeof(int));
            dt.Columns.Add("b11", typeof(int));

            dt.Columns.Add("jb0", typeof(int));
            dt.Columns.Add("jb1", typeof(int));
            dt.Columns.Add("jb2", typeof(int));
            dt.Columns.Add("jb3", typeof(int));
            dt.Columns.Add("jb4", typeof(int));
            dt.Columns.Add("jb5", typeof(int));

            dt.Columns.Add("xb0", typeof(int));
            dt.Columns.Add("xb1", typeof(int));
            dt.Columns.Add("xb2", typeof(int));
            dt.Columns.Add("xb3", typeof(int));
            dt.Columns.Add("xb4", typeof(int));
            dt.Columns.Add("xb5", typeof(int));

            dt.Columns.Add("zb0", typeof(int));
            dt.Columns.Add("zb1", typeof(int));
            dt.Columns.Add("zb2", typeof(int));
            dt.Columns.Add("zb3", typeof(int));
            dt.Columns.Add("zb4", typeof(int));
            dt.Columns.Add("zb5", typeof(int));



            for (int i = 0; i < dt.Rows.Count; i++)
            {
                for (int j = 8; j <= 18; j++)
                {
                    if (i == 0)
                    {
                        if ((j - 7) == Convert.ToInt32(dt.Rows[i][1]) || (j - 7) == Convert.ToInt32(dt.Rows[i][2]) || (j - 7) == Convert.ToInt32(dt.Rows[i][3]) || (j - 7) == Convert.ToInt32(dt.Rows[i][4]) || (j - 7) == Convert.ToInt32(dt.Rows[i][5]))
                        {
                            dt.Rows[i][j] = -1;
                        }
                        else
                        {
                            dt.Rows[i][j] = 1;
                        }
                    }
                    else
                    {
                        if ((j - 7) == Convert.ToInt32(dt.Rows[i][1]) || (j - 7) == Convert.ToInt32(dt.Rows[i][2]) || (j - 7) == Convert.ToInt32(dt.Rows[i][3]) || (j - 7) == Convert.ToInt32(dt.Rows[i][4]) || (j - 7) == Convert.ToInt32(dt.Rows[i][5]))
                        {

                            if (Convert.ToInt32(dt.Rows[i - 1][j]) > 0)
                            {
                                dt.Rows[i][j] = -1;
                            }
                            else
                            {
                                dt.Rows[i][j] = Convert.ToInt32(dt.Rows[i - 1][j]) - 1;
                            }
                        }
                        else
                        {
                            if (Convert.ToInt32(dt.Rows[i - 1][j]) > 0)
                            {
                                dt.Rows[i][j] = Convert.ToInt32(dt.Rows[i - 1][j]) + 1;
                            }
                            else
                            {
                                dt.Rows[i][j] = 1;
                            }

                        }
                    }
                }

                int frag = 0;
                int xfrag1 = 0;
                int zfrag2 = 0;
                if (i == 0)
                {
                    for (int m = 1; m <= 5; m++)
                    {
                        switch (dt.Rows[i][m].ToString())
                        {
                            case "01":
                                frag++;
                                xfrag1++;
                                zfrag2++;
                                break;
                            case "02":
                                xfrag1++;
                                zfrag2++;
                                break;
                            case "03":
                                frag++;
                                xfrag1++;
                                zfrag2++;
                                break;
                            case "04":
                                xfrag1++;
                                break;
                            case "05":
                                frag++;
                                zfrag2++;
                                xfrag1++;
                                break;
                            case "07":
                                frag++;
                                zfrag2++;
                                break;
                            case "09":
                                frag++;
                                break;
                            case "11":
                                frag++;
                                zfrag2++;
                                break;
                        }
                    }
                    for (int m = 0; m <= 5; m++)
                    {
                        if (m != frag)
                        {
                            dt.Rows[i][m + 19] = 1;
                        }
                        else
                        {
                            dt.Rows[i][m + 19] = -1;

                        }

                        if (m != xfrag1)
                        {
                            dt.Rows[i][m + 25] = 1;
                        }
                        else
                        {
                            dt.Rows[i][m + 25] = -1;
                        }

                        if (m != zfrag2)
                        {
                            dt.Rows[i][m + 31] = 1;
                        }
                        else
                        {
                            dt.Rows[i][m + 31] = -1;
                        }
                    }

                }
                else
                {
                    for (int m = 1; m <= 5; m++)
                    {
                        switch (dt.Rows[i][m].ToString())
                        {
                            case "01":
                                frag++;
                                xfrag1++;
                                zfrag2++;
                                break;
                            case "02":
                                xfrag1++;
                                zfrag2++;
                                break;
                            case "03":
                                frag++;
                                xfrag1++;
                                zfrag2++;
                                break;
                            case "04":
                                xfrag1++;
                                break;
                            case "05":
                                frag++;
                                zfrag2++;
                                xfrag1++;
                                break;
                            case "07":
                                frag++;
                                zfrag2++;
                                break;
                            case "09":
                                frag++;
                                break;
                            case "11":
                                frag++;
                                zfrag2++;
                                break;
                        }
                    }
                    for (int m = 0; m <= 5; m++)
                    {
                        if (m != frag)
                        {
                            if (Convert.ToInt32(dt.Rows[i - 1][m + 19]) <= 0)
                            {
                                dt.Rows[i][m + 19] = 1;
                            }
                            else
                            {
                                dt.Rows[i][m + 19] = Convert.ToInt32(dt.Rows[i - 1][m + 19]) + 1;
                            }

                        }
                        else
                        {
                            if (Convert.ToInt32(dt.Rows[i - 1][m + 19]) > 0)
                            {
                                dt.Rows[i][m + 19] = -1;
                            }
                            else
                            {
                                dt.Rows[i][m + 19] = Convert.ToInt32(dt.Rows[i - 1][m + 19]) - 1;
                            }

                        }
                        if (m != xfrag1)
                        {
                            if (Convert.ToInt32(dt.Rows[i - 1][m + 25]) <= 0)
                            {
                                dt.Rows[i][m + 25] = 1;
                            }
                            else
                            {
                                dt.Rows[i][m + 25] = Convert.ToInt32(dt.Rows[i - 1][m + 25]) + 1;
                            }

                        }
                        else
                        {
                            if (Convert.ToInt32(dt.Rows[i - 1][m + 25]) > 0)
                            {
                                dt.Rows[i][m + 25] = -1;
                            }
                            else
                            {
                                dt.Rows[i][m + 25] = Convert.ToInt32(dt.Rows[i - 1][m + 25]) - 1;
                            }
                        }
                        if (m != zfrag2)
                        {
                            if (Convert.ToInt32(dt.Rows[i - 1][m + 31]) <= 0)
                            {
                                dt.Rows[i][m + 31] = 1;
                            }
                            else
                            {
                                dt.Rows[i][m + 31] = Convert.ToInt32(dt.Rows[i - 1][m + 31]) + 1;
                            }


                        }
                        else
                        {
                            if (Convert.ToInt32(dt.Rows[i - 1][m + 31]) > 0)
                            {
                                dt.Rows[i][m + 31] = -1;
                            }
                            else
                            {
                                dt.Rows[i][m + 31] = Convert.ToInt32(dt.Rows[i - 1][m + 31]) - 1;
                            }
                        }
                    }
                }


            }
            Shove._Web.Cache.SetCache("HN11X5_FBZS", dt, 600);
        }
        dtall = dt.Clone();

        DataRow[] dr = null;
        if (Type == 1)
        {
            dr = dt.Select("DaySpan <=" + DaySpan, "ID ASC");
        }
        else
        {
            dr = dt.Select("DaySpan =" + DaySpan, "ID ASC");
        }
        foreach (DataRow d in dr)
        {
            dtall.Rows.Add(d.ItemArray);
        }

        dtall.Columns.Remove("ID");
        dtall.Columns.Remove("DaySpan");



        return dtall;
    }

    //========= 定位走势
    public DataTable HN11X5_DWZS(int DaySpan, int Type, int number, ref int ReturnValue, ref string returnDescription)
    {

        DataSet ds = null;
        DataTable dt = null;
        DataTable dtall = null;
        DateTime Date = System.DateTime.Now;


        dt = Shove._Web.Cache.GetCacheAsDataTable("HN11X5_DWZS");

        if (dt == null)
        {
            DAL.Procedures.P_TrendChart_11YDJ_WINNUM(ref ds, Date, 77, ref ReturnValue, ref returnDescription);

            if (ds == null)
            {
                return dt;
            }

            dt = ds.Tables[0];

            dt.Columns.Add("b1", typeof(int));
            dt.Columns.Add("b2", typeof(int));
            dt.Columns.Add("b3", typeof(int));
            dt.Columns.Add("b4", typeof(int));
            dt.Columns.Add("b5", typeof(int));
            dt.Columns.Add("b6", typeof(int));
            dt.Columns.Add("b7", typeof(int));
            dt.Columns.Add("b8", typeof(int));
            dt.Columns.Add("b9", typeof(int));
            dt.Columns.Add("b10", typeof(int));
            dt.Columns.Add("b11", typeof(int));

            dt.Columns.Add("sb0", typeof(int));
            dt.Columns.Add("sb1", typeof(int));
            dt.Columns.Add("sb2", typeof(int));
            dt.Columns.Add("sb3", typeof(int));
            dt.Columns.Add("sb4", typeof(int));
            dt.Columns.Add("sb5", typeof(int));

            dt.Columns.Add("yb0", typeof(int));
            dt.Columns.Add("yb1", typeof(int));
            dt.Columns.Add("yb2", typeof(int));

            dt.Columns.Add("cb0", typeof(int));
            dt.Columns.Add("cb1", typeof(int));
            dt.Columns.Add("cb2", typeof(int));
            dt.Columns.Add("cb3", typeof(int));

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                for (int j = 8; j <= 18; j++)
                {
                    if (i == 0)
                    {
                        if ((j - 7) == Convert.ToInt32(dt.Rows[i][number]))
                        {
                            dt.Rows[i][j] = -1;
                        }
                        else
                        {

                            dt.Rows[i][j] = 1;
                        }
                    }
                    else
                    {
                        if ((j - 7) == Convert.ToInt32(dt.Rows[i][number]))
                        {

                            if (Convert.ToInt32(dt.Rows[i - 1][j]) > 0)
                            {
                                dt.Rows[i][j] = -1;
                            }
                            else
                            {
                                dt.Rows[i][j] = Convert.ToInt32(dt.Rows[i - 1][j]) - 1;
                            }
                        }
                        else
                        {
                            if (Convert.ToInt32(dt.Rows[i - 1][j]) > 0)
                            {
                                dt.Rows[i][j] = Convert.ToInt32(dt.Rows[i - 1][j]) + 1;
                            }
                            else
                            {
                                dt.Rows[i][j] = 1;
                            }

                        }
                    }
                }

                if (i == 0)
                {
                    //===============数字特征
                    if (Convert.ToInt32(dt.Rows[i][number]) == 1 || Convert.ToInt32(dt.Rows[i][number]) == 3 || Convert.ToInt32(dt.Rows[i][number]) == 5)
                    {
                        dt.Rows[i][19] = -1;
                    }
                    else
                    {
                        dt.Rows[i][19] = 1;
                    }

                    if (Convert.ToInt32(dt.Rows[i][number]) == 2)
                    {
                        dt.Rows[i][20] = -1;
                    }
                    else
                    {
                        dt.Rows[i][20] = 1;
                    }

                    if (Convert.ToInt32(dt.Rows[i][number]) == 4)
                    {
                        dt.Rows[i][21] = -1;
                    }
                    else
                    {
                        dt.Rows[i][21] = 1;
                    }

                    if (Convert.ToInt32(dt.Rows[i][number]) == 7 || Convert.ToInt32(dt.Rows[i][number]) == 11)
                    {
                        dt.Rows[i][22] = -1;
                    }
                    else
                    {
                        dt.Rows[i][22] = 1;
                    }

                    if (Convert.ToInt32(dt.Rows[i][number]) == 9)
                    {
                        dt.Rows[i][23] = -1;
                    }
                    else
                    {
                        dt.Rows[i][23] = 1;
                    }

                    if (Convert.ToInt32(dt.Rows[i][number]) == 6 || Convert.ToInt32(dt.Rows[i][number]) == 8 || Convert.ToInt32(dt.Rows[i][number]) == 10)
                    {
                        dt.Rows[i][24] = -1;
                    }
                    else
                    {
                        dt.Rows[i][24] = 1;
                    }

                    //==============================除3
                    if (Convert.ToInt32(dt.Rows[i][number]) == 3 || Convert.ToInt32(dt.Rows[i][number]) == 6 || Convert.ToInt32(dt.Rows[i][number]) == 9)
                    {
                        dt.Rows[i][25] = -1;
                    }
                    else
                    {
                        dt.Rows[i][25] = 1;
                    }

                    if (Convert.ToInt32(dt.Rows[i][number]) == 1 || Convert.ToInt32(dt.Rows[i][number]) == 4 || Convert.ToInt32(dt.Rows[i][number]) == 7 || Convert.ToInt32(dt.Rows[i][number]) == 10)
                    {
                        dt.Rows[i][26] = -1;
                    }
                    else
                    {
                        dt.Rows[i][26] = 1;
                    }

                    if (Convert.ToInt32(dt.Rows[i][number]) == 2 || Convert.ToInt32(dt.Rows[i][number]) == 5 || Convert.ToInt32(dt.Rows[i][number]) == 8 || Convert.ToInt32(dt.Rows[i][number]) == 11)
                    {
                        dt.Rows[i][27] = -1;
                    }
                    else
                    {
                        dt.Rows[i][27] = 1;
                    }
                    //==============重邻间孤
                    dt.Rows[i][28] = 1;

                    dt.Rows[i][29] = 1;

                    dt.Rows[i][30] = 1;

                    dt.Rows[i][31] = 1;
                }
                else
                {
                    //===============数字特征
                    if (Convert.ToInt32(dt.Rows[i][number]) == 1 || Convert.ToInt32(dt.Rows[i][number]) == 3 || Convert.ToInt32(dt.Rows[i][number]) == 5)
                    {
                        if (Convert.ToInt32(dt.Rows[i - 1][19]) < 0)
                        {
                            dt.Rows[i][19] = Convert.ToInt32(dt.Rows[i - 1][19]) - 1;
                        }
                        else
                        {
                            dt.Rows[i][19] = -1;
                        }
                    }
                    else
                    {
                        if (Convert.ToInt32(dt.Rows[i - 1][19]) > 0)
                        {
                            dt.Rows[i][19] = Convert.ToInt32(dt.Rows[i - 1][19]) + 1;
                        }
                        else
                        {
                            dt.Rows[i][19] = 1;
                        }
                    }

                    if (Convert.ToInt32(dt.Rows[i][number]) == 2)
                    {
                        if (Convert.ToInt32(dt.Rows[i - 1][20]) < 0)
                        {
                            dt.Rows[i][20] = Convert.ToInt32(dt.Rows[i - 1][20]) - 1;
                        }
                        else
                        {
                            dt.Rows[i][20] = -1;
                        }

                    }
                    else
                    {
                        if (Convert.ToInt32(dt.Rows[i - 1][20]) > 0)
                        {
                            dt.Rows[i][20] = Convert.ToInt32(dt.Rows[i - 1][20]) + 1;
                        }
                        else
                        {
                            dt.Rows[i][20] = 1;
                        }

                    }

                    if (Convert.ToInt32(dt.Rows[i][number]) == 4)
                    {
                        if (Convert.ToInt32(dt.Rows[i - 1][21]) < 0)
                        {
                            dt.Rows[i][21] = Convert.ToInt32(dt.Rows[i - 1][21]) - 1;
                        }
                        else
                        {
                            dt.Rows[i][21] = -1;
                        }

                    }
                    else
                    {
                        if (Convert.ToInt32(dt.Rows[i - 1][21]) > 0)
                        {
                            dt.Rows[i][21] = Convert.ToInt32(dt.Rows[i - 1][21]) + 1;
                        }
                        else
                        {
                            dt.Rows[i][21] = 1;
                        }

                    }

                    if (Convert.ToInt32(dt.Rows[i][number]) == 7 || Convert.ToInt32(dt.Rows[i][number]) == 11)
                    {
                        if (Convert.ToInt32(dt.Rows[i - 1][22]) < 0)
                        {
                            dt.Rows[i][22] = Convert.ToInt32(dt.Rows[i - 1][22]) - 1;
                        }
                        else
                        {
                            dt.Rows[i][22] = -1;
                        }

                    }
                    else
                    {
                        if (Convert.ToInt32(dt.Rows[i - 1][22]) > 0)
                        {
                            dt.Rows[i][22] = Convert.ToInt32(dt.Rows[i - 1][22]) + 1;
                        }
                        else
                        {
                            dt.Rows[i][22] = 1;
                        }
                    }

                    if (Convert.ToInt32(dt.Rows[i][number]) == 9)
                    {
                        if (Convert.ToInt32(dt.Rows[i - 1][23]) < 0)
                        {
                            dt.Rows[i][23] = Convert.ToInt32(dt.Rows[i - 1][23]) - 1;
                        }
                        else
                        {
                            dt.Rows[i][23] = -1;
                        }

                    }
                    else
                    {
                        if (Convert.ToInt32(dt.Rows[i - 1][23]) > 0)
                        {
                            dt.Rows[i][23] = Convert.ToInt32(dt.Rows[i - 1][23]) + 1;
                        }
                        else
                        {
                            dt.Rows[i][23] = 1;
                        }

                    }

                    if (Convert.ToInt32(dt.Rows[i][number]) == 6 || Convert.ToInt32(dt.Rows[i][number]) == 8 || Convert.ToInt32(dt.Rows[i][number]) == 10)
                    {
                        if (Convert.ToInt32(dt.Rows[i - 1][24]) < 0)
                        {
                            dt.Rows[i][24] = Convert.ToInt32(dt.Rows[i - 1][24]) - 1;
                        }
                        else
                        {
                            dt.Rows[i][24] = -1;
                        }

                    }
                    else
                    {
                        if (Convert.ToInt32(dt.Rows[i - 1][24]) > 0)
                        {
                            dt.Rows[i][24] = Convert.ToInt32(dt.Rows[i - 1][24]) + 1;
                        }
                        else
                        {
                            dt.Rows[i][24] = 1;
                        }

                    }

                    //==============================除3
                    if (Convert.ToInt32(dt.Rows[i][number]) == 3 || Convert.ToInt32(dt.Rows[i][number]) == 6 || Convert.ToInt32(dt.Rows[i][number]) == 9)
                    {
                        if (Convert.ToInt32(dt.Rows[i - 1][25]) < 0)
                        {
                            dt.Rows[i][25] = Convert.ToInt32(dt.Rows[i - 1][25]) - 1;
                        }
                        else
                        {
                            dt.Rows[i][25] = -1;
                        }
                    }
                    else
                    {
                        if (Convert.ToInt32(dt.Rows[i - 1][25]) > 0)
                        {
                            dt.Rows[i][25] = Convert.ToInt32(dt.Rows[i - 1][25]) + 1;
                        }
                        else
                        {
                            dt.Rows[i][25] = 1;
                        }
                    }

                    if (Convert.ToInt32(dt.Rows[i][number]) == 1 || Convert.ToInt32(dt.Rows[i][number]) == 4 || Convert.ToInt32(dt.Rows[i][number]) == 7 || Convert.ToInt32(dt.Rows[i][number]) == 10)
                    {
                        if (Convert.ToInt32(dt.Rows[i - 1][26]) < 0)
                        {
                            dt.Rows[i][26] = Convert.ToInt32(dt.Rows[i - 1][26]) - 1;
                        }
                        else
                        {
                            dt.Rows[i][26] = -1;
                        }
                    }
                    else
                    {
                        if (Convert.ToInt32(dt.Rows[i - 1][26]) > 0)
                        {
                            dt.Rows[i][26] = Convert.ToInt32(dt.Rows[i - 1][26]) + 1;
                        }
                        else
                        {
                            dt.Rows[i][26] = 1;
                        }

                    }

                    if (Convert.ToInt32(dt.Rows[i][number]) == 2 || Convert.ToInt32(dt.Rows[i][number]) == 5 || Convert.ToInt32(dt.Rows[i][number]) == 8 || Convert.ToInt32(dt.Rows[i][number]) == 11)
                    {
                        if (Convert.ToInt32(dt.Rows[i - 1][27]) < 0)
                        {
                            dt.Rows[i][27] = Convert.ToInt32(dt.Rows[i - 1][27]) - 1;
                        }
                        else
                        {
                            dt.Rows[i][27] = -1;
                        }

                    }
                    else
                    {
                        if (Convert.ToInt32(dt.Rows[i - 1][27]) > 0)
                        {
                            dt.Rows[i][27] = Convert.ToInt32(dt.Rows[i - 1][27]) + 1;
                        }
                        else
                        {
                            dt.Rows[i][27] = 1;
                        }


                    }

                    //==============重邻间孤

                    if (Convert.ToInt32(dt.Rows[i][number]) == Convert.ToInt32(dt.Rows[i - 1][number]))
                    {
                        if (Convert.ToInt32(dt.Rows[i - 1][28]) < 0)
                        {
                            dt.Rows[i][28] = Convert.ToInt32(dt.Rows[i - 1][28]) - 1;
                        }
                        else
                        {
                            dt.Rows[i][28] = -1;
                        }
                    }
                    else
                    {
                        if (Convert.ToInt32(dt.Rows[i - 1][28]) > 0)
                        {
                            dt.Rows[i][28] = Convert.ToInt32(dt.Rows[i - 1][28]) + 1;
                        }
                        else
                        {
                            dt.Rows[i][28] = 1;
                        }
                    }

                    if (System.Math.Abs(Convert.ToInt32(dt.Rows[i][number]) - Convert.ToInt32(dt.Rows[i - 1][number])) == 1)
                    {
                        if (Convert.ToInt32(dt.Rows[i - 1][29]) < 0)
                        {
                            dt.Rows[i][29] = Convert.ToInt32(dt.Rows[i - 1][29]) - 1;
                        }
                        else
                        {
                            dt.Rows[i][29] = -1;
                        }
                    }
                    else
                    {
                        if (Convert.ToInt32(dt.Rows[i - 1][29]) > 0)
                        {
                            dt.Rows[i][29] = Convert.ToInt32(dt.Rows[i - 1][29]) + 1;
                        }
                        else
                        {
                            dt.Rows[i][29] = 1;
                        }
                    }

                    if (System.Math.Abs(Convert.ToInt32(dt.Rows[i][number]) - Convert.ToInt32(dt.Rows[i - 1][number])) == 2)
                    {
                        if (Convert.ToInt32(dt.Rows[i - 1][30]) < 0)
                        {
                            dt.Rows[i][30] = Convert.ToInt32(dt.Rows[i - 1][30]) - 1;
                        }
                        else
                        {
                            dt.Rows[i][30] = -1;
                        }
                    }
                    else
                    {
                        if (Convert.ToInt32(dt.Rows[i - 1][30]) > 0)
                        {
                            dt.Rows[i][30] = Convert.ToInt32(dt.Rows[i - 1][30]) + 1;
                        }
                        else
                        {
                            dt.Rows[i][30] = 1;
                        }
                    }

                    if (System.Math.Abs(Convert.ToInt32(dt.Rows[i][number]) - Convert.ToInt32(dt.Rows[i - 1][number])) > 2)
                    {
                        if (Convert.ToInt32(dt.Rows[i - 1][31]) < 0)
                        {
                            dt.Rows[i][31] = Convert.ToInt32(dt.Rows[i - 1][31]) - 1;
                        }
                        else
                        {
                            dt.Rows[i][31] = -1;
                        }
                    }
                    else
                    {
                        if (Convert.ToInt32(dt.Rows[i - 1][31]) > 0)
                        {
                            dt.Rows[i][31] = Convert.ToInt32(dt.Rows[i - 1][31]) + 1;
                        }
                        else
                        {
                            dt.Rows[i][31] = 1;
                        }
                    }


                }
            }
        }
        dtall = dt.Clone();

        DataRow[] dr = null;
        if (Type == 1)
        {
            dr = dt.Select("DaySpan <=" + DaySpan, "ID ASC");
        }
        else
        {
            dr = dt.Select("DaySpan =" + DaySpan, "ID ASC");
        }
        foreach (DataRow d in dr)
        {
            dtall.Rows.Add(d.ItemArray);
        }

        dtall.Columns.Remove("ID");
        dtall.Columns.Remove("DaySpan");



        return dtall;
    }

    //======= 和值分布
    public DataTable HN11X5_HZFB(int DaySpan, int Type, ref int ReturnValue, ref string returnDescription)
    {

        DataSet ds = null;
        DataTable dt = null;
        DataTable dtall = null;
        DateTime Date = System.DateTime.Now;

        dt = Shove._Web.Cache.GetCacheAsDataTable("HN11X5_HZFB");
        if (dt == null)
        {
            DAL.Procedures.P_TrendChart_11YDJ_WINNUM(ref ds, Date, 77, ref ReturnValue, ref returnDescription);

            if (ds == null)
            {
                return dt;
            }

            dt = ds.Tables[0];

            for (int i = 15; i <= 45; i++)
            {
                dt.Columns.Add("b" + i.ToString(), typeof(int));
            }

            for (int i = 0; i <= 9; i++)
            {
                dt.Columns.Add("b" + i.ToString(), typeof(int));
            }

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                for (int j = 15; j <= 45; j++)
                {
                    if (i == 0)
                    {
                        if (j == (Convert.ToInt32(dt.Rows[i][1]) + Convert.ToInt32(dt.Rows[i][2]) + Convert.ToInt32(dt.Rows[i][3]) + Convert.ToInt32(dt.Rows[i][4]) + Convert.ToInt32(dt.Rows[i][5])))
                        {
                            dt.Rows[i][j - 7] = -1;
                        }
                        else
                        {
                            dt.Rows[i][j - 7] = 1;
                        }
                    }
                    else
                    {
                        if (j == (Convert.ToInt32(dt.Rows[i][1]) + Convert.ToInt32(dt.Rows[i][2]) + Convert.ToInt32(dt.Rows[i][3]) + Convert.ToInt32(dt.Rows[i][4]) + Convert.ToInt32(dt.Rows[i][5])))
                        {
                            if (Convert.ToInt32(dt.Rows[i - 1][j - 7]) < 0)
                            {
                                dt.Rows[i][j - 7] = Convert.ToInt32(dt.Rows[i - 1][j - 7]) - 1;
                            }
                            else
                            {
                                dt.Rows[i][j - 7] = -1;
                            }
                        }
                        else
                        {
                            if (Convert.ToInt32(dt.Rows[i - 1][j - 7]) > 0)
                            {
                                dt.Rows[i][j - 7] = Convert.ToInt32(dt.Rows[i - 1][j - 7]) + 1;
                            }
                            else
                            {
                                dt.Rows[i][j - 7] = 1;
                            }

                        }
                    }
                }

                for (int j = 0; j < 10; j++)
                {
                    if (i == 0)
                    {
                        if (j.ToString() == Convert.ToString(Convert.ToInt32(dt.Rows[i][1]) + Convert.ToInt32(dt.Rows[i][2]) + Convert.ToInt32(dt.Rows[i][3]) + Convert.ToInt32(dt.Rows[i][4]) + Convert.ToInt32(dt.Rows[i][5])).Substring(1))
                        {
                            dt.Rows[i][j + 39] = -1;
                        }
                        else
                        {
                            dt.Rows[i][j + 39] = 1;
                        }
                    }
                    else
                    {
                        if (j.ToString() == Convert.ToString(Convert.ToInt32(dt.Rows[i][1]) + Convert.ToInt32(dt.Rows[i][2]) + Convert.ToInt32(dt.Rows[i][3]) + Convert.ToInt32(dt.Rows[i][4]) + Convert.ToInt32(dt.Rows[i][5])).Substring(1))
                        {
                            if (Convert.ToInt32(dt.Rows[i - 1][j + 39]) < 0)
                            {
                                dt.Rows[i][j + 39] = Convert.ToInt32(dt.Rows[i - 1][j + 39]) - 1;
                            }
                            else
                            {
                                dt.Rows[i][j + 39] = -1;
                            }
                        }
                        else
                        {
                            if (Convert.ToInt32(dt.Rows[i - 1][j + 39]) > 0)
                            {
                                dt.Rows[i][j + 39] = Convert.ToInt32(dt.Rows[i - 1][j + 39]) + 1;
                            }
                            else
                            {
                                dt.Rows[i][j + 39] = 1;
                            }
                        }
                    }
                }
            }
            Shove._Web.Cache.SetCache("HN11X5_HZFB", dt, 600);
        }
        dtall = dt.Clone();

        DataRow[] dr = null;
        if (Type == 1)
        {
            dr = dt.Select("DaySpan <=" + DaySpan, "ID ASC");
        }
        else
        {
            dr = dt.Select("DaySpan =" + DaySpan, "ID ASC");
        }
        foreach (DataRow d in dr)
        {
            dtall.Rows.Add(d.ItemArray);
        }

        dtall.Columns.Remove("ID");
        dtall.Columns.Remove("DaySpan");

        return dtall;
    }

    //=======  前二分布图
    public DataTable HN11X5_Q2FBT(int DaySpan, int Type, ref int ReturnValue, ref string returnDescription)
    {

        DataSet ds = null;
        DataTable dt = null;
        DataTable dtall = null;
        DateTime Date = System.DateTime.Now;

        dt = Shove._Web.Cache.GetCacheAsDataTable("HN11X5_Q2FBT");

        if (dt == null)
        {
            DAL.Procedures.P_TrendChart_11YDJ_WINNUM(ref ds, Date, 77, ref ReturnValue, ref returnDescription);

            if (ds == null)
            {
                return dt;
            }

            dt = ds.Tables[0];

            for (int i = 1; i <= 11; i++)
            {
                dt.Columns.Add("b" + i.ToString(), typeof(int));
            }

            for (int i = 1; i <= 11; i++)
            {
                dt.Columns.Add("bb" + i.ToString(), typeof(int));
            }

            for (int i = 1; i <= 11; i++)
            {
                dt.Columns.Add("bc" + i.ToString(), typeof(int));
            }

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (i == 0)
                {
                    for (int j = 0; j < 11; j++)
                    {
                        if (j == Convert.ToInt32(dt.Rows[i][1]) - 1 || j == Convert.ToInt32(dt.Rows[i][2]) - 1)
                        {
                            dt.Rows[i][j + 8] = -1;
                        }
                        else
                        {
                            dt.Rows[i][j + 8] = 1;
                        }
                    }

                    for (int j = 0; j < 11; j++)
                    {
                        if (j == Convert.ToInt32(dt.Rows[i][1]) - 1)
                        {
                            dt.Rows[i][j + 19] = -1;
                        }
                        else
                        {
                            dt.Rows[i][j + 19] = 1;
                        }
                    }

                    for (int j = 0; j < 11; j++)
                    {
                        if (j == Convert.ToInt32(dt.Rows[i][2]) - 1)
                        {
                            dt.Rows[i][j + 30] = -1;
                        }
                        else
                        {
                            dt.Rows[i][j + 30] = 1;
                        }
                    }
                }
                else
                {
                    for (int j = 0; j < 11; j++)
                    {
                        if (j == Convert.ToInt32(dt.Rows[i][1]) - 1 || j == Convert.ToInt32(dt.Rows[i][2]) - 1)
                        {
                            if (Convert.ToInt32(dt.Rows[i - 1][j + 8]) < 0)
                            {
                                dt.Rows[i][j + 8] = Convert.ToInt32(dt.Rows[i - 1][j + 8]) - 1;
                            }
                            else
                            {
                                dt.Rows[i][j + 8] = -1;
                            }
                        }
                        else
                        {
                            if (Convert.ToInt32(dt.Rows[i - 1][j + 8]) > 0)
                            {
                                dt.Rows[i][j + 8] = Convert.ToInt32(dt.Rows[i - 1][j + 8]) + 1;
                            }
                            else
                            {
                                dt.Rows[i][j + 8] = 1;
                            }

                        }
                    }

                    for (int j = 0; j < 11; j++)
                    {
                        if (j == Convert.ToInt32(dt.Rows[i][1]) - 1)
                        {
                            if (Convert.ToInt32(dt.Rows[i - 1][j + 19]) < 0)
                            {
                                dt.Rows[i][j + 19] = Convert.ToInt32(dt.Rows[i - 1][j + 19]) - 1;
                            }
                            else
                            {
                                dt.Rows[i][j + 19] = -1;
                            }
                        }
                        else
                        {
                            if (Convert.ToInt32(dt.Rows[i - 1][j + 19]) > 0)
                            {
                                dt.Rows[i][j + 19] = Convert.ToInt32(dt.Rows[i - 1][j + 19]) + 1;
                            }
                            else
                            {
                                dt.Rows[i][j + 19] = 1;
                            }

                        }
                    }

                    for (int j = 0; j < 11; j++)
                    {
                        if (j == Convert.ToInt32(dt.Rows[i][2]) - 1)
                        {
                            if (Convert.ToInt32(dt.Rows[i - 1][j + 30]) < 0)
                            {
                                dt.Rows[i][j + 30] = Convert.ToInt32(dt.Rows[i - 1][j + 30]) - 1;
                            }
                            else
                            {
                                dt.Rows[i][j + 30] = -1;
                            }

                        }
                        else
                        {
                            if (Convert.ToInt32(dt.Rows[i - 1][j + 30]) > 0)
                            {
                                dt.Rows[i][j + 30] = Convert.ToInt32(dt.Rows[i - 1][j + 30]) + 1;
                            }
                            else
                            {
                                dt.Rows[i][j + 30] = 1;
                            }
                        }
                    }
                }
            }
            Shove._Web.Cache.SetCache("HN11X5_Q2FBT", dt, 600);

        }

        dtall = dt.Clone();

        DataRow[] dr = null;
        if (Type == 1)
        {
            dr = dt.Select("DaySpan <=" + DaySpan, "ID ASC");
        }
        else
        {
            dr = dt.Select("DaySpan =" + DaySpan, "ID ASC");
        }
        foreach (DataRow d in dr)
        {
            dtall.Rows.Add(d.ItemArray);
        }

        dtall.Columns.Remove("ID");
        dtall.Columns.Remove("DaySpan");

        return dtall;
    }

    //=======   前二组选对应表
    public DataTable HN11X5_Q2ZXDYB(int DaySpan, int Type, ref int ReturnValue, ref string returnDescription)
    {

        DataSet ds = null;
        DataTable dt = null;
        DataTable dtall = null;
        DateTime Date = System.DateTime.Now;

        dt = Shove._Web.Cache.GetCacheAsDataTable("HN11X5_Q2ZXDYB");
        if (dt == null)
        {
            DAL.Procedures.P_TrendChart_11YDJ_WINNUM(ref ds, Date, 77, ref ReturnValue, ref returnDescription);

            if (ds == null)
            {
                return dt;
            }

            dt = ds.Tables[0];

            dt.Columns.Add("bh", typeof(int));

            for (int i = 3; i <= 21; i++)
            {
                dt.Columns.Add("b" + i.ToString(), typeof(int));
            }


            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i][8] = Convert.ToInt32(dt.Rows[i][1]) + Convert.ToInt32(dt.Rows[i][2]);

                if (i == 0)
                {
                    for (int j = 3; j <= 21; j++)
                    {
                        if (j == Convert.ToInt32(dt.Rows[i][1]) + Convert.ToInt32(dt.Rows[i][2]))
                        {
                            dt.Rows[i][j + 6] = -1;
                        }
                        else
                        {
                            dt.Rows[i][j + 6] = 1;
                        }
                    }
                }
                else
                {
                    for (int j = 3; j <= 21; j++)
                    {
                        if (j == Convert.ToInt32(dt.Rows[i][1]) + Convert.ToInt32(dt.Rows[i][2]))
                        {
                            if (Convert.ToInt32(dt.Rows[i - 1][j + 6]) < 0)
                            {
                                dt.Rows[i][j + 6] = Convert.ToInt32(dt.Rows[i - 1][j + 6]) - 1;
                            }
                            else
                            {
                                dt.Rows[i][j + 6] = -1;
                            }
                        }
                        else
                        {
                            if (Convert.ToInt32(dt.Rows[i - 1][j + 6]) > 0)
                            {
                                dt.Rows[i][j + 6] = Convert.ToInt32(dt.Rows[i - 1][j + 6]) + 1;
                            }
                            else
                            {
                                dt.Rows[i][j + 6] = 1;
                            }
                        }
                    }
                }
            }
            Shove._Web.Cache.SetCache("HN11X5_Q2ZXDYB", dt, 600);
        }

        dtall = dt.Clone();

        DataRow[] dr = null;
        if (Type == 1)
        {
            dr = dt.Select("DaySpan <=" + DaySpan, "ID ASC");
        }
        else
        {
            dr = dt.Select("DaySpan =" + DaySpan, "ID ASC");
        }
        foreach (DataRow d in dr)
        {
            dtall.Rows.Add(d.ItemArray);
        }

        dtall.Columns.Remove("ID");
        dtall.Columns.Remove("DaySpan");

        return dtall;
    }

    //=======   前二和值
    public DataTable HN11X5_Q2HZ(int DaySpan, int Type, ref int ReturnValue, ref string returnDescription)
    {

        DataSet ds = null;
        DataTable dt = null;
        DataTable dtall = null;
        DateTime Date = System.DateTime.Now;

        dt = Shove._Web.Cache.GetCacheAsDataTable("HN11X5_Q2HZ");
        if (dt == null)
        {
            DAL.Procedures.P_TrendChart_11YDJ_WINNUM(ref ds, Date, 77, ref ReturnValue, ref returnDescription);

            if (ds == null)
            {
                return dt;
            }

            dt = ds.Tables[0];

            for (int i = 3; i <= 21; i++)
            {
                dt.Columns.Add("b" + i.ToString(), typeof(int));
            }

            for (int i = 0; i <= 9; i++)
            {
                dt.Columns.Add("bb" + i.ToString(), typeof(int));
            }

            for (int i = 0; i <= 2; i++)
            {
                dt.Columns.Add("bc3" + i.ToString(), typeof(int));
            }

            for (int i = 0; i <= 3; i++)
            {
                dt.Columns.Add("bc4" + i.ToString(), typeof(int));
            }

            for (int i = 0; i <= 4; i++)
            {
                dt.Columns.Add("bc5" + i.ToString(), typeof(int));
            }

            dt.Columns.Add("bd1", typeof(int));
            dt.Columns.Add("bd2", typeof(int));
            dt.Columns.Add("bj1", typeof(int));
            dt.Columns.Add("bj2", typeof(int));

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (i == 0)
                {
                    for (int j = 3; j <= 21; j++)
                    {
                        if (j == Convert.ToInt32(dt.Rows[i][1]) + Convert.ToInt32(dt.Rows[i][2]))
                        {
                            dt.Rows[i][j + 5] = -1;
                        }
                        else
                        {
                            dt.Rows[i][j + 5] = 1;
                        }
                    }

                    for (int j = 0; j <= 9; j++)
                    {
                        if (j == (Convert.ToInt32(dt.Rows[i][1]) + Convert.ToInt32(dt.Rows[i][2])) % 10)
                        {
                            dt.Rows[i][j + 27] = -1;
                        }
                        else
                        {
                            dt.Rows[i][j + 27] = 1;
                        }
                    }

                    for (int j = 0; j < 3; j++)
                    {
                        if (j == (Convert.ToInt32(dt.Rows[i][1]) + Convert.ToInt32(dt.Rows[i][2])) % 3)
                        {
                            dt.Rows[i][j + 37] = -1;
                        }
                        else
                        {
                            dt.Rows[i][j + 37] = 1;
                        }
                    }

                    for (int j = 0; j < 4; j++)
                    {
                        if (j == (Convert.ToInt32(dt.Rows[i][1]) + Convert.ToInt32(dt.Rows[i][2])) % 4)
                        {
                            dt.Rows[i][j + 40] = -1;
                        }
                        else
                        {
                            dt.Rows[i][j + 40] = 1;
                        }
                    }

                    for (int j = 0; j < 5; j++)
                    {
                        if (j == (Convert.ToInt32(dt.Rows[i][1]) + Convert.ToInt32(dt.Rows[i][2])) % 5)
                        {
                            dt.Rows[i][j + 44] = -1;
                        }
                        else
                        {
                            dt.Rows[i][j + 44] = 1;
                        }
                    }

                    if ((Convert.ToInt32(dt.Rows[i][1]) + Convert.ToInt32(dt.Rows[i][2])) > 11)
                    {
                        dt.Rows[i][49] = -1;
                        dt.Rows[i][50] = 1;
                    }
                    else
                    {
                        dt.Rows[i][49] = 1;
                        dt.Rows[i][50] = -1;
                    }

                    if ((Convert.ToInt32(dt.Rows[i][1]) + Convert.ToInt32(dt.Rows[i][2])) % 2 == 1)
                    {
                        dt.Rows[i][51] = -1;
                        dt.Rows[i][52] = 1;
                    }
                    else
                    {
                        dt.Rows[i][51] = 1;
                        dt.Rows[i][52] = -1;
                    }
                }
                else
                {
                    for (int j = 3; j <= 21; j++)
                    {
                        if (j == Convert.ToInt32(dt.Rows[i][1]) + Convert.ToInt32(dt.Rows[i][2]))
                        {
                            if (Convert.ToInt32(dt.Rows[i - 1][j + 5]) > 0)
                            {
                                dt.Rows[i][j + 5] = -1;
                            }
                            else
                            {
                                dt.Rows[i][j + 5] = Convert.ToInt32(dt.Rows[i - 1][j + 5]) - 1;
                            }
                        }
                        else
                        {
                            if (Convert.ToInt32(dt.Rows[i - 1][j + 5]) < 0)
                            {
                                dt.Rows[i][j + 5] = 1;
                            }
                            else
                            {
                                dt.Rows[i][j + 5] = Convert.ToInt32(dt.Rows[i - 1][j + 5]) + 1;
                            }
                        }
                    }

                    for (int j = 0; j <= 9; j++)
                    {
                        if (j == (Convert.ToInt32(dt.Rows[i][1]) + Convert.ToInt32(dt.Rows[i][2])) % 10)
                        {
                            if (Convert.ToInt32(dt.Rows[i - 1][j + 27]) > 0)
                            {
                                dt.Rows[i][j + 27] = -1;
                            }
                            else
                            {
                                dt.Rows[i][j + 27] = Convert.ToInt32(dt.Rows[i - 1][j + 27]) - 1;
                            }
                        }
                        else
                        {
                            if (Convert.ToInt32(dt.Rows[i - 1][j + 27]) < 0)
                            {
                                dt.Rows[i][j + 27] = 1;
                            }
                            else
                            {
                                dt.Rows[i][j + 27] = Convert.ToInt32(dt.Rows[i - 1][j + 27]) + 1;
                            }
                        }
                    }

                    for (int j = 0; j < 3; j++)
                    {
                        if (j == (Convert.ToInt32(dt.Rows[i][1]) + Convert.ToInt32(dt.Rows[i][2])) % 3)
                        {
                            if (Convert.ToInt32(dt.Rows[i - 1][j + 37]) > 0)
                            {
                                dt.Rows[i][j + 37] = -1;
                            }
                            else
                            {
                                dt.Rows[i][j + 37] = Convert.ToInt32(dt.Rows[i - 1][j + 37]) - 1;
                            }

                        }
                        else
                        {
                            if (Convert.ToInt32(dt.Rows[i - 1][j + 37]) < 0)
                            {
                                dt.Rows[i][j + 37] = 1;
                            }
                            else
                            {
                                dt.Rows[i][j + 37] = Convert.ToInt32(dt.Rows[i - 1][j + 37]) + 1;
                            }

                        }
                    }

                    for (int j = 0; j < 4; j++)
                    {
                        if (j == (Convert.ToInt32(dt.Rows[i][1]) + Convert.ToInt32(dt.Rows[i][2])) % 4)
                        {
                            if (Convert.ToInt32(dt.Rows[i - 1][j + 40]) > 0)
                            {
                                dt.Rows[i][j + 40] = -1;
                            }
                            else
                            {
                                dt.Rows[i][j + 40] = Convert.ToInt32(dt.Rows[i - 1][j + 40]) - 1;
                            }

                        }
                        else
                        {
                            if (Convert.ToInt32(dt.Rows[i - 1][j + 40]) < 0)
                            {
                                dt.Rows[i][j + 40] = 1;
                            }
                            else
                            {
                                dt.Rows[i][j + 40] = Convert.ToInt32(dt.Rows[i - 1][j + 40]) + 1;
                            }

                        }
                    }

                    for (int j = 0; j < 5; j++)
                    {

                        if (j == (Convert.ToInt32(dt.Rows[i][1]) + Convert.ToInt32(dt.Rows[i][2])) % 5)
                        {
                            if (Convert.ToInt32(dt.Rows[i - 1][j + 44]) > 0)
                            {
                                dt.Rows[i][j + 44] = -1;
                            }
                            else
                            {
                                dt.Rows[i][j + 44] = Convert.ToInt32(dt.Rows[i - 1][j + 44]) - 1;
                            }

                        }
                        else
                        {
                            if (Convert.ToInt32(dt.Rows[i - 1][j + 44]) < 0)
                            {
                                dt.Rows[i][j + 44] = 1;
                            }
                            else
                            {
                                dt.Rows[i][j + 44] = Convert.ToInt32(dt.Rows[i - 1][j + 44]) + 1;
                            }
                        }
                    }

                    if ((Convert.ToInt32(dt.Rows[i][1]) + Convert.ToInt32(dt.Rows[i][2])) > 11)
                    {
                        if (Convert.ToInt32(dt.Rows[i - 1][49]) > 0)
                        {
                            dt.Rows[i][49] = -1;
                        }
                        else
                        {
                            dt.Rows[i][49] = Convert.ToInt32(dt.Rows[i - 1][49]) - 1;
                        }

                        if (Convert.ToInt32(dt.Rows[i - 1][50]) < 0)
                        {
                            dt.Rows[i][50] = 1;
                        }
                        else
                        {
                            dt.Rows[i][50] = Convert.ToInt32(dt.Rows[i - 1][50]) + 1;
                        }

                    }
                    else
                    {
                        if (Convert.ToInt32(dt.Rows[i - 1][49]) < 0)
                        {
                            dt.Rows[i][49] = 1;
                        }
                        else
                        {
                            dt.Rows[i][49] = Convert.ToInt32(dt.Rows[i - 1][49]) + 1;
                        }

                        if (Convert.ToInt32(dt.Rows[i - 1][50]) > 0)
                        {
                            dt.Rows[i][50] = -1;
                        }
                        else
                        {
                            dt.Rows[i][50] = Convert.ToInt32(dt.Rows[i - 1][50]) - 1;
                        }

                    }

                    if ((Convert.ToInt32(dt.Rows[i][1]) + Convert.ToInt32(dt.Rows[i][2])) % 2 == 1)
                    {
                        if (Convert.ToInt32(dt.Rows[i - 1][51]) > 0)
                        {
                            dt.Rows[i][51] = -1;
                        }
                        else
                        {
                            dt.Rows[i][51] = Convert.ToInt32(dt.Rows[i - 1][51]) - 1;
                        }

                        if (Convert.ToInt32(dt.Rows[i - 1][52]) < 0)
                        {
                            dt.Rows[i][52] = 1;
                        }
                        else
                        {
                            dt.Rows[i][52] = Convert.ToInt32(dt.Rows[i - 1][52]) + 1;
                        }

                    }
                    else
                    {
                        if (Convert.ToInt32(dt.Rows[i - 1][51]) < 0)
                        {
                            dt.Rows[i][51] = 1;
                        }
                        else
                        {
                            dt.Rows[i][51] = Convert.ToInt32(dt.Rows[i - 1][51]) + 1;
                        }

                        if (Convert.ToInt32(dt.Rows[i - 1][52]) > 0)
                        {
                            dt.Rows[i][52] = -1;
                        }
                        else
                        {
                            dt.Rows[i][52] = Convert.ToInt32(dt.Rows[i - 1][52]) - 1;
                        }
                    }
                }
            }
            Shove._Web.Cache.SetCache("HN11X5_Q2HZ", dt, 600);
        }

        dtall = dt.Clone();

        DataRow[] dr = null;
        if (Type == 1)
        {
            dr = dt.Select("DaySpan <=" + DaySpan, "ID ASC");
        }
        else
        {
            dr = dt.Select("DaySpan =" + DaySpan, "ID ASC");
        }
        foreach (DataRow d in dr)
        {
            dtall.Rows.Add(d.ItemArray);
        }

        dtall.Columns.Remove("ID");
        dtall.Columns.Remove("DaySpan");

        return dtall;
    }

    //=======   前三分位图 
    public DataTable HN11X5_Q3FWT(int DaySpan, int Type, ref int ReturnValue, ref string returnDescription)
    {

        DataSet ds = null;
        DataTable dt = null;
        DataTable dtall = null;
        DateTime Date = System.DateTime.Now;

        dt = Shove._Web.Cache.GetCacheAsDataTable("HN11X5_Q3FWT");

        if (dt == null)
        {

            DAL.Procedures.P_TrendChart_11YDJ_WINNUM(ref ds, Date, 77, ref ReturnValue, ref returnDescription);

            if (ds == null)
            {
                return dt;
            }

            dt = ds.Tables[0];


            for (int i = 1; i <= 11; i++)
            {
                dt.Columns.Add("b" + i.ToString(), typeof(int));
            }

            for (int i = 1; i <= 11; i++)
            {
                dt.Columns.Add("bb" + i.ToString(), typeof(int));
            }

            for (int i = 1; i <= 11; i++)
            {
                dt.Columns.Add("bc" + i.ToString(), typeof(int));
            }

            for (int i = 0; i <= 3; i++)
            {
                dt.Columns.Add("bd" + i.ToString(), typeof(int));
            }

            for (int i = 0; i <= 3; i++)
            {
                dt.Columns.Add("be" + i.ToString(), typeof(int));
            }


            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (i == 0)
                {
                    for (int j = 1; j <= 11; j++)
                    {
                        if (j == Convert.ToInt32(dt.Rows[i][1]))
                        {
                            dt.Rows[i][j + 7] = -1;
                        }
                        else
                        {
                            dt.Rows[i][j + 7] = 1;
                        }
                    }

                    for (int j = 1; j <= 11; j++)
                    {
                        if (j == Convert.ToInt32(dt.Rows[i][2]))
                        {
                            dt.Rows[i][j + 18] = -1;
                        }
                        else
                        {
                            dt.Rows[i][j + 18] = 1;
                        }
                    }

                    for (int j = 1; j <= 11; j++)
                    {
                        if (j == Convert.ToInt32(dt.Rows[i][3]))
                        {
                            dt.Rows[i][j + 29] = -1;
                        }
                        else
                        {
                            dt.Rows[i][j + 29] = 1;
                        }
                    }

                    int fragj = 0;
                    int fragd = 0;

                    for (int j = 1; j <= 3; j++)
                    {
                        if (Convert.ToInt32(dt.Rows[i][j]) % 2 == 1)
                        {
                            fragj++;
                        }
                        if (Convert.ToInt32(dt.Rows[i][j]) > 5)
                        {
                            fragd++;
                        }
                    }

                    for (int j = 0; j <= 3; j++)
                    {
                        if (j == fragj)
                        {
                            dt.Rows[i][j + 41] = -1;
                        }
                        else
                        {
                            dt.Rows[i][j + 41] = 1;
                        }
                    }

                    for (int j = 0; j <= 3; j++)
                    {
                        if (j == fragd)
                        {
                            dt.Rows[i][j + 45] = -1;
                        }
                        else
                        {
                            dt.Rows[i][j + 45] = 1;
                        }
                    }


                }
                else
                {
                    for (int j = 1; j <= 11; j++)
                    {
                        if (j == Convert.ToInt32(dt.Rows[i][1]))
                        {
                            if (Convert.ToInt32(dt.Rows[i - 1][j + 7]) < 0)
                            {
                                dt.Rows[i][j + 7] = Convert.ToInt32(dt.Rows[i - 1][j + 7]) - 1;
                            }
                            else
                            {
                                dt.Rows[i][j + 7] = -1;
                            }
                        }
                        else
                        {
                            if (Convert.ToInt32(dt.Rows[i - 1][j + 7]) > 0)
                            {
                                dt.Rows[i][j + 7] = Convert.ToInt32(dt.Rows[i - 1][j + 7]) + 1;
                            }
                            else
                            {
                                dt.Rows[i][j + 7] = 1;
                            }

                        }
                    }

                    for (int j = 1; j <= 11; j++)
                    {
                        if (j == Convert.ToInt32(dt.Rows[i][2]))
                        {
                            if (Convert.ToInt32(dt.Rows[i - 1][j + 18]) < 0)
                            {
                                dt.Rows[i][j + 18] = Convert.ToInt32(dt.Rows[i - 1][j + 18]) - 1;
                            }
                            else
                            {
                                dt.Rows[i][j + 18] = -1;
                            }

                        }
                        else
                        {
                            if (Convert.ToInt32(dt.Rows[i - 1][j + 18]) > 0)
                            {
                                dt.Rows[i][j + 18] = Convert.ToInt32(dt.Rows[i - 1][j + 18]) + 1;
                            }
                            else
                            {
                                dt.Rows[i][j + 18] = 1;
                            }
                        }
                    }

                    for (int j = 1; j <= 11; j++)
                    {
                        if (j == Convert.ToInt32(dt.Rows[i][3]))
                        {
                            if (Convert.ToInt32(dt.Rows[i - 1][j + 29]) < 0)
                            {
                                dt.Rows[i][j + 29] = Convert.ToInt32(dt.Rows[i - 1][j + 29]) - 1;
                            }
                            else
                            {
                                dt.Rows[i][j + 29] = -1;
                            }

                        }
                        else
                        {
                            if (Convert.ToInt32(dt.Rows[i - 1][j + 29]) > 0)
                            {
                                dt.Rows[i][j + 29] = Convert.ToInt32(dt.Rows[i - 1][j + 29]) + 1;
                            }
                            else
                            {
                                dt.Rows[i][j + 29] = 1;
                            }

                        }
                    }

                    int fragj = 0;
                    int fragd = 0;

                    for (int j = 1; j <= 3; j++)
                    {
                        if (Convert.ToInt32(dt.Rows[i][j]) % 2 == 1)
                        {
                            fragj++;
                        }
                        if (Convert.ToInt32(dt.Rows[i][j]) > 5)
                        {
                            fragd++;
                        }
                    }


                    for (int j = 0; j <= 3; j++)
                    {
                        if (j == fragj)
                        {
                            if (Convert.ToInt32(dt.Rows[i - 1][j + 41]) < 0)
                            {
                                dt.Rows[i][j + 41] = Convert.ToInt32(dt.Rows[i - 1][j + 41]) - 1;
                            }
                            else
                            {
                                dt.Rows[i][j + 41] = -1;
                            }
                        }
                        else
                        {
                            if (Convert.ToInt32(dt.Rows[i - 1][j + 41]) > 0)
                            {
                                dt.Rows[i][j + 41] = Convert.ToInt32(dt.Rows[i - 1][j + 41]) + 1;
                            }
                            else
                            {
                                dt.Rows[i][j + 41] = 1;
                            }
                        }
                    }

                    for (int j = 0; j <= 3; j++)
                    {
                        if (j == fragd)
                        {
                            if (Convert.ToInt32(dt.Rows[i - 1][j + 45]) < 0)
                            {
                                dt.Rows[i][j + 45] = Convert.ToInt32(dt.Rows[i - 1][j + 45]) - 1;
                            }
                            else
                            {
                                dt.Rows[i][j + 45] = -1;
                            }

                        }
                        else
                        {
                            if (Convert.ToInt32(dt.Rows[i - 1][j + 45]) > 0)
                            {
                                dt.Rows[i][j + 45] = Convert.ToInt32(dt.Rows[i - 1][j + 45]) + 1;
                            }
                            else
                            {
                                dt.Rows[i][j + 45] = 1;
                            }
                        }
                    }

                }
            }
            Shove._Web.Cache.SetCache("HN11X5_Q3FWT", dt, 600);
        }

        dtall = dt.Clone();

        DataRow[] dr = null;
        if (Type == 1)
        {
            dr = dt.Select("DaySpan <=" + DaySpan, "ID ASC");
        }
        else
        {
            dr = dt.Select("DaySpan =" + DaySpan, "ID ASC");
        }
        foreach (DataRow d in dr)
        {
            dtall.Rows.Add(d.ItemArray);
        }

        dtall.Columns.Remove("ID");
        dtall.Columns.Remove("DaySpan");

        return dtall;
    }

    //=======   前三分布图 
    public DataTable HN11X5_Q3FBT(int DaySpan, int Type, ref int ReturnValue, ref string returnDescription)
    {

        DataSet ds = null;
        DataTable dt = null;
        DataTable dtall = null;
        DateTime Date = System.DateTime.Now;

        dt = Shove._Web.Cache.GetCacheAsDataTable("HN11X5_Q3FBT");

        if (dt == null)
        {
            DAL.Procedures.P_TrendChart_11YDJ_WINNUM(ref ds, Date, 77, ref ReturnValue, ref returnDescription);

            if (ds == null)
            {
                return dt;
            }

            dt = ds.Tables[0];

            for (int i = 1; i <= 11; i++)
            {
                dt.Columns.Add("b" + i.ToString(), typeof(int));
            }

            for (int i = 1; i <= 8; i++)
            {
                dt.Columns.Add("bb" + i.ToString(), typeof(int));
            }

            for (int i = 1; i <= 8; i++)
            {
                dt.Columns.Add("bc" + i.ToString(), typeof(int));
            }

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (i == 0)
                {
                    for (int j = 1; j <= 11; j++)
                    {
                        if (j == Convert.ToInt32(dt.Rows[i][1]) || j == Convert.ToInt32(dt.Rows[i][2]) || j == Convert.ToInt32(dt.Rows[i][3]))
                        {
                            dt.Rows[i][j + 7] = -1;
                        }
                        else
                        {
                            dt.Rows[i][j + 7] = 1;
                        }
                    }

                    ArrayList a = new ArrayList();
                    a.Add("111");
                    a.Add("110");
                    a.Add("101");
                    a.Add("011");
                    a.Add("100");
                    a.Add("010");
                    a.Add("001");
                    a.Add("000");

                    for (int j = 1; j <= 8; j++)
                    {
                        if ((j - 1) == a.IndexOf(Convert.ToString(Convert.ToInt32(dt.Rows[i][1]) % 2) + Convert.ToString(Convert.ToInt32(dt.Rows[i][2]) % 2) + Convert.ToString(Convert.ToInt32(dt.Rows[i][3]) % 2)))
                        {
                            dt.Rows[i][j + 18] = -1;
                        }
                        else
                        {
                            dt.Rows[i][j + 18] = 1;
                        }
                    }

                    for (int j = 1; j <= 8; j++)
                    {
                        if ((8 - j) == a.IndexOf(Convert.ToString(Convert.ToInt32(dt.Rows[i][1]) > 5 ? "1" : "0") + Convert.ToString(Convert.ToInt32(dt.Rows[i][2]) > 5 ? "1" : "0") + Convert.ToString(Convert.ToInt32(dt.Rows[i][3]) > 5 ? "1" : "0")))
                        {
                            dt.Rows[i][j + 26] = -1;
                        }
                        else
                        {
                            dt.Rows[i][j + 26] = 1;
                        }
                    }

                }
                else
                {

                    for (int j = 1; j <= 11; j++)
                    {
                        if (j == Convert.ToInt32(dt.Rows[i][1]) || j == Convert.ToInt32(dt.Rows[i][2]) || j == Convert.ToInt32(dt.Rows[i][3]))
                        {
                            if (Convert.ToInt32(dt.Rows[i - 1][j + 7]) > 0)
                            {
                                dt.Rows[i][j + 7] = -1;
                            }
                            else
                            {
                                dt.Rows[i][j + 7] = Convert.ToInt32(dt.Rows[i - 1][j + 7]) - 1;
                            }
                        }
                        else
                        {
                            if (Convert.ToInt32(dt.Rows[i - 1][j + 7]) < 0)
                            {
                                dt.Rows[i][j + 7] = 1;
                            }
                            else
                            {
                                dt.Rows[i][j + 7] = Convert.ToInt32(dt.Rows[i - 1][j + 7]) + 1;
                            }

                        }
                    }

                    ArrayList a = new ArrayList();
                    a.Add("111");
                    a.Add("110");
                    a.Add("101");
                    a.Add("011");
                    a.Add("100");
                    a.Add("010");
                    a.Add("001");
                    a.Add("000");

                    for (int j = 1; j <= 8; j++)
                    {
                        if ((j - 1) == a.IndexOf(Convert.ToString(Convert.ToInt32(dt.Rows[i][1]) % 2) + Convert.ToString(Convert.ToInt32(dt.Rows[i][2]) % 2) + Convert.ToString(Convert.ToInt32(dt.Rows[i][3]) % 2)))
                        {
                            if (Convert.ToInt32(dt.Rows[i - 1][j + 18]) > 0)
                            {
                                dt.Rows[i][j + 18] = -1;
                            }
                            else
                            {
                                dt.Rows[i][j + 18] = Convert.ToInt32(dt.Rows[i - 1][j + 18]) - 1;
                            }

                        }
                        else
                        {
                            if (Convert.ToInt32(dt.Rows[i - 1][j + 18]) < 0)
                            {
                                dt.Rows[i][j + 18] = 1;
                            }
                            else
                            {
                                dt.Rows[i][j + 18] = Convert.ToInt32(dt.Rows[i - 1][j + 18]) + 1;
                            }

                        }
                    }

                    for (int j = 1; j <= 8; j++)
                    {
                        if ((8 - j) == a.IndexOf(Convert.ToString(Convert.ToInt32(dt.Rows[i][1]) > 5 ? "1" : "0") + Convert.ToString(Convert.ToInt32(dt.Rows[i][2]) > 5 ? "1" : "0") + Convert.ToString(Convert.ToInt32(dt.Rows[i][3]) > 5 ? "1" : "0")))
                        {
                            if (Convert.ToInt32(dt.Rows[i - 1][j + 26]) > 0)
                            {
                                dt.Rows[i][j + 26] = -1;
                            }
                            else
                            {
                                dt.Rows[i][j + 26] = Convert.ToInt32(dt.Rows[i - 1][j + 26]) - 1;
                            }
                        }
                        else
                        {
                            if (Convert.ToInt32(dt.Rows[i - 1][j + 26]) < 0)
                            {
                                dt.Rows[i][j + 26] = 1;
                            }
                            else
                            {
                                dt.Rows[i][j + 26] = Convert.ToInt32(dt.Rows[i - 1][j + 26]) + 1;
                            }

                        }
                    }

                }
            }
            Shove._Web.Cache.SetCache("HN11X5_Q3FBT", dt, 600);
        }
        dtall = dt.Clone();

        DataRow[] dr = null;
        if (Type == 1)
        {
            dr = dt.Select("DaySpan <=" + DaySpan, "ID ASC");
        }
        else
        {
            dr = dt.Select("DaySpan =" + DaySpan, "ID ASC");
        }
        foreach (DataRow d in dr)
        {
            dtall.Rows.Add(d.ItemArray);
        }

        dtall.Columns.Remove("ID");
        dtall.Columns.Remove("DaySpan");

        return dtall;
    }

    //=======   前三和值图 
    public DataTable HN11X5_Q3HZT(int DaySpan, int Type, ref int ReturnValue, ref string returnDescription)
    {

        DataSet ds = null;
        DataTable dt = null;
        DataTable dtall = null;
        DateTime Date = System.DateTime.Now;

        dt = Shove._Web.Cache.GetCacheAsDataTable("HN11X5_Q3HZT");

        if (dt == null)
        {

            DAL.Procedures.P_TrendChart_11YDJ_WINNUM(ref ds, Date, 77, ref ReturnValue, ref returnDescription);

            if (ds == null)
            {
                return dt;
            }

            dt = ds.Tables[0];


            for (int i = 6; i <= 30; i++)
            {
                dt.Columns.Add("b" + i.ToString(), typeof(int));
            }

            for (int i = 0; i <= 9; i++)
            {
                dt.Columns.Add("bb" + i.ToString(), typeof(int));
            }

            for (int i = 0; i <= 3; i++)
            {
                dt.Columns.Add("bc3" + i.ToString(), typeof(int));
            }

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (i == 0)
                {
                    for (int j = 6; j <= 30; j++)
                    {
                        if (j == Convert.ToInt32(dt.Rows[i][1]) + Convert.ToInt32(dt.Rows[i][2]) + Convert.ToInt32(dt.Rows[i][3]))
                        {
                            dt.Rows[i][j + 2] = -1;
                        }
                        else
                        {
                            dt.Rows[i][j + 2] = 1;
                        }
                    }

                    for (int j = 0; j <= 9; j++)
                    {
                        if (j == (Convert.ToInt32(dt.Rows[i][1]) + Convert.ToInt32(dt.Rows[i][2]) + Convert.ToInt32(dt.Rows[i][3])) % 10)
                        {
                            dt.Rows[i][j + 33] = -1;
                        }
                        else
                        {
                            dt.Rows[i][j + 33] = 1;
                        }
                    }

                    for (int j = 0; j < 3; j++)
                    {
                        if (j == (Convert.ToInt32(dt.Rows[i][1]) + Convert.ToInt32(dt.Rows[i][2]) + Convert.ToInt32(dt.Rows[i][3])) % 3)
                        {
                            dt.Rows[i][j + 43] = -1;
                        }
                        else
                        {
                            dt.Rows[i][j + 43] = 1;
                        }
                    }


                }
                else
                {
                    for (int j = 6; j <= 30; j++)
                    {
                        if (j == Convert.ToInt32(dt.Rows[i][1]) + Convert.ToInt32(dt.Rows[i][2]) + Convert.ToInt32(dt.Rows[i][3]))
                        {
                            if (Convert.ToInt32(dt.Rows[i - 1][j + 2]) > 0)
                            {
                                dt.Rows[i][j + 2] = -1;
                            }
                            else
                            {
                                dt.Rows[i][j + 2] = Convert.ToInt32(dt.Rows[i - 1][j + 2]) - 1;
                            }
                        }
                        else
                        {
                            if (Convert.ToInt32(dt.Rows[i - 1][j + 2]) < 0)
                            {
                                dt.Rows[i][j + 2] = 1;
                            }
                            else
                            {
                                dt.Rows[i][j + 2] = Convert.ToInt32(dt.Rows[i - 1][j + 2]) + 1;
                            }
                        }
                    }

                    for (int j = 0; j <= 9; j++)
                    {
                        if (j == (Convert.ToInt32(dt.Rows[i][1]) + Convert.ToInt32(dt.Rows[i][2]) + Convert.ToInt32(dt.Rows[i][3])) % 10)
                        {
                            if (Convert.ToInt32(dt.Rows[i - 1][j + 33]) > 0)
                            {
                                dt.Rows[i][j + 33] = -1;
                            }
                            else
                            {
                                dt.Rows[i][j + 33] = Convert.ToInt32(dt.Rows[i - 1][j + 33]) - 1;
                            }
                        }
                        else
                        {
                            if (Convert.ToInt32(dt.Rows[i - 1][j + 33]) < 0)
                            {
                                dt.Rows[i][j + 33] = 1;
                            }
                            else
                            {
                                dt.Rows[i][j + 33] = Convert.ToInt32(dt.Rows[i - 1][j + 33]) + 1;
                            }
                        }
                    }

                    for (int j = 0; j < 3; j++)
                    {
                        if (j == (Convert.ToInt32(dt.Rows[i][1]) + Convert.ToInt32(dt.Rows[i][2]) + Convert.ToInt32(dt.Rows[i][3])) % 3)
                        {
                            if (Convert.ToInt32(dt.Rows[i - 1][j + 43]) > 0)
                            {
                                dt.Rows[i][j + 43] = -1;
                            }
                            else
                            {
                                dt.Rows[i][j + 43] = Convert.ToInt32(dt.Rows[i - 1][j + 43]) - 1;
                            }

                        }
                        else
                        {
                            if (Convert.ToInt32(dt.Rows[i - 1][j + 43]) < 0)
                            {
                                dt.Rows[i][j + 43] = 1;
                            }
                            else
                            {
                                dt.Rows[i][j + 43] = Convert.ToInt32(dt.Rows[i - 1][j + 43]) + 1;
                            }

                        }
                    }

                }
            }
            Shove._Web.Cache.SetCache("HN11X5_Q3HZT", dt, 600);
        }
        dtall = dt.Clone();

        DataRow[] dr = null;
        if (Type == 1)
        {
            dr = dt.Select("DaySpan <=" + DaySpan, "ID ASC");
        }
        else
        {
            dr = dt.Select("DaySpan =" + DaySpan, "ID ASC");
        }
        foreach (DataRow d in dr)
        {
            dtall.Rows.Add(d.ItemArray);
        }

        dtall.Columns.Remove("ID");
        dtall.Columns.Remove("DaySpan");

        return dtall;
    }

    #endregion

    #region 重庆时时彩

    public DataTable CQSSC_5X_ZHFBZST_Select(int num, string str1, string str2, ref string result)
    {
        DataTable dt = null;
        DataTable dt1 = null;

        dt = Shove._Web.Cache.GetCacheAsDataTable("CQSSC_5X_ZHFBZST_Select");

        if (dt == null || dt.Rows.Count < 1)
        {
            DataSet ds = null;
            DAL.Procedures.P_TrendChart_CQSSC_5XZHFBZST(ref ds);

            if (ds == null || ds.Tables.Count < 1)
            {
                PF.GoError(ErrorNumber.Unknow, "参数错误或者数据库--无数据", this.GetType().BaseType.FullName);

                return dt1;
            }
            else
            {
                Shove._Web.Cache.SetCache("CQSSC_5X_ZHFBZST_Select", ds.Tables[0]);

                dt = ds.Tables[0];
            }
        }

        dt1 = dt.Clone();

        //通过期号来查询
        if (num == 0)
        {
            int i = Shove._Convert.StrToInt(str1, 0);
            int j = Shove._Convert.StrToInt(str2, 0);

            DataRow[] dr2 = dt.Select("Isuse>=" + i + " and  Isuse<=" + j);

            foreach (DataRow r in dr2)
            {
                dt1.Rows.Add(r.ItemArray);
            }
        }

        //通过按纽选择查询多少期
        else
        {
            if (num >= dt.Rows.Count)
            {
                num = dt.Rows.Count;
            }

            int ii = dt.Rows.Count;                           //总记录数
            int jj = ii - num;

            DataRow[] drr = dt.Select("id > " + jj + " and id <= " + ii);

            foreach (DataRow rr in drr)
            {
                dt1.Rows.Add(rr.ItemArray);
            }
        }

        return dt1;
    }

    //五星 走势图
    public DataTable CQSSC_5X_ZST_Select(int num, string str1, string str2, ref string result)
    {
        DataTable dt = null;
        DataTable dt1 = null;

        dt = Shove._Web.Cache.GetCacheAsDataTable("CQSSC_5X_ZST_Select");

        if (dt == null || dt.Rows.Count < 1)
        {
            DataSet ds = null;
            DAL.Procedures.P_TrendChart_CQSSC_5XZST(ref ds);

            if (ds == null || ds.Tables.Count < 1)
            {
                PF.GoError(ErrorNumber.Unknow, "参数错误或者数据库--无数据", this.GetType().BaseType.FullName);

                return dt1;
            }
            else
            {
                Shove._Web.Cache.SetCache("CQSSC_5X_ZST_Select", ds.Tables[0]);

                dt = ds.Tables[0];
            }
        }

        dt1 = dt.Clone();

        //通过期号来查询
        if (num == 0)
        {
            int i = Shove._Convert.StrToInt(str1, 0);
            int j = Shove._Convert.StrToInt(str2, 0);

            DataRow[] dr2 = dt.Select("Isuse>=" + i + " and  Isuse<=" + j);

            foreach (DataRow r in dr2)
            {
                dt1.Rows.Add(r.ItemArray);
            }
        }

        //通过按纽选择查询多少期
        else
        {
            if (num >= dt.Rows.Count)
            {
                num = dt.Rows.Count;
            }

            int ii = dt.Rows.Count;                           //总记录数
            int jj = ii - num;

            DataRow[] drr = dt.Select("id > " + jj + " and id <= " + ii);

            foreach (DataRow rr in drr)
            {
                dt1.Rows.Add(rr.ItemArray);
            }
        }

        return dt1;
    }

    //五星和值走势图
    public DataTable CQSSC_5X_HZST_Select(int num, string str1, string str2, ref string result)
    {
        DataTable dt = null;
        DataTable dt1 = null;

        dt = Shove._Web.Cache.GetCacheAsDataTable("CQSSC_5X_HZST_Select");

        if (dt == null || dt.Rows.Count < 1)
        {
            DataSet ds = null;
            DAL.Procedures.P_TrendChart_CQSSC_5XHZZST(ref ds);

            if (ds == null || ds.Tables.Count < 1)
            {
                PF.GoError(ErrorNumber.Unknow, "参数错误或者数据库--无数据", this.GetType().BaseType.FullName);

                return dt1;
            }
            else
            {
                Shove._Web.Cache.SetCache("CQSSC_5X_HZST_Select", ds.Tables[0]);

                dt = ds.Tables[0];
            }
        }

        dt1 = dt.Clone();

        //通过期号来查询
        if (num == 0)
        {
            int i = Shove._Convert.StrToInt(str1, 0);
            int j = Shove._Convert.StrToInt(str2, 0);

            DataRow[] dr2 = dt.Select("Isuse>=" + i + " and  Isuse<=" + j);

            foreach (DataRow r in dr2)
            {
                dt1.Rows.Add(r.ItemArray);
            }
        }

        //通过按纽选择查询多少期
        else
        {
            if (num >= dt.Rows.Count)
            {
                num = dt.Rows.Count;
            }

            int ii = dt.Rows.Count;                           //总记录数
            int jj = ii - num;

            DataRow[] drr = dt.Select("id > " + jj + " and id <= " + ii);

            foreach (DataRow rr in drr)
            {
                dt1.Rows.Add(rr.ItemArray);
            }
        }

        return dt1;
    }

    //五星跨度走势图
    public DataTable CQSSC_5X_KDZST_Select(int num, string str1, string str2, ref string result)
    {
        DataTable dt = null;
        DataTable dt1 = null;

        dt = Shove._Web.Cache.GetCacheAsDataTable("CQSSC_5X_KDZST_Select");

        if (dt == null || dt.Rows.Count < 1)
        {
            DataSet ds = null;
            DAL.Procedures.P_TrendChart_CQSSC_5XKDZST(ref ds);

            if (ds == null || ds.Tables.Count < 1)
            {
                PF.GoError(ErrorNumber.Unknow, "参数错误或者数据库--无数据", this.GetType().BaseType.FullName);

                return dt1;
            }
            else
            {
                Shove._Web.Cache.SetCache("CQSSC_5X_KDZST_Select", ds.Tables[0]);

                dt = ds.Tables[0];
            }
        }

        dt1 = dt.Clone();

        //通过期号来查询
        if (num == 0)
        {
            int i = Shove._Convert.StrToInt(str1, 0);
            int j = Shove._Convert.StrToInt(str2, 0);

            DataRow[] dr2 = dt.Select("Isuse>=" + i + " and  Isuse<=" + j);

            foreach (DataRow r in dr2)
            {
                dt1.Rows.Add(r.ItemArray);
            }
        }

        //通过按纽选择查询多少期
        else
        {
            if (num >= dt.Rows.Count)
            {
                num = dt.Rows.Count;
            }

            int ii = dt.Rows.Count;                           //总记录数
            int jj = ii - num;

            DataRow[] drr = dt.Select("id > " + jj + " and id <= " + ii);

            foreach (DataRow rr in drr)
            {
                dt1.Rows.Add(rr.ItemArray);
            }
        }

        return dt1;
    }

    //五星平均值走势图
    public DataTable CQSSC_5X_PJZZST_Select(int num, string str1, string str2, ref string result)
    {
        DataTable dt = null;
        DataTable dt1 = null;

        dt = Shove._Web.Cache.GetCacheAsDataTable("CQSSC_5X_PJZZST_Select");

        if (dt == null || dt.Rows.Count < 1)
        {
            DataSet ds = null;
            DAL.Procedures.P_TrendChart_CQSSC_5XPJZZST(ref ds);

            if (ds == null || ds.Tables.Count < 1)
            {
                PF.GoError(ErrorNumber.Unknow, "参数错误或者数据库--无数据", this.GetType().BaseType.FullName);

                return dt1;
            }
            else
            {
                Shove._Web.Cache.SetCache("CQSSC_5X_PJZZST_Select", ds.Tables[0]);

                dt = ds.Tables[0];
            }
        }

        dt1 = dt.Clone();

        //通过期号来查询
        if (num == 0)
        {
            int i = Shove._Convert.StrToInt(str1, 0);
            int j = Shove._Convert.StrToInt(str2, 0);

            DataRow[] dr2 = dt.Select("Isuse>=" + i + " and  Isuse<=" + j);

            foreach (DataRow r in dr2)
            {
                dt1.Rows.Add(r.ItemArray);
            }
        }

        //通过按纽选择查询多少期
        else
        {
            if (num >= dt.Rows.Count)
            {
                num = dt.Rows.Count;
            }

            int ii = dt.Rows.Count;                           //总记录数
            int jj = ii - num;

            DataRow[] drr = dt.Select("id > " + jj + " and id <= " + ii);

            foreach (DataRow rr in drr)
            {
                dt1.Rows.Add(rr.ItemArray);
            }
        }

        return dt1;
    }

    //五星大小走势图
    public DataTable CQSSC_5X_DXZST_Select(int num, string str1, string str2, ref string result)
    {
        DataTable dt = null;
        DataTable dt1 = null;

        dt = Shove._Web.Cache.GetCacheAsDataTable("CQSSC_5X_DXZST_Select");

        if (dt == null || dt.Rows.Count < 1)
        {
            DataSet ds = null;
            DAL.Procedures.P_TrendChart_CQSSC_5XDXZST(ref ds);

            if (ds == null || ds.Tables.Count < 1)
            {
                PF.GoError(ErrorNumber.Unknow, "参数错误或者数据库--无数据", this.GetType().BaseType.FullName);

                return dt1;
            }
            else
            {
                Shove._Web.Cache.SetCache("CQSSC_5X_DXZST_Select", ds.Tables[0]);

                dt = ds.Tables[0];
            }
        }

        dt1 = dt.Clone();

        //通过期号来查询
        if (num == 0)
        {
            int i = Shove._Convert.StrToInt(str1, 0);
            int j = Shove._Convert.StrToInt(str2, 0);

            DataRow[] dr2 = dt.Select("Isuse>=" + i + " and  Isuse<=" + j);

            foreach (DataRow r in dr2)
            {
                dt1.Rows.Add(r.ItemArray);
            }
        }

        //通过按纽选择查询多少期
        else
        {
            if (num >= dt.Rows.Count)
            {
                num = dt.Rows.Count;
            }

            int ii = dt.Rows.Count;                           //总记录数
            int jj = ii - num;

            DataRow[] drr = dt.Select("id > " + jj + " and id <= " + ii);

            foreach (DataRow rr in drr)
            {
                dt1.Rows.Add(rr.ItemArray);
            }
        }

        return dt1;
    }

    //五星奇偶走势图
    public DataTable CQSSC_5X_JOZST_Select(int num, string str1, string str2, ref string result)
    {
        DataTable dt = null;
        DataTable dt1 = null;

        dt = Shove._Web.Cache.GetCacheAsDataTable("CQSSC_5X_JOZST_Select");

        if (dt == null || dt.Rows.Count < 1)
        {
            DataSet ds = null;
            DAL.Procedures.P_TrendChart_CQSSC_5XJOZST(ref ds);

            if (ds == null || ds.Tables.Count < 1)
            {
                PF.GoError(ErrorNumber.Unknow, "参数错误或者数据库--无数据", this.GetType().BaseType.FullName);

                return dt1;
            }
            else
            {
                Shove._Web.Cache.SetCache("CQSSC_5X_JOZST_Select", ds.Tables[0]);

                dt = ds.Tables[0];
            }
        }

        dt1 = dt.Clone();

        //通过期号来查询
        if (num == 0)
        {
            int i = Shove._Convert.StrToInt(str1, 0);
            int j = Shove._Convert.StrToInt(str2, 0);

            DataRow[] dr2 = dt.Select("Isuse>=" + i + " and  Isuse<=" + j);

            foreach (DataRow r in dr2)
            {
                dt1.Rows.Add(r.ItemArray);
            }
        }

        //通过按纽选择查询多少期
        else
        {
            if (num >= dt.Rows.Count)
            {
                num = dt.Rows.Count;
            }

            int ii = dt.Rows.Count;                           //总记录数
            int jj = ii - num;

            DataRow[] drr = dt.Select("id > " + jj + " and id <= " + ii);

            foreach (DataRow rr in drr)
            {
                dt1.Rows.Add(rr.ItemArray);
            }
        }

        return dt1;
    }

    //五星质合走势图
    public DataTable CQSSC_5X_ZHZST_Select(int num, string str1, string str2, ref string result)
    {
        DataTable dt = null;
        DataTable dt1 = null;

        dt = Shove._Web.Cache.GetCacheAsDataTable("CQSSC_5X_ZHZST_Select");

        if (dt == null || dt.Rows.Count < 1)
        {
            DataSet ds = null;
            DAL.Procedures.P_TrendChart_CQSSC_5XZHZST(ref ds);

            if (ds == null || ds.Tables.Count < 1)
            {
                PF.GoError(ErrorNumber.Unknow, "参数错误或者数据库--无数据", this.GetType().BaseType.FullName);

                return dt1;
            }
            else
            {
                Shove._Web.Cache.SetCache("CQSSC_5X_ZHZST_Select", ds.Tables[0]);

                dt = ds.Tables[0];
            }
        }

        dt1 = dt.Clone();

        //通过期号来查询
        if (num == 0)
        {
            int i = Shove._Convert.StrToInt(str1, 0);
            int j = Shove._Convert.StrToInt(str2, 0);

            DataRow[] dr2 = dt.Select("Isuse>=" + i + " and  Isuse<=" + j);

            foreach (DataRow r in dr2)
            {
                dt1.Rows.Add(r.ItemArray);
            }
        }

        //通过按纽选择查询多少期
        else
        {
            if (num >= dt.Rows.Count)
            {
                num = dt.Rows.Count;
            }

            int ii = dt.Rows.Count;                           //总记录数
            int jj = ii - num;

            DataRow[] drr = dt.Select("id > " + jj + " and id <= " + ii);

            foreach (DataRow rr in drr)
            {
                dt1.Rows.Add(rr.ItemArray);
            }
        }

        return dt1;
    }

    //4星 走势图

    public DataTable CQSSC_4X_ZHFBZST_Select(int num, string str1, string str2, ref string result)
    {
        DataTable dt = null;
        DataTable dt1 = null;

        dt = Shove._Web.Cache.GetCacheAsDataTable("CQSSC_4X_ZHFBZST_Select");

        if (dt == null || dt.Rows.Count < 1)
        {
            DataSet ds = null;
            DAL.Procedures.P_TrendChart_CQSSC_4XZHFBZST(ref ds);

            if (ds == null || ds.Tables.Count < 1)
            {
                PF.GoError(ErrorNumber.Unknow, "参数错误或者数据库--无数据", this.GetType().BaseType.FullName);

                return dt1;
            }
            else
            {
                Shove._Web.Cache.SetCache("CQSSC_4X_ZHFBZST_Select", ds.Tables[0]);

                dt = ds.Tables[0];
            }
        }

        dt1 = dt.Clone();

        //通过期号来查询
        if (num == 0)
        {
            int i = Shove._Convert.StrToInt(str1, 0);
            int j = Shove._Convert.StrToInt(str2, 0);

            DataRow[] dr2 = dt.Select("Isuse>=" + i + " and  Isuse<=" + j);

            foreach (DataRow r in dr2)
            {
                dt1.Rows.Add(r.ItemArray);
            }
        }

        //通过按纽选择查询多少期
        else
        {
            if (num >= dt.Rows.Count)
            {
                num = dt.Rows.Count;
            }

            int ii = dt.Rows.Count;                           //总记录数
            int jj = ii - num;

            DataRow[] drr = dt.Select("id > " + jj + " and id <= " + ii);

            foreach (DataRow rr in drr)
            {
                dt1.Rows.Add(rr.ItemArray);
            }
        }

        return dt1;
    }

    //4星 走势图
    public DataTable CQSSC_4X_ZST_Select(int num, string str1, string str2, ref string result)
    {
        DataTable dt = null;
        DataTable dt1 = null;

        dt = Shove._Web.Cache.GetCacheAsDataTable("CQSSC_4X_ZST_Select");

        if (dt == null || dt.Rows.Count < 1)
        {
            DataSet ds = null;
            DAL.Procedures.P_TrendChart_CQSSC_4XZST(ref ds);

            if (ds == null || ds.Tables.Count < 1)
            {
                PF.GoError(ErrorNumber.Unknow, "参数错误或者数据库--无数据", this.GetType().BaseType.FullName);

                return dt1;
            }
            else
            {
                Shove._Web.Cache.SetCache("CQSSC_4X_ZST_Select", ds.Tables[0]);

                dt = ds.Tables[0];
            }
        }

        dt1 = dt.Clone();

        //通过期号来查询
        if (num == 0)
        {
            int i = Shove._Convert.StrToInt(str1, 0);
            int j = Shove._Convert.StrToInt(str2, 0);

            DataRow[] dr2 = dt.Select("Isuse>=" + i + " and  Isuse<=" + j);

            foreach (DataRow r in dr2)
            {
                dt1.Rows.Add(r.ItemArray);
            }
        }

        //通过按纽选择查询多少期
        else
        {
            if (num >= dt.Rows.Count)
            {
                num = dt.Rows.Count;
            }

            int ii = dt.Rows.Count;                           //总记录数
            int jj = ii - num;

            DataRow[] drr = dt.Select("id > " + jj + " and id <= " + ii);

            foreach (DataRow rr in drr)
            {
                dt1.Rows.Add(rr.ItemArray);
            }
        }

        return dt1;
    }

    //4星和值走势图
    public DataTable CQSSC_4X_HZST_Select(int num, string str1, string str2, ref string result)
    {
        DataTable dt = null;
        DataTable dt1 = null;

        dt = Shove._Web.Cache.GetCacheAsDataTable("CQSSC_4X_HZST_Select");

        if (dt == null || dt.Rows.Count < 1)
        {
            DataSet ds = null;
            DAL.Procedures.P_TrendChart_CQSSC_4XHZZST(ref ds);

            if (ds == null || ds.Tables.Count < 1)
            {
                PF.GoError(ErrorNumber.Unknow, "参数错误或者数据库--无数据", this.GetType().BaseType.FullName);

                return dt1;
            }
            else
            {
                Shove._Web.Cache.SetCache("CQSSC_4X_HZST_Select", ds.Tables[0]);

                dt = ds.Tables[0];
            }
        }

        dt1 = dt.Clone();

        //通过期号来查询
        if (num == 0)
        {
            int i = Shove._Convert.StrToInt(str1, 0);
            int j = Shove._Convert.StrToInt(str2, 0);

            DataRow[] dr2 = dt.Select("Isuse>=" + i + " and  Isuse<=" + j);

            foreach (DataRow r in dr2)
            {
                dt1.Rows.Add(r.ItemArray);
            }
        }

        //通过按纽选择查询多少期
        else
        {
            if (num >= dt.Rows.Count)
            {
                num = dt.Rows.Count;
            }

            int ii = dt.Rows.Count;                           //总记录数
            int jj = ii - num;

            DataRow[] drr = dt.Select("id > " + jj + " and id <= " + ii);

            foreach (DataRow rr in drr)
            {
                dt1.Rows.Add(rr.ItemArray);
            }
        }

        return dt1;
    }

    //4星跨度走势图
    public DataTable CQSSC_4X_KDZST_Select(int num, string str1, string str2, ref string result)
    {
        DataTable dt = null;
        DataTable dt1 = null;

        dt = Shove._Web.Cache.GetCacheAsDataTable("CQSSC_4X_KDZST_Select");

        if (dt == null || dt.Rows.Count < 1)
        {
            DataSet ds = null;
            DAL.Procedures.P_TrendChart_CQSSC_4XKDZST(ref ds);

            if (ds == null || ds.Tables.Count < 1)
            {
                PF.GoError(ErrorNumber.Unknow, "参数错误或者数据库--无数据", this.GetType().BaseType.FullName);

                return dt1;
            }
            else
            {
                Shove._Web.Cache.SetCache("CQSSC_4X_KDZST_Select", ds.Tables[0]);

                dt = ds.Tables[0];
            }
        }

        dt1 = dt.Clone();

        //通过期号来查询
        if (num == 0)
        {
            int i = Shove._Convert.StrToInt(str1, 0);
            int j = Shove._Convert.StrToInt(str2, 0);

            DataRow[] dr2 = dt.Select("Isuse>=" + i + " and  Isuse<=" + j);

            foreach (DataRow r in dr2)
            {
                dt1.Rows.Add(r.ItemArray);
            }
        }

        //通过按纽选择查询多少期
        else
        {
            if (num >= dt.Rows.Count)
            {
                num = dt.Rows.Count;
            }

            int ii = dt.Rows.Count;                           //总记录数
            int jj = ii - num;

            DataRow[] drr = dt.Select("id > " + jj + " and id <= " + ii);

            foreach (DataRow rr in drr)
            {
                dt1.Rows.Add(rr.ItemArray);
            }
        }

        return dt1;
    }

    //4星平均值走势图
    public DataTable CQSSC_4X_PJZZST_Select(int num, string str1, string str2, ref string result)
    {
        DataTable dt = null;
        DataTable dt1 = null;

        dt = Shove._Web.Cache.GetCacheAsDataTable("CQSSC_4X_PJZZST_Select");

        if (dt == null || dt.Rows.Count < 1)
        {
            DataSet ds = null;
            DAL.Procedures.P_TrendChart_CQSSC_4XPJZZST(ref ds);

            if (ds == null || ds.Tables.Count < 1)
            {
                PF.GoError(ErrorNumber.Unknow, "参数错误或者数据库--无数据", this.GetType().BaseType.FullName);

                return dt1;
            }
            else
            {
                Shove._Web.Cache.SetCache("CQSSC_4X_PJZZST_Select", ds.Tables[0]);

                dt = ds.Tables[0];
            }
        }

        dt1 = dt.Clone();

        //通过期号来查询
        if (num == 0)
        {
            int i = Shove._Convert.StrToInt(str1, 0);
            int j = Shove._Convert.StrToInt(str2, 0);

            DataRow[] dr2 = dt.Select("Isuse>=" + i + " and  Isuse<=" + j);

            foreach (DataRow r in dr2)
            {
                dt1.Rows.Add(r.ItemArray);
            }
        }

        //通过按纽选择查询多少期
        else
        {
            if (num >= dt.Rows.Count)
            {
                num = dt.Rows.Count;
            }

            int ii = dt.Rows.Count;                           //总记录数
            int jj = ii - num;

            DataRow[] drr = dt.Select("id > " + jj + " and id <= " + ii);

            foreach (DataRow rr in drr)
            {
                dt1.Rows.Add(rr.ItemArray);
            }
        }

        return dt1;
    }

    //4星大小走势图
    public DataTable CQSSC_4X_DXZST_Select(int num, string str1, string str2, ref string result)
    {
        DataTable dt = null;
        DataTable dt1 = null;

        dt = Shove._Web.Cache.GetCacheAsDataTable("CQSSC_4X_DXZST_Select");

        if (dt == null || dt.Rows.Count < 1)
        {
            DataSet ds = null;
            DAL.Procedures.P_TrendChart_CQSSC_4XDXZST(ref ds);

            if (ds == null || ds.Tables.Count < 1)
            {
                PF.GoError(ErrorNumber.Unknow, "参数错误或者数据库--无数据", this.GetType().BaseType.FullName);

                return dt1;
            }
            else
            {
                Shove._Web.Cache.SetCache("CQSSC_4X_DXZST_Select", ds.Tables[0]);

                dt = ds.Tables[0];
            }
        }

        dt1 = dt.Clone();

        //通过期号来查询
        if (num == 0)
        {
            int i = Shove._Convert.StrToInt(str1, 0);
            int j = Shove._Convert.StrToInt(str2, 0);

            DataRow[] dr2 = dt.Select("Isuse>=" + i + " and  Isuse<=" + j);

            foreach (DataRow r in dr2)
            {
                dt1.Rows.Add(r.ItemArray);
            }
        }

        //通过按纽选择查询多少期
        else
        {
            if (num >= dt.Rows.Count)
            {
                num = dt.Rows.Count;
            }

            int ii = dt.Rows.Count;                           //总记录数
            int jj = ii - num;

            DataRow[] drr = dt.Select("id > " + jj + " and id <= " + ii);

            foreach (DataRow rr in drr)
            {
                dt1.Rows.Add(rr.ItemArray);
            }
        }

        return dt1;
    }

    //4星奇偶走势图
    public DataTable CQSSC_4X_JOZST_Select(int num, string str1, string str2, ref string result)
    {
        DataTable dt = null;
        DataTable dt1 = null;

        dt = Shove._Web.Cache.GetCacheAsDataTable("CQSSC_4X_JOZST_Select");

        if (dt == null || dt.Rows.Count < 1)
        {
            DataSet ds = null;
            DAL.Procedures.P_TrendChart_CQSSC_4XJOZST(ref ds);

            if (ds == null || ds.Tables.Count < 1)
            {
                PF.GoError(ErrorNumber.Unknow, "参数错误或者数据库--无数据", this.GetType().BaseType.FullName);

                return dt1;
            }
            else
            {
                Shove._Web.Cache.SetCache("CQSSC_4X_JOZST_Select", ds.Tables[0]);

                dt = ds.Tables[0];
            }
        }

        dt1 = dt.Clone();

        //通过期号来查询
        if (num == 0)
        {
            int i = Shove._Convert.StrToInt(str1, 0);
            int j = Shove._Convert.StrToInt(str2, 0);

            DataRow[] dr2 = dt.Select("Isuse>=" + i + " and  Isuse<=" + j);

            foreach (DataRow r in dr2)
            {
                dt1.Rows.Add(r.ItemArray);
            }
        }

        //通过按纽选择查询多少期
        else
        {
            if (num >= dt.Rows.Count)
            {
                num = dt.Rows.Count;
            }

            int ii = dt.Rows.Count;                           //总记录数
            int jj = ii - num;

            DataRow[] drr = dt.Select("id > " + jj + " and id <= " + ii);

            foreach (DataRow rr in drr)
            {
                dt1.Rows.Add(rr.ItemArray);
            }
        }

        return dt1;
    }

    //4星质合走势图
    public DataTable CQSSC_4X_ZHZST_Select(int num, string str1, string str2, ref string result)
    {
        DataTable dt = null;
        DataTable dt1 = null;

        dt = Shove._Web.Cache.GetCacheAsDataTable("CQSSC_4X_ZHZST_Select");

        if (dt == null || dt.Rows.Count < 1)
        {
            DataSet ds = null;
            DAL.Procedures.P_TrendChart_CQSSC_4XZHZST(ref ds);

            if (ds == null || ds.Tables.Count < 1)
            {
                PF.GoError(ErrorNumber.Unknow, "参数错误或者数据库--无数据", this.GetType().BaseType.FullName);

                return dt1;
            }
            else
            {
                Shove._Web.Cache.SetCache("CQSSC_4X_ZHZST_Select", ds.Tables[0]);

                dt = ds.Tables[0];
            }
        }

        dt1 = dt.Clone();

        //通过期号来查询
        if (num == 0)
        {
            int i = Shove._Convert.StrToInt(str1, 0);
            int j = Shove._Convert.StrToInt(str2, 0);

            DataRow[] dr2 = dt.Select("Isuse>=" + i + " and  Isuse<=" + j);

            foreach (DataRow r in dr2)
            {
                dt1.Rows.Add(r.ItemArray);
            }
        }

        //通过按纽选择查询多少期
        else
        {
            if (num >= dt.Rows.Count)
            {
                num = dt.Rows.Count;
            }

            int ii = dt.Rows.Count;                           //总记录数
            int jj = ii - num;

            DataRow[] drr = dt.Select("id > " + jj + " and id <= " + ii);

            foreach (DataRow rr in drr)
            {
                dt1.Rows.Add(rr.ItemArray);
            }
        }

        return dt1;
    }

    //-----------------------------------------------------------------------------------------------------

    //3星 走势图

    public DataTable CQSSC_3X_ZHFBZST_Select(int num, string str1, string str2, ref string result)
    {
        DataTable dt = null;
        DataTable dt1 = null;

        dt = Shove._Web.Cache.GetCacheAsDataTable("CQSSC_3X_ZHFBZST_Select");

        if (dt == null || dt.Rows.Count < 1)
        {
            DataSet ds = null;
            DAL.Procedures.P_TrendChart_CQSSC_3XZHFBZST(ref ds);

            if (ds == null || ds.Tables.Count < 1)
            {
                PF.GoError(ErrorNumber.Unknow, "参数错误或者数据库--无数据", this.GetType().BaseType.FullName);

                return dt1;
            }
            else
            {
                Shove._Web.Cache.SetCache("CQSSC_3X_ZHFBZST_Select", ds.Tables[0]);

                dt = ds.Tables[0];
            }
        }

        dt1 = dt.Clone();

        //通过期号来查询
        if (num == 0)
        {
            int i = Shove._Convert.StrToInt(str1, 0);
            int j = Shove._Convert.StrToInt(str2, 0);

            DataRow[] dr2 = dt.Select("Isuse>=" + i + " and  Isuse<=" + j);

            foreach (DataRow r in dr2)
            {
                dt1.Rows.Add(r.ItemArray);
            }
        }

        //通过按纽选择查询多少期
        else
        {
            if (num >= dt.Rows.Count)
            {
                num = dt.Rows.Count;
            }

            int ii = dt.Rows.Count;                           //总记录数
            int jj = ii - num;

            DataRow[] drr = dt.Select("id > " + jj + " and id <= " + ii);

            foreach (DataRow rr in drr)
            {
                dt1.Rows.Add(rr.ItemArray);
            }
        }

        return dt1;
    }

    //3星 走势图
    public DataTable CQSSC_3X_ZST_Select(int num, string str1, string str2, ref string result)
    {
        DataTable dt = null;
        DataTable dt1 = null;

        dt = Shove._Web.Cache.GetCacheAsDataTable("CQSSC_3X_ZST_Select");

        if (dt == null || dt.Rows.Count < 1)
        {
            DataSet ds = null;
            DAL.Procedures.P_TrendChart_CQSSC_3XZST(ref ds);

            if (ds == null || ds.Tables.Count < 1)
            {
                PF.GoError(ErrorNumber.Unknow, "参数错误或者数据库--无数据", this.GetType().BaseType.FullName);

                return dt1;
            }
            else
            {
                Shove._Web.Cache.SetCache("CQSSC_3X_ZST_Select", ds.Tables[0]);

                dt = ds.Tables[0];
            }
        }

        dt1 = dt.Clone();

        //通过期号来查询
        if (num == 0)
        {
            int i = Shove._Convert.StrToInt(str1, 0);
            int j = Shove._Convert.StrToInt(str2, 0);

            DataRow[] dr2 = dt.Select("Isuse>=" + i + " and  Isuse<=" + j);

            foreach (DataRow r in dr2)
            {
                dt1.Rows.Add(r.ItemArray);
            }
        }

        //通过按纽选择查询多少期
        else
        {
            if (num >= dt.Rows.Count)
            {
                num = dt.Rows.Count;
            }

            int ii = dt.Rows.Count;                           //总记录数
            int jj = ii - num;

            DataRow[] drr = dt.Select("id > " + jj + " and id <= " + ii);

            foreach (DataRow rr in drr)
            {
                dt1.Rows.Add(rr.ItemArray);
            }
        }

        return dt1;
    }

    //3星和值走势图
    public DataTable CQSSC_3X_HZST_Select(int num, string str1, string str2, ref string result)
    {
        DataTable dt = null;
        DataTable dt1 = null;

        dt = Shove._Web.Cache.GetCacheAsDataTable("CQSSC_3X_HZST_Select");

        if (dt == null || dt.Rows.Count < 1)
        {
            DataSet ds = null;
            DAL.Procedures.P_TrendChart_CQSSC_3XHZZST(ref ds);

            if (ds == null || ds.Tables.Count < 1)
            {
                PF.GoError(ErrorNumber.Unknow, "参数错误或者数据库--无数据", this.GetType().BaseType.FullName);

                return dt1;
            }
            else
            {
                Shove._Web.Cache.SetCache("CQSSC_3X_HZST_Select", ds.Tables[0]);

                dt = ds.Tables[0];
            }
        }

        dt1 = dt.Clone();

        //通过期号来查询
        if (num == 0)
        {
            int i = Shove._Convert.StrToInt(str1, 0);
            int j = Shove._Convert.StrToInt(str2, 0);

            DataRow[] dr2 = dt.Select("Isuse>=" + i + " and  Isuse<=" + j);

            foreach (DataRow r in dr2)
            {
                dt1.Rows.Add(r.ItemArray);
            }
        }

        //通过按纽选择查询多少期
        else
        {
            if (num >= dt.Rows.Count)
            {
                num = dt.Rows.Count;
            }

            int ii = dt.Rows.Count;                           //总记录数
            int jj = ii - num;

            DataRow[] drr = dt.Select("id > " + jj + " and id <= " + ii);

            foreach (DataRow rr in drr)
            {
                dt1.Rows.Add(rr.ItemArray);
            }
        }

        return dt1;
    }

    //三星和值尾走势图；
    public DataTable CQSSC_3X_HZWST_Select(int num, string str1, string str2, ref string result)
    {
        DataTable dt = null;
        DataTable dt1 = null;

        dt = Shove._Web.Cache.GetCacheAsDataTable("CQSSC_3X_HZWST_Select");

        if (dt == null || dt.Rows.Count < 1)
        {
            DataSet ds = null;
            DAL.Procedures.P_TrendChart_CQSSC_3XHZWZST(ref ds);

            if (ds == null || ds.Tables.Count < 1)
            {
                PF.GoError(ErrorNumber.Unknow, "参数错误或者数据库--无数据", this.GetType().BaseType.FullName);

                return dt1;
            }
            else
            {
                Shove._Web.Cache.SetCache("CQSSC_3X_HZWST_Select", ds.Tables[0]);

                dt = ds.Tables[0];
            }
        }

        dt1 = dt.Clone();

        //通过期号来查询
        if (num == 0)
        {
            int i = Shove._Convert.StrToInt(str1, 0);
            int j = Shove._Convert.StrToInt(str2, 0);

            DataRow[] dr2 = dt.Select("Isuse>=" + i + " and  Isuse<=" + j);

            foreach (DataRow r in dr2)
            {
                dt1.Rows.Add(r.ItemArray);
            }
        }

        //通过按纽选择查询多少期
        else
        {
            if (num >= dt.Rows.Count)
            {
                num = dt.Rows.Count;
            }

            int ii = dt.Rows.Count;                           //总记录数
            int jj = ii - num;

            DataRow[] drr = dt.Select("id > " + jj + " and id <= " + ii);

            foreach (DataRow rr in drr)
            {
                dt1.Rows.Add(rr.ItemArray);
            }
        }

        return dt1;
    }

    //三星跨度走势图
    public DataTable CQSSC_3X_KDZST_Select(int num, string str1, string str2, ref string result)
    {
        DataTable dt = null;
        DataTable dt1 = null;

        dt = Shove._Web.Cache.GetCacheAsDataTable("CQSSC_3X_KDZST_Select");

        if (dt == null || dt.Rows.Count < 1)
        {
            DataSet ds = null;
            DAL.Procedures.P_TrendChart_CQSSC_3XKDZST(ref ds);

            if (ds == null || ds.Tables.Count < 1)
            {
                PF.GoError(ErrorNumber.Unknow, "参数错误或者数据库--无数据", this.GetType().BaseType.FullName);

                return dt1;
            }
            else
            {
                Shove._Web.Cache.SetCache("CQSSC_3X_KDZST_Select", ds.Tables[0]);

                dt = ds.Tables[0];
            }
        }

        dt1 = dt.Clone();

        //通过期号来查询
        if (num == 0)
        {
            int i = Shove._Convert.StrToInt(str1, 0);
            int j = Shove._Convert.StrToInt(str2, 0);

            DataRow[] dr2 = dt.Select("Isuse>=" + i + " and  Isuse<=" + j);

            foreach (DataRow r in dr2)
            {
                dt1.Rows.Add(r.ItemArray);
            }
        }

        //通过按纽选择查询多少期
        else
        {
            if (num >= dt.Rows.Count)
            {
                num = dt.Rows.Count;
            }

            int ii = dt.Rows.Count;                           //总记录数
            int jj = ii - num;

            DataRow[] drr = dt.Select("id > " + jj + " and id <= " + ii);

            foreach (DataRow rr in drr)
            {
                dt1.Rows.Add(rr.ItemArray);
            }
        }

        return dt1;
    }

    //3星大小走势图
    public DataTable CQSSC_3X_DXZST_Select(int num, string str1, string str2, ref string result)
    {
        DataTable dt = null;
        DataTable dt1 = null;

        dt = Shove._Web.Cache.GetCacheAsDataTable("CQSSC_3X_DXZST_Select");

        if (dt == null || dt.Rows.Count < 1)
        {
            DataSet ds = null;
            DAL.Procedures.P_TrendChart_CQSSC_3XDXZST(ref ds);

            if (ds == null || ds.Tables.Count < 1)
            {
                PF.GoError(ErrorNumber.Unknow, "参数错误或者数据库--无数据", this.GetType().BaseType.FullName);

                return dt1;
            }
            else
            {
                Shove._Web.Cache.SetCache("CQSSC_3X_DXZST_Select", ds.Tables[0]);

                dt = ds.Tables[0];
            }
        }

        dt1 = dt.Clone();

        //通过期号来查询
        if (num == 0)
        {
            int i = Shove._Convert.StrToInt(str1, 0);
            int j = Shove._Convert.StrToInt(str2, 0);

            DataRow[] dr2 = dt.Select("Isuse>=" + i + " and  Isuse<=" + j);

            foreach (DataRow r in dr2)
            {
                dt1.Rows.Add(r.ItemArray);
            }
        }

        //通过按纽选择查询多少期
        else
        {
            if (num >= dt.Rows.Count)
            {
                num = dt.Rows.Count;
            }

            int ii = dt.Rows.Count;                           //总记录数
            int jj = ii - num;

            DataRow[] drr = dt.Select("id > " + jj + " and id <= " + ii);

            foreach (DataRow rr in drr)
            {
                dt1.Rows.Add(rr.ItemArray);
            }
        }

        return dt1;
    }

    //3星奇偶走势图
    public DataTable CQSSC_3X_JOZST_Select(int num, string str1, string str2, ref string result)
    {
        DataTable dt = null;
        DataTable dt1 = null;

        dt = Shove._Web.Cache.GetCacheAsDataTable("CQSSC_3X_JOZST_Select");

        if (dt == null || dt.Rows.Count < 1)
        {
            DataSet ds = null;
            DAL.Procedures.P_TrendChart_CQSSC_3XJOZST(ref ds);

            if (ds == null || ds.Tables.Count < 1)
            {
                PF.GoError(ErrorNumber.Unknow, "参数错误或者数据库--无数据", this.GetType().BaseType.FullName);

                return dt1;
            }
            else
            {
                Shove._Web.Cache.SetCache("CQSSC_3X_JOZST_Select", ds.Tables[0]);

                dt = ds.Tables[0];
            }
        }

        dt1 = dt.Clone();

        //通过期号来查询
        if (num == 0)
        {
            int i = Shove._Convert.StrToInt(str1, 0);
            int j = Shove._Convert.StrToInt(str2, 0);

            DataRow[] dr2 = dt.Select("Isuse>=" + i + " and  Isuse<=" + j);

            foreach (DataRow r in dr2)
            {
                dt1.Rows.Add(r.ItemArray);
            }
        }

        //通过按纽选择查询多少期
        else
        {
            if (num >= dt.Rows.Count)
            {
                num = dt.Rows.Count;
            }

            int ii = dt.Rows.Count;                           //总记录数
            int jj = ii - num;

            DataRow[] drr = dt.Select("id > " + jj + " and id <= " + ii);

            foreach (DataRow rr in drr)
            {
                dt1.Rows.Add(rr.ItemArray);
            }
        }

        return dt1;
    }

    //3星单选012走势图
    public DataTable CQSSC_3X_DX_012_ZST_Select(int num, string str1, string str2, ref string result)
    {
        DataTable dt = null;
        DataTable dt1 = null;

        dt = Shove._Web.Cache.GetCacheAsDataTable("CQSSC_3X_DX_012_ZST_Select");

        if (dt == null || dt.Rows.Count < 1)
        {
            DataSet ds = null;
            DAL.Procedures.P_TrendChart_CQSSC_3X_DX012_ZST(ref ds);

            if (ds == null || ds.Tables.Count < 1)
            {
                PF.GoError(ErrorNumber.Unknow, "参数错误或者数据库--无数据", this.GetType().BaseType.FullName);

                return dt1;
            }
            else
            {
                Shove._Web.Cache.SetCache("CQSSC_3X_DX_012_ZST_Select", ds.Tables[0]);

                dt = ds.Tables[0];
            }
        }

        dt1 = dt.Clone();

        //通过期号来查询
        if (num == 0)
        {
            int i = Shove._Convert.StrToInt(str1, 0);
            int j = Shove._Convert.StrToInt(str2, 0);

            DataRow[] dr2 = dt.Select("Isuse>=" + i + " and  Isuse<=" + j);

            foreach (DataRow r in dr2)
            {
                dt1.Rows.Add(r.ItemArray);
            }
        }

        //通过按纽选择查询多少期
        else
        {
            if (num >= dt.Rows.Count)
            {
                num = dt.Rows.Count;
            }

            int ii = dt.Rows.Count;                           //总记录数
            int jj = ii - num;

            DataRow[] drr = dt.Select("id > " + jj + " and id <= " + ii);

            foreach (DataRow rr in drr)
            {
                dt1.Rows.Add(rr.ItemArray);
            }
        }

        return dt1;
    }

    //3星组选012走势图
    public DataTable CQSSC_3X_ZX_012_ZST_Select(int num, string str1, string str2, ref string result)
    {
        DataTable dt = null;
        DataTable dt1 = null;

        dt = Shove._Web.Cache.GetCacheAsDataTable("CQSSC_3X_ZX_012_ZST_Select");

        if (dt == null || dt.Rows.Count < 1)
        {
            DataSet ds = null;
            DAL.Procedures.P_TrendChart_CQSSC_3X_ZX012_ZST(ref ds);

            if (ds == null || ds.Tables.Count < 1)
            {
                PF.GoError(ErrorNumber.Unknow, "参数错误或者数据库--无数据", this.GetType().BaseType.FullName);

                return dt1;
            }
            else
            {
                Shove._Web.Cache.SetCache("CQSSC_3X_ZX_012_ZST_Select", ds.Tables[0]);

                dt = ds.Tables[0];
            }
        }

        dt1 = dt.Clone();

        //通过期号来查询
        if (num == 0)
        {
            int i = Shove._Convert.StrToInt(str1, 0);
            int j = Shove._Convert.StrToInt(str2, 0);

            DataRow[] dr2 = dt.Select("Isuse>=" + i + " and  Isuse<=" + j);

            foreach (DataRow r in dr2)
            {
                dt1.Rows.Add(r.ItemArray);
            }
        }

        //通过按纽选择查询多少期
        else
        {
            if (num >= dt.Rows.Count)
            {
                num = dt.Rows.Count;
            }

            int ii = dt.Rows.Count;                           //总记录数
            int jj = ii - num;

            DataRow[] drr = dt.Select("id > " + jj + " and id <= " + ii);

            foreach (DataRow rr in drr)
            {
                dt1.Rows.Add(rr.ItemArray);
            }
        }

        return dt1;
    }

    //3星质合走势图
    public DataTable CQSSC_3X_ZHZST_Select(int num, string str1, string str2, ref string result)
    {
        DataTable dt = null;
        DataTable dt1 = null;

        dt = Shove._Web.Cache.GetCacheAsDataTable("CQSSC_3X_ZHZST_Select");

        if (dt == null || dt.Rows.Count < 1)
        {
            DataSet ds = null;
            DAL.Procedures.P_TrendChart_CQSSC_3XZHZST(ref ds);

            if (ds == null || ds.Tables.Count < 1)
            {
                PF.GoError(ErrorNumber.Unknow, "参数错误或者数据库--无数据", this.GetType().BaseType.FullName);

                return dt1;
            }
            else
            {
                Shove._Web.Cache.SetCache("CQSSC_3X_ZHZST_Select", ds.Tables[0]);

                dt = ds.Tables[0];
            }
        }

        dt1 = dt.Clone();

        //通过期号来查询
        if (num == 0)
        {
            int i = Shove._Convert.StrToInt(str1, 0);
            int j = Shove._Convert.StrToInt(str2, 0);

            DataRow[] dr2 = dt.Select("Isuse>=" + i + " and  Isuse<=" + j);

            foreach (DataRow r in dr2)
            {
                dt1.Rows.Add(r.ItemArray);
            }
        }

        //通过按纽选择查询多少期
        else
        {
            if (num >= dt.Rows.Count)
            {
                num = dt.Rows.Count;
            }

            int ii = dt.Rows.Count;                           //总记录数
            int jj = ii - num;

            DataRow[] drr = dt.Select("id > " + jj + " and id <= " + ii);

            foreach (DataRow rr in drr)
            {
                dt1.Rows.Add(rr.ItemArray);
            }
        }

        return dt1;
    }

    //---------------------------------------------------------------------------------------------------------------------

    //2星标准综合走势图

    public DataTable CQSSC_2X_ZHFBZST_Select(int num, string str1, string str2, ref string result)
    {
        DataTable dt = null;
        DataTable dt1 = null;

        dt = Shove._Web.Cache.GetCacheAsDataTable("CQSSC_2X_ZHFBZST_Select");

        if (dt == null || dt.Rows.Count < 1)
        {
            DataSet ds = null;
            DAL.Procedures.P_TrendChart_CQSSC_2XZHFBZST(ref ds);

            if (ds == null || ds.Tables.Count < 1)
            {
                PF.GoError(ErrorNumber.Unknow, "参数错误或者数据库--无数据", this.GetType().BaseType.FullName);

                return dt1;
            }
            else
            {
                Shove._Web.Cache.SetCache("CQSSC_2X_ZHFBZST_Select", ds.Tables[0]);

                dt = ds.Tables[0];
            }
        }

        dt1 = dt.Clone();

        //通过期号来查询
        if (num == 0)
        {
            int i = Shove._Convert.StrToInt(str1, 0);
            int j = Shove._Convert.StrToInt(str2, 0);

            DataRow[] dr2 = dt.Select("Isuse>=" + i + " and  Isuse<=" + j);

            foreach (DataRow r in dr2)
            {
                dt1.Rows.Add(r.ItemArray);
            }
        }

        //通过按纽选择查询多少期
        else
        {
            if (num >= dt.Rows.Count)
            {
                num = dt.Rows.Count;
            }

            int ii = dt.Rows.Count;                           //总记录数
            int jj = ii - num;

            DataRow[] drr = dt.Select("id > " + jj + " and id <= " + ii);

            foreach (DataRow rr in drr)
            {
                dt1.Rows.Add(rr.ItemArray);
            }
        }

        return dt1;
    }

    //2星和值走势图
    public DataTable CQSSC_2X_HZZST_Select(int num, string str1, string str2, ref string result)
    {
        DataTable dt = null;
        DataTable dt1 = null;

        dt = Shove._Web.Cache.GetCacheAsDataTable("CQSSC_2X_HZZST_Select");

        if (dt == null || dt.Rows.Count < 1)
        {
            DataSet ds = null;
            DAL.Procedures.P_TrendChart_CQSSC_2XHZZST(ref ds);

            if (ds == null || ds.Tables.Count < 1)
            {
                PF.GoError(ErrorNumber.Unknow, "参数错误或者数据库--无数据", this.GetType().BaseType.FullName);

                return dt1;
            }
            else
            {
                Shove._Web.Cache.SetCache("CQSSC_2X_HZZST_Select", ds.Tables[0]);

                dt = ds.Tables[0];
            }
        }

        dt1 = dt.Clone();

        //通过期号来查询
        if (num == 0)
        {
            int i = Shove._Convert.StrToInt(str1, 0);
            int j = Shove._Convert.StrToInt(str2, 0);

            DataRow[] dr2 = dt.Select("Isuse>=" + i + " and  Isuse<=" + j);

            foreach (DataRow r in dr2)
            {
                dt1.Rows.Add(r.ItemArray);
            }
        }

        //通过按纽选择查询多少期
        else
        {
            if (num >= dt.Rows.Count)
            {
                num = dt.Rows.Count;
            }

            int ii = dt.Rows.Count;                           //总记录数
            int jj = ii - num;

            DataRow[] drr = dt.Select("id > " + jj + " and id <= " + ii);

            foreach (DataRow rr in drr)
            {
                dt1.Rows.Add(rr.ItemArray);
            }
        }

        return dt1;
    }

    //二星和尾走势图
    public DataTable CQSSC_2X_HZWZST_Select(int num, string str1, string str2, ref string result)
    {
        DataTable dt = null;
        DataTable dt1 = null;

        dt = Shove._Web.Cache.GetCacheAsDataTable("CQSSC_2X_HZWZST_Select");

        if (dt == null || dt.Rows.Count < 1)
        {
            DataSet ds = null;
            DAL.Procedures.P_TrendChart_CQSSC_2XHZWZST(ref ds);

            if (ds == null || ds.Tables.Count < 1)
            {
                PF.GoError(ErrorNumber.Unknow, "参数错误或者数据库--无数据", this.GetType().BaseType.FullName);

                return dt1;
            }
            else
            {
                Shove._Web.Cache.SetCache("CQSSC_2X_HZWZST_Select", ds.Tables[0]);

                dt = ds.Tables[0];
            }
        }

        dt1 = dt.Clone();

        //通过期号来查询
        if (num == 0)
        {
            int i = Shove._Convert.StrToInt(str1, 0);
            int j = Shove._Convert.StrToInt(str2, 0);

            DataRow[] dr2 = dt.Select("Isuse>=" + i + " and  Isuse<=" + j);

            foreach (DataRow r in dr2)
            {
                dt1.Rows.Add(r.ItemArray);
            }
        }

        //通过按纽选择查询多少期
        else
        {
            if (num >= dt.Rows.Count)
            {
                num = dt.Rows.Count;
            }

            int ii = dt.Rows.Count;                           //总记录数
            int jj = ii - num;

            DataRow[] drr = dt.Select("id > " + jj + " and id <= " + ii);

            foreach (DataRow rr in drr)
            {
                dt1.Rows.Add(rr.ItemArray);
            }
        }

        return dt1;
    }

    //二星均值走势图
    public DataTable CQSSC_2X_PJZZST_Select(int num, string str1, string str2, ref string result)
    {
        DataTable dt = null;
        DataTable dt1 = null;

        dt = Shove._Web.Cache.GetCacheAsDataTable("CQSSC_2X_PJZZST_Select");

        if (dt == null || dt.Rows.Count < 1)
        {
            DataSet ds = null;
            DAL.Procedures.P_TrendChart_CQSSC_2XPJZZST(ref ds);

            if (ds == null || ds.Tables.Count < 1)
            {
                PF.GoError(ErrorNumber.Unknow, "参数错误或者数据库--无数据", this.GetType().BaseType.FullName);

                return dt1;
            }
            else
            {
                Shove._Web.Cache.SetCache("CQSSC_2X_PJZZST_Select", ds.Tables[0]);

                dt = ds.Tables[0];
            }
        }

        dt1 = dt.Clone();

        //通过期号来查询
        if (num == 0)
        {
            int i = Shove._Convert.StrToInt(str1, 0);
            int j = Shove._Convert.StrToInt(str2, 0);

            DataRow[] dr2 = dt.Select("Isuse>=" + i + " and  Isuse<=" + j);

            foreach (DataRow r in dr2)
            {
                dt1.Rows.Add(r.ItemArray);
            }
        }

        //通过按纽选择查询多少期
        else
        {
            if (num >= dt.Rows.Count)
            {
                num = dt.Rows.Count;
            }

            int ii = dt.Rows.Count;                           //总记录数
            int jj = ii - num;

            DataRow[] drr = dt.Select("id > " + jj + " and id <= " + ii);

            foreach (DataRow rr in drr)
            {
                dt1.Rows.Add(rr.ItemArray);
            }
        }

        return dt1;
    }

    //二星跨度走势图
    public DataTable CQSSC_2X_KDZST_Select(int num, string str1, string str2, ref string result)
    {
        DataTable dt = null;
        DataTable dt1 = null;

        dt = Shove._Web.Cache.GetCacheAsDataTable("CQSSC_2X_KDZST_Select");

        if (dt == null || dt.Rows.Count < 1)
        {
            DataSet ds = null;
            DAL.Procedures.P_TrendChart_CQSSC_2XKDZST(ref ds);

            if (ds == null || ds.Tables.Count < 1)
            {
                PF.GoError(ErrorNumber.Unknow, "参数错误或者数据库--无数据", this.GetType().BaseType.FullName);

                return dt1;
            }
            else
            {
                Shove._Web.Cache.SetCache("CQSSC_2X_KDZST_Select", ds.Tables[0]);

                dt = ds.Tables[0];
            }
        }

        dt1 = dt.Clone();

        //通过期号来查询
        if (num == 0)
        {
            int i = Shove._Convert.StrToInt(str1, 0);
            int j = Shove._Convert.StrToInt(str2, 0);

            DataRow[] dr2 = dt.Select("Isuse>=" + i + " and  Isuse<=" + j);

            foreach (DataRow r in dr2)
            {
                dt1.Rows.Add(r.ItemArray);
            }
        }

        //通过按纽选择查询多少期
        else
        {
            if (num >= dt.Rows.Count)
            {
                num = dt.Rows.Count;
            }

            int ii = dt.Rows.Count;                           //总记录数
            int jj = ii - num;

            DataRow[] drr = dt.Select("id > " + jj + " and id <= " + ii);

            foreach (DataRow rr in drr)
            {
                dt1.Rows.Add(rr.ItemArray);
            }
        }

        return dt1;
    }

    //二星012路走势图
    public DataTable CQSSC_2X_012ZST_Select(int num, string str1, string str2, ref string result)
    {
        DataTable dt = null;
        DataTable dt1 = null;

        dt = Shove._Web.Cache.GetCacheAsDataTable("CQSSC_2X_012ZST_Select");

        if (dt == null || dt.Rows.Count < 1)
        {
            DataSet ds = null;
            DAL.Procedures.P_TrendChart_CQSSC_2X_012_ZST(ref ds);

            if (ds == null || ds.Tables.Count < 1)
            {
                PF.GoError(ErrorNumber.Unknow, "参数错误或者数据库--无数据", this.GetType().BaseType.FullName);

                return dt1;
            }
            else
            {
                Shove._Web.Cache.SetCache("CQSSC_2X_012ZST_Select", ds.Tables[0]);

                dt = ds.Tables[0];
            }
        }

        dt1 = dt.Clone();

        //通过期号来查询
        if (num == 0)
        {
            int i = Shove._Convert.StrToInt(str1, 0);
            int j = Shove._Convert.StrToInt(str2, 0);

            DataRow[] dr2 = dt.Select("Isuse>=" + i + " and  Isuse<=" + j);

            foreach (DataRow r in dr2)
            {
                dt1.Rows.Add(r.ItemArray);
            }
        }

        //通过按纽选择查询多少期
        else
        {
            if (num >= dt.Rows.Count)
            {
                num = dt.Rows.Count;
            }

            int ii = dt.Rows.Count;                           //总记录数
            int jj = ii - num;

            DataRow[] drr = dt.Select("id > " + jj + " and id <= " + ii);

            foreach (DataRow rr in drr)
            {
                dt1.Rows.Add(rr.ItemArray);
            }
        }

        return dt1;
    }

    //二星最大值走势图
    public DataTable CQSSC_2X_MAXZST_Select(int num, string str1, string str2, ref string result)
    {
        DataTable dt = null;
        DataTable dt1 = null;

        dt = Shove._Web.Cache.GetCacheAsDataTable("CQSSC_2X_MAXZST_Select");

        if (dt == null || dt.Rows.Count < 1)
        {
            DataSet ds = null;
            DAL.Procedures.P_TrendChart_CQSSC_2XMaxZST(ref ds);

            if (ds == null || ds.Tables.Count < 1)
            {
                PF.GoError(ErrorNumber.Unknow, "参数错误或者数据库--无数据", this.GetType().BaseType.FullName);

                return dt1;
            }
            else
            {
                Shove._Web.Cache.SetCache("CQSSC_2X_MAXZST_Select", ds.Tables[0]);

                dt = ds.Tables[0];
            }
        }

        dt1 = dt.Clone();

        //通过期号来查询
        if (num == 0)
        {
            int i = Shove._Convert.StrToInt(str1, 0);
            int j = Shove._Convert.StrToInt(str2, 0);

            DataRow[] dr2 = dt.Select("Isuse>=" + i + " and  Isuse<=" + j);

            foreach (DataRow r in dr2)
            {
                dt1.Rows.Add(r.ItemArray);
            }
        }

        //通过按纽选择查询多少期
        else
        {
            if (num >= dt.Rows.Count)
            {
                num = dt.Rows.Count;
            }

            int ii = dt.Rows.Count;                           //总记录数
            int jj = ii - num;

            DataRow[] drr = dt.Select("id > " + jj + " and id <= " + ii);

            foreach (DataRow rr in drr)
            {
                dt1.Rows.Add(rr.ItemArray);
            }
        }

        return dt1;
    }

    //二星最小值走势图
    public DataTable CQSSC_2X_MinZST_Select(int num, string str1, string str2, ref string result)
    {
        DataTable dt = null;
        DataTable dt1 = null;

        dt = Shove._Web.Cache.GetCacheAsDataTable("CQSSC_2X_MinZST_Select");

        if (dt == null || dt.Rows.Count < 1)
        {
            DataSet ds = null;
            DAL.Procedures.P_TrendChart_CQSSC_2XMINZST(ref ds);

            if (ds == null || ds.Tables.Count < 1)
            {
                PF.GoError(ErrorNumber.Unknow, "参数错误或者数据库--无数据", this.GetType().BaseType.FullName);

                return dt1;
            }
            else
            {
                Shove._Web.Cache.SetCache("CQSSC_2X_MinZST_Select", ds.Tables[0]);

                dt = ds.Tables[0];
            }
        }

        dt1 = dt.Clone();

        //通过期号来查询
        if (num == 0)
        {
            int i = Shove._Convert.StrToInt(str1, 0);
            int j = Shove._Convert.StrToInt(str2, 0);

            DataRow[] dr2 = dt.Select("Isuse>=" + i + " and  Isuse<=" + j);

            foreach (DataRow r in dr2)
            {
                dt1.Rows.Add(r.ItemArray);
            }
        }

        //通过按纽选择查询多少期
        else
        {
            if (num >= dt.Rows.Count)
            {
                num = dt.Rows.Count;
            }

            int ii = dt.Rows.Count;                           //总记录数
            int jj = ii - num;

            DataRow[] drr = dt.Select("id > " + jj + " and id <= " + ii);

            foreach (DataRow rr in drr)
            {
                dt1.Rows.Add(rr.ItemArray);
            }
        }

        return dt1;
    }

    //二星大小单双走势图
    public DataTable CQSSC_2X_DXDSZST_Select(int num, string str1, string str2, ref string result)
    {
        DataTable dt = null;
        DataTable dt1 = null;

        dt = Shove._Web.Cache.GetCacheAsDataTable("CQSSC_2X_DXDSZST_Select");

        if (dt == null || dt.Rows.Count < 1)
        {
            DataSet ds = null;
            DAL.Procedures.P_TrendChart_CQSSC_2XDXDSZST(ref ds);

            if (ds == null || ds.Tables.Count < 1)
            {
                PF.GoError(ErrorNumber.Unknow, "参数错误或者数据库--无数据", this.GetType().BaseType.FullName);

                return dt1;
            }
            else
            {
                Shove._Web.Cache.SetCache("CQSSC_2X_DXDSZST_Select", ds.Tables[0]);

                dt = ds.Tables[0];
            }
        }

        dt1 = dt.Clone();

        //通过期号来查询
        if (num == 0)
        {
            int i = Shove._Convert.StrToInt(str1, 0);
            int j = Shove._Convert.StrToInt(str2, 0);

            DataRow[] dr2 = dt.Select("Isuse>=" + i + " and  Isuse<=" + j);

            foreach (DataRow r in dr2)
            {
                dt1.Rows.Add(r.ItemArray);
            }
        }

        //通过按纽选择查询多少期
        else
        {
            if (num >= dt.Rows.Count)
            {
                num = dt.Rows.Count;
            }

            int ii = dt.Rows.Count;                           //总记录数
            int jj = ii - num;

            DataRow[] drr = dt.Select("id > " + jj + " and id <= " + ii);

            foreach (DataRow rr in drr)
            {
                dt1.Rows.Add(rr.ItemArray);
            }
        }

        return dt1;
    }

    #endregion

    #region 快赢481基本走势图
    /// <summary>
    /// 快赢481 基本走势图
    /// </summary>
    /// <param name="Number">期号</param>
    /// <returns>DataTable</returns>
    public DataTable KY_481_SelectByNum(int Number)
    {
        DataSet ds = null;
        DAL.Procedures.P_TrendChart_KY481_JBZST(ref ds, Number);
        DataTable dt = ds.Tables[0];
        return dt;
    }

    #endregion

    #region 快赢481和值走势图
    /// <summary>
    /// 快赢481和值走势图
    /// </summary>
    /// <param name="Number">期号</param>
    /// <returns>DataTable</returns>
    public DataTable KY_481_HZSelectByNum(int Number)
    {
        DataSet ds = null;
        DAL.Procedures.P_TrendChart_KY481_HZZST(ref ds, Number);
        DataTable dt = ds.Tables[0];
        return dt;
    }
    #endregion

    #region 快赢481大小走势图
    /// <summary>
    /// 快赢481大小走势图
    /// </summary>
    /// <param name="Number">期号</param>
    /// <returns>DataTable</returns>
    public DataTable KY_481_DXSelectByNum(int Number)
    {
        DataSet ds = null;
        DAL.Procedures.P_TrendChart_KY481_DXZST(ref ds, Number);
        DataTable dt = ds.Tables[0];
        return dt;
    }
    #endregion

    #region 快赢481奇偶走势图
    /// <summary>
    /// 快赢481奇偶走势图
    /// </summary>
    /// <param name="Number">期号</param>
    /// <returns>DataTable</returns>
    public DataTable KY_481_JOSelectByNum(int Number)
    {
        DataSet ds = null;
        DAL.Procedures.P_TrendChart_KY481_JOZST(ref ds, Number);
        DataTable dt = ds.Tables[0];
        return dt;
    }
    #endregion

    #region 快赢481跨度走势图
    /// <summary>
    /// 快赢481跨度走势图
    /// </summary>
    /// <param name="Number">期号</param>
    /// <returns>DataTable</returns>
    public DataTable KY_481_KDSelectByNum(int Number)
    {
        DataSet ds = null;
        DAL.Procedures.P_TrendChart_KY481_KDZST(ref ds, Number);
        DataTable dt = ds.Tables[0];
        return dt;

    }
    #endregion

    #region 河南22选5号码走势图
    /// <summary>
    /// 河南22选5号码走势图
    /// </summary>
    /// <param name="Number">期号</param>
    /// <returns>DataTable</returns>
    public DataTable ZYFC_22X5_SelectByNum(int Number)
    {
        DataSet ds = null;
        DAL.Procedures.P_TrendChart_ZYFC22X5_HMZST(ref ds, Number);
        DataTable dt = ds.Tables[0];
        return dt;

    }
    #endregion
}
