using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;
using System.Data.SqlClient;
using System.Configuration;
using System.Net;
using System.IO;


/// <summary>
/// 数据库访问类
/// </summary>
public class MsSql
{
    public static string ConnectionString = "";
    private static object LockAddOrUpdate = new object();

    public MsSql(string Connection)
    {
        ConnectionString = Connection;
    }

    public static SqlConnection creatcon()
    {
        SqlConnection con = new SqlConnection(ConnectionString);
        return con;
    }


    /// <summary>
    /// 查询（注意使用完需关闭dr）
    /// </summary>
    /// <param name="sql">查询语句</param>
    /// <returns>SqlDataReader</returns>
    public static SqlDataReader ExecuteReader(string sql, string filename)
    {
        SqlConnection conn = new SqlConnection(ConnectionString);
        try
        {
            conn.Open();
            SqlCommand cmd = new SqlCommand(sql, conn);
            return cmd.ExecuteReader(CommandBehavior.CloseConnection);
        }
        catch (SystemException exception)
        {
            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
            }
            if (sql.Length > 500)
            {
                sql = sql.Substring(0, 500) + "...";
            }
            ErrLog.HandleException(filename, "ExecuteReader(" + sql + ")：" + exception.Message);
            return null;
        }
    }

    /// <summary>
    /// 添加、更新、删除
    /// </summary>
    /// <param name="sql"></param>
    /// <returns></returns>
    public static int ExecuteNonQuery(string sql, string filename)
    {
        lock (LockAddOrUpdate)
        {
            if (sql.Trim().Length == 0)
            {
                return 0;
            }
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);

                SqlTransaction trans = conn.BeginTransaction();
                cmd.Transaction = trans;

                try
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandTimeout = 90;
                    cmd.ExecuteNonQuery();
                    trans.Commit();
                    return 0;
                }
                catch (SystemException exception)
                {
                    trans.Rollback();

                    if (sql.Length > 500)
                    {
                        sql = sql.Substring(0, 500) + "...";
                    }
                    ErrLog.HandleException(filename, "ExecuteNonQuery(" + sql + ")：" + exception.Message);
                    return -1;
                }
                finally
                {
                    if (conn.State.Equals(ConnectionState.Open))
                    {
                        conn.Close();
                    }
                }
            }
        }
    }

    /// <summary>
    /// 添加、更新、删除
    /// </summary>
    /// <param name="sql"></param>
    /// <returns></returns>
    public static int ExecuteNonQuery(string sql, string filename,string con)
    {
        lock (LockAddOrUpdate)
        {
            if (sql.Trim().Length == 0)
            {
                return 0;
            }
            using (SqlConnection conn = new SqlConnection(con))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);

                SqlTransaction trans = conn.BeginTransaction();
                cmd.Transaction = trans;

                try
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandTimeout = 90;
                    cmd.ExecuteNonQuery();
                    trans.Commit();
                    return 0;
                }
                catch (SystemException exception)
                {
                    trans.Rollback();

                    if (sql.Length > 500)
                    {
                        sql = sql.Substring(0, 500) + "...";
                    }
                    ErrLog.HandleException(filename, "ExecuteNonQuery(" + sql + ")：" + exception.Message);
                    return -1;
                }
                finally
                {
                    if (conn.State.Equals(ConnectionState.Open))
                    {
                        conn.Close();
                    }
                }
            }
        }
    }

    /// <summary>
    /// 执行查询，并返回查询多返回的结果集中第一行的第一列。忽略其他列或行。
    /// </summary>
    /// <param name="sql">查询语句</param>
    /// <returns>结果集中第一行的第一列</returns>
    public static string ExecuteScalar(string sql, string filename)
    {
        using (SqlConnection conn = new SqlConnection(ConnectionString))
        {
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                string s = cmd.ExecuteScalar().ToString();
                return s;
            }
            catch
            {
                return "";
            }
            finally
            {
                if (conn.State.Equals(ConnectionState.Open))
                {
                    conn.Close();
                }
            }
        }
    }

    /// <summary>
    /// 根据查询语句从数据库检索数据
    /// </summary>
    /// <param name="strSelect">查询语句</param>
    /// <param name="SqlConn">数据库连接</param>
    /// <returns>有数据则返回DataSet数据集（类似于ASP中的RecordSet），否则返回null</returns>
    public static DataSet ExecuteDataSet(string sql, string filename)
    {
        using (SqlConnection conn = new SqlConnection(ConnectionString))
        {
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataAdapter mySqlDataAdapter = new SqlDataAdapter();
                mySqlDataAdapter.SelectCommand = cmd;
                DataSet myDS = new DataSet();
                mySqlDataAdapter.Fill(myDS);
                return myDS;
            }
            catch (SystemException exception)
            {
                if (sql.Length > 500)
                {
                    sql = sql.Substring(0, 500) + "...";
                }
                ErrLog.HandleException(filename, "ExecuteDataSet(" + sql + ")：" + exception.Message);
                return null;
            }
            finally
            {
                if (conn.State.Equals(ConnectionState.Open))
                {
                    conn.Close();
                }
            }
        }
    }

    public static SqlDataReader ExecuteStoredProcWithReader(string spName, SqlParameter[] parameters, string filename)
    {
        try
        {
            SqlConnection conn = new SqlConnection(ConnectionString);
            conn.Open();
            SqlCommand cmd = new SqlCommand(spName, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            if (parameters != null)
            {
                for (int i = 0; i < parameters.Length; i++)
                {
                    cmd.Parameters.Add(parameters[i]);
                }
            }
            return cmd.ExecuteReader(CommandBehavior.CloseConnection);
        }
        catch (SqlException exception)
        {
            ErrLog.HandleException(filename, "ExecuteStoredProcWithReader(" + spName + ")：" + exception.Message);
            throw new SystemException(exception.Message);
        }
    }

    public static int ExecuteStoredProcNonQuery(string spName, SqlParameter[] parameters, string filename)
    {
        using (SqlConnection conn = new SqlConnection(ConnectionString))
        {
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(spName, conn);
                cmd.CommandType = CommandType.StoredProcedure;

                if (parameters != null)
                {
                    for (int i = 0; i < parameters.Length; i++)
                    {
                        cmd.Parameters.Add(parameters[i]);
                    }
                }
                int num2 = cmd.ExecuteNonQuery();
                return num2;
            }
            catch (SqlException exception)
            {
                ErrLog.HandleException(filename, "ExecuteStoredProcNonQuery(" + spName + ")：" + exception.Message);
                throw new SystemException(exception.Message);
            }
            finally
            {
                if (conn.State.Equals(ConnectionState.Open))
                {
                    conn.Close();
                }
            }
        }
    }

    public static DataSet ExecuteStoredProcWithDataSet(string spName, ref SqlParameter[] parameters, string filename)
    {
        using (SqlConnection conn = new SqlConnection(ConnectionString))
        {
            try
            {
                conn.Open();
                SqlDataAdapter da = new SqlDataAdapter();
                DataSet ds = new DataSet();
                SqlCommand cmd = new SqlCommand(spName, conn);
                cmd.CommandType = CommandType.StoredProcedure;
                if (parameters != null)
                {
                    for (int i = 0; i < parameters.Length; i++)
                    {
                        cmd.Parameters.Add(parameters[i]);
                    }
                }
                da.SelectCommand = cmd;
                da.Fill(ds);
                return ds;
            }
            catch (SqlException exception)
            {
                ErrLog.HandleException(filename, "ExecuteStoredProcWithDataSet(" + spName + ")：" + exception.Message);
                return null;
                //throw new SystemException(exception.Message);
            }
            finally
            {
                if (conn.State.Equals(ConnectionState.Open))
                {
                    conn.Close();
                }
            }
        }
    }


    #region 写 ini Task 日志、异常文件
    public static void WriteLog(string Text)
    {
        //WriteIni(Text, "Log");
    }

    public static void WriteException(string Text)
    {
       // WriteIni(Text, "Exception");
    }

    /*
    private static void WriteIni(string Text, string Section)
    
    {
        string AppDir = System.Windows.Forms.Application.StartupPath;
        if (!AppDir.EndsWith("\\"))
        {
            AppDir += "\\";
        }

        try
        {
            Shove._IO.IniFile ini = new Shove._IO.IniFile(AppDir + "Task.ini");
            ini.Write(Section, System.DateTime.Now.ToString(), Text);
        }
        catch { }
    }
     */
    #endregion


    //正则表达式替换通用方法
    public static string RegexReplace(string StrReplace, string strRegex, string NewStr)
    {
        Regex regex = new Regex(strRegex, RegexOptions.IgnoreCase | RegexOptions.Compiled);

        return regex.Replace(StrReplace, NewStr);
    }

    //正则表达式
    public static string[] Regex(string Str, string StrRegex)
    {
        Regex regex = new Regex(StrRegex, RegexOptions.IgnoreCase | RegexOptions.Compiled);
        Match m = regex.Match(Str);

        string[] strings = new string[m.Length];
        int i = 0;

        while (m.Success && i < m.Length)
        {
            strings[i] = m.Value;
            m = m.NextMatch();
            i++;
        }

        return strings;
    }

    /// <summary>
    /// 从图片地址下载图片到本地磁盘
    /// </summary>
    /// <param name="ToLocalPath">图片本地磁盘地址</param>
    /// <param name="Url">图片网址</param>
    /// <returns></returns>
    public static bool SavePhotoFromUrl(string FileName, string Url)
    {
        bool Result = false;
        WebResponse response = null;
        Stream stream = null;

        Url = Url.Replace("upload", "Upload").Replace("team", "Team").Replace("thumb", "Thumb");

        try
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);

            response = request.GetResponse();
            stream = response.GetResponseStream();

            if (!response.ContentType.ToLower().StartsWith("text/"))
            {
                Result = SaveBinaryFile(response, FileName);
            }
        }
        catch
        {
        }

        return Result;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="response">The response used to save the file</param>
    // 将二进制文件保存到磁盘
    private static bool SaveBinaryFile(WebResponse response, string FileName)
    {
        bool Result = true;
        byte[] buffer = new byte[1024];

        try
        {
            if (File.Exists(FileName))
            {
                //File.Delete(FileName);
                return Result;
            }

            Stream outStream = System.IO.File.Create(FileName);
            Stream inStream = response.GetResponseStream();

            int l;
            do
            {
                l = inStream.Read(buffer, 0, buffer.Length);

                if (l > 0)
                {
                    outStream.Write(buffer, 0, l);
                }
            }
            while (l > 0);

            outStream.Close();
            inStream.Close();
        }
        catch
        {
            Result = false;
        }

        return Result;
    }

    /// <summary>
    /// 读取指定URL地址
    /// </summary>       
    public static string GetSource(string strUrl, ref string StrError)
    {
        WebRequest request = WebRequest.Create(strUrl);
        try
        {
            //请求服务
            WebResponse response = request.GetResponse();
            //返回信息
            Stream resStream = response.GetResponseStream();
            StreamReader sr = new StreamReader(resStream, System.Text.Encoding.Default);
            string tempCode = sr.ReadToEnd();
            resStream.Close();
            sr.Close();
            return tempCode;
        }
        catch
        {
            StrError = "出错了，请检查网络是否连通;";
            return null;
        }
    }

}
