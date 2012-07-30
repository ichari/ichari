using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.Sql;
using System.Data.SqlClient;

/// <summary>
/// BLL 的摘要说明
/// </summary>
public class BLL
{
    public BLL()
    {
        //
        // TODO: 在此处添加构造函数逻辑
        //
    }

    public static DataTable PL3_LH_SeleteByNum(int Number)
    {
        DataTable dt1 = Shove._Web.Cache.GetCacheAsDataTable("PL3_LH_SeleteByNum_ds11");

       if (dt1 == null)
        {
            DataSet ds = null;

            DAL.Procedures.P_TrendChart_PL3_LH(ref ds);

            dt1 = ds.Tables[0];

            Shove._Web.Cache.SetCache("PL3_LH_SeleteByNum_ds11", ds.Tables[0]);
        }

        if (Number > dt1.Rows.Count)
        {
            Number = dt1.Rows.Count;
        }

        DataTable dt = new DataTable();

        dt = dt1.Clone();//构建与DT1表具有一样表头的空表！

        int temp = Shove._Convert.StrToInt(dt1.Rows.Count.ToString(), 0);
        int tem = Shove._Convert.StrToInt(dt1.Rows.Count.ToString(), 0) - Number;

        DataRow[] drs = dt1.Select("id>" + tem.ToString() + " AND id<=" + temp.ToString());

        foreach (DataRow dr in drs)
        {
            dt.Rows.Add(dr.ItemArray);
        }

        return dt;
    }

    public static DataTable PL3_SeleteByNum(int Number)
    {
        DataTable dt1 = Shove._Web.Cache.GetCacheAsDataTable("PL3_SeleteByNum_ds11");

        //DataTable dt1 = (DataTable)HttpContext.Current.Cache["ds11"];    

        if (dt1 == null)
        {
            DataSet ds = null;

            DAL.Procedures.P_TrendChart_PL3(ref ds);

            dt1 = ds.Tables[0];

            Shove._Web.Cache.SetCache("PL3_SeleteByNum_ds11", ds.Tables[0]);

            //HttpContext.Current.Cache["ds11"] = dt1;
        }

       if ( Number > dt1.Rows.Count)
        {
            Number = dt1.Rows.Count;
        }

        DataTable dt = new DataTable();

        dt = dt1.Clone();

        //dt.Columns.Add("id");
        //dt.Columns.Add("Isuse");
        //dt.Columns.Add("LotteryNumber");
        //dt.Columns.Add("C_0");
        //dt.Columns.Add("C_1");
        //dt.Columns.Add("C_2");
        //dt.Columns.Add("C_3");
        //dt.Columns.Add("C_4");
        //dt.Columns.Add("C_5");
        //dt.Columns.Add("C_6");
        //dt.Columns.Add("C_7");
        //dt.Columns.Add("C_8");
        //dt.Columns.Add("C_9");

        int temp = Shove._Convert.StrToInt(dt1.Rows.Count.ToString(),0);
        int tem = Shove._Convert.StrToInt(dt1.Rows.Count.ToString(),0) - Number;

       DataRow[] drs = dt1.Select("id>"+tem.ToString() + " AND id<="+temp.ToString()); 

       // DataRow[] drs = dt1.Select ("id<="+Number.ToString());
        
        foreach (DataRow dr in drs)
        {
            dt.Rows.Add(dr.ItemArray);
        }

        return dt;

    }

    public static DataTable PL3_YS_SeleteByNum(int Number)
    {
        DataTable dt1 = Shove._Web.Cache.GetCacheAsDataTable("PL3_Ys_SeleteByNum_ds11");

        //DataTable dt1 = (DataTable)HttpContext.Current.Cache["ds11"];    

        if (dt1 == null)
        {
            DataSet ds = null;

            DAL.Procedures.P_TrendChart_PL3_YS(ref ds);

            dt1 = ds.Tables[0];

            Shove._Web.Cache.SetCache("PL3_Ys_SeleteByNum_ds11", ds.Tables[0]);

            //HttpContext.Current.Cache["ds11"] = dt1;
        }

        if (Number > dt1.Rows.Count)
        {
            Number = dt1.Rows.Count;
        }

        DataTable dt = new DataTable();

        dt = dt1.Clone();

        int temp = Shove._Convert.StrToInt(dt1.Rows.Count.ToString(), 0);
        int tem = Shove._Convert.StrToInt(dt1.Rows.Count.ToString(), 0) - Number;

        DataRow[] drs = dt1.Select("id>" + tem.ToString() + " AND id<=" + temp.ToString());

        // DataRow[] drs = dt1.Select ("id<="+Number.ToString());

        foreach (DataRow dr in drs)
        {
            dt.Rows.Add(dr.ItemArray);
        }

        return dt;
    }

    public static DataTable PL3_DX_SeleteByNum(int Number)
    {
        DataSet ds = null;
        DAL.Procedures.P_TrendChart_PL3_DX(ref ds, Number);
        DataTable dt = ds.Tables[0];
        return dt;
    }

    public static DataTable PL3_ZX_SeleteByNum(int Number)
    {
        DataSet ds = null;
        DAL.Procedures.P_TrendChart_PL3_ZX(ref ds, Number);
        DataTable dt = ds.Tables[0];
        return dt;
    }

    public static DataTable PL3_JO_SeleteByNum(int Number)
    {
        DataSet ds = null;
        DAL.Procedures.P_TrendChart_PL3_JO(ref ds, Number);
        DataTable dt = ds.Tables[0];
        return dt;
    }

    public static DataTable PL3_012_SeleteByNum(int Number)
    {
        DataSet ds = null;
        DAL.Procedures.P_TrendChart_PL3_012(ref ds, Number);
        DataTable dt = ds.Tables[0];
        return dt;
    }

    public static DataTable PL3_DZX_SeleteByNum(int Number)
    {
        DataSet ds = null;
        DAL.Procedures.P_TrendChart_PL3_DZX(ref ds, Number);
        DataTable dt = ds.Tables[0];
        return dt;
    }

    public static DataTable PL3_KD_SeleteByNum(int Number)
    {
        DataSet ds = null;
        DAL.Procedures.P_TrendChart_PL3_KD(ref ds, Number);
        DataTable dt = ds.Tables[0];
        return dt;
    }

    public static DataTable PL3_ZH_SeleteByNum(int Number)
    {
        DataSet ds = null;
        DAL.Procedures.P_TrendChart_PL3_ZH(ref ds, Number);
        DataTable dt = ds.Tables[0];
        return dt;
    }

    public static DataTable PL3_HMFB_SeleteByNum(int Number)
    {
        DataSet ds = null;
        DAL.Procedures.P_TrendChart_PL3_HMFB(ref ds, Number);
        DataTable dt = ds.Tables[0];
        return dt;
    }

    public static DataTable PL3_WH_SeleteByNum(int Number)
    {
        DataSet ds = null;
        DAL.Procedures.P_TrendChart_PL3_WH(ref ds, Number);
        DataTable dt = ds.Tables[0];
        return dt;
    }

