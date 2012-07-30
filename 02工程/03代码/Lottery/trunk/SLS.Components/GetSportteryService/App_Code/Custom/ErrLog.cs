using System;
using System.IO;

public class ErrLog
{
    private static object lockWrite = new object();
    
    public static void HandleException(string fileName, string errString)
    {
        LogError(fileName, errString);
    }

    public static void LogError(string filename, string errString)
    {

        new GetSportteryService.Log("Sporttery").Write(errString);
    }

    private static void WriteTextFile(string sName, string sLine)
    {
        lock (lockWrite)
        {
            try
            {
                StreamWriter writer = new StreamWriter(new FileStream(sName, FileMode.Append, FileAccess.Write), System.Text.Encoding.GetEncoding("GBK"));
                writer.WriteLine(sLine);
                writer.Close();
            }
            catch (SystemException ex)
            {
                HandleException("Log.cs", ex.Message);
            }
        }
    }
}