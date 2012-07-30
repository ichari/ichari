using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ICSharpCode.SharpZipLib.Zip;
using Shove.Alipay;
using Shove.Database;
using System.Text.RegularExpressions;
using System.Data;
using System.Collections;
using System.IO;


namespace SLS.MHB.Task
{
    public class GetAlipayPayResultTask
    {
        private long gCount1 = 0;

        private System.Threading.Thread thread;

        private string ConnectionString;

        public string OnlinePay_Alipay_ForUserDistill_UserNumber;
        public string OnlinePay_Alipay_ForUserDistill_MD5Key_ForPayOut;

        public int State = 0;   // 0 停止 1 运行中 2 置为停止
        private Log log = new Log("GetAlipayPayResultTask");

        public GetAlipayPayResultTask(string connectionstring)
        {
            ConnectionString = connectionstring;
        }


        public void Run()
        {
            // 已经启动
            if (State == 1)
            {
                return;
            }

            lock (this) // 确保临界区被一个 Thread 所占用
            {
                State = 1;

                gCount1 = 0;

                thread = new System.Threading.Thread(new System.Threading.ThreadStart(Do));
                thread.IsBackground = true;

                thread.Start();

                log.Write("GetAlipayPayResultTask Start.");

            }
        }

        public void Exit()
        {
            State = 2;
        }

        public void Do()
        {
            while (true)
            {
                if (State == 2)
                {
                    State = 0;
                    thread = null;

                    return;
                }

                System.Threading.Thread.Sleep(1000);   // 1秒为单位

                gCount1++;

                #region 3 小时, 下载派款结果分析结果

                //if (gCount1 >= (60 * 2))//测试@@@@@@@@@@@@@@@@@@@@
                if (gCount1 >= (60 * 60 * 3))//正式
                {
                    gCount1 = 0;

                    try
                    {
                        DownLoadResult();
                    }
                    catch { }
                }

                #endregion

            }
        }

        #region 定时器执行的事件