    public static DataTable PL3_HZ_SeleteByNum(int Number)
    {
        DataSet ds = null;
        DAL.Procedures.P_TrendChart_PL3_HZ(ref ds, Number);
        DataTable dt = ds.Tables[0];
        return dt;
    }

    public static DataTable PL5_HMFB_SeleteByNum(int Number)
    {
        
        DataSet ds =null;
        DAL.Procedures.P_TrendChart_PL5_HMFB(ref ds,Number);
        DataTable dt = ds.Tables[0];
        return dt;
        
    }

    public static DataTable PL5_DX_SeleteByNum(int Number)
    {
      DataTable dt1 = Shove._Web.Cache.GetCacheAsDataTable("pl5_dx_1ds11");

        if (dt1 == null)
        {
            DataSet ds = null;

            DAL.Procedures.P_TrendChart_PL5_DX(ref ds);

            dt1 = ds.Tables[0];

            Shove._Web.Cache.SetCache("pl5_dx_1ds11", ds.Tables[0]);

            //HttpContext.Current.Cache["ds11"] = dt1;
        }

        if (Number > dt1.Rows.Count)
        {
            Number = dt1.Rows.Count;
        }

        DataTable dt = new DataTable();

        dt = dt1.Clone();

        int temp = Shove._Convert.StrToInt(dt1.Rows.Count.ToString(), 0);
        int tem = Shove._Convert.StrToInt(dt1.Rows.Count.ToString(), 0) - Number;

        DataRow[] drs = dt1.Select("id>" + tem.ToString() + " AND id<=" + temp.ToString());

        foreach (DataRow dr in drs)
        {
            dt.Rows.Add(dr.ItemArray);
        }

        return dt;
    }

    public static DataTable PL5_LH_SeleteByNum(int Number)
    {
        DataTable dt1 = Shove._Web.Cache.GetCacheAsDataTable("pl5_lh_AAds11");

        if (dt1 == null)
        {
            DataSet ds = null;

            DAL.Procedures.P_TrendChart_PL5_LH(ref ds);

            dt1 = ds.Tables[0];

            Shove._Web.Cache.SetCache("pl5_lh_AAds11", ds.Tables[0]);
        }

        if (Number > dt1.Rows.Count)
        {
            Number = dt1.Rows.Count;
        }

        DataTable dt = new DataTable();

        dt = dt1.Clone();//构建与DT1表具有一样表头的空表！

        int temp = Shove._Convert.StrToInt(dt1.Rows.Count.ToString(), 0);
        int tem = Shove._Convert.StrToInt(dt1.Rows.Count.ToString(), 0) - Number;

        DataRow[] drs = dt1.Select("id>" + tem.ToString() + " AND id<=" + temp.ToString());

        foreach (DataRow dr in drs)
        {
            dt.Rows.Add(dr.ItemArray);
        }

        return dt;
    }

    public static DataTable PL5_JO_SeleteByNum(int Number)
    {
        DataTable dt1 = Shove._Web.Cache.GetCacheAsDataTable("PL5_JO_SeleteByNum_ds11");

        if (dt1 == null)
        {
            DataSet ds = null;

            DAL.Procedures.P_TrendChart_PL5_JO(ref ds);

            dt1 = ds.Tables[0];

            Shove._Web.Cache.SetCache("PL5_JO_SeleteByNum_ds11", ds.Tables[0]);
        }

        if (Number > dt1.Rows.Count)
        {
            Number = dt1.Rows.Count;
        }

        DataTable dt = new DataTable();

        dt = dt1.Clone();//构建与DT1表具有一样表头的空表！

        int temp = Shove._Convert.StrToInt(dt1.Rows.Count.ToString(), 0);
        int tem = Shove._Convert.StrToInt(dt1.Rows.Count.ToString(), 0) - Number;

        DataRow[] drs = dt1.Select("id>" + tem.ToString() + " AND id<=" + temp.ToString());

        foreach (DataRow dr in drs)
        {
            dt.Rows.Add(dr.ItemArray);
        }

        return dt;
    }

    public static DataTable PL5_ZH_SeleteByNum(int Number)
    {
        DataTable dt1 = Shove._Web.Cache.GetCacheAsDataTable("pl5_ds11");

        if (dt1 == null)
        {
            DataSet ds = null;

            DAL.Procedures.P_TrendChart_PL5_ZH(ref ds);

            dt1 = ds.Tables[0];

            Shove._Web.Cache.SetCache("pl5_ds11", ds.Tables[0]);
        }

        if (Number > dt1.Rows.Count)
        {
            Number = dt1.Rows.Count;
        }

        DataTable dt = new DataTable();

        dt = dt1.Clone();//构建与DT1表具有一样表头的空表！

        int temp = Shove._Convert.StrToInt(dt1.Rows.Count.ToString(), 0);
        int tem = Shove._Convert.StrToInt(dt1.Rows.Count.ToString(), 0) - Number;

        DataRow[] drs = dt1.Select("id>" + tem.ToString() + " AND id<=" + temp.ToString());

        foreach (DataRow dr in drs)
        {
            dt.Rows.Add(dr.ItemArray);
        }

        return dt;
    }

    public static DataTable PL5_DZX_SeleteByNum(int Number)
    {
        DataTable dt1 = Shove._Web.Cache.GetCacheAsDataTable("pl5_dzx_33ds11");

        if (dt1 == null)
        {
            DataSet ds = null;

            DAL.Procedures.P_TrendChart_PL5_DZX(ref ds);

            dt1 = ds.Tables[0];

            Shove._Web.Cache.SetCache("pl5_dzx_33ds11", ds.Tables[0]);
        }

        if (Number > dt1.Rows.Count)
        {
            Number = dt1.Rows.Count;
        }

        DataTable dt = new DataTable();

        dt = dt1.Clone();//构建与DT1表具有一样表头的空表！

        int temp = Shove._Convert.StrToInt(dt1.Rows.Count.ToString(), 0);
        int tem = Shove._Convert.StrToInt(dt1.Rows.Count.ToString(), 0) - Number;

        DataRow[] drs = dt1.Select("id>" + tem.ToString() + " AND id<=" + temp.ToString());

        foreach (DataRow dr in drs)
        {
            dt.Rows.Add(dr.ItemArray);
        }

        return dt;
    }

    public static DataTable PL5_012_SeleteByNum(int Number)
    {
        DataTable dt1 = Shove._Web.Cache.GetCacheAsDataTable("PL5_012_SeleteByNum_ds11");

        if (dt1 == null)
        {
            DataSet ds = null;

            DAL.Procedures.P_TrendChart_PL5_012(ref ds);

            dt1 = ds.Tables[0];

            Shove._Web.Cache.SetCache("PL5_012_SeleteByNum_ds11", ds.Tables[0]);
        }

        if (Number > dt1.Rows.Count)
        {
            Number = dt1.Rows.Count;
        }

        DataTable dt = new DataTable();

        dt = dt1.Clone();//构建与DT1表具有一样表头的空表！

        int temp = Shove._Convert.StrToInt(dt1.Rows.Count.ToString(), 0);
        int tem = Shove._Convert.StrToInt(dt1.Rows.Count.ToString(), 0) - Number;

        DataRow[] drs = dt1.Select("id>" + tem.ToString() + " AND id<=" + temp.ToString());

        foreach (DataRow dr in drs)
        {
            dt.Rows.Add(dr.ItemArray);
        }

        return dt;
    }

