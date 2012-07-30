using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using System.IO;
using Shove.Alipay;

public partial class Admin_UserDistillWaitPay : AdminPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            BindBankType();
            //初始化界面
            hfCurPayType.Value = "支付宝";
            btnAlipayToAlipayAdd.Visible = true;
            btnAlipayToBank.Visible = false;
            PayByAlipay.Attributes["class"] = "SelectedTab";
            PayByBank.Attributes["class"] = "NotSelectedTab";
            AllPay.Attributes["class"] = "NotSelectedTab";
            BindData(true);
        }
    }

    #region Web 窗体设计器生成的代码

    override protected void OnInit(EventArgs e)
    {
        isRequestLogin = true;
        RequestLoginPage = this.Request.Url.AbsoluteUri;

        RequestCompetences = Competences.BuildCompetencesList(Competences.Finance);

        base.OnInit(e);
    }

    #endregion

    private void BindBankType()
    {
        string cacheKeyBankType = "Admin_UserDistillWaitPay_BankType";
        DataTable dt ;
        dt = Shove._Web.Cache.GetCacheAsDataTable(cacheKeyBankType);
        if (dt == null)
        {
            dt = (new DAL.Tables.T_Banks()).Open("", "", "[Name]");
        }
        ddlAccountType.DataSource = dt;
        ddlAccountType.DataTextField = "Name";
        ddlAccountType.DataValueField = "Name";
        ddlAccountType.DataBind();

        ddlAccountType.Items.Insert(0,"");
        ddlAccountType.Items.Add("支付宝");
    }

    private void BindData(bool IsReload)
    {
        DataTable dt;

        string cacheKey = "Admin_UserDistillWaitPay_" + tbBeginTime.Text + "_" + tbEndTime.Text;
        dt = Shove._Web.Cache.GetCacheAsDataTable(cacheKey);
        if (dt == null || IsReload)
        {
            dt = new DAL.Views.V_UserDistills().Open("", "Result=0 and SiteID = " + _Site.ID.ToString(), "[DateTime] desc");
            Shove._Web.Cache.SetCache(cacheKey, dt);
        }
        if (dt == null)
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", "Admin_FinanceDistill");
            return;
        }

        string filterCondition = GetFilterCondition();
        DataView dv = new DataView(dt, filterCondition, "[DateTime] desc", DataViewRowState.OriginalRows);
        PF.DataGridBindData(g, dv, gPager);

        //根据条件显示或隐藏列
        for(int i=0;i<g.Columns.Count;i++)
        {
            if (g.Columns[i].HeaderText == "提款银行卡帐号" || g.Columns[i].HeaderText == "开户银行" || g.Columns[i].HeaderText == "支行名称"
                || g.Columns[i].HeaderText == "所在省" || g.Columns[i].HeaderText == "所在市" || g.Columns[i].HeaderText == "持卡人姓名")
            {
                g.Columns[i].Visible = (hfCurPayType.Value == "所有" || hfCurPayType.Value == "银行卡") ? true : false;
            }
            else if (g.Columns[i].HeaderText == "支付宝账号")
            {
                g.Columns[i].Visible = true;
                g.Columns[i].Visible = (hfCurPayType.Value == "所有" || hfCurPayType.Value == "支付宝") ? true : false;
            }
        }
    }

    private string GetFilterCondition()
    {
        string condiction = " Result=0 ";
        if (tbBeginTime.Text.Trim() != "" && tbEndTime.Text.Trim() != "")
        {
            DateTime fromDate = DateTime.Now;
            DateTime toDate = DateTime.Now;
            if (DateTime.TryParse(tbBeginTime.Text, out fromDate) && DateTime.TryParse(tbEndTime.Text, out toDate))
            {
                condiction += " and ( DateTime >= '" + fromDate.ToString("yyyy-MM-dd") + "' and DateTime <= '" + toDate.ToString("yyyy-MM-dd") + " 23:59:59' )";
            }
            else
            {
                Shove._Web.JavaScript.Alert(this.Page, "请输入正确的日期格式:2008-8-8");
            }
        }

        if (hfCurPayType.Value == "银行卡")//提款到银行卡
        {
            condiction += " and DistillType =2 ";
        }
        else if (hfCurPayType.Value == "支付宝")//支付宝
        {
            condiction += " and DistillType =1 ";
        }

        if (tbUserName.Text.Trim() != "")
        {
            condiction += " and Name='" + Shove._Web.Utility.FilteSqlInfusion(tbUserName.Text.Trim()) + "' ";
        }

        if (ddlAccountType.Text.Trim() != "")
        {
            if (ddlAccountType.Text != "支付宝")
            {
                condiction += " and BankName like '%" + ddlAccountType.Text + "%'";//银行类型
            }
            else
            {
                condiction += " and BankName ='' ";//支付宝
            }
        }

        return condiction;
    }

    protected void g_ItemDataBound(object sender, DataGridItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.EditItem)
        {
            DataRowView view = ((DataRowView)e.Item.DataItem);
            TextBox tbMemo = (TextBox)e.Item.FindControl("tbMemo");
            tbMemo.Text = view["Memo"].ToString();
        }
    }

    protected void gPager_PageWillChange(object Sender, Shove.Web.UI.PageChangeEventArgs e)
    {
        BindData(false);
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        if (tbBeginTime.Text.Trim() != "" && tbEndTime.Text.Trim() != "")
        {
            DateTime fromDate = DateTime.Now;
            DateTime toDate = DateTime.Now;
            if (!DateTime.TryParse(tbBeginTime.Text, out fromDate) && !DateTime.TryParse(tbEndTime.Text, out toDate))
            {
                Shove._Web.JavaScript.Alert(this.Page, "请输入正确的日期格式:2008-8-8");
                return;
            }
        }

        BindData(false);
    }
    protected void g_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        long distillID = Shove._Convert.StrToLong(g.DataKeys[e.Item.ItemIndex].ToString(), -1);
        long distillUserID = Shove._Convert.StrToLong(e.Item.Cells[1].Text, -1);
        if (e.CommandName == "Pay")//确认已经线下付款
        {
            int results = -1;
            int returnValue=0;
            string  returnDescription="";
            results = DAL.Procedures.P_DistillPaySuccess(_Site.ID, distillUserID, distillID, "付款成功", _User.ID, ref returnValue, ref returnDescription);
            if (results < 0)
            {
                Shove._Web.JavaScript.Alert(this.Page, "出错!请联技术人员!");
                return;
            }
            if (returnValue < 0)
            {
                Shove._Web.JavaScript.Alert(this.Page, "出错!请联技术人员:" + returnDescription);
                return;
            }

            BindData(true);
            Shove._Web.JavaScript.Alert(this.Page, "操作成功.");
        }
        else if (e.CommandName == "DistillNoAccept") //拒绝提款
        {
            string memo = ((TextBox)e.Item.FindControl("tbMemo")).Text.Trim();
            if (memo=="" || memo.IndexOf("接受提款") > 0)
            {
                memo = "提款资料不完整";
            }

            int results = -1;
            int returnValue = 0;
            string returnDescription = "";
            results = DAL.Procedures.P_DistillNoAccept(_Site.ID, distillUserID, distillID, memo, _User.ID, ref returnValue, ref returnDescription);
            if (results < 0)
            {
                Shove._Web.JavaScript.Alert(this.Page, "出错!请联技术人员!");
                return;
            }
            if (returnValue < 0)
            {
                Shove._Web.JavaScript.Alert(this.Page, "出错!请联技术人员:" + returnDescription);
                return;
            }

            BindData(true);
            Shove._Web.JavaScript.Alert(this.Page, "操作成功!该笔提款已拒绝提款.");
        }
        else if (e.CommandName == "EditMemo")
        {
            string memo=Shove._Web.Utility.FilteSqlInfusion(((TextBox)e.Item.FindControl("tbMemo")).Text.Trim());
            if (memo == "" || memo.IndexOf("接受提款") > 0)
            {
                memo = "提款资料不完整";
            }
            memo = memo.Replace("'", "\"");
            Shove.Database.MSSQL.ExecuteNonQuery(" update T_UserDistills set [Memo] ='" + memo + "' where [ID] = " + distillID.ToString(), new Shove.Database.MSSQL.Parameter[0]);
        }
    }

    protected void lbtnPayByAlipay_Click(object sender, EventArgs e)
    {
        hfCurPayType.Value = "支付宝";
        btnAlipayToAlipayAdd.Visible = true;
        btnAlipayToBank.Visible = false;

        BindData(false);

        PayByAlipay.Attributes["class"] = "SelectedTab";
        PayByBank.Attributes["class"] = "NotSelectedTab";
        AllPay.Attributes["class"] = "NotSelectedTab";
    }

    protected void lbtnPayByBank_Click(object sender, EventArgs e)
    {
        hfCurPayType.Value = "银行卡";
        btnAlipayToAlipayAdd.Visible = false;
        btnAlipayToBank.Visible = true;
        BindData(false);

        PayByAlipay.Attributes["class"] = "NotSelectedTab";
        PayByBank.Attributes["class"] = "SelectedTab";
        AllPay.Attributes["class"] = "NotSelectedTab";
    }
    protected void lbtnAllPay_Click(object sender, EventArgs e)
    {
        hfCurPayType.Value = "所有";
        btnAlipayToAlipayAdd.Visible = true;
        btnAlipayToBank.Visible = true;
        BindData(false);

        PayByAlipay.Attributes["class"] = "NotSelectedTab";
        PayByBank.Attributes["class"] = "NotSelectedTab";
        AllPay.Attributes["class"] = "SelectedTab";
    }
    protected void ddlAccountType_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindData(false);
    }
    //--------------------------------------------------------------------------------------------------------------------------------------------------------------------
    protected string gateway = "";
    protected string service = "";
    protected string partner = "";
    protected string sign_type = "";
    protected string _input_charset = "";                                                     //编码类型
    protected string digest_bptb_pay_file = "";                                               //文件摘要
    protected string file_digest_type = "";                                                   //文件摘要算法
    protected string biz_type = "";                                                           //业务类型
    protected string key = "";
    protected string agentID = "";                                                            //代理商ID
    protected string sign = "";                                                               //签名
    protected string bptb_pay_file = "";                                                      //上传的文件地址
    protected string Url = "";

    protected void btnAlipayToBank_Click(object sender, EventArgs e)
    {
        int CheckedCount = 0;
        double SumMoney = 0;
        string Body = "";

        string UserID = ""; 
        string DistillID = "";
        string BankUserName = "";
        string BankCardNumber = "";
        string BankTypeName = "";
        string BankInProvince = "";
        string BankInCity = "";
        string BankName = "";
        
        double Money = 0;
        //string Memo = "";

        string UserDistillIDs = "";

        for (int i = 0; i < g.Items.Count; i++)
        {
            CheckBox chkSelect = (CheckBox)g.Items[i].FindControl("chkSelect");

            if (chkSelect != null && chkSelect.Checked)
            {
                UserID = GetDataGridCellValue(g, i, "UserID");
                DistillID = GetDataGridCellValue(g, i, "ID");
                BankUserName = GetDataGridCellValue(g, i, "BankUserName");
                BankCardNumber = GetDataGridCellValue(g, i, "BankCardNumber");
                BankTypeName = GetDataGridCellValue(g, i, "BankTypeName");
                BankInProvince = GetDataGridCellValue(g, i, "BankInProvince");
                BankInCity = GetDataGridCellValue(g, i, "BankInCity");
                BankName = GetDataGridCellValue(g, i, "BankName");
                
                Money = Convert.ToDouble(GetDataGridCellValue(g, i, "Money")) - Shove._Convert.StrToDouble(GetDataGridCellValue(g, i, "FormalitiesFees"), 0); //扣除手续费
              
                //商户流水号,收款银行户名,收款银行帐号,收款开户银行,收款银行所在省份,收款银行所在市,收款支行名称,金额,对公对私标志,备注
                //2007052801,韦吴石,6221885511000609058,工商银行,广东省,深圳市,工商银行深圳宝安分行,10,2,2132

                Body += DistillID + "," + BankUserName + "," + BankCardNumber + "," + BankTypeName + "," + BankInProvince + "," + BankInCity + "," + BankName + "," + Money.ToString() + ",2,会员提款\r\n";

                if (UserDistillIDs == "")
                {
                    UserDistillIDs = DistillID;
                }
                else
                {
                    UserDistillIDs += "," + DistillID;
                }

                SumMoney += Money;
                CheckedCount++;
            }
        }

        if ((Body != "") && (CheckedCount > 0) && (SumMoney > 0))
        {
            OnlinePaymentAlipayToBank(CheckedCount, SumMoney, Body, UserDistillIDs);
        }
        else
        {
            Shove._Web.JavaScript.Alert(this, "您没有选中任何选项。");

            return;
        }
    }


    protected void btnAlipayToAlipayAdd_Click(object sender, EventArgs e)
    {
        int CheckedCount = 0;
        double SumMoney = 0;
        string Body = "";

        int i = 0;

        string DistillID = "";
        string AlipayName = "";
        string RealityName = "";
        double Money = 0;
        string Memo = "";
        string UserDistillIDs = "";

        for (i = 0; i < g.Items.Count; i++)
        {
            CheckBox chkBox = (CheckBox)g.Items[i].FindControl("chkSelect");

            if (chkBox != null && chkBox.Checked)
            {
                //newRow["DistillID"] = rowView["ID"];
                //newRow["AlipayName"] = rowView["AlipayName"];
                //newRow["RealityName"] = rowView["RealityName"];
                //newRow["Money"] = rowView["Money"];
                //newRow["Memo"] = "提款";

                DistillID = GetDataGridCellValue(g, i, "ID");
                AlipayName = GetDataGridCellValue(g, i, "AlipayName");
                RealityName = GetDataGridCellValue(g, i, "RealityName");
                Money = Convert.ToDouble(GetDataGridCellValue(g, i, "Money")) - Shove._Convert.StrToDouble(GetDataGridCellValue(g, i, "FormalitiesFees"),0); //扣除手续费
                Memo = "会员提款";

                if (Body != "")
                {
                    //注意格式多条用"|" 隔开 //"0039411^92781@qq.com^周长军^0.01^test|0039491^550833185@qq.com^谢凯^0.01^263"
                                    //流水号(提款ID) //支付宝帐号    //收款人真实姓名         //Money                   /备注
                    Body += "|" + DistillID + "^" + AlipayName + "^" + RealityName + "^" + Money + "^"+Memo;
                }
                else
                {
                    Body +=  DistillID + "^" + AlipayName + "^" + RealityName + "^" + Money + "^" + Memo;
                }

                if (UserDistillIDs == "")
                {
                    UserDistillIDs = DistillID;
                }
                else
                {
                    UserDistillIDs += "," + DistillID;
                }
                
                SumMoney += Money;
                CheckedCount++;
            }
        }

        if (CheckedCount > 0)
        {
            OnlinePaymentAlipayToAlipay(CheckedCount, SumMoney, Body, UserDistillIDs);
        }
        else
        {
            Shove._Web.JavaScript.Alert(this, "您没有选中任何选项。");

            return;
        }
    }
    protected void OnlinePaymentAlipayToAlipay(int Count, double SumMoney, string Body, string UserDistillIDs)
    {
        //OnlinePay_Alipay_ForUserDistill_MD5Key
        //OnlinePay_Alipay_ForUserDistill_MD5Key_ForPayOut
        //OnlinePay_Alipay_ForUserDistill_Status_ON
        //OnlinePay_Alipay_ForUserDistill_UserName
        //OnlinePay_Alipay_ForUserDistill_UserNumber
        //业务参数赋值；
        SystemOptions sysOptions = new SystemOptions();
        partner = sysOptions["OnlinePay_Alipay_ForUserDistill_UserNumber"].ToString("");		//partner		合作伙伴ID			保留字段
        key = sysOptions["OnlinePay_Alipay_ForUserDistill_MD5Key_ForPayOut"].ToString("");            //partner账户的支付宝安全校验码
        //**************e******************************************
        string email = sysOptions["OnlinePay_Alipay_ForUserDistill_UserName"].ToString("");         //付款人账号

        //未知@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
        string account_name = "深圳天维掌通技术有限公司"; //PublicFunction.GetOptionsAsString("OnlinePayOut_Alipay_RealityName", "");		//	账户真实姓名 

        string gateway = "https://www.alipay.com/cooperate/gateway.do?";//'支付接口'支付接口
        string _input_charset = "utf-8";
        string service = "batch_trans_notify";
        string sign_type = "MD5";

        DateTime dtNow = DateTime.Now;
        string pay_date = dtNow.ToString("yyyyMMdd");   // 付款日期   //注意格式

        //*******************************************************
        System.Random rad = new Random();
        string Num = rad.Next(1, 99).ToString();
        Num = Num.PadLeft(2, '0');
        string batch_no = dtNow.ToString("yyyyMMddhhmmss") + Num;	    //批量付款订单号 日期(20070412)+16位序列号
        string batch_fee = SumMoney.ToString();                         //总金额					0.01～50000.00
        string batch_num = Count.ToString();                            //批次号即该次付款总笔数
        //*********************************************************
        string detail_data = Body; //注意格式多条用"|" 隔开 //"0039411^92781@qq.com^周长军^0.01^test|0039491^550833185@qq.com^谢凯^0.01^263"
        string notify_url = Shove._Web.Utility.GetUrl() + "/Admin/OnlinePayment/Alipay/AlipayNotify.aspx"; //服务器通知返回接口

        //把上传的支付明细写入数据库（FileName）和更新状态
        int returnValue = 0;
        string returnDecription = "";
        if (DAL.Procedures.P_UserDistillPayByAlipay(_User.ID, batch_no, UserDistillIDs, 1, ref returnValue, ref returnDecription) < 0)
        {
            DAL.Procedures.P_UserDistillPayByAlipayWriteLog("提款ID:" + UserDistillIDs + "写入数据库（FileName）和更新状态失败。");
        }
        if (returnDecription != "")
        {
            Shove._Web.JavaScript.Alert(this.Page, "数据更新错误:" + returnDecription);
        }

        Shove.Alipay.Alipay ap = new Shove.Alipay.Alipay();
        string aliay_url = ap.CreatUrl(
             gateway,
             service,
             partner,
             sign_type,
             batch_no,
             account_name,
             batch_fee,
             batch_num,
             email,
             pay_date,
             detail_data,
             key,
             notify_url,
             _input_charset
            );

        //测试到此，不上传@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
        //Shove._Web.JavaScript.Alert(this.Page, "测试到此，不上传");
        //return;
        Response.Redirect(aliay_url, true);                                        //可以采用表单（post）传递数据
        //this.Response.Write("<script language='javascript'>window.top.location.href='" + aliay_url + "'</script>");

        return;
    }


    protected void OnlinePaymentAlipayToBank(int Count, double SumMoney, string Body, string UserDistillIDs)
    {
        //业务参数赋值；
        SystemOptions sysOptions=new SystemOptions();
        partner = sysOptions["OnlinePay_Alipay_ForUserDistill_UserNumber"].ToString("");//OnlinePayOut_Alipay_UserNumber		//partner		合作伙伴ID			保留字段
        key = sysOptions["OnlinePay_Alipay_ForUserDistill_MD5Key"].ToString("");   //OnlinePayOut_Alipay_MD5Key //partner账户的支付宝安全校验码

        gateway = sysOptions["MemberSharing_Alipay_Gateway"].ToString("");  //"https://www.alipay.com/cooperate/gateway.do";
        service = "bptb_pay_file";
        sign_type = "MD5";
        _input_charset = "GB2312";                                                  //编码类型
        file_digest_type = "MD5";                                                   //文件摘要算法
        biz_type = "d_sale";                                                        //业务类型

        agentID = sysOptions["OnlinePay_Alipay_ForUserDistill_UserNumber"].ToString("");// "2088001456282873";

        Shove.Alipay.Alipay ap = new Shove.Alipay.Alipay();

        DateTime dtNow = DateTime.Now;

        string FileName = "";
        string FileAddr = "";
        string ZipFileName = "";
        string ZipFileAddr = "";

        int i = 0;

        if ((Body != "") && (Count > 0) && (SumMoney > 0))
        {
            string strDateTime = dtNow.ToString("yyyyMMdd");

            #region     CSV文件标准格式

            //日期,总金额,总笔数,支付宝帐号(Email)
            //20080418,10,1,53669955@qq.com
            //商户流水号,收款银行户名,收款银行帐号,收款开户银行,收款银行所在省份,收款银行所在市,收款支行名称,金额,对公对私标志,备注
            //2007052801,韦吴石,6221885511000609058,工商银行,广东省,深圳市,工商银行深圳宝安分行,10,2,2132

            #endregion
            
            string headStr = "日期,总金额,总笔数,支付宝帐号(Email)\r\n" ;
            headStr += strDateTime + "," + SumMoney.ToString() + "," + Count.ToString() + "," + partner + "\r\n";
            headStr += "商户流水号,收款银行户名,收款银行帐号,收款开户银行,收款银行所在省份,收款银行所在市,收款支行名称,金额,对公对私标志,备注\r\n";
            Body = headStr + Body;

            System.Random rad = new Random();
            string Num = rad.Next(1, 999).ToString();
            Num = Num + dtNow.Hour.ToString() + dtNow.Minute.ToString() + dtNow.Second.ToString();
            Num = Num.PadLeft(9, '0');

            FileName = "PAPX_" + strDateTime + "_P" + Num + ".csv".ToLower();
            FileAddr = MapPath("../App_Log/Admin/AlipayPayment/PABX/" + FileName).ToLower();
            ZipFileName = "PAPX_" + strDateTime + "_P" + Num + ".Zip".ToLower();
            ZipFileAddr = MapPath("../App_Log/Admin/AlipayPayment/PABX/" + ZipFileName).ToLower();

            if (!System.IO.File.Exists(FileAddr))
            {
                try
                {
                    //File.WriteAllText(FileAddr, Body);
                    if (!Shove._IO.File.WriteFile(FileAddr, Body))
                    {
                        DAL.Procedures.P_UserDistillPayByAlipayWriteLog("CSV文件写入失败:" + FileAddr);
                        Shove._Web.JavaScript.Alert(this.Page, "CSV文件写入失败");

                        return;
                    }
                }
                catch
                {
                    DAL.Procedures.P_UserDistillPayByAlipayWriteLog("CSV文件写入异常:" + FileAddr);
                    Shove._Web.JavaScript.Alert(this.Page, "CSV文件写入异常");

                    return;
                }
            }
            else
            {
                Shove._Web.JavaScript.Alert(this.Page, "文件写已存在");

                return;
            }

            if (System.IO.File.Exists(FileAddr))
            {
                if (!System.IO.File.Exists(ZipFileAddr))
                {
                    try
                    {
                        Shove._IO.File.Compress(FileAddr, ZipFileAddr);
                    }
                    catch
                    {
                        DAL.Procedures.P_UserDistillPayByAlipayWriteLog("文件压缩出现异常:" + FileAddr);
                        Shove._Web.JavaScript.Alert(this.Page, "文件压缩出现异常");
                        return;
                    }
                }
            }
            else
            {
                DAL.Procedures.P_UserDistillPayByAlipayWriteLog("CSV文件不存在(1):" + FileAddr);
                Shove._Web.JavaScript.Alert(this.Page, "CSV文件不存在");

                return;
            }
        }

        if (!System.IO.File.Exists(ZipFileAddr))
        {
            DAL.Procedures.P_UserDistillPayByAlipayWriteLog("ZIP文件不存在(2):" + FileAddr);
            Shove._Web.JavaScript.Alert(this.Page, "ZIP文件不存在");

            return;
        }

        //测试到此，不上传@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
        //Shove._Web.JavaScript.Alert(this.Page, "测试到此，不上传");
        //return;

        byte[] Date = System.IO.File.ReadAllBytes(ZipFileAddr);
        digest_bptb_pay_file = Shove.Alipay.Alipay.GetMD5(Date, _input_charset);

        string[] Sortedstr = ap.GetUploadParams
            (
            service,
            _input_charset,
            partner,
            file_digest_type,
            biz_type,
            agentID
            );                                                                      //签名

        // 读文件流
        FileStream fs = new FileStream(ZipFileAddr, FileMode.Open, FileAccess.Read, FileShare.Read);

        // 这部分需要完善
        string ContentType = "application/octet-stream";
        byte[] fileBytes = new byte[fs.Length];
        fs.Read(fileBytes, 0, Convert.ToInt32(fs.Length));
        fs.Close();

        // 生成需要上传的二进制数组
        CreateBytes cb = new Shove.Alipay.CreateBytes();
        // 所有表单数据
        ArrayList bytesArray = new ArrayList();
        // 普通表单
        string[] fields = new string[Sortedstr.Length + 1];

        char[] delimiterChars = { '=' };

        for (i = 0; i < Sortedstr.Length; i++)
        {
            string fieldName = Sortedstr[i].Split(delimiterChars)[0];
            string fieldValue = Sortedstr[i].Split(delimiterChars)[1];

            bytesArray.Add(cb.CreateFieldData(fieldName, fieldValue));
            fields[i] = fieldName + "=" + fieldValue;
        }

        bytesArray.Add(cb.CreateFieldData("digest_bptb_pay_file", digest_bptb_pay_file));
        fields[i] = "digest_bptb_pay_file=" + digest_bptb_pay_file;
        sign = AlipayCommon.GetSign(fields, key).Trim();
        bytesArray.Add(cb.CreateFieldData("sign", sign));
        bytesArray.Add(cb.CreateFieldData("sign_type", "MD5"));

        // 文件表单
        bytesArray.Add(cb.CreateFieldData("bptb_pay_file", ZipFileAddr, ContentType, fileBytes));

        // 合成所有表单并生成二进制数组
        byte[] bytes = cb.JoinBytes(bytesArray);

        // 返回的内容
        byte[] responseBytes;
        // 上传到指定Url        
        bool uploaded = cb.UploadData(gateway, bytes, out responseBytes);
        if (!uploaded)
        {
            DAL.Procedures.P_UserDistillPayByAlipayWriteLog("上传到指定Url失败:" + FileAddr);
            try
            {
                File.Delete(FileAddr);
                File.Delete(ZipFileAddr);
            }
            catch
            {
            }
            Shove._Web.JavaScript.Alert(this.Page, "上传支付数据到指定Url失败!");

            return;
        }

        string UploadResult = System.Text.Encoding.Default.GetString(responseBytes);

        if (UploadResult.IndexOf("上传成功") >= 0)
        {
            //把上传的支付明细写入数据库（FileName）和更新状态
            int returnValue = 0;
            string returnDecription = "";
            if (DAL.Procedures.P_UserDistillPayByAlipay(_User.ID, ZipFileName, UserDistillIDs, 2, ref returnValue, ref returnDecription) < 0)
            {
                DAL.Procedures.P_UserDistillPayByAlipayWriteLog("提款ID:" + UserDistillIDs + "写入数据库（FileName）和更新状态失败。");
            }
            if (returnDecription != "")
            {
                DAL.Procedures.P_UserDistillPayByAlipayWriteLog("把上传的支付明细写入数据库（FileName）和更新状态出错:" + returnDecription);
                Shove._Web.JavaScript.Alert(this.Page, "出错:" + returnDecription);
                BindData(true);
                return;
            }
            Shove._Web.JavaScript.Alert(this.Page, UploadResult + "银行处理需要到明天才有结果,请耐心等待处理结果！");

            BindData(true);
        }
        else
        {
            DAL.Procedures.P_UserDistillPayByAlipayWriteLog("上传支付到银行明细失败:" + UploadResult);
            Shove._Web.JavaScript.Alert(this.Page, UploadResult.Replace("\n","").Replace("\r",""));
        }
    }


    protected void btnOKAll_Click(object sender, EventArgs e)
    {
       
    }

    private string  GetDataGridCellValue(DataGrid g,int ItemIndex, string dataField)
    {
        bool isFind=false;
        string  returnValue = "";
        for(int i=0;i<g.Columns.Count;i++)
        {
            BoundColumn boundColumn= g.Columns[i] as BoundColumn;
            if (boundColumn != null)
            {
                if (boundColumn.DataField.ToLower() == dataField.ToLower())
                {
                    isFind = true;
                    returnValue = g.Items[ItemIndex].Cells[i].Text;
                    break;
                }
            }
        }
        if (!isFind)
        {
            //Shove._Web.JavaScript.Alert(this.Page, "GetDataGridCellValue 找不到指定的值");
            PF.GoError(-111, "找不到指定的列值,请联系技术员", this.GetType().Name);
        }
        return returnValue;
    }
}
