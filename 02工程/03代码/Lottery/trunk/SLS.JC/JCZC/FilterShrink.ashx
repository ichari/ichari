<%@ WebHandler Language="C#" Class="FilterShrink" %>

using System;
using System.Web;
using System.IO;

public class FilterShrink : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        
        string message = "";

        try
        {
            int bytelength = context.Request.ContentLength;
            byte[] inputbytes = context.Request.BinaryRead(bytelength);
            message = System.Text.Encoding.Default.GetString(inputbytes);
        }
        catch{}

        if (string.IsNullOrEmpty(message))
        {
            context.Response.ContentType = "text/plain";
            context.Response.Write("false");
            context.Response.End();
        }

        string FileName = context.Request.Cookies["ASP.NET_SessionId"].Value;

        string FilePathname = System.AppDomain.CurrentDomain.BaseDirectory + "Temp\\" + FileName + ".txt";

        if (File.Exists(FilePathname))
        {
            File.Delete(FilePathname);
        }

        Write(FilePathname, message);

        context.Response.ContentType = "text/plain";
        context.Response.Write("true");
        context.Response.End();
    }

    public void Write(string FileName, string Message)
    {
        if (String.IsNullOrEmpty(FileName))
        {
            return;
        }

        using (FileStream fs = new FileStream(FileName, FileMode.Append, FileAccess.Write, FileShare.Write))
        {
            StreamWriter writer = new StreamWriter(fs, System.Text.Encoding.GetEncoding("utf-8"));

            try
            {
                writer.WriteLine(Message);
            }
            catch { }

            writer.Close();
        }
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}