    public static DataTable PL5_CF_SeleteByNum(int Number)
    {
        DataTable dt1 = Shove._Web.Cache.GetCacheAsDataTable("pl5_cf_ds11");

        if (dt1 == null)
        {
            DataSet ds = null;

            DAL.Procedures.P_TrendChart_PL5_CF(ref ds);

            dt1 = ds.Tables[0];

            Shove._Web.Cache.SetCache("pl5_cf_ds11", ds.Tables[0]);
        }

        if (Number > dt1.Rows.Count)
        {
            Number = dt1.Rows.Count;
        }

        DataTable dt = new DataTable();

        dt = dt1.Clone();//构建与DT1表具有一样表头的空表！

        int temp = Shove._Convert.StrToInt(dt1.Rows.Count.ToString(), 0);
        int tem = Shove._Convert.StrToInt(dt1.Rows.Count.ToString(), 0) - Number;

        DataRow[] drs = dt1.Select("id>" + tem.ToString() + " AND id<=" + temp.ToString());

        foreach (DataRow dr in drs)
        {
            dt.Rows.Add(dr.ItemArray);
        }

        return dt;
    }

    public static DataTable PL5_HZ_SeleteByNum(int Number)
    {
        DataSet ds = null;
        DAL.Procedures.P_TrendChart_PL5_HZ(ref ds, Number);
        DataTable dt = ds.Tables[0];
        return dt;
    }

    public static DataTable PL5_YS_SeleteByNum(int Number)
    {
        DataTable dt1 = Shove._Web.Cache.GetCacheAsDataTable("PL5_YS_SeleteByNum_ds11");

        //DataTable dt1 = (DataTable)HttpContext.Current.Cache["ds11"];    

        if (dt1 == null)
        {
            DataSet ds = null;

            DAL.Procedures.P_TrendChart_PL5_YS(ref ds);

            dt1 = ds.Tables[0];

            Shove._Web.Cache.SetCache("PL5_YS_SeleteByNum_ds11", ds.Tables[0]);

            //HttpContext.Current.Cache["ds11"] = dt1;
        }

        if (Number > dt1.Rows.Count)
        {
            Number = dt1.Rows.Count;
        }

        DataTable dt = new DataTable();

        dt = dt1.Clone();

        int temp = Shove._Convert.StrToInt(dt1.Rows.Count.ToString(), 0);
        int tem = Shove._Convert.StrToInt(dt1.Rows.Count.ToString(), 0) - Number;

        DataRow[] drs = dt1.Select("id>" + tem.ToString() + " AND id<=" + temp.ToString());

        // DataRow[] drs = dt1.Select ("id<="+Number.ToString());

        foreach (DataRow dr in drs)
        {
            dt.Rows.Add(dr.ItemArray);
        }

        return dt;
    }

    public static DataTable X7_DZX_SeleteByNum(int Number)
    {
        DataTable dt1 = Shove._Web.Cache.GetCacheAsDataTable("X7_DZX_SeleteByNum_ds11");

        if (dt1 == null)
        {
            DataSet ds = null;

            DAL.Procedures.P_TrendChart_7X_DZX(ref ds);

            dt1 = ds.Tables[0];

            Shove._Web.Cache.SetCache("X7_DZX_SeleteByNum_ds11", ds.Tables[0]);
        }

        if (Number > dt1.Rows.Count)
        {
            Number = dt1.Rows.Count;
        }

        DataTable dt = new DataTable();

        dt = dt1.Clone();//构建与DT1表具有一样表头的空表！

        int temp = Shove._Convert.StrToInt(dt1.Rows.Count.ToString(), 0);
        int tem = Shove._Convert.StrToInt(dt1.Rows.Count.ToString(), 0) - Number;

        DataRow[] drs = dt1.Select("id>" + tem.ToString() + " AND id<=" + temp.ToString());

        foreach (DataRow dr in drs)
        {
            dt.Rows.Add(dr.ItemArray);
        }

        return dt;
    }

    public static DataTable X7_012_SeleteByNum(int Number)
    {
        DataTable dt1 = Shove._Web.Cache.GetCacheAsDataTable("X7_012_SeleteByNum_ds11");

        if (dt1 == null)
        {
            DataSet ds = null;

            DAL.Procedures.P_TrendChart_7X_012(ref ds);

            dt1 = ds.Tables[0];

            Shove._Web.Cache.SetCache("X7_012_SeleteByNum_ds11", ds.Tables[0]);
        }

        if (Number > dt1.Rows.Count)
        {
            Number = dt1.Rows.Count;
        }

        DataTable dt = new DataTable();

        dt = dt1.Clone();//构建与DT1表具有一样表头的空表！

        int temp = Shove._Convert.StrToInt(dt1.Rows.Count.ToString(), 0);
        int tem = Shove._Convert.StrToInt(dt1.Rows.Count.ToString(), 0) - Number;

        DataRow[] drs = dt1.Select("id>" + tem.ToString() + " AND id<=" + temp.ToString());

        foreach (DataRow dr in drs)
        {
            dt.Rows.Add(dr.ItemArray);
        }

        return dt;
    }

    public static DataTable X7_YS_SeleteByNum(int Number)
    {
        DataTable dt1 = Shove._Web.Cache.GetCacheAsDataTable("x7_ys_ds11");

        //DataTable dt1 = (DataTable)HttpContext.Current.Cache["ds11"];    

        if (dt1 == null)
        {
            DataSet ds = null;

            DAL.Procedures.P_TrendChart_7X_YS(ref ds);

            dt1 = ds.Tables[0];

            Shove._Web.Cache.SetCache("x7_ys_ds11", ds.Tables[0]);

            //HttpContext.Current.Cache["ds11"] = dt1;
        }

        if (Number > dt1.Rows.Count)
        {
            Number = dt1.Rows.Count;
        }

        DataTable dt = new DataTable();

        dt = dt1.Clone();

        int temp = Shove._Convert.StrToInt(dt1.Rows.Count.ToString(), 0);
        int tem = Shove._Convert.StrToInt(dt1.Rows.Count.ToString(), 0) - Number;

        DataRow[] drs = dt1.Select("id>" + tem.ToString() + " AND id<=" + temp.ToString());

        // DataRow[] drs = dt1.Select ("id<="+Number.ToString());

        foreach (DataRow dr in drs)
        {
            dt.Rows.Add(dr.ItemArray);
        }

        return dt;
    }