        private void DownLoadResult()	//下载结果
        {
            //业务参数赋值；
            string partner = OnlinePay_Alipay_ForUserDistill_UserNumber;        //partner		合作伙伴ID			保留字段
            string key = OnlinePay_Alipay_ForUserDistill_MD5Key_ForPayOut;      //partner账户的支付宝安全校验码
            string gateway = "https://www.alipay.com/cooperate/gateway.do";
            string service = "bptb_result_file";
            string sign = "";
            string _input_charset = "GB2312";                                                  //编码类型
            string biz_type = "d_sale";                                                        //业务类型

            Shove.Alipay.Alipay ap = new Shove.Alipay.Alipay();

            //获取今日之前的处理中的最大日期,按天分组
            DataTable dt = Shove.Database.MSSQL.Select(ConnectionString, " select max([DateTime]) as [DateTime] from T_UserDistillPaymentFiles where Result = 0 and datediff(Day,[DateTime],getdate())>0 group by convert(varchar(8),[DateTime],112)");

            if (dt == null)
            {
                //写日志
                log.Write("获取今日之前的处理中的最大日期,按天分组出错.");
                return;
            }

            foreach (DataRow dr in dt.Rows)
            {
                string fileName = "";
                DateTime time = Shove._Convert.StrToDateTime(dr["DateTime"].ToString(), DateTime.Now.ToShortDateString());

                fileName = "result" + time.ToString("yyyyMMdd");   //文件名的格式

                //string ContentType = "application/octet-stream";
                // 生成需要上传的二进制数组        
                CreateBytes cb = new CreateBytes();

                // 所有表单数据
                ArrayList bytesArray = new ArrayList();

                // 普通表单
                string[] Sortedstr = ap.GetDownloadParams
                    (
                    service,
                    _input_charset,
                    partner,
                    biz_type
                    );

                string[] downFields = new string[Sortedstr.Length + 1];
                int i = 0;

                char[] delimiterChars = { '=' };

                for (i = 0; i < Sortedstr.Length; i++)
                {
                    string fieldName = Sortedstr[i].Split(delimiterChars)[0];
                    string fieldValue = Sortedstr[i].Split(delimiterChars)[1];

                    bytesArray.Add(cb.CreateFieldData(fieldName, fieldValue));
                    downFields[i] = fieldName + "=" + fieldValue;
                }

                string date = time.ToString("yyyyMMdd");
                bytesArray.Add(cb.CreateFieldData("date", date));
                downFields[i] = "date" + "=" + date;
                sign = AlipayCommon.GetSign(downFields, key).Trim();
                bytesArray.Add(cb.CreateFieldData("sign", sign));
                bytesArray.Add(cb.CreateFieldData("sign_type", "MD5"));

                // 合成所有表单并生成二进制数组
                byte[] bytes = cb.JoinBytes(bytesArray);

                // 返回的内容
                byte[] responseBytes;

                // 上传到指定Url
                bool uploaded = false;

                try
                {
                    uploaded = cb.UploadData(gateway, bytes, out responseBytes);
                }
                catch (Exception ex)
                {
                    log.Write("cb.UploadData出错." + ex.Message);
                    continue;
                }

                if (!uploaded)
                {
                    log.Write("cb.UploadData失败.");
                    continue;
                }

                //PathName = System.AppDomain.CurrentDomain.BaseDirectory + "LogFiles\\" + pathname;
                string DownLoadFolder = System.AppDomain.CurrentDomain.BaseDirectory + "LogFiles\\GetAlipayPayResultTask\\RESULT\\";


                #region 查检下载目录是否存在,不存在则创建
                if (!Directory.Exists(DownLoadFolder))
                {
                    try
                    {
                        Directory.CreateDirectory(DownLoadFolder);
                    }
                    catch
                    {
                        log.Write("创建下载文件目录失败.");
                        return;
                    }
                }
                #endregion

                //判断responseByte是否以PK开头(以PK开头说明可以下载)
                if (responseBytes[0] != 80 && responseBytes[1] != 75)
                {
                    log.Write("判断responseByte不是以PK开头");
                    continue;
                }


                try
                {
                    if (File.Exists(DownLoadFolder + fileName + ".zip"))
                    {
                        File.Delete(DownLoadFolder + fileName + ".zip");
                    }

                    using (FileStream file = new FileStream(DownLoadFolder + fileName + ".zip", FileMode.Create, FileAccess.Write, FileShare.Read))
                    {
                        file.Write(responseBytes, 0, responseBytes.Length);
                    }

                    //解压下载的结果数据包
                    UnZip(DownLoadFolder + fileName + ".zip", DownLoadFolder + fileName);

                    //解压出来的结果文件
                    string strpath = DownLoadFolder + fileName + "\\ALIPAY__RESULT.csv";

                    //保存返回结果数据
                    DataTable dtResult = new DataTable("TableName");
                    int dtResultColCount = 0;

                    //保存返回结果中的文件明细表
                    DataTable dtResultFileDetail = new DataTable("dtResultFileDetail");
                    int dtResultFileDetailColCount = 0;


                    string strline;
                    string[] aryline;

                    System.IO.StreamReader mysr = new System.IO.StreamReader(strpath, Encoding.GetEncoding("GB2312"));
                    int resultFileRow = 0;

                    #region 构建返回结果表 和 返回结果中的文件列表数据表
                    while ((strline = mysr.ReadLine()) != null)
                    {
                        aryline = strline.Split(new char[] { ',' });

                        if (resultFileRow < 2) //构建返回结果表(2行,第1行为标题,第2行为对应的数据)   头部2行 
                        {
                            if (resultFileRow == 0)
                            {
                                dtResultColCount = aryline.Length;
                                for (int j = 0; j < aryline.Length; j++)
                                {
                                    DataColumn newCol = new DataColumn(aryline[j]);//付款日期,批次数,已处理批次个数,处理中批次个数
                                    dtResult.Columns.Add(newCol);
                                }
                            }

                            if (resultFileRow == 1)
                            {
                                DataRow newRow = dtResult.NewRow();
                                for (int j = 0; j < dtResultColCount; j++)
                                {
                                    newRow[j] = aryline[j];
                                }
                                dtResult.Rows.Add(newRow);
                            }
                        }
                        else  //构建文件列表数据表
                        {
                            if (resultFileRow == 2) //结果文件的行2:  批次名称,处理状态,处理成功笔数,处理失败笔数,成功付款总金额,失败付款总金额,处理日期,支付宝帐号,备注
                            {
                                dtResultFileDetailColCount = aryline.Length;
                                for (int j = 0; j < aryline.Length; j++)
                                {
                                    DataColumn newCol = new DataColumn(aryline[j]);
                                    dtResultFileDetail.Columns.Add(newCol);
                                }
                            }

                            if (resultFileRow >= 3) //获取:papx_20090416_p623225643.csv,已处理,2,0,2.00,0.00,20090417,53669955@qq.com,处理完成
                            {
                                DataRow newResultFileDetailRow = dtResultFileDetail.NewRow();
                                for (int j = 0; j < dtResultFileDetailColCount; j++)
                                {
                                    try
                                    {
                                        newResultFileDetailRow[j] = aryline[j];
                                    }
                                    catch
                                    {
                                        newResultFileDetailRow[j] = "";
                                    }
                                }
                                dtResultFileDetail.Rows.Add(newResultFileDetailRow);
                            }
                        }

                        resultFileRow++;
                    }
                    #endregion 结束  构建返回结果表 和 返回结果中的文件列表数据表


                    #region 循环处理返回结果中的文件列表的文件,以更新提款状态
                    foreach (DataRow curResultFileDetailRow in dtResultFileDetail.Rows)  //循环每个提交的文件
                    {
                        string CSVFile = curResultFileDetailRow[0].ToString(); //获取文件名
                        CSVFile = CSVFile.Substring(0, CSVFile.IndexOf(".")) + ".zip".ToLower();

                        if ((curResultFileDetailRow[1].ToString() == "已处理") && (curResultFileDetailRow[8].ToString() == "处理完成"))
                        {
                            string FilePath = DownLoadFolder + fileName + "\\" + curResultFileDetailRow[0].ToString();

                            DataTable dtCSVDetail = GetDetail(FilePath);    //获取当前处理的文件的提款记录明细数据

                            //循环处理当前CSV文件中的所有明细
                            foreach (DataRow drCSVDetail in dtCSVDetail.Rows)
                            {
                                if (drCSVDetail[9].ToString().ToLower() == "s")//成功标记
                                {

                                    //派款成功,更新状态标记
                                    long distillID = Shove._Convert.StrToLong(drCSVDetail[0].ToString(), -1);
                                    int returnValue = 0;
                                    string returnDescription = "";

                                    if (DAL.Procedures.P_UserDistillPayByAlipaySuccess(ConnectionString, 1, distillID, 0, ref returnValue, ref returnDescription) < 0)
                                    {
                                        log.Write("派款成功，P_UserDistillPayByAlipaySuccessAndUpdate状态更新失败.");
                                        continue;
                                    }
                                    if (returnValue < 0)
                                    {
                                        DAL.Procedures.P_UserDistillPayByAlipayWriteLog(ConnectionString, "出错!提款ID:" + distillID + "派款成功，但状态更新失败:" + returnDescription);
                                        continue;
                                    }


                                }
                                else
                                {
                                    //支付没有成功，返还会员提款的金额
                                    long distillID = Shove._Convert.StrToLong(drCSVDetail[0].ToString(), -1);
                                    string memo = "批量付款到银行卡失败:";//失败原因
                                    try
                                    {
                                        memo += drCSVDetail[12].ToString();
                                        memo = memo.Length > 50 ? memo.Substring(0, 50) : memo;
                                    }
                                    catch { }

                                    int returnValue = 0;
                                    string returnDescription = "";

                                    if (DAL.Procedures.P_UserDistillPayByAlipayUnsuccess(ConnectionString, 1, distillID, memo, ref returnValue, ref returnDescription) < 0)
                                    {
                                        log.Write("派款失败，P_UserDistillPayByAlipayUnsuccessAndQuash状态更新失败.");
                                        continue;
                                    }
                                    if (returnValue < 0)
                                    {
                                        log.Write("派款成功，但状态更新失败:" + returnDescription);
                                        DAL.Procedures.P_UserDistillPayByAlipayWriteLog(ConnectionString, "出错!提款ID:" + distillID + "派款成功，但状态更新失败:" + returnDescription);
                                        continue;
                                    }
                                }
                            }//end foreach (DataRow drCSVDetail in dtCSVDetail.Rows)


                            if (Shove.Database.MSSQL.ExecuteNonQuery(ConnectionString, "Update T_UserDistillPaymentFiles set [Result] = 1 where  Lower([FileName]) = '" + CSVFile + "'") < 0)
                            {
                                log.Write("更新T_UserDistillPaymentFiles出错.");
                            }
                        }//end if ((curResultFileDetailRow[1].ToString() == "已处理") && (curResultFileDetailRow[8].ToString() == "处理完成"))
                        else if (curResultFileDetailRow[1].ToString() == "处理中")
                        {
                            continue;
                        }
                        else
                        {
                            //文件格式错误的处理         
                            log.Write("文件格式错误的处理,未知处理结果状态.");

                        }
                    }//foreach (DataRow curResultFileDetailRow in dtResultFileDetail.Rows)
                    #endregion 循环处理返回结果中的文件列表的文件,以更新提款状态

                }
                catch (Exception ex)
                {
                    log.Write("未知异常3:" + ex.Message);
                }
            } //end foreach (DataRow dr in dt.Rows)

        }