    public static DataTable X7_DX_SeleteByNum(int Number)
    {
        DataTable dt1 = Shove._Web.Cache.GetCacheAsDataTable("x7_dx_ds11");

        //DataTable dt1 = (DataTable)HttpContext.Current.Cache["ds11"];    

        if (dt1 == null)
        {
            DataSet ds = null;

            DAL.Procedures.P_TrendChart_7X_DX(ref ds);

            dt1 = ds.Tables[0];

            Shove._Web.Cache.SetCache("x7_dx_ds11", ds.Tables[0]);

            //HttpContext.Current.Cache["ds11"] = dt1;
        }

        if (Number > dt1.Rows.Count)
        {
            Number = dt1.Rows.Count;
        }

        DataTable dt = new DataTable();

        dt = dt1.Clone();

        int temp = Shove._Convert.StrToInt(dt1.Rows.Count.ToString(), 0);
        int tem = Shove._Convert.StrToInt(dt1.Rows.Count.ToString(), 0) - Number;

        DataRow[] drs = dt1.Select("id>" + tem.ToString() + " AND id<=" + temp.ToString());

        // DataRow[] drs = dt1.Select ("id<="+Number.ToString());

        foreach (DataRow dr in drs)
        {
            dt.Rows.Add(dr.ItemArray);
        }

        return dt;
    }

    public static DataTable X7_ZH_SeleteByNum(int Number)
    {
        DataTable dt1 = Shove._Web.Cache.GetCacheAsDataTable("X7_ZH_SeleteByNum_ds11");

        //DataTable dt1 = (DataTable)HttpContext.Current.Cache["ds11"];    

        if (dt1 == null)
        {
            DataSet ds = null;

            DAL.Procedures.P_TrendChart_7X_ZH(ref ds);

            dt1 = ds.Tables[0];

            Shove._Web.Cache.SetCache("X7_ZH_SeleteByNum_ds11", ds.Tables[0]);
        }

        if (Number > dt1.Rows.Count)
        {
            Number = dt1.Rows.Count;
        }

        DataTable dt = new DataTable();

        dt = dt1.Clone();

        int temp = Shove._Convert.StrToInt(dt1.Rows.Count.ToString(), 0);
        int tem = Shove._Convert.StrToInt(dt1.Rows.Count.ToString(), 0) - Number;

        DataRow[] drs = dt1.Select("id>" + tem.ToString() + " AND id<=" + temp.ToString());

        // DataRow[] drs = dt1.Select ("id<="+Number.ToString());

        foreach (DataRow dr in drs)
        {
            dt.Rows.Add(dr.ItemArray);
        }

        return dt;
    }

    public static DataTable X7_LH_SeleteByNum(int Number)
    {
        DataTable dt1 = Shove._Web.Cache.GetCacheAsDataTable("X7_LH_SeleteByNum_ds11");

        //DataTable dt1 = (DataTable)HttpContext.Current.Cache["X7_LH_SeleteByNum_ds11"];    

        if (dt1 == null)
        {
            DataSet ds = null;

            DAL.Procedures.P_TrendChart_7X_LH(ref ds);

            dt1 = ds.Tables[0];

            Shove._Web.Cache.SetCache("X7_LH_SeleteByNum_ds11", ds.Tables[0]);

            //HttpContext.Current.Cache["ds11"] = dt1;
        }

        if (Number > dt1.Rows.Count)
        {
            Number = dt1.Rows.Count;
        }

        DataTable dt = new DataTable();

        dt = dt1.Clone();

        int temp = Shove._Convert.StrToInt(dt1.Rows.Count.ToString(), 0);
        int tem = Shove._Convert.StrToInt(dt1.Rows.Count.ToString(), 0) - Number;

        DataRow[] drs = dt1.Select("id>" + tem.ToString() + " AND id<=" + temp.ToString());

        // DataRow[] drs = dt1.Select ("id<="+Number.ToString());

        foreach (DataRow dr in drs)
        {
            dt.Rows.Add(dr.ItemArray);
        }

        return dt;
    }

    public static DataTable X7_JO_SeleteByNum(int Number)
    {
        DataTable dt1 = Shove._Web.Cache.GetCacheAsDataTable("X7_JO_SeleteByNum_ds11");

        //DataTable dt1 = (DataTable)HttpContext.Current.Cache["ds11"];    

        if (dt1 == null)
        {
            DataSet ds = null;

            DAL.Procedures.P_TrendChart_7X_JO(ref ds);

            dt1 = ds.Tables[0];

            Shove._Web.Cache.SetCache("X7_JO_SeleteByNum_ds11", ds.Tables[0]);

            //HttpContext.Current.Cache["ds11"] = dt1;
        }

        if (Number > dt1.Rows.Count)
        {
            Number = dt1.Rows.Count;
        }

        DataTable dt = new DataTable();

        dt = dt1.Clone();

        int temp = Shove._Convert.StrToInt(dt1.Rows.Count.ToString(), 0);
        int tem = Shove._Convert.StrToInt(dt1.Rows.Count.ToString(), 0) - Number;

        DataRow[] drs = dt1.Select("id>" + tem.ToString() + " AND id<=" + temp.ToString());

        // DataRow[] drs = dt1.Select ("id<="+Number.ToString());

        foreach (DataRow dr in drs)
        {
            dt.Rows.Add(dr.ItemArray);
        }

        return dt;
    }

    public static DataTable X7_CF_SeleteByNum(int Number)
    {
        DataTable dt1 = Shove._Web.Cache.GetCacheAsDataTable("X7_CF_SeleteByNum_ds11");

        if (dt1 == null)
        {
            DataSet ds = null;

            DAL.Procedures.P_TrendChart_7X_CF(ref ds);

            dt1 = ds.Tables[0];

            Shove._Web.Cache.SetCache("X7_CF_SeleteByNum_ds11", ds.Tables[0]);
        }

        if (Number > dt1.Rows.Count)
        {
            Number = dt1.Rows.Count;
        }

        DataTable dt = new DataTable();

        dt = dt1.Clone();//构建与DT1表具有一样表头的空表！

        int temp = Shove._Convert.StrToInt(dt1.Rows.Count.ToString(), 0);
        int tem = Shove._Convert.StrToInt(dt1.Rows.Count.ToString(), 0) - Number;

        DataRow[] drs = dt1.Select("id>" + tem.ToString() + " AND id<=" + temp.ToString());

        foreach (DataRow dr in drs)
        {
            dt.Rows.Add(dr.ItemArray);
        }

        return dt;
    }

    public static DataTable X7_HMFB_SeleteByNum(int Number)
    {
        DataTable dt1 = Shove._Web.Cache.GetCacheAsDataTable("X7_HMFB_SeleteByNum_ds11");

        if (dt1 == null)
        {
            DataSet ds = null;

            DAL.Procedures.P_TrendChart_7X_HMFB(ref ds);

            dt1 = ds.Tables[0];

            Shove._Web.Cache.SetCache("X7_HMFB_SeleteByNum_ds11", ds.Tables[0]);
        }

        if (Number > dt1.Rows.Count)
        {
            Number = dt1.Rows.Count;
        }

        DataTable dt = new DataTable();

        dt = dt1.Clone();

        int temp = Shove._Convert.StrToInt(dt1.Rows.Count.ToString(), 0);
        int tem = Shove._Convert.StrToInt(dt1.Rows.Count.ToString(), 0) - Number;

        DataRow[] drs = dt1.Select("id>" + tem.ToString() + " AND id<=" + temp.ToString());

        foreach (DataRow dr in drs)
        {
            dt.Rows.Add(dr.ItemArray);
        }

        return dt;
    }

    public static DataTable X7_HZHeng_SeleteByNum(int Number)
    {
        DataTable dt1 = Shove._Web.Cache.GetCacheAsDataTable("X7_HZHeng_SeleteByNum_ds11");

        if (dt1 == null)
        {
            DataSet ds = null;

            DAL.Procedures.P_TrendChart_7X_HZHeng(ref ds);

            dt1 = ds.Tables[0];

            Shove._Web.Cache.SetCache("X7_HZHeng_SeleteByNum_ds11", ds.Tables[0]);
        }

        if (Number > dt1.Rows.Count)
        {
            Number = dt1.Rows.Count;
        }

        DataTable dt = new DataTable();

        dt = dt1.Clone();

        int temp = Shove._Convert.StrToInt(dt1.Rows.Count.ToString(), 0);
        int tem = Shove._Convert.StrToInt(dt1.Rows.Count.ToString(), 0) - Number;

        DataRow[] drs = dt1.Select("id>" + tem.ToString() + " AND id<=" + temp.ToString());

        foreach (DataRow dr in drs)
        {
            dt.Rows.Add(dr.ItemArray);
        }

        return dt;
    }

    public static DataTable X7_HZZhong_SeleteByNum(int Number)
    {
        DataTable dt1 = Shove._Web.Cache.GetCacheAsDataTable("X7_HZZhong_SeleteByNum_ds11");

        if (dt1 == null)
        {
            DataSet ds = null;

            DAL.Procedures.P_TrendChart_7X_HZzhong(ref ds);

            dt1 = ds.Tables[0];

            Shove._Web.Cache.SetCache("X7_HZZhong_SeleteByNum_ds11", ds.Tables[0]);
        }

        if (Number > dt1.Rows.Count)
        {
            Number = dt1.Rows.Count;
        }

        DataTable dt = new DataTable();

        dt = dt1.Clone();

        int temp = Shove._Convert.StrToInt(dt1.Rows.Count.ToString(), 0);
        int tem = Shove._Convert.StrToInt(dt1.Rows.Count.ToString(), 0) - Number;

        DataRow[] drs = dt1.Select("id>" + tem.ToString() + " AND id<=" + temp.ToString());

        foreach (DataRow dr in drs)
        {
            dt.Rows.Add(dr.ItemArray);
        }

        return dt;
    }

    public static DataTable TC22X5_HMFB_SeleteByNum(int Number)
    {
        DataSet ds = null;
        DAL.Procedures.P_TrendChart_22X5_HMFB(ref ds, Number);
        DataTable dt = ds.Tables[0];
        return dt;
    }

    public static DataTable TC22X5_JO_SeleteByNum(int Number)
    {
        DataTable dt1 = Shove._Web.Cache.GetCacheAsDataTable("TC22X5_JO_SeleteByNum_ds11");

        //DataTable dt1 = (DataTable)HttpContext.Current.Cache["ds11"];    

        if (dt1 == null)
        {
            DataSet ds = null;

            DAL.Procedures.P_TrendChart_22X5_JO(ref ds);

            dt1 = ds.Tables[0];

            Shove._Web.Cache.SetCache("TC22X5_JO_SeleteByNum_ds11", ds.Tables[0]);

            //HttpContext.Current.Cache["ds11"] = dt1;
        }

        if (Number > dt1.Rows.Count)
        {
            Number = dt1.Rows.Count;
        }

        DataTable dt = new DataTable();

        dt = dt1.Clone();

        int temp = Shove._Convert.StrToInt(dt1.Rows.Count.ToString(), 0);
        int tem = Shove._Convert.StrToInt(dt1.Rows.Count.ToString(), 0) - Number;

        DataRow[] drs = dt1.Select("id>" + tem.ToString() + " AND id<=" + temp.ToString());

        foreach (DataRow dr in drs)
        {
            dt.Rows.Add(dr.ItemArray);
        }

        return dt;
    }

    public static DataTable TC22X5_LH_SeleteByNum(int Number)
    {
        DataTable dt1 = Shove._Web.Cache.GetCacheAsDataTable("TC22X5_LH_SeleteByNum_ds11");

        if (dt1 == null)
        {
            DataSet ds = null;

            DAL.Procedures.P_TrendChart_22X5_LH(ref ds);

            dt1 = ds.Tables[0];

            Shove._Web.Cache.SetCache("TC22X5_LH_SeleteByNum_ds11", ds.Tables[0]);
        }

        if (Number > dt1.Rows.Count)
        {
            Number = dt1.Rows.Count;
        }

        DataTable dt = new DataTable();

        dt = dt1.Clone();

        int temp = Shove._Convert.StrToInt(dt1.Rows.Count.ToString(), 0);
        int tem = Shove._Convert.StrToInt(dt1.Rows.Count.ToString(), 0) - Number;

        DataRow[] drs = dt1.Select("id>" + tem.ToString() + " AND id<=" + temp.ToString());

        foreach (DataRow dr in drs)
        {
            dt.Rows.Add(dr.ItemArray);
        }

        return dt;
    }

    public static DataTable TC22X5_WeiHaoCF_SeleteByNum(int Number)
    {
        DataTable dt1 = Shove._Web.Cache.GetCacheAsDataTable("TC22X5_WeiHaoCF_SeleteByNum_ds11");

        if (dt1 == null)
        {
            DataSet ds = null;

            DAL.Procedures.P_TrendChart_22X5_WeiHaoCF(ref ds);

            dt1 = ds.Tables[0];

            Shove._Web.Cache.SetCache("TC22X5_WeiHaoCF_SeleteByNum_ds11", ds.Tables[0]);
        }

        if (Number > dt1.Rows.Count)
        {
            Number = dt1.Rows.Count;
        }

        DataTable dt = new DataTable();

        dt = dt1.Clone();

        int temp = Shove._Convert.StrToInt(dt1.Rows.Count.ToString(), 0);
        int tem = Shove._Convert.StrToInt(dt1.Rows.Count.ToString(), 0) - Number;

        DataRow[] drs = dt1.Select("id>" + tem.ToString() + " AND id<=" + temp.ToString());

        foreach (DataRow dr in drs)
        {
            dt.Rows.Add(dr.ItemArray);
        }

        return dt;
    }

    public static DataTable TC22X5_HZ_Heng_SeleteByNum(int Number)
    {
        DataTable dt1 = Shove._Web.Cache.GetCacheAsDataTable("TC22X5_HZ_Heng_ds11");

        if (dt1 == null)
        {
            DataSet ds = null;

            DAL.Procedures.P_TrendChart_22X5_HZ_Heng(ref ds);

            dt1 = ds.Tables[0];

            Shove._Web.Cache.SetCache("TC22X5_HZ_Heng_ds11", ds.Tables[0]);
        }

        if (Number > dt1.Rows.Count)
        {
            Number = dt1.Rows.Count;
        }

        DataTable dt = new DataTable();

        dt = dt1.Clone();

        int temp = Shove._Convert.StrToInt(dt1.Rows.Count.ToString(), 0);
        int tem = Shove._Convert.StrToInt(dt1.Rows.Count.ToString(), 0) - Number;

        DataRow[] drs = dt1.Select("id>" + tem.ToString() + " AND id<=" + temp.ToString());

        foreach (DataRow dr in drs)
        {
            dt.Rows.Add(dr.ItemArray);
        }

        return dt;
    }