        ///   <summary>   
        ///   解压缩   
        ///   </summary>   
        ///   <param   name="FilePath"></param>   
        private void UnZip(string FilePath, string OutputFolder)
        {
            if (!File.Exists(FilePath))
            {
                throw new FileNotFoundException("File:" + FilePath + "   Not   Found.");
            }

            FileInfo fi = new FileInfo(FilePath);
            using (ZipInputStream stream = new ZipInputStream(fi.OpenRead()))
            {
                string FolderName = null;

                if (string.IsNullOrEmpty(OutputFolder))
                {
                    FolderName = fi.FullName.Replace(fi.Extension, string.Empty);
                }
                else
                {
                    FolderName = OutputFolder;  // fi.Name;
                }

                //首先为该文件创建一个解压缩到的目录   
                Directory.CreateDirectory(FolderName);

                ZipEntry FileName = null;
                while ((FileName = stream.GetNextEntry()) != null)
                {
                    int size = 2048;
                    byte[] data = new byte[2048];

                    string[] s = FileName.Name.Split('\\');
                    if (s.Length > 1)
                    {
                        StringBuilder sb = new StringBuilder(FolderName);

                        int i = 0;
                        while (i < s.Length - 1)
                        {
                            sb.Append('\\');
                            sb.Append(s[i++]);
                        }

                        Directory.CreateDirectory(sb.ToString());
                    }

                    string outfile = FolderName + "\\" + FileName.Name;

                    using (FileStream fs = new FileStream(outfile, FileMode.Create))
                    {

                        while (true)
                        {
                            size = stream.Read(data, 0, data.Length);

                            if (size > 0)
                            {
                                fs.Write(data, 0, size);
                            }
                            else
                            {
                                break;
                            }
                        }

                        fs.Flush();
                        fs.Close();
                    }
                }
            }
        }