    public static DataTable TC22X5_HZZong_SeleteByNum(int Number)
    {
        DataTable dt1 = Shove._Web.Cache.GetCacheAsDataTable("TC22X5_HZZong_ds11");

        if (dt1 == null)
        {
            DataSet ds = null;

            DAL.Procedures.P_TrendChart_22X5_HZzong(ref ds);

            dt1 = ds.Tables[0];

            Shove._Web.Cache.SetCache("TC22X5_HZZong_ds11", ds.Tables[0]);
        }

        if (Number > dt1.Rows.Count)
        {
            Number = dt1.Rows.Count;
        }

        DataTable dt = new DataTable();

        dt = dt1.Clone();

        int temp = Shove._Convert.StrToInt(dt1.Rows.Count.ToString(), 0);
        int tem = Shove._Convert.StrToInt(dt1.Rows.Count.ToString(), 0) - Number;

        DataRow[] drs = dt1.Select("id>" + tem.ToString() + " AND id<=" + temp.ToString());

        foreach (DataRow dr in drs)
        {
            dt.Rows.Add(dr.ItemArray);
        }

        return dt;
    }

    public static DataTable TC22X5_Weihao_SeleteByNum(int Number)
    {
        DataTable dt1 = Shove._Web.Cache.GetCacheAsDataTable("TC22X5_Weihao_SeleteByNum_ds11");

        if (dt1 == null)
        {
            DataSet ds = null;

            DAL.Procedures.P_TrendChart_22X5_WH(ref ds);

            dt1 = ds.Tables[0];

            Shove._Web.Cache.SetCache("TC22X5_Weihao_SeleteByNum_ds11", ds.Tables[0]);
        }

        if (Number > dt1.Rows.Count)
        {
            Number = dt1.Rows.Count;
        }

        DataTable dt = new DataTable();

        dt = dt1.Clone();

        int temp = Shove._Convert.StrToInt(dt1.Rows.Count.ToString(), 0);
        int tem = Shove._Convert.StrToInt(dt1.Rows.Count.ToString(), 0) - Number;

        DataRow[] drs = dt1.Select("id>" + tem.ToString() + " AND id<=" + temp.ToString());

        foreach (DataRow dr in drs)
        {
            dt.Rows.Add(dr.ItemArray);
        }

        return dt;
    }

    public static DataTable CJDLT_JiMaWeihao_SeleteByNum(int Number)
    {
        DataTable dt1 = Shove._Web.Cache.GetCacheAsDataTable("CJDLT_JiMaWeihao_SeleteByNum_ds11");

        if (dt1 == null)
        {
            DataSet ds = null;

            DAL.Procedures.P_TrendChart_CJDLT_WH(ref ds);

            dt1 = ds.Tables[0];

            Shove._Web.Cache.SetCache("CJDLT_JiMaWeihao_SeleteByNum_ds11", ds.Tables[0]);
        }

        if (Number > dt1.Rows.Count)
        {
            Number = dt1.Rows.Count;
        }

        DataTable dt = new DataTable();

        dt = dt1.Clone();

        int temp = Shove._Convert.StrToInt(dt1.Rows.Count.ToString(), 0);
        int tem = Shove._Convert.StrToInt(dt1.Rows.Count.ToString(), 0) - Number;

        DataRow[] drs = dt1.Select("id>" + tem.ToString() + " AND id<=" + temp.ToString());

        foreach (DataRow dr in drs)
        {
            dt.Rows.Add(dr.ItemArray);
        }

        return dt;
    }

    public static DataTable CJDLT_HZHeng_SeleteByNum(int Number)
    {
        DataTable dt1 = Shove._Web.Cache.GetCacheAsDataTable("CJDLT_HZHeng_SeleteByNum_ds11");

        if (dt1 == null)
        {
            DataSet ds = null;

           DAL.Procedures.P_TrendChart_CJDLT_HZ_Heng(ref ds);

            dt1 = ds.Tables[0];

            Shove._Web.Cache.SetCache("CJDLT_HZHeng_SeleteByNum_ds11", ds.Tables[0]);
        }

        if (Number > dt1.Rows.Count)
        {
            Number = dt1.Rows.Count;
        }

        DataTable dt = new DataTable();

        dt = dt1.Clone();

        int temp = Shove._Convert.StrToInt(dt1.Rows.Count.ToString(), 0);
        int tem = Shove._Convert.StrToInt(dt1.Rows.Count.ToString(), 0) - Number;

        DataRow[] drs = dt1.Select("id>" + tem.ToString() + " AND id<=" + temp.ToString());

        foreach (DataRow dr in drs)
        {
            dt.Rows.Add(dr.ItemArray);
        }

        return dt;
    }

    public static DataTable CJDLT_HZZhong_SeleteByNum(int Number)
    {
        DataTable dt1 = Shove._Web.Cache.GetCacheAsDataTable("CJDLT_HZZhong_SeleteByNum_ds11");

        if (dt1 == null)
        {
            DataSet ds = null;

            DAL.Procedures.P_TrendChart_CJDLT_HZzong(ref ds);

            dt1 = ds.Tables[0];

            Shove._Web.Cache.SetCache("CJDLT_HZZhong_SeleteByNum_ds11", ds.Tables[0]);
        }

        if (Number > dt1.Rows.Count)
        {
            Number = dt1.Rows.Count;
        }

        DataTable dt = new DataTable();

        dt = dt1.Clone();

        int temp = Shove._Convert.StrToInt(dt1.Rows.Count.ToString(), 0);
        int tem = Shove._Convert.StrToInt(dt1.Rows.Count.ToString(), 0) - Number;

        DataRow[] drs = dt1.Select("id>" + tem.ToString() + " AND id<=" + temp.ToString());

        foreach (DataRow dr in drs)
        {
            dt.Rows.Add(dr.ItemArray);
        }

        return dt;
    }

    public static DataTable TC22X5_lengre_SeleteByNum(int Number)
    {
        DataSet ds = null;
        DAL.Procedures.P_TrendChart_22X5_HMLR(ref ds, Number);
        DataTable dt = ds.Tables[0];
        return dt;

    }

    public static DataTable  TC22X5_lengrejj_SeleteByNum(int Number)
    {
        DataSet ds = null;
        DAL.Procedures.P_TrendChart_22X5_HMLRjj(ref ds, Number);
       DataTable dt = ds.Tables[0];
       return dt;
    }

    public static DataTable TC22X5_YS_SeleteByNum(int Number)
    {
        DataTable dt1 = Shove._Web.Cache.GetCacheAsDataTable("TC22X5_YS_ds11");

        if (dt1 == null)
        {
            DataSet ds = null;

            DAL.Procedures.P_TrendChart_22X5_YS(ref ds);

            dt1 = ds.Tables[0];

            Shove._Web.Cache.SetCache("TC22X5_YS_ds11", ds.Tables[0]);
        }

        if (Number > dt1.Rows.Count)
        {
            Number = dt1.Rows.Count;
        }

        DataTable dt = new DataTable();

        dt = dt1.Clone();

        int temp = Shove._Convert.StrToInt(dt1.Rows.Count.ToString(), 0);
        int tem = Shove._Convert.StrToInt(dt1.Rows.Count.ToString(), 0) - Number;

        DataRow[] drs = dt1.Select("id>" + tem.ToString() + " AND id<=" + temp.ToString());

        foreach (DataRow dr in drs)
        {
            dt.Rows.Add(dr.ItemArray);
        }

        return dt;
    }

    public static DataTable CJDLT_YS_SeleteByNum(int Number)
    {
        DataTable dt1 = Shove._Web.Cache.GetCacheAsDataTable("CJDLT_YS_SeleteByNum_ds11");

        if (dt1 == null)
        {
            DataSet ds = null;

            DAL.Procedures.P_TrendChart_CJDLT_YS(ref ds);

            dt1 = ds.Tables[0];

            Shove._Web.Cache.SetCache("CJDLT_YS_SeleteByNum_ds11", ds.Tables[0]);
        }

        if (Number > dt1.Rows.Count)
        {
            Number = dt1.Rows.Count;
        }

        DataTable dt = new DataTable();

        dt = dt1.Clone();

        int temp = Shove._Convert.StrToInt(dt1.Rows.Count.ToString(), 0);
        int tem = Shove._Convert.StrToInt(dt1.Rows.Count.ToString(), 0) - Number;

        DataRow[] drs = dt1.Select("id>" + tem.ToString() + " AND id<=" + temp.ToString());

        foreach (DataRow dr in drs)
        {
            dt.Rows.Add(dr.ItemArray);
        }

        return dt;
    }

    public static DataTable CJDLT_HMFB_SeleteByNum(int Number)
    {
        string key = "CJDLT_YS_HMFB_SeleteByNum_ds" + Number.ToString();
        DataTable dt1 = Shove._Web.Cache.GetCacheAsDataTable(key);

        if (dt1 == null)
        {
            DataSet ds = null;

            DAL.Procedures.P_TrendChart_CJDLT_HMFB(ref ds, Number);

            dt1 = ds.Tables[0];

            Shove._Web.Cache.SetCache(key, ds.Tables[0]);
        }

        if (Number > dt1.Rows.Count)
        {
            Number = dt1.Rows.Count;
        }

        DataTable dt = new DataTable();

        dt = dt1.Clone();

        int temp = Shove._Convert.StrToInt(dt1.Rows.Count.ToString(), 0);
        int tem = Shove._Convert.StrToInt(dt1.Rows.Count.ToString(), 0) - Number;
        DataRow[] drs = dt1.Select("id>" + tem.ToString() + " AND id<=" + temp.ToString());

        foreach (DataRow dr in drs)
        {
            dt.Rows.Add(dr.ItemArray);
        }

        return dt;
    }

    public static DataTable CJDLT_jima_SeleteByNum(int Number)
    {
        DataTable dt1 = Shove._Web.Cache.GetCacheAsDataTable("CJDLT_jima_SeleteByNum_ds11");

        if (dt1 == null)
        {
            DataSet ds = null;

            DAL.Procedures.P_TrendChart_CJDLT_jima(ref ds);

            dt1 = ds.Tables[0];

            Shove._Web.Cache.SetCache("CJDLT_jima_SeleteByNum_ds11", ds.Tables[0]);
        }

        if (Number > dt1.Rows.Count)
        {
            Number = dt1.Rows.Count;
        }

        DataTable dt = new DataTable();

        dt = dt1.Clone();

        int temp = Shove._Convert.StrToInt(dt1.Rows.Count.ToString(), 0);
        int tem = Shove._Convert.StrToInt(dt1.Rows.Count.ToString(), 0) - Number;
        DataRow[] drs = dt1.Select("id>" + tem.ToString() + " AND id<=" + temp.ToString());

        foreach (DataRow dr in drs)
        {
            dt.Rows.Add(dr.ItemArray);
        }

        return dt;
    }

    public static DataTable CJDLT_TeMa_SeleteByNum(int Number)
    {
        DataTable dt1 = Shove._Web.Cache.GetCacheAsDataTable("CJDLT_TeMas_SeleteByNum_ds11");

        if (dt1 == null)
        {
            DataSet ds = null;

            DAL.Procedures.P_TrendChart_CJDLT_tema(ref ds);

            dt1 = ds.Tables[0];

            Shove._Web.Cache.SetCache("CJDLT_TeMas_SeleteByNum_ds11", ds.Tables[0]);
        }

        if (Number > dt1.Rows.Count)
        {
            Number = dt1.Rows.Count;
        }

        DataTable dt = new DataTable();

        dt = dt1.Clone();

        int temp = Shove._Convert.StrToInt(dt1.Rows.Count.ToString(), 0);
        int tem = Shove._Convert.StrToInt(dt1.Rows.Count.ToString(), 0) - Number;
        DataRow[] drs = dt1.Select("id>" + tem.ToString() + " AND id<=" + temp.ToString());

        foreach (DataRow dr in drs)
        {
            dt.Rows.Add(dr.ItemArray);
        }

        return dt;
    }

    public static DataTable CJDLT_JO_SeleteByNum(int Number)
    {
        DataTable dt1 = Shove._Web.Cache.GetCacheAsDataTable("CJDLT_JO_SeleteByNum_ds11");

        if (dt1 == null)
        {
            DataSet ds = null;

            DAL.Procedures.P_TrendChart_CJDLT_Jiou(ref ds);

            dt1 = ds.Tables[0];

            Shove._Web.Cache.SetCache("CJDLT_JO_SeleteByNum_ds11", ds.Tables[0]);
        }

        if (Number > dt1.Rows.Count)
        {
            Number = dt1.Rows.Count;
        }

        DataTable dt = new DataTable();

        dt = dt1.Clone();

        int temp = Shove._Convert.StrToInt(dt1.Rows.Count.ToString(), 0);
        int tem = Shove._Convert.StrToInt(dt1.Rows.Count.ToString(), 0) - Number;
        DataRow[] drs = dt1.Select("id>" + tem.ToString() + " AND id<=" + temp.ToString());

        foreach (DataRow dr in drs)
        {
            dt.Rows.Add(dr.ItemArray);
        }

        return dt;
    }

    public static DataTable CJDLT_LH_SeleteByNum(int Number)
    {
        DataTable dt1 = Shove._Web.Cache.GetCacheAsDataTable("CJDLT_LH_SeleteByNum_ds11");

        if (dt1 == null)
        {
            DataSet ds = null;

            DAL.Procedures.P_TrendChart_CJDLT_LH(ref ds);

            dt1 = ds.Tables[0];

            Shove._Web.Cache.SetCache("CJDLT_LH_SeleteByNum_ds11", ds.Tables[0]);
        }

        if (Number > dt1.Rows.Count)
        {
            Number = dt1.Rows.Count;
        }

        DataTable dt = new DataTable();

        dt = dt1.Clone();

        int temp = Shove._Convert.StrToInt(dt1.Rows.Count.ToString(), 0);
        int tem = Shove._Convert.StrToInt(dt1.Rows.Count.ToString(), 0) - Number;
        DataRow[] drs = dt1.Select("id>" + tem.ToString() + " AND id<=" + temp.ToString());

        foreach (DataRow dr in drs)
        {
            dt.Rows.Add(dr.ItemArray);
        }

        return dt;
    }