        /// <summary>
        /// 获取CSV的数据
        /// </summary>
        /// <param name="FilePath"></param>
        /// <returns></returns>
        private DataTable GetDetail(string FilePath)
        {
            int intColCount = 0;
            //bool blnFlag = true;
            bool blnFlagDtDetail = true;
            DataTable dtResultCSV = new DataTable("dtCSV");

            //DataColumn dcResultCSV;
            //DataRow drResultCSV;

            DataTable dtDetailCSV = new DataTable("dtCSVDetail");

            DataColumn dcDetailCSV;
            DataRow drDetailCSV;

            string strline;
            string[] aryline;

            System.IO.StreamReader mysr = new System.IO.StreamReader(FilePath, Encoding.GetEncoding("GB2312"));

            int y = 0;
            while ((strline = mysr.ReadLine()) != null)
            {
                aryline = strline.Split(new char[] { ',' });

                if (y < 2)
                {
                }
                else
                {
                    if (blnFlagDtDetail)
                    {
                        blnFlagDtDetail = false;
                        intColCount = aryline.Length;
                        for (int j = 0; j < aryline.Length; j++)
                        {
                            dcDetailCSV = new DataColumn(aryline[j]);
                            dtDetailCSV.Columns.Add(dcDetailCSV);
                        }
                    }

                    if (y != 2)
                    {
                        drDetailCSV = dtDetailCSV.NewRow();
                        for (int j = 0; j < intColCount; j++)
                        {
                            try
                            {
                                drDetailCSV[j] = aryline[j];
                            }
                            catch
                            {
                                drDetailCSV[j] = "";
                            }
                        }
                        dtDetailCSV.Rows.Add(drDetailCSV);
                    }
                }

                y++;
            }

            return dtDetailCSV;
        }

        #endregion
    }
}