    public static DataTable CJDLT_lengre_SeleteByNum(int Number)
    {
        DataSet ds = null;
        DAL.Procedures.P_TrendChart_CJDLT_HMLR_Tema(ref ds, Number);
        DataTable dt = ds.Tables[0];
        return dt;

    }

    public static DataTable CJDLT_lengrejj_SeleteByNum(int Number)
    {
        DataSet ds = null;
        DAL.Procedures.P_TrendChart_CJDLT_HMLR_Temajj(ref ds, Number);
        DataTable dt = ds.Tables[0];
        return dt;
    }

    public static DataTable CJDLT_JiMa_lengre_SeleteByNum(int Number)
    {
        DataSet ds = null;
        DAL.Procedures.P_TrendChart_CJDLT_HMLR_JiMa(ref ds, Number);
        DataTable dt = ds.Tables[0];
        return dt;

    }

    public static DataTable CJDLT_JiMa_lengrejj_SeleteByNum(int Number)
    {
        DataSet ds = null;
        DAL.Procedures.P_TrendChart_CJDLT_HMLR_JiMajj(ref ds, Number);
        DataTable dt = ds.Tables[0];
        return dt;
    }

    public static DataTable KLPK_DX_SeleteByNum(int Number)
    {
        DataTable dt1 = Shove._Web.Cache.GetCacheAsDataTable("KLPK_DX_SeleteByNum_ds11");

        if (dt1 == null)
        {
            DataSet ds = null;

            DAL.Procedures.P_TrendChart_KLPK_DX(ref ds);

            dt1 = ds.Tables[0];

            Shove._Web.Cache.SetCache("KLPK_DX_SeleteByNum_ds1", ds.Tables[0]);
        }

        if (Number > dt1.Rows.Count)
        {
            Number = dt1.Rows.Count;
        }

        DataTable dt = new DataTable();

        dt = dt1.Clone();

        int temp = Shove._Convert.StrToInt(dt1.Rows.Count.ToString(), 0);
        int tem = Shove._Convert.StrToInt(dt1.Rows.Count.ToString(), 0) - Number;

        DataRow[] drs = dt1.Select("id>" + tem.ToString() + " AND id<=" + temp.ToString());

        foreach (DataRow dr in drs)
        {
            dt.Rows.Add(dr.ItemArray);
        }

        return dt;
    }

    public static DataTable KLPK_ZH_SeleteByNum(int Number)
    {
        DataTable dt1 = Shove._Web.Cache.GetCacheAsDataTable("KLPK_ZH_SeleteByNum_ds11");

        if (dt1 == null)
        {
            DataSet ds = null;

            DAL.Procedures.P_TrendChart_KLPK_ZH(ref ds);

            dt1 = ds.Tables[0];

            Shove._Web.Cache.SetCache("KLPK_ZH_SeleteByNum_ds1", ds.Tables[0]);
        }

        if (Number > dt1.Rows.Count)
        {
            Number = dt1.Rows.Count;
        }

        DataTable dt = new DataTable();

        dt = dt1.Clone();

        int temp = Shove._Convert.StrToInt(dt1.Rows.Count.ToString(), 0);
        int tem = Shove._Convert.StrToInt(dt1.Rows.Count.ToString(), 0) - Number;

        DataRow[] drs = dt1.Select("id>" + tem.ToString() + " AND id<=" + temp.ToString());

        foreach (DataRow dr in drs)
        {
            dt.Rows.Add(dr.ItemArray);
        }

        return dt;
    }

    public static DataTable KLPK_012_SeleteByNum(int Number)
    {
        DataTable dt1 = Shove._Web.Cache.GetCacheAsDataTable("KLPK_012_SeleteByNum_ds11");

        if (dt1 == null)
        {
            DataSet ds = null;

            DAL.Procedures.P_TrendChart_KLPK_012(ref ds);

            dt1 = ds.Tables[0];

            Shove._Web.Cache.SetCache("KLPK_012_SeleteByNum_ds1", ds.Tables[0]);
        }

        if (Number > dt1.Rows.Count)
        {
            Number = dt1.Rows.Count;
        }

        DataTable dt = new DataTable();

        dt = dt1.Clone();

        int temp = Shove._Convert.StrToInt(dt1.Rows.Count.ToString(), 0);
        int tem = Shove._Convert.StrToInt(dt1.Rows.Count.ToString(), 0) - Number;

        DataRow[] drs = dt1.Select("id>" + tem.ToString() + " AND id<=" + temp.ToString());

        foreach (DataRow dr in drs)
        {
            dt.Rows.Add(dr.ItemArray);
        }

        return dt;
    }

    public static DataTable KLPK_DZX_SeleteByNum(int Number)
    {
        DataTable dt1 = Shove._Web.Cache.GetCacheAsDataTable("KLPK_DZX_SeleteByNum_ds11");

        if (dt1 == null)
        {
            DataSet ds = null;

            DAL.Procedures.P_TrendChart_KLPK_DZX(ref ds);

            dt1 = ds.Tables[0];

            Shove._Web.Cache.SetCache("KLPK_DZX_SeleteByNum_ds1", ds.Tables[0]);
        }

        if (Number > dt1.Rows.Count)
        {
            Number = dt1.Rows.Count;
        }

        DataTable dt = new DataTable();

        dt = dt1.Clone();

        int temp = Shove._Convert.StrToInt(dt1.Rows.Count.ToString(), 0);
        int tem = Shove._Convert.StrToInt(dt1.Rows.Count.ToString(), 0) - Number;

        DataRow[] drs = dt1.Select("id>" + tem.ToString() + " AND id<=" + temp.ToString());

        foreach (DataRow dr in drs)
        {
            dt.Rows.Add(dr.ItemArray);
        }

        return dt;
    }

    public static DataTable KLPK_KJFB_SeleteByNum(int Number)
    {
        DataTable dt1 = Shove._Web.Cache.GetCacheAsDataTable("KLPK_KJFB_SeleteByNum_ds11");

        if (dt1 == null)
        {
            DataSet ds = null;

            DAL.Procedures.P_TrendChart_KLPK_KJFB(ref ds);

            dt1 = ds.Tables[0];

            Shove._Web.Cache.SetCache("KLPK_KJFB_SeleteByNum_ds1", ds.Tables[0]);
        }

        if (Number > dt1.Rows.Count)
        {
            Number = dt1.Rows.Count;
        }

        DataTable dt = new DataTable();

        dt = dt1.Clone();

        int temp = Shove._Convert.StrToInt(dt1.Rows.Count.ToString(), 0);
        int tem = Shove._Convert.StrToInt(dt1.Rows.Count.ToString(), 0) - Number;

        DataRow[] drs = dt1.Select("id>" + tem.ToString() + " AND id<=" + temp.ToString());

        foreach (DataRow dr in drs)
        {
            dt.Rows.Add(dr.ItemArray);
        }

        return dt;
    }
}

